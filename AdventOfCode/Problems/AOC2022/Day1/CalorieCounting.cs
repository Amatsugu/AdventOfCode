using AdventOfCode.Runner;
using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2022.Day1;
[ProblemInfo("2022", 1, "Calorie Counting")]
internal class CalorieCounting : IProblem
{
	public List<List<int>> FlaresFood { get; set; }

	private (int calories, int elf)? _mostestElf;
	private IEnumerable<(int sum, int idx)>? _mostestElves;

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
        _mostestElf = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.MaxBy(x => x.sum);
    }

    public void CalculatePart2()
    {
        _mostestElves = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.OrderByDescending(e => e.sum)
			.Take(3);
    }


    public void PrintPart1()
    {
		if (_mostestElf == null)
		{
			Console.WriteLine("Part 1 has not been calculated");
			return;
		}
		Console.WriteLine($"Mostest: {_mostestElf}");
    }

    public void PrintPart2()
    {
		if(_mostestElves == null)
		{
			Console.WriteLine("Part 2 has not been calculated");
			return;
		}
		Console.WriteLine("Top Elves");
		foreach (var elf in _mostestElves)
			Console.WriteLine($"\t{elf}");
		Console.WriteLine($"Total {_mostestElves.Sum(e => e.sum)}");
    }
}
