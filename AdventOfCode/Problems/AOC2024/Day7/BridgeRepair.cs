﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2024.Day7;

[ProblemInfo(2024, 7, "Bridge Repair")]
internal class BridgeRepair : Problem<ulong, ulong>
{
	private List<(ulong total, ulong[] nums)> _data = [];

	private enum Operator
	{
		Mul,
		Add,
		Concat
	}

	public override void CalculatePart1()
	{
		foreach (var (target, nums) in _data)
		{
			if (IsSolvable(target, nums, [Operator.Mul, Operator.Add]))
				Part1 += target;
		}
	}

	private bool IsSolvable(ulong target, ulong[] nums, Operator[] ops)
	{
		return ops.Any(o => IsSolvable(target, nums, o, nums[0], ops));
	}

	private bool IsSolvable(ulong target, ulong[] nums, Operator curOperator, ulong curTotal, Operator[] ops, int idx = 1)
	{
		if (target == curTotal)
			return true;
		if (idx >= nums.Length)
			return false;

		curTotal = curOperator switch
		{
			Operator.Mul => curTotal * nums[idx++],
			Operator.Add => curTotal + nums[idx++],
			Operator.Concat => ulong.Parse($"{curTotal}{nums[idx++]}"),
			_ => curTotal,
		};

		return ops.Any(o => IsSolvable(target, nums, o, curTotal, ops, idx));
	}

	public override void CalculatePart2()
	{
		foreach (var (target, nums) in _data)
		{
			if (IsSolvable(target, nums, [Operator.Mul, Operator.Add, Operator.Concat]))
				Part2 += target;
		}
	}

	public override void LoadInput()
	{
		var lines = ReadInputLines("input.txt");
		_data = new List<(ulong total, ulong[] nums)>(lines.Length);
		foreach (var line in lines)
		{
			var s = line.Split(':');
			var sum = ulong.Parse(s[0].Trim());
			var nums = s[1].Trim().Split(' ').Select(ulong.Parse).ToArray();
			_data.Add((sum, nums));
		}
	}
}