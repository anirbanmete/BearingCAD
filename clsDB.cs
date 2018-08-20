
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

namespace BearingCAD21
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
           


            #region "Data Checking - Retrieval & Insertion:"
            //---------------------------------------------

                //public string CheckDBString(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================
                //{
                //      string pStrFldVal;
                //      if (Convert.IsDBNull(DR_In[FieldName_In]))
                //          pStrFldVal = "";
                //      else
                //          pStrFldVal = Convert.ToString(DR_In[FieldName_In]);

                //      return pStrFldVal;
                //}

                public string CheckDBString(Object Val_In)
                //========================================  //AES 27JUL18
                {
                    string pStrFldVal;
                    if (Val_In==null)
                        pStrFldVal = "";
                    else
                        pStrFldVal = Convert.ToString(Val_In);

                    return pStrFldVal;
                }



                //public DateTime CheckDBDateTime(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================
                //{
                //    DateTime pDtFldVal;
                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pDtFldVal = DateTime.MinValue;
                //    else
                //        pDtFldVal = Convert.ToDateTime(DR_In[FieldName_In]);

                //    return pDtFldVal;
                //}


                //public Int32 CheckDBInt(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================
                //{
                //    Int32 pIntFldVal;
                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pIntFldVal = 0;
                //    else
                //        pIntFldVal = Convert.ToInt32(DR_In[FieldName_In]);

                //    return pIntFldVal;
                //}

                public Int32 CheckDBInt(Object Val_In)
                //====================================  //AES 28MAY18
                {
                    Int32 pIntFldVal;
                    if (Val_In==null)
                        pIntFldVal = 0;
                    else
                        pIntFldVal = Convert.ToInt32(Val_In);

                    return pIntFldVal;
                }


                //public Single CheckDBSingle(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================
                //{
                //    Single pSngFldVal;
                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pSngFldVal = 0;
                //    else
                //        pSngFldVal = Convert.ToSingle(DR_In[FieldName_In]);

                //    return pSngFldVal;
                //}


                //public Double CheckDBDouble(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================
                //{
                //    Double pDblFldVal;
                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pDblFldVal = 0;
                //    else
                //        pDblFldVal = Convert.ToDouble(DR_In[FieldName_In]);

                //    return pDblFldVal;
                //}

                public Double CheckDBDouble(Object Val_In)
                //========================================  //AES 27JUL18
                {
                    Double pDblFldVal;
                    if (Val_In==null)
                        pDblFldVal = 0;
                    else
                        pDblFldVal = Convert.ToDouble(Val_In);

                    return pDblFldVal;
                }


                ////PB 17DEC12. BG, check.
                //public Boolean CheckDBBoolean(SqlDataReader DR_In, String FieldName_In)
                ////===================================================================== 
                //{
                //    Boolean pFldVal = false;
                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pFldVal = false;
                //    else
                //        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                //    return pFldVal;
                //}


                //public bool CheckDBBool(SqlDataReader DR_In, String FieldName_In)
                //===============================================================
                //{
                //    bool pFldVal = false;

                //    if (Convert.IsDBNull(DR_In[FieldName_In]))
                //        pFldVal = false;
                //    else
                //        pFldVal = Convert.ToBoolean(DR_In[FieldName_In]);

                //    return pFldVal;
                //}

                public bool CheckDBBool(Object Val_In)
                //====================================  //AES 28MAY18
                {
                    bool pFldVal = false;

                    if (Val_In==null)
                        pFldVal = false;
                    else
                        pFldVal = Convert.ToBoolean(Val_In);

                    return pFldVal;
                }


               

            #endregion

        #endregion

    }
}


