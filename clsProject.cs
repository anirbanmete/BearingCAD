
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsProject                             '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  23JAN13                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BearingCAD20
{
    [Serializable]
    public class clsProject  
    {

        #region "USER-DEFINED STRUCTURES:"
        //================================

            //....Customer
            [Serializable]
            public struct sCustomer             
            {
                public string Name;
                public string MachineDesc;
                public string PartNo;
            }
                
            //....AssyDwg
            [Serializable]   
            public struct sAssyDwg             
            {
                public string No;
                public string No_Suffix;
                public string Ref;
            }
        
            [Serializable]              
            public struct sPerson
            {
                public String Name;
                public String Initials;
                public DateTime Date;
            }

        #endregion

        
        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================

            private clsUnit mUnit = new clsUnit();
            private clsProduct mProduct;
            
            private string mStatus;
            private string mNo;
            private string mNo_Suffix;        
                   
            private sCustomer mCustomer;            
            private sAssyDwg mAssyDwg;             

            private sPerson mEngg ;
            private sPerson mDesignedBy;
            private sPerson mCheckedBy;

            private DateTime mDate_Closing;      
            
            //....File Paths
            private string mFilePath_CustEnqSheet;  

            private string mFilePath_Project;
            private string mFilePath_DesignTbls_SWFiles;
            
            //....Filter Related
            private Boolean mblnFiltered;
            private StringCollection mFiltered_Project_Nos;

            
            //SolidWorks Model Files Manual Modifications:
            //-------------------------------------------

            private Boolean mFileModified_CompleteAssy;  

            private Boolean mFileModified_RadialPart;
            private Boolean mFileModified_RadialBlankAssy;

            private Boolean mFileModified_EndTB_Part;
            private Boolean mFileModified_EndTB_Assy;

            private Boolean mFileModified_EndSeal_Part;
          

            private string mFileModification_Notes;      
                
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
            //============================

            public clsUnit Unit
            {
                get { return mUnit; }

                set 
                {   mUnit = value;
                    mProduct.Unit.System = mUnit.System;
                }
            }

            //.... Product
            public clsProduct Product
            {
                get { return mProduct; }
                set { mProduct = value; }
            }
        

            public string No           
            {
                get { return mNo; }
                set { mNo = value; }
            }


            public string No_Suffix           
            {
                get { return mNo_Suffix; }
                set { mNo_Suffix = value; }
            }


            public string Status          
            {
                get { return mStatus; }
                set { mStatus = value; }
            }


            #region "....Customer:"
                       
                public sCustomer Customer
                {
                    get { return mCustomer; }
                }


                public string Customer_Name
                {
                    set { mCustomer.Name = value; }
                }

                public string Customer_MachineDesc
                {
                    set { mCustomer.MachineDesc = value; }
                }

                public string Customer_PartNo
                {
                    set { mCustomer.PartNo = value; }
                }

            #endregion 


            #region "....AssyDwg:"
                       
                public sAssyDwg AssyDwg              
                {
                    get { return mAssyDwg; }
                }
        
               
                public string AssyDwgNo               
                {              
                    set { mAssyDwg.No = value; }
                }

                public string AssyDwgNo_Suffix               
                {
                    set { mAssyDwg.No_Suffix = value; }
                }

                public string AssyDwgRef               
                {              
                    set { mAssyDwg.Ref = value; }
                }

            #endregion


            #region "....Engineer:"

                public sPerson Engg               
                {
                    get { return mEngg; }
                }
  
               
                public string Engg_Name               
                {
                    set { mEngg.Name = value; }
                }

                public string Engg_Initials               
                {
                    set { mEngg.Initials = value; }
                }

                public DateTime Engg_Date               
                {
                    set { mEngg.Date = value; }
                }

            #endregion


            #region "....DesignedBy:"
               
                public sPerson DesignedBy               
                {
                    get { return mDesignedBy; }
                }
        
                
                public string DesignedBy_Name               
                {
                    set { mDesignedBy.Name = value; }
                }
        
                public string DesignedBy_Initials                
                {
                    set { mDesignedBy.Initials = value; }
                }
        
                public DateTime DesignedBy_Date               
                {
                    set { mDesignedBy.Date = value; }
                }

            #endregion


            #region "....CheckedBy:"

                public sPerson CheckedBy
                {
                    get { return mCheckedBy; }
                }

                
                public string CheckedBy_Name
                {
                    set { mCheckedBy.Name = value; }
                }


                public string CheckedBy_Initials
                {
                    set { mCheckedBy.Initials = value; }
                }


                public DateTime CheckedBy_Date
                {
                    set { mCheckedBy.Date = value; }
                }

            #endregion


            public DateTime Date_Closing            
            {
                get { return mDate_Closing; }
                set { mDate_Closing = value; }
            }


            #region "....File Path:"

                public string FilePath_CustEnqySheet                  
                {
                    get { return mFilePath_CustEnqSheet; }
                    set { mFilePath_CustEnqSheet = value; }
                }


                public string FilePath_Project                      
                {
                    get { return mFilePath_Project; }
                    set { mFilePath_Project = value; }
                }


                public string FilePath_DesignTbls_SWFiles                         
                {
                    get { return mFilePath_DesignTbls_SWFiles; }
                    set { mFilePath_DesignTbls_SWFiles = value; }
                }

            #endregion


            #region "....Filtered:"

                public Boolean Filtered               
                {
                    get { return mblnFiltered; }
                    set { mblnFiltered = value; }
                }
           
        
                public StringCollection Filtered_Project_No          
                {
                    get { return mFiltered_Project_Nos; }
                }
            #endregion


            #region "....File Modified:"

                public Boolean FileModified_CompleteAssy
                {
                    get { return mFileModified_CompleteAssy; }
                    set { mFileModified_CompleteAssy = value; }
                }

                public Boolean FileModified_RadialPart                      
                {
                    get { return mFileModified_RadialPart; }
                    set { mFileModified_RadialPart = value; }
                }


                public Boolean FileModified_RadialBlankAssy               
                {
                    get { return mFileModified_RadialBlankAssy; }
                    set { mFileModified_RadialBlankAssy = value; }
                }


                public Boolean FileModified_EndTB_Part               
                {
                    get { return mFileModified_EndTB_Part; }
                    set { mFileModified_EndTB_Part = value; }
                }


                public Boolean FileModified_EndTB_Assy                 
                {
                    get { return mFileModified_EndTB_Assy; }
                    set { mFileModified_EndTB_Assy = value; }
                }


                public Boolean FileModified_EndSeal_Part                         
                {
                    get { return mFileModified_EndSeal_Part; }
                    set { mFileModified_EndSeal_Part = value; }
                }


                public String FileModification_Notes              
                {
                    get { return mFileModification_Notes; }
                    set { mFileModification_Notes = value; }
                }

            #endregion

        #endregion


        #region "CLASS CONSTRUCTOR:"
                
            //BG 19DEC12
            //public clsProject(clsUnit.eSystem UnitSystem_In, clsBearing.eType Type_In, clsBearing_Radial_FP.eDesign Design_In,
            //                  clsEndConfig.eType[] EndConfigType_In, clsDB DB_In)
            ////================================================================================================================
            //{
            //    mUnit.System = UnitSystem_In;
            //    mProduct = new clsProduct(UnitSystem_In,Type_In, Design_In, EndConfigType_In, DB_In); 
               

            //    mFiltered_Project_Nos = new StringCollection();
            //}


            public clsProject(clsUnit.eSystem UnitSystem_In)
            //==============================================
            {
                mUnit.System = UnitSystem_In;
                mProduct = new clsProduct(mUnit.System, modMain.gDB);       
                //mProduct.Unit.System = mUnit.System;                      //PB 22JAN13.

                mFiltered_Project_Nos = new StringCollection();
            }

        #endregion


        #region "CLASS METHODS:"
        //=====================

            //public Boolean CheckProjStatus(string ProjNo_In)            //PB 29DEC11. SG, to be reviewed.
            ////==============================================
            //{
            //    Boolean pBln = false;
            //    string pstrWHERE = "";

            //    if (mNo_Suffix != "")
            //        pstrWHERE = "WHERE fldNo = '" + ProjNo_In + "' AND fldNo_Suffix = '" + mNo_Suffix + "'";
            //    else
            //        pstrWHERE = "WHERE fldNo = '" + ProjNo_In + "' AND fldNo_Suffix is NULL";


            //    string pSQL = "SELECT fldStatus FROM tblProject_Details " + pstrWHERE;

            //    SqlConnection pConnection = new SqlConnection();   //SB 06JUL09

            //    SqlDataReader pDR = modMain.gDB.GetDataReader(pSQL, ref pConnection);

            //    if (pDR.Read())
            //    {
            //        string pStatus = pDR["fldStatus"].ToString();
            //        if (pStatus == "Open")
            //            pBln = true;
            //        else
            //            pBln = false;
            //    }

            //    pDR.Close();
            //    pConnection.Close();

            //    return pBln;
            //}


            public Boolean ProjStatus_Open()           
            //==============================
            {
                Boolean pBln = false;

                string pstrWHERE = "";
                string pstrFields = "fldStatus";
                string pstrFrom = " FROM tblProject_Details";

                if (mNo_Suffix != "")
                    pstrWHERE = " WHERE fldNo = '" + mNo + "' AND fldNo_Suffix = '" + mNo_Suffix + "'";
                else
                    pstrWHERE = " WHERE fldNo = '" + mNo + "' AND fldNo_Suffix is NULL";


                string pSQL = "SELECT " + pstrFields + pstrFrom + pstrWHERE;


                SqlConnection pConnection = new SqlConnection();  
                SqlDataReader pDR = modMain.gDB.GetDataReader(pSQL, ref pConnection);

                if (pDR.Read())
                {
                    string pStatus = pDR["fldStatus"].ToString();
                    if (pStatus == "Open")
                        pBln = true;
                    else
                        pBln = false;
                }

                pDR.Close();
                pConnection.Close();

                return pBln;
            }


            public Boolean AreManuallyModified_SWModels ()
            //============================================      //BG 06DEC12
            {
                if (mFileModified_CompleteAssy || mFileModified_RadialPart || 
                    mFileModified_RadialBlankAssy || mFileModified_EndTB_Part || 
                    mFileModified_EndTB_Assy || mFileModified_EndSeal_Part)
                {
                    return true;
                }
                else
                    return false;
            }


            #region "Read Customer Enq. Sheet (PDF):"  
            //--------------------------------------

                public void Read_CustEnqySheet(clsOpCond OpCond_In)
                //====================================================
                {
                    PdfReader pPdfDocu = new PdfReader(mFilePath_CustEnqSheet);
                    AcroFields pPDFField = pPdfDocu.AcroFields;
                    clsUnit pUnit = new clsUnit();

                    //  Project Related Data
                    //  ---------------------
                    mCustomer.Name = pPDFField.GetField("Name");
                    mCustomer.MachineDesc = pPDFField.GetField("Machine Description");

                    //  Operating Condition Related Data
                    //  --------------------------------

                    //....Load Angle.
                    Double pLoadAng = 0.0F;
                    pLoadAng = modMain.ConvTextToDouble(pPDFField.GetField("Load_Angle"));

                    OpCond_In.Radial_LoadAng = pLoadAng;

                    //....Speed.

                    //...Min,Max Val in Eng.
                    //Double[] pSpeed = new Double[2];
                    //pSpeed[0] = modMain.ConvTextToDouble(pPDFField.GetField("Op_Speed_Min"));
                    //pSpeed[1] = modMain.ConvTextToDouble(pPDFField.GetField("Op_Speed_Max"));

                    //BG 22NOV11
                    int[] pSpeed = new int[2];
                    pSpeed[0] = modMain.ConvTextToInt(pPDFField.GetField("Op_Speed_Min"));
                    pSpeed[1] = modMain.ConvTextToInt(pPDFField.GetField("Op_Speed_Max"));

                    //...Nominal Val in Eng.
                    //Double pSpeed_Nom = 0.0F;
                    //pSpeed_Nom = modMain.ConvTextToDouble(pPDFField.GetField("Op_Speed_Nom"));

                    //BG 22NOV11
                    int pSpeed_Nom = 0;
                    pSpeed_Nom = modMain.ConvTextToInt(pPDFField.GetField("Op_Speed_Nom"));

                    //....Assign Values.
                    if (pSpeed[0] != 0.0F && pSpeed[1] != 0.0F)
                    {
                        OpCond_In.Speed_Range[0] = pSpeed[0];
                        OpCond_In.Speed_Range[1] = pSpeed[1];
                    }
                    else
                    {
                        OpCond_In.Speed_Range[0] = pSpeed_Nom;
                        OpCond_In.Speed_Range[1] = pSpeed_Nom;
                    }


                    //....Load

                    //...Min,Max Val in Eng.
                    Double[] pLoad = new Double[2];
                    pLoad[0] = modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Min"));
                    pLoad[1] = modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Max"));

                    //...Nominal Val in Eng.
                    Double pLoad_Nom = 0.0F;
                    pLoad_Nom = modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Nom"));

                    //...Min,Max Val in Met.
                    Double[] pLoad_Met = new Double[2];
                    pLoad_Met[0] = (Double)pUnit.CFac_Load_MetToEng(modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Met_Min")));
                    pLoad_Met[1] = (Double)pUnit.CFac_Load_MetToEng(modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Met_Max")));

                    //...Nominal Val in Met.
                    Double pLoad_Met_Nom = 0.0F;
                    pLoad_Met_Nom = (Double)pUnit.CFac_Load_MetToEng(modMain.ConvTextToDouble(pPDFField.GetField("Op_Load_Met_Nom")));

                    //....Assign Values.
                    if (pLoad[0] != 0.0F && pLoad[1] != 0.0F)
                    //---------------------------------------
                    {
                        OpCond_In.Radial_Load_Range[0] = pLoad[0];
                        OpCond_In.Radial_Load_Range[1] = pLoad[1];
                    }
                    else if (pLoad_Nom != 0.0F)
                    //-------------------------
                    {
                        OpCond_In.Radial_Load_Range[0] = pLoad_Nom;
                        OpCond_In.Radial_Load_Range[1] = pLoad_Nom;
                    }
                    else if (pLoad_Met[0] != 0.0F && pLoad_Met[1] != 0.0F)
                    //----------------------------------------------------
                    {
                        OpCond_In.Radial_Load_Range[0] = pLoad_Met[0];
                        OpCond_In.Radial_Load_Range[1] = pLoad_Met[1];
                    }

                    else if (pLoad_Met_Nom != 0.0F)
                    //-----------------------------
                    {
                        OpCond_In.Radial_Load_Range[0] = pLoad_Met_Nom;
                        OpCond_In.Radial_Load_Range[1] = pLoad_Met_Nom;
                    }


                    //....Pressure

                    //....Val in Eng.
                    Double pPress = 0.0F;
                    pPress = modMain.ConvTextToDouble(pPDFField.GetField("Pressure"));

                    //....Val in Met.
                    Double pPress_Met = 0.0F;
                    pPress_Met = (Double)pUnit.CFac_Press_MetToEng(modMain.ConvTextToDouble(pPDFField.GetField("Pressure_Met")));

                    if (pPress != 0.0F)
                        OpCond_In.OilSupply_Press = pPress;
                    else if (pPress_Met != 0.0F)
                        OpCond_In.OilSupply_Press = pPress_Met;

                    //....Tempurature

                    //....Val in Eng.
                    Double pTemp = 0.0F;
                    pTemp = modMain.ConvTextToDouble(pPDFField.GetField("Temperature"));

                    //....Val in Met.
                    Double pTemp_Met = 0.0F;
                    pTemp_Met = (Double)pUnit.CFac_Temp_MetToEng(modMain.ConvTextToDouble(pPDFField.GetField("Temperature_Met")));

                    if (pPress != 0.0F)
                        OpCond_In.OilSupply_Temp = pTemp;
                    else if (pPress_Met != 0.0F)
                        OpCond_In.OilSupply_Temp = pTemp_Met;

                    //.....Lube Type
                    OpCond_In.OilSupply_Lube_Type = pPDFField.GetField("Type");

                    //.....OilSupply Type
                    string pOilType = pPDFField.GetField("OilSupply_Type");

                    if (pOilType == "Pressurized")
                        OpCond_In.OilSupply_Type = "Pressurized";
                    else if (pOilType != "" && pOilType != null)
                    {
                        MessageBox.Show("Please change oilsupply type, this is not supported in this version");
                        OpCond_In.OilSupply_Type = "Pressurized";
                    }
                    else
                        OpCond_In.OilSupply_Type = "Pressurized";

                    //....Directionality
                    String pRot_Directionality = pPDFField.GetField("Directionality");

                    if (pRot_Directionality == "Uni")
                        OpCond_In.Rot_Directionality = (clsOpCond.eRotDirectionality)Enum.Parse(typeof(clsOpCond.eRotDirectionality), "Uni");
                    else if (pRot_Directionality == "Bi")
                        OpCond_In.Rot_Directionality = (clsOpCond.eRotDirectionality)Enum.Parse(typeof(clsOpCond.eRotDirectionality), "Bi");
                    else if (pRot_Directionality == "" && pRot_Directionality == null)
                        OpCond_In.Rot_Directionality = (clsOpCond.eRotDirectionality)Enum.Parse(typeof(clsOpCond.eRotDirectionality), "Bi");

                    //  Bearing Related Data.
                    //  ---------------------

                    //....Type

                    String pBearingType = pPDFField.GetField("BearingType").Trim();                 
                  
                    //mRadialType = (eRadialType)Enum.Parse(typeof(eRadialType), pBearingType);

                    if (pBearingType != null)
                    {
                        if (pBearingType != clsBearing_Radial.eDesign.Flexure_Pivot.ToString().Replace("_", " ").Trim())
                            System.Windows.Forms.MessageBox.Show("This type not supported in this version");

                        else if (pBearingType == clsBearing_Radial.eDesign.Flexure_Pivot.ToString().Replace("_", " "))
                        {
                            pBearingType = pBearingType.Replace(" ", "_").Trim();
                            ((clsBearing_Radial_FP)mProduct.Bearing).Design = (clsBearing_Radial_FP.eDesign)Enum.Parse(typeof(clsBearing_Radial_FP.eDesign), pBearingType);
                        }
                    }

                    //Bearing_In.TypeRadial = "Flexure Pivot";
                   
                    //....Split Config
                    String pSplitConfig = pPDFField.GetField("SplitConfig");

                    //if (pSplitConfig == "Yes")
                    //    Bearing_In.SplitConfig = true;
                    //else if (pSplitConfig == "No")
                    //    Bearing_In.SplitConfig = false;
                    //else if (pSplitConfig == "" && pSplitConfig == null)
                    //    Bearing_In.SplitConfig = true;

                    if (pSplitConfig == "Yes")
                        ((clsBearing_Radial_FP)mProduct.Bearing).SplitConfig = true;
                    else if (pSplitConfig == "No")
                        ((clsBearing_Radial_FP)mProduct.Bearing).SplitConfig = false;
                    else if (pSplitConfig == "" || pSplitConfig == null)
                        ((clsBearing_Radial_FP)mProduct.Bearing).SplitConfig = true;

                    pPdfDocu.Close();
                }

            #endregion
         

            #region"CLASS OBJECTS COPYING & COMPARISON ROUTINES:"
            //---------------------------------------------------

                public bool Compare(clsProject ObjMOD_In)
                //=======================================
                {
                    Boolean mblnVal_Changed = false;
                    int pRetValue = 0;


                    if (modMain.CompareVar(ObjMOD_In.Unit.System.ToString(), mUnit.System.ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Status, mStatus, pRetValue) > 0)   
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.No, mNo, pRetValue) > 0)   
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.No_Suffix, mNo_Suffix, pRetValue) > 0)  
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Customer.Name, mCustomer.Name, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Customer.MachineDesc, mCustomer.MachineDesc, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Customer.PartNo, mCustomer.PartNo, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Product.EndConfig[0].ToString(), mProduct.EndConfig[0].ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Product.EndConfig[1].ToString(), mProduct.EndConfig[1].ToString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.AssyDwg.No, mAssyDwg.No, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.AssyDwg.No_Suffix, mAssyDwg.No_Suffix, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.AssyDwg.Ref, mAssyDwg.Ref, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Engg.Name, mEngg.Name, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Engg.Initials, mEngg.Initials, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Engg.Date.ToShortDateString(), mEngg.Date.ToShortDateString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.DesignedBy.Name, mDesignedBy.Name, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.DesignedBy.Initials, mDesignedBy.Initials, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.DesignedBy.Date.ToShortDateString(),
                                            mDesignedBy.Date.ToShortDateString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }
                
                    if (modMain.CompareVar(ObjMOD_In.CheckedBy.Name, mCheckedBy.Name, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.CheckedBy.Initials, mCheckedBy.Initials, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.CheckedBy.Date.ToShortDateString(),
                                             mCheckedBy.Date.ToShortDateString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.Date_Closing.ToShortDateString(),
                                            mDate_Closing.ToShortDateString(), pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FilePath_Project, mFilePath_Project, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FilePath_DesignTbls_SWFiles, mFilePath_DesignTbls_SWFiles, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_RadialPart,
                                            mFileModified_RadialPart, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_RadialBlankAssy,
                                                mFileModified_RadialBlankAssy, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_EndTB_Part,
                        mFileModified_EndTB_Part, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_EndTB_Assy,
                        mFileModified_EndTB_Assy, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_EndSeal_Part,
                                            mFileModified_EndSeal_Part, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModified_CompleteAssy,
                                            mFileModified_CompleteAssy, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }

                    if (modMain.CompareVar(ObjMOD_In.FileModification_Notes,
                                            mFileModification_Notes, pRetValue) > 0)
                    {
                        mblnVal_Changed = true;
                        return mblnVal_Changed;
                    }
                      
                    return mblnVal_Changed;
                }

            #endregion


            #region "ICLONEABLE MEMBERS"

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

        #endregion

       
    }
}
