//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsUnit                                '
//                        VERSION NO  :  1.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21DEC12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;


namespace BearingCAD21
{
     [Serializable]
    public class clsUnit
    {
        #region "NAMED CONSTANT"
        //**********************

            private const Double mcUnitCFac = 25.4F;    
            
            //....User unit Labels & Format:
            //.......Used in Bearing,BearingDesignDetails and SealDesignDetails form.
            private const string mcMFormat = "#0.0########";  

        #endregion

            string mLFormat;


        #region "ENUMERATION TYPES:"
        //==========================

            public enum eSystem {English, Metric};          
 
        #endregion


        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================
            private eSystem mSystem;       

        #endregion


        #region "CLASS PROPERTY ROUTINES: "
        //==================================

            public eSystem System           
            {
                get { return mSystem; }
                set 
                { 
                    mSystem = value;
                    
                }
            }

            public string MFormat
            {
                get { return mcMFormat; }
            }

        #endregion


        public clsUnit ()
        //===============
        {
            mSystem = eSystem.English;                  //....Default unit = English.
        }


        #region "CLASS METHODS:"
        //=====================

        public string WriteInUserL(double Val_In)
            //====================================
        {
            String pVal = "";
            if (mSystem == eSystem.English)
            {
                mLFormat = "##0.0000";
            }
            else
            {
                mLFormat = "###0.000";
            }
            if (Math.Abs(Val_In) > modMain.gcEPS)
            {
                pVal = Val_In.ToString(mLFormat);                 
            }
            return pVal;
        }

       

            public double CFac_Load_EngToMet(double Load_In)
            //---------------------------------------------
            {   //....Convert lbf ==> KN.
                //........1 N = 0.22481 lbf 
                return (Load_In / 224.81f);
            }

            public double CFac_Load_MetToEng(double Load_In)    
            //---------------------------------------------
            {   //....Convert lbf ==> KN.
                //........1 N = 0.22481 lbf 
                return (Load_In * 224.81f);
            }


            public double CFac_Press_EngToMet(double Press_In) 
            //------------------------------------------------
            {   //....Convert psi ==> kPa.
                //........1 psi = 6894 Pa = 6.894 kPa. 
                //.........1000 kPa = 1 mPa.
                return (Press_In * 0.006894f);
            }

            public double CFac_Press_MetToEng(double Press_In)  
            //------------------------------------------------
            {   //....Convert psi ==> kPa.
                //........1 psi = 6894 Pa = 6.894 kPa. 
                //.........1000 kPa = 1 mPa.
                return (Press_In / 0.006894f);

            }

            public double CFac_Temp_EngToMet(double Temp_In)
            //----------------------------------------------
            {   //....Convert F ==> C.
                return ((Temp_In - 32) / 1.8f);
            }

            public double CFac_Temp_MetToEng(double Temp_In)    
            //----------------------------------------------
            {
                return ((Temp_In * 1.8f) + 32);
            }

            public double CFac_Power_EngToMet(double HP_In)
            //---------------------------------------------
            {   //....Convert HP ==> kW.
                return (HP_In * 0.7457);
            }

            public double CFac_Power_MetToEng(double HP_In)     
            //---------------------------------------------
            {   //....Convert HP ==> kW.
                return (HP_In / 0.7457);
            }

            public double CFac_GPM_EngToMet(double GPM_In)
            //--------------------------------------------
            {   //....Convert GPM ==> LPM.
                //........1 gal = 3.7854 liter.
                return (GPM_In * 3.7854);
            }

            public double CFac_LPM_MetToEng(double LPM_In)      
            //--------------------------------------------
            {   //....Convert LPM ==> GPM.
                return (LPM_In / 3.7854);
            }

            public double CFac_TRise_EngToMet(double TRise_In)   
            //------------------------------------------------
            {   //....Convert F ==> C.
                //........1.8 F = 1 C Rise.     
                return (TRise_In / 1.8);
            }

            public double CFac_TRise_MetToEng(double TRise_In)      
            //------------------------------------------------
            {   //....Convert F ==> C.
                //........1.8 F = 1 C Rise.     
                return (TRise_In * 1.8);
            }

            public Double CMet_Eng(Double Sng_In)
            //-----------------------------------                   
            {
                return (Sng_In / mcUnitCFac);
            }

           
            public Double CEng_Met(Double Sng_In)
            //-----------------------------------                   
            {
                return (Sng_In * mcUnitCFac);
            }

            public Boolean Compare(ref clsUnit Unit_In)             
            //-----------------------------------------
            {
                bool mblnVal_Changed = false;
                int pRetValue = 0;

                if (modMain.CompareVar(Unit_In.mSystem.ToString(), mSystem.ToString(), pRetValue) > 0)
                {
                    mblnVal_Changed = true;
                }

                return mblnVal_Changed;
            }
           
        #endregion
    }
}
