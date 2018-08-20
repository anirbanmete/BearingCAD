
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       CODE MODULE  :  modMain                                '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  14JAN13                                '
//                                                                              '
//===============================================================================
//
//  Routines :
//  ---------
//      Public static void          LoadImageLogo        ()
//      public static int           NInt                 ()  
//
//      public static Single        ConvTextToSingle     ()
//      public static int           ConvTextToInt        ()   
//      public static Boolean       ConvTextToBoolean    ()
//
//      public static string        ExtractPreData       ()  
//      public static string        ExtractPostData      () 
//      public static string        ExtractMidData       ()
//
//      public  static int          CompareVar           ()
//'==============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Specialized;

namespace BearingCAD20
{
    static class modMain
    {
        #region "NAMED CONSTANTS:"
        //========================

            //....An aribitrarily small number. 
            public static float gcEPS = 0.00000001F;

            //....Conversion factor.
            public const Double gc_RAD2DEG = 180.0 / Math.PI;


            //Program Name & Version No.
            //--------------------------
             public const String gcstrProgramName = "BearingCAD";
             public const String gcstrVersionNo = "2.0";

             public static Bitmap mMemoryImage;        

        #endregion


        #region  "USER-DEFINED CLASS OBJECTS:"
        //===================================
            public static clsUser gUser = new clsUser();

            public static clsUnit gUnit = new clsUnit();
            public static clsProject gProject;
            public static clsOpCond gOpCond = new clsOpCond();  

            public static clsFiles gFiles = new clsFiles();
            public static clsDB gDB = new clsDB();
                        
            public static clsReport gReport = new clsReport();
            public static clsEXCEL_Analysis gEXCEL_Analysis = new clsEXCEL_Analysis();  //PB 03JAN12. To be reviewed.   //Used in frmOpCond and frmThrustBearing
            public static clsEndMill gEndMill = new clsEndMill();

        #endregion   
    

        #region  "FORM OBJECTS:"
        //=====================

            public static frmLogIn gfrmLogIn = new frmLogIn();
            public static frmPassword gfrmPassword = new frmPassword();

            public static frmMain gfrmMain = new frmMain();

            public static frmProject gfrmProject = new frmProject();
            //public static frmImportData gfrmImportData = new frmImportData();
            public static frmOpCond gfrmOperCond = new frmOpCond();

            public static frmBearing gfrmBearing = new frmBearing();
            public static frmAccessories gfrmAccessories = new frmAccessories();    
            public static frmSeal gfrmSeal = new frmSeal();
            public static frmThrustBearing gfrmThrustBearing = new frmThrustBearing();

            public static frmPerformDataBearing gfrmPerformDataBearing = new frmPerformDataBearing();
            
            public static frmBearingDesignDetails gfrmBearingDesignDetails = new frmBearingDesignDetails();
            public static frmSealDesignDetails gfrmSealDesignDetails = new frmSealDesignDetails();
            public static frmThrustBearingDesignDetails gfrmThrustBearingDesignDetails = new frmThrustBearingDesignDetails();

            public static frmCreateFiles gCreateFiles = new frmCreateFiles();
           
            public static frmFilter gfrmFilter = new frmFilter();                       
            public static frmCopyProject gfrmCopyProject = new frmCopyProject();

            public static frmGCodes gfrmGCodes = new frmGCodes();

        #endregion


        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        //================
        {
            Application.EnableVisualStyles();            
            Application.Run(new frmSplash());
        }

        #region "UTILITY ROUTINES:"
        //========================

            public static Single Nom_Val(Single[] Val_Range)
            //==============================================        
            {   //....Given the min & max dimension ranges, calculate nominal value. 
                return (Single)0.5 * (Val_Range[0] + Val_Range[1]);
            }

            public static Double Nom_Val(Double[] Val_Range)        
            //==============================================        
            {   //....Given the min & max dimension ranges, calculate nominal value. 
                return 0.5 * (Val_Range[0] + Val_Range[1]);
            }

            public static int Nom_Val(int[] Val_Range)        
            //==============================================        
            {   //....Given the min & max dimension ranges, calculate nominal value. 
                return (int)(0.5 * (Val_Range[0] + Val_Range[1]));
            }


            public static void LoadImageLogo(PictureBox imgControl)
            //=================================================== 
            {
                try
                {imgControl.Image = System.Drawing.Image.FromFile(gFiles.Logo);}

                catch (FileNotFoundException  pEXP )
                {
                    MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, 
                                                                    MessageBoxIcon.Error); }
              }


