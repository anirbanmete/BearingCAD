//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_9_EDM_Pad         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  07MAY12                                '
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

namespace BearingCAD20
{
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsEDM_Pad
        //======================
        {
            #region "NAMED CONSTANTS:"
            //========================
                //private const Double mc_PAD_RFILLET = 0.030F;           //PB 13FEB12. To be checked later.

            #endregion


            #region "USER-DEFINED STRUCTURE:"
            //================================
                [Serializable]
                public struct sFillet
                {
                    public Double ID;
                    public Double Back;
                }

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private Double mRFillet_Back;
                private Double mAngStart_Web;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================ 

                public Double RFillet_Back
                {
                    get
                    {
                        if (mRFillet_Back < modMain.gcEPS)
                        {
                            mRFillet_Back = mCurrent_Bearing_Radial_FP.Pad.RFillet_ID();
                        }

                        return mRFillet_Back;
                    }

                    set { mRFillet_Back = value; }
                }


                public Double AngStart_Web
                {
                    get
                    {
                        //if (mAngStart_Web < modMain.gcEPS)
                        mAngStart_Web = Calc_AngStart_Web(modMain.gOpCond);
                        return mAngStart_Web;
                    }

                    set { mAngStart_Web = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsEDM_Pad(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //==================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                }

            #endregion


            #region "CLASS METHODS":

                public Double Ang_Offset(clsOpCond OpCond_In)
                //===========================================   
                {
                    //PB 25JAN12. This routine has a few conflicting/confusing relationship. 
                    //....Ref. Radial_Rev11_27OCT11: Col. IW

                    Double pVal = 0.0F;
                    Double pPadAngle    = mCurrent_Bearing_Radial_FP.Pad.Angle();
                    Double pPivotOffset = 0.0F;

                    if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                    {
                        pPivotOffset = mCurrent_Bearing_Radial_FP.Pad.Pivot.Offset;
                        pVal = ((100 - pPivotOffset) / 100) * pPadAngle;
                    }

                    else if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                        pVal = 0.5 * mCurrent_Bearing_Radial_FP.Pad.Angle();

                    return pVal;    //BG 07MAY12
                    //return modMain.MRound(pVal, 1);
                }


                private Double Calc_AngStart_Web(clsOpCond OpCond_In)
                //===================================================       
                {
                    //PB 25JAN12. This routine has a few conflicting/confusing relationship. To be discussed with HK. 
                    //....Ref. Radial_Rev11_27OCT11: Col. IY.

                    Double pPadCount = mCurrent_Bearing_Radial_FP.Pad.Count;
                    Double pPivotOffset = mCurrent_Bearing_Radial_FP.Pad.Pivot.Offset;
                    Double pPadAngle = mCurrent_Bearing_Radial_FP.Pad.Angle();

                    Double pVal = 0.0F;

                    if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                    {
                        pVal = 0.5 * (360 / pPadCount) + ((pPivotOffset - 50) / 100) * pPadAngle;
                    }

                    else if (OpCond_In.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                        pVal = 0.5 * (360 / pPadCount);

                    return pVal;
                }

            #endregion
        }

    }
}
