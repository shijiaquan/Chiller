using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DewPointSensor;

namespace Chiller
{
    public partial class ExternalTempControl : Form
    {
        public ExternalTempControl()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x02000000;
                return cp;
            }
        }

        private void ExternalTempControl_Load(object sender, EventArgs e)
        {
            string[] list = Function.Regedit.Get_Book_List(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties");

            if (list != null)
            {
                this.ListComBox.Items.Clear();
                this.ListComBox.Items.AddRange(list);
            }

          
            if (list != null)
            {
                this.OddTestArmVarietiesName.Items.Clear();
                this.OddTestArmVarietiesName.Items.AddRange(list);
            }
        
            if (list != null)
            {
                this.EvenTestArmVarietiesName.Items.Clear();
                this.EvenTestArmVarietiesName.Items.AddRange(list);
            }
          

            this.ListComBox.Text = Flag.StartEnabled.VarietiesName;
            this.OddTestArmVarietiesName.Text = Flag.StartEnabled.OddTestArmVarieties.Name;
            this.EvenTestArmVarietiesName.Text = Flag.StartEnabled.EvenTestArmVarieties.Name;

            this.ListComBox.Text = Flag.StartEnabled.VarietiesName;

            this.UseHeatChange.Checked = Flag.ExternalTestTempChange.UseIcHeat;
            this.IndependentUse.Checked = Flag.ExternalTestTempChange.IndependentUseHeat;

            this.UseHeatChange.Enabled = !Flag.ExternalTestTempChange.IndependentUseHeat;
            this.TestArmUseHeat.Enabled = Flag.ExternalTestTempChange.IndependentUseHeat;

            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                Switch.ZDSwitch TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use") as Switch.ZDSwitch;
                Label TestArm_lb = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use_lb") as Label;

                TestArm.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                TestArm_lb.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];

                TestArm.Checked = Flag.ExternalTestTempChange.TestArmUseIcHeat[i];
            }

            this.IndependentHeat.Checked = Flag.StartEnabled.IndependentHeat;
            this.ListComBox.Enabled = !Flag.StartEnabled.IndependentHeat;
            this.OddTestArmVarietiesName.Enabled = Flag.StartEnabled.IndependentHeat;
            this.EvenTestArmVarietiesName.Enabled = Flag.StartEnabled.IndependentHeat;

            Flag.SystemThread.ExternalTempControlUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.ExternalTempControlEnabled = true;
            Flag.SystemThread.ExternalTempControlUpData.Start();

            Flag.ExternalTempControlChart.ChartRefreshEnabled = true;
        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            Color _Color = Color.White;
            bool _IsTwinkle = false;
            bool[] IsTwinkle = new bool[8];
            Stopwatch TwinkleTime = new Stopwatch();

            Stopwatch ChangeTime = new Stopwatch();
            while (Flag.SystemThread.ExternalTempControlEnabled)
            {
                Thread.Sleep(100);

                this.Invoke((MethodInvoker)delegate
                {
                    #region STL控制中

                    this.groupBox1.Enabled = !Flag.Handler.IsHandlerControl;
                    this.groupBox2.Enabled = !Flag.Handler.IsHandlerControl;

                    #endregion

                    #region 显示加热器开启状态

                    this.HeatingEnabled.Checked = Flag.StartEnabled.RunHeat.Heating;

                    #endregion

                    #region 提示按下应用按钮,更改产品名称
                    if (this.IndependentHeat.Checked == true)
                    {
                        if (Flag.StartEnabled.IndependentHeat != this.IndependentHeat.Checked ||
                        Flag.StartEnabled.OddTestArmVarieties.Name != this.OddTestArmVarietiesName.Text ||
                        Flag.StartEnabled.EvenTestArmVarieties.Name != this.EvenTestArmVarietiesName.Text)
                        {
                            if (ChangeTime.IsRunning == false)
                            {
                                ChangeTime.Start();
                            }
                        }
                        else
                        {
                            ChangeTime.Stop();
                            AppTempSet.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        if (Flag.StartEnabled.IndependentHeat != this.IndependentHeat.Checked || Flag.StartEnabled.VarietiesName != this.ListComBox.Text)
                        {
                            if (ChangeTime.IsRunning == false)
                            {
                                ChangeTime.Start();
                            }
                        }
                        else
                        {
                            ChangeTime.Stop();
                            AppTempSet.BackColor = Color.White;
                        }
                    }

                    if (ChangeTime.ElapsedMilliseconds >= 500)
                    {
                        if (AppTempSet.BackColor == Color.White)
                        {
                            AppTempSet.BackColor = Color.Green;
                        }
                        else
                        {
                            AppTempSet.BackColor = Color.White;
                        }
                        ChangeTime.Restart();
                    }

                    #endregion

                    #region 提示更改设定，更改 IC 或 Inside 使能
                    if (Flag.Handler.IsHandlerControl == false)
                    {
                        if (this.IndependentUse.Checked == true)
                        {
                            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                            {
                                Switch.ZDSwitch TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use") as Switch.ZDSwitch;

                                if (Flag.ExternalTestTempChange.TestArmUseIcHeat[i] != TestArm.Checked)
                                {
                                    IsTwinkle[i] = true;
                                }
                                else
                                {
                                    IsTwinkle[i] = false;
                                }
                            }
                            _IsTwinkle = false;
                            for (int i = 0; i < IsTwinkle.Length; i++)
                            {
                                if (IsTwinkle[i] == true)
                                {
                                    _IsTwinkle = true;
                                    break;
                                }
                            }
                            if (_IsTwinkle == true || Flag.ExternalTestTempChange.IndependentUseHeat != this.IndependentUse.Checked)
                            {
                                if (TwinkleTime.IsRunning == false)
                                {
                                    TwinkleTime.Restart();
                                }
                            }
                            else
                            {
                                TwinkleTime.Stop();
                                ApplyUse.BackColor = Color.White;
                                for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                                {
                                    Label TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use_lb") as Label;
                                    TestArm.BackColor = Color.White;
                                }
                            }

                            if (TwinkleTime.ElapsedMilliseconds >= 500)
                            {
                                if (ApplyUse.BackColor == Color.White)
                                {
                                    ApplyUse.BackColor = Color.Green;
                                }
                                else
                                {
                                    ApplyUse.BackColor = Color.White;
                                }

                                for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                                {
                                    Label TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use_lb") as Label;
                                    if (IsTwinkle[i] == true)
                                    {
                                        if (ApplyUse.BackColor == Color.White)
                                        {
                                            TestArm.BackColor = Color.White;
                                        }
                                        else
                                        {
                                            TestArm.BackColor = Color.Silver;
                                        }
                                    }
                                    else
                                    {
                                        TestArm.BackColor = Color.White;
                                    }
                                }
                                TwinkleTime.Restart();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                            {
                                Label TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use_lb") as Label;
                                TestArm.BackColor = Color.White;
                            }

                            if (Flag.ExternalTestTempChange.UseIcHeat != this.UseHeatChange.Checked || Flag.ExternalTestTempChange.IndependentUseHeat != this.IndependentUse.Checked)
                            {
                                if (TwinkleTime.IsRunning == false)
                                {
                                    TwinkleTime.Restart();
                                }
                            }
                            else
                            {
                                TwinkleTime.Stop();
                                ApplyUse.BackColor = Color.White;
                                UseHeatChange_lb.BackColor = Color.White;
                            }

                            if (TwinkleTime.ElapsedMilliseconds >= 500)
                            {
                                if (Flag.ExternalTestTempChange.UseIcHeat != this.UseHeatChange.Checked)
                                {
                                    if (ApplyUse.BackColor == Color.White)
                                    {
                                        UseHeatChange_lb.BackColor = Color.Silver;
                                    }
                                    else
                                    {
                                        UseHeatChange_lb.BackColor = Color.White;
                                    }
                                }
                                if (Flag.ExternalTestTempChange.IndependentUseHeat != this.IndependentUse.Checked || Flag.ExternalTestTempChange.UseIcHeat != this.UseHeatChange.Checked)
                                {
                                    if (ApplyUse.BackColor == Color.White)
                                    {
                                        ApplyUse.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        ApplyUse.BackColor = Color.White;
                                    }
                                }

                                TwinkleTime.Restart();
                            }
                        }
                    }
                    #endregion

                    #region 更新TestArm加热通道
                    if (this.TestArmUseHeat.Enabled == true)
                    {
                        for (int i = 0; i < Flag.TestArm.Length; i++)
                        {
                            Switch.ZDSwitch TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use") as Switch.ZDSwitch;
                            Label TestArm_lb = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use_lb") as Label;

                            TestArm.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                            TestArm_lb.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                        }
                    }
                    #endregion

                    #region TestArm

                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        Panel TestArm = Chiller.Function.Other.FindControl(this.DisplayTemp, "TestArm" + (i + 1).ToString()) as Panel;
                        Label TestArm_Inside = Chiller.Function.Other.FindControl(this.DisplayTemp, "TestArm" + (i + 1).ToString() + "_Inside") as Label;
                        Label TestArm_IC = Chiller.Function.Other.FindControl(this.DisplayTemp, "TestArm" + (i + 1).ToString() + "_IC") as Label;

                        if (TestArm == null || TestArm_Inside == null || TestArm_IC == null)
                        {
                            continue;
                        }

                        #region 显示测试头内部温度状态

                        if (Flag.ExternalTestTempChange.TestArmEnabled[i])
                        {
                            if (Flag.TestArm[i].Heat_Inside.GetInData.ErrCode != "")
                            {
                                TestArm_Inside.Text = Flag.TestArm[i].Heat_Inside.GetInData.ErrCode;
                                TestArm_Inside.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArm_Inside.Text = Flag.TestArm[i].Heat_Inside.GetInData.PV.ToString("0.0") + "℃";
                                TestArm_Inside.ForeColor = Color.Blue;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.TestArm[i].Heat_Inside.GetInData.ErrCode != "")
                            {
                                TestArm_Inside.Text = Flag.TestArm[i].Heat_Inside.GetInData.ErrCode;
                                TestArm_Inside.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArm_Inside.Text = Flag.TestArm[i].Heat_Inside.GetInData.PV.ToString("0.0") + "℃";
                                TestArm_Inside.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            TestArm_Inside.Text = "- ℃";
                            TestArm_Inside.ForeColor = Color.Black;
                        }

                        #endregion

                        #region 显示测试头IC温度状态

                        if (Flag.ExternalTestTempChange.TestArmEnabled[i])
                        {
                            if (Flag.TestArm[i].Heat_IC.GetInData.ErrCode != "")
                            {
                                TestArm_IC.Text = Flag.TestArm[i].Heat_IC.GetInData.ErrCode;
                                TestArm_IC.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArm_IC.Text = Flag.TestArm[i].Heat_IC.GetInData.PV.ToString("0.0") + "℃";
                                TestArm_IC.ForeColor = Color.Blue;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp)
                        {
                            if (Flag.TestArm[i].Heat_IC.GetInData.ErrCode != "")
                            {
                                TestArm_IC.Text = Flag.TestArm[i].Heat_IC.GetInData.ErrCode;
                                TestArm_IC.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArm_IC.Text = Flag.TestArm[i].Heat_IC.GetInData.PV.ToString("0.0") + "℃";
                                TestArm_IC.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            TestArm_IC.Text = "- ℃";
                            TestArm_IC.ForeColor = Color.Black;
                        }

                        #endregion

                        #region 显示背景颜色
                        if (TestArm_IC.ForeColor == Color.Red || TestArm_Inside.ForeColor == Color.Red)
                        {
                            TestArm.BackColor = Color.Yellow;
                        }
                        else if (TestArm_IC.ForeColor == Color.Black && TestArm_Inside.ForeColor == Color.Black)
                        {
                            TestArm.BackColor = Color.Silver;
                        }
                        else
                        {
                            TestArm.BackColor = Color.White;
                        }
                        #endregion
                    }

                    #endregion

                    #region ColdPlate
                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        Panel ColdPlate = Chiller.Function.Other.FindControl(this.DisplayTemp, "ColdPlate" + (i + 1).ToString()) as Panel;
                        Label ColdPlateTemp = Chiller.Function.Other.FindControl(this.DisplayTemp, "ColdPlate" + (i + 1).ToString() + "Temp") as Label;

                        if (ColdPlate == null || ColdPlateTemp == null)
                        {
                            continue;
                        }


                        if (Flag.ExternalTestTempChange.ColdPlateEnabled[i])
                        {
                            if (Flag.ColdPlate[i].Heat.GetInData.ErrCode != "")
                            {
                                ColdPlateTemp.Text = Flag.ColdPlate[i].Heat.GetInData.ErrCode;
                                ColdPlateTemp.ForeColor = Color.Red;
                                ColdPlate.BackColor = Color.Yellow;
                            }
                            else
                            {
                                ColdPlateTemp.Text = Flag.ColdPlate[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                ColdPlateTemp.ForeColor = Color.Blue;
                                ColdPlate.BackColor = Color.White;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.ColdPlate[i].Heat.GetInData.ErrCode != "")
                            {
                                ColdPlateTemp.Text = Flag.ColdPlate[i].Heat.GetInData.ErrCode;
                                ColdPlateTemp.ForeColor = Color.Red;
                                ColdPlate.BackColor = Color.Yellow;
                            }
                            else
                            {
                                ColdPlateTemp.Text = Flag.ColdPlate[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                ColdPlateTemp.ForeColor = Color.Black;
                                ColdPlate.BackColor = Color.Silver;
                            }
                        }
                        else
                        {
                            ColdPlateTemp.Text = "- ℃";
                            ColdPlateTemp.ForeColor = Color.Black;
                            ColdPlate.BackColor = Color.Silver;
                        }
                    }

                    #endregion

                    #region HotPlate
                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        Panel HotPlate = Chiller.Function.Other.FindControl(this.DisplayTemp, "HotPlate" + (i + 1).ToString()) as Panel;
                        Label HotPlateTemp = Chiller.Function.Other.FindControl(this.DisplayTemp, "HotPlate" + (i + 1).ToString() + "Temp") as Label;

                        if (HotPlate == null || HotPlateTemp == null)
                        {
                            continue;
                        }

                        if (Flag.ExternalTestTempChange.HotPlateEnabled[i])
                        {
                            if (Flag.HotPlate[i].Heat.GetInData.ErrCode != "")
                            {
                                HotPlateTemp.Text = Flag.HotPlate[i].Heat.GetInData.ErrCode;
                                HotPlateTemp.ForeColor = Color.Red;
                                HotPlate.BackColor = Color.Yellow;
                            }
                            else
                            {
                                HotPlateTemp.Text = Flag.HotPlate[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                HotPlateTemp.ForeColor = Color.Blue;
                                HotPlate.BackColor = Color.White;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.HotPlate[i].Heat.GetInData.ErrCode != "")
                            {
                                HotPlateTemp.Text = Flag.HotPlate[i].Heat.GetInData.ErrCode;
                                HotPlateTemp.ForeColor = Color.Red;
                                HotPlate.BackColor = Color.Yellow;
                            }
                            else
                            {
                                HotPlateTemp.Text = Flag.HotPlate[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                HotPlateTemp.ForeColor = Color.Black;
                                HotPlate.BackColor = Color.Silver;
                            }
                        }
                        else
                        {
                            HotPlateTemp.Text = "- ℃";
                            HotPlateTemp.ForeColor = Color.Black;
                            HotPlate.BackColor = Color.Silver;
                        }
                    }

                    #endregion

                    #region TestArm And Socket Shield

                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        Label TestArmShield = Chiller.Function.Other.FindControl(this.DisplayTemp, "TestArm" + (i + 1).ToString() + "Shield") as Label;
                        Label SocketShield = Chiller.Function.Other.FindControl(this.DisplayTemp, "Socket" + (i + 1).ToString() + "Shield") as Label;

                        #region TestArm Shield

                        if (TestArmShield == null)
                        {
                            continue;
                        }

                        if (Flag.ExternalTestTempChange.BorderEnabled[i])
                        {
                            if (Flag.BorderTemp[i].Heat.GetInData.ErrCode != "")
                            {
                                TestArmShield.Text = Flag.BorderTemp[i].Heat.GetInData.ErrCode;
                                TestArmShield.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArmShield.Text = Flag.BorderTemp[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                TestArmShield.ForeColor = Color.Blue;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.BorderTemp[i].Heat.GetInData.ErrCode != "")
                            {
                                TestArmShield.Text = Flag.BorderTemp[i].Heat.GetInData.ErrCode;
                                TestArmShield.ForeColor = Color.Red;
                            }
                            else
                            {
                                TestArmShield.Text = Flag.BorderTemp[i].Heat.GetInData.PV.ToString("0.0") + "℃";
                                TestArmShield.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            TestArmShield.Text = "- ℃";
                            TestArmShield.ForeColor = Color.Black;
                        }

                        #endregion

                        #region Socket Shield

                        if (SocketShield == null)
                        {
                            continue;
                        }

                        if (Flag.ExternalTestTempChange.BorderEnabled[i + 10])
                        {
                            if (Flag.BorderTemp[i + 10].Heat.GetInData.ErrCode != "")
                            {
                                SocketShield.Text = Flag.BorderTemp[i + 10].Heat.GetInData.ErrCode;
                                SocketShield.ForeColor = Color.Red;
                            }
                            else
                            {
                                SocketShield.Text = Flag.BorderTemp[i + 10].Heat.GetInData.PV.ToString("0.0") + "℃";
                                SocketShield.ForeColor = Color.Blue;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.BorderTemp[i + 10].Heat.GetInData.ErrCode != "")
                            {
                                SocketShield.Text = Flag.BorderTemp[i + 10].Heat.GetInData.ErrCode;
                                SocketShield.ForeColor = Color.Red;
                            }
                            else
                            {
                                SocketShield.Text = Flag.BorderTemp[i + 10].Heat.GetInData.PV.ToString("0.0") + "℃";
                                SocketShield.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            SocketShield.Text = "- ℃";
                            SocketShield.ForeColor = Color.Black;
                        }

                        #endregion

                    }

                    #endregion

                    #region ColdPlate Shield
                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        Label ColdPlateTemp = Chiller.Function.Other.FindControl(this.DisplayTemp, "ColdPlate" + (i + 1).ToString() + "Shield") as Label;

                        if (ColdPlateTemp == null)
                        {
                            continue;
                        }

                        if (Flag.ExternalTestTempChange.BorderEnabled[i + 8])
                        {
                            if (Flag.BorderTemp[i + 8].Heat.GetInData.ErrCode != "")
                            {
                                ColdPlateTemp.Text = Flag.BorderTemp[i + 8].Heat.GetInData.ErrCode;
                                ColdPlateTemp.ForeColor = Color.Red;
                            }
                            else
                            {
                                ColdPlateTemp.Text = Flag.BorderTemp[i + 8].Heat.GetInData.PV.ToString("0.0") + "℃";
                                ColdPlateTemp.ForeColor = Color.Blue;
                            }
                        }
                        else if (Flag.ExternalTestTempChange.NoEnableDisplayTemp == true)
                        {
                            if (Flag.BorderTemp[i + 8].Heat.GetInData.ErrCode != "")
                            {
                                ColdPlateTemp.Text = Flag.BorderTemp[i + 8].Heat.GetInData.ErrCode;
                                ColdPlateTemp.ForeColor = Color.Red;
                            }
                            else
                            {
                                ColdPlateTemp.Text = Flag.BorderTemp[i + 8].Heat.GetInData.PV.ToString("0.0") + "℃";
                                ColdPlateTemp.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            ColdPlateTemp.Text = "- ℃";
                            ColdPlateTemp.ForeColor = Color.Black;
                        }
                    }

                    #endregion

                    #region 刷新界面显示露点值

                    Chiller.Function.Other.SetText(Flag.DewPoint[0].IsConnect, Flag.DewPoint[0].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm1DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[1].IsConnect, Flag.DewPoint[1].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm2DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[2].IsConnect, Flag.DewPoint[2].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm3DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[3].IsConnect, Flag.DewPoint[3].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm4DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[4].IsConnect, Flag.DewPoint[4].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm5DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[5].IsConnect, Flag.DewPoint[5].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm6DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[6].IsConnect, Flag.DewPoint[6].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm7DewPoint);
                    Chiller.Function.Other.SetText(Flag.DewPoint[7].IsConnect, Flag.DewPoint[7].DewPoint.ToString("0.0") + "℃", "Err", ref this.TestArm8DewPoint);

                    Chiller.Function.Other.SetFColor(Flag.DewPoint[0].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm1DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[1].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm2DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[2].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm3DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[3].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm4DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[4].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm5DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[5].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm6DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[6].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm7DewPoint);
                    Chiller.Function.Other.SetFColor(Flag.DewPoint[7].IsConnect, Color.RoyalBlue, Color.Red, ref this.TestArm8DewPoint);
                    #endregion

                    #region 刷新曲线参数
                    if (Flag.ExternalTempControlChart.ChartRefreshEnabled == true)
                    {
                        Flag.ExternalTempControlChart.ChartRefreshEnabled = false;

                        this.HandleTempChart.ChartAreas[0].AxisX.Maximum = Flag.ExternalTempControlChart.ChartPointLength;
                        this.HandleTempChart.ChartAreas[0].AxisY.Maximum = Flag.ExternalTempControlChart.ChartHightTemp;
                        this.HandleTempChart.ChartAreas[0].AxisY.Minimum = Flag.ExternalTempControlChart.ChartLovTemp;

                        for (int i = 0; i < Flag.ExternalTempControlChart.TempChart.Length; i++)
                        {
                            this.HandleTempChart.Series[i].Color = Flag.ExternalTempControlChart.TempChart[i].Color;
                            this.HandleTempChart.Series[i].ChartType = Flag.ExternalTempControlChart.TempChart[i].Type;
                        }
                    }
                    #endregion

                    #region 刷新一次温度曲线

                    if (Sw.ElapsedMilliseconds >= Flag.ExternalTempControlChart.ChartRefreshTime * 1000)
                    {
                        this.HandleTempChart.Series[this.HandleTempChart.Series.Count - 1].Points.Clear();

                        for (int i = 0; i < this.HandleTempChart.Series.Count - 1; i++)
                        {
                            if (i <= 7)
                            {
                                this.HandleTempChart.Series[i].Enabled = Flag.TestArm[i].Heat_IC.SetToHeat.Enabled;

                                if (Flag.TestArm[i].Heat_IC.SetToHeat.Enabled == true)
                                {
                                    this.HandleTempChart.Series[i].Points.Clear();
                                }
                            }
                            else if (i == 8 || i == 9)
                            {
                                this.HandleTempChart.Series[i].Enabled = Flag.ColdPlate[i - 8].Heat.SetToHeat.Enabled;

                                if (Flag.ColdPlate[i - 8].Heat.SetToHeat.Enabled == true)
                                {
                                    this.HandleTempChart.Series[i].Points.Clear();
                                }
                            }
                            else if (i == 10 || i == 11)
                            {
                                this.HandleTempChart.Series[i].Enabled = Flag.HotPlate[i - 10].Heat.SetToHeat.Enabled;

                                if (Flag.HotPlate[i - 10].Heat.SetToHeat.Enabled == true)
                                {
                                    this.HandleTempChart.Series[i].Points.Clear();
                                }
                            }
                        }


                        int StartIndex = Chiller.Function.Other.SetVable(Flag.TestArm[0].Heat_IC.NowRecord.Count >= Flag.ExternalTempControlChart.ChartPointLength, Flag.TestArm[0].Heat_IC.NowRecord.Count - Flag.ExternalTempControlChart.ChartPointLength, 0);

                        for (int i = StartIndex; i < Flag.TestArm[0].Heat_IC.NowRecord.Count; i++)
                        {
                            //加载设定温度
                            FlagStruct.RecordBase Record1 = Flag.TestArm[0].Heat_IC.SetRecord.GetValue(i);
                            this.HandleTempChart.Series[this.HandleTempChart.Series.Count - 1].Points.AddXY(Record1.Time, Record1.Value);

                            //加载当前温度
                            for (int j = 0; j < this.HandleTempChart.Series.Count - 1; j++)
                            {
                                if (j <= 7)
                                {
                                    if (Flag.TestArm[j].Heat_IC.SetToHeat.Enabled == true)
                                    {
                                        FlagStruct.RecordBase Record2 = Flag.TestArm[j].Heat_IC.NowRecord.GetValue(i);
                                        this.HandleTempChart.Series[j].Points.AddXY(Record2.Time, Record2.Value);
                                    }
                                }
                                else if (j == 8 || j == 9)
                                {
                                    if (Flag.ColdPlate[j - 8].Heat.SetToHeat.Enabled == true)
                                    {
                                        FlagStruct.RecordBase Record2 = Flag.ColdPlate[j - 8].Heat.NowRecord.GetValue(i);
                                        this.HandleTempChart.Series[j].Points.AddXY(Record2.Time, Record2.Value);
                                    }
                                }
                                else if (j == 10 || j == 11)
                                {
                                    if (Flag.HotPlate[j - 10].Heat.SetToHeat.Enabled == true)
                                    {
                                        FlagStruct.RecordBase Record2 = Flag.HotPlate[j - 10].Heat.NowRecord.GetValue(i);
                                        this.HandleTempChart.Series[j].Points.AddXY(Record2.Time, Record2.Value);
                                    }
                                }
                            }
                        }

                        Sw.Restart();
                    }
                    #endregion
                });
            }
            Flag.SystemThread.ExternalTempControlEndState = true;
        }

        private void OpenTempVarieties_Click(object sender, EventArgs e)
        {
            ExternalTempVarietiesList fm = new ExternalTempVarietiesList();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void OpenChartSet_Click(object sender, EventArgs e)
        {
            ExternalTempControlChart fm = new ExternalTempControlChart();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void OpenTempSet_Click(object sender, EventArgs e)
        {
            if(Flag.Handler.IsHandlerControl==false)
            {
                Win.FromBox.ExternalTestTempChangeFrom = new ExternalTempVarietiesData();
                Chiller.Function.Win.OpenWindow(Win.FromBox.ExternalTestTempChangeFrom, this, Win.FromBox.ExternalTestTempChangeFrom.Text, false);
            }
            else
            {
                MessageBox.Show("正在接受SLT控制，无法更改当前参数！", "提示");
            }
        }

        private void OpenEnableSet_Click(object sender, EventArgs e)
        {
            if (Flag.Handler.IsHandlerControl == false)
            {
                ExternalTempEnable fm = new ExternalTempEnable();
                Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
            }
            else
            {
                MessageBox.Show("正在接受SLT控制，无法更改当前参数！", "提示");
            }
        }

        private void TempDataOut_Click(object sender, EventArgs e)
        {
            ExternalTempDataOut fm = new ExternalTempDataOut();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);

        }

        private void ConnectingPlateTemp_Click(object sender, EventArgs e)
        {
            ExternalConnectingPlateTemp fm = new ExternalConnectingPlateTemp();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void HeatingEnabled_MouseDown(object sender, MouseEventArgs e)
        {
            bool IsNoEn = false;
            bool IsAtc = false;

            this.HeatingEnabled.Checked = Flag.StartEnabled.RunHeat.Heating;
            if (Work.Initial.SenserDetection(true) == false)
            {
                Function.Other.Message("系统自检未检测通过，详情请查看设备报警页面！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
            {
                IsNoEn = IsNoEn || (Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled || Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled);
            }
            for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
            {
                if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                {
                    IsAtc = true;
                }
            }
            if (IsNoEn == false)
            {
                Function.Other.Message("温控通道全部处于关闭状态，无法启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.HeatingEnabled.Checked = false;
                return;
            }

            if (IsAtc == true && Flag.WaterBox.Height.Value <= 0)
            {
                Function.Other.Message("载冷夜过低，无法启动ATC温控操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (Flag.StartEnabled.RunHeat.Heating == true)
            {
                if (Win.MessageBox(this, "温控系统已开启，是否确认关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                {
                    TempControl.ExternalTempControl.RunOrStopTempControl(false);
                }
                else
                {
                    this.HeatingEnabled.Checked = true;
                }
            }
            else
            {
                if (Win.MessageBox(this, "温控系统已关闭，是否确认开启？" + "\r\n" + "点击Yes确认开启." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                {
                    TempControl.ExternalTempControl.RunOrStopTempControl(true);
                }
                else
                {
                    this.HeatingEnabled.Checked = false;
                }
            }
        }

        private void Independent_CheckedChanged(object sender, EventArgs e)
        {
            this.UseHeatChange.Enabled = !this.IndependentUse.Checked;
            this.TestArmUseHeat.Enabled = this.IndependentUse.Checked;
        }

        private void ListComBox_DropDown(object sender, EventArgs e)
        {

            string Name = ListComBox.Text;

            string[] list = Function.Regedit.Get_Book_List(@"SOFTWARE\JHT\TriTempChiller\","ExternalTempVarieties");
            if (list == null)
            {
                return;
            }
                for (int i = 0; i < list.Length; i++)
                {
                    if ((sender as ComboBox).Items.Contains(list[i]) == false)
                    {
                        (sender as ComboBox).Items.Clear();
                        (sender as ComboBox).Items.AddRange(list);

                        if ((sender as ComboBox).Items.Contains("Handler") == false)
                        {
                            (sender as ComboBox).Items.Add("Handler");
                        }

                         (sender as ComboBox).Text = Name;
                        break;
                    }
                }
            
        }

        private void ApplyUse_Click(object sender, EventArgs e)
        {
            if(this.IndependentUse.Checked==false)
            {
                for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                {
                    Flag.ExternalTestTempChange.TestArmUseIcHeat[i] = UseHeatChange.Checked;
                }
            }
            else
            {
                for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
                {
                    Switch.ZDSwitch TestArm = Function.Other.FindControl(this.TestArmUseHeat, "TestArm" + (i + 1).ToString() + "_Use") as Switch.ZDSwitch;
                    Flag.ExternalTestTempChange.TestArmUseIcHeat[i] = TestArm.Checked;
                }
            }

            Flag.ExternalTestTempChange.IndependentUseHeat = this.IndependentUse.Checked;

           
            Flag.SaveData();
        }

        private void AppTempSet_Click(object sender, EventArgs e)
        {
            Flag.StartEnabled.IndependentHeat = this.IndependentHeat.Checked;
            if (Flag.StartEnabled.IndependentHeat==true)
            {
                string OddvarietiesName = VarietiesIsATC(this.OddTestArmVarietiesName.Text);
                string EvenVarietiesName = VarietiesIsATC(this.EvenTestArmVarietiesName.Text);

                if (OddvarietiesName != null && EvenVarietiesName != null)
                {
                    if (OddvarietiesName != "ATC" || EvenVarietiesName != "ATC")
                    {
                        Flag.StartEnabled.OddTestArmVarieties.Name = this.OddTestArmVarietiesName.Text;
                        Flag.StartEnabled.EvenTestArmVarieties.Name = this.EvenTestArmVarietiesName.Text;

                        Flag.ChangeTempVarietiesName(Flag.StartEnabled.OddTestArmVarieties.Name, Flag.StartEnabled.EvenTestArmVarieties.Name);

                        Flag.SaveData();
                    }
                    else
                    {
                        MessageBox.Show(this, "无法同时两侧ATC模式！" + "\r\n" + "1.请修改为一侧ATC，一侧HOT模式！" + "\r\n" + "2.关闭独立温度功能！", "提示");
                    }
                }
                else
                {
                    if (OddvarietiesName == null)
                    {
                        MessageBox.Show(this, "TestArm(1.3.5.7)品种温度加载失败！", "提示");
                    }
                    else if (EvenVarietiesName == null)
                    {
                        MessageBox.Show(this, "TestArm(2.4.6.8)品种温度加载失败！", "提示");
                    }
                }
            }
            else
            {
                Flag.StartEnabled.VarietiesName = ListComBox.Text;
                Flag.ChangeTempVarietiesName(Flag.StartEnabled.VarietiesName);

                Flag.SaveData();
            }
           
        }

        private void IndependentHeat_CheckedChanged(object sender, EventArgs e)
        {
            this.ListComBox.Enabled = !this.IndependentHeat.Checked;
            this.OddTestArmVarietiesName.Enabled = this.IndependentHeat.Checked;
            this.EvenTestArmVarietiesName.Enabled = this.IndependentHeat.Checked;
        }

        private string VarietiesIsATC(string Name)
        {
            return Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "HeatMode");
        }

        private void DisplayHeatState_Click(object sender, EventArgs e)
        {
            ExternalTempDisplay fm = new ExternalTempDisplay();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void OpenTkOffSet_Click(object sender, EventArgs e)
        {
            TempOffTKSet fm = new TempOffTKSet();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }
    }
}
