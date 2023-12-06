using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day5;
[ProblemInfo(2023, 5, "If You Give A Seed A Fertilizer")]
internal class SeedFertilizer : Problem<long, long>
{
	private long[] _seeds = [];
	private CategoryEvaluator _evaluator = new ([]);

	public override void LoadInput()
	{
		var data = ReadInputText("sample.txt");
		var sections = data.Split("\r\n\r\n");
		_seeds = sections[0].Split(": ")[1].Split(" ").Select(long.Parse).ToArray();

		var mappers = sections[1..].Select(CategoryMapper.Parse).ToArray();
		_evaluator = new CategoryEvaluator(mappers);
	}

	public override void CalculatePart1()
	{
		var evaludated = _seeds.Select(s => _evaluator.Evaluate(CategoryEvaluator.Category.Seed, s, CategoryEvaluator.Category.Location));
		Part1 = evaludated.Min();
	}

	public override void CalculatePart2()
	{
		var splitRanges = _evaluator.Evaluate(CategoryEvaluator.Category.Seed, _seeds.Chunk(2).Select(s => (s[0], s[0] + s[1])).ToArray(), CategoryEvaluator.Category.Location);
		
	}

}
