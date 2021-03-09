using LevelEditor3D.Controls;

namespace LevelEditor3D.Forms
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newProjectgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.conRenderer = new conRenderer();
			this.dockManager = new Telerik.WinControls.Docking.DockingManager();
			this.flToolPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
			this.dockManager.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1004, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectgToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newProjectgToolStripMenuItem
			// 
			this.newProjectgToolStripMenuItem.Name = "newProjectgToolStripMenuItem";
			this.newProjectgToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.newProjectgToolStripMenuItem.Text = "&New Project";
			this.newProjectgToolStripMenuItem.Click += new System.EventHandler(this.newProjectgToolStripMenuItem_Click);
			// 
			// loadProjectToolStripMenuItem
			// 
			this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
			this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.loadProjectToolStripMenuItem.Text = "&Load Project";
			this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.loadProjectToolStripMenuItem_Click);
			// 
			// saveProjectToolStripMenuItem
			// 
			this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
			this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.saveProjectToolStripMenuItem.Text = "&Save Project";
			this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPreferences,
            this.pluginsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// tsPreferences
			// 
			this.tsPreferences.Name = "tsPreferences";
			this.tsPreferences.Size = new System.Drawing.Size(135, 22);
			this.tsPreferences.Text = "&Preferences";
			this.tsPreferences.Click += new System.EventHandler(this.tsPreferences_Click);
			// 
			// pluginsToolStripMenuItem
			// 
			this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
			this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.pluginsToolStripMenuItem.Text = "&Plugins";
			this.pluginsToolStripMenuItem.Click += new System.EventHandler(this.pluginsToolStripMenuItem_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.Location = new System.Drawing.Point(0, 24);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(1004, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 700);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1004, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(38, 49);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.conRenderer);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.dockManager);
			this.splitContainer1.Panel2MinSize = 175;
			this.splitContainer1.Size = new System.Drawing.Size(966, 651);
			this.splitContainer1.SplitterDistance = 787;
			this.splitContainer1.TabIndex = 6;
			// 
			// conRenderer
			// 
			this.conRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conRenderer.IsMouseDown = false;
			this.conRenderer.IsRendering = true;
			this.conRenderer.Location = new System.Drawing.Point(0, 0);
			this.conRenderer.Name = "conRenderer";
			this.conRenderer.Size = new System.Drawing.Size(787, 651);
			this.conRenderer.TabIndex = 0;
			// 
			// dockManager
			// 
			this.dockManager.ActiveDocument = null;
			this.dockManager.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dockManager.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockManager.LayoutTree = resources.GetString("dockManager.LayoutTree");
			this.dockManager.Location = new System.Drawing.Point(0, 0);
			this.dockManager.Name = "dockManager";
			// 
			// dockManager.PrimarySite
			// 
			this.dockManager.PrimarySiteComponent.ActiveDocument = null;
			this.dockManager.PrimarySiteComponent.Location = new System.Drawing.Point(0, 0);
			this.dockManager.PrimarySiteComponent.Name = "PrimarySite";
			this.dockManager.PrimarySiteComponent.PresenterMinSize = 25;
			this.dockManager.PrimarySiteComponent.TabIndex = 0;
			this.dockManager.Size = new System.Drawing.Size(175, 651);
			this.dockManager.SizeWeight = 0.5D;
			this.dockManager.TabIndex = 2;
			this.dockManager.Text = "dockingManager1";
			// 
			// flToolPanel
			// 
			this.flToolPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.flToolPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.flToolPanel.Location = new System.Drawing.Point(0, 49);
			this.flToolPanel.Margin = new System.Windows.Forms.Padding(0);
			this.flToolPanel.Name = "flToolPanel";
			this.flToolPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.flToolPanel.Size = new System.Drawing.Size(38, 651);
			this.flToolPanel.TabIndex = 5;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1004, 722);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.flToolPanel);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Catalyst3D - Level Editor";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
			this.dockManager.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsPreferences;
		private System.Windows.Forms.ToolStripMenuItem newProjectgToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    public conRenderer conRenderer;
    private Telerik.WinControls.Docking.DockingManager dockManager;
    private System.Windows.Forms.FlowLayoutPanel flToolPanel;
	}
}