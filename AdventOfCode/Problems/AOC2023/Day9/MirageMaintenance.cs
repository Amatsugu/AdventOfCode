using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day9;
[ProblemInfo(2023, 9, "Mirage Maintenance")]
internal class MirageMaintenance : Problem<long, long>
{
	private long[][] _history = [];

	public override void LoadInput()
	{
		_history = ReadInputLines("input.txt").Select(ln => ln.Split(' ').Select(long.Parse).ToArray()).ToArray();
	}

	public long CalculateNumber(long[] values, bool last = true) 
	{ 
		var diffs = new long[values.Length - 1];
		for (int i = 0; i < values.Length - 1; i++)
			diffs[i] = values[i + 1] - values[i];

		if (diffs.Any(d => d != 0))
			return last ? (values.Last() + CalculateNumber(diffs, last)) : (values.First() - CalculateNumber(diffs, last));
		else
			return last ? values.Last() : values.First();
	}

	[Obsolete("Optimize too soon garbage")]
	public long GetNextNumber(long[] history)
	{
		var data = new List<List<long>>
		{
			new(history.Reverse())
		};
		CalculateLayer(data, 1, 1);
		var sum = data.Select(d => d.First()).Sum();
		Console.WriteLine();
		Console.WriteLine(string.Join(',', history));
		Console.WriteLine();

		foreach (var item in data)
			Console.WriteLine(string.Join(" | ", item.Select(n => n.ToString())));
		return sum;
	}

	[Obsolete("Optimize too soon garbage")]
	public void CalculateLayer(List<List<long>> data, int layer, int pos)
	{
		while (data.Count <= layer)
			data.Add([]);
		var lastLayer = layer - 1;
		if (layer != 0 && data[lastLayer].Count <= pos)
			CalculateLayer(data, lastLayer, pos + 1);

		var right = data[lastLayer][pos - 1];
		var left = data[lastLayer][pos];
		var diff = right - left;
		data[layer].Add(diff);
		if (diff != 0 && pos == 1)
			CalculateLayer(data, layer + 1, 1);
	}

	public enum State
	{
		Diff,
		Expand,
		Predict
	}

	public override void CalculatePart1()
	{
		//GetNextNumber(_history[2]);
		Part1 = _history.Select(c => CalculateNumber(c)).Sum();
	}

	public override void CalculatePart2()
	{
		Part2 = _history.Select(c => CalculateNumber(c, false)).Sum();
	}

}
