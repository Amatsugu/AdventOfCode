using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZLinq;

namespace AdventOfCode.Problems.AOC2025.Day1;

[ProblemInfo(2025, 1, "Secret Entrance")]
internal class SecretEntrance : Problem<int, int>
{
	public const int LOCK_SIZE = 100;

	public int[] Input { get; set; } = [];
	public override void CalculatePart1()
	{
		var c = 0;
		var v = 50;
		foreach (var item in Input)
		{
			v += item;
			v = v.Mod(LOCK_SIZE);
			if (v == 0)
				c++;
		}
		Part1 = c;
	}

	public override void CalculatePart2()
	{
		var c = 0;
		var v = 50;
		foreach (var item in Input)
		{
			var vStart = v;

			v += item;
			if (item > 0)
				c += (int)Math.Floor(v / (float)LOCK_SIZE);
			else
			{
				var d = v / (float)LOCK_SIZE;
				var fl = Math.Floor(d);
				c += (int)Math.Abs(fl) - (vStart == 0 ? 1 : 0);
				if (fl == d)
					c += 1;
			}
			v = v.Mod(LOCK_SIZE);
		}
		Part2 = c;
	}

	public override void LoadInput()
	{
		Input = ReadInputLines("input.txt")
			.AsValueEnumerable()
			.Select(l =>
			{
				return l[0] switch
				{
					'L' => -int.Parse(l[1..]),
					'R' => int.Parse(l[1..]),
					_ => throw new NotImplementedException()
				};
			}).ToArray();
	}
}
