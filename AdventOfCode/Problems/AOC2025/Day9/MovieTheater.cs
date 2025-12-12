using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day9;

[ProblemInfo(2025, 9, "Movie Theater")]
internal class MovieTheater : Problem<long, long>
{
	private Vec2<long>[] _input = [];

	public override void CalculatePart1()
	{
		for (long i = 0; i < _input.Length; i++)
		{
			var a = _input[i];
			for (long j = (i + 1); j < _input.Length; j++)
			{
				var b = _input[j];
				var area = CalculateArea(a, b);
				if (area > Part1)
				{
					Part1 = area;
				}
			}
		}
	}

	public static long CalculateArea(Vec2<long> a, Vec2<long> b)
	{
		var rect = (a - b).Abs() + 1;
		var area = Math.Abs(rect.X * rect.Y);
		//Console.WriteLine($"{a} -> {b} : {rect} = {area}");
		return area;
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		_input = ReadInputLines("input.txt").Select(l => l.Split(',').Select(long.Parse)).Select(v => new Vec2<long>(v.First(), v.Last())).ToArray();
	}
}
