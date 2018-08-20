//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_8_AntiRotPin      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  05OCT12                                '
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
        public class clsAntiRotPin
        {
            #region "ENUMERATION TYPES:"
            //==========================
                public enum eLoc_Casing_SL { Offset, Center };
                public enum eLoc_Bearing_Vert { L, R };
                public enum eLoc_Bearing_SL { Top, Bottom };
                public enum eInsertedOn { FD, F };              //BG 24APR12

            #endregion


            #region "MEMBER VARIABLES:"
            //==========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                //....Location variables:
                private Double mLoc_Dist_Front;
                private Double mLoc_Angle;                          //....It may become a method in a later version 

                //........W.R.T Flange
                private eInsertedOn mInsertedOn; 


                //........W.R.T Casing S/L.
                    private eLoc_Casing_SL mLoc_Casing_SL;
                    private Double mLoc_Offset;

                //........W.R.T Bearing Vertical.
                private eLoc_Bearing_Vert mLoc_Bearing_Vert;
                private eLoc_Bearing_SL   mLoc_Bearing_SL;            //BG 08MAY12 


                //........(Ref.: Angular Orientation Utility).
                private clsPin mSpec;
                private Double mDepth;
                private Double mStickout;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                public Double Loc_Dist_Front
                {
                    get { return mLoc_Dist_Front; }
                    set { mLoc_Dist_Front = value; }
                }

                public Double Loc_Angle
                {
                    get { return mLoc_Angle; }
                    set { mLoc_Angle = value; }
                }

                public eInsertedOn InsertedOn
                {
                    get { return mInsertedOn; }
                    set { mInsertedOn = value; }
                }

                public eLoc_Casing_SL Loc_Casing_SL
                {
                    get { return mLoc_Casing_SL; }
                    set { mLoc_Casing_SL = value; }
                }

                public Double Loc_Offset
                {
                    get 
                    {
                        if (mLoc_Offset < modMain.gcEPS)
                            mLoc_Offset = mSpec.D;

                        return mLoc_Offset; 
                    }

                    set { mLoc_Offset = value; }
                }

                public eLoc_Bearing_Vert Loc_Bearing_Vert
                {
                    get { return mLoc_Bearing_Vert; }
                    set { mLoc_Bearing_Vert = value; }
                }

                public eLoc_Bearing_SL Loc_Bearing_SL     //BG 08MAY12
                {
                    get { return mLoc_Bearing_SL; }
                    set { mLoc_Bearing_SL = value; }
                }
     
                public clsPin Spec
                {
                    get { return mSpec; }
                    set { mSpec = value; }
                }

                public Double Depth
                {
                    get
                    {
                        if (mDepth < modMain.gcEPS)
                        {
                            mDepth = Depth_DefVal ();
                            mStickout = Calc_Stickout ();               //....Recalculate Stickout.
                            return mDepth;
                        }

                        else
                            return mDepth;
                    }

                    set 
                    {   mDepth = value;
                        if (mDepth > modMain.gcEPS)
                            mStickout = Calc_Stickout(); 
                    }
                }


                public Double Stickout
                {
                    get
                    {
                        if (mStickout < modMain.gcEPS)
                            return Calc_Stickout();
                        else
                            return mStickout;
                    }

                    set 
                    {   mStickout = value;
                        if (mStickout > modMain.gcEPS)
                            mDepth= Calc_Depth();                        //....Recalculate Depth.
                    }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsAntiRotPin(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //=====================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;

                    mSpec = new clsPin(mCurrent_Bearing_Radial_FP.Unit.System);         //PB 18FEB12. May need to be changed.
                    mSpec.Type = "SHCS";
                    mSpec.Mat = "STL";
                }

            #endregion


            #region "CLASS METHODS":

                //BG 08MAY12. This method is supressed now and will be implemented in the next version.
                //public eLoc_Bearing_SL Loc_Bearing_SL()
                ////====================================== 
                //{
                //    //....This parameter will be determined by the Casing S.L angle w.r.t the Bearing S.L &
                //    //........the Loc_From_BearingVert. 
                //    //
                //    //........The Casing S.L angle (= mLoc.Angle) is measured from the Bearing S.L 
                //    //............and considered +ive in the CCW direction.
                //    //
                //    //....The following logic will break down if the ABS(mLoc.Angle) < a certain threshold value.


                //    //....Initialize.
                //    eLoc_Bearing_SL pLoc = eLoc_Bearing_SL.Top;


                //    if (mLoc_Angle < 0)
                //    {
                //        if (mLoc_Bearing_Vert == eLoc_Bearing_Vert.L)
                //        {
                //            pLoc = eLoc_Bearing_SL.Top;
                //        }
                //        else if (mLoc_Bearing_Vert == eLoc_Bearing_Vert.R)
                //        {
                //            pLoc = eLoc_Bearing_SL.Bottom;
                //        }
                //    }
                //    else
                //    {
                //        if (mLoc_Bearing_Vert == eLoc_Bearing_Vert.L)
                //        {
                //            pLoc = eLoc_Bearing_SL.Bottom;
                //        }
                //        else if (mLoc_Bearing_Vert == eLoc_Bearing_Vert.R)
                //        {
                //            pLoc = eLoc_Bearing_SL.Top;
                //        }
                //    }

                //    return pLoc;
                //}


                private Double Depth_DefVal ()
                //============================
                {
                    //....Ref. Radial_Rev11_27OCT11: Col. GI
                    return mSpec.D;
                }


                private Double Calc_Depth ()
                //=============================
                {
                    Double pL = 0.0F;

                    //if (mCurrent_Bearing_Radial_FP.Unit.System == clsUnit.eSystem.English)
                    if (mSpec.Unit.System == clsUnit.eSystem.English)
                        pL = mSpec.L;

                    //else if (mCurrent_Bearing_Radial_FP.Unit.System == clsUnit.eSystem.Metric)
                    else if (mSpec.Unit.System == clsUnit.eSystem.Metric)
                        pL = mSpec.L / 25.4;

                    Double pDepth = 0.0F;
                    pDepth = pL - mStickout;

                    return pDepth;
                }


                public Double Calc_Stickout()        
                //===========================
                {
                    //....Ref. Radial_Rev11_27OCT11: Col. GJ. We should export Stickout to Design Table not Calc_Stickout.
                    Double pL = 0.0F;

                    //if (mCurrent_Bearing_Radial_FP.Unit.System == clsUnit.eSystem.English)
                    if (mSpec.Unit.System == clsUnit.eSystem.English)
                        pL = mSpec.L;

                    // else if (mCurrent_Bearing_Radial_FP.Unit.System == clsUnit.eSystem.Metric)
                    else if (mSpec.Unit.System == clsUnit.eSystem.Metric)
                        pL = mSpec.L / 25.4;

                    Double pStickout = 0.0F;
                    pStickout = pL - mDepth;

                    return pStickout;
                }

            #endregion
        }
    }
}
