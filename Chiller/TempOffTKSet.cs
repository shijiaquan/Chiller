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
    public partial class TempOffTKSet : Form
    {
        public TempOffTKSet()
        {
            InitializeComponent();
        }

        private void TempTkOffSet_Load(object sender, EventArgs e)
        {
            string ReadOutData = "";
            TextBox tb = new TextBox();

            #region TestArm

            for (int i = 0; i < 8; i++)
            {
                #region 温度修正

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk");
                tb = Function.Other.FindControl(this.OffTkTestArm_Inside, "OffTkTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                if (tb != null) { tb.Text = ReadOutData; }

                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk");
                tb = Function.Other.FindControl(this.OffTkTestArm_IC, "OffTkTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                if (tb != null) { tb.Text = ReadOutData; }

                #endregion
            }

            #endregion

            #region ColdPlate

            for (int i = 0; i < 2; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                tb = Function.Other.FindControl(this.OffTkColdPlate, "OffTkColdPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null) { tb.Text = ReadOutData; }
            }

            #endregion

            #region HotPlate
            for (int i = 0; i < 2; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                tb = Function.Other.FindControl(this.OffTkHotPlate, "OffTkHotPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null) { tb.Text = ReadOutData; }
            }
            #endregion

            #region Border

            for (int i = 0; i < 18; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk");
                if (i < 8)
                {
                    tb = Function.Other.FindControl(this.OffTkTestArm_Border, "OffTkTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                }
                else if (i == 8 || i == 9)
                {
                    tb = Function.Other.FindControl(this.OffTkColdPlate_Border, "OffTkColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                }
                else if (i > 9)
                {
                    tb = Function.Other.FindControl(this.OffTkSocket_Border, "OffTkSocket" + (i - 9).ToString()) as TextBox;
                }
                if (tb != null) { tb.Text = ReadOutData; }
            }

            #endregion
        }

        private void TempSetApply_Click(object sender, EventArgs e)
        {
            string ReadOutData = "";
            TextBox tb = new TextBox();

            #region TestArm

            for (int i = 0; i < 8; i++)
            {
                #region 温度修正

                tb = Function.Other.FindControl(this.OffTkTestArm_Inside, "OffTkTestArm" + (i + 1).ToString() + "_Inside") as TextBox;
                if (tb != null)
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk", tb.Text);

                tb = Function.Other.FindControl(this.OffTkTestArm_IC, "OffTkTestArm" + (i + 1).ToString() + "_IC") as TextBox;
                if (tb != null)
                     Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk", tb.Text);

                #endregion
            }

            #endregion

            #region ColdPlate

            for (int i = 0; i < 2; i++)
            {
                tb = Function.Other.FindControl(this.OffTkColdPlate, "OffTkColdPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);

            }

            #endregion

            #region HotPlate
            for (int i = 0; i < 2; i++)
            {
                tb = Function.Other.FindControl(this.OffTkHotPlate, "OffTkHotPlate" + (i + 1).ToString()) as TextBox;
                if (tb != null)
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);

            }
            #endregion

            #region Border

            for (int i = 0; i < 18; i++)
            {
                if (i < 8)
                {
                    tb = Function.Other.FindControl(this.OffTkTestArm_Border, "OffTkTestArm" + (i + 1).ToString() + "_Border") as TextBox;
                }
                else if (i == 8 || i == 9)
                {
                    tb = Function.Other.FindControl(this.OffTkColdPlate_Border, "OffTkColdPlate" + (i - 7).ToString() + "_Border") as TextBox;
                }
                else if (i > 9)
                {
                    tb = Function.Other.FindControl(this.OffTkSocket_Border, "OffTkSocket" + (i - 9).ToString()) as TextBox;
                }
                if (tb != null)
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk", tb.Text);
            }

            #endregion

            #region 加载基底的TkOff

            //TestArm
            for (int i = 0; i < Flag.TempOffTK.TestArm_Inside.Length; i++)
            {
                //Heat_Inside
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_Inside", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.TestArm_Inside[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.TestArm_Inside[i]);

                //Heat_IC
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "TestArm", "Channel" + i.ToString(), "Heat_IC", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.TestArm_IC[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.TestArm_IC[i]);
            }

            //ColdPlate
            for (int i = 0; i < Flag.TempOffTK.ColdPlate.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "ColdPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.ColdPlate[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.ColdPlate[i]);
            }

            //HotPlate
            for (int i = 0; i < Flag.TempOffTK.HotPlate.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "HotPlate", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.HotPlate[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.HotPlate[i]);
            }

            //BorderTemp
            for (int i = 0; i < Flag.TempOffTK.Border.Length; i++)
            {
                ReadOutData = Function.Regedit.Get_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties_TKOff", "BorderTemp", "Channel" + i.ToString(), "Heat", "OffTk");
                Function.Other.SetVable(Function.Other.IsIdentical(Flag.TempOffTK.Border[i], ReadOutData) == true, ReadOutData, 0, ref Flag.TempOffTK.Border[i]);
            }

            #endregion
        }

        private void TempSetClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void In_Put_Float(object sender, KeyPressEventArgs e)
        {
            Function.Input.In_Put_Float(sender, e);
        }
    }
}
