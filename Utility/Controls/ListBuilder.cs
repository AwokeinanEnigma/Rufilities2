// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Controls.ListBuilder
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

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
    private List<LabelValuePair<int>> items;
    private IContainer components;
    private ListBox itemList;
    private ListBox selectionList;
    private Button addButton;
    private Button removeButton;
    private SplitContainer splitContainer;

    public int[] Selection
    {
      get
      {
        int[] selection = new int[this.selectionList.Items.Count];
        int index = 0;
        foreach (object obj in this.selectionList.Items)
        {
          if (obj is LabelValuePair<int>)
          {
            LabelValuePair<int> labelValuePair = (LabelValuePair<int>) obj;
            selection[index] = labelValuePair.Value;
            ++index;
          }
        }
        return selection;
      }
    }

    public ListBuilder(List<LabelValuePair<int>> items, int[] selection)
    {
      this.InitializeComponent();
      this.items = items;
      for (int i = 0; selection != null && i < selection.Length; ++i)
        this.selectionList.Items.Add((object) new LabelValuePair<int>(this.items.First<LabelValuePair<int>>((Func<LabelValuePair<int>, bool>) (x => x.Value == selection[i]))));
    }

    public override Size GetPreferredSize(Size proposedSize) => new Size(this.MinimumSize.Width + this.Padding.Horizontal, this.MinimumSize.Height + this.Padding.Vertical);

    private void ListBuilder_Load(object sender, EventArgs e) => this.itemList.Items.AddRange((object[]) this.items.ToArray());

    private void addButton_Click(object sender, EventArgs e)
    {
      if (!(this.itemList.SelectedItem is LabelValuePair<int> selectedItem))
        return;
      this.selectionList.Items.Add((object) new LabelValuePair<int>(selectedItem));
    }

    private void removeButton_Click(object sender, EventArgs e)
    {
      if (!(this.selectionList.SelectedItem is LabelValuePair<int> selectedItem))
        return;
      this.selectionList.Items.Remove((object) selectedItem);
    }

    private void itemList_MouseDoubleClick(object sender, MouseEventArgs e) => this.addButton_Click(sender, (EventArgs) e);

    private void selectionList_MouseDoubleClick(object sender, MouseEventArgs e) => this.removeButton_Click(sender, (EventArgs) e);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.itemList = new ListBox();
      this.selectionList = new ListBox();
      this.addButton = new Button();
      this.removeButton = new Button();
      this.splitContainer = new SplitContainer();
      this.splitContainer.BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.SuspendLayout();
      this.itemList.Dock = DockStyle.Fill;
      this.itemList.FormattingEnabled = true;
      this.itemList.IntegralHeight = false;
      this.itemList.Location = new Point(0, 0);
      this.itemList.Name = "itemList";
      this.itemList.Size = new Size(137, 151);
      this.itemList.TabIndex = 0;
      this.itemList.MouseDoubleClick += new MouseEventHandler(this.itemList_MouseDoubleClick);
      this.selectionList.Dock = DockStyle.Fill;
      this.selectionList.FormattingEnabled = true;
      this.selectionList.IntegralHeight = false;
      this.selectionList.Location = new Point(0, 0);
      this.selectionList.Name = "selectionList";
      this.selectionList.Size = new Size(139, 151);
      this.selectionList.TabIndex = 1;
      this.selectionList.MouseDoubleClick += new MouseEventHandler(this.selectionList_MouseDoubleClick);
      this.addButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.addButton.Image = Properties.Resources.addButton_Image;
            this.addButton.ImageAlign = ContentAlignment.MiddleLeft;
      this.addButton.Location = new Point(0, 157);
      this.addButton.Name = "addButton";
      this.addButton.Size = new Size(138, 23);
      this.addButton.TabIndex = 2;
      this.addButton.Text = "Add";
      this.addButton.TextAlign = ContentAlignment.MiddleRight;
      this.addButton.TextImageRelation = TextImageRelation.TextBeforeImage;
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new EventHandler(this.addButton_Click);
      this.removeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.removeButton.Image = Properties.Resources.removeButton_Image;
            this.removeButton.ImageAlign = ContentAlignment.MiddleRight;
      this.removeButton.Location = new Point(142, 157);
      this.removeButton.Name = "removeButton";
      this.removeButton.Size = new Size(138, 23);
      this.removeButton.TabIndex = 3;
      this.removeButton.Text = "Remove";
      this.removeButton.TextAlign = ContentAlignment.MiddleLeft;
      this.removeButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.removeButton.UseVisualStyleBackColor = true;
      this.removeButton.Click += new EventHandler(this.removeButton_Click);
      this.splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer.IsSplitterFixed = true;
      this.splitContainer.Location = new Point(0, 0);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Panel1.Controls.Add((Control) this.itemList);
      this.splitContainer.Panel1MinSize = 137;
      this.splitContainer.Panel2.Controls.Add((Control) this.selectionList);
      this.splitContainer.Panel2MinSize = 137;
      this.splitContainer.Size = new Size(280, 151);
      this.splitContainer.SplitterDistance = 137;
      this.splitContainer.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainer);
      this.Controls.Add((Control) this.removeButton);
      this.Controls.Add((Control) this.addButton);
      this.MinimumSize = new Size(280, 180);
      this.Name = "ListBuilder";
      this.Size = new Size(280, 180);
      this.Load += new EventHandler(this.ListBuilder_Load);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.EndInit();
      this.splitContainer.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
