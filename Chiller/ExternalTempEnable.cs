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
    public partial class ExternalTempEnable : Form
    {
        public ExternalTempEnable()
        {
            InitializeComponent();

            this.Width = 780;
            this.Height = 380;
        }

        private void ExternalTempEnable_Load(object sender, EventArgs e)
        {
            #region 初始页面显示
            this.NoEnableTemp.Checked = Flag.ExternalTestTempChange.NoEnableDisplayTemp;
            this.DisplayAllChannel.Checked = Flag.ExternalTestTempChange.DisplayAllChannel;

            Point P;

            if (this.DisplayAllChannel.Checked == true)
            {
                this.Width = 610;
                this.Height = 1030;

                P = new Point(64, 917);
                this.DisplayAllChannel.Location = P;

                P = new Point(64, 954);
                this.NoEnableTemp.Location = P;

                P = new Point(216, 913);
                this.EnableSetApply.Location = P;

                P = new Point(379, 913);
                this.EnableSetClose.Location = P;
            }
            else
            {
                this.Width = 780;
                this.Height = 380;

                P = new Point(604, 35);
                this.DisplayAllChannel.Location = P;

                P = new Point(604, 75);
                this.NoEnableTemp.Location = P;

                P = new Point(604, 127);
                this.EnableSetApply.Location = P;

                P = new Point(604, 222);
                this.EnableSetClose.Location = P;
            }

            #endregion

            RefreshUI();
        }

        private void RefreshUI()
        {
            this.TestArm1.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[0];
            this.TestArm2.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[1];
            this.TestArm3.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[2];
            this.TestArm4.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[3];
            this.TestArm5.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[4];
            this.TestArm6.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[5];
            this.TestArm7.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[6];
            this.TestArm8.SwitchStatus = Flag.ExternalTestTempChange.TestArmEnabled[7];

            this.ColdPlate1.SwitchStatus = Flag.ExternalTestTempChange.ColdPlateEnabled[0];
            this.ColdPlate2.SwitchStatus = Flag.ExternalTestTempChange.ColdPlateEnabled[1];

            this.HotPlate1.SwitchStatus = Flag.ExternalTestTempChange.HotPlateEnabled[0];
            this.HotPlate2.SwitchStatus = Flag.ExternalTestTempChange.HotPlateEnabled[1];

            this.BorderTestArm1.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[0];
            this.BorderTestArm2.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[1];
            this.BorderTestArm3.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[2];
            this.BorderTestArm4.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[3];
            this.BorderTestArm5.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[4];
            this.BorderTestArm6.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[5];
            this.BorderTestArm7.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[6];
            this.BorderTestArm8.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[7];
            this.BorderColdPlate1.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[8];
            this.BorderColdPlate2.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[9];
            this.BorderSocket1.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[10];
            this.BorderSocket2.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[11];
            this.BorderSocket3.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[12];
            this.BorderSocket4.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[13];
            this.BorderSocket5.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[14];
            this.BorderSocket6.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[15];
            this.BorderSocket7.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[16];
            this.BorderSocket8.SwitchStatus = Flag.ExternalTestTempChange.BorderEnabled[17];
        }

        private void EnableSetClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnableSetApply_Click(object sender, EventArgs e)
        {
            Flag.ExternalTestTempChange.DisplayAllChannel = this.DisplayAllChannel.Checked;
            Flag.ExternalTestTempChange.NoEnableDisplayTemp = this.NoEnableTemp.Checked;

            if (Flag.ExternalTestTempChange.DisplayAllChannel == true)
            {
                Flag.ExternalTestTempChange.TestArmEnabled[0]= this.TestArm1.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[1] = this.TestArm2.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[2] = this.TestArm3.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[3] = this.TestArm4.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[4] = this.TestArm5.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[5] = this.TestArm6.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[6] = this.TestArm7.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[7] = this.TestArm8.SwitchStatus;

                Flag.ExternalTestTempChange.ColdPlateEnabled[0] = this.ColdPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.ColdPlateEnabled[1] = this.ColdPlate2.SwitchStatus;

                Flag.ExternalTestTempChange.HotPlateEnabled[0] = this.HotPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.HotPlateEnabled[1] = this.HotPlate2.SwitchStatus;

                Flag.ExternalTestTempChange.BorderEnabled[0] = this.BorderTestArm1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[1] = this.BorderTestArm2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[2] = this.BorderTestArm3.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[3] = this.BorderTestArm4.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[4] = this.BorderTestArm5.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[5] = this.BorderTestArm6.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[6] = this.BorderTestArm7.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[7] = this.BorderTestArm8.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[8] = this.BorderColdPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[9] = this.BorderColdPlate2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[10] = this.BorderSocket1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[11] = this.BorderSocket2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[12] = this.BorderSocket3.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[13] = this.BorderSocket4.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[14] = this.BorderSocket5.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[15] = this.BorderSocket6.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[16] = this.BorderSocket7.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[17] = this.BorderSocket8.SwitchStatus;
            }
            else
            {
                Flag.ExternalTestTempChange.TestArmEnabled[0] = this.TestArm1.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[1] = this.TestArm2.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[2] = this.TestArm3.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[3] = this.TestArm4.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[4] = this.TestArm5.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[5] = this.TestArm6.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[6] = this.TestArm7.SwitchStatus;
                Flag.ExternalTestTempChange.TestArmEnabled[7] = this.TestArm8.SwitchStatus;

                Flag.ExternalTestTempChange.ColdPlateEnabled[0] = this.ColdPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.ColdPlateEnabled[1] = this.ColdPlate2.SwitchStatus;

                Flag.ExternalTestTempChange.HotPlateEnabled[0] = this.HotPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.HotPlateEnabled[1] = this.HotPlate2.SwitchStatus;

                Flag.ExternalTestTempChange.BorderEnabled[0] = this.TestArm1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[1] = this.TestArm2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[2] = this.TestArm3.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[3] = this.TestArm4.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[4] = this.TestArm5.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[5] = this.TestArm6.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[6] = this.TestArm7.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[7] = this.TestArm8.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[8] = this.ColdPlate1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[9] = this.ColdPlate2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[10] = this.TestArm1.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[11] = this.TestArm2.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[12] = this.TestArm3.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[13] = this.TestArm4.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[14] = this.TestArm5.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[15] = this.TestArm6.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[16] = this.TestArm7.SwitchStatus;
                Flag.ExternalTestTempChange.BorderEnabled[17] = this.TestArm8.SwitchStatus;
            }

            for (int i = 0; i < Flag.ExternalTestTempChange.TestArmEnabled.Length; i++)
            {
                Flag.VarietiesData.TestArm[i].TestArm_IC.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
                Flag.VarietiesData.TestArm[i].TestArm_Inside.Enabled = Flag.ExternalTestTempChange.TestArmEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.ColdPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.ColdPlate[i].Enabled = Flag.ExternalTestTempChange.ColdPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.HotPlateEnabled.Length; i++)
            {
                Flag.VarietiesData.HotPlate[i].Enabled = Flag.ExternalTestTempChange.HotPlateEnabled[i];
            }
            for (int i = 0; i < Flag.ExternalTestTempChange.BorderEnabled.Length; i++)
            {
                Flag.VarietiesData.BorderTemp[i].Enabled = Flag.ExternalTestTempChange.BorderEnabled[i];
            }

            Flag.SaveData();
        }

        private void DisplayAllChannel_CheckedChanged(object sender, EventArgs e)
        {
            Point P;
            if (this.DisplayAllChannel.Checked == true)
            {
                this.Width = 610;
                this.Height = 1030;

                P = new Point(64, 917);
                this.DisplayAllChannel.Location = P;

                P = new Point(64, 954);
                this.NoEnableTemp.Location = P;

                P = new Point(216, 913);
                this.EnableSetApply.Location = P;

                P = new Point(379, 913);
                this.EnableSetClose.Location = P;
            }
            else
            {
                this.Width = 780;
                this.Height = 380;

                P = new Point(604, 35);
                this.DisplayAllChannel.Location = P;

                P = new Point(604, 75);
                this.NoEnableTemp.Location = P;

                P = new Point(604, 127);
                this.EnableSetApply.Location = P;

                P = new Point(604, 222);
                this.EnableSetClose.Location = P;
            }

            RefreshUI();

            this.CenterToScreen();
        }
    }
}
