using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day4;
[ProblemInfo(2025, 4, "Printing Department")]
internal class PrintingDeparment: Problem<int, int>
{
	private string[] _data = [];
	private Vec2<int> _size;

	public override void CalculatePart1()
	{
		var c = 0;
		for (int y = 0; y < _size.Y; y++)
		{
			for (int x = 0; x < _size.X; x++)
			{
				var pos = new Vec2<int>(x, y);
				if (_data[pos.Y][pos.X] != '@')
					continue;
				var n = CountNeighbors(pos);
				if (n < 4)
					c++;
			}
		}
		Part1 = c;
	}

	public int CountNeighbors(Vec2<int> pos)
	{
		var c = 0;
		for (int y = pos.Y-1; y <= pos.Y + 1; y++)
		{
			if (y < 0 || y >= _size.Y)
				continue;
			for (int x = pos.X - 1; x <= pos.X + 1; x++)
			{
				if (x < 0 || x >= _size.X)
					continue;
				if (pos.X == x && pos.Y == y)
					continue;
				if (_data[y][x] == '@')
					c++;
			}
		}
		return c;
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt");
		_size = new Vec2<int>(_data[0].Length, _data.Length);
	}
}
