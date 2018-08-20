namespace BearingCAD20
{
    partial class frmImportData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportData));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpThrust = new System.Windows.Forms.GroupBox();
            this.optThrust_Front = new System.Windows.Forms.RadioButton();
            this.optThrust_Back = new System.Windows.Forms.RadioButton();
            this.cmdXLThrust = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdXLKMC = new System.Windows.Forms.Button();
            this.lblRadial = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.grpThrust.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBorder.Location = new System.Drawing.Point(1, 1);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(268, 218);
            this.lblBorder.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.grpThrust);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdXLKMC);
            this.panel1.Controls.Add(this.lblRadial);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 216);
            this.panel1.TabIndex = 1;
            // 
            // grpThrust
            // 
            this.grpThrust.Controls.Add(this.optThrust_Front);
            this.grpThrust.Controls.Add(this.optThrust_Back);
            this.grpThrust.Controls.Add(this.cmdXLThrust);
            this.grpThrust.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThrust.Location = new System.Drawing.Point(11, 80);
            this.grpThrust.Name = "grpThrust";
            this.grpThrust.Size = new System.Drawing.Size(244, 67);
            this.grpThrust.TabIndex = 484;
            this.grpThrust.TabStop = false;
            this.grpThrust.Text = "Thrust:";
            // 
            // optThrust_Front
            // 
            this.optThrust_Front.AutoSize = true;
            this.optThrust_Front.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optThrust_Front.Location = new System.Drawing.Point(13, 31);
            this.optThrust_Front.Name = "optThrust_Front";
            this.optThrust_Front.Size = new System.Drawing.Size(54, 18);
            this.optThrust_Front.TabIndex = 469;
            this.optThrust_Front.TabStop = true;
            this.optThrust_Front.Text = "Front";
            this.optThrust_Front.UseVisualStyleBackColor = true;
            // 
            // optThrust_Back
            // 
            this.optThrust_Back.AutoSize = true;
            this.optThrust_Back.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optThrust_Back.Location = new System.Drawing.Point(76, 31);
            this.optThrust_Back.Name = "optThrust_Back";
            this.optThrust_Back.Size = new System.Drawing.Size(51, 18);
            this.optThrust_Back.TabIndex = 470;
            this.optThrust_Back.TabStop = true;
            this.optThrust_Back.Text = "Back";
            this.optThrust_Back.UseVisualStyleBackColor = true;
            // 
            // cmdXLThrust
            // 
            this.cmdXLThrust.BackColor = System.Drawing.Color.Silver;
            this.cmdXLThrust.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdXLThrust.Location = new System.Drawing.Point(140, 24);
            this.cmdXLThrust.Name = "cmdXLThrust";
            this.cmdXLThrust.Size = new System.Drawing.Size(90, 29);
            this.cmdXLThrust.TabIndex = 471;
            this.cmdXLThrust.Text = "XLThrust";
            this.cmdXLThrust.UseVisualStyleBackColor = false;
            this.cmdXLThrust.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(0, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 2);
            this.label1.TabIndex = 483;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(175, 171);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 481;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // cmdXLKMC
            // 
            this.cmdXLKMC.BackColor = System.Drawing.Color.Silver;
            this.cmdXLKMC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdXLKMC.Location = new System.Drawing.Point(151, 16);
            this.cmdXLKMC.Name = "cmdXLKMC";
            this.cmdXLKMC.Size = new System.Drawing.Size(90, 29);
            this.cmdXLKMC.TabIndex = 467;
            this.cmdXLKMC.Text = "XLKMC";
            this.cmdXLKMC.UseVisualStyleBackColor = false;
            this.cmdXLKMC.Click += new System.EventHandler(this.cmdButtons_Click);
            // 
            // lblRadial
            // 
            this.lblRadial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblRadial.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRadial.Location = new System.Drawing.Point(9, 21);
            this.lblRadial.Name = "lblRadial";
            this.lblRadial.Size = new System.Drawing.Size(54, 18);
            this.lblRadial.TabIndex = 258;
            this.lblRadial.Text = "Radial:";
            this.lblRadial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmAnalyticalData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(270, 220);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAnalyticalData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Analytical Data";
            this.Load += new System.EventHandler(this.frmAnalyticalData_Load);
            this.panel1.ResumeLayout(false);
            this.grpThrust.ResumeLayout(false);
            this.grpThrust.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblRadial;
        private System.Windows.Forms.Button cmdXLKMC;
        private System.Windows.Forms.RadioButton optThrust_Back;
        private System.Windows.Forms.RadioButton optThrust_Front;
        private System.Windows.Forms.Button cmdXLThrust;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpThrust;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}