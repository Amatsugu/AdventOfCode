namespace AdventOfCode.Runner.Attributes;
public class ProblemInfoAttribute : Attribute
{
	public int Day { get; init; }
	public int Year { get; init; }
	public string Name { get; init; }
	public ProblemInfoAttribute(int year, int day, string name)
	{
		Year = year;
		Day = day;
		Name = name;
	}
}