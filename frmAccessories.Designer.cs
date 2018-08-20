namespace BearingCAD21
{
    partial class frmAccessories
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccessories));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkWireClip_Supplied = new System.Windows.Forms.CheckBox();
            this.chkTempSensor_Supplied = new System.Windows.Forms.CheckBox();
            this.cmbWireClip_Size = new System.Windows.Forms.ComboBox();
            this.lblWCSize = new System.Windows.Forms.Label();
            this.cmbWireClip_Count = new System.Windows.Forms.ComboBox();
            this.cmbTempSensor_Type = new System.Windows.Forms.ComboBox();
            this.lblTempType = new System.Windows.Forms.Label();
            this.cmbTempSensor_Name = new System.Windows.Forms.ComboBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblWireClip = new System.Windows.Forms.Label();
            this.cmbTempSensor_Count = new System.Windows.Forms.ComboBox();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblTempSensor = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 154);
            this.label1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkWireClip_Supplied);
            this.panel1.Controls.Add(this.chkTempSensor_Supplied);
            this.panel1.Controls.Add(this.cmbWireClip_Size);
            this.panel1.Controls.Add(this.lblWCSize);
            this.panel1.Controls.Add(this.cmbWireClip_Count);
            this.panel1.Controls.Add(this.cmbTempSensor_Type);
            this.panel1.Controls.Add(this.lblTempType);
            this.panel1.Controls.Add(this.cmbTempSensor_Name);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.lblWireClip);
            this.panel1.Controls.Add(this.cmbTempSensor_Count);
            this.panel1.Controls.Add(this.lblQty);
            this.panel1.Controls.Add(this.lblTempSensor);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 152);
            this.panel1.TabIndex = 1;
            // 
            // chkWireClip_Supplied
            // 
            this.chkWireClip_Supplied.AutoSize = true;
            this.chkWireClip_Supplied.Location = new System.Drawing.Point(99, 65);
            this.chkWireClip_Supplied.Name = "chkWireClip_Supplied";
            this.chkWireClip_Supplied.Size = new System.Drawing.Size(15, 14);
            this.chkWireClip_Supplied.TabIndex = 4;
            this.chkWireClip_Supplied.UseVisualStyleBackColor = true;
            this.chkWireClip_Supplied.CheckedChanged += new System.EventHandler(this.ChkBox_CheckedChanged);
            // 
            // chkTempSensor_Supplied
            // 
            this.chkTempSensor_Supplied.AutoSize = true;
            this.chkTempSensor_Supplied.Location = new System.Drawing.Point(100, 28);
            this.chkTempSensor_Supplied.Name = "chkTempSensor_Supplied";
            this.chkTempSensor_Supplied.Size = new System.Drawing.Size(15, 14);
            this.chkTempSensor_Supplied.TabIndex = 0;
            this.chkTempSensor_Supplied.UseVisualStyleBackColor = true;
            this.chkTempSensor_Supplied.CheckedChanged += new System.EventHandler(this.ChkBox_CheckedChanged);
            // 
            // cmbWireClip_Size
            // 
            this.cmbWireClip_Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWireClip_Size.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWireClip_Size.FormattingEnabled = true;
            this.cmbWireClip_Size.Items.AddRange(new object[] {
            "S",
            "M",
            "L"});
            this.cmbWireClip_Size.Location = new System.Drawing.Point(358, 61);
            this.cmbWireClip_Size.Name = "cmbWireClip_Size";
            this.cmbWireClip_Size.Size = new System.Drawing.Size(50, 22);
            this.cmbWireClip_Size.TabIndex = 6;
            this.cmbWireClip_Size.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblWCSize
            // 
            this.lblWCSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWCSize.Location = new System.Drawing.Point(311, 63);
            this.lblWCSize.Name = "lblWCSize";
            this.lblWCSize.Size = new System.Drawing.Size(41, 14);
            this.lblWCSize.TabIndex = 391;
            this.lblWCSize.Text = "Size";
            this.lblWCSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbWireClip_Count
            // 
            this.cmbWireClip_Count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWireClip_Count.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWireClip_Count.FormattingEnabled = true;
            this.cmbWireClip_Count.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbWireClip_Count.Location = new System.Drawing.Point(246, 61);
            this.cmbWireClip_Count.Name = "cmbWireClip_Count";
            this.cmbWireClip_Count.Size = new System.Drawing.Size(50, 22);
            this.cmbWireClip_Count.TabIndex = 5;
            this.cmbWireClip_Count.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmbTempSensor_Type
            // 
            this.cmbTempSensor_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTempSensor_Type.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTempSensor_Type.FormattingEnabled = true;
            this.cmbTempSensor_Type.Location = new System.Drawing.Point(358, 24);
            this.cmbTempSensor_Type.Name = "cmbTempSensor_Type";
            this.cmbTempSensor_Type.Size = new System.Drawing.Size(50, 22);
            this.cmbTempSensor_Type.TabIndex = 3;
            this.cmbTempSensor_Type.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblTempType
            // 
            this.lblTempType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempType.Location = new System.Drawing.Point(311, 26);
            this.lblTempType.Name = "lblTempType";
            this.lblTempType.Size = new System.Drawing.Size(41, 14);
            this.lblTempType.TabIndex = 387;
            this.lblTempType.Text = "Type";
            this.lblTempType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbTempSensor_Name
            // 
            this.cmbTempSensor_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTempSensor_Name.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTempSensor_Name.FormattingEnabled = true;
            this.cmbTempSensor_Name.Location = new System.Drawing.Point(140, 24);
            this.cmbTempSensor_Name.Name = "cmbTempSensor_Name";
            this.cmbTempSensor_Name.Size = new System.Drawing.Size(66, 22);
            this.cmbTempSensor_Name.TabIndex = 1;
            this.cmbTempSensor_Name.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(328, 108);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(228, 108);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblWireClip
            // 
            this.lblWireClip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblWireClip.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWireClip.Location = new System.Drawing.Point(18, 62);
            this.lblWireClip.Name = "lblWireClip";
            this.lblWireClip.Size = new System.Drawing.Size(75, 19);
            this.lblWireClip.TabIndex = 380;
            this.lblWireClip.Text = "Wire Clip:";
            this.lblWireClip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbTempSensor_Count
            // 
            this.cmbTempSensor_Count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTempSensor_Count.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTempSensor_Count.FormattingEnabled = true;
            this.cmbTempSensor_Count.Location = new System.Drawing.Point(246, 24);
            this.cmbTempSensor_Count.Name = "cmbTempSensor_Count";
            this.cmbTempSensor_Count.Size = new System.Drawing.Size(50, 22);
            this.cmbTempSensor_Count.TabIndex = 2;
            this.cmbTempSensor_Count.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // lblQty
            // 
            this.lblQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty.Location = new System.Drawing.Point(256, 7);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(30, 14);
            this.lblQty.TabIndex = 378;
            this.lblQty.Text = "Qty.";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTempSensor
            // 
            this.lblTempSensor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTempSensor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempSensor.Location = new System.Drawing.Point(4, 26);
            this.lblTempSensor.Name = "lblTempSensor";
            this.lblTempSensor.Size = new System.Drawing.Size(89, 17);
            this.lblTempSensor.TabIndex = 376;
            this.lblTempSensor.Text = "Temp. Sensor:";
            this.lblTempSensor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmAccessories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(427, 157);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAccessories";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accessories Hardware Supplied";
            this.Load += new System.EventHandler(this.frmAccessories_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Label lblWireClip;
        private System.Windows.Forms.ComboBox cmbTempSensor_Count;
        private System.Windows.Forms.Label lblQty;
        internal System.Windows.Forms.Label lblTempSensor;
        private System.Windows.Forms.ComboBox cmbTempSensor_Name;
        private System.Windows.Forms.ComboBox cmbTempSensor_Type;
        private System.Windows.Forms.Label lblTempType;
        private System.Windows.Forms.ComboBox cmbWireClip_Size;
        private System.Windows.Forms.Label lblWCSize;
        private System.Windows.Forms.ComboBox cmbWireClip_Count;
        private System.Windows.Forms.CheckBox chkWireClip_Supplied;
        private System.Windows.Forms.CheckBox chkTempSensor_Supplied;

    }
}