using AdventOfCode.Runner.Attributes;

using System.Collections;
using System.Numerics;
using System.Text;


namespace AdventOfCode.Problems.AOC2021.Day11;

[ProblemInfo(2021, 11, "Dumbo Octopus")]
public class DumboOctopus : Problem<int, int>, IEnumerable<byte>
{
	public byte[] dataPart1 = Array.Empty<byte>();
	public byte[] dataPart2 = Array.Empty<byte>();

	public override void LoadInput()
	{
		var data = ReadInputLines();
		dataPart1 = new byte[10 * 10];
		if (data.Length != 10)
			throw new ArgumentException("Data must contain 10 elements", nameof(data));
		for (int y = 0; y < data.Length; y++)
		{
			var line = data[y];
			if (line.Length != 10)
				throw new ArgumentException($"Lines must contain 10 elements. Line: {y + 1}", nameof(data));

			for (int x = 0; x < line.Length; x++)
			{
				dataPart1[x + y * 10] = byte.Parse(line.Substring(x, 1));
			}
		}
		dataPart2 = new byte[10 * 10];
		Array.Copy(dataPart1, dataPart2, dataPart1.Length);
	}

	public override void CalculatePart1()
	{
		Part1 = Run(ref dataPart1, 100, out _);
	}

	public override void CalculatePart2()
	{
		Run(ref dataPart2, 210, out var fullFlash);
		Part2 = fullFlash;
	}

	public IEnumerator<byte> GetEnumerator()
	{
		return ((IEnumerable<byte>)dataPart1).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dataPart1.GetEnumerator();
	}

	public int Run(ref byte[] data, int count, out int fullFlash)
	{
		var flashes = 0;
		fullFlash = 0;
		for (int i = 0; i < count; i++)
		{
			var start = flashes;
			Run(ref data, ref flashes);
			if (flashes - start == 100)
			{
				fullFlash = i + 1;
				break;
			}
		}
		return flashes;
	}

	public void Run(ref byte[] data, ref int flashes)
	{
		Increment(ref data);
		Flash(ref data, ref flashes);

	}

	private static void Increment(ref byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
			data[i]++;
	}

	private void Flash(ref byte[] data, ref int flashes)
	{
		int diff;
		do
		{
			var startFlash = flashes;
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] > 9)
					PerformFlash(ref data, i, ref flashes);
			}
			diff = flashes - startFlash;
		} while (diff != 0);
	}

	private void PerformFlash(ref byte[] data, int index, ref int flashes)
	{
		flashes++;
		var y = index / 10;
		var x = index % 10;

		data[index] = 0;

		if (x > 0)
		{
			//Left
			index = x - 1 + y * 10;
			if (index >= 0 && data[index] != 0)
				data[index]++;
		}
		if (x < 9)
		{
			//Right
			index = x + 1 + y * 10;
			if (index < data.Length && data[index] != 0)
				data[index]++;
		}
		if (y > 0)
		{
			//Up
			index = x + 0 + (y - 1) * 10;
			if (index >= 0 && data[index] != 0)
				data[index]++;
			if (x > 0)
			{
				//Up Left
				index = x - 1 + (y - 1) * 10;
				if (index >= 0 && data[index] != 0)
					data[index]++;
			}
			if (x < 9)
			{
				//Up Right
				index = x + 1 + (y - 1) * 10;
				if (index < data.Length && data[index] != 0)
					data[index]++;
			}
		}

		if (y < 9)
		{
			//Bottom
			index = x + 0 + (y + 1) * 10;
			if (index < data.Length && data[index] != 0)
				data[index]++;
			if (x > 0)
			{
				//Bottom Left
				index = x - 1 + (y + 1) * 10;
				if (index < data.Length && data[index] != 0)
					data[index]++;
			}
			if (x < 9)
			{
				//Bottom Right
				index = x + 1 + (y + 1) * 10;
				if (index < data.Length && data[index] != 0)
					data[index]++;
			}
		}
	}

	//public override string ToString()
	//{
	//	var output = new StringBuilder();
	//	for (int y = 0; y < 10; y++)
	//	{
	//		for (int x = 0; x < 10; x++)
	//		{
	//			output.Append(DataPart1[x + y * 10]);
	//		}
	//		output.AppendLine();
	//	}
	//	return output.ToString();
	//}

	//public void Render()
	//{
	//	for (int y = 0; y < 10; y++)
	//	{
	//		for (int x = 0; x < 10; x++)
	//		{
	//			var index = x + y * 10;
	//			if (DataPart1[index] == 0)
	//				Console.ForegroundColor = ConsoleColor.Magenta;
	//			else
	//				Console.ForegroundColor = ConsoleColor.White;
	//			Console.Write(DataPart1[index]);
	//		}
	//		Console.WriteLine();
	//	}
	//}
}