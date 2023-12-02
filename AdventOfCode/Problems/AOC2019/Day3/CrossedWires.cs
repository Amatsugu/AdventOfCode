using AdventOfCode.Runner.Attributes;

using System.Drawing;

namespace AdventOfCode.Problems.AOC2019.Day3
{
	[ProblemInfo(2019, 3, "Crossed Wires")]
	public class CrossedWires : Problem<int, int>
	{
		private string[] _inputLines = Array.Empty<string>();

		public struct WireSegment
		{
			public Point min, max;
			public Point start, end;

			public int Length;

			public bool Vertical => min.X == max.X;

			public WireSegment(Point a, Point b)
			{
				start = a;
				end = b;
				if (a.X == b.X) //Vertical
				{
					if (a.Y < b.Y)
					{
						min = a;
						max = b;
					}
					else
					{
						min = b;
						max = a;
					}
					Length = Math.Abs(b.Y - a.Y);
				}
				else
				{
					if (a.X < b.X)
					{
						min = a;
						max = b;
					}
					else
					{
						min = b;
						max = a;
					}
					Length = Math.Abs(b.X - a.X);
				}
			}

			public WireSegment CreateRelative(Point offset)
			{
				return new WireSegment(end, new Point(end.X + offset.X, end.Y + offset.Y));
			}

			public bool ContainsPoint(Point point)
			{
				if (Vertical)
				{
					if (min.X == point.X)
					{
						return min.Y <= point.Y && max.Y >= point.Y;
					}
					else
						return false;
				}
				else
				{
					if (min.Y == point.Y)
					{
						return min.X <= point.X && max.X >= point.X;
					}
					else
						return false;
				}
			}

			public bool Contains(WireSegment other)
			{
				if (Vertical != other.Vertical)
					return false;
				if (Vertical)
				{
					return min.Y <= other.min.Y && max.Y >= other.max.Y;
				}
				else
				{
					return min.X <= other.min.X && max.X >= other.max.X;
				}
			}

			public WireSegment GetOverlap(WireSegment other)
			{
				if (Vertical)
				{
					if (other.Contains(this))
						return this;
					else if (Contains(other))
						return other;
					if (max.Y >= other.min.Y && min.Y <= other.min.Y && max.Y <= other.max.Y)
					{
						return new WireSegment(other.min, max);
					}
					else if (max.Y >= other.max.Y && min.Y >= other.min.Y && min.Y <= other.max.Y)
						return new WireSegment(min, other.max);
					else
						throw new Exception("No Overlap");
				}
				else
				{
					if (other.Contains(this))
						return this;
					else if (Contains(other))
						return other;
					if (max.X >= other.min.X && min.X <= other.min.X && max.X <= other.max.X)
					{
						return new WireSegment(other.min, max);
					}
					else if (max.X >= other.max.X && min.X >= other.min.X && min.X <= other.max.X)
						return new WireSegment(min, other.max);
					else
						throw new Exception("No Overlap");
				}
			}

			public bool Intersect(WireSegment other, out Point intersection)
			{
				if (Vertical)
				{
					if (!other.Vertical)//Other Horizontal
					{
						var potInt = new Point(min.X, other.min.Y);
						if (potInt == default)
						{
							intersection = default;
							return false;
						}
						if (ContainsPoint(potInt) && other.ContainsPoint(potInt))
						{
							intersection = potInt;
							return true;
						}
						else
						{
							intersection = default;
							return false;
						}
					}
					else //Both
					{
						if (min.X != other.min.X)
						{
							intersection = default;
							return false;
						}
						else
						{
							var overlap = GetOverlap(other);
							if (ManhattanMagnitude(overlap.min) < ManhattanMagnitude(overlap.max))
								intersection = overlap.min == default ? overlap.max : overlap.min;
							else
								intersection = overlap.max == default ? overlap.min : overlap.max;
							if (intersection == default)
								return false;
							return true;
						}
					}
				}
				else
				{
					if (!other.Vertical) //Other Horizontal
					{
						if (min.Y != other.min.Y)
						{
							intersection = default;
							return false;
						}
						var overlap = GetOverlap(other);
						if (ManhattanMagnitude(overlap.min) < ManhattanMagnitude(overlap.max))
							intersection = overlap.min == default ? overlap.max : overlap.min;
						else
							intersection = overlap.max == default ? overlap.min : overlap.max;
						if (intersection == default)
							return false;
						return true;
					}
					else
						return other.Intersect(this, out intersection);
				}
			}

