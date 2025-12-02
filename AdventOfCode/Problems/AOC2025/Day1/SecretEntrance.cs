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
	public int[] Input { get; set; } = [];
	public override void CalculatePart1()
	{
		var c = 0;
		var v = 50;
		foreach (var item in Input)
		{
			v += item;
			v = v.Mod(100);
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
			var sign = int.Sign(item);
			for (int i = 0; i < Math.Abs(item); i++)
			{
				v += sign;
				v = v.Mod(100);
				if (v == 0)
					c++;
			}
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
