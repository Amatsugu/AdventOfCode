using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day12;

public record Present(int Id, Vec2i[] Members)
{
	public int Size => Members.Length;
	public Present RotateCCW() => new(Id, [.. Members.Select(m => m.Rotate90CCW())]);
	public Present RotateCW() => new(Id, [.. Members.Select(m => m.Rotate90CW())]);
}