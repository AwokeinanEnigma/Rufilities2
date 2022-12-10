using Rufilities.Properties;
using Rufilities.Utility;
using Rufilities.Utility.Controls;
using Rufilities.Utility.Dialogs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rufilities.Strings.Dialogs
{
    public class StringDialog : Form
    {
        public RufiniString SelectedString => selectedString;
        public bool ChangesMade => madeChanges;
        public StringDialog(StringFile stringFile, bool selectMode, RufiniString initialSelection)
        {
            this.stringFile = stringFile;
            this.selectMode = selectMode;
            this.initialSelection = initialSelection;
            InitializeComponent();
        }
        private void MadeChanges()
        {
            madeChanges = true;
        }
        private void InsertStringInString(string insert)
        {
            int selectionStart = tbString.SelectionStart;
            tbString.Text = tbString.Text.Remove(tbString.SelectionStart, tbString.SelectionLength).Insert(tbString.SelectionStart, insert);
            tbString.SelectionStart = selectionStart;
            tbString.SelectionLength = insert.Length;
        }
        private void PopulateInsertMenu()
        {
            string[] psi_LEVELS = Constants.GREEK_CHARACTERS;
            for (int i = 0; i < psi_LEVELS.Length; i++)
            {
                string s = psi_LEVELS[i];
                tsmLetters.DropDownItems.Add(s, null, delegate (object sender, EventArgs e)
                {
                    InsertStringInString(s);
                });
            }
        }
        private void PopulateSubTree(TreeNode parentTreeNode, StringNode node)
        {
            int num = node.IsContainer ? 0 : 2;
            DraggableTreeNode draggableTreeNode = new DraggableTreeNode(node.Name, num, num)
            {
                Name = node.Name,
                IsContainer = node.IsContainer
            };
            if (parentTreeNode != null)
            {
                parentTreeNode.Nodes.Add(draggableTreeNode);
            }
            else
            {
                tvStrings.Nodes.Add(draggableTreeNode);
            }
            foreach (StringNode node2 in node.Children)
            {
                PopulateSubTree(draggableTreeNode, node2);
            }
        }
        private void PopulateTreeView()
        {
            TreeNode treeNode = null;
            foreach (StringNode node in stringFile.ToNodes())
            {
                PopulateSubTree(null, node);
            }
            if (initialSelection.Names != null)
            {
                string[] names = initialSelection.Names;
                TreeNodeCollection treeNodeCollection = tvStrings.Nodes;
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
            tvStrings.Sort();
            if (treeNode != null)
            {
                tvStrings.SelectedNode = treeNode;
                tbString.Enabled = true;
                bInsert.Enabled = true;
            }
        }
        private void StringDialog_Load(object sender, EventArgs e)
        {
            if (!selectMode)
            {
                bSelect.Visible = false;
                bCancel.Visible = false;
            }
            else
            {
                bCancel.Enabled = true;
            }
            PopulateTreeView();
            PopulateInsertMenu();
        }
        private void bSelect_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            UpdateCurrentString();
            stringFile.Save();
            madeChanges = false;
            base.Close();
        }
        private void bCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }
        private void AddFolder(string qualifiedName)
        {
            if (tvStrings.GetNodeByFullPath(qualifiedName) == null)
            {
                if (stringFile.PutFolder(qualifiedName))
                {
                    string[] array = qualifiedName.Split(new char[]
                    {
                        '.'
                    });
                    string fullPath = string.Join('.'.ToString(), array, 0, array.Length - 1);
                    string text = array[array.Length - 1];
                    DraggableTreeNode draggableTreeNode = new DraggableTreeNode(text, 0, 0)
                    {
                        Name = text,
                        IsContainer = true
                    };
                    TreeNode nodeByFullPath = tvStrings.GetNodeByFullPath(fullPath);
                    if (nodeByFullPath != null)
                    {
                        nodeByFullPath.Nodes.Add(draggableTreeNode);
                    }
                    else
                    {
                        tvStrings.Nodes.Add(draggableTreeNode);
                    }
                    tvStrings.SelectedNode = draggableTreeNode;
                    MadeChanges();
                    return;
                }
            }
            else
            {
                string text2 = string.Format("The folder \"{0}\" already exists.", qualifiedName);
                MessageBox.Show(this, text2, "Cannot Add Folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void RemoveNode(string qualifiedName)
        {
            TreeNode nodeByFullPath = tvStrings.GetNodeByFullPath(qualifiedName);
            if (nodeByFullPath != null && stringFile.Remove(qualifiedName))
            {
                tvStrings.Nodes.Remove(nodeByFullPath);
                MadeChanges();
            }
        }
        private void AddString(string qualifiedName)
        {
            if (tvStrings.GetNodeByFullPath(qualifiedName) == null)
            {
                if (stringFile.Put(qualifiedName, string.Empty))
                {
                    string[] array = qualifiedName.Split(new char[]
                    {
                        '.'
                    });
                    string fullPath = string.Join('.'.ToString(), array, 0, array.Length - 1);
                    string text = array[array.Length - 1];
                    DraggableTreeNode draggableTreeNode = new DraggableTreeNode(text, 2, 2)
                    {
                        Name = text
                    };
                    TreeNode nodeByFullPath = tvStrings.GetNodeByFullPath(fullPath);
                    if (nodeByFullPath != null)
                    {
                        nodeByFullPath.Nodes.Add(draggableTreeNode);
                    }
                    else
                    {
                        tvStrings.Nodes.Add(draggableTreeNode);
                    }
                    tvStrings.SelectedNode = draggableTreeNode;
                    return;
                }
            }
            else
            {
                string text2 = string.Format("The string \"{0}\" already exists.", qualifiedName);
                MessageBox.Show(this, text2, "Cannot Add String", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void UpdateString(string qualifiedName, string[] lines)
        {
            string text = string.Join("\n", lines);
            string value = stringFile.Get(qualifiedName).Value;
            if (text != value)
            {
                stringFile.Put(qualifiedName, text);
                MadeChanges();
            }
        }
        private void RenameNode(string oldQualifiedName, string newQualifiedName)
        {
            stringFile.Move(oldQualifiedName, newQualifiedName);
            MadeChanges();
        }
        private void bAddFolder_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvStrings.SelectedNode;
            InputDialog inputDialog = new InputDialog(InputDialog.InputType.Identifier, "New Folder", "Enter the folder's name");
            if (inputDialog.ShowDialog(this) == DialogResult.OK)
            {
                string arg = (selectedNode != null) ? tvStrings.SelectedNode.FullPath : string.Empty;
                string qualifiedName = (selectedNode != null) ? (arg + "." + inputDialog.Value) : inputDialog.Value;
                AddFolder(qualifiedName);
            }
        }
        private void bRemoveFolder_Click(object sender, EventArgs e)
        {
            if (tvStrings.SelectedNode != null)
            {
                string fullPath = tvStrings.SelectedNode.FullPath;
                string text = string.Format("Are you sure you want to delete the folder \"{0}\" and all of its contents?", fullPath);
                if (MessageBox.Show(this, text, "Folder Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    RemoveNode(fullPath);
                }
            }
        }
        private void bAddString_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = tvStrings.SelectedNode;
            InputDialog inputDialog = new InputDialog(InputDialog.InputType.Identifier, "New String", "Enter the string's name");
            if (inputDialog.ShowDialog(this) == DialogResult.OK)
            {
                string qualifiedName = ((selectedNode != null) ? tvStrings.SelectedNode.FullPath : string.Empty) + "." + inputDialog.Value;
                AddString(qualifiedName);
            }
        }
        private void bRemoveString_Click(object sender, EventArgs e)
        {
            string fullPath = tvStrings.SelectedNode.FullPath;
            string text = string.Format("Are you sure you want to delete the string \"{0}\"?", selectedString.QualifiedName);
            if (MessageBox.Show(this, text, "String Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RemoveNode(fullPath);
            }
        }
        private void UpdateCurrentString()
        {
            TreeNode selectedNode = tvStrings.SelectedNode;
            if (selectedNode is DraggableTreeNode)
            {
                DraggableTreeNode draggableTreeNode = (DraggableTreeNode)selectedNode;
                if (!draggableTreeNode.IsContainer)
                {
                    UpdateString(draggableTreeNode.FullPath, tbString.Lines);
                }
            }
        }
        private void tvStrings_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            UpdateCurrentString();
        }
        private void tvStrings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (!(node is DraggableTreeNode))
            {
                if (node == null)
                {
                    bAddFolder.Enabled = true;
                    bRemoveFolder.Enabled = false;
                    bAddString.Enabled = false;
                    bRemoveString.Enabled = false;
                    bInsert.Enabled = false;
                    bSelect.Enabled = false;
                    tbString.Enabled = false;
                    tbString.Clear();
                }
                return;
            }
            DraggableTreeNode draggableTreeNode = (DraggableTreeNode)node;
            if (!draggableTreeNode.IsContainer)
            {
                string fullPath = draggableTreeNode.FullPath;
                bInsert.Enabled = true;
                tbString.Enabled = true;
                RufiniString rufiniString = stringFile.Get(fullPath);
                if (rufiniString.Value != null)
                {
                    tbString.Lines = rufiniString.Value.Split(new char[]
                    {
                        '\n'
                    });
                }
                else
                {
                    MessageBox.Show(tbString, "Something funky happened. There's a string here, but the value couldn't be read. The string's name probably has a period in it, which isn't allowed. You'll have to recover from this manually.", "Whoa!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbString.Text = string.Empty;
                }
                selectedString = rufiniString;
                bSelect.Enabled = selectMode;
                bAddFolder.Enabled = false;
                bRemoveFolder.Enabled = false;
                bRemoveString.Enabled = true;
                bAddString.Enabled = false;
                return;
            }
            bAddFolder.Enabled = true;
            bRemoveFolder.Enabled = true;
            bRemoveString.Enabled = false;
            bAddString.Enabled = true;
            bInsert.Enabled = false;
            bSelect.Enabled = false;
            tbString.Enabled = false;
            tbString.Clear();
        }
        private void tvStrings_DragDrop(object sender, DragEventArgs e)
        {
            if (tvStrings.LastDragPath != null && tvStrings.LastDropPath != null)
            {
                string lastDragPath = tvStrings.LastDragPath;
                string arg = lastDragPath.Substring(lastDragPath.LastIndexOf('.') + 1);
                string newQualifiedName = tvStrings.LastDropPath + "." + arg;
                RenameNode(lastDragPath, newQualifiedName);
            }
        }
        private void StringDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateCurrentString();
        }
        private void bSave_Click(object sender, EventArgs e)
        {
            UpdateCurrentString();
            stringFile.Save();
            madeChanges = false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new Container();
            tsStrings = new ToolStrip();
            bSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            bAddFolder = new ToolStripButton();
            bRemoveFolder = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            bAddString = new ToolStripButton();
            bRemoveString = new ToolStripButton();
            bSelect = new ToolStripButton();
            bCancel = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            bInsert = new ToolStripDropDownButton();
            tsmControls = new ToolStripMenuItem();
            tsmNames = new ToolStripMenuItem();
            tsmStats = new ToolStripMenuItem();
            tsmLetters = new ToolStripMenuItem();
            tsmValues = new ToolStripMenuItem();
            moneyToolStripMenuItem = new ToolStripMenuItem();
            mainSplit = new SplitContainer();
            ilTree = new ImageList(components);
            tbString = new TextBox();
            tvStrings = new DraggableTreeView();
            tsStrings.SuspendLayout();
            mainSplit.BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            base.SuspendLayout();
            tsStrings.Items.AddRange(new ToolStripItem[]
            {
                bSave,
                toolStripSeparator3,
                bAddFolder,
                bRemoveFolder,
                toolStripSeparator2,
                bAddString,
                bRemoveString,
                bSelect,
                bCancel,
                toolStripSeparator1,
                bInsert
            });
            tsStrings.Location = new Point(0, 0);
            tsStrings.Name = "tsStrings";
            tsStrings.Size = new Size(624, 25);
            tsStrings.TabIndex = 0;
            tsStrings.Text = "String Tools";
            bSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bSave.Image = Resources.bSave_Image;
            bSave.ImageTransparentColor = Color.Magenta;
            bSave.Name = "bSave";
            bSave.Size = new Size(23, 22);
            bSave.Text = "Save Strings";
            bSave.Click += bSave_Click;
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            bAddFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bAddFolder.Image = Resources.bAddFolder_Image;
            bAddFolder.ImageTransparentColor = Color.Magenta;
            bAddFolder.Name = "bAddFolder";
            bAddFolder.Size = new Size(23, 22);
            bAddFolder.Text = "Add Folder";
            bAddFolder.Click += bAddFolder_Click;
            bRemoveFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bRemoveFolder.Enabled = false;
            bRemoveFolder.Image = Resources.bRemoveFolder_Image;
            bRemoveFolder.ImageTransparentColor = Color.Magenta;
            bRemoveFolder.Name = "bRemoveFolder";
            bRemoveFolder.Size = new Size(23, 22);
            bRemoveFolder.Text = "Delete Folder";
            bRemoveFolder.Click += bRemoveFolder_Click;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            bAddString.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bAddString.Image = Resources.bAddString_Image;
            bAddString.ImageTransparentColor = Color.Magenta;
            bAddString.Name = "bAddString";
            bAddString.Size = new Size(23, 22);
            bAddString.Text = "Add String";
            bAddString.Click += bAddString_Click;
            bRemoveString.DisplayStyle = ToolStripItemDisplayStyle.Image;
            bRemoveString.Enabled = false;
            bRemoveString.Image = Resources.bRemoveString_Image;
            bRemoveString.ImageTransparentColor = Color.Magenta;
            bRemoveString.Name = "bRemoveString";
            bRemoveString.Size = new Size(23, 22);
            bRemoveString.Text = "Delete String";
            bRemoveString.Click += bRemoveString_Click;
            bSelect.Alignment = ToolStripItemAlignment.Right;
            bSelect.Enabled = false;
            bSelect.Image = Resources.bSelect_Image;
            bSelect.ImageTransparentColor = Color.Magenta;
            bSelect.Name = "bSelect";
            bSelect.Size = new Size(58, 22);
            bSelect.Text = "Select";
            bSelect.Click += bSelect_Click;
            bCancel.Alignment = ToolStripItemAlignment.Right;
            bCancel.Enabled = false;
            bCancel.Image = Resources.bCancel_Image;
            bCancel.ImageTransparentColor = Color.Magenta;
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(63, 22);
            bCancel.Text = "Cancel";
            bCancel.Click += bCancel_Click;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            bInsert.DropDownItems.AddRange(new ToolStripItem[]
            {
                tsmControls,
                tsmNames,
                tsmStats,
                tsmLetters,
                tsmValues
            });
            bInsert.Enabled = false;
            bInsert.Image = Resources.bInsert_Image;
            bInsert.ImageTransparentColor = Color.Magenta;
            bInsert.Name = "bInsert";
            bInsert.Size = new Size(65, 22);
            bInsert.Text = "Insert";
            tsmControls.Name = "tsmControls";
            tsmControls.Size = new Size(165, 22);
            tsmControls.Text = "Controls";
            tsmNames.Name = "tsmNames";
            tsmNames.Size = new Size(165, 22);
            tsmNames.Text = "Character Names";
            tsmStats.Name = "tsmStats";
            tsmStats.Size = new Size(165, 22);
            tsmStats.Text = "Character Stats";
            tsmLetters.Name = "tsmLetters";
            tsmLetters.Size = new Size(165, 22);
            tsmLetters.Text = "PSI Levels";
            tsmValues.DropDownItems.AddRange(new ToolStripItem[]
            {
                moneyToolStripMenuItem
            });
            tsmValues.Name = "tsmValues";
            tsmValues.Size = new Size(165, 22);
            tsmValues.Text = "Values";
            moneyToolStripMenuItem.Name = "moneyToolStripMenuItem";
            moneyToolStripMenuItem.Size = new Size(111, 22);
            moneyToolStripMenuItem.Text = "Money";
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 25);
            mainSplit.Name = "mainSplit";
            mainSplit.Panel1.Controls.Add(tvStrings);
            mainSplit.Panel2.Controls.Add(tbString);
            mainSplit.Size = new Size(624, 336);
            mainSplit.SplitterDistance = 207;
            mainSplit.TabIndex = 1;
            ilTree.ImageStream = Resources.ilTree_ImageStream;
            ilTree.TransparentColor = Color.Transparent;
            ilTree.Images.SetKeyName(0, "folderClosed");
            ilTree.Images.SetKeyName(1, "folderOpen");
            ilTree.Images.SetKeyName(2, "text");
            tbString.Dock = DockStyle.Fill;
            tbString.Enabled = false;
            tbString.Font = new Font("Lucida Console", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbString.Location = new Point(0, 0);
            tbString.Multiline = true;
            tbString.Name = "tbString";
            tbString.Size = new Size(413, 336);
            tbString.TabIndex = 0;
            tvStrings.AllowDrop = true;
            tvStrings.Dock = DockStyle.Fill;
            tvStrings.DragThreshold = 30;
            tvStrings.HideSelection = false;
            tvStrings.ImageIndex = 0;
            tvStrings.ImageList = ilTree;
            tvStrings.Location = new Point(0, 0);
            tvStrings.Name = "tvStrings";
            tvStrings.PathSeparator = ".";
            tvStrings.SelectedImageIndex = 0;
            tvStrings.Size = new Size(207, 336);
            tvStrings.Sorted = true;
            tvStrings.TabIndex = 0;
            tvStrings.BeforeSelect += tvStrings_BeforeSelect;
            tvStrings.AfterSelect += tvStrings_AfterSelect;
            tvStrings.DragDrop += tvStrings_DragDrop;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(624, 361);
            base.Controls.Add(mainSplit);
            base.Controls.Add(tsStrings);
            base.MinimizeBox = false;
            MinimumSize = new Size(500, 320);
            base.Name = "StringDialog";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            Text = "Strings";
            base.FormClosing += StringDialog_FormClosing;
            base.Load += StringDialog_Load;
            tsStrings.ResumeLayout(false);
            tsStrings.PerformLayout();
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            mainSplit.Panel2.PerformLayout();
            mainSplit.EndInit();
            mainSplit.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
        private readonly StringFile stringFile;
        private RufiniString selectedString;
        private RufiniString initialSelection;
        private readonly bool selectMode;
        private bool madeChanges;
        private IContainer components;
        private ToolStrip tsStrings;
        private ToolStripButton bAddFolder;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton bAddString;
        private ToolStripButton bRemoveString;
        private ToolStripButton bSelect;
        private ToolStripButton bCancel;
        private SplitContainer mainSplit;
        private TextBox tbString;
        private ToolStripButton bRemoveFolder;
        private ImageList ilTree;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripDropDownButton bInsert;
        private ToolStripMenuItem tsmNames;
        private ToolStripMenuItem tsmStats;
        private ToolStripMenuItem tsmLetters;
        private ToolStripMenuItem tsmValues;
        private ToolStripMenuItem moneyToolStripMenuItem;
        private ToolStripMenuItem tsmControls;
        private DraggableTreeView tvStrings;
        private ToolStripButton bSave;
        private ToolStripSeparator toolStripSeparator3;
    }
}