             public static int NInt (Double Dbl_In)
             //======================================      
             {
                 //....This function returns the nearest integer value of 'double'.
                 Double pDecimalPart;
                 pDecimalPart = Dbl_In - Convert.ToInt64(Dbl_In);
              
                 int pNInt = 0;

                 if (pDecimalPart < 0.5)
                 {
                     pNInt = Convert.ToInt32(Dbl_In);
                 }

                 else if (pDecimalPart >= 0.5)
                 {
                     pNInt = Convert.ToInt32(Dbl_In) + 1;
                 }

                 return pNInt;
             }


             public static Double MRound(Double Number_In, Double Multiple_In)         
             //===============================================================
             {//....Returns the given number rounded to the given multiple. 

                 //....Nearest # of Multiples.
                 int pCount;
                 pCount = (Int16)Math.Round(Number_In / Multiple_In, 0);

                 return (pCount * Multiple_In);
             }


            //....Load Combo box with enum type variables
             public static void LoadCmbBox(ComboBox CmbBox_In, Array Array_In)
             //================================================================       
             {               
                 CmbBox_In.Items.Clear();
                
                 for (int i = 0; i < Array_In.Length; i++)
                 {
                     string pName = Array_In.GetValue(i).ToString();
                     if (pName.Contains("_"))
                     {
                         pName = pName.Replace("_", " ");
                     }
                     CmbBox_In.Items.Add(pName);
                 }

                 CmbBox_In.SelectedIndex = 0;
             }


             public static void SortNumberwHash(ref StringCollection Str_In)
             //=============================================================
             {
                 Int32[] pDia = new Int32[Str_In.Count];

                 for (int i = 0; i < Str_In.Count; i++)
                 {
                     pDia[i] = Convert.ToInt32(Str_In[i]);
                 }

                 for (int i = 0; i < Str_In.Count - 1; i++)
                 {
                     for (int j = 0; j < Str_In.Count - (i + 1); j++)
                     {
                         if (pDia[j] > pDia[j + 1])
                         {
                             int pMax = pDia[j];
                             pDia[j] = pDia[j + 1];
                             pDia[j + 1] = pMax;
                         }

                     }
                 }
                 int pLength = Str_In.Count;
                 Str_In.Clear();
                 for (int j = 0; j < pLength; j++)
                 {
                     Str_In.Add(pDia[j].ToString());
                 }

             }


             public static void SortNumberwoHash(ref StringCollection Str_In, bool Bln_In)
             //============================================================================
             {
                 //....Bln_In = TRUE: Change the string members to fractional format e.g. 5/16, 3/8 and the like.


                 Double[] pVal = new Double[Str_In.Count];

                 for (int i = 0; i < Str_In.Count; i++)
                 {
                     pVal[i] = Convert.ToDouble(Str_In[i]);
                 }

                 for (int i = 0; i < Str_In.Count - 1; i++)
                 {
                     for (int j = 0; j < Str_In.Count - (i + 1); j++)
                     {
                         if (pVal[j] > pVal[j + 1])
                         {
                             Double pMax = pVal[j];
                             pVal[j] = pVal[j + 1];
                             pVal[j + 1] = pMax;
                         }
                     }
                 }

                 int pLength = Str_In.Count;
                 Str_In.Clear();
                 for (int j = 0; j < pLength; j++)
                 {
                     Str_In.Add(pVal[j].ToString());
                 }

                 
                 if (Bln_In)
                     ConvertDoubleToChar(ref Str_In);
             }


             public static void Kill_ProcessesAny_Exe()
             //=======================================     
             {
                 Process[] pProcesses = Process.GetProcesses();

                 try
                 {
                     foreach (Process p in pProcesses)
                         if (p.ProcessName == "Excel")
                             p.Kill();
                 }
                 catch (Exception pEXP)
                 {
                     MessageBox.Show(pEXP.Message);
                 }
             }

        #endregion


        #region "CONVERSION ROUTINES:"
        //============================

            public static Single ConvTextToSingle(string strTxtVal_In)
             //========================================================         
            {
                Single sngExactVal = 0.00F;
                
                if (NumericDataValidation(strTxtVal_In))    
                {
                    if (strTxtVal_In != "" && strTxtVal_In != ".")
                        sngExactVal = (Single)Convert.ToDouble(strTxtVal_In);
                    else if (strTxtVal_In == "" || strTxtVal_In == ".")
                        sngExactVal = 0.00F;

                }

                return sngExactVal;
            }

