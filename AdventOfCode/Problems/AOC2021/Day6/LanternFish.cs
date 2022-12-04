using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2021.Day6;

[ProblemInfo(2021, 6, "Lanternfish")]
internal class LanternFish : Problem<long, long>
{
	public int[] Data { get; private set; } = Array.Empty<int>();

	public override void LoadInput()
	{
		var input = ReadInputLines();
		Data = input.First().Split(",").Select(v => int.Parse(v)).ToArray();
	}
	public override void CalculatePart1()
	{
		var lifetimes = PrepareLifetimes();
		Part1 = Simulate(lifetimes, 80);

	}

	public override void CalculatePart2()
	{
		var lifetimes = PrepareLifetimes();
		Part2 = Simulate(lifetimes, 256);
	}

	private long[] PrepareLifetimes()
	{
		var lifetimes = new long[9];
		foreach (var life in Data)
			lifetimes[life] += 1;

		return lifetimes;
	}

	private static long Simulate(long[] lifetimes, int days)
	{
		for (int i = 0; i < days; i++)
		{
			var day0Count = lifetimes[0];
			for (int j = 1; j < lifetimes.Length; j++)
				lifetimes[j - 1] = lifetimes[j];
			lifetimes[6] += day0Count;
			lifetimes[^1] = day0Count;
		}

		return lifetimes.Sum();
	}




}
