using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Catalyst3D.Plugins.Landscape.Controls;
using Catalyst3D.Plugins.Landscape.Forms;
using Catalyst3D.PluginSDK;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.AttributeClasses;
using Catalyst3D.PluginSDK.Controls;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.UtilityClasses;
using Telerik.WinControls.Docking;

namespace Catalyst3D.Plugins.Landscape
{
	[AddinName("Landscape Generator")]
	[AddinDescription("Provides landscape generator for your scene")]
	public class Plugin : Addin
	{
		private IAddinHost Host;
		private conTool tRaise;
		private conTool tLower;
		private conTool tPaint;

		private ToolStripComboBox tsBrushSize;
		private ToolStripTextBox tbWeight;

		private EditorLandscape CurrentLandscape;

		public Globals.LandscapeAction LandscapeGenerated;
		private conBrushSettings conBrushSettings;

		public override void Register(IAddinHost host)
		{
			// copy host to local instance
			Host = host;

			if(host != null)
			{
				Trace.WriteLine(DateTime.Now + " | Landscape Addin Loaded .. ");

				// Create our control for the brush texture
				conBrushSettings = new conBrushSettings(Host);
				conBrushSettings.Enabled = false;

				// Add our control to the docking manager
				Host.DockManager.SetDock(conBrushSettings, DockPosition.Default);

				ToolStripSeparator s = new ToolStripSeparator();

				ToolStripButton tsAddLandscape = new ToolStripButton();
				tsAddLandscape.Text = "Generate Landscape";
				tsAddLandscape.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				tsAddLandscape.Click += OnAddLandscape_Clicked;
				Host.ToolStrip.Items.Add(tsAddLandscape);

				// Add a seperator
				Host.ToolStrip.Items.Add(s);

				// Create a Label for our Brush Size
				ToolStripLabel lb1 = new ToolStripLabel("Brush Size : ");
				lb1.Margin = new Padding(5, 0, 0, 0);
				Host.ToolStrip.Items.Add(lb1);

				// Create our Brush Size combo box
				tsBrushSize = new ToolStripComboBox("tsBrushSize");
				tsBrushSize.Text = "4";
				tsBrushSize.Items.Add("4");
				tsBrushSize.Items.Add("6");
				tsBrushSize.Items.Add("8");
				tsBrushSize.Items.Add("10");
				tsBrushSize.Items.Add("12");
				tsBrushSize.Items.Add("14");
				tsBrushSize.Items.Add("16");
				tsBrushSize.Items.Add("18");
				tsBrushSize.Items.Add("20");
				tsBrushSize.Items.Add("22");
				tsBrushSize.Items.Add("24");
				tsBrushSize.Items.Add("26");
				tsBrushSize.Items.Add("28");
				tsBrushSize.Items.Add("30");
				tsBrushSize.Items.Add("32");
				tsBrushSize.Items.Add("64");
				tsBrushSize.Items.Add("128");
				tsBrushSize.Items.Add("256");
				tsBrushSize.TextChanged += OnBrushSize_TextChanged;
				tsBrushSize.Width = 20;
				Host.ToolStrip.Items.Add(tsBrushSize);

				// Brush Weight
				ToolStripLabel lb2 = new ToolStripLabel("Brush Weight : ");
				lb2.Margin = new Padding(5, 0, 0, 0);
				Host.ToolStrip.Items.Add(lb2);

				tbWeight = new ToolStripTextBox("tbBrushWeight");
				tbWeight.Text = "2.0";
				tbWeight.Width = 30;

				tbWeight.TextChanged += OnBrushWeight_TextChanged;
				Host.ToolStrip.Items.Add(tbWeight);

				// Raise Terrain Tool Panel Button
				tRaise = new conTool();
				tRaise.ButtonImage = Resource1.btnRaise;
				tRaise.IsSelected = true;
				tRaise.BackColor = Color.Yellow;
				tRaise.MouseClick += OnRaiseClicked;
				Host.ToolPanel.Controls.Add(tRaise);

				// Lower Terrain Tool Panel Button
				tLower = new conTool();
				tLower.ButtonImage = Resource1.btnLower;
				tLower.MouseClick += OnLowerClicked;
				Host.ToolPanel.Controls.Add(tLower);

				tPaint = new conTool();
				tPaint.ButtonImage = Resource1.PaintBrush;
				tPaint.MouseClick += OnPaintClicked;
				Host.ToolPanel.Controls.Add(tPaint);

				LandscapeGenerated += OnLandscapeAdded;
			}
		}

		private void OnBrushWeight_TextChanged(object sender, EventArgs e)
		{
			string input = Regex.Replace(tbWeight.Text, "[A-Za-z]", "");

			if (input != string.Empty && CurrentLandscape != null)
			{
				CurrentLandscape.BrushWeight = Convert.ToSingle(tbWeight.Text);
			}
		}

		private void OnBrushSize_TextChanged(object sender, EventArgs e)
		{
			string input = Regex.Replace(tsBrushSize.Text, "[A-Za-z]", "");

			if (input != string.Empty && CurrentLandscape != null)
			{
				CurrentLandscape.BrushSize = Convert.ToInt16(input);
			}
		}

