
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Runtime.CompilerServices;

namespace AdventOfCode.Problems.AOC2025.Day9;

[ProblemInfo(2025, 9, "Movie Theater")]
internal class MovieTheater : Problem<long, long>
{
	private Vec2l[] _input = [];

	public override void CalculatePart1()
	{
		for (long i = 0; i < _input.Length; i++)
		{
			var a = _input[i];
			for (long j = (i + 1); j < _input.Length; j++)
			{
				var b = _input[j];
				var area = CalculateArea(a, b);
				if (area > Part1)
				{
					Part1 = area;
				}
			}
		}
	}

	public static long CalculateArea(Vec2l a, Vec2l b)
	{
		var rect = (a - b).Abs() + 1;
		var area = Math.Abs(rect.X * rect.Y);
		//Console.WriteLine($"{a} -> {b} : {rect} = {area}");
		return area;
	}

	public override void CalculatePart2()
	{
		var bounds = CreateBounds();
		var best = (a: _input[0], b: _input[1]);
		for (long i = 0; i < _input.Length; i++)
		{
			var a = _input[i];
			for (long j = (i + 1); j < _input.Length; j++)
			{
				var b = _input[j];
				var (c, d) = GetOtherCorners(a, b);
				Vec2l[] checkPoints = [a, b, c, d];
				if (checkPoints.All(p => CastRays(p, bounds)))
				{
					best = (a, b);
					var area = CalculateArea(a, b);
					if (Part2 < area)
						Part2 = area;
				}
			}
		}
#if DEBUG
		Debug(best.a, best.b, bounds, GetCheckPoints(best.a, best.b));
#endif
	}

	private void Debug(Vec2l a, Vec2l b, List<Line> shape, Vec2l[] points)
	{
		var (c, d) = GetOtherCorners(a, b);
		DebugDraw(shape, GetRect(a, b, c, d), points, 100);
	}

	private static bool CastRays(Vec2l point, List<Line> shape)
	{
		bool up = false, down = false, left = false, right = false;
		foreach (var line in shape)
		{
			if (line.IsHorizontal)
			{
				var xInt = line.InterceptX(point.X);
				if (xInt is Vec2l x)
				{
					if (x.Y == point.Y)
						return true;

					if (x.Y > point.Y)
						up = true;
					else
						down = true;
				}
			}
			else
			{
				var yInt = line.InterceptY(point.Y);
				if (yInt is Vec2l y)
				{
					if (y.X == point.X)
						return true;

					if (y.X < point.X)
						left = true;
					else
						right = true;
				}
			}
		}
		return up && down && left && right;
	}

	private void DebugDraw(List<Line> lines, Line[] rect, Vec2l[] points, long scale = 100)
	{
		var min = _input.Aggregate((a, b) => a.Min(b));
		var max = _input.Aggregate((a, b) => a.Max(b));
		var pad = new Vec2l(10, 10);
		var size = ((min - max).Abs() / scale) + pad;
		var image = new Image<Rg32>((int)size.X, (int)size.Y);

		image.Mutate(o =>
		{
			var shapeColor = Pens.Solid(Color.DimGray, 1);
			var rectColor = Pens.Solid(Color.Aquamarine, 1);
			var pointColor = Pens.Solid(Color.Red, 1);
			o.Paint(canvas =>
			{
				foreach (var line in lines)
				{
					var a = ((line.A - min) / scale) + pad / 2;
					var b = ((line.B - min) / scale) + pad / 2;

					canvas.DrawLine(shapeColor, new PointF(a.X, a.Y), new PointF(b.X, b.Y));
				}
				foreach (var line in rect)
				{
					var a = ((line.A - min) / scale) + pad / 2;
					var b = ((line.B - min) / scale) + pad / 2;

					canvas.DrawLine(rectColor, new PointF(a.X, a.Y), new PointF(b.X, b.Y));
				}
				foreach (var point in points)
				{
					canvas.DrawEllipse(pointColor, new PointF(((point.X - min.X) / scale) + (pad.X / 2), ((point.Y - min.Y) / scale) + (pad.X / 2)), new SizeF(1, 1));
				}
			});
		});

		image.SaveAsPng("day9p2.png");
	}

