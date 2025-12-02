namespace AdventOfCode.Problems.AOC2023.Day2;

internal class CubeGame
{
	public CubeRound[] Rounds { get; }

	public int Id { get; }

	public CubeGame(string line)
	{
		var info = line.Split(':');
		Id = int.Parse(info[0].Split(' ')[^1]);

		var roundsData = info[1].Split(';');
		Rounds = new CubeRound[roundsData.Length];
		for (int i = 0; i < roundsData.Length; i++)
			Rounds[i] = CubeRound.ParseRound(roundsData[i]);
	}

	public CubeRound GetMinimalConstraints()
	{
		var (r, g, b) = (0, 0, 0);
		foreach (var round in Rounds)
		{
			if (round.Red > r)
				r = round.Red;
			if (round.Green > g)
				g = round.Green;
			if (round.Blue > b)
				b = round.Blue;
		}
		return new CubeRound(r, g, b);
	}
}

internal record struct CubeRound(int Red, int Green, int Blue)
{
	public static CubeRound ParseRound(string round)
	{
		var cubes = round.Split(',');
		var (r, g, b) = (0, 0, 0);

		foreach (var cube in cubes)
		{
			var info = cube.TrimStart().Split(' ');
			var count = int.Parse(info[0]);
			switch (info[1])
			{
				case ['r', ..]:
					r = count;
					break;

				case ['g', ..]:
					g = count;
					break;

				case ['b', ..]:
					b = count;
					break;
			}
		}
		return new CubeRound(r, g, b);
	}

	public readonly bool IsPossible(CubeRound constraints)
	{
		if (Red > constraints.Red)
			return false;
		if (Green > constraints.Green)
			return false;
		if (Blue > constraints.Blue)
			return false;

		return true;
	}

	public readonly int Power()
	{
		return Red * Green * Blue;
	}
};