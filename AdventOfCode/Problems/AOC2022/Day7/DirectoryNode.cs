using System.Collections;

namespace AdventOfCode.Problems.AOC2022.Day7;

internal class DirectoryNode 
{
	public DirectoryNode Parent { get; set; }
	public string Name { get; set; }
	public string FullPath => GetFullPath();
	public List<DirectoryNode> Children { get; set; }
	public List<(string name, int size)> Files { get; set; }
	public int Size => Files.Sum(f => f.size) + Children.Sum(c => c.Size);
	public int LocalSize => Files.Sum(f => f.size);

	public DirectoryNode(string name)
	{
		Name = name;
		Files = new();
		Children = new();
		Parent = this;
	}

	public DirectoryNode(string name, DirectoryNode parent)
	{
		Name = name;
		Files = new();
		Children = new();
		Parent = parent;
	}

	public DirectoryNode(string name, List<(string name, int size)> files)
	{
		Name = name;
		Files = files;
		Children = new();
		Parent = this;
	}

	public DirectoryNode AddDirectory(string path)
	{
		if (path == "/")
			return this;
		if(string.IsNullOrWhiteSpace(path))
			return this;
		var segments = path.Split("/").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
		if (segments.Length == 0)
			throw new Exception("Invalid Path?");
		return AddDirectory(segments);
	}

	private DirectoryNode AddDirectory(string[] segments)
	{
		var curSegment = segments[0];
		var child = Children.FirstOrDefault(c => c.Name == curSegment);
		if (child == null)
		{
			var node = new DirectoryNode(curSegment, this);
			Children.Add(node);
			if (segments.Length == 1)
				return node;
			else
				return node.AddDirectory(segments[1..]);
		}
		else
		{
			if (segments.Length == 1)
				return child;
			return child.AddDirectory(segments[1..]);
		}
	}

	public void AddFile(string name, int size)
	{
		Files.Add((name, size));
	}

	public DirectoryNode? GetDirectory(string path)
	{
		if (path == "/")
			return this;
		var segments = path.Split("/").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
		if (segments.Length == 0)
			throw new Exception("Invalid Path?");
		return GetDirectory(segments);
	}

	private DirectoryNode? GetDirectory(string[] segments)
	{
		if (Children.Count == 0)
			return null;

		var child = Children.FirstOrDefault(c => c.Name == segments[0]);

		if(child == null)
			return null;
		else
			if(segments.Length == 1)
				return child;
			else
				return child.GetDirectory(segments[1..]);
	}

	public List<DirectoryNode> Where(Func<DirectoryNode, bool> filter)
	{
		var result = new List<DirectoryNode>();
		if(filter(this))
			result.Add(this);
		result.AddRange(Children.SelectMany(c => c.Where(filter)));
		return result;
	}

	public string GetFullPath()
	{
		if (Parent == this)
			return "";
		return $"{Parent.GetFullPath()}/{Name}";
	}

	public override string ToString()
	{
		return $"{Name} - {Children.Count}";
	}
}