using LevelEditor2D.Controls;

namespace LevelEditor2D.Forms
{
  partial class frmMain
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showPlayerPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showEnemyPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addEmitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.duplicateSpecialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportVisibleObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.groupObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tsResetCamera = new System.Windows.Forms.ToolStripMenuItem();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadGameTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tsStrip2 = new System.Windows.Forms.ToolStrip();
      this.tsAddModel = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
      this.tsAddActor = new System.Windows.Forms.ToolStripButton();
      this.tsAddButton = new System.Windows.Forms.ToolStripButton();
      this.tsAddLabel = new System.Windows.Forms.ToolStripButton();
      this.tsAddRectangle = new System.Windows.Forms.ToolStripButton();
      this.conContainer = new System.Windows.Forms.SplitContainer();
      this.conRenderer = new LevelEditor2D.Controls.conRenderer();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsPlay = new System.Windows.Forms.ToolStripButton();
      this.tsStop = new System.Windows.Forms.ToolStripButton();
      this.tsPause = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.tsResetCamera2 = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
      this.tsCameraOffsetX = new System.Windows.Forms.ToolStripTextBox();
      this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
      this.tsFollow = new System.Windows.Forms.ToolStripButton();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.conRightTabs = new System.Windows.Forms.TabControl();
      this.tabPropertys = new System.Windows.Forms.TabPage();
      this.pgGrid = new System.Windows.Forms.PropertyGrid();
      this.tabSpriteContainer = new System.Windows.Forms.TabPage();
      this.conSpriteContainer = new LevelEditor2D.Controls.conSpriteContainer();
      this.tsAddEmitter = new System.Windows.Forms.ToolStripButton();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.tsDeselectAll = new System.Windows.Forms.ToolStripButton();
      this.btnDuplicateSpecial = new System.Windows.Forms.ToolStripButton();
      this.tsGroupObjects = new System.Windows.Forms.ToolStripButton();
      this.tsGroupSelected = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
      this.tsCreatePath = new System.Windows.Forms.ToolStripButton();
      this.tsSavePath = new System.Windows.Forms.ToolStripButton();
      this.tsCancelPath = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.tsAttachNodes = new System.Windows.Forms.ToolStripButton();
      this.tsDetach = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.tsLock = new System.Windows.Forms.ToolStripButton();
      this.menuStrip1.SuspendLayout();
      this.tsStrip2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.conContainer)).BeginInit();
      this.conContainer.Panel1.SuspendLayout();
      this.conContainer.Panel2.SuspendLayout();
      this.conContainer.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.conRightTabs.SuspendLayout();
      this.tabPropertys.SuspendLayout();
      this.tabSpriteContainer.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1266, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.importProjectToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.toolStripSeparator4,
            this.saveProjectToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newProjectToolStripMenuItem
      // 
      this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
      this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.newProjectToolStripMenuItem.Text = "&New Project";
      this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
      // 
      // loadProjectToolStripMenuItem
      // 
      this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
      this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.loadProjectToolStripMenuItem.Text = "&Load Project";
      this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.tsLoadProject_Click);
      // 
      // importProjectToolStripMenuItem
      // 
      this.importProjectToolStripMenuItem.Name = "importProjectToolStripMenuItem";
      this.importProjectToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.importProjectToolStripMenuItem.Text = "&Import Project";
      this.importProjectToolStripMenuItem.Click += new System.EventHandler(this.importProjectToolStripMenuItem_Click);
      // 
      // closeProjectToolStripMenuItem
      // 
      this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
      this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.closeProjectToolStripMenuItem.Text = "&Close Project";
      this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
      // 
      // saveProjectToolStripMenuItem
      // 
      this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
      this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.saveProjectToolStripMenuItem.Text = "&Save";
      this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.saveAsToolStripMenuItem.Text = "S&ave As";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(147, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.tsExit_Click);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
      this.editToolStripMenuItem.Text = "&Edit";
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
      this.undoToolStripMenuItem.Text = "&Undo";
      this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
      // 
      // viewToolStripMenuItem
      // 
      this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showPlayerPathsToolStripMenuItem,
            this.showEnemyPathsToolStripMenuItem});
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.viewToolStripMenuItem.Text = "&View";
      // 
      // showPlayerPathsToolStripMenuItem
      // 
      this.showPlayerPathsToolStripMenuItem.CheckOnClick = true;
      this.showPlayerPathsToolStripMenuItem.Name = "showPlayerPathsToolStripMenuItem";
      this.showPlayerPathsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
      this.showPlayerPathsToolStripMenuItem.Text = "Show Player Paths";
      // 
      // showEnemyPathsToolStripMenuItem
      // 
      this.showEnemyPathsToolStripMenuItem.CheckOnClick = true;
      this.showEnemyPathsToolStripMenuItem.Name = "showEnemyPathsToolStripMenuItem";
      this.showEnemyPathsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
      this.showEnemyPathsToolStripMenuItem.Text = "Show Enemy Paths";
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEmitterToolStripMenuItem,
            this.duplicateSpecialToolStripMenuItem,
            this.exportVisibleObjectsToolStripMenuItem,
            this.groupObjectsToolStripMenuItem,
            this.tsResetCamera,
            this.optionsToolStripMenuItem,
            this.loadGameTypesToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // addEmitterToolStripMenuItem
      // 
      this.addEmitterToolStripMenuItem.Name = "addEmitterToolStripMenuItem";
      this.addEmitterToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.addEmitterToolStripMenuItem.Text = "Add &Emitter";
      this.addEmitterToolStripMenuItem.Click += new System.EventHandler(this.addEmitterToolStripMenuItem_Click);
      // 
      // duplicateSpecialToolStripMenuItem
      // 
      this.duplicateSpecialToolStripMenuItem.Name = "duplicateSpecialToolStripMenuItem";
      this.duplicateSpecialToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.duplicateSpecialToolStripMenuItem.Text = "&Duplicate Special";
      this.duplicateSpecialToolStripMenuItem.Click += new System.EventHandler(this.duplicateSpecialToolStripMenuItem_Click);
      // 
      // exportVisibleObjectsToolStripMenuItem
      // 
      this.exportVisibleObjectsToolStripMenuItem.Name = "exportVisibleObjectsToolStripMenuItem";
      this.exportVisibleObjectsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.exportVisibleObjectsToolStripMenuItem.Text = "&Export Visible Objects";
      this.exportVisibleObjectsToolStripMenuItem.Click += new System.EventHandler(this.exportVisibleObjectsToolStripMenuItem_Click);
      // 
      // groupObjectsToolStripMenuItem
      // 
      this.groupObjectsToolStripMenuItem.Name = "groupObjectsToolStripMenuItem";
      this.groupObjectsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.groupObjectsToolStripMenuItem.Text = "&Group Objects";
      this.groupObjectsToolStripMenuItem.Click += new System.EventHandler(this.groupObjectsToolStripMenuItem_Click);
      // 
      // tsResetCamera
      // 
      this.tsResetCamera.Name = "tsResetCamera";
      this.tsResetCamera.Size = new System.Drawing.Size(187, 22);
      this.tsResetCamera.Text = "&Reset Camera";
      this.tsResetCamera.Click += new System.EventHandler(this.tsResetCamera_Click);
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.optionsToolStripMenuItem.Text = "&Options";
      this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
      // 
      // loadGameTypesToolStripMenuItem
      // 
      this.loadGameTypesToolStripMenuItem.Name = "loadGameTypesToolStripMenuItem";
      this.loadGameTypesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.loadGameTypesToolStripMenuItem.Text = "&Load Game Types";
      this.loadGameTypesToolStripMenuItem.Click += new System.EventHandler(this.loadGameTypesToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
      // 
      // tsStrip2
      // 
      this.tsStrip2.BackColor = System.Drawing.SystemColors.Control;
      this.tsStrip2.Font = new System.Drawing.Font("Tahoma", 9F);
      this.tsStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.tsStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddModel,
            this.toolStripButton3,
            this.tsAddActor,
            this.tsAddButton,
            this.tsAddLabel,
            this.tsAddRectangle});
      this.tsStrip2.Location = new System.Drawing.Point(0, 24);
      this.tsStrip2.Name = "tsStrip2";
      this.tsStrip2.Padding = new System.Windows.Forms.Padding(0, 5, 1, 5);
      this.tsStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.tsStrip2.Size = new System.Drawing.Size(1266, 33);
      this.tsStrip2.TabIndex = 12;
      // 
      // tsAddModel
      // 
      this.tsAddModel.Font = new System.Drawing.Font("Arial", 9F);
      this.tsAddModel.Image = global::LevelEditor2D.Resource1.add;
      this.tsAddModel.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddModel.Name = "tsAddModel";
      this.tsAddModel.Size = new System.Drawing.Size(84, 20);
      this.tsAddModel.Text = "Add Model";
      this.tsAddModel.Click += new System.EventHandler(this.tsAddModel_Click);
      // 
      // toolStripButton3
      // 
      this.toolStripButton3.Font = new System.Drawing.Font("Arial", 9F);
      this.toolStripButton3.Image = global::LevelEditor2D.Resource1.add;
      this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton3.Name = "toolStripButton3";
      this.toolStripButton3.Size = new System.Drawing.Size(90, 20);
      this.toolStripButton3.Text = "Add Emitter";
      this.toolStripButton3.ToolTipText = "Add Particle Emitter";
      this.toolStripButton3.Click += new System.EventHandler(this.tsAddEmitter_Click);
      // 
      // tsAddActor
      // 
      this.tsAddActor.Font = new System.Drawing.Font("Arial", 9F);
      this.tsAddActor.Image = global::LevelEditor2D.Resource1.add;
      this.tsAddActor.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddActor.Name = "tsAddActor";
      this.tsAddActor.Size = new System.Drawing.Size(140, 20);
      this.tsAddActor.Text = "Add Sprite Animation";
      this.tsAddActor.Click += new System.EventHandler(this.tsAddActor_Click);
      // 
      // tsAddButton
      // 
      this.tsAddButton.Font = new System.Drawing.Font("Arial", 9F);
      this.tsAddButton.Image = global::LevelEditor2D.Resource1.add;
      this.tsAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddButton.Name = "tsAddButton";
      this.tsAddButton.Size = new System.Drawing.Size(86, 20);
      this.tsAddButton.Text = "Add Button";
      this.tsAddButton.Click += new System.EventHandler(this.tsAddButton_Click);
      // 
      // tsAddLabel
      // 
      this.tsAddLabel.Font = new System.Drawing.Font("Arial", 9F);
      this.tsAddLabel.Image = global::LevelEditor2D.Resource1.add;
      this.tsAddLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddLabel.Name = "tsAddLabel";
      this.tsAddLabel.Size = new System.Drawing.Size(82, 20);
      this.tsAddLabel.Text = "Add Label";
      this.tsAddLabel.Click += new System.EventHandler(this.tsAddLabel_Click);
      // 
      // tsAddRectangle
      // 
      this.tsAddRectangle.Image = global::LevelEditor2D.Resource1.add;
      this.tsAddRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddRectangle.Name = "tsAddRectangle";
      this.tsAddRectangle.Size = new System.Drawing.Size(107, 20);
      this.tsAddRectangle.Text = "Add Rectangle";
      this.tsAddRectangle.Click += new System.EventHandler(this.tsAddRectangle_Click);
      // 
      // conContainer
      // 
      this.conContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conContainer.Location = new System.Drawing.Point(0, 82);
      this.conContainer.Name = "conContainer";
      // 
      // conContainer.Panel1
      // 
      this.conContainer.Panel1.Controls.Add(this.conRenderer);
      this.conContainer.Panel1.Controls.Add(this.toolStrip1);
      // 
      // conContainer.Panel2
      // 
      this.conContainer.Panel2.Controls.Add(this.conRightTabs);
      this.conContainer.Panel2MinSize = 100;
      this.conContainer.Size = new System.Drawing.Size(1266, 682);
      this.conContainer.SplitterDistance = 922;
      this.conContainer.TabIndex = 13;
      this.conContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.conContainer1_SplitterMoved);
      // 
      // conRenderer
      // 
      this.conRenderer.BackColor = System.Drawing.Color.Black;
      this.conRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conRenderer.Location = new System.Drawing.Point(0, 25);
      this.conRenderer.Name = "conRenderer";
      this.conRenderer.Size = new System.Drawing.Size(922, 657);
      this.conRenderer.TabIndex = 0;
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPlay,
            this.tsStop,
            this.tsPause,
            this.toolStripSeparator6,
            this.tsResetCamera2,
            this.toolStripSeparator8,
            this.tsCameraOffsetX,
            this.toolStripLabel2,
            this.tsFollow,
            this.toolStripLabel1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(922, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsPlay
      // 
      this.tsPlay.Image = global::LevelEditor2D.Resource1.player_play_256;
      this.tsPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsPlay.Name = "tsPlay";
      this.tsPlay.Size = new System.Drawing.Size(49, 22);
      this.tsPlay.Text = "Play";
      this.tsPlay.Click += new System.EventHandler(this.tsPlay_Click);
      // 
      // tsStop
      // 
      this.tsStop.Image = global::LevelEditor2D.Resource1.player_stop_256;
      this.tsStop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsStop.Name = "tsStop";
      this.tsStop.Size = new System.Drawing.Size(51, 22);
      this.tsStop.Text = "Stop";
      this.tsStop.Click += new System.EventHandler(this.tsStop_Click);
      // 
      // tsPause
      // 
      this.tsPause.Image = global::LevelEditor2D.Resource1.player_pause_256;
      this.tsPause.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsPause.Name = "tsPause";
      this.tsPause.Size = new System.Drawing.Size(58, 22);
      this.tsPause.Text = "Pause";
      this.tsPause.Click += new System.EventHandler(this.tsPause_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
      // 
      // tsResetCamera2
      // 
      this.tsResetCamera2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsResetCamera2.Image = ((System.Drawing.Image)(resources.GetObject("tsResetCamera2.Image")));
      this.tsResetCamera2.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsResetCamera2.Name = "tsResetCamera2";
      this.tsResetCamera2.Size = new System.Drawing.Size(99, 22);
      this.tsResetCamera2.Text = "Reset Camera";
      this.tsResetCamera2.Click += new System.EventHandler(this.tsResetCamera2_Click);
      // 
      // toolStripSeparator8
      // 
      this.toolStripSeparator8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
      // 
      // tsCameraOffsetX
      // 
      this.tsCameraOffsetX.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsCameraOffsetX.BackColor = System.Drawing.Color.White;
      this.tsCameraOffsetX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tsCameraOffsetX.ForeColor = System.Drawing.Color.Black;
      this.tsCameraOffsetX.Margin = new System.Windows.Forms.Padding(1, 0, 5, 0);
      this.tsCameraOffsetX.Name = "tsCameraOffsetX";
      this.tsCameraOffsetX.Size = new System.Drawing.Size(40, 25);
      this.tsCameraOffsetX.Text = "0";
      this.tsCameraOffsetX.TextChanged += new System.EventHandler(this.tsCameraOffsetX_TextChanged);
      // 
      // toolStripLabel2
      // 
      this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripLabel2.Name = "toolStripLabel2";
      this.toolStripLabel2.Size = new System.Drawing.Size(86, 22);
      this.toolStripLabel2.Text = "Follow Offset : ";
      // 
      // tsFollow
      // 
      this.tsFollow.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsFollow.Image = ((System.Drawing.Image)(resources.GetObject("tsFollow.Image")));
      this.tsFollow.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsFollow.Name = "tsFollow";
      this.tsFollow.Size = new System.Drawing.Size(109, 22);
      this.tsFollow.Text = "Follow Selected";
      this.tsFollow.Click += new System.EventHandler(this.tsFollow_Click);
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(99, 22);
      this.toolStripLabel1.Text = "Camera Settings :";
      // 
      // conRightTabs
      // 
      this.conRightTabs.Controls.Add(this.tabPropertys);
      this.conRightTabs.Controls.Add(this.tabSpriteContainer);
      this.conRightTabs.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conRightTabs.Location = new System.Drawing.Point(0, 0);
      this.conRightTabs.Name = "conRightTabs";
      this.conRightTabs.SelectedIndex = 0;
      this.conRightTabs.Size = new System.Drawing.Size(340, 682);
      this.conRightTabs.TabIndex = 0;
      // 
      // tabPropertys
      // 
      this.tabPropertys.Controls.Add(this.pgGrid);
      this.tabPropertys.Location = new System.Drawing.Point(4, 22);
      this.tabPropertys.Name = "tabPropertys";
      this.tabPropertys.Padding = new System.Windows.Forms.Padding(3);
      this.tabPropertys.Size = new System.Drawing.Size(332, 656);
      this.tabPropertys.TabIndex = 0;
      this.tabPropertys.Text = "Scene Objects";
      this.tabPropertys.UseVisualStyleBackColor = true;
      // 
      // pgGrid
      // 
      this.pgGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pgGrid.Location = new System.Drawing.Point(3, 3);
      this.pgGrid.Name = "pgGrid";
      this.pgGrid.Size = new System.Drawing.Size(326, 650);
      this.pgGrid.TabIndex = 0;
      this.pgGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.pgGrid_SelectedGridItemChanged);
      // 
      // tabSpriteContainer
      // 
      this.tabSpriteContainer.Controls.Add(this.conSpriteContainer);
      this.tabSpriteContainer.Location = new System.Drawing.Point(4, 22);
      this.tabSpriteContainer.Name = "tabSpriteContainer";
      this.tabSpriteContainer.Padding = new System.Windows.Forms.Padding(3);
      this.tabSpriteContainer.Size = new System.Drawing.Size(331, 656);
      this.tabSpriteContainer.TabIndex = 1;
      this.tabSpriteContainer.Text = "Sprite Container";
      this.tabSpriteContainer.UseVisualStyleBackColor = true;
      // 
      // conSpriteContainer
      // 
      this.conSpriteContainer.AutoScroll = true;
      this.conSpriteContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.conSpriteContainer.Location = new System.Drawing.Point(3, 3);
      this.conSpriteContainer.Margin = new System.Windows.Forms.Padding(1);
      this.conSpriteContainer.Name = "conSpriteContainer";
      this.conSpriteContainer.Size = new System.Drawing.Size(321, 650);
      this.conSpriteContainer.TabIndex = 0;
      // 
      // tsAddEmitter
      // 
      this.tsAddEmitter.Image = ((System.Drawing.Image)(resources.GetObject("tsAddEmitter.Image")));
      this.tsAddEmitter.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAddEmitter.Name = "tsAddEmitter";
      this.tsAddEmitter.Size = new System.Drawing.Size(90, 20);
      this.tsAddEmitter.Text = "Add Emitter";
      this.tsAddEmitter.ToolTipText = "Add Particle Emitter";
      // 
      // toolStrip2
      // 
      this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDeselectAll,
            this.btnDuplicateSpecial,
            this.tsGroupObjects,
            this.tsGroupSelected,
            this.toolStripSeparator7,
            this.tsCreatePath,
            this.tsSavePath,
            this.tsCancelPath,
            this.toolStripSeparator3,
            this.tsAttachNodes,
            this.tsDetach,
            this.toolStripSeparator2,
            this.tsLock});
      this.toolStrip2.Location = new System.Drawing.Point(0, 57);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(1266, 25);
      this.toolStrip2.TabIndex = 14;
      this.toolStrip2.Text = "toolStrip2";
      // 
      // tsDeselectAll
      // 
      this.tsDeselectAll.Font = new System.Drawing.Font("Arial", 9F);
      this.tsDeselectAll.Image = global::LevelEditor2D.Resource1.mouse_delete;
      this.tsDeselectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsDeselectAll.Name = "tsDeselectAll";
      this.tsDeselectAll.Size = new System.Drawing.Size(136, 22);
      this.tsDeselectAll.Text = "Deselect All Objects";
      this.tsDeselectAll.Click += new System.EventHandler(this.tsDeselectAll_Click);
      // 
      // btnDuplicateSpecial
      // 
      this.btnDuplicateSpecial.Font = new System.Drawing.Font("Arial", 9F);
      this.btnDuplicateSpecial.Image = global::LevelEditor2D.Resource1.page_copy;
      this.btnDuplicateSpecial.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnDuplicateSpecial.Name = "btnDuplicateSpecial";
      this.btnDuplicateSpecial.Size = new System.Drawing.Size(123, 22);
      this.btnDuplicateSpecial.Text = "Duplicate Special";
      this.btnDuplicateSpecial.Click += new System.EventHandler(this.btnDuplicateSpecial_Click);
      // 
      // tsGroupObjects
      // 
      this.tsGroupObjects.Font = new System.Drawing.Font("Arial", 9F);
      this.tsGroupObjects.Image = global::LevelEditor2D.Resource1.shape_group;
      this.tsGroupObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsGroupObjects.Name = "tsGroupObjects";
      this.tsGroupObjects.Size = new System.Drawing.Size(144, 22);
      this.tsGroupObjects.Text = "Group Scene Objects";
      this.tsGroupObjects.Click += new System.EventHandler(this.tsGroupObjects_Click);
      // 
      // tsGroupSelected
      // 
      this.tsGroupSelected.Image = ((System.Drawing.Image)(resources.GetObject("tsGroupSelected.Image")));
      this.tsGroupSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsGroupSelected.Name = "tsGroupSelected";
      this.tsGroupSelected.Size = new System.Drawing.Size(107, 22);
      this.tsGroupSelected.Text = "Group Selected";
      this.tsGroupSelected.Click += new System.EventHandler(this.tsGroupSelected_Click);
      // 
      // toolStripSeparator7
      // 
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
      // 
      // tsCreatePath
      // 
      this.tsCreatePath.Font = new System.Drawing.Font("Arial", 9F);
      this.tsCreatePath.Image = global::LevelEditor2D.Resource1.pencil;
      this.tsCreatePath.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsCreatePath.Name = "tsCreatePath";
      this.tsCreatePath.Size = new System.Drawing.Size(129, 22);
      this.tsCreatePath.Text = "Show Pathing Tool";
      this.tsCreatePath.Click += new System.EventHandler(this.tsCreatePath_Click);
      // 
      // tsSavePath
      // 
      this.tsSavePath.Font = new System.Drawing.Font("Arial", 9F);
      this.tsSavePath.Image = global::LevelEditor2D.Resource1.pencil_go;
      this.tsSavePath.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsSavePath.Name = "tsSavePath";
      this.tsSavePath.Size = new System.Drawing.Size(126, 22);
      this.tsSavePath.Text = "Save Current Path";
      this.tsSavePath.Click += new System.EventHandler(this.tsSavePath_Click);
      // 
      // tsCancelPath
      // 
      this.tsCancelPath.Font = new System.Drawing.Font("Arial", 9F);
      this.tsCancelPath.Image = global::LevelEditor2D.Resource1.pencil_delete;
      this.tsCancelPath.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsCancelPath.Name = "tsCancelPath";
      this.tsCancelPath.Size = new System.Drawing.Size(94, 22);
      this.tsCancelPath.Text = "Cancel Path";
      this.tsCancelPath.Click += new System.EventHandler(this.tsCancelPath_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // tsAttachNodes
      // 
      this.tsAttachNodes.Image = global::LevelEditor2D.Resource1.application_link;
      this.tsAttachNodes.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsAttachNodes.Name = "tsAttachNodes";
      this.tsAttachNodes.Size = new System.Drawing.Size(141, 22);
      this.tsAttachNodes.Text = "Attach Path to Object";
      this.tsAttachNodes.Click += new System.EventHandler(this.tsAttachNodes_Click);
      // 
      // tsDetach
      // 
      this.tsDetach.Image = ((System.Drawing.Image)(resources.GetObject("tsDetach.Image")));
      this.tsDetach.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsDetach.Name = "tsDetach";
      this.tsDetach.Size = new System.Drawing.Size(95, 22);
      this.tsDetach.Text = "Detatch Path";
      this.tsDetach.Click += new System.EventHandler(this.tsDetach_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // tsLock
      // 
      this.tsLock.Image = global::LevelEditor2D.Resource1._lock;
      this.tsLock.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsLock.Name = "tsLock";
      this.tsLock.Size = new System.Drawing.Size(99, 22);
      this.tsLock.Text = "Lock Selected";
      this.tsLock.Click += new System.EventHandler(this.tsLock_Click);
      // 
      // frmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1266, 764);
      this.Controls.Add(this.conContainer);
      this.Controls.Add(this.toolStrip2);
      this.Controls.Add(this.tsStrip2);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "frmMain";
      this.Text = "Catalyst3D - 2D Level Editor";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tsStrip2.ResumeLayout(false);
      this.tsStrip2.PerformLayout();
      this.conContainer.Panel1.ResumeLayout(false);
      this.conContainer.Panel1.PerformLayout();
      this.conContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.conContainer)).EndInit();
      this.conContainer.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.conRightTabs.ResumeLayout(false);
      this.tabPropertys.ResumeLayout(false);
      this.tabSpriteContainer.ResumeLayout(false);
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton tsAddEmitter;
		private System.Windows.Forms.ToolStrip tsStrip2;
    private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem duplicateSpecialToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem groupObjectsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addEmitterToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showPlayerPathsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showEnemyPathsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsResetCamera;
		private System.Windows.Forms.ToolStripButton tsAddActor;
		private System.Windows.Forms.SplitContainer conContainer;
		public conRenderer conRenderer;
		private System.Windows.Forms.TabControl conRightTabs;
		private System.Windows.Forms.TabPage tabPropertys;
		private System.Windows.Forms.PropertyGrid pgGrid;
		private System.Windows.Forms.TabPage tabSpriteContainer;
		private conSpriteContainer conSpriteContainer;
    private System.Windows.Forms.ToolStripButton tsAddButton;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsPlay;
    private System.Windows.Forms.ToolStripButton tsStop;
		private System.Windows.Forms.ToolStripButton tsAddModel;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton tsDeselectAll;
		private System.Windows.Forms.ToolStripButton btnDuplicateSpecial;
		private System.Windows.Forms.ToolStripButton tsGroupObjects;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton tsCreatePath;
		private System.Windows.Forms.ToolStripButton tsCancelPath;
		private System.Windows.Forms.ToolStripButton tsSavePath;
		private System.Windows.Forms.ToolStripButton tsAddLabel;
		private System.Windows.Forms.ToolStripMenuItem importProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportVisibleObjectsToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton tsAttachNodes;
		private System.Windows.Forms.ToolStripMenuItem loadGameTypesToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton tsPause;
		private System.Windows.Forms.ToolStripButton tsAddRectangle;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton tsFollow;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsResetCamera2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripTextBox tsCameraOffsetX;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripButton tsLock;
		private System.Windows.Forms.ToolStripButton tsGroupSelected;
		private System.Windows.Forms.ToolStripButton tsDetach;
  }
}