
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPerformance                         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  19MAR12                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
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
using System.Drawing.Printing;

namespace BearingCAD20
{
    public partial class frmPerformanceDataRadialBearing : Form
    {
        #region "MEMBER VARIABLE DECLARATION"
        //***********************************

            private clsBearing_Radial_FP mBearing;// = new clsBearing_Radial_FP();  //SB 05JUN09

            //   Boolean Variables to indicate Control Event Type:   
            //  -------------------------------------------------
            //
            //  ....TextBoxes:
            //  ........If variable TRUE  ==> Changed by the user. 
            //  ........            FALSE ==> Value set programmatically.  

            Boolean mblntxtPowerEng_Entered = false;        //SB 09APR09
            Boolean mblntxtPowerMet_Entered = false;

            Boolean mblntxtFlowReqdEng_Entered = false;
            Boolean mblntxtFlowReqdMet_Entered = false;

            Boolean mblntxtTempRiseEng_Entered = false;
            Boolean mblntxtTempRiseMet_Entered = false;

        #endregion

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmPerformanceDataRadialBearing()
            //======================================
            {
                InitializeComponent();
            }

        #endregion

        #region "FORM LOAD RELATED EVENT"

            private void frmPerformance_Load(object sender, EventArgs e)
            //===========================================================
            {
                Initialize_LocalObject();
                SetTabPages();
                //MessageBox.Show("Form Performance");      //....For diagnostics.
    
                //....Set Boolean Variable for Power.
                mblntxtPowerMet_Entered = false;
                mblntxtPowerEng_Entered = true;

                //....Set Boolean Variable for FlowReqd.
                mblntxtFlowReqdMet_Entered = false;
                mblntxtFlowReqdEng_Entered = true;

                //....Set Boolean Variable for Tempurature Rise.
                mblntxtTempRiseMet_Entered = false;
                mblntxtTempRiseEng_Entered = true;

                //....Data Display & Local Object.
                DisplayData();
                SetLocalObject();
                SetControl();           //BG 15JUN09
           }

            private void Initialize_LocalObject()
            //===================================
            {
                 //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clone(ref mBearing);
                mBearing =(clsBearing_Radial_FP)((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clone();
            }


            private void SetTabPages()
            //========================
            { 
                if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Thrust_Bearing_TL &&
                    modMain.gProject.Product.EndConfig[1].Type != clsEndConfig.eType.Thrust_Bearing_TL)

                    tbBearing.TabPages.Remove(tabThrustBearing);

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)

                    tabTB.TabPages.Remove(tabTB_Back);

                else if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    tabTB.TabPages.Remove(tabTB_Front);

            }



