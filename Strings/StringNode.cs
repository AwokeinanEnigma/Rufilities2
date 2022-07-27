// Decompiled with JetBrains decompiler
// Type: Rufilities.Strings.StringNode
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System.Collections.Generic;

namespace Rufilities.Strings
{
  internal class StringNode
  {
    private const char SEPARATOR = '.';
    private string name;
    private bool isContainer;
    private StringNode parent;
    private List<StringNode> children;

    public string Name => this.name;

    public List<StringNode> Children => this.children;

    public bool IsLeaf => this.children.Count == 0;

    public bool IsContainer => this.isContainer;

    public StringNode(StringNode parent, string name, bool isContainer)
    {
      this.name = name;
      this.parent = parent;
      this.children = new List<StringNode>();
      this.isContainer = isContainer;
    }

    public string[] BuildNameList() => this.BuildQualifiedName().Split('.');

    public string BuildQualifiedName()
    {
      string str = string.Empty;
      if (this.parent != null)
        str = str + this.parent.BuildQualifiedName() + (object) '.';
      return str + this.name;
    }
  }
}
