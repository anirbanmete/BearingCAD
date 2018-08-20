//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       CODE MODULE  :  clsEXCEL_Analysis                      '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  : 05APR13                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXCEL = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace BearingCAD20
{
    class clsEXCEL_Analysis    
    {
        #region "CLASS METHODS:"

            #region "XLKMC:"

                public void Retrieve_Params_XLKMC(string ExcelFileName_In,
                                                  List<string> ParameterName_In, List<string> ExcelSheetName_In, List<string> CellName_In,
                                                  List<string> CellRangeStart_In, List<string> CellRangeEnd_In, clsOpCond OpCond_In, clsBearing_Radial_FP Bearing_In)  
                //=========================================================================================================================
                {
                    CloseExcelFiles();       

                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    pApp.DisplayAlerts = false; //Don't want Excel to display error messageboxes

                    //....Open Load.xls WorkBook.
                    EXCEL.Workbook pWkbOrg = null;
                    pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value);

                    string pVal = "";                    
                    double pVal_Out = 0.0F;
                    Boolean pblnError = false;
                    string pMsg = "Speed on the" + "\"" + "Preload Flexure Pivot" + "\"" + "\n" +
                                                    "does not match with speeds given in" + "\"" + "XLRADIAL" + "\"" + "worksheet" + "\n" +
                                                    "Please correct the file and try to read the file again.";

                    for (int i = 0; i < ParameterName_In.Count; i++)
                    {
                        if (pblnError)
                        {
                            break;
                        }

                        EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[ExcelSheetName_In[i]];
                        EXCEL.Range pExcelCellRange = null;
                        if (CellName_In[i] == "")
                        {
                            continue;
                        }
               
                        pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range(CellName_In[i], Missing.Value);

                        if (((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2) != "") 
                        {
                            pVal = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2);
                        }
             
                        switch (ParameterName_In[i])
                        {
                            case "INPUT SPEED(rpm)":

                                OpCond_In.Speed_Range[0] = Convert.ToInt32(pVal);          
                                OpCond_In.Speed_Range[1] = Convert.ToInt32(pVal);

                                break;

                            case "Journal-Min":

                                Bearing_In.DShaft_Range[0] = Convert.ToDouble(pVal);
                                //Bearing_In.Pad.T_Pivot = Bearing_In.Pad.TDef();         //BG 25MAR13 
                                //Bearing_In.Pad.T_Lead = Bearing_In.Pad.T.Pivot;
                                //Bearing_In.Pad.T_Trail = Bearing_In.Pad.T.Pivot;
                                break;

                            case "Journal-Max":

                                Bearing_In.DShaft_Range[1] = Convert.ToDouble(pVal);
                                //Bearing_In.Pad.T_Pivot = Bearing_In.Pad.TDef();         //BG 25MAR13
                                //Bearing_In.Pad.T_Lead = Bearing_In.Pad.T.Pivot;
                                //Bearing_In.Pad.T_Trail = Bearing_In.Pad.T.Pivot;
                                break;

                            case "Bearing-Min":

                                Bearing_In.DSet_Range[0] = Convert.ToDouble(pVal);
                                Bearing_In.LiningT = Bearing_In.Mat_Lining_T();     //BG 03APR13
                                break;

                            case "Bearing-Max":

                                Bearing_In.DSet_Range[1] = Convert.ToDouble(pVal);
                                Bearing_In.LiningT = Bearing_In.Mat_Lining_T();     //BG 03APR13
                                break;

                            case "Pad-Min":

                                Bearing_In.DPad_Range[0] = Convert.ToDouble(pVal);
                                break;

                            case "Pad-Max":

                                Bearing_In.DPad_Range[1] = Convert.ToDouble(pVal);
                                break;

                            case "No.Of Pads":

                                Bearing_In.Pad.Count = Convert.ToInt16(pVal);
                                break;

                            case "Pivot Locations - LBP":

                                Bearing_In.Pad.Pivot_AngStart_LBP_XLKMC = Convert.ToDouble(pVal);       //AM 14AUG12
                                break;

                            case "Pivot Locations - LOP":

                                Bearing_In.Pad.Pivot_AngStart_LOP_XLKMC = Convert.ToDouble(pVal);       //AM 14AUG12
                                break;

                            case "X Load":

                                double pX_Load = 0.0F, pY_Load = 0.0F, pOil_Flow_Req = 0.0F;
                                //Retrieve_LoadAndOilFlow(OpCond_In.Speed(), pWkSheet, pExcelCellRange, ref pX_Load, ref pY_Load, ref pOil_Flow_Req, ref pSpeed, ref pblnError);
                                //pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range(CellName_In[i], "G51");
                                pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range(CellName_In[i], CellRangeEnd_In[i]);
                                Retrieve_LoadAndOilFlow_RefSpeed(pExcelCellRange, OpCond_In.Speed(), ref pX_Load, ref pY_Load, ref pOil_Flow_Req, ref pblnError);
                                if (pblnError)
                                {
                                    MessageBox.Show(pMsg, "Error", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Error);                            
                                    break;
                                }

                                //....pOil_Flow_Req is in cips.
                                Bearing_In.PerformData.FlowReqd_gpm = pOil_Flow_Req / 3.85;         //....1 gpm = 3.85 cips (as per HK's instruction on 04OCT12).
                                double pLoad_Des = 0.0F;
                                const double pcRad2Deg = 180.0 / Math.PI;
                                pLoad_Des = Math.Sqrt(Math.Pow(pX_Load, 2) + Math.Pow(pY_Load, 2));
                                OpCond_In.Radial_Load_Range[0] = pLoad_Des;
                                OpCond_In.Radial_Load_Range[1] = pLoad_Des;

                                //AM 02JAN13
                                double pLoadAng = 0.0F;
                                pLoadAng = Math.Atan2(pY_Load, pX_Load) * pcRad2Deg;

                                if (pLoadAng < 0)
                                    pLoadAng = pLoadAng + 360;

                                OpCond_In.Radial_LoadAng = pLoadAng; 
                                break;

                            case "Selected Lubricant":
                                Microsoft.Office.Interop.Excel.DropDown pCmbBox =
                                (Microsoft.Office.Interop.Excel.DropDown)pWkSheet.Shapes.Item("SetLubricantProperties").OLEFormat.Object;

                                int pIndex = pCmbBox.Value;
                                string pText = pCmbBox.get_List(pIndex).ToString();
                                OpCond_In.OilSupply_Lube_Type = pText;                      
                                break;

                            case "Oil Supply Temp":
                                OpCond_In.OilSupply_Temp = Convert.ToDouble(pVal);
                                break;

                            case "Pivot Rotational Krot":
                                Bearing_In.FlexurePivot.Rot_Stiff = Convert.ToDouble(pVal);
                                break;

                            case "Pad Arc (degrees)":
                                Bearing_In.Pad.Angle();
                                break;

                            case "Offset Fac":
                                //AM 14AUG12
                                double pPivot_Offset = Convert.ToDouble(pVal);
                        
                                if (pPivot_Offset > 0.5)
                                {
                                    OpCond_In.Rot_Directionality = clsOpCond.eRotDirectionality.Uni;                                                     
                                }
                                else
                                {
                                    OpCond_In.Rot_Directionality = clsOpCond.eRotDirectionality.Bi;                            
                                }
                                Bearing_In.Pad.Pivot_Offset = modMain.NInt(pPivot_Offset * 100); 
                        
                                break;

                            case "Pivot Angle(degress)":
                                Bearing_In.Pad.Pivot_AngStart = Convert.ToDouble(pVal);
                        
                                if (Math.Abs(Bearing_In.Pad.Pivot.AngStart - Bearing_In.Pad.Pivot.AngStart_LBP_XLKMC) < modMain.gcEPS)
                                    Bearing_In.Pad.Type = clsBearing_Radial_FP.clsPad.eLoadPos.LBP;
                                else if (Math.Abs(Bearing_In.Pad.Pivot.AngStart - Bearing_In.Pad.Pivot.AngStart_LOP_XLKMC) < modMain.gcEPS)
                                    Bearing_In.Pad.Type = clsBearing_Radial_FP.clsPad.eLoadPos.LOP;
                        
                                break;

                            case "Pad Axial Length (in)":
                                Bearing_In.Pad.L = Convert.ToDouble(pVal);
                                break;

                            case "Web t (in)":
                                Bearing_In.FlexurePivot.Web_T = Convert.ToDouble(pVal);
                                break;

                            case "Web H (in)":
                                Bearing_In.FlexurePivot.Web_H = Convert.ToDouble(pVal);
                                break;

                            case "Power hp":

                                pVal_Out = 0.0F;
                                Retrieve_Params_RefSpeed(pWkSheet, OpCond_In.Speed(), CellName_In[i], CellRangeStart_In[i],
                                                         ref pVal_Out, ref pblnError);

                                if (pblnError)
                                {
                                    MessageBox.Show(pMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                           
                                    break;
                                }

                                Bearing_In.PerformData.Power_HP = pVal_Out;

                                break;

                            case "H Min (in)":
                                pVal_Out = 0.0F;
                                Retrieve_Params_RefSpeed(pWkSheet, OpCond_In.Speed(), CellName_In[i], CellRangeStart_In[i], ref pVal_Out, ref pblnError);
                                Bearing_In.PerformData.TFilm_Min = pVal_Out;
                                break;

                            case "Babbit Tm Deg. F":
                                pVal_Out = 0.0F;
                                Retrieve_Params_RefSpeed(pWkSheet,OpCond_In.Speed(), CellName_In[i],CellRangeStart_In[i], ref pVal_Out, ref pblnError);
                                Bearing_In.PerformData.PadMax_Temp = pVal_Out;
                                break;

                            case "P Max (psi)":
                                pVal_Out = 0.0F;
                                Retrieve_Params_RefSpeed(pWkSheet,OpCond_In.Speed(), CellName_In[i], CellRangeStart_In[i],ref pVal_Out, ref pblnError);
                                Bearing_In.PerformData.PadMax_Press = pVal_Out;
                                break;

                            case "Pad Rotation (rad)":
                                pVal_Out = 0.0F;
                                Retrieve_Max_Params(pWkSheet, CellName_In[i], ref pVal_Out);
                                Bearing_In.PerformData.PadMax_Rot = pVal_Out;
                                break;

                            case "W_stress (psi)":
                                pVal_Out = 0.0F;
                                Retrieve_Max_Params(pWkSheet, CellName_In[i], ref pVal_Out);
                                Bearing_In.PerformData.PadMax_Stress = pVal_Out;
                                break;

                            case "Pad Load (lb)":
                                pVal_Out = 0.0F;
                                Retrieve_Max_Params(pWkSheet, CellName_In[i], ref pVal_Out);
                                Bearing_In.PerformData.PadMax_Load = pVal_Out;
                                break;  
                        }  
                    }

                    pWkbOrg.Close();
                    pApp.Quit();
                }


                #region "Helper Routines:"
                //************************

                private void Retrieve_LoadAndOilFlow_RefSpeed(EXCEL.Range Range_In, int Speed_In, 
                                                              ref double X_Load_out, ref double Y_Load_Out, 
                                                              ref double Oil_Flow_Req_Out, ref bool Error_Out)
                    //=========================================================================================   
                    {
                       // Range_In = (EXCEL.Range)WorkSheet_In.get_Range("A41", "G51");
                        

                        int pColCount = Range_In.Columns.Count;

                        Double[] pX_Load = new Double[Range_In.Rows.Count];
                        Double[] pY_Load = new Double[Range_In.Rows.Count];
                        Double[] pOil_Flow_Req = new Double[Range_In.Rows.Count];
                        int[] pSpeed = new int[Range_In.Rows.Count];


                        for (int i = 1; i <= Range_In.Rows.Count; i++)
                        {
                            if (((EXCEL.Range)Range_In.Cells[i, 1]).Value2 != null)
                            {
                                pX_Load[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 1]).Value2.ToString());
                            }
                            else
                            {
                                pX_Load[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 2]).Value2 != null)
                            {
                                pY_Load[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 2]).Value2.ToString());
                            }
                            else
                            {
                                pY_Load[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 5]).Value2 != null)
                            {
                                pOil_Flow_Req[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 5]).Value2.ToString());
                            }
                            else
                            {
                                pOil_Flow_Req[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 7]).Value2 != null)
                            {
                                pSpeed[i - 1] = Convert.ToInt32(((EXCEL.Range)Range_In.Cells[i, 7]).Value2.ToString());
                            }
                            else
                            {
                                pSpeed[i - 1] = 0;
                            }

                        }

                        for (int j = 0; j < pSpeed.Length; j++)
                        {
                            if (pSpeed[j] == Speed_In)
                            {
                                X_Load_out = pX_Load[j] * (-1);
                                Y_Load_Out = pY_Load[j] * (-1);
                                Oil_Flow_Req_Out = pOil_Flow_Req[j];
                                //Speed_Out = pSpeed[j];
                                Error_Out = false;
                                break;
                            }
                            else
                            {
                                Error_Out = true;
                            }
                        }
                    }


                    private void Retrieve_Params_RefSpeed (EXCEL.Worksheet WkSheet_In, int Speed_In, string CellName_In, 
                                                           string CellRangeStart_In, ref double Val_Out, ref bool Error_Out)
                    //=======================================================================================================
                    {
                        int pcRowCount = 10;

                        string pColumnNo = CellName_In.Substring(0, 1);
                        int pRowNo = Convert.ToInt32(CellName_In.Substring(1, CellName_In.Length - 1));
                        List<double> pValList = new List<double>();
                        //EXCEL.Range pRange = (EXCEL.Range)WkSheet_In.get_Range("A55", pColumnNo + (pRowNo + pcRowCount));
                        EXCEL.Range pRange = (EXCEL.Range)WkSheet_In.get_Range(CellRangeStart_In, pColumnNo + (pRowNo + pcRowCount));

                        Double[] pVal = new double[pRange.Rows.Count];
                        Double[] pSpeed = new double[pRange.Rows.Count];

                        int pRef_ASCII = (int)pColumnNo[0] - 64;    //....ASCII of char 'A' = 64.


                        for (int i = 1; i <= pRange.Rows.Count; i++)
                        {
                            if (((EXCEL.Range)pRange.Cells[i, 1]).Value2 != null)
                            {
                                pSpeed[i - 1] = Convert.ToDouble(((EXCEL.Range)pRange.Cells[i, 1]).Value2.ToString());
                            }
                            else
                            {
                                pSpeed[i - 1] = 0;
                            }

                            if (((EXCEL.Range)pRange.Cells[i, pRef_ASCII]).Value2 != null)
                            {
                                pVal[i - 1] = Convert.ToDouble(((EXCEL.Range)pRange.Cells[i, pRef_ASCII]).Value2.ToString());
                            }
                            else
                            {
                                pVal[i - 1] = 0;
                            }

                        }

                        for (int j = 0; j < pVal.Length; j++)
                        {
                            if (Speed_In == pSpeed[j])
                            {
                                Val_Out = pVal[j];
                                Error_Out = false;
                                break;
                            }   
                            else
                                Error_Out = true;
                        }  
                    }


                    private void Retrieve_Max_Params (EXCEL.Worksheet WkSheet_In, string CellNo_In, ref double Val_Out)
                    //===================================================================================================
                    {
                        string pColumnNo = CellNo_In.Substring(0, 1);
                        int pRowNo = Convert.ToInt32(CellNo_In.Substring(1, CellNo_In.Length - 1));
                        List<double> pValList = new List<double>();
                        EXCEL.Range pRange = (EXCEL.Range)WkSheet_In.get_Range(CellNo_In, pColumnNo + (pRowNo +((clsBearing_Radial_FP) modMain.gProject.Product.Bearing).Pad.Count));

                        for (int i = 1; i <= ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count; i++)
                        {
                            Double pVali = Math.Abs(Convert.ToDouble(((EXCEL.Range)pRange.Cells[i, 1]).Value2.ToString()));
                            pValList.Add(pVali);
                        }

                        pValList.Sort();

                        Val_Out = pValList[pValList.Count - 1];
                    }

                #endregion

            #endregion


            #region "XLTHRUST:"

                public void Retrieve_Params_XLTHRUST (string ExcelFileName_In,  List<string> ParameterName_In, List<string> ExcelSheetName_In, 
                                                      List<string> CellName_In, List<string> CellRangeStart_In, List<string> CellRangeEnd_In, 
                                                      clsOpCond OpCond_In, int EndConfig_Pos_In, clsBearing_Thrust_TL ThrustBearing_In)
                //=============================================================================================================================     //BG 03JAN12
                {
                    CloseExcelFiles();      

                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();

                    pApp.DisplayAlerts = false;     

                    //....Open Load.xls WorkBook.
                    EXCEL.Workbook pWkbOrg = null;
                    pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                    Missing.Value, Missing.Value, Missing.Value);

                    string pVal = "";

                    Boolean pblnError = false;
                    string pMsg = "Speed on the" + "\"" + "Preload Flexure Pivot" + "\"" + "\n" +
                                                    "does not match with speeds given in" + "\"" + "XLTHRUST" + "\"" + "worksheet" + "\n" +
                                                    "Please correct the file and try to read the file again.";
           
                    for (int i = 0; i < ParameterName_In.Count; i++)
                    {               
                        pVal = "";
                        EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets[ExcelSheetName_In[i]];
                        EXCEL.Range pExcelCellRange = null;

                        if (CellName_In[i] == "")
                        {
                            continue;
                        }

                        pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range(CellName_In[i], Missing.Value);
                        if (((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2) != "") //BG 01OCT12
                        {
                            pVal = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[1, 1]).Value2);
                        }

                        switch (ParameterName_In[i])
                        {
                            case "Outer Diameter":
                                ThrustBearing_In.PadD[1] = Convert.ToDouble(pVal);
                            break;

                            case "Inner Diameter":
                                ThrustBearing_In.PadD[0] = Convert.ToDouble(pVal);
                            break;

                            case "Number of Pads":
                                ThrustBearing_In.Pad_Count = Convert.ToInt16(pVal);
                            break;

                            case "S":
                                ThrustBearing_In.FeedGroove.Wid = Convert.ToDouble(pVal);
                            break;

                            case "weep hole":                             
                                Microsoft.Office.Interop.Excel.DropDown pCmbBox =
                                        (Microsoft.Office.Interop.Excel.DropDown)pWkSheet.Shapes.Item("ddWeepHole_Flag").OLEFormat.Object;
                                                       
                                int pIndex = pCmbBox.Value;
                                string pText = pCmbBox.get_List(pIndex).ToString();
                                       
                                if (pText == "V-notch")     
                                {
                                    pText = "V_notch";
                                }

                                if (pText == "Circlur")         
                                {
                                    pText = "Circular";
                                }

                                ThrustBearing_In.WeepSlot.Type = (clsBearing_Thrust_TL.clsWeepSlot.eType)
                                                                Enum.Parse(typeof(clsBearing_Thrust_TL.clsWeepSlot.eType), pText);
                            break;

                            case "b (width - in)":
                                ThrustBearing_In.WeepSlot.Wid = Convert.ToDouble(pVal);
                            break;

                            case "a (depth - in)":
                                if (pVal != "")
                                {
                                    ThrustBearing_In.WeepSlot.Depth = Convert.ToDouble(pVal);
                                }
                                else
                                {
                                    ThrustBearing_In.WeepSlot.Depth = 0.0F;
                                }
                            break;

                            case "Depth at Inner Inlet 1 (in)":
                                ThrustBearing_In.Taper_Depth_ID = Convert.ToDouble(pVal);
                            break;

                            case "Depth at Outer Inlet 2 (in)":
                                ThrustBearing_In.Taper_Depth_OD = Convert.ToDouble(pVal);
                            break;

                            case "BETA (deg.)":
                                ThrustBearing_In.Taper_Angle = Convert.ToDouble(pVal);
                            break;

                            case "Ro (in)":
                                ThrustBearing_In.Shroud_Ro = Convert.ToDouble(pVal);
                            break;

                            case "Ri (in)":
                                ThrustBearing_In.Shroud_Ri = Convert.ToDouble(pVal);
                            break;

                            case "Fric. Loss":           
                                double pLoad_Des = 0.0F;
                                double pPower_HP_Out = 0.0F, pFlowReqd_gpm_Out = 0.0F;
                                double pTFilm_Min_Out = 0.0F, pPadMax_Temp_Out = 0.0F, pPadMax_Press_Out = 0.0F;

                                //pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range("A33", "Q42");
                                pExcelCellRange = (EXCEL.Range)pWkSheet.get_Range(CellRangeStart_In[i], CellRangeEnd_In[i]);

                                Retrieve_Params_PerformanceData(pExcelCellRange, OpCond_In.Speed(), ref pLoad_Des, ref pPower_HP_Out,
                                                         ref pFlowReqd_gpm_Out, ref pTFilm_Min_Out, ref pPadMax_Temp_Out,
                                                         ref pPadMax_Press_Out, ref pblnError);
                      
                                if (pblnError)
                                {
                                    MessageBox.Show(pMsg, "Error", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Error);
                                    break;
                                }

                                ThrustBearing_In.PerformData.Power_HP = pPower_HP_Out;

                                if (EndConfig_Pos_In == 0 && modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    OpCond_In.Thrust_Load_Range_Front[0] = pLoad_Des;
                                    OpCond_In.Thrust_Load_Range_Front[1] = pLoad_Des;
                                }

                                if (EndConfig_Pos_In == 1 && modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    OpCond_In.Thrust_Load_Range_Back[0] = pLoad_Des;
                                    OpCond_In.Thrust_Load_Range_Back[1] = pLoad_Des;
                                }
                                                                            
                                ThrustBearing_In.PerformData.FlowReqd_gpm = pFlowReqd_gpm_Out;
                                //ThrustBearing_In.PerformData.TempRise_F = 12.4 * (pPower_HP_Out / pFlowReqd_gpm_Out);       //BG 10JAN12
                                ThrustBearing_In.PerformData.TFilm_Min = pTFilm_Min_Out;
                                ThrustBearing_In.PerformData.PadMax_Temp = pPadMax_Temp_Out;
                                ThrustBearing_In.PerformData.PadMax_Press = pPadMax_Press_Out;
                            break;

                            case "Unit Load for Actual Area":
                                ThrustBearing_In.PerformData.UnitLoad= Convert.ToDouble(pVal);
                            break;

                        }
                    }
                }


                #region "Helper Routines:"
                //************************

                    private void Retrieve_Params_PerformanceData(EXCEL.Range Range_In, int Speed_In,
                                                                 ref double Load_Out, ref double Power_HP_Out, 
                                                                 ref double FlowReqd_gpm_Out, ref double TFilm_Min_Out, 
                                                                 ref double PadMax_Temp_Out,ref double PadMax_Press_Out, 
                                                                 ref bool Error_Out)
                    //===================================================================================================     
                    {
                        //Range_In = (EXCEL.Range)WkSheet_In.get_Range("A33", "Q42");

                        int pRowCount = Range_In.Rows.Count;

                        Int32[] pSpeed = new Int32[pRowCount];
                        Double[] pLoad_Out = new double[pRowCount];     
                        Double[] pPower_HP_Out = new double[pRowCount];
                        Double[] pFlowReqd_gpm_Out = new double[pRowCount];
                        Double[] pTFilm_Min_Out = new double[pRowCount];
                        Double[] pPadMax_Temp_Out = new double[pRowCount];
                        Double[] pPadMax_Press_Out = new double[pRowCount];                    

                        for (int i = 1; i <= Range_In.Rows.Count; i++)
                        {
                            if (((EXCEL.Range)Range_In.Cells[i, 1]).Value2 != null)
                            {
                                pSpeed[i - 1] = Convert.ToInt32(((EXCEL.Range)Range_In.Cells[i, 1]).Value2.ToString());
                            }
                            else
                            {
                                pSpeed[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 2]).Value2 != null)
                            {
                                pLoad_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 2]).Value2.ToString());
                            }
                            else
                            {
                                pLoad_Out[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 10]).Value2 != null)
                            {
                                pPower_HP_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 10]).Value2.ToString());
                            }
                            else
                            {
                                pPower_HP_Out[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 6]).Value2 != null)
                            {
                                pFlowReqd_gpm_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 6]).Value2.ToString());
                            }
                            else
                            {
                                pFlowReqd_gpm_Out[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 9]).Value2 != null)
                            {
                                pTFilm_Min_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 9]).Value2.ToString());
                            }
                            else
                            {
                                pTFilm_Min_Out[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 15]).Value2 != null)
                            {
                                pPadMax_Temp_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 15]).Value2.ToString());
                            }
                            else
                            {
                                pPadMax_Temp_Out[i - 1] = 0;
                            }

                            if (((EXCEL.Range)Range_In.Cells[i, 17]).Value2 != null)
                            {
                                pPadMax_Press_Out[i - 1] = Convert.ToDouble(((EXCEL.Range)Range_In.Cells[i, 17]).Value2.ToString());
                            }
                            else
                            {
                                pPadMax_Press_Out[i - 1] = 0;
                            }
                        }

                        for (int j = 0; j < pSpeed.Length; j++)
                        {
                            if (Speed_In == pSpeed[j])
                            {
                                Power_HP_Out = pPower_HP_Out[j];
                                Load_Out = pLoad_Out[j];
                                FlowReqd_gpm_Out = pFlowReqd_gpm_Out[j];
                                TFilm_Min_Out = pTFilm_Min_Out[j];
                                PadMax_Temp_Out = pPadMax_Temp_Out[j];
                                PadMax_Press_Out = pPadMax_Press_Out[j];
                                Error_Out = false;
                                break;
                            }
                            else
                                Error_Out = true;
                        }
                    }

                #endregion

            #endregion


            #region "UTILITY ROUTINES:"

                public void CloseExcelFiles()
                //===========================      
                {
                    Process[] pProcesses = Process.GetProcesses();

                    try
                    {
                        foreach (Process p in pProcesses)
                            if (p.ProcessName == "EXCEL")
                                p.Kill();
                    }
                    catch (Exception pEXP)
                    {

                    }
                }

             


            #endregion

        #endregion
    }
}
