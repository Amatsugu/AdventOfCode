namespace AdventOfCode.Problems.AOC2022.Day10;

internal class CathodeCPU
{
	public enum Instruction
	{
		NoOp,
		AddX
	}

	public int X { get; private set; } = 1;
	private int _cycleNumber = 1;
	private int _programCounter;
	private int _pending = -1;

	public int[] ExecuteCode((Instruction ins, int value)[] code, int[] outputCycles, Func<int, int, int>? processor = null)
	{
		var result = new int[outputCycles.Length];

		var ridx = 0;

		if (processor == null)
			processor = (c, x) => c * x;

		ExecuteCode(code, (c, x) =>
		{
			if (ridx < outputCycles.Length && c == outputCycles[ridx])
			{
				result[ridx] = processor(c, x);
				ridx++;
			}
		});

		return result;
	}

	public void ExecuteCode((Instruction ins, int value)[] code, Action<int, int> processor)
	{
		while (_programCounter < code.Length)
		{
			var (ins, value) = code[_programCounter];

			processor(_cycleNumber, X);

			switch ((ins, _pending))
			{
				case { ins: Instruction.NoOp }:
					_programCounter++;
					break;

				case { ins: Instruction.AddX, _pending: -1 }:
					_pending = 1;
					break;

				case { ins: Instruction.AddX, _pending: 0 }:
					X += value;
					_programCounter++;
					break;
			}

			if (_pending >= 0)
				_pending--;
			_cycleNumber++;
		}
	}
}