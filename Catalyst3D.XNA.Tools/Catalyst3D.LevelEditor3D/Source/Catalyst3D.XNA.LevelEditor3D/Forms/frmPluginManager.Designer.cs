namespace LevelEditor3D.Forms
{
	partial class frmPluginManager
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPluginManager));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsAddPlugin = new System.Windows.Forms.ToolStripButton();
      this.rgPlugins = new Telerik.WinControls.UI.RadGridView();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.rgPlugins)).BeginInit();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddPlugin});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1113, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsAddPlugin
      // 
      this.tsAddPlugin.Image = ((System.Drawing.Image)(resources.GetObject("tsAddPlugin.Image")));
      this.tsAddPlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddPlugin.Name = "tsAddPlugin";
      this.tsAddPlugin.Size = new System.Drawing.Size(86, 22);
      this.tsAddPlugin.Text = "Add Plugin";
      this.tsAddPlugin.Click += new System.EventHandler(this.tsAddPlugin_Click);
      // 
      // rgPlugins
      // 
      this.rgPlugins.AutoSizeRows = true;
      this.rgPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rgPlugins.EnableAlternatingRowColor = true;
      this.rgPlugins.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rgPlugins.Location = new System.Drawing.Point(0, 25);
      // 
      // 
      // 
      this.rgPlugins.MasterGridViewTemplate.AllowAddNewRow = false;
      this.rgPlugins.MasterGridViewTemplate.AllowColumnReorder = false;
      this.rgPlugins.MasterGridViewTemplate.EnableGrouping = false;
      this.rgPlugins.Name = "rgPlugins";
      this.rgPlugins.NewRowEnterKeyMode = Telerik.WinControls.UI.RadGridViewNewRowEnterKeyMode.None;
      this.rgPlugins.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
      this.rgPlugins.Size = new System.Drawing.Size(1113, 270);
      this.rgPlugins.TabIndex = 3;
      this.rgPlugins.ThemeName = "Vista";
      this.rgPlugins.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.rgPlugins_DataBindingComplete);
      // 
      // frmPluginManager
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1113, 295);
      this.Controls.Add(this.rgPlugins);
      this.Controls.Add(this.toolStrip1);
      this.Name = "frmPluginManager";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Plugin Manager";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.rgPlugins)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsAddPlugin;
		private Telerik.WinControls.UI.RadGridView rgPlugins;

	}
}