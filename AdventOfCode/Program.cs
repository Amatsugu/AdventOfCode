using AdventOfCode.Problems.AOC2022.Day1;
using AdventOfCode.Runner;

namespace AdventOfCode;

internal class Program
{
	static void Main(string[] args)
	{
		var runner = new AOCRunner();
		runner.RenderMenu();
		var cc = new CalorieCounting();
		cc.LoadInput();
		cc.CalculatePart1();
		cc.PrintPart1();
		cc.CalculatePart2();
		cc.PrintPart2();
	}
}
