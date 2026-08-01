using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day12;

public record Tree(Vec2i Size, int[] Presents)
{
	public int Area => Size.X * Size.Y;

	public bool CanFitPresents(Present[] presents)
	{
		throw new NotImplementedException();
	}
}
