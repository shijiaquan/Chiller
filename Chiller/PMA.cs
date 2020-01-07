using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;

namespace Chiller
{
    public class PMA
    {
        private static SerialPort SP;
        private static ModbusSerialMaster PMAMaster;
        private static object LockValue = 0;

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
        /// 初始化串口,所有PMA温控器的站号均需从1开始的连续站号,E.8.1的链接方式
        /// </summary>
        /// <param name="Port">串口号</param>
        /// <param name="Baud">波特率</param>
        /// <returns>成功或失败</returns>
        public static bool Initialization(string Port, int Baud)
        {
            try
            {
                SP = new SerialPort(Port, Baud, Parity.Even, 8, StopBits.One);
                SP.Open();

                PMAMaster = ModbusSerialMaster.CreateRtu(SP);
                PMAMaster.Transport.ReadTimeout = 200;
                PMAMaster.Transport.WriteTimeout = 200;
                PMAMaster.Transport.SlaveBusyUsesRetryCount = true;
                PMAMaster.Transport.Retries = 3;
                return SP.IsOpen;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取Sta2字状态
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Get_C_Sta2(byte Address, ushort Channel, ref short C_Sta2)
        {
            if(PMAMaster==null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(1238 + Channel * 512);
            ushort DataLenght = 1;
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                     ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }
                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    C_Sta2 = Auxiliary.UshortToShort(ReturnData[0]);
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
        /// 获取Sta2字状态
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="C_Sta2">C_Sta2数据</param>
        /// <returns>成功或失败</returns>
        public static bool Get_C_Sta2(string Address, ref short C_Sta2)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Get_C_Sta2(Station, Channel, ref C_Sta2);
        }

        /// <summary>
        /// 获取指定通道的温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Get_PV(byte Address, ushort Channel, ref float PV)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17006 + Channel * 1);
            ushort DataLenght = 1;
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                     ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    PV = Auxiliary.UshortToShort(ReturnData[0]) / 10f;
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
        /// 获取指定通道的温度
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">地址温度</param>
        /// <returns>成功或失败</returns>
        public static bool Get_PV(string Address, ref float PV)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Get_PV(Station, Channel, ref PV);
        }
        /// <summary>
        /// 获取全部通道的温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="ChannelNumber">通道数量</param>
        /// <param name="Address">返回的温度数组</param>
        /// <returns>成功或失败</returns>
        public static bool GetAll_PV(byte Address, ushort ChannelNumber, ref float[] Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17006);
            ushort DataLenght = ChannelNumber;

            Temp = new float[ChannelNumber];
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                    ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }
                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    for (int i = 0; i < DataLenght; i++)
                    {
                        Temp[i] = Auxiliary.UshortToShort(ReturnData[i]) / 10f;
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
        /// 获取全部通道的温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Address">返回的温度数组</param>
        /// <returns>成功或失败</returns>
        public static bool GetAll_PV(byte Address, ref float[] Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            return GetAll_PV(Address, (ushort)Temp.Length, ref Temp);
        }

        /// <summary>
        /// 设定指定通道温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Set_SP(byte Address, ushort Channel, short Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17526 + Channel * 512);
            ushort Value = Auxiliary.ShortToUshort((short)(Temp * 10));
            float PV = 0;
            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress, StartAddress, Value);
                }
                if (Get_PV(SlaveAddress, StartAddress, ref PV) == false) { return false; }

