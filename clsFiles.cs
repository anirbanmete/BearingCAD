
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsFiles                               '
//                        VERSION NO  :  2.1                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30JUL18                                '
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
using System.Linq;
using System.Configuration;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;



namespace BearingCAD21
{
    public class clsFiles
    {
        #region "FILE DEFINITIONS"
            //=====================
        private const int mcObjFile_Count = 2;

            //File Directories & Names:
            //=========================
            private const string mcDriveRoot_Client = "C:";                                       

            //  Installation:
            //  -------------
            //
            //....Root Directory of Client Machine:  
            private const string mcDirRoot = "\\BearingCAD\\";
                   
            //....Config File Name of Client Machine.               
            private const string mcConfigFile_Client ="BearingCAD21_Client.config";

            //....Config File Name of Client Machine.
            private const string mcConfigFile_Server = "BearingCAD21_Server.config";
                    
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

            private string mFileTitle_Template_EXCEL_Parameter_Complete = "ParameterV21_Complete_06AUG18.xltx";
            private string mFileTitle_Template_EXCEL_Parameter_Driver = "ParameterV21_Driver_25JUL18.xltx"; 


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

            //....Inventor Files.   //....AES 23JUL18
            //....Directory of Design Table Template.
            private string mFilePath_Template_Inventor;

            //....Project Dependent Files.
            private string mFileTitle_Template_Inventor_Radial;
            private string mFileTitle_Template_Inventor_Seal_Front;
            private string mFileTitle_Template_Inventor_Seal_Back;
            private string mFileTitle_Template_Inventor_Thrust_Front;
            private string mFileTitle_Template_Inventor_Thrust_Back;
            private string mFileTitle_Template_Inventor_Complete;
            //private string mFileTitle_Template_Inventor_Radial_Assy;

            private string mFileName_BearingCAD = "";//"D:\\BearingCAD\\Projects\\DataSet.BearingCAD";        //....Session file.

           
         #endregion


        #region "CLASS PROPERTY ROUTINES"
            //===========================

            public string FileName_BearingCAD
            {
                get { return mFileName_BearingCAD; }
                set { mFileName_BearingCAD = value; }
            }

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

                public string FileTitle_Template_EXCEL_Parameter_Complete
                //========================================================    
                {
                    get
                    {
                        return mDriveRoot + mcDirRoot + "\\Templates\\EXCEL\\V21\\" + mFileTitle_Template_EXCEL_Parameter_Complete;
                    }
                }

                public string FileTitle_Template_EXCEL_Parameter_Driver
                //=====================================================    
                {
                    get
                    {
                        return mDriveRoot + mcDirRoot + "\\Templates\\EXCEL\\V21\\" + mFileTitle_Template_EXCEL_Parameter_Driver;
                    }
                }



            //  SW Files.
            //  ---------

                ////public string FileTitle_Template_SW_Radial                 
                //////=========================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_Radial; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Seal                   
                //////======================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_Seal; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_BlankAssy              
                //////===========================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_BlankAssy; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_CompleteAssy           
                //////==============================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_CompleteAssy; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_TL_TB
                //////=========================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_TL_TB;
                ////    }
                ////}

                ////public string FileTitle_Template_SW_TBAssy
                //////=========================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mFileTitle_Template_SW_TBAssy;
                ////    }
                ////}

            

                ////public string FileTitle_Template_SW_Etch_Reqd_Engg_Std    
                //////====================================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Etch_Reqd_Engg_Std; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Pin                    
                //////=====================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Pin; 
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Screw                  
                //////========================================
                ////{
                ////    get
                ////    { 
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_Template_SW_Screw; 
                ////    }
                ////}


                //////BG 31DEC12
                ////public string FileTitle_Template_SW_Dwg_Radial
                //////=============================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Radial;
                ////    }
                ////}

               
                ////public string FileTitle_Template_SW_Dwg_Seal
                //////==========================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Seal;
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Dwg_CompleteAssy
                //////========================================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_CompleteAssy;
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Dwg_TL_TB
                //////=================================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_TL_TB;
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Dwg_TL_TB_Blank
                //////=================================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_TL_TB_Blank;
                ////    }
                ////}

