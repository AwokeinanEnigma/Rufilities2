// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.Dialogs.InputDialog
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

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
    private Dictionary<InputDialog.InputType, string> validationRegex = new Dictionary<InputDialog.InputType, string>()
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
    private IContainer components;
    private Label lbMessage;
    private TextBox tbValue;
    private Button bCancel;
    private Button bOkay;
    private Label lbInvalidValue;
    private PictureBox pbInvalid;

    public string Value => this.value;

    public int IntValue => this.intValue;

    public float FloatValue => this.floatValue;

    public InputDialog(InputDialog.InputType inputType) => this.Initialize(inputType, (string) null, (string) null, (string) null);

    public InputDialog(InputDialog.InputType inputType, string initialValue) => this.Initialize(inputType, (string) null, (string) null, initialValue);

    public InputDialog(InputDialog.InputType inputType, string title, string message) => this.Initialize(inputType, title, message, (string) null);

    private void Initialize(
      InputDialog.InputType inputType,
      string title,
      string message,
      string initialValue)
    {
      this.title = title;
      this.message = message;
      this.initialValue = initialValue;
      string pattern;
      this.validationRegex.TryGetValue(inputType, out pattern);
      this.regex = new Regex(pattern);
      this.InitializeComponent();
    }

    private void InputDialog_Load(object sender, EventArgs e)
    {
      if (this.title != null)
        this.Text = this.title;
      if (this.message != null)
        this.lbMessage.Text = string.Format("{0}:", (object) this.message);
      if (this.initialValue == null)
        return;
      this.tbValue.Text = this.initialValue;
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Dispose();
    }

    private void bOkay_Click(object sender, EventArgs e)
    {
      this.value = this.tbValue.Text;
      int.TryParse(this.value, out this.intValue);
      float.TryParse(this.value, out this.floatValue);
      this.DialogResult = DialogResult.OK;
      this.Dispose();
    }

    private void tbValue_TextChanged(object sender, EventArgs e)
    {
      if (this.regex.Match(this.tbValue.Text).Success)
      {
        this.pbInvalid.Visible = false;
        this.lbInvalidValue.Visible = false;
        this.bOkay.Enabled = true;
      }
      else
      {
        this.pbInvalid.Visible = true;
        this.lbInvalidValue.Visible = true;
        this.bOkay.Enabled = false;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lbMessage = new Label();
      this.tbValue = new TextBox();
      this.bCancel = new Button();
      this.bOkay = new Button();
      this.lbInvalidValue = new Label();
      this.pbInvalid = new PictureBox();
      ((ISupportInitialize) this.pbInvalid).BeginInit();
      this.SuspendLayout();
      this.lbMessage.AutoSize = true;
      this.lbMessage.Location = new Point(12, 9);
      this.lbMessage.Name = "lbMessage";
      this.lbMessage.Size = new Size(73, 13);
      this.lbMessage.TabIndex = 0;
      this.lbMessage.Text = "Enter a value:";
      this.tbValue.Location = new Point(12, 25);
      this.tbValue.Name = "tbValue";
      this.tbValue.Size = new Size(360, 20);
      this.tbValue.TabIndex = 1;
      this.tbValue.TextChanged += new EventHandler(this.tbValue_TextChanged);
      this.bCancel.DialogResult = DialogResult.Cancel;
      this.bCancel.Location = new Point(297, 51);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(75, 23);
      this.bCancel.TabIndex = 2;
      this.bCancel.Text = "Cancel";
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOkay.Enabled = false;
      this.bOkay.Location = new Point(216, 51);
      this.bOkay.Name = "bOkay";
      this.bOkay.Size = new Size(75, 23);
      this.bOkay.TabIndex = 3;
      this.bOkay.Text = "OK";
      this.bOkay.UseVisualStyleBackColor = true;
      this.bOkay.Click += new EventHandler(this.bOkay_Click);
      this.lbInvalidValue.AutoSize = true;
      this.lbInvalidValue.Location = new Point(34, 56);
      this.lbInvalidValue.Margin = new Padding(0);
      this.lbInvalidValue.Name = "lbInvalidValue";
      this.lbInvalidValue.Size = new Size(140, 13);
      this.lbInvalidValue.TabIndex = 4;
      this.lbInvalidValue.Text = "The entered value is invalid.";
      this.lbInvalidValue.Visible = false;
            this.pbInvalid.Image = Properties.Resources.bRemoveAction_Image;
      this.pbInvalid.InitialImage = (Image) null;
      this.pbInvalid.Location = new Point(15, 54);
      this.pbInvalid.Name = "pbInvalid";
      this.pbInvalid.Size = new Size(16, 16);
      this.pbInvalid.TabIndex = 5;
      this.pbInvalid.TabStop = false;
      this.pbInvalid.Visible = false;
      this.AcceptButton = (IButtonControl) this.bOkay;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.bCancel;
      this.ClientSize = new Size(384, 86);
      this.Controls.Add((Control) this.pbInvalid);
      this.Controls.Add((Control) this.lbInvalidValue);
      this.Controls.Add((Control) this.bOkay);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.tbValue);
      this.Controls.Add((Control) this.lbMessage);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "InputDialogue";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Input";
      this.Load += new EventHandler(this.InputDialog_Load);
      ((ISupportInitialize) this.pbInvalid).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
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
