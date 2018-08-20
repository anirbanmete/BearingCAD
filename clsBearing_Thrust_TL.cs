
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsThrustBearing                       '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  27JUL18                                '
//                                                                              '
//===============================================================================
//

//PB 20DEC12. BG, clean up.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;       

namespace BearingCAD21
{
    [Serializable]
    public partial class clsBearing_Thrust_TL : clsEndConfig, ICloneable 
    {

        #region "NAMED CONSTANTS:"
        //========================
        private const Double mc_LINING_T = 0.0F;//0.030F;
            private const Double mc_LAND_L = 0.060F;

            private const Double mc_FACEOFF_ASSY_DEF = 0.010;           //BG 04APR13

            private const Double mc_BACK_RELIEF_DEPTH = 0.020;
            private const Double mc_BACK_RELIEF_FILLET = 0.010;

            private int mPAD_COUNT = 6;   

        #endregion


        #region " ENUMERATION TYPES:"
        //===========================

            //public enum eDirectionType { Uni, Bi, Bumper };
            public enum eDirectionType { Uni, Bi };
            public enum eRotation { CCW,CW };  

        #endregion


        #region "USER-DEFINED STRUCTURES:"
        //===============================    

            [Serializable]
            public struct sTaper
            {
                public Double Depth_ID;
                public Double Depth_OD;
                public Double Angle;
            }
                   
            [Serializable]
            public struct sShroud
            {
                public Double Ri;
                public Double Ro;
                public Double OD;           
                public Double ID;
            }
            
            [Serializable]
                public struct sLiningT          //.... Lining Thickness.
                {
                    public Double Face;
                    public Double ID;
                }

            [Serializable]
            public struct sBackRelief          //.... Back Relief.
            {
                public Boolean Reqd;
                public Double D;
                public Double Depth;
                public Double Fillet;
            }
                                        
        #endregion 


        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================
            private eDirectionType mDirectionType;
          
            //....Pad:
            private int mPad_Count;
            private Double[] mPadD;                                     // ID: 0  & OD: 1.

            //....Bore.
            private Double mLandL;
  
            //....Materials:
            private sLiningT mLiningT;
          
            private sTaper mTaper;
            private sShroud mShroud;

            //....Back Relief
            private sBackRelief mBackRelief;

            //....Miscellaneous
            private Double mLFlange;
            private Double mFaceOff_Assy;          

        #endregion


        #region "Member Class Objects:"
            private clsFeedGroove mFeedGroove;
            private clsWeepSlot mWeepSlot;
            private clsPerformData mPerformData;
        #endregion
        

        #region "CLASS PROPERTY ROUTINES:"
        //===============================

            public Double LINING_T
            {
                get { return mc_LINING_T; }
            }

            public Double LAND_L
            {
                get { return mc_LAND_L; }
            }

            public Double FACEOFF_ASSY_DEF
            {
                get { return mc_FACEOFF_ASSY_DEF; }
            }


            public Double BACK_RELIEF_DEPTH
            {
                get { return mc_BACK_RELIEF_DEPTH; }
            }

            public Double BACK_RELIEF_FILLET
            {
                get { return mc_BACK_RELIEF_FILLET; }
            } 

        
            public eDirectionType DirectionType
            {
                get { return mDirectionType; }
                set { mDirectionType = value; }
            }

       
            #region "Pad:"

                public int Pad_Count
                //==================
                {
                    get
                    {
                        if (mPad_Count < modMain.gcEPS)
                            return mPAD_COUNT;
                        else
                            return mPad_Count;
                    }
                    set { mPad_Count = value; }
                }


                public Double[] PadD
                //===================
                {
                    get { return mPadD; }
                    set { mPadD = value; }
                }

            #endregion


            #region "Bore:"

                public Double LandL
                //==================
                {
                    get {
                        if (mLandL < modMain.gcEPS)
                            return mc_LAND_L;                     
                        else
                            return mLandL; }
                    set { mLandL = value; }
                }

            #endregion


            #region "Face & ID Lining Thicknesses:"

                public sLiningT LiningT
                {
                    get 
                    {
                        mLiningT.Face = mc_LINING_T;
                        mLiningT.ID = mc_LINING_T;

                        //if (mLiningT.Face < modMain.gcEPS)
                        //    mLiningT.Face = mc_LINING_T;

                        //if (mLiningT.ID < modMain.gcEPS)
                        //    mLiningT.ID = mc_LINING_T;

                        return mLiningT; 
                    }
                }


