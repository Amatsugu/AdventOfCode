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
		var results = new int[_data.Count];
		Parallel.ForEach(_data, (m, _, i) => results[i] = m.SolveLights());
		Part1 = results.Sum();
	}

	public override void CalculatePart2()
	{
		var results = new int[_data.Count];
		Parallel.ForEach(_data, (m, _, i) => results[i] = m.SolveJoltage());
		Part2 = results.Sum();
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt").Select(l => new Machine(l)).ToList();
	}

	public class Machine
	{
		public bool[] Target { get; private set; }
		public int[][] Operations { get; private set; }
		public int[] Requirements { get; private set; }

		public Machine(string data)
		{
			var sections = data.Split(' ');
			Target = sections[0][1..^1].Select(c => c == '.' ? false : true).ToArray();
			Operations = sections[1..^1].Select(op => op[1..^1])
				.Select(op => op.Split(',').Select(int.Parse).ToArray())
				.ToArray();
			Requirements = sections[^1][1..^1].Split(',').Select(int.Parse).ToArray();
		}

		public int SolveLights()
		{ 
			var state = Enumerable.Repeat(false, Target.Length).ToArray();
			var best = Target.Length;
			return Solve(Target, Operations, state, ref best);

			static int Solve(bool[] goal, int[][] operations, bool[] curState, ref int best, int depth = 0)
			{
				static bool IsSolved(bool[] goal, bool[] state)
				{
					for (int i = 0; i < goal.Length; i++)
					{
						if (goal[i] != state[i])
							return false;
					}
					return true;
				}

				if (IsSolved(goal, curState))
					return depth;
				if (depth >= best)
					return depth;

				var opCount = int.MaxValue;
				foreach (var op in operations)
				{
					var state = new bool[curState.Length];
					Buffer.BlockCopy(curState, 0, state, 0, curState.Length);
					
					foreach (var idx in op)
						state[idx] = !curState[idx];
					var c = Solve(goal, operations, state, ref best, depth + 1);
					if (c < best)
						best = c;
					if (c < opCount)
						opCount = c;
				}
				return opCount;
			}
		}

		public int SolveJoltage()
		{
			var state = new int[Requirements.Length];
			var best = int.MaxValue;
			return Solve(Requirements, Operations, state, ref best);
			static int Solve(int[] target, int[][] operations, int[] curState, ref int best, int depth = 0)
			{
				if (depth > best)
					return int.MaxValue;
				if (IsOver(target, curState))
					return int.MaxValue;
				if (IsSolved(target, curState))
				{
					if (depth < best)
						best = depth;
					return depth;
				}
				static bool IsOver(int[] target, int[] curState) 
				{
					for (int i = 0; i < target.Length; i++)
					{
						if (target[i] < curState[i])
							return true;
					}
					return false;
				}
				static bool IsSolved(int[] target, int[] curState)
				{
					for (int i = 0; i < target.Length; i++)
					{
						if (target[i] != curState[i])
							return false;
					}
					return true;
				}
				var opCount = int.MaxValue;
				foreach (var op in operations)
				{
					var state = new int[curState.Length];
					Buffer.BlockCopy(curState, 0, state, 0, curState.Length * sizeof(int));
					foreach (var idx in op)
						state[idx] += 1;

					var c = Solve(target, operations, state, ref best, depth + 1);
					if (c < opCount)
						opCount = c;
				}
				return opCount;
			}
		}
	}
}
