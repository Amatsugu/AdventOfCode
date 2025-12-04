using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

using ZLinq;

namespace AdventOfCode.Problems.AOC2025.Day2;

[ProblemInfo(2025, 2, "Gift Shop")]
internal class GiftShop : Problem<long, long>
{
	private IdRange[] _ranges = [];
	public override void CalculatePart1()
	{
		var v = _ranges.SelectMany(GetDoubleSequences);
		//Console.WriteLine(v.AsJoinedString());
		Part1 = v.Sum();
	}

	public static long[] GetDoubleSequences(IdRange range)
	{
		range = range.Snap();
		var minDigits = range.Min.DigitCount() / 2;
		var maxDigits = range.Max.DigitCount() / 2;

		var min = GetMinValue((int)minDigits, range.Min);
		var max = GetMaxValue((int)maxDigits, range.Max);
		//Console.WriteLine($"{min}-{max}");
		if (max < min)
			return [];
		var n = (max - min) + 1;
		var result = new long[n];
		for (long i = min; i <= max; i++)
		{
			result[i - min] = (i * QuickMath.FastPow10(minDigits)) + i;
		}
		return result;
	}

	public static long SnapToUpNearestValidRange(long value)
	{
		var dc = value.DigitCount();
		if (dc.IsEven())
			return value;
		return QuickMath.FastPow10(dc);
	}
	public static long SnapToDownNearestValidRange(long value)
	{
		var dc = value.DigitCount();
		if (dc.IsEven())
			return value;
		return QuickMath.FastPow10(dc - 1) - 1;
	}

	public static long GetMinValue(int digits, long value)
	{
		var val = long.Parse(value.ToString()[..^digits]);
		while ((val * QuickMath.FastPow10(digits)) + val < value)
		{
			val++;
		}
		return val;
	}

	public static long GetMaxValue(int digits, long value)
	{
		var val = long.Parse(value.ToString()[..^digits]);
		while ((val * QuickMath.FastPow10(digits)) + val > value)
		{
			val--;
		}
		return val;
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		var text = ReadInputText("input.txt");
		_ranges = text.Split(',')
			.AsValueEnumerable()
			.Select(r => r.Split('-').Select(long.Parse))
			.Select(r => new IdRange(r.First(), r.Last()))
			.ToArray();
	}

	public record IdRange(long Min, long Max)
	{
		public IdRange Snap()
		{
			return new IdRange(SnapToUpNearestValidRange(Min), SnapToDownNearestValidRange(Max));
		}
	}
}
