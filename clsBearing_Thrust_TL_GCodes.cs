//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Thrust_TL_GCodes            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  16APR13                                '
//                                                                              '
//===============================================================================
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace BearingCAD20
{
    public partial class clsBearing_Thrust_TL : clsEndConfig, ICloneable 
    {
        #region "NAMED CONSTANTS:"
        //========================
            private const Double mc_DESIGN_OVERLAP_FRAC = 0.50F;        //....Fraction of mT2_D. 

            private const Double mc_G0_SPEED_DEF           = 100F;      //.... in/m.    
            private const Double mc_FEED_RATE_TL_DEF       = 30F;
            private const Double mc_FEED_RATE_WEEPSLOT_DEF = 15F;

            private const Double mc_DEPTH_TL_BACKLASH = 0.010F;
            private const Double mc_DEPTH_TL_DWELL_T = 200F;            //....m-Sec.
                
            //private const Double mc_WS_RMARGIN = 0.020F;      //BG 03APR13  
            private const Double mc_WS_RMARGIN = 0.050D;        //BG 03APR13     
            //private const Double mc_DEPTH_WS_CUT_PER_PASS = 0.020F;
            private const Double mc_DEPTH_WS_CUT_PER_PASS_DEF = 0.010D;     //BG 03APR13
            private const Double mc_DEPTH_WS_CUT_PER_PASS_MAX = 0.020D;     //BG 03APR13
                      
        #endregion


        #region "USER-DEFINED STRUCTURES:"
            //================================

            #region "FeedRate:"
                [Serializable]
                public struct sFeedRate
                {
                    public Double Taperland;        
                    public Double WeepSlot;
                }
            #endregion

        #endregion

                    
        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================

            private Double mG0Speed;     //....G0 Speed               

            //....Selected End Mills:
            private clsEndMill mT1;     //....Face-off.
            private clsEndMill mT2;     //....Taper Land.
            private clsEndMill mT3;     //....Weep Slot.
            private clsEndMill mT4;     //....Chamfer. 
          
            private Double mOverlap_frac;        //....Fraction of mT2_D.   

            private sFeedRate mFeedRate;

            private Double mDepth_TL_BackLash;
            private Double mDepth_TL_Dwell_T;

            private Double mRMargin_WeepSlot;   //....Weep Slot Milling - Radial margin @ ID & OD.
            private Double mDepth_WS_Cut_Per_Pass;

            private int mStarting_LineNo;

            private String mFilePath_Dir;

           
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //===============================

            #region "Named Constants:

                public Double G0_SPEED_DEF
                {
                    get { return mc_G0_SPEED_DEF; }
                }

                public Double DESIGN_OVERLAP_FRAC
                {
                    get { return mc_DESIGN_OVERLAP_FRAC; }
                }

                public Double FEED_RATE_TL_DEF
                {
                    get { return mc_FEED_RATE_TL_DEF; }
                }

                public Double DEPTH_TL_BACKLASH
                {
                    get { return mc_DEPTH_TL_BACKLASH; }
                }

                public Double DEPTH_TL_DWELL_T
                {
                    get { return mc_DEPTH_TL_DWELL_T; }
                }

                public Double FEED_RATE_WEEPSLOT_DEF
                {
                    get { return mc_FEED_RATE_WEEPSLOT_DEF; }
                }

                public Double DEPTH_WS_CUT_PER_PASS_DEF
                {
                    get { return mc_DEPTH_WS_CUT_PER_PASS_DEF; }
                }

                public Double DEPTH_WS_CUT_PER_PASS_MAX
                {
                    get { return mc_DEPTH_WS_CUT_PER_PASS_MAX; }
                }

            #endregion

        
            #region "Member Variables:

                public Double G0Speed
                {
                    get 
                    {
                        if (mG0Speed < modMain.gcEPS)
                            return mc_G0_SPEED_DEF;
                        else
                            return mG0Speed; 
                    }
                    set { mG0Speed = value; }
                }

                public clsEndMill T1
                {
                    get { return mT1; }
                    set { mT1 = value; }
                }

                public clsEndMill T2
                {
                    get { return mT2; }
                    set { mT2 = value; }
                }

                public clsEndMill T3
                {
                    get { return mT3; }
                    set { mT3 = value; }
                }

                public clsEndMill T4
                {
                    get { return mT4; }
                    set { mT4 = value; }
                }

                public Double Overlap_frac
                {
                    get { return mOverlap_frac; }
                    set { mOverlap_frac = value; }
                }

                #region "FeedRate:"
                //------------------

                    public sFeedRate FeedRate
                    {
                        get { return mFeedRate; }
                    }


                    public Double FeedRate_Taperland
                    {
                        set { mFeedRate.Taperland = value; }
                    }


                    public Double FeedRate_WeepSlot
                    {
                        set { mFeedRate.WeepSlot = value; }
                    }

                #endregion


                #region "Depth:"
                //--------------

                    public Double Depth_TL_Backlash
                    {
                        get { return mDepth_TL_BackLash; }
                        set { mDepth_TL_BackLash = value; }
                    }


                    public Double Depth_TL_Dwell_T
                    {
                        get { return mDepth_TL_Dwell_T; }
                        set { mDepth_TL_Dwell_T = value; }
                    }


                    public Double Depth_WS_Cut_Per_Pass
                    {
                        get { return mDepth_WS_Cut_Per_Pass; }
                        set { mDepth_WS_Cut_Per_Pass = value; }
                    }

                #endregion

                
                //BG 03APR13
                //public Double RMargin_WeepSlot      //BG 03SEP12
                //{
                //    get
                //    {
                //        if (mRMargin_WeepSlot < modMain.gcEPS)
                //            mRMargin_WeepSlot = Calc_Radial_Margin_WS();

                //        return mRMargin_WeepSlot;
                //    }

                //    set { mRMargin_WeepSlot = value; }

                //}

                //BG 03APR13
                //public int Starting_LineNo
                //{
                //    get { return mStarting_LineNo; }
                //    set { mStarting_LineNo = value; }
                //}

                public String FilePath_Dir
                {
                    get { return mFilePath_Dir; }
                    set { mFilePath_Dir = value; }
                }


            #endregion
           
        #endregion
       

        #region "CLASS METHODS:"
        //=====================

            public Double Overlap()
            //=====================
            {     
               return mOverlap_frac * mT2.D();
            }
            
            //BG 03APR13
            //public Double Calc_Radial_Margin_WS()
            ////=====================================
            //{
            //    return ((mT3.D() / 2) + mc_WS_RMARGIN);
            //}


            public Double DepthOnFeedGroove(Double R_In)            //PB 02JUL12. Do unit testing.
            //===========================================
            {
                //....For a given R_In, this function determines the depth on the feed groove axis by double interpolations.
                //
                Double pRatio      = (R_In - mShroud.Ri) / (mShroud.Ro - mShroud.Ri);
                Double pDepth_R_In = mTaper.Depth_ID + (mTaper.Depth_OD - mTaper.Depth_ID) * pRatio;

                Double pThetaB_R_In = (Math.Asin(mFeedGroove.Wid / (2 * R_In))) * modMain.gc_RAD2DEG;
                Double pVal = pDepth_R_In * (mTaper.Angle / (mTaper.Angle - pThetaB_R_In));   
                return pVal;                   
            }


            public int CountPass_TL()
            //=======================
            {
                int pCountPass = 0;

                Double pD = mT2.D();
                pCountPass = modMain.NInt((mShroud.Ro - mShroud.Ri - pD) / (pD - Overlap ()));

                //....Recalculate OL.
                Double pOverLap;
                pOverLap = pD - ((mShroud.Ro - mShroud.Ri - pD) / pCountPass);

               
                if (pOverLap < modMain.gcEPS)
                {
                    mOverlap_frac = mc_DESIGN_OVERLAP_FRAC;      //....If mOverlap_frac is -ve, mOverlap_frac is set to default.
                }
                else
                {
                    mOverlap_frac = pOverLap / pD;
                }

                //if (Math.Abs(mPadD[0] - (mShroud.Ri * 2)) < modMain.gcEPS)      //....If Pad ID = Shroud ID then add one more pass   //BG 04SEP12   
                if (((mShroud.Ri * 2) - mPadD[0]) < modMain.gcEPS)                //....If Pad ID = Shroud ID then add one more pass   //BG 04SEP12    
                {
                    pCountPass = pCountPass + 1;
                }

                return pCountPass;
            }

            
            public bool Validate_OL_frac (Double OL_frac_In)
            //==============================================
            {
                bool pValidated = false;

                if (OL_frac_In >= 0.5 && OL_frac_In <= 0.75)
                {
                    pValidated =  true;
                }
                else if ( (OL_frac_In < 0.5) || (OL_frac_In > 0.75) )
                {
                    MessageBox.Show("Overlap is outside the recommended range of 0.5 to 0.75 of End Mill Dia!! ", 
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    pValidated = false;
                }

                return pValidated;
            }

            public Double Validate_Depth_WS_Cut_Per_Pass(Double Depth_WS_Cut_Per_Pass)
            //=========================================================================
            {                             
                string pMsg = " Weep Slot Depth_Cut per pass should not be greater than the maximum value of, "
                                + mc_DEPTH_WS_CUT_PER_PASS_MAX + ".";
                MessageBox.Show(pMsg);

                return mc_DEPTH_WS_CUT_PER_PASS_MAX;           
            }


            public void WriteFile_GCode(String FileName_In, int EndConfig_Indx_In)     
            //====================================================================
            {                                                          
                FileStream pFS = File.Create(FileName_In);
                StreamWriter pSW = new StreamWriter(pFS);

                string pRot = "";
                int pLineNo = 1;
           
                // HEADER INFO.
                // ------------
                //
              
                if (modMain.gProject.Product.EndConfig[EndConfig_Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[i]).DirectionType == eDirectionType.Uni)
                        //{
                            if (EndConfig_Indx_In == 0)
                            {
                                pRot = clsBearing_Thrust_TL.eRotation.CCW.ToString();
                            }
                            else
                            {
                                pRot = clsBearing_Thrust_TL.eRotation.CW.ToString();
                            }

                            if (modMain.gProject.Product.EndConfig[EndConfig_Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            {

                                pSW.WriteLine("N" + pLineNo++ +" (" + modMain.gProject.No + modMain.gProject.No_Suffix + " — " + modMain.gProject.AssyDwg.No + "-" +
                                                modMain.gProject.AssyDwg.No_Suffix + " — THRUST OD " +
                                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[EndConfig_Indx_In]).PadD[1].ToString("#0.000") +
                                                " — " + pRot + " — TAPERLANDS)");
                            }

                        //}
                    //}
                 
                }
                pSW.WriteLine("N" + pLineNo++);
                pSW.WriteLine("N" + pLineNo++);
                pSW.WriteLine("N" + pLineNo++);
                pSW.WriteLine("N" + pLineNo++ +" (T1-- " + mT1.D_Desig  + " 4 FLUTE FACE END MILL)");
                pSW.WriteLine("N" + pLineNo++ +" (T2-- " + mT2.D_Desig  + " END MILL TAPER LANDS)");
                pSW.WriteLine("N" + pLineNo++ +" (T3-- " + mT3.D_Desig  + " END MILL WEEP SLOT)");
                pSW.WriteLine("N" + pLineNo++ +" (T4-- " + mT4.D_Desig  + " X 45 DEG. TOOL SHARP POINT)");
                pSW.WriteLine("N" + pLineNo++);
                pSW.WriteLine("N" + pLineNo++);


                //....Write Subroutines:
                //
                //int pLineNo = mStarting_LineNo;

                WriteFile_GCode_Taperlands(EndConfig_Indx_In, pSW, ref pLineNo);
                WriteFile_GCode_WeepSlots(pSW, ref pLineNo);
                System.Diagnostics.Process.Start(FileName_In);
                pSW.Close();
            }



            private void WriteFile_GCode_Taperlands (int EndConfig_Indx_In, StreamWriter SW_In, ref int LineNo_In)
            //================================================================================================
            {
                Double pR_T2 = (mT2.D() / 2);
                Double pDist_Center = mT2.D() - Overlap();

                string pArcDir = "";
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)     //....CCW
                {
                    pArcDir = "G03";
                }
                else                                                                                        //....CW.
                    pArcDir = "G02";


                    //  TAPERLAND.
                    //  ==========

                    //      Sub Routine - Individual.
                    //      -------------------------
                    //
                    SW_In.WriteLine("N" + LineNo_In++ + " (SUB. 1, MILL TAPERLAND)");        //....Comment - Header.
               
                    String pStr_LastLine_Seg = "";

                    int iBeg = 0;
                    Boolean pbln_FirstPass = false;

                    if (((mShroud.Ri * 2) - mPadD[0]) < modMain.gcEPS)      //....If Pad ID = Shroud ID then add one more pass to the left of Shroud ID   //BG 16APR13    
                    {
                        iBeg = -1;
                        pbln_FirstPass = true;
                    }
                    else
                    {
                        iBeg = 0;
                        pbln_FirstPass = true;
                    }


                    for (int i = iBeg; i < CountPass_TL(); i++)              //....Loop over Passes.
                    {
                        //  ith Pass
                        //  --------

                        //....Arc Radius:
                        Double pRArc = mShroud.Ri + pR_T2 + i * pDist_Center;

                        //....Depth on Feed Grv axis @ pRArc.
                        Double pDepth_FeedGrv = DepthOnFeedGroove(pRArc);


                        //....Begin Point:
                        Double pXBeg, pYBeg;
                        pXBeg = pRArc;
                        pYBeg = 0.0F;           //....For reference. Not used.


                        //....End Point:
                        //
                            //....Taper Angle Direction.
                            int pFac = 0; 
                            if (EndConfig_Indx_In == 0)             //....Front TB - CCW.
                            {
                                pFac = 1; 
                            }
                            else if (EndConfig_Indx_In == 1)        //....Back TB - CW.
                            {
                                pFac = -1;
                            }


                            //....Determine the Angular position.
                            //
                            Double pDel_Theta;
                            pDel_Theta = (pR_T2 / pRArc) * modMain.gc_RAD2DEG;

                            Double pTheta_End, pTheta_End_Rad;
                            pTheta_End = mTaper.Angle - pDel_Theta;
                            pTheta_End_Rad = pTheta_End / modMain.gc_RAD2DEG;



                            //....Cordinates.
                            Double pXEnd, pYEnd;
                            pXEnd = Math.Abs(pRArc * Math.Cos(pTheta_End_Rad));
                            pYEnd = pFac * Math.Abs(pRArc * Math.Sin(pTheta_End_Rad));


                        //....Go to the Begin Point on the Feed Groove Axis.
                        String pStr_1stLine;
                        pStr_1stLine = "N" + LineNo_In++ + " G01 X" + pXBeg.ToString("#0.000") + " Y0" ;

                        if (pbln_FirstPass)     //....First Pass.
                        //if (iBeg == 0 || iBeg == -1)     //....First Pass.
                        {
                            //pStr_1stLine = pStr_1stLine + " F" + mG0Speed.ToString("#0");
                            //BG 02APR13  As per HK's instruction in email dated 01APR13.
                            pStr_1stLine = pStr_1stLine + " F" + G0Speed.ToString("#0") + ".";

                            //....Save the G Code Line segment for the End Pt of the 1st Pass. To be needed for the last operation.
                            //........Last operation: Go from the End Pt of the last Pass to the End Pt of the 1st Pass.   
                            pStr_LastLine_Seg =  pXEnd.ToString("#0.000") + " Y" + pYEnd.ToString("#0.000");
                            pbln_FirstPass = false;
                        }

                        SW_In.WriteLine(pStr_1stLine);        
                        //SW_In.WriteLine("N" + LineNo_In++ + " Z-" + mDepth_TL_BackLash + " F" + mFeedRate.Taperland.ToString("#0"));  //....Go down to the Backlash depth.        
                        //BG 02APR13  As per HK's instruction in email dated 01APR13.
                        SW_In.WriteLine("N" + LineNo_In++ + " Z-" + mDepth_TL_BackLash + " F" + mFeedRate.Taperland.ToString("#0") + ".");  //....Go down to the Backlash depth.
                        SW_In.WriteLine("N" + LineNo_In++ + " Z-" + pDepth_FeedGrv.ToString("#0.0000"));       //....Move to the desired depth.                   
                        SW_In.WriteLine("N" + LineNo_In++ + " G04 P" + mDepth_TL_Dwell_T.ToString("#0"));       //....Dwell.

                        //....Go to the End Point on the Taper Angle.
                        SW_In.WriteLine("N" + LineNo_In++ + " " + pArcDir + " X" + pXEnd.ToString("#0.000") + " Y" + pYEnd.ToString("#0.000") + " I-" + pRArc.ToString("#0.000") + " Z0");


                        if (i < (CountPass_TL() - 1))
                        {
                            //....Lift the Cutter to Z = 0.025" before moving for the next Pass. 
                            //........This operation is skipped for the last operation e.g. G0 along the Taper Angle edge. 
                            //SW_In.WriteLine("N" + LineNo_In++ + " G01 Z0.025 F" + mG0Speed.ToString("#0"));
                            //BG 02APR13  As per HK's instruction in email dated 01APR13.
                            SW_In.WriteLine("N" + LineNo_In++ + " G01 Z0.025 F" + mG0Speed.ToString("#0") + ".");
                        }
                    }

                    SW_In.WriteLine("N" + LineNo_In++ + " G01 X" +  pStr_LastLine_Seg);
                    //SW_In.WriteLine("N" + LineNo_In++ + " G01 Z0.025 F" + mG0Speed.ToString("#0"));
                    //BG 02APR13  As per HK's instruction in email dated 01APR13.
                    SW_In.WriteLine("N" + LineNo_In++ + " G01 Z0.025 F" + mG0Speed.ToString("#0") + ".");
                    SW_In.WriteLine("N" + LineNo_In++ + " G69");                            //....Cancel Rotation G68.
                    SW_In.WriteLine("N" + LineNo_In++ + " M99");                            //....Sub-Program Return.
                    SW_In.WriteLine("N" + LineNo_In++);


                    //      Supervisory Sub Routine - All.
                    //      ------------------------------
                    //    
                    SW_In.WriteLine("N" + LineNo_In++ + " (LOC. SUB. 2, ROTATE TAPERLAND)");        //....Comment - Header.

                    Double pAngle_Seg = 360 / mPad_Count;

                    for (int j = 0; j < mPad_Count; j++)                //....Loop over Segments.
                    {
                        SW_In.WriteLine("N" + LineNo_In++ + " G17 G68 R" + (pAngle_Seg * (j + 1)) + ". X0 Y0");     //....Circular Motion XY Plane Selection and Rotation.
                        //SW_In.WriteLine("N" + LineNo_In++ + " G90 M97 P" + (mStarting_LineNo + 1));                 //....Call Sub Routine for TaperLand. 
                        //BG 02APR13  As per HK's instruction in email dated 01APR13.
                        SW_In.WriteLine("N" + LineNo_In++ + " G90 M97 P" + (mStarting_LineNo + 1) + " L1");                 //....Call Sub Routine for TaperLand. 
                        SW_In.WriteLine("N" + LineNo_In++ + " G69");                                                //....Cancel Rotation G68.
                    }

                    SW_In.WriteLine("N" + LineNo_In++ + " M99");            //....Sub-Program Return.
                    SW_In.WriteLine("N" + LineNo_In++);                     //....Blank line.    
            }


            private void WriteFile_GCode_WeepSlots(StreamWriter SW_In, ref int LineNo_In)
            //===========================================================================
            {
                //    WEEP SLOTS.
                //    ========== 

                //     Sub Routine - Individual.
                //     -------------------------
                //
                //mWeepSlot_RMargin = mT3.D() / 2 + mc_WS_RMARGIN;
                //Double pR_ID = mShroud.Ro - mc_WS_RMARGIN;         //....Begin Point on the Feed Groove Axis. 
                //Double pR_ID = ((mFeedGroove.DBC / 2) - (mFeedGroove.Wid / 2)) - mRMargin_WeepSlot;         //....Begin Point on the Feed Groove Axis.      //BG 03SEP12  //BG 03APR13
                Double pR_ID = (mFeedGroove.DBC / 2) - mc_WS_RMARGIN;         //....Begin Point on the Feed Groove Axis.      //BG 03APR13
                //Double pR_OD = (mPadD[1] / 2) + mc_WS_RMARGIN;     //....End Point.
                //Double pR_OD = (mPadD[1] / 2) + mRMargin_WeepSlot;     //....End Point.     //BG 03SEP12  //BG 03APR13
                Double pR_OD = (mPadD[1] / 2) + (mT3.D() / 2) + mc_WS_RMARGIN;     //....End Point.     //BG 03APR13

                SW_In.WriteLine("N" + LineNo_In++ + " (LOC. SUB. 3, MILL WEEP SLOTS)");             //....Comment - Header.
                int pStarting_LineNo_WS = LineNo_In;              //....Store the First Line No of Weep Slot for Supervisory Sub Routine.


                //....Determine # of Passes:
                //
                int pCountPass_FullDepth, pCountPass_WS;
                pCountPass_FullDepth = (int)(Math.Floor(mWeepSlot.Depth / mc_DEPTH_WS_CUT_PER_PASS_DEF));

                Double pRemainder_Depth = mWeepSlot.Depth % mc_DEPTH_WS_CUT_PER_PASS_DEF;

                if (pRemainder_Depth < modMain.gcEPS)
                {
                    pCountPass_WS = pCountPass_FullDepth;
                }
                else
                {
                    pCountPass_WS = pCountPass_FullDepth + 1;
                }

                Double pR_Beg = 0.0, pR_End = 0.0;
                Double pDepth_Tot = 0.0;

                for (int i = 0; i < pCountPass_WS; i++)               //....Loop over Passes.
                {
                    //  Begin point.
                    //  ------------
                    //
                    if (i % 2 == 0)
                    {
                        //....i = even.
                        pR_Beg = pR_ID;
                        pR_End = pR_OD;
                    }
                    else
                    {
                        //....i = odd.
                        pR_Beg = pR_OD;
                        pR_End = pR_ID;
                    }


                    //  Depth of Cut.
                    //  -------------
                    //
                    Double pDeptht_i = 0.0;

                    if (i != pCountPass_WS - 1 && pCountPass_WS > 1)
                    {
                        //....First Pass (but shouldn't also be the last pass).
                        pDeptht_i = mc_DEPTH_WS_CUT_PER_PASS_DEF;
                    }

                    else if (i == pCountPass_WS - 1)
                    {
                        //....Last Pass.
                        if (pRemainder_Depth < modMain.gcEPS)
                        {
                            pDeptht_i = mc_DEPTH_WS_CUT_PER_PASS_DEF;
                        }
                        else
                        {
                            pDeptht_i = pRemainder_Depth;
                        }
                    }

                    //....Total up to the current pass.
                    pDepth_Tot = pDepth_Tot + pDeptht_i;



                    //  Write G-Code lines:
                    //  -------------------
                    //
                    if (i == 0)                 //....First Pass.
                    {
                        //SW_In.WriteLine("N" + LineNo_In++ + " G01 X" + pR_Beg.ToString("#0.0000") + " Y0 F" + mG0Speed.ToString("#0"));            //....Beg. Pt.
                        //BG 02APR13  As per HK's instruction in email dated 01APR13.
                        SW_In.WriteLine("N" + LineNo_In++ + " G01 X" + pR_Beg.ToString("#0.0000") + " Y0 F" + mG0Speed.ToString("#0") + ".");            //....Beg. Pt.
                        SW_In.WriteLine("N" + LineNo_In++ + " Z0");
                        //SW_In.WriteLine("N" + LineNo_In++ + " Z-" + pDepth_Tot.ToString("#0.000") + " F" + mFeedRate.WeepSlot.ToString("#0"));  //....Apply total depth.
                        //BG 02APR13  As per HK's instruction in email dated 01APR13.
                        SW_In.WriteLine("N" + LineNo_In++ + " Z-" + pDepth_Tot.ToString("#0.000") + " F" + mFeedRate.WeepSlot.ToString("#0") + ".");  //....Apply total depth.
                        SW_In.WriteLine("N" + LineNo_In++ + " X" + pR_End.ToString("#0.0000"));                                                   //....End. Pt.
                    }
                    else                        //....Other Pass.
                    {
                        SW_In.WriteLine("N" + LineNo_In++ + " Z-" + pDepth_Tot.ToString("#0.000"));             //....Apply total depth.
                        SW_In.WriteLine("N" + LineNo_In++ + " X" + pR_End.ToString("#0.0000"));                   //....End. Pt.
                    }
                }

                SW_In.WriteLine("N" + LineNo_In++ + " X" + pR_Beg.ToString("#0.0000"));               //....Scraping.
                //SW_In.WriteLine("N" + LineNo_In++ + " Z0.1 " + "F" + mG0Speed.ToString("#0"));      //....Lift the Cutter to Z = 0.1" before moving to next subroutine.
                //BG 02APR13  As per HK's instruction in email dated 01APR13.
                SW_In.WriteLine("N" + LineNo_In++ + " Z0.1 " + "F" + mG0Speed.ToString("#0") + ".");      //....Lift the Cutter to Z = 0.1" before moving to next subroutine.

                SW_In.WriteLine("N" + LineNo_In++ + " G69");                    //....Cancel Rotation G68.
                SW_In.WriteLine("N" + LineNo_In++ + " M99");                    //....Sub-Program Return.
                SW_In.WriteLine("N" + LineNo_In++);                             //....Blank line.


                //      Supervisory Sub Routine - All.
                //      ------------------------------
                //
                SW_In.WriteLine("N" + LineNo_In++ + " (LOC. SUB. 4, ROTATE WEEP SLOT)");            //....Comment - Header.

                Double pAngle_Seg = 360 / mPad_Count;

                for (int j = 0; j < mPad_Count; j++)                    //....Loop over Segments.
                {
                    SW_In.WriteLine("N" + LineNo_In++ + " G17 G68 R" + (pAngle_Seg * (j + 1)) + ". X0 Y0");     //....Circular Motion XY Plane Selection and Rotation.
                    //SW_In.WriteLine("N" + LineNo_In++ + " G90 M97 P" + pStarting_LineNo_WS);                    //....Call Sub Routine for Weep Slots.      
                    //BG 02APR13  As per HK's instruction in email dated 01APR13.
                    SW_In.WriteLine("N" + LineNo_In++ + " G90 M97 P" + pStarting_LineNo_WS + " L1");                    //....Call Sub Routine for Weep Slots.      
                    SW_In.WriteLine("N" + LineNo_In++ + " G69");                                                //....Cancel Rotation G68.
                }
                SW_In.WriteLine("N" + LineNo_In++ + " M99");            //....Sub-Program Return.
                SW_In.WriteLine("N" + LineNo_In++);                     //....Blank line.
            }

        #endregion
    }
}
