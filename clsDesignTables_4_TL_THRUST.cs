//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsDesignTables_4_TL_THRUST            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  12JUN12                                '
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
    partial class clsDesignTables
    {
        //....Design Table Template-type Files (w/o Rev & Date):
        private const string mcTL_TB_Title = "TL THRUST BEARING";

        #region "DATABASE MAPPING:"

            private void Populate_tblMapping_TL_TB(clsProject Project_In, clsDB DB_In)
            //=========================================================================
            {
                clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
                for (int i = 0; i < 2; i++)
                {
                    if (modMain.gProject.Product.EndConfig[i].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(Project_In.Product.EndConfig[i])).Clone();
                    }
                }

                try
                {
                    //.....Set Maping Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_VarName = new StringCollection();

                    string pOrderBy = " ORDER BY fldItemNo ASC";

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_TL_TB", "fldCellColName", pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_TL_TB", "fldSoftware_VarName", pOrderBy);

                    String pUPDATE = "UPDATE tblMapping_TL_TB ";
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
                        pVALUE = null;

                        if (pSoftware_VarName[i] != "")
                        {
                            switch (pSoftware_VarName[i])
                            {
                                case "gProject.Product.EndConfig[i].DirectionType":                 //Col. B
                                    //---------------------------------------------     
                                    string pDirectionType_Front = null; string pDirectionType_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].DirectionType == clsBearing_Thrust_TL.eDirectionType.Uni)
                                        {
                                            pDirectionType_Front = "CCW";
                                        }

                                        else if (mEndTB[0].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                        {
                                            pDirectionType_Front = "BI";
                                        }
                                        else if (mEndTB[0].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bumper)
                                        {
                                            pDirectionType_Front = "BP";
                                        }
                                    }

                                    else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].DirectionType == clsBearing_Thrust_TL.eDirectionType.Uni)
                                        {
                                            pDirectionType_Back = "CW";
                                        }
                                        else if (mEndTB[1].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                        {
                                            pDirectionType_Back = "BI";
                                        }
                                        else if (mEndTB[1].DirectionType == clsBearing_Thrust_TL.eDirectionType.Bumper)
                                        {
                                            pDirectionType_Back = "BP";
                                        }
                                    }

                                    pVALUE = "'" + pDirectionType_Front + ", " + pDirectionType_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Mat.Base":                      //Col. I
                                    //----------------------------------------      
                                    string pBaseMat_Front = null; string pBaseMat_Back = null;
                                    string pLiningMat_Front = null, pLiningMat_Back = null;
                                    string pMatLining_Front = null, pMatLining_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pBaseMat_Front = MatAbbr(mEndTB[0].Mat.Base);
                                        pLiningMat_Front = MatAbbr(mEndTB[0].Mat.Lining);
                                        pMatLining_Front = pBaseMat_Front + "/" + pLiningMat_Front;
                                    }

                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pBaseMat_Back = MatAbbr(mEndTB[1].Mat.Base);
                                        pLiningMat_Back = MatAbbr(mEndTB[1].Mat.Lining);
                                        pMatLining_Back = pBaseMat_Back + "/" + pLiningMat_Back;
                                    }

                                    string pBaseMat = pMatLining_Front + ", " + pMatLining_Back;
                                    pVALUE = "'" + pBaseMat + "'";
                                    break;


                                case "gProject.Product.Bearing.SplitConfig":                        //Col. K
                                    //--------------------------------------       
                                    string pSpiltConfig = null;

                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                    {
                                        pSpiltConfig = "Y";
                                    }
                                    else
                                    {
                                        pSpiltConfig = "N";
                                    }

                                    pVALUE = "'" + pSpiltConfig + "," + pSpiltConfig + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].LiningT.ID":                    //Col. L
                                    //-----------------------------------------     
                                    string pLiningT_ID_Front = null, pLiningT_ID_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLiningT_ID_Front = modMain.ConvDoubleToStr(mEndTB[0].LiningT.ID, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLiningT_ID_Back = modMain.ConvDoubleToStr(mEndTB[1].LiningT.ID, "");
                                    }

                                    pVALUE = "'" + pLiningT_ID_Front + ", " + pLiningT_ID_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].LiningT.Face":                  //Col. M
                                    //--------------------------------------------     
                                    string pLiningT_Face_Front = null, pLiningT_Face_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLiningT_Face_Front = modMain.ConvDoubleToStr(mEndTB[0].LiningT.Face, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLiningT_Face_Back = modMain.ConvDoubleToStr(mEndTB[1].LiningT.Face, "");
                                    }

                                    pVALUE = "'" + pLiningT_Face_Front + ", " + pLiningT_Face_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].DimStart":                      //Col. Z
                                    //----------------------------------------       
                                    string pDimStart_Front = null, pDimStart_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDimStart_Front = modMain.ConvDoubleToStr(mEndTB[0].DimStart(), "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDimStart_Back = modMain.ConvDoubleToStr(mEndTB[1].DimStart(), "");
                                    }

                                    pVALUE = "'" + pDimStart_Front + ", " + pDimStart_Back + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.DFit_EndConfig(0)":            //Col. AB
                                    //--------------------------------------------------       
                                    string pDFit_EndConfig_Front = null, pDFit_EndConfig_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDFit_EndConfig_Front = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(0), "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDFit_EndConfig_Back = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(0), "");
                                    }

                                    pVALUE = "'" + pDFit_EndConfig_Front + ", " + pDFit_EndConfig_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].LFlange":                       //Col. AC
                                    //---------------------------------------       
                                    string pLFlange_Front = null, pLFlange_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLFlange_Front = modMain.ConvDoubleToStr(mEndTB[0].LFlange, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLFlange_Back = modMain.ConvDoubleToStr(mEndTB[1].LFlange, "");
                                    }

                                    pVALUE = "'" + pLFlange_Front + ", " + pLFlange_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].PadD[1]":                       //Col. AD
                                    //---------------------------------------        
                                    string pPadD_Front = null, pPadD_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPadD_Front = modMain.ConvDoubleToStr(mEndTB[0].PadD[1], "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPadD_Back = modMain.ConvDoubleToStr(mEndTB[1].PadD[1], "");
                                    }

                                    pVALUE = "'" + pPadD_Front + ", " + pPadD_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].DBore()":                       //Col. AF
                                    //---------------------------------------      
                                    string pDBore_Front = null, pDBore_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDBore_Front = modMain.ConvDoubleToStr(mEndTB[0].DBore(), "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDBore_Back = modMain.ConvDoubleToStr(mEndTB[1].DBore(), "");
                                    }

                                    pVALUE = "'" + pDBore_Front + ", " + pDBore_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].PadD[0]":                       //Col. AM
                                    //---------------------------------------      
                                    string pPad_Front = null, pPad_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPad_Front = modMain.ConvDoubleToStr(mEndTB[0].PadD[0], "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPad_Back = modMain.ConvDoubleToStr(mEndTB[1].PadD[0], "");
                                    }

                                    pVALUE = "'" + pPad_Front + ", " + pPad_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].LandL":                         //Col. AN
                                    //-------------------------------------        
                                    string pLandL_Front = null, pLandL_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLandL_Front = modMain.ConvDoubleToStr(mEndTB[0].LandL, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pLandL_Back = modMain.ConvDoubleToStr(mEndTB[1].LandL, "");
                                    }

                                    pVALUE = "'" + pLandL_Front + ", " + pLandL_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].BackRelief.Reqd":               //Col. AO
                                    //-----------------------------------------------      
                                    string pBackRelief_Reqd_Front = null, pBackRelief_Reqd_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].BackRelief.Reqd)
                                            pBackRelief_Reqd_Front = "Y";
                                        else
                                            pBackRelief_Reqd_Front = "N";
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].BackRelief.Reqd)
                                            pBackRelief_Reqd_Back = "Y";
                                        else
                                            pBackRelief_Reqd_Back = "N";
                                    }

                                    pVALUE = "'" + pBackRelief_Reqd_Front + ", " + pBackRelief_Reqd_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].BackRelief.D":                  //Col. AP
                                    //--------------------------------------------      
                                    string pBackRelief_D_Front = null, pBackRelief_D_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pBackRelief_D_Front = modMain.ConvDoubleToStr(mEndTB[0].BackRelief.D, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pBackRelief_D_Back = modMain.ConvDoubleToStr(mEndTB[1].BackRelief.D, "");
                                    }

                                    pVALUE = "'" + pBackRelief_D_Front + ", " + pBackRelief_D_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].BackRelief.Depth":              //Col. AQ
                                    //------------------------------------------------      
                                    string pBackRelief_Depth_Front = null, pBackRelief_Depth_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].BackRelief.Reqd)
                                        {
                                            pBackRelief_Depth_Front = modMain.ConvDoubleToStr(mEndTB[0].BackRelief.Depth, "");
                                        }
                                        else
                                        {
                                            pBackRelief_Depth_Front = "0"; 
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].BackRelief.Reqd)
                                        {
                                            pBackRelief_Depth_Back = modMain.ConvDoubleToStr(mEndTB[1].BackRelief.Depth, "");
                                        }
                                        else
                                        {
                                            pBackRelief_Depth_Back = "0";
                                        }
                                    }

                                    pVALUE = "'" + pBackRelief_Depth_Front + ", " + pBackRelief_Depth_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].BackRelief.Fillet":             //Col. AR
                                    //-------------------------------------------------            
                                    string pBackRelief_Fillet_Front = null, pBackRelief_Fillet_Back = null;
                                    Double pBackRelief_Def = 0.00005;       //0.001
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].BackRelief.Reqd)
                                        {
                                            if (mEndTB[0].BackRelief.Fillet < modMain.gcEPS)
                                            {
                                                pBackRelief_Fillet_Front = modMain.ConvDoubleToStr(pBackRelief_Def, "");
                                            }
                                            else
                                            {
                                                pBackRelief_Fillet_Front = mEndTB[0].BackRelief.Fillet.ToString("#0.000");
                                            }
                                        }
                                        else
                                        {
                                            pBackRelief_Fillet_Front = "0.001";
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].BackRelief.Reqd)
                                        {
                                            if (mEndTB[1].BackRelief.Fillet < modMain.gcEPS)
                                            {
                                                pBackRelief_Fillet_Back = modMain.ConvDoubleToStr(pBackRelief_Def, "");
                                            }
                                            else
                                            {
                                                pBackRelief_Fillet_Back = mEndTB[1].BackRelief.Fillet.ToString("#0.000");
                                            }
                                        }
                                        else
                                        {
                                            pBackRelief_Fillet_Back = "0.001";
                                        }
                                    }

                                    pVALUE = "'" + pBackRelief_Fillet_Front + ", " + pBackRelief_Fillet_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FeedGroove.Type":               //Col. AS
                                    //-----------------------------------------------      
                                    string pFeedGroove_Type_Front = null, pFeedGroove_Type_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Type_Front = mEndTB[0].FeedGroove.Type.Substring(0, 1);
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Type_Back = mEndTB[1].FeedGroove.Type.Substring(0, 1);
                                    }

                                    pVALUE = "'" + pFeedGroove_Type_Front + ", " + pFeedGroove_Type_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FeedGroove.Wid":                //Col. AT
                                    //----------------------------------------------       
                                    string pFeedGroove_Wid_Front = null, pFeedGroove_Wid_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Wid_Front = modMain.ConvDoubleToStr(mEndTB[0].FeedGroove.Wid, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Wid_Back = modMain.ConvDoubleToStr(mEndTB[1].FeedGroove.Wid, "");
                                    }

                                    pVALUE = "'" + pFeedGroove_Wid_Front + ", " + pFeedGroove_Wid_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FeedGroove.DBC":                 //Col. AU
                                    //----------------------------------------------       
                                    string pFeedGroove_DBC_Front = null, pFeedGroove_DBC_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_DBC_Front = modMain.ConvDoubleToStr(mEndTB[0].FeedGroove.DBC, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_DBC_Back = modMain.ConvDoubleToStr(mEndTB[1].FeedGroove.DBC, "");
                                    }

                                    pVALUE = "'" + pFeedGroove_DBC_Front + ", " + pFeedGroove_DBC_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FeedGroove.Depth":              //Col. AY
                                    //------------------------------------------------     
                                    string pFeedGroove_Depth_Front = null, pFeedGroove_Depth_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Depth_Front = modMain.ConvDoubleToStr(mEndTB[0].FeedGroove.Depth, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Depth_Back = modMain.ConvDoubleToStr(mEndTB[1].FeedGroove.Depth, "");
                                    }

                                    pVALUE = "'" + pFeedGroove_Depth_Front + ", " + pFeedGroove_Depth_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FeedGroove.Dist_Chamf":         //Col. AZ
                                    //-----------------------------------------------------        
                                    string pFeedGroove_Dist_Chamf_Front = null, pFeedGroove_Dist_Chamf_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Dist_Chamf_Front = modMain.ConvDoubleToStr(mEndTB[0].FeedGroove.Dist_Chamf, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFeedGroove_Dist_Chamf_Back = modMain.ConvDoubleToStr(mEndTB[1].FeedGroove.Dist_Chamf, "");
                                    }

                                    pVALUE = "'" + pFeedGroove_Dist_Chamf_Front + ", " + pFeedGroove_Dist_Chamf_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Type":               //Col. BA
                                    //-----------------------------------------------           
                                    string pMountHoles_Type_Front = null, pMountHoles_Type_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pMountHoles_Type_Front = mEndTB[0].MountHoles.Type.ToString();
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pMountHoles_Type_Back = mEndTB[1].MountHoles.Type.ToString();
                                    }

                                    pVALUE = "'" + pMountHoles_Type_Front + ", " + pMountHoles_Type_Back + "'";

                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Thread_Thru":        //Col. BB
                                    //------------------------------------------------------ 
                                    string pThread_Thru_Front = null;
                                    string pThread_Thru_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].MountHoles.Thread_Thru)
                                            pThread_Thru_Front = "Y";
                                        else
                                            pThread_Thru_Front = "N";
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].MountHoles.Thread_Thru)
                                            pThread_Thru_Back = "Y";
                                        else
                                            pThread_Thru_Back = "N";
                                    }

                                    pVALUE = "'" + pThread_Thru_Front + ", " + pThread_Thru_Back + "'";

                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D_Thru":  //Col. BC
                                    //------------------------------------------------------------      
                                    string pD_Thru_Front = null, pD_Thru_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pD_Thru_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles.Screw_Spec.D_TapDrill, "");
                                        }
                                        else
                                        {
                                            pD_Thru_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles.Screw_Spec.D_Thru, "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.T)
                                        {
                                            pD_Thru_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles.Screw_Spec.D_TapDrill, "");
                                        }
                                        else
                                        {
                                            pD_Thru_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles.Screw_Spec.D_Thru, "");
                                        }
                                    }

                                    pVALUE = "'" + pD_Thru_Front + ", " + pD_Thru_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D":       //Col. BD
                                    //-------------------------------------------------------       
                                    string pEndConfig_D_Front = null, pEndConfig_D_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pEndConfig_D_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles.Screw_Spec.D, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pEndConfig_D_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles.Screw_Spec.D, "");
                                    }

                                    pVALUE = "'" + pEndConfig_D_Front + ", " + pEndConfig_D_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Depth_Thread":       //Col. BE
                                    //-------------------------------------------------------       
                                    string pDepth_Thread_Front = null, pDepth_Thread_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].MountHoles.Thread_Thru)
                                        {
                                            pDepth_Thread_Front = modMain.ConvDoubleToStr(mEndTB[0].LFlange, "");
                                        }
                                        else
                                        pDepth_Thread_Front = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[0].MountHoles.Depth_Thread, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].MountHoles.Thread_Thru)
                                        {
                                            pDepth_Thread_Front = modMain.ConvDoubleToStr(mEndTB[1].LFlange, "");
                                        }
                                        else
                                        pDepth_Thread_Back = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[1].MountHoles.Depth_Thread, "");
                                    }

                                    pVALUE = "'" + pDepth_Thread_Front + ", " + pDepth_Thread_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles_Depth_Tap_Drill":    //Col. BF
                                    //----------------------------------------------------------        
                                    string pDepth_Tap_Drill_Front = null, pDepth_Tap_Drill_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDepth_Tap_Drill_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles_Depth_Tap_Drill(), "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pDepth_Tap_Drill_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles_Depth_Tap_Drill(), "");
                                    }

                                    pVALUE = "'" + pDepth_Tap_Drill_Front + ", " + pDepth_Tap_Drill_Back + "'";
                                    break;

                                case "gProject.Product.EndConfig[i].MountHoles.Screw_Spec.D_CBore":  //Col. BG
                                    //-------------------------------------------------------------        
                                    string pD_CBore_Front = null, pD_CBore_Back = null;
                                  

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pD_CBore_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles.Screw_Spec.D_CBore, "");
                                        }
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pD_CBore_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles.Screw_Spec.D_CBore, "");
                                        }
                                    }

                                    pVALUE = "'" + pD_CBore_Front + ", " + pD_CBore_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].MountHoles.Depth_CBore":        //Col. BH
                                    //------------------------------------------------------            
                                    string pDepth_CBore_Front = null, pDepth_CBore_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[0].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pDepth_CBore_Front = modMain.ConvDoubleToStr(mEndTB[0].MountHoles.Depth_CBore, "");
                                        }
                                       
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        if (mEndTB[1].MountHoles.Type == clsEndConfig.clsMountHoles.eMountHolesType.C)
                                        {
                                            pDepth_CBore_Back = modMain.ConvDoubleToStr(mEndTB[1].MountHoles.Depth_CBore, "");
                                        }
                                       
                                    }

                                    pVALUE = "'" + pDepth_CBore_Front + ", " + pDepth_CBore_Back + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesCount":        //Col. BI
                                    //------------------------------------------------------ 
                                    string pMount_Fixture_HolesCount = modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesCount) + ", " +
                                                                       modMain.ConvIntToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesCount);

                                    pVALUE = "'" + pMount_Fixture_HolesCount + "'";

                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngStart":     //Col. BJ
                                    //--------------------------------------------------------        

                                    //string pMount_Fixture_HolesAngStart = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart, "") + ", " +
                                    //                                      modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart, "");

                                     string pMount_Fixture_HolesAngStart = "";
                                        
                                     pMount_Fixture_HolesAngStart = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].HolesAngStart.ToString("#0.#") + ", " +
                                                                    ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].HolesAngStart.ToString("#0.#");
                                        

                                    pVALUE = "'" + pMount_Fixture_HolesAngStart + "'";
                                    break;


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[0]":   //Col. BK
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[1]":  //Col. BL
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[2]":  //Col. BM
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[3]":  //Col. BN
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[4]":  //Col. BO
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[5]":  //Col. BP
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].HolesAngOther[6]":  //Col. BQ
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


                                case "gProject.Product.Bearing.Mount.Fixture[i].DBC":               //Col. BR
                                    //-----------------------------------------------        
                                    string pMount_Fixture_DBC = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].DBC, "") + ", " +
                                                                    modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].DBC, "");

                                    pVALUE = "'" + pMount_Fixture_DBC + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].L":                             //Col. BS
                                    //---------------------------------        
                                    string pL_Front = null, pL_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pL_Front = modMain.ConvDoubleToStr(mEndTB[0].L, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pL_Back = modMain.ConvDoubleToStr(mEndTB[1].L, "");
                                    }

                                    pVALUE = "'" + pL_Front + ", " + pL_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].FaceOff_Assy":                  //Col. BT
                                    //--------------------------------------------        
                                    string pFaceOff_Assy_Front = null, pFaceOff_Assy_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFaceOff_Assy_Front = modMain.ConvDoubleToStr(mEndTB[0].FaceOff_Assy, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pFaceOff_Assy_Back = modMain.ConvDoubleToStr(mEndTB[1].FaceOff_Assy, "");
                                    }

                                    pVALUE = "'" + pFaceOff_Assy_Front + ", " + pFaceOff_Assy_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WeepSlot.Wid":                  //Col. BU
                                    //--------------------------------------------        
                                    string pWeepSlot_Wid_Front = null, pWeepSlot_Wid_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Wid_Front = modMain.ConvDoubleToStr(mEndTB[0].WeepSlot.Wid, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Wid_Back = modMain.ConvDoubleToStr(mEndTB[1].WeepSlot.Wid, "");
                                    }

                                    pVALUE = "'" + pWeepSlot_Wid_Front + ", " + pWeepSlot_Wid_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WeepSlot.Depth":                //Col. BV
                                    //---------------------------------------------        
                                    string pWeepSlot_Depth_Front = null, pWeepSlot_Depth_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Depth_Front = modMain.ConvDoubleToStr(mEndTB[0].WeepSlot.Depth, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Depth_Back = modMain.ConvDoubleToStr(mEndTB[1].WeepSlot.Depth, "");
                                    }

                                    pVALUE = "'" + pWeepSlot_Depth_Front + ", " + pWeepSlot_Depth_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Taper.Depth_OD":                //Col. BY
                                    //----------------------------------------------        
                                    string pTaper_Depth_OD_Front = null, pTaper_Depth_OD_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Depth_OD_Front = modMain.ConvDoubleToStr(mEndTB[0].Taper.Depth_OD, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Depth_OD_Back = modMain.ConvDoubleToStr(mEndTB[1].Taper.Depth_OD, "");
                                    }

                                    pVALUE = "'" + pTaper_Depth_OD_Front + ", " + pTaper_Depth_OD_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Taper.Depth_ID":                 //Col. BZ
                                    //----------------------------------------------       
                                    string pTaper_Depth_ID_Front = null, pTaper_Depth_ID_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Depth_ID_Front = modMain.ConvDoubleToStr(mEndTB[0].Taper.Depth_ID, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Depth_ID_Back = modMain.ConvDoubleToStr(mEndTB[1].Taper.Depth_ID, "");
                                    }

                                    pVALUE = "'" + pTaper_Depth_ID_Front + ", " + pTaper_Depth_ID_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Taper.Angle":                   //Col. CA
                                    //-------------------------------------------        
                                    string pTaper_Angle_Front = null, pTaper_Angle_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Angle_Front = modMain.ConvDoubleToStr(mEndTB[0].Taper.Angle, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTaper_Angle_Back = modMain.ConvDoubleToStr(mEndTB[1].Taper.Angle, "");
                                    }

                                    pVALUE = "'" + pTaper_Angle_Front + ", " + pTaper_Angle_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Shroud.Ro":                     //Col. CB
                                    //-----------------------------------------        
                                    string pShroud_Ro_Front = null, pShroud_Ro_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pShroud_Ro_Front = modMain.ConvDoubleToStr(mEndTB[0].Shroud.Ro, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pShroud_Ro_Back = modMain.ConvDoubleToStr(mEndTB[1].Shroud.Ro, "");
                                    }

                                    pVALUE = "'" + pShroud_Ro_Front + ", " + pShroud_Ro_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Pad_Count":                     //Col. CC
                                    //-----------------------------------------        
                                    string pPad_Count_Front = null, pPad_Count_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPad_Count_Front = modMain.ConvDoubleToStr(mEndTB[0].Pad_Count, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pPad_Count_Back = modMain.ConvDoubleToStr(mEndTB[1].Pad_Count, "");
                                    }

                                    pVALUE = "'" + pPad_Count_Front + ", " + pPad_Count_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Tol_DBore(Detail)":             //Col. DO
                                    //-------------------------------------------------        
                                    string pTol_DBore_Detail_Front = null, pTol_DBore_Detail_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTol_DBore_Detail_Front = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[0].Tol_DBore("Detail"), "");
                                        pTol_DBore_Detail_Front = "LIMIT;-" + pTol_DBore_Detail_Front + ";" + pTol_DBore_Detail_Front;
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTol_DBore_Detail_Back = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[1].Tol_DBore("Detail"), "");
                                        pTol_DBore_Detail_Back = "LIMIT;-" + pTol_DBore_Detail_Back + ";" + pTol_DBore_Detail_Back;
                                    }

                                    pVALUE = "'" + pTol_DBore_Detail_Front + "," + pTol_DBore_Detail_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Tol_DBore(Assy)":               //Col. DP
                                    //-----------------------------------------------       
                                    string pTol_DBore_Assy_Front = null, pTol_DBore_Assy_Back = null;
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTol_DBore_Assy_Front = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[0].Tol_DBore("Assy"), "");
                                        pTol_DBore_Assy_Front = "LIMIT;-" + pTol_DBore_Assy_Front + ";" + pTol_DBore_Assy_Front;
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pTol_DBore_Assy_Back = modMain.ConvDoubleToStr(Project_In.Product.EndConfig[1].Tol_DBore("Assy"), "");
                                        pTol_DBore_Assy_Back = "LIMIT;-" + pTol_DBore_Assy_Back + ";" + pTol_DBore_Assy_Back;
                                    }

                                    pVALUE = "'" + pTol_DBore_Assy_Front + "," + pTol_DBore_Assy_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].WeepSlot.Type":                  //Col. DQ
                                    //---------------------------------------------       
                                    string pWeepSlot_Type_Front = null, pWeepSlot_Type_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Type_Front = mEndTB[0].WeepSlot.Type.ToString().Substring(0, 1);
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pWeepSlot_Type_Back = mEndTB[1].WeepSlot.Type.ToString().Substring(0, 1);
                                    }

                                    pVALUE = "'" + pWeepSlot_Type_Front + ", " + pWeepSlot_Type_Back + "'";
                                    break;


                                case "gProject.Product.EndConfig[i].Shroud.Ri":                     //Col. DR
                                    //-----------------------------------------        
                                    string pShroud_Ri_Front = null, pShroud_Ri_Back = null;

                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pShroud_Ri_Front = modMain.ConvDoubleToStr(mEndTB[0].Shroud.Ri, "");
                                    }
                                    if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pShroud_Ri_Back = modMain.ConvDoubleToStr(mEndTB[1].Shroud.Ri, "");
                                    }

                                    pVALUE = "'" + pShroud_Ri_Front + ", " + pShroud_Ri_Back + "'";
                                    break;

                                case "gProject.Product.Bearing.Mount.Fixture[i].PartNo":            //Col. DT
                                    //--------------------------------------------------                                         

                                    string[] pMount_Fixture_PartNo_Comp = new string[2];
                                
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                                         Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
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
                                    else if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pMount_Fixture_PartNo_Comp[0] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[0].PartNo))[0];
                                    }
                                    else if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        pMount_Fixture_PartNo_Comp[1] = FixturePartNo(DB_In, (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Fixture[1].PartNo))[0];
                                    }
                                    
                                    pVALUE = "'" + pMount_Fixture_PartNo_Comp[0] + "," + pMount_Fixture_PartNo_Comp[1] + "'";

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

        #endregion

        #region "EXCEL MAPPING:"

            private void Populate_EXCEL_TL_TB(clsFiles Files_In, clsProject Project_In,
                                              clsDB DB_In, string FileName_In)
            //===========================================================================    
            {
                try
                {
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_TL_TB, mobjMissing, false,
                                        mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open 'Sketchs' WorkSheets.
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[1];

                    //....Set Project Number.
                    int pRowBegin = 3;
                    int pJCount = 6;
                    bool pTL_TB_Both = false;

                    int pNo_Suffix = 1;
                    if (Project_In.AssyDwg.No_Suffix != "")
                    {
                        pNo_Suffix = Convert.ToInt32(Project_In.AssyDwg.No_Suffix);
                    }

                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        if (pNo_Suffix <= 9)
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 2).ToString();
                            pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 2).ToString() + "D";
                            pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 5).ToString();

                            pWkSheet.Cells[pRowBegin + 3, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 3).ToString();
                            pWkSheet.Cells[pRowBegin + 4, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 3).ToString() + "D";
                            pWkSheet.Cells[pRowBegin + 5, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 6).ToString();
                        }
                        else
                        {
                            pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 2).ToString();
                            pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 2).ToString() + "D";
                            pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 5).ToString();

                            pWkSheet.Cells[pRowBegin + 3, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 3).ToString();
                            pWkSheet.Cells[pRowBegin + 4, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 3).ToString() + "D";
                            pWkSheet.Cells[pRowBegin + 5, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 6).ToString();
                        }
                        pJCount = 9;
                        pTL_TB_Both = true;
                    }
                    else
                    {
                        if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            if (pNo_Suffix <= 9)
                            {
                                pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 2).ToString();
                                pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 2).ToString() + "D";
                                pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 5).ToString();
                            }
                            else
                            {
                                pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 2).ToString();
                                pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 2).ToString() + "D";
                                pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 5).ToString();
                            }
                            pRowBegin++;
                        }
                        if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            if (pNo_Suffix <= 9)
                            {
                                pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 3).ToString();
                                pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 3).ToString() + "D";
                                pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 5).ToString();
                            }
                            else
                            {
                                pWkSheet.Cells[pRowBegin, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 3).ToString();
                                pWkSheet.Cells[pRowBegin + 1, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 3).ToString() + "D";
                                pWkSheet.Cells[pRowBegin + 2, 1] = Project_In.AssyDwg.No + (pNo_Suffix + 5).ToString();
                            }
                        }
                    }


                    //.....Set Value.
                    StringCollection pCellColName = new StringCollection();
                    StringCollection pSoftware_Var_Val = new StringCollection();

                    string pOrderBy = " ORDER BY fldItemNo ASC";

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_TL_TB", "fldCellColName", pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_Var_Val, "tblMapping_TL_TB", "fldSoftware_VarVal", pOrderBy);


                    for (int i = 0; i < pCellColName.Count; i++)
                    {
                        for (int j = 3; j < pJCount; j++)
                        {
                            if (pSoftware_Var_Val[i] != "")
                            {
                                int pIndx = ColumnNumber(pCellColName[i]);

                                string pSoftware_Val;
                                // pRowBegin = 3;

                                if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (pTL_TB_Both)
                                    {
                                        if (i == 0 && j == 5)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                            //pRowBegin++;
                                        }
                                        else
                                        {                                           
                                            pSoftware_Val = modMain.ExtractPreData(pSoftware_Var_Val[i], ",");
                                            if (pSoftware_Val != "")
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                            //pRowBegin++;
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0 && j == 5)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                            //pRowBegin++;
                                        }
                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPreData(pSoftware_Var_Val[i], ",");
                                            if (pSoftware_Val != "")
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                            //pRowBegin++;
                                        }
                                    }
                                }
                                if (Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (pTL_TB_Both)
                                    {
                                        if (i == 0 && j == 8)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                            // pRowBegin++;
                                        }

                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPostData(pSoftware_Var_Val[i], ",");
                                            if (pSoftware_Val != "")
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                            // pRowBegin++;
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0 && j == 5)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                            // pRowBegin++;
                                        }
                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPostData(pSoftware_Var_Val[i], ",");
                                            if (pSoftware_Val != "")
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                        }
                                    }
                                    //pRowBegin++;
                                }
                            }
                        }
                    }


                    String pFileName = FileName_In + "\\" + mcTL_TB_Title + ".xlsx";

                    if (!Directory.Exists(FileName_In))
                        Directory.CreateDirectory(FileName_In);

                    if (File.Exists(pFileName))
                        File.Delete(pFileName);

                    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;

                    pWkbOrg.SaveAs(pFileName, pWkbOrg.FileFormat, mobjMissing,
                                        mobjMissing, false, mobjMissing, pAccessMode,
                                        mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing);

                    pApp.Visible = true;
                    //}
                }
                catch (Exception pEXP)
                {
                    MessageBox.Show("Excel File Error - " + pEXP.Message);
                }
            }

        #endregion
    }
}
