﻿using AdventOfCode.Runner;
using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day2;

[ProblemInfo("2022", 2, "Rock Paper Scissors")]
internal class RockPaperScissors : IProblem
{
	private string[] _lines;
	private int _part1Score;
	private int _part2Score;

	public RockPaperScissors()
	{
		_lines = Array.Empty<string>();
	}

	public void LoadInput()
	{
		_lines = File.ReadAllLines("Problems/AOC2022/Day2/input.txt");
	}

	public void CalculatePart1()
	{
		var totalScore = 0;
		foreach (var line in _lines)
		{
			var move = line[0];
			var response = line[^1];

			totalScore += GetMoveValue(response);
			totalScore += GetResult(move, response);
		}
		_part1Score = totalScore;
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

	private static int GetResponseValue(char move, char result) {
		var p = result switch
		{
			'X' => (GetMoveValue(move) + 2) % 3, //Lose
			'Y' => GetMoveValue(move), //Tie
			'Z' => (GetMoveValue(move) + 1) % 3, //Win
			_ => 0
		};
		return p == 0 ? 3 : p;
	}

	
	public void CalculatePart2()
	{
		var score = 0;
		foreach (var line in _lines)
		{
			var move = line[0];
			var result = line[^1];
			score += GetResponseValue(move, result);
			score += GetResultValue(result);
		}
		_part2Score = score;
	}

	public void PrintPart1()
	{
		Console.WriteLine($"P1: {_part1Score}");
	}

	public void PrintPart2()
	{
		Console.WriteLine($"P2: {_part2Score}");
	}
}