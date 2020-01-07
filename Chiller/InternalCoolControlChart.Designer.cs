namespace Chiller
{
    partial class InternalColdControlChart
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
            this.NowTempChartColor = new System.Windows.Forms.Button();
            this.SetTempChartColor = new System.Windows.Forms.Button();
            this.Unit3ChartColor = new System.Windows.Forms.Button();
            this.Unit4ChartColor = new System.Windows.Forms.Button();
            this.Unit2ChartColor = new System.Windows.Forms.Button();
            this.Unit1ChartColor = new System.Windows.Forms.Button();
            this.NowTempChartType = new System.Windows.Forms.ComboBox();
            this.SetTempChartType = new System.Windows.Forms.ComboBox();
            this.Unit2ChartType = new System.Windows.Forms.ComboBox();
            this.Unit4ChartType = new System.Windows.Forms.ComboBox();
            this.Unit3ChartType = new System.Windows.Forms.ComboBox();
            this.Unit1ChartType = new System.Windows.Forms.ComboBox();
            this.NowTemp = new System.Windows.Forms.Label();
            this.SetTemp = new System.Windows.Forms.Label();
            this.Unit4 = new System.Windows.Forms.Label();
            this.Unit3 = new System.Windows.Forms.Label();
            this.Unit2 = new System.Windows.Forms.Label();
            this.Unit1 = new System.Windows.Forms.Label();
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
            this.panel51.Size = new System.Drawing.Size(279, 403);
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
            this.ChartTypeAndColorB2.Controls.Add(this.NowTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTempChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit3ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit4ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit2ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit1ChartColor);
            this.ChartTypeAndColorB2.Controls.Add(this.NowTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTempChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit2ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit4ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit3ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit1ChartType);
            this.ChartTypeAndColorB2.Controls.Add(this.NowTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.SetTemp);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit4);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit3);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit2);
            this.ChartTypeAndColorB2.Controls.Add(this.Unit1);
            this.ChartTypeAndColorB2.Controls.Add(this.ChartTypeAndColorB1);
            this.ChartTypeAndColorB2.Font = new System.Drawing.Font("宋体", 10F);
            this.ChartTypeAndColorB2.Location = new System.Drawing.Point(297, 12);
            this.ChartTypeAndColorB2.Name = "ChartTypeAndColorB2";
            this.ChartTypeAndColorB2.Size = new System.Drawing.Size(567, 403);
            this.ChartTypeAndColorB2.TabIndex = 58;
            // 
            // NowTempChartColor
            // 
            this.NowTempChartColor.Location = new System.Drawing.Point(399, 295);
            this.NowTempChartColor.Name = "NowTempChartColor";
            this.NowTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.NowTempChartColor.TabIndex = 56;
            this.NowTempChartColor.TabStop = false;
            this.NowTempChartColor.UseVisualStyleBackColor = true;
            this.NowTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // SetTempChartColor
            // 
            this.SetTempChartColor.Location = new System.Drawing.Point(122, 295);
            this.SetTempChartColor.Name = "SetTempChartColor";
            this.SetTempChartColor.Size = new System.Drawing.Size(138, 31);
            this.SetTempChartColor.TabIndex = 56;
            this.SetTempChartColor.TabStop = false;
            this.SetTempChartColor.UseVisualStyleBackColor = true;
            this.SetTempChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // Unit3ChartColor
            // 
            this.Unit3ChartColor.Location = new System.Drawing.Point(122, 184);
            this.Unit3ChartColor.Name = "Unit3ChartColor";
            this.Unit3ChartColor.Size = new System.Drawing.Size(138, 31);
            this.Unit3ChartColor.TabIndex = 56;
            this.Unit3ChartColor.TabStop = false;
            this.Unit3ChartColor.UseVisualStyleBackColor = true;
            this.Unit3ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // Unit4ChartColor
            // 
            this.Unit4ChartColor.Location = new System.Drawing.Point(399, 184);
            this.Unit4ChartColor.Name = "Unit4ChartColor";
            this.Unit4ChartColor.Size = new System.Drawing.Size(138, 31);
            this.Unit4ChartColor.TabIndex = 56;
            this.Unit4ChartColor.TabStop = false;
            this.Unit4ChartColor.UseVisualStyleBackColor = true;
            this.Unit4ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // Unit2ChartColor
            // 
            this.Unit2ChartColor.Location = new System.Drawing.Point(399, 73);
            this.Unit2ChartColor.Name = "Unit2ChartColor";
            this.Unit2ChartColor.Size = new System.Drawing.Size(138, 31);
            this.Unit2ChartColor.TabIndex = 56;
            this.Unit2ChartColor.TabStop = false;
            this.Unit2ChartColor.UseVisualStyleBackColor = true;
            this.Unit2ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // Unit1ChartColor
            // 
            this.Unit1ChartColor.Location = new System.Drawing.Point(122, 73);
            this.Unit1ChartColor.Name = "Unit1ChartColor";
            this.Unit1ChartColor.Size = new System.Drawing.Size(138, 31);
            this.Unit1ChartColor.TabIndex = 56;
            this.Unit1ChartColor.TabStop = false;
            this.Unit1ChartColor.UseVisualStyleBackColor = true;
            this.Unit1ChartColor.Click += new System.EventHandler(this.UnitChartColorChange_Click);
            // 
            // NowTempChartType
            // 
            this.NowTempChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NowTempChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.NowTempChartType.ItemHeight = 20;
            this.NowTempChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.NowTempChartType.Location = new System.Drawing.Point(399, 332);
            this.NowTempChartType.Name = "NowTempChartType";
            this.NowTempChartType.Size = new System.Drawing.Size(138, 28);
            this.NowTempChartType.TabIndex = 55;
            this.NowTempChartType.TabStop = false;
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
            this.SetTempChartType.Location = new System.Drawing.Point(122, 332);
            this.SetTempChartType.Name = "SetTempChartType";
            this.SetTempChartType.Size = new System.Drawing.Size(138, 28);
            this.SetTempChartType.TabIndex = 55;
            this.SetTempChartType.TabStop = false;
            // 
            // Unit2ChartType
            // 
            this.Unit2ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Unit2ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.Unit2ChartType.ItemHeight = 20;
            this.Unit2ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.Unit2ChartType.Location = new System.Drawing.Point(399, 110);
            this.Unit2ChartType.Name = "Unit2ChartType";
            this.Unit2ChartType.Size = new System.Drawing.Size(138, 28);
            this.Unit2ChartType.TabIndex = 55;
            this.Unit2ChartType.TabStop = false;
            // 
            // Unit4ChartType
            // 
            this.Unit4ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Unit4ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.Unit4ChartType.ItemHeight = 20;
            this.Unit4ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.Unit4ChartType.Location = new System.Drawing.Point(399, 221);
            this.Unit4ChartType.Name = "Unit4ChartType";
            this.Unit4ChartType.Size = new System.Drawing.Size(138, 28);
            this.Unit4ChartType.TabIndex = 55;
            this.Unit4ChartType.TabStop = false;
            // 
            // Unit3ChartType
            // 
            this.Unit3ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Unit3ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.Unit3ChartType.ItemHeight = 20;
            this.Unit3ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.Unit3ChartType.Location = new System.Drawing.Point(122, 221);
            this.Unit3ChartType.Name = "Unit3ChartType";
            this.Unit3ChartType.Size = new System.Drawing.Size(138, 28);
            this.Unit3ChartType.TabIndex = 55;
            this.Unit3ChartType.TabStop = false;
            // 
            // Unit1ChartType
            // 
            this.Unit1ChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Unit1ChartType.Font = new System.Drawing.Font("宋体", 15F);
            this.Unit1ChartType.ItemHeight = 20;
            this.Unit1ChartType.Items.AddRange(new object[] {
            "Point",
            "FasePoint",
            "Bubble",
            "Line",
            "Spline",
            "Stepline",
            "Fastline"});
            this.Unit1ChartType.Location = new System.Drawing.Point(122, 110);
            this.Unit1ChartType.Name = "Unit1ChartType";
            this.Unit1ChartType.Size = new System.Drawing.Size(138, 28);
            this.Unit1ChartType.TabIndex = 55;
            this.Unit1ChartType.TabStop = false;
            // 
            // NowTemp
            // 
            this.NowTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NowTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NowTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NowTemp.ForeColor = System.Drawing.Color.Black;
            this.NowTemp.Location = new System.Drawing.Point(296, 295);
            this.NowTemp.Name = "NowTemp";
            this.NowTemp.Size = new System.Drawing.Size(97, 65);
            this.NowTemp.TabIndex = 54;
            this.NowTemp.Text = "当前温度";
            this.NowTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetTemp
            // 
            this.SetTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SetTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SetTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SetTemp.ForeColor = System.Drawing.Color.Black;
            this.SetTemp.Location = new System.Drawing.Point(19, 295);
            this.SetTemp.Name = "SetTemp";
            this.SetTemp.Size = new System.Drawing.Size(97, 65);
            this.SetTemp.TabIndex = 54;
            this.SetTemp.Text = "设定温度";
            this.SetTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Unit4
            // 
            this.Unit4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Unit4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Unit4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Unit4.ForeColor = System.Drawing.Color.Black;
            this.Unit4.Location = new System.Drawing.Point(296, 184);
            this.Unit4.Name = "Unit4";
            this.Unit4.Size = new System.Drawing.Size(97, 65);
            this.Unit4.TabIndex = 54;
            this.Unit4.Text = "Unit4";
            this.Unit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Unit3
            // 
            this.Unit3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Unit3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Unit3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Unit3.ForeColor = System.Drawing.Color.Black;
            this.Unit3.Location = new System.Drawing.Point(19, 184);
            this.Unit3.Name = "Unit3";
            this.Unit3.Size = new System.Drawing.Size(97, 65);
            this.Unit3.TabIndex = 54;
            this.Unit3.Text = "Unit3";
            this.Unit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Unit2
            // 
            this.Unit2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Unit2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Unit2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Unit2.ForeColor = System.Drawing.Color.Black;
            this.Unit2.Location = new System.Drawing.Point(296, 73);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(97, 65);
            this.Unit2.TabIndex = 54;
            this.Unit2.Text = "Unit2";
            this.Unit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Unit1
            // 
            this.Unit1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Unit1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Unit1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Unit1.ForeColor = System.Drawing.Color.Black;
            this.Unit1.Location = new System.Drawing.Point(19, 73);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(97, 65);
            this.Unit1.TabIndex = 54;
            this.Unit1.Text = "Unit1";
            this.Unit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ChartTypeAndColorB1.Size = new System.Drawing.Size(567, 55);
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
            this.ChartTypeAndColor.Size = new System.Drawing.Size(567, 55);
            this.ChartTypeAndColor.TabIndex = 6;
            this.ChartTypeAndColor.Text = "曲线类型以及颜色设置";
            this.ChartTypeAndColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChartSetApply
            // 
            this.ChartSetApply.CornerRadius = 10;
            this.ChartSetApply.CustomerInformation = null;
            this.ChartSetApply.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.ChartSetApply.Location = new System.Drawing.Point(531, 431);
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
            this.ChartSetClose.Location = new System.Drawing.Point(696, 431);
            this.ChartSetClose.Name = "ChartSetClose";
            this.ChartSetClose.Size = new System.Drawing.Size(138, 63);
            this.ChartSetClose.TabIndex = 59;
            this.ChartSetClose.Text = "关闭";
            this.ChartSetClose.Click += new System.EventHandler(this.ChartSetCancel_Click);
            // 
            // InternalColdControlChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 506);
            this.ControlBox = false;
            this.Controls.Add(this.ChartSetClose);
            this.Controls.Add(this.ChartSetApply);
            this.Controls.Add(this.ChartTypeAndColorB2);
            this.Controls.Add(this.panel51);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InternalColdControlChart";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "显示设置";
            this.Load += new System.EventHandler(this.InternalColdControlChart_Load);
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
        internal System.Windows.Forms.Label Unit1;
        private System.Windows.Forms.Panel ChartTypeAndColorB1;
        private System.Windows.Forms.Label ChartTypeAndColor;
        internal System.Windows.Forms.Label NowTemp;
        internal System.Windows.Forms.Label SetTemp;
        internal System.Windows.Forms.Label Unit4;
        internal System.Windows.Forms.Label Unit3;
        internal System.Windows.Forms.Label Unit2;
        private System.Windows.Forms.ComboBox NowTempChartType;
        private System.Windows.Forms.ComboBox SetTempChartType;
        private System.Windows.Forms.ComboBox Unit2ChartType;
        private System.Windows.Forms.ComboBox Unit4ChartType;
        private System.Windows.Forms.ComboBox Unit3ChartType;
        private System.Windows.Forms.ComboBox Unit1ChartType;
        private HslControls.HslButton ChartSetApply;
        private HslControls.HslButton ChartSetClose;
        private System.Windows.Forms.Button Unit1ChartColor;
        private System.Windows.Forms.Button NowTempChartColor;
        private System.Windows.Forms.Button SetTempChartColor;
        private System.Windows.Forms.Button Unit3ChartColor;
        private System.Windows.Forms.Button Unit4ChartColor;
        private System.Windows.Forms.Button Unit2ChartColor;
        public System.Windows.Forms.TextBox ChartRefreshTime;
        internal System.Windows.Forms.Label label2;
    }
}