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
    public partial class ModbusRtuPrint : Form
    {
        public ModbusRtuPrint()
        {
            InitializeComponent();
        }
        Timer tm;
        private void ModbusRtuPrint_Load(object sender, EventArgs e)
        {
            tm = new Timer() { Interval = 100 };
            tm.Tick += Tm_Tick;
            tm.Start();        
        }

        private void Tm_Tick(object sender, EventArgs e)
        {
            string data = "";
            if (Run.Checked == true)
            {
                if (readPower.Checked == false)
                {
                    for (int i = 0; i < 1500; i++)
                    {
                        data += (i).ToString("0000") + ":" + Handler.NNodbusSlave.DataStore.HoldingRegisters[i + 1].ToString() + "\r\n";
                    }
                }
                else
                {
                    for (int i = 0; i < Flag.TestArm.Length; i++)
                    {
                        data += Flag.TestArm[i].Heat_IC.Name + "：" + Flag.TestArm[i].Heat_IC.GetInPower.PowerLimit.ToString() + "\r\n";
                        data += Flag.TestArm[i].Heat_Inside.Name + "：" + Flag.TestArm[i].Heat_Inside.GetInPower.PowerLimit.ToString() + "\r\n";
                    }
                }
                Display.Text = data;
            }

            try
            {
                if (Address.Text != "")
                {
                    int A = int.Parse(Address.Text);
                    NowNumber.Text = Handler.NNodbusSlave.DataStore.HoldingRegisters[A + 1].ToString();
                }
            }
            catch
            {

            }
        }

        private void ModbusRtuPrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            tm.Stop();
        }

        private void In_Put_Int(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Int(sender, e);
        }

        private void ControlDig_Click(object sender, EventArgs e)
        {
            ModbusDiagnosis fm = new ModbusDiagnosis();
            Chiller.Function.Win.OpenWindow(fm, null, fm.Text, false);
            this.Close();
        }
    }
}
