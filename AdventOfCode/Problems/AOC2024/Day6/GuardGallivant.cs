using AdventOfCode.Utils.Models;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turn = (AdventOfCode.Utils.Models.Vec2<int> pos, int dir, int step);

namespace AdventOfCode.Problems.AOC2024.Day6;

[ProblemInfo(2024, 6, "Guard Gallivant")]
internal class GuardGallivant : Problem<int, int>
{
	private char[][] _data = [];
	private int _height;
	private int _width;
	public static readonly Vec2<int>[] DIRS = [
			new (0, -1),
			new (1, 0),
			new (0, 1),
			new (-1, 0),
		];

	public override void CalculatePart1()
	{
		var map = new GuardMap(_data, GetStartPos());
		Part1 = map.GetPath().DistinctBy(p => p.pos).Count();
	}

	private FrozenSet<(Vec2<int> pos, int dir)> GetVisited(out List<Turn> turns)
	{
		var visited = new HashSet<(Vec2<int> pos, int dir)>();
		turns = [];
		var pos = GetStartPos();
		var dir = 0;
		var step = 0;
		while (IsInBounds(pos))
		{
			var curDir = DIRS[dir];
			visited.Add((pos, dir));
			if (CanMove(pos, curDir, _data))
				pos += curDir;
			else
			{
				turns.Add((pos, dir, step));
				dir = (dir + 1) % DIRS.Length;
			}
			step++;
		}
		//PrintBoard(visited.Select(v => v.pos).ToFrozenSet(), pos, _data);
		return visited.ToFrozenSet();
	}

	private void PrintBoard(FrozenSet<Vec2<int>> visited, Vec2<int> pos, char[][] board, Vec2<int>? obsticle = null)
	{
		Console.WriteLine("======================");
		for (int y = 0; y < board.Length; y++)
		{
			var row = board[y];
			for (int x = 0; x < row.Length; x++)
			{
				var p = new Vec2<int>(x, y);
				Console.ResetColor();
				if(p == obsticle)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write('O');
					continue;
				}
				if (row[x] == '#')
				{
					Console.Write(row[x]);
					continue;
				}
				if (row[x] == '^')
				{
					Console.ForegroundColor = ConsoleColor.DarkBlue;
					if(pos == p)
					{
						Console.Write('@');
						continue;
					}
					if (visited.Contains(p))
						Console.Write('X');
					else
						Console.Write('S');
				}
				else
				{
					if(p == pos)
						Console.Write('@');
					else if (visited.Contains(new(x, y)))
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write('X');
					}
					else
						Console.Write(row[x] == '.' ? ' ' : row[x]);
				}
			}
			Console.WriteLine();
		}
	}

	private bool CanMove(Vec2<int> pos, Vec2<int> dir, char[][] board)
	{
		var p = pos + dir;
		if(IsInBounds(p))
			return board[p.Y][p.X] == '.' || board[p.Y][p.X] == '^';
		return true;
	}

	private Vec2<int> GetStartPos()
	{
		for (int y = 0; y < _data.Length; y++)
		{
			var row = _data[y];
			for (int x = 0; x < row.Length; x++)
			{
				if (row[x] == '^')
					return new (x, y);
			}
		}
		throw new Exception("Start Position not found");
	}

	private bool IsInBounds(Vec2<int> pos)
	{
		if (pos.X < 0 || pos.Y < 0)
			return false;
		if (pos.X >= _width || pos.Y >= _height)
			return false;
		return true;
	}

	public override void CalculatePart2()
	{
		var start = GetStartPos();
		var map = new GuardMap(_data, start);
		var path = map.GetPath();
		var visited = path.Select(p => p.pos).ToFrozenSet();
		foreach (var (pos, node) in path)
		{
			var turn = (node.Direction + 1) % 4;
			if (pos == start && node.Direction == 0)
				continue;
			if(map.GetNextObstacle(pos, turn, out var next)){
				var obstacle = new GuardNode(pos, turn);
				var nextNode = map.Nodes.FirstOrDefault(p => p.Pos == next - DIRS[turn] && p.Direction == (turn + 1) % 4);
				if(nextNode != null)
				{
					var tmp = node.Next;
					node.Next = obstacle;
					obstacle.Next = nextNode;
					if(obstacle.IsLoop())
						Part2++;
					node.Next = tmp;
				}
				else
				{
					//PrintBoard(visited, pos, _data, pos + DIRS[node.Direction]);
					//if(map.SolveNewPath(obstacle, pos + DIRS[node.Direction]))
					//{
					//	Part2++;
					//}
				}
			}
		}
	}



	public override void LoadInput()
	{
		_data = ReadInputLines("shino.txt").Select(r => r.ToCharArray()).ToArray();
		_height = _data.Length;
		_width = _data[0].Length;
	}
}

