//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSWModels                            '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  01JAN13                                '
//                                                                              '
//===============================================================================
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Collections.Specialized;

namespace BearingCAD20
{
    public partial class frmSWModels : Form
    {
        #region "MEMBER VARIABLE"
        //***********************
               
            clsSW_Models mSW_Models = new clsSW_Models();       //AM 01JAN13
             
        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmSWModels()
            {
                InitializeComponent();
       
            }

        #endregion


        #region "FORM EVENT ROUTINES: "
        //*****************************

            private void frmSWModels_Load(object sender, EventArgs e)
            //=======================================================
            {
                DisplayData();
                SetControls_EndConfigs();
            }


            #region "Helper Routines:"
            //************************

                private void DisplayData()
                //========================
                {
                    chkFileModified_CompleteAssy.Checked = modMain.gProject.FileModified_CompleteAssy;

                    chkFileModified_RadialPart.Checked = modMain.gProject.FileModified_RadialPart;
                    chkFileModified_RadialBlankAssy.Checked = modMain.gProject.FileModified_RadialBlankAssy;

                    chkFileModified_EndTB_Part.Checked = modMain.gProject.FileModified_EndTB_Part;
                    chkFileModified_EndTB_Assy.Checked = modMain.gProject.FileModified_EndTB_Assy;

                    chkFileModified_EndSeal_Part.Checked = modMain.gProject.FileModified_EndSeal_Part; 
              
                    txtFileModification_Notes.Text = modMain.gProject.FileModification_Notes;
                }


                private void SetControls_EndConfigs()
                //===================================
                {
                    if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        grpEndSeal.Visible = true;
                        grpEndSeal.Left = 19; 
 
                        grpEndTB.Visible = false;                                       
                    }

                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        grpEndSeal.Visible = true;
                        grpEndSeal.Left = 19;

                        grpEndTB.Visible = true;
                        grpEndTB.Left = 187;
                    }  
                 
                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                    {
                        grpEndTB.Visible = true;
                        grpEndTB.Left = 19;

                        grpEndSeal.Visible = true;                       
                        grpEndSeal.Left = 288;
                    }

                    else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                             modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                    {
                        grpEndSeal.Visible = false;        

                        grpEndTB.Visible = true;
                        grpEndTB.Left = 19; 
                    }
                }

            #endregion

        #endregion


        #region"CONTROL EVENT ROUTINES: "
        //===============================

            #region "COMMAND BUTTON EVENT ROUTINE:"
            //-------------------------------------

                private void cmdButton_Click(object sender, EventArgs e)
                //======================================================
                {
                    Button pCmdBtn = (Button)sender;

                    switch (pCmdBtn.Name)
                    {
                        ////case "cmdCreateRefSwFiles":
                        //////-------------------------

                        ////    //....Ref. Files Path
                        ////    String pRef_FilePath_DesignTbls_SWFiles = modMain.gProject.FilePath_DesignTbls_SWFiles + "\\Ref. Files";
                           
                        ////    Cursor = Cursors.WaitCursor;

                        ////    //....SWFiles: Design Tables, SW Models & Dwg Files.
                        ////    mSW_Models.PopulateAndMove_SWFiles_ProjectFolder(modMain.gFiles, modMain.gProject, modMain.gOpCond,
                        ////                                                     modMain.gDB, pRef_FilePath_DesignTbls_SWFiles);
                        ////    Cursor = Cursors.Default;
                        ////    break;

                        case "cmdPrint":
                        //--------------
                            PrintDocument pPrintDoc = new PrintDocument();
                            pPrintDoc.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                            modMain.CaptureScreen(this);
                            pPrintDoc.Print();               
                            break;

                        case "cmdOK":
                        //-----------
                            SaveData();
                            this.Hide();
                            break;

                        case "cmdCancel":
                        //---------------
                            this.Hide();
                            break;
                    }
                }

     
                #region "Helper Routines:"
                //************************

                    private void SaveData()
                    //=====================
                    {
                        modMain.gProject.FileModified_CompleteAssy = chkFileModified_CompleteAssy.Checked;
                        modMain.gProject.FileModified_RadialPart = chkFileModified_RadialPart.Checked;
                        modMain.gProject.FileModified_RadialBlankAssy = chkFileModified_RadialBlankAssy.Checked;
                        modMain.gProject.FileModified_EndTB_Part = chkFileModified_EndTB_Part.Checked;
                        modMain.gProject.FileModified_EndTB_Assy = chkFileModified_EndTB_Assy.Checked;
                        modMain.gProject.FileModified_EndSeal_Part = chkFileModified_EndSeal_Part.Checked;
                        modMain.gProject.FileModification_Notes = txtFileModification_Notes.Text;
                    }

                #endregion


            #endregion

        #endregion

    }
}
