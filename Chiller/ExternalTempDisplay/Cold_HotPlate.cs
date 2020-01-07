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
    public partial class Cold_HotPlate : Form
    {
        FlagEnum.HeatPlate Plate;
        public Cold_HotPlate(FlagEnum.HeatPlate HeatPlate)
        {
            InitializeComponent();
            Plate = HeatPlate;
        }

        private void Cold_HotPlate_Load(object sender, EventArgs e)
        {
            TestName.Text = Plate.ToString();
            Timer tm = new Timer() { Interval = 100 };
            tm.Tick += Tm_Tick;
            tm.Start();
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            switch (Plate)
            {
                case FlagEnum.HeatPlate.ColdPlate1:
                    this.HeatEnabled.Checked = Flag.ColdPlate[0].Heat.SetToHeat.IsUseHeat && Flag.ColdPlate[0].Heat.SetToHeat.Enabled;
                    this.SetTemp.Text = Flag.ColdPlate[0].Heat.SetToHeat.Temp.ToString();
                    this.SetOfftk.Text = Flag.ColdPlate[0].Heat.SetToHeat.OffTk.ToString();
                    this.SetOffset.Text = Flag.ColdPlate[0].Heat.SetToHeat.Offset.ToString();
                    this.SetPower.Text = Flag.ColdPlate[0].Heat.SetToHeat.PowerLimit.ToString();

                    this.IsHeat.Checked = Flag.ColdPlate[0].Heat.GetInData.IsRun;
                    this.PV.Text = Flag.ColdPlate[0].Heat.GetInData.PV.ToString();
                    this.SP.Text = Flag.ColdPlate[0].Heat.GetInData.SP.ToString();
                    this.ErrCode.Text = Flag.ColdPlate[0].Heat.GetInData.ErrCode.ToString();
                    break;

                case FlagEnum.HeatPlate.ColdPlate2:
                    this.HeatEnabled.Checked = Flag.ColdPlate[1].Heat.SetToHeat.IsUseHeat && Flag.ColdPlate[1].Heat.SetToHeat.Enabled;
                    this.SetTemp.Text = Flag.ColdPlate[1].Heat.SetToHeat.Temp.ToString();
                    this.SetOfftk.Text = Flag.ColdPlate[1].Heat.SetToHeat.OffTk.ToString();
                    this.SetOffset.Text = Flag.ColdPlate[1].Heat.SetToHeat.Offset.ToString();
                    this.SetPower.Text = Flag.ColdPlate[1].Heat.SetToHeat.PowerLimit.ToString();

                    this.IsHeat.Checked = Flag.ColdPlate[1].Heat.GetInData.IsRun;
                    this.PV.Text = Flag.ColdPlate[1].Heat.GetInData.PV.ToString();
                    this.SP.Text = Flag.ColdPlate[1].Heat.GetInData.SP.ToString();
                    this.ErrCode.Text = Flag.ColdPlate[1].Heat.GetInData.ErrCode.ToString();
                    break;

                case FlagEnum.HeatPlate.HotPlate1:
                    this.HeatEnabled.Checked = Flag.HotPlate[0].Heat.SetToHeat.IsUseHeat && Flag.HotPlate[0].Heat.SetToHeat.Enabled;
                    this.SetTemp.Text = Flag.HotPlate[0].Heat.SetToHeat.Temp.ToString();
                    this.SetOfftk.Text = Flag.HotPlate[0].Heat.SetToHeat.OffTk.ToString();
                    this.SetOffset.Text = Flag.HotPlate[0].Heat.SetToHeat.Offset.ToString();
                    this.SetPower.Text = Flag.HotPlate[0].Heat.SetToHeat.PowerLimit.ToString();

                    this.IsHeat.Checked = Flag.HotPlate[0].Heat.GetInData.IsRun;
                    this.PV.Text = Flag.HotPlate[0].Heat.GetInData.PV.ToString();
                    this.SP.Text = Flag.HotPlate[0].Heat.GetInData.SP.ToString();
                    this.ErrCode.Text = Flag.HotPlate[0].Heat.GetInData.ErrCode.ToString();
                    break;

                case FlagEnum.HeatPlate.HotPlate2:
                    this.HeatEnabled.Checked = Flag.HotPlate[1].Heat.SetToHeat.IsUseHeat && Flag.HotPlate[1].Heat.SetToHeat.Enabled;
                    this.SetTemp.Text = Flag.HotPlate[1].Heat.SetToHeat.Temp.ToString();
                    this.SetOfftk.Text = Flag.HotPlate[1].Heat.SetToHeat.OffTk.ToString();
                    this.SetOffset.Text = Flag.HotPlate[1].Heat.SetToHeat.Offset.ToString();
                    this.SetPower.Text = Flag.HotPlate[1].Heat.SetToHeat.PowerLimit.ToString();

                    this.IsHeat.Checked = Flag.HotPlate[1].Heat.GetInData.IsRun;
                    this.PV.Text = Flag.HotPlate[1].Heat.GetInData.PV.ToString();
                    this.SP.Text = Flag.HotPlate[1].Heat.GetInData.SP.ToString();
                    this.ErrCode.Text = Flag.HotPlate[1].Heat.GetInData.ErrCode.ToString();
                    break;
            }
        }
    }
}
