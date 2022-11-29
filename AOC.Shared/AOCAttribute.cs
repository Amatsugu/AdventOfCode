using System;
using System.Collections.Generic;
using System.Text;

namespace AOC.Shared
{
	public class ProblemInfoAttribute : Attribute
	{
		public int Day { get; init; }
		public string Year { get; init; }
		public string Name { get; init; }
		public ProblemInfoAttribute(string year, int day, string name) {
			Year = year;
			Day = day;
			Name = name;
		}
	}
}
