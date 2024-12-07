using System.Numerics;

namespace AdventOfCode.Utils.Models;


public record struct Vec2<T>(T X, T Y) where T : INumber<T>
{
	public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X + right.X, left.Y + right.Y);
	public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right) => new Vec2<T>(left.X - right.X, left.Y - right.Y);
	public static Vec2<T> operator *(Vec2<T> left, T right) => new Vec2<T>(left.X * right, left.Y * right);
	public static Vec2<T> operator /(Vec2<T> left, T right) => new Vec2<T>(left.X / right, left.Y / right);
}

public record struct Vec3<T>(T X, T Y, T Z) where T : INumber<T>
{
	public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right) => new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	public static Vec3<T> operator *(Vec3<T> left, T right) => new Vec3<T>(left.X * right, left.Y * right, left.Z * right);
	public static Vec3<T> operator /(Vec3<T> left, T right) => new Vec3<T>(left.X / right, left.Y / right, left.Z / right);
}