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
    public partial class UnitWorkStateChart : Form
    {
        public UnitWorkStateChart()
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

        private void UnitWorkStateChart_Load(object sender, EventArgs e)
        {
            Flag.UnitWorkState.ColorPicker = ColorPicker.ColorFrom.Instance(this, 50);

            this.CompressorInPressureChartColor.BackColor = Flag.UnitWorkState.CompressorInPressureChart.Color;
            this.CompressorOutPressureChartColor.BackColor = Flag.UnitWorkState.CompressorOutPressureChart.Color;

            this.CondenserInPressureChartColor.BackColor = Flag.UnitWorkState.CondenserInPressureChart.Color;
            this.CondenserOutPressureChartColor.BackColor = Flag.UnitWorkState.CondenserOutPressureChart.Color;

            this.EvaporatorInPressureChartColor.BackColor = Flag.UnitWorkState.EvaporatorInPressureChart.Color;
            this.EvaporatorOutPressureChartColor.BackColor = Flag.UnitWorkState.EvaporatorOutPressureChart.Color;

            this.HeatExchangerOutPressureChartColor.BackColor = Flag.UnitWorkState.HeatExchangerOutPressureChart.Color;

            this.CompressorOutTempChartColor.BackColor = Flag.UnitWorkState.CompressorOutTempChart.Color;
            this.CondenserOutTempChartColor.BackColor = Flag.UnitWorkState.CondenserOutTempChart.Color;
            this.EvaporatorInTempChartColor.BackColor = Flag.UnitWorkState.EvaporatorInTempChart.Color;

            this.CompressorInPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CompressorInPressureChart.Type);
            this.CompressorOutPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CompressorOutPressureChart.Type);

            this.CondenserInPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CondenserInPressureChart.Type);
            this.CondenserOutPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CondenserOutPressureChart.Type);

            this.EvaporatorInPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.EvaporatorInPressureChart.Type);
            this.EvaporatorOutPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.EvaporatorOutPressureChart.Type);

            this.HeatExchangerOutPressureChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.HeatExchangerOutPressureChart.Type);

            this.CompressorOutTempChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CompressorOutTempChart.Type);
            this.CondenserOutTempChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.CondenserOutTempChart.Type);
            this.EvaporatorInTempChartType.Text = Work.TypeConvert.EnumToString(Flag.UnitWorkState.EvaporatorInTempChart.Type);

            this.ChartLovPressure.Text = Flag.UnitWorkState.ChartLovPressure.ToString();
            this.ChartHightPressure.Text = Flag.UnitWorkState.ChartHightPressure.ToString();
            this.ChartLovTemp.Text = Flag.UnitWorkState.ChartLovTemp.ToString();
            this.ChartHightTemp.Text = Flag.UnitWorkState.ChartHightTemp.ToString();

            this.ChartPointLength.Text = (Flag.UnitWorkState.ChartPointLength / 60).ToString();
            this.ChartRefreshTime.Text = Flag.UnitWorkState.ChartRefreshTime.ToString();
        }

        private void UnitChartColorChange_Click(object sender, EventArgs e)
        {
            if (Flag.UnitWorkState.ColorPicker.ShowColor(this, (sender as Button)) == System.Windows.Forms.DialogResult.OK)
            {
                (sender as Button).BackColor = Flag.UnitWorkState.ColorPicker.ResultColor;
            }
        }

        private void ChartSetApply_Click(object sender, EventArgs e)
        {
            Flag.UnitWorkState.CompressorInPressureChart.Color = this.CompressorInPressureChartColor.BackColor;
            Flag.UnitWorkState.CompressorOutPressureChart.Color = this.CompressorOutPressureChartColor.BackColor;

            Flag.UnitWorkState.CondenserInPressureChart.Color = this.CondenserInPressureChartColor.BackColor;
            Flag.UnitWorkState.CondenserOutPressureChart.Color = this.CondenserOutPressureChartColor.BackColor;

            Flag.UnitWorkState.EvaporatorInPressureChart.Color = this.EvaporatorInPressureChartColor.BackColor;
            Flag.UnitWorkState.EvaporatorOutPressureChart.Color = this.EvaporatorOutPressureChartColor.BackColor;

            Flag.UnitWorkState.HeatExchangerOutPressureChart.Color = this.HeatExchangerOutPressureChartColor.BackColor;

            Flag.UnitWorkState.CompressorOutTempChart.Color = this.CompressorOutTempChartColor.BackColor;
            Flag.UnitWorkState.CondenserOutTempChart.Color = this.CondenserOutTempChartColor.BackColor;
            Flag.UnitWorkState.EvaporatorInTempChart.Color = this.EvaporatorInTempChartColor.BackColor;

            Flag.UnitWorkState.CompressorInPressureChart.Type = Work.TypeConvert.StringToEnum(this.CompressorInPressureChartType.Text);
            Flag.UnitWorkState.CompressorOutPressureChart.Type = Work.TypeConvert.StringToEnum(this.CompressorOutPressureChartType.Text);

            Flag.UnitWorkState.CondenserInPressureChart.Type = Work.TypeConvert.StringToEnum(this.CondenserInPressureChartType.Text);
            Flag.UnitWorkState.CondenserOutPressureChart.Type = Work.TypeConvert.StringToEnum(this.CondenserOutPressureChartType.Text);

            Flag.UnitWorkState.EvaporatorInPressureChart.Type = Work.TypeConvert.StringToEnum(this.EvaporatorInPressureChartType.Text);
            Flag.UnitWorkState.EvaporatorOutPressureChart.Type = Work.TypeConvert.StringToEnum(this.EvaporatorOutPressureChartType.Text);

            Flag.UnitWorkState.HeatExchangerOutPressureChart.Type = Work.TypeConvert.StringToEnum(this.HeatExchangerOutPressureChartType.Text);

            Flag.UnitWorkState.CompressorOutTempChart.Type = Work.TypeConvert.StringToEnum(this.CompressorOutTempChartType.Text);
            Flag.UnitWorkState.CondenserOutTempChart.Type = Work.TypeConvert.StringToEnum(this.CondenserOutTempChartType.Text);
            Flag.UnitWorkState.EvaporatorInTempChart.Type = Work.TypeConvert.StringToEnum(this.EvaporatorInTempChartType.Text);

            Flag.UnitWorkState.ChartLovTemp = int.Parse(this.ChartLovTemp.Text);
            Flag.UnitWorkState.ChartHightTemp = int.Parse(this.ChartHightTemp.Text);

            Flag.UnitWorkState.ChartLovPressure = int.Parse(this.ChartLovPressure.Text);
            Flag.UnitWorkState.ChartHightPressure = int.Parse(this.ChartHightPressure.Text);

            Flag.UnitWorkState.ChartPointLength = int.Parse(this.ChartPointLength.Text) * 60;
            Flag.UnitWorkState.ChartRefreshTime = int.Parse(this.ChartRefreshTime.Text);

            Flag.UnitWorkState.ChartRefreshEnabled = true;

            Flag.SaveData();
        }

        private void ChartSetCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
