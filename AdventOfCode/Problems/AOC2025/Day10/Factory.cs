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
		Part1 += results.Sum();
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
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
					//for (int i = 0; i < curState.Length; i++)
					//	state[i] = curState[i];
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
			throw new NotImplementedException();
		}
	}
}
