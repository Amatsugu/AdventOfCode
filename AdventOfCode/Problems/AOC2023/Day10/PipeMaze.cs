using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day10;

[ProblemInfo(2023, 10, "Pipe Maze")]
internal class PipeMaze : Problem<int, int>
{
	private string[] _maze = [];

	public override void LoadInput()
	{
		_maze = ReadInputLines();
	}

	public override void CalculatePart1()
	{
		var start = GetStartPointPos();
		var nPoints = GetStartConnections(start);
		var seen = new Dictionary<(int, int), int>();

		foreach (var point in nPoints)
		{
			var curPoint = start;
			var prevPoint = point;
			var dist = 0;
			while (true)
			{
				dist++;
				var next = GetNextPoint(curPoint, prevPoint);
				prevPoint = curPoint;
				curPoint = next;
				if (next == start)
					break;
				if (seen.TryGetValue(next, out var d))
				{
					if (d > dist)
						seen[next] = dist;
					else
						break;
				}
				else
					seen.Add(next, dist);
			}
		}

		Part1 = seen.Values.Max();
	}

	private (int x, int y) GetStartPointPos()
	{
		for (int y = 0; y < _maze.Length; y++)
		{
			var x = _maze[y].IndexOf('S');
			if (x >= 0)
				return (x, y);
		}
		throw new Exception("Start point not found");
	}

	private List<(int x, int y)> GetStartConnections((int x, int y) pos)
	{
		var points = new List<(int x, int y)>();
		if (_maze[pos.y + 1][pos.x] is '|' or 'J' or 'L')
			points.Add((pos.x + 1, pos.y));
		if (_maze[pos.y - 1][pos.x] is '|' or 'F' or '7')
			points.Add((pos.x - 1, pos.y));

		if (_maze[pos.y][pos.x + 1] is '-' or 'J' or '7')
			points.Add((pos.x, pos.y + 1));
		if (_maze[pos.y][pos.x - 1] is '-' or 'F' or 'L')
			points.Add((pos.x, pos.y - 1));

		return points;
	}

	private (int x, int y) GetNextPoint((int x, int y) pos, (int x, int y) prev)
	{
		var curPipe = _maze[pos.y][pos.x];
		(int x, int y) = (pos.x - prev.x, pos.y - prev.y);

		if (curPipe == 'S')
			return GetStartConnections(pos).First(p => p != prev);

		return curPipe switch
		{
			'|' => (pos.x, pos.y + y),
			'-' => (pos.x + x, pos.y),
			'L' => (pos.x + y, pos.y + x),
			'F' => (pos.x - y, pos.y - x),
			'J' => (pos.x - y, pos.y - x),
			'7' => (pos.x + y, pos.y + x),
			_ => throw new Exception()
		};
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}
}