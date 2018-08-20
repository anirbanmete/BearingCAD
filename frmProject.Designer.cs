namespace BearingCAD21
{
    partial class frmProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProject));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbEndConfig_Back = new System.Windows.Forms.ComboBox();
            this.cmbEndConfig_Front = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtCustomer_Name = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Label0 = new System.Windows.Forms.Label();
            this.txtNo = new System.Windows.Forms.TextBox();
            this.cmbUnitSystem = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(1, 1);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(552, 245);
            this.lblBorder.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.cmbEndConfig_Back);
            this.panel1.Controls.Add(this.cmbEndConfig_Front);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txtCustomer_Name);
            this.panel1.Controls.Add(this.cmbType);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.cmbProduct);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Label0);
            this.panel1.Controls.Add(this.txtNo);
            this.panel1.Controls.Add(this.cmbUnitSystem);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 243);
            this.panel1.TabIndex = 7;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label19.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(341, 130);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 17);
            this.label19.TabIndex = 330;
            this.label19.Text = "Back";
            this.label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(157, 130);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 17);
            this.label18.TabIndex = 329;
            this.label18.Text = "Front";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmbEndConfig_Back
            // 
            this.cmbEndConfig_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndConfig_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndConfig_Back.FormattingEnabled = true;
            this.cmbEndConfig_Back.Items.AddRange(new object[] {
            "End Seal",
            "T/L Thrust Bearing"});
            this.cmbEndConfig_Back.Location = new System.Drawing.Point(307, 150);
            this.cmbEndConfig_Back.Name = "cmbEndConfig_Back";
            this.cmbEndConfig_Back.Size = new System.Drawing.Size(132, 21);
            this.cmbEndConfig_Back.TabIndex = 328;
            this.cmbEndConfig_Back.SelectedIndexChanged += new System.EventHandler(this.cmbEndConfig_SelectedIndexChanged);
            // 
            // cmbEndConfig_Front
            // 
            this.cmbEndConfig_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndConfig_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndConfig_Front.FormattingEnabled = true;
            this.cmbEndConfig_Front.Location = new System.Drawing.Point(123, 150);
            this.cmbEndConfig_Front.Name = "cmbEndConfig_Front";
            this.cmbEndConfig_Front.Size = new System.Drawing.Size(132, 21);
            this.cmbEndConfig_Front.TabIndex = 327;
            this.cmbEndConfig_Front.SelectedIndexChanged += new System.EventHandler(this.cmbEndConfig_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label15.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(5, 150);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 17);
            this.label15.TabIndex = 326;
            this.label15.Text = "End Configuration";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustomer_Name
            // 
            this.txtCustomer_Name.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer_Name.Location = new System.Drawing.Point(123, 92);
            this.txtCustomer_Name.Name = "txtCustomer_Name";
            this.txtCustomer_Name.Size = new System.Drawing.Size(216, 21);
            this.txtCustomer_Name.TabIndex = 3;
            this.txtCustomer_Name.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(287, 15);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(107, 21);
            this.cmbType.TabIndex = 1;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(236, 18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 17);
            this.label14.TabIndex = 300;
            this.label14.Text = "Type";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(123, 15);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(95, 21);
            this.cmbProduct.TabIndex = 0;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(27, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 17);
            this.label13.TabIndex = 298;
            this.label13.Text = "Product";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(457, 199);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 278;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(369, 199);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 277;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(403, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 263;
            this.label3.Text = "Unit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 259;
            this.label1.Text = "Number";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label0
            // 
            this.Label0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label0.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label0.Location = new System.Drawing.Point(53, 94);
            this.Label0.Name = "Label0";
            this.Label0.Size = new System.Drawing.Size(64, 17);
            this.Label0.TabIndex = 257;
            this.Label0.Text = "Customer";
            this.Label0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNo
            // 
            this.txtNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNo.Location = new System.Drawing.Point(123, 57);
            this.txtNo.Name = "txtNo";
            this.txtNo.Size = new System.Drawing.Size(95, 21);
            this.txtNo.TabIndex = 2;
            this.txtNo.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            // 
            // cmbUnitSystem
            // 
            this.cmbUnitSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnitSystem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUnitSystem.FormattingEnabled = true;
            this.cmbUnitSystem.Location = new System.Drawing.Point(457, 15);
            this.cmbUnitSystem.Name = "cmbUnitSystem";
            this.cmbUnitSystem.Size = new System.Drawing.Size(80, 21);
            this.cmbUnitSystem.TabIndex = 5;
            this.cmbUnitSystem.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(554, 249);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Details";
            this.Activated += new System.EventHandler(this.frmProject_Activated);
            this.Load += new System.EventHandler(this.frmProject_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label0;
        private System.Windows.Forms.ComboBox cmbUnitSystem;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TextBox txtNo;
        private System.Windows.Forms.ComboBox cmbProduct;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtCustomer_Name;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cmbEndConfig_Back;
        private System.Windows.Forms.ComboBox cmbEndConfig_Front;
    }
}