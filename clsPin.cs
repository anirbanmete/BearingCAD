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

namespace BearingCAD21
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
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                var pQryManf_Pin = (from pRec in pBearingDBEntities.tblManf_Pin select pRec.fldType).Distinct().ToList();

                if (pQryManf_Pin.Count() > 0)
                {
                    for (int i = 0; i < pQryManf_Pin.Count; i++)
                    {
                        mTypeList.Add(pQryManf_Pin[i].ToString().Trim());
                    }
                }
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
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                if (mD_Desig == null || mD_Desig == "") return;

                if (mUnit.System == clsUnit.eSystem.English)
                {
                    var pProject = (from pRec in pBearingDBEntities.tblManf_Pin_D where pRec.fldD_Desig == mD_Desig select pRec.fldD).ToList();

                    if (pProject.Count > 0)
                    {
                        mD = mDB.CheckDBDouble(pProject[0]);             //....Retrieved in "in"
                    }
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
