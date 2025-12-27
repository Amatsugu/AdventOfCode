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
		var boxes = _boxPositions.Select((p, i) => new JunctionBox(i, p, [])).ToArray();
		var closest = GetClosestPairs(_boxPositions, 1000);
		foreach (var (a, b)in closest)
		{
			var boxA = boxes.First(box => box.Pos == a);
			var boxB = boxes.First(box => box.Pos == b);
			boxA.Connections.Add(boxB.Id);
			boxB.Connections.Add(boxA.Id);
		}
		var networks = TraceNetworks(boxes);
		Part1 = networks.Select(n => (long)n.Count).OrderDescending().Take(3).Aggregate((a, b) => a * b);
	}

	private static List<HashSet<int>> TraceNetworks(JunctionBox[] junctionBoxes)
	{
		var networks = new List<HashSet<int>>();
		foreach (var box in junctionBoxes)
		{
			if (networks.Any(n => n.Contains(box.Id)))
				continue;
			var net = new HashSet<int>() { box.Id };
			TraceJunction(box, junctionBoxes, net);
			networks.Add(net);
		}
		return networks;//.Where(n => n.Count > 0).ToList();
	}

	private static void TraceJunction(JunctionBox box, JunctionBox[] boxes, HashSet<int> result)
	{
		foreach (var connection in box.Connections)
		{
			if (result.Add(connection))
				TraceJunction(boxes[connection], boxes, result);
		}
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
		_boxPositions = ReadInputLines("input.txt")
			.Select(l => l.Split(',').Select(int.Parse))
			.Select(c => new Vec3<int>(c.First(), c.Skip(1).First(), c.Last()))
			.ToArray();
	}

	private record class JunctionBox(int Id, Vec3<int> Pos, HashSet<int> Connections);
}
