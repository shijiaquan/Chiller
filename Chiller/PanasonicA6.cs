using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;

namespace Chiller
{
    public class PanasonicA6
    {
        public struct A6ModbusBody
        {
            /// <summary>
            /// 是否支持清除的报警
            /// </summary>
            public bool IsErrClear;
            /// <summary>
            /// 是否报警
            /// </summary>
            public bool IsErr;
            /// <summary>
            /// 错误代码
            /// </summary>
            public string ErrCode;
            /// <summary>
            /// 电机负载
            /// </summary>
            public int OperatingLoad;
            /// <summary>
            /// 电机当前是否处于运动状态
            /// </summary>
            public bool IsRun;
            /// <summary>
            /// 驱动器子站连接状态
            /// </summary>
            public bool IsConnent;
            /// <summary>
            /// 与驱动器通讯失败次数
            /// </summary>
            public int ConnentErrNumber;
            /// <summary>
            /// 编码器的当前温度
            /// </summary>
            public short EncoderTemp;
            /// <summary>
            /// 电机当前转速
            /// </summary>
            public ushort NowSpeed;
            /// <summary>
            /// 设定的运行速度
            /// </summary>
            public ushort SetSpeed;
        }

        public struct ErrCode
        {
            /// <summary>
            /// 报警代码
            /// </summary>
            public string Code;
            /// <summary>
            /// 是否可以清除
            /// </summary>
            public bool IsClear;
        }

        public static A6ModbusBody[] A6_Channel;
        private static ErrCode[] _A6_ErrCodeList;

        private static SerialPort SP;
        private static ModbusSerialMaster PanasonicA6Master;

        private static object LockValue = 0;
        private static bool _PauseRead = false;

