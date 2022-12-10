// Decompiled with JetBrains decompiler

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rufilities.Utility.Dialogs
{
    public class InputDialog : Form
    {
        private string title;
        private string message;
        private string initialValue;
        private Regex regex;
        private string value;
        private int intValue;
        private float floatValue;
        private readonly Dictionary<InputDialog.InputType, string> validationRegex = new Dictionary<InputDialog.InputType, string>()
    {
      {
        InputDialog.InputType.String,
        "^.+$"
      },
      {
        InputDialog.InputType.Int,
        "^[0-9]+$"
      },
      {
        InputDialog.InputType.Float,
        "^[-+]?[0-9]*\\.?[0-9]*$"
      },
      {
        InputDialog.InputType.Identifier,
        "^[a-zA-Z_][0-9a-zA-Z_]*$"
      }
    };
        private readonly IContainer components;
        private Label lbMessage;
        private TextBox tbValue;
        private Button bCancel;
        private Button bOkay;
        private Label lbInvalidValue;
        private PictureBox pbInvalid;

        public string Value => value;

        public int IntValue => intValue;

        public float FloatValue => floatValue;

        public InputDialog(InputDialog.InputType inputType)
        {
            Initialize(inputType, null, null, null);
        }

        public InputDialog(InputDialog.InputType inputType, string initialValue)
        {
            Initialize(inputType, null, null, initialValue);
        }

        public InputDialog(InputDialog.InputType inputType, string title, string message)
        {
            Initialize(inputType, title, message, null);
        }

        private void Initialize(
      InputDialog.InputType inputType,
      string title,
      string message,
      string initialValue)
        {
            this.title = title;
            this.message = message;
            this.initialValue = initialValue;
            validationRegex.TryGetValue(inputType, out string pattern);
            regex = new Regex(pattern);
            InitializeComponent();
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            if (title != null)
            {
                Text = title;
            }

            if (message != null)
            {
                lbMessage.Text = string.Format("{0}:", message);
            }

            if (initialValue == null)
            {
                return;
            }

            tbValue.Text = initialValue;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void bOkay_Click(object sender, EventArgs e)
        {
            value = tbValue.Text;
            int.TryParse(value, out intValue);
            float.TryParse(value, out floatValue);
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            if (regex.Match(tbValue.Text).Success)
            {
                pbInvalid.Visible = false;
                lbInvalidValue.Visible = false;
                bOkay.Enabled = true;
            }
            else
            {
                pbInvalid.Visible = true;
                lbInvalidValue.Visible = true;
                bOkay.Enabled = false;
            }
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
            lbMessage = new Label();
            tbValue = new TextBox();
            bCancel = new Button();
            bOkay = new Button();
            lbInvalidValue = new Label();
            pbInvalid = new PictureBox();
            ((ISupportInitialize)pbInvalid).BeginInit();
            SuspendLayout();
            lbMessage.AutoSize = true;
            lbMessage.Location = new Point(12, 9);
            lbMessage.Name = "lbMessage";
            lbMessage.Size = new Size(73, 13);
            lbMessage.TabIndex = 0;
            lbMessage.Text = "Enter a value:";
            tbValue.Location = new Point(12, 25);
            tbValue.Name = "tbValue";
            tbValue.Size = new Size(360, 20);
            tbValue.TabIndex = 1;
            tbValue.TextChanged += new EventHandler(tbValue_TextChanged);
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Location = new Point(297, 51);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(75, 23);
            bCancel.TabIndex = 2;
            bCancel.Text = "Cancel";
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Click += new EventHandler(bCancel_Click);
            bOkay.Enabled = false;
            bOkay.Location = new Point(216, 51);
            bOkay.Name = "bOkay";
            bOkay.Size = new Size(75, 23);
            bOkay.TabIndex = 3;
            bOkay.Text = "OK";
            bOkay.UseVisualStyleBackColor = true;
            bOkay.Click += new EventHandler(bOkay_Click);
            lbInvalidValue.AutoSize = true;
            lbInvalidValue.Location = new Point(34, 56);
            lbInvalidValue.Margin = new Padding(0);
            lbInvalidValue.Name = "lbInvalidValue";
            lbInvalidValue.Size = new Size(140, 13);
            lbInvalidValue.TabIndex = 4;
            lbInvalidValue.Text = "The entered value is invalid.";
            lbInvalidValue.Visible = false;
            pbInvalid.Image = Properties.Resources.bRemoveAction_Image;
            pbInvalid.InitialImage = null;
            pbInvalid.Location = new Point(15, 54);
            pbInvalid.Name = "pbInvalid";
            pbInvalid.Size = new Size(16, 16);
            pbInvalid.TabIndex = 5;
            pbInvalid.TabStop = false;
            pbInvalid.Visible = false;
            AcceptButton = bOkay;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = bCancel;
            ClientSize = new Size(384, 86);
            Controls.Add(pbInvalid);
            Controls.Add(lbInvalidValue);
            Controls.Add(bOkay);
            Controls.Add(bCancel);
            Controls.Add(tbValue);
            Controls.Add(lbMessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputDialogue";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Input";
            Load += new EventHandler(InputDialog_Load);
            ((ISupportInitialize)pbInvalid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public enum InputType
        {
            String,
            Int,
            Float,
            Identifier,
        }
    }
}
