
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSealDesignDetails                   '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//================================================================================

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
    public partial class frmSealDesignDetails : Form
    {

        #region "MEMBER VARIABLE DECLARATION:"
        //***********************************
            
            //....Local Class Object
            private clsSeal[] mEndSeal= new clsSeal[2];

            private TextBox[] mTxtDrainHoles_Annulus_D;
            private Double [] mDrainHoles_Annulus_D_Calc;
            private TextBox[] mTxtDrainHoles_AngStart;
            
            private ComboBox[] mCmbDrainHoles_Annulus_Ratio_L_H;
            private ComboBox[] mCmbDrainHoles_D_Desig;
            private TextBox [] mTxtDrainHoles_Count;
            private TextBox [] mTxtDrainHoles_V;
            private ComboBox[] mCmbDrainHoles_AngBet;
            private Label   [] mLblDrainHoles_AngBet_ULim;
            private Label   [] mLblDrainHoles_AngBet_LLim;
            private ComboBox[] mCmbDrainHoles_AngExit;
            private Label   [] mLblDrainHoles_Notes; 
       
            private Double [] mWireClipHole_AngOther = new Double[5];
            private TextBox[] mTxtBoxWireClipHole_Front;
            private TextBox[] mTxtBoxWireClipHole_Back;
                   
            private Boolean mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = false;
            private Boolean mblnDrainHoles_Annulus_D_ManuallyChanged = false;

            private Boolean mblnDrainHoles_D_Front_ManuallyChanged = false;
            private Boolean mblnDrainHoles_AngBet_Front_ManuallyChanged = false;
            private Boolean mblnDrainHoles_AngExit_Front_ManuallyChanged = false;

        #endregion


        #region "FORM CONSTRUCTOR RELATED ROUTINE:"
        //****************************************

            public frmSealDesignDetails()
            //==========================
            {
                InitializeComponent();

                //...Drain Holes
                mCmbDrainHoles_Annulus_Ratio_L_H = new[] {cmbDrainHoles_Annulus_Ratio_L_H_Front,
                                                          cmbDrainHoles_Annulus_Ratio_L_H_Back};

                mTxtDrainHoles_Annulus_D =         new[] {txtDrainHoles_Annulus_D_Front,
                                                          txtDrainHoles_Annulus_D_Back};

                mCmbDrainHoles_D_Desig =           new[] {cmbDrainHoles_D_Desig_Front, 
                                                          cmbDrainHoles_D_Desig_Back };

                mTxtDrainHoles_Count =             new[] {txtDrainHoles_Count_Front,
                                                          txtDrainHoles_Count_Back};

                mTxtDrainHoles_V =                 new[] {txtDrainHoles_V_Front, 
                                                          txtDrainHoles_V_Back };

                mCmbDrainHoles_AngBet =            new[] {cmbDrainHoles_AngBet_Front,
                                                          cmbDrainHoles_AngBet_Back};

                mLblDrainHoles_AngBet_ULim =       new[] {lblDrainHoles_AngBet_ULim_Front,
                                                          lblDrainHoles_AngBet_ULim_Back};

                mLblDrainHoles_AngBet_LLim =       new[] {lblDrainHoles_AngBet_LLim_Front,
                                                          lblDrainHoles_AngBet_LLim_Back};

                mTxtDrainHoles_AngStart =          new[] {txtDrainHoles_AngStart_Front,
                                                          txtDrainHoles_AngStart_Back};

                mCmbDrainHoles_AngExit =           new[] {cmbDrainHoles_AngExit_Front,
                                                          cmbDrainHoles_AngExit_Back};

                mLblDrainHoles_Notes =       new[] {lblDrainHoles_Notes_Front, 
                                                    lblDrainHoles_Notes_Back };


                object[] mAngExit = new object[4] { 30, 35, 40, 45 };

                for (int i = 0; i < 2; i++)
                {
                    mCmbDrainHoles_AngExit[i].Items.Clear();
                    mCmbDrainHoles_AngExit[i].Items.AddRange(mAngExit);
                }

                for (int i = 0; i < 2; i++)
                {
                    mLblDrainHoles_Notes[i].Text = "Note: Drain hole array is crossing the Bearing S/L." + Environment.NewLine +
                                                         "      An extra drain hole has been added at the end.";
                }


                //....Wire Clip Holes
                mTxtBoxWireClipHole_Front = new[] { txtWireClipHoles_AngStart_Front, 
                                                    txtWireClipHoles_AngOther1_Front,
                                                    txtWireClipHoles_AngOther2_Front  };

                mTxtBoxWireClipHole_Back = new[] { txtWireClipHoles_AngStart_Back, 
                                                   txtWireClipHoles_AngOther1_Back,
                                                   txtWireClipHoles_AngOther2_Back  };
            }

        #endregion


        #region "FORM LOAD RELATED ROUTINES:"
        //**********************************

            private void frmSealDesignDetails_Load(object sender, EventArgs e)
            //================================================================
            {
                mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = false;
                mblnDrainHoles_Annulus_D_ManuallyChanged = false;
                               

                //....Set Local Object.
                SetLocalObject();

                Boolean pblnDrainHoles = true;

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                {
                    //  FRONT
                    //  -----
                    //
                    TabPage[] pTabPages_Front = new TabPage[] { tabMounting_Front, tabDrain_Front, tabTempSensor_Front, tabWC_Front };

                    if (tbEndSealDesignDetails_Front.TabPages.Count < 4)
                    {
                        tbEndSealDesignDetails_Front.TabPages.Clear();
                        tbEndSealDesignDetails_Front.TabPages.AddRange(pTabPages_Front);
                    }

                    if (mEndSeal[0].Blade.Count > 1)
                        pblnDrainHoles = true;
                    else
                        pblnDrainHoles = false;

                    SetTabPages(pblnDrainHoles, tabDrain_Front, 0);

                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Exists)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                        {
                            SetTabPages(true, tabTempSensor_Front, 0);
                            SetTabPages(true, tabWC_Front, 0);
                        }
                        else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                        {
                            SetTabPages(false, tabTempSensor_Front, 0);
                            SetTabPages(false, tabWC_Front, 0);
                        }
                    }
                    else
                    {
                        SetTabPages(false, tabTempSensor_Front, 0);
                        SetTabPages(false, tabWC_Front, 0);
                    }

                    //....Load Drain Hole Annulus_LH_Ratio                    
                    Load_DrainHole_Annulus_LH_Ratio(mEndSeal[0], mCmbDrainHoles_Annulus_Ratio_L_H[0]);

                    //....Load Drain Hole Dia.
                    Load_DrainHole_D_Desig(mEndSeal[0], mCmbDrainHoles_D_Desig[0]);

                    //....Load WC Holes Dia.
                    //LoadWireClipHoles_D(cmbWireClipHoles_Thread_Dia_Desig_Front);                  //BG 02JUL13
                    LoadWireClipHoles_D(mEndSeal[0], cmbWireClipHoles_Thread_Dia_Desig_Front);        //BG 02JUL13
                    LoadWireClipHoles_Count(cmbWireClipHoles_Count_Front);

                    LoadUnit(mEndSeal[0], cmbWireClipHoles_UnitSystem_Front);       //BG 02JUL13
                }


                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    //  BACK
                    //  -----
                    //
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = true;
                    mTxtDrainHoles_Annulus_D[1].ReadOnly = false;
                    mTxtDrainHoles_Annulus_D[1].BackColor = Color.White;
                    mCmbDrainHoles_D_Desig[1].Enabled = true;
                    mTxtDrainHoles_AngStart[1].ReadOnly = false;
                    mCmbDrainHoles_AngBet[1].Enabled = true;
                    mCmbDrainHoles_AngExit[1].Enabled = true;

                    TabPage[] pTabPages_Back = new TabPage[] { tabMounting_Back, tabDrain_Back, tabTempSensor_Back, tabWC_Back };

                    if (tbEndSealDesignDetails_Back.TabPages.Count < 4)
                    {
                        tbEndSealDesignDetails_Back.TabPages.Clear();
                        tbEndSealDesignDetails_Back.TabPages.AddRange(pTabPages_Back);
                    }

                    if (mEndSeal[1].Blade.Count > 1)
                        pblnDrainHoles = true;
                    else
                        pblnDrainHoles = false;

                    SetTabPages(pblnDrainHoles, tabDrain_Back, 1);

                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Exists)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                        {
                            SetTabPages(false, tabTempSensor_Back, 1);
                            SetTabPages(false, tabWC_Back, 1);
                        }
                        else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                        {
                            SetTabPages(true, tabTempSensor_Back, 1);
                            SetTabPages(true, tabWC_Back, 1);
                        }
                    }
                    else
                    {
                        SetTabPages(false, tabTempSensor_Back, 1);
                        SetTabPages(false, tabWC_Back, 1);
                    }

                    //....Load Drain Hole Annulus_LH_Ratio                    
                    Load_DrainHole_Annulus_LH_Ratio(mEndSeal[1], mCmbDrainHoles_Annulus_Ratio_L_H[1]);

                    //....Load Drain Hole Dia.
                    Load_DrainHole_D_Desig(mEndSeal[1], mCmbDrainHoles_D_Desig[1]);

                    //....Load WC Holes Dia.  
                    //LoadWireClipHoles_D(cmbWireClipHoles_Thread_Dia_Desig_Back);          //BG 02JUL13
                    LoadWireClipHoles_D(mEndSeal[1], cmbWireClipHoles_Thread_Dia_Desig_Back);        //BG 02JUL13
                    LoadWireClipHoles_Count(cmbWireClipHoles_Count_Back);

                    LoadUnit(mEndSeal[1], cmbWireClipHoles_UnitSystem_Back);       //BG 02JUL13
                }

                CheckNullParams();

                //....Set Control.
                SetControl();

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = false;

                    mTxtDrainHoles_Annulus_D[1].ReadOnly = true;
                    mTxtDrainHoles_Annulus_D[1].BackColor = mTxtDrainHoles_Count[1].BackColor;

                    mCmbDrainHoles_D_Desig[1].Enabled = false;

                    mTxtDrainHoles_AngStart[1].ReadOnly = true;
                    mTxtDrainHoles_AngStart[1].BackColor = mTxtDrainHoles_Count[1].BackColor;
                    mCmbDrainHoles_AngBet[1].Enabled = false;
                    mCmbDrainHoles_AngExit[1].Enabled = false;
                }

                //....Display Data.
                DisplayData();
               
            }


            #region "Helper Routines:"
            //-----------------------

                private void SetLocalObject()
                //===========================
                {
                    //....Initialize Local Variable.               
                    mEndSeal = new clsSeal[2];

                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                        {
                            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndConfig[i])).Clone();
                        }
                    }
                }


                private void SetTabPages(Boolean Checked_In, TabPage TabPage_In, int Indx_In)
                //============================================================================
                {
                    TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

                    tbEndSealDesignDetails.TabPages.Clear();

                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                        {
                            tbEndSealDesignDetails.TabPages.Add(pTabPages[i]);
                        }
                    }

                    TabPage[] pTabPages_Front = new TabPage[] { tabMounting_Front, tabDrain_Front, tabTempSensor_Front, tabWC_Front };
                    TabPage[] pTabPages_Back = new TabPage[] { tabMounting_Back, tabDrain_Back, tabTempSensor_Back, tabWC_Back };

                    Boolean pTab_Exists = false;

                    switch (Indx_In)
                    {

                        case 0:
                        //-----
                            if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                            {
                                if (!Checked_In)
                                {
                                    tbEndSealDesignDetails_Front.TabPages.Remove(TabPage_In);
                                }

                                foreach (TabPage pTp in tbEndSealDesignDetails_Front.TabPages)
                                {
                                    if (pTp.Text == TabPage_In.Text)
                                    {
                                        pTab_Exists = true;
                                    }
                                }

                                if ((Checked_In) && (!pTab_Exists))
                                {
                                    tbEndSealDesignDetails_Front.TabPages.Clear();
                                    tbEndSealDesignDetails_Front.TabPages.AddRange(pTabPages_Front);
                                }

                                tbEndSealDesignDetails_Front.Refresh();
                            }

                            break;


                        case 1:
                        //-----

                            if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            {
                                if (!Checked_In)
                                {
                                    tbEndSealDesignDetails_Back.TabPages.Remove(TabPage_In);
                                    tbEndSealDesignDetails_Back.Refresh();
                                }

                                foreach (TabPage pTp in tbEndSealDesignDetails_Back.TabPages)
                                {
                                    if (pTp.Text == TabPage_In.Text)
                                    {
                                        pTab_Exists = true;
                                    }
                                }

                                if ((Checked_In) && (!pTab_Exists))
                                {
                                    tbEndSealDesignDetails_Back.TabPages.Clear();
                                    tbEndSealDesignDetails_Back.TabPages.AddRange(pTabPages_Back);
                                }

                                tbEndSealDesignDetails_Back.Refresh();
                            }

                            break;
                    }
                }

                private void Load_DrainHole_Annulus_LH_Ratio(clsSeal Seal_In, ComboBox CmbBox_In)
                //===============================================================================    
                {
                    int pLH_Ratio_Min = 2, pLH_Ratio_Max = 6;

                    mDrainHoles_Annulus_D_Calc = new Double[pLH_Ratio_Max + 1];
                    clsSeal pSeal = (clsSeal)Seal_In.Clone(); 
   
                    CmbBox_In.Items.Clear();

                    while (pLH_Ratio_Min <= pLH_Ratio_Max)
                    {
                        CmbBox_In.Items.Add(pLH_Ratio_Min);
                        pSeal.DrainHoles.Annulus_Ratio_L_H = pLH_Ratio_Min;
                        mDrainHoles_Annulus_D_Calc[pLH_Ratio_Min] = pSeal.DrainHoles.Calc_Annulus_D();
                        pLH_Ratio_Min++;
                    }                    
                }


                private void Load_DrainHole_D_Desig (clsSeal Seal_In, ComboBox CmbBox_In)
                //=======================================================================
                {
                    StringCollection pD_Desig = new StringCollection();

                    string pWHERE = "WHERE fldCons_DrainHole = 'Y' OR" +
                                         " fldCons_DrainHole = 'YP'";

                    modMain.gDB.PopulateStringCol(pD_Desig, "tblManf_Drill", "fldD_Desig", pWHERE);


                    StringCollection pD_Desig_woIn = new StringCollection();  //....D_Desig w/o the inch symbol '"'.

                    Double pNum, pDen;
                    string pDia;

                    //....PB 14JAN13. The following logic assumes that each pD_Desig item contains the inch symbol '"'.
                    //........This assumption is ok so far for the Drain Hole drill sizes but may be in violation in general.
                    //
                    for (int i = 0; i < pD_Desig.Count; i++)
                        pD_Desig[i] = pD_Desig[i].Remove(pD_Desig[i].Length - 1);   //....Removes the last character being '"'.


                    for (int i = 0; i < pD_Desig.Count; i++)

                        if (pD_Desig[i].Contains("/"))
                        {
                            //if (pD_Desig[i].ToString() != "1")        //....PB 14JAN13. This logic doesn't make sense. 
                            //{                                         //........If pD_Desig contains "/", then it cannot be 1. 
                                pNum = Convert.ToInt32(modMain.ExtractPreData(pD_Desig[i], "/"));
                                pDen = Convert.ToInt32(modMain.ExtractPostData(pD_Desig[i], "/"));

                                pDia = Convert.ToDouble(pNum / pDen).ToString();

                                pD_Desig_woIn.Add(pDia);
                            //}

                            //else
                            //{
                            //    pD_Desig_woIn.Add(pD_Desig[i]);
                            //}
                        }


                    //....Change the dia value to fractional format e.g. 5/16, 3/8 and the like. 
                    //........The 2nd argument = TRUE.
                    //
                    modMain.SortNumberwoHash(ref pD_Desig_woIn, true);

                    pD_Desig.Clear();

                    for (int i = 0; i < pD_Desig_woIn.Count; i++)
                        pD_Desig.Add(pD_Desig_woIn[i] + "\"");                      //....Now, re-add the inch symbol '"'.

                    CmbBox_In.Items.Clear();

                    clsSeal pSeal =(clsSeal) Seal_In.Clone();               //SG 25JAN13

                    if (pD_Desig.Count > 0)
                    {
                        for (int i = 0; i < pD_Desig.Count; i++)
                        {
                            //....Per Harout K.'s advice: Include only those D_Desig that yields
                            //........Count < 10. Implemented in V1.1.
                            //
                            pSeal.DrainHoles.D_Desig = pD_Desig[i];

                            if (pSeal.DrainHoles.Count <= 10)         
                            {
                                CmbBox_In.Items.Add(pD_Desig[i]);
                            }
                        }                       
                    }
                }

                //BG 02JUL13
                private void LoadWireClipHoles_D(clsSeal Seal_In, ComboBox CmbBox_In)
                //=====================================================================                                         
                {
                    string pUnitSystem = null;

                    if (Seal_In.Unit.System.ToString() != "")
                        pUnitSystem = Seal_In.Unit.System.ToString().Substring(0, 1);

                    string pWHERE = "WHERE fldUnit = '" + pUnitSystem + "'";

                    if (pUnitSystem == "E")
                    {
                        //....Populate Dia Desig.
                        StringCollection pDia_Desig = new StringCollection();
                        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Screw", "fldD_Desig", pWHERE, true);

                        //....Initialize String Collection.
                        StringCollection pDia_DwHash = new StringCollection();      //....Dia_Desig with # symbol.
                        StringCollection pDia_DwoHash = new StringCollection();     //....Dia_Desig without # symbol. 

                        Double pNumerator, pDenominator;
                        String pFinal;

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("#"))
                            {
                                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                            }
                            else
                            {
                                if (pDia_Desig[i].ToString() != "1")
                                {
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                                    pDia_DwoHash.Add(pFinal);
                                }
                                else
                                    pDia_DwoHash.Add(pDia_Desig[i]);
                            }
                        }

                        //....Sort Dia_Desig with # symbol.
                        modMain.SortNumberwHash(ref pDia_DwHash);

                        //....Sort Dia_Desig without # symbol.
                        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                        }

                        CmbBox_In.Items.Clear();

                        for (int i = 0; i < pDia_DwHash.Count; i++)
                            CmbBox_In.Items.Add(pDia_DwHash[i]);

                        for (int i = 0; i < pDia_DwoHash.Count; i++)
                            CmbBox_In.Items.Add(pDia_DwoHash[i]);
                    }

                    else if (pUnitSystem == "M")
                    {
                        //....Populate Dia Desig.
                        StringCollection pDia_Desig = new StringCollection();
                        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Screw", "fldD_Desig", pWHERE, true);

                        //....Initialize String Collection.
                        StringCollection pDia_D = new StringCollection();  //....Dia_Desig with # symbol.

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("M"))
                            {
                                pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                            }
                        }

                        modMain.SortNumberwoHash(ref pDia_D, false);
                        CmbBox_In.Items.Clear();

                        for (int i = 0; i < pDia_D.Count; i++)
                            pDia_D[i] = "M" + pDia_D[i];

                        for (int i = 0; i < pDia_D.Count; i++)
                            CmbBox_In.Items.Add(pDia_D[i]);
                    }

                    if (Seal_In.WireClipHoles.Screw_Spec.D_Desig != null)
                    {
                        int pIndx = CmbBox_In.Items.IndexOf(Seal_In.WireClipHoles.Screw_Spec.D_Desig);

                        if (pIndx != -1)
                        {
                            CmbBox_In.Text = Seal_In.WireClipHoles.Screw_Spec.D_Desig;
                        }
                        else
                        {
                            CmbBox_In.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        CmbBox_In.SelectedIndex = 0;
                    }

                }


                //BG 02JUL13
                //private void LoadWireClipHoles_D(ComboBox CmbBox_In)
                ////==================================================                                         
                //{
                //    string pUnitSystem = null;

                //    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                //        pUnitSystem = "M";
                //    else if (modMain.gProject.Unit.System == clsUnit.eSystem.English)
                //        pUnitSystem = "E";

                //    string pWHERE = "WHERE fldUnit = '" + pUnitSystem + "'";

                //    if (pUnitSystem == "E")
                //    {
                //        //....Populate Dia Desig.
                //        StringCollection pDia_Desig = new StringCollection();
                //        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Screw", "fldD_Desig", pWHERE, true);

                //        //....Initialize String Collection.
                //        StringCollection pDia_DwHash = new StringCollection();      //....Dia_Desig with # symbol.
                //        StringCollection pDia_DwoHash = new StringCollection();     //....Dia_Desig without # symbol. 

                //        Double pNumerator, pDenominator;
                //        String pFinal;

                //        for (int i = 0; i < pDia_Desig.Count; i++)
                //        {
                //            if (pDia_Desig[i].Contains("#"))
                //            {
                //                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                //            }
                //            else
                //            {
                //                if (pDia_Desig[i].ToString() != "1")
                //                {
                //                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                //                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                //                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                //                    pDia_DwoHash.Add(pFinal);
                //                }
                //                else
                //                    pDia_DwoHash.Add(pDia_Desig[i]);
                //            }
                //        }

                //        //....Sort Dia_Desig with # symbol.
                //        modMain.SortNumberwHash(ref pDia_DwHash);

                //        //....Sort Dia_Desig without # symbol.
                //        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                //        //....Concatinate # symbol with pDia_DwHash.
                //        for (int i = 0; i < pDia_DwHash.Count; i++)
                //        {
                //            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                //        }

                //        CmbBox_In.Items.Clear();

                //        for (int i = 0; i < pDia_DwHash.Count; i++)
                //            CmbBox_In.Items.Add(pDia_DwHash[i]);

                //        for (int i = 0; i < pDia_DwoHash.Count; i++)
                //            CmbBox_In.Items.Add(pDia_DwoHash[i]);
                //    }

                //    else if (pUnitSystem == "M")
                //    {
                //        //....Populate Dia Desig.
                //        StringCollection pDia_Desig = new StringCollection();
                //        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Screw", "fldD_Desig", pWHERE, true);

                //        //....Initialize String Collection.
                //        StringCollection pDia_D = new StringCollection();  //....Dia_Desig with # symbol.

                //        for (int i = 0; i < pDia_Desig.Count; i++)
                //        {
                //            if (pDia_Desig[i].Contains("M"))
                //            {
                //                pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                //            }
                //        }

                //        modMain.SortNumberwoHash(ref pDia_D, false);
                //        CmbBox_In.Items.Clear();

                //        for (int i = 0; i < pDia_D.Count; i++)
                //            pDia_D[i] = "M" + pDia_D[i];

                //        for (int i = 0; i < pDia_D.Count; i++)
                //            CmbBox_In.Items.Add(pDia_D[i]);
                //    }


                //    for (int i = 0; i < 2; i++)
                //    {
                //        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                //        {
                //            if (mEndSeal[i].WireClipHoles.Screw_Spec.D_Desig != null)
                //            {
                //                CmbBox_In.Text = mEndSeal[i].WireClipHoles.Screw_Spec.D_Desig;
                //            }
                //            else
                //            {
                //                CmbBox_In.SelectedIndex = 0;
                //            }
                //        }
                //    }
                //}


                private void LoadWireClipHoles_Count(ComboBox CmbBox_In)
                //======================================================
                {
                    CmbBox_In.Items.Clear();

                    int pCount = 0;
                    if (mEndSeal[0] != null)
                        pCount = mEndSeal[0].WireClipHoles.COUNT_WIRE_CLIP_HOLES_MAX;
                    else if (mEndSeal[1] != null)
                        pCount = mEndSeal[1].WireClipHoles.COUNT_WIRE_CLIP_HOLES_MAX;

                    for (int i = 0; i < pCount; i++)
                    {
                        CmbBox_In.Items.Add(i + 1);
                    }

                    if (modMain.gProject.Product.Accessories.WireClip.Count > 0)
                        CmbBox_In.Text = modMain.gProject.Product.Accessories.WireClip.Count.ToString();
                    else if (mEndSeal[0] != null && mEndSeal[0].WireClipHoles.Count > 0)
                        CmbBox_In.Text = mEndSeal[0].WireClipHoles.Count.ToString();
                    else if (mEndSeal[1] != null && mEndSeal[1].WireClipHoles.Count > 0)
                        CmbBox_In.Text = mEndSeal[1].WireClipHoles.Count.ToString();
                    else
                        CmbBox_In.SelectedIndex = 0;
                }

                //BG 02JUL13
                private void LoadUnit(clsSeal Seal_In, ComboBox CmbBox_In)
                //=========================================================
                {
                     if (CmbBox_In.Items.Count <= 0)
                    {
                        CmbBox_In.Items.Clear();
                        CmbBox_In.Items.Add(clsUnit.eSystem.English.ToString());
                        CmbBox_In.Items.Add(clsUnit.eSystem.Metric.ToString());

                        if (Seal_In.WireClipHoles.Unit.System.ToString() != "")
                            CmbBox_In.Text = Seal_In.WireClipHoles.Unit.System.ToString();
                        else
                            CmbBox_In.SelectedIndex = 0;
                    }
                }

        
                private void CheckNullParams()
                //=============================
                {
                    string pMsg = "";
                    string pCaption = "";

                    if (!IsMountThread_NULL() && !IsFlow_GPM_NULL() && !IsSealDO_Null()
                        && !IsDShaft_NULL())
                    //------------------------------------------------------------------
                    {
                        ;
                    }

                    else if (IsDShaft_NULL())
                    {
                        pMsg = "Enter DShaft Value." + System.Environment.NewLine +
                                "Please open form 'Bearing Geometry & Materials'.";
                        pCaption = "DShaft Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        this.Close();
                        return;
                    }

                    else if (IsFlow_GPM_NULL())
                    {
                        pMsg = "Enter Flow GPM."
                                + System.Environment.NewLine +
                                "Please open form 'Performance Data'.";
                        pCaption = "GPM Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                        this.Close();
                        return;
                    }

                    else if (IsMountThread_NULL())
                    {
                        //pMsg = "Select an appropriate Seal Mount Fixture thread first."
                        //        + System.Environment.NewLine +
                        //        "Please open form 'Bearing Design Details'.";
                        //pCaption = "Seal Thread Error";

                        //MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                        //                MessageBoxIcon.Error);

                        //this.Close();
                        //return;
                    }

                    else if (IsSealDO_Null())
                    {
                        pMsg = "Select an appropriate DFinish Seal Mount Fixture thread first."
                                + System.Environment.NewLine +
                                "Please open form 'Bearing Design Details'.";
                        pCaption = "Seal DO Error";

                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }


                private void SetControl()
                //=======================                           
                {
                    SetControls_MountHoles();

                    Boolean pEnabled;

                    if (modMain.gProject.Status == "Open" &&
                       (modMain.gUser.Role == "Engineer" || modMain.gUser.Role == "Designer"))
                    {
                        pEnabled = true;
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        {
                            txtMountHoles_CBore_Depth_Front.BackColor = Color.White;
                        }
                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        {
                            txtMountHoles_CBore_Depth_Back.BackColor = Color.White;
                        }

                        SetControls_Status(pEnabled);
                    }

                    else if (modMain.gProject.Status == "Closed" ||
                             (modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer"))
                    {
                        pEnabled = false;
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        {
                            txtMountHoles_CBore_Depth_Front.BackColor = txtMountHoles_D_CBore_Front.BackColor;
                        }
                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        {
                            txtMountHoles_CBore_Depth_Back.BackColor = txtMountHoles_D_CBore_Front.BackColor;
                        }

                        SetControls_Status(pEnabled);
                    }

                    mLblDrainHoles_Notes[0].Visible = false;
                    mLblDrainHoles_Notes[1].Visible = false;
                }


                #region "Sub-Helper Routines:"
                //***************************

                    private void SetControls_MountHoles()
                    //===================================
                    {
                        int pT_Pos_Left = 12;
                        int pC_Pos_Left = 100;

                        optMountHoles_Type_CBore_Front.Checked = false;
                        optMountHoles_Type_Thru_Front.Checked = false;
                        optMountHoles_Type_Thread_Front.Checked = false;
                        chkMountHoles_Thread_Thru_Front.Checked = false;

                        optMountHoles_Type_CBore_Back.Checked = false;
                        optMountHoles_Type_Thru_Back.Checked = false;
                        optMountHoles_Type_Thread_Back.Checked = false;
                        chkMountHoles_Thread_Thru_Back.Checked = false;

                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_GoThru)
                        {
                            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                            {
                                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Front.Visible = true;
                                    optMountHoles_Type_Thru_Front.Visible = true;
                                    if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        optMountHoles_Type_CBore_Front.Checked = true;

                                    optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                                    optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;

                                    optMountHoles_Type_Thread_Front.Visible = false;
                                    optMountHoles_Type_Thread_Front.Visible = false;

                                    grpMountHoles_Type_Front.Width = 185;
                                    lblMountHoles_Thread_Depth_Front.Top = 141;
                                    txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                                }

                                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Back.Visible = false;
                                    optMountHoles_Type_Thru_Back.Visible = false;

                                    optMountHoles_Type_Thread_Back.Visible = true;
                                    optMountHoles_Type_Thread_Back.Checked = true;
                                    chkMountHoles_Thread_Thru_Back.Visible = true;

                                    grpMountHoles_Type_Back.Width = 100;
                                    chkMountHoles_Thread_Thru_Back.Left = 120;
                                    lblMountHoles_Thread_Depth_Back.Top = 75;
                                    txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                                }
                            }

                            else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                            {
                                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Front.Visible = false;
                                    optMountHoles_Type_Thru_Front.Visible = false;

                                    optMountHoles_Type_Thread_Front.Visible = true;
                                    optMountHoles_Type_Thread_Front.Checked = true;
                                    chkMountHoles_Thread_Thru_Front.Visible = true;

                                    grpMountHoles_Type_Front.Width = 100;
                                    chkMountHoles_Thread_Thru_Front.Left = 120;
                                    lblMountHoles_Thread_Depth_Front.Top = 75;
                                    txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                                }

                                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Back.Visible = true;
                                    optMountHoles_Type_Thru_Back.Visible = true;
                                    if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        optMountHoles_Type_CBore_Back.Checked = true;

                                    optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                                    optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;

                                    optMountHoles_Type_Thread_Back.Visible = false;
                                    chkMountHoles_Thread_Thru_Back.Visible = false;

                                    grpMountHoles_Type_Back.Width = 185;
                                    lblMountHoles_Thread_Depth_Back.Top = 141;
                                    txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                                }
                            }
                        }

                        else
                        {
                            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                            {
                                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Front.Visible = true;
                                    optMountHoles_Type_Thru_Front.Visible = true;

                                    optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                                    optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;

                                    optMountHoles_Type_Thread_Front.Visible = false;
                                    chkMountHoles_Thread_Thru_Front.Visible = false;

                                    if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                    {
                                        optMountHoles_Type_CBore_Front.Checked = true;
                                    }

                                    else if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                    {
                                        optMountHoles_Type_Thru_Front.Checked = true;
                                    }

                                    grpMountHoles_Type_Front.Width = 185;
                                }

                                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                {
                                    optMountHoles_Type_CBore_Back.Visible = true;
                                    optMountHoles_Type_Thru_Back.Visible = true;

                                    optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                                    optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;

                                    optMountHoles_Type_Thread_Back.Visible = false;
                                    chkMountHoles_Thread_Thru_Back.Visible = false;

                                    if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                    {
                                        optMountHoles_Type_CBore_Back.Checked = true;
                                    }

                                    else if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                    {
                                        optMountHoles_Type_Thru_Back.Checked = true;
                                    }

                                    grpMountHoles_Type_Back.Width = 185;
                                }
                            }
                        }
                    }


                    private void SetControls_Status(Boolean Enabled_In)
                    //================================================
                    {
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        {
                            //  FRONT
                            //  ----- 
                            //
                            txtL_Front.ReadOnly = !Enabled_In;

                            //....Mounting Holes.
                            grpMountHoles_Type_Front.Enabled = Enabled_In;

                            if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                txtMountHoles_CBore_Depth_Front.ReadOnly = !Enabled_In;
                            else if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                            {
                                txtMountHoles_Thread_Depth_Front.ReadOnly = !Enabled_In;
                                chkMountHoles_Thread_Thru_Front.Enabled = Enabled_In;
                            }


                            //....Drain Holes.
                            mCmbDrainHoles_Annulus_Ratio_L_H[0].Enabled = Enabled_In;
                            mTxtDrainHoles_Annulus_D[0].ReadOnly = !Enabled_In;

                            if (!Enabled_In)
                                mTxtDrainHoles_Annulus_D[0].BackColor = mTxtDrainHoles_Count[0].BackColor;
                            else
                                mTxtDrainHoles_Annulus_D[0].BackColor = Color.White;

                            mCmbDrainHoles_D_Desig[0].Enabled = Enabled_In;
                            mTxtDrainHoles_AngStart[0].ReadOnly = !Enabled_In;
                            mCmbDrainHoles_AngBet[0].Enabled = Enabled_In;
                            mCmbDrainHoles_AngExit[0].Enabled = Enabled_In;

                            //....Temp. Sensor.
                            txtTempSensor_D_ExitHole_Front.ReadOnly = !Enabled_In;

                            //....Wire Clip Holes.
                            if (modMain.gProject.Status == "Closed" ||
                                modMain.gUser.Role != "Engineer")
                            {
                                chkWireClipHoles_Front.Enabled = Enabled_In;
                                cmbWireClipHoles_Count_Front.Enabled = Enabled_In;
                            }
                            txtWireClipHoles_DBC_Front.ReadOnly = !Enabled_In;
                            cmbWireClipHoles_UnitSystem_Front.Enabled = Enabled_In;     //BG 02JUL13

                            //........Thread.
                            cmbWireClipHoles_Thread_Dia_Desig_Front.Enabled = Enabled_In;
                            cmbWireClipHoles_Thread_Pitch_Front.Enabled = Enabled_In;
                            txtWireClipHoles_Thread_Depth_Front.ReadOnly = !Enabled_In;

                            txtWireClipHoles_AngStart_Front.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther1_Front.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther2_Front.ReadOnly = !Enabled_In;
                        }

                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        {
                            //  BACK
                            //  ----     
                            //
                            txtL_Back.ReadOnly = !Enabled_In;

                            //....Mounting Holes.
                            grpMountHoles_Type_Back.Enabled = Enabled_In;

                            if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                txtMountHoles_CBore_Depth_Back.ReadOnly = !Enabled_In;
                            else if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                            {
                                txtMountHoles_Thread_Depth_Back.ReadOnly = !Enabled_In;
                                chkMountHoles_Thread_Thru_Back.Enabled = Enabled_In;
                            }

                            //....Drain Holes.
                            mCmbDrainHoles_Annulus_Ratio_L_H[1].Enabled = Enabled_In;
                            mTxtDrainHoles_Annulus_D[1].ReadOnly = !Enabled_In;

                            if (!Enabled_In)
                                mTxtDrainHoles_Annulus_D[1].BackColor = mTxtDrainHoles_Count[1].BackColor;
                            else
                                mTxtDrainHoles_Annulus_D[1].BackColor = Color.White;

                            mCmbDrainHoles_D_Desig[1].Enabled = Enabled_In;
                            mTxtDrainHoles_AngStart[1].ReadOnly = !Enabled_In;
                            mCmbDrainHoles_AngBet[1].Enabled = Enabled_In;
                            mCmbDrainHoles_AngExit[1].Enabled = Enabled_In;

                            //....Temp. Sensor.
                            txtTempSensor_D_ExitHole_Back.ReadOnly = !Enabled_In;

                            //....Wire Clip Holes.
                            if (modMain.gProject.Status == "Closed" ||
                                modMain.gUser.Role != "Engineer")
                            {
                                chkWireClipHoles_Back.Enabled = Enabled_In;
                                cmbWireClipHoles_Count_Back.Enabled = Enabled_In;
                            }
                            txtWireClipHoles_DBC_Back.ReadOnly = !Enabled_In;

                            cmbWireClipHoles_UnitSystem_Back.Enabled = Enabled_In;     //BG 02JUL13

                            //........Thread.
                            cmbWireClipHoles_Thread_Dia_Desig_Back.Enabled = Enabled_In;
                            cmbWireClipHoles_Thread_Pitch_Back.Enabled = Enabled_In;
                            txtWireClipHoles_Thread_Depth_Back.ReadOnly = !Enabled_In;

                            txtWireClipHoles_AngStart_Back.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther1_Back.ReadOnly = !Enabled_In;
                            txtWireClipHoles_AngOther2_Back.ReadOnly = !Enabled_In;
                        }
                    }

                #endregion

            #endregion

        #endregion


        #region"DISPLAY DATA:"
        //********************

            private void DisplayData()
            //========================      
            {
                int pIndex = 0;

                #region "FRONT:"
                //--------------

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        pIndex = 0;

                        txtOD_Front.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].D_Finish, "#0.000");
                        txtL_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].L, "#0.000");

                        #region  "Mounting Holes:"
                        //------------------------ 

                            if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                            {
                                optMountHoles_Type_CBore_Front.Checked = true;
                                txtMountHoles_D_ThruHole_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_Thru, "#0.000");
                                txtMountHoles_D_CBore_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_CBore, "#0.000");
                                txtMountHoles_CBore_Depth_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Depth_CBore, "#0.000");  
                             
                                lblMountHoles_CBoreDepth_LLim_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.CBore_Depth_LowerLimit(), "#0.000");
                                lblMountHoles_CBoreDepth_ULim_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.CBore_Depth_UpperLimit(), "#0.000");
                            }

                            else if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                            {
                                optMountHoles_Type_Thru_Front.Checked = true;
                                txtMountHoles_D_ThruHole_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_Thru, "#0.000");
                            }

                            else if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                            {
                                optMountHoles_Type_Thread_Front.Checked = true;
                                chkMountHoles_Thread_Thru_Front.Checked = mEndSeal[0].MountHoles.Thread_Thru;
                                txtMountHoles_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Depth_Thread, "#0.000");
                            }
                            else
                                optMountHoles_Type_CBore_Front.Checked = true;

                        #endregion


                        #region "Drain:"
                        //--------------

                            //  Annulus
                            //  -------
                            mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.Annulus.Ratio_L_H, "");
                            mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.Annulus.D, "#0.000");
                            IsAnnulusDCalc(mEndSeal[pIndex], ref mTxtDrainHoles_Annulus_D[pIndex]);

                            //  Drain Holes                                                                 
                            //  -----------                                                                     
                            mCmbDrainHoles_D_Desig[pIndex].Text = mEndSeal[pIndex].DrainHoles.D_Desig;
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndSeal[pIndex].DrainHoles.Count);
                            txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.V(), "#0.000");

                            //  Angles:
                            //  ------
                            //
                            ////....Upper & Lower Limits:
                            //Double pAng_ULim_Front = Math.Floor(mEndSeal[pIndex].DrainHoles.AngBet_ULim_Sym());
                            //Double pAng_LLim_Front = Math.Ceiling(mEndSeal[pIndex].DrainHoles.AngBet_LLim());

                            //mLblDrainHoles_AngBet_ULim[pIndex].Text = modMain.ConvDoubleToStr(pAng_ULim_Front, "#0");   
                            //mLblDrainHoles_AngBet_LLim[pIndex].Text = modMain.ConvDoubleToStr(pAng_LLim_Front, "#0");   


                            Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                       mCmbDrainHoles_AngBet[pIndex]);

                            if (mEndSeal[pIndex].DrainHoles.AngBet != 0.0F)
                            {
                                //Populate_DrainHoleAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                //                          mCmbDrainHoles_AngBet[pIndex]);

                                mCmbDrainHoles_AngBet[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngBet, "#0");
                                mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngStart, "#0.0");
                                //IsAngStartCalc(ref mTxtDrainHoles_AngStart[pIndex], mEndSeal[pIndex], pIndex);  //PB 28JAN13
                            }

                            mCmbDrainHoles_AngExit[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngExit, "#0");

                        #endregion


                        #region  "Temp Sensor:"
                        //--------------------- 

                            txtTempSensor_D_ExitHole_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].TempSensor_D_ExitHole, modMain.gUnit.MFormat);
                            txtTempSensor_Hole_DBC_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].TempSensor_DBC_Hole(), "#0.000");

                        #endregion


                        #region  "Wire Clip Holes:"
                        //-------------------------
                            if (modMain.gProject.Product.Accessories.WireClip.Supplied)
                            {
                                chkWireClipHoles_Front.Checked = true;
                                chkWireClipHoles_Front.Enabled = false;
                                SetControl_WireClipHoles_Front();
                                cmbWireClipHoles_Count_Front.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.WireClip.Count);
                                cmbWireClipHoles_Count_Front.Enabled = false;
                            }
                            else
                            {
                                chkWireClipHoles_Front.Enabled = true;
                                chkWireClipHoles_Front.Checked = (mEndSeal[0]).WireClipHoles.Exists;
                                SetControl_WireClipHoles_Front();
                                cmbWireClipHoles_Count_Front.Text = modMain.ConvIntToStr(mEndSeal[0].WireClipHoles.Count);
                                cmbWireClipHoles_Count_Front.Enabled = true;
                            }

                            //  Thread:
                            //  -------
                            //....Dia Desig.
                            cmbWireClipHoles_Thread_Dia_Desig_Front.Text = mEndSeal[0].WireClipHoles.Screw_Spec.D_Desig;

                            //....Unit System
                            cmbWireClipHoles_UnitSystem_Front.Text = mEndSeal[0].WireClipHoles.Unit.System.ToString();      //BG 03JUL13

                            //....Pitch.
                            cmbWireClipHoles_Thread_Pitch_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.Screw_Spec.Pitch, "#0.000");

                            //....Depth.
                            txtWireClipHoles_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.ThreadDepth, modMain.gUnit.MFormat);

                            if (mEndSeal[0].Unit.System == clsUnit.eSystem.English)
                                lblWireClipHoles_LUnit_Front.Text = "in";
                            else if (mEndSeal[0].Unit.System == clsUnit.eSystem.Metric)
                                lblWireClipHoles_LUnit_Front.Text = "mm";


                            //  Angle
                            //  ------
                            txtWireClipHoles_AngStart_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngStart, "#0");
                            txtWireClipHoles_AngOther1_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngOther[0], "#0");
                            txtWireClipHoles_AngOther2_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngOther[1], "#0");
                            txtWireClipHoles_DBC_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.DBC, modMain.gUnit.MFormat);

                            DisplayOtherAngle(mEndSeal[0], lblWireClipHole_AngOther_Front, mTxtBoxWireClipHole_Front);

                        #endregion
                }

                #endregion


                #region "BACK:"
                //-------------
                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        pIndex = 1;

                        txtOD_Back.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].D_Finish, "#0.000");
                        txtL_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].L, "#0.000");

                        #region  "Mounting Holes:"
                        //------------------------ 

                            if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                            {
                                optMountHoles_Type_CBore_Back.Checked = true;
                                txtMountHoles_D_ThruHole_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_Thru, "#0.000");
                                txtMountHoles_D_CBore_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_CBore, "#0.000");
                                txtMountHoles_CBore_Depth_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Depth_CBore, "#0.000");
                               
                                lblMountHoles_CBoreDepth_LLim_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.CBore_Depth_LowerLimit(), "#0.000");
                                lblMountHoles_CBoreDepth_ULim_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.CBore_Depth_UpperLimit(), "#0.000");
                            }

                            else if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                            {
                                optMountHoles_Type_Thru_Back.Checked = true;
                                txtMountHoles_D_ThruHole_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_Thru, "#0.000");
                            }

                            else if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                            {
                                optMountHoles_Type_Thread_Back.Checked = true;
                                chkMountHoles_Thread_Thru_Back.Checked = mEndSeal[1].MountHoles.Thread_Thru;
                                txtMountHoles_Thread_Depth_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Depth_Thread, "#0.000");
                            }
                            else
                                optMountHoles_Type_CBore_Back.Checked = true;

                        #endregion


                        #region "Drain:"
                        //-------------

                            //  Annulus
                            //  -------
                            mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.Annulus.Ratio_L_H, "");
                            mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.Annulus.D, "#0.000");
                            IsAnnulusDCalc(mEndSeal[pIndex], ref mTxtDrainHoles_Annulus_D[pIndex]);


                            //  Drain Holes                                                                 
                            //  -----------    
                            mCmbDrainHoles_D_Desig[pIndex].Text = mEndSeal[pIndex].DrainHoles.D_Desig;
                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndSeal[pIndex].DrainHoles.Count);

                            txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.V(), "#0.000");


                            //  Angles:
                            //  ------

                            Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                      mCmbDrainHoles_AngBet[pIndex]);

                            if (mEndSeal[pIndex].DrainHoles.AngBet != 0.0F)
                            {
                                mCmbDrainHoles_AngBet[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngBet, "#0");
                                mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngStart, "#0.0");
                                //IsAngStartCalc(ref mTxtDrainHoles_AngStart[pIndex], mEndSeal[pIndex], pIndex);     //PB 28JAN13
                            }

                            mCmbDrainHoles_AngExit[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngExit, "#0");

                        #endregion


                        #region  "Temp Sensor:"
                        //---------------------
                           
                            txtTempSensor_D_ExitHole_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].TempSensor_D_ExitHole, modMain.gUnit.MFormat);
                            txtTempSensor_Hole_DBC_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].TempSensor_DBC_Hole(), "#0.000");

                        #endregion


                        #region "Wire Clip Holes:"
                        //  ----------------------
                            if (modMain.gProject.Product.Accessories.WireClip.Supplied)
                            {
                                chkWireClipHoles_Back.Checked = true;
                                chkWireClipHoles_Back.Enabled = false;
                                SetControl_WireClipHoles_Back();
                                cmbWireClipHoles_Count_Back.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.WireClip.Count);
                                cmbWireClipHoles_Count_Back.Enabled = false;
                            }
                            else
                            {
                                chkWireClipHoles_Back.Enabled = true;
                                chkWireClipHoles_Back.Checked = mEndSeal[1].WireClipHoles.Exists;
                                SetControl_WireClipHoles_Back();
                                cmbWireClipHoles_Count_Back.Text = modMain.ConvIntToStr(mEndSeal[1].WireClipHoles.Count);
                                cmbWireClipHoles_Count_Back.Enabled = true;
                            }

                            //  Thread:
                            //  -------
                            //....Dia Desig.
                            cmbWireClipHoles_Thread_Dia_Desig_Back.Text = mEndSeal[1].WireClipHoles.Screw_Spec.D_Desig;


                            //....Unit System
                            cmbWireClipHoles_UnitSystem_Back.Text = mEndSeal[1].WireClipHoles.Unit.System.ToString();       //BG 03JUL13

                            //....Pitch.
                            cmbWireClipHoles_Thread_Pitch_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.Screw_Spec.Pitch, "");

                            //....Depth.
                            txtWireClipHoles_Thread_Depth_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.ThreadDepth, modMain.gUnit.MFormat);


                            if (mEndSeal[1].Unit.System == clsUnit.eSystem.English)
                                lblWireClipHoles_LUnit_Back.Text = "in";
                            else if (mEndSeal[1].Unit.System == clsUnit.eSystem.Metric)
                                lblWireClipHoles_LUnit_Back.Text = "mm";


                            //  Angle
                            //  ------
                            txtWireClipHoles_AngStart_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngStart, "#0");
                            txtWireClipHoles_AngOther1_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngOther[0], "#0");
                            txtWireClipHoles_AngOther2_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngOther[1], "#0");
                            txtWireClipHoles_DBC_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.DBC, modMain.gUnit.MFormat);

                            DisplayOtherAngle(mEndSeal[1], lblWireClipHole_AngOther_Back, mTxtBoxWireClipHole_Back);

                        #endregion
                }

                #endregion
            }


            #region "Helper Routines:"
            //************************

                #region "CHECK CALCULATED FIELD:"
                //------------------------------

                    //PB 28JAN13.
                    //private void IsAngStartCalc(ref TextBox TxtBox_In, clsSeal Seal_In, int Indx_In)
                    ////==============================================================================     
                    //{
                    //    Double pAngStart = modMain.ConvTextToDouble(TxtBox_In.Text);

                    //    clsSeal pTempSeal = null;
                    //    pTempSeal = (clsSeal)Seal_In.Clone();

                    //    ////....Assign Annulus ratio to get calculated Annulus D. 
                    //    //pTempSeal.DrainHoles.AngBet = Seal_In.DrainHoles.AngBet;


                    //    int pRet = 0;

                    //    if (Indx_In == 0)
                    //    {
                    //        pTempSeal.DrainHoles.AngStart = pTempSeal.DrainHoles.Calc_AngStart();
                    //        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart, pAngStart, 0, pRet) > 0)
                    //        {
                    //            TxtBox_In.ForeColor = Color.Black;
                    //            return;
                    //        }
                    //    }

                    //    else if (Indx_In == 1)
                    //    {
                    //        pTempSeal.DrainHoles.AngStart_OtherSide = pTempSeal.DrainHoles.Calc_AngStart_OtherSide();
                    //        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart_OtherSide, pAngStart, 0, pRet) > 0)
                    //        {
                    //            TxtBox_In.ForeColor = Color.Black;
                    //            return;
                    //        }
                    //    }

                    //    TxtBox_In.ForeColor = Color.Blue;
                    //    pTempSeal = null;
                    //}

                    //private void IsAnnulusDCalc(ref TextBox TxtBox_In, clsSeal Seal_In)
                    ////=================================================================    
                    //{
                    //    Double pAnnulusD = modMain.ConvTextToDouble(TxtBox_In.Text);
                    //    clsSeal pTempSeal = null;
                    //    pTempSeal = (clsSeal)Seal_In.Clone();

                    //    //....Assign Annulus ratio to get calculated Annulus D. 
                    //    pTempSeal.DrainHoles.Annulus_Ratio_L_H = Seal_In.DrainHoles.Annulus.Ratio_L_H;
                    //    pTempSeal.DrainHoles.Annulus_D = pTempSeal.DrainHoles.Calc_Annulus_D();

                    //    int pRet = 0;

                    //    if (modMain.CompareVar(pTempSeal.DrainHoles.Annulus.D, pAnnulusD, 3, pRet) > 0)
                    //    {
                    //        TxtBox_In.ForeColor = Color.Black;
                    //        return;
                    //    }

                    //    TxtBox_In.ForeColor = Color.Blue;
                    //    pTempSeal = null;
                    //}


                    private void IsAnnulusDCalc(clsSeal Seal_In, ref TextBox TxtBox_In)
                    //=================================================================    
                    {
                        Double pAnnulusD = modMain.ConvTextToDouble(TxtBox_In.Text);
                        clsSeal pTempSeal = null;
                        pTempSeal = (clsSeal)Seal_In.Clone();

                        //....Assign Annulus ratio to get calculated Annulus D. 
                        pTempSeal.DrainHoles.Annulus_Ratio_L_H = Seal_In.DrainHoles.Annulus.Ratio_L_H;
                        pTempSeal.DrainHoles.Annulus_D = pTempSeal.DrainHoles.Annulus.D;


                        for (int i = 2; i < mDrainHoles_Annulus_D_Calc.Length; i++)
                        {
                            if (Math.Abs(pTempSeal.DrainHoles.Annulus.D - mDrainHoles_Annulus_D_Calc[i])<= modMain.gcEPS)
                            {
                                TxtBox_In.ForeColor = Color.Blue;
                                return;
                            }
                        }

                        TxtBox_In.ForeColor = Color.Black;
                        pTempSeal = null;
                    }

                #endregion


                private void DisplayOtherAngle(clsSeal Seal_In, Label LblBox_In, TextBox[] TxtBox_In)
                //===================================================================================
                {
                    if (Seal_In.WireClipHoles.Count == 1)
                        LblBox_In.Visible = false;
                    else if (Seal_In.WireClipHoles.Count > 0)
                        LblBox_In.Visible = true;

                    for (int i = 1; i < TxtBox_In.Length; i++)
                        TxtBox_In[i].Visible = false;

                    for (int i = 1; i < Seal_In.WireClipHoles.Count; i++)
                        TxtBox_In[i].Visible = true;
                }

            #endregion

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE:"
        //*************************************

            private void tbEndSealDesignDetails_SelectedIndexChanged(object sender, EventArgs e)
            //==================================================================================
            {
                if (tbEndSealDesignDetails.SelectedIndex == 1 && modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                {
                    //....When the "Back" tab is clicked and the End Config - FRONT = SEAL, mimic the display on the Front tab to 
                    //........the "Back" tab.
                    //
                    mCmbDrainHoles_Annulus_Ratio_L_H[1].Text = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.Annulus.Ratio_L_H, "");

                    mTxtDrainHoles_Annulus_D[1].Text         = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.Annulus.D, "#0.000");
                    mTxtDrainHoles_Annulus_D[1].ForeColor    = mTxtDrainHoles_Annulus_D[0].ForeColor;

                    mCmbDrainHoles_D_Desig[1].Text           = mEndSeal[0].DrainHoles.D_Desig;
                    mTxtDrainHoles_Count[1].Text             = modMain.ConvIntToStr(mEndSeal[0].DrainHoles.Count);
                    mTxtDrainHoles_V[1].Text                 = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.V(), "#0.000");
                    

                    Populate_DrainHolesAng_Bet(mEndSeal[1], mLblDrainHoles_AngBet_LLim[1], mLblDrainHoles_AngBet_ULim[1],
                                               mCmbDrainHoles_AngBet[1]);

                    if (mEndSeal[1].DrainHoles.AngBet != 0.0F)
                    {                      
                        mCmbDrainHoles_AngBet[1].Text        = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngBet, "#0");

                        mTxtDrainHoles_AngStart[1].Text      = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngStart_OtherSide(), "#0.0");
                        mTxtDrainHoles_AngStart[1].ForeColor = mTxtDrainHoles_AngStart[0].ForeColor;
                    }

                    mCmbDrainHoles_AngExit[1].Text           = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngExit, "#0");

                    //CheckAndAct_DrainHoles_Crossing_180BearingSL(1);
                }
            }


            #region "OPTION BUTTON RELATED ROUTINE:"
            //--------------------------------------

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //================================================================
                {
                    RadioButton pOptButton = (RadioButton)sender;

                    switch (pOptButton.Name)
                    {
                        case "optMountHoles_Type_CBore_Front":
                            //--------------------------------
                            //....CBore 
                            if (pOptButton.Checked)
                                mEndSeal[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepthULim_Front_Upper.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                            break;


                        case "optMountHoles_Type_Thru_Front":
                            //-------------------------------
                            //....Thru'
                            if (pOptButton.Checked)
                                mEndSeal[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepthULim_Front_Upper.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            break;


                        case "optMountHoles_Type_Thread_Front":
                            //---------------------------------
                            //....Thread
                            if (pOptButton.Checked)
                                mEndSeal[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepthULim_Front_Upper.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepthLLim_Front_Lower.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;

                            break;


                        case "optMountHoles_Type_CBore_Back":
                            //-------------------------------
                            //....CBore 

                            if (pOptButton.Checked)
                                mEndSeal[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepthULim_Back_Upper.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;

                            break;


                        case "optMountHoles_Type_Thru_Back":
                            //------------------------------
                            //....Thru'
                            if (pOptButton.Checked)
                                mEndSeal[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepthULim_Back_Upper.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            break;


                        case "optMountHoles_Type_Thread_Back":
                            //--------------------------------
                            //....Thread
                            if (pOptButton.Checked)
                                mEndSeal[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = optMountHoles_Type_Thread_Back.Checked;

                            //....Thru' D
                            lblMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore D
                            lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore Depth
                            lblMountHoles_Depth_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore Depth Limits
                            lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepthULim_Back_Upper.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepthLLim_Back_Lower.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....Thread Depth
                            lblMountHoles_Thread_Depth_Back.Visible = optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = optMountHoles_Type_Thread_Back.Checked;

                            break;
                    }
                }

            #endregion


            #region "CHECKBOX RELATED ROUTINE:"
            //--------------------------------
                     
                private void chkBox_CheckedChanged(object sender, EventArgs e)
                //=============================================================
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                     {
                         case "chkWireClipHoles_Front":
                        //----------------------------
                          mEndSeal[0].WireClipHoles.Exists = pChkBox.Checked;
                          SetControl_WireClipHoles_Front();
                          break;

                         case "chkWireClipHoles_Back":
                        //-----------------------------
                          mEndSeal[1].WireClipHoles.Exists = pChkBox.Checked;
                          SetControl_WireClipHoles_Back();
                          break;

                         case "chkMountHoles_Thread_Thru_Front":
                          //------------------------------------
                          lblMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                          txtMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                          break;

                         case "chkMountHoles_Thread_Thru_Back":
                          //-----------------------------------
                          lblMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                          txtMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                          break;
                     }
                }


                #region "Helper Routines:"
                //************************
               
                    private void SetControl_WireClipHoles_Front()
                    //===========================================
                    {
                        lblWireClipHole_Count_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Count_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHole_Thread_Size_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_ThreadDia_Desig_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Thread_Dia_Desig_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Pitch_Front.Visible = chkWireClipHoles_Front.Checked;
                        cmbWireClipHoles_Thread_Pitch_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblUnit_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHoles_LUnit_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Thread_Depth_Front.Visible = chkWireClipHoles_Front.Checked;
                        txtWireClipHoles_Thread_Depth_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_wrt_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_DBC_Front.Visible = chkWireClipHoles_Front.Checked;
                        txtWireClipHoles_DBC_Front.Visible = chkWireClipHoles_Front.Checked;

                        lblWireClipHole_Angles_Front.Visible = chkWireClipHoles_Front.Checked;
                        lblWireClipHole_Ang_Start_Front.Visible = chkWireClipHoles_Front.Checked;

                        txtWireClipHoles_AngStart_Front.Visible = chkWireClipHoles_Front.Checked;

                        if (mEndSeal[0].WireClipHoles.Count > 1)
                        {
                            lblWireClipHole_AngOther_Front.Visible = chkWireClipHoles_Front.Checked;
                            txtWireClipHoles_AngOther1_Front.Visible = chkWireClipHoles_Front.Checked;
                        }

                        if (mEndSeal[0].WireClipHoles.Count > 2)
                            txtWireClipHoles_AngOther2_Front.Visible = chkWireClipHoles_Front.Checked;
                    }


                    private void SetControl_WireClipHoles_Back()
                    //==========================================
                    {
                        lblWireClipHole_Count_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Count_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHole_Thread_Size_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_ThreadDia_Desig_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Thread_Dia_Desig_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Pitch_Back.Visible = chkWireClipHoles_Back.Checked;
                        cmbWireClipHoles_Thread_Pitch_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblUnit_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHoles_LUnit_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Thread_Depth_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_Thread_Depth_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_wrt_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_DBC_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_DBC_Back.Visible = chkWireClipHoles_Back.Checked;

                        lblWireClipHole_Angles_Back.Visible = chkWireClipHoles_Back.Checked;
                        lblWireClipHole_Ang_Start_Back.Visible = chkWireClipHoles_Back.Checked;
                        txtWireClipHoles_AngStart_Back.Visible = chkWireClipHoles_Back.Checked;
                    

                        if (mEndSeal[1].WireClipHoles.Count > 1)//|| mEndSeal[1].WireClipHoles.Count == 0
                        {
                            lblWireClipHole_AngOther_Back.Visible = chkWireClipHoles_Back.Checked;
                            txtWireClipHoles_AngOther1_Back.Visible = chkWireClipHoles_Back.Checked;
                        }


                        if (mEndSeal[1].WireClipHoles.Count > 2)//|| mEndSeal[1].WireClipHoles.Count == 0
                            txtWireClipHoles_AngOther2_Back.Visible = chkWireClipHoles_Back.Checked;                   
                    }

                #endregion

            #endregion


            #region "TEXTBOX RELATED ROUTINES:"
            //---------------------------------

                private void TextBox_KeyDown(object sender, KeyEventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    pTxtBox.ForeColor = Color.Black;

                    switch (pTxtBox.Name)                                   
                    {
                        case "txtDrainHoles_Annulus_D_Front":
                        case "txtDrainHoles_Annulus_D_Back":
                            mblnDrainHoles_Annulus_D_ManuallyChanged = true;
                            break;
                    }
                }


                //....PB 29JAN13. Not needed. 
                //private void TextBox_MouseDown(object sender, MouseEventArgs e)
                ////=========================================================
                //{
                //    TextBox pTxtBox = (TextBox)sender;
                //    pTxtBox.ForeColor = Color.Black;
                //}


                private void TxtBox_TextChanged(object sender, EventArgs e)
                //==========================================================   
                {
                    TextBox pTxtBox = (TextBox)sender;
                    int pIndx = 0;

                    double pVal = 0.0;


                    switch (pTxtBox.Name)
                    {
                        case "txtL_Front":
                            //------------
                            mEndSeal[0].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor_L(txtL_Front, modMain.gProject.Product.Calc_L_EndConfig());
                            break;


                        case "txtL_Back":
                            //-----------
                            mEndSeal[1].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor_L(txtL_Back, modMain.gProject.Product.Calc_L_EndConfig());
                            break;


                        case "txtMountHole_Depth_CBore_Front":
                            //--------------------------------
                            Double pMountHole_Depth_CBore_Front = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[0].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Front;
                            break;


                        case "txtMountHole_Depth_CBore_Back":
                            //-------------------------------
                            Double pMountHole_Depth_CBore_Back = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[1].MountHoles.Depth_CBore = pMountHole_Depth_CBore_Back;
                            break;


                        case "txtMountHoles_Thread_Depth_Front":
                            //----------------------------------
                            mEndSeal[0].MountHoles.Depth_Thread = modMain.ConvTextToDouble(pTxtBox.Text);

                            pVal = Math.Round(2 * ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.D, 3);

                            if (Math.Abs(mEndSeal[0].MountHoles.Depth_Thread - pVal) < modMain.gcEPS)
                                txtMountHoles_Thread_Depth_Front.ForeColor = Color.Magenta;
                            else
                                txtMountHoles_Thread_Depth_Front.ForeColor = Color.Black;
                            break;


                        case "txtMountHoles_Thread_Depth_Back":
                            //---------------------------------
                            mEndSeal[1].MountHoles.Depth_Thread = modMain.ConvTextToDouble(pTxtBox.Text);

                            pVal = Math.Round(2 * ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.D, 3);

                            if (Math.Abs(mEndSeal[1].MountHoles.Depth_Thread - pVal) < modMain.gcEPS)
                                txtMountHoles_Thread_Depth_Back.ForeColor = Color.Magenta;
                            else
                                txtMountHoles_Thread_Depth_Back.ForeColor = Color.Black;
                            break;


                        case "txtDrainHoles_Annulus_D_Front":
                            //-------------------------------
                            pIndx = 0;

                            pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[pIndx].DrainHoles.Annulus_D = pVal;

                            Populate_DrainHolesAng_Bet(mEndSeal[pIndx], mLblDrainHoles_AngBet_LLim[pIndx], mLblDrainHoles_AngBet_ULim[pIndx], 
                                                      mCmbDrainHoles_AngBet[pIndx]);


                            if (mblnDrainHoles_Annulus_D_ManuallyChanged)
                            {
                                //....The forecolor has already been changed to black in the MouseDown event.
                                //
                                mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].Text =
                                                 modMain.ConvDoubleToStr(mEndSeal[pIndx].DrainHoles.Calc_Annulus_Ratio_L_H(), "#0.000");
                                mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].ForeColor = Color.Blue;
                                mCmbDrainHoles_Annulus_Ratio_L_H[1].ForeColor = Color.Blue;

                                mblnDrainHoles_Annulus_D_ManuallyChanged = false;
                            }

                            else
                            {
                                //....Annulus D is programmatically written (not manually changed).
                                if (Math.Abs(pVal - Math.Round(mEndSeal[pIndx].DrainHoles.Calc_Annulus_D(),3)) <= modMain.gcEPS)
                                {
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }
                            }

                            //....Back Seal: Display the same Annulus D.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{
                            //    mTxtDrainHoles_Annulus_D[1].Text = modMain.ConvDoubleToStr(mEndSeal[pIndx].DrainHoles.Annulus.D, "#0.000");
                            //}
                            break;


                        case "txtDrainHoles_Annulus_D_Back":
                            //------------------------------

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                //....This event will be fired only when the Back Seal Controls are enabled.
                                pIndx = 1;

                                pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                mEndSeal[pIndx].DrainHoles.Annulus_D = pVal;

                                Populate_DrainHolesAng_Bet(mEndSeal[pIndx], mLblDrainHoles_AngBet_LLim[pIndx], mLblDrainHoles_AngBet_ULim[pIndx],
                                                           mCmbDrainHoles_AngBet[pIndx]);

                                if (mblnDrainHoles_Annulus_D_ManuallyChanged)
                                {
                                    mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].Text =
                                                        modMain.ConvDoubleToStr(mEndSeal[pIndx].DrainHoles.Calc_Annulus_Ratio_L_H(), "#0.000");
                                    mCmbDrainHoles_Annulus_Ratio_L_H[pIndx].ForeColor = Color.Blue;

                                    mblnDrainHoles_Annulus_D_ManuallyChanged = false;
                                }
                                else
                                {
                                    //....Annulus D is programmatically written (not manually changed).
                                    if (Math.Abs(pVal - Math.Round(mEndSeal[pIndx].DrainHoles.Calc_Annulus_D(), 3)) <= modMain.gcEPS)
                                    {
                                        pTxtBox.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        pTxtBox.ForeColor = Color.Black;
                                    }
                                }
                            }

                            break;


                        case "txtDrainHoles_AngStart_Front":
                            //------------------------------
                            pIndx = 0;

                            pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[pIndx].DrainHoles.AngStart = pVal;

                            if (Math.Abs(pVal - mEndSeal[pIndx].DrainHoles.Calc_AngStart()) <= modMain.gcEPS)
                            {
                                //....Most likely programmatically changed.
                                pTxtBox.ForeColor = Color.Blue;
                            }
                            else
                            {   //....Manually changed.
                                //........The drain holes array is not symmetric about the Casing SL vertical.

                                pVal = mEndSeal[pIndx].DrainHoles.AngBet_ULim_NonSym();
                                mLblDrainHoles_AngBet_ULim[pIndx].Text = modMain.ConvDoubleToStr(pVal, "#0");

                                pTxtBox.ForeColor = Color.Black;
                            }

                            CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndx);

                            //....Back Seal: Display the corresponding start angle.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{
                            //    mTxtDrainHoles_AngStart[1].Text = modMain.ConvDoubleToStr(mEndSeal[pIndx].DrainHoles.AngStart_OtherSide(), "#0.0");
                            //}

                            break;


                        case "txtDrainHoles_AngStart_Back":
                            //-----------------------------

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                pIndx = 1;

                                pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                mEndSeal[pIndx].DrainHoles.AngStart = pVal;

                                if (Math.Abs(pVal - mEndSeal[pIndx].DrainHoles.Calc_AngStart()) <= modMain.gcEPS)
                                {
                                    //....Most likely programmatically changed.
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {   //....Manually changed.
                                    //........The drain holes array is not symmetric about the Casing SL vertical.

                                    pVal = mEndSeal[pIndx].DrainHoles.AngBet_ULim_NonSym();
                                    mLblDrainHoles_AngBet_ULim[pIndx].Text = modMain.ConvDoubleToStr(pVal, "#0");

                                    pTxtBox.ForeColor = Color.Black;

                                    //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    pTxtBox.ForeColor = Color.Blue;
                                    //    pTxtBox.BackColor = mTxtDrainHoles_Count[pIndx].BackColor;
                                    //}
                                    //else
                                    //{
                                    //    pTxtBox.ForeColor = Color.Black;
                                    //}
                                }

                                CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndx);
                            }

                            break;


                        case "txtTempSensor_ExitHole_D_Front":
                            //--------------------------------
                            mEndSeal[0].TempSensor_D_ExitHole = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtTempSensor_ExitHole_D_Back":
                            //-------------------------------
                            mEndSeal[1].TempSensor_D_ExitHole = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_Thread_Depth_Front":
                            //------------------------------------
                            mEndSeal[0].WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_Thread_Depth_Back":
                            //-----------------------------------
                            mEndSeal[1].WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_Ang_Start_Front":
                            //---------------------------------
                            mEndSeal[0].WireClipHoles.AngStart = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_Ang_Start_Back":
                            //--------------------------------
                            mEndSeal[1].WireClipHoles.AngStart = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_AngOther1_Front":
                            //--------------------------------
                            mWireClipHole_AngOther[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[0].WireClipHoles.AngOther = mWireClipHole_AngOther;
                            break;


                        case "txtWireClipHole_AngOther1_Back":
                            //--------------------------------
                            mWireClipHole_AngOther[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[1].WireClipHoles.AngOther = mWireClipHole_AngOther;
                            break;


                        case "txtWireClipHole_AngOther2_Front":
                            //---------------------------------
                            mWireClipHole_AngOther[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[0].WireClipHoles.AngOther = mWireClipHole_AngOther;
                            break;


                        case "txtWireClipHole_AngOther2_Back":
                            //--------------------------------
                            mWireClipHole_AngOther[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                            mEndSeal[1].WireClipHoles.AngOther = mWireClipHole_AngOther;
                            break;


                        case "txtWireClipHole_DBC_Front":
                            //---------------------------
                            mEndSeal[0].WireClipHoles.DBC = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtWireClipHole_DBC_Back":
                            //--------------------------
                            mEndSeal[1].WireClipHoles.DBC = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;
                    }
                }


                //private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
                ////===============================================================
                //{
                //    mblnDrainHoles_Annulus_D_Front_ManuallyChanged = true;
                //}


                #region "Helper Routines:"
                //------------------------

                    private void SetTxtForeColor_L(TextBox TxtBox_In, Double ActulVal_In)
                    //====================================================================       
                    {
                        if (System.Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - ActulVal_In) <= modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;
                        }
                    }

                #endregion


            #endregion


            #region "COMBOBOX RELATED ROUTINE:"
            //--------------------------------

                private void ComboBox_SelectedIndex_Txt_Changed(object sender, EventArgs e)
                //========================================================================= 
                {
                    ComboBox pCmbBox = (ComboBox)sender;
                    int pIndex = 0;

                    Double pVal = 0.0;

                    //if (pCmbBox.Text != "")
                    //{
                    //    mBearing_Radial_FP.SL.Screw_Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 26MAR12                          
                    //    Populate_SL_Details(cmbSL_Screw_Spec_Type);
                    //}

                    switch (pCmbBox.Name)
                    {
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Front":
                            //=======================================
                            pIndex = 0;

                            pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                            mEndSeal[pIndex].DrainHoles.Annulus_Ratio_L_H = pVal;

                            if (!mblnDrainHoles_Annulus_D_ManuallyChanged)
                            {
                                pVal = mEndSeal[pIndex].DrainHoles.Annulus.D;
                                mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(pVal, "#0.000");
                            }

                            Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                      mCmbDrainHoles_AngBet[pIndex]);

                            //....Back Seal: Display the same.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //{                                
                            //    mCmbDrainHoles_Annulus_Ratio_L_H[1].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.Annulus.Ratio_L_H, "");                                
                            //}

                            break;


                        case "cmbDrainHoles_Annulus_Ratio_L_H_Back":
                            //====================================

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                pIndex = 1;

                                pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                                mEndSeal[pIndex].DrainHoles.Annulus_Ratio_L_H = pVal;

                                if (!mblnDrainHoles_Annulus_D_ManuallyChanged)
                                {
                                    pVal = mEndSeal[pIndex].DrainHoles.Annulus.D;
                                    mTxtDrainHoles_Annulus_D[pIndex].Text = modMain.ConvDoubleToStr(pVal, "#0.000");
                                }

                                Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                           mCmbDrainHoles_AngBet[pIndex]);
                            }

                            break;


                        case "cmbDrainHoles_D_Desig_Front":
                            //=============================
                            pIndex = 0;
                            mEndSeal[pIndex].DrainHoles.D_Desig = pCmbBox.Text;

                            mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndSeal[pIndex].DrainHoles.Count);
                            mTxtDrainHoles_Count[pIndex].ForeColor = Color.Blue;

                            txtDrainHoles_V_Front.Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.V(), "#0.000");
                          

                            Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                       mCmbDrainHoles_AngBet[pIndex]);
                      

                            //CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);           //PB 29JAN13. Not needed.

                            //....Back Seal: Display the same D_Desig.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_D_Desig[1].Text = mEndSeal[pIndex].DrainHoles.D_Desig;

                            break;


                        case "cmbDrainHoles_D_Desig_Back":
                            //============================

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                pIndex = 1;
                                mEndSeal[pIndex].DrainHoles.D_Desig = pCmbBox.Text;

                                mTxtDrainHoles_Count[pIndex].Text = modMain.ConvIntToStr(mEndSeal[pIndex].DrainHoles.Count);
                                mTxtDrainHoles_Count[pIndex].ForeColor = Color.Blue;

                                txtDrainHoles_V_Back.Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.V(), "#0.000");


                                Populate_DrainHolesAng_Bet(mEndSeal[pIndex], mLblDrainHoles_AngBet_LLim[pIndex], mLblDrainHoles_AngBet_ULim[pIndex],
                                                          mCmbDrainHoles_AngBet[pIndex]);
                            }

                            break;


                        case "cmbDrainHoles_AngBet_Front":
                            //============================                     
                            pIndex = 0;

                            pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                            mEndSeal[pIndex].DrainHoles.AngBet = pVal;

                            mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngStart, "#0.0");
                            mTxtDrainHoles_AngStart[pIndex].ForeColor = Color.Blue;

                            CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);

                            //....Back Seal: Display the same.
                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_AngBet[1].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngBet, "#0");

                            break;


                        case "cmbDrainHoles_AngBet_Back":
                            //===========================

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                pIndex = 1;

                                pVal = modMain.ConvTextToDouble(pCmbBox.Text);
                                mEndSeal[pIndex].DrainHoles.AngBet = pVal;

                                if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                                {
                                    mTxtDrainHoles_AngStart[pIndex].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngStart, "#0.0");
                                    mTxtDrainHoles_AngStart[pIndex].ForeColor = Color.Blue;
                                }

                                CheckAndAct_DrainHoles_Crossing_180BearingSL(pIndex);
                            }

                            break;


                        case "cmbDrainHoles_AngExit_Front":
                            //=============================
                            pIndex = 0;
                            mEndSeal[pIndex].DrainHoles.AngExit = modMain.ConvTextToDouble(pCmbBox.Text);

                            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            //    mCmbDrainHoles_AngExit[1].Text = modMain.ConvDoubleToStr(mEndSeal[pIndex].DrainHoles.AngExit, "#0");

                            break;


                        case "cmbDrainHoles_AngExit_Back":
                            //============================

                            if (modMain.gProject.Product.EndConfig[0].Type != clsEndConfig.eType.Seal)
                            {
                                pIndex = 1;
                                mEndSeal[pIndex].DrainHoles.AngExit = modMain.ConvTextToDouble(pCmbBox.Text);
                            }

                            break;


                        case "cmbWireClipHoles_Count_Front":
                            //=============================
                            mEndSeal[0].WireClipHoles.Count = modMain.ConvTextToInt(pCmbBox.Text);
                            DisplayOtherAngle(mEndSeal[0], lblWireClipHole_AngOther_Front, mTxtBoxWireClipHole_Front);
                            break;


                        case "cmbWireClipHoles_Count_Back":
                            //============================
                            mEndSeal[1].WireClipHoles.Count = modMain.ConvTextToInt(pCmbBox.Text);
                            DisplayOtherAngle(mEndSeal[1], lblWireClipHole_AngOther_Back, mTxtBoxWireClipHole_Back);
                            break;

                        case "cmbWireClipHoles_UnitSystem_Front":
                            //===================================
                             mEndSeal[0].Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 02JUL13                          
                             LoadWireClipHoles_D(mEndSeal[0], cmbWireClipHoles_Thread_Dia_Desig_Front);
                             break;

                        case "cmbWireClipHoles_UnitSystem_Back":
                            //==================================
                            mEndSeal[1].Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 02JUL13                          
                            LoadWireClipHoles_D(mEndSeal[1], cmbWireClipHoles_Thread_Dia_Desig_Back);
                            break;
                            
                        case "cmbWireClipHoles_Thread_Dia_Desig_Front":
                            //=======================================
                            mEndSeal[0].WireClipHoles.Screw_Spec.D_Desig = pCmbBox.Text;
                            PopulateWCThreadPitch(((clsSeal)modMain.gProject.Product.EndConfig[0]), cmbWireClipHoles_Thread_Dia_Desig_Front.Text, cmbWireClipHoles_Thread_Pitch_Front);
                            break;

                        case "cmbWireClipHoles_Thread_Dia_Desig_Back":
                            //======================================
                            mEndSeal[1].WireClipHoles.Screw_Spec.D_Desig = pCmbBox.Text;
                            PopulateWCThreadPitch(((clsSeal)modMain.gProject.Product.EndConfig[1]), cmbWireClipHoles_Thread_Dia_Desig_Back.Text, cmbWireClipHoles_Thread_Pitch_Back);
                            break;


                        case "cmbWireClipHoles_Thread_Pitch_Front":
                            //====================================
                            mEndSeal[0].WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                            break;


                        case "cmbWireClipHoles_Thread_Pitch_Back":
                            //===================================
                            mEndSeal[1].WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                            break;
                    }
                }


                private void ComboBox_MouseDown(object sender, MouseEventArgs e)
                //==============================================================
                {
                    ComboBox pcmbBox = (ComboBox)sender;
                    pcmbBox.ForeColor = Color.Black;

                    switch (pcmbBox.Name)
                    {
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Front":
                        case "cmbDrainHoles_Annulus_Ratio_L_H_Back":
                            //---------------------------------------
                            mblnDrainHoles_Annulus_Ratio_L_H_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_D_Front":
                            //-----------------------
                            mblnDrainHoles_D_Front_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_AngBet_Front":
                            //----------------------------
                            mblnDrainHoles_AngBet_Front_ManuallyChanged = true;
                            break;

                        case "cmbDrainHoles_AngExit_Front":
                            //-----------------------------
                            mblnDrainHoles_AngExit_Front_ManuallyChanged = true;
                            break;
                    }
                }


                // PB 29JAN13. It is difficult to implement this rule. may be reviewed later.
                //private void cmbDrainHoles_AngBet_DrawItem(object sender, DrawItemEventArgs e)
                ////============================================================================ 
                //{
                //    if (e.Index < 0) return;
                //    ComboBox pCmbBox = (ComboBox)sender;

                //    int pIndex = 0;

                //    switch (pCmbBox.Name)
                //    {
                //        case "cmbDrainHoles_AngBet_Front":
                //            //============================
                //            pIndex = 0;
                //            Double pAng_ULim_Front = Math.Floor(mEndSeal[pIndex].DrainHoles.AngBet_ULim_Sym());

                //            e.DrawBackground();
                //            Brush pBrush_Front = Brushes.Black;
                //            Double pDrainHoles_AngBet_Front = Convert.ToDouble(pCmbBox.Items[e.Index]);

                //            if (pDrainHoles_AngBet_Front >= pAng_ULim_Front)
                //            {
                //                pBrush_Front = Brushes.Orange;
                //            }

                //            e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                //                                  e.Font, pBrush_Front, e.Bounds, StringFormat.GenericDefault);

                //            e.DrawFocusRectangle();
                //            break;

                //        case "cmbDrainHoles_AngBet_Back":
                //            //===========================
                //            pIndex = 1;
                //            Double pAng_ULim_Back = Math.Floor(mEndSeal[pIndex].DrainHoles.AngBet_ULim_Sym());

                //            e.DrawBackground();
                //            Brush pBrush_Back = Brushes.Black;
                //            Double pDrainHoles_AngBet_Back = Convert.ToDouble(pCmbBox.Items[e.Index]);

                //            if (pDrainHoles_AngBet_Back >= pAng_ULim_Back)
                //            {
                //                pBrush_Back = Brushes.Orange;
                //            }

                //            e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                //                                  e.Font, pBrush_Back, e.Bounds, StringFormat.GenericDefault);

                //            e.DrawFocusRectangle();
                //            break;
                //    }
                //}


                #region "Helper Routines:"
                //------------------------

                    private void PopulateWCThreadPitch(clsSeal Seal_In, string Text_In, ComboBox CmbBox_In)
                    //======================================================================================
                    {
                        String pWHERE = "WHERE fldD_Desig = '" + Text_In + "'";
                        StringCollection pWC_ThreadPitch = new StringCollection();
                        modMain.gDB.PopulateStringCol(pWC_ThreadPitch, "tblManf_Screw", "fldPitch", pWHERE, true);

                        if (pWC_ThreadPitch.Count > 0)
                        {
                            CmbBox_In.Items.Clear();
                            for (int i = 0; i < pWC_ThreadPitch.Count; i++)
                            {
                                Double pVal = Convert.ToDouble(pWC_ThreadPitch[i]);
                                CmbBox_In.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.000"));
                            }
                        }

                        if (CmbBox_In.Items.Count > 0)
                        {
                            if (Seal_In.WireClipHoles.Screw_Spec.Pitch > modMain.gcEPS)
                            {
                                if (CmbBox_In.Items.Contains(Seal_In.WireClipHoles.Screw_Spec.Pitch.ToString("#0.000")))
                                    CmbBox_In.SelectedIndex = CmbBox_In.Items.IndexOf(Seal_In.WireClipHoles.Screw_Spec.Pitch.ToString("#0.000"));
                                else
                                    CmbBox_In.SelectedIndex = 0;
                            }
                            else
                                CmbBox_In.SelectedIndex = 0;
                        }
                    }

                #endregion


            #endregion


            #region "COMMAND BUTTON RELATED ROUTINE:"
            //---------------------------------------

                private void cmdPrint_Click(object sender, EventArgs e)
                //======================================================        
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
                    //.....Validate C'Bore Depth.
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        {
                            if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Front, mEndSeal[0], tbEndSealDesignDetails_Front))
                                return;
                        }

                        if (((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.Count != 1)        
                        {
                            ////.....Validate Angle Between.
                            //if (!ValidateAngBet(mCmbDrainHoles_AngBet[0], mEndSeal[0], tbEndSealDesignDetails_Front))
                            //    return;

                            ////.....Validate Angle Start.
                            //if (!ValidateAngStart())
                            //    return;
                        }

                        //BG 28MAR13. As per HK's instruction in email dated 27MAR13.
                        //if (!ValidateExitHoleDia(txtTempSensor_D_ExitHole_Front, tbEndSealDesignDetails_Front)) 
                        //    return;
                    }


                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        {

                            if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Back, mEndSeal[1], tbEndSealDesignDetails_Back))
                                return;
                        }

                        if (((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count != 1)        
                        {
                            ////.....Validate Angle Between.
                            //if (!ValidateAngBet(mCmbDrainHoles_AngBet[1], mEndSeal[1], tbEndSealDesignDetails_Back))
                            //    return;

                            ////.....Validate Angle Start.
                            //if (!ValidateAngStart())
                            //    return;
                        }

                        //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                        //if (!ValidateExitHoleDia(txtTempSensor_D_ExitHole_Back, tbEndSealDesignDetails_Back)) 
                        //    return;
                    }

                    SaveData();
                   
                    this.Close();

                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                    }
                }

                
                private void SaveData()
                //======================    
                {
                    int pIndex = 0;
                    
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        pIndex = 0;

                        //  FRONT
                        //  -----
                        //
                        modMain.gProject.Product.EndConfig[0].L = modMain.ConvTextToDouble(txtL_Front.Text);

                        //  Mounting Hole
                        //  --------------                      
                        if (optMountHoles_Type_CBore_Front.Checked)
                        {
                            modMain.gProject.Product.EndConfig[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;
                            modMain.gProject.Product.EndConfig[0].MountHoles.Depth_CBore = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text);
                        }

                        else if (optMountHoles_Type_Thru_Front.Checked)
                            modMain.gProject.Product.EndConfig[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                        else if (optMountHoles_Type_Thread_Front.Checked)
                        {
                            modMain.gProject.Product.EndConfig[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;
                            
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Front.Checked;

                            if (chkMountHoles_Thread_Thru_Front.Checked)
                                ((clsSeal)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                   ((clsSeal)modMain.gProject.Product.EndConfig[0]).L;
                            else
                                ((clsSeal)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                    modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Front.Text);
                        }                       


                        //  Drain Hole
                        //  ------------ 
                        //....Annulus
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Annulus_Ratio_L_H = modMain.ConvTextToDouble(mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Annulus_D = modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text);

                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);

                        //....Angle
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngBet = modMain.ConvTextToDouble(mCmbDrainHoles_AngBet[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngStart = modMain.ConvTextToDouble(mTxtDrainHoles_AngStart[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngExit = modMain.ConvTextToDouble(mCmbDrainHoles_AngExit[pIndex].Text);


                        //  Temp. Sensor
                        //  -------------
                        ((clsSeal)modMain.gProject.Product.EndConfig[0]).TempSensor_D_ExitHole = modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Front.Text);


                        //  Wire Clip Holes
                        //  ----------------
                        ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Exists = chkWireClipHoles_Front.Checked;

                        if (((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Exists)
                        {
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Count = modMain.ConvTextToInt(cmbWireClipHoles_Count_Front.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.D_Desig = cmbWireClipHoles_Thread_Dia_Desig_Front.Text;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbWireClipHoles_Thread_Pitch_Front.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Front.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngStart = modMain.ConvTextToDouble(txtWireClipHoles_AngStart_Front.Text);

                            Double[] pWireClipHole_AngOther = new Double[5];
                            pWireClipHole_AngOther[0] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther1_Front.Text);
                            pWireClipHole_AngOther[1] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther2_Front.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.DBC = modMain.ConvTextToDouble(txtWireClipHoles_DBC_Front.Text);
                            
                            if (cmbWireClipHoles_UnitSystem_Front.Text != "")
                                ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Unit.System =
                                                                (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbWireClipHoles_UnitSystem_Front.Text);          //BG 03JUL13

                        }
                        else
                        {
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Count = 0;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.D_Desig = "";
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Screw_Spec.Pitch = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.ThreadDepth = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngStart = 0.0F;

                            Double[] pWireClipHole_AngOther = new Double[5];
                            pWireClipHole_AngOther[0] = 0.0F;
                            pWireClipHole_AngOther[1] = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.DBC = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[0]).WireClipHoles.Unit.System = modMain.gProject.Product.Unit.System;
                        }
                    }

                    
                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        pIndex = 1;
                        //  BACK
                        //  -----
                        //
                        modMain.gProject.Product.EndConfig[1].L = modMain.ConvTextToDouble(txtL_Back.Text);
                        
                        //  Mounting Hole
                        //  --------------
                        if (optMountHoles_Type_CBore_Back.Checked)
                        {
                            modMain.gProject.Product.EndConfig[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;
                            modMain.gProject.Product.EndConfig[1].MountHoles.Depth_CBore = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text);
                        }

                        else if (optMountHoles_Type_Thru_Back.Checked)
                            modMain.gProject.Product.EndConfig[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                        else if (optMountHoles_Type_Thread_Back.Checked)
                        {
                            modMain.gProject.Product.EndConfig[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;
                            
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Back.Checked;

                            if (chkMountHoles_Thread_Thru_Back.Checked)
                                //((clsSeal)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                //                                                                   ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).L;
                                ((clsSeal)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                   ((clsSeal)modMain.gProject.Product.EndConfig[1]).L;//SG 28FEB13
                            else
                                ((clsSeal)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                    modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Back.Text);
                        }


                        //  Drain Hole
                        //  ------------
                        //....Annulus
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Annulus_Ratio_L_H = modMain.ConvTextToDouble(mCmbDrainHoles_Annulus_Ratio_L_H[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Annulus_D = modMain.ConvTextToDouble(mTxtDrainHoles_Annulus_D[pIndex].Text);

                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.D_Desig = mCmbDrainHoles_D_Desig[pIndex].Text;
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.Count = modMain.ConvTextToInt(mTxtDrainHoles_Count[pIndex].Text);

                        //....Angle
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngBet = modMain.ConvTextToDouble(mCmbDrainHoles_AngBet[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngStart = modMain.ConvTextToDouble(mTxtDrainHoles_AngStart[pIndex].Text);
                        ((clsSeal)modMain.gProject.Product.EndConfig[pIndex]).DrainHoles.AngExit = modMain.ConvTextToDouble(mCmbDrainHoles_AngExit[pIndex].Text);

                        //  Temp. Sensor
                        //  -------------
                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).TempSensor_D_ExitHole = modMain.ConvTextToDouble(txtTempSensor_D_ExitHole_Back.Text);

                        //  Wire Clip Holes
                        //  ----------------
                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Exists = chkWireClipHoles_Back.Checked;

                        if (((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Exists)
                        {
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Count = modMain.ConvTextToInt(cmbWireClipHoles_Count_Back.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.D_Desig = cmbWireClipHoles_Thread_Dia_Desig_Back.Text;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbWireClipHoles_Thread_Pitch_Back.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.ThreadDepth = modMain.ConvTextToDouble(txtWireClipHoles_Thread_Depth_Back.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngStart = modMain.ConvTextToDouble(txtWireClipHoles_AngStart_Back.Text);

                            Double[] pWireClipHole_AngOther = new Double[5];
                            pWireClipHole_AngOther[0] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther1_Back.Text);
                            pWireClipHole_AngOther[1] = modMain.ConvTextToDouble(txtWireClipHoles_AngOther2_Back.Text);
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.DBC = modMain.ConvTextToDouble(txtWireClipHoles_DBC_Back.Text);

                            if (cmbWireClipHoles_UnitSystem_Back.Text != "")
                                ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Unit.System =
                                                                (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbWireClipHoles_UnitSystem_Back.Text);          //BG 03JUL13
                        }
                        else
                        {
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Count = 0;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.D_Desig = "";
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Screw_Spec.Pitch = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.ThreadDepth = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngStart = 0.0F;

                            Double[] pWireClipHole_AngOther = new Double[5];
                            pWireClipHole_AngOther[0] = 0.0F;
                            pWireClipHole_AngOther[1] = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.AngOther = pWireClipHole_AngOther;

                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.DBC = 0.0F;
                            ((clsSeal)modMain.gProject.Product.EndConfig[1]).WireClipHoles.Unit.System = modMain.gProject.Product.Unit.System;
                        }
                    }
                  
                }

              
                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================    
                {
                    this.Hide();
                }

            #endregion

        #endregion
        

        #region "UTILITY ROUTINE:"
        //************************

            private void Populate_DrainHolesAng_Bet (clsSeal Seal_In, Label lblAng_Bet_LLim_In, Label lblAng_Bet_ULim_In, ComboBox CmbBox_In)
            //================================================================================================================================   
            {
                Double pAng_LLim = Math.Ceiling((Double)Seal_In.DrainHoles.AngBet_LLim());
                lblAng_Bet_LLim_In.Text = modMain.ConvDoubleToStr(pAng_LLim, "#0");


                Double pAng_ULim = 0.0;

                if (Seal_In.DrainHoles.Sym_CasingSL_Vert())
                {
                    pAng_ULim = Math.Floor((Double)Seal_In.DrainHoles.AngBet_ULim_Sym());  
                }
                else
                {
                    pAng_ULim = Math.Floor((Double)Seal_In.DrainHoles.AngBet_ULim_NonSym());
                }
               
             
                //....Make Ang_Bet list items even nos.
                //
                Double pAng_LLim_Even = 0, pAng_ULim_Even = 0;

                if (pAng_LLim % 2 != 0.0F)
                    pAng_LLim_Even = pAng_LLim + 1.0F;
                else
                    pAng_LLim_Even = pAng_LLim;


                if (pAng_ULim % 2 != 0.0F)
                    pAng_ULim_Even = pAng_ULim - 1.0F;
                else
                    pAng_ULim_Even = pAng_ULim;


                //....Calculate the AngStart_OtherSide corresponding to AngBet = AngBet_LLim to see if 
                //........it is < 0 i.e. AngEnd crossing the 180 deg. Bearing S/L.
                //                
                clsSeal pSeal = (clsSeal)Seal_In.Clone();                       //....Create a local Seal object.
                pSeal.DrainHoles.AngBet = pSeal.DrainHoles.AngBet_LLim();
       

                CmbBox_In.Items.Clear();

                if (pAng_LLim_Even < pAng_ULim_Even)
                {
                    //....Usual case:
                    //
                    lblAng_Bet_ULim_In.Text = modMain.ConvDoubleToStr(pAng_ULim, "#0");
                    lblAng_Bet_ULim_In.ForeColor = Color.Blue;

                    //for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Even; i = i + 2)
                    //    CmbBox_In.Items.Add(i);
                }

                else if (pSeal.DrainHoles.AngStart_OtherSide() < 0 || pAng_LLim_Even >= pAng_ULim_Even)
                {
                    //....Unusual case:
                    //....The following fix has been done to accommodate request by HK, KMC, 17OCT12 
                    //........and telephone discussion with PB on 17OCT12.
                    //
                    lblAng_Bet_ULim_In.Text = "-";
                    lblAng_Bet_ULim_In.ForeColor = Color.Orange;
   
                    //int pAng_ULim_Selected = Convert.ToInt32(pAng_LLim_Even) + 10 * 2;      //.....Upper limit is arbitrarily chosen as 
                    //                                                                        //........20 deg more than the lower limit. 
                    //for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Selected; i = i + 2)
                    //    CmbBox_In.Items.Add(i);
                }


                //....Populate the combo box.
                int pAng_ULim_Selected = Convert.ToInt32(pAng_LLim_Even) + 10 * 2;      //.....Upper limit is arbitrarily chosen as 
                                                                                        //........20 deg more than the lower limit. 
                for (int i = Convert.ToInt32(pAng_LLim_Even); i <= pAng_ULim_Selected; i = i + 2)
                    CmbBox_In.Items.Add(i);


                if (CmbBox_In.Items.Count > 0)
                {
                    CmbBox_In.SelectedIndex = 0;
                }
            }


            //PB 14JAN13.
            //
            //private Boolean IsAnyDrainHolesOnBearingSL (int Indx_In)          //PB 11JAN12. This routine may be moved to clsSeal.
            ////======================================================         
            //{
            //    Boolean pbln = false;
            //    Double pAngi = 0.0F;
            //    Double pAng_LLim = 0.0F;

            //    for (int i = 1; i <= mEndSeal[Indx_In].DrainHoles.Count; i++)
            //    {
            //        if (Indx_In == 0)
            //        {
            //            pAngi = mEndSeal[Indx_In].DrainHoles.AngStart + ((i - 1) * mEndSeal[Indx_In].DrainHoles.AngBet);
            //        }

            //        else
            //        {
            //            pAngi = mEndSeal[Indx_In].DrainHoles.AngStart_OtherSide + ((i - 1) * mEndSeal[Indx_In].DrainHoles.AngBet);
            //        }

            //        pAng_LLim = Math.Ceiling(mEndSeal[Indx_In].DrainHoles.AngBet_LLim());
  
            //        if (pAngi > (180 - (0.5 * pAng_LLim)) && pAngi < (180 + (0.5 * pAng_LLim)))
            //        {
            //            pbln = true;
            //            break;
            //        }
            //    }

            //    return pbln;
            //}


            private void CheckAndAct_DrainHoles_Crossing_180BearingSL(int Index_In)
            //======================================================================
            {
                if (mEndSeal[Index_In].DrainHoles.AngStart_OtherSide () > 0)
                {
                    //....Usual case: Drain holes array does not cross the 180 deg Bearing S/L.
                    //
                    mTxtDrainHoles_Count[Index_In].Text = Convert.ToString(mEndSeal[Index_In].DrainHoles.Calc_Count());
                    mTxtDrainHoles_Count[Index_In].ForeColor = Color.Blue;
                    mLblDrainHoles_Notes[Index_In].Visible = false;
                }

                else
                {
                    //....Unusual case: Drain holes array crosses the 180 deg Bearing S/L.
                    //
                    mEndSeal[Index_In].DrainHoles.Count = mEndSeal[Index_In].DrainHoles.Calc_Count() + 1;
                    mTxtDrainHoles_Count[Index_In].Text = Convert.ToString(mEndSeal[Index_In].DrainHoles.Count);
                    mTxtDrainHoles_Count[Index_In].ForeColor = Color.Orange;

                    mLblDrainHoles_Notes[Index_In].Visible = true;
                    mLblDrainHoles_Notes[Index_In].ForeColor = Color.Orange;
                }
            }


            #region "VALIDATION FOR NULL OBJECT:"
            //----------------------------------

                public bool IsMountThread_NULL()
                //==============================
                {
                    bool pBln = false;

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type == ""
                            ||((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig == ""
                            ||((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Pitch == 0.0F
                            ||((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.L == 0.0F
                            ||((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat == "")
                        {
                            pBln = true;
                        }
                    }

                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Type == ""
                          || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig == ""
                          || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Pitch == 0.0F
                          || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.L == 0.0F
                          || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Mat == "")
                        {
                            pBln = true;
                        }
                    }

                    return pBln;
                }
                       

                public bool IsFlow_GPM_NULL()
                //============================
                {
                    bool pBln = false;

                    //if (Seal_In.DrainHole.Flow_gpm == 0.0F)
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd_gpm == 0.0F)
                        pBln = true;

                    return pBln;
                  
                }


                public bool IsSealDO_Null()
                //===========================
                {
                    bool pBln = false;

                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].D_Finish < modMain.gcEPS)
                        {
                            pBln = true;
                        }
                    }
                    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].D_Finish < modMain.gcEPS)
                        {
                            pBln = true;
                        }
                    }
                    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[i].D_Finish < modMain.gcEPS)
                            {
                                pBln = true;
                            }
                        }
                    }

                    return pBln;
                }


                public bool IsDShaft_NULL()
                //=========================
                {
                    bool pBln = false;

                    //if (modMain.gRadialBearing.DShaft() == 0.0F)
                    if (((clsBearing_Radial_FP) modMain.gProject.Product.Bearing).DShaft() == 0.0F)
                        pBln = true;

                    return pBln;
                }

            #endregion

        #endregion


        #region "VALIDATION ROUTINE:"
        //***************************

            private bool ValidateCBoreDepth(TextBox TxtBox_In, clsSeal Seal_In, TabControl TbCtrl_In)
            //========================================================================================
            {
                Double pCBoreDepth = modMain.ConvTextToDouble(TxtBox_In.Text);

                if (pCBoreDepth <= Seal_In.MountHoles.CBore_Depth_UpperLimit()
                    && pCBoreDepth >= Seal_In.MountHoles.CBore_Depth_LowerLimit())
                    return true;

                String pMSg = "Enter value between Lower Limit: " +
                               modMain.ConvDoubleToStr(Seal_In.MountHoles.CBore_Depth_LowerLimit(), "#0.000") +
                               " to Upper Limit: " +
                               modMain.ConvDoubleToStr(Seal_In.MountHoles.CBore_Depth_UpperLimit(), "#0.000") + ".";

                String pCaption = "CBore Depth Data Input Error";
                MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                TbCtrl_In.SelectedIndex = 0;
                TxtBox_In.Focus();

                return false;
            }

          
            private bool ValidateAngBet(ComboBox CmbBox_In, clsSeal Seal_In, TabControl tbCtrl_In)
            //=====================================================================================
            {
                Double pAngBet = modMain.ConvTextToDouble(CmbBox_In.Text);
                //SB 15JAN10
                Double pAng_LLim = (Double)Math.Ceiling((Double)Seal_In.DrainHoles.AngBet_LLim());
                Double pAng_ULim = (Double)Math.Floor((Double)Seal_In.DrainHoles.AngBet_ULim_Sym());

                if (pAngBet >= pAng_LLim && pAngBet <= pAng_ULim)
                    return true;

                String pMSg = "Enter value between Lower Limit: " + pAng_LLim + " to Upper Limit: " + pAng_ULim;
                String pCaption = "Angle Between Data Input Error";             //SB 04AUG09
                MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                tbCtrl_In.SelectedIndex = 1;
                if (CmbBox_In.Items.Count > 0)
                    CmbBox_In.SelectedIndex = 0;
                CmbBox_In.Focus();

                return false;
            }

           
            public bool ValidateAngStart()
            //=============================
            {
                clsSeal pTempSeal;// = new clsSeal();                

                int pRet = 0;

                String pMSg = "The given input value of Angle Start may not" +
                               System.Environment.NewLine +
                               "gurantee symmetrical positioning of the drain holes" +
                               System.Environment.NewLine +
                               "about the casing vertical." +
                               "Do you want to proceed?";

                String pCaption = "Angle Start Data Input Warning";

                //....Answer = YES
                int pAnsY = 6;


                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                {
                    pTempSeal = (clsSeal)mEndSeal[0].Clone();

                    if ((modMain.CompareVar(mEndSeal[0].DrainHoles.AngBet, ((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.AngBet, 0, pRet) > 0)
                        || (modMain.CompareVar(mEndSeal[0].DrainHoles.AngStart, ((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.AngStart, 0, pRet) > 0))   //SB 10JUL09
                    {
                        pTempSeal.DrainHoles.AngBet = mEndSeal[0].DrainHoles.AngBet;
                        pTempSeal.DrainHoles.Calc_AngStart();
                        //mEndSeal.Calc_DrainHole_Angle_Start_FrontSeal();

                        pRet = 0;
                        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart, mEndSeal[0].DrainHoles.AngStart, 0, pRet) > 0)
                        {
                            int pAns = (int)MessageBox.Show(pMSg, pCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            bool pbln = false;

                            if (pAns == pAnsY)
                                pbln = true;
                            else
                            {
                                pbln = false;
                                tbEndSealDesignDetails_Front.SelectedIndex = 1;
                                mTxtDrainHoles_AngStart[0].Text = modMain.ConvDoubleToStr(pTempSeal.DrainHoles.AngStart, "#0.0");
                                mTxtDrainHoles_AngStart[0].ForeColor = Color.Blue;
                                mTxtDrainHoles_AngStart[0].Focus();
                            }

                            pTempSeal = null;
                            return pbln;
                        }
                    }
                }

                pRet = 0;
                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    pTempSeal = (clsSeal)mEndSeal[1].Clone();

                    if ((modMain.CompareVar(mEndSeal[1].DrainHoles.AngBet, ((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.AngBet, 0, pRet) > 0)
                    || (modMain.CompareVar(mEndSeal[1].DrainHoles.AngStart, ((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.AngStart, 0, pRet) > 0))   
                    {
                        pTempSeal.DrainHoles.AngBet = mEndSeal[1].DrainHoles.AngBet;
                        pTempSeal.DrainHoles.Calc_AngStart();
                        //mEndSeal.Calc_DrainHole_Angle_Start_FrontSeal();

                        pRet = 0;
                        if (modMain.CompareVar(pTempSeal.DrainHoles.AngStart, mEndSeal[1].DrainHoles.AngStart, 0, pRet) > 0)
                        {                            
                            int pAns = (int)MessageBox.Show(pMSg, pCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            bool pbln = false;

                            if (pAns == pAnsY)
                                pbln = true;
                            else
                            {
                                pbln = false;
                                tbEndSealDesignDetails_Back.SelectedIndex = 1;
                                mTxtDrainHoles_AngStart[1].Text = modMain.ConvDoubleToStr(pTempSeal.DrainHoles.AngStart, "#0");
                                mTxtDrainHoles_AngStart[1].ForeColor = Color.Blue;
                                mTxtDrainHoles_AngStart[1].Focus();
                            }

                            pTempSeal = null;
                            return pbln;
                        }
                    }
                }
             
                return true;
            }

                
            private bool ValidateExitHoleDia(TextBox TxtBox_In, TabControl tbCtrl_In)
            //=======================================================================              
            {
                Double pExitHoleDia = modMain.ConvTextToDouble(TxtBox_In.Text);

                if (pExitHoleDia <= ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D)
                    return true;

                String pMSg = "The value should be less than the" + "\"\"" + " Temp Sensor Hole Dia : " + "\"\"" +
                               modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D, 
                               modMain.gUnit.MFormat) + ".";         


                String pCaption = "Exit Hole Dia Data Input Error";
                MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                tbCtrl_In.SelectedIndex = 2;
                TxtBox_In.Focus();

                return false;
            }


        #endregion

            

    }
}
