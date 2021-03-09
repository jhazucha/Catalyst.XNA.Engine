namespace LevelEditor2D.Forms
{
  partial class frmSequencer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSequencer));
			this.gb1 = new System.Windows.Forms.GroupBox();
			this.dgSequences = new System.Windows.Forms.DataGridView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsAdd = new System.Windows.Forms.ToolStripButton();
			this.tsRemove = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsSave = new System.Windows.Forms.ToolStripButton();
			this.gb1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgSequences)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gb1
			// 
			this.gb1.Controls.Add(this.dgSequences);
			this.gb1.Location = new System.Drawing.Point(12, 36);
			this.gb1.Name = "gb1";
			this.gb1.Size = new System.Drawing.Size(444, 430);
			this.gb1.TabIndex = 1;
			this.gb1.TabStop = false;
			this.gb1.Text = "Current Sequences";
			// 
			// dgSequences
			// 
			this.dgSequences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgSequences.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgSequences.Location = new System.Drawing.Point(3, 16);
			this.dgSequences.Name = "dgSequences";
			this.dgSequences.Size = new System.Drawing.Size(438, 411);
			this.dgSequences.TabIndex = 2;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.tsRemove,
            this.toolStripSeparator1,
            this.tsSave});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(466, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsAdd
			// 
			this.tsAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsAdd.Image")));
			this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsAdd.Name = "tsAdd";
			this.tsAdd.Size = new System.Drawing.Size(103, 22);
			this.tsAdd.Text = "Add Sequence";
			this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
			// 
			// tsRemove
			// 
			this.tsRemove.Image = ((System.Drawing.Image)(resources.GetObject("tsRemove.Image")));
			this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsRemove.Name = "tsRemove";
			this.tsRemove.Size = new System.Drawing.Size(124, 22);
			this.tsRemove.Text = "Remove Sequence";
			this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsSave
			// 
			this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsSave.Name = "tsSave";
			this.tsSave.Size = new System.Drawing.Size(94, 22);
			this.tsSave.Text = "Save Sequences";
			this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
			// 
			// frmSequencer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(466, 476);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.gb1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmSequencer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Animation Sequencer";
			this.gb1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgSequences)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox gb1;
    public System.Windows.Forms.DataGridView dgSequences;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsAdd;
    private System.Windows.Forms.ToolStripButton tsRemove;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton tsSave;
  }
}