        /// <summary>
        /// 初始化驱动器连接
        /// </summary>
        /// <param name="Port">串口号</param> 
        /// <param name="Baud">通讯波特率</param> 
        public static bool Initialization(string Port, int Baud, int A6_Channel_Num)
        {
            try
            {
                InitializationErrCode();

                A6_Channel = new A6ModbusBody[A6_Channel_Num];

                Thread GetValue = new Thread(Get_A6_Value) { IsBackground = true };
                GetValue.Start();

                SP = new SerialPort(Port, Baud, Parity.Even, 8, StopBits.One);
                SP.Open();

                PanasonicA6Master = ModbusSerialMaster.CreateRtu(SP);
                PanasonicA6Master.Transport.ReadTimeout = 200;
                PanasonicA6Master.Transport.WriteTimeout = 200;
                PanasonicA6Master.Transport.SlaveBusyUsesRetryCount = true;
                PanasonicA6Master.Transport.Retries = 5;
                PanasonicA6Master.Transport.WaitToRetryMilliseconds = 5;
                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 松下驱动器报警枚举
        /// </summary>
        public static void InitializationErrCode()
        {
            _A6_ErrCodeList = new ErrCode[21];
            _A6_ErrCodeList[0].Code = "11.0";
            _A6_ErrCodeList[0].IsClear = true;

            _A6_ErrCodeList[1].Code = "12.0";
            _A6_ErrCodeList[1].IsClear = true;

            _A6_ErrCodeList[2].Code = "13.0";
            _A6_ErrCodeList[2].IsClear = true;

            _A6_ErrCodeList[3].Code = "13.1";
            _A6_ErrCodeList[3].IsClear = true;

            _A6_ErrCodeList[4].Code = "14.0";
            _A6_ErrCodeList[4].IsClear = true;

            _A6_ErrCodeList[5].Code = "14.1";
            _A6_ErrCodeList[5].IsClear = true;

            _A6_ErrCodeList[6].Code = "15.0";
            _A6_ErrCodeList[6].IsClear = false;

            _A6_ErrCodeList[7].Code = "15.1";
            _A6_ErrCodeList[7].IsClear = false;

            _A6_ErrCodeList[8].Code = "16.0";
            _A6_ErrCodeList[8].IsClear = false;

            _A6_ErrCodeList[9].Code = "16.1";
            _A6_ErrCodeList[9].IsClear = false;

            _A6_ErrCodeList[10].Code = "18.0";
            _A6_ErrCodeList[10].IsClear = false;

            _A6_ErrCodeList[11].Code = "18.1";
            _A6_ErrCodeList[11].IsClear = false;

            _A6_ErrCodeList[12].Code = "21.0";
            _A6_ErrCodeList[12].IsClear = false;

            _A6_ErrCodeList[13].Code = "21.1";
            _A6_ErrCodeList[13].IsClear = false;

            _A6_ErrCodeList[14].Code = "93.0";
            _A6_ErrCodeList[14].IsClear = false;

            _A6_ErrCodeList[15].Code = "93.1";
            _A6_ErrCodeList[15].IsClear = true;

            _A6_ErrCodeList[16].Code = "93.2";
            _A6_ErrCodeList[16].IsClear = false;

            _A6_ErrCodeList[17].Code = "93.3";
            _A6_ErrCodeList[17].IsClear = false;

            _A6_ErrCodeList[18].Code = "93.8";
            _A6_ErrCodeList[18].IsClear = false;

            _A6_ErrCodeList[19].Code = "94.0";
            _A6_ErrCodeList[19].IsClear = true;

            _A6_ErrCodeList[20].Code = "94.2";
            _A6_ErrCodeList[20].IsClear = true;
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
        public static bool IsOpen
        {
            get
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
        }

        /// <summary>
        /// 获取A6总报警代码列表
        /// </summary>
        public static ErrCode[] A6_ErrCodeList
        {
            get
            {
                return _A6_ErrCodeList;
            }
        }
        /// <summary>
        /// 执行其他运行耗时操作或需求高响应时可暂停读取报警、以及负载信息
        /// </summary>
        public static bool PauseRead
        {
            set
            {
                _PauseRead = value;
            }
            get
            {
                return _PauseRead;
            }
        }

        /// <summary>
        /// 伺服上电
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="state">伺服上电 = 1  伺服断电 = 0</param> 
        public static bool SetServo(byte Address, bool State)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleCoil(Address, 0096, State);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取伺服上电状态,1为使能，0为未使能，-1读取错误
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool GetServo(byte Address, ref bool StateOut)
        {
            try
            {
                bool[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadCoils(Address, 0032, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    StateOut = ReturnData[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 电机停止
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool Stop(byte Address)
        {
            bool _Run = false;

            //查询是否在运行

            if (IsRun(Address, ref _Run) == false)
            {
                return false;
            }

            if (_Run == false)
            {
                return true;
            }

            if (Stop(Address, true) == false)
            {
                return false;
            }

            bool RunState = true;
            Stopwatch sw = new Stopwatch();
            sw.Restart();


            while (RunState == true && sw.ElapsedMilliseconds <= 1000)
            {
                Thread.Sleep(5);
                if (IsRun(Address, ref _Run) == true)
                {
                    RunState = _Run;
                }
            }

            if (sw.ElapsedMilliseconds <= 1000)
            {
                if (Stop(Address, false) == false)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 电机停止
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool Stop(byte Address, bool OnOff)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleCoil(Address, 0292, OnOff);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改当前运行速度
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="speed">设定的速度</param> 
        /// <param name="IsUse">是否更新当前速度</param> 
        public static bool SetSpeed(byte Address, ushort speed)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    if (speed > 1000)
                    {
                        speed = 1000;
                    }
                    PanasonicA6Master.WriteSingleRegister(Address, 17920, speed);

                    A6_Channel[Address - 1].SetSpeed = speed;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取设定速度
        /// </summary>
        /// <param name="address">站号</param>
        /// <param name="SpeedOut">读取设定的速度</param>
        /// <returns>成功或失败</returns>
        public static bool GetSetSpeed(byte Address, ref ushort SpeedOut)
        {
            try
            {
                ushort[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadHoldingRegisters(Address, 17920, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    SpeedOut = ReturnData[0];
                    A6_Channel[Address - 1].SetSpeed = SpeedOut;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取电机当前速度
        /// </summary>
        /// <param name="address">站号</param>
        /// <param name="SpeedOut">读取的速度</param>
        /// <returns>成功或失败</returns>
        public static bool GetNowSpeed(byte Address, ref ushort SpeedOut)
        {
            try
            {
                ushort[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadHoldingRegisters(Address, 16421, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    SpeedOut = (ushort)Math.Abs(Auxiliary.UshortToShort(ReturnData[0]));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改当前加速时间
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="speed">设定的加速时间，单位ms</param> 
        public static bool TAcc(byte Address, ushort speed)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleRegister(Address, 17936, speed);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改当前减速时间
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="speed">设定的减速时间，单位ms</param> 
        public static bool TDec(byte Address, ushort speed)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleRegister(Address, 17952, speed);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 电机正转
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="speed">设定的运行速度</param> 
        public static bool FWD(byte Address, ushort speed)
        {
            bool _Servo = false;
            bool _Stop = false;
            bool _Run = false;
            ushort _Speed = 0;
            Stopwatch SW;
            bool RunState;

            #region 速度不允许为＜0

            if (speed <= 0)
            {
                return false;
            }

            #endregion

            #region 查询使能状态
            //查询伺服使能
            if (GetServo(Address, ref _Servo) == false)
            {
                return false;
            }

            //判断伺服使能
            if (_Servo == false)
            {
                if (SetServo(Address, true) == true)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region 电机动作

            //查询是否在运行与设定的速度
            if (IsRun(Address, ref _Run) == false)
            {
                return false;
            }
            Thread.Sleep(2);
            if (GetSetSpeed(Address, ref _Speed) == false)
            {
                return false;
            }

            //判断电机运行
            if (_Run == true && _Speed != speed)
            {
                #region 停止电机
                Thread.Sleep(2);
                if (Stop(Address) == false)
                {
                    return false;
                }

                RunState = true;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == true && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }

                #endregion

                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 电机速度更新
                //更新速度进速度V0
                if (SetSpeed(Address, speed) == false)
                {
                    return false;
                }
                Thread.Sleep(10);
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 0) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == false && _Speed != speed)
            {
                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 电机速度更新
                //更新速度进速度V0
                if (SetSpeed(Address, speed) == false)
                {
                    return false;
                }
                Thread.Sleep(10);
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 0) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == false && _Speed == speed)
            {
                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 0) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == true && _Speed == speed)
            {
                return true;
            }
            else
            {
                return false;
            }

            #endregion
        }

        /// <summary>
        /// 电机反转
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="speed">设定的运行速度</param> 
        public static bool REV(byte Address, ushort speed)
        {
            bool _Servo = false;
            bool _Stop = false;
            bool _Run = false;
            ushort _Speed = 0;
            Stopwatch SW;
            bool RunState;

            #region 速度不允许为＜0

            if (speed <= 0)
            {
                return false;
            }

            #endregion

            #region 查询使能状态
            //查询伺服使能
            if (GetServo(Address, ref _Servo) == false)
            {
                return false;
            }

            //判断伺服使能
            if (_Servo == false)
            {
                if (SetServo(Address, true) == true)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region 电机动作

            //查询是否在运行与设定的速度
            if (IsRun(Address, ref _Run) == false)
            {
                return false;
            }
            Thread.Sleep(2);
            if (GetSetSpeed(Address, ref _Speed) == false)
            {
                return false;
            }

            //判断电机运行
            if (_Run == true && _Speed != speed)
            {
                #region 停止电机
                Thread.Sleep(2);
                if (Stop(Address) == false)
                {
                    return false;
                }

                RunState = true;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == true && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }

                #endregion

                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 电机速度更新
                //更新速度进速度V0
                if (SetSpeed(Address, speed) == false)
                {
                    return false;
                }
                Thread.Sleep(10);
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 1) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == false && _Speed != speed)
            {
                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 电机速度更新
                //更新速度进速度V0
                if (SetSpeed(Address, speed) == false)
                {
                    return false;
                }
                Thread.Sleep(10);
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 1) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == false && _Speed == speed)
            {
                #region 查询电机停止状态
                //查询电机是否停止信号为On
                if (IsStop(Address, ref _Stop) == false)
                {
                    return false;
                }

                //判断电机是否停止信号为On
                if (_Stop == true)
                {
                    if (Stop(Address, false) == false)
                    {
                        return false;
                    }
                }
                #endregion

                #region 重启电机动作
                //启动Block[0]
                if (A6BlockOn(Address, 1) == false)
                {
                    return false;
                }
                #endregion

                #region 判断电机运行状态

                RunState = false;
                SW = new Stopwatch();
                SW.Restart();

                while (RunState == false && SW.ElapsedMilliseconds <= 1000)
                {
                    Thread.Sleep(5);
                    if (IsRun(Address, ref _Run) == true)
                    {
                        RunState = _Run;
                    }
                }

                if (SW.ElapsedMilliseconds > 1000)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                #endregion
            }
            else if (_Run == true && _Speed == speed)
            {
                return true;
            }
            else
            {
                return false;
            }

            #endregion
        }

        /// <summary>
        /// 电机是否停止状态
        /// </summary>
        /// <param name="address">地址0-31</param> 
        private static bool IsStop(byte Address, ref bool StateOut)
        {
            try
            {
                bool[] ReturnData;
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadCoils(Address, 0307, 02);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    if (ReturnData[0] == false && ReturnData[1] == false)
                    {
                        StateOut = false;
                    }
                    else
                    {
                        StateOut = true;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断当前伺服驱动器是否报警中
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool IsErr(byte Address, ref bool StateOut)
        {
            try
            {
                bool[] ReturnData;
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadCoils(Address, 0161, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    StateOut = ReturnData[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断当前电机地址电机是否正在运行
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool IsRun(byte Address, ref bool StateOut)
        {
            try
            {
                bool[] ReturnData;
                bool[] Busy;
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadCoils(Address, 0258, 02);
                    Thread.Sleep(2);
                    Busy = PanasonicA6Master.ReadCoils(Address, 0320, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0 && Busy != null && Busy.Length != 0)
                {
                    if (ReturnData[0] == false && ReturnData[1] == false && Busy[0] == false)
                    {
                        StateOut = false;
                    }
                    else
                    {
                        StateOut = true;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断当前电机地址电机是否正在运行
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="StateOut">是否运行状态</param> 
        /// <param name="NowSpeed">当前运行速度</param> 
        public static bool IsRun(byte Address, ref bool StateOut, ref ushort NowSpeed)
        {
            if (IsRun(Address, ref StateOut) == false)
            {
                return false;
            }

            if (StateOut == false)
            {
                NowSpeed = 0;
                return true;
            }

            if (GetNowSpeed(Address, ref NowSpeed) == false)
            {
                return false;
            }

            if (NowSpeed == 0)
            {
                StateOut = false;
                NowSpeed = 0;
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 读取驱动器的错误代码
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="ErrCode">使用ref返回电机的错误代码</param> 
        public static bool GetErrCode(byte Address, ref string ErrCode)
        {
            try
            {
                ushort[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadHoldingRegisters(Address, 16385, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    ErrCode = Convert.ToInt32(ReturnData[0].ToString("X4").Substring(0, 2), 16).ToString() + "." + Convert.ToInt32(ReturnData[0].ToString("X4").Substring(2, 2), 16).ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool GteErrState(byte Address, ref bool State, ref string ErrCode)
        {
            if (IsErr(Address, ref State) == false)
            {
                return false;
            }

            if (State == false)
            {
                ErrCode = "00.0";
                return true;
            }

            if (GetErrCode(Address, ref ErrCode) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 读取电机的当前负载率
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="OperatingLoad">使用ref返回电机的负载率</param> 
        public static bool GetOperatingLoad(byte Address, ref int OperatingLoad)
        {
            try
            {
                ushort[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadHoldingRegisters(Address, 16398, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    OperatingLoad = ReturnData[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将指定驱动器的报警状态清除
        /// </summary>
        /// <param name="address">地址0-31</param> 
        public static bool ClearErr(byte Address)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleRegister(Address, 16642, 29300);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取电机编码器的温度
        /// </summary>
        /// <param name="Address">地址0-31</param>
        /// <param name="Temp">使用Ref返回电机编码器的温度</param>
        /// <returns></returns>
        public static bool GetEncoderTemp(byte Address, ref short Temp)
        {
            try
            {
                ushort[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadHoldingRegisters(Address, 16411, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    Temp = Auxiliary.UshortToShort(ReturnData[0]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 每隔500毫秒获取电机运行状态以及报警状态
        /// </summary>
        private static void Get_A6_Value()
        {
            int[] ConnentErrNum = new int[A6_Channel.Length];

            Stopwatch ErrReadTime = new Stopwatch();
            Stopwatch RunReadTime = new Stopwatch();

            ErrReadTime.Start();
            RunReadTime.Start();

            while (true)
            {
                Thread.Sleep(10);

                if (_PauseRead == false && IsOpen == true)
                {
                    #region 读取所有驱动器的运动状态
                    if (RunReadTime.ElapsedMilliseconds >= 100)
                    {
                        for (byte i = 0; i < A6_Channel.Length; i++)
                        {
                            if (_PauseRead == true)
                            {
                                break;
                            }

                            A6_Channel[i].IsConnent = IsRun((byte)(i + 1), ref A6_Channel[i].IsRun, ref A6_Channel[i].NowSpeed);

                            if (A6_Channel[i].IsConnent == false)
                            {
                                A6_Channel[i].ConnentErrNumber++;
                            }

                            if (A6_Channel[i].IsRun == true)
                            {
                                A6_Channel[i].IsConnent = GetOperatingLoad((byte)(i + 1), ref A6_Channel[i].OperatingLoad);

                                if (A6_Channel[i].IsConnent == false)
                                {
                                    A6_Channel[i].ConnentErrNumber++;
                                }
                            }
                            else
                            {
                                A6_Channel[i].NowSpeed = 0;
                                A6_Channel[i].OperatingLoad = 0;
                            }
                        }
                        RunReadTime.Restart();
                    }
                    #endregion

                    #region 读取所有驱动器的报警与温度
                    if (ErrReadTime.ElapsedMilliseconds >= 500)
                    {
                        for (byte i = 0; i < A6_Channel.Length; i++)
                        {
                            if (_PauseRead == true)
                            {
                                break;
                            }

                            A6_Channel[i].IsConnent = GetEncoderTemp((byte)(i + 1), ref A6_Channel[i].EncoderTemp);

                            if (A6_Channel[i].IsConnent == false)
                            {
                                A6_Channel[i].ConnentErrNumber++;
                            }

                            A6_Channel[i].IsConnent = GteErrState((byte)(i + 1), ref A6_Channel[i].IsErr, ref A6_Channel[i].ErrCode);

                            if (A6_Channel[i].IsConnent == false)
                            {
                                A6_Channel[i].ConnentErrNumber++;
                            }

                            if (A6_Channel[i].IsErr == true)
                            {
                                for (int j = 0; j < _A6_ErrCodeList.Length; j++)
                                {
                                    if (_A6_ErrCodeList[j].Code == A6_Channel[i].ErrCode)
                                    {
                                        A6_Channel[i].IsErrClear = _A6_ErrCodeList[j].IsClear;
                                    }
                                }
                            }
                            else
                            {
                                A6_Channel[i].IsErrClear = false;
                                A6_Channel[i].ConnentErrNumber++;
                            }
                        }
                        ErrReadTime.Restart();
                    }
                    #endregion
                }

                #region 更新读取失败次数
                for (int i = 0; i < A6_Channel.Length; i++)
                {
                    A6_Channel[i].ConnentErrNumber = A6_Channel[i].ConnentErrNumber;
                }
                #endregion
            }

        }
        /// <summary>
        /// 调用指定Block
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="blocknum">Block地址</param> 
        private static bool UseBlock(byte Address, ushort BlockNum)
        {
            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(2);
                    PanasonicA6Master.WriteSingleRegister(Address, 17428, BlockNum);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 将指定的Block设定为On-Off
        /// </summary>
        /// <param name="address">地址0-31</param> 
        private static bool A6BlockOn(byte Address, ushort BlockNum)
        {
            if (UseBlock(Address, BlockNum) == false)
            {
                return false;
            }

            try
            {
                lock (LockValue)
                {
                    Thread.Sleep(10);
                    PanasonicA6Master.WriteSingleCoil(Address, 288, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定Block状态On或Off
        /// </summary>
        /// <param name="address">地址0-31</param> 
        /// <param name="OnOff">Block状态</param> 
        private static bool A6Block(byte Address, bool OnOff)
        {
            try
            {
                lock (LockValue)
                {
                    PanasonicA6Master.WriteSingleCoil(Address, 288, OnOff);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Block选通状态，只有在Block选通状态为Off下才可再次执行
        /// </summary>
        /// <param name="address">地址0-31</param> 
        private static bool IsA6BlockOn(byte Address, ref bool StateOut)
        {
            try
            {
                bool[] ReturnData;

                lock (LockValue)
                {
                    Thread.Sleep(2);
                    ReturnData = PanasonicA6Master.ReadCoils(Address, 304, 01);
                }

                if (ReturnData != null && ReturnData.Length != 0)
                {
                    StateOut = ReturnData[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
