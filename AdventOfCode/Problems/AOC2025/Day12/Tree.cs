using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;

namespace AdventOfCode.Problems.AOC2025.Day12;

public record Tree(Vec2i Size, int[] Presents)
{
	public int Area => Size.X * Size.Y;

	private class TreeNode
	{
		public TreeNode Root { get; private set; }
		public int ShapeId => Shape.Id;
		public Vec2i Position { get; set; }
		public PresentShape Shape { get; private set; }
		public List<TreeNode> Children { get; private set; } = [];
		public HashSet<Vec2i> Members { get; }
		public Vec2i Size { get; set; }

		public TreeNode(PresentShape shape, TreeNode root, HashSet<Vec2i> members)
		{
			Shape = shape;
			Root = root;
			Members = members;
			Size = CalculateSize(members);
		}

		public TreeNode(PresentShape shape)
		{
			Root = this;
			Shape = shape;
			Size = new Vec2i(3, 3);
			Members = [.. shape.Members];
		}

		private static Vec2i CalculateSize(HashSet<Vec2i> members)
		{

			var min = members.Aggregate((a, b) => a.Min(b));
			var max = members.Aggregate((a, b) => a.Max(b));
			return (min - max).Abs();
		}

	}

	public bool SearchPlacements(PresentShape[] shapes)
	{
		if (Area < Presents.Select((count, idx) => shapes[idx].Size * count).Sum())
			return false;
		var roots = new List<TreeNode>();

		for (int s = 0; s < Presents.Length; s++)
		{
			if (Presents[s] == 0)
				continue;
			var shape = shapes[s];
			roots.Add(new TreeNode(shape));
		}
		throw new NotImplementedException();
	}

	private void BuildGraph(TreeNode node, PresentShape[] shapes, int[] curPresents)
	{
		for (int i = 0; i < shapes.Length; i++)
		{
			if (Presents[i] == 0 || curPresents[i] >= Presents[i])
				continue;
			var newNode = new TreeNode(shapes[i]);
			node.Children.Add(newNode);
			int[] nodePresents = [.. curPresents];
			nodePresents[i]++;
			BuildGraph(newNode, shapes, nodePresents);
		}
	}

	public bool CanFitPresents(PresentShape[] shapes)
	{
		if (Area < Presents.Select((count, idx) => shapes[idx].Size * count).Sum())
			return false;
		var space = new char[Size.X, Size.Y];

		for (int s = 0; s < Presents.Length; s++)
		{
			if (Presents[s] == 0)
				continue;
			var shape = shapes[s];
			for (int i = 0; i < Presents[s]; i++)
			{
				if (!FitShape(shape, space, s.ToString()[0]))
				{
					PrintSpace(space, false);
					return false;
				}
			}
		}
		PrintSpace(space, true);
		return true;
	}

	private void PrintSpace(char[,] space, bool result)
	{
		Console.WriteLine();
		Console.WriteLine($"{Size}: {Presents.AsJoinedString()}");
		Console.WriteLine($"Result: {result}");
		for (int y = 0; y < Size.Y; y++)
		{
			for (int x = 0; x < Size.X; x++)
			{
				Console.Write(space[x, y] == 0 ? "." : space[x, y]);
			}
			Console.WriteLine();
		}
	}

	private bool FitShape(PresentShape shape, char[,] space, char id)
	{
		for (int x = 1; x < Size.X - 1; x++)
		{
			for (int y = 1; y < Size.Y - 1; y++)
			{
				var curShape = shape;
				var pos = new Vec2i(x, y);
				for (int r = 0; r < 4; r++)
				{
					if (ShapeFits(pos, curShape, space))
					{
						PlaceShape(pos, curShape, space, id);
						return true;
					}
					else
						curShape = curShape.RotateCW();
				}
			}
		}

		return false;
	}

	private bool ShapeFits(Vec2i pos, PresentShape shape, char[,] space)
	{
		foreach (var point in shape.Members)
		{
			var p = point + pos;
			if (space[p.X, p.Y] != 0)
				return false;
		}
		return true;
	}

	private static void PlaceShape(Vec2i pos, PresentShape shape, char[,] space, char id)
	{
		foreach (var point in shape.Members)
		{
			var p = pos + point;
			space[p.X, p.Y] = id;
		}
	}
}