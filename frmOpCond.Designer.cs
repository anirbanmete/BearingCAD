namespace BearingCAD21
{
    partial class frmOpCond
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpCond));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbOpCond = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpStaticLoad = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtThrust_Load_Front = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtThrust_Load_Back = new System.Windows.Forms.TextBox();
            this.lblStaticLoad_Thrust = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRadial_Load = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtRadial_LoadAng = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpeed_Design = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpRot_Directionality = new System.Windows.Forms.GroupBox();
            this.optRot_Directionality_Bi = new System.Windows.Forms.RadioButton();
            this.optRot_Directionality_Uni = new System.Windows.Forms.RadioButton();
            this.tabOilSupply = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOilSupply_Temp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbOilSupply_Type = new System.Windows.Forms.ComboBox();
            this.lblPress = new System.Windows.Forms.Label();
            this.lblPressUnit = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTempUnit = new System.Windows.Forms.Label();
            this.lblTempDegF = new System.Windows.Forms.Label();
            this.txtOilSupply_Press = new System.Windows.Forms.TextBox();
            this.lblTempDegC = new System.Windows.Forms.Label();
            this.cmbLube_Type = new System.Windows.Forms.ComboBox();
            this.txtOilSupply_Type = new System.Windows.Forms.TextBox();
            this.txtLube_Type = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tbOpCond.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpStaticLoad.SuspendLayout();
            this.grpRot_Directionality.SuspendLayout();
            this.tabOilSupply.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(2, 4);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(472, 328);
            this.lblBorder.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tbOpCond);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Location = new System.Drawing.Point(3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 326);
            this.panel1.TabIndex = 8;
            // 
            // tbOpCond
            // 
            this.tbOpCond.Controls.Add(this.tabGeneral);
            this.tbOpCond.Controls.Add(this.tabOilSupply);
            this.tbOpCond.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOpCond.Location = new System.Drawing.Point(7, 11);
            this.tbOpCond.Name = "tbOpCond";
            this.tbOpCond.SelectedIndex = 0;
            this.tbOpCond.Size = new System.Drawing.Size(449, 266);
            this.tbOpCond.TabIndex = 9;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabGeneral.Controls.Add(this.grpStaticLoad);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.txtSpeed_Design);
            this.tabGeneral.Controls.Add(this.label8);
            this.tabGeneral.Controls.Add(this.grpRot_Directionality);
            this.tabGeneral.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(441, 240);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // grpStaticLoad
            // 
            this.grpStaticLoad.Controls.Add(this.label4);
            this.grpStaticLoad.Controls.Add(this.label3);
            this.grpStaticLoad.Controls.Add(this.txtThrust_Load_Front);
            this.grpStaticLoad.Controls.Add(this.label19);
            this.grpStaticLoad.Controls.Add(this.label23);
            this.grpStaticLoad.Controls.Add(this.txtThrust_Load_Back);
            this.grpStaticLoad.Controls.Add(this.lblStaticLoad_Thrust);
            this.grpStaticLoad.Controls.Add(this.label2);
            this.grpStaticLoad.Controls.Add(this.txtRadial_Load);
            this.grpStaticLoad.Controls.Add(this.label18);
            this.grpStaticLoad.Controls.Add(this.label17);
            this.grpStaticLoad.Controls.Add(this.txtRadial_LoadAng);
            this.grpStaticLoad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpStaticLoad.Location = new System.Drawing.Point(12, 62);
            this.grpStaticLoad.Name = "grpStaticLoad";
            this.grpStaticLoad.Size = new System.Drawing.Size(419, 163);
            this.grpStaticLoad.TabIndex = 280;
            this.grpStaticLoad.TabStop = false;
            this.grpStaticLoad.Text = "Static Load:";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(164, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 19);
            this.label4.TabIndex = 336;
            this.label4.Text = "Back";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(74, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 19);
            this.label3.TabIndex = 335;
            this.label3.Text = "Front";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtThrust_Load_Front
            // 
            this.txtThrust_Load_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThrust_Load_Front.ForeColor = System.Drawing.Color.Black;
            this.txtThrust_Load_Front.Location = new System.Drawing.Point(59, 77);
            this.txtThrust_Load_Front.Name = "txtThrust_Load_Front";
            this.txtThrust_Load_Front.Size = new System.Drawing.Size(68, 21);
            this.txtThrust_Load_Front.TabIndex = 334;
            this.txtThrust_Load_Front.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label19.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(67, 115);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(226, 35);
            this.label19.TabIndex = 330;
            this.label19.Text = "Load Orientation for CCW Shaft Rotation from Casing Split Line.";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(14, 120);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(50, 17);
            this.label23.TabIndex = 331;
            this.label23.Text = "* Note: ";
            // 
            // txtThrust_Load_Back
            // 
            this.txtThrust_Load_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThrust_Load_Back.ForeColor = System.Drawing.Color.Black;
            this.txtThrust_Load_Back.Location = new System.Drawing.Point(145, 77);
            this.txtThrust_Load_Back.Name = "txtThrust_Load_Back";
            this.txtThrust_Load_Back.Size = new System.Drawing.Size(68, 21);
            this.txtThrust_Load_Back.TabIndex = 332;
            this.txtThrust_Load_Back.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblStaticLoad_Thrust
            // 
            this.lblStaticLoad_Thrust.AutoSize = true;
            this.lblStaticLoad_Thrust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblStaticLoad_Thrust.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaticLoad_Thrust.Location = new System.Drawing.Point(6, 79);
            this.lblStaticLoad_Thrust.Name = "lblStaticLoad_Thrust";
            this.lblStaticLoad_Thrust.Size = new System.Drawing.Size(48, 13);
            this.lblStaticLoad_Thrust.TabIndex = 333;
            this.lblStaticLoad_Thrust.Text = "Thrust:";
            this.lblStaticLoad_Thrust.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 327;
            this.label2.Text = "Radial";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRadial_Load
            // 
            this.txtRadial_Load.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadial_Load.ForeColor = System.Drawing.Color.Black;
            this.txtRadial_Load.Location = new System.Drawing.Point(57, 26);
            this.txtRadial_Load.Name = "txtRadial_Load";
            this.txtRadial_Load.Size = new System.Drawing.Size(68, 21);
            this.txtRadial_Load.TabIndex = 325;
            this.txtRadial_Load.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(382, 27);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 19);
            this.label18.TabIndex = 329;
            this.label18.Text = "deg.";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label17.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(239, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(64, 17);
            this.label17.TabIndex = 328;
            this.label17.Text = "Angle *";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRadial_LoadAng
            // 
            this.txtRadial_LoadAng.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRadial_LoadAng.Location = new System.Drawing.Point(309, 26);
            this.txtRadial_LoadAng.Name = "txtRadial_LoadAng";
            this.txtRadial_LoadAng.Size = new System.Drawing.Size(68, 21);
            this.txtRadial_LoadAng.TabIndex = 326;
            this.txtRadial_LoadAng.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRadial_LoadAng.TextChanged += new System.EventHandler(this.txtLoadAngle_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 259;
            this.label1.Text = "Speed";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSpeed_Design
            // 
            this.txtSpeed_Design.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed_Design.ForeColor = System.Drawing.Color.Black;
            this.txtSpeed_Design.Location = new System.Drawing.Point(71, 18);
            this.txtSpeed_Design.Name = "txtSpeed_Design";
            this.txtSpeed_Design.Size = new System.Drawing.Size(68, 21);
            this.txtSpeed_Design.TabIndex = 1;
            this.txtSpeed_Design.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(142, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 17);
            this.label8.TabIndex = 279;
            this.label8.Text = "RPM";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpRot_Directionality
            // 
            this.grpRot_Directionality.Controls.Add(this.optRot_Directionality_Bi);
            this.grpRot_Directionality.Controls.Add(this.optRot_Directionality_Uni);
            this.grpRot_Directionality.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRot_Directionality.ForeColor = System.Drawing.Color.Black;
            this.grpRot_Directionality.Location = new System.Drawing.Point(193, 9);
            this.grpRot_Directionality.Name = "grpRot_Directionality";
            this.grpRot_Directionality.Size = new System.Drawing.Size(125, 45);
            this.grpRot_Directionality.TabIndex = 11;
            this.grpRot_Directionality.TabStop = false;
            this.grpRot_Directionality.Text = "Directionality";
            // 
            // optRot_Directionality_Bi
            // 
            this.optRot_Directionality_Bi.AutoSize = true;
            this.optRot_Directionality_Bi.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optRot_Directionality_Bi.Location = new System.Drawing.Point(78, 19);
            this.optRot_Directionality_Bi.Name = "optRot_Directionality_Bi";
            this.optRot_Directionality_Bi.Size = new System.Drawing.Size(36, 17);
            this.optRot_Directionality_Bi.TabIndex = 13;
            this.optRot_Directionality_Bi.Text = "Bi";
            this.optRot_Directionality_Bi.UseVisualStyleBackColor = true;
            this.optRot_Directionality_Bi.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optRot_Directionality_Uni
            // 
            this.optRot_Directionality_Uni.AutoSize = true;
            this.optRot_Directionality_Uni.Checked = true;
            this.optRot_Directionality_Uni.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optRot_Directionality_Uni.Location = new System.Drawing.Point(11, 19);
            this.optRot_Directionality_Uni.Name = "optRot_Directionality_Uni";
            this.optRot_Directionality_Uni.Size = new System.Drawing.Size(43, 17);
            this.optRot_Directionality_Uni.TabIndex = 12;
            this.optRot_Directionality_Uni.TabStop = true;
            this.optRot_Directionality_Uni.Text = "Uni";
            this.optRot_Directionality_Uni.UseVisualStyleBackColor = true;
            this.optRot_Directionality_Uni.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // tabOilSupply
            // 
            this.tabOilSupply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabOilSupply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabOilSupply.Controls.Add(this.label12);
            this.tabOilSupply.Controls.Add(this.txtOilSupply_Temp);
            this.tabOilSupply.Controls.Add(this.label5);
            this.tabOilSupply.Controls.Add(this.cmbOilSupply_Type);
            this.tabOilSupply.Controls.Add(this.lblPress);
            this.tabOilSupply.Controls.Add(this.lblPressUnit);
            this.tabOilSupply.Controls.Add(this.lblTemp);
            this.tabOilSupply.Controls.Add(this.label7);
            this.tabOilSupply.Controls.Add(this.lblTempUnit);
            this.tabOilSupply.Controls.Add(this.lblTempDegF);
            this.tabOilSupply.Controls.Add(this.txtOilSupply_Press);
            this.tabOilSupply.Controls.Add(this.lblTempDegC);
            this.tabOilSupply.Controls.Add(this.cmbLube_Type);
            this.tabOilSupply.Controls.Add(this.txtOilSupply_Type);
            this.tabOilSupply.Controls.Add(this.txtLube_Type);
            this.tabOilSupply.Location = new System.Drawing.Point(4, 22);
            this.tabOilSupply.Name = "tabOilSupply";
            this.tabOilSupply.Padding = new System.Windows.Forms.Padding(3);
            this.tabOilSupply.Size = new System.Drawing.Size(441, 240);
            this.tabOilSupply.TabIndex = 1;
            this.tabOilSupply.Text = "Oil Supply";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(11, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 20);
            this.label12.TabIndex = 321;
            this.label12.Text = "Lubricant";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOilSupply_Temp
            // 
            this.txtOilSupply_Temp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOilSupply_Temp.Location = new System.Drawing.Point(283, 103);
            this.txtOilSupply_Temp.Name = "txtOilSupply_Temp";
            this.txtOilSupply_Temp.Size = new System.Drawing.Size(50, 21);
            this.txtOilSupply_Temp.TabIndex = 314;
            this.txtOilSupply_Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(38, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 17);
            this.label5.TabIndex = 316;
            this.label5.Text = "Type";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbOilSupply_Type
            // 
            this.cmbOilSupply_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOilSupply_Type.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOilSupply_Type.FormattingEnabled = true;
            this.cmbOilSupply_Type.Location = new System.Drawing.Point(86, 68);
            this.cmbOilSupply_Type.Name = "cmbOilSupply_Type";
            this.cmbOilSupply_Type.Size = new System.Drawing.Size(99, 21);
            this.cmbOilSupply_Type.TabIndex = 311;
            this.cmbOilSupply_Type.SelectedIndexChanged += new System.EventHandler(this.cmbOilSupply_Type_SelectedIndexChanged);
            // 
            // lblPress
            // 
            this.lblPress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblPress.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPress.Location = new System.Drawing.Point(214, 69);
            this.lblPress.Name = "lblPress";
            this.lblPress.Size = new System.Drawing.Size(63, 17);
            this.lblPress.TabIndex = 317;
            this.lblPress.Text = "Pressure";
            this.lblPress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPressUnit
            // 
            this.lblPressUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblPressUnit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPressUnit.Location = new System.Drawing.Point(334, 69);
            this.lblPressUnit.Name = "lblPressUnit";
            this.lblPressUnit.Size = new System.Drawing.Size(31, 19);
            this.lblPressUnit.TabIndex = 318;
            this.lblPressUnit.Text = "psig";
            this.lblPressUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTemp
            // 
            this.lblTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTemp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemp.Location = new System.Drawing.Point(235, 105);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(42, 17);
            this.lblTemp.TabIndex = 320;
            this.lblTemp.Text = "Temp";
            this.lblTemp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(167, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 17);
            this.label7.TabIndex = 327;
            this.label7.Text = "Type";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTempUnit
            // 
            this.lblTempUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTempUnit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempUnit.Location = new System.Drawing.Point(334, 105);
            this.lblTempUnit.Name = "lblTempUnit";
            this.lblTempUnit.Size = new System.Drawing.Size(31, 18);
            this.lblTempUnit.TabIndex = 322;
            this.lblTempUnit.Text = "F";
            this.lblTempUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTempDegF
            // 
            this.lblTempDegF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTempDegF.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempDegF.Location = new System.Drawing.Point(337, 103);
            this.lblTempDegF.Name = "lblTempDegF";
            this.lblTempDegF.Size = new System.Drawing.Size(8, 8);
            this.lblTempDegF.TabIndex = 323;
            this.lblTempDegF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOilSupply_Press
            // 
            this.txtOilSupply_Press.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOilSupply_Press.Location = new System.Drawing.Point(283, 68);
            this.txtOilSupply_Press.Name = "txtOilSupply_Press";
            this.txtOilSupply_Press.Size = new System.Drawing.Size(50, 21);
            this.txtOilSupply_Press.TabIndex = 312;
            this.txtOilSupply_Press.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTempDegC
            // 
            this.lblTempDegC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTempDegC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempDegC.Location = new System.Drawing.Point(422, 103);
            this.lblTempDegC.Name = "lblTempDegC";
            this.lblTempDegC.Size = new System.Drawing.Size(8, 8);
            this.lblTempDegC.TabIndex = 325;
            this.lblTempDegC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLube_Type
            // 
            this.cmbLube_Type.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLube_Type.FormattingEnabled = true;
            this.cmbLube_Type.Location = new System.Drawing.Point(86, 32);
            this.cmbLube_Type.Name = "cmbLube_Type";
            this.cmbLube_Type.Size = new System.Drawing.Size(178, 21);
            this.cmbLube_Type.TabIndex = 310;
            this.cmbLube_Type.SelectedIndexChanged += new System.EventHandler(this.cmbLube_Type_SelectedIndexChanged);
            // 
            // txtOilSupply_Type
            // 
            this.txtOilSupply_Type.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOilSupply_Type.Location = new System.Drawing.Point(86, 68);
            this.txtOilSupply_Type.Name = "txtOilSupply_Type";
            this.txtOilSupply_Type.Size = new System.Drawing.Size(99, 21);
            this.txtOilSupply_Type.TabIndex = 334;
            this.txtOilSupply_Type.Visible = false;
            // 
            // txtLube_Type
            // 
            this.txtLube_Type.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLube_Type.Location = new System.Drawing.Point(86, 32);
            this.txtLube_Type.Name = "txtLube_Type";
            this.txtLube_Type.Size = new System.Drawing.Size(178, 21);
            this.txtLube_Type.TabIndex = 333;
            this.txtLube_Type.Visible = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(372, 283);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 21;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(266, 283);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 20;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmOpCond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(475, 334);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmOpCond";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operating Conditions";
            this.Load += new System.EventHandler(this.frmOperCond_Load);
            this.panel1.ResumeLayout(false);
            this.tbOpCond.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.grpStaticLoad.ResumeLayout(false);
            this.grpStaticLoad.PerformLayout();
            this.grpRot_Directionality.ResumeLayout(false);
            this.grpRot_Directionality.PerformLayout();
            this.tabOilSupply.ResumeLayout(false);
            this.tabOilSupply.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSpeed_Design;
        private System.Windows.Forms.GroupBox grpRot_Directionality;
        private System.Windows.Forms.RadioButton optRot_Directionality_Bi;
        private System.Windows.Forms.RadioButton optRot_Directionality_Uni;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox grpStaticLoad;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtThrust_Load_Front;
        private System.Windows.Forms.TextBox txtThrust_Load_Back;
        internal System.Windows.Forms.Label lblStaticLoad_Thrust;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRadial_Load;
        private System.Windows.Forms.Label label23;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtRadial_LoadAng;
        private System.Windows.Forms.TabControl tbOpCond;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabOilSupply;
        private System.Windows.Forms.TextBox txtOilSupply_Type;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtOilSupply_Temp;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbOilSupply_Type;
        internal System.Windows.Forms.Label lblPress;
        internal System.Windows.Forms.Label lblPressUnit;
        internal System.Windows.Forms.Label lblTemp;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label lblTempUnit;
        internal System.Windows.Forms.Label lblTempDegF;
        private System.Windows.Forms.TextBox txtOilSupply_Press;
        internal System.Windows.Forms.Label lblTempDegC;
        private System.Windows.Forms.ComboBox cmbLube_Type;
        private System.Windows.Forms.TextBox txtLube_Type;
    }
}