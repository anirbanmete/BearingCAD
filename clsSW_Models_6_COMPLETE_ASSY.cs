//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models_6_COMPLETE_ASSY           '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  10JAN13                                '
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

            private void Populate_tblMapping_Complete(clsProject Project_In, clsOpCond OpCond_In, clsDB DB_In)
            //================================================================================================  
            {
                try
                {
                    //....Set Mapping Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_VarName = new StringCollection();

                    string pOrderBy = " ORDER BY fldItemNo ASC";

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Complete", "fldCellColName",
                                            pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_Complete", "fldSoftware_VarName",
                                            pOrderBy);

                    String pUPDATE = "UPDATE tblMapping_Complete ";
                    String pSET = "SET fldSoftware_VarVal = ";
                    String pVALUE = null;
                    String pWHERE = null;
                    String pSQL = null;

                    pSQL = pUPDATE + pSET + "NULL";
                    DB_In.ExecuteCommand(pSQL);

                    int pNo_Suffix = 1;
                    if (Project_In.AssyDwg.No_Suffix != "")
                    {
                        pNo_Suffix = Convert.ToInt32(Project_In.AssyDwg.No_Suffix);
                    }

                    for (int i = 0; i < pSoftware_VarName.Count; i++)
                    {
                        pSQL = null;
                        pWHERE = null;
                        pVALUE = null;

                        if (pSoftware_VarName[i] != "")
                        {
                            switch (pSoftware_VarName[i])
                            {

                                case "gProject.AssDwg.No-01":
                                    //-----------------------       //Col. A
                                    //....Set Project Number. 
                                    if (pNo_Suffix <= 9)
                                    {
                                        pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix).ToString() + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix).ToString() + "'";
                                    }
                                    break;


                                case "gProject.AssDwg.No-02":
                                    //-----------------------       //Col. C
                                    if ((pNo_Suffix+1) <= 9)
                                    {
                                        pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix+1).ToString() + "'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix+1).ToString() + "'";
                                    }
                                    break;

                                case "gProject.Product.EndConfig[0].Type":
                                    //-------------------------------------     //Col. D
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                        pVALUE = "'E'";
                                    else if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        pVALUE = "'T'";
                                    break;

                                case "gProject.AssDwg.No-03_1":
                                    //-------------------------         //Col. E
                                    if ((pNo_Suffix+2) <= 9)
                                    {
                                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix+2).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix+2).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                   break;

                                case "gProject.AssDwg.No-03_2":
                                   //-------------------------         //Col. F
                                   if ((pNo_Suffix+2) <= 9)
                                    {
                                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix+2).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix+2).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }                              
                                    break;


                                case "gProject.Product.EndConfig[1].Type":
                                    //-------------------------------------     //Col. G
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                        pVALUE = "'E'";
                                    else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        pVALUE = "'T'";
                                    break;

                                case "gProject.AssDwg.No-04_1":
                                    //--------------------------        //Col. H
                                    if ((pNo_Suffix+3) <= 9)
                                    {
                                        if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix+3).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                        if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix+3).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }                            
                                    break;

                                case "gProject.AssDwg.No-04_2":
                                    //--------------------------        //Col. I
                                    if ((pNo_Suffix+3) <= 9)
                                    {
                                        if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-0" + (pNo_Suffix+3).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                        if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                        {
                                            pVALUE = "'" + Project_In.AssyDwg.No + "-" + (pNo_Suffix+3).ToString() + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Loc_Bearing_Vert":
                                    //-------------------------------------------------------       //Col. J
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Center)
                                    {
                                        pVALUE = "'I'";
                                    }
                                    else
                                    {
                                        pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Bearing_Vert.ToString() + "'";
                                    }
                                    break;

                                case "gProject.Product.Bearing.Mount.Holes_GoThru":
                                    //---------------------------------------------     //Col. K
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                        pVALUE = "'Y'";
                                    else
                                        pVALUE = "'N'";

                                    break;

                                case "gProject.Product.Bearing.Mount.Holes_Bolting":
                                    //-----------------------------------------------       //Col. L
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                            pVALUE = "'F'";
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                            pVALUE = "'B'";

                                    }
                                    else
                                    {
                                        pVALUE = "''";
                                    }
                                    break;

                                case "gProject.Product.EndConfig[0].MountHoles.Type":
                                    //-----------------------------------------------       //Col. M
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                            {
                                                pVALUE = "'C'";
                                            }
                                            else if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                            {
                                                pVALUE = "'H'";
                                            }
                                            else if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                            {
                                                pVALUE = "'T'";
                                            }
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                       // pVALUE = "''";
                                        if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pVALUE = "'C'";
                                        }
                                        else if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pVALUE = "'H'";
                                        }
                                        else if (Project_In.Product.EndConfig[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pVALUE = "'T'";
                                        }
                                    }
                                    break;

                                case "gProject.Product.EndConfig[1].MountHoles.Type":
                                    //------------------------------------------------      //Col. N
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Back)
                                        {
                                            if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                            {
                                                pVALUE = "'C'";
                                            }
                                            else if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                            {
                                                pVALUE = "'H'";
                                            }
                                            else if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                            {
                                                pVALUE = "'T'";
                                            }
                                        }
                                        else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eFaceID.Front)
                                        {
                                            pVALUE = "''";
                                        }
                                    }
                                    else
                                    {
                                        //pVALUE = "''";
                                        if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pVALUE = "'C'";
                                        }
                                        else if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.H)
                                        {
                                            pVALUE = "'H'";
                                        }
                                        else if (Project_In.Product.EndConfig[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pVALUE = "'T'";
                                        }
                                    }
                                    break;

                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.Type":
                                    //------------------------------------------------------------      //Col. O    
                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Type + "'";
                                    break;

                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.Unit.System":
                                    //-------------------------------------------------------------------   //Col. P 
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Unit.System == clsUnit.eSystem.English)
                                        pVALUE = "'I'";
                                    else
                                        pVALUE = "'M'";
                                    break;


                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.D_Desig":
                                    //--------------------------------------------------------------        //Col. Q 
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig != null)         //BG 31DEC12
                                    {

                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig.Contains('M'))
                                        {
                                            pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig.Replace('M', ' ').Trim() + "'";
                                        }
                                        else
                                        {
                                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig.Contains('/'))
                                            {
                                                pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D + "'";
                                            }
                                            else
                                            {
                                                pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.D_Desig + "'";
                                            }
                                        }
                                    }

                                    break;


                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.Pitch":
                                    //-------------------------------------------------------------     //Col. R
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Pitch, "") + "'";
                                    break;


                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.L":
                                    //----------------------------------------------------------        //Col. S
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.L, "") + "'";
                                    break;


                                case "gProject.Product.Beraing.Mount.Fixture[0].Screw_Spec.Mat":
                                    //----------------------------------------------------------        //Col. T
                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].Screw_Spec.Mat + "'";
                                    break;

                                case "gProject.Product.Beraing.Mount.Fixture[0].HolesCount":
                                    //------------------------------------------------------            //Col. U
                                    pVALUE = "'" + modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount) + "'";
                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Loc_Casing_SL ":
                                    //----------------------------------------------------      //Col. W
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Center)
                                    {
                                        pVALUE = "'N'";
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Loc_Casing_SL == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Casing_SL.Offset)
                                    {
                                        pVALUE = "'Y'";
                                    }

                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Spec.Type ":
                                    //------------------------------------------------      //Col. X
                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Type + "'";
                                    break;


                                case "gProject.Product.Beraing.AntiRotPin.Spec.Unit.System":
                                    //------------------------------------------------------        //Col. Y
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Unit.System == clsUnit.eSystem.English)
                                        pVALUE = "'I'";
                                    else
                                        pVALUE = "'M'";
                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Spec.D_Desig":
                                    //---------------------------------------------------       //Col. Z
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig.Contains('M'))
                                    {
                                        pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig.Replace('M', ' ').Trim() + "'";
                                    }
                                    else
                                    {
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig.Contains('/'))
                                        {
                                            pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D + "'";
                                        }
                                        else
                                        {
                                            pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.D_Desig + "'";
                                        }
                                    }

                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Spec.L":
                                    //---------------------------------------------     //Col. AA
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.L, "") + "'";
                                    break;

                                case "gProject.Product.Beraing.AntiRotPin.Spec.Mat":
                                    //-----------------------------------------------       //Col. AB
                                    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).AntiRotPin.Spec.Mat + "'";
                                    break;

                                case "gOpCond.Speed()":   //....Shaft Speed
                                    //----------------    //Col. AF
                                    pVALUE = "'" + modMain.ConvIntToStr(OpCond_In.Speed()) + "'";
                                    break;

                                case "gOpCond.OilSupply.Lube.Type":   //....Oil Type
                                    //-----------------------------   //Col. AG    
                                    pVALUE = "'" + OpCond_In.OilSupply.Lube.Type + "'";
                                    break;

                                case "gOpCond.OilSupply.Temp":   //....Inlet Temp
                                    //-----------------------    //Col. AH
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.NInt(OpCond_In.OilSupply.Temp).ToString() + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.NInt(Project_In.Unit.CFac_Temp_EngToMet(OpCond_In.OilSupply.Temp)).ToString() + "'";
                                    }

                                    break;


                                case "gOpCond.OilSupply.Press": //....Oil Pressure
                                    //-----------------------   //Col. AI
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        //pVALUE = "'" + modMain.NInt(OpCond_In.OilSupply.Press).ToString() + "'";
                                        pVALUE = "'" + OpCond_In.OilSupply.Press.ToString("#0.0") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        //pVALUE = "'" + Project_In.Unit.CFac_Press_EngToMet(modMain.NInt(OpCond_In.OilSupply.Press)).ToString("#0.00") + "'";
                                        pVALUE = "'" + Project_In.Unit.CFac_Press_EngToMet(OpCond_In.OilSupply.Press).ToString("#0.00") + "'";
                                    }

                                    break;


                                case  "gOpCond.Radial_Load()":            // "gOpCond.Load()":      //....Radial Load
                                    //------------------------      //Col. AJ
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        //pVALUE = "'" + modMain.NInt(OpCond_In.Load()).ToString() + "'";
                                        pVALUE = "'" + OpCond_In.Radial_Load().ToString("#0.0") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        //pVALUE = "'" + modMain.NInt(Project_In.Unit.CFac_Load_EngToMet(OpCond_In.Load())).ToString() + "'";
                                        pVALUE = "'" + Project_In.Unit.CFac_Load_EngToMet(OpCond_In.Radial_Load()).ToString("#0.00") + "'";
                                    }

                                    break;


                                case "gProject.Product.Bearing.DShaft_Range[1]/ gProject.Product.Bearing.DShaft_Range[0]":   //....Shaft Dia(Max/Min)
                                    //-----------------------------------------------------------------------------------    //Col. AK
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1], "#0.0000") + "/" +
                                             modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0], "#0.0000") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(Project_In.Unit.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1]), "#0.0000") + "/" +
                                                 modMain.ConvDoubleToStr(Project_In.Unit.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0]), "#0.0000") + "'";

                                    }
                                    break;


                                case "gProject.Product.Bearing.DSet_Range[0]/gProject.Product.Bearing.DSet_Range[1]":       //....Bore Dia(Min/Max)
                                    //-------------------------------------------------------------------------------       //Col. AL
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0], "#0.0000") + "/" +
                                                       modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1], "#0.0000") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(Project_In.Unit.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0]), "#0.0000") + "/" +
                                                 modMain.ConvDoubleToStr(Project_In.Unit.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1]), "#0.0000") + "'";
                                    }
                                    break;


                                case "gProject.Product.Bearing.Pad.Pivot.Offset":   //....Offset
                                    //-------------------------------------------   //Col. AM
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset / 100.0, "#0.00") + "'";
                                    break;


                                case "gProject.Product.Bearing.PreLoad()":        //....Preload
                                    //-----------------------------------         //Col. AN 
                                    pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PreLoad(), "#0.000") + "'";
                                    break;


                                case "gProject.Product.Bearing.PerformData.Power_HP":       //....Power loss
                                    //-----------------------------------------------       //Col. AO
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP, "#0.0") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(Project_In.Product.Unit.CFac_Power_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP), "#0.0") + "'";
                                    }
                                    break;


                                case "gProject.Product.Bearing.PerformData.FlowReqd_gpm":   //....Oil Flow
                                    //---------------------------------------------------   //Col. AP
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm, "#0.0") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(Project_In.Product.Unit.CFac_GPM_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm), "#0.0") + "'";
                                    }
                                    break;


                                case "gProject.Product.Bearing.PerformData.TempRise_F":     //....Temp. Rise
                                    //-------------------------------------------------     //Col. AQ
                                    if (Project_In.Unit.System == clsUnit.eSystem.English)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F, "#0.0") + "'";
                                    }
                                    else if (Project_In.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pVALUE = "'" + modMain.ConvDoubleToStr(Project_In.Product.Unit.CFac_TRise_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F), "#0.0") + "'";
                                    }
                                    break;


                                case "gProject.Customer.PartNo":    //....Part No
                                    //---------------------------   //Col. AR
                                    pVALUE = "'" + Project_In.Customer.PartNo + "'";
                                    break;


                                case "gProject.Customer.MachineDesc":
                                    //-------------------------------       //Col. AS
                                    pVALUE = "'" + Project_In.Customer.MachineDesc + "'";
                                    break;

                                //BG 10JAN13
                                case "gOpCond.Thrust_Load_Front()":         //"gProject.Product.EndConfig[0].PerformData.PadMax_Load":
                                    //------------------------------       //Col. CE
                                    //double pMaxLoad = 0.0;
                                    //int pPadCount = 0;
                                    //if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    //{
                                    //    pPadCount = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[0]).Pad_Count;
                                    //    pMaxLoad = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[0]).PerformData.PadMax.Load;

                                    //    pVALUE = "'" + modMain.ConvDoubleToStr(pMaxLoad * pPadCount, "") + "'";
                                    //}
                                    //else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    //{
                                    //    pPadCount = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[1]).Pad_Count;
                                    //    pMaxLoad = ((clsBearing_Thrust_TL)Project_In.Product.EndConfig[1]).PerformData.PadMax.Load;
                                    //    pVALUE = "'" + modMain.ConvDoubleToStr(pMaxLoad * pPadCount, "") + "'";
                                    //}

                                    pVALUE = "'" + modMain.ConvDoubleToStr(OpCond_In.Thrust_Load_Front(), "#0.0") + "'";
                                    break;

                                case "gOpCond.Thrust_Load_Back()":         
                                    //------------------------------       //Col. CF
                                    pVALUE = "'" + modMain.ConvDoubleToStr(OpCond_In.Thrust_Load_Back(), "#0.0") + "'";
                                    break;

                                case "gProject.Customer.Name":
                                    //-------------------------     //Col. CG
                                    pVALUE = "'" + Project_In.Customer.Name + "'";
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
                    MessageBox.Show("Excel File Error - " + pEXP.Message);
                }
            }

        #endregion


        #region "EXCEL MAPPING:"

            private void PopulateAndMove_DT_CompleteAssy(clsFiles Files_In, clsProject Project_In,
                                                         clsDB DB_In, string FilePath_In)
            //=====================================================================================
            {
                try
                {
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_CompleteAssy, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open 'Sketchs' WorkSheets.
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[1];
          
                    //....Set Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_VarVal = new StringCollection();

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_Complete", "fldCellColName", "");
                    DB_In.PopulateStringCol(pSoftware_VarVal, "tblMapping_Complete", "fldSoftware_VarVal", "");


                    for (int i = 0; i < pCellColName.Count; i++)
                    {
                        for (int j = 3; j < 4; j++)
                        {
                            if (pSoftware_VarVal[i] != "")
                            {
                                int pIndx = ColumnNumber(pCellColName[i]);
                                pWkSheet.Cells[j, pIndx] = pSoftware_VarVal[i];
                            }
                        }
                    }

                    string pFile = modMain.ExtractPreData(mcCompleteAssy_Title, ".");
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

                }
            }

        #endregion
    }
}
