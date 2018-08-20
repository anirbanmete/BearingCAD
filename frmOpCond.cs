
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmOperCond                            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
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
//.....Designer changed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace BearingCAD20
{
    public partial class frmOpCond : Form
    {
        #region "MEMBER VARIABLE"
        //***********************

            private clsOpCond mOpCond = new clsOpCond();

            //   Boolean Variables to indicate Control Event Type:   
            //  -------------------------------------------------
            //
            //  ....TextBoxes:
            //  ........If variable TRUE  ==> Changed by the user. 
            //  ........            FALSE ==> Value set programmatically.  

            //Boolean mblntxtSpeedMinMax_Entered = false;   //SB 20MAY09
            //Boolean mblntxtSpeedDesign_Entered = false;

            //Boolean mblntxtLoadMinMax_Entered = false;
            //Boolean mblntxtLoadDesign_Entered = false;

            //Boolean mblntxtLoadEng_Entered = false;
            //Boolean mblntxtLoadMet_Entered = false;

            Boolean mblntxtPressEng_Entered = false;
            Boolean mblntxtPressMet_Entered = false;

            Boolean mblntxtTempEng_Entered = false;
            Boolean mblntxtTempMet_Entered = false;

            //GroupBox Array
            //-------------
            private GroupBox[] mGrpThrust;

            private CheckBox[] mChkThrust_Load_Design;
            private CheckBox[] mChkThrust_Load_Eng;

            private TextBox[] mTxtThrust_Load_Range_Min_Eng, mTxtThrust_Load_Design_Eng;
            private TextBox[] mTxtThrust_Load_Range_Max_Eng, mTxtThrust_Load_Range_Min_Met;
            private TextBox[] mTxtThrust_Load_Design_Met, mTxtThrust_Load_Range_Max_Met;

            private Boolean mblnCmd_XLKMC, mblnCmd_XLThrust_Front, mblnCmd_XLThrust_Back;
           
        #endregion


        #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

            public frmOpCond()
            //====================
            {
                InitializeComponent();

                //.....Initialize Text Rotaion.
                //txtRot_Direction.Text = "CCW";

                //.....Initialize Lube type.
                LoadOilSupply_Lube_Type();

                //.....Initialize Oilsupply Type.
                LoadOilSupply_Type();

                //.....Initialize Temp Labels
                lblTempDegF.Text = Convert.ToString((char)176);
                lblTempDegC.Text = Convert.ToString((char)176);

                mGrpThrust = new GroupBox[] { grpStaticLoad_Thrust_Front, grpStaticLoad_Thrust_Back };

                mChkThrust_Load_Design = new CheckBox[] { chkThrust_Load_Eng_Design_Front, 
                                                          chkThrust_Load_Eng_Design_Back };

                mChkThrust_Load_Eng = new CheckBox[] { chkThrust_Load_Eng_Front, chkThrust_Load_Eng_Back };

                mTxtThrust_Load_Range_Min_Eng = new TextBox[] { txtThrust_Load_Range_Min_Eng_Front, 
                                                                txtThrust_Load_Range_Min_Eng_Back };
                mTxtThrust_Load_Design_Eng = new TextBox[] { txtThrust_Load_Design_Eng_Front, 
                                                             txtThrust_Load_Design_Eng_Back };
                mTxtThrust_Load_Range_Max_Eng = new TextBox[] { txtThrust_Load_Range_Max_Eng_Front, 
                                                                txtThrust_Load_Range_Max_Eng_Back };
                mTxtThrust_Load_Range_Min_Met = new TextBox[] { txtThrust_Load_Range_Min_Met_Front, 
                                                                txtThrust_Load_Range_Min_Met_Back };
                mTxtThrust_Load_Design_Met = new TextBox[] { txtThrust_Load_Design_Met_Front, 
                                                             txtThrust_Load_Design_Met_Back };
                mTxtThrust_Load_Range_Max_Met = new TextBox[] { txtThrust_Load_Range_Max_Met_Front, 
                                                                txtThrust_Load_Range_Max_Met_Back };

            }

            private void LoadOilSupply_Lube_Type()
            //====================================
            {
                int pRecCount = modMain.gDB.PopulateCmbBox(cmbLube_Type, "tblData_Lube", "fldType", "", true);
                if (pRecCount > 0) cmbLube_Type.SelectedIndex = -1;
            }

            private void LoadOilSupply_Type()
            //===============================
            {
                cmbOilSupply_Type.Items.Clear();
                cmbOilSupply_Type.Items.Add("Pressurized");
                cmbOilSupply_Type.Items.Add("Flooded Bath");
                //cmbOilSupply_Type.SelectedIndex = -1;
                cmbOilSupply_Type.SelectedIndex = 0;        //BG 04OCT12
            }

        #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

            private void frmOperCond_Load(object sender, EventArgs e)   
            //========================================================
            {
                //....Set Boolean for Pressure.
                mblntxtPressEng_Entered = false;    
                mblntxtPressMet_Entered = true;     

                //....Set Boolean for Tempurature.
                mblntxtTempEng_Entered = true;
                mblntxtTempMet_Entered = false;

                //....Reset Diff Control value.
                ResetControlVal();

                //....Set Local Object.                    
                SetLocalObject();

                //....DisplayData.
                DisplayData();

                //....Set Controls
                SetControl();
               
            }

            private void SetControl()
            //=======================
            {
                Boolean pEnabled = false;
                SetControlStatus(pEnabled);
                     
                if (modMain.gProject != null)
                {
                    //if (modMain.gProject.Status == "Open" &&
                    //    (modMain.gUser.Privilege == "Engineering"))       //BG 28JUN13
    
                   if (modMain.gProject.Status == "Open" &&
                       modMain.gUser.Role == "Engineer")       //BG 28JUN13
                    {
                        pEnabled = true;
                        SetControlStatus(pEnabled);

                        lblStaticLoad_Thrust.Enabled = false;

                        for (int i = 0; i < 2; i++)
                        {
                            if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            {
                                lblStaticLoad_Thrust.Enabled = true;
                                mGrpThrust[i].Enabled = true;

                                Set_VersionNo_Import_Button("tblMapping_XLThrust", cmdImport_XLThrust_Front, "Import From &XLThrust ");
                                Set_VersionNo_Import_Button("tblMapping_XLThrust", cmdImport_XLThrust_Back, "Import From &XLThrust ");
                            }
                            else
                            {
                                mGrpThrust[i].Enabled = false;
                            }
                        }
                    }
                    
                    //BG 28JUN13
                    //else if (modMain.gProject.Status == "Closed" ||
                    //    modMain.gUser.Privilege == "Manufacturing" ||
                    //     modMain.gUser.Privilege == "Designer")

                    else
                    {
                        pEnabled = false;
                        //SetControlStatus(pEnabled);       //BG 28JUN13
                        SetControlColors(pEnabled);
                    }

                    Set_VersionNo_Import_Button("tblMapping_XLRADIAL", cmdImport_XLKMC, "Import From &XLRADIAL ");
                }
            }

            private void SetControlStatus(Boolean pEnable_In)
            //===============================================        
            {
                //  Speed
                //  -----
                txtSpeed_Range_Min.ReadOnly = !pEnable_In;
                txtSpeed_Design.ReadOnly = !pEnable_In;
                txtSpeed_Range_Max.ReadOnly = !pEnable_In;
                chkSpeed_Design.Enabled = pEnable_In;

                //  Static Load: Radial
                //  -------------------
                txtRadial_Load_Range_Min_Eng.ReadOnly = !pEnable_In;
                txtRadial_Load_Range_Max_Eng.ReadOnly = !pEnable_In;
                txtRadial_Load_Design_Eng.ReadOnly = !pEnable_In;

                chkRadial_Load_Eng.Enabled = pEnable_In;
                chkRadial_Load_Eng_Design.Enabled = pEnable_In;

                txtRadial_Load_Range_Min_Met.ReadOnly = !pEnable_In;
                txtRadial_Load_Range_Max_Met.ReadOnly = !pEnable_In;
                txtRadial_Load_Design_Met.ReadOnly = !pEnable_In;

                ////  Static Load: Thrust
                ////  -------------------
                //txtThrust_Load_Range_Min_Eng_Front.ReadOnly = !pEnable_In;
                //txtThrust_Load_Range_Max_Eng_Front.ReadOnly = !pEnable_In;
                //txtThrust_Load_Design_Eng_Front.ReadOnly = !pEnable_In;

                //chkThrust_Load_Eng_Front.Enabled = pEnable_In;
                //chkThrust_Load_Eng_Design_Front.Enabled = pEnable_In;

                //txtThrust_Load_Range_Min_Met_Front.ReadOnly = !pEnable_In;
                //txtThrust_Load_Design_Met_Front.ReadOnly = !pEnable_In;
                //txtThrust_Load_Range_Max_Met_Front.ReadOnly = !pEnable_In;

                //  Load Angle
                //  ----------
                txtRadial_LoadAng.ReadOnly = !pEnable_In;
                grpRot_Directionality.Enabled = pEnable_In;

                // Import XLRADIAL and XLThrust   
                cmdImport_XLKMC.Enabled = pEnable_In;
                //cmdImport_XLThrust_Front.Enabled = pEnable_In;
                //cmdImport_XLThrust_Back.Enabled = pEnable_In;
                grpStaticLoad_Thrust_Front.Enabled = pEnable_In;
                grpStaticLoad_Thrust_Back.Enabled = pEnable_In;

                //  Lubricant Type
                //  --------------
                cmbLube_Type.Enabled = pEnable_In;
                cmbOilSupply_Type.Enabled = pEnable_In;

                txtOilSupply_Press_Eng.ReadOnly = !pEnable_In;
                txtOilSupply_Press_Met.ReadOnly = !pEnable_In;

                txtOilSupply_Temp_Eng.ReadOnly = !pEnable_In;
                txtOilSupply_Temp_Met.ReadOnly = !pEnable_In;
            }

            private void SetControlColors(Boolean pEnable_In)
            //=============================================== 
            {
                Color pControl = txtLube_Temp_a.BackColor; 
                if (pEnable_In)
                {
                }
                else
                {
                    txtSpeed_Range_Min.BackColor = pControl;
                    txtSpeed_Design.BackColor = pControl;
                    txtSpeed_Range_Max.BackColor = pControl;

                    txtRadial_Load_Range_Min_Eng.BackColor = pControl;
                    txtRadial_Load_Range_Max_Eng.BackColor = pControl;
                    txtRadial_Load_Design_Eng.BackColor = pControl;
                }
            }

            private void SetControl_Open()  
            //============================
            {
                //  Speed
                //  -----
                txtSpeed_Range_Min.ReadOnly = false;
                txtSpeed_Design.ReadOnly = false;
                txtSpeed_Range_Max.ReadOnly = false;
                chkSpeed_Design.Enabled = true;

                //  Static Load Radial
                //  ------------

                txtRadial_Load_Range_Min_Eng.ReadOnly = false;
                txtRadial_Load_Range_Max_Eng.ReadOnly = false;
                txtRadial_Load_Design_Eng.ReadOnly = false;

                chkRadial_Load_Eng.Enabled = true;
                chkRadial_Load_Eng_Design.Enabled = true;

                txtRadial_Load_Range_Min_Met.ReadOnly = false;
                txtRadial_Load_Range_Max_Met.ReadOnly = false;
                txtRadial_Load_Design_Met.ReadOnly = false;


                ////  Static Load
                ////  ------------

                //txtThrust_Load_Range_Min_Eng_Front.ReadOnly = false;
                //txtThrust_Load_Range_Max_Eng_Front.ReadOnly = false;
                //txtThrust_Load_Design_Eng_Front.ReadOnly = false;

                //chkThrust_Load_Eng_Front.Enabled = true;
                //chkThrust_Load_Eng_Design_Front.Enabled = true;

                //txtThrust_Load_Range_Min_Met_Front.ReadOnly = false;
                //txtThrust_Load_Range_Max_Met_Front.ReadOnly = false;
                //txtThrust_Load_Design_Met_Front.ReadOnly = false;

                //  Load Angle
                //  ----------
                txtRadial_LoadAng.ReadOnly = false;
                grpRot_Directionality.Enabled = true;

                //  Lubricant Type
                //  --------------
                cmbLube_Type.Enabled = true;
                cmbOilSupply_Type.Enabled = true;

                txtOilSupply_Press_Eng.ReadOnly = false;
                txtOilSupply_Press_Met.ReadOnly = false;

                txtOilSupply_Temp_Eng.ReadOnly = false;
                txtOilSupply_Temp_Met.ReadOnly = false;
            }

            private void SetControl_Close()        
            //=============================
            {
                Color pControl = txtLube_Temp_a.BackColor; 
                //  Speed
                //  -----
                    txtSpeed_Range_Min.ReadOnly = true;
                    txtSpeed_Range_Min.BackColor = pControl;
                    txtSpeed_Design.ReadOnly = true;
                    txtSpeed_Design.BackColor = pControl;
                    txtSpeed_Range_Max.ReadOnly = true;
                    txtSpeed_Range_Max.BackColor = pControl;
                    chkSpeed_Design.Enabled = false;
                    
                //  Static Load
                //  ------------
                    txtRadial_Load_Range_Min_Eng.ReadOnly = true;
                    txtRadial_Load_Range_Min_Eng.BackColor = pControl;
                    txtRadial_Load_Range_Max_Eng.ReadOnly = true;
                    txtRadial_Load_Range_Max_Eng.BackColor = pControl;
                    txtRadial_Load_Design_Eng.ReadOnly = true;
                    txtRadial_Load_Design_Eng.BackColor = pControl;

                    chkRadial_Load_Eng.Enabled = false;
                    chkRadial_Load_Eng_Design.Enabled = false;

                    txtRadial_Load_Range_Min_Met.ReadOnly = true;
                    txtRadial_Load_Range_Min_Met.ReadOnly = true;
                    txtRadial_Load_Design_Met.ReadOnly = false;

                
                //  Load Angle
                //  ----------
                    txtRadial_LoadAng.ReadOnly = true;
                    grpRot_Directionality.Enabled = false;
             

                //  Lubricant Type
                //  --------------
                    cmbLube_Type.Enabled = false;
                    cmbOilSupply_Type.Enabled = false;

                    txtOilSupply_Press_Eng.ReadOnly = true;
                    txtOilSupply_Press_Met.ReadOnly = true;

                    txtOilSupply_Temp_Eng.ReadOnly = true;
                    txtOilSupply_Temp_Met.ReadOnly = true;

            }

            private void SetLocalObject()
            //===========================
            {
                mOpCond = (clsOpCond)modMain.gOpCond.Clone();
            }

            private void ResetControlVal()      
            //============================
            {
                const string pcBlank = "";  

                //  Speed
                //  -----
                    txtSpeed_Range_Min.Text = pcBlank;
                    txtSpeed_Design.Text = pcBlank;
                    txtSpeed_Range_Max.Text = pcBlank;

                //  Rotation
                //  --------
                    //txtRot_Direction.Text = mOpCond.Rot_Direction.ToString();
   
                //  Load
                //  ----

                    //....English
                    txtRadial_Load_Range_Min_Eng.Text = pcBlank;
                    txtRadial_Load_Design_Eng.Text = pcBlank;
                    txtRadial_Load_Range_Max_Eng.Text = pcBlank;

                    //....Metric
                    txtRadial_Load_Range_Min_Met.Text = pcBlank;
                    txtRadial_Load_Design_Met.Text = pcBlank;
                    txtRadial_Load_Range_Max_Met.Text = pcBlank;

                //  Load Angle
                //  ----------
                    txtRadial_LoadAng.Text = pcBlank;

                //  Load: Thrust
                //  --------------

                    //....Front

                    //....English
                    txtThrust_Load_Range_Min_Eng_Front.Text = pcBlank;
                    txtThrust_Load_Design_Eng_Front.Text = pcBlank;
                    txtThrust_Load_Range_Max_Eng_Front.Text = pcBlank;

                    //....Metric
                    txtThrust_Load_Range_Min_Met_Front.Text = pcBlank;
                    txtThrust_Load_Design_Met_Front.Text = pcBlank;
                    txtThrust_Load_Range_Max_Met_Front.Text = pcBlank;

                    //....Back

                    //....English
                    txtThrust_Load_Range_Min_Eng_Back.Text = pcBlank;
                    txtThrust_Load_Design_Eng_Back.Text = pcBlank;
                    txtThrust_Load_Range_Max_Eng_Back.Text = pcBlank;

                    //....Metric
                    txtThrust_Load_Range_Min_Met_Back.Text = pcBlank;
                    txtThrust_Load_Design_Met_Back.Text = pcBlank;
                    txtThrust_Load_Range_Max_Met_Back.Text = pcBlank;

                //  Press
                //  -----
                    txtOilSupply_Press_Eng.Text = pcBlank;
                    txtOilSupply_Press_Met.Text = pcBlank;

                //  Temp
                //  ----
                    txtOilSupply_Temp_Eng.Text = pcBlank;
                    txtOilSupply_Temp_Met.Text = pcBlank;
            }

            private void Set_VersionNo_Import_Button(String TableName_In, Button Btn_In, String Caption_In)
            //=============================================================================================
            {
                SqlConnection pConnection = new SqlConnection();
                string pstrQuery = "Select fldVersionNo from " + TableName_In;
                SqlDataReader pDR = modMain.gDB.GetDataReader(pstrQuery, ref pConnection);
                String pVerSionNo = "";
           
                while (pDR.Read())
                {
                    if (!Convert.IsDBNull(pDR["fldVersionNo"]))
                    {
                        pVerSionNo = Convert.ToString(pDR["fldVersionNo"]);
                        break;
                   }                 
                }
                
                Btn_In.Text = Caption_In + pVerSionNo;
                pDR.Close();

            }

            private void DisplayData()
            //========================
            {
               
                //  Speed
                //  -----
                if (mOpCond.Speed_Range[0] != mOpCond.Speed() &&
                    mOpCond.Speed_Range[1] != mOpCond.Speed())
                {
                    chkSpeed_Design.Checked = false;
                    txtSpeed_Range_Min.Text = modMain.ConvDoubleToStr(mOpCond.Speed_Range[0], "#0");
                    txtSpeed_Design.Text = modMain.ConvDoubleToStr(mOpCond.Speed(), "#0");
                    txtSpeed_Range_Max.Text = modMain.ConvDoubleToStr(mOpCond.Speed_Range[1], "#0");

                }
                else if (mOpCond.Speed_Range[0] == mOpCond.Speed() &&
                         mOpCond.Speed_Range[1] == mOpCond.Speed())
                {
                    chkSpeed_Design.Checked = true;
                    txtSpeed_Design.Text = modMain.ConvDoubleToStr(mOpCond.Speed(), "#0");
                }

                SetSpeedDesign();

                //  Rotation
                //  --------
                //txtRot_Direction.Text = mOpCond.Rot_Direction.ToString();    

                //  Directionality
                //  --------------
                if (mOpCond.Rot_Directionality.ToString() == "Bi")
                    optRot_Directionality_Bi.Checked = true;
                else if (mOpCond.Rot_Directionality.ToString() == "Uni")
                    optRot_Directionality_Uni.Checked = true;

                //  Load: Radial
                //  -------------
                chkRadial_Load_Eng.Checked = true;

                //....English
                if (mOpCond.Radial_Load_Range[0] != mOpCond.Radial_Load() &&
                    mOpCond.Radial_Load_Range[1] != mOpCond.Radial_Load())
                {
                    chkRadial_Load_Eng_Design.Checked = false;

                    txtRadial_Load_Range_Min_Eng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load_Range[0], "#0.00");
                    txtRadial_Load_Design_Eng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load(), "#0.00");
                    txtRadial_Load_Range_Max_Eng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load_Range[1], "#0.00");
                }
                else if (mOpCond.Radial_Load_Range[0] == mOpCond.Radial_Load() &&
                         mOpCond.Radial_Load_Range[1] == mOpCond.Radial_Load())
                {
                    chkRadial_Load_Eng_Design.Checked = true;
                    txtRadial_Load_Design_Eng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load(), "#0.00");
                }

                //SetLoadDesign();
                SetRadial_LoadDesign();

                //  Load Angle
                //  ----------
                txtRadial_LoadAng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_LoadAng, "#0.#");

                //  Load: Thrust
                //  ------------

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {              
                    mChkThrust_Load_Eng[0].Checked = true;

                    //....English
                    if (mOpCond.Thrust_Load_Range_Front[0] != mOpCond.Thrust_Load_Front() &&
                        mOpCond.Thrust_Load_Range_Front[1] != mOpCond.Thrust_Load_Front())
                    {
                        mChkThrust_Load_Design[0].Checked = false;

                        mTxtThrust_Load_Range_Min_Eng[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Front[0], "#0.00");
                        mTxtThrust_Load_Design_Eng[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Front(), "#0.00");
                        mTxtThrust_Load_Range_Max_Eng[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Front[1], "#0.00");
                    }
                    else if (mOpCond.Thrust_Load_Range_Front[0] == mOpCond.Thrust_Load_Front() &&
                             mOpCond.Thrust_Load_Range_Front[1] == mOpCond.Thrust_Load_Front())
                    {
                        mChkThrust_Load_Design[0].Checked = true;
                        mTxtThrust_Load_Design_Eng[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Front(), "#0.00");
                    }

                    SetThrust_LoadDesign(0);
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    mChkThrust_Load_Eng[1].Checked = true;

                    //....English
                    if (mOpCond.Thrust_Load_Range_Back[0] != mOpCond.Thrust_Load_Back() &&
                        mOpCond.Thrust_Load_Range_Back[1] != mOpCond.Thrust_Load_Back())
                    {
                        mChkThrust_Load_Design[1].Checked = false;

                        mTxtThrust_Load_Range_Min_Eng[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Back[0], "#0.00");
                        mTxtThrust_Load_Design_Eng[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Back(), "#0.00");
                        mTxtThrust_Load_Range_Max_Eng[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Back[1], "#0.00");
                    }
                    else if (mOpCond.Thrust_Load_Range_Back[0] == mOpCond.Thrust_Load_Back() &&
                             mOpCond.Thrust_Load_Range_Back[1] == mOpCond.Thrust_Load_Back())
                    {
                        mChkThrust_Load_Design[1].Checked = true;
                        mTxtThrust_Load_Design_Eng[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Back(), "#0.00");
                    }

                    SetThrust_LoadDesign(1);
                }
              
                //chkThrust_Load_Eng_Front.Checked = true;

                ////....English
                //if (mOpCond.Thrust_Load_Range_Front[0] != mOpCond.Thrust_Load_Front() &&
                //    mOpCond.Thrust_Load_Range_Front[1] != mOpCond.Thrust_Load_Front())
                //{
                //    chkThrust_Load_Eng_Design_Front.Checked = false;

                //    txtThrust_Load_Range_Min_Eng_Front.Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Front[0], "#0.00");
                //    txtThrust_Load_Design_Eng_Front.Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Front(), "#0.00");
                //    txtThrust_Load_Range_Max_Eng_Front.Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range_Front[1], "#0.00");
                //}
                //else if (mOpCond.Thrust_Load_Range_Front[0] == mOpCond.Thrust_Load_Front() &&
                //         mOpCond.Thrust_Load_Range_Front[1] == mOpCond.Thrust_Load_Front())
                //{
                //    chkThrust_Load_Eng_Design_Front.Checked = true;
                //    txtThrust_Load_Design_Met_Front.Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Front(), "#0.00");
                //}

                //SetThrust_LoadDesign_Front(0);

             

                //  Press
                //  -----
                //txtPress_Eng.Text = modMain.ConvSingleToStr(modMain.gOpCond.OilSupply.Press, "#0");     //SB 17JUL09 format change
                txtOilSupply_Press_Met.Text = modMain.ConvDoubleToStr((Double)modMain.gUnit.CFac_Press_EngToMet(mOpCond.OilSupply.Press), "#0.000");
                txtOilSupply_Press_Eng.ForeColor = Color.Black;
                txtOilSupply_Press_Met.ForeColor = Color.Blue;

                //  Temp
                //  ----
                txtOilSupply_Temp_Eng.Text = modMain.ConvDoubleToStr(mOpCond.OilSupply.Temp, "#0");
                cmbLube_Type.Text = mOpCond.OilSupply.Lube.Type;

                if (mOpCond.OilSupply.Type != null && mOpCond.OilSupply.Type != "")         //BG 04OCT12
                    cmbOilSupply_Type.Text = mOpCond.OilSupply.Type;
                else
                    cmbOilSupply_Type.SelectedIndex = 0;

                modMain.gOpCond.OilSupply_Press = (Double)modMain.gUnit.CFac_Press_MetToEng((Double)modMain.ConvTextToDouble(txtOilSupply_Press_Met.Text));
                
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE:"
            //**********************************

            #region "CHECK BOX RELATED ROUTINE"
            //--------------------------------      //SB 20MAY09 CheckBox Event Added.
                private void chkSpeed_Design_CheckedChanged(object sender, EventArgs e)
                //=====================================================================
                {
                    SetSpeedDesign();   //SB 20MAY09
                }

                private void SetSpeedDesign()   //SB 20MAY09
                //===========================
                {
                    txtSpeed_Range_Max.ReadOnly = chkSpeed_Design.Checked;
                    SetBackColor(ref txtSpeed_Range_Max, chkSpeed_Design.Checked);
                    txtSpeed_Design.ReadOnly = !chkSpeed_Design.Checked;
                    SetBackColor(ref txtSpeed_Design, !chkSpeed_Design.Checked);
                    txtSpeed_Range_Min.ReadOnly = chkSpeed_Design.Checked;
                    SetBackColor(ref txtSpeed_Range_Min, chkSpeed_Design.Checked);
                }

                private void chkRadial_Load_Eng_Design_CheckedChanged(object sender, EventArgs e)
                //================================================================================
                {
                    //SetLoadDesign();
                    SetRadial_LoadDesign();
                }

                //private void SetLoadDesign()    //SB 20MAY09
                private void SetRadial_LoadDesign()      //BG 03JAN13
                //=================================
                {
                    if (chkRadial_Load_Eng_Design.Checked == true && chkRadial_Load_Eng.Checked == true)
                    {
                        txtRadial_Load_Range_Min_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Eng,true);
                        txtRadial_Load_Design_Eng.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Design_Eng, false);
                        txtRadial_Load_Range_Max_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Eng, true);

                        txtRadial_Load_Range_Min_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Met, true);
                        txtRadial_Load_Design_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Met, true);
                        txtRadial_Load_Range_Max_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Met, true);
                    }
                    else if (chkRadial_Load_Eng_Design.Checked == true && chkRadial_Load_Eng.Checked == false)
                    {
                        txtRadial_Load_Range_Min_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Eng, true);
                        txtRadial_Load_Design_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Eng, true);
                        txtRadial_Load_Range_Max_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Eng, true);

                        txtRadial_Load_Range_Min_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Met, true);
                        txtRadial_Load_Design_Met.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Design_Met, false);
                        txtRadial_Load_Range_Max_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Met, true);
                    }
                    else if (chkRadial_Load_Eng_Design.Checked == false && chkRadial_Load_Eng.Checked == true)
                    {
                        txtRadial_Load_Range_Min_Eng.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Range_Min_Eng, false);
                        txtRadial_Load_Design_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Eng, true);
                        txtRadial_Load_Range_Max_Eng.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Range_Max_Eng, false);

                        txtRadial_Load_Range_Min_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Met, true);
                        txtRadial_Load_Design_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Met, true);
                        txtRadial_Load_Range_Max_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Met, true);

                    }
                    else if (chkRadial_Load_Eng_Design.Checked == false && chkRadial_Load_Eng.Checked == false)
                    {
                        txtRadial_Load_Range_Min_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Min_Eng, true);
                        txtRadial_Load_Design_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Eng, true);
                        txtRadial_Load_Range_Max_Eng.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Range_Max_Eng, true);

                        txtRadial_Load_Range_Min_Met.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Range_Min_Met, false);
                        txtRadial_Load_Design_Met.ReadOnly = true;
                        SetBackColor(ref txtRadial_Load_Design_Met, true);
                        txtRadial_Load_Range_Max_Met.ReadOnly = false;
                        SetBackColor(ref txtRadial_Load_Range_Max_Met, false);

                    }
                }

                private void chkThrust_Load_Eng_Front_CheckedChanged(object sender, EventArgs e)
                //==============================================================================
                {
                    SetThrust_LoadDesign(0);
                }

                private void chkThrust_Load_Eng_Back_CheckedChanged(object sender, EventArgs e)
                //==============================================================================
                {
                    SetThrust_LoadDesign(1);
                }

               

                private void SetThrust_LoadDesign(int Indx_In)      //BG 03JAN13
                //============================================
                {

                    if (mChkThrust_Load_Design[Indx_In].Checked == true && mChkThrust_Load_Eng[Indx_In].Checked == true)
                    {
                        mTxtThrust_Load_Range_Min_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Eng[Indx_In], true);
                        mTxtThrust_Load_Design_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref  mTxtThrust_Load_Design_Eng[Indx_In], false);
                        mTxtThrust_Load_Range_Max_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Eng[Indx_In], true);

                        mTxtThrust_Load_Range_Min_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Met[Indx_In], true);
                        mTxtThrust_Load_Design_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Met[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Met[Indx_In], true);
                    }
                    else if (mChkThrust_Load_Design[Indx_In].Checked == true && mChkThrust_Load_Eng[Indx_In].Checked == false)
                    {
                        mTxtThrust_Load_Range_Min_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Eng[Indx_In], true);
                        mTxtThrust_Load_Design_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Eng[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Eng[Indx_In], true);

                        mTxtThrust_Load_Range_Min_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Met[Indx_In], true);
                        mTxtThrust_Load_Design_Met[Indx_In].ReadOnly = false;
                        SetBackColor(ref mTxtThrust_Load_Design_Met[Indx_In], false);
                        mTxtThrust_Load_Range_Max_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Met[Indx_In], true);
                    }
                    else if (mChkThrust_Load_Design[Indx_In].Checked == false && mChkThrust_Load_Eng[Indx_In].Checked == true)
                    {
                        mTxtThrust_Load_Range_Min_Eng[Indx_In].ReadOnly = false;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Eng[Indx_In], false);
                        mTxtThrust_Load_Design_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Eng[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Eng[Indx_In].ReadOnly = false;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Eng[Indx_In], false);

                        mTxtThrust_Load_Range_Min_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Met[Indx_In], true);
                        mTxtThrust_Load_Design_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Met[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Met[Indx_In], true);
                    }
                    else if (mChkThrust_Load_Design[Indx_In].Checked == false && mChkThrust_Load_Eng[Indx_In].Checked == false)
                    {
                        mTxtThrust_Load_Range_Min_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Eng[Indx_In], true);
                        mTxtThrust_Load_Design_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Eng[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Eng[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Eng[Indx_In], true);

                        mTxtThrust_Load_Range_Min_Met[Indx_In].ReadOnly = false;
                        SetBackColor(ref mTxtThrust_Load_Range_Min_Met[Indx_In], false);
                        mTxtThrust_Load_Design_Met[Indx_In].ReadOnly = true;
                        SetBackColor(ref mTxtThrust_Load_Design_Met[Indx_In], true);
                        mTxtThrust_Load_Range_Max_Met[Indx_In].ReadOnly = false;
                        SetBackColor(ref mTxtThrust_Load_Range_Max_Met[Indx_In], false);
                    }
                    //if (chkThrust_Load_Eng_Design_Front.Checked == true && chkThrust_Load_Eng_Front.Checked == true)
                    //{
                    //    txtThrust_Load_Range_Min_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Eng_Front, true);
                    //    txtThrust_Load_Design_Eng_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Design_Eng_Front, false);
                    //    txtThrust_Load_Range_Max_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Eng_Front, true);

                    //    txtThrust_Load_Range_Min_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Met_Front, true);
                    //    txtThrust_Load_Design_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Met_Front, true);
                    //    txtThrust_Load_Range_Max_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Met_Front, true);
                    //}
                    //else if (chkThrust_Load_Eng_Design_Front.Checked == true && chkThrust_Load_Eng_Front.Checked == false)
                    //{
                    //    txtThrust_Load_Range_Min_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Eng_Front, true);
                    //    txtThrust_Load_Design_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Eng_Front, true);
                    //    txtThrust_Load_Range_Max_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Eng_Front, true);

                    //    txtThrust_Load_Range_Min_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Met_Front, true);
                    //    txtThrust_Load_Design_Met_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Design_Met_Front, false);
                    //    txtThrust_Load_Range_Max_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Met_Front, true);
                    //}
                    //else if (chkThrust_Load_Eng_Design_Front.Checked == false && chkThrust_Load_Eng_Front.Checked == true)
                    //{
                    //    txtThrust_Load_Range_Min_Eng_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Eng_Front, false);
                    //    txtThrust_Load_Design_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Eng_Front, true);
                    //    txtThrust_Load_Range_Max_Eng_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Eng_Front, false);

                    //    txtThrust_Load_Range_Min_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Met_Front, true);
                    //    txtThrust_Load_Design_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Met_Front, true);
                    //    txtThrust_Load_Range_Max_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Met_Front, true);
                    //}
                    //else if (chkThrust_Load_Eng_Design_Front.Checked == false && chkThrust_Load_Eng_Front.Checked == false)
                    //{
                    //    txtThrust_Load_Range_Min_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Eng_Front, true);
                    //    txtThrust_Load_Design_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Eng_Front, true);
                    //    txtThrust_Load_Range_Max_Eng_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Eng_Front, true);

                    //    txtThrust_Load_Range_Min_Met_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Range_Min_Met_Front, false);
                    //    txtThrust_Load_Design_Met_Front.ReadOnly = true;
                    //    SetBackColor(ref txtThrust_Load_Design_Met_Front, true);
                    //    txtThrust_Load_Range_Max_Met_Front.ReadOnly = false;
                    //    SetBackColor(ref txtThrust_Load_Range_Max_Met_Front, false);
                    //}
                }

                private void SetBackColor(ref TextBox TxtBox_In,bool bln_In)
                //==========================================================
                {
                    if (bln_In)
                    {
                        Color pColor = Color.FromArgb(255, 235, 233, 237);
                        TxtBox_In.BackColor = Color.White;
                        TxtBox_In.ForeColor = Color.Blue;
                        TxtBox_In.BackColor = pColor;
                    }
                    else 
                    {
                        TxtBox_In.BackColor = Color.White;
                        TxtBox_In.ForeColor = Color.Black ;
                    }
                }

            #endregion

            #region"TEXT BOX RELATED ROUTINE:"
            //--------------------------------

                private Double Calc_Design(string Txt_Min_In, string Txt_Max_In)
                //==============================================================
                {
                    Double pMinVal = 0.0, pMaxVal = 0.0, pNomVal = 0.0F;

                    if (Txt_Min_In != "" && Txt_Max_In == "")
                    {
                        pNomVal = modMain.ConvTextToDouble(Txt_Min_In);
                    }
                    else if (Txt_Min_In == "" && Txt_Max_In != "")
                    {
                        pNomVal = modMain.ConvTextToDouble(Txt_Max_In);
                    }
                    else if (Txt_Min_In != "" && Txt_Max_In != "")
                    {
                        pMinVal = modMain.ConvTextToDouble(Txt_Min_In);
                        pMaxVal = modMain.ConvTextToDouble(Txt_Max_In);
                        pNomVal = 0.5 * (pMinVal + pMaxVal);
                    }
                    return pNomVal;
                }
                    

                # region "SPEED RELATED ROUTINE"

                    private void txtSpeed_Min_TextChanged(object sender, EventArgs e)
                    //================================================================  //SB 11MAY09
                    {
                        if (chkSpeed_Design.Checked) return;

                        //Double pDesign = 0.0f;
                        //pDesign = (modMain.ConvTextToDouble(txtSpeed_Range_Min.Text) +
                        //                 modMain.ConvTextToDouble(txtSpeed_Range_Max.Text)) * 0.5F;
                        //txtSpeed_Design.Text = pDesign.ToString();

                        //if (txtSpeed_Range_Min.Text == "" && txtSpeed_Design.Text != "")
                        //    mOpCond.Speed_Range[0] = modMain.ConvTextToDouble(txtSpeed_Design.Text);
                        //else if (txtSpeed_Range_Min.Text != "")
                        //    mOpCond.Speed_Range[0] = modMain.ConvTextToDouble(txtSpeed_Range_Min.Text);

                        //BG 22NOV11
                        int pDesign = 0;
                        //pDesign = (int)((modMain.ConvTextToInt(txtSpeed_Range_Min.Text) +
                        //                 modMain.ConvTextToInt(txtSpeed_Range_Max.Text)) * 0.5);
                        pDesign = (int)(Calc_Design(txtSpeed_Range_Min.Text, txtSpeed_Range_Max.Text));
                        txtSpeed_Design.Text = pDesign.ToString();

                        //if (txtSpeed_Range_Min.Text == "" && txtSpeed_Design.Text != "")
                        //    mOpCond.Speed_Range[0] = modMain.ConvTextToInt(txtSpeed_Design.Text);
                        //else if (txtSpeed_Range_Min.Text != "")
                        //    mOpCond.Speed_Range[0] = modMain.ConvTextToInt(txtSpeed_Range_Min.Text);

                    }

                    private void txtSpeed_Max_TextChanged(object sender, EventArgs e)
                    //===============================================================   //SB 11MAY09
                    {
                        if (chkSpeed_Design.Checked) return;

                        //Double pDesign = 0.0f;
                        //pDesign = (modMain.ConvTextToDouble(txtSpeed_Range_Min.Text) +
                        //                 modMain.ConvTextToDouble(txtSpeed_Range_Max.Text)) * 0.5F;
                        //txtSpeed_Design.Text = pDesign.ToString();

                        //if (txtSpeed_Range_Max.Text == "" && txtSpeed_Design.Text != "")
                        //    mOpCond.Speed_Range[1] = modMain.ConvTextToDouble(txtSpeed_Design.Text);
                        //else if (txtSpeed_Range_Max.Text != "")
                        //    mOpCond.Speed_Range[1] = modMain.ConvTextToDouble(txtSpeed_Range_Max.Text);

                        //BG 22NOV11
                        int pDesign = 0;
                        //pDesign = (int)((modMain.ConvTextToDouble(txtSpeed_Range_Min.Text) +
                        //                 modMain.ConvTextToDouble(txtSpeed_Range_Max.Text)) * 0.5);
                        pDesign = (int)(Calc_Design(txtSpeed_Range_Min.Text, txtSpeed_Range_Max.Text));
                        txtSpeed_Design.Text = pDesign.ToString();

                        //if (txtSpeed_Range_Max.Text == "" && txtSpeed_Design.Text != "")
                        //    mOpCond.Speed_Range[1] = modMain.ConvTextToInt(txtSpeed_Design.Text);
                        //else if (txtSpeed_Range_Max.Text != "")
                        //    mOpCond.Speed_Range[1] = modMain.ConvTextToInt(txtSpeed_Range_Max.Text);

                    }


                    private void txtSpeed_Design_TextChanged(object sender, EventArgs e)
                    //==================================================================    //SB 11MAY09
                    {
                        if (!chkSpeed_Design.Checked) return;    

                        //Double pDesign = 0.0f;
                        //pDesign = modMain.ConvTextToDouble(txtSpeed_Design.Text);

                        //BG 22NOV11
                        int pDesign = 0;
                        pDesign = modMain.ConvTextToInt(txtSpeed_Design.Text);

                        txtSpeed_Range_Min.Text = "";
                        txtSpeed_Range_Max.Text = "";

                        mOpCond.Speed_Range[0] = pDesign;
                        mOpCond.Speed_Range[1] = pDesign;

                    }

                #endregion

                #region "LOAD RELATED ROUTINE"  

                    private void txtLoadAngle_TextChanged(object sender, EventArgs e)
                    //================================================================
                    {
                        mOpCond.Radial_LoadAng = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);
                    }


                    private void OptionButton_CheckedChanged(object sender, EventArgs e)
                    //===================================================================   //BG 23APR09
                    {
                        RadioButton pOptButton = (RadioButton)sender;

                        switch (pOptButton.Name)
                        {
                            case "optDirection_Uni":
                                //=================
                                if (pOptButton.Checked)
                                    modMain.gOpCond.Rot_Directionality =
                                            (clsOpCond.eRotDirectionality)Enum.Parse
                                                (typeof(clsOpCond.eRotDirectionality), "Uni");
                                break;

                            case "optDirection_Bi":
                                //=================
                                if (pOptButton.Checked)
                                    modMain.gOpCond.Rot_Directionality =
                                            (clsOpCond.eRotDirectionality)Enum.Parse
                                            (typeof(clsOpCond.eRotDirectionality), "Bi");
                                break;

                        }
                    }
                      
                    #region "Radial"

                        #region "ENGLISH"

                            private void txtLoad_Eng_Min_TextChanged(object sender, EventArgs e)
                            //===================================================================
                            {
                                if (chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Range_Min_Eng.Text);
                                    txtRadial_Load_Range_Min_Met.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_EngToMet(pLoad),"#0.00");    

                                }

                                if (!chkRadial_Load_Eng_Design.Checked)
                                {
                                    Double pLoad_Design = 0.0F;

                                    pLoad_Design = Calc_Design(txtRadial_Load_Range_Min_Eng.Text, txtRadial_Load_Range_Max_Eng.Text);
                                    txtRadial_Load_Design_Eng.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                }

                                mOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Range_Min_Eng.Text);

                            }

                            private void txtLoad_Eng_Max_TextChanged(object sender, EventArgs e)
                            //====================================================================
                            {
                                if (chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Range_Max_Eng.Text);
                                    txtRadial_Load_Range_Max_Met.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");   
                                }

                                if (!chkRadial_Load_Eng_Design.Checked)
                                {
                                    Double pLoad_Design = 0.0F;

                                    pLoad_Design = Calc_Design(txtRadial_Load_Range_Min_Eng.Text, txtRadial_Load_Range_Max_Eng.Text);
                                    txtRadial_Load_Design_Eng.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");
                                }

                                mOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Range_Max_Eng.Text);

                            }


                             private void txtLoad_Eng_Design_TextChanged(object sender, EventArgs e)
                            //=====================================================================
                            {
                                if (chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                                    txtRadial_Load_Design_Met.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");   //SB 21APR09
                                }

                                if (chkRadial_Load_Eng_Design.Checked)
                                {
                                    txtRadial_Load_Range_Min_Eng.Text = "";
                                    txtRadial_Load_Range_Max_Eng.Text = "";
                                    mOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                                    mOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                                }                            
                            }

                        #endregion

                        #region "METRIC"

                            private void txtLoad_Met_Min_TextChanged(object sender, EventArgs e)
                            //==================================================================
                            {

                                if (!chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Range_Min_Met.Text);
                                    txtRadial_Load_Range_Min_Eng.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");   

                                }

                                if (!chkRadial_Load_Eng_Design.Checked)
                                {
                                    Double pLoad_Design = 0.0F;

                                    //Double pLoad_Min = modMain.ConvTextToDouble(txtLoad_Range_Min_Met.Text);
                                    //Double pLoad_Max = modMain.ConvTextToDouble(txtLoad_Range_Max_Met.Text);

                                    //pLoad_Design = (pLoad_Min + pLoad_Max) / 2;
                                    pLoad_Design = Calc_Design(txtRadial_Load_Range_Min_Met.Text, txtRadial_Load_Range_Max_Met.Text);
                                    txtRadial_Load_Design_Met.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                }

                                mOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Range_Min_Eng.Text);
                            }

                            private void txtLoad_Met_Max_TextChanged(object sender, EventArgs e)
                            //==================================================================
                            {

                                if (!chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Range_Max_Met.Text);
                                    txtRadial_Load_Range_Max_Eng.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");   //SB 21APR09

                                }

                                if (!chkRadial_Load_Eng_Design.Checked)
                                {
                                    Double pLoad_Design = 0.0F;

                                    //Double pLoad_Min = modMain.ConvTextToDouble(txtLoad_Range_Min_Met.Text);
                                    //Double pLoad_Max = modMain.ConvTextToDouble(txtLoad_Range_Max_Met.Text);
                                    //pLoad_Design = (pLoad_Min + pLoad_Max) / 2;

                                    pLoad_Design = Calc_Design(txtRadial_Load_Range_Min_Met.Text, txtRadial_Load_Range_Max_Met.Text);
                                    txtRadial_Load_Design_Met.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                }
                                mOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Range_Max_Eng.Text);
                            }

                            private void txtLoad_Met_Design_TextChanged(object sender, EventArgs e)
                            //===============================================================
                            {
                                if (!chkRadial_Load_Eng.Checked)
                                {
                                    Double pLoad = modMain.ConvTextToDouble(txtRadial_Load_Design_Met.Text);
                                    txtRadial_Load_Design_Eng.Text = modMain.ConvDoubleToStr((Double)
                                                           modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");   //SB 21APR09

                                }

                                if (chkRadial_Load_Eng_Design.Checked)
                                {
                                    txtRadial_Load_Range_Min_Met.Text = "";
                                    txtRadial_Load_Range_Max_Met.Text = "";
                                    mOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                                    mOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                                }
                            
                            }
                        #endregion

                    #endregion

                    #region "Thrust"

                        #region "Front"

                            #region "English"

                                private void txtThrust_Load_Range_Min_Eng_Front_TextChanged(object sender, EventArgs e)
                                //======================================================================================
                                {
                                    if (chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Front.Text);
                                        txtThrust_Load_Range_Min_Met_Front.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");

                                    }

                                    if (!chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Eng_Front.Text, txtThrust_Load_Range_Max_Eng_Front.Text);
                                        txtThrust_Load_Design_Eng_Front.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }

                                    mOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Front.Text);

                                }

                                private void txtThrust_Load_Range_Max_Eng_Front_TextChanged(object sender, EventArgs e)
                                //======================================================================================
                                {
                                    if (chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Front.Text);
                                        txtThrust_Load_Range_Max_Met_Front.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");
                                    }

                                    if (!chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Eng_Front.Text, txtThrust_Load_Range_Max_Eng_Front.Text);
                                        txtThrust_Load_Design_Eng_Front.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");
                                    }

                                    mOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Front.Text);
                                }

                                private void txtThrust_Load_Design_Eng_Front_TextChanged(object sender, EventArgs e)
                                //====================================================================================
                                {
                                    if (chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);
                                        txtThrust_Load_Design_Met_Front.Text = modMain.ConvDoubleToStr((Double)
                                                                               modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");  
                                    }

                                    if (chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        txtThrust_Load_Range_Min_Eng_Front.Text = "";
                                        txtThrust_Load_Range_Max_Eng_Front.Text = "";
                                        mOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);
                                        mOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);
                                    }              
                                }

                        #endregion

                            #region "Metric"

                                private void txtThrust_Load_Range_Min_Met_Front_TextChanged(object sender, EventArgs e)
                                //=====================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Met_Front.Text);
                                        txtThrust_Load_Range_Min_Eng_Front.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");

                                    }

                                    if (!chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Met_Front.Text, txtThrust_Load_Range_Max_Met_Front.Text);
                                        txtThrust_Load_Design_Met_Front.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }

                                    mOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Front.Text);

                                }

                                private void txtThrust_Load_Range_Max_Met_Front_TextChanged(object sender, EventArgs e)
                                //======================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Met_Front.Text);
                                        txtThrust_Load_Range_Max_Eng_Front.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");  

                                    }

                                    if (!chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Met_Front.Text, txtThrust_Load_Range_Max_Met_Front.Text);
                                        txtThrust_Load_Design_Met_Front.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }
                                    mOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Front.Text);

                                }

                                private void txtThrust_Load_Design_Met_Front_TextChanged(object sender, EventArgs e)
                                //==================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Front.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Design_Met_Front.Text);
                                        txtThrust_Load_Design_Eng_Front.Text = modMain.ConvDoubleToStr(modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");   
                                    }

                                    if (chkThrust_Load_Eng_Design_Front.Checked)
                                    {
                                        txtThrust_Load_Range_Min_Met_Front.Text = "";
                                        txtThrust_Load_Range_Max_Met_Front.Text = "";
                                        mOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Met_Front.Text);
                                        mOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Met_Front.Text);
                                    }

                                }


                            #endregion

                        #endregion

                        #region "Back"

                            #region "English"

                                private void txtThrust_Load_Range_Min_Eng_Back_TextChanged(object sender, EventArgs e)
                                //====================================================================================
                                {
                                    if (chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Back.Text);
                                        txtThrust_Load_Range_Min_Met_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");

                                    }

                                    if (!chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Eng_Back.Text, txtThrust_Load_Range_Max_Eng_Back.Text);
                                        txtThrust_Load_Design_Eng_Back.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }

                                    mOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Back.Text);

                                }

                                private void txtThrust_Load_Range_Max_Eng_Back_TextChanged(object sender, EventArgs e)
                                //====================================================================================
                                {
                                    if (chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Back.Text);
                                        txtThrust_Load_Range_Max_Met_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                                 modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");
                                    }

                                    if (!chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Eng_Back.Text, txtThrust_Load_Range_Max_Eng_Back.Text);
                                        txtThrust_Load_Design_Eng_Back.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");
                                    }

                                    mOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Back.Text);

                                }

                                private void txtThrust_Load_Design_Eng_Back_TextChanged(object sender, EventArgs e)
                                //==================================================================================
                                {
                                    if (chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);
                                        txtThrust_Load_Design_Met_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                              modMain.gUnit.CFac_Load_EngToMet(pLoad), "#0.00");
                                    }

                                    if (chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        txtThrust_Load_Range_Min_Eng_Back.Text = "";
                                        txtThrust_Load_Range_Max_Eng_Back.Text = "";
                                        mOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);
                                        mOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);
                                    }              


                                }

                            #endregion

                            #region "Metric"

                                private void txtThrust_Load_Range_Min_Met_Back_TextChanged(object sender, EventArgs e)
                                //====================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Met_Back.Text);
                                        txtThrust_Load_Range_Min_Eng_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                                  modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");

                                    }

                                    if (!chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Met_Back.Text, txtThrust_Load_Range_Max_Met_Back.Text);
                                        txtThrust_Load_Design_Met_Back.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }

                                    mOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Eng_Back.Text);

                                }

                                private void txtThrust_Load_Range_Max_Met_Back_TextChanged(object sender, EventArgs e)
                                //====================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Met_Back.Text);
                                        txtThrust_Load_Range_Max_Eng_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                                 modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");

                                    }

                                    if (!chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        Double pLoad_Design = 0.0F;

                                        pLoad_Design = Calc_Design(txtThrust_Load_Range_Min_Met_Back.Text, txtThrust_Load_Range_Max_Met_Back.Text);
                                        txtThrust_Load_Design_Met_Back.Text = modMain.ConvDoubleToStr(pLoad_Design, "#0.00");

                                    }
                                    mOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Back.Text);

                                }

                                private void txtThrust_Load_Design_Met_Back_TextChanged(object sender, EventArgs e)
                                //==================================================================================
                                {
                                    if (!chkThrust_Load_Eng_Back.Checked)
                                    {
                                        Double pLoad = modMain.ConvTextToDouble(txtThrust_Load_Design_Met_Back.Text);
                                        txtThrust_Load_Design_Eng_Back.Text = modMain.ConvDoubleToStr((Double)
                                                                              modMain.gUnit.CFac_Load_MetToEng(pLoad), "#0.00");
                                    }

                                    if (chkThrust_Load_Eng_Design_Back.Checked)
                                    {
                                        txtThrust_Load_Range_Min_Met_Back.Text = "";
                                        txtThrust_Load_Range_Max_Met_Back.Text = "";
                                        mOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Range_Min_Met_Back.Text);
                                        mOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Met_Back.Text);
                                    }

                                }


                            #endregion

                        #endregion

                    #endregion
                  

                #endregion

                #region "PRESSURE RELATED ROUTINE"

                    private void txtPress_Eng_TextChanged(object sender, EventArgs e)
                    //===============================================================
                    {           
                        if (mblntxtPressMet_Entered) return;
                        if (mblntxtPressMet_Entered == false)
                        {
                            mblntxtPressEng_Entered = true;
                            Double pPress = modMain.ConvTextToDouble(txtOilSupply_Press_Eng.Text);    //SB 20APR09
                            txtOilSupply_Press_Met.Text = modMain.ConvDoubleToStr((Double)modMain.gUnit.CFac_Press_EngToMet(pPress), "#0.000"); //SB 09MAR10
                            txtOilSupply_Press_Eng.ForeColor = Color.Black;   //SB 13OCT09
                            txtOilSupply_Press_Met.ForeColor = Color.Blue;    //SB 13OCT09
                        }

                        else
                        {
                            mblntxtPressEng_Entered = false;
                            txtOilSupply_Press_Met.Text = "";
                            txtOilSupply_Press_Met.ForeColor = Color.Black;   //SB 13OCT09
                            txtOilSupply_Press_Eng.ForeColor = Color.Blue;    //SB 13OCT09
                        }

                        mOpCond.OilSupply_Press = (Double)modMain.gUnit.CFac_Press_MetToEng((Double)modMain.ConvTextToDouble(txtOilSupply_Press_Met.Text)); //SB 30OCT09
                    }

                    private void txtPress_Met_TextChanged(object sender, EventArgs e)
                    //===============================================================
                    {
                        if (mblntxtPressEng_Entered) return;
                        if (mblntxtPressEng_Entered == false)
                        {
                            mblntxtPressMet_Entered = true;
                            Double pPress = modMain.ConvTextToDouble(txtOilSupply_Press_Met.Text);    
                            txtOilSupply_Press_Eng.Text = modMain.ConvDoubleToStr(modMain.NInt(
                                                modMain.gUnit.CFac_Press_MetToEng(pPress)), "#0"); //SB 09MAR10.
                            txtOilSupply_Press_Met.ForeColor = Color.Black;   //SB 13OCT09
                            txtOilSupply_Press_Eng.ForeColor = Color.Blue;    //SB 13OCT09
                        }

                        else
                        {
                            mblntxtPressMet_Entered = false;
                            txtOilSupply_Press_Eng.Text = "";
                            txtOilSupply_Press_Eng.ForeColor = Color.Black;   //SB 13OCT09
                            txtOilSupply_Press_Met.ForeColor = Color.Blue;    //SB 13OCT09
                        }

                        mOpCond.OilSupply_Press = (Double)modMain.gUnit.CFac_Press_MetToEng((Double)modMain.ConvTextToDouble(txtOilSupply_Press_Met.Text));     //SB 30OCT09
                    }

                    private void txtPress_Eng_MouseDown(object sender, MouseEventArgs e)
                    //==================================================================
                    {
                        mblntxtPressMet_Entered = false;
                        txtOilSupply_Press_Eng.ForeColor = Color.Black;
                    }

                    private void txtPress_Eng_KeyDown(object sender, KeyEventArgs e)    
                    //==============================================================
                    {
                        mblntxtPressMet_Entered = false;
                        txtOilSupply_Press_Eng.ForeColor = Color.Black;
                    }

                    private void txtPress_Met_MouseDown(object sender, MouseEventArgs e)
                    //==================================================================
                    {
                        mblntxtPressEng_Entered = false;
                        txtOilSupply_Press_Met.ForeColor = Color.Black;

                    }

                    private void txtPress_Met_KeyDown(object sender, KeyEventArgs e)    
                    //==============================================================
                    {
                        mblntxtPressEng_Entered = false;
                        txtOilSupply_Press_Met.ForeColor = Color.Black;
                    }

                #endregion

                #region "TEMPURATURE RELATED ROUTINE"

                    private void txtTemp_Eng_TextChanged(object sender, EventArgs e)
                    //===========================================================
                    {            
                        if (mblntxtTempMet_Entered) return;

                        if (mblntxtTempMet_Entered == false)
                        {
                            mblntxtTempEng_Entered = true;
                            Double pTemp = modMain.ConvTextToDouble(txtOilSupply_Temp_Eng.Text);      
                            txtOilSupply_Temp_Met.Text = modMain.NInt(modMain.gUnit.CFac_Temp_EngToMet(pTemp)).ToString();    
                            txtOilSupply_Temp_Eng.ForeColor = Color.Black;    //SB 13OCT09
                            txtOilSupply_Temp_Met.ForeColor = Color.Blue;     //SB 13OCT09
                        }

                        else
                        {
                            mblntxtTempEng_Entered = false;
                            txtOilSupply_Temp_Met.Text = "";
                            txtOilSupply_Temp_Eng.ForeColor = Color.Blue;     //SB 13OCT09
                            txtOilSupply_Temp_Met.ForeColor = Color.Black;    //SB 13OCT09
                        }

                        mOpCond.OilSupply_Temp = modMain.ConvTextToDouble(txtOilSupply_Temp_Eng.Text); 
                    }

                    private void txtTemp_Met_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                        if (mblntxtTempEng_Entered) return;

                        if (mblntxtTempEng_Entered == false)
                        {
                            mblntxtTempMet_Entered = true;
                            Double pTemp = modMain.ConvTextToDouble(txtOilSupply_Temp_Met.Text);
                            txtOilSupply_Temp_Eng.Text = modMain.NInt(modMain.gUnit.CFac_Temp_MetToEng(pTemp)).ToString();   
                            txtOilSupply_Temp_Met.ForeColor = Color.Black;    //SB 13OCT09
                            txtOilSupply_Temp_Eng.ForeColor = Color.Blue;     //SB 13OCT09
                        }

                        else
                        {
                            mblntxtTempMet_Entered = false;
                            txtOilSupply_Temp_Eng.Text = "";
                            txtOilSupply_Temp_Eng.ForeColor = Color.Black;    //SB 13OCT09
                            txtOilSupply_Temp_Met.ForeColor = Color.Blue;     //SB 13OCT09
                        }

                        mOpCond.OilSupply_Temp = modMain.ConvTextToDouble(txtOilSupply_Temp_Eng.Text);  //SB 28OCT09
                    }

                    private void txtTemp_Eng_MouseDown(object sender, MouseEventArgs e)
                    //=================================================================
                    {
                        mblntxtTempMet_Entered = false;
                        txtOilSupply_Temp_Eng.ForeColor = Color.Black;
                    }


                    private void txtTemp_Eng_KeyDown(object sender, KeyEventArgs e) 
                    //=============================================================
                    {
                        mblntxtTempMet_Entered = false;
                        txtOilSupply_Temp_Eng.ForeColor = Color.Black;
                    }

                    private void txtTemp_Met_MouseDown(object sender, MouseEventArgs e)
                    //==================================================================
                    {
                        mblntxtTempEng_Entered = false;
                        txtOilSupply_Temp_Met.ForeColor = Color.Black;
                    }

                    private void txtTemp_Met_KeyDown(object sender, KeyEventArgs e)
                    //=============================================================
                    {
                        mblntxtTempEng_Entered = false;
                        txtOilSupply_Temp_Met.ForeColor = Color.Black;
                    }

                #endregion

            #endregion

            #region "COMBO BOX RELATED ROUTINES"
                //----------------------------------
                private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================
                {
                    Boolean pVisible = false;
                    if (cmbOilSupply_Type.Text == "Pressurized")
                    {
                        pVisible = true;                
                    }
                    else
                    {
                        pVisible = false;               
                    }

                    lblPress.Visible = pVisible;
                    txtOilSupply_Press_Eng.Visible = pVisible;
                    lblPressUnit.Visible = pVisible;
                    txtOilSupply_Press_Met.Visible = pVisible;
                    lblPressure_MetUnit.Visible = pVisible;

                    modMain.gOpCond.OilSupply_Type = cmbOilSupply_Type.Text;  
                }

                private void cmbLub_SelectedIndexChanged(object sender, EventArgs e)
                //====================================================================
                {
                    if (cmbLube_Type.Text != "")
                        mOpCond.GetData_Lubricant(cmbLube_Type.Text, modMain.gDB);

                    txtLube_API_sg.Text = mOpCond.OilSupply.Lube.API_sg.ToString("#0.0###");     //BG 07NOV11
                    txtLube_Temp_a.Text = mOpCond.OilSupply.Lube.Temp_a.ToString();
                    txtOilSupply_Lube_Temp_b.Text = mOpCond.OilSupply.Lube.Temp_b.ToString();
                    txtLube_CSt_a.Text = mOpCond.OilSupply.Lube.CSt_a.ToString();
                    txtOilSupply_Lube_CSt_b.Text = mOpCond.OilSupply.Lube.CSt_b.ToString("#0.0###");        //BG 07NOV11

                    mOpCond.OilSupply_Lube_Type = cmbLube_Type.Text;    
                }

            #endregion

            #region "COMMAND BUTTON EVENT ROUTINES"
            //-------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;

                    switch (pcmdButton.Name)
                    {
                        case "cmdImport_XLKMC":
                            //-----------------
                            mblnCmd_XLKMC = true;
                            Import_Analytical_Data("tblMapping_XLRADIAL");
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot_Checked = true;        //BG 26MAR13
                            mblnCmd_XLKMC = false;
                            break;

                        case "cmdImport_XLThrust_Front":
                            //--------------------------
                            mblnCmd_XLThrust_Front = true;
                            Import_Analytical_Data("tblMapping_XLThrust");
                            mblnCmd_XLThrust_Front = false;
                            break;

                        case "cmdImport_XLThrust_Back":
                            //-------------------------
                            mblnCmd_XLThrust_Back = true;
                            Import_Analytical_Data("tblMapping_XLThrust");
                            mblnCmd_XLThrust_Back = false;
                            break;


                        case "cmdPrint":
                            //----------
                            PrintDocument pd = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);
                            modMain.CaptureScreen(this);
                            pd.Print();
                            break;


                        case "cmdOK":
                            //-------
                            CloseForm();
                            break;

                        case "cmdCancel":
                            //----------
                            this.Hide(); 
                            break;
                    }
                }

            
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
                        List<string> pCellRange_Start = new List<string>();
                        List<string> pCellRange_End = new List<string>();

                        while (pDR.Read())
                        {
                            pParameterName.Add(Convert.ToString(pDR["fldParameter"]));
                            pExcelSheetName.Add(Convert.ToString(pDR["fldWorkSheet"]));
                            pCellName.Add(Convert.ToString(pDR["fldCellNo"]));
                            pCellRange_Start.Add(Convert.ToString(pDR["fldCellNo_Start"]));
                            pCellRange_End.Add(Convert.ToString(pDR["fldCellNo_End"]));

                        }
                        pDR.Close();

                        if (mblnCmd_XLKMC)
                        {
                            modMain.gEXCEL_Analysis.Retrieve_Params_XLKMC(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                                                                          mOpCond, (clsBearing_Radial_FP)modMain.gProject.Product.Bearing);
                        }

                        int pEndConfig_Pos = 0;
                        if (mblnCmd_XLThrust_Front)
                        {
                            pEndConfig_Pos = 0;
                            modMain.gEXCEL_Analysis.Retrieve_Params_XLTHRUST(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                                                                             mOpCond, pEndConfig_Pos, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[pEndConfig_Pos]);
                        }
                        else if (mblnCmd_XLThrust_Back)
                        {
                            pEndConfig_Pos = 1;
                            modMain.gEXCEL_Analysis.Retrieve_Params_XLTHRUST(pExcelFileName, pParameterName, pExcelSheetName, pCellName, pCellRange_Start, pCellRange_End,
                                                                             mOpCond, pEndConfig_Pos, (clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[pEndConfig_Pos]);
                        }

                        DisplayData();
                    }
                }

        
                private void CloseForm()
                //======================
                {
                    SaveData();                   
                    this.Hide();

                    modMain.gfrmBearing.ShowDialog();   
                }

                private void SaveData()
                //=====================
                {
                    if (txtSpeed_Range_Min.Text == "" && txtSpeed_Design.Text != "")
                        modMain.gOpCond.Speed_Range[0] = modMain.ConvTextToInt(txtSpeed_Design.Text);
                    else
                        if (txtSpeed_Range_Min.Text != "")
                        modMain.gOpCond.Speed_Range[0] = modMain.ConvTextToInt(txtSpeed_Range_Min.Text);

                    if (txtSpeed_Range_Max.Text == "" && txtSpeed_Design.Text != "")
                        modMain.gOpCond.Speed_Range[1] = modMain.ConvTextToInt(txtSpeed_Design.Text);
                    else 
                    if (txtSpeed_Range_Max.Text != "")
                        modMain.gOpCond.Speed_Range[1] = modMain.ConvTextToInt(txtSpeed_Range_Max.Text);

                    //....Radial Load
                    if (txtRadial_Load_Range_Min_Eng.Text == "" && txtRadial_Load_Design_Eng.Text != "")
                        modMain.gOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                    else 
                    if (txtRadial_Load_Range_Min_Eng.Text != "")
                        modMain.gOpCond.Radial_Load_Range[0] = modMain.ConvTextToDouble(txtRadial_Load_Range_Min_Eng.Text);

                    if (txtRadial_Load_Range_Max_Eng.Text == "" && txtRadial_Load_Design_Eng.Text != "")
                        modMain.gOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Design_Eng.Text);
                    else 
                    if (txtRadial_Load_Range_Max_Eng.Text != "")
                        modMain.gOpCond.Radial_Load_Range[1] = modMain.ConvTextToDouble(txtRadial_Load_Range_Max_Eng.Text);

                    modMain.gOpCond.Radial_LoadAng = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);

                    //....Thrust Load

                        //........Front
                        if (txtThrust_Load_Range_Min_Eng_Front.Text == "" && txtThrust_Load_Design_Eng_Front.Text != "")
                            modMain.gOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);
                        else
                            if (txtThrust_Load_Range_Min_Eng_Front.Text != "")
                                modMain.gOpCond.Thrust_Load_Range_Front[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);

                        if (txtThrust_Load_Range_Max_Eng_Front.Text == "" && txtThrust_Load_Design_Eng_Front.Text != "")
                            modMain.gOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Front.Text);
                        else
                            if (txtThrust_Load_Range_Max_Eng_Front.Text != "")
                                modMain.gOpCond.Thrust_Load_Range_Front[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Front.Text);

                        //........Back
                        if (txtThrust_Load_Range_Min_Eng_Back.Text == "" && txtThrust_Load_Design_Eng_Back.Text != "")
                            modMain.gOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);
                        else
                            if (txtThrust_Load_Range_Min_Eng_Back.Text != "")
                                modMain.gOpCond.Thrust_Load_Range_Back[0] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);

                        if (txtThrust_Load_Range_Max_Eng_Back.Text == "" && txtThrust_Load_Design_Eng_Back.Text != "")
                            modMain.gOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Design_Eng_Back.Text);
                        else
                            if (txtThrust_Load_Range_Max_Eng_Back.Text != "")
                                modMain.gOpCond.Thrust_Load_Range_Back[1] = modMain.ConvTextToDouble(txtThrust_Load_Range_Max_Eng_Back.Text);

                    if (optRot_Directionality_Bi.Checked)
                    {
                        modMain.gOpCond.Rot_Directionality =
                             (clsOpCond.eRotDirectionality)Enum.Parse
                                (typeof(clsOpCond.eRotDirectionality), "Bi");
                    }
                    else
                    {
                        modMain.gOpCond.Rot_Directionality =
                             (clsOpCond.eRotDirectionality)Enum.Parse
                                (typeof(clsOpCond.eRotDirectionality), "Uni");
                    }
                   

                    modMain.gOpCond.OilSupply_Lube_Type = cmbLube_Type.Text;
                    modMain.gOpCond.OilSupply_Type = cmbOilSupply_Type.Text;
                    modMain.gOpCond.OilSupply_Press = (Double)modMain.gUnit.CFac_Press_MetToEng((Double)modMain.ConvTextToDouble(txtOilSupply_Press_Met.Text)); //SB 30OCT09
                    modMain.gOpCond.OilSupply_Temp = modMain.ConvTextToDouble(txtOilSupply_Temp_Eng.Text);

                }

      
            #endregion

                 
        #endregion

    }
}
