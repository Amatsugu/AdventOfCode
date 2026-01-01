global using Vec3i = AdventOfCode.Utils.Models.Vec3<int>;
global using Vec2i = AdventOfCode.Utils.Models.Vec2<int>;
global using Vec3l = AdventOfCode.Utils.Models.Vec3<long>;
global using Vec2l = AdventOfCode.Utils.Models.Vec2<long>;
global using Vec3f = AdventOfCode.Utils.Models.Vec3<float>;
global using Vec2f = AdventOfCode.Utils.Models.Vec2<float>;
using System.Numerics;

namespace AdventOfCode.Utils.Models;

public record struct Vec2<T>(T X, T Y) where T : INumber<T>
{
	public static Vec2<T> Zero => new (T.Zero, T.Zero);
	public static Vec2<T> One => new (T.One, T.One);

	public readonly Vec2<T> YX => new(Y, X);
	public readonly Vec2<T> YY => new(Y, Y);
	public readonly Vec2<T> XX => new(X, X);
	public static Vec2<T> Splat(T v) => new(v, v);
	public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X + right.X, left.Y + right.Y);
	public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X - right.X, left.Y - right.Y);
	public static Vec2<T> operator -(Vec2<T> vec) => new Vec2<T>(-vec.X, -vec.Y);
	public static Vec2<T> operator *(Vec2<T> left, T right) => new Vec2<T>(left.X * right, left.Y * right);
	public static Vec2<T> operator *(T left, Vec2<T> right) => new Vec2<T>(right.X * left, right.Y * left);
	public static Vec2<T> operator /(Vec2<T> left, T right) => new Vec2<T>(left.X / right, left.Y / right);

	public static implicit operator Vec2<T>(T value)
	{
		return new(value, value);
	}

	public readonly T DistanceSq(Vec2<T> other)
	{
		var a = other.X - this.X;
		var b = other.Y - this.Y;
		return (a * a) + (b * b);
	}

	public readonly Vec2<T> Min(Vec2<T> other) => new(T.Min(X, other.X), T.Min(Y, other.Y));

	public readonly Vec2<T> Max(Vec2<T> other) => new(T.Max(X, other.X), T.Max(Y, other.Y));

	public readonly Vec2<T> Abs() => new(T.Abs(X), T.Abs(Y));


	public override readonly string ToString()
	{
		return $"({X}, {Y})";
	}
}

public record struct Vec3<T>(T X, T Y, T Z) where T : INumber<T>
{
	public static Vec3<T> Zero => new(T.Zero, T.Zero, T.Zero);
	public static Vec3<T> One => new(T.One, T.One, T.One);
	public static Vec3<T> Splat(T v) => new(v, v, v);
	public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	public static Vec3<T> operator -(Vec3<T> vec) => new Vec3<T>(-vec.X, -vec.Y, -vec.Z);
	public static Vec3<T> operator *(Vec3<T> left, T right) => new Vec3<T>(left.X * right, left.Y * right, left.Z * right);
	public static Vec3<T> operator *(T left, Vec3<T> right) => new Vec3<T>(right.X * left, right.Y * left, right.Z * left);
	public static Vec3<T> operator /(Vec3<T> left, T right) => new Vec3<T>(left.X / right, left.Y / right, left.Z / right);

	public static implicit operator Vec3<T>(T value)
	{
		return new(value, value, value);
	}

	public readonly T DistanceSq(Vec3<T> other)
	{
		var a = other.X - this.X;
		var b = other.Y - this.Y;
		var c = other.Z - this.Z;
		return T.Abs((a * a) + (b * b) + (c * c));
	}

	public readonly Vec3<T> Min(Vec3<T> other) => new(T.Min(X, other.X), T.Min(Y, other.Y), T.Min(Z, other.Z));

	public readonly Vec3<T> Max(Vec3<T> other) => new(T.Max(X, other.X), T.Max(Y, other.Y), T.Max(Z, other.Z));
	public readonly Vec3<T> Abs() => new(T.Abs(X), T.Abs(Y), T.Abs(Z));

	public override readonly string ToString()
	{
		return $"({X}, {Y}, {Z})";
	}
}