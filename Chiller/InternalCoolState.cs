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
    public partial class InternalColdState : Form
    {
        public InternalColdState()
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

        private void InternalColdState_Load(object sender, EventArgs e)
        {
            Flag.SystemThread.InternalColdStateUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.InternalColdStateEnabled = true;
            Flag.SystemThread.InternalColdStateUpData.Start();
        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            while (Flag.SystemThread.InternalColdStateEnabled)
            {
                Thread.Sleep(200);

                this.Invoke((MethodInvoker)delegate
                {
                    #region 显示各区域温度、泵转速、液位状态

                    this.WaterBox.Value = Flag.WaterBox.Height.Value;

                    this.WaterTemp.Text = Flag.WaterBox.Temp.Value.ToString("0.0") + "℃";
                    this.Unit1EvaporatorTemp.Text = Flag.Unit[0].Evaporator.Out.Temp.Value.ToString("0.0") + "℃";
                    this.Unit2EvaporatorTemp.Text = Flag.Unit[1].Evaporator.Out.Temp.Value.ToString("0.0") + "℃";
                    this.Unit3EvaporatorTemp.Text = Flag.Unit[2].Evaporator.Out.Temp.Value.ToString("0.0") + "℃";
                    this.Unit4EvaporatorTemp.Text = Flag.Unit[3].Evaporator.Out.Temp.Value.ToString("0.0") + "℃";

                    if (Flag.Unit[0].InternalPump.IsErr == false)
                    {
                        this.Unit1PumpSpeed.Text = Flag.Unit[0].InternalPump.Speed.ToString() + " r/min - " + Flag.Unit[0].InternalPump.OperatingLoad.ToString() + "％";
                    }
                    else
                    {
                        this.Unit1PumpSpeed.Text = "驱动器错误：" + Flag.Unit[0].InternalPump.ErrCode;
                    }

                    if (Flag.Unit[1].InternalPump.IsErr == false)
                    {
                        this.Unit2PumpSpeed.Text = Flag.Unit[1].InternalPump.Speed.ToString() + " r/min - " + Flag.Unit[1].InternalPump.OperatingLoad.ToString() + "％";
                    }
                    else
                    {
                        this.Unit2PumpSpeed.Text = "驱动器错误：" + Flag.Unit[1].InternalPump.ErrCode;
                    }

                    if (Flag.Unit[2].InternalPump.IsErr == false)
                    {
                        this.Unit3PumpSpeed.Text = Flag.Unit[2].InternalPump.Speed.ToString() + " r/min - " + Flag.Unit[2].InternalPump.OperatingLoad.ToString() + "％";
                    }
                    else
                    {
                        this.Unit3PumpSpeed.Text = "驱动器错误：" + Flag.Unit[2].InternalPump.ErrCode;
                    }

                    if (Flag.Unit[3].InternalPump.IsErr == false)
                    {
                        this.Unit4PumpSpeed.Text = Flag.Unit[3].InternalPump.Speed.ToString() + " r/min - " + Flag.Unit[3].InternalPump.OperatingLoad.ToString() + "％";
                    }
                    else
                    {
                        this.Unit4PumpSpeed.Text = "驱动器错误：" + Flag.Unit[3].InternalPump.ErrCode;
                    }



                    #endregion
                });

                if (Sw.ElapsedMilliseconds > 5000)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        #region 显示系统水箱与内部换热过程

                        this.BypassValve.EdgeColor = Function.Other.SetVable(Flag.WaterBox.InternalValve.State, Color.Green, Color.Red);

                        this.WaterBoxOut1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.WaterBoxOut1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.WaterBoxOut2.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.BypassValveOut1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.BypassValveOut2.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;

                        this.Unit1PumpInPipe1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit2PumpInPipe1.PipeLineActive = Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit3PumpInPipe1.PipeLineActive = Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit4PumpInPipe1.PipeLineActive = Flag.Unit[3].InternalPump.IsRun;

                        if (Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit1PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[0].InternalPump.IsRun)
                            {
                                this.Unit1PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.Down;
                            }
                            else
                            {
                                this.Unit1PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }
                        if (Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit2PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[1].InternalPump.IsRun)
                            {
                                this.Unit2PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.Down;
                            }
                            else
                            {
                                this.Unit2PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }
                        if (Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit3PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[2].InternalPump.IsRun)
                            {
                                this.Unit3PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.Down;
                            }
                            else
                            {
                                this.Unit3PumpInPipe1.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }

                        this.Unit1PumpInPipe2.PipeLineActive = Flag.Unit[0].InternalPump.IsRun;
                        this.Unit2PumpInPipe2.PipeLineActive = Flag.Unit[1].InternalPump.IsRun;
                        this.Unit3PumpInPipe2.PipeLineActive = Flag.Unit[2].InternalPump.IsRun;
                        this.Unit4PumpInPipe2.PipeLineActive = Flag.Unit[3].InternalPump.IsRun;

                        this.Unit1PumpOut.PipeLineActive = Flag.Unit[0].InternalPump.IsRun;
                        this.Unit2PumpOut.PipeLineActive = Flag.Unit[1].InternalPump.IsRun;
                        this.Unit3PumpOut.PipeLineActive = Flag.Unit[2].InternalPump.IsRun;
                        this.Unit4PumpOut.PipeLineActive = Flag.Unit[3].InternalPump.IsRun;

                        this.Unit1EvaporatorOut1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun;
                        this.Unit2EvaporatorOut1.PipeLineActive = Flag.Unit[1].InternalPump.IsRun;
                        this.Unit3EvaporatorOut1.PipeLineActive = Flag.Unit[2].InternalPump.IsRun;
                        this.Unit4EvaporatorOut1.PipeLineActive = Flag.Unit[3].InternalPump.IsRun;

                        this.Unit1EvaporatorOut2.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit2EvaporatorOut2.PipeLineActive = Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit3EvaporatorOut2.PipeLineActive = Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.Unit4EvaporatorOut2.PipeLineActive = Flag.Unit[3].InternalPump.IsRun;

                        if (Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit1EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[0].InternalPump.IsRun)
                            {
                                this.Unit1EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.Up;
                            }
                            else
                            {
                                this.Unit1EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }
                        if (Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit2EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[1].InternalPump.IsRun)
                            {
                                this.Unit2EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.Up;
                            }
                            else
                            {
                                this.Unit2EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }
                        if (Flag.Unit[3].InternalPump.IsRun)
                        {
                            this.Unit3EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                        }
                        else
                        {
                            if (Flag.Unit[2].InternalPump.IsRun)
                            {
                                this.Unit3EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.Up;
                            }
                            else
                            {
                                this.Unit3EvaporatorOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.None;
                            }
                        }

                        this.WaterBoxIn2.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;
                        this.WaterBoxIn1.PipeLineActive = Flag.Unit[0].InternalPump.IsRun || Flag.Unit[1].InternalPump.IsRun || Flag.Unit[2].InternalPump.IsRun || Flag.Unit[3].InternalPump.IsRun;

                        #endregion

                        #region 显示压缩机组1运行状态

                        this.Unit1CompressorOut.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CondenserOut.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CapillaryOut.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CompressorIn1.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CompressorIn2.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CompressorIn3.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;

                        #endregion

                        #region 显示压缩机组2运行状态

                        this.Unit2CompressorOut.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CondenserOut.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CapillaryOut.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CompressorIn1.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CompressorIn2.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CompressorIn3.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;

                        #endregion

                        #region 显示压缩机组3运行状态

                        this.Unit3CompressorOut.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CondenserOut.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CapillaryOut.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CompressorIn1.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CompressorIn2.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CompressorIn3.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;

                        #endregion

                        #region 显示压缩机组4运行状态

                        this.Unit4CompressorOut.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CondenserOut.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CapillaryOut.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CompressorIn1.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CompressorIn2.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CompressorIn3.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;

                        #endregion

                        #region 显示冷水机工作状况

                        this.CoolWaterOut1.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        this.CoolWaterOut2.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;
                        this.CoolWaterIn.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State || Flag.Unit[1].Compressor.RunAndStop.State || Flag.Unit[2].Compressor.RunAndStop.State || Flag.Unit[3].Compressor.RunAndStop.State;

                        this.Unit1CondenserWaterIn.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;
                        this.Unit1CondenserWaterOut.PipeLineActive = Flag.Unit[0].Compressor.RunAndStop.State;

                        this.Unit2CondenserWaterIn.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;
                        this.Unit2CondenserWaterOut.PipeLineActive = Flag.Unit[1].Compressor.RunAndStop.State;

                        this.Unit3CondenserWaterIn.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;
                        this.Unit3CondenserWaterOut.PipeLineActive = Flag.Unit[2].Compressor.RunAndStop.State;

                        this.Unit4CondenserWaterIn.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;
                        this.Unit4CondenserWaterOut.PipeLineActive = Flag.Unit[3].Compressor.RunAndStop.State;

                        #endregion
                    });
                }
            }
            Flag.SystemThread.InternalColdStateEndState = true;
        }
    }
}
