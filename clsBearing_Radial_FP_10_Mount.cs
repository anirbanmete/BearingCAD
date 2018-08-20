//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_10_Mount          '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 23AUG12. Changes made. 

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

namespace BearingCAD20
{
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        
        [Serializable]
        public class clsMount
        //====================
        {
            #region "NAMED CONSTANTS:"
            //========================

                //....Design Diametral Clearance between Bearing End Config. Fit Dia and End Config. OD.     
                //........Used in EndConfig_DO_Max (), DFit().
                //
                private const Double mc_DESIGN_DCLEAR_END_CONFIG_D_FIT = 0.002F;

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================

                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                //   Index: Front 0; Back 1. 
                //
                //....Holes.
                private Boolean mHoles_GoThru;
                private eFaceID mHoles_Bolting;


                //....If GoThru = TRUE,  Depth = 0 (irrelevant).
                //........        FALSE, Depth > 0. 
                //
                private Double[] mHoles_Thread_Depth = new Double[2];


                //....Candidate set of Fixtures (which will be the same for FRONT & BACK).
                //
                private Boolean[] mFixture_Candidates_Chosen = new Boolean[2];      //....Whether Candidates to be presented for User's selection.
                private clsFixture_Candidates mFixture_Candidates;


                //....Selected / New Fixture. 
                //
                private clsFixture[] mFixture = new clsFixture[2];
                //private clsScrew[] mFixture_Screw_Spec;             //PB 07JUN12. This member variable should be a part of mFixture. 
                //                                                    //....To be done in the future version. //PB 23AUG12. Move this as a member variable of clsFixture. 
            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //=================================

                #region "Holes:". Move 
                //-------------

                    public Boolean Holes_GoThru
                    {
                        get { return mHoles_GoThru; }
                        set { mHoles_GoThru = value; }
                    }


                    public eFaceID Holes_Bolting
                    {
                        get { return mHoles_Bolting; }
                        set { mHoles_Bolting = value; }
                    }


                    public Double[] Holes_Thread_Depth   
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mHoles_Thread_Depth[i] < modMain.gcEPS)
                                    //mHoles_Thread_Depth[i] = 2 * mFixture_Screw_Spec[i].D;      //....Def. Val.
                                    mHoles_Thread_Depth[i] = 2 * mFixture[i].Screw_Spec.D;      //....Def. Val.     //BG 24AUG12
                            }
                            return mHoles_Thread_Depth;
                        }

                        set   //PB 07JUN12. For an array type, "get" property method is needed but the "set" property method 
                              //....is not necessary. If a specific index is given, it is not resolved here for the purpose of 
                              //....validation or triggering another method. It is to be handled differently.
                        {
                            //for (int i = 0; i < 2; i++)
                            //{
                            //     mHoles_Thread_Depth[i] = Validate_Holes_Thread_Depth(value[i]);
                            //}
                        }
                    }

                #endregion


                #region "Candidate Fixtures:"
                //--------------------------

                    //.....Check whether candidate chosen or not.
                    public Boolean[] Fixture_Candidates_Chosen
                    {
                        get { return mFixture_Candidates_Chosen; }
                        set { mFixture_Candidates_Chosen = value; }
                    }


                    //.....Candidate Set of Mount Fixtures:
                    //........Read-only as the fixture dataset is retrieved from the database.
                    //
                    public clsFixture_Candidates Fixture_Candidates
                    {
                        get
                        {
                            mFixture_Candidates.Retrieve();
                            return mFixture_Candidates;
                        }
                    }

                #endregion


                #region "Selected / New Fixture:"
                //-------------------------------

                    public clsFixture[] Fixture
                    {
                        get { return mFixture; }
                    }


                    #region "FRONT:"
                    //-------------

                        public string Fixture_Front_PartNo
                        {
                            set
                            {
                                //....This is called only when the mFixture_Candidates_Chosen [0] == true. 
                                mFixture[0].PartNo = value;

                                if (mFixture_Candidates.Exists)
                                    Set_Fixture_Sel_Params(0);
                            }
                        }


                        public bool Fixture_Front_HolesAngStart_Comp_Chosen
                        {
                            set { mFixture[0].HolesAngStart_Comp_Chosen = value; }
                        }

                        //....Keep these properties here. They may need to be exposed when 
                        //........there are no candidates.

                        public Double Fixture_Front_DBC
                        {
                            set { mFixture[0].DBC = value; }
                        }

                        public Double Fixture_Front_D_Finish
                        {
                            set { mFixture[0].D_Finish = value; }
                        }

                        public int Fixture_Front_HolesCount
                        {
                            set { mFixture[0].HolesCount = value; }
                        }

                        public bool Fixture_Front_HolesEquiSpaced
                        {
                            set { mFixture[0].HolesEquiSpaced = value; }
                        }

                        public Double Fixture_Front_HolesAngStart
                        {
                            set { mFixture[0].HolesAngStart = value; }
                        }

                        public Double[] Fixture_Front_HolesAngOther
                        {
                            set { mFixture[0].HolesAngOther = value; }
                        }

                    #endregion


                    #region "BACK:"

                        public string Fixture_Back_PartNo
                        {
                            set
                            {
                                //....This is called only when the mFixture_Candidates_Chosen [1] == true. 
                                mFixture[1].PartNo = value;

                                if (mFixture_Candidates.Exists)
                                    Set_Fixture_Sel_Params(1);
                            }
                        }


                        public bool Fixture_Back_HolesAngStart_Comp_Chosen
                        {
                            set { mFixture[1].HolesAngStart_Comp_Chosen = value; }
                        }


                        //....Keep these properties here. They may need to be exposed when 
                        //........there are no candidates.

                        public Double Fixture_Back_DBC
                        {
                            set { mFixture[1].DBC = value; }
                        }

                        public Double Fixture_Back_D_Finish
                        {
                            set { mFixture[1].D_Finish = value; }
                        }

                        public int Fixture_Back_HolesCount
                        {
                            set { mFixture[1].HolesCount = value; }
                        }

                        public bool Fixture_Back_HolesEqiSpaced
                        {
                            set { mFixture[1].HolesEquiSpaced = value; }
                        }

                        public Double Fixture_Back_HolesAngStart
                        {
                            set { mFixture[1].HolesAngStart = value; }
                        }

                        public Double[] Fixture_Back_HolesAngOther
                        {
                            set { mFixture[1].HolesAngOther = value; }
                        }

                    #endregion


                    //#region "Screw:"      //BG 24AUG12

                    //    public clsScrew[] Fixture_Screw_Spec
                    //    {
                    //        get { return mFixture_Screw_Spec; }
                    //        set { mFixture_Screw_Spec = value; }
                    //    }

                    //#endregion

                #endregion


            #endregion


            #region "CONSTRUCTOR:"

                public clsMount(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //===============================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                    mFixture_Candidates = new clsFixture_Candidates(mCurrent_Bearing_Radial_FP);

                    //BG 24AUG12
                    //clsUnit.eSystem pUnitSystem = mCurrent_Bearing_Radial_FP.Unit.System;
                    //mFixture_Screw_Spec = new clsScrew[2] { new clsScrew(pUnitSystem), new clsScrew(pUnitSystem) };       


                    //....Initialize:
                    for (int i = 0; i < 2; i++)                     //....i = 0: FRONT, = 1: BACK.
                    {
                        mFixture_Candidates_Chosen[i] = true;
                        mFixture[i] = new clsFixture(mCurrent_Bearing_Radial_FP);

                        ////....Screw.      /BG 24AUG12
                        //mFixture_Screw_Spec[i].Type = "SHCS";
                        //mFixture_Screw_Spec[i].Mat = "STL";
                    }

                    mHoles_GoThru = true;
                    //mHoles_Bolting = eFaceID.Back;        //BG 02JUL13
                    mHoles_Bolting = eFaceID.Front;         //BG 02JUL13
                }

            #endregion


            #region "CLASS METHODS":

                public Double DFit_EndConfig(int Indx_In)
                //========================================                 
                {
                    //....Ref. Radial_Rev11_27OCT11: Col. CX (FRONT). BACK ?.
                    //....Ref. Radial_Rev12_14APR12: Col. CX (FRONT). KY (BACK).
                    return (mFixture[Indx_In].D_Finish + mc_DESIGN_DCLEAR_END_CONFIG_D_FIT);
                }


                public Double TWall_BearingCB(int Indx_In)         
                //==========================================           
                {
                    Double pTWall = 0;
                    pTWall = 0.5 * (mCurrent_Bearing_Radial_FP.DFit() - DFit_EndConfig(Indx_In));

                    return pTWall;
                }


                private void Set_Fixture_Sel_Params(int Indx_In)
                //===============================================                          
                {
                    //....This routine assumes that the candidate fixtures are available. 
                    //........Indx_In = 0 ==> FRONT.
                    //                = 1 ==> BACK.

                    if (!mFixture_Candidates.Exists) return;

                    int pISel = -1;
                    for (int i = 0; i < mFixture_Candidates.PartNo.Length; i++)
                    {
                        if (mFixture_Candidates.PartNo[i] == mFixture[Indx_In].PartNo)
                            pISel = i;
                    }


                    if (pISel != -1)
                    {
                        mFixture[Indx_In].DBC = mFixture_Candidates.DBC[pISel];
                        mFixture[Indx_In].D_Finish = mFixture_Candidates.D_Finish[pISel];

                        mFixture[Indx_In].HolesCount = mFixture_Candidates.HolesCount[pISel];
                        mFixture[Indx_In].HolesEquiSpaced = mFixture_Candidates.HolesEquiSpaced[pISel];
                        mFixture[Indx_In].HolesAngStart = mFixture_Candidates.HolesAngStart[pISel];
                        //mFixture[Indx_In].HolesAngStart_Comp = mFixture_Candidates.HolesAngStart_Comp[pISel];     //BG 24AUG12

                        for (int i = 0; i < mFixture[Indx_In].HolesCount - 1; i++)
                        {
                            mFixture[Indx_In].HolesAngOther[i] = mFixture_Candidates.HolesAngOther[pISel, i];
                        }

                        //mFixture_Screw_Spec[Indx_In].D_Desig = mFixture_Candidates.ThreadDia_Desig[pISel];
                        mFixture[Indx_In].Screw_Spec.D_Desig = mFixture_Candidates.ThreadDia_Desig[pISel];          //BG 24AUG12
                        //mMountFixture_Screw_Spec[Indx_In].Pitch = Get_Pitch(mMountFixture_Screw_Spec[Indx_In].D_Desig); //PB 01FEB12. May be incorrect.
                    }
                }


                public Double[] Fixture_Sel_Ang_Other_EndConfig()
                //=================================================
                {
                    //....This routine is invoked only when GoThru = TRUE.
                    int piBoltEnd = 0;

                    if (mHoles_Bolting == eFaceID.Front)
                    {
                        piBoltEnd = 0;
                    }
                    else if (mHoles_Bolting == eFaceID.Back)
                    {
                        piBoltEnd = 1;
                    }

                    Double[] pAng_Back = new Double[mFixture[piBoltEnd].HolesCount];


                    if (!mFixture[piBoltEnd].HolesEquiSpaced)
                    {
                        //Not EquiSpaced:
                        //---------------
                        //
                        //....Find out in which angle these angles going to cross 180 degree.
                        Double pStartAng = 0.0F;

                        if (!mFixture[piBoltEnd].HolesAngStart_Comp_Chosen)
                            pStartAng = mFixture[piBoltEnd].HolesAngStart;
                        else
                            //pStartAng = mFixture[piBoltEnd].HolesAngStart_Comp;
                            pStartAng = mFixture[piBoltEnd].HolesAngStart_Comp();           //BG 24AUG12

                        Double pSumAng180 = pStartAng;
                        int pIndx = 0;

                        while (pSumAng180 < 180)
                        {
                            pSumAng180 = pSumAng180 + mFixture[piBoltEnd].HolesAngOther[pIndx];
                            pIndx = pIndx + 1;
                        }
                        //pIndx = pIndx - 1;  //...This index of Angle other will cross 180.

                        Double pSumAngIndx = 0.0F;
                        for (int i = 0; i < pIndx - 1; i++)
                            pSumAngIndx = pSumAngIndx + mFixture[piBoltEnd].HolesAngOther[i];

                        //....Start Angle for "Other" End Config..  
                        pAng_Back[0] = (180 - (pStartAng + pSumAngIndx));

                        //....Other Angles. 
                        for (int i = 1; i <= pIndx - 1; i++)
                            pAng_Back[i] = mFixture[piBoltEnd].HolesAngOther[(pIndx - 1) - i]; //SB 04AUG09

                        Double pSumAngOther = 0.0F;
                        for (int i = 0; i < mFixture[piBoltEnd].HolesCount - 1; i++)
                            pSumAngOther = pSumAngOther + mFixture[piBoltEnd].HolesAngOther[i];

                        //....Other Angle[2] for "Other" End Config.. 
                        pAng_Back[pIndx] = (360 - pSumAngOther);

                        //....Other Angle[3] for "Other" End Config..
                        for (int i = pIndx + 1; i < mFixture[piBoltEnd].HolesCount; i++)
                            pAng_Back[i] =
                                mFixture[piBoltEnd].HolesAngOther[mFixture[piBoltEnd].HolesCount - i + (pIndx - 1)];
                    }

                    else if (mFixture[piBoltEnd].HolesEquiSpaced)
                    {
                        //EquiSpaced:
                        //-----------
                        //
                        Double pStartAng = 0.0F;

                        if (!mFixture[piBoltEnd].HolesAngStart_Comp_Chosen)
                            pStartAng = mFixture[piBoltEnd].HolesAngStart;
                        else
                            //pStartAng = mFixture[piBoltEnd].HolesAngStart_Comp;   //SB 03AUG09
                            pStartAng = mFixture[piBoltEnd].HolesAngStart_Comp();   //BG 24AUG12
                        Double pEquiSpacedAngle = (360.0F / mFixture[piBoltEnd].HolesCount);

                        //....Find out in which angle these angles going to cross 180 degree.
                        Double pSumAng180 = pStartAng;
                        int pIndx = 0;

                        while (pSumAng180 < 180)
                        {
                            pSumAng180 = pSumAng180 + pEquiSpacedAngle;
                            pIndx = pIndx + 1;
                        }

                        Double pSumAngIndx = 0.0F;
                        for (int i = 0; i < pIndx - 1; i++)
                            pSumAngIndx = pSumAngIndx + pEquiSpacedAngle;

                        //....Start Angle for Back Seal.  
                        pAng_Back[0] = (180 - (pStartAng + pSumAngIndx));

                        //....Other Angles for Back Seal. 
                        for (int i = 1; i < mFixture[piBoltEnd].HolesCount; i++)
                            pAng_Back[i] = pEquiSpacedAngle;
                    }

                    return pAng_Back;
                }


                public Double MountFixture_Sel_AngOther(int Indx_In)
                //==================================================
                {
                    return (360.0F / mFixture[Indx_In].HolesCount);
                }


                public Double MountFixture_Screw_L_UpperLimit(int Indx_In)
                //========================================================        
                {
                    //PB 10FEB12. This routine is valid for Go Thru' only. Ask Harout.

                    //clsScrew pScrew_Spec = mFixture_Screw_Spec[Indx_In];
                    clsScrew pScrew_Spec = mFixture[Indx_In].Screw_Spec;        //BG 24AUG12

                    //....Get Screw Head Height.
                    //
                    string pD_Desig = pScrew_Spec.D_Desig;
                    Double pHead_H = pScrew_Spec.Head_H;
                    Double pL_UpperLimit = 0.0F;

                    if (pD_Desig != "" && pD_Desig != null)
                    {

                        if (pD_Desig.Contains("M"))     //....Metric
                        {
                            //....Convert Head_H from mm ==> in.
                            pHead_H = pHead_H / 25.4F;                          //....in.
                            pL_UpperLimit = mCurrent_Bearing_Radial_FP.mCurrentProduct.L_Tot() - pHead_H - (2 * 0.020F);
                            pL_UpperLimit = pL_UpperLimit * 25.4F;              //....mm
                        }

                        else                            //....English.
                        {
                            pL_UpperLimit = mCurrent_Bearing_Radial_FP.mCurrentProduct.L_Tot() - pHead_H - (2 * 0.020F);   //....in
                        }
                    }

                    return pL_UpperLimit;
                }


                public Double MountFixture_Screw_L_LowerLimit(int Indx_In)
                //========================================================= 
                {
                    //....Relevant Radial Bearing Parameter:
                    //clsScrew pScrew_Spec = Fixture_Screw_Spec[Indx_In];
                    clsScrew pScrew_Spec = Fixture[Indx_In].Screw_Spec;  //BG 24AUG12
                   
                    double pBearing_Pad_L = mCurrent_Bearing_Radial_FP.Pad.L;

                    //....Relevant End Config Parameter:
                    double pEndConfigL = mCurrent_Bearing_Radial_FP.mCurrentProduct.EndConfig[Indx_In].L;

                    //....Mounting Screw Parameters:
                    string pD_Desig = pScrew_Spec.D_Desig;
                    Double pHead_H = pScrew_Spec.Head_H;
                    Double pD = pScrew_Spec.D;

                    Double pLowerLimit = 0.0F;

                    if (pD_Desig != "" && pD_Desig != null)
                    {
                        if (pD_Desig.Contains("M"))                             //....Metric
                        {
                            //....Convert from mm ==> in.
                            pHead_H = pHead_H / 25.4F;          //....in 
                            pD = pD / 25.4F;

                            pLowerLimit = (pEndConfigL + pBearing_Pad_L) - pHead_H - 0.020F + 1.5F * pD;
                            pLowerLimit = pLowerLimit * 25.4F;  //....mm
                        }

                        else                                                    //....English.
                        {
                            pLowerLimit = (pEndConfigL + pBearing_Pad_L) - pHead_H - 0.020F + 1.5F * pD;
                        }
                    }

                    return pLowerLimit;
                }


                #region "VALIDATION ROUTINE:"

                    public Double Validate_Holes_Thread_Depth (int Indx_In, Double Depth_In)
                    //======================================================================
                    {
                        //....This function is used only when Go Thru' = No;

                        //....Establish the lower & upper limits of the thread depth.
                        double pDepth_LLim, pDepth_ULim;
                        pDepth_LLim = 1.5 * mFixture[Indx_In].Screw_Spec.D;

                        double pDepth_TapDrill_Max;
                        pDepth_TapDrill_Max = 0.5F * mCurrent_Bearing_Radial_FP.Pad.L - 0.125;
                        pDepth_ULim         = pDepth_TapDrill_Max - 0.0625;

                        double pDepth_Lim = Depth_In;

                        string pMsg;

                        if (Depth_In < pDepth_LLim)
                        {
                            pMsg = "Mount holes thread depth should not be less than 1.5 X Thread Dia.";
                            MessageBox.Show(pMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pDepth_Lim = pDepth_LLim;
                        }

                        if (Depth_In > pDepth_ULim)
                        {
                            pMsg = "Mount holes thread depth should not exceed the mid-point of the pad length.";
                            MessageBox.Show(pMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pDepth_Lim = pDepth_ULim;
                        }

                        return pDepth_Lim;
                    }

                #endregion

            #endregion
                  

            #region "SUB-NESTED CLASSES:"

                #region "Class Fixture Candidates":

                    [Serializable]
                    public class clsFixture_Candidates
                    //=================================
                    {
                        #region "NAMED CONSTANTS:"
                        //========================
                            //....Minimum allowable Wall Thickness @ Bearing OD Counter Bore.        
                            private const Double mc_DESIGN_TWALL_BEARING_CB_MIN = 0.125F;           //....Used in EndConfig_DO_Max()

                        #endregion


                        #region "MEMBER VARIABLES:"
                        //=========================
                            private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                            private bool mExists;
                            private String[] mPartNo;
                            private Double[] mDBC;
                            private Double[] mD_Finish;
                            private int[] mHolesCount;
                            private bool[] mHolesEquiSpaced;
                            private Double[] mHolesAngStart;
                            //private Double[] mHolesAngStart_Comp;           //PB 23AUG12. This property should be suppressed in this class..  //BG 24AUG12    
                            private Double[,] mHolesAngOther;
                            private String[] mThreadDia_Desig;
                            private Double[] mThreadPitch;

                            private int mCount;               

                        #endregion


                        #region "CLASS PROPERTY ROUTINES:"
                        //=================================

                            public bool Exists
                            {
                                get { return mExists; }
                            }

                            public string[] PartNo
                            {
                                get { return mPartNo; }
                            }

                            public Double[] DBC
                            {
                                get { return mDBC; }
                            }

                            public Double[] D_Finish
                            {
                                get { return mD_Finish; }
                            }

                            public int[] HolesCount
                            {
                                get { return mHolesCount; }
                            }

                            public bool[] HolesEquiSpaced
                            {
                                get { return mHolesEquiSpaced; }
                            }

                            public Double[] HolesAngStart
                            {
                                get { return mHolesAngStart; }
                            }

                            //public Double[] HolesAngStart_Comp        //BG 24AUG12
                            //{
                            //    get { return mHolesAngStart_Comp; }
                            //}

                            public Double[,] HolesAngOther
                            {
                                get { return mHolesAngOther; }
                            }

                            public string[] ThreadDia_Desig
                            {
                                get { return mThreadDia_Desig; }
                            }

                            public Double[] ThreadPitch
                            {
                                get { return mThreadPitch; }
                            }

                            public int Count
                            {
                                get { return mCount; }                    
                            }

                        #endregion


                        #region "CONSTRUCTOR:"

                            public clsFixture_Candidates(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                            //=============================================================================
                            {
                                mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                            }

                        #endregion


                        #region "CLASS METHODS":

                            public void Retrieve()          //PB 20FEB12. To be reviewed.
                            //=====================
                            {
                                //Approximate DBC:
                                //----------------
                                Double pDBC_Calc = Calc_DBC_Ballpark();


                                //DBC Limits:
                                //-----------
                                //....Based on conversation with Harout K., Waukesha Bearings, 13APR12.
                                //
                                Double pcHalfRange;// pDup;
                                if (mCurrent_Bearing_Radial_FP.OilInlet.Annulus.Exists == true)     //....Annulus.
                                {
                                    //pDup = mCurrent_Bearing_Radial_FP.OilInlet.Annulus.D;
                                    pcHalfRange = 0.25;
                                }
                                else                                                                //....No Annulus.
                                {
                                    //pDup = mCurrent_Bearing_Radial_FP.DFit();
                                    pcHalfRange = 0.75;
                                }

                                Double pDBC_Max, pDBC_Min;
                                pDBC_Max = pDBC_Calc + pcHalfRange;
                                pDBC_Min = pDBC_Calc - pcHalfRange;

                   
                                //Max. Allowable Seal OD:
                                //-----------------------
                                //....Seal_Do_Max ().


                                //Retrieve the Candidate Fixtures:
                                //---------------------------------
                                string pFIELDS, pFROM, pWHERE, pSQL;

                                pFIELDS = "* ";
                                pFROM = "FROM tblManf_Fixture_SplitAndTurn ";

                                pWHERE = "WHERE fldDBC >=" + pDBC_Min + " AND fldDBC <=" + pDBC_Max +
                                            " AND fldDBC IS NOT NULL AND fldDFinish <=" + EndConfig_DO_Max() +
                                            " AND fldDFinish IS NOT NULL AND fldPartNo IS NOT NULL" +
                                            " ORDER BY " + "[fldDFinish]" + " ASC";
                                                         
                                pSQL = "SELECT " + pFIELDS + pFROM + pWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = mCurrent_Bearing_Radial_FP.mDB.GetDataReader(pSQL, ref pConnection);

                                //....Data Set created because of row count.
                                //....Data Reader do not have any row count property.
                                DataSet pDS = null;
                                pDS = mCurrent_Bearing_Radial_FP.mDB.GetDataSet(pSQL, "tblManf_Fixture_SplitAndTurn");  

                                //....Row Count.
                                int pRowCount = 0;
                                pRowCount = pDS.Tables[0].Rows.Count;

                                pDS.Dispose();
                                pDS = null;


                                if (pRowCount == 0)
                                {
                                    //....Candidate Fixture doesn't exist.
                                    mExists = false;

                                    //.....Close Data Reader.       
                                    pDR.Close();
                                    pDR = null;
                                    pConnection.Close();

                                    return;
                                }
                                else
                                {
                                    //....Candidate Fixture Exists.
                                    mExists = true;

                                    //....Dynamically Allocate Array with RowNumber.
                                    mPartNo             = new string[pRowCount];
                                    mDBC                = new Double[pRowCount];
                                    mD_Finish           = new Double[pRowCount];
                                    mHolesCount         = new int[pRowCount];
                                    mHolesEquiSpaced    = new bool[pRowCount];
                                    mHolesAngStart      = new Double[pRowCount];
                                    //mHolesAngStart_Comp = new Double[pRowCount];      //BG 24AUG12

                                    //.....Maximum Number of other Angles.
                                    mHolesAngOther = new Double[pRowCount, mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX];

                                    mThreadDia_Desig    = new string[pRowCount];
                                    mThreadPitch        = new Double[pRowCount];


                                    //.....Retrieve Fixture data from tblManf_Fixture_SplitAndTurn.  PB 19FEB12. Recheck.
                                    for (int i = 0; i <= pRowCount; i++)
                                    {
                                        if (pDR.Read())
                                        {
                                            mPartNo[i]             = pDR["fldPartNo"].ToString();
                                            mDBC[i]                = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pDR, "fldDBC");
                                            mD_Finish[i]           = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pDR, "fldDFinish");
                                            mHolesCount[i]         = mCurrent_Bearing_Radial_FP.mDB.CheckDBInt(pDR, "fldCountHoles");
                                            mHolesEquiSpaced[i]    = mCurrent_Bearing_Radial_FP.mDB.CheckDBBool(pDR, "fldEqui_Spaced");
                                            mHolesAngStart[i]      = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pDR, "fldAng_Start");
                                            //mHolesAngStart_Comp[i] = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pDR, "fldAng_Start_Comp");

                                            mThreadDia_Desig[i]    = pDR["fldDia_Desig"].ToString();
                                            //mThreadPitch [i]        = Get_Pitch(mThreadDia_Desig[i]); //PB 01FEB12. May be incorrect.

                                            if (!mHolesEquiSpaced[i])
                                            {
                                                for (int j = 0; j < mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX; j++)
                                                    mHolesAngOther[i, j] = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pDR, "fldAng_Other" + (j + 1).ToString());
                                            }

                                            //mHolesAngStart_Comp[i] = Calc_AngStart_Comp(i);       //BG 24AUG12

                                            if (!mCurrent_Bearing_Radial_FP.Mount.Holes_GoThru)
                                            {
                                                //....Go Thru' = NO;
                                                //........Further selection criteria applied based on minimum thread depth.

                                                //........Min Tap Drill depth.
                                                clsScrew pScrew_Spec = new clsScrew (mCurrent_Bearing_Radial_FP.Unit.System);
                                                pScrew_Spec.D_Desig  = mThreadDia_Desig[i];

                                                Double pDepth_TapDrill_Min;
                                                pDepth_TapDrill_Min = 1.5 * pScrew_Spec.D + 0.0625;

                                                if (pDepth_TapDrill_Min > 0.5 * mCurrent_Bearing_Radial_FP.Pad.L - 0.125F)
                                                {
                                                    //....Criteria violated. This fixture is rejected from the candidate list.
                                                    mPartNo[i] = "";
                                                }
                                            }
                                        }
                                    }
                                 }


                                //.....Close Data Reader.
                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }


                            public Double Calc_DBC_Ballpark()    // ?private Review later
                            //===============================                               
                            {
                                double pD_Up;

                                if (mCurrent_Bearing_Radial_FP.OilInlet.Annulus.Exists == true)
                                {
                                    pD_Up = mCurrent_Bearing_Radial_FP.OilInlet.Annulus.D;
                                }
                                else
                                {
                                    pD_Up = mCurrent_Bearing_Radial_FP.DFit();
                                }


                                double pD_PadRelief = mCurrent_Bearing_Radial_FP.MillRelief.D_PadRelief();

                                Double pDBC;
                                pDBC = 0.5 * (pD_Up + pD_PadRelief);
                                return pDBC;
                            }


                            public Double EndConfig_DO_Max()       // ?private Review later
                            //===============================                              
                            {
                                Double pDFit = mCurrent_Bearing_Radial_FP.DFit();
                                Double pDO_Max;
                                pDO_Max = pDFit - 2 * mc_DESIGN_TWALL_BEARING_CB_MIN
                                                    - mc_DESIGN_DCLEAR_END_CONFIG_D_FIT;
                                return pDO_Max;
                            }


                            //public Double Calc_AngStart_Comp(int Indx_In)
                            ////============================================          //BG 22AUG12  //PB23AUG12. This routine should be in clsFixture.      //BG 24AUG12
                            //{
                            //    Double pAngStart_Comp = 0.0F;
                               
                            //    if (mHolesEquiSpaced[Indx_In])
                            //    {
                            //        Double pAngleOther = (360.0F / mHolesCount[Indx_In]);

                            //        int pCount = mHolesCount[Indx_In] - 1;
                            //        pAngStart_Comp = 360 - (mHolesAngStart[Indx_In] + (pCount * pAngleOther));
                            //    }
                            //    else
                            //    {
                            //        Double pAng_Other_Total = 0.0F;

                            //        for (int j = 0; j < mHolesCount[Indx_In] - 1; j++)
                            //        {
                            //            pAng_Other_Total = pAng_Other_Total + mHolesAngOther[Indx_In, j];
                            //        }

                            //        pAngStart_Comp = 360 - (mHolesAngStart[Indx_In] + pAng_Other_Total);
                            //    }

                            //    return pAngStart_Comp;
                            //}

                        #endregion
                    }

                #endregion


                #region "Class Fixture Selected/New:"

                    [Serializable]
                    public class clsFixture: ICloneable
                    //=====================
                    {
                        #region "MEMBER VARIABLES:"
                        //=========================

                            //private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;    //BG 24AUG12

                            public String mPartNo;
                            public Double mDBC;
                            public Double mD_Finish;
                            public int mHolesCount;
                            public bool mHolesEquiSpaced;
                            public Double mHolesAngStart;
                            //public Double mHolesAngStart_Comp;          //PB 23AUG12. This should be a method not property. In the frmBearingDesignDetails, in the start angle drop down show the start angle as the fist item and its comp angle as the 2nd list item, if it is not equql to the start angle. //BG 24AUG12
                            public bool mHolesAngStart_Comp_Chosen;
                            public Double[] mHolesAngOther;
                            private clsScrew mScrew_Spec;         //BG 24AUG12 
                            

                        #endregion


                        #region "CLASS PROPERTY ROUTINES:"
                        //=================================

                            public string PartNo
                            {
                                get { return mPartNo; }
                                set { mPartNo = value; }
                            }

                            public Double DBC
                            {
                                get { return mDBC; }
                                set { mDBC = value; }
                            }

                            public Double D_Finish
                            {
                                get { return mD_Finish; }
                                set { mD_Finish = value; }
                            }

                            public int HolesCount
                            {
                                get { return mHolesCount; }
                                set { mHolesCount = value; }
                            }

                            public bool HolesEquiSpaced
                            {
                                get { return mHolesEquiSpaced; }
                                set { mHolesEquiSpaced = value; }
                            }

                            public Double HolesAngStart
                            {
                                get { return mHolesAngStart; }
                                set { mHolesAngStart = value; }
                            }

                            //public Double HolesAngStart_Comp          //BG 24AUG12
                            //{
                            //    get { return mHolesAngStart_Comp; }
                            //    set { mHolesAngStart_Comp = value; }
                            //}

                            public bool HolesAngStart_Comp_Chosen
                            {
                                get { return mHolesAngStart_Comp_Chosen; }
                                set { mHolesAngStart_Comp_Chosen = value; }
                            }

                            public Double[] HolesAngOther
                            {
                                get { return mHolesAngOther; }
                                set { mHolesAngOther = value; }
                            }

                            #region "Screw:"

                                public clsScrew Screw_Spec
                                {
                                    get { return mScrew_Spec; }
                                    set { mScrew_Spec = value; }
                                }

                            #endregion

                        #endregion


                        #region "CONSTRUCTOR:"

                             //BG 24AUG12
                            //public clsFixture()
                            //{                              

                            //    mHolesAngOther = new Double[mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX];

                            //}

                            public clsFixture(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)    
                            //==================================================================        
                            {
                                mHolesAngOther = new Double[mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX];

                                //BG 24AUG12
                                clsUnit.eSystem pUnitSystem = Current_Bearing_Radial_FP_In.Unit.System;
                                mScrew_Spec = new clsScrew(pUnitSystem);

                                //....Screw.         //BG 24AUG12
                                mScrew_Spec.Type = "SHCS";
                                mScrew_Spec.Mat = "STL";
                            }

                        #endregion


                        #region "CLASS METHODS":


                            public Double HolesAngStart_Comp()
                            //=================================   //BG 24AUG12       
                            {
                                Double pAngStart_Comp = 0.0F;

                                if (mHolesEquiSpaced)
                                {
                                    Double pAngleOther = (360.0F / mHolesCount);

                                    int pCount = mHolesCount - 1;
                                    pAngStart_Comp = 360 - (mHolesAngStart + (pCount * pAngleOther));
                                }
                                else
                                {
                                    Double pAng_Other_Total = 0.0F;

                                    for (int i = 0; i < mHolesCount - 1; i++)
                                    {
                                        pAng_Other_Total = pAng_Other_Total + mHolesAngOther[i];
                                    }

                                    pAngStart_Comp = 360 - (mHolesAngStart + pAng_Other_Total);
                                }

                                return pAngStart_Comp;
                            }


                            #region "ICLONABLE METHOD:"

                                public object Clone()
                                //===================           //BG 08JUN12
                                {
                                    return this.MemberwiseClone();
                                }

                            #endregion

                        #endregion
                    }

                #endregion

            #endregion
        }
    }
}
