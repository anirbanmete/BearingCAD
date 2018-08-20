
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsFiles                               '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  11AUG14                                '
//                                                                              '
//===============================================================================


//    FILE NAMING CONVENTIONS:
//    -----------------------
//    ....FileName  ==>  Path, File, Extn
//    ....FileTitle ==>        File, Extn
//    ....File      ==>        File

//    *******************************************************************************
//    *          CLASS FOR  FILE MANIPULATION - READ & WRITE AND DELETE.            *
//    *******************************************************************************

using System;
using System.Globalization;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Xml;



namespace BearingCAD20
{
    public class clsFiles
    {
        #region "FILE DEFINITIONS"
            //=====================

            //File Directories & Names:
            //=========================
            private const string mcDriveRoot_Client = "C:";                                       

            //  Installation:
            //  -------------
            //
            //....Root Directory of Client Machine:  
            private const string mcDirRoot = "\\BearingCAD\\";
                   
            //....Config File Name of Client Machine.               
            private const string mcConfigFile_Client ="BearingCAD20_Client.config";

            //....Config File Name of Client Machine.
            private const string mcConfigFile_Server = "BearingCAD20_Server.config";
                    
            //....LogoFile.     
            private const string mcLogo_Title = "Waukesha Logo.bmp"; //"KMC Logo.jpg";

            //....Design Tables.
            
            //....Project Files.
            private const string mcSummaryInfo_Title = "Summary Information Sheet.dot";

            //....SW Files.

                //....Project Dependent Files.

                //....Project Invariant Files.
                private const string mcFileTitle_Template_SW_Etch_Reqd_Engg_Std = "ETCH REQ-ENG STD.SLDPRT";
                private const string mcFileTitle_Template_SW_Pin = "PINS.SLDPRT";
                private const string mcFileTitle_Template_SW_Screw = "SCREWS.SLDPRT";

                //....Project Dependent Files.
                //private const string mcFileTitle_Template_SW_Dwg01_Title = "XXXX-01.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg02_Title = "XXXX-02.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg03_Title = "XXXX-03.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg04_Title = "XXXX-04.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg05_Title = "XXXX-05.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg06_Title = "XXXX-06.SLDDRW";
                //private const string mcFileTitle_Template_SW_Dwg07_Title = "XXXX-07.SLDDRW";


                //BG 28DEC12
                //........Drawing Files:
                private const string mcFileTitle_SW_Dwg_Radial = "RADIAL-02.SLDDRW";
                private const string mcFileTitle_SW_Dwg_Seal = "SEAL.SLDDRW";
                private const string mcFileTitle_SW_Dwg_CompleteAssy = "COMPLETE-01.SLDDRW";
                private const string mcFileTitle_SW_Dwg_TL_TB = "TB.SLDDRW";
                private const string mcFileTitle_SW_Dwg_TL_TB_Blank = "TB_BLANK.SLDDRW";
                private const string mcFileTitle_SW_Dwg_Roughing = "ROUGHING-05.SLDDRW";
  
        #endregion


        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            //....DriveRoot
            private string mDriveRoot;

            //....DB FileName and Type
            private static string mDBFileName, mDBServerName;                   

            //SB 24JUL09 Member variables declared.
            //....Project Files.
                
                //....Template Files.
                    private string mFilePath_Template_WORD;

            private string mFileTitle_Template_SIS;         //....Not Used Now.
            //private string mFileTitle_Template_DIS;  
            private string mFileTitle_Template_DDR;         //AES 11AUG14 DIS => DDR

            //....Design Tables.

                //....Directory of Design Table Template.
                    private string mFilePath_Template_EXCEL;

            private string mFileTitle_Template_EXCEL_Radial;        
            private string mFileTitle_Template_EXCEL_Seal;
            private string mFileTitle_Template_EXCEL_BlankAssy;
            private string mFileTitle_Template_EXCEL_CompleteAssy;
            private string mFileTitle_Template_EXCEL_TL_TB;
            private string mFileTitle_Template_EXCEL_TB_Assy;


            //....SW Files.
                //....Directory of Design Table Template.
                    private string mFilePath_Template_SW;

