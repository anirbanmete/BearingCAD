
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

namespace BearingCAD21
{
    class clsDrill
    {
        public static Double D (String D_Desig_In)
        //========================================
        {
            //....Returns Diameter in inch.
            clsDB pDB = new clsDB();
            BearingDBEntities pBearingDBEntities = new BearingDBEntities();
            Double pD = 0.0F;
            if (D_Desig_In != null)
            {
                var pProject = (from pRec in pBearingDBEntities.tblManf_Drill where pRec.fldD_Desig == D_Desig_In select pRec.fldD).ToList();
                if (pProject.Count > 0)
                    pD = pDB.CheckDBDouble(pProject[0]);
                else if (!D_Desig_In.Contains("/"))
                    pD = modMain.ConvTextToDouble(D_Desig_In);
            }
            return pD;
        }
    }
}
