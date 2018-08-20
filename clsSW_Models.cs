//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSW_Models                           '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  05APR13                                '
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
        #region "NAMED CONSTANTS:"
        //------------------------

            //....SW Files:                 
 
            //........Model Files:
            private const string mcRadial_Title       = "RADIAL BEARING.SLDPRT";
            private const string mcSeal_Title         = "SEAL.SLDPRT";
            private const string mcBlankAssy_Title    = "BLANK.SLDASM";
            private const string mcCompleteAssy_Title = "COMPLETE ASSY.SLDASM";
            private const string mcTL_TB_Title        = "TL THRUST BEARING.SLDPRT";
            private const string mcTBAssy_Title       = "THRUST BEARING ASSY.SLDASM";
              

            //........Drawing Files:
            //private const string mcDwg01_Title_Suffix = "-01.SLDDRW";     //SG 23JAN13
            //private const string mcDwg02_Title_Suffix = "-02.SLDDRW";
            //private const string mcDwg03_Title_Suffix = "-03.SLDDRW";
            //private const string mcDwg04_Title_Suffix = "-04.SLDDRW";
            //private const string mcDwg05_Title_Suffix = "-05.SLDDRW";
            //private const string mcDwg06_Title_Suffix = "-06.SLDDRW";
            //private const string mcDwg07_Title_Suffix = "-07.SLDDRW";

        #endregion


        #region "MEMBER VARIABLE DECLARATION"
        //***********************************   
            private object mobjMissing = Missing.Value;              //....Missing object.
            private string[] mDwg_Title_Suffix = new string[8];      //....1-based

        #endregion



        #region "CLASS METHODS:"
        //----------------------

            public void PopulateAndMove_SWFiles_ProjectFolder(clsFiles Files_In, clsProject Project_In,
                                                              clsOpCond OpCond_In, clsDB DB_In, string FilePath_In)
            //====================================================================================================
            {
                //....SWFiles - Design tables, Model & Drawing files.
                //
                PopulateAndMove_DesignTables(Files_In, Project_In, OpCond_In, DB_In, FilePath_In);
                CopyAndMove_SW_Model_Dwg_Files(Project_In, Files_In, FilePath_In);
            }


            #region "DESIGN TABLES POPULATION:"

                private void PopulateAndMove_DesignTables(clsFiles Files_In, clsProject Project_In,
                                                            clsOpCond OpCond_In, clsDB DB_In, string FilePath_In)
                //=============================================================================================== 
                {
                    //....This routine copies the design table templates to the project design tables, populates them
                    //........and places them in the given project folder. 

                    Close_DesignTables();

                    //  RADIAL: 
                    //  -------  
                    Populate_tblMapping_Radial(Project_In, OpCond_In, DB_In);               //....Software variable value field.
                    PopulateAndMove_DT_Radial(Files_In, Project_In, DB_In, FilePath_In);    //....Design Table.

                    //  BLANK Assy:   
                    //  -----------
                    Populate_tblMapping_Blank(Project_In, DB_In);
                    PopulateAndMove_DT_BlankAssy(Files_In, Project_In, DB_In, FilePath_In);

                    //  SEAL:
                    //  -----
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        Populate_tblMapping_Seal(Project_In, DB_In);
                        PopulateAndMove_DT_Seal(Files_In, Project_In, DB_In, FilePath_In);
                    }

                    //  TL THRUST BEARING:   
                    //  ------------------
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        Populate_tblMapping_TL_TB(Project_In, DB_In);
                        PopulateAndMove_DT_TL_TB(Files_In, Project_In, DB_In, FilePath_In);
                    }

                    //  THRUST BEARING ASSY:    
                    //  --------------------
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        Populate_tblMapping_TB_Assy(Project_In, DB_In);
                        PopulateAndMove_DT_TB_Assy(Files_In, Project_In, DB_In, FilePath_In);
                    }

                    //  COMPLETE ASSY:   
                    //  --------------
                    Populate_tblMapping_Complete(Project_In, OpCond_In, DB_In);
                    PopulateAndMove_DT_CompleteAssy(Files_In, Project_In, DB_In, FilePath_In);
                }


                #region "HELPER ROUTINES:"

                    private void Close_DesignTables()
                    //===============================       
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


        #region "COPY & MOVE SW Model & Dwg FILES:"

            private void CopyAndMove_SW_Model_Dwg_Files(clsProject Project_In, clsFiles Files_In, String FilePath_In)
            //========================================================================================================
            {
                try
                {
                    //  MODEL FILES.
                    //  -----------
                    //
                    //....Complete Assy: 
                    //
                    string pFileName_CompleteAssy = FilePath_In + "\\" + mcCompleteAssy_Title;

                    if (File.Exists(pFileName_CompleteAssy))
                        File.Delete(pFileName_CompleteAssy);

                    File.Copy(Files_In.FileTitle_Template_SW_CompleteAssy, pFileName_CompleteAssy);


                    //....Blank Assy:
                    //
                    string pFileName_BlankAssy = FilePath_In + "\\" + mcBlankAssy_Title;

                    if (File.Exists(pFileName_BlankAssy))
                        File.Delete(pFileName_BlankAssy);

                    File.Copy(Files_In.FileTitle_Template_SW_BlankAssy, pFileName_BlankAssy);


                    //....Radial:
                    //
                    string pFileName_Radial = FilePath_In + "\\" + mcRadial_Title;

                    if (File.Exists(pFileName_Radial))
                        File.Delete(pFileName_Radial);
                    File.Copy(Files_In.FileTitle_Template_SW_Radial, pFileName_Radial);


                    //....Seal:
                    //
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Seal ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        string pFileName_Seal = FilePath_In + "\\" + mcSeal_Title;
                        if (File.Exists(pFileName_Seal))
                            File.Delete(pFileName_Seal);
                        File.Copy(Files_In.FileTitle_Template_SW_Seal, pFileName_Seal);
                    }


                    //....Thrust Bearing: 
                    //
                    if (Project_In.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||
                        Project_In.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        string pFileName_TL_TB = FilePath_In + "\\" + mcTL_TB_Title;

                        if (File.Exists(pFileName_TL_TB))
                            File.Delete(pFileName_TL_TB);

                        File.Copy(Files_In.FileTitle_Template_SW_TL_TB, pFileName_TL_TB);


                        string pFileName_TB_Assy = FilePath_In + "\\" + mcTBAssy_Title;

                        if (File.Exists(pFileName_TB_Assy))
                            File.Delete(pFileName_TB_Assy);

                        File.Copy(Files_In.FileTitle_Template_SW_TBAssy, pFileName_TB_Assy);
                    }


                    //  DRAWING FILES.
                    //  -------------  

                    Populate_Dwg_Title_Suffix_Array(Project_In);

                    //....Complete Assy.
                    //
                    string pFileName_Dwg_CompleteAssy = FilePath_In + "\\" + Project_In.AssyDwg.No + mDwg_Title_Suffix[1];

                    if (File.Exists(pFileName_Dwg_CompleteAssy))
                        File.Delete(pFileName_Dwg_CompleteAssy);

                    File.Copy(Files_In.FileTitle_Template_SW_CompleteAssy, pFileName_Dwg_CompleteAssy);


                    //....Radial.
                    //
                    string pFileName_Dwg_Radial = FilePath_In + "\\" + Project_In.AssyDwg.No + mDwg_Title_Suffix[2];

                    if (File.Exists(pFileName_Dwg_Radial))
                        File.Delete(pFileName_Dwg_Radial);

                    File.Copy(Files_In.FileTitle_Template_SW_Dwg_Radial, pFileName_Dwg_Radial);


                    //....Roughing.
                    //
                    string pFileName_Dwg_Roughing = FilePath_In + "\\" + Project_In.AssyDwg.No  + mDwg_Title_Suffix[5];

                    if (File.Exists(pFileName_Dwg_Roughing))
                        File.Delete(pFileName_Dwg_Roughing);

                    File.Copy(Files_In.FileTitle_Template_SW_Dwg_Roughing, pFileName_Dwg_Roughing);


                    //....Seal(s):
                    //
                    //....FRONT.
                    //
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                    {
                        string pFileName_Dwg_Seal_Front = FilePath_In + "\\" + Project_In.AssyDwg.No + mDwg_Title_Suffix[3]; 

                        if (File.Exists(pFileName_Dwg_Seal_Front))
                            File.Delete(pFileName_Dwg_Seal_Front);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_Seal, pFileName_Dwg_Seal_Front);
                    }


                    //....BACK.
                    //
                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        string pFileName_Dwg_Seal_Back = FilePath_In + "\\" + Project_In.AssyDwg.No  + mDwg_Title_Suffix[4]; 

                        if (File.Exists(pFileName_Dwg_Seal_Back))
                            File.Delete(pFileName_Dwg_Seal_Back);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_Seal, pFileName_Dwg_Seal_Back);
                    }


                    //....Thrust Bearing(s):
                    //
                    //....FRONT.
                    //
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        string pFileName_Dwg_TB_Front = FilePath_In + "\\" + Project_In.AssyDwg.No  + mDwg_Title_Suffix[3]; 

                        if (File.Exists(pFileName_Dwg_TB_Front))
                            File.Delete(pFileName_Dwg_TB_Front);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_TL_TB, pFileName_Dwg_TB_Front);


                        //....Blank.
                        string pFileName_Dwg_TB_Blank_Front = FilePath_In + "\\" + Project_In.AssyDwg.No  + mDwg_Title_Suffix[6];

                        if (File.Exists(pFileName_Dwg_TB_Blank_Front))
                            File.Delete(pFileName_Dwg_TB_Blank_Front);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_TL_TB_Blank, pFileName_Dwg_TB_Blank_Front);
                    }


                    //....BACK.
                    //
                    if (modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        string pFileName_Dwg_TB_Back = FilePath_In + "\\" + Project_In.AssyDwg.No  + mDwg_Title_Suffix[4];

                        if (File.Exists(pFileName_Dwg_TB_Back))
                            File.Delete(pFileName_Dwg_TB_Back);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_TL_TB, pFileName_Dwg_TB_Back);


                        //....Blank.
                        //
                        string pDwg_Title_Suffix = "";

                        if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal)
                        {
                            pDwg_Title_Suffix = mDwg_Title_Suffix[6]; 
                        }
                        else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                        {
                            pDwg_Title_Suffix = mDwg_Title_Suffix[7]; 
                        }


                        string pFileName_Dwg_TB_Blank_Back = FilePath_In + "\\" + Project_In.AssyDwg.No  + pDwg_Title_Suffix;

                        if (File.Exists(pFileName_Dwg_TB_Blank_Back))
                            File.Delete(pFileName_Dwg_TB_Blank_Back);

                        File.Copy(Files_In.FileTitle_Template_SW_Dwg_TL_TB_Blank, pFileName_Dwg_TB_Blank_Back);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Unable to copy SW File.Please close SW Files.");
                }
            }


            #region "HELPER ROUTINE:"

                private void Populate_Dwg_Title_Suffix_Array (clsProject Project_In)
                //==================================================================         
                {
                    int pNo = Convert.ToInt32(Project_In.AssyDwg.No_Suffix);

                    string pPrefix = "";

                    for (int i = 1; i < mDwg_Title_Suffix.Length; i++)
                    {
                        if (pNo < 10)
                            pPrefix = "-0";
                        else
                            pPrefix = "-";

                        mDwg_Title_Suffix [i] = pPrefix + pNo + ".SLDDRW";

                        pNo++;
                    }
                }

            #endregion

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

    #endregion
    }
}
