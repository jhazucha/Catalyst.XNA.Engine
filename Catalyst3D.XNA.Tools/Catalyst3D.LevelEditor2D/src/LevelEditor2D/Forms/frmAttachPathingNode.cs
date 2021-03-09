using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Catalyst3D.XNA.Engine.AbstractClasses;
using LevelEditor2D.EntityClasses;

namespace LevelEditor2D.Forms
{
	public partial class frmAttachPathingNode : Form
	{
		public List<VisualObject> SceneObjects = new List<VisualObject>();
		public List<LedgeBuilder> PathingNodes = new List<LedgeBuilder>();

		public frmAttachPathingNode()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Add our Scene Objects to our list view
			lsSceneObjects.DataSource = SceneObjects;
			lsSceneObjects.DisplayMember = "Name";

			// Add our Pathing Nodes to our list view
			lsAvailablePaths.DataSource = PathingNodes;
			lsAvailablePaths.DisplayMember = "Name";

			Globals.IsDialogWindowOpen = true;
		}
	
		private void lsSceneObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Deselect all scene objects
			foreach (VisualObject vo in SceneObjects)
				vo.IsSelected = false;

			// Loop thru and select all the ones we have highlighted in the list box
			foreach (var i in lsSceneObjects.SelectedItems)
			{
				if (i is VisualObject)
				{
					VisualObject o = i as VisualObject;
					o.IsSelected = true;
				}
			}
		}

		private void lsAvailablePaths_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Deselect all scene objects
			foreach (LedgeBuilder vo in PathingNodes)
				vo.IsSelected = false;

			// Loop thru and select all the ones we have highlighted in the list box
			foreach (var i in lsAvailablePaths.SelectedItems)
			{
				if (i is LedgeBuilder)
				{
					LedgeBuilder o = i as LedgeBuilder;
					o.IsSelected = true;
				}
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (lsSceneObjects.SelectedItem != null && lsAvailablePaths.SelectedItem != null)
			{
				if (lsSceneObjects.SelectedItem is EditorGroup)
				{
					EditorGroup vo = lsSceneObjects.SelectedItem as EditorGroup;
					LedgeBuilder bo = lsAvailablePaths.SelectedItem as LedgeBuilder;

					if (bo != null)
					{
						vo.AttachedPathingNode = bo;
						vo.CurrentPathNodeIndex = Convert.ToInt16(tbNodeIndex.Text);
						vo.AttachedPathingNodeName = bo.Name;

						foreach (LedgeNodeDisplay node in bo.Nodes)
							node.Scale = vo.Scale;

						Globals.IsDialogWindowOpen = false;

						Dispose();
					}
				}

				if (lsSceneObjects.SelectedItem is EditorSprite)
				{
					EditorSprite vo = lsSceneObjects.SelectedItem as EditorSprite;
					LedgeBuilder bo = lsAvailablePaths.SelectedItem as LedgeBuilder;

					if (bo != null)
					{
						vo.AttachedPathingNode = bo;
						vo.CurrentPathNodeIndex = Convert.ToInt16(tbNodeIndex.Text);
						vo.AttachedPathingNodeName = bo.Name;

						foreach(LedgeNodeDisplay node in bo.Nodes)
							node.Scale = vo.Scale;

						Globals.IsDialogWindowOpen = false;

						Dispose();
					}
				}

				if (lsSceneObjects.SelectedItem is EditorEmitter)
				{
					EditorEmitter vo = lsSceneObjects.SelectedItem as EditorEmitter;
					LedgeBuilder bo = lsAvailablePaths.SelectedItem as LedgeBuilder;

					if (bo != null)
					{
						vo.AttachedPathingNode = bo;
						vo.CurrentPathNodeIndex = Convert.ToInt16(tbNodeIndex.Text);
						vo.AttachedPathingNodeName = bo.Name;

						foreach (LedgeNodeDisplay node in bo.Nodes)
							node.Scale = vo.Scale;

						Globals.IsDialogWindowOpen = false;

						Dispose();
					}
				}

				if (lsSceneObjects.SelectedItem is EditorActor)
				{
					EditorActor vo = lsSceneObjects.SelectedItem as EditorActor;
					LedgeBuilder bo = lsAvailablePaths.SelectedItem as LedgeBuilder;

					if (bo != null)
					{
						vo.AttachedPathingNode = bo;
						vo.CurrentPathNodeIndex = Convert.ToInt16(tbNodeIndex.Text);
						vo.AttachedPathingNodeName = bo.Name;
						
						// Store these in the pathing node
						bo.Sequences = vo.Sequences;

						foreach (LedgeNodeDisplay node in bo.Nodes)
						{
							node.AnimationSequence = vo.ClipPlayer.CurrentSequence;
							node.Scale = vo.Scale;
						}

						Globals.IsDialogWindowOpen = false;

						Dispose();
					}
				}
			}
			else
			{
				MessageBox.Show(@"Please select a scene object and an available pathing node to attach it to.");
			}
		}

		protected override void DestroyHandle()
		{
			Globals.IsDialogWindowOpen = false;

			base.DestroyHandle();
		}
	}
}