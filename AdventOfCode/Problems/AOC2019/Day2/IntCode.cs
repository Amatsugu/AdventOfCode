using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day2
{
	[ProblemInfo(2019, 2, "Program Alarm")]
	public class IntCode : Problem<int, int>
	{
		private int[] _inputPart1 = Array.Empty<int>();
		private int[] _inputPart2 = Array.Empty<int>();

		public static int ExecuteCode(int[] code, int noun, int verb)
		{
			int[] memory = code;
			memory[1] = noun;
			memory[2] = verb;
			var curAddr = 0;

			while (true)
			{
				var opCode = memory[curAddr];

				if (opCode == 99) //Halt
					return memory[0];

				//Working Adresses
				int a = memory[curAddr + 1], b = memory[curAddr + 2], c = memory[curAddr + 3];

				if (a > memory.Length || b > memory.Length || c > memory.Length)
				{
					Console.WriteLine("ERROR: Out of Bounds");
					return 0;
				}

				if (opCode == 1) //Add
					memory[c] = memory[a] + memory[b];
				if (opCode == 2) //Multiply
					memory[c] = memory[a] * memory[b];

				curAddr += 4;
			}
		}

		public override void LoadInput()
		{
			_inputPart1 = InputParsing.ParseIntCsv(GetInputFile("input.csv"));
			_inputPart2 = InputParsing.ParseIntCsv(GetInputFile("input.csv"));
		}

		public override void CalculatePart1()
		{
			Part1 = ExecuteCode(_inputPart1, 12, 2);
		}

		public override void CalculatePart2()
		{
			int targetOutput = 19690720;
			for (int n = 0; n < 100; n++)
			{
				for (int v = 0; v < 100; v++)
				{
					var curInput = new int[_inputPart2.Length];
					Array.Copy(_inputPart2, curInput, _inputPart2.Length);
					if (ExecuteCode(curInput, n, v) == targetOutput)
					{
						Part2 = 100 * n + v;
						return;
					}
				}
			}
		}
	}
}
