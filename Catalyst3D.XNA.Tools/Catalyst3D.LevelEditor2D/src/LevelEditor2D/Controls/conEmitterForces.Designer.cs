namespace LevelEditor2D.Controls
{
  partial class conEmitterForces
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(conEmitterForces));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.tbY = new System.Windows.Forms.TextBox();
			this.tbX = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lbForces = new System.Windows.Forms.ListBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsRemove = new System.Windows.Forms.ToolStripButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.tbY);
			this.groupBox1.Controls.Add(this.tbX);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(248, 38);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 79);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Propertys";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(119, 43);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 18;
			this.btnAdd.Text = "Add Force";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// tbY
			// 
			this.tbY.Location = new System.Drawing.Point(49, 43);
			this.tbY.Name = "tbY";
			this.tbY.Size = new System.Drawing.Size(40, 20);
			this.tbY.TabIndex = 17;
			this.tbY.Text = "0";
			// 
			// tbX
			// 
			this.tbX.Location = new System.Drawing.Point(49, 19);
			this.tbX.Name = "tbX";
			this.tbX.Size = new System.Drawing.Size(40, 20);
			this.tbX.TabIndex = 16;
			this.tbX.Text = "0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(20, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Y :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(20, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "X :";
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(329, 228);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(112, 23);
			this.btnSubmit.TabIndex = 16;
			this.btnSubmit.Text = "Save Changes";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lbForces);
			this.groupBox2.Controls.Add(this.toolStrip1);
			this.groupBox2.Location = new System.Drawing.Point(13, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(226, 235);
			this.groupBox2.TabIndex = 19;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Forces";
			// 
			// lbForces
			// 
			this.lbForces.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbForces.Location = new System.Drawing.Point(3, 41);
			this.lbForces.Name = "lbForces";
			this.lbForces.Size = new System.Drawing.Size(220, 191);
			this.lbForces.TabIndex = 20;
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRemove});
			this.toolStrip1.Location = new System.Drawing.Point(3, 16);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(220, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsRemove
			// 
			this.tsRemove.Image = ((System.Drawing.Image)(resources.GetObject("tsRemove.Image")));
			this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsRemove.Name = "tsRemove";
			this.tsRemove.Size = new System.Drawing.Size(70, 22);
			this.tsRemove.Text = "Remove";
			this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
			// 
			// conEmitterForces
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSubmit);
			this.Name = "conEmitterForces";
			this.Size = new System.Drawing.Size(460, 266);
			this.Leave += new System.EventHandler(this.conEmitterForces_Leave);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.TextBox tbY;
    private System.Windows.Forms.TextBox tbX;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnSubmit;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ListBox lbForces;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsRemove;

  }
}