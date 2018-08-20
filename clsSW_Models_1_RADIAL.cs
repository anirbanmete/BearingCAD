//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models_1_RADIAL                  '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  01JAN13                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BearingCAD20
{
    partial class clsSW_Models
    {
     
        #region "DATABASE POPULATION:"

            private void Populate_tblMapping_Radial(clsProject Project_In, clsOpCond OpCond_In, clsDB DB_In)
            //================================================================================================ 
            {
                //....Populate the variable value field.

                try
                {
                    //.....Set Mapping Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_VarName = new StringCollection();

                    string pOrderBy = " ORDER BY fldItemNo ASC";

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Radial", "fldCellColName", pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_Radial", "fldSoftware_VarName", pOrderBy);

                    String pUPDATE = "UPDATE tblMapping_Radial ";
                    String pSET = "SET fldSoftware_VarVal = ";
                    String pVALUE = null;
                    String pWHERE = null;

                    String pSQL = null;

                    pSQL = pUPDATE + pSET + "NULL";
                    DB_In.ExecuteCommand(pSQL);

                    for (int i = 0; i < pSoftware_VarName.Count; i++)
                    {
                        pSQL = null;
                        pWHERE = null;
                        pVALUE = "''";

                        if (pSoftware_VarName[i] != "")
                        {
                            switch (pSoftware_VarName[i])
                            {

                                case "gProject.Product.Bearing.Mat.Base/gProject.Product.Bearing.Mat.Lining":  //Col. F
                                    //-----------------------------------------------------------------------
                                    string pBaseMat = null;
                                    pBaseMat = MatAbbr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base);

                                    string pLiningMat = null;
                                    pLiningMat = MatAbbr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining);

                                    pVALUE = "'" + pBaseMat + "/" + pLiningMat + "'";
                                    break;

                                case "gProject.Product.Bearing.SplitConfig":                                   //Col. G
                                    //--------------------------------------      
                                    string pAny = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                        pAny = "Y";
                                    else
                                        pAny = "N";

                                    pVALUE = "'" + pAny + "'";
                                    break;


                                case "gProject.Product.Bearing.LiningT":                                       //Col. H
                                    //----------------------------------                                           
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT, "") + "'";
                                    break;

                               

                                case "gProject.Product.Bearing.SL.RScrew_Loc.Center":                          //Col. L
                                    //----------------------------------------------                               

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.RScrew_Loc.Front":                           //Col. M
                                    //----------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Front, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.LScrew_Loc.Center":                          //Col. N
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.LScrew_Loc.Front":                           //Col. O
                                    //----------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Front, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Screw_Spec.D_TapDrill":                      //Col. P
                                    //---------------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_TapDrill, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Screw_Spec.D":                               //Col. Q
                                    //------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Screw_Spec.D_Thru":                          //Col. S
                                    //-----------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Thru, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Screw_Spec.D_Cbore":                         //Col. T
                                    //------------------------------------------------           

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_CBore, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Thread_Depth":                               //Col. U
                                    //-------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Thread_Depth, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.CBore_Depth":                                //Col. X 
                                    //-----------------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.CBore_Depth, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Screw_Spec.L":                               //Col. Y
                                    //-----------------------------------------     
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L / 25.4, "") + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L, "") + "'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.SL.RDowel_Loc.Center":                          //Col. BE
                                    //-----------------------------------------------      

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.RDowel_Loc.Front":                           //Col. BF
                                    //----------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Front, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.LDowel_Loc.Center":                          //Col. BG
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.LDowel_Loc.Front":                           //Col. BH
                                    //----------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Front, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Dowel_Spec.D":                               //Col. BI
                                    //------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.SL.Dowel_Depth":                                //Col. BJ
                                    //------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth, "") + "'";
                                    break;

                              

                                case "gProject.Product.Bearing.Loc_Fixture_B()":                               //Col. BN
                                    //------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Loc_Fixture_B(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.DimStart_FrontFace":                            //Col. BR
                                    //---------------------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DimStart_FrontFace, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Dfit()":                                        //Col. BS
                                    //---------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.L":                                             //Col. BT
                                    //---------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).L, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Annulus.Exists":                       //Col. BU
                                    //--------------------------------------------------     
                                    string pAnnulus_Exists = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
                                        pAnnulus_Exists = "Y";
                                    else
                                        pAnnulus_Exists = "N";

                                    pVALUE = "'" + pAnnulus_Exists + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Annulus.Loc_Back":                     //Col. BV
                                    //----------------------------------------------------  
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back, "") + "'";
                                       
                                    }
                                    else
                                    {
                                        pVALUE = "'0'";
                                       
                                    }
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Annulus.D":                            //Col. BW
                                    //---------------------------------------------  

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Annulus.L":                            //Col. BX
                                    //---------------------------------------------  

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.L, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.DFit_EndConfig(0)":                       //Col. BY
                                    //------------------------------------------------    

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(0), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Depth_EndConfig[0]":                            //Col. BZ
                                    //---------------------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[0], "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Depth_EndConfig[1]":                            //Col. CA
                                    //---------------------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndConfig[1], "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Flange.Exists":                                 //Col. CB 
                                    //----------------------------------------     

                                    string pFlange_Exists = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists)
                                        pFlange_Exists = "Y";
                                    else
                                        pFlange_Exists = "N";

                                    pVALUE = "'" + pFlange_Exists + "'";

                                    break;

                                case "gProject.Product.Bearing.Flange.D":                                      //Col. CC
                                    //-----------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Flange.Wid":                                    //Col. CD
                                    //--------------------------------------    

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Flange.DimStart_Front":                         //Col. CE
                                    //------------------------------------------------- 

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Front, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Orifice.D":                            //Col. CG
                                    //--------------------------------------------- 

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.DDrill_CBore":                         //Col. CH
                                    //------------------------------------------------  

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.DDrill_CBore, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.LocFeedHole_FrontFace":                //Col. CI
                                    //--------------------------------------------------------- 

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_FrontFace, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Orifice.StartPos":                     //Col. CJ
                                    //---------------------------------------------------- 
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above)
                                    {
                                        pVALUE = "'A'";
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below)
                                    {
                                        pVALUE = "'B'";
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On)
                                    {
                                        pVALUE = "'O'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.OilInlet.Orifice.L":                            //Col. CK           
                                    //---------------------------------------------     

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.AngStart_Feed_BDV":                    //Col. CL
                                    //----------------------------------------------------- 

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.AngStart_BDV, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Orifice_Exists_2ndSet()":              //Col. CN
                                    //----------------------------------------------------------- 

                                    string pOrifice_Exists_2ndSet = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice_Exists_2ndSet())
                                        pOrifice_Exists_2ndSet = "Y";
                                    else
                                        pOrifice_Exists_2ndSet = "N";

                                    pVALUE = "'" + pOrifice_Exists_2ndSet + "'";
                                    break;

                                case "gProject.Product.Bearing.OilInlet.Dist_FeedHoles":                       //Col. CO
                                    //---------------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Dist_Holes, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Holes_GoThru":                            //Col. DA
                                    //---------------------------------------------       
                                    string pMount_Holes_GoThru = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                        pMount_Holes_GoThru = "Y";
                                    else
                                        pMount_Holes_GoThru = "N";

                                    pVALUE = "'" + pMount_Holes_GoThru + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].Screw_Spec.D_Thru":            //Col. DB
                                    //-------------------------------------------------------------     
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Thru, "") + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_TapDrill, "") + "'";
                                    }
                                    
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].Screw_Spec.D_Thru":            //Col. DC
                                    //--------------------------------------------------------------    
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_Thru, "") + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D_TapDrill, "") + "'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].Screw_Spec.D":                 //Col. DD
                                    //-------------------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].Screw_Spec.D":                 //Col. DE
                                    //-------------------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].Screw_Spec.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Holes_Thread_Depth[0]":                   //Col. DF
                                    //-------------------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0], "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Holes_Thread_Depth[1]":                   //Col. DG
                                    //-------------------------------------------------------      

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1], "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesCount":                   //Col. DJ
                                    //-------------------------------------------------------       

                                    pVALUE = "'" + modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount) + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngStart":                //Col. DK
                                    //-------------------------------------------------------       

                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart.ToString("#0.#") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[0]":             //Col. DL
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther0 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 1)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther0 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[0], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther0 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther0 + "'";
                                    }
                                    
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[1]":             //Col. DM
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther1 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 2)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther1 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[1], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther1 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther1 + "'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[2]":             //Col. DN
                                    //-----------------------------------------------------------       

                                    string pMount_Fixture_HolesAngOther2 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 3)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther2 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[2], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther2 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther2 + "'";

                                    }
                                  
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[3]":             //Col. DO
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther3 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 4)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther3 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[3], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther3 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther3 + "'";
                                    }
                                                                       
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[4]":             //Col. DP
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther4 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 5)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther4 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[4], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther4 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther4 + "'";
                                    }
                                                                        
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[5]":             //Col. DQ
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther5= null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 6)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther5 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[5], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther5 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther5 + "'";
                                       
                                    }
                                                                        
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].HolesAngOther[6]":             //Col. DR
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther6 = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 7)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther6 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[6], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther6 = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther6 + "'";
                                    }                                   

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[0].DBC":                          //Col. DS          
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Angle":                          //Col. DT
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Angle, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Bearing_Vert":                   //Col. DU 
                                    //------------------------------------------------------                                             
                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString() + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Bearing_SL()":                   //Col. DV
                                    //------------------------------------------------------       
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top)
                                    {
                                        pVALUE = "'T'";
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom)
                                    {
                                        pVALUE = "'B'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Casing_SL":                      //Col. DW
                                    //---------------------------------------------------       
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Center)
                                    {
                                        pVALUE = "'C'";
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset)
                                    {
                                        pVALUE = "'O'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Dist_Front":                     //Col. DX
                                    //----------------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Dist_Front, "") + "'";
                                    break;


                                case "gProject.Product.Bearing.AntiRotPin.Spec.D":                             //Col. DY  
                                    //---------------------------------------------    

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Depth":                              //Col. DZ
                                    //--------------------------------------------      

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Depth, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Stickout":                           //Col. EA
                                    //----------------------------------------------    

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Stickout, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.Loc_Offset":                         //Col. EB
                                    //------------------------------------------------    

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Offset, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.MillRelief.Exists":                             //Col. EF
                                    //----------------------------------------------    

                                    string pMillRelief_Exists = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists)
                                        pMillRelief_Exists = "Y";
                                    else
                                        pMillRelief_Exists = "N";

                                    pVALUE = "'" + pMillRelief_Exists + "'";
                                    break;

                                case "gProject.Product.Bearing.MillRelief.D()":                                //Col. EG
                                    //-----------------------------------------    
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D(), "") + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "0.125";
                                    }
                                    break;

                                case "gProject.Product.Bearing.TempSensor.Loc":                                //Col. EK
                                    //-----------------------------------------        
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pVALUE = "'F'";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pVALUE = "'B'";
                                        }
                                    }
                                    else
                                    {
                                        pVALUE = "'N'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.TempSensor.AngStart":                           //Col. EL
                                    //----------------------------------------------    
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart, "") + "'";
                                    }      
                                    break;

                                case "gProject.Product.Bearing.TempSensor.D":                                  //Col. EM
                                    //---------------------------------------    
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D, "") + "'";
                                    }          
                                    break;

                                case "gProject.Product.Bearing.TempSensor.CanLength":                           //Col. EO
                                    //----------------------------------------------    
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength, "") + "'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.TempSensor.Depth":                              //Col. EP
                                    //-------------------------------------------   
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth, "") + "'";
                                    }                                   
                                    break;

                                case "gProject.Product.Bearing.TempSensor.Count":                              //Col. EQ
                                    //-------------------------------------------  
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        pVALUE = "'" + modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count) + "'";
                                    }                                   
                                    break;

                                case "gOpCond.LoadAng":                                                        //Col. FU
                                    //-----------------   
                                    //....Hard coded to 90 and KMC will manually put a value here. When in the future version
                                    //........we add the utility program, this value will be calculated by our program.
                                    pVALUE = "'90'";
                                    break;

                                case "gProject.Product.Bearing.Pad.Type":                                      //Col. FV
                                    //-----------------------------------   

                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString() + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.Count":                                     //Col. FW
                                    //-----------------------------------   

                                    pVALUE = "'" + modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count) + "'";
                                    break;

                                case "gOpCond.Rot_Direction":                                                  //Col. FX
                                    //------------------------   
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset == 50.0)
                                    {
                                        pVALUE = "'BI'";
                                    }
                                    else
                                    {
                                        pVALUE = "'CCW'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.Pad.Pivot.Offset":                              //Col. FY
                                    //-------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.DSet()":                                        //Col. FZ
                                    //---------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.DPad();":                                       //Col. GA
                                    //----------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.FlexurePivot.Web.T":                            //Col. GB
                                    //---------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.FlexurePivot.Web.RFillet":                      //Col. GC
                                    //---------------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.FlexurePivot.Web.H":                            //Col. GD
                                    //---------------------------------------------  

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.T.Lead":                                    //Col. GE
                                    //-------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.T.Pivot":                                   //Col. GF
                                    //--------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.T.Trail":                                   //Col. GG
                                    //--------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.RFillet_ID()":
                                    //-------------------------------------------                              //Col. GH

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.RFillet_ID(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.FlexurePivot.GapEDM":                           //Col. GI
                                    //---------------------------------------------   

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.Angle()":                                   //Col. GL
                                    //--------------------------------------  

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Angle(), "") + "'";
                                    break;

                                case "gProject.Product.Bearing.EDM_Pad.RFillet_Back":                          //Col. GM
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.RFillet_Back, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.EDM_Pad.Ang_Offset()":                          //Col. GN
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.Ang_Offset(OpCond_In), "#0.#") + "'";
                                    break;

                                case "gProject.Product.Bearing.EDM_Pad.AngStart_Web":                          //Col. GP
                                    //-----------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Pad.AngStart_Web, "") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.DFit_EndConfig[1]":                       //Col. IP
                                    //--------------------------------------------------        

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(1), "#0.0000") + "'";
                                    break;

                                case "gProject.Product.Bearing.Tol_Detail_DSet()":                             //Col. IQ
                                    //--------------------------------------------        

                                    pVALUE = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Tol_Detail_DSet(), "");
                                    pVALUE = "'LIMIT;-" + pVALUE + ";" + pVALUE + "'";
                                    break;

                                case "gProject.Product.Bearing.AntiRotPin.InsertedOn":                         //Col. IR 
                                    //-----------------------------------------------        

                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.InsertedOn.ToString() + "'";

                                    break;

                                    //BG 10JAN13
                                case "gProject.Product.Bearing.MillRelief.EDM_Relief[0]":    //"gProject.Product.Bearing.DESIGN_EDM_RELIEF1":                            //Col. IT
                                    //---------------------------------------------------           
                                    //pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[0], "#0.000") + "'";     //BG 06DEC12
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[0], "#0.000") + "'";       //BG 06DEC12
                                    break;

                                    //BG 10JAN13
                                case "gProject.Product.Bearing.MillRelief.EDM_Relief[1]":                            //Col. IU
                                    //---------------------------------------------------        
                                    //pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).EDM_Relief[1], "#0.000") + "'"; //BG 06DEC12
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.EDM_Relief[1], "#0.000") + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngStart":                //Col. IY
                                    //-------------------------------------------------------       

                                    pVALUE = "'" +((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart.ToString("#0.#") + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[0]":             //Col. IZ
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther0_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 1)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther0_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[0], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther0_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther0_Back + "'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[1]":             //Col. JA
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther1_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 2)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther1_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[1], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther1_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther1_Back + "'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[2]":             //Col. JB
                                    //-----------------------------------------------------------       

                                    string pMount_Fixture_HolesAngOther2_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 3)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther2_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[2], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther2_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther2_Back + "'";

                                    }

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[3]":             //Col. JC
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther3_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 4)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther3_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[3], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther3_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther3_Back + "'";
                                    }

                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[4]":             //Col. JD
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther4_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 5)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther4_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[4], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther4_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther4_Back + "'";
                                    }
                                                                        
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[5]":             //Col. JE
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther5_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 6)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther5_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[5], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther5_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther5_Back + "'";

                                    }
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].HolesAngOther[6]":             //Col. JF
                                    //-----------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther6_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 7)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther6_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[6], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther6_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }

                                        pVALUE = "'" + pMount_Fixture_HolesAngOther6_Back + "'";
                                    }
                                                                        
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[1].DBC":                          //Col. JG          
                                    //-----------------------------------------------       

                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC, "") + "'";
                                    break;
                            }

                                pWHERE = " WHERE fldCellColName = '" + pCellColName[i] + "'";
                                pSQL = pUPDATE + pSET + pVALUE + pWHERE;
                                DB_In.ExecuteCommand(pSQL);
                           
                        }
                    }

                }
                catch (Exception pEXP)
                {
                    MessageBox.Show("Database Population Error - " + pEXP.Message);
                }
            }

        #endregion


        #region "DESIGN TABLE POPULATION:"

            private void PopulateAndMove_DT_Radial (clsFiles Files_In, clsProject Project_In, clsDB DB_In, string FilePath_In)
            //================================================================================================================       
            {
                try
                {
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_Radial, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open 'Sketchs' WorkSheets.
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[1];

                    //....Set Project Number.
                    int pNo_Suffix = 1;
                    if (Project_In.AssyDwg.No_Suffix != "")
                    {
                        pNo_Suffix = Convert.ToInt32(Project_In.AssyDwg.No_Suffix);
                    }
                    if (pNo_Suffix <= 9)
                    {
                        pWkSheet.Cells[3, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 1).ToString();//"-02";
                        pWkSheet.Cells[4, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 1).ToString() + "T";//"-02T";
                        pWkSheet.Cells[5, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 4).ToString();//"-05";
                    }
                    else
                    {
                        pWkSheet.Cells[3, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 1).ToString();//"-02";
                        pWkSheet.Cells[4, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 1).ToString() + "T";//"-02T";
                        pWkSheet.Cells[5, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 4).ToString();//"-05";
                    }


                    //.....Set Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_Var_Val = new StringCollection();

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Radial", "fldCellColName", "");
                    DB_In.PopulateStringCol(pSoftware_Var_Val, "tblMapping_Radial", "fldSoftware_VarVal", "");

                    //for (int i = 2; i < pCellColName.Count; i++)      //BG 20JUN11
                    for (int i = 0; i < pCellColName.Count; i++)        //BG 20JUN11
                    {
                        for (int j = 3; j < 6; j++)
                        {
                            if (pSoftware_Var_Val[i] != "")
                            {
                                int pIndx = ColumnNumber(pCellColName[i]);
                                pWkSheet.Cells[j, pIndx] = pSoftware_Var_Val[i];
                            }
                        }
                    }

                    //private const string mcSWRadial_Title = "RADIAL BEARING.SLDPRT";
                    string pFile = modMain.ExtractPreData(mcRadial_Title, ".");
                    String pFileName = FilePath_In + "\\" + pFile + ".xlsx";

                    if (!Directory.Exists(FilePath_In))
                        Directory.CreateDirectory(FilePath_In);

                    if (File.Exists(pFileName))
                        File.Delete(pFileName);


                    //....Office 2003.
                    //pWkbOrg.SaveAs(pFileName, pWkbOrg.FileFormat, mobjMissing,
                    //                    mobjMissing, false, mobjMissing, pAccessMode,
                    //                    mobjMissing, mobjMissing, mobjMissing,
                    //                    mobjMissing, mobjMissing);

                    //....Office 2010.
                    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
                    pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing, mobjMissing, pAccessMode,
                                        mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing);

                    //pApp.Visible = true;

                    if (FilePath_In.Contains("Ref. Files"))
                    {
                        pApp.Visible = false;
                    }
                    else
                    {
                        pApp.Visible = true;
                    }
                }

                catch (Exception pEXP)
                {
                    MessageBox.Show("Excel File Error - " + pEXP.Message);
                }
            }

        #endregion
    }
}
