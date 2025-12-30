using AdventOfCode.Utils.Models;

using System;
using System.Collections.Frozen;
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
		Part1 = GetAccessableRolls(_data, _size).Count;
	}

	public static int CountNeighbors(string[] data, Vec2<int> size, Vec2<int> pos)
	{
		var c = 0;
		for (int y = pos.Y-1; y <= pos.Y + 1; y++)
		{
			if (y < 0 || y >= size.Y)
				continue;
			for (int x = pos.X - 1; x <= pos.X + 1; x++)
			{
				if (x < 0 || x >= size.X)
					continue;
				if (pos.X == x && pos.Y == y)
					continue;
				if (data[y][x] == '@')
					c++;
			}
		}
		return c;
	}

	public override void CalculatePart2()
	{
		var data = _data;
		var rolls = GetAccessableRolls(data, _size);
		Part2 += rolls.Count;
		while(rolls.Count > 0)
		{
			data = RemoveRolls(data, _size, rolls);
			rolls = GetAccessableRolls(data, _size);
			Part2 += rolls.Count;
		}
	}

	public static List<Vec2<int>> GetAccessableRolls(string[] data, Vec2<int> size)
	{
		var results = new List<Vec2<int>>();
		for (int y = 0; y < size.Y; y++)
		{
			for (int x = 0; x < size.X; x++)
			{
				var pos = new Vec2<int>(x, y);
				if (data[pos.Y][pos.X] != '@')
					continue;
				var n = CountNeighbors(data, size, pos);
				if (n < 4)
					results.Add(pos);
			}
		}
		return results;
	}

	public static string[] RemoveRolls(string[] data, Vec2<int> size, List<Vec2<int>> rolls)
	{
		var positions = rolls.ToFrozenSet();
		return data.Select((row, y) =>
		{
			var updatedRow = row.Select((col, x) =>
			{
				var pos = new Vec2<int>(x, y);
				if (positions.Contains(pos))
					return '.';
				else
					return col;
			}).ToArray();
			return new string(updatedRow);
		}).ToArray();
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt");
		_size = new Vec2<int>(_data[0].Length, _data.Length);
	}
}
