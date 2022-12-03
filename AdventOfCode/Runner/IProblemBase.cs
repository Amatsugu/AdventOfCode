using AdventOfCode.Runner.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Runner;
public abstract class Problem
{
	public abstract void LoadInput();
	public abstract void CalculatePart1();
	public abstract void PrintPart1();
	public abstract void CalculatePart2();
	public abstract void PrintPart2();

	protected string GetInputFile(string filename)
	{
		var info = this.GetType().GetCustomAttribute<ProblemInfoAttribute>();
		if (info == null)
			return filename;
		
		return Path.Combine($"Problems/AOC{info.Year}/Day{info.Day}", filename);
	}
}