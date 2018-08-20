
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmProject                             '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  04JUL13                                '
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

namespace BearingCAD20
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

                cmdEnquiry.Visible = false;
                mbln_NewProject    = false;
                optActive.Checked  = false; 

                Initialize_LocalObjects();

                Populate_CmbBoxes_All();
            }


            #region "Helper Routines:"
            //************************

                private void Initialize_LocalObjects()
                //=====================================
                {
                    //....This routine is called when a new project is created & when frmProject initialized.
                

                    //....PB 22JAN13. The following "Unit" assignment should be cascaded down automatically from mProject.
                    //....Project.
                    mProject         = new clsProject(clsUnit.eSystem.English);
                    //mProject.Product = new clsProduct(mProject.Unit.System, modMain.gDB);

                    //PB 22JAN13.  The following assignments may not be necessary.
                    //mProject.Product.Unit.System = clsUnit.eSystem.English;

                    //mProject.Product.Bearing.Unit.System = clsUnit.eSystem.English;
                    //mProject.Product.EndConfig[0].Unit.System = clsUnit.eSystem.English;
                    //mProject.Product.EndConfig[1].Unit.System = clsUnit.eSystem.English;

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


                    //....Engineer Name.
                    String pWHERE = " WHERE fldPrivilege = 'Engineering'";
                    LoadUserNames(cmbEngg_Name, pWHERE);

                    //....CheckedBy Name.
                    pWHERE = " WHERE fldPrivilege = 'Engineering' OR fldPrivilege = 'Designer'";
                    LoadUserNames(cmbCheckedBy_Name, pWHERE);

                    //....DesignedBy Name.
                    pWHERE = " WHERE fldPrivilege != 'Manufacturing'";
                    LoadUserNames(cmbDesignedBy_Name, pWHERE);
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


                    private void LoadUserNames(ComboBox CmbBox_In, string WHERE_In)
                    //=============================================================
                    {
                        modMain.gDB.PopulateCmbBox(CmbBox_In, "tblUser", "fldName", WHERE_In, true);
                    }


                #endregion

            #endregion

        #endregion


        #region "FORM EVENT ROUTINES:"
        //*****************************

            private void frmProject_Load(object sender, EventArgs e)
            //======================================================
            {
                //....Whenever the form is opened, this event is fired.
                //........The "Activate" event follows this event. 
         
                //....Check whether any project has been opened or not.
                //
                if(modMain.gProject != null)
                {
                    if (modMain.gProject.No != null)
                    {
                        string pNo        = modMain.gProject.No;
                        string pNo_Suffix = modMain.gProject.No_Suffix;

                        if (modMain.gDB.ProjectNo_Exists(pNo, pNo_Suffix, "tblProject_Details"))
                        {
                            //....Project No exists in the DB.
                            //
                            //........Check the Project Status if "Open" or "Closed".
                            Boolean pblnOpen = modMain.gProject.ProjStatus_Open();

                            optActive.Checked = pblnOpen;
                            optClosed.Checked = !pblnOpen;
                        }

                        else
                        {
                            //....Project No doesn't exist in the DB.
                            if (mbln_NewProject)
                                return;
                        }
                    }
                }

                else        //....gProject = NULL. This case exists only when the user travereses to this form for the first time.
                {
                    optActive.Checked = true;

                    //....Command Button = "New".
                    //
                    if (modMain.gUser.Role == "Engineer")
                    {
                        cmdNew.Enabled = true;
                    }
                    else
                    {
                        cmdNew.Enabled = false;
                    }                                   
                }
                            
                DisplayData();
            }


            private void frmProject_Activated(object sender, EventArgs e)
            //============================================================
            {
                //...."Activate" event occurs whenever the Form becomes active e.g. when the user opens the form from another form 
                //........(this event will fire after the Load event) or when a modal form launched from this form is closed (this 
                //........event fires by itself).
                //
                if (modMain.gProject != null)
                {

                    if (optClosed.Checked)   
                    {
                        optFiltered.Checked = modMain.gProject.Filtered;
                        optAll.Checked      = !modMain.gProject.Filtered;


                        if (modMain.gProject.Filtered)
                        {
                            cmbNo.Items.Clear();
                            cmbNo.Refresh();

                            for (int i = 0; i < modMain.gProject.Filtered_Project_No.Count; i++)
                            {
                                cmbNo.Items.Add(modMain.gProject.Filtered_Project_No[i]);
                            }

                            if (modMain.gProject.Filtered_Project_No.Count > 0)
                            {
                                cmbNo.SelectedIndex = 0;
                            }
                        }
                    }
                    
                }
            }

        #endregion


        #region"DISPLAY DATA:"
        //********************

            private void DisplayData()
            //=========================
            {
                //  Unit
                //  ----
                cmbUnitSystem.Text = mProject.Unit.System.ToString(); 
                txtUnit.Text = mProject.Unit.System.ToString();          //....Only for "Closed".


                //  Customer
                //  --------
                txtCustomer_Name.Text = mProject.Customer.Name;                  
                txtCustomer_MachineDesc.Text = mProject.Customer.MachineDesc;
                txtCustomer_PartNo.Text = mProject.Customer.PartNo;


                //  End Configuration
                //  -----------------
                if (!mbln_NewProject)
                {
                    //....Existing Projects:
                    //
                    if (optActive.Checked)
                    {
                        //....Show ComboBox.
                        cmbEndConfig_Front.Text = mProject.Product.EndConfig[0].Type.ToString().Replace("_", " ");
                        cmbEndConfig_Back.Text = mProject.Product.EndConfig[1].Type.ToString().Replace("_", " ");
                    }

                    else if (optClosed.Checked)
                    {   //....Show TextBox.
                        txtEndConfig_Front.Text = mProject.Product.EndConfig[0].Type.ToString().Replace("_", " ");
                        txtEndConfig_Back.Text = mProject.Product.EndConfig[1].Type.ToString().Replace("_", " ");
                    }
                }

                else if (mbln_NewProject)
                {
                    //....New Project.
                    cmbEndConfig_Front.Text = mProject.Product.EndConfig[0].Type.ToString().Replace("_", " ");
                    cmbEndConfig_Back.Text = mProject.Product.EndConfig[1].Type.ToString().Replace("_", " ");
                }


                //  AssyDwg
                //  --------       
                txtAssyDwg_No.Text = mProject.AssyDwg.No;
                txtAssyDwg_No_Suffix.Text = mProject.AssyDwg.No_Suffix;
                txtAssyDwg_Ref.Text = mProject.AssyDwg.Ref;


                //  User Names, Initials & Dates:
                //  =============================
                //
                //....Default Date if the Date TextBox is blank.
                string pstrDefDate = DateTime.MinValue.ToShortDateString();


                //  Engineer 
                //  --------

                    //BG 28JUN13
                    //....Name
                    //if (mProject.Engg.Name != "")              
                    //    cmbEngg_Name.Text = mProject.Engg.Name;
                    //else
                    //    cmbEngg_Name.SelectedIndex = -1;

                    if (mProject.Engg.Name != "")
                    {
                        cmbEngg_Name.Text = mProject.Engg.Name;
                    }
                    else
                    {
                        if(modMain.gUser.Role == "Engineer")
                            cmbEngg_Name.SelectedIndex = cmbEngg_Name.Items.IndexOf(modMain.gUser.Name);
                        else
                            cmbEngg_Name.SelectedIndex = -1;
                    }

                    txtEngg_Name.Text = mProject.Engg.Name;     

                    //....Initials
                    txtEngg_Initials.Text = mProject.Engg.Initials;

                    //....Date.
                    if (mProject.Engg.Date.ToShortDateString() != pstrDefDate)
                        txtEngg_Date.Text = mProject.Engg.Date.ToShortDateString();
                    else
                        txtEngg_Date.Text = "";


                //  Designed By 
                //  -----------

                    ////....Name
                    //BG 28JUN13
                    //if (mProject.DesignedBy.Name != "")
                    //    cmbDesignedBy_Name.Text = mProject.DesignedBy.Name;
                    //else
                    //    cmbDesignedBy_Name.SelectedIndex = -1;

                    //BG 28JUN13
                    //....Name
                    if (mProject.DesignedBy.Name != "")
                    {
                        cmbDesignedBy_Name.Text = mProject.DesignedBy.Name;
                        txtDesignedBy_Name.Text = mProject.DesignedBy.Name;
                    }
                    else
                    {
                        if (modMain.gUser.Role == "Designer")
                        {
                            cmbDesignedBy_Name.SelectedIndex = cmbDesignedBy_Name.Items.IndexOf(modMain.gUser.Name);                            
                            txtDesignedBy_Name.Text = modMain.gUser.Name;
                        }
                        else
                        {
                            cmbDesignedBy_Name.SelectedIndex = -1;
                        }
                    }

                    //....Initials
                    txtDesignedBy_Initials.Text = mProject.DesignedBy.Initials;

                    //....Date
                    if (mProject.DesignedBy.Date.ToShortDateString() != pstrDefDate)
                        txtDesignedBy_Date.Text =
                            (mProject.DesignedBy.Date).ToShortDateString();
                    else
                        txtDesignedBy_Date.Text = "";


                //  Checked By 
                //  -----------

                    //....Name
                    //if (mProject.CheckedBy.Name != "")
                    //    cmbCheckedBy_Name.Text = mProject.CheckedBy.Name;
                    //else
                    //    cmbCheckedBy_Name.SelectedIndex = -1;

                    //BG 28JUN13
                    //....Name
                    if (mProject.CheckedBy.Name != "")
                    {
                        cmbCheckedBy_Name.Text = mProject.CheckedBy.Name;
                    }
                    else
                    {
                        if (modMain.gUser.Role == "Engineer" || modMain.gUser.Role == "Checker")
                        {
                            cmbCheckedBy_Name.SelectedIndex = cmbCheckedBy_Name.Items.IndexOf(modMain.gUser.Name);                           
                            txtCheckedBy_Name.Text = modMain.gUser.Name;
                        }
                        else
                        {
                            cmbCheckedBy_Name.SelectedIndex = -1;
                        }
                    }

                    //txtCheckedBy_Name.Text = modMain.gUser.Name;     

                    //....Initials
                    txtCheckedBy_Initials.Text = mProject.CheckedBy.Initials;

                    //....Date
                    if (mProject.CheckedBy.Date.ToShortDateString() != pstrDefDate)
                        txtCheckedBy_Date.Text = (mProject.CheckedBy.Date).ToShortDateString();
                    else
                        txtCheckedBy_Date.Text = "";
            }

        #endregion


        #region "CONTROL EVENT ROUTINES:"
        //******************************

            #region "OPTION BUTTON RELATED ROUTINES:"
            //--------------------------------------

                private void OptButton_CheckedChange(object sender, EventArgs e)
                //==============================================================
                {
                    Set_Controls_ProjectClosed();       //....Initialize.

                    if (optActive.Checked)
                    {
                        mProject.Status = "Open";

                        if (modMain.gUser.Role == "Engineer")       //BG 28JUN13
                        {
                            Set_Controls_ProjectOpen();
                        }
                        else if (modMain.gUser.Role == "Designer")
                        {
                            cmbDesignedBy_Name.Visible = true;
                            cmbCheckedBy_Name.Visible = true;
                        }
                        else if (modMain.gUser.Role == "Checker")
                        {
                            cmbCheckedBy_Name.Visible = true;
                        }
                    }
                    else if (optClosed.Checked)
                    {
                        mProject.Status = "Closed";
                    }
                                        
                    LoadProject_Nos_Per_Status_UnitSys();

                    //.....Select Project Number, if any.                                
                    if (cmbNo.Items.Count > 0)
                    {
                        if (mProject.No != null)
                            cmbNo.SelectedItem = cmbNo.Items.IndexOf(mProject.No);      
                    }

                    else
                    {
                        ResetControlVal();
                        modMain.gOpCond = new clsOpCond();    
                    }
                }


                #region "Helper Routines:"
                //------------------------

                    private void Set_Controls_ProjectOpen()
                    //=====================================
                    {
                        mbln_NewProject = false;
                        cmdNew.Visible = true;
                        grpFilter.Visible = false;
                        optFiltered.Visible = false;
                        optAll.Visible = false;
                        cmdCopy.Visible = true;


                        //  Project Number Related Control
                        //  ------------------------------
                        cmbNo.Visible = true;
                        txtNo.Visible = false;

                        cmbNo_Suffix.Visible = true;
                        cmbNo_Suffix.DropDownStyle = ComboBoxStyle.DropDownList;       //BG 03JUL13
                 
                        //cmbNo_Suffix.DropDownStyle = ComboBoxStyle.DropDownList;

                        //  Customer Name
                        //  -------------
                        txtCustomer_Name.ReadOnly = false;

                        //  Unit
                        //  ----
                        cmbUnitSystem.Visible = true;
                        txtUnit.Visible = false;

                        //  Machine Desc & Dwg Number
                        //  -------------------------

                        txtCustomer_MachineDesc.ReadOnly = false;
                        txtCustomer_PartNo.ReadOnly = false;

                        txtAssyDwg_No.ReadOnly = false;
                        txtAssyDwg_No_Suffix.ReadOnly = false;
                        txtAssyDwg_No_Suffix.BackColor = Color.White;
                        txtAssyDwg_Ref.ReadOnly = false;


                        //  End Config
                        //  ----------
                        cmbEndConfig_Front.Visible = true;
                        txtEndConfig_Front.Visible = false;

                        cmbEndConfig_Back.Visible = true;
                        txtEndConfig_Back.Visible = false;


                        //  Engg Related Control
                        //  --------------------

                        //....Name.
                        cmbEngg_Name.Visible = true;
                        txtEngg_Name.Visible = false;

                        //....Initials.
                        txtEngg_Initials.ReadOnly = false;

                        //....Date.
                        dtpEngg.Visible = true;
                        txtEngg_Date.ReadOnly = false;


                        //  Detailed By Related Control
                        //  ---------------------------

                        //....Name
                        cmbDesignedBy_Name.Visible = true;
                        txtDesignedBy_Name.Visible = false;

                        //....Initials.
                        txtDesignedBy_Initials.ReadOnly = false;

                        //....Date.
                        dtpDesignedBy.Visible = true;
                        txtDesignedBy_Date.ReadOnly = false;


                        //  Checked By Related Control
                        //  ---------------------------

                        //....Name
                        txtCheckedBy_Name.Visible = false;
                        cmbCheckedBy_Name.Visible = true;

                        //....Initials.
                        txtCheckedBy_Initials.ReadOnly = false;

                        //....Date.
                        txtCheckedBy_Date.ReadOnly = false;
                        dtpCheckedBy.Visible = true;

                        //  Diff Staus related button
                        //  -------------------------
                        cmdEnquiry.Visible = false;
                        cmdFilter.Visible = false;
                        cmdCopy.Visible = true;
                        cmdClose.Visible = true;
                        cmdOpenAgain.Visible = false;
                        cmdDelete.Visible = true;
                    }

               
                    private void Set_Controls_ProjectClosed()
                    //=======================================
                    {
                        mbln_NewProject = false;
                        cmdNew.Visible = false;
                        grpFilter.Visible = true;
                        optFiltered.Visible = true;
                        optAll.Visible = true;
                        optAll.Checked = true;

                        if (modMain.gProject != null)
                        {
                            optFiltered.Checked = modMain.gProject.Filtered;
                            optAll.Checked = !modMain.gProject.Filtered;
                        }
                        grpFilter.Enabled = false;

                        cmdCopy.Visible = false;
                        cmdOpenAgain.Left = 390;

                        //  Project Number Related Control
                        //  ------------------------------
                        cmbNo.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbNo.Visible = true;
                        txtNo.Visible = false;

                        cmbNo_Suffix.Visible = true;
                        //txtNo_Suffix.Visible = false;
                        //cmbNo_Suffix.DropDownStyle = ComboBoxStyle.DropDownList;

                        //  Customer Name, Machine Desc & Part No
                        //  -------------------------------------
                        txtCustomer_Name.ReadOnly = true;
                        txtCustomer_MachineDesc.ReadOnly = true;
                        txtCustomer_PartNo.ReadOnly = true;


                        //  Assy. Dwg Number
                        //  ----------------
                        if (optActive.Checked && modMain.gUser.Role == "Designer")
                        {
                            txtAssyDwg_No.ReadOnly = false;
                            txtAssyDwg_Ref.ReadOnly = false;
                        }
                        else
                        {
                            txtAssyDwg_No.ReadOnly = true;
                            txtAssyDwg_Ref.ReadOnly = true;
                        }

                        txtAssyDwg_No_Suffix.ReadOnly = true;
                        txtAssyDwg_No_Suffix.BackColor = txtAssyDwg_No.BackColor;


                        //  End Config
                        //  ----------
                        cmbEndConfig_Front.Visible = false;
                        txtEndConfig_Front.Visible = true;

                        cmbEndConfig_Back.Visible = false;
                        txtEndConfig_Back.Visible = true;


                        //  Engg Related Controls
                        //  --------------------

                        //...Name.
                        txtEngg_Name.Visible = true;
                        txtEngg_Name.ReadOnly = true;
                        cmbEngg_Name.Visible = false;

                        //....Initials.
                        //txtEngg_Ini.Text = "";
                        txtEngg_Initials.ReadOnly = true;

                        //....Date.
                        txtEngg_Date.ReadOnly = true;
                        dtpEngg.Visible = false;


                        //  Detailed By Related Controls
                        //  ---------------------------

                        //...Name.
                        txtDesignedBy_Name.Visible = true;
                        txtDesignedBy_Name.ReadOnly = true;
                        cmbDesignedBy_Name.Visible = false;

                        //....Initials.
                        txtDesignedBy_Initials.ReadOnly = true;

                        //....Date.
                        txtDesignedBy_Date.ReadOnly = true;
                        dtpDesignedBy.Visible = false;


                        //  Checked By Related Controls
                        //  ---------------------------

                        //...Name.
                        cmbCheckedBy_Name.Visible = false;
                        txtCheckedBy_Name.Visible = true;
                        txtCheckedBy_Name.ReadOnly = true;

                        //....Initials.
                        dtpCheckedBy.Visible = false;

                        //....Date.
                        txtCheckedBy_Initials.ReadOnly = true;
                        txtCheckedBy_Date.ReadOnly = true;


                        //  Diff Staus related button
                        //  -------------------------

                        if (modMain.gUser.Role == "Engineer")  
                        {
                            cmdFilter.Visible = true;
                            //cmdCopy.Visible = true;
                            cmdOpenAgain.Visible = true;
                        }
                        else
                        {
                            cmdFilter.Visible = false;
                            grpFilter.Visible = false;
                            cmdCopy.Visible = false;
                            cmdOpenAgain.Visible = false;
                        }

                        cmdEnquiry.Visible = false;
                        cmdClose.Visible = false;
                        cmdDelete.Visible = false;
                    }


                    private void LoadProject_Nos_Per_Status_UnitSys()
                    //===============================================
                    {
                        string pWHERE = "WHERE fldStatus = '" + mProject.Status +
                                        "' AND fldUnitSystem = '" + mProject.Unit.System + "'";

                        //....Retrieve Project Number from Database.
                        int pRecCount = modMain.gDB.PopulateCmbBox(cmbNo, "tblProject_Details", "fldNo", pWHERE, true);
                        if (pRecCount > 0)
                            cmbNo.SelectedIndex = 0;
                    }
        

                #endregion

            #endregion


            #region "DATE_TIME_PICKER RELATED:"
            //--------------------------------

                private void dtpDateTimePickers_ValueChanged(object sender, EventArgs e)
                //=======================================================================
                {
                    DateTimePicker pDtp = (DateTimePicker)sender;

                    switch (pDtp.Name)
                    {

                        case "dtpEngg":
                            //---------
                            txtEngg_Date.Text = dtpEngg.Value.ToShortDateString();
                            break;

                        case "dtpDesignedBy":
                            //--------------
                            txtDesignedBy_Date.Text = dtpDesignedBy.Value.ToShortDateString();
                            break;

                        case "dtpCheckedBy":
                            //-------------                    
                            txtCheckedBy_Date.Text = dtpCheckedBy.Value.ToShortDateString();
                            break;
                    }
                }

            #endregion


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

                        case "txtNo_Suffix":
                            //---------------
                            //mProject.No_Suffix = txtNo_Suffix.Text;
                            break;

                        case "txtCustomer_Name":
                            //------------------
                            mProject.Customer_Name = txtCustomer_Name.Text;
                            break;

                        case "txtCustomer_MachineDesc":
                            //-------------------------
                            mProject.Customer_MachineDesc = txtCustomer_MachineDesc.Text;
                            break;

                        case "txtAssyDwg_No":
                            //---------------
                            mProject.AssyDwgNo = txtAssyDwg_No.Text;
                            break;

                        case"txtAssyDwg_No_Suffix":
                            //--------------------
                            mProject.AssyDwgNo_Suffix = txtAssyDwg_No_Suffix.Text;
                            break;

                        case "txtAssyDwg_Ref":
                            //---------------
                            mProject.AssyDwgRef = txtAssyDwg_Ref.Text;
                            break;

                        case "txtEngg_Initials":
                            //-----------------
                            mProject.Engg_Initials = txtEngg_Initials.Text; 
                            break;

                        case "txtDesignedBy_Initials":
                            //-----------------------
                            mProject.DesignedBy_Initials = txtDesignedBy_Initials.Text;  
                            break;

                        case "txtCheckedBy_Initials":
                            //----------------------
                            mProject.CheckedBy_Initials = txtCheckedBy_Initials.Text; 
                            break;

                        case "txtEngg_Date":
                            //--------------
                            if (!String.IsNullOrEmpty(txtEngg_Date.Text) && CheckDate(txtEngg_Date.Text))
                                mProject.Engg_Date = Convert.ToDateTime(txtEngg_Date.Text);
                            break;

                        case "txtDesignedBy_Date":
                            //--------------------
                            if (!String.IsNullOrEmpty(txtDesignedBy_Date.Text) && CheckDate(txtDesignedBy_Date.Text))
                                mProject.DesignedBy_Date = Convert.ToDateTime(txtDesignedBy_Date.Text);
                            break;

                        case "txtCheckedBy_Date":
                            //-------------------
                            if (!String.IsNullOrEmpty(txtCheckedBy_Date.Text ) && CheckDate(txtCheckedBy_Date.Text ))
                                mProject.CheckedBy_Date = Convert.ToDateTime(txtCheckedBy_Date.Text);
                            break;
                    }
                }

                //BG 03JUL13
                private void txtNo_Validating(object sender, CancelEventArgs e)
                //==============================================================
                {
                    string pSQL = "Select fldNo_Suffix from tblProject_Details WHERE fldNo = '" + txtNo.Text + "'";

                    SqlConnection pConnection = new SqlConnection();
                    SqlDataReader pDR = modMain.gDB.GetDataReader(pSQL, ref pConnection);

                    //StringCollection pNo_Suffix = new StringCollection();
                    mNo_Suffix = new StringCollection();

                    while (pDR.Read())
                    {
                        mNo_Suffix.Add(modMain.gDB.CheckDBString(pDR, "fldNo_Suffix"));
                    }

                    cmbNo_Suffix.Text = "";
                    cmbNo_Suffix.Items.Clear();

                    const int pcStart_CharAsciiVal = 65;          //....Ascii Value for 'A'.
                    const int pcEnd_CharAsciiVal = 90;           //....Ascii Value for 'Z'.

                    for (int i = pcStart_CharAsciiVal; i <= pcEnd_CharAsciiVal; i++)
                    {
                        for (int j = 0; j < mNo_Suffix.Count; j++)
                        {
                            int pAsciiVal = Convert.ToInt16(Convert.ToChar(mNo_Suffix[j]));
                            if (pAsciiVal == i)
                               i++;
                        }
                        cmbNo_Suffix.Items.Add(Convert.ToChar(i));
                    }

                    cmbNo_Suffix.SelectedIndex = 0;

                    mbln_TxtNoValidated = true;
                }


                #region "HELPER ROUTINE:"
                //-----------------------

                private bool CheckDate(String date)
                    //=================================
                    {
                        //...This helper routine checks if the input string "date" is in a legitimate date format.
                        try
                        {
                            DateTime dt = DateTime.Parse(date);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }

                #endregion

            #endregion


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
                    //mProject.Product.Unit.System = mProject.Unit.System;

                    ////PB 22JAN13.
                    //mProject.Product.Bearing.Unit.System = mProject.Unit.System;
                    //mProject.Product.EndConfig[0].Unit.System = mProject.Unit.System;
                    //mProject.Product.EndConfig[1].Unit.System = mProject.Unit.System;
                  

                    if (!mbln_NewProject)
                    {
                        LoadProject_Nos_Per_Status_UnitSys();                                           
                    }
                }


                # region "PROJECT NUMBER RELATED:"
                //--------------------------------

                    private void cmbNo_SelectedIndexChanged(object sender, EventArgs e)
                    //==================================================================
                    {
                        //....Assign Project Number.
                        mProject.No = cmbNo.Text;

                        //....Retrieve Suffix.
                        //........If the Suffix field is NULL, "PopulateCmbBox" treats it as a blank string (e.g. "") 
                        //........and hence, this will be counted. Therefore, pNo_Suffix_Count will be always > 0 and 
                        //........No_Suffix will never be NULL (but may be "").
                        //
                        string pWHERE= "WHERE fldNo = '" + mProject.No + "'";
                        int pNo_Suffix_Count = modMain.gDB.PopulateCmbBox(cmbNo_Suffix, "tblProject_Details", "fldNo_Suffix", pWHERE, true);
                        
                        if (pNo_Suffix_Count > 0 )
                        {
                            cmbNo_Suffix.Enabled = true;

                            if (mProject.No_Suffix != null)
                                //cmbNo_Suffix.SelectedIndex = cmbNo_Suffix.Items.IndexOf(mProject.No_Suffix);      //BG 20MAR13
                                //BG 20MAR13
                                if (cmbNo_Suffix.Items.IndexOf(mProject.No_Suffix) != -1)
                                {
                                    cmbNo_Suffix.SelectedIndex = cmbNo_Suffix.Items.IndexOf(mProject.No_Suffix);
                                }
                                else
                                {
                                    cmbNo_Suffix.SelectedIndex = 0;
                                }

                            else    //....This case is unlikely.
                                cmbNo_Suffix.SelectedIndex = 0;
                        }

                        else       //....This case is unlikely.
                        {
                            cmbNo_Suffix.Enabled = false;
                        }         
                    }


                    private void cmbNo_Suffix_SelectedIndexChanged(object sender, EventArgs e)
                    //========================================================================
                    {
                        //....Assign Project Number Suffix.
                        mProject.No_Suffix = cmbNo_Suffix.Text;

                        if (!mbln_NewProject)           //BG 03JUL13
                        {

                            //....Retrieve Data.
                            //....End Config.
                            clsEndConfig.eType[] pEndConfig = new clsEndConfig.eType[2];
                            modMain.gDB.Retrieve_EndConfigs_Type(mProject, pEndConfig);

                            mProject.Product = new clsProduct(mProject.Unit.System, modMain.gDB);     //PB 22JAN13. To be reviewed later.
                            //mProject.Product.Unit.System = mProject.Unit.System;


                            for (int i = 0; i < 2; i++)
                            {
                                if (pEndConfig[i] == clsEndConfig.eType.Seal)                                 //....End Seal.
                                {
                                    mProject.Product.EndConfig[i] = new clsSeal(mProject.Unit.System, pEndConfig[i], modMain.gDB, mProject.Product);
                                }
                                else if (pEndConfig[i] == clsEndConfig.eType.Thrust_Bearing_TL)               //....End TB.
                                {
                                    mProject.Product.EndConfig[i] = new clsBearing_Thrust_TL(mProject.Unit.System, pEndConfig[i], modMain.gDB, mProject.Product);
                                }
                            }


                            if (modMain.gProject != null)
                            {
                                //if (mProject.No != modMain.gProject.No || mProject.No_Suffix != modMain.gProject.No_Suffix)
                                if (mProject.No != modMain.gProject.No || mProject.No_Suffix != modMain.gProject.No_Suffix || mProject.Status != modMain.gProject.Status)   //BG 09APR13
                                {
                                    modMain.gDB.RetrieveRecord(mProject, modMain.gOpCond);
                                }
                            }

                            else
                            {
                                modMain.gDB.RetrieveRecord(mProject, modMain.gOpCond);
                            }

                            //....Display Data.
                            DisplayData();
                        }
                    }

                    private void cmbNo_Suffix_MouseHover(object sender, EventArgs e)
                    //==============================================================
                    {
                        if (mbln_NewProject)
                        {
                            string pMsg = "";
                            if (mbln_TxtNoValidated)
                            {
                                if (mNo_Suffix.Count > 0)
                                {
                                    for (int i = 0; i < mNo_Suffix.Count; i++)
                                    {
                                        pMsg = pMsg + "'" + mNo_Suffix[i] + "',";
                                    }

                                    pMsg = pMsg.Remove(pMsg.Length - 1);
                                    toolTip1.SetToolTip(cmbNo_Suffix, pMsg + " Suffix(es) already exists for this Project No.");
                                }
                                else
                                {
                                    toolTip1.SetToolTip(cmbNo_Suffix, pMsg);
                                }
                            }
                            
                        }
                    }
                
                   
                #endregion

        
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


                    //  Instantiate new End Config object for the "index", if necessary.
                    //  ----------------------------------------------------------------
                    //
                    pEndConfig_Type_Current[index] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), pCmbBox.Text.Replace(" ", "_"));

                    if (pEndConfig_Type_Current[index] != pEndConfig_Type_Existing[index])
                    {
                        if (pEndConfig_Type_Current[index] == clsEndConfig.eType.Seal)
                        {
                            mProject.Product.EndConfig[index] = new clsSeal(mProject.Unit.System, pEndConfig_Type_Current[index], modMain.gDB, mProject.Product);
                            //MessageBox.Show( index.ToString() + " == Seal");
                        }
                        else
                        {
                            mProject.Product.EndConfig[index] = new clsBearing_Thrust_TL(mProject.Unit.System, pEndConfig_Type_Current[index], modMain.gDB, mProject.Product);
                            //MessageBox.Show(index.ToString() + " == T/B");
                        }
                    }
                   

                    //....Check if the current end configs match with the DB end configs.
                    clsEndConfig.eType[] pEndConfig_DB = new clsEndConfig.eType[2];
                    modMain.gDB.Retrieve_EndConfigs_Type(mProject, pEndConfig_DB);
                                       
                    if (pEndConfig_DB[0] == mProject.Product.EndConfig[0].Type &&
                        pEndConfig_DB[1] == mProject.Product.EndConfig[1].Type &&

                        (pEndConfig_Type_Existing[0] != mProject.Product.EndConfig[0].Type ||
                         pEndConfig_Type_Existing[1] != mProject.Product.EndConfig[1].Type))
                    {
                        //....Retrieve data from the database which will erase any changes made so far. 
                        modMain.gDB.RetrieveRecord(mProject, modMain.gOpCond);
                    }
                }
        

                private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
                //========================================================================
                {
                    //....This Routine Retrives Initials from Database.
                
                    ComboBox pCmbBox = (ComboBox)sender;
                    string pName = pCmbBox.Text;

                    String pSQL = "SELECT fldInitials FROM tblUser WHERE fldName = '" + pName + "'";
                
                    SqlConnection pConnection = new SqlConnection();   
                    SqlDataReader pDR = modMain.gDB.GetDataReader(pSQL,ref pConnection);

                    if (pDR.Read())         
                    {
                        switch (pCmbBox.Name)
                        {
                            case "cmbEngg_Name":
                                if (pCmbBox.Text != "")
                                {
                                    mProject.Engg_Name = pCmbBox.Text;                                         
                                    txtEngg_Initials.Text = modMain.gDB.CheckDBString(pDR, "fldInitials");           
                                }
                                break;

                            case "cmbDesignedBy_Name":
                                if (pCmbBox.Text != "")
                                {
                                    mProject.DesignedBy_Name = pCmbBox.Text;                                    
                                    txtDesignedBy_Initials.Text = modMain.gDB.CheckDBString(pDR, "fldInitials");     
                                }
                                break;

                            case "cmbCheckedBy_Name":
                                if (pCmbBox.Text != "")
                                {
                                    mProject.CheckedBy_Name = pCmbBox.Text;                                     
                                    txtCheckedBy_Initials.Text = modMain.gDB.CheckDBString(pDR, "fldInitials");      
                                }
                                break;
                        }
                    }
                    pDR.Close();
                    pConnection.Close();
                }
                  
            #endregion
        
        
        #endregion


        #region"COMMAND BUTTON EVENT ROUTINES:"
        //------------------------------------
             
            private void cmdDelete_Click(object sender, EventArgs e)
            //======================================================       
            {
                int pAns = (int)MessageBox.Show("Do you want to delete the record of the Project No."
                                + cmbNo.Text + "?", "Delete Record", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

                if (pAns == pAnsY)
                {
                    if (cmbNo.Text != "")
                    {
                        Cursor = Cursors.WaitCursor;

                        string pProjNo = cmbNo.Text;
                        int pSelectedIndx = cmbNo.SelectedIndex;

                        mProject.No = cmbNo.Text;
                                            
                        mProject.No_Suffix = cmbNo_Suffix.Text;
                        modMain.gDB.DeleteRecord(mProject);

                        cmbNo.Items.RemoveAt(pSelectedIndx);
                        //cmbNo.SelectedIndex = 0;              //BG 20MAR13

                        //....Repopulate Project Number
                        LoadProject_Nos_Per_Status_UnitSys();   //BG 20MAR13

                        Cursor = Cursors.Default;       
                    }
                    else if (cmbNo.Text == "")
                    {
                        MessageBox.Show("Please Select a Project Number.");
                    }
                }
            }


            private void cmdFilter_Click(object sender, EventArgs e)
            //======================================================
            {
                //CheckProjectNo();
                SaveData_GlobalObj();

                modMain.gProject.Status = mProject.Status;

                modMain.gfrmFilter.ShowDialog();
            }


            private void cmdCopy_Click(object sender, EventArgs e)
            //====================================================
            {
                //CheckProjectNo();
                SaveData_GlobalObj();

                modMain.gfrmCopyProject.ShowDialog();

                if (modMain.gProject.No != null)
                    mProject = (clsProject)modMain.gProject.Clone();

                mProject.No = cmbNo.Text;
                mProject.No_Suffix = cmbNo_Suffix.Text;
                DisplayData();
            }


            private void cmdCloseProject_Click(object sender, EventArgs e)
            //============================================================
            {
                string pstrWHERE = "";

                if (cmbNo.Text != "")
                {
                    Cursor = Cursors.WaitCursor;

                    string pProjNo = cmbNo.Text;
                    int pSelectedIndx = cmbNo.SelectedIndex;

                    string pProNo_Suffix = cmbNo_Suffix.Text;

                    if (pProNo_Suffix != "")
                        pstrWHERE = " WHERE fldNo = '" + pProjNo + "' AND fldNo_Suffix = '" + pProNo_Suffix + "'";
                    else
                        pstrWHERE = " WHERE fldNo = '" + pProjNo + "' AND fldNo_Suffix is NULL";

                    string pSQL = "UPDATE tblProject_Details SET fldStatus = 'Closed', fldDate_Closing = " +
                                    "GetDate()" + pstrWHERE;
                    modMain.gDB.ExecuteCommand(pSQL);

                    cmbNo.Items.RemoveAt(pSelectedIndx);
                    cmbNo.SelectedIndex = 0;

                    Cursor = Cursors.Default;
                }
                else if (cmbNo.Text == "")
                {
                    MessageBox.Show("Please Select a Project Number.");
                }
            }


            #region "NEW RELATED:"

                private void cmdNew_Click(object sender, EventArgs e)
                //====================================================          
                {
                    cmdEnquiry.Visible = true;
                    mbln_NewProject = true;

                    //.....Status = "Open"
                    mProject.Status = "Open";

                    //....Set Control for New Project.
                    Set_Controls_New();

                    //....Initialize All LocalObject
                    Initialize_LocalObjects();

                    //....Reset Control To Blank.
                    ResetControlVal();

                    modMain.gOpCond = new clsOpCond();
                }

               
                #region "Helper Routines:"
                //------------------------

                    private void Set_Controls_New()
                    //============================= 
                    {
                        //  Project Number Related Control
                        //  ------------------------------
                        txtNo.Visible = true;
                        txtNo.Text = "";
                        cmbNo.Visible = false;

                        cmbNo_Suffix.DropDownStyle = ComboBoxStyle.DropDown;
                        cmbNo_Suffix.Items.Clear();

                        //BG 03JUl13
                        const int pcStart_CharAsciiVal = 65;          //....Ascii Value for 'A'.
                        const int pcEnd_CharAsciiVal = 90;           //....Ascii Value for 'Z'.

                        for (int i = pcStart_CharAsciiVal; i <= pcEnd_CharAsciiVal; i++)
                        {
                           cmbNo_Suffix.Items.Add(Convert.ToChar(i));
                        }
                        cmbNo_Suffix.SelectedIndex = 0;


                        //  Customer Name
                        //  -------------
                        txtCustomer_Name.Text = "";
                        txtCustomer_Name.ReadOnly = false;
                        txtCustomer_PartNo.Text = "";

                        //  Unit
                        //  ----
                        cmbUnitSystem.Visible = true;
                        txtUnit.Visible = false;

                        //  Machine Desc & Dwg Number
                        //  -------------------------

                        //....Machine Desc.
                        txtCustomer_MachineDesc.Text = "";
                        txtCustomer_MachineDesc.ReadOnly = false;

                        //....Dwg No.
                        txtAssyDwg_No.Text = "";
                        txtAssyDwg_No.ReadOnly = false;

                        //....Dwg No. Suffix.
                        txtAssyDwg_No_Suffix.Text = "";

                        //....Dwg Ref.
                        txtAssyDwg_Ref.Text = "";
                        txtAssyDwg_Ref.ReadOnly = false;

                        //  End Config
                        //  ----------
                        cmbEndConfig_Front.Visible = true;
                        txtEndConfig_Front.Visible = false;

                        cmbEndConfig_Back.Visible = true;
                        txtEndConfig_Back.Visible = false;

                        cmbEndConfig_Front.Enabled = true;
                        cmbEndConfig_Back.Enabled = true;

                        //  Engg Related Control
                        //  --------------------

                        //....Name
                        cmbEngg_Name.Visible = true;
                        txtEngg_Name.Visible = false;
                        txtEngg_Name.ReadOnly = false;

                        //....Initials
                        txtEngg_Initials.ReadOnly = false;

                        //....Date
                        txtEngg_Date.ReadOnly = false;
                        txtEngg_Date.Text = DateTime.Today.ToShortDateString();
                        dtpEngg.Visible = true;

                        //  Designed By Related Control
                        //  ---------------------------

                        //....Name
                        cmbDesignedBy_Name.Visible = true;
                        txtDesignedBy_Name.Visible = false;
                        txtDesignedBy_Name.ReadOnly = false;

                        //....Initials
                        txtDesignedBy_Initials.ReadOnly = false;

                        //....Date
                        txtDesignedBy_Date.ReadOnly = false;
                        dtpDesignedBy.Visible = true;


                        //  Checked By Related Control
                        //  ---------------------------

                        //....Name
                        cmbCheckedBy_Name.Visible = true;
                        txtCheckedBy_Name.ReadOnly = false;
                        txtCheckedBy_Name.Visible = false;

                        //....Initials
                        txtCheckedBy_Initials.ReadOnly = false;

                        //....Date
                        //txtCheckedby_Date.Text = "";
                        txtCheckedBy_Date.ReadOnly = false;
                        dtpCheckedBy.Visible = true;

                        //  Diff Staus related button
                        //  -------------------------
                        cmdEnquiry.Visible = true;
                        cmdFilter.Visible = false;
                        cmdCopy.Visible = false;
                        cmdClose.Visible = false;
                        cmdOpenAgain.Visible = false;
                        cmdDelete.Visible = false;
                    }


                    private void ResetControlVal()
                    //============================ 
                    {
                        //.....blank string
                        const string pcBlank = "";

                        txtNo.Text = pcBlank;                           //....Project Number 
                        //txtNo_Suffix.Text = pcBlank;
                        //txtCust.Text = "Kobe";                
                        txtCustomer_Name.Text = pcBlank;                //....Customer Name 
                        txtCustomer_MachineDesc.Text = pcBlank;         //....Machine Desc
                        txtCustomer_PartNo.Text = pcBlank;              //....PartNo

                        txtAssyDwg_No.Text = pcBlank;                   //....Dwg No
                        txtAssyDwg_No_Suffix.Text = pcBlank;            //....Dwg No Suffix    
                        txtAssyDwg_Ref.Text = pcBlank;                  //....DwgRef No

                        //  Engg
                        //  =====
                        //BG 28JUN13
                        //cmbEngg_Name.Text = pcBlank;                            //...Name
                        //cmbEngg_Name.SelectedIndex = -1;

                        //BG 28JUN13
                        if (modMain.gUser.Role == "Engineer")
                        {
                            cmbEngg_Name.SelectedIndex = cmbEngg_Name.Items.IndexOf(modMain.gUser.Name);
                        }
                        else
                        {
                            cmbEngg_Name.Text = pcBlank;
                            cmbEngg_Name.SelectedIndex = -1;
                            txtEngg_Initials.Text = pcBlank;                        //...Initials       //BG 28JUN13
                        }

                        txtEngg_Name.Text = pcBlank;
                        //txtEngg_Initials.Text = pcBlank;                        //...Initials     //BG 28JUN13
                        txtEngg_Date.Text = DateTime.Today.ToShortDateString(); //...Date   

                        //  Designed By
                        //  ============
                        cmbDesignedBy_Name.Text = pcBlank;                      //...Name
                        cmbDesignedBy_Name.SelectedIndex = -1;

                        txtDesignedBy_Name.Text = pcBlank;
                        txtDesignedBy_Initials.Text = pcBlank;                  //...Initials
                        txtDesignedBy_Date.Text = pcBlank;                      //...Date

                        //  Checked By
                        //  ===========
                        cmbCheckedBy_Name.Text = pcBlank;                       //...Name
                        cmbCheckedBy_Name.SelectedIndex = -1;

                        txtCheckedBy_Name.Text = pcBlank;
                        txtCheckedBy_Initials.Text = pcBlank;                   //...Initials
                        txtCheckedBy_Date.Text = pcBlank;                       //...Date
                    }

                #endregion


                private void cmdEnquiry_Click(object sender, EventArgs e)
                //=======================================================
                {
                    openFileDialog1.Filter = "BearingCAD Customer Files (*.pdf)|*.pdf";
                    openFileDialog1.FilterIndex = 1;
                    //openFileDialog1.InitialDirectory = modMain.gFiles.DirInputFiles;
                    openFileDialog1.FileName = "";
                    openFileDialog1.Title = "Open";

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        mProject.FilePath_CustEnqySheet = openFileDialog1.FileName;
                        mProject.Read_CustEnqySheet(modMain.gOpCond);

                        txtCustomer_Name.Text = mProject.Customer.Name;      //BG 30DEC11                  
                        txtCustomer_MachineDesc.Text = mProject.Customer.MachineDesc;    //BG 30DEC11
                    }
                }


            #endregion


            private void cmdOpenAgain_Click(object sender, EventArgs e)
            //=========================================================     
            {
                string pstrWHERE = "";

                //.....Project Number.
                string pProjNo = cmbNo.Text;

                //.....Check radio button Open.
                optActive.Checked = true;

                string pProNo_Suffix = cmbNo_Suffix.Text;

                if (pProNo_Suffix != "")
                    pstrWHERE = "WHERE fldNo = '" + pProjNo + "' AND fldNo_Suffix = '" + pProNo_Suffix + "'";
                else
                    pstrWHERE = "WHERE fldNo = '" + pProjNo + "' AND fldNo_Suffix is NULL";

                string pSQL = "UPDATE tblProject_Details SET fldStatus = 'Open', fldDate_Closing = Null  " + pstrWHERE;
                modMain.gDB.ExecuteCommand(pSQL);

                //.....Add Project number.
                cmbNo.Items.Add(pProjNo);
              
                int pIndx = cmbNo.Items.IndexOf(pProjNo);
                cmbNo.SelectedIndex = pIndx;
                         
            }


            #region "FORM CLOSE RELATED:"

                private void cmdClose_Click(object sender, EventArgs e)
                //=====================================================
                {
                    Button pCmdBtn = (Button)sender;

                    switch (pCmdBtn.Name)
                    {
                        case "cmdOK":
                            //-------
                            //....Set Cursor To WaitCursor.
                            Cursor = Cursors.WaitCursor;

                            if (txtAssyDwg_No_Suffix.Text != "")
                            {
                                int pNo_Suffix = Convert.ToInt32(txtAssyDwg_No_Suffix.Text);
                                if (pNo_Suffix < 1 || pNo_Suffix > 97)
                                {
                                    MessageBox.Show("Please enter Assy.Dwg.No.Suffix between 1 to 97.","ERROR",
                                                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                                    txtAssyDwg_No_Suffix.Focus();
                                    txtAssyDwg_No_Suffix.Text = "";
                                    Cursor = Cursors.Default;
                                    return;
                                }
                            }

                            if (Save_IfValidProjectNo())               //....CheckProjectNo() Routine Check 
                            {                                   //....Whether Project is New or not.
                                Cursor = Cursors.Default;
                                this.Hide();
                                modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);
                                //modMain.gfrmImportData.ShowDialog();      //BG 10JAN13
                                modMain.gfrmOperCond.ShowDialog();    
                                break;
                            }
                            else
                            {
                                Cursor = Cursors.Default;
                                return;
                            }


                        case "cmdCancel":
                            //-----------

                            this.Hide();                               
                            break;
                    }
                }


                #region "Helper Routines:"
                //------------------------
                          
                    private Boolean Save_IfValidProjectNo ()  
                    //======================================
                    {
                        Boolean pblnExists = false;

                        if (mbln_NewProject)
                        {
                            //  New Project:
                            //  ------------
                            //
                            if (txtNo.Text != "")
                            {
                                //if (txtNo_Suffix.Text != "")            //BG 28JUN13
                                if (cmbNo_Suffix.Text != "")            //BG 28JUN13
                                {

                                    //.....Check Project No: if New or Not.
                                    //if (!modMain.gDB.ProjectNo_Exists(txtNo.Text, cmbNo_Suffix.Text, "tblProject_Details"))  //BG 20MAR13
                                    //if (!modMain.gDB.ProjectNo_Exists(txtNo.Text, txtNo_Suffix.Text, "tblProject_Details"))     //BG 20MAR13
                                    if (!modMain.gDB.ProjectNo_Exists(txtNo.Text, cmbNo_Suffix.Text, "tblProject_Details"))     //BG 03JUL13
                                    {
                                        SaveData_GlobalObj();                     //....Save Data. why?

                                        //....Return true;
                                        pblnExists = true;
                                        return pblnExists;
                                    }

                                    else
                                    {
                                        MessageBox.Show("Project Number already exists in the database. \n" +
                                                        "Please insert another Project Number.",
                                                        "Project Details",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtNo.Focus();
                                        return pblnExists;
                                    }
                                }
                                else
                                {
                                    //BG 28JUN13
                                    MessageBox.Show("Project Number Suffix can't be blank.\n" +
                                               "Please Insert a Project Number Suffix.",
                                               "Project Details",
                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //txtNo_Suffix.Focus();
                                }
                            }

                            else
                            {
                                MessageBox.Show("Project Number can't be blank.\n" +
                                                "Please Insert a Project Number.",
                                                "Project Details",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtNo.Focus();
                                return pblnExists;
                            }
                        }

                        else
                        {
                            //  Existing Project:
                            //  -----------------
                            //
                            SaveData_GlobalObj(); 
                            pblnExists = true;
                        }

                        return pblnExists;
                    }


                    //private bool CheckDwgNo()
                    ////=======================
                    //{
                    //    bool pBlnChkDwgNo = System.Text.RegularExpressions.Regex.IsMatch(txtAssyDwg_No.Text, @"^R\d{4}$");
                    //    return pBlnChkDwgNo;
                    //}


                    private void SaveData_GlobalObj()
                    //===============================       
                    {                       
                        if (modMain.gProject == null || mbln_NewProject)
                        {
                            clsUnit.eSystem pUnitSystem = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);

                            modMain.gProject = new clsProject(pUnitSystem);
                            //modMain.gProject.Product = new clsProduct(modMain.gProject.Unit.System, modMain.gDB);

                            //modMain.gProject.Product.Unit.System = modMain.gProject.Unit.System;

                            ////PB 22JAN13.
                            //modMain.gProject.Product.Bearing.Unit.System = modMain.gProject.Unit.System;
                            //modMain.gProject.Product.EndConfig[0].Unit.System = modMain.gProject.Unit.System;
                            //modMain.gProject.Product.EndConfig[1].Unit.System = modMain.gProject.Unit.System;


                            //....BG 21JAN13  The EndConfig in Bearing is not Saving in new case

                            if (mbln_NewProject)
                            {
                                modMain.gProject.No = txtNo.Text;
                               //modMain.gProject.No_Suffix = txtNo_Suffix.Text;        //BG 03JUL13
                            }
                            else
                            {
                                modMain.gProject.No = cmbNo.Text;
                                //modMain.gProject.No_Suffix = cmbNo_Suffix.Text;       //BG 03JUL13
                            }

                            modMain.gProject.No_Suffix = cmbNo_Suffix.Text;         //BG 03JUL13


                            //....Product
                            //........Bearing 
                            modMain.gProject.Product.Bearing.Type                        = (clsBearing.eType)Enum.Parse(typeof(clsBearing.eType), cmbProduct.Text);
                            ((clsBearing_Radial)modMain.gProject.Product.Bearing).Design = (clsBearing_Radial.eDesign)Enum.Parse(typeof(clsBearing_Radial.eDesign), cmbType.Text.ToString().Replace(" ", "_"));

                            
                            clsEndConfig.eType[] pType = new clsEndConfig.eType[2];
                            pType[0] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                            pType[1] = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));
                            

                            //modMain.gProject.Product.EndConfig[0].Type = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                            //modMain.gProject.Product.EndConfig[1].Type = (clsEndConfig.eType)Enum.Parse(typeof(clsEndConfig.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));


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

                            if (!mbln_NewProject)
                            {
                                modMain.gProject = (clsProject)mProject.Clone();
                            }
                        }

                        else
                        {
                            modMain.gProject = (clsProject)mProject.Clone();
                        }
                        


                        //....Project No. Suffix
                        //modMain.gProject.No_Suffix = cmbNo_Suffix.Text;                   

                        //....Set Project Status.
                        if (mbln_NewProject || optActive.Checked) 
                            modMain.gProject.Status = "Open";
                        else 
                            modMain.gProject.Status = "Closed";
                                            

                        //Customer.
                        //---------
                        modMain.gProject.Customer_Name = txtCustomer_Name.Text;
                        modMain.gProject.Customer_MachineDesc = txtCustomer_MachineDesc.Text;
                        modMain.gProject.Customer_PartNo = txtCustomer_PartNo.Text;
                        
                
                        // AssyDwg.
                        // --------
                        modMain.gProject.AssyDwgNo = txtAssyDwg_No.Text;
                        if (txtAssyDwg_No_Suffix.Text != "")        //AM 26APR12
                        {
                            int pNo_Suffix = Convert.ToInt32(txtAssyDwg_No_Suffix.Text);
                            if (pNo_Suffix <= 9 && txtAssyDwg_No_Suffix.Text.Length==1)
                            {
                                modMain.gProject.AssyDwgNo_Suffix = "0" + txtAssyDwg_No_Suffix.Text;
                            }
                            else
                            {
                                modMain.gProject.AssyDwgNo_Suffix = txtAssyDwg_No_Suffix.Text;
                            }
                        }
                        else
                        {
                            modMain.gProject.AssyDwgNo_Suffix = txtAssyDwg_No_Suffix.Text;
                        }

                        modMain.gProject.AssyDwgRef = txtAssyDwg_Ref.Text;


                        //  Engg
                        //  -----
                        modMain.gProject.Engg_Name = cmbEngg_Name.Text;                          //...Name
                        modMain.gProject.Engg_Initials = txtEngg_Initials.Text;                  //...Initials
                        if (txtEngg_Date.Text != "")                                             //...Date
                            modMain.gProject.Engg_Date = Convert.ToDateTime(txtEngg_Date.Text);             
                        else
                            modMain.gProject.Engg_Date = DateTime.MinValue;


                        //  Designed By
                        //  -----------
                        modMain.gProject.DesignedBy_Name = cmbDesignedBy_Name.Text;               //...Name
                        modMain.gProject.DesignedBy_Initials = txtDesignedBy_Initials.Text;       //...Initials
                        if (txtDesignedBy_Date.Text != "")                                        //...Date                   
                            modMain.gProject.DesignedBy_Date = Convert.ToDateTime(txtDesignedBy_Date.Text); 
                        else
                            modMain.gProject.DesignedBy_Date = DateTime.MinValue; 
                          

                        //  Checked By
                        //  ----------
                        modMain.gProject.CheckedBy_Name = cmbCheckedBy_Name.Text;                 //...Name
                        modMain.gProject.CheckedBy_Initials = txtCheckedBy_Initials.Text;         //...Initials 
                        if (txtCheckedBy_Date.Text != "")                                         //...Date
                            modMain.gProject.CheckedBy_Date = Convert.ToDateTime(txtCheckedBy_Date.Text);   
                        else
                            modMain.gProject.CheckedBy_Date = DateTime.MinValue; 

                    
                        //  Closing Date
                        //  -----------
                        if (optClosed.Checked)
                            modMain.gProject.Date_Closing = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                        else
                            modMain.gProject.Date_Closing = DateTime.MinValue;
                    }

                #endregion


            #endregion


            private void cmdPrint_Click(object sender, EventArgs e)
            //======================================================
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                modMain.CaptureScreen(this);
                pd.Print();
            }

        #endregion    
         
    }
}
