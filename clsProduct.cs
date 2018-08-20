//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsProduct                             '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  23JAN13                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;  

namespace BearingCAD20
{
    [Serializable]
    public class clsProduct : ICloneable 
    {
        #region "ENUMERATION TYPES:"
        //==========================
            public enum eFaceID { Front, Back, Both };
            public enum eEndConfigState { Overhang = 0, Flush = 1, Inside = 1 };

        #endregion


        #region "MEMBER VARIABLES:"
        //=========================

            private clsUnit mUnit; // = new clsUnit();

            //COMPONENTS:
            //-----------
                private clsBearing mBearing;                                    //....Main Component.

                //....Front: 0, Back:1.
                private clsEndConfig[] mEndConfig ;                            //....End Components.
                private clsAccessories mAccessories;
            
            private Double mL_Available;                //....Constraint - Total available envelope length.
            
            //....Thrust Bearing specific (if one of the End Configs. is a Thrust Bearing). 
            //........Face location from Pad mid-point.
            private Double[] mDist_ThrustFace = new Double[2];          //....If T/B is present at both faces, the index (0 or 1) containing the non-zero
                                                                        //.........value determines the face datum used.  
        #endregion


        #region "PROPERTIES:"
        //===================

            public clsUnit Unit
            {
                get { return mUnit; }

                set 
                {   
                    mUnit = value;

                    mBearing.Unit.System      = mUnit.System;

                    mEndConfig[0].Unit.System = mUnit.System;
                    mEndConfig[1].Unit.System = mUnit.System;
                }
            }


            public clsBearing Bearing
            {
                get { return mBearing; }

                //....While setting the following value, declaration should be done with "new".
                set { mBearing = value; }             
            }


            public clsEndConfig[] EndConfig
            {
                get
                {
                    Set_Mount_ScrewSpecs_EndConfigs();
                    return mEndConfig;
                }

                //....PB 19DEC12: An arrayed class member variable doesn't need a SET Property. It is redundant.
                //........It is automatically there which means that it can't have a READ-ONLY proprty.
                //.........Furthermore, 
                set { mEndConfig = value; }
            }


            public clsAccessories Accessories
            {
                get { return mAccessories; }
                set { mAccessories = value; }         
            }
               
    
            public Double L_Available
            {
                get { return mL_Available; }
                set { mL_Available = value; }
            }
        

            #region "....T/B Specific (if any present):"

                public Double[] Dist_ThrustFace
                {
                    get 
                    {
                        //for (int i = 0; i < 2; i++)
                        //{
                        //    if (mDist_ThrustFace[i] < modMain.gcEPS)
                        //    {
                        //        mDist_ThrustFace[i] = Calc_Dist_ThrustFace(i);
                        //    }
                        //}
                        return mDist_ThrustFace; 
                    }

                    set { mDist_ThrustFace = value; }
                }

            #endregion

        #endregion


        //....Class Constructor
        //public clsProduct(clsUnit.eSystem UnitSystem_In, clsBearing.eType Type_In, clsBearing_Radial_FP.eDesign Design_In, 
        //                  clsEndConfig.eType [] EndConfigType_In, clsDB DB_In)
        ////================================================================================================================           
        //{
        //    mUnit.System = UnitSystem_In;

        //    //Main Component.
        //    //---------------
        //    //
        //    if (Type_In == clsBearing.eType.Radial)
        //    {
        //        if (Design_In == clsBearing_Radial.eDesign.Flexure_Pivot)
        //        {
        //            mBearing = new clsBearing_Radial_FP(UnitSystem_In, Type_In, Design_In, DB_In, this);
        //        }
        //    }
            
            
        //    //End Configurations:
        //    //-------------------
        //    //
        //    mEndConfig = new clsEndConfig[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (EndConfigType_In [i]== clsEndConfig.eType.Seal)                                 //....End Seal.
        //        {
        //            mEndConfig[i] = new clsSeal (UnitSystem_In, EndConfigType_In [i], DB_In, this);
        //        }
        //        else if (EndConfigType_In[i] == clsEndConfig.eType.Thrust_Bearing_TL)               //....End TB.
        //        {
        //            mEndConfig[i] = new clsBearing_Thrust_TL (UnitSystem_In, EndConfigType_In [i], DB_In, this);
        //        }
        //    }


