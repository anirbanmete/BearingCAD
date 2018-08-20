//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_3_OilInlet        '
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
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace BearingCAD21
{
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsOilInlet
        //======================
        {
            #region "ENUMERATION TYPES:"
            //==========================
                //public enum eOrificeStartPos { On, Above, Below };      //....w.r.t Bearing S/L.
                public enum eOrificeStartPos {Above, On, Below };      //....w.r.t Bearing S/L. 

            #endregion


            #region "USER-DEFINED STRUCTURES:"
            //============================

                #region "Orifice:"
                    [Serializable]
                    public struct sOrifice
                    {
                        public int Count;       //....If Pad L <= 4 in (100 mm), then Orifice_Count = Count_Pad 
                        public Double D;        //    Else                     , then Orifice_Count = Count_Pad or 2*Count_Pad.
                        public Double L;
                        //PB 18APR12. BG, SEE below for instructions. 
                        public eOrificeStartPos StartPos;
                        public Double DDrill_CBore;    
                        public Double Loc_FrontFace;   
                        public Double AngStart_BDV;
                        public Double Dist_Holes;   //....If Orifice_Count =   Count_Pad, Dist_FeedHole = 0 (irrelevant).
                                                    //                                                  = 2*Count_Pad, Dist_FeedHole > 0.
                    }
                #endregion


                #region "Annulus:"
                    [Serializable]
                    public struct sAnnulus
                    {
                        public bool Exists;
                        public Double Ratio_L_H;
                        public Double D;
                        public Double L;
                        public Double Loc_Back;

                        //....V - Velocity (fps).   //....Method.      
                    }
                #endregion

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;
                private sOrifice mOrifice;

                private int mCount_MainOilSupply;

                private sAnnulus mAnnulus;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================ 

                #region "Orifice:"
                //---------------

                    public sOrifice Orifice
                    {
                        get
                        {
                            int pPad_Count = mCurrent_Bearing_Radial_FP.Pad.Count;

                            if (mOrifice.Count <= 0 && pPad_Count > 0)
                                mOrifice.Count  = pPad_Count;

                            mOrifice.L = Calc_Orifice_L();

                            mOrifice.DDrill_CBore = Calc_Orifice_DDrill_CBore();

                            if (mOrifice.Loc_FrontFace < modMain.gcEPS)
                                mOrifice.Loc_FrontFace = Calc_Orifice_Loc_FrontFace();
                                                
                            mOrifice.AngStart_BDV = Calc_Orifice_AngStart_BDV();

                            if (mOrifice.Dist_Holes < modMain.gcEPS)
                                mOrifice.Dist_Holes = Calc_Orifice_Dist_Holes();
                       

                            return mOrifice;
                        }
                    }


                    public int Orifice_Count
                    {
                        set { mOrifice.Count = value; }
                    }

                    public Double Orifice_D
                    {
                        set { mOrifice.D = value; }
                    }

                    public eOrificeStartPos Orifice_StartPos
                    {
                        set
                        {
                            mOrifice.StartPos = value;
                            //mOrifice.AngStart_BDV = Calc_AngStart_Feed_BDV();         //Review    BG 19APR12
                        }
                    }

                    public Double Orifice_Loc_FrontFace
                    {
                        set { mOrifice.Loc_FrontFace = value; }
                    }

                    public Double Orifice_Dist_Holes
                    {
                        set { mOrifice.Dist_Holes = value; }
                    }       

                #endregion


                #region "Main Members:"
                //---------------------

                    public int Count_MainOilSupply
                    {
                        get { return mCount_MainOilSupply; }
                        set { mCount_MainOilSupply = value; }
                    }


                   

                #endregion


                #region "Annulus:"
                //---------------

                    public sAnnulus Annulus
                    {
                        get
                        {
                            if (mAnnulus.D < modMain.gcEPS)
                                Calc_Annulus_Params();           //....D, L.  

                            if (mAnnulus.L < modMain.gcEPS)
                                Calc_Annulus_Params();

                            if (mAnnulus.Loc_Back < modMain.gcEPS)
                                mAnnulus.Loc_Back = Calc_Annulus_Loc_Back();

                            return mAnnulus;
                        }
                    }


                    public bool Annulus_Exists
                    {
                        set { mAnnulus.Exists = value; }
                    }

                    public Double Annulus_Ratio_L_H
                    {
                        set
                        {
                            mAnnulus.Ratio_L_H = value;
                        }
                    }

                    public Double Annulus_D
                    {
                        set { mAnnulus.D = value; }
                    }

                    public Double Annulus_L
                    {
                        set { mAnnulus.L = value; }
                    }

                    public Double Annulus_Loc_Back
                    {
                        set { mAnnulus.Loc_Back = value; }
                    }

                #endregion


            #endregion


            #region "CONSTRUCTOR:"

                public clsOilInlet(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //=====================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                    mCount_MainOilSupply = 1;
                    //OilInlet_AnnulusRatio_L_H = 2.0F;   
                }

            #endregion


            #region "CLASS METHODS":

                #region "ORIFICE:"

                    private Double Calc_Orifice_DDrill_CBore()
                    //========================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DK
                        //........Oil Inlet Orifice D: Col. DJ
                        return modMain.MRound(2.5 * mOrifice.D, 0.0625);
                    }

                   
                    public Double Calc_Orifice_Loc_FrontFace()         
                    //==========================================   
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DL.
                        Double pDepthF = mCurrent_Bearing_Radial_FP.Depth_EndConfig[0];        //....Col. CZ.
                        //Double pDepthB = mCurrent_Bearing_Radial_FP.Depth_EndConfig[1];        //....Col. DA.   PB 18JAN13. Not yet needed.

                        Double pVal = 0.0;

                        if (!Orifice_Exists_2ndSet())
                        {
                            //....# of Orifice Sets = 1.
                            pVal = pDepthF + mCurrent_Bearing_Radial_FP.MillRelief.EDM_Relief[0] + (0.5 * mCurrent_Bearing_Radial_FP.Pad.L);   
                        }


                        else
                        {
                            //....# of Orifice Sets = 2.
                            pVal = pDepthF + mCurrent_Bearing_Radial_FP.MillRelief.EDM_Relief[0] + (mCurrent_Bearing_Radial_FP.Pad.L / 2.5);  
                        }

                        return pVal;
                    }

                    private Double Calc_Orifice_L()
                    //=============================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DN
                        return 1.5 * mOrifice.D;
                    }


                    private Double Calc_Orifice_AngStart_BDV()
                    //=========================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DO
                        eOrificeStartPos pAngPos = mOrifice.StartPos;
                        Double pCount_FeedHole = mCurrent_Bearing_Radial_FP.Pad.Count;    //....As defined in Radial_Rev11 Design Table.  

                        Double pAng = 0.5 * ((360 / pCount_FeedHole) - mCurrent_Bearing_Radial_FP.Pad.Angle());
                        Double pVal = 0.0;

                        if (pAngPos == eOrificeStartPos.Above)
                            pVal = 90 + pAng;

                        else if (pAngPos == eOrificeStartPos.Below)
                            pVal = 90 - pAng;

                        else if (pAngPos == eOrificeStartPos.On)
                            pVal = 90;

                        return pVal;
                    }

                #endregion


                #region "MAIN:"
     
                    public bool Orifice_Exists_2ndSet()
                    //==================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. DQ
                        if (mOrifice.Count == 2 * mCurrent_Bearing_Radial_FP.Pad.Count)
                            return true;
                        else
                            return false;
                    }


                    private Double Calc_Orifice_Dist_Holes()      
                    //======================================
                    {
                        Double pDepthB = mCurrent_Bearing_Radial_FP.Depth_EndConfig[1];       
                        Double pPadL = mCurrent_Bearing_Radial_FP.Pad.L;

                        Double pVal = 0.0F;

                        if (Orifice_Exists_2ndSet())
                            //....# of Orifice Sets = 2.
                            pVal = 0.2 * pPadL;        

                        else
                            //....# of Orifice Sets = 1.
                            //........Design Table cols. requires a non-null value.
                            pVal = 1;

                        return pVal;
                    }

                #endregion


                #region "ANNULUS:"

                    public void Calc_Annulus_Params()
                    //=================================
                    {
                        //....Flow reqd. GPM.
                        Double pFlowReqd_gpm = mCurrent_Bearing_Radial_FP.PerformData.FlowReqd_gpm;

                        //....Calculate AMin:
                        Double pUp, pDown;
                        pUp = 231 * pFlowReqd_gpm;
                        pDown = 2 * mCount_MainOilSupply * 60 * 12 * 10;

                        Double pAMin = 0.0F;
                        if (pDown != 0.0F)
                            pAMin = pUp / pDown;


                        //....Calculate H:
                        //
                        Double pH = 0.0F;

                        if (mAnnulus.Ratio_L_H != 0.0F)
                        {
                            double pAny = 0.0F;
                            pAny = (pAMin / mAnnulus.Ratio_L_H);

                            if (pAny != 0.0F)
                                pH = (Double)Math.Sqrt(pAny);
                        }


                        //....Diameter & Length:
                        //
                        if (pH != 0.0F)
                        {
                            mAnnulus.D = mCurrent_Bearing_Radial_FP.DFit() - 2 * pH;
                            mAnnulus.L = pH * mAnnulus.Ratio_L_H;
                        }
                    }

                    public Double Calc_Annulus_L(Double Annulus_D_In)
                    //================================================
                    {
                        Double pH = 0.5 * (mCurrent_Bearing_Radial_FP.DFit() - Annulus_D_In);        
                        Double pL = mAnnulus.Ratio_L_H * pH;

                        return pL;
                    }

                    public Double Calc_Annulus_Ratio_L_H(Double Annulus_D_In, Double Annulus_L_In)
                    //==============================================================================        
                    {
                        Double pH = 0.5 * (mCurrent_Bearing_Radial_FP.DFit() - Annulus_D_In);
                        Double pRatio_L_H = Annulus_L_In / pH;

                        return pRatio_L_H;
                    }


                    public Double Annulus_V(Double Annulus_D_In, Double Annulus_L_In)
                    //================================================================      //BG 16MAY12
                    {
                        Double pV = 0.0F;

                        //....Annulus Height.
                        Double pH;
                        pH = 0.5F * (mCurrent_Bearing_Radial_FP.DFit() - Annulus_D_In);

                        //....Annulus Area.
                        Double pArea;
                        //pArea = pH * Annulus.L;
                        pArea = pH * Annulus_L_In;

                        //....Velocity
                        Double pFlowReqd_gpm = mCurrent_Bearing_Radial_FP.PerformData.FlowReqd_gpm;

                        Double pUp, pDown;
                        pUp = 231 * pFlowReqd_gpm;
                        pDown = 2 * mCount_MainOilSupply * 60 * 12 * pArea;

                        if (pDown != 0.0F)
                            pV = pUp / pDown;

                        return pV;
                    }


                    public Double Calc_Annulus_Loc_Back()
                    //=====================================
                    {
                        //....Ref. Radial_Rev11_27OCT11: Col. CU.
                        //........Bearing L: Col. CS
                        //........Annulus L: Col. CW
                        return (0.5 * (mCurrent_Bearing_Radial_FP.L - mAnnulus.L));
                    }

                #endregion

            #endregion
        }
    }
}
