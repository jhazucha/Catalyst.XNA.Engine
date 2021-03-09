using System.Xml.Serialization;

namespace LevelEditor2D.Controls
{
	partial class conKeyFrameContainer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		[XmlIgnore]
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conKeyFrameContainer));
			this.tsKeyFrame = new System.Windows.Forms.ToolStrip();
			this.tsAddFrame = new System.Windows.Forms.ToolStripButton();
			this.tsAddInFront = new System.Windows.Forms.ToolStripButton();
			this.tsRemove = new System.Windows.Forms.ToolStripButton();
			this.tsDuplicate = new System.Windows.Forms.ToolStripButton();
			this.tsLockFrame = new System.Windows.Forms.ToolStripButton();
			this.tsUnlockFrame = new System.Windows.Forms.ToolStripButton();
			this.tbFrameSpeed = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsLevelSequencer = new System.Windows.Forms.ToolStripButton();
			this.pnKeyFrameContainer = new System.Windows.Forms.FlowLayoutPanel();
			this.tsKeyFrame.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsKeyFrame
			// 
			this.tsKeyFrame.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddFrame,
            this.tsAddInFront,
            this.tsRemove,
            this.tsDuplicate,
            this.tsLockFrame,
            this.tsUnlockFrame,
            this.tbFrameSpeed,
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.tsLevelSequencer});
			this.tsKeyFrame.Location = new System.Drawing.Point(0, 0);
			this.tsKeyFrame.Name = "tsKeyFrame";
			this.tsKeyFrame.Size = new System.Drawing.Size(843, 25);
			this.tsKeyFrame.TabIndex = 1;
			// 
			// tsAddFrame
			// 
			this.tsAddFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsAddFrame.Name = "tsAddFrame";
			this.tsAddFrame.Size = new System.Drawing.Size(69, 22);
			this.tsAddFrame.Text = "Add Frame";
			this.tsAddFrame.Click += new System.EventHandler(this.tsAddFrame_Click);
			// 
			// tsAddInFront
			// 
			this.tsAddInFront.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsAddInFront.Name = "tsAddInFront";
			this.tsAddInFront.Size = new System.Drawing.Size(113, 22);
			this.tsAddInFront.Text = "Add Frame In Front";
			this.tsAddInFront.Click += new System.EventHandler(this.tsAddInFront_Click);
			// 
			// tsRemove
			// 
			this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsRemove.Name = "tsRemove";
			this.tsRemove.Size = new System.Drawing.Size(90, 22);
			this.tsRemove.Text = "Remove Frame";
			this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
			// 
			// tsDuplicate
			// 
			this.tsDuplicate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsDuplicate.Name = "tsDuplicate";
			this.tsDuplicate.Size = new System.Drawing.Size(97, 22);
			this.tsDuplicate.Text = "Duplicate Frame";
			this.tsDuplicate.Click += new System.EventHandler(this.tsDuplicate_Click);
			// 
			// tsLockFrame
			// 
			this.tsLockFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsLockFrame.Name = "tsLockFrame";
			this.tsLockFrame.Size = new System.Drawing.Size(72, 22);
			this.tsLockFrame.Text = "Lock Frame";
			this.tsLockFrame.Click += new System.EventHandler(this.tsLockFrame_Click);
			// 
			// tsUnlockFrame
			// 
			this.tsUnlockFrame.Image = global::LevelEditor2D.Resource1.unlock;
			this.tsUnlockFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsUnlockFrame.Name = "tsUnlockFrame";
			this.tsUnlockFrame.Size = new System.Drawing.Size(100, 22);
			this.tsUnlockFrame.Text = "Unlock Frame";
			this.tsUnlockFrame.Click += new System.EventHandler(this.tsUnlockFrame_Click);
			// 
			// tbFrameSpeed
			// 
			this.tbFrameSpeed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tbFrameSpeed.BackColor = System.Drawing.Color.White;
			this.tbFrameSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbFrameSpeed.MaxLength = 4;
			this.tbFrameSpeed.Name = "tbFrameSpeed";
			this.tbFrameSpeed.Size = new System.Drawing.Size(32, 25);
			this.tbFrameSpeed.Text = "1";
			this.tbFrameSpeed.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbFrameSpeed.ToolTipText = "Speed of Selected Frame";
			this.tbFrameSpeed.TextChanged += new System.EventHandler(this.tbFrameSpeed_TextChanged);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(80, 22);
			this.toolStripLabel1.Text = "Frame Speed";
			this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsLevelSequencer
			// 
			this.tsLevelSequencer.Image = ((System.Drawing.Image)(resources.GetObject("tsLevelSequencer.Image")));
			this.tsLevelSequencer.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsLevelSequencer.Name = "tsLevelSequencer";
			this.tsLevelSequencer.Size = new System.Drawing.Size(118, 22);
			this.tsLevelSequencer.Text = "Frame Sequencer";
			this.tsLevelSequencer.Click += new System.EventHandler(this.tsLevelSequencer_Click);
			// 
			// pnKeyFrameContainer
			// 
			this.pnKeyFrameContainer.BackColor = System.Drawing.Color.DimGray;
			this.pnKeyFrameContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnKeyFrameContainer.Location = new System.Drawing.Point(0, 25);
			this.pnKeyFrameContainer.Margin = new System.Windows.Forms.Padding(1);
			this.pnKeyFrameContainer.Name = "pnKeyFrameContainer";
			this.pnKeyFrameContainer.Padding = new System.Windows.Forms.Padding(5, 10, 0, 10);
			this.pnKeyFrameContainer.Size = new System.Drawing.Size(843, 55);
			this.pnKeyFrameContainer.TabIndex = 2;
			// 
			// conKeyFrameContainer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnKeyFrameContainer);
			this.Controls.Add(this.tsKeyFrame);
			this.Name = "conKeyFrameContainer";
			this.Size = new System.Drawing.Size(843, 80);
			this.tsKeyFrame.ResumeLayout(false);
			this.tsKeyFrame.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsKeyFrame;
		private System.Windows.Forms.ToolStripButton tsLockFrame;
		private System.Windows.Forms.ToolStripButton tsAddFrame;
		private System.Windows.Forms.ToolStripButton tsUnlockFrame;
		
		[XmlIgnore]
		public System.Windows.Forms.FlowLayoutPanel pnKeyFrameContainer;
		private System.Windows.Forms.ToolStripButton tsRemove;
		private System.Windows.Forms.ToolStripButton tsDuplicate;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox tbFrameSpeed;
		private System.Windows.Forms.ToolStripButton tsAddInFront;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsLevelSequencer;


	}
}