        //    //Accessories.
        //    //---------------
        //    //
        //    mAccessories = new clsAccessories();
        //}


        public clsProduct(clsUnit.eSystem UnitSystem_In, clsDB DB_In)
        //===========================================================
        {
            //PB 23JAN13. At the time of instantiation of this object, the Bearing Type & Bearing Design, EndConfig [0,1] Types are
            //....known. Hence, instantiate mBearing & mEndConfig [0,1] using the above info. To be done in the future. 

            //  Initialize.
            //  -----------
            //
            //....Unit System.
            mUnit        = new clsUnit();       //....Default unit = English (automatically).
            mUnit.System = UnitSystem_In;
           

            //....Bearing.
            //mBearing = new clsBearing_Radial_FP(clsUnit.eSystem.English, clsBearing.eType.Radial,
            //                                    clsBearing_Radial.eDesign.Flexure_Pivot, DB_In, this);

            mBearing = new clsBearing_Radial_FP(mUnit.System, clsBearing.eType.Radial,
                                                clsBearing_Radial.eDesign.Flexure_Pivot, DB_In, this);

            //....End Configurations:
            clsEndConfig.eType[] pEndConfig = new clsEndConfig.eType[2];

            for (int i = 0; i < pEndConfig.Length; i++)
                pEndConfig[i] = clsEndConfig.eType.Seal;        //....Default cofigs = [Seal, Seal]


            mEndConfig = new clsEndConfig[2];

            for (int i = 0; i < 2; i++)
            {
                mEndConfig[i] = new clsSeal(mUnit.System, pEndConfig[i], DB_In, this);
            }


            //....Accessories.
            mAccessories = new clsAccessories();          
        }
        

        #region "CLASS METHODS:"
        //*********************

            #region "REF. / DEPENDENT VARIABLES:"
            
                public Double L_Tot()
                //-------------------
                {   
                    //....Relevant Radial Bearing Parameters:
                    //....Keep the following commented lines for the sake of history.
                    //double pEDM_Relief = ((clsBearing_Radial_FP)mBearing).DESIGN_EDM_RELIEF;
                    //double pEDM_Relief_Tot = ((clsBearing_Radial_FP)mBearing).EDM_Relief[0] + 
                    //                         ((clsBearing_Radial_FP)mBearing).EDM_Relief[1];     

                    double pEDM_Relief_Tot = ((clsBearing_Radial_FP)mBearing).MillRelief.EDM_Relief[0] + 
                                             ((clsBearing_Radial_FP)mBearing).MillRelief.EDM_Relief[1]; 
    
                    double pBearing_Pad_L = ((clsBearing_Radial_FP) mBearing).Pad.L;
                    
                    double pL_Tot = 0;


                    //....Store End Configs Depth & Lengths in local variables:
                    //
                        double[] pDepth_EndConfig = new double[2];
                        double[] pL_EndConfig = new double[2];

                        for (int i = 0; i < 2; i++)
                        {
                            pDepth_EndConfig[i] = ((clsBearing_Radial_FP)mBearing).Depth_EndConfig[i];
                            pL_EndConfig[i] = mEndConfig[i].L;
                        }
                 

                    //....Determine End Configs' State:   Overhang, Flush/Inside.
                    //
                        int[] pEndConfig_State = new int[2];
                        for (int j = 0; j < 2; j++)
                        {
                            if(pL_EndConfig[j] > pDepth_EndConfig[j])
                                pEndConfig_State[j] = (int) eEndConfigState.Overhang;
                            else 
                                 pEndConfig_State[j] = (int) eEndConfigState.Inside;    //....Also include Flush.
                        }


                    //Calculate Total Length of the Product Assembly.
                    //-----------------------------------------------
                    //
                        //....Case 1: Both End Configs are overhung. 
                        //
                            if (pEndConfig_State[0] == (int)eEndConfigState.Overhang &&
                                pEndConfig_State[1] == (int)eEndConfigState.Overhang)
                            {
                                pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pEDM_Relief_Tot + pL_EndConfig[1];
                            }


                        //....Case 2: Both End Configs are Flush / Inside. 
                        //
                            else if (pEndConfig_State[0] == (int)eEndConfigState.Inside &&
                                pEndConfig_State[1] == (int)eEndConfigState.Inside)
                            {
                                pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pEDM_Relief_Tot + pDepth_EndConfig[1];
                            }


                        //....Case 3: Front End Config = Inside & Back = Overhung.
                        //
                            else if (pEndConfig_State[0] == (int)eEndConfigState.Inside &&
                                pEndConfig_State[1] == (int)eEndConfigState.Overhang)
                            {
                                pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pEDM_Relief_Tot + pL_EndConfig[1];
                            }


                        //....Case 4: Front End Config = Overhung & Back = Flush/Inside.
                            else if (pEndConfig_State[0] == (int)eEndConfigState.Overhang &&
                                pEndConfig_State[1] == (int)eEndConfigState.Inside)
                            {
                                pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pEDM_Relief_Tot + pDepth_EndConfig[1];
                            }
  
                    return pL_Tot;
                }

