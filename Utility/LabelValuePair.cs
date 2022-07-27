// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.LabelValuePair`1
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

namespace Rufilities.Utility
{
  public class LabelValuePair<T>
  {
    private readonly string label;
    private readonly T value;

    public string Label => this.label;

    public T Value => this.value;

    public LabelValuePair(string label, T value)
    {
      this.label = label;
      this.value = value;
    }

    public LabelValuePair(LabelValuePair<T> labelValuePair)
    {
      this.label = labelValuePair.label;
      this.value = labelValuePair.value;
    }

    public override string ToString() => this.label;
  }
}
