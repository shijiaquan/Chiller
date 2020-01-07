using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DewPointSensor;
using System.Drawing;

namespace Chiller
{
    class Work
    {
        public class Initial
        {
            /// <summary>
            /// 初始化研华模块
            /// </summary>
            public static bool[] Initialization_Adam()
            {
                Adam.Adam6017 = new Adam.AdamModule6017[5];
                Adam.Adam6018 = new Adam.AdamModule6018[2];
                Adam.Adam6250 = new Adam.AdamModule6250[1];
                Adam.Adam6256 = new Adam.AdamModule6256[1];
                bool[] RetState = new bool[Flag.Manufacturer.Network.Modular.Length];
                for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
                {
                    RetState[i] = Adam.InitializationAdamModule(i, Flag.Manufacturer.Network.Modular[i]);
                }

                return RetState;
            }

            /// <summary>
            /// 数据读取转换，交换线程
            /// </summary>
            public static void Initialization_Thread()
            {
                Thread PMA = new Thread(new ThreadStart(PMARead)) { IsBackground = true };
                PMA.Start();

                Thread Refresh = new Thread(new ThreadStart(RefreshSystemIO)) { IsBackground = true };
                Refresh.Start();

                Thread Record = new Thread(new ThreadStart(RecordSystemData)) { IsBackground = true };
                Record.Start();
            }

            /// <summary>
            /// 温度传感器数据读取
            /// </summary>
            private static void PMARead()
            {
                Stopwatch sw = new Stopwatch();
                
                float[] Station1 = new float[22];
                float[] Station2 = new float[24];
                while (true)
                {
                    Thread.Sleep(10);

                    #region PMA温控器的相关数据

                    PMA.GetAll_PV(1, ref Station1);
                    PMA.GetAll_PV(2, ref Station2);

                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        Flag.TestArm[i].Heat_Inside.GetInData.ErrCode = PMA.IsErrState(Flag.TestArm[i].Heat_Inside.Address);
                        if (Flag.TestArm[i].Heat_Inside.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.TestArm[i].Heat_Inside.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if(Station==1)
                            {
                                Flag.TestArm[i].Heat_Inside.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.TestArm[i].Heat_Inside.GetInData.PV = Station2[Channel];
                            }
                        }

                        Flag.TestArm[i].Heat_IC.GetInData.ErrCode = PMA.IsErrState(Flag.TestArm[i].Heat_IC.Address);
                        if (Flag.TestArm[i].Heat_IC.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.TestArm[i].Heat_IC.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if (Station == 1)
                            {
                                Flag.TestArm[i].Heat_IC.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.TestArm[i].Heat_IC.GetInData.PV = Station2[Channel];
                            }
                        }
                    }
                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        Flag.ColdPlate[i].Heat.GetInData.ErrCode = PMA.IsErrState(Flag.ColdPlate[i].Heat.Address);
                        if (Flag.ColdPlate[i].Heat.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.ColdPlate[i].Heat.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if (Station == 1)
                            {
                                Flag.ColdPlate[i].Heat.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.ColdPlate[i].Heat.GetInData.PV = Station2[Channel];
                            }
                        }
                    }
                    for (int i = 0; i < Flag.HotPlate.Length; i++)
                    {
                        Flag.HotPlate[i].Heat.GetInData.ErrCode = PMA.IsErrState(Flag.HotPlate[i].Heat.Address);
                        if (Flag.HotPlate[i].Heat.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.HotPlate[i].Heat.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if (Station == 1)
                            {
                                Flag.HotPlate[i].Heat.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.HotPlate[i].Heat.GetInData.PV = Station2[Channel];
                            }
                        }
                    }
                    for (int i = 0; i < Flag.BorderTemp.Length; i++)
                    {
                        Flag.BorderTemp[i].Heat.GetInData.ErrCode = PMA.IsErrState(Flag.BorderTemp[i].Heat.Address);
                        if (Flag.BorderTemp[i].Heat.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.BorderTemp[i].Heat.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if (Station == 1)
                            {
                                Flag.BorderTemp[i].Heat.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.BorderTemp[i].Heat.GetInData.PV = Station2[Channel];
                            }
                        }
                    }
                    for (int i = 0; i < Flag.OtherTemp.Length; i++)
                    {
                        Flag.OtherTemp[i].Heat.GetInData.ErrCode = PMA.IsErrState(Flag.OtherTemp[i].Heat.Address);
                        if (Flag.OtherTemp[i].Heat.GetInData.ErrCode != "Err")
                        {
                            string[] Index = Flag.OtherTemp[i].Heat.Address.Replace(" ", "").Split(new char[] { '-' });
                            byte Station = byte.Parse(Index[0]);
                            ushort Channel = ushort.Parse(Index[1]);
                            if (Station == 1)
                            {
                                Flag.OtherTemp[i].Heat.GetInData.PV = Station1[Channel];
                            }
                            else if (Station == 2)
                            {
                                Flag.OtherTemp[i].Heat.GetInData.PV = Station2[Channel];
                            }
                        }
                    }
                    #endregion

                    if(sw.ElapsedMilliseconds>=2000)
                    {
                        RestartPMA();
                        sw.Restart();
                    }
                    if(sw.IsRunning==false)
                    {
                        sw.Start();
                    }

                    #region 随机温度，用于校准通讯

