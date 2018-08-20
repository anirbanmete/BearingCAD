
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP                   '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  27JUL18                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;

namespace BearingCAD21
{
     [Serializable]
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
         #region "NAMED CONSTANTS:"
        //========================

            //DESIGN PARAMETERS:
            //------------------
            //....EDM Relief (used in Main Class & clsOilInlet).
            //private const Double mc_DESIGN_EDM_RELIEF = 0.010D;       //BG 06DEC12

            //....Min. End Config. Depth (used in Main Class).
            private const Double mc_DEPTH_END_CONFIG_MIN = 0.125D;
           

            //....Fixture Hole Depth (used in Main Class & clsFlange).
            private const Double mc_DEPTH_FIXTURE_HOLE = 0.1875D;

            //....Total Length Limit for the End Configs mounting hole to be "Go Thru'". (Not used internally).
            private Double mc_L_MAX_GO_THRU = 4D;                                   //....4 in (100 mm). 

            //....Pad length threshold beyond which a 2nd set of Orifices is usually needed. 
            //private const Double mcPAD_L_THRESHOLD = 3.94D;                            //....4 in (100 mm).  //PB 13AUG12
            private const Double mcPAD_L_THRESHOLD = 3.937D;                            //....4 in (100 mm).  //BG 22MAR13
      
            private const Double mc_DESIGN_TOL_END_CONFIG_D_FIT = 0.0005D;  //....End Config. Fit Dia Tol. (Symmetric).  Not used internally.


            //OTHERS:
            //------
            //private const Double mc_MAT_LINING_T = 0.025F;                //....Not reqd. any more. To be retrieved from DB. 18APR12.
            //....Fixture Others Angle Count 
            private const int mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX = 7;
              
        #endregion


        #region "ENUMERATION TYPES:"
        //==========================
            public enum eFaceID { Front, Back, Both };

        #endregion


        #region "MEMBER VARIABLES:"
        //========================

            protected clsProduct mCurrentProduct;
            private bool mSplitConfig;

            #region "Diameters:"

                //....Min.= 0 & Max.= 1:
                //  
                private Double[] mDShaft_Range = new Double[2];
                private Double[] mDFit_Range   = new Double[2];           //....Bearing Fit Dia  
                private Double[] mDSet_Range   = new Double[2];                      
                private Double[] mDPad_Range   = new Double[2];

            #endregion


            #region "Lengths:"

                private Double mL;
                private double[] mDepth_EndConfig = new Double[2];
                private Double mDimStart_FrontFace;                     //....From "Turning" dwg.  
                //private Double[] mEDM_Relief = new Double[2];           //BG 27APR12
                     
            #endregion


            #region "Materials:"
                private clsMaterial mMat = new clsMaterial();
                private Double mLiningT;                                //....Not included in clsMaterial.
            #endregion


            #region "Member Class Objects:"
                private clsPerformData mPerformData;

                private clsPad mPad;
                private clsFlexurePivot mFlexurePivot;
                private clsOilInlet mOilInlet;
            #endregion           


            #region "Detailed Design Data:"

                #region "Member Class Objects:"

                    private clsMillRelief mMillRelief;
                    private clsSL mSL;
                    private clsFlange mFlange;
                    private clsAntiRotPin mAntiRotPin;
                    private clsMount mMount;
                    private clsTempSensor mTempSensor;
                    private clsEDM_Pad mEDM_Pad;      

                #endregion

            #endregion

        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //==============================

            #region "NAMED CONSTANTS:"

                public Double PAD_L_THRESHOLD               //PB 13AUG12
                //===========================
                {
                    get {return (mcPAD_L_THRESHOLD);}
                } 

                public Double DEPTH_END_CONFIG_MIN
                //=================================
                {
                    get { return mc_DEPTH_END_CONFIG_MIN; }
                }

                //public Double DESIGN_EDM_RELIEF           //BG 06DEC12
                ////=============================    
                //{
                //    get { return mc_DESIGN_EDM_RELIEF; }
                //}

                public int COUNT_MOUNT_HOLES_ANG_OTHER_MAX      //SG 21AUG12
                {
                    get { return mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX; }
                } 

            #endregion


            #region "Split Configuration:"

