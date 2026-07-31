using MathNet.Numerics.LinearAlgebra.Solvers;

namespace AdventOfCode.Problems.AOC2025.Day11;

[ProblemInfo(2025, 11, nameof(Reactor))]
internal class Reactor : Problem<long, long>
{
	private readonly Dictionary<string, string[]> _routes = [];

	public override void CalculatePart1()
	{
		Part1 = FindPath("you");
	}

	private int FindPath(string src) => FindPath(src, []);
	private int FindPath(string src, Dictionary<string, int> cache)
	{
		if (src == "out")
			return 1;

		if (cache.TryGetValue(src, out var c))
			return c;

		var sum = _routes[src].Sum(d =>
		{
			var s = FindPath(d, cache);
			return s;
		});
		cache.Add(src, sum);

		return sum;
	}

	public override void CalculatePart2()
	{
		Part2 = FindDacFftPath("svr");
	}

	private long FindDacFftPath(string src) => FindDacFftPath(src, []);
	private long FindDacFftPath(string src, Dictionary<(string, bool, bool), long> cache, bool dac = false, bool fft = false)
	{
		if (src == "out")
			return (dac && fft) ? 1 : 0;

		if (cache.TryGetValue((src, dac, fft), out var c))
			return c;

		var sum = _routes[src].Sum(dst => FindDacFftPath(dst, cache, dac || dst == "dac", fft || dst == "fft"));
		

		cache.TryAdd((src, dac, fft), sum);

		return sum;
	}

	public override void LoadInput()
	{
		foreach (var line in ReadInputLines("input.txt"))
		{
			var mapping = line.Split(": ");
			var values = mapping[1].Split(' ');
			_routes.Add(mapping[0], values);
		}
	}
}