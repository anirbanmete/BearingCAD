namespace BearingCAD20
{
    partial class frmSWModels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSWModels));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.grpNotes = new System.Windows.Forms.GroupBox();
            this.txtFileModification_Notes = new System.Windows.Forms.TextBox();
            this.grpEndSeal = new System.Windows.Forms.GroupBox();
            this.chkFileModified_EndSeal_Part = new System.Windows.Forms.CheckBox();
            this.cmdCreateRefSwFiles = new System.Windows.Forms.Button();
            this.grpEndTB = new System.Windows.Forms.GroupBox();
            this.chkFileModified_EndTB_Part = new System.Windows.Forms.CheckBox();
            this.chkFileModified_EndTB_Assy = new System.Windows.Forms.CheckBox();
            this.grpRadialBearing = new System.Windows.Forms.GroupBox();
            this.chkFileModified_RadialPart = new System.Windows.Forms.CheckBox();
            this.chkFileModified_RadialBlankAssy = new System.Windows.Forms.CheckBox();
            this.chkFileModified_CompleteAssy = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.grpNotes.SuspendLayout();
            this.grpEndSeal.SuspendLayout();
            this.grpEndTB.SuspendLayout();
            this.grpRadialBearing.SuspendLayout();
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
            this.lblBorder.Size = new System.Drawing.Size(463, 435);
            this.lblBorder.TabIndex = 0;
            this.lblBorder.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmdPrint);
            this.panel1.Controls.Add(this.grpNotes);
            this.panel1.Controls.Add(this.grpEndSeal);
            this.panel1.Controls.Add(this.cmdCreateRefSwFiles);
            this.panel1.Controls.Add(this.grpEndTB);
            this.panel1.Controls.Add(this.grpRadialBearing);
            this.panel1.Controls.Add(this.chkFileModified_CompleteAssy);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 433);
            this.panel1.TabIndex = 1;
            // 
            // cmdPrint
            // 
            this.cmdPrint.BackColor = System.Drawing.Color.Silver;
            this.cmdPrint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPrint.Location = new System.Drawing.Point(19, 390);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(80, 32);
            this.cmdPrint.TabIndex = 465;
            this.cmdPrint.Text = "&Print";
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdButton_Click);
            // 
            // grpNotes
            // 
            this.grpNotes.Controls.Add(this.txtFileModification_Notes);
            this.grpNotes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpNotes.ForeColor = System.Drawing.Color.Black;
            this.grpNotes.Location = new System.Drawing.Point(19, 241);
            this.grpNotes.Name = "grpNotes";
            this.grpNotes.Size = new System.Drawing.Size(420, 87);
            this.grpNotes.TabIndex = 306;
            this.grpNotes.TabStop = false;
            this.grpNotes.Text = "Notes:";
            // 
            // txtFileModification_Notes
            // 
            this.txtFileModification_Notes.Location = new System.Drawing.Point(7, 20);
            this.txtFileModification_Notes.Multiline = true;
            this.txtFileModification_Notes.Name = "txtFileModification_Notes";
            this.txtFileModification_Notes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileModification_Notes.Size = new System.Drawing.Size(407, 61);
            this.txtFileModification_Notes.TabIndex = 305;
            // 
            // grpEndSeal
            // 
            this.grpEndSeal.Controls.Add(this.chkFileModified_EndSeal_Part);
            this.grpEndSeal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpEndSeal.ForeColor = System.Drawing.Color.Black;
            this.grpEndSeal.Location = new System.Drawing.Point(288, 145);
            this.grpEndSeal.Name = "grpEndSeal";
            this.grpEndSeal.Size = new System.Drawing.Size(150, 81);
            this.grpEndSeal.TabIndex = 304;
            this.grpEndSeal.TabStop = false;
            this.grpEndSeal.Text = "End Seal:";
            // 
            // chkFileModified_EndSeal_Part
            // 
            this.chkFileModified_EndSeal_Part.AutoSize = true;
            this.chkFileModified_EndSeal_Part.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_EndSeal_Part.Location = new System.Drawing.Point(70, 20);
            this.chkFileModified_EndSeal_Part.Name = "chkFileModified_EndSeal_Part";
            this.chkFileModified_EndSeal_Part.Size = new System.Drawing.Size(70, 18);
            this.chkFileModified_EndSeal_Part.TabIndex = 297;
            this.chkFileModified_EndSeal_Part.Text = "Part File";
            this.chkFileModified_EndSeal_Part.UseVisualStyleBackColor = true;
            //this.chkFileModified_EndSeal_Part.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // cmdCreateRefSwFiles
            // 
            this.cmdCreateRefSwFiles.BackColor = System.Drawing.Color.Silver;
            this.cmdCreateRefSwFiles.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCreateRefSwFiles.Location = new System.Drawing.Point(19, 341);
            this.cmdCreateRefSwFiles.Name = "cmdCreateRefSwFiles";
            this.cmdCreateRefSwFiles.Size = new System.Drawing.Size(138, 32);
            this.cmdCreateRefSwFiles.TabIndex = 303;
            this.cmdCreateRefSwFiles.Text = "Create Ref. SW Files";
            this.cmdCreateRefSwFiles.UseVisualStyleBackColor = false;
            this.cmdCreateRefSwFiles.Click += new System.EventHandler(this.cmdButton_Click);
            // 
            // grpEndTB
            // 
            this.grpEndTB.Controls.Add(this.chkFileModified_EndTB_Part);
            this.grpEndTB.Controls.Add(this.chkFileModified_EndTB_Assy);
            this.grpEndTB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpEndTB.ForeColor = System.Drawing.Color.Black;
            this.grpEndTB.Location = new System.Drawing.Point(19, 145);
            this.grpEndTB.Name = "grpEndTB";
            this.grpEndTB.Size = new System.Drawing.Size(252, 81);
            this.grpEndTB.TabIndex = 302;
            this.grpEndTB.TabStop = false;
            this.grpEndTB.Text = "Thrust Bearing:";
            // 
            // chkFileModified_EndTB_Part
            // 
            this.chkFileModified_EndTB_Part.AutoSize = true;
            this.chkFileModified_EndTB_Part.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_EndTB_Part.Location = new System.Drawing.Point(138, 25);
            this.chkFileModified_EndTB_Part.Name = "chkFileModified_EndTB_Part";
            this.chkFileModified_EndTB_Part.Size = new System.Drawing.Size(70, 18);
            this.chkFileModified_EndTB_Part.TabIndex = 295;
            this.chkFileModified_EndTB_Part.Text = "Part File";
            this.chkFileModified_EndTB_Part.UseVisualStyleBackColor = true;
            //this.chkFileModified_EndTB_Part.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // chkFileModified_EndTB_Assy
            // 
            this.chkFileModified_EndTB_Assy.AutoSize = true;
            this.chkFileModified_EndTB_Assy.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_EndTB_Assy.Location = new System.Drawing.Point(138, 56);
            this.chkFileModified_EndTB_Assy.Name = "chkFileModified_EndTB_Assy";
            this.chkFileModified_EndTB_Assy.Size = new System.Drawing.Size(104, 18);
            this.chkFileModified_EndTB_Assy.TabIndex = 296;
            this.chkFileModified_EndTB_Assy.Text = "Assembly File";
            this.chkFileModified_EndTB_Assy.UseVisualStyleBackColor = true;
            //this.chkFileModified_EndTB_Assy.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // grpRadialBearing
            // 
            this.grpRadialBearing.Controls.Add(this.chkFileModified_RadialPart);
            this.grpRadialBearing.Controls.Add(this.chkFileModified_RadialBlankAssy);
            this.grpRadialBearing.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRadialBearing.ForeColor = System.Drawing.Color.Black;
            this.grpRadialBearing.Location = new System.Drawing.Point(19, 49);
            this.grpRadialBearing.Name = "grpRadialBearing";
            this.grpRadialBearing.Size = new System.Drawing.Size(420, 81);
            this.grpRadialBearing.TabIndex = 301;
            this.grpRadialBearing.TabStop = false;
            this.grpRadialBearing.Text = "Radial Bearing:";
            // 
            // chkFileModified_RadialPart
            // 
            this.chkFileModified_RadialPart.AutoSize = true;
            this.chkFileModified_RadialPart.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_RadialPart.Location = new System.Drawing.Point(138, 25);
            this.chkFileModified_RadialPart.Name = "chkFileModified_RadialPart";
            this.chkFileModified_RadialPart.Size = new System.Drawing.Size(105, 18);
            this.chkFileModified_RadialPart.TabIndex = 0;
            this.chkFileModified_RadialPart.Text = "Radial Part File";
            this.chkFileModified_RadialPart.UseVisualStyleBackColor = true;
            //this.chkFileModified_RadialPart.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // chkFileModified_RadialBlankAssy
            // 
            this.chkFileModified_RadialBlankAssy.AutoSize = true;
            this.chkFileModified_RadialBlankAssy.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_RadialBlankAssy.Location = new System.Drawing.Point(138, 56);
            this.chkFileModified_RadialBlankAssy.Name = "chkFileModified_RadialBlankAssy";
            this.chkFileModified_RadialBlankAssy.Size = new System.Drawing.Size(137, 18);
            this.chkFileModified_RadialBlankAssy.TabIndex = 4;
            this.chkFileModified_RadialBlankAssy.Text = "Blank Assembly File";
            this.chkFileModified_RadialBlankAssy.UseVisualStyleBackColor = true;
            //this.chkFileModified_RadialBlankAssy.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // chkFileModified_CompleteAssy
            // 
            this.chkFileModified_CompleteAssy.AutoSize = true;
            this.chkFileModified_CompleteAssy.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFileModified_CompleteAssy.Location = new System.Drawing.Point(157, 17);
            this.chkFileModified_CompleteAssy.Name = "chkFileModified_CompleteAssy";
            this.chkFileModified_CompleteAssy.Size = new System.Drawing.Size(104, 18);
            this.chkFileModified_CompleteAssy.TabIndex = 300;
            this.chkFileModified_CompleteAssy.Text = "Assembly File";
            this.chkFileModified_CompleteAssy.UseVisualStyleBackColor = true;
            //this.chkFileModified_CompleteAssy.CheckedChanged += new System.EventHandler(this.chkBox_CheckedChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(348, 395);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 294;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdButton_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(252, 395);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(82, 32);
            this.cmdOK.TabIndex = 293;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(26, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Complete Assembly:";
            // 
            // frmSWModels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(465, 437);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.Name = "frmSWModels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SolidWorks Model Files: Manual Modifications";
            this.Load += new System.EventHandler(this.frmSWModels_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpNotes.ResumeLayout(false);
            this.grpNotes.PerformLayout();
            this.grpEndSeal.ResumeLayout(false);
            this.grpEndSeal.PerformLayout();
            this.grpEndTB.ResumeLayout(false);
            this.grpEndTB.PerformLayout();
            this.grpRadialBearing.ResumeLayout(false);
            this.grpRadialBearing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkFileModified_RadialBlankAssy;
        private System.Windows.Forms.CheckBox chkFileModified_RadialPart;
        private System.Windows.Forms.CheckBox chkFileModified_EndTB_Assy;
        private System.Windows.Forms.CheckBox chkFileModified_EndTB_Part;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.CheckBox chkFileModified_EndSeal_Part;
        private System.Windows.Forms.GroupBox grpEndTB;
        private System.Windows.Forms.GroupBox grpRadialBearing;
        private System.Windows.Forms.CheckBox chkFileModified_CompleteAssy;
        private System.Windows.Forms.Button cmdCreateRefSwFiles;
        private System.Windows.Forms.GroupBox grpEndSeal;
        private System.Windows.Forms.GroupBox grpNotes;
        private System.Windows.Forms.TextBox txtFileModification_Notes;
        private System.Windows.Forms.Button cmdPrint;
    }
}