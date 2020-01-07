namespace Chiller
{
    partial class UnitWorkStateChart
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
            this.ChartHightPressure = new System.Windows.Forms.TextBox();
            this.ChartLovPressure = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel67 = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.ChartRefreshTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ChartPointLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ChartTypeAndColorB2 = new System.Windows.Forms.Panel();
            this.CondenserOutTempChartColor = new System.Windows.Forms.Button();
            this.CompressorOutTempChartColor = new System.Windows.Forms.Button();
            this.HeatExchangerOutPressureChartColor = new System.Windows.Forms.Button();
            this.CompressorOutPressureChartColor = new System.Windows.Forms.Button();
            this.CondenserOutPressureChartColor = new System.Windows.Forms.Button();
            this.EvaporatorInTempChartColor = new System.Windows.Forms.Button();
            this.EvaporatorOutPressureChartColor = new System.Windows.Forms.Button();
            this.EvaporatorInPressureChartColor = new System.Windows.Forms.Button();
            this.CondenserInPressureChartColor = new System.Windows.Forms.Button();
            this.CompressorInPressureChartColor = new System.Windows.Forms.Button();
            this.CondenserOutTempChartType = new System.Windows.Forms.ComboBox();
            this.CompressorOutTempChartType = new System.Windows.Forms.ComboBox();
            this.HeatExchangerOutPressureChartType = new System.Windows.Forms.ComboBox();
            this.EvaporatorInTempChartType = new System.Windows.Forms.ComboBox();
            this.EvaporatorOutPressureChartType = new System.Windows.Forms.ComboBox();
            this.EvaporatorInPressureChartType = new System.Windows.Forms.ComboBox();
            this.CondenserInPressureChartType = new System.Windows.Forms.ComboBox();
            this.CondenserOutPressureChartType = new System.Windows.Forms.ComboBox();
            this.CompressorOutPressureChartType = new System.Windows.Forms.ComboBox();
            this.CompressorInPressureChartType = new System.Windows.Forms.ComboBox();
            this.CondenserOutTemp = new System.Windows.Forms.Label();
            this.CompressorOutTemp = new System.Windows.Forms.Label();
            this.HeatExchangerOutPressure = new System.Windows.Forms.Label();
            this.CondenserOutPressure = new System.Windows.Forms.Label();
            this.CompressorOutPressure = new System.Windows.Forms.Label();
            this.EvaporatorInTemp = new System.Windows.Forms.Label();
            this.EvaporatorOutPressure = new System.Windows.Forms.Label();
            this.EvaporatorInPressure = new System.Windows.Forms.Label();
            this.CondenserInPressure = new System.Windows.Forms.Label();
            this.CompressorInPressure = new System.Windows.Forms.Label();
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
            this.panel51.Controls.Add(this.ChartHightPressure);
            this.panel51.Controls.Add(this.ChartLovPressure);
            this.panel51.Controls.Add(this.label8);
            this.panel51.Controls.Add(this.Password_lb);
            this.panel51.Controls.Add(this.panel67);
            this.panel51.Controls.Add(this.ChartRefreshTime);
            this.panel51.Controls.Add(this.label2);
            this.panel51.Controls.Add(this.ChartPointLength);
            this.panel51.Controls.Add(this.label5);
            this.panel51.Controls.Add(this.ChartHightTemp);
            this.panel51.Controls.Add(this.label6);
            this.panel51.Controls.Add(this.label1);
            this.panel51.Controls.Add(this.ChartLovTemp);
            this.panel51.Font = new System.Drawing.Font("宋体", 10F);
            this.panel51.Location = new System.Drawing.Point(12, 12);
            this.panel51.Name = "panel51";
            this.panel51.Size = new System.Drawing.Size(279, 530);
            this.panel51.TabIndex = 57;
            // 
            // ChartHightPressure
            // 
            this.ChartHightPressure.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartHightPressure.Location = new System.Drawing.Point(167, 189);
            this.ChartHightPressure.Name = "ChartHightPressure";
            this.ChartHightPressure.Size = new System.Drawing.Size(90, 35);
            this.ChartHightPressure.TabIndex = 4;
            this.ChartHightPressure.Text = "40";
            this.ChartHightPressure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartHightPressure.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // ChartLovPressure
            // 
            this.ChartLovPressure.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartLovPressure.Location = new System.Drawing.Point(167, 148);
            this.ChartLovPressure.Name = "ChartLovPressure";
            this.ChartLovPressure.Size = new System.Drawing.Size(90, 35);
            this.ChartLovPressure.TabIndex = 3;
            this.ChartLovPressure.Text = "-100";
            this.ChartLovPressure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartLovPressure.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(12, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 20);
            this.label8.TabIndex = 54;
            this.label8.Text = "最低压力（Mpa）";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ChartRefreshTime.Location = new System.Drawing.Point(167, 271);
            this.ChartRefreshTime.Name = "ChartRefreshTime";
            this.ChartRefreshTime.Size = new System.Drawing.Size(90, 35);
            this.ChartRefreshTime.TabIndex = 6;
            this.ChartRefreshTime.Text = "40";
            this.ChartRefreshTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartRefreshTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Int);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 20);
            this.label2.TabIndex = 54;
            this.label2.Text = "刷新频率（s）";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChartPointLength
            // 
            this.ChartPointLength.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChartPointLength.Location = new System.Drawing.Point(167, 230);
            this.ChartPointLength.Name = "ChartPointLength";
            this.ChartPointLength.Size = new System.Drawing.Size(90, 35);
            this.ChartPointLength.TabIndex = 5;
            this.ChartPointLength.Text = "40";
            this.ChartPointLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChartPointLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Int);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(12, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 20);
            this.label5.TabIndex = 54;
            this.label5.Text = "显示长度（min）";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(12, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 20);
            this.label6.TabIndex = 54;
            this.label6.Text = "最高压力（Mpa）";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChartTypeAndColorB2
            // 
            this.ChartTypeAndColorB2.BackColor = System.Drawing.Color.White;
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.HeatExchangerOutPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorOutPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserInPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorInPressureChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.HeatExchangerOutPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorOutPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserInPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorInPressureChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.HeatExchangerOutPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserOutPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorOutPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorOutPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.EvaporatorInPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.CondenserInPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.CompressorInPressure);
            this.ChartTypeAndColorB2.Controls.Add(this.ChartTypeAndColorB1);
            this.ChartTypeAndColorB2.Font = new System.Drawing.Font("宋体", 10F);
            this.ChartTypeAndColorB2.Location = new System.Drawing.Point(297, 12);
            this.ChartTypeAndColorB2.Name = "ChartTypeAndColorB2";
            this.ChartTypeAndColorB2.Size = new System.Drawing.Size(823, 530);
            this.ChartTypeAndColorB2.TabIndex = 58;
            // 
            // CondenserOutTempChartColor
            // 
            this.CondenserOutTempChartColor.Location = new System.Drawing.Point(392, 412);
            this.CondenserOutTempChartColor.Name = "CondenserOutTempChartColor";
            this.CondenserOutTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.CondenserOutTempChartColor.TabIndex = 56;
            this.CondenserOutTempChartColor.TabStop = false;
            this.CondenserOutTempChartColor.UseVisualStyleBackColor = true;
            this.CondenserOutTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CompressorOutTempChartColor
            // 
            this.CompressorOutTempChartColor.Location = new System.Drawing.Point(122, 412);
            this.CompressorOutTempChartColor.Name = "CompressorOutTempChartColor";
            this.CompressorOutTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.CompressorOutTempChartColor.TabIndex = 56;
            this.CompressorOutTempChartColor.TabStop = false;
            this.CompressorOutTempChartColor.UseVisualStyleBackColor = true;
            this.CompressorOutTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // HeatExchangerOutPressureChartColor
            // 
            this.HeatExchangerOutPressureChartColor.Location = new System.Drawing.Point(122, 299);
            this.HeatExchangerOutPressureChartColor.Name = "HeatExchangerOutPressureChartColor";
            this.HeatExchangerOutPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.HeatExchangerOutPressureChartColor.TabIndex = 56;
            this.HeatExchangerOutPressureChartColor.TabStop = false;
            this.HeatExchangerOutPressureChartColor.UseVisualStyleBackColor = true;
            this.HeatExchangerOutPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CompressorOutPressureChartColor
            // 
            this.CompressorOutPressureChartColor.Location = new System.Drawing.Point(122, 186);
            this.CompressorOutPressureChartColor.Name = "CompressorOutPressureChartColor";
            this.CompressorOutPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.CompressorOutPressureChartColor.TabIndex = 56;
            this.CompressorOutPressureChartColor.TabStop = false;
            this.CompressorOutPressureChartColor.UseVisualStyleBackColor = true;
            this.CompressorOutPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CondenserOutPressureChartColor
            // 
            this.CondenserOutPressureChartColor.Location = new System.Drawing.Point(392, 186);
            this.CondenserOutPressureChartColor.Name = "CondenserOutPressureChartColor";
            this.CondenserOutPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.CondenserOutPressureChartColor.TabIndex = 56;
            this.CondenserOutPressureChartColor.TabStop = false;
            this.CondenserOutPressureChartColor.UseVisualStyleBackColor = true;
            this.CondenserOutPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // EvaporatorInTempChartColor
            // 
            this.EvaporatorInTempChartColor.Location = new System.Drawing.Point(662, 412);
            this.EvaporatorInTempChartColor.Name = "EvaporatorInTempChartColor";
            this.EvaporatorInTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.EvaporatorInTempChartColor.TabIndex = 56;
            this.EvaporatorInTempChartColor.TabStop = false;
            this.EvaporatorInTempChartColor.UseVisualStyleBackColor = true;
            this.EvaporatorInTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // EvaporatorOutPressureChartColor
            // 
            this.EvaporatorOutPressureChartColor.Location = new System.Drawing.Point(662, 186);
            this.EvaporatorOutPressureChartColor.Name = "EvaporatorOutPressureChartColor";
            this.EvaporatorOutPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.EvaporatorOutPressureChartColor.TabIndex = 56;
            this.EvaporatorOutPressureChartColor.TabStop = false;
            this.EvaporatorOutPressureChartColor.UseVisualStyleBackColor = true;
            this.EvaporatorOutPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // EvaporatorInPressureChartColor
            // 
            this.EvaporatorInPressureChartColor.Location = new System.Drawing.Point(662, 69);
            this.EvaporatorInPressureChartColor.Name = "EvaporatorInPressureChartColor";
            this.EvaporatorInPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.EvaporatorInPressureChartColor.TabIndex = 56;
            this.EvaporatorInPressureChartColor.TabStop = false;
            this.EvaporatorInPressureChartColor.UseVisualStyleBackColor = true;
            this.EvaporatorInPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CondenserInPressureChartColor
            // 
            this.CondenserInPressureChartColor.Location = new System.Drawing.Point(392, 73);
            this.CondenserInPressureChartColor.Name = "CondenserInPressureChartColor";
            this.CondenserInPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.CondenserInPressureChartColor.TabIndex = 56;
            this.CondenserInPressureChartColor.TabStop = false;
            this.CondenserInPressureChartColor.UseVisualStyleBackColor = true;
            this.CondenserInPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CompressorInPressureChartColor
            // 
            this.CompressorInPressureChartColor.Location = new System.Drawing.Point(122, 73);
            this.CompressorInPressureChartColor.Name = "CompressorInPressureChartColor";
            this.CompressorInPressureChartColor.Size = new System.Drawing.Size(138, 31);
            this.CompressorInPressureChartColor.TabIndex = 56;
            this.CompressorInPressureChartColor.TabStop = false;
            this.CompressorInPressureChartColor.UseVisualStyleBackColor = true;
            this.CompressorInPressureChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // CondenserOutTempChartType
            // 
            this.CondenserOutTempChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CondenserOutTempChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CondenserOutTempChartType.ItemHeight = 20;
            this.CondenserOutTempChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CondenserOutTempChartType.Location = new System.Drawing.Point(392, 449);
            this.CondenserOutTempChartType.Name = "CondenserOutTempChartType";
            this.CondenserOutTempChartType.Size = new System.Drawing.Size(138, 28);
            this.CondenserOutTempChartType.TabIndex = 55;
            this.CondenserOutTempChartType.TabStop = false;
            // 
            // CompressorOutTempChartType
            // 
            this.CompressorOutTempChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CompressorOutTempChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CompressorOutTempChartType.ItemHeight = 20;
            this.CompressorOutTempChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CompressorOutTempChartType.Location = new System.Drawing.Point(122, 449);
            this.CompressorOutTempChartType.Name = "CompressorOutTempChartType";
            this.CompressorOutTempChartType.Size = new System.Drawing.Size(138, 28);
            this.CompressorOutTempChartType.TabIndex = 55;
            this.CompressorOutTempChartType.TabStop = false;
            // 
            // HeatExchangerOutPressureChartType
            // 
            this.HeatExchangerOutPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HeatExchangerOutPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.HeatExchangerOutPressureChartType.ItemHeight = 20;
            this.HeatExchangerOutPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.HeatExchangerOutPressureChartType.Location = new System.Drawing.Point(122, 336);
            this.HeatExchangerOutPressureChartType.Name = "HeatExchangerOutPressureChartType";
            this.HeatExchangerOutPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.HeatExchangerOutPressureChartType.TabIndex = 55;
            this.HeatExchangerOutPressureChartType.TabStop = false;
            // 
            // EvaporatorInTempChartType
            // 
            this.EvaporatorInTempChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EvaporatorInTempChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.EvaporatorInTempChartType.ItemHeight = 20;
            this.EvaporatorInTempChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.EvaporatorInTempChartType.Location = new System.Drawing.Point(662, 449);
            this.EvaporatorInTempChartType.Name = "EvaporatorInTempChartType";
            this.EvaporatorInTempChartType.Size = new System.Drawing.Size(138, 28);
            this.EvaporatorInTempChartType.TabIndex = 55;
            this.EvaporatorInTempChartType.TabStop = false;
            // 
            // EvaporatorOutPressureChartType
            // 
            this.EvaporatorOutPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EvaporatorOutPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.EvaporatorOutPressureChartType.ItemHeight = 20;
            this.EvaporatorOutPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.EvaporatorOutPressureChartType.Location = new System.Drawing.Point(662, 223);
            this.EvaporatorOutPressureChartType.Name = "EvaporatorOutPressureChartType";
            this.EvaporatorOutPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.EvaporatorOutPressureChartType.TabIndex = 55;
            this.EvaporatorOutPressureChartType.TabStop = false;
            // 
            // EvaporatorInPressureChartType
            // 
            this.EvaporatorInPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EvaporatorInPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.EvaporatorInPressureChartType.ItemHeight = 20;
            this.EvaporatorInPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.EvaporatorInPressureChartType.Location = new System.Drawing.Point(662, 106);
            this.EvaporatorInPressureChartType.Name = "EvaporatorInPressureChartType";
            this.EvaporatorInPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.EvaporatorInPressureChartType.TabIndex = 55;
            this.EvaporatorInPressureChartType.TabStop = false;
            // 
            // CondenserInPressureChartType
            // 
            this.CondenserInPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CondenserInPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CondenserInPressureChartType.ItemHeight = 20;
            this.CondenserInPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CondenserInPressureChartType.Location = new System.Drawing.Point(392, 110);
            this.CondenserInPressureChartType.Name = "CondenserInPressureChartType";
            this.CondenserInPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.CondenserInPressureChartType.TabIndex = 55;
            this.CondenserInPressureChartType.TabStop = false;
            // 
            // CondenserOutPressureChartType
            // 
            this.CondenserOutPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CondenserOutPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CondenserOutPressureChartType.ItemHeight = 20;
            this.CondenserOutPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CondenserOutPressureChartType.Location = new System.Drawing.Point(392, 223);
            this.CondenserOutPressureChartType.Name = "CondenserOutPressureChartType";
            this.CondenserOutPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.CondenserOutPressureChartType.TabIndex = 55;
            this.CondenserOutPressureChartType.TabStop = false;
            // 
            // CompressorOutPressureChartType
            // 
            this.CompressorOutPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CompressorOutPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CompressorOutPressureChartType.ItemHeight = 20;
            this.CompressorOutPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CompressorOutPressureChartType.Location = new System.Drawing.Point(122, 223);
            this.CompressorOutPressureChartType.Name = "CompressorOutPressureChartType";
            this.CompressorOutPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.CompressorOutPressureChartType.TabIndex = 55;
            this.CompressorOutPressureChartType.TabStop = false;
            // 
            // CompressorInPressureChartType
            // 
            this.CompressorInPressureChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CompressorInPressureChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.CompressorInPressureChartType.ItemHeight = 20;
            this.CompressorInPressureChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.CompressorInPressureChartType.Location = new System.Drawing.Point(122, 110);
            this.CompressorInPressureChartType.Name = "CompressorInPressureChartType";
            this.CompressorInPressureChartType.Size = new System.Drawing.Size(138, 28);
            this.CompressorInPressureChartType.TabIndex = 55;
            this.CompressorInPressureChartType.TabStop = false;
            // 
            // CondenserOutTemp
            // 
            this.CondenserOutTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CondenserOutTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CondenserOutTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserOutTemp.ForeColor = System.Drawing.Color.Black;
            this.CondenserOutTemp.Location = new System.Drawing.Point(289, 412);
            this.CondenserOutTemp.Name = "CondenserOutTemp";
            this.CondenserOutTemp.Size = new System.Drawing.Size(97, 65);
            this.CondenserOutTemp.TabIndex = 54;
            this.CondenserOutTemp.Text = "冷凝器 出口温度";
            this.CondenserOutTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorOutTemp
            // 
            this.CompressorOutTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CompressorOutTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CompressorOutTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorOutTemp.ForeColor = System.Drawing.Color.Black;
            this.CompressorOutTemp.Location = new System.Drawing.Point(19, 412);
            this.CompressorOutTemp.Name = "CompressorOutTemp";
            this.CompressorOutTemp.Size = new System.Drawing.Size(97, 65);
            this.CompressorOutTemp.TabIndex = 54;
            this.CompressorOutTemp.Text = "压缩机 出口温度";
            this.CompressorOutTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HeatExchangerOutPressure
            // 
            this.HeatExchangerOutPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.HeatExchangerOutPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeatExchangerOutPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HeatExchangerOutPressure.ForeColor = System.Drawing.Color.Black;
            this.HeatExchangerOutPressure.Location = new System.Drawing.Point(19, 299);
            this.HeatExchangerOutPressure.Name = "HeatExchangerOutPressure";
            this.HeatExchangerOutPressure.Size = new System.Drawing.Size(97, 65);
            this.HeatExchangerOutPressure.TabIndex = 54;
            this.HeatExchangerOutPressure.Text = "回热器 出口压力";
            this.HeatExchangerOutPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CondenserOutPressure
            // 
            this.CondenserOutPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CondenserOutPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CondenserOutPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserOutPressure.ForeColor = System.Drawing.Color.Black;
            this.CondenserOutPressure.Location = new System.Drawing.Point(289, 186);
            this.CondenserOutPressure.Name = "CondenserOutPressure";
            this.CondenserOutPressure.Size = new System.Drawing.Size(97, 65);
            this.CondenserOutPressure.TabIndex = 54;
            this.CondenserOutPressure.Text = "冷凝器 出口压力";
            this.CondenserOutPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorOutPressure
            // 
            this.CompressorOutPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CompressorOutPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CompressorOutPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorOutPressure.ForeColor = System.Drawing.Color.Black;
            this.CompressorOutPressure.Location = new System.Drawing.Point(19, 186);
            this.CompressorOutPressure.Name = "CompressorOutPressure";
            this.CompressorOutPressure.Size = new System.Drawing.Size(97, 65);
            this.CompressorOutPressure.TabIndex = 54;
            this.CompressorOutPressure.Text = "压缩机 出口压力";
            this.CompressorOutPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorInTemp
            // 
            this.EvaporatorInTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.EvaporatorInTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EvaporatorInTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorInTemp.ForeColor = System.Drawing.Color.Black;
            this.EvaporatorInTemp.Location = new System.Drawing.Point(559, 412);
            this.EvaporatorInTemp.Name = "EvaporatorInTemp";
            this.EvaporatorInTemp.Size = new System.Drawing.Size(97, 65);
            this.EvaporatorInTemp.TabIndex = 54;
            this.EvaporatorInTemp.Text = "蒸发器 入口温度";
            this.EvaporatorInTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorOutPressure
            // 
            this.EvaporatorOutPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.EvaporatorOutPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EvaporatorOutPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorOutPressure.ForeColor = System.Drawing.Color.Black;
            this.EvaporatorOutPressure.Location = new System.Drawing.Point(559, 186);
            this.EvaporatorOutPressure.Name = "EvaporatorOutPressure";
            this.EvaporatorOutPressure.Size = new System.Drawing.Size(97, 65);
            this.EvaporatorOutPressure.TabIndex = 54;
            this.EvaporatorOutPressure.Text = "蒸发器 出口压力";
            this.EvaporatorOutPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorInPressure
            // 
            this.EvaporatorInPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.EvaporatorInPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EvaporatorInPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorInPressure.ForeColor = System.Drawing.Color.Black;
            this.EvaporatorInPressure.Location = new System.Drawing.Point(559, 69);
            this.EvaporatorInPressure.Name = "EvaporatorInPressure";
            this.EvaporatorInPressure.Size = new System.Drawing.Size(97, 65);
            this.EvaporatorInPressure.TabIndex = 54;
            this.EvaporatorInPressure.Text = "蒸发器 入口压力";
            this.EvaporatorInPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CondenserInPressure
            // 
            this.CondenserInPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CondenserInPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CondenserInPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserInPressure.ForeColor = System.Drawing.Color.Black;
            this.CondenserInPressure.Location = new System.Drawing.Point(289, 73);
            this.CondenserInPressure.Name = "CondenserInPressure";
            this.CondenserInPressure.Size = new System.Drawing.Size(97, 65);
            this.CondenserInPressure.TabIndex = 54;
            this.CondenserInPressure.Text = "冷凝器 入口压力";
            this.CondenserInPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorInPressure
            // 
            this.CompressorInPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CompressorInPressure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CompressorInPressure.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorInPressure.ForeColor = System.Drawing.Color.Black;
            this.CompressorInPressure.Location = new System.Drawing.Point(19, 73);
            this.CompressorInPressure.Name = "CompressorInPressure";
            this.CompressorInPressure.Size = new System.Drawing.Size(97, 65);
            this.CompressorInPressure.TabIndex = 54;
            this.CompressorInPressure.Text = "压缩机 入口压力";
            this.CompressorInPressure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ChartTypeAndColorB1.Size = new System.Drawing.Size(823, 55);
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
            this.ChartTypeAndColor.Size = new System.Drawing.Size(823, 55);
            this.ChartTypeAndColor.TabIndex = 6;
            this.ChartTypeAndColor.Text = "曲线类型以及颜色设置";
            this.ChartTypeAndColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChartSetApply
            // 
            this.ChartSetApply.CornerRadius = 10;
            this.ChartSetApply.CustomerInformation = null;
            this.ChartSetApply.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.ChartSetApply.Location = new System.Drawing.Point(817, 558);
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
            this.ChartSetClose.Location = new System.Drawing.Point(982, 558);
            this.ChartSetClose.Name = "ChartSetClose";
            this.ChartSetClose.Size = new System.Drawing.Size(138, 63);
            this.ChartSetClose.TabIndex = 59;
            this.ChartSetClose.Text = "关闭";
            this.ChartSetClose.Click += new System.EventHandler(this.ChartSetCancel_Click);
            // 
            // UnitWorkStateChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 633);
            this.ControlBox = false;
            this.Controls.Add(this.ChartSetClose);
            this.Controls.Add(this.ChartSetApply);
            this.Controls.Add(this.ChartTypeAndColorB2);
            this.Controls.Add(this.panel51);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnitWorkStateChart";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UnitWorkStateChart";
            this.Load += new System.EventHandler(this.UnitWorkStateChart_Load);
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
        internal System.Windows.Forms.Label CompressorInPressure;
        private System.Windows.Forms.Panel ChartTypeAndColorB1;
        private System.Windows.Forms.Label ChartTypeAndColor;
        internal System.Windows.Forms.Label HeatExchangerOutPressure;
        internal System.Windows.Forms.Label CondenserOutPressure;
        internal System.Windows.Forms.Label CompressorOutPressure;
        internal System.Windows.Forms.Label CondenserInPressure;
        private System.Windows.Forms.ComboBox HeatExchangerOutPressureChartType;
        private System.Windows.Forms.ComboBox CondenserInPressureChartType;
        private System.Windows.Forms.ComboBox CondenserOutPressureChartType;
        private System.Windows.Forms.ComboBox CompressorOutPressureChartType;
        private System.Windows.Forms.ComboBox CompressorInPressureChartType;
        private HslControls.HslButton ChartSetApply;
        private HslControls.HslButton ChartSetClose;
        private System.Windows.Forms.Button CompressorInPressureChartColor;
        private System.Windows.Forms.Button HeatExchangerOutPressureChartColor;
        private System.Windows.Forms.Button CompressorOutPressureChartColor;
        private System.Windows.Forms.Button CondenserOutPressureChartColor;
        private System.Windows.Forms.Button CondenserInPressureChartColor;
        public System.Windows.Forms.TextBox ChartRefreshTime;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CondenserOutTempChartColor;
        private System.Windows.Forms.Button CompressorOutTempChartColor;
        private System.Windows.Forms.ComboBox CondenserOutTempChartType;
        private System.Windows.Forms.ComboBox CompressorOutTempChartType;
        internal System.Windows.Forms.Label CondenserOutTemp;
        internal System.Windows.Forms.Label CompressorOutTemp;
        private System.Windows.Forms.Button EvaporatorInTempChartColor;
        private System.Windows.Forms.Button EvaporatorOutPressureChartColor;
        private System.Windows.Forms.Button EvaporatorInPressureChartColor;
        private System.Windows.Forms.ComboBox EvaporatorInTempChartType;
        private System.Windows.Forms.ComboBox EvaporatorOutPressureChartType;
        private System.Windows.Forms.ComboBox EvaporatorInPressureChartType;
        internal System.Windows.Forms.Label EvaporatorInTemp;
        internal System.Windows.Forms.Label EvaporatorOutPressure;
        internal System.Windows.Forms.Label EvaporatorInPressure;
        public System.Windows.Forms.TextBox ChartHightPressure;
        public System.Windows.Forms.TextBox ChartLovPressure;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label6;
    }
}