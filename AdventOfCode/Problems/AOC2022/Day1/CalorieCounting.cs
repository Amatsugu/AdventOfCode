using AdventOfCode.Runner;
using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2022.Day1;
[ProblemInfo(2022, 1, "Calorie Counting")]
internal class CalorieCounting : Problem
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

	public override void LoadInput()
    {
        var lines = File.ReadAllLines(GetInputFile("input.txt"));
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
    public override void CalculatePart1()
    {
        _mostestElf = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.MaxBy(x => x.sum);

		Part1 = _mostestElf.Value.ToString();
    }

    public override void CalculatePart2()
    {
        _mostestElves = FlaresFood
			.Select((x, idx) => (sum: x.Sum(), idx))
			.OrderByDescending(e => e.sum)
			.Take(3);
		Part2 = _mostestElves.Sum(e => e.sum).ToString();
	}
}
