using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Problems.AOC2025.Day10;

public class JoltageGraph
{
	private readonly uint[] _target;
	private readonly (int id, uint[] ops)[] _ops;
	private readonly Node _root;
	private readonly List<Node> _nodes;

	public JoltageGraph(uint[] target, uint[][] ops)
	{
		_target = target;
		_ops = [.. ops.Select((ops, id) => (id, ops))];
		_root = new(target.Length, 0);
		_nodes = [_root];
	}

	public int Solve()
	{
		var open = new List<Node>() { _root };
		while (open.Count != 0)
		{
			var curNode = open[0];
			open.RemoveAt(0);
			//if (curNode.IsOverJoltage(_target))
			//	continue;
			foreach (var (opId, op) in _ops)
			{
				if (curNode.HasOp(opId))
					continue;
				var v = curNode.ApplyJoltageOperation(op);
				if (IsOverJoltage(v))
					continue;
				var existing = _nodes.FirstOrDefault(n => n == v);
				if (existing != null)
				{
					curNode.AddConnection(opId, existing.Id);
					existing.AddBackConnection(curNode.Id);
				}
				else
				{
					var newNode = new Node(v, _nodes.Count);
					_nodes.Add(newNode);
					curNode.AddConnection(opId, newNode.Id);
					newNode.AddBackConnection(curNode.Id);
					open.Add(newNode);
					if(newNode == _target)
					{
						open.Clear();
						break;
					}
				}
			}
		}
		return TraverseToTarget();
	}

	private bool IsOverJoltage(uint[] value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] > _target[i])
				return true;
		}
		return false;
	}

	public int TraverseToTarget()
	{
		var visited = new Dictionary<int, int>();
		return ReverseTraverse(_nodes.First(n => n == _target), visited, 0);
	}

	private int Traverse(Node node, Dictionary<int, int> visited, int depth = 0)
	{
		if(visited.TryGetValue(node.Id, out var pd))
		{
			if (pd > depth)
				visited[node.Id] = depth;
		}
		else
		{
			visited.Add(node.Id, depth);
		}
		if (node == _target)
			return depth;
		var best = int.MaxValue;
		foreach (var idx in node.Connections.Values)
		{
			var next = _nodes[idx];
			if (visited.TryGetValue(next.Id, out var vd) && vd < depth)
				continue;
			var d = Traverse(next, visited, depth + 1);
			if (d < best)
				best = d;
		}
		return best;
	}

	private int ReverseTraverse(Node node, Dictionary<int, int> visited, int depth = 0)
	{
		if (visited.TryGetValue(node.Id, out var pd))
		{
			if (pd > depth)
				visited[node.Id] = depth;
		}
		else
		{
			visited.Add(node.Id, depth);
		}
		if (node.Id == 0)
			return depth;
		var best = int.MaxValue;
		foreach (var idx in node.BackConnections)
		{
			var nextNode = _nodes[idx];
			if (visited.TryGetValue(nextNode.Id, out var vd) && vd < depth)
				continue;
			var d = ReverseTraverse(nextNode, visited, depth + 1);
			if (d < best)
				best = d;
		}
		return best;
	}

	private class Node : IEquatable<uint[]>, IEquatable<Node>
	{
		public int Id { get; }
		public uint[] Value { get; }
		public Dictionary<int, int> Connections { get; } = [];
		public List<int> BackConnections { get; } = [];

		public Node(int size, int id)
		{
			Id = id;
			Value = new uint[size];
		}

		public Node(uint[] value, int id)
		{
			Id = id;
			Value = value;
		}

		public bool HasOp(int opId)
		{
			return Connections.ContainsKey(opId);
		}
		public bool IsConnectedTo(int nodeId)
		{
			return Connections.ContainsValue(nodeId);
		}

		public bool AddConnection(int opId, int toNode)
		{
			return Connections.TryAdd(opId, toNode);
		}

		public void AddBackConnection(int nodeId)
		{
			BackConnections.Add(nodeId);
		}

		public uint[] ApplyJoltageOperation(uint[] ops)
		{
			var result = new uint[Value.Length];
			for (uint i = 0; i < Value.Length; i++)
			{
				if (ops.Contains(i))
					result[i] = Value[i] + 1;
				else
					result[i] = Value[i];
			}
			return result;
		}

		public bool IsOverJoltage(uint[] target)
		{
			for (int i = 0; i < Value.Length; i++)
			{
				if (Value[i] > target[i])
					return true;
			}
			return false;
		}

		public bool Equals(uint[]? other)
		{
			if (other is null) return false;
			if (other.Length != Value.Length) return false;
			for (int i = 0; i < Value.Length; i++)
			{
				if (Value[i] != other[i])
					return false;
			}
			return true;
		}

		public static bool operator ==(Node? node, uint[]? value)
		{
			if (node is null) return false;
			return node.Equals(value);
		}

		public static bool operator !=(Node? node, uint[]? value)
		{
			if (node is null) return false;
			return !node.Equals(value);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
			if (obj is null)
				return false;
			if (obj is Node node)
				return Equals(node);
			if (obj is uint[] value)
				return Equals(value);
			return false;
		}

		public bool Equals(Node? other)
		{
			if (other is null)
				return false;
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"[{Id}]: {string.Join(",", Value)}";
		}
	}
}