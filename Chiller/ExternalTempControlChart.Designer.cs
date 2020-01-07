namespace Chiller
{
    partial class ExternalTempControlChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChartLovTemp = new System.Windows.Forms.TextBox();
            this.Password_lb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ChartHightTemp = new System.Windows.Forms.TextBox();
            this.panel51 = new System.Windows.Forms.Panel();
            this.panel67 = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.ChartRefreshTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ChartPointLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ChartTypeAndColorB2 = new System.Windows.Forms.Panel();
            this.HotPlate2ChartColor = new System.Windows.Forms.Button();
            this.TestArm8ChartColor = new System.Windows.Forms.Button();
            this.HotPlate1ChartColor = new System.Windows.Forms.Button();
            this.TestArm6ChartColor = new System.Windows.Forms.Button();
            this.SetTempChartColor = new System.Windows.Forms.Button();
            this.TestArm7ChartColor = new System.Windows.Forms.Button();
            this.TestArm5ChartColor = new System.Windows.Forms.Button();
            this.TestArm3ChartColor = new System.Windows.Forms.Button();
            this.ColdPlate2ChartColor = new System.Windows.Forms.Button();
            this.TestArm4ChartColor = new System.Windows.Forms.Button();
            this.ColdPlate1ChartColor = new System.Windows.Forms.Button();
            this.TestArm2ChartColor = new System.Windows.Forms.Button();
            this.TestArm1ChartColor = new System.Windows.Forms.Button();
            this.HotPlate2ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm8ChartType = new System.Windows.Forms.ComboBox();
            this.HotPlate1ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm6ChartType = new System.Windows.Forms.ComboBox();
            this.SetTempChartType = new System.Windows.Forms.ComboBox();
            this.TestArm7ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm5ChartType = new System.Windows.Forms.ComboBox();
            this.ColdPlate1ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm2ChartType = new System.Windows.Forms.ComboBox();
            this.ColdPlate2ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm4ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm3ChartType = new System.Windows.Forms.ComboBox();
            this.TestArm1ChartType = new System.Windows.Forms.ComboBox();
            this.HotPlate2 = new System.Windows.Forms.Label();
            this.TestArm8 = new System.Windows.Forms.Label();
            this.SetTemp = new System.Windows.Forms.Label();
            this.TestArm7 = new System.Windows.Forms.Label();
            this.HotPlate1 = new System.Windows.Forms.Label();
            this.TestArm6 = new System.Windows.Forms.Label();
            this.TestArm5 = new System.Windows.Forms.Label();
            this.ColdPlate2 = new System.Windows.Forms.Label();
            this.TestArm4 = new System.Windows.Forms.Label();
            this.TestArm3 = new System.Windows.Forms.Label();
            this.ColdPlate1 = new System.Windows.Forms.Label();
            this.TestArm2 = new System.Windows.Forms.Label();
            this.TestArm1 = new System.Windows.Forms.Label();
            this.ChartTypeAndColorB1 = new System.Windows.Forms.Panel();
            this.ChartTypeAndColor = new System.Windows.Forms.Label();
            this.ChartSetApply = new HslControls.HslButton();
            this.ChartSetClose = new HslControls.HslButton();
            this.panel51.SuspendLayout();
            this.panel67.SuspendLayout();
            this.ChartTypeAndColorB2.SuspendLayout();
            this.ChartTypeAndColorB1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChartLovTemp
            // 
            this.ChartLovTemp.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartLovTemp.Location = new System.Drawing.Point(167, 66);
            this.ChartLovTemp.Name = "ChartLovTemp";
            this.ChartLovTemp.Size = new System.Drawing.Size(90, 35);
            this.ChartLovTemp.TabIndex = 1;
            this.ChartLovTemp.Text = "-100";
            this.ChartLovTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartLovTemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // Password_lb
            // 
            this.Password_lb.AutoSize = true;
            this.Password_lb.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password_lb.ForeColor = System.Drawing.Color.Black;
            this.Password_lb.Location = new System.Drawing.Point(12, 73);
            this.Password_lb.Name = "Password_lb";
            this.Password_lb.Size = new System.Drawing.Size(149, 20);
            this.Password_lb.TabIndex = 54;
            this.Password_lb.Text = "最低温度（℃）";
            this.Password_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 20);
            this.label1.TabIndex = 54;
            this.label1.Text = "最高温度（℃）";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChartHightTemp
            // 
            this.ChartHightTemp.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartHightTemp.Location = new System.Drawing.Point(167, 107);
            this.ChartHightTemp.Name = "ChartHightTemp";
            this.ChartHightTemp.Size = new System.Drawing.Size(90, 35);
            this.ChartHightTemp.TabIndex = 2;
            this.ChartHightTemp.Text = "40";
            this.ChartHightTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartHightTemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // panel51
            // 
            this.panel51.BackColor = System.Drawing.Color.White;
            this.panel51.Controls.Add(this.Password_lb);
            this.panel51.Controls.Add(this.panel67);
            this.panel51.Controls.Add(this.ChartRefreshTime);
            this.panel51.Controls.Add(this.label2);
            this.panel51.Controls.Add(this.ChartPointLength);
            this.panel51.Controls.Add(this.label5);
            this.panel51.Controls.Add(this.ChartHightTemp);
            this.panel51.Controls.Add(this.label1);
            this.panel51.Controls.Add(this.ChartLovTemp);
            this.panel51.Font = new System.Drawing.Font("宋体", 10F);
            this.panel51.Location = new System.Drawing.Point(12, 12);
            this.panel51.Name = "panel51";
            this.panel51.Size = new System.Drawing.Size(279, 621);
            this.panel51.TabIndex = 57;
            // 
            // panel67
            // 
            this.panel67.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel67.Controls.Add(this.label27);
            this.panel67.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel67.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel67.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.panel67.Location = new System.Drawing.Point(0, 0);
            this.panel67.Name = "panel67";
            this.panel67.Size = new System.Drawing.Size(279, 55);
            this.panel67.TabIndex = 3;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(0, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(279, 55);
            this.label27.TabIndex = 6;
            this.label27.Text = "曲线显示";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChartRefreshTime
            // 
            this.ChartRefreshTime.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartRefreshTime.Location = new System.Drawing.Point(167, 189);
            this.ChartRefreshTime.Name = "ChartRefreshTime";
            this.ChartRefreshTime.Size = new System.Drawing.Size(90, 35);
            this.ChartRefreshTime.TabIndex = 4;
            this.ChartRefreshTime.Text = "40";
            this.ChartRefreshTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartRefreshTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Int);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 20);
            this.label2.TabIndex = 54;
            this.label2.Text = "刷新频率（s）";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChartPointLength
            // 
            this.ChartPointLength.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartPointLength.Location = new System.Drawing.Point(167, 148);
            this.ChartPointLength.Name = "ChartPointLength";
            this.ChartPointLength.Size = new System.Drawing.Size(90, 35);
            this.ChartPointLength.TabIndex = 3;
            this.ChartPointLength.Text = "40";
            this.ChartPointLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartPointLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Int);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(12, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 20);
            this.label5.TabIndex = 54;
            this.label5.Text = "显示长度（min）";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChartTypeAndColorB2
            // 
            this.ChartTypeAndColorB2.BackColor = System.Drawing.Color.White;
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate2ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm8ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate1ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm6ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm7ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm5ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm3ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate2ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm4ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate1ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm2ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm1ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate2ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm8ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate1ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm6ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm7ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm5ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate1ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm2ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate2ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm4ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm3ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm1ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate2);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm8);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm7);
            this.ChartTypeAndColorB2.Controls.Add(this.HotPlate1);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm6);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm5);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate2);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm4);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm3);
            this.ChartTypeAndColorB2.Controls.Add(this.ColdPlate1);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm2);
            this.ChartTypeAndColorB2.Controls.Add(this.TestArm1);
            this.ChartTypeAndColorB2.Controls.Add(this.ChartTypeAndColorB1);
            this.ChartTypeAndColorB2.Font = new System.Drawing.Font("宋体", 10F);
            this.ChartTypeAndColorB2.Location = new System.Drawing.Point(297, 12);
            this.ChartTypeAndColorB2.Name = "ChartTypeAndColorB2";
            this.ChartTypeAndColorB2.Size = new System.Drawing.Size(837, 621);
            this.ChartTypeAndColorB2.TabIndex = 58;
            // 
            // HotPlate2ChartColor
            // 
            this.HotPlate2ChartColor.Location = new System.Drawing.Point(676, 412);
            this.HotPlate2ChartColor.Name = "HotPlate2ChartColor";
            this.HotPlate2ChartColor.Size = new System.Drawing.Size(138, 31);
            this.HotPlate2ChartColor.TabIndex = 56;
            this.HotPlate2ChartColor.TabStop = false;
            this.HotPlate2ChartColor.UseVisualStyleBackColor = true;
            this.HotPlate2ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm8ChartColor
            // 
            this.TestArm8ChartColor.Location = new System.Drawing.Point(399, 412);
            this.TestArm8ChartColor.Name = "TestArm8ChartColor";
            this.TestArm8ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm8ChartColor.TabIndex = 56;
            this.TestArm8ChartColor.TabStop = false;
            this.TestArm8ChartColor.UseVisualStyleBackColor = true;
            this.TestArm8ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // HotPlate1ChartColor
            // 
            this.HotPlate1ChartColor.Location = new System.Drawing.Point(676, 299);
            this.HotPlate1ChartColor.Name = "HotPlate1ChartColor";
            this.HotPlate1ChartColor.Size = new System.Drawing.Size(138, 31);
            this.HotPlate1ChartColor.TabIndex = 56;
            this.HotPlate1ChartColor.TabStop = false;
            this.HotPlate1ChartColor.UseVisualStyleBackColor = true;
            this.HotPlate1ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm6ChartColor
            // 
            this.TestArm6ChartColor.Location = new System.Drawing.Point(399, 299);
            this.TestArm6ChartColor.Name = "TestArm6ChartColor";
            this.TestArm6ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm6ChartColor.TabIndex = 56;
            this.TestArm6ChartColor.TabStop = false;
            this.TestArm6ChartColor.UseVisualStyleBackColor = true;
            this.TestArm6ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // SetTempChartColor
            // 
            this.SetTempChartColor.Location = new System.Drawing.Point(122, 525);
            this.SetTempChartColor.Name = "SetTempChartColor";
            this.SetTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.SetTempChartColor.TabIndex = 56;
            this.SetTempChartColor.TabStop = false;
            this.SetTempChartColor.UseVisualStyleBackColor = true;
            this.SetTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm7ChartColor
            // 
            this.TestArm7ChartColor.Location = new System.Drawing.Point(122, 412);
            this.TestArm7ChartColor.Name = "TestArm7ChartColor";
            this.TestArm7ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm7ChartColor.TabIndex = 56;
            this.TestArm7ChartColor.TabStop = false;
            this.TestArm7ChartColor.UseVisualStyleBackColor = true;
            this.TestArm7ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm5ChartColor
            // 
            this.TestArm5ChartColor.Location = new System.Drawing.Point(122, 299);
            this.TestArm5ChartColor.Name = "TestArm5ChartColor";
            this.TestArm5ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm5ChartColor.TabIndex = 56;
            this.TestArm5ChartColor.TabStop = false;
            this.TestArm5ChartColor.UseVisualStyleBackColor = true;
            this.TestArm5ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm3ChartColor
            // 
            this.TestArm3ChartColor.Location = new System.Drawing.Point(122, 186);
            this.TestArm3ChartColor.Name = "TestArm3ChartColor";
            this.TestArm3ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm3ChartColor.TabIndex = 56;
            this.TestArm3ChartColor.TabStop = false;
            this.TestArm3ChartColor.UseVisualStyleBackColor = true;
            this.TestArm3ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // ColdPlate2ChartColor
            // 
            this.ColdPlate2ChartColor.Location = new System.Drawing.Point(676, 186);
            this.ColdPlate2ChartColor.Name = "ColdPlate2ChartColor";
            this.ColdPlate2ChartColor.Size = new System.Drawing.Size(138, 31);
            this.ColdPlate2ChartColor.TabIndex = 56;
            this.ColdPlate2ChartColor.TabStop = false;
            this.ColdPlate2ChartColor.UseVisualStyleBackColor = true;
            this.ColdPlate2ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm4ChartColor
            // 
            this.TestArm4ChartColor.Location = new System.Drawing.Point(399, 186);
            this.TestArm4ChartColor.Name = "TestArm4ChartColor";
            this.TestArm4ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm4ChartColor.TabIndex = 56;
            this.TestArm4ChartColor.TabStop = false;
            this.TestArm4ChartColor.UseVisualStyleBackColor = true;
            this.TestArm4ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // ColdPlate1ChartColor
            // 
            this.ColdPlate1ChartColor.Location = new System.Drawing.Point(676, 73);
            this.ColdPlate1ChartColor.Name = "ColdPlate1ChartColor";
            this.ColdPlate1ChartColor.Size = new System.Drawing.Size(138, 31);
            this.ColdPlate1ChartColor.TabIndex = 56;
            this.ColdPlate1ChartColor.TabStop = false;
            this.ColdPlate1ChartColor.UseVisualStyleBackColor = true;
            this.ColdPlate1ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm2ChartColor
            // 
            this.TestArm2ChartColor.Location = new System.Drawing.Point(399, 73);
            this.TestArm2ChartColor.Name = "TestArm2ChartColor";
            this.TestArm2ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm2ChartColor.TabIndex = 56;
            this.TestArm2ChartColor.TabStop = false;
            this.TestArm2ChartColor.UseVisualStyleBackColor = true;
            this.TestArm2ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // TestArm1ChartColor
            // 
            this.TestArm1ChartColor.Location = new System.Drawing.Point(122, 73);
            this.TestArm1ChartColor.Name = "TestArm1ChartColor";
            this.TestArm1ChartColor.Size = new System.Drawing.Size(138, 31);
            this.TestArm1ChartColor.TabIndex = 56;
            this.TestArm1ChartColor.TabStop = false;
            this.TestArm1ChartColor.UseVisualStyleBackColor = true;
            this.TestArm1ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // HotPlate2ChartType
            // 
            this.HotPlate2ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HotPlate2ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.HotPlate2ChartType.ItemHeight = 20;
            this.HotPlate2ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.HotPlate2ChartType.Location = new System.Drawing.Point(676, 449);
            this.HotPlate2ChartType.Name = "HotPlate2ChartType";
            this.HotPlate2ChartType.Size = new System.Drawing.Size(138, 28);
            this.HotPlate2ChartType.TabIndex = 55;
            this.HotPlate2ChartType.TabStop = false;
            // 
            // TestArm8ChartType
            // 
            this.TestArm8ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm8ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm8ChartType.ItemHeight = 20;
            this.TestArm8ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm8ChartType.Location = new System.Drawing.Point(399, 449);
            this.TestArm8ChartType.Name = "TestArm8ChartType";
            this.TestArm8ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm8ChartType.TabIndex = 55;
            this.TestArm8ChartType.TabStop = false;
            // 
            // HotPlate1ChartType
            // 
            this.HotPlate1ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HotPlate1ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.HotPlate1ChartType.ItemHeight = 20;
            this.HotPlate1ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.HotPlate1ChartType.Location = new System.Drawing.Point(676, 336);
            this.HotPlate1ChartType.Name = "HotPlate1ChartType";
            this.HotPlate1ChartType.Size = new System.Drawing.Size(138, 28);
            this.HotPlate1ChartType.TabIndex = 55;
            this.HotPlate1ChartType.TabStop = false;
            // 
            // TestArm6ChartType
            // 
            this.TestArm6ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm6ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm6ChartType.ItemHeight = 20;
            this.TestArm6ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm6ChartType.Location = new System.Drawing.Point(399, 336);
            this.TestArm6ChartType.Name = "TestArm6ChartType";
            this.TestArm6ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm6ChartType.TabIndex = 55;
            this.TestArm6ChartType.TabStop = false;
            // 
            // SetTempChartType
            // 
            this.SetTempChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SetTempChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.SetTempChartType.ItemHeight = 20;
            this.SetTempChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.SetTempChartType.Location = new System.Drawing.Point(122, 562);
            this.SetTempChartType.Name = "SetTempChartType";
            this.SetTempChartType.Size = new System.Drawing.Size(138, 28);
            this.SetTempChartType.TabIndex = 55;
            this.SetTempChartType.TabStop = false;
            // 
            // TestArm7ChartType
            // 
            this.TestArm7ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm7ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm7ChartType.ItemHeight = 20;
            this.TestArm7ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm7ChartType.Location = new System.Drawing.Point(122, 449);
            this.TestArm7ChartType.Name = "TestArm7ChartType";
            this.TestArm7ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm7ChartType.TabIndex = 55;
            this.TestArm7ChartType.TabStop = false;
            // 
            // TestArm5ChartType
            // 
            this.TestArm5ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm5ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm5ChartType.ItemHeight = 20;
            this.TestArm5ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm5ChartType.Location = new System.Drawing.Point(122, 336);
            this.TestArm5ChartType.Name = "TestArm5ChartType";
            this.TestArm5ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm5ChartType.TabIndex = 55;
            this.TestArm5ChartType.TabStop = false;
            // 
            // ColdPlate1ChartType
            // 
            this.ColdPlate1ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColdPlate1ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.ColdPlate1ChartType.ItemHeight = 20;
            this.ColdPlate1ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.ColdPlate1ChartType.Location = new System.Drawing.Point(676, 110);
            this.ColdPlate1ChartType.Name = "ColdPlate1ChartType";
            this.ColdPlate1ChartType.Size = new System.Drawing.Size(138, 28);
            this.ColdPlate1ChartType.TabIndex = 55;
            this.ColdPlate1ChartType.TabStop = false;
            // 
            // TestArm2ChartType
            // 
            this.TestArm2ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm2ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm2ChartType.ItemHeight = 20;
            this.TestArm2ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm2ChartType.Location = new System.Drawing.Point(399, 110);
            this.TestArm2ChartType.Name = "TestArm2ChartType";
            this.TestArm2ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm2ChartType.TabIndex = 55;
            this.TestArm2ChartType.TabStop = false;
            // 
            // ColdPlate2ChartType
            // 
            this.ColdPlate2ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColdPlate2ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.ColdPlate2ChartType.ItemHeight = 20;
            this.ColdPlate2ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.ColdPlate2ChartType.Location = new System.Drawing.Point(676, 223);
            this.ColdPlate2ChartType.Name = "ColdPlate2ChartType";
            this.ColdPlate2ChartType.Size = new System.Drawing.Size(138, 28);
            this.ColdPlate2ChartType.TabIndex = 55;
            this.ColdPlate2ChartType.TabStop = false;
            // 
            // TestArm4ChartType
            // 
            this.TestArm4ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm4ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm4ChartType.ItemHeight = 20;
            this.TestArm4ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm4ChartType.Location = new System.Drawing.Point(399, 223);
            this.TestArm4ChartType.Name = "TestArm4ChartType";
            this.TestArm4ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm4ChartType.TabIndex = 55;
            this.TestArm4ChartType.TabStop = false;
            // 
            // TestArm3ChartType
            // 
            this.TestArm3ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm3ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm3ChartType.ItemHeight = 20;
            this.TestArm3ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm3ChartType.Location = new System.Drawing.Point(122, 223);
            this.TestArm3ChartType.Name = "TestArm3ChartType";
            this.TestArm3ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm3ChartType.TabIndex = 55;
            this.TestArm3ChartType.TabStop = false;
            // 
            // TestArm1ChartType
            // 
            this.TestArm1ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestArm1ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.TestArm1ChartType.ItemHeight = 20;
            this.TestArm1ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.TestArm1ChartType.Location = new System.Drawing.Point(122, 110);
            this.TestArm1ChartType.Name = "TestArm1ChartType";
            this.TestArm1ChartType.Size = new System.Drawing.Size(138, 28);
            this.TestArm1ChartType.TabIndex = 55;
            this.TestArm1ChartType.TabStop = false;
            // 
            // HotPlate2
            // 
            this.HotPlate2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.HotPlate2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HotPlate2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HotPlate2.ForeColor = System.Drawing.Color.Black;
            this.HotPlate2.Location = new System.Drawing.Point(573, 412);
            this.HotPlate2.Name = "HotPlate2";
            this.HotPlate2.Size = new System.Drawing.Size(97, 65);
            this.HotPlate2.TabIndex = 54;
            this.HotPlate2.Text = "Hot Plate2";
            this.HotPlate2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm8
            // 
            this.TestArm8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm8.ForeColor = System.Drawing.Color.Black;
            this.TestArm8.Location = new System.Drawing.Point(296, 412);
            this.TestArm8.Name = "TestArm8";
            this.TestArm8.Size = new System.Drawing.Size(97, 65);
            this.TestArm8.TabIndex = 54;
            this.TestArm8.Text = "TestArm8";
            this.TestArm8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetTemp
            // 
            this.SetTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SetTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SetTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetTemp.ForeColor = System.Drawing.Color.Black;
            this.SetTemp.Location = new System.Drawing.Point(19, 525);
            this.SetTemp.Name = "SetTemp";
            this.SetTemp.Size = new System.Drawing.Size(97, 65);
            this.SetTemp.TabIndex = 54;
            this.SetTemp.Text = "Set Temp";
            this.SetTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm7
            // 
            this.TestArm7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm7.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm7.ForeColor = System.Drawing.Color.Black;
            this.TestArm7.Location = new System.Drawing.Point(19, 412);
            this.TestArm7.Name = "TestArm7";
            this.TestArm7.Size = new System.Drawing.Size(97, 65);
            this.TestArm7.TabIndex = 54;
            this.TestArm7.Text = "TestArm7";
            this.TestArm7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HotPlate1
            // 
            this.HotPlate1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.HotPlate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HotPlate1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HotPlate1.ForeColor = System.Drawing.Color.Black;
            this.HotPlate1.Location = new System.Drawing.Point(573, 299);
            this.HotPlate1.Name = "HotPlate1";
            this.HotPlate1.Size = new System.Drawing.Size(97, 65);
            this.HotPlate1.TabIndex = 54;
            this.HotPlate1.Text = "Hot Plate1";
            this.HotPlate1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm6
            // 
            this.TestArm6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm6.ForeColor = System.Drawing.Color.Black;
            this.TestArm6.Location = new System.Drawing.Point(296, 299);
            this.TestArm6.Name = "TestArm6";
            this.TestArm6.Size = new System.Drawing.Size(97, 65);
            this.TestArm6.TabIndex = 54;
            this.TestArm6.Text = "TestArm6";
            this.TestArm6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm5
            // 
            this.TestArm5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm5.ForeColor = System.Drawing.Color.Black;
            this.TestArm5.Location = new System.Drawing.Point(19, 299);
            this.TestArm5.Name = "TestArm5";
            this.TestArm5.Size = new System.Drawing.Size(97, 65);
            this.TestArm5.TabIndex = 54;
            this.TestArm5.Text = "TestArm5";
            this.TestArm5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColdPlate2
            // 
            this.ColdPlate2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ColdPlate2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColdPlate2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColdPlate2.ForeColor = System.Drawing.Color.Black;
            this.ColdPlate2.Location = new System.Drawing.Point(573, 186);
            this.ColdPlate2.Name = "ColdPlate2";
            this.ColdPlate2.Size = new System.Drawing.Size(97, 65);
            this.ColdPlate2.TabIndex = 54;
            this.ColdPlate2.Text = "Cold Plate2";
            this.ColdPlate2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm4
            // 
            this.TestArm4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm4.ForeColor = System.Drawing.Color.Black;
            this.TestArm4.Location = new System.Drawing.Point(296, 186);
            this.TestArm4.Name = "TestArm4";
            this.TestArm4.Size = new System.Drawing.Size(97, 65);
            this.TestArm4.TabIndex = 54;
            this.TestArm4.Text = "TestArm4";
            this.TestArm4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm3
            // 
            this.TestArm3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm3.ForeColor = System.Drawing.Color.Black;
            this.TestArm3.Location = new System.Drawing.Point(19, 186);
            this.TestArm3.Name = "TestArm3";
            this.TestArm3.Size = new System.Drawing.Size(97, 65);
            this.TestArm3.TabIndex = 54;
            this.TestArm3.Text = "TestArm3";
            this.TestArm3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColdPlate1
            // 
            this.ColdPlate1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ColdPlate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColdPlate1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColdPlate1.ForeColor = System.Drawing.Color.Black;
            this.ColdPlate1.Location = new System.Drawing.Point(573, 73);
            this.ColdPlate1.Name = "ColdPlate1";
            this.ColdPlate1.Size = new System.Drawing.Size(97, 65);
            this.ColdPlate1.TabIndex = 54;
            this.ColdPlate1.Text = "Cold Plate1";
            this.ColdPlate1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm2
            // 
            this.TestArm2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm2.ForeColor = System.Drawing.Color.Black;
            this.TestArm2.Location = new System.Drawing.Point(296, 73);
            this.TestArm2.Name = "TestArm2";
            this.TestArm2.Size = new System.Drawing.Size(97, 65);
            this.TestArm2.TabIndex = 54;
            this.TestArm2.Text = "TestArm2";
            this.TestArm2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestArm1
            // 
            this.TestArm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TestArm1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TestArm1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestArm1.ForeColor = System.Drawing.Color.Black;
            this.TestArm1.Location = new System.Drawing.Point(19, 73);
            this.TestArm1.Name = "TestArm1";
            this.TestArm1.Size = new System.Drawing.Size(97, 65);
            this.TestArm1.TabIndex = 54;
            this.TestArm1.Text = "TestArm1";
            this.TestArm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChartTypeAndColorB1
            // 
            this.ChartTypeAndColorB1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ChartTypeAndColorB1.Controls.Add(this.ChartTypeAndColor);
            this.ChartTypeAndColorB1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartTypeAndColorB1.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartTypeAndColorB1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.ChartTypeAndColorB1.Location = new System.Drawing.Point(0, 0);
            this.ChartTypeAndColorB1.Name = "ChartTypeAndColorB1";
            this.ChartTypeAndColorB1.Size = new System.Drawing.Size(837, 55);
            this.ChartTypeAndColorB1.TabIndex = 3;
            // 
            // ChartTypeAndColor
            // 
            this.ChartTypeAndColor.BackColor = System.Drawing.Color.Transparent;
            this.ChartTypeAndColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartTypeAndColor.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.ChartTypeAndColor.ForeColor = System.Drawing.Color.Black;
            this.ChartTypeAndColor.Location = new System.Drawing.Point(0, 0);
            this.ChartTypeAndColor.Name = "ChartTypeAndColor";
            this.ChartTypeAndColor.Size = new System.Drawing.Size(837, 55);
            this.ChartTypeAndColor.TabIndex = 6;
            this.ChartTypeAndColor.Text = "曲线类型以及颜色设置";
            this.ChartTypeAndColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChartSetApply
            // 
            this.ChartSetApply.CornerRadius = 10;
            this.ChartSetApply.CustomerInformation = null;
            this.ChartSetApply.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.ChartSetApply.Location = new System.Drawing.Point(435, 665);
            this.ChartSetApply.Name = "ChartSetApply";
            this.ChartSetApply.Size = new System.Drawing.Size(138, 63);
            this.ChartSetApply.TabIndex = 59;
            this.ChartSetApply.Text = "应用";
            this.ChartSetApply.Click += new System.EventHandler(this.ChartSetApply_Click);
            // 
            // ChartSetClose
            // 
            this.ChartSetClose.CornerRadius = 10;
            this.ChartSetClose.CustomerInformation = null;
            this.ChartSetClose.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.ChartSetClose.Location = new System.Drawing.Point(600, 665);
            this.ChartSetClose.Name = "ChartSetClose";
            this.ChartSetClose.Size = new System.Drawing.Size(138, 63);
            this.ChartSetClose.TabIndex = 59;
            this.ChartSetClose.Text = "关闭";
            this.ChartSetClose.Click += new System.EventHandler(this.ChartSetCancel_Click);
            // 
            // ExternalTempControlChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 740);
            this.ControlBox = false;
            this.Controls.Add(this.ChartSetClose);
            this.Controls.Add(this.ChartSetApply);
            this.Controls.Add(this.ChartTypeAndColorB2);
            this.Controls.Add(this.panel51);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExternalTempControlChart";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "显示设置";
            this.Load += new System.EventHandler(this.ExternalTempControlChart_Load);
            this.panel51.ResumeLayout(false);
            this.panel51.PerformLayout();
            this.panel67.ResumeLayout(false);
            this.ChartTypeAndColorB2.ResumeLayout(false);
            this.ChartTypeAndColorB1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox ChartLovTemp;
        internal System.Windows.Forms.Label Password_lb;
        internal System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox ChartHightTemp;
        private System.Windows.Forms.Panel panel51;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Panel panel67;
        public System.Windows.Forms.TextBox ChartPointLength;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel ChartTypeAndColorB2;
        internal System.Windows.Forms.Label TestArm1;
        private System.Windows.Forms.Panel ChartTypeAndColorB1;
        private System.Windows.Forms.Label ChartTypeAndColor;
        internal System.Windows.Forms.Label TestArm6;
        internal System.Windows.Forms.Label TestArm5;
        internal System.Windows.Forms.Label TestArm4;
        internal System.Windows.Forms.Label TestArm3;
        internal System.Windows.Forms.Label TestArm2;
        private System.Windows.Forms.ComboBox TestArm6ChartType;
        private System.Windows.Forms.ComboBox TestArm5ChartType;
        private System.Windows.Forms.ComboBox TestArm2ChartType;
        private System.Windows.Forms.ComboBox TestArm4ChartType;
        private System.Windows.Forms.ComboBox TestArm3ChartType;
        private System.Windows.Forms.ComboBox TestArm1ChartType;
        private HslControls.HslButton ChartSetApply;
        private HslControls.HslButton ChartSetClose;
        private System.Windows.Forms.Button TestArm1ChartColor;
        private System.Windows.Forms.Button TestArm6ChartColor;
        private System.Windows.Forms.Button TestArm5ChartColor;
        private System.Windows.Forms.Button TestArm3ChartColor;
        private System.Windows.Forms.Button TestArm4ChartColor;
        private System.Windows.Forms.Button TestArm2ChartColor;
        public System.Windows.Forms.TextBox ChartRefreshTime;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button TestArm8ChartColor;
        private System.Windows.Forms.Button TestArm7ChartColor;
        private System.Windows.Forms.ComboBox TestArm8ChartType;
        private System.Windows.Forms.ComboBox TestArm7ChartType;
        internal System.Windows.Forms.Label TestArm8;
        internal System.Windows.Forms.Label TestArm7;
        private System.Windows.Forms.Button SetTempChartColor;
        private System.Windows.Forms.ComboBox SetTempChartType;
        internal System.Windows.Forms.Label SetTemp;
        private System.Windows.Forms.Button HotPlate2ChartColor;
        private System.Windows.Forms.Button HotPlate1ChartColor;
        private System.Windows.Forms.Button ColdPlate2ChartColor;
        private System.Windows.Forms.Button ColdPlate1ChartColor;
        private System.Windows.Forms.ComboBox HotPlate2ChartType;
        private System.Windows.Forms.ComboBox HotPlate1ChartType;
        private System.Windows.Forms.ComboBox ColdPlate1ChartType;
        private System.Windows.Forms.ComboBox ColdPlate2ChartType;
        internal System.Windows.Forms.Label HotPlate2;
        internal System.Windows.Forms.Label HotPlate1;
        internal System.Windows.Forms.Label ColdPlate2;
        internal System.Windows.Forms.Label ColdPlate1;
    }
}