using System.Windows.Forms;

namespace LevelEditor2D.Forms
{
  partial class frmDuplicateSpecial
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rlSceneObjects = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbDirection = new System.Windows.Forms.ComboBox();
			this.radLabel5 = new System.Windows.Forms.Label();
			this.tbYOffset = new System.Windows.Forms.TextBox();
			this.radLabel4 = new System.Windows.Forms.Label();
			this.tbXOffset = new System.Windows.Forms.TextBox();
			this.radLabel3 = new System.Windows.Forms.Label();
			this.tbQTY = new System.Windows.Forms.TextBox();
			this.radLabel2 = new System.Windows.Forms.Label();
			this.btnDuplicate = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rlSceneObjects);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(170, 350);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Scene Objects";
			// 
			// rlSceneObjects
			// 
			this.rlSceneObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rlSceneObjects.Location = new System.Drawing.Point(3, 16);
			this.rlSceneObjects.Name = "rlSceneObjects";
			this.rlSceneObjects.Size = new System.Drawing.Size(164, 331);
			this.rlSceneObjects.TabIndex = 9;
			this.rlSceneObjects.SelectedIndexChanged += new System.EventHandler(this.rlSceneObjects_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbDirection);
			this.groupBox2.Controls.Add(this.radLabel5);
			this.groupBox2.Controls.Add(this.tbYOffset);
			this.groupBox2.Controls.Add(this.radLabel4);
			this.groupBox2.Controls.Add(this.tbXOffset);
			this.groupBox2.Controls.Add(this.radLabel3);
			this.groupBox2.Controls.Add(this.tbQTY);
			this.groupBox2.Controls.Add(this.radLabel2);
			this.groupBox2.Controls.Add(this.btnDuplicate);
			this.groupBox2.Location = new System.Drawing.Point(195, 51);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(219, 192);
			this.groupBox2.TabIndex = 27;
			this.groupBox2.TabStop = false;
			// 
			// cbDirection
			// 
			this.cbDirection.FormattingEnabled = true;
			this.cbDirection.Items.AddRange(new object[] {
            "X",
            "Y",
            "Both"});
			this.cbDirection.Location = new System.Drawing.Point(142, 53);
			this.cbDirection.Name = "cbDirection";
			this.cbDirection.Size = new System.Drawing.Size(59, 21);
			this.cbDirection.TabIndex = 35;
			this.cbDirection.Text = "X";
			// 
			// radLabel5
			// 
			this.radLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel5.Location = new System.Drawing.Point(16, 56);
			this.radLabel5.Name = "radLabel5";
			this.radLabel5.Size = new System.Drawing.Size(129, 14);
			this.radLabel5.TabIndex = 34;
			this.radLabel5.Text = "Duplicate Direction :";
			// 
			// tbYOffset
			// 
			this.tbYOffset.Location = new System.Drawing.Point(142, 109);
			this.tbYOffset.Name = "tbYOffset";
			this.tbYOffset.Size = new System.Drawing.Size(31, 20);
			this.tbYOffset.TabIndex = 33;
			this.tbYOffset.Text = "0";
			// 
			// radLabel4
			// 
			this.radLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel4.Location = new System.Drawing.Point(80, 112);
			this.radLabel4.Name = "radLabel4";
			this.radLabel4.Size = new System.Drawing.Size(62, 14);
			this.radLabel4.TabIndex = 32;
			this.radLabel4.Text = "Y Offset :";
			// 
			// tbXOffset
			// 
			this.tbXOffset.Location = new System.Drawing.Point(142, 83);
			this.tbXOffset.Name = "tbXOffset";
			this.tbXOffset.Size = new System.Drawing.Size(31, 20);
			this.tbXOffset.TabIndex = 31;
			this.tbXOffset.Text = "0";
			// 
			// radLabel3
			// 
			this.radLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel3.Location = new System.Drawing.Point(79, 86);
			this.radLabel3.Name = "radLabel3";
			this.radLabel3.Size = new System.Drawing.Size(63, 14);
			this.radLabel3.TabIndex = 30;
			this.radLabel3.Text = "X Offset :";
			// 
			// tbQTY
			// 
			this.tbQTY.Location = new System.Drawing.Point(142, 26);
			this.tbQTY.Name = "tbQTY";
			this.tbQTY.Size = new System.Drawing.Size(59, 20);
			this.tbQTY.TabIndex = 29;
			this.tbQTY.Text = "1";
			// 
			// radLabel2
			// 
			this.radLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radLabel2.Location = new System.Drawing.Point(77, 29);
			this.radLabel2.Name = "radLabel2";
			this.radLabel2.Size = new System.Drawing.Size(65, 14);
			this.radLabel2.TabIndex = 28;
			this.radLabel2.Text = "Quantity :";
			// 
			// btnDuplicate
			// 
			this.btnDuplicate.Location = new System.Drawing.Point(56, 153);
			this.btnDuplicate.Name = "btnDuplicate";
			this.btnDuplicate.Size = new System.Drawing.Size(117, 23);
			this.btnDuplicate.TabIndex = 27;
			this.btnDuplicate.Text = "Duplicate Objects";
			this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
			// 
			// frmDuplicateSpecial
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 370);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmDuplicateSpecial";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Duplicate Special";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

    }

    #endregion

		private GroupBox groupBox1;
		private ListBox rlSceneObjects;
		private GroupBox groupBox2;
		private ComboBox cbDirection;
		private Label radLabel5;
		private TextBox tbYOffset;
		private Label radLabel4;
		private TextBox tbXOffset;
		private Label radLabel3;
		private TextBox tbQTY;
		private Label radLabel2;
		private Button btnDuplicate;

	}
}