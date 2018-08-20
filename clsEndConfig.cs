//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsEndConfig                           '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  23JAN13                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BearingCAD20
{
     [Serializable]
    public abstract class clsEndConfig
    {
        #region "Main Class:"

            #region "ENUMERATION TYPES:"
            //==========================
                public enum eType { Seal, Thrust_Bearing_TL };

            #endregion


            #region "MEMBER VARIABLES:"
            //========================
                protected clsProduct mCurrentProduct;

                private clsUnit mUnit = new clsUnit();              //PB 20DEC12. May not be needed. use the unit system of mCurrentProduct.
                private eType mType;
                private clsMaterial mMat = new clsMaterial();       //...Materials - Base & Lining.

                //....Envelope Geometry:
                private Double mDO;
                private Double[] mDBore_Range = new Double[2];
                private Double mL;

                //....Mounting Holes: 
                private clsMountHoles mMountHoles;
   
                protected clsDB mDB;

            #endregion


            #region "PROPERTY ROUTINES:"
            //=========================

                public clsUnit Unit
                {
                    get { return mUnit; }
                }


                public eType Type
                {
                    get { return mType; }
                    set { mType = value; }            //PB 23JAN13
                }


                #region "Material:"

                    // Base & Lining Materials:
                    //------------------------
                    public clsMaterial Mat
                    {
                        get { return mMat; }
                        set { mMat = value; }
                    }

                #endregion


                #region "Envelope Geometry:"

                    //....OD  
                    public Double DO
                    {
                        get { return mDO; }
                        set { mDO = value; }
                    }


                    //....Bore Dia 
                    public Double[] DBore_Range
                    {
                        get { return mDBore_Range; }
                        set { mDBore_Range = value;}
                    }


                    //....Length                   
                    public Double L
                    {
                        get
                        {   if (mL < modMain.gcEPS)
                            {   
                                mL = mCurrentProduct.Calc_L_EndConfig();
                            }
                            return mL; 
                        }

                        set { mL = value; }
                    }

                #endregion


                #region "Mount Holes:"

                    public clsMountHoles MountHoles
                    {
                        get { return mMountHoles; }
                        set { mMountHoles = value; }
                    }

                #endregion

            #endregion


            #region "Constructor:"
                
                public clsEndConfig(clsUnit.eSystem UnitSystem_In, eType Type_In, 
                                    clsDB DB_In, clsProduct CurrentProduct_In)
                //=================================================================
                {
                    mUnit.System = UnitSystem_In;
                    mType        = Type_In;

                    mMountHoles  = new clsMountHoles (this);

                    mMat.Base         = "Bronze 660";
                    mMat.LiningExists = false;
                    mMat.Lining       = "None";
           
                    mCurrentProduct = CurrentProduct_In;

                    mDB = DB_In;
                }

            #endregion


            #region "CLASS METHODS:"
            //---------------------

                public Double DBore()
                //===================
                {
                    return modMain.Nom_Val(mDBore_Range);
                }


                public Double Clearance()
                //=======================              
                {   
                    //....Diametral Clearance.
                    Double pClear; 
                    pClear = DBore() - ((clsBearing_Radial_FP)mCurrentProduct.Bearing).DShaft();
                    return pClear;
                }


                public Double Tol_DBore (String Tol_Type_In)                        
                //==========================================
                {
                    Double pDSet = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).DSet();

                    //....Retrieve from database.                                  
                    //
                    string pstrTblName, pstrFIELDS = "", pstrFROM, pstrWHERE, pstrSQL;
                    pstrTblName = "tblDesign_Tol";
                    
                    if (Tol_Type_In == "Assy")
                    {
                        pstrFIELDS = "fldTol_Assy_EndConfig_DBore";       
                    }
                    else if (Tol_Type_In == "Detail")
                    {
                        pstrFIELDS = "fldTol_Detail_EndConfig_DBore";       
                    }

                    pstrFROM = " FROM " + pstrTblName;
                    pstrWHERE = " WHERE fldDSet_LLim <= " + pDSet +
                                  " AND fldDSet_ULim >= " + pDSet;

                    pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;       

                    SqlConnection pConnection = new SqlConnection();

                    SqlDataReader pDR = null;
                    pDR = mDB.GetDataReader(pstrSQL, ref pConnection);

                    Double pTol_Detail_DSet = 0.0f;
                    if (pDR.Read())
                    {
                        pTol_Detail_DSet = Convert.ToDouble(pDR[pstrFIELDS]);
                    }
                    pDR.Close();
                    pConnection.Close();

                    return pTol_Detail_DSet;
                }


                public Double Calc_DBore_Limit(int Dia_Indx_In)
                //=============================================          
                {
                    Double pLimit = 0.0F;
                    if (Dia_Indx_In == 0)
                    {
                        pLimit = mDBore_Range[Dia_Indx_In] + (2 * Tol_DBore("Assy"));
                    }
                    else if (Dia_Indx_In == 1)
                    {
                        pLimit = mDBore_Range[Dia_Indx_In] - (2 * Tol_DBore("Assy"));
                    }
                    return pLimit;
                }
               

            #endregion

        #endregion


        #region "Nested Class: clsMountHoles:"
        //-----------------------------------
             [Serializable]
            public class clsMountHoles
            //=========================
            {
                #region "NAMED CONSTANTS:"
                //----------------------

                    //DESIGN PARAMETERS:           
                    //------------------
                    //....Diametral Clearance between the Mounting Screw Head & Counter Bore.         
                    private const Double mc_DESIGN_CBORE_CLEARANCE = 0.030F;

                    //....Diametral Clearance between the Mounting Screw & Thru' Hole Dia.       
                    private const Double mc_DESIGN_THRU_HOLE_CLEARANCE = 0.030F;


                    //....Counter Bore Depth Limit Margins:
                    //
                        //....Lower Limit Margin: w.r.t Mounting Screw Head Height.
                        private const Double mc_DESIGN_CBORE_DEPTH_MARGIN_LOWER_LIM = 0.015F;

                        //....Upper Limit Margin: w.r.t Seal Length.
                        private const Double mc_DESIGN_CBORE_DEPTH_MARGIN_UPPER_LIM = 0.030F;

                #endregion
        
                
                #region "ENUMERATION TYPES:"
                //-------------------------
                    public enum eMountHolesType { C, H, T };  //....C: CBore, H: Thru', T: Thread.

                #endregion


                #region "MEMBER VARIABLES:"
                //------------------------
                    private clsScrew mScrew_Spec;       //....Equals clsBearing_Radial_FP.MountFixture.ScrewSpec 

                    private clsEndConfig mCurrentEndConfig;

                    private eMountHolesType mType;
                    //....D_Thru                        //....Retrived from DB. In earlier version, a Method "D_ThruHole" was used.     
                    //....D_CBore;                      //....Retrived from DB. In earlier version, a Method "D_CBore" was used.     
                    private Double mDepth_CBore;

                    private Boolean mThread_Thru;                   
                    private Double mDepth_Thread;

                #endregion


                #region "PROPERTY ROUTINES:"
                //==========================  

                    public clsScrew Screw_Spec
                    {
                        get { return mScrew_Spec; }
                        set { mScrew_Spec = value; }
                    }

  
                    public eMountHolesType Type
                    {
                        get { return mType; }
                        set { mType = value; }
                    }


                    public Double Depth_CBore
                    {
                        get { return mDepth_CBore; }
                        set { mDepth_CBore = value; }
                    }
                 

                    public Boolean Thread_Thru
                    {
                        get { return mThread_Thru; }
                        set { mThread_Thru = value; }
                    }


                    public Double Depth_Thread
                    {
                        get {
                            if (mDepth_Thread < modMain.gcEPS)
                               return 2 * mScrew_Spec.D;
                            else
                               return mDepth_Thread;}

                        set { mDepth_Thread = value; }
                    }
                 
                #endregion


                #region "CONSTRUCTOR:"
                //===================  

                    public clsMountHoles(clsEndConfig CurrentEndConfig_In)
                    //====================================================
                    {
                        mCurrentEndConfig = CurrentEndConfig_In;
                        mScrew_Spec = new clsScrew(CurrentEndConfig_In.Unit.System);
                    }

                #endregion


                #region "CLASS METHODS:"
                //----------------------
                 
                    public Double CBore_Depth_LowerLimit ()
                    //-------------------------------------         
                    {
                        Double pHead_H;

                        if (mScrew_Spec.D_Desig.Contains("M"))          //....Metric.
                            //....Convert from mm ==> in.
                            pHead_H = mScrew_Spec.Head_H / 25.4F;

                        else                                            //....English.
                            pHead_H = mScrew_Spec.Head_H;


                        Double pLowerLimit = 0.0F;
                        Double pMargin = mc_DESIGN_CBORE_DEPTH_MARGIN_LOWER_LIM;

                        pLowerLimit = pHead_H + pMargin;

                        return pLowerLimit;
                    }


                    public Double CBore_Depth_UpperLimit()
                    //------------------------------------   
                    {
                        Double pUpperLimit;
                        Double pMargin = mc_DESIGN_CBORE_DEPTH_MARGIN_UPPER_LIM;
                        pUpperLimit = mCurrentEndConfig.L - pMargin;

                        return pUpperLimit;
                    }

                #endregion
            }

        #endregion
    }
}

