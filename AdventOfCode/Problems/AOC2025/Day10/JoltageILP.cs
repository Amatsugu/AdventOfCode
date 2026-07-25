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

	public int Solve2()
	{
		var a = GetButtonsMatrix();
		var b = _target;

		var limits = new int[Rows];
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Columns; j++)
			{
				if (a[i,j] == 1)
					limits[i] = Math.Max(limits[i], _target[i]);
			}
		}


		var solution = SearchSolution(a, b, new int[Columns], limits);

		return solution.Sum();
	}

	private int[] SearchSolution(int[,] a, int[] b, int[] vars, int[] limits, int curBest = int.MaxValue)
	{
		var solution = vars;
		var solutionSum = curBest;
		for (int i = 0; i < Columns; i++)
		{
			var vars2 = new int[Columns];
			Array.Copy(vars, vars2, Columns);
			vars2[i] += 1;
			if (IsOverLimit(a, vars, limits))
				continue;
			if (vars2.Sum() > curBest)
				continue;
			if(IsSolution(a, b, vars2))
			{
				solution = vars2;
				solutionSum = vars2.Sum();
				continue;
			}
			var s = SearchSolution(a, b, vars2, limits, solutionSum);
			var sum = s.Sum();
			if(sum < solutionSum)
				solution = s;
		}
		return solution;
	}

	private bool IsOverLimit(int[,] a, int[] vars, int[] limits)
	{
		for (int i = 0; i < Rows; i++)
		{
			var sum = 0;
			for (int j = 0; j < Columns; j++)
			{
				sum += a[i, j] * vars[j];
				if (sum > limits[i])
					return true;
			}
		}
		return false;
	}

	private int RowSum(int[,] a, int[] vars, int row)
	{
		var sum = 0;
		for (int j = 0; j < Columns; j++)
		{
			sum += a[row, j] * vars[j];
		}
		return sum;
	}

	private bool IsSolution(int[,] a, int[] b, int[] vars)
	{
		for (int i = 0; i < Rows; i++)
		{
			if (RowSum(a, vars, i) != b[i])
				return false;
		}
		return true;
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