			public override string ToString()
			{
				return $"{start} > {end}";
			}
		}

		public static int StepsToPoint(List<WireSegment> wires, Point p)
		{
			var steps = 0;
			for (int i = 0; i < wires.Count; i++)
			{
				if (wires[i].ContainsPoint(p))
				{
					if (wires[i].Vertical)
						steps += Math.Abs(wires[i].start.Y - p.Y);
					else
						steps += Math.Abs(wires[i].start.X - p.X);
					break;
				}
				else
					steps += wires[i].Length;
			}
			return steps;
		}

		public static int SolveWires(string[] wires)
		{
			var (wireSegmentsA, wireSegmentsB) = CreateWirePair(wires);

			int shortestWire = int.MaxValue;
			for (int i = 0; i < wireSegmentsA.Count; i++)
			{
				for (int j = 0; j < wireSegmentsB.Count; j++)
				{
					if (wireSegmentsA[i].Intersect(wireSegmentsB[j], out var intersection))
					{
						var len = StepsToPoint(wireSegmentsA, intersection) + StepsToPoint(wireSegmentsB, intersection);
						if (len < shortestWire)
							shortestWire = len;
					}
				}
			}
			return shortestWire;
		}

		public static int SolveClosestDistane(string[] wires)
		{
			var (wireSegmentsA, wireSegmentsB) = CreateWirePair(wires);

			int lastIntersection = int.MaxValue;
			for (int i = 0; i < wireSegmentsA.Count; i++)
			{
				for (int j = 0; j < wireSegmentsB.Count; j++)
				{
					if (wireSegmentsA[i].Intersect(wireSegmentsB[j], out var intersection))
					{
						var dist = ManhattanMagnitude(intersection);
						if (dist < lastIntersection)
							lastIntersection = dist;
					}
				}
			}
			return lastIntersection;
		}

		private static (List<WireSegment> A, List<WireSegment> B) CreateWirePair(string[] wires)
		{
			var wireA = wires[0].Split(',');
			var wireSegmentsA = CreateWire(wireA);

			var wireB = wires[1].Split(',');
			var wireSegmentsB = CreateWire(wireB);

			return (wireSegmentsA, wireSegmentsB);
		}

		private static List<WireSegment> CreateWire(string[] wires)
		{
			var wireSegments = new List<WireSegment>();

			for (int i = 0; i < wires.Length; i++)
			{
				var curSegment = wires[i];
				var offset = GetOffset(curSegment);
				if (i == 0)
					wireSegments.Add(new WireSegment(new Point(0, 0), offset));
				else
					wireSegments.Add(wireSegments.Last().CreateRelative(offset));
			}
			return wireSegments;
		}

		public static int ManhattanMagnitude(Point point) => Math.Abs(point.X) + Math.Abs(point.Y);

		public static Point GetOffset(string move)
		{
			int x = 0, y = 0;
			if (move[0] == 'R')
				x = int.Parse(move.Remove(0, 1));
			else if (move[0] == 'L')
				x = -int.Parse(move.Remove(0, 1));
			else if (move[0] == 'U')
				y = int.Parse(move.Remove(0, 1));
			else if (move[0] == 'D')
				y = -int.Parse(move.Remove(0, 1));
			return new Point(x, y);
		}

		public override void LoadInput()
		{
			_inputLines = ReadInputLines();
		}

		public override void CalculatePart1()
		{
			Part1 = SolveClosestDistane(_inputLines);
		}

		public override void CalculatePart2()
		{
			Part2 = SolveWires(_inputLines);
		}
	}
}