
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_1_Pad             '
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
    
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsPad
        //==================
        {
            #region "NAMED CONSTANTS:"
            //=======================
                private const int mc_COUNT_PAD_MAX = 6;

            #endregion


            #region "ENUMERATION TYPES:"
            //==========================
                public enum eLoadPos { LBP, LOP };

            #endregion


            #region "USER-DEFINED STRUCTURES:"
            //================================

                #region "Pivot:"
                    [Serializable]
                    public struct sPivot
                    {
                        //....If Bidirectional, Offset = 50%. If not 50%, it refers to the pad angular extent 
                        //........from the leading edge to the pivot as a % of the Pad angle.
                        public Double Offset;      //....stored and displayed in %.  
                        public Double AngStart;
                        public Double AngStart_LBP_XLKMC;
                        public Double AngStart_LOP_XLKMC;
                    }
                #endregion


                #region "Thick:"

                //........If Pivot input checkbox is checked, user will input the corresponding thickness. 
                //............In that case, the thicknesses @ Lead & Trail  will be the same as that @ Pivot. 
                //........If unchecked, user will independently input all three thicknesses. 
                //
                    [Serializable]
                    public struct sT
                    {
                        public Double Lead;
                        public Double Pivot;
                        public Boolean Pivot_Checked;           //BG 26MAR13
                        public Double Trail;
                    }
                #endregion

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private eLoadPos mType;                
                private int mCount;
                private Double mL;
                //....Angle;                    //....Method (Retrieved).  
                //....Offset                    //....Method.
                //....RBack                     //....Method.                   
                //....RFillet_ID;               //....Method.

                private sPivot mPivot;
                private sT mT;
              
            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================  

                public int Count_Max
                {
                    get { return mc_COUNT_PAD_MAX; }
                }


                public eLoadPos Type
                {
                    get { return mType; }
                    set { mType = value; }
                }
                       
                public int Count
                {
                    get { return mCount; }
                    set
                    {
                        mCount = value;
                        //mOilInlet_Orifice_Count = mCount;     //PB 17FEB12
                    }
                }


                public Double L
                {
                    get { return mL; }
                    set { mL = value; }
                }


                //public Double RFillet
                //{
                //    get { return mRFillet; }
                //    set { mRFillet = value; }
                //}


                #region "PIVOT:"
                //--------------

                    public sPivot Pivot
                    {
                        get { return mPivot; }
                    }


                    public Double Pivot_Offset
                    {
                        set { mPivot.Offset = value; }
                    }


                    public Double Pivot_AngStart
                    {
                        set { mPivot.AngStart = value; }
                    }

                    //AM 14AUG12
                    public Double Pivot_AngStart_LBP_XLKMC
                    {
                        set { mPivot.AngStart_LBP_XLKMC = value; }
                    }

                    public Double Pivot_AngStart_LOP_XLKMC
                    {
                        set { mPivot.AngStart_LOP_XLKMC = value; }
                    }

                #endregion


                #region "T:"
                //---------

                    public sT T
                    {
                        get 
                        {
                            if (mT.Pivot < modMain.gcEPS)       //BG 16AUG12
                               mT.Pivot = TDef();

                            return mT; 
                        }
                    }


                    public Double T_Lead
                    {
                        set { mT.Lead = value; }
                    }


                    public Double T_Pivot
                    {
                        set 
                        { mT.Pivot = value; }
                    }

                    public Boolean T_Pivot_Checked          //BG 26MAR13
                    {
                        set
                        { mT.Pivot_Checked = value; }
                    }


                    public Double T_Trail
                    {
                        set{ mT.Trail = value; }
                    }

                #endregion


              


            #endregion


            #region "CONSTRUCTOR:"

                public clsPad(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //==============================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;

                    //....Default Values: 
                    mCount = 4;
                    mPivot.Offset = 50.0F;
                }

            #endregion


            #region "CLASS METHODS":

                public Double Angle()
                //====================
                {
                    //....Retrieve from database.

                    //....Retrieve from database.
                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    Double pAngle = 0.0f;

                    var pProject = (from pRec in pBearingDBEntities.tblDesign_Pad where pRec.fldQty == mCount select pRec.fldAngle).ToList();

                    if (pProject.Count > 0)
                    {
                        pAngle = Convert.ToDouble(pProject[0]);
                    }
                    return pAngle;
                }


                public Double Offset()
                //--------------------
                {
                    return (0.5F * (mCurrent_Bearing_Radial_FP.DPad() - mCurrent_Bearing_Radial_FP.DSet()));
                }


                //PB 09AUG11. Eventually, there should be a method: Pad_RBack (i as int). 
                // i = 0 ==> at Pivot
                // i = 1 ==> at Leading Edge. 
                // i = 2 ==> at Trailing Edge.

                public Double RBack()
                //====================
                {   //....Assumption: Uniform Pad Thickness.

                    return (Double)(0.5 * mCurrent_Bearing_Radial_FP.DSet() + mT.Pivot);
                }


                public Double RFillet_ID()      
                //========================
                {
                    //....Retrieve from database.
                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    Double pR = 0.0f;
                    Decimal pDSet = (Decimal)mCurrent_Bearing_Radial_FP.DSet();

                    var pProject = (from pRec in pBearingDBEntities.tblDesign_Pad_Fillet where pRec.fldDSet_LLim <= pDSet && pRec.fldDSet_ULim >= pDSet select pRec.fldR).ToList();

                    if (pProject.Count > 0)
                    {
                        pR = Convert.ToDouble(pProject[0]);
                    }
                    return pR;
                }


                public Double TDef()
                //------------------       
                {
                    Double pDShaft = mCurrent_Bearing_Radial_FP.DShaft();
                    Double pT = 0.0F;

                    if (pDShaft > modMain.gcEPS)
                        pT = (0.15F * pDShaft);

                    return pT;
                }


                public Double AngBetween()
                //========================
                {
                    //....Angle between:
                    //          1. Pivot Locations. 
                    //          2. Temp. Sensor Holes.

                    return (360.0F / mCount);
                }

            
                public Double[] Pivot_OtherAng()
                //==============================    
                {
                    Double[] pOtherAng = new Double[mCount];

                    for (int i = 0; i < mCount; i++)
                    {
                        pOtherAng[i] = mPivot.AngStart + (i * AngBetween());
                    }
                    return pOtherAng;
                }


                public Double Set_Pivot_AngStart()
                //================================      //AM 14AUG12  
                {
                    Double pPivot_AngStart = 0.0F;

                    if (mType == eLoadPos.LBP)
                    {
                        if (mPivot.AngStart < modMain.gcEPS && mPivot.AngStart_LBP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LBP_XLKMC;
                        }
                        else if (mPivot.AngStart_LBP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LBP_XLKMC;
                        }
                        else if (!(Math.Abs(mPivot.AngStart_LBP_XLKMC - mPivot.AngStart) < modMain.gcEPS) && 
                                 !(Math.Abs(mPivot.AngStart_LOP_XLKMC - mPivot.AngStart) < modMain.gcEPS) && 
                                 mPivot.AngStart > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart;
                        }
                        else if (mPivot.AngStart_LBP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LBP_XLKMC;
                        }
                        else
                        {
                            pPivot_AngStart = mPivot.AngStart;
                        }                       
                    }
                    else if (mType == eLoadPos.LOP)
                    {
                        if (mPivot.AngStart < modMain.gcEPS && mPivot.AngStart_LOP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LOP_XLKMC;
                        }
                        else if (mPivot.AngStart_LOP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LOP_XLKMC;
                        }
                        else if (!(Math.Abs(mPivot.AngStart_LOP_XLKMC - pPivot_AngStart) < modMain.gcEPS) &&
                                 !(Math.Abs(mPivot.AngStart_LBP_XLKMC - mPivot.AngStart) < modMain.gcEPS)
                                 && mPivot.AngStart > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart;
                        }
                        else if (mPivot.AngStart_LOP_XLKMC > modMain.gcEPS)
                        {
                            pPivot_AngStart = mPivot.AngStart_LOP_XLKMC;
                        }
                        else
                        {
                            pPivot_AngStart = mPivot.AngStart;
                        }
                    }

                    return pPivot_AngStart;

                }

            #endregion
        }
    
    }
}
