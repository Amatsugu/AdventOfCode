﻿using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Problems.AOC2024.Day12;

[ProblemInfo(2024, 12, "Garden Groups")]
internal class GardenGroups : Problem<int, int>
{
	private char[][] _data = [];

	public static readonly Vec2<int>[] DIRS = [
			new(0,1),
			new(1,0),
			new(0,-1),
			new(-1,0),
		];

	public override void CalculatePart1()
	{
		var r = FindPlots(_data);
		Part1 = r.Sum(plot => plot.area.Count * plot.perimeter);
	}

	public override void CalculatePart2()
	{
		var plots = FindPlots(_data);
		var r = plots.Select(plot => (plot.plant, area: plot.area.Count, sides: CountSides(GroupSides(plot.outline), plot.area)));
		foreach (var item in plots)
		{
			Console.WriteLine($"{item.plant}: {CountSides(GroupSides(item.outline), item.area)}");
			var groups = GroupSides(item.outline);
			DrawPlot(item.area, groups, item.plant);
		}
		Part2 = r.Sum(v => v.area * v.sides);
	}

	public static List<List<Vec2<int>>> GroupSides(List<Vec2<int>> outline)
	{
		var result = new List<List<Vec2<int>>>();
		var visited = new HashSet<Vec2<int>>();
		var open = new HashSet<Vec2<int>>(outline);

		while (open.Count > 0)
		{ 
			var p = open.First();
			open.Remove(p);
			var group = new List<Vec2<int>>() { p };
			GetGroup(p, group);
			result.Add(group);
		}

		void GetGroup(Vec2<int> point, List<Vec2<int>> group)
		{
			throw new NotImplementedException();
		}

		return result;

	}

	public static void DrawPlot(List<Vec2<int>> area, List<List<Vec2<int>>> outline, char plant)
	{
		var (min, max) = GetBounds(outline.SelectMany(v => v).ToList());

		ConsoleColor[] colors = [
			ConsoleColor.Red,
			ConsoleColor.Green,
			ConsoleColor.Blue,
			ConsoleColor.Yellow,
			ConsoleColor.Magenta,
			ConsoleColor.Cyan,
			];

		for (int y = min.Y; y <= max.Y; y++)
		{
			for (int x = min.X; x <= max.X; x++)
			{
				Console.ResetColor();
				var p = new Vec2<int>(x,y);
				if (area.Contains(p))
				{
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					Console.Write(plant);
				}
				else
				{
					var match = outline.FirstOrDefault(v => v.Contains(p));
					if (match != null)
					{
						var idx = outline.IndexOf(match);
						Console.BackgroundColor = colors[idx % colors.Length];
						Console.Write(' ');
					}else
						Console.Write(' ');
				}
				Console.ResetColor();
			}
			Console.WriteLine();
		}
		Console.ResetColor();
	}

	public static void DrawPoints(List<Vec2<int>> data, char display)
	{
		var (min, max) = GetBounds(data);

		var output = new StringBuilder();
		for (int y = min.Y; y <= max.Y; y++)
		{
			for (int x = min.X; x <= max.X; x++)
			{
				var p = new Vec2<int>(x,y);
				if (data.Contains(p))
					output.Append(display);
				else
					output.Append(' ');

			}
			output.AppendLine();
		}
		Console.WriteLine(output);
	}

	public static (Vec2<int> min, Vec2<int> max) GetBounds(List<Vec2<int>> points)
	{
		var min = Vec2<int>.Splat(int.MaxValue);
		var max = Vec2<int>.Splat(int.MinValue);
		foreach (var pos in points)
		{
			if (pos.X < min.X)
				min.X = pos.X;
			if (pos.Y < min.Y)
				min.Y = pos.Y;

			if (pos.X > max.X)
				max.X = pos.X;
			if (pos.Y > max.Y)
				max.Y = pos.Y;
		}
		return (min, max);
	}

	public static int CountSides(List<List<Vec2<int>>> groups, List<Vec2<int>> area)
	{
		int Sides(Vec2<int> point)
		{
			var s = 0;
			foreach (var dir in DIRS)
			{
				var n = point + dir;
				if(area.Contains(n))
					s++;
			}
			return s;
		}

		return groups.Sum(s => s.Select(Sides).Max());
	}

	private static List<(char plant, List<Vec2<int>> area, int perimeter, List<Vec2<int>> outline)> FindPlots(char[][] data)
	{
		var visited = new HashSet<Vec2<int>>(data.Length * data[0].Length);

		var results = new List<(char plant, List<Vec2<int>>, int perimeter, List<Vec2<int>> outline)>();

		for (int y = 0; y < data.Length; y++)
		{
			var row = data[y];
			for (int x = 0; x < row.Length; x++)
			{
				var p = new Vec2<int>(x, y);
				if (visited.Contains(p))
					continue;
				var members = new List<Vec2<int>>();
				var plant = data[y][x];
				var perimeter = 0;
				var outline = new List<Vec2<int>>();
				GetMembers(data, plant, p, visited, members, ref perimeter, outline);
				results.Add((plant, members, perimeter, outline));
			}
		}
		return results;
	}

	private static void GetMembers(char[][] data, char plant, Vec2<int> point, HashSet<Vec2<int>> visited, List<Vec2<int>> members, ref int perimeter, List<Vec2<int>> outline)
	{
		if (visited.Contains(point))
			return;
		visited.Add(point);
		members.Add(point);

		foreach (var dir in DIRS)
		{
			var n = dir + point;
			if (!IsInBounds(n, data))
			{
				perimeter += 1;
				outline.Add(n);
				continue;
			}
			if (data[n.Y][n.X] != plant)
			{
				perimeter += 1;
				outline.Add(n);
				continue;
			}
			GetMembers(data, plant, n, visited, members, ref perimeter, outline);
		}
	}

	public static bool IsInBounds(Vec2<int> pos, char[][] data)
	{
		if (pos.X < 0 || pos.Y < 0)
			return false;
		if (pos.X >= data.Length || pos.Y >= data[0].Length)
			return false;
		return true;
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("sample3.txt").Select(ln => ln.ToCharArray()).ToArray();
	}
}