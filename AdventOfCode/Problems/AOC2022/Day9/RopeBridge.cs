using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day9;

[ProblemInfo(2022, 9, "Rope Bridge")]
internal class RopeBridge : Problem<int, int>
{
	private (RopeSimulator.Direction, int)[] _moves = Array.Empty<(RopeSimulator.Direction, int)>();

	public override void CalculatePart1()
	{
		var sim = new RopeSimulator(_moves);
		sim.Simulate();
		Part1 = sim.Visited;
	}

	public override void CalculatePart2()
	{
		var sim = new RopeSimulator(_moves, 9);
		sim.Simulate();
		Part2 = sim.Visited;
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_moves = lines.Select(ln => ln.Split(' '))
			.Select(move => (Enum.Parse<RopeSimulator.Direction>(move.First()), int.Parse(move.Last())))
			.ToArray();
	}
}