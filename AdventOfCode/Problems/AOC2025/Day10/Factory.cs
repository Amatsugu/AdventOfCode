using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day10;

[ProblemInfo(2025, 10, "Factory")]
internal class Factory : Problem<int, int>
{
	private List<Machine> _data = [];

	public override void CalculatePart1()
	{
		Part1 = _data.Sum(m => m.SolveLights());
	}

	public override void CalculatePart2()
	{
		Part2 = _data[0].SolveJoltage();
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt").Select(l => new Machine(l)).ToList();
	}

	public class Machine
	{
		public uint Target { get; private set; }
		public uint[] Operations { get; private set; }
		public uint[][] JoltOperations { get; private set; }
		public uint[] JoltageTarget { get; private set; }

		public Machine(string data)
		{
			var sections = data.Split(' ');
			Target = (uint)sections[0][1..^1].Select((v, idx) => (value: v == '.' ? 0u : 1u, idx)).Sum(v => v.value * (uint)Math.Pow(2, v.idx));
			Operations = sections[1..^1].Select(v => v[1..^1]).Select(v => (uint)v.Split(',').Select(uint.Parse).Sum(v => (uint)Math.Pow(2, v))).ToArray();
			JoltOperations = sections[1..^1].Select(v => v[1..^1]).Select(v => v.Split(',').Select(uint.Parse).ToArray()).ToArray();
			JoltageTarget = sections[^1][1..^1].Split(',').Select(uint.Parse).ToArray();
		}

		public int SolveLights()
		{
			var open = new List<(uint value, int depth)>() { (0, 0) };
			var best = -1;

			while (open.Count != 0)
			{
				var (value, depth) = open[0];
				open.RemoveAt(0);
				foreach (var op in Operations)
				{
					var v = value ^ op;
					if (v == Target)
						return depth + 1;
					if (CanSolveLight(v))
					{
						best = depth + 2;
						continue;
					}
					open.Add((v, depth + 1));
				}
				if (best != -1)
					return best;
			}
			return -1;
		}

		private bool CanSolveLight(uint value)
		{
			var targetOp = value ^ Target;
			return Operations.Contains(targetOp);
		}

		public int SolveJoltage()
		{
			var graph = new JoltageGraph(JoltageTarget, JoltOperations);
			return graph.Solve();
		}

	}
}