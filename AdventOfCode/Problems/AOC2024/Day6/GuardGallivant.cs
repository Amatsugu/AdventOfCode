﻿using AdventOfCode.Utils.Models;

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

	//this does not work
	public override void CalculatePart2()
	{
		var start = GetStartPos();
		var map = new GuardMap(_data, start);
		var path = map.GetPath();
		var visited = path.Select(p => p.pos).ToFrozenSet();
		var nodes = new List<GuardNode>();
		var found = new HashSet<Vec2<int>>();
		foreach (var (pos, node) in path)
		{
			var turn = (node.Direction + 1) % 4;
			if (pos == start)
				continue;
			if (map.GetNextObstacle(pos, turn, out var next))
			{
				var obstacleNode = new GuardNode(pos, turn, 0);
				var obstaclePos = pos + DIRS[node.Direction];
				if (!IsInBounds(obstaclePos))
					continue;
				if (map.SolveNewPath(obstacleNode, pos + DIRS[node.Direction], nodes))
				{
					if (!found.Contains(obstaclePos))
					{
						Part2++;
						//map.PrintBoard(nodes, pos, obstaclePos);
						//Console.ReadLine();
					}
					found.Add(obstaclePos);
				}
			}
		}
	}

	private Vec2<int> GetStartPos()
	{
		for (int y = 0; y < _data.Length; y++)
		{
			var row = _data[y];
			for (int x = 0; x < row.Length; x++)
			{
				if (row[x] == '^')
					return new(x, y);
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

	public override void LoadInput()
	{
		_data = ReadInputLines("input.txt").Select(r => r.ToCharArray()).ToArray();
		_height = _data.Length;
		_width = _data[0].Length;
	}
}

file class GuardMap
{
	public List<GuardNode> Nodes { get; set; }
	private readonly bool[][] _map;

	private readonly int _height;
	private readonly int _width;

	public GuardMap(char[][] map, Vec2<int> start)
	{
		_map = map.Select(r => r.Select(c => c == '#' ? true : false).ToArray()).ToArray();
		_height = map.Length;
		_width = map[0].Length;
		var startNode = new GuardNode(start, 0, 0);
		Nodes = [startNode];
		Solve();
	}

	public List<(Vec2<int> pos, GuardNode node)> GetPath()
	{
		return GetPath(Nodes);
	}

	public FrozenSet<Vec2<int>> GetPathSet(List<GuardNode> nodes)
	{
		var path = GetPath(nodes);
		return path.Select(p => p.pos).ToFrozenSet();
	}

	public List<(Vec2<int> pos, GuardNode node)> GetPath(List<GuardNode> nodes)
	{
		var path = new List<(Vec2<int>, GuardNode)>();

		var curNode = nodes[0];
		while (true)
		{
			if (curNode.Next is int nextId && nextId >= 0)
			{
				var next = nodes[nextId];
				path.AddRange(GetPointsBetween(curNode.Pos, next.Pos, curNode.Direction).Select(p => (p, curNode)));
				curNode = next;
			}
			else if (curNode.Next == -1)
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
			else
				break;
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

	private void Solve()
	{
		var curNode = Nodes[0];
		while (true)
		{
			if (!GetNextObstacle(curNode.Pos, curNode.Direction, out var next))
			{
				curNode.Next = -1;
				Nodes[curNode.Id] = curNode;
				break;
			}
			var newNode = new GuardNode(next - GuardGallivant.DIRS[curNode.Direction], (curNode.Direction + 1) % 4, Nodes.Count);
			curNode.Next = newNode.Id;
			Nodes[curNode.Id] = curNode;
			Nodes.Add(newNode);
			curNode = newNode;
		}
	}

	public bool SolveNewPath(GuardNode start, Vec2<int> extraObsticle, List<GuardNode> nodes)
	{
		var curNode = start;
		nodes.Clear();
		nodes.Add(start);
		while (true)
		{
			if (!GetNextObstacle(curNode.Pos, curNode.Direction, out var next, extraObsticle))
				return false;
			var newNode = new GuardNode(next - GuardGallivant.DIRS[curNode.Direction], (curNode.Direction + 1) % 4, nodes.Count);
			if (nodes.Any(n => n.Pos == newNode.Pos && n.Direction == newNode.Direction))
			{
				return true;
			}
			curNode.Next = newNode.Id;
			nodes[curNode.Id] = curNode;
			curNode = newNode;
			nodes.Add(newNode);
		}
	}

	public bool GetNextObstacle(Vec2<int> start, int dir, out Vec2<int> pos, Vec2<int>? extraObsticle = null)
	{
		if (extraObsticle is Vec2<int> ex)
			_map[ex.Y][ex.X] = true;
		pos = default;

		var (sX, sY) = start;
		switch (dir)
		{
			case 0:
				for (int y = sY; y >= 0; y--)
				{
					if (_map[y][sX])
					{
						pos = new(sX, y);
						ResetExtraObsticle();
						return true;
					}
				}
				break;

			case 1:
				var rowRight = _map[sY];
				for (int x = sX; x < _width; x++)
				{
					if (rowRight[x])
					{
						pos = new(x, sY);
						ResetExtraObsticle();
						return true;
					}
				}
				break;

			case 2:
				for (int y = sY; y < _height; y++)
				{
					if (_map[y][sX])
					{
						pos = new(sX, y);
						ResetExtraObsticle();
						return true;
					}
				}
				break;

			case 3:
				var rowLeft = _map[sY];
				for (int x = sX; x >= 0; x--)
				{
					if (rowLeft[x])
					{
						pos = new(x, sY);
						ResetExtraObsticle();
						return true;
					}
				}
				break;
		}

		void ResetExtraObsticle()
		{
			if (extraObsticle is Vec2<int> ex)
				_map[ex.Y][ex.X] = false;
		}
		ResetExtraObsticle();
		return false;
	}

	public void PrintBoard(List<GuardNode> nodes, Vec2<int> start, Vec2<int>? extraObsticle = null)
	{
		var path = GetPathSet(nodes);
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				Console.ResetColor();
				var p = new Vec2<int>(x, y);
				var node = nodes.FirstOrDefault(n => n.Pos == p, new GuardNode(p, 0, -1));
				if (node.Id != -1)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					if (node.Id == 0)
						Console.ForegroundColor = ConsoleColor.Yellow;
					else if (node.Id == nodes.Count - 1)
						Console.ForegroundColor = ConsoleColor.Red;
					switch (node.Direction)
					{
						case 0:
							Console.Write('^');
							break;

						case 1:
							Console.Write('>');
							break;

						case 2:
							Console.Write('v');
							break;

						case 3:
							Console.Write('<');
							break;
					}
					continue;
				}
				if (p == start)
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Write('S');
				}
				if (_map[y][x])
					Console.Write('#');
				else if (p == extraObsticle)
				{
					Console.ForegroundColor = ConsoleColor.DarkBlue;
					Console.Write('$');
				}
				else if (path.Contains(p))
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write('.');
				}
				else
					Console.Write(' ');
			}
			Console.WriteLine();
		}
	}
}

file struct GuardNode
{
	public int Id { get; private set; }
	public Vec2<int> Pos { get; }
	public int Direction { get; }
	public int? Next { get; set; }
	public GuardNode(Vec2<int> pos, int dir, int id)
	{
		Pos = pos;
		Direction = dir;
		Id = id;
	}

	public bool IsLoop(List<GuardNode> nodes)
	{
		return NodeExists(Id, nodes);
	}

	public bool NodeExists(int target, List<GuardNode> nodes)
	{
		if (Next == target)
			return true;
		if (Next is int id)
			return nodes[id].NodeExists(target, nodes);
		return false;
	}

	public override string ToString()
	{
		return $"{Pos}: {Direction}";
	}
}