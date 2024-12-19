using Superpower.Model;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Problems.AOC2024.Day11;

[ProblemInfo(2024, 11, "Plutonian Pebbles")]
public class PlutonianPebbles : Problem<long, long>
{
	private List<long> _data = [];

	public override void CalculatePart1()
	{
		Part1 = Run(25);
	}

	public override void CalculatePart2()
	{
		//Runs into max array length limit
		Part2 = Run(75);
	}

	public long Run(long count)
	{
		var a = _data.ToList();
		var b = new List<long>(a.Count);
		for (long i = 0; i < count; i++)
		{
			foreach (var stone in a)
				ProcessStone(stone, b);

			(a, b) = (b, a);
			b.Clear();
		}
		return a.Count;
	}

	public static void ProcessStone(long stone, List<long> data)
	{
		if (stone == 0)
		{
			data.Add(1);
			return;
		}
		if (FastSplit(stone, out var left, out var right))
		{
			data.Add(left);
			data.Add(right);
			return;
		}
		data.Add(stone * 2024);
	}

	private static IEnumerable<long> Split(long stone, int len)
	{
		var v = stone.ToString();
		return [long.Parse(v[..(len / 2)]), long.Parse(v[(len / 2)..])];
	}

	private static bool FastSplit(long stone, out long left, out long right)
	{
		var len = stone.DigitCount();
		if (len % 2 != 0)
		{
			left = 0;
			right = 0;
			return false;
		}
		var l = QuickMath.FastPow10(len / 2);
		var a = stone / l;

		(left, right) = (a, stone - (a * l));
		return true;

	}

	private static bool IsEvenDigits(long value, out int len)
	{
		var v = len = value.ToString().Length;
		return v % 2 == 0;
	}

	public override void LoadInput()
	{
		_data = ReadInputText("input.txt").Split(' ').Select(long.Parse).ToList();
	}
}