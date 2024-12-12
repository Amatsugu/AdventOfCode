using System.Numerics;

namespace AdventOfCode.Utils.Models;

public record struct Vec2<T>(T X, T Y) where T : INumber<T>
{
	public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X + right.X, left.Y + right.Y);
	public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X - right.X, left.Y - right.Y);
	public static Vec2<T> operator -(Vec2<T> vec) => new Vec2<T>(-vec.X, -vec.Y);
	public static Vec2<T> operator *(Vec2<T> left, T right) => new Vec2<T>(left.X * right, left.Y * right);
	public static Vec2<T> operator *(T left, Vec2<T> right) => new Vec2<T>(right.X * left, right.Y * left);
	public static Vec2<T> operator /(Vec2<T> left, T right) => new Vec2<T>(left.X / right, left.Y / right);

	public T DistanceSq(Vec2<T> other)
	{
		var a = other.X - this.X;
		var b = other.Y - this.Y;
		return (a * a) + (b * b);
	}
}

public record struct Vec3<T>(T X, T Y, T Z) where T : INumber<T>
{
	public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	public static Vec3<T> operator -(Vec3<T> vec) => new Vec3<T>(-vec.X, -vec.Y, -vec.Z);
	public static Vec3<T> operator *(Vec3<T> left, T right) => new Vec3<T>(left.X * right, left.Y * right, left.Z * right);
	public static Vec3<T> operator *(T left, Vec3<T> right) => new Vec3<T>(right.X * left, right.Y * left, right.Z * left);
	public static Vec3<T> operator /(Vec3<T> left, T right) => new Vec3<T>(left.X / right, left.Y / right, left.Z / right);

	public T DistanceSq(Vec3<T> other)
	{
		var a = other.X - this.X;
		var b = other.Y - this.Y;
		var c = other.Z - this.Z;
		return (a * a) + (b * b) + (c * c);
	}
}