file class GuardMap
{
	public GuardNode Start { get; set; }
	public List<GuardNode> Nodes { get; set; }

	private readonly List<Vec2<int>> _obstacles = [];

	private readonly int _height;
	private readonly int _width;

	public GuardMap(char[][] map, Vec2<int> start)
	{
		_height = map.Length;
		_width = map[0].Length;
		Start = new GuardNode(start, 0);
		Nodes = [Start];
		FindObstacles(map);
		Solve();
	}

	public List<(Vec2<int> pos, GuardNode node)> GetPath()
	{
		var path = new List<(Vec2<int>, GuardNode)>();

		var curNode = Start;
		while (true)
		{

			if (curNode.Next == null)
			{
				var end = curNode.Direction switch
				{
					0 => new Vec2<int>(curNode.Pos.X, -1),
					1 => new Vec2<int>(_width, curNode.Pos.Y),
					2 => new Vec2<int>(curNode.Pos.X, _height),
					3 => new Vec2<int>(-1, curNode.Pos.Y),
					_ => throw new InvalidOperationException()
				};
				path.AddRange(GetPointsBetween(curNode.Pos, end, curNode.Direction).Select(p => (p, curNode)));
				break;
			}
			path.AddRange(GetPointsBetween(curNode.Pos, curNode.Next.Pos, curNode.Direction).Select(p => (p, curNode)));
			curNode = curNode.Next;
		}

		return path;
	}

	private List<Vec2<int>> GetPointsBetween(Vec2<int> start, Vec2<int> end, int dir)
	{
		var result = new List<Vec2<int>>();
		switch (dir)
		{
			case 0:
				for (int i = start.Y; i > end.Y; i--)
					result.Add(new Vec2<int>(start.X, i));
				break;
			case 1:
				for (int i = start.X; i < end.X; i++)
					result.Add(new Vec2<int>(i, start.Y));
				break;
			case 2:
				for (int i = start.Y; i < end.Y; i++)
					result.Add(new Vec2<int>(start.X, i));
				break;
			case 3:
				for (int i = start.X; i > end.X; i--)
					result.Add(new Vec2<int>(i, start.Y));
				break;
		}

		return result;
	}

	private void FindObstacles(char[][] map)
	{
		for (int y = 0; y < map.Length; y++)
		{
			for (int x = 0; x < map[0].Length; x++)
			{
				if (map[y][x] == '#')
					_obstacles.Add(new(x, y));
			}
		}
	}

	private void Solve()
	{
		var curNode = Start;
		while (true)
		{
			if(!GetNextObstacle(curNode.Pos, curNode.Direction, out var next))
				break;
			curNode.Next = new GuardNode(next - GuardGallivant.DIRS[curNode.Direction], (curNode.Direction + 1) % 4);
			curNode = curNode.Next;
			Nodes.Add(curNode);
		}
	}

	public bool SolveNewPath(GuardNode start, Vec2<int> extraObsticle)
	{
		var curNode = start;
		var nodes = new List<GuardNode>()
		{
			start
		};
		while (true)
		{
			if (nodes.Count > 4)
				return false;
			if (!GetNextObstacle(curNode.Pos, curNode.Direction, out var next, extraObsticle))
				return false;
			var newNode = new GuardNode(next - GuardGallivant.DIRS[curNode.Direction], (curNode.Direction + 1) % 4);
			if (nodes.Any(n => n.Pos == newNode.Pos && n.Direction == newNode.Direction))
				return true;
			curNode.Next = newNode;
			curNode = curNode.Next;
		}
	}

	public bool GetNextObstacle(Vec2<int> start, int dir, out Vec2<int> pos, Vec2<int>? extraObsticle = null)
	{
		var obstacles = extraObsticle != null ? _obstacles.Append((Vec2<int>)extraObsticle) : _obstacles;
		pos = default;
		switch (dir)
		{
			case 0:
				var up = obstacles.Where(o => o.X == start.X && o.Y < start.Y);
				if (up.Any())
				{
					pos = up.MaxBy(o => o.Y);
					return true;
				}
				break;
			case 1:
				var right = obstacles.Where(o => o.Y == start.Y && o.X > start.X);
				if (right.Any())
				{
					pos = right.MinBy(o => o.X);
					return true;
				}
				break;
			case 2:
				var down = obstacles.Where(o => o.X == start.X && o.Y > start.Y);
				if (down.Any())
				{
					pos = down.MinBy(o => o.Y);
					return true;
				}
				break;
			case 3:
				var left = obstacles.Where(o => o.Y == start.Y && o.X < start.X);
				if (left.Any())
				{
					pos = left.MaxBy(o => o.X);
					return true;
				}
				break;
		}
		return false;
	}
}

file class GuardNode
{
	public Vec2<int> Pos { get; }
	public int Direction { get; }
	public GuardNode? Next { get; set; }
	public GuardNode(Vec2<int> pos, int dir)
	{
		Pos = pos;
		Direction = dir;
	}

	public bool IsLoop()
	{
		return NodeExists(this);
	}

	public bool NodeExists(GuardNode target)
	{
		if (Next == null)
			return false;
		if (Next == target)
			return true;
		return Next.NodeExists(target);
	}

	public override string ToString()
	{
		return $"{Pos}: {Direction}";
	}
}