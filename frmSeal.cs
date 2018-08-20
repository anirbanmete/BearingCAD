
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSeal                                '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03AUG18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       frmSeal_Load                        ()
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace BearingCAD21
{
    public partial class frmSeal : Form
    {
        #region "MEMBER VARIABLE DECLARATION:"
        //************************************
        //private const string mcBladeThickness = "0.060";   // Move to clsSeal SG 06MAR12
        //private const string mcLiningThickness = "0.030"; 

        //....Local Class Object
        private clsSeal[] mEndSeal = new clsSeal[2];

        private ComboBox[] mcmbSealType;
        private TextBox[] mtxtDBore;
        private ComboBox[] mcmbBladeCount;
        private TextBox[] mtxtBladeT;
        private ComboBox[] mcmbBladeAngleTaper;
        private ComboBox[] mcmbMatBase;
        private ComboBox[] mcmbMatLining;
        private TextBox[] mtxtMatLiningT;

        private Boolean mblnDBore_Min_Front_Changed = false;
        private Boolean mblnDBore_Max_Front_Changed = false;

        private Boolean mblnDBore_Min_Back_Changed = false;
        private Boolean mblnDBore_Max_Back_Changed = false;

        #endregion


        #region" FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

        public frmSeal()
        //===============
        {
            InitializeComponent();

            //....Set Local Object.
            //SetLocalObject();

            //....Set Seal Type
            mcmbSealType = new ComboBox[] { cmbType_Front, cmbType_Back };
            //.....Set BoreD TextBox.
            mtxtDBore = new TextBox[] { txtDBore_Range_Min_Front, txtDBore_Range_Max_Front, txtDBore_Range_Min_Back, txtDBore_Range_Max_Back };
            //....Blade Count
            mcmbBladeCount = new ComboBox[] { cmbBlade_Count_Front, cmbBlade_Count_Back };
            //....Blade Thickness
            mtxtBladeT = new TextBox[] { txtBlade_T_Front, txtBlade_T_Back };
            //....Blade Angle Taper
            mcmbBladeAngleTaper = new ComboBox[] { cmbBlade_AngTaper_Front, cmbBlade_AngTaper_Back };
            //....Material Base
            mcmbMatBase = new ComboBox[] { cmbMat_Base_Front, cmbMat_Base_Back };
            //....Material Lining 
            //mchkMatLining = new CheckBox[] { chkMat_Lining_Front, chkMat_Lining_Back };
            mcmbMatLining = new ComboBox[] { cmbMat_Lining_Front, cmbMat_Lining_Back };
            //....Material Thick
            mtxtMatLiningT = new TextBox[] { txtMat_LiningT_Front, txtMat_LiningT_Back };
        
            //.....Load Seal Type.
            LoadSealType();

            //....Taper Angle
            LoadTaperAngle();

            //.....Populate Base and Lining Material.
            PopulateMaterial();
           
        }


        private void LoadSealType()
        //=========================         
        {
            for (int i = 0; i < 2; i++)
            {
                //mcmbSealType[i].DataSource = Enum.GetValues(typeof(clsSeal.eSealType));
                mcmbSealType[i].DataSource = Enum.GetValues(typeof(clsSeal.eDesign));
                mcmbSealType[i].SelectedIndex = 0;
            }
        }

        private void LoadTaperAngle()
        //============================
        {
            int[] pAngle = new int[] { 45, 50, 55, 60 };

            for (int i = 0; i < 2; i++)
            {
                mcmbBladeAngleTaper[i].Items.Clear();

                for (int j = 0; j < pAngle.Length; j++)
                {
                    mcmbBladeAngleTaper[i].Items.Add(pAngle[j].ToString());
                }
            }
        }


        private void PopulateMaterial()
        //==============================
        {
            for (int i = 0; i < 2; i++)
            {
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                //....Base Material.
                var pQryBaseMat = (from pRec in pBearingDBEntities.tblData_Mat
                                   where
                                       pRec.fldBase == true
                                   orderby pRec.fldName ascending
                                   select pRec).ToList();
                mcmbMatBase[i].Items.Clear();
                if (pQryBaseMat.Count() > 0)
                {
                    for (int j = 0; j < pQryBaseMat.Count; j++)
                    {
                        mcmbMatBase[i].Items.Add(pQryBaseMat[j].fldName);
                    }
                    mcmbMatBase[i].SelectedIndex = 0;
                }

                //....Lining Material.
                var pQryLiningMat = (from pRec in pBearingDBEntities.tblData_Mat
                                     where
                                         pRec.fldLining == true
                                     orderby pRec.fldName ascending
                                     select pRec).ToList();
                mcmbMatLining[i].Items.Clear();
                if (pQryBaseMat.Count() > 0)
                {
                    for (int k = 0; k < pQryLiningMat.Count; k++)
                    {
                        mcmbMatLining[i].Items.Add(pQryLiningMat[k].fldName);
                    }
                    mcmbMatLining[i].SelectedIndex = 0;
                }
            };

        }

        #endregion


        #region" FORM RELATED ROUTINES:"
        //******************************

        private void frmSeal_Load(object sender, EventArgs e)
        //===================================================
        {
            //....Taper Angle
            LoadTaperAngle();

            //....Set Local Object.
            SetLocalObject();

            //....Set Tab Pages.
            SetTabPages();

            //....Diaplay Data.
            DisplayData();

            //....Set Control.         
            SetControl_MatLining();
        }


        private void SetLocalObject()
        //===========================
        {
            mEndSeal = new clsSeal[2];

            for (int i = 0; i < 2; i++)
            {
                if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                {
                    mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndConfig[i])).Clone();
                }
            }
        }


        private void SetTabPages()
        //========================
        {
            TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

            tbEndSealData.TabPages.Clear();

            for (int i = 0; i < 2; i++)
            {
                if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                    tbEndSealData.TabPages.Add(pTabPages[i]);
            }

        }

        private void DisplayData()
        //========================
        {
            for (int i = 0; i < 2; i++)
            {
                if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                {

                    //....Seal Type
                    mcmbSealType[i].Text = mEndSeal[i].Design.ToString();

                    //....Bore Dia
                    int k = 0;
                    if (i == 1) k = 2;

                    for (int j = 0; j < 2; j++, k++)
                    {
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mtxtDBore[k].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndSeal[i].DBore_Range[j]));
                        }
                        else
                        {
                            mtxtDBore[k].Text = modMain.gProject.Unit.WriteInUserL(mEndSeal[i].DBore_Range[j]);
                        }
                    }

                    //  Blade
                    //  -----

                    //....Count.
                    mcmbBladeCount[i].Text = modMain.ConvIntToStr(mEndSeal[i].Blade.Count);

                    if (mEndSeal[i].Blade.Count != 0)
                    {
                        //....Thick
                        //if(mEndSeal[i].Blade.Count > 1)
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mtxtBladeT[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndSeal[i].Blade.T));
                        }
                        else
                        {
                            mtxtBladeT[i].Text = modMain.gProject.Unit.WriteInUserL(mEndSeal[i].Blade.T);
                        }

                        //....Blade Taper Angle.
                        //else if(mEndSeal[i].Blade.Count == 1)
                        if (mEndSeal[i].Blade.Count == 1)
                        {
                            if (mcmbBladeAngleTaper[i].Items.Contains(mEndSeal[i].Blade.AngTaper.ToString()))
                            {
                                mcmbBladeAngleTaper[i].SelectedIndex = mcmbBladeAngleTaper[i].Items.IndexOf(mEndSeal[i].Blade.AngTaper.ToString());
                            }

                            else
                            {
                                mcmbBladeAngleTaper[i].Text = modMain.ConvDoubleToStr(mEndSeal[i].Blade.AngTaper, "");
                            }
                        }

                    }


                    //  Material
                    //  --------
                    mcmbMatBase[i].Text = mEndSeal[i].Mat.Base;
                    mcmbMatLining[i].Text = mEndSeal[i].Mat.Lining;
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mtxtMatLiningT[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndSeal[i].Mat_LiningT));
                    }
                    else
                    {
                        mtxtMatLiningT[i].Text = modMain.gProject.Unit.WriteInUserL(mEndSeal[i].Mat_LiningT);
                    }
                    SetMatLiningT(mcmbMatLining[i].Text);
                }
            }

        }


        private void SetTxtForeColorAndDefVal(Double T_In, TextBox TxtBox_In, Double DefVal_In)
        //======================================================================================     
        {
            if (T_In != 0.000)
            {
                if (Math.Abs(T_In - DefVal_In) < modMain.gcEPS)
                    TxtBox_In.ForeColor = Color.Magenta;
                else
                    TxtBox_In.ForeColor = Color.Black;
            }
            else
            {
                TxtBox_In.ForeColor = Color.Magenta;
                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    TxtBox_In.Text = modMain.gProject.Unit.CEng_Met(DefVal_In).ToString("#0.000");
                }
                else
                {
                    TxtBox_In.Text = DefVal_In.ToString("#0.000");
                }
            }
        }

        private void SetTxtForeColor(TextBox TxtBox_In, Double DefVal_In)
        //===============================================================  
        {
            if (Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - DefVal_In) < modMain.gcEPS)
            {
                TxtBox_In.ForeColor = Color.Magenta;
            }
            else
            {
                TxtBox_In.ForeColor = Color.Black;
            }
        }


        private void SetControl_MatLining()
        //=================================
        {
            Boolean[] pblnVisible = new Boolean[2];

            for (int i = 0; i < 2; i++)
            {
                if (mEndSeal[i] != null)
                {
                    if (mEndSeal[i].Design == clsSeal.eDesign.Fixed)
                    {
                        pblnVisible[i] = true;
                    }
                    else
                    {
                        pblnVisible[i] = false;
                    }
                }
            }

            lblMat_Lining_Front.Visible = !pblnVisible[0];
            cmbMat_Lining_Front.Visible = !pblnVisible[0];

            lblMat_LiningT_Front.Visible = !pblnVisible[0];
            txtMat_LiningT_Front.Visible = !pblnVisible[0];

            lblMat_Lining_Back.Visible = !pblnVisible[1];
            lblMat_LiningT_Back.Visible = !pblnVisible[1];

            txtMat_LiningT_Back.Visible = !pblnVisible[1];
            cmbMat_Lining_Back.Visible = !pblnVisible[1];
        }



        private void SetMatLiningT(string MatLining_In)
        //====================================================
        {
            if (MatLining_In != "None")
            {
                lblMat_LiningT_Front.Visible = true;
                txtMat_LiningT_Front.Visible = true;
            }
            else
            {
                lblMat_LiningT_Front.Visible = false;
                txtMat_LiningT_Front.Visible = false;
            }
        }


        #endregion


        #region" CONTROL EVENT RELATED ROUTINE:"
        //***********************************

        #region "COMBOBOX RELATED ROUTINE:"
        //---------------------------------

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //====================================================================
        {
            ComboBox pCmbBox = (ComboBox)sender;

            switch (pCmbBox.Name)
            {
                case "cmbType_Front":
                    //======================
                    cmbType_Front.SelectedIndex = 0;
                    cmbBlade_Count_Front.Refresh();
                    cmbBlade_Count_Front.Items.Clear();                  

                    if (cmbType_Front.SelectedIndex == 0)
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            cmbBlade_Count_Front.Items.Add(i.ToString());
                        }
                        //cmbBlade_Count_Front.SelectedIndex = 1;
                        cmbBlade_Count_Front.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbBlade_Count_Front.Items.Add("1");
                        cmbBlade_Count_Front.SelectedIndex = 0;
                    }
                    

                    if (cmbType_Front.Text != "")
                    {
                        if (mEndSeal[0] != null)
                        {
                            mEndSeal[0].Design = (clsSeal.eDesign)
                                            Enum.Parse(typeof(clsSeal.eDesign), cmbType_Front.Text);
                            SetControl_MatLining();

                            if (mEndSeal[0].Mat_LiningT < modMain.gcEPS)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtMat_LiningT_Front.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mEndSeal[0].DESIGN_LINING_THICK), "#0.000");
                                }
                                else
                                {
                                    txtMat_LiningT_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].DESIGN_LINING_THICK, "#0.000");
                                }
                            }
                        }
                    }
                    break;


                case "cmbType_Back":
                    //==================
                    cmbType_Back.SelectedIndex = 0;
                    cmbBlade_Count_Back.Refresh();
                    cmbBlade_Count_Back.Items.Clear();                    

                    if (cmbType_Back.SelectedIndex == 0)
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            cmbBlade_Count_Back.Items.Add(i.ToString());
                        }
                        //cmbBlade_Count_Back.SelectedIndex = 1;
                        cmbBlade_Count_Back.SelectedIndex = 0;

                    }
                    else
                    {
                        cmbBlade_Count_Back.Items.Add("1");
                        cmbBlade_Count_Back.SelectedIndex = 0;
                    }                   

                    if (cmbType_Back.Text != "")
                    {
                        if (mEndSeal[1] != null)
                        {
                            mEndSeal[1].Design = (clsSeal.eDesign)
                                               Enum.Parse(typeof(clsSeal.eDesign), cmbType_Back.Text);
                            SetControl_MatLining();

                            if (mEndSeal[1].Mat_LiningT < modMain.gcEPS)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtMat_LiningT_Back.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mEndSeal[1].DESIGN_LINING_THICK), "#0.000");
                                }
                                else
                                {
                                    txtMat_LiningT_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].DESIGN_LINING_THICK, "#0.000");
                                }
                            }
                        }
                    }
                    break;

                case "cmbBlade_Count_Front":
                    //=====================
                    cmbBlade_Count_Front.SelectedIndex = 0;
                    if (cmbBlade_Count_Front.SelectedItem.ToString() == "1")
                    {
                        //lblBladeThick_Front.Visible = false;
                        //txtBlade_T_Front.Visible = false;
                        lblBladeThick_Front.Text = "Land L";

                        lblBlade_TapAng_Front.Visible = true;
                        cmbBlade_AngTaper_Front.Visible = true;
                        lblDeg_Front.Visible = true;
                        //cmbBlade_AngTaper_Front.SelectedIndex = 0;
                        if (mEndSeal[0] != null)
                        {
                            if (mEndSeal[0].Blade.AngTaper < modMain.gcEPS)
                                cmbBlade_AngTaper_Front.SelectedIndex = 0;
                            else
                                cmbBlade_AngTaper_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].Blade.AngTaper, "");
                        }
                    }
                    else
                    {                        
                        //lblBladeThick_Front.Visible = true;
                        ////lblBladeThick_Front.Text = "Thick";
                        //////txtBlade_T_Front.Visible = true;

                        ////lblBlade_TapAng_Front.Visible = false;
                        ////cmbBlade_AngTaper_Front.Visible = false;
                        ////lblDeg_Front.Visible = false;
                    }

                    if (cmbBlade_Count_Front.Text != "")
                    {
                        if (mEndSeal[0] != null)
                            mEndSeal[0].Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Front.Text);
                    }
                    break;

                case "cmbBlade_Count_Back":
                    //=====================
                    cmbBlade_Count_Back.SelectedIndex = 0;
                    if (cmbBlade_Count_Back.SelectedItem.ToString() == "1")
                    {
                        //lblBladeThick_Back.Visible = false;
                        //txtBlade_T_Back.Visible = false;
                        lblBladeThick_Back.Text = "Land L";

                        lblBlade_TapAng_Back.Visible = true;
                        cmbBlade_AngTaper_Back.Visible = true;
                        lblDeg_Back.Visible = true;

                        if (mEndSeal[1] != null)
                        {
                            if (mEndSeal[1].Blade.AngTaper < modMain.gcEPS)
                                cmbBlade_AngTaper_Back.SelectedIndex = 0;
                            else
                                cmbBlade_AngTaper_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].Blade.AngTaper, "");
                        }
                       
                    }
                    else
                    {

                        //lblBladeThick_Back.Visible = true;
                        //lblBladeThick_Back.Text = "Thick";
                        //txtBlade_T_Back.Visible = true;

                        //lblBlade_TapAng_Back.Visible = false;
                        //cmbBlade_AngTaper_Back.Visible = false;
                        //lblDeg_Back.Visible = false;
                    }

                    if (cmbBlade_Count_Back.Text != "")
                    {
                        if (mEndSeal[1] != null)
                            mEndSeal[1].Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Back.Text);
                    }

                    break;

                case "cmbBlade_AngTaper_Front":
                    //========================
                    if (mEndSeal[0] != null)
                        mEndSeal[0].Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Front.Text);
                    break;

                case "cmbBlade_AngTaper_Back":
                    //========================
                    if (mEndSeal[1] != null)
                        mEndSeal[1].Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Back.Text);
                    break;

                case "cmbMat_Base_Front":
                    //==================
                    if (mEndSeal[0] != null)
                        mEndSeal[0].Mat.Base = pCmbBox.Text;
                    break;

                case "cmbMat_Base_Back":
                    //=================
                    if (mEndSeal[1] != null)
                        mEndSeal[1].Mat.Base = pCmbBox.Text;
                    break;

                case "cmbMat_Lining_Front":
                    //=====================
                    SetMatLiningT(cmbMat_Lining_Front.Text);                                            
                        if (mEndSeal[0] != null)
                            mEndSeal[0].Mat.Lining = pCmbBox.Text;
                    
                    break;

                case "cmbMat_Lining_Back":
                    //====================
                    SetMatLiningT(cmbMat_Lining_Back.Text);
                    if (mEndSeal[1] != null)
                        mEndSeal[1].Mat.Lining = pCmbBox.Text;
                    break;
            }

        }

        #endregion


        #region "TEXTBOX RELATED ROUTINE"
        //-------------------------------

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
        //================================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtDBore_Range_Min_Front":
                    mblnDBore_Min_Front_Changed = true;
                    break;

                case "txtDBore_Range_Max_Front":
                    mblnDBore_Max_Front_Changed = true;
                    break;

                case "txtDBore_Range_Min_Back":
                    mblnDBore_Min_Back_Changed = true;
                    break;

                case "txtDBore_Range_Max_Back":
                    mblnDBore_Max_Back_Changed = true;
                    break;

            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        //==========================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtDBore_Range_Min_Front":
                    //=========================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].DBore_Range[0] =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                         mEndSeal[0].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);                    
                    }
                    txtDBore_Range_Max_Front.ForeColor = Color.Black;

                    if (mblnDBore_Min_Front_Changed)
                    {
                        if (((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1] < modMain.gcEPS)
                        {
                            if (mEndSeal[0].DBore_Range[0] > modMain.gcEPS)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mEndSeal[0].Calc_DBore_Limit(0)), "#0.0000");
                                }
                                else
                                {
                                    txtDBore_Range_Max_Front.Text = modMain.ConvDoubleToStr(mEndSeal[0].Calc_DBore_Limit(0), "#0.0000");
                                }
                                txtDBore_Range_Max_Front.ForeColor = Color.Blue;
                            }
                            else
                            {
                                txtDBore_Range_Max_Front.Text = "";

                            }
                            mblnDBore_Min_Front_Changed = false;
                        }
                    }
                    break;

                case "txtDBore_Range_Max_Front":
                    //=========================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].DBore_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[0].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }

                    if (mblnDBore_Max_Front_Changed)
                    {
                        txtDBore_Range_Max_Front.ForeColor = Color.Black;

                    }
                    break;

                case "txtBlade_T_Front":
                    //==================  
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].Blade.T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.Unit.CEng_Met(mEndSeal[0].Blade.DESIGN_BLADE_THICK));
                    }
                    else
                    {
                        mEndSeal[0].Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mEndSeal[0].Blade.DESIGN_BLADE_THICK);
                    }
                    break;

                case "txtMat_LiningT_Front":
                    //====================== 
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.Unit.CEng_Met(mEndSeal[0].DESIGN_LINING_THICK));
                    }
                    else
                    {
                        mEndSeal[0].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mEndSeal[0].DESIGN_LINING_THICK);
                    }
                    break;

                case "txtDBore_Range_Min_Back":
                    //========================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].DBore_Range[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[1].DBore_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    txtDBore_Range_Max_Back.ForeColor = Color.Black;

                    if (mblnDBore_Min_Back_Changed)
                    {
                        if (((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1] < modMain.gcEPS)
                        {
                            if (mEndSeal[1].DBore_Range[0] > modMain.gcEPS)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mEndSeal[1].Calc_DBore_Limit(0)), "#0.0000");
                                }
                                else
                                {
                                    txtDBore_Range_Max_Back.Text = modMain.ConvDoubleToStr(mEndSeal[1].Calc_DBore_Limit(0), "#0.0000");
                                }

                                txtDBore_Range_Max_Back.ForeColor = Color.Blue;
                            }
                            else
                            {
                                txtDBore_Range_Max_Back.Text = "";
                            }
                            mblnDBore_Min_Front_Changed = false;
                        }
                    }
                    break;

                case "txtDBore_Range_Max_Back":
                    //=========================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].DBore_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[1].DBore_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                    }

                    if (mblnDBore_Max_Back_Changed)
                    {
                        txtDBore_Range_Max_Back.ForeColor = Color.Black;
                    }
                    break;

                case "txtBlade_T_Back":
                    //==================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].Blade.T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.Unit.CEng_Met(mEndSeal[1].Blade.DESIGN_BLADE_THICK));
                    }
                    else
                    {
                        mEndSeal[1].Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mEndSeal[1].Blade.DESIGN_BLADE_THICK);
                    }
                    break;

                case "txtMat_LiningT_Back":
                    //======================
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        SetTxtForeColor(pTxtBox, modMain.gProject.Unit.CEng_Met(mEndSeal[1].DESIGN_LINING_THICK));
                    }
                    else
                    {
                        mEndSeal[1].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                        SetTxtForeColor(pTxtBox, mEndSeal[1].DESIGN_LINING_THICK);
                    }
                    break;

            }
        }

        

        //BG 01APR13  As per HK's instruction in email dated 27MAR13.
        private void TextBox_Validating(object sender, CancelEventArgs e)
        //================================================================
        {
            TextBox pTxtBox = (TextBox)sender;

            switch (pTxtBox.Name)
            {
                case "txtBlade_T_Front":
                    //--------------------
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].Blade.T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));                        
                    }
                    else
                    {
                        mEndSeal[0].Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                       
                    }
                    if (mEndSeal[0].Blade.T < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mEndSeal[0].Blade.T, txtBlade_T_Front, mEndSeal[0].Blade.DESIGN_BLADE_THICK);
                        e.Cancel = true;
                    }
                    break;

                case "txtBlade_T_Back":
                    //-----------------
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].Blade.T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[1].Blade.T = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    if (mEndSeal[1].Blade.T < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mEndSeal[1].Blade.T, txtBlade_T_Back, mEndSeal[1].Blade.DESIGN_BLADE_THICK);
                        e.Cancel = true;
                    }
                    break;

                case "txtMat_LiningT_Front":
                    //----------------------
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[0].Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[0].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    if (mEndSeal[0].Mat_LiningT < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mEndSeal[0].Mat_LiningT, txtMat_LiningT_Front, mEndSeal[0].DESIGN_LINING_THICK);
                        e.Cancel = true;
                    }
                    break;

                case "txtMat_LiningT_Back":
                    //----------------------
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mEndSeal[1].Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                    }
                    else
                    {
                        mEndSeal[1].Mat_LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                    }
                    if (mEndSeal[1].Mat_LiningT < modMain.gcEPS)
                    {
                        SetTxtForeColorAndDefVal(mEndSeal[1].Mat_LiningT, txtMat_LiningT_Back, mEndSeal[1].DESIGN_LINING_THICK);
                    }
                    break;
            }
        }

        #endregion


        #region "COMMAND BUTTON RELATED ROUTINE"
        //------------------------------------------------

        


        private void cmdOK_Click(object sender, EventArgs e)
        //==================================================
        {
            CloseForm();
        }


        private void CloseForm()
        //======================
        {
            //string pMsg = "Bore Dia entry is not valid." + System.Environment.NewLine +
            //                    "Seal Clearance should be greater than Bearing Clearance: " +
            //                    modMain.ConvDoubleToStr(modMain.gRadialBearing.Clearance(), "#0.000");
            string pMsg = "Bore Dia entry is not valid." + System.Environment.NewLine +
                                "Seal Clearance should be greater than Bearing Clearance: " +
                                modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clearance(), "#0.000");
            string pCaption = "Error in record entry";

            //if (tbEndSealData.SelectedTab.Text == "Front")

            //if(modMain.gRadialBearing.EndConfig_Front == clsBearing_Radial_FP.eEndConfig.Seal)
            if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            {
                if (ValidateSealData(mEndSeal[0]))
                {

                    MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtDBore_Range_Min_Front.Text = "";
                    txtDBore_Range_Max_Front.Text = "";

                    txtDBore_Range_Min_Front.Focus();
                    return;
                }
            }
            //else if(tbEndSealData.SelectedTab.Text == "Back")

            //if (modMain.gRadialBearing.EndConfig_Back == clsBearing_Radial_FP.eEndConfig.Seal)
            if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            {
                if (ValidateSealData(mEndSeal[1]))
                {

                    MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtDBore_Range_Min_Back.Text = "";
                    txtDBore_Range_Max_Back.Text = "";

                    txtDBore_Range_Min_Back.Focus();
                    return;
                }
            }

            SaveData();
            //Cursor = Cursors.WaitCursor;

            //modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
            //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            //{
            //    //modMain.gEndSeal[0].UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front"); 
            //    ((clsSeal)modMain.gProject.Product.EndConfig[0]).UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front");
            //}

            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //{
            //    //modMain.gEndSeal[1].UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Back");
            //    ((clsSeal)modMain.gProject.Product.EndConfig[1]).UpdateRec_Seal(modMain.gProject.No, modMain.gDB, "Seal", "Front");
            //}

            //Cursor = Cursors.Default;

            this.Hide();

            if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
            {
                modMain.gfrmThrustBearing.ShowDialog();
            }
            else
                //modMain.gfrmPerformanceData.ShowDialog();
                //modMain.gfrmPerformDataBearing.ShowDialog();
                modMain.gfrmBearingDesignDetails.ShowDialog();
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        //=======================================================
        {
            //if (modMain.gRadialBearing.EndConfig_Front == clsBearing_Radial_FP.eEndConfig.Seal)
            //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            //{
            //    //if (modMain.gEndSeal[0].Compare(mEndSeal[0], "Seal"))
            //    if (((clsSeal)modMain.gProject.Product.EndConfig[0]).Compare(mEndSeal[0], "Seal"))
            //    {
            //        SetMessage_SaveData();
            //    }
            //    else
            //    {
            //        this.Hide();
            //    }
            //}

            ////if (modMain.gRadialBearing.EndConfig_Back == clsBearing_Radial_FP.eEndConfig.Seal)
            //if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            //{
            //    //if (modMain.gEndSeal[1].Compare(mEndSeal[1], "Seal"))
            //    if (((clsSeal)modMain.gProject.Product.EndConfig[1]).Compare(mEndSeal[1], "Seal"))
            //    {
            //        SetMessage_SaveData();
            //    }
            //    else
            //    {
            //        this.Hide();
            //    }
            //}

            this.Hide();
        }


        private void SetMessage_SaveData()
        //================================
        {
            int pAns = (int)MessageBox.Show("Data has been modified in this form." +
                           System.Environment.NewLine + "Do you want to save before exit?"
                           , "Save Record", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

            if (pAns == pAnsY)
            {
                CloseForm();
            }
            else
            {
                this.Hide();
            }
        }


        private void SaveData()
        //=====================
        {
            //....Front
            if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
            {

                if (cmbType_Front.Text != "")
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Design = (clsSeal.eDesign)
                       Enum.Parse(typeof(clsSeal.eDesign), cmbType_Front.Text);
                }

                Double[] pDBore_Range_Front = new Double[2];
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pDBore_Range_Front[i] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtDBore[i].Text));
                    }
                    else
                    {
                        pDBore_Range_Front[i] = modMain.ConvTextToDouble(mtxtDBore[i].Text);
                    }
                }

                ((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range = pDBore_Range_Front;

                ((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Front.Text);
                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.T =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(txtBlade_T_Front.Text));
                }
                else
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.T = modMain.ConvTextToDouble(txtBlade_T_Front.Text);
                }

                ((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Front.Text);

                ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat.Base = cmbMat_Base_Front.Text;
                ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat.Lining = cmbMat_Lining_Front.Text;
                //((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Front.Text); //BG 01APR13

                //BG 01APR13
                if (((clsSeal)modMain.gProject.Product.EndConfig[0]).Design != clsSeal.eDesign.Fixed)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Front.Text));
                    }
                    else
                    {
                        ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Front.Text);
                    }
                }
                else
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[0]).Mat_LiningT = 0.0F;
                }

            }


            //....Back 
            if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
            {
                if (cmbType_Front.Text != "")
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design = (clsSeal.eDesign)
                       Enum.Parse(typeof(clsSeal.eDesign), cmbType_Back.Text);
                }

                Double[] pDBore_Range_Back = new Double[2];

                for (int i = 0, j = 2; j < 4; i++, j++)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pDBore_Range_Back[i] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtDBore[j].Text));
                    }
                    else
                    {
                        pDBore_Range_Back[i] = modMain.ConvTextToDouble(mtxtDBore[j].Text);
                    }
                }
                ((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range = pDBore_Range_Back;

                ((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count = modMain.ConvTextToInt(cmbBlade_Count_Back.Text);

                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtBlade_T_Back.Text));
                }
                else
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T = modMain.ConvTextToDouble(txtBlade_T_Back.Text);
                }
                ((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.AngTaper = modMain.ConvTextToDouble(cmbBlade_AngTaper_Back.Text);

                ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat.Base = cmbMat_Base_Back.Text;
                ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat.Lining = cmbMat_Lining_Back.Text;
                //((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);    //BG 01APR13

                //BG 01APR13
                if (((clsSeal)modMain.gProject.Product.EndConfig[1]).Design != clsSeal.eDesign.Fixed)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMat_LiningT_Back.Text));
                    }
                    else
                    {
                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = modMain.ConvTextToDouble(txtMat_LiningT_Back.Text);
                    }
                }
                else
                {
                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Mat_LiningT = 0.0F;
                }

            }
        }


        private bool ValidateSealData(clsSeal Seal_In)
        //=============================================
        {
            bool pbln = false;

            if (Seal_In.Clearance() < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clearance())
            {
                pbln = true;
            }

            return pbln;
        }

        #endregion

        #endregion

    }
}
