using AdventOfCode.Runner.Attributes;

using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Runner;

public class AOCRunner
{
	private Dictionary<int, List<(ProblemInfoAttribute info, Type type)>> _loadedProblems;
	private List<int> _years;
	private int _selectedYear;
	private int _selectedDay;
	private int _scollOffset = 0;
	private int _maxProblemCount;
	private bool _isQuiting;

	private bool _isProblemMode = false;

	public AOCRunner()
	{
		Console.OutputEncoding = Encoding.UTF8;
		_loadedProblems = new();
		_years = new List<int>();
		_selectedYear = DateTime.Now.Year;
		FindProblemClasses();

		if (!_loadedProblems.ContainsKey(_selectedYear))
			_selectedYear = _loadedProblems.Keys.First();

		InitSizing();
		if (_years.Count > 0)
		{
			_selectedDay = _loadedProblems[_selectedYear].Count - 1;
			ConstrainListScroll();
		}
	}

	public AOCRunner WithDay(int day)
	{
		var problem = _loadedProblems[_selectedYear].FirstOrDefault(d => d.info.Day == day);
		if (problem == default)
			throw new ArgumentException($"There are no problems have been loaded for the day '{day}' of year '{_selectedYear}'", nameof(day));

		_selectedDay = _loadedProblems[_selectedYear].IndexOf(problem);
		return this;
	}

	public AOCRunner WithYear(int year)
	{
		if (!_loadedProblems.ContainsKey(year))
			throw new ArgumentException($"There are no problems have been loaded for the year '{year}'", nameof(year));
		_selectedYear = year;
		return this;
	}

	private void InitSizing()
	{
		_maxProblemCount = Console.WindowHeight - 9;
	}

	private void ConstrainListScroll()
	{
		if (_selectedDay >= _maxProblemCount)
		{
			_scollOffset = _loadedProblems[_selectedYear].Count - _maxProblemCount;
		}
		if (_selectedDay < _scollOffset)
		{
			_scollOffset = _selectedDay;
		}
	}

