namespace AdventOfCode.Problems.AOC2022.Day9;

public class RopeSimulator
{
	public int Visited => _visited.Count;

	private readonly (Direction dir, int ammount)[] _movement;
	private readonly bool _render;

	public enum Direction
	{
		L,
		R,
		U,
		D
	}

	private (int x, int y) _head;
	private (int x, int y)[] _segments;
	private Dictionary<(int x, int y), int> _visited;

	public RopeSimulator((Direction dir, int ammount)[] movement, int segments = 1, bool render = false)
	{
		_movement = movement;
		_render = render;
		_visited = new Dictionary<(int x, int y), int>()
		{
			{ (0,0), 1 }
		};
		_head = (0, 0);
		_segments = Enumerable.Repeat((0, 0), segments).ToArray();
	}

	public void Simulate()
	{
		foreach (var (dir, ammount) in _movement)
		{
			ProcessStep(dir, ammount);
		}
	}

	private void ProcessStep(Direction dir, int ammount)
	{
		for (int i = 0; i < ammount; i++)
		{
			switch (dir)
			{
				case Direction.L:
					_head.x--;
					break;

				case Direction.R:
					_head.x++;
					break;

				case Direction.U:
					_head.y++;
					break;

				case Direction.D:
					_head.y--;
					break;
			}
			FollowHead();
			var tail = _segments.Last();
			if (_visited.ContainsKey(tail))
				_visited[tail]++;
			else
				_visited.Add(tail, 1);
		}
		
	}

	private void FollowHead()
	{
		var curHead = _head;
		for (int i = 0; i < _segments.Length; i++)
		{
			var tail = _segments[i];
			curHead = _segments[i] = FollowSegment(curHead, tail);
		}
		if (_render)
		{
			DrawScene();
			Console.ReadKey();
		}
	}

	private static (int x, int y) FollowSegment((int x, int y) head, (int x, int y) tail)
	{
		var xDist = head.x - tail.x;
		var yDist = head.y - tail.y;

		if (xDist * xDist + yDist * yDist < 4)
			return tail;

		if (xDist > 1)
			head.x -= 1;
		if (xDist < -1)
			head.x += 1;

		if (yDist > 1)
			head.y -= 1;
		if (yDist < -1)
			head.y += 1;

		return head;
	}

	private static bool ShouldMoveTail((int x, int y) head, (int x, int y) tail)
	{
		var xDist = Math.Abs(head.x - tail.x);
		var yDist = Math.Abs(head.y - tail.y);
		return (xDist > 1 || yDist > 1);
	}

	private void DrawScene(int segment = -1)
	{
		var upperX = Math.Max(_head.x, _segments.MaxBy(s => s.x).x);
		var upperY = Math.Max(_head.y, _segments.MaxBy(s => s.y).y);
		var lowerX = Math.Min(_head.x, _segments.MinBy(s => s.y).y);
		var lowerY = Math.Min(_head.y, _segments.MinBy(s => s.y).y);

		var width = upperX - lowerX;
		var height = upperY - lowerY;

		Console.Clear();
		Console.ForegroundColor = ConsoleColor.Gray;
		Draw('s', (0, 0), lowerX, upperY);
		Draw('H', _head, lowerX, upperY);

		for (int i = _segments.Length -1; i >= 0; i--)
		{
			if(segment  == i)
				Console.ForegroundColor= ConsoleColor.Red;
			else
				Console.ForegroundColor= ConsoleColor.Gray;
			Draw((i + 1).ToString()[0], _segments[i], lowerX, upperY);
		}
	}

	private void Draw(char c, (int x, int y) pos, int offsetX, int offsetY)
	{
		Console.SetCursorPosition(pos.x + offsetX, -pos.y + offsetY);
		Console.WriteLine(c);
	}
}