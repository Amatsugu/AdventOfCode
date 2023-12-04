using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day4;
[ProblemInfo(2023, 4, "Scratchcards")]
internal class Scratchcards : Problem<double, int>
{
	private (int card, int[] win, int[] have)[] _cards = [];

	public override void CalculatePart1()
	{
		Part1 = _cards
			.Select(c => c.have.Intersect(c.win).Count())
			.Select(c => c == 0 ? 0 : Math.Pow(2, c - 1))
			.Sum();
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines();
		_cards = new (int card, int[] win, int[] have)[lines.Length]; 
		for (int i = 0; i < lines.Length; i++)
		{
			var card = lines[i].Split(':');
			var cardNum = int.Parse(card[0].Split(' ').Last());
			var numbers = card[1].Split('|')
				.Select(v => v.Split(' ').Where(v => v.Length > 0).Select(int.Parse));
			_cards[i] = (cardNum, numbers.First().ToArray(), numbers.Last().ToArray());
		}
	}
}
