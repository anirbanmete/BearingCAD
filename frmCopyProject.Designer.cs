namespace BearingCAD20
{
    partial class frmCopyProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopyProject));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.optFiltered = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.cmdFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProjectNo_Suffix_To = new System.Windows.Forms.TextBox();
            this.txtProjectNo_To = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProjectNo_Suffix_From = new System.Windows.Forms.ComboBox();
            this.cmbProjectNo_From = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(430, 228);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.grpFilter);
            this.panel1.Controls.Add(this.cmdFilter);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 226);
            this.panel1.TabIndex = 1;
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(325, 183);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 28);
            this.cmdCancel.TabIndex = 337;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(228, 183);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 336;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.optFiltered);
            this.grpFilter.Controls.Add(this.optAll);
            this.grpFilter.Enabled = false;
            this.grpFilter.Location = new System.Drawing.Point(108, 120);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(148, 36);
            this.grpFilter.TabIndex = 335;
            this.grpFilter.TabStop = false;
            // 
            // optFiltered
            // 
            this.optFiltered.AutoSize = true;
            this.optFiltered.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optFiltered.Location = new System.Drawing.Point(6, 13);
            this.optFiltered.Name = "optFiltered";
            this.optFiltered.Size = new System.Drawing.Size(67, 18);
            this.optFiltered.TabIndex = 335;
            this.optFiltered.TabStop = true;
            this.optFiltered.Text = "Filtered";
            this.optFiltered.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Checked = true;
            this.optAll.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optAll.Location = new System.Drawing.Point(87, 13);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(39, 18);
            this.optAll.TabIndex = 336;
            this.optAll.TabStop = true;
            this.optAll.Text = "All";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.CheckedChanged += new System.EventHandler(this.optAll_CheckedChanged);
            // 
            // cmdFilter
            // 
            this.cmdFilter.BackColor = System.Drawing.Color.Silver;
            this.cmdFilter.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdFilter.Location = new System.Drawing.Point(27, 131);
            this.cmdFilter.Name = "cmdFilter";
            this.cmdFilter.Size = new System.Drawing.Size(75, 23);
            this.cmdFilter.TabIndex = 6;
            this.cmdFilter.Text = "Filter";
            this.cmdFilter.UseVisualStyleBackColor = false;
            this.cmdFilter.Click += new System.EventHandler(this.cmdFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProjectNo_Suffix_To);
            this.groupBox1.Controls.Add(this.txtProjectNo_To);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbProjectNo_Suffix_From);
            this.groupBox1.Controls.Add(this.cmbProjectNo_From);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(21, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtProjectNo_Suffix_To
            // 
            this.txtProjectNo_Suffix_To.Location = new System.Drawing.Point(334, 34);
            this.txtProjectNo_Suffix_To.Name = "txtProjectNo_Suffix_To";
            this.txtProjectNo_Suffix_To.ReadOnly = true;
            this.txtProjectNo_Suffix_To.Size = new System.Drawing.Size(42, 20);
            this.txtProjectNo_Suffix_To.TabIndex = 338;
            // 
            // txtProjectNo_To
            // 
            this.txtProjectNo_To.Location = new System.Drawing.Point(207, 34);
            this.txtProjectNo_To.Name = "txtProjectNo_To";
            this.txtProjectNo_To.ReadOnly = true;
            this.txtProjectNo_To.Size = new System.Drawing.Size(121, 20);
            this.txtProjectNo_To.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "From";
            // 
            // cmbProjectNo_Suffix_From
            // 
            this.cmbProjectNo_Suffix_From.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectNo_Suffix_From.FormattingEnabled = true;
            this.cmbProjectNo_Suffix_From.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.cmbProjectNo_Suffix_From.Location = new System.Drawing.Point(133, 33);
            this.cmbProjectNo_Suffix_From.Name = "cmbProjectNo_Suffix_From";
            this.cmbProjectNo_Suffix_From.Size = new System.Drawing.Size(42, 22);
            this.cmbProjectNo_Suffix_From.TabIndex = 3;
            //this.cmbProjectNo_Suffix_From.SelectedIndexChanged += new System.EventHandler(this.cmbProjectNo_Suffix_From_SelectedIndexChanged);
            // 
            // cmbProjectNo_From
            // 
            this.cmbProjectNo_From.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectNo_From.FormattingEnabled = true;
            this.cmbProjectNo_From.Items.AddRange(new object[] {
            "2958",
            "3099"});
            this.cmbProjectNo_From.Location = new System.Drawing.Point(6, 33);
            this.cmbProjectNo_From.Name = "cmbProjectNo_From";
            this.cmbProjectNo_From.Size = new System.Drawing.Size(121, 22);
            this.cmbProjectNo_From.TabIndex = 2;
            this.cmbProjectNo_From.SelectedIndexChanged += new System.EventHandler(this.cmbProjectNo_From_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Project Number:";
            // 
            // frmCopyProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(432, 231);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCopyProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Project";
            this.Activated += new System.EventHandler(this.frmCopyProject_Activated);
            this.Load += new System.EventHandler(this.frmCopyProject_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProjectNo_Suffix_From;
        private System.Windows.Forms.ComboBox cmbProjectNo_From;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.RadioButton optFiltered;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TextBox txtProjectNo_Suffix_To;
        private System.Windows.Forms.TextBox txtProjectNo_To;
    }
}