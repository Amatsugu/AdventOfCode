
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pos = (int x, int y);

namespace AdventOfCode.Problems.AOC2024.Day4;
[ProblemInfo(2024, 4, "Ceres Search")]
internal class CeresSearch : Problem<int, int>
{

	private string[] _data = [];
	private static Pos[] dirs = [
		(-1, 0),	//Left
		(-1, -1),	//Top Left
		(0, -1),	//Top
		(1, -1),	//Top Right
		(1, 0),		//Right
		(1, 1),		//Bottom Right
		(0, 1),		//Bottom
		(-1, 1),	//Bottom Left
		];

	private static Pos[] xdirs = [
		(-1, -1),	//Top Left
		(1, -1),	//Top Right
		(1, 1),		//Bottom Right
		(-1, 1),	//Bottom Left
		];

	public override void CalculatePart1()
	{
		Part1 = FindWord(_data, "XMAS");
	}

	public override void CalculatePart2()
	{
		Part2 = FindXWord(_data, "MAS");
	}
	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt");
	}

	private static int FindXWord(string[] data, string target)
	{
		var matches = 0;
		var pivot = target.Length / 2;
		var tgt = target[pivot];
		var branchA = string.Join("", target[..(pivot+1)].Reverse());
		var branchB = target[pivot..];
		for (int y = 0; y < data.Length; y++)
		{
			var row = data[y];
			for (int x = 0; x < row.Length; x++)
			{
				var c = row[x];
				if (c == tgt)
				{
					for (int i = 0; i < xdirs.Length; i++)
					{
						Pos dir = xdirs[i];
						Pos opposingDir = xdirs[(i + 2) % xdirs.Length];

						if (CheckWord(data, (x, y), dir, branchA, 1, [c]) && CheckWord(data, (x,y), opposingDir, branchB, 1, [c]))
						{
							Pos dir2 = xdirs[(i+1) % xdirs.Length];
							Pos opposingDir2 = xdirs[(i + 3) % xdirs.Length];
							if (CheckWord(data, (x, y), dir2, branchA, 1, [c]) && CheckWord(data, (x, y), opposingDir2, branchB, 1, [c]))
								matches++;
						}
					}	
				}
			}
		}
		return matches;
	}

	private static int FindWord(string[] data, string target)
	{
		var matches = 0;
		var tgt = target[0];
		for (int y = 0; y < data.Length; y++)
		{
			var row = data[y];
			for (int x = 0; x < row.Length; x++)
			{
				var c = row[x];
				if(c == tgt)
				{
					foreach (var dir in dirs)
					{
						if(CheckWord(data, (x, y), dir, target, 1, [c]))
							matches++;
					}
				}
			}
		}
		return matches;
	}

	private static bool CheckWord(string[] data, Pos pos, Pos dir, string target, int targetPos, List<char>? acc = null)
	{
		if (acc == null)
			acc = [];
		Pos curPos = (pos.x + dir.x, pos.y + dir.y);


		if(curPos.y < 0 || curPos.y >= data.Length)
			return false;
		if (curPos.x < 0 || curPos.x >= data[0].Length)
			return false;

		var c = data[curPos.y][curPos.x];
		if(c == target[targetPos])
		{
			acc.Add(c);
			if (targetPos == target.Length - 1)
				return true;
			return CheckWord(data, curPos, dir, target, targetPos + 1, acc);
		}
		return false;
	}


	
}
