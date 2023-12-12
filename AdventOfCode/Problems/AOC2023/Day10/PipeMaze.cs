using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day10;

[ProblemInfo(2023, 10, "Pipe Maze")]
internal class PipeMaze : Problem<int, int>
{
	private string[] _maze = [];

	public override void LoadInput()
	{
		_maze = ReadInputLines("sample.txt");
	}

	public override void CalculatePart1()
	{
		Console.WriteLine(GetNextPoint((1, 1), (1, 2)));
		Console.WriteLine(GetNextPoint((1, 1), (2, 1)));

		Console.WriteLine(GetNextPoint((3, 1), (3, 2)));
		Console.WriteLine(GetNextPoint((3, 1), (2, 1)));

		Console.WriteLine(GetNextPoint((3, 3), (3, 2)));
		Console.WriteLine(GetNextPoint((3, 3), (2, 3)));

		Console.WriteLine(GetNextPoint((1, 3), (1, 2)));
		Console.WriteLine(GetNextPoint((1, 3), (2, 3)));
	}

	private (int x, int y) GetNextPoint((int x, int y) pos, (int x, int y) prev)
	{
		var curPipe = _maze[pos.y][pos.x];
		var dir = (x: pos.x - prev.x, y: pos.y - prev.y);
		if(curPipe == 'S')
		{
			throw new Exception();
		}
		return curPipe switch
		{
			'|' => (pos.x, pos.y + dir.y),
			'-' => (pos.x + dir.x, pos.y),
			'L' => (pos.x + dir.y, pos.y + dir.x),
			'F' => (pos.x - dir.y, pos.y - dir.x),
			'J' => (pos.x - dir.y, pos.y - dir.x),
			'7' => (pos.x + dir.y, pos.y + dir.x),
			_ => throw new Exception()
		};
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

}
