
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       FORM MODULE  :  frmFilter                              '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  24JAN13                                '
//                                                                              '
//===============================================================================

//  Routines :
//  ---------


//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Data.Sql;

namespace BearingCAD20
{
    public partial class frmFilter : Form
    {
        Boolean mblnNum = false, mblnDate = false;       
        StringCollection mFieldsNames = new StringCollection();
        StringCollection mTableNames = new StringCollection();
        StringCollection mCompareOperators = new StringCollection();
        StringCollection mLogicalOperators = new StringCollection();
        StringCollection mValues = new StringCollection();
        string mStatus = "";
        int mFlag = 0;
      
        public frmFilter()
        //================
        {
            InitializeComponent();

            //if(modMain.gProject!=null)
            //modMain.gProject.Filtered = false;
           
        }

        //***************************************************************************
        // *                    FORM EVENT ROUTINES - BEGIN                         *
        //*************************************************************************** 

        private void frmFilter_Load(object sender, EventArgs e)
       //========================================================
        {
            mFlag = mFlag + 1;
            if (mFlag == 1)
            {
                mStatus = modMain.gProject.Status;
            }
            else
            {
                if (mStatus != modMain.gProject.Status)
                {
                    mFieldsNames = new StringCollection();
                    mTableNames = new StringCollection();
                    mCompareOperators = new StringCollection();
                    mLogicalOperators = new StringCollection();
                    mValues = new StringCollection();
                    txtCriteria.Clear();
                }
            }
          
        
            InitializeControls();
            mblnNum = false;
        }

        private void InitializeControls()
        //=================================
        {  
            lstAvailableFields.Items.Clear();
            lstSelectedFields.Items.Clear();
            cmbCompareOperator.Items.Clear();
            cmbSelectData.Items.Clear();
            //txtCriteria.Clear();
            cmdAdd.Enabled = true;
            cmdAddOperator.Enabled = false;

            string pWHERE;
            if (modMain.gProject.Status == "Open")
                pWHERE = " WHERE fldInclude = 1 and fldListBoxItem != 'End Configuration'";
            else
                pWHERE = " WHERE fldInclude = 1";
            
            modMain.gDB.PopulateLstBox(lstAvailableFields, "tblMapping_Filter", "fldListBoxItem", pWHERE, false);

            if (lstSelectedFields.Items.Count > 0)
            {
                cmdAddCriteria.Enabled = true;
                cmdRem.Enabled = true;
               
            }
            else
            {
                cmdAddCriteria.Enabled = false;
                cmdRem.Enabled = false;
            }

            if (txtCriteria.Text != "")
            {
                cmbAddOperator.Enabled = true;
                cmdAddOperator.Enabled = true;      //BG 21NOV11
            }
            else
            {
                cmbAddOperator.Enabled = false;
                cmdAddOperator.Enabled = false;     //BG 21NOV11
            }

            //lblSelectData.Visible = false;
            lblSelectData.Text = "Select Data";
            cmbSelectData.Visible = true;
            txtSelectData.Visible = false;
            dtpDate.Visible = false;

            //if (txtCriteria.Tag!= null)
            //txtCriteria.Text = txtCriteria.Tag.ToString();

            cmbAddOperator.SelectedIndex = 0;
        }

        //***************************************************************************
        // *                    FORM EVENT ROUTINES - END                           *
        //*************************************************************************** 

        //***************************************************************************
        //*                       CONTROL EVENT ROUTINES - BEGIN                    *
        //***************************************************************************


        private void cmdAdd_Click(object sender, EventArgs e)
        //======================================================
        {
            PopulateListBox(lstAvailableFields, lstSelectedFields);
        }

        private void cmdRem_Click(object sender, EventArgs e)
        //======================================================
        {
            PopulateListBox(lstSelectedFields, lstAvailableFields);
            cmdAddCriteria.Enabled = false;
            cmbCompareOperator.SelectedIndex = -1;

            cmbSelectData.Items.Clear();
        }

