using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day1
{
	[ProblemInfo(2019, 1, "The Tyranny of the Rocket Equation")]
	public class FuelCaluclation : Problem<int, int>
	{
		private int[] _input = Array.Empty<int>();

		public static int GetFuelRequirement(int[] input)
		{
			var curFuel = input.Sum(i => GetFuelCost(i));
			return curFuel;
		}

		public static int GetFuelCost(int mass)
		{
			var curCost = mass / 3 - 2;
			if (curCost <= 0)
				return 0;
			return curCost + GetFuelCost(curCost);
		}

		public static int GetCost(int mass) => mass / 3 - 2;

		public override void LoadInput()
		{
			_input = InputParsing.ParseIntArray(GetInputFile());
		}

		public override void CalculatePart1()
		{
			Part1 = _input.Sum(i => GetCost(i));
		}

		public override void CalculatePart2()
		{
			Part2 = GetFuelRequirement(_input);
		}
	}
}