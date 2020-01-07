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
    public partial class ExternalTempState : Form
    {
        public ExternalTempState()
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

        private void ExternalTempState_Load(object sender, EventArgs e)
        {
            Flag.SystemThread.ExternalTempStateUpData = new Thread(UpdateFrom) { IsBackground = true };
            Flag.SystemThread.ExternalTempStateEnabled = true;
            Flag.SystemThread.ExternalTempStateUpData.Start();
        }

        private void UpdateFrom()
        {
            while (Flag.SystemThread.ExternalTempStateEnabled)
            {
                Thread.Sleep(100);
                this.Invoke((MethodInvoker)delegate
                {


                });
            }
            Flag.SystemThread.ExternalTempStateEndState = true;
        }
    }
}
