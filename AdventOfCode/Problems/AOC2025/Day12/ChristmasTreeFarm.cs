using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day12;

[ProblemInfo(2025, 12, "Christmas Tree Farm")]
internal class ChristmasTreeFarm : Problem<int, int>
{
	private Present[] _presents = [];
	private Tree[] _trees = [];

	public override void CalculatePart1()
	{
		throw new NotImplementedException();
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("sample.txt");

		var shapesIds = new List<int>();
		var trees  = new List<int>();


		for (int i = 0; i < lines.Length; i++)
		{
			string? line = lines[i];
			if (line.Length == 0)
				continue;
			switch (line)
			{
				case [.., ':']:
					shapesIds.Add(i);
					i += 3;
					break;
				case string a when a.Contains('x'):
					trees.Add(i);
					break;
			}
		}

		_presents = shapesIds.Select(i =>
		{
			var id = int.Parse(lines[i][0..^1]);
			var shapeData = lines[(i + 1)..(i + 4)];

			var points = shapeData.SelectMany((line, idx) =>
			{
				return line.Select((c, x) => (c, x: x - 1)).Where(v => v.c == '#').Select(v => new Vec2i(v.x, idx - 1));
			});

			return new Present(id, [.. points]);
		}).ToArray();


		_trees = trees.Select(i =>
		{
			var lineData = lines[i].Split(':');
			var size = lineData[0].Split('x').Select(int.Parse).ToArray();

			var presents = lineData[1].TrimStart().Split(' ').Select(int.Parse);

			return new Tree(new Vec2i(size[0], size[1]), presents.ToArray());
		}).ToArray();
	}
}
