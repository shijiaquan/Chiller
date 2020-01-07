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
    public partial class ExternalTempControlChart : Form
    {
        public ExternalTempControlChart()
        {
            InitializeComponent();
        }

        private void ExternalTempControlChart_Load(object sender, EventArgs e)
        {
            Flag.ExternalTempControlChart.ColorPicker = ColorPicker.ColorFrom.Instance(this, 50);

            this.TestArm1ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[0].Color;
            this.TestArm2ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[1].Color;

            this.TestArm3ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[2].Color;
            this.TestArm4ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[3].Color;

            this.TestArm5ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[4].Color;
            this.TestArm6ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[5].Color;

            this.TestArm7ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[6].Color;
            this.TestArm8ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[7].Color;

            this.ColdPlate1ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[8].Color;
            this.ColdPlate2ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[9].Color;

            this.HotPlate1ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[10].Color;
            this.HotPlate2ChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[11].Color;

            this.SetTempChartColor.BackColor = Flag.ExternalTempControlChart.TempChart[Flag.ExternalTempControlChart.TempChart.Length - 1].Color;

            this.TestArm1ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[0].Type);
            this.TestArm2ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[1].Type);

            this.TestArm3ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[2].Type);
            this.TestArm4ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[3].Type);

            this.TestArm5ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[4].Type);
            this.TestArm6ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[5].Type);

            this.TestArm7ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[6].Type);
            this.TestArm8ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[7].Type);

            this.ColdPlate1ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[8].Type);
            this.ColdPlate2ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[9].Type);

            this.HotPlate1ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[10].Type);
            this.HotPlate2ChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[11].Type);

            this.SetTempChartType.Text = Work.TypeConvert.EnumToString(Flag.ExternalTempControlChart.TempChart[Flag.ExternalTempControlChart.TempChart.Length - 1].Type);

            this.ChartLovTemp.Text = Flag.ExternalTempControlChart.ChartLovTemp.ToString();
            this.ChartHightTemp.Text = Flag.ExternalTempControlChart.ChartHightTemp.ToString();

            this.ChartPointLength.Text = (Flag.ExternalTempControlChart.ChartPointLength / 60).ToString();
            this.ChartRefreshTime.Text = Flag.ExternalTempControlChart.ChartRefreshTime.ToString();
        }

        private void UnitChartColorChange_Click(object sender, EventArgs e)
        {
            if (Flag.ExternalTempControlChart.ColorPicker.ShowColor(this, (sender as Button)) == System.Windows.Forms.DialogResult.OK)
            {
                (sender as Button).BackColor = Flag.ExternalTempControlChart.ColorPicker.ResultColor;
            }
        }

        private void ChartSetApply_Click(object sender, EventArgs e)
        {
            Flag.ExternalTempControlChart.TempChart[0].Color = this.TestArm1ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[1].Color = this.TestArm2ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[2].Color = this.TestArm3ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[3].Color = this.TestArm4ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[4].Color = this.TestArm5ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[5].Color = this.TestArm6ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[6].Color = this.TestArm7ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[7].Color = this.TestArm8ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[8].Color = this.ColdPlate1ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[9].Color = this.ColdPlate2ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[10].Color = this.HotPlate1ChartColor.BackColor;
            Flag.ExternalTempControlChart.TempChart[11].Color = this.HotPlate2ChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[Flag.ExternalTempControlChart.TempChart.Length - 1].Color = this.SetTempChartColor.BackColor;

            Flag.ExternalTempControlChart.TempChart[0].Type = Work.TypeConvert.StringToEnum(this.TestArm1ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[1].Type = Work.TypeConvert.StringToEnum(this.TestArm2ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[2].Type = Work.TypeConvert.StringToEnum(this.TestArm3ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[3].Type = Work.TypeConvert.StringToEnum(this.TestArm4ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[4].Type = Work.TypeConvert.StringToEnum(this.TestArm5ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[5].Type = Work.TypeConvert.StringToEnum(this.TestArm6ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[6].Type = Work.TypeConvert.StringToEnum(this.TestArm7ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[7].Type = Work.TypeConvert.StringToEnum(this.TestArm8ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[8].Type = Work.TypeConvert.StringToEnum(this.ColdPlate1ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[9].Type = Work.TypeConvert.StringToEnum(this.ColdPlate2ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[10].Type = Work.TypeConvert.StringToEnum(this.HotPlate1ChartType.Text);
            Flag.ExternalTempControlChart.TempChart[11].Type = Work.TypeConvert.StringToEnum(this.HotPlate2ChartType.Text);

            Flag.ExternalTempControlChart.TempChart[Flag.ExternalTempControlChart.TempChart.Length - 1].Type = Work.TypeConvert.StringToEnum(this.SetTempChartType.Text);

            Flag.ExternalTempControlChart.ChartLovTemp = int.Parse(this.ChartLovTemp.Text);
            Flag.ExternalTempControlChart.ChartHightTemp = int.Parse(this.ChartHightTemp.Text);

            Flag.ExternalTempControlChart.ChartPointLength = int.Parse(this.ChartPointLength.Text) * 60;
            Flag.ExternalTempControlChart.ChartRefreshTime = int.Parse(this.ChartRefreshTime.Text);

            Flag.ExternalTempControlChart.ChartRefreshEnabled = true;

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
