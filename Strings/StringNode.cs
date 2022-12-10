using System;
using System.Collections.Generic;

namespace Rufilities.Strings
{
	internal class StringNode
	{
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public List<StringNode> Children
		{
			get
			{
				return this.children;
			}
		}

		public bool IsLeaf
		{
			get
			{
				return this.children.Count == 0;
			}
		}

		public bool IsContainer
		{
			get
			{
				return this.isContainer;
			}
		}

		public StringNode(StringNode parent, string name, bool isContainer)
		{
			this.name = name;
			this.parent = parent;
			this.children = new List<StringNode>();
			this.isContainer = isContainer;
		}

		public string[] BuildNameList()
		{
			return this.BuildQualifiedName().Split(new char[]
			{
				'.'
			});
		}

		public string BuildQualifiedName()
		{
			string text = string.Empty;
			if (this.parent != null)
			{
				text = text + this.parent.BuildQualifiedName() + '.';
			}
			return text + this.name;
		}

		private const char SEPARATOR = '.';

		private string name;

		private bool isContainer;

		private StringNode parent;

		private List<StringNode> children;
	}
}
