
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
using System.Runtime.InteropServices;

namespace BearingCAD21
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
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                var pQryManf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw select pRec.fldType).Distinct().ToList();

                if (pQryManf_Screw.Count() > 0)
                {
                    for (int i = 0; i < pQryManf_Screw.Count; i++)
                    {
                        mTypeList.Add(pQryManf_Screw[i].ToString().Trim());
                    }
                }
            }


            public void GetPitch(string DiaDesig_In,  out ArrayList Pitch_Array, out ArrayList PitchType_Array, string UnitSystem ="")
            //========================================================================================================================
            {
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                //String pUnit = mUnit.System.ToString().Substring(0, 1);     //..."E" or "M".
                String pUnit = mUnit.System.ToString().Substring(0, 1);     //..."E" or "M".
                if (UnitSystem != "")
                {
                    pUnit = UnitSystem.Substring(0, 1);     //..."E" or "M".
                }
                
                Pitch_Array = new ArrayList();
                PitchType_Array = new ArrayList();
                var pProject = (from pRec in pBearingDBEntities.tblManf_Screw where pRec.fldUnit == pUnit && pRec.fldD_Desig == DiaDesig_In select (new { pRec.fldPitch, pRec.fldPitchType })).Distinct().ToList();
                int pCount = pProject.Count();

                Pitch_Array = new ArrayList();
                PitchType_Array = new ArrayList();

                if (pCount > 0)
                {
                    for (int i = 0; i < pCount; i++)
                    {
                        double pPitch = Convert.ToDouble(pProject[i].fldPitch);
                        Pitch_Array.Add(pPitch.ToString("#0.000"));
                        PitchType_Array.Add(pProject[i].fldPitchType);
                    }
                }     
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

                private void Retrieve_D_Params()       
                //==============================
                {
                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    if (mD_Desig == "" || mD_Desig == null) return;

                    var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_D where pRec.fldD_Desig == mD_Desig select pRec).ToList();

                    if (pProject.Count() > 0)
                    {
                        Double pD = mDB.CheckDBDouble(pProject[0].fldD);             //....Retrieved in "in" or "mm".

                        if (mUnit.System == clsUnit.eSystem.Metric)
                            mD = pD / 25.4;     //....1 in = 25.4 mm.

                        else                    //....English Unit.
                            mD = pD;

                        //....Always retrieved in "in".
                        if (pProject[0].fldD_TapDrill !=null)
                        {
                            mD_TapDrill = (Double)pProject[0].fldD_TapDrill;
                        }
                        else
                        {
                            mD_TapDrill = 0;
                        }

                        if (pProject[0].fldD_Thru != null)
                        {
                            mD_Thru = (Double)pProject[0].fldD_Thru;
                        }
                        else
                        {
                            mD_Thru = 0;
                        }

                        if (pProject[0].fldD_CBore != null)
                        {
                            mD_CBore = (Double)pProject[0].fldD_CBore;
                        }
                        else
                        {
                            mD_CBore = 0;
                        }



                        //mD_TapDrill = mDB.CheckDBDouble((Double)pProject[0].fldD_TapDrill);
                        //mD_Thru = mDB.CheckDBDouble((Double)pProject[0].fldD_Thru);
                        //mD_CBore = mDB.CheckDBDouble((Double)pProject[0].fldD_CBore);
                    }
                }


                private void Retrieve_Head_Params()
                //=================================
                {
                    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    if (mType == null || mD_Desig == null) return;
                    var pProject = (from pRec in pBearingDBEntities.tblManf_Screw_Head where pRec.fldD_Desig == mD_Desig && pRec.fldType == mType select (new { pRec.fldHead_H, pRec.fldHead_D })).ToList();

                    if (pProject.Count() > 0)
                    {
                        //....Unit dependent - in or mm. 
                        Double pHead_H = mDB.CheckDBDouble(pProject[0].fldHead_H);
                        Double pHead_D = mDB.CheckDBDouble(pProject[0].fldHead_D);

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
