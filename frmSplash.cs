
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSplash                              '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  05APR13                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//   METHODS:
//   -------
//       Private Sub     frmSplash_Load      ()
//       Private Sub     cmdButtons_Click    ()
//
//================================================================================
//....Designer changed SB 03AUG09.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BearingCAD21
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        //================
        {
            InitializeComponent();
        }

     //*******************************************************************************
     //*                       FORM EVENT ROUTINES - BEGIN                           *
     //*******************************************************************************

        private void frmSplash_Load(object sender, EventArgs e)
        //=====================================================
        {
         modMain.LoadImageLogo(imgLogo);
        }

     //*******************************************************************************
     //*                       FORM EVENT ROUTINES - END                             *
     //*******************************************************************************


    //*******************************************************************************
    //*                    COMMAND BUTTON EVENT ROUTINE - BEGIN                     *
    //*******************************************************************************
        
        private void cmdButtons_Click(object sender, EventArgs e)
        //=======================================================
        {
            Button pCmdBtn = (Button)sender;

            switch (pCmdBtn.Name)
            {
                case "cmdMainForm":
                     this.Hide();
                    //this.Close();
                     //modMain.gfrmLogIn.ShowDialog();//SB 15JUN09
                     modMain.gfrmMain.Show();    
                     break;

                case "cmdExit":
                     System.Environment.Exit(0);    //SB 21APR09
                     break;
            }
        }

        //*******************************************************************************
        //*                    COMMAND BUTTON EVENT ROUTINE - END                       *
        //*******************************************************************************
    }
}
