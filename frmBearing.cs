
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmBearing                             '
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
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Collections.Specialized;

namespace BearingCAD21
{
   public partial class frmBearing : Form
   {
       #region "MEMBER VARIABLE DECLARATION:"
       //************************************

            private TextBox[] mtxtDFit_Range;           
            private TextBox[] mtxtDSet_Range;         
            private TextBox[] mtxtDShaft_Range;       
            private TextBox[] mtxtDPad_Range;
            private TextBox[] mtxtPivot_Loc;
            //public TextBox[] mtxtEDM_Relief;          //....Moved to BearingDesignDetails form.

            //....Local Objects:    
            private clsProduct mProduct;
            private clsBearing_Radial_FP mBearing_Radial_FP ;           //PB 13AUG12

            private Boolean mblnDShaft_ManuallyChanged = false;     
            private Boolean mblnDSet_ManuallyChanged   = false;
            private Boolean mblnPad_T_Pivot_ManuallyChanged = false;
            private Boolean mblnWeb_T_ManuallyChanged  = false;

       #endregion


       #region "FORM CONSTRUCTOR & RELATED ROUTINES:"
       //********************************************

            public frmBearing()
            //=================
            {
                InitializeComponent();               
           
                //....Initialize TextBoxes.
                mtxtDFit_Range   = new TextBox[] { txtDFit_Range_Min, txtDFit_Range_Max };
                mtxtDSet_Range   = new TextBox[] { txtDSet_Range_Min, txtDSet_Range_Max };
                mtxtDShaft_Range = new TextBox[] { txtDShaft_Range_Min, txtDShaft_Range_Max };
                mtxtDPad_Range   = new TextBox[] { txtDPad_Range_Min, txtDPad_Range_Max };
                mtxtPivot_Loc    = new TextBox[] { txtPad_Pivot_AngStart, txtPad_Pivot_AngOther2, 
                                                   txtPad_Pivot_AngOther3, txtPad_Pivot_AngOther4, 
                                                   txtPad_Pivot_AngOther5, txtPad_Pivot_AngOther6 };

                //....Populate Split Configuration.
                cmbSplitConfig.Items.Clear();
                cmbSplitConfig.Items.Add("Y");
                cmbSplitConfig.Items.Add("N");
                cmbSplitConfig.SelectedIndex = 0;

                //....Populate Pad Type.
                LoadPadType();

                ////....Populate EDM Gap        
                //LoadEDMGap();
                
                //....Populate Base & Lining  Material.
                LoadMat();
            }


            private void LoadPadType()
            //========================      
            {
                cmbPad_Type.DataSource = Enum.GetValues(typeof(clsBearing_Radial_FP.clsPad.eLoadPos));
                cmbPad_Type.SelectedIndex = 0;  
            }


            private void LoadEDMGap()
            //======================
            {
                
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                StringCollection pEDMGap = new StringCollection();

                var pQry = (from pRec in pBearingDBEntities.tblManf_EDM orderby pRec.fldGap ascending select pRec).Distinct().ToList();

                if (pQry.Count() > 0)
                {
                    for (int i = 0; i < pQry.Count; i++)
                    {
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            double pVal = (double)pQry[i].fldGap;
                            pEDMGap.Add(modMain.gProject.Unit.CEng_Met(pVal).ToString());
                        }
                        else
                        {
                            pEDMGap.Add(pQry[i].fldGap.ToString());
                        }
                        
                    }
                }

                cmbFlexPivot_GapEDM.Items.Clear();

                for (int i = 0; i < pEDMGap.Count; i++)
                {
                    Double pVal = Convert.ToDouble(pEDMGap[i]);
                    cmbFlexPivot_GapEDM.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.000"));
                }

                if (cmbFlexPivot_GapEDM.Items.Count > 0)
                    cmbFlexPivot_GapEDM.SelectedIndex = 3;
            }


            private void LoadMat()
            //=====================    
            {
                BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                //....Base Material.
                var pQryBaseMat = (from pRec in pBearingDBEntities.tblData_Mat
                                   where
                                       pRec.fldBase == true
                                   orderby pRec.fldName ascending
                                   select pRec).ToList();
                cmbMat_Base.Items.Clear();
                if (pQryBaseMat.Count() > 0)
                {
                    for (int i = 0; i < pQryBaseMat.Count; i++)
                    {
                        cmbMat_Base.Items.Add(pQryBaseMat[i].fldName);
                    }
                    cmbMat_Base.SelectedIndex = 3;
                }

                //....Lining Material.
                var pQryLiningMat = (from pRec in pBearingDBEntities.tblData_Mat
                                     where
                                         pRec.fldLining == true
                                     orderby pRec.fldName ascending
                                     select pRec).ToList();
                cmbMat_Lining.Items.Clear();
                if (pQryBaseMat.Count() > 0)
                {
                    for (int i = 0; i < pQryLiningMat.Count; i++)
                    {
                        cmbMat_Lining.Items.Add(pQryLiningMat[i].fldName);
                    }
                    cmbMat_Lining.SelectedIndex = 0;
                }            
            }

       #endregion


       #region "FORM EVENT ROUTINES:"
       //***************************

            private void frmBearing_Load(object sender, EventArgs e)
            //======================================================
            {
                mblnDShaft_ManuallyChanged = false;
                mblnDSet_ManuallyChanged = false;

                //....Initialize control.
                InitializeControls();

                //....Set Locl Object.
                SetLocalObject();

                //....Populate EDM Gap        
                LoadEDMGap();

                //....Display data.
                DisplayData();              

                //....Set Control for diff privilege & Project status.
                SetControls();                                    
            }


            private void InitializeControls()
            //===============================
            {
                const string pcBlank = "";

                //....DShaft,DFit,DSet,DPad
                for (int i = 0; i < 2; i++)
                {
                    mtxtDShaft_Range[i].Text = pcBlank;
                    mtxtDFit_Range[i].Text = pcBlank;
                    mtxtDSet_Range[i].Text = pcBlank;
                    mtxtDPad_Range[i].Text = pcBlank;
                }

                //....Clearence & PreLoad
                txtClearance.Text = pcBlank;
                txtPreLoad.Text = pcBlank;

                //  Pad
                //  ===
                    txtPad_L.Text = pcBlank;
                    txtPad_Pivot_Offset.Text = pcBlank;
            
                    for (int i = 0; i < 6; i++)
                    {
                        mtxtPivot_Loc[i].Text = pcBlank;
                    }

                    txtPad_T_Lead.Text = pcBlank;
                    txtPad_T_Pivot.Text = pcBlank;
                    txtPad_T_Trail.Text = pcBlank;
                    //txtPad_RFillet_ID.Text = pcBlank;  

                //  Flexure Pivot
                //  =============
                    txtFlexPivot_Web_RFillet.Text = pcBlank;
                    txtFlexPivot_Web_T.Text = pcBlank;
                    txtFlexPivot_Web_RFillet.Text = pcBlank;
                    txtFlexPivot_Web_H.Text = pcBlank;
                    txtFlexPivot_Rot_Stiff.Text = pcBlank;

                //  OilInlet
                //  ========
                    txtOilInlet_Orifice_D.Text = pcBlank;

                //  Material
                //  ========
                    txtLiningT.Text = pcBlank;
            }


