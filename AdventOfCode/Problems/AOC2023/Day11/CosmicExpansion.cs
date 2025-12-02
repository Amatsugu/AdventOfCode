using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day11;

[ProblemInfo(2023, 11, "Cosmic Expansion")]
internal class CosmicExpansion : Problem<int, long>
{
	private string[] _data = [];
	private int[] _yGaps = [];
	private int[] _xGaps = [];

	public override void CalculatePart1()
	{
		var points = GetPoints().Select(p => Inflate(p, 2)).ToList();
		for (int i = 0; i < points.Count - 1; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				var a = points[i];
				var b = points[j];
				Part1 += GetDistance(a, b);
			}
		}
	}

	public List<(int x, int y)> GetPoints()
	{
		var result = new List<(int x, int y)>();
		for (int y = 0; y < _data.Length; y++)
		{
			for (int x = 0; x < _data[0].Length; x++)
			{
				if (_data[y][x] == '#')
					result.Add((x, y));
			}
		}
		return result;
	}

	public int GetDistance((int x, int y) a, (int x, int y) b)
	{
		var xDist = Math.Abs(a.x - b.x);
		var yDist = Math.Abs(a.y - b.y);

		return xDist + yDist;
	}

	public (int x, int y) Inflate((int x, int y) point, int factor)
	{
		factor -= 1;
		var cX = _xGaps.Count(x => point.x >= x); 
		var cY = _yGaps.Count(y => point.y >= y);
		return (point.x + cX * factor, point.y + cY * factor);
	}

	public override void CalculatePart2()
	{
		var points = GetPoints().Select(p => Inflate(p, 1000000)).ToList();
		for (int i = 0; i < points.Count - 1; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				var a = points[i];
				var b = points[j];
				Part2 += GetDistance(a, b);
			}
		}
	}

	public override void LoadInput()
	{
		_data = ReadInputLines();
		_yGaps = _data.Select((v, i) => (c: v.Count(x => x == '.'), i))
					.Where(v => v.c == _data[0].Length)
					.Select(v => v.i)
					.ToArray();

		_xGaps = _data.Transpose().Select((v, i) => (c: v.Count(x => x == '.'), i))
					.Where(v => v.c == _data[0].Length)
					.Select(v => v.i)
					.ToArray();
	}
}
