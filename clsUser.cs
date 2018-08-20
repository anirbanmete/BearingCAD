//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsUser                                '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  22FEB12                                '
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
    class clsUser
    {
        #region "MEMBER VARIABLE DECLARATION"
        //*********************************** 

            private string mPrivilege;
            private string mUserName;
            private string mInitials;
            private string mPassword;
            private string mRole;           //BG 28JUN13

        #endregion

        #region "PROPERTY NAME"
        //*********************
            
            public string Name
            {
                get { return mUserName; }
                set 
                { 
                    mUserName = value;
                    RetriveUserCredentials();
                }
            }

            public string Password
            {
                get { return mPassword; }
            }

            public string Privilege
            {
                get { return mPrivilege; }
            }

            public string Initials
            {
                get { return mInitials; }
            }

            //BG 28JUN13
            public string Role
            {
                get { return mRole; }
                set { mRole = value;}
            }

        #endregion

        #region "CLASS METHOD"

            private void RetriveUserCredentials()
            //===================================
            {
                clsDB pDB = new clsDB();

                string pWHERE, pSQL;

                pSQL = "SELECT * FROM tblUser ";
                pWHERE = "WHERE fldName ='" + mUserName + "'" ;
                pSQL = pSQL + pWHERE;

                SqlConnection pConnection = new SqlConnection();   //SB 06JUL09

                SqlDataReader pDR = pDB.GetDataReader(pSQL,ref pConnection);

                if (pDR.Read())
                {
                    mPassword = pDB.CheckDBString(pDR, "fldPassword");
                    mInitials = pDB.CheckDBString(pDR, "fldInitials");
                    mPrivilege = pDB.CheckDBString(pDR, "fldPrivilege");
                }
                pDR.Close();
                pConnection.Close();

            }

        #endregion
    }
}
