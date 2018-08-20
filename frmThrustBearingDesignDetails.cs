//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmThrustBearingDesignDetails          '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03AUG18                                '
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
using System.Drawing.Printing;

namespace BearingCAD21
{
    public partial class frmThrustBearingDesignDetails : Form
    {

        #region "MEMBER VARIABLES"
        //***********************

            private clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
            private ComboBox[] mCmbFeedGroove;

        #endregion


        #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
        //********************************************

            public frmThrustBearingDesignDetails()
            //====================================
            {
                InitializeComponent();

                mCmbFeedGroove = new ComboBox[] { cmbFeedGroove_Type_Front, cmbFeedGroove_Type_Back };
            }

         #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

            private void frmThrustBearingDesignDetails_Load(object sender, EventArgs e)
            //==========================================================================
            {               
                //....Set Local Object.
                SetLocalObject();

                //....Set Tab Pages.
                SetTabPages();

                //....Feed Groove Type
                Populate_FeedGrooveType();

                //....Set Control.
                SetControl();

                //....Diaplay Data.
                DisplayData();
            }


            private void Populate_FeedGrooveType()
            //===================================
            {

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        mCmbFeedGroove[i].Items.Clear();
                        mCmbFeedGroove[i].Items.Add("Flat-Bottomed");
                        mCmbFeedGroove[i].Items.Add("Scribed");
                        mCmbFeedGroove[i].SelectedIndex = 0;
                    }
                }
               
            }


            private void SetLocalObject()
            //===========================
            {
                mEndTB = new clsBearing_Thrust_TL[2];

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndConfig[i])).Clone();
                    }
                }
            }


            private void SetTabPages()
            //===========================
            {
                TabPage[] pTabPages = new TabPage[] { tabFront, tabBack };

                tbTBDesignDetails.TabPages.Clear();

                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        tbTBDesignDetails.TabPages.Add(pTabPages[i]);
                }

                tbTB_Front.TabPages.Remove(tbBackRelief_Front);
                tbTB_Front.TabPages.Remove(tbFeedGroove_Front);

                tbTB_Back.TabPages.Remove(tbBackRelief_Back);
                tbTB_Back.TabPages.Remove(tbFeedGroove_Back);
                
            }


            private void SetControl()
            //=======================                           
            {
                SetControls_MountHoles();
                SetControl_BackRelief();

                Boolean pBlnSet;
              
                pBlnSet = true;                    
                SetControls_Status(pBlnSet);

              
            }


            private void SetControls_MountHoles()
            //===================================   SG 28FEB13
            {
               
                int pT_Pos_Left = 12;
                int pC_Pos_Left = 100;

                optMountHoles_Type_CBore_Front.Checked = false;
                optMountHoles_Type_Thru_Front.Checked = false;
                optMountHoles_Type_Thread_Front.Checked = false;
                chkMountHoles_Thread_Thru_Front.Checked = false;
          
                optMountHoles_Type_CBore_Back.Checked = false;
                optMountHoles_Type_Thru_Back.Checked = false;
                optMountHoles_Type_Thread_Back.Checked = false;
                chkMountHoles_Thread_Thru_Back.Checked = false;
              
                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_GoThru)
                {
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                    {
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            optMountHoles_Type_CBore_Front.Visible = true;
                            optMountHoles_Type_Thru_Front.Visible = true;

                            if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                optMountHoles_Type_CBore_Front.Checked = true;

                            optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                            optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;

                            optMountHoles_Type_Thread_Front.Visible = false;                            
                            chkMountHoles_Thread_Thru_Front.Visible = false;
                         
                            grpMountHoles_Type_Front.Width = 185;
                            lblMountHoles_Thread_Depth_Front.Top = 141;
                            txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                        }

                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {                           
                            optMountHoles_Type_CBore_Back.Visible = false;
                            optMountHoles_Type_Thru_Back.Visible = false;
                         
                            optMountHoles_Type_Thread_Back.Visible = true;
                            optMountHoles_Type_Thread_Back.Checked = true;
                            chkMountHoles_Thread_Thru_Back.Visible = true;

                            grpMountHoles_Type_Back.Width = 100;
                            chkMountHoles_Thread_Thru_Back.Left = 120;
                            lblMountHoles_Thread_Depth_Back.Top = 75;
                            txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                        }
                    }

                    else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                    {

                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            optMountHoles_Type_CBore_Front.Visible = false;
                            optMountHoles_Type_Thru_Front.Visible = false;
                        
                            optMountHoles_Type_Thread_Front.Visible = true;                           
                            optMountHoles_Type_Thread_Front.Checked = true;
                            chkMountHoles_Thread_Thru_Front.Visible = true;

                            grpMountHoles_Type_Front.Width = 100;
                            chkMountHoles_Thread_Thru_Front.Left = 120;
                            lblMountHoles_Thread_Depth_Front.Top = 75;
                            txtMountHoles_Thread_Depth_Front.Top = lblMountHoles_Thread_Depth_Front.Top - 2;
                        }

                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {                           
                            optMountHoles_Type_CBore_Back.Visible = true;
                            optMountHoles_Type_Thru_Back.Visible = true;
                            if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                optMountHoles_Type_CBore_Back.Checked = true;

                            optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                            optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;

                            optMountHoles_Type_Thread_Back.Visible = false;                           
                            chkMountHoles_Thread_Thru_Back.Visible = false;                           

                            grpMountHoles_Type_Back.Width = 185;
                            lblMountHoles_Thread_Depth_Back.Top = 141;
                            txtMountHoles_Thread_Depth_Back.Top = lblMountHoles_Thread_Depth_Back.Top - 2;
                        }
                    }
                }

                else
                {
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                    {
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            optMountHoles_Type_CBore_Front.Visible = true;
                            optMountHoles_Type_Thru_Front.Visible = true;
                            
                            optMountHoles_Type_CBore_Front.Left = pT_Pos_Left;
                            optMountHoles_Type_Thru_Front.Left = pC_Pos_Left;                                                      

                            optMountHoles_Type_Thread_Front.Visible = false;
                            optMountHoles_Type_Thread_Front.Checked = false;
                            chkMountHoles_Thread_Thru_Front.Visible = false;

                            if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                            {
                                optMountHoles_Type_CBore_Front.Checked = true;
                            }

                            else if (modMain.gProject.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                            {
                                optMountHoles_Type_Thru_Front.Checked = true;
                            }

                            grpMountHoles_Type_Front.Width = 185;     
                           
                        }

                        if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {                           
                            optMountHoles_Type_CBore_Back.Visible = true;
                            optMountHoles_Type_Thru_Back.Visible = true;

                            optMountHoles_Type_CBore_Back.Left = pT_Pos_Left;
                            optMountHoles_Type_Thru_Back.Left = pC_Pos_Left;
                          
                            optMountHoles_Type_Thread_Back.Visible = false;
                            optMountHoles_Type_Thread_Back.Checked = false;
                            chkMountHoles_Thread_Thru_Back.Visible = false;

                            if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                            {
                                optMountHoles_Type_CBore_Back.Checked = true;
                            }

                            else if (modMain.gProject.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                            {
                                optMountHoles_Type_Thru_Back.Checked = true;
                            }

                            grpMountHoles_Type_Back.Width = 185;
                           
                        }
                    }

                }
            }


            private void SetControls_Status(Boolean Enable_In)
            //===============================================
            {
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtL_Front.ReadOnly = !Enable_In;
                    txtLFlange_Front.ReadOnly = !Enable_In;
                     //txtFaceOff_Assy_Front.ReadOnly = !Enable_In;
                
                    //....Mount Holes
                    grpMountHoles_Type_Front.Enabled = Enable_In;
                    txtMountHoles_CBore_Depth_Front.ReadOnly = !Enable_In;
                    txtMountHoles_Thread_Depth_Front.ReadOnly = !Enable_In;

                    //....Back Relief
                    chkBackRelief_Reqd_Front.Enabled = Enable_In;
                    txtBackRelief_D_Front.ReadOnly = !Enable_In;
                    txtBackRelief_Depth_Front.ReadOnly = !Enable_In;
                    txtBackRelief_Fillet_Front.ReadOnly = !Enable_In;

                    //....Feed Groove
                    cmbFeedGroove_Type_Front.Enabled = Enable_In;
                    txtFeedGroove_DBC_Front.ReadOnly = !Enable_In;
                    txtFeedGroove_Dist_Chamf_Front.ReadOnly = !Enable_In;
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtL_Back.ReadOnly = !Enable_In;
                    txtLFlange_Back.ReadOnly = !Enable_In;
                    //txtFaceOff_Assy_Back.ReadOnly = !Enable_In;

                    //....Mount Holes
                    grpMountHoles_Type_Back.Enabled = Enable_In;
                    txtMountHoles_CBore_Depth_Back.ReadOnly = !Enable_In;
                    txtMountHoles_Thread_Depth_Back.ReadOnly = !Enable_In;

                    //....Back Relief
                    chkBackRelief_Reqd_Back.Enabled = Enable_In;
                    txtBackRelief_D_Back.ReadOnly = !Enable_In;
                    txtBackRelief_Depth_Back.ReadOnly = !Enable_In;
                    txtBackRelief_Fillet_Back.ReadOnly = !Enable_In;

                    //....Feed Groove
                    cmbFeedGroove_Type_Back.Enabled = Enable_In;
                    txtFeedGroove_DBC_Back.ReadOnly = !Enable_In;
                    txtFeedGroove_Dist_Chamf_Back.ReadOnly = !Enable_In;
                }
            }


            private void DisplayData()
            //========================
            {
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtRotation_Front.Text = clsBearing_Thrust_TL.eRotation.CCW.ToString();
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtL_Front.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].L));
                        txtLFlange_Front.Text =modMain.gProject.Unit.WriteInUserL( modMain.gProject.Unit.CEng_Met(mEndTB[0].LFlange)); 
                    }
                    else
                    {                       
                        txtL_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].L);
                        txtLFlange_Front.Text =modMain.gProject.Unit.WriteInUserL(mEndTB[0].LFlange); 
                    }
                                   

                    //....Mount Holes
                    if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        optMountHoles_Type_CBore_Front.Checked = true;
                                      
                    else if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                        optMountHoles_Type_Thru_Front.Checked = true;

                    else if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting != clsBearing_Radial_FP.eFaceID.Both)  //SG 28FEB13
                        {
                            optMountHoles_Type_Thread_Front.Checked = true;
                        }
                        else
                        {
                            optMountHoles_Type_CBore_Front.Checked = true;
                        }
                    }

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.Screw_Spec.D_Thru));
                        txtMountHoles_D_CBore_Front.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.Screw_Spec.D_CBore));
                        txtMountHoles_CBore_Depth_Front.Text =modMain.gProject.Unit.WriteInUserL( modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.Depth_CBore));
                        lblMountHoles_CBoreDepth_LLim_Front.Text =modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.CBore_Depth_LowerLimit()));
                        lblMountHoles_CBoreDepth_ULim_Front.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.CBore_Depth_UpperLimit()));

                        chkMountHoles_Thread_Thru_Front.Checked = mEndTB[0].MountHoles.Thread_Thru;
                        txtMountHoles_Thread_Depth_Front.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[0].MountHoles.Depth_Thread));
                    }
                    else
                    {
                        txtMountHoles_D_ThruHole_Front.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[0].MountHoles.Screw_Spec.D_Thru);
                        txtMountHoles_D_CBore_Front.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[0].MountHoles.Screw_Spec.D_CBore);
                        txtMountHoles_CBore_Depth_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].MountHoles.Depth_CBore);
                        lblMountHoles_CBoreDepth_LLim_Front.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[0].MountHoles.CBore_Depth_LowerLimit());
                        lblMountHoles_CBoreDepth_ULim_Front.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[0].MountHoles.CBore_Depth_UpperLimit());

                        chkMountHoles_Thread_Thru_Front.Checked = mEndTB[0].MountHoles.Thread_Thru;
                        txtMountHoles_Thread_Depth_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].MountHoles.Depth_Thread);
                    }

                    //....Back Relief               
                    chkBackRelief_Reqd_Front.Checked  = mEndTB[0].BackRelief.Reqd;

                    if(mEndTB[0].BackRelief.Reqd)
                    {
                        txtBackRelief_D_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].BackRelief.D);
                        txtBackRelief_Depth_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].BackRelief.Depth);
                        txtBackRelief_Fillet_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].BackRelief.Fillet);
                    }

                    //....Feed Groove
                    cmbFeedGroove_Type_Front.Text =mEndTB[0].FeedGroove.Type ;
                    txtFeedGroove_DBC_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].FeedGroove.DBC);
                    txtFeedGroove_Dist_Chamf_Front.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[0].FeedGroove.Dist_Chamf);
                }

                if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtRotation_Back.Text = clsBearing_Thrust_TL.eRotation.CW.ToString();
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtL_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].L));
                        txtLFlange_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].LFlange));
                    }
                    else
                    {
                        txtL_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].L);
                        txtLFlange_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].LFlange);
                    }
                    //txtDimStart_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].DimStart(), "#0.0000");
                    //txtFaceOff_Assy_Back.Text = mEndTB[1].FaceOff_Assy.ToString("#0.000");

                    //....Mount Holes
                    if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        optMountHoles_Type_CBore_Back.Checked = true;

                    else if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                        optMountHoles_Type_Thru_Back.Checked = true;

                    else if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                    {
                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting != clsBearing_Radial_FP.eFaceID.Both)  //SG 28FEB13
                        {
                            optMountHoles_Type_Thread_Back.Checked = true;
                        }
                        else
                        {
                            optMountHoles_Type_CBore_Back.Checked = true;
                        }
                    }
                    //modMain.gProject.Unit.CEng_Met(
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.Screw_Spec.D_Thru));

                        txtMountHoles_D_CBore_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.Screw_Spec.D_CBore));
                        txtMountHoles_CBore_Depth_Back.Text =modMain.gProject.Unit.WriteInUserL( modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.Depth_CBore));
                        lblMountHoles_CBoreDepth_LLim_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.CBore_Depth_LowerLimit()));
                        lblMountHoles_CBoreDepth_ULim_Back.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.CBore_Depth_UpperLimit()));

                        chkMountHoles_Thread_Thru_Back.Checked = mEndTB[1].MountHoles.Thread_Thru;
                        txtMountHoles_Thread_Depth_Back.Text =modMain.gProject.Unit.WriteInUserL( modMain.gProject.Unit.CEng_Met(mEndTB[1].MountHoles.Depth_Thread));
                    }
                    else
                    {
                        txtMountHoles_D_ThruHole_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].MountHoles.Screw_Spec.D_Thru);

                        txtMountHoles_D_CBore_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].MountHoles.Screw_Spec.D_CBore);
                        txtMountHoles_CBore_Depth_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].MountHoles.Depth_CBore);
                        lblMountHoles_CBoreDepth_LLim_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].MountHoles.CBore_Depth_LowerLimit());
                        lblMountHoles_CBoreDepth_ULim_Back.Text = modMain.gProject.Unit.WriteInUserL(mEndTB[1].MountHoles.CBore_Depth_UpperLimit());

                        chkMountHoles_Thread_Thru_Back.Checked = mEndTB[1].MountHoles.Thread_Thru;
                        txtMountHoles_Thread_Depth_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].MountHoles.Depth_Thread);
                    }

                    //....Back Relief               
                    chkBackRelief_Reqd_Back.Checked = mEndTB[1].BackRelief.Reqd;

                    if (mEndTB[1].BackRelief.Reqd)
                    {
                        txtBackRelief_D_Back.Text = modMain.gProject.Unit.WriteInUserL( mEndTB[1].BackRelief.D);
                        txtBackRelief_Depth_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].BackRelief.Depth);
                        txtBackRelief_Fillet_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].BackRelief.Fillet);
                    }

                    //....Feed Groove
                    cmbFeedGroove_Type_Back.Text = mEndTB[1].FeedGroove.Type;
                    txtFeedGroove_DBC_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].FeedGroove.DBC);
                    txtFeedGroove_Dist_Chamf_Back.Text =modMain.gProject.Unit.WriteInUserL( mEndTB[1].FeedGroove.Dist_Chamf);
                }
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE"
        //*************************************

            #region "OPTION BUTTON RELATED ROUTINE"

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //================================================================
                {
                    RadioButton pOptButton = (RadioButton)sender;

                    switch (pOptButton.Name)
                    {
                        case "optMountHoles_Type_CBore_Front":
                            //-------------------------------
                            //....CBore 
                            if (pOptButton.Checked)
                            mEndTB[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front_Upper.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = optMountHoles_Type_CBore_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front_Lower.Visible = optMountHoles_Type_CBore_Front.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_CBore_Front.Checked;

                            break;


                        case "optMountHoles_Type_Thru_Front":
                            //------------------------------
                            //....Thru'
                            if (pOptButton.Checked)
                            mEndTB[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = optMountHoles_Type_Thru_Front.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front_Upper.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front_Lower.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = !optMountHoles_Type_Thru_Front.Checked;

                            break;

                        case "optMountHoles_Type_Thread_Front":
                            //---------------------------------
                            //....Thread
                            if (pOptButton.Checked)
                            mEndTB[0].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_D_ThruHole_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_D_CBore_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_CBore_Depth_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_ULim_Front_Upper.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front.Visible = !optMountHoles_Type_Thread_Front.Checked;
                            lblMountHoles_CBoreDepth_LLim_Front_Lower.Visible = !optMountHoles_Type_Thread_Front.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Front.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Front.Visible = optMountHoles_Type_Thread_Front.Checked;

                            break;

                        case "optMountHoles_Type_CBore_Back":
                            //-------------------------------                         
                            //....CBore 
                            if (pOptButton.Checked)
                            mEndTB[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            
                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back_Upper.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = optMountHoles_Type_CBore_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back_Lower.Visible = optMountHoles_Type_CBore_Back.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_CBore_Back.Checked;
                            
                            break;


                        case "optMountHoles_Type_Thru_Back":
                            //------------------------------
                            //....Thru
                            if (pOptButton.Checked)
                            mEndTB[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = optMountHoles_Type_Thru_Back.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back_Upper.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back_Lower.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = !optMountHoles_Type_Thru_Back.Checked;
                            
                            break;

                        case "optMountHoles_Type_Thread_Back":
                            //-------------------------------
                            if (pOptButton.Checked)
                            mEndTB[1].MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            //....Thru' Dia.
                            lblMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_D_ThruHole_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore Dia.
                            lblMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_D_CBore_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore Depth.
                            lblMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_CBore_Depth_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....CBore Depth Limits.
                            lblMountHoles_Limits_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_ULim_Back_Upper.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back.Visible = !optMountHoles_Type_Thread_Back.Checked;
                            lblMountHoles_CBoreDepth_LLim_Back_Lower.Visible = !optMountHoles_Type_Thread_Back.Checked;

                            //....Thread Depth.
                            lblMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Back.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = optMountHoles_Type_Thread_Back.Checked;

                            //....Thru'
                            chkMountHoles_Thread_Thru_Back.Visible = optMountHoles_Type_Thread_Back.Checked;

                            break;
                    }
            }

            #endregion


            #region "CHECKBOX RELATED ROUTINE"

                private void chkBox_CheckedChanged(object sender, EventArgs e)
                //=============================================================
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                    {
                        case "chkBackRelief_Reqd_Front":
                            //-----------------------
                            mEndTB[0].BackRelief_Reqd = pChkBox.Checked;
                            SetControl_BackRelief();
                            //txtDimStart_Front.Text = modMain.ConvDoubleToStr(mEndTB[0].DimStart(),"#0.0000");
                            break;

                        case "chkBackRelief_Reqd_Back":
                            //----------------------
                            mEndTB[1].BackRelief_Reqd = pChkBox.Checked;
                            SetControl_BackRelief();
                            //txtDimStart_Back.Text = modMain.ConvDoubleToStr(mEndTB[1].DimStart(), "#0.0000");
                            break;

                        case "chkMountHoles_Thread_Thru_Front":
                            //---------------------------------
                            lblMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                            txtMountHoles_Thread_Depth_Front.Visible = !chkMountHoles_Thread_Thru_Front.Checked;
                            break;

                        case "chkMountHoles_Thread_Thru_Back":
                            //--------------------------------
                            lblMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                            txtMountHoles_Thread_Depth_Back.Visible = !chkMountHoles_Thread_Thru_Back.Checked;
                            break;
                    }
                }

                private void SetControl_BackRelief()
                //===========================================
                {
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        lblBackRelief_D_Front.Visible = chkBackRelief_Reqd_Front.Checked;
                        txtBackRelief_D_Front.Visible = chkBackRelief_Reqd_Front.Checked;

                        lblBackRelief_Depth_Front.Visible = chkBackRelief_Reqd_Front.Checked;
                        txtBackRelief_Depth_Front.Visible = chkBackRelief_Reqd_Front.Checked;

                        lblBackRelief_Fillet_Front.Visible = chkBackRelief_Reqd_Front.Checked;
                        txtBackRelief_Fillet_Front.Visible = chkBackRelief_Reqd_Front.Checked;
                    }

                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {

                        lblBackRelief_D_Back.Visible = chkBackRelief_Reqd_Back.Checked;
                        txtBackRelief_D_Back.Visible = chkBackRelief_Reqd_Back.Checked;

                        lblBackRelief_Depth_Back.Visible = chkBackRelief_Reqd_Back.Checked;
                        txtBackRelief_Depth_Back.Visible = chkBackRelief_Reqd_Back.Checked;

                        lblBackRelief_Fillet_Back.Visible = chkBackRelief_Reqd_Back.Checked;
                        txtBackRelief_Fillet_Back.Visible = chkBackRelief_Reqd_Back.Checked;
                    }
                }

             #endregion


            #region "COMBO BOX RELATED ROUTINE"

                private void CmbBox_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================
                {
                    
                    //mEndTB[1].FeedGroove.Type = cmbFeedGroove_Type_Back.Text;
                     ComboBox pCmbBox = (ComboBox)sender;

                     switch (pCmbBox.Name)
                     {
                         case "cmbFeedGroove_Type_Front":
                             //============================
                             mEndTB[0].FeedGroove.Type = cmbFeedGroove_Type_Front.Text;
                             break;

                         case "cmbFeedGroove_Type_Back":
                             //============================
                             mEndTB[1].FeedGroove.Type = cmbFeedGroove_Type_Back.Text;
                             break;
                     }

                   
                }

            #endregion


            #region "TEXTBOX RELATED ROUTINE"
            //-------------------------------

                private void TextBox_TextChanged(object sender, EventArgs e)
                //==========================================================
                {
                    Double pVal= 0.0;
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtL_Front":
                            //------------
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mEndTB[0].L = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mEndTB[0].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                                //SetTxtForeColor_L(txtL_Front, ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).L);
                                SetTxtForeColor_L(txtL_Front, modMain.gProject.Product.Calc_L_EndConfig());       
                            break;

                        case "txtL_Back":
                            //-----------
                            //mEndTB[1].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mEndTB[1].L = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mEndTB[1].L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            //SetTxtForeColor_L(txtL_Back, ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).L);
                            SetTxtForeColor_L(txtL_Back, modMain.gProject.Product.Calc_L_EndConfig());        //BG 07DEC12
                            break;
                    
                        case "txtFaceOff_Assy_Front":
                            //-----------------------
                            mEndTB[0].FaceOff_Assy = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColorAndDefVal(mEndTB[0].FaceOff_Assy, pTxtBox, 0.010);
                            SetTxtForeColor(pTxtBox, mEndTB[0].FACEOFF_ASSY_DEF);
                            break;

                        case "txtFaceOff_Assy_Back":
                            //----------------------
                            mEndTB[1].FaceOff_Assy = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColorAndDefVal(mEndTB[1].FaceOff_Assy, pTxtBox, 0.010);
                            SetTxtForeColor(pTxtBox, mEndTB[1].FACEOFF_ASSY_DEF);
                            break;

                        case "txtBackRelief_Depth_Front":
                            //---------------------------
                            mEndTB[0].BackRelief_Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColorAndDefVal(mEndTB[0].BackRelief.Depth, pTxtBox, mEndTB[0].BACK_RELIEF_DEPTH);
                            SetTxtForeColor(pTxtBox, mEndTB[0].BACK_RELIEF_DEPTH);
                            break;

                        case "txtBackRelief_Depth_Back":
                            //--------------------------
                            mEndTB[1].BackRelief_Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            //SetTxtForeColorAndDefVal(mEndTB[1].BackRelief.Depth, pTxtBox,mEndTB[1].BACK_RELIEF_DEPTH);
                            SetTxtForeColor(pTxtBox, mEndTB[1].BACK_RELIEF_DEPTH);
                            break;

                        case "txtBackRelief_Fillet_Front":
                            //----------------------------
                            mEndTB[0].BackRelief_Fillet = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor(pTxtBox, mEndTB[0].BACK_RELIEF_FILLET);
                            break;

                        case "txtBackRelief_Fillet_Back":
                            //---------------------------
                            mEndTB[1].BackRelief_Fillet = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor(pTxtBox, mEndTB[1].BACK_RELIEF_FILLET);
                            break;

                        case "txtFeedGroove_Dist_Chamf_Front":
                            //--------------------------------
                            mEndTB[0].FeedGroove.Dist_Chamf = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor(pTxtBox, mEndTB[0].FeedGroove.DIST_CHAMF);
                            //SetTxtForeColorAndDefVal(mEndTB[0].FeedGroove.Dist_Chamf, pTxtBox, mEndTB[0].FeedGroove.DIST_CHAMF);
                            break;

                        case "txtFeedGroove_Dist_Chamf_Back":
                            //-------------------------------
                            mEndTB[1].FeedGroove.Dist_Chamf = modMain.ConvTextToDouble(pTxtBox.Text);
                            SetTxtForeColor(pTxtBox, mEndTB[1].FeedGroove.DIST_CHAMF);
                            //SetTxtForeColorAndDefVal(mEndTB[1].FeedGroove.Dist_Chamf, pTxtBox, mEndTB[1].FeedGroove.DIST_CHAMF);
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
                        case "txtFaceOff_Assy_Front":
                       //----------------------------
                            mEndTB[0].FaceOff_Assy = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mEndTB[0].FaceOff_Assy < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(mEndTB[0].FaceOff_Assy, pTxtBox, mEndTB[0].FACEOFF_ASSY_DEF);
                                e.Cancel = true;
                            }
                            break;

                        case "txtFaceOff_Assy_Back":
                        //--------------------------
                            mEndTB[1].FaceOff_Assy = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mEndTB[1].FaceOff_Assy < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(mEndTB[1].FaceOff_Assy, pTxtBox, mEndTB[1].FACEOFF_ASSY_DEF);
                                e.Cancel = true;
                            }
                            break;

                        case "txtBackRelief_Depth_Front":
                        //-------------------------------
                            mEndTB[0].BackRelief_Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mEndTB[0].BackRelief.Depth < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(mEndTB[0].BackRelief.Depth, pTxtBox, mEndTB[0].BACK_RELIEF_DEPTH);
                                e.Cancel = true;
                            } 
                            break;

                        case "txtBackRelief_Depth_Back":
                        //-------------------------------
                            mEndTB[1].BackRelief_Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mEndTB[1].BackRelief.Depth < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(mEndTB[1].BackRelief.Depth, pTxtBox, mEndTB[1].BACK_RELIEF_DEPTH);
                                e.Cancel = true;
                            } 
                            break;

                        case "txtFeedGroove_Dist_Chamf_Front":
                        //------------------------------------
                            //mEndTB[0].FeedGroove.Dist_Chamf = modMain.ConvTextToDouble(pTxtBox.Text);
                            Double pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (pVal < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(pVal, pTxtBox, mEndTB[0].FeedGroove.DIST_CHAMF);
                                pVal = modMain.ConvTextToDouble(pTxtBox.Text);
                                e.Cancel = true;
                            }
                            break;

                        case "txtFeedGroove_Dist_Chamf_Back":
                            //-------------------------------
                            mEndTB[1].FeedGroove.Dist_Chamf = modMain.ConvTextToDouble(pTxtBox.Text);
                            if (mEndTB[1].FeedGroove.Dist_Chamf < modMain.gcEPS)
                            {
                                SetTxtForeColorAndDefVal(mEndTB[1].FeedGroove.Dist_Chamf, pTxtBox, mEndTB[1].FeedGroove.DIST_CHAMF);
                                e.Cancel = true;
                            }
                            break;

                    }
                }


                private void SetTxtForeColorAndDefVal(Double T_In, TextBox TxtBox_In, Double DefVal_In)
                //======================================================================================     
                {
                    if (T_In != 0.000)
                    {
                        if (Math.Abs(T_In - DefVal_In) < modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Magenta;
                            TxtBox_In.Text = T_In.ToString("#0.000");
                        }
                        else
                            TxtBox_In.ForeColor = Color.Black;                        
                    }
                    else
                    {
                        TxtBox_In.ForeColor = Color.Magenta;
                        TxtBox_In.Text = DefVal_In.ToString("#0.000");
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

                private void SetTxtForeColor_L(TextBox TxtBox_In, Double ActulVal_In)
                //====================================================================       
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (System.Math.Abs(modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(TxtBox_In.Text)) - ActulVal_In) <= modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(modMain.ConvTextToDouble(TxtBox_In.Text) - ActulVal_In) <= modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;
                        }
                    }
                }

                //private void SetTextForeColor_BackRelief_Fillet(Double T_In, TextBox TxtBox_In, Double DefVal_In)
                ////===============================================================================================
                //{
                //    if (T_In != DefVal_In)
                //    {
                //        TxtBox_In.ForeColor = Color.Black;
                //    }
                //    else
                //    {
                //        TxtBox_In.ForeColor = Color.Magenta;
                //    }
                //    TxtBox_In.Text = T_In.ToString("#0.000");
                //}

        #endregion


            #region "COMMAND BUTTON EVENT ROUTINE:"
                //*************************************

                private void cmdPrint_Click(object sender, EventArgs e)
                //======================================================   
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }


                private void cmdOK_Click(object sender, EventArgs e)
                //==================================================
                {
                    SaveData();
                    this.Hide();

                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        modMain.gfrmSealDesignDetails.ShowDialog();
                    }
                }

      
                private void cmdCancel_Click(object sender, EventArgs e)
                //=======================================================
                {
                    this.Hide();
                }


                private void SaveData()
                //=====================
                {

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        {
                            if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Front, mEndTB[0], tbTB_Front))
                                return;
                        }

                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).L = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Front.Text));
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).LFlange = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLFlange_Front.Text));
                        }
                        else
                        {
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).L = modMain.ConvTextToDouble(txtL_Front.Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).LFlange = modMain.ConvTextToDouble(txtLFlange_Front.Text);
                        }

                        //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DimStart= modMain.ConvTextToDouble(txtDimStart_Front.Text); //BG 24APR12
                        //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).FaceOff_Assy= modMain.ConvTextToDouble(txtFaceOff_Assy_Front.Text);
                   

                        //  Mount Holes
                        //  ===========

                            //....Type
                            if(optMountHoles_Type_CBore_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            else if(optMountHoles_Type_Thru_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            else if(optMountHoles_Type_Thread_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {

                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_CBore =
                                                                                                    modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text));

                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Front.Checked;

                                if (chkMountHoles_Thread_Thru_Front.Checked)
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                       modMain.gProject.Unit.CMet_Eng(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).LFlange);
                                else
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                        modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Front.Text));
                            }
                            else
                            {
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_CBore =
                                                                                                    modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text);

                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Front.Checked;

                                if (chkMountHoles_Thread_Thru_Front.Checked)
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                       ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).LFlange;
                                else
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).MountHoles.Depth_Thread =
                                                                                                        modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Front.Text);
                            }

                        //  Back Relief
                        //  ===========

                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).BackRelief_Reqd = chkBackRelief_Reqd_Front.Checked;
                            if (chkBackRelief_Reqd_Front.Checked)
                            {
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).BackRelief_D = modMain.ConvTextToDouble(txtBackRelief_D_Front.Text);
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).BackRelief_Depth = modMain.ConvTextToDouble(txtBackRelief_Depth_Front.Text);
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).BackRelief_Fillet = modMain.ConvTextToDouble(txtBackRelief_Fillet_Front.Text);
                            }


                        //  Feed Groove
                        //  ===========
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).FeedGroove.Type= cmbFeedGroove_Type_Front.Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).FeedGroove.DBC= modMain.ConvTextToDouble(txtFeedGroove_DBC_Front.Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).FeedGroove.Dist_Chamf=modMain.ConvTextToDouble(txtFeedGroove_Dist_Chamf_Front.Text);               
                    }


                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                        {
                            if (!ValidateCBoreDepth(txtMountHoles_CBore_Depth_Back, mEndTB[1], tbTB_Back))
                                return;
                        }

                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).L = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Back.Text));
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).LFlange = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLFlange_Back.Text));
                        }
                        else
                        {
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).L = modMain.ConvTextToDouble(txtL_Back.Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).LFlange = modMain.ConvTextToDouble(txtLFlange_Back.Text);
                        }

                        //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DimStart = modMain.ConvTextToDouble(txtDimStart_Back.Text);
                        //((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).FaceOff_Assy = modMain.ConvTextToDouble(txtFaceOff_Assy_Back.Text);


                        //  Mount Holes
                        //  ===========

                            //....Type
                            if (optMountHoles_Type_CBore_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.C;

                            else if (optMountHoles_Type_Thru_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.H;

                            else if (optMountHoles_Type_Thread_Front.Checked)
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Type = clsEndConfig.clsMountHoles.eMountHolesType.T;

                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_CBore =
                                                                                                    modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text));

                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Back.Checked;

                                if (chkMountHoles_Thread_Thru_Back.Checked)
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                       modMain.gProject.Unit.CMet_Eng(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).LFlange);
                                else
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                        modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Back.Text));
                            }
                            else
                            {
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_CBore =
                                                                                                    modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text);

                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Thread_Thru = chkMountHoles_Thread_Thru_Back.Checked;

                                if (chkMountHoles_Thread_Thru_Back.Checked)
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                       ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).LFlange;
                                else
                                    ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).MountHoles.Depth_Thread =
                                                                                                        modMain.ConvTextToDouble(txtMountHoles_Thread_Depth_Back.Text);
                            }
                           

                        //  Back Relief
                        //  ===========
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).BackRelief_Reqd = chkBackRelief_Reqd_Back.Checked;
                            if (chkBackRelief_Reqd_Front.Checked)
                            {
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).BackRelief_D = modMain.ConvTextToDouble(txtBackRelief_D_Back.Text);
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).BackRelief_Depth = modMain.ConvTextToDouble(txtBackRelief_Depth_Back.Text);
                                ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).BackRelief_Fillet = modMain.ConvTextToDouble(txtBackRelief_Fillet_Back.Text);
                            }


                        //  Feed Groove
                        //  ===========
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).FeedGroove.Type = cmbFeedGroove_Type_Back.Text;
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).FeedGroove.DBC = modMain.ConvTextToDouble(txtFeedGroove_DBC_Back.Text);
                            ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).FeedGroove.Dist_Chamf = modMain.ConvTextToDouble(txtFeedGroove_Dist_Chamf_Back.Text);
                    }
                }

        #endregion

              
        #endregion


        #region "VALIDATION ROUTINE"
        //--------------------------

            private bool ValidateCBoreDepth(TextBox TxtBox_In, clsBearing_Thrust_TL TB_In, TabControl TbCtrl_In)
            //==================================================================================================
            {
                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    Double pCBoreDepth = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(TxtBox_In.Text));

                    if (pCBoreDepth <= TB_In.MountHoles.CBore_Depth_UpperLimit()
                        && pCBoreDepth >= TB_In.MountHoles.CBore_Depth_LowerLimit())
                        return true;

                    String pMSg = "Enter value between Lower Limit: " +
                                        modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(TB_In.MountHoles.CBore_Depth_LowerLimit()), "#0.000") +
                                        " to Upper Limit: " +
                                        modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(TB_In.MountHoles.CBore_Depth_UpperLimit()), "#0.000") + ".";
                    String pCaption = "CBore Depth Data Input Error";
                    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TbCtrl_In.SelectedIndex = 0;
                    TxtBox_In.Focus();
                }
                else
                {
                    Double pCBoreDepth = modMain.ConvTextToDouble(TxtBox_In.Text);

                    if (pCBoreDepth <= TB_In.MountHoles.CBore_Depth_UpperLimit()
                        && pCBoreDepth >= TB_In.MountHoles.CBore_Depth_LowerLimit())
                        return true;

                    String pMSg = "Enter value between Lower Limit: " +
                                        modMain.ConvDoubleToStr(TB_In.MountHoles.CBore_Depth_LowerLimit(), "#0.000") +
                                        " to Upper Limit: " +
                                        modMain.ConvDoubleToStr(TB_In.MountHoles.CBore_Depth_UpperLimit(), "#0.000") + ".";
                    String pCaption = "CBore Depth Data Input Error";
                    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TbCtrl_In.SelectedIndex = 0;
                    TxtBox_In.Focus();
                }

                return false;
            }

        #endregion

    }
}
