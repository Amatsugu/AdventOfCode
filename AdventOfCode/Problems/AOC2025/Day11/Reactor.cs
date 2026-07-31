namespace AdventOfCode.Problems.AOC2025.Day11;

[ProblemInfo(2025, 11, nameof(Reactor))]
internal class Reactor : Problem<long, long>
{
	private readonly Dictionary<string, string[]> _routes = [];

	public override void CalculatePart1()
	{
		Part1 = FindPath("you");
	}

	private int FindPath(string src, bool dac = true, bool fft = true) => FindPath(src, []);
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

	private int FindDacFftPath(string src) => FindDacFftPath(src, []).Item1;
	private (int sum, int outs) FindDacFftPath(string src, Dictionary<string, int> cache, bool dac = false, bool fft = false)
	{
		if (src == "out")
			return (dac && fft ? 1 : 0, 1);
		if (dac && fft && cache.TryGetValue(src, out var c))
			return (c, c);

		var sum = (0, 0);
		foreach (var dst in _routes[src])
		{
			var res = FindDacFftPath(dst, cache, dac || src == "dac", fft || src == "fft");
			sum.Item1 += res.sum;
			sum.Item2 += res.outs;
		}

		cache.TryAdd(src, sum.Item2);

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