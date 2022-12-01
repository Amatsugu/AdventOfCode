using AdventOfCode.Runner.Attributes;

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
		var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.IsAssignableTo(typeof(IProblem)) && !t.IsInterface);
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

		Console.WriteLine("Available Problems:");
		foreach (var year in years)
			RenderYearMenu(year);	
	}

	private void RenderYearMenu(string year)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"{year}:");
		Console.ForegroundColor = ConsoleColor.Gray;
		var days = _loadedProblems[year];
		for (int i = 0; i < days.Count; i++)
		{
			var (info, type) = days[i];
			Console.WriteLine($"\tDay {info.Day} - {info.Name}");
		}
	}
}