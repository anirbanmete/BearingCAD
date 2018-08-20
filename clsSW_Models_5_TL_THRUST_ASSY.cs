//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models_5_TL_THRUST_ASSY          '
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

        #region "DATABASE MAPPING:"

            private void Populate_tblMapping_TB_Assy(clsProject Project_In, clsDB DB_In)
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

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_TB_Assy", "fldCellColName", pOrderBy);
                    DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_TB_Assy", "fldSoftware_VarName", pOrderBy);

                    String pUPDATE = "UPDATE tblMapping_TB_Assy ";
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
                                case "gProject.Product.EndConfig[i].DirectionType":
                                    //---------------------------------------------     //Col. B
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

                                case "gProject.Product.EndConfig[i].Mat.Base/.Mat.Lining":
                                    //----------------------------------------------------      //Col. J
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

                                case "gProject.Product.Bearing.SplitConfig":
                                    //---------------------------------------       //Col. L
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

            private void PopulateAndMove_DT_TB_Assy(clsFiles Files_In, clsProject Project_In,
                                                    clsDB DB_In, string FilePath_In)
            //================================================================================    
            {
                try
                {
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_TB_Assy, mobjMissing, false,
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

                    DB_In.PopulateStringCol(pCellColName, "tblMapping_TB_Assy", "fldCellColName", "");
                    DB_In.PopulateStringCol(pSoftware_Var_Val, "tblMapping_TB_Assy", "fldSoftware_VarVal", "");

                    for (int i = 0; i < pCellColName.Count; i++)
                    {
                        for (int j = 3; j < pJCount; j++)
                        {
                            if (pSoftware_Var_Val[i] != "")
                            {
                                int pIndx = ColumnNumber(pCellColName[i]);

                                string pSoftware_Val;

                                if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (pTL_TB_Both)
                                    {
                                        if (i == 0 && j == 7)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                        }

                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPreData(pSoftware_Var_Val[i], ",");
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0 && j == 5)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                        }

                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPreData(pSoftware_Var_Val[i], ",");
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
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
                                        }

                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPostData(pSoftware_Var_Val[i], ",");
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0 && j == 5)
                                        {
                                            //pWkSheet.Cells[j, pIndx] = "B";
                                        }
                                        else
                                        {
                                            pSoftware_Val = modMain.ExtractPostData(pSoftware_Var_Val[i], ",");
                                            pWkSheet.Cells[j, pIndx] = pSoftware_Val;
                                        }
                                    }
                                }

                            }
                        }

                    }

                    string pFile = modMain.ExtractPreData(mcTBAssy_Title, ".");
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
