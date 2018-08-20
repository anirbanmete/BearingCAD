//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models_3_SEAL                    '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  28JAN13                                '
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

        #region "DATABASE MAPPING:"

            private void Populate_tblMapping_Seal(clsProject Project_In, clsDB DB_In)
            //=======================================================================
            {
                clsSeal[] mEndSeal = new clsSeal[2];
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Seal)
                    {
                        mEndSeal[i] = (clsSeal)((clsSeal)(Project_In.Product.EndConfig[i])).Clone();
                    }
                }

                try
                {
                    //.....Set Maping Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_VarName = new StringCollection();

                    string pOrderBy = " ORDER BY fldItemNo ASC";

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Seal", "fldCellColName", pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_Seal", "fldSoftware_VarName", pOrderBy);

                    String pUPDATE = "UPDATE tblMapping_Seal ";
                    String pSET = "SET fldSoftware_VarVal = ";
                    String pVALUE = null;
                    String pWHERE = null;
                    String pSQL = null;

                    pSQL = pUPDATE + pSET + "NULL";

                    DB_In.ExecuteCommand(pSQL);

                    int i = 0, ii = 0;

                    for (i = 0; i < pSoftware_VarName.Count; i++)
                    {
                        pSQL = null;
                        pWHERE = null;
                        pVALUE = null;

                        if (pSoftware_VarName[i] != "")
                        {
                            switch (pSoftware_VarName[i])
                            {
                                case "gProject.Product.EndConfig[i].Mat.Base":              //Col. D              
                                    //---------------------------------------                       
                                    string pBaseMat_Front = null; string pBaseMat_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                        pBaseMat_Front = MatAbbr(mEndSeal[0].Mat.Base);

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                        pBaseMat_Back = MatAbbr(mEndSeal[1].Mat.Base);

                                    string pBaseMat = pBaseMat_Front + ", " + pBaseMat_Back;

                                    pVALUE = "'" + pBaseMat + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Blade.Count":           //Col. E
                                    //-------------------------------------------       
                                    string pBlade_Count_Front = null;
                                    string pBlade_Count_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                            pBlade_Count_Front = "S";
                                        else
                                            pBlade_Count_Front = "D";
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                            pBlade_Count_Back = "S";
                                        else
                                            pBlade_Count_Back = "D";
                                    }

                                    string pBlade_Count = pBlade_Count_Front + " ," + pBlade_Count_Back;

                                    pVALUE = "'" + pBlade_Count + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].D_Finish":  //Col. G
                                    //----------------------------------------------------                                       
                                    string pDO = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].D_Finish, "") + ", " +
                                                      modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].D_Finish, "");

                                    pVALUE = "'" + pDO + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].DBore()":               //Col. H
                                    //----------------------------------------      
                                    string pDBore_Front = null, pDBore_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pDBore_Front = modMain.ConvDoubleToStr(mEndSeal[0].DBore(), "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pDBore_Back = modMain.ConvDoubleToStr(mEndSeal[1].DBore(), "");
                                    }


                                    pVALUE = "'" + pDBore_Front + ", " + pDBore_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].L":                     //Col. I
                                    //---------------------------------         
                                    string pL_Front = null, pL_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pL_Front = modMain.ConvDoubleToStr(mEndSeal[0].L, "");
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pL_Back = modMain.ConvDoubleToStr(mEndSeal[1].L, "");
                                    }

                                    pVALUE = "'" + pL_Front + ", " + pL_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].DrainHoles.Annulus.D":  //Col. J 
                                    //-----------------------------------------------------     
                                    string pAnnulus_D_Front = null, pAnnulus_D_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                        {
                                            pAnnulus_D_Front = modMain.ConvDoubleToStr(mEndSeal[0].DBore() + 0.25, "");
                                        }
                                        else
                                        {
                                            pAnnulus_D_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.Annulus.D, "");
                                        }
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pAnnulus_D_Back = modMain.ConvDoubleToStr(mEndSeal[1].DBore() + 0.25, "");
                                        }
                                        else
                                        {
                                            pAnnulus_D_Back = modMain.ConvDoubleToStr(mEndSeal[1].DrainHoles.Annulus.D, "");
                                        }
                                    }

                                    pVALUE = "'" + pAnnulus_D_Front + ", " + pAnnulus_D_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Blade.T":               //Col. K
                                    //----------------------------------------          
                                    string pBlade_T_Front = null, pBlade_T_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pBlade_T_Front = modMain.ConvDoubleToStr(mEndSeal[0].Blade.T, "");
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pBlade_T_Back = modMain.ConvDoubleToStr(mEndSeal[1].Blade.T, "");
                                    }

                                    pVALUE = "'" + pBlade_T_Front + ", " + pBlade_T_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Blade.AngTaper":         //Col. L
                                    //-----------------------------------------------      
                                    string pBlade_AngTaper_Front = null, pBlade_AngTaper_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pBlade_AngTaper_Front = modMain.ConvDoubleToStr(mEndSeal[0].Blade.AngTaper, "");
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pBlade_AngTaper_Back = modMain.ConvDoubleToStr(mEndSeal[1].Blade.AngTaper, "");
                                    }

                                    pVALUE = "'" + pBlade_AngTaper_Front + ", " + pBlade_AngTaper_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Type":       //Col. O
                                    //-----------------------------------------------       
                                    string pMountHoles_Type_Front = null, pMountHoles_Type_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMountHoles_Type_Front = mEndSeal[0].MountHoles.Type.ToString();
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMountHoles_Type_Back = mEndSeal[1].MountHoles.Type.ToString();
                                    }

                                    pVALUE = "'" + pMountHoles_Type_Front + ", " + pMountHoles_Type_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D_Thru":  //Col. P
                                    //------------------------------------------------------------   
                                    string pMountHoles_D_Thru_Front = null, pMountHoles_D_Thru_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pMountHoles_D_Thru_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_TapDrill, "");
                                        }
                                        else
                                        {
                                            pMountHoles_D_Thru_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_Thru, "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pMountHoles_D_Thru_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_TapDrill, "");
                                        }
                                        else
                                        {
                                            pMountHoles_D_Thru_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_Thru, "");
                                        }
                                    }

                                    pVALUE = "'" + pMountHoles_D_Thru_Front + ", " + pMountHoles_D_Thru_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D":   //Col. Q
                                    //-------------------------------------------------------       
                                    string pMountHoles_Screw_Spec_D_Front = null, pMountHoles_Screw_Spec_D_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMountHoles_Screw_Spec_D_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMountHoles_Screw_Spec_D_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D, "");
                                    }

                                    pVALUE = "'" + pMountHoles_Screw_Spec_D_Front + ", " + pMountHoles_Screw_Spec_D_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D_CBore": //Col. R
                                    //-------------------------------------------------------------     
                                    string pMountHoles_D_CBore_Front = null, pMountHoles_D_CBore_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pMountHoles_D_CBore_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Screw_Spec.D_CBore, "");
                                        }
                                       
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pMountHoles_D_CBore_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Screw_Spec.D_CBore, "");
                                        }                                        
                                    }

                                    pVALUE = "'" + pMountHoles_D_CBore_Front + ", " + pMountHoles_D_CBore_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Depth_CBore":    //Col. S
                                    //-------------------------------------------------------       
                                    string pMountHoles_Depth_CBore_Front = null, pMountHoles_Depth_CBore_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pMountHoles_Depth_CBore_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Depth_CBore, "");
                                        }                                       
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pMountHoles_Depth_CBore_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Depth_CBore, "");
                                        }                                     
                                    }

                                    pVALUE = "'" + pMountHoles_Depth_CBore_Front + ", " + pMountHoles_Depth_CBore_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesCount":        //Col. T
                                    //-------------------------------------------------------       
                                    string pMount_Fixture_HolesCount = modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount) + ", " +
                                                                       modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount);

                                    pVALUE = "'" + pMount_Fixture_HolesCount + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngStart":     //Col. U
                                    //---------------------------------------------------------            
                                    string pMount_Fixture_HolesAngStart = "";
                                    pMount_Fixture_HolesAngStart = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart.ToString("#0.#") + ", " +
                                                                   ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart.ToString("#0.#");
                                  
                                    pVALUE = "'" + pMount_Fixture_HolesAngStart + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[0]":  //Col. V
                                    //------------------------------------------------------------     
                                    string pMount_Fixture_HolesAngOther1_Front = null, pMount_Fixture_HolesAngOther1_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 1)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther1_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[0], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther1_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 1)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther1_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[0], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther1_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther1_Front + ", " + pMount_Fixture_HolesAngOther1_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[1]":  //Col. W
                                    //------------------------------------------------------------ 
                                    string pMount_Fixture_HolesAngOther2_Front = null, pMount_Fixture_HolesAngOther2_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 2)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther2_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[1], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther2_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 2)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther2_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[1], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther2_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther2_Front + ", " + pMount_Fixture_HolesAngOther2_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[2]":  //Col. X
                                    //------------------------------------------------------------     
                                    string pMount_Fixture_HolesAngOther3_Front = null, pMount_Fixture_HolesAngOther3_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 3)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther3_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[2], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther3_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 3)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther3_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[2], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther3_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther3_Front + ", " + pMount_Fixture_HolesAngOther3_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[3]":  //Col. Y
                                    //------------------------------------------------------------       
                                    string pMount_Fixture_HolesAngOther4_Front = null, pMount_Fixture_HolesAngOther4_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 4)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther4_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[3], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther4_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 4)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther4_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[3], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther4_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther4_Front + ", " + pMount_Fixture_HolesAngOther4_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[4]":  //Col. Z 
                                    //------------------------------------------------------------      
                                    string pMount_Fixture_HolesAngOther5_Front = null, pMount_Fixture_HolesAngOther5_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 5)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther5_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[4], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther5_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 5)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther5_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[4], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther5_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther5_Front + ", " + pMount_Fixture_HolesAngOther5_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[5]":  //Col. AA
                                    //------------------------------------------------------------        
                                    string pMount_Fixture_HolesAngOther6_Front = null, pMount_Fixture_HolesAngOther6_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 6)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther6_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[5], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther6_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 6)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther6_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[5], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther6_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther6_Front + ", " + pMount_Fixture_HolesAngOther6_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[6]":  //Col. AB
                                    //------------------------------------------------------------            
                                    string pMount_Fixture_HolesAngOther7_Front = null, pMount_Fixture_HolesAngOther7_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount > 7)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther7_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngOther[6], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther7_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(0), "");
                                        }
                                    }

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount > 7)
                                    {
                                        if (!((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesEquiSpaced)
                                        {
                                            pMount_Fixture_HolesAngOther7_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngOther[6], "");
                                        }
                                        else
                                        {
                                            pMount_Fixture_HolesAngOther7_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.MountFixture_Sel_AngOther(1), "");
                                        }
                                    }

                                    pVALUE = "'" + pMount_Fixture_HolesAngOther7_Front + ", " + pMount_Fixture_HolesAngOther7_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].DBC":               //Col. AC
                                    //-----------------------------------------------     
                                    string pMount_Fixture_DBC = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC, "") + ", " +
                                                                          modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC, "");

                                    pVALUE = "'" + pMount_Fixture_DBC + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[i].PartNo":            //Col. AD 
                                    //-------------------------------------------------- 
                                    string[] pMount_Fixture_PartNo_Comp = new string[2];
                                
                                  if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                                      Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {

                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo !=
                                            ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo)
                                        {
                                            pMount_Fixture_PartNo_Comp[0] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo))[0];
                                            pMount_Fixture_PartNo_Comp[1] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo))[0];
                                        }
                                        else
                                        {
                                            pMount_Fixture_PartNo_Comp[0] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo))[0];

                                            if (FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo))[1] != "")
                                            {
                                              pMount_Fixture_PartNo_Comp[1] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo))[1];
                                            }
                                        }                                       
                                    }
                                    else if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMount_Fixture_PartNo_Comp[0] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo))[0];
                                    }
                                    else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        pMount_Fixture_PartNo_Comp[1] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo))[0];
                                    }

                                    pVALUE = "'" + pMount_Fixture_PartNo_Comp[0] + "," + pMount_Fixture_PartNo_Comp[1] + "'";

                                    break;

                                case "gProject.Product.EndConfig[i].DrainHoles.AngStart_FrontMounted":  //Col. AE    
                                    //----------------------------------------------------------------        
                                    string pDrainHoles_AngStart_Front = null, pDrainHoles_AngStart_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngStart_Front = "0";
                                            pDrainHoles_AngStart_Back = "0";
                                        }
                                        else
                                        {
                                            pDrainHoles_AngStart_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngStart, "");
                                            pDrainHoles_AngStart_Back = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngStart_OtherSide (), "");
                                        }

                                    }

                                    else if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngStart_Front = "0";// null;
                                        }
                                        else
                                        {
                                            pDrainHoles_AngStart_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngStart, "");
                                        }
                                    }

                                    else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngStart_Back = "0";
                                        }
                                        else
                                        {
                                            pDrainHoles_AngStart_Back = modMain.ConvDoubleToStr(mEndSeal[1].DrainHoles.AngStart, "");
                                        }
                                    }
                                  
                                    pVALUE = "'" + pDrainHoles_AngStart_Front + ", " + pDrainHoles_AngStart_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].DrainHoles.D":                  //Col. AG
                                    //--------------------------------------------        
                                    string pDrainHoles_D_Front = null, pDrainHoles_D_Front_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                        {
                                            pDrainHoles_D_Front = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_D_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.D(), "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pDrainHoles_D_Front_Back = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_D_Front_Back = modMain.ConvDoubleToStr(mEndSeal[1].DrainHoles.D(), "");
                                        }
                                    }

                                    pVALUE = "'" + pDrainHoles_D_Front + ", " + pDrainHoles_D_Front_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].DrainHoles.AngExit":             //Col. AH
                                    //--------------------------------------------------     
                                    string pDrainHoles_AngExit_Front = null, pDrainHoles_AngExit_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngExit_Front = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_AngExit_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngExit, "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngExit_Back = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_AngExit_Back = modMain.ConvDoubleToStr(mEndSeal[1].DrainHoles.AngExit, "");
                                        }
                                    }

                                    pVALUE = "'" + pDrainHoles_AngExit_Front + ", " + pDrainHoles_AngExit_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].DrainHoles.Count":               //Col. AI
                                    //--------------------------------------------------  
                                    //string pDrainHoles_Count_Front = null, pDrainHoles_Count_Back = null;

                                    //if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    if (mEndSeal[0].Blade.Count == 1)
                                    //    {
                                    //        pDrainHoles_Count_Front = null;
                                    //    }
                                    //    else
                                    //    {
                                    //        pDrainHoles_Count_Front = modMain.ConvIntToStr(mEndSeal[0].DrainHoles.Count);
                                    //    }
                                    //}

                                    //if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    if (mEndSeal[1].Blade.Count == 1)
                                    //    {
                                    //        pDrainHoles_Count_Back = null;
                                    //    }
                                    //    else
                                    //    {
                                    //        pDrainHoles_Count_Back = modMain.ConvIntToStr(mEndSeal[1].DrainHoles.Count);
                                    //    }
                                    //}

                                    //pVALUE = "'" + pDrainHoles_Count_Front + ", " + pDrainHoles_Count_Back + "'";

                                    string[] pDrainHoles_Count = new string[2]{null, null};

                                    for (ii = 0; ii < 2; ii++)
                                    {
                                        if (mEndSeal[ii] != null)           //BG 21JAN13
                                        {
                                            if (mEndSeal[ii].Blade.Count > 1)
                                            {
                                                pDrainHoles_Count[ii] = modMain.ConvIntToStr(mEndSeal[ii].DrainHoles.Count);
                                            }
                                        }
                                    }

                                    pVALUE = "'" + pDrainHoles_Count[0] + ", " + pDrainHoles_Count [1] + "'";

                                  
                                    //if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    if (mEndSeal[0].Blade.Count == 1)
                                    //    {
                                    //        pDrainHoles_Count_Front = null;
                                    //    }
                                    //    else
                                    //    {
                                    //        pDrainHoles_Count_Front = modMain.ConvIntToStr(mEndSeal[0].DrainHoles.Count);
                                    //    }
                                    //}

                                    //if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    //{
                                    //    if (mEndSeal[1].Blade.Count == 1)
                                    //    {
                                    //        pDrainHoles_Count_Back = null;
                                    //    }
                                    //    else
                                    //    {
                                    //        pDrainHoles_Count_Back = modMain.ConvIntToStr(mEndSeal[1].DrainHoles.Count);
                                    //    }
                                    //}

                                    //pVALUE = "'" + pDrainHoles_Count_Front + ", " + pDrainHoles_Count_Back + "'";

                                    break;


                                case "gProject.Product.EndConfig[i].DrainHoles.AngBet":             //Col. AJ
                                    //-------------------------------------------------       
                                    string pDrainHoles_AngBet_Front = null, pDrainHoles_AngBet_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[0].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngBet_Front = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_AngBet_Front = modMain.ConvDoubleToStr(mEndSeal[0].DrainHoles.AngBet, "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].Blade.Count == 1)
                                        {
                                            pDrainHoles_AngBet_Back = null;
                                        }
                                        else
                                        {
                                            pDrainHoles_AngBet_Back = modMain.ConvDoubleToStr(mEndSeal[1].DrainHoles.AngBet, "");
                                        }
                                    }
                                    pVALUE = "'" + pDrainHoles_AngBet_Front + ", " + pDrainHoles_AngBet_Back + "'";

                                    break;


                                case "gProject.Product.Bearing.TempSensor.Exists":                  //Col. AK
                                    //--------------------------------------------     
                                    string pTempSensor_Exists_Front = null, pTempSensor_Exists_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pTempSensor_Exists_Front = "Y";
                                            pTempSensor_Exists_Back = "N";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pTempSensor_Exists_Front = "N";
                                            pTempSensor_Exists_Back = "Y";
                                        }
                                    }

                                    else
                                    {
                                        pTempSensor_Exists_Front = "N";
                                        pTempSensor_Exists_Back = "N";
                                    }

                                    pVALUE = "'" + pTempSensor_Exists_Front + ", " + pTempSensor_Exists_Back + "'";
                                    break;

                                case "gProject.Product.Bearing.TempSensor.D":                       //Col. AL
                                    //---------------------------------------     
                                    string pTempSensor_D_Front = null, pTempSensor_D_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pTempSensor_D_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D, "");
                                            pTempSensor_D_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pTempSensor_D_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D, "");
                                            pTempSensor_D_Front = "";
                                        }
                                      
                                    }
                                    else
                                    {
                                        pTempSensor_D_Front = "";
                                        pTempSensor_D_Back = "";
                                    }

                                    pVALUE = "'" + pTempSensor_D_Front + "," + pTempSensor_D_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].TempSensor_DBC_Hole()":          //Col. AM
                                    //-----------------------------------------------------      
                                    string pTempSensor_DBC_Hole_Front = null, pTempSensor_DBC_Hole_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                            {
                                                pTempSensor_DBC_Hole_Front = modMain.ConvDoubleToStr(mEndSeal[0].TempSensor_DBC_Hole(), "");
                                                pTempSensor_DBC_Hole_Back = "";
                                            }
                                        }
                                        else
                                        {
                                            pTempSensor_DBC_Hole_Front = "";
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                            {
                                                pTempSensor_DBC_Hole_Back = modMain.ConvDoubleToStr(mEndSeal[1].TempSensor_DBC_Hole(), "");
                                                pTempSensor_DBC_Hole_Front = "";
                                            }
                                        }
                                        else
                                        {
                                            pTempSensor_DBC_Hole_Back = "";
                                        }
                                    }
                                    pVALUE = "'" + pTempSensor_DBC_Hole_Front + ", " + pTempSensor_DBC_Hole_Back + "'";

                                    break;


                                case "gProject.Product.Bearing.TempSensor.AngStart":                //Col. AN
                                    //----------------------------------------------      
                                    string pTempSensor_AngStart_Front = null, pTempSensor_AngStart_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pTempSensor_AngStart_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart, "");
                                            pTempSensor_AngStart_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pTempSensor_AngStart_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart, "");
                                            pTempSensor_AngStart_Front = "";
                                        }
                                                                                                       
                                    }
                                    else
                                    {
                                        pTempSensor_AngStart_Front = "";
                                        pTempSensor_AngStart_Back = "";
                                    }

                                    pVALUE = "'" + pTempSensor_AngStart_Front + "," + pTempSensor_AngStart_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].TempSensor_D_ExitHole":         //Col. AO
                                    //-----------------------------------------------------        
                                    string pTempSensor_D_ExitHole_Front = null, pTempSensor_D_ExitHole_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                            {
                                                pTempSensor_D_ExitHole_Front = modMain.ConvDoubleToStr(mEndSeal[0].TempSensor_D_ExitHole, "");
                                                pTempSensor_D_ExitHole_Back = "";
                                            }
                                        }
                                        else
                                        {
                                            pTempSensor_D_ExitHole_Front = "";
                                        }

                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                            {
                                                pTempSensor_D_ExitHole_Back = modMain.ConvDoubleToStr(mEndSeal[1].TempSensor_D_ExitHole, "");
                                                pTempSensor_D_ExitHole_Front = "";
                                            }
                                        }
                                        else
                                        {
                                            pTempSensor_D_ExitHole_Back = "";
                                        }
                                    }
                                    pVALUE = "'" + pTempSensor_D_ExitHole_Front + ", " + pTempSensor_D_ExitHole_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.TempSensor.Count":                   //Col. AP
                                    //-------------------------------------------       
                                    string pTempSensor_Count_Front = null, pTempSensor_Count_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pTempSensor_Count_Front = modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count);
                                            pTempSensor_Count_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pTempSensor_Count_Back = modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count);
                                            pTempSensor_Count_Front = "";
                                        }

                                    }
                                    else
                                    {
                                        pTempSensor_Count_Front = "";
                                        pTempSensor_Count_Back = "";
                                    }

                                    pVALUE = "'" + pTempSensor_Count_Front + "," + pTempSensor_Count_Back + "'";
                                    break;

                                case "gProject.Product.Bearing.Pad.AngBetween()":                   //Col. AQ
                                    //-------------------------------------------          
                                    string pTempSensor_Pad_Ang_Between_Front = null, pTempSensor_Pad_Ang_Between_Back = null;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pTempSensor_Pad_Ang_Between_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.AngBetween(), "");
                                            pTempSensor_Pad_Ang_Between_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pTempSensor_Pad_Ang_Between_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.AngBetween(), "");
                                            pTempSensor_Pad_Ang_Between_Front = "";
                                        }
                                                                             
                                    }
                                    else
                                    {
                                        pTempSensor_Pad_Ang_Between_Front = "";
                                        pTempSensor_Pad_Ang_Between_Back = "";
                                    }

                                    pVALUE = "'" + pTempSensor_Pad_Ang_Between_Front + "," + pTempSensor_Pad_Ang_Between_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.Exists":          //Col. AR
                                    //----------------------------------------------------         
                                    string pWireClip_Exists_Front = "", pWireClip_Exists_Back = "";

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClip_Exists_Front = "Y";
                                            pWireClip_Exists_Back = "N";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClip_Exists_Front = "N";
                                            pWireClip_Exists_Back = "Y";
                                        }
                                    }

                                    else
                                    {
                                        pWireClip_Exists_Front = "N";
                                        pWireClip_Exists_Back = "N";
                                    }

                                    pVALUE = "'" + pWireClip_Exists_Front + ", " + pWireClip_Exists_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.AngStart":        //Col. AS
                                    //------------------------------------------------------      
                                    string pWireClipHoles_AngStart_Front = null, pWireClipHoles_AngStart_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_AngStart_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngStart, "");
                                            pWireClipHoles_AngStart_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_AngStart_Front = "";
                                            pWireClipHoles_AngStart_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngStart, "");
                                        }
                                    }

                                    else
                                    {
                                        pWireClipHoles_AngStart_Front = "";
                                        pWireClipHoles_AngStart_Back = "";
                                    }

                                    pVALUE = "'" + pWireClipHoles_AngStart_Front + ", " + pWireClipHoles_AngStart_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.Count2":          //Col. AT
                                    //----------------------------------------------------            
                                    string pWireClipHoles2_Exists_Front = null;
                                    string pWireClipHoles2_Exists_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            if (mEndSeal[0].WireClipHoles.Count > 1)
                                                pWireClipHoles2_Exists_Front = "Y";

                                            else
                                                pWireClipHoles2_Exists_Front = "N";

                                            pWireClipHoles2_Exists_Back = "N";
                                        }

                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            if (mEndSeal[1].WireClipHoles.Count > 1)
                                                pWireClipHoles2_Exists_Back = "Y";

                                            else
                                                pWireClipHoles2_Exists_Back = "N";

                                            pWireClipHoles2_Exists_Front = "N";
                                        }

                                    }
                                    else
                                    {
                                        pWireClipHoles2_Exists_Front = "N";
                                        pWireClipHoles2_Exists_Back = "N";
                                    }

                                    pVALUE = "'" + pWireClipHoles2_Exists_Front + ", " + pWireClipHoles2_Exists_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WireClipHoles.AngOther[0]":      //Col. AU
                                    //---------------------------------------------------------       
                                    string pWireClipHoles_AngOther0_Front = null, pWireClipHoles_AngOther0_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_AngOther0_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngOther[0], "");
                                            pWireClipHoles_AngOther0_Back = "";
                                        }

                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_AngOther0_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngOther[0], "");
                                            pWireClipHoles_AngOther0_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles_AngOther0_Front = "";
                                        pWireClipHoles_AngOther0_Back = "";
                                    }
                                
                                    pVALUE = "'" + pWireClipHoles_AngOther0_Front + ", " + pWireClipHoles_AngOther0_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WireClipHoles.Count3":          //Col. AW
                                    //----------------------------------------------------        
                                    string pWireClipHoles3_Exists_Front = null;
                                    string pWireClipHoles3_Exists_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            if (mEndSeal[0].WireClipHoles.Count > 2)
                                                pWireClipHoles3_Exists_Front = "Y";
                                         
                                            else
                                                pWireClipHoles3_Exists_Front = "N";

                                            pWireClipHoles3_Exists_Back = "N";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            if (mEndSeal[1].WireClipHoles.Count > 2)
                                                pWireClipHoles3_Exists_Back = "Y";
                                         
                                            else
                                                pWireClipHoles3_Exists_Back = "N";

                                            pWireClipHoles3_Exists_Front = "N";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles3_Exists_Front = "N";
                                        pWireClipHoles3_Exists_Back = "N";
                                    }

                                    pVALUE = "'" + pWireClipHoles3_Exists_Front + ", " + pWireClipHoles3_Exists_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WireClipHoles.AngOther[1]":     //Col. AX
                                    //---------------------------------------------------------        
                                    string pWireClipHoles_AngOther1_Front = null, pWireClipHoles_AngOther1_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_AngOther1_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.AngOther[1], "");
                                            pWireClipHoles_AngOther1_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_AngOther1_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.AngOther[1], "");
                                            pWireClipHoles_AngOther1_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles_AngOther1_Front = "";
                                        pWireClipHoles_AngOther1_Back = "";
                                    }
                                
                                    pVALUE = "'" + pWireClipHoles_AngOther1_Front + ", " + pWireClipHoles_AngOther1_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WireClipHoles.DBC":             //Col. BA
                                    //-------------------------------------------------            
                                    string pWireClipHoles_DBC_Front = null, pWireClipHoles_DBC_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_DBC_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.DBC, "");
                                            pWireClipHoles_DBC_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_DBC_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.DBC, "");
                                            pWireClipHoles_DBC_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles_DBC_Front = "";
                                        pWireClipHoles_DBC_Back = "";
                                    }
                                
                                    pVALUE = "'" + pWireClipHoles_DBC_Front + ", " + pWireClipHoles_DBC_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.Screw_Spec.D_TapDrill":   //Col. BB
                                    //-------------------------------------------------------------------     
                                    string pScrew_Spec_D_TapDrill_Front = null, pScrew_Spec_D_TapDrill_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pScrew_Spec_D_TapDrill_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.Screw_Spec.D_TapDrill, "");
                                            pScrew_Spec_D_TapDrill_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pScrew_Spec_D_TapDrill_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.Screw_Spec.D_TapDrill, "");
                                            pScrew_Spec_D_TapDrill_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pScrew_Spec_D_TapDrill_Front = "";
                                        pScrew_Spec_D_TapDrill_Back = "";
                                    }
                  
                                    pVALUE = "'" + pScrew_Spec_D_TapDrill_Front + ", " + pScrew_Spec_D_TapDrill_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.Screw_Spec.D":     //Col. BC
                                    //----------------------------------------------------------      
                                    string pWireClipHoles_ThreadDia_Desig_Front = null, pWireClipHoles_ThreadDia_Desig_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_ThreadDia_Desig_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.Screw_Spec.D, "");
                                            pWireClipHoles_ThreadDia_Desig_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_ThreadDia_Desig_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.Screw_Spec.D, "");
                                            pWireClipHoles_ThreadDia_Desig_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles_ThreadDia_Desig_Front = "";
                                        pWireClipHoles_ThreadDia_Desig_Back = "";
                                    }
                              
                                    pVALUE = "'" + pWireClipHoles_ThreadDia_Desig_Front + ", " + pWireClipHoles_ThreadDia_Desig_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].WireClipHoles.ThreadDepth":     //Col. BE
                                    //---------------------------------------------------------       
                                    string pWireClipHoles_ThreadDepth_Front = null, pWireClipHoles_ThreadDepth_Back = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pWireClipHoles_ThreadDepth_Front = modMain.ConvDoubleToStr(mEndSeal[0].WireClipHoles.ThreadDepth, "");
                                            pWireClipHoles_ThreadDepth_Back = "";
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pWireClipHoles_ThreadDepth_Back = modMain.ConvDoubleToStr(mEndSeal[1].WireClipHoles.ThreadDepth, "");
                                            pWireClipHoles_ThreadDepth_Front = "";
                                        }
                                    }
                                    else
                                    {
                                        pWireClipHoles_ThreadDepth_Front = "";
                                        pWireClipHoles_ThreadDepth_Back = "";
                                    }

                                    pVALUE = "'" + pWireClipHoles_ThreadDepth_Front + ", " + pWireClipHoles_ThreadDepth_Back + "'";
                                    break;

                                case "gProject.Product.Bearing.SplitConfig":                        //Col. CB
                                    //--------------------------------------        
                                    string pSpiltConfig = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                    {
                                        pSpiltConfig = "U";
                                    }
                                    else
                                    {
                                        pSpiltConfig = "S";
                                    }

                                    pVALUE = "'" + pSpiltConfig + "," + pSpiltConfig + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Tol_DBore(Detail)":              //Col. CC
                                    //------------------------------------------------- 
                                    string pTol_DBore_Detail_Front = null, pTol_DBore_Detail_Back = null;
                                    ////if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    ////{
                                    ////    pTol_DBore_Detail_Front = modMain.ConvDoubleToStr(mEndSeal[0].Tol_DBore("Detail"), "");
                                    ////    pTol_DBore_Detail_Front = "LIMIT;-" + pTol_DBore_Detail_Front + ";" + pTol_DBore_Detail_Front;
                                    ////}
                                    ////if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    ////{
                                    ////    pTol_DBore_Detail_Back = modMain.ConvDoubleToStr(mEndSeal[1].Tol_DBore("Detail"), "");
                                    ////    pTol_DBore_Detail_Back = "LIMIT;-" + pTol_DBore_Detail_Back + ";" + pTol_DBore_Detail_Back;
                                    ////}

                                    pVALUE = "'" + pTol_DBore_Detail_Front + "," + pTol_DBore_Detail_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].Tol_DBore(Assy)":               //Col. CD
                                    //----------------------------------------------- 
                                    string pTol_DBore_Assy_Front = null, pTol_DBore_Assy_Back = null;
                                    ////if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    ////{
                                    ////    pTol_DBore_Assy_Front = modMain.ConvDoubleToStr(mEndSeal[0].Tol_DBore("Assy"), "");
                                    ////    pTol_DBore_Assy_Front = "LIMIT;-" + pTol_DBore_Assy_Front + ";" + pTol_DBore_Assy_Front;
                                    ////}
                                    ////if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    ////{
                                    ////    pTol_DBore_Assy_Back = modMain.ConvDoubleToStr(mEndSeal[1].Tol_DBore("Assy"), "");
                                    ////    pTol_DBore_Assy_Back = "LIMIT;-" + pTol_DBore_Assy_Back + ";" + pTol_DBore_Assy_Back;
                                    ////}

                                    pVALUE = "'" + pTol_DBore_Assy_Front + "," + pTol_DBore_Assy_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Thread_Thru":         //Col. CE
                                    //------------------------------------------------------ 
                                    string pThread_Thru_Front = null;
                                    string pThread_Thru_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                      
                                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C ||
                                           mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pThread_Thru_Front = "Y";
                                        }
                                        else if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            if (mEndSeal[0].MountHoles.Thread_Thru)
                                                pThread_Thru_Front = "Y";
                                            else
                                                pThread_Thru_Front = "N";
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C ||
                                            mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pThread_Thru_Back = "Y";
                                        }
                                        else if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            if (mEndSeal[1].MountHoles.Thread_Thru)
                                                pThread_Thru_Back = "Y";
                                            else
                                                pThread_Thru_Back = "N";
                                        }
                                    }

                                    pVALUE = "'" + pThread_Thru_Front + ", " + pThread_Thru_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Depth_Thread":       //Col. CF
                                    //--------------------------------------------------------      
                                    string pMountHoles_Depth_Thread_Front = null, pMountHoles_Depth_Thread_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                    {
                                      
                                        if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C ||
                                           mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pMountHoles_Depth_Thread_Front = modMain.ConvDoubleToStr(mEndSeal[0].L, "#0.000");
                                        }
                                        else if (mEndSeal[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            if (mEndSeal[0].MountHoles.Thread_Thru)
                                                pMountHoles_Depth_Thread_Front = modMain.ConvDoubleToStr(mEndSeal[0].L, "#0.000");
                                            else
                                                pMountHoles_Depth_Thread_Front = modMain.ConvDoubleToStr(mEndSeal[0].MountHoles.Depth_Thread, "#0.000");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C ||
                                            mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pMountHoles_Depth_Thread_Back = modMain.ConvDoubleToStr(mEndSeal[1].L, "#0.000");
                                        }
                                        else if (mEndSeal[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            if (mEndSeal[1].MountHoles.Thread_Thru)
                                                pMountHoles_Depth_Thread_Back = modMain.ConvDoubleToStr(mEndSeal[1].L, "#0.000");
                                            else
                                                pMountHoles_Depth_Thread_Back = modMain.ConvDoubleToStr(mEndSeal[1].MountHoles.Depth_Thread, "#0.000");
                                        }
                                    }
                                  
                                    pVALUE = "'" + pMountHoles_Depth_Thread_Front + ", " + pMountHoles_Depth_Thread_Back + "'";
                                    break;
                            }

                            if (pVALUE != null)
                            {
                                pWHERE = " WHERE fldCellColName = '" + pCellColName[i] + "'";
                                pSQL = pUPDATE + pSET + pVALUE + pWHERE;
                                DB_In.ExecuteCommand(pSQL);
                            }
                        }
                    }
                }
                catch (Exception pEXP)
                {
                    MessageBox.Show("Database Error - " + pEXP.Message);
                }
            }

            private String[] FixturePartNo(clsDB DB_In, String PartNo_In)
            //============================================================
            {
                string pWHERE = "", pSQL = "";
                StringCollection pMount_Fixture_PartNo_Comp = new StringCollection();

                pWHERE = " WHERE fldPartNo = '" + PartNo_In + "'";               
               
                pSQL = "Select fldPartNo_Comp from tblManf_Fixture_SplitAndTurn" + pWHERE;

                SqlConnection pConnection = new SqlConnection();
                SqlDataReader pDR = null;

                pDR = DB_In.GetDataReader(pSQL, ref pConnection);
                string pFixture_PartNo_Comp = "";

                if (pDR.Read())
                {
                    pFixture_PartNo_Comp = DB_In.CheckDBString(pDR, "fldPartNo_Comp");
                }

                pDR.Close();
                pConnection.Close();
                string[] pFix_PartNo_Comp_Sub = null;

                if (pFixture_PartNo_Comp.Contains(","))
                {
                    pFix_PartNo_Comp_Sub = pFixture_PartNo_Comp.Split(',');                 

                    //pMount_Fixture_PartNo_Comp[j] = pFix_PartNo_Comp_Sub[0].Trim();

                }
                else
                {
                    pFix_PartNo_Comp_Sub = new string[2];
                    pFix_PartNo_Comp_Sub[0] = pFixture_PartNo_Comp;
                    pFix_PartNo_Comp_Sub[1] = "";
                    //pMount_Fixture_PartNo_Comp[j] = pFixture_PartNo_Comp;
                }

                return pFix_PartNo_Comp_Sub;
            }


        #endregion


        #region "EXCEL MAPPING:"

            private void PopulateAndMove_DT_Seal(clsFiles Files_In, clsProject Project_In,
                                                 clsDB DB_In, string FilePath_In)
            //============================================================================    
            {
                try
                {
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_Seal, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open 'Sketchs' WorkSheets.
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[1];

                    //....Set Project Number.
                    int pRowBegin = 3;
                    int pNo_Suffix = 1;
                    if (Project_In.AssyDwg.No_Suffix != "")
                    {
                        pNo_Suffix = Convert.ToInt32(Project_In.AssyDwg.No_Suffix);
                    }
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        if (pNo_Suffix <= 9)
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 2).ToString();//"-03" ;
                        }
                        else
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 2).ToString();//"-03" ;
                        }
                        pRowBegin++;
                    }
                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        if (pNo_Suffix <= 9)
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 3).ToString();//"-04" ;
                        }
                        else
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 3).ToString();//"-04" ;
                        }
                    }

                    //.....Set Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_Var_Val = new StringCollection();

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Seal", "fldCellColName", "");
                    DB_In.PopulateStringCol(pSoftware_Var_Val, "tblMapping_Seal", "fldSoftware_VarVal", "");

                    //Double[] pSealMountFixture_Sel_Ang_BackSeal = Bearing_In.SealMountFixture_Sel_Ang_BackSeal();

                    for (int i = 0; i < pCellColName.Count; i++)
                    {
                        //for (int j = 3; j < 5; j++)
                        //{
                        if (pSoftware_Var_Val[i] != "")
                        {
                            int pIndx = ColumnNumber(pCellColName[i]);

                            string pSoftware_Val;
                            pRowBegin = 3;

                            if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                            {
                                pSoftware_Val = modMain.ExtractPreData(pSoftware_Var_Val[i], ",");
                                if (pSoftware_Val != "")
                                pWkSheet.Cells[pRowBegin, pIndx] = pSoftware_Val;
                                pRowBegin++;
                            }
                            if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                            {
                                pSoftware_Val = modMain.ExtractPostData(pSoftware_Var_Val[i], ",");
                                if (pSoftware_Val != "")
                                pWkSheet.Cells[pRowBegin, pIndx] = pSoftware_Val;
                            }
                        }
                       
                    }

                    string pFile = modMain.ExtractPreData(mcSeal_Title, ".");
                    String pFileName = FilePath_In + "\\" + pFile + ".xlsx";

                    if (!Directory.Exists(FilePath_In))
                        Directory.CreateDirectory(FilePath_In);

                    if (File.Exists(pFileName))
                        File.Delete(pFileName);

                    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;

                    pWkbOrg.SaveAs(pFileName, pWkbOrg.FileFormat, mobjMissing,
                                        mobjMissing, false, mobjMissing, pAccessMode,
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
