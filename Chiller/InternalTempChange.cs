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
    public partial class InternalTempChange : Form
    {
        public InternalTempChange()
        {
            InitializeComponent();
        }

        private void InternalColdControlChart_Load(object sender, EventArgs e)
        {
            RefreshUI();
            RefreshOffsetPointState(Flag.WaterBox.Offset.TempOffsetPoint);
        }

        private void RefreshUI()
        {
            this.SetTemp.Text = Flag.WaterBox.Parameter.SetTemp.ToString();
            this.SetRange.Text = Flag.WaterBox.Parameter.SetRange.ToString();

            Function.Other.Int_To_Radio(Flag.WaterBox.Offset.TempOffsetPoint, ref this.ChillerOne, ref this.ChillerTwo, ref this.ChillerThree);

            this.ChillerOffset1.Text = Flag.WaterBox.Offset.TempOffset[0].ToString();
            this.ChillerOffset2.Text = Flag.WaterBox.Offset.TempOffset[1].ToString();
            this.ChillerOffset3.Text = Flag.WaterBox.Offset.TempOffset[2].ToString();
            this.ChillerOffset4.Text = Flag.WaterBox.Offset.TempOffset[3].ToString();

            this.BasePoint1.Text = Flag.WaterBox.Offset.TempBasicPoint[0].ToString();
            this.BasePoint2.Text = Flag.WaterBox.Offset.TempBasicPoint[1].ToString();
            this.BasePoint3.Text = Flag.WaterBox.Offset.TempBasicPoint[2].ToString();
        }

        private void RefreshOffsetPointState( int num)
        {
            if (num == 0)
            {
                this.ChillerOffset1.Enabled = true;
                this.ChillerOffset2.Enabled = false;
                this.ChillerOffset3.Enabled = false;

                this.BasePoint1.Enabled = true;
                this.BasePoint2.Enabled = false;
                this.BasePoint3.Enabled = false;
            }
            else if (num == 1)
            {
                this.ChillerOffset1.Enabled = true;
                this.ChillerOffset2.Enabled = false;
                this.ChillerOffset3.Enabled = true;

                this.BasePoint1.Enabled = true;
                this.BasePoint2.Enabled = false;
                this.BasePoint3.Enabled = true;
            }
            else if (num == 2)
            {
                this.ChillerOffset1.Enabled = true;
                this.ChillerOffset2.Enabled = true;
                this.ChillerOffset3.Enabled = true;

                this.BasePoint1.Enabled = true;
                this.BasePoint2.Enabled = true;
                this.BasePoint3.Enabled = true;
            }
            else
            {
                this.ChillerOffset1.Enabled = false;
                this.ChillerOffset2.Enabled = false;
                this.ChillerOffset3.Enabled = false;

                this.BasePoint1.Enabled = false;
                this.BasePoint2.Enabled = false;
                this.BasePoint3.Enabled = false;
            }
        }

        private void OffsetNumberChange(object sender, EventArgs e)
        {
            try
            {
                if (Flag.WaterBox.Offset.TempOffsetPoint == 0)
                {

                }
                else if (Flag.WaterBox.Offset.TempOffsetPoint == 1)
                {
                    PointF One = new PointF(float.Parse(this.BasePoint1.Text), float.Parse(this.ChillerOffset1.Text));
                    PointF Two = new PointF(float.Parse(this.BasePoint3.Text), float.Parse(this.ChillerOffset3.Text));
                    this.ChillerOffset4.Text = Function.Other.CalculationTempOffset(One, Two, float.Parse(this.SetTemp.Text)).ToString();
                }
                else if (Flag.WaterBox.Offset.TempOffsetPoint == 2)
                {

                }
                else
                {

                }
            }
            catch
            {

            }
        }

        private void ChartSetApply_Click(object sender, EventArgs e)
        {
            Flag.WaterBox.Parameter.SetTemp = float.Parse(this.SetTemp.Text);
            Flag.WaterBox.Parameter.SetRange = float.Parse(this.SetRange.Text);

            Flag.WaterBox.Offset.TempOffsetPoint = Function.Other.Radio_To_Int(this.ChillerOne, this.ChillerTwo, this.ChillerThree);

            Flag.WaterBox.Offset.TempOffset[0] = float.Parse(this.ChillerOffset1.Text);
            Flag.WaterBox.Offset.TempOffset[1] = float.Parse(this.ChillerOffset2.Text);
            Flag.WaterBox.Offset.TempOffset[2] = float.Parse(this.ChillerOffset3.Text);
            Flag.WaterBox.Offset.TempOffset[3] = float.Parse(this.ChillerOffset4.Text);

            Flag.WaterBox.Offset.TempBasicPoint[0] = float.Parse(this.BasePoint1.Text);
            Flag.WaterBox.Offset.TempBasicPoint[1] = float.Parse(this.BasePoint2.Text);
            Flag.WaterBox.Offset.TempBasicPoint[2] = float.Parse(this.BasePoint3.Text);

            Flag.SaveData();
        }

        private void ChartSetCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChillerOne_MouseClick(object sender, MouseEventArgs e)
        {
            Flag.WaterBox.Offset.TempOffsetPoint = Function.Other.Radio_To_Int(this.ChillerOne, this.ChillerTwo, this.ChillerThree);
            RefreshOffsetPointState(Flag.WaterBox.Offset.TempOffsetPoint);
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
