using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day11;

[ProblemInfo(2022, 11, "Monkey in the Middle")]
internal class MonkeyInTheMiddle : Problem<int, int>
{
	private Monkey[] _monkeysPart1 = Array.Empty<Monkey>();
	private Monkey[] _monkeysPart2 = Array.Empty<Monkey>();

	public override void CalculatePart1()
	{
		Simulate(_monkeysPart1, 20);
		Part1 = _monkeysPart1.OrderByDescending(m => m.InspectionCount)
			.Take(2)
			.Select(m => m.InspectionCount)
			.Aggregate((a, b) => a * b);
	}

	public override void CalculatePart2()
	{
		Simulate(_monkeysPart2, 10000, 0);
		Part2 = _monkeysPart2.OrderByDescending(m => m.InspectionCount)
			.Take(2)
			.Select(m => m.InspectionCount)
			.Aggregate((a, b) => a * b);
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("test.txt").Chunk(7);
		_monkeysPart1 = lines.Select(ln => new Monkey(ln)).ToArray();
		_monkeysPart2 = lines.Select(ln => new Monkey(ln)).ToArray();
	}

	private static void Simulate(Monkey[] monkeys, int rounds, uint worry = 3)
	{
		for (int i = 0; i < rounds; i++)
		{
			SimulateRound(monkeys, worry);
		}
	}

	private static void SimulateRound(Monkey[] monkeys, uint worry = 3)
	{
		foreach (var monkey in monkeys)
			SimulateTurn(monkey, monkeys, worry);
	}

	private static void SimulateTurn(Monkey monkey, Monkey[] monkeys, uint worry = 3)
	{
		for (int i = 0; i < monkey.Items.Count; i++)
		{
			var item = monkey.Inspect(monkey.Items[i], worry);
			var target = monkey.GetThrowTarget(item);
			monkeys[target].Items.Add(item);
		}
		monkey.Items.Clear();
	}
}