        private void PopulateListBox(ListBox ListBox_From, ListBox ListBox_In)
        //======================================================================  
        {
            int i = 0;
            int j = 0;

            if (ListBox_From.SelectedIndex == -1)
            {
                MessageBox.Show("PLEASE SELECT AN APPROPRIATE FIELD", 
                                "**FIELDS**", MessageBoxButtons.OK,
                                              MessageBoxIcon.Exclamation);

                ListBox_From.Focus();
                ListBox_From.SetSelected(0, true);

            }

            else if (ListBox_From.SelectedIndex >= 0)
            {

                i = ListBox_From.Items.Count - 1;

                for (j = 0; j <= i; j++)
                {
                    if (ListBox_From.GetSelected(j))
                    {
                        ListBox_In.Items.Add(ListBox_From.Items[j]);

                        if (ListBox_In.Items.Count > 0)
                            cmdAdd.Enabled = false;
                        else
                            cmdAdd.Enabled = true;

                        ListBox_From.Items.RemoveAt(j);   
                        ListBox_In.SetSelected(0, true);
                        break;
                        //i = i - 1; 
                    }
                }
               
            }

        }

      

        private void lstSelectedFields_SelectedIndexChanged(object sender, EventArgs e)
      //================================================================================
        {
           if (lstSelectedFields.Items.Count>0)
            {
                SetCompareOperatorAndControls(lstSelectedFields.Text);             
                               
                cmdAdd.Enabled = false;
                cmdRem.Enabled = true;
                cmdAddCriteria.Enabled = true;
                
            }
            else
            {
                cmdRem.Enabled = false;
                cmdAdd.Enabled = true;
                cmbCompareOperator.Text = "";
                cmbSelectData.Text = "";
                cmbSelectData.Items.Clear();
                cmdAddCriteria.Enabled = false;
               
            }
        }

       
        private void SetCompareOperatorAndControls(string SeletedItem_In)
        //================================================================
        {
            string pstrSQL = "SELECT fldDataType, fldOperators FROM tblMapping_Filter WHERE fldListBoxItem = '" + SeletedItem_In + "'";

            SqlConnection pConnection = new SqlConnection();

            SqlDataReader pDR = null;
            pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

            string pDataType = "", pstrOperator = "";
            if (pDR.Read())
            {
                pDataType = modMain.gDB.CheckDBString(pDR, "fldDataType");
                pstrOperator = modMain.gDB.CheckDBString(pDR, "fldOperators");
            }

            string[] pOperators = pstrOperator.Split(',');
            cmbCompareOperator.Items.Clear();

            for (int i = 0; i < pOperators.Length; i++)
                cmbCompareOperator.Items.Add(pOperators[i]);
            
            switch(pDataType)
            {
                case "varchar":
                //-------------  
                      lblSelectData.Text = "Select Data";
                      txtSelectData.Visible = false;
                      cmbSelectData.Visible = true;
                      Populate_cmbSelectData();
                      mblnNum = false;
                      mblnDate = false;
                      break;

                case "numeric":
                //-------------- 
                      lblSelectData.Text = "Enter Data";
                      txtSelectData.Visible = true;        
                      cmbSelectData.Visible = false;
                      mblnNum = true;
                      mblnDate = false;
                      break;

                case "int":
                      //-------------- 
                      lblSelectData.Text = "Enter Data";
                      txtSelectData.Visible = true;
                      cmbSelectData.Visible = false;
                      mblnNum = true;
                      mblnDate = false;
                      break;

                case "datetime":
                //--------------  
                      lblSelectData.Text = "Enter Data";
                      txtSelectData.Visible = true;        
                      cmbSelectData.Visible = false;
                      dtpDate.Visible = true;
                      mblnDate = true;
                      mblnNum = false;
                      break;

                case "bit":
                //----------
                      lblSelectData.Text = "Select Data";
                      txtSelectData.Visible = false;
                      cmbSelectData.Visible = true;
                      Populate_cmbSelectData();
                      mblnNum = false;
                      mblnDate = false;
                      break;

             }

            cmbCompareOperator.SelectedIndex = 0;                  
        }


