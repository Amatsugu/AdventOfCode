using AdventOfCode.Day_5;
using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day7
{
	[ProblemInfo(2019, 7, "Amplification Circuit")]
	public class AmplificationCircuit : Problem<int, int>
	{
		private int[] _code = Array.Empty<int>();

		public static int RunPhase(IntCodeV2 cpu, int[] code, int[] phaseSettings)
		{
			if (HasDuplicateValues(phaseSettings))
				return int.MinValue;
			int[] outputBuffer = { 0 };
			int[] inputBuffer;
			//Amp A
			inputBuffer = new int[] { phaseSettings[0], outputBuffer[0] };
			cpu.ExecuteCode(code, inputBuffer, outputBuffer);
			//Amp B
			inputBuffer = new int[] { phaseSettings[1], outputBuffer[0] };
			cpu.ExecuteCode(code, inputBuffer, outputBuffer);
			//Amp C
			inputBuffer = new int[] { phaseSettings[2], outputBuffer[0] };
			cpu.ExecuteCode(code, inputBuffer, outputBuffer);
			//Amp D
			inputBuffer = new int[] { phaseSettings[3], outputBuffer[0] };
			cpu.ExecuteCode(code, inputBuffer, outputBuffer);
			//Amp E
			inputBuffer = new int[] { phaseSettings[4], outputBuffer[0] };
			cpu.ExecuteCode(code, inputBuffer, outputBuffer);
			return outputBuffer[0];
		}

		public static int RunFeedback(int[] code, int[] phaseSettings)
		{
			if (HasDuplicateValues(phaseSettings))
				return int.MinValue;
			var ampA = new IntCodeV2(true, true).LoadCode(code);
			var ampB = new IntCodeV2(true, true).LoadCode(code);
			var ampC = new IntCodeV2(true, true).LoadCode(code);
			var ampD = new IntCodeV2(true, true).LoadCode(code);
			var ampE = new IntCodeV2(true, true).LoadCode(code);
			var outputA = new int[] { 273 };
			var outputB = new int[] { 0 };
			var outputC = new int[] { 0 };
			var outputD = new int[] { 0 };
			var outputE = new int[] { 0 };
			var inputA = new int[] { phaseSettings[0], outputE[0] };
			var inputB = new int[] { phaseSettings[1], outputA[0] };
			var inputC = new int[] { phaseSettings[2], outputB[0] };
			var inputD = new int[] { phaseSettings[3], outputC[0] };
			var inputE = new int[] { phaseSettings[4], outputD[0] };
			ampA.SetIO(inputA, outputA);
			ampB.SetIO(inputB, outputB);
			ampC.SetIO(inputC, outputC);
			ampD.SetIO(inputD, outputD);
			ampE.SetIO(inputE, outputE);
			int iter = 0;
			while (!ampE.IsHalted)
			{
				//Console.WriteLine($"Iteration {iter}");
				inputA[1] = outputE[0];

				ampA.Run();
				inputB[1] = outputA[0];
				ampB.Run();
				inputC[1] = outputB[0];
				ampC.Run();
				inputD[1] = outputC[0];
				ampD.Run();
				inputE[1] = outputD[0];
				ampE.Run();

				//Console.WriteLine($"Output {outputE[0]}");
				iter++;
			}

			return outputE[0];
		}

		public static bool HasDuplicateValues(int[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				for (int j = 0; j < arr.Length; j++)
				{
					if (i == j)
						continue;
					if (arr[i] == arr[j])
						return true;
				}
			}
			return false;
		}

		public override void LoadInput()
		{
			_code = InputParsing.ParseIntCsv(GetInputFile("input.csv"));
		}

		public override void CalculatePart1()
		{
			int output = int.MinValue;
			int min = 0;
			int max = 5;
			var cpu = new IntCodeV2();

			for (int i = min; i < max; i++)
			{
				for (int j = min; j < max; j++)
				{
					for (int k = min; k < max; k++)
					{
						for (int l = min; l < max; l++)
						{
							for (int m = min; m < max; m++)
							{
								var result = RunPhase(cpu, _code, new int[] { i, j, k, l, m });
								if (output < result)
								{
									output = result;
								}
							}
						}
					}
				}
			}
			Part1 = output;
		}

		public override void CalculatePart2()
		{
			int output = int.MinValue;
			int min = 5;
			int max = 10;

			for (int i = min; i < max; i++)
			{
				for (int j = min; j < max; j++)
				{
					for (int k = min; k < max; k++)
					{
						for (int l = min; l < max; l++)
						{
							for (int m = min; m < max; m++)
							{
								var result = RunFeedback(_code, new int[] { i, j, k, l, m });
								if (output < result)
								{
									output = result;
								}
							}
						}
					}
				}
			}
			Part2 = output;
		}
	}
}