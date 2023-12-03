using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2023.Day3;

[ProblemInfo(2023, 3, "Gear Ratios")]
internal class GearRatios : Problem<int, int>
{
	private string[] _data = [];
	private int _width;
	private int _height;

	public override void LoadInput()
	{
		_data = ReadInputLines();
		_width = _data[0].Length - 1;
		_height = _data.Length - 1;
	}

	public override void CalculatePart1()
	{
		var partNumbers = new List<int>();

		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x <= _width; x++)
			{
				var cell = _data[y][x];
				switch (cell - '0')
				{
					case '.' - '0':
						continue;
					case < 0 or > 9:
						FindNumbers(x, y, ref partNumbers);
						break;
				}
			}
		}
		Part1 = partNumbers.Sum();
	}

	public void FindNumbers(int x, int y, ref List<int> results)
	{
		var seen = new HashSet<(int x, int y)>();

		var n = GetNeighborPoints(x, y);
		foreach (var (xN, yN) in n)
		{
			var c = _data[yN][xN] - '0';
			if (c >= 0)
			{
				var num = GetNumber(xN, yN, out var idx);

				if (seen.Contains(idx))
					continue;
				seen.Add(idx);
				results.Add(num);
			}
		}
	}

	public int GetNumber(int x, int y, out (int iX, int iY) index)
	{
		var row = _data[y];

		var numStart = 0;
		var numEnd = _width + 1;
		for (int i = x; i >= 0; i--)
		{
			switch (row[i] - '0')
			{
				case < 0 or > 9:
					numStart = i + 1;
					goto leftDone;
			}
		}
	leftDone:
		for (int i = x; i <= _width; i++)
		{
			switch (row[i] - '0')
			{
				case < 0 or > 9:
					numEnd = i;
					goto done;
			}
		}
	done:
		index = (numStart, y);
		return int.Parse(row[numStart..numEnd]);
	}

	public List<(int x, int y)> GetNeighborPoints(int x, int y)
	{
		var points = new List<(int x, int y)>();
		if (x > 0)
			points.Add((x - 1, y));
		if (x < _width)
			points.Add((x + 1, y));
		if (y > 0)
			points.Add((x, y - 1));
		if (y < _height)
			points.Add((x, y + 1));
		if (x > 0 && y > 0)
			points.Add((x - 1, y - 1));
		if (x > 0 && y < _height)
			points.Add((x - 1, y + 1));
		if (x < _width && y < _height)
			points.Add((x + 1, y + 1));
		if (x < _width && y > 0)
			points.Add((x + 1, y - 1));

		return points;
	}

	public override void CalculatePart2()
	{
		var ratios = new List<int>();
		var curNums = new List<int>();
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x <= _width; x++)
			{
				var cell = _data[y][x];
				switch (cell)
				{
					case '*':
						curNums.Clear();
						FindNumbers(x, y, ref curNums);
						if(curNums.Count == 2)
							ratios.Add(curNums[0] * curNums[1]);
						break;
				}
			}
		}
		Part2 = ratios.Sum();
	}
}