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
	private Dictionary<int, int> _cardCount = new ();

	public override void CalculatePart1()
	{
		Part1 = _cards
			.Select(c => c.have.Intersect(c.win).Count())
			.Select(c => c == 0 ? 0 : Math.Pow(2, c - 1))
			.Sum();
	}

	public override void CalculatePart2()
	{
		Part2 = _cards.Length;
		for (int i = 0; i < _cards.Length; i++)
		{
			var card = _cards[i];
			var wins = card.have.Intersect(card.win).Count();
			var cCount = GetCardCount(card.card);
			for (int j = 1; j <= wins; j++)
				AddCards(card.card + j, cCount);
			Part2 += wins * cCount;
		}
	}

	private int GetCardCount(int card)
	{
		if(_cardCount.TryGetValue(card, out var count)) 
			return count;
		return 1;
	}

	private void AddCards(int card, int count)
	{
		if(_cardCount.ContainsKey(card))
			_cardCount[card] += count;
		else
			_cardCount.Add(card, count + 1);
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