        private void Populate_cmbSelectData()
        //====================================
        {
            string pstrWHERE = "";
            if( modMain.gProject.Status=="Closed")
                pstrWHERE = "Where fldStatus = 'Closed'";

            string pFieldName = "";
            string pTableName = "";

            string pSelecctedData = "";
            if (lstSelectedFields.Items.Count > 0)
            {
                pSelecctedData = lstSelectedFields.Items[0].ToString();
            }

            switch (pSelecctedData)
            {
                case "Customer":
                    pFieldName = "fldCustomer_Name";
                    pTableName = "tblProject_Details";    
                    break;

                case "Machine Description":
                    pFieldName = "fldCustomer_MachineDesc";
                    pTableName = "tblProject_Details";        
                    break;

                case "End Configuration":
                    cmbSelectData.Items.Clear();
                    cmbSelectData.Items.Add("Seal / Thrust Bearing TL");
                    cmbSelectData.Items.Add("Thrust Bearing TL / Seal");
                    cmbSelectData.Items.Add("Thrust Bearing TL / Thrust Bearing TL");
                    cmbSelectData.Items.Add("Seal / Seal ");
                    cmbSelectData.SelectedIndex = 0;
                    return;

                case "Closing Date":
                        
                    break;

                 case "Part No.":
                    pFieldName = "fldPartNo";
                    pstrWHERE = "";
                    pTableName = "tblManf_Fixture_SplitAndTurn";    
                    break;

                 case "Engineer":
                    pFieldName = "fldEngg_Name";
                    pTableName = "tblProject_Details";    
                    break;
                    
                 case "Designer":
                    pFieldName = "fldDesignedBy_Name";
                    pTableName = "tblProject_Details";    
                    break;

                case "Split Configuration":
                    cmbSelectData.Items.Clear();
                    cmbSelectData.Items.Add("Yes");
                    cmbSelectData.Items.Add("No");
                    cmbSelectData.SelectedIndex = 0;
                    return;
                    //break;

                case "Bearing Material":
                    pFieldName = "fldName";
                    pstrWHERE = "";
                    pTableName = "tblData_Mat";    
                    break;
            }

            modMain.gDB.PopulateCmbBox(cmbSelectData, pTableName,pFieldName, pstrWHERE, true);

            if (cmbSelectData.Items.Count > 0)
            {
                cmbSelectData.SelectedIndex = 0;
            }
              
        }


        private Boolean CheckLogicalOperator(string Criteria_In)
        //======================================================
        {
            Boolean pblnVal = false;

            int pLen = Criteria_In.Length - 4;
            string pstrEnd = Criteria_In.Substring(pLen);

            if (pstrEnd.ToUpper().Trim() == "AND" || pstrEnd.ToUpper().Trim() =="OR")
            {
                pblnVal = true;
            }
            return pblnVal;
        }


