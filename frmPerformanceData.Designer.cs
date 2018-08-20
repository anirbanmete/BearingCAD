namespace BearingCAD20
{
    partial class frmPerformanceData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPerformanceData));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdDisplay = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optBearing_Radial = new System.Windows.Forms.RadioButton();
            this.optBearing_Thrust_Back = new System.Windows.Forms.RadioButton();
            this.optBearing_Thrust_Front = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(2, 2);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(296, 198);
            this.lblBorder.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdDisplay);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 196);
            this.panel1.TabIndex = 1;
            // 
            // cmdDisplay
            // 
            this.cmdDisplay.BackColor = System.Drawing.Color.Silver;
            this.cmdDisplay.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDisplay.Location = new System.Drawing.Point(20, 154);
            this.cmdDisplay.Name = "cmdDisplay";
            this.cmdDisplay.Size = new System.Drawing.Size(80, 32);
            this.cmdDisplay.TabIndex = 466;
            this.cmdDisplay.Text = "Display";
            this.cmdDisplay.UseVisualStyleBackColor = false;
            this.cmdDisplay.Click += new System.EventHandler(this.cmdDisplay_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(192, 154);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(76, 32);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optBearing_Radial);
            this.groupBox1.Controls.Add(this.optBearing_Thrust_Back);
            this.groupBox1.Controls.Add(this.optBearing_Thrust_Front);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 126);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // optBearing_Radial
            // 
            this.optBearing_Radial.AutoSize = true;
            this.optBearing_Radial.Checked = true;
            this.optBearing_Radial.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optBearing_Radial.Location = new System.Drawing.Point(17, 22);
            this.optBearing_Radial.Name = "optBearing_Radial";
            this.optBearing_Radial.Size = new System.Drawing.Size(107, 19);
            this.optBearing_Radial.TabIndex = 3;
            this.optBearing_Radial.TabStop = true;
            this.optBearing_Radial.Text = "Radial Bearing";
            this.optBearing_Radial.UseVisualStyleBackColor = true;
            // 
            // optBearing_Thrust_Back
            // 
            this.optBearing_Thrust_Back.AutoSize = true;
            this.optBearing_Thrust_Back.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optBearing_Thrust_Back.Location = new System.Drawing.Point(17, 92);
            this.optBearing_Thrust_Back.Name = "optBearing_Thrust_Back";
            this.optBearing_Thrust_Back.Size = new System.Drawing.Size(148, 19);
            this.optBearing_Thrust_Back.TabIndex = 5;
            this.optBearing_Thrust_Back.Text = "Thrust Bearing - Back";
            this.optBearing_Thrust_Back.UseVisualStyleBackColor = true;
            // 
            // optBearing_Thrust_Front
            // 
            this.optBearing_Thrust_Front.AutoSize = true;
            this.optBearing_Thrust_Front.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optBearing_Thrust_Front.Location = new System.Drawing.Point(17, 57);
            this.optBearing_Thrust_Front.Name = "optBearing_Thrust_Front";
            this.optBearing_Thrust_Front.Size = new System.Drawing.Size(148, 19);
            this.optBearing_Thrust_Front.TabIndex = 4;
            this.optBearing_Thrust_Front.Text = "Thrust Bearing - Front";
            this.optBearing_Thrust_Front.UseVisualStyleBackColor = true;
            // 
            // frmPerformanceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(299, 202);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.Name = "frmPerformanceData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Performance Data";
            this.Load += new System.EventHandler(this.frmPerformanceData_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optBearing_Thrust_Front;
        private System.Windows.Forms.RadioButton optBearing_Radial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optBearing_Thrust_Back;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdDisplay;
    }
}