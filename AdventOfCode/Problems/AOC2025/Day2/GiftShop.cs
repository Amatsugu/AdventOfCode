using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

using ZLinq;

namespace AdventOfCode.Problems.AOC2025.Day2;

[ProblemInfo(2025, 2, "Gift Shop")]
internal class GiftShop : Problem<long, long>
{
	private IdRange[] _ranges = [];
	public override void CalculatePart1()
	{
		throw new NotImplementedException();
	}

	public static List<long> GetDoubleSequences(IdRange range)
	{
		var minDigits = range.Min.DigitCount() / 2;
		var maxDigits = range.Max.DigitCount() / 2;
		throw new NotImplementedException();
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		var text = ReadInputText("sample.txt");
		_ranges = text.Split(',')
			.AsValueEnumerable()
			.Select(r => r.Split('-').Select(int.Parse))
			.Select(r => new IdRange(r.First(), r.Last()))
			.ToArray();
	}

	public record IdRange(int Min, int Max);
}
