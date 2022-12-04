using AdventOfCode.Runner;
using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2022.Day3;
[ProblemInfo(2022, 3, "Rucksack Reorganization")]
internal class RucksackReorganization : Problem
{
	private string[] _sacks;

	public RucksackReorganization() { 
		_sacks = Array.Empty<string>();
	}

	public override void LoadInput()
	{
		_sacks = ReadInputLines("input.txt");
	}
	public override void CalculatePart1()
	{
		var total = 0;
		foreach (var sack in _sacks)
		{
			var mid = sack.Length / 2;
			var left = sack[..mid];
			var right = sack[mid..];
			var common = right.First(itm => left.Contains(itm));
			total += GetValue(common);
		}
		Part1 = total.ToString();
	}

	public static int GetValue(char c)
	{
		return c switch
		{
			<= 'Z' => c - 'A' + 27,
			_ => c - 'a' + 1
		};
	}

	public override void CalculatePart2()
	{
		var groups = _sacks.Chunk(3);
		var total = 0;
		foreach (var group in groups)
		{
			var badgeType = group[0].First(badge => group[1..].All(sack => sack.Contains(badge)));
			total += GetValue(badgeType);
		}
		Part2 = total.ToString();
	}

}
