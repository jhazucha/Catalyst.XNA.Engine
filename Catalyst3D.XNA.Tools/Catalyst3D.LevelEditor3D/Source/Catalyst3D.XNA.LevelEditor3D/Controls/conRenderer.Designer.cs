namespace LevelEditor3D.Controls
{
	partial class conRenderer
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
			this.tsTools = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.tsViewportColor = new System.Windows.Forms.ToolStripTextBox();
			this.tsColor = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tsCameraLock = new System.Windows.Forms.ToolStripComboBox();
			this.RenderWindow = new System.Windows.Forms.Panel();
			this.rColorDialog = new Telerik.WinControls.RadColorDialog();
			this.tsTools.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsTools
			// 
			this.tsTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tsViewportColor,
            this.tsColor,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tsCameraLock});
			this.tsTools.Location = new System.Drawing.Point(0, 0);
			this.tsTools.Name = "tsTools";
			this.tsTools.Size = new System.Drawing.Size(479, 25);
			this.tsTools.Stretch = true;
			this.tsTools.TabIndex = 0;
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(92, 22);
			this.toolStripLabel2.Text = "Viewport Color :";
			// 
			// tsViewportColor
			// 
			this.tsViewportColor.BackColor = System.Drawing.Color.DimGray;
			this.tsViewportColor.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold);
			this.tsViewportColor.ForeColor = System.Drawing.Color.White;
			this.tsViewportColor.Name = "tsViewportColor";
			this.tsViewportColor.Size = new System.Drawing.Size(100, 25);
			this.tsViewportColor.TextChanged += new System.EventHandler(this.tsViewportColor_TextChanged);
			// 
			// tsColor
			// 
			this.tsColor.Image = global::LevelEditor3D.Resource1.colorwheel;
			this.tsColor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsColor.Name = "tsColor";
			this.tsColor.Size = new System.Drawing.Size(23, 22);
			this.tsColor.Click += new System.EventHandler(this.tsColor_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(99, 22);
			this.toolStripLabel1.Text = "Cam Movement :";
			// 
			// tsCameraLock
			// 
			this.tsCameraLock.Items.AddRange(new object[] {
            "Locked",
            "UnLocked"});
			this.tsCameraLock.Name = "tsCameraLock";
			this.tsCameraLock.Size = new System.Drawing.Size(121, 25);
			this.tsCameraLock.Text = "Locked";
			this.tsCameraLock.SelectedIndexChanged += new System.EventHandler(this.tsCameraLock_SelectedIndexChanged);
			// 
			// RenderWindow
			// 
			this.RenderWindow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RenderWindow.Location = new System.Drawing.Point(0, 25);
			this.RenderWindow.Name = "RenderWindow";
			this.RenderWindow.Size = new System.Drawing.Size(479, 130);
			this.RenderWindow.TabIndex = 1;
			// 
			// rColorDialog
			// 
			this.rColorDialog.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(149)))), ((int)(((byte)(237)))));
			this.rColorDialog.SelectedHslColor = Telerik.WinControls.HslColor.FromAhsl(0.60705596107055959D, 0.57805907172995785D, 0.92941176470588238D);
			// 
			// conRenderer
			// 
			this.Controls.Add(this.RenderWindow);
			this.Controls.Add(this.tsTools);
			this.Name = "conRenderer";
			this.Size = new System.Drawing.Size(479, 155);
			this.tsTools.ResumeLayout(false);
			this.tsTools.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

    private System.Windows.Forms.ToolStrip tsTools;
    public System.Windows.Forms.Panel RenderWindow;
		private Telerik.WinControls.RadColorDialog rColorDialog;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripTextBox tsViewportColor;
		private System.Windows.Forms.ToolStripButton tsColor;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripComboBox tsCameraLock;
	}
}
