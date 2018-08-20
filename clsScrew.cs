
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsScrew                               '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  16APR12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 16APR12. Just cleaned up. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Collections; 

namespace BearingCAD20
{
    [Serializable]                      
    public class clsScrew : ICloneable
    {
         # region "MEMBER VARIABLE DECLARATIONS:"
        //======================================

            private clsUnit mUnit = new clsUnit();          
        
            private string mType;                   //....Independent.
            private string mMat;                    //....Available set depends on "Type".
            private string mD_Desig;                //....Available set depends on "Unit", "Type" & "Mat". 

            private StringCollection mTypeList;     //.....List of Screw Types. 

            private string mPitchType;              //....Only available for "English" - UNC & UNF.
            private Double mPitch;                  //....Depends on "D_Desig" (& "PitchType", if any).

            private Double mL;                      //....Available set depends on "Type", "Mat" & "D_Desig".


            //Retrieved (Dependent) variables:
            //--------------------------------
            //....Could have been methods. But for the sake of database retrieval 
            //........efficiency, they have been made member variables here.
 
            //....Dependent on "D_Desig":
            //........Ref. Radial_Rev11_27OCT11.
            //
            private Double mD;              //....Col. X.
            private Double mD_TapDrill;     //....Col. S
            private Double mD_Thru;         //....Col. AB
            private Double mD_CBore;        //....Col. AG

            //....Dependent on "Type" & "D_Desig":
            private Double mHead_H;                     
            private Double mHead_D;                                         
            
            //....Other:
            //[NonSerialized]                         
            private clsDB mDB;

        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================
            
            //PB 15APR12.
            public clsUnit Unit
            {
                get { return mUnit; }

                set
                {
                    mUnit = value;                  
                    Reset_D_Params();
                    Reset_Head_Params();
                }
            }

            public string Type
            {
                get { return mType; }

                set 
                {   mType = value;
                    Reset_Head_Params();
                }
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
                {   mD_Desig = value;
                    Reset_D_Params();
                    Reset_Head_Params();                                
                }
            }

            
            public string PitchType 
            {
                get { return mPitchType; }
                set { mPitchType = value; }
            }


            public Double Pitch     
            {
                //....Unit dependent - /in or /mm.
                get { return mPitch; }
                set { mPitch = value; }
            }


            public Double L
            {   
                //....Unit dependent - in or mm.
                get { return mL; }
                set { mL = value; }
            }


            // Diameters
            // ---------
            public Double D                     
            {
                //....Unit independent - if Unit = "Metric", has been converted into "in".
                get 
                { 
                    if (mD < modMain.gcEPS)
                        Retrieve_D_Params();
                   
                    return mD; 
                }
            }


            public Double D_TapDrill
            {
                //....Unit independent - always retrieved in "in".     
                get 
                {
                    if (mD_TapDrill < modMain.gcEPS)
                        Retrieve_D_Params();
                    
                    return mD_TapDrill; 
                }
            }


            public Double D_Thru
            {
                //....Unit independent - always retrieved in "in". 
                get 
                {
                    if (mD_Thru < modMain.gcEPS)
                        Retrieve_D_Params();

                    return mD_Thru; 
                }
            }


            public Double D_CBore
            {
                //....Unit independent - always retrieved in "in". 
                get 
                {
                    if (mD_CBore < modMain.gcEPS)
                        Retrieve_D_Params();

                    return mD_CBore; 
                }
            }
          

            public Double Head_H                       
            {
                //....Unit independent - if Unit = "Metric", has been converted into "in".
                get 
                {
                    if(mHead_H < modMain.gcEPS)
                        Retrieve_Head_Params();

                    return mHead_H;
                } 
            }


            public Double Head_D                   
            {
                //....Unit independent - if Unit = "Metric", has been converted into "in".
                get 
                {
                    if (mHead_D < modMain.gcEPS)
                        Retrieve_Head_Params();

                    return mHead_D; 
                }
            }

        #endregion


        //....Class Constructor
         public clsScrew (clsUnit.eSystem UnitSystem_In)           
        //==============================================
        {
            mDB = new clsDB();

            mUnit.System = UnitSystem_In;           
            mTypeList   = new StringCollection();                    
            GetTypeList ();                         //PB 20FEB12. PitchType is not used anywhere.
        }


        # region "CLASS METHODS:"
        //=======================

            private void GetTypeList()              //PB 18JAN12. To be reviewed later. 
            //========================
            {
                mDB.PopulateStringCol(mTypeList, "tblManf_Screw", "fldType", "", false);
            }
  

