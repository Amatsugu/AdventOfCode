using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Runner;

public static class ExtraMath
{
	public static T GCF<T>(this T a, T b) where T : INumber<T>
	{
		while (!b.Equals(T.Zero))
		{
			var t = b;
			b = a % b;
			a = t;
		}
		return a;
	}

	public static T LCM<T>(this T a, T b) where T : INumber<T>
	{
		return (a / GCF(a, b)) * b;
	}

	public static T Max<T>(this T a, T b) where T : INumber<T>
	{
		return T.Max(a, b);
	}

	public static T Min<T>(this T a, T b) where T : INumber<T>
	{
		return T.Min(a, b);
	}
}