            #endregion


            public Double Calc_L_EndConfig()
            //-------------------------------     
            {
                //....Default Case: Both End Configs' are of equal length.
                double pEDM_Relief_Tot = ((clsBearing_Radial_FP)mBearing).MillRelief.EDM_Relief[0] + 
                                         ((clsBearing_Radial_FP)mBearing).MillRelief.EDM_Relief[1];   
                double pBearing_Pad_L = ((clsBearing_Radial_FP)mBearing).Pad.L;

                Double pL = 0.0;
                pL = 0.5 * (mL_Available - (pBearing_Pad_L + pEDM_Relief_Tot));

                return pL;
            }


            public Double Calc_Dist_ThrustFace(int Indx_In)
            //---------------------------------------------
            {
                Double pBearing_Pad_L = ((clsBearing_Radial_FP)mBearing).Pad.L;
                Double pEDM_Relief = 0.0;
                Double pDist_ThrustFace = 0.0;
                Double pEndConfig_L = 0.0;


                if (mEndConfig[Indx_In].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    //pEDM_Relief = ((clsBearing_Radial_FP)mBearing).EDM_Relief[Indx_In];   //BG 06DEC12
                    pEDM_Relief = ((clsBearing_Radial_FP)mBearing).MillRelief.EDM_Relief[Indx_In];     //BG 06DEC12
                    pEndConfig_L = mEndConfig[Indx_In].L;
                    pDist_ThrustFace = pEndConfig_L + (0.5 * pBearing_Pad_L + pEDM_Relief);
                }              

                return pDist_ThrustFace;
            }


            #region "MOUNTING SCREWS:"

                private void Set_Mount_ScrewSpecs_EndConfigs ()
                //=============================================
                {
                    string pD_DesigF, pD_DesigB;

                    Boolean pGoThru                       = ((clsBearing_Radial_FP)mBearing).Mount.Holes_GoThru;
                    clsBearing_Radial_FP.eFaceID pBolting = ((clsBearing_Radial_FP)mBearing).Mount.Holes_Bolting;

                    if (pGoThru == true && pBolting == clsBearing_Radial_FP.eFaceID.Front)
                    {
                        //pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[0].D_Desig;
                        pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture[0].Screw_Spec.D_Desig;       //BG 24AUG12

                        mEndConfig[0].MountHoles.Screw_Spec.D_Desig = pD_DesigF;
                        mEndConfig[1].MountHoles.Screw_Spec.D_Desig = pD_DesigF;
                    }

                    else if (pGoThru == true && pBolting == clsBearing_Radial_FP.eFaceID.Back)
                    {
                        //pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[1].D_Desig;
                        pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture[1].Screw_Spec.D_Desig;      //BG 24AUG12

                        mEndConfig[0].MountHoles.Screw_Spec.D_Desig = pD_DesigB;
                        mEndConfig[1].MountHoles.Screw_Spec.D_Desig = pD_DesigB;
                    }

                    else if (pGoThru == false)
                    {
                        //....Bolting = 'Both'.
                        //pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[0].D_Desig;
                        //pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[1].D_Desig;

                        //BG 24AUG12
                        pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture[0].Screw_Spec.D_Desig;
                        pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture[1].Screw_Spec.D_Desig;

                        mEndConfig[0].MountHoles.Screw_Spec.D_Desig = pD_DesigF;
                        mEndConfig[1].MountHoles.Screw_Spec.D_Desig = pD_DesigB;
                    }
                }

            #endregion

        #endregion

        #region "ICLONEABLE MEMBERS:"
        //==========================

            public object Clone()
            //===================
            {
                //return this.MemberwiseClone();

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
    }
}
