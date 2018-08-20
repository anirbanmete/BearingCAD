
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsReport                              '
//                        VERSION NO  :  2.0                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  12AUG14                                '
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
using Microsoft.Office;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace BearingCAD21
{
    class clsReport
    {

        //Object[] objBookMarksName = new Object[] {  "bmrkProjNo", "bmrkCustName", 
        //                                            "bmrkMachineDesc","bmrkProjEngg", 
        //                                            "bmrkDate", "bmrkDwgNo", "bmrkDwgRef", 
        //                                            "bmrkCheckedBy", "bmrkCheckedByDate", 
        //                                            "bmrkSpeed", "bmrkPadType", "bmrkLoad", 
        //                                            "bmrkRot", "bmrkRot1", "bmrkLube", 
        //                                            "bmrkType", "bmrkOilSupplyPress", 
        //                                            "bmrkOilSupplyTemp", 
        //                                            "bmrkBearingType", "bmrkSplitConfig",
        //                                            "bmrkShaftD", "bmrkSetD", 
        //                                            "bmrkClearance", "bmrkPreLoad",
        //                                            "bmrkPadD", "bmrkBearingDO", 
        //                                            "bmrkBearingType_Web", "bmrkPWThick", "bmrkPWHt", 
        //                                            "bmrkPWRFillet", "bmrkRotationStiff",
        //                                            "bmrkEDMWireCut", "bmrkDOilInOrifice",
        //                                            "bmrkMat", "bmrkLiningMat", "bmrkT", "bmrkCount",
        //                                            "bmrkL", "bmrkAng", "bmrkPiOffset",
        //                                            "bmrkPiLocn", "bmrkPBRLead", 
        //                                            "bmrkPBRTrail", "bmrkThLead", 
        //                                            "bmrkThPivot", "bmrkThTrail", 
        //                                            "bmrkSealType_Front", 
        //                                            "bmrkDIn_Front", 
        //                                            "bmrkBladeCount_Front", "bmrkBladeL_Front", 
        //                                            "bmrkSealDrnHD_Front", 
        //                                            "bmrkPLoss", 
        //                                            "bmrkReqFlow", "bmrkTempRise", 
        //                                            "bmrkMinFilmT", "bmrkTemp", 
        //                                            "bmrkPress", "bmrkPadMaxRot", 
        //                                            "bmrkPadMaxLoad", "bmrkStress"};


        Object[] objBookMarksName = new Object[] {  "Text5", "txtCustName", 
                                                     "blank1", "blank2", 
                                                    "Text7",  
                                                    "bmrkDesignedBy", "bmrkDesignedByDate",
                                                    "bmrkCheckedBy", "bmrkCheckedByDate", 
                                                    "bmrkShaftSpeed", "bmrkPadType", "bmrkLoad", 
                                                    "bmrkRot", "bmrkRot1", "bmrkLube", 
                                                    "bmrkType", "bmrkOilSupplyPress", 
                                                    "bmrkOilSupplyTemp", 
                                                    "bmrkBearingType", "Split",
                                                    "Shaft_Max", "BearingBore_Min", 
                                                    "MaxBearingBoreClear", "Preload",
                                                    "PadBore_Min", "BearingOD_max", 
                                                    "bmrkBearingType_Web", "WebThk", "WebHght", 
                                                    "WebRadii", "RotStiff",
                                                    "EDMWireGap", "OrificeQty",
                                                    "HousingMatl", "bmrkLiningMat", "BabbittThk", "PadQty",
                                                    "PadLgth", "PadAngle", "PivotOffset",
                                                    "PivotStart", "bmrkPBRLead", 
                                                    "bmrkPBRTrail", "PadThkLead", 
                                                    "PadThkPivot", "PadThkTrail", 
                                                    "FrontSeal", 
                                                    "FrontSealBore_Min", 
                                                    "FrontBaldeQty", "FrontBladeThk", 
                                                    "FrontDrainDia", 
                                                    "PowerLoss", 
                                                    "FlowRate", "TempRise", 
                                                    "MinFilmThk", "PadMaxTEmp", 
                                                    "PadMaxPress", "PadMaxRot", 
                                                    "PadMaxLoad", "PadMaxStress", "bmrkSupplyPress_MPa",
                                                    "bmrkLoad_kN", "bmrkLoadAngle","bmrkSupplyTemp_C",
                                                    "Shaft_Min", "Shaft_Max_mm", "Shaft_Min_mm",
                                                    "BearingBore_Max","BearingBore_Min_mm", "BearingBore_Max_mm",
                                                    "MinBearingBoreClear","PadBore_Max","PadBore_Min_mm","PadBore_Max_mm",
                                                    "BearingOD_Min","BearingOD_Max_mm","BearingOD_Min_mm",
                                                    "OrificeDia", "OrificeDia_mm","LiningMatl",
                                                    "PadLgth_mm", "Pivot2nd","Pivot3rd","Pivot4th",
                                                    "BackSeal", "BackSealBore_Max", "FrontSealBore_Min_mm", "FrontSealBore_Max_mm",
                                                     "BackSealBore_Min", "FrontSealBore_Max","BackSealBore_Min_mm","BackSealBore_Max_mm",
                                                     "BackBladeQty", "BackBladeThk", "FrontDrainQty","BackDrainQty","BackDrainDia",
                                                     "PowerLoss_kW", "FlowRate_lpm","TempRise_C", "HydroLiftPress","HydroLiftPress_MPa",
                                                     "HydroLiftFlow", "HydroLiftFlow_lpm"};

        //BG 03JUN09
        Object[] objBookMarksSummaryInfo = new Object[] {"bmrkProjectNo", 
                                                        "bmrkCustomerName", 
                                                        "bmrkDwgNo","bmrkUnit", 
                                                        "bmrkSplitLine_Thread_LLowerLimit", 
                                                        "bmrkSplitLine_Pin_LLowerLimit", 
                                                        "bmrkSealFix_DBC", 
                                                        "bmrkSealFix_Do_Max", 
                                                        "bmrkThreadLengths_LLower", 
                                                        "bmrkThreadLengths_LUpper", 
                                                        "bmrkClearance", 
                                                        "bmrkMountHoles_CBoreDepth_LowerLimit", 
                                                        "bmrkMountHoles_CBoreDepth_UpperLimit"};


        //BG 28DEC12
        //public void WriteReport(clsFiles Files_In, clsProject Project_In, clsUnit Unit_In, 
        //                        clsOpCond OpCond_In, string ProjectPath_In)

        public void Populate_DDR(clsFiles Files_In, clsProject Project_In, clsUnit Unit_In, 
                                 clsOpCond OpCond_In, string ProjectPath_In)
        //==================================================================================      
        {
            CloseWordFiles();

            Object pInfoSheetName = Files_In.FileTitle_Template_DDR;
            Object pMissing = Missing.Value;


            if (!Directory.Exists(ProjectPath_In))
                Directory.CreateDirectory(ProjectPath_In);


            //Object pDocName = Project_In.No + "_DIS";                                 //BG 22MAR13
            Object pDocName = Project_In.No + Project_In.No_Suffix + "_D";            //BG 22MAR13

            Word.Application pWordApp = new Word.Application();
            Word.Document pWordDoc = null;

            try
            {

                pWordDoc = pWordApp.Documents.Add(ref pInfoSheetName, ref pMissing, ref pMissing, ref pMissing);

                //---------------------------------------------------------------------------------------------------

                pWordDoc.Unprotect();

                //   Set Line Rotation           
                //   -----------------
                object pLine = "Line 7";

                //....Original Top, Left & Height of Line.
                Double pOrgTop = 0.0F;
                Double pOrgLeft = 0.0F;
                Double pOrgHeight = 0.0F;

                //....Constant Value taken as radius.
                Double pcRad = 0.0F;
                pcRad = 30.0F;

                //....Height, Width, Top & Left.
                Double pHeight = 0.0F;
                Double pWidth = 0.0F;
                Double pTop = 0.0F;
                Double pLeft = 0.0F;

                Word.Shape pShape = pWordDoc.Shapes.get_Item(ref pLine);

                pOrgTop = pShape.Top;
                pOrgHeight = pShape.Height;
                pOrgTop = pOrgTop + pOrgHeight;
                pOrgLeft = pShape.Left;

                //....Rotate Vector Angle 0 to 90.
                if (OpCond_In.Radial_LoadAng >= 0.0F && OpCond_In.Radial_LoadAng <= 90.0F)
                {
                    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pTop = pOrgTop - pHeight;
                    pLeft = pOrgLeft;

                    pShape.Select(ref pMissing);

                    pShape.Width = (Single)pWidth;
                    pShape.Height = (Single)pHeight;
                    pShape.Top = (Single)pTop;
                    pShape.Left = (Single)pLeft;

                }

                //....Rotate Vector Angle 90 to 180.
                else if (OpCond_In.Radial_LoadAng >= 90.0F && OpCond_In.Radial_LoadAng <= 180.0F)
                {
                    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pTop = pOrgTop - pHeight;
                    pLeft = pOrgLeft + pWidth;

                    pShape.Select(ref pMissing);
                    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipHorizontal);

                    pShape.Width = Convert.ToSingle(-pWidth);
                    pShape.Height = (Single)pHeight;
                    pShape.Top = (Single)pTop;
                    pShape.Left = (Single)pLeft;

                }

                //....Rotate Vector Angle 180 to 270.
                else if (OpCond_In.Radial_LoadAng >= 180.0F && OpCond_In.Radial_LoadAng <= 270.0F)
                {
                    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pTop = pOrgTop;
                    pLeft = pOrgLeft + pWidth;

                    pShape.Select(ref pMissing);

                    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipHorizontal);
                    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipVertical);

                    pShape.Width = Convert.ToSingle(-pWidth);
                    pShape.Height = Convert.ToSingle(-pHeight);
                    pShape.Top = (Single)pTop;
                    pShape.Left = (Single)pLeft;

                }

                //....Rotate Vector Angle 270 to 360.
                else if (OpCond_In.Radial_LoadAng >= 270.0F && OpCond_In.Radial_LoadAng <= 360.0F)
                {
                    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                    pTop = pOrgTop;
                    pLeft = pOrgLeft;

                    pShape.Select(ref pMissing);
                    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipVertical);

                    pShape.Width = (Single)pWidth;
                    pShape.Height = Convert.ToSingle(-pHeight);
                    pShape.Top = (Single)pTop;
                    pShape.Left = (Single)pLeft;

                }

                pWordDoc.Protect(Word.WdProtectionType.wdAllowOnlyFormFields);

                //---------------------------------------------------------------------------------------------------

                //   Project related bookmark:
                //   -------------------------

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[0]).Range.Text = Project_In.No;      //BG 10MAY13
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[0]).Range.Text = Project_In.No + Project_In.No_Suffix;      //BG 10MAY13
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[1]).Range.Text = Project_In.Customer.Name;

                pWordDoc.FormFields[objBookMarksName[0]].Result = Project_In.No + Project_In.No_Suffix;      //BG 10MAY13
                pWordDoc.FormFields[objBookMarksName[1]].Result = Project_In.Customer.Name;
                                
               
                pWordDoc.FormFields[objBookMarksName[4]].Result = Project_In.AssyDwg.No + "-" + Project_In.AssyDwg.No_Suffix;   //BG 25MAR13

                //....Ref Dwg No.
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[6]).Range.Text = Project_In.AssyDwg.Ref;

                //....Default Date if the Date TextBox is blank.
                string pstrDefDate = DateTime.MinValue.ToShortDateString();

                //....DesignedBy    
                if (Project_In.DesignedBy.Name != "")
                    pWordDoc.FormFields[objBookMarksName[5]].Result =
                                                         Project_In.DesignedBy.Name + "  (" +
                                                         Project_In.DesignedBy.Initials + ")";
                if (Project_In.DesignedBy.Date.ToShortDateString() != pstrDefDate)
                    pWordDoc.FormFields[objBookMarksName[6]].Result =
                                                         Project_In.DesignedBy.Date.ToShortDateString();

                //....CheckedBy
                if (Project_In.CheckedBy.Name != "")
                    pWordDoc.FormFields[objBookMarksName[7]].Result =
                                                         Project_In.CheckedBy.Name + "  (" +
                                                         Project_In.CheckedBy.Initials + ")";
                if (Project_In.CheckedBy.Date.ToShortDateString() != pstrDefDate)
                    pWordDoc.FormFields[objBookMarksName[8]].Result =
                                                         Project_In.CheckedBy.Date.ToShortDateString();

                //   OpCond related bookmark
                //   ------------------------

                ////if (OpCond_In.Speed() > 0.00F)       //BG 22NOV11 
                ////    pWordDoc.FormFields[objBookMarksName[9]].Result =
                ////                                modMain.ConvIntToStr(OpCond_In.Speed());

                pWordDoc.FormFields[objBookMarksName[10]].Result = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString();

                //AES 11AUG14
                ////if (OpCond_In.Radial_Load() > 0.00F)
                ////{
                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[11]).Range.Text =
                ////    //                                  modMain.ConvDoubleToStr(OpCond_In.Radial_Load(), "") + " (" +
                ////    //                                  modMain.ConvDoubleToStr((Double)
                ////    //                                  Unit_In.CFac_Load_EngToMet(OpCond_In.Radial_Load()), "#0.00") + "kN) @ " +
                ////    //                                  modMain.ConvDoubleToStr(OpCond_In.Radial_LoadAng, "#0") + Convert.ToChar(176);
                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[11]).Range.Text =
                ////    //                                 modMain.ConvDoubleToStr(OpCond_In.Radial_Load(), "") + " (" +
                ////    //                                 modMain.ConvDoubleToStr((Double)
                ////    //                                 Unit_In.CFac_Load_EngToMet(OpCond_In.Radial_Load()), "#0.00") + "kN) @ " +
                ////    //                                 modMain.ConvDoubleToStr(OpCond_In.Radial_LoadAng, "#0") + Convert.ToChar(176);

                ////    pWordDoc.FormFields[objBookMarksName[11]].Result =
                ////                                    modMain.ConvDoubleToStr(OpCond_In.Radial_Load(), "");

                ////    pWordDoc.FormFields[objBookMarksName[61]].Result = modMain.ConvDoubleToStr((Double)
                ////                                     Unit_In.CFac_Load_EngToMet(OpCond_In.Radial_Load()), "#0.00");

                ////    pWordDoc.FormFields[objBookMarksName[62]].Result = modMain.ConvDoubleToStr(OpCond_In.Radial_LoadAng, "#0");

                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[61]).Range.Text = modMain.ConvDoubleToStr((Double)
                ////    //                                 Unit_In.CFac_Load_EngToMet(OpCond_In.Radial_Load()), "#0.00") ;
                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[62]).Range.Text = modMain.ConvDoubleToStr(OpCond_In.Radial_LoadAng, "#0");
                ////}

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[12]).Range.Text =
                //                                  OpCond_In.Rot_Direction.ToString() + " ";
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[13]).Range.Text =
                //                                  OpCond_In.Rot_Direction.ToString().ToLower() + " ";


               ////pWordDoc.FormFields[objBookMarksName[14]].Result = OpCond_In.OilSupply.Lube.Type.ToString();
               //// pWordDoc.FormFields[objBookMarksName[15]].Result = OpCond_In.OilSupply.Type;  


                ////if (OpCond_In.OilSupply.Press > 0.00F)
                ////{
                ////    Double pPress_Met = (Double)Unit_In.CFac_Press_EngToMet((double)OpCond_In.OilSupply.Press);
                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[16]).Range.Text = modMain.ConvDoubleToStr(OpCond_In.OilSupply.Press, "#0") + " (" +
                ////    //                                                                   modMain.ConvDoubleToStr(pPress_Met, "#0.000") + " MPa)";

                ////    pWordDoc.FormFields[objBookMarksName[16]].Result = modMain.ConvDoubleToStr(OpCond_In.OilSupply.Press, "#0");
                ////    pWordDoc.FormFields[objBookMarksName[60]].Result = modMain.ConvDoubleToStr(pPress_Met, "#0.000");

                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[60]).Range.Text = modMain.ConvDoubleToStr(pPress_Met, "#0.000");

                ////}

                ////if (OpCond_In.OilSupply.Temp > 0.00F)
                ////{
                ////    int pTemp_Met = (int)modMain.NInt(Unit_In.CFac_Temp_EngToMet((double)OpCond_In.OilSupply.Temp));
                ////    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[17]).Range.Text =
                ////    //                                 modMain.ConvDoubleToStr(OpCond_In.OilSupply.Temp, "") + " (" +
                ////    //                                 modMain.ConvDoubleToStr(pTemp_Met, "") + Convert.ToChar(176) + "C)";

                ////    pWordDoc.FormFields[objBookMarksName[17]].Result =
                ////                                     modMain.ConvDoubleToStr(OpCond_In.OilSupply.Temp, "");

                ////    pWordDoc.FormFields[objBookMarksName[63]].Result = modMain.ConvDoubleToStr(pTemp_Met, "");
                ////}

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[17]).Range.Text = Project_In.RadialType.ToString(); 
                pWordDoc.FormFields[objBookMarksName[18]].Result = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Design.ToString().Replace("_", " ").Trim();


                //   Bearing related bookmark:
                //   -------------------------

                string pstrSplitConfig;
                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                    pstrSplitConfig = "Yes"; //SB 10JUL09
                else
                    pstrSplitConfig = "No";  //SB 10JUL09


                pWordDoc.FormFields[objBookMarksName[19]].Result =
                                                     pstrSplitConfig;

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[20]).Range.Text =
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1], "#0.0000") + " / " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0], "#0.0000") + "\n(" +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1]), "#0.000") + " / " +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0]), "#0.000") + ")";

                pWordDoc.FormFields[objBookMarksName[20]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1], "#0.0000");

                pWordDoc.FormFields[objBookMarksName[64]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[65]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1]), "#0.000");
                pWordDoc.FormFields[objBookMarksName[66]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0]), "#0.000");


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[21]).Range.Text =
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0], "#0.0000") + " / " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1], "#0.0000") + "\n(" +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0]), "#0.000") + " / " +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1]), "#0.000") + ")";

                //AES 11AUG14
                pWordDoc.FormFields[objBookMarksName[21]].Result =
                                                    modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[67]].Result =
                                                    modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[68]].Result =
                                                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0]), "#0.000");
                pWordDoc.FormFields[objBookMarksName[69]].Result =
                                                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1]), "#0.000");


                //BG 27JUL11
                pWordDoc.FormFields[objBookMarksName[22]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Clearance(), "#0.0000");

                //BG 24JUN11
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[21]).Range.Text =
                //                                    modMain.ConvDoubleToStr(Bearing_In.Clearance(), "#0.0000");


                //BG 27JUL11
                pWordDoc.FormFields[objBookMarksName[23]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PreLoad(), "#0.000");

                //BG 24JUN11
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[22]).Range.Text =
                //                                  modMain.ConvDoubleToStr(Bearing_In.PreLoad(), "#0.000");     


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[24]).Range.Text =
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0], "#0.0000") + " / " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1], "#0.0000") + "\n(" +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0]), "#0.000") + " / " +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1]), "#0.000") + ")";

                pWordDoc.FormFields[objBookMarksName[24]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[71]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[72]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[0]), "#0.000");
                pWordDoc.FormFields[objBookMarksName[73]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DPad_Range[1]), "#0.000");

                //SB 12OCT09 change format min/max to max/min.
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[25]).Range.Text =
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1], "#0.0000") + " / " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0], "#0.0000") + "\n(" +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1]), "#0.000") + " / " +
                //                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0]), "#0.000") + ")";

                //AES 11AUG14
                pWordDoc.FormFields[objBookMarksName[25]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[74]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0], "#0.0000");
                pWordDoc.FormFields[objBookMarksName[75]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[1]), "#0.000");
                pWordDoc.FormFields[objBookMarksName[76]].Result =
                                                     modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).DFit_Range[0]), "#0.000");

                //AES 12AUG14
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[26]).Range.Text =
                //                                     ((clsBearing_Radial_FP)Project_In.Product.Bearing).Design.ToString().Replace("_", " ").Trim();

                pWordDoc.FormFields[objBookMarksName[27]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T, "#0.000");
                pWordDoc.FormFields[objBookMarksName[28]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H, "#0.000");
                pWordDoc.FormFields[objBookMarksName[29]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet, "#0.000");
                pWordDoc.FormFields[objBookMarksName[30]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Rot_Stiff, "#0");
                pWordDoc.FormFields[objBookMarksName[31]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM, "#0.000");


                //BG 30JUN11
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[32]).Range.Text =
                //                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count) + " " +
                //                                     "orifices- " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D, "#0.000") +
                //                                     "'' (" + modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D), "#0.000") + ")";

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[32]).Range.Text =
                //                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count) + " " +
                //                                     "orifices- " +
                //                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D, "#0.000") +
                //                                     "'' (" + modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D), "#0.000") + ")";

                pWordDoc.FormFields[objBookMarksName[32]].Result =
                                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count);
                pWordDoc.FormFields[objBookMarksName[77]].Result =
                                                    modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D, "#0.000");
                pWordDoc.FormFields[objBookMarksName[78]].Result =
                                                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D), "#0.000");


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[33]).Range.Text =
                //                                             ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base + ", " +
                //                                             ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;

                pWordDoc.FormFields[objBookMarksName[33]].Result =
                                                           ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base;
                pWordDoc.FormFields[objBookMarksName[79]].Result =
                                                           ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[34]).Range.Text =
                //                                     ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining + " ";
                pWordDoc.FormFields[objBookMarksName[35]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT, "#0.000");
                pWordDoc.FormFields[objBookMarksName[36]].Result =
                                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count);


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[37]).Range.Text =
                //                                   modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L, "#0.000") + " (" +
                //                                   modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L), "#0.000") + ")";      //BG 13JUN11

                pWordDoc.FormFields[objBookMarksName[37]].Result =
                                                   modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L, "#0.000");
                pWordDoc.FormFields[objBookMarksName[80]].Result =
                                                  modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L), "#0.000");



                pWordDoc.FormFields[objBookMarksName[38]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Angle(), "#0");
                pWordDoc.FormFields[objBookMarksName[39]].Result =
                                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset, "#0.0");    //SB 12OCT09 

                //string pstrLocation = "";
                //string pstrChar = ", ";

                //AES 11AUG14
                Double[] pLocation = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot_OtherAng();
                Int16[] pbmrkLoc = { 40, 81, 82, 83 };
                for (int i = 0; i < pLocation.Length; i++)
                {
                    //pstrLocation = pstrLocation + Convert.ToString(modMain.ConvDoubleToStr(pLocation[i], "#0") + pstrChar);
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[pbmrkLoc[i]]).Range.Text
                    pWordDoc.FormFields[objBookMarksName[pbmrkLoc[i]]].Result = Convert.ToString(modMain.ConvDoubleToStr(pLocation[i], "#0"));
                }

                //if (pstrLocation != "")
                //{
                //    int pLastIndx = pstrLocation.Length - 2;
                //    pWordDoc.Bookmarks.get_Item(ref objBookMarksName[40]).Range.Text = pstrLocation.Remove(pLastIndx) + Convert.ToChar(176) +
                //                                                                        " from Casing Split Line";
                //}

                Double pLeadingEnd, pTrailingEnd;

                //BG 21JUN11
                //PB 09AUG11. To be replaced with Bearing_In.Pad_RBack [1) ==> Leading & Bearing_In.Pad_RBack [2) ==> Trailing.
                pLeadingEnd = (Convert.ToDouble((((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] +
                              ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1]) / 4) +
                              ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead);
                pTrailingEnd = (Convert.ToDouble((((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[0] +
                               ((clsBearing_Radial_FP)Project_In.Product.Bearing).DSet_Range[1]) / 4) +
                               ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail);

                //AES 11AUG14   Not needed.
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[41]).Range.Text =
                //                     modMain.ConvDoubleToStr(pLeadingEnd, "#0.000"); //Back Radius Leading End
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[42]).Range.Text =
                //                     modMain.ConvDoubleToStr(pTrailingEnd, "#0.000");//Back Radius Trailing End

                pWordDoc.FormFields[objBookMarksName[43]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead, "#0.000");
                pWordDoc.FormFields[objBookMarksName[44]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot, "#0.000");
                pWordDoc.FormFields[objBookMarksName[45]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail, "#0.000");



                //....Front
                if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                    modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                    //   Seal related bookmark:
                    //   ----------------------
                    //BG 26MAR13
                    pWordDoc.FormFields[objBookMarksName[46]].Result =
                                         ((clsSeal)modMain.gProject.Product.EndConfig[0]).Design.ToString();

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000") + "\n(" +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000") + ")";

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000") + "(" +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000") + ")-Front";

                    pWordDoc.FormFields[objBookMarksName[47]].Result =
                                      modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[85]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[86]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[87]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000");

                    //clsSeal.sBlade pBlade = Seal_In.Blade;

                    pWordDoc.FormFields[objBookMarksName[48]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.Count);

                    pWordDoc.FormFields[objBookMarksName[49]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.T, "#0.000");

                    pWordDoc.FormFields[objBookMarksName[50]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.D(), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[94]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.Count);

                }
                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    //   Seal related bookmark:
                    //   ----------------------
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[51]).Range.Text =
                    //                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design.ToString();
                    pWordDoc.FormFields[objBookMarksName[84]].Result =
                                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design.ToString();

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[52]).Range.Text =
                    //                 modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000") + " / " +
                    //                 modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000") + "\n(" +
                    //                 modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000") + " / " +
                    //                 modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000") + ")";

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                    //                  modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000") + " / " +
                    //                  modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000") + "(" +
                    //                  modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000") + " / " +
                    //                  modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000") + ")-Back";

                    pWordDoc.FormFields[objBookMarksName[88]].Result =
                                        modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[89]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[90]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[91]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000");

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[53]).Range.Text =
                    //                   Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count);

                    //BG 26MAR13
                    pWordDoc.FormFields[objBookMarksName[92]].Result =
                                     Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count);

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[54]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T, "#0.000");

                    //BG 26MAR13
                    pWordDoc.FormFields[objBookMarksName[93]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T, "#0.000");

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[55]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.D(), "#0.000");  

                    pWordDoc.FormFields[objBookMarksName[95]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.Count);
                    pWordDoc.FormFields[objBookMarksName[96]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.D(), "#0.000");
                }
                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Seal &&
                        modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Seal)
                {
                    //   Front Seal related bookmark:
                    //   ----------------------
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[46]).Range.Text =
                    //                     ((clsSeal)modMain.gProject.Product.EndConfig[0]).Design.ToString() + "-Front" + "\n" +
                    //                     ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design.ToString() + "-Back";

                    pWordDoc.FormFields[objBookMarksName[46]].Result =
                                        ((clsSeal)modMain.gProject.Product.EndConfig[0]).Design.ToString();

                    pWordDoc.FormFields[objBookMarksName[84]].Result =
                                        ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design.ToString();

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000") + "\n(" +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000") + " / " +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000") + ")";

                    //BG 26MAR13
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000") + "/" +
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000") + "(" +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000") + "/" +
                    //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000") + ")-Front\n" +
                    //                    modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000") + "/" +
                    //                  modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000") + "(" +
                    //                  modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000") + "/" +
                    //                  modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000") + ")-Back"; 

                    //AES 11AUG14
                    pWordDoc.FormFields[objBookMarksName[47]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[85]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[86]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[0]), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[87]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[0]).DBore_Range[1]), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[88]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[89]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000");
                    pWordDoc.FormFields[objBookMarksName[90]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[91]].Result =
                                        modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000");


                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[48]).Range.Text =
                    //                     Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.Count) + "-Front\n" +
                    //                     Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count) + "-Back";
                    pWordDoc.FormFields[objBookMarksName[48]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.Count);
                    pWordDoc.FormFields[objBookMarksName[92]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count);

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[49]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.T, "#0.000") + "-Front\n" +
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T, "#0.000") + "-Back"  ;

                    pWordDoc.FormFields[objBookMarksName[49]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).Blade.T, "#0.000");
                    pWordDoc.FormFields[objBookMarksName[93]].Result =
                                       modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T, "#0.000");

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[50]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.D(), "#0.000")+ "-Front\n" +
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.D(), "#0.000") + "-Back";

                    pWordDoc.FormFields[objBookMarksName[94]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.Count);
                    pWordDoc.FormFields[objBookMarksName[50]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[0]).DrainHoles.D(), "#0.000");
                    pWordDoc.FormFields[objBookMarksName[95]].Result =
                                         Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.Count);
                    pWordDoc.FormFields[objBookMarksName[96]].Result =
                                         modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.D(), "#0.000");

                    //BG 26MAR13
                    //   Back Seal related bookmark:
                    //   ----------------------
                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[51]).Range.Text =
                    //                    ((clsSeal)modMain.gProject.Product.EndConfig[1]).Design.ToString();

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[52]).Range.Text =
                    //                 modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0], "#0.000") + " / " +
                    //                 modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1], "#0.000") + "\n(" +
                    //                 modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[0]), "#0.000") + " / " +
                    //                 modMain.ConvDoubleToStr(Unit_In.CEng_Met(((clsSeal)modMain.gProject.Product.EndConfig[1]).DBore_Range[1]), "#0.000") + ")";

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[53]).Range.Text =
                    //                   Convert.ToString(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.Count);

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[54]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).Blade.T, "#0.000");

                    //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[55]).Range.Text =
                    //                     modMain.ConvDoubleToStr(((clsSeal)modMain.gProject.Product.EndConfig[1]).DrainHoles.D(), "#0.000");
                }
                else if (modMain.gProject.Product.EndConfig[0].Type == clsEndConfig.eType.Thrust_Bearing_TL &&
                         modMain.gProject.Product.EndConfig[1].Type == clsEndConfig.eType.Thrust_Bearing_TL)
                {
                }


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                //                    modMain.ConvDoubleToStr(Seal_In.BoreD_Range[0], "#0.000") + " / " +
                //                    modMain.ConvDoubleToStr(Seal_In.BoreD_Range[1], "#0.000") + "\n(" +
                //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(Seal_In.BoreD_Range[0]), "#0.000") + " / " +
                //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(Seal_In.BoreD_Range[1]), "#0.000") + ")";


                //BG 21NOV11
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[46]).Range.Text =
                //                     Seal_In.Type.ToString();




                ////BG 30JUN11      
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[47]).Range.Text =
                //                    modMain.ConvDoubleToStr(Seal_In.BoreD_Range[0], "#0.000") + " / " +
                //                    modMain.ConvDoubleToStr(Seal_In.BoreD_Range[1], "#0.000") + "\n(" +
                //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(Seal_In.BoreD_Range[0]), "#0.000") + " / " +
                //                    modMain.ConvDoubleToStr(Unit_In.CEng_Met(Seal_In.BoreD_Range[1]), "#0.000") + ")";


                //clsSeal.sBlade pBlade = Seal_In.Blade;


                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[48]).Range.Text =
                //                     Convert.ToString(pBlade.Count);

                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[49]).Range.Text =
                //                     modMain.ConvDoubleToStr(modMain.gEndSeal[0].Blade.T, "#0.000");




                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[50]).Range.Text =
                //                     modMain.ConvDoubleToStr(modMain.gEndSeal[0].DrainHole.D, "#0.000");   //SB 10JUL09



                //   Performance related bookmark:
                //   -----------------------------

                //BG 26MAR13
                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[51]).Range.Text =
                //                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP)
                //                     + " (" + Unit_In.CFac_Power_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP).ToString("#0.0") + " kW)";            //SB 03NOV09        

                //AES 12AUG14
                pWordDoc.FormFields[objBookMarksName[51]].Result =
                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP);
                pWordDoc.FormFields[objBookMarksName[97]].Result =
                                    Unit_In.CFac_Power_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power_HP).ToString("#0.0");



                //pWordDoc.Bookmarks.get_Item(ref objBookMarksName[52]).Range.Text =
                //                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm)
                //                     + " (" + Unit_In.CFac_GPM_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm).ToString("#0.0") + " LPM)";

                //AES 12AUG14
                pWordDoc.FormFields[objBookMarksName[52]].Result =
                                     Convert.ToString(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm);
                pWordDoc.FormFields[objBookMarksName[98]].Result =
                                   Unit_In.CFac_GPM_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd_gpm).ToString("#0.0");


                //if (((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F != 0.00)
                //    pWordDoc.Bookmarks.get_Item(ref objBookMarksName[53]).Range.Text = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F.ToString("#0")
                //                         + " (" + Unit_In.CFac_TRise_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F).ToString("#0") + " " +
                //                         Convert.ToChar(176) + "C)";

                if (((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F != 0.00)
                {
                    pWordDoc.FormFields[objBookMarksName[53]].Result = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F.ToString("#0");
                    pWordDoc.FormFields[objBookMarksName[99]].Result = Unit_In.CFac_TRise_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise_F).ToString("#0");
                }



                pWordDoc.FormFields[objBookMarksName[54]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TFilm_Min, "#0.0000000");
                pWordDoc.FormFields[objBookMarksName[55]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Temp, "#0");                             //SB 03NOV09  
                pWordDoc.FormFields[objBookMarksName[56]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Press, "#0");                                //SB 03NOV09  
                pWordDoc.FormFields[objBookMarksName[57]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Rot, "");
                pWordDoc.FormFields[objBookMarksName[58]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Load, "#0");                                 //SB 03NOV09  
                pWordDoc.FormFields[objBookMarksName[59]].Result =
                                     modMain.ConvDoubleToStr(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.PadMax.Stress, "#0");                               //SB 03NOV09  

               
                //////   Set Line Rotation           
                //////   -----------------
                ////object pLine = "Line 7";

                //////....Original Top, Left & Height of Line.
                ////Double pOrgTop = 0.0F;
                ////Double pOrgLeft = 0.0F;
                ////Double pOrgHeight = 0.0F;

                //////....Constant Value taken as radius.
                ////Double pcRad = 0.0F;
                ////pcRad = 30.0F;

                //////....Height, Width, Top & Left.
                ////Double pHeight = 0.0F;
                ////Double pWidth = 0.0F;
                ////Double pTop = 0.0F;
                ////Double pLeft = 0.0F;

                ////Word.Shape pShape = pWordDoc.Shapes.get_Item(ref pLine);

                ////pOrgTop = pShape.Top;
                ////pOrgHeight = pShape.Height;
                ////pOrgTop = pOrgTop + pOrgHeight;
                ////pOrgLeft = pShape.Left;

                //////....Rotate Vector Angle 0 to 90.
                ////if (OpCond_In.Radial_LoadAng >= 0.0F && OpCond_In.Radial_LoadAng <= 90.0F)
                ////{
                ////    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pTop = pOrgTop - pHeight;
                ////    pLeft = pOrgLeft;

                ////    pShape.Select(ref pMissing);

                ////    pShape.Width = (Single)pWidth;
                ////    pShape.Height = (Single)pHeight;
                ////    pShape.Top = (Single)pTop;
                ////    pShape.Left = (Single)pLeft;

                ////}

                //////....Rotate Vector Angle 90 to 180.
                ////else if (OpCond_In.Radial_LoadAng >= 90.0F && OpCond_In.Radial_LoadAng <= 180.0F)
                ////{
                ////    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pTop = pOrgTop - pHeight;
                ////    pLeft = pOrgLeft + pWidth;

                ////    pShape.Select(ref pMissing);
                ////    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipHorizontal);

                ////    pShape.Width = Convert.ToSingle(-pWidth);
                ////    pShape.Height = (Single)pHeight;
                ////    pShape.Top = (Single)pTop;
                ////    pShape.Left = (Single)pLeft;

                ////}

                //////....Rotate Vector Angle 180 to 270.
                ////else if (OpCond_In.Radial_LoadAng >= 180.0F && OpCond_In.Radial_LoadAng <= 270.0F)
                ////{
                ////    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pTop = pOrgTop;
                ////    pLeft = pOrgLeft + pWidth;

                ////    pShape.Select(ref pMissing);

                ////    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipHorizontal);
                ////    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipVertical);

                ////    pShape.Width = Convert.ToSingle(-pWidth);
                ////    pShape.Height = Convert.ToSingle(-pHeight);
                ////    pShape.Top = (Single)pTop;
                ////    pShape.Left = (Single)pLeft;

                ////}

                //////....Rotate Vector Angle 270 to 360.
                ////else if (OpCond_In.Radial_LoadAng >= 270.0F && OpCond_In.Radial_LoadAng <= 360.0F)
                ////{
                ////    pHeight = (Double)(pcRad * Math.Sin((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pWidth = (Double)(pcRad * Math.Cos((Double)(OpCond_In.Radial_LoadAng * (Math.PI / 180.0F))));
                ////    pTop = pOrgTop;
                ////    pLeft = pOrgLeft;

                ////    pShape.Select(ref pMissing);
                ////    pShape.Flip(Microsoft.Office.Core.MsoFlipCmd.msoFlipVertical);

                ////    pShape.Width = (Single)pWidth;
                ////    pShape.Height = Convert.ToSingle(-pHeight);
                ////    pShape.Top = (Single)pTop;
                ////    pShape.Left = (Single)pLeft;

                ////}

                //pWordDoc.Protect(Word.WdProtectionType.wdAllowOnlyFormFields);

                pWordApp.ChangeFileOpenDirectory(ProjectPath_In);

                if (File.Exists(ProjectPath_In + "\\" + pDocName + ".Docx"))
                    File.Delete(ProjectPath_In + "\\" + pDocName + ".Docx");

               
                pWordDoc.SaveAs(ref pDocName, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
                                ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
                                ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing);
               
                //pWordDoc.SaveAs2(ref pDocName, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
                //                ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
                //                ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing);

                
                pWordApp.Visible = true;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Please check your directory path or data entry.",
                                                     "Error", System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);

            }
            finally
            {
                
            }
           
        }

        private void CloseWordFiles()
        //=============================       
        {
            Process[] pProcesses = Process.GetProcesses();

            try
            {
                foreach (Process p in pProcesses)
                    if (p.ProcessName == "WINWORD")
                        p.Kill();
            }
            catch (Exception pEXP)
            {

            }
        }

        //Not used now BG 04JUN09
        public void WriteSummaryInformation(clsProject Project_In, clsOpCond OpCond_In,
                               clsBearing_Radial_FP Bearing_In, clsMaterial Mat_In,clsSeal Seal_In, 
                               clsUnit Unit_In, clsFiles Files_In, clsDB DB_In, clsScrew Screw_In,
                               string ProjectPath_In)
        //========================================================================      
        {

            //Object pInfoSheetName = Files_In.FileTitle_Template_SIS;        //SB 24JUL09
            //Object pMissing = Missing.Value;


            //if (!Directory.Exists(ProjectPath_In))                       
            //    Directory.CreateDirectory(ProjectPath_In);          


            //Object pDocName = Project_In.No + "_SI";

            //Word.Application pWordApp = new Word.Application();
            //Word.Document pWordDoc = null;

            //try
            //{
            //    pWordDoc = pWordApp.Documents.Add(ref pInfoSheetName, ref pMissing, ref pMissing, ref pMissing);

            //    //   Project related bookmark:
            //    //   -------------------------

            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[0]).Range.Text = Project_In.No;
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[1]).Range.Text = Project_In.Customer.Name;
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[2]).Range.Text = Project_In.AssyDwg.No;
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[3]).Range.Text = Unit_In.System.ToString();

            //    //....Bearing
            //    //........Split Line Haradware
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[4]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.SplitLine_Thread_L_LowerLimit(),"#0.000");
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[5]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.SplitLine_Pin_L_LowerLimit(), "#0.000");

            //    //........Seal Fixture Selection
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[6]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.Calc_DBC_Ballpark(), "#0.000");     //SB 05JUN09    //BG 28JUN11
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[7]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.EndConfig_DO_Max(), "#0.000");   //BG 24JUN11

            //    //........Thread Lengths Limit
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[8]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.MountFixture_Screw_L_LowerLimit(Seal_In, Screw_In), "#0.000");
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[9]).Range.Text = modMain.ConvDoubleToStr(Bearing_In.MountFixture_Screw_L_UpperLimit(Screw_In), "#0.000");      

            //    //....Seal
            //    //........Clearance
            //    //pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[10]).Range.Text = modMain.ConvDoubleToStr(Seal_In.Clearence(), "#0.000");  //SB 02JUL09   
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[10]).Range.Text = modMain.ConvDoubleToStr(Seal_In.Clearance(), "#0.000");  //SB 02JUL09    //BG 21NOV11

                                               

            //    //........Counter Bore Depth Limits     
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[11]).Range.Text = modMain.ConvDoubleToStr(Seal_In.MountHole_CBore_Depth_LowerLimit(), "#0.000");
            //    pWordDoc.Bookmarks.get_Item(ref objBookMarksSummaryInfo[12]).Range.Text = modMain.ConvDoubleToStr(Seal_In.MountHole_CBore_Depth_UpperLimit(), "#0.000");

               
             
            //    pWordApp.ChangeFileOpenDirectory(ProjectPath_In);        

            //    if (File.Exists(ProjectPath_In + "\\" + pDocName + ".Doc"))
            //        File.Delete(ProjectPath_In + "\\" + pDocName + ".Doc");

            //    pWordDoc.SaveAs(ref pDocName, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
            //                    ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing,
            //                    ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing, ref pMissing);

            //    pWordApp.Visible = true;                                     

            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please check your directory path or data entry.",
            //                                        "Error", System.Windows.Forms.MessageBoxButtons.OK,
            //                                        System.Windows.Forms.MessageBoxIcon.Error);

            //}
            //finally
            //{

            //}

        }
    }
}
