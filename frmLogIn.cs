//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmLogIn                               '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  28JUN13                                '
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

//....Designer Changed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;

namespace BearingCAD20
{
    public partial class frmLogIn : Form
    {
        #region "MEMBER VARIABLE"
        //***********************
            private Boolean mblnChangePasswd = false;

        #endregion

        #region "PROPERTY DECLARATION"
        //****************************
            public Boolean ChangePasswd
            {
                set { mblnChangePasswd = value; }
            }

        #endregion

        #region "FORM RELATED ROUTINE"
            //****************************

            public frmLogIn()
            //===============
            {
                InitializeComponent();
            }


            private void frmLogIn_Load(object sender, EventArgs e)
            //====================================================
            {
                //....Load User Name in ComboBox.
                LoadUser();
            }

            private void LoadUser()
            //=====================
            {
                modMain.gDB.PopulateCmbBox(cmbUserName, "tblUser", "fldName", "", true);
                cmbUserName.SelectedIndex = 0;
            }

        #endregion

        #region "CONTROL EVENT RELATED ROUTINE"
        //*************************************

            #region "COMBOBOX RELATED ROUTINE"

                private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
                //=======================================================================
                {
                    modMain.gUser.Name = cmbUserName.Text;
                    txtInitials.Text = modMain.gUser.Initials;

                    cmbRole.Items.Clear();
                   
                    switch(modMain.gUser.Privilege)
                    {
                        case "Engineering":
                        //-----------------
                            cmbRole.Items.Add("Engineer");
                            cmbRole.Items.Add("Designer");
                            cmbRole.Items.Add("Checker");
                            cmbRole.Items.Add("Viewer");
                        break;

                        case "Designer":
                        //--------------
                            cmbRole.Items.Add("Designer");
                            cmbRole.Items.Add("Checker");
                            cmbRole.Items.Add("Viewer");
                        break;

                        case "Manufacturing":
                        //--------------------
                            //cmbRole.Items.Add("Checker");
                            cmbRole.Items.Add("Editor");
                            cmbRole.Items.Add("Viewer");
                        break;

                        //BG 28JUN13
                        //case "Checking":
                        //    cmbRole.Items.Add("Checker");
                        //    cmbRole.Items.Add("Viewer");
                        //break;

                        //BG 28JUN13
                        case "General":
                        //-------------
                            cmbRole.Items.Add("Viewer");
                        break;
                    }

                    cmbRole.SelectedIndex = 0;
                }

            #endregion


            #region "COMMAND BUTTON"

                private void cmdChangePassword_Click(object sender, EventArgs e)
                //==============================================================
                {
                    string pstrMsg = "";
                    if (modMain.gUser.Name != "")                    
                    {
                        modMain.gfrmPassword.ShowDialog();
                        if (mblnChangePasswd)
                        txtPassword.Text = modMain.gUser.Password;
                    }
                    else
                    {
                        pstrMsg = "Please Select User Name.";

                        MessageBox.Show(pstrMsg, "LogIn Authentication",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cmbUserName.Focus();
                    }

                }


                private void cmdOK_Click(object sender, EventArgs e)
                //==================================================
                {
                    string pstrMsg = null;

                    modMain.gUser.Role = cmbRole.Text;      //BG 28JUN13

                    if (cmbUserName.Text.Trim() != "")
                    {
                        if (txtPassword.Text.Trim() != "")
                        {
                            CheckPassword();
                        }
                        else
                        {
                            pstrMsg = "Password can not be blank.";

                            MessageBox.Show(pstrMsg, "LogIn Authentication",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtPassword.Focus();
                            return;
                        }
                    }
                    else
                    {
                        pstrMsg = "Please select user name.";

                        MessageBox.Show(pstrMsg, "LogIn Authentication",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cmbUserName.Focus();
                        return;
                    }
              
                }

                private void CheckPassword()           
                //==========================
                {
                    string pstrPassword = modMain.gUser.Password;
                    string pstrMsg = "";

                    if (txtPassword.Text == pstrPassword)
                    {

                        //pstrMsg = "LogIn Successful.";

                        //MessageBox.Show(pstrMsg, "LogIn Authentication",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();                  
                        modMain.gfrmMain.ShowDialog();
                    }

                    else
                    {

                        pstrMsg = "Password is Wrong. Login Failure ";

                        MessageBox.Show(pstrMsg, "LogIn Authentication",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtPassword.Focus();
                        txtPassword.Text = "";
                        return;
                    }
                }


                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {
                    Cursor = Cursors.WaitCursor;
                    System.Environment.Exit(0);
                    Cursor = Cursors.Default;
                }

            #endregion

        #endregion

    }
}