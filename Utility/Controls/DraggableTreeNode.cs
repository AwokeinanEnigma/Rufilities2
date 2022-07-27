// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Controls.DraggableTreeNode
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System.Windows.Forms;

namespace Rufilities.Utility.Controls
{
  public class DraggableTreeNode : TreeNode
  {
    private bool allowDrop;

    public bool IsContainer
    {
      get => this.allowDrop;
      set => this.allowDrop = value;
    }

    public DraggableTreeNode()
    {
    }

    public DraggableTreeNode(string text)
      : base(text)
    {
    }

    public DraggableTreeNode(string text, TreeNode[] children)
      : base(text, children)
    {
    }

    public DraggableTreeNode(string text, int imageIndex, int selectedImageIndex)
      : base(text, imageIndex, selectedImageIndex)
    {
    }

    public DraggableTreeNode(
      string text,
      int imageIndex,
      int selectedImageIndex,
      TreeNode[] children)
      : base(text, imageIndex, selectedImageIndex, children)
    {
    }
  }
}
