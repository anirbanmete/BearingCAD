
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmProject                             '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  24JUL18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       frmProject_Load                     ()
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using EXCEL = Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Drawing.Printing;
using System.Collections.Specialized;

namespace BearingCAD21
{
    public partial class frmProject : Form
    {

        #region "MEMBER VARIABLE"
        //***********************

            //....Local Class Objects:
            private clsProject mProject;
            private clsOpCond mOpCond;      //....Used in Read_CustEnqySheet().
 
            private Boolean mbln_NewProject;

            StringCollection mNo_Suffix; // = new StringCollection();
            private Boolean mbln_TxtNoValidated;

        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmProject()
            //==================
            {
                //....Constructor is called only once during its creation in modMain. 
                InitializeComponent();

                //cmdEnquiry.Visible = false;
                mbln_NewProject    = false;
                //optActive.Checked  = false; 

                Initialize_LocalObjects();

                Populate_CmbBoxes_All();
            }


            #region "Helper Routines:"
            //************************

                private void Initialize_LocalObjects()
                //=====================================
                {
                    //....PB 22JAN13. The following "Unit" assignment should be cascaded down automatically from mProject.
                    //....Project.
                    mProject         = new clsProject(clsUnit.eSystem.English);                   

                    //....Operating Conditions.
                    mOpCond = new clsOpCond();
                }
               

                private void Populate_CmbBoxes_All()
                //==================================
                {
                    LoadProducts();
                    LoadTypes();  
              
                    LoadUnits(); 

                    LoadEndConfigs(cmbEndConfig_Front);     
                    LoadEndConfigs(cmbEndConfig_Back);

                }


                #region "Sub-Helper Routines:"
                //***************************

                    private void LoadProducts()
                    //=========================
                    {
                        //.....Products: "Radial, Thrust".
                        cmbProduct.Items.Clear();
                        cmbProduct.DataSource = Enum.GetValues(typeof(clsBearing.eType));
                        cmbProduct.SelectedIndex = 0;           //....Default: Radial.
                    }


                    private void LoadTypes()
                    //======================
                    {
                        //....Radial Bearing Type: "Flexture_Pivot, Tilting_Pad, Sleeve".
                        Array pArray = Enum.GetValues(typeof(clsBearing_Radial.eDesign));
                        modMain.LoadCmbBox(cmbType, pArray);    //....Selected Index = 0; Default: Flexure_Pivot.
                    }


                    private void LoadUnits()
                    //======================
                    {
                        //....Units: English, Metric.
                        cmbUnitSystem.Items.Clear();
                        cmbUnitSystem.DataSource = Enum.GetValues(typeof(clsUnit.eSystem));
                        cmbUnitSystem.SelectedIndex = 0;        //Default: English  
                    }


                    private void LoadEndConfigs(ComboBox cmbBox_In)
                    //=============================================         
                    {
                        //....End Configs: "Seal , Thrust Brearing TL"
                        Array pArray = Enum.GetValues(typeof(clsEndConfig.eType));
                        modMain.LoadCmbBox(cmbBox_In, pArray);      //....Selected Index = 0; Default: Seal.
                    }        

                #endregion

            #endregion

        #endregion


        #region "FORM EVENT ROUTINES:"
        //*****************************

            private void frmProject_Load(object sender, EventArgs e)
            //======================================================
            {       
                DisplayData();
            }


            private void frmProject_Activated(object sender, EventArgs e)
            //============================================================
            {
               
            }

        #endregion


        #region"DISPLAY DATA:"
        //********************

            private void DisplayData()
            //=========================
            {
                if (modMain.gProject != null)
                {
                    mProject =(clsProject) modMain.gProject.Clone();
                }

                //  Unit
                //  ----
                cmbUnitSystem.Text = mProject.Unit.System.ToString();
                txtNo.Text = mProject.No;

                //  Customer
                //  --------
                txtCustomer_Name.Text = mProject.Customer.Name;                  
            
                cmbEndConfig_Front.Text = mProject.Product.EndConfig[0].Type.ToString().Replace("_", " ");
                cmbEndConfig_Back.Text = mProject.Product.EndConfig[1].Type.ToString().Replace("_", " ");
            }

        #endregion


        #region "CONTROL EVENT ROUTINES:"
        //******************************


            #region "TEXTBOX RELATED:"
            //------------------------

                private void txtBox_TextChanged(object sender, EventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtNo":
                            //-------
                            mProject.No = txtNo.Text;
                            break;

                        //case "txtNo_Suffix":
                        //    //---------------
                        //    mProject.No_Suffix = txtNo_Suffix.Text;
                        //    break;

                        case "txtCustomer_Name":
                            //------------------
                            mProject.Customer_Name = txtCustomer_Name.Text;
                            break;
                    }
                }               
        

            #region "COMBOBOX RELATED ROUTINES:"
            //----------------------------------

                private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
                //======================================================================
                {
                    if (cmbProduct.SelectedIndex != 0)
                    {
                        string pstrMsg = "In this version 'Thrust Bearing' is not supported.";
                        string pstrCaption = "Bearing Design Information";
                        MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbProduct.Text = "Radial";
                    }

                    mProject.Product.Bearing.Type = (clsBearing.eType)Enum.Parse(typeof(clsBearing.eType), cmbProduct.Text);
                }


