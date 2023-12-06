using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day5;
internal class CategoryMapper
{
	public CategoryEvaluator.Category Source { get; }
	public CategoryEvaluator.Category Destination { get; }
	public long[] DestinationStart { get; }
	public long[] DestinationEnd { get; }
	public long[] SourceStart { get; }
	public long[] SourceEnd { get; }
	public long[] RangeLength { get; }
	public long[] Gap { get; }

	public CategoryMapper(CategoryEvaluator.Category source, CategoryEvaluator.Category destination, long[] destinationStart, long[] sourceStart, long[] length)
	{
		Source = source;
		Destination = destination;
		DestinationStart = destinationStart;
		DestinationEnd =  destinationStart.Select((v, i) => v + length[i]).ToArray();
		SourceStart = sourceStart;
		SourceEnd = sourceStart.Select((v, i) => v + length[i]).ToArray();
		RangeLength = length;
		Gap = sourceStart.Select((v, i) => destinationStart[i] - v).ToArray();
	}

	public static CategoryMapper Parse(string categoryData)
	{
		var lines = categoryData.Split("\r\n");
		var names = lines[0].Split(' ')[0].Split('-');
		var source = Enum.Parse<CategoryEvaluator.Category>(names[0], true);
		var dst = Enum.Parse<CategoryEvaluator.Category>(names[^1], true);

		var mappingData = lines[1..].Select(ln => ln.Split(' ').Select(long.Parse).ToArray());
		var dStart = mappingData.Select(d => d[0]).ToArray();
		var sStart = mappingData.Select(d => d[1]).ToArray();
		var len = mappingData.Select(d => d[2]).ToArray();
		return new CategoryMapper(source, dst, dStart, sStart, len);

	}

	public long Evaluate(long value)
	{
		for (int i = 0; i < DestinationStart.Length; i++)
		{
			if (SourceStart[i] > value)
				continue;
			if (SourceEnd[i] < value)
				continue;
			return value + Gap[i];
		}
		return value;
	}

	public (long start, long end)[] Evaluate((long start, long end)[] ranges)
	{
		var result = new List<(long start, long end)>();
		for (int i = 0; i < DestinationStart.Length; i++)
		{
			result.AddRange(ranges.SelectMany(r => SubdivideRange(r, (SourceStart[i], SourceEnd[i]))));
		}
		return result.Distinct().Select(v => (Evaluate(v.start), Evaluate(v.end))).ToArray();
	}

	public static (long start, long end)[] SubdivideRange((long start, long end) source, (long start, long end) dst)
	{
		if (source.start >= dst.start && source.end <= dst.end)
			return [source];
		if(source.end < dst.end)
			return [source];
		if(source.start >  dst.start)
			return [source];
		
		if(source.start < dst.start)
			return [(source.start, dst.start - 1), (dst.start, source.end), (source.end + 1, dst.end)];
		else
			return [(dst.start, source.start - 1), (source.start, dst.end), (dst.end + 1, source.end)];
	}

	public override string ToString()
	{
		return $"{Source} -> {Destination}";
	}
}