            public static Double ConvTextToDouble(string strTxtVal_In)
            //========================================================          
            {
                Double dblExactVal = 0.00F;

                if (NumericDataValidation(strTxtVal_In))
                {
                    if (strTxtVal_In != "" && strTxtVal_In != ".")
                    {
                        if (strTxtVal_In == "-")        //BG 28NOV12
                            dblExactVal = 0.00F;
                        else
                            dblExactVal = Convert.ToDouble(strTxtVal_In);
                    }
                    else if (strTxtVal_In == "" || strTxtVal_In == ".")
                        dblExactVal = 0.00F;

                }

                return dblExactVal;
            }


            public static void ConvertDoubleToChar(ref StringCollection Str_In)
            //=================================================================    
            {
                String[] pDia = new String[Str_In.Count];

                for (int i = 0; i < Str_In.Count; i++)
                {
                    Double pAny = (Convert.ToDouble(Str_In[i]) * 32.0F);
                    pDia[i] = pAny.ToString();

                    if (pDia[i] != "32")
                    {
                        if (Convert.ToDouble(pDia[i]) % 16 == 0)
                        {
                            pDia[i] = ((Convert.ToDouble(Str_In[i]) * 32) / 16).ToString();
                            pDia[i] = pDia[i] + "/2";
                        }

                        else if (Convert.ToDouble(pDia[i]) % 8 == 0)
                        {
                            pDia[i] = ((Convert.ToDouble(Str_In[i]) * 32) / 8).ToString();
                            pDia[i] = pDia[i] + "/4";
                        }
                        else if (Convert.ToDouble(pDia[i]) % 4 == 0)
                        {
                            pDia[i] = ((Convert.ToDouble(Str_In[i]) * 32) / 4).ToString();
                            pDia[i] = pDia[i] + "/8";
                        }

                        else if (Convert.ToDouble(pDia[i]) % 2 == 0)
                        {
                            pDia[i] = ((Convert.ToDouble(Str_In[i]) * 32) / 2).ToString();
                            pDia[i] = pDia[i] + "/16";
                        }
                        else
                            pDia[i] = pDia[i] + "/32";
                    }
                    else
                        pDia[i] = "1";

                }

                int pLength = Str_In.Count;
                Str_In.Clear();
                for (int j = 0; j < pLength; j++)
                {
                    Str_In.Add(pDia[j].ToString());
                }

            }


            public static int ConvTextToInt(string strTxtVal_In)
            //==================================================
            {
                int intExactVal = 0;

                if (NumericDataValidation(strTxtVal_In))    
                {
                    if (strTxtVal_In != "" && strTxtVal_In != ".")
                        intExactVal = Convert.ToInt32(strTxtVal_In);
                    else if (strTxtVal_In == "" || strTxtVal_In == ".")
                        intExactVal = 0;
                }

                return intExactVal;
            }


            public static Boolean ConvTextToBoolean(string strTxtVal_In)
            //==========================================================
            {
                Boolean blnExactVal;

                if (strTxtVal_In == "Y")
                    blnExactVal = true;
                else
                    blnExactVal = false;

                return blnExactVal;
            }


            public static string ConvSingleToStr(Single Sng_In, String Format_In)
            //===================================================================
            {
                string pRetVal = null;
                if (Format_In != "")
                {
                    if (Sng_In != 0.0) pRetVal = Sng_In.ToString(Format_In);
                    else if (Sng_In <= 0.0) pRetVal = "";
                }
                else if (Format_In == "")
                {
                    if (Sng_In != 0.0) pRetVal = Sng_In.ToString();
                    else if (Sng_In <= 0.0) pRetVal = "";
                }

                return pRetVal;
            }

            
            public static string ConvDoubleToStr(Double Dbl_In, String Format_In)
            //===================================================================
            {
                string pRetVal = null;
                if (Format_In != "")
                {
                    if (Dbl_In != 0.0) pRetVal = Dbl_In.ToString(Format_In);
                    else if (Dbl_In <= 0.0) pRetVal = "";
                }
                else if (Format_In == "")
                {
                    if (Dbl_In != 0.0) pRetVal = Dbl_In.ToString();
                    else if (Dbl_In <= 0.0) pRetVal = "";
                }

                return pRetVal;
            }


            public static string ConvIntToStr(int Int_In)
            //===========================================
            {
                string pRetVal = null;

                if (Int_In != 0) pRetVal = Int_In.ToString();
                else if (Int_In <= 0) pRetVal = "";

                return pRetVal;
            }


