namespace BearingCAD21
{
    partial class frmCreateDataSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateDataSet));
            this.lblBorder = new System.Windows.Forms.Label();
            this.pnlpanel1 = new System.Windows.Forms.Panel();
            this.cmdOpen_CompleteAssy = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCreateParameterList = new System.Windows.Forms.Button();
            this.cmdBrowse_FilePath_Project = new System.Windows.Forms.Button();
            this.txtFilePath_Project = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlpanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(1, 1);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(510, 250);
            this.lblBorder.TabIndex = 0;
            this.lblBorder.Text = "label1";
            // 
            // pnlpanel1
            // 
            this.pnlpanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlpanel1.Controls.Add(this.cmdOpen_CompleteAssy);
            this.pnlpanel1.Controls.Add(this.cmdCancel);
            this.pnlpanel1.Controls.Add(this.cmdOK);
            this.pnlpanel1.Controls.Add(this.cmdCreateParameterList);
            this.pnlpanel1.Controls.Add(this.cmdBrowse_FilePath_Project);
            this.pnlpanel1.Controls.Add(this.txtFilePath_Project);
            this.pnlpanel1.Location = new System.Drawing.Point(2, 2);
            this.pnlpanel1.Name = "pnlpanel1";
            this.pnlpanel1.Size = new System.Drawing.Size(508, 248);
            this.pnlpanel1.TabIndex = 1;
            // 
            // cmdOpen_CompleteAssy
            // 
            this.cmdOpen_CompleteAssy.BackColor = System.Drawing.Color.Silver;
            this.cmdOpen_CompleteAssy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOpen_CompleteAssy.Image = ((System.Drawing.Image)(resources.GetObject("cmdOpen_CompleteAssy.Image")));
            this.cmdOpen_CompleteAssy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOpen_CompleteAssy.Location = new System.Drawing.Point(187, 70);
            this.cmdOpen_CompleteAssy.Name = "cmdOpen_CompleteAssy";
            this.cmdOpen_CompleteAssy.Size = new System.Drawing.Size(145, 28);
            this.cmdOpen_CompleteAssy.TabIndex = 295;
            this.cmdOpen_CompleteAssy.Text = "Open Project Folder";
            this.cmdOpen_CompleteAssy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOpen_CompleteAssy.UseVisualStyleBackColor = false;
            this.cmdOpen_CompleteAssy.Click += new System.EventHandler(this.cmdOpen_CompleteAssy_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(411, 207);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 294;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(311, 207);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(82, 32);
            this.cmdOK.TabIndex = 293;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCreateParameterList
            // 
            this.cmdCreateParameterList.BackColor = System.Drawing.Color.Silver;
            this.cmdCreateParameterList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCreateParameterList.Image = ((System.Drawing.Image)(resources.GetObject("cmdCreateParameterList.Image")));
            this.cmdCreateParameterList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCreateParameterList.Location = new System.Drawing.Point(9, 70);
            this.cmdCreateParameterList.Name = "cmdCreateParameterList";
            this.cmdCreateParameterList.Size = new System.Drawing.Size(160, 28);
            this.cmdCreateParameterList.TabIndex = 6;
            this.cmdCreateParameterList.Text = "CAD Neutral Data Set";
            this.cmdCreateParameterList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCreateParameterList.UseVisualStyleBackColor = false;
            this.cmdCreateParameterList.Click += new System.EventHandler(this.cmdCreateParameterList_Click);
            // 
            // cmdBrowse_FilePath_Project
            // 
            this.cmdBrowse_FilePath_Project.BackColor = System.Drawing.Color.Silver;
            this.cmdBrowse_FilePath_Project.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBrowse_FilePath_Project.Image = ((System.Drawing.Image)(resources.GetObject("cmdBrowse_FilePath_Project.Image")));
            this.cmdBrowse_FilePath_Project.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdBrowse_FilePath_Project.Location = new System.Drawing.Point(9, 28);
            this.cmdBrowse_FilePath_Project.Name = "cmdBrowse_FilePath_Project";
            this.cmdBrowse_FilePath_Project.Size = new System.Drawing.Size(84, 25);
            this.cmdBrowse_FilePath_Project.TabIndex = 2;
            this.cmdBrowse_FilePath_Project.Text = "  Browse";
            this.cmdBrowse_FilePath_Project.UseVisualStyleBackColor = false;
            this.cmdBrowse_FilePath_Project.Click += new System.EventHandler(this.cmdBrowse_FilePath_Project_Click);
            // 
            // txtFilePath_Project
            // 
            this.txtFilePath_Project.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath_Project.Location = new System.Drawing.Point(108, 30);
            this.txtFilePath_Project.Name = "txtFilePath_Project";
            this.txtFilePath_Project.Size = new System.Drawing.Size(383, 21);
            this.txtFilePath_Project.TabIndex = 3;
            // 
            // frmCreateDataSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(513, 254);
            this.ControlBox = false;
            this.Controls.Add(this.pnlpanel1);
            this.Controls.Add(this.lblBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCreateDataSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventor Model";
            this.pnlpanel1.ResumeLayout(false);
            this.pnlpanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel pnlpanel1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCreateParameterList;
        private System.Windows.Forms.Button cmdBrowse_FilePath_Project;
        private System.Windows.Forms.TextBox txtFilePath_Project;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        internal System.Windows.Forms.Button cmdOpen_CompleteAssy;
    }
}