using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DewPointSensor;
using System.IO.Ports;

namespace Chiller
{
    public partial class Home : Form
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                //设置应用程序处理异常方式：ThreadException处理
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // 注册控件示例，如果注册失败，你的控件仍然只能使用8个小时
                HslControls.Authorization.SetAuthorizationCode("e62046f4-138e-4a92-af2b-a474c241db8b");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Home());
            }
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, string.Empty);
                MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        private static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            Win.MDIChild = new Win.FromBox();
            Win.ResetButton += Win_ResetButton;
            Win.OpenFrom(Win.FromWindows.AlmDisplay);

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 10 };
            t.Tick += DisplayDateTime;
            t.Enabled = true;

            this.UserGroup.Text = "None";

            Flag.NowLogList.InitiLog();
            Flag.RunLogList.InitiLog();

            Thread Load = new Thread(Initialization) { IsBackground = true };
            Load.Start();
        }

        private void Initialization()
        {
            Flag.RunLogList.Add("系统被开启！", Color.Black);

            //SerialPort SP = new SerialPort("COM4", 19200, Parity.None, 8, StopBits.One);
            //SP.Open();

            this.Invoke((MethodInvoker)delegate { this.FunctionBase.Enabled = false; });

            Function.Other.SetDateTimeFormat();

            Flag.RunLogList.Add("系统正在执行初始化……", Color.Black);

            Flag.Initialization();
            Flag.LoadData();
            Flag.SaveData();

            Flag.RunLogList.Add("系统数据加载完成！", Color.Green);

            Flag.RunLogList.Add("开始进行远程模块连接……", Color.Black);

            #region 初始化网络IP 研华远程IO模块，并开启线程读取远程IO模块状态以及数据
            bool[] state = Work.Initial.Initialization_Adam();
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == true)
                {
                    Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "连接完成！", Color.Green);
                }
                else
                {
                    Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "连接失败！", Color.Red);
                }
            }
            #endregion

            #region 初始化COM1-RS485 松下A6驱动器串口，并开启线程读取驱动器状态
            if (PanasonicA6.Initialization("COM1", 115200, Flag.Unit.Length + Flag.ExternalPump.Length) == true)
            {
                Flag.RunLogList.Add("初始化循环泵串口完成！", Color.Black);

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Work.Pump.SetServo(Flag.Unit[i].InternalPump.Address, true) == true &&
                        Work.Pump.TAcc(Flag.Unit[i].InternalPump.Address, Flag.Unit[i].InternalPump.Tacc) &&
                        Work.Pump.TDec(Flag.Unit[i].InternalPump.Address, Flag.Unit[i].InternalPump.Tdec))
                    {
                        Flag.RunLogList.Add("内循环泵：" + (i + 1).ToString() + "驱动器连接成功！", Color.Green);
                    }
                    else
                    {
                        Flag.RunLogList.Add("内循环泵：" + (i + 1).ToString() + "驱动器连接失败！", Color.Red);
                    }
                }

                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Work.Pump.SetServo(Flag.ExternalPump[i].Address, true) == true &&
                        Work.Pump.TAcc(Flag.ExternalPump[i].Address, Flag.ExternalPump[i].Tacc) &&
                        Work.Pump.TDec(Flag.ExternalPump[i].Address, Flag.ExternalPump[i].Tdec))
                    {
                        Flag.RunLogList.Add("外循环泵：" + (i + 1).ToString() + "驱动器连接成功！", Color.Green);
                    }
                    else
                    {
                        Flag.RunLogList.Add("外循环泵：" + (i + 1).ToString() + "驱动器连接失败！", Color.Red);
                    }
                }
            }
            else
            {
                Flag.RunLogList.Add("初始化循环泵串口失败！", Color.Red);

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    Flag.RunLogList.Add("内循环泵：" + (i + 1).ToString() + "驱动器连接失败！", Color.Red);
                }
                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    Flag.RunLogList.Add("外循环泵：" + (i + 1).ToString() + "驱动器连接失败！", Color.Red);
                }
            }
            #endregion

            #region 初始化COM2-RS485 PMA温控器串口，并开启线程读取温控器状态
            byte[] PMA_Length = new byte[2] { 22, 24 };
            if (PMA.Initialization("COM2", 38400) == true)
            {
                for (int i = 0; i < 2; i++)
                {
                    Thread.Sleep(100);
                    if (PMA.StopAllChannel((byte)(i + 1), PMA_Length[i] ) == true)
                    {
                        Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：温控器连接完成！", Color.Green);
                    }
                    else
                    {
                        Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：温控器连接失败！", Color.Red);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：温控器连接失败！", Color.Red);
                }
            }

            #endregion

            #region 初始化COM3-RS232 Handler串口，并开启接收中断，与之进行交互（被动模式）
            if (Handler.Initialization("COM3", 115200) == true)
            {
                Flag.RunLogList.Add("初始化Handler连接已完成，等待设备连接！", Color.Green);
            }
            else
            {
                Flag.RunLogList.Add("初始化Handler连接失败！", Color.Red);
            }
            #endregion

            #region 初始化COM4-RS485 露点传感器串口，并开启线程读取湿度状态状态
            int[] Station = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            if (HTU21D.Initialization("COM4", 19200, Station) == true)
            {
                for (int i = 0; i < HTU21D.ConnectNumber(); i++)
                {
                    float Temp = 999;
                    float Hum = 999;
                    float Dew = 999;
                    bool IsConnect = true;
                    IsConnect = IsConnect && HTU21D.GetTemperature(i + 1, ref Temp);
                    IsConnect = IsConnect && HTU21D.GetHumidity(i + 1, ref Hum);
                    IsConnect = IsConnect && HTU21D.GetDewPoint(i + 1, ref Dew);
                    if (IsConnect == true && Temp != 999 && Hum != 999 && Dew != 999)
                    {
                        Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：露点传感器连接完成！", Color.Green);
                    }
                    else
                    {
                        Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：露点传感器连接失败！", Color.Red);
                    }
                }

            }
            else
            {
                for (int i = 0; i < HTU21D.ConnectNumber(); i++)
                {
                    Flag.RunLogList.Add("初始化站号" + (i + 1).ToString() + "：露点传感器连接失败！", Color.Red);
                }
            }
            #endregion

            Work.Initial.Initialization_Thread();
            TempControl.ExternalTempControl.Initialization();
            TempControl.Colding.Initialization();
            TempControl.Alarm.Initialization();

            Flag.RunLogList.Add("设备初始化结束！", Color.Black);

            Flag.SystemThread.HomeUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.HomeEnabled = true;
            Flag.SystemThread.HomeUpData.Start();

            Flag.LogOnUser.FreeTime = new SystemFreeTime();
            Flag.LogOnUser.FreeTime.TimeToAchieveClick += FreeTime_TimeToAchieveClick;
            Flag.LogOnUser.FreeTime.Enabled = false;

            Flag.MenuChangeState = true;

            Thread.Sleep(2000);

            Work.Initial.SenserDetection(true);

        }

        private void UpdateFrom()
        {
            Stopwatch Sw = new Stopwatch();
            Sw.Start();

            while (Flag.SystemThread.HomeEnabled)
            {
                Thread.Sleep(100);

                #region 是否允许进行菜单切换

                this.Invoke((MethodInvoker)delegate { this.FunctionBase.Enabled = Flag.MenuChangeState; });

                #endregion

                #region 显示用户空闲时间
                this.Invoke((MethodInvoker)delegate
                {
                    if (Flag.LogOnUser.FreeTime.Enabled == true)
                    {
                        this.FreeTimeDisplay.Text = Flag.LogOnUser.FreeTime.FreeTimeSpan.ToString();
                    }
                    else
                    {
                        this.FreeTimeDisplay.Text = "";
                    }
                });
                #endregion

                #region 判断Adam的连接状态
                for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
                {
                    if (Adam.IsConnect(i) == false)
                    {
                        if (Flag.NowLogList.Contains("远程IO模块" + (i + 1).ToString() + "连接失败！") == false)
                        {
                            Flag.NowLogList.Add("远程IO模块" + (i + 1).ToString() + "连接失败！", Color.Red);
                        }
                    }
                    else
                    {
                        if (Flag.NowLogList.Contains("远程IO模块" + (i + 1).ToString() + "连接失败！") == true)
                        {
                            Flag.NowLogList.Remove("远程IO模块" + (i + 1).ToString() + "连接失败！");
                        }
                    }
                }
                #endregion

                #region 判断A6的连接状态

                for (int i = 0; i < Flag.Unit.Length; i++)
                {
                    if (Flag.Unit[i].InternalPump.IsConnent == false)
                    {
                        if (Flag.NowLogList.Contains("内循环泵" + (i + 1).ToString() + "连接失败！") == false)
                        {
                            Flag.NowLogList.Add("内循环泵" + (i + 1).ToString() + "连接失败！", Color.Red);
                        }
                    }
                    else
                    {
                        if (Flag.NowLogList.Contains("内循环泵" + (i + 1).ToString() + "连接失败！") == true)
                        {
                            Flag.NowLogList.Remove("内循环泵" + (i + 1).ToString() + "连接失败！");
                        }
                    }

                    if (Flag.Unit[i].InternalPump.IsErr == true)
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (PanasonicA6.A6_ErrCodeList[j].Code == Flag.Unit[i].InternalPump.ErrCode)
                            {
                                if (Flag.NowLogList.Contains("内循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code) == false)
                                {
                                    Flag.NowLogList.Add("内循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code, Color.Red);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (Flag.NowLogList.Contains("内循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code) == true)
                            {
                                Flag.NowLogList.Remove("内循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code);
                            }
                        }
                    }
                }

                for (int i = 0; i < Flag.ExternalPump.Length; i++)
                {
                    if (Flag.ExternalPump[i].IsConnent == false)
                    {
                        if (Flag.NowLogList.Contains("外循环泵" + (i + 1).ToString() + "连接失败！") == false)
                        {
                            Flag.NowLogList.Add("外循环泵" + (i + 1).ToString() + "连接失败！", Color.Red);
                        }
                    }
                    else
                    {
                        if (Flag.NowLogList.Contains("外循环泵" + (i + 1).ToString() + "连接失败！") == true)
                        {
                            Flag.NowLogList.Remove("外循环泵" + (i + 1).ToString() + "连接失败！");
                        }
                    }

                    if (Flag.ExternalPump[i].IsErr == true)
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (PanasonicA6.A6_ErrCodeList[j].Code == Flag.ExternalPump[i].ErrCode)
                            {
                                if (Flag.NowLogList.Contains("外循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code) == false)
                                {
                                    Flag.NowLogList.Add("外循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code, Color.Red);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                        {
                            if (Flag.NowLogList.Contains("外循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code) == true)
                            {
                                Flag.NowLogList.Remove("外循环泵" + (i + 1).ToString() + "驱动器报警！" + "报警代码：" + PanasonicA6.A6_ErrCodeList[j].Code);
                            }
                        }
                    }
                }
                #endregion

                #region 判断PMA的连接状态


                #endregion

                #region 判断Handler的连接状态
                if (Flag.Handler.IsHandlerControl == false)
                {
                    if (Flag.NowLogList.Contains("Handler未取得控制权！") == false)
                    {
                        Flag.NowLogList.Add("Handler未取得控制权！", Color.Red);
                        Flag.NowLogList.Remove("Handler取得控制权！");
                        Flag.RunLogList.Add("Handler已断开！", Color.Red);
                    }
                }
                else
                {
                    if (Flag.NowLogList.Contains("Handler取得控制权！") == false)
                    {
                        Flag.NowLogList.Remove("Handler未取得控制权！");
                        Flag.NowLogList.Add("Handler取得控制权！",Color.Green);
                        Flag.RunLogList.Add("Handler取得控制权！", Color.Green);
                    }
                }
                #endregion

                #region 判断露点传感器的连接状态
                for (int i = 0; i < HTU21D.ConnectNumber(); i++)
                {
                    if (HTU21D.IsConnect(i + 1) == false)
                    {
                        if (Flag.NowLogList.Contains("站号" + (i + 1).ToString() + "：露点传感器当前未连接！") == false)
                        {
                            Flag.NowLogList.Add("站号" + (i + 1).ToString() + "：露点传感器当前未连接！", Color.Red);
                        }
                    }
                    else
                    {
                        if (Flag.NowLogList.Contains("站号" + (i + 1).ToString() + "：露点传感器当前未连接！") == true)
                        {
                            Flag.NowLogList.Remove("站号" + (i + 1).ToString() + "：露点传感器当前未连接！");
                        }
                    }
                }
                #endregion

                #region 显示制冷机组开启状态

                if (Flag.StartEnabled.RunCold.Colding == true)
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.Unit[i].Enabled == true)
                        {
                            if (Flag.Unit[i].StartErr == true)
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷开启失败！") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "制冷开启失败！", Color.Red);
                                }
                            }
                            else
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷开启失败！") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷开启失败！");
                                }
                            }
                        }
                        else
                        {
                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷开启失败！") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷开启失败！");
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷开启失败！") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷开启失败！");
                        }
                    }
                }

                #endregion

                #region 显示制冷机组停止状态

                if (Flag.StartEnabled.RunCold.Colding == false)
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.Unit[i].Enabled == true)
                        {
                            if (Flag.Unit[i].EndErr == true)
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷关闭失败！") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "制冷关闭失败！", Color.Red);
                                }
                            }
                            else
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷关闭失败！") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷关闭失败！");
                                }
                            }
                        }
                        else
                        {
                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷关闭失败！") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷关闭失败！");
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "制冷关闭失败！") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "制冷关闭失败！");
                        }
                    }
                }

                #endregion

                #region 显示制冷机组温度状态

                if (Flag.StartEnabled.RunCold.Colding == true)
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        if (Flag.Unit[i].Enabled == true)
                        {
                            #region 排气温度状态显示

                            if (Flag.Unit[i].Compressor.Out.Temp.HightTempGrade == true)
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度正常！") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度正常！");
                                }
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……", Color.Red);
                                }
                            }
                            else
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度正常！") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "排气温度正常！", Color.Green);
                                }
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……");
                                }
                              
                            }

                            #endregion

                            #region 冷凝温度状态显示

                            if (Flag.Unit[i].Condenser.Out.Temp.HightTempGrade == true)
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度正常！") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度正常！");
                                }

                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！",Color.Red);
                                }

                            }
                            else
                            {
                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度正常！") == false)
                                {
                                    Flag.NowLogList.Add("机组" + (i + 1).ToString() + "冷凝温度正常！", Color.Green);
                                }

                                if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！") == true)
                                {
                                    Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！");
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 移除排气温度显示

                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度正常！") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度正常！");
                            }

                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……");
                            }

                            #endregion

                            #region 移除冷凝温度显示

                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度正常！") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度正常！");
                            }

                            if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！") == true)
                            {
                                Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！");
                            }

                            #endregion
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Flag.Unit.Length; i++)
                    {
                        #region 移除排气温度显示

                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度正常！") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度正常！");
                        }

                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "排气温度已超限，已停止降温……");
                        }

                        #endregion

                        #region 移除冷凝温度显示

                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度正常！") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度正常！");
                        }

                        if (Flag.NowLogList.Contains("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！") == true)
                        {
                            Flag.NowLogList.Remove("机组" + (i + 1).ToString() + "冷凝温度已超限，已停止降温，请检测冷水机状态！");
                        }

                        #endregion
                    }
                }

                #endregion
            }
            Flag.SystemThread.HomeEndState = true;
        }

        private void DisplayDateTime(object sender, EventArgs e)
        {
            this.DisplayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.DisplayTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Win_ResetButton(Win.FromWindows OpenFrom)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    switch (OpenFrom)
                    {
                        case Win.FromWindows.AlmDisplay:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.AlmDisplayFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Blue;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.InternalColdControl:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.InternalColdControlFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Blue;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.ExternalTempControl:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.ExternalTempControlFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Blue;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.InternalColdState:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.InternalColdStateFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Blue;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.ExternalTempState:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.ExternalTempStateFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Blue;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;
                        case Win.FromWindows.UnitWorkState:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.UnitWorkStateFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Blue;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.Diagnosis:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.DiagnosisFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Blue;
                            this.Manufacturer.OriginalColor = Color.Gray;

                            break;

                        case Win.FromWindows.Manufacturer:

                            Win.MDIChild.AlmDisplayFrom.Hide();
                            Win.MDIChild.InternalColdControlFrom.Hide();
                            Win.MDIChild.ExternalTempControlFrom.Hide();
                            Win.MDIChild.InternalColdStateFrom.Hide();
                            Win.MDIChild.ExternalTempStateFrom.Hide();
                            Win.MDIChild.UnitWorkStateFrom.Hide();
                            Win.MDIChild.DiagnosisFrom.Hide();
                            Win.MDIChild.ManufacturerFrom.Hide();

                            Win.MDIChild.ManufacturerFrom.Show(this);

                            this.OpenAlmDisplay.OriginalColor = Color.Gray;
                            this.OpenInternalColdControl.OriginalColor = Color.Gray;
                            this.OpenExternalTempControl.OriginalColor = Color.Gray;
                            this.OpenInternalColdState.OriginalColor = Color.Gray;
                            this.OpenExternalTempState.OriginalColor = Color.Gray;
                            this.OpenUnitWorkState.OriginalColor = Color.Gray;
                            this.OpenDiagnosis.OriginalColor = Color.Gray;
                            this.Manufacturer.OriginalColor = Color.Blue;
                            break;
                    }
                });
            }
            else
            {
                switch (OpenFrom)
                {
                    case Win.FromWindows.AlmDisplay:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.AlmDisplayFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Blue;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.InternalColdControl:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.InternalColdControlFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Blue;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.ExternalTempControl:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.ExternalTempControlFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Blue;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.InternalColdState:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.InternalColdStateFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Blue;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.ExternalTempState:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.ExternalTempStateFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Blue;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;
                    case Win.FromWindows.UnitWorkState:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.UnitWorkStateFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Blue;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.Diagnosis:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.DiagnosisFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Blue;
                        this.Manufacturer.OriginalColor = Color.Gray;

                        break;

                    case Win.FromWindows.Manufacturer:

                        Win.MDIChild.AlmDisplayFrom.Hide();
                        Win.MDIChild.InternalColdControlFrom.Hide();
                        Win.MDIChild.ExternalTempControlFrom.Hide();
                        Win.MDIChild.InternalColdStateFrom.Hide();
                        Win.MDIChild.ExternalTempStateFrom.Hide();
                        Win.MDIChild.UnitWorkStateFrom.Hide();
                        Win.MDIChild.DiagnosisFrom.Hide();
                        Win.MDIChild.ManufacturerFrom.Hide();

                        Win.MDIChild.ManufacturerFrom.Show(this);

                        this.OpenAlmDisplay.OriginalColor = Color.Gray;
                        this.OpenInternalColdControl.OriginalColor = Color.Gray;
                        this.OpenExternalTempControl.OriginalColor = Color.Gray;
                        this.OpenInternalColdState.OriginalColor = Color.Gray;
                        this.OpenExternalTempState.OriginalColor = Color.Gray;
                        this.OpenUnitWorkState.OriginalColor = Color.Gray;
                        this.OpenDiagnosis.OriginalColor = Color.Gray;
                        this.Manufacturer.OriginalColor = Color.Blue;
                        break;
                }
            }
        }

        private void Function_Click(object sender, EventArgs e)
        {
            if ((sender as HslControls.HslButton).Name == "OpenAlmDisplay")
            {
                Win.OpenFrom(Win.FromWindows.AlmDisplay);
            }
            if ((sender as HslControls.HslButton).Name == "OpenInternalColdControl")
            {
                Win.OpenFrom(Win.FromWindows.InternalColdControl);
            }
            else if ((sender as HslControls.HslButton).Name == "OpenExternalTempControl")
            {
                Win.OpenFrom(Win.FromWindows.ExternalTempControl);
            }
            else if ((sender as HslControls.HslButton).Name == "OpenInternalColdState")
            {
                Win.OpenFrom(Win.FromWindows.InternalColdState);
            }
            else if ((sender as HslControls.HslButton).Name == "OpenExternalTempState")
            {
                Win.OpenFrom(Win.FromWindows.ExternalTempState);
            }
            else if ((sender as HslControls.HslButton).Name == "OpenUnitWorkState")
            {
                Win.OpenFrom(Win.FromWindows.UnitWorkState);
            }
            else if ((sender as HslControls.HslButton).Name == "OpenDiagnosis")
            {
                Win.OpenFrom(Win.FromWindows.Diagnosis);
            }
            else if ((sender as HslControls.HslButton).Name == "Manufacturer")
            {
                Win.OpenFrom(Win.FromWindows.Manufacturer);
            }
            else if ((sender as HslControls.HslButton).Name == "ExitSystem")
            {
                if (Flag.SystemThread.SystemExitState != null)
                {
                    return;
                }
                if (Flag.StartEnabled.RunCold.Colding == true)
                {
                    Function.Other.Message("制冷系统正在运行，请优先关系制冷系统！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Flag.StartEnabled.RunHeat.Heating == true)
                {
                    Function.Other.Message("加热系统正在运行，请优先关系加热系统！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Win.MessageBox(this, "请确认是否需要关闭系统？" + "\r\n" + "点击Yes确认关闭." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
                {
                    Win.OpenFrom(Win.FromWindows.AlmDisplay);
                    Flag.SystemThread.SystemExitState = new Thread(SystemExit) { IsBackground = true };
                    Flag.SystemThread.SystemExitState.Start();
                }
            }
        }

        private void UserGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (UserGroup.Text == "None")
            {
                Flag.LogOnUser.IsAutoLogout = false;
                Flag.LogOnUser.AutoLogoutTime = 0;
                Flag.LogOnUser.UserName = "";
                Flag.LogOnUser.UserAuthority = FlagEnum.UserAuthority.None;

                Flag.LogOnUser.FreeTime.Enabled = Flag.LogOnUser.IsAutoLogout;
                Flag.LogOnUser.FreeTime.Interval = Flag.LogOnUser.AutoLogoutTime;

                this.UserGroup.Text = "None";
                return;
            }

            Logon fm = new Logon
            {
                TopMost = true,
                ShowIcon = true,
                ShowInTaskbar = false,
                MaximizeBox = false,
                MinimizeBox = false,

                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                StartPosition = FormStartPosition.CenterScreen
            };

            fm.User_Name.Text = UserGroup.Text;

            if (fm.ShowDialog(this) == DialogResult.OK)
            {
                Flag.LogOnUser.IsAutoLogout = fm.IsAutoLogout;
                Flag.LogOnUser.AutoLogoutTime = fm.AutoLogoutTime * 60;
                Flag.LogOnUser.UserName = fm.ReturnUserName;
                Flag.LogOnUser.UserAuthority = fm.ReturnUserAuthority;

                Flag.LogOnUser.FreeTime.Enabled = Flag.LogOnUser.IsAutoLogout;
                Flag.LogOnUser.FreeTime.Interval = Flag.LogOnUser.AutoLogoutTime;
            }
            else
            {
                Flag.LogOnUser.IsAutoLogout = false;
                Flag.LogOnUser.AutoLogoutTime = 0;
                Flag.LogOnUser.UserName = "";
                Flag.LogOnUser.UserAuthority = FlagEnum.UserAuthority.None;

                Flag.LogOnUser.FreeTime.Enabled = Flag.LogOnUser.IsAutoLogout;
                Flag.LogOnUser.FreeTime.Interval = Flag.LogOnUser.AutoLogoutTime;

                this.UserGroup.Text = "None";
            }

        }

        private void FreeTime_TimeToAchieveClick()
        {
            try
            {
                Flag.LogOnUser.FreeTime.Enabled = false;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.UserGroup.Text = "None";
                    });
                }
                else
                {
                    this.UserGroup.Text = "None";
                }
            }
            catch (Exception ex)
            {
                GetExceptionMsg(ex, ex.ToString());
            }
        }

        private void SystemExit()
        {
            
            Flag.SystemThread.HomeEndState = false;
            Flag.SystemThread.AlmDisplayEndState = false;
            Flag.SystemThread.InternalColdControlEndState = false;
            Flag.SystemThread.InternalColdStateEndState = false;
            Flag.SystemThread.ExternalTempControlEndState = false;
            Flag.SystemThread.ExternalTempStateEndState = false;
            Flag.SystemThread.UnitWorkStateEndState = false;
            Flag.SystemThread.DiagnosisEndState = false;

            Flag.SystemThread.HomeEnabled = false;
            Flag.SystemThread.AlmDisplayEnabled = false;
            Flag.SystemThread.InternalColdControlEnabled = false;
            Flag.SystemThread.InternalColdStateEnabled = false;
            Flag.SystemThread.ExternalTempControlEnabled = false;
            Flag.SystemThread.ExternalTempStateEnabled = false;
            Flag.SystemThread.UnitWorkStateEnabled = false;
            Flag.SystemThread.DiagnosisEnabled = false;

            Flag.SaveData();

            Flag.RunLogList.Add("正在等待系统线程退出中……");

            int ExitThreadNum = 0;
            int ExitDisplayLock = -1;

            Stopwatch Sw = new Stopwatch();
            Sw.Restart();

            do
            {
                Thread.Sleep(10);

                ExitThreadNum = 0;

                if (Flag.SystemThread.HomeUpData != null)
                {
                    if (Flag.SystemThread.HomeEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.AlmDisplayUpData != null)
                {
                    if (Flag.SystemThread.AlmDisplayEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.InternalColdControlUpData != null)
                {
                    if (Flag.SystemThread.InternalColdControlEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.InternalColdStateUpData != null)
                {
                    if (Flag.SystemThread.InternalColdStateEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.ExternalTempControlUpData != null)
                {
                    if (Flag.SystemThread.ExternalTempControlEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.ExternalTempStateUpData != null)
                {
                    if (Flag.SystemThread.ExternalTempStateEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.UnitWorkStateUpData != null)
                {
                    if (Flag.SystemThread.UnitWorkStateEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }

                if (Flag.SystemThread.DiagnosisUpData != null)
                {
                    if (Flag.SystemThread.DiagnosisEndState)
                    {
                        ExitThreadNum++;
                    }
                }
                else
                {
                    ExitThreadNum++;
                }
                if (ExitDisplayLock != ExitThreadNum)
                {
                    ExitDisplayLock = ExitThreadNum;
                    Flag.RunLogList.Add("系统线程已退出 8/" + ExitThreadNum.ToString() + "……");
                }

                if (Sw.ElapsedMilliseconds >= 30000)
                {
                    Flag.RunLogList.Add("系统自动退出超时，程序将强制退出！",Color.Red);

                    Thread.Sleep(1000);

                    Application.Exit();
                }
            }
            while (ExitThreadNum < 8);
            Flag.RunLogList.Add("系统线程已退出完成！");

            Flag.RunLogList.Add("正在关闭系统！");

            Thread.Sleep(1000);

            Application.Exit();
        }

    }
}
