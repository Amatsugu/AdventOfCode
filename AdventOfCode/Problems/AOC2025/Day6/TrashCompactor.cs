using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.AOC2025.Day6;
[ProblemInfo(2025, 6, "Trash Compactor")]
public partial class TrashCompactor : Problem<long, long>
{
	private long[][] _values = [];
	private string[] _operators = [];
	private IEnumerable<(char op, long[] values)> _part2Data = [];

	public override void CalculatePart1()
	{
		for (int i = 0; i < _operators.Length; i++)
		{
			var op = _operators[i];
			var col = _values.Select(r => r[i]).ToArray();
			Part1 += op switch
			{
				"+" => col.Aggregate((a, b) => a + b),
				"*" => col.Aggregate((a, b) => a * b),
				_ => throw new InvalidOperationException()
			};
		}
	}

	public override void CalculatePart2()
	{
		foreach (var item in _part2Data)
		{
			Part2 += item.op switch
			{
				'+' => item.values.Aggregate((a, b) => a + b),
				'*' => item.values.Aggregate((a, b) => a * b),
				_ => throw new InvalidOperationException()
			};
		}
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		ParsePart1(lines);
		ParsePart2(lines);
	}

	private void ParsePart1(string[] lines)
	{
		_values = lines[..^1].Select(l => LineMatch().Matches(l).Select(v => long.Parse(v.Value)).ToArray()).ToArray();
		_operators = LineMatch().Matches(lines[^1]).Select(v => v.Value).ToArray();
	}

	private void ParsePart2(string[] lines)
	{
		var valueLines = lines[..^1];
		var opLines = lines[^1];

		var opPos = 0;
		var len = 1;

		var data = new List<(char op, string[] values)>();

		for (int i = 1; i < opLines.Length; i++)
		{
			var curChar = opLines[i];
			if (curChar != ' ' || i == opLines.Length - 1)
			{
				if (i == opLines.Length - 1)
					len = opLines.Length - opPos + 1;
				var op = opLines[opPos];
				var values = valueLines.Select(v => v[opPos..(opPos + len - 1)]).ToArray();
				data.Add((op, values));

				len = 1;
				opPos = i;
			}
			else
				len++;
		}

		_part2Data = data.Select(v => (v.op, v.values.Transpose().Select(long.Parse).ToArray()));
	}

	[GeneratedRegex(@"(\S+)")]
	private static partial Regex LineMatch();
}
