using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Chiller
{
    /// <summary>
    /// 温度控制系统
    /// </summary>
    public class TempControl
    {
        /// <summary>
        /// 制冷控制
        /// </summary>
        public class Colding
        {
            private static Thread ColdThread;

            private static Thread PumpThread;
            /// <summary>
            /// 初始化内部制冷
            /// </summary>
            public static void Initialization()
            {
                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    ColdThread = new Thread(ColdUnit) { IsBackground = true };
                    ColdThread.Start((object)i);
                }
                PumpThread = new Thread(StartInsidePump) { IsBackground = true };
                PumpThread.Start();
            }
            /// <summary>
            /// 压缩机启动与停止锁，将不能同时开启压缩机
            /// </summary>
            private static object LockValue = 0;

            public static void ColdUnit(object l)
            {
                int Unit = (int)l;
                int Index = 0;
                bool LovEvaporatorTemp = false;
                bool ValueChange1 = false;              //电磁阀更改状态时判断1
                bool ValueChange2 = false;              //电磁阀更改状态时判断2
                Stopwatch ChangeTime = new Stopwatch(); //电磁阀更改时间为10s
                Stopwatch RecoveryTempTime = new Stopwatch();  //超出最低温度需要进行回温一段时间到指定的温度

                Stopwatch RestartTime = new Stopwatch();

                int LastStartTime = 0;

                int RestartNum = 0;
                while (true)
                {
                    Thread.Sleep(100);

                    switch (Index)
                    {
                        case 0:

                            if (Flag.StartEnabled.RunCold.Colding == true && Flag.Unit[Unit].Enabled == true)
                            {
                                Flag.Unit[Unit].Compressor.Out.Temp.HightTempGrade = false;

                                Flag.Unit[Unit].Condenser.Out.Temp.HightTempGrade = false;

                                Flag.Unit[Unit].Compressor.Out.Pressure.HightPressureGrade = false;

                                RestartNum = 0;

                                LastStartTime = 0;

                                Index = 1;

                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "制冷正在启动！", Color.Black);
                            }

                            break;

                        case 1:

                            if (Flag.StartEnabled.RunCold.Colding == false || Flag.Unit[Unit].Enabled == false)
                            {
                                Index = 20;
                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "关闭中！", Color.Black);
                            }

                            #region 系统状态检查，是否符合开启状态
                            if (Flag.Unit[Unit].Enabled == false)
                            {
                                if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                                {
                                    SetCompressorState(Unit, false);
                                }
                                continue;
                            }

                            if (CheckUnitSenser(Unit) == false)
                            {
                                if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                                {
                                    SetCompressorState(Unit, false);
                                }
                                continue;
                            }

                            if (CheckUnitTemp(Unit) == false)
                            {
                                if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                                {
                                    SetCompressorState(Unit, false);
                                }
                                continue;
                            }

                            if (CheckUnitPressure(Unit) == false)
                            {
                                if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                                {
                                    SetCompressorState(Unit, false);
                                }
                                continue;
                            }

                            if (Flag.WaterBox.InternalValve.State == false)
                            {
                                Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, true);
                                continue;
                            }

                            #endregion

                            #region 检测循环泵是否报警

                            //检测循环泵是否异常
                            if (Flag.Unit[Unit].InternalPump.IsErr == true)
                            {
                                for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                                {
                                    if (Flag.Unit[Unit].InternalPump.ErrCode == PanasonicA6.A6_ErrCodeList[j].Code && PanasonicA6.A6_ErrCodeList[j].IsClear == false)
                                    {
                                        Flag.Unit[Unit].Enabled = false;
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "循环泵报警，且无法自动清除，ErrCode：" + Flag.Unit[Unit].InternalPump.ErrCode, Color.Red);
                                        Index = 20;
                                    }
                                }
                            }

                            #endregion

                            if (Flag.Unit[Unit].Compressor.RunAndStop.State == false)
                            {
                                #region 压缩机未启动，将控制压缩机开启
                                if (Flag.Unit[Unit].BypassValve.State == false)
                                {
                                    Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);     //打开旁通阀
                                }
                                if (Flag.Unit[Unit].ParallelValve.State == false)
                                {
                                    Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);     //打开并通阀
                                }
                                if (Flag.Unit[Unit].BypassValve.State == true && Flag.Unit[Unit].ParallelValve.State == true)
                                {
                                    if (Flag.Unit[Unit].Compressor.RunAndStop.IntervalTime.IsRunning == true)
                                    {
                                        if (Flag.Unit[Unit].Compressor.RunAndStop.IntervalTime.ElapsedMilliseconds >= 600000)
                                        {
                                            if (SetCompressorState(Unit, true) == true)
                                            {
                                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "已启动！", Color.Green);
                                            }
                                            else
                                            {
                                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "启动失败！", Color.Red);
                                            }
                                        }
                                        else
                                        {
                                            int DelayTime = (int)(Flag.Unit[Unit].Compressor.RunAndStop.IntervalTime.ElapsedMilliseconds / 1000 / 60);

                                            if (LastStartTime != DelayTime)
                                            {
                                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "启动还需" + (10 - DelayTime).ToString() + "min！", Color.Red);
                                            }
                                        }
                                    }
                                    else if (Flag.Unit[Unit].Compressor.RunAndStop.IntervalTime.IsRunning == false)
                                    {
                                        if (SetCompressorState(Unit, true) == true)
                                        {
                                            Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "已启动！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "启动失败！", Color.Red);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                Flag.StartEnabled.RunCold.IsStop = false;

                                #region 压缩机已启动，检测系统温度与压力，以及循环泵等

                                //排气温度超极限
                                if (Flag.Unit[Unit].Compressor.Out.Temp.Value >= Flag.Manufacturer.CompressorSystemData.HightExhaustTemp)
                                {
                                    Flag.Unit[Unit].Enabled = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "超过极限排气温度，已自动关闭当前通道！", Color.Red);
                                    Flag.CompressorErr = true;
                                    Index = 20;
                                }
                                //冷凝温度超极限
                                if (Flag.Unit[Unit].Condenser.Out.Temp.Value >= Flag.Manufacturer.CompressorSystemData.HightCondenserTemp)
                                {
                                    Flag.Unit[Unit].Enabled = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "超过极限冷凝温度，已自动关闭当前通道！", Color.Red);
                                    Flag.CompressorErr = true;
                                    Index = 20;
                                }

                                //排气压力超极限
                                if (Flag.Unit[Unit].Compressor.Out.Pressure.Value >= Flag.Manufacturer.CompressorSystemData.HightOutPressure)
                                {
                                    Flag.Unit[Unit].Enabled = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "超过极限排气压力，已自动关闭当前通道！", Color.Red);
                                    Flag.CompressorErr = true;
                                    Index = 20;
                                }


                                //排气温度超限
                                if (Flag.Unit[Unit].Compressor.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.ExhaustOverrun)
                                {
                                    if (RestartNum < 2)
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "排气温度超限，已暂停运行当前通道，将自动恢复！", Color.Red);
                                        Flag.Unit[Unit].Compressor.Out.Temp.HightTempGrade = true;
                                        Index = 10;
                                    }
                                    else
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "排气温度超限，当前通道重启次数已达上限，将自动关闭当前通道！", Color.Red);
                                        Flag.Unit[Unit].Enabled = false;
                                        Flag.CompressorErr = true;
                                        Index = 20;
                                    }
                                }

                                //冷凝温度超限
                                if (Flag.Unit[Unit].Condenser.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.CondensateOverrun)
                                {
                                    if (RestartNum < 2)
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "冷凝温度超限，已暂停运行当前通道，将自动恢复！", Color.Red);
                                        Flag.Unit[Unit].Condenser.Out.Temp.HightTempGrade = true;
                                        Index = 10;
                                    }
                                    else
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "冷凝温度超限，当前通道重启次数已达上限，将自动关闭当前通道！", Color.Red);
                                        Flag.Unit[Unit].Enabled = false;
                                        Flag.CompressorErr = true;
                                        Index = 20;
                                    }
                                }
                                //排气压力超限
                                if (Flag.Unit[Unit].Compressor.Out.Pressure.Value >= Flag.Manufacturer.CompressorSystemData.HightOutPressure)
                                {
                                    if (RestartNum < 2)
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "排气压力超限，已暂停运行当前通道，将自动恢复！", Color.Red);
                                        Flag.Unit[Unit].Compressor.Out.Pressure.HightPressureGrade = true;
                                        Index = 10;
                                    }
                                    else
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "冷凝温度超限，当前通道重启次数已达上限，将自动关闭当前通道！", Color.Red);
                                        Flag.Unit[Unit].Enabled = false;
                                        Flag.CompressorErr = true;
                                        Index = 20;
                                    }
                                }
                                #endregion

                                #region 压缩机已启动，电磁阀动作，单次动作切换时间为10s
                                if (ChangeTime.IsRunning == false)
                                {
                                    ChangeTime.Restart();
                                }

                                if (ChangeTime.IsRunning == true && ChangeTime.ElapsedMilliseconds >= 10000)
                                {
                                    if (LovEvaporatorTemp == false)
                                    {
                                        #region 快速降温区间，打开旁通和并通
                                        if (Flag.Unit[Unit].Evaporator.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.RapidColding)
                                        {
                                            if (ValueChange1 == false)
                                            {
                                                if (Flag.WaterBox.Parameter.SetTemp <= Flag.Manufacturer.CompressorColdData.DeepColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                else if (Flag.WaterBox.Parameter.SetTemp > Flag.Manufacturer.CompressorColdData.DeepColding && Flag.WaterBox.Parameter.SetTemp <= Flag.Manufacturer.CompressorColdData.RapidColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                else if (Flag.WaterBox.Parameter.SetTemp > Flag.Manufacturer.CompressorColdData.RapidColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                ValueChange1 = false;
                                            }
                                            else
                                            {
                                                if (Flag.Unit[Unit].Evaporator.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.RapidColding + 5)
                                                {
                                                    ValueChange1 = false;
                                                }
                                            }

                                            ValueChange2 = false;
                                        }
                                        #endregion

                                        #region 持续快速降温区间，打开并通，关闭旁通
                                        else if (Flag.Unit[Unit].Evaporator.Out.Temp.Value < Flag.Manufacturer.CompressorColdData.RapidColding && Flag.Unit[Unit].Evaporator.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.DeepColding)
                                        {
                                            if (ValueChange2 == false)
                                            {
                                                if (Flag.WaterBox.Parameter.SetTemp <= Flag.Manufacturer.CompressorColdData.DeepColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == true)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, false);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                else if (Flag.WaterBox.Parameter.SetTemp > Flag.Manufacturer.CompressorColdData.DeepColding && Flag.WaterBox.Parameter.SetTemp <= Flag.Manufacturer.CompressorColdData.RapidColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == true)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, false);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                else if (Flag.WaterBox.Parameter.SetTemp > Flag.Manufacturer.CompressorColdData.RapidColding)
                                                {
                                                    if (Flag.Unit[Unit].BypassValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                                    }
                                                    if (Flag.Unit[Unit].ParallelValve.State == false)
                                                    {
                                                        Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                    }
                                                }
                                                ValueChange2 = false;
                                            }
                                            else
                                            {
                                                if (Flag.Unit[Unit].Evaporator.Out.Temp.Value < Flag.Manufacturer.CompressorColdData.RapidColding && Flag.Unit[Unit].Evaporator.Out.Temp.Value >= Flag.Manufacturer.CompressorColdData.DeepColding + 5)
                                                {
                                                    ValueChange2 = false;
                                                }
                                            }

                                            ValueChange1 = true;
                                        }
                                        #endregion

                                        #region 深度降温区间，关闭并通，关闭旁通
                                        else if (Flag.Unit[Unit].Evaporator.Out.Temp.Value < Flag.Manufacturer.CompressorColdData.DeepColding && Flag.Unit[Unit].Evaporator.Out.Temp.Value >= Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp)
                                        {
                                            if (Flag.WaterBox.Parameter.SetTemp <= Flag.Manufacturer.CompressorColdData.DeepColding)
                                            {
                                                if (Flag.Unit[Unit].BypassValve.State == true)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, false);
                                                }
                                                if (Flag.Unit[Unit].ParallelValve.State == true)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, false);
                                                }
                                            }
                                            else if (Flag.WaterBox.Parameter.SetTemp > Flag.Manufacturer.CompressorColdData.DeepColding && Flag.WaterBox.Parameter.SetTemp < Flag.Manufacturer.CompressorColdData.RapidColding)
                                            {
                                                if (Flag.Unit[Unit].BypassValve.State == true)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, false);
                                                }
                                                if (Flag.Unit[Unit].ParallelValve.State == false)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                }
                                            }
                                            else if (Flag.WaterBox.Parameter.SetTemp >= Flag.Manufacturer.CompressorColdData.RapidColding)
                                            {
                                                if (Flag.Unit[Unit].BypassValve.State == false)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                                }
                                                if (Flag.Unit[Unit].ParallelValve.State == false)
                                                {
                                                    Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                                }
                                            }

                                            ValueChange1 = false;
                                            ValueChange2 = true;
                                        }
                                        #endregion

                                        #region 超出设定的最低蒸发温度
                                        else if (Flag.Unit[Unit].Evaporator.Out.Temp.Value < Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp)
                                        {
                                            if (Flag.Unit[Unit].BypassValve.State == false)
                                            {
                                                Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);
                                            }
                                            if (Flag.Unit[Unit].ParallelValve.State == false)
                                            {
                                                Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);
                                            }
                                            if (Flag.Unit[Unit].BypassValve.State == true && Flag.Unit[Unit].ParallelValve.State == true)
                                            {
                                                LovEvaporatorTemp = true;
                                                ValueChange1 = false;
                                                ValueChange2 = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (Flag.Unit[Unit].Evaporator.Out.Temp.Value < Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp)
                                        {
                                            RecoveryTempTime.Restart();
                                        }
                                        if (RecoveryTempTime.ElapsedMilliseconds >= 30000)
                                        {
                                            LovEvaporatorTemp = false;
                                            RecoveryTempTime.Stop();
                                        }
                                    }
                                    ChangeTime.Restart();
                                }
                                #endregion
                            }

                            break;

                        case 10:

                            if (Flag.StartEnabled.RunCold.Colding == false)
                            {
                                Index = 20;
                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "关闭中！", Color.Black);

                            }

                            if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                            {
                                SetCompressorState(Unit, false);
                            }
                            else
                            {
                                if (Flag.Unit[Unit].Compressor.Out.Temp.Value < 40 && Flag.Unit[Unit].Compressor.Out.Temp.HightTempGrade == true)
                                {
                                    Flag.Unit[Unit].Compressor.Out.Temp.HightTempGrade = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "排气温度恢复！", Color.Green);
                                }

                                if (Flag.Unit[Unit].Condenser.Out.Temp.Value < 30 && Flag.Unit[Unit].Condenser.Out.Temp.HightTempGrade == true)
                                {
                                    Flag.Unit[Unit].Condenser.Out.Temp.HightTempGrade = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "冷凝温度恢复！", Color.Green);
                                }
                                if (Flag.Unit[Unit].Compressor.Out.Pressure.Value < 1.5 && Flag.Unit[Unit].Compressor.Out.Pressure.HightPressureGrade == true)
                                {
                                    Flag.Unit[Unit].Compressor.Out.Pressure.HightPressureGrade = false;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "排气压力恢复！", Color.Green);
                                }

                                if (Flag.Unit[Unit].Compressor.Out.Temp.HightTempGrade == false && Flag.Unit[Unit].Condenser.Out.Temp.HightTempGrade == false && Flag.Unit[Unit].Compressor.Out.Pressure.HightPressureGrade == false)
                                {
                                    if (RestartTime.IsRunning == false)
                                    {
                                        RestartTime.Restart();
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "系统恢复完成，等待" + Flag.Manufacturer.CompressorColdData.RestartTime.ToString() + "后重新开启！", Color.Black);
                                    }
                                }
                            }
                            if (RestartTime.ElapsedMilliseconds >= Flag.Manufacturer.CompressorColdData.RestartTime * 1000)
                            {
                                Index = 1;
                                RestartNum++;
                                RestartTime.Stop();
                                Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "系统已重启！", Color.Green);
                            }
                            break;

                        case 20:

                            #region 关闭压缩机

                            LastStartTime = 0;

                            if (Flag.Unit[Unit].BypassValve.State == false)
                            {
                                Work.IO.Set_DO(Flag.Unit[Unit].BypassValve.Address, true);     //打开旁通阀
                            }
                            if (Flag.Unit[Unit].ParallelValve.State == false)
                            {
                                Work.IO.Set_DO(Flag.Unit[Unit].ParallelValve.Address, true);     //打开并通阀
                            }

                            if (Flag.Unit[Unit].BypassValve.State == true && Flag.Unit[Unit].ParallelValve.State == true)
                            {
                                if (Flag.Unit[Unit].Compressor.RunAndStop.State == true)
                                {
                                    if (SetCompressorState(Unit, false) == false)
                                    {
                                        Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "关闭失败,等待10s后重试！", Color.Red);
                                        Thread.Sleep(10000);
                                    }
                                }
                                else
                                {
                                    Index = 0;
                                    Flag.RunLogList.Add("压缩机组" + (Unit + 1).ToString() + "关闭成功！", Color.Green);
                                }
                            }
                            #endregion

                            break;
                    }
                }
            }

            public static bool SetCompressorState(int UnitIndex, bool RunOrStop)
            {
                lock (LockValue)
                {
                    bool TimeOutState = false;
                    Stopwatch TimeOut = new Stopwatch();
                    TimeOut.Restart();
                    do
                    {
                        Work.IO.Set_DO(Flag.Unit[UnitIndex].Compressor.RunAndStop.Address, RunOrStop);
                        if (TimeOut.ElapsedMilliseconds >= 5000)
                        {
                            TimeOutState = true;
                        }
                        Thread.Sleep(10);
                    }
                    while (Flag.Unit[UnitIndex].Compressor.RunAndStop.State != RunOrStop && TimeOutState == false);

                    if (TimeOutState == false)
                    {
                        Thread.Sleep(10000);
                        if (RunOrStop == false)
                        {
                            Flag.Unit[UnitIndex].Compressor.RunAndStop.IntervalTime.Restart();
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            private static void StartInsidePump()
            {

                int StartIndex = 0;

                int Index = 0;

                int ChangePump = 0;                         //更改起始泵的地址
                int ChangeContinuityErr = 0;                //连续变更失败

                int StartNum = 0;                           //启动泵的总数
                int LastPumpNum = 0;                        //上次泵启动的总和
                double LastTemp = 0;                        //上次液体的温度

                int HightSpeed;                             //用于计算泵用的最高速度
                double AllSpeedDiffer = 0;                  //用于计算泵的总和增益值

                double TotalTemp = 0;                       //蒸发器温度总和，参与计算各个泵的运行速度
                double TotaGainSpeed = 0;                   //厂商增益速度总和，参与计算各个泵的运行速度                  

                bool PumpSpeedChange = false;
                ushort[] UnitSpeed = new ushort[4] { 0, 0, 0, 0 };                    //计算完成各泵的当前速度
                bool[] UnitRun = new bool[4] { false, false, false, false };    //记录内循环泵是否需要启动

                bool IsTempControl = false;                                     //预转动时间是否满足，进行控制

                Stopwatch DelayPumpRun = new Stopwatch();                       //泵初次启动，慢速抽取液体，防止泵腔无液体（慢速抽取时间）
                DelayPumpRun.Start();

                Stopwatch ChangeDelaytime = new Stopwatch();                          //泵速度更改时间，用于比较设定的时间(稳定温度)
                ChangeDelaytime.Start();

                Flag.SystemThread.InsidePumpEnabled = true;
                while (true)
                {
                    Thread.Sleep(10);

                    switch (StartIndex)
                    {
                        case 0:

                            if (Flag.StartEnabled.RunCold.Colding == true)
                            {
                                StartIndex = 10;
                            }
                            break;

                        case 10:

                            if (Flag.StartEnabled.RunCold.Colding == false)
                            {
                                StartIndex = 20;
                            }

                            #region 预降温

                            if (Flag.WaterBox.Temp.Value > Flag.Manufacturer.InternalPump.PreRotationTemp)
                            {
                                #region 泵进行预转动，保证液体全部进入泵腔
                                for (int i = 0; i < Flag.Unit.Length; i++)
                                {
                                    if (Flag.Unit[i].InternalPump.IsErr == true && Flag.Unit[i].InternalPump.ErrCode == "94.0")
                                    {
                                        Work.Pump.ClearErr(Flag.Unit[i].InternalPump.Address);
                                        break;
                                    }

                                    if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsRun) == false)
                                    {
                                        break;
                                    }

                                    if (Flag.Unit[i].Compressor.RunAndStop.State == true && Flag.Unit[i].InternalPump.IsRun == false)
                                    {
                                        Work.Pump.REV(Flag.Unit[i].InternalPump.Address, Flag.Manufacturer.InternalPump.PreRotationSpeed);
                                    }
                                    else if (Flag.Unit[i].Compressor.RunAndStop.State == false && Flag.Unit[i].InternalPump.IsRun == true)
                                    {
                                        Work.Pump.Stop(Flag.Unit[i].InternalPump.Address);
                                    }
                                }
                                #endregion

                                IsTempControl = false;
                                DelayPumpRun.Stop();
                                DelayPumpRun.Reset();
                            }
                            else if (Flag.WaterBox.Temp.Value <= Flag.Manufacturer.InternalPump.PreRotationTemp && DelayPumpRun.IsRunning == false && IsTempControl == false)
                            {
                                DelayPumpRun.Restart();
                            }
                            if (Flag.WaterBox.Temp.Value <= Flag.Manufacturer.InternalPump.PreRotationTemp && DelayPumpRun.ElapsedMilliseconds >= Flag.Manufacturer.InternalPump.PreRotationTime * 1000 && IsTempControl == false)
                            {
                                Index = 0;
                                DelayPumpRun.Stop();
                                IsTempControl = true;
                            }

                            #endregion

                            #region 开始温度控制，道道指定的温度

                            if (IsTempControl == false)
                            {
                                break;
                            }
                            switch (Index)
                            {
                                case 0:

                                    #region 内循环泵控制

                                    StartNum = 0;

                                    #region 获取或设定泵用于计算的最高转速
                                    if (Flag.Manufacturer.InternalPump.HightSpeed != 0)
                                    {
                                        HightSpeed = Flag.Manufacturer.InternalPump.HightSpeed;
                                    }
                                    else
                                    {
                                        HightSpeed = 4200;
                                    }
                                    #endregion

                                    #region 检查符合运行的泵数量
                                    for (int i = 0; i < Flag.Unit.Length; i++)
                                    {
                                        if (Flag.Unit[i].Compressor.RunAndStop.State == true && Flag.Unit[i].Evaporator.Out.Temp.Value < (Flag.WaterBox.Temp.Value + Flag.Manufacturer.InternalPump.EvaporationTempDiffer))
                                        {
                                            StartNum++;
                                            UnitRun[i] = true;
                                        }
                                        else
                                        {
                                            UnitRun[i] = false;
                                        }
                                    }
                                    #endregion

                                    #region 检查报警状态与清除可消报警
                                    for (int i = 0; i < Flag.Unit.Length; i++)
                                    {
                                        if (Flag.Unit[i].InternalPump.IsErr == true)
                                        {
                                            for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                                            {
                                                if (Flag.Unit[i].InternalPump.ErrCode == PanasonicA6.A6_ErrCodeList[j].Code && PanasonicA6.A6_ErrCodeList[j].IsClear == true)
                                                {
                                                    Work.Pump.ClearErr(Flag.Unit[i].InternalPump.Address);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (StartNum > 0)
                                    {
                                        #region 推算总的增益系数
                                        AllSpeedDiffer = 0;
                                        for (int i = 0; i < UnitRun.Length; i++)
                                        {
                                            if (UnitRun[i] == true)
                                            {
                                                AllSpeedDiffer += Flag.Manufacturer.InternalPump.PumpSpeedDiffer[i] / 100f;
                                            }
                                        }
                                        #endregion

                                        #region 推算蒸发器温度总和
                                        TotalTemp = 0;
                                        for (int i = 0; i < UnitRun.Length; i++)
                                        {
                                            if (UnitRun[i] == true)
                                            {
                                                TotalTemp += Flag.Unit[i].Evaporator.Out.Temp.Value;
                                            }
                                        }
                                        #endregion

                                        #region 判断更新速度条件是否满足
                                        PumpSpeedChange = false;
                                        for (int i = 0; i < Flag.Unit.Length; i++)
                                        {
                                            if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsRun) == true)
                                            {
                                                if (UnitRun[i] == true && Flag.Unit[i].InternalPump.IsRun == false)
                                                {
                                                    PumpSpeedChange = true;
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion

                                        #region 快速降温，使用最高转速
                                        if ((Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp + Flag.WaterBox.Parameter.SetRange + (Flag.WaterBox.Parameter.SetRange * 0.5f))) > 0)
                                        {
                                            if (ChangeDelaytime.ElapsedMilliseconds >= Flag.Manufacturer.InternalPump.SpeedCorrectTime * 1000 || StartNum != LastPumpNum || Math.Abs(Flag.WaterBox.Temp.Value - LastTemp) >= (Flag.Manufacturer.InternalPump.LevelChange * 50) || PumpSpeedChange == true)
                                            {
                                                #region 推算各个轴的增益速度总和

                                                TotaGainSpeed = (HightSpeed / StartNum) * AllSpeedDiffer;

                                                #endregion

                                                for (int i = 0; i < UnitRun.Length; i++)
                                                {
                                                    if (UnitRun[i] == true)
                                                    {
                                                        UnitSpeed[i] = (ushort)((HightSpeed - TotaGainSpeed) * (Flag.Unit[i].Evaporator.Out.Temp.Value / TotalTemp));             //获取以蒸发器温度为基础计算出来的初始速度

                                                        if (AllSpeedDiffer != 0)
                                                        {
                                                            UnitSpeed[i] += (ushort)(TotaGainSpeed * ((Flag.Manufacturer.InternalPump.PumpSpeedDiffer[i] / 100f) / AllSpeedDiffer));   //再附加以厂商增益的补偿速度
                                                        }

                                                        if (UnitSpeed[i] > Flag.Manufacturer.InternalPump.OneHightSpeed)
                                                        {
                                                            UnitSpeed[i] = Flag.Manufacturer.InternalPump.OneHightSpeed;
                                                        }
                                                        else if (UnitSpeed[i] <= 10)
                                                        {
                                                            UnitSpeed[i] = 10;
                                                        }
                                                    }
                                                }

                                                Index = 10;
                                                LastPumpNum = StartNum;
                                                LastTemp = Flag.WaterBox.Temp.Value;
                                                ChangeDelaytime.Restart();
                                            }
                                        }
                                        #endregion

                                        #region 进入温度范围，进行速度调节1

                                        else if ((Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp + Flag.WaterBox.Parameter.SetRange + (Flag.WaterBox.Parameter.SetRange * 0.5f))) <= 0 && (Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp + (Flag.WaterBox.Parameter.SetRange * 0.5f))) > 0)
                                        {
                                            if (ChangeDelaytime.ElapsedMilliseconds >= Flag.Manufacturer.InternalPump.SpeedCorrectTime * 1000 || StartNum != LastPumpNum || Math.Abs(Flag.WaterBox.Temp.Value - LastTemp) >= Flag.Manufacturer.InternalPump.LevelChange || PumpSpeedChange == true)
                                            {
                                                double TotalSpeed = 0;           //总和的速度
                                                double LevelDiffer1 = 0;         //计算温度差,当前温度与设定温度的差值
                                                double LevelDiffer2 = 0;         //计算温度差,当前设定温度与最低温度的差值,最高减少1.5倍速度

                                                LevelDiffer1 = (Flag.WaterBox.Parameter.SetRange + (Flag.WaterBox.Parameter.SetRange * 0.5f)) / (Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp - (Flag.WaterBox.Parameter.SetRange * 0.5f)));       //计算当前差值
                                                LevelDiffer2 = ((0.5f / Flag.Manufacturer.CompressorSystemData.SystemLovTemp) * Flag.WaterBox.Parameter.SetTemp) + 1;

                                                if (LevelDiffer1 < 1)
                                                {
                                                    LevelDiffer1 = 1;
                                                }

                                                if (LevelDiffer2 < 1)
                                                {
                                                    LevelDiffer2 = 1;
                                                }

                                                TotalSpeed = (HightSpeed / LevelDiffer1 / LevelDiffer2);

                                                TotaGainSpeed = (TotalSpeed / StartNum) * AllSpeedDiffer;

                                                for (int i = 0; i < UnitRun.Length; i++)
                                                {
                                                    if (UnitRun[i] == true)
                                                    {
                                                        UnitSpeed[i] = (ushort)((TotalSpeed - TotaGainSpeed) * (Flag.Unit[i].Evaporator.Out.Temp.Value / TotalTemp));               //获取以蒸发器温度为基础计算出来的初始速度

                                                        if (AllSpeedDiffer != 0)
                                                        {
                                                            UnitSpeed[i] += (ushort)(TotaGainSpeed * ((Flag.Manufacturer.InternalPump.PumpSpeedDiffer[i] / 100f) / AllSpeedDiffer));   //再附加以厂商增益的补偿速度
                                                        }

                                                        if (UnitSpeed[i] > Flag.Manufacturer.InternalPump.OneHightSpeed)
                                                        {
                                                            UnitSpeed[i] = Flag.Manufacturer.InternalPump.OneHightSpeed;
                                                        }
                                                        else if (UnitSpeed[i] <= 10)
                                                        {
                                                            UnitSpeed[i] = 10;
                                                        }
                                                    }
                                                }
                                                Index = 10;
                                                LastPumpNum = StartNum;
                                                LastTemp = Flag.WaterBox.Temp.Value;
                                                ChangeDelaytime.Restart();
                                            }
                                        }

                                        #endregion

                                        #region 进入温度范围，进行速度调节2

                                        else if ((Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp + (Flag.WaterBox.Parameter.SetRange * 0.5f))) <= 0 && (Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp - (Flag.WaterBox.Parameter.SetRange * 0.5f))) > 0)
                                        {
                                            if (ChangeDelaytime.ElapsedMilliseconds >= Flag.Manufacturer.InternalPump.SpeedCorrectTime * 1000 || StartNum != LastPumpNum || Math.Abs(Flag.WaterBox.Temp.Value - LastTemp) >= Flag.Manufacturer.InternalPump.LevelChange || PumpSpeedChange == true)
                                            {
                                                double TotalSpeed = 0;          //总的泵运行速度
                                                double LevelDiffer1 = 0;        //计算温度差,当前温度与设定温度的差值
                                                double LevelDiffer2 = 0;        //计算温度差,当前设定温度与最低温度的差值,最高减少1.5倍速度
                                                double GroupDiffer = 0;         //加入机组蒸发温度比列
                                                double HalfSpeed = 1.2;

                                                LevelDiffer1 = (Flag.WaterBox.Parameter.SetRange + (Flag.WaterBox.Parameter.SetRange * 0.5f)) / (Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp - (Flag.WaterBox.Parameter.SetRange * 0.5f)));       //计算当前液体相差温度
                                                LevelDiffer2 = ((0.5f / Flag.Manufacturer.CompressorSystemData.SystemLovTemp) * Flag.WaterBox.Parameter.SetTemp) + 1;

                                                if (TotalTemp < StartNum * Flag.WaterBox.Parameter.SetTemp)
                                                {
                                                    GroupDiffer = TotalTemp / (StartNum * Flag.WaterBox.Parameter.SetTemp);      //计算当前蒸发器与液体相差温度
                                                }

                                                if (LevelDiffer1 < 1)
                                                {
                                                    LevelDiffer1 = 1;
                                                }

                                                if (LevelDiffer2 < 1)
                                                {
                                                    LevelDiffer2 = 1;
                                                }

                                                if (GroupDiffer < 1)
                                                {
                                                    GroupDiffer = 1;
                                                }

                                                TotalSpeed = HightSpeed / LevelDiffer1 / LevelDiffer2 / GroupDiffer / HalfSpeed;

                                                TotaGainSpeed = (TotalSpeed / StartNum) * AllSpeedDiffer;

                                                for (int i = 0; i < UnitRun.Length; i++)
                                                {
                                                    if (UnitRun[i] == true)
                                                    {
                                                        UnitSpeed[i] = (ushort)((TotalSpeed - TotaGainSpeed) * (Flag.Unit[i].Evaporator.Out.Temp.Value / TotalTemp));               //获取以蒸发器温度为基础计算出来的初始速度

                                                        if (AllSpeedDiffer != 0)
                                                        {
                                                            UnitSpeed[i] += (ushort)(TotaGainSpeed * ((Flag.Manufacturer.InternalPump.PumpSpeedDiffer[i] / 100f) / AllSpeedDiffer));   //再附加以厂商增益的补偿速度
                                                        }

                                                        if (UnitSpeed[i] > Flag.Manufacturer.InternalPump.OneHightSpeed)
                                                        {
                                                            UnitSpeed[i] = Flag.Manufacturer.InternalPump.OneHightSpeed;
                                                        }
                                                        else if (UnitSpeed[i] <= 10)
                                                        {
                                                            UnitSpeed[i] = 10;
                                                        }
                                                    }
                                                }
                                                Index = 10;
                                                LastPumpNum = StartNum;
                                                LastTemp = Flag.WaterBox.Temp.Value;
                                                ChangeDelaytime.Restart();
                                            }
                                        }

                                        #endregion

                                        #region 设定温度超限，将停止循环泵
                                        else if ((Flag.WaterBox.Temp.Value - (Flag.WaterBox.Parameter.SetTemp - (Flag.WaterBox.Parameter.SetRange * 0.5f))) <= 0)
                                        {
                                            LastTemp = Flag.WaterBox.Temp.Value;
                                            Index = 20;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        Index = 20;
                                    }
                                    #endregion

                                    break;

                                case 10:

                                    #region 开启内循环泵电磁阀

                                    if (Flag.WaterBox.InternalValve.State == false)
                                    {
                                        Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, true);     //打开内循环泵出液口电磁阀
                                        continue;
                                    }

                                    #endregion

                                    #region 内循环泵速度更新
                                    bool IsChangeOk = true;

                                    for (int i = ChangePump; i < Flag.Unit.Length; i++)
                                    {
                                        #region 压缩机组开启，且泵通讯正常准备更新速度

                                        if (Flag.Unit[i].Compressor.RunAndStop.State == true && Flag.Unit[i].InternalPump.IsConnent == true)
                                        {
                                            #region 查询报警状态
                                            if (Work.Pump.IsErr(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsErr) == false)
                                            {
                                                IsChangeOk = false;
                                                ChangePump = i;
                                                break;
                                            }
                                            #endregion

                                            #region 未报警
                                            if (Flag.Unit[i].InternalPump.IsErr == false)
                                            {
                                                #region 机组正常运行
                                                if (UnitRun[i] == true)
                                                {
                                                    Work.Pump.PauseRead = true;

                                                    #region 机组未存在报警，进行速度更新
                                                    if (Flag.Unit[i].Compressor.Out.Temp.HightTempGrade == false && Flag.Unit[i].Condenser.Out.Temp.HightTempGrade == false)
                                                    {
                                                        if (Work.Pump.REV(Flag.Unit[i].InternalPump.Address, UnitSpeed[i]) == true)
                                                        {
                                                            Flag.Unit[i].InternalPump.Speed = UnitSpeed[i];
                                                            Flag.Unit[i].InternalPump.IsRun = true;
                                                        }
                                                        else
                                                        {
                                                            IsChangeOk = false;
                                                            ChangePump = i;
                                                            Work.Pump.PauseRead = false;
                                                            break;
                                                        }
                                                    }
                                                    #endregion

                                                    #region 机组存在报警，停止循环泵运行
                                                    else
                                                    {
                                                        if (Work.Pump.Stop(Flag.Unit[i].InternalPump.Address) == true)
                                                        {
                                                            Flag.Unit[i].InternalPump.IsRun = false;
                                                        }
                                                        else
                                                        {
                                                            IsChangeOk = false;
                                                            ChangePump = i;
                                                            Work.Pump.PauseRead = false;
                                                            break;
                                                        }
                                                    }
                                                    #endregion

                                                    Work.Pump.PauseRead = false;
                                                }
                                                #endregion

                                                #region 机组异常运行或未运行
                                                else if (UnitRun[i] == false)
                                                {
                                                    if (Work.Pump.Stop(Flag.Unit[i].InternalPump.Address) == true)
                                                    {
                                                        Flag.Unit[i].InternalPump.IsRun = false;
                                                    }
                                                    else
                                                    {
                                                        IsChangeOk = false;
                                                        ChangePump = i;
                                                        break;
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            #region 报警中
                                            else
                                            {
                                                #region 报警中，是否可清除
                                                if (Flag.Unit[i].InternalPump.IsErrClear == true)
                                                {
                                                    if (Work.Pump.ClearErr(Flag.Unit[i].InternalPump.Address) == true)
                                                    {
                                                        IsChangeOk = false;
                                                        Index = 10;
                                                        ChangePump = i;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        ChangeContinuityErr++;

                                                        if (ChangeContinuityErr >= 6)
                                                        {
                                                            ChangeContinuityErr = 0;

                                                            Flag.Unit[i].Enabled = false;

                                                            Flag.RunLogList.Add("ColdUnit" + (i + 1).ToString() + "：循环泵处于报警状态，且状态消除失败，已关闭当前制冷机组！", Color.Orange);

                                                        }
                                                        else
                                                        {
                                                            IsChangeOk = false;
                                                            Index = 10;
                                                            ChangePump = i;
                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region 报警中，不可清除，停止压缩机工作
                                                else if (Flag.Unit[i].InternalPump.IsErrClear == false)
                                                {
                                                    Flag.Unit[i].Enabled = false;

                                                    Flag.RunLogList.Add("ColdUnit" + (i + 1).ToString() + "：循环泵处于报警状态，且状态无法消除，已关闭当前制冷机组！", Color.Orange);
                                                }
                                                #endregion
                                            }
                                            #endregion

                                        }
                                        #endregion

                                        #region 压缩机组未启动关闭循环泵
                                        else if (Flag.Unit[i].Compressor.RunAndStop.State == false)
                                        {
                                            if (Work.Pump.Stop(Flag.Unit[i].InternalPump.Address) == true)
                                            {
                                                Flag.Unit[i].InternalPump.IsRun = false;
                                            }
                                            else
                                            {
                                                IsChangeOk = false;
                                                ChangePump = i;
                                                break;
                                            }
                                        }
                                        #endregion
                                    }

                                    if (IsChangeOk == true)
                                    {
                                        ChangePump = 0;
                                        Index = 0;
                                        ChangeContinuityErr = 0;
                                    }
                                    else
                                    {
                                        Index = 10;
                                    }

                                    #endregion

                                    break;

                                case 20:

                                    #region 内循环泵停止
                                    for (int i = 0; i < Flag.Unit.Length; i++)
                                    {
                                        if (Flag.Unit[i].Compressor.RunAndStop.State == true && Flag.Unit[i].InternalPump.IsConnent == true)
                                        {
                                            if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsRun) == true)
                                            {
                                                if (Flag.Unit[i].InternalPump.IsRun == true && Work.Pump.Stop(Flag.Unit[i].InternalPump.Address) == true)
                                                {
                                                    Flag.Unit[i].InternalPump.IsRun = false;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else if (Flag.Unit[i].Compressor.RunAndStop.State == false)
                                        {
                                            if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsRun) == true)
                                            {
                                                if (Flag.Unit[i].InternalPump.IsRun == true && Work.Pump.Stop(Flag.Unit[i].InternalPump.Address) == true)
                                                {
                                                    Flag.Unit[i].InternalPump.IsRun = false;
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    Index = 0;

                                    break;
                            }

                            #endregion

                            break;

                        case 20:

                            #region 内循环泵停止
                            bool StopOk = true;

                            for (int i = 0; i < Flag.Unit.Length; i++)
                            {
                                if (Work.Pump.IsRun(Flag.Unit[i].InternalPump.Address, ref Flag.Unit[i].InternalPump.IsRun) == true)
                                {
                                    if (Flag.Unit[i].InternalPump.IsRun == true)
                                    {
                                        StopOk = false;
                                        Work.Pump.Stop(Flag.Unit[i].InternalPump.Address);
                                    }
                                }
                            }

                            if (StopOk == true)
                            {
                                StartIndex = 0;
                            }
                            #endregion

                            break;
                    }


                }
            }

            private static bool CheckUnitSenser(int i)
            {
                bool SenserState = true;

                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Compressor.In.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Compressor.Out.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Condenser.In.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Condenser.Out.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Evaporator.In.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Evaporator.Out.Pressure.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].HeatExchanger.Out.Pressure.Address);

                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Compressor.Out.Temp.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Condenser.Out.Temp.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Evaporator.Out.Temp.Address);

                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].Compressor.RunAndStop.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].ParallelValve.Address);
                SenserState = SenserState && Adam.IsConnect(Flag.Unit[i].BypassValve.Address);

                SenserState = SenserState && Flag.Unit[i].InternalPump.IsConnent;
                SenserState = SenserState && Flag.Unit[i].InternalPump.IsErr == true && Flag.Unit[i].InternalPump.IsErrClear == false ? false : true;

                return SenserState;
            }

            private static bool CheckUnitTemp(int i)
            {
                bool TempState = true;

                if (Flag.Unit[i].Compressor.Out.Temp.Value >= 200)
                {
                    TempState = false;
                }
                if (Flag.Unit[i].Condenser.Out.Temp.Value >= 200)
                {
                    TempState = false;
                }
                if (Flag.Unit[i].Evaporator.Out.Temp.Value >= 200)
                {
                    TempState = false;
                }

                return TempState;
            }

            private static bool CheckUnitPressure(int i)
            {
                bool PressureState = true;

                if (Flag.Unit[i].Compressor.In.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].Compressor.Out.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].Condenser.In.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].Condenser.Out.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].Evaporator.In.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].Evaporator.Out.Pressure.Value == 3)
                {
                    PressureState = false;
                }
                if (Flag.Unit[i].HeatExchanger.Out.Pressure.Value == 3)
                {
                    PressureState = false;
                }

                return PressureState;
            }
        }

        /// <summary>
        /// 加热控制
        /// </summary>
        public class ExternalTempControl
        {
            private static Thread HeatThread;
            private static Thread ColdThread;
            private static Thread PumpControl;

            /// <summary>
            /// 初始化外部加热
            /// </summary>
            public static void Initialization()
            {
                HeatThread = new Thread(HeatControl) { IsBackground = true };
                HeatThread.Start();

                ColdThread = new Thread(ColdControl) { IsBackground = true };
                ColdThread.Start();

                PumpControl = new Thread(PumpSpeedControl) { IsBackground = true };
                PumpControl.Start();
            }

            public static bool RunOrStopTempControl(bool State)
            {
                try
                {
                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        Flag.TestArm[i].Heat_IC.SetToHeat = new FlagStruct._SetToHeat();
                        Flag.TestArm[i].Heat_Inside.SetToHeat = new FlagStruct._SetToHeat();
                    }
                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        Flag.ColdPlate[i].Heat.SetToHeat = new FlagStruct._SetToHeat();
                    }
                    for (int i = 0; i < Flag.HotPlate.Length; i++)
                    {
                        Flag.HotPlate[i].Heat.SetToHeat = new FlagStruct._SetToHeat();
                    }
                    for (int i = 0; i < Flag.BorderTemp.Length; i++)
                    {
                        Flag.BorderTemp[i].Heat.SetToHeat = new FlagStruct._SetToHeat();
                    }

                    Flag.StartEnabled.RunHeat.IsHandlerHeat = Flag.Handler.IsHandlerControl;
                    Flag.StartEnabled.RunHeat.Heating = State;
                    Flag.CompressorErr = false;

                    if (Flag.StartEnabled.RunHeat.IsHandlerHeat == false)
                    {
                        if (Handler.NNodbusSlave != null)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[103] = 0;
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private static void HeatControl()
            {
                int Index = 0;

                bool Offset = true;
                bool StartState = true;

                #region TestArm

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    #region 设定TestArm IC 温度以及偏移使能状态等


                    Offset &= PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f, Flag.TempOffTK.TestArm_IC[i] + 150f, 150f);

                    StartState &= PMA.Stop(Flag.TestArm[i].Heat_IC.Address);

                    #endregion

                    #region 设定TestArm Inside 温度以及偏移使能状态等


                    Offset &= PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f, Flag.TempOffTK.TestArm_Inside[i] + 150f, 150f);
                    StartState &= PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);
                }
                #endregion

                #endregion

                #region ColdPlate
                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                {
                    #region 设定ColdPlate 温度、偏移、使能

                    Offset &= PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f, Flag.TempOffTK.ColdPlate[i] + 150f, 150f);

                    StartState &= PMA.Stop(Flag.ColdPlate[i].Heat.Address);

                    #endregion
                }
                #endregion

                #region HotPlate
                for (int i = 0; i < Flag.HotPlate.Length; i++)
                {
                    #region 设定 HotPlate 温度、偏移、使能

                    Offset &= PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f, Flag.TempOffTK.HotPlate[i] + 150f, 150f);

                    StartState &= PMA.Stop(Flag.HotPlate[i].Heat.Address);

                    #endregion
                }
                #endregion

                #region BorderTemp
                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    #region 设定 BorderTemp 温度、偏移、使能

                    Offset &= PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f, Flag.TempOffTK.Border[i] + 150f, 150f);

                    StartState &= PMA.Stop(Flag.BorderTemp[i].Heat.Address);

                    #endregion

                }
                #endregion

                while (true)
                {
                    Thread.Sleep(100);

                    #region 实时更新TkOff

                    if (Handler.NNodbusSlave != null)
                    {
                        //TestArm IC
                        for (int i = 0; i < Flag.TempOffTK.TestArm_IC.Length; i++)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i] = Auxiliary.ShortToUshort((short)(Flag.TempOffTK.TestArm_IC[i] * 100f));
                        }

                        //TestArm Inside
                        for (int i = 0; i < Flag.TempOffTK.TestArm_Inside.Length; i++)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[546 + i] = Auxiliary.ShortToUshort((short)(Flag.TempOffTK.TestArm_Inside[i] * 100f));
                        }

                        //ColdPlate
                        for (int i = 0; i < Flag.TempOffTK.ColdPlate.Length; i++)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i] = Auxiliary.ShortToUshort((short)(Flag.TempOffTK.ColdPlate[i] * 100f));
                        }

                        //HotPlate
                        for (int i = 0; i < Flag.TempOffTK.HotPlate.Length; i++)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i] = Auxiliary.ShortToUshort((short)(Flag.TempOffTK.HotPlate[i] * 100f));
                        }

                        //BorderTemp
                        for (int i = 0; i < Flag.TempOffTK.Border.Length; i++)
                        {
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i] = Auxiliary.ShortToUshort((short)(Flag.TempOffTK.Border[i] * 100f));
                        }
                    }
                    //TestArm IC
                    for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                    {
                        Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk = Flag.TempOffTK.TestArm_IC[i];
                    }

                    //TestArm Inside
                    for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                    {
                        Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk = Flag.TempOffTK.TestArm_Inside[i];
                    }

                    //ColdPlate
                    for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                    {
                        Flag.VarietiesData.ColdPlate[i].OffTk = Flag.TempOffTK.ColdPlate[i];
                    }

                    //HotPlate
                    for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                    {
                        Flag.VarietiesData.HotPlate[i].OffTk = Flag.TempOffTK.HotPlate[i];
                    }

                    //BorderTemp
                    for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                    {
                        Flag.VarietiesData.BorderTemp[i].OffTk = Flag.TempOffTK.Border[i];
                    }
                    #endregion

                    #region 实时更新是否采用IC加热方式
                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        Flag.VarietiesData.TestArm[i].TestArm_IC.IsUseHeat = Flag.ExternalTestTempChange.TestArmUseIcHeat[i];
                        Flag.VarietiesData.TestArm[i].TestArm_Inside.IsUseHeat = !Flag.ExternalTestTempChange.TestArmUseIcHeat[i];
                    }
                    #endregion
                   
                    if (Flag.StartEnabled.RunHeat.IsHandlerHeat == true)
                    {
                        #region 使用SLT参数
                        switch (Index)
                        {
                            case 0:
                                
                                if (Flag.StartEnabled.RunHeat.Heating == true)
                                {
                                    Index = 10;

                                    Flag.RunLogList.Add("加热器控制开启！", Color.Black);

                                }
                                break;

                            case 10:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    Index = 20;
                                    Flag.RunLogList.Add("加热器控制关闭中！", Color.Black);
                                    break;
                                }

                                #region TestArm
                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    float Temp = Function.Other.SetVable(i % 2 == 0, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[492]) / 100f, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[496]) / 100f);
                            
                                    #region 设定TestArm IC 温度以及偏移使能状态等
                                    if ((short)(Flag.TestArm[i].Heat_IC.SetToHeat.Offset * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[500 + i]) ||
                                        (short)(Flag.TestArm[i].Heat_IC.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.Enabled != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i]) ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]) ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.Temp != Temp ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;
                                        if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i]) == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f,
                                                     Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) / 100f +
                                                     Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[500 + i]) / 100f + Temp, Temp);
                                            if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]) == true)
                                            {
                                                StartState = PMA.Start(Flag.TestArm[i].Heat_IC.Address, Temp);
                                            }
                                            else
                                            {
                                                StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);
                                            }
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f,
                                                     (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) / 100f + 150f), (int)(150f));

                                            StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Offset = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[500 + i]) / 100f;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) / 100f;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i]);
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]);
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Temp = Temp;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma = true;

                                            Flag.TestArm[i].Heat_IC.SetToHeat.ChangeState = true;
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm Inside 温度以及偏移使能状态等
                                    if ((short)(Flag.TestArm[i].Heat_Inside.SetToHeat.Offset * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[508 + i]) ||
                                        (short)(Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i]) ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat != !Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]) ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.Temp != Temp ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;
                                        if ((Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i])) == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f,
                                                     Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) / 100f +
                                                     Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[508 + i]) / 100f + Temp, Temp);

                                            if (!Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]) == true)
                                            {
                                                StartState = PMA.Start(Flag.TestArm[i].Heat_Inside.Address, Temp);
                                            }
                                            else
                                            {
                                                StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);
                                            }
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f,
                                                     (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) / 100f + 150f), (int)(150f));

                                            StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Offset = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[508 + i]) / 100f;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) / 100f;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[301 + i]);
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat = !Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[331 + i]);
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Temp = Temp;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma = true;

                                            Flag.TestArm[i].Heat_Inside.SetToHeat.ChangeState = true;
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm IC 加热功率等
                                    if (Flag.TestArm[i].Heat_IC.SetToHeat.PowerLimit != Handler.NNodbusSlave.DataStore.HoldingRegisters[576 + i])
                                    {
                                        if (Handler.NNodbusSlave.DataStore.HoldingRegisters[576 + i] <= 0)
                                        {
                                            Handler.NNodbusSlave.DataStore.HoldingRegisters[576 + i] = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.TestArm[i].Heat_IC.Address, Handler.NNodbusSlave.DataStore.HoldingRegisters[576 + i]) == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.PowerLimit = Handler.NNodbusSlave.DataStore.HoldingRegisters[576 + i];

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm Inside 加热功率等
                                    if (Flag.TestArm[i].Heat_Inside.SetToHeat.PowerLimit != Handler.NNodbusSlave.DataStore.HoldingRegisters[584 + i])
                                    {
                                        if (Handler.NNodbusSlave.DataStore.HoldingRegisters[584 + i] <= 0)
                                        {
                                            Handler.NNodbusSlave.DataStore.HoldingRegisters[584 + i] = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.TestArm[i].Heat_Inside.Address, Handler.NNodbusSlave.DataStore.HoldingRegisters[584 + i]) == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.PowerLimit = Handler.NNodbusSlave.DataStore.HoldingRegisters[584 + i];

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定测试模式 Hot与ATC
                                    switch (Handler.NNodbusSlave.DataStore.HoldingRegisters[339 + i])
                                    {
                                        case 1:
                                            Flag.TestArm[i].HeatMode = FlagEnum.HeatMode.Hot;
                                            break;

                                        case 2:
                                            Flag.TestArm[i].HeatMode = FlagEnum.HeatMode.ATC;
                                            break;

                                        default:
                                            Flag.TestArm[i].HeatMode = FlagEnum.HeatMode.Hot;
                                            break;
                                    }
                                    #endregion
                                }
                                #endregion

                                #region ColdPlate
                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    float Temp = Function.Other.SetVable(i % 2 == 0, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[493]) / 100f, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[497]) / 100f);

                                    #region 设定ColdPlate 温度、偏移、使能
                                    if (Flag.ColdPlate[i].Heat.SetToHeat.Enabled != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[309 + i]) ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.IsUseHeat != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[309 + i]) ||
                                        (short)(Flag.ColdPlate[i].Heat.SetToHeat.Offset * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[516 + i]) ||
                                        (short)(Flag.ColdPlate[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.Temp != Temp ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[309 + i]) == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f,
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[516 + i]) / 100f +
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) / 100f + Temp, Temp);

                                            StartState = PMA.Start(Flag.ColdPlate[i].Heat.Address, Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f,
                                                    (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) / 100f + 150f), (int)(150f));

                                            StartState = PMA.Stop(Flag.ColdPlate[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[309 + i]);
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsUseHeat = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[309 + i]);
                                            Flag.ColdPlate[i].Heat.SetToHeat.Offset = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[516 + i]) / 100f;
                                            Flag.ColdPlate[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) / 100f;
                                            Flag.ColdPlate[i].Heat.SetToHeat.Temp = Temp;
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.ColdPlate[i].Heat.SetToHeat.ChangeState = true;
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定ColdPlate 加热功率等
                                    if (Flag.ColdPlate[i].Heat.SetToHeat.PowerLimit != Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i])
                                    {
                                        if (Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i] <= 0)
                                        {
                                            Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i] = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.ColdPlate[i].Heat.Address, Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i]) == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.PowerLimit = Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i];

                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region HotPlate
                                for (int i = 0; i < Flag.HotPlate.Length; i++)
                                {
                                    float Temp = Function.Other.SetVable(i % 2 == 0, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[494]) / 100f, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[498]) / 100f);

                                    #region 设定 HotPlate 温度、偏移、使能
                                    if (Flag.HotPlate[i].Heat.SetToHeat.Enabled != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[311 + i]) ||
                                        Flag.HotPlate[i].Heat.SetToHeat.IsUseHeat != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[311 + i]) ||
                                        (short)(Flag.HotPlate[i].Heat.SetToHeat.Offset * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[518 + i]) ||
                                        (short)(Flag.HotPlate[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) ||
                                        Flag.HotPlate[i].Heat.SetToHeat.Temp != Temp ||
                                        Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[311 + i]) == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f,
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[518 + i]) / 100f +
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) / 100f + Temp, Temp);

                                            StartState = PMA.Start(Flag.HotPlate[i].Heat.Address, Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f,
                                                    (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) / 100f + 150f), (int)(150f));

                                            StartState = PMA.Stop(Flag.HotPlate[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[311 + i]);
                                            Flag.HotPlate[i].Heat.SetToHeat.IsUseHeat = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[311 + i]);
                                            Flag.HotPlate[i].Heat.SetToHeat.Offset = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[518 + i]) / 100f;
                                            Flag.HotPlate[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) / 100f;
                                            Flag.HotPlate[i].Heat.SetToHeat.Temp = Temp;
                                            Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.HotPlate[i].Heat.SetToHeat.ChangeState = true;
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定 HotPlate 加热功率等
                                    if (Flag.HotPlate[i].Heat.SetToHeat.PowerLimit != Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i])
                                    {
                                        if (Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i] <= 0)
                                        {
                                            Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i] = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.HotPlate[i].Heat.Address, Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i]) == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.PowerLimit = Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i];

                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region BorderTemp
                                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                                {
                                    float Temp = Function.Other.SetVable(i % 2 == 0, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[495]) / 100f, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[499]) / 100f);

                                    #region 设定 BorderTemp 温度、偏移、使能
                                    if (Flag.BorderTemp[i].Heat.SetToHeat.Enabled != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[313 + i]) ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.IsUseHeat != Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[313 + i]) ||
                                        (short)(Flag.BorderTemp[i].Heat.SetToHeat.Offset * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[520 + i]) ||
                                        (short)(Flag.BorderTemp[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.Temp != Temp ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[313 + i]) == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f,
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[520 + i]) / 100f +
                                                    Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) / 100f + Temp, Temp);

                                            StartState = PMA.Start(Flag.BorderTemp[i].Heat.Address, Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f,
                                                    (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) / 100f + 150f), (int)(150f));

                                            StartState = PMA.Stop(Flag.BorderTemp[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[313 + i]);
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsUseHeat = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[313 + i]);
                                            Flag.BorderTemp[i].Heat.SetToHeat.Offset = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[520 + i]) / 100f;
                                            Flag.BorderTemp[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) / 100f;
                                            Flag.BorderTemp[i].Heat.SetToHeat.Temp = Temp;
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.BorderTemp[i].Heat.SetToHeat.ChangeState = true;
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "参数功率修改失败！", Color.Red);
                                        }

                                    }
                                    #endregion

                                    #region 设定 BorderTemp 加热功率等
                                    if (Flag.BorderTemp[i].Heat.SetToHeat.PowerLimit != Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i])
                                    {
                                        if (Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i] <= 0)
                                        {
                                            Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i] = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.BorderTemp[i].Heat.Address, Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i]) == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.PowerLimit = Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i];

                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                break;

                            case 20:

                                bool StopOk = true;

                                #region TestArm
                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    #region 设定TestArm IC 温度以及偏移使能状态等
                                    if ((short)(Flag.TestArm[i].Heat_IC.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;
                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f, (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) / 100f + 150f), (int)(150f));

                                        StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[538 + i]) / 100f;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm Inside 温度以及偏移使能状态等
                                    if ((short)(Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f, (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) / 100f + 150f), (int)(150f));
                                        StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[545 + i]) / 100f;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region ColdPlate
                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    #region 设定ColdPlate 温度、偏移、使能
                                    if ((short)(Flag.ColdPlate[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {

                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f, (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) / 100f + 150f), (int)(150f));

                                        StartState = PMA.Stop(Flag.ColdPlate[i].Heat.Address);


                                        if (Offset && StartState == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[554 + i]) / 100f;
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region HotPlate
                                for (int i = 0; i < Flag.HotPlate.Length; i++)
                                {
                                    #region 设定 HotPlate 温度、偏移、使能
                                    if ((short)(Flag.HotPlate[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) ||
                                        Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f, (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) / 100f + 150f), (int)(150f));

                                        StartState = PMA.Stop(Flag.HotPlate[i].Heat.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[556 + i]) / 100f;
                                            Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region BorderTemp
                                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                                {
                                    float Temp = Function.Other.SetVable(i % 2 == 0, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[495]) / 100f, Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[499]) / 100f);

                                    #region 设定 BorderTemp 温度、偏移、使能
                                    if ((short)(Flag.BorderTemp[i].Heat.SetToHeat.OffTk * 100f) != Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f, (int)(Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) / 100f + 150f), (int)(150f));

                                        StartState = PMA.Stop(Flag.BorderTemp[i].Heat.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.OffTk = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[558 + i]) / 100f;
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion

                                }
                                #endregion

                                if (StopOk == true)
                                {
                                    Index = 0;
                                    Flag.RunLogList.Add("加热器控制关闭成功！", Color.Green);
                                }

                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 使用Chiller参数
                        switch (Index)
                        {
                            case 0:
                                if (Flag.StartEnabled.RunHeat.Heating == true)
                                {
                                    Index = 10;
                                    Flag.RunLogList.Add("加热器控制开启！", Color.Black);
                                }
                                break;

                            case 10:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    Index = 20;
                                    Flag.RunLogList.Add("加热器控制关闭中！", Color.Black);
                                    break;
                                }

                                #region TestArm
                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    #region 设定TestArm IC 温度以及偏移使能状态等
                                    if (Flag.TestArm[i].Heat_IC.SetToHeat.OffTk != Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.Offset != Flag.VarietiesData.TestArm[i].TestArm_IC.Offset ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.Enabled != Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat != Flag.VarietiesData.TestArm[i].TestArm_IC.IsUseHeat ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.Temp != Flag.VarietiesData.TestArm[i].TestArm_IC.Temp ||
                                        Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;
                                        if (Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f,
                                                     Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk +
                                                     Flag.VarietiesData.TestArm[i].TestArm_IC.Offset +
                                                     Flag.VarietiesData.TestArm[i].TestArm_IC.Temp,
                                                     Flag.VarietiesData.TestArm[i].TestArm_IC.Temp);

                                            if (Flag.VarietiesData.TestArm[i].TestArm_IC.IsUseHeat == true)
                                            {
                                                StartState = PMA.Start(Flag.TestArm[i].Heat_IC.Address, Flag.VarietiesData.TestArm[i].TestArm_IC.Temp);
                                            }
                                            else
                                            {
                                                StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);
                                            }
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f,
                                                     Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk + 150f, (int)150f);

                                            StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.OffTk = Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Offset = Flag.VarietiesData.TestArm[i].TestArm_IC.Offset;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Enabled = Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat = Flag.VarietiesData.TestArm[i].TestArm_IC.IsUseHeat;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.Temp = Flag.VarietiesData.TestArm[i].TestArm_IC.Temp;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm Inside 温度以及偏移使能状态等
                                    if (Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk != Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.Offset != Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled != Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat != Flag.VarietiesData.TestArm[i].TestArm_Inside.IsUseHeat ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.Temp != Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp ||
                                        Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;
                                        if (Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f,
                                                     Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk +
                                                     Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset +
                                                     Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp,
                                                     Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp);

                                            if (Flag.VarietiesData.TestArm[i].TestArm_Inside.IsUseHeat == true)
                                            {
                                                StartState = PMA.Start(Flag.TestArm[i].Heat_Inside.Address, Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp);
                                            }
                                            else
                                            {
                                                StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);
                                            }
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f,
                                                     Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk + 150f, (int)150f);

                                            StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk = Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Offset = Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled = Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat = Flag.VarietiesData.TestArm[i].TestArm_Inside.IsUseHeat;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.Temp = Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm IC 加热功率等
                                    if (Flag.TestArm[i].Heat_IC.SetToHeat.PowerLimit != Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit)
                                    {
                                        if (Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit <= 0)
                                        {
                                            Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.TestArm[i].Heat_IC.Address, Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit) == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.PowerLimit = Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit;

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "加热功率修改失败！", Color.Red);
                                        }

                                    }
                                    #endregion

                                    #region 设定TestArm Inside 加热功率等
                                    if (Flag.TestArm[i].Heat_Inside.SetToHeat.PowerLimit != Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit)
                                    {
                                        if (Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit <= 0)
                                        {
                                            Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.TestArm[i].Heat_Inside.Address, Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit) == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.PowerLimit = Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit;

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定测试模式 Hot与ATC

                                    Flag.TestArm[i].HeatMode = Flag.VarietiesData.TestArm[i].HeatMode;

                                    #endregion
                                }
                                #endregion

                                #region ColdPlate
                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    #region 设定ColdPlate 温度、偏移、使能
                                    if (Flag.ColdPlate[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.ColdPlate[i].OffTk ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.Offset != Flag.VarietiesData.ColdPlate[i].Offset ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.Enabled != Flag.VarietiesData.ColdPlate[i].Enabled ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.IsUseHeat != Flag.VarietiesData.ColdPlate[i].IsUseHeat ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.Temp != Flag.VarietiesData.ColdPlate[i].Temp ||
                                        Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Flag.VarietiesData.ColdPlate[i].Enabled == true && Flag.VarietiesData.ColdPlate[i].IsUseHeat == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.ColdPlate[i].OffTk +
                                                    Flag.VarietiesData.ColdPlate[i].Offset +
                                                    Flag.VarietiesData.ColdPlate[i].Temp,
                                                    Flag.VarietiesData.ColdPlate[i].Temp);

                                            StartState = PMA.Start(Flag.ColdPlate[i].Heat.Address, Flag.VarietiesData.ColdPlate[i].Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.ColdPlate[i].OffTk + 150f, (int)150f);

                                            StartState = PMA.Stop(Flag.ColdPlate[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.ColdPlate[i].OffTk;
                                            Flag.ColdPlate[i].Heat.SetToHeat.Offset = Flag.VarietiesData.ColdPlate[i].Offset;
                                            Flag.ColdPlate[i].Heat.SetToHeat.Enabled = Flag.VarietiesData.ColdPlate[i].Enabled;
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsUseHeat = Flag.VarietiesData.ColdPlate[i].IsUseHeat;
                                            Flag.ColdPlate[i].Heat.SetToHeat.Temp = Flag.VarietiesData.ColdPlate[i].Temp;
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定ColdPlate 加热功率等
                                    if (Flag.ColdPlate[i].Heat.SetToHeat.PowerLimit != Flag.VarietiesData.ColdPlate[i].PowerLimit)
                                    {
                                        if (Flag.VarietiesData.ColdPlate[i].PowerLimit <= 0)
                                        {
                                            Flag.VarietiesData.ColdPlate[i].PowerLimit = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.ColdPlate[i].Heat.Address, Flag.VarietiesData.ColdPlate[i].PowerLimit) == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.PowerLimit = Flag.VarietiesData.ColdPlate[i].PowerLimit;

                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }

                                    }
                                    #endregion
                                }
                                #endregion

                                #region HotPlate
                                for (int i = 0; i < Flag.HotPlate.Length; i++)
                                {
                                    #region 设定 HotPlate 温度、偏移、使能
                                    if (Flag.HotPlate[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.HotPlate[i].OffTk ||
                                        Flag.HotPlate[i].Heat.SetToHeat.Offset != Flag.VarietiesData.HotPlate[i].Offset ||
                                        Flag.HotPlate[i].Heat.SetToHeat.Enabled != Flag.VarietiesData.HotPlate[i].Enabled ||
                                        Flag.HotPlate[i].Heat.SetToHeat.IsUseHeat != Flag.VarietiesData.HotPlate[i].IsUseHeat ||
                                        Flag.HotPlate[i].Heat.SetToHeat.Temp != Flag.VarietiesData.HotPlate[i].Temp ||
                                        Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Flag.VarietiesData.HotPlate[i].Enabled == true && Flag.VarietiesData.HotPlate[i].IsUseHeat == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.HotPlate[i].OffTk +
                                                    Flag.VarietiesData.HotPlate[i].Offset +
                                                    Flag.VarietiesData.HotPlate[i].Temp,
                                                    Flag.VarietiesData.HotPlate[i].Temp);

                                            StartState = PMA.Start(Flag.HotPlate[i].Heat.Address, Flag.VarietiesData.HotPlate[i].Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.HotPlate[i].OffTk + 150f, (int)150f);

                                            StartState = PMA.Stop(Flag.HotPlate[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.HotPlate[i].OffTk;
                                            Flag.HotPlate[i].Heat.SetToHeat.Offset = Flag.VarietiesData.HotPlate[i].Offset;
                                            Flag.HotPlate[i].Heat.SetToHeat.Enabled = Flag.VarietiesData.HotPlate[i].Enabled;
                                            Flag.HotPlate[i].Heat.SetToHeat.IsUseHeat = Flag.VarietiesData.HotPlate[i].IsUseHeat;
                                            Flag.HotPlate[i].Heat.SetToHeat.Temp = Flag.VarietiesData.HotPlate[i].Temp;
                                            Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定 HotPlate 加热功率等
                                    if (Flag.HotPlate[i].Heat.SetToHeat.PowerLimit != Flag.VarietiesData.HotPlate[i].PowerLimit)
                                    {
                                        if (Flag.VarietiesData.HotPlate[i].PowerLimit <= 0)
                                        {
                                            Flag.VarietiesData.HotPlate[i].PowerLimit = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.HotPlate[i].Heat.Address, Flag.VarietiesData.HotPlate[i].PowerLimit) == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.PowerLimit = Flag.VarietiesData.HotPlate[i].PowerLimit;

                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region BorderTemp
                                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                                {
                                    #region 设定 BorderTemp 温度、偏移、使能
                                    if (Flag.BorderTemp[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.BorderTemp[i].OffTk ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.Offset != Flag.VarietiesData.BorderTemp[i].Offset ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.Enabled != Flag.VarietiesData.BorderTemp[i].Enabled ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.IsUseHeat != Flag.VarietiesData.BorderTemp[i].IsUseHeat ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.Temp != Flag.VarietiesData.BorderTemp[i].Temp ||
                                        Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        Offset = false;
                                        StartState = false;

                                        if (Flag.VarietiesData.BorderTemp[i].Enabled == true)
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.BorderTemp[i].OffTk +
                                                    Flag.VarietiesData.BorderTemp[i].Offset +
                                                    Flag.VarietiesData.BorderTemp[i].Temp,
                                                    Flag.VarietiesData.BorderTemp[i].Temp);

                                            StartState = PMA.Start(Flag.BorderTemp[i].Heat.Address, Flag.VarietiesData.BorderTemp[i].Temp);
                                        }
                                        else
                                        {
                                            Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f,
                                                    Flag.VarietiesData.BorderTemp[i].OffTk + 150f, (int)150f);

                                            StartState = PMA.Stop(Flag.BorderTemp[i].Heat.Address);
                                        }

                                        if (Offset && StartState == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.BorderTemp[i].OffTk;
                                            Flag.BorderTemp[i].Heat.SetToHeat.Offset = Flag.VarietiesData.BorderTemp[i].Offset;
                                            Flag.BorderTemp[i].Heat.SetToHeat.Enabled = Flag.VarietiesData.BorderTemp[i].Enabled;
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsUseHeat = Flag.VarietiesData.BorderTemp[i].IsUseHeat;
                                            Flag.BorderTemp[i].Heat.SetToHeat.Temp = Flag.VarietiesData.BorderTemp[i].Temp;
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "参数修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "参数修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion

                                    #region 设定 BorderTemp 加热功率等
                                    if (Flag.BorderTemp[i].Heat.SetToHeat.PowerLimit != Flag.VarietiesData.BorderTemp[i].PowerLimit)
                                    {
                                        if (Flag.VarietiesData.BorderTemp[i].PowerLimit <= 0)
                                        {
                                            Flag.VarietiesData.BorderTemp[i].PowerLimit = 1;
                                        }
                                        if (PMA.SetPowerLimit(Flag.BorderTemp[i].Heat.Address, Flag.VarietiesData.BorderTemp[i].PowerLimit) == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.PowerLimit = Flag.VarietiesData.BorderTemp[i].PowerLimit;

                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "加热功率修改成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "加热功率修改失败！", Color.Red);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                break;

                            case 20:

                                bool StopOk = true;

                                #region TestArm
                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    #region 设定TestArm IC 温度以及偏移使能状态等
                                    if (Flag.TestArm[i].Heat_IC.SetToHeat.OffTk != Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk || Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_IC.Address, -1000f, -1000f, Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk + 150f, (int)150f);

                                        StartState = PMA.Stop(Flag.TestArm[i].Heat_IC.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_IC.SetToHeat.OffTk = Flag.VarietiesData.TestArm[i].TestArm_IC.OffTk;
                                            Flag.TestArm[i].Heat_IC.SetToHeat.IsWriteToPma = true;

                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion

                                    #region 设定TestArm Inside 温度以及偏移使能状态等
                                    if (Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk != Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk || Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.TestArm[i].Heat_Inside.Address, -1000f, -1000f, Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk + 150f, (int)150f);
                                        StartState = PMA.Stop(Flag.TestArm[i].Heat_Inside.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.OffTk = Flag.VarietiesData.TestArm[i].TestArm_Inside.OffTk;
                                            Flag.TestArm[i].Heat_Inside.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "通道关闭失败！", Color.Green);
                                        }

                                    }
                                    #endregion
                                }
                                #endregion

                                #region ColdPlate
                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    #region 设定ColdPlate 温度、偏移、使能
                                    if (Flag.ColdPlate[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.ColdPlate[i].OffTk || Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.ColdPlate[i].Heat.Address, -1000f, -1000f, Flag.VarietiesData.ColdPlate[i].OffTk + 150f, (int)150f);

                                        StartState = PMA.Stop(Flag.ColdPlate[i].Heat.Address);


                                        if (Offset && StartState == true)
                                        {
                                            Flag.ColdPlate[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.ColdPlate[i].OffTk;
                                            Flag.ColdPlate[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region HotPlate
                                for (int i = 0; i < Flag.HotPlate.Length; i++)
                                {
                                    #region 设定 HotPlate 温度、偏移、使能
                                    if (Flag.HotPlate[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.HotPlate[i].OffTk || Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.HotPlate[i].Heat.Address, -1000f, -1000f, Flag.VarietiesData.HotPlate[i].OffTk + 150f, (int)150f);

                                        StartState = PMA.Stop(Flag.HotPlate[i].Heat.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.HotPlate[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.HotPlate[i].OffTk;
                                            Flag.HotPlate[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region BorderTemp
                                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                                {
                                    #region 设定 BorderTemp 温度、偏移、使能
                                    if (Flag.BorderTemp[i].Heat.SetToHeat.OffTk != Flag.VarietiesData.BorderTemp[i].OffTk || Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma != true)
                                    {
                                        StopOk = false;

                                        Offset = false;
                                        StartState = false;

                                        Offset = PMA.SetTempDeviation(Flag.BorderTemp[i].Heat.Address, -1000f, -1000f, Flag.VarietiesData.BorderTemp[i].OffTk + 150f, (int)150f);

                                        StartState = PMA.Stop(Flag.BorderTemp[i].Heat.Address);

                                        if (Offset && StartState == true)
                                        {
                                            Flag.BorderTemp[i].Heat.SetToHeat.OffTk = Flag.VarietiesData.BorderTemp[i].OffTk;
                                            Flag.BorderTemp[i].Heat.SetToHeat.IsWriteToPma = true;
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "通道关闭成功！", Color.Green);
                                        }
                                        else
                                        {
                                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "通道关闭失败！", Color.Green);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                if (StopOk == true)
                                {
                                    Index = 0;
                                    Flag.RunLogList.Add("加热器控制关闭成功！", Color.Green);
                                }

                                break;
                        }
                        #endregion
                    }
                }
            }

            private static void ColdControl()
            {
                int index = 0;
                int ATC_Num = 0;
                int Com_Num = 0;

                while (true)
                {
                    Thread.Sleep(10);

                    ATC_Num = 0;
                    Com_Num = 0;
                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        if (Flag.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                        {
                            ATC_Num++;
                        }
                    }
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.Unit[i].Enabled == true)
                        {
                            Com_Num++;
                        }
                    }

                    if (Flag.StartEnabled.RunHeat.IsHandlerHeat == true)
                    {
                        #region 使用SLT参数

                        Flag.Unit[0].Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[202]);
                        Flag.Unit[1].Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[203]);
                        Flag.Unit[2].Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[204]);
                        Flag.Unit[3].Enabled = Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[205]);

                        Flag.WaterBox.Parameter.SetTemp = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[490]) / 100f;
                        Flag.WaterBox.Parameter.SetRange = Auxiliary.UshortToShort(Handler.NNodbusSlave.DataStore.HoldingRegisters[491]) / 100f;

                        switch (index)
                        {
                            case 0:

                                if (Flag.StartEnabled.RunHeat.Heating == true)
                                {
                                    index = 1;
                                }

                                break;

                            case 1:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    index = 100;
                                    Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                    break;
                                }

                                if (ATC_Num > 0 && Com_Num > 0 && Flag.StartEnabled.RunCold.Colding == false)
                                {
                                    Flag.StartEnabled.RunCold.Colding = true;
                                    index = 10;
                                }
                                else if (ATC_Num > 0 && Com_Num > 0 && Flag.StartEnabled.RunCold.Colding == true)
                                {
                                    index = 10;
                                }
                                else if ((ATC_Num == 0 || Com_Num == 0) && Flag.StartEnabled.RunCold.Colding == true)
                                {
                                    Flag.StartEnabled.RunCold.Colding = false;
                                    index = 20;
                                }
                                else if ((ATC_Num == 0 || Com_Num == 0) && Flag.StartEnabled.RunCold.Colding == false)
                                {
                                    index = 20;
                                }

                                break;

                            case 10:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    if (Flag.StartEnabled.RunCold.Colding == true)
                                    {
                                        Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                        index = 100;
                                    }
                                    else
                                    {
                                        index = 0;
                                    }

                                    break;
                                }

                                if (ATC_Num == 0 || Com_Num == 0)
                                {
                                    index = 1;
                                }

                                break;


                            case 20:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    if (Flag.StartEnabled.RunCold.Colding == true)
                                    {
                                        Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                        index = 100;
                                    }
                                    else
                                    {
                                        index = 0;
                                    }

                                    break;
                                }

                                if (ATC_Num > 0 || Com_Num > 0)
                                {
                                    index = 1;
                                }


                                break;

                            case 100:

                                Flag.StartEnabled.RunCold.Colding = false;
                                if (Flag.StartEnabled.RunCold.IsStop == true)
                                {
                                    Flag.RunLogList.Add("停止制冷成功！", Color.Black);
                                    index = 0;
                                }

                                break;
                        }

                        #endregion

                    }
                    else
                    {
                        #region 使用Chiller参数
                        switch (index)
                        {
                            case 0:

                                if (Flag.StartEnabled.RunHeat.Heating == true)
                                {
                                    index = 1;
                                }

                                break;

                            case 1:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    if (Flag.StartEnabled.RunCold.Colding == true)
                                    {
                                        if (Win.MessageBox("制冷是否需要关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            index = 100;
                                            Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                        }
                                        else
                                        {
                                            index = 0;
                                        }
                                    }
                                    break;
                                }


                                if (ATC_Num > 0 && Com_Num > 0 && Flag.StartEnabled.RunCold.Colding == false)
                                {
                                    if (Win.MessageBox("选择模式为ATC模式，制冷系统已关闭，是否确认开启？" + "\r\n" + "点击Yes确认开启." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        Flag.StartEnabled.RunCold.Colding = true;
                                    }
                                    index = 10;
                                }
                                else if (ATC_Num > 0 && Com_Num > 0 && Flag.StartEnabled.RunCold.Colding == true)
                                {
                                    index = 10;
                                }
                                else if ((ATC_Num == 0 || Com_Num == 0) && Flag.StartEnabled.RunCold.Colding == true)
                                {
                                    if (Win.MessageBox("选择模式为Hot模式，制冷系统已开启，是否确认关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        Flag.StartEnabled.RunCold.Colding = false;
                                    }
                                    index = 20;
                                }
                                else if ((ATC_Num == 0 || Com_Num == 0) && Flag.StartEnabled.RunCold.Colding == false)
                                {
                                    index = 20;
                                }

                                break;

                            case 10:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    if (Flag.StartEnabled.RunCold.Colding == true)
                                    {
                                        if (Win.MessageBox("制冷是否需要关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            index = 100;
                                            Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                        }
                                        else
                                        {
                                            index = 0;
                                        }
                                    }

                                    break;
                                }

                                if (ATC_Num == 0 || Com_Num == 0)
                                {
                                    index = 1;
                                }

                                break;


                            case 20:

                                if (Flag.StartEnabled.RunHeat.Heating == false)
                                {
                                    if (Flag.StartEnabled.RunCold.Colding == true)
                                    {
                                        if (Win.MessageBox("制冷是否需要关闭？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            index = 100;
                                            Flag.RunLogList.Add("停止制冷中！", Color.Black);
                                        }
                                        else
                                        {
                                            index = 0;
                                        }
                                    }

                                    break;
                                }

                                if (ATC_Num > 0 && Com_Num > 0)
                                {
                                    index = 1;
                                }


                                break;

                            case 100:

                                Flag.StartEnabled.RunCold.Colding = false;
                                bool State = false;
                                for (int i = 0; i < Flag.Unit.Length; i++)
                                {
                                    State = State || Flag.Unit[i].Compressor.RunAndStop.State;
                                }
                                if (State == false)
                                {
                                    Flag.RunLogList.Add("停止制冷成功！", Color.Black);

                                    index = 0;
                                }

                                break;
                        }

                        #endregion
                    }
                }
            }

            private static void PumpSpeedControl()
            {
                int Index = 0;
                bool ValueOk = false;
                while (true)
                {
                    Thread.Sleep(10);

                    switch (Index)
                    {
                        case 0:

                            if (Flag.StartEnabled.RunHeat.Heating == true)
                            {
                                Index = 1;
                            }
                            break;

                        case 1:

                            if (Flag.StartEnabled.RunHeat.Heating == false)
                            {
                                Index = 2;
                            }
                            ValueOk = true;
                            for (int i = 0; i < Flag.TestArm.Length; i++)
                            {
                                ValueOk = ValueOk & Adam.IsConnect(Flag.TestArm[i].Valve.Address);
                            }

                            for (int i = 0; i < Flag.ColdPlate.Length; i++)
                            {
                                ValueOk = ValueOk & Adam.IsConnect(Flag.ColdPlate[i].Valve.Address);
                            }

                            int TestArm1357 = 0;
                            int ColdPlate = 0;
                            int TestArm2468 = 0;
                            bool ChangePumpSpeed = false;
                            ushort TestArmSpeed1357 = 0;
                            ushort ColdPlateSpeed = 0;
                            ushort TestArmSpeed2468 = 0;

                            if (Flag.StartEnabled.RunHeat.IsHandlerHeat == true)
                            {
                                #region 更改泵速度状态

                                for (int i = 0; i < Flag.OutPumpSpeed.Length; i++)
                                {
                                    Flag.OutPumpSpeed[i] = Handler.NNodbusSlave.DataStore.HoldingRegisters[616 - i];
                                }

                                #endregion
                            }
                            else
                            {
                                #region 更改泵速度状态

                                for (int i = 0; i < Flag.OutPumpSpeed.Length; i++)
                                {
                                    Flag.OutPumpSpeed[i] = Flag.VarietiesData.PumpSpeed[i];
                                }

                                #endregion
                            }

                            if (Flag.StartEnabled.RunCold.Colding == true && ValueOk == true)
                            {
                                #region 打开对应制冷通道

                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    if (Flag.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                                    {
                                        if (Flag.TestArm[i].Valve.State != (Flag.TestArm[i].Heat_IC.SetToHeat.Enabled && Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat) || (Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled && Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat))
                                        {
                                            Work.IO.Set_DO(Flag.TestArm[i].Valve.Address, (Flag.TestArm[i].Heat_IC.SetToHeat.Enabled && Flag.TestArm[i].Heat_IC.SetToHeat.IsUseHeat) || (Flag.TestArm[i].Heat_Inside.SetToHeat.Enabled && Flag.TestArm[i].Heat_Inside.SetToHeat.IsUseHeat));
                                        }
                                    }
                                    else
                                    {
                                        Work.IO.Set_DO(Flag.TestArm[i].Valve.Address, false);
                                    }
                                }

                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    if (Flag.ColdPlate[i].Valve.State != Flag.ColdPlate[i].Heat.SetToHeat.Enabled)
                                    {
                                        Work.IO.Set_DO(Flag.ColdPlate[i].Valve.Address, Flag.ColdPlate[i].Heat.SetToHeat.Enabled);
                                    }
                                }

                                #endregion

                                #region 检测阀体开启数量与是否需要开启总阀
                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    if (Flag.TestArm[i].Valve.State == true)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            TestArm1357++;
                                        }
                                        else
                                        {
                                            TestArm2468++;
                                        }
                                    }
                                }
                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    if (Flag.ColdPlate[i].Valve.State == true)
                                    {
                                        ColdPlate++;
                                    }
                                }

                                if (TestArm1357 > 0 || TestArm2468 > 0 || ColdPlate > 0)
                                {
                                    if (Flag.WaterBox.ExternalValve.State == false)
                                    {
                                        Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, true);
                                    }
                                }
                                else
                                {
                                    Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, false);
                                }
                                #endregion

                                #region 检测循环泵是否需要开启

                                if (TestArm1357 > 0 && Flag.ExternalPump[2].IsRun == false)
                                {
                                    ChangePumpSpeed = true;
                                }
                                else if (TestArm1357 <= 0 && Flag.ExternalPump[2].IsRun == true)
                                {
                                    ChangePumpSpeed = true;
                                }

                                if (ColdPlate > 0 && Flag.ExternalPump[1].IsRun == false)
                                {
                                    ChangePumpSpeed = true;
                                }
                                else if (ColdPlate <= 0 && Flag.ExternalPump[1].IsRun == true)
                                {
                                    ChangePumpSpeed = true;
                                }

                                if (TestArm2468 > 0 && Flag.ExternalPump[0].IsRun == false)
                                {
                                    ChangePumpSpeed = true;
                                }
                                else if (TestArm2468 <= 0 && Flag.ExternalPump[0].IsRun == true)
                                {
                                    ChangePumpSpeed = true;
                                }

                                #endregion

                                #region 查询泵转速是否一致
                                #region 流速自动控制
                                if (Function.Other.Int_To_Bool(Handler.NNodbusSlave.DataStore.HoldingRegisters[104]) == false)
                                {
                                    if (TestArm1357 > 0)
                                    {
                                        if (Flag.OutPumpSpeed[2] > 1000)
                                        {
                                            TestArmSpeed1357 = (ushort)(1000 / 4 * TestArm1357);
                                        }
                                        else
                                        {
                                            TestArmSpeed1357 = (ushort)(Flag.OutPumpSpeed[2] / 4 * TestArm1357);
                                        }

                                        if (TestArmSpeed1357 > 1000)
                                        {
                                            TestArmSpeed1357 = 1000;
                                        }
                                        if (TestArmSpeed1357 < 10)
                                        {
                                            TestArmSpeed1357 = 10;
                                        }
                                        if (TestArmSpeed1357 != Flag.ExternalPump[2].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }
                                    else
                                    {
                                        TestArmSpeed1357 = 0;
                                    }

                                    if (ColdPlate > 0)
                                    {
                                        if (Flag.OutPumpSpeed[1] > 600)
                                        {
                                            ColdPlateSpeed = (ushort)(600 / 2 * ColdPlate);
                                        }
                                        else
                                        {
                                            ColdPlateSpeed = (ushort)(Flag.OutPumpSpeed[1] / 2 * ColdPlate);
                                        }

                                        if (ColdPlateSpeed > 1000)
                                        {
                                            ColdPlateSpeed = 1000;
                                        }
                                        if (ColdPlateSpeed < 10)
                                        {
                                            ColdPlateSpeed = 10;
                                        }
                                        if (ColdPlateSpeed != Flag.ExternalPump[1].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }
                                    else
                                    {
                                        ColdPlateSpeed = 0;
                                    }

                                    if (TestArm2468 > 0)
                                    {
                                        if (Flag.OutPumpSpeed[0] > 1000)
                                        {
                                            TestArmSpeed2468 = (ushort)(1000 / 4 * TestArm2468);
                                        }
                                        else
                                        {
                                            TestArmSpeed2468 = (ushort)(Flag.OutPumpSpeed[0] / 4 * TestArm2468);
                                        }

                                        if (TestArmSpeed2468 > 1000)
                                        {
                                            TestArmSpeed2468 = 1000;
                                        }
                                        if (TestArmSpeed2468 < 10)
                                        {
                                            TestArmSpeed2468 = 10;
                                        }
                                        if (TestArmSpeed2468 != Flag.ExternalPump[0].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }
                                    else
                                    {
                                        TestArmSpeed2468 = 0;
                                    }
                                }
                                #endregion
                                #region 流速手动控制
                                else
                                {
                                    if (TestArm1357 > 0)
                                    {
                                        if (Flag.OutPumpSpeed[2] > 1000)
                                        {
                                            TestArmSpeed1357 = (ushort)(1000);
                                        }
                                        else
                                        {
                                            TestArmSpeed1357 = (ushort)(Flag.OutPumpSpeed[2]);
                                        }

                                        if (TestArmSpeed1357 > 1000)
                                        {
                                            TestArmSpeed1357 = 1000;
                                        }
                                        if (TestArmSpeed1357 < 10)
                                        {
                                            TestArmSpeed1357 = 10;
                                        }
                                        if (TestArmSpeed1357 != Flag.ExternalPump[2].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }

                                    else
                                    {
                                        TestArmSpeed1357 = 0;
                                    }

                                    if (ColdPlate > 0)
                                    {
                                        if (Flag.OutPumpSpeed[1] > 600)
                                        {
                                            ColdPlateSpeed = (ushort)(600);
                                        }
                                        else
                                        {
                                            ColdPlateSpeed = (ushort)(Flag.OutPumpSpeed[1]);
                                        }

                                        if (ColdPlateSpeed > 1000)
                                        {
                                            ColdPlateSpeed = 1000;
                                        }
                                        if (ColdPlateSpeed < 10)
                                        {
                                            ColdPlateSpeed = 10;
                                        }
                                        if (ColdPlateSpeed != Flag.ExternalPump[1].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }
                                    else
                                    {
                                        ColdPlateSpeed = 0;
                                    }

                                    if (TestArm2468 > 0)
                                    {
                                        if (Flag.OutPumpSpeed[0] > 1000)
                                        {
                                            TestArmSpeed2468 = (ushort)(1000);
                                        }
                                        else
                                        {
                                            TestArmSpeed2468 = (ushort)(Flag.OutPumpSpeed[0]);
                                        }

                                        if (TestArmSpeed2468 > 1000)
                                        {
                                            TestArmSpeed2468 = 1000;
                                        }
                                        if (TestArmSpeed2468 < 10)
                                        {
                                            TestArmSpeed2468 = 10;
                                        }
                                        if (TestArmSpeed2468 != Flag.ExternalPump[0].SetSpeed)
                                        {
                                            ChangePumpSpeed = true;
                                        }
                                    }
                                    else
                                    {
                                        TestArmSpeed2468 = 0;
                                    }
                                }
                                #endregion
                                //#region 检测负载超限
                                //if (int.Parse(Flag.ExternalPump[0].OperatingLoad.ToString()) >= 50)
                                //{

                                //}
                                //#endregion
                                #endregion

                                #region 更新泵转速
                                if (Flag.WaterBox.Temp.Value < (Flag.WaterBox.Parameter.SetTemp / 2))
                                {
                                    if (Flag.WaterBox.ExternalValve.State == true && ChangePumpSpeed == true)
                                    {
                                        if (TestArm1357 > 0 && TestArmSpeed1357 > 0)
                                        {
                                            Work.Pump.REV(Flag.ExternalPump[2].Address, TestArmSpeed1357);
                                        }
                                        else
                                        {
                                            Work.Pump.Stop(Flag.ExternalPump[2].Address);
                                        }

                                        if (ColdPlate > 0 && ColdPlateSpeed > 0)
                                        {
                                            Work.Pump.REV(Flag.ExternalPump[1].Address, ColdPlateSpeed);
                                        }
                                        else
                                        {
                                            Work.Pump.Stop(Flag.ExternalPump[1].Address);
                                        }

                                        if (TestArm2468 > 0 && TestArmSpeed2468 > 0)
                                        {
                                            Work.Pump.REV(Flag.ExternalPump[0].Address, TestArmSpeed2468);
                                        }
                                        else
                                        {
                                            Work.Pump.Stop(Flag.ExternalPump[0].Address);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < Flag.ExternalPump.Length; i++)
                                    {
                                        Work.Pump.Stop(Flag.ExternalPump[i].Address);
                                    }
                                }
                                #endregion
                            }
                            else if (Flag.StartEnabled.RunCold.Colding == false && ValueOk == true)
                            {
                                #region 检测循环泵是否开启状态

                                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                                {
                                    if (Flag.ExternalPump[i].IsRun == true)
                                    {
                                        Work.Pump.Stop(Flag.ExternalPump[i].Address);
                                    }
                                }

                                #endregion

                                #region 关闭对应的制冷通道

                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    if (Flag.TestArm[i].Valve.State != false)
                                    {
                                        Work.IO.Set_DO(Flag.TestArm[i].Valve.Address, false);
                                    }
                                }

                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    if (Flag.ColdPlate[i].Valve.State != false)
                                    {
                                        Work.IO.Set_DO(Flag.ColdPlate[i].Valve.Address, false);
                                    }
                                }

                                #endregion
                            }

                            break;

                        case 2:

                            bool StopState = true;

                            #region 关闭循环泵
                            for (int i = 0; i < Flag.ExternalPump.Length; i++)
                            {
                                if (Flag.ExternalPump[i].IsRun == true)
                                {
                                    StopState = StopState && Work.Pump.Stop(Flag.ExternalPump[i].Address);
                                }
                            }
                            #endregion

                            #region 检测循环泵状态
                            for (int i = 0; i < Flag.ExternalPump.Length; i++)
                            {
                                StopState = StopState && !Flag.ExternalPump[i].IsRun;
                            }
                            #endregion

                            if (StopState == true)
                            {
                                #region 打开对应制冷通道

                                for (int i = 0; i < Flag.TestArm.Length; i++)
                                {
                                    if (Flag.TestArm[i].Valve.State != false)
                                    {
                                        Work.IO.Set_DO(Flag.TestArm[i].Valve.Address, false);
                                        StopState = false;
                                    }
                                }

                                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                                {
                                    if (Flag.ColdPlate[i].Valve.State != false)
                                    {
                                        Work.IO.Set_DO(Flag.ColdPlate[i].Valve.Address, false);
                                        StopState = false;
                                    }
                                }

                                #endregion

                                #region 是否开启总阀

                                if (Flag.WaterBox.ExternalValve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, false);
                                    StopState = false;
                                }

                                #endregion
                            }

                            if (StopState == true)
                            {
                                Index = 0;
                            }

                            break;

                    }
                }

            }
        }

        public class Alarm
        {
            private static Thread AlmThread;
            private static SystemFreeTime FreeTime;
            public static void Initialization()
            {
                FreeTime = new SystemFreeTime(60000);
                FreeTime.Enabled = true;
                AlmThread = new Thread(AlmControl) { IsBackground = true };
                AlmThread.Start();

            }

            private static void AlmControl()
            {
                while (true)
                {
                    Thread.Sleep(50);

                    if (FreeTime.FreeTimeSpan >= Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime * 60)
                    {
                        #region 自动关阀
                        if (Flag.StartEnabled.RunCold.Colding == false)
                        {
                            for (int i = 0; i < Flag.Unit.Length; i++)
                            {
                                if (Flag.Unit[i].Compressor.RunAndStop.State == false)
                                {
                                    Work.IO.Set_DO(Flag.Unit[i].BypassValve.Address, false);
                                    Work.IO.Set_DO(Flag.Unit[i].ParallelValve.Address, false);
                                }
                            }
                            bool State = false;
                            for (int i = 0; i < Flag.Unit.Length; i++)
                            {
                                State = State || Flag.Unit[i].InternalPump.IsRun;
                            }
                            if (State == false && Flag.WaterBox.InternalValve.State == true)
                            {
                                Work.IO.Set_DO(Flag.WaterBox.InternalValve.Address, false);
                            }
                        }

                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            if (Flag.ExternalPump[2].IsRun == false)
                            {
                                if (Flag.TestArm[0].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[0].Valve.Address, false);
                                }
                                if (Flag.TestArm[2].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[2].Valve.Address, false);
                                }
                                if (Flag.TestArm[4].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[4].Valve.Address, false);
                                }
                                if (Flag.TestArm[6].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[6].Valve.Address, false);
                                }
                            }

                            if (Flag.ExternalPump[1].IsRun == false)
                            {
                                if (Flag.ColdPlate[0].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.ColdPlate[0].Valve.Address, false);
                                }
                                if (Flag.ColdPlate[1].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.ColdPlate[1].Valve.Address, false);
                                }
                            }

                            if (Flag.ExternalPump[0].IsRun == false)
                            {
                                if (Flag.TestArm[1].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[1].Valve.Address, false);
                                }
                                if (Flag.TestArm[3].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[3].Valve.Address, false);
                                }
                                if (Flag.TestArm[5].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[5].Valve.Address, false);
                                }
                                if (Flag.TestArm[7].Valve.State == true)
                                {
                                    Work.IO.Set_DO(Flag.TestArm[7].Valve.Address, false);
                                }
                            }
                        }

                        if (Flag.ExternalPump[0].IsRun == false && Flag.ExternalPump[1].IsRun == false && Flag.ExternalPump[2].IsRun == false)
                        {
                            Work.IO.Set_DO(Flag.WaterBox.ExternalValve.Address, false);
                        }
                        #endregion
                    }

                    if (Flag.WaterBox.Height.Value <= 0)
                    {
                        #region 低液位自动关机
                        if (Flag.StartEnabled.RunCold.Colding == true)
                        {
                            Flag.StartEnabled.RunCold.Colding = false;
                            Flag.RunLogList.Add("液位过低，停止制冷！", Color.Red);
                        }

                        if (Flag.StartEnabled.RunHeat.Heating == true)
                        {
                            Flag.StartEnabled.RunHeat.Heating = false;
                            Handler.NNodbusSlave.DataStore.HoldingRegisters[103] = 0;
                            Flag.RunLogList.Add("液位过低，停止加热！", Color.Red);
                        }

                        for (int i = 0; i < Flag.ExternalPump.Length; i++)
                        {
                            if (Flag.ExternalPump[i].IsRun == true && PanasonicA6.IsOpen == true)
                            {
                                Work.Pump.Stop(Flag.ExternalPump[i].Address);
                            }
                        }
                        #endregion
                    }


                }
            }
        }
    }
}
