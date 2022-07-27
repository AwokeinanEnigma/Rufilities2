// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Dialogs.ColorEditDialog
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Rufilities.Utility.Dialogs
{
  public class ColorEditDialog : Form
  {
    private Color origColor;
    private Color newColor;
    private IContainer components;
    private TrackBar alphaBar;
    private Label label1;
    private Button saveButton;
    private NumericUpDown alphaBox;
    private PictureBox origPreviewBox;
    private PictureBox newPreviewBox;
    private Button cancelButton;
    private NumericUpDown redBox;
    private Label label2;
    private TrackBar redBar;
    private NumericUpDown greenBox;
    private Label label3;
    private TrackBar greenBar;
    private NumericUpDown blueBox;
    private Label label4;
    private TrackBar blueBar;
    private Label label5;
    private TextBox hexBox;

    public Color Color => this.newColor;

    public ColorEditDialog(Color color)
    {
      this.InitializeComponent();
      this.origColor = color;
      this.newColor = color;
      this.alphaBox.Value = (Decimal) color.A;
      this.redBox.Value = (Decimal) color.R;
      this.greenBox.Value = (Decimal) color.G;
      this.blueBox.Value = (Decimal) color.B;
      this.UpdateColor();
    }

    private void alphaBar_Scroll(object sender, EventArgs e)
    {
      this.alphaBox.Value = (Decimal) this.alphaBar.Value;
      this.UpdateColor();
    }

    private void alphaBox_ValueChanged(object sender, EventArgs e)
    {
      this.alphaBar.Value = (int) this.alphaBox.Value;
      this.UpdateColor();
    }

    private void redBar_Scroll(object sender, EventArgs e)
    {
      this.redBox.Value = (Decimal) this.redBar.Value;
      this.UpdateColor();
    }

    private void redBox_ValueChanged(object sender, EventArgs e)
    {
      this.redBar.Value = (int) this.redBox.Value;
      this.UpdateColor();
    }

    private void greenBar_Scroll(object sender, EventArgs e)
    {
      this.greenBox.Value = (Decimal) this.greenBar.Value;
      this.UpdateColor();
    }

    private void greenBox_ValueChanged(object sender, EventArgs e)
    {
      this.greenBar.Value = (int) this.greenBox.Value;
      this.UpdateColor();
    }

    private void blueBar_Scroll(object sender, EventArgs e)
    {
      this.blueBox.Value = (Decimal) this.blueBar.Value;
      this.UpdateColor();
    }

    private void blueBox_ValueChanged(object sender, EventArgs e)
    {
      this.blueBar.Value = (int) this.blueBox.Value;
      this.UpdateColor();
    }

    private void UpdateColor()
    {
      this.newColor = Color.FromArgb(this.alphaBar.Value, this.redBar.Value, this.greenBar.Value, this.blueBar.Value);
      this.newPreviewBox.Refresh();
      this.hexBox.TextChanged -= new EventHandler(this.hexBox_TextChanged);
      this.hexBox.Text = string.Format("{0:x2}{1:x2}{2:x2}", (object) this.newColor.R, (object) this.newColor.G, (object) this.newColor.B);
      this.hexBox.TextChanged += new EventHandler(this.hexBox_TextChanged);
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void origPreviewBox_Paint(object sender, PaintEventArgs e) => e.Graphics.FillRectangle((Brush) new SolidBrush(this.origColor), new Rectangle(new Point(), this.origPreviewBox.Size));

    private void newPreviewBox_Paint(object sender, PaintEventArgs e) => e.Graphics.FillRectangle((Brush) new SolidBrush(this.newColor), new Rectangle(new Point(), this.origPreviewBox.Size));

    private void hexBox_TextChanged(object sender, EventArgs e)
    {
      string text = this.hexBox.Text;
      if (text.Length < 6)
        return;
      string[] strArray = new string[3]
      {
        text.Substring(0, 2),
        text.Substring(2, 2),
        text.Substring(4, 2)
      };
      int result1;
      bool flag1 = int.TryParse(strArray[0], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result1);
      int result2;
      bool flag2 = int.TryParse(strArray[1], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result2);
      int result3;
      bool flag3 = int.TryParse(strArray[2], NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result3);
      if (!flag1 || !flag2 || !flag3)
        return;
      this.newColor = Color.FromArgb(this.alphaBar.Value, result1, result2, result3);
      this.redBox.Value = (Decimal) result1;
      this.greenBox.Value = (Decimal) result2;
      this.blueBox.Value = (Decimal) result3;
      this.newPreviewBox.Refresh();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.alphaBar = new TrackBar();
      this.label1 = new Label();
      this.saveButton = new Button();
      this.alphaBox = new NumericUpDown();
      this.newPreviewBox = new PictureBox();
      this.origPreviewBox = new PictureBox();
      this.cancelButton = new Button();
      this.redBox = new NumericUpDown();
      this.label2 = new Label();
      this.redBar = new TrackBar();
      this.greenBox = new NumericUpDown();
      this.label3 = new Label();
      this.greenBar = new TrackBar();
      this.blueBox = new NumericUpDown();
      this.label4 = new Label();
      this.blueBar = new TrackBar();
      this.label5 = new Label();
      this.hexBox = new TextBox();
      this.alphaBar.BeginInit();
      this.alphaBox.BeginInit();
      ((ISupportInitialize) this.newPreviewBox).BeginInit();
      ((ISupportInitialize) this.origPreviewBox).BeginInit();
      this.redBox.BeginInit();
      this.redBar.BeginInit();
      this.greenBox.BeginInit();
      this.greenBar.BeginInit();
      this.blueBox.BeginInit();
      this.blueBar.BeginInit();
      this.SuspendLayout();
      this.alphaBar.LargeChange = 8;
      this.alphaBar.Location = new Point(55, 271);
      this.alphaBar.Maximum = (int) byte.MaxValue;
      this.alphaBar.Name = "alphaBar";
      this.alphaBar.Size = new Size(260, 45);
      this.alphaBar.TabIndex = 0;
      this.alphaBar.TickFrequency = 8;
      this.alphaBar.Value = (int) byte.MaxValue;
      this.alphaBar.Scroll += new EventHandler(this.alphaBar_Scroll);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 276);
      this.label1.Name = "label1";
      this.label1.Size = new Size(37, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Alpha:";
      this.label1.TextAlign = ContentAlignment.TopRight;
      this.saveButton.Location = new Point(240, 322);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new Size(60, 23);
      this.saveButton.TabIndex = 2;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new EventHandler(this.saveButton_Click);
      this.alphaBox.Location = new Point(321, 274);
      this.alphaBox.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.alphaBox.Name = "alphaBox";
      this.alphaBox.Size = new Size(45, 20);
      this.alphaBox.TabIndex = 3;
      this.alphaBox.Value = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.alphaBox.ValueChanged += new EventHandler(this.alphaBox_ValueChanged);
            this.newPreviewBox.BackgroundImage = Properties.Resources.newPreviewBox_BackgroundImage; // (Image) componentResourceManager.GetObject("newPreviewBox.BackgroundImage");
            this.newPreviewBox.Location = new Point(183, 12);
      this.newPreviewBox.Name = "newPreviewBox";
      this.newPreviewBox.Size = new Size(128, 100);
      this.newPreviewBox.TabIndex = 5;
      this.newPreviewBox.TabStop = false;
      this.newPreviewBox.Paint += new PaintEventHandler(this.newPreviewBox_Paint);
      this.origPreviewBox.BackgroundImage = Properties.Resources.origPreviewBox_BackgroundImage;
            this.origPreviewBox.Location = new Point(55, 12);
      this.origPreviewBox.Name = "origPreviewBox";
      this.origPreviewBox.Size = new Size(128, 100);
      this.origPreviewBox.TabIndex = 4;
      this.origPreviewBox.TabStop = false;
      this.origPreviewBox.Paint += new PaintEventHandler(this.origPreviewBox_Paint);
      this.cancelButton.Location = new Point(306, 322);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(60, 23);
      this.cancelButton.TabIndex = 6;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
      this.redBox.Location = new Point(321, 121);
      this.redBox.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.redBox.Name = "redBox";
      this.redBox.Size = new Size(45, 20);
      this.redBox.TabIndex = 9;
      this.redBox.Value = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.redBox.ValueChanged += new EventHandler(this.redBox_ValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(19, 123);
      this.label2.Name = "label2";
      this.label2.Size = new Size(30, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Red:";
      this.label2.TextAlign = ContentAlignment.TopRight;
      this.redBar.LargeChange = 8;
      this.redBar.Location = new Point(55, 118);
      this.redBar.Maximum = (int) byte.MaxValue;
      this.redBar.Name = "redBar";
      this.redBar.Size = new Size(260, 45);
      this.redBar.TabIndex = 7;
      this.redBar.TickFrequency = 8;
      this.redBar.Value = (int) byte.MaxValue;
      this.redBar.Scroll += new EventHandler(this.redBar_Scroll);
      this.greenBox.Location = new Point(321, 172);
      this.greenBox.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.greenBox.Name = "greenBox";
      this.greenBox.Size = new Size(45, 20);
      this.greenBox.TabIndex = 12;
      this.greenBox.Value = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.greenBox.ValueChanged += new EventHandler(this.greenBox_ValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 174);
      this.label3.Name = "label3";
      this.label3.Size = new Size(39, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "Green:";
      this.label3.TextAlign = ContentAlignment.TopRight;
      this.greenBar.LargeChange = 8;
      this.greenBar.Location = new Point(55, 169);
      this.greenBar.Maximum = (int) byte.MaxValue;
      this.greenBar.Name = "greenBar";
      this.greenBar.Size = new Size(260, 45);
      this.greenBar.TabIndex = 10;
      this.greenBar.TickFrequency = 8;
      this.greenBar.Value = (int) byte.MaxValue;
      this.greenBar.Scroll += new EventHandler(this.greenBar_Scroll);
      this.blueBox.Location = new Point(321, 223);
      this.blueBox.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.blueBox.Name = "blueBox";
      this.blueBox.Size = new Size(45, 20);
      this.blueBox.TabIndex = 15;
      this.blueBox.Value = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.blueBox.ValueChanged += new EventHandler(this.blueBox_ValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(19, 225);
      this.label4.Name = "label4";
      this.label4.Size = new Size(31, 13);
      this.label4.TabIndex = 14;
      this.label4.Text = "Blue:";
      this.label4.TextAlign = ContentAlignment.TopRight;
      this.blueBar.LargeChange = 8;
      this.blueBar.Location = new Point(55, 220);
      this.blueBar.Maximum = (int) byte.MaxValue;
      this.blueBar.Name = "blueBar";
      this.blueBar.Size = new Size(260, 45);
      this.blueBar.TabIndex = 13;
      this.blueBar.TickFrequency = 8;
      this.blueBar.Value = (int) byte.MaxValue;
      this.blueBar.ValueChanged += new EventHandler(this.blueBar_Scroll);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 327);
      this.label5.Name = "label5";
      this.label5.Size = new Size(71, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Hexadecimal:";
      this.hexBox.Location = new Point(89, 324);
      this.hexBox.Name = "hexBox";
      this.hexBox.Size = new Size(60, 20);
      this.hexBox.TabIndex = 17;
      this.hexBox.TextChanged += new EventHandler(this.hexBox_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(378, 357);
      this.Controls.Add((Control) this.hexBox);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.blueBox);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.blueBar);
      this.Controls.Add((Control) this.greenBox);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.greenBar);
      this.Controls.Add((Control) this.redBox);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.redBar);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.newPreviewBox);
      this.Controls.Add((Control) this.origPreviewBox);
      this.Controls.Add((Control) this.alphaBox);
      this.Controls.Add((Control) this.saveButton);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.alphaBar);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ColorEditDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Edit Color";
      this.alphaBar.EndInit();
      this.alphaBox.EndInit();
      ((ISupportInitialize) this.newPreviewBox).EndInit();
      ((ISupportInitialize) this.origPreviewBox).EndInit();
      this.redBox.EndInit();
      this.redBar.EndInit();
      this.greenBox.EndInit();
      this.greenBar.EndInit();
      this.blueBox.EndInit();
      this.blueBar.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
