using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2022.Day10;

[ProblemInfo(2022, 10, "Cathode-Ray Tube")]
internal class CathodeRayTube : Problem<int, string>
{
	private (CathodeCPU.Instruction ins, int value)[] _code = Array.Empty<(CathodeCPU.Instruction ins, int value)>();

	public override void CalculatePart1()
	{
		var cpu = new CathodeCPU();
		var result = cpu.ExecuteCode(_code, new[] { 20, 60, 100, 140, 180, 220 });
		Part1 = result.Sum();
	}

	public override void CalculatePart2()
	{
		var output = Enumerable.Repeat(' ', 6 * 40).ToArray();

		var cpu = new CathodeCPU();
		cpu.ExecuteCode(_code, (cycle, signal) =>
		{
			cycle -= 1;
			if (cycle > output.Length)
				return;

			var pos = signal % 40;
			var head = (cycle % 40);
			var sprite = Math.Abs(pos - head);
			if (sprite <= 1)
				output[cycle] = '█';
		});

		var lines = output.Chunk(40).Select(r => new string(r));
		Part2 = $"\n{string.Join("\n", lines)}";
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_code = new (CathodeCPU.Instruction ins, int value)[lines.Length];
		for (int i = 0; i < lines.Length; i++)
		{
			var ln = lines[i];
			if (ln == "noop")
				_code[i] = (CathodeCPU.Instruction.NoOp, 0);
			else
			{
				var instruction = ln.Split(' ');
				_code[i] = (Enum.Parse<CathodeCPU.Instruction>(instruction[0], true), int.Parse(instruction[1]));
			}
		}
	}
}