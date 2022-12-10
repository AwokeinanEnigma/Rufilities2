// Decompiled with JetBrains decompiler

using System.Collections.Generic;

namespace Rufilities.Strings
{
    internal class StringNode
    {
        private const char SEPARATOR = '.';
        private readonly string name;
        private readonly bool isContainer;
        private readonly StringNode parent;
        private readonly List<StringNode> children;

        public string Name => name;

        public List<StringNode> Children => children;

        public bool IsLeaf => children.Count == 0;

        public bool IsContainer => isContainer;

        public StringNode(StringNode parent, string name, bool isContainer)
        {
            this.name = name;
            this.parent = parent;
            children = new List<StringNode>();
            this.isContainer = isContainer;
        }

        public string[] BuildNameList()
        {
            return BuildQualifiedName().Split('.');
        }

        public string BuildQualifiedName()
        {
            string str = string.Empty;
            if (parent != null)
            {
                str = str + parent.BuildQualifiedName() + '.';
            }

            return str + name;
        }
    }
}