                public bool SplitConfig
                {
                    get { return mSplitConfig; }
                    set { mSplitConfig = value; }
                }
            #endregion

           
            #region "Diameters:"

                //....Shaft Dia:
                public Double[] DShaft_Range
                {
                    get { return mDShaft_Range; }
                    set
                    {
                        mDShaft_Range = value;
                        //mPad.T_Pivot = mPad.TDef();         //BG 25MAR13
                    }
                }

                //....Fit Dia:
                public Double[] DFit_Range
                {
                    get { return mDFit_Range; }
                    set { mDFit_Range = value; }
                }

                //.... Pad Dia:             
                public Double[] DPad_Range
                {
                    get { return mDPad_Range; }
                    set { mDPad_Range = value; }
                }

                //.... Set Dia:            
                public Double[] DSet_Range
                {
                    get { return mDSet_Range; }
                    set { mDSet_Range = value; }
                }

            #endregion


            #region "Lengths:"

                public Double L
                {
                    get { return mL; }
                    set { mL = value; }
                }


                #region "Depth - End Configs.:"

                    public double[] Depth_EndConfig
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mDepth_EndConfig[i] < modMain.gcEPS)
                                {
                                    mDepth_EndConfig[i] = Calc_Depth_EndConfig();
                                }
                            }

                            return mDepth_EndConfig;
                        }

                        //set               //This set syntax doesn't work. Validate data in the form.
                        //{
                        //    for (int i = 0; i < 2; i++)
                        //    {
                        //        mDepth_EndConfig[i] = Validate_Depth_EndConfig(value[i]);
                        //    }
                        //}
                    }

                #endregion


                //....Start Dimension from Front Face.
                public Double DimStart_FrontFace
                {
                    get
                    {
                        if (mDimStart_FrontFace < modMain.gcEPS)
                            return Calc_Dim_Start_FrontFace();
                        else
                            return mDimStart_FrontFace;
                    }

                    set { mDimStart_FrontFace = value; }
                }

                //#region "EDM Relief:"     //BG 06DEC12

                //    public Double[] EDM_Relief          //BG 27APR12
                //    {
                //        get
                //        {
                //            for (int i = 0; i < 2; i++)
                //            {
                //                if (mEDM_Relief[i] < modMain.gcEPS)
                //                {
                //                    mEDM_Relief[i] = mc_DESIGN_EDM_RELIEF;
                //                }
                //            }
                //            return mEDM_Relief;
                //        }
                //        //set { mEDM_Relief = value; }
                //    }
                //#endregion

            #endregion


            #region "Materials:"

                public clsMaterial Mat
                {
                    get { return mMat; }
                    set { mMat = value; }
                }

                //.... Linning Thickness.
                public Double LiningT
                {
                    get {
                        if (mLiningT < modMain.gcEPS)
                            mLiningT = Mat_Lining_T();
                         return mLiningT;
                    }
                 
                    set { mLiningT = value; }
                }

            #endregion


            #region "PAD:"
            //============  

                public clsPad Pad
                {
                    get { return mPad; }
                    set { mPad = value; }
                }

            #endregion


            #region "FLEXURE PIVOT:"
            // ====================

                public clsFlexurePivot FlexurePivot
                {
                    get { return mFlexurePivot; }
                    set { mFlexurePivot = value; }
                }

            #endregion


            #region "OIL INLET:"

                public clsOilInlet OilInlet
                {
                    get { return mOilInlet; }
                    set { mOilInlet = value; }
                }

            #endregion


            #region "MILL RELIEF:"
            //===================

                public clsMillRelief MillRelief
                {
                    get { return mMillRelief; }
                    set { mMillRelief = value; }
                }

            #endregion


            #region "SPLIT LINE HARDWARE:"
            //===========================

                public clsSL SL
                {
                    get { return mSL; }
                    set { mSL = value; }
                }
            #endregion


            #region "FLANGE:"
            //===============

                public clsFlange Flange
                {
                    get { return mFlange; }
                    set { mFlange = value; }
                }

            #endregion


            #region "ANTI ROTATION PIN:"
            //==========================

                public clsAntiRotPin AntiRotPin
                {
                    get { return mAntiRotPin; }
                    set { mAntiRotPin = value; }
                }

            #endregion


            #region "MOUNTING DETAILS:"
            //========================                          
  
                public clsMount Mount
                {
                    get { return mMount; }
                    set { mMount = value; }
                }

            #endregion


            #region "TEMP SENSOR HOLES:"
            //=========================

                public clsTempSensor TempSensor
                {
                    get { return mTempSensor; }
                    set { mTempSensor = value; }
                }

            #endregion


            #region "EDM Pad:"
            //===============

                public clsEDM_Pad EDM_Pad
                {
                    get { return mEDM_Pad; }
                    set { mEDM_Pad = value; }
                }

            #endregion


            #region "PERFORMANCE DATA:"
            //========================

                public clsPerformData PerformData
                {
                    get { return mPerformData; }
                    set { mPerformData = value; }
                }

            #endregion

        #endregion


        #region "CONSTRUCTOR:"

                public clsBearing_Radial_FP(clsUnit.eSystem UnitSystem_In, eType Type_In, eDesign Design_In, clsDB DB_In, clsProduct CurrentProduct_In)
                    : base(UnitSystem_In, Type_In, Design_In, DB_In)
                //==========================================================================================================================================
                {

                    //....Instantiate member class objects:
                    //

                    mPad = new clsPad(this);
                    mFlexurePivot = new clsFlexurePivot();
                    mOilInlet = new clsOilInlet(this);
                    mFlange = new clsFlange(this);
                    mTempSensor = new clsTempSensor(this);
                    mMillRelief = new clsMillRelief(this);
                    mSL = new clsSL(this);
                    mAntiRotPin = new clsAntiRotPin(this);
                    mEDM_Pad = new clsEDM_Pad(this);
                    mMount = new clsMount(this);
                    mPerformData = new clsPerformData();


                    //....Initialize: 
                    mSplitConfig = true;

                    //........Material.
                    mMat.Base = "Steel 4340";
                    mMat.LiningExists = true;
                    mMat.Lining = "Babbitt";

                    mCurrentProduct = CurrentProduct_In;
                }


        #endregion


        #region "CLASS METHODS:"
        //*********************

            #region "REF. / DEPENDENT VARIABLES:"

                #region "LENGTHS:"

                    public Double Calc_Depth_EndConfig()
                    //----------------------------------
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. CZ and DA.  
                        //........Assumes equal depth on both sides as a starting estimate. 
                        double pDepth = 0.0F;
                        //pDepth = (mL - (mPad.L + 2 * mc_DESIGN_EDM_RELIEF)) * 0.5F;                               //....V1.1 - Fixed EDM relief on either side.
                        //pDepth = (mL - (mPad.L + mEDM_Relief[0] + mEDM_Relief[1])) * 0.5F;                        //....V2.0 - Variable EDM relief on either side.      
                        pDepth = (mL - (mPad.L + mMillRelief.EDM_Relief[0] + mMillRelief.EDM_Relief[1])) * 0.5F;    //....V2.0 - Variable EDM relief on either side.  
                        return pDepth;
                    }


                    private Double Calc_Dim_Start_FrontFace()
                   //---------------------------------------
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. CQ.
                        Double pDepthF = mDepth_EndConfig [0];

                        if ((pDepthF + 0.01) <= (0.244 - 0.1))
                            return mc_DEPTH_FIXTURE_HOLE;
                        else
                            return 0.1005F;
                    }

                    
                    public Double Loc_Fixture_B()                       
                    //---------------------------                     
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. CM.  
                        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        Double pApprox = 0.0F;
                        pApprox = 0.25 * (mMount.DFit_EndConfig(0) + DSet());             //....Always from the Front face. 

                        Double pLoc_Fixture_B = 0.0F;

                        var pProject = (from pRec in pBearingDBEntities.tblManf_Fixture_Bearing_Splitting orderby pRec.fldB_Hdist_Nom ascending select pRec.fldB_Hdist_Nom).ToList();

                        if (pProject.Count > 0)
                        {
                            for (int i = 0; i < pProject.Count; i++)
                            {
                                double pHDist = mDB.CheckDBDouble(pProject[i]);
                                if (pHDist > pApprox)
                                {
                                    pLoc_Fixture_B = pHDist;
                                    break;
                                }
                            }
                        }
                        return pLoc_Fixture_B;
                    }


                    public Double Mat_Lining_T()
                    //==========================            
                    {
                        //....Retrieve from database.
                        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        Double pT = 0.0f;

                        Decimal pDSet = (Decimal)DSet();
                        var pProject = (from pRec in pBearingDBEntities.tblDesign_Lining where pRec.fldDSet_LLim <= pDSet && pRec.fldDSet_ULim >= pDSet select pRec.fldT).ToList();

                        if (pProject.Count > 0)
                        {
                            pT = Convert.ToDouble(pProject[0]);
                        }

                        return pT;
                    }

                #endregion


                #region "DIAMETERS:"

                    //....Nominal 

                    public Double DShaft()
                    //---------------------
                    { 
                        return modMain.Nom_Val(mDShaft_Range); 
                    }
         
                    public Double DFit()
                    //------------------
                    {
                        return modMain.Nom_Val(mDFit_Range);
                    }

                    public Double DPad()
                    //------------------
                    {
                        return modMain.Nom_Val(mDPad_Range);
                    }

                    public Double DSet()
                    //------------------
                    {
                        return modMain.Nom_Val(mDSet_Range);
                    }


                    public Double Tol_Detail_DSet ()                          
                    //--------------------------------
                    {
                        //....Retrieve from database.                                  
                        BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        Double pTol_Detail_DSet = 0.0f;
                        Decimal pDSet = (Decimal)DSet();

                        var pProject = (from pRec in pBearingDBEntities.tblDesign_Tol where pRec.fldDSet_LLim <= pDSet && pRec.fldDSet_ULim >= pDSet select pRec.fldTol_Detail_DSet).ToList();

                        if (pProject.Count > 0)
                        {
                            pTol_Detail_DSet = Convert.ToDouble(pProject[0]);
                        }
                        return pTol_Detail_DSet;                
                    }
    

                    public Double Clearance()
                    //-----------------------
                    {
                        return (mDSet_Range[0] - mDShaft_Range[0]);
                    }


                    public Double PreLoad()
                    //---------------------
                    {
                        Double pPreLoad = 0.0f;
                        pPreLoad = (DPad() - DSet()) / (DPad() - DShaft());

                        return pPreLoad;
                    }

                #endregion        

            #endregion      


            #region "VALIDATION ROUTINE:"

                public Double Validate_Depth_EndConfig (Double Depth_In)
                //======================================================
                {
                    if (Depth_In < mc_DEPTH_END_CONFIG_MIN)
                    {
                        string pMsg = " End config. depth should not be less than the design minimum value of, " 
                                      + mc_DEPTH_END_CONFIG_MIN + ".";
                        MessageBox.Show(pMsg);

                        Depth_In = mc_DEPTH_END_CONFIG_MIN;
                    }

                    return Depth_In;
                }

            #endregion


            #region"CLASS OBJECTS COPYING & COMPARISON ROUTINES:"

            public bool Compare(clsBearing_Radial_FP Bearing_In, string FormName_In)
            //=======================================================================   
            {
                bool mblnVal_Changed = false;
                //int pRetValue = 0;

                //if (FormName_In == "Bearing")
                //{

                //    if (modMain.CompareVar(Bearing_In.SplitConfig, mSplitConfig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    for (int i = 0; i < 2; i++)
                //    {
                //        if (modMain.CompareVar(Bearing_In.DShaft_Range[i], mDShaft_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DFit_Range[i], mDFit_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DPad_Range[i], mDPad_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DSet_Range[i], mDSet_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Count, mPad.Count, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.L, mPad.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.Pad.Ang, mPad.Ang, 0, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    //if (modMain.CompareVar(Bearing_In.Pad.RFillet, mPad.RFillet, 3, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.Pad.Pivot.AngStart, mPad.Pivot.AngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Pivot.Offset, mPad.Pivot.Offset, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Lead, mPad.T.Lead, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Pivot, mPad.T.Pivot, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Trail, mPad.T.Trail, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.T, mFlexurePivot.Web.T, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.RFillet, mFlexurePivot.Web.RFillet, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.H, mFlexurePivot.Web.H, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.GapEDM, mFlexurePivot.GapEDM, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Rot_Stiff, mFlexurePivot.Rot_Stiff, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Orifice.D, mOilInlet.Orifice.D, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.Base, mMat.Base, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.Lining, mMat.Lining, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.LiningExists, mMat.LiningExists, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.LiningT, mLiningT, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Type.ToString(), mPad.Type.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.L_Tot, mL_Tot,4, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;         
                //    //}

                //}

                //else if (FormName_In == "Performance")
                //{

                //    if (modMain.CompareVar(Bearing_In.mPerformData.Power_HP, mPower_HP, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlowReqd_gpm, mFlowReqd_gpm, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempRise_F, mTempRise_F, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.TFilm_Min, mTFilm_Min, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.TempRise, mPad_Perform_Max.TempRise, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Press, mPad_Perform_Max.Press, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Rot, mPad_Perform_Max.Rot, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Load, mPad_Perform_Max.Load, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Stress, mPad_Perform_Max.Stress, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //}

                //else if (FormName_In == "Bearing Design Details")
                //{

                //    if (modMain.CompareVar(Bearing_In.L, mL, 4, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Count_MainOilSupply, mOilInlet.Count_MainOilSupply, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.Ratio_L_H, mOilInlet.Annulus.Ratio_L_H, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.L, mOilInlet.Annulus.L, 3, pRetValue) > 0) //SB 15MAR10
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.D, mOilInlet.Annulus.D, 3, pRetValue) > 0) //SB 15MAR10
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.OrificeStartPos.ToString(), mOilInlet.OrificeStartPos.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.MillRelief.Exists, mMillRelief.Exists, pRetValue) > 0)    //SB 26MAY09
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    //if (modMain.CompareVar(Bearing_In.MillRelief.D, mMillRelief.D,3, pRetValue) > 0)  //SG 09JAN12 Review
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Type, mSplitLine_Screw_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.D_Desig, mSplitLine_Screw_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Pitch, mSplitLine_Screw_Spec.Pitch, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.L, mSplitLine_Screw_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Mat, mSplitLine_Screw_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.Type, mSplitLine_Dowel_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.D_Desig, mSplitLine_Dowel_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.L, mSplitLine_Dowel_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.Mat, mSplitLine_Dowel_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.Type, mAntiRot_Pin_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.D_Desig, mAntiRot_Pin_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.L, mAntiRot_Pin_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.Mat, mAntiRot_Pin_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Angle, mAntiRot_Pin_Loc.Angle, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Dist_Front, mAntiRot_Pin_Loc.Dist_Front, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.AntiRotPin.Loc.From_BearingSplit.ToString(), mAntiRotPin.Loc.From_BearingSplit.ToString(), pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Bearing_Vert.ToString(), mAntiRot_Pin_Loc.Bearing_Vert.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Casing_SL.ToString(), mAntiRot_Pin_Loc.Casing_SL.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Offset, mAntiRot_Pin_Loc.Offset, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FrontMountingHoles.GoThru, mEndConfig_MountingHoles_Front.GoThru, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FrontMountingHoles.Depth, mEndConfig_MountingHoles_Front.Depth, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.PartNo, mEndConfig_MountFixture_Sel_Front.PartNo, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.DBC, mEndConfig_MountFixture_Sel_Front.DBC, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.D_Finish, mEndConfig_MountFixture_Sel_Front.D_Finish, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.PartNo, mEndConfig_MountFixture_Sel_Front.PartNo, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesCount, mEndConfig_MountFixture_Sel_Front.HolesCount, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesEqiSpaced, mEndConfig_MountFixture_Sel_Front.HolesEqiSpaced, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngStart, mEndConfig_MountFixture_Sel_Front.HolesAngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngStart_Comp_Chosen, mEndConfig_MountFixture_Sel_Front.HolesAngStart_Comp_Chosen, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    for (int i = 0; i < (mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX); i++)
                //        if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngOther[i], mEndConfig_MountFixture_Sel_Front.HolesAngOther[i], 0, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }


                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Type, mEndConfig_MountFixture_Sel_Front_Thread.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.D_Desig, mEndConfig_MountFixture_Sel_Front_Thread.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.L, mEndConfig_MountFixture_Sel_Front_Thread.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Pitch, mEndConfig_MountFixture_Sel_Front_Thread.Pitch, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Mat, mEndConfig_MountFixture_Sel_Front_Thread.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.CanLength, mTempSensorHoles.CanLength, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Exists, mTempSensorHoles.Exists, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Loc.ToString(), mTempSensorHoles.Loc.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Count, mTempSensorHoles.Count, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.D, mTempSensorHoles.D, 4, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.AngStart, mTempSensorHoles.AngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //}

                return mblnVal_Changed;

            }


            //public void Clone(ref clsBearing_Radial_FP Bearing_In)
            ////=====================================================   
            //{
            //    //....Set SplitConfig.
            //    Bearing_In.SplitConfig = mSplitConfig;
            //    //Bearing_In.TypeRadial = mTypeRadial;   //SB 10JUL09
            //    //Bearing_In.Config = mConfig;           //SB 10JUL09

            //    Double[] pDShaft_Range = new Double[2];    //BG 24JUN11
            //    Double[] pDFit_Range = new Double[2];        //BG 24JUN11
            //    Double[] pDPad_Range = new Double[2];
            //    Double[] pDSet_Range = new Double[2];

            //    //....Set DFit,DSet,DShaft,DPad.
            //    for (int i = 0; i < 2; i++)
            //    {

            //        pDShaft_Range[i] = mDShaft_Range[i];
            //        pDFit_Range[i] = mDFit_Range[i];
            //        pDPad_Range[i] = mDPad_Range[i];
            //        pDSet_Range[i] = mDSet_Range[i];
            //    }

            //    Bearing_In.DShaft_Range = pDShaft_Range;
            //    Bearing_In.DFit_Range = pDFit_Range;
            //    Bearing_In.DPad_Range = pDPad_Range;
            //    Bearing_In.DSet_Range = pDSet_Range;
            //    Bearing_In.UnitSystem = mUnitSystem;
            //    //Bearing_In.L_Tot = mL_Tot;

            //    //  Pad
            //    //  ---
            //    Bearing_In.PadType = mPad.Type;
            //    Bearing_In.PadCount = mPad.Count;
            //    Bearing_In.PadL = mPad.L;
            //    //Bearing_In.PadAng = mPad.Ang;          
            //    Bearing_In.PadPivot_Offset = mPad.Pivot.Offset;
            //    Bearing_In.PadPivot_AngStart = mPad.Pivot.AngStart;
            //    Bearing_In.PadT_Lead = mPad.T.Lead;
            //    Bearing_In.PadT_Pivot = mPad.T.Pivot;
            //    Bearing_In.PadT_Trail = mPad.T.Trail;
            //    Bearing_In.PadRFillet = mPad.RFillet;
            //    Bearing_In.PadType = mPad.Type;

            //    //  Flexure Pivot
            //    //  -------------
            //    Bearing_In.FlexPivot_Web_T = mFlexPivot.Web.T;
            //    Bearing_In.FlexPivot_Web_H = mFlexPivot.Web.H;
            //    Bearing_In.FlexPivot_Web_RFillet = mFlexPivot.Web.RFillet;
            //    Bearing_In.FlexPivot_GapEDM = mFlexPivot.GapEDM;
            //    Bearing_In.FlexPivot_Rot_Stiff = mFlexPivot.Rot_Stiff;

            //    //  OilInlet
            //    //  --------
            //    Bearing_In.OilInlet_Orifice_D = mOilInlet.Orifice.D;

            //    //  Material
            //    //  ---------
            //    Bearing_In.Mat.Base = mMat.Base;
            //    Bearing_In.Mat.Lining_Mat = mMat.Lining.Lining;
            //    Bearing_In.Mat.Lining_Exists = mMat.Lining.Exists; //SB 08JUL09

            //    //....Lining Thickness
            //    Bearing_In.LiningT = mLiningT;

            //    // Performence Data
            //    // ----------------
            //    Bearing_In.Power_HP = mPower_HP;          //....Power
            //    Bearing_In.FlowReqd_gpm = mFlowReqd_gpm;  //....Flow Reqd
            //    Bearing_In.TempRise_F = mTempRise_F;      //....Temp Rise
            //    Bearing_In.TFilm_Min = mTFilm_Min;        //....TFilmMin

            //    //  Pad Max
            //    //  -------
            //    Bearing_In.Pad_Perform_MaxLoad = mPad_Perform_Max.Load;           //....Temp
            //    Bearing_In.Pad_Perform_MaxPress = mPad_Perform_Max.Press;         //....Pressure
            //    Bearing_In.Pad_Perform_MaxRot = mPad_Perform_Max.Rot;             //....Rotation
            //    Bearing_In.Pad_Perform_MaxStress = mPad_Perform_Max.Stress;       //....Load
            //    Bearing_In.Pad_Perform_MaxTempRise = mPad_Perform_Max.TempRise;   //....Stress


            //    //  OD Length
            //    //  ---------
            //    Bearing_In.L = mL;

            //    //  OilInlet
            //    //  --------
            //    Bearing_In.OilInlet_Count_MainOilSupply = mOilInlet.Count_MainOilSupply;
            //    Bearing_In.OilInlet_AnnulusRatio_L_H = mOilInlet.Annulus.Ratio_L_H;
            //    Bearing_In.OilInlet_Annulus_D = mOilInlet.Annulus.D;
            //    Bearing_In.OilInlet_Annulus_L = mOilInlet.Annulus.L;
            //    Bearing_In.OilInlet_OrificeStartPos = mOilInlet.OrificeStartPos;

            //    //  WebRelief
            //    //  ---------
            //    Bearing_In.MillRelief_Exists = mMillRelief.Exists;
            //    Bearing_In.MillRelief_D_Desig = mMillRelief.D_Desig;

            //    //  SplitLine HardWare
            //    //  ------------------
            //    //....Thread.
            //    Bearing_In.SplitLine_Screw_Spec.Type = mSplitLine_Screw_Spec.Type;
            //    Bearing_In.SplitLine_Screw_Spec.D_Desig = mSplitLine_Screw_Spec.D_Desig;
            //    Bearing_In.SplitLine_Screw_Spec.Pitch = mSplitLine_Screw_Spec.Pitch;
            //    Bearing_In.SplitLine_Screw_Spec.L = mSplitLine_Screw_Spec.L;
            //    Bearing_In.SplitLine_Screw_Spec.Mat = mSplitLine_Screw_Spec.Mat;

            //    //....Dowel Pin.
            //    Bearing_In.SplitLine_Dowel_Spec.Type = mSplitLine_Dowel_Spec.Type;
            //    Bearing_In.SplitLine_Dowel_Spec.D_Desig = mSplitLine_Dowel_Spec.D_Desig;
            //    Bearing_In.SplitLine_Dowel_Spec.L = mSplitLine_Dowel_Spec.L;
            //    Bearing_In.SplitLine_Dowel_Spec.Mat = mSplitLine_Dowel_Spec.Mat;

            //    //  Anti Rotation Pin
            //    //  ------------------
            //    Bearing_In.AntiRotPin_Loc_Angle = mAntiRot_Pin_Loc.Angle;
            //    //Bearing_In.AntiRot_PinLoc_Dist_FromFront = mAntiRot_PinLoc.Dist_Front;
            //    Bearing_In.AntiRotPin_Loc_Dist_Front = mAntiRot_Pin_Loc.Dist_Front;
            //    //Bearing_In.AntiRotPin_Loc_From_BearingSplit = mAntiRotPin.Loc.From_BearingSplit;
            //    Bearing_In.AntiRotPin_Loc_Bearing_Vert = mAntiRot_Pin_Loc.Bearing_Vert;
            //    Bearing_In.AntiRotPin_Loc_Casing_SL = mAntiRot_Pin_Loc.Casing_SL;
            //    //Bearing_In.AntiRot_PinLoc_Offset = mAntiRot_PinLoc.Offset;
            //    Bearing_In.AntiRotPin_Loc_Offset = mAntiRot_Pin_Loc.Offset;

            //    //....Anti Rotation Pin.
            //    Bearing_In.AntiRot_Pin_Spec.Type = mAntiRot_Pin_Spec.Type;
            //    Bearing_In.AntiRot_Pin_Spec.D_Desig = mAntiRot_Pin_Spec.D_Desig;
            //    Bearing_In.AntiRot_Pin_Spec.L = mAntiRot_Pin_Spec.L;
            //    Bearing_In.AntiRot_Pin_Spec.Mat = mAntiRot_Pin_Spec.Mat;

            //    //  Seal Mounting Hole
            //    //  ------------------

            //    Bearing_In.FrontMountingHoles_GoThru = mEndConfig_MountingHoles_Front.GoThru;
            //    Bearing_In.FrontMountingHoles_Depth = mEndConfig_MountingHoles_Front.Depth;

            //    //  Seal Mounting Hole Selected
            //    //  ----------------------------                       
            //    Bearing_In.SealMountFixture_Candidates_Chosen_Front = mEndConfig_MountFixture_Candidadtes_Chosen_Front;
            //    Bearing_In.Retrieve_CandidateMountFixtures(mEndCofig_MountFixture_Candidadtes_Front);

            //    Bearing_In.SealMountFixture_Sel_Front_PartNo = mEndConfig_MountFixture_Sel_Front.PartNo;
            //    Bearing_In.SealMountFixture_Sel_Front_DBC = mEndConfig_MountFixture_Sel_Front.DBC;
            //    Bearing_In.SealMountFixture_Sel_Front_D_Finish = mEndConfig_MountFixture_Sel_Front.D_Finish;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngStart_Comp_Chosen = mEndConfig_MountFixture_Sel_Front.HolesAngStart_Comp_Chosen;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesCount = mEndConfig_MountFixture_Sel_Front.HolesCount;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesEqiSpaced = mEndConfig_MountFixture_Sel_Front.HolesEqiSpaced;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngStart = mEndConfig_MountFixture_Sel_Front.HolesAngStart;

            //    Double[] pSealMountFixture_Sel_HolesAngOther = new Double[7];
            //    for (int i = 0; i < Bearing_In.SealMountFixture_Sel_Front.HolesCount - 1; i++)
            //        pSealMountFixture_Sel_HolesAngOther[i] = mEndConfig_MountFixture_Sel_Front.HolesAngOther[i];

            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngOther = (Double[])pSealMountFixture_Sel_HolesAngOther.Clone(); //SB 09JUL09

            //    //....MountHole Thread
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Type = mEndConfig_MountFixture_Sel_Front_Thread.Type;
            //    Bearing_In.SealMountFixture_Sel_Front_D_Desig = mEndConfig_MountFixture_Sel_Front_Thread.D_Desig;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Pitch = mEndConfig_MountFixture_Sel_Front_Thread.Pitch;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_L = mEndConfig_MountFixture_Sel_Front_Thread.L;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Mat = mEndConfig_MountFixture_Sel_Front_Thread.Mat;

            //    //  Temp Sensor Holes
            //    //  -----------------
            //    Bearing_In.TempSensorHoles_Exists = mTempSensorHoles.Exists;
            //    Bearing_In.TempSensorHoles_Count = mTempSensorHoles.Count;
            //    Bearing_In.TempSensorHoles_D = mTempSensorHoles.D;
            //    Bearing_In.TempSensorHoles_Loc = mTempSensorHoles.Loc;
            //    Bearing_In.TempSensorHoles_AngStart = mTempSensorHoles.AngStart;
            //}

            #endregion


            #region "ICLONEABLE MEMBERS:"
            //==========================

                public object Clone()
                //===================
                {
                    //return this.MemberwiseClone();

                    BinaryFormatter pBinSerializer;
                    StreamingContext pStreamContext;

                    pStreamContext = new StreamingContext(StreamingContextStates.Clone);
                    pBinSerializer = new BinaryFormatter(null, pStreamContext);

                    MemoryStream pMemBuffer;
                    pMemBuffer = new MemoryStream();

                    //....Serialize the object into the memory stream
                    pBinSerializer.Serialize(pMemBuffer, this);

                    //....Move the stream pointer to the beginning of the memory stream
                    pMemBuffer.Seek(0, SeekOrigin.Begin);


                    //....Get the serialized object from the memory stream
                    Object pobjClone;
                    pobjClone = pBinSerializer.Deserialize(pMemBuffer);
                    pMemBuffer.Close();   //....Release the memory stream.

                    return pobjClone;    //.... Return the deeply cloned object.
                }

            #endregion


        #endregion
 

    }
}