                public Double LiningT_Face
                {
                    set { mLiningT.Face = value; }
                }

                public Double LiningT_ID
                {
                    set { mLiningT.ID = value; }
                }

            #endregion


            #region "Taper:"

                public sTaper Taper
                {
                    get { return mTaper; }
                }

                public Double Taper_Depth_OD
                {
                    set { mTaper.Depth_OD = value; }
                }

                public Double Taper_Depth_ID
                {
                    set { mTaper.Depth_ID = value; }
                }

                public Double Taper_Angle
                {
                    set { mTaper.Angle = value; }
                }

            #endregion


            #region "Shroud:"

                public sShroud Shroud
                {
                    get
                    {
                        if (mShroud.Ri < modMain.gcEPS || mShroud.Ri < 0.5 * mPadD[0])
                            mShroud.Ri = 0.5 * mPadD[0];

                        return mShroud;
                    }
                }


                public Double Shroud_Ri
                {
                    set
                    {
                        mShroud.Ri = value;
                        //if (value >= 0.5 * mPadD[0])
                        //    mShroud.Ri = value;
                        //else
                        //    mShroud.Ri = 0.5 * mPadD[0];
                    }
                }

                public Double Shroud_Ro
                {
                    set { mShroud.Ro = value; }
                }

                public Double Shroud_OD
                {
                    set { mShroud.OD = value; }
                }

                public Double Shroud_ID
                {
                    set { mShroud.ID = value; }
                }

            #endregion


            #region "Back Relief:"

                public sBackRelief BackRelief
                {
                    get
                    {
                        if (mBackRelief.Depth < modMain.gcEPS)
                            mBackRelief.Depth = mc_BACK_RELIEF_DEPTH;
         
                        return mBackRelief;
                    }
                }


                public Boolean BackRelief_Reqd
                {
                    set { mBackRelief.Reqd = value; }
                }

                public Double BackRelief_D
                {
                    set { mBackRelief.D = value; }
                }

                public Double BackRelief_Depth
                {
                    set { mBackRelief.Depth = value; }
                }

                public Double BackRelief_Fillet
                {
                    set { mBackRelief.Fillet = value; }
                }

            #endregion


            #region "Miscellaneous:"
        
                public Double LFlange
                {
                    get { return mLFlange; }
                    set { mLFlange = value; }
                }


                public Double FaceOff_Assy
                {
                    get { return mFaceOff_Assy; }
                    set { mFaceOff_Assy = value; }
                }

            #endregion


            #region "Feed Groove:"

                public clsFeedGroove FeedGroove
                {
                    get { return mFeedGroove; }
                    set { mFeedGroove = value; }
                }

             #endregion


            #region "Weep Slot:"

                public clsWeepSlot WeepSlot
                {
                    get { return mWeepSlot; }
                    set { mWeepSlot = value; }
                }          

            #endregion


            #region "Perform Data:"

                public clsPerformData PerformData
                {
                    get { return mPerformData; }
                    set { mPerformData = value; }
                }

            #endregion            

        #endregion


        #region "CLASS CONSTRUCTOR:"

                public clsBearing_Thrust_TL(clsUnit.eSystem UnitSystem_In, eType Type_In, clsDB DB_In, clsProduct CurrentProduct_In)
                    : base(UnitSystem_In, Type_In, DB_In, CurrentProduct_In)
                //====================================================================================================================
                {
                    mPadD = new Double[2];

                    mFeedGroove = new clsFeedGroove(this);
                    mWeepSlot = new clsWeepSlot();
                    mPerformData = new clsPerformData();

                    ////mT1 = new clsEndMill();
                    ////mT2 = new clsEndMill();
                    ////mT3 = new clsEndMill();
                    ////mT4 = new clsEndMill();

                    ////mT2.Type = clsEndMill.eType.Flat;
                    ////mT4.Type = clsEndMill.eType.Chamfer;

                    ////mOverlap_frac = mc_DESIGN_OVERLAP_FRAC;
                    ////mFeedRate.Taperland = mc_FEED_RATE_TL_DEF;
                    ////mDepth_TL_BackLash = mc_DEPTH_TL_BACKLASH;
                    ////mDepth_TL_Dwell_T = mc_DEPTH_TL_DWELL_T;
                    ////mFeedRate.WeepSlot = mc_FEED_RATE_WEEPSLOT_DEF;
                    ////mDepth_WS_Cut_Per_Pass = mc_DEPTH_WS_CUT_PER_PASS_DEF;

                    Mat.Base = "Steel 4340";
                    Mat.LiningExists = false;
                    Mat.Lining = "Babbitt";
                }

