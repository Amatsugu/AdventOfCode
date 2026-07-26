using AdventOfCode.Utils.Models;

using Google.OrTools.PDLP;

using MathNet.Numerics;

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day8;

[ProblemInfo(2025, 8, "Playground")]
internal class Playground : Problem<long, long>
{
	private Vec3l[] _boxPositions = [];

	public override void CalculatePart1()
	{
		Part1 = BuildNetwork(1000);
	}

	public long BuildNetwork(int size) => BuildNetwork(size, out _);
	public long BuildNetwork(int size, out (Vec3l a, Vec3l b) last)
	{
		var networks = _boxPositions.Select(b => new Network(b)).ToList();
		var closest = GetClosestPairs(_boxPositions, size);
		last = closest[0];
		foreach (var (a, b) in closest)
		{
			var existingNetworkA = networks.FirstOrDefault(n => n.Members.Contains(a));
			var existingNetworkB = networks.FirstOrDefault(n => n.Members.Contains(b));
			if (existingNetworkA == existingNetworkB)
				continue;
			if ((existingNetworkA != null && existingNetworkB == null))
			{
				existingNetworkA.AddConnection(b);
				last = (a, b);
			}
			else if (existingNetworkB != null && existingNetworkA == null)
			{
				existingNetworkB.AddConnection(a);
				last = (a, b);
			}
			else if (existingNetworkA != null && existingNetworkB != null)
			{
				existingNetworkA.MergeWith(existingNetworkB);
				networks.Remove(existingNetworkB);
				last = (a, b);
			}
		}
		return networks.Select(n => n.Members.Count)
			.OrderDescending()
			.Take(3)
			.Aggregate((a, b) => a * b);
	}

	private static List<(Vec3l a, Vec3l b)> GetClosestPairs(Vec3l[] boxes, int count = 10)
	{
		var distances = new Dictionary<(Vec3l a, Vec3l b), long>();

		for (int i = 0; i < boxes.Length; i++)
		{
			var a = boxes[i];
			for (int j = (i + 1); j < boxes.Length; j++)
			{
				var b = boxes[j];
				var d = a.DistanceSq(b);
				distances.Add((a, b), d);
			}
		}
		return distances.OrderBy(v => v.Value).Take(count).Select(v => v.Key).ToList();
	}

	public override void CalculatePart2()
	{
		BuildNetwork(int.MaxValue, out var lastPair);

		Part2 = lastPair.a.X * lastPair.b.X;
	}

	public override void LoadInput()
	{
		_boxPositions = ReadInputLines("input.txt")
			.Select(l => l.Split(',').Select(long.Parse))
			.Select(c => new Vec3l(c.First(), c.Skip(1).First(), c.Last()))
			.ToArray();
	}


	private class Network
	{
		public HashSet<Vec3l> Members { get; private set; } = [];

		public Network(Vec3l box)
		{
			Members.Add(box);
		}

		public Network AddConnection(Vec3l other)
		{
			Members.Add(other);
			return this;
		}

		public Network MergeWith(Network other)
		{
			foreach (var box in other.Members)
			{
				AddConnection(box);
			}
			return this;
		}
	}
}