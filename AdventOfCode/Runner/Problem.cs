﻿using AdventOfCode.Runner.Attributes;

using System.Reflection;

namespace AdventOfCode.Runner;

public abstract class Problem
{
	protected string? Part1 { get; set; }
	protected string? Part2 { get; set; }

	public abstract void LoadInput();

	public abstract void CalculatePart1();

	public virtual void PrintPart1() {
		Console.ForegroundColor = ConsoleColor.Gray;
		if (Part1 == null)
		{
			Console.Write("Part 1: ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No Solution");
		}
		else
		{
			Console.Write("Part 1: ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"{Part1}");
		}
		Console.ForegroundColor = ConsoleColor.Gray;
	}

	public abstract void CalculatePart2();

	public virtual void PrintPart2()
	{
		Console.ForegroundColor = ConsoleColor.Gray;
		if (Part2 == null)
		{
			Console.Write("Part 2: ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No Solution");
		}
		else
		{
			Console.Write("Part 2: ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"{Part2}");
		}
		Console.ForegroundColor = ConsoleColor.Gray;
	}

	protected string GetInputFile(string filename = "input.txt")
	{
		var info = this.GetType().GetCustomAttribute<ProblemInfoAttribute>();
		if (info == null)
			return filename;

		return Path.Combine($"Problems/AOC{info.Year}/Day{info.Day}", filename);
	}

	protected string[] ReadInputLines(string filename = "input.txt")
	{
		return File.ReadAllLines(GetInputFile(filename));
	}

	protected string ReadInputText(string filename = "input.txt")
	{
		return File.ReadAllText(GetInputFile(filename));
	}
}