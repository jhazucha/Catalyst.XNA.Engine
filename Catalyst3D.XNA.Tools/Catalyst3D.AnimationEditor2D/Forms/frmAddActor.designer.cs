namespace AnimationEditor.Forms
{
  partial class frmAddActor
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
      this.gbPreview = new System.Windows.Forms.GroupBox();
      this.gbSequences = new System.Windows.Forms.GroupBox();
      this.tvSequences = new System.Windows.Forms.TreeView();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.tbName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cbRole = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnSave = new System.Windows.Forms.Button();
      this.pgBar = new System.Windows.Forms.ProgressBar();
      this.tsAddSprite = new System.Windows.Forms.ToolStripButton();
      this.tsRemove = new System.Windows.Forms.ToolStripButton();
      this.tsAddSequence = new System.Windows.Forms.ToolStripButton();
      this.tsRemoveSequence = new System.Windows.Forms.ToolStripButton();
      this.pbPreview = new System.Windows.Forms.PictureBox();
      this.gbPreview.SuspendLayout();
      this.gbSequences.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
      this.SuspendLayout();
      // 
      // gbPreview
      // 
      this.gbPreview.Controls.Add(this.pbPreview);
      this.gbPreview.Location = new System.Drawing.Point(680, 12);
      this.gbPreview.Name = "gbPreview";
      this.gbPreview.Size = new System.Drawing.Size(257, 257);
      this.gbPreview.TabIndex = 2;
      this.gbPreview.TabStop = false;
      this.gbPreview.Text = "Image Preview";
      // 
      // gbSequences
      // 
      this.gbSequences.Controls.Add(this.tvSequences);
      this.gbSequences.Controls.Add(this.toolStrip2);
      this.gbSequences.Location = new System.Drawing.Point(242, 12);
      this.gbSequences.Name = "gbSequences";
      this.gbSequences.Size = new System.Drawing.Size(418, 424);
      this.gbSequences.TabIndex = 4;
      this.gbSequences.TabStop = false;
      this.gbSequences.Text = "Clip Player Sequences";
      // 
      // tvSequences
      // 
      this.tvSequences.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tvSequences.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tvSequences.FullRowSelect = true;
      this.tvSequences.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.tvSequences.Location = new System.Drawing.Point(3, 41);
      this.tvSequences.Name = "tvSequences";
      this.tvSequences.Size = new System.Drawing.Size(412, 380);
      this.tvSequences.TabIndex = 2;
      this.tvSequences.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSequences_AfterSelect);
      // 
      // toolStrip2
      // 
      this.toolStrip2.Font = new System.Drawing.Font("Tahoma", 8F);
      this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddSprite,
            this.tsRemove,
            this.toolStripSeparator1,
            this.tsAddSequence,
            this.tsRemoveSequence});
      this.toolStrip2.Location = new System.Drawing.Point(3, 16);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(412, 25);
      this.toolStrip2.TabIndex = 0;
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.tbName);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.cbRole);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Location = new System.Drawing.Point(12, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(222, 95);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Actor Settings";
      // 
      // tbName
      // 
      this.tbName.Location = new System.Drawing.Point(64, 25);
      this.tbName.Name = "tbName";
      this.tbName.Size = new System.Drawing.Size(145, 20);
      this.tbName.TabIndex = 9;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 27);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(46, 15);
      this.label3.TabIndex = 8;
      this.label3.Text = "Name :";
      // 
      // cbRole
      // 
      this.cbRole.FormattingEnabled = true;
      this.cbRole.Items.AddRange(new object[] {
            "Player",
            "Enemy",
            "NPC"});
      this.cbRole.Location = new System.Drawing.Point(64, 55);
      this.cbRole.Name = "cbRole";
      this.cbRole.Size = new System.Drawing.Size(145, 21);
      this.cbRole.TabIndex = 7;
      this.cbRole.Text = "Player";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(20, 57);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 15);
      this.label2.TabIndex = 6;
      this.label2.Text = "Role :";
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(837, 409);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(100, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Add Actor";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnAddActor_Click);
      // 
      // pgBar
      // 
      this.pgBar.Location = new System.Drawing.Point(683, 275);
      this.pgBar.Name = "pgBar";
      this.pgBar.Size = new System.Drawing.Size(251, 23);
      this.pgBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.pgBar.TabIndex = 7;
      // 
      // tsAddSprite
      // 
      this.tsAddSprite.Image = global::AnimationEditor.Properties.Resources.add;
      this.tsAddSprite.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddSprite.Name = "tsAddSprite";
      this.tsAddSprite.Size = new System.Drawing.Size(79, 22);
      this.tsAddSprite.Text = "Add Frame";
      this.tsAddSprite.Click += new System.EventHandler(this.tsAddSprite_Click);
      // 
      // tsRemove
      // 
      this.tsRemove.Image = global::AnimationEditor.Properties.Resources.cancel;
      this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemove.Name = "tsRemove";
      this.tsRemove.Size = new System.Drawing.Size(99, 22);
      this.tsRemove.Text = "Remove Frame";
      this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
      // 
      // tsAddSequence
      // 
      this.tsAddSequence.Image = global::AnimationEditor.Properties.Resources.cog_add;
      this.tsAddSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddSequence.Name = "tsAddSequence";
      this.tsAddSequence.Size = new System.Drawing.Size(96, 22);
      this.tsAddSequence.Text = "Add Sequence";
      this.tsAddSequence.ToolTipText = "Add Sequence";
      this.tsAddSequence.Click += new System.EventHandler(this.tsAddSequence_Click);
      // 
      // tsRemoveSequence
      // 
      this.tsRemoveSequence.Image = global::AnimationEditor.Properties.Resources.cog_delete;
      this.tsRemoveSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsRemoveSequence.Name = "tsRemoveSequence";
      this.tsRemoveSequence.Size = new System.Drawing.Size(116, 22);
      this.tsRemoveSequence.Text = "Remove Sequence";
      this.tsRemoveSequence.Click += new System.EventHandler(this.tsRemoveSequence_Click);
      // 
      // pbPreview
      // 
      this.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbPreview.Location = new System.Drawing.Point(3, 16);
      this.pbPreview.Name = "pbPreview";
      this.pbPreview.Size = new System.Drawing.Size(251, 238);
      this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pbPreview.TabIndex = 0;
      this.pbPreview.TabStop = false;
      // 
      // frmAddActor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(947, 444);
      this.Controls.Add(this.pgBar);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.gbSequences);
      this.Controls.Add(this.gbPreview);
      this.Name = "frmAddActor";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Actor";
      this.gbPreview.ResumeLayout(false);
      this.gbSequences.ResumeLayout(false);
      this.gbSequences.PerformLayout();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gbPreview;
    private System.Windows.Forms.PictureBox pbPreview;
    private System.Windows.Forms.GroupBox gbSequences;
    private System.Windows.Forms.TreeView tvSequences;
    private System.Windows.Forms.ToolStrip toolStrip2;
    private System.Windows.Forms.ToolStripButton tsAddSprite;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbRole;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbName;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.ToolStripButton tsRemove;
    private System.Windows.Forms.ProgressBar pgBar;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton tsAddSequence;
    private System.Windows.Forms.ToolStripButton tsRemoveSequence;
  }
}