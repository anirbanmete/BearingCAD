//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_6_MillRelief      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  25APR12                                '
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
        public class clsMillRelief
        {
            #region "NAMED CONSTANTS:"
            //========================

                //DESIGN PARAMETERS:
                //------------------
                //....EDM Relief (used in Main Class & clsOilInlet).
                private const Double mc_DESIGN_EDM_RELIEF = 0.010D;         //BG 06DEC12

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================

                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private bool mExists;
                public string mD_Desig;
                //....D                      //....Method.   
                //....D_PadRelief            //....Method.

                private Double[] mEDM_Relief = new Double[2];           //BG 06DEC12

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                #region "NAMED CONSTANTS:"
              
                    public Double DESIGN_EDM_RELIEF
                    //=============================    //BG 06DEC12
                    {
                        get { return mc_DESIGN_EDM_RELIEF; }
                    }

                #endregion


                public bool Exists
                {
                    get { return mExists; }
                    set { mExists = value; }
                }

                public string D_Desig
                {
                    get { return mD_Desig; }

                    set
                    {
                        mD_Desig = value;

                        //if (mMillRelief.D_Desig!=null)                        Review SG                  
                        //mMillRelief.D = clsDrill.D(mMillRelief.D_Desig);

                        //if (mMillRelief.D!=0.0F)
                        //        D_PadRelief();
                    }
                }

                #region "EDM Relief:"

                    public Double[] EDM_Relief          //BG 06DEC12      
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mEDM_Relief[i] < modMain.gcEPS)
                                {
                                    mEDM_Relief[i] = mc_DESIGN_EDM_RELIEF;
                                }
                            }
                            return mEDM_Relief;
                        }
                        //set { mEDM_Relief = value; }
                    }

                #endregion

            #endregion


            #region "CONSTRUCTOR:"

                public clsMillRelief(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //======================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                }

            #endregion


            #region "CLASS METHODS":

                public Double D()
                //================
                {
                    if (mD_Desig != null && mD_Desig != "")     
                    {
                        return clsDrill.D(mD_Desig);
                    }
                    else
                        return 0;
                }


                public Double D_PadRelief()
                //==========================
                {   // Ref: Design Table - "Radail_Rev5_30MAR09.xls": Col. DJ. 

                    Double pRBack = mCurrent_Bearing_Radial_FP.Pad.RBack();
                    Double pWeb_H = mCurrent_Bearing_Radial_FP.FlexurePivot.Web.H;
                    Double pWeb_RFillet = mCurrent_Bearing_Radial_FP.FlexurePivot.Web.RFillet;
                    Double pWeb_T = mCurrent_Bearing_Radial_FP.FlexurePivot.Web.T;

                    Double pD = 0.0;


                    if (mExists == true)
                    {
                        pD = 2 * (pRBack + 0.034);
                    }

                    else if (mExists == false)
                    {
                        //....IO3: Web Length Radius.
                        Double pX1, pX2;
                        pX1 = pRBack + pWeb_H - pWeb_RFillet;
                        pX2 = 0.5 * pWeb_T + pWeb_RFillet;

                        Double pIO3;
                        pIO3 = Math.Sqrt(Math.Pow(pX1, 2) + Math.Pow(pX2, 2));

                        pD = 2 * (pIO3 + pWeb_RFillet) + 0.04;
                    }

                    return (Double)(pD);
                }

            #endregion
        }

    }
}
