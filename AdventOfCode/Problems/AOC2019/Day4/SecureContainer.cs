using AdventOfCode.Runner.Attributes;

namespace AdventOfCode.Problems.AOC2019.Day4
{
	[ProblemInfo(2019, 4, "Secure Container")]
	public class SecureContainer : Problem<int, int>
	{
		public static bool IsValidPassword(int[] password)
		{
			if (password.Length != 6)
				return false;
			return HasRepeating(password) && IsAssending(password);
		}

		public static bool HasRepeating(int[] password)
		{
			for (int i = 0; i < password.Length - 1; i++)
			{
				if (password[i] == password[i + 1])
					return true;
			}
			return false;
		}

		public static bool HasDoubles(int[] password)
		{
			bool foundDouble = false;
			for (int i = 0; i < 6; i++)
			{
				int c = 0;
				for (int j = 0; j < 6; j++)
				{
					if (password[j] == password[i])
						c++;
					else
					{
						if (c != 0)
						{
							if (c == 2)
								foundDouble = true;
							c = 0;
						}
					}
				}
				if (c == 2)
					foundDouble = true;
			}
			return foundDouble;
		}

		public static bool IsAssending(int[] password)
		{
			for (int i = 1; i < 6; i++)
			{
				if (password[i] < password[i - 1])
				{
					return false;
				}
			}
			return true;
		}

		public static int CountPasswordsPart1(int lower, int upper)
		{
			int passwordCount = 0;
			int[] curPassword = lower.ToIntArray();
			CleanPassword(ref curPassword);
			while (curPassword.ToInt() <= upper)
			{
				if (IsValidPassword(curPassword))
				{
					passwordCount++;
				}
				curPassword[^1]++;
				Propagate(ref curPassword, curPassword.Length - 1);
				CleanPassword(ref curPassword);
			}
			return passwordCount;
		}

		public static int CountPasswordsPart2(int lower, int upper)
		{
			int passwordCount = 0;
			int[] curPassword = lower.ToIntArray();
			CleanPassword(ref curPassword);
			while (curPassword.ToInt() <= upper)
			{
				if (HasDoubles(curPassword))
				{
					passwordCount++;
				}
				curPassword[^1]++;
				Propagate(ref curPassword, curPassword.Length - 1);
				CleanPassword(ref curPassword);
			}
			return passwordCount;
		}

		public static void CleanPassword(ref int[] password)
		{
			for (int i = 1; i < 6; i++)
			{
				if (password[i] < password[i - 1])
				{
					password[i] += password[i - 1] - password[i];
					if (password[i] == 10)
					{
						Propagate(ref password, i);
						password[i] = password[i - 1];
					}
				}
			}
		}

		public static void Propagate(ref int[] password, int digit)
		{
			for (int i = digit; i >= 0; i--)
			{
				if (i == 0 && password[i] == 10)
				{
					password[i] = 9;
					break;
				}

				if (password[i] == 10)
				{
					password[i] = 0;
					password[i - 1]++;
				}
			}
		}

		public override void LoadInput()
		{
		}

		public override void CalculatePart1()
		{
			Part1 = CountPasswordsPart1(147981, 691423);
		}

		public override void CalculatePart2()
		{
			Part2 = CountPasswordsPart2(147981, 691423);
		}
	}
}