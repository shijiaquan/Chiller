using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chiller
{
    public class SystemFreeTime
    {
        private Thread FreeTimeThread;
        private Point LastPoint;
        private DateTime LastTime;
        private TimeSpan _FreeTimeSpan;
        private bool Lock = true;
        private bool _Enabled = false;

        /// <summary>
        /// 事件触发委托
        /// </summary>
        public delegate void TimeToAchieve();
        /// <summary>
        /// 达到时间触发的事件
        /// </summary>
        public event TimeToAchieve TimeToAchieveClick; //定义一个刷新界面的委托类型事件   
        /// <summary>
        /// 获取当前设定不动间隔触发事件的时间
        /// </summary>
        public int Interval
        {
            set;
            get;
        }

        /// <summary>
        /// 时间触发使能
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                LastTime = DateTime.Now;
                _Enabled = value;
            }
        }
        /// <summary>
        /// 获取当前鼠标停留不动间隔
        /// </summary>
        public int FreeTimeSpan
        {
            get
            {
                try

                {
                    return _FreeTimeSpan.Hours * 60 * 60 + _FreeTimeSpan.Minutes * 60 + _FreeTimeSpan.Seconds;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 初始化间隔触发事件
        /// </summary>
        /// <param name="Time">间隔时间</param>
        public SystemFreeTime(int Time)
        {
            Interval = Time;
            LastPoint = new Point();
            LastTime = new DateTime();
            _FreeTimeSpan = new TimeSpan();

            FreeTimeThread = new Thread(FreeTimeThreadRun) { IsBackground = true };
            FreeTimeThread.Start();
        }

        /// <summary>
        /// 初始化间隔触发事件
        /// </summary>
        public SystemFreeTime()
        {
            Interval = 0;
            LastPoint = new Point();
            LastTime = new DateTime();
            _FreeTimeSpan = new TimeSpan();

            FreeTimeThread = new Thread(FreeTimeThreadRun) { IsBackground = true };
            FreeTimeThread.Start();
        }

        private void FreeTimeThreadRun()
        {
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    Point NowPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);

                    if ((LastPoint.X == NowPoint.X) && (LastPoint.Y == NowPoint.Y) && Lock)
                    {
                        LastTime = DateTime.Now;
                        Lock = false;
                    }

                    if (LastPoint.X != NowPoint.X || LastPoint.Y != NowPoint.Y)
                    {
                        LastPoint = NowPoint;
                        LastTime = DateTime.Now;
                        Lock = true;
                    }

                    _FreeTimeSpan = DateTime.Now.Subtract(LastTime);
                    if (_Enabled == true)
                    {
                        if ((_FreeTimeSpan.Hours * 60 * 60 + _FreeTimeSpan.Minutes * 60 + _FreeTimeSpan.Seconds) > Interval)
                        {
                            TimeToAchieveClick();
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
