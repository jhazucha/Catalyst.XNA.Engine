﻿namespace AnimationEditor.Forms
{
  partial class frmAddSequence
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.label1 = new System.Windows.Forms.Label();
      this.btnSubmit = new System.Windows.Forms.Button();
      this.tbName = new System.Windows.Forms.TextBox();
      this.eProvider = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.eProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
      this.label1.Location = new System.Drawing.Point(12, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(103, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Sequence Name :";
      // 
      // btnSubmit
      // 
      this.btnSubmit.Location = new System.Drawing.Point(192, 55);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new System.Drawing.Size(75, 23);
      this.btnSubmit.TabIndex = 1;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
      // 
      // tbName
      // 
      this.tbName.Location = new System.Drawing.Point(121, 20);
      this.tbName.Name = "tbName";
      this.tbName.Size = new System.Drawing.Size(146, 20);
      this.tbName.TabIndex = 2;
      // 
      // eProvider
      // 
      this.eProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.eProvider.ContainerControl = this;
      // 
      // frmAddSequence
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(279, 92);
      this.ControlBox = false;
      this.Controls.Add(this.tbName);
      this.Controls.Add(this.btnSubmit);
      this.Controls.Add(this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmAddSequence";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Sequence";
      ((System.ComponentModel.ISupportInitialize)(this.eProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnSubmit;
    private System.Windows.Forms.TextBox tbName;
    private System.Windows.Forms.ErrorProvider eProvider;
  }
}