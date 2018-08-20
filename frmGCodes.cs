//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmGCodes                              '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Drawing.Printing;

namespace BearingCAD20
{
    public partial class frmGCodes : Form
    {

        #region "MEMBER VARIABLES"
        //***********************
            private clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];

            private ComboBox[] mCmbT1;
            private ComboBox[] mCmbT2;
            private ComboBox[] mCmbT3;
            private ComboBox[] mCmbT4;
            private Label[] mLblT3;

            private TextBox[] mTxtOverlap;
            private NumericUpDown[] mupdOverlap_frac;
            private TextBox[] mTxtCount_Pass;
            private TextBox[] mTxtFeedRate_TL;

            private Button[] mCmdCalculate;

            private TextBox[] mTxtDepth_Backlash;
            private TextBox[] mTxtDepth_Dwell_T;

            private TextBox[] mTxtFeedRate_WeepSlot;
            private TextBox[] mTxtWeepSlot_RMargin;
            private TextBox[] mTxtDepth_WeepSlot_Cut_Per_Pass;
            private TextBox[] mTxtDepth_WeepSlot;

            private Label[] mLblPos;
            private TextBox[] mTxtFileName;
            
            //   Boolean Variables to indicate Control Event Type:   
            //  -------------------------------------------------
            //
            //  ....TextBoxes:
            //  ........If variable TRUE  ==> Changed by the user. 
            //  ........            FALSE ==> Value set programmatically.  

            Boolean mblnT3_Front = false;
            Boolean mblnT3_Back = false;
            Boolean mblnUpdOverlap_frac_Front_Entered = false;
            Boolean mblnUpdOverlap_frac_Back_Entered = false;

            private Boolean mblnRadialMargin_WS_Front = false;
            private Boolean mblnRadialMargin_WS_Back = false;
            private Boolean mblnDepth_WS_Cut_Per_Pass_Front = false;
            private Boolean mblnDepth_WS_Cut_Per_Pass_Back = false;
            
        #endregion


        #region "FORM RELATED ROUTINE"
        //============================

            public frmGCodes()
            //=================
            {
                InitializeComponent();

                mCmbT1 = new ComboBox[] { cmbT1_Front, cmbT1_Back };
                mCmbT2 = new ComboBox[] { cmbT2_Front, cmbT2_Back };
                //mTxtT2 = new TextBox[]  { txtT2_Front, txtT2_Back };
                mCmbT3 = new ComboBox[] { cmbT3_Front, cmbT3_Back };
                mCmbT4 = new ComboBox[] { cmbT4_Front, cmbT4_Back };
                mLblT3 = new Label[] { lblT3_Front, lblT3_Back };
                mTxtOverlap = new TextBox[] { txtOverlap_Frac_Front, txtOverlap_Frac_Back };
                mupdOverlap_frac = new NumericUpDown[] { updOverlap_frac_Front, updOverlap_frac_Back };
                mTxtCount_Pass = new TextBox[] { txtCount_Pass_Front, txtCount_Pass_Back };

                mCmdCalculate = new Button[] { cmdCalculate_Front, cmdCalculate_Back };

                mTxtFeedRate_TL = new TextBox[] { txtFeed_TL_Front, txtFeed_TL_Back };
                mTxtDepth_Backlash = new TextBox[] { txtDepth_Backlash_Front, txtDepth_Backlash_Back };
                mTxtDepth_Dwell_T = new TextBox[] { txtDepth_Dwell_T_Front, txtDepth_Dwell_T_Back };

                mTxtFeedRate_WeepSlot = new TextBox[] { txtFeed_WS_Front, txtFeed_WS_Back };
                //mTxtWeepSlot_RMargin = new TextBox[] { txtRadialMargin_WS_Front, txtRadialMargin_WS_Back };       //BG 09APR13
                mTxtDepth_WeepSlot_Cut_Per_Pass = new TextBox[] { txtDepth_WS_Cut_Per_Pass_Front, txtDepth_WS_Cut_Per_Pass_Back };
                mTxtDepth_WeepSlot = new TextBox[] { txtDepth_WS_Front, txtDepth_WS_Back };

                mLblPos = new Label[] { lblFront, lblBack };
                mTxtFileName = new TextBox[] { txtFileName_Front, txtFileName_Back };

                mblnUpdOverlap_frac_Front_Entered = false;
                mblnUpdOverlap_frac_Back_Entered = false;

                for (int i = 0; i < mupdOverlap_frac.Length; i++)
                {
                    mupdOverlap_frac[i].Maximum =   (decimal)0.950F;
                    mupdOverlap_frac[i].Minimum =   (decimal)0.010F;
                    mupdOverlap_frac[i].Increment = (decimal)0.050F;

                }

            }


