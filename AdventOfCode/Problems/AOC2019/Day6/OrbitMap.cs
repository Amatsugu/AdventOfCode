namespace AdventOfCode.Problems.AOC2019.Day6
{
	public class OrbitMap
	{
		public CelestialObject root;
		public Dictionary<string, CelestialObject> objectMap;

		public OrbitMap(string[] orbits)
		{
			objectMap = new Dictionary<string, CelestialObject>();
			GenerateOrbits(orbits);
		}

		public void GenerateOrbits(string[] orbits)
		{
			for (int i = 0; i < orbits.Length; i++)
			{
				var bodies = orbits[i].Split(')');
				if (bodies[0] == "COM")
					root = CreateObject("COM");

				var parent = GetOrCreateObject(bodies[0]);
				var child = GetOrCreateObject(bodies[1]);

				parent.AddChild(child);
			}
		}

		public CelestialObject GetOrCreateObject(string name)
		{
			if (objectMap.ContainsKey(name))
				return objectMap[name];
			else
				return CreateObject(name);
		}

		public CelestialObject CreateObject(string name)
		{
			var o = new CelestialObject(name);
			objectMap.Add(name, o);
			return o;
		}

		public int CalculateOrbits()
		{
			return root.GetOrbitCount();
		}

		public List<CelestialObject> FindPathTo(string name)
		{
			var path = new List<CelestialObject>();
			root.FindPathTo(name, path);
			return path;
		}

		public int GetDepthOf(string name) => root.GetDepth(name);

		public class CelestialObject
		{
			public string Name { get; set; }
			public int ChildCount => children.Count;

			public List<CelestialObject> children;

			public CelestialObject(string name)
			{
				children = new List<CelestialObject>();
				Name = name;
			}

			public void AddChild(CelestialObject child)
			{
				children.Add(child);
			}

			public int GetOrbitCount(int depth = 0)
			{
				var count = 0;
				for (int i = 0; i < children.Count; i++)
				{
					count += children[i].GetOrbitCount(depth + 1);
				}
				return depth + count;
			}

			public bool FindPathTo(string name, List<CelestialObject> path)
			{
				if (name == Name)
					return true;
				for (int i = 0; i < ChildCount; i++)
				{
					if (children[i].FindPathTo(name, path))
					{
						path.Add(children[i]);
						return true;
					}
				}
				return false;
			}

			public int GetDepth(string name, int depth = 0)
			{
				if (name == Name)
					return depth;
				var d = 0;
				for (int i = 0; i < ChildCount; i++)
				{
					d += children[i].GetDepth(name, depth + 1);
				}
				return d;
			}

			public override string ToString()
			{
				return Name;
			}
		}
	}
}