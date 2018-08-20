//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmAccessories                         '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  03JUL13                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BearingCAD21
{
    public partial class frmAccessories : Form
    {
        #region "MEMBER VARIABLE DEClARATION"
        //***********************************

            private clsAccessories mAccessories;// = new clsAccessories();
            private int mTempSensor_Count;          

        #endregion

            //....This property used for populating TempSensor_Count ComboBox.
            public int TempSensor_Count
            //=========================
            {
                set { mTempSensor_Count = value; }  
            }

        #region "FORM RELATED ROUTINE"
        //============================

            public frmAccessories()
            //======================
            {
                InitializeComponent();

            }

            private void frmAccessories_Load(object sender, EventArgs e)
            //==========================================================
            {
                //....Initialize Control.
                InitializeControls();

                //....Local Object.
                SetLocalObject();   

                //....Set Temp Sensor Control.
                SetTempSensorControl();

                //....Set Wire Clip Control.
                SetWireClipControl();

                //....Display Data.
                DisplayData();      

                SetControl();       
            }

            private void SetControl()   
            //=======================                           
            {
                ////Boolean pEnabled;
                ////if (modMain.gProject.Status == "Open" &&
                ////    (modMain.gUser.Role == "Engineer" || modMain.gUser.Role == "Designer"))                   
                ////{
                ////    pEnabled = true;
                ////    SetControlStatus(pEnabled);
                ////}
                ////else if (modMain.gProject.Status == "Closed" ||
                ////         modMain.gUser.Role != "Engineer" || modMain.gUser.Role != "Designer")                   
                ////{
                ////    pEnabled = false;
                ////    SetControlStatus(pEnabled);
                ////}

            }

            private void SetControlStatus(Boolean pEnable_In)      
            //===============================================
            {
                //....Temp. Sensor.               
                chkTempSensor_Supplied.Enabled = pEnable_In;
                cmbTempSensor_Name.Enabled = pEnable_In;
                cmbTempSensor_Count.Enabled = pEnable_In;
                cmbTempSensor_Type.Enabled = pEnable_In;
               
                //....Wire Clip.
                chkWireClip_Supplied.Enabled = pEnable_In;
                cmbWireClip_Count.Enabled = pEnable_In;
                cmbWireClip_Size.Enabled = pEnable_In;

            }

            public void SetWireClipControl()        
            //==============================
            {

                //.....Populate cmbTempQty.
                cmbWireClip_Count.Items.Clear();

                const int pTempQtyCount = 10;

                for (int i = 1; i <= pTempQtyCount; i++)
                {
                    cmbWireClip_Count.Items.Add(i);
                }

                cmbWireClip_Count.SelectedIndex = 0;

                //.....Populate cmbWCSize.
                LoadWireClipSize();
                cmbWireClip_Size.SelectedIndex = 0;

            }

            public void SetTempSensorControl()      
            //================================
            {
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                   modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    lblTempSensor.Visible = false;
                    chkTempSensor_Supplied.Visible = false;
                }
                else
                {
                    lblTempSensor.Visible = true;
                    chkTempSensor_Supplied.Visible = true;

                    //.....Populate cmbTempSensor.
                    LoadTempSensorName();
                    cmbTempSensor_Name.SelectedIndex = 0;

                    //.....Populate cmbTempQty.
                    cmbTempSensor_Count.Items.Clear();
                    for (int i = 1; i <= mTempSensor_Count; i++)
                    {
                        cmbTempSensor_Count.Items.Add(i);
                    }
                    cmbTempSensor_Count.SelectedIndex = 0;
                }

            }

            private void LoadTempSensorName()   
            //===============================
            {
                cmbTempSensor_Name.DataSource = Enum.GetValues(typeof(clsAccessories.eTempSensorName));
                cmbTempSensor_Name.SelectedIndex = 0;
            }

            private void PopulateTempSensorType()
            //===================================   //SB 09JUL09
            {
                cmbTempSensor_Type.Items.Clear();
                if (cmbTempSensor_Name.Text == "TC")
                {
                    cmbTempSensor_Type.Items.Add("E");
                    cmbTempSensor_Type.Items.Add("J");
                    cmbTempSensor_Type.Items.Add("K");
                    cmbTempSensor_Type.Items.Add("T");
                }
                else if (cmbTempSensor_Name.Text == "RTD")
                {
                    cmbTempSensor_Type.Items.Add("2");
                    cmbTempSensor_Type.Items.Add("3");
                }

                cmbTempSensor_Type.SelectedIndex = 0;
            }

            public void LoadWireClipSize()      
            //============================
            {
                cmbWireClip_Size.DataSource = Enum.GetValues(typeof(clsAccessories.eWireClipSize));
                cmbWireClip_Size.SelectedIndex = 0;
            }

             
            private void cmbTCWireClip_SelectedIndexChanged(object sender, EventArgs e)
            //=========================================================================
            {
                InitializeControls();
            }

            private void InitializeControls()       
            //===============================
            {
                    chkTempSensor_Supplied.Checked = false;
                    lblQty.Visible = false;             //BG 13APR09
                    cmbTempSensor_Name.Visible = false;
                    cmbTempSensor_Count.Visible = false;
                    lblTempType.Visible = false;
                    cmbTempSensor_Type.Visible = false;

                    chkWireClip_Supplied.Checked = false;
                    cmbWireClip_Count.Visible = false;
                    lblWCSize.Visible = false;
                    cmbWireClip_Size.Visible = false;

            }

            private void DisplayData()  //SB 13APR09    Method changed.
            //========================
            {

                //  TempSensor
                //  ----------
                chkTempSensor_Supplied.Checked = modMain.gProject.Product.Accessories.TempSensor.Supplied; 
                cmbTempSensor_Name.Text = modMain.gProject.Product.Accessories.TempSensor.Name.ToString();
                cmbTempSensor_Count.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.TempSensor.Count);
                cmbTempSensor_Type.Text = modMain.gProject.Product.Accessories.TempSensor.Type.ToString();

                //  Wire Clip
                //  ---------

                chkWireClip_Supplied.Checked = modMain.gProject.Product.Accessories.WireClip.Supplied;
                cmbWireClip_Count.Text = modMain.ConvIntToStr(modMain.gProject.Product.Accessories.WireClip.Count);
                cmbWireClip_Size.Text = modMain.gProject.Product.Accessories.WireClip.Size.ToString();
            }

            private void SetLocalObject()
            //===========================
            {
                mAccessories = (clsAccessories)modMain.gProject.Product.Accessories.Clone();     
            }

        #endregion

        #region "CONTROL EVENT RELATED ROUTINE"
        //*************************************

            #region "CHECK BOX RELATED ROUTINE"
            //---------------------------------

                private void ChkBox_CheckedChanged(object sender, EventArgs e)
                //=============================================================     //BG 13APR09
                {
                    CheckBox pChkBox = (CheckBox)sender;

                    switch (pChkBox.Name)
                    {
                        case "chkTempSensor_Supplied":
                            //------------------------
                            if (pChkBox.Checked)
                            {
                                cmbTempSensor_Name.Visible = true;
                                cmbTempSensor_Count.Visible = true;
                                lblTempType.Visible = true;
                                cmbTempSensor_Type.Visible = true;
                            }
                            else
                            {
                                cmbTempSensor_Name.Visible = false;
                                cmbTempSensor_Count.Visible = false;
                                lblTempType.Visible = false;
                                cmbTempSensor_Type.Visible = false;
                            }

                            mAccessories.TempSensor_Supplied = chkTempSensor_Supplied.Checked;   //BG 23APR09

                            break;

                        case "chkWireClip_Supplied":
                            //---------------------
                            if (pChkBox.Checked)
                            {
                                cmbWireClip_Count.Visible = true;
                                lblWCSize.Visible = true;
                                cmbWireClip_Size.Visible = true;
                            }
                            else
                            {
                                cmbWireClip_Count.Visible = false;
                                lblWCSize.Visible = false;
                                cmbWireClip_Size.Visible = false;
                            }

                            mAccessories.WireClip_Supplied = chkWireClip_Supplied.Checked;       //BG 23APR09

                            break;
                    }

                    if (chkTempSensor_Supplied.Checked || chkWireClip_Supplied.Checked)
                    {
                        lblQty.Visible = true;
                    }
                    else
                        lblQty.Visible = false;

                }

            #endregion

            #region "COMBO BOX RELATED ROUTINE"
            //---------------------------------

            private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
            //===================================================================== //BG 23APR09
            {
                ComboBox pCmbBox = (ComboBox)sender;

                switch (pCmbBox.Name)
                {
                    case "cmbTempSensor_Name":
                        //====================
                        if (pCmbBox.Text != "")
                        {
                            mAccessories.TempSensor_Name = (clsAccessories.eTempSensorName)Enum.Parse(
                                                            typeof(clsAccessories.eTempSensorName), pCmbBox.Text);
                            PopulateTempSensorType();   //SB 09JUL09
                        }
                        break;

                    case "cmbTempSensor_Count":
                        //====================
                        mAccessories.TempSensor_Count = modMain.ConvTextToInt(pCmbBox.Text);
                        break;

                    case "cmbTempSensor_Type":
                        //====================
                        if (pCmbBox.Text != "")
                        {
                            mAccessories.TempSensor_Type = (clsAccessories.eTempSensorType)Enum.Parse(
                                                            typeof(clsAccessories.eTempSensorType), pCmbBox.Text);
                        }

                        break;

                    case "cmbWireClip_Count":
                        //====================
                        int pCount = modMain.ConvTextToInt(pCmbBox.Text);

                        const int pcMaxWireClip_Count = 3;

                        if (pCount <= pcMaxWireClip_Count)
                        {
                            mAccessories.WireClip_Count = modMain.ConvTextToInt(cmbWireClip_Count.Text);
                        }
                        else
                        {
                            string pMsg = "Wire Clip Count should be less than or equal to 3";          //SB 13JUL09
                            String pCaption = "WireClip Count Error.";
                            MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            mAccessories.WireClip_Count = pcMaxWireClip_Count;
                            cmbWireClip_Count.Text = modMain.ConvIntToStr(pcMaxWireClip_Count);
                        }
                        break;

                    case "cmbWireClip_Size":
                        //====================
                        if (pCmbBox.Text!="")
                        mAccessories.WireClip_Size = (clsAccessories.eWireClipSize)
                            Enum.Parse(typeof(clsAccessories.eWireClipSize),pCmbBox.Text);
                        break;
                }
            }

            #endregion

            #region "COMMAND BUTTON RELATED ROUTINE"
            //======================================

            private void cmdOK_Click(object sender, EventArgs e)
            //===================================================
            {
                CloseForm();
            }

            private void CloseForm()
            //======================
            {
                SaveData();

                //Boolean mblnCheckChange;

                //if (!modMain.gDB.CheckProjectNo(modMain.gProject.No,modMain.gProject.No_Suffix,  "tblProject_Accessories"))     
                //{
                //    Cursor = Cursors.WaitCursor;
                //    SaveData();
                //    modMain.gProject.Product.Accessories.AddRec_Accessories(modMain.gProject.No, modMain.gDB); //SB 09JUN09
                //    Cursor = Cursors.Default;
                //}
                //else
                //{

                //    mblnCheckChange = modMain.gProject.Product.Accessories.Compare(ref mAccessories);    
                //    if (mblnCheckChange)
                //    {
                //        SaveData();  
                //        Cursor = Cursors.WaitCursor;
                //        modMain.gProject.Product.Accessories.UpdateRec_Accessories(modMain.gProject.No, modMain.gDB);//SB 09JUN09
                //        Cursor = Cursors.Default;
                //    }
                //}

                this.Hide();
            }

            private void cmdCancel_Click(object sender, EventArgs e)
            //======================================================
            {
                //SaveData();

                //if (modMain.gProject.Product.Accessories.Compare(ref mAccessories))    
                //{
                //    int pAns = (int)MessageBox.Show("Data has been modified in this form." +
                //            System.Environment.NewLine + "Do you want to save before exit?"
                //            , "Save Record", MessageBoxButtons.YesNo,
                //            MessageBoxIcon.Question);

                //    const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

                //    if (pAns == pAnsY)
                //    {
                //        SaveData();
                //        //CloseForm();
                //    }
                //    //else
                //    //{
                //    //    modMain.gAccessories = (clsAccessories)mAccessories.Clone();
                //    //    this.Hide();
                //    //}
                //}
                ////else
                ////{
                ////    this.Hide();
                ////}
                //CloseForm();

                this.Hide();
            }

        #endregion

        #endregion

        #region "UTILITY ROUTINE"
            //=======================

            private void SaveData()
            //=====================
            {
                //.....Save Data in Temp Sensor. 

                modMain.gProject.Product.Accessories.TempSensor_Supplied = chkTempSensor_Supplied.Checked;   
                modMain.gProject.Product.Accessories.TempSensor_Count = modMain.ConvTextToInt(cmbTempSensor_Count.Text);

                if (cmbTempSensor_Name.Text!="")
                    modMain.gProject.Product.Accessories.TempSensor_Name = (clsAccessories.eTempSensorName)Enum.Parse(
                                                        typeof(clsAccessories.eTempSensorName), cmbTempSensor_Name.Text);
                if (cmbTempSensor_Type.Text!="")    
                    modMain.gProject.Product.Accessories.TempSensor_Type = (clsAccessories.eTempSensorType)Enum.Parse(
                                                        typeof(clsAccessories.eTempSensorType),cmbTempSensor_Type.Text);

                //.....Save Data in Wire Clip.

                modMain.gProject.Product.Accessories.WireClip_Supplied = chkWireClip_Supplied.Checked;       //SB 13APR09
                modMain.gProject.Product.Accessories.WireClip_Count = modMain.ConvTextToInt(cmbWireClip_Count.Text);

                if(cmbWireClip_Size.Text!="")
                    modMain.gProject.Product.Accessories.WireClip_Size = (clsAccessories.eWireClipSize)
                                                     Enum.Parse(typeof(clsAccessories.eWireClipSize),cmbWireClip_Size.Text);
            }

        #endregion
          
    }
}
