namespace LevelEditor2D.Controls
{
	partial class conObjectTypeDropDown
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lsTypes = new System.Windows.Forms.ListBox();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lsTypes);
			this.groupBox1.Controls.Add(this.btnSubmit);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(351, 252);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Available Object Types";
			// 
			// lsTypes
			// 
			this.lsTypes.FormattingEnabled = true;
			this.lsTypes.Location = new System.Drawing.Point(8, 18);
			this.lsTypes.Name = "lsTypes";
			this.lsTypes.ScrollAlwaysVisible = true;
			this.lsTypes.Size = new System.Drawing.Size(327, 199);
			this.lsTypes.TabIndex = 1;
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(260, 223);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(75, 23);
			this.btnSubmit.TabIndex = 0;
			this.btnSubmit.Text = "Select";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// conObjectTypeDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "conObjectTypeDropDown";
			this.Size = new System.Drawing.Size(351, 252);
			this.Leave += new System.EventHandler(this.conObjectTypeDropDown_Leave);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox lsTypes;
		private System.Windows.Forms.Button btnSubmit;
	}
}