            public static Boolean NumericDataValidation(string Value_In)
            //==========================================================
            {
                string pMsg;
                bool val1 = true;
                bool val2 = true;

                if (Value_In != "" && Value_In != "." && Value_In != null)
                {
                    val1 = System.Text.RegularExpressions.Regex.IsMatch(Value_In, @"^-?\d+([\.]{1}\d*)?$");
                    val2 = System.Text.RegularExpressions.Regex.IsMatch(Value_In, @"^(.{1}\d*)?$");

                    if (!(val1 || val2)) 
                    {
                        pMsg = "Input value is not in a correct format.";

                        MessageBox.Show(pMsg, "Invalid Input.", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                    }
                }

                return (val1 || val2);
            }

        #endregion


        #region "STRING EXTRACTION ROUTINES:"
        //===================================

            public static string ExtractPreData(string anyString, string searchString)
            //========================================================================
            {
                string anyStr = "";

                int iPos = 0;
                iPos = (anyString.IndexOf(searchString, 0) + 1);

                if (iPos > 0)
                    anyStr = anyString.Substring(0, iPos - 1).Trim(' ');

                return anyStr;
            }


            public static string ExtractPostData(string anyString, string searchString)
            //========================================================================
            {
                string anyStr = "";

                int iPos = 0;
                iPos = (anyString.IndexOf(searchString, 0) + 1);

                if (iPos > 0)
                    //anyStr = anyString.Substring(iPos + 1).Trim(' ');
                    anyStr = anyString.Substring(iPos).Trim(' ');
                return anyStr;
            }


            public static string ExtractMidData(string anyString, string searchStringStart,
                                                                  string searchStringEnd)
            //===========================================================================
            {
                string anyStr = "";
                int iPosS = 0, iPosE = 0;

                iPosS = (anyString.IndexOf(searchStringStart, 0) + 1);

                if (iPosS > 0)
                {
                    iPosE = anyString.IndexOf(searchStringEnd, iPosS);
                    anyStr = anyString.Substring(iPosS, iPosE - iPosS);
                }

                return anyStr;
            }

        #endregion


        #region "COMPARE ROUTINES: "
        //==========================

            public static int CompareVar(Single VarOrg_In, Single VarMod_In,
                                         int DecimalPlace_In, int CheckChange_Out)
            //==================================================================== 
            {

                VarOrg_In = (Single)Math.Round((double)VarOrg_In,DecimalPlace_In);
                VarMod_In = (Single)Math.Round((double)VarMod_In, DecimalPlace_In);

                if (System.Math.Abs(VarOrg_In - VarMod_In) >= modMain.gcEPS)
                    CheckChange_Out = CheckChange_Out + 1;
                return CheckChange_Out;
            }

            public static int CompareVar(Double VarOrg_In, Double VarMod_In,
                                            int DecimalPlace_In, int CheckChange_Out)
            //=======================================================================          
            {

                VarOrg_In = (Double)Math.Round((double)VarOrg_In, DecimalPlace_In);
                VarMod_In = (Double)Math.Round((double)VarMod_In, DecimalPlace_In);

                if (System.Math.Abs(VarOrg_In - VarMod_In) >= modMain.gcEPS)
                    CheckChange_Out = CheckChange_Out + 1;
                return CheckChange_Out;
            }


            public static int CompareVar(Int64 VarOrg_In, Int64 VarMod_In,
                                         int CheckChange_Out)
            //=========================================================== 
            {
                if (System.Math.Abs(VarOrg_In - VarMod_In) >= modMain.gcEPS)
                    CheckChange_Out = CheckChange_Out + 1;
                return CheckChange_Out;
            }

            public static int CompareVar(String VarOrg_In, String VarMod_In,
                                         int CheckChange_Out)
            //=========================================================== 
            {
                if (VarOrg_In != VarMod_In)
                    CheckChange_Out = CheckChange_Out + 1;
                return CheckChange_Out;
            }

            public static int CompareVar(Boolean VarOrg_In, Boolean VarMod_In,
                                         int CheckChange_Out)
            //=========================================================== 
            {
                if (VarOrg_In != VarMod_In)
                    CheckChange_Out = CheckChange_Out + 1;
                return CheckChange_Out;
            }

        #endregion


        #region" PRINT FORM: "  
        //=====================
            
            public static void printDocument1_PrintPage(System.Object sender,
                                                  System.Drawing.Printing.PrintPageEventArgs e)
            //==================================================================================        
            {
                e.Graphics.DrawImage(mMemoryImage, 0, 0);
            }

            public static void CaptureScreen(Form Form_In)
            //=============================================      //BG 20OCT11
            {
                Graphics myGraphics = Form_In.CreateGraphics();
                Size s = Form_In.Size;
                mMemoryImage = new Bitmap(s.Width, s.Height, myGraphics);
                Graphics memoryGraphics = Graphics.FromImage(mMemoryImage);
                memoryGraphics.CopyFromScreen(
                    Form_In.Location.X, Form_In.Location.Y, 0, 0, s);
            }

        #endregion
    }
}
