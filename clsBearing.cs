
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing                             '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  27JUL18                                '
//                                                                              '
//===============================================================================

//PB 13AUG12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearingCAD21
{
     [Serializable]
    public abstract class clsBearing
    {

        //#region "NAMED CONSTANTS:"
        ////========================
        //    private const Double mcPAD_L_THRESHOLD = 100F;     //AM 13AUG12  //PB 13AUG12, moved to clsBearing_Radial_FP

        //#endregion


        #region "ENUMERATION TYPES:"
        //==========================
            public enum eType { Radial, Thrust };
        #endregion


        #region "MEMBER VARIABLES:"
        //=========================           
            private clsUnit mUnit = new clsUnit();
            private eType mType;

            protected clsDB mDB;
            
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //================================

            public clsUnit Unit
            {
                get { return mUnit; }
            }


            public eType Type
            {
                get { return mType; }
                set { mType = value; }
            }

         //AM 13AUG12   //PB 13AUG12
            //public Double PAD_L_THRESHOLD
            //{
            //    get 
            //    { 
            //        return mUnit.CMet_Eng(mcPAD_L_THRESHOLD);
            //    }
            //} 

        #endregion

        
        //....Class Constructor
            public clsBearing(clsUnit.eSystem UnitSystem_In, eType Type_In,
                              clsBearing_Radial_FP.eDesign Design_In, clsDB DB_In)
        //====================================================================
        {
            mUnit.System = UnitSystem_In;
            mType = Type_In;
            mDB = DB_In;
        }
    }
}
