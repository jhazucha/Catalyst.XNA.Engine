namespace LevelEditor2D.Forms
{
	partial class frmGroupObjects
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
			if(disposing && (components != null))
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
			this.rlSceneObjects = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbName = new System.Windows.Forms.TextBox();
			this.radLabel1 = new System.Windows.Forms.Label();
			this.btnGroup = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rlSceneObjects
			// 
			this.rlSceneObjects.Location = new System.Drawing.Point(12, 12);
			this.rlSceneObjects.Name = "rlSceneObjects";
			this.rlSceneObjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.rlSceneObjects.Size = new System.Drawing.Size(168, 355);
			this.rlSceneObjects.TabIndex = 8;
			this.rlSceneObjects.SelectedIndexChanged += new System.EventHandler(this.rlSceneObjects_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbName);
			this.groupBox1.Controls.Add(this.radLabel1);
			this.groupBox1.Controls.Add(this.btnGroup);
			this.groupBox1.Location = new System.Drawing.Point(186, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(263, 101);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(103, 25);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(139, 20);
			this.tbName.TabIndex = 10;
			// 
			// radLabel1
			// 
			this.radLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel1.Location = new System.Drawing.Point(17, 26);
			this.radLabel1.Name = "radLabel1";
			this.radLabel1.Size = new System.Drawing.Size(78, 14);
			this.radLabel1.TabIndex = 9;
			this.radLabel1.Text = "Group Name :";
			// 
			// btnGroup
			// 
			this.btnGroup.Location = new System.Drawing.Point(134, 63);
			this.btnGroup.Name = "btnGroup";
			this.btnGroup.Size = new System.Drawing.Size(108, 23);
			this.btnGroup.TabIndex = 8;
			this.btnGroup.Text = "Group Selected";
			this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
			// 
			// frmGroupObjects
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(461, 374);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.rlSceneObjects);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmGroupObjects";
			this.Text = "Group Scene Objects";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox rlSceneObjects;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label radLabel1;
		private System.Windows.Forms.Button btnGroup;

	}
}