            //....Project Dependent Files.
            private string mFileTitle_Template_SW_Radial;
            private string mFileTitle_Template_SW_Seal;
            private string mFileTitle_Template_SW_BlankAssy;
            private string mFileTitle_Template_SW_CompleteAssy;
            private string mFileTitle_Template_SW_TL_TB;
            private string mFileTitle_Template_SW_TBAssy;          


         #endregion


        #region "CLASS PROPERTY ROUTINES"
            //===========================

            //READ-ONLY PROPERTIES:
            //=====================

            public string Logo
            //=================
            {
                get
                { 
                    return mcDriveRoot_Client + mcDirRoot + "Images\\" + mcLogo_Title; 
                }   
            }

            public static string DBFileName
            //==============================
            {
                get { return mDBFileName; }
            }


            public static string DBServerName
            //===============================
            {
                get { return mDBServerName; }
            }

            //SB 24JUL09 Rename Property and change return values.

            //  Project Files.
            //  --------------

                public string FileTitle_Template_SIS
                //==================================  
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_WORD + "\\" + mFileTitle_Template_SIS;
                    }
                }

                public string FileTitle_Template_DDR
                //==================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_WORD + "\\" + mFileTitle_Template_DDR; 
                    }
                }

            //  Design Tables.
            //  --------------

                public string FileTitle_Template_EXCEL_Radial
                //===========================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_Radial;
                    }
                }

                public string FileTitle_Template_EXCEL_Seal
                //=========================================     
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_Seal;
                    }
                }

                public string FileTitle_Template_EXCEL_BlankAssy
                //==============================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_BlankAssy; 
                    }
                }

                public string FileTitle_Template_EXCEL_CompleteAssy
                //=================================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_CompleteAssy; 
                    }    
                }

                public string FileTitle_Template_EXCEL_TL_TB
                //=================================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_TL_TB;
                    }
                }

                public string FileTitle_Template_EXCEL_TB_Assy
                //=================================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_TB_Assy;
                    }
                }



            //  SW Files.
            //  ---------

                public string FileTitle_Template_SW_Radial                 
                //=========================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_Radial; 
                    }
                }

                public string FileTitle_Template_SW_Seal                   
                //======================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_Seal; 
                    }
                }

                public string FileTitle_Template_SW_BlankAssy              
                //===========================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_BlankAssy; 
                    }
                }

                public string FileTitle_Template_SW_CompleteAssy           
                //==============================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_CompleteAssy; 
                    }
                }

                public string FileTitle_Template_SW_TL_TB
                //=========================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_TL_TB;
                    }
                }

                public string FileTitle_Template_SW_TBAssy
                //=========================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_TBAssy;
                    }
                }

            

                public string FileTitle_Template_SW_Etch_Reqd_Engg_Std    
                //====================================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Etch_Reqd_Engg_Std; 
                    }
                }

                public string FileTitle_Template_SW_Pin                    
                //=====================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Pin; 
                    }
                }

                public string FileTitle_Template_SW_Screw                  
                //========================================
                {
                    get
                    { 
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Screw; 
                    }
                }


                //BG 31DEC12
                public string FileTitle_Template_SW_Dwg_Radial
                //=============================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Radial;
                    }
                }

               
                public string FileTitle_Template_SW_Dwg_Seal
                //==========================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Seal;
                    }
                }

                public string FileTitle_Template_SW_Dwg_CompleteAssy
                //========================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_CompleteAssy;
                    }
                }

                public string FileTitle_Template_SW_Dwg_TL_TB
                //=================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_TL_TB;
                    }
                }

                public string FileTitle_Template_SW_Dwg_TL_TB_Blank
                //=================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_TL_TB_Blank;
                    }
                }

                public string FileTitle_Template_SW_Dwg_Roughing
                //==============================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Roughing;
                    }
                }


                //BG 31DEC12
                //public string FileTitle_Template_SW_Dwg01_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg01_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg02_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg02_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg03_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg03_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg04_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg04_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg05_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg05_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg06_Title                  
                ////=============================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg06_Title; 
                //    }
                //}

                //public string FileTitle_Template_SW_Dwg07_Title                  
                ////=============================================
                //{

                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Dwg07_Title; 
                //    }
                //}

        
        #endregion

        public clsFiles()
        //===============
        {
            //....Reads Configuration File.
            ReadConfigFile();
        }

        #region "CLASS METHODS"

            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - BEGIN                             '
            //---------------------------------------------------------------------------                 

            private void ReadConfigFile()
            //==========================
            {

                try      
                {
                    //  READ CLIENT CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Client = mcDriveRoot_Client + mcDirRoot + mcConfigFile_Client;

                        FileStream pSW = new FileStream(pConfigFileName_Client, FileMode.Open,
                                                        FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            System.Xml.XmlDocument pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            XmlNode pRoot;
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                        //....Child Node.
                            XmlNode pRootChild = pRoot.FirstChild;

                        //.....Get Installation Directory Of Server Configuration.
                            mDriveRoot = pRootChild.InnerText;
                            pXML = null;
                            pSW.Close();

                    //  READ SERVER CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Server = mDriveRoot + mcDirRoot + mcConfigFile_Server;

                        if (!File.Exists(pConfigFileName_Server))
                        {
                            MessageBox.Show("Please Specify Proper Root Installation Directory in Client configuration file.", "Error");
                            System.Environment.Exit(0);
                        }

                        pSW = new FileStream(pConfigFileName_Server, FileMode.Open,
                                                            FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                            foreach (XmlNode pRChild in pRoot.ChildNodes)       //SB 10JUL09
                            {
                                //.....Mapping Rules Implementation.
                                switch (pRChild.Name)
                                {
                                    case "SEREVERName":
                                        //-----------------
                                        mDBServerName = pRChild.InnerText;
                                        break;

                                    case "DataBaseName":
                                        //--------------
                                        mDBFileName = pRChild.InnerText;
                                        break;

                                    //SB 24JUL09 Other name are added.
                                    case "FilePath_Template_WORD":
                                        //------------------------
                                        mFilePath_Template_WORD = pRChild.InnerText;
                                        break;

                                        //AES 11AUG14 DIS => DDR
                                    case "FileTitle_Template_DDR":
                                        //------------------------
                                        mFileTitle_Template_DDR = pRChild.InnerText;
                                        break;

                                    case "FilePath_Template_EXCEL":
                                        //-------------------------
                                        mFilePath_Template_EXCEL = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_Radial":
                                        //---------------------------------
                                        mFileTitle_Template_EXCEL_Radial = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_Seal":
                                        //-------------------------------
                                        mFileTitle_Template_EXCEL_Seal = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_BlankAssy":
                                        //------------------------------------
                                        mFileTitle_Template_EXCEL_BlankAssy = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_CompleteAssy":
                                        //---------------------------------------
                                        mFileTitle_Template_EXCEL_CompleteAssy = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_TLThrustBearing":
                                        //---------------------------------------
                                        mFileTitle_Template_EXCEL_TL_TB = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_ThrustBearingAssy":
                                        //---------------------------------------
                                        mFileTitle_Template_EXCEL_TB_Assy = pRChild.InnerText;
                                        break;

                                    case "FilePath_Template_SW":
                                        //----------------------
                                        mFilePath_Template_SW = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_Radial":
                                        //------------------------------
                                        mFileTitle_Template_SW_Radial = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_Seal":
                                        //----------------------------
                                        mFileTitle_Template_SW_Seal = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_BlankAssy":
                                        //---------------------------------
                                        mFileTitle_Template_SW_BlankAssy = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_CompleteAssy":
                                        //------------------------------------
                                        mFileTitle_Template_SW_CompleteAssy = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_TL_TB":
                                        //------------------------------------
                                        mFileTitle_Template_SW_TL_TB = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_SW_TBAssy":
                                        //------------------------------------
                                        mFileTitle_Template_SW_TBAssy = pRChild.InnerText;
                                        break;

                                }
                            }
                            pXML = null;
                            pSW.Close();

                }

                catch (FileNotFoundException pEXP)      //BG 13JUL09
                {
                    MessageBox.Show(pEXP.Message, "File Error");        
                }

            }
       
            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - END                              '
            //---------------------------------------------------------------------------
        #endregion
    }
        
}