            public void GetPitch (string DiaDesig_In, out ArrayList Pitch_Array, out ArrayList PitchType_Array)
            //=================================================================================================
            { 
                String pUnit = mUnit.System.ToString().Substring(0, 1);     //..."E" or "M".

                string pstrTABLE  = "tblManf_Screw";
                string pstrFIELDS = "fldPitch, fldPitchType";
                string pstrFROM   = " FROM " + pstrTABLE;
                string pstrWHERE  = " WHERE fldUnit = '" + pUnit + "' AND fldD_Desig = '" + DiaDesig_In + "'";

                string pSQL = "SELECT DISTINCT " + pstrFIELDS + pstrFROM + pstrWHERE;

                SqlConnection pConnection = new SqlConnection();   
                SqlDataReader pDR = null;
                pDR               = mDB.GetDataReader(pSQL, ref pConnection);

                Pitch_Array     = new ArrayList();
                PitchType_Array = new ArrayList();
                
                while(pDR.Read())
                {
                    double pPitch = Convert.ToDouble (pDR["fldPitch"]);      
                    Pitch_Array.Add(pPitch.ToString("#0.000"));            
                    PitchType_Array.Add(pDR["fldPitchType"]);
                }

                pDR.Close();    
                pConnection.Close();
            }

                   
            #region "Reset Dependent Member Variables:"

                private void Reset_D_Params()
                //===========================
                {
                    mD = 0.0F;
                    mD_TapDrill = 0.0F;
                    mD_Thru = 0.0F;
                    mD_CBore = 0.0F;
                }


                private void Reset_Head_Params()
                //==============================
                {
                    mHead_D = 0.0F;
                    mHead_H = 0.0F;
                }

            #endregion


            #region "Retrieve Dependent Member Variables:"

                private void Retrieve_D_Params()        //....To be unit-tested. 
                //==============================
                {
                    if (mD_Desig == "" || mD_Desig == null) return;

                    string pstrTABLE  = "tblManf_Screw_D";
                    string pstrFIELDS = "fldD, fldD_TapDrill, fldD_Thru, fldD_CBore ";
                    string pstrFROM   = "FROM " + pstrTABLE;
                    string pWHERE     = " WHERE fldD_Desig = '" + mD_Desig + "'";

                    string pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pWHERE;

                    SqlConnection pConnection = new SqlConnection();   
                    SqlDataReader pDR = null;

                    pDR = mDB.GetDataReader(pstrSQL, ref pConnection);

                    if (pDR.Read())
                    {
                        Double pD = mDB.CheckDBDouble(pDR, "fldD");             //....Retrieved in "in" or "mm".

                        if (mUnit.System == clsUnit.eSystem.Metric)  
                            mD = pD / 25.4;     //....1 in = 25.4 mm.

                        else                    //....English Unit.
                            mD = pD;
                            
                        //....Always retrieved in "in".
                        mD_TapDrill = mDB.CheckDBDouble(pDR, "fldD_TapDrill");
                        mD_Thru     = mDB.CheckDBDouble(pDR, "fldD_Thru");
                        mD_CBore    = mDB.CheckDBDouble(pDR, "fldD_CBore");
                    }

                    pDR.Close(); 
                    pConnection.Close();
                }


                private void Retrieve_Head_Params()
                //=================================
                {
                    if (mType == null || mD_Desig == null) return;

                    string pstrTABLE = "tblManf_Screw_Head";
                    string pstrFIELDS = "fldHead_H, fldHead_D";
                    string pstrFROM = " FROM " + pstrTABLE;
                    string pWHERE = " WHERE fldD_Desig = '" + mD_Desig + "' AND fldType = '" + mType + "'";

                    string pSQL = "SELECT " + pstrFIELDS + pstrFROM + pWHERE;

                    SqlConnection pConnection = new SqlConnection();   
                    SqlDataReader pDR = null;

                    pDR = mDB.GetDataReader(pSQL, ref pConnection);

                    if (pDR.Read())
                    {
                        //....Unit dependent - in or mm. 
                        Double pHead_H = mDB.CheckDBDouble(pDR, "fldHead_H");
                        Double pHead_D = mDB.CheckDBDouble(pDR, "fldHead_D");

                        if (mUnit.System == clsUnit.eSystem.Metric)
                        {
                            mHead_H = pHead_H / 25.4;
                            mHead_D = pHead_D / 25.4;
                        }
                        else                //....English Unit.
                        {
                            mHead_H = pHead_H;
                            mHead_D = pHead_D;
                        }
                    }

                    pDR.Close();   
                    pConnection.Close();
                }

            #endregion           


            #region "ICLONABLE METHOD:"

                public object Clone()
                //===================
                {
                    return this.MemberwiseClone();
                }

            #endregion

        #endregion

    }
}