            private void SetControl()   //BG 15JUN09
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
                         modMain.gUser.Privilege == "Manufacturing"|| 
                         modMain.gUser.Privilege == "Designer")               
                {
                    pEnabled = false;
                    SetControlStatus(pEnabled);
                }

            }

            private void SetControlStatus(Boolean pEnable_In)      
            //===============================================
            {
                //....Bore Dia.
                txtPower_HP.ReadOnly = !pEnable_In;
                txtPower_Met.ReadOnly = !pEnable_In;
                txtFlowReqd_gpm.ReadOnly = !pEnable_In;
                txtFlowReqd_Met.ReadOnly = !pEnable_In;
                txtTempRise_F.ReadOnly = !pEnable_In;
                txtTempRise_Met.ReadOnly = !pEnable_In;
                txtTFilm_Min.ReadOnly = !pEnable_In;
                txtPadMax_TempRise.ReadOnly = !pEnable_In;
                txtPadMax_Press.ReadOnly = !pEnable_In;
                txtPadMax_Rot.ReadOnly = !pEnable_In;
                txtPadMax_Load.ReadOnly = !pEnable_In;
                txtPadMax_Stress.ReadOnly = !pEnable_In;

            }

            private void ReSetControlVal()
            //============================
            {
                const string pcBlank = "";

                txtPower_HP.Text = pcBlank;            //....Power
                txtFlowReqd_gpm.Text = pcBlank;         //....Flow Reqd
                txtTempRise_F.Text = pcBlank;         //....Temp Rise
                txtTFilm_Min.Text = pcBlank;         //....TFilmMin

                //  Pad Max
                //  -------
                    txtPadMax_TempRise.Text = pcBlank;      //....Temp
                    txtPadMax_Press.Text = pcBlank;     //....Pressure
                    txtPadMax_Rot.Text = pcBlank;       //....Rotation
                    txtPadMax_Load.Text = pcBlank;      //....Load
                    txtPadMax_Stress.Text = pcBlank;    //....Stress
            }


            private void DisplayData()
            //========================
            {
                //....Power
                //txtPower_HP.Text = modMain.ConvDoubleToStr(modMain.gRadialBearing.Power_HP,"");
                txtPower_HP.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP, "");

                //....Flow Reqd
                txtFlowReqd_gpm.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm, "");

                //....Temp Rise
                //txtTempRise.Text = modMain.ConvDoubleToStr(modMain.gBearing.TempRise_F,"");
                txtTempRise_F.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F, "#0.0000");     //BG 08NOV11
                
                //....TFilmMin
                //txtTFilmMin.Text = modMain.ConvDoubleToStr(modMain.gBearing.TFilm_Min,"");
                txtTFilm_Min.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min, "#0.0000");      //BG 08NOV11

                //  Pad Max
                //  -------

                    //....Temp
                    //txtPadMax_Temp.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.TempRise,"");      //BG 08NOV11
                txtPadMax_TempRise.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.TempRise, "#0.0000");    //BG 08NOV11

                    //....Pressure
                    //txtPadMax_Press.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Press,"");
                txtPadMax_Press.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Press, "#0.0000");

                    //....Rotation    
                    //txtPadMax_Rot.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Rot,"");
                txtPadMax_Rot.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Rot, "#0.0000");

                    //....Load
                    //txtPadMax_Load.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Load,"");
                txtPadMax_Load.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Load, "#0.0000");

                    //....Stress
                    //txtPadMax_Stress.Text = modMain.ConvDoubleToStr(modMain.gBearing.Pad_Perform_Max.Stress,"");
                txtPadMax_Stress.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Stress, "#0.0000");
            }


            private void SetLocalObject()
            //===========================
            {
                mBearing.PerformData.Power_HP = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP;          //....Power
                mBearing.PerformData.FlowReqd_gpm = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm;  //....Flow Reqd
                mBearing.PerformData.TempRise_F = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F;      //....Temp Rise
                mBearing.PerformData.TFilm_Min = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min;        //....TFilmMin

                //  Pad Max
                //  -------
                mBearing.PerformData.PadMax_Load = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Load;           //....Temp
                mBearing.PerformData.PadMax_Press = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Press;         //....Pressure
                mBearing.PerformData.PadMax_Rot = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Rot;             //....Rotation
                mBearing.PerformData.PadMax_Stress = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Stress;       //....Load
                mBearing.PerformData.PadMax_TempRise = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.TempRise;   //....Stress


            }

        #endregion

        #region "CONTROL EVENT ROUTINE"
            //*************************

            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

                private void cmdPrint_Click(object sender, EventArgs e)
               //=======================================================       
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }

                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    CloseForm();                
                }

                private void CloseForm()        
                //======================
                {

                    SaveData();
                    //Cursor = Cursors.WaitCursor;
                    //....AM 02MAR12 SG/BG Move to clsDB.
                    //modMain.gRadialBearing.UpdateRec_Bearing(modMain.gProject.No, "Performance", modMain.gProject.Product);
                    //Cursor = Cursors.Default;
                    //SaveData_Seal();

                    this.Hide();

                    //if (modMain.gProject.Customer.Contains("Kobe") && modMain.gUnit.System.ToString() == "Metric")
                    //{
                        //modMain.gfrmBearingDesignDetails.ShowDialog();   //SB 31JUL09
                    //}
                }

                private void SaveData()
                //=======================
                {
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min = modMain.ConvTextToDouble(txtTFilm_Min.Text);


                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_TempRise = modMain.ConvTextToDouble(txtPadMax_TempRise.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Press = modMain.ConvTextToDouble(txtPadMax_Press.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Rot = modMain.ConvTextToDouble(txtPadMax_Rot.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Load = modMain.ConvTextToDouble(txtPadMax_Load.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Stress = modMain.ConvTextToDouble(txtPadMax_Stress.Text);
                }

                private void SaveData_Seal()
                //==========================
                {
                    ////modMain.gSeal.DrainHole_Flow_gpm = modMain.gBearing.FlowReqd_gpm;     //BG 21NOV11      

                    //if (modMain.gProject.Product.EndConfig[0].Type== clsEndConfig.eType.Seal)
                    //{
                    //    modMain.gEndSeal[0].DrainHole_Flow_gpm = modMain.gRadialBearing.FlowReqd_gpm;
                       
                    //}
                    //if (modMain.gRadialBearing.EndConfig_Back == clsBearing_Radial_FP.eEndConfig.Seal)
                    //{
                    //    modMain.gEndSeal[1].DrainHole_Flow_gpm = modMain.gRadialBearing.FlowReqd_gpm;
                    //}
                   
                }

                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {
                    //if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Compare(mBearing, "Performance"))  
                    //{
                    //    int pAns = (int)MessageBox.Show("Data has been modified in this form." +
                    //            System.Environment.NewLine + "Do you want to save before exiting?"
                    //            , "Save Record", MessageBoxButtons.YesNo,
                    //            MessageBoxIcon.Question);

                    //    const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

                    //    if (pAns == pAnsY)
                    //    {
                    //        CloseForm();
                    //    }
                    //    else
                    //    {
                    //        this.Hide();
                    //    }
                    //}
                    //else
                    //{
                    //    this.Hide();
                    //}

                    this.Hide();

                }

            #endregion

            #region "TEXTBOX RELATED ROUTINE"
            //-------------------------------

                #region "POWER"

                    private void txtPower_TextChanged(object sender, EventArgs e)
                    //===========================================================
                    {
                        if (!mblntxtPowerEng_Entered) return;

                        txtPower_HP.ForeColor = Color.Black;
                        txtPower_Met.ForeColor = Color.Blue;

                        if (txtPower_HP.Text == "")
                        {
                            txtPower_Met.Text = "";
                            return;
                        }

                        else
                        {
                            Double pPower = modMain.ConvTextToDouble(txtPower_HP.Text);
                            txtPower_Met.Text = modMain.gUnit.CFac_Power_EngToMet(pPower).ToString("#0.0"); //SB 21APR09
                            
                        }
                        mBearing.PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP.Text);        
                    }

                    private void txtPower_Met_TextChanged(object sender, EventArgs e)       
                    //===============================================================
                    {
                        if (!mblntxtPowerMet_Entered) return;

                        txtPower_Met.ForeColor = Color.Black;
                        txtPower_HP.ForeColor = Color.Blue;

                        if (txtPower_Met.Text == "")
                        {
                            txtPower_HP.Text = "";
                            return;
                        }

                        else
                        {
                            Double pPower = modMain.ConvTextToDouble(txtPower_Met.Text);
                            txtPower_HP.Text = modMain.gUnit.CFac_Power_MetToEng(pPower).ToString("#0.0"); //SB 21APR09
                        }
                        mBearing.PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP.Text);

                    }

                    private void txtPower_Met_MouseDown(object sender, MouseEventArgs e)
                    //==================================================================
                    {
                        mblntxtPowerMet_Entered = true;
                        mblntxtPowerEng_Entered = false;
                    }

                    private void txtPower_Met_KeyDown(object sender, KeyEventArgs e)    //SB 16JUL09
                    //==============================================================
                    {
                        mblntxtPowerMet_Entered = true;
                        mblntxtPowerEng_Entered = false;
                    }

                    private void txtPower_MouseDown(object sender, MouseEventArgs e)
                    //==============================================================
                    {
                        mblntxtPowerMet_Entered = false;
                        mblntxtPowerEng_Entered = true;
                    }

                    private void txtPower_KeyDown(object sender, KeyEventArgs e)        //SB 16JUL09
                    //==========================================================
                    {
                        mblntxtPowerMet_Entered = false;
                        mblntxtPowerEng_Entered = true;
                    }

                #endregion

                #region "FLOW REQD"

                    private void txtFlowReqd_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                        if (!mblntxtFlowReqdEng_Entered) return;

                        txtFlowReqd_gpm.ForeColor = Color.Black;
                        txtFlowReqd_Met.ForeColor = Color.Blue;

                        if (txtFlowReqd_gpm.Text == "")
                        {
                            txtFlowReqd_Met.Text = "";
                            return;
                        }

                        else
                        {
                            Double pFlowReqd = modMain.ConvTextToDouble(txtFlowReqd_gpm.Text);
                            txtFlowReqd_Met.Text = modMain.gUnit.CFac_GPM_EngToMet(pFlowReqd).ToString("#0.0");//SB 21APR09
                        }

                        mBearing.PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm.Text); 
                    }

                    private void txtFlowReqd_Met_TextChanged(object sender, EventArgs e)
                    //==================================================================
                    {
                        if (!mblntxtFlowReqdMet_Entered) return;

                        txtFlowReqd_gpm.ForeColor = Color.Blue;
                        txtFlowReqd_Met.ForeColor = Color.Black;

                        if (txtFlowReqd_Met.Text == "")
                        {
                            txtFlowReqd_gpm.Text = "";
                            return;
                        }

                        else
                        {
                            Double pFlowReqd = modMain.ConvTextToDouble(txtFlowReqd_Met.Text);
                            txtFlowReqd_gpm.Text = modMain.gUnit.CFac_LPM_MetToEng(pFlowReqd).ToString("#0.0");//SB 21APR09
                        }

                        mBearing.PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm.Text); 

                    }

                    private void txtFlowReqd_Met_MouseDown(object sender, MouseEventArgs e)
                    //=====================================================================
                    {
                        mblntxtFlowReqdMet_Entered = true;
                        mblntxtFlowReqdEng_Entered = false;
                    }


                    private void txtFlowReqd_Met_KeyDown(object sender, KeyEventArgs e)
                    //=================================================================     
                    {
                        mblntxtFlowReqdMet_Entered = true;
                        mblntxtFlowReqdEng_Entered = false;
                    }

                    private void txtFlowReqd_MouseDown(object sender, MouseEventArgs e)
                    //=================================================================
                    {
                        mblntxtFlowReqdEng_Entered = true;
                        mblntxtFlowReqdMet_Entered = false;
                    }

                    private void txtFlowReqd_KeyDown(object sender, KeyEventArgs e)
                    //=============================================================         
                    {
                        mblntxtFlowReqdEng_Entered = true;
                        mblntxtFlowReqdMet_Entered = false;
                    }

                #endregion

                #region " TEMP RISE"

                    private void txtTempRise_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                        if (!mblntxtTempRiseEng_Entered) return;

                        txtTempRise_F.ForeColor = Color.Black;
                        txtTempRise_Met.ForeColor = Color.Blue;

                        if (txtTempRise_F.Text == "")
                        {
                            txtTempRise_Met.Text = "";
                            return;
                        }

                        else
                        {
                            Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F.Text);
                            int pTempRise_Met = (int)modMain.NInt(modMain.gUnit.CFac_TRise_EngToMet(pTempRise));
                            txtTempRise_Met.Text = pTempRise_Met.ToString();
                        }

                        mBearing.PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F.Text);   
                    }

                    private void txtTempRise_Met_TextChanged(object sender, EventArgs e)
                    //==================================================================
                    {
                        if (!mblntxtTempRiseMet_Entered) return;

                        txtTempRise_F.ForeColor = Color.Blue;
                        txtTempRise_Met.ForeColor = Color.Black;

                        if (txtTempRise_Met.Text == "")
                        {
                            txtTempRise_F.Text = "";
                            return;
                        }

                        else
                        {
                            Double pTempRise_Eng = modMain.ConvTextToDouble(txtTempRise_Met.Text);  
                            int pTempRise = (int)modMain.NInt(modMain.gUnit.CFac_TRise_MetToEng(pTempRise_Eng));
                            txtTempRise_F.Text = pTempRise.ToString();
                        }

                        mBearing.PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F.Text);   
                    }


                    private void txtTempRise_MouseDown(object sender, MouseEventArgs e)
                    //=================================================================
                    {
                        mblntxtTempRiseEng_Entered = true;
                        mblntxtTempRiseMet_Entered = false;
                    }

                    private void txtTempRise_KeyDown(object sender, KeyEventArgs e)
                    //=============================================================     //SB 16JUL09
                    {
                        mblntxtTempRiseEng_Entered = true;
                        mblntxtTempRiseMet_Entered = false;
                    }

                    private void txtTempRise_Met_MouseDown(object sender, MouseEventArgs e)
                    //=====================================================================
                    {
                        mblntxtTempRiseEng_Entered = false;
                        mblntxtTempRiseMet_Entered = true;
                    }

                    private void txtTempRise_Met_KeyDown(object sender, KeyEventArgs e)
                    //================================================================= //SB 16JUL09
                    {
                        mblntxtTempRiseEng_Entered = false;
                        mblntxtTempRiseMet_Entered = true;
                    }

                #endregion

                #region "TEXTBOX TEXT CHENGE RELATED ROUTINE"

                    private void TextBox_TextChanged(object sender, EventArgs e)
                    //============================================================  //BG 23APR09
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtTFilmMin":
                                //==============
                                mBearing.PerformData.TFilm_Min = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;

                            case "txtPadMax_TempRise":
                                //====================
                                mBearing.PerformData.PadMax_TempRise = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;

                            case "txtPadMax_Press":
                                //==================
                                mBearing.PerformData.PadMax_Press = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;

                            case "txtPadMax_Rot":
                                //================
                                mBearing.PerformData.PadMax_Rot = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;

                            case "txtPadMax_Load":
                                //=================
                                mBearing.PerformData.PadMax_Load = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;

                            case "txtPadMax_Stress":
                                //===================
                                mBearing.PerformData.PadMax_Stress = modMain.ConvTextToDouble(pTxtBox.Text);
                                break;
                        }
                    }

                #endregion

                   

            #endregion

        #endregion

    }
}
