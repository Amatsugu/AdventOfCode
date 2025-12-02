namespace AdventOfCode
{
	public static class InputParsing
	{
		public static int[] ParseIntArray(string file)
		{
			return File.ReadAllLines(file).Select(s => int.Parse(s)).ToArray();
		}

		public static int[] ParseIntCsv(string file)
		{
			return File.ReadAllText(file).Split(',').Select(s => int.Parse(s)).ToArray();
		}

		public static int ToInt(this int[] intArr)
		{
			int value = 0;
			for (int i = 0; i < intArr.Length; i++)
			{
				value += (int)Math.Pow(10, intArr.Length - i - 1) * intArr[i];
			}
			return value;
		}

		public static int[] ToIntArray(this int number)
		{
			int[] intArr = number.ToString().Select(d => int.Parse(d.ToString())).ToArray();
			return intArr;
		}
	}
}