            private void frmGCodes_Load(object sender, EventArgs e)
            //=====================================================
            {
                SetTabPages();
                SetLocalObject();

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        LoadT1(i, mCmbT1[i]);
                        LoadT2(i, mCmbT2[i]);
                        //LoadT2(i, mTxtT2[i]);
                        LoadT3(i, mLblT3[i], mCmbT3[i]);
                        LoadT4(mCmbT4[i]);
                    }
                }

                SetControls();

                DisplayData();
            }

            private void SetControls()
            //=========================
            {
                Boolean pEnabled = false;
                if (modMain.gUser.Role == "Engineer" || modMain.gUser.Role == "Editor")
                {
                    pEnabled = true;
                }
                else
                {
                    pEnabled = false;
                }

                SetControlStatus(pEnabled);

               if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                   modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    mLblPos[0].Visible = true;
                    mTxtFileName[0].Visible = true;
                    mLblPos[1].Visible = false;
                    mTxtFileName[1].Visible = false;
                    mLblPos[1].Left  = 438;
                    mTxtFileName[1].Left = 348;
                    if (mEndTB[0].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                    {
                        cmdCreateGCodes.Enabled = false;
                    }
                    else
                    {
                        cmdCreateGCodes.Enabled = true;
                    }
                }
               else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
               {
                   mLblPos[0].Visible = false;
                   mTxtFileName[0].Visible = false;
                   mLblPos[1].Visible = true;
                   mTxtFileName[1].Visible = true;
                   mLblPos[1].Left =178;
                   mTxtFileName[1].Left = 88;

                   if (mEndTB[1].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                   {
                       cmdCreateGCodes.Enabled = false;
                   }
                   else
                   {
                       cmdCreateGCodes.Enabled = true;
                   }
               }
               else if(modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                       modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
               {
                   mLblPos[0].Visible = true;
                   mTxtFileName[0].Visible = true;
                   mLblPos[1].Visible = true;
                   mTxtFileName[1].Visible = true;
                   mLblPos[1].Left = 438;
                   mTxtFileName[1].Left = 348;

                   if (mEndTB[0].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi &&
                       mEndTB[1].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                   {
                       cmdCreateGCodes.Enabled = false;
                   }
                   else
                   {
                       cmdCreateGCodes.Enabled = true;
                   }
               }
            
            }

            private void SetControlStatus(Boolean pEnable_In)
            //===============================================
            {
                for (int i = 0; i < 2; i++)
                {
                    mCmbT1[i].Enabled = pEnable_In;
                    mCmbT2[i].Enabled = pEnable_In;
                    //mTxtT2[i].Enabled = pEnable_In;
                    mCmbT3[i].Enabled = pEnable_In;
                    mCmbT4[i].Enabled = pEnable_In;

                    mTxtOverlap[i].ReadOnly = !pEnable_In;
                    mupdOverlap_frac[i].Enabled = pEnable_In;
                    mCmdCalculate[i].Enabled = pEnable_In;
                    mTxtFeedRate_TL[i].ReadOnly = !pEnable_In;
                    mTxtDepth_Backlash[i].ReadOnly = !pEnable_In;
                    mTxtDepth_Dwell_T[i].ReadOnly = !pEnable_In;
                    mTxtFeedRate_WeepSlot[i].ReadOnly = !pEnable_In;
                    mTxtDepth_WeepSlot_Cut_Per_Pass[i].ReadOnly = !pEnable_In;
                    //mTxtDepth_WeepSlot[i].ReadOnly = !pEnable_In;         //BG 03APR13
                }

                txtFilePath_Directory.ReadOnly = !pEnable_In;
                cmdBrowse.Enabled = pEnable_In;
                txtFileName_Front.ReadOnly = !pEnable_In;
                txtFileName_Back.ReadOnly = !pEnable_In;
                //txtStarting_LineNo.ReadOnly = !pEnable_In;        //BG 03APR13
                txtG0Speed.ReadOnly = !pEnable_In;
            }


            private void SetTabPages()
            //========================
            {
                TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

                tbTB_Pos.TabPages.Clear();

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        tbTB_Pos.TabPages.Add(pTabPages[i]);
                    }
                }
            }


            private void SetLocalObject()
            //===========================
            {
                mEndTB = new clsBearing_Thrust_TL[2];

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[i])).Clone();
                    }
                }
            }

            private void LoadT1(int Indx_In, ComboBox cmbBox_In)
            //==================================================
            {
                string pTblName = "tblManf_EndMill";
                string pFIELD = "fldD_Desig";
                string pWHERE = "WHERE fldType = 'Flat'";

                StringCollection pT1 = new StringCollection();
                modMain.gDB.PopulateStringCol(pT1, pTblName, pFIELD, pWHERE);

                //Load_D_Desig(Indx_In, pT1, cmbBox_In);
                Populate_T1(Indx_In, pT1, cmbBox_In);

                //BG 06SEP12
                if (((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[Indx_In])).T1.D_Desig != cmbBox_In.SelectedItem.ToString())
                {
                    cmbBox_In.Text = ((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[Indx_In])).T1.D_Desig;
                }
            }

            private void Populate_T1(int Indx_In, StringCollection D_Desig_In, ComboBox cmbBox_In)
            //======================================================================================
            {
                Double pR_Span = (mEndTB[Indx_In].PadD[1] - mEndTB[Indx_In].PadD[0])/2;

                List<Double> pDia = new List<Double>();

                Double pMin = 0.0F;
                int pIndx = 0, pSelectIndx = 0;

                //for (int i = 0; i < 2; i++)
                //{
                if (modMain.gProject.Product.EndConfig[Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        cmbBox_In.Items.Clear();

                        for (int j = 0; j < D_Desig_In.Count; j++)
                        {
                            string pVal = D_Desig_In[j].Remove(D_Desig_In[j].Length - 1, 1);
                            Double pD = 0.0;
                            Double pNumerator = 0.0;
                            Double pDenominator = 0.0;

                            if (pVal.Contains("/"))
                            {
                                if (pVal.Contains("-"))
                                {
                                    string[] pTemp_D_Desig = pVal.Split('-');
                                    Double pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                    pD = pPrime_Num + (pNumerator / pDenominator);
                                }
                                else
                                {
                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                    pD = pNumerator / pDenominator;
                                }
                            }
                            else
                                pD = modMain.ConvTextToDouble(pVal);

                           
                            if (pD > pR_Span)
                            {
                                if (pD >= Pref_D("fldPref_T1"))
                                {
                                    cmbBox_In.Items.Add(D_Desig_In[j]);

                                    if (pDia.Count == 0)
                                    {
                                        pMin = pD;
                                        pIndx = 0;
                                    }
                                    else
                                        pIndx++;

                                    pDia.Add(pD);
                                   
                                    if (pMin > pD)
                                    {
                                        pMin = pD;
                                        pSelectIndx = pIndx;
                                    }
                                }
                            }

                        }

                        cmbBox_In.SelectedIndex = pSelectIndx;

                    }
                //}

            }


            private void LoadT2(int Indx_In, ComboBox cmbBox_In)
            //==================================================
            {
                string pTblName = "tblManf_EndMill";
                string pFIELD = "fldD_Desig";
                string pWHERE = "WHERE fldType = 'Flat'";

                StringCollection pT2 = new StringCollection();
                modMain.gDB.PopulateStringCol(pT2, pTblName, pFIELD, pWHERE);

                //Load_D_Desig(Indx_In, pT2, cmbBox_In);
                Populate_T2(Indx_In, pT2, cmbBox_In);

                //BG 06SEP12
                if (((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[Indx_In])).T2.D_Desig != cmbBox_In.SelectedItem.ToString())
                {
                    cmbBox_In.Text = ((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[Indx_In])).T2.D_Desig;
                }

            }

            private void Populate_T2(int Indx_In, StringCollection D_Desig_In, ComboBox cmbBox_In)
            //======================================================================================
            {
                List<Double> pDia = new List<Double>();
                    
                Double pMax = 0.0F;
                int pIndx = 0, pSelectIndx = 0;

                string pVal = "";
                Double pD = 0.0, pNumerator = 0.0, pDenominator = 0.0;
                Double pPrime_Num = 0.0;
                string[] pTemp_D_Desig;
                //for (int i = 0; i < 2; i++)       //BG 09APR13
                //{
                    //if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)       //BG 09APR13
                    if (modMain.gProject.Product.EndConfig[Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        cmbBox_In.Items.Clear();

                        for (int i = 0; i < D_Desig_In.Count; i++)
                        {
                            pVal = D_Desig_In[i].Remove(D_Desig_In[i].Length - 1, 1);
                            pD = 0.0;
                            pNumerator = 0.0;
                            pDenominator = 0.0;

                            if (pVal.Contains("/"))
                            {
                                if (pVal.Contains("-"))
                                {
                                    pTemp_D_Desig = pVal.Split('-');
                                    pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                    pD = pPrime_Num + (pNumerator / pDenominator);
                                }
                                else
                                {
                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                    pD = pNumerator / pDenominator;
                                }
                            }
                            else
                                pD = modMain.ConvTextToDouble(pVal);

                            //if (pD < mEndTB[i].FeedGroove.Wid && pD <= Pref_D("fldPref_T2"))      //BG 09APR13
                            if (pD < mEndTB[Indx_In].FeedGroove.Wid && pD <= Pref_D("fldPref_T2"))
                            {
                                //cmbBox_In.Items.Add(D_Desig_In[i]);
           
                                if (pDia.Count == 0)
                                {
                                    pMax = pD;
                                    pIndx = 0;
                                }
                                else
                                    pIndx++;

                                pDia.Add(pD);
                                                                  
                                if (pMax < pD)
                                {
                                    pMax = pD;
                                    pSelectIndx = pIndx;
                                }
                            }
                        }
                          
                        pDia.Sort();

                        for (int j = 0; j < pDia.Count; j++)
                        {
                            for (int k = 0; k < D_Desig_In.Count; k++)
                            {
                                pVal = D_Desig_In[k].Remove(D_Desig_In[k].Length - 1, 1);
                                pD = 0.0;
                                pNumerator = 0.0;
                                pDenominator = 0.0;

                                if (pVal.Contains("/"))
                                {
                                    if (pVal.Contains("-"))
                                    {
                                        pTemp_D_Desig = pVal.Split('-');
                                        pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                        pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                        pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                        pD = pPrime_Num + (pNumerator / pDenominator);
                                    }
                                    else
                                    {
                                        pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                        pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                        pD = pNumerator / pDenominator;
                                    }
                                }
                                else
                                    pD = modMain.ConvTextToDouble(pVal);

                                if (pDia[j] == pD)
                                {
                                    cmbBox_In.Items.Add(D_Desig_In[k]);
                                }

                            }
                        }
                                                                                                 
                                                         
                        if (cmbBox_In.Items.Count > 0)
                            cmbBox_In.SelectedIndex = pSelectIndx;
                        else
                            cmbBox_In.SelectedIndex = -1;
                    }
                //}

            }


            private void LoadT3(int Indx_In, Label Lbl_In, ComboBox cmbBox_In)
            //=================================================================
            {                
                string pTblName = "tblManf_EndMill";
                string pFIELD = "fldD_Desig";
          
                string pWHERE = "WHERE fldType = '" + Type_T3(Indx_In) + "'";
                Lbl_In.Text = Type_T3(Indx_In);

                StringCollection pT3 = new StringCollection();
                modMain.gDB.PopulateStringCol(pT3, pTblName, pFIELD, pWHERE);

                //Load_D_Desig(Indx_In, pT3, cmbBox_In);
                Populate_T3(Indx_In, pT3, cmbBox_In);

            }

            private void Populate_T3(int Indx_In, StringCollection D_Desig_In, ComboBox cmbBox_In)
            //=====================================================================================
            {
                Boolean pblnWS_T3 = false;
                int pSelectIndx = 0;
                //for (int i = 0; i < 2; i++)
                //{
                if (modMain.gProject.Product.EndConfig[Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        cmbBox_In.Items.Clear();

                        for (int j = 0; j < D_Desig_In.Count; j++)
                        {
                            string pVal = D_Desig_In[j].Remove(D_Desig_In[j].Length - 1, 1);
                            Double pD = 0.0;
                            Double pNumerator = 0.0;
                            Double pDenominator = 0.0;

                            if (pVal.Contains("/"))
                            {
                                if (pVal.Contains("-"))
                                {
                                    string[] pTemp_D_Desig = pVal.Split('-');
                                    Double pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                    pD = pPrime_Num + (pNumerator / pDenominator);
                                }
                                else
                                {
                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                    pD = pNumerator / pDenominator;
                                }
                            }
                            else
                                pD = modMain.ConvTextToDouble(pVal);


                            if (Math.Round(Math.Abs(mEndTB[Indx_In].WeepSlot.Wid - pD), 4) <= 0.0005)
                                {
                                    //if (mEndTB[i].T3.D_Desig == D_Desig_In[j])
                                    //{
                                    //    pblnWS_T3 = true;
                                    //    pSelectIndx = j;
                                    //}
                                    
                                    pblnWS_T3 = true;
                                    pSelectIndx = j;

                                    cmbBox_In.Items.Add(D_Desig_In[j]);     //BG 10SEP12
                                    cmbBox_In.SelectedIndex = 0;
                                }
                           

                            //cmbBox_In.Items.Add(D_Desig_In[j]);
                            
                        }

                        //if (pblnWS_T3)
                        //{
                        //    //cmbBox_In.SelectedIndex = pSelectIndx;
                        //}
                        //else if (mEndTB[i].T3.D_Desig !="")
                        //{
                        //     cmbBox_In.SelectedIndex = D_Desig_In.IndexOf(mEndTB[i].T3.D_Desig);
                        //}
                        if (!pblnWS_T3)
                        {
                            MessageBox.Show("No Cutter is found with diameter same as WeepSlot width, " + mEndTB[Indx_In].WeepSlot.Wid, "Error",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cmbBox_In.SelectedIndex = -1;
                        }
                    }
                //}

            }



            private void Load_D_Desig(int Indx_In, StringCollection D_Desig_In, ComboBox cmbBox_In)
            //======================================================================================
            {
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        cmbBox_In.Items.Clear();

                        for (int j = 0; j < D_Desig_In.Count; j++)
                        {
                            string pVal = D_Desig_In[j].Remove(D_Desig_In[j].Length - 1, 1);
                            Double pD = 0.0;
                            Double pNumerator = 0.0;
                            Double pDenominator = 0.0;

                            if (pVal.Contains("/"))
                            {
                                if (pVal.Contains("-"))
                                {
                                    string[] pTemp_D_Desig = pVal.Split('-');
                                    Double pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                    pD = pPrime_Num + (pNumerator / pDenominator);
                                }
                                else
                                {
                                    pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                    pD = pNumerator / pDenominator;
                                }
                            }
                            else
                                pD = modMain.ConvTextToDouble(pVal);

                            if (pD < mEndTB[i].FeedGroove.Wid)
                            {
                                cmbBox_In.Items.Add(D_Desig_In[j]);


                            }
                        }
                    }
                }
     
            }


            private String Type_T3(int Indx_In)
            //==================================
            {              
                if (mEndTB[Indx_In].WeepSlot.Type == clsBearing_Thrust_TL.clsWeepSlot.eType.Rectangular)
                {
                    mEndTB[Indx_In].T3.Type = clsEndMill.eType.Flat;                    
                }
                else if (mEndTB[Indx_In].WeepSlot.Type == clsBearing_Thrust_TL.clsWeepSlot.eType.Circular)
                {
                    mEndTB[Indx_In].T3.Type = clsEndMill.eType.Ball;
                }
                else if (mEndTB[Indx_In].WeepSlot.Type == clsBearing_Thrust_TL.clsWeepSlot.eType.V_notch)
                {
                    mEndTB[Indx_In].T3.Type = clsEndMill.eType.Chamfer;
                }

                return mEndTB[Indx_In].T3.Type.ToString();
            }


            private void LoadT4(ComboBox cmbBox_In)
            //=====================================
            {
                int pRecCount = 0;
                string pTblName = "tblManf_EndMill";
                string pFIELD = "fldD_Desig";
                string pWHERE = "WHERE fldType = 'Chamfer'";

                pRecCount = modMain.gDB.PopulateCmbBox(cmbBox_In, pTblName, pFIELD, pWHERE, true);

                if (pRecCount > 0)
                    cmbBox_In.SelectedIndex = 0;
            }
       

            private void DisplayData()
            //========================
            {
               
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        if (Math.Abs(mEndTB[i].G0Speed - mEndTB[i].G0_SPEED_DEF) < modMain.gcEPS)
                        {
                            txtG0Speed.Text = modMain.ConvDoubleToStr(mEndTB[i].G0_SPEED_DEF, "#0.00");
                        }
                        else
                            txtG0Speed.Text = modMain.ConvDoubleToStr(mEndTB[i].G0Speed, "#0.00");
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        //....T1 End Mill                     
                        if (mEndTB[i].T1.D_Desig != "" && mEndTB[i].T1.D_Desig != null)
                            mCmbT1[i].Text = mEndTB[i].T1.D_Desig;
                        else
                        {
                            mCmbT1[i].SelectedIndex = mCmbT1[i].Items.Count - 1;
                        }

                        //....T2 End Mill                     
                        if (mEndTB[i].T2.D_Desig != "" && mEndTB[i].T2.D_Desig != null)
                            mCmbT2[i].Text = mEndTB[i].T2.D_Desig;
                        else
                        {
                            if (mCmbT2[i].Items.Contains("1/8''"))
                            {
                                int pIndx = mCmbT2[i].Items.IndexOf("1/8''");
                                mCmbT2[i].SelectedIndex = pIndx;
                            }
                            else
                            {
                                mCmbT2[i].SelectedIndex = mCmbT2[i].Items.Count - 1;
                            }
                        }
                        
                        //...T3 End Mill
                        if (mEndTB[i].T3.D_Desig != "" && mEndTB[i].T3.D_Desig != null)
                            mCmbT3[i].Text = mEndTB[i].T3.D_Desig;
                     

                        //....T4 Mill
                        if (mEndTB[i].T4.D_Desig != "" && mEndTB[i].T4.D_Desig != null)
                            mCmbT4[i].Text = mEndTB[i].T4.D_Desig;
                        else
                            mCmbT4[i].SelectedIndex = 0;
                  
                        //....Overlap
                        if (mEndTB[i].Overlap_frac > modMain.gcEPS)
                        {
                            mTxtOverlap[i].Text = mEndTB[i].Overlap_frac.ToString("#0.000");
                            Display_Count_Pass(i);
                        }
                        else
                        {
                            mTxtOverlap[i].Text = modMain.ConvDoubleToStr(mEndTB[i].DESIGN_OVERLAP_FRAC, "#0.000");
                        }

                        //....TaperLand
                        if (mEndTB[i].FeedRate.Taperland > modMain.gcEPS)
                            mTxtFeedRate_TL[i].Text = modMain.ConvDoubleToStr(mEndTB[i].FeedRate.Taperland, "#0");
                        else
                            mTxtFeedRate_TL[i].Text = modMain.ConvDoubleToStr(mEndTB[i].FEED_RATE_TL_DEF, "#0");

                        if (mEndTB[i].Depth_TL_Backlash > modMain.gcEPS)
                            mTxtDepth_Backlash[i].Text = modMain.ConvDoubleToStr(mEndTB[i].Depth_TL_Backlash, "#0.000");
                        else
                            mTxtDepth_Backlash[i].Text = modMain.ConvDoubleToStr(mEndTB[i].DEPTH_TL_BACKLASH, "#0.000");

                        if (mEndTB[i].Depth_TL_Dwell_T> modMain.gcEPS)
                            mTxtDepth_Dwell_T[i].Text = modMain.ConvDoubleToStr(mEndTB[i].Depth_TL_Dwell_T, "#0");
                        else
                            mTxtDepth_Dwell_T[i].Text = modMain.ConvDoubleToStr(mEndTB[i].DEPTH_TL_DWELL_T, "#0");

                        //....WeepSlot
                        if (mEndTB[i].FeedRate.WeepSlot > modMain.gcEPS)
                            mTxtFeedRate_WeepSlot[i].Text = modMain.ConvDoubleToStr(mEndTB[i].FeedRate.WeepSlot, "#0");
                        else
                            mTxtFeedRate_WeepSlot[i].Text = modMain.ConvDoubleToStr(mEndTB[i].FEED_RATE_WEEPSLOT_DEF, "#0");

                        if (mEndTB[i].Depth_WS_Cut_Per_Pass > modMain.gcEPS)
                            mTxtDepth_WeepSlot_Cut_Per_Pass[i].Text = modMain.ConvDoubleToStr(mEndTB[i].Depth_WS_Cut_Per_Pass, "#0.000");
                        else
                            mTxtDepth_WeepSlot_Cut_Per_Pass[i].Text = modMain.ConvDoubleToStr(mEndTB[i].DEPTH_WS_CUT_PER_PASS_DEF, "#0.000");

                        mTxtDepth_WeepSlot[i].Text = modMain.ConvDoubleToStr(mEndTB[i].WeepSlot.Depth, "#0.000");

                        //mTxtWeepSlot_RMargin[i].Text = modMain.ConvDoubleToStr(mEndTB[i].RMargin_WeepSlot,"#0.000"); // SUC 03SEP12   //BG 03APR13
                        

                        //....File Directory
                        txtFilePath_Directory.Text = mEndTB[i].FilePath_Dir;


                        //....File Name
                        string pFileName="";
                        string pProjectNo = modMain.gProject.No;
                        if (modMain.gProject.No_Suffix != "")
                        {
                            pProjectNo = modMain.gProject.No + "-" + modMain.gProject.No_Suffix;
                        }

                        if(i==0)
                            pFileName = pProjectNo + "_TLTB_Front_GCode.txt";
                        else
                            pFileName = pProjectNo + "_TLTB_Back_GCode.txt";

                        mTxtFileName[i].Text = pFileName;

                        //BG 03APR13
                        ////....Starting Line No.
                        //if (mEndTB[i].Starting_LineNo > modMain.gcEPS)
                        //   txtStarting_LineNo.Text = mEndTB[i].Starting_LineNo.ToString();
                    }
                }

            }

            private void Display_Count_Pass(int Indx_In)
            //===========================================
            {
                Double pOverlap_frac = mEndTB[Indx_In].Overlap_frac;

                mEndTB[Indx_In].CountPass_TL();

                if (Math.Abs(pOverlap_frac - Math.Round( mEndTB[Indx_In].Overlap_frac,3)) <= modMain.gcEPS)
                {
                    mTxtCount_Pass[Indx_In].Text = mEndTB[Indx_In].CountPass_TL().ToString();
                }
                else
                {
                    mTxtCount_Pass[Indx_In].Text = "";
                    mEndTB[Indx_In].Overlap_frac = pOverlap_frac;
                }

            }

        #endregion


        #region" CONTROL EVENT RELATED ROUTINE:"
        //*************************************

            #region "COMBOBOX RELATED ROUTINES:"
            //---------------------------------- 

                private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
                //====================================================================
                {
                     ComboBox pCmbBox = (ComboBox)sender;

                     switch (pCmbBox.Name)
                     {
                         case "cmbT1_Front":
                             //-------------
                             if (mEndTB[0] != null)
                             {
                                 mEndTB[0].T1.D_Desig = pCmbBox.Text;
                             }
                             break;

                         case "cmbT1_Back":
                             //------------
                             if (mEndTB[1] != null)
                             {
                                 mEndTB[1].T1.D_Desig = pCmbBox.Text;
                             }
                             break;

                         case "cmbT2_Front":
                             //-------------
                             if (mEndTB[0] != null)
                             {
                                 mEndTB[0].T2.D_Desig = pCmbBox.Text;
                                 txtOverLap_Front.Text = mEndTB[0].Overlap().ToString("#0.000");
                             }
                             break;

                         case "cmbT2_Back":
                             //------------
                             if (mEndTB[1] != null)
                             {
                                 mEndTB[1].T2.D_Desig = pCmbBox.Text;
                                 txtOverLap_Back.Text = mEndTB[1].Overlap().ToString("#0.000");
                             }
                             break;

                         case "cmbT3_Front":
                             //-------------
                             if (mEndTB[0] != null)
                             {
                                 mEndTB[0].T3.D_Desig = pCmbBox.Text;
                                 //txtRadialMargin_WS_Front.Text = mEndTB[0].Calc_Radial_Margin_WS().ToString("#0.000");
                                 //txtRadialMargin_WS_Front.ForeColor = Color.Blue;

                                 //BG 03APR13
                                 //if (mblnT3_Front)
                                 //{
                                 //    txtRadialMargin_WS_Front.Text = mEndTB[0].Calc_Radial_Margin_WS().ToString("#0.000");
                                 //    txtRadialMargin_WS_Front.ForeColor = Color.Blue;
                                 //}
                                 //else
                                 //{
                                 //    if (mEndTB[0].RMargin_WeepSlot.ToString() != mEndTB[0].Calc_Radial_Margin_WS().ToString("#0.000"))
                                 //    {
                                 //        txtRadialMargin_WS_Front.ForeColor = Color.Black;
                                 //    }
                                 //    else
                                 //    {
                                 //        txtRadialMargin_WS_Front.ForeColor = Color.Blue;
                                 //    }
                                 //    txtRadialMargin_WS_Front.Text = mEndTB[0].RMargin_WeepSlot.ToString("#0.000");
                                 //}

                                 mblnT3_Front = false;
                                 
                             }
                             break;

                         case "cmbT3_Back":
                             //------------
                             if (mEndTB[1] != null)
                             {
                                 mEndTB[1].T3.D_Desig = pCmbBox.Text;
                                 
                                 //BG 03APR13
                                 //if (mblnT3_Back)
                                 //{
                                 //    txtRadialMargin_WS_Back.Text = mEndTB[1].Calc_Radial_Margin_WS().ToString("#0.000");
                                 //    txtRadialMargin_WS_Back.ForeColor = Color.Blue;
                                 //}                                
                                 //else
                                 //{
                                 //    if (mEndTB[1].RMargin_WeepSlot.ToString() != mEndTB[1].Calc_Radial_Margin_WS().ToString("#0.000"))
                                 //    {
                                 //        txtRadialMargin_WS_Back.ForeColor = Color.Black;
                                 //    }
                                 //    else
                                 //    {
                                 //        txtRadialMargin_WS_Back.ForeColor = Color.Blue;
                                 //    }
                                 //    txtRadialMargin_WS_Back.Text = mEndTB[1].RMargin_WeepSlot.ToString("#0.000");
                                 //}

                                 mblnT3_Back = false;
                             }
                                
                             break;

                         case "cmbT4_Front":
                             //-------------
                             if (mEndTB[0] != null)
                             {
                                 mEndTB[0].T4.D_Desig = pCmbBox.Text;                                
                             }
                             break;

                         case "cmbT4_Back":
                             //------------
                             if (mEndTB[1] != null)
                             {
                                 mEndTB[1].T4.D_Desig = pCmbBox.Text;
                             }
                             break;
                     }
                }

                private void cmbT3_Front_MouseDown(object sender, MouseEventArgs e)
                //===================================================================
                {
                    mblnT3_Front = true;
                }


                private void cmbT3_Back_MouseDown(object sender, MouseEventArgs e)
                //=================================================================
                {
                    mblnT3_Back = true;
                }

                private void cmbFlatEndMill_DrawItem(object sender, DrawItemEventArgs e)
                //======================================================================
                {
                    if (e.Index < 0) return;

                    ComboBox pCmbBox = (ComboBox)sender;
                    e.DrawBackground();
                    Brush pBrush = Brushes.Black;

                    string pFieldName = "";

                    switch (pCmbBox.Name)
                    {
                        case "cmbT1_Front":
                        case "cmbT1_Back":
                            //-------------
                            pFieldName = "fldPref_T1";
                            break;

                        case "cmbT2_Front":
                        case "cmbT2_Back":
                            //-------------
                            pFieldName = "fldPref_T2";
                            break;
                    }                                    

                    if (pCmbBox.Items[e.Index].ToString() == Pref_D_Desig(pFieldName))
                        pBrush = Brushes.OrangeRed;

                    e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                        e.Font, pBrush, e.Bounds, StringFormat.GenericDefault);

                    e.DrawFocusRectangle();
                }

                private String Pref_D_Desig(String FieldName_In)
                //===============================================
                {
                    string pstrFIELDS = "fldD_Desig";
                    string pstrFROM = " FROM tblManf_EndMill ";
                    string pstrWHERE = " WHERE " + FieldName_In + " = 1";
                    string pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                    SqlConnection pConnection = new SqlConnection();

                    SqlDataReader pDR = null;
                    pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

                    String pD_Desig = "";

                    while (pDR.Read())
                    {
                        pD_Desig = pDR["fldD_Desig"].ToString();
                    }

                    pDR.Close();
                    pDR = null;
                    pConnection.Close();

                    return pD_Desig;
                }

                private Double Pref_D(String FieldName_In)
                //===========================================
                {
                    Double pD = 0.0F;
                    string pstrFIELDS = "fldD_Desig";
                    string pstrFROM = " FROM tblManf_EndMill ";
                    string pstrWHERE = " WHERE " + FieldName_In + " = 1";
                    string pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                    SqlConnection pConnection = new SqlConnection();

                    SqlDataReader pDR = null;
                    pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

                    String pD_Desig = "";

                    while (pDR.Read())
                    {
                        pD_Desig = pDR["fldD_Desig"].ToString();

                        string pVal = pD_Desig.Remove(pD_Desig.Length - 1, 1);

                        Double pNumerator = 0.0;
                        Double pDenominator = 0.0;

                        if (pVal.Contains("/"))
                        {
                            if (pVal.Contains("-"))
                            {
                                string[] pTemp_D_Desig = pVal.Split('-');
                                Double pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                                pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                                pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                                pD = pPrime_Num + (pNumerator / pDenominator);
                            }
                            else
                            {
                                pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pVal, "/"));
                                pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pVal, "/"));
                                pD = pNumerator / pDenominator;
                            }
                        }
                        else
                            pD = modMain.ConvTextToDouble(pVal);
                    }

                    pDR.Close();
                    pDR = null;
                    pConnection.Close();

                    return pD;

                }

            #endregion


            #region "TEXTBOX RELATED ROUTINE"
            //-------------------------------

                private void TextBox_KeyDown(object sender, KeyEventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    //if (!pTxtBox.ReadOnly)
                    //    pTxtBox.ForeColor = Color.Black;

                    switch (pTxtBox.Name)
                    {
                        case "txtDepth_WS_Cut_Per_Pass_Front":
                            //--------------------------------
                            mblnDepth_WS_Cut_Per_Pass_Front = true;
                            break;

                        case "txtDepth_WS_Cut_Per_Pass_Back":
                            //--------------------------------
                            mblnDepth_WS_Cut_Per_Pass_Back = true;
                            break;
                    }
                }

                //private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
                ////================================================================
                //{
                //    TextBox pTxtBox = (TextBox)sender;

                //    switch (pTxtBox.Name)
                //    {
                //        case "txtRadialMargin_WS_Front":
                //            mblnRadialMargin_WS_Front = true;
                //            break;

                //        case "txtRadialMargin_WS_Back":
                //            mblnRadialMargin_WS_Back = true;
                //            break;
                //    }
                //}

                private void TextBox_TextChanged(object sender, EventArgs e)
                //==========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {

                        case "txtG0Speed":
                        //-----------------
                            for (int i = 0; i < 2; i++)
                            {
                                if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    mEndTB[i].G0Speed = modMain.ConvTextToDouble(pTxtBox.Text);
                                    SetTxtForeColorAndDefVal(mEndTB[i].G0Speed, pTxtBox, mEndTB[i].G0_SPEED_DEF);
                                }
                            }
                            break;

                        case "txtOverlap_Frac_Front":
                            //----------------------
                            mEndTB[0].Overlap_frac = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].Overlap_frac, pTxtBox, mEndTB[0].DESIGN_OVERLAP_FRAC);
                            txtOverLap_Front.Text = mEndTB[0].Overlap().ToString("#0.000");
                                                       
                            updOverlap_frac_Front.Value = (decimal) modMain.ConvTextToDouble(pTxtBox.Text);

                            pTxtBox.BackColor = Color.White;          
                            break;

                        case "txtOverlap_Frac_Back":
                            //----------------------
                            mEndTB[1].Overlap_frac = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].Overlap_frac, pTxtBox, mEndTB[1].DESIGN_OVERLAP_FRAC);
                            txtOverLap_Back.Text = mEndTB[1].Overlap().ToString("#0.000");

                            updOverlap_frac_Back.Value = (decimal)modMain.ConvTextToDouble(pTxtBox.Text);

                            pTxtBox.BackColor = Color.White; 
                            break;
                        
                        //BG 03APR13
                        //case "txtStarting_LineNo":
                            //---------------------
                            //for (int i = 0; i < 2; i++)
                            //{
                            //    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            //    {
                            //        mEndTB[i].Starting_LineNo = modMain.ConvTextToInt(pTxtBox.Text);

                            //    }
                            //}
                            //break;

                        case "txtFeed_TL_Front":
                            //------------------
                            mEndTB[0].FeedRate_Taperland = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].FeedRate.Taperland, pTxtBox, mEndTB[0].FEED_RATE_TL_DEF);
                            break;

                        case "txtFeed_TL_Back":
                            //-----------------
                            mEndTB[1].FeedRate_Taperland = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].FeedRate.Taperland, pTxtBox, mEndTB[1].FEED_RATE_TL_DEF);
                            break;

                        case "txtDepth_Backlash_Front":
                            //-----------------------
                            mEndTB[0].Depth_TL_Backlash = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].Depth_TL_Backlash, pTxtBox, mEndTB[0].DEPTH_TL_BACKLASH);
                            break;

                        case "txtDepth_Backlash_Back":
                            //-------------------------
                            mEndTB[1].Depth_TL_Backlash = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].Depth_TL_Backlash, pTxtBox, mEndTB[1].DEPTH_TL_BACKLASH);
                            break;

                        case "txtDepth_Dwell_T_Front":
                            //-----------------------
                            mEndTB[0].Depth_TL_Dwell_T = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].Depth_TL_Dwell_T, pTxtBox, mEndTB[0].DEPTH_TL_DWELL_T);
                            break;

                        case "txtDepth_Dwell_T_Back":
                            //-----------------------
                            mEndTB[1].Depth_TL_Dwell_T = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].Depth_TL_Dwell_T, pTxtBox, mEndTB[1].DEPTH_TL_DWELL_T);
                            break;

                        case "txtFeed_WS_Front":
                            //------------------
                            mEndTB[0].FeedRate_WeepSlot = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].FeedRate.WeepSlot, pTxtBox, mEndTB[0].FEED_RATE_WEEPSLOT_DEF);
                            break;

                        case "txtFeed_WS_Back":
                            //-----------------
                            mEndTB[1].FeedRate_WeepSlot = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].FeedRate.WeepSlot, pTxtBox, mEndTB[1].FEED_RATE_WEEPSLOT_DEF);
                            break;

                        //BG 03APR13
                        //case "txtRadialMargin_WS_Front":   
                        //    //-------------------------
                        //    mEndTB[0].RMargin_WeepSlot = modMain.ConvTextToDouble(pTxtBox.Text);

                        //    if (mblnRadialMargin_WS_Front)
                        //    {
                        //        pTxtBox.ForeColor = Color.Black;
                        //    }                           
                        //    break;

                        //BG 03APR13
                        //case "txtRadialMargin_WS_Back":   
                        //    //-------------------------
                        //    mEndTB[1].RMargin_WeepSlot = modMain.ConvTextToDouble(pTxtBox.Text);
                        //    if (mblnRadialMargin_WS_Back)
                        //    {
                        //        pTxtBox.ForeColor = Color.Black;
                        //    }
                        //    break;

                        case "txtDepth_WS_Cut_Per_Pass_Front":
                            //--------------------------------
                            mEndTB[0].Depth_WS_Cut_Per_Pass = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColor(mEndTB[0].Depth_WS_Cut_Per_Pass, pTxtBox, mEndTB[0].DEPTH_WS_CUT_PER_PASS_DEF);
                            SetTxtForeColor(txtDepth_WS_Cut_Per_Pass_Front, mEndTB[0].DEPTH_WS_CUT_PER_PASS_DEF);

                           break;

                        case "txtDepth_WS_Cut_Per_Pass_Back":
                            //-------------------------------
                            mEndTB[1].Depth_WS_Cut_Per_Pass = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColor(mEndTB[1].Depth_WS_Cut_Per_Pass, pTxtBox, mEndTB[1].DEPTH_WS_CUT_PER_PASS_DEF);
                            SetTxtForeColor(txtDepth_WS_Cut_Per_Pass_Back, mEndTB[1].DEPTH_WS_CUT_PER_PASS_DEF);
                            break;
                    }
                }

               
                private void TextBox_Validating(object sender, CancelEventArgs e)
                //================================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
          

                    switch (pTxtBox.Name)
                    {
                        case "txtDepth_WS_Cut_Per_Pass_Front":
                            //------------------------------
                            double pDepth_WS_Cut_Per_Pass_Front = modMain.ConvTextToDouble(pTxtBox.Text);

                            if (mblnDepth_WS_Cut_Per_Pass_Front && pTxtBox.Text != "")
                            {
                                if (pDepth_WS_Cut_Per_Pass_Front > mEndTB[0].DEPTH_WS_CUT_PER_PASS_MAX)
                                {
                                    //....Validate & reset to Max Value, if necessary.
                                    pDepth_WS_Cut_Per_Pass_Front = mEndTB[0].Validate_Depth_WS_Cut_Per_Pass(pDepth_WS_Cut_Per_Pass_Front);
                                    e.Cancel = true;
                                }
                            }

                            pTxtBox.Text = pDepth_WS_Cut_Per_Pass_Front.ToString("#0.000");             //....Redisplay.
                            mEndTB[0].Depth_WS_Cut_Per_Pass = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[0].Depth_WS_Cut_Per_Pass, pTxtBox, mEndTB[0].DEPTH_WS_CUT_PER_PASS_DEF);

                            break;

                        case "txtDepth_WS_Cut_Per_Pass_Back":
                            //------------------------------
                            double pDepth_WS_Cut_Per_Pass_Back = modMain.ConvTextToDouble(pTxtBox.Text);

                            if (mblnDepth_WS_Cut_Per_Pass_Back && pTxtBox.Text != "")
                            {
                                if (pDepth_WS_Cut_Per_Pass_Back > mEndTB[1].DEPTH_WS_CUT_PER_PASS_MAX)
                                {
                                    //....Validate & reset to Max Value, if necessary.
                                    pDepth_WS_Cut_Per_Pass_Back = mEndTB[1].Validate_Depth_WS_Cut_Per_Pass(pDepth_WS_Cut_Per_Pass_Back);
                                    e.Cancel = true;
                                }
                            }

                            pTxtBox.Text = pDepth_WS_Cut_Per_Pass_Back.ToString("#0.000");             //....Redisplay.
                            mEndTB[1].Depth_WS_Cut_Per_Pass = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColorAndDefVal(mEndTB[1].Depth_WS_Cut_Per_Pass, pTxtBox, mEndTB[1].DEPTH_WS_CUT_PER_PASS_DEF);

                            break;
                    }
                }

            
                private void SetTxtForeColorAndDefVal(Double OrgVal_In, TextBox TxtBox_In, Double DefVal_In)
                //==========================================================================================     
                {
                    if (OrgVal_In != 0.000)
                    {
                        if (Math.Abs(OrgVal_In - DefVal_In) < modMain.gcEPS)
                            TxtBox_In.ForeColor = Color.Magenta;
                        else
                            TxtBox_In.ForeColor = Color.Black;

                    }
                    else
                    {                        
                        TxtBox_In.Text = DefVal_In.ToString();
                        TxtBox_In.ForeColor = Color.Magenta;
                    }
                }


                private void SetTxtForeColor(TextBox TxtBox_In, Double DefVal_In)
                //===============================================================  SG 04APR13     
                {
                    if (Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - DefVal_In) < modMain.gcEPS)
                    {
                        TxtBox_In.ForeColor = Color.Magenta;
                    }
                    else
                    {
                        TxtBox_In.ForeColor = Color.Black;
                    }
                }

            #endregion


            #region "NUMERIC UPDOWN RELATED:"
                //-------------------------------

                    private void updOverlap_frac_ValueChanged(object sender, EventArgs e)
                    //======================================================================
                    {
                        NumericUpDown pUpd = (NumericUpDown)sender;

                        switch (pUpd.Name)
                        {
                            case "updOverlap_frac_Front":
                                if (mblnUpdOverlap_frac_Front_Entered)
                                {
                                    txtOverlap_Frac_Front.Text = updOverlap_frac_Front.Value.ToString("#0.000");

                                    txtOverlap_Frac_Front.BackColor = Color.White;
                                    txtCount_Pass_Front.Text = "";
                                }
                                break;

                            case "updOverlap_frac_Back":
                                if (mblnUpdOverlap_frac_Back_Entered)
                                {
                                    txtOverlap_Frac_Back.Text = updOverlap_frac_Back.Value.ToString("#0.000");

                                    txtOverlap_Frac_Back.BackColor = Color.White;
                                    txtCount_Pass_Back.Text = "";
                                }
                                break;
                        }
                    }

                    private void updOverlap_frac_Front_Enter(object sender, EventArgs e)
                    //==================================================================
                    {
                        NumericUpDown pUpd = (NumericUpDown)sender;

                        switch (pUpd.Name)
                        {
                            case "updOverlap_frac_Front":
                                mblnUpdOverlap_frac_Front_Entered = true;
                                break;

                            case "updOverlap_frac_Back":
                                mblnUpdOverlap_frac_Back_Entered = true;
                                break;
                        }

                    }

                #endregion


            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------
                
                private void cmdBrowse_Click(object sender, EventArgs e)
                //======================================================
                {
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath_Directory.Text = folderBrowserDialog1.SelectedPath;
                    }
                }

                private void cmdCalculate_Click(object sender, EventArgs e)
                //=========================================================
                {
                    Button pcmdButton = (Button)sender;
                    int pIndx = 0;
  
                    switch (pcmdButton.Name)
                    {
                        case "cmdCalculate_Front":
                            //--------------------
                            pIndx = 0;
                            mblnUpdOverlap_frac_Front_Entered = false;
                            break;

                        case "cmdCalculate_Back":
                            //-------------------
                            pIndx = 1;
                            mblnUpdOverlap_frac_Back_Entered = false;
                            break;
                    }
                   
                    mTxtCount_Pass[pIndx].Text = mEndTB[pIndx].CountPass_TL().ToString("#0");

                    mTxtOverlap[pIndx].Text = mEndTB[pIndx].Overlap_frac.ToString("#0.000");
                    mTxtOverlap[pIndx].ForeColor = Color.Blue;

                    if (!mEndTB[pIndx].Validate_OL_frac(mEndTB[pIndx].Overlap_frac))
                    {
                        mTxtOverlap[pIndx].BackColor = Color.Red;
                    }
                    else
                        mTxtOverlap[pIndx].BackColor = Color.White;                 
                }            
                       

                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {

                            if (!mEndTB[i].Validate_OL_frac(mEndTB[i].Overlap_frac))
                            {
                                mTxtOverlap[i].BackColor = Color.Red;
                            }
                            else
                                mTxtOverlap[i].BackColor = Color.White;
                        }
                    }

                    SaveData();
                    this.Hide();
                }

                private void cmdCancel_Click(object sender, EventArgs e)
                //=======================================================
                {
                    this.Hide();
                }

                private void cmdCreateGCodes_Click(object sender, EventArgs e)
                //============================================================
                {
                    if (txtFilePath_Directory.Text == "")
                    {
                        MessageBox.Show("Directory files path can not be blank.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtFilePath_Directory.Focus();
                        return;
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            if (mTxtCount_Pass[i].Text == "")
                            {
                                MessageBox.Show("# of Passes can not be blank. Please click on Calculate button to continue", "Error",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                mTxtCount_Pass[i].Focus();
                                return;
                            }
                        }
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            //mEndTB[i].G0Speed = modMain.ConvTextToDouble(txtG0Speed.Text);
                            if (!mEndTB[i].Validate_OL_frac(mEndTB[i].Overlap_frac))
                            {
                                mTxtOverlap[i].BackColor = Color.Red;
                            }
                            else
                                mTxtOverlap[i].BackColor = Color.White;
                        }
                    }
                                                            
                    int pIndx = 0;
                    string pFileName = "";
                    string pMsg = "";

                  

                    if (tbTB_Pos.TabCount == 1)
                    {
                        if (tbTB_Pos.SelectedTab.Text == "Front")
                        {
                            pIndx = 0;
                        }

                        else if (tbTB_Pos.SelectedTab.Text == "Back")
                        {
                            pIndx = 1;
                        }
                        pFileName = txtFilePath_Directory.Text + "/" + mTxtFileName[pIndx].Text;

                        Cursor = Cursors.WaitCursor;
                        mEndTB[pIndx].WriteFile_GCode(pFileName, pIndx);
                        Cursor = Cursors.Default;
                                             
                    }                  
                    else
                    {
                        for (int i = 0; i < tbTB_Pos.TabCount; i++)
                        {
                            pFileName = txtFilePath_Directory.Text + "/" + mTxtFileName[i].Text;

                            Cursor = Cursors.WaitCursor;
                            if (mEndTB[i].DirectionType == clsBearing_Thrust_TL.eDirectionType.Uni)
                            {
                                mEndTB[i].WriteFile_GCode(pFileName, i);
                                Cursor = Cursors.Default;

                                //pMsg = "The file named '" + mTxtFileName[i].Text + "' \n has been created in the selected directory.";
                                //MessageBox.Show(pMsg, "G-Codes File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                pMsg = "The file named '" + mTxtFileName[i].Text + "' \n has not been created because G-Code generation for Bi-directional TB is not supported in this version.";
                                MessageBox.Show(pMsg, "G-Codes File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }

                }


                private void cmdPrint_Click(object sender, EventArgs e)
                //======================================================
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }


            #endregion            

        #endregion

        #region "UTILITY ROUTINE"
        //=======================
                        
                private void SaveData()
                //=====================
                {
                    //.....Front Tab:   i=0
                    //.....Back Tab:    i=1. 
   
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).G0Speed = modMain.ConvTextToDouble(txtG0Speed.Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).T1.D_Desig = mCmbT1[i].Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).T2.D_Desig = mCmbT2[i].Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).T3.D_Desig = mCmbT3[i].Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).T3.Type = mEndTB[i].T3.Type;                                                     
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).T4.D_Desig = mCmbT4[i].Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).Overlap_frac = modMain.ConvTextToDouble(mTxtOverlap[i].Text);

                            //....TaperLand
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).FeedRate_Taperland = modMain.ConvTextToDouble(mTxtFeedRate_TL[i].Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).Depth_TL_Backlash = modMain.ConvTextToDouble(mTxtDepth_Backlash[i].Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).Depth_TL_Dwell_T = modMain.ConvTextToDouble(mTxtDepth_Dwell_T[i].Text);

                            //....WeepSlot
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).FeedRate_WeepSlot = modMain.ConvTextToDouble(mTxtFeedRate_WeepSlot[i].Text);
                            //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).RMargin_WeepSlot = modMain.ConvTextToDouble(mTxtWeepSlot_RMargin[i].Text);  //BG 03APR13
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass = modMain.ConvTextToDouble(mTxtDepth_WeepSlot_Cut_Per_Pass[i].Text);

                            //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).Starting_LineNo = modMain.ConvTextToInt(txtStarting_LineNo.Text);       //BG 03APR13
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).FilePath_Dir = txtFilePath_Directory.Text;

                        }
                    }
                }

        #endregion

                
         
               
    }
}
