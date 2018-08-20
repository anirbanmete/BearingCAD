//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPassword                            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21OCT11                                '
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

//....Designer changed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BearingCAD20
{
    public partial class frmPassword : Form
    {

        #region "FROM RELATED ROUTINE"

            public frmPassword()
            //==================
            {
                InitializeComponent();
            }

            private void frmPassword_Load(object sender, EventArgs e)
            //=======================================================
            {
                lblUsername.Text = modMain.gUser.Name;
                modMain.gfrmLogIn.ChangePasswd = false;
                txtNewPassword.Text = "";
                txtOldPassword.Text = "";
            }

        #endregion

        #region "CONTROL RELATED ROUTINE"

            private void cmdOK_Click(object sender, EventArgs e)
            //==================================================
            {
                string pstrMsg = null;

                if (txtOldPassword.Text == modMain.gUser.Password)
                {
                    if (txtNewPassword.Text != "")
                    {
                        string pWHERE = "UPDATE tblUser SET fldPassword = '" + txtNewPassword.Text +
                                        "' WHERE fldName = '" + modMain.gUser.Name + "'";
                        modMain.gDB.ExecuteCommand(pWHERE);
                        modMain.gUser.Name = lblUsername.Text;
                        modMain.gfrmLogIn.ChangePasswd = true;
                    }
                    else
                    {
                        pstrMsg = "Please enter a Password.";

                        MessageBox.Show(pstrMsg, "Password Authentication",MessageBoxButtons.OK, 
                                        MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else
                {
                    pstrMsg = "Wrong User Password.";
                    MessageBox.Show(pstrMsg, "Password Authentication", MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);

                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtOldPassword.Focus();

                    return;
                }

                this.Close();
            }

            private void cmdCancel_Click(object sender, EventArgs e)
            //=====================================================
            {
                this.Close();
            }

        #endregion


    }
}
