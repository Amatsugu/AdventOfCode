using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rule = (int after, int before);


namespace AdventOfCode.Problems.AOC2024.Day5;
[ProblemInfo(2024, 5, "Print Queue")]
internal class PrintQueue : Problem<int, int>
{
	private List<Rule> _rules = [];
	private List<int[]> updates = [];
	public override void CalculatePart1()
	{
		foreach (var update in updates)
		{
			if (IsOrdered(update, out _))
			{
				var mid = update[update.Length / 2];
				Part1 += mid;
			}
		}
	}

	public bool IsOrdered(int[] update, out List<int> orderd)
	{
		var list = new PageList(update, _rules);
		orderd = list.Traverse();
		return orderd.Zip(update).All(e => e.First== e.Second);

	}

	public int[] SortUpdates(int[] updates)
	{
		throw new NotImplementedException();
	}

	public override void CalculatePart2()
	{
		foreach (var update in updates)
		{
			if (!IsOrdered(update, out var ordered))
			{
				var mid = ordered[update.Length / 2];
				Part2 += mid;
			}
		}
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");

		var parsingPages = false;
		foreach (var line in lines)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				parsingPages = true;
				continue;
			}
			if (parsingPages)
			{
				updates.Add(line.Split(',').Select(int.Parse).ToArray());
			}
			else{
				var d = line.Split('|').Select(int.Parse);
				_rules.Add((d.First(), d.Last()));
			}
		}

	}
}


