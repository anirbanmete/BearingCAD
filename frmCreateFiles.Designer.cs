namespace BearingCAD20
{
    partial class frmCreateFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateFiles));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdCustomized_SWFiles = new System.Windows.Forms.Button();
            this.lblModified_SWFiles = new System.Windows.Forms.Label();
            this.chkModified_SWFiles = new System.Windows.Forms.CheckBox();
            this.lblIndicator = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSIS = new System.Windows.Forms.Button();
            this.lblFilePath_Project = new System.Windows.Forms.Label();
            this.cmdDesignTbls_SWFiles = new System.Windows.Forms.Button();
            this.cmdBrowse_FilePath_DesignTbls_SWFiles = new System.Windows.Forms.Button();
            this.txtFilePath_DesignTbls_SWFiles = new System.Windows.Forms.TextBox();
            this.lblFilePath_DesignTbls_SWFiles = new System.Windows.Forms.Label();
            this.cmdBrowse_FilePath_Project = new System.Windows.Forms.Button();
            this.txtFilePath_Project = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdDDR = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(563, 345);
            this.label1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdCustomized_SWFiles);
            this.panel1.Controls.Add(this.lblModified_SWFiles);
            this.panel1.Controls.Add(this.chkModified_SWFiles);
            this.panel1.Controls.Add(this.lblIndicator);
            this.panel1.Controls.Add(this.lblNote);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdSIS);
            this.panel1.Controls.Add(this.lblFilePath_Project);
            this.panel1.Controls.Add(this.cmdDesignTbls_SWFiles);
            this.panel1.Controls.Add(this.cmdBrowse_FilePath_DesignTbls_SWFiles);
            this.panel1.Controls.Add(this.txtFilePath_DesignTbls_SWFiles);
            this.panel1.Controls.Add(this.lblFilePath_DesignTbls_SWFiles);
            this.panel1.Controls.Add(this.cmdBrowse_FilePath_Project);
            this.panel1.Controls.Add(this.txtFilePath_Project);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdDDR);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 343);
            this.panel1.TabIndex = 1;
            // 
            // cmdCustomized_SWFiles
            // 
            this.cmdCustomized_SWFiles.BackColor = System.Drawing.Color.Silver;
            this.cmdCustomized_SWFiles.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCustomized_SWFiles.Location = new System.Drawing.Point(11, 267);
            this.cmdCustomized_SWFiles.Name = "cmdCustomized_SWFiles";
            this.cmdCustomized_SWFiles.Size = new System.Drawing.Size(142, 28);
            this.cmdCustomized_SWFiles.TabIndex = 297;
            this.cmdCustomized_SWFiles.Text = "Customized SW Files";
            this.cmdCustomized_SWFiles.UseVisualStyleBackColor = false;
            this.cmdCustomized_SWFiles.Visible = false;
            this.cmdCustomized_SWFiles.Click += new System.EventHandler(this.cmdCustomized_SWFiles_Click);
            // 
            // lblModified_SWFiles
            // 
            this.lblModified_SWFiles.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified_SWFiles.Location = new System.Drawing.Point(240, 243);
            this.lblModified_SWFiles.Name = "lblModified_SWFiles";
            this.lblModified_SWFiles.Size = new System.Drawing.Size(51, 17);
            this.lblModified_SWFiles.TabIndex = 296;
            this.lblModified_SWFiles.Text = "No";
            this.lblModified_SWFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkModified_SWFiles
            // 
            this.chkModified_SWFiles.AutoSize = true;
            this.chkModified_SWFiles.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkModified_SWFiles.Location = new System.Drawing.Point(11, 242);
            this.chkModified_SWFiles.Name = "chkModified_SWFiles";
            this.chkModified_SWFiles.Size = new System.Drawing.Size(226, 19);
            this.chkModified_SWFiles.TabIndex = 295;
            this.chkModified_SWFiles.Text = "Model Files have been customized?";
            this.chkModified_SWFiles.UseVisualStyleBackColor = true;
            this.chkModified_SWFiles.CheckedChanged += new System.EventHandler(this.chkModified_SWFiles_CheckedChanged);
            // 
            // lblIndicator
            // 
            this.lblIndicator.AutoSize = true;
            this.lblIndicator.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIndicator.ForeColor = System.Drawing.Color.Red;
            this.lblIndicator.Location = new System.Drawing.Point(164, 126);
            this.lblIndicator.Name = "lblIndicator";
            this.lblIndicator.Size = new System.Drawing.Size(13, 16);
            this.lblIndicator.TabIndex = 294;
            this.lblIndicator.Text = "*";
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.Color.Red;
            this.lblNote.Location = new System.Drawing.Point(10, 300);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(336, 32);
            this.lblNote.TabIndex = 293;
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(471, 300);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 292;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdSIS
            // 
            this.cmdSIS.BackColor = System.Drawing.Color.Silver;
            this.cmdSIS.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSIS.Image = ((System.Drawing.Image)(resources.GetObject("cmdSIS.Image")));
            this.cmdSIS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSIS.Location = new System.Drawing.Point(206, 76);
            this.cmdSIS.Name = "cmdSIS";
            this.cmdSIS.Size = new System.Drawing.Size(179, 28);
            this.cmdSIS.TabIndex = 291;
            this.cmdSIS.Text = "Summary Info";
            this.cmdSIS.UseVisualStyleBackColor = false;
            this.cmdSIS.Visible = false;
            this.cmdSIS.Click += new System.EventHandler(this.cmdSIS_Click);
            // 
            // lblFilePath_Project
            // 
            this.lblFilePath_Project.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath_Project.Location = new System.Drawing.Point(8, 6);
            this.lblFilePath_Project.Name = "lblFilePath_Project";
            this.lblFilePath_Project.Size = new System.Drawing.Size(116, 20);
            this.lblFilePath_Project.TabIndex = 288;
            this.lblFilePath_Project.Text = "Project Files:";
            this.lblFilePath_Project.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdDesignTbls_SWFiles
            // 
            this.cmdDesignTbls_SWFiles.BackColor = System.Drawing.Color.Silver;
            this.cmdDesignTbls_SWFiles.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDesignTbls_SWFiles.Image = ((System.Drawing.Image)(resources.GetObject("cmdDesignTbls_SWFiles.Image")));
            this.cmdDesignTbls_SWFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDesignTbls_SWFiles.Location = new System.Drawing.Point(11, 191);
            this.cmdDesignTbls_SWFiles.Name = "cmdDesignTbls_SWFiles";
            this.cmdDesignTbls_SWFiles.Size = new System.Drawing.Size(179, 28);
            this.cmdDesignTbls_SWFiles.TabIndex = 5;
            this.cmdDesignTbls_SWFiles.Text = "Design Tables && SW Files";
            this.cmdDesignTbls_SWFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdDesignTbls_SWFiles.UseVisualStyleBackColor = false;
            this.cmdDesignTbls_SWFiles.Click += new System.EventHandler(this.cmdDesignTbls_SWFiles_Click);
            // 
            // cmdBrowse_FilePath_DesignTbls_SWFiles
            // 
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.BackColor = System.Drawing.Color.Silver;
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Image = ((System.Drawing.Image)(resources.GetObject("cmdBrowse_FilePath_DesignTbls_SWFiles.Image")));
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Location = new System.Drawing.Point(11, 153);
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Name = "cmdBrowse_FilePath_DesignTbls_SWFiles";
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Size = new System.Drawing.Size(84, 25);
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.TabIndex = 3;
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Text = "  Browse";
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.UseVisualStyleBackColor = false;
            this.cmdBrowse_FilePath_DesignTbls_SWFiles.Click += new System.EventHandler(this.cmdBrowse_FilePath_DesignTbls_SWFiles_Click);
            // 
            // txtFilePath_DesignTbls_SWFiles
            // 
            this.txtFilePath_DesignTbls_SWFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath_DesignTbls_SWFiles.Location = new System.Drawing.Point(113, 154);
            this.txtFilePath_DesignTbls_SWFiles.Name = "txtFilePath_DesignTbls_SWFiles";
            this.txtFilePath_DesignTbls_SWFiles.Size = new System.Drawing.Size(437, 20);
            this.txtFilePath_DesignTbls_SWFiles.TabIndex = 4;
            // 
            // lblFilePath_DesignTbls_SWFiles
            // 
            this.lblFilePath_DesignTbls_SWFiles.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath_DesignTbls_SWFiles.Location = new System.Drawing.Point(8, 123);
            this.lblFilePath_DesignTbls_SWFiles.Name = "lblFilePath_DesignTbls_SWFiles";
            this.lblFilePath_DesignTbls_SWFiles.Size = new System.Drawing.Size(184, 20);
            this.lblFilePath_DesignTbls_SWFiles.TabIndex = 284;
            this.lblFilePath_DesignTbls_SWFiles.Text = "Design Tables && SW Files:";
            this.lblFilePath_DesignTbls_SWFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdBrowse_FilePath_Project
            // 
            this.cmdBrowse_FilePath_Project.BackColor = System.Drawing.Color.Silver;
            this.cmdBrowse_FilePath_Project.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBrowse_FilePath_Project.Image = ((System.Drawing.Image)(resources.GetObject("cmdBrowse_FilePath_Project.Image")));
            this.cmdBrowse_FilePath_Project.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdBrowse_FilePath_Project.Location = new System.Drawing.Point(11, 36);
            this.cmdBrowse_FilePath_Project.Name = "cmdBrowse_FilePath_Project";
            this.cmdBrowse_FilePath_Project.Size = new System.Drawing.Size(84, 25);
            this.cmdBrowse_FilePath_Project.TabIndex = 0;
            this.cmdBrowse_FilePath_Project.Text = "  Browse";
            this.cmdBrowse_FilePath_Project.UseVisualStyleBackColor = false;
            this.cmdBrowse_FilePath_Project.Click += new System.EventHandler(this.cmdBrowse_FilePath_Project_Click);
            // 
            // txtFilePath_Project
            // 
            this.txtFilePath_Project.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtFilePath_Project.Location = new System.Drawing.Point(113, 37);
            this.txtFilePath_Project.Name = "txtFilePath_Project";
            this.txtFilePath_Project.Size = new System.Drawing.Size(437, 20);
            this.txtFilePath_Project.TabIndex = 1;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(371, 300);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(82, 32);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdDDR
            // 
            this.cmdDDR.BackColor = System.Drawing.Color.Silver;
            this.cmdDDR.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDDR.Image = ((System.Drawing.Image)(resources.GetObject("cmdDDR.Image")));
            this.cmdDDR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDDR.Location = new System.Drawing.Point(11, 76);
            this.cmdDDR.Name = "cmdDDR";
            this.cmdDDR.Size = new System.Drawing.Size(179, 28);
            this.cmdDDR.TabIndex = 2;
            this.cmdDDR.Text = "Design Dept. Request";
            this.cmdDDR.UseVisualStyleBackColor = false;
            this.cmdDDR.Click += new System.EventHandler(this.cmdDIS_Click);
            // 
            // frmCreateFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(566, 347);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCreateFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Files";
            this.Activated += new System.EventHandler(this.frmCreateFiles_Activated);
            this.Load += new System.EventHandler(this.frmCreateFiles_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdDDR;
        private System.Windows.Forms.TextBox txtFilePath_Project;
        internal System.Windows.Forms.Button cmdDesignTbls_SWFiles;
        private System.Windows.Forms.Button cmdBrowse_FilePath_DesignTbls_SWFiles;
        private System.Windows.Forms.TextBox txtFilePath_DesignTbls_SWFiles;
        private System.Windows.Forms.Label lblFilePath_DesignTbls_SWFiles;
        private System.Windows.Forms.Button cmdBrowse_FilePath_Project;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblFilePath_Project;
        internal System.Windows.Forms.Button cmdSIS;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblIndicator;
        private System.Windows.Forms.Label lblModified_SWFiles;
        private System.Windows.Forms.CheckBox chkModified_SWFiles;
        internal System.Windows.Forms.Button cmdCustomized_SWFiles;

    }
}