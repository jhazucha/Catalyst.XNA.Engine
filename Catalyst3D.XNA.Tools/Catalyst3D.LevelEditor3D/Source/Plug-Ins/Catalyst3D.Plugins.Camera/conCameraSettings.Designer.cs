namespace Catalyst3D.Plugin.Camera
{
	partial class conCameraSettings
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
			this.pgCamera = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// pgCamera
			// 
			this.pgCamera.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgCamera.Location = new System.Drawing.Point(0, 0);
			this.pgCamera.Name = "pgCamera";
			this.pgCamera.Size = new System.Drawing.Size(176, 360);
			this.Text = "Camera";
			this.pgCamera.TabIndex = 0;
			// 
			// conCameraSettings
			// 
			this.Controls.Add(this.pgCamera);
			this.Name = "conCameraSettings";
			this.Size = new System.Drawing.Size(176, 360);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.PropertyGrid pgCamera;
	}
}
