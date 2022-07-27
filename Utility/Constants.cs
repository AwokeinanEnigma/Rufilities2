// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Constants
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System;

namespace Rufilities.Utility
{
  internal static class Constants
  {

    public static readonly string[] GREEK_CHARACTERS = new string[8]
    {
      "α",
      "β",
      "γ",
      "Ω",
      "Σ",
      "π",
      "λ",
      "××"
    };
    public static readonly int[] POSITIVE_INTEGERS_WITH_ZERO = new int[2]
    {
      0,
      int.MaxValue
    };
    public static readonly int[] NEGATIVE_INTEGERS_WITH_ZERO = new int[2]
    {
      int.MinValue,
      0
    };
    public static readonly int[] POSITIVE_INTEGERS = new int[2]
    {
      1,
      int.MaxValue
    };
    public static readonly int[] NEGATIVE_INTEGERS = new int[2]
    {
      int.MinValue,
      -1
    };
  }
}
