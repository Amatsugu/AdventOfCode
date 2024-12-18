using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day9;

[ProblemInfo(2024, 9, "Disk Fragmenter")]
internal class DiskFragmenter : Problem<long, long>
{
	private int[] _data = [];

	public override void CalculatePart1()
	{
		var data = Expand(_data);
		Compact(data);
		Part1 = ComputeHash(data);
	}

	public override void CalculatePart2()
	{
		var blocks = ExpandBlocks(_data);
		var empty = blocks.Where(b => b.isEmpty).Sum(b => b.length);
		var files = blocks.Where(b => !b.isEmpty).Sum(b => b.length);
		CompactV2(blocks);
		var empty2 = blocks.Where(b => b.isEmpty).Sum(b => b.length);
		var files2 = blocks.Where(b => !b.isEmpty).Sum(b => b.length);

		if (empty != empty2)
			Console.WriteLine("Empty space does not match");
		if (files != files2)
			Console.WriteLine($"Files space does not match Befor: {files} -> {files2}");
		//Print(blocks);
		// Too High: 8838426222802
		Part2 = ComputeHashV2(blocks);
	}

	public static void Print(List<Block> blocks, int idx = -1)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			Console.ResetColor();
			if(i == idx)
				Console.BackgroundColor = ConsoleColor.Green;
			if (blocks[i].isEmpty)
				Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(blocks[i].ToString());
		}
		Console.ResetColor();
		Console.WriteLine();
	}

	public static void Print(int[] data)
	{
		Console.WriteLine(data.Select(x => x == -1 ? " " : x.ToString()).AsJoinedString("").Trim());
	}

	public static int[] Expand(int[] data)
	{
		var result = new List<int>(data.Sum());
		var isFile = true;
		var id = 0;
		foreach (var item in data)
		{
			if (isFile)
				result.AddRange(Enumerable.Repeat(id++, item));
			else
				result.AddRange(Enumerable.Repeat(-1, item));
			isFile = !isFile;
		}
		return [.. result];
	}

	public static void Compact(int[] data)
	{
		var readPos = data.Length - 1;
		var writePos = 0;
		var curValue = 0;
		var isWriting = false;
		while (true)
		{
			if (readPos - 2 < writePos && !isWriting)
				break;
			if (isWriting)
			{
				while (writePos < data.Length)
				{
					if (data[writePos] == -1)
					{
						data[writePos] = curValue;
						data[readPos] = -1;
						curValue = -1;
						writePos++;
						break;
					}
					writePos++;
				}
				isWriting = false;
			}
			else
			{
				do
				{
					curValue = data[readPos];
					if (curValue != -1)
					{
						isWriting = true;
						break;
					}
					readPos--;
				}
				while (readPos >= 0);
			}
		}
	}

	public static long ComputeHash(int[] data)
	{
		long hash = 0;
		//Compensate to incomplete compaction
		long idx = 0;
		for (long i = 0; i < data.Length; i++)
		{
			if (data[i] == -1)
				continue;
			hash += (idx++) * (long)data[i];
		}
		return hash;
	}

	public static List<Block> ExpandBlocks(int[] data)
	{
		var blocks = new List<Block>();
		var isFile = true;
		var id = 0;
		foreach (var block in data)
		{
			if (block == 0)
			{
				isFile = !isFile;
				continue;
			}
			if (isFile)
				blocks.Add(new Block(id++, block));
			else
				blocks.Add(new Block(block));
			isFile = !isFile;
		}
		return blocks;
	}

	public static void CompactV2(List<Block> blocks)
	{
		for (int idx = blocks.Count - 1; idx >= 0; idx--)
		{
			var block = blocks[idx];
			if (block.isEmpty)
				continue;
			var emptyPos = FindFirstEmptyBlock(block.length, idx, blocks);
			if (emptyPos == -1)
				continue;
			var emptyBlock = blocks[emptyPos];
			//Replace Empty Block
			if (emptyBlock.length == block.length)
				blocks[emptyPos] = block;
			else
			{
				//Insert and Shrink Empty Block
				blocks.Insert(emptyPos, block);
				idx += 1;
				emptyBlock.length -= block.length;
			}

			//Merge Left and Right empty blocks
			if (idx + 1 < blocks.Count && blocks[idx + 1].isEmpty && idx - 1 > 0 && blocks[idx - 1].isEmpty)
			{
				blocks[idx - 1].length += block.length + blocks[idx + 1].length;
				blocks.RemoveAt(idx);
				blocks.RemoveAt(idx);
				idx -= 1;
			}
			//Extend Right Block
			else if (idx + 1 < blocks.Count && blocks[idx + 1].isEmpty)
			{
				blocks[idx + 1].length += block.length;
				blocks.RemoveAt(idx);
			}
			//Extend Left Block
			else if (idx - 1 > 0 && blocks[idx - 1].isEmpty)
			{
				blocks[idx - 1].length += block.length;
				blocks.RemoveAt(idx);
			}
			//Insert new Empty Block
			else
				blocks[idx] = new Block(block.length);
			//Print(blocks, idx);
		}

		static int FindFirstEmptyBlock(int length, int limit, List<Block> blocks)
		{
			for (int i = 0; i < limit; i++)
			{
				var block = blocks[i];
				if (block.isEmpty && block.length >= length)
					return i;
			}
			return -1;
		}
	}

	public static long ComputeHashV2(List<Block> blocks)
	{
		long hash = 0;
		long pos = 0;
		for (int i = 0; i < blocks.Count; i++)
		{
			var block = blocks[i];
			if (!block.isEmpty)
			{
				//There's probably an equation for this
				for (int x = 0; x < block.length; x++)
				{
					hash += block.id * pos++;
				}
			}else
				pos += block.length;
		}

		return hash;
	}

	public override void LoadInput()
	{
		_data = ReadInputText("input.txt").Select(x => (int)(x - '0')).ToArray();
	}
}

public class Block
{
	public bool isEmpty;
	public int id;
	public int length;

	public Block(int id, int length)
	{
		this.id = id;
		this.length = length;
	}

	public Block(int length)
	{
		isEmpty = true;
		this.length = length;
	}

	public override string ToString()
	{
		return isEmpty ? $"[{length}]" : $"{id}:{length}";
	}
}