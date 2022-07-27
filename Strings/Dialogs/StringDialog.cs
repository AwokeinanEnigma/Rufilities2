
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Rufilities.Properties;
using Rufilities.Utility;
using Rufilities.Utility.Controls;
using Rufilities.Utility.Dialogs;

namespace Rufilities.Strings.Dialogs
{
	// Token: 0x0200000E RID: 14
	public class StringDialog : Form
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00005170 File Offset: 0x00003370
		public RufiniString SelectedString
		{
			get
			{
				return this.selectedString;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00005178 File Offset: 0x00003378
		public bool ChangesMade
		{
			get
			{
				return this.madeChanges;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005180 File Offset: 0x00003380
		public StringDialog(StringFile stringFile, bool selectMode, RufiniString initialSelection)
		{
			this.stringFile = stringFile;
			this.selectMode = selectMode;
			this.initialSelection = initialSelection;
			this.InitializeComponent();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000051A3 File Offset: 0x000033A3
		private void MadeChanges()
		{
			this.madeChanges = true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000051AC File Offset: 0x000033AC
		private void InsertStringInString(string insert)
		{
			int selectionStart = this.tbString.SelectionStart;
			this.tbString.Text = this.tbString.Text.Remove(this.tbString.SelectionStart, this.tbString.SelectionLength).Insert(this.tbString.SelectionStart, insert);
			this.tbString.SelectionStart = selectionStart;
			this.tbString.SelectionLength = insert.Length;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005224 File Offset: 0x00003424
		private void PopulateInsertMenu()
		{
			string[] psi_LEVELS = Constants.GREEK_CHARACTERS;
			for (int i = 0; i < psi_LEVELS.Length; i++)
			{
				string s = psi_LEVELS[i];
				this.tsmLetters.DropDownItems.Add(s, null, delegate (object sender, EventArgs e)
				{
					this.InsertStringInString(s);
				});
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005280 File Offset: 0x00003480
		private void PopulateSubTree(TreeNode parentTreeNode, StringNode node)
		{
			int num = node.IsContainer ? 0 : 2;
			DraggableTreeNode draggableTreeNode = new DraggableTreeNode(node.Name, num, num);
			draggableTreeNode.Name = node.Name;
			draggableTreeNode.IsContainer = node.IsContainer;
			if (parentTreeNode != null)
			{
				parentTreeNode.Nodes.Add(draggableTreeNode);
			}
			else
			{
				this.tvStrings.Nodes.Add(draggableTreeNode);
			}
			foreach (StringNode node2 in node.Children)
			{
				this.PopulateSubTree(draggableTreeNode, node2);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000532C File Offset: 0x0000352C
		private void PopulateTreeView()
		{
			TreeNode treeNode = null;
			foreach (StringNode node in this.stringFile.ToNodes())
			{
				this.PopulateSubTree(null, node);
			}
			if (this.initialSelection.Names != null)
			{
				string[] names = this.initialSelection.Names;
				TreeNodeCollection treeNodeCollection = this.tvStrings.Nodes;
				int num = 0;
				do
				{
					if (treeNodeCollection.ContainsKey(names[num]))
					{
						treeNode = treeNodeCollection[names[num]];
					}
					treeNodeCollection = ((treeNode != null) ? treeNode.Nodes : null);
					num++;
				}
				while (treeNodeCollection != null && num < names.Length);
			}
			this.tvStrings.Sort();
			if (treeNode != null)
			{
				this.tvStrings.SelectedNode = treeNode;
				this.tbString.Enabled = true;
				this.bInsert.Enabled = true;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000541C File Offset: 0x0000361C
		private void StringDialog_Load(object sender, EventArgs e)
		{
			if (!this.selectMode)
			{
				this.bSelect.Visible = false;
				this.bCancel.Visible = false;
			}
			else
			{
				this.bCancel.Enabled = true;
			}
			this.PopulateTreeView();
			this.PopulateInsertMenu();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000023C2 File Offset: 0x000005C2
		private void bSelect_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			this.UpdateCurrentString();
			this.stringFile.Save();
			this.madeChanges = false;
			base.Close();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000023D1 File Offset: 0x000005D1
		private void bCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005458 File Offset: 0x00003658
		private void AddFolder(string qualifiedName)
		{
			if (this.tvStrings.GetNodeByFullPath(qualifiedName) == null)
			{
				if (this.stringFile.PutFolder(qualifiedName))
				{
					string[] array = qualifiedName.Split(new char[]
					{
						'.'
					});
					string fullPath = string.Join('.'.ToString(), array, 0, array.Length - 1);
					string text = array[array.Length - 1];
					DraggableTreeNode draggableTreeNode = new DraggableTreeNode(text, 0, 0);
					draggableTreeNode.Name = text;
					draggableTreeNode.IsContainer = true;
					TreeNode nodeByFullPath = this.tvStrings.GetNodeByFullPath(fullPath);
					if (nodeByFullPath != null)
					{
						nodeByFullPath.Nodes.Add(draggableTreeNode);
					}
					else
					{
						this.tvStrings.Nodes.Add(draggableTreeNode);
					}
					this.tvStrings.SelectedNode = draggableTreeNode;
					this.MadeChanges();
					return;
				}
			}
			else
			{
				string text2 = string.Format("The folder \"{0}\" already exists.", qualifiedName);
				MessageBox.Show(this, text2, "Cannot Add Folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005538 File Offset: 0x00003738
		private void RemoveNode(string qualifiedName)
		{
			TreeNode nodeByFullPath = this.tvStrings.GetNodeByFullPath(qualifiedName);
			if (nodeByFullPath != null && this.stringFile.Remove(qualifiedName))
			{
				this.tvStrings.Nodes.Remove(nodeByFullPath);
				this.MadeChanges();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000557C File Offset: 0x0000377C
		private void AddString(string qualifiedName)
		{
			if (this.tvStrings.GetNodeByFullPath(qualifiedName) == null)
			{
				if (this.stringFile.Put(qualifiedName, string.Empty))
				{
					string[] array = qualifiedName.Split(new char[]
					{
						'.'
					});
					string fullPath = string.Join('.'.ToString(), array, 0, array.Length - 1);
					string text = array[array.Length - 1];
					DraggableTreeNode draggableTreeNode = new DraggableTreeNode(text, 2, 2);
					draggableTreeNode.Name = text;
					TreeNode nodeByFullPath = this.tvStrings.GetNodeByFullPath(fullPath);
					if (nodeByFullPath != null)
					{
						nodeByFullPath.Nodes.Add(draggableTreeNode);
					}
					else
					{
						this.tvStrings.Nodes.Add(draggableTreeNode);
					}
					this.tvStrings.SelectedNode = draggableTreeNode;
					return;
				}
			}
			else
			{
				string text2 = string.Format("The string \"{0}\" already exists.", qualifiedName);
				MessageBox.Show(this, text2, "Cannot Add String", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005654 File Offset: 0x00003854
		private void UpdateString(string qualifiedName, string[] lines)
		{
			string text = string.Join("\n", lines);
			string value = this.stringFile.Get(qualifiedName).Value;
			if (text != value)
			{
				this.stringFile.Put(qualifiedName, text);
				this.MadeChanges();
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000569F File Offset: 0x0000389F
		private void RenameNode(string oldQualifiedName, string newQualifiedName)
		{
			this.stringFile.Move(oldQualifiedName, newQualifiedName);
			this.MadeChanges();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000056B8 File Offset: 0x000038B8
		private void bAddFolder_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvStrings.SelectedNode;
			InputDialog inputDialog = new InputDialog(InputDialog.InputType.Identifier, "New Folder", "Enter the folder's name");
			if (inputDialog.ShowDialog(this) == DialogResult.OK)
			{
				string arg = (selectedNode != null) ? this.tvStrings.SelectedNode.FullPath : string.Empty;
				string qualifiedName = (selectedNode != null) ? (arg + "." + inputDialog.Value) : inputDialog.Value;
				this.AddFolder(qualifiedName);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000572C File Offset: 0x0000392C
		private void bRemoveFolder_Click(object sender, EventArgs e)
		{
			if (this.tvStrings.SelectedNode != null)
			{
				string fullPath = this.tvStrings.SelectedNode.FullPath;
				string text = string.Format("Are you sure you want to delete the folder \"{0}\" and all of its contents?", fullPath);
				if (MessageBox.Show(this, text, "Folder Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					this.RemoveNode(fullPath);
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005784 File Offset: 0x00003984
		private void bAddString_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.tvStrings.SelectedNode;
			InputDialog inputDialog = new InputDialog(InputDialog.InputType.Identifier, "New String", "Enter the string's name");
			if (inputDialog.ShowDialog(this) == DialogResult.OK)
			{
				string qualifiedName = ((selectedNode != null) ? this.tvStrings.SelectedNode.FullPath : string.Empty) + "." + inputDialog.Value;
				this.AddString(qualifiedName);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000057EC File Offset: 0x000039EC
		private void bRemoveString_Click(object sender, EventArgs e)
		{
			string fullPath = this.tvStrings.SelectedNode.FullPath;
			string text = string.Format("Are you sure you want to delete the string \"{0}\"?", this.selectedString.QualifiedName);
			if (MessageBox.Show(this, text, "String Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				this.RemoveNode(fullPath);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005840 File Offset: 0x00003A40
		private void UpdateCurrentString()
		{
			TreeNode selectedNode = this.tvStrings.SelectedNode;
			if (selectedNode is DraggableTreeNode)
			{
				DraggableTreeNode draggableTreeNode = (DraggableTreeNode)selectedNode;
				if (!draggableTreeNode.IsContainer)
				{
					this.UpdateString(draggableTreeNode.FullPath, this.tbString.Lines);
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005887 File Offset: 0x00003A87
		private void tvStrings_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			this.UpdateCurrentString();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005890 File Offset: 0x00003A90
		private void tvStrings_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (!(node is DraggableTreeNode))
			{
				if (node == null)
				{
					this.bAddFolder.Enabled = true;
					this.bRemoveFolder.Enabled = false;
					this.bAddString.Enabled = false;
					this.bRemoveString.Enabled = false;
					this.bInsert.Enabled = false;
					this.bSelect.Enabled = false;
					this.tbString.Enabled = false;
					this.tbString.Clear();
				}
				return;
			}
			DraggableTreeNode draggableTreeNode = (DraggableTreeNode)node;
			if (!draggableTreeNode.IsContainer)
			{
				string fullPath = draggableTreeNode.FullPath;
				this.bInsert.Enabled = true;
				this.tbString.Enabled = true;
				RufiniString rufiniString = this.stringFile.Get(fullPath);
				if (rufiniString.Value != null)
				{
					this.tbString.Lines = rufiniString.Value.Split(new char[]
					{
						'\n'
					});
				}
				else
				{
					MessageBox.Show(this.tbString, "Something funky happened. There's a string here, but the value couldn't be read. The string's name probably has a period in it, which isn't allowed. You'll have to recover from this manually.", "Whoa!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this.tbString.Text = string.Empty;
				}
				this.selectedString = rufiniString;
				this.bSelect.Enabled = this.selectMode;
				this.bAddFolder.Enabled = false;
				this.bRemoveFolder.Enabled = false;
				this.bRemoveString.Enabled = true;
				this.bAddString.Enabled = false;
				return;
			}
			this.bAddFolder.Enabled = true;
			this.bRemoveFolder.Enabled = true;
			this.bRemoveString.Enabled = false;
			this.bAddString.Enabled = true;
			this.bInsert.Enabled = false;
			this.bSelect.Enabled = false;
			this.tbString.Enabled = false;
			this.tbString.Clear();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005A4C File Offset: 0x00003C4C
		private void tvStrings_DragDrop(object sender, DragEventArgs e)
		{
			if (this.tvStrings.LastDragPath != null && this.tvStrings.LastDropPath != null)
			{
				string lastDragPath = this.tvStrings.LastDragPath;
				string arg = lastDragPath.Substring(lastDragPath.LastIndexOf('.') + 1);
				string newQualifiedName = this.tvStrings.LastDropPath + "." + arg;
				this.RenameNode(lastDragPath, newQualifiedName);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005887 File Offset: 0x00003A87
		private void StringDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.UpdateCurrentString();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005AAF File Offset: 0x00003CAF
		private void bSave_Click(object sender, EventArgs e)
		{
			this.UpdateCurrentString();
			this.stringFile.Save();
			this.madeChanges = false;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005AC9 File Offset: 0x00003CC9
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005AE8 File Offset: 0x00003CE8
		private void InitializeComponent()
		{
			this.components = new Container();
			this.tsStrings = new ToolStrip();
			this.bSave = new ToolStripButton();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.bAddFolder = new ToolStripButton();
			this.bRemoveFolder = new ToolStripButton();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.bAddString = new ToolStripButton();
			this.bRemoveString = new ToolStripButton();
			this.bSelect = new ToolStripButton();
			this.bCancel = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.bInsert = new ToolStripDropDownButton();
			this.tsmLetters = new ToolStripMenuItem();
			this.mainSplit = new SplitContainer();
			this.ilTree = new ImageList(this.components);
			this.tbString = new TextBox();
			this.tvStrings = new DraggableTreeView();
			this.tsStrings.SuspendLayout();
			this.mainSplit.BeginInit();
			this.mainSplit.Panel1.SuspendLayout();
			this.mainSplit.Panel2.SuspendLayout();
			this.mainSplit.SuspendLayout();
			base.SuspendLayout();
			this.tsStrings.Items.AddRange(new ToolStripItem[]
			{
				this.bSave,
				this.toolStripSeparator3,
				this.bAddFolder,
				this.bRemoveFolder,
				this.toolStripSeparator2,
				this.bAddString,
				this.bRemoveString,
				this.bSelect,
				this.bCancel,
				this.toolStripSeparator1,
				this.bInsert
			});
			this.tsStrings.Location = new Point(0, 0);
			this.tsStrings.Name = "tsStrings";
			this.tsStrings.Size = new Size(624, 25);
			this.tsStrings.TabIndex = 0;
			this.tsStrings.Text = "String Tools";
			this.bSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.bSave.Image = Resources.bSave_Image;
			this.bSave.ImageTransparentColor = Color.Magenta;
			this.bSave.Name = "bSave";
			this.bSave.Size = new Size(23, 22);
			this.bSave.Text = "Save Strings";
			this.bSave.Click += this.bSave_Click;
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new Size(6, 25);
			this.bAddFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.bAddFolder.Image = Resources.bAddFolder_Image;
			this.bAddFolder.ImageTransparentColor = Color.Magenta;
			this.bAddFolder.Name = "bAddFolder";
			this.bAddFolder.Size = new Size(23, 22);
			this.bAddFolder.Text = "Add Folder";
			this.bAddFolder.Click += this.bAddFolder_Click;
			this.bRemoveFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.bRemoveFolder.Enabled = false;
			this.bRemoveFolder.Image = Resources.bRemoveFolder_Image;
			this.bRemoveFolder.ImageTransparentColor = Color.Magenta;
			this.bRemoveFolder.Name = "bRemoveFolder";
			this.bRemoveFolder.Size = new Size(23, 22);
			this.bRemoveFolder.Text = "Delete Folder";
			this.bRemoveFolder.Click += this.bRemoveFolder_Click;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 25);
			this.bAddString.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.bAddString.Image = Resources.bAddString_Image;
			this.bAddString.ImageTransparentColor = Color.Magenta;
			this.bAddString.Name = "bAddString";
			this.bAddString.Size = new Size(23, 22);
			this.bAddString.Text = "Add String";
			this.bAddString.Click += this.bAddString_Click;
			this.bRemoveString.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.bRemoveString.Enabled = false;
			this.bRemoveString.Image = Resources.bRemoveString_Image;
			this.bRemoveString.ImageTransparentColor = Color.Magenta;
			this.bRemoveString.Name = "bRemoveString";
			this.bRemoveString.Size = new Size(23, 22);
			this.bRemoveString.Text = "Delete String";
			this.bRemoveString.Click += this.bRemoveString_Click;
			this.bSelect.Alignment = ToolStripItemAlignment.Right;
			this.bSelect.Enabled = false;
			this.bSelect.Image = Resources.bSelect_Image;
			this.bSelect.ImageTransparentColor = Color.Magenta;
			this.bSelect.Name = "bSelect";
			this.bSelect.Size = new Size(58, 22);
			this.bSelect.Text = "Select";
			this.bSelect.Click += this.bSelect_Click;
			this.bCancel.Alignment = ToolStripItemAlignment.Right;
			this.bCancel.Enabled = false;
			this.bCancel.Image = Resources.bCancel_Image;
			this.bCancel.ImageTransparentColor = Color.Magenta;
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new Size(63, 22);
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += this.bCancel_Click;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 25);
			this.bInsert.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.tsmLetters,
			});
			this.bInsert.Enabled = false;
			this.bInsert.Image = Resources.bInsert_Image;
			this.bInsert.ImageTransparentColor = Color.Magenta;
			this.bInsert.Name = "bInsert";
			this.bInsert.Size = new Size(65, 22);
			this.bInsert.Text = "Insert";
			this.tsmLetters.Name = "tsmLetters";
			this.tsmLetters.Size = new Size(165, 22);
			this.tsmLetters.Text = "PSI Levels";
			this.mainSplit.Dock = DockStyle.Fill;
			this.mainSplit.Location = new Point(0, 25);
			this.mainSplit.Name = "mainSplit";
			this.mainSplit.Panel1.Controls.Add(this.tvStrings);
			this.mainSplit.Panel2.Controls.Add(this.tbString);
			this.mainSplit.Size = new Size(624, 336);
			this.mainSplit.SplitterDistance = 207;
			this.mainSplit.TabIndex = 1;
			this.ilTree.ImageStream = Resources.ilTree_ImageStream;
			this.ilTree.TransparentColor = Color.Transparent;
			this.ilTree.Images.SetKeyName(0, "folderClosed");
			this.ilTree.Images.SetKeyName(1, "folderOpen");
			this.ilTree.Images.SetKeyName(2, "text");
			this.tbString.Dock = DockStyle.Fill;
			this.tbString.Enabled = false;
			this.tbString.Font = new Font("Lucida Console", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.tbString.Location = new Point(0, 0);
			this.tbString.Multiline = true;
			this.tbString.Name = "tbString";
			this.tbString.Size = new Size(413, 336);
			this.tbString.TabIndex = 0;
			this.tvStrings.AllowDrop = true;
			this.tvStrings.Dock = DockStyle.Fill;
			this.tvStrings.DragThreshold = 30;
			this.tvStrings.HideSelection = false;
			this.tvStrings.ImageIndex = 0;
			this.tvStrings.ImageList = this.ilTree;
			this.tvStrings.Location = new Point(0, 0);
			this.tvStrings.Name = "tvStrings";
			this.tvStrings.PathSeparator = ".";
			this.tvStrings.SelectedImageIndex = 0;
			this.tvStrings.Size = new Size(207, 336);
			this.tvStrings.Sorted = true;
			this.tvStrings.TabIndex = 0;
			this.tvStrings.BeforeSelect += this.tvStrings_BeforeSelect;
			this.tvStrings.AfterSelect += this.tvStrings_AfterSelect;
			this.tvStrings.DragDrop += this.tvStrings_DragDrop;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(624, 361);
			base.Controls.Add(this.mainSplit);
			base.Controls.Add(this.tsStrings);
			base.MinimizeBox = false;
			this.MinimumSize = new Size(500, 320);
			base.Name = "StringDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Strings";
			base.FormClosing += this.StringDialog_FormClosing;
			base.Load += this.StringDialog_Load;
			this.tsStrings.ResumeLayout(false);
			this.tsStrings.PerformLayout();
			this.mainSplit.Panel1.ResumeLayout(false);
			this.mainSplit.Panel2.ResumeLayout(false);
			this.mainSplit.Panel2.PerformLayout();
			this.mainSplit.EndInit();
			this.mainSplit.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400004D RID: 77
		private StringFile stringFile;

		// Token: 0x0400004E RID: 78
		private RufiniString selectedString;

		// Token: 0x0400004F RID: 79
		private RufiniString initialSelection;

		// Token: 0x04000050 RID: 80
		private bool selectMode;

		// Token: 0x04000051 RID: 81
		private bool madeChanges;

		// Token: 0x04000052 RID: 82
		private IContainer components;

		// Token: 0x04000053 RID: 83
		private ToolStrip tsStrings;

		// Token: 0x04000054 RID: 84
		private ToolStripButton bAddFolder;

		// Token: 0x04000055 RID: 85
		private ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000056 RID: 86
		private ToolStripButton bAddString;

		// Token: 0x04000057 RID: 87
		private ToolStripButton bRemoveString;

		// Token: 0x04000058 RID: 88
		private ToolStripButton bSelect;

		// Token: 0x04000059 RID: 89
		private ToolStripButton bCancel;

		// Token: 0x0400005A RID: 90
		private SplitContainer mainSplit;

		// Token: 0x0400005B RID: 91
		private TextBox tbString;

		// Token: 0x0400005C RID: 92
		private ToolStripButton bRemoveFolder;

		// Token: 0x0400005D RID: 93
		private ImageList ilTree;

		// Token: 0x0400005E RID: 94
		private ToolStripSeparator toolStripSeparator1;

		// Token: 0x0400005F RID: 95
		private ToolStripDropDownButton bInsert;

		// Token: 0x04000062 RID: 98
		private ToolStripMenuItem tsmLetters;

		// Token: 0x04000066 RID: 102
		private DraggableTreeView tvStrings;

		// Token: 0x04000067 RID: 103
		private ToolStripButton bSave;

		// Token: 0x04000068 RID: 104
		private ToolStripSeparator toolStripSeparator3;
	}
}
