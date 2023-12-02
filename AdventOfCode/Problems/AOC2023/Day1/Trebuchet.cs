using AdventOfCode.Runner.Attributes;

using Superpower.Parsers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day1;

[ProblemInfo(2023, 1, "Trebuchet!?")]
public partial class Trebuchet : Problem<int, int>
{
	private string[] _inputData = Array.Empty<string>();

	public override void LoadInput()
	{
		_inputData = ReadInputLines();
	}
	public override void CalculatePart1()
	{
		Part1 = _inputData.Select(GetCalibrationValues)
			.Select(cv => cv.left * 10 + cv.right)
			.Sum();
	}

	private (int left, int right) GetCalibrationValues(string line)
	{
		var (left, right) = (0, 0);
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] - '0' >= 10)
				continue;
			left = line[i] - '0';
			break;
		}
		for (int i = line.Length - 1; i >= 0; i--)
		{
			if (line[i] - '0' >= 10)
				continue;
			right = line[i] - '0';
			break;
		}
		return (left, right);
	}

	private readonly (string word, int value)[] numberWords = new []
	{
		 ("one", 1),
		 ("two", 2),
		 ("three", 3),
		 ("four", 4),
		 ("five", 5),
		 ("six", 6),
		 ("seven", 7),
		 ("eight", 8),
		 ("nine", 9)
	};

	public override void CalculatePart2()
	{
		Part2 = _inputData.Select(GetNamedCalibrationValues)
				.Select(cv => cv.left * 10 + cv.right)
				.Sum();
	}

	private (int left, int right) GetNamedCalibrationValues(string line)
	{
		var (left, right) = (0, 0);
		for (int i = 0; i < line.Length; i++)
		{
			var word = numberWords.FirstOrDefault(v => line[i..].StartsWith(v.word), (word: "", value: -1)).value;
			if (word != -1)
			{
				left = word;
				break;
			}else if(line[i] - '0' >= 10)
				continue;

			left = line[i] - '0';
			break;
		}
		for (int i = line.Length - 1; i >= 0; i--)
		{
			var word = numberWords.FirstOrDefault(v => line[..(i + 1)].EndsWith(v.word), (word: "", value: -1)).value;
			if (word != -1)
			{
				right = word;
				break;
			}
			else if (line[i] - '0' >= 10)
				continue;

			right = line[i] - '0';
			break;
		}
		return (left, right);
	}


	[GeneratedRegex("(?<1>one)|(?<2>two)|(?<3>three)|(?<4>four)|(?<5>five)|(?<6>six)|(?<7>seven)|(?<8>eight)|(?<9>nine)|(?<1>1)|(?<2>2)|(?<3>3)|(?<4>4)|(?<5>5)|(?<6>6)|(?<7>7)|(?<8>8)|(?<9>9)")]
	public static partial Regex ParseNumbers();

}
