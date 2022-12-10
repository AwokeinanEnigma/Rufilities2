// Decompiled with JetBrains decompiler

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Rufilities.Utility.Dialogs
{
    public class ColorEditDialog : Form
    {
        private readonly Color origColor;
        private Color newColor;
        private readonly IContainer components;
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

        public Color Color => newColor;

        public ColorEditDialog(Color color)
        {
            InitializeComponent();
            origColor = color;
            newColor = color;
            alphaBox.Value = color.A;
            redBox.Value = color.R;
            greenBox.Value = color.G;
            blueBox.Value = color.B;
            UpdateColor();
        }

        private void alphaBar_Scroll(object sender, EventArgs e)
        {
            alphaBox.Value = alphaBar.Value;
            UpdateColor();
        }

        private void alphaBox_ValueChanged(object sender, EventArgs e)
        {
            alphaBar.Value = (int)alphaBox.Value;
            UpdateColor();
        }

        private void redBar_Scroll(object sender, EventArgs e)
        {
            redBox.Value = redBar.Value;
            UpdateColor();
        }

        private void redBox_ValueChanged(object sender, EventArgs e)
        {
            redBar.Value = (int)redBox.Value;
            UpdateColor();
        }

        private void greenBar_Scroll(object sender, EventArgs e)
        {
            greenBox.Value = greenBar.Value;
            UpdateColor();
        }

        private void greenBox_ValueChanged(object sender, EventArgs e)
        {
            greenBar.Value = (int)greenBox.Value;
            UpdateColor();
        }

        private void blueBar_Scroll(object sender, EventArgs e)
        {
            blueBox.Value = blueBar.Value;
            UpdateColor();
        }

        private void blueBox_ValueChanged(object sender, EventArgs e)
        {
            blueBar.Value = (int)blueBox.Value;
            UpdateColor();
        }

        private void UpdateColor()
        {
            newColor = Color.FromArgb(alphaBar.Value, redBar.Value, greenBar.Value, blueBar.Value);
            newPreviewBox.Refresh();
            hexBox.TextChanged -= new EventHandler(hexBox_TextChanged);
            hexBox.Text = string.Format("{0:x2}{1:x2}{2:x2}", newColor.R, newColor.G, newColor.B);
            hexBox.TextChanged += new EventHandler(hexBox_TextChanged);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void origPreviewBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(origColor), new Rectangle(new Point(), origPreviewBox.Size));
        }

        private void newPreviewBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(newColor), new Rectangle(new Point(), origPreviewBox.Size));
        }

        private void hexBox_TextChanged(object sender, EventArgs e)
        {
            string text = hexBox.Text;
            if (text.Length < 6)
            {
                return;
            }

            string[] strArray = new string[3]
      {
        text.Substring(0, 2),
        text.Substring(2, 2),
        text.Substring(4, 2)
      };
            bool flag1 = int.TryParse(strArray[0], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result1);
            bool flag2 = int.TryParse(strArray[1], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result2);
            bool flag3 = int.TryParse(strArray[2], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result3);
            if (!flag1 || !flag2 || !flag3)
            {
                return;
            }

            newColor = Color.FromArgb(alphaBar.Value, result1, result2, result3);
            redBox.Value = result1;
            greenBox.Value = result2;
            blueBox.Value = result3;
            newPreviewBox.Refresh();
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
            alphaBar = new TrackBar();
            label1 = new Label();
            saveButton = new Button();
            alphaBox = new NumericUpDown();
            newPreviewBox = new PictureBox();
            origPreviewBox = new PictureBox();
            cancelButton = new Button();
            redBox = new NumericUpDown();
            label2 = new Label();
            redBar = new TrackBar();
            greenBox = new NumericUpDown();
            label3 = new Label();
            greenBar = new TrackBar();
            blueBox = new NumericUpDown();
            label4 = new Label();
            blueBar = new TrackBar();
            label5 = new Label();
            hexBox = new TextBox();
            alphaBar.BeginInit();
            alphaBox.BeginInit();
            ((ISupportInitialize)newPreviewBox).BeginInit();
            ((ISupportInitialize)origPreviewBox).BeginInit();
            redBox.BeginInit();
            redBar.BeginInit();
            greenBox.BeginInit();
            greenBar.BeginInit();
            blueBox.BeginInit();
            blueBar.BeginInit();
            SuspendLayout();
            alphaBar.LargeChange = 8;
            alphaBar.Location = new Point(55, 271);
            alphaBar.Maximum = byte.MaxValue;
            alphaBar.Name = "alphaBar";
            alphaBar.Size = new Size(260, 45);
            alphaBar.TabIndex = 0;
            alphaBar.TickFrequency = 8;
            alphaBar.Value = byte.MaxValue;
            alphaBar.Scroll += new EventHandler(alphaBar_Scroll);
            label1.AutoSize = true;
            label1.Location = new Point(12, 276);
            label1.Name = "label1";
            label1.Size = new Size(37, 13);
            label1.TabIndex = 1;
            label1.Text = "Alpha:";
            label1.TextAlign = ContentAlignment.TopRight;
            saveButton.Location = new Point(240, 322);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(60, 23);
            saveButton.TabIndex = 2;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += new EventHandler(saveButton_Click);
            alphaBox.Location = new Point(321, 274);
            alphaBox.Maximum = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            alphaBox.Name = "alphaBox";
            alphaBox.Size = new Size(45, 20);
            alphaBox.TabIndex = 3;
            alphaBox.Value = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            alphaBox.ValueChanged += new EventHandler(alphaBox_ValueChanged);
            newPreviewBox.BackgroundImage = Properties.Resources.newPreviewBox_BackgroundImage;
            newPreviewBox.Location = new Point(183, 12);
            newPreviewBox.Name = "newPreviewBox";
            newPreviewBox.Size = new Size(128, 100);
            newPreviewBox.TabIndex = 5;
            newPreviewBox.TabStop = false;
            newPreviewBox.Paint += new PaintEventHandler(newPreviewBox_Paint);
            origPreviewBox.BackgroundImage = Properties.Resources.origPreviewBox_BackgroundImage;
            origPreviewBox.Location = new Point(55, 12);
            origPreviewBox.Name = "origPreviewBox";
            origPreviewBox.Size = new Size(128, 100);
            origPreviewBox.TabIndex = 4;
            origPreviewBox.TabStop = false;
            origPreviewBox.Paint += new PaintEventHandler(origPreviewBox_Paint);
            cancelButton.Location = new Point(306, 322);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(60, 23);
            cancelButton.TabIndex = 6;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += new EventHandler(cancelButton_Click);
            redBox.Location = new Point(321, 121);
            redBox.Maximum = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            redBox.Name = "redBox";
            redBox.Size = new Size(45, 20);
            redBox.TabIndex = 9;
            redBox.Value = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            redBox.ValueChanged += new EventHandler(redBox_ValueChanged);
            label2.AutoSize = true;
            label2.Location = new Point(19, 123);
            label2.Name = "label2";
            label2.Size = new Size(30, 13);
            label2.TabIndex = 8;
            label2.Text = "Red:";
            label2.TextAlign = ContentAlignment.TopRight;
            redBar.LargeChange = 8;
            redBar.Location = new Point(55, 118);
            redBar.Maximum = byte.MaxValue;
            redBar.Name = "redBar";
            redBar.Size = new Size(260, 45);
            redBar.TabIndex = 7;
            redBar.TickFrequency = 8;
            redBar.Value = byte.MaxValue;
            redBar.Scroll += new EventHandler(redBar_Scroll);
            greenBox.Location = new Point(321, 172);
            greenBox.Maximum = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            greenBox.Name = "greenBox";
            greenBox.Size = new Size(45, 20);
            greenBox.TabIndex = 12;
            greenBox.Value = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            greenBox.ValueChanged += new EventHandler(greenBox_ValueChanged);
            label3.AutoSize = true;
            label3.Location = new Point(10, 174);
            label3.Name = "label3";
            label3.Size = new Size(39, 13);
            label3.TabIndex = 11;
            label3.Text = "Green:";
            label3.TextAlign = ContentAlignment.TopRight;
            greenBar.LargeChange = 8;
            greenBar.Location = new Point(55, 169);
            greenBar.Maximum = byte.MaxValue;
            greenBar.Name = "greenBar";
            greenBar.Size = new Size(260, 45);
            greenBar.TabIndex = 10;
            greenBar.TickFrequency = 8;
            greenBar.Value = byte.MaxValue;
            greenBar.Scroll += new EventHandler(greenBar_Scroll);
            blueBox.Location = new Point(321, 223);
            blueBox.Maximum = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            blueBox.Name = "blueBox";
            blueBox.Size = new Size(45, 20);
            blueBox.TabIndex = 15;
            blueBox.Value = new decimal(new int[4]
            {
         byte.MaxValue,
        0,
        0,
        0
            });
            blueBox.ValueChanged += new EventHandler(blueBox_ValueChanged);
            label4.AutoSize = true;
            label4.Location = new Point(19, 225);
            label4.Name = "label4";
            label4.Size = new Size(31, 13);
            label4.TabIndex = 14;
            label4.Text = "Blue:";
            label4.TextAlign = ContentAlignment.TopRight;
            blueBar.LargeChange = 8;
            blueBar.Location = new Point(55, 220);
            blueBar.Maximum = byte.MaxValue;
            blueBar.Name = "blueBar";
            blueBar.Size = new Size(260, 45);
            blueBar.TabIndex = 13;
            blueBar.TickFrequency = 8;
            blueBar.Value = byte.MaxValue;
            blueBar.ValueChanged += new EventHandler(blueBar_Scroll);
            label5.AutoSize = true;
            label5.Location = new Point(12, 327);
            label5.Name = "label5";
            label5.Size = new Size(71, 13);
            label5.TabIndex = 16;
            label5.Text = "Hexadecimal:";
            hexBox.Location = new Point(89, 324);
            hexBox.Name = "hexBox";
            hexBox.Size = new Size(60, 20);
            hexBox.TabIndex = 17;
            hexBox.TextChanged += new EventHandler(hexBox_TextChanged);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(378, 357);
            Controls.Add(hexBox);
            Controls.Add(label5);
            Controls.Add(blueBox);
            Controls.Add(label4);
            Controls.Add(blueBar);
            Controls.Add(greenBox);
            Controls.Add(label3);
            Controls.Add(greenBar);
            Controls.Add(redBox);
            Controls.Add(label2);
            Controls.Add(redBar);
            Controls.Add(cancelButton);
            Controls.Add(newPreviewBox);
            Controls.Add(origPreviewBox);
            Controls.Add(alphaBox);
            Controls.Add(saveButton);
            Controls.Add(label1);
            Controls.Add(alphaBar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ColorEditDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Color";
            alphaBar.EndInit();
            alphaBox.EndInit();
            ((ISupportInitialize)newPreviewBox).EndInit();
            ((ISupportInitialize)origPreviewBox).EndInit();
            redBox.EndInit();
            redBar.EndInit();
            greenBox.EndInit();
            greenBar.EndInit();
            blueBox.EndInit();
            blueBar.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
