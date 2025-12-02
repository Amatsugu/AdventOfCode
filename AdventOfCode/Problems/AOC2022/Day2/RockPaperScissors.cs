using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day2;

[ProblemInfo(2022, 2, "Rock Paper Scissors")]
internal class RockPaperScissors : Problem
{
	private string[] _lines;

	public RockPaperScissors()
	{
		_lines = Array.Empty<string>();
	}

	public override void LoadInput()
	{
		_lines = File.ReadAllLines(GetInputFile("input.txt"));
	}

	public override void CalculatePart1()
	{
		var totalScore = 0;
		foreach (var line in _lines)
		{
			var move = line[0];
			var response = line[^1];

			totalScore += GetMoveValue(response);
			totalScore += GetResult(move, response);
		}
		Part1 = totalScore.ToString();
	}

	private static int GetMoveValue(char move)
	{
		return move switch
		{
			'A' => 1,
			'B' => 2,
			'C' => 3,
			'X' => 1,
			'Y' => 2,
			'Z' => 3,
			_ => 0,
		};
	}

	private static int GetResult(char move, char response)
	{
		return (move, response) switch
		{
			('A', 'X') => 3,
			('B', 'X') => 0,
			('C', 'X') => 6,
			('A', 'Y') => 6,
			('B', 'Y') => 3,
			('C', 'Y') => 0,
			('A', 'Z') => 0,
			('B', 'Z') => 6,
			('C', 'Z') => 3,
			_ => 0,
		};
	}

	private static int GetResultValue(char result)
	{
		return result switch
		{
			'X' => 0,
			'Y' => 3,
			'Z' => 6,
			_ => 0,
		};
	}

	private static int GetResponseValue(char move, char result)
	{
		var p = result switch
		{
			'X' => (GetMoveValue(move) + 2) % 3, //Lose
			'Y' => GetMoveValue(move), //Tie
			'Z' => (GetMoveValue(move) + 1) % 3, //Win
			_ => 0
		};
		return p == 0 ? 3 : p;
	}

	public override void CalculatePart2()
	{
		var score = 0;
		foreach (var line in _lines)
		{
			var move = line[0];
			var result = line[^1];
			score += GetResponseValue(move, result);
			score += GetResultValue(result);
		}
		Part2 = score.ToString();
	}
}