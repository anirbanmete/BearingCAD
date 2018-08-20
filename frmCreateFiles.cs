
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmCreateFiles                         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  22JAN13                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BearingCAD20
{
    public partial class frmCreateFiles : Form
    {
        #region "MEMBER VARIABLE DECLARATION"
        //***********************************    
             clsSW_Models mSW_Models = new clsSW_Models();
  
        #endregion


        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmCreateFiles()
            //=====================
            {
                InitializeComponent();
            }

        #endregion


        #region "FROM LOAD RELATED ROUTINE:"
        //*********************************

            private void frmCreateFiles_Load (object sender, EventArgs e)
            //===========================================================
            {
                txtFilePath_Project.Text = modMain.gProject.FilePath_Project;           
                txtFilePath_DesignTbls_SWFiles.Text = modMain.gProject.FilePath_DesignTbls_SWFiles;

                //chkModified_SWFiles.Checked = modMain.gProject.AreManuallyModified_SWModels();    
            }

            private void frmCreateFiles_Activated(object sender, EventArgs e)
            //================================================================
            {
                chkModified_SWFiles.Checked = modMain.gProject.AreManuallyModified_SWModels();

            }

        #endregion


        #region "CONTROL EVENT ROUTINES:"
        //******************************

            #region "CHECK BOX RELATED ROUTINE"
            //---------------------------------

                private void chkModified_SWFiles_CheckedChanged(object sender, EventArgs e)
                //==========================================================================
                {
                    if (chkModified_SWFiles.Checked)
                    {
                        lblModified_SWFiles.Text = "Yes";
                        lblModified_SWFiles.ForeColor = Color.Red;    
  
                        cmdCustomized_SWFiles.Visible = true;
                    }

                    else
                    {
                        lblModified_SWFiles.Text = "No";
                        lblModified_SWFiles.ForeColor = Color.Black;

                        cmdCustomized_SWFiles.Visible = false;
                    }
                }

            #endregion

        #endregion


        #region "COMMAND BUTTON RELATED ROUTINE"
        //**************************************

            private void cmdBrowse_FilePath_Project_Click(object sender, EventArgs e) 
            //=======================================================================
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath_Project.Text = folderBrowserDialog1.SelectedPath;
                }
            }


            private void cmdDIS_Click(object sender, EventArgs e)
            //===================================================
            {
                if (txtFilePath_Project.Text == "")
                {
                    MessageBox.Show("Project files folder path can not be blank.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cursor = Cursors.WaitCursor;
                modMain.gReport.Populate_DDR(modMain.gFiles, modMain.gProject, modMain.gUnit,
                                             modMain.gOpCond, txtFilePath_Project.Text);
                Cursor = Cursors.Default;
            }


            //....Not used now   //BG 28DEC12
            private void cmdSIS_Click(object sender, EventArgs e)
            //==================================================== 
            {
                if (txtFilePath_Project.Text == "")
                {
                    MessageBox.Show("Project files folder path can not be blank.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            private void cmdBrowse_FilePath_DesignTbls_SWFiles_Click(object sender, EventArgs e) 
            //==================================================================================
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath_DesignTbls_SWFiles.Text = folderBrowserDialog1.SelectedPath;
                }
            }


            private void cmdDesignTbls_SWFiles_Click(object sender, EventArgs e)   
            //==================================================================
            {
                ////if (txtFilePath_DesignTbls_SWFiles.Text == "")
                ////{
                ////    MessageBox.Show("SW folder path can not be blank.","Error",
                ////                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                ////    return;
                ////}

                ////Cursor = Cursors.WaitCursor;

                //////....SWFiles: Design Tables, SW Models & Dwg Files.
                ////mSW_Models.PopulateAndMove_SWFiles_ProjectFolder(modMain.gFiles, modMain.gProject, modMain.gOpCond,
                ////                                                 modMain.gDB, txtFilePath_DesignTbls_SWFiles.Text);
                ////Cursor = Cursors.Default;
            }


            private void cmdCustomized_SWFiles_Click(object sender, EventArgs e)
            //==================================================================        
            {
                SaveData_Local();       //AM 01JAN13
                frmSWModels mfrmSWModels = new frmSWModels();
                mfrmSWModels.ShowDialog();
            }


            private void cmdOK_Click(object sender, EventArgs e)
            //==================================================
            {                
                SaveData_Local();
                modMain.gDB.SaveData_Global();                
                this.Hide();
            }


            #region "Helper Routines:"
            //************************

                private void SaveData_Local()
                //===========================    
                {
                    modMain.gProject.FilePath_Project = txtFilePath_Project.Text;
                    modMain.gProject.FilePath_DesignTbls_SWFiles = txtFilePath_DesignTbls_SWFiles.Text;
                }

            #endregion
           

            private void cmdCancel_Click(object sender, EventArgs e)
            //======================================================
            {
                this.Close();              
            }

        #endregion

    }
}
