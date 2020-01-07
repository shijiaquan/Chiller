using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    class Win
    {
        public delegate void SelectHandler(FromWindows OpenFrom);
        public static event SelectHandler ResetButton; //定义一个委托类型的事件  

        public static void OpenFrom(FromWindows From)
        {
            ResetButton(From);
        }

        public enum FromWindows
        {
            AlmDisplay = 1,
            InternalColdControl = 2,
            ExternalTempControl = 3,
            InternalColdState = 4,
            ExternalTempState = 5,
            UnitWorkState = 6,
            Diagnosis = 7,
            Manufacturer = 8,
        }

        /// <summary>
        /// 窗体函数结构
        /// </summary>
        public class FromBox
        {
            /// <summary>
            /// 信息显示页面
            /// </summary>
            public AlmDisplay AlmDisplayFrom;
            /// <summary>
            /// 内部液体制冷控制页面
            /// </summary>
            public InternalColdControl InternalColdControlFrom;
            /// <summary>
            /// 外部温度控制页面
            /// </summary>
            public ExternalTempControl ExternalTempControlFrom;
            /// <summary>
            /// 内部制冷状态监控页面
            /// </summary>
            public InternalColdState InternalColdStateFrom;
            /// <summary>
            /// 外部温度监控页面
            /// </summary>
            public ExternalTempState ExternalTempStateFrom;
            /// <summary>
            /// 压缩机运行状态监控页面
            /// </summary>
            public UnitWorkState UnitWorkStateFrom;
            /// <summary>
            /// 系统诊断页面
            /// </summary>
            public Diagnosis DiagnosisFrom;
            /// <summary>
            /// 厂商参数页面
            /// </summary>
            public Manufacturer ManufacturerFrom;
            /// <summary>
            /// 温度与偏移设定
            /// </summary>
            public static ExternalTempVarietiesData ExternalTestTempChangeFrom;

            public FromBox()
            {
                AlmDisplayFrom = new AlmDisplay();
                InternalColdControlFrom = new InternalColdControl();
                ExternalTempControlFrom = new ExternalTempControl();
                InternalColdStateFrom = new InternalColdState();
                ExternalTempStateFrom = new ExternalTempState();
                UnitWorkStateFrom = new UnitWorkState();
                DiagnosisFrom = new Diagnosis();
                ManufacturerFrom = new Manufacturer();
            }

        }

        public static FromBox MDIChild;

        public static DialogResult MessageBox(Control Box ,string Message,string Tip)
        {
            MessageDisplay Display = new MessageDisplay(Message, Tip);
            return Display.ShowDialog(Box);
        }
        public static DialogResult MessageBox( string Message, string Tip)
        {
            MessageDisplay Display = new MessageDisplay(Message, Tip);
            return Display.ShowDialog();
        }

        public static void MessageHeatState(Control Box)
        {
            if (Box != null)
            {
                Flag.StartEnabled.RunHeat.ChangeProgress = 0;
                Thread td = new Thread(OpenMessageHeatState) { IsBackground = true };
                td.Start(Box);
            }
        }
        private static void OpenMessageHeatState(object Box)
        {
            try
            {
                if ((Box as Control) != null)
                {
                    (Box as Control).Invoke((MethodInvoker)delegate
                    {
                        MessageDisplayHeat Display = new MessageDisplayHeat();
                        Display.ShowDialog((Box as Control));
                    });
                }
            }
            catch
            {

            }
        }
    }
}