                private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
                //====================================================================
                {
                    string pType= cmbType.Text;

                    if (cmbType.SelectedIndex != 0)                
                    {
                        string pstrMsg = "In this Version '" + cmbType.SelectedItem.ToString() + "' is not Supported";
                        string pstrCaption = "Bearing Design Information";
                        MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbType.SelectedIndex = 0;
                    }


                    if (pType == clsBearing_Radial.eDesign.Flexure_Pivot.ToString().Replace("_", " "))
                    {
                        ((clsBearing_Radial_FP)mProject.Product.Bearing).Design = clsBearing_Radial_FP.eDesign.Flexure_Pivot;
                    }
                }


                private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================
                {
                    mProject.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);
                }
              
        
                private void cmbEndConfig_SelectedIndexChanged(object sender, EventArgs e)
                //=========================================================================
                {
                    clsEndConfig.eType[] pEndConfig_Type_Existing = new clsEndConfig.eType[]{mProject.Product.EndConfig[0].Type,
                                                                                             mProject.Product.EndConfig[1].Type};
                    clsEndConfig.eType[] pEndConfig_Type_Current  = new clsEndConfig.eType[2];


                    ComboBox pCmbBox = (ComboBox)sender;
                    string pName = pCmbBox.Name;
                    int index = 0;

                    switch (pName)
                    {
                        case "cmbEndConfig_Front":
                            index = 0;
                            break;

                        case "cmbEndConfig_Back":
                            index = 1;
                            break;
                    }
                    pEndConfig_Type_Current[index] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), pCmbBox.Text.Replace(" ", "_"));

                    if (pEndConfig_Type_Current[index] != pEndConfig_Type_Existing[index])
                    {
                        if (pEndConfig_Type_Current[index] == clsEndConfig.eType.Seal)
                        {
                            mProject.Product.EndConfig[index] = new clsSeal(mProject.Unit.System, pEndConfig_Type_Current[index], modMain.gDB, mProject.Product);
                        }
                        else
                        {
                            mProject.Product.EndConfig[index] = new clsBearing_Thrust_TL(mProject.Unit.System, pEndConfig_Type_Current[index], modMain.gDB, mProject.Product);
                        }
                    }
                   
                }
        
            #endregion
        
        
        #endregion

        
            #region "FORM CLOSE RELATED:"

                private void cmdClose_Click(object sender, EventArgs e)
                //=====================================================
                {
                    Button pCmdBtn = (Button)sender;

                    switch (pCmdBtn.Name)
                    {
                        case "cmdOK":
                            //-------
                             SaveData();
                             this.Hide();
                             modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);
                             modMain.gfrmOperCond.ShowDialog();    
                             break;

                        case "cmdCancel":
                            //-----------

                            this.Hide();                               
                            break;
                    }
                }

                private void SaveData()
                //======================
                { 

                    Boolean pNewProject = true;

                    if (modMain.gProject != null)
                    {
                        if (modMain.gProject.Unit.System != (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text))
                        {
                            pNewProject = true;
                        }
                        else
                        {
                            clsEndConfig.eType[] pType = new clsEndConfig.eType[2];
                            pType[0] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                            pType[1] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));

                            for (int i = 0; i < 2; i++)
                            {
                                if (modMain.gProject.Product.EndConfig[i].Type != pType[i])
                                {
                                    pNewProject = true;
                                    break;
                                }
                                else
                                {
                                    pNewProject = false;
                                }
                            }
                        }
                    }

                    if (pNewProject)
                    {

                        clsUnit.eSystem pUnitSystem = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);

                        modMain.gProject = new clsProject(pUnitSystem);

                        modMain.gProject.No = txtNo.Text;
                        //modMain.gProject.No_Suffix = txtNo_Suffix.Text;

                        //....Product
                        //........Bearing 
                        modMain.gProject.Product.Bearing.Type = (clsBearing.eType)Enum.Parse(typeof(clsBearing.eType), cmbProduct.Text);
                        ((clsBearing_Radial)modMain.gProject.Product.Bearing).Design = (clsBearing_Radial.eDesign)Enum.Parse(typeof(clsBearing_Radial.eDesign), cmbType.Text.ToString().Replace(" ", "_"));

                        //Customer.
                        //---------
                        modMain.gProject.Customer_Name = txtCustomer_Name.Text;

                        clsEndConfig.eType[] pType = new clsEndConfig.eType[2];
                        pType[0] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                        pType[1] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));

                        for (int i = 0; i < 2; i++)
                        {
                            if (pType[i] == clsEndConfig.eType.Seal)
                            {
                                modMain.gProject.Product.EndConfig[i] = new clsSeal(modMain.gProject.Unit.System, pType[i], modMain.gDB, modMain.gProject.Product);
                            }
                            else if (pType[i] == clsEndConfig.eType.Thrust_Bearing_TL)
                            {
                                modMain.gProject.Product.EndConfig[i] = new clsBearing_Thrust_TL(modMain.gProject.Unit.System, pType[i], modMain.gDB, modMain.gProject.Product);
                            }
                        }
                    }
                }

            #endregion

        #endregion    
         
    }
}
