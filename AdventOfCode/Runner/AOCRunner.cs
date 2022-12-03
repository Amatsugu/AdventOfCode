using AdventOfCode.Runner.Attributes;

using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Runner;

public class AOCRunner
{
	private Dictionary<string, List<(ProblemInfoAttribute info, Type type)>> _loadedProblems;

	public AOCRunner()
	{
		_loadedProblems = new();
		FindProblemClasses();
	}

	private void FindProblemClasses()
	{
		var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.IsAssignableTo(typeof(Problem)) && !t.IsInterface);
		if (types == null)
			return;
		foreach (var type in types)
		{
			var info = type.GetCustomAttribute<ProblemInfoAttribute>();
			if (info == null)
				continue;
			if (_loadedProblems.TryGetValue(info.Year, out var list))
				list.Add((info, type));
			else
				_loadedProblems.Add(info.Year, new() { (info, type) });
		}
		foreach (var (year, list) in _loadedProblems)
			_loadedProblems[year] = list.OrderBy(l => l.info.Day).ToList();
	}

	public void RenderMenu()
	{
		var years = _loadedProblems.Keys.OrderByDescending(k => k);

		var defaultYear = DateTime.Now.Year.ToString();

		Console.WriteLine($"Select a Year: (blank is {defaultYear})");
		var inputYear = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(inputYear))
			inputYear = defaultYear;

		RenderYearMenu(defaultYear);
	}

	private void RenderYearMenu(string year)
	{
		if(!_loadedProblems.ContainsKey(year))
		{
			Console.WriteLine($"No problems for {year} exist");
			return;
		}
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"{year}:");
		Console.ForegroundColor = ConsoleColor.Gray;
		var days = _loadedProblems[year];
		for (int i = 0; i < days.Count; i++)
		{
			var (info, _) = days[i];
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.Write($"\t [{i}]");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($" - Day {info.Day} - {info.Name}");
		}

		Console.WriteLine();
		var defaultDay = DateTime.Now.Day;

		Console.WriteLine($"Select Day Index: (blank is {defaultDay})");
		var inputDay = Console.ReadLine();

		if(!int.TryParse(inputDay, out var parsedDay))
			parsedDay = defaultDay;

		RunDay(year, parsedDay);
	}

	private void RunDay(string year, int dayIndex)
	{
		var yearList = _loadedProblems[year];
		if (yearList.Count <= dayIndex || dayIndex < 0)
			Console.WriteLine($"No problem exists for day index {dayIndex}");

		Console.Clear();
		var (info, problemType) = yearList[dayIndex];

		Console.WriteLine($"Problem: \t{info.Name}");
		Console.WriteLine($"\t\t{info.Year} - {info.Day}");


		var problem = Activator.CreateInstance(problemType) as Problem;
		if(problem == null ) {
			Console.WriteLine("Failed to create problem isntance");
			return;
		}
		Console.WriteLine("Loading Input data...");
		problem.LoadInput();

		Console.WriteLine();

		RunPart("Calculating Part 1", problem.CalculatePart1);
		RunPart("Calculating Part 2", problem.CalculatePart2);

		Console.WriteLine();
		Console.WriteLine("Printing Results:");

		Console.WriteLine("---- Part 1 ----");
		problem.PrintPart1();
		Console.Write("\n\n");
		Console.WriteLine("---- Part 2 ----");
		problem.PrintPart2();
		Console.Write("\n\n");
	}

	private static void RunPart(string name, Action action)
	{
		Console.ForegroundColor = ConsoleColor.Gray;
		var sw = new Stopwatch();
		Console.Write($"{name}... ");
		try
		{
			sw.Start();
			action();
			sw.Stop();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("Done in ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
		}
		catch (Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Failed");
			Console.WriteLine(e);
		}
		finally
		{
			sw.Stop();
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}