		private void OnLandscapeAdded(EditorLandscape landscape)
		{
			CurrentLandscape = landscape;
			conBrushSettings.CurrentLandscape = landscape;
		}

		private void OnLowerClicked(object sender, MouseEventArgs e)
		{
			// Deselect other Tool Panel Icons
			foreach(Control con in AddinManager.Host.ToolPanel.Controls)
			{
				conTool c = con as conTool;
				if(c != null)
				{
					c.BackColor = Color.Transparent;
					c.IsSelected = false;
				}
			}

			if(CurrentLandscape != null)
				CurrentLandscape.BrushMode = BrushModes.Lower;

			tLower.IsSelected = true;
			tLower.BackColor = Color.Yellow;
		}

		private void OnRaiseClicked(object sender, MouseEventArgs e)
		{
			// Deselect other Tool Panel Icons
			foreach(Control con in AddinManager.Host.ToolPanel.Controls)
			{
				conTool c = con as conTool;
				if(c != null)
				{
					c.BackColor = Color.Transparent;
					c.IsSelected = false;
				}
			}

			if(CurrentLandscape != null)
				CurrentLandscape.BrushMode = BrushModes.Raise;

			tRaise.IsSelected = true;
			tRaise.BackColor = Color.Yellow;
		}

		private void OnPaintClicked(object sender, MouseEventArgs e)
		{
			// Deselect other Tool Panel Icons
			foreach(Control con in AddinManager.Host.ToolPanel.Controls)
			{
				conTool c = con as conTool;
				if(c != null)
				{
					c.BackColor = Color.Transparent;
					c.IsSelected = false;
				}
			}

			if(CurrentLandscape != null)
				CurrentLandscape.BrushMode = BrushModes.Paint;

			tPaint.IsSelected = true;
			tPaint.BackColor = Color.Yellow;
		}

		private void OnAddLandscape_Clicked(object sender, EventArgs e)
		{
			frmGenLandscape frm = new frmGenLandscape(Host);
			frm.Tag = LandscapeGenerated;
			frm.Show();
		}

		public override void Save(string path)
		{
			base.Save(path);

			if(CurrentLandscape != null)
			{
				Serializer.Serialize(path + @"\landscape.xml", CurrentLandscape);
			}
		}

		public override void Load(string path)
		{
			base.Load(path);

			string dir = System.IO.Path.GetDirectoryName(path);

			if(System.IO.File.Exists(dir + @"\landscape.xml"))
			{
				EditorLandscape temp = Serializer.Deserialize<EditorLandscape>(System.IO.Path.GetDirectoryName(path) + @"\landscape.xml");

				if(temp != null)
				{
					CurrentLandscape = new EditorLandscape(Host.Game, temp.BlockSize, temp.Width, temp.Height, Host.Camera);
					
					CurrentLandscape.Host = Host;

					CurrentLandscape.FillMode = temp.FillMode;
					CurrentLandscape.CullMode = temp.CullMode;
					CurrentLandscape.Enabled = temp.Enabled;
					CurrentLandscape.Visible = temp.Visible;

					CurrentLandscape.AmbientLightColor = temp.AmbientLightColor;
					CurrentLandscape.AmbientLightIntensity = temp.AmbientLightIntensity;
					CurrentLandscape.DiffuseLightColor = temp.DiffuseLightColor;
					CurrentLandscape.DiffuseLightDirection = temp.DiffuseLightDirection;
					CurrentLandscape.DiffuseLightIntensity = temp.DiffuseLightIntensity;

					CurrentLandscape.Position = temp.Position;
					CurrentLandscape.Scale = temp.Scale;

					CurrentLandscape.Smoothness = temp.Smoothness;
					
					CurrentLandscape.PaintMaskAssetName = temp.PaintMaskAssetName;
					
					CurrentLandscape.Layer1Path = temp.Layer1Path;
					CurrentLandscape.Layer2Path = temp.Layer2Path;
					CurrentLandscape.Layer3Path = temp.Layer3Path;
					CurrentLandscape.Layer4Path = temp.Layer4Path;

					conBrushSettings.CurrentLandscape = CurrentLandscape;

					if(!string.IsNullOrEmpty(temp.Layer1Path))
					{
						conBrushSettings.cbBrushTexture.Items.Add(temp.Layer1Path);
						conBrushSettings.cbBrushTexture.SelectedIndex = 0;
					}

					if(!string.IsNullOrEmpty(temp.Layer2Path))
					{
						conBrushSettings.cbBrushTexture.Items.Add(temp.Layer2Path);
					}

					if(!string.IsNullOrEmpty(temp.Layer3Path))
					{
						conBrushSettings.cbBrushTexture.Items.Add(temp.Layer3Path);
					}

					if(!string.IsNullOrEmpty(temp.Layer4Path))
					{
						conBrushSettings.cbBrushTexture.Items.Add(temp.Layer4Path);
					}

					Host.SceneManager.CurrentScreen.AddVisualObject(CurrentLandscape);
				}
			}
		}

		public override void OnProjectLoaded()
		{
			base.OnProjectLoaded();

			conBrushSettings.Enabled = true;
		}
	}
}