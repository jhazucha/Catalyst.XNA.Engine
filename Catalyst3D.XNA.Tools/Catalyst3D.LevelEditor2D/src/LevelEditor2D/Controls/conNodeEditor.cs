using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using LevelEditor2D.EntityClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor2D.Controls
{
	public partial class conNodeEditor : UserControl
	{
		private readonly LedgeBuilder Builder;
		private readonly IWindowsFormsEditorService Service;

		public conNodeEditor(IWindowsFormsEditorService editor)
		{
			InitializeComponent();

			Service = editor;

			Globals.IsDialogWindowOpen = true;

      if (Globals.RenderWindow.CurrentSelectedObject is LedgeBuilder)
      {
				LedgeBuilder b = Globals.RenderWindow.CurrentSelectedObject as LedgeBuilder;
        {
          Builder = b;
          lbNodes.DataSource = Builder.Nodes;

          if (Builder.Nodes.Count > 0)
          {
            // Node Scale
            tbScaleX.Text = Builder.Nodes[0].Scale.X.ToString();
            tbScaleY.Text = Builder.Nodes[0].Scale.Y.ToString();

            // Node Position
            tbPositionX.Text = Builder.Nodes[0].Position.X.ToString();
            tbPositionY.Text = Builder.Nodes[0].Position.Y.ToString();

            tbRotation.Text = Builder.Nodes[0].Rotation.ToString();

            tbTravelSpeed.Text = Builder.Nodes[0].TravelSpeed.ToString();
            tbDrawOrder.Text = Builder.Nodes[0].DrawOrder.ToString();

            switch (Builder.Nodes[0].SpriteEffect)
            {
              case SpriteEffects.None:
                cbSpriteEffects.SelectedIndex = 0;
                break;
              case SpriteEffects.FlipHorizontally:
                cbSpriteEffects.SelectedIndex = 1;
                break;
              case SpriteEffects.FlipVertically:
                cbSpriteEffects.SelectedIndex = 2;
                break;
            }

            if (Builder.Sequences != null && Builder.Sequences.Count > 0)
            {
              // Add our sequences to the drop down box
              foreach (var s in Builder.Sequences)
              {
                if (cbAnimationSequence.Items.Contains(s.Name.Trim()))
                  continue;

                cbAnimationSequence.Items.Add(s.Name.Trim());
              }

              cbAnimationSequence.Text = Builder.Sequences[0].Name;

              switch (Builder.Nodes[0].IsLooped)
              {
                case true:
                  cbLooped.Text = @"True";
                  break;
                case false:
                  cbLooped.Text = @"False";
                  break;
              }
            }
          }
        }
      }
      else
				return;
		}

		private void tsAdd_Click(object sender, EventArgs e)
		{
			if (Builder != null)
			{
				// Create our new node
				LedgeNodeDisplay node = new LedgeNodeDisplay(Globals.Game);
				node.Parent = Builder;
				node.Initialize();

				Builder.Nodes.Add(node);

				// if their are nodes behind us copy their attributes
				if (node.Parent.Nodes.Count > 0)
				{
					var index = Builder.Nodes.IndexOf(node);
					node.TravelSpeed = Builder.Nodes[index - 1].TravelSpeed;
					node.Scale = Builder.Nodes[index - 1].Scale;
					node.DrawOrder = Builder.Nodes[index - 1].DrawOrder;
					node.Position = new Vector2(Builder.Nodes[index - 1].Position.X + 25, Builder.Nodes[index - 1].Position.Y);
					node.SpriteEffect = Builder.Nodes[index - 1].SpriteEffect;

					if (Builder.Nodes[index - 1].AnimationSequence != null)
						node.AnimationSequence = Builder.Nodes[index - 1].AnimationSequence;

					node.IsLooped = Builder.Nodes[index - 1].IsLooped;
				}

				lbNodes.DataSource = null;
				lbNodes.DataSource = Builder.Nodes;
			}
		}
		private void tsRemove_Click(object sender, EventArgs e)
		{
			if (Builder != null)
			{
				if (MessageBox.Show(@"Are you sure you want to remove this node?", @"Removal Confirmation", MessageBoxButtons.YesNo,
													MessageBoxIcon.Question) == DialogResult.Yes)
				{

					Builder.Nodes.RemoveAt(lbNodes.SelectedIndex);

					lbNodes.DataSource = null;
					lbNodes.DataSource = Builder.Nodes;
				}
			}
		}

		private void lbNodes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					// Node values
					tbScaleX.Text = n.Scale.X.ToString();
					tbScaleY.Text = n.Scale.Y.ToString();
					tbPositionX.Text = n.Position.X.ToString();
					tbPositionY.Text = n.Position.Y.ToString();
					tbTravelSpeed.Text = n.TravelSpeed.ToString();
					tbRotation.Text = n.Rotation.ToString();
					tbDrawOrder.Text = n.DrawOrder.ToString();

					switch (n.SpriteEffect)
					{
						case SpriteEffects.None:
							cbSpriteEffects.SelectedIndex = 0;
							break;
						case SpriteEffects.FlipHorizontally:
							cbSpriteEffects.SelectedIndex = 1;
							break;
						case SpriteEffects.FlipVertically:
							cbSpriteEffects.SelectedIndex = 2;
							break;
					}

					if (n.AnimationSequence != null)
					{
						// Stop anything playing
						Globals.OnSceneEvent.Invoke(Enums.SceneState.Stopped);

						cbAnimationSequence.Text = n.AnimationSequence.Name;
						cbLooped.Text = n.IsLooped.ToString();
					}
					else
					{
						cbAnimationSequence.Text = string.Empty;
					}
				}
			}
		}

		private void tbScaleX_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbScaleX.Text, out value);

					if (value >= 0)
						n.Scale = new Vector2(value, n.Scale.Y);
				}
			}
		}

		private void tbScaleY_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbScaleY.Text, out value);

					if (value >= 0)
						n.Scale = new Vector2(n.Scale.X, value);
				}
			}
		}

		private void tbPositionX_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbPositionX.Text, out value);

					if (value >= 0)
						n.Position = new Vector2(value, n.Position.Y);
				}
			}
		}

		private void tbPositionY_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbPositionY.Text, out value);

					if (value >= 0)
						n.Position = new Vector2(n.Position.X, value);
				}
			}
		}

		private void tbTravelSpeed_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbTravelSpeed.Text, out value);

					if (value >= 0)
						n.TravelSpeed = value;
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Globals.IsDialogWindowOpen = false;

			Service.CloseDropDown();
		}

		private void btnSetAll_Click(object sender, EventArgs e)
		{
			foreach (var n in Builder.Nodes)
			{
				float value;
				float.TryParse(tbTravelSpeed.Text, out value);

				if (value >= 0)
					n.TravelSpeed = value;
			}
		}

		private void tbDrawOrder_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					int value;
					int.TryParse(tbDrawOrder.Text, out value);
					n.DrawOrder = value;
				}
			}
		}

		private void btnDrawOrderSetAll_Click(object sender, EventArgs e)
		{
			foreach (var n in Builder.Nodes)
			{
				int value;
				int.TryParse(tbDrawOrder.Text, out value);
				n.DrawOrder = value;
			}
		}

		private void tbRotation_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					float value;
					float.TryParse(tbRotation.Text, out value);
					n.Rotation = value;
				}
			}
		}

		private void cbLooped_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					switch (cbLooped.Text)
					{
						case "True":
							n.IsLooped = true;
							break;
						case "False":
							n.IsLooped = false;
							break;
					}
				}
			}
		}
	
		private void cbAnimationSequence_TextChanged(object sender, EventArgs e)
		{
			if (lbNodes.SelectedItem != null)
			{
				int index = lbNodes.SelectedIndex;

				LedgeNodeDisplay n = Builder.Nodes[index];

				if (n != null)
				{
					var sequence = (from s in Builder.Sequences where s.Name.ToUpper().Trim() == cbAnimationSequence.Text.ToUpper().Trim() select s).FirstOrDefault();

					if (sequence != null)
						n.AnimationSequence = sequence;
				}
			}
		}

		private void cbSpriteEffects_SelectedIndexChanged(object sender, EventArgs e)
		{
			SpriteEffects effect = SpriteEffects.None;

			switch (cbSpriteEffects.Text)
			{
				case "None":
					effect = SpriteEffects.None;
					break;
				case "Flip Horizontally":
					effect = SpriteEffects.FlipHorizontally;
					break;
				case "Flip Vertically":
					effect = SpriteEffects.FlipVertically;
					break;
			}

			int index = lbNodes.SelectedIndex;
			LedgeNodeDisplay n = Builder.Nodes[index];

			if (n != null)
				n.SpriteEffect = effect;
		}

		private void conNodeEditor_Leave(object sender, EventArgs e)
		{
			Globals.IsDialogWindowOpen = false;
		}
	}
}