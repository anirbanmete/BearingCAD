//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsDesignTables                        '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  12JUN12                                '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 01MAY12. Instruction.

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
        //PB 09AUG11. Check for duplication in clsFiles. 


        //....SW Files:                 //PB 30APR12. To be modified later.
        //....Project Dependent.

        //........Model Files:
        private const string mcSWRadial_Title = "RADIAL BEARING.SLDPRT";
        private const string mcSWSeal_Title = "SEAL.SLDPRT";
        private const string mcSWBlankAssy_Title = "BLANK.SLDASM";
        private const string mcSWCompleteAssy_Title = "COMPLETE ASSY.SLDASM";
        private const string mcSWTL_TB_Title = "TL THRUST BEARING.SLDPRT";
        private const string mcSWTBAssy_Title = "THRUST BEARING ASSY.SLDASM";

        //........Drawing Files:
        private const string mcSWDwg01_Title = "-01.SLDDRW";
        private const string mcSWDwg02_Title = "-02.SLDDRW";
        private const string mcSWDwg03_Title = "-03.SLDDRW";
        private const string mcSWDwg04_Title = "-04.SLDDRW";
        private const string mcSWDwg05_Title = "-05.SLDDRW";
        private const string mcSWDwg06_Title = "-06.SLDDRW";
        private const string mcSWDwg07_Title = "-07.SLDDRW";


        //....Project Independent.      //PB 30APR12. Not required any more.
        //........Model Files:
        //private const string mcSW_Etch_Reqd_Engg_Std_Title = "ETCH REQ-ENG STD.SLDPRT";
        //private const string mcSWPin_Title = "PINS.SLDPRT";
        //private const string mcSWScrew_Title = "SCREWS.SLDPRT";


        #region "MEMBER VARIABLE DECLARATION"
        //***********************************       
   
            //....Missing object.
            private object mobjMissing = Missing.Value;

        #endregion


        #region "CLASS METHODS:"
           
            public void Populate_EXCEL_DesignTables(clsFiles Files_In, clsProject Project_In,
                                                    clsOpCond OpCond_In,  clsDB DB_In, string FilePath_In)
            //============================================================================================ 
            {
                    CloseExcelFiles();

                //  RADIAL: 
                //  -------  
                    Populate_tblMapping_Radial(Project_In, OpCond_In, DB_In);           //....Software variable value field.
                    Populate_EXCEL_Radial(Files_In, Project_In, DB_In, FilePath_In);    //....Design Table.

                //  BLANK Assy:   
                //  -----------
                    Populate_tblMapping_Blank(Project_In, DB_In);
                    Populate_EXCEL_BlankAssy(Files_In, Project_In, DB_In, FilePath_In);
      
                //  SEAL:
                //  -----
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        Populate_tblMapping_Seal(Project_In, DB_In);
                        Populate_EXCEL_Seal(Files_In, Project_In, DB_In, FilePath_In);
                    }
                             
                //  TL THRUST BEARING:   
                //  ------------------
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        Populate_tblMapping_TL_TB(Project_In, DB_In);
                        Populate_EXCEL_TL_TB(Files_In, Project_In, DB_In, FilePath_In);
                    }

                //  THRUST BEARING ASSY:    
                //  --------------------
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        Populate_tblMapping_TB_Assy(Project_In, DB_In);
                        Populate_EXCEL_TB_Assy(Files_In, Project_In, DB_In, FilePath_In);
                    }

                //  COMPLETE ASSY:   
                //  --------------
                    Populate_tblMapping_Complete(Project_In, OpCond_In, DB_In);
                    Populate_EXCEL_CompleteAssy(Files_In, Project_In, DB_In, FilePath_In);  


                //  Solid Works Files.    
                //  ------------------   
                    CreateSWFiles(Project_In, Files_In, FilePath_In);
            }


            #region "CREATE SW FILES"

                private void CreateSWFiles(clsProject Project_In, clsFiles Files_In, String FilePath_In)
                //=====================================================================================
                {
                    try
                    {

                        //  Project Dependent Files.
                        //  ------------------------

                            //....Model Files.
                            //
                                //....Radial:
                                    string pSWRadialFileName = FilePath_In + "\\" + mcSWRadial_Title;
                                    if (File.Exists(pSWRadialFileName))
                                        File.Delete(pSWRadialFileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Radial, pSWRadialFileName);


                                //....Seal: .
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal
                                        || Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                    {
                                        string pSWSealFileName = FilePath_In + "\\" + mcSWSeal_Title;
                                        if (File.Exists(pSWSealFileName))
                                            File.Delete(pSWSealFileName);
                                        File.Copy(Files_In.FileTitle_Template_SW_Seal, pSWSealFileName);
                                    }


                                //....Blank Assembly:
                                    string pSWBlankAssy = FilePath_In + "\\" + mcSWBlankAssy_Title;
                                    if (File.Exists(pSWBlankAssy))
                                        File.Delete(pSWBlankAssy);
                                    File.Copy(Files_In.FileTitle_Template_SW_BlankAssy, pSWBlankAssy);


                                //....Complete Assembly: 
                                    string pSWCompleteAssy = FilePath_In + "\\" + mcSWCompleteAssy_Title;
                                    if (File.Exists(pSWCompleteAssy))
                                        File.Delete(pSWCompleteAssy);
                                    File.Copy(Files_In.FileTitle_Template_SW_CompleteAssy, pSWCompleteAssy);

                                //....Thrust Bearing: 
                                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL
                                        || Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                    {
                                        string pSWTL_TB = FilePath_In + "\\" + mcSWTL_TB_Title;
                                        string pSWTB_Assy = FilePath_In + "\\" + mcSWTBAssy_Title;

                                        if (File.Exists(pSWTL_TB))
                                            File.Delete(pSWTL_TB);
                                        File.Copy(Files_In.FileTitle_Template_SW_TL_TB, pSWTL_TB);

                                        if (File.Exists(pSWTB_Assy))
                                            File.Delete(pSWTB_Assy);
                                        File.Copy(Files_In.FileTitle_Template_SW_TBAssy, pSWTB_Assy);
                                    }


                            //....Drawing Files.
                            //  ----------------

                                //....-01: 
                                    string pSWDwg01FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg01_Title;
                                    if (File.Exists(pSWDwg01FileName))
                                        File.Delete(pSWDwg01FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg01_Title, pSWDwg01FileName);


                                //....-02: 
                                    string pSWDwg02FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg02_Title;
                                    if (File.Exists(pSWDwg02FileName))
                                        File.Delete(pSWDwg02FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg02_Title, pSWDwg02FileName);


                                //....-03: 
                                    string pSWDwg03FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg03_Title;
                                    if (File.Exists(pSWDwg03FileName))
                                        File.Delete(pSWDwg03FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg03_Title, pSWDwg03FileName);


                                //....-04: 
                                    string pSWDwg04FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg04_Title;
                                    if (File.Exists(pSWDwg04FileName))
                                        File.Delete(pSWDwg04FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg04_Title, pSWDwg04FileName);


                                //....-05: 
                                    string pSWDwg05FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg05_Title;
                                    if (File.Exists(pSWDwg05FileName))
                                        File.Delete(pSWDwg05FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg05_Title, pSWDwg05FileName);


                                //....-06: 
                                    string pSWDwg06FileName = FilePath_In + "\\" + Project_In.AssyDwg.No + mcSWDwg06_Title;
                                    if (File.Exists(pSWDwg06FileName))
                                        File.Delete(pSWDwg06FileName);
                                    File.Copy(Files_In.FileTitle_Template_SW_Dwg06_Title, pSWDwg06FileName);


                                ////....-07: 
                                //    string pSWDwg07FileName = FileName_In + "\\" + Project_In.AssyDwg.No + mcSWDwg07_Title;
                                //    if (File.Exists(pSWDwg07FileName))
                                //        File.Delete(pSWDwg07FileName);
                                //    File.Copy(Files_In.FileTitle_Template_SW_Dwg07_Title, pSWDwg07FileName);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Unable to copy SW File.Please close SW Files.");
                    }
                }

            #endregion


            #region "UTILITY ROUTINES:"

                private string MatAbbr(string Mat_In)
                //====================================  
                {
                    string pMat = null;

                    if (Mat_In.ToUpper().Contains("STEEL"))
                        pMat = "STL";
                    else if (Mat_In.ToUpper().Contains("BABBITT"))
                        pMat = "BABB";
                    else if (Mat_In.ToUpper().Contains("BRONZE"))
                        pMat = "BRNZ";
                    else
                        pMat = Mat_In;

                    return pMat;
                }

                private int ColumnNumber(string ColumnName_In)
                //============================================       
                {
                    int pIndx = 0;

                    if (ColumnName_In.Length == 1)
                    {
                        for (int i = 0; i < 26; i++)
                        {
                            char[] pAny = ColumnName_In.ToCharArray();
                            if (pAny[0] == Convert.ToChar(65+i))
                            {
                                pIndx = (i+1);
                                break;
                            }
                        }
                    }
                    else if (ColumnName_In.Length == 2)
                    {

                        char[] pAny = ColumnName_In.ToCharArray();

                        for (int i = 0; i < 26; i++)
                        {
                            if (Convert.ToChar(65+i) == pAny[0])
                            {
                                pIndx = (i+1) * 26;
                                break;
                            }
                        }

                        for (int i = 0; i < 26; i++)
                        {
                            if (Convert.ToChar(65+i) == pAny[1])
                            {
                                pIndx = pIndx + i + 1;
                                break;
                            }
                        }
                    }

                    return pIndx;
                }

            #endregion


            //public void CloseExcelFiles()                 //PB 01MAY12
            private void CloseExcelFiles() 
            //=============================       
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
    }
}
