namespace BearingCAD20
{
    partial class frmFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilter));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSelectData = new System.Windows.Forms.TextBox();
            this.cmdAddOperator = new System.Windows.Forms.Button();
            this.cmbAddOperator = new System.Windows.Forms.ComboBox();
            this.cmbSelectData = new System.Windows.Forms.ComboBox();
            this.txtCriteria = new System.Windows.Forms.TextBox();
            this.cmdAddCriteria = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbCompareOperator = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmdRem = new System.Windows.Forms.Button();
            this.lblSelectData = new System.Windows.Forms.Label();
            this.lblSelectedFields = new System.Windows.Forms.Label();
            this.lstSelectedFields = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstAvailableFields = new System.Windows.Forms.ListBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cmdReset = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBorder.BackColor = System.Drawing.SystemColors.ControlText;
            this.lblBorder.Location = new System.Drawing.Point(3, 3);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(680, 309);
            this.lblBorder.TabIndex = 68;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Controls.Add(this.txtSelectData);
            this.panel1.Controls.Add(this.cmdAddOperator);
            this.panel1.Controls.Add(this.cmbAddOperator);
            this.panel1.Controls.Add(this.cmbSelectData);
            this.panel1.Controls.Add(this.txtCriteria);
            this.panel1.Controls.Add(this.cmdAddCriteria);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbCompareOperator);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.cmdRem);
            this.panel1.Controls.Add(this.lblSelectData);
            this.panel1.Controls.Add(this.lblSelectedFields);
            this.panel1.Controls.Add(this.lstSelectedFields);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lstAvailableFields);
            this.panel1.Controls.Add(this.cmdAdd);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(678, 307);
            this.panel1.TabIndex = 69;
            // 
            // txtSelectData
            // 
            this.txtSelectData.Location = new System.Drawing.Point(489, 39);
            this.txtSelectData.Name = "txtSelectData";
            this.txtSelectData.Size = new System.Drawing.Size(113, 20);
            this.txtSelectData.TabIndex = 123;
            // 
            // cmdAddOperator
            // 
            this.cmdAddOperator.BackColor = System.Drawing.Color.Silver;
            this.cmdAddOperator.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAddOperator.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAddOperator.Location = new System.Drawing.Point(531, 142);
            this.cmdAddOperator.Name = "cmdAddOperator";
            this.cmdAddOperator.Size = new System.Drawing.Size(121, 30);
            this.cmdAddOperator.TabIndex = 122;
            this.cmdAddOperator.Text = "&Add Operator";
            this.cmdAddOperator.UseVisualStyleBackColor = false;
            this.cmdAddOperator.Click += new System.EventHandler(this.cmdAddOperator_Click);
            // 
            // cmbAddOperator
            // 
            this.cmbAddOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddOperator.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAddOperator.FormattingEnabled = true;
            this.cmbAddOperator.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.cmbAddOperator.Location = new System.Drawing.Point(381, 142);
            this.cmbAddOperator.Name = "cmbAddOperator";
            this.cmbAddOperator.Size = new System.Drawing.Size(50, 22);
            this.cmbAddOperator.TabIndex = 121;
            // 
            // cmbSelectData
            // 
            this.cmbSelectData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectData.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSelectData.FormattingEnabled = true;
            this.cmbSelectData.Location = new System.Drawing.Point(438, 40);
            this.cmbSelectData.Name = "cmbSelectData";
            this.cmbSelectData.Size = new System.Drawing.Size(214, 22);
            this.cmbSelectData.TabIndex = 120;
            // 
            // txtCriteria
            // 
            this.txtCriteria.BackColor = System.Drawing.Color.White;
            this.txtCriteria.Location = new System.Drawing.Point(22, 206);
            this.txtCriteria.Multiline = true;
            this.txtCriteria.Name = "txtCriteria";
            this.txtCriteria.ReadOnly = true;
            this.txtCriteria.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCriteria.Size = new System.Drawing.Size(334, 85);
            this.txtCriteria.TabIndex = 118;
            // 
            // cmdAddCriteria
            // 
            this.cmdAddCriteria.BackColor = System.Drawing.Color.Silver;
            this.cmdAddCriteria.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAddCriteria.Image = ((System.Drawing.Image)(resources.GetObject("cmdAddCriteria.Image")));
            this.cmdAddCriteria.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAddCriteria.Location = new System.Drawing.Point(531, 83);
            this.cmdAddCriteria.Name = "cmdAddCriteria";
            this.cmdAddCriteria.Size = new System.Drawing.Size(121, 30);
            this.cmdAddCriteria.TabIndex = 117;
            this.cmdAddCriteria.Text = "    &Add Criterion";
            this.cmdAddCriteria.UseVisualStyleBackColor = false;
            this.cmdAddCriteria.Click += new System.EventHandler(this.cmdAddCriteria_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(378, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 18);
            this.label3.TabIndex = 115;
            this.label3.Text = "Operator";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbCompareOperator
            // 
            this.cmbCompareOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompareOperator.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCompareOperator.FormattingEnabled = true;
            this.cmbCompareOperator.Location = new System.Drawing.Point(382, 40);
            this.cmbCompareOperator.Name = "cmbCompareOperator";
            this.cmbCompareOperator.Size = new System.Drawing.Size(50, 22);
            this.cmbCompareOperator.TabIndex = 114;
            this.cmbCompareOperator.SelectedIndexChanged += new System.EventHandler(this.cmbCompareOperator_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Silver;
            this.button3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(172, 107);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 25);
            this.button3.TabIndex = 113;
            this.button3.Text = "<<";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Silver;
            this.button2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(172, 138);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 25);
            this.button2.TabIndex = 112;
            this.button2.Text = ">>";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            // 
            // cmdRem
            // 
            this.cmdRem.BackColor = System.Drawing.Color.Silver;
            this.cmdRem.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdRem.Location = new System.Drawing.Point(172, 71);
            this.cmdRem.Name = "cmdRem";
            this.cmdRem.Size = new System.Drawing.Size(45, 25);
            this.cmdRem.TabIndex = 111;
            this.cmdRem.Text = "<";
            this.cmdRem.UseVisualStyleBackColor = false;
            this.cmdRem.Click += new System.EventHandler(this.cmdRem_Click);
            // 
            // lblSelectData
            // 
            this.lblSelectData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectData.Location = new System.Drawing.Point(499, 19);
            this.lblSelectData.Name = "lblSelectData";
            this.lblSelectData.Size = new System.Drawing.Size(92, 18);
            this.lblSelectData.TabIndex = 109;
            this.lblSelectData.Text = "Select Data";
            this.lblSelectData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelectedFields
            // 
            this.lblSelectedFields.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedFields.Location = new System.Drawing.Point(252, 18);
            this.lblSelectedFields.Name = "lblSelectedFields";
            this.lblSelectedFields.Size = new System.Drawing.Size(97, 18);
            this.lblSelectedFields.TabIndex = 108;
            this.lblSelectedFields.Text = "Selected Fields";
            this.lblSelectedFields.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstSelectedFields
            // 
            this.lstSelectedFields.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSelectedFields.FormattingEnabled = true;
            this.lstSelectedFields.HorizontalScrollbar = true;
            this.lstSelectedFields.ItemHeight = 15;
            this.lstSelectedFields.Location = new System.Drawing.Point(225, 39);
            this.lstSelectedFields.Name = "lstSelectedFields";
            this.lstSelectedFields.Size = new System.Drawing.Size(150, 124);
            this.lstSelectedFields.TabIndex = 107;
            this.lstSelectedFields.SelectedIndexChanged += new System.EventHandler(this.lstSelectedFields_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 18);
            this.label1.TabIndex = 106;
            this.label1.Text = "Available Fields";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstAvailableFields
            // 
            this.lstAvailableFields.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAvailableFields.FormattingEnabled = true;
            this.lstAvailableFields.HorizontalScrollbar = true;
            this.lstAvailableFields.ItemHeight = 15;
            this.lstAvailableFields.Location = new System.Drawing.Point(16, 39);
            this.lstAvailableFields.Name = "lstAvailableFields";
            this.lstAvailableFields.Size = new System.Drawing.Size(150, 124);
            this.lstAvailableFields.TabIndex = 105;
            // 
            // cmdAdd
            // 
            this.cmdAdd.BackColor = System.Drawing.Color.Silver;
            this.cmdAdd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAdd.Location = new System.Drawing.Point(172, 40);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(45, 25);
            this.cmdAdd.TabIndex = 98;
            this.cmdAdd.Text = ">";
            this.cmdAdd.UseVisualStyleBackColor = false;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(560, 267);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(92, 30);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(459, 267);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(92, 30);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdPrint);
            this.groupBox1.Controls.Add(this.cmdReset);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(16, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 112);
            this.groupBox1.TabIndex = 126;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Criteria:";
            // 
            // cmdPrint
            // 
            this.cmdPrint.BackColor = System.Drawing.Color.Silver;
            this.cmdPrint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdPrint.Location = new System.Drawing.Point(346, 79);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(70, 25);
            this.cmdPrint.TabIndex = 125;
            this.cmdPrint.Text = "&Print";
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdReset
            // 
            this.cmdReset.BackColor = System.Drawing.Color.Silver;
            this.cmdReset.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdReset.Location = new System.Drawing.Point(346, 48);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(70, 25);
            this.cmdReset.TabIndex = 124;
            this.cmdReset.Text = "&Reset";
            this.cmdReset.UseVisualStyleBackColor = false;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.CalendarFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Location = new System.Drawing.Point(601, 39);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(20, 20);
            this.dtpDate.TabIndex = 127;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // frmFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(686, 314);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter Records";
            this.Load += new System.EventHandler(this.frmFilter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdAddOperator;
        private System.Windows.Forms.ComboBox cmbAddOperator;
        private System.Windows.Forms.ComboBox cmbSelectData;
        private System.Windows.Forms.TextBox txtCriteria;
        private System.Windows.Forms.Button cmdAddCriteria;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbCompareOperator;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button cmdRem;
        private System.Windows.Forms.Label lblSelectData;
        private System.Windows.Forms.Label lblSelectedFields;
        private System.Windows.Forms.ListBox lstSelectedFields;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstAvailableFields;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TextBox txtSelectData;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.Button cmdPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpDate;
    }
}