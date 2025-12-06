using System;
using System.Collections.Generic;
using System.Text;

using ZLinq;

namespace AdventOfCode.Problems.AOC2025.Day5;
[ProblemInfo(2025, 5, "Cafeteria")]
internal class Cafeteria : Problem<long, long>
{
	private (long start, long end)[] _ranges = [];
	private long[] _values = [];

	public override void CalculatePart1()
	{
		Part1 = _values.AsValueEnumerable().Count(v => _ranges.AsValueEnumerable().Any(r => IsInRange(r, v)));
	}

	public static bool IsInRange((long start, long end) range, long value)
	{
		return (range.start <= value && range.end >= value);
	}

	public override void CalculatePart2()
	{
		var merged = MergeRanges(_ranges);
		merged.Print();
		Console.WriteLine("----");
		MergeRanges(merged.ToArray()).Print();
		//merged.Print();
		Part2 = merged.Select(r => r.end - r.start + 1).Sum();
	}

	public static List<(long start, long end)> MergeRanges((long start, long end)[] ranges)
	{
		var result = new List<(long start, long end)>(ranges.Length);
		var used = new HashSet<int>();
		for (int i = 0; i < ranges.Length; i++)
		{
			if (used.Contains(i))
				continue;
			var range = ranges[i];

			for (int j = (i + 1); j < ranges.Length; j++)
			{
				if (used.Contains(j))
					continue;
				var range2 = ranges[j];
				if(IsOverlapping(range, range2))
				{
					range = Merge(range, range2);
					used.Add(j);
					j = i;
				}
			}

			result.Add(range);
		}
		return result;
	}

	public static bool IsOverlapping((long start, long end) a, (long start, long end) b)
	{
		return IsInRange(a, b.start) || IsInRange(a, b.end) || IsInRange(b, a.start) || IsInRange(b, a.end);
	}

	public static (long start, long end) Merge((long start, long end) a, (long start, long end) b)
	{
		return (a.start.Min(b.start), a.end.Max(b.end));
	}


	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_ranges = lines
			.TakeWhile(l => !string.IsNullOrWhiteSpace(l))
			.Select(l => l.Split('-').Select(long.Parse))
			.Select(v => (start: v.First(), end: v.Last()))
			.ToArray();
		_values = lines[(_ranges.Length + 1)..]
			.Select(long.Parse)
			.ToArray();
	}
}
