// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Controls.DraggableTreeView
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Rufilities.Utility.Controls
{
  public class DraggableTreeView : TreeView
  {
    private TreeNode dragNode;
    private TreeNode tempDropNode;
    private string lastDragPath;
    private string lastDropPath;
    private ImageList imageListDrag;
    private Timer timer;
    private int dragThreshold;

    public int DragThreshold
    {
      get => this.dragThreshold;
      set => this.dragThreshold = value;
    }

    public string LastDragPath => this.lastDragPath;

    public string LastDropPath => this.lastDropPath;

    public DraggableTreeView()
    {
      this.imageListDrag = new ImageList();
      this.timer = new Timer();
      this.timer.Tick += new EventHandler(this.timer_Tick);
      this.TreeViewNodeSorter = (IComparer) new DraggableTreeView.ContainerComparer();
    }

    private TreeNode GetNodeByFullPathStep(string[] nameParts, int index, TreeNode node)
    {
      TreeNode nodeByFullPathStep = (TreeNode) null;
      TreeNode[] treeNodeArray = node.Nodes.Find(nameParts[index], false);
      if (treeNodeArray.Length == 1)
        nodeByFullPathStep = nameParts.Length <= index + 1 ? treeNodeArray[0] : this.GetNodeByFullPathStep(nameParts, index + 1, treeNodeArray[0]);
      return nodeByFullPathStep;
    }

    public TreeNode GetNodeByFullPath(string fullPath)
    {
      TreeNode nodeByFullPath = (TreeNode) null;
      string[] nameParts = fullPath.Split(this.PathSeparator[0]);
      TreeNode[] treeNodeArray = this.Nodes.Find(nameParts[0], false);
      if (treeNodeArray.Length == 1)
        nodeByFullPath = nameParts.Length <= 1 ? treeNodeArray[0] : this.GetNodeByFullPathStep(nameParts, 1, treeNodeArray[0]);
      return nodeByFullPath;
    }

    public bool AddNodeByContainingPath(string containingPath, TreeNode node)
    {
      bool flag = false;
      string[] strArray = containingPath.Split(new string[1]
      {
        this.PathSeparator
      }, StringSplitOptions.None);
      Stack<string> stringStack = new Stack<string>();
      for (int index = strArray.Length - 1; index >= 0; --index)
        stringStack.Push(strArray[index]);
      TreeNodeCollection nodes = this.Nodes;
      TreeNode treeNode;
      do
      {
        string str = stringStack.Pop();
        treeNode = nodes[str];
        if (treeNode != null)
        {
          nodes = treeNode.Nodes;
        }
        else
        {
          DraggableTreeNode node1 = new DraggableTreeNode(str);
          node1.IsContainer = true;
          nodes.Add((TreeNode) node1);
          treeNode = (TreeNode) node1;
          nodes = node1.Nodes;
        }
      }
      while (stringStack.Count > 0);
      if (stringStack.Count == 0)
      {
        treeNode.Nodes.Add(node);
        flag = true;
      }
      return flag;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if (this.HitTest(e.X, e.Y).Node != null)
        return;
      this.OnBeforeSelect(new TreeViewCancelEventArgs(this.SelectedNode, false, TreeViewAction.ByMouse));
      this.SelectedNode = (TreeNode) null;
      this.OnAfterSelect(new TreeViewEventArgs(this.SelectedNode, TreeViewAction.ByMouse));
    }

    protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
    {
      if (gfbevent.Effect == DragDropEffects.Move)
      {
        gfbevent.UseDefaultCursors = false;
        this.Cursor = Cursors.Default;
      }
      else
        gfbevent.UseDefaultCursors = true;
      base.OnGiveFeedback(gfbevent);
    }

    protected override void OnItemDrag(ItemDragEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.dragNode = (TreeNode) e.Item;
      this.SelectedNode = this.dragNode;
      this.imageListDrag.Images.Clear();
      this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.Indent, this.dragNode.Bounds.Height);
      Bitmap data = new Bitmap(this.dragNode.Bounds.Width + this.Indent, this.dragNode.Bounds.Height);
      using (Graphics graphics = Graphics.FromImage((Image) data))
      {
        if (this.ImageList != null)
          graphics.DrawImage(this.ImageList.Images[this.dragNode.ImageIndex], 0, 0);
        graphics.DrawString(this.dragNode.Text, this.Font, (Brush) new SolidBrush(this.ForeColor), (float) this.Indent, 1f);
      }
      this.imageListDrag.Images.Add((Image) data);
      Point client = this.PointToClient(Control.MousePosition);
      if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, client.X + this.Indent - this.dragNode.Bounds.Left, client.Y - this.dragNode.Bounds.Top))
      {
        int num = (int) this.DoDragDrop((object) data, DragDropEffects.Move);
        DragHelper.ImageList_EndDrag();
      }
      base.OnItemDrag(e);
    }

    private bool NodeAllowsDrops(TreeNode dropNode)
    {
      bool flag = false;
      if (dropNode is DraggableTreeNode)
        flag = ((DraggableTreeNode) dropNode).IsContainer;
      return flag;
    }

    protected override void OnDragDrop(DragEventArgs drgevent)
    {
      DragHelper.ImageList_DragLeave(this.Handle);
      TreeNode nodeAt = this.GetNodeAt(this.PointToClient(new Point(drgevent.X, drgevent.Y)));
      if (this.dragNode == nodeAt || !this.NodeAllowsDrops(nodeAt))
        return;
      this.lastDragPath = this.dragNode.FullPath;
      if (this.dragNode.Parent == null)
      {
        this.Nodes.Remove(this.dragNode);
      }
      else
      {
        if (this.dragNode.Parent.IsExpanded && this.dragNode.Parent.Nodes.Count == 1)
        {
          this.dragNode.Parent.ImageIndex = 0;
          this.dragNode.Parent.SelectedImageIndex = this.dragNode.Parent.ImageIndex;
        }
        this.dragNode.Parent.Nodes.Remove(this.dragNode);
      }
      nodeAt.Nodes.Add(this.dragNode);
      nodeAt.ExpandAll();
      this.timer.Enabled = false;
      this.lastDropPath = nodeAt.FullPath;
      this.dragNode = (TreeNode) null;
      base.OnDragDrop(drgevent);
    }

    protected override void OnDragOver(DragEventArgs drgevent)
    {
      Point client = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      DragHelper.ImageList_DragMove(client.X - this.Left, client.Y - this.Top);
      TreeNode nodeAt = this.GetNodeAt(this.PointToClient(new Point(drgevent.X, drgevent.Y)));
      if (nodeAt == null || !this.NodeAllowsDrops(nodeAt))
      {
        drgevent.Effect = DragDropEffects.None;
      }
      else
      {
        drgevent.Effect = DragDropEffects.Move;
        if (this.tempDropNode != nodeAt)
        {
          DragHelper.ImageList_DragShowNolock(false);
          this.SelectedNode = nodeAt;
          DragHelper.ImageList_DragShowNolock(true);
          this.tempDropNode = nodeAt;
        }
        for (TreeNode treeNode = nodeAt; treeNode.Parent != null; treeNode = treeNode.Parent)
        {
          if (treeNode.Parent == this.dragNode)
            drgevent.Effect = DragDropEffects.None;
        }
        base.OnDragOver(drgevent);
      }
    }

    protected override void OnDragEnter(DragEventArgs drgevent)
    {
      DragHelper.ImageList_DragEnter(this.Handle, drgevent.X - this.Left, drgevent.Y - this.Top);
      this.timer.Enabled = true;
      base.OnDragEnter(drgevent);
    }

    protected override void OnDragLeave(EventArgs e)
    {
      DragHelper.ImageList_DragLeave(this.Handle);
      this.timer.Enabled = false;
      base.OnDragLeave(e);
    }

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
    {
      e.Node.ImageIndex = 1;
      e.Node.SelectedImageIndex = e.Node.ImageIndex;
      base.OnBeforeExpand(e);
    }

    protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
    {
      e.Node.ImageIndex = 0;
      e.Node.SelectedImageIndex = e.Node.ImageIndex;
      base.OnBeforeCollapse(e);
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      Point client = this.PointToClient(Control.MousePosition);
      TreeNode nodeAt = this.GetNodeAt(client);
      if (nodeAt == null)
        return;
      if (client.Y < this.dragThreshold)
      {
        if (nodeAt.PrevVisibleNode == null)
          return;
        TreeNode prevVisibleNode = nodeAt.PrevVisibleNode;
        DragHelper.ImageList_DragShowNolock(false);
        prevVisibleNode.EnsureVisible();
        this.Refresh();
        DragHelper.ImageList_DragShowNolock(true);
      }
      else
      {
        if (client.Y <= this.Size.Height - this.dragThreshold || nodeAt.NextVisibleNode == null)
          return;
        TreeNode nextVisibleNode = nodeAt.NextVisibleNode;
        DragHelper.ImageList_DragShowNolock(false);
        nextVisibleNode.EnsureVisible();
        this.Refresh();
        DragHelper.ImageList_DragShowNolock(true);
      }
    }

    private class ContainerComparer : Comparer<TreeNode>
    {
      public override int Compare(TreeNode x, TreeNode y)
      {
        if (x is DraggableTreeNode && y is DraggableTreeNode)
        {
          if (((DraggableTreeNode) x).IsContainer && !((DraggableTreeNode) y).IsContainer)
            return -1;
          if (!((DraggableTreeNode) x).IsContainer && ((DraggableTreeNode) y).IsContainer)
            return 1;
        }
        return string.Compare(x.Name, y.Name);
      }
    }
  }
}
