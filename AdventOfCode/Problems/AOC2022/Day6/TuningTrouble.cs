using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2022.Day6;

[ProblemInfo(2022, 6, "Tuning Trouble")]
internal class TuningTrouble : Problem<int, int>
{
	private string _input = string.Empty;

	public override void CalculatePart1()
	{
		Part1 = FindMarker(4);
	}

	private int FindMarker(int size = 4)
	{
		for (int i = size; i < _input.Length; i++)
		{
			var group = _input[(i - size)..i];
			if (group.All(c => group.Count(gc => gc == c) == 1))
				return i;
		}
		return -1;
	}

	public override void CalculatePart2()
	{
		Part2 = FindMarker(14);
	}

	public override void LoadInput()
	{
		_input = ReadInputText();
	}
}
