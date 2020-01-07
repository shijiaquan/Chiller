using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace DewPointSensor
{
    public class HTU21D
    {
        public struct HTU21DBody
        {
            /// <summary>
            /// 连接站号
            /// </summary>
            public int Station;
            /// <summary>
            /// 是否报警
            /// </summary>
            public bool IsConnect;
            /// <summary>
            /// 温度-2位浮点（最后两位数）
            /// </summary>
            public float Temperature;
            /// <summary>
            /// 湿度-2位浮点（最后两位数）
            /// </summary>
            public float Humidity;
            /// <summary>
            /// 露点-2位浮点（最后两位数）
            /// </summary>
            public float DewPoint;
        }

        private static HTU21DBody[] HTU21D_Channel;
        private static SerialPort SP;
        private static object LockValue = 0;
        private static bool ExitRead;
        private static bool AutoLoad;
        private static Thread GetValue;
        public static HTU21DBody[] NowHTU21D
        {
            get
            {
                return HTU21D_Channel;
            }
        }

        /// <summary>
        /// 初始化驱动器连接
        /// </summary>
        /// <param name="Port">串口号</param> 
        /// <param name="Baud">通讯波特率</param> 
        /// <param name="HTU21D_Channel_Num">连接的指定站号</param> 
        public static bool Initialization(string Port, int Baud, int[] Station_Num)
        {
            try
            {
                HTU21D_Channel = new HTU21DBody[Station_Num.Length];
                for (int i = 0; i < Station_Num.Length; i++)
                {
                    HTU21D_Channel[i].Station = Station_Num[i];
                    HTU21D_Channel[i].IsConnect = false;
                    HTU21D_Channel[i].Temperature = 9999;
                    HTU21D_Channel[i].Humidity = 9999;
                    HTU21D_Channel[i].DewPoint = 9999;
                }

                SP = new SerialPort(Port, Baud, Parity.None, 8, StopBits.One);
                SP.Open();

                if (GetValue == null)
                {
                    GetValue = new Thread(Get_HTU21D_Value) { IsBackground = true };
                    GetValue.Start();
                    ExitRead = false;
                }

                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化驱动器连接
        /// </summary>
        /// <param name="Port">串口号</param> 
        /// <param name="Baud">通讯波特率</param> 
        /// <param name="HTU21D_Channel_Num">连接的指定站号</param> 
        public static bool Initialization(string Port, int Baud)
        {
            try
            {
                SP = new SerialPort(Port, Baud, Parity.None, 8, StopBits.One);
                SP.Open();

                if (GetValue == null)
                {
                    GetValue = new Thread(Get_HTU21D_Value) { IsBackground = true };
                    GetValue.Start();
                    ExitRead = false;
                }
                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 加载站号-自动扫描模式
        /// </summary>
        /// <returns></returns>
        public static void LoadStation()
        {
            AutoLoad = true;
            Thread LoadValue = new Thread(AutoLoadStation) { IsBackground = true };
            LoadValue.Start();
        }

        /// <summary>
        /// 加载站号-手动输入模式
        /// </summary>
        /// <returns></returns>
        public static void LoadStation(int[] Station)
        {
            HTU21D_Channel = new HTU21DBody[Station.Length];
            for (int i = 0; i < Station.Length; i++)
            {
                HTU21D_Channel[i].Station = Station[i];
                HTU21D_Channel[i].IsConnect = false;
                HTU21D_Channel[i].Temperature = 999;
                HTU21D_Channel[i].Humidity = 999;
                HTU21D_Channel[i].DewPoint = 999;
            }
            ExitRead = false;
        }

        /// <summary>
        /// 获取当前连接站号
        /// </summary>
        /// <returns>返回所有连接站号</returns>
        public static int[] GetStation()
        {
            int[] Station = new int[HTU21D_Channel.Length];

            for (int i = 0; i < Station.Length; i++)
            {
                Station[i] = HTU21D_Channel[i].Station;
            }

            return Station;
        }

        /// <summary>
        /// 自动扫描站号
        /// </summary>
        private static void AutoLoadStation()
        {
            ArrayList List = new ArrayList();

            for (int i = 0; i < 16; i++)
            {
                Thread.Sleep(10);
                if (TestConnect(i + 1) == true)
                {
                    List.Add(i + 1);
                }
            }

            if (List.Count > 0)
            {
                HTU21D_Channel = new HTU21DBody[List.Count];
                for (int i = 0; i < List.Count; i++)
                {
                    HTU21D_Channel[i].Station = (int)List[i];
                    HTU21D_Channel[i].IsConnect = false;
                    HTU21D_Channel[i].Temperature = 999;
                    HTU21D_Channel[i].Humidity = 999;
                    HTU21D_Channel[i].DewPoint = 999;
                }
            }
            else
            {
                HTU21D_Channel = null;
            }

            AutoLoad = false;
            ExitRead = false;
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
                    ExitRead = true;
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
        /// 判断连接是否成功
        /// </summary>
        public static bool IsConnect(int Address)
        {
            return HTU21D_Channel[Address - 1].IsConnect;
        }

        /// <summary>
        /// 获取通道连接数量
        /// </summary>
        /// <returns></returns>
        public static int ConnectNumber()
        {
            return HTU21D_Channel.Length;
        }

        /// <summary>
        /// 向串口写入数据
        /// </summary>
        /// <param name="SendData">需发送的数据</param>
        /// <param name="TimeOut">超时</param>
        /// <returns>返回Byte数组报文</returns>
        private static byte[] WriteData(byte[] SendData, int TimeOut)
        {
            try
            {
                lock (LockValue)
                {
                    byte[] GetBuffer;
                    Stopwatch TimeOutDelay = new Stopwatch();

                    if (AutoLoad == true || ExitRead == false)
                    {
                        if (SP.IsOpen == false)
                        {
                            SP.Open();
                        }
                    }
                    else
                    {
                        return null;
                    }

                    if (SP.BytesToRead != 0)
                    {
                        SP.DiscardInBuffer();
                    }

                    SP.Write(SendData, 0, SendData.Length);
                    int ReturnDatByte = ReadDataBit(SendData);
                    if (TimeOut != 0)
                    {
                        TimeOutDelay.Start();
                    }
                    do
                    {
                        Thread.Sleep(1);
                    }
                    while (SP.BytesToRead != ReturnDatByte && TimeOutDelay.ElapsedMilliseconds <= TimeOut);
                    if (TimeOutDelay.ElapsedMilliseconds < TimeOut)
                    {
                        GetBuffer = new byte[SP.BytesToRead];
                        SP.Read(GetBuffer, 0, SP.BytesToRead);
                        if (Auxiliary.CheckCRC(GetBuffer))
                        {
                            return GetBuffer;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取返回数据长，用于判断接收数据的完整性
        /// </summary>
        /// <param name="WriteData">需发送的数据</param>
        /// <returns>成功或失败</returns>
        private static int ReadDataBit(byte[] WriteData)
        {
            int RetBit = 0;
            if (WriteData[1] == 03 || WriteData[1] == 04)
            {
                RetBit = 5 + Convert.ToInt32(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"), 16) * 2;
            }
            else if (WriteData[1] == 06)
            {
                RetBit = 8;
            }
            else if (WriteData[1] == 16)
            {
                RetBit = 8;
            }
            return RetBit;
        }

        /// <summary>
        /// 测试连接可行性
        /// </summary>
        /// <returns></returns>
        private static bool TestConnect(int Address)
        {
            string SendString = Auxiliary.NumToHex(Address, 1) + Auxiliary.NumToHex(4, 1) + Auxiliary.NumToHex(1000, 2) + Auxiliary.NumToHex(1, 2);
            byte[] SendData = Auxiliary.CheckCRC(SendString);
            byte[] ReturnData = WriteData(SendData, 100);
            if (ReturnData == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取环境温度
        /// </summary>
        /// <param name="Address">站号</param>
        /// <param name="Temperature">返回环境温度</param>
        /// <returns>读取成功或失败</returns>
        public static bool GetTemperature(int Address, ref float Temperature)
        {
            string SendString = Auxiliary.NumToHex(Address, 1) + Auxiliary.NumToHex(4, 1) + Auxiliary.NumToHex(1000, 2) + Auxiliary.NumToHex(1, 2);
            byte[] SendData = Auxiliary.CheckCRC(SendString);
            byte[] ReturnData = WriteData(SendData, 100);
            if (ReturnData == null)
            {
                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = false;
                    }
                }

                return false;
            }
            else
            {
                Temperature = Auxiliary.HexToNum(ReturnData[3].ToString("X2") + ReturnData[4].ToString("X2")) / 100f;

                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = true;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 读取环境湿度
        /// </summary>
        /// <param name="Address">站号</param>
        /// <param name="Humidity">返回环境湿度</param>
        /// <returns>读取成功或失败</returns>
        public static bool GetHumidity(int Address, ref float Humidity)
        {
            string SendString = Auxiliary.NumToHex(Address, 1) + Auxiliary.NumToHex(4, 1) + Auxiliary.NumToHex(1001, 2) + Auxiliary.NumToHex(1, 2);
            byte[] SendData = Auxiliary.CheckCRC(SendString);
            byte[] ReturnData = WriteData(SendData, 100);
            if (ReturnData == null)
            {
                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = false;
                    }
                }

                return false;
            }
            else
            {
                Humidity = Auxiliary.HexToNum(ReturnData[3].ToString("X2") + ReturnData[4].ToString("X2")) / 100f;
                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = true;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 读取环境露点
        /// </summary>
        /// <param name="Address">站号</param>
        /// <param name="DewPoint">返回环境露点</param>
        /// <returns>读取成功或失败</returns>
        public static bool GetDewPoint(int Address, ref float DewPoint)
        {
            string SendString = Auxiliary.NumToHex(Address, 1) + Auxiliary.NumToHex(4, 1) + Auxiliary.NumToHex(1002, 2) + Auxiliary.NumToHex(1, 2);
            byte[] SendData = Auxiliary.CheckCRC(SendString);
            byte[] ReturnData = WriteData(SendData, 100);
            if (ReturnData == null)
            {
                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = false;
                    }
                }

                return false;
            }
            else
            {
                DewPoint = Auxiliary.HexToNum(ReturnData[3].ToString("X2") + ReturnData[4].ToString("X2")) / 100f;

                for (int i = 0; i < HTU21D_Channel.Length; i++)
                {
                    if (HTU21D_Channel[i].Station == Address)
                    {
                        HTU21D_Channel[i].IsConnect = true;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 每隔100毫秒获取湿度状态
        /// </summary>
        private static void Get_HTU21D_Value()
        {
            float ReadNum = 0;
            while (true)
            {
                if (HTU21D_Channel != null && ExitRead == false && AutoLoad == false)
                {
                    try
                    {
                        for (int i = 0; i < HTU21D_Channel.Length; i++)
                        {
                            Thread.Sleep(5);

                            if (GetTemperature(HTU21D_Channel[i].Station, ref ReadNum) == true)
                            {
                                HTU21D_Channel[i].IsConnect = true;
                                HTU21D_Channel[i].Temperature = ReadNum;
                            }
                            else
                            {
                                HTU21D_Channel[i].IsConnect = false;
                                HTU21D_Channel[i].Temperature = 999;
                                continue;
                            }

                            if (GetHumidity(HTU21D_Channel[i].Station, ref ReadNum) == true)
                            {
                                HTU21D_Channel[i].IsConnect = true;
                                HTU21D_Channel[i].Humidity = ReadNum;
                            }
                            else
                            {
                                HTU21D_Channel[i].IsConnect = false;
                                HTU21D_Channel[i].Humidity = 999;
                                continue;
                            }

                            if (GetDewPoint(HTU21D_Channel[i].Station, ref ReadNum) == true)
                            {
                                HTU21D_Channel[i].IsConnect = true;
                                HTU21D_Channel[i].DewPoint = ReadNum;
                            }
                            else
                            {
                                HTU21D_Channel[i].IsConnect = false;
                                HTU21D_Channel[i].DewPoint = 999;
                                continue;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

        }
    }

    public class SUTO
    {
        public struct SUTOBody
        {
            /// <summary>
            /// 环境温度
            /// </summary>
            public float Temperature;
            /// <summary>
            /// 相对湿度
            /// </summary>
            public float Humidity;
            /// <summary>
            /// 环境压力
            /// </summary>
            public float Pressure;
            /// <summary>
            /// 压力露点
            /// </summary>
            public float PressureDewPoint;
            /// <summary>
            /// 常压露点
            /// </summary>
            public float DewPoint;
            /// <summary>
            /// 混合比
            /// </summary>
            public float MixingRatio;
            /// <summary>
            /// 绝对湿度
            /// </summary>
            public float AtHumidity;
            /// <summary>
            /// 常压绝对湿度
            /// </summary>
            public float AtAbsoluteHumidity;
            /// <summary>
            /// 体积比
            /// </summary>
            public float VolumeRatio;
        }

        private static SUTOBody[] SUTO_Channel;
        private static SerialPort SP;
        private static object LockValue = 0;
        public static bool _IsConnect = false;

        public SUTOBody[] NowSUTO
        {
            get
            {
                return SUTO_Channel;
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

        public static bool IsConnect
        {
            get { return _IsConnect; }
        }

        public static bool Initialization(string Port, int Baud, int SUTO_Channel_Num)
        {
            try
            {
                SUTO_Channel = new SUTOBody[SUTO_Channel_Num];

                for (int i = 0; i < SUTO_Channel_Num; i++)
                {
                    SUTO_Channel[i].Temperature = 9999;
                    SUTO_Channel[i].Humidity = 9999;
                    SUTO_Channel[i].Pressure = 9999;
                    SUTO_Channel[i].PressureDewPoint = 9999;
                    SUTO_Channel[i].DewPoint = 9999;
                    SUTO_Channel[i].MixingRatio = 9999;
                    SUTO_Channel[i].AtHumidity = 9999;
                    SUTO_Channel[i].AtAbsoluteHumidity = 9999;
                    SUTO_Channel[i].VolumeRatio = 9999;

                }
                Thread GetValue = new Thread(Get_SUTO_Value) { IsBackground = true };
                GetValue.Start();

                SP = new SerialPort(Port, Baud, Parity.None, 8, StopBits.One);
                SP.Open();
                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        private static string WriteData(string SendData, int TimeOut)
        {
            try
            {
                lock (LockValue)
                {
                    string RetData = "";
                    byte[] GetBuffer;
                    Stopwatch TimeOutDelay = new Stopwatch();

                    if (SP.IsOpen == false)
                    {
                        SP.Open();
                    }
                    if (SP.BytesToRead != 0)
                    {
                        SP.DiscardInBuffer();
                    }
                    byte[] WriteData = Auxiliary.CheckCRC(SendData);

                    SP.Write(WriteData, 0, WriteData.Length);
                    int ReturnDatByte = ReadDataBit(WriteData);

                    if (TimeOut != 0)
                    {
                        TimeOutDelay.Start();
                    }
                    do
                    {
                        Thread.Sleep(1);
                    }
                    while (SP.BytesToRead != ReturnDatByte && TimeOutDelay.ElapsedMilliseconds <= TimeOut);

                    if (TimeOutDelay.ElapsedMilliseconds < TimeOut)
                    {
                        GetBuffer = new byte[SP.BytesToRead];
                        SP.Read(GetBuffer, 0, SP.BytesToRead);
                        if (Auxiliary.CheckCRC(GetBuffer))
                        {
                            if (GetBuffer[1] == 04)
                            {
                                for (int i = 0; i < GetBuffer[2] / 2; i += 2)
                                {
                                    RetData += Convert.ToInt32(GetBuffer[3 + i].ToString("X2") + GetBuffer[4 + i].ToString("X2"), 16).ToString() + "-";
                                }
                            }
                        }
                    }
                    else
                    {
                        RetData = null;
                    }
                    return RetData;
                }
            }
            catch
            {
                return null;
            }

        }

        private static byte[] WriteData(byte[] SendData, int TimeOut)
        {
            try
            {
                lock (LockValue)
                {
                    byte[] GetBuffer;
                    Stopwatch TimeOutDelay = new Stopwatch();

                    if (SP.IsOpen == false)
                    {
                        SP.Open();
                    }
                    if (SP.BytesToRead != 0)
                    {
                        SP.DiscardInBuffer();
                    }

                    SP.Write(SendData, 0, SendData.Length);
                    int ReturnDatByte = ReadDataBit(SendData);
                    if (TimeOut != 0)
                    {
                        TimeOutDelay.Start();
                    }
                    do
                    {
                        Thread.Sleep(1);
                    }
                    while (SP.BytesToRead != ReturnDatByte && TimeOutDelay.ElapsedMilliseconds <= TimeOut);
                    if (TimeOutDelay.ElapsedMilliseconds < TimeOut)
                    {
                        GetBuffer = new byte[SP.BytesToRead];
                        SP.Read(GetBuffer, 0, SP.BytesToRead);
                        if (Auxiliary.CheckCRC(GetBuffer))
                        {
                            return GetBuffer;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private static int ReadDataBit(byte[] WriteData)
        {
            int RetBit = 0;
            if (WriteData[1] == 03 || WriteData[1] == 04)
            {
                RetBit = 5 + Convert.ToInt32(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"), 16) * 2;
            }
            else if (WriteData[1] == 06)
            {
                RetBit = 8;
            }
            else if (WriteData[1] == 16)
            {
                RetBit = 8;
            }
            return RetBit;
        }

        private static bool Get(int Address, ref SUTOBody[] Body)
        {
            string SendString = Auxiliary.NumToHex(Address, 1) + Auxiliary.NumToHex(3, 1) + Auxiliary.NumToHex(0, 2) + Auxiliary.NumToHex(18, 2);
            byte[] SendData = Auxiliary.CheckCRC(SendString);
            byte[] ReturnData = WriteData(SendData, 250);
            if (ReturnData == null)
            {
                _IsConnect = false;
                return false;
            }
            Body[Address - 1].Temperature = Auxiliary.HexToFloat(ReturnData[5].ToString("X2") + ReturnData[6].ToString("X2") + ReturnData[3].ToString("X2") + ReturnData[4].ToString("X2"));
            Body[Address - 1].Humidity = Auxiliary.HexToFloat(ReturnData[9].ToString("X2") + ReturnData[10].ToString("X2") + ReturnData[7].ToString("X2") + ReturnData[8].ToString("X2"));
            Body[Address - 1].Pressure = Auxiliary.HexToFloat(ReturnData[13].ToString("X2") + ReturnData[14].ToString("X2") + ReturnData[11].ToString("X2") + ReturnData[12].ToString("X2"));
            Body[Address - 1].PressureDewPoint = Auxiliary.HexToFloat(ReturnData[17].ToString("X2") + ReturnData[18].ToString("X2") + ReturnData[15].ToString("X2") + ReturnData[16].ToString("X2"));
            Body[Address - 1].DewPoint = Auxiliary.HexToFloat(ReturnData[21].ToString("X2") + ReturnData[22].ToString("X2") + ReturnData[19].ToString("X2") + ReturnData[20].ToString("X2"));
            Body[Address - 1].MixingRatio = Auxiliary.HexToFloat(ReturnData[25].ToString("X2") + ReturnData[26].ToString("X2") + ReturnData[23].ToString("X2") + ReturnData[24].ToString("X2"));
            Body[Address - 1].AtHumidity = Auxiliary.HexToFloat(ReturnData[29].ToString("X2") + ReturnData[30].ToString("X2") + ReturnData[27].ToString("X2") + ReturnData[28].ToString("X2"));
            Body[Address - 1].AtAbsoluteHumidity = Auxiliary.HexToFloat(ReturnData[33].ToString("X2") + ReturnData[34].ToString("X2") + ReturnData[31].ToString("X2") + ReturnData[32].ToString("X2"));
            Body[Address - 1].VolumeRatio = Auxiliary.HexToFloat(ReturnData[37].ToString("X2") + ReturnData[38].ToString("X2") + ReturnData[35].ToString("X2") + ReturnData[36].ToString("X2"));

            _IsConnect = true;
            return true;
        }

        /// <summary>
        /// 每隔100毫秒获取电机运行状态以及报警状态
        /// </summary>
        private static void Get_SUTO_Value()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (IsOpen == true)
                {
                    for (int i = 0; i < SUTO_Channel.Length; i++)
                    {
                        Get(i + 1, ref SUTO_Channel);
                    }
                }
            }

        }
    }

    public class Auxiliary
    {
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="Number">需要转换的数</param>
        /// <param name="Lenght">转换成的字节数</param>
        /// <returns></returns>
        public static string NumToHex(int Number, int Lenght)
        {
            int iNumber = Number;
            string strResult = string.Empty;
            if (iNumber >= 0)
            {
                strResult = iNumber.ToString("X" + (Lenght * 2).ToString());
            }
            else if (iNumber < 0)
            {

                strResult = (65536 - Math.Abs(iNumber)).ToString("X" + (Lenght * 2).ToString());
            }
            return strResult;
        }
        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="Number">需要转换的数</param>
        /// <param name="Lenght">转换成的字节数</param>
        /// <returns></returns>
        public static string NumToHex(string Number, int Lenght)
        {
            int iNumber = int.Parse(Number);
            string strResult = string.Empty;
            if (iNumber >= 0)
            {
                strResult = iNumber.ToString("X" + (Lenght * 2).ToString());
            }
            else if (iNumber < 0)
            {

                strResult = (65536 - Math.Abs(iNumber)).ToString("X" + (Lenght * 2).ToString());
            }
            return strResult;
        }
        /// <summary>
        /// 十六进制转十进制，仅支持十六位
        /// </summary>
        /// <param name="Number">需要转换的字符串</param>
        /// <returns></returns>
        public static int HexToNum(string Number)
        {
            long iNumber = Convert.ToInt64(Number, 16);
            if (iNumber <= 65536)
            {
                if (iNumber <= 32767)
                {
                    return (int)iNumber;
                }
                else
                {
                    return (int)(iNumber - 65536);
                }
            }
            else if (iNumber <= 4294967295)
            {
                if (iNumber <= 2147483647)
                {
                    return (int)iNumber;
                }
                else
                {
                    return (int)(iNumber - 4294967295);
                }
            }
            else
            {
                return 0;
            }
        }
        ///<summary>
        /// 将ASCII格式十六进制字符串转浮点数（符合IEEE-754标准（32））
        /// </summary>
        /// <paramname="data">十六进制字符串</param>
        /// <returns>浮点数值</returns>
        public static float HexToFloat(string data)
        {
            byte[] intBuffer = new byte[4];
            // 将16进制串按字节逆序化（一个字节2个ASCII码）
            for (int i = 0; i < 4; i++)
            {
                intBuffer[i] = Convert.ToByte(data.Substring((3 - i) * 2, 2), 16);
            }
            return BitConverter.ToSingle(intBuffer, 0);
        }
        /// <summary>
        /// CRC数据校验，返回Byte数组
        /// </summary>
        /// <param name="SendData">输入字符串</param>
        /// <returns></returns>
        public static byte[] CheckCRC(string SendData)
        {
            UInt16 rCRC;
            rCRC = 0xFFFF;

            int iLength = SendData.Length / 2;
            byte[] btVal = new byte[SendData.Length / 2 + 2];

            for (int i = 0; i < SendData.Length; i += 2)
            {
                btVal[i / 2] = Convert.ToByte(SendData.Substring(i, 2), 16);
            }

            for (int n = 0; n < iLength; n++)
            {
                rCRC = (UInt16)(rCRC ^ btVal[n]);
                for (int i = 0; i < 8; i++)
                {

                    if ((rCRC & 0x0001) != 0)
                    {
                        rCRC >>= 1;
                        rCRC ^= 0xA001;
                    }
                    else
                    {
                        rCRC >>= 1;
                    }
                }
            }
            byte crcH = (byte)((rCRC & 0xFF00) >> 8);
            byte crcL = (byte)(rCRC & 0x00FF);

            btVal[SendData.Length / 2] = crcL;
            btVal[SendData.Length / 2 + 1] = crcH;

            return btVal;
        }
        /// <summary>
        /// 判断一串Byte数组的CRC校验是否正确
        /// </summary>
        /// <param name="btVal">Byte数组</param>
        /// <returns></returns>
        public static bool CheckCRC(byte[] btVal)
        {
            try
            {
                if (btVal == null)
                {
                    return false;
                }

                byte targetcrcH, crcH, targetcrcL, crcL;
                targetcrcH = btVal[btVal.Length - 1];
                targetcrcL = btVal[btVal.Length - 2];

                Array.Resize(ref btVal, btVal.Length - 2);

                UInt16 rCRC;
                rCRC = 0xFFFF;
                for (int n = 0; n < btVal.Length; n++)
                {
                    rCRC = (UInt16)(rCRC ^ btVal[n]);
                    for (int i = 0; i < 8; i++)
                    {
                        if ((rCRC & 0x0001) != 0)
                        {
                            rCRC >>= 1;
                            rCRC ^= 0xA001;
                        }
                        else
                        {
                            rCRC >>= 1;
                        }
                    }
                }
                crcH = (byte)((rCRC & 0xFF00) >> 8);
                crcL = (byte)(rCRC & 0x00FF);

                if (crcH == targetcrcH && crcL == targetcrcL)
                {
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
