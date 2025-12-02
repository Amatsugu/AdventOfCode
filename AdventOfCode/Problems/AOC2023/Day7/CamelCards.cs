using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day7;
[ProblemInfo(2023, 7, "Camel Cards")]
internal class CamelCards : Problem<int, int>
{
	private CamelHand[] _hands = [];
	private CamelHand[] _jokerHands = [];

	public override void LoadInput()
	{
		var data = ReadInputLines("input.txt");
		_hands = data.Select(d => new CamelHand(d)).ToArray();
		_jokerHands = data.Select(d => new CamelHand(d, true)).ToArray();
	}

	public override void CalculatePart1()
	{
		var x = _hands.Order();
		Part1 = x.Select((h, i) => (i + 1) * h.Bid).Sum();
	}

	public override void CalculatePart2()
	{
		var x = _jokerHands.Order().Print();
		//x.Where(x => x.Hand.Contains('J')).Print();
		Part2 = x.Select((h, i) => (i + 1) * h.Bid).Sum();
	}

}
