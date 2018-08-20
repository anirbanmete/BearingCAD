//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_11_PerformData    '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  29JAN13                                '
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
        #region "Class Performance Data":
            [Serializable]
            public class clsPerformData
            //=========================
            {
                #region "USER-DEFINED STRUCTURES:"
                //================================
                    [Serializable]
                    public struct sPadMax
                    {
                        public Double Temp;
                        public Double Press;
                        public Double Rot;
                        public Double Load;
                        public Double Stress;
                    }

                #endregion


                #region "MEMBER VARIABLES:"
                //=========================

                    private Double mPower_HP;
                    private Double mFlowReqd_gpm;
                    private Double mTempRise_F;
                    private Double mTFilm_Min;

                    //....Pad Maximums:
                    private sPadMax mPadMax;

                #endregion


                #region "CLASS PROPERTY ROUTINES:"
                //=================================

                    //.... Power (Eng Unit).
                    public Double Power_HP
                    {
                        get { return mPower_HP; }
                        set 
                        {   mPower_HP = value;
                            mTempRise_F = Calc_TempRise_F();
                        }
                    }


                    //.... Flow Reqd (Eng Unit).
                    public Double FlowReqd_gpm
                    {
                        get { return mFlowReqd_gpm; }
                        set 
                        { 
                            mFlowReqd_gpm = value;
                            mTempRise_F = Calc_TempRise_F();
                        }
                    }

                    //.... Temp Rise (Eng Unit).
                    public Double TempRise_F
                    {
                        get
                        {
                            if (Math.Abs(mTempRise_F) < modMain.gcEPS)
                                mTempRise_F = Calc_TempRise_F();

                                return mTempRise_F; 
                        }


                        set { mTempRise_F = value; }
                    }

                    //.... TFilm_Min (Eng Unit).
                    public Double TFilm_Min
                    {
                        get { return mTFilm_Min; }
                        set { mTFilm_Min = value; }
                    }


                    //....Pad Maximums:
                    public sPadMax PadMax
                    {
                        get { return mPadMax; }
                    }


                    public Double PadMax_Temp
                    {
                        set { mPadMax.Temp = value; }
                    }

                    public Double PadMax_Press
                    {
                        set { mPadMax.Press = value; }
                    }

                    public Double PadMax_Rot
                    {
                        set { mPadMax.Rot = value; }
                    }

                    public Double PadMax_Load
                    {
                        set { mPadMax.Load = value; }
                    }

                    public Double PadMax_Stress
                    {
                        set { mPadMax.Stress = value; }
                    }

                #endregion


                #region "CONSTRUCTOR:"

                    public clsPerformData()
                    //=====================
                    {

                    }

                #endregion


                #region "CLASS METHODS:"

                    public Double Calc_TempRise_F ()
                    //==============================
                    {
                        Double pTempRise = 0.0;

                        if(mFlowReqd_gpm != 0.0)        //BG 29JAN13
                        {
                           pTempRise = 12.4 * (mPower_HP / mFlowReqd_gpm);
                        }
                       
                        return pTempRise;
                    }

                #endregion


            }

        #endregion
    }
}
