namespace Plugins.SceneManager
{
	partial class conSceneObjects
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conSceneObjects));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.pgSceneObjects = new System.Windows.Forms.PropertyGrid();
      this.tsRemove = new System.Windows.Forms.ToolStripButton();
      this.tsAdd = new System.Windows.Forms.ToolStripButton();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
			this.Text = "Scene Objects";
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		  this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
		                                   {
                                          this.tsAdd,
		                                     this.tsRemove
		                                   });
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(200, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // pgSceneObjects
      // 
      this.pgSceneObjects.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pgSceneObjects.Location = new System.Drawing.Point(0, 25);
      this.pgSceneObjects.Name = "pgSceneObjects";
      this.pgSceneObjects.Size = new System.Drawing.Size(200, 286);
      this.pgSceneObjects.TabIndex = 1;
      // 
      // tsAdd
      // 
      this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAdd.Name = "tsAdd";
      this.tsAdd.Size = new System.Drawing.Size(23, 22);
      this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
      // 
      // tsRemove
      // 
      this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemove.Name = "tsRemove";
      this.tsRemove.Size = new System.Drawing.Size(23, 22);
      this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
      // 
      // conSceneObjects
      // 
      this.Controls.Add(this.pgSceneObjects);
      this.Controls.Add(this.toolStrip1);
      this.Name = "conSceneObjects";
		  this.Text = "Scene Objects";
      this.Size = new System.Drawing.Size(200, 311);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsAdd;
    private System.Windows.Forms.ToolStripButton tsRemove;
    public System.Windows.Forms.PropertyGrid pgSceneObjects;

  }
}