                    //if (sw.IsRunning == false)
                    //{
                    //    sw.Start();
                    //}
                    //if (sw.ElapsedMilliseconds >= 5000)
                    //{
                    //    Random R = new Random();

                    //    for (int i = 0; i < Flag.TestArm.Length; i++)
                    //    {
                    //        Flag.TestArm[i].Heat_Inside.GetInData.PV = R.Next(-100, 100);
                    //        Flag.TestArm[i].Heat_Inside.GetInData.SP = R.Next(-100, 100);

                    //        Flag.TestArm[i].Heat_IC.GetInData.PV = R.Next(-100, 100);
                    //        Flag.TestArm[i].Heat_IC.GetInData.SP = R.Next(-100, 100);
                    //    }
                    //    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    //    {
                    //        Flag.ColdPlate[i].Heat.GetInData.PV = R.Next(-100, 100);
                    //        Flag.ColdPlate[i].Heat.GetInData.SP = R.Next(-100, 100);
                    //    }
                    //    for (int i = 0; i < Flag.HotPlate.Length; i++)
                    //    {
                    //        Flag.HotPlate[i].Heat.GetInData.PV = R.Next(-100, 100);
                    //        Flag.HotPlate[i].Heat.GetInData.SP = R.Next(-100, 100);
                    //    }
                    //    for (int i = 0; i < Flag.BorderTemp.Length; i++)
                    //    {
                    //        Flag.BorderTemp[i].Heat.GetInData.PV = R.Next(-100, 100);
                    //        Flag.BorderTemp[i].Heat.GetInData.SP = R.Next(-100, 100);
                    //    }
                    //    for (int i = 0; i < Flag.OtherTemp.Length; i++)
                    //    {
                    //        Flag.OtherTemp[i].Heat.GetInData.PV = R.Next(-100, 100);
                    //        Flag.OtherTemp[i].Heat.GetInData.SP = R.Next(-100, 100);
                    //    }
                    //    sw.Restart();
                    //}
                    #endregion
                }
            } 

