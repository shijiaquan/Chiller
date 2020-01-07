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

namespace Chiller
{
    public partial class InternalColdControl : Form
    {
        public InternalColdControl()
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

        private void InternalColdControl_Load(object sender, EventArgs e)
        {
            this.Unit1.SwitchStatus = Flag.Unit[0].Enabled;
            this.Unit2.SwitchStatus = Flag.Unit[1].Enabled;
            this.Unit3.SwitchStatus = Flag.Unit[2].Enabled;
            this.Unit4.SwitchStatus = Flag.Unit[3].Enabled;

            Flag.StartEnabled.RunCold.RefreshColdingState += StartEnabled_RefreshColdingState;

            Flag.InternalColdControl.ChartRefreshEnabled = true;

            Flag.SystemThread.InternalColdControlUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.InternalColdControlEnabled = true;
            Flag.SystemThread.InternalColdControlUpData.Start();
        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            while (Flag.SystemThread.InternalColdControlEnabled)
            {
                Thread.Sleep(100);
                this.Invoke((MethodInvoker)delegate
                {
                    #region 显示制冷按钮状态
                    this.ColdingEnabled.Checked = Flag.StartEnabled.RunCold.Colding;
                    #endregion

                    #region 刷新选择按钮状态
                    this.Unit1.SwitchStatus = Flag.Unit[0].Enabled;
                    this.Unit2.SwitchStatus = Flag.Unit[1].Enabled;
                    this.Unit3.SwitchStatus = Flag.Unit[2].Enabled;
                    this.Unit4.SwitchStatus = Flag.Unit[3].Enabled;
                    #endregion

                    #region 刷新液位与液体温度
                    this.WaterTemp.Value = float.Parse(Flag.WaterBox.Temp.Value.ToString("0.0"));
                    this.WaterHeight.Value =  float.Parse(Flag.WaterBox.Height.Value.ToString("0.0"));
                    this.WaterHeight.BottleTag = Flag.WaterBox.Height.Value.ToString("0.0") + "％";
                    #endregion

                    #region 刷新曲线参数
                    if (Flag.InternalColdControl.ChartRefreshEnabled == true)
                    {
                        Flag.InternalColdControl.ChartRefreshEnabled = false;

                        this.LevelTempChart.ChartAreas[0].AxisX.Maximum = Flag.InternalColdControl.ChartPointLength;
                        this.LevelTempChart.ChartAreas[0].AxisY.Maximum = Flag.InternalColdControl.ChartHightTemp;
                        this.LevelTempChart.ChartAreas[0].AxisY.Minimum = Flag.InternalColdControl.ChartLovTemp;

                        for (int i = 0; i < this.LevelTempChart.Series.Count; i++)
                        {
                            if (i < Flag.Unit.Length)
                            {
                                this.LevelTempChart.Series[i].ChartType = Flag.InternalColdControl.UnitChart[i].Type;
                                this.LevelTempChart.Series[i].Color = Flag.InternalColdControl.UnitChart[i].Color;
                            }
                            else if (i == this.LevelTempChart.Series.Count - 2)
                            {
                                this.LevelTempChart.Series[i].ChartType = Flag.InternalColdControl.SetTempChart.Type;
                                this.LevelTempChart.Series[i].Color = Flag.InternalColdControl.SetTempChart.Color;
                            }
                            else if (i == this.LevelTempChart.Series.Count - 1)
                            {
                                this.LevelTempChart.Series[i].ChartType = Flag.InternalColdControl.NowTempChart.Type;
                                this.LevelTempChart.Series[i].Color = Flag.InternalColdControl.NowTempChart.Color;
                            }
                        }

                    }
                    #endregion

                    #region 刷新一次温度曲线
                    if (Sw.ElapsedMilliseconds >= Flag.InternalColdControl.ChartRefreshTime * 1000)
                    {
                        int StartIndex = Function.Other.SetVable(Flag.WaterBox.Parameter.Count >= Flag.InternalColdControl.ChartPointLength, Flag.WaterBox.Parameter.Count - Flag.InternalColdControl.ChartPointLength, 0);

                        for (int i = 0; i < this.LevelTempChart.Series.Count; i++)
                        {
                            if (i < Flag.Unit.Length)
                            {
                                this.LevelTempChart.Series[i].Enabled = Flag.Unit[i].Enabled;
                                if (Flag.Unit[i].Enabled == true)
                                {
                                    this.LevelTempChart.Series[i].Points.Clear();
                                }
                            }
                            else if (i == this.LevelTempChart.Series.Count - 2)
                            {
                                this.LevelTempChart.Series[i].Enabled = true;
                                this.LevelTempChart.Series[i].Points.Clear();
                            }
                            else if (i == this.LevelTempChart.Series.Count - 1)
                            {
                                this.LevelTempChart.Series[i].Enabled = true;
                                this.LevelTempChart.Series[i].Points.Clear();
                            }
                        }

                        for (int i = StartIndex; i < Flag.WaterBox.Parameter.Count; i++)
                        {
                            for (int j = 0; j < this.LevelTempChart.Series.Count; j++)
                            {
                                if (j < Flag.Unit.Length && Flag.Unit[j].Enabled == true)
                                {
                                    FlagStruct.RecordBase Record = Flag.Unit[j].Evaporator.Out.Temp.GetRecordValue(i);
                                    this.LevelTempChart.Series[j].Points.AddXY(Record.Time, Record.Value);
                                }
                                else if (j == this.LevelTempChart.Series.Count - 2)
                                {
                                    FlagStruct.RecordBase Record = Flag.WaterBox.Parameter.GetRecordValue(i);
                                    this.LevelTempChart.Series[j].Points.AddXY(Record.Time, Record.Value);
                                }
                                else if (j == this.LevelTempChart.Series.Count - 1)
                                {
                                    FlagStruct.RecordBase Record = Flag.WaterBox.Temp.GetRecordValue(i);
                                    this.LevelTempChart.Series[j].Points.AddXY(Record.Time, Record.Value);
                                }
                            }
                        }

                        Sw.Restart();
                    }
                    #endregion
                });
            }
            Flag.SystemThread.InternalColdControlEndState = true;
        }

