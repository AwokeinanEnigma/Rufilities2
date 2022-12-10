// Decompiled with JetBrains decompiler

using System.Windows.Forms;

namespace Rufilities.Utility.Controls
{
    public class DraggableTreeNode : TreeNode
    {
        private bool allowDrop;

        public bool IsContainer
        {
            get => allowDrop;
            set => allowDrop = value;
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
