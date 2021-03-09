using System.Xml.Serialization;

namespace LevelEditor2D.Controls
{
  partial class conSpriteContainer
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
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsAdd = new System.Windows.Forms.ToolStripButton();
      this.tsDelete = new System.Windows.Forms.ToolStripButton();
      this.pnContainer = new System.Windows.Forms.FlowLayoutPanel();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                                this.tsAdd,
                                                                                this.tsDelete});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(260, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsAdd
      // 
			this.tsAdd.Image = Resource1.add_page;
      this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAdd.Name = "tsAdd";
      this.tsAdd.Size = new System.Drawing.Size(82, 22);
      this.tsAdd.Text = "Add Sprite";
      this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
      // 
      // tsDelete
      // 
			this.tsDelete.Image = Resource1.delete_page;
      this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsDelete.Name = "tsDelete";
      this.tsDelete.Size = new System.Drawing.Size(93, 22);
      this.tsDelete.Text = "Delete Sprite";
      this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
      // 
      // pnContainer
      // 
      this.pnContainer.AutoScroll = true;
      this.pnContainer.BackColor = System.Drawing.Color.DimGray;
      this.pnContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnContainer.Location = new System.Drawing.Point(0, 25);
      this.pnContainer.Name = "pnContainer";
      this.pnContainer.Size = new System.Drawing.Size(260, 275);
      this.pnContainer.TabIndex = 1;
      // 
      // conSpriteContainer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add(this.pnContainer);
      this.Controls.Add(this.toolStrip1);
      this.Margin = new System.Windows.Forms.Padding(1);
      this.Name = "conSpriteContainer";
      this.Size = new System.Drawing.Size(260, 300);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    [XmlIgnore]
    private System.Windows.Forms.ToolStrip toolStrip1;

    [XmlIgnore]
    private System.Windows.Forms.ToolStripButton tsAdd;

    [XmlIgnore]
    public System.Windows.Forms.FlowLayoutPanel pnContainer;
    private System.Windows.Forms.ToolStripButton tsDelete;

  }
}