            public static void RestartPMA()
            {
                #region PMA温控器和加热功率的相关数据

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    if (Flag.TestArm[i].Heat_Inside.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.TestArm[i].Heat_Inside.Address, ref Flag.TestArm[i].Heat_Inside.GetInData.SP);
                            PMA.Get_C_Off(Flag.TestArm[i].Heat_Inside.Address, ref Flag.TestArm[i].Heat_Inside.GetInData.IsRun);
                        if(Flag.TestArm[i].Heat_Inside.GetInData.IsRun==true)
                        {                          
                            PMA.Getpower(Flag.TestArm[i].Heat_Inside.Address, ref Flag.TestArm[i].Heat_Inside.GetInPower.PowerLimit);
                        }
                    }
                    if (Flag.TestArm[i].Heat_IC.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.TestArm[i].Heat_IC.Address, ref Flag.TestArm[i].Heat_IC.GetInData.SP);
                            PMA.Get_C_Off(Flag.TestArm[i].Heat_IC.Address, ref Flag.TestArm[i].Heat_IC.GetInData.IsRun);
                        if (Flag.TestArm[i].Heat_IC.GetInData.IsRun == true)
                        {
                            PMA.Getpower(Flag.TestArm[i].Heat_IC.Address, ref Flag.TestArm[i].Heat_IC.GetInPower.PowerLimit);
                        }
                    }
                }
                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                {
                    if (Flag.ColdPlate[i].Heat.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.ColdPlate[i].Heat.Address, ref Flag.ColdPlate[i].Heat.GetInData.SP);
                            PMA.Get_C_Off(Flag.ColdPlate[i].Heat.Address, ref Flag.ColdPlate[i].Heat.GetInData.IsRun);
                    }
                }
                for (int i = 0; i < Flag.HotPlate.Length; i++)
                {
                    if (Flag.HotPlate[i].Heat.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.HotPlate[i].Heat.Address, ref Flag.HotPlate[i].Heat.GetInData.SP);
                            PMA.Get_C_Off(Flag.HotPlate[i].Heat.Address, ref Flag.HotPlate[i].Heat.GetInData.IsRun);
                    }
                }
                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    if (Flag.BorderTemp[i].Heat.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.BorderTemp[i].Heat.Address, ref Flag.BorderTemp[i].Heat.GetInData.SP);
                            PMA.Get_C_Off(Flag.BorderTemp[i].Heat.Address, ref Flag.BorderTemp[i].Heat.GetInData.IsRun);
                    }
                }
                for (int i = 0; i < Flag.OtherTemp.Length; i++)
                {
                    if (Flag.OtherTemp[i].Heat.GetInData.ErrCode != "Err")
                    {
                            PMA.Get_SP(Flag.OtherTemp[i].Heat.Address, ref Flag.OtherTemp[i].Heat.GetInData.SP);
                            PMA.Get_C_Off(Flag.OtherTemp[i].Heat.Address, ref Flag.OtherTemp[i].Heat.GetInData.IsRun);
                    }
                }
                #endregion
            }
            /// <summary>
            /// 开始转换各类数据到Flag类
            /// </summary>
            private static void RefreshSystemIO()
            {
                int NowState = 0;
               
                while (true)
                {
                    Thread.Sleep(1);

                    #region 研华IO以及模拟量的相关数据

                    NowState = IO.Get_DO(Flag.WaterBox.InternalValve.Address);
                    if (NowState != -1)
                    {
                        Flag.WaterBox.InternalValve.State = Function.Other.Int_To_Bool(NowState);
                    }

                    NowState = IO.Get_DO(Flag.WaterBox.ExternalValve.Address);
                    if (NowState != -1)
                    {
                        Flag.WaterBox.ExternalValve.State = Function.Other.Int_To_Bool(NowState);
                    }

                    Flag.WaterBox.Height.Value = IO.Get_AI(Flag.WaterBox.Height.Address, 0, 6.25f);
                    //Flag.WaterBox.Height.Value = 25f;
                    float OffsetPoint = Flag.WaterBox.Offset.TempOffset[3] / (Math.Abs(Flag.WaterBox.Offset.TempBasicPoint[0]) + Math.Abs(Flag.WaterBox.Offset.TempBasicPoint[2]));
                    Flag.WaterBox.Temp.Value = IO.Get_AI(Flag.WaterBox.Temp.Address, -100, 12.5f) + IO.Get_AI(Flag.WaterBox.Temp.Address, -100, 12.5f) * Math.Abs(OffsetPoint);

                    Flag.WaterBox.Height.Analog = IO.Get_AI(Flag.WaterBox.Height.Address);
                    Flag.WaterBox.Temp.Analog = IO.Get_AI(Flag.WaterBox.Temp.Address);

                    Flag.WaterBox.Height.AnalogCompany = IO.Get_Company(Flag.WaterBox.Height.Address);
                    Flag.WaterBox.Temp.AnalogCompany = IO.Get_Company(Flag.WaterBox.Temp.Address);

                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        Flag.Unit[i].Compressor.In.Pressure.Value = IO.Get_AI(Flag.Unit[i].Compressor.In.Pressure.Address, 0, 0.1875f);
                        Flag.Unit[i].Compressor.Out.Pressure.Value = IO.Get_AI(Flag.Unit[i].Compressor.Out.Pressure.Address, 0, 0.1875f);

                        Flag.Unit[i].Condenser.In.Pressure.Value = IO.Get_AI(Flag.Unit[i].Condenser.In.Pressure.Address, 0, 0.1875f);
                        Flag.Unit[i].Condenser.Out.Pressure.Value = IO.Get_AI(Flag.Unit[i].Condenser.Out.Pressure.Address, 0, 0.1875f);

                        Flag.Unit[i].Evaporator.In.Pressure.Value = IO.Get_AI(Flag.Unit[i].Evaporator.In.Pressure.Address, 0, 0.1875f);
                        Flag.Unit[i].Evaporator.Out.Pressure.Value = IO.Get_AI(Flag.Unit[i].Evaporator.Out.Pressure.Address, 0, 0.1875f);

                        Flag.Unit[i].HeatExchanger.Out.Pressure.Value = IO.Get_AI(Flag.Unit[i].HeatExchanger.Out.Pressure.Address, 0, 0.1875f);

                        Flag.Unit[i].Compressor.Out.Temp.Value = IO.Get_AI(Flag.Unit[i].Compressor.Out.Temp.Address);
                        Flag.Unit[i].Condenser.Out.Temp.Value = IO.Get_AI(Flag.Unit[i].Condenser.Out.Temp.Address);
                        Flag.Unit[i].Evaporator.Out.Temp.Value = IO.Get_AI(Flag.Unit[i].Evaporator.Out.Temp.Address);

                        NowState = IO.Get_DO(Flag.Unit[i].Compressor.RunAndStop.Address);
                        if (NowState != -1)
                        {
                            Flag.Unit[i].Compressor.RunAndStop.State = Function.Other.Int_To_Bool(NowState);
                        }

                        NowState = IO.Get_DO(Flag.Unit[i].BypassValve.Address);
                        if (NowState != -1)
                        {
                            Flag.Unit[i].BypassValve.State = Function.Other.Int_To_Bool(NowState);
                        }

                        NowState = IO.Get_DO(Flag.Unit[i].ParallelValve.Address);
                        if (NowState != -1)
                        {
                            Flag.Unit[i].ParallelValve.State = Function.Other.Int_To_Bool(NowState);
                        }

                        Flag.Unit[i].Compressor.In.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Compressor.In.Pressure.Address);
                        Flag.Unit[i].Compressor.Out.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Compressor.Out.Pressure.Address);

                        Flag.Unit[i].Condenser.In.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Condenser.In.Pressure.Address);
                        Flag.Unit[i].Condenser.Out.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Condenser.Out.Pressure.Address);

                        Flag.Unit[i].Evaporator.In.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Evaporator.In.Pressure.Address);
                        Flag.Unit[i].Evaporator.Out.Pressure.Analog = IO.Get_AI(Flag.Unit[i].Evaporator.Out.Pressure.Address);

                        Flag.Unit[i].HeatExchanger.Out.Pressure.Analog = IO.Get_AI(Flag.Unit[i].HeatExchanger.Out.Pressure.Address);

                        Flag.Unit[i].Compressor.Out.Temp.Analog = IO.Get_AI(Flag.Unit[i].Compressor.Out.Temp.Address);
                        Flag.Unit[i].Condenser.Out.Temp.Analog = IO.Get_AI(Flag.Unit[i].Condenser.Out.Temp.Address);
                        Flag.Unit[i].Evaporator.Out.Temp.Analog = IO.Get_AI(Flag.Unit[i].Evaporator.Out.Temp.Address);


                        Flag.Unit[i].Compressor.In.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Compressor.In.Pressure.Address);
                        Flag.Unit[i].Compressor.Out.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Compressor.Out.Pressure.Address);

                        Flag.Unit[i].Condenser.In.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Condenser.In.Pressure.Address);
                        Flag.Unit[i].Condenser.Out.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Condenser.Out.Pressure.Address);

                        Flag.Unit[i].Evaporator.In.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Evaporator.In.Pressure.Address);
                        Flag.Unit[i].Evaporator.Out.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].Evaporator.Out.Pressure.Address);

                        Flag.Unit[i].HeatExchanger.Out.Pressure.AnalogCompany = IO.Get_Company(Flag.Unit[i].HeatExchanger.Out.Pressure.Address);

                        Flag.Unit[i].Compressor.Out.Temp.AnalogCompany = IO.Get_Company(Flag.Unit[i].Compressor.Out.Temp.Address);
                        Flag.Unit[i].Condenser.Out.Temp.AnalogCompany = IO.Get_Company(Flag.Unit[i].Condenser.Out.Temp.Address);
                        Flag.Unit[i].Evaporator.Out.Temp.AnalogCompany = IO.Get_Company(Flag.Unit[i].Evaporator.Out.Temp.Address);
                    }

                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        NowState = IO.Get_DO(Flag.TestArm[i].Valve.Address);
                        if (NowState != -1)
                        {
                            Flag.TestArm[i].Valve.State = Function.Other.Int_To_Bool(NowState);
                        }
                    }

                    for (int i = 0; i < Flag.ColdPlate.Length; i++)
                    {
                        NowState = IO.Get_DO(Flag.ColdPlate[i].Valve.Address);
                        if (NowState != -1)
                        {
                            Flag.ColdPlate[i].Valve.State = Function.Other.Int_To_Bool(NowState);
                        }
                    }
                    #endregion

                    #region 温湿度传感器相关参数
                    for (int i = 0; i < Flag.DewPoint.Length; i++)
                    {
                        Flag.DewPoint[i].IsConnect = HTU21D.NowHTU21D[Flag.DewPoint[i].Address - 1].IsConnect;
                        Flag.DewPoint[i].DewPoint = HTU21D.NowHTU21D[Flag.DewPoint[i].Address - 1].DewPoint;
                        Flag.DewPoint[i].Humidity = HTU21D.NowHTU21D[Flag.DewPoint[i].Address - 1].Humidity;
                        Flag.DewPoint[i].Temperature = HTU21D.NowHTU21D[Flag.DewPoint[i].Address - 1].Temperature;
                    }
                    #endregion

                    #region 泵的相关数据

                    #region 内循环数据转换
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Work.Pump.PauseRead == false)
                        {
                            Flag.Unit[i].InternalPump.IsConnent = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].IsConnent;
                            Flag.Unit[i].InternalPump.ConnentErrNumber = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].ConnentErrNumber;
                            Flag.Unit[i].InternalPump.IsRun = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].IsRun;
                            Flag.Unit[i].InternalPump.IsErr = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].IsErr;
                            Flag.Unit[i].InternalPump.IsErrClear = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].IsErrClear;
                            Flag.Unit[i].InternalPump.ErrCode = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].ErrCode;
                            Flag.Unit[i].InternalPump.OperatingLoad = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].OperatingLoad;
                            Flag.Unit[i].InternalPump.EncoderTemp = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].EncoderTemp;
                            Flag.Unit[i].InternalPump.Speed = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].NowSpeed;
                            Flag.Unit[i].InternalPump.SetSpeed = PanasonicA6.A6_Channel[Flag.Unit[i].InternalPump.Address - 1].SetSpeed;
                        }
                    }
                    #endregion

                    #region 外循环数据转换
                    for (int i = 0; i < Flag.ExternalPump.Length; i++)
                    {
                        if (Work.Pump.PauseRead == false)
                        {
                            Flag.ExternalPump[i].IsConnent = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].IsConnent;
                            Flag.ExternalPump[i].ConnentErrNumber = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].ConnentErrNumber;
                            Flag.ExternalPump[i].IsRun = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].IsRun;
                            Flag.ExternalPump[i].IsErr = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].IsErr;
                            Flag.ExternalPump[i].ErrCode = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].ErrCode;
                            Flag.ExternalPump[i].OperatingLoad = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].OperatingLoad;
                            Flag.ExternalPump[i].EncoderTemp = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].EncoderTemp;
                            Flag.ExternalPump[i].Speed = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].NowSpeed;
                            Flag.ExternalPump[i].SetSpeed = PanasonicA6.A6_Channel[Flag.ExternalPump[i].Address - 1].SetSpeed;
                        }
                    }
                    #endregion

                    #endregion
                }
            }

            /// <summary>
            /// 开始每隔1s记录一次温度或压力，用于生成曲线
            /// </summary>
            private static void RecordSystemData()
            {
                Stopwatch Sw = new Stopwatch();
                Sw.Start();

                while (true)
                {
                    Thread.Sleep(100);

                    if (Sw.ElapsedMilliseconds >= 1000)
                    {
                        for (int i = 0; i < Flag.Unit.Length; i++)
                        {
                            Flag.Unit[i].Compressor.In.Pressure.AddToRecord();
                            Flag.Unit[i].Compressor.Out.Pressure.AddToRecord();
                            Flag.Unit[i].Compressor.Out.Temp.AddToRecord();

                            Flag.Unit[i].Condenser.In.Pressure.AddToRecord();
                            Flag.Unit[i].Condenser.Out.Pressure.AddToRecord();
                            Flag.Unit[i].Condenser.Out.Temp.AddToRecord();

                            Flag.Unit[i].Evaporator.In.Pressure.AddToRecord();
                            Flag.Unit[i].Evaporator.Out.Pressure.AddToRecord();
                            Flag.Unit[i].Evaporator.Out.Temp.AddToRecord();

                            Flag.Unit[i].HeatExchanger.Out.Pressure.AddToRecord();
                        }

                        Flag.WaterBox.Temp.AddToRecord();
                        Flag.WaterBox.Height.AddToRecord();
                        Flag.WaterBox.Parameter.AddToSetTempRecord();

                        for (int i = 0; i < Flag.TestArm.Length; i++)
                        {
                            Flag.TestArm[i].Heat_Inside.NowRecord.Add(Flag.TestArm[i].Heat_Inside.GetInData.PV);
                            Flag.TestArm[i].Heat_Inside.SetRecord.Add(Flag.TestArm[i].Heat_Inside.GetInData.SP);

                            Flag.TestArm[i].Heat_IC.NowRecord.Add(Flag.TestArm[i].Heat_IC.GetInData.PV);
                            Flag.TestArm[i].Heat_IC.SetRecord.Add(Flag.TestArm[i].Heat_IC.GetInData.SP);
                        }

                        for (int i = 0; i < Flag.ColdPlate.Length; i++)
                        {
                            Flag.ColdPlate[i].Heat.NowRecord.Add(Flag.ColdPlate[i].Heat.GetInData.PV);
                            Flag.ColdPlate[i].Heat.SetRecord.Add(Flag.ColdPlate[i].Heat.GetInData.SP);
                        }

                        for (int i = 0; i < Flag.HotPlate.Length; i++)
                        {
                            Flag.HotPlate[i].Heat.NowRecord.Add(Flag.HotPlate[i].Heat.GetInData.PV);
                            Flag.HotPlate[i].Heat.SetRecord.Add(Flag.HotPlate[i].Heat.GetInData.SP);
                        }

                        for (int i = 0; i < Flag.BorderTemp.Length; i++)
                        {
                            Flag.BorderTemp[i].Heat.NowRecord.Add(Flag.BorderTemp[i].Heat.GetInData.PV);
                            Flag.BorderTemp[i].Heat.SetRecord.Add(Flag.BorderTemp[i].Heat.GetInData.SP);
                        }

                        for (int i = 0; i < Flag.OtherTemp.Length; i++)
                        {
                            Flag.OtherTemp[i].Heat.NowRecord.Add(Flag.OtherTemp[i].Heat.GetInData.PV);
                            Flag.OtherTemp[i].Heat.SetRecord.Add(Flag.OtherTemp[i].Heat.GetInData.SP);
                        }
                        Sw.Restart();
                    }
                }
            }

            /// <summary>
            /// 判断系统所有传感器状态是否正常
            /// </summary>
            /// <returns></returns>
            public static bool SenserDetection(bool Display)
            {
                bool IsOk = true;

                for (int i = 0; i < 8; i++)
                {
                    if (Adam.IsConnect(i) == false)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "：检测失败！", Color.Red);
                        }
                    }
                }

                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    if (Flag.TestArm[i].Heat_IC.GetInData.PV >= 200)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_IC.Name + "：温度传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.TestArm[i].Heat_Inside.GetInData.PV >= 200)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add(Flag.TestArm[i].Heat_Inside.Name + "：温度传感器检测失败！", Color.Red);

                        }
                    }
                }
                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                {
                    if (Flag.ColdPlate[i].Heat.GetInData.PV >= 200)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add(Flag.ColdPlate[i].Heat.Name + "：温度传感器检测失败！", Color.Red);
                        }
                    }
                }
                for (int i = 0; i < Flag.HotPlate.Length; i++)
                {
                    if (Flag.HotPlate[i].Heat.GetInData.PV >= 200)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add(Flag.HotPlate[i].Heat.Name + "：温度传感器检测失败！", Color.Red);
                        }
                    }
                            }
                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    if (Flag.BorderTemp[i].Heat.GetInData.PV >= 200)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add(Flag.BorderTemp[i].Heat.Name + "：温度传感器检测失败！", Color.Red);
                        }
                    }
                }

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit[i].Compressor.In.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：压缩机入口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].Compressor.Out.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：压缩机出口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].Condenser.In.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：冷凝器入口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].Condenser.Out.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：冷凝器出口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].Evaporator.In.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：蒸发器入口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].Evaporator.Out.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：蒸发器出口压力传感器检测失败！", Color.Red);
                        }
                    }
                    if (Flag.Unit[i].HeatExchanger.Out.Pressure.Value == 3)
                    {
                        IsOk = false;
                        if (Display == true)
                        {
                            Flag.RunLogList.Add("压缩机组" + (i + 1).ToString() + "：换热器出口压力传感器检测失败！", Color.Red);
                        }
                    }
                }

                if (Flag.WaterBox.Height.Value == 100)
                {
                    IsOk = false;
                    if (Display == true)
                    {
                        Flag.RunLogList.Add("水箱液位传感器检测失败！", Color.Red);

                    }
                }
                if (Flag.WaterBox.Temp.Value == 200 || Flag.WaterBox.Temp.Value == -100)
                {
                    IsOk = false;
                    if (Display == true)
                    {
                        Flag.RunLogList.Add("水箱温度传感器检测失败！", Color.Red);
                    }
                }

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit[i].InternalPump.IsErr == true)
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (Flag.Unit[i].InternalPump.ErrCode == PanasonicA6.A6_ErrCodeList[j].Code && PanasonicA6.A6_ErrCodeList[j].IsClear == false)
                            {
                                IsOk = false;
                                Flag.RunLogList.Add("内循环泵" + (i + 1).ToString() + "驱动器报警，ErrCode：" + Flag.Unit[i].InternalPump.ErrCode, Color.Red);
                            }
                        }
                    }
                }

                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Flag.ExternalPump[i].IsErr == true)
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (Flag.ExternalPump[i].ErrCode == PanasonicA6.A6_ErrCodeList[j].Code && PanasonicA6.A6_ErrCodeList[j].IsClear == false)
                            {
                                Flag.RunLogList.Add("外循环泵" + (i + 1).ToString() + "驱动器报警，ErrCode：" + Flag.ExternalPump[i].ErrCode, Color.Red);
                            }
                        }
                    }
                }

                return IsOk;
            }
        }

        public class IO
        {
            /// <summary>              
            /// 获取转换过后的模拟量值
            /// </summary>
            /// <param name="address">读取的模拟量地址</param>  
            /// <param name="benchmark">采集到的电流为4ma时对应的转换后的基准值</param>  
            /// <param name="deviation">电流所需乘以的倍数</param>  
            public static float Get_AI(string address, float benchmark, float deviation)
            {
                string[] index = address.Split(new char[] { '-' });
                int Id_Num = int.Parse(index[0]);
                int Channel_Num = int.Parse(index[1]);
                float Now_num = 0;

                switch (Adam.GetAdamType(Id_Num, ref Id_Num))
                {
                    case Adam.HaveAdamType.AdamModule6017:

                        if (Adam.Adam6017[Id_Num].AI != null)
                        {
                            if (deviation == 0)
                            {
                                Now_num = Adam.Adam6017[Id_Num].AI[Channel_Num];
                            }
                            else
                            {
                                if (Adam.Adam6017[Id_Num].AI[int.Parse(index[1])] != 0)
                                {
                                    Now_num = (Adam.Adam6017[Id_Num].AI[Channel_Num] - 4) * deviation + benchmark;
                                }
                            }
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6018:

                        if (Adam.Adam6018[Id_Num].Temp != null)
                        {
                            Now_num = Adam.Adam6018[Id_Num].Temp[int.Parse(index[1])];
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6250:

                        //6250无模拟量IO

                        break;

                    case Adam.HaveAdamType.AdamModule6256:

                        //6256无模拟量IO

                        break;

                    default:

                        break;
                }

                return Now_num;
            }

            /// <summary>
            /// 获取指定地址的模拟量原始值
            /// </summary>
            /// <param name="address">读取的模拟量地址</param>  
            public static float Get_AI(string address)
            {
                return Get_AI(address, 0, 0);
            }

            /// <summary>
            /// 获取指定模拟量地址的单位
            /// </summary>
            /// <param name="address">读取的模拟量地址</param>  
            public static string Get_Company(string address)
            {
                string[] index = address.Split(new char[] { '-' });
                int Id_Num = int.Parse(index[0]);
                int Channel_Num = int.Parse(index[1]);
                string Now_num = "";

                switch (Adam.GetAdamType(Id_Num, ref Id_Num))
                {
                    case Adam.HaveAdamType.AdamModule6017:

                        if (Adam.Adam6017[Id_Num].AI != null)
                        {
                            Now_num = Adam.GetSymbol(Adam.Adam6017[Id_Num], Channel_Num);
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6018:

                        if (Adam.Adam6018[0].Temp != null)
                        {
                            Now_num = Adam.GetSymbol(Adam.Adam6018[Id_Num], Channel_Num);
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6250:

                        //6250无模拟量IO

                        break;

                    case Adam.HaveAdamType.AdamModule6256:

                        //6256无模拟量IO

                        break;

                    default:

                        break;
                }

                return Now_num;
            }

            /// <summary>
            /// 获取输出IO状态
            /// </summary>
            /// <param name="address">读取地址</param>  
            public static int Get_DO(string address)
            {
                string[] index = address.Split(new char[] { '-' });
                int Id_Num = int.Parse(index[0]);
                int Channel_Num = int.Parse(index[1]);
                int State = 0;
                
                switch (Adam.GetAdamType(Id_Num, ref Id_Num))
                {
                    case Adam.HaveAdamType.AdamModule6017:

                        if (Adam.Adam6017 != null)
                        {
                            if (Adam.Adam6017[Id_Num].IsComR == true && Adam.Adam6017[Id_Num].DO != null && Adam.Adam6017[Id_Num].DO.Length > Channel_Num)
                            {
                                State = Adam.Adam6017[Id_Num].DO[int.Parse(index[1])];
                            }
                            else
                            {
                                State = -1;
                            }
                        }
                        else
                        {
                            State = -1;
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6018:

                        if (Adam.Adam6018 != null)
                        {
                            if (Adam.Adam6018[Id_Num].IsComR == true && Adam.Adam6018[Id_Num].DO != null && Adam.Adam6018[Id_Num].DO.Length > Channel_Num)
                            {
                                State = Adam.Adam6018[Id_Num].DO[Channel_Num];
                            }
                            else
                            {
                                State = -1;
                            }
                        }
                        else
                        {
                            State = -1;
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6250:

                        if (Adam.Adam6250 != null)
                        {
                            if (Adam.Adam6250[Id_Num].IsComR == true && Adam.Adam6250[Id_Num].DO != null && Adam.Adam6250[Id_Num].DO.Length > Channel_Num)
                            {
                                State = Adam.Adam6250[Id_Num].DO[Channel_Num];
                            }
                            else
                            {
                                State = -1;
                            }
                        }
                        else
                        {
                            State = -1;
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6256:

                        if (Adam.Adam6256 != null)
                        {
                            if (Adam.Adam6256[Id_Num].IsComR == true && Adam.Adam6256[Id_Num].DO != null && Adam.Adam6256[Id_Num].DO.Length > Channel_Num)
                            {
                                State = Adam.Adam6256[Id_Num].DO[Channel_Num];
                            }
                            else
                            {
                                State = -1;
                            }
                        }
                        else
                        {
                            State = -1;
                        }

                        break;

                    default:

                        State = -1;

                        break;
                }

                return State;
            }
            /// <summary>
            /// 设置输出IO状态
            /// </summary>
            /// <param name="address">地址</param>  
            /// <param name="iOnOff">0为常开，1为常闭</param>  
            public static bool Set_DO(string address, int iOnOff)
            {
                string[] index = address.Split(new char[] { '-' });

                int Id_Num = int.Parse(index[0]);
                int Channel_Num = int.Parse(index[1]);

                int Address = 0;

                bool bl = false;
                int timeout = 0;
                if (iOnOff >= 1)
                {
                    iOnOff = 1;
                }
                else if (iOnOff <= 0)
                {
                    iOnOff = 0;
                }

                switch (Adam.GetAdamType(Id_Num, ref Id_Num))
                {
                    case Adam.HaveAdamType.AdamModule6017:

                        if (Adam.Adam6017[Id_Num].ComT != null && Adam.Adam6017[Id_Num].IsComT == true)
                        {
                            Address = 17 + Channel_Num;
                            bl = Adam.Adam6017[Id_Num].ComT.Modbus().ForceSingleCoil(Address, iOnOff);

                            if (bl == false)
                            {
                                do
                                {
                                    Thread.Sleep(50);
                                    if (Get_DO(address) == iOnOff)
                                    {
                                        bl = true;
                                    }
                                    timeout++;
                                }
                                while (bl == false && timeout < 10);

                                if (timeout < 10)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    bl = false;
                                }
                            }
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6018:

                        if (Adam.Adam6018[Id_Num].ComT != null && Adam.Adam6018[Id_Num].IsComT == true)
                        {
                            Address = 17 + Channel_Num;
                            bl = Adam.Adam6018[Id_Num].ComT.Modbus().ForceSingleCoil(Address, iOnOff);

                            if (bl == false)
                            {
                                do
                                {
                                    Thread.Sleep(50);
                                    if (Get_DO(address) == iOnOff)
                                    {
                                        bl = true;
                                    }
                                    timeout++;
                                }
                                while (bl == false && timeout < 10);

                                if (timeout < 10)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    bl = false;
                                }
                            }
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6250:

                        if (Adam.Adam6250[Id_Num].ComT != null && Adam.Adam6250[Id_Num].IsComT == true)
                        {
                            Address = 17 + Channel_Num;
                            bl = Adam.Adam6250[Id_Num].ComT.Modbus().ForceSingleCoil(Address, iOnOff);

                            if (bl == false)
                            {
                                do
                                {
                                    Thread.Sleep(50);
                                    if (Get_DO(address) == iOnOff)
                                    {
                                        bl = true;
                                    }
                                    timeout++;
                                }
                                while (bl == false && timeout < 10);

                                if (timeout < 10)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    bl = false;
                                }
                            }
                        }

                        break;

                    case Adam.HaveAdamType.AdamModule6256:

                        if (Adam.Adam6256[Id_Num].ComT != null && Adam.Adam6256[Id_Num].IsComT == true)
                        {
                            Address = 17 + Channel_Num;
                            bl = Adam.Adam6256[Id_Num].ComT.Modbus().ForceSingleCoil(Address, iOnOff);

                            if (bl == false)
                            {
                                do
                                {
                                    Thread.Sleep(50);
                                    if (Get_DO(address) == iOnOff)
                                    {
                                        bl = true;
                                    }
                                    timeout++;
                                }
                                while (bl == false && timeout < 10);

                                if (timeout < 10)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    bl = false;
                                }
                            }
                        }

                        break;

                    default:

                        bl = false;

                        break;
                }


                return bl;
            }
            /// <summary>
            /// 设置输出IO状态
            /// </summary>
            /// <param name="address">地址</param>  
            /// <param name="state">false为常开，true为常闭</param>  
            public static bool Set_DO(string address, bool state)
            {
                bool bl = false;
                if (state == true)
                {
                    bl = Set_DO(address, 1);
                }
                else if (state == false)
                {
                    bl = Set_DO(address, 0);
                }

                return bl;
            }

        }

        public class Pump
        {
            /// <summary>
            /// 电机反转
            /// </summary>
            /// <param name="VFD_NUM">电机站号</param>   
            /// <param name="Speed">电机运行速度</param>   
            public static bool FWD(byte Address, ushort Speed)
            {
                return PanasonicA6.FWD(Address, Speed);
            }

            /// <summary>
            /// 电机正转
            /// </summary>
            /// <param name="VFD_NUM">电机站号</param>   
            /// <param name="Speed">电机运行速度</param>  
            public static bool REV(byte Address, ushort Speed)
            {
                return PanasonicA6.REV(Address, Speed);
            }

            /// <summary>
            /// 电机停止
            /// </summary>
            /// <param name="Address">站号</param>  
            public static bool Stop(byte Address)
            {

                bool ret = PanasonicA6.Stop(Address);

                if (ret == true)
                {
                    bool IsIn = false;
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.Unit[i].InternalPump.Address == Address)
                        {
                            IsIn = true;
                            Flag.Unit[i].InternalPump.Speed = 0;
                            break;
                        }
                    }
                    if (IsIn == false)
                    {
                        for (int i = 0; i < Flag.ExternalPump.Length; i++)
                        {
                            if (Flag.ExternalPump[i].Address == Address)
                            {
                                Flag.ExternalPump[i].Speed = 0;
                                break;
                            }
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// 电机运行速度
            /// </summary>
            /// <param name="Address">站号</param> 
            /// <param name="Speed">如果连接为变频器则输出Hz，如果连接电机为伺服驱动器输出为转速</param> 
            public static bool SetSpeed(byte Address, ushort Speed)
            {
                return PanasonicA6.SetSpeed(Address, Speed);
            }

            /// <summary>
            /// 电机减速时间
            /// </summary>
            /// <param name="Address">站号</param> 
            /// <param name="time">减速时间，单位ms</param> 
            public static bool TAcc(byte Address, ushort time)
            {
                return PanasonicA6.TAcc(Address, time);
            }
            /// <summary>
            /// 电机加速时间
            /// </summary>
            /// <param name="Address">站号</param> 
            /// <param name="time">加速时间，单位为ms</param> 
            public static bool TDec(byte Address, ushort time)
            {
                return PanasonicA6.TDec(Address, time);
            }

            /// <summary>
            /// 伺服上电
            /// </summary>
            /// <param name="address">地址0-31</param> 
            /// <param name="state">伺服上电 = 1  伺服断电 = 0</param> 
            public static bool SetServo(byte Address, bool OnOff)
            {
                return PanasonicA6.SetServo(Address, OnOff);
            }

            /// <summary>
            /// 将指定驱动器的报警状态清除
            /// </summary>
            /// <param name="address">地址0-31</param> 
            public static bool ClearErr(byte Address)
            {
                return PanasonicA6.ClearErr(Address);
            }

            public static bool IsErr(byte Address, ref bool StateOut)
            {
                return PanasonicA6.IsErr(Address,ref StateOut);
            }

            /// <summary>
            /// 判断当前电机地址电机是否正在运行
            /// </summary>
            /// <param name="address">地址0-31</param> 
            public static bool IsRun(byte Address, ref bool StateOut)
            {
                return PanasonicA6.IsRun(Address, ref StateOut);
            }

            /// <summary>
            /// 暂停泵的读取操作，释放出通讯口
            /// </summary>
            public static bool PauseRead
            {
                get
                {
                    return PanasonicA6.PauseRead;
                }
                set
                {
                    PanasonicA6.PauseRead = value;
                }
            }
        }

        public class TypeConvert
        {
            /// <summary>
            /// 将字符串转换为枚举 Enem
            /// </summary>
            /// <param name="str">枚举字符串</param>
            /// <returns>枚举</returns>
            public static System.Windows.Forms.DataVisualization.Charting.SeriesChartType StringToEnum(string str)
            {
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                try
                {
                    ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)Enum.Parse(typeof(System.Windows.Forms.DataVisualization.Charting.SeriesChartType), str);
                }
                catch
                {
                    return ChartType;
                }

                return ChartType;
            }
            /// <summary>
            /// 将枚举转换为数值
            /// </summary>
            /// <param name="ChartType">枚举</param>
            /// <returns>数值</returns>
            public static int EnumToInt(System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType)
            {
                return (int)ChartType;
            }
            /// <summary>
            /// 将枚举转换为字符串
            /// </summary>
            /// <param name="ChartType">枚举</param>
            /// <returns>字符串</returns>
            public static string EnumToString(System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType)
            {
                return ChartType.ToString();
            }
        }

        public class SystemEnum
        {
            /// <summary>
            /// 初始化系统状态刷新
            /// </summary>
            public static void Initial()
            {
                Thread State = new Thread(new ThreadStart(SystemEnumResart)) { IsBackground = true };
                State.Start();
            }

            /// <summary>
            /// 刷新系统状态
            /// </summary>
            public static void SystemEnumResart()
            {

            }
        }

    }
}
