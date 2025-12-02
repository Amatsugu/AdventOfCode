using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day4;

[ProblemInfo(2022, 4, "Camp Cleanup")]
internal class CampCleanup : Problem<int, int>
{
	private List<(Range a, Range b)> _pairs = new(500);

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		foreach (var line in lines)
		{
			var (a, b) = line.Split(',')
				.Select(range =>
					range.Split('-')
					.Select(v => int.Parse(v))
					.Chunk(2)
					.Select(r => new Range(r.First(), r.Last()))
					.First()
				).Chunk(2)
				 .Select(pair => (pair.First(), pair.Last()))
				 .First();
			_pairs.Add((a, b));
		}
	}

	public override void CalculatePart1()
	{
		var total = 0;
		foreach (var (a, b) in _pairs)
		{
			if (a.Contains(b) || b.Contains(a))
				total++;
		}
		Part1 = total;
	}

	public override void CalculatePart2()
	{
		foreach (var (a, b) in _pairs)
		{
			if (a.OverlapsWith(b))
				Part2++;
		}
	}

	record Range(int A, int B)
	{
		public bool Contains(Range other)
		{
			return (A <= other.A && B >= other.B);
		}

		public bool OverlapsWith(Range other)
		{
			return (B >= other.A && A <= other.A) || (A <= other.B && B >= other.B) || (A >= other.A && B <= other.B);
		}

		public static implicit operator Range((int a, int b) value) => new(value.a, value.b);
	}
}