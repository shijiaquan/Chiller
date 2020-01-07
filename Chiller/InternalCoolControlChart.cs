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
    public partial class InternalColdControlChart : Form
    {
        public InternalColdControlChart()
        {
            InitializeComponent();
        }

        private void InternalColdControlChart_Load(object sender, EventArgs e)
        {
            Flag.InternalColdControl.ColorPicker = ColorPicker.ColorFrom.Instance(this, 50);

            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                Button bt = Function.Other.FindControl(this, "Unit" + (i + 1).ToString() + "ChartColor") as Button;
                ComboBox cm = Function.Other.FindControl(this, "Unit" + (i + 1).ToString() + "ChartType") as ComboBox;

                bt.BackColor = Flag.InternalColdControl.UnitChart[i].Color;
                cm.Text = Work.TypeConvert.EnumToString(Flag.InternalColdControl.UnitChart[i].Type);
            }

            this.SetTempChartColor.BackColor = Flag.InternalColdControl.SetTempChart.Color;
            this.NowTempChartColor.BackColor = Flag.InternalColdControl.NowTempChart.Color;

            this.SetTempChartType.Text = Work.TypeConvert.EnumToString(Flag.InternalColdControl.SetTempChart.Type);
            this.NowTempChartType.Text = Work.TypeConvert.EnumToString(Flag.InternalColdControl.NowTempChart.Type);

            this.ChartLovTemp.Text = Flag.InternalColdControl.ChartLovTemp.ToString();
            this.ChartHightTemp.Text = Flag.InternalColdControl.ChartHightTemp.ToString();

            this.ChartPointLength.Text = (Flag.InternalColdControl.ChartPointLength / 60).ToString();
            this.ChartRefreshTime.Text = Flag.InternalColdControl.ChartRefreshTime.ToString();
        }

        private void UnitChartColorChange_Click(object sender, EventArgs e)
        {
            if (Flag.InternalColdControl.ColorPicker.ShowColor(this, (sender as Button)) == System.Windows.Forms.DialogResult.OK)
            {
                (sender as Button).BackColor = Flag.InternalColdControl.ColorPicker.ResultColor;
            }
        }
     
        private void ChartSetApply_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Flag.Unit.Length; i++)
            {
                Button bt = Function.Other.FindControl(this, "Unit" + (i + 1).ToString() + "ChartColor") as Button;
                ComboBox cm = Function.Other.FindControl(this, "Unit" + (i + 1).ToString() + "ChartType") as ComboBox;

                Flag.InternalColdControl.UnitChart[i].Color = bt.BackColor;
                Flag.InternalColdControl.UnitChart[i].Type = Work.TypeConvert.StringToEnum(cm.Text);
            }

            Flag.InternalColdControl.SetTempChart.Color = this.SetTempChartColor.BackColor;
            Flag.InternalColdControl.NowTempChart.Color = this.NowTempChartColor.BackColor;

            Flag.InternalColdControl.SetTempChart.Type = Work.TypeConvert.StringToEnum(this.SetTempChartType.Text);
            Flag.InternalColdControl.NowTempChart.Type = Work.TypeConvert.StringToEnum(this.NowTempChartType.Text);

            Flag.InternalColdControl.ChartLovTemp = int.Parse(this.ChartLovTemp.Text);
            Flag.InternalColdControl.ChartHightTemp = int.Parse(this.ChartHightTemp.Text);

            Flag.InternalColdControl.ChartPointLength = int.Parse(this.ChartPointLength.Text) * 60;
            Flag.InternalColdControl.ChartRefreshTime = int.Parse(this.ChartRefreshTime.Text);

            Flag.InternalColdControl.ChartRefreshEnabled = true;

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
