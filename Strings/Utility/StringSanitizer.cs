// Decompiled with JetBrains decompiler

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
            string str1 = string.Empty;
            if (str != null)
            {
                str1 = str.Replace("\n", "\n\r");
            }

            return str1;
        }
    }
}
