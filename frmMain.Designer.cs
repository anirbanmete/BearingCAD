namespace BearingCAD21
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUserInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExit = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdThrustBearingDesgnDetail = new System.Windows.Forms.Button();
            this.cmdThrustBearingData = new System.Windows.Forms.Button();
            this.cmdCreateFiles = new System.Windows.Forms.Button();
            this.cmdEndSealDesgnDetail = new System.Windows.Forms.Button();
            this.cmdEndSealData = new System.Windows.Forms.Button();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.cmdRadialBearingData = new System.Windows.Forms.Button();
            this.cmdOpCond = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdProject = new System.Windows.Forms.Button();
            this.cmdRadialBearingDesgnDetail = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.SBar1 = new System.Windows.Forms.StatusStrip();
            this.SBpanel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SBpanel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SBpanel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SBpanel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SBpanel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.MainMenu.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuUserInfo});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainMenu.Size = new System.Drawing.Size(792, 24);
            this.MainMenu.TabIndex = 66;
            this.MainMenu.Text = "MainMenu";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.ToolStripMenuItem1,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.ShortcutKeyDisplayString = "";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Enabled = false;
            this.mnuFileNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileNew.Image")));
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.Size = new System.Drawing.Size(114, 22);
            this.mnuFileNew.Text = "&New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuItem_Click);
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileOpen.Image")));
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.Size = new System.Drawing.Size(114, 22);
            this.mnuFileOpen.Text = "&Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuItem_Click);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileSave.Image")));
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.Size = new System.Drawing.Size(114, 22);
            this.mnuFileSave.Text = "&Save";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuItem_Click);
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Enabled = false;
            this.mnuFileSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileSaveAs.Image")));
            this.mnuFileSaveAs.Name = "mnuFileSaveAs";
            this.mnuFileSaveAs.Size = new System.Drawing.Size(114, 22);
            this.mnuFileSaveAs.Text = "Save &As";
            this.mnuFileSaveAs.Click += new System.EventHandler(this.mnuItem_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(111, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileExit.Image")));
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(114, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuItem_Click);
            // 
            // mnuUserInfo
            // 
            this.mnuUserInfo.Enabled = false;
            this.mnuUserInfo.Name = "mnuUserInfo";
            this.mnuUserInfo.Size = new System.Drawing.Size(66, 20);
            this.mnuUserInfo.Text = "&User Info";
            // 
            // ToolStrip
            // 
            this.ToolStrip.BackColor = System.Drawing.Color.Silver;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripSeparator1,
            this.tsNew,
            this.ToolStripSeparator2,
            this.tsOpen,
            this.ToolStripSeparator3,
            this.tsSave,
            this.ToolStripSeparator4,
            this.tsExit,
            this.ToolStripSeparator5});
            this.ToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip.Size = new System.Drawing.Size(792, 25);
            this.ToolStrip.TabIndex = 67;
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsNew
            // 
            this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNew.Enabled = false;
            this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(23, 22);
            this.tsNew.Text = "New";
            this.tsNew.Click += new System.EventHandler(this.ToolStrip_ItemClicked);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Enabled = false;
            this.tsOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsOpen.Image")));
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.Text = "Open";
            this.tsOpen.Click += new System.EventHandler(this.ToolStrip_ItemClicked);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Enabled = false;
            this.tsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsSave.Image")));
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.Text = "Save";
            this.tsSave.Click += new System.EventHandler(this.ToolStrip_ItemClicked);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsExit
            // 
            this.tsExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsExit.Image = ((System.Drawing.Image)(resources.GetObject("tsExit.Image")));
            this.tsExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(23, 22);
            this.tsExit.Text = "Close";
            this.tsExit.Click += new System.EventHandler(this.ToolStrip_ItemClicked);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.BackColor = System.Drawing.Color.White;
            this.GroupBox1.Controls.Add(this.cmdThrustBearingDesgnDetail);
            this.GroupBox1.Controls.Add(this.cmdThrustBearingData);
            this.GroupBox1.Controls.Add(this.cmdCreateFiles);
            this.GroupBox1.Controls.Add(this.cmdEndSealDesgnDetail);
            this.GroupBox1.Controls.Add(this.cmdEndSealData);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.cmdRadialBearingData);
            this.GroupBox1.Controls.Add(this.cmdOpCond);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.cmdProject);
            this.GroupBox1.Controls.Add(this.cmdRadialBearingDesgnDetail);
            this.GroupBox1.Controls.Add(this.cmdExit);
            this.GroupBox1.Location = new System.Drawing.Point(630, 44);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(159, 497);
            this.GroupBox1.TabIndex = 68;
            this.GroupBox1.TabStop = false;
            // 
            // cmdThrustBearingDesgnDetail
            // 
            this.cmdThrustBearingDesgnDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdThrustBearingDesgnDetail.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdThrustBearingDesgnDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdThrustBearingDesgnDetail.Location = new System.Drawing.Point(5, 215);
            this.cmdThrustBearingDesgnDetail.Name = "cmdThrustBearingDesgnDetail";
            this.cmdThrustBearingDesgnDetail.Size = new System.Drawing.Size(151, 32);
            this.cmdThrustBearingDesgnDetail.TabIndex = 71;
            this.cmdThrustBearingDesgnDetail.Text = "Thrust Bearing Details";
            this.cmdThrustBearingDesgnDetail.UseVisualStyleBackColor = false;
            this.cmdThrustBearingDesgnDetail.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdThrustBearingData
            // 
            this.cmdThrustBearingData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdThrustBearingData.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdThrustBearingData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdThrustBearingData.Location = new System.Drawing.Point(4, 113);
            this.cmdThrustBearingData.Name = "cmdThrustBearingData";
            this.cmdThrustBearingData.Size = new System.Drawing.Size(151, 32);
            this.cmdThrustBearingData.TabIndex = 70;
            this.cmdThrustBearingData.Text = "Thrust Bearing";
            this.cmdThrustBearingData.UseVisualStyleBackColor = false;
            this.cmdThrustBearingData.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdCreateFiles
            // 
            this.cmdCreateFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdCreateFiles.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCreateFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCreateFiles.Location = new System.Drawing.Point(5, 284);
            this.cmdCreateFiles.Name = "cmdCreateFiles";
            this.cmdCreateFiles.Size = new System.Drawing.Size(151, 32);
            this.cmdCreateFiles.TabIndex = 7;
            this.cmdCreateFiles.Text = "Inventor Model";
            this.cmdCreateFiles.UseVisualStyleBackColor = false;
            this.cmdCreateFiles.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdEndSealDesgnDetail
            // 
            this.cmdEndSealDesgnDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdEndSealDesgnDetail.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEndSealDesgnDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdEndSealDesgnDetail.Location = new System.Drawing.Point(5, 250);
            this.cmdEndSealDesgnDetail.Name = "cmdEndSealDesgnDetail";
            this.cmdEndSealDesgnDetail.Size = new System.Drawing.Size(151, 32);
            this.cmdEndSealDesgnDetail.TabIndex = 6;
            this.cmdEndSealDesgnDetail.Text = "End Seal Details";
            this.cmdEndSealDesgnDetail.UseVisualStyleBackColor = false;
            this.cmdEndSealDesgnDetail.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdEndSealData
            // 
            this.cmdEndSealData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdEndSealData.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEndSealData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdEndSealData.Location = new System.Drawing.Point(4, 147);
            this.cmdEndSealData.Name = "cmdEndSealData";
            this.cmdEndSealData.Size = new System.Drawing.Size(151, 32);
            this.cmdEndSealData.TabIndex = 3;
            this.cmdEndSealData.Text = "End Seal ";
            this.cmdEndSealData.UseVisualStyleBackColor = false;
            this.cmdEndSealData.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // Label4
            // 
            this.Label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label4.BackColor = System.Drawing.Color.Black;
            this.Label4.Location = new System.Drawing.Point(4, 494);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(154, 2);
            this.Label4.TabIndex = 10;
            this.Label4.Text = "Label4";
            // 
            // Label3
            // 
            this.Label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label3.BackColor = System.Drawing.Color.Black;
            this.Label3.Location = new System.Drawing.Point(157, 6);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(2, 490);
            this.Label3.TabIndex = 9;
            this.Label3.Text = "Label3";
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.BackColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(0, 6);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(158, 2);
            this.Label2.TabIndex = 8;
            this.Label2.Text = "Label2";
            // 
            // cmdRadialBearingData
            // 
            this.cmdRadialBearingData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdRadialBearingData.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRadialBearingData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdRadialBearingData.Location = new System.Drawing.Point(4, 79);
            this.cmdRadialBearingData.Name = "cmdRadialBearingData";
            this.cmdRadialBearingData.Size = new System.Drawing.Size(151, 32);
            this.cmdRadialBearingData.TabIndex = 2;
            this.cmdRadialBearingData.Text = "Radial Bearing ";
            this.cmdRadialBearingData.UseVisualStyleBackColor = false;
            this.cmdRadialBearingData.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdOpCond
            // 
            this.cmdOpCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdOpCond.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOpCond.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOpCond.Location = new System.Drawing.Point(4, 45);
            this.cmdOpCond.Name = "cmdOpCond";
            this.cmdOpCond.Size = new System.Drawing.Size(151, 32);
            this.cmdOpCond.TabIndex = 1;
            this.cmdOpCond.Text = "Operating Conditions";
            this.cmdOpCond.UseVisualStyleBackColor = false;
            this.cmdOpCond.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label1.BackColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(0, 6);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(2, 490);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Label1";
            // 
            // cmdProject
            // 
            this.cmdProject.AllowDrop = true;
            this.cmdProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdProject.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdProject.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdProject.Location = new System.Drawing.Point(4, 10);
            this.cmdProject.Name = "cmdProject";
            this.cmdProject.Size = new System.Drawing.Size(151, 32);
            this.cmdProject.TabIndex = 0;
            this.cmdProject.Text = "Project Details";
            this.cmdProject.UseVisualStyleBackColor = false;
            this.cmdProject.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdRadialBearingDesgnDetail
            // 
            this.cmdRadialBearingDesgnDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdRadialBearingDesgnDetail.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRadialBearingDesgnDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdRadialBearingDesgnDetail.Location = new System.Drawing.Point(4, 181);
            this.cmdRadialBearingDesgnDetail.Name = "cmdRadialBearingDesgnDetail";
            this.cmdRadialBearingDesgnDetail.Size = new System.Drawing.Size(151, 32);
            this.cmdRadialBearingDesgnDetail.TabIndex = 5;
            this.cmdRadialBearingDesgnDetail.Text = "Radial Bearing Details";
            this.cmdRadialBearingDesgnDetail.UseVisualStyleBackColor = false;
            this.cmdRadialBearingDesgnDetail.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdExit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.Image = ((System.Drawing.Image)(resources.GetObject("cmdExit.Image")));
            this.cmdExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdExit.Location = new System.Drawing.Point(5, 317);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(151, 32);
            this.cmdExit.TabIndex = 8;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = false;
            this.cmdExit.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // SBar1
            // 
            this.SBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.SBar1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.SBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SBpanel1,
            this.SBpanel2,
            this.SBpanel3,
            this.SBpanel4,
            this.SBpanel5});
            this.SBar1.Location = new System.Drawing.Point(0, 544);
            this.SBar1.Name = "SBar1";
            this.SBar1.Size = new System.Drawing.Size(840, 22);
            this.SBar1.TabIndex = 69;
            // 
            // SBpanel1
            // 
            this.SBpanel1.AutoSize = false;
            this.SBpanel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SBpanel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.SBpanel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SBpanel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SBpanel1.Name = "SBpanel1";
            this.SBpanel1.Size = new System.Drawing.Size(230, 17);
            // 
            // SBpanel2
            // 
            this.SBpanel2.AutoSize = false;
            this.SBpanel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SBpanel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.SBpanel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SBpanel2.Name = "SBpanel2";
            this.SBpanel2.Size = new System.Drawing.Size(200, 17);
            // 
            // SBpanel3
            // 
            this.SBpanel3.AutoSize = false;
            this.SBpanel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SBpanel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.SBpanel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SBpanel3.Name = "SBpanel3";
            this.SBpanel3.Size = new System.Drawing.Size(190, 17);
            // 
            // SBpanel4
            // 
            this.SBpanel4.AutoSize = false;
            this.SBpanel4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SBpanel4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.SBpanel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SBpanel4.Name = "SBpanel4";
            this.SBpanel4.Size = new System.Drawing.Size(190, 17);
            // 
            // SBpanel5
            // 
            this.SBpanel5.AutoSize = false;
            this.SBpanel5.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SBpanel5.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.SBpanel5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SBpanel5.Name = "SBpanel5";
            this.SBpanel5.Size = new System.Drawing.Size(4, 17);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(792, 567);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.SBar1);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BearingCAD Version 2.1                 Main Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.SBar1.ResumeLayout(false);
            this.SBar1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.MenuStrip MainMenu;
        internal System.Windows.Forms.ToolStripMenuItem mnuFile;
        internal System.Windows.Forms.ToolStripMenuItem mnuFileNew;
        internal System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        internal System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        internal System.Windows.Forms.ToolStripMenuItem mnuFileSaveAs;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        internal System.Windows.Forms.ToolStripMenuItem mnuUserInfo;
        internal System.Windows.Forms.ToolStrip ToolStrip;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton tsNew;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton tsOpen;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripButton tsSave;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ToolStripButton tsExit;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button cmdEndSealData;
        private System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button cmdRadialBearingData;
        internal System.Windows.Forms.Button cmdOpCond;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button cmdProject;
        internal System.Windows.Forms.Button cmdRadialBearingDesgnDetail;
        internal System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.StatusStrip SBar1;
        private System.Windows.Forms.ToolStripStatusLabel SBpanel1;
        private System.Windows.Forms.ToolStripStatusLabel SBpanel2;
        private System.Windows.Forms.ToolStripStatusLabel SBpanel3;
        private System.Windows.Forms.ToolStripStatusLabel SBpanel4;
        internal System.Windows.Forms.Button cmdEndSealDesgnDetail;
        internal System.Windows.Forms.Button cmdCreateFiles;
        private System.Windows.Forms.ToolStripStatusLabel SBpanel5;
        internal System.Windows.Forms.Button cmdThrustBearingDesgnDetail;
        internal System.Windows.Forms.Button cmdThrustBearingData;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}