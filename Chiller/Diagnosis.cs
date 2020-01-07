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
    public partial class Diagnosis : Form
    {
        public Diagnosis()
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

        private void Diagnosis_Load(object sender, EventArgs e)
        {
            this.InternalPumpSpeed1.Value = Flag.Diagnosis.InternalPumpSpeed1;
            this.InternalPumpSpeed2.Value = Flag.Diagnosis.InternalPumpSpeed2;
            this.InternalPumpSpeed3.Value = Flag.Diagnosis.InternalPumpSpeed3;
            this.InternalPumpSpeed4.Value = Flag.Diagnosis.InternalPumpSpeed4;

            this.ExternalPumpSpeed1.Value = Flag.Diagnosis.ExternalPumpSpeed1;
            this.ExternalPumpSpeed2.Value = Flag.Diagnosis.ExternalPumpSpeed2;
            this.ExternalPumpSpeed3.Value = Flag.Diagnosis.ExternalPumpSpeed3;

            this.InternalPumpSpeed1.Maximum = Flag.Manufacturer.InternalPump.OneHightSpeed;
            this.InternalPumpSpeed2.Maximum = Flag.Manufacturer.InternalPump.OneHightSpeed;
            this.InternalPumpSpeed3.Maximum = Flag.Manufacturer.InternalPump.OneHightSpeed;
            this.InternalPumpSpeed4.Maximum = Flag.Manufacturer.InternalPump.OneHightSpeed;

            Flag.SystemThread.DiagnosisUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.DiagnosisEnabled = true;
            Flag.SystemThread.DiagnosisUpData.Start();
        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            while (Flag.SystemThread.DiagnosisEnabled)
            {
                Thread.Sleep(200);
                this.Invoke((MethodInvoker)delegate
                {
                    #region 压缩机组相关
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        CheckBox Company = Function.Other.FindControl(this, "Company" + (i + 1).ToString()) as CheckBox;

                        Label COMP_P_In = Function.Other.FindControl(this, "COMP_P_In_" + (i + 1).ToString()) as Label;
                        Label COMP_P_Out = Function.Other.FindControl(this, "COMP_P_Out_" + (i + 1).ToString()) as Label;
                        Label COND_P_In = Function.Other.FindControl(this, "COND_P_In_" + (i + 1).ToString()) as Label;
                        Label COND_P_Out = Function.Other.FindControl(this, "COND_P_Out_" + (i + 1).ToString()) as Label;
                        Label EVAP_P_In = Function.Other.FindControl(this, "EVAP_P_In_" + (i + 1).ToString()) as Label;
                        Label EVAP_P_Out = Function.Other.FindControl(this, "EVAP_P_Out_" + (i + 1).ToString()) as Label;
                        Label REG_P_Out = Function.Other.FindControl(this, "REG_P_Out_" + (i + 1).ToString()) as Label;

                        Label NowExhaustTemp = Function.Other.FindControl(this, "NowExhaustTemp_" + (i + 1).ToString()) as Label;
                        Label NowCondenserTemp = Function.Other.FindControl(this, "NowCondenserTemp_" + (i + 1).ToString()) as Label;
                        Label NowEvaporatorTemp = Function.Other.FindControl(this, "NowEvaporatorTemp_" + (i + 1).ToString()) as Label;

                        Label NowInternalPumpSpeed = Function.Other.FindControl(this, "InternalPumpSpeed_" + (i + 1).ToString()) as Label;
                        Label NowInternalPumpLoad = Function.Other.FindControl(this, "InternalPumpLoad_" + (i + 1).ToString()) as Label;
                        Label NowInternalPumpTemp = Function.Other.FindControl(this, "InternalPumpTemp_" + (i + 1).ToString()) as Label;
                        Label NowInternalPumpState = Function.Other.FindControl(this, "InternalPumpState_" + (i + 1).ToString()) as Label;

                        HslControls.HslButton Compressor = Function.Other.FindControl(this, "Compressor" + (i + 1).ToString()) as HslControls.HslButton;
                        HslControls.HslButton Bypass = Function.Other.FindControl(this, "Bypass" + (i + 1).ToString()) as HslControls.HslButton;
                        HslControls.HslButton Parallel = Function.Other.FindControl(this, "Parallel" + (i + 1).ToString()) as HslControls.HslButton;
                        HslControls.HslButton PumpStart = Function.Other.FindControl(this, "InternalPump" + (i + 1).ToString()) as HslControls.HslButton;

                        if (Company.Checked == false)
                        {
                            COMP_P_In.Text = Flag.Unit[i].Compressor.In.Pressure.Value.ToString("0.00") + " Mpa";
                            COMP_P_Out.Text = Flag.Unit[i].Compressor.Out.Pressure.Value.ToString("0.00") + " Mpa";

                            COND_P_In.Text = Flag.Unit[i].Condenser.In.Pressure.Value.ToString("0.00") + " Mpa";
                            COND_P_Out.Text = Flag.Unit[i].Condenser.Out.Pressure.Value.ToString("0.00") + " Mpa";

                            EVAP_P_In.Text = Flag.Unit[i].Evaporator.In.Pressure.Value.ToString("0.00") + " Mpa";
                            EVAP_P_Out.Text = Flag.Unit[i].Evaporator.Out.Pressure.Value.ToString("0.00") + " Mpa";

                            REG_P_Out.Text = Flag.Unit[i].HeatExchanger.Out.Pressure.Value.ToString("0.00") + " Mpa";

                            NowExhaustTemp.Text = Flag.Unit[i].Compressor.Out.Temp.Value.ToString("0.00") + " ℃";
                            NowCondenserTemp.Text = Flag.Unit[i].Condenser.Out.Temp.Value.ToString("0.00") + " ℃";
                            NowEvaporatorTemp.Text = Flag.Unit[i].Evaporator.Out.Temp.Value.ToString("0.00") + " ℃";
                        }
                        else
                        {
                            COMP_P_In.Text = Flag.Unit[i].Compressor.In.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Compressor.In.Pressure.AnalogCompany;
                            COMP_P_Out.Text = Flag.Unit[i].Compressor.Out.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Compressor.Out.Pressure.AnalogCompany;

                            COND_P_In.Text = Flag.Unit[i].Condenser.In.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Condenser.In.Pressure.AnalogCompany;
                            COND_P_Out.Text = Flag.Unit[i].Condenser.Out.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Condenser.Out.Pressure.AnalogCompany;

                            EVAP_P_In.Text = Flag.Unit[i].Evaporator.In.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Evaporator.In.Pressure.AnalogCompany;
                            EVAP_P_Out.Text = Flag.Unit[i].Evaporator.Out.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].Evaporator.Out.Pressure.AnalogCompany;

                            REG_P_Out.Text = Flag.Unit[i].HeatExchanger.Out.Pressure.Analog.ToString("0.0000") + Flag.Unit[i].HeatExchanger.Out.Pressure.AnalogCompany;

                            NowExhaustTemp.Text = Flag.Unit[i].Compressor.Out.Temp.Analog.ToString("0.0000") + Flag.Unit[i].Compressor.Out.Temp.AnalogCompany;
                            NowCondenserTemp.Text = Flag.Unit[i].Condenser.Out.Temp.Analog.ToString("0.0000") + Flag.Unit[i].Condenser.Out.Temp.AnalogCompany;
                            NowEvaporatorTemp.Text = Flag.Unit[i].Evaporator.Out.Temp.Analog.ToString("0.0000") + Flag.Unit[i].Evaporator.Out.Temp.AnalogCompany;
                        }

                        Function.Other.SetBColor(Flag.Unit[i].Compressor.RunAndStop.State, Color.Blue, Color.LemonChiffon, ref Compressor);
                        Function.Other.SetBColor(Flag.Unit[i].BypassValve.State, Color.Blue, Color.LemonChiffon, ref Bypass);
                        Function.Other.SetBColor(Flag.Unit[i].ParallelValve.State, Color.Blue, Color.LemonChiffon, ref Parallel);
                        Function.Other.SetBColor(Flag.Unit[i].InternalPump.IsRun, Color.Blue, Color.LemonChiffon, ref PumpStart);

                        Function.Other.SetActiveColor(Flag.Unit[i].Compressor.RunAndStop.State, Color.Blue, Color.LemonChiffon, ref Compressor);
                        Function.Other.SetActiveColor(Flag.Unit[i].BypassValve.State, Color.Blue, Color.LemonChiffon, ref Bypass);
                        Function.Other.SetActiveColor(Flag.Unit[i].ParallelValve.State, Color.Blue, Color.LemonChiffon, ref Parallel);
                        Function.Other.SetActiveColor(Flag.Unit[i].InternalPump.IsRun, Color.Blue, Color.LemonChiffon, ref PumpStart);

                        NowInternalPumpSpeed.Text = Flag.Unit[i].InternalPump.Speed.ToString() + " r/min";
                        NowInternalPumpLoad.Text = Flag.Unit[i].InternalPump.OperatingLoad.ToString() + " ％";
                        NowInternalPumpTemp.Text = Flag.Unit[i].InternalPump.EncoderTemp.ToString() + "℃";
                        Function.Other.SetText(Flag.Unit[i].InternalPump.IsErr, "Err：" + Flag.Unit[i].InternalPump.ErrCode, "Normal", ref NowInternalPumpState);

                        Function.Other.SetFColor(Flag.Unit[i].InternalPump.IsErr, Color.Red, Color.Green, ref NowInternalPumpState);

                    }
                    #endregion

                    #region 整机相关数据显示
                    this.ExternalPumpSpeed_1.Text = Flag.ExternalPump[0].Speed.ToString() + " r/min";
                    this.ExternalPumpSpeed_2.Text = Flag.ExternalPump[1].Speed.ToString() + " r/min";
                    this.ExternalPumpSpeed_3.Text = Flag.ExternalPump[2].Speed.ToString() + " r/min";


                    this.ExternalPumpLoad_1.Text = Flag.ExternalPump[0].OperatingLoad.ToString() + " ％";
                    this.ExternalPumpLoad_2.Text = Flag.ExternalPump[1].OperatingLoad.ToString() + " ％";
                    this.ExternalPumpLoad_3.Text = Flag.ExternalPump[2].OperatingLoad.ToString() + " ％";

                    this.ExternalPumpTemp_1.Text = Flag.ExternalPump[0].EncoderTemp.ToString() + "℃";
                    this.ExternalPumpTemp_2.Text = Flag.ExternalPump[1].EncoderTemp.ToString() + "℃";
                    this.ExternalPumpTemp_3.Text = Flag.ExternalPump[2].EncoderTemp.ToString() + "℃";

                    Function.Other.SetText(Flag.ExternalPump[0].IsErr, "Err：" + Flag.ExternalPump[0].ErrCode, "Normal", ref this.ExternalPumpState_1);
                    Function.Other.SetText(Flag.ExternalPump[1].IsErr, "Err：" + Flag.ExternalPump[1].ErrCode, "Normal", ref this.ExternalPumpState_2);
                    Function.Other.SetText(Flag.ExternalPump[2].IsErr, "Err：" + Flag.ExternalPump[2].ErrCode, "Normal", ref this.ExternalPumpState_3);

                    Function.Other.SetFColor(Flag.ExternalPump[0].IsErr, Color.Red, Color.Green, ref this.ExternalPumpState_1);
                    Function.Other.SetFColor(Flag.ExternalPump[1].IsErr, Color.Red, Color.Green, ref this.ExternalPumpState_2);
                    Function.Other.SetFColor(Flag.ExternalPump[2].IsErr, Color.Red, Color.Green, ref this.ExternalPumpState_3);

                    Function.Other.SetBColor(Flag.WaterBox.InternalValve.State, Color.Blue, Color.LemonChiffon, ref InternalValve);
                    Function.Other.SetBColor(Flag.WaterBox.ExternalValve.State, Color.Blue, Color.LemonChiffon, ref ExternalValve);
                    Function.Other.SetBColor(Flag.ExternalPump[0].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump1);
                    Function.Other.SetBColor(Flag.ExternalPump[1].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump2);
                    Function.Other.SetBColor(Flag.ExternalPump[2].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump3);

                    Function.Other.SetActiveColor(Flag.WaterBox.InternalValve.State, Color.Blue, Color.LemonChiffon, ref InternalValve);
                    Function.Other.SetActiveColor(Flag.WaterBox.ExternalValve.State, Color.Blue, Color.LemonChiffon, ref ExternalValve);
                    Function.Other.SetActiveColor(Flag.ExternalPump[0].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump1);
                    Function.Other.SetActiveColor(Flag.ExternalPump[1].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump2);
                    Function.Other.SetActiveColor(Flag.ExternalPump[2].IsRun, Color.Blue, Color.LemonChiffon, ref ExternalPump3);

                    this.TestArm1Valve.SwitchStatus = Flag.TestArm[0].Valve.State;
                    this.TestArm2Valve.SwitchStatus = Flag.TestArm[1].Valve.State;
                    this.TestArm3Valve.SwitchStatus = Flag.TestArm[2].Valve.State;
                    this.TestArm4Valve.SwitchStatus = Flag.TestArm[3].Valve.State;
                    this.TestArm5Valve.SwitchStatus = Flag.TestArm[4].Valve.State;
                    this.TestArm6Valve.SwitchStatus = Flag.TestArm[5].Valve.State;
                    this.TestArm7Valve.SwitchStatus = Flag.TestArm[6].Valve.State;
                    this.TestArm8Valve.SwitchStatus = Flag.TestArm[7].Valve.State;
                    this.ColdPlate1Valve.SwitchStatus = Flag.ColdPlate[0].Valve.State;
                    this.ColdPlate2Valve.SwitchStatus = Flag.ColdPlate[1].Valve.State;

                    this.Level_Height.Text = Flag.WaterBox.Height.Value.ToString("0.0") + " ％";
                    this.Level_Temp.Text = Flag.WaterBox.Temp.Value.ToString("0.00") + " ℃";

                    #endregion
                });
            }
            Flag.SystemThread.DiagnosisEndState = true;
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Flag.Unit.Length; i++)
            {

                if (Flag.StartEnabled.RunCold.Colding == true && Flag.Unit[i].Enabled == true)
                {
                    return;
                }

                #region 压缩机启动或停止
                if ((sender as HslControls.HslButton).Name == "Compressor" + (i + 1).ToString())
                {
                    Work.IO.Set_DO(Flag.Unit[i].Compressor.RunAndStop.Address, !Flag.Unit[i].Compressor.RunAndStop.State);
                }
                #endregion

                #region 旁通电磁阀打开或关闭
                else if ((sender as HslControls.HslButton).Name == "Bypass" + (i + 1).ToString())
                {
                    Work.IO.Set_DO(Flag.Unit[i].BypassValve.Address, !Flag.Unit[i].BypassValve.State);
                }
                #endregion

                #region 并通电磁阀打开或关闭
                else if ((sender as HslControls.HslButton).Name == "Parallel" + (i + 1).ToString())
                {
                    Work.IO.Set_DO(Flag.Unit[i].ParallelValve.Address, !Flag.Unit[i].ParallelValve.State);
                }
                #endregion

                #region 内循环泵启动或停止
                else if ((sender as HslControls.HslButton).Name == "InternalPump" + (i + 1).ToString())
                {
                    Work.Pump.PauseRead = true;
                    bool IsRun = false; ;
                    if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref IsRun))
                    {
                        if (IsRun == true)
                        {
                            Work.Pump.Stop(Flag.Unit[i].InternalPump.Address);
                        }
                        else if (IsRun == false)
                        {
                            if (Flag.WaterBox.InternalValve.State == false)
                            {
                                if (Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, true) == false)
                                {
                                    Work.Pump.PauseRead = false;
                                    return;
                                }
                            }

                            NumericUpDown Speed = Function.Other.FindControl(this, "InternalPumpSpeed" + (i + 1).ToString()) as NumericUpDown;
                            CheckBox Reverse = Function.Other.FindControl(this, "InternalPumpReverse" + (i + 1).ToString()) as CheckBox;

                            if (Reverse.Checked == false)
                            {
                                Work.Pump.REV(Flag.Unit[i].InternalPump.Address, (ushort)Speed.Value);
                            }
                            else if (Reverse.Checked == true)
                            {
                                Work.Pump.FWD(Flag.Unit[i].InternalPump.Address, (ushort)Speed.Value);
                            }
                        }
                    }
                    Work.Pump.PauseRead = false;
                }
                #endregion

            }
        }

        private void WaterBox_Click(object sender, EventArgs e)
        {
            #region 内通道电磁阀
            if ((sender as HslControls.HslButton).Name == "InternalValve")
            {
                bool IsRun = false;
                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit[i].InternalPump.IsRun == true)
                    {
                        IsRun = true;
                        break;
                    }
                }
                if (IsRun == false)
                {
                    Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, !Flag.WaterBox.InternalValve.State);
                }
                else
                {
                    Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, true);
                }
            }
            #endregion

            #region 外通道电磁阀
            else if ((sender as HslControls.HslButton).Name == "ExternalValve")
            {
                bool IsRun = false;
                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Flag.ExternalPump[i].IsRun == true)
                    {
                        IsRun = true;
                        break;
                    }
                }
                if (IsRun == false)
                {
                    Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, !Flag.WaterBox.ExternalValve.State);
                }
                else
                {
                    Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, true);
                }
            }
            #endregion

            #region 外循环泵
            else
            {
                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if ((sender as HslControls.HslButton).Name == "ExternalPump" + (i + 1).ToString())
                    {
                        Work.Pump.PauseRead = true;
                        bool IsRun = false; ;
                        if (Work.Pump.IsRun(Flag.ExternalPump[i].Address, ref IsRun))
                        {
                            if (IsRun == true)
                            {
                                Work.Pump.Stop(Flag.ExternalPump[i].Address);
                            }
                            else if (IsRun == false)
                            {
                                if (Flag.WaterBox.ExternalValve.State == false)
                                {
                                    if (Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, true) == false)
                                    {
                                        Work.Pump.PauseRead = false;
                                        return;
                                    }
                                }

                                NumericUpDown Speed = Function.Other.FindControl(this, "ExternalPumpSpeed" + (i + 1).ToString()) as NumericUpDown;
                                CheckBox Reverse = Function.Other.FindControl(this, "ExternalPumpReverse" + (i + 1).ToString()) as CheckBox;

                                switch (i + 1)
                                {
                                    case 1:

                                        if (Flag.TestArm[1].Valve.State == false && Flag.TestArm[3].Valve.State == false &&
                                            Flag.TestArm[5].Valve.State == false && Flag.TestArm[7].Valve.State == false)
                                        {
                                            MessageBox.Show(this, "测试头液体通道处于全部关闭状态无法启动外循环！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        break;

                                    case 2:

                                        if (Flag.ColdPlate[0].Valve.State == false && Flag.ColdPlate[1].Valve.State == false)
                                        {
                                            MessageBox.Show(this, "预冷盘液体通道处于全部关闭状态无法启动外循环！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        break;

                                    case 3:

                                        if (Flag.TestArm[0].Valve.State == false && Flag.TestArm[2].Valve.State == false &&
                                            Flag.TestArm[4].Valve.State == false && Flag.TestArm[6].Valve.State == false)
                                        {
                                            MessageBox.Show(this, "测试头液体通道处于全部关闭状态无法启动外循环！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

                                        break;
                                }

                                if (Reverse.Checked == false)
                                {
                                    Work.Pump.REV(Flag.ExternalPump[i].Address, (ushort)Speed.Value);
                                }
                                else if (Reverse.Checked == true)
                                {
                                    Work.Pump.FWD(Flag.ExternalPump[i].Address, (ushort)Speed.Value);
                                }
                            }
                        }
                        Work.Pump.PauseRead = false;

                    }
                }
            }
            #endregion
        }

        private void In_Put_Int(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Int(sender, e);
        }

        private void In_Put_Float(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Float(sender, e);
        }

        private void In_Put_UInt(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void STL_Valve(object sender, MouseEventArgs e)
        {
            int OnNumber = 0;
            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                if ((sender as HslControls.HslSwitch).Name == "TestArm" + (i + 1).ToString() + "Valve")
                {
                    if (i == 0 || i == 2 || i == 4 || i == 6)
                    {
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[0].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[2].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[4].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[6].Valve.State);
                        if (Flag.ExternalPump[2].IsRun == true && OnNumber == 1 && Function.Other.Bool_To_Int(Flag.TestArm[i].Valve.State) == 1)
                        {
                            MessageBox.Show(this, "测试头液体循环中，无法关闭当前电磁阀，如需关闭请优先停止当前通道循环泵！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[1].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[3].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[5].Valve.State);
                        OnNumber += Function.Other.Bool_To_Int(Flag.TestArm[7].Valve.State);
                        if (Flag.ExternalPump[0].IsRun == true && OnNumber == 1 && Function.Other.Bool_To_Int(Flag.TestArm[i].Valve.State) == 1)
                        {
                            MessageBox.Show(this, "测试头液体循环中，无法关闭当前电磁阀，如需关闭请优先停止当前通道循环泵！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    Work.IO.Set_DO(Flag.TestArm[i].Valve.Address, !Flag.TestArm[i].Valve.State);
                    return;
                }
            }
            for (int i = 0; i < Flag.ColdPlate.Length; i++)
            {
                if ((sender as HslControls.HslSwitch).Name == "ColdPlate" + (i + 1).ToString() + "Valve")
                {
                    OnNumber += Function.Other.Bool_To_Int(Flag.ColdPlate[0].Valve.State);
                    OnNumber += Function.Other.Bool_To_Int(Flag.ColdPlate[1].Valve.State);

                    if (Flag.ExternalPump[1].IsRun == true && OnNumber == 1 && Function.Other.Bool_To_Int(Flag.ColdPlate[i].Valve.State) == 1)
                    {
                        MessageBox.Show(this, "测试头液体循环中，无法关闭全部电磁阀！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Work.IO.Set_DO(Flag.ColdPlate[i].Valve.Address, !Flag.ColdPlate[i].Valve.State);
                    return;
                }
            }
        }

        private void ValueChange(object sender, EventArgs e)
        {
            if ((sender as NumericUpDown).Name == "InternalPumpSpeed1")
                Flag.Diagnosis.InternalPumpSpeed1 = (uint)this.InternalPumpSpeed1.Value;
            if ((sender as NumericUpDown).Name == "InternalPumpSpeed2")
                Flag.Diagnosis.InternalPumpSpeed2 = (uint)this.InternalPumpSpeed2.Value;
            if ((sender as NumericUpDown).Name == "InternalPumpSpeed3")
                Flag.Diagnosis.InternalPumpSpeed3 = (uint)this.InternalPumpSpeed3.Value;
            if ((sender as NumericUpDown).Name == "InternalPumpSpeed4")
                Flag.Diagnosis.InternalPumpSpeed4 = (uint)this.InternalPumpSpeed4.Value;

            if ((sender as NumericUpDown).Name == "ExternalPumpSpeed1")
                Flag.Diagnosis.ExternalPumpSpeed1 = (uint)this.ExternalPumpSpeed1.Value;
            if ((sender as NumericUpDown).Name == "ExternalPumpSpeed2")
                Flag.Diagnosis.ExternalPumpSpeed2 = (uint)this.ExternalPumpSpeed2.Value;
            if ((sender as NumericUpDown).Name == "ExternalPumpSpeed3")
                Flag.Diagnosis.ExternalPumpSpeed3 = (uint)this.ExternalPumpSpeed3.Value;

            Flag.SaveData();
        }
    }
}
