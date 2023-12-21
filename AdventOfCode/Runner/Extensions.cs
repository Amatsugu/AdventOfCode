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

	public static IEnumerable<IEnumerable<T2>> Transpose<T, T2>(this IEnumerable<T> data) where T: IEnumerable<T2>
	{
		var range = Enumerable.Range(0, data.First().Count());

		return range.Select(i => data.Select(l => l.Skip(i).First()));
	}

	public static IEnumerable<T> Transpose<T, T2>(this IEnumerable<T> data, Func<IEnumerable<T2>, T> formatter) where T : IEnumerable<T2>
	{
		var range = Enumerable.Range(0, data.First().Count());

		return range.Select(i => formatter(data.Select(l => l.Skip(i).First())));
	}

	public static IEnumerable<string> Transpose(this IEnumerable<string> data)
	{
		return Transpose<string, char>(data, a => string.Join("", a));
	}

	public static IEnumerable<List<T>> Permute<T>(this IEnumerable<T> values)
	{
		IEnumerable<List<T>> permutate(IEnumerable<T> reminder, IEnumerable<T> prefix)
		{
			return !reminder.Any()
				? new List<List<T>> { prefix.ToList() }
				: reminder.SelectMany((c, i) => permutate(
					reminder.Take(i).Concat(reminder.Skip(i + 1)).ToList(),
					prefix.Append(c)));
		}
		return permutate(values, Enumerable.Empty<T>());
	}

}
