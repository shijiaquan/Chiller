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
    public partial class ExternalTempDisplay : Form
    {
        public ExternalTempDisplay()
        {
            InitializeComponent();
        }

        TestArm TestArm1, TestArm2, TestArm3, TestArm4, TestArm5, TestArm6, TestArm7, TestArm8;
        Cold_HotPlate ColdPlate1, ColdPlate2, HotPlate1, HotPlate2;

        private void ExternalTempDisplay_Load(object sender, EventArgs e)
        {
            Point P = new Point();
            P.X = 10;
            P.Y = 15;
            TestArm1 = new TestArm(FlagEnum.TestArm.TestArm1);
            TestArm1.TopLevel = false;
            TestArm1.Location = P;
            TestArm1.Parent = this;
            TestArm1.Show();

            P.X += TestArm1.Size.Width + 5;
            TestArm3 = new TestArm(FlagEnum.TestArm.TestArm3);
            TestArm3.TopLevel = false;
            TestArm3.Location = P;
            TestArm3.Parent = this;
            TestArm3.Show();

            P.X += TestArm3.Size.Width + 5;
            TestArm5 = new TestArm(FlagEnum.TestArm.TestArm5);
            TestArm5.TopLevel = false;
            TestArm5.Location = P;
            TestArm5.Parent = this;
            TestArm5.Show();

            P.X += TestArm5.Size.Width + 5;
            TestArm7 = new TestArm(FlagEnum.TestArm.TestArm7);
            TestArm7.TopLevel = false;
            TestArm7.Location = P;
            TestArm7.Parent = this;
            TestArm7.Show();

            P.X += TestArm7.Size.Width + 5;
            ColdPlate1 = new Cold_HotPlate(FlagEnum.HeatPlate.ColdPlate1);
            ColdPlate1.TopLevel = false;
            ColdPlate1.Location = P;
            ColdPlate1.Parent = this;
            ColdPlate1.Show();

            P.X += ColdPlate1.Size.Width + 5;
            HotPlate1 = new Cold_HotPlate(FlagEnum.HeatPlate.HotPlate1);
            HotPlate1.TopLevel = false;
            HotPlate1.Location = P;
            HotPlate1.Parent = this;
            HotPlate1.Show();

            P = new Point();
            P.X = 10;
            P.Y = 15 + HotPlate1.Size.Height + 5;

            TestArm2 = new TestArm(FlagEnum.TestArm.TestArm2);
            TestArm2.TopLevel = false;
            TestArm2.Location = P;
            TestArm2.Parent = this;
            TestArm2.Show();

            P.X += TestArm2.Size.Width + 5;
            TestArm4 = new TestArm(FlagEnum.TestArm.TestArm4);
            TestArm4.TopLevel = false;
            TestArm4.Location = P;
            TestArm4.Parent = this;
            TestArm4.Show();

            P.X += TestArm4.Size.Width + 5;
            TestArm6 = new TestArm(FlagEnum.TestArm.TestArm6);
            TestArm6.TopLevel = false;
            TestArm6.Location = P;
            TestArm6.Parent = this;
            TestArm6.Show();

            P.X += TestArm6.Size.Width + 5;
            TestArm8 = new TestArm(FlagEnum.TestArm.TestArm8);
            TestArm8.TopLevel = false;
            TestArm8.Location = P;
            TestArm8.Parent = this;
            TestArm8.Show();

            P.X += TestArm8.Size.Width + 5;
            ColdPlate2 = new Cold_HotPlate(FlagEnum.HeatPlate.ColdPlate2);
            ColdPlate2.TopLevel = false;
            ColdPlate2.Location = P;
            ColdPlate2.Parent = this;
            ColdPlate2.Show();

            P.X += ColdPlate2.Size.Width + 5;
            HotPlate2 = new Cold_HotPlate(FlagEnum.HeatPlate.HotPlate2);
            HotPlate2.TopLevel = false;
            HotPlate2.Location = P;
            HotPlate2.Parent = this;
            HotPlate2.Show();

            P.X += HotPlate2.Size.Width + 25;
            P.Y += HotPlate2.Size.Height + 40;
            this.Width = P.X;
            this.Height = P.Y;
        }
    }
}
