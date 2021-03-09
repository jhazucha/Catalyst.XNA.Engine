using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;

namespace LevelEditor2D.Controls
{
	
	public partial class conKeyFrame : UserControl
	{
		public List<VisualObject> VisualObjects = new List<VisualObject>();

		public int FrameNumber { get; set; }
		public float FrameSpeed = 1.0f;

		public bool IsSelected { get; set; }
		public bool IsLocked { get; set; }

		public conKeyFrame()
		{
			InitializeComponent();
		}

		protected override void OnClick(EventArgs e)
		{
			Globals.FrameSelectedEvent.Invoke(FrameNumber);
			base.OnClick(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rect = new Rectangle(0, 0, 20, 15);
			
			if(IsSelected)
			{
				Pen pen = new Pen(Color.Blue);
				e.Graphics.DrawRectangle(pen, rect);
			}
			if (IsLocked)
			{
				Brush backBrush = Brushes.Red;
				e.Graphics.FillRectangle(backBrush, rect);
			}

			// Draw the label on the frame displaying the Frame Number
			Font font = new Font("Arial", 9, FontStyle.Bold);
			Brush fontBrush = Brushes.Black;
			e.Graphics.DrawString((FrameNumber + 1).ToString(), font, fontBrush, new PointF((Width / 2) - 9, 1));

		}
	}
}