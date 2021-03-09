namespace LevelEditor2D.Controls
{
  partial class conKeyFrameEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conKeyFrameEditor));
      this.btnSave = new System.Windows.Forms.Button();
      this.tbDuration = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.pbFramePic = new System.Windows.Forms.PictureBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.lsFrames = new System.Windows.Forms.ListBox();
      this.tsRemove = new System.Windows.Forms.ToolStripButton();
      ((System.ComponentModel.ISupportInitialize)(this.pbFramePic)).BeginInit();
      this.panel1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(450, 388);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(108, 27);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Save Changes";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSubmit_Click);
      // 
      // tbDuration
      // 
      this.tbDuration.Location = new System.Drawing.Point(370, 317);
      this.tbDuration.Name = "tbDuration";
      this.tbDuration.Size = new System.Drawing.Size(45, 20);
      this.tbDuration.TabIndex = 2;
      this.tbDuration.TextChanged += new System.EventHandler(this.tbDuration_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(267, 320);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(97, 14);
      this.label1.TabIndex = 3;
      this.label1.Text = "Frame Duration :";
      // 
      // pbFramePic
      // 
      this.pbFramePic.Location = new System.Drawing.Point(246, 8);
      this.pbFramePic.Name = "pbFramePic";
      this.pbFramePic.Size = new System.Drawing.Size(312, 295);
      this.pbFramePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pbFramePic.TabIndex = 4;
      this.pbFramePic.TabStop = false;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lsFrames);
      this.panel1.Controls.Add(this.toolStrip1);
      this.panel1.Location = new System.Drawing.Point(8, 8);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(228, 407);
      this.panel1.TabIndex = 5;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRemove});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(228, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // lsFrames
      // 
      this.lsFrames.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lsFrames.FormattingEnabled = true;
      this.lsFrames.Location = new System.Drawing.Point(0, 25);
      this.lsFrames.Name = "lsFrames";
      this.lsFrames.Size = new System.Drawing.Size(228, 381);
      this.lsFrames.TabIndex = 1;
      this.lsFrames.SelectedIndexChanged += new System.EventHandler(this.lsFrames_SelectedIndexChanged);
      // 
      // tsRemove
      // 
      this.tsRemove.Image = ((System.Drawing.Image)(resources.GetObject("tsRemove.Image")));
      this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemove.Name = "tsRemove";
      this.tsRemove.Size = new System.Drawing.Size(106, 22);
      this.tsRemove.Text = "Remove Frame";
      this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
      // 
      // conKeyFrameEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.pbFramePic);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbDuration);
      this.Controls.Add(this.btnSave);
      this.Name = "conKeyFrameEditor";
      this.Size = new System.Drawing.Size(570, 425);
      ((System.ComponentModel.ISupportInitialize)(this.pbFramePic)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.TextBox tbDuration;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.PictureBox pbFramePic;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ListBox lsFrames;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsRemove;

  }
}
