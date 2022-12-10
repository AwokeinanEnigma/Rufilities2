using System;

namespace Rufilities.Strings.Utility
{
	internal class StringSanitizer
	{
		public static string Sanitize(string str)
		{
			str.Replace("\r", string.Empty);
			return str;
		}

		public static string RestoreCarriageReturns(string str)
		{
			string result = string.Empty;
			if (str != null)
			{
				result = str.Replace("\n", "\n\r");
			}
			return result;
		}
	}
}
