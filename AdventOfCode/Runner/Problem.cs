using AdventOfCode.Runner.Attributes;

using System.Reflection;

namespace AdventOfCode.Runner;

public interface IProblem
{
	void LoadInput();

	void CalculatePart1();

	void PrintPart1();

	void CalculatePart2();

	void PrintPart2();
}

public abstract class Problem : Problem<string, string>
{
}

public abstract class Problem<TPart1, TPart2> : IProblem
{
	protected TPart1? Part1 { get; set; }
	protected TPart2? Part2 { get; set; }

	public abstract void LoadInput();

	public abstract void CalculatePart1();

	public virtual void PrintPart1()
	{
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
			Console.ForegroundColor = ConsoleColor.DarkYellow;
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
			Console.ForegroundColor = ConsoleColor.DarkYellow;
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