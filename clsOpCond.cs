
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsOpCond                              '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  10JAN13                                '
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
using System.Runtime.Serialization.Formatters.Binary;     
using System.IO;                                        
using System.Runtime.Serialization;                     

namespace BearingCAD20
{
    [Serializable]
    public class clsOpCond :ICloneable
    {
       
        #region " ENUMERATION TYPES:"
        //===========================

            //public enum eRotDir {CW, CCW};                      
            public enum eRotDirectionality { Uni, Bi };        
        
        #endregion


        #region "STRUCTURE TYPES:"
        //========================
            [Serializable]          
            public struct sLube
            {
                public String Type;
                public Double API_sg;
                public Double Temp_a;
                public Double CSt_a;
                public Double Temp_b;
                public Double CSt_b;
            }

            [Serializable]          
            public struct sOilSupply
            {
                public sLube Lube;      
                public string Type;
                public Double Press;        //....English Unit.
                public Double Temp;         //....English Unit.   
            }

        #endregion


        #region " MEMBER VARIABLE DECLARATIONS: "
        //======================================

            private int[] mSpeed_Range = new int[2];            //BG 22NOV11 

            //private eRotDir mRot_Direction;             
            private eRotDirectionality mRot_Directionality;

            //....PB 10JAN13. If it is more appropriate to place the radial & Thrust loads & angle in the respective classes,
            //........they may be moved there.
            //
            private Double[] mRadial_Load_Range = new Double[2];       //....English Unit.
            private Double mRadial_LoadAng;

            private Double[] mThrust_Load_Range_Front = new Double[2];
            private Double[] mThrust_Load_Range_Back = new Double[2];

            private sOilSupply mOilSupply;

        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //===============================

            //Speed:
            //------
            public int[] Speed_Range
                {
                    get { return mSpeed_Range; }
                    set { mSpeed_Range = value; }
                }

 
            //Rotation:
            //---------
               
                public eRotDirectionality Rot_Directionality
                {
                    get { return mRot_Directionality; }
                    set { mRot_Directionality = value; }
                }


            //Static Load: (Unit = English)
            //------------
                public Double[] Radial_Load_Range
                {
                    get { return mRadial_Load_Range; }
                    set { mRadial_Load_Range = value; }
                }

                public Double Radial_LoadAng
                {
                    get { return mRadial_LoadAng; }
                    set { mRadial_LoadAng = value; }
                }

                public Double[] Thrust_Load_Range_Front
                {
                    get { return mThrust_Load_Range_Front; }
                    set { mThrust_Load_Range_Front = value; }
                }

                public Double[] Thrust_Load_Range_Back
                {
                    get { return mThrust_Load_Range_Back; }
                    set { mThrust_Load_Range_Back = value; }
                }


            //Oil Supply:
            //-----------
                
                //GET:
                //----
                    public sOilSupply OilSupply
                    {
                        get { return mOilSupply; }
                    }


                //SET:
                //----
                    public string OilSupply_Lube_Type
                    {
                        set { mOilSupply.Lube.Type = value; }
                    }

                    public string OilSupply_Type
                    {
                        set { mOilSupply.Type = value; }
                    }

                    public Double OilSupply_Press         //....English Unit.
                    {
                        set { mOilSupply.Press = value; }
                    }

                    public Double OilSupply_Temp          //....English Unit.
                    {
                        set { mOilSupply.Temp = value; }
                    }
 
            #endregion


        //Class Constructor.
        //==================

            public clsOpCond()
            {
                mRot_Directionality = eRotDirectionality.Bi;    //SB 13JUL09
            }


        #region " CLASS METHODS: "
        //========================

            #region "DEPENDENT VARIABLES:
            //===========================
        
                public int Speed()       //BG 22NOV11
                //-------------------
                {
                    return modMain.Nom_Val(mSpeed_Range);
                }

                public Double Radial_Load()                     
                //--------------------------
                {
                    return modMain.Nom_Val(mRadial_Load_Range);
                }

                public Double Thrust_Load_Front()
                //-------------------------------
                {
                    return modMain.Nom_Val(mThrust_Load_Range_Front);
                }

                public Double Thrust_Load_Back()
                //-------------------------------
                {
                    return modMain.Nom_Val(mThrust_Load_Range_Back);
                }

            #endregion


            #region "DATABASE  RELATED  ROUTINES:"
            //===================================

                public void GetData_Lubricant(string Lube_In, clsDB DB_In)
                //======================================================== 
                {
                    string pstrFIELDS, pstrFROM, pstrWHERE;
                    string pstrSQL;

                    pstrFIELDS = "* ";
                    pstrFROM = "FROM tblData_Lube "; 
                    pstrWHERE = "WHERE fldType = '" + Lube_In + "'";

                    pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;
                   
                    SqlConnection pConnection = new SqlConnection();   //SB 06JUL09
                    SqlDataReader pDR = null;
                    pDR = DB_In.GetDataReader(pstrSQL, ref pConnection);

                    if (pDR.Read())
                    {
                        mOilSupply.Lube.API_sg = Convert.ToDouble(pDR["fldAPI_sg"]);
                        mOilSupply.Lube.Temp_a = Convert.ToDouble(pDR["fldTemp_a"]);
                        mOilSupply.Lube.Temp_b = Convert.ToDouble(pDR["fldTemp_b"]);
                        mOilSupply.Lube.CSt_a = Convert.ToDouble(pDR["fldCSt_a"]);
                        mOilSupply.Lube.CSt_b = Convert.ToDouble(pDR["fldCSt_b"]);
                    }

                    pDR.Close();

                    pConnection.Close();
                    pDR = null;
                }

            #endregion


            //#region "CLASS OBJECTS COPYING & COMPARISON ROUTINES "
            ////----------------------------------------------------

            //    public bool Compare(ref clsOpCond OpCond_In)    
            //    //==========================================
            //    {
                    
            //        Boolean mblnVal_Changed = false;
            //        int pRetValue = 0;

            //        if (modMain.CompareVar(OpCond_In.Speed_Range[0], mSpeed_Range[0],3, pRetValue) > 0)   //SB 06APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Speed(), Speed(), 3,pRetValue) > 0)                  //SB 06APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Speed_Range[0], mSpeed_Range[0],3, pRetValue) > 0)   //SB 06APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Rot_Directionality.ToString(), 
            //            mRot_Directionality.ToString(), pRetValue) > 0)             //SB 10APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Radial_Load_Range[0], mRadial_Load_Range[0],3, pRetValue) > 0)               //SB 13APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Radial_Load_Range[1], mRadial_Load_Range[1],3, pRetValue) > 0)                //SB 13APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Load(), Load(),3, pRetValue) > 0)                //SB 13APR09
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.Radial_LoadAng, mRadial_LoadAng,0, pRetValue) > 0)
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.OilSupply.Lube.Type, mOilSupply.Lube.Type, pRetValue) > 0)
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.OilSupply.Type, mOilSupply.Type, pRetValue) > 0)
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.OilSupply.Press, mOilSupply.Press,3, pRetValue) > 0)
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;
            //        }

            //        if (modMain.CompareVar(OpCond_In.OilSupply.Temp, mOilSupply.Temp,3, pRetValue) > 0)
            //        {
            //            mblnVal_Changed = true;
            //            return mblnVal_Changed;

            //        }

            //        return mblnVal_Changed;
            //    }

            //#endregion

        #endregion                         


        #region " ICLONEABLE MEMBERS: "

            public object Clone()
            //===================           //SB 31MAR09
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
    }
}
