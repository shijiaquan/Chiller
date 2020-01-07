namespace Chiller
{
    partial class UnitWorkState
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel2 = new System.Windows.Forms.Panel();
            this.OpenChartSet = new HslControls.HslButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Unit3 = new HslControls.HslSwitch();
            this.Unit2 = new HslControls.HslSwitch();
            this.Unit4 = new HslControls.HslSwitch();
            this.Unit1 = new HslControls.HslSwitch();
            this.CoolWater = new System.Windows.Forms.Label();
            this.CoolWaterOut2 = new HslControls.HslPipeLine();
            this.BypassInPipe1 = new HslControls.HslPipeLine();
            this.CoolWaterOut1 = new HslControls.HslPipeLine();
            this.CoolWaterIn = new HslControls.HslPipeLine();
            this.Condenser = new System.Windows.Forms.Label();
            this.Capillary = new System.Windows.Forms.Label();
            this.Compressor = new System.Windows.Forms.Label();
            this.CompressorOutPipe = new HslControls.HslPipeLine();
            this.CompressorInPipe = new HslControls.HslPipeLine();
            this.CoolWaterOut3 = new HslControls.HslPipeLine();
            this.HeatExchanger = new System.Windows.Forms.Label();
            this.CondenserOutPipe = new HslControls.HslPipeLine();
            this.Evaporator = new System.Windows.Forms.Label();
            this.EvaporatorOutPipe = new HslControls.HslPipeLine();
            this.HeatExchangerOutPipe = new HslControls.HslPipeLine();
            this.EvaporatorInPipe = new HslControls.HslPipeLine();
            this.CondenserTempOut = new System.Windows.Forms.Label();
            this.CompressorTempOut = new System.Windows.Forms.Label();
            this.EvaporatorTempIn = new System.Windows.Forms.Label();
            this.CompressorPressureOut = new System.Windows.Forms.Label();
            this.CondenserPressureOut = new System.Windows.Forms.Label();
            this.HeatExchangerPressureOut = new System.Windows.Forms.Label();
            this.EvaporatorPressureIn = new System.Windows.Forms.Label();
            this.EvaporatorPressureOut = new System.Windows.Forms.Label();
            this.CompressorPressureIn = new System.Windows.Forms.Label();
            this.CondenserPressureIn = new System.Windows.Forms.Label();
            this.BypassValve = new HslControls.HslValves();
            this.BypassInPipe2 = new HslControls.HslPipeLine();
            this.BypassCapillaryOut2 = new HslControls.HslPipeLine();
            this.BypassCapillaryOut1 = new HslControls.HslPipeLine();
            this.BypassCapillary = new System.Windows.Forms.Label();
            this.BypassOutPipe = new HslControls.HslPipeLine();
            this.ParallelCapillary = new System.Windows.Forms.Label();
            this.ParallelCapillaryOutPipe1 = new HslControls.HslPipeLine();
            this.ParallelOutPipe = new HslControls.HslPipeLine();
            this.ParallelValve = new HslControls.HslValves();
            this.ParallelInPipe = new HslControls.HslPipeLine();
            this.ParallelCapillaryOutPipe2 = new HslControls.HslPipeLine();
            this.UnitChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BranchPipe = new HslControls.HslPipeLine();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitChart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SkyBlue;
            this.panel2.Controls.Add(this.OpenChartSet);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 900);
            this.panel2.TabIndex = 34;
            // 
            // OpenChartSet
            // 
            this.OpenChartSet.CornerRadius = 10;
            this.OpenChartSet.CustomerInformation = null;
            this.OpenChartSet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenChartSet.Location = new System.Drawing.Point(20, 25);
            this.OpenChartSet.Name = "OpenChartSet";
            this.OpenChartSet.Size = new System.Drawing.Size(160, 55);
            this.OpenChartSet.TabIndex = 41;
            this.OpenChartSet.Text = "曲线设定";
            this.OpenChartSet.Click += new System.EventHandler(this.OpenChartSet_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Unit3);
            this.groupBox3.Controls.Add(this.Unit2);
            this.groupBox3.Controls.Add(this.Unit4);
            this.groupBox3.Controls.Add(this.Unit1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(3, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(194, 793);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通道选择";
            // 
            // Unit3
            // 
            this.Unit3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Unit3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Unit3.ForeColor = System.Drawing.Color.Blue;
            this.Unit3.Location = new System.Drawing.Point(16, 410);
            this.Unit3.Name = "Unit3";
            this.Unit3.Size = new System.Drawing.Size(158, 180);
            this.Unit3.SwitchForeground = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Unit3.TabIndex = 35;
            this.Unit3.Text = "Unit3";
            this.Unit3.Click += new System.EventHandler(this.SwitchStatusChange_Click);
            // 
            // Unit2
            // 
            this.Unit2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Unit2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Unit2.ForeColor = System.Drawing.Color.Blue;
            this.Unit2.Location = new System.Drawing.Point(16, 222);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(158, 180);
            this.Unit2.SwitchForeground = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Unit2.TabIndex = 35;
            this.Unit2.Text = "Unit2";
            this.Unit2.Click += new System.EventHandler(this.SwitchStatusChange_Click);
            // 
            // Unit4
            // 
            this.Unit4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Unit4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Unit4.ForeColor = System.Drawing.Color.Blue;
            this.Unit4.Location = new System.Drawing.Point(16, 598);
            this.Unit4.Name = "Unit4";
            this.Unit4.Size = new System.Drawing.Size(158, 180);
            this.Unit4.SwitchForeground = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Unit4.TabIndex = 35;
            this.Unit4.Text = "Unit4";
            this.Unit4.Click += new System.EventHandler(this.SwitchStatusChange_Click);
            // 
            // Unit1
            // 
            this.Unit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Unit1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Unit1.ForeColor = System.Drawing.Color.Blue;
            this.Unit1.Location = new System.Drawing.Point(16, 34);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(158, 180);
            this.Unit1.SwitchForeground = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Unit1.TabIndex = 35;
            this.Unit1.Text = "Unit1";
            this.Unit1.Click += new System.EventHandler(this.SwitchStatusChange_Click);
            // 
            // CoolWater
            // 
            this.CoolWater.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CoolWater.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold);
            this.CoolWater.Location = new System.Drawing.Point(274, 502);
            this.CoolWater.Margin = new System.Windows.Forms.Padding(0);
            this.CoolWater.Name = "CoolWater";
            this.CoolWater.Size = new System.Drawing.Size(179, 372);
            this.CoolWater.TabIndex = 48;
            this.CoolWater.Text = "外置冷水机";
            this.CoolWater.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CoolWaterOut2
            // 
            this.CoolWaterOut2.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CoolWaterOut2.Location = new System.Drawing.Point(900, 563);
            this.CoolWaterOut2.Margin = new System.Windows.Forms.Padding(0);
            this.CoolWaterOut2.Name = "CoolWaterOut2";
            this.CoolWaterOut2.PipeLineStyle = HslControls.HslDirectionStyle.Vertical;
            this.CoolWaterOut2.Size = new System.Drawing.Size(15, 253);
            this.CoolWaterOut2.TabIndex = 47;
            // 
            // BypassInPipe1
            // 
            this.BypassInPipe1.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BypassInPipe1.Location = new System.Drawing.Point(1290, 166);
            this.BypassInPipe1.Margin = new System.Windows.Forms.Padding(0);
            this.BypassInPipe1.MoveSpeed = -0.3F;
            this.BypassInPipe1.Name = "BypassInPipe1";
            this.BypassInPipe1.PipeLineStyle = HslControls.HslDirectionStyle.Vertical;
            this.BypassInPipe1.PipeTurnLeft = HslControls.HslPipeTurnDirection.Right;
            this.BypassInPipe1.Size = new System.Drawing.Size(15, 165);
            this.BypassInPipe1.TabIndex = 41;
            // 
            // CoolWaterOut1
            // 
            this.CoolWaterOut1.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CoolWaterOut1.ForeColor = System.Drawing.Color.Black;
            this.CoolWaterOut1.Location = new System.Drawing.Point(453, 816);
            this.CoolWaterOut1.Margin = new System.Windows.Forms.Padding(0);
            this.CoolWaterOut1.MoveSpeed = -0.3F;
            this.CoolWaterOut1.Name = "CoolWaterOut1";
            this.CoolWaterOut1.PipeTurnRight = HslControls.HslPipeTurnDirection.Up;
            this.CoolWaterOut1.Size = new System.Drawing.Size(462, 15);
            this.CoolWaterOut1.TabIndex = 38;
            // 
            // CoolWaterIn
            // 
            this.CoolWaterIn.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CoolWaterIn.ForeColor = System.Drawing.Color.Black;
            this.CoolWaterIn.Location = new System.Drawing.Point(453, 549);
            this.CoolWaterIn.Margin = new System.Windows.Forms.Padding(0);
            this.CoolWaterIn.Name = "CoolWaterIn";
            this.CoolWaterIn.Size = new System.Drawing.Size(217, 15);
            this.CoolWaterIn.TabIndex = 39;
            // 
            // Condenser
            // 
            this.Condenser.BackColor = System.Drawing.Color.Gold;
            this.Condenser.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.Condenser.Location = new System.Drawing.Point(670, 261);
            this.Condenser.Margin = new System.Windows.Forms.Padding(0);
            this.Condenser.Name = "Condenser";
            this.Condenser.Size = new System.Drawing.Size(156, 372);
            this.Condenser.TabIndex = 35;
            this.Condenser.Text = "冷凝器";
            this.Condenser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Capillary
            // 
            this.Capillary.BackColor = System.Drawing.Color.CornflowerBlue;
            this.Capillary.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.Capillary.Location = new System.Drawing.Point(1468, 321);
            this.Capillary.Margin = new System.Windows.Forms.Padding(0);
            this.Capillary.Name = "Capillary";
            this.Capillary.Size = new System.Drawing.Size(73, 32);
            this.Capillary.TabIndex = 36;
            this.Capillary.Text = "毛细管";
            this.Capillary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Compressor
            // 
            this.Compressor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Compressor.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold);
            this.Compressor.Location = new System.Drawing.Point(274, 39);
            this.Compressor.Margin = new System.Windows.Forms.Padding(0);
            this.Compressor.Name = "Compressor";
            this.Compressor.Size = new System.Drawing.Size(179, 372);
            this.Compressor.TabIndex = 37;
            this.Compressor.Text = "压缩机";
            this.Compressor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorOutPipe
            // 
            this.CompressorOutPipe.EdgeColor = System.Drawing.Color.Magenta;
            this.CompressorOutPipe.ForeColor = System.Drawing.Color.Black;
            this.CompressorOutPipe.Location = new System.Drawing.Point(453, 331);
            this.CompressorOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.CompressorOutPipe.Name = "CompressorOutPipe";
            this.CompressorOutPipe.Size = new System.Drawing.Size(217, 15);
            this.CompressorOutPipe.TabIndex = 38;
            // 
            // CompressorInPipe
            // 
            this.CompressorInPipe.EdgeColor = System.Drawing.Color.SkyBlue;
            this.CompressorInPipe.ForeColor = System.Drawing.Color.Black;
            this.CompressorInPipe.Location = new System.Drawing.Point(453, 104);
            this.CompressorInPipe.Margin = new System.Windows.Forms.Padding(0);
            this.CompressorInPipe.MoveSpeed = -0.3F;
            this.CompressorInPipe.Name = "CompressorInPipe";
            this.CompressorInPipe.Size = new System.Drawing.Size(590, 15);
            this.CompressorInPipe.TabIndex = 38;
            // 
            // CoolWaterOut3
            // 
            this.CoolWaterOut3.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CoolWaterOut3.ForeColor = System.Drawing.Color.Black;
            this.CoolWaterOut3.Location = new System.Drawing.Point(826, 549);
            this.CoolWaterOut3.Margin = new System.Windows.Forms.Padding(0);
            this.CoolWaterOut3.Name = "CoolWaterOut3";
            this.CoolWaterOut3.PipeTurnRight = HslControls.HslPipeTurnDirection.Down;
            this.CoolWaterOut3.Size = new System.Drawing.Size(89, 15);
            this.CoolWaterOut3.TabIndex = 39;
            // 
            // HeatExchanger
            // 
            this.HeatExchanger.BackColor = System.Drawing.Color.SkyBlue;
            this.HeatExchanger.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.HeatExchanger.Location = new System.Drawing.Point(1043, 39);
            this.HeatExchanger.Margin = new System.Windows.Forms.Padding(0);
            this.HeatExchanger.Name = "HeatExchanger";
            this.HeatExchanger.Size = new System.Drawing.Size(156, 372);
            this.HeatExchanger.TabIndex = 35;
            this.HeatExchanger.Text = "回热器";
            this.HeatExchanger.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CondenserOutPipe
            // 
            this.CondenserOutPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CondenserOutPipe.ForeColor = System.Drawing.Color.Black;
            this.CondenserOutPipe.Location = new System.Drawing.Point(826, 331);
            this.CondenserOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.CondenserOutPipe.Name = "CondenserOutPipe";
            this.CondenserOutPipe.Size = new System.Drawing.Size(217, 15);
            this.CondenserOutPipe.TabIndex = 38;
            // 
            // Evaporator
            // 
            this.Evaporator.BackColor = System.Drawing.Color.DodgerBlue;
            this.Evaporator.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.Evaporator.Location = new System.Drawing.Point(1736, 46);
            this.Evaporator.Margin = new System.Windows.Forms.Padding(0);
            this.Evaporator.Name = "Evaporator";
            this.Evaporator.Size = new System.Drawing.Size(156, 372);
            this.Evaporator.TabIndex = 35;
            this.Evaporator.Text = "蒸发器";
            this.Evaporator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorOutPipe
            // 
            this.EvaporatorOutPipe.EdgeColor = System.Drawing.Color.SteelBlue;
            this.EvaporatorOutPipe.ForeColor = System.Drawing.Color.Black;
            this.EvaporatorOutPipe.Location = new System.Drawing.Point(1199, 104);
            this.EvaporatorOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.EvaporatorOutPipe.MoveSpeed = -0.3F;
            this.EvaporatorOutPipe.Name = "EvaporatorOutPipe";
            this.EvaporatorOutPipe.Size = new System.Drawing.Size(537, 15);
            this.EvaporatorOutPipe.TabIndex = 38;
            // 
            // HeatExchangerOutPipe
            // 
            this.HeatExchangerOutPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.HeatExchangerOutPipe.ForeColor = System.Drawing.Color.Black;
            this.HeatExchangerOutPipe.Location = new System.Drawing.Point(1199, 329);
            this.HeatExchangerOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.HeatExchangerOutPipe.Name = "HeatExchangerOutPipe";
            this.HeatExchangerOutPipe.Size = new System.Drawing.Size(269, 15);
            this.HeatExchangerOutPipe.TabIndex = 38;
            // 
            // EvaporatorInPipe
            // 
            this.EvaporatorInPipe.EdgeColor = System.Drawing.Color.DodgerBlue;
            this.EvaporatorInPipe.ForeColor = System.Drawing.Color.Black;
            this.EvaporatorInPipe.Location = new System.Drawing.Point(1540, 331);
            this.EvaporatorInPipe.Margin = new System.Windows.Forms.Padding(0);
            this.EvaporatorInPipe.Name = "EvaporatorInPipe";
            this.EvaporatorInPipe.Size = new System.Drawing.Size(196, 15);
            this.EvaporatorInPipe.TabIndex = 38;
            // 
            // CondenserTempOut
            // 
            this.CondenserTempOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserTempOut.ForeColor = System.Drawing.Color.Blue;
            this.CondenserTempOut.Location = new System.Drawing.Point(828, 305);
            this.CondenserTempOut.Name = "CondenserTempOut";
            this.CondenserTempOut.Size = new System.Drawing.Size(92, 16);
            this.CondenserTempOut.TabIndex = 51;
            this.CondenserTempOut.Text = "0℃";
            this.CondenserTempOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorTempOut
            // 
            this.CompressorTempOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorTempOut.ForeColor = System.Drawing.Color.Blue;
            this.CompressorTempOut.Location = new System.Drawing.Point(455, 305);
            this.CompressorTempOut.Name = "CompressorTempOut";
            this.CompressorTempOut.Size = new System.Drawing.Size(92, 16);
            this.CompressorTempOut.TabIndex = 51;
            this.CompressorTempOut.Text = "0℃";
            this.CompressorTempOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorTempIn
            // 
            this.EvaporatorTempIn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorTempIn.ForeColor = System.Drawing.Color.Blue;
            this.EvaporatorTempIn.Location = new System.Drawing.Point(1641, 76);
            this.EvaporatorTempIn.Name = "EvaporatorTempIn";
            this.EvaporatorTempIn.Size = new System.Drawing.Size(92, 16);
            this.EvaporatorTempIn.TabIndex = 51;
            this.EvaporatorTempIn.Text = "0℃";
            this.EvaporatorTempIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorPressureOut
            // 
            this.CompressorPressureOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorPressureOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CompressorPressureOut.Location = new System.Drawing.Point(455, 356);
            this.CompressorPressureOut.Name = "CompressorPressureOut";
            this.CompressorPressureOut.Size = new System.Drawing.Size(92, 16);
            this.CompressorPressureOut.TabIndex = 51;
            this.CompressorPressureOut.Text = "0Mpa";
            this.CompressorPressureOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CondenserPressureOut
            // 
            this.CondenserPressureOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserPressureOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CondenserPressureOut.Location = new System.Drawing.Point(828, 356);
            this.CondenserPressureOut.Name = "CondenserPressureOut";
            this.CondenserPressureOut.Size = new System.Drawing.Size(92, 16);
            this.CondenserPressureOut.TabIndex = 51;
            this.CondenserPressureOut.Text = "0Mpa";
            this.CondenserPressureOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HeatExchangerPressureOut
            // 
            this.HeatExchangerPressureOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HeatExchangerPressureOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.HeatExchangerPressureOut.Location = new System.Drawing.Point(1201, 356);
            this.HeatExchangerPressureOut.Name = "HeatExchangerPressureOut";
            this.HeatExchangerPressureOut.Size = new System.Drawing.Size(92, 16);
            this.HeatExchangerPressureOut.TabIndex = 51;
            this.HeatExchangerPressureOut.Text = "0Mpa";
            this.HeatExchangerPressureOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorPressureIn
            // 
            this.EvaporatorPressureIn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorPressureIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.EvaporatorPressureIn.Location = new System.Drawing.Point(1641, 356);
            this.EvaporatorPressureIn.Name = "EvaporatorPressureIn";
            this.EvaporatorPressureIn.Size = new System.Drawing.Size(92, 16);
            this.EvaporatorPressureIn.TabIndex = 51;
            this.EvaporatorPressureIn.Text = "0Mpa";
            this.EvaporatorPressureIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EvaporatorPressureOut
            // 
            this.EvaporatorPressureOut.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EvaporatorPressureOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.EvaporatorPressureOut.Location = new System.Drawing.Point(1641, 131);
            this.EvaporatorPressureOut.Name = "EvaporatorPressureOut";
            this.EvaporatorPressureOut.Size = new System.Drawing.Size(92, 16);
            this.EvaporatorPressureOut.TabIndex = 51;
            this.EvaporatorPressureOut.Text = "0Mpa";
            this.EvaporatorPressureOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CompressorPressureIn
            // 
            this.CompressorPressureIn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CompressorPressureIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CompressorPressureIn.Location = new System.Drawing.Point(455, 131);
            this.CompressorPressureIn.Name = "CompressorPressureIn";
            this.CompressorPressureIn.Size = new System.Drawing.Size(76, 16);
            this.CompressorPressureIn.TabIndex = 51;
            this.CompressorPressureIn.Text = "0Mpa";
            this.CompressorPressureIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CondenserPressureIn
            // 
            this.CondenserPressureIn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CondenserPressureIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CondenserPressureIn.Location = new System.Drawing.Point(576, 356);
            this.CondenserPressureIn.Name = "CondenserPressureIn";
            this.CondenserPressureIn.Size = new System.Drawing.Size(92, 16);
            this.CondenserPressureIn.TabIndex = 51;
            this.CondenserPressureIn.Text = "0Mpa";
            this.CondenserPressureIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BypassValve
            // 
            this.BypassValve.EdgeColor = System.Drawing.Color.Red;
            this.BypassValve.Location = new System.Drawing.Point(1354, 149);
            this.BypassValve.Margin = new System.Windows.Forms.Padding(0);
            this.BypassValve.Name = "BypassValve";
            this.BypassValve.Size = new System.Drawing.Size(64, 37);
            this.BypassValve.TabIndex = 50;
            // 
            // BypassInPipe2
            // 
            this.BypassInPipe2.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BypassInPipe2.ForeColor = System.Drawing.Color.Black;
            this.BypassInPipe2.Location = new System.Drawing.Point(1304, 166);
            this.BypassInPipe2.Margin = new System.Windows.Forms.Padding(0);
            this.BypassInPipe2.Name = "BypassInPipe2";
            this.BypassInPipe2.Size = new System.Drawing.Size(50, 15);
            this.BypassInPipe2.TabIndex = 38;
            // 
            // BypassCapillaryOut2
            // 
            this.BypassCapillaryOut2.EdgeColor = System.Drawing.Color.DodgerBlue;
            this.BypassCapillaryOut2.Location = new System.Drawing.Point(1623, 119);
            this.BypassCapillaryOut2.Margin = new System.Windows.Forms.Padding(0);
            this.BypassCapillaryOut2.MoveSpeed = -0.3F;
            this.BypassCapillaryOut2.Name = "BypassCapillaryOut2";
            this.BypassCapillaryOut2.PipeLineStyle = HslControls.HslDirectionStyle.Vertical;
            this.BypassCapillaryOut2.PipeTurnRight = HslControls.HslPipeTurnDirection.Left;
            this.BypassCapillaryOut2.Size = new System.Drawing.Size(15, 62);
            this.BypassCapillaryOut2.TabIndex = 49;
            // 
            // BypassCapillaryOut1
            // 
            this.BypassCapillaryOut1.EdgeColor = System.Drawing.Color.DodgerBlue;
            this.BypassCapillaryOut1.ForeColor = System.Drawing.Color.Black;
            this.BypassCapillaryOut1.Location = new System.Drawing.Point(1540, 166);
            this.BypassCapillaryOut1.Margin = new System.Windows.Forms.Padding(0);
            this.BypassCapillaryOut1.Name = "BypassCapillaryOut1";
            this.BypassCapillaryOut1.Size = new System.Drawing.Size(83, 15);
            this.BypassCapillaryOut1.TabIndex = 38;
            // 
            // BypassCapillary
            // 
            this.BypassCapillary.BackColor = System.Drawing.Color.CornflowerBlue;
            this.BypassCapillary.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.BypassCapillary.Location = new System.Drawing.Point(1468, 157);
            this.BypassCapillary.Margin = new System.Windows.Forms.Padding(0);
            this.BypassCapillary.Name = "BypassCapillary";
            this.BypassCapillary.Size = new System.Drawing.Size(73, 32);
            this.BypassCapillary.TabIndex = 36;
            this.BypassCapillary.Text = "毛细管";
            this.BypassCapillary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BypassOutPipe
            // 
            this.BypassOutPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BypassOutPipe.ForeColor = System.Drawing.Color.Black;
            this.BypassOutPipe.Location = new System.Drawing.Point(1418, 166);
            this.BypassOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.BypassOutPipe.Name = "BypassOutPipe";
            this.BypassOutPipe.Size = new System.Drawing.Size(50, 15);
            this.BypassOutPipe.TabIndex = 38;
            // 
            // ParallelCapillary
            // 
            this.ParallelCapillary.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ParallelCapillary.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.ParallelCapillary.Location = new System.Drawing.Point(1468, 239);
            this.ParallelCapillary.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelCapillary.Name = "ParallelCapillary";
            this.ParallelCapillary.Size = new System.Drawing.Size(73, 32);
            this.ParallelCapillary.TabIndex = 36;
            this.ParallelCapillary.Text = "毛细管";
            this.ParallelCapillary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParallelCapillaryOutPipe1
            // 
            this.ParallelCapillaryOutPipe1.EdgeColor = System.Drawing.Color.DodgerBlue;
            this.ParallelCapillaryOutPipe1.ForeColor = System.Drawing.Color.Black;
            this.ParallelCapillaryOutPipe1.Location = new System.Drawing.Point(1541, 248);
            this.ParallelCapillaryOutPipe1.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelCapillaryOutPipe1.Name = "ParallelCapillaryOutPipe1";
            this.ParallelCapillaryOutPipe1.Size = new System.Drawing.Size(83, 15);
            this.ParallelCapillaryOutPipe1.TabIndex = 38;
            // 
            // ParallelOutPipe
            // 
            this.ParallelOutPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ParallelOutPipe.ForeColor = System.Drawing.Color.Black;
            this.ParallelOutPipe.Location = new System.Drawing.Point(1418, 248);
            this.ParallelOutPipe.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelOutPipe.Name = "ParallelOutPipe";
            this.ParallelOutPipe.Size = new System.Drawing.Size(50, 15);
            this.ParallelOutPipe.TabIndex = 38;
            // 
            // ParallelValve
            // 
            this.ParallelValve.EdgeColor = System.Drawing.Color.Red;
            this.ParallelValve.Location = new System.Drawing.Point(1354, 231);
            this.ParallelValve.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelValve.Name = "ParallelValve";
            this.ParallelValve.Size = new System.Drawing.Size(64, 37);
            this.ParallelValve.TabIndex = 50;
            // 
            // ParallelInPipe
            // 
            this.ParallelInPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ParallelInPipe.ForeColor = System.Drawing.Color.Black;
            this.ParallelInPipe.Location = new System.Drawing.Point(1304, 248);
            this.ParallelInPipe.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelInPipe.Name = "ParallelInPipe";
            this.ParallelInPipe.Size = new System.Drawing.Size(50, 15);
            this.ParallelInPipe.TabIndex = 53;
            // 
            // ParallelCapillaryOutPipe2
            // 
            this.ParallelCapillaryOutPipe2.EdgeColor = System.Drawing.Color.DodgerBlue;
            this.ParallelCapillaryOutPipe2.Location = new System.Drawing.Point(1623, 248);
            this.ParallelCapillaryOutPipe2.Margin = new System.Windows.Forms.Padding(0);
            this.ParallelCapillaryOutPipe2.Name = "ParallelCapillaryOutPipe2";
            this.ParallelCapillaryOutPipe2.PipeLineStyle = HslControls.HslDirectionStyle.Vertical;
            this.ParallelCapillaryOutPipe2.PipeTurnLeft = HslControls.HslPipeTurnDirection.Left;
            this.ParallelCapillaryOutPipe2.Size = new System.Drawing.Size(15, 83);
            this.ParallelCapillaryOutPipe2.TabIndex = 49;
            // 
            // UnitChart
            // 
            this.UnitChart.BackColor = System.Drawing.Color.Transparent;
            this.UnitChart.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.Interval = 80D;
            chartArea1.AxisX.LabelAutoFitMinFontSize = 5;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)((((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep45) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea1.AxisX.Maximum = 900D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX2.Maximum = 900D;
            chartArea1.AxisX2.Minimum = 0D;
            chartArea1.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Blue;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.Maximum = 150D;
            chartArea1.AxisY.Minimum = -70D;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.Red;
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.Maximum = 3D;
            chartArea1.AxisY2.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.IsSameFontSizeForAllAxes = true;
            chartArea1.Name = "TempPressure";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 87F;
            chartArea1.Position.Width = 100F;
            chartArea1.Position.Y = 13F;
            chartArea1.ShadowColor = System.Drawing.Color.Silver;
            this.UnitChart.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.AutoFitMinFontSize = 5;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.BackImageTransparentColor = System.Drawing.Color.Transparent;
            legend1.BackSecondaryColor = System.Drawing.Color.Transparent;
            legend1.BorderColor = System.Drawing.Color.Transparent;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("宋体", 10F);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.UnitChart.Legends.Add(legend1);
            this.UnitChart.Location = new System.Drawing.Point(918, 414);
            this.UnitChart.Name = "UnitChart";
            series1.ChartArea = "TempPressure";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Red;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series1.Legend = "Legend1";
            series1.LegendText = "压缩机入口压力";
            series1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series1.Name = "CompressorPressureIn";
            series1.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.ChartArea = "TempPressure";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Orange;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series2.Legend = "Legend1";
            series2.LegendText = "压缩机出口压力";
            series2.MarkerColor = System.Drawing.Color.Blue;
            series2.Name = "CompressorPressureOut";
            series2.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series3.ChartArea = "TempPressure";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Color = System.Drawing.Color.Gold;
            series3.Legend = "Legend1";
            series3.LegendText = "冷凝器入口压力";
            series3.Name = "CondenserPressureIn";
            series3.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series4.ChartArea = "TempPressure";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Color = System.Drawing.Color.Green;
            series4.Legend = "Legend1";
            series4.LegendText = "冷凝器出口压力";
            series4.Name = "CondenserPressureOut";
            series4.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series5.ChartArea = "TempPressure";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Color = System.Drawing.Color.Cyan;
            series5.Legend = "Legend1";
            series5.LegendText = "蒸发器入口压力";
            series5.Name = "EvaporatorPressureIn";
            series5.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series6.ChartArea = "TempPressure";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Blue;
            series6.Legend = "Legend1";
            series6.LegendText = "蒸发器出口压力";
            series6.Name = "EvaporatorPressureOut";
            series6.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series7.ChartArea = "TempPressure";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series7.Color = System.Drawing.Color.Purple;
            series7.Legend = "Legend1";
            series7.LegendText = "换热器出口压力";
            series7.Name = "HeatExchangerPressureOut";
            series7.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series8.ChartArea = "TempPressure";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series8.Color = System.Drawing.Color.Pink;
            series8.Legend = "Legend1";
            series8.LegendText = "压缩机出口温度";
            series8.Name = "CompressorTempOut";
            series9.ChartArea = "TempPressure";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series9.Color = System.Drawing.Color.Brown;
            series9.Legend = "Legend1";
            series9.LegendText = "冷凝器出口温度";
            series9.Name = "CondenserTempOut";
            series10.ChartArea = "TempPressure";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series10.Color = System.Drawing.Color.Fuchsia;
            series10.Legend = "Legend1";
            series10.LegendText = "蒸发器出口温度";
            series10.Name = "EvaporatorTempOut";
            this.UnitChart.Series.Add(series1);
            this.UnitChart.Series.Add(series2);
            this.UnitChart.Series.Add(series3);
            this.UnitChart.Series.Add(series4);
            this.UnitChart.Series.Add(series5);
            this.UnitChart.Series.Add(series6);
            this.UnitChart.Series.Add(series7);
            this.UnitChart.Series.Add(series8);
            this.UnitChart.Series.Add(series9);
            this.UnitChart.Series.Add(series10);
            this.UnitChart.Size = new System.Drawing.Size(990, 474);
            this.UnitChart.TabIndex = 54;
            // 
            // BranchPipe
            // 
            this.BranchPipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BranchPipe.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BranchPipe.Location = new System.Drawing.Point(1290, 249);
            this.BranchPipe.Margin = new System.Windows.Forms.Padding(0);
            this.BranchPipe.MoveSpeed = -0.3F;
            this.BranchPipe.Name = "BranchPipe";
            this.BranchPipe.PipeLineStyle = HslControls.HslDirectionStyle.Vertical;
            this.BranchPipe.PipeTurnLeft = HslControls.HslPipeTurnDirection.Right;
            this.BranchPipe.Size = new System.Drawing.Size(15, 82);
            this.BranchPipe.TabIndex = 41;
            // 
            // UnitWorkState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 900);
            this.ControlBox = false;
            this.Controls.Add(this.BranchPipe);
            this.Controls.Add(this.EvaporatorTempIn);
            this.Controls.Add(this.EvaporatorPressureIn);
            this.Controls.Add(this.CompressorPressureIn);
            this.Controls.Add(this.EvaporatorPressureOut);
            this.Controls.Add(this.HeatExchangerPressureOut);
            this.Controls.Add(this.CondenserPressureIn);
            this.Controls.Add(this.CondenserPressureOut);
            this.Controls.Add(this.CompressorPressureOut);
            this.Controls.Add(this.CompressorTempOut);
            this.Controls.Add(this.CondenserTempOut);
            this.Controls.Add(this.ParallelValve);
            this.Controls.Add(this.BypassValve);
            this.Controls.Add(this.CoolWater);
            this.Controls.Add(this.ParallelOutPipe);
            this.Controls.Add(this.BypassOutPipe);
            this.Controls.Add(this.ParallelCapillaryOutPipe1);
            this.Controls.Add(this.BypassCapillaryOut1);
            this.Controls.Add(this.EvaporatorInPipe);
            this.Controls.Add(this.HeatExchangerOutPipe);
            this.Controls.Add(this.EvaporatorOutPipe);
            this.Controls.Add(this.CompressorInPipe);
            this.Controls.Add(this.CondenserOutPipe);
            this.Controls.Add(this.CompressorOutPipe);
            this.Controls.Add(this.CoolWaterOut1);
            this.Controls.Add(this.CoolWaterOut3);
            this.Controls.Add(this.CoolWaterIn);
            this.Controls.Add(this.Evaporator);
            this.Controls.Add(this.HeatExchanger);
            this.Controls.Add(this.Condenser);
            this.Controls.Add(this.ParallelCapillary);
            this.Controls.Add(this.BypassCapillary);
            this.Controls.Add(this.Capillary);
            this.Controls.Add(this.Compressor);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.BypassInPipe1);
            this.Controls.Add(this.ParallelInPipe);
            this.Controls.Add(this.BypassInPipe2);
            this.Controls.Add(this.ParallelCapillaryOutPipe2);
            this.Controls.Add(this.BypassCapillaryOut2);
            this.Controls.Add(this.UnitChart);
            this.Controls.Add(this.CoolWaterOut2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 90);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnitWorkState";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.UnitWorkState_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnitChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private HslControls.HslSwitch Unit3;
        private HslControls.HslSwitch Unit2;
        private HslControls.HslSwitch Unit4;
        private HslControls.HslSwitch Unit1;
        private System.Windows.Forms.Label CoolWater;
        private HslControls.HslPipeLine CoolWaterOut2;
        private HslControls.HslPipeLine BypassInPipe1;
        private HslControls.HslPipeLine CoolWaterOut1;
        private HslControls.HslPipeLine CoolWaterIn;
        private System.Windows.Forms.Label Condenser;
        private System.Windows.Forms.Label Capillary;
        private System.Windows.Forms.Label Compressor;
        private HslControls.HslPipeLine CompressorOutPipe;
        private HslControls.HslPipeLine CompressorInPipe;
        private HslControls.HslPipeLine CoolWaterOut3;
        private System.Windows.Forms.Label HeatExchanger;
        private HslControls.HslPipeLine CondenserOutPipe;
        private System.Windows.Forms.Label Evaporator;
        private HslControls.HslPipeLine EvaporatorOutPipe;
        private HslControls.HslPipeLine HeatExchangerOutPipe;
        private HslControls.HslPipeLine EvaporatorInPipe;
        private System.Windows.Forms.Label CondenserTempOut;
        private System.Windows.Forms.Label CompressorTempOut;
        private System.Windows.Forms.Label EvaporatorTempIn;
        private System.Windows.Forms.Label CompressorPressureOut;
        private System.Windows.Forms.Label CondenserPressureOut;
        private System.Windows.Forms.Label HeatExchangerPressureOut;
        private System.Windows.Forms.Label EvaporatorPressureIn;
        private System.Windows.Forms.Label EvaporatorPressureOut;
        private System.Windows.Forms.Label CompressorPressureIn;
        private System.Windows.Forms.Label CondenserPressureIn;
        private HslControls.HslValves BypassValve;
        private HslControls.HslPipeLine BypassInPipe2;
        private HslControls.HslPipeLine BypassCapillaryOut2;
        private HslControls.HslPipeLine BypassCapillaryOut1;
        private System.Windows.Forms.Label BypassCapillary;
        private HslControls.HslPipeLine BypassOutPipe;
        private System.Windows.Forms.Label ParallelCapillary;
        private HslControls.HslPipeLine ParallelCapillaryOutPipe1;
        private HslControls.HslPipeLine ParallelOutPipe;
        private HslControls.HslValves ParallelValve;
        private HslControls.HslPipeLine ParallelInPipe;
        private HslControls.HslPipeLine ParallelCapillaryOutPipe2;
        private System.Windows.Forms.DataVisualization.Charting.Chart UnitChart;
        private HslControls.HslButton OpenChartSet;
        private HslControls.HslPipeLine BranchPipe;
    }
}