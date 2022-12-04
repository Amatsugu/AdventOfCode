using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode.Problems.AOC2021.Day3;

[ProblemInfo(2021, 3, "Binary Diagnostic")]
public class BinaryDiagnostic : Problem<int, int>
{
	public int[][] Data { get; set; } = Array.Empty<int[]>();

	private static int ToDecimal(int[] value)
	{
		return (int)value.Select((b, idx) => b * Math.Pow(2, value.Length - idx - 1)).Sum();
	}

	public override void LoadInput()
	{
		var data = ReadInputLines();
		Data = data.Select(line => line.Select(b => int.Parse(b.ToString())).ToArray()).ToArray();
	}

	public override void CalculatePart1()
	{
		var width = Data[0].Length;
		var transposed = Enumerable.Range(0, width).Select(idx => Data.Select(row => row[idx]));

		var transposed2 = new int[width][];
		for (int i = 0; i < width; i++)
		{
			transposed2[i] = Data.Select(row => row[i]).ToArray();
		}

		var mid = Data.Length / 2;
		var gamma = transposed.Select(col => col.Count(c => c == 1) > mid ? 1 : 0).ToArray();
		var epsilon = gamma.Select(b => b == 0 ? 1 : 0).ToArray();

		var gammaDec = ToDecimal(gamma);
		var epsilongDec = ToDecimal(epsilon);

		Part1 = gammaDec * epsilongDec;
	}

	public override void CalculatePart2()
	{
		var oxygen = Data;
		var index = 0;
		while (oxygen.Length > 1)
		{
			var t = oxygen.Select(row => row[index]);
			var m = oxygen.Length / 2f;
			var g = t.Count(c => c == 1) >= m ? 1 : 0;
			oxygen = oxygen.Where(row => row[index] == g).ToArray();
			index++;
		}

		var carbon = Data;
		index = 0;
		while (carbon.Length > 1)
		{
			var t = carbon.Select(row => row[index]);
			var m = carbon.Length / 2f;
			var e = t.Count(c => c == 1) < m ? 1 : 0;
			carbon = carbon.Where(row => row[index] == e).ToArray();
			index++;
		}

		var oxygenLevel = ToDecimal(oxygen.First());
		var carbonLevel = ToDecimal(carbon.First());

		Part2 = oxygenLevel * carbonLevel;
	}
}