            private void SetLocalObject()
            //===========================
            {
                mProduct =(clsProduct) modMain.gProject.Product.Clone();
                mBearing_Radial_FP = (clsBearing_Radial_FP)((clsBearing_Radial_FP)mProduct.Bearing).Clone();
            }


            private void DisplayData()  
            //========================
            {
                //....Type Radial.                
                txtRadialType.Text = mBearing_Radial_FP.Design.ToString().Replace("_", " "); 

                //....Split Config.                
                if (mBearing_Radial_FP.SplitConfig)
                    cmbSplitConfig.SelectedIndex = 0;       
                else
                    cmbSplitConfig.SelectedIndex = 1;       

                //....Set Length Unit.                      //....Not Used Now
                //string pUnit = "in"; 
                //lblLengthUnit.Text = pUnit; 

                //....DSet,DShaft,DFit & DPad.
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        mtxtDShaft_Range[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.DShaft_Range[i]));
                        mtxtDFit_Range[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.DFit_Range[i]));
                        mtxtDSet_Range[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.DSet_Range[i]));
                        mtxtDPad_Range[i].Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.DPad_Range[i]));
                    }
                    else
                    {
                        mtxtDShaft_Range[i].Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.DShaft_Range[i]);
                        mtxtDFit_Range[i].Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.DFit_Range[i]);
                        mtxtDSet_Range[i].Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.DSet_Range[i]);
                        mtxtDPad_Range[i].Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.DPad_Range[i]);
                    }
                }

                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    //.....PreLoad.
                    //if (IsDiaNotNull())
                    //    txtPreLoad.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()));
                    if (IsDiaNotNull())
                        txtPreLoad.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.PreLoad());


                    //.....Clearence.                
                    txtClearance.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()));
                }
                else
                {
                    //.....PreLoad.
                    if (IsDiaNotNull())
                        txtPreLoad.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.PreLoad());

                    //.....Clearence.                
                    txtClearance.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Clearance());
                }
                    
                //....Lengths
                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                {
                    txtL_Available.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(modMain.gProject.Product.L_Available));
                    txtL_Tot.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(modMain.gProject.Product.L_Tot()));
                }
                else
                {
                    txtL_Available.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Product.L_Available);
                    txtL_Tot.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Product.L_Tot());
                }
                   
              
                //  Pad:
                //  ====                  
                    cmbPad_Type.Text = mBearing_Radial_FP.Pad.Type.ToString();

                    //...Count
                    txtPad_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Pad.Count);
                    updPad_Count.Value = mBearing_Radial_FP.Pad.Count;
                
                    //...Length  
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtPad_L.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.L));
                    }
                    else
                    {
                        txtPad_L.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Pad.L);
                    }

                    //....Angle.
                    txtPad_Ang.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Angle(), "#0");  //....Retrieved     
                                      
                    //....Pivot Offset
                    if (modMain.gOpCond.Rot_Directionality == clsOpCond.eRotDirectionality.Bi)
                    {
                        txtPad_Pivot_Offset.Text = "50";                       
                        txtPad_Pivot_Offset.ReadOnly = true;
                        txtPad_Pivot_Offset.BackColor =  txtClearance.BackColor;
                        txtPad_Pivot_Offset.ForeColor = Color.Blue;   
                    }

                    else if (modMain.gOpCond.Rot_Directionality == clsOpCond.eRotDirectionality.Uni)
                    {                       
                        txtPad_Pivot_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Pivot.Offset, "#0.0");  
                        txtPad_Pivot_Offset.ReadOnly = false;
                        txtPad_Pivot_Offset.BackColor = Color.White;
                        txtPad_Pivot_Offset.ForeColor = Color.Black;
                    }

                    //....Location.     
                    mtxtPivot_Loc[0].Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Pivot.AngStart, "#0.0");

                    //....Thick  
                    //if (Math.Abs(mBearing_Radial_FP.Pad.T.Lead -  mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                    //    || Math.Abs(mBearing_Radial_FP.Pad.T.Trail - mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                    //    || Math.Abs(mBearing_Radial_FP.Pad.T.Lead - mBearing_Radial_FP.Pad.T.Trail) > modMain.gcEPS)  
                    //{
                    //    chkThick_Pivot.Checked = false;
                    //}
                    //else
                    //{
                    //    chkThick_Pivot.Checked = true;
                    //}

                    //BG 26MAR13
                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot_Checked)
                    {
                        if (Math.Abs(mBearing_Radial_FP.Pad.T.Lead - mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                          || Math.Abs(mBearing_Radial_FP.Pad.T.Trail - mBearing_Radial_FP.Pad.T.Pivot) > modMain.gcEPS
                          || Math.Abs(mBearing_Radial_FP.Pad.T.Lead - mBearing_Radial_FP.Pad.T.Trail) > modMain.gcEPS)
                        {
                            chkThick_Pivot.Checked = false;
                        }
                        else
                        {
                            chkThick_Pivot.Checked = true;
                        }
                    }
                    else
                        chkThick_Pivot.Checked = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot_Checked;

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtPad_T_Lead.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.T.Lead));
                        txtPad_T_Pivot.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.T.Pivot));
                        txtPad_T_Trail.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.T.Trail));

                        //....RFillet_ID
                        txtPad_RFillet_ID.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet_ID()));
                    }
                    else
                    {
                        txtPad_T_Lead.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Pad.T.Lead);
                        txtPad_T_Pivot.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Pad.T.Pivot);
                        txtPad_T_Trail.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Pad.T.Trail);

                        //....RFillet_ID
                        txtPad_RFillet_ID.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Pad.RFillet_ID());
                    }

                    ////....EDM Relief       //BG 06DEC12
                    //for(int i = 0; i < 2; i++)
                    //{
                    //    mtxtEDM_Relief[i].Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.EDM_Relief[i], "#0.000");
                    //}
  
                //  Flexure Pivot
                //  =============

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtFlexPivot_Web_T.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Web.T));
                        txtFlexPivot_Web_RFillet.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Web.RFillet));
                        txtFlexPivot_Web_H.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Web.H));
                    }
                    else
                    {
                        txtFlexPivot_Web_T.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.FlexurePivot.Web.T);
                        txtFlexPivot_Web_RFillet.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.FlexurePivot.Web.RFillet);
                        txtFlexPivot_Web_H.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.FlexurePivot.Web.H);
                    }

                    if (mBearing_Radial_FP.FlexurePivot.GapEDM != 0.0)
                    {
                        int pIndx;
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            pIndx = cmbFlexPivot_GapEDM.Items.IndexOf(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.GapEDM).ToString());
                        }
                        else
                        {
                            pIndx = cmbFlexPivot_GapEDM.Items.IndexOf(mBearing_Radial_FP.FlexurePivot.GapEDM.ToString());
                        }
                        cmbFlexPivot_GapEDM.SelectedIndex = pIndx;
                    }
                    else
                        cmbFlexPivot_GapEDM.Text = "";
                    //txtFlexPivot_GapEDM.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.FlexurePivot.GapEDM, "#0.000");

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtFlexPivot_Rot_Stiff.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Rot_Stiff), "#0");
                    }
                    else
                    {
                        txtFlexPivot_Rot_Stiff.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.FlexurePivot.Rot_Stiff, "#0");
                    }


                //  End Thrust Bearing related.
                //  ==========================

                    if (mProduct.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        mProduct.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            //....Dist_ThrustFace
                            if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] < modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mProduct.Dist_ThrustFace[0]));
                                optEndTBPos_Front.Checked = true;
                            }

                            else if ((mProduct.Dist_ThrustFace[0] < modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mProduct.Dist_ThrustFace[1]));
                                optEndTBPos_Back.Checked = true;
                            }

                            else if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mProduct.Dist_ThrustFace[0]));
                                optEndTBPos_Front.Checked = true;
                            }
                        }
                        else
                        {
                            //....Dist_ThrustFace
                            if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] < modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(mProduct.Dist_ThrustFace[0]);
                                optEndTBPos_Front.Checked = true;
                            }

                            else if ((mProduct.Dist_ThrustFace[0] < modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(mProduct.Dist_ThrustFace[1]);
                                optEndTBPos_Back.Checked = true;
                            }

                            else if ((mProduct.Dist_ThrustFace[0] > modMain.gcEPS) && (mProduct.Dist_ThrustFace[1] > modMain.gcEPS))
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.gProject.Unit.WriteInUserL(mProduct.Dist_ThrustFace[0]);
                                optEndTBPos_Front.Checked = true;
                            }
                        }
                    }


                //  OilInlet
                //  ========                  

                    if (mBearing_Radial_FP.Pad.L > mBearing_Radial_FP.PAD_L_THRESHOLD)
                        cmbOilInlet_Orifice_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.OilInlet.Orifice.Count);
                    else
                        txtOilInlet_Orifice_Count.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Pad.Count);

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtOilInlet_Orifice_D.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.D));
                    }
                    else
                    {
                        txtOilInlet_Orifice_D.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Orifice.D);
                    }


                //  Material
                //  ========
                    cmbMat_Base.Text = mBearing_Radial_FP.Mat.Base;

                    chkMat_LiningExists.Checked = mBearing_Radial_FP.Mat.LiningExists;
                    Set_LiningMat_Design();

                    cmbMat_Lining.Text = mBearing_Radial_FP.Mat.Lining;
                   
                    if (mBearing_Radial_FP != null)
                    {
                        txtLining_WaukeshaCode.Text = mBearing_Radial_FP.Mat.MatCode(cmbMat_Lining.Text);
                    }

                    if (mBearing_Radial_FP.Mat.LiningExists)
                    {
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                            {
                                txtLiningT.ForeColor = Color.Magenta; //Color.Purple;
                                txtLiningT.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()));
                            }
                            else
                            {
                                txtLiningT.ForeColor = Color.Black;
                                txtLiningT.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.LiningT));
                            }
                        }
                        else
                        {
                            if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                            {
                                txtLiningT.ForeColor = Color.Magenta; //Color.Purple;
                                txtLiningT.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.Mat_Lining_T());
                            }
                            else
                            {
                                txtLiningT.ForeColor = Color.Black;
                                txtLiningT.Text = modMain.gProject.Unit.WriteInUserL(mBearing_Radial_FP.LiningT);
                            }
                        }
                    }                                                                         
            }
       

            private void SetControls()
            //=======================                           
            {
                Boolean pEnabled = false;
                Color pControlColor = txtRadialType.BackColor;
                SetControls_Status(pEnabled);       //BG 28JUN13

                //if (modMain.gProject.Status == "Open" &&
                //     (modMain.gUser.Privilege == "Engineering"))  //BG 28JUN13

                ////if (modMain.gProject.Status == "Open" &&
                ////    modMain.gUser.Role == "Engineer")   //BG 28JUN13
                ////{
                    pEnabled = true;
                    //txtPad_T_Pivot.BackColor = Color.White;
                    txtPad_Count.BackColor = Color.White;

                    SetControls_Status(pEnabled);
                ////}
                //else if (modMain.gProject.Status == "Closed" ||
                //         modMain.gUser.Privilege == "Manufacturing" ||
                //         modMain.gUser.Privilege == "Designer" ||
                //         modMain.gUser.Privilege == "General" ||
                //         modMain.gUser.Role != "Engineer")      //BG 28JUN13
                ////else
                ////{
                ////    //pEnabled = false;     //BG 28JUN13

                ////    txtPad_Ang.BackColor = pControlColor;   
                ////    txtPad_Pivot_Offset.BackColor = pControlColor;
                ////    txtPad_T_Pivot.BackColor = pControlColor;
                ////    txtPad_Count.BackColor = pControlColor;
                ////    txtPad_Pivot_AngStart.BackColor = pControlColor;
                ////    txtPad_T_Lead.BackColor = pControlColor;
                ////    txtPad_T_Trail.BackColor = pControlColor;

                ////    //SetControls_Status(pEnabled);     //BG 28JUN13
                ////}

               
                //....End Thrust Bearing related.
                Boolean pblnEndTB = false;

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)        
                {
                    pblnEndTB = true;
                    grpPos_TB.Refresh();
                    grpPos_TB.Visible = pblnEndTB;

                    //....For Both T/B
                    if (modMain.gProject.Product.EndConfig[0].Type== clsEndConfig.eType.Thrust_Bearing_TL &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        optEndTBPos_Front.Visible = pblnEndTB;
                        optEndTBPos_Front.Checked = pblnEndTB;
                        optEndTBPos_Back.Visible = pblnEndTB;                        
                    }

                    //....For Front T/B
                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        optEndTBPos_Front.Visible = pblnEndTB;
                        optEndTBPos_Front.Checked = pblnEndTB;
                        optEndTBPos_Back.Visible = !pblnEndTB;                       
                    }

                    //....For Back T/B
                    else if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        optEndTBPos_Front.Visible = !pblnEndTB;
                        optEndTBPos_Back.Visible = pblnEndTB;
                        optEndTBPos_Back.Checked = pblnEndTB;                       
                    }                   
                }
                else
                {
                    pblnEndTB = false;
                    grpPos_TB.Refresh();
                    grpPos_TB.Visible = pblnEndTB;                    
                }

                lblEndConfig_Thrust_Sep.Visible = pblnEndTB;
                lblAxialDist_PadMidPt_ThrustFace.Visible = pblnEndTB;
                txtAxialDist_PadMidPt_ThrustFace.Visible = pblnEndTB;
            }


            private void SetControls_Status(Boolean Enable_In)
            //==================================================
            {
                cmbSplitConfig.Enabled = Enable_In;
                //....Shaft Dia.
                txtDShaft_Range_Min.ReadOnly = !Enable_In;
                txtDShaft_Range_Max.ReadOnly = !Enable_In;

                //....Outer Dia.
                txtDFit_Range_Min.ReadOnly = !Enable_In;
                txtDFit_Range_Max.ReadOnly = !Enable_In;

                //....Pad Dia.
                txtDPad_Range_Min.ReadOnly = !Enable_In;
                txtDPad_Range_Max.ReadOnly = !Enable_In;

                //....Set Dia.
                txtDSet_Range_Min.ReadOnly = !Enable_In;
                txtDSet_Range_Max.ReadOnly = !Enable_In;

                //....Length.
                txtL_Available.ReadOnly = !Enable_In;
                //txtL_Tot.ReadOnly = !pEnable_In;

                //  Pad:
                //  ----
                cmbPad_Type.Enabled = false; //Enable_In;
                updPad_Count.Enabled = Enable_In;

                txtPad_Count.ReadOnly = !Enable_In;
                txtPad_L.ReadOnly = !Enable_In;
                if(modMain.gOpCond.Rot_Directionality== clsOpCond.eRotDirectionality.Uni)
                    txtPad_Ang.ReadOnly = !Enable_In;                             
                txtPad_Pivot_Offset.ReadOnly = !Enable_In;
                txtPad_Pivot_AngStart.ReadOnly = !Enable_In;
                //chkRound.Enabled = pEnable_In;                                  

                //....Thickness.
                chkThick_Pivot.Enabled = Enable_In;

                ////if (modMain.gProject.Status == "Closed" ||
                ////    modMain.gUser.Role != "Engineer")                      
                ////{
                ////    txtPad_T_Lead.ReadOnly = !Enable_In;
                ////    txtPad_T_Pivot.ReadOnly = !Enable_In;
                ////    txtPad_T_Trail.ReadOnly = !Enable_In;
                ////}

                //txtPad_RFillet_ID.ReadOnly = !pEnable_In;

                //  Web
                //  ----
                txtFlexPivot_Web_T.ReadOnly = !Enable_In;
                txtFlexPivot_Web_RFillet.ReadOnly = !Enable_In;
                txtFlexPivot_Web_H.ReadOnly = !Enable_In;
                cmbFlexPivot_GapEDM.Enabled = Enable_In;
                txtFlexPivot_Rot_Stiff.ReadOnly = !Enable_In;

                txtOilInlet_Orifice_D.ReadOnly = !Enable_In;

                ////chkMat_LiningExists.Enabled = Enable_In;
                cmbMat_Base.Enabled = Enable_In;
                ////cmbMat_Lining.Enabled = Enable_In;
                ////txtLiningT.ReadOnly = !Enable_In;


                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    txtAxialDist_PadMidPt_ThrustFace.ReadOnly = !Enable_In;
                }
            }            

        #endregion   
                

        #region "CONTROL EVENT ROUTINES:" 
        //*****************************

            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

                private void cmdOK_Click(object sender, EventArgs e)
                //==================================================
                {
                    CloseForm();            
                }

                private void CloseForm()    
                //======================
                {
                    SaveData(); 

                    this.Hide();

                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        modMain.gfrmSeal.ShowDialog();
                    }

                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        modMain.gfrmThrustBearing.ShowDialog();
                    }
                }


                private void SaveData()
                //=====================
                {
                    //....SplitConfig.
                    if (cmbSplitConfig.SelectedIndex == 0)
                       ((clsBearing_Radial_FP) modMain.gProject.Product.Bearing).SplitConfig = true;
                    else
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SplitConfig = false;

                    //....Save DFit,DSet,DShaft & DPad
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DFit_Range[i] =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtDFit_Range[i].Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DPad_Range[i] =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtDPad_Range[i].Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DSet_Range[i] =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtDSet_Range[i].Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DShaft_Range[i] =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(mtxtDShaft_Range[i].Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DFit_Range[i] = modMain.ConvTextToDouble(mtxtDFit_Range[i].Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DPad_Range[i] = modMain.ConvTextToDouble(mtxtDPad_Range[i].Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DSet_Range[i] = modMain.ConvTextToDouble(mtxtDSet_Range[i].Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DShaft_Range[i] = modMain.ConvTextToDouble(mtxtDShaft_Range[i].Text);
                        }
                    }

                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        modMain.gProject.Product.L_Available = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL_Available.Text));
                    }
                    else
                    {
                        modMain.gProject.Product.L_Available = modMain.ConvTextToDouble(txtL_Available.Text);
                    }


                    #region "Pad:"
                    //  ---------
                        if (cmbPad_Type.Text!="")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Type = (clsBearing_Radial_FP.clsPad.eLoadPos)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.clsPad.eLoadPos),cmbPad_Type.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count = modMain.ConvTextToInt(updPad_Count.Value.ToString());

                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_L.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L = modMain.ConvTextToDouble(txtPad_L.Text);
                        }

                        //....Pivot
                        if (mtxtPivot_Loc[0].Text != "")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot_AngStart = modMain.ConvTextToDouble(mtxtPivot_Loc[0].Text);

                        Calc_Pad_Pivot_Locations(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count);
                     
                        //modMain.gRadialBearing.PadAng = modMain.ConvTextToDouble(txtPad_Ang.Text);

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot_Offset = modMain.ConvTextToDouble(txtPad_Pivot_Offset.Text);

                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (mBearing_Radial_FP.Pad.T.Lead == mBearing_Radial_FP.Pad.T.Pivot && mBearing_Radial_FP.Pad.T.Trail == mBearing_Radial_FP.Pad.T.Pivot)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Lead =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Trail = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Lead =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Lead.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Pivot.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Trail = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtPad_T_Trail.Text));
                            }
                        }
                        else
                        {
                            if (mBearing_Radial_FP.Pad.T.Lead == mBearing_Radial_FP.Pad.T.Pivot && mBearing_Radial_FP.Pad.T.Trail == mBearing_Radial_FP.Pad.T.Pivot)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Lead = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Trail = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Lead = modMain.ConvTextToDouble(txtPad_T_Lead.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot = modMain.ConvTextToDouble(txtPad_T_Pivot.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Trail = modMain.ConvTextToDouble(txtPad_T_Trail.Text);
                            }
                        }
                        //modMain.gRadialBearing.PadRFillet = modMain.ConvTextToDouble(txtPad_RFillet_ID.Text);

                    #endregion


                    //#region "EDM Relief:"
                        ////  ----------------       //BG 06DEC12
                    //    for (int i = 0; i < 2; i++)
                    //    {
                    //        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).EDM_Relief[i]  = modMain.ConvTextToDouble(mtxtEDM_Relief[i].Text);
                    //    }
                        
                    //#endregion


                    #region"Flexure Pivot:"
                    //  ------------------
                        //....Web
                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_T = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_T.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_RFillet = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_H = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtFlexPivot_Web_H.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text));
                            ////((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Rot_Stiff = modMain.ConvTextToDouble(txtFlexPivot_Rot_Stiff.Text);
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_T = modMain.ConvTextToDouble(txtFlexPivot_Web_T.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_RFillet = modMain.ConvTextToDouble(txtFlexPivot_Web_RFillet.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web_H = modMain.ConvTextToDouble(txtFlexPivot_Web_H.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM = modMain.ConvTextToDouble(cmbFlexPivot_GapEDM.Text);
                        }

                    #endregion


                    //....End Thrust Bearing Related
                        
                            if (mProduct.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                                mProduct.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                            {
                                if (optEndTBPos_Front.Checked)
                                {
                                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        modMain.gProject.Product.Dist_ThrustFace[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text));
                                    }
                                    else
                                    {
                                        modMain.gProject.Product.Dist_ThrustFace[0] = modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text);
                                    }
                                }
                                else if (optEndTBPos_Back.Checked)
                                {
                                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        modMain.gProject.Product.Dist_ThrustFace[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text));
                                    }
                                    else
                                    {
                                        modMain.gProject.Product.Dist_ThrustFace[1] = modMain.ConvTextToDouble(txtAxialDist_PadMidPt_ThrustFace.Text);
                                    }
                                }
                            }
                       
                      

                    #region "OilInlet:"
                    //  --------------
                        if (mBearing_Radial_FP.Pad.L > mBearing_Radial_FP.PAD_L_THRESHOLD)
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Count = modMain.ConvTextToInt(cmbOilInlet_Orifice_Count.Text);
                        else
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Count = modMain.ConvTextToInt(txtOilInlet_Orifice_Count.Text);

                        if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_D = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Orifice_D.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_D = modMain.ConvTextToDouble(txtOilInlet_Orifice_D.Text);
                        }

                    #endregion


                    #region "Material:"
                    // --------------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.Base = cmbMat_Base.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.LiningExists = chkMat_LiningExists.Checked;

                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.LiningExists)                   
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.Lining = cmbMat_Lining.Text;
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLiningT.Text));
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT = modMain.ConvTextToDouble(txtLiningT.Text);
                            }
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.Lining = "None";
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT = 0.0F;
                        }

                    #endregion     
                }


                private void Update_Depth_EndConfig(Double pPrev_Pad_L_In, Double pCurr_Pad_L_In)
                {
                    if (pPrev_Pad_L_In != pCurr_Pad_L_In)
                    {
                        MessageBox.Show("Not Exact Value");
                    }
                }

                private void cmdAccessories_Click(object sender, EventArgs e)
                //===========================================================
                {
                    //modMain.gfrmAccessories.TempSensor_Count = mBearing_Radial_FP.Pad.Count;  
                    //modMain.gfrmAccessories.ShowDialog();                           
                }


                private void cmdCancel_Click(object sender, EventArgs e)
                //=======================================================
                {                    
                    this.Close();
                }


                private void cmdPrint_Click(object sender, EventArgs e)
                //=======================================================   
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }

            #endregion


            #region "TEXTBOX RELATED ROUTINES:"
            //--------------------------------
       
            //....KeyDown is raised as soon as the user presses a key on the keyboard, while they're still holding 
            //........it down. USE THIS ONE.

            //....KeyPress is raised for character keys (unlike KeyDown and KeyUp, which are also raised for 
            //........noncharacter keys) while the key is pressed. This is a "higher-level" event than either 
            //........KeyDown or KeyUp, and as such, different data is available in the EventArgs.

            //....KeyUp is raised after the user releases a key on the keyboard.


            //private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
            //==============================================================
            //{
            //    ...."Key Press" event is called when a 
            //    TextBox pTxtBox = (TextBox)sender;

            //    switch (pTxtBox.Name)
            //    {
            //        case "txtDShaft_Range_Min":
            //                mblnDShaft_ManuallyChanged = true; 
            //            break;

            //        case "txtDShaft_Range_Max":
            //                mblnDShaft_ManuallyChanged = true; 
            //            break;

            //        case "txtDSet_Range_Min":
            //            mblnDSet_Changed = true;
            //            break;

            //        case "txtDSet_Range_Max":
            //            mblnDSet_Changed = true;
            //            break;

            //        case "txtFlexPivot_Web_T":
            //            mblnFlexPivot_Web_T = true;
            //            break;

            //    }
            //}


                private void TxtBox_KeyDown(object sender, KeyEventArgs e)
                //========================================================
                {       
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtDShaft_Range_Min":
                        case "txtDShaft_Range_Max":

                            mblnDShaft_ManuallyChanged = true;
                            break;

                        case "txtDSet_Range_Min":
                        case "txtDSet_Range_Max":

                            mblnDSet_ManuallyChanged = true;
                            break;

                        case "txtPad_T_Pivot":
                            mblnPad_T_Pivot_ManuallyChanged = true;
                            break;

                        case "txtFlexPivot_Web_T":
                            mblnWeb_T_ManuallyChanged = true;
                            break;
                    }
                }


                private void TextBox_TextChanged(object sender, EventArgs e)
                //==========================================================    
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtDShaft_Range_Min":
                            //=====================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DShaft_Range[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDShaft_Range_Min.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000"); 
                            }
                            else
                            {
                                mBearing_Radial_FP.DShaft_Range[0] = modMain.ConvTextToDouble(txtDShaft_Range_Min.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
  
                              

                            
                            //....Pad pivot thickness is 15% of DShaft.
                            if (mblnDShaft_ManuallyChanged || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot_Checked)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.TDef()), "#0.000");
                                }
                                else
                                {
                                    txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");
                                }
                               txtPad_T_Pivot.ForeColor = Color.Blue;
                               mblnDShaft_ManuallyChanged = false;
                            }

                            Check_Pad_T_Pivot(txtDShaft_Range_Min.Text, txtDShaft_Range_Max.Text);

                            
                            
                            break;


                        case "txtDShaft_Range_Max":
                            //=====================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DShaft_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDShaft_Range_Max.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.00");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.00"); 
                            }
                            else
                            {
                                mBearing_Radial_FP.DShaft_Range[1] = modMain.ConvTextToDouble(txtDShaft_Range_Max.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
                                
                            
                            
                            //....Pad pivot thickness is 15% of DShaft.
                            if (mblnDShaft_ManuallyChanged || ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot_Checked)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.TDef()), "#0.000");
                                }
                                else
                                {
                                    txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");
                                }
                                mblnDShaft_ManuallyChanged = false;
                            }

                            Check_Pad_T_Pivot(txtDShaft_Range_Min.Text, txtDShaft_Range_Max.Text);
                            break;

                        case "txtDFit_Range_Min":
                            //=================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DFit_Range[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.DFit_Range[0] = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;

                        case "txtDFit_Range_Max":
                            //==================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DFit_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.DFit_Range[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;

                        case "txtDSet_Range_Min":
                            //===================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DSet_Range[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDSet_Range_Min.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000");
                            }
                            else
                            {
                                mBearing_Radial_FP.DSet_Range[0] = modMain.ConvTextToDouble(txtDSet_Range_Min.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");
                            }

                           

                            //....RFillet_ID
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet_ID()), "#0.000");
                            }
                            else
                            {
                                txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet_ID(), "#0.000");
                            }

                            //....Lining T
                            if (mblnDSet_ManuallyChanged)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).EDM_Pad.RFillet_Back = 0.0;
                                ////txtLiningT.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mat_Lining_T(), "#0.000");
                                mblnDSet_ManuallyChanged = false;
                            }
                          
                            break;

                        case "txtDSet_Range_Max":
                            //===================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DSet_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDSet_Range_Max.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000");

                            }
                            else
                            {
                                mBearing_Radial_FP.DSet_Range[1] = modMain.ConvTextToDouble(txtDSet_Range_Max.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");

                            }

                           
                            //....RFillet_ID
                            //txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet_ID(), "#0.000");
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Pad.RFillet_ID()), "#0.000");
                            }
                            else
                            {
                                txtPad_RFillet_ID.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.RFillet_ID(), "#0.000");
                            }

                            //....Lining T
                            if (mblnDSet_ManuallyChanged)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).EDM_Pad.RFillet_Back = 0.0;
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtLiningT.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()), "#0.000");
                                }
                                else
                                {
                                    txtLiningT.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mat_Lining_T(), "#0.000");
                                }
                                mblnDSet_ManuallyChanged = false;
                            }                          

                            break;


                        case "txtDPad_Range_Min":
                            //===================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DPad_Range[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDPad_Range_Min.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.00");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.00");   
                            }
                            else
                            {
                                mBearing_Radial_FP.DPad_Range[0] = modMain.ConvTextToDouble(txtDPad_Range_Min.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000");   
                            }

                           
                            
                            break;

                        case "txtDPad_Range_Max":
                            //===================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.DPad_Range[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDPad_Range_Max.Text));

                                //if (IsDiaNotNull())
                                //    txtPreLoad.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.PreLoad()), "#0.000");
                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Clearance()), "#0.0000"); 
                            }
                            else
                            {
                                mBearing_Radial_FP.DPad_Range[1] = modMain.ConvTextToDouble(txtDPad_Range_Max.Text);

                                if (IsDiaNotNull())
                                    txtPreLoad.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.PreLoad(), "#0.000");

                                txtClearance.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Clearance(), "#0.0000"); 
                            }
                            
                            
                            break;                      
                       

                        case "txtPad_Count":
                            //==============
                            //....Pad Count.
                            int pCount = modMain.ConvTextToInt(txtPad_Count.Text);
                            Set_Pad_Pivot_Locations(pCount);

                            //....Set Pad Count.
                            mBearing_Radial_FP.Pad.Count = pCount;

                            //....Set Pad Angle.
                            //txtPad_Ang.ForeColor = Color.Purple;               
                            txtPad_Ang.Text = mBearing_Radial_FP.Pad.Angle().ToString();
                            if (txtPad_Pivot_AngStart.Text != "")
                                Calc_Pad_Pivot_Locations(pCount);                            

                            //....Set Orifice Count.
                            //txtOilInlet_Orifice_Count.Text = Convert.ToString(pCount);
                            Display_OilInlet_Orifice_Count(mBearing_Radial_FP.Pad.Count, mBearing_Radial_FP.Pad.L);

                            break;
                                                   

                        case "txtPad_L":
                            //==========
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Pad.L = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.Pad.L = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            Display_OilInlet_Orifice_Count(mBearing_Radial_FP.Pad.Count,mBearing_Radial_FP.Pad.L);

                            //....Total L
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtL_Tot.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(mProduct.L_Tot()));
                            }
                            else
                            {
                                txtL_Tot.Text = modMain.gProject.Unit.WriteInUserL(mProduct.L_Tot());
                            }
                            break;

                        
                        case "txtPad_Pivot_Offset":
                            //=====================
                            mBearing_Radial_FP.Pad.Pivot_Offset = modMain.ConvTextToDouble(pTxtBox.Text);

                            if (pTxtBox.Text.Contains("."))
                            {
                                string pNum = modMain.ExtractPostData(pTxtBox.Text, ".");
                                if (pNum.Length > 1)
                                    pTxtBox.Text = modMain.ConvDoubleToStr(Math.Round(mBearing_Radial_FP.Pad.Pivot.Offset, 1),"#0.0");
                            }
                            break;


                        case "txtPad_Pivot_AngStart":
                            //=======================
                            int pPadCount = modMain.ConvTextToInt(txtPad_Count.Text);
                            mBearing_Radial_FP.Pad.Pivot_AngStart =
                               modMain.ConvTextToDouble(txtPad_Pivot_AngStart.Text);

                            if (txtPad_Pivot_AngStart.Text != "")
                            {
                                Calc_Pad_Pivot_Locations(pPadCount);
                            }
                            else if (txtPad_Pivot_AngStart.Text == "")
                            {
                                for (int i = 1; i < pPadCount; i++)
                                {
                                    mtxtPivot_Loc[i].Text = "";
                                }
                            }
                            break;


                        case "txtPad_T_Lead":
                            //================   
                                  
                            if (!chkThick_Pivot.Checked)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.Pad.T_Lead = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                                }
                                else
                                {
                                    mBearing_Radial_FP.Pad.T_Lead = modMain.ConvTextToDouble(pTxtBox.Text);
                                }
                            }
                           
                            break;


                        case "txtPad_T_Pivot":
                            //================  

                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Pad.T_Pivot =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.Pad.T_Pivot = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            if (chkThick_Pivot.Checked)
                            {
                                Set_Pad_T_Lead_AND_Trail (mBearing_Radial_FP.Pad.T.Pivot);
                            }

                            if (mblnPad_T_Pivot_ManuallyChanged)
                            {
                                pTxtBox.ForeColor = Color.Black;
                            }
                            break;


                        case "txtPad_T_Trail":
                            //=================  
       
                            if (!chkThick_Pivot.Checked)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.Pad.T_Trail = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                                }
                                else
                                {
                                    mBearing_Radial_FP.Pad.T_Trail = modMain.ConvTextToDouble(pTxtBox.Text);
                                }
                            }
                                                        
                            break;


                        case "txtFlexPivot_Web_T":
                            //====================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_T =modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_T = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                             if (mblnWeb_T_ManuallyChanged)
                             {
                                 Double pWeb_RFillet;
                                 if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                 {
                                     pWeb_RFillet = modMain.MRound(modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.FlexurePivot.Web.T), 0.005);
                                 }
                                 else
                                 {
                                     pWeb_RFillet = modMain.MRound(mBearing_Radial_FP.FlexurePivot.Web.T, 0.005);
                                 }
                                 txtFlexPivot_Web_RFillet.Text = modMain.ConvDoubleToStr(pWeb_RFillet, "#0.000");
                                 txtFlexPivot_Web_RFillet.ForeColor = Color.Blue;
                             }
                             else
                             {
                                 txtFlexPivot_Web_RFillet.ForeColor = Color.Black;
                             }
                             mblnWeb_T_ManuallyChanged = false;
                            break;


                        case "txtFlexPivot_Web_RFillet":
                            //============================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_RFillet =modMain.gProject.Unit.CMet_Eng( modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_RFillet = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            Double pPrev_Web_RFillet = modMain.MRound(mBearing_Radial_FP.FlexurePivot.Web.T, 0.005);
                            if (Math.Abs(mBearing_Radial_FP.FlexurePivot.Web.RFillet - pPrev_Web_RFillet) < modMain.gcEPS)
                            {
                                txtFlexPivot_Web_RFillet.ForeColor = Color.Magenta;
                            }
                            else
                            {
                                txtFlexPivot_Web_RFillet.ForeColor = Color.Black;
                            }

                            break;


                        case "txtFlexPivot_Web_H":
                            //======================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_H = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.FlexurePivot.Web_H = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;


                        case "txtFlexPivot_Rot_Stiff":
                            //=========================
                            mBearing_Radial_FP.FlexurePivot.Rot_Stiff = modMain.ConvTextToDouble(pTxtBox.Text);
                            break;


                        case "txtAxialDist_PadMidPt_ThrustFace":
                            //==================================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (optEndTBPos_Front.Checked)
                                    mProduct.Dist_ThrustFace[0] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));

                                else if (optEndTBPos_Back.Checked)
                                    mProduct.Dist_ThrustFace[1] = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                if (optEndTBPos_Front.Checked)
                                    mProduct.Dist_ThrustFace[0] = modMain.ConvTextToDouble(pTxtBox.Text);

                                else if (optEndTBPos_Back.Checked)
                                    mProduct.Dist_ThrustFace[1] = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;
                          
  
                        case "txtOilInlet_Orifice_D":
                            //=======================
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.OilInlet.Orifice_D = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.OilInlet.Orifice_D = modMain.ConvTextToDouble(pTxtBox.Text);
                            }
                            break;


                        case "txtLiningT":
                            //============
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.LiningT = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.LiningT = modMain.ConvTextToDouble(pTxtBox.Text);
                            }

                            if (Math.Abs(mBearing_Radial_FP.LiningT - mBearing_Radial_FP.Mat_Lining_T()) < modMain.gcEPS)
                            {
                                txtLiningT.ForeColor = Color.Magenta; 
                            }
                            else
                            {
                                txtLiningT.ForeColor = Color.Black;                                
                            }

                            break;
                    }
                }


                private void Check_Pad_T_Pivot(String DShaft_Min_In, String DShaft_Max_In)
                //========================================================================
                {
                    //PB 16JAN13. To be reviewed. Some adhoc changes made here.
                    //
                    if(DShaft_Min_In == "" && DShaft_Max_In == "")
                    {                       
                        txtPad_T_Pivot.Text = "";

                        if (chkThick_Pivot.Checked)
                        {
                            txtPad_T_Lead.Text = "";
                            txtPad_T_Trail.Text = "";
                        }
                    }
                    else if (DShaft_Min_In == "" && DShaft_Max_In != "")
                    {                       
                        if (mBearing_Radial_FP.DShaft_Range[0] < modMain.gcEPS)
                        {                 
                            mBearing_Radial_FP.DShaft_Range[0] = 0;
                        }
                        //txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000"); //PB 16JAN13
                    }
                    else if (DShaft_Min_In != "" && DShaft_Max_In == "")
                    {
                       if (mBearing_Radial_FP.DShaft_Range[1] < modMain.gcEPS)
                        {
                            mBearing_Radial_FP.DShaft_Range[1] = 0;
                        }
                        //txtPad_T_Pivot.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.TDef(), "#0.000");   //PB 16JAN13
                    }
                }


                private void Display_OilInlet_Orifice_Count(int PadCount_In,double PadL_In)
                //========================================================================    
                { 
                    //if (PadL_In > mBearing_Radial_FP.PAD_L_THRESHOLD)  //BG 22MAR13
                    if (PadL_In >= mBearing_Radial_FP.PAD_L_THRESHOLD)   //BG 22MAR13 
                    {
                        cmbOilInlet_Orifice_Count.Visible = true;
                        txtOilInlet_Orifice_Count.Visible = false;
                        
                        cmbOilInlet_Orifice_Count.Items.Clear();
                        cmbOilInlet_Orifice_Count.Items.Add(PadCount_In);
                        cmbOilInlet_Orifice_Count.Items.Add((PadCount_In * 2));

                        if (mBearing_Radial_FP.OilInlet.Orifice.Count != 0)
                        {
                            cmbOilInlet_Orifice_Count.SelectedIndex = cmbOilInlet_Orifice_Count.Items.
                                                                          IndexOf(mBearing_Radial_FP.OilInlet.Orifice.Count);

                            if( cmbOilInlet_Orifice_Count.SelectedIndex==-1)
                                cmbOilInlet_Orifice_Count.SelectedIndex = 0;
                        }
                        else
                            cmbOilInlet_Orifice_Count.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbOilInlet_Orifice_Count.Visible = false;
                        txtOilInlet_Orifice_Count.Visible = true;
                        txtOilInlet_Orifice_Count.Text = Convert.ToString(PadCount_In);
                    }
                }

               
                private void Set_Pad_T_Lead_AND_Trail(double T_Pivot_In)    
                //======================================================    
                {
                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                    {
                        if (T_Pivot_In != 0.0)
                        {
                            txtPad_T_Lead.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(T_Pivot_In));
                            txtPad_T_Trail.Text = modMain.gProject.Unit.WriteInUserL(modMain.gProject.Unit.CEng_Met(T_Pivot_In));

                            mBearing_Radial_FP.Pad.T_Lead = T_Pivot_In;
                            mBearing_Radial_FP.Pad.T_Trail = T_Pivot_In;
                        }
                    }
                    else
                    {
                        if (T_Pivot_In != 0.0)
                        {
                            txtPad_T_Lead.Text = modMain.gProject.Unit.WriteInUserL(T_Pivot_In);
                            txtPad_T_Trail.Text = modMain.gProject.Unit.WriteInUserL(T_Pivot_In);

                            mBearing_Radial_FP.Pad.T_Lead = T_Pivot_In;
                            mBearing_Radial_FP.Pad.T_Trail = T_Pivot_In;
                        }
                    }
                }


                private void Set_Pad_Pivot_Locations(int PadCount_In)
                //====================================================
                {
                    int pIndx;

                    for (pIndx = 0; pIndx < PadCount_In; pIndx++)
                    {
                        mtxtPivot_Loc[pIndx].Visible = true;
                    }
                    
                    for (; pIndx < mBearing_Radial_FP.Pad.Count_Max; pIndx++)              
                        mtxtPivot_Loc[pIndx].Visible = false;
                }


                private void Calc_Pad_Pivot_Locations(int PadCount_In)
                //====================================================
                {
                    if (mBearing_Radial_FP.Pad.Pivot.AngStart != 0.0F)    
                    {
                        Double pLocVal;
                        pLocVal = modMain.ConvTextToDouble(txtPad_Pivot_AngStart.Text);
                        Double psngDeg = 0.0F;

                        if (PadCount_In != 0)
                        {
                            psngDeg = Convert.ToDouble(360 / PadCount_In);

                            for (int i = 1; i < PadCount_In; i++)
                            {
                                mtxtPivot_Loc[i].Text = (pLocVal + (i * psngDeg)).ToString("#0.0");
                            }
                        }
                    }
                }


                 //AM 13AUG12
                //private void txtBox_MouseDown(object sender, MouseEventArgs e)
                ////============================================================  
                //{
                //    TextBox pTxtBox = (TextBox)sender;
                //    //pTxtBox.ForeColor = Color.Black;

                //    switch (pTxtBox.Name)
                //    {
                //        case "":

                //            break;

                //    //    case "txtL_Tot":
                //    //    //--------------    //SB 15JUL09
                //    //        mBearing.Clone(ref mTempBearing);

                //    //        //chkRound.Checked = false;

                //    //        mblnL_Tot = true;
                //    //        mblnSeal_L = false;

                //    //        break;

                //    //    case "txtPad_L":
                //    //        //--------------
                //    //        mBearing.Clone(ref mTempBearing);

                //    //        //chkRound.Checked = false;

                //    //        mblnL_Tot = true;
                //    //        mblnSeal_L = false;

                //    //        break;
                //    }
                //}
       
            #endregion


            #region "UPDOWN CONTROL RELATED ROUTINE"
            //--------------------------------------

                private void updPad_Count_ValueChanged(object sender, EventArgs e)
                //=================================================================
                {
                    txtPad_Count.Text = updPad_Count.Value.ToString();
                }

            #endregion


            #region "COMBO BOX RELATED ROUTINE"
            //---------------------------------

                private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================== 
                {
                    ComboBox pCmbBox = (ComboBox)sender;

                    switch (pCmbBox.Name)
                    {
                        case "cmbSplitConfig":
                            //==============
                            if (mBearing_Radial_FP != null)
                            {
                                if (pCmbBox.SelectedIndex == 0)
                                    mBearing_Radial_FP.SplitConfig = true;
                                else
                                    mBearing_Radial_FP.SplitConfig = false;
                            }
                            break;

                        case "cmbPad_Type":
                            //==============
                            if (pCmbBox.Text != "")
                                if (mBearing_Radial_FP != null)
                                {
                                    mBearing_Radial_FP.Pad.Type = (clsBearing_Radial_FP.clsPad.eLoadPos)
                                                                   Enum.Parse(typeof(clsBearing_Radial_FP.clsPad.eLoadPos), pCmbBox.Text);
                                    //Set_Pad_Pivot_AngStart(pCmbBox.Text);
                                    txtPad_Pivot_AngStart.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Pad.Set_Pivot_AngStart(),"");
                                }
                            break;

                        case "cmbFlexPivot_GapEDM":
                            //=========================
                            if (mBearing_Radial_FP != null)
                            {
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.FlexurePivot.GapEDM = modMain.gProject.Unit.CMet_Eng(modMain.ConvTextToDouble(pCmbBox.Text));
                                }
                                else
                                {
                                    mBearing_Radial_FP.FlexurePivot.GapEDM = modMain.ConvTextToDouble(pCmbBox.Text);
                                }
                            }
                            break;                    

                        case "cmbMat_Base":
                            //=============
                            if (mBearing_Radial_FP != null)
                            {
                                mBearing_Radial_FP.Mat.Base = pCmbBox.Text;
                                txtBaseMat_WaukeshaCode.Text = mBearing_Radial_FP.Mat.MatCode(pCmbBox.Text);
                            }
                               
                            break;

                        case "cmbMat_Lining":
                            //===============
                            if (mBearing_Radial_FP != null)
                            {

                                mBearing_Radial_FP.Mat.Lining = pCmbBox.Text;
                                if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtLiningT.Text =modMain.gProject.Unit.CEng_Met( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT).ToString("0.000"); 
                                }
                                else
                                {
                                    txtLiningT.Text = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT.ToString("0.000"); 
                                }
                                


                                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mat.Lining != mBearing_Radial_FP.Mat.Lining)
                                {
                                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        txtLiningT.Text = modMain.gProject.Unit.CEng_Met(mBearing_Radial_FP.Mat_Lining_T()).ToString("0.000");
                                    }
                                    else
                                    {
                                        txtLiningT.Text = mBearing_Radial_FP.Mat_Lining_T().ToString("0.000");
                                    }
                                }
                                else
                                {
                                    if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        txtLiningT.Text = modMain.gProject.Unit.CEng_Met(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT).ToString("0.000");
                                    }
                                    else
                                    {
                                        txtLiningT.Text = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT.ToString("0.000");
                                    }
                                    
                                }

                                txtLining_WaukeshaCode.Text = mBearing_Radial_FP.Mat.MatCode(pCmbBox.Text);
                            }
  
                            break;
                    }
                }

              

            #endregion


            #region "CHECKBOX RELATED ROUTINE"

                private void ChkBox_CheckedChanged(object sender, EventArgs e)
                //============================================================   
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                    {
                        case "chkThick_Pivot":
                            //----------------
                               Set_Pad_T_Design();
                             
                               if (chkThick_Pivot.Checked)      //BG 21MAR13
                               {              
                                   Set_Pad_T_Lead_AND_Trail(mBearing_Radial_FP.Pad.T.Pivot);
                               }
                               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T_Pivot_Checked = chkThick_Pivot.Checked;
                           
                            break;


                        case "chkMat_LiningExists":
                            //---------------------
                            mBearing_Radial_FP.Mat.LiningExists = chkMat_LiningExists.Checked;
                            Set_LiningMat_Design();

                            if (!chkMat_LiningExists.Checked)
                            {
                                mBearing_Radial_FP.Mat.Lining = "None";    
                                mBearing_Radial_FP.LiningT = 0.0F;
                                txtLiningT.Text = modMain.ConvDoubleToStr(0.0F, "#0.000");
                            }
                            else
                            {
                                cmbMat_Lining.Text = "Babbitt";
                                txtLiningT.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.LiningT, "#0.000");
                            }

                            break;
                    }
                }

                private void Set_LiningMat_Design()
                //=================================   
                {
                    cmbMat_Lining.Visible = chkMat_LiningExists.Checked;
                    txtLining_WaukeshaCode.Visible = chkMat_LiningExists.Checked;   
                    lblThick.Visible = chkMat_LiningExists.Checked;
                    txtLiningT.Visible = chkMat_LiningExists.Checked;
                }


                private void Set_Pad_T_Design()
                //===========================                      
                {
                    txtPad_T_Lead.ReadOnly = chkThick_Pivot.Checked;
                    Set_ForeAndBackColor(ref txtPad_T_Lead, chkThick_Pivot.Checked);

                    txtPad_T_Trail.ReadOnly = chkThick_Pivot.Checked;
                    Set_ForeAndBackColor(ref txtPad_T_Trail, chkThick_Pivot.Checked);                    
                }


                private void Set_ForeAndBackColor (ref TextBox TxtBox_In, bool bln_In)
                //====================================================================    
                {
                    if (bln_In)
                    {
                        TxtBox_In.ForeColor = Color.Blue;
                        Color pColor = Color.FromArgb(255, 235, 233, 237);
                        TxtBox_In.BackColor = pColor;
                    }
                    else
                    {
                        TxtBox_In.ForeColor = Color.Black;
                        TxtBox_In.BackColor = Color.White;
                    }
                }

            #endregion


            #region "OPTION BUTTON RELATED ROUTINE:"

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //===============================================================
                {
                    RadioButton pOptBtn = (RadioButton)sender;

                    switch (pOptBtn.Name)
                    {
                        case "optEndTBPos_Front":
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(modMain.gProject.Product.Dist_ThrustFace[0]), "");
                            }
                            else
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[0], "");
                            }
                            break;

                        case "optEndTBPos_Back":
                            if (modMain.gProject.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Unit.CEng_Met(modMain.gProject.Product.Dist_ThrustFace[1]), "");
                            }
                            else
                            {
                                txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[1], "");
                            }
                            //txtAxialDist_PadMidPt_ThrustFace.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Dist_ThrustFace[1], "");
                            break;
                    }
                }
           #endregion

       #endregion


       #region "UTILITY ROUTINES:"
       //*************************

            private Boolean IsDiaNotNull()
            //============================
            {
                if ((mBearing_Radial_FP.DShaft_Range[0] != 0.0F && mBearing_Radial_FP.DShaft_Range[1] != 0.0F)
                ||  (mBearing_Radial_FP.DSet_Range[0]   != 0.0F && mBearing_Radial_FP.DSet_Range[1]   != 0.0F)
                ||  (mBearing_Radial_FP.DPad_Range[0]   != 0.0F && mBearing_Radial_FP.DPad_Range[1]   != 0.0F))
     
                    return true;

                else
                    return false;
            }

       #endregion

           

        

   }
}
