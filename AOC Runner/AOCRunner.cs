using AOC.Runner;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
public class AOCRunner
{
	public AOCRunner() 
	{
		FindProblemClasses();
	}

	private void FindProblemClasses()
	{
		
		var types = Assembly.GetExecutingAssembly()?.DefinedTypes.Where(t => t.IsAssignableTo(typeof(IProblemBase)));
		if (types == null)
			return;
		foreach (var type in types)
		{
			Console.WriteLine(type.Name);
		}
		
	}
}
