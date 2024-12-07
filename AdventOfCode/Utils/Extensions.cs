using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils;
public static class Extensions
{
	public static string AsJoinedString(this object[] data, string delim = ", ")
	{
		return string.Join(delim, data);
	}
}
