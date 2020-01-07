using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chiller
{
    public class Handler
    {
        public static ModbusSerialSlave NNodbusSlave;
        private static SerialPort SP;
        private static bool _IsConnect = false;
        private static Stopwatch OverTime;

        private static Thread TestArmIcTempState;
        private static Thread TestArmInsideTempState;
        private static Thread ColdPlateTempState;
        private static Thread HotPlatePlateTempState;
        private static Thread BorderPlateTempState;

        /// <summary>
        /// 初始化串口连接
        /// </summary>
        /// <param name="Port">串口号</param>
        /// <param name="Baud">波特率</param>
        /// <returns>成功或失败</returns>
        public static bool Initialization(string Port, int Baud)
        {
            try
            {
                OverTime = new Stopwatch();

                SP = new SerialPort(Port, Baud, Parity.Even, 8, StopBits.One);
             
                NNodbusSlave = ModbusSerialSlave.CreateRtu(1, SP);
                NNodbusSlave.Transport.ReadTimeout = 250;
                NNodbusSlave.Transport.WriteTimeout = 250;
                NNodbusSlave.DataStore = DataStoreFactory.CreateDefaultDataStore();
                NNodbusSlave.ModbusSlaveRequestReceived += NNodbusSlave_ModbusSlaveRequestReceived;

                NNodbusSlave.DataStore.HoldingRegisters[103] = 99;

                Thread SlaveListen = new Thread(Listen) { IsBackground = true };
                SlaveListen.Start();

                Thread UpdataToHandler = new Thread(SlaveUpdataToHandler) { IsBackground = true };
                UpdataToHandler.Start();

                Thread UpDataToChiller = new Thread(SlaveUpDataToChiller) { IsBackground = true };
                UpDataToChiller.Start();

                for (int i = 0; i < 8; i++)
                {
                    TestArmIcTempState = new Thread(SlaveUpdataTestArmICTempStateToHandler) { IsBackground = true };
                    TestArmIcTempState.Start(i);

                    TestArmInsideTempState = new Thread(SlaveUpdataTestArmInsedeTempStateToHandler) { IsBackground = true };
                    TestArmInsideTempState.Start(i);
                }

                for (int i = 0; i < 2; i++)
                {
                    ColdPlateTempState = new Thread(SlaveUpdataColdPlateTempStateToHandler) { IsBackground = true };
                    ColdPlateTempState.Start(i);
                }

                for (int i = 0; i < 2; i++)
                {
                    HotPlatePlateTempState = new Thread(SlaveUpdataHotPlateTempStateToHandler) { IsBackground = true };
                    HotPlatePlateTempState.Start(i);
                }

                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    BorderPlateTempState = new Thread(SlaveUpdataBorderTempStateToHandler) { IsBackground = true };
                    BorderPlateTempState.Start(i);
                }

                _IsConnect = false;

                SP.Open();
                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        private static void Listen()
        {
            while (true)
            {
                Thread.Sleep(10);
                try
                {
                    NNodbusSlave.Listen();
                }
                catch (Exception ex)
                {
                    Flag.RunLogList.Add(ex.Message, Color.Red);
                }
            }
        }

        private static void NNodbusSlave_ModbusSlaveRequestReceived(object sender, ModbusSlaveRequestEventArgs e)
        {
            OverTime.Restart();
        }

        private static void SlaveUpdataToHandler()
        {
            Stopwatch[] TestArm_Ic_OneHeat = new Stopwatch[8];
            Stopwatch[] TestArm_Inside_OneHeat = new Stopwatch[8];
            Stopwatch[] ColdPlate_OneHeat = new Stopwatch[2];
            Stopwatch[] HotPlate_OneHeat = new Stopwatch[2];

            while (true)
            {
                Thread.Sleep(1000);

                #region 水箱液体相关

                NNodbusSlave.DataStore.HoldingRegisters[998] = Auxiliary.ShortToUshort((short)(Flag.WaterBox.Temp.Value * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[999] = Auxiliary.ShortToUshort((short)(Flag.WaterBox.Height.Value * 100f));

                #endregion

                #region 所有区域的当前温度

                NNodbusSlave.DataStore.HoldingRegisters[1000] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1001] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1002] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1003] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1004] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1005] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1006] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1007] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_IC.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1008] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1009] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1010] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1011] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1012] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1013] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1014] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1015] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_Inside.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1016] = Auxiliary.ShortToUshort((short)(Flag.ColdPlate[0].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1017] = Auxiliary.ShortToUshort((short)(Flag.ColdPlate[1].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1018] = Auxiliary.ShortToUshort((short)(Flag.HotPlate[0].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1019] = Auxiliary.ShortToUshort((short)(Flag.HotPlate[1].Heat.GetInData.PV * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1020] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[0].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1021] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[1].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1022] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[2].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1023] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[3].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1024] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[4].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1025] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[5].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1026] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[6].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1027] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[7].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1028] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[8].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1029] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[9].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1030] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[10].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1031] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[11].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1032] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[12].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1033] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[13].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1034] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[14].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1035] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[15].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1036] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[16].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1037] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[17].Heat.GetInData.PV * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1038] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[0].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1039] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[1].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1040] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[2].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1041] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[3].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1042] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[4].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1043] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[5].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1044] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[6].Heat.GetInData.PV * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1045] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[7].Heat.GetInData.PV * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1257] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1258] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1259] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1260] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1261] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1262] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1263] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_IC.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1264] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_IC.GetInPower.PowerLimit * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1265] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1266] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1267] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1268] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1269] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1270] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1271] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_Inside.GetInPower.PowerLimit * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1272] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_Inside.GetInPower.PowerLimit * 100f));
                #endregion

                #region 所有区域的设定温度

                NNodbusSlave.DataStore.HoldingRegisters[1100] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1101] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1102] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1103] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1104] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1105] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1106] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1107] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_IC.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1108] = Auxiliary.ShortToUshort((short)(Flag.TestArm[0].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1109] = Auxiliary.ShortToUshort((short)(Flag.TestArm[1].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1110] = Auxiliary.ShortToUshort((short)(Flag.TestArm[2].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1111] = Auxiliary.ShortToUshort((short)(Flag.TestArm[3].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1112] = Auxiliary.ShortToUshort((short)(Flag.TestArm[4].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1113] = Auxiliary.ShortToUshort((short)(Flag.TestArm[5].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1114] = Auxiliary.ShortToUshort((short)(Flag.TestArm[6].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1115] = Auxiliary.ShortToUshort((short)(Flag.TestArm[7].Heat_Inside.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1116] = Auxiliary.ShortToUshort((short)(Flag.ColdPlate[0].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1117] = Auxiliary.ShortToUshort((short)(Flag.ColdPlate[1].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1118] = Auxiliary.ShortToUshort((short)(Flag.HotPlate[0].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1119] = Auxiliary.ShortToUshort((short)(Flag.HotPlate[1].Heat.GetInData.SP * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1120] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[0].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1121] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[1].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1122] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[2].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1123] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[3].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1124] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[4].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1125] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[5].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1126] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[6].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1127] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[7].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1128] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[8].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1129] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[9].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1130] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[10].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1131] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[11].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1132] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[12].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1133] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[13].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1134] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[14].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1135] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[15].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1136] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[16].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1137] = Auxiliary.ShortToUshort((short)(Flag.BorderTemp[17].Heat.GetInData.SP * 100f));

                NNodbusSlave.DataStore.HoldingRegisters[1138] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[0].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1139] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[1].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1140] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[2].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1141] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[3].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1142] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[4].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1143] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[5].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1144] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[6].Heat.GetInData.SP * 100f));
                NNodbusSlave.DataStore.HoldingRegisters[1145] = Auxiliary.ShortToUshort((short)(Flag.OtherTemp[7].Heat.GetInData.SP * 100f));

                #endregion

                #region 所有区域的加热开启状态
                for (int i = 0; i < Flag.TestArm.Length; i++)
                {
                    NNodbusSlave.DataStore.HoldingRegisters[1146 + i] = (ushort)Function.Other.Bool_To_Int(Flag.TestArm[i].Heat_IC.GetInData.IsRun);
                    NNodbusSlave.DataStore.HoldingRegisters[1154 + i] = (ushort)Function.Other.Bool_To_Int(Flag.TestArm[i].Heat_Inside.GetInData.IsRun);
                }

                for (int i = 0; i < Flag.ColdPlate.Length; i++)
                {
                    NNodbusSlave.DataStore.HoldingRegisters[1162 + i] = (ushort)Function.Other.Bool_To_Int(Flag.ColdPlate[i].Heat.GetInData.IsRun);
                }

                for (int i = 0; i < Flag.HotPlate.Length; i++)
                {
                    NNodbusSlave.DataStore.HoldingRegisters[1164 + i] = (ushort)Function.Other.Bool_To_Int(Flag.HotPlate[i].Heat.GetInData.IsRun);
                }

                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    NNodbusSlave.DataStore.HoldingRegisters[1166 + i] = (ushort)Function.Other.Bool_To_Int(Flag.BorderTemp[i].Heat.GetInData.IsRun);
                }

                #endregion

                #region 所有区域的当前露点
                for (int i = 0; i < Flag.DewPoint.Length; i++)
                {
                    if (Flag.DewPoint[i].DewPoint == 9999)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1200 + i] = 32767;
                    }
                    else
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1200 + i] = Auxiliary.ShortToUshort((short)(Flag.DewPoint[i].DewPoint * 100f));
                    }
                }
                #endregion

                #region 循环泵异常码

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit[i].InternalPump.ErrCode != null)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1248 + i] = (ushort)(float.Parse(Flag.Unit[i].InternalPump.ErrCode) * 10f);
                    }
                }
                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Flag.ExternalPump[i].ErrCode != null)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1252 + i] = (ushort)(float.Parse(Flag.ExternalPump[i].ErrCode) * 10f);
                    }
                }

                #endregion

                #region 循环泵当前转速

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit != null)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1208 + i] = (ushort)Flag.Unit[i].InternalPump.Speed;
                        NNodbusSlave.DataStore.HoldingRegisters[1215 + i] = (ushort)Flag.Unit[i].InternalPump.OperatingLoad;
                    }
                }
                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Flag.ExternalPump != null)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1212 + i] = (ushort)Flag.ExternalPump[i].Speed;
                        NNodbusSlave.DataStore.HoldingRegisters[1219 + i] = (ushort)Flag.ExternalPump[i].OperatingLoad;
                    }
                }

                #endregion

                #region Chiller传感器自检

                if (Flag.CompressorErr == false)
                {
                    if (Work.Initial.SenserDetection(false) == true)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1299] = 1;
                    }
                    else
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1299] = 0;
                    }
                }
                else
                {
                    NNodbusSlave.DataStore.HoldingRegisters[1299] = 1;
                }

                #endregion
            }
        }

        private static void SlaveUpdataTestArmICTempStateToHandler(object Heat)
        {
            int Num = (int)Heat;

            Stopwatch OneHeat = new Stopwatch();
            Stopwatch StopHeat = new Stopwatch();
            Stopwatch StartHeat = new Stopwatch();
            Stopwatch StabTime = new Stopwatch();

            bool TempStab = false;
            bool OneHeatState = false;

            while (true)
            {
                Thread.Sleep(100);

                if (Flag.TestArm[Num].Heat_IC.GetInData.ErrCode != null)
                {
                    if (Flag.TestArm[Num].Heat_IC.GetInData.ErrCode == "Err")
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 16;
                    }
                    else if (Flag.TestArm[Num].Heat_IC.GetInData.ErrCode.Length > 4)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = ushort.Parse(Flag.TestArm[Num].Heat_IC.GetInData.ErrCode.Replace("Err:", ""));
                    }
                    else if (Flag.TestArm[Num].Heat_IC.GetInData.ErrCode == "")
                    {
                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            OneHeat.Stop();
                            StartHeat.Stop();
                            StabTime.Stop();

                            TempStab = false;
                            OneHeatState = false;


                            if (Flag.TestArm[Num].Heat_IC.GetInData.IsRun == true)
                            {
                                #region 加热关闭失败 状态码：22
                                if (NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] != 22)
                                {
                                    if (StopHeat.IsRunning == false)
                                    {
                                        StopHeat.Restart();
                                    }

                                    if (StopHeat.ElapsedMilliseconds >= 10000)
                                    {
                                        NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 22;
                                    }
                                }
                                else
                                {
                                    StopHeat.Stop();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 温控器停止状态 状态码：17
                                StopHeat.Stop();
                                NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 17;
                                #endregion
                            }
                        }
                        else
                        {
                            StopHeat.Stop();

                            if (Flag.TestArm[Num].Heat_IC.SetToHeat.Enabled == true && Flag.TestArm[Num].Heat_IC.SetToHeat.IsUseHeat==true)
                            {
                                if (Flag.TestArm[Num].Heat_IC.GetInData.IsRun == false)
                                {
                                    #region 加热开启，但是温控器并未开始加热（加热开启失败 状态码：21）

                                    if (NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] != 21)
                                    {
                                        if (StartHeat.IsRunning == false)
                                        {
                                            StartHeat.Restart();
                                        }

                                        if (StartHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 21;
                                        }
                                    }
                                    else
                                    {
                                        StartHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Flag.TestArm[Num].Heat_IC.SetToHeat.ChangeState == true)
                                    {
                                        Flag.TestArm[Num].Heat_IC.SetToHeat.ChangeState = false;
                                        OneHeatState = false;
                                        TempStab = false;
                                        StabTime.Stop();
                                        OneHeat.Stop();
                                    }


                                    if (Flag.TestArm[Num].Heat_IC.GetInData.PV < Flag.TestArm[Num].Heat_IC.SetToHeat.Temp + Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[478 ]) &&
                                       Flag.TestArm[Num].Heat_IC.GetInData.PV > Flag.TestArm[Num].Heat_IC.SetToHeat.Temp - Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[478 ]))
                                    {
                                        #region 温度稳定 状态码：19

                                        OneHeatState = true;

                                        if (TempStab == false)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[486 ]*1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 19;
                                                TempStab = true;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 温度超限 状态码：20
                                        if (OneHeatState == true && TempStab == true)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[486] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 20;
                                                TempStab = false;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }
                                        #endregion
                                    }

                                    #region 在指定时间是否能达到对应温度范围（初始加热超时 状态码：23     初始加热 状态码：18）
                                    if (OneHeatState == false)
                                    {
                                        if (OneHeat.IsRunning == false)
                                        {
                                            OneHeat.Restart();
                                        }

                                        if (OneHeat.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[482] * 1000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 23;
                                        }
                                        else
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 18;
                                        }
                                    }
                                    else
                                    {
                                        OneHeat.Stop();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (Flag.TestArm[Num].Heat_IC.GetInData.IsRun == true)
                                {
                                    #region 加热关闭失败 状态码：22
                                    if (NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] != 22)
                                    {
                                        if (StopHeat.IsRunning == false)
                                        {
                                            StopHeat.Restart();
                                        }

                                        if (StopHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 22;
                                        }
                                    }
                                    else
                                    {
                                        StopHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 温控器停止状态 状态码：17
                                    StopHeat.Stop();
                                    NNodbusSlave.DataStore.HoldingRegisters[1300 + Num] = 17;
                                    #endregion
                                }
                            }
                        }
                    }

                }
            }
        }

        private static void SlaveUpdataTestArmInsedeTempStateToHandler(object Heat)
        {
            int Num = (int)Heat;

            Stopwatch OneHeat = new Stopwatch();
            Stopwatch StopHeat = new Stopwatch();
            Stopwatch StartHeat = new Stopwatch();
            Stopwatch StabTime = new Stopwatch();

            bool TempStab = false;
            bool OneHeatState = false;

            while (true)
            {
                Thread.Sleep(100);

                if (Flag.TestArm[Num].Heat_Inside.GetInData.ErrCode != null)
                {
                    if (Flag.TestArm[Num].Heat_Inside.GetInData.ErrCode == "Err")
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 16;
                    }
                    else if (Flag.TestArm[Num].Heat_Inside.GetInData.ErrCode.Length > 4)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = ushort.Parse(Flag.TestArm[Num].Heat_Inside.GetInData.ErrCode.Replace("Err:", ""));
                    }
                    else if (Flag.TestArm[Num].Heat_Inside.GetInData.ErrCode == "")
                    {
                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            OneHeat.Stop();
                            StartHeat.Stop();
                            StabTime.Stop();

                            TempStab = false;
                            OneHeatState = false;


                            if (Flag.TestArm[Num].Heat_Inside.GetInData.IsRun == true)
                            {
                                #region 加热关闭失败 状态码：22
                                if (NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] != 22)
                                {
                                    if (StopHeat.IsRunning == false)
                                    {
                                        StopHeat.Restart();
                                    }

                                    if (StopHeat.ElapsedMilliseconds >= 10000)
                                    {
                                        NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 22;
                                    }
                                }
                                else
                                {
                                    StopHeat.Stop();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 温控器停止状态 状态码：17
                                StopHeat.Stop();
                                NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 17;
                                #endregion
                            }
                        }
                        else
                        {
                            StopHeat.Stop();

                            if (Flag.TestArm[Num].Heat_Inside.SetToHeat.Enabled == true && Flag.TestArm[Num].Heat_Inside.SetToHeat.IsUseHeat == true)
                            {
                                if (Flag.TestArm[Num].Heat_Inside.GetInData.IsRun == false)
                                {
                                    #region 加热开启，但是温控器并未开始加热（加热开启失败 状态码：21）

                                    if (NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] != 21)
                                    {
                                        if (StartHeat.IsRunning == false)
                                        {
                                            StartHeat.Restart();
                                        }

                                        if (StartHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 21;
                                        }
                                    }
                                    else
                                    {
                                        StartHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Flag.TestArm[Num].Heat_Inside.SetToHeat.ChangeState == true)
                                    {
                                        Flag.TestArm[Num].Heat_Inside.SetToHeat.ChangeState = false;
                                        OneHeatState = false;
                                        TempStab = false;
                                        StabTime.Stop();
                                        OneHeat.Stop();
                                    }

                                    if (Flag.TestArm[Num].Heat_Inside.GetInData.PV < Flag.TestArm[Num].Heat_Inside.SetToHeat.Temp + Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[478]) &&
                                       Flag.TestArm[Num].Heat_Inside.GetInData.PV > Flag.TestArm[Num].Heat_Inside.SetToHeat.Temp - Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[478]))
                                    {
                                        #region 温度稳定 状态码：19

                                        OneHeatState = true;

                                        if (TempStab == false)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[486]*1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 19;
                                                TempStab = true;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 温度超限 状态码：20
                                        if (OneHeatState == true && TempStab == true)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[486 ] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 20;
                                                TempStab = false;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }
                                        #endregion
                                    }

                                    #region 在指定时间是否能达到对应温度范围（初始加热超时 状态码：23     初始加热 状态码：18）
                                    if (OneHeatState == false)
                                    {
                                       
                                            if (OneHeat.IsRunning == false)
                                            {
                                                OneHeat.Restart();
                                            }

                                            if (OneHeat.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[482] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 23;
                                            }
                                            else
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 18;
                                            }
                                       
                                    }
                                    else
                                    {
                                        OneHeat.Stop();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (Flag.TestArm[Num].Heat_Inside.GetInData.IsRun == true)
                                {
                                    #region 加热关闭失败 状态码：22
                                    if (NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] != 22)
                                    {
                                        if (StopHeat.IsRunning == false)
                                        {
                                            StopHeat.Restart();
                                        }

                                        if (StopHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 22;
                                        }
                                    }
                                    else
                                    {
                                        StopHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 温控器停止状态 状态码：17
                                    StopHeat.Stop();
                                    NNodbusSlave.DataStore.HoldingRegisters[1308 + Num] = 17;
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SlaveUpdataColdPlateTempStateToHandler(object Heat)
        {
            int Num = (int)Heat;

            Stopwatch OneHeat = new Stopwatch();
            Stopwatch StopHeat = new Stopwatch();
            Stopwatch StartHeat = new Stopwatch();
            Stopwatch StabTime = new Stopwatch();

            bool TempStab = false;
            bool OneHeatState = false;

            while (true)
            {
                Thread.Sleep(100);

                if (Flag.ColdPlate[Num].Heat.GetInData.ErrCode != null)
                {
                    if (Flag.ColdPlate[Num].Heat.GetInData.ErrCode == "Err")
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 16;
                    }
                    else if (Flag.ColdPlate[Num].Heat.GetInData.ErrCode.Length > 4)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = ushort.Parse(Flag.ColdPlate[Num].Heat.GetInData.ErrCode.Replace("Err:", ""));
                    }
                    else if (Flag.ColdPlate[Num].Heat.GetInData.ErrCode == "")
                    {
                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            OneHeat.Stop();
                            StartHeat.Stop();
                            StabTime.Stop();

                            TempStab = false;
                            OneHeatState = false;


                            if (Flag.ColdPlate[Num].Heat.GetInData.IsRun == true)
                            {
                                #region 加热关闭失败 状态码：22
                                if (NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] != 22)
                                {
                                    if (StopHeat.IsRunning == false)
                                    {
                                        StopHeat.Restart();
                                    }

                                    if (StopHeat.ElapsedMilliseconds >= 10000)
                                    {
                                        NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 22;
                                    }
                                }
                                else
                                {
                                    StopHeat.Stop();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 温控器停止状态 状态码：17
                                StopHeat.Stop();
                                NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 17;
                                #endregion
                            }
                        }
                        else
                        {
                            StopHeat.Stop();

                            if (Flag.ColdPlate[Num].Heat.SetToHeat.Enabled == true)
                            {
                                if (Flag.ColdPlate[Num].Heat.GetInData.IsRun == false)
                                {
                                    #region 加热开启，但是温控器并未开始加热（加热开启失败 状态码：21）

                                    if (NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] != 21)
                                    {
                                        if (StartHeat.IsRunning == false)
                                        {
                                            StartHeat.Restart();
                                        }

                                        if (StartHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 21;
                                        }
                                    }
                                    else
                                    {
                                        StartHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Flag.ColdPlate[Num].Heat.SetToHeat.ChangeState == true)
                                    {
                                        Flag.ColdPlate[Num].Heat.SetToHeat.ChangeState = false;
                                        OneHeatState = false;
                                        TempStab = false;
                                        StabTime.Stop();
                                        OneHeat.Stop();
                                    }

                                    if (Flag.ColdPlate[Num].Heat.GetInData.PV < Flag.ColdPlate[Num].Heat.SetToHeat.Temp + Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[479]) &&
                                       Flag.ColdPlate[Num].Heat.GetInData.PV > Flag.ColdPlate[Num].Heat.SetToHeat.Temp - Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[479]))
                                    {
                                        #region 温度稳定 状态码：19

                                        OneHeatState = true;

                                        if (TempStab == false)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[487] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 19;
                                                TempStab = true;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 温度超限 状态码：20
                                        if (OneHeatState == true && TempStab == true)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[487 ] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 20;
                                                TempStab = false;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }
                                        #endregion
                                    }

                                    #region 在指定时间是否能达到对应温度范围（初始加热超时 状态码：23     初始加热 状态码：18）
                                    if (OneHeatState == false)
                                    {
                                      
                                            if (OneHeat.IsRunning == false)
                                            {
                                                OneHeat.Restart();
                                            }

                                            if (OneHeat.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[483 ] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 23;
                                            }
                                            else
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 18;
                                            }
                                        
                                    }
                                    else
                                    {
                                        OneHeat.Stop();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (Flag.ColdPlate[Num].Heat.GetInData.IsRun == true)
                                {
                                    #region 加热关闭失败 状态码：22
                                    if (NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] != 22)
                                    {
                                        if (StopHeat.IsRunning == false)
                                        {
                                            StopHeat.Restart();
                                        }

                                        if (StopHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 22;
                                        }
                                    }
                                    else
                                    {
                                        StopHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 温控器停止状态 状态码：17
                                    StopHeat.Stop();
                                    NNodbusSlave.DataStore.HoldingRegisters[1316 + Num] = 17;
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SlaveUpdataHotPlateTempStateToHandler(object Heat)
        {
            int Num = (int)Heat;

            Stopwatch OneHeat = new Stopwatch();
            Stopwatch StopHeat = new Stopwatch();
            Stopwatch StartHeat = new Stopwatch();
            Stopwatch StabTime = new Stopwatch();

            bool TempStab = false;
            bool OneHeatState = false;

            while (true)
            {
                Thread.Sleep(100);

                if (Flag.HotPlate[Num].Heat.GetInData.ErrCode != null)
                {
                    if (Flag.HotPlate[Num].Heat.GetInData.ErrCode == "Err")
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 16;
                    }
                    else if (Flag.HotPlate[Num].Heat.GetInData.ErrCode.Length > 4)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = ushort.Parse(Flag.HotPlate[Num].Heat.GetInData.ErrCode.Replace("Err:", ""));
                    }
                    else if (Flag.HotPlate[Num].Heat.GetInData.ErrCode == "")
                    {
                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            OneHeat.Stop();
                            StartHeat.Stop();
                            StabTime.Stop();

                            TempStab = false;
                            OneHeatState = false;

                            if (Flag.HotPlate[Num].Heat.GetInData.IsRun == true)
                            {
                                #region 加热关闭失败 状态码：22
                                if (NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] != 22)
                                {
                                    if (StopHeat.IsRunning == false)
                                    {
                                        StopHeat.Restart();
                                    }

                                    if (StopHeat.ElapsedMilliseconds >= 10000)
                                    {
                                        NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 22;
                                    }
                                }
                                else
                                {
                                    StopHeat.Stop();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 温控器停止状态 状态码：17
                                StopHeat.Stop();
                                NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 17;
                                #endregion
                            }
                        }
                        else
                        {
                            StopHeat.Stop();

                            if (Flag.HotPlate[Num].Heat.SetToHeat.Enabled == true)
                            {
                                if (Flag.HotPlate[Num].Heat.GetInData.IsRun == false)
                                {
                                    #region 加热开启，但是温控器并未开始加热（加热开启失败 状态码：21）

                                    if (NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] != 21)
                                    {
                                        if (StartHeat.IsRunning == false)
                                        {
                                            StartHeat.Restart();
                                        }

                                        if (StartHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 21;
                                        }
                                    }
                                    else
                                    {
                                        StartHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Flag.HotPlate[Num].Heat.SetToHeat.ChangeState == true)
                                    {
                                        Flag.HotPlate[Num].Heat.SetToHeat.ChangeState = false;
                                        OneHeatState = false;
                                        TempStab = false;
                                        StabTime.Stop();
                                        OneHeat.Stop();
                                    }

                                    if (Flag.HotPlate[Num].Heat.GetInData.PV < Flag.HotPlate[Num].Heat.SetToHeat.Temp + Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[480]) &&
                                       Flag.HotPlate[Num].Heat.GetInData.PV > Flag.HotPlate[Num].Heat.SetToHeat.Temp - Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[480]))
                                    {
                                        #region 温度稳定 状态码：19

                                        OneHeatState = true;

                                        if (TempStab == false)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[488] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 19;
                                                TempStab = true;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 温度超限 状态码：20
                                        if (OneHeatState == true && TempStab == true)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[488 ] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 20;
                                                TempStab = false;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }
                                        #endregion
                                    }

                                    #region 在指定时间是否能达到对应温度范围（初始加热超时 状态码：23     初始加热 状态码：18）
                                    if (OneHeatState == false)
                                    {
                                       
                                            if (OneHeat.IsRunning == false)
                                            {
                                                OneHeat.Restart();
                                            }

                                            if (OneHeat.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[484 ] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 23;
                                            }
                                            else
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 18;
                                            }
                                        
                                    }
                                    else
                                    {
                                        OneHeat.Stop();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (Flag.HotPlate[Num].Heat.GetInData.IsRun == true)
                                {
                                    #region 加热关闭失败 状态码：22
                                    if (NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] != 22)
                                    {
                                        if (StopHeat.IsRunning == false)
                                        {
                                            StopHeat.Restart();
                                        }

                                        if (StopHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 22;
                                        }
                                    }
                                    else
                                    {
                                        StopHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 温控器停止状态 状态码：17
                                    StopHeat.Stop();
                                    NNodbusSlave.DataStore.HoldingRegisters[1318 + Num] = 17;
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SlaveUpdataBorderTempStateToHandler(object Heat)
        {
            int Num = (int)Heat;

            Stopwatch OneHeat = new Stopwatch();
            Stopwatch StopHeat = new Stopwatch();
            Stopwatch StartHeat = new Stopwatch();
            Stopwatch StabTime = new Stopwatch();

            bool TempStab = false;
            bool OneHeatState = false;

            while (true)
            {
                Thread.Sleep(100);

                if (Flag.BorderTemp[Num].Heat.GetInData.ErrCode != null)
                {
                    if (Flag.BorderTemp[Num].Heat.GetInData.ErrCode == "Err")
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 16;
                    }
                    else if (Flag.BorderTemp[Num].Heat.GetInData.ErrCode.Length > 4)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = ushort.Parse(Flag.BorderTemp[Num].Heat.GetInData.ErrCode.Replace("Err:", ""));
                    }
                    else if (Flag.BorderTemp[Num].Heat.GetInData.ErrCode == "")
                    {
                        if (Flag.StartEnabled.RunHeat.Heating == false)
                        {
                            OneHeat.Stop();
                            StartHeat.Stop();
                            StabTime.Stop();

                            TempStab = false;
                            OneHeatState = false;

                            if (Flag.BorderTemp[Num].Heat.GetInData.IsRun == true)
                            {
                                #region 加热关闭失败 状态码：22
                                if (NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] != 22)
                                {
                                    if (StopHeat.IsRunning == false)
                                    {
                                        StopHeat.Restart();
                                    }

                                    if (StopHeat.ElapsedMilliseconds >= 10000)
                                    {
                                        NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 22;
                                    }
                                }
                                else
                                {
                                    StopHeat.Stop();
                                }
                                #endregion
                            }
                            else
                            {
                                #region 温控器停止状态 状态码：17
                                StopHeat.Stop();
                                NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 17;
                                #endregion
                            }
                        }
                        else
                        {
                            StopHeat.Stop();

                            if (Flag.BorderTemp[Num].Heat.SetToHeat.Enabled == true)
                            {
                                if (Flag.BorderTemp[Num].Heat.GetInData.IsRun == false)
                                {
                                    #region 加热开启，但是温控器并未开始加热（加热开启失败 状态码：21）

                                    if (NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] != 21)
                                    {
                                        if (StartHeat.IsRunning == false)
                                        {
                                            StartHeat.Restart();
                                        }

                                        if (StartHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 21;
                                        }
                                    }
                                    else
                                    {
                                        StartHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (Flag.BorderTemp[Num].Heat.SetToHeat.ChangeState == true)
                                    {
                                        Flag.BorderTemp[Num].Heat.SetToHeat.ChangeState = false;
                                        OneHeatState = false;
                                        TempStab = false;
                                        StabTime.Stop();
                                        OneHeat.Stop();
                                    }

                                    if (Flag.BorderTemp[Num].Heat.GetInData.PV < Flag.BorderTemp[Num].Heat.SetToHeat.Temp + Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[481]) &&
                                       Flag.BorderTemp[Num].Heat.GetInData.PV > Flag.BorderTemp[Num].Heat.SetToHeat.Temp - Auxiliary.UshortToShort(NNodbusSlave.DataStore.HoldingRegisters[481]))
                                    {
                                        #region 温度稳定 状态码：19

                                        OneHeatState = true;

                                        if (TempStab == false)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[489] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 19;
                                                TempStab = true;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 温度超限 状态码：20
                                        if (OneHeatState == true && TempStab == true)
                                        {
                                            if (StabTime.IsRunning == false)
                                            {
                                                StabTime.Restart();
                                            }

                                            if (StabTime.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[489] * 1000)
                                            {
                                                NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 20;
                                                TempStab = false;
                                            }
                                        }
                                        else
                                        {
                                            StabTime.Stop();
                                        }
                                        #endregion
                                    }

                                    #region 在指定时间是否能达到对应温度范围（初始加热超时 状态码：23     初始加热 状态码：18）
                                    if (OneHeatState == false)
                                    {

                                        if (OneHeat.IsRunning == false)
                                        {
                                            OneHeat.Restart();
                                        }

                                        if (OneHeat.ElapsedMilliseconds >= NNodbusSlave.DataStore.HoldingRegisters[485] * 1000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 23;
                                        }
                                        else
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 18;
                                        }

                                    }
                                    else
                                    {
                                        OneHeat.Stop();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {

                                if (Flag.BorderTemp[Num].Heat.GetInData.IsRun == true)
                                {
                                    #region 加热关闭失败 状态码：22
                                    if (NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] != 22)
                                    {
                                        if (StopHeat.IsRunning == false)
                                        {
                                            StopHeat.Restart();
                                        }

                                        if (StopHeat.ElapsedMilliseconds >= 10000)
                                        {
                                            NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 22;
                                        }
                                    }
                                    else
                                    {
                                        StopHeat.Stop();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 温控器停止状态 状态码：17
                                    StopHeat.Stop();
                                    NNodbusSlave.DataStore.HoldingRegisters[1320 + Num] = 17;
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SlaveUpDataToChiller()
        {
            while (true)
            {
                Thread.Sleep(10);

                #region 控制权超时
                if (Flag.Handler.ProhibitHandlerControl == false)
                {
                    if (OverTime.ElapsedMilliseconds >= 300000)
                    {
                        NNodbusSlave.DataStore.HoldingRegisters[101] = 0;
                    }

                    if (NNodbusSlave.DataStore.HoldingRegisters[101] == 1)
                    {
                        Flag.Handler.IsHandlerControl = true;
                    }
                    else
                    {
                        Flag.Handler.IsHandlerControl = false;
                    }
                }
                else
                {
                    OverTime.Stop();
                    Flag.Handler.IsHandlerControl = false;
                }
                #endregion

                if (Flag.Handler.IsHandlerControl == true)
                {
                    if (NNodbusSlave.DataStore.HoldingRegisters[103] == 1 && Flag.StartEnabled.RunHeat.Heating == false)
                    {
                        TempControl.ExternalTempControl.RunOrStopTempControl(true);
                    }
                    else if (NNodbusSlave.DataStore.HoldingRegisters[103] == 0 && Flag.StartEnabled.RunHeat.Heating == true)
                    {
                        TempControl.ExternalTempControl.RunOrStopTempControl(false);
                    }
                }
            }
        }

        /// <summary> 
        /// 打开串口
        /// </summary>
        public static bool Open()
        {
            try
            {
                if (SP.IsOpen == true)
                {
                    SP.Close();
                    SP.Open();
                }
                else
                {
                    SP.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> 
        /// 关闭串口
        /// </summary>
        public static bool Close()
        {
            try
            {
                if (SP.IsOpen == true)
                {
                    SP.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> 
        /// 获取串口是否打开
        /// </summary>
        public static bool IsOpen()
        {
            if (SP == null)
            {
                return false;
            }
            else
            {
                return SP.IsOpen;
            }
        }

        public static bool IsConnect
        {
            get { return _IsConnect; }
        }

    }
}
