using System;
using System.Collections.Generic;
using System.Text;

using ZLinq;

namespace AdventOfCode.Problems.AOC2025.Day3;

[ProblemInfo(2025, 3, "Lobby")]
internal class Lobby : Problem<long, long>
{
	private (int val, int idx)[][] _batteryBanks = [];

	public override void CalculatePart1()
	{
		Part1 = _batteryBanks.AsValueEnumerable().Select(bank =>
		{
			var batteries = GetViableBatteries(bank);
			return GetPower(batteries);
		}).Sum();
	}


	public override void CalculatePart2()
	{
		Console.WriteLine();
		var b = _batteryBanks.AsValueEnumerable().Select(bank =>
		{
			var batteries = GetViableBatteries(bank, 12);
			return GetPower(batteries);
		});
		Part2 = b.Sum();
	}

	public static long GetPower(int[] values)
	{
		return values.Select((v, idx) =>
		{
			var mag = (long)Math.Pow(10, values.Length - idx - 1);
			return v * mag;
		}).Sum();
	}

	public static int[] GetViableBatteries((int val, int idx)[] source, int count = 2)
	{
		var batteries = new int[count];
		var offset = 0;
		for (int i = count; i > 0; i--)
		{
			var tgt = i - 1;
			var (val, idx) = source[offset..^tgt].MaxBy(v => v.val);
			offset = idx + 1;
			batteries[count - i] = val;
		}
		return batteries;
	}

	public override void LoadInput()
	{
		_batteryBanks = ReadInputLines("input.txt")
			.AsValueEnumerable()
			.Select(l => l.AsValueEnumerable().Select((v, idx) => (v - '0', idx)).ToArray())
			.ToArray();
	}
}
