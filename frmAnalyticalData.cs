//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmAnalyticalData                      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JAN13                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BearingCAD20
{
    public partial class frmAnalyticalData : Form
    {
        #region "MEMBER VARIABLE DECLARATION:"
        //************************************

            private RadioButton[] mOptThrust;
            private Boolean mblnCmd_XLKMC, mblnCmd_XLThrust;

        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmAnalyticalData()
            {
                InitializeComponent();
            }

        #endregion


        #region "FORM EVENT ROUTINES:"
        //*****************************

            private void frmAnalyticalData_Load(object sender, EventArgs e)
            //==============================================================
            {
                mOptThrust = new RadioButton[] { optThrust_Front, optThrust_Back };        
                grpThrust.Enabled = false;

                SetControls();
            }

            #region "Helper Routines:"
            //-----------------------

                private void SetControls()
                //========================
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            grpThrust.Enabled = true;
                            mOptThrust[i].Enabled = true;
                            mOptThrust[i].Checked = true;
                            cmdXLThrust.Enabled = true;
                        }
                        else
                        {
                            mOptThrust[i].Enabled = false;
                        }
                    }
                }

            #endregion

        #endregion


        #region "CONTROL EVENT ROUTINES:"
        //******************************

            #region"COMMAND BUTTON EVENT ROUTINES:"
            //------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;

                    switch (pcmdButton.Name)
                    {
                        case "cmdXLKMC":
                            //-----------
                            mblnCmd_XLKMC = true;
                            Import_Analytical_Data("tblMapping_XLKMC_V33");
                            mblnCmd_XLKMC = false;
                            break;

                        case "cmdXLThrust":
                            //-------------
                            mblnCmd_XLThrust = true;
                            Import_Analytical_Data("tblMapping_XLThrust_V22");
                            mblnCmd_XLThrust = false;
                            break;

                        case "cmdOK":
                            //--------
                             this.Hide();
                             modMain.gfrmOperCond.ShowDialog();
                             break;
                    }                   
                }


                #region "Helper Routines:"
                //************************

                    private void Import_Analytical_Data(String TableName_In)
                    //======================================================
                    {
                        string pExcelFileName = "";
                        openFileDialog1.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                        openFileDialog1.FilterIndex = 1;
                        openFileDialog1.InitialDirectory = "C:\\";
                        openFileDialog1.Title = "Open";
                        openFileDialog1.FileName = " ";

                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            pExcelFileName = openFileDialog1.FileName;

                            SqlConnection pConnection = new SqlConnection();
                            string pstrQuery = "Select * from " + TableName_In;
                            SqlDataReader pDR = modMain.gDB.GetDataReader(pstrQuery, ref pConnection);

                            List<string> pParameterName = new List<string>();
                            List<string> pExcelSheetName = new List<string>();
                            List<string> pCellName = new List<string>();

                            while (pDR.Read())
                            {
                                pParameterName.Add(Convert.ToString(pDR["fldParameter"]));
                                pExcelSheetName.Add(Convert.ToString(pDR["fldWorkSheet"]));
                                pCellName.Add(Convert.ToString(pDR["fldCellNo"]));

                            }
                            pDR.Close();

                            if (mblnCmd_XLKMC)
                            {
                                modMain.gEXCEL_Analysis.GetValFromExcel_Radial(modMain.gOpCond, (clsBearing_Radial_FP)modMain.gProject.Product.Bearing,
                                                                               pExcelFileName, pParameterName, pExcelSheetName, pCellName);
                            }

                            if (mblnCmd_XLThrust)
                            {
                                if (optThrust_Front.Checked)
                                {
                                    modMain.gEXCEL_Analysis.GetValFromExcel_Thrust(modMain.gOpCond, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0],
                                                                                   pExcelFileName, pParameterName, pExcelSheetName, pCellName);
                                }
                                else if (optThrust_Back.Checked)
                                {
                                    modMain.gEXCEL_Analysis.GetValFromExcel_Thrust(modMain.gOpCond, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1],
                                                                                   pExcelFileName, pParameterName, pExcelSheetName, pCellName);
                                }
                            }
                         
                        }
                    }

                #endregion
        
            #endregion

        #endregion
     
    }
}
