using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalyst3D.PluginSDK;
using Catalyst3D.PluginSDK.AbstractClasses;
using Telerik.WinControls.UI;

namespace LevelEditor3D.Forms
{
  public partial class frmPluginManager : Form
  {
    public frmPluginManager()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      rgPlugins.DataSource = AddinManager.Addins;
    }

    private void tsAddPlugin_Click(object sender, EventArgs e)
    {
      OpenFileDialog dia = new OpenFileDialog();
      dia.Filter = @"Catalyst3D Plugins|*.dll";

      if (dia.ShowDialog() == DialogResult.OK)
      {
        // get the addins from that asm
        List<Addin> addins = AddinManager.ExamineAddin(dia.FileName);

        // install the addins
        AddinManager.LoadAddins(addins, AddinManager.Host);

        // refresh the grid with new addins
        rgPlugins.DataSource = null;
        rgPlugins.DataSource = AddinManager.Addins;
      }
    }

    private void rgPlugins_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
    {

      rgPlugins.Columns["Guid"].IsVisible = false;
      rgPlugins.Columns["Instance"].IsVisible = false;
      rgPlugins.Columns["File"].IsVisible = false;

      rgPlugins.Columns["Path"].HeaderTextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Path"].TextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Path"].Width = 200;
      rgPlugins.Columns["Path"].ReadOnly = true;

      rgPlugins.Columns["Name"].HeaderTextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Name"].TextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Name"].Width = 200;
      rgPlugins.Columns["Name"].ReadOnly = true;

      rgPlugins.Columns["Description"].HeaderTextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Description"].TextAlignment = ContentAlignment.MiddleCenter;
      rgPlugins.Columns["Description"].Width = 400;
      rgPlugins.Columns["Description"].ReadOnly = true;

      rgPlugins.Columns["LoadOnStartup"].HeaderTextAlignment = ContentAlignment.MiddleLeft;
      rgPlugins.Columns["LoadOnStartup"].TextAlignment = ContentAlignment.MiddleLeft;
      rgPlugins.Columns["LoadOnStartup"].HeaderText = @"Load On Startup";
      rgPlugins.Columns["LoadOnStartup"].Width = 100;
      rgPlugins.Columns["Path"].ReadOnly = false;
    }
  }
}
