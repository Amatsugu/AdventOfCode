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

	
}
