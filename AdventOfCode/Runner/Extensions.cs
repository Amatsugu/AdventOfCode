using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

	public static T LCM<T>(this IEnumerable<T> values) where T : INumber<T>
	{
		var a = values.First();
		values = values.Skip(1);
		foreach (var item in values)
		{
			a = a.LCM(item);
		}
		return a;
	}
}
