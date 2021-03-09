namespace AnimationEditor.Controls
{
	partial class conSequenceEditor
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conSequenceEditor));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.pbImage = new System.Windows.Forms.PictureBox();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.lsSequences = new System.Windows.Forms.ListBox();
      this.tsMenu = new System.Windows.Forms.ToolStrip();
      this.tsAdd = new System.Windows.Forms.ToolStripButton();
      this.tsRemoveSequence = new System.Windows.Forms.ToolStripButton();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.tbFrameDuration = new System.Windows.Forms.TextBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.lsFrames = new System.Windows.Forms.ListBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsAddFrame = new System.Windows.Forms.ToolStripButton();
      this.tsRemoveFrame = new System.Windows.Forms.ToolStripButton();
      this.btnClose = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tbName = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
      this.groupBox5.SuspendLayout();
      this.tsMenu.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.pbImage);
      this.groupBox1.Controls.Add(this.groupBox5);
      this.groupBox1.Controls.Add(this.groupBox4);
      this.groupBox1.Controls.Add(this.groupBox3);
      this.groupBox1.Controls.Add(this.btnClose);
      this.groupBox1.Controls.Add(this.groupBox2);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(794, 318);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      // 
      // pbImage
      // 
      this.pbImage.BackColor = System.Drawing.Color.Black;
      this.pbImage.Location = new System.Drawing.Point(527, 20);
      this.pbImage.Name = "pbImage";
      this.pbImage.Size = new System.Drawing.Size(256, 256);
      this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pbImage.TabIndex = 12;
      this.pbImage.TabStop = false;
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.lsSequences);
      this.groupBox5.Controls.Add(this.tsMenu);
      this.groupBox5.Location = new System.Drawing.Point(7, 13);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(143, 297);
      this.groupBox5.TabIndex = 11;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Sequences";
      // 
      // lsSequences
      // 
      this.lsSequences.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lsSequences.FormattingEnabled = true;
      this.lsSequences.Location = new System.Drawing.Point(3, 41);
      this.lsSequences.Name = "lsSequences";
      this.lsSequences.Size = new System.Drawing.Size(137, 253);
      this.lsSequences.TabIndex = 3;
      this.lsSequences.SelectedIndexChanged += new System.EventHandler(this.lsSequences_SelectedIndexChanged_1);
      // 
      // tsMenu
      // 
      this.tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.tsRemoveSequence});
      this.tsMenu.Location = new System.Drawing.Point(3, 16);
      this.tsMenu.Name = "tsMenu";
      this.tsMenu.Size = new System.Drawing.Size(137, 25);
      this.tsMenu.TabIndex = 2;
      this.tsMenu.Text = "toolStrip1";
      // 
      // tsAdd
      // 
      this.tsAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsAdd.Image")));
      this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAdd.Name = "tsAdd";
      this.tsAdd.Size = new System.Drawing.Size(49, 22);
      this.tsAdd.Text = "Add";
      this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
      // 
      // tsRemoveSequence
      // 
      this.tsRemoveSequence.Image = ((System.Drawing.Image)(resources.GetObject("tsRemoveSequence.Image")));
      this.tsRemoveSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemoveSequence.Name = "tsRemoveSequence";
      this.tsRemoveSequence.Size = new System.Drawing.Size(70, 22);
      this.tsRemoveSequence.Text = "Remove";
      this.tsRemoveSequence.Click += new System.EventHandler(this.tsRemoveSequence_Click);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.tbFrameDuration);
      this.groupBox4.Location = new System.Drawing.Point(318, 79);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(202, 55);
      this.groupBox4.TabIndex = 10;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Frame Details";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(25, 23);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 14);
      this.label4.TabIndex = 10;
      this.label4.Text = "Duration :";
      // 
      // tbFrameDuration
      // 
      this.tbFrameDuration.Location = new System.Drawing.Point(90, 20);
      this.tbFrameDuration.Name = "tbFrameDuration";
      this.tbFrameDuration.Size = new System.Drawing.Size(44, 20);
      this.tbFrameDuration.TabIndex = 9;
      this.tbFrameDuration.TextChanged += new System.EventHandler(this.tbFrameDuration_TextChanged);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.lsFrames);
      this.groupBox3.Controls.Add(this.toolStrip1);
      this.groupBox3.Location = new System.Drawing.Point(156, 12);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(144, 298);
      this.groupBox3.TabIndex = 9;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Frames";
      // 
      // lsFrames
      // 
      this.lsFrames.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lsFrames.FormattingEnabled = true;
      this.lsFrames.Location = new System.Drawing.Point(3, 41);
      this.lsFrames.Name = "lsFrames";
      this.lsFrames.Size = new System.Drawing.Size(138, 254);
      this.lsFrames.TabIndex = 1;
      this.lsFrames.SelectedIndexChanged += new System.EventHandler(this.lsFrames_SelectedIndexChanged);
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddFrame,
            this.tsRemoveFrame});
      this.toolStrip1.Location = new System.Drawing.Point(3, 16);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(138, 25);
      this.toolStrip1.TabIndex = 0;
      // 
      // tsAddFrame
      // 
      this.tsAddFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsAddFrame.Image")));
      this.tsAddFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddFrame.Name = "tsAddFrame";
      this.tsAddFrame.Size = new System.Drawing.Size(49, 22);
      this.tsAddFrame.Text = "Add";
      this.tsAddFrame.Click += new System.EventHandler(this.tsAddFrame_Click);
      // 
      // tsRemoveFrame
      // 
      this.tsRemoveFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsRemoveFrame.Image")));
      this.tsRemoveFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemoveFrame.Name = "tsRemoveFrame";
      this.tsRemoveFrame.Size = new System.Drawing.Size(70, 22);
      this.tsRemoveFrame.Text = "Remove";
      this.tsRemoveFrame.Click += new System.EventHandler(this.tsRemoveFrame_Click);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(675, 284);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(108, 23);
      this.btnClose.TabIndex = 8;
      this.btnClose.Text = "Save Changes";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.tbName);
      this.groupBox2.Location = new System.Drawing.Point(318, 13);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(202, 60);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Sequence Details";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(40, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 14);
      this.label1.TabIndex = 8;
      this.label1.Text = "Name :";
      // 
      // tbName
      // 
      this.tbName.Location = new System.Drawing.Point(90, 23);
      this.tbName.Name = "tbName";
      this.tbName.Size = new System.Drawing.Size(100, 20);
      this.tbName.TabIndex = 7;
      this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
      // 
      // conSequenceEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "conSequenceEditor";
      this.Size = new System.Drawing.Size(794, 318);
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.tsMenu.ResumeLayout(false);
      this.tsMenu.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsRemoveFrame;
		private System.Windows.Forms.ListBox lsFrames;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbFrameDuration;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ListBox lsSequences;
		private System.Windows.Forms.ToolStrip tsMenu;
		private System.Windows.Forms.ToolStripButton tsRemoveSequence;
		private System.Windows.Forms.ToolStripButton tsAdd;
		private System.Windows.Forms.ToolStripButton tsAddFrame;
    private System.Windows.Forms.PictureBox pbImage;
	}
}
