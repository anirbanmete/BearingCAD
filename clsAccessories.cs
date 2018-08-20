//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsAccessories                         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  02MAR12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

// PB 13APR09. important Comment (some not good programming) & Cleaned up.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace BearingCAD21
{
    [Serializable]
    public class clsAccessories : ICloneable
    {
        #region "ENUMERATION TYPES:"  
        //==========================

            public enum eTempSensorName {TC, RTD}     
            public enum eTempSensorType {E, J, K, T}    //SB 07JUL09
            public enum eWireClipSize {S, M, L}

        #endregion


        #region "STRUCTURE TYPES:"
        //=======================
            [Serializable]
            public struct sTempSensor
            {
                public bool Supplied;            
                public eTempSensorName Name;
                public int Count;
                public eTempSensorType Type;
            }

            [Serializable]
            public struct sWireClip
            {
                public bool Supplied;    
                public int Count;
                public eWireClipSize Size;
            }

        #endregion


        #region "MEMBER VARIABLE DECLARATIONS: "
        //======================================

            private sTempSensor mTempSensor;
            private sWireClip mWireClip;

        #endregion


        #region "PROPERTY DECLARATIONS:"
        //============================  

            //Temp Sensor:
            //============
            
                //GET:
                //----
                public sTempSensor TempSensor
                {
                    get { return mTempSensor; }
                }


                //SET:
                //----
                public bool TempSensor_Supplied
                {
                    set 
                    { 
                        mTempSensor.Supplied = value;
                        if (!mTempSensor.Supplied)
                            mTempSensor.Count = 0;
                    }
                }


                public eTempSensorName TempSensor_Name
                {
                    set
                    {   if (mTempSensor.Supplied == true)   
                        {   mTempSensor.Name = value;}
                    }
                }

                public int TempSensor_Count
                {
                    set
                    {
                        if (mTempSensor.Supplied == true)       
                        {   mTempSensor.Count = value; }

                        else if (mTempSensor.Supplied == false) 
                        {   mTempSensor.Count = 0; }
                    }
                }

                public eTempSensorType TempSensor_Type 
                {
                    set
                    {
                        if (mTempSensor.Supplied == true)       
                        {   mTempSensor.Type = value; }
                    }
                }


            //Wire Clip:
            //==========
                
                //GET:
                //----
                public sWireClip WireClip
                {
                    get { return mWireClip; }}


                //SET:
                //----
                public bool WireClip_Supplied 
                {
                    set 
                    { 
                        mWireClip.Supplied = value;
                        if (!mWireClip.Supplied)
                            mWireClip.Count = 0;
                    }
                }

                public int WireClip_Count   
                {
                    set
                    {
                        if (mWireClip.Supplied == true)         
                        {   mWireClip.Count = value; }

                        else if (mWireClip.Supplied == false)   
                        {   mWireClip.Count = 0; }
                    }
                }

                public eWireClipSize WireClip_Size 
                {
                    set
                    {
                        if (mWireClip.Supplied == true) 
                        {   mWireClip.Size = value; }
                    }
                }

        #endregion


        //....Class Constructor.           
        public clsAccessories ()
         //=====================
         {

             mTempSensor.Supplied = false;  
             mWireClip.Supplied = false;    
         }


        #region "CLASS METHODS:"
        //=====================

            #region "DATABASE RELATED ROUTINE"

            public int AddRec_Accessories(string Proj_In, clsDB DB_In)                      
                //====================================================
                {
                    int pCountRec = 0;

                    try
                    {

                        string pFIELDS = "(fldProjectNo,fldTempSensor_Supplied,fldTempSensor_Name,fldTempSensor_Count," +
                                         "fldTempSensor_Type,fldWireClip_Supplied,fldWireClip_Count,fldWireClip_Size)";                              //SB 10APR09

                        string pINSERT = "INSERT INTO tblProject_Accessories";

                        byte pTempSensor_Supplied = 0;
                        if (mTempSensor.Supplied)
                            pTempSensor_Supplied = 1;
                        else
                            pTempSensor_Supplied = 0;

                        byte pWireClip_Supplied = 0;
                        if (mWireClip.Supplied)
                            pWireClip_Supplied = 1;
                        else
                            pWireClip_Supplied = 0;


                        string pVALUES = "VALUES ('" + Proj_In + "',"                   
                                         + pTempSensor_Supplied + ",'"
                                         + mTempSensor.Name + "'," 
                                         + mTempSensor.Count + ",'"
                                         + mTempSensor.Type.ToString()+ "',"
                                         + pWireClip_Supplied + ","
                                         + mWireClip.Count + ",'"
                                         + mWireClip.Size.ToString() + "')";                                         

                        string pSQL = pINSERT + pFIELDS + pVALUES;

                        pCountRec = DB_In.ExecuteCommand(pSQL);

                    }
                    catch (Exception pEXP)
                    {
                        MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                    }

                    return pCountRec;
                }


                public int UpdateRec_Accessories(string ProjNo_In, clsDB DB_In)      
                //=============================================================
                {
                     //....UPDATE Records.
                     String pSET , pWHERE, pSQL;
                     int pCountRec = 0;

                     try
                     {
                         byte pTempSensor_Supplied = 0;
                         if (mTempSensor.Supplied)
                             pTempSensor_Supplied = 1;
                         else
                             pTempSensor_Supplied = 0;

                         byte pWireClip_Supplied = 0;
                         if (mWireClip.Supplied)
                             pWireClip_Supplied = 1;
                         else
                             pWireClip_Supplied = 0;


                         pSET = " SET fldTempSensor_Supplied = " + pTempSensor_Supplied +
                                ", fldTempSensor_Name = '" + mTempSensor.Name +
                                "', fldTempSensor_Count = " + mTempSensor.Count +
                                ", fldTempSensor_Type = '" + mTempSensor.Type.ToString() +
                                "', fldWireClip_Supplied = " + pWireClip_Supplied +
                                ", fldWireClip_Count = " + mWireClip.Count +
                                ", fldWireClip_Size = '" + mWireClip.Size.ToString() + "'" ;

                         pWHERE = " WHERE fldProjectNo = '" + ProjNo_In + "'";
                         pSQL = "UPDATE tblProject_Accessories" + pSET + pWHERE;
                         pCountRec = DB_In.ExecuteCommand(pSQL);

                     }
                     catch (Exception pEXP)
                     {
                         MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                     }

                     return pCountRec;
                }


                public void RetrieveRec_Accessories(string ProjNo_In, clsDB DB_In)     
                //================================================================
                {
                    ////string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                    ////pstrFIELDS = "* ";
                    ////pstrFROM = "FROM tblProject_Accessories ";         
                    ////pstrWHERE = "WHERE fldProjectNo = '" + ProjNo_In + "'";

                    ////pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;
                    ////SqlConnection pConnection = new SqlConnection();   //SB 06JUL09
                    
                    ////SqlDataReader pDR = null;
                    ////pDR = DB_In.GetDataReader(pstrSQL,ref pConnection);

                    ////if (pDR.Read())
                    ////{
                    ////    mTempSensor.Supplied = DB_In.CheckDBBool(pDR, "fldTempSensor_Supplied");

                    ////    if (mTempSensor.Supplied == true)        //PB 13APR09.
                    ////    {
                    ////        mTempSensor.Name = (eTempSensorName)Enum.Parse
                    ////                           (typeof(eTempSensorName), DB_In.CheckDBString(pDR, "fldTempSensor_Name"));
                    ////        mTempSensor.Count = DB_In.CheckDBInt(pDR, "fldTempSensor_Count");

                    ////        mTempSensor.Type = (eTempSensorType)Enum.Parse
                    ////                           (typeof(eTempSensorType), DB_In.CheckDBString(pDR, "fldTempSensor_Type"));
                    ////    }


                    ////    mWireClip.Supplied = DB_In.CheckDBBool(pDR, "fldWireClip_Supplied");

                    ////    if (mWireClip.Supplied == true)           //PB 13APR09.
                    ////    {
                    ////        mWireClip.Count = DB_In.CheckDBInt(pDR, "fldWireClip_Count");
                    ////        mWireClip.Size = (eWireClipSize)Enum.Parse
                    ////                         (typeof(eWireClipSize),DB_In.CheckDBString(pDR, "fldWireClip_Size"));
                    ////    }
                    ////}

                    ////pDR.Close();
                    ////pDR = null;

                    ////pConnection.Close();
                }

                public void DeleteRec_Accessories(string Proj_In, clsDB DB_In)    
                //============================================================
                {
                    string pSQL = "DELETE FROM tblProject_Accessories WHERE fldProjectNo = '" + Proj_In + "'";
                    DB_In.ExecuteCommand(pSQL);
                }

            #endregion


            #region "COMPARE & COPYING OBJECT"

                public bool Compare(ref clsAccessories Accessories_In)      
                //==================================================== 
                {
                    bool mblnVal_Changed = false;
                    int pRetValue = 0;

                    if (modMain.CompareVar(Accessories_In.TempSensor.Supplied, 
                                            mTempSensor.Supplied, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.TempSensor.Name.ToString(),
                                           mTempSensor.Name.ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.TempSensor.Count, 
                                           mTempSensor.Count, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.TempSensor.Type.ToString(), 
                                           mTempSensor.Type.ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.WireClip.Supplied, 
                                           mWireClip.Supplied, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.WireClip.Count, 
                                           mWireClip.Count, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    if (modMain.CompareVar(Accessories_In.WireClip.Size.ToString(), 
                                           mWireClip.Size.ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                    }

                    return mblnVal_Changed;
                }

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

        #endregion      
    }
}
