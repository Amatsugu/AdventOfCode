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
		Part1 = v.Sum();
	}

	public override void CalculatePart2()
	{
		Part2 = _ranges.Sum(GetRepeatedSequencesSum);
	}


	public long GetRepeatedSequencesSum(IdRange range)
	{
		HashSet<long> values = [
			.. GetCandidateSequences(range, range.Min),
			.. GetCandidateSequences(range, range.Max)
			];
		Console.WriteLine($"{range}: {values.AsJoinedString()}");
		return values.Sum();
	}

	private static List<long> GetCandidateSequences(IdRange range, long baseNumber)
	{
		var results = new List<long>();
		var digits = (int)baseNumber.DigitCount();
		var baseVal = baseNumber.ToString()[0..(digits/2)];
		while(baseVal.Length != 0)
		{
			var seqNum = long.Parse(baseVal);
			while (seqNum.DigitCount() == baseVal.Length)
			{
				var groups = $"{seqNum}";
				while (true)
				{
					groups = $"{groups}{seqNum}";
					var seq = long.Parse(groups);
					if (seq < range.Min)
						continue;
					if (seq > range.Max)
						break;
					results.Add(seq);
				}
				seqNum++;
			}
			baseVal = baseVal[0..^1];
		}
		return results;
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
