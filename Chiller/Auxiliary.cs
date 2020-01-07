using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chiller
{
    public class Auxiliary
    {
        ///// <summary>
        ///// 用于截取PMA报文之间的数据
        ///// </summary>
        ///// <param name="WriteData">向PMA温控器发送的数据</param>
        ///// <param name="ReadData">PMA向用户回复的数据</param>
        ///// <param name="PMATemp">解析完成后存放的地址</param>
        ///// <returns></returns>
        //public static bool GetMessageData(byte[] WriteData, byte[] ReadData, ref PMA.PMABody[] PMATemp)
        //{
        //    if (WriteData == null || ReadData == null)
        //    {
        //        return false;
        //    }
        //    if (CheckCRC(WriteData) == false || CheckCRC(ReadData) == false)
        //    {
        //        return false;
        //    }
        //    if (WriteData[0] != ReadData[0] || WriteData[1] != ReadData[1])
        //    {
        //        return false;
        //    }

        //    if (WriteData[1] == 03 || WriteData[1] == 04)
        //    {
        //        int WriteAddress = HexToShort(WriteData[2].ToString("X2") + WriteData[3].ToString("X2"));       //获取内存地址，数据格式为十进制
        //        int WriteLenght = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));        //获取数据的长度，数据格式为十进制
        //        int ReadLenght = ReadData[2] / 2;                                                                        //输入寄存器数量

        //        #region 校验数据长度是否正确
        //        if (WriteLenght != ReadLenght)
        //        {
        //            return false;
        //        }
        //        #endregion

        //        for (int i = 0; i < 32; i++)
        //        {
        //            #region 读取SP设定值
        //            if (WriteAddress == 1142 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].SP = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17526 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].SP = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 读取PV当前值
        //            else if (WriteAddress == 622 + i)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    PMATemp[i + j].PV = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                }
        //                break;
        //            }
        //            else if (WriteAddress == 17006 + i)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    PMATemp[i + j].PV = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                }
        //                break;
        //            }
        //            #endregion

        //            #region 连续或单个读取C_Sta、Ypid、Xeff、SPeff、T_Sta、C_Steuer状态值
        //            else if (WriteAddress >= 722 + i * 7 && WriteAddress <= 777 + i * 7)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 772 + i * 512)
        //                    {
        //                        PMATemp[i].C_Sta = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 773 + i * 512)
        //                    {
        //                        PMATemp[i].Ypid = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 774 + i * 512)
        //                    {
        //                        PMATemp[i].Xeff = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 775 + i * 512)
        //                    {
        //                        PMATemp[i].SPeff = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 776 + i * 512)
        //                    {
        //                        PMATemp[i].T_Sta = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 777 + i * 512)
        //                    {
        //                        PMATemp[i].C_Steuer = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                }
        //                break;
        //            }
        //            else if (WriteAddress >= 17156 + i * 7 && WriteAddress <= 17161 + i * 7)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 17156 + i * 512)
        //                    {
        //                        PMATemp[i].C_Sta = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17157 + i * 512)
        //                    {
        //                        PMATemp[i].Ypid = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17158 + i * 512)
        //                    {
        //                        PMATemp[i].Xeff = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17159 + i * 512)
        //                    {
        //                        PMATemp[i].SPeff = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17160 + i * 512)
        //                    {
        //                        PMATemp[i].T_Sta = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17161 + i * 512)
        //                    {
        //                        PMATemp[i].C_Steuer = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                }
        //                break;
        //            }
        //            #endregion

        //            #region 连续或单个读取InL、OuL、InH、OuH设定值
        //            else if (WriteAddress >= 1081 + i * 512 && WriteAddress <= 1084 + i * 512)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 1081 + i * 512)
        //                    {
        //                        PMATemp[i].InL = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1082 + i * 512)
        //                    {
        //                        PMATemp[i].OuL = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1083 + i * 512)
        //                    {
        //                        PMATemp[i].InH = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1084 + i * 512)
        //                    {
        //                        PMATemp[i].OuH = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2"));
        //                    }
        //                }
        //                break;
        //            }
        //            else if (WriteAddress >= 17465 + i * 512 && WriteAddress <= 17468 + i * 512)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 17465 + i * 512)
        //                    {
        //                        PMATemp[i].InL = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17466 + i * 512)
        //                    {
        //                        PMATemp[i].OuL = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17467 + i * 512)
        //                    {
        //                        PMATemp[i].InH = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17468 + i * 512)
        //                    {
        //                        PMATemp[i].OuH = HexToShort(ReadData[3 + j * 2].ToString("X2") + ReadData[4 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                }
        //                break;
        //            }
        //            #endregion

        //            #region 读取P设定值
        //            else if (WriteAddress == 1174 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].P = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17558 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].P = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 读取I设定值
        //            else if (WriteAddress == 1176 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].I = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17560 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].I = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 读取D设定值
        //            else if (WriteAddress == 1178 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].D = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17562 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].D = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 读取C_Sta2状态值，包含断线传感器故障等报警状态
        //            else if (WriteAddress == 1238 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].C_Sta2 = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17622 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].C_Sta2 = (short)(HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f);
        //                break;
        //            }
        //            #endregion

        //            #region 设定Y_Y2激活状态状态值
        //            else if (WriteAddress == 1495 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].Y_Y2 = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17879 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].Y_Y2 = (short)(HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f);
        //                break;
        //            }
        //            #endregion

        //            #region 设定C_Off是否禁用所用输出状态
        //            else if (WriteAddress == 1497 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].C_Off = HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17881 + i * 512 && WriteLenght == 1)
        //            {
        //                PMATemp[i].C_Off = (short)(HexToShort(ReadData[3].ToString("X2") + ReadData[4].ToString("X2")) / 10f);
        //                break;
        //            }
        //            #endregion
        //        }
        //        return true;
        //    }
        //    else if (WriteData[1] == 06 && WriteData.Length == ReadData.Length)
        //    {
        //        #region 校验数据长度是否正确
        //        for (int i = 0; i < WriteData.Length; i++)
        //        {
        //            if (WriteData[i] != ReadData[i])
        //            {
        //                return false;
        //            }
        //        }
        //        #endregion

        //        int WriteAddress = HexToShort(WriteData[2].ToString("X2") + WriteData[3].ToString("X2"));       //获取内存地址，数据格式为十进制

        //        for (int i = 0; i < 32; i++)
        //        {
        //            #region 设定SP当前值
        //            if (WriteAddress == 1142 + i * 512)
        //            {
        //                PMATemp[i].SP = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17526 + i * 512)
        //            {
        //                PMATemp[i].SP = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 设定C_Steuer状态值
        //            else if (WriteAddress == 777 + i * 7)
        //            {
        //                PMATemp[i].C_Steuer = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17161 + i * 7)
        //            {
        //                PMATemp[i].C_Steuer = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 设定Y_Y2激活状态状态值
        //            else if (WriteAddress == 1495 + i * 512)
        //            {
        //                PMATemp[i].Y_Y2 = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17879 + i * 512)
        //            {
        //                PMATemp[i].Y_Y2 = (short)(HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f);
        //                break;
        //            }
        //            #endregion

        //            #region 设定C_Off是否禁用所用输出状态
        //            else if (WriteAddress == 1497 + i * 512)
        //            {
        //                PMATemp[i].C_Off = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17881 + i * 512)
        //            {
        //                PMATemp[i].C_Off = (short)(HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f);
        //                break;
        //            }
        //            #endregion

        //            #region 设定InL标定下限,修正前
        //            else if (WriteAddress == 1081 + i * 512)
        //            {
        //                PMATemp[i].InL = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17465 + i * 512)
        //            {
        //                PMATemp[i].InL = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 设定OuL标定下限,修正后
        //            else if (WriteAddress == 1082 + i * 512)
        //            {
        //                PMATemp[i].OuL = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17466 + i * 512)
        //            {
        //                PMATemp[i].OuL = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 设定InH标定上限,修正前
        //            else if (WriteAddress == 1083 + i * 512)
        //            {
        //                PMATemp[i].InH = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17466 + i * 512)
        //            {
        //                PMATemp[i].InH = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion

        //            #region 设定OuH标定上限,修正后
        //            else if (WriteAddress == 1084 + i * 512)
        //            {
        //                PMATemp[i].OuH = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2"));
        //                break;
        //            }
        //            else if (WriteAddress == 17467 + i * 512)
        //            {
        //                PMATemp[i].OuH = HexToShort(WriteData[4].ToString("X2") + WriteData[5].ToString("X2")) / 10f;
        //                break;
        //            }
        //            #endregion
        //        }
        //        return true;
        //    }
        //    else if (WriteData[1] == 16 && ReadData.Length == 8)
        //    {
        //        int WriteAddress = Convert.ToInt32(WriteData[2].ToString("X2") + WriteData[3].ToString("X2"), 16);       //获取内存地址，数据格式为十进制
        //        int WriteLenght = WriteData[6] / 2; ;                                                                    //获取数据的长度，数据格式为十进制

        //        for (int i = 0; i < 32; i++)
        //        {
        //            #region 连续或单个设定InL、OuL、InH、OuH标定值
        //            if (WriteAddress >= 1081 + i * 512 && WriteAddress <= 1084 + i * 512)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 1081 + i * 512)
        //                    {
        //                        PMATemp[i].InL = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1082 + i * 512)
        //                    {
        //                        PMATemp[i].OuL = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1083 + i * 512)
        //                    {
        //                        PMATemp[i].InH = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2"));
        //                    }
        //                    else if (WriteAddress + j == 1084 + i * 512)
        //                    {
        //                        PMATemp[i].OuH = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2"));
        //                    }
        //                }
        //                break;
        //            }
        //            else if (WriteAddress >= 17465 + i * 512 && WriteAddress <= 17468 + i * 512)
        //            {
        //                for (int j = 0; j < WriteLenght; j++)
        //                {
        //                    if (WriteAddress + j == 17465 + i * 512)
        //                    {
        //                        PMATemp[i].InL = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17466 + i * 512)
        //                    {
        //                        PMATemp[i].OuL = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17467 + i * 512)
        //                    {
        //                        PMATemp[i].InH = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                    else if (WriteAddress + j == 17468 + i * 512)
        //                    {
        //                        PMATemp[i].OuH = HexToShort(WriteData[7 + j * 2].ToString("X2") + WriteData[8 + j * 2].ToString("X2")) / 10f;
        //                    }
        //                }
        //                break;
        //            }
        //            #endregion
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 有符号short类型转无符号ushort
        /// </summary>
        /// <param name="Number">需转换的值</param>
        /// <returns>转化结果</returns>
        public static ushort ShortToUshort(short Number)
        {
            if (Number >= 0)
            {
                return (ushort)(Number);
            }
            else
            {
                return (ushort)(65536 - Math.Abs(Number));
            }
        }
        /// <summary>
        /// 无符号ushort转有符号short类型
        /// </summary>
        /// <param name="Number">需转换的值</param>
        /// <returns>转化结果</returns>
        public static short UshortToShort(ushort Number)
        {
            if (Number <= 32767)
            {
                return (short)Number;
            }
            else
            {
                return (short)(Number - 65536);
            }
        }

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
        public static int HexToInt(string Number)
        {
            long iNumber = Convert.ToInt64(Number, 16);

            if (iNumber <= 2147483647)
            {
                return (int)iNumber;
            }
            else
            {
                return (int)(iNumber - 4294967295);
            }
        }

        /// <summary>
        /// 十六进制转十进制，仅支持十六位
        /// </summary>
        /// <param name="Number">需要转换的字符串</param>
        /// <returns></returns>
        public static short HexToShort(string Number)
        {
            int iNumber = Convert.ToInt32(Number, 16);

            if (iNumber <= 32767)
            {
                return (short)iNumber;
            }
            else
            {
                return (short)(iNumber - 65536);
            }
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

        private static void CheckCRC(byte[] btVal, int iLength, out byte crcH, out byte crcL)
        {
            UInt16 rCRC;
            rCRC = 0xFFFF;
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
            crcH = (byte)((rCRC & 0xFF00) >> 8);
            crcL = (byte)(rCRC & 0x00FF);

        }

        private static int ReadDataBit(byte[] WriteData)
        {
            int RetBit = 0;
            if (WriteData[1] == 01)
            {
                if (int.Parse(WriteData[4].ToString() + WriteData[5].ToString()) % 8 != 0)
                {
                    RetBit = 5 + int.Parse(WriteData[4].ToString() + WriteData[5].ToString()) / 8 + 1;
                }
                else
                {
                    RetBit = 5 + int.Parse(WriteData[4].ToString() + WriteData[5].ToString()) / 8;
                }

            }
            else if (WriteData[1] == 03)
            {
                RetBit = 5 + int.Parse(WriteData[4].ToString() + WriteData[5].ToString()) * 2;
            }
            else if (WriteData[1] == 05)
            {
                RetBit = WriteData.Length;
            }
            else if (WriteData[1] == 06)
            {
                RetBit = WriteData.Length;
            }
            else if (WriteData[1] == 08)
            {
                RetBit = WriteData.Length;
            }
            return RetBit;
        }
    }
}
