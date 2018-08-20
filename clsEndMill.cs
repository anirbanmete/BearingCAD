//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsEndMill                             '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace BearingCAD20
{
     [Serializable]
    public class clsEndMill
    {

        #region "MEMBER VARIABLES:"
        //=========================
            public enum eType { Flat, Ball, Chamfer };
            private eType mType; 
            private string mD_Desig;          
        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================

            public eType Type
            {
                get { return mType; }
                set { mType = value; }
            }

            public string D_Desig
            {
                get { return mD_Desig; }
                set { mD_Desig = value;}
            }

        #endregion


        #region "CLASS METHODS:"
        //====================

            public Double D()
            //================
            {
                Double pVal = 0;
                Double pNumerator = 0;
                Double pDenominator = 0;
                if (mD_Desig == null || mD_Desig == "") return 0;

                string pD_Desig = mD_Desig;
                pD_Desig = pD_Desig.Remove(pD_Desig.Length - 1, 1);

                if (pD_Desig.Contains("/"))
                {
                    if (pD_Desig.Contains("-"))
                    {
                        string[] pTemp_D_Desig = pD_Desig.Split('-');
                        Double pPrime_Num = modMain.ConvTextToInt(pTemp_D_Desig[0]);

                        pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pTemp_D_Desig[1], "/"));
                        pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pTemp_D_Desig[1], "/"));

                        pVal = pPrime_Num + (pNumerator / pDenominator);
                    }
                    else
                    {
                        pNumerator = modMain.ConvTextToDouble(modMain.ExtractPreData(pD_Desig, "/"));
                        pDenominator = modMain.ConvTextToDouble(modMain.ExtractPostData(pD_Desig, "/"));

                        pVal = pNumerator / pDenominator;
                    }
                }
                else
                    pVal = modMain.ConvTextToInt(pD_Desig);

                return pVal;
            }

        #endregion
    }
}
