
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsMaterial                            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  02APR12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================
//PB 03FEB12. SG, to be completed.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace BearingCAD20
{
    [Serializable]              
    public class clsMaterial
    {
        ////....Lining.
        //[Serializable]          
        //public struct sLining
        //{
        //    public bool Exists;
        //    public string Lining;
        //    //public Double T;          
        //}

        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            private string mBase;
            //private sLining mLining;        
            private string mLining;
            private bool mLiningExists;

        #endregion


        #region "CLASS PROPERTY ROUTINES"

            public String Base
            {                
                get { return mBase; }
                set { mBase = value; }
            }                   


            public string Lining
            {
                get { return mLining; }
                set { mLining = value; }
            }

           
            public bool LiningExists
            {
                get { return mLiningExists; }
                set { mLiningExists = value; }
            }
            
        #endregion


        #region "CLASS METHODS"
            //====================

            public void GetBaseMaterial()
            //============================
            {

            }

            //public string MatCode(string Mat_In, string WaukeshaCode_Out)
            public string MatCode(string Mat_In)
            //===================================
            {
                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                //pstrFIELDS = "fldCode_UNS, fldCode_ProfitKey ";
                pstrFIELDS = "fldCode_Waukesha ";
                pstrFROM = "FROM tblData_Mat ";
                pstrWHERE = "WHERE fldName = '" + Mat_In + "'";
                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                
                SqlConnection pConnection = new SqlConnection();

                SqlDataReader pDR = null;
                pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

                //string pCode_UNS = "", pCode_ProfitKey = "";
                string pWaukeshaCode = "";

                if (pDR.Read())
                {
                    //pCode_ProfitKey = modMain.gDB.CheckDBString(pDR, "fldCode_ProfitKey");
                    pWaukeshaCode = modMain.gDB.CheckDBString(pDR, "fldCode_Waukesha");
                }
                pDR.Close();
                pConnection.Close();

                //txtCodeUNS_In.Text = pCode_UNS;
                //txtCodeProfitKey_In.Text = pCode_ProfitKey;
               // WaukeshaCode_Out = pWaukeshaCode;
                return pWaukeshaCode;
            }

        #endregion
    }





   

    
}
