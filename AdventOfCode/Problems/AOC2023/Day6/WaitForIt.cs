using AdventOfCode.Runner.Attributes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day6;
[ProblemInfo(2023, 6, "Wait For It")]
internal class WaitForIt : Problem<int, long>
{
	private int[] _times = [];
	private int[] _distances = [];
	private int _realTime;
	private long _realDistance;

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_times = lines[0].Split(':')[1]
			.Split(' ')
			.Where(e => e.Length > 0)
			.Select(int.Parse)
			.ToArray();

		_distances = lines[1].Split(':')[1]
			.Split(' ')
			.Where(e => e.Length > 0)
			.Select(int.Parse)
			.ToArray();

		_realTime = int.Parse(lines[0].Split(":")[1].Replace(" ", ""));
		_realDistance = long.Parse(lines[1].Split(":")[1].Replace(" ", ""));
	}

	public override void CalculatePart1()
	{
		var winList = new List<int>();
		for (int i = 0; i < _times.Length; i++)
		{
			var time = _times[i];
			var distance = _distances[i];
			var minTime = (int)Math.Floor((float)distance / time);
			var possibleHeldTimes = Enumerable.Range(minTime, time - minTime);
			var races = possibleHeldTimes.Select(t => (time - t) * t);
			winList.Add(races.Count(d => d > distance));
		}
		Part1 = winList.Aggregate((a, b) => a * b);
	}

	public override void CalculatePart2()
	{
		var minTime = (long)Math.Floor((float)_realDistance/ _realTime);
		var maxTime = _realTime - minTime;
		for (long i = minTime; i <= maxTime; i++)
		{
			var dist = (_realTime - i) * i;
			if(dist > _realDistance)
				Part2++;
		}
	}

}
