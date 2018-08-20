
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPerformance                         '
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
    public partial class frmPerformDataBearing : Form
    {
        #region "MEMBER VARIABLE DECLARATION"
        //***********************************

            private clsProduct mProduct;

            //   Boolean Variables to indicate Control Event Type:   
            //  -------------------------------------------------
            //
            //  ....TextBoxes:
            //  ........If variable TRUE  ==> Changed by the user. 
            //  ........            FALSE ==> Value set programmatically.  

            Boolean mblnPower_Eng_Radial_ManuallyChanged = false;        
            Boolean mblnPower_Met_Radial_ManuallyChanged = false;

            Boolean mblnFlowReqd_Eng_Radial_ManuallyChanged = false;
            Boolean mblnFlowReqd_Met_Radial_ManuallyChanged = false;

            Boolean mblnTempRise_Eng_Radial_ManuallyChanged = false;
            Boolean mblnTempRise_Met_Radial_ManuallyChanged = false;

            //....Thrust
            Boolean [] mblnPower_Eng_Thrust_ManuallyChanged = new Boolean [] {false,false};
            Boolean [] mblnPower_Met_Thrust_ManuallyChanged = new Boolean[] { false, false };

            Boolean [] mblnFlowReqd_Eng_Thrust_ManuallyChanged = new Boolean[] { false, false };
            Boolean [] mblnFlowReqd_Met_Thrust_ManuallyChanged = new Boolean[] { false, false };

            Boolean [] mblnTempRise_Eng_Thrust_ManuallyChanged = new Boolean[] { false, false };
            Boolean [] mblnTempRise_Met_Thrust_ManuallyChanged = new Boolean[] { false, false };    


        #endregion

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmPerformDataBearing()
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
                   
                //....Set Boolean Variable for Power.
                mblnPower_Met_Radial_ManuallyChanged = false;
                mblnPower_Eng_Radial_ManuallyChanged = true;

                //....Set Boolean Variable for FlowReqd.
                mblnFlowReqd_Met_Radial_ManuallyChanged = false;
                mblnFlowReqd_Eng_Radial_ManuallyChanged = true;

                //....Set Boolean Variable for Tempurature Rise.
                mblnTempRise_Met_Radial_ManuallyChanged = false;
                mblnTempRise_Eng_Radial_ManuallyChanged = true;


                //....Thrust
                for (int i = 0; i < 2; i++)
                {
                    mblnPower_Eng_Thrust_ManuallyChanged[i] = true;
                    mblnPower_Met_Thrust_ManuallyChanged[i] = false;

                    mblnFlowReqd_Eng_Thrust_ManuallyChanged [i] = true;
                    mblnFlowReqd_Met_Thrust_ManuallyChanged [i] = false;

                    mblnTempRise_Eng_Thrust_ManuallyChanged [i] = true;
                    mblnTempRise_Met_Thrust_ManuallyChanged [i] = false;
                }

                //....Data Display & Local Object.
                DisplayData();
               
                SetControl();          
           }

            private void Initialize_LocalObject()
            //===================================
            {
                mProduct = (clsProduct)((clsProduct)modMain.gProject.Product).Clone();
            }


            private void SetTabPages()
            //========================
            {
                if (tbBearing.TabPages.Count != 2)
                {
                    tbBearing.TabPages.Add(tabThrustBearing);
                }

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    tabTB.TabPages.Clear();
                    tabTB.TabPages.Add(tabTB_Front);
                    tabTB.TabPages.Add(tabTB_Back);
                }

                else if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Thrust_Bearing_TL &&
                    modMain.gProject.Product.EndConfig[1].Type != clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    tbBearing.TabPages.Remove(tabThrustBearing);
                    tbBearing.Refresh();
                }

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    tabTB.TabPages.Clear();
                    tabTB.TabPages.Add(tabTB_Front);
                    tabTB.Refresh();
                }

                else if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    tabTB.TabPages.Clear();
                    tabTB.TabPages.Add(tabTB_Back);
                    tabTB.Refresh();
                }
            }


            private void SetControl()   
            //=======================                           
            {
                Boolean pEnabled = false;
                SetControlStatus(pEnabled);     //BG 28JUN13
                if (modMain.gProject.Status == "Open" &&
                    modMain.gUser.Role == "Engineer")                
                {
                    pEnabled = true;
                    SetControlStatus(pEnabled);
                }
                //BG 28JUN13
                //else if (modMain.gProject.Status == "Closed" ||
                //         modMain.gUser.Privilege == "Manufacturing"|| 
                //         modMain.gUser.Privilege == "Designer")               
                //{
                //    pEnabled = false;
                //    SetControlStatus(pEnabled);
                //}
            }


            private void SetControlStatus(Boolean pEnable_In)      
            //===============================================
            {
                //....Bore Dia.
                txtPower_HP_Radial.ReadOnly = !pEnable_In;
                txtPower_Met_Radial.ReadOnly = !pEnable_In;
                txtFlowReqd_gpm_Radial.ReadOnly = !pEnable_In;
                txtFlowReqd_Met_Radial.ReadOnly = !pEnable_In;
                txtTempRise_F_Radial.ReadOnly = !pEnable_In;
                txtTempRise_Met_Radial.ReadOnly = !pEnable_In;
                txtTFilm_Min_Radial.ReadOnly = !pEnable_In;
                txtPadMax_Temp_Radial.ReadOnly = !pEnable_In;
                txtPadMax_Press_Radial.ReadOnly = !pEnable_In;
                txtPadMax_Rot_Radial.ReadOnly = !pEnable_In;
                txtPadMax_Load_Radial.ReadOnly = !pEnable_In;
                txtPadMax_Stress_Radial.ReadOnly = !pEnable_In;

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtPower_HP_TB_Front.ReadOnly = !pEnable_In;
                    txtPower_Met_TB_Front.ReadOnly = !pEnable_In;
                    txtFlowReqd_gpm_TB_Front.ReadOnly = !pEnable_In;
                    txtFlowReqd_Met_TB_Front.ReadOnly = !pEnable_In;
                    txtTempRise_F_TB_Front.ReadOnly = !pEnable_In;
                    txtTempRise_Met_TB_Front.ReadOnly = !pEnable_In;
                    txtTFilm_Min_TB_Front.ReadOnly = !pEnable_In;
                    txtPadMax_Temp_TB_Front.ReadOnly = !pEnable_In;
                    txtPadMax_Press_TB_Front.ReadOnly = !pEnable_In;
                    txtActual_Unit_Load_Front.ReadOnly = !pEnable_In;
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtPower_HP_TB_Back.ReadOnly = !pEnable_In;
                    txtPower_Met_TB_Back.ReadOnly = !pEnable_In;
                    txtFlowReqd_gpm_TB_Back.ReadOnly = !pEnable_In;
                    txtFlowReqd_Met_TB_Back.ReadOnly = !pEnable_In;
                    txtTempRise_F_TB_Back.ReadOnly = !pEnable_In;
                    txtTempRise_Met_TB_Back.ReadOnly = !pEnable_In;
                    txtTFilm_Min_TB_Back.ReadOnly = !pEnable_In;
                    txtPadMax_Temp_TB_Back.ReadOnly = !pEnable_In;
                    txtPadMax_Press_TB_Back.ReadOnly = !pEnable_In;
                    txtPadMax_Load_TB_Back.ReadOnly = !pEnable_In;
                }
            }

            private void ReSetControlVal()
            //============================
            {
                const string pcBlank = "";

                txtPower_HP_Radial.Text = pcBlank;             //....Power
                txtFlowReqd_gpm_Radial.Text = pcBlank;         //....Flow Reqd
                txtTempRise_F_Radial.Text = pcBlank;           //....Temp Rise
                txtTFilm_Min_Radial.Text = pcBlank;            //....TFilmMin

                //  Pad Max
                //  -------
                    txtPadMax_Temp_Radial.Text = pcBlank;      //....Temp
                    txtPadMax_Press_Radial.Text = pcBlank;     //....Pressure
                    txtPadMax_Rot_Radial.Text = pcBlank;       //....Rotation
                    txtPadMax_Load_Radial.Text = pcBlank;      //....Load
                    txtPadMax_Stress_Radial.Text = pcBlank;    //....Stress
            }


            private void DisplayData()
            //========================
            {
                //Radial Bearing
                //---------------

                //....Power
                txtPower_HP_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP, "##0.00");

                //....Flow Reqd
                txtFlowReqd_gpm_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm, "#0.00");

                //....Temp Rise
                txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F, "#0.0");     
                
                //....TFilmMin
                txtTFilm_Min_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min, "#0.0000");      

                //  Pad Max
                //  -------

                    //....Temp
                    txtPadMax_Temp_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Temp, "#0.0");    

                    //....Pressure
                     txtPadMax_Press_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Press, "#0.0");

                    //....Rotation    
                    txtPadMax_Rot_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Rot, "#0.0000");

                    //....Load
                    txtPadMax_Load_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Load, "#0.0");
                   
                    //....Stress
                    txtPadMax_Stress_Radial.Text =  modMain.ConvIntToStr(modMain.NInt(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Stress));


                 //Thrust Bearing
                //---------------
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    //....Power
                    txtPower_HP_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.Power_HP, "##0.00");

                    //....Flow Reqd
                    txtFlowReqd_gpm_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.FlowReqd_gpm, "#0.00");

                    //....Temp Rise
                    txtTempRise_F_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.TempRise_F, "#0.0");    

                    //....TFilmMin
                    txtTFilm_Min_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.TFilm_Min, "#0.0000");      

                    //  Pad Max
                    //  -------

                    //....Temp
                    txtPadMax_Temp_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.PadMax.Temp, "#0.0");    

                    //....Pressure
                    txtPadMax_Press_TB_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.PadMax.Press, "#0.0");

                    //....Load
                    txtActual_Unit_Load_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.UnitLoad, "#0.0");
              
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    //....Power
                    txtPower_HP_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.Power_HP, "##0.00");

                    //....Flow Reqd
                    txtFlowReqd_gpm_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.FlowReqd_gpm, "#0.00");

                    //....Temp Rise
                    txtTempRise_F_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.TempRise_F, "#0.0");

                    //....TFilmMin
                    txtTFilm_Min_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.TFilm_Min, "#0.0000");

                    //  Pad Max
                    //  -------

                    //....Temp
                    txtPadMax_Temp_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.PadMax.Temp, "#0.0");

                    //....Pressure
                    txtPadMax_Press_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.PadMax.Press, "#0.0");

                    //....Load
                    txtPadMax_Load_TB_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.UnitLoad, "#0.0");

                }

            }


            private void SetLocalObject()
            //===========================
            {
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.Power_HP = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP;          //....Power
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.FlowReqd_gpm = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm;  //....Flow Reqd
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F;      //....Temp Rise
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TFilm_Min = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min;        //....TFilmMin

                //  Pad Max
                //  -------
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Load = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Load;           //....Temp
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Press = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Press;         //....Pressure
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Rot = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Rot;             //....Rotation
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Stress = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Stress;       //....Load
                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Temp = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax.Temp;   //....Stress

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
                    SaveData();
                    this.Hide();

                    modMain.gfrmBearingDesignDetails.ShowDialog();                
                }


                private void SaveData()
                //=======================
                {
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TFilm_Min = modMain.ConvTextToDouble(txtTFilm_Min_Radial.Text);


                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Temp = modMain.ConvTextToDouble(txtPadMax_Temp_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Press = modMain.ConvTextToDouble(txtPadMax_Press_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Rot = modMain.ConvTextToDouble(txtPadMax_Rot_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Load = modMain.ConvTextToDouble(txtPadMax_Load_Radial.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.PadMax_Stress = modMain.ConvTextToDouble(txtPadMax_Stress_Radial.Text);


                    if (mProduct.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.Power_HP = 
                                                                            modMain.ConvTextToDouble(txtPower_HP_TB_Front.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.FlowReqd_gpm =
                                                                            modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Front.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.TempRise_F =
                                                                            modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.TFilm_Min =
                                                                            modMain.ConvTextToDouble(txtTFilm_Min_TB_Front.Text);


                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.PadMax_Temp = 
                                                                            modMain.ConvTextToDouble(txtPadMax_Temp_TB_Front.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.PadMax_Press =
                                                                            modMain.ConvTextToDouble(txtPadMax_Press_TB_Front.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).PerformData.UnitLoad =
                                                                            modMain.ConvTextToDouble(txtActual_Unit_Load_Front.Text);


                    }
                    if (mProduct.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.Power_HP =
                                                                            modMain.ConvTextToDouble(txtPower_HP_TB_Back.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.FlowReqd_gpm =
                                                                            modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Back.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.TempRise_F =
                                                                            modMain.ConvTextToDouble(txtTempRise_F_TB_Back.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.TFilm_Min =
                                                                            modMain.ConvTextToDouble(txtTFilm_Min_TB_Back.Text);


                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.PadMax_Temp =
                                                                            modMain.ConvTextToDouble(txtPadMax_Temp_TB_Back.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.PadMax_Press =
                                                                            modMain.ConvTextToDouble(txtPadMax_Press_TB_Back.Text);

                        ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).PerformData.UnitLoad =
                                                                            modMain.ConvTextToDouble(txtPadMax_Load_TB_Back.Text);
                    }

                }


                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {
                    this.Hide();
                }


            #endregion

            #region "TEXTBOX RELATED ROUTINE"
            //-------------------------------

                #region "POWER"

                     private void txtPower_TextChanged(object sender, EventArgs e)
                    //=============================================================
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtPower_HP_Radial":
                                //--------------------
                                if (!mblnPower_Eng_Radial_ManuallyChanged) return;

                                if (txtPower_HP_Radial.Text != "")
                                {
                                    Double pPower = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);
                                    txtPower_Met_Radial.Text = modMain.gUnit.CFac_Power_EngToMet(pPower).ToString("#0.0");
                                }
                                else
                                {
                                    txtPower_Met_Radial.Text = "";
                                    return;
                                }

                                txtPower_HP_Radial.ForeColor = Color.Black;
                                txtPower_Met_Radial.ForeColor = Color.Blue;
                                
                                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);

                                Double pTempRise_F = ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F;
                                txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(pTempRise_F, "#0.0");

                                break;


                            case "txtPower_HP_TB_Front":
                                //----------------------
                                if (!mblnPower_Eng_Thrust_ManuallyChanged[0]) return;

                                if(txtPower_HP_TB_Front.Text!="")
                                {
                                    Double pPower = modMain.ConvTextToDouble(txtPower_HP_TB_Front.Text);
                                    txtPower_Met_TB_Front.Text = modMain.gUnit.CFac_Power_EngToMet(pPower).ToString("#0.0");
                                }
                                else
                                {
                                    txtPower_Met_TB_Front.Text = "";
                                    return;
                                }

                                txtPower_HP_TB_Front.ForeColor = Color.Black;
                                txtPower_Met_TB_Front.ForeColor = Color.Blue;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_TB_Front.Text);

                                Double pTempRise_F_TB_Front = ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.TempRise_F;
                                txtTempRise_F_TB_Front.Text = modMain.ConvDoubleToStr(pTempRise_F_TB_Front, "#0.0");

                                break;


                            case "txtPower_HP_TB_Back":
                                //---------------------
                                if (!mblnPower_Eng_Thrust_ManuallyChanged[1]) return;

                                if(txtPower_HP_TB_Back.Text!="")
                                {
                                    Double pPower = modMain.ConvTextToDouble(txtPower_HP_TB_Back.Text);
                                    txtPower_Met_TB_Back.Text = modMain.gUnit.CFac_Power_EngToMet(pPower).ToString("#0.0");
                                }
                                else
                                {
                                    txtPower_Met_TB_Back.Text = "";
                                    return;
                                }

                                txtPower_HP_TB_Back.ForeColor = Color.Black;
                                txtPower_Met_TB_Back.ForeColor = Color.Blue;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_TB_Back.Text);

                                Double pTempRise_F_TB_Back = ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.TempRise_F;
                                txtTempRise_F_TB_Back.Text = modMain.ConvDoubleToStr(pTempRise_F_TB_Back, "#0.0");

                                break;


                            case "txtPower_Met_Radial":
                                //---------------------
                                 if (!mblnPower_Met_Radial_ManuallyChanged) return;

                                 if (txtPower_Met_Radial.Text != "")
                                 {
                                     Double pPower = modMain.ConvTextToDouble(txtPower_Met_Radial.Text);
                                     txtPower_HP_Radial.Text = modMain.gUnit.CFac_Power_MetToEng(pPower).ToString("#0.0");
                                 }
                                 else
                                 {
                                     txtPower_HP_Radial.Text = "";
                                     return;
                                 }

                                 txtPower_Met_Radial.ForeColor = Color.Black;
                                 txtPower_HP_Radial.ForeColor = Color.Blue;
                               
                                 ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);
                                
                                break;

                            case "txtPower_Met_TB_Front":
                                //-----------------------
                                if (!mblnPower_Met_Thrust_ManuallyChanged[0]) return;

                                if(txtPower_Met_TB_Front.Text!="")
                                {
                                    Double pPower = modMain.ConvTextToDouble(txtPower_Met_TB_Front.Text);
                                    txtPower_Met_TB_Front.Text = modMain.gUnit.CFac_Power_MetToEng(pPower).ToString("#0.0");
                                }
                                else
                                {
                                    txtPower_HP_TB_Front.Text = "";
                                    return;
                                }

                                txtPower_Met_TB_Front.ForeColor = Color.Black;
                                txtPower_HP_TB_Front.ForeColor = Color.Blue;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_TB_Front.Text);

                                break;

                            case "txtPower_Met_TB_Back":
                                //----------------------
                                if (!mblnPower_Met_Thrust_ManuallyChanged[1]) return;

                                if (txtPower_Met_TB_Back.Text != "")
                                {
                                    Double pPower = modMain.ConvTextToDouble(txtPower_Met_TB_Back.Text);
                                    txtPower_Met_TB_Back.Text = modMain.gUnit.CFac_Power_MetToEng(pPower).ToString("#0.0");
                                }
                                else
                                {
                                    txtPower_HP_TB_Back.Text = "";
                                    return;
                                }

                                txtPower_Met_TB_Back.ForeColor = Color.Black;
                                txtPower_HP_TB_Back.ForeColor = Color.Blue;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.Power_HP = modMain.ConvTextToDouble(txtPower_HP_TB_Back.Text);

                                break;
                        }                           
                    }

                   
                    //private void txtPower_MouseDown(object sender, MouseEventArgs e)
                    ////==============================================================
                    //{
                    //    TextBox pTxtBox = (TextBox)sender;

                    //    switch (pTxtBox.Name)
                    //    {
                    //        case "txtPower_HP_Radial":
                    //        //------------------------                           
                    //            mblnPower_Eng_Radial_ManuallyChanged = true;
                    //            mblnPower_Met_Radial_ManuallyChanged = false;
                    //            break;
                                
                    //        case "txtPower_HP_TB_Front":
                    //        //--------------------------
                    //            mblnPower_Eng_Thrust_ManuallyChanged[0] = true;
                    //            mblnPower_Met_Thrust_ManuallyChanged[0] = false;
                    //            break;

                    //        case "txtPower_HP_TB_Back":
                    //        //-------------------------
                    //            mblnPower_Eng_Thrust_ManuallyChanged[1] = true;
                    //            mblnPower_Met_Thrust_ManuallyChanged[1] = false;
                    //            break;

                    //        case "txtPower_Met_Radial":
                    //        //---------------------
                    //            mblnPower_Met_Radial_ManuallyChanged = true;
                    //            mblnPower_Eng_Radial_ManuallyChanged = false;
                    //            break;

                    //        case "txtPower_Met_TB_Front":
                    //        //-----------------------
                    //            mblnPower_Eng_Thrust_ManuallyChanged[0] = false;
                    //            mblnPower_Met_Thrust_ManuallyChanged[0] = true;
                    //            break;

                    //        case "txtPower_Met_TB_Back":
                    //        //--------------------------
                    //            mblnPower_Eng_Thrust_ManuallyChanged[1] = false;
                    //            mblnPower_Met_Thrust_ManuallyChanged[1] = true;
                    //            break;
                    //    }                       
                    //}


                    private void txtPower_KeyDown(object sender, KeyEventArgs e)        
                    //==========================================================
                    {
                         TextBox pTxtBox = (TextBox)sender;

                         switch (pTxtBox.Name)
                         {
                             case "txtPower_HP_Radial":
                                 //------------------------  
                                 mblnPower_Eng_Radial_ManuallyChanged = true;
                                 mblnPower_Met_Radial_ManuallyChanged = false;
                                 break;

                             case "txtPower_HP_TB_Front":
                                 //--------------------------
                                 mblnPower_Eng_Thrust_ManuallyChanged[0] = true;
                                 mblnPower_Met_Thrust_ManuallyChanged[0] = false;
                                 break;

                             case "txtPower_HP_TB_Back":
                                 //-------------------------
                                 mblnPower_Eng_Thrust_ManuallyChanged[1] = true;
                                 mblnPower_Met_Thrust_ManuallyChanged[1] = false;
                                 break;

                             case "txtPower_Met_Radial":
                                 //---------------------
                                 mblnPower_Eng_Radial_ManuallyChanged = false;
                                 mblnPower_Met_Radial_ManuallyChanged = true;
                                 break;

                             case "txtPower_Met_TB_Front":
                                 //-----------------------
                                 mblnPower_Eng_Thrust_ManuallyChanged[0] = false;
                                 mblnPower_Met_Thrust_ManuallyChanged[0] = true;
                                 break;

                             case "txtPower_Met_TB_Back":
                                 //--------------------------
                                 mblnPower_Eng_Thrust_ManuallyChanged[1] = false;
                                 mblnPower_Met_Thrust_ManuallyChanged[1] = true;
                                 break;

                         }
                    }

                #endregion


                #region "FLOW REQD"

                    private void txtFlowReqd_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                         TextBox pTxtBox = (TextBox)sender;

                         switch (pTxtBox.Name)
                         {
                             case "txtFlowReqd_gpm_Radial":
                                 //------------------------
                                  if (!mblnFlowReqd_Eng_Radial_ManuallyChanged) return;

                                  if (txtFlowReqd_gpm_Radial.Text != "")
                                  {
                                    Double pFlowReqd = modMain.ConvTextToDouble(txtFlowReqd_gpm_Radial.Text);
                                    txtFlowReqd_Met_Radial.Text = modMain.gUnit.CFac_GPM_EngToMet(pFlowReqd).ToString("#0.0");
                                  }
                                  else
                                  {
                                    txtFlowReqd_Met_Radial.Text = "";
                                    return;
                                  }

                                  txtFlowReqd_gpm_Radial.ForeColor = Color.Black;
                                  txtFlowReqd_Met_Radial.ForeColor = Color.Blue;

                                  ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_Radial.Text);

                                  Double pTempRise_F = ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F;
                                  txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(pTempRise_F, "#0.0");
                                 
                                  break;

                             case "txtFlowReqd_gpm_TB_Front":
                                 //--------------------------
                                  if(!mblnFlowReqd_Eng_Thrust_ManuallyChanged[0]) return;

                                  if (pTxtBox.Text != "")
                                  {
                                      Double pFlowReqd = modMain.ConvTextToDouble(pTxtBox.Text);
                                      txtFlowReqd_Met_TB_Front.Text = modMain.gUnit.CFac_GPM_EngToMet(pFlowReqd).ToString("#0.0");
                                  }
                                  else
                                  {
                                      txtFlowReqd_Met_TB_Front.Text = "";
                                      return;
                                  }

                                  pTxtBox.ForeColor = Color.Black;
                                  txtFlowReqd_Met_TB_Front.ForeColor = Color.Blue;

                                  ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Front.Text);

                                  Double pTempRise_F_TB_Front = ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.TempRise_F;
                                  txtTempRise_F_TB_Front.Text = modMain.ConvDoubleToStr(pTempRise_F_TB_Front, "#0.0");
                                                                   
                                 break;

                             case "txtFlowReqd_gpm_TB_Back":
                                 //-------------------------
                                  if(!mblnFlowReqd_Eng_Thrust_ManuallyChanged[1]) return;

                                  if (pTxtBox.Text != "")
                                  {
                                      Double pFlowReqd = modMain.ConvTextToDouble(pTxtBox.Text);
                                      txtFlowReqd_Met_TB_Back.Text = modMain.gUnit.CFac_GPM_EngToMet(pFlowReqd).ToString("#0.0");
                                  }
                                  else
                                  {
                                      txtFlowReqd_Met_TB_Back.Text = "";
                                      return;
                                  }

                                  pTxtBox.ForeColor = Color.Black;
                                  txtFlowReqd_Met_TB_Back.ForeColor = Color.Blue;

                                  ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Back.Text);

                                  Double pTempRise_F_TB_Back = ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.TempRise_F;
                                  txtTempRise_F_TB_Back.Text = modMain.ConvDoubleToStr(pTempRise_F_TB_Back, "#0.0");

                                 break;

                             case "txtFlowReqd_Met_Radial":
                                 //------------------------
                                  if (!mblnFlowReqd_Met_Radial_ManuallyChanged) return;

                                  if (txtFlowReqd_Met_Radial.Text != "")
                                  {
                                    Double pFlowReqd = modMain.ConvTextToDouble(txtFlowReqd_Met_Radial.Text);
                                    txtFlowReqd_gpm_Radial.Text = modMain.gUnit.CFac_LPM_MetToEng(pFlowReqd).ToString("#0.0"); 
                                  }
                                  else
                                  {                                    
                                    txtFlowReqd_gpm_Radial.Text = "";
                                    return;
                                  }

                                  txtFlowReqd_gpm_Radial.ForeColor = Color.Blue;
                                  txtFlowReqd_Met_Radial.ForeColor = Color.Black;

                                 ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_Radial.Text); 
                                 break;


                             case "txtFlowReqd_Met_TB_Front":
                                 //--------------------------
                                 if(!mblnFlowReqd_Met_Thrust_ManuallyChanged[0]) return;

                                  if (pTxtBox.Text != "")
                                  {
                                      Double pFlowReqd = modMain.ConvTextToDouble(pTxtBox.Text);
                                      txtFlowReqd_gpm_TB_Front.Text = modMain.gUnit.CFac_LPM_MetToEng(pFlowReqd).ToString("#0.0");
                                  }
                                  else
                                  {
                                      txtFlowReqd_gpm_TB_Front.Text = "";
                                      return;
                                  }

                                  pTxtBox.ForeColor = Color.Black;
                                  txtFlowReqd_gpm_TB_Front.ForeColor = Color.Blue;

                                  ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Front.Text);
                                
                                  break;


                             case "txtFlowReqd_Met_TB_Back":
                                 //-------------------------
                                 if(!mblnFlowReqd_Met_Thrust_ManuallyChanged[1]) return;

                                  if (pTxtBox.Text != "")
                                  {
                                      Double pFlowReqd = modMain.ConvTextToDouble(pTxtBox.Text);
                                      txtFlowReqd_gpm_TB_Back.Text = modMain.gUnit.CFac_LPM_MetToEng(pFlowReqd).ToString("#0.0");
                                  }
                                  else
                                  {
                                      txtFlowReqd_gpm_TB_Back.Text = "";
                                      return;
                                  }

                                  pTxtBox.ForeColor = Color.Black;
                                  txtFlowReqd_gpm_TB_Front.ForeColor = Color.Blue;

                                  ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.FlowReqd_gpm = modMain.ConvTextToDouble(txtFlowReqd_gpm_TB_Front.Text);

                                 break;
                         }
                    }


                    //private void txtFlowReqd_MouseDown(object sender, MouseEventArgs e)
                    ////=================================================================
                    //{
                    //    TextBox pTxtBox = (TextBox)sender;

                    //    switch (pTxtBox.Name)
                    //    {
                    //        case "txtFlowReqd_gpm_Radial":
                    //        //----------------------------
                    //            mblnFlowReqd_Eng_Radial_ManuallyChanged = true;
                    //            mblnFlowReqd_Met_Radial_ManuallyChanged = false;
                    //            break;

                    //        case "txtFlowReqd_gpm_TB_Front":
                    //        //------------------------------
                    //            mblnFlowReqd_Eng_Thrust_ManuallyChanged[0] = true;
                    //            mblnFlowReqd_Met_Thrust_ManuallyChanged[0] = false;
                    //            break;


                    //        case "txtFlowReqd_gpm_TB_Back":
                    //        //-------------------------
                    //            mblnFlowReqd_Eng_Thrust_ManuallyChanged[1] = true;
                    //            mblnFlowReqd_Met_Thrust_ManuallyChanged[1] = false;
                    //            break;

                    //        case "txtFlowReqd_Met_Radial":
                    //        //----------------------------                                
                    //            mblnFlowReqd_Met_Radial_ManuallyChanged = true;
                    //            mblnFlowReqd_Eng_Radial_ManuallyChanged = false;
                    //            break;

                    //        case "txtFlowReqd_Met_TB_Front":
                    //        //------------------------------
                    //             mblnFlowReqd_Eng_Thrust_ManuallyChanged[0] = false;
                    //            mblnFlowReqd_Met_Thrust_ManuallyChanged[0] = true;
                    //            break;

                    //        case "txtFlowReqd_Met_TB_Back":
                    //        //-----------------------------
                    //            mblnFlowReqd_Eng_Thrust_ManuallyChanged[1] = false;
                    //            mblnFlowReqd_Met_Thrust_ManuallyChanged[1] = true;
                    //            break;
                    //    }                      
                    //}


                    private void txtFlowReqd_KeyDown(object sender, KeyEventArgs e)
                    //=============================================================         
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtFlowReqd_gpm_Radial":
                                //----------------------------
                                mblnFlowReqd_Eng_Radial_ManuallyChanged = true;
                                mblnFlowReqd_Met_Radial_ManuallyChanged = false;
                                break;

                            case "txtFlowReqd_gpm_TB_Front":
                                //------------------------------
                                mblnFlowReqd_Eng_Thrust_ManuallyChanged[0] = true;
                                mblnFlowReqd_Met_Thrust_ManuallyChanged[0] = false;
                                break;


                            case "txtFlowReqd_gpm_TB_Back":
                                //-------------------------
                                mblnFlowReqd_Eng_Thrust_ManuallyChanged[1] = true;
                                mblnFlowReqd_Met_Thrust_ManuallyChanged[1] = false;
                                break;

                            case "txtFlowReqd_Met_Radial":
                                //----------------------------                                
                                mblnFlowReqd_Met_Radial_ManuallyChanged = true;
                                mblnFlowReqd_Eng_Radial_ManuallyChanged = false;
                                break;

                            case "txtFlowReqd_Met_TB_Front":
                                //------------------------------
                                mblnFlowReqd_Eng_Thrust_ManuallyChanged[0] = false;
                                mblnFlowReqd_Met_Thrust_ManuallyChanged[0] = true;
                                break;

                            case "txtFlowReqd_Met_TB_Back":
                                //-----------------------------
                                mblnFlowReqd_Eng_Thrust_ManuallyChanged[1] = false;
                                mblnFlowReqd_Met_Thrust_ManuallyChanged[1] = true;
                                break;
                        }
                      
                    }

                #endregion


                #region " TEMP RISE"

                    private void txtTempRise_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtTempRise_F_Radial":
                                //----------------------
                                if (!mblnTempRise_Eng_Radial_ManuallyChanged) return;

                                if (txtTempRise_F_Radial.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text);
                                    int pTempRise_Met = (int)modMain.NInt(modMain.gUnit.CFac_TRise_EngToMet(pTempRise));
                                    txtTempRise_Met_Radial.Text = pTempRise_Met.ToString();                            
                                }
                                else
                                {
                                    txtTempRise_Met_Radial.Text = "";
                                    return;
                                }

                                //txtTempRise_F_Radial.ForeColor = Color.Black;
                                txtTempRise_Met_Radial.ForeColor = Color.Blue;

                                Double pTempRiseF_Cur = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text);
                                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F = pTempRiseF_Cur;

                                Double pTempRiseF_Calc = modMain.ConvTextToDouble(((clsBearing_Radial_FP)mProduct.Bearing).PerformData.Calc_TempRise_F().ToString("#0.0"));
                                SetColor_TempRise (pTempRiseF_Cur, pTempRiseF_Calc, ref txtTempRise_F_Radial);
                            
                                break;


                            case "txtTempRise_F_TB_Front":
                                //------------------------
                                if (!mblnTempRise_Eng_Thrust_ManuallyChanged[0]) return;

                                if (txtTempRise_F_TB_Front.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);
                                   
                                    int pTempRise_Met = (int)modMain.NInt(modMain.gUnit.CFac_TRise_EngToMet(pTempRise));
                                    txtTempRise_Met_TB_Front.Text = pTempRise_Met.ToString();        
                                }
                                else
                                {
                                    txtTempRise_Met_TB_Front.Text = "";
                                    return;
                                }

                                //txtTempRise_F_TB_Front.ForeColor = Color.Black;
                                txtTempRise_Met_TB_Front.ForeColor = Color.Blue;

                                Double pTempRiseF_Cur_Front = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);;
                                ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.TempRise_F = pTempRiseF_Cur_Front;

                                Double pTempRiseF_Calc_Front = modMain.ConvTextToDouble(((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.Calc_TempRise_F().ToString("#0.0"));
                                SetColor_TempRise(pTempRiseF_Cur_Front, pTempRiseF_Calc_Front, ref txtTempRise_F_TB_Front);
                                                             
                                break;

                            case "txtTempRise_F_TB_Back":
                                //-----------------------
                                 if (!mblnTempRise_Eng_Thrust_ManuallyChanged[1]) return;

                                 if (txtTempRise_F_TB_Back.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F_TB_Back.Text);
                                    int pTempRise_Met = (int)modMain.NInt(modMain.gUnit.CFac_TRise_EngToMet(pTempRise));
                                    txtTempRise_Met_TB_Back.Text = pTempRise_Met.ToString();        
                                }
                                else
                                {
                                    txtTempRise_Met_TB_Back.Text = "";
                                    return;
                                }

                                //txtTempRise_F_TB_Back.ForeColor = Color.Black;
                                txtTempRise_Met_TB_Back.ForeColor = Color.Blue;

                                Double pTempRiseF_Cur_Back = modMain.ConvTextToDouble(txtTempRise_F_TB_Back.Text);
                                ((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.TempRise_F = pTempRiseF_Cur_Back;

                                Double pTempRiseF_Calc_Back = modMain.ConvTextToDouble(((clsBearing_Thrust_TL)mProduct.EndConfig[1]).PerformData.Calc_TempRise_F().ToString("#0.0"));
                                SetColor_TempRise(pTempRiseF_Cur_Back, pTempRiseF_Calc_Back, ref txtTempRise_F_TB_Back);

                                break;

                            case "txtTempRise_Met_Radial":
                                //------------------------
                                 if (!mblnTempRise_Met_Radial_ManuallyChanged) return;                             

                                if (txtTempRise_Met_Radial.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_Met_Radial.Text);
                                    int pTempRise_Eng = (int)modMain.NInt(modMain.gUnit.CFac_TRise_MetToEng(pTempRise));
                                    txtTempRise_F_Radial.Text = pTempRise_Eng.ToString();
                                }

                                else
                                {
                                    txtTempRise_F_Radial.Text = "";
                                    return;
                                }

                                txtTempRise_F_Radial.ForeColor = Color.Blue;
                                txtTempRise_Met_Radial.ForeColor = Color.Black;

                                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text);  
                                
                                break;

                            case "txtTempRise_Met_TB_Front":
                                //--------------------------
                                if (!mblnTempRise_Met_Thrust_ManuallyChanged[0]) return;

                                if (txtTempRise_Met_TB_Front.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);
                                    int pTempRise_Eng = (int)modMain.NInt(modMain.gUnit.CFac_TRise_MetToEng(pTempRise));
                                    txtTempRise_Met_TB_Front.Text = pTempRise_Eng.ToString();        
                                }
                                else
                                {
                                    txtTempRise_Met_TB_Front.Text = "";
                                    return;
                                }

                                txtTempRise_F_TB_Front.ForeColor = Color.Blue;
                                txtTempRise_Met_TB_Front.ForeColor = Color.Black;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);
                                break;

                            case "txtTempRise_Met_TB_Back":
                                //-------------------------
                                if (!mblnTempRise_Met_Thrust_ManuallyChanged[0]) return;

                                if (txtTempRise_Met_TB_Front.Text != "")
                                {
                                    Double pTempRise = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);
                                    int pTempRise_Eng = (int)modMain.NInt(modMain.gUnit.CFac_TRise_MetToEng(pTempRise));
                                    txtTempRise_Met_TB_Front.Text = pTempRise_Eng.ToString();        
                                }
                                else
                                {
                                    txtTempRise_Met_TB_Front.Text = "";
                                    return;
                                }

                                txtTempRise_F_TB_Front.ForeColor = Color.Blue;
                                txtTempRise_Met_TB_Front.ForeColor = Color.Black;

                                ((clsBearing_Thrust_TL)mProduct.EndConfig[0]).PerformData.TempRise_F = modMain.ConvTextToDouble(txtTempRise_F_TB_Front.Text);
                                
                                break;
                        }
                    }

                    private void SetColor_TempRise(Double Curr_TempRise_In, Double Org_TempRise_In, ref TextBox TxtBox_In)
                    //=================================================================================================
                    {                       
                        if (Math.Abs(Curr_TempRise_In - Org_TempRise_In) < modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;
                        }

                    }

        
                    //private void txtTempRise_MouseDown(object sender, MouseEventArgs e)
                    ////=================================================================
                    //{
                    //    TextBox pTxtBox = (TextBox)sender;

                    //    switch (pTxtBox.Name)
                    //    {
                    //        case "txtTempRise_F_Radial":
                    //            //----------------------
                    //            mblnTempRise_Eng_Radial_ManuallyChanged = true;
                    //            mblnTempRise_Met_Radial_ManuallyChanged = false;
                    //            break;

                    //        case "txtTempRise_F_TB_Front":
                    //        //------------------------
                    //            mblnTempRise_Eng_Thrust_ManuallyChanged[0] = true;
                    //            mblnTempRise_Met_Thrust_ManuallyChanged[0] = false;
                    //            break;

                    //        case "txtTempRise_F_TB_Back":
                    //        //---------------------------
                    //            mblnTempRise_Eng_Thrust_ManuallyChanged[1] = true;
                    //            mblnTempRise_Met_Thrust_ManuallyChanged[1] = false;
                    //            break;

                    //        case "txtTempRise_Met_Radial":
                    //        //---------------------------
                    //            mblnTempRise_Eng_Radial_ManuallyChanged = false;
                    //            mblnTempRise_Met_Radial_ManuallyChanged = true;
                    //            break;

                    //        case "txtTempRise_Met_TB_Front":
                    //        //--------------------------
                    //            mblnTempRise_Eng_Thrust_ManuallyChanged[0] = false;
                    //            mblnTempRise_Met_Thrust_ManuallyChanged[0] = true;
                    //            break;

                    //        case "txtTempRise_Met_TB_Back":
                    //        //-----------------------------
                    //            mblnTempRise_Eng_Thrust_ManuallyChanged[1] = false;
                    //            mblnTempRise_Met_Thrust_ManuallyChanged[1] = true;
                    //            break;
                    //    }                      
                    //}


                    private void txtTempRise_KeyDown(object sender, KeyEventArgs e)
                    //=============================================================     
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtTempRise_F_Radial":
                                //----------------------
                                mblnTempRise_Eng_Radial_ManuallyChanged = true;
                                mblnTempRise_Met_Radial_ManuallyChanged = false;
                                break;

                            case "txtTempRise_F_TB_Front":
                                //------------------------
                                mblnTempRise_Eng_Thrust_ManuallyChanged[0] = true;
                                mblnTempRise_Met_Thrust_ManuallyChanged[0] = false;
                                break;

                            case "txtTempRise_F_TB_Back":
                                //---------------------------
                                mblnTempRise_Eng_Thrust_ManuallyChanged[1] = true;
                                mblnTempRise_Met_Thrust_ManuallyChanged[1] = false;
                                break;

                            case "txtTempRise_Met_Radial":
                                //---------------------------
                                mblnTempRise_Eng_Radial_ManuallyChanged = false;
                                mblnTempRise_Met_Radial_ManuallyChanged = true;
                                break;

                            case "txtTempRise_Met_TB_Front":
                                //--------------------------
                                mblnTempRise_Eng_Thrust_ManuallyChanged[0] = false;
                                mblnTempRise_Met_Thrust_ManuallyChanged[0] = true;
                                break;

                            case "txtTempRise_Met_TB_Back":
                                //-----------------------------
                                mblnTempRise_Eng_Thrust_ManuallyChanged[1] = false;
                                mblnTempRise_Met_Thrust_ManuallyChanged[1] = true;
                                break;
                        }
                    }
                           

                #endregion


                private void TextBox_TextChanged(object sender, EventArgs e)
                //============================================================  
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtTFilmMin_Radial":
                            //===================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TFilm_Min = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;

                        case "txtPadMax_Temp_Radial":
                            //============================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Temp = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;

                        case "txtPadMax_Press_Radial":
                            //========================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Press = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;

                        case "txtPadMax_Rot_Radial":
                            //======================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Rot = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;

                        case "txtPadMax_Load_Radial":
                            //======================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Load = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;

                        case "txtPadMax_Stress_Radial":
                            //=========================
                            ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.PadMax_Stress = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;
                    }
                }      

            #endregion

    
        #endregion


    }
}
