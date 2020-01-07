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
    public partial class TestArm : Form
    {
        FlagEnum.TestArm TestArmName;
        public TestArm(FlagEnum.TestArm Test)
        {
            InitializeComponent();
            TestArmName = Test;
        }
        Timer tm;
        private void TestArm_Load(object sender, EventArgs e)
        {
            TestName.Text = TestArmName.ToString();

            tm = new Timer() { Interval = 100 };
            tm.Tick += Tm_Tick;
            tm.Start();
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            int index = (int)TestArmName;

            Function.Other.SetVable(Flag.TestArm[index].HeatMode == FlagEnum.HeatMode.ATC, true, false, ref this.AtcEnabled);

            this.IcEnabled.Checked = Flag.TestArm[index].Heat_IC.SetToHeat.IsUseHeat;

            if (Flag.TestArm[index].Heat_IC.SetToHeat.IsUseHeat == true)
            {
                this.HeatEnabled.Checked = Flag.TestArm[index].Heat_IC.SetToHeat.IsUseHeat && Flag.TestArm[index].Heat_IC.SetToHeat.Enabled;

                this.SetTemp.Text = Flag.TestArm[index].Heat_IC.SetToHeat.Temp.ToString();
                this.SetOfftk.Text = Flag.TestArm[index].Heat_IC.SetToHeat.OffTk.ToString();
                this.SetOffset.Text = Flag.TestArm[index].Heat_IC.SetToHeat.Offset.ToString();
                this.SetPower.Text = Flag.TestArm[index].Heat_IC.SetToHeat.PowerLimit.ToString();

                this.IsHeat.Checked = Flag.TestArm[index].Heat_IC.GetInData.IsRun;
                this.PV.Text = Flag.TestArm[index].Heat_IC.GetInData.PV.ToString("0.0");
                this.SP.Text = Flag.TestArm[index].Heat_IC.GetInData.SP.ToString("0.0");
                this.ErrCode.Text = Flag.TestArm[index].Heat_IC.GetInData.ErrCode.ToString();
            }
            else if (Flag.TestArm[index].Heat_Inside.SetToHeat.IsUseHeat == true)
            {
                this.HeatEnabled.Checked = Flag.TestArm[index].Heat_Inside.SetToHeat.IsUseHeat && Flag.TestArm[index].Heat_Inside.SetToHeat.Enabled;

                this.SetTemp.Text = Flag.TestArm[index].Heat_Inside.SetToHeat.Temp.ToString();
                this.SetOfftk.Text = Flag.TestArm[index].Heat_Inside.SetToHeat.OffTk.ToString();
                this.SetOffset.Text = Flag.TestArm[index].Heat_Inside.SetToHeat.Offset.ToString();
                this.SetPower.Text = Flag.TestArm[index].Heat_Inside.SetToHeat.PowerLimit.ToString();

                this.IsHeat.Checked = Flag.TestArm[index].Heat_Inside.GetInData.IsRun;
                this.PV.Text = Flag.TestArm[index].Heat_Inside.GetInData.PV.ToString("0.0");
                this.SP.Text = Flag.TestArm[index].Heat_Inside.GetInData.SP.ToString("0.0");
                this.ErrCode.Text = Flag.TestArm[index].Heat_Inside.GetInData.ErrCode.ToString();
            }
            else
            {
                this.IsHeat.Checked = Flag.TestArm[index].Heat_IC.GetInData.IsRun;
                this.PV.Text = Flag.TestArm[index].Heat_IC.GetInData.PV.ToString("0.0");
                this.SP.Text = Flag.TestArm[index].Heat_IC.GetInData.SP.ToString("0.0");
                this.ErrCode.Text = Flag.TestArm[index].Heat_IC.GetInData.ErrCode.ToString();
            }
        }

        private void TestArm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tm.Stop();
        }
    }
}
