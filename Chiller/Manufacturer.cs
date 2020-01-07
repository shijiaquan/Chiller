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
    public partial class Manufacturer : Form
    {
        public Manufacturer()
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

        private void Manufacturer_VisibleChanged(object sender, EventArgs e)
        {
            this.Modular1.Text = Flag.Manufacturer.Network.Modular[0];
            this.Modular2.Text = Flag.Manufacturer.Network.Modular[1];
            this.Modular3.Text = Flag.Manufacturer.Network.Modular[2];
            this.Modular4.Text = Flag.Manufacturer.Network.Modular[3];
            this.Modular5.Text = Flag.Manufacturer.Network.Modular[4];
            this.Modular6.Text = Flag.Manufacturer.Network.Modular[5];
            this.Modular7.Text = Flag.Manufacturer.Network.Modular[6];
            this.Modular8.Text = Flag.Manufacturer.Network.Modular[7];
            this.Modular9.Text = Flag.Manufacturer.Network.Modular[8];

            this.ExhaustColding.Text = Flag.Manufacturer.CompressorColdData.ExhaustColding.ToString();
            this.DeepColding.Text = Flag.Manufacturer.CompressorColdData.DeepColding.ToString();
            this.ExhaustOverrun.Text = Flag.Manufacturer.CompressorColdData.ExhaustOverrun.ToString();
            this.RapidColding.Text = Flag.Manufacturer.CompressorColdData.RapidColding.ToString();
            this.CondensateOverrun.Text = Flag.Manufacturer.CompressorColdData.CondensateOverrun.ToString();
            this.RestartTime.Text = Flag.Manufacturer.CompressorColdData.RestartTime.ToString();

            this.StartInterval.Text = Flag.Manufacturer.CompressorSystemData.StartInterval.ToString();
            this.LowOutPressure.Text = Flag.Manufacturer.CompressorSystemData.LowOutPressure.ToString();
            this.LowInPressure.Text = Flag.Manufacturer.CompressorSystemData.LowInPressure.ToString();
            this.HightOutPressure.Text = Flag.Manufacturer.CompressorSystemData.HightOutPressure.ToString();
            this.HightInPressure.Text = Flag.Manufacturer.CompressorSystemData.HightInPressure.ToString();
            this.HightExhaustTemp.Text = Flag.Manufacturer.CompressorSystemData.HightExhaustTemp.ToString();
            this.LovEvaporatorTemp.Text = Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp.ToString();
            this.EvaporatorRecoveryTemp.Text = Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp.ToString();
            this.HightCondenserTemp.Text = Flag.Manufacturer.CompressorSystemData.HightCondenserTemp.ToString();
            this.SystemLovTemp.Text = Flag.Manufacturer.CompressorSystemData.SystemLovTemp.ToString();

            this.SpeedCorrectTime.Text = Flag.Manufacturer.InternalPump.SpeedCorrectTime.ToString();
            this.PumpSpeedDiffer4.Text = Flag.Manufacturer.InternalPump.PumpSpeedDiffer[3].ToString();
            this.PumpSpeedDiffer3.Text = Flag.Manufacturer.InternalPump.PumpSpeedDiffer[2].ToString();
            this.PumpSpeedDiffer2.Text = Flag.Manufacturer.InternalPump.PumpSpeedDiffer[1].ToString();
            this.PumpSpeedDiffer1.Text = Flag.Manufacturer.InternalPump.PumpSpeedDiffer[0].ToString();
            this.LevelChange.Text = Flag.Manufacturer.InternalPump.LevelChange.ToString();

            this.HightSpeed.Text = Flag.Manufacturer.InternalPump.HightSpeed.ToString();
            this.OneHightSpeed.Text = Flag.Manufacturer.InternalPump.OneHightSpeed.ToString();

            this.EvaporationTempDiffer.Text = Flag.Manufacturer.InternalPump.EvaporationTempDiffer.ToString();

            this.PreRotationSpeed.Text = Flag.Manufacturer.InternalPump.PreRotationSpeed.ToString();
            this.PreRotationTemp.Text = Flag.Manufacturer.InternalPump.PreRotationTemp.ToString();
            this.PreRotationTime.Text = Flag.Manufacturer.InternalPump.PreRotationTime.ToString();

            this.AutoCloseValue.Text = Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime.ToString();

            this.NetworkCard.Text = Flag.Manufacturer.LocalNetwork.NetworkCard;
            this.NetworkIP.Text = Flag.Manufacturer.LocalNetwork.NetworkIP;
            this.NetworkMask.Text = Flag.Manufacturer.LocalNetwork.NetworkMask;
            this.NetworkGateWay.Text = Flag.Manufacturer.LocalNetwork.NetworkGateWay;

        }

        private void SetApply_Click(object sender, EventArgs e)
        {
            Flag.Manufacturer.Network.Modular[0] = this.Modular1.Text;
            Flag.Manufacturer.Network.Modular[1] = this.Modular2.Text;
            Flag.Manufacturer.Network.Modular[2] = this.Modular3.Text;
            Flag.Manufacturer.Network.Modular[3] = this.Modular4.Text;
            Flag.Manufacturer.Network.Modular[4] = this.Modular5.Text;
            Flag.Manufacturer.Network.Modular[5] = this.Modular6.Text;
            Flag.Manufacturer.Network.Modular[6] = this.Modular7.Text;
            Flag.Manufacturer.Network.Modular[7] = this.Modular8.Text;
            Flag.Manufacturer.Network.Modular[8] = this.Modular9.Text;

            Flag.Manufacturer.CompressorColdData.ExhaustColding = float.Parse(this.ExhaustColding.Text);
            Flag.Manufacturer.CompressorColdData.DeepColding = float.Parse(this.DeepColding.Text);
            Flag.Manufacturer.CompressorColdData.ExhaustOverrun = float.Parse(this.ExhaustOverrun.Text);
            Flag.Manufacturer.CompressorColdData.RapidColding = float.Parse(this.RapidColding.Text);
            Flag.Manufacturer.CompressorColdData.CondensateOverrun = float.Parse(this.CondensateOverrun.Text);
            Flag.Manufacturer.CompressorColdData.RestartTime = int.Parse(this.RestartTime.Text);

            Flag.Manufacturer.CompressorSystemData.StartInterval = int.Parse(this.StartInterval.Text);
            Flag.Manufacturer.CompressorSystemData.LowOutPressure = float.Parse(this.LowOutPressure.Text);
            Flag.Manufacturer.CompressorSystemData.LowInPressure = float.Parse(this.LowInPressure.Text);
            Flag.Manufacturer.CompressorSystemData.HightOutPressure = float.Parse(this.HightOutPressure.Text);
            Flag.Manufacturer.CompressorSystemData.HightInPressure = float.Parse(this.HightInPressure.Text);
            Flag.Manufacturer.CompressorSystemData.HightExhaustTemp = float.Parse(this.HightExhaustTemp.Text);
            Flag.Manufacturer.CompressorSystemData.LovEvaporatorTemp = float.Parse(this.LovEvaporatorTemp.Text);
            Flag.Manufacturer.CompressorSystemData.EvaporatorRecoveryTemp = float.Parse(this.EvaporatorRecoveryTemp.Text);

            Flag.Manufacturer.CompressorSystemData.HightCondenserTemp = float.Parse(this.HightCondenserTemp.Text);
            Flag.Manufacturer.CompressorSystemData.SystemLovTemp = float.Parse(this.SystemLovTemp.Text);

            Flag.Manufacturer.InternalPump.SpeedCorrectTime = int.Parse(this.SpeedCorrectTime.Text);
            Flag.Manufacturer.InternalPump.PumpSpeedDiffer[3] = int.Parse(this.PumpSpeedDiffer4.Text);
            Flag.Manufacturer.InternalPump.PumpSpeedDiffer[2] = int.Parse(this.PumpSpeedDiffer3.Text);
            Flag.Manufacturer.InternalPump.PumpSpeedDiffer[1] = int.Parse(this.PumpSpeedDiffer2.Text);
            Flag.Manufacturer.InternalPump.PumpSpeedDiffer[0] = int.Parse(this.PumpSpeedDiffer1.Text);
            Flag.Manufacturer.InternalPump.LevelChange = float.Parse(this.LevelChange.Text);

            Flag.Manufacturer.InternalPump.HightSpeed = ushort.Parse(this.HightSpeed.Text);
            Flag.Manufacturer.InternalPump.OneHightSpeed = ushort.Parse(this.OneHightSpeed.Text);

            Flag.Manufacturer.InternalPump.EvaporationTempDiffer = float.Parse(this.EvaporationTempDiffer.Text);

            Flag.Manufacturer.InternalPump.PreRotationSpeed = ushort.Parse(this.PreRotationSpeed.Text);
            Flag.Manufacturer.InternalPump.PreRotationTemp = int.Parse(this.PreRotationTemp.Text);
            Flag.Manufacturer.InternalPump.PreRotationTime = uint.Parse(this.PreRotationTime.Text);

            Flag.Manufacturer.CompressorSystemData.AutoCloseValueTime = uint.Parse(this.AutoCloseValue.Text);

            Flag.Manufacturer.LocalNetwork.NetworkCard = this.NetworkCard.Text;
            Flag.Manufacturer.LocalNetwork.NetworkIP = this.NetworkIP.Text;
            Flag.Manufacturer.LocalNetwork.NetworkMask = this.NetworkMask.Text;
            Flag.Manufacturer.LocalNetwork.NetworkGateWay = this.NetworkGateWay.Text ;

            Flag.SaveData();
        }

        private void In_Put_Int(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Int(sender, e);
        }

        private void In_Put_Float(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Float(sender, e);
        }
    }
}
