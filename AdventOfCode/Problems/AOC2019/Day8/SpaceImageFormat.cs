using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Problems.AOC2019.Day8
{
	[ProblemInfo(2019, 8, "Space Image Format")]
	public class SpaceImageFormat : Problem<int, string>
	{
		private int[] _imageData = Array.Empty<int>();

		public static void Execute()
		{
			var imageData = File.ReadAllText("Day8/input.txt").Replace("\n", "").Select(c => int.Parse(c.ToString())).ToArray();

			Console.WriteLine(Checksum(imageData, 25, 6));

		}

		public static void RenderImage(int[] data, int h, int w)
		{
			var imgSize = h * w;
			var layerCount = data.Length / imgSize;

			for (int l = 0; l < layerCount; l++)
			{

			}
		}


		public static int Checksum(int[] data, int h, int w)
		{
			var imgSize = h * w;
			var layerCount = data.Length / imgSize;

			int[] zeroCount = new int[layerCount];
			int[] oneCount = new int[layerCount];
			int[] twoCount = new int[layerCount];

			int smallestLayer = -1;
			int smallestLayerCount = int.MaxValue;

			for (int l = 0; l < layerCount; l++)
			{
				for (int i = imgSize * l; i < imgSize * (l + 1); i++)
				{
					switch (data[i])
					{
						case 0:
							zeroCount[l]++;
							break;
						case 1:
							oneCount[l]++;
							break;
						case 2:
							twoCount[l]++;
							break;
					}
				}
				if (zeroCount[l] <= smallestLayerCount)
				{
					smallestLayer = l;
					smallestLayerCount = zeroCount[l];
				}
			}

			return oneCount[smallestLayer] * twoCount[smallestLayer];

		}

		public override void LoadInput()
		{
			_imageData = ReadInputText().Replace("\n", "").Select(c => int.Parse(c.ToString())).ToArray();
		}

		public override void CalculatePart1()
		{
			Part1 = Checksum(_imageData, 25, 6);
		}

		public override void CalculatePart2()
		{
			Part2 = null;
		}
	}
}
