using System.Numerics;

namespace AdventOfCode.Problems.AOC2022.Day11;

internal class Monkey
{
	public int MonkeyNumber { get; set; }
	public int InspectionCount { get; private set; }
	public List<BigInteger> Items { get; set; }

	private MonkeyOperator _operator;
	private uint _operand;
	private uint _divisor;
	private int _trueTarget;
	private int _falseTarget;

	public Monkey(string[] lines)
	{
		Items = new List<BigInteger>();
		MonkeyNumber = int.Parse(lines[0].Split(' ')[^1][0..^1]);
		Items = lines[1].Split(": ")[^1].Split(", ").Select(v => BigInteger.Parse(v)).ToList();
		var operation = lines[2].Split("= old ")[^1];
		_operator = operation[0] switch
		{
			'*' => MonkeyOperator.Multiply,
			'+' => MonkeyOperator.Add,
			_ => MonkeyOperator.Add
		};
		if (!uint.TryParse(operation[1..], out _operand))
			_operator = MonkeyOperator.Power;

		_divisor = uint.Parse(lines[3].Split(' ')[^1]);
		_trueTarget = int.Parse(lines[4].Split(' ')[^1]);
		_falseTarget = int.Parse(lines[5].Split(' ')[^1]);
	}

	public BigInteger Inspect(BigInteger value, uint worryOffset = 3)
	{
		InspectionCount++;
		value = Operate(value);
		if (worryOffset != 0)
			value /= worryOffset;
		return value;
	}

	public int GetThrowTarget(BigInteger value)
	{
		return value % _divisor == 0 ? _trueTarget : _falseTarget;
	}

	private BigInteger Operate(BigInteger value)
	{
		return _operator switch
		{
			MonkeyOperator.Multiply => value * _operand,
			MonkeyOperator.Add => value + _operand,
			MonkeyOperator.Power => value * value,
			_ => value
		};
	}

	private enum MonkeyOperator
	{
		Multiply,
		Add,
		Power
	}

	public override string ToString()
	{
		return $"{MonkeyNumber}: ({InspectionCount}) [{string.Join(", ", Items)}]";
	}
}