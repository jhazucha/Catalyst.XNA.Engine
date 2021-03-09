using AnimationEditor.Controls;

namespace AnimationEditor.Forms
{
  partial class frmMain
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsPlay = new System.Windows.Forms.ToolStripButton();
      this.tsStop = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.tsPreviousFrame = new System.Windows.Forms.ToolStripButton();
      this.tsNextFrame = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.tsSequence = new System.Windows.Forms.ToolStripComboBox();
      this.btnLoop = new System.Windows.Forms.ToolStripButton();
      this.pgGrid = new System.Windows.Forms.PropertyGrid();
      this.conRenderer = new AnimationEditor.Controls.conRenderer();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.lbCurrentFrame = new System.Windows.Forms.ToolStripStatusLabel();
      this.lbFrame = new System.Windows.Forms.ToolStripStatusLabel();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1032, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.toolStripSeparator3,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newProjectToolStripMenuItem
      // 
      this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
      this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.newProjectToolStripMenuItem.Text = "&New Project";
      this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
      // 
      // loadToolStripMenuItem
      // 
      this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
      this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.loadToolStripMenuItem.Text = "&Load Project";
      this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.optionsToolStripMenuItem.Text = "&Options";
      this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPlay,
            this.tsStop,
            this.toolStripSeparator2,
            this.tsPreviousFrame,
            this.tsNextFrame,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tsSequence,
            this.btnLoop});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1032, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsPlay
      // 
      this.tsPlay.Image = global::AnimationEditor.Properties.Resources.control_play_blue;
      this.tsPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsPlay.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
      this.tsPlay.Name = "tsPlay";
      this.tsPlay.Size = new System.Drawing.Size(49, 22);
      this.tsPlay.Text = "Play";
      this.tsPlay.Click += new System.EventHandler(this.tsPlay_Click);
      // 
      // tsStop
      // 
      this.tsStop.Image = global::AnimationEditor.Properties.Resources.control_stop_blue;
      this.tsStop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsStop.Name = "tsStop";
      this.tsStop.Size = new System.Drawing.Size(51, 22);
      this.tsStop.Text = "Stop";
      this.tsStop.Click += new System.EventHandler(this.tsStop_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // tsPreviousFrame
      // 
      this.tsPreviousFrame.Image = global::AnimationEditor.Properties.Resources.control_rewind_blue;
      this.tsPreviousFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsPreviousFrame.Name = "tsPreviousFrame";
      this.tsPreviousFrame.Size = new System.Drawing.Size(108, 22);
      this.tsPreviousFrame.Text = "Previous Frame";
      this.tsPreviousFrame.Click += new System.EventHandler(this.tsPreviousFrame_Click);
      // 
      // tsNextFrame
      // 
      this.tsNextFrame.Image = global::AnimationEditor.Properties.Resources.control_fastforward_blue;
      this.tsNextFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsNextFrame.Name = "tsNextFrame";
      this.tsNextFrame.Size = new System.Drawing.Size(87, 22);
      this.tsNextFrame.Text = "Next Frame";
      this.tsNextFrame.Click += new System.EventHandler(this.tsNextFrame_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(110, 22);
      this.toolStripLabel1.Text = "Current Sequence : ";
      // 
      // tsSequence
      // 
      this.tsSequence.Name = "tsSequence";
      this.tsSequence.Size = new System.Drawing.Size(121, 25);
      this.tsSequence.TextChanged += new System.EventHandler(this.tsSequence_TextChanged);
      // 
      // btnLoop
      // 
      this.btnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnLoop.Image = ((System.Drawing.Image)(resources.GetObject("btnLoop.Image")));
      this.btnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnLoop.Name = "btnLoop";
      this.btnLoop.Size = new System.Drawing.Size(100, 22);
      this.btnLoop.Text = "Looping Enabled";
      this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
      // 
      // pgGrid
      // 
      this.pgGrid.Dock = System.Windows.Forms.DockStyle.Right;
      this.pgGrid.Location = new System.Drawing.Point(804, 49);
      this.pgGrid.Name = "pgGrid";
      this.pgGrid.Size = new System.Drawing.Size(228, 554);
      this.pgGrid.TabIndex = 2;
      // 
      // conRenderer
      // 
      this.conRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conRenderer.Location = new System.Drawing.Point(0, 49);
      this.conRenderer.Name = "conRenderer";
      this.conRenderer.Size = new System.Drawing.Size(804, 554);
      this.conRenderer.TabIndex = 3;
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbCurrentFrame,
            this.lbFrame});
      this.statusStrip1.Location = new System.Drawing.Point(0, 603);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(1032, 22);
      this.statusStrip1.TabIndex = 4;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // lbCurrentFrame
      // 
      this.lbCurrentFrame.Name = "lbCurrentFrame";
      this.lbCurrentFrame.Size = new System.Drawing.Size(89, 17);
      this.lbCurrentFrame.Text = "Current Frame :";
      // 
      // lbFrame
      // 
      this.lbFrame.Name = "lbFrame";
      this.lbFrame.Size = new System.Drawing.Size(13, 17);
      this.lbFrame.Text = "0";
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.saveAsToolStripMenuItem.Text = "S&ave As";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // frmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1032, 625);
      this.Controls.Add(this.conRenderer);
      this.Controls.Add(this.pgGrid);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.statusStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "frmMain";
      this.Text = "Catalyst3D - 2D Character Editor";
      this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsPlay;
    private System.Windows.Forms.ToolStripButton tsStop;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox tsSequence;
		private System.Windows.Forms.ToolStripButton btnLoop;
		private System.Windows.Forms.PropertyGrid pgGrid;
		public conRenderer conRenderer;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lbCurrentFrame;
		private System.Windows.Forms.ToolStripStatusLabel lbFrame;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsPreviousFrame;
		private System.Windows.Forms.ToolStripButton tsNextFrame;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
  }
}