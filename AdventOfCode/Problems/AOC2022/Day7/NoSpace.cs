using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day7;

[ProblemInfo(2022, 7, "No Space Left On Device")]
internal class NoSpace : Problem<int, int>
{
	private DirectoryNode _dirTree = new DirectoryNode("/");

	public override void CalculatePart1()
	{
		var dirs = _dirTree.Where(d => d.Size <= 100000);
		Part1 = dirs.Sum(d => d.Size);
	}

	public override void CalculatePart2()
	{
		var neededPace = 30000000;
		var totalSize = 70000000;
		var unusedSpace = totalSize - _dirTree.Size;
		var targetSize = neededPace - unusedSpace;
		var bigEnough = _dirTree.Where(d => d.Size >= targetSize);
		Part2 = bigEnough.MinBy(d => d.Size)?.Size ?? 0;
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");

		var curDir = new Stack<string>();

		var dir = "/";
		foreach (var line in lines)
		{
			if (line[0] == '$')
			{
				ParseCommand(line[2..], ref curDir);
				dir = $"/{string.Join("/", curDir.Reverse())}";
				_dirTree.AddDirectory(dir);
			}
			else
				ReadDirectory(line, _dirTree.GetDirectory(dir)!);
		}
	}

	private static void ReadDirectory(string line, DirectoryNode curDir)
	{
		if (line.StartsWith("dir"))
			return;

		var split = line.Split(' ');
		var name = split[1];
		var size = int.Parse(split[0]);
		curDir.AddFile(name, size);
	}

	private static void ParseCommand(string command, ref Stack<string> curDir)
	{
		var split = command.Split(' ');
		var keyword = split.First();
		var param = split.Last();

		switch (keyword)
		{
			case "cd":
				if (param == "..")
					curDir.Pop();
				else if (param == "/")
					curDir.Clear();
				else
					curDir.Push(param);
				break;
		}
	}
}