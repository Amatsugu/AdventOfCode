using AdventOfCode.Runner.Attributes;

using Superpower;
using Superpower.Parsers;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.AOC2022.Day5;

[ProblemInfo(2022, 5, "Supply Stacks")]
internal partial class SupplyStacks : Problem
{
	private List<char>[] _stacksPart1 = Array.Empty<List<char>>();
	private List<char>[] _stacksPart2 = Array.Empty<List<char>>();
	private readonly TextParser<(int, int, int)> _moveParser = from move in Character.Letter.Or(Character.WhiteSpace).Many()
													  from stack in Character.Digit.Many()
													  from frm in Character.Letter.Or(Character.WhiteSpace).Many()
													  from source in Character.Digit.Many()
													  from to in Character.Letter.Or(Character.WhiteSpace).Many()
													  from dst in Character.Digit.Many()
													  select (int.Parse(stack), int.Parse(source), int.Parse(dst));

	private List<(int stack, int from, int to)> _moves = new ();

	public override void CalculatePart1()
	{
		foreach (var move in _moves)
		{
			PerformBasicMove(_stacksPart1, move);
		}
		Part1 = new string(_stacksPart1.Select(b => b.Last()).ToArray());
	}

	private static void PerformBasicMove(List<char>[] data, (int stack, int from, int to) move)
	{
		var from = data[move.from-1];
		var to = data[move.to-1];
		for (int i = 0; i < move.stack; i++)
		{
			var item = from[^1];
			from.RemoveAt(from.Count-1);
			to.Add(item);
		}
	}

	private static void PerformMove(List<char>[] data, (int stack, int from, int to) move)
	{
		var from = data[move.from - 1];
		var to = data[move.to - 1];
		var items = from.Skip(from.Count - move.stack);
		to.AddRange(items);
		from.RemoveRange(from.Count - move.stack, move.stack);
	}

	public override void CalculatePart2()
	{
		foreach (var move in _moves)
		{
			PerformMove(_stacksPart2, move);
		}
		Part2 = new string(_stacksPart2.Select(b => b.Last()).ToArray());
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		var readMoves = false;

		var buffers = new List<List<char>>();
		var moves = new List<(int stack, int from, int to)>();
		foreach (var line in lines)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				readMoves = true;
				continue;
			}

			if (readMoves)
			{
				moves.Add(ParseMoveLineRegex(line));
			}
			else
			{
				var crates = ParseCrateLine(line);
				for (int i = 0; i < crates.Count; i++)
				{
					var crate = crates[i];
					if (buffers.Count == i)
						buffers.Add(crate == ' ' ? new() : new List<char>() { crate });
					else if (crate != ' ')
						buffers[i].Add(crate);
				}
			}
		}
		_stacksPart1 = buffers.Select(b => b.Take(b.Count - 1).Reverse().ToList()).ToArray();
		_stacksPart2 = buffers.Select(b => b.Take(b.Count - 1).Reverse().ToList()).ToArray();
		_moves = moves;
	}

	//Way slower
	private (int stack, int from, int to) ParseMoveLine(string line)
	{
		return _moveParser.Parse(line);
	}

	private static (int stack, int from, int to) ParseMoveLineRegex(string line)
	{
		var r = MoveParser().Matches(line);
		var items = r.First()
			.Groups.Values.Skip(1)
			.Select(v => int.Parse(v.ValueSpan))
			.ToArray();
		return (items[0], items[1], items[2]);
	}

	private static List<char> ParseCrateLine(string line)
	{
		var result = new List<char>(line.Length / 4);
		for (int i = 1; i < line.Length; i += 4)
		{
			var c = line[i];
			result.Add(c);
		}
		return result;
	}

	[GeneratedRegex("move (\\d+) from (\\d+) to (\\d+)")]
	private static partial Regex MoveParser();
}