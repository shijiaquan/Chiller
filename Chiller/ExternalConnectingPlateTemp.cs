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
    public partial class ExternalConnectingPlateTemp : Form
    {
        public ExternalConnectingPlateTemp()
        {
            InitializeComponent();
        }

        private void ExternalConnectingPlateTemp_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer TM = new Timer();
            TM.Tick += TM_Tick;
            TM.Enabled = true;
            TM.Interval = 500;
            TM.Start();
        }

        private void TM_Tick(object sender, EventArgs e)
        {
            this.TestArm1.Text = "TestArm 1 ：" + Flag.OtherTemp[0].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm2.Text = "TestArm 2 ：" + Flag.OtherTemp[1].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm3.Text = "TestArm 3 ：" + Flag.OtherTemp[2].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm4.Text = "TestArm 4 ：" + Flag.OtherTemp[3].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm5.Text = "TestArm 5 ：" + Flag.OtherTemp[4].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm6.Text = "TestArm 6 ：" + Flag.OtherTemp[5].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm7.Text = "TestArm 7 ：" + Flag.OtherTemp[6].Heat.GetInData.PV.ToString("0.0") + "℃";
            this.TestArm8.Text = "TestArm 8 ：" + Flag.OtherTemp[7].Heat.GetInData.PV.ToString("0.0") + "℃";
        }

        private void LookClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
