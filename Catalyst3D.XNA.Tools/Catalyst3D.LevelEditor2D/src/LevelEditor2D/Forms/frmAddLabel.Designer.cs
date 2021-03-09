namespace LevelEditor2D.Forms
{
	partial class frmAddLabel
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbFontSize = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbFontType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbFontSize);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cbFontType);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.tbText);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(425, 168);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// cbFontSize
			// 
			this.cbFontSize.FormattingEnabled = true;
			this.cbFontSize.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "18"});
			this.cbFontSize.Location = new System.Drawing.Point(92, 46);
			this.cbFontSize.Name = "cbFontSize";
			this.cbFontSize.Size = new System.Drawing.Size(63, 21);
			this.cbFontSize.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(18, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 14);
			this.label3.TabIndex = 5;
			this.label3.Text = "Font Size : ";
			// 
			// cbFontType
			// 
			this.cbFontType.FormattingEnabled = true;
			this.cbFontType.Items.AddRange(new object[] {
            "Arial",
            "Tahoma",
            "Georgia"});
			this.cbFontType.Location = new System.Drawing.Point(92, 19);
			this.cbFontType.Name = "cbFontType";
			this.cbFontType.Size = new System.Drawing.Size(152, 21);
			this.cbFontType.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(13, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 14);
			this.label2.TabIndex = 3;
			this.label2.Text = "Font Type : ";
			// 
			// tbText
			// 
			this.tbText.Location = new System.Drawing.Point(92, 73);
			this.tbText.Name = "tbText";
			this.tbText.Size = new System.Drawing.Size(307, 20);
			this.tbText.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(47, 75);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 14);
			this.label1.TabIndex = 1;
			this.label1.Text = "Text : ";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(331, 130);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(83, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add Label";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// frmAddLabel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(449, 193);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmAddLabel";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Label";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbFontType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbFontSize;

	}
}