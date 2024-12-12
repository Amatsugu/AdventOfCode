using AdventOfCode.Utils.Models;

using System.Collections.Frozen;

namespace AdventOfCode.Problems.AOC2024.Day8;

[ProblemInfo(2024, 8, "Resonant Collinearity")]
internal class ResonantCollinearity : Problem<int, int>
{
	private string[] _map = [];
	private int _width;
	private int _height;
	private FrozenDictionary<char, List<Vec2<int>>> _nodes = FrozenDictionary<char, List<Vec2<int>>>.Empty;

	public override void CalculatePart1()
	{
		var antiNodes = new List<Vec2<int>>();
		foreach (var (nodeType, nodes) in _nodes)
		{
			foreach (var a in nodes)
			{
				foreach (var b in nodes)
				{
					if (a == b)
						continue;
					antiNodes.AddRange(GetAntiNodes(a, b));
				}
			}
		}
		//PrintBoard(antiNodes);
		Part1 = antiNodes.Where(IsInBounds).Distinct().Count();
	}

	public void PrintBoard(List<Vec2<int>> antinodes)
	{
		Console.WriteLine();
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				Console.ResetColor();
				var p = new Vec2<int>(x, y);
				if (antinodes.Contains(p))
					Console.BackgroundColor = ConsoleColor.DarkGreen;
				Console.Write(_map[y][x]);
			}
			Console.ResetColor();
			Console.WriteLine();
		}
		Console.ResetColor();
	}

	public bool IsInBounds(Vec2<int> pos)
	{
		if (pos.X < 0 || pos.Y < 0)
			return false;
		if (pos.X >= _width || pos.Y >= _height)
			return false;
		return true;
	}

	private Vec2<int>[] GetAntiNodes(Vec2<int> a, Vec2<int> b)
	{
		var dir = a - b;

		var aNode1 = dir + a;
		var aNode2 = -dir + b;

		return [aNode1, aNode2];
	}

	private Vec2<int>[] GetHarmonicAntiNodes(Vec2<int> a, Vec2<int> b)
	{
		var dir = a - b;

		List<Vec2<int>> GetNodes(Vec2<int> start, Vec2<int> dir)
		{
			var results = new List<Vec2<int>>();
			while (true)
			{
				var p = dir + start;
				if(!IsInBounds(p))
					break;
				results.Add(p);
				start = p;

			}
			return results;
		}

		return [.. GetNodes(a, dir), .. GetNodes(b, -dir)];
	}

	public override void CalculatePart2()
	{
		var antiNodes = new List<Vec2<int>>();
		foreach (var (nodeType, nodes) in _nodes)
		{
			foreach (var a in nodes)
			{
				foreach (var b in nodes)
				{
					if (a == b)
						continue;
					antiNodes.AddRange(GetHarmonicAntiNodes(a, b));
				}
			}
		}
		//PrintBoard(antiNodes);
		antiNodes.AddRange(_nodes.Values.Where(v => v.Count > 1).SelectMany(v => v));
		Part2 = antiNodes.Distinct().Count();
	}

	public override void LoadInput()
	{
		_map = ReadInputLines("input.txt");
		var nodes = new Dictionary<char, List<Vec2<int>>>();
		_width = _map[0].Length;
		_height = _map.Length;
		for (int y = 0; y < _map.Length; y++)
		{
			var row = _map[y];
			for (int x = 0; x < row.Length; x++)
			{
				switch (row[x])
				{
					case '.':
						continue;
					default:
						var p = new Vec2<int>(x, y);
						if (!nodes.TryAdd(row[x], [p]))
							nodes[row[x]].Add(p);
						continue;
				}
			}
		}
		_nodes = nodes.ToFrozenDictionary();
	}
}