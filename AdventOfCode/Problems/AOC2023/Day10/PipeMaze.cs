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
		_maze = ReadInputLines();
	}

	public override void CalculatePart1()
	{
		throw new NotImplementedException();
	}

	private (int x, int y) GetNextPoint((int x, int y) pos, (int x, int y) prev)
	{
		var curPipe = _maze[pos.y][pos.x];
		if(curPipe == 'S')
		{
			throw new Exception();
		}
		return curPipe switch
		{
			'|' => (pos.x, pos.y + (pos.y - prev.y)),
			'-' => (pos.x + (pos.x - prev.x), pos.y),
			'L' => (0,0),
			'F' => (0,0),
			'J' => (0,0),
			'7' => (0,0),
			_ => throw new Exception()
		};
	}

	public override void CalculatePart2()
	{
		throw new NotImplementedException();
	}

}
