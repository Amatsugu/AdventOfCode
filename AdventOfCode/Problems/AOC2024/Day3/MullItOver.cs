using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day3;
[ProblemInfo(2024, 3, "Mull It Over")]
internal partial class MullItOver : Problem<int, int>
{
	private string _data = string.Empty;

	public override void CalculatePart1()
	{
		var matches = Mul().Matches(_data);
		Part1 = matches.Select(m => (int.Parse(m.Groups["a"].ValueSpan), int.Parse(m.Groups["b"].ValueSpan))).Select(v => v.Item1 * v.Item2).Sum();
	}

	public override void CalculatePart2()
	{
		var doing = true;
		var muls = DosAndDonts().Matches(_data);
		foreach (Match match in muls)
		{
			switch (match.ValueSpan)
			{
				case ['d', 'o', 'n', ..]:
					doing = false;
					break;
				case ['d', 'o', ..]:
					doing = true;
					break;
				default:
					if (!doing)
						continue;
					Part2 += int.Parse(match.Groups["a"].ValueSpan) * int.Parse(match.Groups["b"].ValueSpan);
					break;
			}
		}
	}

	public override void LoadInput()
	{
		 _data = ReadInputText("input.txt");
	}

	[GeneratedRegex("(mul\\((?<a>\\d+)\\,(?<b>\\d+)\\))|(do\\(\\))|(don't\\(\\))")]
	private static partial Regex DosAndDonts();
	[GeneratedRegex("mul\\((?<a>\\d+)\\,(?<b>\\d+)\\)")]
	private static partial Regex Mul();
}
