using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Runner;
public interface IProblem
{
	void LoadInput();

	void CalculatePart1();

	void PrintPart1();

	void CalculatePart2();
	void PrintPart2();
}