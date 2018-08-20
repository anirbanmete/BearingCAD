
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPerformanceDataThrustBearing        '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  20MAR12                                '
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
using System.Drawing.Printing;

namespace BearingCAD20
{
    public partial class frmPerformanceDataThrustBearing : Form
    {

        #region "MEMBER VARIABLE DECLARATION"
        //***********************************       
            


       
        #endregion


        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmPerformanceDataThrustBearing()
            {
                InitializeComponent();
            }

        #endregion


        #region "FORM LOAD RELATED EVENT"
        //*******************************

            private void frmPerformanceDataThrustBearing_Load(object sender, EventArgs e)
            //=============================================================================
            {
                string pText = "Performance Data - Thrust: ";

                //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                //    this.Text = pText + "Front";

                //else if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                //    this.Text = pText + "Back";

                //if (modMain.gfrmPerformanceData.mblnThrust_Front)
                //    this.Text = pText + "Front";

                //else if (modMain.gfrmPerformanceData.mblnThrust_Back)
                //    this.Text = pText + "Back";

                Initialize_LocalObject();              
               
                //....Data Display .
                DisplayData();
                //SetLocalObject();
                SetControls();           
            }


            private void Initialize_LocalObject()
            //===================================
            {               
                
            }


            private void SetControls()   
            //=======================                           
            {
                Boolean pEnabled;
                if (modMain.gProject.Status == "Open" &&
                    (modMain.gUser.Privilege == "Engineering"))
                {
                    pEnabled = true;
                    SetControlStatus(pEnabled);
                }
                else if (modMain.gProject.Status == "Closed" ||
                         modMain.gUser.Privilege == "Manufacturing" ||
                         modMain.gUser.Privilege == "Designer")
                {
                    pEnabled = false;
                    SetControlStatus(pEnabled);
                }
            }


            private void SetControlStatus(Boolean pEnable_In)
            //===============================================
            {               
                txtPower_HP.ReadOnly = !pEnable_In;
                txtPower_Met.ReadOnly = !pEnable_In;
                txtFlowReqd_gpm.ReadOnly = !pEnable_In;
                txtFlowReqd_Met.ReadOnly = !pEnable_In;
                txtTempRise_F.ReadOnly = !pEnable_In;
                txtTempRise_Met.ReadOnly = !pEnable_In;
                txtTFilm_Min.ReadOnly = !pEnable_In;

                txtPadMax_TempRise.ReadOnly = !pEnable_In;
                txtPadMax_Press.ReadOnly = !pEnable_In;               
                txtPadMax_Load.ReadOnly = !pEnable_In;
            }


            private void ReSetControlVal()
            //============================
            {
                const string pcBlank = "";

                txtPower_HP.Text = pcBlank;             //....Power
                txtFlowReqd_gpm.Text = pcBlank;         //....Flow Reqd
                txtTempRise_F.Text = pcBlank;           //....Temp Rise
                txtTFilm_Min.Text = pcBlank;            //....TFilmMin

                //  Pad Max
                //  -------
                txtPadMax_TempRise.Text = pcBlank;      //....Temp
                txtPadMax_Press.Text = pcBlank;         //....Pressure              
                txtPadMax_Load.Text = pcBlank;          //....Load
                
            }


            private void DisplayData()
            //========================
            {
                ////....Power
                ////txtPower_HP.Text = modMain.ConvDoubleToStr(modMain.gRadialBearing.Power_HP,"");
                //txtPower_HP.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP, "");

                ////....Flow Reqd
                //txtFlowReqd_gpm.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm, "");

                ////....Temp Rise
                ////txtTempRise.Text = modMain.ConvDoubleToStr(modMain.gBearing.TempRise_F,"");
                //txtTempRise_F.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F, "#0.0000");     //BG 08NOV11

                ////....TFilmMin
                ////txtTFilmMin.Text = modMain.ConvDoubleToStr(modMain.gBearing.TFilm_Min,"");
                //txtTFilm_Min.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min, "#0.0000");      //BG 08NOV11

                ////  Pad Max
                ////  -------

                ////....Temp
                ////txtPadMax_Temp.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.TempRise,"");      //BG 08NOV11
                //txtPadMax_TempRise.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.TempRise, "#0.0000");    //BG 08NOV11

                ////....Pressure
                ////txtPadMax_Press.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Press,"");
                //txtPadMax_Press.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Press, "#0.0000");
                                

                ////....Load
                ////txtPadMax_Load.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Load,"");
                //txtPadMax_Load.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Load, "#0.0000");
               
            }
             
        #endregion


        #region "CONTROL EVENT ROUTINES"
        //****************************
        
            private void cmdPrint_Click(object sender, EventArgs e)
            //=====================================================
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                modMain.CaptureScreen(this);
                pd.Print();
            }


            private void cmdOK_Click(object sender, EventArgs e)
            //==================================================
            {
                CloseForm();    
            }


            private void CloseForm()
            //======================
            {
                SaveData();
                this.Hide();                
            }

            private void SaveData()
            //=======================
            {
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP.Text);
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm.Text);
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F.Text);
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min = modMain.ConvTextToDouble(txtTFilm_Min.Text);

                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_TempRise = modMain.ConvTextToDouble(txtPadMax_TempRise.Text);
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Press = modMain.ConvTextToDouble(txtPadMax_Press.Text);
                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Load = modMain.ConvTextToDouble(txtPadMax_Load.Text);
                
            }

            private void cmdCancel_Click(object sender, EventArgs e)
            //======================================================
            {
                this.Hide();
            }

        #endregion

    }
}
