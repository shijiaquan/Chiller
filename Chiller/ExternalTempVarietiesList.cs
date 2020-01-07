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
    public partial class ExternalTempVarietiesList : Form
    {
        public ExternalTempVarietiesList()
        {
            InitializeComponent();
        }

        private void ExternalTempVarieties_Load(object sender, EventArgs e)
        {
            ResetList();
        }

        private void ResetList()
        {
            VarietiesList.Items.Clear();
            string[] list = Function.Regedit.Get_Book_List(@"SOFTWARE\JHT\TriTempChiller\","ExternalTempVarieties");
            if (list != null && list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    VarietiesList.Items.Add(list[i]);
                }
            }
        }

        private void NewVarieties_Click(object sender, EventArgs e)
        {
            if (this.VarietiesName.Text == "")
            {
                MessageBox.Show(this, "请输入正确的品种名称！", "提示");
                return;
            }
            if (this.VarietiesList.Items.Contains(this.VarietiesName.Text) == true)
            {
                MessageBox.Show(this, "当前品种名称已存在！", "提示");
                return;
            }

            #region 加热模式以及基本参数设定

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "HeatMode", "Hot");


            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "ChillerTemp", -10f);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "ChillerRange", 1);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "SetTemp", 30);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "PreCoolTemp", 5);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "ResultTemp", 30);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "SetRange", 3);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "BorderTemp", 30);

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "IsPreCooling", false);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "PreCooling", false);
            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseData", "BorderHeat", false);

            #endregion

            #region 温度偏移的基底参数包含测试和边框

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseOffset", "TestTempOffsetPoint", 0);
            for (int i = 0; i < 3; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseOffset", "TestTempBasicPoint" + i.ToString(), 0);
            }

            Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseOffset", "BorderTempOffsetPoint", 0);
            for (int i = 0; i < 3; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BaseOffset", "BorderTempBasicPoint" + i.ToString(), 0);
            }

            #endregion

            #region TestArm

            for (int i = 0; i < 8; i++)
            {
                #region Offset

                for (int j = 0; j < 4; j++)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "TempOffset" + j.ToString(), 0);
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "TestArm", "Channel" + i.ToString(), "Heat_IC", "TempOffset" + j.ToString(), 0);
                }

                #endregion

                #region 加热功率

                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "TestArm", "Channel" + i.ToString(), "Heat_Inside", "PowerLimit", 50);
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "TestArm", "Channel" + i.ToString(), "Heat_IC", "PowerLimit", 50);

                #endregion
            }

            #endregion

            #region ColdPlate

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "ColdPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), 0);
                }

                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "ColdPlate", "Channel" + i.ToString(), "Heat", "PowerLimit", 50);

            }

            #endregion

            #region HotPlate
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "HotPlate", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), 0);
                }

                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "HotPlate", "Channel" + i.ToString(), "Heat", "PowerLimit", 50);
            }
            #endregion

            #region Border

            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BorderTemp", "Channel" + i.ToString(), "Heat", "TempOffset" + j.ToString(), 0);
                }

                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "BorderTemp", "Channel" + i.ToString(), "Heat", "PowerLimit", 100);
            }

            #endregion

            #region Pump

            for (int i = 0; i < 3; i++)
            {
                Function.Regedit.Set_Value(@"SOFTWARE\JHT\TriTempChiller\", "ExternalTempVarieties", this.VarietiesName.Text, "Pump", "Speed" + i.ToString(), 100);
            }

            #endregion

            ResetList();

            MessageBox.Show(this, "品种已新建完成！", "提示");
        }

        private void VarietiesClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CopyVarieties_Click(object sender, EventArgs e)
        {
            if (this.VarietiesName.Text == "")
            {
                Function.Other.Message("请输入正确的机种名称！", "提示");
                return;
            }
            if (VarietiesList.Items.Contains(this.VarietiesName.Text) == true)
            {
                Function.Other.Message("当前机种名称已存在！", "提示");
                return;
            }

            string OldSubkey = @"SOFTWARE\JHT\TriTempChiller\" + "\\"  + "ExternalTempVarieties";
            string NewSubkey = @"SOFTWARE\JHT\TriTempChiller\" + "\\"  + "ExternalTempVarieties";

            Function.Regedit.Copy_Book(OldSubkey, this.VarietiesList.Text, NewSubkey, this.VarietiesName.Text);

            ResetList();

            MessageBox.Show(this, "品种复制完成！", "提示");
        }

        private void ChangeVarieties_Click(object sender, EventArgs e)
        {
            if (this.VarietiesName.Text == "")
            {
                Function.Other.Message("请输入正确的机种名称！", "提示");
                return;
            }
            if (VarietiesList.Items.Contains(this.VarietiesName.Text) == true)
            {
                Function.Other.Message("当前机种名称已存在！", "提示");
                return;
            }

            string OldSubkey = @"SOFTWARE\JHT\TriTempChiller\" + "\\" + "ExternalTempVarieties";
            string NewSubkey = @"SOFTWARE\JHT\TriTempChiller\" + "\\"  + "ExternalTempVarieties";

            Function.Regedit.Copy_Book(OldSubkey, this.VarietiesList.Text, NewSubkey, this.VarietiesName.Text);

            Function.Regedit.Del_Book(OldSubkey, this.VarietiesList.Text);

            ResetList();

            MessageBox.Show(this, "品种更名完成！", "提示");

        }

        private void DeleteVarieties_Click(object sender, EventArgs e)
        {
            if (Win.MessageBox(this, "请确认是否删除品种：" + this.VarietiesList.Text + "？" + "\r\n" + "点击Yes确认删除." + "\r\n" + "点击No取消操作.", "提示") == System.Windows.Forms.DialogResult.Yes)
            {
                string Subkey = @"SOFTWARE\JHT\TriTempChiller\" + "\\" + "ExternalTempVarieties";
                Function.Regedit.Del_Book(Subkey, this.VarietiesList.Text);
                ResetList();
                MessageBox.Show(this, "品种删除完成！", "提示");
            }
        }
    }
}
