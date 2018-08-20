//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPerformanceData                     '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  15MAR12                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//   METHODS:
//   -------

//       Private Sub       cmdDisplay_Click                    ()
//       Private Sub       SaveData                            ()
//===============================================================================

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
    public partial class frmPerformanceData : Form
    {
        public Boolean mblnThrust_Front, mblnThrust_Back;
        public frmPerformanceData()
        {
            InitializeComponent();
        }

        private void frmPerformanceData_Load(object sender, EventArgs e)
        //==============================================================
        {
            optBearing_Thrust_Front.Enabled = false;
            optBearing_Thrust_Back.Enabled = false;

            if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
            {
                optBearing_Thrust_Front.Enabled = true;
            }

            if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
            {
                optBearing_Thrust_Back.Enabled = true;          
            }

        }

        private void cmdDisplay_Click(object sender, EventArgs e)
        //=======================================================
        {
            if (optBearing_Radial.Checked)
            {
                modMain.gfrmPerformanceDataRadialBearing.ShowDialog();
            }
            else if (optBearing_Thrust_Front.Checked)
            {
                mblnThrust_Front = true;
                mblnThrust_Back = false;
                modMain.gfrmPerformanceDataThrustBearing.ShowDialog();
               
            }
            else if (optBearing_Thrust_Back.Checked)
            {
                mblnThrust_Back = true;
                mblnThrust_Front = false;
                modMain.gfrmPerformanceDataThrustBearing.ShowDialog();               
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            modMain.gfrmBearingDesignDetails.ShowDialog();   
        }


    }
}
