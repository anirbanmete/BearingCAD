//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_7_SL              '
//                                       Sub Class: S/L Hardware                '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  27JUL18                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 16APR12. Some cleaning. No instructions.

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
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {     
        [Serializable]
        public class clsSL
        {
            #region "USER-DEFINED STRUCTURES:"
            //================================
                [Serializable]
                public struct sLoc
                {
                    public Double Center;
                    public Double Front;
                }

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                #region "Screw:"                    
                    private clsScrew mScrew_Spec;
                    private sLoc mLScrew_Loc;
                    private sLoc mRScrew_Loc;
                    private Double mThread_Depth;
                    private Double mCBore_Depth;

                #endregion


                #region "Dowel:"                   
                    private clsPin mDowel_Spec;
                    private sLoc mLDowel_Loc;
                    private sLoc mRDowel_Loc;
                    private Double mDowel_Depth;

                #endregion

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                #region "Screw:"
                //--------------
            
                    #region "Spec:"
                    //------------
                        public clsScrew Screw_Spec
                        {
                            get { return mScrew_Spec; }
                            set { mScrew_Spec = value; }
                        }
                    #endregion


                    #region "Locations:"
                    //-----------------
                    //

                        #region "LScrew:"
                        //---------------

                            public sLoc LScrew_Loc
                            {
                                get
                                {
                                    if (mLScrew_Loc.Center < modMain.gcEPS)
                                        mLScrew_Loc.Center = Calc_Screw_Loc_Center();

                                    if (mLScrew_Loc.Front < modMain.gcEPS)
                                        mLScrew_Loc.Front = Calc_Screw_Loc_Front("LScrew");
                                    
                                    return mLScrew_Loc;
                                }
                            }


                            public Double LScrew_Loc_Center
                            {
                                set { mLScrew_Loc.Center = value; }
                            }

                            public Double LScrew_Loc_Front
                            {
                                set { mLScrew_Loc.Front = value; }
                            }

                        #endregion


                        #region "RScrew:"
                        //---------------

                            public sLoc RScrew_Loc
                            {
                                get
                                {
                                    if (mRScrew_Loc.Center < modMain.gcEPS)
                                        mRScrew_Loc.Center = Calc_Screw_Loc_Center();

                                    if (mRScrew_Loc.Front < modMain.gcEPS)
                                        mRScrew_Loc.Front = Calc_Screw_Loc_Front("RScrew");

                                    return mRScrew_Loc;
                                }
                            }


                            public Double RScrew_Loc_Center
                            {
                                set { mRScrew_Loc.Center = value; }
                            }

                            public Double RScrew_Loc_Front
                            {
                                set { mRScrew_Loc.Front = value; }
                            }

                        #endregion

                    #endregion


                    #region"Depths:"
                    //-------------

                        #region "Thread:"
                        //---------------

                            public Double Thread_Depth                      //....Read-only.
                            {
                                get
                                {
                                    //if (mThread_Depth < modMain.gcEPS)
                                    return Calc_Thread_Depth();
                                    //else
                                    //    return mThread_Depth;
                                }

                                //set { mThread_Depth = value; }
                            }

                        #endregion


                        #region "CBore:"
                        //---------------

                            public Double CBore_Depth                       //....Read-only.
                            {
                                get
                                {
                                    //if (mCBore_Depth < modMain.gcEPS)
                                    return Calc_CBore_Depth();
                                    //else
                                    //    return mCBore_Depth;
                                }

                                //set { mCBore_Depth = value; }
                            }

                        #endregion

                    #endregion

                #endregion


                #region "Dowel:"
                //--------------      

                    #region "Spec:"
                    //------------
                        public clsPin Dowel_Spec
                        {
                            get { return mDowel_Spec; }
                            set { mDowel_Spec = value; }
                        }
                    #endregion


                    #region "Locations":
                    //-----------------

                        #region "LDowel:"
                        //---------------

                            public sLoc LDowel_Loc
                            {
                                get
                                {
                                    if (mLDowel_Loc.Center < modMain.gcEPS)
                                        mLDowel_Loc.Center = Calc_LDowel_Loc_Center();

                                    if (mLDowel_Loc.Front < modMain.gcEPS)
                                        mLDowel_Loc.Front = Calc_LDowel_Loc_Front();

                                    return mLDowel_Loc;
                                }
                            }


                            public Double LDowel_Loc_Center
                            {
                                set { mLDowel_Loc.Center = value; }
                            }

                            public Double LDowel_Loc_Front
                            {
                                set { mLDowel_Loc.Front = value; }
                            }

                        #endregion


                        #region "RDowel:"
                        //---------------

                            public sLoc RDowel_Loc
                            {
                                get
                                {
                                    if (mRDowel_Loc.Center < modMain.gcEPS)
                                        mRDowel_Loc.Center = Calc_RDowel_Loc_Center();

                                    if (mRDowel_Loc.Front < modMain.gcEPS)
                                        mRDowel_Loc.Front = Calc_RDowel_Loc_Front();

                                    return mRDowel_Loc;
                                }
                            }


                            public Double RDowel_Loc_Center
                            {
                                set { mRDowel_Loc.Center = value; }
                            }

                            public Double RDowel_Loc_Front
                            {
                                set { mRDowel_Loc.Front = value; }
                            }

                        #endregion

                    #endregion


                    #region "Depths":
                    //--------------
            
                        public Double Dowel_Depth                   //....Read-only.
                        {
                            get
                            {
                                //if (mDowel_Depth < modMain.gcEPS)
                                return Calc_Dowel_Depth();
                                //else
                                //    return mDowel_Depth;
                            }

                            //set { mDowel_Depth = value; }
                        }

                    #endregion

                #endregion

            #endregion


            #region "CONSTRUCTOR:"

                public clsSL(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //=============================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;

                    //....Screw.
                    //
                        mScrew_Spec = new clsScrew(mCurrent_Bearing_Radial_FP.Unit.System);

                        mScrew_Spec.Type = "SHCS";
                        mScrew_Spec.Mat  = "STL";

                    //....Dowel.
                    //
                        mDowel_Spec = new clsPin(mCurrent_Bearing_Radial_FP.Unit.System);

                        mDowel_Spec.Type = "P";
                        mDowel_Spec.Mat  = "STL";
                }

            #endregion


            #region "CLASS METHODS":

                #region "SCREW:"
                //-------------

                    #region "Locations:"
                    //------------------

                        public Double Calc_Screw_Loc_Center()
                        //=====================================
                        {
                            //....Commmon for RScrew & LScrew.
                            //
                            //........Ref. Radial_Rev11_27OCT11: Col. M & O.
                            //........Bearing Fit Dia: Col. CR

                            Double pDFit = mCurrent_Bearing_Radial_FP.DFit();
                            Double pD_CBore = mScrew_Spec.D_CBore;               //....Col. AG
                            Double pLoc = 0.5F * (pDFit - pD_CBore) - 0.1;

                            return modMain.MRound(pLoc, 0.05);
                        }

                       
                         public Double Calc_Screw_Loc_Front(string Screw_In)
                        //==================================================        
                        {
                            //....Used for both RScrew & LScrew.
                            //
                            //....Ref. Radial_Rev11_27OCT11: Col. N & Col. P.
                            //
                            Double pAR_Pin_Loc_Dist_Front = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Dist_Front;               //....Col. FY.
                            string pAR_Pin_Loc_Bearing_V  = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();  //....Col. FV.

                            Double pDimStart     = mCurrent_Bearing_Radial_FP.DimStart_FrontFace;                               //....Col. CQ.
                            Double pAnnulus_LocB = mCurrent_Bearing_Radial_FP.OilInlet.Annulus.Loc_Back;                        //....Col. CU

                            Double pDepthF = mCurrent_Bearing_Radial_FP.Depth_EndConfig[0];                                     //....Col. CZ.
                            Double pDepthB = mCurrent_Bearing_Radial_FP.Depth_EndConfig[1];                                     //....Col. DA.

                            Double pBearingL = mCurrent_Bearing_Radial_FP.L;

                            Double pVal1 = modMain.MRound(pDimStart + 0.5 * (pAnnulus_LocB + pDepthF), 0.01F);
                            Double pVal2 = modMain.MRound(pDimStart + pBearingL - 0.5 * (pAnnulus_LocB + pDepthB), 0.01F);

                            Double pVal = 0.0F;


                            switch (Screw_In)
                            {
                                case "RScrew":
                                //------------

                                    if (((0.5F * pBearingL) > pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "L") ||
                                        ((0.5F * pBearingL) < pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "R"))
                                    {
                                        pVal = pVal1;
                                    }
                                    else
                                    {
                                        pVal = pVal2;
                                    }
                                
                                    break;


                                case "LScrew":
                                //------------

                                    if (((0.5F * pBearingL) > pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "R") ||
                                        ((0.5F * pBearingL) < pAR_Pin_Loc_Dist_Front && pAR_Pin_Loc_Bearing_V == "L"))
                                    {
                                        pVal = pVal1;
                                    }
                                    else
                                    {
                                        pVal = pVal2;
                                    }
                                
                                    break;
                            }              

                            return pVal;
                        }
            
                    #endregion


                    #region "DEPTHS:"
                    //---------------

                        #region "Thread:"
                        //--------------

                            private Double Calc_Thread_Depth()
                            //=================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. AL.
                                Double pD = mScrew_Spec.D;                      //....Col. X
                                return modMain.MRound(2.0 * pD, 0.0625);
                            }

                        #endregion


                        #region "CBore:"
                        //--------------

                            private Double Calc_CBore_Depth()
                            //================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. AO
                                Double pL = 0.0F;

                                if (mScrew_Spec.Unit.System == clsUnit.eSystem.Metric)                      
                                    pL = mScrew_Spec.L / 25.4;                  //....mm ==> in
                                else
                                    pL = mScrew_Spec.L;                         //....Col. AP

                                return (pL - (1.5 * mScrew_Spec.D));
                            }

                        #endregion

                    #endregion


                    #region "Length - Lower Limit:"
                    //-----------------------------

                            public Double Thread_L_LowerLimit()
                            //=====================================   AES 04JUL18
                            {
                                //....The routine output unit:      //PB 09JUN09. SB, change elsewhere.
                                //........UnitSystem = English: in.
                                //........           = Metric:  mm.
                                BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                                Double pL_LowerLimit = 0.0F;

                                Double pDThread = 0.0F;

                                //....If     Unit = English, DThread = in.
                                //....ElseIf Unit = Metric,  DThread = mm.
                                //
                                var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_D where pRec.fldD_Desig == mScrew_Spec.D_Desig select pRec).ToList();

                                if (pProject.Count > 0)
                                    pDThread = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble(pProject[0].fldD);


                                if (mScrew_Spec.Unit.System == clsUnit.eSystem.English)
                                {
                                    //SB 31JUL09

                                    //...To be done later. 
                                    //....Refer to "Radial_Rev7_10JUL09.xls", column AR (English). 

                                    //....MROUND((AN3+0.25)-Z3,0.125)

                                    Double pZ3, pAN3;
                                    pZ3 = pDThread / 25.4;                          //....Convert to in. 
                                    pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);

                                    pL_LowerLimit = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);
                                }

                                else if (mScrew_Spec.Unit.System == clsUnit.eSystem.Metric)
                                {

                                    //....Refer to "Radial_Rev5_30MAR05", column AS (Metric). 
                                    Double pZ3, pAN3, pAR3, pAQ3;
                                    pZ3 = pDThread / 25.4;                          //....Convert to in. 

                                    pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);
                                    pAR3 = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);

                                    pAQ3 = pAR3 - pAN3 + pZ3;

                                    pL_LowerLimit = (pAN3 + pAQ3 - pZ3) * 25.4;    //....Convert back to mm.            
                                    pL_LowerLimit = modMain.MRound(pL_LowerLimit, 1);
                                }
                                return pL_LowerLimit;
                            }

                        ////public Double Thread_L_LowerLimit()
                        //////==================================
                        ////{
                        ////    //....The routine output unit:      //PB 09JUN09. SB, change elsewhere.
                        ////    //........UnitSystem = English: in.
                        ////    //........           = Metric:  mm.
                        ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                        ////    Double pL_LowerLimit = 0.0F;

                        ////    Double pDThread = 0.0F;

                        ////    //....If     Unit = English, DThread = in.
                        ////    //....ElseIf Unit = Metric,  DThread = mm.
                        ////    //
                        ////    var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_D where pRec.fldD_Desig == mScrew_Spec.D_Desig select pRec).ToList();

                        ////    if (pProject.Count > 0)
                        ////        pDThread = mCurrent_Bearing_Radial_FP.mDB.CheckDBDouble((Double)pProject[0].fldD);


                        ////    if (mScrew_Spec.Unit.System == clsUnit.eSystem.English)
                        ////    {
                        ////        //SB 31JUL09

                        ////        //...To be done later. 
                        ////        //....Refer to "Radial_Rev7_10JUL09.xls", column AR (English). 

                        ////        //....MROUND((AN3+0.25)-Z3,0.125)

                        ////        Double pZ3, pAN3;
                        ////        pZ3 = pDThread / 25.4;                          //....Convert to in. 
                        ////        pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);

                        ////        pL_LowerLimit = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);
                        ////    }

                        ////    else if (mScrew_Spec.Unit.System == clsUnit.eSystem.Metric)
                        ////    {

                        ////        //....Refer to "Radial_Rev5_30MAR05", column AS (Metric). 
                        ////        Double pZ3, pAN3, pAR3, pAQ3;
                        ////        pZ3 = pDThread / 25.4;                          //....Convert to in. 

                        ////        pAN3 = modMain.MRound(2.5 * pZ3, 0.0625);
                        ////        pAR3 = modMain.MRound(pAN3 + 0.25 - pZ3, 0.125);

                        ////        pAQ3 = pAR3 - pAN3 + pZ3;

                        ////        pL_LowerLimit = (pAN3 + pAQ3 - pZ3) * 25.4;    //....Convert back to mm.            
                        ////        pL_LowerLimit = modMain.MRound(pL_LowerLimit, 1);
                        ////    }
                        ////    return pL_LowerLimit;
                        ////}

                    #endregion

                #endregion


                #region "PIN:"
                //-----------    

                    #region "Locations:"
                    //------------------  

                        #region "LDowel:"
                        //---------------  

                            //private Double Calc_LDowel_Loc_Center()
                             public Double Calc_LDowel_Loc_Center()     //BG 27NOV12
                            //======================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. BX

                                //........SplitLine_LScrew_Loc.Center: Col. O
                                //........AntiRot_Pin_Spec.D :         Col. GC

                                string pARPin_Loc = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();   //....Col. FV.
                                Double pVal = 0.0F;

                                if (pARPin_Loc == "L")
                                    pVal = mLScrew_Loc.Center - mCurrent_Bearing_Radial_FP.AntiRotPin.Spec.D;

                                else if (pARPin_Loc == "R")
                                    pVal = mLScrew_Loc.Center;

                                return modMain.MRound(pVal, 0.05);
                            }


                            //private Double Calc_LDowel_Loc_Front()
                             public Double Calc_LDowel_Loc_Front()
                             //=====================================     //BG 27NOV12
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. BY.
                                return Calc_Screw_Loc_Front("RScrew");         //....Col. N
                            }

                        #endregion


                        #region "RDowel:"
                        //---------------  

                            //private Double Calc_RDowel_Loc_Center()
                            public Double Calc_RDowel_Loc_Center()         //BG 27NOV12
                            //======================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. BV   

                                //........SplitLine_RScrew_Loc.Center : Col. M.
                                //........AntiRot_Pin_Spec.D          : Col. GC.

                                string pAR_Pin_Loc = mCurrent_Bearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert.ToString();   //....Col. FV.

                                Double pVal = 0.0F;

                                if (pAR_Pin_Loc == "R")
                                    pVal = mRScrew_Loc.Center - mCurrent_Bearing_Radial_FP.AntiRotPin.Spec.D;

                                else if (pAR_Pin_Loc == "L")
                                    pVal = mRScrew_Loc.Center;

                                return modMain.MRound(pVal, 0.05);
                            }


                            //private Double Calc_RDowel_Loc_Front()
                            public Double Calc_RDowel_Loc_Front()          //BG 27NOV12
                            //=====================================
                            {
                                //....Ref. Radial_Rev11_27OCT11: Col. BW.
                                return Calc_Screw_Loc_Front("LScrew");   //....Col. P
                            }

                        #endregion

                    #endregion


                    #region "Depth:"
                    //---------------

                        private Double Calc_Dowel_Depth()
                        //================================
                        {
                            //....Ref. Radial_Rev11_27OCT11: Col. CI
                            Double pL = 0.0F;

                            if (mDowel_Spec.Unit.System == clsUnit.eSystem.English)
                                pL = mDowel_Spec.L;             //....Col. CF

                            else if (mDowel_Spec.Unit.System == clsUnit.eSystem.Metric)
                                pL = mDowel_Spec.L / 25.4;

                            return modMain.MRound((0.5 * pL + 0.05), 0.01);
                        }

                    #endregion


                    public Double Pin_L_LowerLimit()
                    //===============================
                    {
                        return (2.0F * mDowel_Spec.D);
                    }


                #endregion

            #endregion
        }
    }
}
