using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day2;
[ProblemInfo(2023, 2, "Cube Conundrum")]
internal class CubeConundrum : Problem<int, int>
{
	private CubeGame[] _gameInfo = [];

	public override void LoadInput()
	{
		var lines = ReadInputLines();
		_gameInfo = lines.Select(l => new CubeGame(l)).ToArray();
	}

	public override void CalculatePart1()
	{
		Part1 = FindPossibleGames(new CubeRound(12, 13, 14)).Sum();
	}

	public override void CalculatePart2()
	{
		Part2 = _gameInfo.Select(g => g.GetMinimalConstraints().Power()).Sum();
	}

	public IEnumerable<int> FindPossibleGames(CubeRound constraints)
	{
		return _gameInfo.Where(g => !g.Rounds.Any(r => !r.IsPossible(constraints)))
			.Select(r => r.Id);
	}
}
