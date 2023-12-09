using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day8;

[ProblemInfo(2023, 8, "Haunted Wasteland")]
internal class HauntedWasteland : Problem<int, long>
{
	private string _path = string.Empty;
	private Dictionary<string, (string left, string right)> _nodes = [];

	public override void LoadInput()
	{
		var data = ReadInputLines();
		_path = data[0];

		for (int i = 2; i < data.Length; i++)
		{
			var curLine = data[i].Split('=');
			var node = curLine[0].TrimEnd();
			var branches = curLine[1][2..^1].Split(", ");
			_nodes.Add(node, (branches[0], branches[1]));
		}
	}

	public override void CalculatePart1()
	{
		var curPos = "AAA";
		var i = 0;
		var steps = 0;
		do
		{
			curPos = _path[i] switch
			{
				'L' => _nodes[curPos].left,
				'R' => _nodes[curPos].right,
				_ => throw new Exception("Something went horribly wrong")
			};
			i = (i + 1) % _path.Length;
			steps++;
		} while (curPos != "ZZZ");
		Part1 = steps;
	}

	public override void CalculatePart2()
	{
		var curPos = _nodes.Keys.Where(n => n[^1] == 'A').ToArray();
		var len = new long[curPos.Length];
		var i = 0;
		do
		{
			for (int j = 0; j < curPos.Length; j++)
			{
				if (curPos[j][^1] == 'Z')
					continue;
				len[j]++;
				curPos[j] = _path[i] switch
				{
					'L' => _nodes[curPos[j]].left,
					'R' => _nodes[curPos[j]].right,
					_ => throw new Exception("Something went horribly wrong")
				};
			}
			i = (i + 1) % _path.Length;
		} while (curPos.Any(n => n[^1] != 'Z'));

		Part2 = len.LCM();
	}
}