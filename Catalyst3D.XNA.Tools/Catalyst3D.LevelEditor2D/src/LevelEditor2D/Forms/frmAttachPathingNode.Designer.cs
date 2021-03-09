namespace LevelEditor2D.Forms
{
	partial class frmAttachPathingNode
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
			this.lsSceneObjects = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lsAvailablePaths = new System.Windows.Forms.ListBox();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.tbNodeIndex = new System.Windows.Forms.TextBox();
			this.radLabel2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lsSceneObjects);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(159, 267);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Scene Objects";
			// 
			// lsSceneObjects
			// 
			this.lsSceneObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsSceneObjects.FormattingEnabled = true;
			this.lsSceneObjects.Location = new System.Drawing.Point(3, 16);
			this.lsSceneObjects.Name = "lsSceneObjects";
			this.lsSceneObjects.Size = new System.Drawing.Size(153, 248);
			this.lsSceneObjects.TabIndex = 0;
			this.lsSceneObjects.SelectedIndexChanged += new System.EventHandler(this.lsSceneObjects_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lsAvailablePaths);
			this.groupBox2.Location = new System.Drawing.Point(185, 13);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(159, 267);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Available Paths";
			// 
			// lsAvailablePaths
			// 
			this.lsAvailablePaths.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsAvailablePaths.FormattingEnabled = true;
			this.lsAvailablePaths.Location = new System.Drawing.Point(3, 16);
			this.lsAvailablePaths.Name = "lsAvailablePaths";
			this.lsAvailablePaths.Size = new System.Drawing.Size(153, 248);
			this.lsAvailablePaths.TabIndex = 0;
			this.lsAvailablePaths.SelectedIndexChanged += new System.EventHandler(this.lsAvailablePaths_SelectedIndexChanged);
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(418, 251);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(148, 26);
			this.btnSubmit.TabIndex = 2;
			this.btnSubmit.Text = "Attach Selected Path";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// tbNodeIndex
			// 
			this.tbNodeIndex.Location = new System.Drawing.Point(515, 34);
			this.tbNodeIndex.Name = "tbNodeIndex";
			this.tbNodeIndex.Size = new System.Drawing.Size(39, 20);
			this.tbNodeIndex.TabIndex = 3;
			this.tbNodeIndex.Text = "0";
			// 
			// radLabel2
			// 
			this.radLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel2.Location = new System.Drawing.Point(362, 37);
			this.radLabel2.Name = "radLabel2";
			this.radLabel2.Size = new System.Drawing.Size(147, 14);
			this.radLabel2.TabIndex = 29;
			this.radLabel2.Text = "Start Path Node Index :";
			// 
			// frmAttachPathingNode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(578, 292);
			this.Controls.Add(this.radLabel2);
			this.Controls.Add(this.tbNodeIndex);
			this.Controls.Add(this.btnSubmit);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmAttachPathingNode";
			this.Text = "Attach Pathing Node";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.ListBox lsSceneObjects;
		private System.Windows.Forms.ListBox lsAvailablePaths;
		private System.Windows.Forms.TextBox tbNodeIndex;
		private System.Windows.Forms.Label radLabel2;
	}
}