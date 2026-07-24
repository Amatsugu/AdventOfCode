using Google.OrTools.Sat;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AdventOfCode.Problems.AOC2025.Day10;

internal class JoltageILP
{
	private readonly int[] _target;
	private readonly int[][] _ops;

	public int Columns => _ops.Length;
	public int Rows => _target.Length;

	public JoltageILP(int[] target, int[][] ops)
	{
		_target = target;
		_ops = ops;
	}

	public int[,] GetButtonsMatrix()
	{
		var mat = new int[Rows, Columns];
		for (int i = 0; i < _target.Length; i++)
		{
			for (int j = 0; j < _ops.Length; j++)
			{
				if (_ops[j].Contains(i))
					mat[i, j] = 1;
			}
		}
		return mat;
	}

	public int Solve()
	{
		var a = GetButtonsMatrix();
		var b = _target;
		var model = new CpModel();

		var x = new IntVar[Columns];
		for (int j = 0; j < Columns; j++)
		{
			x[j] = model.NewIntVar(0, _target.Max(), $"x_{j}");
		}

		for (int i = 0; i < Rows; i++)
		{
			var vars = new List<IntVar>();
			for (int j = 0; j < Columns; j++)
			{
				if (a[i, j] == 1)
					vars.Add(x[j]);
			}
			model.Add(LinearExpr.Sum(vars) == b[i]);
		}

		model.Minimize(LinearExpr.Sum(x));

		var solver = new CpSolver();
		var result = solver.Solve(model);

		if (result == CpSolverStatus.Optimal)
		{
			var sum = 0;
			for (int i = 0; i < Columns; i++)
			{
				var v = (int)solver.Value(x[i]);
				sum += v;
			}
			return sum;
		}
		return -1;
	}
}