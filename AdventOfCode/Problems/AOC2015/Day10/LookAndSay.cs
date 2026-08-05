using AdventOfCode.Runner.Attributes;

using System.Text;

namespace AdventOfCode.Problems.AOC2015.Day10;

[ProblemInfo(2015, 10, "Evles Look, Elves Say")]
internal class LookAndSay : Problem<int, int>
{
	private string _input = string.Empty;

	public override void CalculatePart1()
	{
		Part1 = RunV2(40);
	}

	public override void CalculatePart2()
	{
		Part2 = RunV2(50);
	}

	public override void LoadInput()
	{
		_input = "3113322113";
	}

	public int Run(int iter)
	{
		var value = new StringBuilder(_input);
		for (int i = 0; i < iter; i++)
			CalculateNext(ref value);

		return value.Length;
	}

	public int RunV2(int iter)
	{
		var value = _input.Select(x => int.Parse(x.ToString())).ToList();
		for (int i = 0; i < iter; i++)
			CalculateNext(ref value);

		return value.Count;
	}

	private static void CalculateNext(ref List<int> input)
	{
		var next = new List<int>(input.Count * 2);
		var curCount = 1;
		var curChar = input[0];
		for (int i = 1; i < input.Count; i++)
		{
			var c = input[i];
			if (c != curChar)
			{
				next.Add(curCount);
				next.Add(curChar);
				curChar = c;
				curCount = 1;
				continue;
			}
			curCount++;
		}
		next.Add(curCount);
		next.Add(curChar);

		input = next;
	}
	private static void CalculateNext(ref StringBuilder input)
	{
		var next = new StringBuilder();
		var len = input.Length;
		var curCount = 1;
		var curChar = input[0];
		for (int i = 1; i < len; i++)
		{
			var c = input[i];
			if (c != curChar)
			{
				next.Append(curCount).Append(curChar);
				curChar = c;
				curCount = 1;
				continue;
			}
			curCount++;
		}
		next.Append(curCount).Append(curChar);

		input = next;
	}
}