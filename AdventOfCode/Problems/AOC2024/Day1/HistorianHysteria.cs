using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day1;
[ProblemInfo(2024, 1, "Historian Hysteria")]
internal class HistorianHysteria : Problem<int, int>
{
	private int[] _left = [];
	private int[] _right = [];

	public override void CalculatePart1()
	{
		Part1 = _left.Order()
			.Zip(_right.Order())
			.Select(a => Math.Abs(a.First - a.Second))
			.Sum();
	}

	public override void CalculatePart2()
	{
		var rightFeq = _right.GroupBy(x => x)
			.ToFrozenDictionary(g => g.Key, g => g.Count());
		Part2 = _left.Select(x => rightFeq.TryGetValue(x, out var f) ? f * x : 0)
			.Sum();
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines();
		var data = lines.Select(l => l.Split(' ').Select(int.Parse)).ToList();
		_left = data.Select(l => l.First()).ToArray();
		_right = data.Select(l => l.Last()).ToArray();
	}
}
