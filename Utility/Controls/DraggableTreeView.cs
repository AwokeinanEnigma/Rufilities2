// Decompiled with JetBrains decompiler

using System;
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
        private readonly ImageList imageListDrag;
        private readonly Timer timer;
        private int dragThreshold;

        public int DragThreshold
        {
            get => dragThreshold;
            set => dragThreshold = value;
        }

        public string LastDragPath => lastDragPath;

        public string LastDropPath => lastDropPath;

        public DraggableTreeView()
        {
            imageListDrag = new ImageList();
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            TreeViewNodeSorter = new DraggableTreeView.ContainerComparer();
        }

        private TreeNode GetNodeByFullPathStep(string[] nameParts, int index, TreeNode node)
        {
            TreeNode nodeByFullPathStep = null;
            TreeNode[] treeNodeArray = node.Nodes.Find(nameParts[index], false);
            if (treeNodeArray.Length == 1)
            {
                nodeByFullPathStep = nameParts.Length <= index + 1 ? treeNodeArray[0] : GetNodeByFullPathStep(nameParts, index + 1, treeNodeArray[0]);
            }

            return nodeByFullPathStep;
        }

        public TreeNode GetNodeByFullPath(string fullPath)
        {
            TreeNode nodeByFullPath = null;
            string[] nameParts = fullPath.Split(PathSeparator[0]);
            TreeNode[] treeNodeArray = Nodes.Find(nameParts[0], false);
            if (treeNodeArray.Length == 1)
            {
                nodeByFullPath = nameParts.Length <= 1 ? treeNodeArray[0] : GetNodeByFullPathStep(nameParts, 1, treeNodeArray[0]);
            }

            return nodeByFullPath;
        }

        public bool AddNodeByContainingPath(string containingPath, TreeNode node)
        {
            bool flag = false;
            string[] strArray = containingPath.Split(new string[1]
            {
        PathSeparator
            }, StringSplitOptions.None);
            Stack<string> stringStack = new Stack<string>();
            for (int index = strArray.Length - 1; index >= 0; --index)
            {
                stringStack.Push(strArray[index]);
            }

            TreeNodeCollection nodes = Nodes;
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
                    DraggableTreeNode node1 = new DraggableTreeNode(str)
                    {
                        IsContainer = true
                    };
                    nodes.Add(node1);
                    treeNode = node1;
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
            if (HitTest(e.X, e.Y).Node != null)
            {
                return;
            }

            OnBeforeSelect(new TreeViewCancelEventArgs(SelectedNode, false, TreeViewAction.ByMouse));
            SelectedNode = null;
            OnAfterSelect(new TreeViewEventArgs(SelectedNode, TreeViewAction.ByMouse));
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            if (gfbevent.Effect == DragDropEffects.Move)
            {
                gfbevent.UseDefaultCursors = false;
                Cursor = Cursors.Default;
            }
            else
            {
                gfbevent.UseDefaultCursors = true;
            }

            base.OnGiveFeedback(gfbevent);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            dragNode = (TreeNode)e.Item;
            SelectedNode = dragNode;
            imageListDrag.Images.Clear();
            imageListDrag.ImageSize = new Size(dragNode.Bounds.Size.Width + Indent, dragNode.Bounds.Height);
            Bitmap data = new Bitmap(dragNode.Bounds.Width + Indent, dragNode.Bounds.Height);
            using (Graphics graphics = Graphics.FromImage(data))
            {
                if (ImageList != null)
                {
                    graphics.DrawImage(ImageList.Images[dragNode.ImageIndex], 0, 0);
                }

                graphics.DrawString(dragNode.Text, Font, new SolidBrush(ForeColor), Indent, 1f);
            }
            imageListDrag.Images.Add(data);
            Point client = PointToClient(Control.MousePosition);
            if (DragHelper.ImageList_BeginDrag(imageListDrag.Handle, 0, client.X + Indent - dragNode.Bounds.Left, client.Y - dragNode.Bounds.Top))
            {
                int num = (int)DoDragDrop(data, DragDropEffects.Move);
                DragHelper.ImageList_EndDrag();
            }
            base.OnItemDrag(e);
        }

        private bool NodeAllowsDrops(TreeNode dropNode)
        {
            bool flag = false;
            if (dropNode is DraggableTreeNode)
            {
                flag = ((DraggableTreeNode)dropNode).IsContainer;
            }

            return flag;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            DragHelper.ImageList_DragLeave(Handle);
            TreeNode nodeAt = GetNodeAt(PointToClient(new Point(drgevent.X, drgevent.Y)));
            if (dragNode == nodeAt || !NodeAllowsDrops(nodeAt))
            {
                return;
            }

            lastDragPath = dragNode.FullPath;
            if (dragNode.Parent == null)
            {
                Nodes.Remove(dragNode);
            }
            else
            {
                if (dragNode.Parent.IsExpanded && dragNode.Parent.Nodes.Count == 1)
                {
                    dragNode.Parent.ImageIndex = 0;
                    dragNode.Parent.SelectedImageIndex = dragNode.Parent.ImageIndex;
                }
                dragNode.Parent.Nodes.Remove(dragNode);
            }
            nodeAt.Nodes.Add(dragNode);
            nodeAt.ExpandAll();
            timer.Enabled = false;
            lastDropPath = nodeAt.FullPath;
            dragNode = null;
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            Point client = PointToClient(new Point(drgevent.X, drgevent.Y));
            DragHelper.ImageList_DragMove(client.X - Left, client.Y - Top);
            TreeNode nodeAt = GetNodeAt(PointToClient(new Point(drgevent.X, drgevent.Y)));
            if (nodeAt == null || !NodeAllowsDrops(nodeAt))
            {
                drgevent.Effect = DragDropEffects.None;
            }
            else
            {
                drgevent.Effect = DragDropEffects.Move;
                if (tempDropNode != nodeAt)
                {
                    DragHelper.ImageList_DragShowNolock(false);
                    SelectedNode = nodeAt;
                    DragHelper.ImageList_DragShowNolock(true);
                    tempDropNode = nodeAt;
                }
                for (TreeNode treeNode = nodeAt; treeNode.Parent != null; treeNode = treeNode.Parent)
                {
                    if (treeNode.Parent == dragNode)
                    {
                        drgevent.Effect = DragDropEffects.None;
                    }
                }
                base.OnDragOver(drgevent);
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            DragHelper.ImageList_DragEnter(Handle, drgevent.X - Left, drgevent.Y - Top);
            timer.Enabled = true;
            base.OnDragEnter(drgevent);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            DragHelper.ImageList_DragLeave(Handle);
            timer.Enabled = false;
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
            Point client = PointToClient(Control.MousePosition);
            TreeNode nodeAt = GetNodeAt(client);
            if (nodeAt == null)
            {
                return;
            }

            if (client.Y < dragThreshold)
            {
                if (nodeAt.PrevVisibleNode == null)
                {
                    return;
                }

                TreeNode prevVisibleNode = nodeAt.PrevVisibleNode;
                DragHelper.ImageList_DragShowNolock(false);
                prevVisibleNode.EnsureVisible();
                Refresh();
                DragHelper.ImageList_DragShowNolock(true);
            }
            else
            {
                if (client.Y <= Size.Height - dragThreshold || nodeAt.NextVisibleNode == null)
                {
                    return;
                }

                TreeNode nextVisibleNode = nodeAt.NextVisibleNode;
                DragHelper.ImageList_DragShowNolock(false);
                nextVisibleNode.EnsureVisible();
                Refresh();
                DragHelper.ImageList_DragShowNolock(true);
            }
        }

        private class ContainerComparer : Comparer<TreeNode>
        {
            public override int Compare(TreeNode x, TreeNode y)
            {
                if (x is DraggableTreeNode && y is DraggableTreeNode)
                {
                    if (((DraggableTreeNode)x).IsContainer && !((DraggableTreeNode)y).IsContainer)
                    {
                        return -1;
                    }

                    if (!((DraggableTreeNode)x).IsContainer && ((DraggableTreeNode)y).IsContainer)
                    {
                        return 1;
                    }
                }
                return string.Compare(x.Name, y.Name);
            }
        }
    }
}
