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
		Console.WriteLine();
		Console.WriteLine(Record.CheckValidity(new Record(".###...##...", [3,2,1])));
		
		var a = CountPossiblilites(_data[5]);
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public int CountPossiblilites(Record record, int pos = 0)
	{
		if(!record.Data.Contains('?'))
			Console.WriteLine(record);
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
		var data = ReadInputLines("sample.txt");

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
				var len = section >= record.Pattern.Length - 1 ? -1 : record.Pattern[section];

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
			if (section != record.Pattern.Length)
				return false;
			if (inSection && record.Pattern[section] < (record.Data.Length - 1 - start))
				return false;
			return true;
		}
	};
}
