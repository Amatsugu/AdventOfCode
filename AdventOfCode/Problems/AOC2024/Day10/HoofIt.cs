using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day10;
[ProblemInfo(2024, 10, "Hoof It")]
internal class HoofIt : Problem<int, int>
{
	private int[][] _data = [];

	public static Vec2<int>[] DIRS = [
			new(0, -1),
			new(1, 0),
			new(0, 1),
			new(-1, 0),
		];

	public override void CalculatePart1()
	{
		for (int y = 0; y < _data.Length; y++)
		{
			var row = _data[y];
			for (int x = 0; x < row.Length; x++)
			{
				var h = row[x];
				if (h != 0)
					continue;
				var (s,_) = GetScore(new(x, y));
				Part1 += s;
			}
		}
	}

	public override void CalculatePart2()
	{
		for (int y = 0; y < _data.Length; y++)
		{
			var row = _data[y];
			for (int x = 0; x < row.Length; x++)
			{
				var h = row[x];
				if (h != 0)
					continue;
				var (_, s) = GetScore(new(x, y));
				Part2 += s;
			}
		}
	}

	public (int score, int scoreDistinct) GetScore(Vec2<int> pos)
	{
		return GetScore(pos, []);
	}

	public (int score, int scoreDistinct) GetScore(Vec2<int> pos, HashSet<Vec2<int>> visited)
	{
		var curHeight = _data[pos.Y][pos.X];
		if (curHeight == 9)
		{
			if(visited.Contains(pos))
				return (0, 1);
			visited.Add(pos);
			return (1, 1);
		}

		var score = 0;
		var scoreDistinct = 0;
		foreach (var dir in DIRS)
		{
			var n = pos + dir;
			if (!IsInBounds(n))
				continue;
			var h = _data[n.Y][n.X];
			if (h - curHeight != 1)
				continue;
			var (s, d)= GetScore(n, visited);
			score += s;
			scoreDistinct += d;
		}

		return (score, scoreDistinct);
	}

	public bool IsInBounds(Vec2<int> pos)
	{
		if (pos.X < 0 || pos.Y < 0) 
			return false;
		if(pos.X >= _data.Length || pos.Y >= _data[0].Length) 
			return false;
		return true;
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt").Select(l => l.Select(v => v - '0').ToArray()).ToArray();
	}
}
