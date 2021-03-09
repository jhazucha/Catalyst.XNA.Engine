namespace LevelEditor2D.Forms
{
  partial class frmOptions
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
			this.btnSave = new System.Windows.Forms.Button();
			this.tbProjectPath = new System.Windows.Forms.TextBox();
			this.tbContentPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnProjectBrowse = new System.Windows.Forms.Button();
			this.btnContentBrowse = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cbRenderSurfaceColor = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbResolution = new System.Windows.Forms.ComboBox();
			this.btnGameObjects = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.tbGameObjects = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(641, 201);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(107, 23);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "Save Options";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// tbProjectPath
			// 
			this.tbProjectPath.Location = new System.Drawing.Point(116, 22);
			this.tbProjectPath.Name = "tbProjectPath";
			this.tbProjectPath.Size = new System.Drawing.Size(555, 20);
			this.tbProjectPath.TabIndex = 1;
			// 
			// tbContentPath
			// 
			this.tbContentPath.Location = new System.Drawing.Point(116, 49);
			this.tbContentPath.Name = "tbContentPath";
			this.tbContentPath.Size = new System.Drawing.Size(555, 20);
			this.tbContentPath.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Project Path :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(98, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Content Path :";
			// 
			// btnProjectBrowse
			// 
			this.btnProjectBrowse.Location = new System.Drawing.Point(686, 20);
			this.btnProjectBrowse.Name = "btnProjectBrowse";
			this.btnProjectBrowse.Size = new System.Drawing.Size(62, 23);
			this.btnProjectBrowse.TabIndex = 5;
			this.btnProjectBrowse.Text = "Browse";
			this.btnProjectBrowse.UseVisualStyleBackColor = true;
			this.btnProjectBrowse.Click += new System.EventHandler(this.btnProjectBrowse_Click);
			// 
			// btnContentBrowse
			// 
			this.btnContentBrowse.Location = new System.Drawing.Point(686, 46);
			this.btnContentBrowse.Name = "btnContentBrowse";
			this.btnContentBrowse.Size = new System.Drawing.Size(62, 23);
			this.btnContentBrowse.TabIndex = 6;
			this.btnContentBrowse.Text = "Browse";
			this.btnContentBrowse.UseVisualStyleBackColor = true;
			this.btnContentBrowse.Click += new System.EventHandler(this.btnContentBrowse_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(16, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(153, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Render Surface Color :";
			// 
			// cbRenderSurfaceColor
			// 
			this.cbRenderSurfaceColor.FormattingEnabled = true;
			this.cbRenderSurfaceColor.Items.AddRange(new object[] {
            "Black",
            "CornFlowerBlue",
            "Dark Gray",
            "White"});
			this.cbRenderSurfaceColor.Location = new System.Drawing.Point(175, 124);
			this.cbRenderSurfaceColor.Name = "cbRenderSurfaceColor";
			this.cbRenderSurfaceColor.Size = new System.Drawing.Size(175, 21);
			this.cbRenderSurfaceColor.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(42, 155);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(127, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Target Resolution :";
			// 
			// cbResolution
			// 
			this.cbResolution.FormattingEnabled = true;
			this.cbResolution.Items.AddRange(new object[] {
            "1920x1080",
            "1280x720",
            "800x480",
            "480x800"});
			this.cbResolution.Location = new System.Drawing.Point(175, 153);
			this.cbResolution.Name = "cbResolution";
			this.cbResolution.Size = new System.Drawing.Size(175, 21);
			this.cbResolution.TabIndex = 10;
			// 
			// btnGameObjects
			// 
			this.btnGameObjects.Location = new System.Drawing.Point(686, 82);
			this.btnGameObjects.Name = "btnGameObjects";
			this.btnGameObjects.Size = new System.Drawing.Size(62, 23);
			this.btnGameObjects.TabIndex = 13;
			this.btnGameObjects.Text = "Browse";
			this.btnGameObjects.UseVisualStyleBackColor = true;
			this.btnGameObjects.Click += new System.EventHandler(this.btnGameObjects_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(12, 86);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(134, 16);
			this.label5.TabIndex = 12;
			this.label5.Text = "Game Objects DLL :";
			// 
			// tbGameObjects
			// 
			this.tbGameObjects.Location = new System.Drawing.Point(152, 84);
			this.tbGameObjects.Name = "tbGameObjects";
			this.tbGameObjects.Size = new System.Drawing.Size(519, 20);
			this.tbGameObjects.TabIndex = 11;
			// 
			// frmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 236);
			this.Controls.Add(this.btnGameObjects);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tbGameObjects);
			this.Controls.Add(this.cbResolution);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbRenderSurfaceColor);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnContentBrowse);
			this.Controls.Add(this.btnProjectBrowse);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbContentPath);
			this.Controls.Add(this.tbProjectPath);
			this.Controls.Add(this.btnSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Options";
			this.ResumeLayout(false);
			this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.TextBox tbProjectPath;
    private System.Windows.Forms.TextBox tbContentPath;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnProjectBrowse;
    private System.Windows.Forms.Button btnContentBrowse;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox cbRenderSurfaceColor;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox cbResolution;
		private System.Windows.Forms.Button btnGameObjects;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbGameObjects;
  }
}