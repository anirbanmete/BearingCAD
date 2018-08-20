
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsMaterial                            '
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
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Linq;

namespace BearingCAD21
{
    [Serializable]              
    public class clsMaterial
    {

        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            private string mBase;
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
            
            public string MatCode(string Mat_In)
            //===================================
            {
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                string pWaukeshaCode = "";
                var pProject = (from pRec in pBearingDBEntities.tblData_Mat where pRec.fldName == Mat_In select pRec.fldCode_Waukesha).ToList();

                if (pProject.Count > 0)
                {
                    pWaukeshaCode = modMain.gDB.CheckDBString(pProject[0]);
                }
                return pWaukeshaCode;
            }

        #endregion
    }





   

    
}
