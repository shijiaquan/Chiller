using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chiller
{
    class Adam
    {
        /// <summary>
        /// Adam模块所拥有的类型
        /// </summary>
        public enum HaveAdamType
        {
            /// <summary>
            /// 研华Adam6017远程模块，包含8组电压/电流模拟量输入、2组数字量输出，单以太网口
            /// </summary>
            AdamModule6017 = 1,
            /// <summary>
            /// 研华Adam6018远程模块，包含8组热电偶输入、8组数字量输出，单以太网口
            /// </summary>
            AdamModule6018 = 2,
            /// <summary>
            /// 研华Adam6250远程模块，包含8组数字量输入、7组数字量输出，双以太网口支持菊花链模式
            /// </summary>
            AdamModule6250 = 3,
            /// <summary>
            /// 研华Adam6256远程模块，包含16组数字量输出，双以太网口支持菊花链模式
            /// </summary>
            AdamModule6256 = 4,
        }

        public struct AdamModule6017
        {
            /// <summary>
            /// 模块通讯链路读取通道
            /// </summary>
            public AdamSocket ComR;
            /// <summary>
            /// 模块通讯链路写入通道
            /// </summary>
            public AdamSocket ComT;
            /// <summary>
            /// 输入范围
            /// </summary>
            public byte[] Range;
            /// <summary>
            /// 实时模拟量输入值
            /// </summary>
            public float[] AI;
            /// <summary>
            /// 实时数字量输出值
            /// </summary>
            public int[] DO;
            /// <summary>
            /// 模块读取链路状态
            /// </summary>
            public bool IsComR;
            /// <summary>
            /// 模块写入链路状态
            /// </summary>
            public bool IsComT;
            /// <summary>
            /// 是否停止读取用的后台线程
            /// </summary>
            public bool Stop;
            /// <summary>
            /// 读取用的后台线程
            /// </summary>
            public Thread ThreadRead;
            /// <summary>
            /// 通讯周期时间ms
            /// </summary>
            public int Time_Difference;
            /// <summary>
            /// 正在读取数据中
            /// </summary>
            public bool IsReading;
        }

        public struct AdamModule6018
        {
            /// <summary>
            /// 模块通讯链路读取通道
            /// </summary>
            public AdamSocket ComR;
            /// <summary>
            /// 模块通讯链路写入通道
            /// </summary>
            public AdamSocket ComT;
            /// <summary>
            /// 输入范围
            /// </summary>
            public byte[] Range;
            /// <summary>
            /// 实时温度输入值
            /// </summary>
            public float[] Temp;
            /// <summary>
            /// 实时数字量输出值
            /// </summary>
            public int[] DO;
            /// <summary>
            /// 模块读取链路状态
            /// </summary>
            public bool IsComR;
            /// <summary>
            /// 模块写入链路状态
            /// </summary>
            public bool IsComT;
            /// <summary>
            /// 是否停止读取用的后台线程
            /// </summary>
            public bool Stop;
            /// <summary>
            /// 读取用的后台线程
            /// </summary>
            public Thread ThreadRead;
            /// <summary>
            /// 通讯周期时间ms
            /// </summary>
            public int Time_Difference;
            /// <summary>
            /// 正在读取数据中
            /// </summary>
            public bool IsReading;
        }

        public struct AdamModule6250
        {
            /// <summary>
            /// 6250模块通讯链路读取通道
            /// </summary>
            public AdamSocket ComR;
            /// <summary>
            /// 6250模块通讯链路写入通道
            /// </summary>
            public AdamSocket ComT;
            /// <summary>
            /// 实时数字量输入值
            /// </summary>
            public int[] DI;
            /// <summary>
            /// 实时数字量输出值
            /// </summary>
            public int[] DO;
            /// <summary>
            /// 模块读取链路状态
            /// </summary>
            public bool IsComR;
            /// <summary>
            /// 模块写入链路状态
            /// </summary>
            public bool IsComT;
            /// <summary>
            /// 是否停止读取用的后台线程
            /// </summary>
            public bool Stop;
            /// <summary>
            /// 读取用的后台线程
            /// </summary>
            public Thread ThreadRead;
            /// <summary>
            /// 通讯周期时间ms
            /// </summary>
            public int Time_Difference;
            /// <summary>
            /// 正在读取数据中
            /// </summary>
            public bool IsReading;
        }

        public struct AdamModule6256
        {
            /// <summary>
            /// 6256模块通讯链路读取通道
            /// </summary>
            public AdamSocket ComR;
            /// <summary>
            /// 6256模块通讯链路写入通道
            /// </summary>
            public AdamSocket ComT;
            /// <summary>
            /// 实时数字量输出值
            /// </summary>
            public int[] DO;
            /// <summary>
            /// 模块读取链路状态
            /// </summary>
            public bool IsComR;
            /// <summary>
            /// 模块写入链路状态
            /// </summary>
            public bool IsComT;
            /// <summary>
            /// 是否停止读取用的后台线程
            /// </summary>
            public bool Stop;
            /// <summary>
            /// 读取用的后台线程
            /// </summary>
            public Thread ThreadRead;
            /// <summary>
            /// 通讯周期时间ms
            /// </summary>
            public int Time_Difference;
            /// <summary>
            /// 正在读取数据中
            /// </summary>
            public bool IsReading;
        }

        public static AdamModule6017[] Adam6017;     //模块参数

        public static AdamModule6018[] Adam6018;     //模块参数

        public static AdamModule6250[] Adam6250;     //模块参数

        public static AdamModule6256[] Adam6256;     //模块参数

        /// <summary>
        /// 获取研华模块序号所对应的类型
        /// </summary>
        /// <param name="Index">研华模型系统里对应的序号</param>
        /// <param name="IndexOut">返回模块命名的序号</param>
        /// <returns>返回当前存在的模块类型</returns>
        public static HaveAdamType GetAdamType(int Index ,ref int IndexOut)
        {
            HaveAdamType AdamT = new HaveAdamType();
            switch (Index)
            {
                case 0:
                    AdamT = HaveAdamType.AdamModule6017;
                    IndexOut = 0;
                    break;
                case 1:
                    AdamT = HaveAdamType.AdamModule6017;
                    IndexOut = 1;
                    break;
                case 2:
                    AdamT = HaveAdamType.AdamModule6017;
                    IndexOut = 2;
                    break;
                case 3:
                    AdamT = HaveAdamType.AdamModule6017;
                    IndexOut = 3;
                    break;
                case 4:
                    AdamT = HaveAdamType.AdamModule6018;
                    IndexOut = 0;
                    break;
                case 5:
                    AdamT = HaveAdamType.AdamModule6018;
                    IndexOut = 1;
                    break;
                case 6:
                    AdamT = HaveAdamType.AdamModule6017;
                    IndexOut = 4;
                    break;
                case 7:
                    AdamT = HaveAdamType.AdamModule6250;
                    IndexOut = 0;
                    break;
                case 8:
                    AdamT = HaveAdamType.AdamModule6256;
                    IndexOut = 0;
                    break;
            }
            return AdamT;
        }

        /// <summary>
        /// 获取所对应研华模块的连接状态
        /// </summary>
        /// <param name="Index">研华模型系统里对应的序号</param>
        /// <returns>连接或未连接</returns>
        public static bool IsConnect(int Index)
        {
            int Id_Num = -1;
            bool IsConnect = false;
            switch (Adam.GetAdamType(Index, ref Id_Num))
            {
                case Adam.HaveAdamType.AdamModule6017:

                    if (Adam6017 != null)
                    {
                        IsConnect = Adam6017[Id_Num].IsComR && Adam6017[Id_Num].IsComT;
                    }
                    else
                    {
                        IsConnect = false;
                    }

                    break;

                case Adam.HaveAdamType.AdamModule6018:

                    if (Adam6018 != null)
                    {
                        IsConnect = Adam6018[Id_Num].IsComR && Adam6018[Id_Num].IsComT;
                    }
                    else
                    {
                        IsConnect = false;
                    }
                    break;

                case Adam.HaveAdamType.AdamModule6250:

                    if (Adam6250 != null)
                    {
                        IsConnect = Adam6250[Id_Num].IsComR && Adam6250[Id_Num].IsComT;
                    }
                    else
                    {
                        IsConnect = false;
                    }

                    break;

                case Adam.HaveAdamType.AdamModule6256:

                    if (Adam6256 != null)
                    {
                        IsConnect = Adam6256[Id_Num].IsComR && Adam6256[Id_Num].IsComT;
                    }
                    else
                    {
                        IsConnect = false;
                    }

                    break;

                default:

                    IsConnect = false;

                    break;
            }

            return IsConnect;
        }

        /// <summary>
        /// 获取所对应研华模块的连接状态
        /// </summary>
        /// <param name="Index">研华模型系统里对应的序号</param>
        /// <returns>连接或未连接</returns>
        public static bool IsConnect(string Address)
        {
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            return IsConnect(Station);
        }

        /// <summary>
        /// 初始化Adam远程模块
        /// </summary>
        /// <param name="Index">模块地址</param>
        /// <param name="IP">模块IP</param>
        /// <returns></returns>
        public static bool InitializationAdamModule(int Index, string IP)
        {
            bool Ret = false;

            switch (GetAdamType(Index, ref Index))
            {
                case HaveAdamType.AdamModule6017:

                    Adam6017[Index].Range = new byte[8];
                    Adam6017[Index].AI = new float[8];
                    Adam6017[Index].DO = new int[2];
                    Adam6017[Index].ComR = new AdamSocket();
                    Adam6017[Index].ComR.SetTimeout(500, 500, 500);
                    Adam6017[Index].ComT = new AdamSocket();
                    Adam6017[Index].ComT.SetTimeout(500, 500, 500);
                    Adam6017[Index].IsComR = Adam6017[Index].ComR.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);
                    Adam6017[Index].IsComT = Adam6017[Index].ComT.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);

                    if (Adam6017[Index].IsComR == true && Adam6017[Index].IsComT == true)
                    {
                        Adam6017[Index].Stop = false;
                        Adam6017[Index] = Get_Range(Adam6017[Index]);

                        if (Adam6017[Index].IsReading == false)
                        {
                            Adam6017[Index].ThreadRead = new Thread(new ParameterizedThreadStart(Read_Value_6017));
                            Adam6017[Index].ThreadRead.IsBackground = true;
                            Adam6017[Index].ThreadRead.Start(Index);
                        }
                        Ret = true;
                    }
                    else
                    {
                        if (Adam6017[Index].IsComR == true)
                        {
                            Adam6017[Index].ComR.Disconnect();
                            Adam6017[Index].IsComR = false;
                        }
                        if (Adam6017[Index].IsComT == true)
                        {
                            Adam6017[Index].ComR.Disconnect();
                            Adam6017[Index].IsComT = false;
                        }
                        Ret = false;
                    }

                    break;

                case HaveAdamType.AdamModule6018:

                    Adam6018[Index].Range = new byte[8];
                    Adam6018[Index].Temp = new float[8];
                    Adam6018[Index].DO = new int[8];
                    Adam6018[Index].ComR = new AdamSocket();
                    Adam6018[Index].ComR.SetTimeout(100, 100, 100);
                    Adam6018[Index].ComT = new AdamSocket();
                    Adam6018[Index].ComT.SetTimeout(100, 100, 100);

                    Adam6018[Index].IsComR = Adam6018[Index].ComR.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);
                    Adam6018[Index].IsComT = Adam6018[Index].ComT.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);

                    if (Adam6018[Index].IsComR == true && Adam6018[Index].IsComT == true)
                    {

                        Adam6018[Index].Stop = false;
                        Adam6018[Index] = Get_Range(Adam6018[Index]);
                        if (Adam6018[Index].IsReading == false)
                        {
                            Adam6018[Index].ThreadRead = new Thread(new ParameterizedThreadStart(Read_Value_6018));
                            Adam6018[Index].ThreadRead.IsBackground = true;
                            Adam6018[Index].ThreadRead.Start(Index);
                        }
                        Ret = true;
                    }
                    else
                    {
                        if (Adam6018[Index].IsComR == true)
                        {
                            Adam6018[Index].ComR.Disconnect();
                            Adam6018[Index].IsComR = false;
                        }
                        if (Adam6018[Index].IsComT == true)
                        {
                            Adam6018[Index].ComR.Disconnect();
                            Adam6018[Index].IsComT = false;
                        }
                        Ret = false;
                    }

                    break;

                case HaveAdamType.AdamModule6250:

                    Adam6250[Index].DI = new int[8];
                    Adam6250[Index].DO = new int[7];
                    Adam6250[Index].ComR = new AdamSocket();
                    Adam6250[Index].ComR.SetTimeout(100, 100, 100);
                    Adam6250[Index].ComT = new AdamSocket();
                    Adam6250[Index].ComT.SetTimeout(100, 100, 100);

                    Adam6250[Index].IsComR = Adam6250[Index].ComR.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);
                    Adam6250[Index].IsComT = Adam6250[Index].ComT.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);

                    if (Adam6250[Index].IsComR == true && Adam6250[Index].IsComT == true)
                    {
                        Adam6250[Index].Stop = false;
                        if (Adam6250[Index].IsReading == false)
                        {
                            Adam6250[Index].ThreadRead = new Thread(new ParameterizedThreadStart(Read_Value_6250));
                            Adam6250[Index].ThreadRead.IsBackground = true;
                            Adam6250[Index].ThreadRead.Start(Index);
                        }
                        Ret = true;
                    }
                    else
                    {
                        if (Adam6250[Index].IsComR == true)
                        {
                            Adam6250[Index].ComR.Disconnect();
                            Adam6250[Index].IsComR = false;
                        }
                        if (Adam6250[Index].IsComT == true)
                        {
                            Adam6250[Index].ComR.Disconnect();
                            Adam6250[Index].IsComT = false;
                        }
                        Ret = false;
                    }

                    break;

                case HaveAdamType.AdamModule6256:

                    Adam6256[Index].DO = new int[16];
                    Adam6256[Index].ComR = new AdamSocket();
                    Adam6256[Index].ComR.SetTimeout(100, 100, 100);
                    Adam6256[Index].ComT = new AdamSocket();
                    Adam6256[Index].ComT.SetTimeout(100, 100, 100);

                    Adam6256[Index].IsComR = Adam6256[Index].ComR.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);
                    Adam6256[Index].IsComT = Adam6256[Index].ComT.Connect(AdamType.Adam6000, IP, ProtocolType.Tcp);

                    if (Adam6256[Index].IsComR == true && Adam6256[Index].IsComT == true)
                    {
                        Adam6256[Index].Stop = false;
                        if (Adam6256[Index].IsReading == false)
                        {
                            Adam6256[Index].ThreadRead = new Thread(new ParameterizedThreadStart(Read_Value_6256));
                            Adam6256[Index].ThreadRead.IsBackground = true;
                            Adam6256[Index].ThreadRead.Start(Index);
                        }
                        Ret = true;
                    }
                    else
                    {
                        if (Adam6256[Index].IsComR == true)
                        {
                            Adam6256[Index].ComR.Disconnect();
                            Adam6256[Index].IsComR = false;
                        }
                        if (Adam6256[Index].IsComT == true)
                        {
                            Adam6256[Index].ComR.Disconnect();
                            Adam6256[Index].IsComT = false;
                        }
                        Ret = false;
                    }

                    break;
            }

            return Ret;
        }

        public static void Read_Value_6017(object l)
        {
            int Index = (int)l;
            Stopwatch time = new Stopwatch();
            time.Start();
            Adam6017[Index].ComR.SetTimeout(5000, 500, 500);
            Adam6017[Index].ComT.SetTimeout(5000, 500, 500);
            while (Adam6017[Index].Stop == false)
            {
                Thread.Sleep(1);

                Adam6017[Index].IsReading = true;

                if (Adam6017[Index].ComR != null)
                {
                    Adam6017[Index].IsComR = Adam6017[Index].ComR.Connected;
                    if (Adam6017[Index].IsComR == false)
                    {
                        Adam6017[Index].ComR.Disconnect();
                        Reconnect(Adam6017[Index].ComR);
                    }
                }
                else
                {
                    Adam6017[Index].IsComR = false;
                }

                if (Adam6017[Index].ComT != null)
                {
                    Adam6017[Index].IsComT = Adam6017[Index].ComT.Connected;
                    if (Adam6017[Index].IsComT == false)
                    {
                        Adam6017[Index].ComT.Disconnect();
                        Reconnect(Adam6017[Index].ComT);
                    }
                }
                else
                {
                    Adam6017[Index].IsComT = false;
                }

                if (Adam6017[Index].IsComR && Adam6017[Index].IsComT)
                {
                    Adam6017[Index] = Get_Value_6017(Adam6017[Index]);
                    Adam6017[Index].Time_Difference = (int)time.ElapsedMilliseconds;
                    time.Restart();
                }
            }

            Adam6017[Index].IsReading = false;

            if (Adam6017[Index].IsComR == true)
            {
                Adam6017[Index].ComR.Disconnect();
                Adam6017[Index].IsComR = false;
            }
            if (Adam6017[Index].IsComT == true)
            {
                Adam6017[Index].ComR.Disconnect();
                Adam6017[Index].IsComT = false;
            }
        }

        public static void Read_Value_6018(object l)
        {
            int Index = (int)l;
            Stopwatch time = new Stopwatch();
            time.Start();
            Adam6018[Index].ComR.SetTimeout(5000, 500, 500);
            Adam6018[Index].ComT.SetTimeout(5000, 500, 500);
            while (Adam6018[Index].Stop == false)
            {
                Thread.Sleep(1);

                Adam6018[Index].IsReading = true;

                if (Adam6018[Index].ComR != null)
                {
                    Adam6018[Index].IsComR = Adam6018[Index].ComR.Connected;
                    if (Adam6018[0].IsComR == false)
                    {
                        Adam6018[Index].ComR.Disconnect();
                        Reconnect(Adam6018[Index].ComR);
                    }
                }
                else
                {
                    Adam6018[Index].IsComR = false;
                }

                if (Adam6018[Index].ComT != null)
                {
                    Adam6018[Index].IsComT = Adam6018[Index].ComT.Connected;
                    if (Adam6018[0].IsComT == false)
                    {
                        Adam6018[Index].ComT.Disconnect();
                        Reconnect(Adam6018[Index].ComT);
                    }
                }
                else
                {
                    Adam6018[Index].IsComT = false;
                }

                if (Adam6018[Index].IsComR && Adam6018[Index].IsComT)
                {
                    Adam6018[Index] = Get_Value_6018(Adam6018[Index]);
                    Adam6018[Index].Time_Difference = (int)time.ElapsedMilliseconds;
                    time.Reset();
                }
            }

            Adam6018[Index].IsReading = false;

            if (Adam6018[Index].IsComR == true)
            {
                Adam6018[Index].ComR.Disconnect();
                Adam6018[Index].IsComR = false;
            }
            if (Adam6018[Index].IsComT == true)
            {
                Adam6018[Index].ComR.Disconnect();
                Adam6018[Index].IsComT = false;
            }
        }

        public static void Read_Value_6250(object l)
        {
            int Index = (int)l;
            Stopwatch time = new Stopwatch();
            time.Start();
            Adam6250[Index].ComR.SetTimeout(5000, 500, 500);
            Adam6250[Index].ComT.SetTimeout(5000, 500, 500);
            while (Adam6250[Index].Stop == false)
            {
                Thread.Sleep(1);

                Adam6250[Index].IsReading = true;

                if (Adam6250[Index].ComR != null)
                {
                    Adam6250[Index].IsComR = Adam6250[Index].ComR.Connected;
                    if (Adam6250[0].IsComR == false)
                    {
                        Adam6250[Index].ComR.Disconnect();
                        Reconnect(Adam6250[Index].ComR);
                    }
                }
                else
                {
                    Adam6250[Index].IsComR = false;
                }

                if (Adam6250[Index].ComT != null)
                {
                    Adam6250[Index].IsComT = Adam6250[Index].ComT.Connected;
                    if (Adam6250[0].IsComT == false)
                    {
                        Adam6250[Index].ComT.Disconnect();
                        Reconnect(Adam6250[Index].ComT);
                    }
                }
                else
                {
                    Adam6250[Index].IsComT = false;
                }

                if (Adam6250[Index].IsComR && Adam6250[Index].IsComT)
                {
                    Adam6250[Index] = Get_Value_6250(Adam6250[Index]);
                    Adam6250[Index].Time_Difference = (int)time.ElapsedMilliseconds;
                    time.Reset();
                }
            }

            Adam6250[Index].IsReading = false;

            if (Adam6250[Index].IsComR == true)
            {
                Adam6250[Index].ComR.Disconnect();
                Adam6250[Index].IsComR = false;
            }
            if (Adam6250[Index].IsComT == true)
            {
                Adam6250[Index].ComR.Disconnect();
                Adam6250[Index].IsComT = false;
            }
        }

        public static void Read_Value_6256(object l)
        {
            int Index = (int)l;
            Stopwatch time = new Stopwatch();
            time.Start();
            Adam6256[Index].ComR.SetTimeout(5000, 500, 500);
            Adam6256[Index].ComT.SetTimeout(5000, 500, 500);
            while (Adam6256[Index].Stop == false)
            {
                Thread.Sleep(1);

                Adam6256[Index].IsReading = true;

                if (Adam6256[Index].ComR != null)
                {
                    Adam6256[Index].IsComR = Adam6256[Index].ComR.Connected;
                    if (Adam6256[0].IsComR == false)
                    {
                        Adam6256[Index].ComR.Disconnect();
                        Reconnect(Adam6256[Index].ComR);
                    }
                }
                else
                {
                    Adam6256[Index].IsComR = false;
                }

                if (Adam6256[Index].ComT != null)
                {
                    Adam6256[Index].IsComT = Adam6256[Index].ComT.Connected;
                    if (Adam6256[0].IsComT == false)
                    {
                        Adam6256[Index].ComT.Disconnect();
                        Reconnect(Adam6256[Index].ComT);
                    }
                }
                else
                {
                    Adam6256[Index].IsComT = false;
                }
                if (Adam6256[Index].IsComR && Adam6256[Index].IsComT)
                {
                    Adam6256[Index] = Get_Value_6256(Adam6256[Index]);
                    Adam6256[Index].Time_Difference = (int)time.ElapsedMilliseconds;
                    time.Reset();
                }
            }

            Adam6256[Index].IsReading = false;

            if (Adam6256[Index].IsComR == true)
            {
                Adam6256[Index].ComR.Disconnect();
                Adam6256[Index].IsComR = false;
            }
            if (Adam6256[Index].IsComT == true)
            {
                Adam6256[Index].ComR.Disconnect();
                Adam6256[Index].IsComT = false;
            }
        }

        public static void Close()
        {
            if (Adam6017 != null)
            {
                for (int i = 0; i < Adam6017.Length; i++)
                {
                    Adam6017[i].Stop = true;
                    if (Adam6017[i].ComR != null)
                    {
                        Adam6017[i].ComR.Disconnect();
                    }
                    if (Adam6017[i].ComT != null)
                    {
                        Adam6017[i].ComT.Disconnect();
                    }
                }
            }
            if (Adam6018 != null)
            {
                for (int i = 0; i < Adam6018.Length; i++)
                {
                    Adam6018[i].Stop = true;
                    if (Adam6018[i].ComR != null)
                    {
                        Adam6018[i].ComR.Disconnect();
                    }
                    if (Adam6018[i].ComT != null)
                    {
                        Adam6018[i].ComT.Disconnect();
                    }
                }
            }
            if (Adam6250 != null)
            {
                for (int i = 0; i < Adam6250.Length; i++)
                {
                    Adam6250[i].Stop = true;
                    if (Adam6250[i].ComR != null)
                    {
                        Adam6250[i].ComR.Disconnect();
                    }
                    if (Adam6250[i].ComT != null)
                    {
                        Adam6250[i].ComT.Disconnect();
                    }
                }
            }
            if (Adam6256 != null)
            {
                for (int i = 0; i < Adam6256.Length; i++)
                {
                    Adam6256[i].Stop = true;
                    if (Adam6256[i].ComR != null)
                    {
                        Adam6256[i].ComR.Disconnect();
                    }
                    if (Adam6256[i].ComT != null)
                    {
                        Adam6256[i].ComT.Disconnect();
                    }
                }
            }
        }

        /// <summary>
        /// 刷新Adam-6017模块的模拟量输入与数字量输出的状态
        /// </summary>
        public static AdamModule6017 Get_Value_6017(AdamModule6017 Adam)
        {
            if (Adam.ComR != null && Adam.IsComR != false)
            {
                int[] iData;
                bool[] bData;

                Adam = Get_Range(Adam);

                if (Adam.ComR.Modbus().ReadInputRegs(1, Adam.AI.Length, out iData))
                {
                    for (int i = 0; i < Adam.AI.Length; i++)
                    {
                        Adam.AI[i] = AnalogInput.GetScaledValue(Adam6000Type.Adam6017, Adam.Range[i], iData[i]);
                    }
                }

                if (Adam.ComT.Modbus().ReadCoilStatus(17, Adam.DO.Length, out bData))
                {
                    for (int i = 0; i < Adam.DO.Length; i++)
                    {
                        Adam.DO[i] = Convert.ToInt32(bData[i]);
                    }
                }
            }
            return Adam;
        }
        /// <summary>
        /// 刷新Adam-6018模块的温度和数字量输出状态
        /// </summary>
        public static AdamModule6018 Get_Value_6018(AdamModule6018 Adam)
        {
            if (Adam.ComR != null && Adam.IsComR != false)
            {
                int[] iData;
                bool[] bData;

                Adam = Get_Range(Adam);

                if (Adam.ComR.Modbus().ReadInputRegs(1, Adam.Temp.Length, out iData))
                {
                    for (int i = 0; i < Adam.Temp.Length; i++)
                    {
                        Adam.Temp[i] = AnalogInput.GetScaledValue(Adam6000Type.Adam6018, Adam.Range[i], iData[i]);      //转化为对应的温度
                    }
                }
                if (Adam.ComT.Modbus().ReadCoilStatus(17, Adam.DO.Length, out bData))
                {
                    for (int i = 0; i < Adam.DO.Length; i++)
                    {
                        Adam.DO[i] = Convert.ToInt32(bData[i]);
                    }
                }
            }
            return Adam;
        }
        /// <summary>
        /// 刷新Adam-6250模块的数字量输入和数字量输出状态
        /// </summary>
        public static AdamModule6250 Get_Value_6250(AdamModule6250 Adam)
        {
            if (Adam.ComR != null && Adam.IsComR != false)
            {
                bool[] iData;
                bool[] bData;
                if (Adam.ComR.Modbus().ReadCoilStatus(1, Adam.DI.Length, out iData))
                {
                    for (int i = 0; i < Adam.DI.Length; i++)
                    {
                        Adam.DI[i] = Convert.ToInt32(iData[i]);
                    }
                }
                if (Adam.ComT.Modbus().ReadCoilStatus(17, Adam.DO.Length, out bData))
                {
                    for (int i = 0; i < Adam.DO.Length; i++)
                    {
                        Adam.DO[i] = Convert.ToInt32(bData[i]);
                    }
                }
            }
            return Adam;
        }
        /// <summary>
        /// 刷新Adam-6256模块的数字量输出状态
        /// </summary>
        public static AdamModule6256 Get_Value_6256(AdamModule6256 Adam)
        {
            if (Adam.ComR != null && Adam.IsComR != false)
            {
                bool[] bData;
                if (Adam.ComT.Modbus().ReadCoilStatus(17, Adam.DO.Length, out bData))
                {
                    for (int i = 0; i < Adam.DO.Length; i++)
                    {
                        Adam.DO[i] = Convert.ToInt32(bData[i]);
                    }
                }
            }
            return Adam;
        }

        /// <summary>
        /// 获取Adam-6017模块电流/电压输入范围
        /// </summary>
        /// <param name="Adam">已连接的Adam模块</param>
        /// <returns>返回输入范围</returns>
        public static AdamModule6017 Get_Range(AdamModule6017 Adam)
        {
            if (Adam.ComR != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    Adam.Range[i] = GetChannelRange(Adam.ComR, i);
                }
            }
            return Adam;
        }
        /// <summary>
        /// 获取Adam-6018模块温度输入范围
        /// </summary>
        /// <param name="Adam">已连接的Adam模块</param>
        /// <returns>返回输入范围</returns>
        public static AdamModule6018 Get_Range(AdamModule6018 Adam)
        {
            if (Adam.ComR != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    Adam.Range[i] = GetChannelRange(Adam.ComR, i);
                }
            }
            return Adam;
        }

        /// <summary>
        /// 获取模块模拟量/温度输入范围
        /// </summary>
        /// <param name="Adam">已连接的Adam模块</param>
        /// <param name="Channel">通道序号</param>
        /// <returns>返回输入范围</returns>
        private static byte GetChannelRange(AdamSocket Adam, int Channel)
        {
            byte usRange;
            if (Adam != null &&Adam.Connected==true)
            {
                if (Adam.AnalogInput().GetInputRange(Channel, out usRange))
                {
                    return usRange;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取浮动格式
        /// </summary>
        public static string RefreshValue(AdamModule6017 Adam, int Channel)
        {
            string szFormat = AnalogInput.GetFloatFormat(Adam6000Type.Adam6017, Adam.Range[Channel]);
            return Adam.AI[Channel].ToString(szFormat);
        }
        /// <summary>
        /// 获取浮动格式
        /// </summary>
        public static string RefreshValue(AdamModule6018 Adam, int Channel)
        {
            string szFormat = AnalogInput.GetFloatFormat(Adam6000Type.Adam6018, Adam.Range[Channel]);
            return Adam.Temp[Channel].ToString(szFormat);
        }

        /// <summary>
        /// 获取当前使用单位，电压或电流单位
        /// </summary>
        public static string GetSymbol(AdamModule6017 Adam, int Channel)
        {
            return  AnalogInput.GetUnitName(Adam6000Type.Adam6017, Adam.Range[Channel]);
        }
        /// <summary>
        /// 获取当前使用单位，电压或电流单位
        /// </summary>
        public static string GetSymbol(AdamModule6018 Adam, int Channel)
        {
            string str = AnalogInput.GetUnitName(Adam6000Type.Adam6018, Adam.Range[Channel]);
            if (str == "'C")
            {
                str = " ℃";
            }
            else if (str == "'F")
            {
                str = " ℉";
            }
            return str;
        }
        /// <summary>
        /// 获取当前使用单位，电压或电流单位
        /// </summary>
        /// <param name="AdamIndex">已连接的Adam模块ID序号</param>
        /// <param name="Channel">模块通道序号</param>
        /// <returns>返回温度单位</returns>
        public static string GetSymbol(int AdamIndex, int Channel)
        {
            string Symbol = "";
            switch (GetAdamType(AdamIndex,ref AdamIndex))
            {
                case HaveAdamType.AdamModule6017:

                    Symbol = AnalogInput.GetUnitName(Adam6000Type.Adam6017, Adam6017[AdamIndex].Range[Channel]);
                  

                    break;

                case HaveAdamType.AdamModule6018:

                    Symbol = AnalogInput.GetUnitName(Adam6000Type.Adam6018, Adam6018[AdamIndex].Range[Channel]);

                    if (Symbol == "'C")
                    {
                        Symbol = " ℃";
                    }
                    else
                    {
                        Symbol = " ℉";
                    }

                    break;

                case HaveAdamType.AdamModule6250:

                    break;

                case HaveAdamType.AdamModule6256:

                    break;
            }

            return Symbol;

        }
        /// <summary>
        /// 将模块链路重新连接
        /// </summary>
        /// <param name="Adam">链路Socket</param>
        /// <returns></returns>
        public static bool Reconnect(AdamSocket Adam)
        {
            bool state = false;
            if ( Adam != null)
            {
                if (Adam.Connected == false)
                {
                    string IP = Adam.GetIP();
                    state = Adam.Connect(AdamType.Adam6000,IP, ProtocolType.Tcp);
                }
            }
            return state;
        }

        /// <summary>
        /// 将模块链路重新连接
        /// </summary>
        /// <param name="Index">模块的序号</param>
        /// <returns></returns>
        public static bool Reconnect(int Index)
        {
            bool state = true;

            switch (GetAdamType(Index, ref Index))
            {
                case HaveAdamType.AdamModule6017:

                    state = state && Reconnect(Adam.Adam6017[Index].ComR);
                    state = state && Reconnect(Adam.Adam6017[Index].ComT);
                    break;

                case HaveAdamType.AdamModule6018:

                    state = state && Reconnect(Adam.Adam6018[Index].ComR);
                    state = state && Reconnect(Adam.Adam6018[Index].ComT);

                    break;

                case HaveAdamType.AdamModule6250:

                    state = state && Reconnect(Adam.Adam6250[Index].ComR);
                    state = state && Reconnect(Adam.Adam6250[Index].ComT);

                    break;

                case HaveAdamType.AdamModule6256:

                    state = state && Reconnect(Adam.Adam6256[Index].ComR);
                    state = state && Reconnect(Adam.Adam6256[Index].ComT);

                    break;
            }
            return state;
        }
    }
}
