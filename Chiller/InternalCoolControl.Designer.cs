namespace Chiller
{
    partial class InternalColdControl
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
            this.WaterTemp = new HslControls.HslThermometer();
            this.WaterHeight = new HslControls.HslBottle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ColdingEnabled = new Switch.ZDSwitch();
            this.OpenChartSet = new HslControls.HslButton();
            this.OpenTempSet = new HslControls.HslButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Unit3 = new HslControls.HslSwitch();
            this.Unit2 = new HslControls.HslSwitch();
            this.Unit4 = new HslControls.HslSwitch();
            this.Unit1 = new HslControls.HslSwitch();
            this.DisplayLiquid = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LevelTempChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.DisplayLiquid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelTempChart)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // WaterTemp
            // 
            this.WaterTemp.BackColor = System.Drawing.Color.White;
            this.WaterTemp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.WaterTemp.Font = new System.Drawing.Font("宋体", 12F);
            this.WaterTemp.IsRenderText = true;
            this.WaterTemp.Location = new System.Drawing.Point(236, 85);
            this.WaterTemp.Margin = new System.Windows.Forms.Padding(32, 3, 32, 3);
            this.WaterTemp.Name = "WaterTemp";
            this.WaterTemp.SegmentCount = 10;
            this.WaterTemp.Size = new System.Drawing.Size(169, 785);
            this.WaterTemp.TabIndex = 32;
            this.WaterTemp.TemperatureBackColor = System.Drawing.Color.LightGray;
            this.WaterTemp.TemperatureColor = System.Drawing.Color.DodgerBlue;
            this.WaterTemp.Value = 3F;
            this.WaterTemp.ValueMax = 40F;
            this.WaterTemp.ValueStart = -100F;
            // 
            // WaterHeight
            // 
            this.WaterHeight.BackColor = System.Drawing.Color.White;
            this.WaterHeight.BackColorCenter = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.WaterHeight.BackColorEdge = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(196)))), ((int)(((byte)(216)))));
            this.WaterHeight.BackColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(232)))), ((int)(((byte)(244)))));
            this.WaterHeight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.WaterHeight.BottleTag = "50%";
            this.WaterHeight.DockHeight = 0F;
            this.WaterHeight.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold);
            this.WaterHeight.ForeColorCenter = System.Drawing.Color.SkyBlue;
            this.WaterHeight.ForeColorEdge = System.Drawing.Color.CornflowerBlue;
            this.WaterHeight.ForeColorTop = System.Drawing.Color.DodgerBlue;
            this.WaterHeight.HeadTag = "";
            this.WaterHeight.Location = new System.Drawing.Point(30, 85);
            this.WaterHeight.Margin = new System.Windows.Forms.Padding(32, 3, 32, 3);
            this.WaterHeight.Name = "WaterHeight";
            this.WaterHeight.Size = new System.Drawing.Size(169, 785);
            this.WaterHeight.TabIndex = 31;
            this.WaterHeight.TabStop = false;
            this.WaterHeight.Value = 50D;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SkyBlue;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.ColdingEnabled);
            this.panel2.Controls.Add(this.OpenChartSet);
            this.panel2.Controls.Add(this.OpenTempSet);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 900);
            this.panel2.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(38, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 27);
            this.label3.TabIndex = 43;
            this.label3.Text = "制冷使能";
            // 
            // ColdingEnabled
            // 
            this.ColdingEnabled.Checked = false;
            this.ColdingEnabled.FalseColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.ColdingEnabled.Location = new System.Drawing.Point(20, 55);
            this.ColdingEnabled.Name = "ColdingEnabled";
            this.ColdingEnabled.Size = new System.Drawing.Size(160, 73);
            this.ColdingEnabled.SwitchType = Switch.SwitchType.Ellipse;
            this.ColdingEnabled.TabIndex = 42;
            this.ColdingEnabled.Texts = new string[0];
            this.ColdingEnabled.TrueColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ColdingEnabled.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColdingEnabled_MouseDown);
            // 
            // OpenChartSet
            // 
            this.OpenChartSet.CornerRadius = 10;
            this.OpenChartSet.CustomerInformation = null;
            this.OpenChartSet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenChartSet.Location = new System.Drawing.Point(19, 261);
            this.OpenChartSet.Name = "OpenChartSet";
            this.OpenChartSet.Size = new System.Drawing.Size(160, 55);
            this.OpenChartSet.TabIndex = 40;
            this.OpenChartSet.Text = "曲线设定";
            this.OpenChartSet.Click += new System.EventHandler(this.OpenChartSet_Click);
            // 
            // OpenTempSet
            // 
            this.OpenTempSet.CornerRadius = 10;
            this.OpenTempSet.CustomerInformation = null;
            this.OpenTempSet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenTempSet.Location = new System.Drawing.Point(19, 200);
            this.OpenTempSet.Name = "OpenTempSet";
            this.OpenTempSet.Size = new System.Drawing.Size(160, 55);
            this.OpenTempSet.TabIndex = 41;
            this.OpenTempSet.Text = "温度设定";
            this.OpenTempSet.Click += new System.EventHandler(this.OpenTempSet_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Unit3);
            this.groupBox3.Controls.Add(this.Unit2);
            this.groupBox3.Controls.Add(this.Unit4);
            this.groupBox3.Controls.Add(this.Unit1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(3, 347);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(194, 550);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通道选择";
            // 
            // Unit3
            // 
            this.Unit3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Unit3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Unit3.ForeColor = System.Drawing.Color.Blue;
            this.Unit3.Location = new System.Drawing.Point(47, 294);
            this.Unit3.Name = "Unit3";
            this.Unit3.Size = new System.Drawing.Size(105, 120);
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
            this.Unit2.Location = new System.Drawing.Point(47, 167);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(105, 120);
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
            this.Unit4.Location = new System.Drawing.Point(47, 421);
            this.Unit4.Name = "Unit4";
            this.Unit4.Size = new System.Drawing.Size(105, 120);
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
            this.Unit1.Location = new System.Drawing.Point(47, 40);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(105, 120);
            this.Unit1.SwitchForeground = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Unit1.TabIndex = 35;
            this.Unit1.Text = "Unit1";
            this.Unit1.Click += new System.EventHandler(this.SwitchStatusChange_Click);
            // 
            // DisplayLiquid
            // 
            this.DisplayLiquid.Controls.Add(this.label2);
            this.DisplayLiquid.Controls.Add(this.label1);
            this.DisplayLiquid.Controls.Add(this.WaterHeight);
            this.DisplayLiquid.Controls.Add(this.WaterTemp);
            this.DisplayLiquid.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.DisplayLiquid.Location = new System.Drawing.Point(206, 12);
            this.DisplayLiquid.Name = "DisplayLiquid";
            this.DisplayLiquid.Size = new System.Drawing.Size(435, 876);
            this.DisplayLiquid.TabIndex = 34;
            this.DisplayLiquid.TabStop = false;
            this.DisplayLiquid.Text = "Liquid";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(236, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 20);
            this.label2.TabIndex = 33;
            this.label2.Text = "Temperature";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(30, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 33;
            this.label1.Text = "Level";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LevelTempChart
            // 
            this.LevelTempChart.BorderlineColor = System.Drawing.Color.Silver;
            this.LevelTempChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.AlignWithChartArea = "Temp";
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
            chartArea1.AxisY.Interval = 20D;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 40D;
            chartArea1.AxisY.Minimum = -100D;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.Interval = 20D;
            chartArea1.AxisY2.Maximum = 40D;
            chartArea1.AxisY2.Minimum = -100D;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 93F;
            chartArea1.InnerPlotPosition.Width = 96F;
            chartArea1.InnerPlotPosition.X = 3F;
            chartArea1.InnerPlotPosition.Y = 2F;
            chartArea1.Name = "Temp";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.LevelTempChart.ChartAreas.Add(chartArea1);
            this.LevelTempChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.AutoFitMinFontSize = 5;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.BackImageTransparentColor = System.Drawing.Color.White;
            legend1.BackSecondaryColor = System.Drawing.Color.White;
            legend1.BorderColor = System.Drawing.Color.Transparent;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            legend1.TitleBackColor = System.Drawing.Color.Transparent;
            this.LevelTempChart.Legends.Add(legend1);
            this.LevelTempChart.Location = new System.Drawing.Point(3, 26);
            this.LevelTempChart.Name = "LevelTempChart";
            series1.ChartArea = "Temp";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Olive;
            series1.Legend = "Legend1";
            series1.LegendText = "Unit1 Temp";
            series1.Name = "Unit1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "Temp";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Green;
            series2.Legend = "Legend1";
            series2.LegendText = "Unit2 Temp";
            series2.Name = "Unit2";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.ChartArea = "Temp";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Color = System.Drawing.Color.Turquoise;
            series3.Legend = "Legend1";
            series3.LegendText = "Unit3 Temp";
            series3.Name = "Unit3";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series4.ChartArea = "Temp";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Color = System.Drawing.Color.Blue;
            series4.Legend = "Legend1";
            series4.LegendText = "Unit4 Temp";
            series4.Name = "Unit4";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series5.ChartArea = "Temp";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Color = System.Drawing.Color.DarkViolet;
            series5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series5.Legend = "Legend1";
            series5.LegendText = "设定温度";
            series5.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series5.Name = "SetTemp";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series6.ChartArea = "Temp";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Red;
            series6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series6.Legend = "Legend1";
            series6.LegendText = "载冷液温度";
            series6.MarkerColor = System.Drawing.Color.Blue;
            series6.Name = "NowTemp";
            series6.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.LevelTempChart.Series.Add(series1);
            this.LevelTempChart.Series.Add(series2);
            this.LevelTempChart.Series.Add(series3);
            this.LevelTempChart.Series.Add(series4);
            this.LevelTempChart.Series.Add(series5);
            this.LevelTempChart.Series.Add(series6);
            this.LevelTempChart.Size = new System.Drawing.Size(1255, 847);
            this.LevelTempChart.TabIndex = 36;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LevelTempChart);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(647, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1261, 876);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temperature Chart";
            // 
            // InternalColdControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1920, 900);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.DisplayLiquid);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 90);
            this.Name = "InternalColdControl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.InternalColdControl_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.DisplayLiquid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LevelTempChart)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HslControls.HslThermometer WaterTemp;
        private HslControls.HslBottle WaterHeight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox DisplayLiquid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart LevelTempChart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private HslControls.HslSwitch Unit3;
        private HslControls.HslSwitch Unit2;
        private HslControls.HslSwitch Unit4;
        private HslControls.HslSwitch Unit1;
        private HslControls.HslButton OpenChartSet;
        private HslControls.HslButton OpenTempSet;
        private System.Windows.Forms.Label label1;
        private Switch.ZDSwitch ColdingEnabled;
        private System.Windows.Forms.Label label3;
    }
}