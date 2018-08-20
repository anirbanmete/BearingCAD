
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsDB                                  '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Collections.Specialized;
using System.Globalization;

namespace BearingCAD20
{
     [Serializable]
    public class clsDB
    {

        #region "MEMBER VARIABLE DECLARATIONS:"
        //================================

            private string mDBFileName;
            private string mDBServerName;
            CultureInfo mInvCulture = CultureInfo.InvariantCulture;
            string mstrDefDate = "";

        #endregion


        //....Class Constructor. 

        public clsDB()
        //============ 
        {
            mDBFileName = clsFiles.DBFileName;
            mDBServerName = clsFiles.DBServerName;
            mstrDefDate = DateTime.MinValue.ToString("d", mInvCulture);
        }


        #region "CLASS METHODS:"

            #region "ADO.NET HELPER ROUTINES:"

                public SqlConnection GetConnection()
                //====================================
                {
                    SqlConnection pGetConnection = new SqlConnection();

                    try
                    {
                        string pstrConnectDB = "";
                        pstrConnectDB = "Data source= " + mDBServerName + ";" +
                                        "Initial Catalog = " + mDBFileName + ";" +
                                        "Integrated Security= SSPI;";
                                               

                        //....Create & Open a new Connection -
                        pGetConnection = new SqlConnection(pstrConnectDB);
                        pGetConnection.Open();
                    }

                    catch (System.Data.SqlClient.SqlException pEXP)
                    {
                        //throw new System.Exception(pEXP.Message, pEXP.InnerException);
                        //....Handles connection-level Errors
                        MessageBox.Show(pEXP.Message + "--" + pEXP.StackTrace, "Connection Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    catch (InvalidOperationException pEXP)
                    {
                        MessageBox.Show(pEXP.Message + "--" + pEXP.StackTrace, "Connection Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return pGetConnection;
                }


                public SqlDataReader GetDataReader(string strSELECTQry_In, ref SqlConnection Conn_In)   
                //=================================================================================== 

                //This routine returns DataReader
                //       Input   Parameters      :   SQL Statement 
                //       Output  Parameters      :   DataReader
                {
                    try
                    {
                        string pstrConnectDB = "";
                        pstrConnectDB = "Data source= " + mDBServerName + ";" +
                                        "Initial Catalog = " + mDBFileName + ";" +
                                        "Integrated Security= SSPI;";
                       
                        //....Create & Open a new Connection -
                        Conn_In = new SqlConnection(pstrConnectDB);
                        Conn_In.Open();
                    }

                    catch (System.Data.SqlClient.SqlException pEXP)
                    {
                        //throw new System.Exception(pEXP.Message, pEXP.InnerException);
                        //....Handles connection-level Errors
                        MessageBox.Show(pEXP.Message + "--" + pEXP.StackTrace, "Connection Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    catch (InvalidOperationException pEXP)
                    {
                        MessageBox.Show(pEXP.Message + "--" + pEXP.StackTrace, "Connection Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    SqlDataReader pGetDataReader = null;
                    SqlCommand pCmd = new SqlCommand(strSELECTQry_In, Conn_In);
                    SqlDataReader pDR = null;

                    try
                    { pDR = pCmd.ExecuteReader(); }

                    catch (Exception pEXP)
                    {
                        MessageBox.Show(pEXP.Message + "--" + pEXP.StackTrace, "Data Read Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    finally
                    { pGetDataReader = pDR; }

                    return pGetDataReader;
                }


                public int ExecuteCommand(string strACTIONQry_In)
                //================================================

                 //This routine returns DataReader
                 //       Input   Parameters      :   SQL Statement 
                 //       Output  Parameters      :   DataReader
                {
                    int pExecuteCommand = 0;

                    SqlConnection pConn = new SqlConnection();
                    pConn = GetConnection();

                    SqlCommand pCmd = new SqlCommand(strACTIONQry_In, pConn);
                    int pCountRecords = 0;

                    try
                    {
                        pCountRecords = pCmd.ExecuteNonQuery();
                    }

                    catch (System.Data.SqlClient.SqlException pEXP)
                    {
                        MessageBox.Show("Error in Data Entry.", "Command Execution Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    finally
                    {
                        pExecuteCommand = pCountRecords;
                    }

                    pConn.Close();

                    return pExecuteCommand;
                }


                private SqlDataAdapter GetDataAdapter(string strQry_In)
                //=====================================================

                //This routine returns DataAdapter
                //       Input   Parameters      :   SQL Statement 
                //       Output  Parameters      :   DataAdapter

                //'....Not yet used anywhere. 
                {
                    SqlConnection pConn = new SqlConnection();
                    pConn = GetConnection();

                    SqlDataAdapter pDA = new SqlDataAdapter(strQry_In, pConn);

                    //....Set SelectCommand Properties:
                    pDA.SelectCommand = new SqlCommand();
                    pDA.SelectCommand.Connection = pConn;
                    pDA.SelectCommand.CommandText = strQry_In;
                    pDA.SelectCommand.CommandType = CommandType.Text;

                    //....Now execute the command.
                    pDA.SelectCommand.ExecuteNonQuery();

                    return pDA;
                }


                public DataSet GetDataSet(string strQry_In, string strTableName_In)                   
                //=================================================================  

                  //This routine returns DataSet
                //      Input   Parameters      :   SQL Statement , Database Table Name
                //      Output  Parameters      :   DataSet
                {

                    //Data Adapter Object:
                    //--------------------
                    SqlDataAdapter pDA = new SqlDataAdapter();
                    pDA.SelectCommand = new SqlCommand(strQry_In, GetConnection());


                    //....Set SelectCommand Properties.
                    //pDA.SelectCommand.Connection = GetConnection();       //AM 16MAR12
                    //pDA.SelectCommand.CommandText = strQry_In;
                    pDA.SelectCommand.CommandType = CommandType.Text;

                    //....Now execute the command.
                    try
                    {
                        pDA.SelectCommand.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    //DataSet object: Fill with Data.
                    //-------------------------------
                    DataSet pDS = new DataSet();
                    pDS.Clear();
                    pDA.Fill(pDS, strTableName_In);

                    return pDS;
                }


                public DataView GetDataView(string strQry_In, string strTableName_In)                              
                //===================================================================   

                //This routine returns DataView
                //       Input   Parameters      :   SQL Statement , Database Table Name
                //       Output  Parameters      :   DataView
                {
                    SqlConnection pConn = new SqlConnection();
                    pConn = GetConnection();

                    //Data Adapter Object:
                    //--------------------
                    SqlDataAdapter pDA = new SqlDataAdapter();
                    pDA.SelectCommand = new SqlCommand(strQry_In, pConn);

                    //....Set SelectCommand Properties:
                    pDA.SelectCommand.Connection = pConn;
                    pDA.SelectCommand.CommandText = strQry_In;
                    pDA.SelectCommand.CommandType = CommandType.Text;
                    //pDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                    //....Execute the command.
                    pDA.SelectCommand.ExecuteNonQuery();

                    //DataSet object: Fill with Data.
                    //-------------------------------
                    DataSet pDS = new DataSet();
                    pDS.Clear();
                    pDA.Fill(pDS, strTableName_In);

                    //....DataView Object.
                    return new DataView(pDS.Tables[strTableName_In]);
                }

            #endregion


            #region "DATABASE RELATED ROUTINES:"
            //---------------------------------

                #region "Database Retrieval Routine:"

                    public void RetrieveRecord(clsProject Project_In, clsOpCond OpCond_In)
                    //=====================================================================
                    {
                        RetrieveRec_Project(Project_In);
                        RetrieveRec_OpCond(Project_In, OpCond_In);
                        RetrieveRec_Product(Project_In);
                        RetrieveRec_Bearing_Radial(Project_In);
                        RetrieveRec_Bearing_Radial_FP_Detail(Project_In);
                        RetrieveRec_EndConfigs(Project_In);
                        RetrieveRec_Accessories(Project_In);
                    }
       

                    #region "Retrieval: Project"

                        private void RetrieveRec_Project(clsProject Project_In)
                        //====================================================     
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE = "";
                            string pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_Details ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            if (pDR.Read())
                            {
                                Project_In.Status = CheckDBString(pDR, "fldStatus");

                                //....Customer
                                Project_In.Customer_Name = CheckDBString(pDR, "fldCustomer_Name");
                                Project_In.Customer_MachineDesc = CheckDBString(pDR, "fldCustomer_MachineDesc");
                                Project_In.Customer_PartNo = CheckDBString(pDR, "fldCustomer_PartNo");

                                //....Unit System
                                Project_In.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem),
                                                                        CheckDBString(pDR, "fldUnitSystem"));

                                //....AssyDwg
                                Project_In.AssyDwgNo = CheckDBString(pDR, "fldAssyDwg_No");
                                Project_In.AssyDwgNo_Suffix = CheckDBString(pDR, "fldAssyDwg_No_Suffix");
                                Project_In.AssyDwgRef = CheckDBString(pDR, "fldAssyDwg_Ref");

                                //....Engg.
                                Project_In.Engg_Name = CheckDBString(pDR, "fldEngg_Name");
                                Project_In.Engg_Initials = CheckDBString(pDR, "fldEngg_Initials");
                                Project_In.Engg_Date = CheckDBDateTime(pDR, "fldEngg_Date");

                                //....Designer.
                                Project_In.DesignedBy_Name = CheckDBString(pDR, "fldDesignedBy_Name");
                                Project_In.DesignedBy_Initials = CheckDBString(pDR, "fldDesignedBy_Initials");
                                Project_In.DesignedBy_Date = CheckDBDateTime(pDR, "fldDesignedBy_Date");

                                //....Checked By.
                                Project_In.CheckedBy_Name = CheckDBString(pDR, "fldCheckedby_Name");
                                Project_In.CheckedBy_Initials = CheckDBString(pDR, "fldCheckedby_Initials");
                                Project_In.CheckedBy_Date = CheckDBDateTime(pDR, "fldCheckedby_Date");

                                //....Closing Date.
                                Project_In.Date_Closing = CheckDBDateTime(pDR, "fldDate_Closing");

                                //....File Paths    //To be saved in CreateFiles.
                                Project_In.FilePath_Project = CheckDBString(pDR, "fldFilePath_Project");
                                Project_In.FilePath_DesignTbls_SWFiles = CheckDBString(pDR, "fldFilePath_DesignTbls_SWFiles");

                                //....SolidWorks Model Files Paths
                                Project_In.FileModified_CompleteAssy = CheckDBBoolean(pDR, "fldFileModified_CompleteAssy");     //BG 05DEC12

                                Project_In.FileModified_RadialPart = CheckDBBoolean(pDR, "fldFileModified_Radial_Part");
                                Project_In.FileModified_RadialBlankAssy = CheckDBBoolean(pDR, "fldFileModified_Radial_BlankAssy");

                                Project_In.FileModified_EndTB_Part = CheckDBBoolean(pDR, "fldFileModified_EndTB_Part");
                                Project_In.FileModified_EndTB_Assy = CheckDBBoolean(pDR, "fldFileModified_EndTB_Assy");

                                Project_In.FileModified_EndSeal_Part = CheckDBBoolean(pDR, "fldFileModified_EndSeal_Part");
                                
                                Project_In.FileModification_Notes = CheckDBString(pDR, "fldFileModification_Notes");
                            }

                            pDR.Close();
                            pConnection.Close();
                            pDR = null;
                        }

                    #endregion


                    #region "Retrieval: Operating Condition"

                        private void RetrieveRec_OpCond(clsProject Project_In, clsOpCond OpCond_In)
                        //========================================================================= 
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE;
                            string pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_OpCond ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();
                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            if (pDR.Read())
                            {
                                OpCond_In.Speed_Range[0] = CheckDBInt(pDR, "fldSpeed_Range_Min");
                                OpCond_In.Speed_Range[1] = CheckDBInt(pDR, "fldSpeed_Range_Max");

                                string pRot = pDR["fldRot_Directionality"].ToString();
                                if (pRot != "")
                                    OpCond_In.Rot_Directionality =
                                        (clsOpCond.eRotDirectionality)Enum.Parse(typeof(clsOpCond.eRotDirectionality), pRot);

                                OpCond_In.Radial_Load_Range[0] = CheckDBDouble(pDR, "fldRadial_Load_Range_Min");
                                OpCond_In.Radial_Load_Range[1] = CheckDBDouble(pDR, "fldRadial_Load_Range_Max");

                                OpCond_In.Radial_LoadAng = CheckDBDouble(pDR, "fldRadial_LoadAng");

                                OpCond_In.Thrust_Load_Range_Front[0] = CheckDBDouble(pDR, "fldThrust_Load_Range_Front_Min");
                                OpCond_In.Thrust_Load_Range_Front[1] = CheckDBDouble(pDR, "fldThrust_Load_Range_Front_Max");

                                OpCond_In.Thrust_Load_Range_Back[0] = CheckDBDouble(pDR, "fldThrust_Load_Range_Back_Min");
                                OpCond_In.Thrust_Load_Range_Back[1] = CheckDBDouble(pDR, "fldThrust_Load_Range_Back_Max");

                                OpCond_In.OilSupply_Press = CheckDBDouble(pDR, "fldOilSupply_Press");
                                OpCond_In.OilSupply_Temp = CheckDBDouble(pDR, "fldOilSupply_Temp");
                                OpCond_In.OilSupply_Type = CheckDBString(pDR, "fldOilSupply_Type");
                                OpCond_In.OilSupply_Lube_Type = CheckDBString(pDR, "fldOilSupply_Lube_Type");
                            }

                            pDR.Close();
                            pDR = null;
                            pConnection.Close();
                        }
        
                    #endregion


                    #region "Retrieval: Product"

                        private void RetrieveRec_Product(clsProject Project_In)
                        //=====================================================     
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE = "";
                            string pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_Product ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            if (pDR.Read())
                            {
                                //....Bearing Type
                                Project_In.Product.Bearing.Type = (clsBearing.eType)Enum.Parse(typeof(clsBearing.eType),
                                                                        CheckDBString(pDR, "fldBearing_Type"));

                                //....EndConfigs                                                                 //SG 23JAN13 following assignment not needed.
                                //if (CheckDBString(pDR, "fldEndConfig_Front_Type") != "")
                                //    Project_In.Product.EndConfig[0].Type = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType),
                                //                                            CheckDBString(pDR, "fldEndConfig_Front_Type"));

                                //if (CheckDBString(pDR, "fldEndConfig_Back_Type") != "")
                                //    Project_In.Product.EndConfig[1].Type = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType),
                                //                                            CheckDBString(pDR, "fldEndConfig_Back_Type"));

                                Project_In.Product.L_Available = CheckDBDouble(pDR, "fldL_Available");
                                Project_In.Product.Dist_ThrustFace[0] = CheckDBDouble(pDR, "fldDist_ThrustFace_Front");
                                Project_In.Product.Dist_ThrustFace[1] = CheckDBDouble(pDR, "fldDist_ThrustFace_Back");
                            }

                            pDR.Close();
                            pConnection.Close();
                            pDR = null;
                        }

                    #endregion


                    #region "Retrieval: Bearing Radial"

                        private void RetrieveRec_Bearing_Radial(clsProject Project_In)
                        //===========================================================   
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE = "";
                            string pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_Bearing_Radial ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else 
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            if (pDR.Read())
                            {
                                //....Radial Bearing Type
                                ((clsBearing_Radial)Project_In.Product.Bearing).Design = (clsBearing_Radial.eDesign)Enum.Parse(typeof(clsBearing_Radial.eDesign),
                                                                        CheckDBString(pDR, "fldDesign")); 
                            }

                            pDR.Close();
                            pConnection.Close();
                            pDR = null;
                        }

                    #endregion


                    #region "Retrieval: Bearing Radial FP Detail"

                        private void RetrieveRec_Bearing_Radial_FP_Detail(clsProject Project_In)
                        //=======================================================================
                        {
                            RetrieveRec_Bearing_Radial_FP(Project_In);
                            RetrieveRec_Bearing_Radial_FP_PerformData(Project_In);
                            RetrieveRec_Bearing_Radial_FP_Pad(Project_In);
                            RetrieveRec_Bearing_Radial_FP_FlexurePivot(Project_In);
                            RetrieveRec_Bearing_Radial_FP_OilInlet(Project_In);
                            RetrieveRec_Bearing_Radial_FP_MillRelief(Project_In);
                            RetrieveRec_Bearing_Radial_FP_Flange(Project_In);
                            RetrieveRec_Bearing_Radial_FP_SL(Project_In);
                            RetrieveRec_Bearing_Radial_FP_AntiRotPin(Project_In);
                            RetrieveRec_Bearing_Radial_FP_Mount(Project_In);
                            RetrieveRec_Bearing_Radial_FP_TempSensor(Project_In);
                            RetrieveRec_Bearing_Radial_FP_EDM_Pad(Project_In);                            
                        }


                        #region "Retrieval: Bearing Radial FP"
        
                            private void RetrieveRec_Bearing_Radial_FP(clsProject Project_In)
                            //===============================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;
                           
                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);
                            
                                if (pDR.Read())
                                {
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig = CheckDBBoolean(pDR, "fldSplitConfig");

                                    //Bearing Geometry. 
                                    //---------------- 

                                    //....DShaft. 
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] = CheckDBDouble(pDR, "fldDShaft_Range_Min");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] = CheckDBDouble(pDR, "fldDShaft_Range_Max");

                                    //....DFit.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0] = CheckDBDouble(pDR, "fldDFit_Range_Min");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1] = CheckDBDouble(pDR, "fldDFit_Range_Max");

                                    //....DSet.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] = CheckDBDouble(pDR, "fldDSet_Range_Min");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1] = CheckDBDouble(pDR, "fldDSet_Range_Max");

                                    //....DPad.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0] = CheckDBDouble(pDR, "fldDPad_Range_Min");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1] = CheckDBDouble(pDR, "fldDPad_Range_Max");

                                    //....Bearing Length, Length Total.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).L = CheckDBDouble(pDR, "fldL");

                                    RetrieveRec_Bearing_Radial_FP_Pad(Project_In); //....For Pad L.

                                    ////....EDM Relief
                                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[0] = CheckDBDouble(pDR, "fldEDMRelief_Front");
                                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[1] = CheckDBDouble(pDR, "fldEDMRelief_Back");

                                    //....EndConfig Depth 
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0] = CheckDBDouble(pDR, "fldDepth_EndConfig_Front");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1] = CheckDBDouble(pDR, "fldDepth_EndConfig_Back");

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace = CheckDBDouble(pDR, "fldDimStart_FrontFace");
                                    
                                    // Material
                                    // --------                            
                                    //....Base                            
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base = CheckDBString(pDR,"fldMat_Base");

                                    //....Lining    
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists = CheckDBBoolean(pDR, "fldMat_LiningExists");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining = pDR["fldMat_Lining"].ToString();

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT = CheckDBDouble(pDR, "fldLiningT");     
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - PerformData"

                            private void RetrieveRec_Bearing_Radial_FP_PerformData(clsProject Project_In)
                            //==========================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;
                             
                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_PerformData  ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);
                        
                                if (pDR.Read())
                                {
                                    // Perfor Data
                                    // ------------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP = CheckDBDouble(pDR, "fldPower_HP");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm = CheckDBDouble(pDR, "fldFlowReqd_gpm");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F = CheckDBDouble(pDR, "fldTempRise_F");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min = CheckDBDouble(pDR, "fldTFilm_Min");

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Temp = CheckDBDouble(pDR, "fldPadMax_Temp");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Press = CheckDBDouble(pDR, "fldPadMax_Press");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Rot = CheckDBDouble(pDR, "fldPadMax_Rot");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Load = CheckDBDouble(pDR, "fldPadMax_Load");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax_Stress = CheckDBDouble(pDR, "fldPadMax_Stress");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Pad"

                            private void RetrieveRec_Bearing_Radial_FP_Pad(clsProject Project_In)
                            //==================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;
                          
                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_Pad ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Pad
                                    // ---                             
                                    //....Type.
                                    if (CheckDBString(pDR, "fldType") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type =
                                            (clsBearing_Radial_FP.clsPad.eLoadPos)Enum.Parse(typeof(clsBearing_Radial_FP.clsPad.eLoadPos), CheckDBString(pDR, "fldType"));

                                    //....Count.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count = CheckDBInt(pDR, "fldCount");

                                    //....Length.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L = CheckDBDouble(pDR, "fldL");

                                    //....Pivot.
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot_Offset = CheckDBDouble(pDR, "fldPivot_Offset");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot_AngStart = CheckDBDouble(pDR, "fldPivot_AngStart");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Lead = CheckDBDouble(pDR, "fldT_Lead");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Pivot = CheckDBDouble(pDR, "fldT_Pivot");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T_Trail = CheckDBDouble(pDR, "fldT_Trail");                                    
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Flexure Pivot"

                            private void RetrieveRec_Bearing_Radial_FP_FlexurePivot(clsProject Project_In)
                            //============================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_FlexurePivot ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // FlexPivot
                                    // ---------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_T = CheckDBDouble(pDR, "fldWeb_T");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_H = CheckDBDouble(pDR, "fldWeb_H");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web_RFillet = CheckDBDouble(pDR, "fldWeb_RFillet");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM = CheckDBDouble(pDR, "fldGapEDM");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff = CheckDBDouble(pDR, "fldRot_Stiff");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Oil Inlet"

                            private void RetrieveRec_Bearing_Radial_FP_OilInlet(clsProject Project_In)
                            //========================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_OilInlet ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // OilInlet
                                    // --------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Count = CheckDBInt(pDR, "fldOrifice_Count");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_D = CheckDBDouble(pDR, "fldOrifice_D");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply = CheckDBInt(pDR, "fldCount_MainOilSupply");

                                    if (CheckDBString(pDR, "fldOrifice_StartPos") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_StartPos = (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos),
                                                                    CheckDBString(pDR, "fldOrifice_StartPos"));

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Loc_FrontFace = CheckDBDouble(pDR, "fldOrifice_Loc_FrontFace");
                                 
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_Exists = CheckDBBoolean(pDR, "fldAnnulus_Exists");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_D = CheckDBDouble(pDR, "fldAnnulus_D");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_Loc_Back = CheckDBDouble(pDR, "fldAnnulus_Loc_Back");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_L = CheckDBDouble(pDR, "fldAnnulus_L");

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D == 0.0F && ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L == 0.0F)
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Calc_Annulus_Params();

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Dist_Holes = CheckDBDouble(pDR, "fldOrifice_Dist_Holes");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Mill Relief"

                            private void RetrieveRec_Bearing_Radial_FP_MillRelief(clsProject Project_In)
                            //==========================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_MillRelief ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Mill Relief
                                    // -----------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists = CheckDBBoolean(pDR, "fldExists");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig = CheckDBString(pDR, "fldD_Desig");
                                    //....EDM Relief
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0] = CheckDBDouble(pDR, "fldEDMRelief_Front");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1] = CheckDBDouble(pDR, "fldEDMRelief_Back");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Flange"

                            private void RetrieveRec_Bearing_Radial_FP_Flange(clsProject Project_In)
                            //======================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_Flange ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Flange
                                    // ------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists = CheckDBBoolean(pDR, "fldExists");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D = CheckDBDouble(pDR, "fldD");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid =CheckDBDouble(pDR, "fldWid");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front = CheckDBDouble(pDR, "fldDimStart_Front");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - SL"

                            private void RetrieveRec_Bearing_Radial_FP_SL(clsProject Project_In)
                            //=================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_SL ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Split Line Hardware
                                    // -------------------

                                    // Thread:
                                    // -------                                 
                                    if (CheckDBString(pDR, "fldScrew_Spec_UnitSystem") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System = (clsUnit.eSystem)
                                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pDR, "fldScrew_Spec_UnitSystem"));                //BG 26MAR12
                                                                       
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type = CheckDBString(pDR, "fldScrew_Spec_Type");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig = CheckDBString(pDR, "fldScrew_Spec_D_Desig");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch = CheckDBDouble(pDR, "fldScrew_Spec_Pitch");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L = CheckDBDouble(pDR, "fldScrew_Spec_L");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat = CheckDBString(pDR, "fldScrew_Spec_Mat");

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc_Center = CheckDBDouble(pDR, "fldLScrew_Spec_Loc_Center");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc_Front = CheckDBDouble(pDR, "fldLScrew_Spec_Loc_Front");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc_Center = CheckDBDouble(pDR, "fldRScrew_Spec_Loc_Center");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc_Front = CheckDBDouble(pDR, "fldRScrew_Spec_Loc_Front");
                                    
