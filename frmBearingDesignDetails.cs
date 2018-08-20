
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmBearingDesignDetails                '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  02JUL13                                '
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace BearingCAD20
{
    public partial class frmBearingDesignDetails : Form
    {
        #region "MEMBER VARIABLES:"
        //************************

            //.....Local Bearing Object.
            private clsBearing_Radial_FP mBearing_Radial_FP;

            private TextBox[] mtxtEDM_Relief;
            private TextBox[] mTxtMount_Fixture_HolesAngOther_Front;
            private TextBox[] mTxtMount_Fixture_HolesAngOther_Back;

            private Boolean mblnL_ManuallyChanged = false;
            private Boolean mblnDepth_EndConfig_F_ManuallyChanged = false;       
            private Boolean mblnDepth_EndConfig_B_ManuallyChanged = false;

            private Boolean mblnRoughing_DimStart_FrontFace_ManuallyChanged = false;

            private Boolean mblnOilInlet_Annulus_D_ManuallyChanged = false;
            private Boolean mblnOilInlet_Annulus_L_ManuallyChanged = false;
            private Boolean mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = false;  
                                                
            private Boolean mblnEDM_Relief_Front_ManuallyChanged = false;                     
            private Boolean mblnEDM_Relief_Back_ManuallyChanged = false;                     

            private Boolean mblnAntiRotPin_Loc_Dist_Front_ManuallyChanged = false;          
            private Boolean mblnAntiRotPin_Loc_Bearing_Vert_ManuallyChanged = false;        

            private Boolean mblnAntiRotPin_Depth_Changed_ManuallyChanged = false;
            private Boolean mblnAntiRotPin_Stickout_Changed_ManuallyChanged = false;

            private Boolean mblnAntiRotPin_Spec_D_Desig_ManuallyChanged = false;

            private Boolean mblnMount_Holes_Thread_Depth_ManuallyChanged = false;         
         
            private Boolean mblnTempSensor_CanLength_ManuallyChanged = false;

        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmBearingDesignDetails()
            //=============================
            {
                InitializeComponent();
                mtxtEDM_Relief = new TextBox[] { txtEDMRelief_Front, txtEDMRelief_Back };
            }

        #endregion


        #region "FORM EVENT ROUTINES:"
        //****************************

            private void frmBearingDesgnDetails_Load(object sender, EventArgs e)
            //==================================================================    
            {
                mblnL_ManuallyChanged = false;
                mblnDepth_EndConfig_F_ManuallyChanged = false;
                mblnDepth_EndConfig_B_ManuallyChanged  = false; 
                      
                //....Local Object.
                SetLocalObject();              

                //....Initialize 
                InitializeControls();
               

                //  OilInlet:
                //  --------
                Load_OilInlet_cmbOrificeStartPos();                                
                Load_OilInlet_cmbAnnulus_LH_Ratio();                             
                Load_OilInlet_Orifice_Dist_Holes();


                //  S/L Hardware:
                //  -------------
                Load_SL_HardWare();  


                //  Anti Rotation Pin:
                //  ------------------
                mblnAntiRotPin_Depth_Changed_ManuallyChanged = false;
                mblnAntiRotPin_Stickout_Changed_ManuallyChanged = false;

                Load_AntiRotPin();
                optDFit.Checked = true;          


                //  Mounting:
                //  ---------
                mblnMount_Holes_Thread_Depth_ManuallyChanged = false;
                SetMountFixture(mBearing_Radial_FP);
                Load_Mount_Fixture("Front");
                Load_Mount_Fixture("Back");                


                //  Temp Sensor: 
                //  ------------
                mblnTempSensor_CanLength_ManuallyChanged = false;
                Load_TempSensor_Loc();   
                Load_TempSensor_Count();


                //....Set Control.
                SetControls();

                //....Display Data.
                DisplayData();
            }


            private void frmBearingDesignDetails_Activated(object sender, EventArgs e)
            //========================================================================
            {
                if (modMain.gProject.Product.Accessories.TempSensor.Supplied)
                {
                    chkTempSensor_Exists.Checked = true;
                    chkTempSensor_Exists.Enabled = false;
                    cmbTempSensor_Count.Enabled = false;
                    cmbTempSensor_Count.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.TempSensor.Count);
                }
                else
                {
                    chkTempSensor_Exists.Enabled = true;
                    chkTempSensor_Exists.Checked = mBearing_Radial_FP.TempSensor.Exists;
                    cmbTempSensor_Count.Enabled = true;
                    cmbTempSensor_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.TempSensor.Count);
                }
            }
        

            private void SetLocalObject()
            //===========================
            {
               mBearing_Radial_FP = (clsBearing_Radial_FP)((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clone();
            }


            #region "....INITIALIZE CONTROLS ROUTINES:"

                private void InitializeControls()
                //===============================
                {
                    SetTabPages(mBearing_Radial_FP.SplitConfig, tabSplitLineHardware);

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        //....Both End Configs are Thrust Bearings. Temp. Sensor doesn't exist.
                        SetTabPages(false, tabTempSensor);
                    }
                    else
                    {   //....At least, one of the End Configs is a Seal.
                        SetTabPages(true, tabTempSensor);
                    }

                    //....Initialize Checkboxes.
                    //
                        //....Mount Fixture:                        
                        chkMount_Fixture_Screw_LenLim_Front.Checked = false;
                        chkMount_Fixture_Screw_LenLim_Back.Checked = false;

                        lblMsg_MountFixture_EquiSpaced_Front.Visible = false;
                        lblMsg_MountFixture_EquiSpaced_Back.Visible = false;

                        //....S/L Hardware:                        
                        chkSL_Dowel_LenLim.Checked = false;
                        chkSL_Screw_LenLim.Checked = false;


                    //....Create TextBox Arrays:
                    //
                    mTxtMount_Fixture_HolesAngOther_Front = new TextBox[] {txtMount_Fixture_HolesAngOther1_Front,txtMount_Fixture_HolesAngOther2_Front,
                                                                           txtMount_Fixture_HolesAngOther3_Front,txtMount_Fixture_HolesAngOther4_Front,
                                                                           txtMount_Fixture_HolesAngOther5_Front,txtMount_Fixture_HolesAngOther6_Front,
                                                                           txtMount_Fixture_HolesAngOther7_Front};

                    mTxtMount_Fixture_HolesAngOther_Back = new TextBox[] {txtMount_Fixture_HolesAngOther1_Back,txtMount_Fixture_HolesAngOther2_Back,
                                                                          txtMount_Fixture_HolesAngOther3_Back,txtMount_Fixture_HolesAngOther4_Back,
                                                                          txtMount_Fixture_HolesAngOther5_Back,txtMount_Fixture_HolesAngOther6_Back,
                                                                          txtMount_Fixture_HolesAngOther7_Back};
                }


                private void SetTabPages(Boolean Checked_In, TabPage TabPage_In)
                //==============================================================
                {
                    TabPage pTabRoughing = tabRoughing;
                    TabPage pTabOilInlet = tabOilInlet;
                    TabPage pTabWebRelief = tabWebRelief;
                    TabPage pTabAntiRotationPin = tabAntiRotationPin;
                    TabPage pTabSplitLineHardware = tabSplitLineHardware;   //BG 26NOV12
                    TabPage pTabFlange = tabFlange;
                    TabPage pTabMountingHoles = tabMounting;
                    TabPage pTabTempSensorHoles = tabTempSensor;
                    TabPage pTabEDM = tabEDM;

                    if (!Checked_In)
                    {
                        tbBearingDesignDetails.TabPages.Remove(TabPage_In);
                    }

                    Boolean pTab_Exist = false;
                    foreach (TabPage pTp in tbBearingDesignDetails.TabPages)
                    {
                        if (pTp.Text == TabPage_In.Text)
                        {
                            pTab_Exist = true;
                        }
                    }

                   
                    if ((Checked_In) && (!pTab_Exist))
                    {
                        tbBearingDesignDetails.TabPages.Clear();
                        tbBearingDesignDetails.TabPages.Add(pTabRoughing);
                        tbBearingDesignDetails.TabPages.Add(pTabOilInlet);
                        tbBearingDesignDetails.TabPages.Add(pTabWebRelief);
                        tbBearingDesignDetails.TabPages.Add(pTabAntiRotationPin);       
                        tbBearingDesignDetails.TabPages.Add(pTabSplitLineHardware);
                        tbBearingDesignDetails.TabPages.Add(pTabFlange);
                        tbBearingDesignDetails.TabPages.Add(pTabMountingHoles);
                        tbBearingDesignDetails.TabPages.Add(pTabTempSensorHoles);
                        tbBearingDesignDetails.TabPages.Add(pTabEDM);
                    }

                    tbBearingDesignDetails.Refresh();
                }

            #endregion


            #region "....SET CONTROLS ROUTINES:"

                private void SetControls()
                //=======================                           
                {
                    Boolean pblnSet = false;

                    //....BG 01JUL13
                    //if (modMain.gProject.Status == "Open" &&
                    //   (modMain.gUser.Privilege == "Engineering" || modMain.gUser.Privilege == "Designer"))

                    //....BG 01JUL13
                    if (modMain.gProject.Status == "Open" &&
                       (modMain.gUser.Role == "Engineer" || modMain.gUser.Role == "Designer"))
                    {
                        pblnSet = true;
                    }
                    else if (modMain.gProject.Status == "Closed" || 
                            (modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer"))
                    {
                        pblnSet = false;
                    }

                    SetControlStatus(pblnSet);


                    //....Oil Inlet
                    if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                    {
                        pblnSet = true;
                    }
                    else
                    {
                        pblnSet = false;
                    }
                    
                    lblSeparator.Visible = pblnSet;
                    lblOrificeHoles1.Visible = pblnSet;
                    lblOrificeHoles2.Visible = pblnSet;
                    lblOrificeHoles3.Visible = pblnSet;
                    lblOilInlet_DistBetFeedHoles.Visible = pblnSet;
                    txtOilInlet_Orifice_Dist_Holes.Visible = pblnSet;


                    //........Annulus
                    grpOilInlet_Annulus.Visible = chkOilInlet_Annulus_Exists.Checked;

                    //....Flange
                    SetControls_Flange();

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        lblDepth_Front.Text = "Seal";
                        lblDepth_Back.Text  = "Seal";
                        lblMount_Fixture_D_Finish_Front.Text = "Seal OD";
                        lblMount_Fixture_D_Finish_Back.Text  = "Seal OD";
                    }
                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        lblDepth_Front.Text = "Seal";
                        lblDepth_Back.Text  = "T/B";
                        lblMount_Fixture_D_Finish_Front.Text = "Seal OD";
                        lblMount_Fixture_D_Finish_Back.Text  = "T/B OD";
                    }
                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        lblDepth_Front.Text = "T/B";
                        lblDepth_Back.Text  = "Seal";
                        lblMount_Fixture_D_Finish_Front.Text = "T/B OD";
                        lblMount_Fixture_D_Finish_Back.Text  = "Seal OD";
                    }
                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        lblDepth_Front.Text = "T/B";
                        lblDepth_Back.Text  = "T/B";
                        lblMount_Fixture_D_Finish_Front.Text = "T/B OD";
                        lblMount_Fixture_D_Finish_Back.Text  = "T/B OD";
                    }
                }


                private void SetControlStatus(Boolean Enabled_In)
                //===============================================
                {
                    //....Bearing Length.
                        txtL.ReadOnly = !Enabled_In;
                        txtDepth_EndConfig_Front.ReadOnly = !Enabled_In;
                        txtDepth_EndConfig_Back.ReadOnly  = !Enabled_In;

                    //....Roughing
                        txtRoughing_DimStart_FrontFace.ReadOnly = !Enabled_In;

                    //....Oil Inlet.
                        cmbOilInlet_Count_MainOilSupply.Enabled    = Enabled_In;
                        cmbOilInlet_Orifice_StartPos.Enabled       = Enabled_In;
                        txtOilInlet_Orifice_Loc_FrontFace.ReadOnly = !Enabled_In;

                        cmbOilInlet_Annulus_Ratio_L_H.Enabled = Enabled_In;
                        txtOilInlet_Annulus_D.ReadOnly        = !Enabled_In;
                        txtOilInlet_Annulus_Loc_Back.ReadOnly = !Enabled_In;
                        txtOilInlet_Annulus_L.ReadOnly        = !Enabled_In;
                        chkOilInlet_Annulus_Exists.Enabled = Enabled_In;

                        if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                        {
                            txtOilInlet_Orifice_Dist_Holes.ReadOnly = !Enabled_In;
                        }

                    //....Web Relief.
                        chkMillRelief_Exists.Enabled = Enabled_In;
                        cmbMillRelief_D_Desig.Enabled = Enabled_In;

                        //....EDM Relief
                        txtEDMRelief_Front.Enabled = Enabled_In;
                        txtEDMRelief_Back.Enabled = Enabled_In;

                        if (!Enabled_In)
                        {
                            txtEDMRelief_Front.BackColor = txtMillRelief_D_PadRelief.BackColor;
                            txtEDMRelief_Back.BackColor = txtMillRelief_D_PadRelief.BackColor;
                        }
                        else
                        {
                            txtEDMRelief_Front.BackColor = Color.White;
                            txtEDMRelief_Back.BackColor = Color.White;
                        }


                    //....Split Line Hardware.
                    //
                        //Screw:
                        //------
                            cmbSL_Screw_Spec_Type.Enabled = Enabled_In;
                            cmbSL_Screw_Spec_D_Desig.Enabled = Enabled_In;
                            cmbSL_Screw_Spec_Pitch.Enabled = Enabled_In;
                            cmbSL_Screw_Spec_L.Enabled = Enabled_In;
                            cmbSL_Screw_Spec_Mat.Enabled = Enabled_In;
                            chkSL_Screw_LenLim.Enabled = Enabled_In;
                            cmbSL_Screw_Spec_UnitSystem.Enabled = Enabled_In;

                            txtSL_LScrew_Loc_Center.ReadOnly = !Enabled_In;
                            txtSL_LScrew_Loc_Front.ReadOnly = !Enabled_In;
                            txtSL_RScrew_Loc_Center.ReadOnly = !Enabled_In;
                            txtSL_RScrew_Loc_Front.ReadOnly = !Enabled_In;


                        //Dowel:
                        //-----
                            cmbSL_Dowel_Spec_Type.Enabled = Enabled_In;
                            cmbSL_Dowel_Spec_D_Desig.Enabled = Enabled_In;
                            cmbSL_Dowel_Spec_L.Enabled = Enabled_In;
                            cmbSL_Dowel_Spec_Mat.Enabled = Enabled_In;
                            chkSL_Dowel_LenLim.Enabled = Enabled_In;
                            cmbSL_Dowel_Spec_UnitSystem.Enabled = Enabled_In;

                            txtSL_LDowel_Loc_Center.ReadOnly = !Enabled_In;
                            txtSL_LDowel_Loc_Front.ReadOnly = !Enabled_In;
                            txtSL_RDowel_Loc_Center.ReadOnly = !Enabled_In;
                            txtSL_RDowel_Loc_Front.ReadOnly = !Enabled_In;


                    //....Flange
                        chkFlange_Exists.Enabled = Enabled_In;
                        txtFlange_D.ReadOnly = !Enabled_In;
                        txtFlange_Wid.ReadOnly = !Enabled_In;
                        txtFlange_DimStart_Front.ReadOnly = !Enabled_In;


                    //....Anti Rotation Pin.
                    //
                        //Location:
                        //---------
                            txtAntiRotPin_Loc_Angle.ReadOnly = !Enabled_In;
                            txtAntiRotPin_Loc_Dist_Front.ReadOnly = !Enabled_In;
                            cmbAntiRotPin_Loc_Bearing_SL.Enabled = Enabled_In;
                            //txtAntiRotPin_Loc_Bearing_SL.Enabled = Enabled_In;
                            cmbAntiRotPin_Loc_Bearing_Vert.Enabled = Enabled_In;
                            cmbAntiRotPin_Loc_CasingSL.Enabled = Enabled_In;
                            txtAntiRotPin_Loc_Offset.ReadOnly = !Enabled_In;

                            grpInsertedOn.Enabled = Enabled_In;


                        //Hardware:
                        //---------
                            cmbAntiRotPin_Spec_Type.Enabled = Enabled_In;
                            cmbAntiRotPin_Spec_D_Desig.Enabled = Enabled_In;
                            cmbAntiRotPin_Spec_L.Enabled = Enabled_In;
                            cmbAntiRotPin_Spec_Mat.Enabled = Enabled_In;

                            txtAntiRotPin_Depth.ReadOnly = !Enabled_In;
                            txtAntiRotPin_Stickout.ReadOnly = !Enabled_In;


                    //....Mounting Holes.                    
                        chkMount_Holes_GoThru.Enabled = Enabled_In;
                        grpMount_Holes_Bolting.Enabled = Enabled_In;


                        //  Front:
                        //  ------
                        //
                            txtMount_Holes_Thread_Depth_Front.ReadOnly = !Enabled_In;

                            //........Seal Splitting Fixture.
                            chkMount_Fixture_Candidates_Chosen_Front.Enabled = Enabled_In;      // BG 08APR13
                            if (modMain.gProject.Status == "Closed" || modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer")
                            {
                                //chkMount_Fixture_Candidates_Chosen_Front.Enabled = Enabled_In;    // BG 08APR13
                                if (!Enabled_In)
                                    txtMount_Fixture_DBC_Front.BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                else
                                    txtMount_Fixture_DBC_Front.BackColor = Color.White;

                                txtMount_Fixture_DBC_Front.ReadOnly = !Enabled_In;
                                if (!Enabled_In)
                                    txtMount_Fixture_D_Finish_Front.BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                else
                                    txtMount_Fixture_D_Finish_Front.BackColor = Color.White;

                                txtMount_Fixture_D_Finish_Front.ReadOnly = !Enabled_In;

                                for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Front.Length; i++)
                                {
                                    mTxtMount_Fixture_HolesAngOther_Front[i].ReadOnly = !Enabled_In;
                                    if (!Enabled_In)
                                        mTxtMount_Fixture_HolesAngOther_Front[i].BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                    else
                                        mTxtMount_Fixture_HolesAngOther_Front[i].BackColor = Color.White;
                                }
                            }

                            cmbMount_Fixture_PartNo_Front.Enabled = Enabled_In;

                            cmbMount_Fixture_HolesAngStart_Front.Enabled = Enabled_In;
                            chkMount_Fixture_HolesEquiSpaced_Front.Enabled = Enabled_In;
                            cmbMount_Fixture_HolesCount_Front.Enabled = Enabled_In;
                            chkMount_Fixture_Screw_LenLim_Front.Enabled = Enabled_In;

                            cmbMount_Fixture_Screw_Type_Front.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_D_Desig_Front.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_Pitch_Front.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_L_Front.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_Mat_Front.Enabled = Enabled_In;


                        //  Back:
                        //  -----

                            txtMount_Holes_Thread_Depth_Back.ReadOnly = !Enabled_In;

                            //........Seal Splitting Fixture.
                            chkMount_Fixture_Candidates_Chosen_Back.Enabled = Enabled_In;       // BG 08APR13
                            if (modMain.gProject.Status == "Closed" || modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer")
                            {
                                //chkMount_Fixture_Candidates_Chosen_Back.Enabled = Enabled_In;     // BG 08APR13
                                if (!Enabled_In)
                                    txtMount_Fixture_DBC_Back.BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                else
                                    txtMount_Fixture_DBC_Back.BackColor = Color.White;

                                txtMount_Fixture_DBC_Back.ReadOnly = !Enabled_In;
                                if (!Enabled_In)
                                    txtMount_Fixture_D_Finish_Back.BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                else
                                    txtMount_Fixture_D_Finish_Back.BackColor = Color.White;

                                txtMount_Fixture_D_Finish_Back.ReadOnly = !Enabled_In;

                                for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Back.Length; i++)
                                {
                                    mTxtMount_Fixture_HolesAngOther_Back[i].ReadOnly = !Enabled_In;
                                    if (!Enabled_In)
                                        mTxtMount_Fixture_HolesAngOther_Back[i].BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                    else
                                        mTxtMount_Fixture_HolesAngOther_Back[i].BackColor = Color.White;
                                }
                            }

                            cmbMount_Fixture_PartNo_Back.Enabled = Enabled_In;

                            cmbMount_Fixture_HolesAngStart_Back.Enabled = Enabled_In;
                            chkMount_Fixture_HolesEquiSpaced_Back.Enabled = Enabled_In;
                            cmbMount_Fixture_HolesCount_Back.Enabled = Enabled_In;
                            chkMount_Fixture_Screw_LenLim_Back.Enabled = Enabled_In;

                            cmbMount_Fixture_Screw_Type_Back.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_D_Desig_Back.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_Pitch_Back.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_L_Back.Enabled = Enabled_In;
                            cmbMount_Fixture_Screw_Mat_Back.Enabled = Enabled_In;


                    //....Temp. Sensor Holes.
                        txtTempSensor_CanLength.ReadOnly = !Enabled_In;

                        if (modMain.gProject.Status == "Closed" || modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer")
                        {
                            chkTempSensor_Exists.Enabled = Enabled_In;
                            cmbTempSensor_Count.Enabled = Enabled_In;
                        }
                        cmbTempSensor_Loc.Enabled = Enabled_In;
                        txtTempSensor_D.ReadOnly = !Enabled_In;
                        txtTempSensor_AngStart.ReadOnly = !Enabled_In;
                        txtTempSensor_Depth.ReadOnly = !Enabled_In;

                        if (!Enabled_In)
                            txtTempSensor_AngStart.BackColor =
                                txtTempSensor_AngBet.BackColor;
                        else
                            txtTempSensor_AngStart.BackColor = Color.White;


                    //....EDM
                        txtEDM_Pad_RFilletBack.ReadOnly = !Enabled_In;
                }


                #region "WEB RELIEF:"

                    private void SetControl_Web_MillRelief()
                    //======================================
                    {
                        lblWebMillRelief_D.Visible = chkMillRelief_Exists.Checked;
                        cmbMillRelief_D_Desig.Visible = chkMillRelief_Exists.Checked;
                    }

                #endregion


                #region "FLANGE:"

                    private void SetControls_Flange()
                    //================================
                    {
                        lblFlange_D.Visible = chkFlange_Exists.Checked;
                        txtFlange_D.Visible = chkFlange_Exists.Checked;
                        lblFlange_Wid.Visible = chkFlange_Exists.Checked;
                        txtFlange_Wid.Visible = chkFlange_Exists.Checked;
                        lblFlange_DimStart_Front.Visible = chkFlange_Exists.Checked;
                        txtFlange_DimStart_Front.Visible = chkFlange_Exists.Checked;
                        lblFlange_DimStart_Front1.Visible = chkFlange_Exists.Checked;
                        grpInsertedOn.Visible = chkFlange_Exists.Checked;
                    }
               #endregion

            #endregion


            #region "Display Data:"

                private void DisplayData()      
                //========================
                {
                    //....Reset Msg Text.
                    lblMsg1.Text = "";
               
                    //....Set TabPage Index.
                    tbBearingDesignDetails.SelectedIndex = 0;


                    #region "General Header:"
                    //-----------------------

                        //  Bearing Length:
                            txtL.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.L, modMain.gUnit.MFormat);   


                        //  Depths:
                        //  -------
                            if (mBearing_Radial_FP.Depth_EndConfig[0] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndConfig[0] - mBearing_Radial_FP.Calc_Depth_EndConfig()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Depth_EndConfig[0], "#0.000");
                                }
                                else
                                {
                                    txtDepth_EndConfig_Front.Text = mBearing_Radial_FP.Calc_Depth_EndConfig().ToString("#0.000");
                                }
                            }                    

                            if (mBearing_Radial_FP.Depth_EndConfig[1] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndConfig[1] - mBearing_Radial_FP.Calc_Depth_EndConfig()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Depth_EndConfig[1], "#0.000");
                                }
                                else
                                {
                                    txtDepth_EndConfig_Back.Text = mBearing_Radial_FP.Calc_Depth_EndConfig().ToString("#0.000");
                                }
                            }

                    #endregion


                    #region "Roughing"
                    //-----------------

                        //....Start Dim                   
                        txtRoughing_DimStart_FrontFace.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.DimStart_FrontFace, "#0.000");

                        //....Fixture Hole Location B                    
                        txtRoughing_Loc_Fixture_B.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Loc_Fixture_B(), "#0.000");

                    #endregion


                    #region  "OilInlet"
                    //  ---------------

                        if (mBearing_Radial_FP.OilInlet.Count_MainOilSupply != 0.0F || 
                            modMain.ConvIntToStr(mBearing_Radial_FP.OilInlet.Count_MainOilSupply) != "")
                            cmbOilInlet_Count_MainOilSupply.Text = modMain.ConvIntToStr(mBearing_Radial_FP.OilInlet.Count_MainOilSupply);
                        else
                            cmbOilInlet_Count_MainOilSupply.SelectedIndex = 0;

                        //....Orifice
                            cmbOilInlet_Orifice_StartPos.Text = mBearing_Radial_FP.OilInlet.Orifice.StartPos.ToString();
                            txtOilInlet_Orifice_DDrill_CBore.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.DDrill_CBore, "#0.000");
                            txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.Loc_FrontFace, "#0.000#");
                            txtOilInlet_Orifice_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.L, "#0.000");
                            txtOilInlet_Orifice_AngStart_BDV.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.AngStart_BDV, "#0.0");
                                 

                        //....Annulus 
                            //chkOilInlet_Annulus_Exists.Checked = mBearing_Radial_FP.OilInlet.Annulus.Exists;      //BG 01JUL13

                            //BG 02JUL13
                            if (!modMain.gDB.ProjectNo_Exists(modMain.gProject.No, modMain.gProject.No_Suffix, "tblProject_Details"))    
                            {
                                mBearing_Radial_FP.OilInlet.Annulus_Exists = true;
                            }

                            chkOilInlet_Annulus_Exists.Checked = mBearing_Radial_FP.OilInlet.Annulus.Exists;      //BG 02JUL13

                       

                            if (chkOilInlet_Annulus_Exists.Checked)
                            {
                                txtOilInlet_Annulus_D.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.D, modMain.gUnit.MFormat);
                                txtOilInlet_Annulus_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.L, modMain.gUnit.MFormat);
                                if (mBearing_Radial_FP.OilInlet.Annulus.D < modMain.gcEPS && mBearing_Radial_FP.OilInlet.Annulus.L < modMain.gcEPS)
                                {
                                    cmbOilInlet_Annulus_Ratio_L_H.SelectedIndex = 0;
                                    //....Decide whether it is calculated or not.
                                    IsOilInlet_Annulus_D_Calc(ref txtOilInlet_Annulus_D);
                                    IsOilInlet_Annulus_L_Calc(ref txtOilInlet_Annulus_L);
                                }
                                else
                                {
                                    cmbOilInlet_Annulus_Ratio_L_H.Text = mBearing_Radial_FP.OilInlet.Calc_Annulus_Ratio_L_H
                                                                        (modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text)).ToString("#0.00");      //BG 16MAY12
                                    
                                    txtOilInlet_Annulus_D.ForeColor = Color.Black;
                                    txtOilInlet_Annulus_L.ForeColor = Color.Black;
                                }

                                txtOilInlet_Annulus_V.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus_V(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text),modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text)), "#0.000");
                                txtOilInlet_Annulus_Loc_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back, "#0.000");                           
                            }

                            //....If Orifice_Count =   Count_Pad, Dist_FeedHole = 0
                            //                     = 2*Count_Pad, Dist_FeedHole = non-zero.
                            if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                            {
                                txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes, "#0.000");
                            }

                    #endregion


                    #region "Web Relief:"
                    //  -----------------

                        txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.D_PadRelief(), "#0.000");    
                        chkMillRelief_Exists.Checked = mBearing_Radial_FP.MillRelief.Exists;
                        SetControl_Web_MillRelief();
                        cmbMillRelief_D_Desig.Text = mBearing_Radial_FP.MillRelief.D_Desig;

                        //....EDM Relief
                        for (int i = 0; i < 2; i++)         //BG 06DEC12
                        {
                            mtxtEDM_Relief[i].Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.EDM_Relief[i], "#0.000");
                        }

                   #endregion

                    
                    #region "SplitLine Hardware:"
                    //  -------------------------  
                  
                        //....Screw.
                        cmbSL_Screw_Spec_UnitSystem.Text =mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString();      //BG 26MAR12

                        if (mBearing_Radial_FP.SL.Screw_Spec.Type != null && mBearing_Radial_FP.SL.Screw_Spec.Type != "")
                            cmbSL_Screw_Spec_Type.Text = mBearing_Radial_FP.SL.Screw_Spec.Type;
                        else if (cmbSL_Screw_Spec_Type.Items.Count > 0)
                            cmbSL_Screw_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw_Spec.Mat != null && mBearing_Radial_FP.SL.Screw_Spec.Mat != "")
                            cmbSL_Screw_Spec_Mat.Text = mBearing_Radial_FP.SL.Screw_Spec.Mat;
                        else if (cmbSL_Screw_Spec_Mat.Items.Count > 0)
                            cmbSL_Screw_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw_Spec.D_Desig != null && mBearing_Radial_FP.SL.Screw_Spec.D_Desig != "")
                            cmbSL_Screw_Spec_D_Desig.Text = mBearing_Radial_FP.SL.Screw_Spec.D_Desig;
                        else if (cmbSL_Screw_Spec_D_Desig.Items.Count > 0)
                            cmbSL_Screw_Spec_D_Desig.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw_Spec.Pitch != 0.0F)
                            cmbSL_Screw_Spec_Pitch.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Screw_Spec.Pitch, "#0.000");
                        else if (cmbSL_Screw_Spec_Pitch.Items.Count > 0)
                            cmbSL_Screw_Spec_Pitch.SelectedIndex = 0;                       

                        Update_SL_Screw_L();
                                       
                        //Check_SpLineThread_LLim(mBearing.SL.Screw_Spec.L, Get_SL_Screw_L(mBearing));         
                        //Check_SL_Screw_LLim(mBearing.SL.Screw_Spec.L, mBearing.SL.Thread_L_LowerLimit());     //SG 25MAY12

                        if (mBearing_Radial_FP.SL.Screw_Spec.L != 0.0F)
                            cmbSL_Screw_Spec_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Screw_Spec.L, "#0.00#");
                        else if (cmbSL_Screw_Spec_L.Items.Count > 0)
                            cmbSL_Screw_Spec_L.SelectedIndex = 0;

                        Check_SL_Screw_LLim(mBearing_Radial_FP.SL.Screw_Spec.L, mBearing_Radial_FP.SL.Thread_L_LowerLimit());

                        txtSL_LScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.LScrew_Loc.Center, "#0.000");
                        txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.LScrew_Loc.Front, "#0.000");

                        txtSL_RScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.RScrew_Loc.Center, "#0.000");
                        txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.RScrew_Loc.Front, "#0.000");

                        txtSL_Thread_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_Depth, "#0.000");
                        txtSL_CBore_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.CBore_Depth, "#0.000");
                                       

                        //....Dowel. 
                        cmbSL_Dowel_Spec_UnitSystem.SelectedIndex = cmbSL_Dowel_Spec_UnitSystem.Items.IndexOf(mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString());      //BG 26MAR12
       
                        if (mBearing_Radial_FP.SL.Dowel_Spec.Type != null && mBearing_Radial_FP.SL.Dowel_Spec.Type != "")
                            cmbSL_Dowel_Spec_Type.Text = mBearing_Radial_FP.SL.Dowel_Spec.Type;
                        else if (cmbSL_Dowel_Spec_Type.Items.Count > 0)
                            cmbSL_Dowel_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Dowel_Spec.Mat != null && mBearing_Radial_FP.SL.Dowel_Spec.Mat != "")
                            cmbSL_Dowel_Spec_Mat.Text = mBearing_Radial_FP.SL.Dowel_Spec.Mat;
                        else if (cmbSL_Dowel_Spec_Mat.Items.Count > 0)
                            cmbSL_Dowel_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Dowel_Spec.D_Desig != null && mBearing_Radial_FP.SL.Dowel_Spec.D_Desig != "")
                            cmbSL_Dowel_Spec_D_Desig.Text = mBearing_Radial_FP.SL.Dowel_Spec.D_Desig;
                        else if (cmbSL_Dowel_Spec_D_Desig.Items.Count >0)
                            cmbSL_Dowel_Spec_D_Desig.SelectedIndex = 0;
                                    
                        Update_SL_Dowel_L();                    

                        if (mBearing_Radial_FP.SL.Dowel_Spec.L != 0.0F)
                            cmbSL_Dowel_Spec_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Dowel_Spec.L, "#0.00#");
                        else if (cmbSL_Dowel_Spec_L.Items.Count > 0)
                            cmbSL_Dowel_Spec_L.SelectedIndex = 0;

                        Check_SL_Dowel_LLim(mBearing_Radial_FP.SL.Dowel_Spec.L, mBearing_Radial_FP.SL.Pin_L_LowerLimit());

                        txtSL_LDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.LDowel_Loc.Center, "#0.000");
                        txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.LDowel_Loc.Front, "#0.000");

                        txtSL_RDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.RDowel_Loc.Center, "#0.000");
                        txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.RDowel_Loc.Front, "#0.000");

                        txtSL_Dowel_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Dowel_Depth, "#0.000");
                
                    #endregion


                    #region "Flange:"
                    //  ------
                        chkFlange_Exists.Checked = mBearing_Radial_FP.Flange.Exists;
                        if (mBearing_Radial_FP.Flange.Exists)
                        {
                            txtFlange_D.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.D, "#0.000");
                            txtFlange_Wid.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.Wid, "#0.000");
                            txtFlange_DimStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.DimStart_Front, "#0.000");
                        }

                    #endregion
                    
                
                    #region "Anti Rotation Pin:"
                    //  -----------------------

                        txtAntiRotPin_Loc_Dist_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Loc_Dist_Front, modMain.gUnit.MFormat);     

                        txtAntiRotPin_Loc_Angle.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Loc_Angle, "");

                        if (mBearing_Radial_FP.Flange.Exists)
                        {
                            grpInsertedOn.Visible = true;
                            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.InsertedOn == clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn.FD)
                            {
                                optDFit.Checked = true;
                            }
                            else
                            {
                                optFlange.Checked = true;
                            }
                        }
                        else
                        {
                            grpInsertedOn.Visible = false;
                        }
                    
      
                        cmbAntiRotPin_Loc_CasingSL.Text = mBearing_Radial_FP.AntiRotPin.Loc_Casing_SL.ToString();
                        txtAntiRotPin_Loc_Offset.Text = mBearing_Radial_FP.AntiRotPin.Loc_Offset.ToString("0.000");

                        if(mBearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                        {
                            cmbAntiRotPin_Loc_Bearing_Vert.Text = "Left";
                        }
                        else if (mBearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R)
                        {
                            cmbAntiRotPin_Loc_Bearing_Vert.Text = "Right";
                        }

                        cmbAntiRotPin_Loc_Bearing_SL.Text = mBearing_Radial_FP.AntiRotPin.Loc_Bearing_SL.ToString();

                        //...Pin
                        cmbAntiRotPin_Spec_UnitSystem.Text = mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString();              //BG 26MAR12

                        if (mBearing_Radial_FP.AntiRotPin.Spec.Type != null && mBearing_Radial_FP.AntiRotPin.Spec.Type != "")
                            cmbAntiRotPin_Spec_Type.Text = mBearing_Radial_FP.AntiRotPin.Spec.Type;
                        else if (cmbAntiRotPin_Spec_Type.Items.Count > 0)
                            cmbAntiRotPin_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.AntiRotPin.Spec.Mat != null && mBearing_Radial_FP.AntiRotPin.Spec.Mat != "")
                            cmbAntiRotPin_Spec_Mat.Text = mBearing_Radial_FP.AntiRotPin.Spec.Mat;
                        else if (cmbAntiRotPin_Spec_Mat.Items.Count > 0)
                            cmbAntiRotPin_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.AntiRotPin.Spec.D_Desig != null && mBearing_Radial_FP.AntiRotPin.Spec.D_Desig != "")
                            cmbAntiRotPin_Spec_D_Desig.Text = mBearing_Radial_FP.AntiRotPin.Spec.D_Desig;
                        else if (cmbAntiRotPin_Spec_D_Desig.Items.Count > 0)
                            cmbAntiRotPin_Spec_D_Desig.SelectedIndex = 0;

                        if (mBearing_Radial_FP.AntiRotPin.Spec.L != 0.0F)
                            cmbAntiRotPin_Spec_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Spec.L, "#0.000");      
                        else if (cmbAntiRotPin_Spec_L.Items.Count > 0)
                            cmbAntiRotPin_Spec_L.SelectedIndex = 0;

                        txtAntiRotPin_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Depth, "#0.000");
                        txtAntiRotPin_Stickout.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Stickout, "#0.000");

                    #endregion
                                         

                    #region "Mounting Holes:"
                    //  --------------------
                    
                        //....GoThru'
                        chkMount_Holes_GoThru.Checked = mBearing_Radial_FP.Mount.Holes_GoThru;
                        SetControls_Mount_Holes_GoThru(mBearing_Radial_FP.Mount.Holes_GoThru);


                        for (int i = 0; i < 2; i++)
                        {
                            //....Front
                            if (chkMount_Holes_Bolting_Front.Checked && i == 0)                       
                            {
                                txtMount_Holes_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Holes_Thread_Depth[i], "#0.000");
                                chkMount_Fixture_Candidates_Chosen_Front.Checked = mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[i];

                                //....Populate candidates mount fixture.
                                Populate_Fixture(mBearing_Radial_FP, i);

                                if (chkMount_Fixture_Candidates_Chosen_Front.Checked)
                                {
                                    //....Part No
                                    cmbMount_Fixture_PartNo_Front.Text = mBearing_Radial_FP.Mount.Fixture[i].PartNo;

                                    //....Part No Indx.
                                    int pIndx = Get_Fixture_Indx(mBearing_Radial_FP,mBearing_Radial_FP.Mount.Fixture[i].PartNo);

                                    if (pIndx != -1)
                                        Display_Candidate_Fixture_Details(pIndx, mBearing_Radial_FP, i); //....to be Reviewed later

                                    else
                                        //lblMsg1.Text = "Please choose an appropriate candidate fixture.";
                                        if (cmbMount_Fixture_PartNo_Front.Items.Count > 0)
                                            cmbMount_Fixture_PartNo_Front.SelectedIndex = 0;

                                    //.....Ang Start.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp_Chosen)
                                        //cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp, "");   
                                        cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp(), "");      //BG 24AUG12  
                                    else
                                        cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart, "");
                                
                                    cmbMount_Fixture_HolesAngStart_Front.ForeColor = Color.Magenta;

                                
                                }
                                else
                                {
                                    //.....DBC.
                                    IsMount_Fixture_DBC_Calc(ref txtMount_Fixture_DBC_Front);

                                    //....Seal DO.
                                    IsMount_Fixture_DO_Calc(ref txtMount_Fixture_D_Finish_Front);

                                    //....Wall thick
                                    txtMount_Fixture_WallT_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.TWall_BearingCB(i), "#0.000");

                                    //....Count.
                                    cmbMount_Fixture_HolesCount_Front.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesCount);

                                    //....EquiSpaced.
                                    chkMount_Fixture_HolesEquiSpaced_Front.Checked = mBearing_Radial_FP.Mount.Fixture[i].HolesEquiSpaced;

                                    //....Angle Start.
                                    cmbMount_Fixture_HolesAngStart_Front.Items.Clear();
                                    cmbMount_Fixture_HolesAngStart_Front.Items.Add("30");
                                    cmbMount_Fixture_HolesAngStart_Front.Items.Add("60");
                                    cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart, "");
                                    cmbMount_Fixture_HolesAngStart_Front.ForeColor = Color.Black;

                                    //....Angle other.
                                    Display_Fixture_OtherAngles(mBearing_Radial_FP, i);

                                    //....Thread.                       
                                    //........Type.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type != "")
                                        cmbMount_Fixture_Screw_Type_Front.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type;
                                    else if (cmbMount_Fixture_Screw_Type_Front.Items.Count > 0)
                                        cmbMount_Fixture_Screw_Type_Front.SelectedIndex = 0;

                                    //........Material.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat != "")
                                        cmbMount_Fixture_Screw_Mat_Front.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat;
                                    else if (cmbMount_Fixture_Screw_Mat_Front.Items.Count > 0)
                                        cmbMount_Fixture_Screw_Mat_Front.SelectedIndex = 0;

                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Front,
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type,
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat, 
                                                            mBearing_Radial_FP.Unit.System, 
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig);
                                    //........D_Desig.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig != "")
                                        cmbMount_Fixture_Screw_D_Desig_Front.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig;
                                    else
                                       if(cmbMount_Fixture_Screw_D_Desig_Front.Items.Count>0) cmbMount_Fixture_Screw_D_Desig_Front.SelectedIndex = 0;

                                    //........Length.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L != 0.0F)
                                        cmbMount_Fixture_Screw_L_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L, "#0.000");
                                    else if (cmbMount_Fixture_Screw_L_Front.Items.Count > 0)
                                        cmbMount_Fixture_Screw_L_Front.SelectedIndex = 0;
                                }

                                //txtMount_Holes_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(mBearing.Mount.Holes_Thread_Depth[i], "#0.000");

                                Check_Fixture_Screw_LLims(mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L,
                                                        mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(i),
                                                        mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(i),i);

                                Update_Display_Fixture_Thread_LLims(mBearing_Radial_FP, i);

                            }


                            //....Back 
                            if (chkMount_Holes_Bolting_Back.Checked && i == 1)                        
                            {
                                txtMount_Holes_Thread_Depth_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Holes_Thread_Depth[i], "#0.000");
                                chkMount_Fixture_Candidates_Chosen_Back.Checked = mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[i];

                                Populate_Fixture(mBearing_Radial_FP, i);

                                if (chkMount_Fixture_Candidates_Chosen_Back.Checked)
                                {
                                    //....Part No
                                    cmbMount_Fixture_PartNo_Back.Text = mBearing_Radial_FP.Mount.Fixture[i].PartNo;


                                    //....Part No Indx.
                                    int pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[i].PartNo);

                                    if (pIndx != -1)
                                        Display_Candidate_Fixture_Details(pIndx, mBearing_Radial_FP, i);

                                    else
                                        //lblMsg1.Text = "Please choose an appropriate candidate fixture.";
                                        if (cmbMount_Fixture_PartNo_Back.Items.Count > 0)
                                            cmbMount_Fixture_PartNo_Back.SelectedIndex = 0;

                                    //.....Ang Start.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp_Chosen)
                                        //cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp, "");
                                        cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart_Comp(), "");     //BG 24AUG12
                                    else
                                        cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart, "");
                                    cmbMount_Fixture_HolesAngStart_Back.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    //.....DBC.
                                    IsMount_Fixture_DBC_Calc(ref txtMount_Fixture_DBC_Back);

                                    //....Seal DO.
                                    IsMount_Fixture_DO_Calc(ref txtMount_Fixture_D_Finish_Back);

                                    //....Wall thick
                                    txtMount_Fixture_WallT_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.TWall_BearingCB(i), "#0.000");

                                    //....Count.
                                    cmbMount_Fixture_HolesCount_Back.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesCount);

                                    //....EquiSpaced.
                                    chkMount_Fixture_HolesEquiSpaced_Back.Checked = mBearing_Radial_FP.Mount.Fixture[i].HolesEquiSpaced;

                                    //....Angle Start.
                                    cmbMount_Fixture_HolesAngStart_Back.Items.Clear();
                                    cmbMount_Fixture_HolesAngStart_Back.Items.Add("30");
                                    cmbMount_Fixture_HolesAngStart_Back.Items.Add("60");
                                    cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].HolesAngStart, "");
                                    cmbMount_Fixture_HolesAngStart_Back.ForeColor = Color.Black;

                                    //....Angle other.
                                    Display_Fixture_OtherAngles(mBearing_Radial_FP, i);

                                    //....Thread.                           
                                    //........Type.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type != "")
                                        cmbMount_Fixture_Screw_Type_Back.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type;
                                    else if (cmbMount_Fixture_Screw_Type_Back.Items.Count > 0)
                                        cmbMount_Fixture_Screw_Type_Back.SelectedIndex = 0;

                                    //........Material.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat != "")
                                        cmbMount_Fixture_Screw_Mat_Back.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat;
                                    else if (cmbMount_Fixture_Screw_Mat_Back.Items.Count > 0)
                                        cmbMount_Fixture_Screw_Mat_Back.SelectedIndex = 0;

                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Back,
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Type,
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.Mat,
                                                            mBearing_Radial_FP.Unit.System,
                                                            mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig);
                                    //........D_Desig.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig != null && mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig != "")
                                        cmbMount_Fixture_Screw_D_Desig_Back.Text = mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.D_Desig;
                                    else
                                        cmbMount_Fixture_Screw_D_Desig_Back.SelectedIndex = 0;

                                    //........Length.
                                    if (mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L != 0.0F)
                                        cmbMount_Fixture_Screw_L_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L, "#0.000");
                                    else if (cmbMount_Fixture_Screw_L_Back.Items.Count > 0)
                                        cmbMount_Fixture_Screw_L_Back.SelectedIndex = 0;
                                }

                                Check_Fixture_Screw_LLims(mBearing_Radial_FP.Mount.Fixture[i].Screw_Spec.L,
                                                     mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(i),
                                                     mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(i),i);

                                Update_Display_Fixture_Thread_LLims(mBearing_Radial_FP, i);

                            }

                        }      
               

                    #endregion
                                

                    #region "Temp sensor Holes:"
                    //  -----------------------
                        SetControl_TempSensor();

                        txtTempSensor_CanLength.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.CanLength, modMain.gUnit.MFormat);

                        cmbTempSensor_Loc.Text = mBearing_Radial_FP.TempSensor.Loc.ToString();
                        txtTempSensor_D.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.D, modMain.gUnit.MFormat);
                        txtTempSensor_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.Depth, modMain.gUnit.MFormat);
                            
                        txtTempSensor_AngStart.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.AngStart, "");
                        txtTempSensor_AngBet.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.AngBetween(), "");

                    #endregion


                    #region "EDM:"
                    //------------
                        txtEDM_Pad_RFilletBack.Text =modMain.ConvDoubleToStr( mBearing_Radial_FP.EDM_Pad.RFillet_Back,"#0.000");
                        txtEDM_PAD_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.EDM_Pad.Ang_Offset(modMain.gOpCond), "#0.#");
                        txtEDM_Pad_AngStart_Web.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.EDM_Pad.AngStart_Web, "");

                    #endregion
                }

            #endregion


            #region "CHECK LENGTH LIMITS:"

                private void Check_Fixture_Screw_LLims(Double Thread_L, Double Thread_LLower,Double Thread_LUpper, int Indx_In)
                //=============================================================================================================      
                {
                    Boolean pblnChecked = false;

                    if (Thread_L > Thread_LLower && Thread_L < Thread_LUpper)
                        pblnChecked = true;
                    else
                        pblnChecked = false;

                    switch (Indx_In)
                    {
                        case 0:
                            chkMount_Fixture_Screw_LenLim_Front.Checked = pblnChecked;
                            break;

                        case 1:
                            chkMount_Fixture_Screw_LenLim_Back.Checked = pblnChecked;
                            break;
                    }
                }


                private void Check_SL_Screw_LLim(Double Thread_L, Double Thread_LLower)
                //=====================================================================     //BG 12APR12
                {
                    if (Thread_L != 0.0F)
                    {
                        if (Thread_L >= Thread_LLower)
                            chkSL_Screw_LenLim.Checked = true;
                        else
                            chkSL_Screw_LenLim.Checked = false;
                    }
                    else
                    {
                        chkSL_Screw_LenLim.Checked = true;
                    }
                }


                private void Check_SL_Dowel_LLim(Double Pin_L, Double Pin_LLower)
                //================================================================  //BG 12APR12
                {
                    if (Pin_L != 0.0F)
                    {
                        if (Pin_L > Pin_LLower)
                            chkSL_Dowel_LenLim.Checked = true;
                        else
                            chkSL_Dowel_LenLim.Checked = false;
                    }
                    else
                        chkSL_Dowel_LenLim.Checked = true;
                }

            #endregion

                    
            #region "ROUTINE RELATED CALCULATED FIELD"
            
                private void IsOilInlet_Annulus_D_Calc(ref TextBox TxtBox_In)
                //===========================================================
                {
                    clsBearing_Radial_FP pTempBearing ;
                    
                    pTempBearing =( clsBearing_Radial_FP) mBearing_Radial_FP.Clone();
                    pTempBearing.OilInlet.Annulus_Ratio_L_H =  mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H;
                    pTempBearing.OilInlet.Calc_Annulus_Params();

                    int pRet = 0;
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D > modMain.gcEPS)
                    {
                        if (modMain.CompareVar(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D,
                                               pTempBearing.OilInlet.Annulus.D, 4, pRet) > 0)
                            TxtBox_In.ForeColor = Color.Black;
                        else
                            TxtBox_In.ForeColor = Color.Blue;
                    }
                    else
                        TxtBox_In.ForeColor = Color.Blue;
                }


                private void IsOilInlet_Annulus_L_Calc(ref TextBox TxtBox_In)
                //===========================================================
                {
                    clsBearing_Radial_FP pTempBearing;
                    pTempBearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();

                    pTempBearing.OilInlet.Annulus_Ratio_L_H = mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H;
                    pTempBearing.OilInlet.Calc_Annulus_Params();

                    int pRet = 0;
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.L > modMain.gcEPS)
                    {
                        if (modMain.CompareVar(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.L,
                                              pTempBearing.OilInlet.Annulus.L, 4, pRet) > 0)
                            TxtBox_In.ForeColor = Color.Black;
                        else
                            TxtBox_In.ForeColor = Color.Blue;
                    }
                    else
                        TxtBox_In.ForeColor = Color.Blue;
                }


                private void IsMount_Fixture_DBC_Calc(ref TextBox TxtBox_In)
                //==========================================================
                {
                    int pRet = 0;

                    if (mBearing_Radial_FP.Mount.Fixture[0].DBC == 0.0F ||
                        modMain.CompareVar(mBearing_Radial_FP.Mount.Fixture[0].DBC , 
                                           mBearing_Radial_FP.Mount.Fixture_Candidates.Calc_DBC_Ballpark(),3,pRet)== 0)
                    {
                        TxtBox_In.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture_Candidates.Calc_DBC_Ballpark(), "#0.000");
                        TxtBox_In.ForeColor = Color.Blue;
                    }
                    else if (mBearing_Radial_FP.Mount.Fixture[0].DBC <= mBearing_Radial_FP.Mount.Fixture_Candidates.Calc_DBC_Ballpark())
                    {
                        TxtBox_In.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[0].DBC, "#0.000");
                        TxtBox_In.ForeColor = Color.Black;
                    }
                    else                                 
                    {
                        TxtBox_In.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture_Candidates.Calc_DBC_Ballpark(), "#0.000");
                        TxtBox_In.ForeColor = Color.Blue;
                    }
                }


                private void IsMount_Fixture_DO_Calc(ref TextBox TxtBox_In)
                //=========================================================
                {
                    int pRet = 0;

                    if (mBearing_Radial_FP.Mount.Fixture[0].D_Finish == 0.0F ||
                        modMain.CompareVar(mBearing_Radial_FP.Mount.Fixture[0].D_Finish , 
                                           mBearing_Radial_FP.Mount.Fixture_Candidates.EndConfig_DO_Max(),3,pRet) == 0)
                    {
                        TxtBox_In.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture_Candidates.EndConfig_DO_Max(), "#0.000");     
                        TxtBox_In.ForeColor = Color.Blue;
                    }
                    else
                    {
                        TxtBox_In.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[0].D_Finish, "#0.000");
                        TxtBox_In.ForeColor = Color.Black;
                    }
                }

            #endregion


            #region "UTILITY LOAD ROUTINES"
                //============================

                private void LoadUnit(ComboBox CmbBox_In)
                //=======================================
                {
                    if (CmbBox_In.Items.Count <= 0)
                    {
                        CmbBox_In.Items.Clear();
                        CmbBox_In.Items.Add(clsUnit.eSystem.English.ToString());
                        CmbBox_In.Items.Add(clsUnit.eSystem.Metric.ToString());

                        switch (CmbBox_In.Name)
                        {
                            case "cmbSL_Screw_Spec_UnitSystem":
                                //-----------------------------
                                if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;

                            case "cmbSL_Dowel_Spec_UnitSystem":
                                //-----------------------------
                                if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;

                            case "cmbAntiRotPin_Spec_UnitSystem":
                                //-------------------------------
                                if (mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;
                        }
                    }
                }


                #region "OilInlet:"

                    private void Load_OilInlet_cmbOrificeStartPos()
                    //=============================================
                    {
                        cmbOilInlet_Orifice_StartPos.Items.Clear();
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On);
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above);
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below);

                        if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 0;
                        else if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 1;
                        else if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 2;
                        else
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 0;
                    }


                    private void Load_OilInlet_cmbAnnulus_LH_Ratio()
                    //===============================================     
                    {
                        Double pLH_Ratio_Min = 0, pLH_Ratio_Max = 6.0F;

                        if (mBearing_Radial_FP.Pad.Count == mBearing_Radial_FP.OilInlet.Orifice.Count)
                        {
                            pLH_Ratio_Min = 2.0F;
                        }
                        else
                        {
                            //....# of Orifices = 2 X # of Pads.
                            pLH_Ratio_Min = 3.0F;
                        }

                        cmbOilInlet_Annulus_Ratio_L_H.Items.Clear();

                        while (pLH_Ratio_Min <= pLH_Ratio_Max)
                        {
                            cmbOilInlet_Annulus_Ratio_L_H.Items.Add(pLH_Ratio_Min);
                            pLH_Ratio_Min++;
                        }

                        if (mBearing_Radial_FP.OilInlet.Annulus.D < modMain.gcEPS) 
                            cmbOilInlet_Annulus_Ratio_L_H.SelectedIndex = 0;
                    }


                    private void Load_OilInlet_Orifice_Dist_Holes()
                    //=============================================   
                    {
                        if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                        {
                            txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes, "#0.000");
                        }

                        else
                        {
                            txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(0.0F, "#0.000");
                        }                     
                    }

                #endregion


                #region "SL"

                    private void Load_SL_HardWare()
                    //=============================              
                    {
                        //Split Line Hardware:
                        //--------------------

                        //  Unit
                        //  ----                
                        LoadUnit(cmbSL_Screw_Spec_UnitSystem);
                        LoadUnit(cmbSL_Dowel_Spec_UnitSystem);

                        //  Type
                        //  ----
                        Populate_SL_Details(cmbSL_Screw_Spec_Type);
                        Populate_SL_Details(cmbSL_Dowel_Spec_Type);       
                    }


                    private void Populate_SL_Details(ComboBox CmbBox_In)
                    //==================================================
                    {                     
                        int pIndx = 0,  pRecCount =0;
                        string pTblName = "";
                        String pUnit    = "";
                        String pWHERE   = "";

                        switch (CmbBox_In.Name)
                        {
                            case "cmbSL_Screw_Spec_Type":
                                //-----------------------
                                if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString() != "")
                                    pUnit = mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString().Substring(0, 1);

                                    pWHERE = "WHERE fldUnit = '" + pUnit + "'";
                                    pTblName = "tblManf_Screw";

                                    //.....Type.
                                    pRecCount = modMain.gDB.PopulateCmbBox(CmbBox_In, pTblName, "fldType", pWHERE, true);
                                    if (pRecCount > 0)
                                    {
                                        if (cmbSL_Screw_Spec_Type.Items.Contains(mBearing_Radial_FP.SL.Screw_Spec.Type))
                                        {
                                            pIndx = cmbSL_Screw_Spec_Type.Items.IndexOf(mBearing_Radial_FP.SL.Screw_Spec.Type);
                                            cmbSL_Screw_Spec_Type.SelectedIndex = pIndx;
                                        }
                                        else
                                            cmbSL_Screw_Spec_Type.SelectedIndex = 0;
                                    }

                                break;


                            case "cmbSL_Dowel_Spec_Type":
                                //-----------------------
                                if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString() != "")
                                    pUnit = mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString().Substring(0, 1);

                                pWHERE = "WHERE fldUnit = '" + pUnit + "'";
                                pTblName = "tblManf_Pin";

                                //.....Type.
                                pRecCount = modMain.gDB.PopulateCmbBox(CmbBox_In, pTblName, "fldType", pWHERE, true);
                                if (pRecCount > 0)
                                {
                                    if (cmbSL_Dowel_Spec_Type.Items.Contains(mBearing_Radial_FP.SL.Dowel_Spec.Type))
                                    {
                                        pIndx = cmbSL_Dowel_Spec_Type.Items.IndexOf(mBearing_Radial_FP.SL.Dowel_Spec.Type);
                                        cmbSL_Dowel_Spec_Type.SelectedIndex = pIndx;
                                    }

                                    else
                                        cmbSL_Dowel_Spec_Type.SelectedIndex = 0;
                                }

                                break;
                        }
                    }
               
                #endregion


                #region "AntiRotPin:"

                    private void Load_AntiRotPin()
                    //============================              
                    {
                        Load_AntiRotPin_Loc_CasingSL();
                        Load_AntiRotPin_Loc_BearingVert();
                        Load_AntiRotPin_Loc_BearingSL();

                        LoadUnit(cmbAntiRotPin_Spec_UnitSystem);
                        Populate_AntiRotPin_Spec_Details();                       
                    }


                    private void Populate_AntiRotPin_Spec_Details()
                    //===============================================
                    {
                        String pUnit = "";
                        if (mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString() != "")
                            pUnit = mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString().Substring(0, 1);

                        String pWHERE = "WHERE fldUnit = '" + pUnit + "'";
                        string pTblName = "tblManf_Pin";
                        int pIndx = 0;

                        //.....Type.
                        int pRecCount = modMain.gDB.PopulateCmbBox(cmbAntiRotPin_Spec_Type, pTblName, "fldType", pWHERE, true);
                        if (pRecCount > 0)
                        {
                            if (cmbAntiRotPin_Spec_Type.Items.Contains(mBearing_Radial_FP.AntiRotPin.Spec.Type))
                            {
                                pIndx = cmbAntiRotPin_Spec_Type.Items.IndexOf(mBearing_Radial_FP.AntiRotPin.Spec.Type);
                                cmbAntiRotPin_Spec_Type.SelectedIndex = pIndx;
                            }
                            else
                                cmbAntiRotPin_Spec_Type.SelectedIndex = 0;
                        }
                    }


                    private void Load_AntiRotPin_Loc_CasingSL()
                    //=========================================
                    {
                        cmbAntiRotPin_Loc_CasingSL.Items.Clear();
                        cmbAntiRotPin_Loc_CasingSL.Items.Add(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Center);
                        cmbAntiRotPin_Loc_CasingSL.Items.Add(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset);

                        if (mBearing_Radial_FP.AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Center)
                        {
                            cmbAntiRotPin_Loc_CasingSL.SelectedIndex = 0;    
                        }
                        else if (mBearing_Radial_FP.AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset)
                        {
                            cmbAntiRotPin_Loc_CasingSL.SelectedIndex = 1;
                        }
                        else
                            cmbAntiRotPin_Loc_CasingSL.SelectedIndex = 0;  
                    }


                    private void Load_AntiRotPin_Loc_BearingVert()
                    //============================================       
                    {
                        //.....Load Bearing Vert.
                        Array pArray = Enum.GetValues(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert));

                        cmbAntiRotPin_Loc_Bearing_Vert.Items.Clear();

                        for (int i = 0; i < pArray.Length; i++)
                        {
                            string pName = pArray.GetValue(i).ToString();
                            if (pName.StartsWith("L"))
                            {
                                pName = "Left";
                            }
                            else if(pName.StartsWith("R"))
                            {
                                pName = "Right";
                            }
                            cmbAntiRotPin_Loc_Bearing_Vert.Items.Add(pName);
                        }

                        if (mBearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                        {
                            cmbAntiRotPin_Loc_Bearing_Vert.Text = "Left";
                        }
                        else if (mBearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R)
                        {
                            cmbAntiRotPin_Loc_Bearing_Vert.Text = "Right";
                        }
                        else
                            cmbAntiRotPin_Loc_Bearing_Vert.SelectedIndex = 0;                    
                    }


                    private void Load_AntiRotPin_Loc_BearingSL()
                    //==========================================
                    {
                        cmbAntiRotPin_Loc_Bearing_SL.Items.Clear();
                        cmbAntiRotPin_Loc_Bearing_SL.Items.Add(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top);
                        cmbAntiRotPin_Loc_Bearing_SL.Items.Add(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom);


                        if (mBearing_Radial_FP.AntiRotPin.Loc_Bearing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top)
                        {
                            cmbAntiRotPin_Loc_Bearing_SL.SelectedIndex = 0;
                        }
                        else if (mBearing_Radial_FP.AntiRotPin.Loc_Bearing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom)
                        {
                            cmbAntiRotPin_Loc_Bearing_SL.SelectedIndex = 1;
                        }
                        else
                            cmbAntiRotPin_Loc_Bearing_SL.SelectedIndex = 0;
                    }

                #endregion


                #region "MountHoles:"

                    private void SetMountFixture(clsBearing_Radial_FP Bearing_In)
                    //===========================================================
                    {                     
                        if (Bearing_In.Mount.Holes_GoThru)
                        {
                            if (Bearing_In.Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                            {
                                Bearing_In.Mount.Fixture[1] = (clsBearing_Radial_FP.clsMount.clsFixture)Bearing_In.Mount.Fixture[0].Clone();
                                Bearing_In.Mount.Fixture[1].Screw_Spec = (clsScrew)Bearing_In.Mount.Fixture[0].Screw_Spec.Clone();

                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart_Comp_Chosen)
                                {
                                    Bearing_In.Mount.Fixture[1].HolesAngStart = Bearing_In.Mount.Fixture[0].HolesAngStart_Comp();
                                }
                                else
                                {
                                    //PB 08JUN12 to be developed later. unusual case.
                                }                               
                            }

                            else if (Bearing_In.Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                            {
                                Bearing_In.Mount.Fixture[0] = (clsBearing_Radial_FP.clsMount.clsFixture) Bearing_In.Mount.Fixture[1].Clone();
                                Bearing_In.Mount.Fixture[0].Screw_Spec = (clsScrew)Bearing_In.Mount.Fixture[1].Screw_Spec.Clone();
                       
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart_Comp_Chosen)
                                {
                                    //Bearing_In.Mount.Fixture[0].HolesAngStart = Bearing_In.Mount.Fixture[1].HolesAngStart_Comp;
                                    Bearing_In.Mount.Fixture[0].HolesAngStart = Bearing_In.Mount.Fixture[1].HolesAngStart_Comp();       //BG 24AUG12
                                }
                                else
                                {
                                    //PB 08JUN12 to be developed later. unusual case.
                                }                              
                            }
                        }
                    }


                    private void Load_Mount_Fixture(string MountHolePos_In)
                    //=====================================================       
                    {
                        //Mounting Holes:
                        //---------------

                        //.....Populate ComboBox SplitLine Thread Mat.
                        String pUnit = null;
                       
                        if (modMain.gProject.Unit.System.ToString() != "")
                            pUnit = modMain.gProject.Unit.System.ToString().Substring(0, 1);
                    
                        String pWHERE = "WHERE fldUnit = '" + pUnit + "'";

                        switch (MountHolePos_In)
                        {
                            case "Front":
                            //----------
                                if (!mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0] ||
                                     mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Type == ""
                                    || cmbMount_Fixture_Screw_Type_Front.Items.Count == 0)
                                    {
                                        //.....Type.
                                        int pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Front, "tblManf_Screw", "fldType", pWHERE, true);
                                        int pIndx = cmbMount_Fixture_Screw_Type_Front.Items.IndexOf("SHCS");

                                        //.....Material.
                                        pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Front, "tblManf_Screw", "fldMat", pWHERE, true);
                                        pIndx = cmbSL_Screw_Spec_Mat.Items.IndexOf("STL");
                                        if (pRecCountType > 0) cmbMount_Fixture_Screw_Mat_Front.SelectedIndex = pIndx;
                                    }

                                break;


                            case "Back":
                            //----------
                               if (!mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1] ||
                                     mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Type == ""
                                    || cmbMount_Fixture_Screw_Type_Back.Items.Count == 0)
                                {
                                    //..... Type.
                                    int pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Back, "tblManf_Screw", "fldType", pWHERE, true);
                                    int pIndx = cmbMount_Fixture_Screw_Type_Back.Items.IndexOf("SHCS");

                                    //.....Material.
                                    pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Back, "tblManf_Screw", "fldMat", pWHERE, true);
                                    pIndx = cmbSL_Screw_Spec_Mat.Items.IndexOf("STL");
                                    if (pRecCountType > 0) cmbMount_Fixture_Screw_Mat_Back.SelectedIndex = pIndx;
                                }

                            break;
                        }
                    }

                #endregion


                #region "TempSensor:"

                    private void Load_TempSensor_Count()
                    //=================================
                    {
                        cmbTempSensor_Count.Items.Clear();
                        if (mBearing_Radial_FP.Pad.Count != 0)
                        {
                            for (int i = 0; i < mBearing_Radial_FP.Pad.Count; i++)
                            {
                                cmbTempSensor_Count.Items.Add(i + 1);  
                            }

                            if (modMain.gProject.Product.Accessories.TempSensor.Count > 0)
                                cmbTempSensor_Count.Text = modMain.gProject.Product.Accessories.TempSensor.Count.ToString();
                            else if (mBearing_Radial_FP.TempSensor.Count > 0)
                                cmbTempSensor_Count.Text = mBearing_Radial_FP.TempSensor.Count.ToString();
                            else
                                cmbTempSensor_Count.SelectedIndex = 0;
                        }
                    }


                    private void Load_TempSensor_Loc()
                    //===============================
                    {                       
                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                            modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        {
                            cmbTempSensor_Loc.Items.Clear();
                            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eFaceID.Front);
                            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eFaceID.Back);
                        }

                        else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        {
                            cmbTempSensor_Loc.Items.Clear();
                            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eFaceID.Front);
                        }

                        else if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                        {
                            cmbTempSensor_Loc.Items.Clear();
                            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eFaceID.Back);
                        }
                   

                        if(cmbTempSensor_Loc.Items.Count == 2 )
                        {
                            if (mBearing_Radial_FP.TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                cmbTempSensor_Loc.SelectedIndex = 0;

                            else if (mBearing_Radial_FP.TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                cmbTempSensor_Loc.SelectedIndex = 1;

                            else
                                cmbTempSensor_Loc.SelectedIndex = 0;
                        }

                        else if (cmbTempSensor_Loc.Items.Count == 1)
                            cmbTempSensor_Loc.SelectedIndex = 0;   
                    }

                #endregion

            #endregion

        #endregion


        #region "FORM CONTROLS EVENT ROUTINES:"
        //************************************     

            #region "CHECKBOX RELATED ROUTINES:"
            //---------------------------------   

                private void ChkBox_CheckedChanged(object sender, EventArgs e)  
                //============================================================
                {
                    int pIndx = 0;
                    CheckBox pChkBox = (CheckBox)sender;

                    TabPage [] pTabPageMountingHoles = new TabPage[]{ tabFront, tabBack};
                   
                    switch (pChkBox.Name)
                    {
                        // Oil Inlet.
                        // ----------
                        case "chkOilInlet_Annulus_Exists":
                            mBearing_Radial_FP.OilInlet.Annulus_Exists = chkOilInlet_Annulus_Exists.Checked;
                            grpOilInlet_Annulus.Visible = chkOilInlet_Annulus_Exists.Checked;
                            break;


                        //  Web Relief.
                        //  ----------
                        case "chkMillRelief_Exists":
                            //....Update Related Web Mill Relief.                          
                            mBearing_Radial_FP.MillRelief.Exists = chkMillRelief_Exists.Checked;
                            SetControl_Web_MillRelief();
                            Populate_MillRelief_D_Desig();

                            //....Update related D_PadRelief.
                            txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.D_PadRelief(), "#0.000");           
                            break;


                        //  Split Line Hardware.
                        //  -------------------
                        case "chkSL_Screw_LenLim":
                            Populate_SL_Screw_L();
                            break;

                        case "chkSL_Dowel_LenLim":
                            Populate_SL_Dowel_L();
                            break;


                        // Flange.
                        // -------
                        case "chkFlange_Exists":
                            SetControls_Flange();
                            break;


                        // Mount Hole.        
                        // ----------
                        case "chkMount_Holes_GoThru":
                             mBearing_Radial_FP.Mount.Holes_GoThru = chkMount_Holes_GoThru.Checked;
                            SetControls_Mount_Holes_GoThru(mBearing_Radial_FP.Mount.Holes_GoThru);

                            break;

                        case "chkMount_Holes_Bolting_Front":
                            //-------------------------------
                            if (chkMount_Holes_GoThru.Checked && chkMount_Holes_Bolting_Front.Checked)
                            {
                                mBearing_Radial_FP.Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Front;
                                SetControls_Mount_Holes_GoThru(mBearing_Radial_FP.Mount.Holes_GoThru);
                                chkMount_Holes_Bolting_Back.Checked = false;

                                Cursor = Cursors.WaitCursor;
                                Populate_Fixture(mBearing_Radial_FP, 0);
                                //....Get Part# index From Candiate Fixture.
                                pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[0].PartNo);
                                if(pIndx >= 0)
                                    cmbMount_Fixture_PartNo_Front.SelectedIndex = pIndx;
                                Cursor = Cursors.Default;
                            }
                            else if (!chkMount_Holes_GoThru.Checked && chkMount_Holes_Bolting_Front.Checked && 
                                                                        chkMount_Holes_Bolting_Back.Checked)
                            {
                                mBearing_Radial_FP.Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Both;
                                Cursor = Cursors.WaitCursor;
                                Populate_Fixture(mBearing_Radial_FP, 0);
                                //....Get Part# index From Candiate Fixture.
                                pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[0].PartNo);
                                if (pIndx >= 0)
                                    cmbMount_Fixture_PartNo_Front.SelectedIndex = pIndx;
                                Cursor = Cursors.Default;
                            }
                            break;


                        case "chkMount_Holes_Bolting_Back":
                            //------------------------------
                            if (chkMount_Holes_GoThru.Checked && chkMount_Holes_Bolting_Back.Checked)
                            {
                                mBearing_Radial_FP.Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Back;
                                SetControls_Mount_Holes_GoThru(mBearing_Radial_FP.Mount.Holes_GoThru);
                                chkMount_Holes_Bolting_Front.Checked = false;

                                Cursor = Cursors.WaitCursor;
                                Populate_Fixture(mBearing_Radial_FP, 1);
                                //....Get Part# index From Candiate Fixture.
                                pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[1].PartNo);
                                if (pIndx >= 0)
                                    cmbMount_Fixture_PartNo_Back.SelectedIndex = pIndx;
                                Cursor = Cursors.Default;
                            }

                            else if (!chkMount_Holes_GoThru.Checked && chkMount_Holes_Bolting_Front.Checked &&
                                                                       chkMount_Holes_Bolting_Back.Checked)
                            {
                                mBearing_Radial_FP.Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Both;
                                Cursor = Cursors.WaitCursor;
                                Populate_Fixture(mBearing_Radial_FP, 1);
                                //....Get Part# index From Candiate Fixture.
                                pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[1].PartNo);
                                if (pIndx >= 0)
                                {
                                    cmbMount_Fixture_PartNo_Back.SelectedIndex = pIndx;                                    
                                }
                                Cursor = Cursors.Default;

                            }
                             
                            break;


                        case "chkMount_Fixture_Candidates_Chosen_Front":
                            //-----------------------------------------               
                            mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0] = pChkBox.Checked;                                                       
                            //Upadate_Fixture_Selection(mBearing, true);
                            
                            Cursor = Cursors.WaitCursor;
                            Populate_Fixture(mBearing_Radial_FP, 0);
                            //....Get Part# index From Candiate Fixture.
                            pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[0].PartNo);
                            if (pIndx >= 0)
                                cmbMount_Fixture_PartNo_Front.SelectedIndex = pIndx;
                            Cursor = Cursors.Default;                              
                            break;


                        case "chkMount_Fixture_Candidates_Chosen_Back":
                            //----------------------------------------               
                            mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1] = pChkBox.Checked;
                            //Upadate_Fixture_Selection(mBearing, true);
                            
                            Cursor = Cursors.WaitCursor;
                            Populate_Fixture(mBearing_Radial_FP, 1);
                            //....Get Part# index From Candiate Fixture.
                            pIndx = Get_Fixture_Indx(mBearing_Radial_FP, mBearing_Radial_FP.Mount.Fixture[1].PartNo);
                            if (pIndx >= 0)
                            {
                                cmbMount_Fixture_PartNo_Back.SelectedIndex = pIndx;
                            }
                            Cursor = Cursors.Default;                              
                            break;


                        case "chkMount_Fixture_HolesEquiSpaced_Front":
                            //-------------------------------------
                            mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced = chkMount_Fixture_HolesEquiSpaced_Front.Checked;
                            Display_Fixture_OtherAngles(mBearing_Radial_FP, 0);              
                            break;


                        case "chkMount_Fixture_HolesEquiSpaced_Back":
                            //-------------------------------------
                            mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced = chkMount_Fixture_HolesEquiSpaced_Back.Checked;
                            Display_Fixture_OtherAngles(mBearing_Radial_FP, 1);
                            break;

                        case "chkMount_Fixture_Screw_LenLim_Front":
                            //----------------------------------
                            Populate_Mount_Fixture_Thread_L(0);
                            break;

                        case "chkMount_Fixture_Screw_LenLim_Back":
                            //----------------------------------
                            Populate_Mount_Fixture_Thread_L(1);
                            break;


                        //  Temp Sensor Holes
                        //  -----------------
                        case "chkTempSensor_Exists":
                            mBearing_Radial_FP.TempSensor.Exists = chkTempSensor_Exists.Checked;
                            SetControl_TempSensor();
                            SetColor_TempSensor_Depth();

                            if (!mBearing_Radial_FP.TempSensor.Exists && !modMain.gProject.Product.Accessories.TempSensor.Supplied)
                            {                               
                                modMain.gProject.Product.Accessories.WireClip_Supplied = mBearing_Radial_FP.TempSensor.Exists;
                            }                         
                            break;
                    }
                }

            #endregion


            #region"COMMAND BUTTON EVENT ROUTINE:"
            //------------------------------------  

                private void cmdPrint_Click(object sender, EventArgs e)     //BG 20OCT11
                //======================================================
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }

                private void cmdAccessories_Click(object sender, EventArgs e)
                //============================================================
                {
                    modMain.gfrmAccessories.TempSensor_Count = mBearing_Radial_FP.Pad.Count;
                    modMain.gfrmAccessories.ShowDialog();                           
                }

     
                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    CloseForm();  
                }

                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {                    
                    this.Hide();
                    
                }             


                private void CloseForm()
                //======================
                {                 
                    if (mBearing_Radial_FP.OilInlet.Annulus.Exists)
                    {
                        //....Validate OilInlet Annulus Dia.
                        if (!ValidateOilInlet_Annulus_D())
                            return;

                        //....Validate OilInlet Annulus Length.         //BG 09APR13
                        if (!ValidateOilInlet_Annulus_L())
                            return;
                    }

                    ////....Validate OilInlet Annulus Length.
                    //if (!ValidateOilInlet_Annulus_L())
                    //    return;

                    //....Validate Anti Rotation Pin Angle.
                    if (!ValidateAng_AntiRotPin())              
                        return;
                   
                    SaveData();

                    this.Hide();

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        modMain.gfrmSealDesignDetails.ShowDialog();
                    }
                    else  if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                    }
                }


                private void SaveData()
                //=====================
                {

                    //  Length
                    //  -------
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L = modMain.ConvTextToDouble(txtL.Text);

                    //  Depth EndConfigs.
                    //  ---------------
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndConfig[0] = 
                                                                modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);
                    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndConfig[1] = 
                                                                modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);

                    #region "Roughing:"

                        //  Start Dim.
                        //  ----------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DimStart_FrontFace = 
                                                                        modMain.ConvTextToDouble(txtRoughing_DimStart_FrontFace.Text);
                    
                    #endregion
                    

                    #region "Oil Inlet:"
                    
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Count_MainOilSupply = 
                                                                        modMain.ConvTextToInt(cmbOilInlet_Count_MainOilSupply.Text);


                        if (cmbOilInlet_Orifice_StartPos.Text != "")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_StartPos =
                                                                        (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)
                                                                        Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos), 
                                                                        cmbOilInlet_Orifice_StartPos.Text);


                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Loc_FrontFace=
                                                                        modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_FrontFace.Text);


                        //  Annulus
                        //  -------

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Exists =
                                                                            chkOilInlet_Annulus_Exists.Checked;

                        if (chkOilInlet_Annulus_Exists.Checked)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Ratio_L_H =
                                                                        modMain.ConvTextToDouble(cmbOilInlet_Annulus_Ratio_L_H.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_D =
                                                                        modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_L =
                                                                        modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Loc_Back =
                                                                        modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text);
                        }

                        if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Dist_Holes =
                                                                        modMain.ConvTextToDouble(txtOilInlet_Orifice_Dist_Holes.Text);

                    #endregion


                    #region  "WebRelief"
                    //-----------------
                    
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.Exists = chkMillRelief_Exists.Checked;
                        if (mBearing_Radial_FP.MillRelief.Exists)
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.D_Desig = cmbMillRelief_D_Desig.Text;   
                        else
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.D_Desig = "";

                        #region "EDM Relief:"
                        //  ----------------       //BG 06DEC12
                        for (int i = 0; i < 2; i++)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.EDM_Relief[i] = modMain.ConvTextToDouble(mtxtEDM_Relief[i].Text);
                        }

                        #endregion

                    
                    #endregion

                    
                    #region "SplitLine HardWare"
                    //-------------------------
                        //.....Thread.
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.Unit.System = 
                                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbSL_Screw_Spec_UnitSystem.Text);            //BG 26MAR12

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.Type = cmbSL_Screw_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.D_Desig = cmbSL_Screw_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbSL_Screw_Spec_Pitch.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.L = modMain.ConvTextToDouble(cmbSL_Screw_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw_Spec.Mat = cmbSL_Screw_Spec_Mat.Text;

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Loc_Center =
                                                                        modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Loc_Front =
                                                                        modMain.ConvTextToDouble(txtSL_LScrew_Loc_Front.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Loc_Center =
                                                                        modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Loc_Front =
                                                                        modMain.ConvTextToDouble(txtSL_RScrew_Loc_Front.Text);


                        //.....Dowel.
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.Unit.System =
                                                           (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbSL_Dowel_Spec_UnitSystem.Text);             //BG 26MAR12

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.Type = cmbSL_Dowel_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.D_Desig = cmbSL_Dowel_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.L = modMain.ConvTextToDouble(cmbSL_Dowel_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.Mat = cmbSL_Dowel_Spec_Mat.Text;

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Center =
                                                                        modMain.ConvTextToDouble(txtSL_LDowel_Loc_Center.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Front =
                                                                        modMain.ConvTextToDouble(txtSL_LDowel_Loc_Front.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Center =
                                                                        modMain.ConvTextToDouble(txtSL_RDowel_Loc_Center.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Front =
                                                                        modMain.ConvTextToDouble(txtSL_RDowel_Loc_Front.Text);


                    #endregion


                    #region  "Flange"
                    //--------------

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.Exists = chkFlange_Exists.Checked;

                        if(chkFlange_Exists.Checked)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.D =  modMain.ConvTextToDouble(txtFlange_D.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.Wid =  modMain.ConvTextToDouble(txtFlange_Wid.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.DimStart_Front=  modMain.ConvTextToDouble(txtFlange_DimStart_Front.Text);
                        }
                    #endregion


                    #region  "Anti Rotation Pin"
                    //------------------------- 

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Dist_Front = modMain.ConvTextToDouble(txtAntiRotPin_Loc_Dist_Front.Text);
                        
                        if (cmbAntiRotPin_Loc_CasingSL.Text != "")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Casing_SL = 
                                                            (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL)
                                                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL), cmbAntiRotPin_Loc_CasingSL.Text);

                        if(mBearing_Radial_FP.AntiRotPin.Loc_Casing_SL== clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset)
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Offset = 
                                                            modMain.ConvTextToDouble(txtAntiRotPin_Loc_Offset.Text);       


                        if (cmbAntiRotPin_Loc_Bearing_Vert.Text != "")
                        {
                            string pName = cmbAntiRotPin_Loc_Bearing_Vert.Text;

                            if (pName.StartsWith("L"))
                            {
                                pName = clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L.ToString();
                            }
                            else if (pName.StartsWith("R"))
                            {
                                pName = clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R.ToString();
                            }

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Bearing_Vert = (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert)
                                Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert), pName);
                        }

                        if (cmbAntiRotPin_Loc_Bearing_SL.Text != "")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Bearing_SL =
                                                            (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL)
                                                            Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL), cmbAntiRotPin_Loc_Bearing_SL.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Loc_Angle = modMain.ConvTextToDouble(txtAntiRotPin_Loc_Angle.Text);

                        //BG 24APR12
                        if(mBearing_Radial_FP.Flange.Exists)
                        {
                            string pInsert ="";
                            if(optDFit.Checked)
                                pInsert = clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn.FD.ToString();
                            else
                                pInsert = clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn.F.ToString();
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.InsertedOn = (clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn)
                                                                                                          Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn), pInsert);
                        }
                       
            
                        if(cmbAntiRotPin_Spec_UnitSystem.Text!="")
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Spec.Unit.System =
                                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbAntiRotPin_Spec_UnitSystem.Text);          //BG 26MAR12

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Spec.Type = cmbAntiRotPin_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Spec.D_Desig = cmbAntiRotPin_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Spec.L = modMain.ConvTextToDouble(cmbAntiRotPin_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Spec.Mat = cmbAntiRotPin_Spec_Mat.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Depth = modMain.ConvTextToDouble(txtAntiRotPin_Depth.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.Stickout = modMain.ConvTextToDouble(txtAntiRotPin_Stickout.Text);

                    #endregion


                    #region "Mounting:"
                    //  --------------

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_GoThru = chkMount_Holes_GoThru.Checked;                    

                        if (chkMount_Holes_GoThru.Checked)
                        {
                            if (chkMount_Holes_Bolting_Front.Checked)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Front;

                            else if (chkMount_Holes_Bolting_Back.Checked)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Back;
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting = clsBearing_Radial_FP.eFaceID.Both;
                        }
                    

                        //....Front Bolting:
                        //
                        if (chkMount_Holes_Bolting_Front.Checked)
                        {
                            if (!chkMount_Holes_GoThru.Checked)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[0] = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Front.Text);                            

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates_Chosen[0] = chkMount_Fixture_Candidates_Chosen_Front.Checked;

                            if (chkMount_Fixture_Candidates_Chosen_Front.Checked)
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates.Retrieve();
                                if(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates.Exists)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Front_PartNo = cmbMount_Fixture_PartNo_Front.Text; 
                               
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesAngStart_Comp_Chosen = mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart_Comp_Chosen;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig = txtMount_Fixture_Screw_D_Desig_Front.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Pitch = modMain.ConvTextToDouble(txtMount_Fixture_Screw_Pitch_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesAngStart = modMain.ConvTextToDouble(cmbMount_Fixture_HolesAngStart_Front.Text);
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].PartNo = "";
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].DBC = modMain.ConvTextToDouble(txtMount_Fixture_DBC_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].D_Finish = modMain.ConvTextToDouble(txtMount_Fixture_D_Finish_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced = chkMount_Fixture_HolesEquiSpaced_Front.Checked;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesAngStart = modMain.ConvTextToDouble(cmbMount_Fixture_HolesAngStart_Front.Text);

                                Double[] pMount_Fixture_HolesAngOther = new Double[mBearing_Radial_FP.COUNT_MOUNT_HOLES_ANG_OTHER_MAX];//SG 21AUG12

                                if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                    for (int i = 0; i < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesCount - 1; i++)
                                        pMount_Fixture_HolesAngOther[i] = modMain.ConvTextToDouble(mTxtMount_Fixture_HolesAngOther_Front[i].Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesAngOther = pMount_Fixture_HolesAngOther;

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig = cmbMount_Fixture_Screw_D_Desig_Front.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_Pitch_Front.Text);
                            }

                            //....Thread
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type = cmbMount_Fixture_Screw_Type_Front.Text;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.L = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_L_Front.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat = cmbMount_Fixture_Screw_Mat_Front.Text;
                        }


                        //....Back Bolting:
                        //
                        if (chkMount_Holes_Bolting_Back.Checked)
                        {
                            if (!chkMount_Holes_GoThru.Checked)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[1] = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Back.Text);
                           
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates_Chosen[1] = chkMount_Fixture_Candidates_Chosen_Back.Checked;

                            if (chkMount_Fixture_Candidates_Chosen_Back.Checked)
                            {                                
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates.Retrieve();
                                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Candidates.Exists)
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture_Back_PartNo = cmbMount_Fixture_PartNo_Back.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesAngStart_Comp_Chosen = mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart_Comp_Chosen;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig = txtMount_Fixture_Screw_D_Desig_Back.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Pitch = modMain.ConvTextToDouble(txtMount_Fixture_Screw_Pitch_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesAngStart = modMain.ConvTextToDouble(cmbMount_Fixture_HolesAngStart_Back.Text);
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].PartNo = "";
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].DBC = modMain.ConvTextToDouble(txtMount_Fixture_DBC_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].D_Finish = modMain.ConvTextToDouble(txtMount_Fixture_D_Finish_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced = chkMount_Fixture_HolesEquiSpaced_Back.Checked;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesAngStart = modMain.ConvTextToDouble(cmbMount_Fixture_HolesAngStart_Back.Text);

                                Double[] pMount_Fixture_HolesAngOther = new Double[mBearing_Radial_FP.COUNT_MOUNT_HOLES_ANG_OTHER_MAX];//SG 21AUG12

                                if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                    for (int i = 0; i < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[0].HolesCount - 1; i++)
                                        pMount_Fixture_HolesAngOther[i] = modMain.ConvTextToDouble(mTxtMount_Fixture_HolesAngOther_Front[i].Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].HolesAngOther = pMount_Fixture_HolesAngOther;

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Desig = cmbMount_Fixture_Screw_D_Desig_Back.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Pitch = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_Pitch_Back.Text);
                            }

                            //....Thread
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Type = cmbMount_Fixture_Screw_Type_Back.Text;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.L = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_L_Back.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Fixture[1].Screw_Spec.Mat = cmbMount_Fixture_Screw_Mat_Back.Text;
                        }

                        if (chkMount_Holes_GoThru.Checked)
                        {
                            SetMountFixture((clsBearing_Radial_FP)modMain.gProject.Product.Bearing);
                        }                        

                    #endregion


                    #region "Temp Sensor Holes"
                    //-------------------------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Exists = chkTempSensor_Exists.Checked;                       
                    
                        if (chkTempSensor_Exists.Checked)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.CanLength = modMain.ConvTextToDouble(txtTempSensor_CanLength.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Count = modMain.ConvTextToInt(cmbTempSensor_Count.Text);  

                            if (cmbTempSensor_Loc.Text != "")
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Loc = (clsBearing_Radial_FP.eFaceID)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID), cmbTempSensor_Loc.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D = modMain.ConvTextToDouble(txtTempSensor_D.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Depth = modMain.ConvTextToDouble(txtTempSensor_Depth.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.AngStart = modMain.ConvTextToDouble(txtTempSensor_AngStart.Text);
                        }

                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Count = 0;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.D = 0.0F;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.Depth = 0.0F;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TempSensor.AngStart = 0.0F;
                        }

                    #endregion


                    #region  EDM Pad
                    //  -----------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).EDM_Pad.RFillet_Back = modMain.ConvTextToDouble(txtEDM_Pad_RFilletBack.Text);
                    #endregion
                }

            #endregion


            #region "COMBOBOX RELATED ROUTINES:"
            //----------------------------------  

                private void ComboBox_MouseDown(object sender, MouseEventArgs e)
                //===============================================================
                {
                     ComboBox pCmbBox = (ComboBox)sender;

                     switch (pCmbBox.Name)
                     {
                         case "cmbAntiRotPin_Loc_Bearing_Vert":
                             //--------------------------------
                             mblnAntiRotPin_Loc_Bearing_Vert_ManuallyChanged = true;
                             break;

                         case "cmbAntiRotPin_Spec_D_Desig":
                             //----------------------------
                             mblnAntiRotPin_Stickout_Changed_ManuallyChanged = false;        
                             mblnAntiRotPin_Spec_D_Desig_ManuallyChanged = true;
                             break;

                         case "cmbOilInlet_Annulus_Ratio_L_H":
                             //-------------------------------                             
                             mblnOilInlet_Annulus_L_ManuallyChanged = false;
                             mblnOilInlet_Annulus_D_ManuallyChanged = false;
                             break;
                     }
                }
             

                private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)  
                //====================================================================
                {
                    ComboBox pCmbBox = (ComboBox)sender;

                    switch(pCmbBox.Name)
                    {
                        #region "OilInlet:"
                        //-----------------

                            case "cmbOilInlet_Count_MainOilSupply":
                            //-------------------------------------
                                mBearing_Radial_FP.OilInlet.Count_MainOilSupply = modMain.ConvTextToInt(pCmbBox.Text);
                                if(mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H> modMain.gcEPS)
                                    Update_OilInlet_Annulus_Display();             
                                break;

                            case "cmbOilInlet_Annulus_Ratio_L_H":
                            //----------------------------------- 
                                mBearing_Radial_FP.OilInlet.Annulus_Ratio_L_H = modMain.ConvTextToDouble(pCmbBox.Text);

                           
                                if(!mblnOilInlet_Annulus_L_ManuallyChanged)
                                    Update_OilInlet_Annulus_Display();             
                                break;

                            case "cmbOilInlet_Orifice_StartPos":
                            //---------------------------------
                                mBearing_Radial_FP.OilInlet.Orifice_StartPos = (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos), pCmbBox.Text);
                                txtOilInlet_Orifice_AngStart_BDV.Text = mBearing_Radial_FP.OilInlet.Orifice.AngStart_BDV.ToString("#0.0");
                                break;

                        #endregion


                        #region  "Web MillRelief:"
                        //------------------------

                            case "cmbMillRelief_D":
                            //-------------------------
                                mBearing_Radial_FP.MillRelief.D_Desig = pCmbBox.Text;
                                txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.D_PadRelief(), "#0.000");
                                break;

                        #endregion


                        #region "Split Line Hardware:"
                        //----------------------------

                            case "cmbSL_Screw_Spec_UnitSystem":
                                //----------------------------
                                if (pCmbBox.Text != "")
                                {
                                    mBearing_Radial_FP.SL.Screw_Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);       //BG 26MAR12                          
                                    Populate_SL_Details(cmbSL_Screw_Spec_Type);
                                }
                                break;

                            case "cmbSL_Screw_Spec_Type":
                            //-------------------------------
                                mBearing_Radial_FP.SL.Screw_Spec.Type = pCmbBox.Text;
                                Populate_Screw_Mat(ref cmbSL_Screw_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.SL.Screw_Spec.Unit.System);
                                break;


                            case "cmbSL_Screw_Spec_D_Desig":
                            //-------------------------------                                                       
                                ArrayList Pitch_Array, PitchType_Array;
                                mBearing_Radial_FP.SL.Screw_Spec.D_Desig = pCmbBox.Text;

                          
                                mBearing_Radial_FP.SL.Screw_Spec.GetPitch(pCmbBox.Text, out Pitch_Array, out PitchType_Array); 
                                
                                if (Pitch_Array.Count > 0)
                                {
                                    cmbSL_Screw_Spec_Pitch.Items.Clear();
                                    for (int i = 0; i < Pitch_Array.Count; i++)
                                        cmbSL_Screw_Spec_Pitch.Items.Add(Pitch_Array[i]);
                                }

                                
                                if (cmbSL_Screw_Spec_Pitch.Items.Count > 0)
                                {
                                    if (mBearing_Radial_FP.SL.Screw_Spec.Pitch > modMain.gcEPS && 
                                            cmbSL_Screw_Spec_Pitch.Items.Contains(mBearing_Radial_FP.SL.Screw_Spec.Pitch.ToString("#0.000")))
                                        cmbSL_Screw_Spec_Pitch.Text = mBearing_Radial_FP.SL.Screw_Spec.Pitch.ToString("#0.000");
                                    else
                                        cmbSL_Screw_Spec_Pitch.SelectedIndex = 0;

                                }

                                Update_SL_Screw_L();

                                //BG 02JUL13
                                txtSL_LScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center(), "#0.000");
                                txtSL_RScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center(), "#0.000");

                                txtSL_Thread_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_Depth, "#0.000");
                                break;

                            case "cmbSL_Screw_Spec_Pitch":
                            //----------------------------
                                mBearing_Radial_FP.SL.Screw_Spec.Pitch = modMain.ConvTextToDouble(pCmbBox.Text); 
                                break;

                            case "cmbSL_Screw_Spec_L":  
                                //--------------------      
                                mBearing_Radial_FP.SL.Screw_Spec.L = modMain.ConvTextToDouble(pCmbBox.Text);
                                txtSL_CBore_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.CBore_Depth, "#0.000");
                                break;

                            case "cmbSL_Screw_Spec_Mat":
                            //--------------------------
                                mBearing_Radial_FP.SL.Screw_Spec.Mat = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbSL_Screw_Spec_D_Desig, mBearing_Radial_FP.SL.Screw_Spec.Type,
                                                                                     cmbSL_Screw_Spec_Mat.Text, 
                                                                                     mBearing_Radial_FP.SL.Screw_Spec.Unit.System,
                                                                                     mBearing_Radial_FP.SL.Screw_Spec.D_Desig); 
                                break;

                            case "cmbSL_Dowel_Spec_UnitSystem":
                                //-----------------------------
                                if (pCmbBox.Text != "")
                                {                               
                                    mBearing_Radial_FP.SL.Dowel_Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);
                                    Populate_SL_Details(cmbSL_Dowel_Spec_Type);
                                }
                                break;

                            case "cmbSL_Dowel_Spec_Type":
                            //---------------------------------
                                mBearing_Radial_FP.SL.Dowel_Spec.Type = pCmbBox.Text;
                                Populate_Pin_Mat(ref cmbSL_Dowel_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.SL.Dowel_Spec.Unit.System);
                                break;

                            case "cmbSL_Dowel_Spec_D_Desig":
                            //------------------------------
                                mBearing_Radial_FP.SL.Dowel_Spec.D_Desig = pCmbBox.Text;
                                Update_SL_Dowel_L();                            
                                break;

                            case "cmbSL_Dowel_Spec_L":
                                //------------------------      
                                mBearing_Radial_FP.SL.Dowel_Spec.L = modMain.ConvTextToDouble(pCmbBox.Text);
                                txtSL_Dowel_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Dowel_Depth, "#0.000");
                                break;

                            case "cmbSL_Dowel_Spec_Mat":
                            //--------------------------------
                                mBearing_Radial_FP.SL.Dowel_Spec.Mat = pCmbBox.Text;
                                Populate_Pin_D_Desig(ref cmbSL_Dowel_Spec_D_Desig, cmbSL_Dowel_Spec_Type.Text,
                                                     cmbSL_Dowel_Spec_Mat.Text, mBearing_Radial_FP.SL.Dowel_Spec.Unit.System,mBearing_Radial_FP.SL.Dowel_Spec.D_Desig );         //BG 26MAR12
                                break;
                        
                        #endregion


                        #region "Anti Rotation Pin:"
                        //--------------------------

                            case "cmbAntiRotPin_Spec_Type":
                            //-------------------------
                                mBearing_Radial_FP.AntiRotPin.Spec.Type = pCmbBox.Text;
                                Populate_Pin_Mat(ref cmbAntiRotPin_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.AntiRotPin.Spec.Unit.System);
                            break;

                            case "cmbAntiRotPin_Spec_D_Desig":
                            //---------------------------------
                                string pPrevVal = mBearing_Radial_FP.AntiRotPin.Spec.D_Desig;

                                mBearing_Radial_FP.AntiRotPin.Spec.D_Desig = pCmbBox.Text;                                                           
                               
                                if (cmbAntiRotPin_Spec_Type.Text != ""
                                    && cmbAntiRotPin_Spec_D_Desig.Text != "")
                                    Populate_AntiRotPin_L();

                          
                                if (mBearing_Radial_FP.AntiRotPin.Spec.D_Desig != pPrevVal)
                                {
                                    mblnAntiRotPin_Depth_Changed_ManuallyChanged = true;
                                    mBearing_Radial_FP.AntiRotPin.Depth=0.0;
                                    txtAntiRotPin_Loc_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Spec.D, "#0.000"); //BG 04OCT12
                                    txtAntiRotPin_Depth.Text = mBearing_Radial_FP.AntiRotPin.Depth.ToString("#0.000");
                                }


                                if (mblnAntiRotPin_Spec_D_Desig_ManuallyChanged)
                                {
                                    txtSL_LDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Center(), "#0.000");         //BG 27NOV12
                                    txtSL_RDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Center(), "#0.000");         //BG 27NOV12
                                    mblnAntiRotPin_Spec_D_Desig_ManuallyChanged = false;
                                }
                            break;

                            case "cmbAntiRotPin_Spec_L":
                            //--------------------------
                                Double pPrevVal_AntiRotPin_Spec_L = mBearing_Radial_FP.AntiRotPin.Spec.L; 
                                mBearing_Radial_FP.AntiRotPin.Spec.L = modMain.ConvTextToDouble(pCmbBox.Text);

                                if (pPrevVal_AntiRotPin_Spec_L != mBearing_Radial_FP.AntiRotPin.Spec.L)      //BG 04OCT12
                                    txtAntiRotPin_Stickout.Text = mBearing_Radial_FP.AntiRotPin.Calc_Stickout().ToString("#0.000");                                                                      
                            break;

                            case "cmbAntiRotPin_Spec_Mat":
                            //----------------------------
                                mBearing_Radial_FP.AntiRotPin.Spec.Mat = pCmbBox.Text;
                                Populate_Pin_D_Desig(ref cmbAntiRotPin_Spec_D_Desig, cmbAntiRotPin_Spec_Type.Text,
                                                            cmbAntiRotPin_Spec_Mat.Text, mBearing_Radial_FP.AntiRotPin.Spec.Unit.System, mBearing_Radial_FP.AntiRotPin.Spec.D_Desig);         //BG 26MAR12
                            break;

                            case "cmbAntiRotPin_Spec_UnitSystem":
                            //-----------------------------------
                            if (pCmbBox.Text != "")
                            {
                                mBearing_Radial_FP.AntiRotPin.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);
                                Populate_AntiRotPin_Spec_Details();
                            }
                            break;

                            case "cmbAntiRotPin_Loc_CasingSL":
                            //-------------------------------
                                if (cmbAntiRotPin_Loc_CasingSL.Text == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset.ToString())
                                {
                                    txtAntiRotPin_Loc_Offset.Visible = true;
                                    txtAntiRotPin_Loc_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.AntiRotPin.Loc_Offset, "#0.000");
                                    if (Math.Abs(Math.Round(mBearing_Radial_FP.AntiRotPin.Loc_Offset,3) - Math.Round(mBearing_Radial_FP.AntiRotPin.Spec.D,3))< modMain.gcEPS)
                                        txtAntiRotPin_Loc_Offset.ForeColor = Color.Magenta;
                                    else
                                        txtAntiRotPin_Loc_Offset.ForeColor = Color.Black;
                                }
                                else
                                    txtAntiRotPin_Loc_Offset.Visible = false;

                                    mBearing_Radial_FP.AntiRotPin.Loc_Casing_SL =
                                        (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL)
                                        Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL), pCmbBox.Text);
                            break;

                            case "cmbAntiRotPin_Loc_Bearing_Vert":
                            //----------------------------------
                                string pName = pCmbBox.Text;
                                if (pName.StartsWith("L"))
                                {
                                    pName = clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L.ToString();
                                }
                                else if (pName.StartsWith("R"))
                                {
                                    pName = clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R.ToString();
                                }
                                mBearing_Radial_FP.AntiRotPin.Loc_Bearing_Vert =
                                    (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert), pName);

                                if (mblnAntiRotPin_Loc_Bearing_Vert_ManuallyChanged)
                                {
                                    txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                    txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                    txtSL_LDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Center(), "#0.000");         //BG 27NOV12
                                    txtSL_RDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Center(), "#0.000");         //BG 27NOV12

                                    txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 26NOV12
                                    txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 26NOV12
                                    mblnAntiRotPin_Loc_Bearing_Vert_ManuallyChanged = false;
                                }
                            break;

                            case "cmbAntiRotPin_Loc_Bearing_SL":
                            //----------------------------------
                                mBearing_Radial_FP.AntiRotPin.Loc_Bearing_SL =
                                    (clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL), pCmbBox.Text);
                            break;

                        #endregion


                        #region  "Mount Fixture:"
                        //-----------------------

                            case "cmbMount_Fixture_PartNo_Front":
                            //-----------------------------------  
                               String pPrev_Mount_Fixture_PartNo_Front = mBearing_Radial_FP.Mount.Fixture[0].PartNo;
                               int pIndx = Get_Fixture_Indx(mBearing_Radial_FP,pCmbBox.Text);
                               mBearing_Radial_FP.Mount.Fixture_Front_PartNo = pCmbBox.Text;

                               if (pIndx != -1)
                               {
                                   Display_Candidate_Fixture_Details(pIndx, mBearing_Radial_FP,0);
                                   mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig = txtMount_Fixture_Screw_D_Desig_Front.Text;
                                   lblMsg1.Text = "";

                                   Boolean pDisp = false;                                            
                                   if (mBearing_Radial_FP.Mount.Fixture_Candidates.HolesEquiSpaced[pIndx])
                                       pDisp=true;                               
                                   else                               
                                       pDisp = false;
                               
                                   lblMsg_MountFixture_EquiSpaced_Front.Visible = pDisp;

                                   if (pPrev_Mount_Fixture_PartNo_Front != mBearing_Radial_FP.Mount.Fixture[0].PartNo)          //BG 04OCT12
                                   {
                                       txtMount_Holes_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(2 * mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D, "#0.000");
                                       txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Magenta;
                                   }
                                   else
                                   {
                                       txtMount_Holes_Thread_Depth_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Holes_Thread_Depth[0], "#0.000");
                                       txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Black;
                                   }
                               }
                               break;


                            case "cmbMount_Fixture_PartNo_Back":
                               //-------------------------------
                               String pPrev_Mount_Fixture_PartNo_Back = mBearing_Radial_FP.Mount.Fixture[1].PartNo;
                               pIndx = Get_Fixture_Indx(mBearing_Radial_FP, pCmbBox.Text);
                               mBearing_Radial_FP.Mount.Fixture_Back_PartNo = pCmbBox.Text;

                               if (pIndx != -1)
                               {
                                   Display_Candidate_Fixture_Details(pIndx, mBearing_Radial_FP, 1);
                                   mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig = txtMount_Fixture_Screw_D_Desig_Back.Text;
                                   lblMsg1.Text = "";

                                   Boolean pDisp = false;
                                   if (mBearing_Radial_FP.Mount.Fixture_Candidates.HolesEquiSpaced[pIndx])
                                       pDisp = true;
                                   else
                                       pDisp = false;

                                   lblMsg_MountFixture_EquiSpaced_Back.Visible = pDisp;

                                   if (pPrev_Mount_Fixture_PartNo_Back != mBearing_Radial_FP.Mount.Fixture[1].PartNo)       //BG 04OCT12
                                   {
                                       txtMount_Holes_Thread_Depth_Back.Text = modMain.ConvDoubleToStr(2 * mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D, "#0.000");
                                       txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Magenta;
                                   }
                                   else
                                   {
                                       txtMount_Holes_Thread_Depth_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Holes_Thread_Depth[1], "#0.000");
                                       txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Black;
                                   }
                               }
                               break;


                            case "cmbMount_Fixture_HolesCount_Front":
                            //---------------------------------------
                               int pCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Front.Text);
                               mBearing_Radial_FP.Mount.Fixture[0].HolesCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Front.Text);

                               //....Angle other.
                               Display_Fixture_OtherAngles(mBearing_Radial_FP, 0);                          
                               break;


                            case "cmbMount_Fixture_HolesCount_Back":
                            //--------------------------------------
                                pCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Back.Text); 
                                mBearing_Radial_FP.Mount.Fixture[1].HolesCount = modMain.ConvTextToInt(cmbMount_Fixture_HolesCount_Back.Text);
                                    //....Angle other.
                                Display_Fixture_OtherAngles(mBearing_Radial_FP, 1); 
                                break;


                            case "cmbMount_Fixture_HolesAngStart_Front":
                            //------------------------------------------
                                String pMountFixture_Sel_HolesAngStart = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart,"");
                                String pMountFixture_Sel_HolesAngStart_Comp = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart_Comp(), "");    //BG 24AUG12

                                if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                    if (pMountFixture_Sel_HolesAngStart != null)
                                    {
                                        if (pMountFixture_Sel_HolesAngStart == pCmbBox.Text)
                                        {
                                            mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart_Comp_Chosen = false;

                                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                            {
                                                if(cmbMount_Fixture_HolesAngStart_Back.Items.IndexOf(pMountFixture_Sel_HolesAngStart_Comp) !=-1 &&
                                                    cmbMount_Fixture_PartNo_Front.Text == cmbMount_Fixture_PartNo_Back.Text)
                                                    cmbMount_Fixture_HolesAngStart_Back.SelectedIndex = cmbMount_Fixture_HolesAngStart_Back.Items.IndexOf(pMountFixture_Sel_HolesAngStart_Comp);
                                            }
                                        }

                                        else if (pMountFixture_Sel_HolesAngStart_Comp == pCmbBox.Text)
                                        {
                                            mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart_Comp_Chosen = true;
                                            
                                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                            {
                                                if(cmbMount_Fixture_HolesAngStart_Back.Items.IndexOf(pMountFixture_Sel_HolesAngStart)!= -1 &&
                                                   cmbMount_Fixture_PartNo_Front.Text == cmbMount_Fixture_PartNo_Back.Text)
                                                cmbMount_Fixture_HolesAngStart_Back.SelectedIndex = cmbMount_Fixture_HolesAngStart_Back.Items.IndexOf(pMountFixture_Sel_HolesAngStart);
                                            }
                                        }

                                        Display_Fixture_OtherAngles(mBearing_Radial_FP,0);  
                                    }
                                }
                                else
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngStart = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;


                            case "cmbMount_Fixture_HolesAngStart_Back":
                                //-------------------------------------
                                pMountFixture_Sel_HolesAngStart = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart, "");
                                pMountFixture_Sel_HolesAngStart_Comp = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart_Comp(), "");  //BG 24AUG12

                                if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    if (pMountFixture_Sel_HolesAngStart != null)
                                    {
                                        if (pMountFixture_Sel_HolesAngStart == pCmbBox.Text)
                                        {
                                            mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart_Comp_Chosen = false;
                                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                            {
                                                if(cmbMount_Fixture_HolesAngStart_Front.Items.IndexOf(pMountFixture_Sel_HolesAngStart_Comp)!=-1 &&
                                                     cmbMount_Fixture_PartNo_Front.Text == cmbMount_Fixture_PartNo_Back.Text)
                                                cmbMount_Fixture_HolesAngStart_Front.SelectedIndex =cmbMount_Fixture_HolesAngStart_Front.Items.IndexOf(pMountFixture_Sel_HolesAngStart_Comp);
                                            }
                                        }
                                        else if (pMountFixture_Sel_HolesAngStart_Comp == pCmbBox.Text)
                                        {
                                            mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart_Comp_Chosen = true;
                                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                            {
                                                if(cmbMount_Fixture_HolesAngStart_Front.Items.IndexOf(pMountFixture_Sel_HolesAngStart_Comp)!= -1 &&
                                                     cmbMount_Fixture_PartNo_Front.Text == cmbMount_Fixture_PartNo_Back.Text)
                                                cmbMount_Fixture_HolesAngStart_Front.SelectedIndex =cmbMount_Fixture_HolesAngStart_Front.Items.IndexOf(pMountFixture_Sel_HolesAngStart);
                                            }
                                        }

                                        Display_Fixture_OtherAngles(mBearing_Radial_FP, 1);
                                    }
                                }
                                else
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngStart = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;


                            //  Screw:
                            //  ------
                            case "cmbMount_Fixture_Screw_Type_Front":
                            //---------------------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Type = pCmbBox.Text;

                                if (!chkMount_Fixture_Candidates_Chosen_Front.Checked)
                                {
                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Front, mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Type,
                                                                                                         cmbMount_Fixture_Screw_Mat_Front.Text,
                                                                                                         mBearing_Radial_FP.Unit.System,
                                                                                                         mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig);
                                    Populate_Screw_Mat(ref cmbMount_Fixture_Screw_Mat_Front, pCmbBox.Text, mBearing_Radial_FP.Unit.System);
                                }
                                else
                                {
                                    Populate_Fixture_Thread_Mat(ref cmbMount_Fixture_Screw_Mat_Front, pCmbBox.Text,
                                                                txtMount_Fixture_Screw_D_Desig_Front.Text); 
                                }                            
                                break;


                            case "cmbMount_Fixture_Screw_Type_Back":
                                //-----------------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Type = pCmbBox.Text;

                                if (!chkMount_Fixture_Candidates_Chosen_Back.Checked)
                                {
                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Back, mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Type,
                                                            cmbMount_Fixture_Screw_Mat_Back.Text, mBearing_Radial_FP.Unit.System, 
                                                            mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig);
                                    Populate_Screw_Mat(ref cmbMount_Fixture_Screw_Mat_Back, pCmbBox.Text, mBearing_Radial_FP.Unit.System);

                                }
                                else
                                {
                                    Populate_Fixture_Thread_Mat(ref cmbMount_Fixture_Screw_Mat_Back, pCmbBox.Text,
                                                                txtMount_Fixture_Screw_D_Desig_Back.Text);
                                }
                                break;


                            case "cmbMount_Fixture_Screw_D_Desig_Front":
                            //------------------------------------------
                                //....Populate Pitch Array & Pitch Type.                                   
                                mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.GetPitch(pCmbBox.Text, out Pitch_Array, out PitchType_Array); 
                                cmbMount_Fixture_Screw_Pitch_Front.DataSource = Pitch_Array;
                                mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig = pCmbBox.Text;
                                Update_Fixture_Thread_L(0);
                                break;


                            case "cmbMount_Fixture_Screw_D_Desig_Back":
                                //-------------------------------------
                                //....Populate Pitch Array & Pitch Type.                                   
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.GetPitch(pCmbBox.Text, out Pitch_Array, out PitchType_Array); 
                                cmbMount_Fixture_Screw_Pitch_Back.DataSource = Pitch_Array;
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig = pCmbBox.Text;
                                Update_Fixture_Thread_L(1);
                                break;
                            

                            case "cmbMount_Fixture_Screw_L_Front":
                            //------------------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.L = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;


                            case "cmbMount_Fixture_Screw_L_Back":
                                //-------------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.L = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;


                            case "cmbMount_Fixture_Screw_Mat_Front":
                            //--------------------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Mat = pCmbBox.Text;
                                if (!chkMount_Fixture_Candidates_Chosen_Front.Checked)
                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Front, 
                                                            mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Type,
                                                            cmbMount_Fixture_Screw_Mat_Front.Text,
                                                            mBearing_Radial_FP.Unit.System,
                                                            mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig);   
                                else
                                    Populate_Mount_Fixture_Thread_L(0);
                                break;

                            case "cmbMount_Fixture_Screw_Mat_Back":
                                //---------------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Mat = pCmbBox.Text;
                                if (!chkMount_Fixture_Candidates_Chosen_Back.Checked)
                                    Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Back,
                                                            mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Type,
                                                            cmbMount_Fixture_Screw_Mat_Back.Text,
                                                            mBearing_Radial_FP.Unit.System,
                                                            mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig);
                                else
                                    Populate_Mount_Fixture_Thread_L(1);
                                break;

                        #endregion


                        #region Temp Sensor Holes:
                        //------------------------

                            case "cmbTempSensor_Count":
                            //------------------------------
                                mBearing_Radial_FP.TempSensor.Count = modMain.ConvTextToInt(cmbTempSensor_Count.Text); 
                                break;

                            case "cmbTempSensor_Loc":
                            //-----------------------
                                mBearing_Radial_FP.TempSensor.Loc =
                                    (clsBearing_Radial_FP.eFaceID)Enum.Parse(typeof(clsBearing_Radial_FP.eFaceID), pCmbBox.Text);
                                break;

                        #endregion
                    }
                }

        
                private void cmbOilInlet_Annulus_Ratio_L_H_DrawItem(object sender, DrawItemEventArgs e)
                //=======================================================================================
                {
                    //ComboBox pCmbBox = (ComboBox)sender;
                    //e.DrawBackground();
                    //Brush pBrush = Brushes.Black;
                    //string pVal = cmbOilInlet_Annulus_Ratio_L_H.Items[e.Index].ToString();

                    //if (!mblnDraw_Oil_Inllet_LH_Ratio)
                    //{
                    //    if (pVal != cmbOilInlet_Annulus_Ratio_L_H.Text)
                    //    {
                    //        pBrush = Brushes.Blue;
                    //        mblnDraw_Oil_Inllet_LH_Ratio = true;
                    //    }
                    //}

                    //e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                    //                      e.Font, pBrush, e.Bounds, StringFormat.GenericDefault);

                    //e.DrawFocusRectangle();
                }


                private void cmbBox_MouseHover(object sender, EventArgs e)
                //=========================================================
                {
                    ComboBox pcmbBox = (ComboBox)sender;

                    switch (pcmbBox.Name)
                    {
                        case "cmbSL_Screw_Spec_Mat":
                            //---------------------
                            toolTip1.SetToolTip(cmbSL_Screw_Spec_Mat, cmbSL_Screw_Spec_Mat.Text);
                            break;

                        case "cmbSL_Dowel_Spec_Mat":
                            //---------------------
                            toolTip1.SetToolTip(cmbSL_Dowel_Spec_Mat, cmbSL_Dowel_Spec_Mat.Text);
                            break;

                        case "cmbAntiRotPin_Spec_Mat":
                            //-----------------------
                            toolTip1.SetToolTip(cmbAntiRotPin_Spec_Mat, cmbAntiRotPin_Spec_Mat.Text);
                            break;

                        case "cmbMount_Fixture_Screw_Mat_Front":
                            //----------------------------------
                            toolTip1.SetToolTip(cmbMount_Fixture_Screw_Mat_Front, cmbMount_Fixture_Screw_Mat_Front.Text);
                            break;

                        case "cmbMount_Fixture_Screw_Mat_Back":
                            //----------------------------------
                            toolTip1.SetToolTip(cmbMount_Fixture_Screw_Mat_Front, cmbMount_Fixture_Screw_Mat_Front.Text);
                            break;
                    }
                }
                

                private void cmbMountFixture_PartNo_DrawItem(object sender, DrawItemEventArgs e)
                //==============================================================================     
                {
                    if (e.Index < 0) return;
                    
                    ComboBox pCmbBox = (ComboBox)sender;
                    e.DrawBackground();
                    Brush pBrush = Brushes.Black;
                                        
                    if (mBearing_Radial_FP.Mount.Fixture_Candidates.HolesEquiSpaced[e.Index])
                        pBrush = Brushes.OrangeRed;

                    e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                        e.Font, pBrush, e.Bounds, StringFormat.GenericDefault);
                   
                    e.DrawFocusRectangle();
                }

            #endregion


            #region "TEXTBOX RELATED ROUTINE:"
            //--------------------------------
              

                private void TextBox_KeyDown(object sender, KeyEventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    
                    if (!pTxtBox.ReadOnly)
                        pTxtBox.ForeColor = Color.Black;

                    switch (pTxtBox.Name)
                    {
                        case "txtL":
                            //------
                             mblnL_ManuallyChanged = true;
                             break;

                        case "txtDepth_EndConfig_Front":
                            //--------------------------
                            mblnDepth_EndConfig_F_ManuallyChanged = true;

                            pTxtBox.ForeColor = Color.Black;
                            txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                            break;

                        case "txtDepth_EndConfig_Back":
                            //-------------------------
                            mblnDepth_EndConfig_B_ManuallyChanged = true;

                            txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                            pTxtBox.ForeColor = Color.Black;
                            break;

                        case "txtRoughing_DimStart_FrontFace":
                            //---------------------------------
                            mblnRoughing_DimStart_FrontFace_ManuallyChanged = true;
                            break;

                        case "txtOilInlet_Annulus_D":
                            //-----------------------
                            mblnOilInlet_Annulus_D_ManuallyChanged = true;
                            break;

                        case "txtOilInlet_Annulus_L":
                            //-----------------------
                            mblnOilInlet_Annulus_L_ManuallyChanged = true;
                            break;

                        case "txtOilInlet_Annulus_Loc_Back":
                            //------------------------------
                            mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = true;
                            break;

                        case "txtEDMRelief_Front":
                            //---------------------
                            mblnEDM_Relief_Front_ManuallyChanged = true;
                            break;

                        case "txtEDMRelief_Back":
                            //-------------------
                            mblnEDM_Relief_Back_ManuallyChanged = true;
                            break;

                        case "txtAntiRotPin_Loc_Dist_Front":
                            //------------------------------
                            mblnAntiRotPin_Loc_Dist_Front_ManuallyChanged = true;
                            break;

                        case "txtAntiRotPin_Depth":
                            //---------------------
                            mblnAntiRotPin_Depth_Changed_ManuallyChanged = true;
                            break;

                        case "txtAntiRotPin_Stickout":
                            //------------------------
                            mblnAntiRotPin_Stickout_Changed_ManuallyChanged = true;
                            break;


                        case "txtMount_Holes_Thread_Depth_Front":
                        case "txtMount_Holes_Thread_Depth_Back":
                            //------------------------------------
                            mblnMount_Holes_Thread_Depth_ManuallyChanged = true;
                            break;

                        case "txtTempSensor_CanLength":
                            //-------------------------
                            mblnTempSensor_CanLength_ManuallyChanged = true;
                            break;

                    }
                }
               

                private void TextBox_TextChanged(object sender, EventArgs e)       
                //==========================================================            
                {
                    TextBox pTxtBox = (TextBox)sender;
                    Double pVal = 0.0;

                    switch(pTxtBox.Name)
                    {
                        case "txtL":                                //  Bearing Length:
                        //----------
                            mBearing_Radial_FP.L = modMain.ConvTextToDouble(txtL.Text);

                            if (mblnL_ManuallyChanged)
                            {
                                //....The following special actions to taken when L is manually changed.
                                //

                                //....End-Configs Depth:
                                //
                                    double pDepth = mBearing_Radial_FP.Calc_Depth_EndConfig();      //....Symmetrical Depths.

                                    //....FRONT:
                                    //
                                    txtDepth_EndConfig_Front.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                                    mblnDepth_EndConfig_F_ManuallyChanged = false;
                                   
                                    mBearing_Radial_FP.Depth_EndConfig[0] = pDepth;  

                                
                                    //....BACK:
                                    //
                                    txtDepth_EndConfig_Back.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                                    mblnDepth_EndConfig_B_ManuallyChanged = false;

                                    mBearing_Radial_FP.Depth_EndConfig[1] = pDepth;  
                               

                                //....Oil Inlet:
                                //
                                    //....Orifice.
                                    double pLoc = mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace();
                                    txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(pLoc, "#0.000");


                                    //....Annulus.
                                    txtOilInlet_Annulus_Loc_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Calc_Annulus_Loc_Back(), "#0.000");     //BG 05OCT12
                                    txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Blue;

                                    mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = false;          //  PB17JAN13.


                                //....S/L Hardware:          //  PB17JAN13. We may have to include the boolean for manually changing the following text boxes.
                                //
                                    txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                    txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                    txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                    txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12


                                //  Reset the state. 
                                //  ---------------
                                mblnL_ManuallyChanged = false;           
                            }

        
                            //PB 10AUG11. To be written later.
                            //if (mBearing.L <= mcBearingL_Limit)
                            //{
                            //    mBearing.SealMountHoles_GoThru = true;
                            //}
                            //else
                            //{
                            //    mBearing.SealMountHoles_GoThru = false;
                            //}

                            break;


                        // Depths:
                        // -------

                        //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                        //case "txtDepth_EndConfig_Front":
                        ////------------------------------
                            //Double pPreVal = mBearing_Radial_FP.Depth_EndConfig[0];                     

                            ////if (mblnL_ManuallyChanged)    //PB 17JAN13
                            ////{
                            ////    mBearing_Radial_FP.Depth_EndConfig[0] = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);  
                            ////}

                            //if (mblnDepth_EndConfig_F_ManuallyChanged && txtDepth_EndConfig_Front.Text != "")
                            //{                        
                            //    if (!mblnDepth_EndConfig_B_ManuallyChanged)
                            //    {
                            //        //....Retrieve from Text Box.
                            //        double pDepthF = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);

                            //        //....Validate & reset to Min Value, if necessary.
                            //        if (pDepthF < mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                            //        {
                            //            pDepthF = mBearing_Radial_FP.Validate_Depth_EndConfig(pDepthF);
                            //            txtDepth_EndConfig_Front.Text      = pDepthF.ToString("#0.000");             //....Redisplay.
                            //            txtDepth_EndConfig_Front.ForeColor = Color.Magenta;
                            //        }

                            //        //....Assign. 
                            //        mBearing_Radial_FP.Depth_EndConfig[0] = pDepthF;

                            //        //....Update the Depth Back.
                            //        Update_Depth_EndConfig(txtDepth_EndConfig_Front, txtDepth_EndConfig_Back);
                            //    }


                            //    double pLoc = mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace();
                            //    txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(pLoc, "#0.000");

                            //    txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                            //    txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                            //    txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                            //    txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12


                            //    //  Reset the state. 
                            //    //  ---------------
                            //    mblnDepth_EndConfig_F_ManuallyChanged = false;
                            // }


                            //if (Math.Abs(pPreVal - mBearing_Radial_FP.Depth_EndConfig[0]) > modMain.gcEPS)
                            //{
                            //    txtOilInlet_Orifice_Loc_FrontFace.ForeColor = Color.Blue;
                            //}

                            //break;

                        //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                        //case "txtDepth_EndConfig_Back":
                        //-----------------------------
                            //if (mblnL_ManuallyChanged)    //PB 17JAN13
                            //{
                            //    mBearing_Radial_FP.Depth_EndConfig[1] = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                            //}

                            //if (mblnDepth_EndConfig_B_ManuallyChanged && txtDepth_EndConfig_Back.Text != "")
                            //{ 
                            //    if (!mblnDepth_EndConfig_F_ManuallyChanged)
                            //     {
                            //         //....Retrieve from Text Box.
                            //        double pDepthB = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);                        

                            //        //....Validate & reset to Min Value, if necessary.
                            //        if (pDepthB < mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                            //        {
                            //            pDepthB = mBearing_Radial_FP.Validate_Depth_EndConfig(pDepthB);
                            //            txtDepth_EndConfig_Back.Text = pDepthB.ToString("#0.000");          //....Redisplay.
                            //            txtDepth_EndConfig_Back.ForeColor = Color.Magenta;
                            //        }

                            //        //....Assign. 
                            //        mBearing_Radial_FP.Depth_EndConfig[1] = pDepthB;

                            //         //....Update the Depth Front.
                            //        Update_Depth_EndConfig(txtDepth_EndConfig_Back, txtDepth_EndConfig_Front);
                            //     }


                            //     txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                            //     txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                            //     txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                            //     txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12


                            //     //  Reset the state. 
                            //     //  ---------------
                            //     mblnDepth_EndConfig_B_ManuallyChanged = false;
                            // }

                            //break;


                        #region "Roughing:"
                        //------------------

                            case "txtRoughing_DimStart_FrontFace":
                            //------------------------------------
                            mBearing_Radial_FP.DimStart_FrontFace = modMain.ConvTextToDouble(txtRoughing_DimStart_FrontFace.Text);

                            if (mblnRoughing_DimStart_FrontFace_ManuallyChanged)
                            {                                
                                txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                mblnRoughing_DimStart_FrontFace_ManuallyChanged = false;
                            }

                            break;

                        #endregion


                        #region "OilInlet:"
                        //------------------

                            case "txtOilInlet_Orifice_Loc_FrontFace":
                            //--------------------------------------
                             mBearing_Radial_FP.OilInlet.Orifice_Loc_FrontFace = modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_FrontFace.Text);
                             SetColor_Orifice_Loc_FrontFace();
                             break;

                            case "txtOilInlet_Annulus_D":
                            //---------------------------
                                mBearing_Radial_FP.OilInlet.Annulus_D = modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text);
                                txtOilInlet_Annulus_V.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus_V(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), 
                                                                                     modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text)), "#0.000");

                                if (mblnOilInlet_Annulus_D_ManuallyChanged)
                                {
                                    txtOilInlet_Annulus_L.Text = mBearing_Radial_FP.OilInlet.Calc_Annulus_L(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text)).ToString("#0.000");
                                    txtOilInlet_Annulus_L.ForeColor = Color.Blue;                               
                                }             
                                break;

                            case "txtOilInlet_Annulus_L":
                            //---------------------------
                                mBearing_Radial_FP.OilInlet.Annulus_L = modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text);
                                txtOilInlet_Annulus_V.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus_V(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), 
                                                                                     modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text)), "#0.000");

                                if (mblnOilInlet_Annulus_L_ManuallyChanged)
                                {
                                    Double pRatio_L_H = mBearing_Radial_FP.OilInlet.Calc_Annulus_Ratio_L_H
                                                       (modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text));     
                                    if (!cmbOilInlet_Annulus_Ratio_L_H.Items.Contains(pRatio_L_H.ToString("#0.00")))
                                    {
                                         cmbOilInlet_Annulus_Ratio_L_H.Text = pRatio_L_H.ToString("#0.00");
                                    }

                                    txtOilInlet_Annulus_Loc_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Calc_Annulus_Loc_Back(), "#0.000");
                                    txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Blue;
                                    mblnOilInlet_Annulus_D_ManuallyChanged = false;
                                }
                                    
                                break;

                        case "txtOilInlet_Annulus_Loc_Back":
                            //------------------------------
                            mBearing_Radial_FP.OilInlet.Annulus_Loc_Back = modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text);     //SG 05OCT12
                            SetColor_Annulus_Loc_Back();

                            if (mblnOilInlet_Annulus_Loc_Back_ManuallyChanged)
                            {
                                txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = false;
                            }
                            break;

                        #endregion


                        #region "Web Relief:"
                        //------------------

                            case "txtEDMRelief_Front":
                                //====================
                                mBearing_Radial_FP.MillRelief.EDM_Relief[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                                

                                if (Math.Abs(mBearing_Radial_FP.MillRelief.EDM_Relief[0] - mBearing_Radial_FP.MillRelief.DESIGN_EDM_RELIEF) < modMain.gcEPS)
                                {
                                    pTxtBox.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }

                               
                                if (mblnEDM_Relief_Front_ManuallyChanged)
                                {
                                    double pDepth = mBearing_Radial_FP.Calc_Depth_EndConfig();      //....Symmetrical Depths.

                                    txtDepth_EndConfig_Front.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;

                                    txtDepth_EndConfig_Back.Text  = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;

                                    //Update_Depth_EndConfig(txtDepth_EndConfig_Front, txtDepth_EndConfig_Back);

                                    txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace(), "#0.000#");
                                    mblnEDM_Relief_Front_ManuallyChanged = false;
                                }

                                break;


                            case "txtEDMRelief_Back":
                                //===================
                                mBearing_Radial_FP.MillRelief.EDM_Relief[1] = modMain.ConvTextToDouble(pTxtBox.Text);

                                if (Math.Abs(mBearing_Radial_FP.MillRelief.EDM_Relief[1] - mBearing_Radial_FP.MillRelief.DESIGN_EDM_RELIEF) < modMain.gcEPS)
                                {
                                    pTxtBox.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }

                                if (mblnEDM_Relief_Back_ManuallyChanged)
                                {
                                    double pDepth = mBearing_Radial_FP.Calc_Depth_EndConfig();      //....Symmetrical Depths.

                                    txtDepth_EndConfig_Front.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;

                                    txtDepth_EndConfig_Back.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;

                                    //Update_Depth_EndConfig(txtDepth_EndConfig_Back, txtDepth_EndConfig_Front);

                                    txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace(), "#0.000#");
                                    mblnEDM_Relief_Back_ManuallyChanged = false;
                                }

                                break;

                        #endregion


                        #region "Anti Rotation Pin:"
                        //------------------------

                            case "txtAntiRotPin_Loc_Angle":
                                //-------------------------
                                mBearing_Radial_FP.AntiRotPin.Loc_Angle = modMain.ConvTextToDouble(txtAntiRotPin_Loc_Angle.Text);

                                break;

                            case "txtAntiRotPin_Loc_Dist_Front":
                                //------------------------------
                                mBearing_Radial_FP.AntiRotPin.Loc_Dist_Front = modMain.ConvTextToDouble(txtAntiRotPin_Loc_Dist_Front.Text);

                                if (mblnAntiRotPin_Loc_Dist_Front_ManuallyChanged)
                                {
                                    txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                    txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                    txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 26NOV12
                                    txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 26NOV12
                                    mblnAntiRotPin_Loc_Dist_Front_ManuallyChanged = false;
                                }

                                break;

                            case "txtAntiRotPin_Loc_Offset":
                                //--------------------------                            
                                mBearing_Radial_FP.AntiRotPin.Loc_Offset = modMain.ConvTextToDouble(txtAntiRotPin_Loc_Offset.Text);

                                if (Math.Abs(Math.Round(mBearing_Radial_FP.AntiRotPin.Loc_Offset, 3) - 
                                    Math.Round(mBearing_Radial_FP.AntiRotPin.Spec.D, 3)) < modMain.gcEPS)
                                    txtAntiRotPin_Loc_Offset.ForeColor = Color.Magenta;
                                else
                                    txtAntiRotPin_Loc_Offset.ForeColor = Color.Black;

                                break;

                            case "txtAntiRotPin_Depth":
                                //---------------------
                                mBearing_Radial_FP.AntiRotPin.Depth = modMain.ConvTextToDouble(txtAntiRotPin_Depth.Text);

                                if (Math.Abs(Math.Round(mBearing_Radial_FP.AntiRotPin.Depth, 3) - 
                                    Math.Round(mBearing_Radial_FP.AntiRotPin.Spec.D, 3)) < modMain.gcEPS)
                                {
                                    txtAntiRotPin_Depth.ForeColor = Color.Magenta;
                                }

                                if (mblnAntiRotPin_Depth_Changed_ManuallyChanged)
                                {
                                    txtAntiRotPin_Stickout.Text = mBearing_Radial_FP.AntiRotPin.Stickout.ToString("#0.000");
                                    txtAntiRotPin_Stickout.ForeColor = Color.Blue;
                                    mblnAntiRotPin_Depth_Changed_ManuallyChanged = false;
                                }

                                break;


                            case "txtAntiRotPin_Stickout":
                                //------------------------
                                mBearing_Radial_FP.AntiRotPin.Stickout = modMain.ConvTextToDouble(txtAntiRotPin_Stickout.Text);

                                if (mblnAntiRotPin_Stickout_Changed_ManuallyChanged)
                                {
                                    txtAntiRotPin_Depth.ForeColor = Color.Blue;
                                    txtAntiRotPin_Depth.Text = mBearing_Radial_FP.AntiRotPin.Depth.ToString("#0.000");
                                    mblnAntiRotPin_Stickout_Changed_ManuallyChanged = false;
                                }

                                break;

                        #endregion


                        #region "Mount Fixture:"
                        //----------------------  


                            //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                            //case "txtMount_Holes_Thread_Depth_Front":
                                //----------------------------------
                                //if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                //{                                    
                                //    pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Front.Text);                                   

                                //    if (mblnMount_Holes_Thread_Depth_ManuallyChanged)
                                //    {
                                //        mblnMount_Holes_Thread_Depth_ManuallyChanged = false;
                                //        //txtMount_Holes_Thread_Depth_Front.Text = mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth
                                //        //                                         (0, mBearing_Radial_FP.Mount.Holes_Thread_Depth[0]).ToString("#0.00#"); 
                                //        txtMount_Holes_Thread_Depth_Front.Text = 
                                //            mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth(0, pVal).ToString("#0.00#");
                                //        pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Front.Text); 

                                //    }

                                //    mBearing_Radial_FP.Mount.Holes_Thread_Depth[0] = pVal;

                                //    if (mBearing_Radial_FP.Mount.Holes_Thread_Depth[0] == 2 * mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D)
                                //    {
                                //        txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Magenta;
                                //    }
                                //    else
                                //    {
                                //        txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Black;
                                //    }
                                   
                                //}
                                //break;

                            //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                            //case "txtMount_Holes_Thread_Depth_Back":
                                //----------------------------------
                                   //if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                                   //{
                                   //    pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Back.Text);

                                   //    if (mblnMount_Holes_Thread_Depth_ManuallyChanged)
                                   //    {
                                   //        mblnMount_Holes_Thread_Depth_ManuallyChanged = false;
                                   //        //txtMount_Holes_Thread_Depth_Back.Text = mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth
                                   //        //                                   (1, mBearing_Radial_FP.Mount.Holes_Thread_Depth[1]).ToString("#0.00#");
                                   //        txtMount_Holes_Thread_Depth_Back.Text =
                                   //            mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth(1, pVal).ToString("#0.00#");
                                   //        pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Back.Text);
                                   //    }

                                   //    mBearing_Radial_FP.Mount.Holes_Thread_Depth[1] = pVal;
                                      
                                   //    if (mBearing_Radial_FP.Mount.Holes_Thread_Depth[1] == 2 * mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D)
                                   //    {
                                   //        txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Magenta;
                                   //    }
                                   //    else
                                   //    {
                                   //        txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Black;
                                   //    }
                                   //}
                                   //break;
                            
                            case "txtMount_Fixture_WallT_Back":
                                //---------------------------
                                Double pAny = modMain.ConvTextToDouble(txtMount_Fixture_WallT_Back.Text); //To be reviewed SG 02MAR12
                                break;
                           
                            case "txtMount_Fixture_HolesCount_Front":
                                //------------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].HolesCount =
                                    modMain.ConvTextToInt(txtMount_Fixture_HolesCount_Front.Text);
                                break;

                            case "txtMount_Fixture_HolesCount_Back":
                                //------------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].HolesCount = 
                                    modMain.ConvTextToInt(txtMount_Fixture_HolesCount_Back.Text);
                                                        
                                break;

                            case "txtMount_Fixture_DBC_Front":
                                //-------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].DBC =
                                    modMain.ConvTextToDouble(txtMount_Fixture_DBC_Front.Text);
                                break;


                            case "txtMount_Fixture_DBC_Back":
                                //-------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].DBC = 
                                    modMain.ConvTextToDouble(txtMount_Fixture_DBC_Back.Text);
                                break;

                            case "txtMount_Fixture_D_Finish_Front":
                                //---------------------------------
                                mBearing_Radial_FP.Mount.Fixture[0].D_Finish = modMain.ConvTextToDouble(txtMount_Fixture_D_Finish_Front.Text);
                                txtMount_Fixture_WallT_Back.Text = modMain.ConvDoubleToStr((Double)mBearing_Radial_FP.Mount.TWall_BearingCB(0), "#0.000");

                                SetMountFixture(mBearing_Radial_FP);
                                txtRoughing_Loc_Fixture_B.Text = mBearing_Radial_FP.Loc_Fixture_B().ToString("0.000");
                                break;

                            case "txtMount_Fixture_D_Finish_Back":
                                //--------------------------------
                                mBearing_Radial_FP.Mount.Fixture[1].D_Finish = modMain.ConvTextToDouble(txtMount_Fixture_D_Finish_Back.Text);
                                txtMount_Fixture_WallT_Back.Text = modMain.ConvDoubleToStr((Double)mBearing_Radial_FP.Mount.TWall_BearingCB(1), "#0.000");

                                SetMountFixture(mBearing_Radial_FP);
                                txtRoughing_Loc_Fixture_B.Text = mBearing_Radial_FP.Loc_Fixture_B().ToString("0.000");
                                break;


                            case "txtMount_Fixture_HolesAngOther1_Front":
                                //---------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                   mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[0] = 
                                                              modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther1_Front.Text);
                                   
                                }
                                break;

                            case "txtMount_Fixture_HolesAngOther1_Back":
                                //-------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[0] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther1_Back.Text);
                                     
                                }
                                break;



                            case "txtMount_Fixture_HolesAngOther2_Front":
                                //----------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[1] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther2_Front.Text);
                                }
                                break;

                            case "txtMount_Fixture_HolesAngOther2_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[1] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther2_Back.Text);
                                }
                                break;


                            case "txtMount_Fixture_HolesAngOther3_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[2] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther3_Front.Text);
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther3_Back":
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[2] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther3_Back.Text);
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther4_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[3] =
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther4_Front.Text);
                                }
                                break;

                            case "txtMount_Fixture_HolesAngOther4_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[3] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther4_Back.Text);
                                }
                                break;

                            case "txtMount_Fixture_HolesAngOther5_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {                                   
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[4] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther5_Front.Text);
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther5_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                     mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[4] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther5_Back.Text);
                                }
                                break;

                            case "txtMount_Fixture_HolesAngOther6_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[5] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther6_Front.Text);
                                    
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther6_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                     mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[5] = modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther6_Back.Text);
                                    
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther7_Front":
                                //-------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[0].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                                     mBearing_Radial_FP.Mount.Fixture[0].HolesAngOther[6] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther7_Front.Text);
                                }

                                break;

                            case "txtMount_Fixture_HolesAngOther7_Back":
                                //---------------------------------------
                                if (!mBearing_Radial_FP.Mount.Fixture[1].HolesEquiSpaced && !mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    mBearing_Radial_FP.Mount.Fixture[1].HolesAngOther[6] = 
                                                                        modMain.ConvTextToDouble(txtMount_Fixture_HolesAngOther7_Back.Text);
                                }

                                break;

                            case "txtMount_Fixture_Screw_D_Desig_Front":
                                //----------------------------------------
                                if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                {
                               
                                    String pWHERE = "WHERE fldD_Desig = '" + mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig + "'";
                                    modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Front, "tblManf_Screw", "fldType", pWHERE, true);
                                    Update_Fixture_Thread_L(0);                              
                                }

                                break;

                            case "txtMount_Fixture_Screw_D_Desig_Back":
                                //----------------------------------------
                                if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                {
                                    String pWHERE = "WHERE fldD_Desig = '" + mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig + "'";
                                    modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Back, "tblManf_Screw", "fldType", pWHERE, true);
                                    Update_Fixture_Thread_L(1);                               
                                }

                                break;

                        #endregion


                        #region "TempSensor:"

                            case "txtTempSensor_AngStart":
                                //------------------------------
                                mBearing_Radial_FP.TempSensor.AngStart =
                                    modMain.ConvTextToDouble(txtTempSensor_AngStart.Text);

                                break;


                            case "txtTempSensor_CanLength":
                             //-------------------------
                                 mBearing_Radial_FP.TempSensor.CanLength = modMain.ConvTextToDouble(pTxtBox.Text);

                                if (Math.Abs(mBearing_Radial_FP.TempSensor.CanLength - mBearing_Radial_FP.TempSensor.CAN_LENGTH) < modMain.gcEPS)
                                {
                                    txtTempSensor_CanLength.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    txtTempSensor_CanLength.ForeColor = Color.Black;
                                }

                                if (mblnTempSensor_CanLength_ManuallyChanged)
                                {
                                    txtTempSensor_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.Calc_Depth(),
                                                                                                 modMain.gUnit.MFormat);
                                    txtTempSensor_Depth.ForeColor = Color.Blue;
                                    mblnTempSensor_CanLength_ManuallyChanged= false;
                                }

                                SetColor_TempSensor_Depth();
                                break;


                            case "txtTempSensor_Depth":
                            //-------------------------
                                mBearing_Radial_FP.TempSensor.Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                                SetColor_TempSensor_Depth();
                                break;

                        #endregion


                        #region"EDM PAD:"
                        //---------------

                            case "txtEDM_Pad_RFilletBack":
                            //-----------------------------
                                mBearing_Radial_FP.EDM_Pad.RFillet_Back = modMain.ConvTextToDouble(pTxtBox.Text);
                                if (Math.Abs(mBearing_Radial_FP.EDM_Pad.RFillet_Back - mBearing_Radial_FP.Pad.RFillet_ID()) < modMain.gcEPS)
                                {
                                    txtEDM_Pad_RFilletBack.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    txtEDM_Pad_RFilletBack.ForeColor = Color.Black;
                                }
                                                    
                                break;
                        #endregion

                    }
                }

                //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                private void TextBox_Validating(object sender, CancelEventArgs e)
                //================================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    Double pVal = 0.0;

                    switch (pTxtBox.Name)
                    {
                        case "txtDepth_EndConfig_Front":
                            //------------------------------
                            Double pPreVal = mBearing_Radial_FP.Depth_EndConfig[0];

                            //if (mblnL_ManuallyChanged)    //PB 17JAN13
                            //{
                            //    mBearing_Radial_FP.Depth_EndConfig[0] = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);  
                            //}

                            if (mblnDepth_EndConfig_F_ManuallyChanged && txtDepth_EndConfig_Front.Text != "")
                            {
                                if (!mblnDepth_EndConfig_B_ManuallyChanged)
                                {
                                    //....Retrieve from Text Box.
                                    double pDepthF = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);

                                    //....Validate & reset to Min Value, if necessary.
                                    if (pDepthF < mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                                    {
                                        pDepthF = mBearing_Radial_FP.Validate_Depth_EndConfig(pDepthF);
                                        txtDepth_EndConfig_Front.Text = pDepthF.ToString("#0.000");             //....Redisplay.
                                        txtDepth_EndConfig_Front.ForeColor = Color.Magenta;
                                        e.Cancel = true;
                                    }

                                    //....Assign. 
                                    mBearing_Radial_FP.Depth_EndConfig[0] = pDepthF;

                                    //....Update the Depth Back.
                                    Update_Depth_EndConfig(txtDepth_EndConfig_Front, txtDepth_EndConfig_Back);

                                    double pLoc = mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace();
                                    txtOilInlet_Orifice_Loc_FrontFace.Text = modMain.ConvDoubleToStr(pLoc, "#0.000");

                                    txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                    txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                    txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                    txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12

                                    //  Reset the state. 
                                    //  ---------------
                                    mblnDepth_EndConfig_F_ManuallyChanged = false;
                                }

                                if (Math.Abs(pPreVal - mBearing_Radial_FP.Depth_EndConfig[0]) > modMain.gcEPS)
                                {
                                    txtOilInlet_Orifice_Loc_FrontFace.ForeColor = Color.Blue;
                                }
                            }

                            break;

                        case "txtDepth_EndConfig_Back":
                            //-------------------------
                            //if (mblnL_ManuallyChanged)    //PB 17JAN13
                            //{
                            //    mBearing_Radial_FP.Depth_EndConfig[1] = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                            //}

                            if (mblnDepth_EndConfig_B_ManuallyChanged && txtDepth_EndConfig_Back.Text != "")
                            {
                                if (!mblnDepth_EndConfig_F_ManuallyChanged)
                                {
                                    //....Retrieve from Text Box.
                                    double pDepthB = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);

                                    //....Validate & reset to Min Value, if necessary.
                                    if (pDepthB < mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                                    {
                                        pDepthB = mBearing_Radial_FP.Validate_Depth_EndConfig(pDepthB);
                                        txtDepth_EndConfig_Back.Text = pDepthB.ToString("#0.000");          //....Redisplay.
                                        txtDepth_EndConfig_Back.ForeColor = Color.Magenta;
                                        e.Cancel = true;
                                    }

                                    //....Assign. 
                                    mBearing_Radial_FP.Depth_EndConfig[1] = pDepthB;

                                    //....Update the Depth Front.
                                    Update_Depth_EndConfig(txtDepth_EndConfig_Back, txtDepth_EndConfig_Front);
                                }

                                txtSL_LScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("LScrew"), "#0.000");    //BG 26NOV12
                                txtSL_RScrew_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Front("RScrew"), "#0.000");    //BG 26NOV12

                                txtSL_LDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Front(), "#0.000");    //BG 27NOV12
                                txtSL_RDowel_Loc_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Front(), "#0.000");    //BG 27NOV12

                                //  Reset the state. 
                                //  ---------------
                                mblnDepth_EndConfig_B_ManuallyChanged = false;
                            }

                            break;

                        case "txtMount_Holes_Thread_Depth_Front":
                            //---------------------------------------
                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                            {
                                pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Front.Text);

                                if (mblnMount_Holes_Thread_Depth_ManuallyChanged)
                                {
                                    mblnMount_Holes_Thread_Depth_ManuallyChanged = false;
                                    txtMount_Holes_Thread_Depth_Front.Text =
                                        mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth(0, pVal).ToString("#0.00#");
                                    pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Front.Text);
                                    e.Cancel = true;
                                }

                                mBearing_Radial_FP.Mount.Holes_Thread_Depth[0] = pVal;
                                Double pScrew_Spec_D = Math.Round(2 * mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D, 3);
                                //if (mBearing_Radial_FP.Mount.Holes_Thread_Depth[0] == 2 * mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D)
                                if (Math.Abs(mBearing_Radial_FP.Mount.Holes_Thread_Depth[0] - pScrew_Spec_D) < modMain.gcEPS)
                                {
                                    txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    txtMount_Holes_Thread_Depth_Front.ForeColor = Color.Black;
                                }
                            }

                            break;

                        case "txtMount_Holes_Thread_Depth_Back":
                            //---------------------------------------
                            if (!mBearing_Radial_FP.Mount.Holes_GoThru)
                            {
                                pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Back.Text);

                                if (mblnMount_Holes_Thread_Depth_ManuallyChanged)
                                {
                                    mblnMount_Holes_Thread_Depth_ManuallyChanged = false;
                                    txtMount_Holes_Thread_Depth_Back.Text =
                                        mBearing_Radial_FP.Mount.Validate_Holes_Thread_Depth(1, pVal).ToString("#0.00#");
                                    pVal = modMain.ConvTextToDouble(txtMount_Holes_Thread_Depth_Back.Text);
                                    e.Cancel = true;
                                }

                                mBearing_Radial_FP.Mount.Holes_Thread_Depth[1] = pVal;
                                Double pScrew_Spec_D = Math.Round(2 * mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D, 3);

                                //if (mBearing_Radial_FP.Mount.Holes_Thread_Depth[1] == 2 * mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D)
                                if (Math.Abs(mBearing_Radial_FP.Mount.Holes_Thread_Depth[1] - pScrew_Spec_D) < modMain.gcEPS)
                                {
                                    txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Magenta;
                                }
                                else
                                {
                                    txtMount_Holes_Thread_Depth_Back.ForeColor = Color.Black;
                                }
                            }

                            break;
                    }
                }

                #region "Helper Routines:"
                //************************

                    private void SetColor_Orifice_Loc_FrontFace()
                    //===========================================
                    {
                        Double pCalc_Orifice_Loc_FrontFace = mBearing_Radial_FP.OilInlet.Calc_Orifice_Loc_FrontFace();

                        if (Math.Abs(mBearing_Radial_FP.OilInlet.Orifice.Loc_FrontFace - pCalc_Orifice_Loc_FrontFace) < modMain.gcEPS)
                        {
                            txtOilInlet_Orifice_Loc_FrontFace.ForeColor = Color.Blue;
                        }
                        else
                        {
                            txtOilInlet_Orifice_Loc_FrontFace.ForeColor = Color.Black;
                        }

                    }

                    private void SetColor_Annulus_Loc_Back()
                    //=======================================
                    {
                        Double pCalc_Annulus_Loc_Back = Math.Round( mBearing_Radial_FP.OilInlet.Calc_Annulus_Loc_Back(),3);
                        if (Math.Abs(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back - pCalc_Annulus_Loc_Back) < modMain.gcEPS)
                        {
                            txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Blue;
                        }
                        else
                        {
                            txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Black;
                        }

                    }

                    private void SetColor_TempSensor_Depth()
                   //=======================================
                    {
                        Double pCalc_Depth = mBearing_Radial_FP.TempSensor.Calc_Depth();
                        if (Math.Abs(mBearing_Radial_FP.TempSensor.Depth - pCalc_Depth) < modMain.gcEPS)
                        {
                            txtTempSensor_Depth.ForeColor = Color.Blue;
                        }
                        else
                        {
                            txtTempSensor_Depth.ForeColor = Color.Black;
                        }

                    }

                    private void SetForeColor_Depth_EndConfig(TextBox TxtBox_In, int Indx_In)
                    //========================================================================
                    {
                        if (Math.Abs(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndConfig[Indx_In] -
                                                           mBearing_Radial_FP.Depth_EndConfig[Indx_In]) < modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;

                        }
                    }


                    private void Update_Depth_EndConfig(TextBox Txt_In, TextBox Txt_Out)
                    //===================================================================
                    {
                        //Double pDepth_Tot = mBearing_Radial_FP.L - (mBearing_Radial_FP.Pad.L + mBearing_Radial_FP.EDM_Relief[0] + mBearing_Radial_FP.EDM_Relief[1]);      //BG 06DEC12
                        Double pDepth_Tot = mBearing_Radial_FP.L - (mBearing_Radial_FP.Pad.L + 
                                                                    mBearing_Radial_FP.MillRelief.EDM_Relief[0] + 
                                                                    mBearing_Radial_FP.MillRelief.EDM_Relief[1]);  

                        //if (modMain.ConvTextToDouble(Txt_In.Text) >= mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                        //{
                        //    Txt_Out.Text = (pDepth_Tot - modMain.ConvTextToDouble(Txt_In.Text)).ToString("#0.000");
                        //}

                        Double pDepth_Other = pDepth_Tot - modMain.ConvTextToDouble(Txt_In.Text);
                        Txt_Out.Text = pDepth_Other.ToString("#0.000");

                        if(pDepth_Other >= mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                        {
                            Txt_Out.ForeColor = Color.Blue;
                        }
                        else
                        {
                            Txt_Out.ForeColor = Color.Red;
                        }
                    }

                #endregion


            #endregion


            #region "TAB CONTROL RELATED ROUTINE"
            //===================================              
            
                private void tbBearingDesignDetails_SelectedIndexChanged(object sender, EventArgs e)
                //==================================================================================
                {
                    //  Index Mounting Tab is 6
                    //  -------------------------

                    if (tbBearingDesignDetails.SelectedIndex == 6)
                    //--------------------------------------------
                    {
                        Upadate_Fixture_Selection(mBearing_Radial_FP, false);
                    }

                    if (tbBearingDesignDetails.SelectedIndex == 7)
                    //--------------------------------------------
                    {
                        if (modMain.gProject.Product.Accessories.TempSensor.Supplied)
                        {
                            chkTempSensor_Exists.Checked = true;
                            chkTempSensor_Exists.Enabled = false;
                            cmbTempSensor_Count.Enabled = false;
                            cmbTempSensor_Count.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.TempSensor.Count);
                        }
                        else
                        {
                            chkTempSensor_Exists.Enabled = true;
                            chkTempSensor_Exists.Checked = mBearing_Radial_FP.TempSensor.Exists;
                            cmbTempSensor_Count.Enabled = true;
                            cmbTempSensor_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.TempSensor.Count);
                        }

                    }
                    
                }

            #endregion

        #endregion


        #region "UTILITY ROUTINES:"
        //************************

            #region "OIL INLET."

                private void Update_OilInlet_Annulus_Display()
                //============================================ 
                {
                    if (mBearing_Radial_FP.OilInlet.Count_MainOilSupply != 0)
                    {
                        Double pRatio_LH = mBearing_Radial_FP.OilInlet.Calc_Annulus_Ratio_L_H(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text));      //BG 16MAY12
                        if (mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H.ToString("#0.00") != pRatio_LH.ToString("#0.00"))
                        {
                            if (!mblnOilInlet_Annulus_D_ManuallyChanged && !mblnOilInlet_Annulus_L_ManuallyChanged)
                            {
                                mBearing_Radial_FP.OilInlet.Annulus_D = 0.0;
                                mBearing_Radial_FP.OilInlet.Annulus_L = 0.0;
                            }
                        }
                                     
                        txtOilInlet_Annulus_D.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.D, modMain.gUnit.MFormat);
                        if (!mblnOilInlet_Annulus_D_ManuallyChanged) txtOilInlet_Annulus_D.ForeColor = Color.Blue;

                        txtOilInlet_Annulus_L.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.L, modMain.gUnit.MFormat);      
                        if(!mblnOilInlet_Annulus_L_ManuallyChanged) txtOilInlet_Annulus_L.ForeColor = Color.Blue;
                       
                        //txtOilInlet_Annulus_Loc_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back, "#0.000"); //SG 05OCT12
                                               
                        txtOilInlet_Annulus_V.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Annulus_V(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text), modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text)), "#0.000");
                        txtOilInlet_Annulus_V.ForeColor = Color.Blue;
                    }
                }

            #endregion


            #region "WEB RELIEF:"

                private void Populate_MillRelief_D_Desig()
                //========================================     
                {                   
                    string pWHERE;
                    pWHERE = " WHERE fldCons_MillRelief = 'Y'";
                   
                    bool pBln = false;

                    StringCollection pMillRelief_D = new StringCollection();
                    modMain.gDB.PopulateStringCol(pMillRelief_D, "tblManf_Drill", "fldD_Desig", pWHERE);

                    StringCollection pMillRelief_DwoIn = new StringCollection();
                    Double pNumerator, pDenominator;
                    Double pFinal;

                    for (int i = 0; i < pMillRelief_D.Count; i++)
                        pMillRelief_D[i] = pMillRelief_D[i].Remove(pMillRelief_D[i].Length - 1);

                    for (int i = 0; i < pMillRelief_D.Count; i++)
                    if (pMillRelief_D[i].Contains("/"))
                    {
                        if (pMillRelief_D[i].ToString() != "1")
                        {
                            pNumerator = Convert.ToInt32(modMain.ExtractPreData(pMillRelief_D[i], "/"));
                            pDenominator = Convert.ToInt32(modMain.ExtractPostData(pMillRelief_D[i], "/"));
                            pFinal = Convert.ToDouble(pNumerator / pDenominator);

                            //BG 02JUL13
                            if (pFinal > mBearing_Radial_FP.FlexurePivot.Web.H)
                            {
                                pMillRelief_DwoIn.Add(pFinal.ToString());
                            }
                        }
                        else      
                        {
                            //BG 02JUL13
                            pFinal = Convert.ToDouble(pMillRelief_D[i]);

                            if (pFinal > mBearing_Radial_FP.FlexurePivot.Web.H)
                            {
                                pMillRelief_DwoIn.Add(pFinal.ToString());
                            }

                            //pMillRelief_DwoIn.Add(pMillRelief_D[i]);
                        }
                    }
                   
                    modMain.SortNumberwoHash(ref pMillRelief_DwoIn, true);

                    pMillRelief_D.Clear();
                    for (int i = 0; i < pMillRelief_DwoIn.Count; i++)
                        pMillRelief_D.Add(pMillRelief_DwoIn[i] + "\"");

                    cmbMillRelief_D_Desig.Items.Clear();
                    if (pMillRelief_D.Count > 0)
                        for (int i = 0; i < pMillRelief_D.Count; i++)
                            cmbMillRelief_D_Desig.Items.Add(pMillRelief_D[i]);  
                }

            #endregion
        

            #region "SPLIT LINE HARDWARE:"

                private void Update_SL_Screw_L()
                //==============================
                {
                    if (cmbSL_Screw_Spec_Type.Text != ""
                                && cmbSL_Screw_Spec_D_Desig.Text != "")
                        Populate_SL_Screw_L();

                    string pFormat = null;

                    if (cmbSL_Screw_Spec_Type.Text != "" && cmbSL_Screw_Spec_D_Desig.Text != "")
                    {
                        if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System == clsUnit.eSystem.Metric)       //BG 26MAR12
                        {
                            pFormat = "#0";
                            lblSL_Screw_LLim.Text = modMain.ConvDoubleToStr(Math.Ceiling(mBearing_Radial_FP.SL.Thread_L_LowerLimit()), pFormat);
                        }
                        else if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System == clsUnit.eSystem.English) //BG 26MAR12
                        {
                            pFormat = "#0.000";
                            lblSL_Screw_LLim.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_L_LowerLimit(), pFormat);
                        }
                    }
                    else
                        lblSL_Screw_LLim.Text = "";
                }


                private void Populate_SL_Screw_L()
                //================================
                {
                    string pSL_Screw_Unit = mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Screw_D = mBearing_Radial_FP.SL.Screw_Spec.D_Desig; //cmbSL_Screw_Spec_D_Desig.Text;
                    string pSL_Screw_Type = mBearing_Radial_FP.SL.Screw_Spec.Type;//cmbSL_Screw_Spec_Type.Text;                    
                    string pSL_Screw_Mat = mBearing_Radial_FP.SL.Screw_Spec.Mat; //cmbSL_Screw_Spec_Mat.Text;
                    string pstrWHERE = null;

                    if (chkSL_Screw_LenLim.Checked)
                    {
                        pstrWHERE = " WHERE fldUnit  = '" + pSL_Screw_Unit +
                                  "' AND fldType = '"     + pSL_Screw_Type +
                                  "' AND fldD_Desig = '"  + pSL_Screw_D +
                                  "' AND fldMat = '"      + pSL_Screw_Mat +
                                  "' AND fldL >" + modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_L_LowerLimit(), ""); 
                    }
                    else
                    {
                        pstrWHERE = " WHERE fldUnit  = '"  + pSL_Screw_Unit +                                                                 
                                    "' AND fldType = '"    + pSL_Screw_Type +
                                    "' AND fldD_Desig = '" + pSL_Screw_D + 
                                    "' AND fldMat = '"     + pSL_Screw_Mat + "'";
                    }

                    StringCollection pScrew_L = new StringCollection();
                    modMain.gDB.PopulateStringCol(pScrew_L, "tblManf_Screw", "fldL", pstrWHERE, true);                  
                    modMain.SortNumberwoHash(ref pScrew_L, false);

                    if (pScrew_L.Count > 0)
                    {
                        cmbSL_Screw_Spec_L.Items.Clear();
                        for (int i = 0; i < pScrew_L.Count; i++)
                        {
                            Double pVal = Convert.ToDouble(pScrew_L[i]);
                            cmbSL_Screw_Spec_L.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.00#"));
                        }
                    }

                    if (cmbSL_Screw_Spec_L.Items.Count > 0)
                    {
                        if (mBearing_Radial_FP.SL.Screw_Spec.L > modMain.gcEPS)
                        {
                            if (cmbSL_Screw_Spec_L.Items.Contains(mBearing_Radial_FP.SL.Screw_Spec.L.ToString("#0.00#")))
                                cmbSL_Screw_Spec_L.SelectedIndex = cmbSL_Screw_Spec_L.Items.IndexOf(mBearing_Radial_FP.SL.Screw_Spec.L.ToString("#0.00#"));
                            else
                                cmbSL_Screw_Spec_L.SelectedIndex = 0;
                        }
                    }

                    else
                    {
                        ChangeCheck_SL_Screw();
                    }
                }


                private void ChangeCheck_SL_Screw()
                //=================================
                {
                    //....Caption & Message.
                    String pMsg = "For the selected Type, Material and Diameter no thread length" + System.Environment.NewLine +
                                 "is found in " + "\"" + "Screw" + "\""+
                                 " table in the database that statisfies" + System.Environment.NewLine +
                                 "the given limit constraint. Hence limit can not be imposed.";

                    String pCaption = "Information";

                    //....Show message box.
                    MessageBox.Show(pMsg,pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);        

                    //....Checked = false.
                    chkSL_Screw_LenLim.Checked = false;   
                }


                //....Not used now
                private Double Get_SL_Screw_L(clsBearing_Radial_FP Bearing_In)
                //============================================================
                {
                    Double pThread_Length = 0.0F;
                    string pSpLineHard_Thread_D = cmbSL_Screw_Spec_D_Desig.Text;
                    string pSpLineHard_Thread_Type = cmbSL_Screw_Spec_Type.Text;
                    string pSpLineHard_Thread_Mat = cmbSL_Screw_Spec_Mat.Text;
                    

                    if (pSpLineHard_Thread_D != "" && pSpLineHard_Thread_Type != ""
                        && Bearing_In.SL.Thread_L_LowerLimit() != 0.0F && chkSL_Screw_LenLim.Checked)
                    {
                        string pstrWHERE = null;
                        
                        //pstrWHERE = " WHERE fldUnit  = '" + modMain.gUnit.System.ToString().Substring(0, 1) +
                        pstrWHERE = " WHERE fldUnit  = '" + mBearing_Radial_FP.SL.Screw_Spec.Unit.System.ToString().Substring(0, 1) +     
                                    "' AND fldType = '" + pSpLineHard_Thread_Type +
                                    "' AND fldD_Desig = '" + pSpLineHard_Thread_D +
                                    "' AND fldMat = '" + pSpLineHard_Thread_Mat +
                                    "' AND fldL >" + modMain.ConvDoubleToStr(Bearing_In.SL.Thread_L_LowerLimit(), "");                    
                        

                        StringCollection pThread_L = new StringCollection();
                        modMain.gDB.PopulateStringCol(pThread_L, "tblManf_Screw", "fldL", pstrWHERE, true); 
                        //SortNumberwoHash(ref pThread_L, false);
                        modMain.SortNumberwoHash(ref pThread_L, false);

                        if (pThread_L.Count > 0)
                        {
                            cmbSL_Screw_Spec_L.Items.Clear();
                            for (int i = 0; i < pThread_L.Count; i++)
                            {
                                Double pVal = modMain.ConvTextToDouble(pThread_L[i]);
                                cmbSL_Screw_Spec_L.Items.Add(pVal.ToString("#0.00#"));
                            }

                            if (Bearing_In.SL.Screw_Spec.L > modMain.gcEPS && cmbSL_Screw_Spec_L.Items.Contains(Bearing_In.SL.Screw_Spec.L.ToString()))
                            {
                                pThread_Length = Bearing_In.SL.Screw_Spec.L;
                            }
                            else
                            {
                                pThread_Length = modMain.ConvTextToDouble(pThread_L[0]);
                            }
                        }
                    }

                    return pThread_Length;
                }


                private void Update_SL_Dowel_L()
                //==============================
                {
                    if (cmbSL_Dowel_Spec_Type.Text != "" && cmbSL_Dowel_Spec_D_Desig.Text != "")
                        Populate_SL_Dowel_L();

                    string pFormat = null;

                    if (cmbSL_Dowel_Spec_Type.Text != "" && cmbSL_Dowel_Spec_D_Desig.Text != "")
                    {
                        if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System == clsUnit.eSystem.Metric)           
                        {
                            pFormat = "#0";
                            lblSL_Dowel_LLim.Text = modMain.ConvDoubleToStr(Math.Ceiling(mBearing_Radial_FP.SL.Pin_L_LowerLimit()), pFormat);
                        }
                        else if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System == clsUnit.eSystem.English)     
                        {
                            pFormat = "#0.000";
                            lblSL_Dowel_LLim.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Pin_L_LowerLimit(), pFormat);
                        }
                    }
                    else
                        lblSL_Dowel_LLim.Text = "";
                }


                private void Populate_SL_Dowel_L()
                //================================         
                {  
                    string pSL_Dowel_Unit = mBearing_Radial_FP.SL.Dowel_Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Dowel_D = mBearing_Radial_FP.SL.Dowel_Spec.D_Desig;         
                    string pSL_Dowel_Type = mBearing_Radial_FP.SL.Dowel_Spec.Type;
                    string pSL_Dowel_Mat = mBearing_Radial_FP.SL.Dowel_Spec.Mat;

                    string pWHERE = null;

                    if (chkSL_Dowel_LenLim.Checked)
                    {   
                        pWHERE = " WHERE fldUnit  = '"   + pSL_Dowel_Unit +                                                       
                                  "' AND fldType = '"    + pSL_Dowel_Type +
                                  "' AND fldD_Desig = '" + pSL_Dowel_D + 
                                  "' AND fldMat = '"     + pSL_Dowel_Mat +
                                  "' AND fldL > " +  modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Pin_L_LowerLimit(), "");

                    }
                    else
                    {                        
                        pWHERE = " WHERE fldUnit  = '"   + pSL_Dowel_Unit +        
                                  "' AND fldType = '"    + pSL_Dowel_Type +
                                  "' AND fldD_Desig = '" + pSL_Dowel_D +
                                  "' AND fldMat = '"     + pSL_Dowel_Mat + "'";
                    }

                    StringCollection pDowel_L = new StringCollection();
                    modMain.gDB.PopulateStringCol(pDowel_L, "tblManf_Pin", "fldL", pWHERE, true);

                    if (pDowel_L.Count > 0)
                    {
                        cmbSL_Dowel_Spec_L.Items.Clear();
                        for (int i = 0; i < pDowel_L.Count; i++)
                        {
                            Double pVal = Convert.ToDouble(pDowel_L[i]);
                            cmbSL_Dowel_Spec_L.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.00#"));
                        }
                    }

                    if (cmbSL_Dowel_Spec_L.Items.Count > 0)
                    {
                        if (mBearing_Radial_FP.SL.Dowel_Spec.L > modMain.gcEPS)

                            if( cmbSL_Dowel_Spec_L.Items.Contains(mBearing_Radial_FP.SL.Dowel_Spec.L.ToString("#0.00#")))
                                cmbSL_Dowel_Spec_L.SelectedIndex = cmbSL_Dowel_Spec_L.Items.IndexOf(mBearing_Radial_FP.SL.Dowel_Spec.L.ToString("#0.00#"));
                           else
                                cmbSL_Dowel_Spec_L.SelectedIndex = 0;
                    }
                    else
                    {
                        ChangeCheck_SL_Dowel();
                    }
                }


                private void ChangeCheck_SL_Dowel()
                //=================================
                {
                    //....Caption & Message.
                    String pMsg = "For the selected Type, Material and Diameter no Pin length" + System.Environment.NewLine +
                                 "is found in " + "\"" + "Pin" + "\"" +
                                 " table in the database that statisfies" + System.Environment.NewLine +
                                 "the given limit constraint. Hence limit can not be imposed.";

                    String pCaption = "Information";

                    //....Show message box.
                    MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);         //SB 10AUG09

                    chkSL_Dowel_LenLim.Checked = false;
                }

            #endregion


            #region "Anti-Rotation Pin:"

                private void Populate_AntiRotPin_L()
                //==================================
                {
                    string pAntiRotPin_Unit = mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString().Substring(0, 1);
                    string pAntiRotPin_D = mBearing_Radial_FP.AntiRotPin.Spec.D_Desig;
                    string pAntiRotPin_Type = mBearing_Radial_FP.AntiRotPin.Spec.Type; 
                    string pAntiRotPin_Mat = mBearing_Radial_FP.AntiRotPin.Spec.Mat;
                    string pWHERE = null;

                    pWHERE = " WHERE fldUnit  = '" + pAntiRotPin_Unit +
                             "' AND fldType = '" + pAntiRotPin_Type +
                             "' AND fldD_Desig = '" + pAntiRotPin_D +
                             "' AND fldMat = '" + pAntiRotPin_Mat + "'";

                    StringCollection pAntiRotPin_L = new StringCollection();
                    modMain.gDB.PopulateStringCol(pAntiRotPin_L, "tblManf_Pin", "fldL", pWHERE, true);

                    cmbAntiRotPin_Spec_L.Items.Clear();
                    for (int i = 0; i < pAntiRotPin_L.Count; i++)
                    {
                        Double pVal = Convert.ToDouble(pAntiRotPin_L[i]);
                        cmbAntiRotPin_Spec_L.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.00#"));
                    }

                    if (cmbAntiRotPin_Spec_L.Items.Count > 0)
                    {
                        if (mBearing_Radial_FP.AntiRotPin.Spec.L > modMain.gcEPS &&
                                        cmbAntiRotPin_Spec_L.Items.Contains(mBearing_Radial_FP.AntiRotPin.Spec.L.ToString("#0.00#")))

                            cmbAntiRotPin_Spec_L.Text = mBearing_Radial_FP.AntiRotPin.Spec.L.ToString("#0.00#");
                        else
                            cmbAntiRotPin_Spec_L.SelectedIndex = 0;
                    }
                }

            #endregion


            #region "END CONFIGS MOUNT FIXTURE:"

                #region "GO-THRU RELATED ROUTINE:"              

                    private void SetControls_Mount_Holes_GoThru(Boolean GoThru_In)
                    //============================================================   
                    {
                         //....Bolting, TabPage and Depth.
                        if (!chkMount_Holes_GoThru.Checked)
                        {
                            chkMount_Holes_Bolting_Front.Checked = !chkMount_Holes_GoThru.Checked;
                            chkMount_Holes_Bolting_Back.Checked = !chkMount_Holes_GoThru.Checked;
                        }
                        else
                        {
                            if (mBearing_Radial_FP.Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Both)
                            {
                                ////BG 02JUL13
                                //chkMount_Holes_Bolting_Back.Checked = !chkMount_Holes_GoThru.Checked;
                                //chkMount_Holes_Bolting_Front.Checked = !chkMount_Holes_GoThru.Checked;
                                //chkMount_Holes_Bolting_Back.Checked = chkMount_Holes_GoThru.Checked;

                                // BG 02JUL13
                                chkMount_Holes_Bolting_Front.Checked = !chkMount_Holes_GoThru.Checked;
                                chkMount_Holes_Bolting_Back.Checked = !chkMount_Holes_GoThru.Checked;
                                chkMount_Holes_Bolting_Front.Checked = chkMount_Holes_GoThru.Checked;
                            }
                        }
                        
                        TabPage pTabFront = tabFront;
                        TabPage pTabBack = tabBack;

                        tabControl_Mount_Holes.TabPages.Clear();

                        //if (modMain.gProject.Status == "Open" && modMain.gUser.Privilege == "Engineering")
                        if (modMain.gProject.Status == "Open" && modMain.gUser.Role == "Engineer")
                        {
                            grpMount_Holes_Bolting.Enabled = GoThru_In;
                        }

                        if (GoThru_In)
                        {
                            if (mBearing_Radial_FP.Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)  
                            {                                
                                chkMount_Holes_Bolting_Front.Checked = GoThru_In;
                                chkMount_Holes_Bolting_Back.Checked = !GoThru_In;

                                tabControl_Mount_Holes.TabPages.Add(pTabFront);
                                tabControl_Mount_Holes.Refresh();
                            }

                            else if (mBearing_Radial_FP.Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                            {
                                chkMount_Holes_Bolting_Back.Checked = GoThru_In;
                                chkMount_Holes_Bolting_Front.Checked = !GoThru_In;

                                tabControl_Mount_Holes.TabPages.Add(pTabBack);
                                tabControl_Mount_Holes.Refresh();
                            }                          
                        }

                        else 
                        {                           
                            chkMount_Holes_Bolting_Front.Checked = !GoThru_In;
                            chkMount_Holes_Bolting_Back.Checked = !GoThru_In;
                            //grpMount_Holes_Bolting.Enabled = GoThru_In;

                            tabControl_Mount_Holes.TabPages.Add(pTabFront);
                            tabControl_Mount_Holes.TabPages.Add(pTabBack);
                        }

                        lblMount_Holes_Depth_Front.Visible = !GoThru_In;
                        txtMount_Holes_Thread_Depth_Front.Visible = !GoThru_In;
                        lblMount_Holes_Depth_Back.Visible = !GoThru_In;
                        txtMount_Holes_Thread_Depth_Back.Visible = !GoThru_In;
                    }


                #endregion


                #region "THREAD RELATED ROUTINES:"

                    private void Populate_Fixture_Thread_Mat(ref ComboBox CmbMat_In, String Thread_Type_In, String Thread_D_Desig)
                    //============================================================================================================
                    {
                        //.....Populate Dia_Desig ComboBox.
                        string pUnitSystem = "";

                        if (!Thread_D_Desig.Contains("M"))      
                            pUnitSystem = "E";
                        else if (Thread_D_Desig.Contains("M"))  
                            pUnitSystem = "M";

                        string pstrWHERE = " WHERE fldType  = '" + Thread_Type_In +
                                         "' AND fldD_Desig = '" + Thread_D_Desig + 
                                         "' AND fldUnit = '" + pUnitSystem + "'";

                        modMain.gDB.PopulateCmbBox(CmbMat_In, "tblManf_Screw", "fldMat", pstrWHERE, true);

                        if (CmbMat_In.Items.Count > 0)      
                        {
                            int pIndx = -1;

                            if (CmbMat_In.Items.Contains("STL"))
                                pIndx = CmbMat_In.Items.IndexOf("STL");
                            if (pIndx != -1)
                                CmbMat_In.SelectedIndex = pIndx;
                            else
                                CmbMat_In.SelectedIndex = 0;
                        }
                    }


                    private void Populate_Mount_Fixture_Thread_L(Int32 MountHole_Pos_In)    
                    //==================================================================
                    {
                        string pThread_Type = null;
                        string pThread_D = null;
                        string pThread_Mat = null;

                        string pWHERE = null;
                      
                        Double pThread_L_UpperLimit = 0.0F;
                        Double pThread_L_LowerLimit = 0.0F;

                        StringCollection pSealMountFixture_Sel_Thread_L = new StringCollection();

                        if (MountHole_Pos_In == 0)
                        {
                            //....  If SealMountFixture_Candidates_Chosen = true then
                            //          SealMountFixture D_Desig became textbox.
                            //....  Else If SealMountFixture_Candidates_Chosen = false then
                            //          SealMountFixture D_Desig became combobox.

                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                            {
                                if (cmbMount_Fixture_Screw_Type_Front.Text != "" &&
                                    txtMount_Fixture_Screw_D_Desig_Front.Text != ""
                                    && cmbMount_Fixture_Screw_Mat_Front.Text != "")
                                {
                                    pThread_Type = mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Type;// cmbMount_Fixture_Screw_Type_Front.Text;
                                    pThread_D = mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig;//txtMount_Fixture_Screw_D_Desig_Front.Text;
                                    pThread_Mat = mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.Mat;//cmbMount_Fixture_Screw_Mat_Front.Text;
                                }
                                else
                                    return;
                            }
                            else
                            {
                                if (cmbMount_Fixture_Screw_Type_Front.Text != "" &&
                                    cmbMount_Fixture_Screw_D_Desig_Front.Text != ""
                                    && cmbMount_Fixture_Screw_Mat_Front.Text != "")
                                {
                                    pThread_Type = cmbMount_Fixture_Screw_Type_Front.Text;
                                    pThread_D = cmbMount_Fixture_Screw_D_Desig_Front.Text;
                                    pThread_Mat = cmbMount_Fixture_Screw_Mat_Front.Text;
                                }
                                else
                                    return;
                            }

                            //....  If SealMountFixture_Candidates_Chosen = true then 
                            //      pThread_L_UpperLimit,pThread_L_LowerLimit applicable for selected length.
                            //....  Else If SealMountFixture_Candidates_Chosen = false then
                            //      pThread_L_UpperLimit,pThread_L_LowerLimit not applicable for selected length.
                    

                            if (chkMount_Fixture_Screw_LenLim_Front.Checked)
                            {
                                if (mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig.Contains("M"))
                                {                                    
                                    pThread_L_UpperLimit = (Double)Math.Floor(mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(0));
                                    pThread_L_LowerLimit = (Double)Math.Ceiling(mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(0));  
                                }
                                else
                                {
                                    pThread_L_UpperLimit = mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(0);// Fixture_Screw_L_UpperLimit(mRadialBearing.SealMountFixture_Sel_Thread_Front);
                                    pThread_L_LowerLimit = mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(0);// Fixture_Screw_L_LowerLimit(modMain.gEndSeal[0], mRadialBearing.SealMountFixture_Sel_Thread_Front);
                                }

                                pWHERE = " WHERE fldType = '" + pThread_Type +
                                         "' AND fldD_Desig = '" + pThread_D +
                                         "' AND fldMat = '" + pThread_Mat + "'" +
                                         "  AND fldL <=" + modMain.ConvDoubleToStr(pThread_L_UpperLimit, "") +
                                         "  AND fldL >=" + modMain.ConvDoubleToStr(pThread_L_LowerLimit, "");

                            }
                            else if (!chkMount_Fixture_Screw_LenLim_Front.Checked)
                            {

                                    pWHERE = " WHERE fldType = '" + pThread_Type +
                                             "' AND fldD_Desig = '" + pThread_D +
                                             "' AND fldMat = '" + pThread_Mat + "'"; 
                            }

                            //....Populate SealFixture Length.
                            modMain.gDB.PopulateStringCol(pSealMountFixture_Sel_Thread_L, "tblManf_Screw", "fldL", pWHERE, false);

                            cmbMount_Fixture_Screw_L_Front.Items.Clear();
                            for (int i = 0; i < pSealMountFixture_Sel_Thread_L.Count; i++)          
                            {
                                Double pVal = Convert.ToDouble(pSealMountFixture_Sel_Thread_L[i]);
                                cmbMount_Fixture_Screw_L_Front.Items.Add(pVal.ToString("#0.000"));
                            }

                            //....Check Change == True.
                            if (cmbMount_Fixture_Screw_L_Front.Items.Count > 0)
                            {
                                if (mBearing_Radial_FP.Mount.Fixture[MountHole_Pos_In].Screw_Spec.L > modMain.gcEPS &&
                                        cmbMount_Fixture_Screw_L_Front.Items.Contains(mBearing_Radial_FP.Mount.Fixture[MountHole_Pos_In].Screw_Spec.L.ToString("#0.000")))
                                    cmbMount_Fixture_Screw_L_Front.Text = mBearing_Radial_FP.Mount.Fixture[MountHole_Pos_In].Screw_Spec.L.ToString("#0.000");
                                else
                                     cmbMount_Fixture_Screw_L_Front.SelectedIndex = 0;

                                Double pThreadL = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_L_Front.Text);
                            }
                            else
                            {
                                //....Check Change == false.
                                ChangeCheck_SealMountHole_Thread(chkMount_Fixture_Screw_LenLim_Front);
                            }
                        }

                        else if (MountHole_Pos_In == 1)
                        {

                            //....  If SealMountFixture_Candidates_Chosen = true then
                            //          SealMountFixture D_Desig became textbox.
                            //....  Else If SealMountFixture_Candidates_Chosen = false then
                            //          SealMountFixture D_Desig became combobox.

                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                            {
                                if (cmbMount_Fixture_Screw_Type_Back.Text != "" &&
                                    txtMount_Fixture_Screw_D_Desig_Back.Text != ""
                                    && cmbMount_Fixture_Screw_Mat_Back.Text != "")
                                {
                                    pThread_Type = mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Type; //cmbMount_Fixture_Screw_Type_Back.Text;
                                    pThread_D = mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig; //txtMount_Fixture_Screw_D_Desig_Back.Text;
                                    pThread_Mat = mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.Mat; //cmbMount_Fixture_Screw_Mat_Back.Text;
                                }
                                else
                                    return;
                            }
                            else
                            {
                                if (cmbMount_Fixture_Screw_Type_Back.Text != "" &&
                                    cmbMount_Fixture_Screw_D_Desig_Back.Text != ""
                                    && cmbMount_Fixture_Screw_Mat_Back.Text != "")
                                {
                                    pThread_Type = cmbMount_Fixture_Screw_Type_Back.Text;
                                    pThread_D = cmbMount_Fixture_Screw_D_Desig_Back.Text;
                                    pThread_Mat = cmbMount_Fixture_Screw_Mat_Back.Text;
                                }
                                else
                                    return;
                            }

                            //....  If SealMountFixture_Candidates_Chosen = true then 
                            //      pThread_L_UpperLimit,pThread_L_LowerLimit applicable for selected length.
                            //....  Else If SealMountFixture_Candidates_Chosen = false then
                            //      pThread_L_UpperLimit,pThread_L_LowerLimit not applicable for selected length.


                            if (chkMount_Fixture_Screw_LenLim_Back.Checked)
                            {
                                if (mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig.Contains("M"))
                                {
                                    pThread_L_UpperLimit = (Double)Math.Floor(mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(1));
                                    pThread_L_LowerLimit = (Double)Math.Ceiling(mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(1));
                                }
                                else
                                {
                                    pThread_L_UpperLimit = mBearing_Radial_FP.Mount.MountFixture_Screw_L_UpperLimit(1);// Fixture_Screw_L_UpperLimit(mRadialBearing.SealMountFixture_Sel_Thread_Front);
                                    pThread_L_LowerLimit = mBearing_Radial_FP.Mount.MountFixture_Screw_L_LowerLimit(1);// Fixture_Screw_L_LowerLimit(modMain.gEndSeal[0], mRadialBearing.SealMountFixture_Sel_Thread_Front);
                                }

                                pWHERE = " WHERE fldType = '" + pThread_Type +
                                        "' AND fldD_Desig = '" + pThread_D +
                                        "' AND fldMat = '" + pThread_Mat +
                                        "'" +
                                        " AND fldL <=" + modMain.ConvDoubleToStr(pThread_L_UpperLimit, "") +
                                        " AND fldL >=" + modMain.ConvDoubleToStr(pThread_L_LowerLimit, "");

                            }
                            else if (!chkMount_Fixture_Screw_LenLim_Back.Checked)
                            {

                                pWHERE = " WHERE fldType = '" + pThread_Type +
                                         "' AND fldD_Desig = '" + pThread_D +
                                         "' AND fldMat = '" + pThread_Mat + "'";
                            }

                            //....Populate SealFixture Length.
                            modMain.gDB.PopulateStringCol(pSealMountFixture_Sel_Thread_L, "tblManf_Screw", "fldL", pWHERE, false);

                            cmbMount_Fixture_Screw_L_Back.Items.Clear();
                            for (int i = 0; i < pSealMountFixture_Sel_Thread_L.Count; i++)
                            {
                                Double pVal = Convert.ToDouble(pSealMountFixture_Sel_Thread_L[i]);
                                cmbMount_Fixture_Screw_L_Back.Items.Add(pVal.ToString("#0.000"));
                            }

                            //....Check Change == True.
                            if (cmbMount_Fixture_Screw_L_Back.Items.Count > 0)
                            {
                                if (cmbMount_Fixture_Screw_L_Back.Items.Contains(mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.L.ToString("#0.000")))
                                    cmbMount_Fixture_Screw_L_Back.Text = mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.L.ToString("#0.000");
                                else
                                    cmbMount_Fixture_Screw_L_Back.SelectedIndex = 0;

                                Double pThreadL = modMain.ConvTextToDouble(cmbMount_Fixture_Screw_L_Back.Text);
                            }
                            else
                            {
                                //....Check Change == false.
                                ChangeCheck_SealMountHole_Thread(chkMount_Fixture_Screw_LenLim_Back);
                            }

                        }
                    }



                    private void ChangeCheck_SealMountHole_Thread(CheckBox ChkBox_In )
                    //=================================================================
                    {
                        //....Caption & Message.
                        String pMsg = "For the selected Type, Material and Diameter no thread length" + System.Environment.NewLine +
                                     "is found in " + "\"" + "Screw" + "\"" +
                                     " table in the database that statisfies" + System.Environment.NewLine +
                                     "the given limit constraints. Hence limit can not be imposed.";

                        String pCaption = "Information";

                        //....Show message box.
                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);         

                        //....Checked = false.
                        ChkBox_In.Checked = false;
                    }


                    private void Update_Fixture_Thread_L(Int32 MountHole_Pos_In)          
                    //===========================================================
                    {
                        Populate_Mount_Fixture_Thread_L(MountHole_Pos_In);
                        Update_Display_Fixture_Thread_LLims(mBearing_Radial_FP,MountHole_Pos_In );
                    }


                    private void Update_Display_Fixture_Thread_LLims(clsBearing_Radial_FP Bearing_In ,Int32 MountHole_Pos_In)     
                    //=========================================================================================================
                    {
                        string pFormat = null;

                        if (MountHole_Pos_In == 0)
                        {
                            if (Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig != null)
                            {
                                if (Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig.Contains("M"))
                                    pFormat = "#0";
                                else
                                    pFormat = "#0.000";

                                //....Update Length Limits.
                                if (Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig != "" &&
                                    Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig != null)
                                {
                                    if (Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig.Contains("M"))
                                    {
                                        lblMount_Fixture_Screw_ULim_Front.Text = modMain.ConvDoubleToStr(
                                                                                 Math.Floor(Bearing_In.Mount.MountFixture_Screw_L_UpperLimit(0)), pFormat);
                                        lblMount_Fixture_Screw_LLim_Front.Text = modMain.ConvDoubleToStr(
                                                                                 Math.Ceiling(Bearing_In.Mount.MountFixture_Screw_L_LowerLimit(0)), pFormat);
                                    }
                                    else
                                    {
                                        lblMount_Fixture_Screw_ULim_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.MountFixture_Screw_L_UpperLimit(0), pFormat);
                                        lblMount_Fixture_Screw_LLim_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.MountFixture_Screw_L_LowerLimit(0), pFormat);     
                                    }
                                }

                                else
                                {
                                    lblMount_Fixture_Screw_ULim_Front.Text = "";
                                    lblMount_Fixture_Screw_LLim_Front.Text = "";
                                }
                            }
                        }

                        else if (MountHole_Pos_In == 1)
                        {
                            if (Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig != null)
                            {
                                if (Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig.Contains("M"))
                                    pFormat = "#0";
                                else
                                    pFormat = "#0.000";

                                //....Update Length Limits.
                                if (Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig != "" &&
                                    Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig != null)
                                {
                                    if (Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig.Contains("M"))
                                    {
                                        lblMount_Fixture_Screw_ULim_Back.Text = modMain.ConvDoubleToStr(
                                                                                Math.Floor(Bearing_In.Mount.MountFixture_Screw_L_UpperLimit(1)), pFormat);
                                        lblMount_Fixture_Screw_LLim_Back.Text = modMain.ConvDoubleToStr(
                                                                                Math.Ceiling(Bearing_In.Mount.MountFixture_Screw_L_LowerLimit(1)), pFormat);
                                    }
                                    else
                                    {
                                        lblMount_Fixture_Screw_ULim_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.MountFixture_Screw_L_UpperLimit(1), pFormat);
                                        lblMount_Fixture_Screw_LLim_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.MountFixture_Screw_L_LowerLimit(1), pFormat);
                                    }
                                }

                                else
                                {
                                    lblMount_Fixture_Screw_ULim_Back.Text = "";
                                    lblMount_Fixture_Screw_LLim_Back.Text = "";
                                }
                            }

                        }
                    }
                 
                #endregion


                #region "UPDATE FIXTURE:"

                    private void Upadate_Fixture_Selection(clsBearing_Radial_FP Bearing_In,bool BlnSel_Fixture_In)
                    //============================================================================================      
                    {
                       
                        if (Bearing_In.Mount.Fixture_Candidates.Exists)
                        {
                            //....Candidates Exists.
                            if (chkMount_Holes_Bolting_Front.Checked)       //....FRONT
                            {
                                chkMount_Fixture_Candidates_Chosen_Front.Checked = Bearing_In.Mount.Fixture_Candidates_Chosen[0];
                                //chkMount_Fixture_Candidates_Chosen_Front.Enabled = true;
                                lblMsg_Mount_Fixture_Candidates_Front.Text = " Select candidates from the available fixtures.";
                            }

                            if (chkMount_Holes_Bolting_Back.Checked)         //....BACK
                            {
                                chkMount_Fixture_Candidates_Chosen_Back.Checked = Bearing_In.Mount.Fixture_Candidates_Chosen[1];
                                //chkMount_Fixture_Candidates_Chosen_Back.Enabled = true;
                                lblMsg_Mount_Fixture_Candidates_Back.Text = " Select candidates from the available fixtures.";
                            }
                        }
                        else if (!Bearing_In.Mount.Fixture_Candidates.Exists)
                        {
                            //....Candidates doesn't exists.
                            if (chkMount_Holes_Bolting_Front.Checked)       //....FRONT
                            {
                                chkMount_Fixture_Candidates_Chosen_Front.Checked = false;
                                //chkMount_Fixture_Candidates_Chosen_Front.Enabled = false;
                                lblMsg_Mount_Fixture_Candidates_Front.Text = " No suitable fixture found in DB.";
                            }

                            if (chkMount_Holes_Bolting_Back.Checked)         //....BACK
                            {
                                chkMount_Fixture_Candidates_Chosen_Back.Checked = false;
                                //chkMount_Fixture_Candidates_Chosen_Back.Enabled = false;
                                lblMsg_Mount_Fixture_Candidates_Back.Text = " No suitable fixture found in DB.";                            
                            }
                        }


                        //....if Candidate fixture is chosen. 
                        if (Bearing_In.Mount.Fixture_Candidates_Chosen[0])
                        {
                            //....Populate Fixtures in a Combo Box and set controls for 
                            //.....fixture population.
                            Populate_Fixture(Bearing_In,0);

                            //....Get Part# index From Candiate Fixture.
                            int pIndx = Get_Fixture_Indx(Bearing_In,Bearing_In.Mount.Fixture[0].PartNo) ;

                            if (Bearing_In.Mount.Fixture[0].PartNo != null)// ||Bearing_In.Mount.Fixture[0].PartNo != "")
                            {
                                cmbMount_Fixture_PartNo_Front.Text = Bearing_In.Mount.Fixture[0].PartNo;
                                if (pIndx != -1)
                                    Display_Candidate_Fixture_Details(pIndx, Bearing_In, 0);
                                else
                                {
                                    if (cmbMount_Fixture_PartNo_Front.Items.Count > 0)
                                        cmbMount_Fixture_PartNo_Front.SelectedIndex = 0;
                                }
                            }
                            
                        }
                        else if (BlnSel_Fixture_In)
                        {
                            SetControl_New_Fixture_Details(0);
                            Display_New_Fixture_Details(Bearing_In, "Front");
                        }
                        else if (!BlnSel_Fixture_In)                            
                        {
                            txtMount_Fixture_D_Finish_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.EndConfig_DO_Max(), "#0.000");     
                            txtMount_Fixture_D_Finish_Front.ForeColor = Color.Blue;
                        }


                        //....if Candidate fixture is chosen. 
                        if (Bearing_In.Mount.Fixture_Candidates_Chosen[1])
                        {
                            //....Populate Fixtures in a Combo Box and set controls for 
                            //.....fixture population.
                            Populate_Fixture(Bearing_In, 1);

                            //....Get Part# index From Candiate Fixture.
                            int pIndx = Get_Fixture_Indx(Bearing_In,Bearing_In.Mount.Fixture[1].PartNo);

                            if (Bearing_In.Mount.Fixture[1].PartNo != null )//||Bearing_In.Mount.Fixture[1].PartNo != "")
                            {
                                cmbMount_Fixture_PartNo_Back.Text = Bearing_In.Mount.Fixture[1].PartNo;
                                if (pIndx != -1)
                                    Display_Candidate_Fixture_Details(pIndx, Bearing_In,1);
                                else
                                {
                                    if (cmbMount_Fixture_PartNo_Back.Items.Count > 0)
                                        cmbMount_Fixture_PartNo_Back.SelectedIndex = 0;
                                }
                            }
                        }
                        else if (BlnSel_Fixture_In)
                        {
                            SetControl_New_Fixture_Details(1);
                            Display_New_Fixture_Details(Bearing_In, "Back");
                        }
                        else if (!BlnSel_Fixture_In)
                        {
                            txtMount_Fixture_D_Finish_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.EndConfig_DO_Max(), "#0.000");     
                            txtMount_Fixture_D_Finish_Back.ForeColor = Color.Blue;
                        }
                    }


                    private void Populate_Fixture(clsBearing_Radial_FP Bearing_In, Int32 MountHolePos_In)
                    //===================================================================================
                    {
                        switch (MountHolePos_In)
                        {
                            case 0:
                            //-----
                                
                                if (Bearing_In.Mount.Fixture_Candidates.Exists)
                                {
                                    //....Candidate Exists.
                                    chkMount_Fixture_Candidates_Chosen_Front.Checked = Bearing_In.Mount.Fixture_Candidates_Chosen[0];
                                    //chkMount_Fixture_Candidates_Chosen_Front.Enabled = true;
                                    lblMsg_Mount_Fixture_Candidates_Front.Text = " Select candidates from the available fixtures.";

                                    if (chkMount_Fixture_Candidates_Chosen_Front.Checked)
                                    {
                                        //....Set Control For Candidate fixture.
                                        SetControl_Candidate_Fixture_Details(MountHolePos_In);

                                        //....Populate Fixture ComboBox.
                                        cmbMount_Fixture_PartNo_Front.Items.Clear();

                                        for (int i = 0; i < Bearing_In.Mount.Fixture_Candidates.PartNo.Length; i++)
                                        {   
                                            if(Bearing_In.Mount.Fixture_Candidates.PartNo[i]!= "")
                                            cmbMount_Fixture_PartNo_Front.Items.Add(Bearing_In.Mount.Fixture_Candidates.PartNo[i]);
                                        }
                                    }
                                    else
                                    {
                                        //....Set Control for New fixture.
                                        SetControl_New_Fixture_Details(MountHolePos_In);
                                        Display_New_Fixture_Details(Bearing_In, "Front");
                                    }
                                }
                                else if (!Bearing_In.Mount.Fixture_Candidates.Exists)
                                {
                                    chkMount_Fixture_Candidates_Chosen_Front.Checked = false;
                                    //chkMount_Fixture_Candidates_Chosen_Front.Enabled = false;
                                    lblMsg_Mount_Fixture_Candidates_Front.Text = " No suitable fixture found in DB.";
                                    SetControl_New_Fixture_Details(MountHolePos_In);
                                }
                                break;


                            case 1:
                            //---------
                               
                                if (Bearing_In.Mount.Fixture_Candidates.Exists)
                                {
                                    //....Candidate Exists.
                                    chkMount_Fixture_Candidates_Chosen_Back.Checked = Bearing_In.Mount.Fixture_Candidates_Chosen[1];
                                    //chkMount_Fixture_Candidates_Chosen_Back.Enabled = true;
                                    lblMsg_Mount_Fixture_Candidates_Back.Text = " Select candidates from the available fixtures.";

                                    if (chkMount_Fixture_Candidates_Chosen_Back.Checked)
                                    {
                                        //....Set Control For Candidate fixture.
                                        SetControl_Candidate_Fixture_Details(MountHolePos_In);

                                        //....Populate SealFixture ComboBox.
                                        cmbMount_Fixture_PartNo_Back.Items.Clear();

                                        for (int i = 0; i < Bearing_In.Mount.Fixture_Candidates.PartNo.Length; i++)
                                        {
                                            if (Bearing_In.Mount.Fixture_Candidates.PartNo[i] != "")
                                            cmbMount_Fixture_PartNo_Back.Items.Add(Bearing_In.Mount.Fixture_Candidates.PartNo[i]);
                                        }
                                    }
                                    else
                                    {
                                        //....Set Control for New fixture.
                                        SetControl_New_Fixture_Details(MountHolePos_In);
                                        Display_New_Fixture_Details(Bearing_In, "Back");
                                    }

                                }
                                else if (!Bearing_In.Mount.Fixture_Candidates.Exists)
                                {
                                    chkMount_Fixture_Candidates_Chosen_Back.Checked = false;
                                    //chkMount_Fixture_Candidates_Chosen_Back.Enabled = false;
                                    lblMsg_Mount_Fixture_Candidates_Back.Text = " No suitable fixture found in DB.";
                                    SetControl_New_Fixture_Details(MountHolePos_In);
                                }
                                break;
                        }
                    }


                    private void SetControl_Candidate_Fixture_Details(Int32 MountHolePosition_In)
                    //============================================================================
                    {
                        int pCount = 0;

                        switch (MountHolePosition_In)
                        {
                            case 0:
                            //-----------
                                //....Part #.
                                lblSealSplitFixture_PartNo_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_PartNo_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //....DBC.
                                txtMount_Fixture_DBC_Front.ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                txtMount_Fixture_DBC_Front.BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                //txtMount_Fixture_DBC_Front.ForeColor = Color.Blue;

                                //....Seal_Do.
                                txtMount_Fixture_D_Finish_Front.ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                txtMount_Fixture_D_Finish_Front.BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                //txtMount_Fixture_D_Finish_Front.ForeColor = Color.Blue;

                                //....Count.
                                txtMount_Fixture_HolesCount_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_HolesCount_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //.....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                lblMount_Fixture_HolesEquiSpaced_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //....Other Angle.
                                pCount = modMain.ConvTextToInt(txtMount_Fixture_HolesCount_Front.Text);

                                //....Thread
                                txtMount_Fixture_Screw_D_Desig_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_Screw_D_Desig_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                txtMount_Fixture_Screw_Pitch_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_Screw_Pitch_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                break;

                            case 1:
                            //----------
                                //....Part #.
                                lblSealSplitFixture_PartNo_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_PartNo_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //....DBC.
                                txtMount_Fixture_DBC_Back.ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                txtMount_Fixture_DBC_Back.BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                //txtMount_Fixture_DBC_Back.ForeColor = Color.Blue;

                                //....Seal_Do.
                                txtMount_Fixture_D_Finish_Back.ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                txtMount_Fixture_D_Finish_Back.BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                //txtMount_Fixture_D_Finish_Back.ForeColor = Color.Blue;

                                //....Count.
                                txtMount_Fixture_HolesCount_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_HolesCount_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //.....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                lblMount_Fixture_HolesEquiSpaced_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //....Other Angle.
                                pCount = modMain.ConvTextToInt(txtMount_Fixture_HolesCount_Back.Text);

                                //....Thread
                                txtMount_Fixture_Screw_D_Desig_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_Screw_D_Desig_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                txtMount_Fixture_Screw_Pitch_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_Screw_Pitch_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                break;
                        }
                    }


                    private void SetControl_New_Fixture_Details(Int32 MountHolePos_In)
                    //======================================================================
                    {
                        String pUnit = null;
                        int pRecCountType = 0, pIndx = 0;

                        if (modMain.gProject.Unit.System.ToString() == "Metric")
                            pUnit = "M";
                        else
                            pUnit = "E";

                        String pWHERE = "WHERE fldUnit = '" + pUnit + "'";

                        switch (MountHolePos_In)
                        {
                            case 0:
                            //-----------
                                //....Part #.
                                lblSealSplitFixture_PartNo_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_PartNo_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //....DBC.
                                txtMount_Fixture_DBC_Front.ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                txtMount_Fixture_DBC_Front.BackColor = Color.White;
                                txtMount_Fixture_DBC_Front.ForeColor = Color.Black;

                                //....Seal_Do.
                                txtMount_Fixture_D_Finish_Front.ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                txtMount_Fixture_D_Finish_Front.BackColor = Color.White;
                                txtMount_Fixture_D_Finish_Front.ForeColor = Color.Black;

                                //....Count.
                                txtMount_Fixture_HolesCount_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_HolesCount_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //.....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                lblMount_Fixture_HolesEquiSpaced_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                lblMsg_MountFixture_EquiSpaced_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //  Thread
                                //  =======
                                //.....Populate ComboBox SplitLine DowelPin Type.
                                pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Front, "tblManf_Screw", "fldType", pWHERE, true);
                                pIndx = cmbMount_Fixture_Screw_Type_Front.Items.IndexOf("SHCS");
                                cmbMount_Fixture_Screw_Type_Front.SelectedIndex = pIndx;

                                txtMount_Fixture_Screw_D_Desig_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_Screw_D_Desig_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                txtMount_Fixture_Screw_Pitch_Front.Visible = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                cmbMount_Fixture_Screw_Pitch_Front.Visible = !chkMount_Fixture_Candidates_Chosen_Front.Checked;

                                //.....Populate ComboBox SplitLine Thread Mat.
                                pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Front, "tblManf_Screw", "fldMat", pWHERE, true);

                                //.....To Get the default index.
                                pIndx = cmbMount_Fixture_Screw_Mat_Front.Items.IndexOf("STL");
                                if (pRecCountType > 0) cmbMount_Fixture_Screw_Mat_Front.SelectedIndex = pIndx;

                                break;

                            case 1:
                            //----------
                                //....Part #.
                                lblSealSplitFixture_PartNo_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_PartNo_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //....DBC.
                                txtMount_Fixture_DBC_Back.ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                txtMount_Fixture_DBC_Back.BackColor = Color.White;
                                txtMount_Fixture_DBC_Back.ForeColor = Color.Black;

                                //....Seal_Do.
                                txtMount_Fixture_D_Finish_Back.ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                txtMount_Fixture_D_Finish_Back.BackColor = Color.White;
                                txtMount_Fixture_D_Finish_Back.ForeColor = Color.Black;

                                //....Count.
                                txtMount_Fixture_HolesCount_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_HolesCount_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //.....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                lblMount_Fixture_HolesEquiSpaced_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                lblMsg_MountFixture_EquiSpaced_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //  Thread
                                //  =======
                                //.....Populate ComboBox SplitLine DowelPin Type.
                                pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Back, "tblManf_Screw", "fldType", pWHERE, true);
                                pIndx = cmbMount_Fixture_Screw_Type_Back.Items.IndexOf("SHCS");
                                cmbMount_Fixture_Screw_Type_Back.SelectedIndex = pIndx;

                                txtMount_Fixture_Screw_D_Desig_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_Screw_D_Desig_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                txtMount_Fixture_Screw_Pitch_Back.Visible = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                cmbMount_Fixture_Screw_Pitch_Back.Visible = !chkMount_Fixture_Candidates_Chosen_Back.Checked;

                                //.....Populate ComboBox Thread Mat.
                                pRecCountType = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Back, "tblManf_Screw", "fldMat", pWHERE, true);

                                //.....To Get the default index.
                                pIndx = cmbMount_Fixture_Screw_Mat_Back.Items.IndexOf("STL");
                                if (pRecCountType > 0) cmbMount_Fixture_Screw_Mat_Back.SelectedIndex = pIndx;

                                break;
                        }
                    }


                    private void Reset_Fixture_Controls()
                    //===================================
                    {
                        //....Front
                            //....Part #.
                            if (!mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                cmbMount_Fixture_PartNo_Front.SelectedIndex = -1;

                            //....DBC
                            txtMount_Fixture_DBC_Front.Text = "";

                            //....SealDO.
                            txtMount_Fixture_D_Finish_Front.Text = "";

                            //....Wall Thick.
                            txtMount_Fixture_WallT_Front.Text = "";

                            //....Count.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                cmbMount_Fixture_HolesCount_Front.SelectedIndex = -1;
                            else
                                txtMount_Fixture_HolesCount_Front.Text = "";

                            for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Front.Length; i++)
                            {
                                //if(mBearing.SealMountFixture_Candidates_Chosen)
                                mTxtMount_Fixture_HolesAngOther_Front[i].Text = "";
                                mTxtMount_Fixture_HolesAngOther_Front[i].ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                            }

                            //....D_Desig.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                cmbMount_Fixture_Screw_D_Desig_Front.SelectedIndex = -1;
                            else
                                txtMount_Fixture_Screw_D_Desig_Front.Text = "";

                            //....Pitch.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                cmbMount_Fixture_Screw_Pitch_Front.SelectedIndex = -1;
                            else
                                txtMount_Fixture_Screw_Pitch_Front.Text = "";

                            //....L.
                            cmbMount_Fixture_Screw_L_Front.Items.Clear();
                            cmbMount_Fixture_Screw_L_Front.Text = "";

                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[0])
                                cmbMount_Fixture_Screw_Mat_Front.Items.Clear();
                            else
                            {
                                String pWHERE = "WHERE fldD_Desig = '" + mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig + "'";
                                modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Front, "tblManf_Screw", "fldMat", pWHERE, true);
                            }


                        //....Back
                            //....Part #.
                            if (!mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                cmbMount_Fixture_PartNo_Back.SelectedIndex = -1;

                            //....DBC
                            txtMount_Fixture_DBC_Back.Text = "";

                            //....SealDO.
                            txtMount_Fixture_D_Finish_Back.Text = "";

                            //....Wall Thick.
                            txtMount_Fixture_WallT_Back.Text = "";

                            //....Count.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                cmbMount_Fixture_HolesCount_Back.SelectedIndex = -1;
                            else
                                txtMount_Fixture_HolesCount_Back.Text = "";

                            for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Back.Length; i++)
                            {
                                //if(mBearing.SealMountFixture_Candidates_Chosen)
                                mTxtMount_Fixture_HolesAngOther_Back[i].Text = "";
                                mTxtMount_Fixture_HolesAngOther_Back[i].ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                            }

                            //....D_Desig.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                cmbMount_Fixture_Screw_D_Desig_Back.SelectedIndex = -1;
                            else
                                txtMount_Fixture_Screw_D_Desig_Back.Text = "";

                            //....Pitch.
                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                cmbMount_Fixture_Screw_Pitch_Back.SelectedIndex = -1;
                            else
                                txtMount_Fixture_Screw_Pitch_Back.Text = "";

                            //....L.
                            cmbMount_Fixture_Screw_L_Back.Items.Clear();
                            cmbMount_Fixture_Screw_L_Back.Text = "";

                            if (mBearing_Radial_FP.Mount.Fixture_Candidates_Chosen[1])
                                cmbMount_Fixture_Screw_Mat_Back.Items.Clear();
                            else
                            {
                                String pWHERE = "WHERE fldD_Desig = '" + mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig + "'";
                                modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Back, "tblManf_Screw", "fldMat", pWHERE, true);
                            }
                    }


                    private int Get_Fixture_Indx( clsBearing_Radial_FP Bearing_In,string PartNo_In)
                    //=============================================================================                 
                    {
                        int pIndx = -1;
                        int pCount = -1;
                        if (Bearing_In.Mount.Fixture_Candidates.Exists)
                        {
                            for (int i = 0; i < Bearing_In.Mount.Fixture_Candidates.PartNo.Length; i++)
                            {
                                if (Bearing_In.Mount.Fixture_Candidates.PartNo[i] != "")
                                    pCount++;
                                if (PartNo_In == Bearing_In.Mount.Fixture_Candidates.PartNo[i])
                                    pIndx = pCount;     //AM 04JAN13
                            }
                        }

                        return pIndx;
                    }


                    private void Display_Candidate_Fixture_Details(int Indx_In,clsBearing_Radial_FP Bearing_In, Int32 MountHole_Pos_In)
                    //==================================================================================================================
                    {

                        Double[] pAngStart = null; String pWHERE = "";

                        //  Seal Splitting Fixture
                        //  ======================

                        switch (MountHole_Pos_In)
                        {
                            case 0:
                            //-----------

                                //  DBC,D_Finish,Count & WallT:
                                //  --------------------------
                                txtMount_Fixture_DBC_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].DBC, "#0.000");
                                txtMount_Fixture_DBC_Front.ForeColor = Color.Magenta;
                                txtMount_Fixture_D_Finish_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].D_Finish, "#0.000");
                                txtMount_Fixture_D_Finish_Front.ForeColor = Color.Magenta;                                
                                txtMount_Fixture_WallT_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.TWall_BearingCB(MountHole_Pos_In), "#0.000");                                
                                txtMount_Fixture_HolesCount_Front.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesCount);
                                txtMount_Fixture_HolesCount_Front.ForeColor = Color.Magenta;

                                //  Angles:
                                //  -------

                                    //  Start Angle:
                                    //  ------------

                                    //....Seal Mount fixture AngStart.
                                    if (Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() != 0.0F &&
                                        Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() != Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart)
                                    {
                                        pAngStart = new Double[2];
                                        pAngStart[0] = Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart;
                                        pAngStart[1] = Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp();
                                    }                                   
                                    else if (Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() == 0.0F ||
                                             Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() == Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart)
                                    {
                                        pAngStart = new Double[1];
                                        pAngStart[0] = Bearing_In.Mount.Fixture[0].HolesAngStart;
                                    }


                                    //SwapVal(ref pAngStart);           BG 24AUG12

                                    //....Populate ComboBox.
                                    cmbMount_Fixture_HolesAngStart_Front.Items.Clear();
                                    for (int i = 0; i < pAngStart.Length; i++)
                                        cmbMount_Fixture_HolesAngStart_Front.Items.Add(pAngStart[i].ToString());

                                    if (!Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp_Chosen)
                                        cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart, "");
                                    else
                                        cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp(), "");
                                    cmbMount_Fixture_HolesAngStart_Front.ForeColor = Color.Magenta;

                                    //  Other Angle:
                                    //  ------------
                                    for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Front.Length; i++)
                                        mTxtMount_Fixture_HolesAngOther_Front[i].Visible = false;

                                    Display_Fixture_OtherAngles(Bearing_In, 0);


                                //  Thread Pitch & Dia:
                                //  -------------------

                                    //....Thread.                        
                                    txtMount_Fixture_Screw_D_Desig_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig;
                                    pWHERE = "WHERE fldD_Desig = '" + Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig + "'";
                                    int pRecCount = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Front, "tblManf_Screw", "fldType", pWHERE, true);

                                    if(pRecCount>0)
                                        if (Bearing_In.Mount.Fixture[0].Screw_Spec.Type != "")                                        
                                            cmbMount_Fixture_Screw_Type_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.Type;
                                        
                                        else
                                            cmbMount_Fixture_Screw_Type_Front.SelectedIndex = 0;                                      

                                    pWHERE = "WHERE fldD_Desig = '" + Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig + "' AND fldType = '" +
                                                                      Bearing_In.Mount.Fixture[0].Screw_Spec.Type + "'";    
                                    modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Front, "tblManf_Screw", "fldMat", pWHERE, true);

                                    cmbMount_Fixture_Screw_Mat_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.Mat;


                                    ArrayList pPitch_Array_Front, pPitchType_Array_Front;
                                    mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.GetPitch(txtMount_Fixture_Screw_D_Desig_Front.Text, out pPitch_Array_Front, out pPitchType_Array_Front);

                                    if (pPitch_Array_Front.Count > 0)
                                    {
                                        if (pPitch_Array_Front.Count > 1)
                                        {
                                            cmbMount_Fixture_Screw_Pitch_Front.Visible = true;
                                            cmbMount_Fixture_Screw_Pitch_Front.DataSource = pPitch_Array_Front;
                                        }
                                        else
                                        {
                                            txtMount_Fixture_Screw_Pitch_Front.Text = pPitch_Array_Front[0].ToString();
                                        }
                                    }

                                    cmbMount_Fixture_Screw_L_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[0].Screw_Spec.L, "#0.000");
                                
                                break;

                            case 1:
                            //---------

                                //  DBC,D_Finish,Count & WallT:
                                //  --------------------------
                                txtMount_Fixture_DBC_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].DBC, "#0.000");
                                txtMount_Fixture_DBC_Back.ForeColor = Color.Magenta;
                                txtMount_Fixture_D_Finish_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].D_Finish, "#0.000");
                                txtMount_Fixture_D_Finish_Back.ForeColor = Color.Magenta;
                                txtMount_Fixture_WallT_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.TWall_BearingCB(MountHole_Pos_In), "#0.000");
                                txtMount_Fixture_HolesCount_Back.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesCount);
                                txtMount_Fixture_HolesCount_Back.ForeColor = Color.Magenta;

                                //  Angles:
                                //  -------

                                //....Seal Mount fixture AngStart.
                                if (Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() != 0.0F &&
                                    Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() != Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart)
                                {
                                    pAngStart = new Double[2];
                                    pAngStart[0] = Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart;
                                    pAngStart[1] = Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp();
                                }
                                else if (Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() == 0.0F ||
                                         Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp() == Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart)
                                {
                                    pAngStart = new Double[1];
                                    pAngStart[0] = Bearing_In.Mount.Fixture[1].HolesAngStart;
                                }

                                //SwapVal(ref pAngStart); BG 24AUG12

                             
                                //....Populate ComboBox.
                                cmbMount_Fixture_HolesAngStart_Back.Items.Clear();
                                for (int i = 0; i < pAngStart.Length; i++)
                                    cmbMount_Fixture_HolesAngStart_Back.Items.Add(pAngStart[i].ToString());

                                if (!Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp_Chosen)
                                    cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart, "");
                                else
                                    cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[MountHole_Pos_In].HolesAngStart_Comp(), "");
                                cmbMount_Fixture_HolesAngStart_Back.ForeColor = Color.Magenta;

                                //  Other Angle:
                                //  ------------
                                for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Back.Length; i++)
                                    mTxtMount_Fixture_HolesAngOther_Back[i].Visible = false;

                                Display_Fixture_OtherAngles(Bearing_In, 1);


                            //  Thread Pitch & Dia:
                            //  -------------------

                                //....Thread.                        
                                txtMount_Fixture_Screw_D_Desig_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig;
                                pWHERE = "WHERE fldD_Desig = '" + Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig + "'";
                                pRecCount = modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Type_Back, "tblManf_Screw", "fldType", pWHERE, true);


                                if (pRecCount > 0)
                                    if (Bearing_In.Mount.Fixture[1].Screw_Spec.Type != "")
                                        cmbMount_Fixture_Screw_Type_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.Type;

                                    else
                                        cmbMount_Fixture_Screw_Type_Front.SelectedIndex = 0;              

                                pWHERE = "WHERE fldD_Desig = '" + Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig + "' AND fldType = '" +
                                                                    Bearing_In.Mount.Fixture[1].Screw_Spec.Type + "'";    
                                modMain.gDB.PopulateCmbBox(cmbMount_Fixture_Screw_Mat_Back, "tblManf_Screw", "fldMat", pWHERE, true);

                                cmbMount_Fixture_Screw_Mat_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.Mat;

                                ArrayList pPitch_Array_Back, pPitchType_Array_Back;
                                mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.GetPitch(txtMount_Fixture_Screw_D_Desig_Back.Text, out pPitch_Array_Back, out pPitchType_Array_Back);

                                if (pPitch_Array_Back.Count > 0)
                                {
                                    if (pPitch_Array_Back.Count > 1)
                                    {
                                        cmbMount_Fixture_Screw_Pitch_Back.Visible = true;
                                        cmbMount_Fixture_Screw_Pitch_Back.DataSource = pPitch_Array_Back;
                                    }
                                    else
                                    {
                                        txtMount_Fixture_Screw_Pitch_Back.Text = pPitch_Array_Back[0].ToString();
                                    }

                                }
                                cmbMount_Fixture_Screw_L_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[1].Screw_Spec.L, "#0.000");
                                                                      
                                break;
                        }
                    }


                    private void Display_New_Fixture_Details(clsBearing_Radial_FP Bearing_In, string MountHole_Pos_In)
                    //================================================================================================
                    {
                        switch (MountHole_Pos_In)
                        {
                            case "Front":
                            //----------
                                //.....DBC.
                                txtMount_Fixture_DBC_Front.Text =
                                    modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.Calc_DBC_Ballpark(), "#0.000");
                                txtMount_Fixture_DBC_Front.ForeColor = Color.Blue;

                                //....Seal DO.
                                txtMount_Fixture_D_Finish_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.EndConfig_DO_Max(), "#0.000");
                                txtMount_Fixture_D_Finish_Front.ForeColor = Color.Blue;

                                //....Wall thick
                                txtMount_Fixture_WallT_Front.Text = modMain.ConvDoubleToStr((Double)Bearing_In.Mount.TWall_BearingCB(0), "#0.000");

                                //....Count.
                                //txtMount_Fixture_HolesCount_Front.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[0].HolesCount);
                                cmbMount_Fixture_HolesCount_Front.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[0].HolesCount);

                                //....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Front.Checked = Bearing_In.Mount.Fixture[0].HolesEquiSpaced;

                                //....Angle Start.
                                cmbMount_Fixture_HolesAngStart_Front.Items.Clear();
                                cmbMount_Fixture_HolesAngStart_Front.Items.Add("30");
                                cmbMount_Fixture_HolesAngStart_Front.Items.Add("60");
                                if(cmbMount_Fixture_HolesAngStart_Front.Items.Contains(Bearing_In.Mount.Fixture[0].HolesAngStart))                                
                                    cmbMount_Fixture_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[0].HolesAngStart, "#0");
                                else
                                     cmbMount_Fixture_HolesAngStart_Front.SelectedIndex =0;
                                cmbMount_Fixture_HolesAngStart_Front.ForeColor = Color.Black;

                                //....Angle other.
                                Display_Fixture_OtherAngles(Bearing_In, 0);

                                //....Thread.
                                cmbMount_Fixture_Screw_Type_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.Type;
                                cmbMount_Fixture_Screw_Mat_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.Mat;
                                Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Front,
                                                        Bearing_In.Mount.Fixture[0].Screw_Spec.Type,
                                                        Bearing_In.Mount.Fixture[0].Screw_Spec.Mat,
                                                        mBearing_Radial_FP.Unit.System,
                                                        mBearing_Radial_FP.Mount.Fixture[0].Screw_Spec.D_Desig);

                                cmbMount_Fixture_Screw_D_Desig_Front.Text = Bearing_In.Mount.Fixture[0].Screw_Spec.D_Desig;
                                cmbMount_Fixture_Screw_L_Front.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[0].Screw_Spec.L, "#0.000");

                                break;

                            case "Back":
                            //----------
                                //.....DBC.
                                txtMount_Fixture_DBC_Back.Text =
                                    modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.Calc_DBC_Ballpark(), "#0.000");
                                txtMount_Fixture_DBC_Back.ForeColor = Color.Blue;

                                //....Seal DO.
                                txtMount_Fixture_D_Finish_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture_Candidates.EndConfig_DO_Max(), "#0.000");
                                txtMount_Fixture_D_Finish_Back.ForeColor = Color.Blue;

                                //....Wall thick
                                txtMount_Fixture_WallT_Back.Text = modMain.ConvDoubleToStr((Double)Bearing_In.Mount.TWall_BearingCB(1), "#0.000");

                                //....Count.
                                //txtMount_Fixture_HolesCount_Back.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[1].HolesCount);
                                cmbMount_Fixture_HolesCount_Back.Text = modMain.ConvIntToStr(Bearing_In.Mount.Fixture[1].HolesCount);

                                //....EquiSpaced.
                                chkMount_Fixture_HolesEquiSpaced_Back.Checked = Bearing_In.Mount.Fixture[1].HolesEquiSpaced;

                                //....Angle Start.
                                cmbMount_Fixture_HolesAngStart_Back.Items.Clear();
                                cmbMount_Fixture_HolesAngStart_Back.Items.Add("30");
                                cmbMount_Fixture_HolesAngStart_Back.Items.Add("60");

                                if (cmbMount_Fixture_HolesAngStart_Back.Items.Contains(Bearing_In.Mount.Fixture[1].HolesAngStart))
                                    cmbMount_Fixture_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[1].HolesAngStart, "#0");
                                else
                                    cmbMount_Fixture_HolesAngStart_Back.SelectedIndex = 0;
                                cmbMount_Fixture_HolesAngStart_Back.ForeColor = Color.Black;

                                //....Angle other.
                                Display_Fixture_OtherAngles(Bearing_In, 1);

                                //....Thread.
                                cmbMount_Fixture_Screw_Type_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.Type;
                                cmbMount_Fixture_Screw_Mat_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.Mat;
                                Populate_Screw_D_Desig(ref cmbMount_Fixture_Screw_D_Desig_Back,
                                                        Bearing_In.Mount.Fixture[1].Screw_Spec.Type,
                                                        Bearing_In.Mount.Fixture[1].Screw_Spec.Mat,
                                                        mBearing_Radial_FP.Unit.System,
                                                        mBearing_Radial_FP.Mount.Fixture[1].Screw_Spec.D_Desig);

                                cmbMount_Fixture_Screw_D_Desig_Back.Text = Bearing_In.Mount.Fixture[1].Screw_Spec.D_Desig;
                                cmbMount_Fixture_Screw_L_Back.Text = modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[1].Screw_Spec.L, "#0.000");

                                break;
                        }
                    }


                    private void Display_Fixture_OtherAngles(clsBearing_Radial_FP Bearing_In, Int32 MountHoles_Pos_In)
                    //=================================================================================================
                    {
                        //....If Holes are not equispaced then retrieve data from database.
                        //....Show other angle.
                        switch (MountHoles_Pos_In)
                        {
                            case 0:
                                //-------
                                for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Front.Length; i++)
                                {
                                    mTxtMount_Fixture_HolesAngOther_Front[i].Visible = false;
                                }

                                if (!Bearing_In.Mount.Fixture[0].HolesEquiSpaced)
                                {
                                    for (int i = 0; i < Bearing_In.Mount.Fixture[0].HolesCount - 1; i++)
                                    {
                                        //....Set Controls for Angle Other.
                                        mTxtMount_Fixture_HolesAngOther_Front[i].ReadOnly = chkMount_Fixture_Candidates_Chosen_Front.Checked;
                                        if (Bearing_In.Mount.Fixture_Candidates_Chosen[0])
                                        {
                                            mTxtMount_Fixture_HolesAngOther_Front[i].BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                            mTxtMount_Fixture_HolesAngOther_Front[i].ForeColor = Color.Magenta;
                                        }
                                        else
                                        {
                                            mTxtMount_Fixture_HolesAngOther_Front[i].BackColor = Color.White;
                                            mTxtMount_Fixture_HolesAngOther_Front[i].ForeColor = Color.Black;
                                        }
                                        mTxtMount_Fixture_HolesAngOther_Front[i].Visible = true;

                                        //....Set Text Values.
                                        mTxtMount_Fixture_HolesAngOther_Front[i].Text =
                                            modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[0].HolesAngOther[i], "#0");
                                    }
                                }
                                else
                                    for (int i = 0; i < Bearing_In.Mount.Fixture[0].HolesCount - 1; i++)
                                    {
                                        //....Set Controls for Angle Other.
                                        mTxtMount_Fixture_HolesAngOther_Front[i].Visible = true;
                                        mTxtMount_Fixture_HolesAngOther_Front[i].ReadOnly = true;
                                        mTxtMount_Fixture_HolesAngOther_Front[i].BackColor = txtMount_Fixture_WallT_Front.BackColor;
                                        mTxtMount_Fixture_HolesAngOther_Front[i].ForeColor = Color.Blue;

                                        //....Set Text Values.
                                        Double pOtherAngle;
                                        pOtherAngle = Bearing_In.Mount.MountFixture_Sel_AngOther(0);
                                        mTxtMount_Fixture_HolesAngOther_Front[i].Text = modMain.ConvDoubleToStr(pOtherAngle, "#0");
                                    }
                                
                                break;

                            case 1:
                                //-----------
                                for (int i = 0; i < mTxtMount_Fixture_HolesAngOther_Back.Length; i++)
                                {
                                    mTxtMount_Fixture_HolesAngOther_Back[i].Visible = false;
                                }

                                if (!Bearing_In.Mount.Fixture[1].HolesEquiSpaced)
                                {
                                    for (int i = 0; i < Bearing_In.Mount.Fixture[1].HolesCount - 1; i++)
                                    {
                                        //....Set Controls for Angle Other.
                                        mTxtMount_Fixture_HolesAngOther_Back[i].ReadOnly = chkMount_Fixture_Candidates_Chosen_Back.Checked;
                                        if (Bearing_In.Mount.Fixture_Candidates_Chosen[1])
                                        {
                                            mTxtMount_Fixture_HolesAngOther_Back[i].BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                            mTxtMount_Fixture_HolesAngOther_Back[i].ForeColor = Color.Magenta;
                                        }
                                        else
                                        {
                                            mTxtMount_Fixture_HolesAngOther_Back[i].BackColor = Color.White;
                                            mTxtMount_Fixture_HolesAngOther_Back[i].ForeColor = Color.Black;
                                        }
                                        mTxtMount_Fixture_HolesAngOther_Back[i].Visible = true;

                                        //....Set Text Values.
                                        mTxtMount_Fixture_HolesAngOther_Back[i].Text =
                                            modMain.ConvDoubleToStr(Bearing_In.Mount.Fixture[1].HolesAngOther[i], "#0");
                                    }
                                }
                                else
                                    for (int i = 0; i < Bearing_In.Mount.Fixture[1].HolesCount - 1; i++)
                                    {
                                        //....Set Controls for Angle Other.
                                        mTxtMount_Fixture_HolesAngOther_Back[i].Visible = true;
                                        mTxtMount_Fixture_HolesAngOther_Back[i].ReadOnly = true;
                                        mTxtMount_Fixture_HolesAngOther_Back[i].BackColor = txtMount_Fixture_WallT_Back.BackColor;
                                        mTxtMount_Fixture_HolesAngOther_Back[i].ForeColor = Color.Blue;

                                        //....Set Text Values.
                                        Double pOtherAngle;
                                        pOtherAngle = Bearing_In.Mount.MountFixture_Sel_AngOther(1);
                                        mTxtMount_Fixture_HolesAngOther_Back[i].Text = modMain.ConvDoubleToStr(pOtherAngle, "#0");
                                    }

                                break;
                        }
                    }  


                    private void SwapVal(ref Double[] Sng_In)
                    //=======================================
                    {
                        if (Sng_In.Length == 2)
                        {
                            if (Sng_In[0] > Sng_In[1])
                            {
                                Double pAny;
                                pAny = Sng_In[0];
                                Sng_In[0] = Sng_In[1];
                                Sng_In[1] = pAny;
                            }
                        }
                    }

                #endregion

            #endregion


            #region "TEMP SENSOR:"

                private void SetControl_TempSensor()
                //==================================
                {
                    lblTempSensor_CanLength.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_CanLength.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Count.Visible = chkTempSensor_Exists.Checked;
                    cmbTempSensor_Count.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Loc.Visible = chkTempSensor_Exists.Checked;
                    cmbTempSensor_Loc.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_D.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_D.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Depth.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_Depth.Visible = chkTempSensor_Exists.Checked;  

                    lblTempSensor_Angles.Visible = chkTempSensor_Exists.Checked;
                    lblTempSensor_txt.Visible = chkTempSensor_Exists.Checked;   

                    lblTempSensor_Ang_Start.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_AngStart.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_AngBet.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_AngBet.Visible = chkTempSensor_Exists.Checked;
                }

            #endregion


            #region "POPULATE PIN & THREAD:"

                private void Populate_Pin_D_Desig(ref ComboBox CmbD_In,String Pin_Type_In,
                                                  String Pin_Mat_In, clsUnit.eSystem Unit_In, string D_Desig_In )
                //===============================================================================================
                {
                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";
                                      
                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";
                    
                    string pPin_Type = Pin_Type_In;

                    if (Pin_Mat_In == "")
                        return;

                    string pstrWHERE = " WHERE fldType  = '" + pPin_Type +
                                     "' AND fldUnit = '" + pUnitSystem +
                                     "' AND fldMat = '" + Pin_Mat_In + "'";

                    if (pUnitSystem == "E")
                    {
                        //....Populate Dia Desig.
                        StringCollection pDia_Desig = new StringCollection();
                        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Pin",
                                                      "fldD_Desig", pstrWHERE, true);

                        //....Initialize String Collection.
                        StringCollection pDia_DwHash = new StringCollection();  //....Dia_Desig with # symbol.
                        StringCollection pDia_DwoHash = new StringCollection(); //....Dia_Desig without # symbol. 

                        Double pNumerator, pDenominator;
                        String pFinal;

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("#"))
                            {
                                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                            }
                            else
                            {
                                if (pDia_Desig[i].ToString() != "1")
                                {
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                                    pDia_DwoHash.Add(pFinal);
                                }
                                else
                                    pDia_DwoHash.Add(pDia_Desig[i]);
                            }
                        }

                        //....Sort Dia_Desig with # symbol.
                        //SortNumberwHash(ref pDia_DwHash);
                        modMain.SortNumberwHash(ref pDia_DwHash);

                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_DwoHash, true);
                        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                        }

                        //....Clear Combo Box Split Line Hardware Thread Dia_desig.
                        CmbD_In.Items.Clear();

                        //....Populate Combo Box Split Line Hardware Thread Dia_Desig.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwHash[i]);
                        }

                        for (int i = 0; i < pDia_DwoHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwoHash[i]);
                        }
                    }
                    else if (pUnitSystem == "M")
                    {
                        //....Populate Dia Desig.
                        StringCollection pDia_Desig = new StringCollection();
                        modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Pin",
                                                      "fldD_Desig", pstrWHERE, true);

                        //....Initialize String Collection.
                        StringCollection pDia_D = new StringCollection();  //....Dia_Desig with # symbol.

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("M"))
                            {
                                pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                            }
                        }

                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_D, false);
                        modMain.SortNumberwoHash(ref pDia_D, false);

                        CmbD_In.Items.Clear();

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            pDia_D[i] = "M" + pDia_D[i];
                        }

                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_D[i]);
                        }
                    }
                                       
                    if (CmbD_In.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(D_Desig_In) && CmbD_In.Items.Contains(D_Desig_In))
                            CmbD_In.Text = D_Desig_In;
                        else
                            CmbD_In.SelectedIndex = 0;
                    }
                }

               
                private void Populate_Pin_Mat(ref ComboBox CmbMat_In, String Pin_Type_In, clsUnit.eSystem Unit_In)
                //================================================================================================
                {
                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)      //BG 26MAR12
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

               
                    string pstrWHERE = " WHERE fldType  = '" + Pin_Type_In +
                                       "' AND fldUnit = '" + pUnitSystem + "'";

                    modMain.gDB.PopulateCmbBox(CmbMat_In, "tblManf_Pin", "fldMat", pstrWHERE, true);

                    if (CmbMat_In.Items.Count > 0)     
                    {
                        int pIndx = -1;                        

                        if (CmbMat_In.Items.Contains("STL"))
                        {
                            pIndx = CmbMat_In.Items.IndexOf("STL");

                            if (pIndx != -1)
                                CmbMat_In.SelectedIndex = pIndx;
                            else
                                CmbMat_In.SelectedIndex = 0;
                        }
                    }
                }


                private void Populate_Screw_D_Desig(ref ComboBox CmbD_In, String Screw_Type_In,
                                                     String Screw_Mat_In, clsUnit.eSystem Unit_In, string D_Desig_In)
                //===================================================================================================
                {
                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";
                  
                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";
                                      
                    if (Screw_Mat_In == "")
                        return;

                    string pstrWHERE = " WHERE fldType  = '" + Screw_Type_In +
                                       "' AND fldUnit = '" + pUnitSystem + "'" +
                                       " AND fldMat = '" + Screw_Mat_In + "'";

                    //....Populate Dia Desig.
                    StringCollection pDia_Desig = new StringCollection();
                    modMain.gDB.PopulateStringCol(pDia_Desig, "tblManf_Screw", "fldD_Desig", pstrWHERE, true);

                    if (pUnitSystem == "E")
                    {
                        //....Initialize String Collection.
                        StringCollection pDia_DwHash = new StringCollection();  //....Dia_Desig with # symbol.
                        StringCollection pDia_DwoHash = new StringCollection(); //....Dia_Desig without # symbol. 

                        Double pNumerator, pDenominator;
                        String pFinal;

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("#"))
                            {
                                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                            }
                            else if (pDia_Desig[i].Contains("/"))
                            {
                                if (pDia_Desig[i].ToString() != "1")
                                {
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                                    pDia_DwoHash.Add(pFinal);
                                }
                                else
                                    pDia_DwoHash.Add(pDia_Desig[i]);
                            }
                        }

                        //....Sort Dia_Desig with # symbol.
                        //SortNumberwHash(ref pDia_DwHash);
                        modMain.SortNumberwHash(ref pDia_DwHash);

                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_DwoHash, true);
                        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                        }

                        //....Clear Combo Box Split Line Hardware Thread Dia_desig.
                        CmbD_In.Items.Clear();

                        //....Populate Combo Box Split Line Hardware Thread Dia_Desig.

                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwHash[i]);
                        }

                        for (int i = 0; i < pDia_DwoHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwoHash[i]);
                        }
                    }
                    else if (pUnitSystem == "M")
                    {
                        //....Initialize String Collection.
                        StringCollection pDia_D = new StringCollection();

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("M"))
                            {
                                {
                                    pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                                }
                            }

                        }
                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_D, false);
                        modMain.SortNumberwoHash(ref pDia_D, false);

                        CmbD_In.Items.Clear();

                        //....Concatinate M symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            pDia_D[i] = "M" + pDia_D[i];
                        }

                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_D[i]);
                        }
                    }

                    if (CmbD_In.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(D_Desig_In) && CmbD_In.Items.Contains(D_Desig_In))
                            CmbD_In.Text = D_Desig_In;
                        else
                            CmbD_In.SelectedIndex = 0;
                    }
                }


                private void Populate_Screw_Mat(ref ComboBox CmbMat_In, String Thread_Type_In, clsUnit.eSystem Unit_In)
                //=====================================================================================================
                {
                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";
                                     

                    string pstrWHERE = " WHERE fldType  = '" + Thread_Type_In +
                                       "' AND fldUnit = '" + pUnitSystem + "'";

                    modMain.gDB.PopulateCmbBox(CmbMat_In, "tblManf_Screw", "fldMat", pstrWHERE, true);

                    if (CmbMat_In.Items.Count > 0)     
                    {
                        int pIndx = -1;

                        if (CmbMat_In.Items.Contains("STL"))
                            pIndx = CmbMat_In.Items.IndexOf("STL");
                        if (pIndx != -1)
                            CmbMat_In.SelectedIndex = pIndx;
                        else
                            CmbMat_In.SelectedIndex = 0;
                    }
                }

               
            #endregion

        #endregion

              
        #region "VALIDATION ROUTINES:"

            private bool ValidateOilInlet_Annulus_D()
            //=======================================
            {
                Double pOilInlet_Annulus_D = 0.0F;          //BG 28JUN11
                pOilInlet_Annulus_D = modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text);

                clsBearing_Radial_FP pTempBearing; 
                pTempBearing =(clsBearing_Radial_FP) mBearing_Radial_FP.Clone();
                pTempBearing.OilInlet.Annulus_Ratio_L_H = mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H;
                              
                //BG 21JUL11
                pTempBearing.OilInlet.Annulus_D = modMain.ConvTextToDouble(
                                                 modMain.ConvDoubleToStr(pTempBearing.OilInlet.Annulus.D, modMain.gUnit.MFormat));

                if (pOilInlet_Annulus_D <= pTempBearing.OilInlet.Annulus.D)               
                {
                    return true;
                }
                else
                {
                    String pMSg = " Please enter value Less than : " + modMain.ConvDoubleToStr(pTempBearing.OilInlet.Annulus.D, modMain.gUnit.MFormat);      

                    String pCaption = "OilInlet Annulus Dia Error";
                    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //....OilInlet Tab Index.
                    tbBearingDesignDetails.SelectedIndex = 0;   //SB 03AUG09

                    txtOilInlet_Annulus_D.Focus();
                    return false;
                }
                    
            }


            private bool ValidateOilInlet_Annulus_L()
            //=======================================
            {
                //BG 28JUN11
                Double pOilInlet_Annulus_L = 0.0F;
                pOilInlet_Annulus_L = modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text);

                clsBearing_Radial_FP pTempBearing;
                pTempBearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();
                pTempBearing.OilInlet.Annulus_Ratio_L_H = mBearing_Radial_FP.OilInlet.Annulus.Ratio_L_H;               
                pTempBearing.OilInlet.Annulus_L = modMain.ConvTextToDouble(
                                                 modMain.ConvDoubleToStr(pTempBearing.OilInlet.Annulus.L, modMain.gUnit.MFormat));

                if (pOilInlet_Annulus_L >= pTempBearing.OilInlet.Annulus.L)
                {
                    return true;
                }
                else
                {
                    String pMSg = " Please enter value greater than : " + modMain.ConvDoubleToStr(pTempBearing.OilInlet.Annulus.L, modMain.gUnit.MFormat);       

                    String pCaption = "OilInlet Annulus Length Error";
                    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //....OilInlet Tab Index.
                    tbBearingDesignDetails.SelectedIndex = 0;   //SB 03AUG09

                    txtOilInlet_Annulus_L.Focus();
                    return false;
                }
            }


            private bool ValidateAng_AntiRotPin()   //SB 03AUG09
            //===================================
            {
                if (mBearing_Radial_FP.AntiRotPin.Loc_Angle >= 90)
                {
                    String pMSg = " Only acute pin angle is allowed in this version.";

                    String pCaption = "Anti Rotation Pin Angle Error";
                    MessageBox.Show(pMSg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //....OilInlet Tab Index.
                    tbBearingDesignDetails.SelectedIndex = 3;   

                    txtAntiRotPin_Loc_Angle.Focus();
                    return false;
                }

                return true;
            }

            //....Not used Now
            private bool Validate_Seal_Mount_Fixture_PartNo()   //SB 03AUG09
            //===============================================
            {
                if (mBearing_Radial_FP.Mount.Fixture[0].PartNo != "" &&
                    mBearing_Radial_FP.Mount.Fixture[0].PartNo != null)
                {
                    if (!cmbMount_Fixture_PartNo_Back.Items.Contains(mBearing_Radial_FP.Mount.Fixture[0].PartNo))
                    {
                        String pMSg = "Since you have changed data in Bearing Design Details it changed " 
                                      + System.Environment.NewLine +
                                      "your selected fixture part # from: '" + mBearing_Radial_FP.Mount.Fixture[0].PartNo + 
                                      "' to: '" + mBearing_Radial_FP.Mount.Fixture[0].PartNo + "'."
                                      + System.Environment.NewLine +
                                      System.Environment.NewLine +
                                      "Click 'Yes' to save the changed fixture part #."
                                      + System.Environment.NewLine +
                                      "Click 'No' to change fixture part # as your desire."
                                      + System.Environment.NewLine +
                                      System.Environment.NewLine +
                                      "Do you want to proceed?";

                        String pCaption = "Inforamtion";
                        int pAns = (int)MessageBox.Show(pMSg, pCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        //....Seal Mount Hole Tab Index.
                        if (pAns == 6)
                        {
                            return true;
                        }
                        else
                        {
                            tbBearingDesignDetails.SelectedIndex = 4;
                            cmbMount_Fixture_PartNo_Back.Focus();
                            //mblnCheck_Fixture_PartNo = true;
                            return false;
                        }
                    }
                }

                return true;
            }

        #endregion
            
      
    }
}
