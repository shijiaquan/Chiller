using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    public partial class AlmDisplay : Form
    {
        Stopwatch IntervalTime;

        public AlmDisplay()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x02000000;
                return cp;
            }
        }

        private void AlmDisplay_Load(object sender, EventArgs e)
        {
            this.Paragraph.Text = "Now";

            ProhibitHandlerControl.Checked = !Flag.Handler.ProhibitHandlerControl;

            IntervalTime = new Stopwatch();
            IntervalTime.Start();
            Flag.NowLogList.ResetDisplay += NowLogList_ResetDisplay;
            Flag.RunLogList.ResetDisplay += RunLogList_ResetDisplay;

            Flag.SystemThread.AlmDisplayUpData = new Thread(UpData) { IsBackground = true };
            Flag.SystemThread.AlmDisplayUpData.Start();
        }

        private void UpData()
        {
            Flag.SystemThread.AlmDisplayEnabled = true;
            while (Flag.SystemThread.AlmDisplayEnabled)
            {
                Thread.Sleep(200);

                #region 是否允许进行复位

                this.Invoke((MethodInvoker)delegate { this.Reset.Enabled = Flag.MenuChangeState; });

                #endregion
            }
            Flag.SystemThread.AlmDisplayEndState = true;
        }

        private void NowLogList_ResetDisplay()
        {
            if (this.NowLog.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.NowLog.Clear();
                });
            }
            else
            {
                this.NowLog.Clear();
            }

            for (int i = 0; i < Flag.NowLogList.Count; i++)
            {
               FlagStruct.LogBase Log = Flag.NowLogList.GetLog(i);
               Function.Log.Message(this.NowLog, Log.color, Log.time, Log.text);
            }
            Function.Log.End(this.NowLog);
        }

        private void RunLogList_ResetDisplay()
        {
            string NowState = "Now";

            if (this.Paragraph.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    NowState = this.Paragraph.Text;

                    if (Flag.AddLogList.Paragraph > 0 && Flag.AddLogList.Paragraph > this.Paragraph.Items.Count - 1)
                    {
                        this.Paragraph.Items.Clear();
                        for (int i = 1; i < Flag.AddLogList.Paragraph + 1; i++)
                        {
                            this.Paragraph.Items.Add((((i - 1) * 300) + 1).ToString("00000") + "—" + (i * 300).ToString("00000"));
                        }
                        this.Paragraph.Items.Add("Now");
                        this.Paragraph.Text = NowState;
                    }
                });
            }
            else
            {
                NowState = this.Paragraph.Text;
            }
            if (NowState == "Now")
            {
                for (int i = Flag.AddLogList.LastIndex; i < Flag.RunLogList.Count; i++)
                {
                    if (Flag.AddLogList.AdditionNum >= 300)
                    {
                        if (this.RunLog.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate { this.RunLog.Clear(); });
                        }
                        else
                        {
                            this.RunLog.Clear();
                        }
                        Flag.AddLogList.ClearAdditionNum();
                    }

                    FlagStruct.LogBase Log = Flag.RunLogList.GetLog(i);
                    Function.Log.Message(this.RunLog, Log.color, Log.time, Log.text);
                    Flag.AddLogList.Add(Log.time, Log.text, Log.color);
                }

                Function.Log.End(this.RunLog);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(ResetAlm) { IsBackground = true };
            th.Start();
        }

        private void ResetAlm()
        {
            Flag.MenuChangeState = false;

            Flag.RunLogList.Add("系统复位中……", Color.Orange);

            #region 复位Adam的连接状态
            for (int i = 0; i < Flag.Manufacturer.Network.Modular.Length; i++)
            {
                if (Adam.IsConnect(i) == false)
                {
                    Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "断线重接中……");
                    if (Adam.Reconnect(i) == true)
                    {
                        Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "连接成功！", Color.Green);
                    }
                    else
                    {
                        Flag.RunLogList.Add("远程IO模块" + (i + 1).ToString() + "连接失败！", Color.Red);
                    }
                }
            }
            #endregion

            #region 复位A6的连接状态

            bool IsConnent = false;

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                IsConnent = IsConnent || Flag.Unit[i].InternalPump.IsConnent;

                if (Flag.Unit[i].InternalPump.IsErr == true)
                {
                    for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                    {
                        if (Flag.Unit[i].InternalPump.ErrCode == PanasonicA6.A6_ErrCodeList[j].Code && PanasonicA6.A6_ErrCodeList[j].IsClear == true)
                        {
                            if (PanasonicA6.A6_ErrCodeList[j].IsClear == true)
                            {
                                Flag.RunLogList.Add("内循环泵" + (i + 1).ToString() + "报警清除中……");
                                if (Work.Pump.ClearErr(Flag.Unit[i].InternalPump.Address) == true)
                                {
                                    Flag.RunLogList.Add("内循环泵" + (i + 1).ToString() + "报警清除成功！", Color.Green);
                                }
                                else
                                {
                                    Flag.RunLogList.Add("内循环泵" + (i + 1).ToString() + "报警清除失败！", Color.Red);
                                }
                            }
                            else
                            {
                                Flag.RunLogList.Add("内循环泵" + (i + 1).ToString() + "报警代码：" + Flag.Unit[i].InternalPump.ErrCode + "无法清除！");
                            }

                        }
                    }
                }
            }

            for (int i = 0; i < Flag.ExternalPump.Length; i++)
            {
                IsConnent = IsConnent || Flag.ExternalPump[i].IsConnent;

                if (Flag.ExternalPump[i].IsErr == true)
                {
                    for (int j = 0; j < PanasonicA6.A6_ErrCodeList.Length; j++)
                    {
                        if (Flag.ExternalPump[i].ErrCode == PanasonicA6.A6_ErrCodeList[j].Code)
                        {
                            if (PanasonicA6.A6_ErrCodeList[j].IsClear == true)
                            {
                                Flag.RunLogList.Add("外循环泵" + (i + 1).ToString() + "报警清除中……");
                                if (Work.Pump.ClearErr(Flag.ExternalPump[i].Address) == true)
                                {
                                    Flag.RunLogList.Add("外循环泵" + (i + 1).ToString() + "报警清除成功！", Color.Green);
                                }
                                else
                                {
                                    Flag.RunLogList.Add("外循环泵" + (i + 1).ToString() + "报警清除失败！", Color.Red);
                                }
                            }
                            else
                            {
                                Flag.RunLogList.Add("外循环泵" + (i + 1).ToString() + "报警代码：" + Flag.ExternalPump[i].ErrCode + "无法清除！");
                            }
                        }
                    }
                }
            }

            if (IsConnent == false)
            {
                PanasonicA6.Open();
            }
            #endregion

            #region 复位Handler的连接状态
            if(Handler.NNodbusSlave!=null)
            {
                if (Handler.IsConnect == false)
                {
                    Handler.Open();
                }
            }
            else
            {
                if (Handler.Initialization("COM3", 115200) == true)
                {
                    Flag.RunLogList.Add("初始化Handler连接已完成，等待设备连接！", Color.Green);
                }
                else
                {
                    Flag.RunLogList.Add("初始化Handler连接失败！", Color.Red);
                }
            }
           
            #endregion

            Flag.MenuChangeState = true;
        }

        private void Paragraph_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Paragraph.Text == "Now")
            {
                this.RunLog.Clear();
                for (int i = Flag.AddLogList.Count - Flag.AddLogList.AdditionNum; i < Flag.AddLogList.Count; i++)
                {
                    FlagStruct.LogBase Log = Flag.AddLogList.GetLog(i);
                    Function.Log.Message(this.RunLog, Log.color, Log.time, Log.text);
                }

                for (int i = Flag.AddLogList.LastIndex; i < Flag.RunLogList.Count; i++)
                {
                    if (Flag.AddLogList.AdditionNum >= 300)
                    {
                        this.RunLog.Clear();
                        Flag.AddLogList.ClearAdditionNum();
                    }

                    FlagStruct.LogBase Log = Flag.RunLogList.GetLog(i);
                    Function.Log.Message(this.RunLog, Log.color, Log.time, Log.text);
                    Flag.AddLogList.Add(Log.time, Log.text, Log.color);
                }

                Function.Log.End(this.RunLog);
            }
            else
            {
                this.RunLog.Clear();
                string[] Index = Paragraph.Text.Split('—');
                for (int i = int.Parse(Index[0]); i < int.Parse(Index[1]); i++)
                {
                    FlagStruct.LogBase Log = Flag.AddLogList.GetLog(i);
                    Function.Log.Message(this.RunLog, Log.color, Log.time, Log.text);
                }
            }
        }

        private void ProhibitHandlerControl_CheckedChanged(object sender, EventArgs e)
        {
            Flag.Handler.ProhibitHandlerControl = !ProhibitHandlerControl.Checked;
        }

        private void ModbusRtuData_Click(object sender, EventArgs e)
        {
            ModbusRtuPrint fm = new ModbusRtuPrint();
            Chiller.Function.Win.OpenWindow(fm, this, fm.Text, false);
        }
    }
}
