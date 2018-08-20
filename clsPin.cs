//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsPin                                 '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  23APR12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 18APR12. Just cleaned up. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace BearingCAD20
{
     [Serializable]
    public class clsPin             
    {
        #region "MEMBER VARIABLES:"
        //=========================

            private clsUnit mUnit = new clsUnit();      

            private string mType;               //....Independent. 
            private string mMat;                //....Available set depends on "Type".
            private string mD_Desig;            //....Available set depends on "Type" & "Mat".
            
            private StringCollection mTypeList; //....List of Pin Types.
                   
            private Double mL;                  //....Available set depends on "Type", "Mat" & "D_Desig".

            //Retrieved (Dependent) variable:
            //------------------------------
            private Double mD;                              

            //....Other:
            private clsDB mDB;

        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================

            public clsUnit Unit                 
            {
                get { return mUnit; }

                set
                {
                    mUnit = value;
                    Reset_D();
                }
            }


            public string Type      
            {
                get { return mType; }
                set { mType = value; }
            }


            public string Mat       
            {
                get { return mMat; }
                set { mMat = value; }
            }


            public string D_Desig   
            {
                get { return mD_Desig; }
                set 
                { 
                    mD_Desig = value;
                    Reset_D();                   
                }
            }


            public Double L         
            {
                get { return mL; }
                set { mL = value; }
            }


            public Double D
            {
                get
                {
                    if (mD < modMain.gcEPS)
                        Retrieve_D();

                    return mD;
                }
            }

        #endregion

         
        public clsPin(clsUnit.eSystem UnitSystem_In)     
        //==========================================
        {
            mUnit.System = UnitSystem_In;
            mDB = new clsDB();
            
            mTypeList = new StringCollection();
            GetTypeList();        
        }


        #region "CLASS METHODS:"
        //====================

            private void GetTypeList()
            //========================
            {
                mDB.PopulateStringCol(mTypeList, "tblManf_Pin", "fldType", "", false);
            }           
         
            private void Reset_D()
            //====================
            {
                mD = 0.0F;
            }

            private void Retrieve_D()          
            //-----------------------
            {
                //....Ref. Radial_Rev11_27OCT11: Col. CC - 'SplitLine' & GC - 'AntiRotation'
                if (mD_Desig == null || mD_Desig == "") return;

                if (mUnit.System == clsUnit.eSystem.English)            
                {
                    string pstrTABLE = "tblManf_Pin_D";
                    string pstrFIELDS = "fldD";
                    string pstrFROM = " FROM " + pstrTABLE;
                    string pstrWHERE = " WHERE fldD_Desig = '" + mD_Desig + "'";

                    string pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                    SqlConnection pConnection = new SqlConnection();
                    SqlDataReader pDR = null;

                    pDR = mDB.GetDataReader(pstrSQL, ref pConnection);

                    if (pDR.Read())
                    {
                        mD = mDB.CheckDBDouble(pDR, "fldD");             //....Retrieved in "in"
                    }

                    pDR.Close();
                    pConnection.Close();
                }

                else if (mUnit.System == clsUnit.eSystem.Metric && mD_Desig.Contains("M"))
                {   
                    //....Not yet in the database. Parse the D_Desig string (e.g. M3) to get the dia in mm. 
                    mD = Convert.ToDouble(mD_Desig.Remove(0, 1)) / 25.4;         //....1 in = 25.4 mm.
                }
            }
           
        #endregion
       
    }
}
