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
    public partial class ModbusDiagnosis : Form
    {
        public ModbusDiagnosis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Handler.NNodbusSlave.DataStore.HoldingRegisters[101] = (ushort)Function.Other.Bool_To_Int(SLTControl.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[103] = (ushort)Function.Other.Bool_To_Int(checkBox1.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[478] = ushort.Parse(textBox1.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[479] = ushort.Parse(textBox10.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[480] = ushort.Parse(textBox15.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[481] = ushort.Parse(textBox20.Text);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[482] = ushort.Parse(textBox2.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[483] = ushort.Parse(textBox9.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[484] = ushort.Parse(textBox14.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[485] = ushort.Parse(textBox19.Text);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[486] = ushort.Parse(textBox3.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[487] = ushort.Parse(textBox8.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[488] = ushort.Parse(textBox13.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[489] = ushort.Parse(textBox18.Text);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[492] = (ushort)(ushort.Parse(textBox4.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[493] = (ushort)(ushort.Parse(textBox7.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[494] = (ushort)(ushort.Parse(textBox12.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[495] = (ushort)(ushort.Parse(textBox17.Text) * 100);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[496] = (ushort)(ushort.Parse(textBox5.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[497] = (ushort)(ushort.Parse(textBox6.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[498] = (ushort)(ushort.Parse(textBox11.Text) * 100);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[499] = (ushort)(ushort.Parse(textBox16.Text) * 100);


            for (int i = 0; i < 16; i++)
            {
                Handler.NNodbusSlave.DataStore.HoldingRegisters[575 + i] = ushort.Parse(textBox21.Text);
            }

            for (int i = 0; i < 2; i++)
            {
                Handler.NNodbusSlave.DataStore.HoldingRegisters[592 + i] = ushort.Parse(textBox22.Text);
            }

            for (int i = 0; i < 2; i++)
            {
                Handler.NNodbusSlave.DataStore.HoldingRegisters[594 + i] = ushort.Parse(textBox23.Text);
            }

            for (int i = 0; i < 18; i++)
            {
                Handler.NNodbusSlave.DataStore.HoldingRegisters[596 + i] = ushort.Parse(textBox24.Text);
            }

            Handler.NNodbusSlave.DataStore.HoldingRegisters[614] = ushort.Parse(textBox30.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[615] = ushort.Parse(textBox29.Text);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[616] = ushort.Parse(textBox28.Text);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[301] = (ushort)Function.Other.Bool_To_Int(checkBox2.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[313] = (ushort)Function.Other.Bool_To_Int(checkBox2.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[323] = (ushort)Function.Other.Bool_To_Int(checkBox2.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[302] = (ushort)Function.Other.Bool_To_Int(checkBox3.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[314] = (ushort)Function.Other.Bool_To_Int(checkBox3.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[324] = (ushort)Function.Other.Bool_To_Int(checkBox3.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[303] = (ushort)Function.Other.Bool_To_Int(checkBox4.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[315] = (ushort)Function.Other.Bool_To_Int(checkBox4.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[325] = (ushort)Function.Other.Bool_To_Int(checkBox4.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[304] = (ushort)Function.Other.Bool_To_Int(checkBox5.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[316] = (ushort)Function.Other.Bool_To_Int(checkBox5.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[326] = (ushort)Function.Other.Bool_To_Int(checkBox5.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[305] = (ushort)Function.Other.Bool_To_Int(checkBox6.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[317] = (ushort)Function.Other.Bool_To_Int(checkBox6.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[327] = (ushort)Function.Other.Bool_To_Int(checkBox6.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[306] = (ushort)Function.Other.Bool_To_Int(checkBox7.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[318] = (ushort)Function.Other.Bool_To_Int(checkBox7.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[328] = (ushort)Function.Other.Bool_To_Int(checkBox7.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[307] = (ushort)Function.Other.Bool_To_Int(checkBox8.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[319] = (ushort)Function.Other.Bool_To_Int(checkBox8.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[329] = (ushort)Function.Other.Bool_To_Int(checkBox8.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[308] = (ushort)Function.Other.Bool_To_Int(checkBox9.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[320] = (ushort)Function.Other.Bool_To_Int(checkBox9.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[330] = (ushort)Function.Other.Bool_To_Int(checkBox9.Checked);

            Handler.NNodbusSlave.DataStore.HoldingRegisters[331] = (ushort)Function.Other.Bool_To_Int(checkBox17.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[332] = (ushort)Function.Other.Bool_To_Int(checkBox16.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[333] = (ushort)Function.Other.Bool_To_Int(checkBox15.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[334] = (ushort)Function.Other.Bool_To_Int(checkBox14.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[335] = (ushort)Function.Other.Bool_To_Int(checkBox13.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[336] = (ushort)Function.Other.Bool_To_Int(checkBox12.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[337] = (ushort)Function.Other.Bool_To_Int(checkBox11.Checked);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[338] = (ushort)Function.Other.Bool_To_Int(checkBox10.Checked);

            ushort Value = 0;
             Function.Other.SetVable(checkBox25.Checked==true,2,1,ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[339] = Value;

            Function.Other.SetVable(checkBox24.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[340] = Value;

            Function.Other.SetVable(checkBox23.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[341] = Value;

            Function.Other.SetVable(checkBox22.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[342] = Value;

            Function.Other.SetVable(checkBox21.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[343] = Value;

            Function.Other.SetVable(checkBox20.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[344] = Value;

            Function.Other.SetVable(checkBox19.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[345] = Value;

            Function.Other.SetVable(checkBox18.Checked == true, 2, 1, ref Value);
            Handler.NNodbusSlave.DataStore.HoldingRegisters[346] = Value;

        }
    }
}
