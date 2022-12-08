using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day8;

[ProblemInfo(2022, 8, "Treetop Tree House")]
internal class TreetopTreeHouse : Problem<int, int>
{
	private int[][] _input = Array.Empty<int[]>();
	private int _height;
	private int _width;

	public override void CalculatePart1()
	{
		for (int y = 1; y < _height - 1; y++)
		{
			for (int x = 1; x < _width - 1; x++)
			{
				Part1 += IsVisible(x, y) ? 1 : 0;
			}
		}
		Part1 += _height * 2 + _width * 2 - 4;
	}

	private bool IsVisible(int x, int y)
	{
		return IsVisibleColumn(x, y) || IsVisibleRow(x, y);
	}

	private bool IsVisibleColumn(int col, int row)
	{
		var tree = _input[row][col];

		var columnOfTrees = _input.Select(r => r[col]);

		var above = columnOfTrees.Take(row);
		var below = columnOfTrees.Skip(row + 1);

		return above.All(t => t < tree) || below.All(t => t < tree);
	}

	private bool IsVisibleRow(int col, int row)
	{
		var tree = _input[row][col];

		var rowOfTrees = _input[row];

		var left = rowOfTrees.Take(col);
		var right = rowOfTrees.Skip(col + 1);

		return left.All(t => t < tree) || right.All(t => t < tree);
	}

	public override void CalculatePart2()
	{
		Part2 = int.MinValue;
		for (int y = 1; y < _height - 1; y++)
		{
			for (int x = 1; x < _width - 1; x++)
			{
				var v = GetScenicScore(x, y);
				if (Part2 < v)
					Part2 = v;
			}
		}
	}

	public int GetScenicScore(int row, int col)
	{
		var tree = _input[row][col];

		var columnOfTrees = _input.Select(r => r[col]);
		var rowOfTrees = _input[row];

		var above = columnOfTrees.Take(row).Reverse();
		var below = columnOfTrees.Skip(row + 1);
		var left = rowOfTrees.Take(col).Reverse();
		var right = rowOfTrees.Skip(col + 1);

		var score = above.Select((t, idx) => (t, idx))
			.FirstOrDefault(v => v.t >= tree, (t: 0, idx: above.Count() - 1)).idx + 1;
		score *= below.Select((t, idx) => (t, idx))
			.FirstOrDefault(v => v.t >= tree, (t: 0, idx: below.Count() - 1)).idx + 1;

		score *= left.Select((t, idx) => (t, idx))
			.FirstOrDefault(v => v.t >= tree, (t: 0, idx: left.Count() - 1)).idx + 1;
		score *= right.Select((t, idx) => (t, idx))
			.FirstOrDefault(v => v.t >= tree, (t: 0, idx: right.Count() - 1)).idx + 1;

		return score;
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_input = lines.Select(ln => ln.Select(tree => int.Parse(tree.ToString())).ToArray()).ToArray();
		_height = _input.Length;
		_width = _input[0].Length;
	}
}