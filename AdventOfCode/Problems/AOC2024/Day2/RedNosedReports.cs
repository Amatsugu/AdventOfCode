using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day2;
[ProblemInfo(2024, 2, "Red-Nosed Reports")]
internal class RedNosedReports : Problem<int, int>
{
	private int[][] _data = [];

	private static bool CheckIncrease(int l) => l >= 1 && l <= 3;
	private static bool CheckDecrease(int l) => l <= -1 && l >= -3;

	public override void CalculatePart1()
	{
		var reportAnalysis = Analyze();
		var increasing = reportAnalysis.Count(r => r.All(CheckIncrease));
		var decreasing = reportAnalysis.Count(r => r.All(CheckDecrease));
		Part1 = increasing + decreasing;
	}

	public override void CalculatePart2()
	{
		Console.WriteLine();
		foreach (var report in _data)
		{
			if (CheckSafety(report, out _))
				Part2++;
		}
	}

	private static bool CheckSafety(int[] report, out bool increase)
	{
		increase = false;
		var inFail = Check(report, CheckIncrease);
		var deFail = Check(report, CheckDecrease);

		if(inFail.Count == 0)
		{
			increase = true;
			return true;
		}
		else
		{
			if (inFail.Count < report.Length && RemoveFails(report, inFail).Any(g => Check(g, CheckIncrease).Count == 0))
			{
				increase = true;
				return true;
			}
		}
		if(deFail.Count == 0)
			return true;
		else
		{
			if (deFail.Count < report.Length && RemoveFails(report, deFail).Any(g => Check(g, CheckDecrease).Count == 0))
				return true;
		}
		return false;
	}

	static int[][] RemoveFails(int[] report, List<int> fails)
	{
		var results = new int[fails.Count][];
		for (int i = 0; i < fails.Count; i++)
		{
			var f = fails[i];
			results[i] = f == 0 ? report[1..] : f == report.Length-1 ? report[..^1] : [.. report[..f], .. report[(f+1)..]];
		}
		return results;
	}

	static List<int> Check(int[] report, Func<int, bool> check)
	{
		var fails = new List<int>();
		for (int i = 1; i < report.Length; i++)
		{
			var d = report[i] - report[i-1];
			if (!check(d))
				fails.AddRange([i, i-1]);
		}

		return fails.Distinct().ToList();
	}

	private int[][] Analyze()
	{
		var reportAnalysis = new int[_data.Length][];
		for (int ridx = 0; ridx < _data.Length; ridx++)
		{
			int[]? report = _data[ridx];
			var safety = new int[report.Length - 1];
			for (int i = 1; i < report.Length; i++)
				safety[i - 1] = report[i] - report[i - 1];
			reportAnalysis[ridx] = safety;
		}
		return reportAnalysis;
	}

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();
	}
}