	private List<Line> CreateBounds()
	{
		var lines = new List<Line>();
		for (int i = 0; i < _input.Length - 1; i++)
		{
			var a = _input[i];
			var b = _input[i + 1];
			lines.Add(Line.FromPoints(a, b));
		}
		lines.Add(Line.FromPoints(_input[^1], _input[0]));
		return lines;
	}

	private static Vec2l[] GetCheckPoints(Vec2l a, Vec2l b)
	{
		return [new Vec2l(GetMidpoint(a.X, b.X), a.Y), new Vec2l(a.X, GetMidpoint(a.Y, b.Y)), new Vec2l(GetMidpoint(a.X, b.X), b.Y), new Vec2l(b.X, GetMidpoint(a.Y, b.Y))];
	}

	private static long GetMidpoint(long a, long b)
	{
		if (a > b)
		{
			return a - ((a - b) / 2);
		}
		else
		{
			return b - ((b - a) / 2);
		}
	}

	private static (Vec2l, Vec2l) GetOtherCorners(Vec2l a, Vec2l b)
	{
		return (new Vec2l(a.X, b.Y), new Vec2l(b.X, a.Y));
	}

	private static Line[] GetRect(Vec2l a, Vec2l b, Vec2l c, Vec2l d)
	{
		return [Line.FromPoints(a, c), Line.FromPoints(c, b), Line.FromPoints(b, d), Line.FromPoints(d, a)];
	}

	public override void LoadInput()
	{
		_input = ReadInputLines("input.txt").Select(l => l.Split(',').Select(long.Parse)).Select(v => new Vec2<long>(v.First(), v.Last())).ToArray();
	}

	private record Line(Vec2l A, Vec2l B, long Dir)
	{
		public long MinX => long.Min(A.X, B.X);
		public long MinY => long.Min(A.Y, B.Y);
		public long MaxX => long.Max(A.X, B.X);
		public long MaxY => long.Max(A.Y, B.Y);
		public bool IsHorizontal => A.Y == B.Y;
		public bool IsVertical => A.X == B.X;
		public static Line FromPoints(Vec2l a, Vec2l b)
		{
			var dir = (a - b);
			return new Line(a, b, dir.X == 0 ? dir.Y : dir.X);
		}

		public bool Intersects(Line other)
		{
			//if(IsVertical == other.IsVertical)
			//	return Intersects(other.A) || Intersects(other.B);
			if (IsVertical)
			{
				if (!IsBetween(other.MinX, other.MaxX, A.X))
					return false;
				return IsBetween(MinY, MaxY, other.A.Y);
			}
			else
			{
				if (!IsBetween(other.MinY, other.MaxY, A.Y))
					return false;
				return IsBetween(MinX, MaxX, other.A.X);
			}
		}

		public bool Intersects(Vec2l point)
		{
			if (point.X == A.X && point.X == B.X && IsBetween(A.Y, B.Y, point.Y))
				return true;

			if (point.Y == A.Y && point.Y == B.Y && IsBetween(A.X, B.X, point.X))
				return true;
			return false;
		}

		public bool IntersectsY(long y)
		{
			return IsVertical && IsBetween(A.Y, B.Y, y);
		}

		public bool IntersectsX(long x)
		{
			return IsHorizontal && IsBetween(A.X, B.X, x);
		}

		public Vec2l? InterceptX(long x)
		{
			if (IsVertical)
				return null;
			if (!IsBetween(A.X, B.X, x))
				return null;
			return new Vec2l(x, A.Y);
		}

		public Vec2l? InterceptY(long y)
		{
			if (IsHorizontal)
				return null;
			if (!IsBetween(A.Y, B.Y, y))
				return null;
			return new Vec2l(A.X, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsBetween(long a, long b, long value)
		{
			return (a <= value && b >= value) || (a >= value && b <= value);
		}
	}
}