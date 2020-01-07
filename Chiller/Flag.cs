using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DewPointSensor;

namespace Chiller
{
    /// <summary>
    /// 枚举函数
    /// </summary>
    public class FlagEnum
    {
        /// <summary>
        /// 指示单元各部件状态
        /// </summary>
        public enum UnitState
        {
            /// <summary>
            /// 单元温度/压力良好
            /// </summary>
            Good = 1,
            /// <summary>
            /// 单元温度/压力正常
            /// </summary>
            Normal = 2,
            /// <summary>
            /// 单元温度/压力稍高
            /// </summary>
            SlightlyHigher = 3,
            /// <summary>
            /// 单元温度/压力超高
            /// </summary>
            Higher = 3,
            /// <summary>
            /// 单元温度/压力超限
            /// </summary>
            Transfinite = 4,
            /// <summary>
            /// 单元温度/压力异常
            /// </summary>
            Abnormal = 5,
            /// <summary>
            /// 等待降温/压，重启中
            /// </summary>
            Restart = 6,
        }
        /// <summary>
        /// 用户权限
        /// </summary>
        public enum UserAuthority
        {
            None = 1,
            Operator = 2,
            Engineer = 3,
            Administrator = 4,
        }

        public enum HeatMode
        {
            Hot = 1,
            ATC = 2,
        }

        /// <summary>
        /// 所有TestArm加热器枚举
        /// </summary>
        public enum TestArm
        {
            TestArm1 = 0,
            TestArm2 = 1,
            TestArm3 = 2,
            TestArm4 = 3,
            TestArm5 = 4,
            TestArm6 = 5,
            TestArm7 = 6,
            TestArm8 = 7,
        }
        /// <summary>
        /// 所有HeatPlate加热器枚举
        /// </summary>
        public enum HeatPlate
        {
            ColdPlate1 = 0,
            ColdPlate2 = 1,
            HotPlate1 = 2,
            HotPlate2 = 3,
        }

        public enum TempState
        {
            停止状态 = 0 ,
            初始加热 = 1 ,
            温度稳定 = 2 ,
            温度超限 = 3 

        }

        public enum ChillerState
        {
            无异常,
            参数写入失败,
            系统硬件异常,
            水箱液位低 ,
            水箱初始降温失败
        }
    }

    /// <summary>
    /// 类函数
    /// </summary>
    public class FlagStruct
    {
        #region 制冷单元结构参数
        public struct RecordBase
        {
            /// <summary>
            /// 记录数据
            /// </summary>
            public double Value;
            /// <summary>
            /// 记录时间
            /// </summary>
            public string Time;
        }

