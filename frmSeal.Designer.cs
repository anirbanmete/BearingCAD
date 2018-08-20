namespace BearingCAD20
{
    partial class frmSeal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeal));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.tbEndSealData = new System.Windows.Forms.TabControl();
            this.tabFront = new System.Windows.Forms.TabPage();
            this.lblDeg_Front = new System.Windows.Forms.Label();
            this.lblBlade_TapAng_Front = new System.Windows.Forms.Label();
            this.cmbBlade_AngTaper_Front = new System.Windows.Forms.ComboBox();
            this.cmbBlade_Count_Front = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMat_Lining_Front = new System.Windows.Forms.ComboBox();
            this.cmbMat_Base_Front = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblMat_LiningT_Front = new System.Windows.Forms.Label();
            this.txtMat_LiningT_Front = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblMat_Lining_Front = new System.Windows.Forms.Label();
            this.txtBlade_T_Front = new System.Windows.Forms.TextBox();
            this.lblBladeThick_Front = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDBore_Range_Max_Front = new System.Windows.Forms.TextBox();
            this.txtDBore_Range_Min_Front = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbType_Front = new System.Windows.Forms.ComboBox();
            this.tabBack = new System.Windows.Forms.TabPage();
            this.cmbBlade_AngTaper_Back = new System.Windows.Forms.ComboBox();
            this.lblDeg_Back = new System.Windows.Forms.Label();
            this.lblBlade_TapAng_Back = new System.Windows.Forms.Label();
            this.cmbBlade_Count_Back = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbMat_Lining_Back = new System.Windows.Forms.ComboBox();
            this.cmbMat_Base_Back = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblMat_LiningT_Back = new System.Windows.Forms.Label();
            this.txtMat_LiningT_Back = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lblMat_Lining_Back = new System.Windows.Forms.Label();
            this.txtBlade_T_Back = new System.Windows.Forms.TextBox();
            this.lblBladeThick_Back = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDBore_Range_Max_Back = new System.Windows.Forms.TextBox();
            this.txtDBore_Range_Min_Back = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cmbType_Back = new System.Windows.Forms.ComboBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tbEndSealData.SuspendLayout();
            this.tabFront.SuspendLayout();
            this.tabBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdPrint);
            this.panel1.Controls.Add(this.tbEndSealData);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 344);
            this.panel1.TabIndex = 1;
            // 
            // cmdPrint
            // 
            this.cmdPrint.BackColor = System.Drawing.Color.Silver;
            this.cmdPrint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPrint.Location = new System.Drawing.Point(13, 304);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(80, 32);
            this.cmdPrint.TabIndex = 468;
            this.cmdPrint.Text = "&Print";
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // tbEndSealData
            // 
            this.tbEndSealData.Controls.Add(this.tabFront);
            this.tbEndSealData.Controls.Add(this.tabBack);
            this.tbEndSealData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEndSealData.Location = new System.Drawing.Point(9, 9);
            this.tbEndSealData.Name = "tbEndSealData";
            this.tbEndSealData.SelectedIndex = 0;
            this.tbEndSealData.Size = new System.Drawing.Size(406, 284);
            this.tbEndSealData.TabIndex = 0;
            // 
            // tabFront
            // 
            this.tabFront.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabFront.Controls.Add(this.lblDeg_Front);
            this.tabFront.Controls.Add(this.lblBlade_TapAng_Front);
            this.tabFront.Controls.Add(this.cmbBlade_AngTaper_Front);
            this.tabFront.Controls.Add(this.cmbBlade_Count_Front);
            this.tabFront.Controls.Add(this.label10);
            this.tabFront.Controls.Add(this.label4);
            this.tabFront.Controls.Add(this.cmbMat_Lining_Front);
            this.tabFront.Controls.Add(this.cmbMat_Base_Front);
            this.tabFront.Controls.Add(this.label9);
            this.tabFront.Controls.Add(this.lblMat_LiningT_Front);
            this.tabFront.Controls.Add(this.txtMat_LiningT_Front);
            this.tabFront.Controls.Add(this.label8);
            this.tabFront.Controls.Add(this.lblMat_Lining_Front);
            this.tabFront.Controls.Add(this.txtBlade_T_Front);
            this.tabFront.Controls.Add(this.lblBladeThick_Front);
            this.tabFront.Controls.Add(this.label7);
            this.tabFront.Controls.Add(this.label5);
            this.tabFront.Controls.Add(this.txtDBore_Range_Max_Front);
            this.tabFront.Controls.Add(this.txtDBore_Range_Min_Front);
            this.tabFront.Controls.Add(this.label3);
            this.tabFront.Controls.Add(this.label2);
            this.tabFront.Controls.Add(this.cmbType_Front);
            this.tabFront.Location = new System.Drawing.Point(4, 24);
            this.tabFront.Name = "tabFront";
            this.tabFront.Padding = new System.Windows.Forms.Padding(3);
            this.tabFront.Size = new System.Drawing.Size(398, 256);
            this.tabFront.TabIndex = 0;
            this.tabFront.Text = "Front";
            // 
            // lblDeg_Front
            // 
            this.lblDeg_Front.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblDeg_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeg_Front.Location = new System.Drawing.Point(327, 125);
            this.lblDeg_Front.Name = "lblDeg_Front";
            this.lblDeg_Front.Size = new System.Drawing.Size(32, 19);
            this.lblDeg_Front.TabIndex = 475;
            this.lblDeg_Front.Text = "deg.";
            this.lblDeg_Front.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBlade_TapAng_Front
            // 
            this.lblBlade_TapAng_Front.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblBlade_TapAng_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlade_TapAng_Front.Location = new System.Drawing.Point(270, 107);
            this.lblBlade_TapAng_Front.Name = "lblBlade_TapAng_Front";
            this.lblBlade_TapAng_Front.Size = new System.Drawing.Size(68, 17);
            this.lblBlade_TapAng_Front.TabIndex = 474;
            this.lblBlade_TapAng_Front.Text = "Taper Ang.";
            this.lblBlade_TapAng_Front.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbBlade_AngTaper_Front
            // 
            this.cmbBlade_AngTaper_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_AngTaper_Front.FormattingEnabled = true;
            this.cmbBlade_AngTaper_Front.Location = new System.Drawing.Point(285, 125);
            this.cmbBlade_AngTaper_Front.Name = "cmbBlade_AngTaper_Front";
            this.cmbBlade_AngTaper_Front.Size = new System.Drawing.Size(38, 22);
            this.cmbBlade_AngTaper_Front.TabIndex = 457;
            this.cmbBlade_AngTaper_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.cmbBlade_AngTaper_Front.TextChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbBlade_Count_Front
            // 
            this.cmbBlade_Count_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlade_Count_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_Count_Front.FormattingEnabled = true;
            this.cmbBlade_Count_Front.Location = new System.Drawing.Point(79, 126);
            this.cmbBlade_Count_Front.Name = "cmbBlade_Count_Front";
            this.cmbBlade_Count_Front.Size = new System.Drawing.Size(39, 22);
            this.cmbBlade_Count_Front.TabIndex = 455;
            this.cmbBlade_Count_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(106, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 17);
            this.label10.TabIndex = 473;
            this.label10.Text = "Min";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(198, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 472;
            this.label4.Text = "Max";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbMat_Lining_Front
            // 
            this.cmbMat_Lining_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Lining_Front.FormattingEnabled = true;
            this.cmbMat_Lining_Front.Location = new System.Drawing.Point(309, 186);
            this.cmbMat_Lining_Front.Name = "cmbMat_Lining_Front";
            this.cmbMat_Lining_Front.Size = new System.Drawing.Size(67, 22);
            this.cmbMat_Lining_Front.TabIndex = 459;
            this.cmbMat_Lining_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbMat_Base_Front
            // 
            this.cmbMat_Base_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Base_Front.FormattingEnabled = true;
            this.cmbMat_Base_Front.Items.AddRange(new object[] {
            "Steel 4340",
            "Steel 4140",
            "Chrome Copper",
            "Bronze"});
            this.cmbMat_Base_Front.Location = new System.Drawing.Point(79, 189);
            this.cmbMat_Base_Front.Name = "cmbMat_Base_Front";
            this.cmbMat_Base_Front.Size = new System.Drawing.Size(160, 22);
            this.cmbMat_Base_Front.TabIndex = 458;
            this.cmbMat_Base_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(2, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 17);
            this.label9.TabIndex = 471;
            this.label9.Text = "Materials:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMat_LiningT_Front
            // 
            this.lblMat_LiningT_Front.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMat_LiningT_Front.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Front.Location = new System.Drawing.Point(259, 224);
            this.lblMat_LiningT_Front.Name = "lblMat_LiningT_Front";
            this.lblMat_LiningT_Front.Size = new System.Drawing.Size(49, 17);
            this.lblMat_LiningT_Front.TabIndex = 470;
            this.lblMat_LiningT_Front.Text = "Thick";
            this.lblMat_LiningT_Front.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMat_LiningT_Front
            // 
            this.txtMat_LiningT_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_LiningT_Front.Location = new System.Drawing.Point(309, 223);
            this.txtMat_LiningT_Front.Name = "txtMat_LiningT_Front";
            this.txtMat_LiningT_Front.Size = new System.Drawing.Size(67, 20);
            this.txtMat_LiningT_Front.TabIndex = 460;
            this.txtMat_LiningT_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMat_LiningT_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtMat_LiningT_Front.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 17);
            this.label8.TabIndex = 468;
            this.label8.Text = "Base";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMat_Lining_Front
            // 
            this.lblMat_Lining_Front.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMat_Lining_Front.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_Lining_Front.Location = new System.Drawing.Point(266, 189);
            this.lblMat_Lining_Front.Name = "lblMat_Lining_Front";
            this.lblMat_Lining_Front.Size = new System.Drawing.Size(42, 17);
            this.lblMat_Lining_Front.TabIndex = 469;
            this.lblMat_Lining_Front.Text = "Lining";
            this.lblMat_Lining_Front.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBlade_T_Front
            // 
            this.txtBlade_T_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlade_T_Front.ForeColor = System.Drawing.Color.Black;
            this.txtBlade_T_Front.Location = new System.Drawing.Point(179, 125);
            this.txtBlade_T_Front.Name = "txtBlade_T_Front";
            this.txtBlade_T_Front.Size = new System.Drawing.Size(59, 20);
            this.txtBlade_T_Front.TabIndex = 456;
            this.txtBlade_T_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlade_T_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtBlade_T_Front.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // lblBladeThick_Front
            // 
            this.lblBladeThick_Front.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblBladeThick_Front.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBladeThick_Front.Location = new System.Drawing.Point(117, 127);
            this.lblBladeThick_Front.Name = "lblBladeThick_Front";
            this.lblBladeThick_Front.Size = new System.Drawing.Size(61, 17);
            this.lblBladeThick_Front.TabIndex = 465;
            this.lblBladeThick_Front.Text = "Thick";
            this.lblBladeThick_Front.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 17);
            this.label7.TabIndex = 464;
            this.label7.Text = "Qty";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 463;
            this.label5.Text = "Blade:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDBore_Range_Max_Front
            // 
            this.txtDBore_Range_Max_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Max_Front.Location = new System.Drawing.Point(179, 63);
            this.txtDBore_Range_Max_Front.Name = "txtDBore_Range_Max_Front";
            this.txtDBore_Range_Max_Front.Size = new System.Drawing.Size(59, 20);
            this.txtDBore_Range_Max_Front.TabIndex = 454;
            this.txtDBore_Range_Max_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Max_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Max_Front.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // txtDBore_Range_Min_Front
            // 
            this.txtDBore_Range_Min_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Min_Front.Location = new System.Drawing.Point(79, 63);
            this.txtDBore_Range_Min_Front.Name = "txtDBore_Range_Min_Front";
            this.txtDBore_Range_Min_Front.Size = new System.Drawing.Size(59, 20);
            this.txtDBore_Range_Min_Front.TabIndex = 453;
            this.txtDBore_Range_Min_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Min_Front.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Min_Front.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 462;
            this.label3.Text = "Bore Dia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 461;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbType_Front
            // 
            this.cmbType_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType_Front.FormattingEnabled = true;
            this.cmbType_Front.Location = new System.Drawing.Point(79, 14);
            this.cmbType_Front.Name = "cmbType_Front";
            this.cmbType_Front.Size = new System.Drawing.Size(81, 22);
            this.cmbType_Front.TabIndex = 452;
            this.cmbType_Front.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // tabBack
            // 
            this.tabBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabBack.Controls.Add(this.cmbBlade_AngTaper_Back);
            this.tabBack.Controls.Add(this.lblDeg_Back);
            this.tabBack.Controls.Add(this.lblBlade_TapAng_Back);
            this.tabBack.Controls.Add(this.cmbBlade_Count_Back);
            this.tabBack.Controls.Add(this.label12);
            this.tabBack.Controls.Add(this.label13);
            this.tabBack.Controls.Add(this.cmbMat_Lining_Back);
            this.tabBack.Controls.Add(this.cmbMat_Base_Back);
            this.tabBack.Controls.Add(this.label14);
            this.tabBack.Controls.Add(this.lblMat_LiningT_Back);
            this.tabBack.Controls.Add(this.txtMat_LiningT_Back);
            this.tabBack.Controls.Add(this.label16);
            this.tabBack.Controls.Add(this.lblMat_Lining_Back);
            this.tabBack.Controls.Add(this.txtBlade_T_Back);
            this.tabBack.Controls.Add(this.lblBladeThick_Back);
            this.tabBack.Controls.Add(this.label19);
            this.tabBack.Controls.Add(this.label20);
            this.tabBack.Controls.Add(this.txtDBore_Range_Max_Back);
            this.tabBack.Controls.Add(this.txtDBore_Range_Min_Back);
            this.tabBack.Controls.Add(this.label21);
            this.tabBack.Controls.Add(this.label22);
            this.tabBack.Controls.Add(this.cmbType_Back);
            this.tabBack.Location = new System.Drawing.Point(4, 24);
            this.tabBack.Name = "tabBack";
            this.tabBack.Padding = new System.Windows.Forms.Padding(3);
            this.tabBack.Size = new System.Drawing.Size(398, 256);
            this.tabBack.TabIndex = 1;
            this.tabBack.Text = "Back";
            // 
            // cmbBlade_AngTaper_Back
            // 
            this.cmbBlade_AngTaper_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_AngTaper_Back.FormattingEnabled = true;
            this.cmbBlade_AngTaper_Back.Location = new System.Drawing.Point(285, 125);
            this.cmbBlade_AngTaper_Back.Name = "cmbBlade_AngTaper_Back";
            this.cmbBlade_AngTaper_Back.Size = new System.Drawing.Size(38, 22);
            this.cmbBlade_AngTaper_Back.TabIndex = 476;
            this.cmbBlade_AngTaper_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.cmbBlade_AngTaper_Back.TextChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblDeg_Back
            // 
            this.lblDeg_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblDeg_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeg_Back.Location = new System.Drawing.Point(327, 125);
            this.lblDeg_Back.Name = "lblDeg_Back";
            this.lblDeg_Back.Size = new System.Drawing.Size(32, 19);
            this.lblDeg_Back.TabIndex = 475;
            this.lblDeg_Back.Text = "deg.";
            this.lblDeg_Back.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBlade_TapAng_Back
            // 
            this.lblBlade_TapAng_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblBlade_TapAng_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlade_TapAng_Back.Location = new System.Drawing.Point(270, 107);
            this.lblBlade_TapAng_Back.Name = "lblBlade_TapAng_Back";
            this.lblBlade_TapAng_Back.Size = new System.Drawing.Size(68, 17);
            this.lblBlade_TapAng_Back.TabIndex = 474;
            this.lblBlade_TapAng_Back.Text = "Taper Ang.";
            this.lblBlade_TapAng_Back.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbBlade_Count_Back
            // 
            this.cmbBlade_Count_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlade_Count_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBlade_Count_Back.FormattingEnabled = true;
            this.cmbBlade_Count_Back.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbBlade_Count_Back.Location = new System.Drawing.Point(79, 126);
            this.cmbBlade_Count_Back.Name = "cmbBlade_Count_Back";
            this.cmbBlade_Count_Back.Size = new System.Drawing.Size(39, 22);
            this.cmbBlade_Count_Back.TabIndex = 455;
            this.cmbBlade_Count_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(106, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 17);
            this.label12.TabIndex = 473;
            this.label12.Text = "Min";
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(198, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 17);
            this.label13.TabIndex = 472;
            this.label13.Text = "Max";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbMat_Lining_Back
            // 
            this.cmbMat_Lining_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Lining_Back.FormattingEnabled = true;
            this.cmbMat_Lining_Back.Location = new System.Drawing.Point(309, 186);
            this.cmbMat_Lining_Back.Name = "cmbMat_Lining_Back";
            this.cmbMat_Lining_Back.Size = new System.Drawing.Size(67, 22);
            this.cmbMat_Lining_Back.TabIndex = 459;
            this.cmbMat_Lining_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbMat_Base_Back
            // 
            this.cmbMat_Base_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMat_Base_Back.FormattingEnabled = true;
            this.cmbMat_Base_Back.Items.AddRange(new object[] {
            "Steel 4340",
            "Steel 4140",
            "Chrome Copper",
            "Bronze"});
            this.cmbMat_Base_Back.Location = new System.Drawing.Point(79, 189);
            this.cmbMat_Base_Back.Name = "cmbMat_Base_Back";
            this.cmbMat_Base_Back.Size = new System.Drawing.Size(160, 22);
            this.cmbMat_Base_Back.TabIndex = 458;
            this.cmbMat_Base_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(2, 160);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 17);
            this.label14.TabIndex = 471;
            this.label14.Text = "Materials:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMat_LiningT_Back
            // 
            this.lblMat_LiningT_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMat_LiningT_Back.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_LiningT_Back.Location = new System.Drawing.Point(259, 224);
            this.lblMat_LiningT_Back.Name = "lblMat_LiningT_Back";
            this.lblMat_LiningT_Back.Size = new System.Drawing.Size(49, 17);
            this.lblMat_LiningT_Back.TabIndex = 470;
            this.lblMat_LiningT_Back.Text = "Thick";
            this.lblMat_LiningT_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMat_LiningT_Back
            // 
            this.txtMat_LiningT_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMat_LiningT_Back.Location = new System.Drawing.Point(309, 223);
            this.txtMat_LiningT_Back.Name = "txtMat_LiningT_Back";
            this.txtMat_LiningT_Back.Size = new System.Drawing.Size(67, 20);
            this.txtMat_LiningT_Back.TabIndex = 460;
            this.txtMat_LiningT_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMat_LiningT_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtMat_LiningT_Back.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label16.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(12, 191);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 17);
            this.label16.TabIndex = 468;
            this.label16.Text = "Base";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMat_Lining_Back
            // 
            this.lblMat_Lining_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMat_Lining_Back.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMat_Lining_Back.Location = new System.Drawing.Point(266, 189);
            this.lblMat_Lining_Back.Name = "lblMat_Lining_Back";
            this.lblMat_Lining_Back.Size = new System.Drawing.Size(42, 17);
            this.lblMat_Lining_Back.TabIndex = 469;
            this.lblMat_Lining_Back.Text = "Lining";
            this.lblMat_Lining_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBlade_T_Back
            // 
            this.txtBlade_T_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlade_T_Back.ForeColor = System.Drawing.Color.Black;
            this.txtBlade_T_Back.Location = new System.Drawing.Point(179, 125);
            this.txtBlade_T_Back.Name = "txtBlade_T_Back";
            this.txtBlade_T_Back.Size = new System.Drawing.Size(59, 20);
            this.txtBlade_T_Back.TabIndex = 456;
            this.txtBlade_T_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlade_T_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtBlade_T_Back.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
            // 
            // lblBladeThick_Back
            // 
            this.lblBladeThick_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblBladeThick_Back.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBladeThick_Back.Location = new System.Drawing.Point(117, 127);
            this.lblBladeThick_Back.Name = "lblBladeThick_Back";
            this.lblBladeThick_Back.Size = new System.Drawing.Size(61, 17);
            this.lblBladeThick_Back.TabIndex = 465;
            this.lblBladeThick_Back.Text = "Thick";
            this.lblBladeThick_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(6, 127);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 17);
            this.label19.TabIndex = 464;
            this.label19.Text = "Qty";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(18, 95);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 17);
            this.label20.TabIndex = 463;
            this.label20.Text = "Blade:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDBore_Range_Max_Back
            // 
            this.txtDBore_Range_Max_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Max_Back.Location = new System.Drawing.Point(179, 63);
            this.txtDBore_Range_Max_Back.Name = "txtDBore_Range_Max_Back";
            this.txtDBore_Range_Max_Back.Size = new System.Drawing.Size(59, 20);
            this.txtDBore_Range_Max_Back.TabIndex = 454;
            this.txtDBore_Range_Max_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Max_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Max_Back.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // txtDBore_Range_Min_Back
            // 
            this.txtDBore_Range_Min_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBore_Range_Min_Back.Location = new System.Drawing.Point(79, 63);
            this.txtDBore_Range_Min_Back.Name = "txtDBore_Range_Min_Back";
            this.txtDBore_Range_Min_Back.Size = new System.Drawing.Size(59, 20);
            this.txtDBore_Range_Min_Back.TabIndex = 453;
            this.txtDBore_Range_Min_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDBore_Range_Min_Back.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.txtDBore_Range_Min_Back.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_KeyPress);
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label21.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(9, 63);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(64, 17);
            this.label21.TabIndex = 462;
            this.label21.Text = "Bore Dia";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label22.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(9, 16);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 17);
            this.label22.TabIndex = 461;
            this.label22.Text = "Type";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbType_Back
            // 
            this.cmbType_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType_Back.FormattingEnabled = true;
            this.cmbType_Back.Items.AddRange(new object[] {
            "Fixed",
            "Floating"});
            this.cmbType_Back.Location = new System.Drawing.Point(79, 14);
            this.cmbType_Back.Name = "cmbType_Back";
            this.cmbType_Back.Size = new System.Drawing.Size(81, 22);
            this.cmbType_Back.TabIndex = 452;
            this.cmbType_Back.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(237, 304);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 466;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(333, 304);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 467;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(422, 346);
            this.label1.TabIndex = 0;
            // 
            // frmSeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(424, 348);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSeal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "End Seal Data";
            this.Load += new System.EventHandler(this.frmSeal_Load);
            this.panel1.ResumeLayout(false);
            this.tbEndSealData.ResumeLayout(false);
            this.tabFront.ResumeLayout(false);
            this.tabFront.PerformLayout();
            this.tabBack.ResumeLayout(false);
            this.tabBack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tbEndSealData;
        private System.Windows.Forms.TabPage tabFront;
        internal System.Windows.Forms.Label lblDeg_Front;
        internal System.Windows.Forms.Label lblBlade_TapAng_Front;
        private System.Windows.Forms.ComboBox cmbBlade_AngTaper_Front;
        private System.Windows.Forms.ComboBox cmbBlade_Count_Front;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMat_Lining_Front;
        private System.Windows.Forms.ComboBox cmbMat_Base_Front;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label lblMat_LiningT_Front;
        private System.Windows.Forms.TextBox txtMat_LiningT_Front;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label lblMat_Lining_Front;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TextBox txtBlade_T_Front;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDBore_Range_Max_Front;
        private System.Windows.Forms.TextBox txtDBore_Range_Min_Front;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbType_Front;
        private System.Windows.Forms.TabPage tabBack;
        internal System.Windows.Forms.Label lblDeg_Back;
        internal System.Windows.Forms.Label lblBlade_TapAng_Back;
        private System.Windows.Forms.ComboBox cmbBlade_Count_Back;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbMat_Lining_Back;
        private System.Windows.Forms.ComboBox cmbMat_Base_Back;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label lblMat_LiningT_Back;
        private System.Windows.Forms.TextBox txtMat_LiningT_Back;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label lblMat_Lining_Back;
        private System.Windows.Forms.TextBox txtBlade_T_Back;
        internal System.Windows.Forms.Label lblBladeThick_Back;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDBore_Range_Max_Back;
        private System.Windows.Forms.TextBox txtDBore_Range_Min_Back;
        internal System.Windows.Forms.Label label21;
        internal System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cmbType_Back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdPrint;
        internal System.Windows.Forms.Label lblBladeThick_Front;
        private System.Windows.Forms.ComboBox cmbBlade_AngTaper_Back;

    }
}