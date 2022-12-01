using AdventOfCode.Runner;
using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2022.Day1;
[ProblemInfo("2022", 1, "Calorie Counting")]
internal class CalorieCounting : IProblemBase
{
	private (int calories, int elf) mostestElf;
	private IEnumerable<(int sum, int idx)> mostestElves;

	public List<List<int>> FlaresFood { get; set; }

	public CalorieCounting() {
		FlaresFood = new List<List<int>>
		{
			new List<int>()
		};
	}

	public void LoadInput()
    {
        var lines = File.ReadAllLines("Problems/AOC2022/Day1/input.txt");
		var c = 0;
		foreach (var calorie in lines)
		{
			if(string.IsNullOrWhiteSpace(calorie))
			{
				FlaresFood.Add(new List<int>());
				c++;
				continue;
			}
			FlaresFood[c].Add(int.Parse(calorie));
		}
    }
    public void CalculatePart1()
    {
        mostestElf = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.MaxBy(x => x.sum);
    }

    public void CalculatePart2()
    {
        mostestElves = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.OrderByDescending(e => e.sum)
			.Take(3);
    }


    public void PrintPart1()
    {
		Console.WriteLine($"Mostest: {mostestElf}");
    }

    public void PrintPart2()
    {
		Console.WriteLine("Top Elves");
		foreach (var elf in mostestElves)
			Console.WriteLine($"\t{elf}");
		Console.WriteLine($"Total {mostestElves.Sum(e => e.sum)}");
    }
}
