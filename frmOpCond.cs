
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmOperCond                            '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30JUL18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================
//.....Designer changed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace BearingCAD21
{
    public partial class frmOpCond : Form
    {
        #region "MEMBER VARIABLE"
        //***********************

            private clsOpCond mOpCond = new clsOpCond();

            Boolean mblntxtPressEng_Entered = false;
            Boolean mblntxtPressMet_Entered = false;

            Boolean mblntxtTempEng_Entered = false;
            Boolean mblntxtTempMet_Entered = false;

            //GroupBox Array
            //-------------         

            private TextBox[] mTxtThrust_Load_Design;         
           
        #endregion


        #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

            public frmOpCond()
            //====================
            {
                InitializeComponent();

                //.....Initialize Lube type.
                LoadOilSupply_Lube_Type();

                //.....Initialize Oilsupply Type.
                LoadOilSupply_Type();

                //////.....Initialize Temp Labels
                ////lblTempDegF.Text = Convert.ToString((char)176);
                ////lblTempDegC.Text = Convert.ToString((char)176);

                mTxtThrust_Load_Design = new TextBox[] { txtThrust_Load_Front, txtThrust_Load_Back };
            }

            private void LoadOilSupply_Lube_Type()
            //====================================
            {
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                //....Lube
                var pQryLube = (from pRec in pBearingDBEntities.tblData_Lube orderby pRec.fldType ascending select pRec).ToList();
                cmbLube_Type.Items.Clear();
                if (pQryLube.Count() > 0)
                {
                    for (int i = 0; i < pQryLube.Count; i++)
                    {
                        cmbLube_Type.Items.Add(pQryLube[i].fldType);
                    }
                    cmbLube_Type.SelectedIndex = -1;
                }
            }

            private void LoadOilSupply_Type()
            //===============================
            {
                cmbOilSupply_Type.Items.Clear();
                cmbOilSupply_Type.Items.Add("Pressurized");
                cmbOilSupply_Type.Items.Add("Flooded Bath");
                //cmbOilSupply_Type.SelectedIndex = -1;
                cmbOilSupply_Type.SelectedIndex = 0;        //BG 04OCT12
            }
            

        #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

            private void frmOperCond_Load(object sender, EventArgs e)   
            //========================================================
            {

                //....Reset Diff Control value.
                ResetControlVal();

                //....Set Local Object.                    
                SetLocalObject();

                //....DisplayData.
                DisplayData();

                //....Set Controls
                SetControl();
               
            }

            private void SetControl()
            //=======================
            {     
                if (modMain.gProject != null)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.English)
                    {
                        grpStaticLoad.Text = "Static Load (" + "lbf):";
                        lblPressUnit.Text = "psig";
                        lblTempUnit.Text = Convert.ToString((char)176) + "F";
                    }
                    else
                    {
                        grpStaticLoad.Text = "Static Load (" + "kN):";
                        lblPressUnit.Text = "MPa";
                        lblTempUnit.Text = Convert.ToString((char)176) + "C";
                    }

                    lblStaticLoad_Thrust.Enabled = false;

                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            lblStaticLoad_Thrust.Enabled = true;
                            mTxtThrust_Load_Design[i].Enabled = true;
                        }
                        else
                        {
                            mTxtThrust_Load_Design[i].Enabled = false;
                        }
                    }
                }
              
            }  
            

            private void SetLocalObject()
            //===========================
            {
                mOpCond = (clsOpCond)modMain.gOpCond.Clone();
            }

            private void ResetControlVal()      
            //============================
            {
                const string pcBlank = "";  

                //  Speed
                //  -----                    
                    txtSpeed_Design.Text = pcBlank;               

   
                //  Load
                //  ----
                    txtRadial_Load.Text = pcBlank;
              

                //  Load Angle
                //  ----------
                    txtRadial_LoadAng.Text = pcBlank;

                //  Load: Thrust
                //  --------------

                    //....Front       
                    txtThrust_Load_Front.Text = pcBlank;
            
                    //....Back
                    txtThrust_Load_Back.Text = pcBlank;  

            }

            

            private void DisplayData()
            //========================
            {
                txtSpeed_Design.Text = modMain.ConvIntToStr(mOpCond.Speed);

                //  Directionality
                //  --------------
                if (mOpCond.Rot_Directionality.ToString() == "Bi")
                    optRot_Directionality_Bi.Checked = true;
                else if (mOpCond.Rot_Directionality.ToString() == "Uni")
                    optRot_Directionality_Uni.Checked = true;

                //  Load: Radial
                //  -------------
                txtRadial_Load.Text = modMain.ConvDoubleToStr(mOpCond.Radial_Load, "#0.#");


                //  Load Angle
                //  ----------
                txtRadial_LoadAng.Text = modMain.ConvDoubleToStr(mOpCond.Radial_LoadAng, "#0.#");

                //  Load: Thrust
                //  ------------    
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    mTxtThrust_Load_Design[0].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range[0], "#0.00");
                  
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    mTxtThrust_Load_Design[1].Text = modMain.ConvDoubleToStr(mOpCond.Thrust_Load_Range[1], "#0.00");
                }

                cmbLube_Type.Text = mOpCond.OilSupply.Lube_Type;
                cmbOilSupply_Type.Text = mOpCond.OilSupply.Type;
                txtOilSupply_Press.Text = modMain.ConvDoubleToStr(mOpCond.OilSupply.Press, "#0.#");
                txtOilSupply_Temp.Text = modMain.ConvDoubleToStr(mOpCond.OilSupply.Temp, "#0.#");
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE:"
            //**********************************

            #region"TEXT BOX RELATED ROUTINE:"
            //--------------------------------

                #region "LOAD RELATED ROUTINE"  

                    private void txtLoadAngle_TextChanged(object sender, EventArgs e)
                    //================================================================
                    {
                        mOpCond.Radial_LoadAng = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);
                    }


                    private void OptionButton_CheckedChanged(object sender, EventArgs e)
                    //===================================================================   //BG 23APR09
                    {
                        RadioButton pOptButton = (RadioButton)sender;

                        switch (pOptButton.Name)
                        {
                            case "optDirection_Uni":
                                //=================
                                if (pOptButton.Checked)
                                    modMain.gOpCond.Rot_Directionality =
                                            (clsOpCond.eRotDirectionality)Enum.Parse
                                                (typeof(clsOpCond.eRotDirectionality), "Uni");
                                break;

                            case "optDirection_Bi":
                                //=================
                                if (pOptButton.Checked)
                                    modMain.gOpCond.Rot_Directionality =
                                            (clsOpCond.eRotDirectionality)Enum.Parse
                                            (typeof(clsOpCond.eRotDirectionality), "Bi");
                                break;

                        }
                    }
                  

                #endregion              
                

            #endregion

            #region "COMBO BOX RELATED ROUTINES"

                private void cmbLube_Type_SelectedIndexChanged(object sender, EventArgs e)
                //==========================================================================
                {
                    mOpCond.OilSupply_Lube_Type = cmbLube_Type.Text; 
                }

                private void cmbOilSupply_Type_SelectedIndexChanged(object sender, EventArgs e)
                //==============================================================================
                {
                    cmbOilSupply_Type.SelectedIndex = 0;
                    modMain.gOpCond.OilSupply_Type = cmbOilSupply_Type.Text;  
                }

            #endregion

            #region "COMMAND BUTTON EVENT ROUTINES"
                    //-------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;

                    switch (pcmdButton.Name)
                    {
                        case "cmdOK":
                            //-------
                            CloseForm();
                            break;

                        case "cmdCancel":
                            //----------
                            this.Hide(); 
                            break;
                    }
                }
                
                private void CloseForm()
                //======================
                {
                    SaveData();                   
                    this.Hide();

                    modMain.gfrmBearing.ShowDialog();   
                }

                private void SaveData()
                //=====================
                {
                    modMain.gOpCond.Speed = modMain.ConvTextToInt(txtSpeed_Design.Text);
                   

                    if (optRot_Directionality_Bi.Checked)
                    {
                        modMain.gOpCond.Rot_Directionality =
                             (clsOpCond.eRotDirectionality)Enum.Parse
                                (typeof(clsOpCond.eRotDirectionality), "Bi");
                    }
                    else
                    {
                        modMain.gOpCond.Rot_Directionality =
                             (clsOpCond.eRotDirectionality)Enum.Parse
                                (typeof(clsOpCond.eRotDirectionality), "Uni");
                    }

                    modMain.gOpCond.Radial_Load = modMain.ConvTextToDouble(txtRadial_Load.Text);
                    modMain.gOpCond.Radial_LoadAng = modMain.ConvTextToDouble(txtRadial_LoadAng.Text);

                    //  Load: Thrust
                    //  ------------    
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        modMain.gOpCond.Thrust_Load_Range[0] = modMain.ConvTextToDouble(txtThrust_Load_Front.Text);                       

                    }

                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        modMain.gOpCond.Thrust_Load_Range[1] = modMain.ConvTextToDouble(txtThrust_Load_Back.Text);  
                    }

                    modMain.gOpCond.OilSupply_Lube_Type = cmbLube_Type.Text;
                    modMain.gOpCond.OilSupply_Type = cmbOilSupply_Type.Text;
                    modMain.gOpCond.OilSupply_Press = modMain.ConvTextToDouble(txtOilSupply_Press.Text); 
                    modMain.gOpCond.OilSupply_Temp = modMain.ConvTextToDouble(txtOilSupply_Temp.Text);

                }

      
            #endregion

                

                
                        
        #endregion

    }
}