        private void cmdAddCriteria_Click(object sender, EventArgs e)
        //==============================================================
        {
            String pCriteria = "";

            if (txtCriteria.Text != "")
            {
                if (!CheckLogicalOperator(txtCriteria.Text))
                {
                    MessageBox.Show("Please add operator, before adding next criterion.","ERROR",
                                     MessageBoxButtons.OK,MessageBoxIcon.Error);
                    cmbAddOperator.Focus();
                    return;
                }
            }

            if (mblnNum)
            {
                if (txtSelectData.Text == "")
                {
                    MessageBox.Show("Please enter data.", "Data Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSelectData.Focus();
                    return;
                }

                if (txtCriteria.Text != "")
                    pCriteria = "\r\n" + lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text
                                                                    + " " + txtSelectData.Text.Trim();
                else                              
                    pCriteria = lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text
                                                                    + " " + txtSelectData.Text.Trim();                                
            }
            else if (mblnDate)
            {
                if (txtCriteria.Text != "")
                    pCriteria = "\r\n" + lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text
                                                          + " '" + txtSelectData.Text.Trim() + "'";
                else
                    pCriteria = lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text
                                                    + " '" + txtSelectData.Text.Trim() + "'";
            }
            else
            {
                if (txtCriteria.Text != "")
                    pCriteria = "\r\n" + lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text                                                          + " '" + cmbSelectData.Text.Trim() + "'";

                else
                    pCriteria = lstSelectedFields.Text.Trim() + " " + cmbCompareOperator.Text
                                                    + " '" + cmbSelectData.Text.Trim() + "'";
            }

            txtCriteria.Text = txtCriteria.Text + pCriteria;
            RetrieveFieldAndTableName(lstSelectedFields.Text);
           
            lstAvailableFields.Items.Add(lstSelectedFields.Text);
            
            lstSelectedFields.Items.Clear();
            cmbSelectData.Text = "";
            cmbSelectData.Items.Clear();
            cmbCompareOperator.Text = "";
            cmdAdd.Enabled = true;
            cmdRem.Enabled = false;
            cmdAddCriteria.Enabled = false;
            cmbAddOperator.Enabled = true;
            cmdAddOperator.Enabled = true;
            txtSelectData.Text = "";
            dtpDate.Visible = false;
            cmbCompareOperator.Items.Clear();       

        }

        private void RetrieveFieldAndTableName(string ListBoxItem_In)
        //=============================================================
        {

            string pstrSQL = "SELECT fldFieldName, fldDataType, fldTableName FROM tblMapping_Filter WHERE fldListBoxItem = '" + ListBoxItem_In + "'";
            string pDataType = "";
            string pFieldName = "";
            string pCompOper = "";

            SqlConnection pConnection = new SqlConnection();

            SqlDataReader pDR = null;
            pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

            if (pDR.Read())
            {
                mTableNames.Add(modMain.gDB.CheckDBString(pDR, "fldTableName"));
                pFieldName = modMain.gDB.CheckDBString(pDR, "fldFieldName");

                if (pFieldName.Contains(","))
                {
                    pCompOper = cmbCompareOperator.Text;
                    mFieldsNames.Add(modMain.gDB.CheckDBString(pDR, "fldFieldName"));
                }
                else
                {
                    mFieldsNames.Add(modMain.gDB.CheckDBString(pDR, "fldFieldName"));
                }

                mCompareOperators.Add(cmbCompareOperator.Text);  

                pDataType = modMain.gDB.CheckDBString(pDR, "fldDataType");

                if (pDataType == "numeric")
                {
                    mValues.Add(txtSelectData.Text);
                }
                else if(pDataType =="varchar")
                {
                    mValues.Add(" '" + cmbSelectData.Text + "' ");
                }
                else if (pDataType == "datetime")
                {
                    mValues.Add("'" + txtSelectData.Text + "'");
                }

                else if (pDataType == "bit")
                {
                    if (cmbSelectData.Text.ToUpper() == "YES")
                    {
                        mValues.Add("1");
                    }
                    else
                    {
                        mValues.Add("0");
                    }
                }
            }
        }

        private void SQLQuery()
        //=========================
        {
            int pTblCount_OpCond = 1, pTblCount_Manf_Fixture_SplitAndTurn = 1, pTblCount_Data_Mat = 1;
            int pTblCount_End_Config = 1, pTblCount_Bearing_Radial_FP = 1, pTblCount_Bearing_Radial_FP_Pad = 1;

            string pstrSelect = "";

            //pstrSelect = "Select Distinct fldNo From tblProject_Details tblPD";

            if (modMain.gProject.Status == "Open")
            {
                pstrSelect = "Select Distinct fldNo From tblProject_Details tblPD inner join tblProject_Product tblPP on " +
                             "tblPD.fldNo = tblPP.fldProjectNo ";               
            }
            else
            {
                pstrSelect = "Select Distinct fldNo From tblProject_Details tblPD";
            }
        
            string pStr = " INNER JOIN ";
            string pstrJoin = ""; 
            StringCollection pCondition = new StringCollection();
           
            if (mTableNames.Contains("tblProject_Bearing_Radial_FP") ||
                mTableNames.Contains("tblManf_Fixture_SplitAndTurn") ||
                    mTableNames.Contains("tblData_Mat") ||
                    mTableNames.Contains("tblProject_Bearing_Radial_FP_Pad"))
            {
                pstrJoin = pStr + " tblProject_Bearing_Radial_FP tblPB on tblPD.fldNo = tblPB.fldProjectNo ";
            }                  
            
            string[] pFieldName = new string[2];
            string[] pFieldVal = new string[2];
            
            for (int i = 0; i < mTableNames.Count; i++)
            {
               
                if (mTableNames[i] == "tblManf_Fixture_SplitAndTurn")
                {
                    if (pTblCount_Manf_Fixture_SplitAndTurn == 1)
                    {
                        pstrJoin = pstrJoin + pStr + mTableNames[i] + " tblMF on  tblPB.fldSealMountFixture_Sel_PartNo = tblMF." + mFieldsNames[i];
                    }
                    pTblCount_Manf_Fixture_SplitAndTurn++;
                    pCondition.Add(" tblMF." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]); 
                }

                else if (mTableNames[i] == "tblData_Mat")
                {
                    if (pTblCount_Data_Mat == 1)
                    {
                        pstrJoin = pstrJoin + pStr + mTableNames[i] + " tblDM on tblPB.fldMat_Base = tblDM." + mFieldsNames[i];
                    }
                    pTblCount_Data_Mat++;

                    pCondition.Add(" tblDM." + mFieldsNames[i] + mCompareOperators[i] + mValues[i] ); 
                }

                else if (mTableNames[i] == "tblProject_OpCond")
                {
                    pFieldName = mFieldsNames[i].Split(',');
                    string pFieldName_Sum = pFieldName[0].Trim() + " + tblOC." + pFieldName[1].Trim();

                    if (pTblCount_OpCond == 1)
                    {
                        pstrJoin = pstrJoin + pStr + mTableNames[i] + " tblOC on tblPD.fldNo =  tblOC.fldProjectNo";// '+ mFieldsNames[i];
                    }
                    pTblCount_OpCond++;
                 
                    pCondition.Add(" tblOC." + pFieldName_Sum + mCompareOperators[i] + (modMain.ConvTextToInt(mValues[i]) * 2)); 
                }

                else if (mTableNames[i] == "tblProject_Bearing_Radial_FP")
                {
                    if (mFieldsNames[i].Contains(","))
                    {
                         pFieldName = mFieldsNames[i].Split(',');
                         string pFieldName_Sum = pFieldName[0].Trim() + " + tblPB." + pFieldName[1].Trim();

                        pCondition.Add(" tblPB." + pFieldName_Sum + mCompareOperators[i] + (modMain.ConvTextToDouble(mValues[i]) * 2));
                    }
                    else
                    {
                        pCondition.Add(" tblPB." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]);
                    }
                }

                else if (mTableNames[i] == "tblProject_Bearing_Radial_FP_Pad")
                {
                    if (pTblCount_Bearing_Radial_FP_Pad == 1)
                        {
                            pstrJoin = pstrJoin + pStr + mTableNames[i] + " tblPB_Pad on tblPB.fldProjectNo = tblPB_Pad.fldProjectNo ";
                        }
                        pTblCount_Bearing_Radial_FP_Pad++;

                        pCondition.Add(" tblPB_Pad." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]);
                   
                }

