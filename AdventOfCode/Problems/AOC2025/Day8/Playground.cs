using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day8;

[ProblemInfo(2025, 8, "Playground")]
internal class Playground : Problem<long, long>
{
	private Vec3<int>[] _boxPositions= [];

	public override void CalculatePart1()
	{
		var boxes = _boxPositions.Select((p, i) => new JunctionBox(i, p, []));
		var closest = GetClosestPairs(_boxPositions, 10);
		foreach (var (a, b)in closest)
		{
			var boxA = boxes.First(box => box.Pos == a);
			var boxB = boxes.First(box => box.Pos == b);
			boxA.Connections.Add(boxB.Id);
			boxB.Connections.Add(boxA.Id);
		}

	}

	private static long[] TraceNetworks(JunctionBox[] junctionBoxes)
	{
		return [];
	}

	private static List<(Vec3<int> a , Vec3<int> b)> GetClosestPairs(Vec3<int>[] boxes, int count = 10)
	{
		var distances = new Dictionary<(Vec3<int> a, Vec3<int> b), double>();

		for (int i = 0; i < boxes.Length; i++)
		{
			var a = boxes[i];
			for (int j = (i + 1); j < boxes.Length; j++)
			{
				var b = boxes[j];
				if (distances.ContainsKey((a, b)) || distances.ContainsKey((b, a)))
					continue;
				distances.Add((a, b), Math.Sqrt(a.DistanceSq(b)));
			}
		}
		return distances.OrderBy(v => v.Value).Take(count).Select(v => v.Key).ToList();
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		_boxPositions = ReadInputLines("sample.txt")
			.Select(l => l.Split(',').Select(int.Parse))
			.Select(c => new Vec3<int>(c.First(), c.Skip(1).First(), c.Last()))
			.ToArray();
	}

	private record JunctionBox(int Id, Vec3<int> Pos, HashSet<int> Connections);
}