        public struct _Temp
        {
            /// <summary>
            /// 温度记录用于生产曲线
            /// </summary>
            private ArrayList RecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int RecordLength;
            /// <summary>
            /// 温度值
            /// </summary>
            public float Value;
            /// <summary>
            /// 模拟量原始值
            /// </summary>
            public float Analog;
            /// <summary>
            /// 模拟量原始值使用单位
            /// </summary>
            public string AnalogCompany;
            /// <summary>
            /// 温度地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 温度超限
            /// </summary>
            public bool HightTempGrade;

            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void InitiRecord()
            {
                RecordList = new ArrayList();
            }
            /// <summary>
            /// 将当前温度添加到曲线记录
            /// </summary>
            public void AddToRecord()
            {
                if (RecordList == null)
                {
                    RecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = Value;
                if (RecordList.Count >= RecordLength)
                {
                    RecordList.RemoveRange(0, RecordList.Count - RecordLength + 1);
                }
                RecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void RecordClear()
            {
                if (RecordList != null)
                {
                    RecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (RecordList != null)
                    {
                        return RecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }

            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetRecordValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (RecordList != null)
                {
                    if (RecordList.Count > Index)
                    {
                        Record = (RecordBase)RecordList[Index];
                    }
                }
                return Record;
            }

        }
        public struct _Pressure
        {
            /// <summary>
            /// 压力记录用于生产曲线
            /// </summary>
            private ArrayList RecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int RecordLength;
            /// <summary>
            /// 压力值
            /// </summary>
            public float Value;
            /// <summary>
            /// 模拟量原始值
            /// </summary>
            public float Analog;
            /// <summary>
            /// 模拟量原始值使用单位
            /// </summary>
            public string AnalogCompany;
            /// <summary>
            /// 压力地址
            /// </summary>
            public string Address;

            /// <summary>
            /// 压力超限
            /// </summary>
            public bool HightPressureGrade;
            
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void InitiRecord()
            {
                RecordList = new ArrayList();
            }
            /// <summary>
            /// 将当前压力添加到曲线记录
            /// </summary>
            public void AddToRecord()
            {
                if (RecordList == null)
                {
                    RecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = Value;
                if (RecordList.Count >= RecordLength)
                {
                    RecordList.RemoveRange(0, RecordList.Count - RecordLength + 1);
                }
                RecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void RecordClear()
            {
                if (RecordList != null)
                {
                    RecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前已记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (RecordList != null)
                    {
                        return RecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetRecordValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (RecordList != null)
                {
                    if (RecordList.Count > Index)
                    {
                        Record = (RecordBase)RecordList[Index];
                    }
                }
                return Record;
            }
        }

        public struct _RunAndStop
        {
            /// <summary>
            /// 压缩机启动或停止状态
            /// </summary>
            public bool State;
            /// <summary>
            /// 压缩机启动或停止地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 启动或停止中间间隔
            /// </summary>
            public Stopwatch IntervalTime;
        }
        public struct _Compressor_In
        {
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Compressor_Out
        {
            /// <summary>
            /// 温度
            /// </summary>
            public _Temp Temp;
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Compressor
        {
            /// <summary>
            /// 启动或停止
            /// </summary>
            public _RunAndStop RunAndStop;
            /// <summary>
            /// 入口
            /// </summary>
            public _Compressor_In In;
            /// <summary>
            /// 出口
            /// </summary>
            public _Compressor_Out Out;
        }

        public struct _Condenser_In
        {
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Condenser_Out
        {
            /// <summary>
            /// 温度
            /// </summary>
            public _Temp Temp;
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Condenser
        {
            /// <summary>
            /// 入口
            /// </summary>
            public _Condenser_In In;
            /// <summary>
            /// 出口
            /// </summary>
            public _Condenser_Out Out;
        }

        public struct _Evaporator_In
        {
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Evaporator_Out
        {          
            /// <summary>
            /// 温度
            /// </summary>
            public _Temp Temp;
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _Evaporator
        {
            /// <summary>
            /// 入口
            /// </summary>
            public _Evaporator_In In;
            /// <summary>
            /// 出口
            /// </summary>
            public _Evaporator_Out Out;
        }

        public struct _HeatExchanger_Out
        {
            /// <summary>
            /// 压力
            /// </summary>
            public _Pressure Pressure;
        }
        public struct _HeatExchanger
        {
            /// <summary>
            /// 出口
            /// </summary>
            public _HeatExchanger_Out Out;
        }

        public struct Valve
        {
            /// <summary>
            /// 电磁阀状态
            /// </summary>
            public bool State;
            /// <summary>
            /// 电磁阀状态地址
            /// </summary>
            public string Address;
        }

        /// <summary>
        /// 压缩机制冷单元结构
        /// </summary>
        public struct Unit
        {
            /// <summary>
            /// 压缩机参数
            /// </summary>
            public _Compressor Compressor;

            /// <summary>
            /// 冷凝器参数
            /// </summary>
            public _Condenser Condenser;

            /// <summary>
            /// 蒸发器参数
            /// </summary>
            public _Evaporator Evaporator;

            /// <summary>
            /// 换热器参数
            /// </summary>
            public _HeatExchanger HeatExchanger;

            /// <summary>
            /// 内循环泵
            /// </summary>
            public Pump InternalPump;

            /// <summary>
            /// 旁通电磁阀参数
            /// </summary>
            public Valve BypassValve;

            /// <summary>
            /// 并通电磁阀参数
            /// </summary>
            public Valve ParallelValve;

            /// <summary>
            /// 机组制冷开启使能
            /// </summary>
            public bool Enabled;

            /// <summary>
            /// 制冷开启失败，超时等错误！
            /// </summary>
            public bool StartErr;

            /// <summary>
            /// 制冷关闭失败，超时等错误！
            /// </summary>
            public bool EndErr;

            /// <summary>
            /// 线程开启状态
            /// </summary>
            public bool IsThreadStart;

        }
        #endregion

        #region 水箱结构参数
        public struct _InternalValve
        {
            /// <summary>
            /// 内通道电磁阀状态
            /// </summary>
            public bool State;
            /// <summary>
            /// 内通道电磁阀状态地址
            /// </summary>
            public string Address;
        }
        public struct _ExternalValve
        {
            /// <summary>
            /// 外通道电磁阀状态
            /// </summary>
            public bool State;
            /// <summary>
            /// 外通道电磁阀状态地址
            /// </summary>
            public string Address;
        }
        public struct _Height
        {
            /// <summary>
            /// 液位记录用于生产曲线
            /// </summary>
            private ArrayList RecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int RecordLength;
            /// <summary>
            /// 当前液位高度
            /// </summary>
            public float Value;
            /// <summary>
            /// 模拟量原始值
            /// </summary>
            public float Analog;
            /// <summary>
            /// 模拟量原始值使用单位
            /// </summary>
            public string AnalogCompany;
            /// <summary>
            /// 当前液位高度地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void InitiRecord()
            {
                RecordList = new ArrayList();
            }
            /// <summary>
            /// 将当前液位添加到曲线记录
            /// </summary>
            public void AddToRecord()
            {
                if (RecordList == null)
                {
                    RecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = Value;
                if (RecordList.Count >= RecordLength)
                {
                    RecordList.RemoveRange(0, RecordList.Count - RecordLength + 1);
                }
                RecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void RecordClear()
            {
                if (RecordList != null)
                {
                    RecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前已记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (RecordList != null)
                    {
                        return RecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }

            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetRecordValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (RecordList != null)
                {
                    if (RecordList.Count > Index)
                    {
                        Record = (RecordBase)RecordList[Index];
                    }
                }
                return Record;
            }
        }
        public struct _WaterTemp
        {
            /// <summary>
            /// 液体温度记录用于生产曲线
            /// </summary>
            private ArrayList RecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int RecordLength;
            /// <summary>
            /// 当前液位温度
            /// </summary>
            public float Value;
            /// <summary>
            /// 模拟量原始值
            /// </summary>
            public float Analog;
            /// <summary>
            /// 模拟量原始值使用单位
            /// </summary>
            public string AnalogCompany;
            /// <summary>
            /// 当前液位温度地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void InitiRecord()
            {
                RecordList = new ArrayList();
            }
            /// <summary>
            /// 将当前液体温度添加到曲线记录
            /// </summary>
            public void AddToRecord()
            {
                if (RecordList == null)
                {
                    RecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = Value;
                if (RecordList.Count >= RecordLength)
                {
                    RecordList.RemoveRange(0, RecordList.Count - RecordLength + 1);
                }
                RecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void RecordClear()
            {
                if (RecordList != null)
                {
                    RecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前已记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (RecordList != null)
                    {
                        return RecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }

            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetRecordValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (RecordList != null)
                {
                    if (RecordList.Count > Index)
                    {
                        Record = (RecordBase)RecordList[Index];
                    }
                }
                return Record;
            }
        }
        public struct _Parameter
        {
            /// <summary>
            /// 最高液位
            /// </summary>
            public float Hight;
            /// <summary>
            /// 最低液位
            /// </summary>
            public float Low;
            /// <summary>
            /// 设定的液体温度
            /// </summary>
            public float SetTemp;
            /// <summary>
            /// 制冷液体温度范围
            /// </summary>
            public float SetRange;
            /// <summary>
            /// 液体设定温度记录用于生产曲线
            /// </summary>
            private ArrayList SetTempRecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int RecordLength;
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void InitiRecord()
            {
                SetTempRecordList = new ArrayList();
            }
            /// <summary>
            /// 将当前液体设定温度添加到曲线记录
            /// </summary>
            public void AddToSetTempRecord()
            {
                if (SetTempRecordList == null)
                {
                    SetTempRecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = SetTemp;
                if (SetTempRecordList.Count >= RecordLength)
                {
                    SetTempRecordList.RemoveRange(0, SetTempRecordList.Count - RecordLength + 1);
                }
                SetTempRecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void RecordClear()
            {
                if (SetTempRecordList != null)
                {
                    SetTempRecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前已记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (SetTempRecordList != null)
                    {
                        return SetTempRecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }

            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetRecordValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (SetTempRecordList != null)
                {
                    if (SetTempRecordList.Count > Index)
                    {
                        Record = (RecordBase)SetTempRecordList[Index];
                    }
                }
                return Record;
            }
        }
        public struct _Offset
        {
            /// <summary>
            /// 液体温度当前偏移值,[0]-[2]为3点的校准偏移值[3]为当前使用的偏移值
            /// </summary>
            public float[] TempOffset;
            /// <summary>
            /// 温度偏移点数，使用几点校准
            /// </summary>
            public int TempOffsetPoint;
            /// <summary>
            /// 温度偏移基点数组仅长度为3
            /// </summary>
            public float[] TempBasicPoint;
        }
        /// <summary>
        /// 水箱结构
        /// </summary>
        public struct WaterBox
        {
            /// <summary>
            /// 内通道
            /// </summary>
            public _InternalValve InternalValve;

            /// <summary>
            /// 外通道
            /// </summary>
            public _ExternalValve ExternalValve;

            /// <summary>
            /// 液体高度
            /// </summary>
            public _Height Height;

            /// <summary>
            /// 液体温度
            /// </summary>
            public _WaterTemp Temp;

            /// <summary>
            /// 液体相关参数
            /// </summary>
            public _Parameter Parameter;

            /// <summary>
            /// 偏移相关参数
            /// </summary>
            public _Offset Offset;
        }
        #endregion

        /// <summary>
        /// 循环泵单元结构
        /// </summary>
        public struct Pump
        {
            /// <summary>
            /// 泵的地址
            /// </summary>
            public byte Address;
            /// <summary>
            /// 泵是否连接成功
            /// </summary>
            public bool IsConnent;
            /// <summary>
            /// 内循环泵开启状态
            /// </summary>
            public bool IsRun;
            /// <summary>
            /// 泵加速时间，单位ms
            /// </summary>
            public ushort Tacc;
            /// <summary>
            /// 泵减速时间，单位ms
            /// </summary>
            public ushort Tdec;
            /// <summary>
            /// 泵当前转速r/min
            /// </summary>
            public ushort Speed;
            /// <summary>
            /// 泵设定的转速
            /// </summary>
            public ushort SetSpeed;
            /// <summary>
            /// 泵当前是否反向旋转，false为正传 ，true为反转
            /// </summary>
            public bool Direction;
            /// <summary>
            /// 是否支持清除的报警
            /// </summary>
            public bool IsErrClear;
            /// <summary>
            /// 伺服是否存在报警
            /// </summary>
            public bool IsErr;
            /// <summary>
            /// 伺服的报警码
            /// </summary>
            public string ErrCode;
            /// <summary>
            /// 伺服电机当前负载
            /// </summary>
            public int OperatingLoad;
            /// <summary>
            /// 当前地址连接失败次数
            /// </summary>
            public int ConnentErrNumber;
            /// <summary>
            /// 电机编码器当前温度
            /// </summary>
            public short EncoderTemp;
        }

        public struct LogBase
        {
            public string time;
            public string text;
            public Color color;
        }
        /// <summary>
        /// 系统运行日志和当前报警信息记录
        /// </summary>
        public struct AddSystemLogList
        {
            private ArrayList LogList;
            private int _LastIndex;
            private int _AdditionNum;
            /// <summary>
            /// 初始化Log
            /// </summary>
            public void InitiLog()
            {
                LogList = new ArrayList();
            }
            /// <summary>
            /// 往系统中添加Log
            /// </summary>
            /// <param name="text">文本字符</param>
            /// <param name="color">文本颜色</param>
            public void Add(string Time, string text, Color color)
            {
                if (LogList == null)
                {
                    LogList = new ArrayList();
                }
                LogBase Log;
                Log.time = Time;
                Log.text = text;
                Log.color = color;

                _LastIndex = LogList.Add(Log) + 1;
                _AdditionNum++;
            }
            /// <summary>
            /// 获取最后一次添加的位置
            /// </summary>
            public int LastIndex
            {
                get
                {
                    return _LastIndex;
                }
            }
            /// <summary>
            /// 当前添加次数
            /// </summary>
            public int AdditionNum
            {
                get
                {
                    return _AdditionNum;
                }
            }
            /// <summary>
            /// 对当前已添加的数量计数进行清零
            /// </summary>
            public void ClearAdditionNum()
            {
                _AdditionNum = 0;
            }
            /// <summary>
            /// 移除与当前字符串相匹配的Log
            /// </summary>
            /// <param name="Time">Log的前端时间</param>
            /// <param name="text">索引字符串</param>
            public void Remove(LogBase SystemLog)
            {
                if (LogList != null)
                {
                    LogList.Remove(SystemLog);
                }
            }
            /// <summary>
            /// 移除指定索引的Log
            /// </summary>
            /// <param name="Index">索引</param>
            public void RemoveAt(int Index)
            {
                if (LogList != null)
                {
                    LogList.RemoveAt(Index);
                }
            }
            /// <summary>
            /// 清除系统中的全部Log
            /// </summary>
            public void Clear()
            {
                if (LogList != null)
                {
                    LogList.Clear();
                }
            }
            /// <summary>
            /// 获取系统中记录的Log总数
            /// </summary>
            public int Count
            {
                get
                {
                    if (LogList != null)
                    {
                        return LogList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            /// <summary>
            /// 分段数量，每段显示300条
            /// </summary>
            public int Paragraph
            {
                get
                {
                    if (LogList != null)
                    {
                        return LogList.Count / 300;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            /// <summary>
            /// 判断该Log是否存在与当前系统中
            /// </summary>
            /// <param name="SystemLog">查询的Log</param>
            /// <returns></returns>
            public bool Contains(LogBase SystemLog)
            {
                bool Contains = false;
                if (LogList != null)
                {
                    Contains = LogList.Contains(SystemLog);
                }
                return Contains;
            }
            /// <summary>
            /// 判断该Log是否存在与当前系统中
            /// </summary>
            /// <param name="text">查询的Log</param>
            /// <returns></returns>
            public bool Contains(string text)
            {
                bool Contains = false;
                if (LogList != null)
                {
                    LogBase Log;
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        Log = (LogBase)LogList[i];
                        if (Log.text == text)
                        {
                            Contains = true;
                            break;
                        }
                    }
                }
                return Contains;
            }
            /// <summary>
            /// 获取Log所在序号从0开始，返回-1表示不存在
            /// </summary>
            /// <param name="Time">Log的前端时间</param>
            /// <param name="text">查询的字符串</param>
            /// <returns></returns>
            public int Index(LogBase SystemLog)
            {
                int Index = -1;
                if (LogList != null)
                {
                    Index = LogList.IndexOf(SystemLog);
                }
                return Index;
            }
            /// <summary>
            /// 获取索引的Log信息
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public LogBase GetLog(int Index)
            {
                LogBase Log;
                Log.color = Color.Black;
                Log.text = "";
                Log.time = "";
                if (LogList != null)
                {
                    if (LogList.Count > Index)
                    {
                        Log = (LogBase)LogList[Index];
                    }
                }
                return Log;
            }
        }
        /// <summary>
        /// 系统运行日志和当前报警信息记录
        /// </summary>
        public struct SystemLogList
        {
            private ArrayList LogList;
            public delegate void SelectHandler();
            public event SelectHandler ResetDisplay; //定义一个刷新界面的委托类型事件  

            /// <summary>
            /// 初始化Log
            /// </summary>
            public void InitiLog()
            {
                LogList = new ArrayList();
            }
            /// <summary>
            /// 往系统中添加Log
            /// </summary>
            /// <param name="text">文本字符</param>
            /// <param name="color">文本颜色</param>
            public void Add(string text, Color color)
            {
                if (LogList == null)
                {
                    LogList = new ArrayList();
                }
                LogBase Log;
                Log.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
                Log.text = text;
                Log.color = color;

                LogList.Add(Log);
                if (ResetDisplay != null)
                {
                    ResetDisplay();
                }
            }
            /// <summary>
            /// 往系统中添加Log,默认黑色字体
            /// </summary>
            /// <param name="text">文本字符</param>
            public void Add(string text)
            {
                if (LogList == null)
                {
                    LogList = new ArrayList();
                }
                LogBase Log;
                Log.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
                Log.text = text;
                Log.color = Color.Black;

                LogList.Add(Log);
                if (ResetDisplay != null)
                {
                    ResetDisplay();
                }
            }
            /// <summary>
            /// 移除与当前字符串相匹配的Log
            /// </summary>
            /// <param name="text">索引字符串</param>
            public void Remove(string text)
            {
                if (LogList != null)
                {
                    LogBase Log;
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        Log = (LogBase)LogList[i];
                        if (Log.text == text)
                        {
                            LogList.RemoveAt(i);
                            if (ResetDisplay != null)
                            {
                                ResetDisplay();
                            }
                            break;
                        }
                    }
                }
            }
            /// <summary>
            /// 移除指定索引的Log
            /// </summary>
            /// <param name="Index">索引</param>
            public void RemoveAt(int Index)
            {
                if (LogList != null)
                {
                    LogList.RemoveAt(Index);
                    if (ResetDisplay != null)
                    {
                        ResetDisplay();
                    }
                }
            }
            /// <summary>
            /// 清除系统中的全部Log
            /// </summary>
            public void Clear()
            {
                if (LogList != null)
                {
                    LogList.Clear();
                    if (ResetDisplay != null)
                    {
                        ResetDisplay();
                    }
                }
            }
            /// <summary>
            /// 获取系统中记录的Log总数
            /// </summary>
            public int Count
            {
                get
                {
                    if (LogList != null)
                    {
                        return LogList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            /// <summary>
            /// 判断该Log是否存在与当前系统中
            /// </summary>
            /// <param name="text">查询的Log</param>
            /// <returns></returns>
            public bool Contains(string text)
            {
                bool Contains = false;
                if (LogList != null)
                {
                    LogBase Log;
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        Log = (LogBase)LogList[i];
                        if (Log.text == text)
                        {
                            Contains = true;
                            break;
                        }
                    }
                }
                return Contains;
            }
            /// <summary>
            /// 判断该Log是否存在与当前系统中
            /// </summary>
            /// <param name="QueryLog">查询的Log</param>
            /// <returns></returns>
            public bool Contains(LogBase QueryLog)
            {
                bool Contains = false;
                if (LogList != null)
                {
                    Contains = LogList.Contains(QueryLog);
                }
                return Contains;
            }
            /// <summary>
            /// 获取Log所在序号从0开始，返回-1表示不存在
            /// </summary>
            /// <param name="text">查询的字符串</param>
            /// <returns></returns>
            public int Index(string text)
            {
                int Index = -1;
                if (LogList != null)
                {
                    LogBase Log;
                    for (int i = 0; i < LogList.Count; i++)
                    {
                        Log = (LogBase)LogList[0];
                        if (Log.text == text)
                        {
                            Index = i;
                            break;
                        }
                    }
                }
                return Index;
            }
            /// <summary>
            /// 获取索引的Log信息
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public LogBase GetLog(int Index)
            {
                LogBase Log;
                Log.color = Color.Black;
                Log.text = "";
                Log.time = "";
                if (LogList != null)
                {
                    if (LogList.Count > Index)
                    {
                        if (LogList[Index] != null)
                        {
                            try
                            {
                                Log = (LogBase)LogList[Index];
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                return Log;
            }
        }

        /// <summary>
        /// 液体通道电磁阀
        /// </summary>
        public struct _ColdValve
        {
            /// <summary>
            /// 通道电磁阀状态
            /// </summary>
            public bool State;
            /// <summary>
            /// 通道电磁阀状态地址
            /// </summary>
            public string Address;
        }

        /// <summary>
        /// 设定加热器所需要的参数
        /// </summary>
        public struct _SetToHeat
        {
            /// <summary>
            /// 设定的温度
            /// </summary>
            public float Temp;
            /// <summary>
            /// 设定的温度范围
            /// </summary>
            public float Range;
            /// <summary>
            /// 通道使能
            /// </summary>
            public bool Enabled;
            /// <summary>
            /// 当前通道是否开启加热
            /// </summary>
            public bool IsUseHeat;
            /// <summary>
            /// 输入温度修正值
            /// </summary>
            public float OffTk;
            /// <summary>
            /// 加热功率上限（100％）
            /// </summary>
            public ushort PowerLimit;
            /// <summary>
            /// 偏移设定
            /// </summary>
            public float Offset;
            /// <summary>
            /// 指示当前加热器的运行状态
            /// </summary>
            public bool IsWriteToPma;
            /// <summary>
            /// 更改过当前温控参数
            /// </summary>
            public bool ChangeState;
        }

        public struct _GetInData
        {
            /// <summary>
            /// 读取设定的温度
            /// </summary>
            public float SP;
            /// <summary>
            /// 当前温度
            /// </summary>
            public float PV;
            /// <summary>
            ///该通道控制字状态2
            /// </summary>
            public double C_Sta2;
            /// <summary>
            /// 该温控器通道是否处于开启状态
            /// </summary>
            public bool IsRun;
            /// <summary>
            /// 异常错误代码，为""时表示当前无错误
            /// </summary>
            public string ErrCode;
        }

        public struct _GetInPower
        {
            /// <summary>
            /// 读取加热功率
            /// </summary>
            public ushort PowerLimit;
        }

        public struct _Record
        {
            /// <summary>
            /// 设定的温度记录用于生产曲线
            /// </summary>
            private ArrayList RecordList;
            /// <summary>
            /// 设定或获取系统记录数据的长度
            /// </summary>
            public int Length;
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            public void Init()
            {
                RecordList = new ArrayList();
            }
            /// <summary>
            /// 初始化数据记录
            /// </summary>
            /// <param name="Length">数据记录长度</param>
            public void Init(int length)
            {
                RecordList = new ArrayList();
                Length = length;
            }
            /// <summary>
            /// 将当前温度添加到曲线记录
            /// </summary>
            public void Add(double Value)
            {
                if (RecordList == null)
                {
                    RecordList = new ArrayList();
                }
                RecordBase data;
                data.Time = DateTime.Now.ToString("HH:mm:ss");
                data.Value = Value;
                if (RecordList.Count >= Length)
                {
                    RecordList.RemoveRange(0, RecordList.Count - Length + 1);
                }
                RecordList.Add(data);
            }
            /// <summary>
            /// 清除系统中记录的数据
            /// </summary>
            public void Clear()
            {
                if (RecordList != null)
                {
                    RecordList.Clear();
                }
            }
            /// <summary>
            /// 获取当前已记录的数据长度
            /// </summary>
            public int Count
            {
                get
                {
                    if (RecordList != null)
                    {
                        return RecordList.Count;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            /// <summary>
            /// 读取索引序号所记录的数据
            /// </summary>
            /// <param name="Index">索引</param>
            /// <returns></returns>
            public RecordBase GetValue(int Index)
            {
                RecordBase Record;
                Record.Value = 0;
                Record.Time = "";
                if (RecordList != null)
                {
                    if (RecordList.Count > Index)
                    {
                        Record = (RecordBase)RecordList[Index];
                    }
                }
                return Record;
            }
        }
        /// <summary>
        /// 对应Handler加热
        /// </summary>
        public struct _Heating
        {
            /// <summary>
            /// 通道名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 通道对应温控器地址 例：1-1
            /// </summary>
            public string Address;
            /// <summary>
            /// 该数据用于下载进温控器或温控器设定相关
            /// </summary>
            public _SetToHeat SetToHeat;
            /// <summary>
            /// 该数据是从温控器读取出来
            /// </summary>
            public _GetInData GetInData;
            /// <summary>
            /// 用于记录设定的温度记录
            /// </summary>
            public _Record SetRecord;
            /// <summary>
            /// 用于记录试试的温度记录
            /// </summary>
            public _Record NowRecord;
            /// <summary>
            /// 读取加热功率
            /// </summary>
            public _GetInPower GetInPower;
        }

        /// <summary>
        ///  Handler侧温度相关信息，包含当前温度和设定温度
        /// </summary>
        public struct TestArmHeatingChannel
        {
            /// <summary>
            /// 冷液阀
            /// </summary>
            public _ColdValve Valve;
            /// <summary>
            /// 测试头内部加热
            /// </summary>
            public _Heating Heat_Inside;
            /// <summary>
            /// 测试头接触IC位置加热
            /// </summary>
            public _Heating Heat_IC;
            /// <summary>
            /// 加热模式
            /// </summary>
            public FlagEnum.HeatMode HeatMode;
        }
        /// <summary>
        ///  Handler侧温度相关信息，包含当前温度和设定温度
        /// </summary>
        public struct ColdPlateHeatingChannel
        {
            /// <summary>
            /// 冷液阀
            /// </summary>
            public _ColdValve Valve;
            /// <summary>
            /// 加热
            /// </summary>
            public _Heating Heat;
        }
        public struct HeatingChannel
        {
            /// <summary>
            /// 加热
            /// </summary>
            public _Heating Heat;
        }

        public struct ChartSetUp
        {
            /// <summary>
            /// 曲线颜色
            /// </summary>
            public Color Color;

            /// <summary>
            /// 曲线类型
            /// </summary>
            public System.Windows.Forms.DataVisualization.Charting.SeriesChartType Type;
        }

        /// <summary>
        /// 外部温控控制页面所有参数
        /// </summary>
        public struct InternalColdControl
        {
            /// <summary>
            /// 是否刷新曲线使能
            /// </summary>
            public bool ChartRefreshEnabled;
            /// <summary>
            /// 曲线显示的最低温度点
            /// </summary>
            public int ChartLovTemp;
            /// <summary>
            /// 曲线显示的最高温度点
            /// </summary>
            public int ChartHightTemp;
            /// <summary>
            /// 显示曲线的长度
            /// </summary>
            public int ChartPointLength;
            /// <summary>
            /// 曲线刷新频率
            /// </summary>
            public int ChartRefreshTime;

            /// <summary>
            /// 压缩机组曲线相关设定
            /// </summary>
            public ChartSetUp[] UnitChart;

            /// <summary>
            /// 水箱设定温度曲线相关
            /// </summary>
            public ChartSetUp SetTempChart;
            /// <summary>
            /// 水箱当前温度曲线相关
            /// </summary>
            public ChartSetUp NowTempChart;

            /// <summary>
            /// 颜色选择器
            /// </summary>
            public ColorPicker.ColorFrom ColorPicker;
        }

        /// <summary>
        /// 外部测试所需要的温度设定
        /// </summary>
        public struct ExternalTestTempChange
        {
            /// <summary>
            /// 加热模式
            /// </summary>
            public FlagEnum.HeatMode HeatMode;
            /// <summary>
            /// 是否开启边框加热
            /// </summary>
            public bool BorderHeat ;
            /// <summary>
            /// 使能开启界面是否打开全部通道显示
            /// </summary>
            public bool DisplayAllChannel;
            /// <summary>
            /// 测试头或预热预冷盘所需使用温度
            /// </summary>
            public float TestTemp;
            /// <summary>
            /// 测试头或预热预冷盘所需温度范围
            /// </summary>
            public float TestRange;
            /// <summary>
            /// Chiller制冷温度
            /// </summary>
            public float ChillerTemp;
            /// <summary>
            /// Chiller制冷控制精度
            /// </summary>
            public float ChillerRange;
            /// <summary>
            /// 预冷盘使用温度
            /// </summary>
            public double PreCoolTemp;
            /// <summary>
            /// 回温所需温度
            /// </summary>
            public double ResultTemp;

            /// <summary>
            /// 未使能状态，显示当前通道温度
            /// </summary>
            public bool NoEnableDisplayTemp;

            /// <summary>
            /// 是否使用独立通道切换加热使能
            /// </summary>
            public bool IndependentUseHeat;
            /// <summary>
            /// 是否采用IC加热
            /// </summary>
            public bool UseIcHeat;

            /// <summary>
            /// 测试臂工作使能
            /// </summary>
            public bool[] TestArmEnabled;
            /// <summary>
            /// 预冷盘工作使能
            /// </summary>
            public bool[] ColdPlateEnabled;
            /// <summary>
            /// 预热盘工作使能
            /// </summary>
            public bool[] HotPlateEnabled;
            /// <summary>
            /// 边框工作使能
            /// </summary>
            public bool[] BorderEnabled;
            /// <summary>
            /// 测试臂选择IC加热使能
            /// </summary>
            public bool[] TestArmUseIcHeat;

            /// <summary>
            /// 测试头或预热预冷盘使用几点便宜校准
            /// </summary>
            public short TestTempOffsetPoint;
            /// <summary>
            /// 测试头或预热预冷盘偏移校准点
            /// </summary>
            public double[] TestTempBasicPoint;

            /// <summary>
            /// 边框所需使用温度
            /// </summary>
            public double BorderTemp;
            /// <summary>
            /// 边框使用几点便宜校准
            /// </summary>
            public int BorderTempOffsetPoint;
            /// <summary>
            /// 边框偏移校准点
            /// </summary>
            public double[] BorderTempBasicPoint;
        }

        /// <summary>
        /// 外部温控控制页面所有参数
        /// </summary>
        public struct ExternalTempControlChart
        {
            /// <summary>
            /// 是否刷新曲线使能
            /// </summary>
            public bool ChartRefreshEnabled;
            /// <summary>
            /// 曲线显示的最低温度点
            /// </summary>
            public int ChartLovTemp;
            /// <summary>
            /// 曲线显示的最高温度点
            /// </summary>
            public int ChartHightTemp;
            /// <summary>
            /// 显示曲线的长度
            /// </summary>
            public int ChartPointLength;
            /// <summary>
            /// 曲线刷新频率
            /// </summary>
            public int ChartRefreshTime;

            ///// <summary>
            ///// Plate1的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp Plate1Chart;
            ///// <summary>
            ///// Plate2的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp Plate2Chart;
            ///// <summary>
            ///// ShuttleA的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp ShuttleAChart;
            ///// <summary>
            ///// ShuttleB的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp ShuttleBChart;
            ///// <summary>
            ///// TestArmA1的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp TestArmA1Chart;
            ///// <summary>
            ///// TestArmA2的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp TestArmA2Chart;
            ///// <summary>
            ///// TestArmB1的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp TestArmB1Chart;
            ///// <summary>
            ///// TestArmB2的当前温度曲线相关
            ///// </summary>
            //public ChartSetUp TestArmB2Chart;

            /// <summary>
            /// 温度曲线相关
            /// </summary>
            public ChartSetUp[] TempChart;
            /// <summary>
            /// 颜色选择器
            /// </summary>
            public ColorPicker.ColorFrom ColorPicker;
        }

        /// <summary>
        /// 压缩机运行状态页面数据组
        /// </summary>
        public struct UnitWorkState
        {
            /// <summary>
            /// 是否刷新曲线使能
            /// </summary>
            public bool ChartRefreshEnabled;
            /// <summary>
            /// 显示数据的压缩机组序号
            /// </summary>
            public uint UnitIndex;
            /// <summary>
            /// 曲线显示的最低温度点
            /// </summary>
            public int ChartLovTemp;
            /// <summary>
            /// 曲线显示的最高温度点
            /// </summary>
            public int ChartHightTemp;
            /// <summary>
            /// 曲线显示的最低压力点
            /// </summary>
            public int ChartLovPressure;
            /// <summary>
            /// 曲线显示的最高压力点
            /// </summary>
            public int ChartHightPressure;
            /// <summary>
            /// 显示曲线的长度
            /// </summary>
            public int ChartPointLength;
            /// <summary>
            /// 曲线刷新频率
            /// </summary>
            public int ChartRefreshTime;

            /// <summary>
            /// 压缩机入口压力曲线相关设定
            /// </summary>
            public ChartSetUp CompressorInPressureChart;
            /// <summary>
            /// 压缩机出口压力曲线相关设定
            /// </summary>
            public ChartSetUp CompressorOutPressureChart;

            /// <summary>
            /// 冷凝器入口压力曲线相关设定
            /// </summary>
            public ChartSetUp CondenserInPressureChart;
            /// <summary>
            /// 冷凝器出口压力曲线相关设定
            /// </summary>
            public ChartSetUp CondenserOutPressureChart;

            /// <summary>
            /// 蒸发器入口压力曲线相关设定
            /// </summary>
            public ChartSetUp EvaporatorInPressureChart;
            /// <summary>
            /// 蒸发器出口压力曲线相关设定
            /// </summary>
            public ChartSetUp EvaporatorOutPressureChart;

            /// <summary>
            /// 回热器出口压力曲线相关设定
            /// </summary>
            public ChartSetUp HeatExchangerOutPressureChart;

            /// <summary>
            /// 压缩机出口温度曲线相关设定
            /// </summary>
            public ChartSetUp CompressorOutTempChart;
            /// <summary>
            /// 冷凝器出口温度曲线相关设定
            /// </summary>
            public ChartSetUp CondenserOutTempChart;
            /// <summary>
            /// 蒸发器入口温度曲线相关设定
            /// </summary>
            public ChartSetUp EvaporatorInTempChart;

            /// <summary>
            /// 颜色选择器
            /// </summary>
            public ColorPicker.ColorFrom ColorPicker;
        }

        /// <summary>
        /// 系统诊断页面
        /// </summary>
        public struct Diagnosis
        {
            /// <summary>
            /// 内循环泵1手动速度
            /// </summary>
            public uint InternalPumpSpeed1;
            /// <summary>
            /// 内循环泵2手动速度
            /// </summary>
            public uint InternalPumpSpeed2;
            /// <summary>
            /// 内循环泵3手动速度
            /// </summary>
            public uint InternalPumpSpeed3;
            /// <summary>
            /// 内循环泵4手动速度
            /// </summary>
            public uint InternalPumpSpeed4;

            /// <summary>
            /// 外循环泵1手动速度
            /// </summary>
            public uint ExternalPumpSpeed1;
            /// <summary>
            /// 外循环泵2手动速度
            /// </summary>
            public uint ExternalPumpSpeed2;
            /// <summary>
            /// 外循环泵3手动速度
            /// </summary>
            public uint ExternalPumpSpeed3;
        }

        /// <summary>
        /// 用户组相关数据
        /// </summary>
        public struct User
        {
            /// <summary>
            /// 登陆用户名
            /// </summary>
            public string UserName;
            /// <summary>
            /// 用户权限
            /// </summary>
            public FlagEnum.UserAuthority UserAuthority;
            /// <summary>
            /// 是否支持自动登出
            /// </summary>
            public bool IsAutoLogout;
            /// <summary>
            /// 自动登出时间
            /// </summary>
            public int AutoLogoutTime;
            /// <summary>
            /// 测量鼠标空闲时间，用于做自动登出使用
            /// </summary>
            public SystemFreeTime FreeTime;
        }

        public struct RunHeat
        {
            private bool _Heating;
            /// <summary>
            /// 是否开启加热总使能
            /// </summary>
            public bool Heating
            {
                get
                {
                    return _Heating;
                }

                set
                {
                    _Heating = value;
                }
            }

            /// <summary>
            /// 当前参数是否为Handler传入
            /// </summary>
            public bool IsHandlerHeat;

            /// <summary>
            /// 修改状态的进度
            /// </summary>
            public int ChangeProgress;
        }

        public struct RunCold
        {
            private bool _Colding;
            /// <summary>
            /// 是否开启制冷总使能
            /// </summary>
            public bool Colding
            {
                get
                {
                    return _Colding;
                }

                set
                {
                    _Colding = value;
                    if (RefreshColdingState != null)
                    {
                        RefreshColdingState(_Colding);
                    }
                }
            }

            /// <summary>
            /// 是否停止制冷
            /// </summary>
            public bool IsStop;

            /// <summary>
            /// 制冷状态更改事件委托
            /// </summary>
            /// <param name="State">当前状态</param>
            public delegate void ColdingChange(bool State);
            /// <summary>
            /// 制冷状态更改触发事件
            /// </summary>
            public event ColdingChange RefreshColdingState; //定义一个刷新界面的委托类型事件  
        }

        public struct Varieties
        {
            /// <summary>
            /// 设定的品种名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 设定的加热模式
            /// </summary>
            public FlagEnum.HeatMode HeatMode;
            /// <summary>
            /// Chiller水箱温度
            /// </summary>
            public float ChillerTemp;
            /// <summary>
            /// 水箱的温度控制范围
            /// </summary>
            public float ChillerRange;
        }

        /// <summary>
        /// 启动加热或制冷总使能
        /// </summary>
        public struct StartEnabled
        {
            /// <summary>
            /// 当前选择的产品名称
            /// </summary>
            public string VarietiesName;

            /// <summary>
            /// 区分开TestArm两侧独立控温
            /// </summary>
            public bool IndependentHeat;
            /// <summary>
            /// 左侧独立温控TestArm1.3.5.7
            /// </summary>
            public Varieties OddTestArmVarieties;
            /// <summary>
            /// 左侧独立温控TestArm2.4.6.8
            /// </summary>
            public Varieties EvenTestArmVarieties;

            /// <summary>
            /// 加热使能与控制
            /// </summary>
            public RunHeat RunHeat;

            /// <summary>
            /// 制冷使能与控制
            /// </summary>
            public RunCold RunCold;

        }

        /// <summary>
        /// 系统线程
        /// </summary>
        public struct SystemThread
        {
            /// <summary>
            /// 异常显示界面后台刷新数据线程
            /// </summary>
            public Thread AlmDisplayUpData;
            /// <summary>
            /// 异常显示界面后台刷新数据线程，后台线程使能标志
            /// </summary>
            public bool AlmDisplayEnabled;
            /// <summary>
            /// 异常显示界面后台刷新数据线程，后台线程结束标志
            /// </summary>
            public bool AlmDisplayEndState;

            /// <summary>
            /// 主界面后台刷新数据线程
            /// </summary>
            public Thread HomeUpData;
            /// <summary>
            /// 主界面后台刷新数据线程，后台线程使能标志
            /// </summary>
            public bool HomeEnabled;
            /// <summary>
            /// 主界面后台刷新数据线程，后台线程结束标志
            /// </summary>
            public bool HomeEndState;

            /// <summary>
            /// 内部制冷系统页面数据刷新线程
            /// </summary>
            public Thread InternalColdControlUpData;
            /// <summary>
            /// 内部制冷系统页面数据刷新线程，后台线程使能标志
            /// </summary>
            public bool InternalColdControlEnabled;
            /// <summary>
            /// 内部制冷系统页面数据刷新线程，后台线程结束标志
            /// </summary>
            public bool InternalColdControlEndState;

            /// <summary>
            /// 内部制冷系统页面管道刷新线程
            /// </summary>
            public Thread InternalColdStateUpData;
            /// <summary>
            /// 内部制冷系统页面管道刷新线程，后台线程使能标志
            /// </summary>
            public bool InternalColdStateEnabled;
            /// <summary>
            /// 内部制冷系统页面管道刷新线程，后台线程结束标志
            /// </summary>
            public bool InternalColdStateEndState;

            /// <summary>
            /// 外部温度控制系统页面数据刷新线程
            /// </summary>
            public Thread ExternalTempControlUpData;
            /// <summary>
            /// 外部温度控制系统页面数据刷新线程，后台线程使能标志
            /// </summary>
            public bool ExternalTempControlEnabled;
            /// <summary>
            /// 外部温度控制系统页面数据刷新线程，后台线程结束标志
            /// </summary>
            public bool ExternalTempControlEndState;

            /// <summary>
            /// 外部温度控制系统页面管道刷新线程
            /// </summary>
            public Thread ExternalTempStateUpData;
            /// <summary>
            /// 内部制冷系统页面管道刷新线程，后台线程使能标志
            /// </summary>
            public bool ExternalTempStateEnabled;
            /// <summary>
            /// 外部温度控制系统页面管道刷新线程，后台线程结束标志
            /// </summary>
            public bool ExternalTempStateEndState;

            /// <summary>
            /// 机组状态监控线程
            /// </summary>
            public Thread UnitWorkStateUpData;
            /// <summary>
            /// 机组状态监控线程，后台线程使能标志
            /// </summary>
            public bool UnitWorkStateEnabled;
            /// <summary>
            /// 机组状态监控线程，后台线程结束标志
            /// </summary>
            public bool UnitWorkStateEndState;

            /// <summary>
            /// 系统诊断页面数据刷新线程
            /// </summary>
            public Thread DiagnosisUpData;
            /// <summary>
            /// 系统诊断页面数据刷新线程，后台线程使能标志
            /// </summary>
            public bool DiagnosisEnabled;
            /// <summary>
            /// 系统诊断页面数据刷新线程，后台线程结束标志
            /// </summary>
            public bool DiagnosisEndState;

            /// <summary>
            /// 系统退出线程
            /// </summary>
            public Thread SystemExitState;

            /// <summary>
            /// 内循环泵换热制冷线程
            /// </summary>
            public Thread InsidePump;
            /// <summary>
            /// 内循环泵换热制冷线程，后台线程使能标志
            /// </summary>
            public bool InsidePumpEnabled;
        }

        /// <summary>
        /// 制冷系统参数
        /// </summary>
        public struct _CompressorSystemData
        {
            /// <summary>
            /// 最高排气温度
            /// </summary>
            public float HightExhaustTemp;
            /// <summary>
            /// 最高冷凝温度
            /// </summary>
            public float HightCondenserTemp;
            /// <summary>
            /// 最低蒸发温度
            /// </summary>
            public float LovEvaporatorTemp;
            /// <summary>
            /// 蒸发器恢复温度
            /// </summary>
            public float EvaporatorRecoveryTemp;
            /// <summary>
            /// 系统支持设定的最低温度
            /// </summary>
            public float SystemLovTemp;
            /// <summary>
            /// 最低入口压力
            /// </summary>
            public float LowInPressure;
            /// <summary>
            /// 最高入口压力
            /// </summary>
            public float HightInPressure;
            /// <summary>
            /// 最低出口压力
            /// </summary>
            public float LowOutPressure;
            /// <summary>
            /// 最高出口压力
            /// </summary>
            public float HightOutPressure;
            /// <summary>
            /// 重启间隔
            /// </summary>
            public int StartInterval;
            /// <summary>
            /// 无操作时间，自动关闭系统阀门
            /// </summary>
            public uint AutoCloseValueTime;
        }
        /// <summary>
        /// 制冷控制参数
        /// </summary>
        public struct _CompressorColdData
        {
            /// <summary>
            /// 快速降温区间
            /// </summary>
            public float RapidColding;
            /// <summary>
            /// 深度降温区间
            /// </summary>
            public float DeepColding;
            /// <summary>
            /// 排气支路降温度数
            /// </summary>
            public float ExhaustColding;
            /// <summary>
            /// 排气超限温度，将进行降温重启
            /// </summary>
            public float ExhaustOverrun;
            /// <summary>
            /// 冷凝器温度超限
            /// </summary>
            public float CondensateOverrun;
            /// <summary>
            /// 制冷单元重启时间
            /// </summary>
            public int RestartTime;
        }
        /// <summary>
        /// 内循环泵参数
        /// </summary>
        public struct _InternalPump
        {
            /// <summary>
            /// 最高转速
            /// </summary>
            public ushort HightSpeed;
            /// <summary>
            /// 单泵最高转速
            /// </summary>
            public ushort OneHightSpeed;
            /// <summary>
            /// 泵转速补偿
            /// </summary>
            public int[] PumpSpeedDiffer;
            /// <summary>
            /// 蒸发温度补偿
            /// </summary>
            public float EvaporationTempDiffer;
            /// <summary>
            /// 泵转速修正时间
            /// </summary>
            public int SpeedCorrectTime;
            /// <summary>
            /// 液体温度浮动修正转速
            /// </summary>
            public float LevelChange;
            /// <summary>
            /// 泵与转动速度
            /// </summary>
            public ushort PreRotationSpeed;
            /// <summary>
            /// 泵预降温度
            /// </summary>
            public int PreRotationTemp;
            /// <summary>
            /// 泵与转动时间
            /// </summary>
            public uint PreRotationTime;
        }

        /// <summary>
        /// 模块连接参数
        /// </summary>
        public struct _Network
        {
            /// <summary>
            /// 模块1-9的IP地址
            /// </summary>
            public string[] Modular;
            /// <summary>
            /// 连接使用的网卡
            /// </summary>
            public string NetworkCard;
            /// <summary>
            /// 连接使用网卡的IP地址
            /// </summary>
            public string NetworkIP;
            /// <summary>
            /// 连接使用网卡的子网掩码
            /// </summary>
            public string NetworkMask;
            /// <summary>
            /// 连接使用网卡的网关
            /// </summary>
            public string NetworkGateWay;
        }

        public struct _LocalNetwork
        {
            /// <summary>
            /// 本地连接参数
            /// </summary>
            public string NetworkCard;
            /// <summary>
            /// 本地联机的IP地址
            /// </summary>
            public string NetworkIP;
            /// <summary>
            /// 本地联机的子网掩码
            /// </summary>
            public string NetworkMask;
            /// <summary>
            /// 本地联机的网关
            /// </summary>
            public string NetworkGateWay;
        }
        /// <summary>
        /// 厂商参数集合
        /// </summary>
        public struct Manufacturer
        {
            /// <summary>
            /// 模块连接参数
            /// </summary>
            public _Network Network;
            /// <summary>
            /// 本地网络属性
            /// </summary>
            public _LocalNetwork LocalNetwork;
            /// <summary>
            /// 制冷系统参数
            /// </summary>
            public _CompressorSystemData CompressorSystemData;
            /// <summary>
            /// 制冷控制参数
            /// </summary>
            public _CompressorColdData CompressorColdData;
            /// <summary>
            /// 内循环泵参数
            /// </summary>
            public _InternalPump InternalPump;
        }
        public struct _DewPoint
        {
            /// <summary>
            /// 是否报警
            /// </summary>
            public bool IsConnect;
            /// <summary>
            /// 湿度传感器地址
            /// </summary>
            public int Address;
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

        /// <summary>
        /// Handler控制状态
        /// </summary>
        public struct _Handler
        {
            /// <summary>
            /// Handler通讯控制权
            /// </summary>
            public bool IsHandlerControl;

            /// <summary>
            /// 是否开启了温度控制
            /// </summary>
            public bool IsTempStartControl;

            /// <summary>
            /// 使用Chiller控制
            /// </summary>
            public bool ProhibitHandlerControl;

            /// <summary>
            /// 通讯超时，将断开与Handler的控制权
            /// </summary>
            public uint OverTime;
        }

        public struct TempOffTK
        {
            /// <summary>
            /// 测试头IC
            /// </summary>
            public float[] TestArm_IC;
            /// <summary>
            /// 测试头内部
            /// </summary>
            public float[] TestArm_Inside;
            /// <summary>
            /// 预冷盘
            /// </summary>
            public float[] ColdPlate;
            /// <summary>
            /// 预热盘
            /// </summary>
            public float[] HotPlate;
            /// <summary>
            /// 边框
            /// </summary>
            public float[] Border;
        }


        public struct _TestArm
        {
            public FlagEnum.HeatMode HeatMode;
            public _SetToHeat TestArm_IC;
            public _SetToHeat TestArm_Inside;
        }
        /// <summary>
        /// 当前产品所有的数据
        /// </summary>
        public struct VarietiesData
        {
            public _TestArm[] TestArm;
            public _SetToHeat[] ColdPlate;
            public _SetToHeat[] HotPlate;
            public _SetToHeat[] BorderTemp;
            public ushort[] PumpSpeed;
        }
    }

    public class Flag
    {
        /// <summary>
        /// 初始化所有变量数据
        /// </summary>
        public static void Initialization()
        {
            #region 压缩机组各传感器与执行器的IO地址
            Flag.Unit = new FlagStruct.Unit[4];

            Flag.Unit[0].Compressor.RunAndStop.Address = "0 - 0";         //压缩机开启地址-DO
            Flag.Unit[0].BypassValve.Address = "4 - 0";                   //旁通电磁阀开启地址-DO
            Flag.Unit[0].ParallelValve.Address = "4 - 1";                 //并通电磁阀开启地址-DO

            Flag.Unit[0].Compressor.In.Pressure.Address = "0 - 0";        //压缩机入口压力地址-AI
            Flag.Unit[0].Compressor.Out.Pressure.Address = "0 - 1";       //压缩机出口压力地址-AI

            Flag.Unit[0].Condenser.In.Pressure.Address = "0 - 2";         //冷凝器入口压力地址-AI
            Flag.Unit[0].Condenser.Out.Pressure.Address = "0 - 3";        //冷凝器出口压力地址-AI

            Flag.Unit[0].Evaporator.In.Pressure.Address = "0 - 4";        //蒸发器入口压力地址-AI
            Flag.Unit[0].Evaporator.Out.Pressure.Address = "0 - 5";       //蒸发器出口压力地址-AI

            Flag.Unit[0].HeatExchanger.Out.Pressure.Address = "0 - 6";    //换热器出口压力地址-AI

            Flag.Unit[0].Compressor.Out.Temp.Address = "4 - 0";           //压缩机出口温度地址-AI
            Flag.Unit[0].Condenser.Out.Temp.Address = "4 - 1";            //冷凝器出口温度地址-AI
            Flag.Unit[0].Evaporator.Out.Temp.Address = "4 - 2";            //蒸发器入口温度地址-AI

            Flag.Unit[1].Compressor.RunAndStop.Address = "1 - 0";        //压缩机开启地址-DO
            Flag.Unit[1].BypassValve.Address = "4 - 4";                  //旁通电磁阀开启地址-DO
            Flag.Unit[1].ParallelValve.Address = "4 - 5";                //并通电磁阀开启地址-DO

            Flag.Unit[1].Compressor.In.Pressure.Address = "1 - 0";       //压缩机入口压力地址-AI
            Flag.Unit[1].Compressor.Out.Pressure.Address = "1 - 1";      //压缩机出口压力地址-AI

            Flag.Unit[1].Condenser.In.Pressure.Address = "1 - 2";        //冷凝器入口压力地址-AI
            Flag.Unit[1].Condenser.Out.Pressure.Address = "1 - 3";       //冷凝器出口压力地址-AI

            Flag.Unit[1].Evaporator.In.Pressure.Address = "1 - 4";       //蒸发器入口压力地址-AI
            Flag.Unit[1].Evaporator.Out.Pressure.Address = "1 - 5";      //蒸发器出口压力地址-AI

            Flag.Unit[1].HeatExchanger.Out.Pressure.Address = "1 - 6";   //换热器出口压力地址-AI

            Flag.Unit[1].Compressor.Out.Temp.Address = "4 - 4";          //压缩机出口温度地址-AI
            Flag.Unit[1].Condenser.Out.Temp.Address = "4 - 5";           //冷凝器出口温度地址-AI
            Flag.Unit[1].Evaporator.Out.Temp.Address = "4 - 6";           //蒸发器入口温度地址-AI

            Flag.Unit[2].Compressor.RunAndStop.Address = "2 - 0";        //压缩机开启地址-DO
            Flag.Unit[2].BypassValve.Address = "5 - 0";                  //旁通电磁阀开启地址-DO
            Flag.Unit[2].ParallelValve.Address = "5 - 1";                //并通电磁阀开启地址-DO

            Flag.Unit[2].Compressor.In.Pressure.Address = "2 - 0";       //压缩机入口压力地址-AI
            Flag.Unit[2].Compressor.Out.Pressure.Address = "2 - 1";      //压缩机出口压力地址-AI

            Flag.Unit[2].Condenser.In.Pressure.Address = "2 - 2";        //冷凝器入口压力地址-AI
            Flag.Unit[2].Condenser.Out.Pressure.Address = "2 - 3";       //冷凝器出口压力地址-AI

            Flag.Unit[2].Evaporator.In.Pressure.Address = "2 - 4";       //蒸发器入口压力地址-AI
            Flag.Unit[2].Evaporator.Out.Pressure.Address = "2 - 5";      //蒸发器出口压力地址-AI

            Flag.Unit[2].HeatExchanger.Out.Pressure.Address = "2 - 6";   //换热器出口压力地址-AI

            Flag.Unit[2].Compressor.Out.Temp.Address = "5 - 0";          //压缩机出口温度地址-AI
            Flag.Unit[2].Condenser.Out.Temp.Address = "5 - 1";           //冷凝器出口温度地址-AI
            Flag.Unit[2].Evaporator.Out.Temp.Address = "5 - 2";           //蒸发器入口温度地址-AI

            Flag.Unit[3].Compressor.RunAndStop.Address = "3 - 0";        //压缩机开启地址-DO
            Flag.Unit[3].BypassValve.Address = "5 - 4";                  //旁通电磁阀开启地址-DO
            Flag.Unit[3].ParallelValve.Address = "5 - 5";                //并通电磁阀开启地址-DO

            Flag.Unit[3].Compressor.In.Pressure.Address = "3 - 0";        //压缩机入口压力地址-AI
            Flag.Unit[3].Compressor.Out.Pressure.Address = "3 - 1";       //压缩机出口压力地址-AI

            Flag.Unit[3].Condenser.In.Pressure.Address = "3 - 2";         //冷凝器入口压力地址-AI
            Flag.Unit[3].Condenser.Out.Pressure.Address = "3 - 3";        //冷凝器出口压力地址-AI

            Flag.Unit[3].Evaporator.In.Pressure.Address = "3 - 4";        //蒸发器入口压力地址-AI
            Flag.Unit[3].Evaporator.Out.Pressure.Address = "3 - 5";       //蒸发器出口压力地址-AI

            Flag.Unit[3].HeatExchanger.Out.Pressure.Address = "3 - 6";    //换热器出口压力地址-AI

            Flag.Unit[3].Compressor.Out.Temp.Address = "5 - 4";      //压缩机出口温度地址-AI
            Flag.Unit[3].Condenser.Out.Temp.Address = "5 - 5";       //冷凝器出口温度地址-AI
            Flag.Unit[3].Evaporator.Out.Temp.Address = "5 - 6";       //蒸发器入口温度地址-AI

            for (int i = 0; i < Unit.Length; i++)
            {
                Flag.Unit[i].InternalPump.Tacc = 1500;
                Flag.Unit[i].InternalPump.Tdec = 1500;
                Flag.Unit[i].InternalPump.EncoderTemp = 0;
                Flag.Unit[i].Compressor.RunAndStop.IntervalTime = new Stopwatch();
                Flag.Unit[i].Compressor.RunAndStop.IntervalTime.Stop();
            }

            Flag.Unit[0].InternalPump.Address = 1;
            Flag.Unit[1].InternalPump.Address = 2;
            Flag.Unit[2].InternalPump.Address = 3;
            Flag.Unit[3].InternalPump.Address = 4;

            #endregion

            #region 水箱的温度液位阀等IO地址

            Flag.WaterBox = new FlagStruct.WaterBox();

            Flag.WaterBox.Height.Address = "6 - 0";              // 液位状态地址
            Flag.WaterBox.Temp.Address = "6 - 1";                // 液体温度状态地址
            Flag.WaterBox.InternalValve.Address = "6 - 0";      // 内循环出液口电磁阀输出地址
            Flag.WaterBox.ExternalValve.Address = "6 - 1";      // 外循环出液口电磁阀输出地址

            Flag.WaterBox.Offset.TempBasicPoint = new float[3];
            Flag.WaterBox.Offset.TempOffset = new float[4];
            Flag.WaterBox.Offset.TempOffsetPoint = -1;

            #endregion

            #region 泵的初始化以及加减速时间设定

            Flag.ExternalPump = new FlagStruct.Pump[3];

            for (int i = 0; i < ExternalPump.Length; i++)
            {
                Flag.ExternalPump[i].Tacc = 1500;
                Flag.ExternalPump[i].Tdec = 1500;
                Flag.ExternalPump[i].EncoderTemp = 0;
            }
            Flag.ExternalPump[0].Address = 5;
            Flag.ExternalPump[1].Address = 6;
            Flag.ExternalPump[2].Address = 7;

            #endregion

            #region PMA温控与Handler侧电磁阀初始化以及地址分配
            Flag.ExternalTestTempChange = new FlagStruct.ExternalTestTempChange();
            Flag.OutPumpSpeed = new ushort[3];

            #region TestArm

            Flag.TestArm = new FlagStruct.TestArmHeatingChannel[8];

            Flag.TestArm[0].Heat_Inside.Address = "1 - 8";
            Flag.TestArm[1].Heat_Inside.Address = "1 - 12";
            Flag.TestArm[2].Heat_Inside.Address = "1 - 9";
            Flag.TestArm[3].Heat_Inside.Address = "1 - 13";
            Flag.TestArm[4].Heat_Inside.Address = "1 - 10";
            Flag.TestArm[5].Heat_Inside.Address = "1 - 14";
            Flag.TestArm[6].Heat_Inside.Address = "1 - 11";
            Flag.TestArm[7].Heat_Inside.Address = "1 - 15";

            Flag.TestArm[0].Heat_IC.Address = "1 - 0";
            Flag.TestArm[1].Heat_IC.Address = "1 - 4";
            Flag.TestArm[2].Heat_IC.Address = "1 - 1";
            Flag.TestArm[3].Heat_IC.Address = "1 - 5";
            Flag.TestArm[4].Heat_IC.Address = "1 - 2";
            Flag.TestArm[5].Heat_IC.Address = "1 - 6";
            Flag.TestArm[6].Heat_IC.Address = "1 - 3";
            Flag.TestArm[7].Heat_IC.Address = "1 - 7";

            Flag.TestArm[0].Heat_Inside.Name = "Test Arm 1 Inside";
            Flag.TestArm[1].Heat_Inside.Name = "Test Arm 2 Inside";
            Flag.TestArm[2].Heat_Inside.Name = "Test Arm 3 Inside";
            Flag.TestArm[3].Heat_Inside.Name = "Test Arm 4 Inside";
            Flag.TestArm[4].Heat_Inside.Name = "Test Arm 5 Inside";
            Flag.TestArm[5].Heat_Inside.Name = "Test Arm 6 Inside";
            Flag.TestArm[6].Heat_Inside.Name = "Test Arm 7 Inside";
            Flag.TestArm[7].Heat_Inside.Name = "Test Arm 8 Inside";

            Flag.TestArm[0].Heat_IC.Name = "Test Arm 1 IC";
            Flag.TestArm[1].Heat_IC.Name = "Test Arm 2 IC";
            Flag.TestArm[2].Heat_IC.Name = "Test Arm 3 IC";
            Flag.TestArm[3].Heat_IC.Name = "Test Arm 4 IC";
            Flag.TestArm[4].Heat_IC.Name = "Test Arm 5 IC";
            Flag.TestArm[5].Heat_IC.Name = "Test Arm 6 IC";
            Flag.TestArm[6].Heat_IC.Name = "Test Arm 7 IC";
            Flag.TestArm[7].Heat_IC.Name = "Test Arm 8 IC";

            Flag.TestArm[0].Valve.Address = "8 - 0";        //TestArm1
            Flag.TestArm[1].Valve.Address = "8 - 1";        //TestArm2
            Flag.TestArm[2].Valve.Address = "8 - 2";        //TestArm3
            Flag.TestArm[3].Valve.Address = "8 - 3";        //TestArm4
            Flag.TestArm[4].Valve.Address = "8 - 4";        //TestArm5
            Flag.TestArm[5].Valve.Address = "8 - 5";        //TestArm6
            Flag.TestArm[6].Valve.Address = "8 - 6";        //TestArm7
            Flag.TestArm[7].Valve.Address = "8 - 7";        //TestArm8

            #endregion

            #region ColdPlate

            Flag.ColdPlate = new FlagStruct.ColdPlateHeatingChannel[2];

            Flag.ColdPlate[0].Heat.Address = "1 - 16";
            Flag.ColdPlate[1].Heat.Address = "1 - 17";

            Flag.ColdPlate[0].Heat.Name = "Cold Plate 1";
            Flag.ColdPlate[1].Heat.Name = "Cold Plate 2";

            Flag.ColdPlate[0].Valve.Address = "8 - 8";
            Flag.ColdPlate[1].Valve.Address = "8 - 9";

            #endregion

            #region HotPlate

            Flag.HotPlate = new FlagStruct.HeatingChannel[2];

            Flag.HotPlate[0].Heat.Address = "1 - 18";
            Flag.HotPlate[1].Heat.Address = "1 - 19";

            Flag.HotPlate[0].Heat.Name = "Hot Plate1";
            Flag.HotPlate[1].Heat.Name = "Hot Plate2";
            #endregion

            #region BorderTemp

            Flag.BorderTemp = new FlagStruct.HeatingChannel[18];

            Flag.BorderTemp[0].Heat.Address = "2 - 0";       //Test Arm 1 Shield
            Flag.BorderTemp[1].Heat.Address = "2 - 8";       //Test Arm 2 Shield
            Flag.BorderTemp[2].Heat.Address = "2 - 2";       //Test Arm 3 Shield
            Flag.BorderTemp[3].Heat.Address = "2 - 10";       //Test Arm 4 Shield
            Flag.BorderTemp[4].Heat.Address = "2 - 4";       //Test Arm 5 Shield
            Flag.BorderTemp[5].Heat.Address = "2 - 12";       //Test Arm 6 Shield
            Flag.BorderTemp[6].Heat.Address = "2 - 6";       //Test Arm 7 Shield
            Flag.BorderTemp[7].Heat.Address = "2 - 14";       //Test Arm 8 Shield
            Flag.BorderTemp[8].Heat.Address = "1 - 20";       //Cold Plate 1 Shield
            Flag.BorderTemp[9].Heat.Address = "1 - 21";       //Cold Plate 2 Shield
            Flag.BorderTemp[10].Heat.Address = "2 - 16";       //Socket 1 Shield
            Flag.BorderTemp[11].Heat.Address = "2 - 20";       //Socket 2 Shield
            Flag.BorderTemp[12].Heat.Address = "2 - 17";       //Socket 3 Shield
            Flag.BorderTemp[13].Heat.Address = "2 - 21";       //Socket 4 Shield
            Flag.BorderTemp[14].Heat.Address = "2 - 18";       //Socket 5 Shield
            Flag.BorderTemp[15].Heat.Address = "2 - 22";       //Socket 6 Shield
            Flag.BorderTemp[16].Heat.Address = "2 - 19";       //Socket 7 Shield
            Flag.BorderTemp[17].Heat.Address = "2 - 23";       //Socket 8 Shield

            Flag.BorderTemp[0].Heat.Name = "Test Arm 1 Shield";
            Flag.BorderTemp[1].Heat.Name = "Test Arm 2 Shield";
            Flag.BorderTemp[2].Heat.Name = "Test Arm 3 Shield";
            Flag.BorderTemp[3].Heat.Name = "Test Arm 4 Shield";
            Flag.BorderTemp[4].Heat.Name = "Test Arm 5 Shield";
            Flag.BorderTemp[5].Heat.Name = "Test Arm 6 Shield";
            Flag.BorderTemp[6].Heat.Name = "Test Arm 7 Shield";
            Flag.BorderTemp[7].Heat.Name = "Test Arm 8 Shield";
            Flag.BorderTemp[8].Heat.Name = "Cold Plate 1 Shield";
            Flag.BorderTemp[9].Heat.Name = "Cold Plate 2 Shield";
            Flag.BorderTemp[10].Heat.Name = "Socket 1 Shield";
            Flag.BorderTemp[11].Heat.Name = "Socket 2 Shield";
            Flag.BorderTemp[12].Heat.Name = "Socket 3 Shield";
            Flag.BorderTemp[13].Heat.Name = "Socket 4 Shield";
            Flag.BorderTemp[14].Heat.Name = "Socket 5 Shield";
            Flag.BorderTemp[15].Heat.Name = "Socket 6 Shield";
            Flag.BorderTemp[16].Heat.Name = "Socket 7 Shield";
            Flag.BorderTemp[17].Heat.Name = "Socket 8 Shield";
            #endregion

            #region 连接板温度

            Flag.OtherTemp = new FlagStruct.HeatingChannel[8];

            Flag.OtherTemp[0].Heat.Address = "2 - 1";
            Flag.OtherTemp[1].Heat.Address = "2 - 9";
            Flag.OtherTemp[2].Heat.Address = "2 - 3";
            Flag.OtherTemp[3].Heat.Address = "2 - 11";
            Flag.OtherTemp[4].Heat.Address = "2 - 5";
            Flag.OtherTemp[5].Heat.Address = "2 - 13";
            Flag.OtherTemp[6].Heat.Address = "2 - 7";
            Flag.OtherTemp[7].Heat.Address = "2 - 15";

            #endregion

            #endregion

            #region 湿度传感器初始化以及地址分配

            Flag.DewPoint = new FlagStruct._DewPoint[8];
            for (int i = 0; i < Flag.DewPoint.Length; i++)
            {
                Flag.DewPoint[i].Address = i + 1;
            }

            #endregion

            #region 设定数据记录的长度，默认为10小时
            int Lenght = 10 * 60 * 60;
            Flag.WaterBox.Height.RecordLength = Lenght;
            Flag.WaterBox.Temp.RecordLength = Lenght;
            Flag.WaterBox.Parameter.RecordLength = Lenght;

            for (int i = 0; i < Unit.Length; i++)
            {
                Flag.Unit[i].Compressor.In.Pressure.RecordLength = Lenght;
                Flag.Unit[i].Compressor.Out.Pressure.RecordLength = Lenght;

                Flag.Unit[i].Condenser.In.Pressure.RecordLength = Lenght;
                Flag.Unit[i].Condenser.Out.Pressure.RecordLength = Lenght;

                Flag.Unit[i].Evaporator.In.Pressure.RecordLength = Lenght;
                Flag.Unit[i].Evaporator.Out.Pressure.RecordLength = Lenght;

                Flag.Unit[i].HeatExchanger.Out.Pressure.RecordLength = Lenght;

                Flag.Unit[i].Compressor.Out.Temp.RecordLength = Lenght;
                Flag.Unit[i].Condenser.Out.Temp.RecordLength = Lenght;
                Flag.Unit[i].Evaporator.Out.Temp.RecordLength = Lenght;
            }

            for (int i = 0; i < Flag.TestArm.Length; i++)
            {
                Flag.TestArm[i].Heat_Inside.NowRecord.Length = Lenght;
                Flag.TestArm[i].Heat_Inside.SetRecord.Length = Lenght;

                Flag.TestArm[i].Heat_IC.NowRecord.Length = Lenght;
                Flag.TestArm[i].Heat_IC.SetRecord.Length = Lenght;
            }

            for (int i = 0; i < Flag.ColdPlate.Length; i++)
            {
                Flag.ColdPlate[i].Heat.NowRecord.Length = Lenght;
                Flag.ColdPlate[i].Heat.SetRecord.Length = Lenght;
            }

            for (int i = 0; i < Flag.HotPlate.Length; i++)
            {
                Flag.HotPlate[i].Heat.NowRecord.Length = Lenght;
                Flag.HotPlate[i].Heat.SetRecord.Length = Lenght;
            }

            for (int i = 0; i < Flag.BorderTemp.Length; i++)
            {
                Flag.BorderTemp[i].Heat.NowRecord.Length = Lenght;
                Flag.BorderTemp[i].Heat.SetRecord.Length = Lenght;
            }
            for (int i = 0; i < Flag.OtherTemp.Length; i++)
            {
                Flag.OtherTemp[i].Heat.NowRecord.Length = Lenght;
                Flag.OtherTemp[i].Heat.SetRecord.Length = Lenght;
            }
            #endregion

            #region 初始化设定页面数据

            #region 内部制冷控制页面数据

            Flag.InternalColdControl.ChartHightTemp = 40;
            Flag.InternalColdControl.ChartLovTemp = -100;
            Flag.InternalColdControl.ChartPointLength = 900;
            Flag.InternalColdControl.ChartRefreshTime = 2;

            Flag.InternalColdControl.UnitChart = new FlagStruct.ChartSetUp[Unit.Length];

            Flag.InternalColdControl.UnitChart[0].Color = Color.Olive;
            Flag.InternalColdControl.UnitChart[1].Color = Color.Green;
            Flag.InternalColdControl.UnitChart[2].Color = Color.Turquoise;
            Flag.InternalColdControl.UnitChart[3].Color = Color.Blue;
            Flag.InternalColdControl.SetTempChart.Color = Color.DarkViolet;
            Flag.InternalColdControl.NowTempChart.Color = Color.Red;

            Flag.InternalColdControl.UnitChart[0].Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.InternalColdControl.UnitChart[1].Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.InternalColdControl.UnitChart[2].Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.InternalColdControl.UnitChart[3].Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.InternalColdControl.SetTempChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.InternalColdControl.NowTempChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            Flag.InternalColdControl.ChartRefreshEnabled = true;

            #endregion

            #region 外部温度控制页面数据
            Flag.ExternalTestTempChange.HeatMode = FlagEnum.HeatMode.Hot;
            Flag.ExternalTestTempChange.TestTemp = -25;
            Flag.ExternalTestTempChange.PreCoolTemp = 5;
            Flag.ExternalTestTempChange.ResultTemp = 40;
            Flag.ExternalTestTempChange.TestRange = 3;
            Flag.ExternalTestTempChange.TestTempOffsetPoint = 1;
            Flag.ExternalTestTempChange.TestTempBasicPoint = new double[3];
            Flag.ExternalTestTempChange.NoEnableDisplayTemp = false;
            Flag.ExternalTestTempChange.TestArmEnabled = new bool[8];
            Flag.ExternalTestTempChange.ColdPlateEnabled = new bool[2];
            Flag.ExternalTestTempChange.HotPlateEnabled = new bool[2];
            Flag.ExternalTestTempChange.BorderEnabled = new bool[18];
            Flag.ExternalTestTempChange.TestArmUseIcHeat = new bool[8];
            Flag.ExternalTestTempChange.IndependentUseHeat = false;

            Flag.ExternalTestTempChange.BorderTemp = 50;
            Flag.ExternalTestTempChange.BorderTempOffsetPoint = 1;
            Flag.ExternalTestTempChange.BorderTempBasicPoint = new double[3];

            Flag.ExternalTempControlChart.ChartHightTemp = 150;
            Flag.ExternalTempControlChart.ChartLovTemp = -70;
            Flag.ExternalTempControlChart.ChartPointLength = 900;
            Flag.ExternalTempControlChart.ChartRefreshTime = 2;



            Flag.ExternalTempControlChart.TempChart = new FlagStruct.ChartSetUp[Flag.TestArm.Length + Flag.ColdPlate.Length + Flag.HotPlate.Length + 1];

            for (int i = 0; i < Flag.ExternalTempControlChart.TempChart.Length; i++)
            {
                Flag.ExternalTempControlChart.TempChart[i].Color = Color.Black;
                Flag.ExternalTempControlChart.TempChart[i].Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }

            Flag.ExternalTempControlChart.ChartRefreshEnabled = true;

            #endregion

            #region 机组工作状态页面数据
            Flag.UnitWorkState.UnitIndex = 0;
            Flag.UnitWorkState.ChartHightPressure = 3;
            Flag.UnitWorkState.ChartLovPressure = 0;
            Flag.UnitWorkState.ChartHightTemp = 150;
            Flag.UnitWorkState.ChartLovTemp = -70;
            Flag.UnitWorkState.ChartPointLength = 900;
            Flag.UnitWorkState.ChartRefreshTime = 2;

            Flag.UnitWorkState.CompressorInPressureChart.Color = Color.Red;
            Flag.UnitWorkState.CompressorOutPressureChart.Color = Color.Orange;
            Flag.UnitWorkState.CondenserInPressureChart.Color = Color.Gold;
            Flag.UnitWorkState.CondenserOutPressureChart.Color = Color.Green;
            Flag.UnitWorkState.EvaporatorInPressureChart.Color = Color.Cyan;
            Flag.UnitWorkState.EvaporatorOutPressureChart.Color = Color.Blue;
            Flag.UnitWorkState.HeatExchangerOutPressureChart.Color = Color.Purple;
            Flag.UnitWorkState.CompressorOutTempChart.Color = Color.Pink;
            Flag.UnitWorkState.CondenserOutTempChart.Color = Color.Brown;
            Flag.UnitWorkState.EvaporatorInTempChart.Color = Color.Fuchsia;

            Flag.UnitWorkState.CompressorInPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.CompressorOutPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.CondenserInPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.CondenserOutPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.EvaporatorInPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.EvaporatorOutPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.HeatExchangerOutPressureChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.CompressorOutTempChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.CondenserOutTempChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            Flag.UnitWorkState.EvaporatorInTempChart.Type = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            Flag.UnitWorkState.ChartRefreshEnabled = true;

            #endregion

            #region 用户诊断状态页面数据
            Flag.Diagnosis.InternalPumpSpeed1 = 100;
            Flag.Diagnosis.InternalPumpSpeed2 = 100;
            Flag.Diagnosis.InternalPumpSpeed3 = 100;
            Flag.Diagnosis.InternalPumpSpeed4 = 100;

            Flag.Diagnosis.ExternalPumpSpeed1 = 100;
            Flag.Diagnosis.ExternalPumpSpeed2 = 100;
            Flag.Diagnosis.ExternalPumpSpeed3 = 100;
            #endregion

            #region  厂商参数状态页面数据

            Flag.Manufacturer.Network = new FlagStruct._Network();
            Flag.Manufacturer.Network.Modular = new string[9];

            for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
            {
                Flag.Manufacturer.Network.Modular[i] = "192.168.1." + (200 + i).ToString();
            }
            Flag.Manufacturer.InternalPump = new FlagStruct._InternalPump();
            Flag.Manufacturer.CompressorColdData = new FlagStruct._CompressorColdData();
            Flag.Manufacturer.CompressorSystemData = new FlagStruct._CompressorSystemData();
            Flag.Manufacturer.InternalPump.PumpSpeedDiffer = new int[4];
            #endregion

            #endregion

            #region 初始化用户数据组相关数据
            Flag.LogOnUser.AutoLogoutTime = 0;
            Flag.LogOnUser.FreeTime = new SystemFreeTime();
            Flag.LogOnUser.IsAutoLogout = false;
            Flag.LogOnUser.UserAuthority = FlagEnum.UserAuthority.None;
            Flag.LogOnUser.UserName = "";
            #endregion

            #region 加热与制冷变量初始化

            StartEnabled = new FlagStruct.StartEnabled();

            StartEnabled.RunHeat.Heating = false;

            StartEnabled.VarietiesName = "";
            StartEnabled.OddTestArmVarieties = new FlagStruct.Varieties();
            StartEnabled.EvenTestArmVarieties = new FlagStruct.Varieties();
            StartEnabled.OddTestArmVarieties.Name = "";
            StartEnabled.EvenTestArmVarieties.Name = "";

            StartEnabled.OddTestArmVarieties.HeatMode = FlagEnum.HeatMode.Hot;
            StartEnabled.EvenTestArmVarieties.HeatMode = FlagEnum.HeatMode.Hot;

            #endregion

            #region Handler 控制中断
            Flag.Handler = new FlagStruct._Handler();
            Flag.Handler.IsHandlerControl = false;
            #endregion

            #region 初始化TempOffTK参数

            Flag.TempOffTK = new FlagStruct.TempOffTK();

            Flag.TempOffTK.TestArm_IC = new float[Flag.TestArm.Length];
            Flag.TempOffTK.TestArm_Inside = new float[Flag.TestArm.Length];
            Flag.TempOffTK.ColdPlate = new float[Flag.ColdPlate.Length];
            Flag.TempOffTK.HotPlate = new float[Flag.HotPlate.Length];
            Flag.TempOffTK.Border = new float[Flag.BorderTemp.Length];

            #endregion

            #region 初始化产品数据
            Flag.VarietiesData = new FlagStruct.VarietiesData();

            Flag.VarietiesData.TestArm = new  FlagStruct._TestArm[Flag.TestArm.Length];
            Flag.VarietiesData.ColdPlate = new FlagStruct._SetToHeat[Flag.ColdPlate.Length];
            Flag.VarietiesData.HotPlate = new FlagStruct._SetToHeat[Flag.HotPlate.Length];
            Flag.VarietiesData.BorderTemp = new FlagStruct._SetToHeat[Flag.BorderTemp.Length];
            Flag.VarietiesData.PumpSpeed = new ushort[3];
            #endregion
        }
        /// <summary>
        /// 加载所有变量数据
        /// </summary>
        public static void LoadData()
        {
            string ReadOutData;

            #region 读取品种名称

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "VarietiesName");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.StartEnabled.VarietiesName, ReadOutData) == true, ReadOutData, "", ref Flag.StartEnabled.VarietiesName);

            #endregion

            #region 内部制冷水箱温度以及温度偏移设定等读取

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Parameter", "SetTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.WaterBox.Parameter.SetTemp, ReadOutData) == true, ReadOutData, -60, ref Flag.WaterBox.Parameter.SetTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Parameter", "SetRange");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.WaterBox.Parameter.SetRange, ReadOutData) == true, ReadOutData, 3, ref Flag.WaterBox.Parameter.SetRange);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempOffsetPoint");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.WaterBox.Offset.TempOffsetPoint, ReadOutData) == true, ReadOutData, 1, ref Flag.WaterBox.Offset.TempOffsetPoint);

            for (int i = 0; i < Flag.WaterBox.Offset.TempBasicPoint.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempBasicPoint" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.WaterBox.Offset.TempBasicPoint[i], ReadOutData) == true, ReadOutData, 0, ref Flag.WaterBox.Offset.TempBasicPoint[i]);
            }

            for (int i = 0; i < Flag.WaterBox.Offset.TempOffset.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempOffset" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.WaterBox.Offset.TempOffset[i], ReadOutData) == true, ReadOutData, 0, ref Flag.WaterBox.Offset.TempOffset[i]);
            }

            #endregion

            #region 内部工作制冷曲线参数读取

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartHightTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.ChartHightTemp, ReadOutData) == true, ReadOutData, 40, ref Flag.InternalColdControl.ChartHightTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartLovTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.ChartLovTemp, ReadOutData) == true, ReadOutData, -100, ref Flag.InternalColdControl.ChartLovTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartPointLength");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.ChartPointLength, ReadOutData) == true, ReadOutData, 900, ref Flag.InternalColdControl.ChartPointLength);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartRefreshTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.ChartRefreshTime, ReadOutData) == true, ReadOutData, 3, ref Flag.InternalColdControl.ChartRefreshTime);

            for (int i = 0; i < Flag.InternalColdControl.UnitChart.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "UnitChart" + i.ToString(), "Color");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.UnitChart[i].Color, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.UnitChart[i].Color, ref Flag.InternalColdControl.UnitChart[i].Color);
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "UnitChart" + i.ToString(), "Type");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.UnitChart[i].Type, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.UnitChart[i].Type, ref Flag.InternalColdControl.UnitChart[i].Type);
            }
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "SetTempChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.SetTempChart.Color, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.SetTempChart.Color, ref Flag.InternalColdControl.SetTempChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "SetTempChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.SetTempChart.Type, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.SetTempChart.Type, ref Flag.InternalColdControl.SetTempChart.Type);


            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "NowTempChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.NowTempChart.Color, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.NowTempChart.Color, ref Flag.InternalColdControl.NowTempChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "NowTempChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.InternalColdControl.NowTempChart.Type, ReadOutData) == true, ReadOutData, Flag.InternalColdControl.NowTempChart.Type, ref Flag.InternalColdControl.NowTempChart.Type);

            #endregion

            #region 外部温度通用参数

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "DisplayAllChannel");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.DisplayAllChannel, ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.DisplayAllChannel);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "NoEnableDisplayTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.NoEnableDisplayTemp, ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.NoEnableDisplayTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "Independent");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.IndependentUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.IndependentUseHeat);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "UseIcHeat");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.UseIcHeat, ReadOutData) == true, ReadOutData, false, ref ExternalTestTempChange.UseIcHeat);


            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "TestArmUseIcHeat" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.TestArmUseIcHeat[i], ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.TestArmUseIcHeat[i]);
            }


            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmEnabled.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "TestArmEnabled" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.TestArmEnabled[i], ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.TestArmEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.ColdPlateEnabled.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "ColdPlateEnabled" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.ColdPlateEnabled[i], ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.ColdPlateEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.HotPlateEnabled.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "HotPlateEnabled" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.HotPlateEnabled[i], ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.HotPlateEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.BorderEnabled.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "BorderEnabled" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.BorderEnabled[i], ReadOutData) == true, ReadOutData, false, ref Flag.ExternalTestTempChange.BorderEnabled[i]);
            }

            #endregion

            #region 加热通道参数

            if (Flag.StartEnabled.IndependentHeat == false)
            {
                ChangeTempVarietiesName(Flag.StartEnabled.VarietiesName);
            }
            else
            {
                ChangeTempVarietiesName(Flag.StartEnabled.OddTestArmVarieties.Name, Flag.StartEnabled.EvenTestArmVarieties.Name);
            }

            #endregion

            #region 外部工作温度曲线参数读取

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartHightTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.ChartHightTemp, ReadOutData) == true, ReadOutData, 150, ref Flag.ExternalTempControlChart.ChartHightTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartLovTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.ChartLovTemp, ReadOutData) == true, ReadOutData, -70, ref Flag.ExternalTempControlChart.ChartLovTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartPointLength");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.ChartPointLength, ReadOutData) == true, ReadOutData, 900, ref Flag.ExternalTempControlChart.ChartPointLength);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartRefreshTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.ChartRefreshTime, ReadOutData) == true, ReadOutData, 3, ref Flag.ExternalTempControlChart.ChartRefreshTime);

            for (int i = 0; i < Flag.ExternalTempControlChart.TempChart.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "TempChart" + i.ToString(), "Color");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.TempChart[i].Color, ReadOutData) == true, ReadOutData, Flag.ExternalTempControlChart.TempChart[i].Color, ref Flag.ExternalTempControlChart.TempChart[i].Color);
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "TempChart" + i.ToString(), "Type");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTempControlChart.TempChart[i].Type, ReadOutData) == true, ReadOutData, Flag.ExternalTempControlChart.TempChart[i].Type, ref Flag.ExternalTempControlChart.TempChart[i].Type);
            }

            #endregion

            #region 机组工作状态曲线参数读取

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartHightPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartHightPressure, ReadOutData) == true, ReadOutData, 3, ref Flag.UnitWorkState.ChartHightPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartHightTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartHightTemp, ReadOutData) == true, ReadOutData, 150, ref Flag.UnitWorkState.ChartHightTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartLovPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartLovPressure, ReadOutData) == true, ReadOutData, 0, ref Flag.UnitWorkState.ChartLovPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartLovTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartLovTemp, ReadOutData) == true, ReadOutData, -70, ref Flag.UnitWorkState.ChartLovTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartPointLength");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartPointLength, ReadOutData) == true, ReadOutData, 900, ref Flag.UnitWorkState.ChartPointLength);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartRefreshTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.ChartRefreshTime, ReadOutData) == true, ReadOutData, 3, ref Flag.UnitWorkState.ChartRefreshTime);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorInPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorInPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorInPressureChart.Color, ref Flag.UnitWorkState.CompressorInPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorInPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorInPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorInPressureChart.Type, ref Flag.UnitWorkState.CompressorInPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorOutPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorOutPressureChart.Color, ref Flag.UnitWorkState.CompressorOutPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorOutPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorOutPressureChart.Type, ref Flag.UnitWorkState.CompressorOutPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutTempChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorOutTempChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorOutTempChart.Color, ref Flag.UnitWorkState.CompressorOutTempChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutTempChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CompressorOutTempChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CompressorOutTempChart.Type, ref Flag.UnitWorkState.CompressorOutTempChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserInPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserInPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserInPressureChart.Color, ref Flag.UnitWorkState.CondenserInPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserInPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserInPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserInPressureChart.Type, ref Flag.UnitWorkState.CondenserInPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserOutPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserOutPressureChart.Color, ref Flag.UnitWorkState.CondenserOutPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserOutPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserOutPressureChart.Type, ref Flag.UnitWorkState.CondenserOutPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutTempChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserOutTempChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserOutTempChart.Color, ref Flag.UnitWorkState.CondenserOutTempChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutTempChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.CondenserOutTempChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.CondenserOutTempChart.Type, ref Flag.UnitWorkState.CondenserOutTempChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorInPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorInPressureChart.Color, ref Flag.UnitWorkState.EvaporatorInPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorInPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorInPressureChart.Type, ref Flag.UnitWorkState.EvaporatorInPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInTempChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorInTempChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorInTempChart.Color, ref Flag.UnitWorkState.EvaporatorInTempChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInTempChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorInTempChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorInTempChart.Type, ref Flag.UnitWorkState.EvaporatorInTempChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorOutPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorOutPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorOutPressureChart.Color, ref Flag.UnitWorkState.EvaporatorOutPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorOutPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.EvaporatorOutPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.EvaporatorOutPressureChart.Type, ref Flag.UnitWorkState.EvaporatorOutPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "HeatExchangerOutPressureChart", "Color");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.HeatExchangerOutPressureChart.Color, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.HeatExchangerOutPressureChart.Color, ref Flag.UnitWorkState.HeatExchangerOutPressureChart.Color);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "HeatExchangerOutPressureChart", "Type");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.HeatExchangerOutPressureChart.Type, ReadOutData) == true, ReadOutData, Flag.UnitWorkState.HeatExchangerOutPressureChart.Type, ref Flag.UnitWorkState.HeatExchangerOutPressureChart.Type);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "UnitIndex");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.UnitWorkState.UnitIndex, ReadOutData) == true, ReadOutData, 0, ref Flag.UnitWorkState.UnitIndex);

            #endregion

            #region 机组使能参数读取

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "Unit" + i.ToString(), "Enabled");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.Unit[i].Enabled, ReadOutData) == true, ReadOutData, false, ref Flag.Unit[i].Enabled);
            }

            #endregion

            #region 诊断页面泵手动速度读取
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed1");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.InternalPumpSpeed1, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.InternalPumpSpeed1);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed2");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.InternalPumpSpeed2, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.InternalPumpSpeed2);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed3");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.InternalPumpSpeed3, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.InternalPumpSpeed3);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed4");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.InternalPumpSpeed4, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.InternalPumpSpeed4);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed1");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.ExternalPumpSpeed1, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.ExternalPumpSpeed1);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed2");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.ExternalPumpSpeed2, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.ExternalPumpSpeed2);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed3");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Diagnosis.ExternalPumpSpeed3, ReadOutData) == true, ReadOutData, 100, ref Flag.Diagnosis.ExternalPumpSpeed3);

            #endregion

            #region 厂商参数读取

            for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "Modular" + i.ToString());
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.Network.Modular[i], ReadOutData) == true, ReadOutData, Flag.Manufacturer.Network.Modular[i], ref Flag.Manufacturer.Network.Modular[i]);
            }

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkCard");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.Network.NetworkCard, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.Network.NetworkCard);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkGateWay");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.Network.NetworkGateWay, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.Network.NetworkGateWay);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkIP");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.Network.NetworkIP, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.Network.NetworkIP);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkMask");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.Network.NetworkMask, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.Network.NetworkMask);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "ExhaustColding");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.ExhaustColding, ReadOutData) == true, ReadOutData, 100, ref Flag.Manufacturer.CompressorColdData.ExhaustColding);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "DeepColding");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.DeepColding, ReadOutData) == true, ReadOutData, -60, ref Flag.Manufacturer.CompressorColdData.DeepColding);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "ExhaustOverrun");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.ExhaustOverrun, ReadOutData) == true, ReadOutData, 105, ref Flag.Manufacturer.CompressorColdData.ExhaustOverrun);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "RapidColding");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.RapidColding, ReadOutData) == true, ReadOutData, -35, ref Flag.Manufacturer.CompressorColdData.RapidColding);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "CondensateOverrun");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.CondensateOverrun, ReadOutData) == true, ReadOutData, 600, ref Flag.Manufacturer.CompressorColdData.CondensateOverrun);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "RestartTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorColdData.RestartTime, ReadOutData) == true, ReadOutData, 600, ref Flag.Manufacturer.CompressorColdData.RestartTime);


            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightCondenserTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.HightCondenserTemp, ReadOutData) == true, ReadOutData, 70, ref Flag.Manufacturer.CompressorSystemData.HightCondenserTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LovEvaporatorTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp, ReadOutData) == true, ReadOutData, -85, ref Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "EvaporatorRecoveryTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp, ReadOutData) == true, ReadOutData, -60, ref Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp);


            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightExhaustTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.HightExhaustTemp, ReadOutData) == true, ReadOutData, 110, ref Flag.Manufacturer.CompressorSystemData.HightExhaustTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "SystemLovTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.SystemLovTemp, ReadOutData) == true, ReadOutData, -85, ref Flag.Manufacturer.CompressorSystemData.SystemLovTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightInPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.HightInPressure, ReadOutData) == true, ReadOutData, 0.6f, ref Flag.Manufacturer.CompressorSystemData.HightInPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightOutPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.HightOutPressure, ReadOutData) == true, ReadOutData, 2.8f, ref Flag.Manufacturer.CompressorSystemData.HightOutPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LowInPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.LowInPressure, ReadOutData) == true, ReadOutData, 0.35f, ref Flag.Manufacturer.CompressorSystemData.LowInPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LowOutPressure");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.LowOutPressure, ReadOutData) == true, ReadOutData, 1.8f, ref Flag.Manufacturer.CompressorSystemData.LowOutPressure);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "StartInterval");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.StartInterval, ReadOutData) == true, ReadOutData, 600, ref Flag.Manufacturer.CompressorSystemData.StartInterval);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "EvaporationTempDiffer");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.EvaporationTempDiffer, ReadOutData) == true, ReadOutData, 10, ref Flag.Manufacturer.InternalPump.EvaporationTempDiffer);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "HightSpeed");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.HightSpeed, ReadOutData) == true, ReadOutData, 3000, ref Flag.Manufacturer.InternalPump.HightSpeed);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "OneHightSpeed");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.OneHightSpeed, ReadOutData) == true, ReadOutData, 1200, ref Flag.Manufacturer.InternalPump.OneHightSpeed);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "LevelChange");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.LevelChange, ReadOutData) == true, ReadOutData, 0.2f, ref Flag.Manufacturer.InternalPump.LevelChange);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer1");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PumpSpeedDiffer[0], ReadOutData) == true, ReadOutData, 0, ref Flag.Manufacturer.InternalPump.PumpSpeedDiffer[0]);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer2");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PumpSpeedDiffer[1], ReadOutData) == true, ReadOutData, 0, ref Flag.Manufacturer.InternalPump.PumpSpeedDiffer[1]);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer3");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PumpSpeedDiffer[2], ReadOutData) == true, ReadOutData, 0, ref Flag.Manufacturer.InternalPump.PumpSpeedDiffer[2]);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer4");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PumpSpeedDiffer[3], ReadOutData) == true, ReadOutData, 0, ref Flag.Manufacturer.InternalPump.PumpSpeedDiffer[3]);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "SpeedCorrectTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.SpeedCorrectTime, ReadOutData) == true, ReadOutData, 15, ref Flag.Manufacturer.InternalPump.SpeedCorrectTime);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationSpeed");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PreRotationSpeed, ReadOutData) == true, ReadOutData, 500, ref Flag.Manufacturer.InternalPump.PreRotationSpeed);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationTemp");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PreRotationTemp, ReadOutData) == true, ReadOutData, 0, ref Flag.Manufacturer.InternalPump.PreRotationTemp);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.InternalPump.PreRotationTime, ReadOutData) == true, ReadOutData, 30, ref Flag.Manufacturer.InternalPump.PreRotationTime);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "AutoCloseValueTime");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime, ReadOutData) == true, ReadOutData, 10, ref Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkCard");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.LocalNetwork.NetworkCard, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.LocalNetwork.NetworkCard);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkIP");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.LocalNetwork.NetworkIP, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.LocalNetwork.NetworkIP);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkMask");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.LocalNetwork.NetworkMask, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.LocalNetwork.NetworkMask);
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkGateWay");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.Manufacturer.LocalNetwork.NetworkGateWay, ReadOutData) == true, ReadOutData, "", ref Flag.Manufacturer.LocalNetwork.NetworkGateWay);

            #endregion

            #region 开始参数读取
            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "IndependentHeat");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.StartEnabled.IndependentHeat, ReadOutData) == true, ReadOutData, false, ref Flag.StartEnabled.IndependentHeat);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "OddTestArmVarietiesName");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.StartEnabled.OddTestArmVarieties.Name, ReadOutData) == true, ReadOutData, "", ref Flag.StartEnabled.OddTestArmVarieties.Name);

            ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "EvenTestArmVarietiesName");
            Function.Other.SetVable(Function.Other.IsIdentical(Flag.StartEnabled.EvenTestArmVarieties.Name, ReadOutData) == true, ReadOutData, "", ref Flag.StartEnabled.EvenTestArmVarieties.Name);

            #endregion

            #region 加载基底的TkOff

            //TestArm
            for (int i = 0; i < Flag.TempOffTK.TestArm_Inside.Length; i++)
            {
                //Heat_Inside
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.TestArm_Inside[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.TestArm_Inside[i]);

                //Heat_IC
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.TestArm_IC[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.TestArm_IC[i]);
            }

            //ColdPlate
            for (int i = 0; i < Flag.TempOffTK.ColdPlate.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.ColdPlate[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.ColdPlate[i]);
            }

            //HotPlate
            for (int i = 0; i < Flag.TempOffTK.HotPlate.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.HotPlate[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.HotPlate[i]);
            }

            //BorderTemp
            for (int i = 0; i < Flag.TempOffTK.Border.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.Border[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.Border[i]);
            }

            #endregion
        }

        /// <summary>
        /// 保存所有变量数据
        /// </summary>
        public static void SaveData()
        {
            #region 读取产品名称

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "VarietiesName", Flag.StartEnabled.VarietiesName);

            #endregion

            #region 内部制冷水箱温度以及温度偏移设定等保存

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Parameter", "SetTemp", Flag.WaterBox.Parameter.SetTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Parameter", "SetRange", Flag.WaterBox.Parameter.SetRange);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempOffsetPoint", Flag.WaterBox.Offset.TempOffsetPoint);
            for (int i = 0; i < Flag.WaterBox.Offset.TempBasicPoint.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempBasicPoint" + i.ToString(), Flag.WaterBox.Offset.TempBasicPoint[i]);
            }
            for (int i = 0; i < Flag.WaterBox.Offset.TempOffset.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "WaterBox", "Offset", "TempOffset" + i.ToString(), Flag.WaterBox.Offset.TempOffset[i]);
            }

            #endregion

            #region 内部工作制冷曲线参数保存

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartHightTemp", Flag.InternalColdControl.ChartHightTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartLovTemp", Flag.InternalColdControl.ChartLovTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartPointLength", Flag.InternalColdControl.ChartPointLength);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "ChartRefreshTime", Flag.InternalColdControl.ChartRefreshTime);

            for (int i = 0; i < Flag.InternalColdControl.UnitChart.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "UnitChart" + i.ToString(), "Color", Flag.InternalColdControl.UnitChart[i].Color);
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "UnitChart" + i.ToString(), "Type", Flag.InternalColdControl.UnitChart[i].Type);
            }
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "SetTempChart", "Color", Flag.InternalColdControl.SetTempChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "SetTempChart", "Type", Flag.InternalColdControl.SetTempChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "NowTempChart", "Color", Flag.InternalColdControl.NowTempChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "NowTempChart", "Type", Flag.InternalColdControl.NowTempChart.Type);

            #endregion

            #region 外部温度使能保存
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "Independent", Flag.ExternalTestTempChange.IndependentUseHeat);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "UseIcHeat", Flag.ExternalTestTempChange.UseIcHeat);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "DisplayAllChannel", Flag.ExternalTestTempChange.DisplayAllChannel);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "NoEnableDisplayTemp", Flag.ExternalTestTempChange.NoEnableDisplayTemp);


            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmUseIcHeat.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "TestArmUseIcHeat" + i.ToString(), Flag.ExternalTestTempChange.TestArmUseIcHeat[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmEnabled.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "TestArmEnabled" + i.ToString(), Flag.ExternalTestTempChange.TestArmEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.ColdPlateEnabled.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "ColdPlateEnabled" + i.ToString(), Flag.ExternalTestTempChange.ColdPlateEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.HotPlateEnabled.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "HotPlateEnabled" + i.ToString(), Flag.ExternalTestTempChange.HotPlateEnabled[i]);
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.BorderEnabled.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControl", "ExternalTestTempChange", "BorderEnabled" + i.ToString(), Flag.ExternalTestTempChange.BorderEnabled[i]);
            }

            #endregion

            #region 外部工作温度曲线参数保存

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartHightTemp", Flag.ExternalTempControlChart.ChartHightTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartLovTemp", Flag.ExternalTempControlChart.ChartLovTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartPointLength", Flag.ExternalTempControlChart.ChartPointLength);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "ChartRefreshTime", Flag.ExternalTempControlChart.ChartRefreshTime);

            for (int i = 0; i < Flag.ExternalTempControlChart.TempChart.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "TempChart" + i.ToString(), "Color", Flag.ExternalTempControlChart.TempChart[i].Color);
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempControlChart", "TempChart" + i.ToString(), "Type", Flag.ExternalTempControlChart.TempChart[i].Type);
            }

            #endregion

            #region 机组工作状态曲线参数保存

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartHightPressure", Flag.UnitWorkState.ChartHightPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartHightTemp", Flag.UnitWorkState.ChartHightTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartLovPressure", Flag.UnitWorkState.ChartLovPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartLovTemp", Flag.UnitWorkState.ChartLovTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartPointLength", Flag.UnitWorkState.ChartPointLength);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "ChartRefreshTime", Flag.UnitWorkState.ChartRefreshTime);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorInPressureChart", "Color", Flag.UnitWorkState.CompressorInPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorInPressureChart", "Type", Flag.UnitWorkState.CompressorInPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutPressureChart", "Color", Flag.UnitWorkState.CompressorOutPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutPressureChart", "Type", Flag.UnitWorkState.CompressorOutPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutTempChart", "Color", Flag.UnitWorkState.CompressorOutTempChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CompressorOutTempChart", "Type", Flag.UnitWorkState.CompressorOutTempChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserInPressureChart", "Color", Flag.UnitWorkState.CondenserInPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserInPressureChart", "Type", Flag.UnitWorkState.CondenserInPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutPressureChart", "Color", Flag.UnitWorkState.CondenserOutPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutPressureChart", "Type", Flag.UnitWorkState.CondenserOutPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutTempChart", "Color", Flag.UnitWorkState.CondenserOutTempChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "CondenserOutTempChart", "Type", Flag.UnitWorkState.CondenserOutTempChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInPressureChart", "Color", Flag.UnitWorkState.EvaporatorInPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInPressureChart", "Type", Flag.UnitWorkState.EvaporatorInPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInTempChart", "Color", Flag.UnitWorkState.EvaporatorInTempChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorInTempChart", "Type", Flag.UnitWorkState.EvaporatorInTempChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorOutPressureChart", "Color", Flag.UnitWorkState.EvaporatorOutPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "EvaporatorOutPressureChart", "Type", Flag.UnitWorkState.EvaporatorOutPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "HeatExchangerOutPressureChart", "Color", Flag.UnitWorkState.HeatExchangerOutPressureChart.Color);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "HeatExchangerOutPressureChart", "Type", Flag.UnitWorkState.HeatExchangerOutPressureChart.Type);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "UnitWorkState", "UnitIndex", Flag.UnitWorkState.UnitIndex);

            #endregion

            #region 机组使能参数保存

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "InternalColdControl", "Unit" + i.ToString(), "Enabled", Flag.Unit[i].Enabled);
            }

            #endregion

            #region 诊断页面泵手动速度保存

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed1", Flag.Diagnosis.InternalPumpSpeed1);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed2", Flag.Diagnosis.InternalPumpSpeed2);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed3", Flag.Diagnosis.InternalPumpSpeed3);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "InternalPumpSpeed4", Flag.Diagnosis.InternalPumpSpeed4);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed1", Flag.Diagnosis.ExternalPumpSpeed1);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed2", Flag.Diagnosis.ExternalPumpSpeed2);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Diagnosis", "ExternalPumpSpeed3", Flag.Diagnosis.ExternalPumpSpeed3);

            #endregion

            #region 厂商参数保存

            for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "Modular" + i.ToString(), Flag.Manufacturer.Network.Modular[i]);
            }
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkCard", Flag.Manufacturer.Network.NetworkCard);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkGateWay", Flag.Manufacturer.Network.NetworkGateWay);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkIP", Flag.Manufacturer.Network.NetworkIP);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "Network", "NetworkMask", Flag.Manufacturer.Network.NetworkMask);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "ExhaustColding", Flag.Manufacturer.CompressorColdData.ExhaustColding);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "DeepColding", Flag.Manufacturer.CompressorColdData.DeepColding);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "ExhaustOverrun", Flag.Manufacturer.CompressorColdData.ExhaustOverrun);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "CondensateOverrun", Flag.Manufacturer.CompressorColdData.CondensateOverrun);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "RapidColding", Flag.Manufacturer.CompressorColdData.RapidColding);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorColdData", "RestartTime", Flag.Manufacturer.CompressorColdData.RestartTime);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightCondenserTemp", Flag.Manufacturer.CompressorSystemData.HightCondenserTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LovEvaporatorTemp", Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "EvaporatorRecoveryTemp", Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightInPressure", Flag.Manufacturer.CompressorSystemData.HightExhaustTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "SystemLovTemp", Flag.Manufacturer.CompressorSystemData.SystemLovTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightInPressure", Flag.Manufacturer.CompressorSystemData.HightInPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "HightOutPressure", Flag.Manufacturer.CompressorSystemData.HightOutPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LowInPressure", Flag.Manufacturer.CompressorSystemData.LowInPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "LowOutPressure", Flag.Manufacturer.CompressorSystemData.LowOutPressure);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "StartInterval", Flag.Manufacturer.CompressorSystemData.StartInterval);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "EvaporationTempDiffer", Flag.Manufacturer.InternalPump.EvaporationTempDiffer);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "OneHightSpeed", Flag.Manufacturer.InternalPump.OneHightSpeed);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "HightSpeed", Flag.Manufacturer.InternalPump.HightSpeed);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "LevelChange", Flag.Manufacturer.InternalPump.LevelChange);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer1", Flag.Manufacturer.InternalPump.PumpSpeedDiffer[0]);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer2", Flag.Manufacturer.InternalPump.PumpSpeedDiffer[1]);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer3", Flag.Manufacturer.InternalPump.PumpSpeedDiffer[2]);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PumpSpeedDiffer4", Flag.Manufacturer.InternalPump.PumpSpeedDiffer[3]);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "SpeedCorrectTime", Flag.Manufacturer.InternalPump.SpeedCorrectTime);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationSpeed", Flag.Manufacturer.InternalPump.PreRotationSpeed);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationTemp", Flag.Manufacturer.InternalPump.PreRotationTemp);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "InternalPump", "PreRotationTime", Flag.Manufacturer.InternalPump.PreRotationTime);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "CompressorSystemData", "AutoCloseValue", Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkCard", Flag.Manufacturer.LocalNetwork.NetworkCard);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkIP", Flag.Manufacturer.LocalNetwork.NetworkIP);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkMask", Flag.Manufacturer.LocalNetwork.NetworkMask);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "Manufacturer", "LocalNetwork", "NetworkGateWay", Flag.Manufacturer.LocalNetwork.NetworkGateWay);

            #endregion

            #region 开始参数保存
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "IndependentHeat", Flag.StartEnabled.IndependentHeat);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "OddTestArmVarietiesName", Flag.StartEnabled.OddTestArmVarieties.Name);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "StartEnabled", "EvenTestArmVarietiesName", Flag.StartEnabled.EvenTestArmVarieties.Name);
            #endregion
        }

        /// <summary>
        /// 品种进行了更换
        /// </summary>
        public static void ChangeTempVarietiesName(string Name)
        {
            string ReadOutData;

            if (Name == "")
            {

            }
            if (Flag.Handler.IsHandlerControl == false)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerTemp" );
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.ChillerTemp, ReadOutData) == true, ReadOutData, -10f, ref Flag.ExternalTestTempChange.ChillerTemp);

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ChillerRange");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.ExternalTestTempChange.ChillerTemp, ReadOutData) == true, ReadOutData, 1, ref Flag.ExternalTestTempChange.ChillerRange);

                #region 加载测试模式

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "HeatMode");
                if (ReadOutData != null)
                {
                    if (ReadOutData == "ATC")
                    {
                        #region 设定测试头模式

                        for (int i = 0; i < Flag.TestArm.Length; i++)
                        {
                            Flag.VarietiesData.TestArm[i].HeatMode = FlagEnum.HeatMode.ATC;
                        }

                        #endregion

                        #region 是否启用边框加热

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderHeat");
                        for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.BorderTemp[i].IsUseHeat);
                        }

                        #endregion

                        #region 是否启用预冷盘

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "IsPreCooling");
                        for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.ColdPlate[i].IsUseHeat);
                        }

                        #endregion

                        #region 是否启用回热盘

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCooling");
                        for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.HotPlate[i].IsUseHeat);
                        }

                        #endregion

                        #region 设定温度

                        //测试头温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetTemp");
                        for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Temp);
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp);
                        }
                        //测试头温度范围
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetRange");
                        for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Range);
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Range);
                        }
                        //预冷盘温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "PreCoolTemp");
                        for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].Temp, ReadOutData) == true, ReadOutData, 5, ref Flag.VarietiesData.ColdPlate[i].Temp);
                        }
                        //预热盘温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "ResultTemp");
                        for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Temp, ReadOutData) == true, ReadOutData, 5, ref Flag.VarietiesData.HotPlate[i].Temp);
                        }
                        //边框温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "BorderTemp");
                        for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].Temp, ReadOutData) == true, ReadOutData, 5, ref Flag.VarietiesData.BorderTemp[i].Temp);
                        }

                        #endregion
                    }
                    else
                    {
                        #region 设定测试头模式

                        for (int i = 0; i < Flag.TestArm.Length; i++)
                        {
                            Flag.VarietiesData.TestArm[i].HeatMode = FlagEnum.HeatMode.Hot;
                        }

                        #endregion

                        #region 是否启用边框加热

                        for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                        {
                            Flag.VarietiesData.BorderTemp[i].IsUseHeat = false;
                        }

                        #endregion

                        #region 是否启用预冷盘

                        for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                        {
                            Flag.VarietiesData.ColdPlate[i].IsUseHeat = false;
                        }

                        #endregion

                        #region 是否启用回热盘

                        for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                        {
                            Flag.VarietiesData.HotPlate[i].IsUseHeat = true;
                        }

                        #endregion

                        #region 设定温度

                        //测试头温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetTemp");
                        for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Temp);
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp);
                        }
                        //测试头温度范围
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetRange");
                        for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Range);
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Range);
                        }
                        //预热盘温度
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BaseData", "SetTemp");
                        for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                        {
                            Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Temp, ReadOutData) == true, ReadOutData, 5, ref Flag.VarietiesData.HotPlate[i].Temp);
                        }

                        #endregion
                    }
                }

                #endregion

                #region 设定偏移

                //TestArm
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    //Heat_Inside
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset);

                    //Heat_IC
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Offset);
                }

                //ColdPlate
                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.ColdPlate[i].Offset);
                }

                //HotPlate
                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.HotPlate[i].Offset);
                }

                //BorderTemp
                for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.BorderTemp[i].Offset);
                }

                #endregion

                #region 设定功率

                //TestArm
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    //Heat_Inside
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit);

                    //Heat_IC
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "TestArm", "Channel" + i.ToString(), "Heat_IC", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit);
                }

                //ColdPlate
                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "ColdPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.ColdPlate[i].PowerLimit);
                }

                //HotPlate
                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "HotPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.HotPlate[i].PowerLimit);
                }

                //BorderTemp
                for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "BorderTemp", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.BorderTemp[i].PowerLimit);
                }

                #endregion

                #region 外循环泵转速读取

                for (int i = 0; i < Flag.VarietiesData.PumpSpeed.Length; i++)
                {
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", Name, "Pump", "Speed" + i.ToString());
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.PumpSpeed[i], ReadOutData) == true, ReadOutData, 100, ref Flag.VarietiesData.PumpSpeed[i]);
                }

                #endregion
            }

            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmEnabled.Length; i++)
            {
                Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.ColdPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.ColdPlate[i].Enabled = Flag.ExternalTestTempChange.ColdPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.HotPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.HotPlate[i].Enabled = Flag.ExternalTestTempChange.HotPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.BorderEnabled.Length; i++)
            {
                Flag.VarietiesData.BorderTemp[i].Enabled = Flag.ExternalTestTempChange.BorderEnabled[i];
            }

            Flag.WaterBox.Parameter.SetTemp = Flag.ExternalTestTempChange.ChillerTemp;
            Flag.WaterBox.Parameter.SetRange = Flag.ExternalTestTempChange.ChillerRange;
        }

        /// <summary>
        /// 品种进行了更换，切换为独立测试温度
        /// </summary>
        /// <param name="OddTestArmVarietiesName">TestArm1.3.5.7的测试品种</param>
        /// <param name="EvenTestArmVarietiesName">TestArm2.4.6.8的测试品种</param>
        public static void ChangeTempVarietiesName(string OddTestArmVarietiesName, string EvenTestArmVarietiesName)
        {
            string ReadOutData = "";
            string VarietiesName = "";

            if (OddTestArmVarietiesName == "" || EvenTestArmVarietiesName == "")
            {

            }

            if (Flag.Handler.IsHandlerControl == false)
            {
                float OddTemp = 0;
                float EvenTemp = 0;
                float OddRange = 0;
                float EvenRange = 0;

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", OddTestArmVarietiesName, "BaseData", "ChillerTemp");
                Function.Other.SetVable(Function.Other.IsIdentical(OddTemp, ReadOutData) == true, ReadOutData, -10f, ref OddTemp);

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", EvenTestArmVarietiesName, "BaseData", "ChillerTemp");
                Function.Other.SetVable(Function.Other.IsIdentical(EvenTemp, ReadOutData) == true, ReadOutData, -10f, ref EvenTemp);


                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", OddTestArmVarietiesName, "BaseData", "ChillerRange");
                Function.Other.SetVable(Function.Other.IsIdentical(OddRange, ReadOutData) == true, ReadOutData, 1, ref OddRange);
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", EvenTestArmVarietiesName, "BaseData", "ChillerRange");
                Function.Other.SetVable(Function.Other.IsIdentical(EvenRange, ReadOutData) == true, ReadOutData, 1, ref EvenRange);

                if (OddTemp < EvenTemp)
                {
                    Flag.ExternalTestTempChange.ChillerTemp = OddTemp;
                    Flag.ExternalTestTempChange.ChillerRange = OddRange;
                }
                else
                {
                    Flag.ExternalTestTempChange.ChillerTemp = EvenTemp;
                    Flag.ExternalTestTempChange.ChillerRange = EvenRange;
                }

                #region 加载测试参数

                #region 设定测试模式

                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "HeatMode");
                    if (ReadOutData != null)
                    {
                        if (ReadOutData == "ATC")
                        {
                            Flag.VarietiesData.TestArm[i].HeatMode = FlagEnum.HeatMode.ATC;
                        }
                        else
                        {
                            Flag.VarietiesData.TestArm[i].HeatMode = FlagEnum.HeatMode.Hot;
                        }
                    }
                }

                #endregion

                #region 是否启用边框加热

                for (int i = 0; i < Flag.BorderTemp.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "BorderHeat");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.BorderTemp[i].IsUseHeat);
                    }
                    else
                    {
                        Flag.VarietiesData.BorderTemp[i].IsUseHeat = false;
                    }
                }

                #endregion

                #region 是否启用预冷盘

                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "IsPreCooling");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.ColdPlate[i].IsUseHeat);
                    }
                    else
                    {
                        Flag.ColdPlate[i].Heat.SetToHeat.IsUseHeat = false;
                    }
                }

                #endregion

                #region 是否启用回热盘

                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "PreCooling");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].IsUseHeat, ReadOutData) == true, ReadOutData, false, ref Flag.VarietiesData.HotPlate[i].IsUseHeat);
                    }
                    else
                    {
                        Flag.VarietiesData.HotPlate[i].IsUseHeat = true;
                    }
                }
                

                #endregion

                #region 设定温度

                //测试头温度
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "SetTemp");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Temp);
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Temp);
                }

                //测试头温度范围
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "SetRange");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Range);
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Range, ReadOutData) == true, ReadOutData, 3, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Range);
                }

                //预冷盘温度
                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "PreCoolTemp");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].Temp, ReadOutData) == true, ReadOutData, 5, ref Flag.VarietiesData.ColdPlate[i].Temp);
                    }
                    else
                    {
                        Flag.VarietiesData.ColdPlate[i].Temp = 30;
                    }

                }

                //预热盘温度
                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "ResultTemp");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.HotPlate[i].Temp);
                    }
                    else
                    {
                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "SetTemp");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.HotPlate[i].Temp);
                    }
                }

                //边框温度
                for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                {
                    if (Flag.VarietiesData.TestArm[i].HeatMode == FlagEnum.HeatMode.ATC)
                    {
                        Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                        ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BaseData", "BorderTemp");
                        Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].Temp, ReadOutData) == true, ReadOutData, 30, ref Flag.VarietiesData.BorderTemp[i].Temp);
                    }
                    else
                    {
                        Flag.VarietiesData.BorderTemp[i].Temp = 30;
                    }
                }

                #endregion

                #endregion

                #region 设定偏移

                //TestArm
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    //Heat_Inside
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.Offset);

                    //Heat_IC
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "TestArm", "Channel" + i.ToString(), "Heat_IC", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_IC.Offset);
                }

                //ColdPlate
                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "ColdPlate", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.ColdPlate[i].Offset);
                }

                //HotPlate
                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "HotPlate", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.HotPlate[i].Offset);
                }

                //BorderTemp
                for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BorderTemp", "Channel" + i.ToString(), "Heat", "TempOffset3");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].Offset, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.BorderTemp[i].Offset);
                }

                #endregion

                #region 设定功率

                //TestArm
                for (int i = 0; i < Flag.VarietiesData.TestArm.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    //Heat_Inside
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_Inside.PowerLimit);

                    //Heat_IC
                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "TestArm", "Channel" + i.ToString(), "Heat_IC", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.TestArm[i].TestArm_IC.PowerLimit);
                }

                //ColdPlate
                for (int i = 0; i < Flag.VarietiesData.ColdPlate.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "ColdPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.ColdPlate[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.ColdPlate[i].PowerLimit);
                }

                //HotPlate
                for (int i = 0; i < Flag.VarietiesData.HotPlate.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "HotPlate", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.HotPlate[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.HotPlate[i].PowerLimit);
                }

                //BorderTemp
                for (int i = 0; i < Flag.VarietiesData.BorderTemp.Length; i++)
                {
                    Function.Other.SetVable(i % 2 == 0, OddTestArmVarietiesName, EvenTestArmVarietiesName, ref VarietiesName);

                    ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", VarietiesName, "BorderTemp", "Channel" + i.ToString(), "Heat", "PowerLimit");
                    Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.BorderTemp[i].PowerLimit, ReadOutData) == true, ReadOutData, 0, ref Flag.VarietiesData.BorderTemp[i].PowerLimit);
                }

                #endregion

                #region 外循环泵转速读取

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", EvenTestArmVarietiesName, "Pump", "Speed0");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.PumpSpeed[0], ReadOutData) == true, ReadOutData, 100, ref Flag.VarietiesData.PumpSpeed[0]);

                ushort Pump1 = 0;
                ushort Pump2 = 0;

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", EvenTestArmVarietiesName, "Pump", "Speed1");
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", OddTestArmVarietiesName, "Pump", "Speed1");
                Function.Other.SetVable(Pump1 > Pump2, Pump1, Pump2, ref Flag.VarietiesData.PumpSpeed[1]);

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", OddTestArmVarietiesName, "Pump", "Speed2");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.VarietiesData.PumpSpeed[2], ReadOutData) == true, ReadOutData, 100, ref Flag.VarietiesData.PumpSpeed[2]);

                #endregion

            }

            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmEnabled.Length; i++)
            {
                Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.ColdPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.ColdPlate[i].Enabled = Flag.ExternalTestTempChange.ColdPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.HotPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.HotPlate[i].Enabled = Flag.ExternalTestTempChange.HotPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.BorderEnabled.Length; i++)
            {
                Flag.VarietiesData.BorderTemp[i].Enabled = Flag.ExternalTestTempChange.BorderEnabled[i];
            }

            Flag.WaterBox.Parameter.SetTemp = Flag.ExternalTestTempChange.ChillerTemp;
            Flag.WaterBox.Parameter.SetRange = Flag.ExternalTestTempChange.ChillerRange;
        }

        /// <summary>
        /// 菜单与复位切换使能
        /// </summary>
        public static bool MenuChangeState = false;

        /// <summary>
        /// 制冷单元相关
        /// </summary>
        public static FlagStruct.Unit[] Unit;
        /// <summary>
        /// 外循环泵相关，地址为5-7，初始地址为1开始
        /// </summary>
        public static FlagStruct.Pump[] ExternalPump;
        /// <summary>
        /// 水箱相关
        /// </summary>
        public static FlagStruct.WaterBox WaterBox;
        /// <summary>
        /// 系统开机后就存在的信息
        /// </summary>
        public static FlagStruct.SystemLogList RunLogList;
        /// <summary>
        /// 系统开机后就存在的信息（已向系统添加的，用于查询创建缓存）
        /// </summary>
        public static FlagStruct.AddSystemLogList AddLogList;
        /// <summary>
        /// 系统当前异常信息
        /// </summary>
        public static FlagStruct.SystemLogList NowLogList;
        /// <summary>
        /// Handler各通道设定温度与当前温度状态
        /// </summary>
        public static FlagStruct.TestArmHeatingChannel[] TestArm;
        /// <summary>
        /// 冷盘通道
        /// </summary>
        public static FlagStruct.ColdPlateHeatingChannel[] ColdPlate;
        /// <summary>
        /// 热盘通道
        /// </summary>
        public static FlagStruct.HeatingChannel[] HotPlate;
        /// <summary>
        /// 外边框加热通道
        /// </summary>
        public static FlagStruct.HeatingChannel[] BorderTemp;
        /// <summary>
        /// 浮动头与连接板加热通道
        /// </summary>
        public static FlagStruct.HeatingChannel[] OtherTemp;

        /// <summary>
        /// 外循环泵转速 ， 0=TestArm2.4.6.8    1=ColdPlate1.2     2=TestArm1.3.5.7
        /// </summary>
        public static ushort[] OutPumpSpeed;

        /// <summary>
        /// 内部制冷控制页面所有参数
        /// </summary>
        public static FlagStruct.InternalColdControl InternalColdControl;
        /// <summary>
        /// 外部测试所需要的温度设定
        /// </summary>
        public static FlagStruct.ExternalTestTempChange ExternalTestTempChange;
        /// <summary>
        /// 外部温控控制页面所有参数
        /// </summary>
        public static FlagStruct.ExternalTempControlChart ExternalTempControlChart;
        /// <summary>
        /// 压缩机运行状态页面数据组
        /// </summary>
        public static FlagStruct.UnitWorkState UnitWorkState;
        /// <summary>
        /// 系统诊断页面相关数据
        /// </summary>
        public static FlagStruct.Diagnosis Diagnosis;
        /// <summary>
        /// 已登陆用户相关数据
        /// </summary>
        public static FlagStruct.User LogOnUser;
        /// <summary>
        /// 加热和制冷总使能
        /// </summary>
        public static FlagStruct.StartEnabled StartEnabled;
        /// <summary>
        /// 系统线程
        /// </summary>
        public static FlagStruct.SystemThread SystemThread;
        /// <summary>
        /// 厂商参数集合
        /// </summary>
        public static FlagStruct.Manufacturer Manufacturer;
        /// <summary>
        /// 露点传感器
        /// </summary>
        public static FlagStruct._DewPoint[] DewPoint;
        /// <summary>
        /// Handler通讯状态
        /// </summary>
        public static FlagStruct._Handler Handler;
        /// <summary>
        /// 温控器所用到的温度修正基低参数
        /// </summary>
        public static FlagStruct.TempOffTK TempOffTK;
        /// <summary>
        /// 当前产品所有的数据
        /// </summary>
        public static FlagStruct.VarietiesData VarietiesData;

        public static bool CompressorErr;
    }
}
