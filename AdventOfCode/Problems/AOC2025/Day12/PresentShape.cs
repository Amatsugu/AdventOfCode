using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Problems.AOC2025.Day12;

public record PresentShape(int Id, Vec2i[] Members)
{
	public int Size => Members.Length;
	public PresentShape RotateCCW() => new(Id, [.. Members.Select(m => m.Rotate90CCW())]);
	public PresentShape RotateCW() => new(Id, [.. Members.Select(m => m.Rotate90CW())]);
}