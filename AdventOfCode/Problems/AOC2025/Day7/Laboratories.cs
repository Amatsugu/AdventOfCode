using AdventOfCode.Utils.Models;

using MaybeError;
using MaybeError.Errors;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day7;
[ProblemInfo(2025, 7, "Laboratories")]
internal class Laboratories : Problem<long, long>
{
	private string[] _grid = [];
	private Vec2<int> _size;

	public override void CalculatePart1()
	{
		var start = GetStart(_grid).Value;
		Part1 = TraceBeam(start, _grid, _size);
	}

	public override void CalculatePart2()
	{
		var start = GetStart(_grid).Value;
		Part2 = QuantumTraceBeam(start, _grid, _size, []);
	}

	private static Maybe<Vec2<int>> GetStart(string[] data)
	{
		for (int y = 0; y < data.Length; y++)
		{
			var ln = data[y];
			for (int x = 0; x < ln.Length; x++)
			{
				if (ln[x] == 'S')
					return new Vec2<int>(x, y);
			}
		}
		return new Error("There is no start position");
	}

	private static long TraceBeam(Vec2<int> pos, string[] data, Vec2<int> size)
	{
		return TraceBeam(pos, data, size, []);
	}
	private static long TraceBeam(Vec2<int> pos, string[] data, Vec2<int> size, HashSet<Vec2<int>> visited)
	{
		if (pos.X < 0 || pos.Y < 0)
			return 0;
		if (pos.X >= size.X || pos.Y >= size.Y)
			return 0;
		var curPos = data[pos.Y][pos.X];

		if (curPos == '^')
		{
			var left = new Vec2<int>(pos.X - 1, pos.Y);
			var right = new Vec2<int>(pos.X + 1, pos.Y);
			var leftCount = visited.Add(left) ? TraceBeam(left, data, size, visited) : 0;
			var rightCount = visited.Add(right) ? TraceBeam(right, data, size, visited) : 0;
			return 1 + leftCount + rightCount;
		}
		var next = new Vec2<int>(pos.X, pos.Y + 1);
		if (visited.Contains(next))
			return 0;
		visited.Add(next);
		return TraceBeam(next, data, size, visited);
	}

	private static long QuantumTraceBeam(Vec2<int> pos, string[] data, Vec2<int> size, Dictionary<Vec2<int>, long> trace)
	{

		if (pos.X < 0 || pos.Y < 0)
			return 1;
		if (pos.X >= size.X || pos.Y >= size.Y)
			return 1;
		if (trace.TryGetValue(pos, out var c))
			return c;
		var curPos = data[pos.Y][pos.X];

		if (curPos == '^')
		{
			var left = new Vec2<int>(pos.X - 1, pos.Y);
			var right = new Vec2<int>(pos.X + 1, pos.Y);

			if (!trace.TryGetValue(left, out long leftCount))
			{
				leftCount = QuantumTraceBeam(left, data, size, trace);
				trace.Add(left, leftCount);
			}

			if (!trace.TryGetValue(right, out long rightCount))
			{
				rightCount = QuantumTraceBeam(right, data, size, trace);
				trace.Add(right, rightCount);
			}

			return leftCount + rightCount;
		}
		return QuantumTraceBeam(new(pos.X, pos.Y + 1), data, size, trace);
	}



	public override void LoadInput()
	{
		_grid = ReadInputLines("input.txt");
		_size = new Vec2<int>(_grid[0].Length, _grid.Length);
	}
}
