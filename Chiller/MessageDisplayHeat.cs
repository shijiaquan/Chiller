using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    public partial class MessageDisplayHeat : Form
    {
        public MessageDisplayHeat()
        {
            InitializeComponent();
            this.HeatSetProgressLine.Value = 0;
        }
        Timer tm;
        private void MessageDisplayHeat_Load(object sender, EventArgs e)
        {
           
            this.HeatSetText.Text = "正在设定加热器…………";
            
            tm = new Timer();
            tm.Interval = 10;
            tm.Tick += Tm_Tick;
            tm.Start();
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();

            if (Flag.StartEnabled.RunHeat.ChangeProgress > this.HeatSetProgressLine.Maximum)
            {
                Flag.StartEnabled.RunHeat.ChangeProgress = this.HeatSetProgressLine.Maximum;
            }
            this.HeatSetProgressLine.Value = Flag.StartEnabled.RunHeat.ChangeProgress;

            if (this.HeatSetProgressLine.Value == 100)
            {
                Flag.StartEnabled.RunHeat.ChangeProgress = 0;
                Timer tm2 = new Timer();
                tm2.Tick += Tm2_Tick;
                tm2.Interval = 500;
                tm2.Start();
            }
            else
            {
                tm.Start();
            }
        }

        private void Tm2_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            this.Close();
        }
    }
}
