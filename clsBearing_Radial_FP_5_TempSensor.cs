//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_5_TempSensor      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB18APR12. Minor cleaning.
 
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

namespace BearingCAD21
{
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsTempSensor
        {
            #region "NAMED CONSTANTS:"
            //========================
                //private const Double mc_CAN_LENGTH = 0.15D;       //BG 02JUL13
                private const Double mc_CAN_LENGTH = 0.30D;         //BG 02JUL13

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private bool mExists;
                private Double mCanLength;
                private eFaceID mLoc;
                private int mCount;
                private Double mD;
                private Double mAngStart;         //....w.r.t Bearing Split Line.
                private Double mDepth;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                #region "NAMED CONSTANTS:"

                    public Double CAN_LENGTH
                    {
                        get { return mc_CAN_LENGTH; }
                    }

                #endregion


                public bool Exists
                {
                    get { return mExists; }
                    set { mExists = value; }
                }

                public Double CanLength
                {
                    get
                    {
                        if (mCanLength < modMain.gcEPS)
                            mCanLength = mc_CAN_LENGTH;
                        return mCanLength;
                    }

                    set { mCanLength = value; }
                }

                public eFaceID Loc
                {
                    get { return mLoc; }
                    set { mLoc = value; }
                }

                public int Count
                {
                    get { return mCount; }
                    set { mCount = value; }
                }

                public Double D
                {
                    get { return mD; }
                    set { mD = value; }
                }

                public Double AngStart
                {
                    get { return mAngStart; }
                    set { mAngStart = value; }
                }

                public Double Depth
                {
                    get
                    {
                        if (mDepth < modMain.gcEPS)
                            mDepth = Calc_Depth();

                        return mDepth;
                    }

                    set { mDepth = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsTempSensor(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //======================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;

                    //....Default Values:
                    //mExists = true;
                    mCount = 1;
                }

            #endregion


            #region "CLASS METHODS":

                public Double Calc_Depth()      //SG 16AUG12
                //========================
                {
                    //....Ref. Radial_Rev11_27OCT11: Col. GY
                    return 0.5 * (mCurrent_Bearing_Radial_FP.Pad.L + mCanLength);
                }

            #endregion
        }

    }
}
