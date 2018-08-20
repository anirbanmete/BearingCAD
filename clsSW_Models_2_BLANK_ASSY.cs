//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models_2_BLANK_ASSY              '
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

             private void Populate_tblMapping_Blank(clsProject Project_In, clsDB DB_In)
             //==========================================================================   
             {
                 try
                 {
                     //....Set Mapping Value.
                     StringCollection pCellColName = new StringCollection();
                     StringCollection pSoftware_VarName = new StringCollection();

                     string pOrderBy = " ORDER BY fldItemNo ASC";

                     DB_In.PopulateStringCol(pCellColName, "tblMapping_Blank", "fldCellColName", pOrderBy);
                     DB_In.PopulateStringCol(pSoftware_VarName, "tblMapping_Blank", "fldSoftware_VarName", pOrderBy);

                     String pUPDATE = "UPDATE tblMapping_Blank ";
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
                                 case "gProject.Product.Bearing.Mat.Base/ gProject.Product.Bearing.Mat.Lining":
                                     //------------------------------------------------------------------------  //Col. I        
                                     string pBaseMat = null;
                                     pBaseMat = MatAbbr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base);

                                     string pLiningMat = null;
                                     pLiningMat = MatAbbr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining);

                                     pVALUE = "'" + pBaseMat + "/" + pLiningMat + "'";
                                     break;


                                 case "gProject.Product.Bearing.SplitConfig":
                                     //---------------------------------------      //Col. K
                                     string pAny = null;

                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                         pAny = "Y";
                                     else
                                         pAny = "N";

                                     pVALUE = "'" + pAny + "'";
                                     break;

                               case "gProject.Product.Bearing.SL.Screw_Spec.Type":
                                     //-------------------------------------------      //Col. N
                                     pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Type + "'";
                                     break;


                                 case "gProject.Product.Bearing.SL.Screw_Spec.Unit.System":
                                     //----------------------------------------------------  //Col. O
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System.ToString() == "English")
                                         pVALUE = "'I'";
                                     else
                                         pVALUE = "'M'";
                                     break;

                                     
                                 case "gProject.Product.Bearing.SL.Screw_Spec.D":
                                     //-------------------------------------------          //Col. P
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig.Contains('M'))
                                     {
                                         pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig.Replace('M', ' ').Trim() + "'";
                                     }
                                     else
                                     {
                                         if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig.Contains('/'))
                                         {
                                             pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D.ToString("#0.000") + "'";
                                         }
                                         else
                                         {
                                             pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + "'";
                                         }
                                     }
                                     break;


                                 case "gProject.Product.Bearing.SL.Screw_Spec.Pitch":
                                     //-----------------------------------------------        //Col. Q
                                     pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch, "") + "'";
                                     break;


                                 case "gProject.Product.Bearing.SL.Screw_Spec.L":
                                     //--------------------------------------------          //Col. R
                                     pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.L, "") + "'";
                                     break;

                                 case "gProject.Product.Bearing.SL.Screw_Spec.Mat":
                                     //--------------------------------------------         //Col. S
                                     pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Mat + "'";
                                     break;


                                 case "gProject.Product.Bearing.SL.Dowel_Spec.Type ":
                                     //----------------------------------------------       //Col. AB
                                     pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Type + "'";
                                     break;


                                 case "gProject.Product.Bearing.SL.Dowel_Spec.Unit.System":
                                     //---------------------------------------------------- //Col. AC
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System.ToString() == "English")
                                         pVALUE = "'I'";
                                     else
                                         pVALUE = "'M'";
                                     break;

                                 case "gProject.Product.Bearing.SL.Dowel_Spec.D":
                                     //------------------------------------------  //Col. AD
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig.Contains('M'))
                                     {
                                         pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig.Replace('M', ' ').Trim() + "'";
                                     }
                                     else
                                     {
                                         if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig.Contains('/'))
                                         {
                                             pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D.ToString("#0.000") + "'";
                                         }
                                         else
                                         {
                                             pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig + "'";
                                         }
                                     }
                                     break;


                                 case "gProject.Product.Bearing.SL.Dowel_Spec.L":
                                     //------------------------------------------   //Col. AE
                                     pVALUE = "'" + modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.L, "") + "'";
                                     break;


                                 case "gProject.Product.Bearing.SL.Dowel_Spec.Mat":
                                     //-------------------------------------------- //Col. AF
                                     pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Mat + "'";
                                     break;

                                 case "gProject.Product.Bearing.SL.Screw_Spec":
                                     //----------------------------------------     //Col. BJ
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System == clsUnit.eSystem.Metric)
                                     {
                                         //if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig.Contains('M'))
                                         //{
                                         //    pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig.Replace('M', ' ').Trim() + "x" +
                                         //         modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch, "") +
                                         //         " THREAD'";
                                         //}   

                                         pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + "x" +
                                                  modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch, "") +
                                                  " THREAD'";

                                     }
                                     else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Unit.System == clsUnit.eSystem.English)
                                     {
                                         pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.D_Desig + "-" +
                                                 modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw_Spec.Pitch, "") +
                                                 "UNC'";
                                     }
                                     break;

                                 case "gProject.Product.Bearing.SL.Dowel_Spec":
                                     //-----------------------------------------//Col. BK
                                     if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System == clsUnit.eSystem.Metric)
                                     {
                                         if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig.Contains('M'))
                                         {
                                             pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig.Replace('M', ' ').Trim() + "mm'";
                                         }
                                     }
                                     else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.Unit.System == clsUnit.eSystem.English)
                                     {
                                         pVALUE = "'" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Spec.D_Desig + "'";
                                     }

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

             private void PopulateAndMove_DT_BlankAssy(clsFiles Files_In, clsProject Project_In,
                                                       clsDB DB_In, string FilePath_In)
             //=================================================================================
             {
                 try
                 {
                     EXCEL.Application pApp = null;
                     pApp = new EXCEL.Application();

                     //....Open Original WorkBook.
                     EXCEL.Workbook pWkbOrg = null;

                     pWkbOrg = pApp.Workbooks.Open(Files_In.FileTitle_Template_EXCEL_BlankAssy, mobjMissing, false,
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
                         pWkSheet.Cells[3, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 1).ToString();
                         pWkSheet.Cells[4, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 1).ToString() + "T";
                         pWkSheet.Cells[5, 1] = Project_In.AssyDwg.No + "-0" + (pNo_Suffix + 4).ToString();
                     }
                     else
                     {
                         pWkSheet.Cells[3, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 1).ToString();
                         pWkSheet.Cells[4, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 1).ToString() + "T";
                         pWkSheet.Cells[5, 1] = Project_In.AssyDwg.No + "-" + (pNo_Suffix + 4).ToString();
                     }


                     //....Set Value.
                     StringCollection pCellColName = new StringCollection();
                     StringCollection pSoftware_VarVal = new StringCollection();

                     DB_In.PopulateStringCol(pCellColName, "tblMapping_Blank", "fldCellColName", "");
                     DB_In.PopulateStringCol(pSoftware_VarVal, "tblMapping_Blank", "fldSoftware_VarVal", "");


                     for (int i = 0; i < pCellColName.Count; i++)
                     {
                         for (int j = 3; j < 6; j++)
                         {
                             if (pSoftware_VarVal[i] != "")
                             {
                                 int pIndx = ColumnNumber(pCellColName[i]);
                                 pWkSheet.Cells[j, pIndx] = pSoftware_VarVal[i];
                             }
                         }
                     }

                     string pFile = modMain.ExtractPreData(mcBlankAssy_Title, ".");
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
