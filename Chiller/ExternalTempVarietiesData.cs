using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    public partial class ExternalTempVarietiesData : Form
    {
        Timer tm;
        public ExternalTempVarietiesData()
        {
            InitializeComponent();
        }

        private void InternalColdControlChart_Load(object sender, EventArgs e)
        {
            this.ListComBox.Items.Clear();
            string[] list = Function.Regedit.Get_Book_List(@"SOFTWARE\JHT\TriTempChiller\","ExternalTempVarieties");
            if (list != null && list.Length > 0)
            {
                this.ListComBox.Items.AddRange(list);
                if (this.ListComBox.Items.Contains("Handler") == false)
                {
                    this.ListComBox.Items.Add("Handler");
                }
            }

            ChangeTempVarietiesName(Flag.StartEnabled.VarietiesName);

            tm = new Timer();
            tm.Interval = 300;
            tm.Tick += Tm_Tick;
            tm.Start();
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            Color c = new Color();

            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                if (i % 2 == 0)
                {
                    HslControls.HslLanternSimple tb = Function.Other.FindControl(this.LeftTestArm, "TestArm" + (i + 1).ToString() + "_Led") as HslControls.HslLanternSimple;
                    if (tb != null)
                    {
                        Function.Other.SetColor(Flag.TestArm[i].Heat_IC.SetToHeat.Enabled || Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled, Color.LimeGreen, Color.Gray, ref c);
                        tb.LanternBackground = c;
                    }
                }
                else
                {
                    HslControls.HslLanternSimple tb = Function.Other.FindControl(this.RightTestArm, "TestArm" + (i + 1).ToString() + "_Led") as HslControls.HslLanternSimple;
                    if (tb != null)
                    {
                        Function.Other.SetColor(Flag.TestArm[i].Heat_IC.SetToHeat.Enabled || Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled, Color.LimeGreen, Color.Gray, ref c);
                        tb.LanternBackground = c;
                    }
                }
            }
       
            for (int i = 0; i < Flag.ColdPlate.Length; i++)
            {
                HslControls.HslLanternSimple tb = Function.Other.FindControl(this.AllColdPlate, "ColdPlate" + (i + 1).ToString() + "_Led") as HslControls.HslLanternSimple;
                if (tb != null)
                {
                    Function.Other.SetColor(Flag.ColdPlate[i].Heat.SetToHeat.Enabled || Flag.ColdPlate[i].Heat.SetToHeat.Enabled, Color.LimeGreen, Color.Gray, ref c);
                    tb.LanternBackground = c;
                }
            }

            this.LeftMotorSpeed.Text = "Motor Speed：" + Flag.ExternalPump[2].Speed.ToString() + "r/min";
            this.PlateMotorSpeed.Text = "Motor Speed：" + Flag.ExternalPump[1].Speed.ToString() + "r/min";
            this.RightMotorSpeed.Text = "Motor Speed：" + Flag.ExternalPump[0].Speed.ToString() + "r/min";

            this.LeftMotorLoad.Text = "Motor Load：" + Flag.ExternalPump[2].OperatingLoad.ToString() + "％";
            this.PlateMotorLoad.Text = "Motor Load：" + Flag.ExternalPump[1].OperatingLoad.ToString() + "％";
            this.RightMotorLoad.Text = "Motor Load：" + Flag.ExternalPump[0].OperatingLoad.ToString() + "％";
        }

        private void RefreshTestOffsetPointState(int num, bool IsAtc)
        {
            #region 切换了加热模式
            if (IsAtc == true)
            {
                this.IsPreCooling.Enabled = true;
                this.IsResultHeat.Enabled = true;
                this.BorderHeat.Enabled = true;

                this.PreCoolTemp.Enabled = this.IsPreCooling.Checked;
                this.ResultHeatTemp.Enabled = this.IsResultHeat.Checked;
                this.BorderTemp.Enabled = this.BorderHeat.Checked;
            }
            else
            {
                this.PreCoolTemp.Enabled = false;
                this.ResultHeatTemp.Enabled = false;
                this.BorderTemp.Enabled = false;

                this.IsPreCooling.Enabled = false;
                this.IsResultHeat.Enabled = false;
                this.BorderHeat.Enabled = false;
            }
            this.TestTemp.Enabled = true;
            this.TempRange.Enabled = true;

            this.ChillerTemp.Enabled = IsAtc;
            this.ChillerRange.Enabled = IsAtc;

            #endregion

            #region 使用几点校准方式

            if (num == 0)
            {
                #region 1点
                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = false; }
                }

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = false; }
                }

                if (IsAtc == true)
                {
                    this.ColdPlate1Offset1.Enabled = true;
                    this.ColdPlate1Offset2.Enabled = false;
                    this.ColdPlate1Offset3.Enabled = false;

                    this.ColdPlate2Offset1.Enabled = true;
                    this.ColdPlate2Offset2.Enabled = false;
                    this.ColdPlate2Offset3.Enabled = false;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = false;
                    this.HotPlate1Offset3.Enabled = false;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = false;
                    this.HotPlate2Offset3.Enabled = false;
                }
                else
                {
                    this.ColdPlate1Offset1.Enabled = false;
                    this.ColdPlate1Offset2.Enabled = false;
                    this.ColdPlate1Offset3.Enabled = false;

                    this.ColdPlate2Offset1.Enabled = false;
                    this.ColdPlate2Offset2.Enabled = false;
                    this.ColdPlate2Offset3.Enabled = false;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = false;
                    this.HotPlate1Offset3.Enabled = false;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = false;
                    this.HotPlate2Offset3.Enabled = false;
                }

                this.TestBasePoint1.Enabled = false;
                this.TestBasePoint2.Enabled = false;
                this.TestBasePoint3.Enabled = false;

                #endregion 
            }
            else if (num == 1)
            {
                #region 2点
                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = true; }
                }

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = true; }
                }

                if (IsAtc == true)
                {
                    this.ColdPlate1Offset1.Enabled = true;
                    this.ColdPlate1Offset2.Enabled = false;
                    this.ColdPlate1Offset3.Enabled = true;

                    this.ColdPlate2Offset1.Enabled = true;
                    this.ColdPlate2Offset2.Enabled = false;
                    this.ColdPlate2Offset3.Enabled = true;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = false;
                    this.HotPlate1Offset3.Enabled = true;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = false;
                    this.HotPlate2Offset3.Enabled = true;
                }
                else
                {
                    this.ColdPlate1Offset1.Enabled = false;
                    this.ColdPlate1Offset2.Enabled = false;
                    this.ColdPlate1Offset3.Enabled = false;

                    this.ColdPlate2Offset1.Enabled = false;
                    this.ColdPlate2Offset2.Enabled = false;
                    this.ColdPlate2Offset3.Enabled = false;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = false;
                    this.HotPlate1Offset3.Enabled = true;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = false;
                    this.HotPlate2Offset3.Enabled = true;
                }

                this.TestBasePoint1.Enabled = true;
                this.TestBasePoint2.Enabled = false;
                this.TestBasePoint3.Enabled = true;
                #endregion
            }
            else if (num == 2)
            {
                #region 3点
                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = true; }
                    if (tb3 != null) { tb3.Enabled = true; }
                }

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = true; }
                    if (tb2 != null) { tb2.Enabled = true; }
                    if (tb3 != null) { tb3.Enabled = true; }
                }

                if (IsAtc == true)
                {
                    this.ColdPlate1Offset1.Enabled = true;
                    this.ColdPlate1Offset2.Enabled = true;
                    this.ColdPlate1Offset3.Enabled = true;

                    this.ColdPlate2Offset1.Enabled = true;
                    this.ColdPlate2Offset2.Enabled = true;
                    this.ColdPlate2Offset3.Enabled = true;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = true;
                    this.HotPlate1Offset3.Enabled = true;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = true;
                    this.HotPlate2Offset3.Enabled = true;
                }
                else
                {
                    this.ColdPlate1Offset1.Enabled = false;
                    this.ColdPlate1Offset2.Enabled = false;
                    this.ColdPlate1Offset3.Enabled = false;

                    this.ColdPlate2Offset1.Enabled = false;
                    this.ColdPlate2Offset2.Enabled = false;
                    this.ColdPlate2Offset3.Enabled = false;

                    this.HotPlate1Offset1.Enabled = true;
                    this.HotPlate1Offset2.Enabled = true;
                    this.HotPlate1Offset3.Enabled = true;

                    this.HotPlate2Offset1.Enabled = true;
                    this.HotPlate2Offset2.Enabled = true;
                    this.HotPlate2Offset3.Enabled = true;
                }

                this.TestBasePoint1.Enabled = true;
                this.TestBasePoint2.Enabled = true;
                this.TestBasePoint3.Enabled = true;
                #endregion
            }
            else
            {
                #region 未知点
                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_InsideOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = false; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = false; }
                }

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    TextBox tb1 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset1") as TextBox;
                    TextBox tb2 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset2") as TextBox;
                    TextBox tb3 = Function.Other.FindControl(this, "TestArm" + (i + 1).ToString() + "_IcOffset3") as TextBox;
                    if (tb1 != null) { tb1.Enabled = false; }
                    if (tb2 != null) { tb2.Enabled = false; }
                    if (tb3 != null) { tb3.Enabled = false; }
                }

                this.ColdPlate1Offset1.Enabled = false;
                this.ColdPlate1Offset2.Enabled = false;
                this.ColdPlate1Offset3.Enabled = false;

                this.ColdPlate2Offset1.Enabled = false;
                this.ColdPlate2Offset2.Enabled = false;
                this.ColdPlate2Offset3.Enabled = false;

                this.HotPlate1Offset1.Enabled = false;
                this.HotPlate1Offset2.Enabled = false;
                this.HotPlate1Offset3.Enabled = false;

                this.HotPlate2Offset1.Enabled = false;
                this.HotPlate2Offset2.Enabled = false;
                this.HotPlate2Offset3.Enabled = false;

                this.TestBasePoint1.Enabled = false;
                this.TestBasePoint2.Enabled = false;
                this.TestBasePoint3.Enabled = false;
                #endregion
            }

            #endregion

            TestOffsetChange();
        }

        private void RefreshBorderOffsetPointState(int num, bool IsAtc)
        {
            this.BorderTestArmOffset.Enabled = IsAtc;
            this.BorderSocket.Enabled = IsAtc;
            this.BorderColdPlate.Enabled = IsAtc;
            this.BorderOffsetPoint.Enabled = IsAtc;
            this.BorderBaseOffsetPoint.Enabled = IsAtc;

            if (num == 0)
            {
                #region 1点
                this.BorderTestArm1Offset1.Enabled = true;
                this.BorderTestArm1Offset2.Enabled = false;
                this.BorderTestArm1Offset3.Enabled = false;

                this.BorderTestArm2Offset1.Enabled = true;
                this.BorderTestArm2Offset2.Enabled = false;
                this.BorderTestArm2Offset3.Enabled = false;

                this.BorderTestArm3Offset1.Enabled = true;
                this.BorderTestArm3Offset2.Enabled = false;
                this.BorderTestArm3Offset3.Enabled = false;

                this.BorderTestArm4Offset1.Enabled = true;
                this.BorderTestArm4Offset2.Enabled = false;
                this.BorderTestArm4Offset3.Enabled = false;

                this.BorderTestArm5Offset1.Enabled = true;
                this.BorderTestArm5Offset2.Enabled = false;
                this.BorderTestArm5Offset3.Enabled = false;

                this.BorderTestArm6Offset1.Enabled = true;
                this.BorderTestArm6Offset2.Enabled = false;
                this.BorderTestArm6Offset3.Enabled = false;

                this.BorderTestArm7Offset1.Enabled = true;
                this.BorderTestArm7Offset2.Enabled = false;
                this.BorderTestArm7Offset3.Enabled = false;

                this.BorderTestArm8Offset1.Enabled = true;
                this.BorderTestArm8Offset2.Enabled = false;
                this.BorderTestArm8Offset3.Enabled = false;

                this.BorderColdPlate1Offset1.Enabled = true;
                this.BorderColdPlate1Offset2.Enabled = false;
                this.BorderColdPlate1Offset3.Enabled = false;

                this.BorderColdPlate2Offset1.Enabled = true;
                this.BorderColdPlate2Offset2.Enabled = false;
                this.BorderColdPlate2Offset3.Enabled = false;

                this.BorderSocket1Offset1.Enabled = true;
                this.BorderSocket1Offset2.Enabled = false;
                this.BorderSocket1Offset3.Enabled = false;

                this.BorderSocket2Offset1.Enabled = true;
                this.BorderSocket2Offset2.Enabled = false;
                this.BorderSocket2Offset3.Enabled = false;

                this.BorderSocket3Offset1.Enabled = true;
                this.BorderSocket3Offset2.Enabled = false;
                this.BorderSocket3Offset3.Enabled = false;

                this.BorderSocket4Offset1.Enabled = true;
                this.BorderSocket4Offset2.Enabled = false;
                this.BorderSocket4Offset3.Enabled = false;

                this.BorderSocket5Offset1.Enabled = true;
                this.BorderSocket5Offset2.Enabled = false;
                this.BorderSocket5Offset3.Enabled = false;

                this.BorderSocket6Offset1.Enabled = true;
                this.BorderSocket6Offset2.Enabled = false;
                this.BorderSocket6Offset3.Enabled = false;

                this.BorderSocket7Offset1.Enabled = true;
                this.BorderSocket7Offset2.Enabled = false;
                this.BorderSocket7Offset3.Enabled = false;

                this.BorderSocket8Offset1.Enabled = true;
                this.BorderSocket8Offset2.Enabled = false;
                this.BorderSocket8Offset3.Enabled = false;

                this.BorderBasePoint1.Enabled = false;
                this.BorderBasePoint2.Enabled = false;
                this.BorderBasePoint3.Enabled = false;

                #endregion
            }
            else if (num == 1)
            {
                #region 2点
                this.BorderTestArm1Offset1.Enabled = true;
                this.BorderTestArm1Offset2.Enabled = false;
                this.BorderTestArm1Offset3.Enabled = true;

                this.BorderTestArm2Offset1.Enabled = true;
                this.BorderTestArm2Offset2.Enabled = false;
                this.BorderTestArm2Offset3.Enabled = true;

                this.BorderTestArm3Offset1.Enabled = true;
                this.BorderTestArm3Offset2.Enabled = false;
                this.BorderTestArm3Offset3.Enabled = true;

                this.BorderTestArm4Offset1.Enabled = true;
                this.BorderTestArm4Offset2.Enabled = false;
                this.BorderTestArm4Offset3.Enabled = true;

                this.BorderTestArm5Offset1.Enabled = true;
                this.BorderTestArm5Offset2.Enabled = false;
                this.BorderTestArm5Offset3.Enabled = true;

                this.BorderTestArm6Offset1.Enabled = true;
                this.BorderTestArm6Offset2.Enabled = false;
                this.BorderTestArm6Offset3.Enabled = true;

                this.BorderTestArm7Offset1.Enabled = true;
                this.BorderTestArm7Offset2.Enabled = false;
                this.BorderTestArm7Offset3.Enabled = true;

                this.BorderTestArm8Offset1.Enabled = true;
                this.BorderTestArm8Offset2.Enabled = false;
                this.BorderTestArm8Offset3.Enabled = true;

                this.BorderColdPlate1Offset1.Enabled = true;
                this.BorderColdPlate1Offset2.Enabled = false;
                this.BorderColdPlate1Offset3.Enabled = true;

                this.BorderColdPlate2Offset1.Enabled = true;
                this.BorderColdPlate2Offset2.Enabled = false;
                this.BorderColdPlate2Offset3.Enabled = true;

                this.BorderSocket1Offset1.Enabled = true;
                this.BorderSocket1Offset2.Enabled = false;
                this.BorderSocket1Offset3.Enabled = true;

                this.BorderSocket2Offset1.Enabled = true;
                this.BorderSocket2Offset2.Enabled = false;
                this.BorderSocket2Offset3.Enabled = true;

                this.BorderSocket3Offset1.Enabled = true;
                this.BorderSocket3Offset2.Enabled = false;
                this.BorderSocket3Offset3.Enabled = true;

                this.BorderSocket4Offset1.Enabled = true;
                this.BorderSocket4Offset2.Enabled = false;
                this.BorderSocket4Offset3.Enabled = true;

                this.BorderSocket5Offset1.Enabled = true;
                this.BorderSocket5Offset2.Enabled = false;
                this.BorderSocket5Offset3.Enabled = true;

                this.BorderSocket6Offset1.Enabled = true;
                this.BorderSocket6Offset2.Enabled = false;
                this.BorderSocket6Offset3.Enabled = true;

                this.BorderSocket7Offset1.Enabled = true;
                this.BorderSocket7Offset2.Enabled = false;
                this.BorderSocket7Offset3.Enabled = true;

                this.BorderSocket8Offset1.Enabled = true;
                this.BorderSocket8Offset2.Enabled = false;
                this.BorderSocket8Offset3.Enabled = true;

                this.BorderBasePoint1.Enabled = true;
                this.BorderBasePoint2.Enabled = false;
                this.BorderBasePoint3.Enabled = true;

                #endregion
            }
            else if (num == 2)
            {
                #region 3点
                this.BorderTestArm1Offset1.Enabled = true;
                this.BorderTestArm1Offset2.Enabled = true;
                this.BorderTestArm1Offset3.Enabled = true;

                this.BorderTestArm2Offset1.Enabled = true;
                this.BorderTestArm2Offset2.Enabled = true;
                this.BorderTestArm2Offset3.Enabled = true;

                this.BorderTestArm3Offset1.Enabled = true;
                this.BorderTestArm3Offset2.Enabled = true;
                this.BorderTestArm3Offset3.Enabled = true;

                this.BorderTestArm4Offset1.Enabled = true;
                this.BorderTestArm4Offset2.Enabled = true;
                this.BorderTestArm4Offset3.Enabled = true;

                this.BorderTestArm5Offset1.Enabled = true;
                this.BorderTestArm5Offset2.Enabled = true;
                this.BorderTestArm5Offset3.Enabled = true;

                this.BorderTestArm6Offset1.Enabled = true;
                this.BorderTestArm6Offset2.Enabled = true;
                this.BorderTestArm6Offset3.Enabled = true;

                this.BorderTestArm7Offset1.Enabled = true;
                this.BorderTestArm7Offset2.Enabled = true;
                this.BorderTestArm7Offset3.Enabled = true;

                this.BorderTestArm8Offset1.Enabled = true;
                this.BorderTestArm8Offset2.Enabled = true;
                this.BorderTestArm8Offset3.Enabled = true;

                this.BorderColdPlate1Offset1.Enabled = true;
                this.BorderColdPlate1Offset2.Enabled = true;
                this.BorderColdPlate1Offset3.Enabled = true;

                this.BorderColdPlate2Offset1.Enabled = true;
                this.BorderColdPlate2Offset2.Enabled = true;
                this.BorderColdPlate2Offset3.Enabled = true;

                this.BorderSocket1Offset1.Enabled = true;
                this.BorderSocket1Offset2.Enabled = true;
                this.BorderSocket1Offset3.Enabled = true;

                this.BorderSocket2Offset1.Enabled = true;
                this.BorderSocket2Offset2.Enabled = true;
                this.BorderSocket2Offset3.Enabled = true;

                this.BorderSocket3Offset1.Enabled = true;
                this.BorderSocket3Offset2.Enabled = true;
                this.BorderSocket3Offset3.Enabled = true;

                this.BorderSocket4Offset1.Enabled = true;
                this.BorderSocket4Offset2.Enabled = true;
                this.BorderSocket4Offset3.Enabled = true;

                this.BorderSocket5Offset1.Enabled = true;
                this.BorderSocket5Offset2.Enabled = true;
                this.BorderSocket5Offset3.Enabled = true;

                this.BorderSocket6Offset1.Enabled = true;
                this.BorderSocket6Offset2.Enabled = true;
                this.BorderSocket6Offset3.Enabled = true;

                this.BorderSocket7Offset1.Enabled = true;
                this.BorderSocket7Offset2.Enabled = true;
                this.BorderSocket7Offset3.Enabled = true;

                this.BorderSocket8Offset1.Enabled = true;
                this.BorderSocket8Offset2.Enabled = true;
                this.BorderSocket8Offset3.Enabled = true;

                this.BorderBasePoint1.Enabled = true;
                this.BorderBasePoint2.Enabled = true;
                this.BorderBasePoint3.Enabled = true;

                #endregion
            }
            else
            {
                #region 未知

                this.BorderTestArm1Offset1.Enabled = false;
                this.BorderTestArm1Offset2.Enabled = false;
                this.BorderTestArm1Offset3.Enabled = false;

                this.BorderTestArm2Offset1.Enabled = false;
                this.BorderTestArm2Offset2.Enabled = false;
                this.BorderTestArm2Offset3.Enabled = false;

                this.BorderTestArm3Offset1.Enabled = false;
                this.BorderTestArm3Offset2.Enabled = false;
                this.BorderTestArm3Offset3.Enabled = false;

                this.BorderTestArm4Offset1.Enabled = false;
                this.BorderTestArm4Offset2.Enabled = false;
                this.BorderTestArm4Offset3.Enabled = false;

                this.BorderTestArm5Offset1.Enabled = false;
                this.BorderTestArm5Offset2.Enabled = false;
                this.BorderTestArm5Offset3.Enabled = false;

                this.BorderTestArm6Offset1.Enabled = false;
                this.BorderTestArm6Offset2.Enabled = false;
                this.BorderTestArm6Offset3.Enabled = false;

                this.BorderTestArm7Offset1.Enabled = false;
                this.BorderTestArm7Offset2.Enabled = false;
                this.BorderTestArm7Offset3.Enabled = false;

                this.BorderTestArm8Offset1.Enabled = false;
                this.BorderTestArm8Offset2.Enabled = false;
                this.BorderTestArm8Offset3.Enabled = false;

                this.BorderColdPlate1Offset1.Enabled = false;
                this.BorderColdPlate1Offset2.Enabled = false;
                this.BorderColdPlate1Offset3.Enabled = false;

                this.BorderColdPlate2Offset1.Enabled = false;
                this.BorderColdPlate2Offset2.Enabled = false;
                this.BorderColdPlate2Offset3.Enabled = false;

                this.BorderSocket1Offset1.Enabled = false;
                this.BorderSocket1Offset2.Enabled = false;
                this.BorderSocket1Offset3.Enabled = false;

                this.BorderSocket2Offset1.Enabled = false;
                this.BorderSocket2Offset2.Enabled = false;
                this.BorderSocket2Offset3.Enabled = false;

                this.BorderSocket3Offset1.Enabled = false;
                this.BorderSocket3Offset2.Enabled = false;
                this.BorderSocket3Offset3.Enabled = false;

                this.BorderSocket4Offset1.Enabled = false;
                this.BorderSocket4Offset2.Enabled = false;
                this.BorderSocket4Offset3.Enabled = false;

                this.BorderSocket5Offset1.Enabled = false;
                this.BorderSocket5Offset2.Enabled = false;
                this.BorderSocket5Offset3.Enabled = false;

                this.BorderSocket6Offset1.Enabled = false;
                this.BorderSocket6Offset2.Enabled = false;
                this.BorderSocket6Offset3.Enabled = false;

                this.BorderSocket7Offset1.Enabled = false;
                this.BorderSocket7Offset2.Enabled = false;
                this.BorderSocket7Offset3.Enabled = false;

                this.BorderSocket8Offset1.Enabled = false;
                this.BorderSocket8Offset2.Enabled = false;
                this.BorderSocket8Offset3.Enabled = false;

                this.BorderBasePoint1.Enabled = false;
                this.BorderBasePoint2.Enabled = false;
                this.BorderBasePoint3.Enabled = false;

                #endregion
            }

            BorderOffsetChange();
        }

        private void OffsetTestNumberChange(object sender, EventArgs e)
        {
            TestOffsetChange();
        }

        private void OffsetBorderNumberChange(object sender, EventArgs e)
        {
            BorderOffsetChange();
        }

        private void TestOffsetChange()
        {
            try
            {
                PointF One;
                PointF Two;

                if (Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3) == 0)
                {
                    #region 1点校准
                    //Inside

                    this.TestArm1_InsideOffset4.Text = float.Parse(this.TestArm1_InsideOffset1.Text).ToString("0.0");
                    this.TestArm2_InsideOffset4.Text = float.Parse(this.TestArm2_InsideOffset1.Text).ToString("0.0");
                    this.TestArm3_InsideOffset4.Text = float.Parse(this.TestArm3_InsideOffset1.Text).ToString("0.0");
                    this.TestArm4_InsideOffset4.Text = float.Parse(this.TestArm4_InsideOffset1.Text).ToString("0.0");
                    this.TestArm5_InsideOffset4.Text = float.Parse(this.TestArm5_InsideOffset1.Text).ToString("0.0");
                    this.TestArm6_InsideOffset4.Text = float.Parse(this.TestArm6_InsideOffset1.Text).ToString("0.0");
                    this.TestArm7_InsideOffset4.Text = float.Parse(this.TestArm7_InsideOffset1.Text).ToString("0.0");
                    this.TestArm8_InsideOffset4.Text = float.Parse(this.TestArm8_InsideOffset1.Text).ToString("0.0");

                    //IC

                    this.TestArm1_IcOffset4.Text = float.Parse(this.TestArm1_IcOffset1.Text).ToString("0.0");
                    this.TestArm2_IcOffset4.Text = float.Parse(this.TestArm2_IcOffset1.Text).ToString("0.0");
                    this.TestArm3_IcOffset4.Text = float.Parse(this.TestArm3_IcOffset1.Text).ToString("0.0");
                    this.TestArm4_IcOffset4.Text = float.Parse(this.TestArm4_IcOffset1.Text).ToString("0.0");
                    this.TestArm5_IcOffset4.Text = float.Parse(this.TestArm5_IcOffset1.Text).ToString("0.0");
                    this.TestArm6_IcOffset4.Text = float.Parse(this.TestArm6_IcOffset1.Text).ToString("0.0");
                    this.TestArm7_IcOffset4.Text = float.Parse(this.TestArm7_IcOffset1.Text).ToString("0.0");
                    this.TestArm8_IcOffset4.Text = float.Parse(this.TestArm8_IcOffset1.Text).ToString("0.0");

                    if (Flag.ExternalTestTempChange.HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        this.ColdPlate1Offset4.Text = float.Parse(this.ColdPlate1Offset1.Text).ToString("0.0");
                        this.ColdPlate2Offset4.Text = float.Parse(this.ColdPlate2Offset1.Text).ToString("0.0");

                        this.HotPlate1Offset4.Text = float.Parse(this.HotPlate1Offset1.Text).ToString("0.0");
                        this.HotPlate2Offset4.Text = float.Parse(this.HotPlate2Offset1.Text).ToString("0.0");
                    }
                    else
                    {
                        this.HotPlate1Offset4.Text = float.Parse(this.HotPlate1Offset1.Text).ToString("0.0");
                        this.HotPlate2Offset4.Text = float.Parse(this.HotPlate2Offset1.Text).ToString("0.0");
                    }
                    #endregion
                }
                else if (Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3) == 1)
                {
                    #region 2点校准
                    //Inside
                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm1_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm1_InsideOffset3.Text));
                    this.TestArm1_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm2_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm2_InsideOffset3.Text));
                    this.TestArm2_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm3_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm3_InsideOffset3.Text));
                    this.TestArm3_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm4_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm4_InsideOffset3.Text));
                    this.TestArm4_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm5_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm5_InsideOffset3.Text));
                    this.TestArm5_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm6_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm6_InsideOffset3.Text));
                    this.TestArm6_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm7_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm7_InsideOffset3.Text));
                    this.TestArm7_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm8_InsideOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm8_InsideOffset3.Text));
                    this.TestArm8_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                    //IC
                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm1_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm1_IcOffset3.Text));
                    this.TestArm1_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm2_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm2_IcOffset3.Text));
                    this.TestArm2_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm3_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm3_IcOffset3.Text));
                    this.TestArm3_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm4_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm4_IcOffset3.Text));
                    this.TestArm4_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm5_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm5_IcOffset3.Text));
                    this.TestArm5_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm6_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm6_IcOffset3.Text));
                    this.TestArm6_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm7_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm7_IcOffset3.Text));
                    this.TestArm7_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                    One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm8_IcOffset1.Text));
                    Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm8_IcOffset3.Text));
                    this.TestArm8_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                    if (Flag.ExternalTestTempChange.HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.ColdPlate1Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.ColdPlate1Offset3.Text));
                        this.ColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.ColdPlate2Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.ColdPlate2Offset3.Text));
                        this.ColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate1Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate1Offset3.Text));
                        this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate2Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate2Offset3.Text));
                        this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");
                    }
                    else
                    {
                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate1Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate1Offset3.Text));
                        this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate2Offset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate2Offset3.Text));
                        this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");
                    }
                    #endregion
                }
                else if (Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3) == 2)
                {
                    #region 3点校准
                    float SetTemp = float.Parse(this.TestTemp.Text);
                    float BasePoint1 = float.Parse(this.TestBasePoint1.Text);
                    float BasePoint2 = float.Parse(this.TestBasePoint2.Text);
                    float BasePoint3 = float.Parse(this.TestBasePoint3.Text);

                    if (SetTemp >= BasePoint1 && SetTemp <= BasePoint2)
                    {
                        //Inside
                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm1_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm1_InsideOffset2.Text));
                        this.TestArm1_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm2_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm2_InsideOffset2.Text));
                        this.TestArm2_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm3_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm3_InsideOffset2.Text));
                        this.TestArm3_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm4_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm4_InsideOffset2.Text));
                        this.TestArm4_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm5_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm5_InsideOffset2.Text));
                        this.TestArm5_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm6_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm6_InsideOffset2.Text));
                        this.TestArm6_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm7_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm7_InsideOffset2.Text));
                        this.TestArm7_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm8_InsideOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm8_InsideOffset2.Text));
                        this.TestArm8_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                        //IC
                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm1_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm1_IcOffset2.Text));
                        this.TestArm1_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm2_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm2_IcOffset2.Text));
                        this.TestArm2_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm3_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm3_IcOffset2.Text));
                        this.TestArm3_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm4_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm4_IcOffset2.Text));
                        this.TestArm4_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm5_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm5_IcOffset2.Text));
                        this.TestArm5_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm6_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm6_IcOffset2.Text));
                        this.TestArm6_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm7_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm7_IcOffset2.Text));
                        this.TestArm7_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.TestArm8_IcOffset1.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm8_IcOffset2.Text));
                        this.TestArm8_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                        if (Flag.ExternalTestTempChange.HeatMode == FlagEnum.HeatMode.ATC)
                        {
                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.ColdPlate1Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.ColdPlate1Offset2.Text));
                            this.ColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.ColdPlate2Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.ColdPlate2Offset2.Text));
                            this.ColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate1Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate1Offset2.Text));
                            this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate2Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate2Offset2.Text));
                            this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");
                        }
                        else
                        {
                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate1Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate1Offset2.Text));
                            this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint1.Text), float.Parse(this.HotPlate2Offset1.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate2Offset2.Text));
                            this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");
                        }
                    }
                    else if (SetTemp >= BasePoint2 && SetTemp <= BasePoint3)
                    {
                        //Inside
                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm1_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm1_InsideOffset3.Text));
                        this.TestArm1_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm2_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm2_InsideOffset3.Text));
                        this.TestArm2_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm3_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm3_InsideOffset3.Text));
                        this.TestArm3_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm4_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm4_InsideOffset3.Text));
                        this.TestArm4_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm5_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm5_InsideOffset3.Text));
                        this.TestArm5_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm6_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm6_InsideOffset3.Text));
                        this.TestArm6_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm7_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm7_InsideOffset3.Text));
                        this.TestArm7_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm8_InsideOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm8_InsideOffset3.Text));
                        this.TestArm8_InsideOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                        //IC
                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm1_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm1_IcOffset3.Text));
                        this.TestArm1_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm2_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm2_IcOffset3.Text));
                        this.TestArm2_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm3_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm3_IcOffset3.Text));
                        this.TestArm3_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm4_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm4_IcOffset3.Text));
                        this.TestArm4_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm5_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm5_IcOffset3.Text));
                        this.TestArm5_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm6_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm6_IcOffset3.Text));
                        this.TestArm6_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm7_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm7_IcOffset3.Text));
                        this.TestArm7_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                        One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.TestArm8_IcOffset2.Text));
                        Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.TestArm8_IcOffset3.Text));
                        this.TestArm8_IcOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");


                        if (Flag.ExternalTestTempChange.HeatMode == FlagEnum.HeatMode.ATC)
                        {
                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.ColdPlate1Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.ColdPlate1Offset3.Text));
                            this.ColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.ColdPlate2Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.ColdPlate2Offset3.Text));
                            this.ColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate1Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate1Offset3.Text));
                            this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate2Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate2Offset3.Text));
                            this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.ResultHeatTemp.Text)).ToString("0.0");
                        }
                        else
                        {
                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate1Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate1Offset3.Text));
                            this.HotPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");

                            One = new PointF(float.Parse(this.TestBasePoint2.Text), float.Parse(this.HotPlate2Offset2.Text));
                            Two = new PointF(float.Parse(this.TestBasePoint3.Text), float.Parse(this.HotPlate2Offset3.Text));
                            this.HotPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.TestTemp.Text)).ToString("0.0");
                        }
                    }
                    #endregion
                }
                else
                {

                }
            }
            catch
            {

            }
        }

        private void BorderOffsetChange()
        {
            try
            {
                PointF One;
                PointF Two;

                if (Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3) == 0)
                {
                    #region 1点校准

                    this.BorderTestArm1Offset4.Text = this.BorderTestArm1Offset1.Text;
                    this.BorderTestArm2Offset4.Text = this.BorderTestArm2Offset1.Text;
                    this.BorderTestArm3Offset4.Text = this.BorderTestArm3Offset1.Text;
                    this.BorderTestArm4Offset4.Text = this.BorderTestArm4Offset1.Text;
                    this.BorderTestArm5Offset4.Text = this.BorderTestArm5Offset1.Text;
                    this.BorderTestArm6Offset4.Text = this.BorderTestArm6Offset1.Text;
                    this.BorderTestArm7Offset4.Text = this.BorderTestArm7Offset1.Text;
                    this.BorderTestArm8Offset4.Text = this.BorderTestArm8Offset1.Text;

                    this.BorderColdPlate1Offset4.Text = this.BorderColdPlate1Offset1.Text;
                    this.BorderColdPlate2Offset4.Text = this.BorderColdPlate2Offset1.Text;

                    this.BorderSocket1Offset4.Text = this.BorderSocket1Offset1.Text;
                    this.BorderSocket2Offset4.Text = this.BorderSocket2Offset1.Text;
                    this.BorderSocket3Offset4.Text = this.BorderSocket3Offset1.Text;
                    this.BorderSocket4Offset4.Text = this.BorderSocket4Offset1.Text;
                    this.BorderSocket5Offset4.Text = this.BorderSocket5Offset1.Text;
                    this.BorderSocket6Offset4.Text = this.BorderSocket6Offset1.Text;
                    this.BorderSocket7Offset4.Text = this.BorderSocket7Offset1.Text;
                    this.BorderSocket8Offset4.Text = this.BorderSocket8Offset1.Text;

                    #endregion
                }
                else if (Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3) == 1)
                {
                    #region 2点校准
                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm1Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm1Offset3.Text));
                    this.BorderTestArm1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm2Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm2Offset3.Text));
                    this.BorderTestArm2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm3Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm3Offset3.Text));
                    this.BorderTestArm3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm4Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm4Offset3.Text));
                    this.BorderTestArm4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm5Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm5Offset3.Text));
                    this.BorderTestArm5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm6Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm6Offset3.Text));
                    this.BorderTestArm6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm7Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm7Offset3.Text));
                    this.BorderTestArm7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm8Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm8Offset3.Text));
                    this.BorderTestArm8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();


                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket1Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket1Offset3.Text));
                    this.BorderSocket1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket2Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket2Offset3.Text));
                    this.BorderSocket2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket3Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket3Offset3.Text));
                    this.BorderSocket3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket4Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket4Offset3.Text));
                    this.BorderSocket4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket5Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket5Offset3.Text));
                    this.BorderSocket5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket6Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket6Offset3.Text));
                    this.BorderSocket6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket7Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket7Offset3.Text));
                    this.BorderSocket7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket8Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket8Offset3.Text));
                    this.BorderSocket8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderColdPlate1Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderColdPlate1Offset3.Text));
                    this.BorderColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderColdPlate2Offset1.Text));
                    Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderColdPlate2Offset3.Text));
                    this.BorderColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                    #endregion
                }
                else if (Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3) == 2)
                {
                    #region 3点校准
                    float SetTemp = float.Parse(this.BorderTemp.Text);
                    float BasePoint1 = float.Parse(this.BorderBasePoint1.Text);
                    float BasePoint2 = float.Parse(this.BorderBasePoint2.Text);
                    float BasePoint3 = float.Parse(this.BorderBasePoint3.Text);

                    if (SetTemp >= BasePoint1 && SetTemp <= BasePoint2)
                    {
                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm1Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm1Offset2.Text));
                        this.BorderTestArm1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm2Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm2Offset2.Text));
                        this.BorderTestArm2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm3Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm3Offset2.Text));
                        this.BorderTestArm3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm4Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm4Offset2.Text));
                        this.BorderTestArm4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm5Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm5Offset2.Text));
                        this.BorderTestArm5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm6Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm6Offset2.Text));
                        this.BorderTestArm6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm7Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm7Offset2.Text));
                        this.BorderTestArm7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderTestArm8Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm8Offset2.Text));
                        this.BorderTestArm8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket1Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket1Offset2.Text));
                        this.BorderSocket1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket2Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket2Offset2.Text));
                        this.BorderSocket2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket3Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket3Offset2.Text));
                        this.BorderSocket3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket4Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket4Offset2.Text));
                        this.BorderSocket4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket5Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket5Offset2.Text));
                        this.BorderSocket5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket6Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket6Offset2.Text));
                        this.BorderSocket6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket7Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket7Offset2.Text));
                        this.BorderSocket7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderSocket8Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket8Offset2.Text));
                        this.BorderSocket8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderColdPlate1Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderColdPlate1Offset2.Text));
                        this.BorderColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint1.Text), float.Parse(this.BorderColdPlate2Offset1.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderColdPlate2Offset2.Text));
                        this.BorderColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();
                    }
                    else if (SetTemp >= BasePoint2 && SetTemp <= BasePoint3)
                    {
                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm1Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm1Offset3.Text));
                        this.BorderTestArm1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm2Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm2Offset3.Text));
                        this.BorderTestArm2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm3Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm3Offset3.Text));
                        this.BorderTestArm3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm4Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm4Offset3.Text));
                        this.BorderTestArm4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm5Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm5Offset3.Text));
                        this.BorderTestArm5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm6Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm6Offset3.Text));
                        this.BorderTestArm6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm7Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm7Offset3.Text));
                        this.BorderTestArm7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderTestArm8Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderTestArm8Offset3.Text));
                        this.BorderTestArm8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();


                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket1Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket1Offset3.Text));
                        this.BorderSocket1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket2Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket2Offset3.Text));
                        this.BorderSocket2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket3Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket3Offset3.Text));
                        this.BorderSocket3Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket4Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket4Offset3.Text));
                        this.BorderSocket4Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket5Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket5Offset3.Text));
                        this.BorderSocket5Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket6Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket6Offset3.Text));
                        this.BorderSocket6Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket7Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket7Offset3.Text));
                        this.BorderSocket7Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderSocket8Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderSocket8Offset3.Text));
                        this.BorderSocket8Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderColdPlate1Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderColdPlate1Offset3.Text));
                        this.BorderColdPlate1Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();

                        One = new PointF(float.Parse(this.BorderBasePoint2.Text), float.Parse(this.BorderColdPlate2Offset2.Text));
                        Two = new PointF(float.Parse(this.BorderBasePoint3.Text), float.Parse(this.BorderColdPlate2Offset3.Text));
                        this.BorderColdPlate2Offset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.BorderTemp.Text)).ToString();
                    }
                    #endregion
                }
                else
                {

                }
            }
            catch
            {

            }
        }

        private void TestOffsetPointChange_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshTestOffsetPointState(Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3), this.AtcMode.Checked);
        }

        private void BorderOffsetPointChange_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshBorderOffsetPointState(Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3), this.AtcMode.Checked);
        }

        private void HeatModeSet_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.AtcMode.Checked == true)
            {
                this.IsPreCooling.Enabled = true;
                this.IsResultHeat.Enabled = true;
                this.BorderHeat.Enabled = true;

                this.PreCoolTemp.Enabled = this.BorderHeat.Checked;
                this.ResultHeatTemp.Enabled = this.IsResultHeat.Checked;
                this.BorderTemp.Enabled = this.BorderHeat.Checked;
            }
            else
            {
                this.PreCoolTemp.Enabled = false;
                this.ResultHeatTemp.Enabled = false;
                this.BorderTemp.Enabled = false;

                this.IsPreCooling.Enabled = false;
                this.IsResultHeat.Enabled = false;
                this.BorderHeat.Enabled = false;
            }

            RefreshTestOffsetPointState(Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3), this.AtcMode.Checked);
            RefreshBorderOffsetPointState(Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3), this.AtcMode.Checked);
        }

        private void In_Put_Int(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Int(sender, e);
        }

        private void In_Put_Float(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Float(sender, e);
        }

        private void FloatOffsetChange_Validated(object sender, EventArgs e)
        {
            if (Function.Other.IsFloat((sender as TextBox).Text) == false)
            {
                MessageBox.Show(this, "设定失败，请输入合法的有效值！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (sender as TextBox).SelectAll();
                (sender as TextBox).Focus();
            }
        }

        private void IntNumberChange_Validated(object sender, EventArgs e)
        {
            if (Function.Other.IsInt((sender as TextBox).Text) == false)
            {
                MessageBox.Show(this, "设定失败，请输入合法的有效值！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (sender as TextBox).SelectAll();
                (sender as TextBox).Focus();
            }
        }

        private void TempSetApply_Click(object sender, EventArgs e)
        {
            SaveVarieties(ListComBox.Text);

            if (Flag.StartEnabled.IndependentHeat == false)
            {
                if (ListComBox.Text == Flag.StartEnabled.VarietiesName)
                {
                    Flag.ChangeTempVarietiesName(Flag.StartEnabled.VarietiesName);
                }
            }
            else
            {
                if (Flag.StartEnabled.OddTestArmVarieties.Name == ListComBox.Text || Flag.StartEnabled.EvenTestArmVarieties.Name == ListComBox.Text)
                {
                    Flag.ChangeTempVarietiesName(Flag.StartEnabled.OddTestArmVarieties.Name, Flag.StartEnabled.EvenTestArmVarieties.Name);
                }
            }
        }

        private void TempSetCancel_Click(object sender, EventArgs e)
        {
            if (tm != null)
            {
                this.tm.Stop();
                this.tm.Dispose();
            }

            this.Close();
        }

        private void BorderHeat_CheckedChanged(object sender, EventArgs e)
        {
            this.BorderTemp.Enabled = this.BorderHeat.Checked;
        }

        private void IsPreCooling_CheckedChanged(object sender, EventArgs e)
        {
            this.PreCoolTemp.Enabled = this.IsPreCooling.Checked;
        }

        private void IsResultHeat_CheckedChanged(object sender, EventArgs e)
        {
            this.ResultHeatTemp.Enabled = this.IsResultHeat.Checked;
        }

        private void ListComBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ChangeTempVarietiesName(this.ListComBox.Text);
        }

        private void ListComBox_DropDown(object sender, EventArgs e)
        {
            string Name = ListComBox.Text;

            string[] list = Function.Regedit.Get_Book_List(@"SOFTWARE\JHT\TriTempChiller\","ExternalTempVarieties");

            for (int i = 0; i < list.Length; i++)
            {
                if (ListComBox.Items.Contains(list[i]) == false)
                {
                    ListComBox.Items.Clear();
                    ListComBox.Items.AddRange(list);

                    if (this.ListComBox.Items.Contains("Handler") == false)
                    {
                        this.ListComBox.Items.Add("Handler");
                    }

                    ListComBox.Text = Name;

                    break;
                }
            }
        }

        private void ChangeTempVarietiesName(string Name)
        {
            string ReadOutData;
            TextBox tb = new TextBox();
            if (Name != "")
            {
                this.ListComBox.Text = Name;

                #region 加热模式以及基本参数设定

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "HeatMode");
                if (ReadOutData != null)
                {
                    if (ReadOutData == "ATC")
                    {
                        this.HotMode.Checked = false;
                        this.AtcMode.Checked = true;
                    }
                    else
                    {
                        this.HotMode.Checked = true;
                        this.AtcMode.Checked = false;
                    }
                }

                this.ChillerTemp.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerTemp");
                this.ChillerRange.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerRange");
                this.TestTemp.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetTemp");
                this.PreCoolTemp.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCoolTemp");
                this.ResultHeatTemp.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ResultTemp");
                this.TempRange.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetRange");
                this.BorderTemp.Text = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderTemp");

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "IsPreCooling");
                Function.Other.SetVable(Function.Other.IsIdentical(this.IsPreCooling.Checked, ReadOutData) == true, ReadOutData, false, ref this.IsPreCooling);

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCooling");
                Function.Other.SetVable(Function.Other.IsIdentical(this.IsResultHeat.Checked, ReadOutData) == true, ReadOutData, false, ref this.IsResultHeat);

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderHeat");
                Function.Other.SetVable(Function.Other.IsIdentical(this.BorderHeat.Checked, ReadOutData) == true, ReadOutData, false, ref this.BorderHeat);

                if(this.AtcMode.Checked==true)
                {
                    this.IsPreCooling.Enabled = true;
                    this.IsResultHeat.Enabled = true;
                    this.BorderHeat.Enabled = true;

                    this.PreCoolTemp.Enabled = this.BorderHeat.Checked;
                    this.ResultHeatTemp.Enabled = this.IsResultHeat.Checked;
                    this.BorderTemp.Enabled = this.BorderHeat.Checked;
                }
                else
                {
                    this.PreCoolTemp.Enabled = false;
                    this.ResultHeatTemp.Enabled = false;
                    this.BorderTemp.Enabled = false;

                    this.IsPreCooling.Enabled = false;
                    this.IsResultHeat.Enabled = false;
                    this.BorderHeat.Enabled = false;
                }

                #endregion

                #region 温度偏移的基底参数包含测试和边框

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "TestTempOffsetPoint");
                if (ReadOutData != null)
                {
                    if (ReadOutData == "0")
                    {
                        this.TestOffsetPoint1.Checked = true;
                        this.TestOffsetPoint2.Checked = false;
                        this.TestOffsetPoint3.Checked = false;
                    }
                    else if (ReadOutData == "1")
                    {
                        this.TestOffsetPoint1.Checked = false;
                        this.TestOffsetPoint2.Checked = true;
                        this.TestOffsetPoint3.Checked = false;
                    }
                    else if (ReadOutData == "2")
                    {
                        this.TestOffsetPoint1.Checked = false;
                        this.TestOffsetPoint2.Checked = false;
                        this.TestOffsetPoint3.Checked = true;
                    }
                    else
                    {
                        this.TestOffsetPoint1.Checked = true;
                        this.TestOffsetPoint2.Checked = false;
                        this.TestOffsetPoint3.Checked = false;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "TestTempBasicPoint" + i.ToString());
                    tb = Function.Other.FindControl(this, "TestBasePoint" + (i + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        tb.Text = ReadOutData;
                    }
                }


                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "BorderTempOffsetPoint");
                if (ReadOutData != null)
                {
                    if (ReadOutData == "0")
                    {
                        this.BorderOffsetPoint1.Checked = true;
                        this.BorderOffsetPoint2.Checked = false;
                        this.BorderOffsetPoint3.Checked = false;
                    }
                    else if (ReadOutData == "1")
                    {
                        this.BorderOffsetPoint1.Checked = false;
                        this.BorderOffsetPoint2.Checked = true;
                        this.BorderOffsetPoint3.Checked = false;
                    }
                    else if (ReadOutData == "2")
                    {
                        this.BorderOffsetPoint1.Checked = false;
                        this.BorderOffsetPoint2.Checked = false;
                        this.BorderOffsetPoint3.Checked = true;
                    }
                    else
                    {
                        this.BorderOffsetPoint1.Checked = true;
                        this.BorderOffsetPoint2.Checked = false;
                        this.BorderOffsetPoint3.Checked = false;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "BorderTempBasicPoint" + i.ToString());
                    tb = Function.Other.FindControl(this, "BorderBasePoint" + (i + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        Function.Other.SetVable(Function.Other.IsIdentical(tb.Text, ReadOutData) == true, ReadOutData, "", ref tb);
                    }
                }
                #endregion

                #region TestArm

                for (int i = 0; i < 8; i++)
                {
                    #region Offset

                    for (int j = 0; j < 4; j++)
                    {
                        //Heat_Inside
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "TempOffset" + j.ToString());
                        tb = Function.Other.FindControl(this.TestArm_Inside, "TestArm" + (i + 1).ToString() + "_InsideOffset" + (j + 1).ToString()) as TextBox;
                        if (tb != null) { tb.Text = ReadOutData; }

                        //Heat_IC
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "TempOffset" + j.ToString());
                        tb = Function.Other.FindControl(this.TestArm_IC, "TestArm" + (i + 1).ToString() + "_IcOffset" + (j + 1).ToString()) as TextBox;
                        if (tb != null) { tb.Text = ReadOutData; }
                    }

                    #endregion

                    #region 温度修正

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk");
                    tb = Function.Other.FindControl(this.OffTkTestArm_Inside, "OffTkTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk");
                    tb = Function.Other.FindControl(this.OffTkTestArm_IC, "OffTkTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    #endregion

                    #region 加热功率

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "PowerLimit");
                    tb = Function.Other.FindControl(this.PowerTestArm_Inside, "PowerTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "PowerLimit");
                    tb = Function.Other.FindControl(this.PowerTestArm_IC, "PowerTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    #endregion
                }

                #endregion

                #region ColdPlate

                for (int i = 0; i < 2; i++)
                {

                    for (int j = 0; j < 4; j++)
                    {
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString());
                        tb = Function.Other.FindControl(this.ColdPlate, "ColdPlate" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                        if (tb != null) { tb.Text = ReadOutData; }
                    }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                    tb = Function.Other.FindControl(this.OffTkColdPlate, "OffTkColdPlate" + (i + 1).ToString()) as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    tb = Function.Other.FindControl(this.PowerColdPlate, "PowerColdPlate" + (i + 1).ToString()) as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }
                }

                #endregion

                #region HotPlate
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString());
                        tb = Function.Other.FindControl(this.HotPlate, "HotPlate" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                        if (tb != null) { tb.Text = ReadOutData; }
                    }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                    tb = Function.Other.FindControl(this.OffTkHotPlate, "OffTkHotPlate" + (i + 1).ToString()) as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    tb = Function.Other.FindControl(this.PowerHotPlate, "PowerHotPlate" + (i + 1).ToString()) as TextBox;
                    if (tb != null) { tb.Text = ReadOutData; }

                }
                #endregion

                #region Border

                for (int i = 0; i < 18; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString());
                        if (i < 8)
                        {
                            tb = Function.Other.FindControl(this.BorderTestArmOffset, "BorderTestArm" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                        }
                        else if (i == 8 || i == 9)
                        {
                            tb = Function.Other.FindControl(this.BorderColdPlate, "BorderColdPlate" + (i - 7).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                        }
                        else if (i > 9)
                        {
                            tb = Function.Other.FindControl(this.BorderSocket, "BorderSocket" + (i - 9).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                        }
                        if (tb != null) { tb.Text = ReadOutData; }
                    }

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk");
                    if (i < 8)
                    {
                        tb = Function.Other.FindControl(this.OffTkTestArm_Border, "OffTkTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                    }
                    else if (i == 8 || i == 9)
                    {
                        tb = Function.Other.FindControl(this.OffTkColdPlate_Border, "OffTkColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                    }
                    else if (i > 9)
                    {
                        tb = Function.Other.FindControl(this.OffTkSocket_Border, "OffTkSocket" + (i - 9).ToString()) as TextBox;
                    }
                    if (tb != null) { tb.Text = ReadOutData; }


                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    if (i < 8)
                    {
                        tb = Function.Other.FindControl(this.PowerTestArm_Border, "PowerTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                    }
                    else if (i == 8 || i == 9)
                    {
                        tb = Function.Other.FindControl(this.PowerColdPlate_Border, "PowerColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                    }
                    else if (i > 9)
                    {
                        tb = Function.Other.FindControl(this.PowerSocket_Border, "PowerSocket" + (i - 9).ToString()) as TextBox;
                    }
                    if (tb != null) { tb.Text = ReadOutData; }
                }

                #endregion

                #region Pump

                for (int i = 0; i < 3; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "Pump", "Speed" + i.ToString());
                    TrackBar tkb = Function.Other.FindControl(this.PumpSpeed, "PumpSpeed" + (i + 1).ToString()) as TrackBar;
                    if (tkb != null) { Function.Other.SetVable(Function.Other.IsIdentical(tkb.Text, ReadOutData) == true, ReadOutData, 100, ref tkb); }
                }

                #endregion
            }
            RefreshTestOffsetPointState(Function.Other.Radio_To_Short(this.TestOffsetPoint1, this.TestOffsetPoint2, this.TestOffsetPoint3), this.AtcMode.Checked);
            RefreshBorderOffsetPointState(Function.Other.Radio_To_Short(this.BorderOffsetPoint1, this.BorderOffsetPoint2, this.BorderOffsetPoint3), this.AtcMode.Checked);
        }

        private void SaveVarieties(string Name)
        {
            TextBox tb = new TextBox();

            #region 加热模式以及基本参数设定

            if (this.HotMode.Checked == true)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "HeatMode", "HOT");
            }
            else if (this.AtcMode.Checked == true)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "HeatMode", "ATC");
            }

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerTemp", this.ChillerTemp.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerRange", this.ChillerRange.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetTemp", this.TestTemp.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCoolTemp", this.PreCoolTemp.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ResultTemp", this.ResultHeatTemp.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetRange", this.TempRange.Text);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderTemp", this.BorderTemp.Text);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "IsPreCooling", this.IsPreCooling.Checked);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCooling", this.IsResultHeat.Checked);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderHeat", this.BorderHeat.Checked);

            #endregion

            #region 温度偏移的基底参数包含测试和边框

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "TestTempOffsetPoint", Function.Other.Radio_To_Int(TestOffsetPoint1, TestOffsetPoint2, TestOffsetPoint3));
            for (int i = 0; i < 3; i++)
            {
                tb = Function.Other.FindControl(this, "TestBasePoint" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "TestTempBasicPoint" + i.ToString(), tb.Text);
                }
            }

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "BorderTempOffsetPoint", Function.Other.Radio_To_Int(BorderOffsetPoint1, BorderOffsetPoint2, BorderOffsetPoint3));
            for (int i = 0; i < 3; i++)
            {
                tb = Function.Other.FindControl(this, "BorderBasePoint" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseOffset", "BorderTempBasicPoint" + i.ToString(), tb.Text);
                }
            }

            #endregion

            #region TestArm

            for (int i = 0; i < 8; i++)
            {
                #region Offset

                for (int j = 0; j < 4; j++)
                {
                    //Heat_Inside
                    tb = Function.Other.FindControl(this.TestArm_Inside, "TestArm" + (i + 1).ToString() + "_InsideOffset" + (j + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "TempOffset" + j.ToString(), tb.Text);
                    }

                    //Heat_IC
                    tb = Function.Other.FindControl(this.TestArm_IC, "TestArm" + (i + 1).ToString() + "_IcOffset" + (j + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "TempOffset" + j.ToString(), tb.Text);
                    }
                }

                #endregion

                #region 温度修正

                tb = Function.Other.FindControl(this.OffTkTestArm_Inside, "OffTkTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk", tb.Text);
                }

                tb = Function.Other.FindControl(this.OffTkTestArm_IC, "OffTkTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk", tb.Text);
                }

                #endregion

                #region 加热功率

                tb = Function.Other.FindControl(this.PowerTestArm_Inside, "PowerTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "PowerLimit", tb.Text);
                }

                tb = Function.Other.FindControl(this.PowerTestArm_IC, "PowerTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "PowerLimit", tb.Text);
                }

                #endregion
            }

            #endregion

            #region ColdPlate

            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    tb = Function.Other.FindControl(this.ColdPlate, "ColdPlate" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), tb.Text);
                    }
                }

                tb = Function.Other.FindControl(this.OffTkColdPlate, "OffTkColdPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);
                }

                tb = Function.Other.FindControl(this.PowerColdPlate, "PowerColdPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "PowerLimit", tb.Text);
                }
            }

            #endregion

            #region HotPlate
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tb = Function.Other.FindControl(this.HotPlate, "HotPlate" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                    if (tb != null)
                    {
                        Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), tb.Text);
                    }
                }

                tb = Function.Other.FindControl(this.OffTkHotPlate, "OffTkHotPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);
                }

                tb = Function.Other.FindControl(this.PowerHotPlate, "PowerHotPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "PowerLimit", tb.Text);
                }
            }
            #endregion

            #region Border

            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i < 8)
                    {
                        tb = Function.Other.FindControl(this.BorderTestArmOffset, "BorderTestArm" + (i + 1).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                    }
                    else if (i == 8 || i == 9)
                    {
                        tb = Function.Other.FindControl(this.BorderColdPlate, "BorderColdPlate" + (i - 7).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                    }
                    else if (i > 9)
                    {
                        tb = Function.Other.FindControl(this.BorderSocket, "BorderSocket" + (i - 9).ToString() + "Offset" + (j + 1).ToString()) as TextBox;
                    }
                    if (tb != null)
                    {
                        Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), tb.Text);
                    }
                }

                if (i < 8)
                {
                    tb = Function.Other.FindControl(this.OffTkTestArm_Border, "OffTkTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                }
                else if (i == 8 || i == 9)
                {
                    tb = Function.Other.FindControl(this.OffTkColdPlate_Border, "OffTkColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                }
                else if (i > 9)
                {
                    tb = Function.Other.FindControl(this.OffTkSocket_Border, "OffTkSocket" + (i - 9).ToString()) as TextBox;
                }
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);
                }


                if (i < 8)
                {
                    tb = Function.Other.FindControl(this.PowerTestArm_Border, "PowerTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                }
                else if (i == 8 || i == 9)
                {
                    tb = Function.Other.FindControl(this.PowerColdPlate_Border, "PowerColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                }
                else if (i > 9)
                {
                    tb = Function.Other.FindControl(this.PowerSocket_Border, "PowerSocket" + (i - 9).ToString()) as TextBox;
                }
                if (tb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "PowerLimit", tb.Text);
                }
            }

            #endregion

            #region Pump

            for (int i = 0; i < 3; i++)
            {
                TrackBar tkb = Function.Other.FindControl(this.PumpSpeed, "PumpSpeed" + (i + 1).ToString()) as TrackBar;
                if (tkb != null)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "Pump", "Speed" + i.ToString(), tkb.Value);
                }
            }

            #endregion
        }

    }
}