                if (PV != Temp) { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定指定通道温度
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Set_SP(string Address, ushort Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Set_SP(Station, Channel, Temp);
        }
        /// <summary>
        /// 设定指定通道温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Set_SP(byte Address, ushort Channel, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17526 + Channel * 512);
            ushort Value = Auxiliary.ShortToUshort((short)(Temp * 10));
            float SP = 0;
            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress, StartAddress, Value);
                }
                if (Get_SP(Address, Channel, ref SP) == false) { return false; }

                if (SP != Temp) { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定指定通道温度
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Set_SP(string Address, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Set_SP(Station, Channel, Temp);
        }

        /// <summary>
        /// 获取指定通道的设定值
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="SP">设定的温度</param>
        /// <returns></returns>
        public static bool Get_SP(byte Address, ushort Channel, ref float SP)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17526 + Channel * 512);
            ushort DataLenght = 1;
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                    ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    SP = Auxiliary.UshortToShort(ReturnData[0]) / 10f;
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
        /// 获取指定通道的设定值
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="SP">设定的温度</param>
        /// <returns></returns>
        public static bool Get_SP(string Address, ref float SP)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Get_SP(Station, Channel, ref SP);
        }

        /// <summary>
        /// 设定温度偏移
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="InL">标定下限，修正前</param>
        /// <param name="OuL">标定下限，修正后</param>
        /// <param name="InH">标定上限，修正前</param>
        /// <param name="OuH">标定上限，修正后</param>
        /// <returns>成功或失败</returns>
        public static bool SetTempDeviation(byte Address, ushort Channel, float InL, float OuL, float InH, float OuH)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17465 + Channel * 512);
            ushort[] Value = new ushort[4];
            Value[0] = Auxiliary.ShortToUshort((short)(InL * 10f));
            Value[1] = Auxiliary.ShortToUshort((short)(OuL * 10f));
            Value[2] = Auxiliary.ShortToUshort((short)(InH * 10f));
            Value[3] = Auxiliary.ShortToUshort((short)(OuH * 10f));

            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteMultipleRegisters(SlaveAddress, StartAddress, Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定温度偏移
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="InL">标定下限，修正前</param>
        /// <param name="OuL">标定下限，修正后</param>
        /// <param name="InH">标定上限，修正前</param>
        /// <param name="OuH">标定上限，修正后</param>
        /// <returns>成功或失败</returns>
        public static bool SetTempDeviation(string Address, float InL, float OuL, float InH, float OuH)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return SetTempDeviation(Station, Channel, InL, OuL, InH, OuH);
        }

        /// <summary>
        /// 设定温度修正
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Offset">修正温度值</param>
        /// <returns></returns>
        public static bool SetOffTK(byte Address, ushort Channel, float Offset)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17470 + Channel * 512);
            ushort Value = Auxiliary.ShortToUshort((short)(Offset * 10));
            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress, StartAddress, Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定温度修正
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Offset">修正温度值</param>
        /// <returns></returns>
        public static bool SetOffTK(string Address, float Offset)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return SetOffTK(Station, Channel, Offset);
        }

        /// <summary>
        /// 设定加热功率上限
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Limit">功率上限值</param>
        /// <returns></returns>
        public static bool SetPowerLimit(byte Address, ushort Channel, ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17572 + Channel * 512);
            ushort Value = Auxiliary.ShortToUshort((short)(Limit * 10));
            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress, StartAddress, Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 设定加热功率上限
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Limit">功率上限值</param>
        /// <returns></returns>
        public static bool SetPowerLimit(string Address, ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return SetPowerLimit(Station, Channel, Limit);
        }

        /// <summary>
        /// 获取当前的加热功率
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Limit">返回的功率上限值</param>
        /// <returns></returns>
        public static bool GetPowerLimit(byte Address, ushort Channel, ref ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17572 + Channel * 512);
            
            ushort DataLenght = 1;
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                    ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    Limit = (ushort)(ReturnData[0]/10f);
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
        /// 获取当前的加热功率
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Limit">返回的功率上限值</param>
        /// <returns></returns>
        public static bool GetPowerLimit(string Address,ref ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return GetPowerLimit(Station, Channel,ref Limit);
        }
        /// <summary>
        /// 获取当前的加热功率
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Limit">返回的功率上限值</param>
        /// <returns></returns>
        public static bool Getpower(byte Address, ushort Channel, ref ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(17157 + Channel * 512);
            ushort DataLenght = 1;
            ushort[] ReturnData;
            try
            {
                lock (LockValue)
                {
                    ReturnData = PMAMaster.ReadInputRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (ReturnData != null && ReturnData.Length == DataLenght)
                {
                    Limit = (ushort)(ReturnData[0] / 10f);
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
        /// 获取当前的加热功率
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Limit">返回的功率上限值</param>
        /// <returns></returns>
        public static bool Getpower(string Address, ref ushort Limit)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return GetPowerLimit(Station, Channel, ref Limit);
        }
        /***********************************************************************
         *                        温控器开启或关闭方法
         * ********************************************************************/

        /// <summary>
        /// 开启或关闭温控器，通过OnOff标志判断，On开启温控器且设定温度，Off关闭温控器，不设定温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="OnOff">打开或关闭温控器</param>
        /// <param name="Temp">开启温控器需要的设定温度</param>
        /// <returns>成功或失败</returns>
        public static bool RunOrStop(byte Address, ushort Channel, bool OnOff, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (OnOff == false)
            {
                return Stop(Address, Channel);
            }
            else
            {
                return Start(Address, Channel, Temp);
            }
        }
        /// <summary>
        /// 开启或关闭温控器，通过OnOff标志判断，On开启温控器且设定温度，Off关闭温控器，不设定温度
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="OnOff">打开或关闭温控器</param>
        /// <param name="Temp">开启温控器需要的设定温度</param>
        /// <returns>成功或失败</returns>
        public static bool RunOrStop(string Address, bool OnOff, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return RunOrStop(Station, Channel, OnOff, Temp);
        }

        /// <summary>
        /// 开启或关闭温控器，通过OnOff标志判断，On开启温控器且设定温度，Off关闭温控器，不设定温度
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="OnOff">打开或关闭温控器</param>
        /// <returns>成功或失败</returns>
        public static bool RunOrStop(byte Address, ushort Channel, bool OnOff)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (OnOff == false)
            {
                return Stop(Address, Channel);
            }
            else
            {
                return Start(Address, Channel);
            }
        }
        /// <summary>
        /// 开启或关闭温控器，通过OnOff标志判断，On开启温控器且设定温度，Off关闭温控器，不设定温度
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="OnOff">打开或关闭温控器</param>
        /// <returns>成功或失败</returns>
        public static bool RunOrStop(string Address, bool OnOff)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return RunOrStop(Station, Channel, OnOff);
        }

        /// <summary>
        /// 停止温控器指定通道加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Stop(byte Address, ushort Channel)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress2 = Address;
            ushort StartAddress2 = (ushort)(1497 + Channel * 512);
            ushort Value2 = 1;

            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress2, StartAddress2, Value2);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 停止温控器指定通道加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Stop(string Address)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);
            return Stop(Station, Channel);

        }

        /// <summary>
        /// 停止温控器全部通道加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <returns>成功或失败</returns>
        public static bool StopAllChannel(byte Address, ushort ChannelNumber)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            for (ushort i = 0; i < ChannelNumber; i++)
            {
                if (Stop(Address, i) == false)
                { return false; }
            }
            return true;
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Start(byte Address, ushort Channel)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress2 = Address;
            ushort StartAddress2 = (ushort)(1497 + Channel * 512);
            ushort StartAddress3 = (ushort)(1495 + Channel * 512);
            ushort Value2 = 0;

            try
            {
                lock (LockValue)
                {
                    PMAMaster.WriteSingleRegister(SlaveAddress2, StartAddress2, Value2);
                    PMAMaster.WriteSingleRegister(SlaveAddress2, StartAddress3, Value2);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <returns>成功或失败</returns>
        public static bool Start(string Address)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return Start(Station, Channel);

        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Start(byte Address, ushort Channel, short Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (Set_SP(Address, Channel, Temp) == false) { return false; }

            if (Start(Address, Channel) == false) { return false; }

            return true;
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Start(string Address, short Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return Start(Station, Channel, Temp);
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Start(byte Address, ushort Channel, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (Set_SP(Address, Channel, Temp) == false) { return false; }

            if (Start(Address, Channel) == false) { return false; }

            return true;
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool Start(string Address, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            if (Set_SP(Station, Channel, Temp) == false) { return false; }

            if (Start(Station, Channel) == false) { return false; }

            return true;
        }

        /// <summary>
        /// 开启温控器，并开始加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <returns>成功或失败</returns>
        public static bool StartAllChannel(byte Address, ushort ChannelNumber)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            for (ushort i = 0; i < ChannelNumber; i++)
            {
                if (Start(Address, i) == false) { return false; }
            }
            return true;
        }

        /// <summary>
        /// 重启加热，先关闭加热更改温度，开启加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool ReStart(byte Address, ushort Channel, short Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (Stop(Address, Channel) == false) { return false; }

            Thread.Sleep(100);

            if (Start(Address, Channel, Temp) == false) { return false; }

            Thread.Sleep(100);

            return true;
        }

        /// <summary>
        /// 重启加热，先关闭加热更改温度，开启加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool ReStart(string Address, short Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return ReStart(Station, Channel, Temp);
        }

        /// <summary>
        /// 重启加热，先关闭加热更改温度，开启加热
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool ReStart(byte Address, ushort Channel, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            if (Stop(Address, Channel) == false) { return false; }

            Thread.Sleep(100);

            if (Start(Address, Channel, Temp) == false) { return false; }

            Thread.Sleep(100);

            return true;
        }

        /// <summary>
        /// 重启加热，先关闭加热更改温度，开启加热
        /// </summary>
        /// <param name="Address">站号地址与通道地址</param>
        /// <param name="Temp">设定的温度</param>
        /// <returns>成功或失败</returns>
        public static bool ReStart(string Address, float Temp)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return ReStart(Station, Channel, Temp);
        }

        /// <summary>
        /// 读取Y.Y2状态，是否激活Y2输出
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="State">使用Ref返回Y_Y2状态</param>
        /// <returns>成功或失败</returns>
        public static bool Get_Y_Y2(byte Address, ushort Channel, ref short State)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(1495 + Channel * 512);
            ushort DataLenght = 1;
            ushort[] Value;
            try
            {
                lock (LockValue)
                {
                    Value = PMAMaster.ReadHoldingRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (Value.Length != DataLenght) { return false; }

                State = Auxiliary.UshortToShort(Value[0]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取Y.Y2状态，是否激活Y2输出
        /// </summary>
        /// <param name="Address">站号和地址1-1方式</param>
        /// <param name="State">使用Ref返回Y_Y2状态</param>
        /// <returns>成功或失败</returns>
        public static bool Get_Y_Y2(string Address, ref short State)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return Get_Y_Y2(Station, Channel, ref State);
        }

        /// <summary>
        /// 读取C_Off状态，是否关闭温控器输出
        /// </summary>
        /// <param name="Address">站号地址</param>
        /// <param name="Channel">通道地址</param>
        /// <param name="State">使用Ref返回C_Off状态</param>
        /// <returns>成功或失败</returns>
        public static bool Get_C_Off(byte Address, ushort Channel, ref bool State)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            byte SlaveAddress = Address;
            ushort StartAddress = (ushort)(1497 + Channel * 512);
            ushort DataLenght = 1;
            ushort[] Value;
            try
            {
                lock (LockValue)
                {
                    Value = PMAMaster.ReadHoldingRegisters(SlaveAddress, StartAddress, DataLenght);
                }

                if (Value.Length != DataLenght) { return false; }

                if (Auxiliary.UshortToShort(Value[0]) == 0)
                {
                    State = true;
                }
                else
                {
                    State = false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取C_Off状态，是否关闭温控器输出
        /// </summary>
        /// <param name="Address">站号和地址1-1方式</param>
        /// <param name="State">使用Ref返回C_Off状态</param>
        /// <returns>成功或失败</returns>
        public static bool Get_C_Off(string Address, ref bool State)
        {
            if (PMAMaster == null)
            {
                return false;
            }
            string[] Index = Address.Replace(" ", "").Split(new char[] { '-' });
            byte Station = byte.Parse(Index[0]);
            ushort Channel = ushort.Parse(Index[1]);

            return Get_C_Off(Station, Channel, ref State);
        }

        /// <summary>
        /// 获取PMA温控器状态，返回为""表示正常，其余则是报警状态，返回："Err"通信失败
        /// 
        /// </summary>
        /// <param name="Address">站号和地址1-1方式</param>
        /// <returns></returns>
        public static string IsErrState(string Address)
        {
            short C_Sta2 = 0;
            string ErrorCode = "";

            if (Get_C_Sta2(Address, ref C_Sta2) == false)
            {
                return "Err";
            }

            if (Int_To_Bit(C_Sta2, 3) == true)
            {
                ErrorCode = "Err:02";
            }
            else if (Int_To_Bit(C_Sta2, 9) == true)
            {
                ErrorCode = "Err:08";
            }
            else if (Int_To_Bit(C_Sta2, 11) == true)
            {
                ErrorCode = "Err:10";
            }
            else if (Int_To_Bit(C_Sta2, 12) == true)
            {
                ErrorCode = "Err:11";
            }
            else if (Int_To_Bit(C_Sta2, 13) == true)
            {
                ErrorCode = "Err:12";
            }
            else if (Int_To_Bit(C_Sta2, 14) == true)
            {
                ErrorCode = "Err:13";
            }
            else if (Int_To_Bit(C_Sta2, 15) == true)
            {
                ErrorCode = "Err:14";
            }
            else if (Int_To_Bit(C_Sta2, 16) == true)
            {
                ErrorCode = "Err:15";
            }
            else
            {
                ErrorCode = "";
            }

            return ErrorCode;
        }

        /// <summary> 
        /// 将十进制转化为二进制，然后判断右侧向左的某一位是1还是0，1为true，0为false
        /// </summary> 
        /// <param name="num">需要转化的数</param> 
        /// <param name="Bit">获取的位置</param> 
        private static bool Int_To_Bit(short num, int Bit)
        {
            string data = Convert.ToString((int)num, 2).PadLeft(16, '0');
            if (data.Substring(data.Length - Bit, 1) == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