                else if (mTableNames[i] == "tblProject_Details")
                {
                    
                    if (mFieldsNames[i] == "fldDate_Closing")
                    {
                        //pCondition.Add(" tblPD.fldStatus = 'Closed'");
                        //mLogicalOperators.Add( " AND ");
                        pCondition.Add(" tblPD." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]);
                    }
                   else
                        pCondition.Add(" tblPD." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]);
                }

                else if (mTableNames[i] == "tblProject_EndConfigs")
                {
                    if (mFieldsNames[i].Contains(",") && mValues[i].Contains("/"))
                    {
                        char pQuote = '\'';
                        pFieldName = mFieldsNames[i].Split(',');
                        pFieldVal = mValues[i].Replace(pQuote, ' ').Split('/');
                        String[] pPosition = new String[2] { "Front", "Back" };

                        for (int j = 0; j < pFieldVal.Length; j++)
                        {
                            pFieldVal[j] = pFieldVal[j].Trim();
                            if (pFieldVal[j].Contains("Thrust Bearing"))
                                pFieldVal[j] = "Thrust_Bearing_TL";
                        }
                                              
                        if (pTblCount_End_Config == 1)
                        {
                            pstrJoin = pstrJoin + pStr + " tblProject_EndConfigs tblPE1 on tblPD.fldNo =  tblPE1.fldProjectNo " +
                                        " inner join tblProject_EndConfigs tblPE2 on tblPE1.fldProjectNo = tblPE2.fldProjectNo ";
                        }
                        pTblCount_End_Config++;

                        pCondition.Add(" tblPE1." + pFieldName[0] + mCompareOperators[i] + "'" + pFieldVal[0] + "' AND " + "tblPE1." + pFieldName[1].Trim() + mCompareOperators[i] + "'" + pPosition[0] + "' AND" +
                                       " tblPE2." + pFieldName[0] + mCompareOperators[i] + "'" + pFieldVal[1] + "' AND " + "tblPE2." + pFieldName[1].Trim() + mCompareOperators[i] + "'" + pPosition[1] + "'");
                    }

                    //else
                    //    pCondition.Add(" tblPD." + mFieldsNames[i] + mCompareOperators[i] + mValues[i]);
                }                

            }

           
            string pstrWHERE = "";
            
            for (int i = 0; i < pCondition.Count; i++)
            {
                if (i == 0)
                {                   
                    pstrWHERE = pstrWHERE + pCondition[i];
                }
                else
                {
                    pstrWHERE =  pstrWHERE +  " " + mLogicalOperators[i-1] +" "+ pCondition[i];
                }
            }

            if (modMain.gProject.Status == "Closed")
            {
                string pstrClosed = " AND fldStatus = 'Closed'";
                pstrSelect = pstrSelect + " " + pstrJoin + " WHERE " + pstrWHERE + pstrClosed;
            }
            else
            {
                //string pstrEndConfig = " fldEndConfig_Front_Type = '" + modMain.gProject.Product.EndConfig[0].Type.ToString() + "' AND" + //SG 24JAN13
                //                       " fldEndConfig_Back_Type = '" + modMain.gProject.Product.EndConfig[1].Type.ToString() + "'";

                pstrSelect = pstrSelect + " " + pstrJoin + " WHERE " + pstrWHERE;// +" AND" + pstrEndConfig;

                //pstrSelect = pstrSelect + " " + pstrJoin + " WHERE " + pstrWHERE;
            }

            SqlDataReader pDR = null;
            SqlConnection pConnection = new SqlConnection();           

            pDR = modMain.gDB.GetDataReader(pstrSelect, ref pConnection);

            modMain.gProject.Filtered_Project_No.Clear();

            if (pDR.HasRows)
            {
                while (pDR.Read())
                {
                    modMain.gProject.Filtered_Project_No.Add(modMain.gDB.CheckDBString(pDR, "fldNo"));
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("No records found!!", "Filter Data", MessageBoxButtons.OK);
                return;
                this.Show();
            }

            pDR.Close();

            modMain.gProject.Filtered = true;
        }


        private void cmdAddOperator_Click(object sender, EventArgs e)
        //============================================================
        {           
            if (txtCriteria.Text != "")
            {                
                txtCriteria.Text = txtCriteria.Text + " " + cmbAddOperator.Text + " ";
                mLogicalOperators.Add(cmbAddOperator.Text);
            }
            cmdAddOperator.Enabled = false;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        //===========================================================
        {
            txtSelectData.Text = dtpDate.Value.ToString("MM-dd-yyyy");
        }


        private void cmdOK_Click(object sender, EventArgs e)
       //====================================================
        {
            if (txtCriteria.Text != "")
            {
                if(CheckLogicalOperator(txtCriteria.Text))
                {
                    MessageBox.Show("Please add criteria properly.");
                    cmdAddCriteria.Focus();
                    return; 
                }
               
                txtCriteria.Tag = txtCriteria.Text.Trim();
                SQLQuery();
                //this.Close();
            }

            else
            {
                MessageBox.Show("Please add criteria properly.");
                cmdAddCriteria.Focus();
                return;
            }
        }

        
        private void cmdCancel_Click(object sender, EventArgs e)
        //====================================================
        {
            txtCriteria.Text = "";
            mFieldsNames.Clear();
            mTableNames.Clear();
            mCompareOperators.Clear();
            mLogicalOperators.Clear();
            mValues.Clear();
          
            this.Close();
        }


        private void cmdReset_Click(object sender, EventArgs e)
        //======================================================
        {
            txtCriteria.Text = "";
            cmbAddOperator.Enabled = false;
            cmdAddOperator.Enabled = false;     //BG 21NOV11  

            mFieldsNames.Clear();
            mTableNames.Clear();
            mCompareOperators.Clear();
            mLogicalOperators.Clear();
            mValues.Clear();                     
        }


        private void cmdPrint_Click(object sender, EventArgs e)
        //=====================================================
        {
            string pFilePath = "C:\\BearingCAD\\Query.txt";
            if (File.Exists(pFilePath))
            {
                File.Delete(pFilePath);
               
            }            

            StreamWriter pSW = new StreamWriter(pFilePath);
            //pSW.WriteLine();

            string pTxt = txtCriteria.Text;
            pSW.WriteLine(pTxt);
            pSW.Close();    

            System.Diagnostics.Process.Start(pFilePath);

        }


        private void cmbCompareOperator_SelectedIndexChanged(object sender, EventArgs e)
        //==============================================================================
        {
            if (cmbCompareOperator.Text.Trim().ToUpper() == "LIKE")
            {
                cmbSelectData.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else
            {
                cmbSelectData.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }    
         

        //***************************************************************************
        //*                       CONTROL EVENT ROUTINES - END                      *
        //***************************************************************************
      
       

    }
}