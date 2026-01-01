using AdventOfCode.Utils.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day8;

[ProblemInfo(2025, 8, "Playground")]
internal class Playground : Problem<long, long>
{
	private Vec3i[] _boxPositions= [];

	public override void CalculatePart1()
	{
		var networks = new List<Network>();
		var closest = GetClosestPairs(_boxPositions, 1000);
		foreach (var (a, b)in closest)
		{
			var existingNetworkA = networks.FirstOrDefault(n => n.Members.Contains(a));
			var existingNetworkB = networks.FirstOrDefault(n => n.Members.Contains(b));
			if ((existingNetworkA != null && existingNetworkB == null))
				existingNetworkA.AddConnection(a, b);
			else if (existingNetworkB != null && existingNetworkA == null)
				existingNetworkB.AddConnection(a, b);
			else if (existingNetworkA != null && existingNetworkB != null && existingNetworkA != existingNetworkB)
			{
				existingNetworkA.AddConnection(a, b);
				existingNetworkA.MergeWith(existingNetworkB);
				networks.Remove(existingNetworkB);
			}
			else if(existingNetworkA == null && existingNetworkB == null)
			{
				var newNetwork = new Network().AddConnection(a, b);
				networks.Add(newNetwork);
			}
		}
		Console.WriteLine($"Networks: {networks.Count}");
		Part1 = networks.Select(n => n.Members.Count)
			.OrderDescending()
			.Take(3)
			.Aggregate((a, b) => a * b);
	}

	private static List<(Vec3i a , Vec3i b)> GetClosestPairs(Vec3i[] boxes, int count = 10)
	{
		var distances = new Dictionary<(Vec3i a, Vec3i b), double>();

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
			.Select(c => new Vec3i(c.First(), c.Skip(1).First(), c.Last()))
			.ToArray();
	}

	private record class JunctionBox(Vec3i Pos, HashSet<Vec3i> Connections);

	private class Network
	{
		public HashSet<Vec3i> Members { get; private set; } = [];
		public List<JunctionBox> Boxes { get; private set; } = [];

		public bool IsConnectedTo(Vec3i pos)
		{
			return Members.Contains(pos);
		}

		public bool IntersectsWith(Network other)
		{
			return Members.Intersect(other.Members).Any();
		}

		public Network AddConnection(Vec3i a, Vec3i b)
		{
			if (Members.Contains(a) && Members.Contains(b))
				return this;
			Members.Add(a);
			Members.Add(b);
			var boxA = GetOrAddBox(a);
			var boxB = GetOrAddBox(b);
			boxA.Connections.Add(b);
			boxB.Connections.Add(a);
			return this;
		}

		private JunctionBox GetOrAddBox(Vec3i pos)
		{
			var box = Boxes.FirstOrDefault(box => box.Pos == pos);
			if(box == null)
			{
				box = new JunctionBox(pos, []);
				Boxes.Add(box);
			}
			return box;
		}

		public Network MergeWith(Network other)
		{
			foreach (var box in other.Boxes)
			{
				foreach (var connection in box.Connections)
				{
					AddConnection(box.Pos, connection);
				}
			}
			return this;
		}
	}
}