                                    // Pin:
                                    // ----                                  
                                    if (CheckDBString(pDR, "fldDowel_Spec_UnitSystem") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System = (clsUnit.eSystem)        //BG 26MAR12
                                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pDR, "fldDowel_Spec_UnitSystem"));

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type = CheckDBString(pDR, "fldDowel_Spec_Type");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig = CheckDBString(pDR, "fldDowel_Spec_D_Desig");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L = CheckDBDouble(pDR, "fldDowel_Spec_L");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat = CheckDBString(pDR, "fldDowel_Spec_Mat");

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc_Center = CheckDBDouble(pDR, "fldLDowel_Spec_Loc_Center");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc_Front = CheckDBDouble(pDR, "fldLDowel_Spec_Loc_Front");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc_Center = CheckDBDouble(pDR, "fldRDowel_Spec_Loc_Center");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc_Front = CheckDBDouble(pDR, "fldRDowel_Spec_Loc_Front");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Anti Rotation Pin"

                            private void RetrieveRec_Bearing_Radial_FP_AntiRotPin(clsProject Project_In)
                            //===========================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_AntiRotPin ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Anti Rotation Pin
                                    // ------------------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front = CheckDBDouble(pDR, "fldLoc_Dist_Front");

                                    if (CheckDBString(pDR, "fldLoc_Casing_SL") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL)
                                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL),
                                            CheckDBString(pDR, "fldLoc_Casing_SL"));

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset = CheckDBDouble(pDR, "fldLoc_Offset");

                                    if (CheckDBString(pDR, "fldLoc_Bearing_Vert") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert)
                                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert),
                                            CheckDBString(pDR, "fldLoc_Bearing_Vert"));

                                    if (CheckDBString(pDR, "fldLoc_Bearing_SL") != "")                  //BG 08MAY12
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL)
                                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL),
                                            CheckDBString(pDR, "fldLoc_Bearing_SL"));

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle = CheckDBDouble(pDR, "fldLoc_Angle");
                     
                                    // Pin:
                                    // ----
                                    if (CheckDBString(pDR, "fldSpec_UnitSystem") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System = (clsUnit.eSystem)
                                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pDR, "fldSpec_UnitSystem"));                      //BG 26MAR12

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type = CheckDBString(pDR, "fldSpec_Type");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig = CheckDBString(pDR, "fldSpec_D_Desig");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L = CheckDBDouble(pDR, "fldSpec_L");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat = CheckDBString(pDR, "fldSpec_Mat");

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth = CheckDBDouble(pDR, "fldDepth");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout = CheckDBDouble(pDR, "fldStickOut");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - Mount"

                            private void RetrieveRec_Bearing_Radial_FP_Mount(clsProject Project_In)
                            //====================================================================            
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_Mount ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Mount Hole - Front
                                    // ------------------  
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru = CheckDBBoolean(pDR, "fldHoles_GoThru");
                                    
                                    if(CheckDBString(pDR, "fldHoles_Bolting")!="")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting = (clsBearing_Radial_FP.eFaceID)
                                                                                                Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID),
                                                                                                        CheckDBString(pDR, "fldHoles_Bolting"));

                                    if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                       
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0] = CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Front");
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1] = CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Back");

                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] = CheckDBDouble(pDR, "fldHoles_Thread_Depth_Front");
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1] = CheckDBDouble(pDR, "fldHoles_Thread_Depth_Back");

                                        RetrieveRec_Bearing_Radial_FP_Mount_Fixture(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
                                    }
                                    else if(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0] = CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Front");
                                            RetrieveRec_Bearing_Radial_FP_Mount_Fixture(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1] = CheckDBBool(pDR, "fldFixture_Candidates_Chosen_Back");
                                            RetrieveRec_Bearing_Radial_FP_Mount_Fixture(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString(), Project_In);
                                        }
                                    }
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                       
                            #region ""Retrieval: Bearing Radial FP - Mount Fixture"

                                private StringCollection Retrieve_Bearing_Radial_FP_Mount_Fixture_Bolting_Pos(clsProject Project_In)
                                //==================================================================================================    BG 28FEB13
                                {
                                    StringCollection pPos = new StringCollection();

                                    string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                    pstrFIELDS = "* ";
                                    pstrFROM = "FROM tblProject_Bearing_Radial_FP_Mount_Fixture ";

                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                    else
                                        pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                    pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                    SqlConnection pConnection = new SqlConnection();

                                    SqlDataReader pDR = null;
                                    pDR = GetDataReader(pstrSQL, ref pConnection);

                                    while (pDR.Read())
                                    {
                                        pPos.Add(CheckDBString(pDR, "fldPosition"));
                                    }

                                    return pPos;
                                }


                                private void RetrieveRec_Bearing_Radial_FP_Mount_Fixture(string Bolting_In, clsProject Project_In)
                                //=================================================================================================
                                {
                                    string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                                    pstrFIELDS = "* ";
                                    pstrFROM = "FROM tblProject_Bearing_Radial_FP_Mount_Fixture ";

                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                    else
                                        pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                    pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                    SqlConnection pConnection = new SqlConnection();

                                    SqlDataReader pDR = null;
                                    pDR = GetDataReader(pstrSQL, ref pConnection);

                                    string pPosition = "";

                                    while (pDR.Read())
                                    {
                                        pPosition = CheckDBString(pDR, "fldPosition");

                                        //....Bolting = Front and Both. Position = Front
                                        if (pPosition == "Front") 
                                        {
                                            SqlConnection pConnection_MountFixture_Sel = new SqlConnection();  
                                            SqlDataReader pDR_MountFixture_Sel = null;

                                            if (((clsBearing_Radial_FP) Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0])
                                            {
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo = CheckDBString(pDR, "fldPartNo");
                                          
                                                string pSQL = "SELECT * FROM tblManf_Fixture_SplitAndTurn WHERE fldPartNo = '" +
                                                              ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo + "'";

                                                pDR_MountFixture_Sel = GetDataReader(pSQL, ref pConnection_MountFixture_Sel);

                                                if (pDR_MountFixture_Sel.Read())
                                                {
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC = CheckDBSingle(pDR_MountFixture_Sel, "fldDBC");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].D_Finish = CheckDBSingle(pDR_MountFixture_Sel, "fldDFinish");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount = CheckDBInt(pDR_MountFixture_Sel, "fldCountHoles");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Start");
                                                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Start_Comp");    //BG 24AUG12

                                                    for (int i = 0; i < ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount - 1; i++)
                                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[i] = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Other" + (i + 1).ToString());

                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced = CheckDBBoolean(pDR_MountFixture_Sel, "fldEqui_Spaced");

                                                    //....Complimentary Angle start true or false.
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp_Chosen = CheckDBBoolean(pDR, "fldHolesAngStart_Comp_Chosen");   //SB 23JUN09

                                                    //.....Selected Thread.
                                                   ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig = CheckDBString(pDR_MountFixture_Sel, "fldDia_Desig");
                                                }
                                                pConnection_MountFixture_Sel.Close();
                                            }

                                            else
                                            {
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC = CheckDBSingle(pDR, "fldDBC");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].D_Finish = CheckDBSingle(pDR, "fldD_Finish");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount = CheckDBInt(pDR, "fldHolesCount");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced = CheckDBBool(pDR, "fldHolesEquispaced");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart = CheckDBSingle(pDR, "fldHolesAngStart");

                                                for (int i = 0; i < ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount - 1; i++)
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[i] = CheckDBSingle(pDR, "fldHolesAngOther" + (i + 1).ToString());

                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig = CheckDBString(pDR, "fldScrew_Spec_D_Desig");

                                            }

                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type = CheckDBString(pDR, "fldScrew_Spec_Type");
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.L = CheckDBSingle(pDR, "fldScrew_Spec_L");
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat = CheckDBString(pDR, "fldScrew_Spec_Mat");
                                        }


                                        //....Bolting = Back and Both. Position = Back
                                        else if ( pPosition == "Back")
                                        {
                                            SqlConnection pConnection_MountFixture_Sel = new SqlConnection();
                                            SqlDataReader pDR_MountFixture_Sel = null;

                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1])
                                            {
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo = CheckDBString(pDR, "fldPartNo");                                               

                                                string pSQL = "SELECT * FROM tblManf_Fixture_SplitAndTurn WHERE fldPartNo = '" +
                                                              ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo + "'";

                                                pDR_MountFixture_Sel = GetDataReader(pSQL, ref pConnection_MountFixture_Sel);

                                                if (pDR_MountFixture_Sel.Read())
                                                {
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC = CheckDBSingle(pDR_MountFixture_Sel, "fldDBC");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].D_Finish = CheckDBSingle(pDR_MountFixture_Sel, "fldDFinish");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount = CheckDBInt(pDR_MountFixture_Sel, "fldCountHoles");
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Start");
                                                    //((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart_Comp = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Start_Comp");    //BG 24AUG12

                                                    for (int i = 0; i < ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount - 1; i++)
                                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[i] = CheckDBSingle(pDR_MountFixture_Sel, "fldAng_Other" + (i + 1).ToString());

                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced = CheckDBBoolean(pDR_MountFixture_Sel, "fldEqui_Spaced");

                                                    //....Complimentary Angle start true or false.
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart_Comp_Chosen = CheckDBBoolean(pDR, "fldHolesAngStart_Comp_Chosen");   //SB 23JUN09

                                                    //.....Selected Thread.
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig = CheckDBString(pDR_MountFixture_Sel, "fldDia_Desig");
                                                }
                                                pConnection_MountFixture_Sel.Close();
                                            }

                                            else
                                            {
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC = CheckDBSingle(pDR, "fldDBC");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].D_Finish = CheckDBSingle(pDR, "fldD_Finish");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount = CheckDBInt(pDR, "fldHolesCount");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced = CheckDBBool(pDR, "fldHolesEquispaced");
                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart = CheckDBSingle(pDR, "fldHolesAngStart");

                                                for (int i = 0; i < ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount - 1; i++)
                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[i] = CheckDBSingle(pDR, "fldHolesAngOther" + (i + 1).ToString());

                                                ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig = CheckDBString(pDR, "fldScrew_Spec_D_Desig");

                                            }

                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Type = CheckDBString(pDR, "fldScrew_Spec_Type");
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.L = CheckDBSingle(pDR, "fldScrew_Spec_L");
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Mat = CheckDBString(pDR, "fldScrew_Spec_Mat");
                                        }
                                    }

                                    pDR.Close();
                                }

                            #endregion

                    #endregion


                        #region "Retrieval: Bearing Radial FP - TempSensor"

                            private void RetrieveRec_Bearing_Radial_FP_TempSensor(clsProject Project_In)
                            //==========================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;
                             
                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_TempSensor ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // Temp Sensor
                                    // -----------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists = CheckDBBoolean(pDR, "fldExists");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength = CheckDBDouble(pDR, "fldCanLength");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count = CheckDBInt(pDR, "fldCount");

                                    if (CheckDBString(pDR, "fldLoc") != "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc = 
                                        (clsBearing_Radial_FP.eFaceID)Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID), CheckDBString(pDR, "fldLoc"));

                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D = CheckDBDouble(pDR, "fldD");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth = CheckDBDouble(pDR, "fldDepth");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart = CheckDBDouble(pDR, "fldAngStart");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: Bearing Radial FP - EDM PAD"

                            private void RetrieveRec_Bearing_Radial_FP_EDM_Pad(clsProject Project_In)
                            //=======================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;
                              
                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_Bearing_Radial_FP_EDM_Pad ";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                if (pDR.Read())
                                {
                                    // EDM Pad
                                    // -------
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back = CheckDBDouble(pDR, "fldRFillet_Back");
                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web = CheckDBDouble(pDR, "fldAngStart_Web");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion
        
                    #endregion


                    #region "Retrieval: End Configs Detail"

                        public void Retrieve_EndConfigs_Type(clsProject Project_In, clsEndConfig.eType[] Type_Out)
                        //=========================================================================================
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_EndConfigs ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);
                            string pPosition = "";

                            while (pDR.Read())
                            {
                                pPosition = CheckDBString(pDR, "fldPosition");

                                int pIndx = 0;
                                if (pPosition == "Front")
                                {
                                    pIndx = 0;
                                }
                                else if (pPosition == "Back")
                                {
                                    pIndx = 1;
                                }

                                if(CheckDBString(pDR, "fldType")!= "")
                                    Type_Out[pIndx] = (clsEndConfig.eType) Enum.Parse(typeof(clsEndConfig.eType),
                                                                                       CheckDBString(pDR, "fldType"));
                            }

                            pDR.Close();
                            pDR = null;
                            pConnection.Close();
                        }


                        private void RetrieveRec_EndConfigs(clsProject Project_In)
                        //=======================================================              
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_EndConfigs ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);
                            string pPosition = "";

                            while (pDR.Read())
                            {
                                pPosition = CheckDBString(pDR, "fldPosition");
                                int pIndx = 0;
                                if (pPosition == "Front")
                                {
                                    pIndx = 0;
                                }
                                else if (pPosition == "Back")
                                {
                                    pIndx = 1;
                                }

                                Project_In.Product.EndConfig[pIndx].Mat.Base = CheckDBString(pDR, "fldMat_Base");
                                Project_In.Product.EndConfig[pIndx].Mat.Lining = CheckDBString(pDR, "fldMat_Lining");
                                Project_In.Product.EndConfig[pIndx].DO = CheckDBDouble(pDR, "fldDO");
                                Project_In.Product.EndConfig[pIndx].DBore_Range[0] = CheckDBDouble(pDR, "fldDBore_Range_Min");
                                Project_In.Product.EndConfig[pIndx].DBore_Range[1] = CheckDBDouble(pDR, "fldDBore_Range_Max");
                                Project_In.Product.EndConfig[pIndx].L = CheckDBDouble(pDR, "fldL");

                                RetrieveRec_EndConfig_MountHoles(Project_In, pPosition);

                                if (Project_In.Product.EndConfig[pIndx].Type == clsEndConfig.eType.Seal)
                                {
                                    RetrieveRec_EndConfig_Seal_Detail(Project_In, pIndx);
                                }
                                else if (Project_In.Product.EndConfig[pIndx].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    RetrieveRec_EndConfig_TB_Detail(Project_In, pIndx);
                                }
                            }

                            pDR.Close();
                            pDR = null;
                            pConnection.Close();
                        }

                    #endregion


                    #region "Retrieval: End Config - Mount Holes "

                        private void RetrieveRec_EndConfig_MountHoles(clsProject Project_In, string Position_In)
                        //=======================================================================================
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_EndConfig_MountHoles ";

                            int pIndx = 0;
                            if (Position_In == "Front")
                            {
                                pIndx = 0;
                            }
                            else if (Position_In == "Back")
                            {
                                pIndx = 1;
                            }

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix +
                                            "' AND fldPosition = '" + Position_In + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL" +
                                            " AND fldPosition = '" + Position_In + "'";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            while (pDR.Read())
                            {
                                if (CheckDBString(pDR, "fldType") != "")
                                    Project_In.Product.EndConfig[pIndx].MountHoles.Type = (clsEndConfig.clsMountHoles.eMountHolesType)
                                                                                                Enum.Parse(typeof(clsEndConfig.clsMountHoles.eMountHolesType),
                                                                                                CheckDBString(pDR, "fldType"));

                                (Project_In.Product.EndConfig[pIndx]).MountHoles.Depth_CBore = CheckDBDouble(pDR, "fldDepth_CBore");
                                (Project_In.Product.EndConfig[pIndx]).MountHoles.Thread_Thru = CheckDBBoolean(pDR, "fldThread_Thru");

                                (Project_In.Product.EndConfig[pIndx]).MountHoles.Depth_Thread = CheckDBDouble(pDR, "fldDepth_Thread");
                            }

                            pDR.Close();
                            pDR = null;
                            pConnection.Close();
                        }

                    #endregion


                    #region "Retrieval: End Config - Seal Detail"

                        private void RetrieveRec_EndConfig_Seal_Detail(clsProject Project_In, int PosIndx_In)
                        //==================================================================================
                        {
                            RetrieveRec_EndConfig_Seal(Project_In, PosIndx_In);
                            RetrieveRec_EndConfig_Seal_Blade(Project_In, PosIndx_In);
                            RetrieveRec_EndConfig_Seal_DrainHoles(Project_In, PosIndx_In);
                            RetrieveRec_EndConfig_Seal_WireClipHoles(Project_In, PosIndx_In);
                        }


                        #region "Retrieval: End Config - Seal"

                            private void RetrieveRec_EndConfig_Seal(clsProject Project_In, int PosIndx_In)
                            //============================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_Seal ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    if (CheckDBString(pDR, "fldType") != "")
                                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Design = (clsSeal.eDesign)
                                                                                                    Enum.Parse(typeof(clsSeal.eDesign),
                                                                                                    CheckDBString(pDR, "fldType"));    

                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Mat_LiningT = CheckDBDouble(pDR, "fldLiningT");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).TempSensor_D_ExitHole = CheckDBDouble(pDR, "fldTempSensor_D_ExitHole");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                            #endregion


                        #region "Retrieval: End Config Seal - Blade"

                            private void RetrieveRec_EndConfig_Seal_Blade(clsProject Project_In, int PosIndx_In)
                            //=================================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_Seal_Blade ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.Count = CheckDBInt(pDR, "fldCount");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.T = CheckDBDouble(pDR, "fldT");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).Blade.AngTaper = CheckDBDouble(pDR, "fldAngTaper");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                            #endregion


                        #region "Retrieval: End Config Seal - DrainHoles"

                            private void RetrieveRec_EndConfig_Seal_DrainHoles(clsProject Project_In, int PosIndx_In)
                            //======================================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_Seal_DrainHoles ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Annulus_Ratio_L_H = CheckDBDouble(pDR, "fldAnnulus_Ratio_L_H");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Annulus_D = CheckDBDouble(pDR, "fldAnnulus_D");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.D_Desig = CheckDBString(pDR, "fldD_Desig");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.Count = CheckDBInt(pDR, "fldCount");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngBet = CheckDBDouble(pDR, "fldAngBet");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngStart = CheckDBDouble(pDR, "fldAngStart");
                                    //((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngBet = CheckDBDouble(pDR, "fldAngBet");      //BG 31JAN13
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).DrainHoles.AngExit = CheckDBDouble(pDR, "fldAngExit");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: End Config Seal - WireClipHoles"

                            private void RetrieveRec_EndConfig_Seal_WireClipHoles(clsProject Project_In, int PosIndx_In)
                            //======================================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_Seal_WireClipHoles ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Exists = CheckDBBoolean(pDR, "fldExists");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Count = CheckDBInt(pDR, "fldCount");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.DBC = CheckDBDouble(pDR, "fldDBC");
                                    
                                    if (CheckDBString(pDR, "fldUnitSystem") != "")
                                        ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Unit.System = (clsUnit.eSystem)
                                                         Enum.Parse(typeof(clsUnit.eSystem), CheckDBString(pDR, "fldUnitSystem"));       

                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Screw_Spec.D_Desig = CheckDBString(pDR, "fldThread_Dia_Desig");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.Screw_Spec.Pitch = CheckDBDouble(pDR, "fldThread_Pitch");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.ThreadDepth = CheckDBDouble(pDR, "fldThread_Depth");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngStart = CheckDBDouble(pDR, "fldAngStart");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngOther[0] = CheckDBDouble(pDR, "fldAngOther1");
                                    ((clsSeal)Project_In.Product.EndConfig[PosIndx_In]).WireClipHoles.AngOther[1] = CheckDBDouble(pDR, "fldAngOther2");                                  
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion                        


                    #endregion


                    #region "Retrieval: End Config - TB Detail"

                        private void RetrieveRec_EndConfig_TB_Detail(clsProject Project_In, int PosIndx_In)
                        //================================================================================
                        {
                            RetrieveRec_Bearing_Thrust_TL(Project_In, PosIndx_In);
                            RetrieveRec_Bearing_Thrust_TL_PerformData(Project_In, PosIndx_In);
                            RetrieveRec_Bearing_Thrust_TL_FeedGroove(Project_In, PosIndx_In);
                            RetrieveRec_Bearing_Thrust_TL_WeepSlot(Project_In, PosIndx_In);
                            RetrieveRec_Bearing_Thrust_TL_GCodes(Project_In, PosIndx_In);
                        }
             

                        #region "Retrieval: End Config - Thrust_TL"

                            private void RetrieveRec_Bearing_Thrust_TL(clsProject Project_In, int PosIndx_In)
                            //==============================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_TB ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    if (CheckDBString(pDR, "fldDirectionType") != "")
                                        ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).DirectionType = (clsBearing_Thrust_TL.eDirectionType)
                                                                                                                         Enum.Parse(typeof(clsBearing_Thrust_TL.eDirectionType),
                                                                                                                         CheckDBString(pDR, "fldDirectionType"));

                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PadD[0] = CheckDBDouble(pDR, "fldPad_ID");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PadD[1] = CheckDBDouble(pDR, "fldPad_OD");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LandL = CheckDBDouble(pDR, "fldLandL");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LiningT_Face = CheckDBDouble(pDR, "fldLiningT_Face");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LiningT_ID = CheckDBDouble(pDR, "fldLiningT_ID");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Pad_Count = CheckDBInt(pDR, "fldPad_Count");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Depth_OD = CheckDBDouble(pDR, "fldTaper_Depth_OD");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Depth_ID = CheckDBDouble(pDR, "fldTaper_Depth_ID");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Taper_Angle = CheckDBDouble(pDR, "fldTaper_Angle");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Shroud_Ro = CheckDBDouble(pDR, "fldShroud_Ro");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Shroud_Ri = CheckDBDouble(pDR, "fldShroud_Ri");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Reqd = CheckDBBoolean(pDR, "fldBackRelief_Reqd");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_D = CheckDBDouble(pDR, "fldBackRelief_D");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Depth = CheckDBDouble(pDR, "fldBackRelief_Depth");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).BackRelief_Fillet = CheckDBDouble(pDR, "fldBackRelief_Fillet");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).LFlange = CheckDBDouble(pDR, "fldLFlange");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FaceOff_Assy = CheckDBDouble(pDR, "fldFaceOff_Assy");

                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: End Config - Thrust_TL - Perform Data"

                            private void RetrieveRec_Bearing_Thrust_TL_PerformData(clsProject Project_In, int PosIndx_In)
                            //===========================================================================================
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_TB_PerformData ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    // Perfor Data
                                    // ------------
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.Power_HP = CheckDBDouble(pDR, "fldPower_HP");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.FlowReqd_gpm = CheckDBDouble(pDR, "fldFlowReqd_gpm");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.TempRise_F = CheckDBDouble(pDR, "fldTempRise_F");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.TFilm_Min = CheckDBDouble(pDR, "fldTFilm_Min");

                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.PadMax_Temp = CheckDBDouble(pDR, "fldPadMax_Temp");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.PadMax_Press = CheckDBDouble(pDR, "fldPadMax_Press");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).PerformData.UnitLoad = CheckDBDouble(pDR, "fldUnitLoad");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: End Config - Thrust_TL:Feed Groove"

                            private void RetrieveRec_Bearing_Thrust_TL_FeedGroove(clsProject Project_In, int PosIndx_In)
                            //=========================================================================================              
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_TB_FeedGroove ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Type = CheckDBString(pDR, "fldType");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Wid = CheckDBDouble(pDR, "fldWid");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Depth = CheckDBDouble(pDR, "fldDepth");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.DBC = CheckDBDouble(pDR, "fldDBC");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedGroove.Dist_Chamf = CheckDBDouble(pDR, "fldDist_Chamf");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: End Config - Thrust_TL:Weep Slot"

                            private void RetrieveRec_Bearing_Thrust_TL_WeepSlot(clsProject Project_In, int PosIndx_In)
                            //=======================================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_TB_WeepSlot ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Type = (clsBearing_Thrust_TL.clsWeepSlot.eType)
                                                                                                                     Enum.Parse(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType), CheckDBString(pDR, "fldType"));
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Wid = CheckDBDouble(pDR, "fldWid");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).WeepSlot.Depth = CheckDBDouble(pDR, "fldDepth");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }

                        #endregion


                        #region "Retrieval: End Config - Thrust_TL:GCodes"

                            private void RetrieveRec_Bearing_Thrust_TL_GCodes(clsProject Project_In, int PosIndx_In)
                            //=======================================================================================             
                            {
                                string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL, pPosition = "";

                                pstrFIELDS = "* ";
                                pstrFROM = "FROM tblProject_EndConfig_TB_GCodes ";

                                if (PosIndx_In == 0)
                                {
                                    pPosition = "Front";
                                }
                                else
                                {
                                    pPosition = "Back";
                                }

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "' AND fldPosition = '" + pPosition + "'";
                                else
                                    pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + pPosition + "'";

                                pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                                SqlConnection pConnection = new SqlConnection();

                                SqlDataReader pDR = null;
                                pDR = GetDataReader(pstrSQL, ref pConnection);

                                while (pDR.Read())
                                {  
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T1.D_Desig = CheckDBString(pDR, "fldD_Desig_T1");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T2.D_Desig = CheckDBString(pDR, "fldD_Desig_T2");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T3.D_Desig = CheckDBString(pDR, "fldD_Desig_T3");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).T4.D_Desig = CheckDBString(pDR, "fldD_Desig_T4");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Overlap_frac = CheckDBDouble(pDR, "fldOverlap_frac");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedRate_Taperland = CheckDBDouble(pDR, "fldFeed_Taperland");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FeedRate_WeepSlot = CheckDBDouble(pDR, "fldFeed_WeepSlot");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_TL_Backlash = CheckDBDouble(pDR, "fldDepth_Backlash");
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_TL_Dwell_T = CheckDBDouble(pDR, "fldDepth_Dwell_T");
                                    //((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).RMargin_WeepSlot = CheckDBDouble(pDR, "fldRMargin_WeepSlot");        //BG 03APR13
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Depth_WS_Cut_Per_Pass = CheckDBDouble(pDR, "fldDepth_WeepSlot_Cut");
                                    //((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).Starting_LineNo = CheckDBInt(pDR, "fldStarting_LineNo");         //BG 03APR13
                                    ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[PosIndx_In]).FilePath_Dir = CheckDBString(pDR, "fldFilePath_Dir");
                                }

                                pDR.Close();
                                pDR = null;
                                pConnection.Close();
                            }
                            
                        #endregion

                    #endregion


                    #region "Retrieval: Accessories"

                        private void RetrieveRec_Accessories(clsProject Project_In)
                        //=========================================================              
                        {
                            string pstrFIELDS, pstrFROM, pstrWHERE, pstrSQL;

                            pstrFIELDS = "* ";
                            pstrFROM = "FROM tblProject_Product_Accessories ";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = "WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrSQL = "SELECT " + pstrFIELDS + pstrFROM + pstrWHERE;

                            SqlConnection pConnection = new SqlConnection();

                            SqlDataReader pDR = null;
                            pDR = GetDataReader(pstrSQL, ref pConnection);

                            if (pDR.Read())
                            {
                                // Perfor Data
                                // ------------
                                Project_In.Product.Accessories.TempSensor_Supplied = CheckDBBoolean(pDR, "fldTempSensor_Supplied");
                                Project_In.Product.Accessories.TempSensor_Name = (clsAccessories.eTempSensorName)Enum.Parse(typeof(clsAccessories.eTempSensorName),
                                                                                    CheckDBString(pDR, "fldTempSensor_Name"));
                                Project_In.Product.Accessories.TempSensor_Count = CheckDBInt(pDR, "fldTempSensor_Count");
                                Project_In.Product.Accessories.TempSensor_Type = (clsAccessories.eTempSensorType)Enum.Parse(typeof(clsAccessories.eTempSensorType),
                                                                                    CheckDBString(pDR, "fldTempSensor_Type"));

                                Project_In.Product.Accessories.WireClip_Supplied = CheckDBBoolean(pDR, "fldWireClip_Supplied");
                                Project_In.Product.Accessories.WireClip_Count = CheckDBInt(pDR, "fldWireClip_Count");
                                Project_In.Product.Accessories.WireClip_Size = (clsAccessories.eWireClipSize)Enum.Parse(typeof(clsAccessories.eWireClipSize),
                                                                                CheckDBString(pDR, "fldWireClip_Size"));
                            }

                            pDR.Close();
                            pDR = null;
                            pConnection.Close();
                        }

                    #endregion

                #endregion


                #region "Save Global Data"
                //------------------------

                    public void SaveData_Global ()
                    //============================
                    {
                        if (modMain.gDB.ProjectNo_Exists(modMain.gProject.No, modMain.gProject.No_Suffix, "tblProject_Details"))
                        {
                            modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
                        }
                        else
                        {
                            modMain.gDB.AddRecord(modMain.gProject, modMain.gOpCond);
                        }
                    }


                #endregion


                #region "DATABASE INSERTION ROUTINES:"

                    public void AddRecord(clsProject Project_In, clsOpCond OpCond_In)
                    //================================================================
                    {
                        AddRec_Project(Project_In);
                        AddRec_Product(Project_In);
                        AddRec_OpCond(Project_In, OpCond_In);
                        AddRec_Bearing_Radial(Project_In);
                        AddRec_Bearing_Radial_FP_Detail(Project_In);

                        AddRec_EndConfigs(Project_In);
                        AddRec_EndConfig_MountHoles(Project_In);

                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                            Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)

                        AddRec_EndConfig_Seal_Detail(Project_In);

                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                            Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        AddRec_EndConfig_TB_Detail(Project_In);

                        AddRec_Project_Product_Accessories(Project_In);
                    }


                    #region "Project Table:"

                        private int AddRec_Project(clsProject Project_In)
                        //===============================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                string pFIELDS = "(fldNo,fldNo_Suffix,fldStatus,fldCustomer_Name,fldCustomer_MachineDesc, fldCustomer_PartNo,fldUnitSystem," +
                                                 "fldAssyDwg_No,fldAssyDwg_No_Suffix, fldAssyDwg_Ref," +
                                                 "fldEngg_Name,fldEngg_Initials,fldEngg_Date," +
                                                 "fldDesignedBy_Name,fldDesignedBy_Initials,fldDesignedBy_Date," +
                                                 "fldCheckedby_Name,fldCheckedby_Initials,fldCheckedby_Date," +
                                                 "fldFilePath_Project, fldFilePath_DesignTbls_SWFiles, fldFileModified_CompleteAssy, fldFileModified_Radial_Part," +   //BG 05DEC12
                                                 "fldFileModified_Radial_BlankAssy, fldFileModified_EndTB_Part," +
                                                 "fldFileModified_EndTB_Assy, fldFileModified_EndSeal_Part," +
                                                 "fldFileModification_Notes)";

                                string pINSERT = "INSERT INTO tblProject_Details";
                                string pVALUES = null;

                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";


                                int pFileModified_RadialPart = 0, pFileModified_RadialBlankAssy = 0, pFileModified_EndTB_Part = 0;
                                int pFileModified_EndTB_Assy = 0, pFileModified_EndSeal_Part = 0, pFileModified_CompleteAssy = 0;

                                if (Project_In.FileModified_RadialPart)
                                    pFileModified_RadialPart = 1;
                                else
                                    pFileModified_RadialPart = 0;

                                if (Project_In.FileModified_RadialBlankAssy)
                                    pFileModified_RadialBlankAssy = 1;
                                else
                                    pFileModified_RadialBlankAssy = 0;

                                if (Project_In.FileModified_EndTB_Part)
                                    pFileModified_EndTB_Part = 1;
                                else
                                    pFileModified_EndTB_Part = 0;

                                if (Project_In.FileModified_EndTB_Assy)
                                    pFileModified_EndTB_Assy = 1;
                                else
                                    pFileModified_EndTB_Assy = 0;

                                if (Project_In.FileModified_EndSeal_Part)
                                    pFileModified_EndSeal_Part = 1;
                                else
                                    pFileModified_EndSeal_Part = 0;

                                if (Project_In.FileModified_CompleteAssy)
                                    pFileModified_CompleteAssy = 1;
                                else
                                    pFileModified_CompleteAssy = 0;
                                
                                if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) != mstrDefDate &&
                                        Project_In.CheckedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
                                {
                                    pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Status + "','" + Project_In.Customer.Name + "','" +
                                             Project_In.Customer.MachineDesc + "','" + Project_In.Customer.PartNo + "','" + Project_In.Unit.System.ToString() + "','" +
                                             Project_In.AssyDwg.No + "','" + Project_In.AssyDwg.No_Suffix + "','" + Project_In.AssyDwg.Ref + "','" + Project_In.Engg.Name + "','" +
                                             Project_In.Engg.Initials + "','" + Project_In.Engg.Date.ToString("d", mInvCulture) + "','" + Project_In.DesignedBy.Name + "','" +
                                             Project_In.DesignedBy.Initials + "','" + Project_In.DesignedBy.Date.ToString("d", mInvCulture) + "','" +
                                             Project_In.CheckedBy.Name + "','" + Project_In.CheckedBy.Initials + "','" + Project_In.CheckedBy.Date.ToString("d", mInvCulture) + "','" +
                                             Project_In.FilePath_Project + "', '" + Project_In.FilePath_DesignTbls_SWFiles + "'," + pFileModified_CompleteAssy + "," + pFileModified_RadialPart + "," +       //BG 05DEC12
                                             pFileModified_RadialBlankAssy + "," +  pFileModified_EndTB_Part + "," + pFileModified_EndTB_Assy + "," +
                                             pFileModified_EndSeal_Part + ",'" + Project_In.FileModification_Notes + "')";          //BG 05DEC12

                                }

                                else if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) != mstrDefDate &&
                                            Project_In.CheckedBy.Date.ToString("d", mInvCulture) == mstrDefDate)
                                {
                                    pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Status + "','" + Project_In.Customer.Name + "','" +
                                               Project_In.Customer.MachineDesc + "','" + Project_In.Customer.PartNo + "','" + Project_In.Unit.System.ToString() + "','" +
                                               Project_In.AssyDwg.No + "','" + Project_In.AssyDwg.No_Suffix + "','" + Project_In.AssyDwg.Ref + "','" + Project_In.Engg.Name + "','" +
                                               Project_In.Engg.Initials + "','" + Project_In.Engg.Date.ToString("d", mInvCulture) + "','" + Project_In.DesignedBy.Name + "','" +
                                               Project_In.DesignedBy.Initials + "','" + Project_In.DesignedBy.Date.ToString("d", mInvCulture) + "','" +
                                               Project_In.CheckedBy.Name + "','" + Project_In.CheckedBy.Initials + "'," + "NULL,'"+ Project_In.FilePath_Project + "','" +
                                               Project_In.FilePath_DesignTbls_SWFiles + "'," + pFileModified_CompleteAssy + "," + pFileModified_RadialPart + "," + pFileModified_RadialBlankAssy + "," +      //BG 05DEC12
                                               pFileModified_EndTB_Part + "," + pFileModified_EndTB_Assy + "," + pFileModified_EndSeal_Part + ",'" + Project_In.FileModification_Notes + "')";          //BG 05DEC12
                                }

                                else if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) == mstrDefDate &&
                                            Project_In.CheckedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
                                {
                                    pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Status + "','" + Project_In.Customer.Name + "','" +
                                          Project_In.Customer.MachineDesc + "','" + Project_In.Customer.PartNo + "','" + Project_In.Unit.System.ToString() + "','" +
                                          Project_In.AssyDwg.No + "','" + Project_In.AssyDwg.No_Suffix + "','" + Project_In.AssyDwg.Ref + "','" + Project_In.Engg.Name + "','" +
                                          Project_In.Engg.Initials + "','" + Project_In.Engg.Date.ToString("d", mInvCulture) + "','" + Project_In.DesignedBy.Name + "','" +
                                          Project_In.DesignedBy.Initials + "'," + "NULL,'" +
                                          Project_In.CheckedBy.Name + "','" + Project_In.CheckedBy.Initials + "','" + Project_In.CheckedBy.Date.ToString("d", mInvCulture) + "','" +
                                          Project_In.FilePath_Project + "', '" + Project_In.FilePath_DesignTbls_SWFiles + "'," + pFileModified_CompleteAssy + "," + pFileModified_RadialPart + "," +      //BG 05DEC12
                                          pFileModified_RadialBlankAssy + "," + pFileModified_EndTB_Part + "," + pFileModified_EndTB_Assy + "," +
                                          pFileModified_EndSeal_Part + ",'" + Project_In.FileModification_Notes + "')";         
                                }

                                else if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) == mstrDefDate &&
                                            Project_In.CheckedBy.Date.ToString("d", mInvCulture) == mstrDefDate)
                                {
                                    pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Status + "','" + Project_In.Customer.Name + "','" +
                                        Project_In.Customer.MachineDesc + "','" + Project_In.Customer.PartNo + "','" + Project_In.Unit.System.ToString() + "','" +
                                        Project_In.AssyDwg.No + "','" + Project_In.AssyDwg.No_Suffix + "','" + Project_In.AssyDwg.Ref + "','" + Project_In.Engg.Name + "','" +
                                        Project_In.Engg.Initials + "','" + Project_In.Engg.Date.ToString("d", mInvCulture) + "','" + Project_In.DesignedBy.Name + "','" +
                                        Project_In.DesignedBy.Initials + "'," + "NULL,'" +
                                        Project_In.CheckedBy.Name + "','" + Project_In.CheckedBy.Initials + "'," + "NULL,'" + Project_In.FilePath_Project + "','" +
                                        Project_In.FilePath_DesignTbls_SWFiles + "'," + pFileModified_CompleteAssy + "," + pFileModified_RadialPart + "," + pFileModified_RadialBlankAssy + "," +
                                        pFileModified_EndTB_Part + "," + pFileModified_EndTB_Assy + "," + pFileModified_EndSeal_Part + ",'" + Project_In.FileModification_Notes + "')";
                                }

                                string pSQL = pINSERT + pFIELDS + pVALUES;
                                pCountRec = ExecuteCommand(pSQL);

                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Record insert error.");
                                return pCountRec;
                            }

                            return pCountRec;
                        }

                    #endregion


                    #region "Product"

                        private int AddRec_Product(clsProject Project_In)
                        //===============================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                //SG 23JAN13
                                //string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldBearing_Type,fldEndConfig_Front_Type," +
                                //                 "fldEndConfig_Back_Type,fldL_Available, fldDist_ThrustFace_Front, fldDist_ThrustFace_Back)";
                                string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldBearing_Type," +
                                                 "fldL_Available, fldDist_ThrustFace_Front, fldDist_ThrustFace_Back)";

                                string pINSERT = "INSERT INTO tblProject_Product";
                                string pVALUES = null;

                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";

                                //SG 23JAN13
                                //pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Product.Bearing.Type.ToString() + "','" +
                                //            Project_In.Product.EndConfig[0].Type.ToString() + "','" + Project_In.Product.EndConfig[1].Type.ToString() + "'," +
                                //            Project_In.Product.L_Available + "," + Project_In.Product.Dist_ThrustFace[0] + "," + Project_In.Product.Dist_ThrustFace[1] + ")";

                                pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + Project_In.Product.Bearing.Type.ToString() + "'," +                                            
                                            Project_In.Product.L_Available + "," + Project_In.Product.Dist_ThrustFace[0] + "," + Project_In.Product.Dist_ThrustFace[1] + ")";

                                string pSQL = pINSERT + pFIELDS + pVALUES;
                                pCountRec = ExecuteCommand(pSQL);
                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Record insert error.");
                                return pCountRec;
                            }

                            return pCountRec;
                        }             

                    #endregion


                    #region "Addition: Operating Condition"

                        private int AddRec_OpCond(clsProject Project_In, clsOpCond OpCond_In)        
                        //===================================================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                string pFIELDS = "(fldProjectNo, fldProjectNo_Suffix, fldSpeed_Range_Min, fldSpeed_Range_Max," +
                                                 "fldRot_Directionality, fldRadial_Load_Range_Min, fldRadial_Load_Range_Max," +
                                                 "fldRadial_LoadAng, fldThrust_Load_Range_Front_Min, fldThrust_Load_Range_Front_Max," +
                                                 "fldThrust_Load_Range_Back_Min, fldThrust_Load_Range_Back_Max," +
                                                 "fldOilSupply_Lube_Type,fldOilSupply_Type,fldOilSupply_Press,fldOilSupply_Temp)";

                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";

                                //....SET Default Data:
                                //---------------------
                                OpCond_In.OilSupply_Type = "Pressurized";
                                OpCond_In.OilSupply_Lube_Type = "ISO 32";    

                                string pINSERT = "INSERT INTO tblProject_OpCond";

                                string pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", " + OpCond_In.Speed_Range[0] + ","
                                                            + OpCond_In.Speed_Range[1] + ", '"
                                                            + OpCond_In.Rot_Directionality + "'," + OpCond_In.Radial_Load_Range[0] + ","
                                                            + OpCond_In.Radial_Load_Range[1] + "," + OpCond_In.Radial_LoadAng + ","
                                                            + OpCond_In.Thrust_Load_Range_Front[0] + "," + OpCond_In.Thrust_Load_Range_Front[1] + ","
                                                            + OpCond_In.Thrust_Load_Range_Back[0] + "," + OpCond_In.Thrust_Load_Range_Back[1] + ",'"
                                                            + OpCond_In.OilSupply.Lube.Type + "','" + OpCond_In.OilSupply.Type + "',"
                                                            + OpCond_In.OilSupply.Press + "," + OpCond_In.OilSupply.Temp + ")";



                                string pSQL = pINSERT + pFIELDS + pVALUES;
                                pCountRec = ExecuteCommand(pSQL);
                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Data Insert Error.\n" + pEXP.Message);
                            }

                            return pCountRec;
                        }


                    #endregion


                    #region "Addition: Bearing Radial"

                        private int AddRec_Bearing_Radial(clsProject Project_In)
                        //=====================================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldDesign)";


                                string pINSERT = "INSERT INTO tblProject_Bearing_Radial";
                                string pVALUES = null;

                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";

                                pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" +
                                                   ((clsBearing_Radial)Project_In.Product.Bearing).Design.ToString() + "')";

                                string pSQL = pINSERT + pFIELDS + pVALUES;
                                pCountRec = ExecuteCommand(pSQL);

                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Record insert error.");
                                return pCountRec;
                            }

                            return pCountRec;
                        }

                    #endregion


                    #region "Addition: Bearing Radial FP Detail"

                        private void AddRec_Bearing_Radial_FP_Detail(clsProject Project_In)
                        //=================================================================
                        {
                            AddRec_Bearing_Radial_FP(Project_In);
                            AddRec_Bearing_Radial_FP_PerformData(Project_In);
                            AddRec_Bearing_Radial_FP_Pad(Project_In);
                            AddRec_Bearing_Radial_FP_FlexurePivot(Project_In);
                            AddRec_Bearing_Radial_FP_OilInlet(Project_In);
                            AddRec_Bearing_Radial_FP_MillRelief(Project_In);
                            AddRec_Bearing_Radial_FP_Flange(Project_In);
                            AddRec_Bearing_Radial_FP_SL(Project_In);
                            AddRec_Bearing_Radial_FP_AntiRotPin(Project_In);
                            AddRec_Bearing_Radial_FP_Mount(Project_In);
                            AddRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                            AddRec_Bearing_Radial_FP_TempSensor(Project_In);
                            AddRec_Bearing_Radial_FP_EDM_Pad(Project_In);
                        }


                        #region "Addition: Bearing Radial FP"

                            private int AddRec_Bearing_Radial_FP(clsProject Project_In)
                            //=========================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix, fldSplitConfig, " +
                                                     "fldDShaft_Range_Min,fldDShaft_Range_Max," +
                                                     "fldDFit_Range_Min,fldDFit_Range_Max," +
                                                     "fldDSet_Range_Min, fldDSet_Range_Max," +
                                                     "fldDPad_Range_Min, fldDPad_Range_Max," +
                                                     "fldL,fldDepth_EndConfig_Front, fldDepth_EndConfig_Back, " +
                                                     "fldDimStart_FrontFace,fldMat_Base,fldMat_LiningExists," +
                                                     "fldMat_Lining,fldLiningT)";                                

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pSplitConfig = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig) pSplitConfig = 1;

                                    int pMat_LiningExists = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists) pMat_LiningExists = 1;
                             
                                    //  Set Default Value:          
                                    //  ------------------
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base == null || ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base == "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base = "Steel 4340";

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining == null || ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining == "")
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining = "Babbitt";

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT == 0.00F)
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT = 0.020F;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", " +
                                                                 + pSplitConfig + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] + "," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0] + "," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1] + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] + "," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1] + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0] + "," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1] + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).L + "," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0] + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1] + ","+ ((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace + ",'"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base + "'," + pMat_LiningExists + ",'"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining + "'," + ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Perform Data"

                            private int AddRec_Bearing_Radial_FP_PerformData(clsProject Project_In)
                            //====================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPower_HP,fldFlowReqd_gpm,fldTempRise_F," +
                                                     "fldTFilm_Min,fldPadMax_Temp,fldPadMax_Press,fldPadMax_Rot," +
                                                     "fldPadMax_Load,fldPadMax_Stress)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_PerformData";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";
                                 
                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", " 
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);

                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Pad"

                            private int AddRec_Bearing_Radial_FP_Pad(clsProject Project_In)
                            //============================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldType,fldCount," +
                                                     "fldL,fldPivot_Offset,fldPivot_AngStart," +
                                                     "fldT_Lead,fldT_Pivot,fldT_Trail)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_Pad";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count == 0)
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count = 4;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", '"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type + "', "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);

                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Flexure Pivot"

                            private int AddRec_Bearing_Radial_FP_FlexurePivot(clsProject Project_In)
                            //=====================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldWeb_T,fldWeb_H," +
                                                     "fldWeb_RFillet,fldGapEDM,fldRot_Stiff)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_FlexurePivot";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM == 0.00F)
                                        ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM = 0.014F;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);

                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Oil Inlet"

                            private int AddRec_Bearing_Radial_FP_OilInlet(clsProject Project_In)
                            //==================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldOrifice_Count,fldOrifice_D," +
                                                     "fldCount_MainOilSupply, fldOrifice_StartPos, " +
                                                     "fldOrifice_DDrill_CBore, fldOrifice_Loc_FrontFace, " +
                                                     "fldOrifice_L, fldOrifice_AngStart_BDV, fldAnnulus_Exists, " +
                                                     "fldAnnulus_D, fldAnnulus_Loc_Back, " +
                                                     "fldAnnulus_L, fldOrifice_Dist_Holes)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_OilInlet";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pAnnulus_Exists = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
                                        pAnnulus_Exists = 1;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV + ", "
                                                                + pAnnulus_Exists + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Mill Relief"

                            private int AddRec_Bearing_Radial_FP_MillRelief(clsProject Project_In)
                            //===================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldExists,fldD_Desig,fldEDMRelief_Front,fldEDMRelief_Back)";
                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_MillRelief";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pMillRelief = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists) pMillRelief = 1;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                 + pMillRelief + ", '"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig + "'," +
                                                                 +((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0] + "," +
                                                                 +((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1] + ")"; //SG 24JAN13

                                    string pSQL = pINSERT + pFIELDS + pVALUES;

                                    pCountRec = ExecuteCommand(pSQL);                                   
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Flange"

                            private int AddRec_Bearing_Radial_FP_Flange(clsProject Project_In)
                            //===============================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldExists,fldD,fldWid,fldDimStart_Front)";
                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_Flange";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pFlange_Exists = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists)
                                        pFlange_Exists = 1;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                 + pFlange_Exists + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);

                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - SL"

                            private int AddRec_Bearing_Radial_FP_SL(clsProject Project_In)
                            //===========================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldScrew_Spec_UnitSystem, " +
                                                     "fldScrew_Spec_Type,fldScrew_Spec_D_Desig, " +
                                                     "fldScrew_Spec_Pitch, fldScrew_Spec_L, " +
                                                     "fldScrew_Spec_Mat, fldLScrew_Spec_Loc_Center, " +
                                                     "fldLScrew_Spec_Loc_Front, fldRScrew_Spec_Loc_Center, " +
                                                     "fldRScrew_Spec_Loc_Front, fldThread_Depth, fldCBore_Depth, " +
                                                     "fldDowel_Spec_UnitSystem, fldDowel_Spec_Type, fldDowel_Spec_D_Desig, " +
                                                     "fldDowel_Spec_L, fldDowel_Spec_Mat, fldLDowel_Spec_Loc_Center, " +
                                                     "fldLDowel_Spec_Loc_Front, fldRDowel_Spec_Loc_Center, " +
                                                     "fldRDowel_Spec_Loc_Front, fldDowel_Depth)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_SL";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";
                                   
                                    string pVALUES = "VALUES ('"+ Project_In.No + "'," + pNo_Suffix + ", '"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString() + "','"           //BG 26MAR12
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type + "','"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString() + "','"       //BG 26MAR12
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type + "','"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Anti Rot Pin"

                            private int AddRec_Bearing_Radial_FP_AntiRotPin(clsProject Project_In)
                            //===================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldLoc_Dist_Front,fldLoc_Casing_SL, " +
                                                     "fldLoc_Offset, fldLoc_Bearing_Vert, fldLoc_Bearing_SL, " +                    //BG 08MAY12
                                                     "fldLoc_Angle, fldSpec_UnitSystem, fldSpec_Type, " +
                                                     "fldSpec_D_Desig, fldSpec_L, " +
                                                     "fldSpec_Mat, fldDepth, fldStickOut)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_AntiRotPin";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL.ToString() + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString() + "','"     //BG 08MAY12
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL.ToString() + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System.ToString() + "','"         //BG 26MAR12
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type + "','"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat + "',"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Mount"

                            private int AddRec_Bearing_Radial_FP_Mount(clsProject Project_In)
                            //==============================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldHoles_GoThru,fldHoles_Bolting, " +                                                    
                                                     "fldFixture_Candidates_Chosen_Front, fldFixture_Candidates_Chosen_Back," +
                                                     "fldHoles_Thread_Depth_Front, fldHoles_Thread_Depth_Back)"; 

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_Mount";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pHolesGoThru = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru) pHolesGoThru = 1;

                                    int pFixtureCandidate_Chosen_Front = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixtureCandidate_Chosen_Front = 1;

                                    int pFixtureCandidate_Chosen_Back = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixtureCandidate_Chosen_Back = 1;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                + pHolesGoThru + ",'"
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString() + "',"                                                               
                                                                + pFixtureCandidate_Chosen_Front + ","
                                                                + pFixtureCandidate_Chosen_Back + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] + ","
                                                                + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1] + ")";                                                             
                                                                

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Mount Fixture"

                            private int AddRec_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In)
                            //=======================================================================            
                            {
                                int pCountRec = 0; string pVALUES = "";

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition,fldPartNo, " +
                                                     "fldDBC, fldD_Finish, fldHolesCount, fldHolesEquispaced, fldHolesAngStart," +
                                                     "fldHolesAngStart_Comp_Chosen, fldHolesAngOther1, fldHolesAngOther2," +
                                                     "fldHolesAngOther3, fldHolesAngOther4, fldHolesAngOther5, fldHolesAngOther6," +
                                                     "fldHolesAngOther7, fldScrew_Spec_Type," +
                                                     "fldScrew_Spec_D_Desig, fldScrew_Spec_L, fldScrew_Spec_Mat)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_Mount_Fixture";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pHolesEquispaced = 0;
                                    int pHolesAngStart_Comp_Chosen = 0;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced) pHolesEquispaced = 1;
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                        pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", '"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString() + "','"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo + "',"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].D_Finish + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount + ","
                                                              + pHolesEquispaced + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart + ","
                                                              + pHolesAngStart_Comp_Chosen + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[0] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[1] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[2] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[3] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[4] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[5] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[6] + ",'"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type + "','"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig + "',"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.L + ",'"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat + "')";

                                        string pSQL = pINSERT + pFIELDS + pVALUES;
                                        pCountRec = ExecuteCommand(pSQL);
                                    }

                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced) pHolesEquispaced = 1;
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                        pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", '"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString() + "','"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo + "',"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].D_Finish + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount + ","
                                                              + pHolesEquispaced + ","
                                                              //+ ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart + ","     //BG 20MAR13
                                                              + pHolesAngStart_Comp_Chosen + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[0] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[1] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[2] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[3] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[4] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[5] + ","
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[6] + ",'"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Type + "','"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig + "',"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.L + ",'"
                                                              + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Mat + "')";

                                        string pSQL = pINSERT + pFIELDS + pVALUES;
                                        pCountRec = ExecuteCommand(pSQL);
                                    }

                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                                    {
                                        string[] pPosition = new string[] { "Front", "Back" };

                                        for (int i = 0; i < 2; i++)
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced) pHolesEquispaced = 1;
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                            pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", '" + pPosition[i] + "','"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].PartNo + "',"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].DBC + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].D_Finish + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesCount + ","
                                                                  + pHolesEquispaced + ","
                                                                  //+ ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngStart + ","     //BG 20MAR13
                                                                  + pHolesAngStart_Comp_Chosen + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[0] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[1] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[2] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[3] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[4] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[5] + ","
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[6] + ",'"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.Type + "','"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.D_Desig + "',"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.L + ",'"
                                                                  + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.Mat + "')";

                                            string pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }                                  
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - Temp Sensor"

                            private int AddRec_Bearing_Radial_FP_TempSensor(clsProject Project_In)
                            //===================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldExists,fldCanLength,fldCount," +
                                                     "fldLoc,fldD,fldDepth,fldAngStart)";

                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_TempSensor";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    int pExists = 0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists) pExists = 1;

                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                 + pExists + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength + ","                                                               
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count + ",'"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc + "',"
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth + ","
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: Bearing Radial FP - EDM Pad"

                            private int AddRec_Bearing_Radial_FP_EDM_Pad(clsProject Project_In)
                            //=================================================================            
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldRFillet_Back,fldAngStart_Web)";
                                    string pINSERT = "INSERT INTO tblProject_Bearing_Radial_FP_EDM_Pad";

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";
                     
                                    string pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back + ", "
                                                                 + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web + ")";

                                    string pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);

                                }
                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Add Record Error.\n" + pEXP.Message);
                                }
                                return pCountRec;
                            }

                        #endregion
                                               
                    #endregion


                    #region "Addition: End Config Details"

                            private int AddRec_EndConfigs(clsProject Project_In)
                            //===================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition,fldType," +
                                                     "fldMat_Base,fldMat_Lining, fldDO, fldDBore_Range_Min," +
                                                     "fldDBore_Range_Max, fldL)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfigs";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";

                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                           Project_In.Product.EndConfig[i].Type.ToString() + "','" +
                                                           Project_In.Product.EndConfig[i].Mat.Base + "','" + Project_In.Product.EndConfig[i].Mat.Lining + "'," +
                                                           Project_In.Product.EndConfig[i].DO + "," + Project_In.Product.EndConfig[i].DBore_Range[0] + "," +
                                                           Project_In.Product.EndConfig[i].DBore_Range[1] + "," + Project_In.Product.EndConfig[i].L + ")";
                                                                               
                                        pSQL = pINSERT + pFIELDS + pVALUES;
                                        pCountRec = ExecuteCommand(pSQL);                                    
                                    }                                  
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }             

                    #endregion


                    #region "Addition: End Config - Mount Holes"

                        private int AddRec_EndConfig_MountHoles(clsProject Project_In)
                        //============================================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix, fldPosition," +
                                                  "fldType, fldDepth_CBore, fldThread_Thru, fldDepth_Thread)";


                                string pINSERT = "INSERT INTO tblProject_EndConfig_MountHoles";
                                string pVALUES = null;
                                string[] pPosition = new string[] { "Front", "Back" };

                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";

                                int pThread_Thru = 0; 
                                string pSQL = "";

                                for (int i = 0; i < pPosition.Length; i++)
                                {
                                    if (Project_In.Product.EndConfig[i].MountHoles.Thread_Thru) pThread_Thru = 1;

                                    pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                        Project_In.Product.EndConfig[i].MountHoles.Type.ToString() + "'," + 
                                                        Project_In.Product.EndConfig[i].MountHoles.Depth_CBore + "," +
                                                        pThread_Thru + "," +
                                                        Project_In.Product.EndConfig[i].MountHoles.Depth_Thread + ")";

                                    pSQL = pINSERT + pFIELDS + pVALUES;
                                    pCountRec = ExecuteCommand(pSQL);
                                }
                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Record insert error.");
                                return pCountRec;
                            }

                            return pCountRec;
                        }

                    #endregion


                    #region "Addition: End Config - Seal Details"

                        private void AddRec_EndConfig_Seal_Detail(clsProject Project_In)
                        //=============================================================
                        {
                            AddRec_EndConfig_Seal(Project_In);
                            AddRec_EndConfig_Seal_Blade(Project_In);
                            AddRec_EndConfig_Seal_DrailnHoles(Project_In);
                            AddRec_EndConfig_Seal_WireClipHoles(Project_In);                            
                        }

                        #region "Addition: End Config - Seal"

                            private int AddRec_EndConfig_Seal(clsProject Project_In)
                            //=======================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldType,fldLiningT,fldTempSensor_D_ExitHole)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_Seal";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";
                                  
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                              //((clsSeal)Project_In.Product.EndConfig[i]).Type.ToString() + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).Design.ToString() + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).Mat_LiningT + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).TempSensor_D_ExitHole + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }                                  
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Seal - Blade"

                            private int AddRec_EndConfig_Seal_Blade(clsProject Project_In)
                            //============================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldCount,fldT,fldAngTaper)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_Seal_Blade";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";


                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).Blade.Count + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).Blade.T + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).Blade.AngTaper + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }
                                   
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Seal - Drain Holes"

                            private int AddRec_EndConfig_Seal_DrailnHoles(clsProject Project_In)
                            //==================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldAnnulus_Ratio_L_H,fldAnnulus_D,fldD_Desig," +
                                                      "fldCount,fldAngStart, fldAngBet, fldAngExit)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_Seal_DrainHoles";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.Ratio_L_H + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.D + ",'" +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.D_Desig + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Count + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngStart + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngBet + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngExit + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }                                    
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Seal - Wire Clip Holes"

                            private int AddRec_EndConfig_Seal_WireClipHoles(clsProject Project_In)
                            //====================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldExists,fldCount,fldDBC, fldUnitSystem, fldThread_Dia_Desig," +
                                                      "fldThread_Pitch, fldThread_Depth, fldAngStart,"+
                                                      "fldAngOther1, fldAngOther2)";                                                     

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_Seal_WireClipHoles";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";


                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        int pExists = 0; 
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                        {
                                            if (((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Exists) pExists = 1;

                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "'," +
                                                              pExists + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Count + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.DBC + ",'" +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Unit.System.ToString() + "','" +      //BG 03JUL13
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.D_Desig + "'," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.Pitch + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.ThreadDepth + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngStart + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[0] + "," +
                                                              ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[1] + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }                                   
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                    #endregion


                    #region "Addition: End Config - TB Details"

                        private void AddRec_EndConfig_TB_Detail(clsProject Project_In)
                        //============================================================
                        {
                            AddRec_Bearing_Thrust_TL(Project_In);
                            AddRec_Bearing_Thrust_TL_PerformData(Project_In);
                            AddRec_Bearing_Thrust_TL_FeedGroove(Project_In);
                            AddRec_Bearing_Thrust_TL_WeepSlot(Project_In);
                            AddRec_Bearing_Thrust_TL_GCodes(Project_In);
                        }


                        #region "Addition: End Config - Thrust_TL"

                            private int AddRec_Bearing_Thrust_TL(clsProject Project_In)
                            //=========================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition,fldDirectionType," +
                                                      "fldPad_ID,fldPad_OD,fldLandL,fldLiningT_Face," +
                                                      "fldLiningT_ID, fldPad_Count, fldTaper_Depth_OD," +
                                                      "fldTaper_Depth_ID, fldTaper_Angle, fldShroud_Ro," +
                                                      "fldShroud_Ri, fldBackRelief_Reqd, fldBackRelief_D," +
                                                      "fldBackRelief_Depth, fldBackRelief_Fillet, fldDimStart," +
                                                      "fldLFlange, fldFaceOff_Assy)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_TB";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        int pReqd = 0;
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            if (((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Reqd) pReqd = 1;

                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DirectionType.ToString() + "'," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[0] + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[1] + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LandL + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.Face + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.ID + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Pad_Count + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_OD + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_ID + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Angle + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ro + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ri + "," +
                                                              pReqd + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.D + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Depth + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Fillet + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DimStart() + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LFlange + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FaceOff_Assy + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }                                   
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Thrust- Perform Data"

                            private int AddRec_Bearing_Thrust_TL_PerformData(clsProject Project_In)
                            //======================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition,fldPower_HP,fldFlowReqd_gpm,fldTempRise_F," +
                                                     "fldTFilm_Min,fldPadMax_Temp,fldPadMax_Press,fldUnitLoad)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_TB_PerformData";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ",'" + pPosition[i] + "'," +
                                                              +((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.Power_HP + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.FlowReqd_gpm + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TempRise_F + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TFilm_Min + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Temp + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Press + ","
                                                              + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.UnitLoad + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                        }
                                    }
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Thrust- Feed Groove"

                            private int AddRec_Bearing_Thrust_TL_FeedGroove(clsProject Project_In)
                            //====================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldType,fldWid,fldDepth,fldDBC,fldDist_Chamf)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_TB_FeedGroove";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";
                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "'," + pNo_Suffix + ",'" + pPosition[i] + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Type + "'," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Wid + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Depth + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.DBC + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Dist_Chamf + ")";
                                           
                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);                                                              
                                        }
                                    }                                  
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion


                        #region "Addition: End Config Thrust- Weep Slot"

                            private int AddRec_Bearing_Thrust_TL_WeepSlot(clsProject Project_In)
                            //====================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldType,fldWid,fldDepth)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_TB_WeepSlot";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";

                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Type + "'," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Wid + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Depth + ")";

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);
                                  
                                        }
                                    }                                 
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;
                            }

                        #endregion
         

                        #region "Addition: End Config Thrust- GCodes"

                            private int AddRec_Bearing_Thrust_TL_GCodes(clsProject Project_In)
                            //================================================================
                            {
                                int pCountRec = 0;

                                try
                                {
                                    //BG 04APR13
                                    //string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                    //                  "fldD_Desig_T1,fldD_Desig_T2,fldD_Desig_T3,fldD_Desig_T4," +
                                    //                  "fldOverlap_frac,fldFeed_Taperland,fldFeed_WeepSlot,fldDepth_Backlash," +
                                    //                  "fldDepth_Dwell_T,fldRMargin_WeepSlot,fldDepth_WeepSlot_Cut,fldStarting_LineNo,fldFilePath_Dir)";

                                    //BG 04APR13
                                    string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldPosition," +
                                                      "fldD_Desig_T1,fldD_Desig_T2,fldD_Desig_T3,fldD_Desig_T4," +
                                                      "fldOverlap_frac,fldFeed_Taperland,fldFeed_WeepSlot,fldDepth_Backlash," +
                                                      "fldDepth_Dwell_T,fldDepth_WeepSlot_Cut,fldFilePath_Dir)";

                                    string pINSERT = "INSERT INTO tblProject_EndConfig_TB_GCodes";
                                    string pVALUES = null;
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    string pNo_Suffix = "";
                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                    else
                                        pNo_Suffix = "NULL";

                                    string pSQL = "";

                                    for (int i = 0; i < pPosition.Length; i++)
                                    {
                                        if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + ", '" + pPosition[i] + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T1.D_Desig + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T2.D_Desig + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T3.D_Desig + "','" +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T4.D_Desig + "'," +           //BG 12DEC12
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Overlap_frac + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.Taperland + "," + 
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.WeepSlot + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Backlash + "," +
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Dwell_T + "," +
                                                              //((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).RMargin_WeepSlot + "," +        //BG 03APR13
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass + ",'" +    //BG 03APR13
                                                              //((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass + "," +   //BG 03APR13
                                                              //((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Starting_LineNo + ",'" +        //BG 03APR13
                                                              ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FilePath_Dir + "')" ;

                                            pSQL = pINSERT + pFIELDS + pVALUES;
                                            pCountRec = ExecuteCommand(pSQL);

                                        }
                                    }
                                }

                                catch (Exception pEXP)
                                {
                                    MessageBox.Show("Record insert error.");
                                    return pCountRec;
                                }

                                return pCountRec;

                            }

                       #endregion

                    #endregion


                    #region "Addition: Product Accessories"

                            private int AddRec_Project_Product_Accessories(clsProject Project_In)
                        //====================================================================
                        {
                            int pCountRec = 0;

                            try
                            {
                                string pFIELDS = "(fldProjectNo,fldProjectNo_Suffix,fldTempSensor_Supplied," +
                                                  "fldTempSensor_Name,fldTempSensor_Count,fldTempSensor_Type," +
                                                  "fldWireClip_Supplied, fldWireClip_Count,fldWireClip_Size)";

                                string pINSERT = "INSERT INTO tblProject_Product_Accessories";
                                string pVALUES = null;
                             
                                string pNo_Suffix = "";
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pNo_Suffix = "'" + Project_In.No_Suffix + "'";
                                else
                                    pNo_Suffix = "NULL";

                                int pTempSensor_Supplied = 0;
                                if (Project_In.Product.Accessories.TempSensor.Supplied) pTempSensor_Supplied = 1;

                                int pWireClip_Supplied = 0;
                                if (Project_In.Product.Accessories.WireClip.Supplied) pWireClip_Supplied = 1;

                                pVALUES = "VALUES ('" + Project_In.No + "', " + pNo_Suffix + "," + pTempSensor_Supplied + ",'" +
                                                    Project_In.Product.Accessories.TempSensor.Name + "'," +
                                                    Project_In.Product.Accessories.TempSensor.Count + ",'" +
                                                    Project_In.Product.Accessories.TempSensor.Type + "'," +
                                                    pWireClip_Supplied + "," +
                                                    Project_In.Product.Accessories.WireClip.Count + ",'" +
                                                    Project_In.Product.Accessories.WireClip.Size + "')";
                                                                
                                string pSQL = pINSERT + pFIELDS + pVALUES;
                                pCountRec = ExecuteCommand(pSQL);
                            }

                            catch (Exception pEXP)
                            {
                                MessageBox.Show("Record insert error.");
                                return pCountRec;
                            }

                            return pCountRec;
                        }

                    #endregion
         
                #endregion


                #region "Database Update Routine:"

                    public void UpdateRecord(clsProject Project_In, clsOpCond OpCond_In)
                    //==================================================================
                    {
                        UpdateRec_Project(Project_In);

                        if (!Does_EndConfigs_Match_ObjAndDB(Project_In))      
                        {
                            DeleteRecords_Table(Project_In, "tblProject_Product");
                            AddRec_Product(Project_In);

                            DeleteRecords_Table(Project_In, "tblProject_EndConfigs");
                            AddRec_EndConfigs(Project_In);

                            DeleteRecords_Table(Project_In, "tblProject_EndConfig_MountHoles");
                            AddRec_EndConfig_MountHoles(Project_In);

                            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            {
                                DeleteRecords_EndConfig_Seal_Detail(Project_In);
                                DeleteRecords_EndConfig_TB_Detail(Project_In);
                                AddRec_EndConfig_Seal_Detail(Project_In);
                            }

                            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            {
                                DeleteRecords_EndConfig_TB_Detail(Project_In);
                                DeleteRecords_EndConfig_Seal_Detail(Project_In);
                                AddRec_EndConfig_TB_Detail(Project_In);
                            }
                        }

                        else
                        {
                            UpdateRec_Product(Project_In);
                            UpdateRec_EndConfigs(Project_In);
                            UpdateRec_EndConfig_MountHoles(Project_In);

                            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            {
                                Update_EndConfig_Seal_Detail(Project_In);
                            }

                            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                                Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            {
                                Update_EndConfig_TB_Detail(Project_In);
                            }
                        }

                        UpdateRec_OpCond(Project_In, OpCond_In);
                        UpdateRec_Bearing_Radial(Project_In);
                        UpdateRec_Bearing_Radial_FP_Detail(Project_In);
                        Update_Project_Product_Accessories(Project_In);
                    }


                    private Boolean Does_EndConfigs_Match_ObjAndDB (clsProject Project_In)
                    //====================================================================            
                    {
                        Boolean pbln = false;

                        //SG 23JAN13
                        clsEndConfig.eType[] pType_DB = new clsEndConfig.eType[2];
                        Retrieve_EndConfigs_Type(Project_In, pType_DB);

                        if (Project_In.Product.EndConfig[0].Type.ToString() == pType_DB[0].ToString() &&
                            Project_In.Product.EndConfig[1].Type.ToString() == pType_DB[1].ToString())
                        {
                            pbln = true;

                        }
                       
                        //String pstrWHERE = "";

                        //String pstrAny = " AND fldBearing_Type = '" + Project_In.Product.Bearing.Type.ToString() + 
                        //                 "' AND fldEndConfig_Front_Type = '" + Project_In.Product.EndConfig[0].Type.ToString() + 
                        //                 "' AND fldEndConfig_Back_Type = '" + Project_In.Product.EndConfig[1].Type.ToString() + "'";

                        //if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                        //    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'" + pstrAny;


                        //else
                        //    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL" + pstrAny;
                                       
                        //String pSQL = "SELECT * FROM tblProject_Product" + pstrWHERE;

                        //SqlConnection pConnection = new SqlConnection();
                        //SqlDataReader pDR = modMain.gDB.GetDataReader(pSQL, ref pConnection);

                        //if (pDR.Read())
                        //{
                        //    pExists = true;
                        //}

                        return pbln;
                    }
                   

                    #region "Update: Project"

                        private void UpdateRec_Project(clsProject Project_In)
                        //=====================================================
                        {
                            string pEnggDate, pCheckedby_Date, pDesignedby_Date;

                            if (Project_In.Engg.Date.ToString("d", mInvCulture) != mstrDefDate)
                                pEnggDate = "'" + Project_In.Engg.Date.ToString("d", mInvCulture) + "'";
                            else
                                pEnggDate = "NULL";

                            if (Project_In.CheckedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
                                pCheckedby_Date = "'" + Project_In.CheckedBy.Date.ToString("d", mInvCulture) + "'";
                            else
                                pCheckedby_Date = "NULL";

                            if (Project_In.DesignedBy.Date.ToString("d", mInvCulture) != mstrDefDate)
                                pDesignedby_Date = "'" + Project_In.DesignedBy.Date.ToString("d", mInvCulture) + "'";
                            else
                                pDesignedby_Date = "NULL";


                            int pFileModified_RadialPart = 0, pFileModified_RadialBlankAssy = 0, pFileModified_EndTB_Part = 0;
                            int pFileModified_EndTB_Assy = 0, pFileModified_EndSeal_Part = 0, pFileModified_CompleteAssy = 0; 

                            if (Project_In.FileModified_RadialPart)
                                pFileModified_RadialPart = 1;
                            else
                                pFileModified_RadialPart = 0;

                            if (Project_In.FileModified_RadialBlankAssy)
                                pFileModified_RadialBlankAssy = 1;
                            else
                                pFileModified_RadialBlankAssy = 0;
                                                 
                            if (Project_In.FileModified_EndTB_Part)
                                pFileModified_EndTB_Part = 1;
                            else
                                pFileModified_EndTB_Part = 0;
                          
                            if (Project_In.FileModified_EndTB_Assy)
                                pFileModified_EndTB_Assy = 1;
                            else
                                pFileModified_EndTB_Assy = 0;
                           
                            if (Project_In.FileModified_EndSeal_Part)
                                pFileModified_EndSeal_Part = 1;
                            else
                                pFileModified_EndSeal_Part = 0;

                            if (Project_In.FileModified_CompleteAssy)
                                pFileModified_CompleteAssy = 1;
                            else
                                pFileModified_CompleteAssy = 0;

                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
 
                            pstrSET = " SET fldStatus = '" + Project_In.Status +
                                        "', fldCustomer_Name = '" + Project_In.Customer.Name +
                                        "', fldCustomer_MachineDesc = '" + Project_In.Customer.MachineDesc +
                                        "', fldCustomer_PartNo = '" + Project_In.Customer.PartNo +
                                        "', fldUnitSystem = '" + Project_In.Unit.System.ToString() +
                                        "', fldAssyDwg_No = '" + Project_In.AssyDwg.No +
                                        "', fldAssyDwg_No_Suffix = '" + Project_In.AssyDwg.No_Suffix +
                                        "', fldAssyDwg_Ref = '" + Project_In.AssyDwg.Ref +
                                        "', fldEngg_Name = '" + Project_In.Engg.Name +
                                        "', fldEngg_Initials = '" + Project_In.Engg.Initials +
                                        "', fldEngg_Date = " + pEnggDate +
                                        ",  fldDesignedBy_Name = '" + Project_In.DesignedBy.Name +
                                        "', fldDesignedBy_Initials = '" + Project_In.DesignedBy.Initials +
                                        "', fldDesignedBy_Date = " + pDesignedby_Date +
                                        ",  fldCheckedby_Name = '" + Project_In.CheckedBy.Name +
                                        "', fldCheckedby_Initials = '" + Project_In.CheckedBy.Initials +
                                        "', fldCheckedby_Date = " + pCheckedby_Date + 
                                        ", fldFilePath_Project = '" + modMain.gProject.FilePath_Project +
                                        "', fldFilePath_DesignTbls_SWFiles = '" + modMain.gProject.FilePath_DesignTbls_SWFiles +
                                        "', fldFileModified_CompleteAssy = " + pFileModified_CompleteAssy +             //BG 05DEC12
                                        ", fldFileModified_Radial_Part = " + pFileModified_RadialPart +
                                        ", fldFileModified_Radial_BlankAssy = " + pFileModified_RadialBlankAssy +
                                        ", fldFileModified_EndTB_Part = " + pFileModified_EndTB_Part +
                                        ", fldFileModified_EndTB_Assy = " + pFileModified_EndTB_Assy +
                                        ", fldFileModified_EndSeal_Part = " + pFileModified_EndSeal_Part +
                                        //", fldFileModified_CompleteAssy = " + pFileModified_CompleteAssy +        //BG 05DEC12
                                        ", fldFileModification_Notes = '" + modMain.gProject.FileModification_Notes + "'";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix is NULL";

                            pstrActionSQL = "UPDATE tblProject_Details" + pstrSET + pstrWHERE;
                            pCountRecords = ExecuteCommand(pstrActionSQL);
                        }

                    #endregion


                    #region "Update: Product"

                        private void UpdateRec_Product(clsProject Project_In)
                        //==================================================
                        {                           
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
                   
                            pstrSET = " SET fldBearing_Type = '" + Project_In.Product.Bearing.Type.ToString() +
                                //"', fldEndConfig_Front_Type = '" + Project_In.Product.EndConfig[0].Type.ToString() +      //SG 23JAN13
                                        //"', fldEndConfig_Back_Type = '" + Project_In.Product.EndConfig[1].Type.ToString() +
                                        "', fldL_Available = " + Project_In.Product.L_Available +
                                        ", fldDist_ThrustFace_Front = " + Project_In.Product.Dist_ThrustFace[0] +
                                        ", fldDist_ThrustFace_Back = " + Project_In.Product.Dist_ThrustFace[1];

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrActionSQL = "UPDATE tblProject_Product" + pstrSET + pstrWHERE;
                            pCountRecords = ExecuteCommand(pstrActionSQL);
                        }

                    #endregion


                    #region "Update: Operating Condition"

                        private void UpdateRec_OpCond(clsProject Project_In, clsOpCond OpCond_In)    
                        //=======================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET, pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            pstrSET = " SET fldSpeed_Range_Min = " + OpCond_In.Speed_Range[0] +
                                     ", fldSpeed_Range_Max = " + OpCond_In.Speed_Range[1] +
                                     ", fldRadial_Load_Range_Min = " + OpCond_In.Radial_Load_Range[0] +
                                     ", fldRadial_Load_Range_Max = " + OpCond_In.Radial_Load_Range[1] +
                                     ", fldRadial_LoadAng = " + OpCond_In.Radial_LoadAng +
                                     ", fldThrust_Load_Range_Front_Min = " + OpCond_In.Thrust_Load_Range_Front[0] +
                                     ", fldThrust_Load_Range_Front_Max = " + OpCond_In.Thrust_Load_Range_Front[1] +
                                     ", fldThrust_Load_Range_Back_Min = " + OpCond_In.Thrust_Load_Range_Back[0] +
                                     ", fldThrust_Load_Range_Back_Max = " + OpCond_In.Thrust_Load_Range_Back[1] +
                                     ", fldOilSupply_Lube_Type = '" + OpCond_In.OilSupply.Lube.Type +
                                     "', fldOilSupply_Type = '" + OpCond_In.OilSupply.Type +
                                     "', fldOilSupply_Press = " + OpCond_In.OilSupply.Press +
                                     ", fldOilSupply_Temp = " + OpCond_In.OilSupply.Temp +
                                     ", fldRot_Directionality = '" + OpCond_In.Rot_Directionality.ToString() + "'";


                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrActionSQL = "UPDATE tblProject_OpCond" + pstrSET + pstrWHERE;
                            pCountRecords = ExecuteCommand(pstrActionSQL);
                        }

                    #endregion


                    #region "Update: Bearing Radial"

                        private void UpdateRec_Bearing_Radial(clsProject Project_In)
                        //==========================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            pstrSET = " SET fldDesign = '" + ((clsBearing_Radial)Project_In.Product.Bearing).Design.ToString() + "'";

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                            pstrActionSQL = "UPDATE tblProject_Bearing_Radial" + pstrSET + pstrWHERE;
                            pCountRecords = ExecuteCommand(pstrActionSQL);
                        }

                    #endregion


                    #region "Update: Bearing Radial FP Detail"

                        private void UpdateRec_Bearing_Radial_FP_Detail(clsProject Project_In)
                        //=====================================================================
                        {
                            UpdateRec_Bearing_Radial_FP(Project_In);
                            UpdateRec_Bearing_Radial_FP_PerformData(Project_In);
                            UpdateRec_Bearing_Radial_FP_Pad(Project_In);
                            UpdateRec_Bearing_Radial_FP_FlexurePivot(Project_In);
                            UpdateRec_Bearing_Radial_FP_OilInlet(Project_In);
                            UpdateRec_Bearing_Radial_FP_MillRelief(Project_In);
                            UpdateRec_Bearing_Radial_FP_Flange(Project_In);
                            UpdateRec_Bearing_Radial_FP_SL(Project_In);
                            UpdateRec_Bearing_Radial_FP_AntiRotPin(Project_In);
                            UpdateRec_Bearing_Radial_FP_Mount(Project_In);
                            //UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);       //BG 28FEB13
                            CheckAndUpdate_Bearing_Radial_FP_Mount_Fixture(Project_In);    //BG 28FEB13
                            UpdateRec_Bearing_Radial_FP_TempSensor(Project_In);
                            UpdateRec_Bearing_Radial_FP_EDM_Pad(Project_In);      
                        } 

                        #region "Update: Bearing Radial FP"

                            public void UpdateRec_Bearing_Radial_FP(clsProject Project_In)
                            //===============================================================        
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords, pSplitConfig = 0;

                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                    pSplitConfig = 1;
                                else
                                    pSplitConfig = 0;

                                int pMatLiningExists = 0;
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.LiningExists) pMatLiningExists = 1;                                                             

                                pstrSET = " SET fldSplitConfig = " + pSplitConfig +
                                            ", fldDShaft_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] +
                                            ", fldDShaft_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] +
                                            ", fldDFit_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0] +
                                            ", fldDFit_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1] +
                                            ", fldDSet_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] +
                                            ", fldDSet_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1] +
                                            ", fldDPad_Range_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0] +
                                            ", fldDPad_Range_Max = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1] +
                                            ", fldL = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).L +
                                            //", fldEDMRelief_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[0] +
                                            //", fldEDMRelief_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[1] +
                                            ", fldDepth_EndConfig_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0] +
                                            ", fldDepth_EndConfig_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1] +
                                            ", fldDimStart_FrontFace = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace +
                                            ", fldMat_Base = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base +
                                            "',fldMat_LiningExists = " + pMatLiningExists +
                                            ", fldMat_Lining = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining +
                                            "',fldLiningT = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Perform Data"

                            public void UpdateRec_Bearing_Radial_FP_PerformData(clsProject Project_In)
                            //=========================================================================       
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldPower_HP = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP +
                                          ", fldFlowReqd_gpm = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm +
                                          ", fldTempRise_F = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F +
                                          ", fldTFilm_Min = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min +
                                          ", fldPadMax_Temp = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp +
                                          ", fldPadMax_Press = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press +
                                          ", fldPadMax_Rot = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot +
                                          ", fldPadMax_Load = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load +
                                          ", fldPadMax_Stress = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_PerformData" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Pad"

                            public void UpdateRec_Bearing_Radial_FP_Pad(clsProject Project_In)
                            //================================================================       
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldType = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString() +
                                          "', fldCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count +
                                          ", fldL = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L +
                                          ", fldPivot_Offset = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset +
                                          ", fldPivot_AngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart +
                                          ", fldT_Lead = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead +
                                          ", fldT_Pivot = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot +
                                          ", fldT_Trail = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail;


                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Pad" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Flexure Pivot"

                            public void UpdateRec_Bearing_Radial_FP_FlexurePivot(clsProject Project_In)
                            //=========================================================================       
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldWeb_T = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T +
                                          ", fldWeb_RFillet = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet +
                                          ", fldWeb_H = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H +
                                          ", fldGapEDM = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM +
                                          ", fldRot_Stiff = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_FlexurePivot" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Oil Inlet"

                            public void UpdateRec_Bearing_Radial_FP_OilInlet(clsProject Project_In)
                            //=====================================================================       
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords, pExists = 0;

                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
                                    pExists = 1;
                                else
                                    pExists = 0;

                                pstrSET = " SET fldOrifice_Count = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count +
                                          ", fldOrifice_D = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D +
                                          ", fldCount_MainOilSupply = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply +
                                          ", fldOrifice_StartPos = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos +
                                          "', fldOrifice_DDrill_CBore = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore +
                                          ", fldOrifice_Loc_FrontFace = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace +
                                          ", fldOrifice_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L +
                                          ", fldOrifice_AngStart_BDV = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV +
                                          ", fldAnnulus_Exists = " + pExists +
                                          ", fldAnnulus_D = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D +
                                          ", fldAnnulus_Loc_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back +
                                          ", fldAnnulus_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L +
                                          ", fldOrifice_Dist_Holes = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_OilInlet" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Mill Relief"

                            public void UpdateRec_Bearing_Radial_FP_MillRelief(clsProject Project_In)
                            //=======================================================================       
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                int pExists = 0;
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists) pExists = 1;

                                pstrSET = " SET fldExists = " + pExists +
                                          ", fldD_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_Desig + "'" +
                                          ", fldEDMRelief_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0] +
                                          ", fldEDMRelief_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1];

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_MillRelief" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Flange"

                            public void UpdateRec_Bearing_Radial_FP_Flange(clsProject Project_In)
                            //====================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                int pExists = 0;
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists) pExists = 1;

                                pstrSET = " SET fldExists = " + pExists +
                                          ", fldD = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D + "'" +
                                          ", fldWid = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid + "'" +
                                          ", fldDimStart_Front = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front + "'";

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Flange" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - SL"

                            public void UpdateRec_Bearing_Radial_FP_SL(clsProject Project_In)
                            //================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldScrew_Spec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString() +        //BG 26MAR12
                                          "', fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type +
                                          "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + 
                                          "', fldScrew_Spec_Pitch = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch + 
                                          ", fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L +
                                          ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat +
                                          "', fldLScrew_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center +
                                          ", fldLScrew_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front + 
                                          ", fldRScrew_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center +
                                          ", fldRScrew_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front + 
                                          ", fldThread_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth + 
                                          ", fldCBore_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth +
                                          ", fldDowel_Spec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString() +       //BG 26MAR12
                                          "', fldDowel_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type +
                                          "', fldDowel_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig +
                                          "', fldDowel_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L  +
                                          ", fldDowel_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat +
                                          "', fldLDowel_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center + 
                                          ", fldLDowel_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front + 
                                          ", fldRDowel_Spec_Loc_Center = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center +
                                          ", fldRDowel_Spec_Loc_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front +
                                          ", fldDowel_Depth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_SL" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - AntiRotPin"

                            public void UpdateRec_Bearing_Radial_FP_AntiRotPin(clsProject Project_In)
                            //=======================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldLoc_Dist_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front +
                                          ", fldLoc_Casing_SL = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL.ToString() +
                                          "', fldLoc_Offset = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset +
                                          ", fldLoc_Bearing_Vert = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString() +
                                          "', fldLoc_Bearing_SL = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL.ToString() +
                                          "', fldLoc_Angle = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle +
                                          ", fldSpec_UnitSystem = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System.ToString() +           //BG 26MAR12
                                          "', fldSpec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type +
                                          "', fldSpec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig +
                                          "', fldSpec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L +
                                          ", fldSpec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat +
                                          "', fldDepth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth +
                                          ", fldStickOut = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout;


                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_AntiRotPin" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Mount"

                            public void UpdateRec_Bearing_Radial_FP_Mount(clsProject Project_In)
                            //===================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                int pHolesGoThru = 0;
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru) pHolesGoThru = 1;                              
                               

                                int pFixture_Candidate_Chosen_Front = 0; int pFixture_Candidate_Chosen_Back = 0;

                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[0]) pFixture_Candidate_Chosen_Front = 1;
                                
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture_Candidates_Chosen[1]) pFixture_Candidate_Chosen_Back = 1;

                                pstrSET = " SET fldHoles_GoThru = " + pHolesGoThru +
                                          ", fldHoles_Bolting = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting +
                                          "',fldFixture_Candidates_Chosen_Front = " + pFixture_Candidate_Chosen_Front +
                                          ", fldFixture_Candidates_Chosen_Back = " + pFixture_Candidate_Chosen_Back +
                                          ", fldHoles_Thread_Depth_Front = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] +
                                          ", fldHoles_Thread_Depth_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1];

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion


                        #region "Update: Bearing Radial FP - Mount Fixture"

                            public void CheckAndUpdate_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In)
                            //===============================================================================   'BG 20MAR13      
                            {
                                StringCollection pBolting = new StringCollection();
                                pBolting = Retrieve_Bearing_Radial_FP_Mount_Fixture_Bolting_Pos(Project_In);

                                switch (pBolting.Count)
                                {
                                    case 1:
                                    //----- 

                                        //....Bolting = Front or Bolting = Back, when user changes Bolting = Both
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            if (pBolting[0] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
                                            {
                                                UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                                            }
                                            else
                                            {
                                                DeleteRecords_Table(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
                                                AddRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                                            }
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                                        {
                                            DeleteRecords_Table(Project_In, "tblProject_Bearing_Radial_FP_Mount_Fixture");
                                            AddRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                                        }
                                        break;

                                    case 2:
                                    //-----

                                        //....Bolting = Both, When user changes Bolting = Front or  Bolting = Back
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front ||
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            for (int i = 0; i < pBolting.Count; i++)
                                            {
                                                if (pBolting[i] == ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting.ToString())
                                                {
                                                    UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                                                }
                                                else
                                                {
                                                    DeleteRecord_Bearing_Radial_FP_Mount_Fixture(Project_In, pBolting[i]);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            UpdateRec_Bearing_Radial_FP_Mount_Fixture(Project_In);
                                        }
                                        break;
                                }
                            }                            

                            public void UpdateRec_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In)
                            //===========================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting + "'";

                                int pHolesEquispaced = 0, pHolesAngStart_Comp_Chosen = 0;

                                //....Front
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                {
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced) pHolesEquispaced = 1;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                    pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo +
                                                "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC +
                                                ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].D_Finish +
                                                ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount +
                                                ", fldHolesEquispaced = " + pHolesEquispaced +
                                                ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart +
                                                ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
                                                ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[0] +
                                                ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[1] +
                                                ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[2] +
                                                ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[3] +
                                                ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[4] +
                                                ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[5] +
                                                ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[6] +
                                                ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type +
                                                "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig +
                                                "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.L +
                                                ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat + "'";

                                    pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }

                                //....Back
                                else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back )
                                {
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced) pHolesEquispaced = 1;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                    pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo +
                                                "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC +
                                                ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].D_Finish +
                                                ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount +
                                                ", fldHolesEquispaced = " + pHolesEquispaced +
                                                ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart +
                                                ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
                                                ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[0] +
                                                ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[1] +
                                                ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[2] +
                                                ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[3] +
                                                ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[4] +
                                                ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[5] +
                                                ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[6] +
                                                ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Type +
                                                "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig +
                                                "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.L +
                                                ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.Mat + "'";

                                    pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                                
                                //....Both
                                else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                                {
                                    string[] pPosition = new string[] { "Front", "Back" };

                                    for (int i = 0; i < 2; i++)
                                    {
                                        pHolesEquispaced = 0; pHolesAngStart_Comp_Chosen = 0;
                                        if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                        else
                                            pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                        pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesEquiSpaced) pHolesEquispaced = 1;
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngStart_Comp_Chosen) pHolesAngStart_Comp_Chosen = 1;

                                        pstrSET = " SET fldPartNo = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].PartNo +
                                                  "', fldDBC = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].DBC +
                                                  ", fldD_Finish = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].D_Finish +
                                                  ", fldHolesCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesCount +
                                                  ", fldHolesEquispaced = " + pHolesEquispaced +
                                                  ", fldHolesAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngStart +
                                                  ", fldHolesAngStart_Comp_Chosen = " + pHolesAngStart_Comp_Chosen +
                                                  ", fldHolesAngOther1 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[0] +
                                                  ", fldHolesAngOther2 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[1] +
                                                  ", fldHolesAngOther3 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[2] +
                                                  ", fldHolesAngOther4 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[3] +
                                                  ", fldHolesAngOther5 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[4] +
                                                  ", fldHolesAngOther6 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[5] +
                                                  ", fldHolesAngOther7 = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].HolesAngOther[6] +
                                                  ", fldScrew_Spec_Type = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.Type +
                                                  "', fldScrew_Spec_D_Desig = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.D_Desig +
                                                  "', fldScrew_Spec_L = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.L +
                                                  ", fldScrew_Spec_Mat = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[i].Screw_Spec.Mat + "'";

                                        pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrSET + pstrWHERE;
                                        pCountRecords = ExecuteCommand(pstrActionSQL);
                                    }
                                }
                            }

                            
                           
                        #endregion


                        #region "Update: Bearing Radial FP - Temp Sensor"

                            public void UpdateRec_Bearing_Radial_FP_TempSensor(clsProject Project_In)
                            //========================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                int pExists = 0;
                                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists) pExists = 1;

                                pstrSET = " SET fldExists = " + pExists +
                                          ", fldCanLength = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength +
                                          ", fldCount = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count +
                                          ", fldLoc = '" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc +
                                          "', fldD = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D +
                                          ", fldDepth = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth +
                                          ", fldAngStart = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_TempSensor" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                    #endregion


                        #region "Update: Bearing Radial FP - EDM Pad"

                            public void UpdateRec_Bearing_Radial_FP_EDM_Pad(clsProject Project_In)
                            //====================================================================      
                            {
                                //....UPDATE Records.
                                String pstrSET = "", pstrWHERE, pstrActionSQL;
                                int pCountRecords;

                                pstrSET = " SET fldRFillet_Back = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back +
                                          ", fldAngStart_Web = " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web;

                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrActionSQL = "UPDATE tblProject_Bearing_Radial_FP_EDM_Pad" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }

                        #endregion

                #endregion


                    #region "Update: End Configs"

                        private void UpdateRec_EndConfigs(clsProject Project_In)
                        //=======================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
                            string[] pPosition = new string[] { "Front", "Back" };  

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                pstrSET = " SET fldType = '" + Project_In.Product.EndConfig[i].Type +
                                            "', fldMat_Base = '" + Project_In.Product.EndConfig[i].Mat.Base +
                                            "', fldMat_Lining = '" + Project_In.Product.EndConfig[i].Mat.Lining +
                                            "', fldDO = " + Project_In.Product.EndConfig[i].DO +
                                            ",  fldDBore_Range_Min = " + Project_In.Product.EndConfig[i].DBore_Range[0] +
                                            ",  fldDBore_Range_Max = " + Project_In.Product.EndConfig[i].DBore_Range[1] +
                                            ",  fldL = " + Project_In.Product.EndConfig[i].L;
                                         
                                pstrActionSQL = "UPDATE tblProject_EndConfigs" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }                                                             
                           
                        }
                    
                    #endregion


                    #region "Update: End Config - Mount Holes"

                        private void UpdateRec_EndConfig_MountHoles(clsProject Project_In)
                        //=================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
                            string[] pPosition = new string[] { "Front", "Back" };
                         
                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                int pThread_Thru = 0; 
                                if (Project_In.Product.EndConfig[i].MountHoles.Thread_Thru) pThread_Thru = 1;

                                pstrSET = " SET fldType = '" + Project_In.Product.EndConfig[i].MountHoles.Type.ToString() +
                                            "', fldDepth_CBore = " + Project_In.Product.EndConfig[i].MountHoles.Depth_CBore +
                                            ",  fldThread_Thru = " + pThread_Thru + 
                                            ",  fldDepth_Thread = " + Project_In.Product.EndConfig[i].MountHoles.Depth_Thread;

                                pstrActionSQL = "UPDATE tblProject_EndConfig_MountHoles" + pstrSET + pstrWHERE;
                                pCountRecords = ExecuteCommand(pstrActionSQL);
                            }
                        }

                    #endregion


                    #region "Update: End Config - Seal Details"

                    private void Update_EndConfig_Seal_Detail(clsProject Project_In)
                    //==============================================================
                    {
                        Update_EndConfig_Seal(Project_In);
                        Update_EndConfig_Seal_Blade(Project_In);
                        Update_EndConfig_Seal_DrainHoles(Project_In);
                        Update_EndConfig_Seal_WireClipHoles(Project_In);
                    }

                    #region "Update: End Config - Seal"

                        private void Update_EndConfig_Seal(clsProject Project_In)
                        //========================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;                       

                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                {
                                    //SG 13DEC12
                                    pstrSET = " SET fldType = '" + ((clsSeal)Project_In.Product.EndConfig[i]).Design.ToString() +
                                                "', fldLiningT = " + ((clsSeal)Project_In.Product.EndConfig[i]).Mat_LiningT +
                                                ", fldTempSensor_D_ExitHole = " + ((clsSeal)Project_In.Product.EndConfig[i]).TempSensor_D_ExitHole;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }                                                             
                        }
                        
                    #endregion

                    #region "Update: End Config Seal - Blade"

                        private void Update_EndConfig_Seal_Blade(clsProject Project_In)
                        //=============================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
               
                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                {
                                    pstrSET = " SET fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.Count +
                                                ", fldT = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.T +
                                                ", fldAngTaper = " + ((clsSeal)Project_In.Product.EndConfig[i]).Blade.AngTaper;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_Blade" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion

                    #region "Update: End Config Seal - Drain Holes"

                        private void Update_EndConfig_Seal_DrainHoles(clsProject Project_In)
                        //==================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;
                                                          
                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                {
                                    pstrSET = " SET fldAnnulus_Ratio_L_H = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.Ratio_L_H +
                                                ", fldAnnulus_D = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Annulus.D +
                                                ", fldD_Desig = '" + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.D_Desig +
                                                "', fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.Count +
                                                ", fldAngStart = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngStart +
                                                ", fldAngBet = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngBet +
                                                ", fldAngExit = " + ((clsSeal)Project_In.Product.EndConfig[i]).DrainHoles.AngExit;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_DrainHoles" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion

                    #region "Update: End Config Seal - Wire Clip Holes"

                        private void Update_EndConfig_Seal_WireClipHoles(clsProject Project_In)
                        //======================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };

                                
                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                int pExists = 0;
                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                                {
                                    if (((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Exists) pExists = 1;

                                    pstrSET = " SET fldExists = " + pExists +
                                                ", fldCount = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Count +
                                                ", fldDBC = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.DBC +
                                                ", fldUnitSystem = '" + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Unit.System.ToString() +           //BG 03JUL13
                                                "', fldThread_Dia_Desig = '" + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.D_Desig +
                                                "', fldThread_Pitch = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.Screw_Spec.Pitch +
                                                ", fldThread_Depth = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.ThreadDepth +
                                                ", fldAngStart = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngStart +
                                                ", fldAngOther1 = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[0] +
                                                ", fldAngOther2 = " + ((clsSeal)Project_In.Product.EndConfig[i]).WireClipHoles.AngOther[1] ;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_Seal_WireClipHoles" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion

                #endregion


                    #region "Update: End Config - TB Details"

                    private void Update_EndConfig_TB_Detail(clsProject Project_In)
                    //==============================================================
                    {
                        Update_EndConfig_Thrust_TL(Project_In);
                        Update_EndConfig_Thrust_TL_PerformData(Project_In);
                        Update_EndConfig_Thrust_TL_FeedGroove(Project_In);
                        Update_EndConfig_Thrust_TL_WeepSlot(Project_In);
                        Update_EndConfig_Thrust_TL_GCodes(Project_In); 
                    }

                    #region "Update: End Config - Thrust_TL"

                        private void Update_EndConfig_Thrust_TL(clsProject Project_In)
                        //============================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };
                                
                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                int pReqd = 0;

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Reqd) pReqd = 1;

                                    pstrSET = " SET fldDirectionType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DirectionType.ToString() +
                                                "', fldPad_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[0] +
                                                ", fldPad_OD = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PadD[1] +
                                                ", fldLandL = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LandL +
                                                ", fldLiningT_Face = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.Face +
                                                ", fldLiningT_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LiningT.ID +
                                                ", fldPad_Count = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Pad_Count +
                                                ", fldTaper_Depth_OD = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_OD +
                                                ", fldTaper_Depth_ID = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Depth_ID +
                                                ", fldTaper_Angle = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Taper.Angle +
                                                ", fldShroud_Ro = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ro +
                                                ", fldShroud_Ri = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Shroud.Ri +
                                                ", fldBackRelief_Reqd = " + pReqd +
                                                ", fldBackRelief_D = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.D +
                                                ", fldBackRelief_Depth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Depth +
                                                ", fldBackRelief_Fillet = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).BackRelief.Fillet +
                                                ", fldDimStart = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).DimStart() +
                                                ", fldLFlange = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).LFlange +
                                                ", fldFaceOff_Assy = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FaceOff_Assy;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion


                    #region "Update: End Config Thrust - Perform Data"

                        private void Update_EndConfig_Thrust_TL_PerformData(clsProject Project_In)
                        //========================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                       
                                    pstrSET = " SET fldPower_HP = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.Power_HP +
                                        ", fldFlowReqd_gpm = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.FlowReqd_gpm +
                                        ", fldTempRise_F = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TempRise_F +
                                        ", fldTFilm_Min = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.TFilm_Min +
                                        ", fldPadMax_Temp = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Temp +
                                        ", fldPadMax_Press = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.PadMax.Press +
                                        ", fldUnitLoad = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).PerformData.UnitLoad;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_PerformData" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion


                    #region "Update: End Config Thrust - Feed Groove"

                        private void Update_EndConfig_Thrust_TL_FeedGroove(clsProject Project_In)
                        //========================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    pstrSET = " SET fldType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Type +
                                                "', fldWid = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Wid +
                                                ", fldDepth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Depth +
                                                ", fldDBC = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.DBC +
                                                ", fldDist_Chamf = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedGroove.Dist_Chamf;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_FeedGroove" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion


                    #region "Update: End Config Thrust - Weep Slot"

                        private void Update_EndConfig_Thrust_TL_WeepSlot(clsProject Project_In)
                        //======================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    pstrSET = " SET fldType = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Type +
                                                "', fldWid = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Wid +
                                                ", fldDepth = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).WeepSlot.Depth;

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_WeepSlot" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion


                    #region "Update: End Config Thrust - GCodes"

                        private void Update_EndConfig_Thrust_TL_GCodes(clsProject Project_In)
                        //======================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            string[] pPosition = new string[] { "Front", "Back" };

                            for (int i = 0; i < pPosition.Length; i++)
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";

                                pstrWHERE = pstrWHERE + " AND fldPosition = '" + pPosition[i] + "'";

                                if (Project_In.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {                                                   

                                    pstrSET = " SET fldD_Desig_T1 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T1.D_Desig +
                                                "', fldD_Desig_T2 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T2.D_Desig +
                                                "', fldD_Desig_T3 = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T3.D_Desig +
                                                "', fldD_Desig_T4 = '" +  ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).T4.D_Desig +
                                                "', fldOverlap_frac = " +  ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Overlap_frac +
                                                ", fldFeed_Taperland = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.Taperland +
                                                ", fldFeed_WeepSlot = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FeedRate.WeepSlot +
                                                ", fldDepth_Backlash = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Backlash +
                                                ", fldDepth_Dwell_T = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_TL_Dwell_T +
                                                //", fldRMargin_WeepSlot = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).RMargin_WeepSlot +       //BG 03APR13
                                                ", fldDepth_WeepSlot_Cut = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Depth_WS_Cut_Per_Pass +
                                                //", fldStarting_LineNo = " + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).Starting_LineNo + //BG 03APR13
                                                ", fldFilePath_Dir = '" + ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[i]).FilePath_Dir + "'";

                                    pstrActionSQL = "UPDATE tblProject_EndConfig_TB_GCodes" + pstrSET + pstrWHERE;
                                    pCountRecords = ExecuteCommand(pstrActionSQL);
                                }
                            }
                        }

                    #endregion

                #endregion


                    #region "Update: Product Accessories"

                        private int Update_Project_Product_Accessories(clsProject Project_In)
                        //====================================================================
                        {
                            //....UPDATE Records.
                            String pstrSET = "", pstrWHERE, pstrActionSQL;
                            int pCountRecords;

                            if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                            else
                                pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";
                                                     
                            int pTempSensor_Supplied = 0;
                            if (Project_In.Product.Accessories.TempSensor.Supplied) pTempSensor_Supplied = 1;

                            int pWireClip_Supplied = 0;
                            if (Project_In.Product.Accessories.WireClip.Supplied) pWireClip_Supplied = 1;

                            pstrSET = " SET fldTempSensor_Supplied = " + pTempSensor_Supplied + 
                                        ", fldTempSensor_Name = '" + Project_In.Product.Accessories.TempSensor.Name +
                                        "', fldTempSensor_Count = " + Project_In.Product.Accessories.TempSensor.Count +
                                        ", fldTempSensor_Type = '" + Project_In.Product.Accessories.TempSensor.Type +
                                        "', fldWireClip_Supplied = " + pWireClip_Supplied +
                                        ", fldWireClip_Count = " + Project_In.Product.Accessories.WireClip.Count +
                                        ", fldWireClip_Size = '" + Project_In.Product.Accessories.WireClip.Size + "'";                                        

                            pstrActionSQL = "UPDATE tblProject_Product_Accessories" + pstrSET + pstrWHERE;
                            pCountRecords = ExecuteCommand(pstrActionSQL);
                            return pCountRecords;                           
                        }

                    #endregion


                #endregion


                #region "Database Deletion Routine:"

                    public void DeleteRecord(clsProject Project_In)
                    //=============================================
                    {                        
                        DeleteRecords_Table(Project_In, "tblProject_Details");
                        DeleteRecords_Table(Project_In, "tblProject_Product");
                        DeleteRecords_Table(Project_In, "tblProject_OpCond");
                        DeleteRecords_Table(Project_In, "tblProject_Bearing_Radial");
                        DeleteRecords_Bearing_Radial_FP_Detail(Project_In);
                        DeleteRecords_Table(Project_In, "tblProject_EndConfigs");
                        DeleteRecords_Table(Project_In, "tblProject_EndConfig_MountHoles");
                        DeleteRecords_EndConfig_Seal_Detail(Project_In);
                        DeleteRecords_EndConfig_TB_Detail(Project_In);
                        DeleteRecords_Table(Project_In, "tblProject_Product_Accessories");
                    }

                    #region "Deletion From Different Tables:"

                        private void DeleteRecords_Table(clsProject Project_In, string TableName_In)
                        //=========================================================================
                        {
                            string pstrWHERE = "";
                            string pSQL = "";

                            if (TableName_In == "tblProject_Details")
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldNo = '" + Project_In.No + "' AND fldNo_Suffix is NULL";
                            }                           
                            else
                            {
                                if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix + "'";
                                else
                                    pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL";
                            }

                            pSQL = "DELETE FROM " + TableName_In + pstrWHERE;
                            ExecuteCommand(pSQL);
                        }


                        #region "Delete: Bearing Radial FP Detail"

                            #region "Delete: Bearing Radial FP Mount_Fixture"

                                private void DeleteRecord_Bearing_Radial_FP_Mount_Fixture(clsProject Project_In, string Position_In)
                                //================================================================================================== BG 28FEB13
                                {
                                    string pstrWHERE = "";
                                    string pSQL = "";

                                    if (Project_In.No_Suffix != "" && Project_In.No_Suffix != null)
                                        pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No +
                                                    "' AND fldProjectNo_Suffix = '" + Project_In.No_Suffix +
                                                    "' AND fldPosition = '" + Position_In + "'";
                                    else
                                        pstrWHERE = " WHERE fldProjectNo = '" + Project_In.No + "' AND fldProjectNo_Suffix is NULL AND fldPosition = '" + Position_In + "'";


                                    pSQL = "DELETE FROM  tblProject_Bearing_Radial_FP_Mount_Fixture" + pstrWHERE;
                                    ExecuteCommand(pSQL);
                                }

                            #endregion

                            private void DeleteRecords_Bearing_Radial_FP_Detail(clsProject Project_In)
                            //=========================================================================
                            {
                                string[] pTblName = new string[]{"tblProject_Bearing_Radial_FP", "tblProject_Bearing_Radial_FP_PerformData",
                                                                 "tblProject_Bearing_Radial_FP_Pad", "tblProject_Bearing_Radial_FP_FlexurePivot",
                                                                 "tblProject_Bearing_Radial_FP_OilInlet", "tblProject_Bearing_Radial_FP_MillRelief",
                                                                 "tblProject_Bearing_Radial_FP_Flange", "tblProject_Bearing_Radial_FP_SL",
                                                                 "tblProject_Bearing_Radial_FP_AntiRotPin", "tblProject_Bearing_Radial_FP_Mount",
                                                                 "tblProject_Bearing_Radial_FP_Mount_Fixture", "tblProject_Bearing_Radial_FP_TempSensor",
                                                                 "tblProject_Bearing_Radial_FP_EDM_Pad"};

                                for (int i = 0; i < pTblName.Length; i++)
                                {
                                    DeleteRecords_Table(Project_In, pTblName[i]);
                                }
                            }

                        #endregion


                       

                        #region "Delete: End Config Seal Details"

                            private void DeleteRecords_EndConfig_Seal_Detail(clsProject Project_In)
                            //=====================================================================
                            {
                                string[] pTblName = new string[]{"tblProject_EndConfig_Seal", "tblProject_EndConfig_Seal_Blade",
                                                                    "tblProject_EndConfig_Seal_DrainHoles", "tblProject_EndConfig_Seal_WireClipHoles"};

                                for (int i = 0; i < pTblName.Length; i++)
                                {
                                    DeleteRecords_Table(Project_In, pTblName[i]);
                                }
                            }

                        #endregion


                        #region "Delete: End Config TB Details"

                            private void DeleteRecords_EndConfig_TB_Detail(clsProject Project_In)
                            //===================================================================
                            {
                                string[] pTblName = new string[]{"tblProject_EndConfig_TB",  "tblProject_EndConfig_TB_PerformData", 
                                                                 "tblProject_EndConfig_TB_FeedGroove", "tblProject_EndConfig_TB_WeepSlot",
                                                                 "tblProject_EndConfig_TB_GCodes"};

                                for (int i = 0; i < pTblName.Length; i++)
                                {
                                    DeleteRecords_Table(Project_In, pTblName[i]);
                                }
                            }

                        #endregion

                    #endregion

                #endregion
        
            #endregion


            #region "Populate Combo & List Boxes, String Collections:"
            //--------------------------------------------------------

                public int PopulateCmbBox(ComboBox cmbBox_In, 
                                            string strTableName_In, string strFldName_In,
                                            string strWHERE_In, bool blnOrderBy_In)
                //============================================================================   
                {
                    //....This utility function populates a comboBox and 
                    //......returns the # of list items, if any.

                    //This routine populate comboboxes
                    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                    //       Output  Parameters      :   No of Records

                    //....Create the SQL string.   
                    //

                    string pstrORDERBY = "";
                    if (blnOrderBy_In == true)
                        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                    string pstrSQL = "";

                    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                    //....Get the corresponding data reader object.
                    SqlDataReader pobjDR = null;
                    SqlConnection pConnection = new SqlConnection();   

                    pobjDR = GetDataReader(pstrSQL,ref pConnection);
              
                    //....Store the ordinal for the given field for better performance.
                    int pColFldName = 0;
                    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                    //Add list items to the Combo Box
                    //-------------------------------
                    int pCountRec = 0;
                    string pRowVal = "";

                    cmbBox_In.Items.Clear();
                    while (pobjDR.Read())
                    {
                        pCountRec = pCountRec + 1;
                        if (pobjDR.IsDBNull(pColFldName) == false)
                            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                        //if (pRowVal != "")
                        cmbBox_In.Items.Add(pRowVal);
                    }

                    pobjDR.Close();
                    pConnection.Close();

                    return pCountRec;
                }


                public int PopulateCmbBox(ComboBox cmbBox_In,
                                        string strTableName_In, string strFldName_In,
                                        int Val_In, string strWHERE_In, bool blnOrderBy_In)
                //============================================================================   
                {
                    //....This utility function populates a comboBox and 
                    //......returns the # of list items, if any.

                    //This routine populate comboboxes
                    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                    //       Output  Parameters      :   No of Records

                    //....Create the SQL string.   
                    //

                    string pstrORDERBY = "";
                    if (blnOrderBy_In == true)
                        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                    string pstrSQL = "";

                    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                    //....Get the corresponding data reader object.
                
                    SqlConnection pConnection = new SqlConnection();   
                    SqlDataReader pobjDR = null;
                    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                    //....Store the ordinal for the given field for better performance.
                    int pColFldName = 0;
                    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                    //Add list items to the Combo Box
                    //-------------------------------
                    int pCountRec = 0;
                    string pRowVal = "";
                    Single pModVal;

                    cmbBox_In.Items.Clear();
                    while (pobjDR.Read())
                    {
                        pCountRec = pCountRec + 1;
                        if (pobjDR.IsDBNull(pColFldName) == false)
                            pRowVal = Convert.ToString(pobjDR[pColFldName]);
                        pModVal = Convert.ToSingle(pRowVal) / Val_In;

                        cmbBox_In.Items.Add(pModVal.ToString());
                    }

                    pConnection.Close();

                    pobjDR.Close();
                    return pCountRec;
                }


                public int PopulateLstBox(ListBox listBox_In,string strTableName_In, string strFldName_In,
                                          string strWHERE_In, bool blnOrderBy_In)
                //========================================================================================   
                {
                    //....This utility function populates a comboBox and 
                    //......returns the # of list items, if any.

                    //This routine populate comboboxes
                    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                    //       Output  Parameters      :   No of Records

                    //....Create the SQL string.   
                    //

                    string pstrORDERBY = "";
                    if (blnOrderBy_In == true)
                        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                    string pstrSQL = "";

                    pstrSQL = "SELECT " + strFldName_In + " FROM " +
                                strTableName_In + " " + strWHERE_In + pstrORDERBY;

                    //....Get the corresponding data reader object.

                    SqlConnection pConnection = new SqlConnection();
                    SqlDataReader pobjDR = null;
                    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                    //....Store the ordinal for the given field for better performance.
                    int pColFldName = 0;
                    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                    //Add list items to the Combo Box
                    //-------------------------------
                    int pCountRec = 0;
                    string pRowVal = "";               

                    listBox_In.Items.Clear();
                    while (pobjDR.Read())
                    {
                        pCountRec = pCountRec + 1;
                        if (pobjDR.IsDBNull(pColFldName) == false)
                            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                        listBox_In.Items.Add(pRowVal.ToString());
                    }

                    pConnection.Close();

                    pobjDR.Close();
                    return pCountRec;
                }

            
                //PB 18JAN12. To be reviewed later.
                public int PopulateStringCol(StringCollection strCol_In, 
                                         string strTableName_In, string strFldName_In,
                                         string strWHERE_In, bool blnOrderBy_In)
                //============================================================================    
                {
                    //....This utility function populates a comboBox and 
                    //......returns the # of list items, if any.

                    //This routine populate comboboxes
                    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                    //       Output  Parameters      :   No of Records

                    //....Create the SQL string.   
                    //

                    string pstrORDERBY = "";
                    if (blnOrderBy_In == true)
                        pstrORDERBY = " ORDER BY " + "[" + strFldName_In + "]" + " ASC";

                    string pstrSQL = "";

                    pstrSQL = "SELECT " + " DISTINCT " + strFldName_In + " FROM " +
                                strTableName_In + " " + strWHERE_In + pstrORDERBY;


                    //....Get the corresponding data reader object.
                    SqlConnection pConnection = new SqlConnection();   //SB 06JUL09

                    SqlDataReader pobjDR = null;
                    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                    //....Store the ordinal for the given field for better performance.
                    int pColFldName = 0;
                    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                    //Add list items to the Combo Box
                    //-------------------------------
                    int pCountRec = 0;
                    string pRowVal = "";

                    strCol_In.Clear();
                    while (pobjDR.Read())
                    {
                        pCountRec = pCountRec + 1;
                        if (pobjDR.IsDBNull(pColFldName) == false)
                            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                        //if (pRowVal != "")
                        strCol_In.Add(pRowVal);                  
                    }

                    pConnection.Close();

                    pobjDR.Close();
                    return pCountRec;
                }


                public int PopulateStringCol(StringCollection strCol_In,
                                                 string strTableName_In, string strFldName_In,
                                                 string strWHERE_In)
                //============================================================================      //SB 27APR09
                {
                    //....This utility function populates a comboBox and 
                    //......returns the # of list items, if any.

                    //This routine populate comboboxes
                    //       Input   Parameters      :   ComboBoxName, TableName, FieldName
                    //       Output  Parameters      :   No of Records

                    //....Create the SQL string.   
                    //

                    string pstrSQL = "";

                    pstrSQL = "SELECT " + strFldName_In + " FROM " +
                                            strTableName_In + " " + strWHERE_In;    //SB 09JUL09

                    //....Get the corresponding data reader object.
                    SqlConnection pConnection = new SqlConnection();   
                
                    SqlDataReader pobjDR = null;
                    pobjDR = GetDataReader(pstrSQL, ref pConnection);

                    //....Store the ordinal for the given field for better performance.
                    int pColFldName = 0;
                    pColFldName = pobjDR.GetOrdinal(strFldName_In);

                    //Add list items to the Combo Box
                    //-------------------------------
                    int pCountRec = 0;
                    string pRowVal = "";

                    strCol_In.Clear();
                    while (pobjDR.Read())
                    {
                        pRowVal = "";   //SB 19JUN09
                        pCountRec = pCountRec + 1;
                        if (pobjDR.IsDBNull(pColFldName) == false)
                            pRowVal = Convert.ToString(pobjDR[pColFldName]);

                        //if (pRowVal != "")
                        strCol_In.Add(pRowVal);
                    }

                    pobjDR.Close();
                    pConnection.Close();

                    return pCountRec;
                }

            #endregion


            #region "Data Checking - Retrieval & Insertion:"
            //---------------------------------------------

                public string CheckDBString(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                      string pStrFldVal;
                      if (Convert.IsDBNull(DR_In[FieldName_In]))
                          pStrFldVal = "";
                      else
                          pStrFldVal = Convert.ToString(DR_In[FieldName_In]);

                      return pStrFldVal;
                }


                public DateTime CheckDBDateTime(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                    DateTime pDtFldVal;
                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pDtFldVal = DateTime.MinValue;
                    else
                        pDtFldVal = Convert.ToDateTime(DR_In[FieldName_In]);

                    return pDtFldVal;
                }


                public Int32 CheckDBInt(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                    Int32 pIntFldVal;
                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pIntFldVal = 0;
                    else
                        pIntFldVal = Convert.ToInt32(DR_In[FieldName_In]);

                    return pIntFldVal;
                }


                public Single CheckDBSingle(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                    Single pSngFldVal;
                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pSngFldVal = 0;
                    else
                        pSngFldVal = Convert.ToSingle(DR_In[FieldName_In]);

                    return pSngFldVal;
                }


                public Double CheckDBDouble(SqlDataReader DR_In, String FieldName_In)
                //===================================================================
                {
                    Double pDblFldVal;
                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pDblFldVal = 0;
                    else
                        pDblFldVal = Convert.ToDouble(DR_In[FieldName_In]);

                    return pDblFldVal;
                }


                //PB 17DEC12. BG, check.
                public Boolean CheckDBBoolean(SqlDataReader DR_In, String FieldName_In)
                //===================================================================== 
                {
                    Boolean pFldVal = false;
                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pFldVal = false;
                    else
                        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                    return pFldVal;
                }


                public bool CheckDBBool(SqlDataReader DR_In, String FieldName_In)
                //===============================================================
                {
                    bool pFldVal = false;

                    if (Convert.IsDBNull(DR_In[FieldName_In]))
                        pFldVal = false;
                    else
                        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                    return pFldVal;
                }


                public Boolean ProjectNo_Exists (string No_In,string No_Suffix_In, string tbl_In)
                //===============================================================================
                {
                    Boolean pblnChkProjectNo;

                    string pNo_Suffix, pstrFIELD1, pstrFIELD2, pstrFROM, pstrWHERE;
                    string pstrSQL;

                    if (tbl_In == "tblProject_Details")     
                    {
                        pstrFIELD1 = "fldNo";
                        pstrFIELD2 = "fldNo_Suffix";
                    }
                    else
                    {
                        pstrFIELD1 = "fldProjectNo";
                        pstrFIELD2 = "fldProjectNo_Suffix";
                    }

                    if (No_Suffix_In != "" && No_Suffix_In != null)
                        pNo_Suffix = " = '" + No_Suffix_In + "'";
                    else
                        pNo_Suffix = " IS NULL";

                    pstrFROM = " FROM " + tbl_In;
                    pstrWHERE = " WHERE " + pstrFIELD1 + " = '" + No_In + "' AND " + pstrFIELD2 +  pNo_Suffix;

                    pstrSQL = "SELECT " + pstrFIELD1 + pstrFROM + pstrWHERE;

                    SqlConnection pConnection = new SqlConnection();   //SB 06JUL09
                    SqlDataReader pDR = null;
                    pDR = GetDataReader(pstrSQL, ref pConnection);

                    if (pDR.Read())
                    {
                        pblnChkProjectNo = true;                
                    }
                    else
                        pblnChkProjectNo = false;
                    pDR.Close();
                    pConnection.Close();

                    return pblnChkProjectNo;
                }

            #endregion

        #endregion

    }
}