                ////public string FileTitle_Template_SW_Dwg_Roughing
                //////==============================================
                ////{
                ////    get
                ////    {
                ////        return mDriveRoot + "\\" + mFilePath_Template_SW + "\\" + mcFileTitle_SW_Dwg_Roughing;
                ////    }
                ////}

                //....Inventor Files        //....AES 23JUL18
                public string FileTitle_Template_Inventor_Radial
                //===============================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Radial;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Complete
                //=================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Complete;
                    }
                }

                //public string FileTitle_Template_Inventor_Radial_Assy
                ////===================================================
                //{
                //    get
                //    {
                //        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Radial_Assy;
                //    }
                //}



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

                                    case "FilePath_Template_Inventor":
                                        //----------------------
                                        mFilePath_Template_Inventor = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Radial":
                                        //------------------------------
                                        mFileTitle_Template_Inventor_Radial = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Seal_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Seal_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Thrust_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Thrust_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Complete":
                                        //------------------------------------
                                        mFileTitle_Template_Inventor_Complete = pRChild.InnerText;
                                        break;

                                    //case "FileTitle_Template_Inventor_Radial_Assy":
                                    //    //------------------------------------
                                    //    mFileTitle_Template_Inventor_Radial_Assy = pRChild.InnerText;
                                    //    break;   
                                }
                            }
                            pXML = null;
                            pSW.Close();

                            UpdateAppConfig(mDBServerName);

                }

                catch (FileNotFoundException pEXP)      //BG 13JUL09
                {
                    MessageBox.Show(pEXP.Message, "File Error");        
                }

            }

            private void UpdateAppConfig(String DataSource_In)
            //================================================
            {
                Configuration pConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // ....First Connection String
                // ........Because it's an EF connection string it's not a normal connection string
                // ........so we pull it into the EntityConnectionStringBuilder instead
                EntityConnectionStringBuilder pEFB = new EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString);

                // ....Then we extract the actual underlying provider connection string
                SqlConnectionStringBuilder pSQB = new SqlConnectionStringBuilder(pEFB.ProviderConnectionString);

                // ....Now we can set the datasource
                pSQB.DataSource = DataSource_In;

                // ....Pop it back into the EntityConnectionStringBuilder 
                pEFB.ProviderConnectionString = pSQB.ConnectionString;

                // ....And update
                pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString = pEFB.ConnectionString;

                pConfig.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
       
            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - END                              '
            //---------------------------------------------------------------------------
        #endregion


            #region "SESSION SAVE/RESTORE RELATED ROUTINES:"
            //-----------------------------------------

            #region "SAVE SESSION:"
            //--------------------
            public void Save_SessionData(clsProject Project_In, clsOpCond OpCond_In)
            //=================================================================================================
            {
                try
                {
                    string pFilePath = mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);// mFileName_BearingCAD;

                    Boolean pProject =
                    Project_In.Serialize(pFilePath);

                    Boolean pOpCond =
                    OpCond_In.Serialize(pFilePath);
                                       

                    //....Merge two Binary files created for two different objects.
                    Merge_ObjFiles(pFilePath);

                    //....Delete two Binary files.
                    Delete_ObjFiles(pFilePath);
                }
                catch (Exception pEXP)
                {

                }
            }


            private void Merge_ObjFiles(string FilePath_In)
            //=============================================
            {
                byte[] pHeader;
                byte[] buffer;
                int count = 0;
                string pFileHeader = null;
                FileStream OpenFile = null;

                string pFileName_Out = FilePath_In + ".BearingCAD";
                FileStream OutputFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write);

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    string pFileName = FilePath_In + index + ".BearingCAD";

                    OpenFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //....Initialize the buffer by the total byte length of the file.
                    buffer = new byte[OpenFile.Length];

                    //....Read the file and store it into the buffer.
                    OpenFile.Read(buffer, 0, buffer.Length);
                    count = OpenFile.Read(buffer, 0, buffer.Length);

                    //....Create a header for each file.
                    pFileHeader = "BeginFile" + index + "," + buffer.Length.ToString();

                    //....Transfer the header string into bytes.
                    pHeader = Encoding.Default.GetBytes(pFileHeader);

                    //....Write the header info. into file.
                    OutputFile.Write(pHeader, 0, pHeader.Length);

                    //....Write a Linefeed into file for seperating header info and file info.
                    OutputFile.WriteByte(10); // linefeed

                    //....Write buffer data into file.
                    OutputFile.Write(buffer, 0, buffer.Length);
                    OpenFile.Close();
                }

                OutputFile.Close();
            }

            private void Delete_ObjFiles(string FilePath_In)
            //==========================================
            {
                string pFileName = null;

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    pFileName = FilePath_In + index + ".BearingCAD";
                    File.Delete(pFileName);
                }
            }

            #endregion


            #region "RESTORE SESSION:"
            //------------------------

            public void Restore_SessionData(ref clsProject Project_In, ref clsOpCond OpCond_In, string FilePath_In)
            //======================================================================================================
            {
                try
                {
                    Split_SessionFile();
                    Project_In = (clsProject)modMain.gProject.Deserialize(FilePath_In);
                    OpCond_In = (clsOpCond)modMain.gOpCond.Deserialize(FilePath_In);
                    Delete_ObjFiles(FilePath_In);
                }
                catch (Exception pEXP)
                {

                }
            }

            private void Split_SessionFile()
            //==============================
            {
                string line = null;
                Int32 pLength = 0;
                int pIndex = 1;

                FileStream OpenFile = null;
                OpenFile = new FileStream(mFileName_BearingCAD, FileMode.Open, FileAccess.Read, FileShare.Read);

                while (OpenFile.Position != OpenFile.Length)
                {
                    line = null;
                    while (string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Read the header info.
                        line = ReadLine(OpenFile);
                    }

                    if (!string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Store the total byte length of the file stored into the header.
                        pLength = GetLength(line);
                    }
                    if (!string.IsNullOrEmpty(line))
                    {
                        //....Write bin files from the marged file.
                        Write_ObjFiles(OpenFile, pLength, pIndex);
                        pIndex++;
                    }
                }
                OpenFile.Close();
            }


            private string ReadLine(FileStream fs)
            //===================================
            {
                string line = string.Empty;

                const int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];
                byte b = 0;
                byte lf = 10;
                int i = 0;

                while (b != lf)
                {
                    b = (byte)fs.ReadByte();
                    buffer[i] = b;
                    i++;
                }

                line = System.Text.Encoding.Default.GetString(buffer, 0, i - 1);

                return line;
            }


            private Int32 GetLength(string fileInfo)
            //=====================================
            {
                Int32 pLength = 0;
                if (!string.IsNullOrEmpty(fileInfo))
                {
                    //....get the file information
                    string[] info = fileInfo.Split(',');
                    if (info != null && info.Length == 2)
                    {
                        pLength = Convert.ToInt32(info[1]);
                    }
                }
                return pLength;
            }


            private void Write_ObjFiles(FileStream fs, int fileLength, int Index_In)
            //=====================================================================
            {
                FileStream fsFile = null;
                string pFilePath = "";
                if (mFileName_BearingCAD != "")
                {
                    pFilePath =  mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);
                }

                try
                {
                    string pFileName_Out = pFilePath + Index_In + ".BearingCAD";

                    byte[] buffer = new byte[fileLength];
                    int count = fs.Read(buffer, 0, fileLength);
                    fsFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write, FileShare.None);
                    fsFile.Write(buffer, 0, buffer.Length);
                    fsFile.Write(buffer, 0, count);
                }
                catch (Exception ex1)
                {
                    // handle or display the error
                    throw ex1;
                }
                finally
                {
                    if (fsFile != null)
                    {
                        fsFile.Flush();
                        fsFile.Close();
                        fsFile = null;
                    }
                }
            }

            #endregion

            #endregion

    }
        
}




