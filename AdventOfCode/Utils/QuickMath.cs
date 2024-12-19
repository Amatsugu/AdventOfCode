namespace AdventOfCode.Utils;

public static class QuickMath
{
	private static readonly long[] pow10Long = [1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000, 10000000000, 100000000000, 1000000000000, 10000000000000, 100000000000000, 1000000000000000, 10000000000000000, 100000000000000000, 1000000000000000000];
	private static readonly int[] pow10int = [1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000];

	public static long FastPow10(long exp)
	{
		return pow10Long[exp];
	}

	public static int FastPow10(int exp)
	{
		return pow10int[exp];
	}

	private static readonly long[] longCorrectionTable= [9, 99, 999, 9999, 99999, 999999, 9999999, 99999999, 999999999, 9999999999, 99999999999, 999999999999, 9999999999999, 99999999999999, 999999999999999, 9999999999999999, 99999999999999999, 999999999999999999];
	private static readonly int[] intCorrectionTable = [9, 99, 999, 9999, 99999, 999999, 9999999, 99999999, 999999999];
	public static long DigitCount(this long value)
	{
		var l2 = 63 - long.LeadingZeroCount(value | 1);
		var ans = ((9 * l2) >> 5);
		if (value > longCorrectionTable[ans])
			ans += 1;
		return ans + 1;
	}

	public static int DigitCount(this int value)
	{
		var l2 = 31 - int.LeadingZeroCount(value | 1);
		var ans = ((9 * l2) >> 5);
		if (value > intCorrectionTable[ans])
			ans += 1;
		return ans + 1;
	}
}