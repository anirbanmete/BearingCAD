
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsDrill                               '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21SEP12                                '
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
    class clsDrill
    {
        public static Double D (String D_Desig_In)
        //========================================
        {
            //....Returns Diameter in inch.
            clsDB pDB   = new clsDB();    

            string pSQL = "SELECT fldD FROM tblManf_Drill " + "WHERE fldD_Desig = '" + D_Desig_In + "'";

            SqlConnection pConnection = new SqlConnection();   
            SqlDataReader pDR = pDB.GetDataReader(pSQL,ref pConnection);

            Double pD = 0.0F;
            if (pDR.Read())
                pD = pDB.CheckDBDouble(pDR, "fldD");

            else if (!D_Desig_In.Contains("/"))                     //PB 21FEB12. To be reviewed.
                pD = modMain.ConvTextToDouble(D_Desig_In);

            pDR.Close();
            pConnection.Close();

            return pD;
        }
    }
}
