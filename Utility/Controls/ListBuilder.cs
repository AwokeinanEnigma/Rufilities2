// Decompiled with JetBrains decompiler

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rufilities.Utility.Controls
{
    public class ListBuilder : UserControl
    {
        private readonly List<LabelValuePair<int>> items;
        private readonly IContainer components;
        private ListBox itemList;
        private ListBox selectionList;
        private Button addButton;
        private Button removeButton;
        private SplitContainer splitContainer;

        public int[] Selection
        {
            get
            {
                int[] selection = new int[selectionList.Items.Count];
                int index = 0;
                foreach (object obj in selectionList.Items)
                {
                    if (obj is LabelValuePair<int>)
                    {
                        LabelValuePair<int> labelValuePair = (LabelValuePair<int>)obj;
                        selection[index] = labelValuePair.Value;
                        ++index;
                    }
                }
                return selection;
            }
        }

        public ListBuilder(List<LabelValuePair<int>> items, int[] selection)
        {
            InitializeComponent();
            this.items = items;
            for (int i = 0; selection != null && i < selection.Length; ++i)
            {
                selectionList.Items.Add(new LabelValuePair<int>(this.items.First<LabelValuePair<int>>(x => x.Value == selection[i])));
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(MinimumSize.Width + Padding.Horizontal, MinimumSize.Height + Padding.Vertical);
        }

        private void ListBuilder_Load(object sender, EventArgs e)
        {
            itemList.Items.AddRange(items.ToArray());
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!(itemList.SelectedItem is LabelValuePair<int> selectedItem))
            {
                return;
            }

            selectionList.Items.Add(new LabelValuePair<int>(selectedItem));
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (!(selectionList.SelectedItem is LabelValuePair<int> selectedItem))
            {
                return;
            }

            selectionList.Items.Remove(selectedItem);
        }

        private void itemList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            addButton_Click(sender, e);
        }

        private void selectionList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            removeButton_Click(sender, e);
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
            itemList = new ListBox();
            selectionList = new ListBox();
            addButton = new Button();
            removeButton = new Button();
            splitContainer = new SplitContainer();
            splitContainer.BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            itemList.Dock = DockStyle.Fill;
            itemList.FormattingEnabled = true;
            itemList.IntegralHeight = false;
            itemList.Location = new Point(0, 0);
            itemList.Name = "itemList";
            itemList.Size = new Size(137, 151);
            itemList.TabIndex = 0;
            itemList.MouseDoubleClick += new MouseEventHandler(itemList_MouseDoubleClick);
            selectionList.Dock = DockStyle.Fill;
            selectionList.FormattingEnabled = true;
            selectionList.IntegralHeight = false;
            selectionList.Location = new Point(0, 0);
            selectionList.Name = "selectionList";
            selectionList.Size = new Size(139, 151);
            selectionList.TabIndex = 1;
            selectionList.MouseDoubleClick += new MouseEventHandler(selectionList_MouseDoubleClick);
            addButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            addButton.Image = Properties.Resources.addButton_Image;
            addButton.ImageAlign = ContentAlignment.MiddleLeft;
            addButton.Location = new Point(0, 157);
            addButton.Name = "addButton";
            addButton.Size = new Size(138, 23);
            addButton.TabIndex = 2;
            addButton.Text = "Add";
            addButton.TextAlign = ContentAlignment.MiddleRight;
            addButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += new EventHandler(addButton_Click);
            removeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            removeButton.Image = Properties.Resources.removeButton_Image;
            removeButton.ImageAlign = ContentAlignment.MiddleRight;
            removeButton.Location = new Point(142, 157);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(138, 23);
            removeButton.TabIndex = 3;
            removeButton.Text = "Remove";
            removeButton.TextAlign = ContentAlignment.MiddleLeft;
            removeButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += new EventHandler(removeButton_Click);
            splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Panel1.Controls.Add(itemList);
            splitContainer.Panel1MinSize = 137;
            splitContainer.Panel2.Controls.Add(selectionList);
            splitContainer.Panel2MinSize = 137;
            splitContainer.Size = new Size(280, 151);
            splitContainer.SplitterDistance = 137;
            splitContainer.TabIndex = 4;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer);
            Controls.Add(removeButton);
            Controls.Add(addButton);
            MinimumSize = new Size(280, 180);
            Name = "ListBuilder";
            Size = new Size(280, 180);
            Load += new EventHandler(ListBuilder_Load);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
