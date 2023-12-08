using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day7;
internal class CamelHand : IComparable<CamelHand>
{
	public static readonly List<char> CARDS = [ '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' ];
	public static readonly List<char> CARDS_JOKER = [ 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' ];

	public string Hand { get; set; }
	public int Bid { get; set; }
	public int Value { get; set; }
	public HandType Type { get; set; }
	public CamelHand(string data, bool useJoker = false)
	{
		var split = data.Split(' ');
		Hand = split[0];
		Bid = int.Parse(split[1]);
		Type = useJoker ? GetJokerHandType(Hand) : GetHandType(Hand);
		Value = CalculateValue(Hand, useJoker);
	}

	public static int CalculateValue(string hand, bool useJoker)
	{
		var total = 0;
		for (int i = 0; i < hand.Length; i++)
		{
			var p = (hand.Length - i - 1);
			var v = useJoker ? CARDS_JOKER.IndexOf(hand[i]) : CARDS.IndexOf(hand[i]);
			total += (v + 1) * (int)Math.Pow(13, p);
		}
		return total;
	}

	public bool IsStrongerThan(CamelHand card)
	{
		if (Type > card.Type)
			return true;
		if(Type < card.Type) 
			return false;

		return Value >= card.Value;
	}

	private static HandType GetJokerHandType(string hand)
	{
		var type = GetHandType(hand);
		if (type == HandType.FiveOfKind)
			return type;

		if (!hand.Contains('J'))
			return type;
		var bestCard = hand.GroupBy(c => c)
			.OrderByDescending(c => c.Count())
			.First(c => c.Key != 'J').Key;

		var newHand = hand.Replace('J', bestCard);
		return GetHandType(newHand);
	} 
	private static HandType GetHandType(string hand)
	{
		var cardGroups = hand.GroupBy(c => c).Select(g => g.Count()).ToArray();

		if (cardGroups.Length == 1)
			return HandType.FiveOfKind;
		if(cardGroups.Contains(4))
			return HandType.FourOfKind;
		if (cardGroups.Contains(3) && cardGroups.Contains(2))
			return HandType.FullHouse;
		if(cardGroups.Contains(3))
			return HandType.ThreeOfKind;

		var pairs = cardGroups.Count(c => c == 2);
		if (pairs == 2)
			return HandType.TwoPair;
		if (pairs == 1)
			return HandType.OnePair;

		return HandType.HighCard;
	}

	public int CompareTo(CamelHand? other)
	{
		if(other == null) return 1;
		return IsStrongerThan(other) ? 1 : -1;
	}

	public override string ToString()
	{
		return $"[{Value}] {Hand}: {Type} | {Bid}";
	}

	public enum HandType
	{
		HighCard,
		OnePair,
		TwoPair,
		ThreeOfKind,
		FullHouse,
		FourOfKind,
		FiveOfKind
	}
}
