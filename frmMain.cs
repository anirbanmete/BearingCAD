
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmMain                                '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  10MAY13                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BearingCAD20
{
    public partial class frmMain : Form
    {       
        int mTop_CmdButton;
        int mHt_cmdButton = 32 + 2;
        Boolean mblnExitFromBtn = false;

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //----------------------------------------

            public frmMain()
            //===============
            {
                InitializeComponent();
            }

        #endregion


        #region "FORM RELATED ROUTINE"
        //----------------------------

            private void frmMain_Load(object sender, EventArgs e)
            //===================================================
            {             
                mTop_CmdButton = cmdRadialBearingData.Top + mHt_cmdButton;

                Form pForm = (Form)sender;
                UpdateDisplay(pForm);            //SB 29JUN09
            }

            private void frmMain_Activated(object sender, EventArgs e)
            //========================================================
            {
                Form pForm = (Form)sender;
                UpdateDisplay(pForm);            //SB 29JUN09
            }

            public void UpdateDisplay(Form Form_In)
            //======================================   
            {
                //------------------------------------------------------------
                //....Status Bar Panels: 

                Int32 pWidth = Form_In.Width  / 5;
                SBar1.Width = Form_In.Width;

                SBpanel1.Width = pWidth ;
                SBpanel2.Width = pWidth ;
                SBpanel3.Width = pWidth ;
                SBpanel4.Width = pWidth ;
                SBpanel5.Width = (Form_In.Width - (4 * pWidth));

                SBpanel1.Text = modMain.gUser.Name + " (" + modMain.gUser.Initials + ")";
                if (modMain.gProject != null)
                {
                    //SBpanel2.Text = "Project No: " + modMain.gProject.No;     //BG 10MAY13
                    SBpanel2.Text = "Project No: " + modMain.gProject.No + modMain.gProject.No_Suffix;
                    SBpanel3.Text = "Dwg No: " + modMain.gProject.AssyDwg.No;
                } 
                else
                {
                    SBpanel2.Text = "Project No: ";
                    SBpanel3.Text = "Dwg No: ";
                }    
                SBpanel4.Text = modMain.gUser.Role;
                SBpanel5.Text = DateTime.Today.DayOfWeek.ToString() + ", " +
                                DateTime.Today.ToString(" MMMM dd, yyyy");

                //------------------------------------------------------------------

                //....Form caption.
                this.Text = modMain.gcstrProgramName + " " + modMain.gcstrVersionNo +
                            "                                           Main Form";

                if (modMain.gProject != null)
                {
                    UpdateDisplay_Project();
                }
            }

            private void UpdateDisplay_Project()
            //==================================
            {
                //cmdCreateGCodes.Enabled = true;
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    cmdEndSealData.Visible = true;
                    cmdEndSealDesgnDetail.Visible = true;
                    cmdThrustBearingData.Visible = true;
                    cmdThrustBearingDesgnDetail.Visible = true;
                    cmdCreateGCodes.Visible = true;

                    cmdEndSealData.Top = mTop_CmdButton;
                    cmdThrustBearingData.Top = cmdEndSealData.Top + mHt_cmdButton;

                    SetPosition_CmdButtons(cmdThrustBearingData.Top);

                    //if(((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                    //{
                    //    cmdCreateGCodes.Enabled = false;
                    //}

                }

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    cmdThrustBearingData.Visible = false;
                    cmdThrustBearingDesgnDetail.Visible = false;
                    cmdEndSealData.Visible = true;
                    cmdEndSealDesgnDetail.Visible = true;
                    cmdCreateGCodes.Visible = false;

                    cmdEndSealData.Top = mTop_CmdButton;
                    SetPosition_CmdButtons(cmdEndSealData.Top);
                }

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    cmdThrustBearingData.Visible = true;
                    cmdThrustBearingDesgnDetail.Visible = true;
                    cmdEndSealData.Visible = true;
                    cmdEndSealDesgnDetail.Visible = true;
                    cmdCreateGCodes.Visible = true;

                    cmdThrustBearingData.Top = mTop_CmdButton;
                    cmdEndSealData.Top = cmdThrustBearingData.Top + mHt_cmdButton;

                    SetPosition_CmdButtons(cmdEndSealData.Top);

                    //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                    //{
                    //    cmdCreateGCodes.Enabled = false;
                    //}
                }

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    cmdEndSealData.Visible = false;
                    cmdEndSealDesgnDetail.Visible = false;
                    cmdThrustBearingData.Visible = true;
                    cmdThrustBearingDesgnDetail.Visible = true;
                    cmdCreateGCodes.Visible = true;

                    cmdThrustBearingData.Top = mTop_CmdButton;
                    SetPosition_CmdButtons(cmdThrustBearingData.Top);

                    //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi ||
                    //   ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                    //{
                    //    cmdCreateGCodes.Enabled = false;
                    //}
                }
               
            }

            private void SetPosition_CmdButtons(int Top_In)
            //==============================================
            {
                cmdPerfData.Top = Top_In + mHt_cmdButton;
                cmdRadialBearingDesgnDetail.Top = cmdPerfData.Top + mHt_cmdButton;

                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdThrustBearingDesgnDetail.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateFiles.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                    cmdExit.Top = cmdCreateGCodes.Top + mHt_cmdButton;
                    
                }
                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    cmdEndSealDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                    cmdExit.Top = cmdCreateFiles.Top + mHt_cmdButton;
                }

                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    cmdThrustBearingDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdEndSealDesgnDetail.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateFiles.Top = cmdEndSealDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                    cmdExit.Top = cmdCreateGCodes.Top + mHt_cmdButton;
                    
                }
                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type== clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    cmdThrustBearingDesgnDetail.Top = cmdRadialBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateFiles.Top = cmdThrustBearingDesgnDetail.Top + mHt_cmdButton;
                    cmdCreateGCodes.Top = cmdCreateFiles.Top + mHt_cmdButton;
                    cmdExit.Top = cmdCreateGCodes.Top + mHt_cmdButton;
                }
                
            }


            private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
            //======================================================================
            {
                if (!mblnExitFromBtn)
                {
                    ExitProgram();
                    e.Cancel = true;    
                }
            }

            private void ExitProgram()
            //=============================
            //....Will be fully implemented later. 04MAR07.
            {
                string pstrPrompt = null;
                string pstrTitle = null;
                DialogResult pAnswer;
                Boolean pReturn = false;

                pstrPrompt = " Do you want to exit from application?";
                pstrTitle = "Exit Application";
                pAnswer = MessageBox.Show(pstrPrompt, pstrTitle, MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (pAnswer == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;

                    if (modMain.gProject != null)
                    {                       
                        if(modMain.gDB.ProjectNo_Exists(modMain.gProject.No, modMain.gProject.No_Suffix, "tblProject_Details"))
                        {
                            modMain.gDB.UpdateRecord(modMain.gProject, modMain.gOpCond);
                        }
                        else
                        {
                            modMain.gDB.AddRecord(modMain.gProject,modMain.gOpCond);
                        }
                    }

                    //modMain.gfrmLogIn.Close();

                    //this.Close();               

                    //System.Environment.Exit(0);
                    mblnExitFromBtn = true;
                    Application.Exit();
                    Cursor = Cursors.Default;       
                }
                else if (pAnswer == DialogResult.No)
                    return;
              
            }

        #endregion


        #region "CONTROL EVENT RELATED ROUTINE"
        //-------------------------------------

            #region "MENU ITEM RELATED ROUTINE"
            //---------------------------------

                private void mnuItem_Click(object sender, EventArgs e)
                //====================================================
                {
                    ToolStripMenuItem pMenuStrip = (ToolStripMenuItem)sender;

                    switch (pMenuStrip.Text)
                    {
                        case "&New":
                            break;

                        case "&Open":
                            break;

                        case "&Save":
                            break;

                        case "Save &As":
                            break;

                        case "E&xit":
                            ExitProgram();

                            break;
                    }
                }

            #endregion


            #region "TOOL STRIP RELATED ROUTINE"
            //----------------------------------

                private void ToolStrip_ItemClicked(object sender, EventArgs e)
                //============================================================
                {
                    ToolStripItem pToolStripItem = (ToolStripItem)sender;

                    switch (pToolStripItem.Name)
                    {
                        case "tsNew":
                            break;

                        case "tsOpen":
                            break;

                        case "tsSave":
                            break;

                        case "tsExit":
                           ExitProgram();
                            break;
                    }
                }

            #endregion


            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

                private void cmdButtons_Click(object sender, System.EventArgs e)
                //==============================================================
                {
                    Button pcmdButton = (Button)sender;
                    
                    string pMsg = "Please insert a Project Number";
                    string pCaption = "Project Information Sheet";

                    Cursor = Cursors.WaitCursor;        

                    switch (pcmdButton.Name)
                    {

                        case "cmdProject":
                            //------------
                            modMain.gfrmProject.ShowDialog();
                            break;

                        //case "cmdImportAnalyticalData":
                        //    //-------------------------
                            //if (modMain.gProject != null)
                            //    modMain.gfrmImportData.ShowDialog();
                            //else
                            //    MsgBox(pMsg, pCaption);                           
                            //break;

                        case "cmdOpCond":
                            //-----------
                            if (modMain.gProject != null)
                                modMain.gfrmOperCond.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;          
                
                        case "cmdRadialBearingData":
                            //----------------------
                            if (modMain.gProject != null)
                                modMain.gfrmBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;

                        case "cmdThrustBearingData":
                            //----------------------
                            if (modMain.gProject!= null)
                                modMain.gfrmThrustBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;


                        case "cmdEndSealData":
                            //-------------
                            if (modMain.gProject != null)
                                modMain.gfrmSeal.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;


                        case "cmdPerfData":
                            //-------------

                            if (modMain.gProject!= null)
                                modMain.gfrmPerformDataBearing.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);
                            break;

                        case "cmdRadialBearingDesgnDetail":
                            //-----------------------------
                            if (modMain.gProject != null)
                                modMain.gfrmBearingDesignDetails.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdThrustBearingDesgnDetail":
                            //-----------------------------
                            if (modMain.gProject != null)
                                modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdEndSealDesgnDetail":
                            //---------------------
                            if (modMain.gProject != null)
                            {                                
                                modMain.gfrmSealDesignDetails.ShowDialog();
                               
                            }
                            else
                            {              
                               MsgBox(pMsg, pCaption);
                            }
                            break;

                        case "cmdCreateGCodes":
                            //------------------
                            //if (modMain.gProject != null)
                            //    modMain.gfrmGCodes.ShowDialog();
                            //else
                            //    MsgBox(pMsg, pCaption);


                            if (modMain.gProject != null)
                            {
                                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                    {
                                        MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                                    }
                                }
                               
                                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                                {
                                    if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                    {
                                        MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                                    }
                                }
                                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                                {
                                    if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi ||
                                       ((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[1]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                    {
                                        MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                                    }
                                }
                            
                                //for (int i = 0; i < 2; i++)
                                //{
                                //    //if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL ||)
                                    //{
                                        //if (((clsBearing_Thrust_TL)modMain.gProject.Product.EndConfig[0]).DirectionType == clsBearing_Thrust_TL.eDirectionType.Bi)
                                        //{
                                        //    MessageBox.Show("G-Code generation for Bi-directional TB is not supported in this version.", "G-Code", MessageBoxButtons.OK);
                                        //}
                                    //}
                                //}
                            
                                modMain.gfrmGCodes.ShowDialog();
                            }
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdCreateFiles":                                          
                            //----------------
                            if (modMain.gProject != null)
                                modMain.gCreateFiles.ShowDialog();
                            else
                                MsgBox(pMsg, pCaption);

                            break;

                        case "cmdExit":
                            //---------
                            ExitProgram();
                            break;

                    }

                    Cursor = Cursors.Default;        

                }

                //private void cmdCreateGCodes_MouseHover(object sender, EventArgs e)         //BG 03APR13
                ////=================================================================
                //{
                //    //if(!cmdCreateGCodes.Enabled)
                //    toolTip1.SetToolTip(cmdCreateGCodes, "G-Code generation for Bi-directional TB is not supported in this version.");
                //}


                private void MsgBox(string Msg_In, string Caption_In)                   //SB 09APR09
                //===================================================
                {
                    MessageBox.Show(Msg_In, Caption_In, MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    modMain.gfrmProject.Show();
                }

            #endregion

               
        
        #endregion

    }
}
