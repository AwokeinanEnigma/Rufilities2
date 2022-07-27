// Decompiled with JetBrains decompiler
// Type: Rufilities.Strings.Utility.StringSanitizer
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

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
        str1 = str.Replace("\n", "\n\r");
      return str1;
    }
  }
}
