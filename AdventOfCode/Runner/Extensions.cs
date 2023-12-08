using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Runner;
public static class Extensions
{
	public static IEnumerable<T> Print<T>(this IEnumerable<T> values)
	{
		foreach (var item in values) 
		{ 
			Console.WriteLine(item);
		}
		return values;
	}
}
