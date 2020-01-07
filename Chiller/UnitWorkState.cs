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
    public partial class UnitWorkState : Form
    {
        public UnitWorkState()
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

        private void UnitWorkState_Load(object sender, EventArgs e)
        {
            Flag.SystemThread.UnitWorkStateUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.UnitWorkStateEnabled = true;
            Flag.SystemThread.UnitWorkStateUpData.Start();
            Flag.UnitWorkState.ChartRefreshEnabled = true;
            RefreshSwitchStatus();
        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            while (Flag.SystemThread.UnitWorkStateEnabled)
            {
                Thread.Sleep(100);
             
                this.Invoke((MethodInvoker)delegate
                {
                    #region 刷新选定机组的压力与温度数据
                    this.CompressorPressureIn.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.In.Pressure.Value.ToString("0.00") + "Mpa";
                    this.CompressorPressureOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Pressure.Value.ToString("0.00") + "Mpa";
                    this.CondenserPressureIn.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.In.Pressure.Value.ToString("0.00") + "Mpa";
                    this.CondenserPressureOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.Out.Pressure.Value.ToString("0.00") + "Mpa";
                    this.EvaporatorPressureIn.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.In.Pressure.Value.ToString("0.00") + "Mpa";
                    this.EvaporatorPressureOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.Out.Pressure.Value.ToString("0.00") + "Mpa";
                    this.HeatExchangerPressureOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].HeatExchanger.Out.Pressure.Value.ToString("0.00") + "Mpa";

                    this.CompressorTempOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Temp.Value.ToString("0.0") + "℃";
                    this.CondenserTempOut.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.Out.Temp.Value.ToString("0.0") + "℃";
                    this.EvaporatorTempIn.Text = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.Out.Temp.Value.ToString("0.0") + "℃";
                    #endregion

                    #region 刷新压缩机组曲线参数
                    if (Flag.UnitWorkState.ChartRefreshEnabled == true)
                    {
                        Flag.UnitWorkState.ChartRefreshEnabled = false;

                        this.UnitChart.ChartAreas[0].AxisX.Maximum = Flag.UnitWorkState.ChartPointLength;

                        this.UnitChart.ChartAreas[0].AxisY.Maximum = Flag.UnitWorkState.ChartHightTemp;
                        this.UnitChart.ChartAreas[0].AxisY.Minimum = Flag.UnitWorkState.ChartLovTemp;

                        this.UnitChart.ChartAreas[0].AxisY2.Maximum = Flag.UnitWorkState.ChartHightPressure;
                        this.UnitChart.ChartAreas[0].AxisY2.Minimum = Flag.UnitWorkState.ChartLovPressure;

                        this.UnitChart.Series[0].Color = Flag.UnitWorkState.CompressorInPressureChart.Color;
                        this.UnitChart.Series[1].Color = Flag.UnitWorkState.CompressorOutPressureChart.Color;

                        this.UnitChart.Series[2].Color = Flag.UnitWorkState.CondenserInPressureChart.Color;
                        this.UnitChart.Series[3].Color = Flag.UnitWorkState.CondenserOutPressureChart.Color;

                        this.UnitChart.Series[4].Color = Flag.UnitWorkState.EvaporatorInPressureChart.Color;
                        this.UnitChart.Series[5].Color = Flag.UnitWorkState.EvaporatorOutPressureChart.Color;

                        this.UnitChart.Series[6].Color = Flag.UnitWorkState.HeatExchangerOutPressureChart.Color;

                        this.UnitChart.Series[7].Color = Flag.UnitWorkState.CompressorOutTempChart.Color;
                        this.UnitChart.Series[8].Color = Flag.UnitWorkState.CondenserOutTempChart.Color;
                        this.UnitChart.Series[9].Color = Flag.UnitWorkState.EvaporatorInTempChart.Color;

                        this.UnitChart.Series[0].ChartType = Flag.UnitWorkState.CompressorInPressureChart.Type;
                        this.UnitChart.Series[1].ChartType = Flag.UnitWorkState.CompressorOutPressureChart.Type;

                        this.UnitChart.Series[2].ChartType = Flag.UnitWorkState.CondenserInPressureChart.Type;
                        this.UnitChart.Series[3].ChartType = Flag.UnitWorkState.CondenserOutPressureChart.Type;

                        this.UnitChart.Series[4].ChartType = Flag.UnitWorkState.EvaporatorInPressureChart.Type;
                        this.UnitChart.Series[5].ChartType = Flag.UnitWorkState.EvaporatorOutPressureChart.Type;

                        this.UnitChart.Series[6].ChartType = Flag.UnitWorkState.HeatExchangerOutPressureChart.Type;

                        this.UnitChart.Series[7].ChartType = Flag.UnitWorkState.CompressorOutTempChart.Type;
                        this.UnitChart.Series[8].ChartType = Flag.UnitWorkState.CondenserOutTempChart.Type;
                        this.UnitChart.Series[9].ChartType = Flag.UnitWorkState.EvaporatorInTempChart.Type;
                    }
                    #endregion

                    #region 刷新一次曲线，包含温度和压力
                    if (Sw.ElapsedMilliseconds >= Flag.UnitWorkState.ChartRefreshTime * 1000)
                    {
                        for (int i = 0; i < this.UnitChart.Series.Count; i++)
                        {
                            this.UnitChart.Series[i].Points.Clear();
                        }

                        int StartIndex = Function.Other.SetVable(Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Pressure.Count >= Flag.UnitWorkState.ChartPointLength, Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Pressure.Count - Flag.UnitWorkState.ChartPointLength, 0);

                        for (int i = StartIndex; i < Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Pressure.Count; i++)
                        {
                            FlagStruct.RecordBase CompressorPressureIn = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.In.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[0].Points.AddXY(CompressorPressureIn.Time, CompressorPressureIn.Value);

                            FlagStruct.RecordBase CompressorPressureOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[1].Points.AddXY(CompressorPressureOut.Time, CompressorPressureOut.Value);

                            FlagStruct.RecordBase CondenserPressureIn = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.In.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[2].Points.AddXY(CondenserPressureIn.Time, CondenserPressureIn.Value);

                            FlagStruct.RecordBase CondenserPressureOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.Out.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[3].Points.AddXY(CondenserPressureOut.Time, CondenserPressureOut.Value);

                            FlagStruct.RecordBase EvaporatorPressureIn = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.In.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[4].Points.AddXY(EvaporatorPressureIn.Time, EvaporatorPressureIn.Value);

                            FlagStruct.RecordBase EvaporatorPressureOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.Out.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[5].Points.AddXY(EvaporatorPressureOut.Time, EvaporatorPressureOut.Value);

                            FlagStruct.RecordBase HeatExchangerPressureOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].HeatExchanger.Out.Pressure.GetRecordValue(i);
                            this.UnitChart.Series[6].Points.AddXY(HeatExchangerPressureOut.Time, HeatExchangerPressureOut.Value);

                            FlagStruct.RecordBase CompressorTempOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.Out.Temp.GetRecordValue(i);
                            this.UnitChart.Series[7].Points.AddXY(CompressorTempOut.Time, CompressorTempOut.Value);

                            FlagStruct.RecordBase CondenserTempOut = Flag.Unit[Flag.UnitWorkState.UnitIndex].Condenser.Out.Temp.GetRecordValue(i);
                            this.UnitChart.Series[8].Points.AddXY(CondenserTempOut.Time, CondenserTempOut.Value);

                            FlagStruct.RecordBase EvaporatorTempIn = Flag.Unit[Flag.UnitWorkState.UnitIndex].Evaporator.Out.Temp.GetRecordValue(i);
                            this.UnitChart.Series[9].Points.AddXY(EvaporatorTempIn.Time, EvaporatorTempIn.Value);
                        }

                        Sw.Restart();
                    }
                    #endregion
                });
                if (Sw.ElapsedMilliseconds >= 2000)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        #region 显示冷水机运行状态
                        this.CoolWaterIn.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        this.CoolWaterOut1.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        this.CoolWaterOut2.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        this.CoolWaterOut3.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        #endregion

                        #region 显示压缩机组运行状态
                        this.BypassValve.EdgeColor = Function.Other.SetVable(Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State, Color.Green, Color.Red);
                        this.ParallelValve.EdgeColor = Function.Other.SetVable(Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State, Color.Green, Color.Red);

                        this.CompressorOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.CondenserOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.HeatExchangerOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.EvaporatorInPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.EvaporatorOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.CompressorInPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;

                        this.BranchPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State;
                        this.BranchPipe.Visible = Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State;

                        this.BypassInPipe1.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State;
                        this.BypassInPipe2.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State;
                        this.BypassOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State;
                        this.BypassCapillaryOut1.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State;
                        this.BypassCapillaryOut2.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].BypassValve.State;

                        this.ParallelInPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State;
                        this.ParallelOutPipe.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State;
                        this.ParallelCapillaryOutPipe1.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State;
                        this.ParallelCapillaryOutPipe2.PipeLineActive = Flag.Unit[Flag.UnitWorkState.UnitIndex].Compressor.RunAndStop.State && Flag.Unit[Flag.UnitWorkState.UnitIndex].ParallelValve.State;

                        #endregion
                    });
                }
            }
            Flag.SystemThread.UnitWorkStateEndState = true;
        }

        private void SwitchStatusChange_Click(object sender, EventArgs e)
        {
            if ((sender as HslControls.HslSwitch).Name == "Unit1")
            {
                Flag.UnitWorkState.UnitIndex = 0;
            }
            else if ((sender as HslControls.HslSwitch).Name == "Unit2")
            {
                Flag.UnitWorkState.UnitIndex = 1;
            }
            else if ((sender as HslControls.HslSwitch).Name == "Unit3")
            {
                Flag.UnitWorkState.UnitIndex = 2;
            }
            else if ((sender as HslControls.HslSwitch).Name == "Unit4")
            {
                Flag.UnitWorkState.UnitIndex = 3;
            }
            RefreshSwitchStatus();

            Flag.SaveData();
        }

        private void RefreshSwitchStatus()
        {
            if (Flag.UnitWorkState.UnitIndex == 0)
            {
                this.Unit1.SwitchStatus = true;
                this.Unit2.SwitchStatus = false;
                this.Unit3.SwitchStatus = false;
                this.Unit4.SwitchStatus = false;
            }
            else if (Flag.UnitWorkState.UnitIndex == 1)
            {
                this.Unit1.SwitchStatus = false;
                this.Unit2.SwitchStatus = true;
                this.Unit3.SwitchStatus = false;
                this.Unit4.SwitchStatus = false;
            }
            else if (Flag.UnitWorkState.UnitIndex == 2)
            {
                this.Unit1.SwitchStatus = false;
                this.Unit2.SwitchStatus = false;
                this.Unit3.SwitchStatus = true;
                this.Unit4.SwitchStatus = false;
            }
            else if (Flag.UnitWorkState.UnitIndex == 3)
            {
                this.Unit1.SwitchStatus = false;
                this.Unit2.SwitchStatus = false;
                this.Unit3.SwitchStatus = false;
                this.Unit4.SwitchStatus = true;
            }
        }

        private void OpenChartSet_Click(object sender, EventArgs e)
        {
            UnitWorkStateChart fm = new UnitWorkStateChart();
            Function.Win.OpenWindow(fm, this, fm.Text, false);
        }


    }
}
