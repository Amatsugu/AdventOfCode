using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day12;

[ProblemInfo(2024, 12, "Garden Groups")]
internal class GardenGroups : Problem<int, int>
{
	private char[][] _data = [];

	public readonly static Vec2<int>[] DIRS = [
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
		var r = FindPlots(_data).Select(plot => (plot.plant, plot.area.Count, CountSides(plot.outline)));
	}

	private static int CountSides(List<Vec2<int>> perimeter)
	{
		if (perimeter.Count <= 2)
			return 4;
		var sides = 1;

		var lastDir = new Vec2<int>(0,0);
		//var startDir = lastDir;
		for (int i = 0; i < perimeter.Count; i++)
		{
			var p = perimeter[i];
			var p2 = perimeter[(i + 1) % perimeter.Count];
			var dir = p - p2;
			if(dir != lastDir)
				sides++;
			lastDir = dir;
		}

		return sides < 4 ? 4 : sides;
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
				GetMembers(data, plant, p,visited, members, ref perimeter, outline);
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
		_data = ReadInputLines("sample.txt").Select(ln => ln.ToCharArray()).ToArray();
	}
}