        private void SwitchStatusChange_Click(object sender, EventArgs e)
        {
            int EnableUnit = 0;

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                EnableUnit += Function.Other.Bool_To_Int(Flag.Unit[i].Enabled);
            }

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                if ((sender as HslControls.HslSwitch).Name == "Unit" + (i + 1).ToString())
                {
                    if (EnableUnit == 1 && Flag.Unit[i].Enabled == true && Flag.StartEnabled.RunCold.Colding == true)
                    {
                        MessageBox.Show(this, "制冷开启过程中，无法全部关闭制冷通道，如需停机请优先关闭制冷使能！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (Flag.Unit[i].Enabled != (sender as HslControls.HslSwitch).SwitchStatus)
                    {
                        Flag.Unit[i].Enabled = (sender as HslControls.HslSwitch).SwitchStatus;
                    }
                }
            }
            Flag.SaveData();
        }

        private void OpenChartSet_Click(object sender, EventArgs e)
        {
              InternalColdControlChart fm = new InternalColdControlChart();
              Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void OpenTempSet_Click(object sender, EventArgs e)
        {
            InternalTempChange fm = new InternalTempChange();
            Function.Win.OpenWindow(fm, this, fm.Text, false);
        }

        private void ColdingEnabled_MouseDown(object sender, MouseEventArgs e)
        {
            bool IsNoEn = false;
            this.ColdingEnabled.Checked = Flag.StartEnabled.RunCold.Colding;

            if (Work.Initial.SenserDetection(true) == false)
            {
                Function.Other.Message("系统自检未检测通过，详情请查看设备报警页面！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                IsNoEn = IsNoEn || Flag.Unit[i].Enabled;
            }

            if (IsNoEn == false)
            {
                Function.Other.Message("制冷通道全部处于关闭状态，无法启动制冷！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.ColdingEnabled.Checked = false;
                return;
            }

            if (Flag.WaterBox.Height.Value <= 0)
            {
                Function.Other.Message("载冷夜过低，无法启动制冷操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.ColdingEnabled.Checked = false;
                return;
            }

            if (Flag.StartEnabled.RunCold.Colding == true)
            {
                if (Win.MessageBox(this, "制冷系统已开启，是否确认关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                {
                    Win.OpenFrom(Win.FromWindows.AlmDisplay);
                    Flag.StartEnabled.RunCold.Colding = false;
                }
                else
                {
                    this.ColdingEnabled.Checked = true;
                }
            }
            else
            {
                if (Win.MessageBox(this, "制冷系统已关闭，是否确认开启？" + "\r\n" + "点击Yes确认开启." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                {
                    Win.OpenFrom(Win.FromWindows.AlmDisplay);
                    Flag.StartEnabled.RunCold.Colding = true;
                }
                else
                {
                    this.ColdingEnabled.Checked = false;
                }
            }
        }

        private void StartEnabled_RefreshColdingState(bool State)
        {
            //if (this.ColdingEnabled.InvokeRequired)
            //{
            //    this.ColdingEnabled.Invoke((MethodInvoker)delegate
            //    {
            //        this.ColdingEnabled.Checked = State;
            //    });
            //}
            //else
            //{
            //    this.ColdingEnabled.Checked = State;
            //}
        }
    }
}