        #endregion


        #region "CLASS METHODS:"
        
            #region "REF. / DEPENDENT VARIABLES:"

                public Double DimStart ()          
                //------------------------
                {
                    //....Ref. TL Thrust Bearing_Rev1_27OCT11: Col. AA
                    if(mBackRelief.Reqd)
                        return (0.15 - mLandL);
                    else
                        return (0.15 + mLandL);
                }


                public Double MountHoles_Depth_Tap_Drill()
                //=========================================
                {           
                    if ((MountHoles.Type == clsMountHoles.eMountHolesType.T && MountHoles.Thread_Thru)
                    || (MountHoles.Type == clsMountHoles.eMountHolesType.T))
                    {
                        return mLFlange;
                    }
                    else
                        return (MountHoles.Depth_Thread + 0.0625);
                }


                public Double Validate_Shroud_Ri(ref Double Shroud_Ri)
                //====================================================
                {
                    string pMsg = "Shroud ID should not be less than Pad ID.";

                    if (Shroud_Ri < 0.5 * mPadD[0])
                    {
                        MessageBox.Show(pMsg,"ERROR", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Shroud_Ri = 0.5 * mPadD[0];
                    }

                    return Shroud_Ri;
                }

                

            #endregion

        #endregion
        

       
        #region "ICLONEABLE MEMBERS:"
        //==========================

            public object Clone()
            //===================
            {
               
                BinaryFormatter pBinSerializer;
                StreamingContext pStreamContext;

                pStreamContext = new StreamingContext(StreamingContextStates.Clone);
                pBinSerializer = new BinaryFormatter(null, pStreamContext);

                MemoryStream pMemBuffer;
                pMemBuffer = new MemoryStream();

                //....Serialize the object into the memory stream
                pBinSerializer.Serialize(pMemBuffer, this);

                //....Move the stream pointer to the beginning of the memory stream
                pMemBuffer.Seek(0, SeekOrigin.Begin);


                //....Get the serialized object from the memory stream
                Object pobjClone;
                pobjClone = pBinSerializer.Deserialize(pMemBuffer);
                pMemBuffer.Close();   //....Release the memory stream.

                return pobjClone;    //.... Return the deeply cloned object.
            }

        #endregion


        #region "NESTED CLASSES:"

            #region "Class FeedGroove":
            [Serializable]

                public class clsFeedGroove
                //=========================
                {
                    #region "NAMED CONSTANTS:"
                    //========================
                        private const Double mc_DIST_CHAMF = 0.030F;

                    #endregion

                    #region "MEMBER VARIABLES:"
                    //=========================
                        private clsBearing_Thrust_TL mCurrent_Bearing_Thrust_TL;

                        private String mType;
                        private Double mWid;
                        private Double mDepth;
                        private Double mDBC;
                        private Double mDist_Chamf;    

                    #endregion


                    #region "CLASS PROPERTY ROUTINES:"
                    //================================  

                        public Double DIST_CHAMF
                        {
                            get { return mc_DIST_CHAMF; }
                        } 
                

                        public String Type                       
                        {
                            get { return mType; }
                            set { mType = value; }
                        }

                        public Double Wid                       
                        {
                            get { return mWid; }
                            set { mWid = value; }
                        }

                        public Double Depth                       
                        {
                            get
                            {
                                if (mDepth < modMain.gcEPS)
                                    mDepth = (2 * mCurrent_Bearing_Thrust_TL.WeepSlot.Depth);
                                return mDepth;
                            }
                            set { mDepth = value; }
                        }

                        public Double DBC                      
                        {
                            get {
                                  if (mDBC < modMain.gcEPS)
                                    mDBC = Calc_DBC();

                                  return mDBC; 
                                }

                            set { mDBC = value; }
                        }

                        public Double Dist_Chamf                        
                        {
                            get 
                            {
                                if (mDist_Chamf < modMain.gcEPS)
                                    return mc_DIST_CHAMF;
                                else
                                    return mDist_Chamf; 
                            }
                            set { mDist_Chamf = value; }
                        }

                    #endregion


                    #region "CONSTRUCTOR:"

                         public clsFeedGroove(clsBearing_Thrust_TL Current_Bearing_Thrust_TL_In)
                         //=====================================================================
                         {
                             mCurrent_Bearing_Thrust_TL = Current_Bearing_Thrust_TL_In;
                         }
                    #endregion


                    #region "CLASS METHODS:"

                        #region "REF. / DEPENDENT VARIABLES:"

                            private Double Calc_DBC()
                            //=======================
                            {
                                //....Ref. TL Thrust Bearing_Rev1_27OCT11: Col. AY
                                return modMain.MRound(1.95 * mCurrent_Bearing_Thrust_TL.Shroud.Ri, 0.01);
                            }

                        #endregion

                #endregion
        
                }

            #endregion

            #region "Class WeepSlot":
            [Serializable]

                public class clsWeepSlot
                //========================
                {
                        public enum eType { Rectangular, Circular, V_notch };
                     

                    #region "MEMBER VARIABLES:"
                    //=========================

                        private eType mType;
                        private Double mWid;
                        private Double mDepth;

                    #endregion


                    #region "CLASS PROPERTY ROUTINES:"
                    //================================  
                                     
                        public eType Type                                      
                        {
                            get { return mType; }
                            set { mType = value; }
                        }

                        public Double Wid                       
                        {
                            get { return mWid; }
                            set { mWid = value; }
                        }

                        public Double Depth
                        {
                            get { return mDepth; }
                            set { mDepth = value; }
                        }
            
                    #endregion


                    #region "CONSTRUCTOR:"

                         public clsWeepSlot()
                         {
                         }

                    #endregion

                }

            #endregion

            #region "Class Performance Data":
            [Serializable]

                public class clsPerformData
                //=========================
                {
                   #region "USER-DEFINED STRUCTURES:"
                    //================================
                    [Serializable]
                        public struct sPadMax
                        {
                           public Double Temp;
                           public Double Press;
                           //public Double Load;        //....This property has been suppressed as as per HK's instruction on 09JAN13
                          
                        }

                   #endregion


                   #region "MEMBER VARIABLES:"
                   //=========================

                       private Double mPower_HP;
                       private Double mFlowReqd_gpm;
                       private Double mTempRise_F;
                       private Double mTFilm_Min;
                       public Double mUnitLoad; 

                       //....Pad Maximums:
                       private sPadMax mPadMax;

                   #endregion


                   #region "CLASS PROPERTY ROUTINES:"
                   //================================

                       //.... Power (Eng Unit).
                       public Double Power_HP
                       {
                           get { return mPower_HP; }
                           set 
                           { 
                               mPower_HP = value;
                               mTempRise_F = Calc_TempRise_F();
                           }
                       }


                       //.... Flow Reqd (Eng Unit).
                       public Double FlowReqd_gpm
                       {
                           get { return mFlowReqd_gpm; }
                           set 
                           { 
                               mFlowReqd_gpm = value;
                               mTempRise_F = Calc_TempRise_F();
                           }
                       }


                       //.... Temp Rise (Eng Unit).
                       public Double TempRise_F
                       {
                           get      
                           {
                               if (Math.Abs(mTempRise_F) < modMain.gcEPS)
                                   mTempRise_F = Calc_TempRise_F();

                               return mTempRise_F;
                           }

                           set { mTempRise_F = value; }
                       }


                       //.... TFilm_Min (Eng Unit).
                       public Double TFilm_Min
                       {
                           get { return mTFilm_Min; }
                           set { mTFilm_Min = value; }
                       }


                       //.... UnitLoad
                       public Double UnitLoad           //....This property has been added as as per HK's instruction on 09JAN13
                       {
                           get { return mUnitLoad; }
                           set { mUnitLoad = value; }
                       }


                       //....Pad Maximums:
                       public sPadMax PadMax
                       {
                           get { return mPadMax; }
                       }


                       public Double PadMax_Temp
                       {
                           set { mPadMax.Temp = value; }
                       }


                       public Double PadMax_Press
                       {
                           set { mPadMax.Press = value; }
                       }
       
                       //public Double PadMax_Load
                       //{
                       //    set { mPadMax.Load = value; }
                       //}

  
                   #endregion


                   #region "CONSTRUCTOR:"

                       public clsPerformData()
                       //=====================
                       {

                       }

                   #endregion

                       #region "CLASS METHODS:"

                           public Double Calc_TempRise_F()
                           //=============================
                           {
                               Double pTempRise = 0.0;

                               if (mFlowReqd_gpm != 0.0)        //BG 30JAN13
                               {
                                   pTempRise = 12.4 * (mPower_HP / mFlowReqd_gpm);
                               }

                               return pTempRise;
                           }

                       #endregion

               }

           #endregion

        #endregion

    }
}
