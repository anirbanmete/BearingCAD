
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial                      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  18MAY12                                '
//                                                                              '
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearingCAD20
{
     [Serializable]
    public abstract class clsBearing_Radial: clsBearing 
    {
        #region "ENUMERATION TYPES:"
        //==========================
           public enum eDesign { Flexure_Pivot, Tilting_Pad, Sleeve };

        #endregion


        #region "MEMBER VARIABLES:"
        //=========================
           private eDesign mDesign;

        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //================================
      
            public eDesign Design
            {
                get { return mDesign; }
                set { mDesign = value; }
            }

        #endregion


        //....Class Constructor
        public clsBearing_Radial(clsUnit.eSystem UnitSystem_In, eType Type_In, eDesign Design_In, clsDB DB_In)
                                 : base(UnitSystem_In, Type_In, Design_In, DB_In)
        //====================================================================================================
        {
            mDesign = Design_In;
        }
    }
}
