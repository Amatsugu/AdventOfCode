using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day6;

[ProblemInfo(2019, 6, "Universal Orbit Map")]
internal class UniversalOrbits : Problem<int, int>
{
	private OrbitMap? _map;

	public override void CalculatePart1()
	{
		if (_map == null)
			return;
		Part1 = _map.CalculateOrbits();
	}

	public override void CalculatePart2()
	{
		if (_map == null)
			return;

		var pathToYOU = _map.FindPathTo("YOU");
		var pathToSAN = _map.FindPathTo("SAN");
		string pivot = "";
		int dist = 0;

		HashSet<string> pathYOU = new(pathToYOU.Select(o => o.Name));

		for (int i = 0; i < pathToSAN.Count; i++)
		{
			if (pathYOU.Contains(pathToSAN[i].Name))
			{
				pivot = pathToSAN[i].Name;
				dist = i;
				break;
			}
		}
		for (int i = 0; i < pathToYOU.Count; i++)
		{
			if (pathToYOU[i].Name == pivot)
			{
				dist += i;
				break;
			}
		}
		Part2 = dist - 2;
	}

	public override void LoadInput()
	{
		_map = new OrbitMap(ReadInputLines());
	}
}