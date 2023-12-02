using AdventOfCode.Day_5;
using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day5;

[ProblemInfo(2019, 5, "Sunny with a Chance of Asteroids")]
internal class ChanceOfAsteroids : Problem<int, int>
{
	private IntCodeV2 _cpu = new IntCodeV2();
	private int[] _baseInput = Array.Empty<int>();

	public override void CalculatePart1()
	{
		var output = new int[1];
		_cpu.ExecuteCode(_baseInput, new int[] { 1 }, output);
		Part1 = output[0];
	}

	public override void CalculatePart2()
	{
		var output = new int[1];
		_cpu.ExecuteCode(_baseInput, new int[] { 5 }, output);
		Part2 = output[0];
	}

	public override void LoadInput()
	{
		_baseInput = InputParsing.ParseIntCsv(GetInputFile("input.csv"));
	}
}