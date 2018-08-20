
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmCopyProject                         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  21MAR13                                '
//                                                                              '
//===============================================================================
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BearingCAD21
{
    public partial class frmCopyProject : Form
    {

        #region "MEMBER VARIABLE"
        //***********************

            //....Local Objects
            private clsProject mProject;
            private clsOpCond mOpCond;

        #endregion


        public frmCopyProject()
        //=====================
        {
            InitializeComponent();
        }

        private void Initialize_LocalObject()
        //===================================
        {    
            //....Operating Condition.
            mOpCond = new clsOpCond();

            //BG 21DEC12
            ////....End Config.
            //clsEndConfig.eType[] pEndConfig = new clsEndConfig.eType[2];
            //for (int i = 0; i < pEndConfig.Length; i++)
            //    pEndConfig[i] = clsEndConfig.eType.Seal;

           
            //....Project.
            //mProject = new clsProject(clsUnit.eSystem.Metric, clsBearing.eType.Radial,
            //                            clsBearing_Radial.eDesign.Flexure_Pivot, pEndConfig, modMain.gDB);

            mProject = new clsProject(modMain.gProject.Unit.System);
            mProject.Product = new clsProduct(mProject.Unit.System, modMain.gDB);
        }


        private void frmCopyProject_Load(object sender, EventArgs e)
        //==========================================================
        {
            Initialize_LocalObject();
            txtProjectNo_To.Text = modMain.gProject.No;
            txtProjectNo_Suffix_To.Text = modMain.gProject.No_Suffix;

            //....Retrieve Project Number from Database.
            Populate_ProjectNo();

           //int pRecCount = modMain.gDB.PopulateCmbBox(cmbProjectNo_From,
           //                                           "tblProject_Details", "fldNo", "", true);
           //if (pRecCount > 0)
           //{
           //    cmbProjectNo_From.SelectedIndex = 0;
           //}
        }

        private void Populate_ProjectNo()
        //=============================== //SG 24JAN13
        {
            //string pstrSelect = "Select Distinct fldNo From tblProject_Details tblPD inner join tblProject_Product tblPP on " +
            //                    "tblPD.fldNo = tblPP.fldProjectNo"; 
            //string pstrWhere = " WHERE " + "fldEndConfig_Front_Type = '" + modMain.gProject.Product.EndConfig[0].Type.ToString() + 
            //                   "' AND " +"fldEndConfig_Back_Type = '" + modMain.gProject.Product.EndConfig[1].Type.ToString() + "'";

        
            string pstrInnerSelect = "(SELECT DISTINCT fldProjectNo FROM tblProject_EndConfigs  WHERE " +
                                   " fldPosition = 'Front' AND " + "fldType = '" + modMain.gProject.Product.EndConfig[0].Type.ToString() +
                                   "' UNION SELECT  DISTINCT fldProjectNo FROM tblProject_EndConfigs  WHERE " +
                                   "fldPosition = 'Back' AND " + "fldType = '" + modMain.gProject.Product.EndConfig[1].Type.ToString() + "')";


            string pstrSelect = "SELECT DISTINCT fldNo FROM tblProject_Details tblDetails INNER JOIN tblProject_EndConfigs tblEndConfig on " +
                                "tblDetails.fldNo = tblEndConfig.fldProjectNo" +
                                " AND tblEndConfig.fldProjectNo IN " + pstrInnerSelect;  

            string pstrWhere = " WHERE tblDetails.fldUnitSystem = '" + modMain.gProject.Unit.System.ToString() + "'";

            string pstrSQL = pstrSelect + pstrWhere;

            SqlDataReader pDR = null;
            SqlConnection pConnection = new SqlConnection();

            pDR = modMain.gDB.GetDataReader(pstrSQL, ref pConnection);

            cmbProjectNo_From.Items.Clear();
         
            while (pDR.Read())
            {
                string pPrjNo = modMain.gDB.CheckDBString(pDR, "fldNo");
                //if(txtProjectNo_To.Text!= pPrjNo)
                    cmbProjectNo_From.Items.Add(pPrjNo);
            }                 

            pDR.Close();

            if (cmbProjectNo_From.Items.Count > 0)
            {
                cmbProjectNo_From.SelectedIndex = 0;
            }

        }
               


        private void frmCopyProject_Activated(object sender, EventArgs e)
        //================================================================
        {
            optFiltered.Checked = modMain.gProject.Filtered;
            optAll.Checked = !modMain.gProject.Filtered;

            if (modMain.gProject.Filtered)
            {      
                cmbProjectNo_From.Items.Clear();
                cmbProjectNo_From.Refresh();

                for (int i = 0; i < modMain.gProject.Filtered_Project_No.Count; i++)
                {
                    cmbProjectNo_From.Items.Add(modMain.gProject.Filtered_Project_No[i]);
                }

                if (modMain.gProject.Filtered_Project_No.Count > 0)
                {
                    cmbProjectNo_From.SelectedIndex = 0;
                }
            }
           
        }


        private void cmbProjectNo_From_SelectedIndexChanged(object sender, EventArgs e)
        //=============================================================================
        {
            string pProjectNo = cmbProjectNo_From.Text;

            string pWHERE = "WHERE fldNo = '" + pProjectNo + "'";
            int pNo_Suffix_Count = modMain.gDB.PopulateCmbBox(cmbProjectNo_Suffix_From, "tblProject_Details", 
                                                              "fldNo_Suffix", pWHERE, true);
            if (pNo_Suffix_Count > 0)
            {
              cmbProjectNo_Suffix_From.SelectedIndex = 0;
            }
        }

      

        private void cmdFilter_Click(object sender, EventArgs e)
        //======================================================
        {
            //modMain.gblnCopyProject = true;
            ////modMain.gfrmFilter.ShowDialog();      
        }


        private void cmdClose_Click(object sender, EventArgs e)
        //======================================================
        {
            Button pBtn = (Button)sender;

            if(pBtn.Name == "cmdOK")
            {
                CopyProject();                
            }
            //modMain.gblnCopyProject = false;
            this.Close();
        }


        private void CopyProject()
        //========================
        {
            mProject.No = cmbProjectNo_From.Text;
            mProject.No_Suffix = cmbProjectNo_Suffix_From.Text;
            
            clsEndConfig.eType[] pEndConfig_Type = new clsEndConfig.eType[2];
            ////modMain.gDB.Retrieve_EndConfigs_Type(mProject, pEndConfig_Type);

            //BG 21DEC12
            mProject = new clsProject(modMain.gProject.Unit.System);
            mProject.Product = new clsProduct(mProject.Unit.System, modMain.gDB);

                  
            for (int i = 0; i < 2; i++)
            {
                if (pEndConfig_Type[i] == clsEndConfig.eType.Seal)                                 //....End Seal.
                {
                    mProject.Product.EndConfig[i] = new clsSeal(mProject.Unit.System, pEndConfig_Type[i], modMain.gDB, mProject.Product);
                }
                else if (pEndConfig_Type[i] == clsEndConfig.eType.Thrust_Bearing_TL)               //....End TB.
                {
                    mProject.Product.EndConfig[i] = new clsBearing_Thrust_TL(mProject.Unit.System, pEndConfig_Type[i], modMain.gDB, mProject.Product);
                }
            }


            mProject.No = cmbProjectNo_From.Text;
            mProject.No_Suffix = cmbProjectNo_Suffix_From.Text;
      

            ////modMain.gDB.RetrieveRecord(mProject, mOpCond);      
                        
            modMain.gProject = (clsProject)mProject.Clone();        
            modMain.gOpCond = (clsOpCond)mOpCond.Clone();

            modMain.gProject.No = txtProjectNo_To.Text;
            modMain.gProject.No_Suffix = txtProjectNo_Suffix_To.Text;
           
        }


        private void optAll_CheckedChanged(object sender, EventArgs e)
        //============================================================
        {
               //....Retrieve Project Number from Database.
           int pRecCount = modMain.gDB.PopulateCmbBox(cmbProjectNo_From,
                                                      "tblProject_Details", "fldNo", "", true);
           if (pRecCount > 0)
           {
               cmbProjectNo_From.SelectedIndex = 0;
           }
       
        }       
        
    }
}
