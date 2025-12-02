using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils;
public static class Extensions
{
	public static string AsJoinedString<T>(this IEnumerable<T> data, string delim = ", ")
	{
		return string.Join(delim, data);
	}

	public static T Mod<T>(this T value, T divisor) where T : INumber<T>
	{
		T remainder = value % divisor;

		if (remainder < T.Zero)
			return remainder + divisor;
		else
			return remainder;
	}
}