	private void FindProblemClasses()
	{
		var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => !t.IsAbstract);
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
		_years = _loadedProblems.Keys.OrderDescending().ToList();
	}

	private void RunDay(int year, int dayIndex)
	{
		var yearList = _loadedProblems[year];
		if (yearList.Count <= dayIndex || dayIndex < 0)
			Console.WriteLine($"No problem exists for day index {dayIndex}");

		Console.Clear();
		var (info, problemType) = yearList[dayIndex];
		Console.Write("Problem: ");
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"\t{info.Name}");
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine($"\t\t{info.Year} - Day {info.Day}");

		Console.ForegroundColor = ConsoleColor.Gray;

		if (Activator.CreateInstance(problemType) is not IProblem problem)
		{
			Console.WriteLine("Failed to create problem isntance");
			return;
		}

		var time = ReadInput(problem);
		time += RunPart("Calculating Part 1", problem.CalculatePart1);
		time += RunPart("Calculating Part 2", problem.CalculatePart2);

		Console.WriteLine();
		Console.Write($"Total Elapsed Time: ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine($"{time.TotalMilliseconds}ms");

		Console.ForegroundColor = ConsoleColor.Gray;

		Console.WriteLine();
		Console.WriteLine("Printing Results:");

		Console.WriteLine("---- Part 1 ----");
		problem.PrintPart1();
		Console.Write("\n\n");
		Console.WriteLine("---- Part 2 ----");
		problem.PrintPart2();
		Console.Write("\n\n");
	}

	private static TimeSpan ReadInput(IProblem problem)
	{
		var sw = new Stopwatch();
		Console.WriteLine();
		Console.Write("Loading Input data... ");
		sw.Start();
		problem.LoadInput();
		sw.Stop();

		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write("Done in ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine($"{sw.Elapsed.TotalMilliseconds:n}ms");
		return sw.Elapsed;
	}

	private static TimeSpan RunPart(string name, Action action)
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
			Console.WriteLine($"{sw.Elapsed.TotalMilliseconds:n}ms");
		}
		catch (NotImplementedException)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Not Implemented");
		}
		//catch (Exception e)
		//{
		//	Console.ForegroundColor = ConsoleColor.Red;
		//	Console.WriteLine("Failed");
		//	Console.WriteLine(e);
		//}
		finally
		{
			sw.Stop();
			Console.ForegroundColor = ConsoleColor.Gray;
		}
		return sw.Elapsed;
	}

	public void RenderInteractiveMenu()
	{
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Gray;
		Console.CursorVisible = false;
		Console.Clear();
		while (!_isQuiting)
		{
			InitSizing();
			RenderTopBar();
			RenderContentView();
			ReadInput();
		}
	}

	private void ReadInput()
	{
		var input = Console.ReadKey(true);
		if (_isProblemMode)
		{
			if (input.Key is ConsoleKey.Enter or ConsoleKey.Escape)
			{
				_isProblemMode = false;
				Console.Clear();
			}
			return;
		}
		var yearIndex = _years.IndexOf(_selectedYear);
		var dayMax = _loadedProblems[_selectedYear].Count - 1;
		switch (input.Key)
		{
			case ConsoleKey.LeftArrow:
				_scollOffset = 0;
				_selectedDay = 0;
				if (yearIndex == 0)
				{
					_selectedYear = _years.Last();
					break;
				}
				_selectedYear = _years[--yearIndex];
				break;

			case ConsoleKey.RightArrow:
				_scollOffset = 0;
				_selectedDay = 0;
				if (yearIndex == _years.Count - 1)
				{
					_selectedYear = _years.First();
					break;
				}
				_selectedYear = _years[++yearIndex];
				break;

			case ConsoleKey.UpArrow:
				if (_selectedDay == 0)
				{
					_selectedDay = dayMax;
					break;
				}
				_selectedDay--;
				break;

			case ConsoleKey.DownArrow:
				if (_selectedDay == dayMax)
				{
					_selectedDay = 0;
					break;
				}
				_selectedDay++;
				break;

			case ConsoleKey.Enter:
				_isProblemMode = true;
				break;

			case ConsoleKey.Escape:
				_isQuiting = true;
				break;
		}
		ConstrainListScroll();
	}

	private void RenderTopBar()
	{
		if (_isProblemMode)
			return;
		//Render Border
		DrawBorder(1, 0, Console.WindowWidth, 3);
		Console.SetCursorPosition(Console.WindowWidth / 2 - 4, 1);
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write(" Years ");
		//Render Tabs
		RenderTabList();
	}

	private void RenderTabList()
	{
		var buttonWidth = 6;
		var tabMaxPos = Console.WindowWidth - 3;
		for (int i = 0; i < _years.Count; i++)
		{
			var year = _years[i];
			var col = (i * 7) + 2;
			var end = col + buttonWidth;
			if (end >= tabMaxPos)
				break;
			if (year == _selectedYear)
				DrawSelectedButton(year.ToString(), 2, col, buttonWidth, 1, ConsoleColor.Red, ConsoleColor.Blue);
			else
				DrawButton(year.ToString(), 2, col, buttonWidth, 1, ConsoleColor.Gray, Console.BackgroundColor);
		}
	}

	private void RenderContentView()
	{
		if (!_isProblemMode)
		{
			DrawBorder(5, 0, Console.WindowWidth, Console.WindowHeight - 5);
			Console.SetCursorPosition(Console.WindowWidth / 2 - 5, 5);
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write(" Problems ");
			RenderProblemList();
		}
		else
			RenderProblemResults();
	}

	private void RenderProblemList()
	{
		if (_loadedProblems.Count == 0)
		{
			DrawButton("There are no problems...", 6, 2, Console.WindowWidth - 2, Console.WindowHeight - 7);
			return;
		}
		var problems = _loadedProblems[_selectedYear];

		var listEnd = Math.Min(_maxProblemCount, problems.Count);

		for (int i = 0; i < listEnd; i++)
		{
			var (info, _) = problems[i + _scollOffset];
			var buttonText = $"{i + _scollOffset}.\t[Day {info.Day}] {info.Name}";
			var row = i + 7;
			if (i + _scollOffset == _selectedDay)
				DrawSelectedButton(buttonText, row, 2, Console.WindowWidth - 4, 1, ConsoleColor.DarkMagenta, ConsoleColor.DarkGray, false, 2);
			else
				DrawButton(buttonText, row, 2, Console.WindowWidth - 4, 1, ConsoleColor.Cyan, Console.BackgroundColor, false, 2);
		}
		for (int i = problems.Count + 7; i < Console.WindowHeight - 2; i++)
		{
			Console.SetCursorPosition(2, i);
			Console.Write(new string(' ', Console.WindowWidth - 4));
		}
	}

	private void RenderProblemResults()
	{
		Console.SetCursorPosition(2, 7);
		RunDay(_selectedYear, _selectedDay);
	}

	private void DrawSelectedButton(string text, int row, int col, int width, int height, ConsoleColor color = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black, bool centered = true, int padding = 0)
	{
		//text = $"\ue0c7{text}\ue0c6";
		var origBg = Console.BackgroundColor;
		Console.BackgroundColor = background;
		for (int y = row; y < row + height; y++)
		{
			Console.SetCursorPosition(col, y);
			Console.Write(new string(' ', width));
		}
		Console.ForegroundColor = color;
		var xOffset = centered ? (width / 2) - (text.Length / 2) : padding;
		var yOffset = centered ? height / 2 : padding;
		if (height == 1)
			yOffset = 0;
		Console.SetCursorPosition(col + xOffset, row + yOffset);
		Console.Write(text);
		Console.BackgroundColor = origBg;
		Console.ForegroundColor = background;
		Console.SetCursorPosition(col, row + height / 2);
		Console.Write('\ue0c7');
		Console.SetCursorPosition(col + width - 1, row + height / 2);
		Console.Write('\ue0c6');

		Console.BackgroundColor = origBg;
	}

	private void DrawButton(string text, int row, int col, int width, int height, ConsoleColor color = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black, bool centered = true, int padding = 0)
	{
		var origBg = Console.BackgroundColor;
		Console.BackgroundColor = background;
		for (int y = row; y < row + height; y++)
		{
			Console.SetCursorPosition(col, y);
			Console.Write(new string(' ', width));
		}
		Console.ForegroundColor = color;
		var xOffset = centered ? (width / 2) - (text.Length / 2) : padding;
		var yOffset = centered ? height / 2 : padding;
		if (height == 1)
			yOffset = 0;
		Console.SetCursorPosition(col + xOffset, row + yOffset);
		Console.Write(text);
		Console.BackgroundColor = origBg;
	}

	private void DrawBorder(int row, int col, int width, int height, ConsoleColor color = ConsoleColor.Gray, bool drawFill = false)
	{
		//║═╔╗╝╚
		Console.ForegroundColor = ConsoleColor.Gray;
		var w = col + width - 1;
		var h = row + height - 1;
		for (int x = col; x <= w; x++)
		{
			for (int y = row; y <= h; y++)
			{
				Console.SetCursorPosition(x, y);
				if (x == col && y == row)
					Console.Write('╔');
				else if (x == col && y == h)
					Console.Write('╚');
				else if (x == w && y == row)
					Console.Write('╗');
				else if (x == w && y == h)
					Console.Write('╝');
				else if (x == col || x == w)
					Console.Write('║');
				else if (y == row || y == h)
					Console.Write('═');
				else if (drawFill)
					Console.Write(' ');
			}
		}
	}
}