namespace LevelEditor2D.Controls
{
	partial class conNodeEditor
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbNodes = new System.Windows.Forms.ListBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsAdd = new System.Windows.Forms.ToolStripButton();
			this.tsRemove = new System.Windows.Forms.ToolStripButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbSpriteEffects = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.cbLooped = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cbAnimationSequence = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbRotation = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnDrawOrderSetAll = new System.Windows.Forms.Button();
			this.tbDrawOrder = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnSetAllNodes = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.tbPositionY = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbPositionX = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbTravelSpeed = new System.Windows.Forms.TextBox();
			this.tbScaleY = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbScaleX = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.lbNodes);
			this.groupBox1.Controls.Add(this.toolStrip1);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(157, 325);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// lbNodes
			// 
			this.lbNodes.FormattingEnabled = true;
			this.lbNodes.Location = new System.Drawing.Point(4, 45);
			this.lbNodes.Name = "lbNodes";
			this.lbNodes.ScrollAlwaysVisible = true;
			this.lbNodes.Size = new System.Drawing.Size(148, 277);
			this.lbNodes.TabIndex = 1;
			this.lbNodes.SelectedIndexChanged += new System.EventHandler(this.lbNodes_SelectedIndexChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAdd,
            this.tsRemove});
			this.toolStrip1.Location = new System.Drawing.Point(3, 16);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(151, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsAdd
			// 
			this.tsAdd.Image = global::LevelEditor2D.Resource1.add;
			this.tsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsAdd.Name = "tsAdd";
			this.tsAdd.Size = new System.Drawing.Size(49, 22);
			this.tsAdd.Text = "Add";
			this.tsAdd.Click += new System.EventHandler(this.tsAdd_Click);
			// 
			// tsRemove
			// 
			this.tsRemove.Image = global::LevelEditor2D.Resource1.remove;
			this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsRemove.Name = "tsRemove";
			this.tsRemove.Size = new System.Drawing.Size(70, 22);
			this.tsRemove.Text = "Remove";
			this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox2.Controls.Add(this.cbSpriteEffects);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.cbLooped);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.cbAnimationSequence);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.tbRotation);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.btnDrawOrderSetAll);
			this.groupBox2.Controls.Add(this.tbDrawOrder);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.btnSetAllNodes);
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.tbPositionY);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.tbPositionX);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.tbTravelSpeed);
			this.groupBox2.Controls.Add(this.tbScaleY);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.tbScaleX);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(166, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(329, 325);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Node Settings";
			// 
			// cbSpriteEffects
			// 
			this.cbSpriteEffects.FormattingEnabled = true;
			this.cbSpriteEffects.Items.AddRange(new object[] {
            "None",
            "Flip Horizontally",
            "Flip Vertically"});
			this.cbSpriteEffects.Location = new System.Drawing.Point(149, 235);
			this.cbSpriteEffects.Name = "cbSpriteEffects";
			this.cbSpriteEffects.Size = new System.Drawing.Size(157, 21);
			this.cbSpriteEffects.TabIndex = 28;
			this.cbSpriteEffects.Text = "None";
			this.cbSpriteEffects.SelectedIndexChanged += new System.EventHandler(this.cbSpriteEffects_SelectedIndexChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(66, 238);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(80, 14);
			this.label10.TabIndex = 27;
			this.label10.Text = "Sprite Effect :";
			// 
			// cbLooped
			// 
			this.cbLooped.FormattingEnabled = true;
			this.cbLooped.Items.AddRange(new object[] {
            "True",
            "False"});
			this.cbLooped.Location = new System.Drawing.Point(149, 208);
			this.cbLooped.Name = "cbLooped";
			this.cbLooped.Size = new System.Drawing.Size(72, 21);
			this.cbLooped.TabIndex = 26;
			this.cbLooped.Text = "True";
			this.cbLooped.TextChanged += new System.EventHandler(this.cbLooped_TextChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(91, 211);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(55, 14);
			this.label9.TabIndex = 25;
			this.label9.Text = "Looped :";
			// 
			// cbAnimationSequence
			// 
			this.cbAnimationSequence.FormattingEnabled = true;
			this.cbAnimationSequence.Location = new System.Drawing.Point(149, 182);
			this.cbAnimationSequence.Name = "cbAnimationSequence";
			this.cbAnimationSequence.Size = new System.Drawing.Size(157, 21);
			this.cbAnimationSequence.TabIndex = 24;
			this.cbAnimationSequence.TextChanged += new System.EventHandler(this.cbAnimationSequence_TextChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(19, 185);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(127, 14);
			this.label8.TabIndex = 23;
			this.label8.Text = "Animation Sequence :";
			// 
			// tbRotation
			// 
			this.tbRotation.Location = new System.Drawing.Point(149, 83);
			this.tbRotation.Name = "tbRotation";
			this.tbRotation.Size = new System.Drawing.Size(46, 20);
			this.tbRotation.TabIndex = 22;
			this.tbRotation.Text = "0";
			this.tbRotation.TextChanged += new System.EventHandler(this.tbRotation_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(51, 86);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(96, 14);
			this.label7.TabIndex = 21;
			this.label7.Text = "Object Rotation :";
			// 
			// btnDrawOrderSetAll
			// 
			this.btnDrawOrderSetAll.Location = new System.Drawing.Point(207, 141);
			this.btnDrawOrderSetAll.Name = "btnDrawOrderSetAll";
			this.btnDrawOrderSetAll.Size = new System.Drawing.Size(99, 23);
			this.btnDrawOrderSetAll.TabIndex = 20;
			this.btnDrawOrderSetAll.Text = "Set All Nodes";
			this.btnDrawOrderSetAll.UseVisualStyleBackColor = true;
			this.btnDrawOrderSetAll.Click += new System.EventHandler(this.btnDrawOrderSetAll_Click);
			// 
			// tbDrawOrder
			// 
			this.tbDrawOrder.Location = new System.Drawing.Point(149, 143);
			this.tbDrawOrder.Name = "tbDrawOrder";
			this.tbDrawOrder.Size = new System.Drawing.Size(46, 20);
			this.tbDrawOrder.TabIndex = 19;
			this.tbDrawOrder.Text = "0";
			this.tbDrawOrder.TextChanged += new System.EventHandler(this.tbDrawOrder_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(70, 146);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(76, 14);
			this.label6.TabIndex = 18;
			this.label6.Text = "Draw Order :";
			// 
			// btnSetAllNodes
			// 
			this.btnSetAllNodes.Location = new System.Drawing.Point(207, 111);
			this.btnSetAllNodes.Name = "btnSetAllNodes";
			this.btnSetAllNodes.Size = new System.Drawing.Size(99, 23);
			this.btnSetAllNodes.TabIndex = 7;
			this.btnSetAllNodes.Text = "Set All Nodes";
			this.btnSetAllNodes.UseVisualStyleBackColor = true;
			this.btnSetAllNodes.Click += new System.EventHandler(this.btnSetAll_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(232, 293);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(90, 23);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// tbPositionY
			// 
			this.tbPositionY.Location = new System.Drawing.Point(224, 54);
			this.tbPositionY.Name = "tbPositionY";
			this.tbPositionY.Size = new System.Drawing.Size(46, 20);
			this.tbPositionY.TabIndex = 4;
			this.tbPositionY.Text = "0";
			this.tbPositionY.TextChanged += new System.EventHandler(this.tbPositionY_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(201, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(20, 14);
			this.label4.TabIndex = 17;
			this.label4.Text = "Y :";
			// 
			// tbPositionX
			// 
			this.tbPositionX.Location = new System.Drawing.Point(149, 54);
			this.tbPositionX.Name = "tbPositionX";
			this.tbPositionX.Size = new System.Drawing.Size(46, 20);
			this.tbPositionX.TabIndex = 3;
			this.tbPositionX.Text = "0";
			this.tbPositionX.TextChanged += new System.EventHandler(this.tbPositionX_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(42, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(105, 14);
			this.label5.TabIndex = 15;
			this.label5.Text = "Node Position   X :";
			// 
			// tbTravelSpeed
			// 
			this.tbTravelSpeed.Location = new System.Drawing.Point(149, 113);
			this.tbTravelSpeed.Name = "tbTravelSpeed";
			this.tbTravelSpeed.Size = new System.Drawing.Size(46, 20);
			this.tbTravelSpeed.TabIndex = 5;
			this.tbTravelSpeed.Text = "0";
			this.tbTravelSpeed.TextChanged += new System.EventHandler(this.tbTravelSpeed_TextChanged);
			// 
			// tbScaleY
			// 
			this.tbScaleY.Location = new System.Drawing.Point(224, 25);
			this.tbScaleY.Name = "tbScaleY";
			this.tbScaleY.Size = new System.Drawing.Size(46, 20);
			this.tbScaleY.TabIndex = 2;
			this.tbScaleY.Text = "0";
			this.tbScaleY.TextChanged += new System.EventHandler(this.tbScaleY_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(201, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(20, 14);
			this.label3.TabIndex = 12;
			this.label3.Text = "Y :";
			// 
			// tbScaleX
			// 
			this.tbScaleX.Location = new System.Drawing.Point(149, 25);
			this.tbScaleX.Name = "tbScaleX";
			this.tbScaleX.Size = new System.Drawing.Size(46, 20);
			this.tbScaleX.TabIndex = 1;
			this.tbScaleX.Text = "0";
			this.tbScaleX.TextChanged += new System.EventHandler(this.tbScaleX_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(62, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 14);
			this.label2.TabIndex = 10;
			this.label2.Text = "Travel Speed :";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(51, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 14);
			this.label1.TabIndex = 9;
			this.label1.Text = "Object Scale   X :";
			// 
			// conNodeEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "conNodeEditor";
			this.Size = new System.Drawing.Size(498, 331);
			this.Leave += new System.EventHandler(this.conNodeEditor_Leave);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox lbNodes;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsAdd;
		private System.Windows.Forms.ToolStripButton tsRemove;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox tbScaleY;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbScaleX;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbTravelSpeed;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox tbPositionY;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbPositionX;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnSetAllNodes;
		private System.Windows.Forms.TextBox tbDrawOrder;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnDrawOrderSetAll;
		private System.Windows.Forms.TextBox tbRotation;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbAnimationSequence;
		private System.Windows.Forms.ComboBox cbLooped;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cbSpriteEffects;
		private System.Windows.Forms.Label label10;

	}
}
