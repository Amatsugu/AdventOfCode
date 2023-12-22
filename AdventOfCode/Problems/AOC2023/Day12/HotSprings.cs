using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day12;

[ProblemInfo(2023, 12, "Hot Springs")]
internal class HotSprings : Problem<int, int>
{
	private Record[] _data = [];

	public override void CalculatePart1()
	{
		Part1 = _data.Select(r => CountPossiblilites(r)).Sum();
	}

	public override void CalculatePart2()
	{
		var unfolded = _data.Select(r =>
		{
			var unfoldData = string.Join("", Enumerable.Repeat(r.Data, 5));
			var unfoldPattern = Enumerable.Repeat(r.Pattern, 5).SelectMany(x => x).ToArray();
			return new Record(unfoldData, unfoldPattern);
		}).ToArray();
		Part2 = unfolded.Select(r => CountPossiblilites(r)).Sum();
	}

	public static int CountPossiblilites(Record record, int pos = 0)
	{
		if (pos == -1 || pos >= record.Data.Length)
			return record.IsValid ? 1 : 0;
		if (!record.IsValid)
			return 0;
		if (record.Data[pos] != '?')
			return CountPossiblilites(record, record.Data.IndexOf('?'));

		var front = record.Data[..pos];
		var back = record.Data[(pos + 1)..];
		var r1 = record with { Data = $"{front}.{back}" };
		var r2 = record with { Data = $"{front}#{back}" };
		return CountPossiblilites(r1, pos + 1) + CountPossiblilites(r2, pos + 1);
	}

	public override void LoadInput()
	{
		var data = ReadInputLines();

		_data = data.Select(x => x.Split(' '))
			.Select(x => new Record(x[0], x[1].Split(',').Select(int.Parse).ToArray()))
			.ToArray();
	}

	public record Record(string Data, int[] Pattern)
	{
		public bool IsValid => CheckValidity(this);
		public static bool CheckValidity(Record record)
		{
			var section = 0;
			var start = 0;
			var inSection = false;
			for (int i = 0; i < record.Data.Length; i++)
			{
				var c = record.Data[i];
				if (section >= record.Pattern.Length)
					return !record.Data[i..].Contains('#');

				var len = record.Pattern[section];

				switch (c)
				{
					case '?':
						if (inSection && i - start > len)
							return false;
						return true;

					case '.':
						if (inSection)
						{
							if (i - start != len)
								return false;
							inSection = false;
							start = 0;
							section++;
						}
						continue;
					case '#':
						if (!inSection)
						{
							inSection = true;
							start = i;
						}
						break;
				}
			}
			if (inSection)
			{
				if (record.Pattern[section] != (record.Data[start..].Length))
					return false;
				else
					section++;
			}
			if (section != record.Pattern.Length)
				return false;
			return true;
		}
	};
}