namespace Chiller
{
    partial class Home
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.DisplayTime = new System.Windows.Forms.Label();
            this.DisplayDate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FreeTimeDisplay = new System.Windows.Forms.Label();
            this.UserGroup = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FunctionBase = new System.Windows.Forms.Panel();
            this.ExitSystem = new HslControls.HslButton();
            this.OpenAlmDisplay = new HslControls.HslButton();
            this.OpenDiagnosis = new HslControls.HslButton();
            this.Manufacturer = new HslControls.HslButton();
            this.OpenInternalColdControl = new HslControls.HslButton();
            this.OpenExternalTempControl = new HslControls.HslButton();
            this.OpenUnitWorkState = new HslControls.HslButton();
            this.OpenInternalColdState = new HslControls.HslButton();
            this.OpenExternalTempState = new HslControls.HslButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.FunctionBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // DisplayTime
            // 
            this.DisplayTime.AutoSize = true;
            this.DisplayTime.BackColor = System.Drawing.Color.Transparent;
            this.DisplayTime.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.DisplayTime.ForeColor = System.Drawing.Color.Navy;
            this.DisplayTime.Location = new System.Drawing.Point(1755, 12);
            this.DisplayTime.Name = "DisplayTime";
            this.DisplayTime.Size = new System.Drawing.Size(114, 24);
            this.DisplayTime.TabIndex = 26;
            this.DisplayTime.Text = "12:00:00";
            this.DisplayTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DisplayDate
            // 
            this.DisplayDate.AutoSize = true;
            this.DisplayDate.BackColor = System.Drawing.Color.Transparent;
            this.DisplayDate.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.DisplayDate.ForeColor = System.Drawing.Color.Navy;
            this.DisplayDate.Location = new System.Drawing.Point(1609, 12);
            this.DisplayDate.Name = "DisplayDate";
            this.DisplayDate.Size = new System.Drawing.Size(140, 24);
            this.DisplayDate.TabIndex = 26;
            this.DisplayDate.Text = "2018-06-27";
            this.DisplayDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SkyBlue;
            this.panel1.Controls.Add(this.FreeTimeDisplay);
            this.panel1.Controls.Add(this.UserGroup);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.DisplayTime);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DisplayDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 85);
            this.panel1.TabIndex = 0;
            // 
            // FreeTimeDisplay
            // 
            this.FreeTimeDisplay.ForeColor = System.Drawing.Color.Black;
            this.FreeTimeDisplay.Location = new System.Drawing.Point(1875, 46);
            this.FreeTimeDisplay.Name = "FreeTimeDisplay";
            this.FreeTimeDisplay.Size = new System.Drawing.Size(36, 23);
            this.FreeTimeDisplay.TabIndex = 30;
            this.FreeTimeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserGroup
            // 
            this.UserGroup.BackColor = System.Drawing.SystemColors.Window;
            this.UserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UserGroup.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserGroup.FormattingEnabled = true;
            this.UserGroup.Items.AddRange(new object[] {
            "None",
            "Engineer",
            "Administrator"});
            this.UserGroup.Location = new System.Drawing.Point(1613, 43);
            this.UserGroup.Name = "UserGroup";
            this.UserGroup.Size = new System.Drawing.Size(256, 28);
            this.UserGroup.TabIndex = 29;
            this.UserGroup.SelectionChangeCommitted += new System.EventHandler(this.UserGroup_SelectionChangeCommitted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Chiller.Properties.Resources.logo;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(5, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 77);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(124, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(773, 40);
            this.label1.TabIndex = 26;
            this.label1.Text = "Chiller Refrigeration Control System";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(1470, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 24);
            this.label4.TabIndex = 26;
            this.label4.Text = "登陆用户：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(1470, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 24);
            this.label2.TabIndex = 26;
            this.label2.Text = "系统时间：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FunctionBase
            // 
            this.FunctionBase.BackColor = System.Drawing.Color.SkyBlue;
            this.FunctionBase.Controls.Add(this.ExitSystem);
            this.FunctionBase.Controls.Add(this.OpenAlmDisplay);
            this.FunctionBase.Controls.Add(this.OpenDiagnosis);
            this.FunctionBase.Controls.Add(this.Manufacturer);
            this.FunctionBase.Controls.Add(this.OpenInternalColdControl);
            this.FunctionBase.Controls.Add(this.OpenExternalTempControl);
            this.FunctionBase.Controls.Add(this.OpenUnitWorkState);
            this.FunctionBase.Controls.Add(this.OpenInternalColdState);
            this.FunctionBase.Controls.Add(this.OpenExternalTempState);
            this.FunctionBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FunctionBase.Location = new System.Drawing.Point(0, 995);
            this.FunctionBase.Name = "FunctionBase";
            this.FunctionBase.Size = new System.Drawing.Size(1920, 85);
            this.FunctionBase.TabIndex = 32;
            // 
            // ExitSystem
            // 
            this.ExitSystem.ActiveColor = System.Drawing.Color.Magenta;
            this.ExitSystem.CornerRadius = 10;
            this.ExitSystem.CustomerInformation = null;
            this.ExitSystem.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExitSystem.ForeColor = System.Drawing.SystemColors.Control;
            this.ExitSystem.Location = new System.Drawing.Point(1730, 15);
            this.ExitSystem.Margin = new System.Windows.Forms.Padding(15);
            this.ExitSystem.Name = "ExitSystem";
            this.ExitSystem.OriginalColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ExitSystem.Size = new System.Drawing.Size(160, 55);
            this.ExitSystem.TabIndex = 33;
            this.ExitSystem.Text = "关闭系统";
            this.ExitSystem.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenAlmDisplay
            // 
            this.OpenAlmDisplay.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenAlmDisplay.CornerRadius = 10;
            this.OpenAlmDisplay.CustomerInformation = null;
            this.OpenAlmDisplay.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenAlmDisplay.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenAlmDisplay.Location = new System.Drawing.Point(24, 15);
            this.OpenAlmDisplay.Margin = new System.Windows.Forms.Padding(15);
            this.OpenAlmDisplay.Name = "OpenAlmDisplay";
            this.OpenAlmDisplay.OriginalColor = System.Drawing.Color.Gray;
            this.OpenAlmDisplay.Size = new System.Drawing.Size(160, 55);
            this.OpenAlmDisplay.TabIndex = 34;
            this.OpenAlmDisplay.Text = "设备报警信息";
            this.OpenAlmDisplay.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenDiagnosis
            // 
            this.OpenDiagnosis.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenDiagnosis.CornerRadius = 10;
            this.OpenDiagnosis.CustomerInformation = null;
            this.OpenDiagnosis.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenDiagnosis.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenDiagnosis.Location = new System.Drawing.Point(1164, 15);
            this.OpenDiagnosis.Margin = new System.Windows.Forms.Padding(15);
            this.OpenDiagnosis.Name = "OpenDiagnosis";
            this.OpenDiagnosis.OriginalColor = System.Drawing.Color.Gray;
            this.OpenDiagnosis.Size = new System.Drawing.Size(160, 55);
            this.OpenDiagnosis.TabIndex = 33;
            this.OpenDiagnosis.Text = "系统运行诊断";
            this.OpenDiagnosis.Click += new System.EventHandler(this.Function_Click);
            // 
            // Manufacturer
            // 
            this.Manufacturer.ActiveColor = System.Drawing.Color.Magenta;
            this.Manufacturer.CornerRadius = 10;
            this.Manufacturer.CustomerInformation = null;
            this.Manufacturer.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Manufacturer.ForeColor = System.Drawing.SystemColors.Control;
            this.Manufacturer.Location = new System.Drawing.Point(1354, 15);
            this.Manufacturer.Margin = new System.Windows.Forms.Padding(15);
            this.Manufacturer.Name = "Manufacturer";
            this.Manufacturer.OriginalColor = System.Drawing.Color.Gray;
            this.Manufacturer.Size = new System.Drawing.Size(160, 55);
            this.Manufacturer.TabIndex = 33;
            this.Manufacturer.Text = "厂商参数设定";
            this.Manufacturer.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenInternalColdControl
            // 
            this.OpenInternalColdControl.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenInternalColdControl.CornerRadius = 10;
            this.OpenInternalColdControl.CustomerInformation = null;
            this.OpenInternalColdControl.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenInternalColdControl.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenInternalColdControl.Location = new System.Drawing.Point(214, 15);
            this.OpenInternalColdControl.Margin = new System.Windows.Forms.Padding(15);
            this.OpenInternalColdControl.Name = "OpenInternalColdControl";
            this.OpenInternalColdControl.OriginalColor = System.Drawing.Color.Gray;
            this.OpenInternalColdControl.Size = new System.Drawing.Size(160, 55);
            this.OpenInternalColdControl.TabIndex = 33;
            this.OpenInternalColdControl.Text = "内部制冷控制";
            this.OpenInternalColdControl.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenExternalTempControl
            // 
            this.OpenExternalTempControl.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenExternalTempControl.CornerRadius = 10;
            this.OpenExternalTempControl.CustomerInformation = null;
            this.OpenExternalTempControl.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenExternalTempControl.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenExternalTempControl.Location = new System.Drawing.Point(404, 15);
            this.OpenExternalTempControl.Margin = new System.Windows.Forms.Padding(15);
            this.OpenExternalTempControl.Name = "OpenExternalTempControl";
            this.OpenExternalTempControl.OriginalColor = System.Drawing.Color.Gray;
            this.OpenExternalTempControl.Size = new System.Drawing.Size(160, 55);
            this.OpenExternalTempControl.TabIndex = 33;
            this.OpenExternalTempControl.Text = "外部温度控制";
            this.OpenExternalTempControl.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenUnitWorkState
            // 
            this.OpenUnitWorkState.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenUnitWorkState.CornerRadius = 10;
            this.OpenUnitWorkState.CustomerInformation = null;
            this.OpenUnitWorkState.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenUnitWorkState.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenUnitWorkState.Location = new System.Drawing.Point(974, 15);
            this.OpenUnitWorkState.Margin = new System.Windows.Forms.Padding(15);
            this.OpenUnitWorkState.Name = "OpenUnitWorkState";
            this.OpenUnitWorkState.OriginalColor = System.Drawing.Color.Gray;
            this.OpenUnitWorkState.Size = new System.Drawing.Size(160, 55);
            this.OpenUnitWorkState.TabIndex = 33;
            this.OpenUnitWorkState.Text = "机组工作状态";
            this.OpenUnitWorkState.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenInternalColdState
            // 
            this.OpenInternalColdState.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenInternalColdState.CornerRadius = 10;
            this.OpenInternalColdState.CustomerInformation = null;
            this.OpenInternalColdState.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenInternalColdState.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenInternalColdState.Location = new System.Drawing.Point(594, 15);
            this.OpenInternalColdState.Margin = new System.Windows.Forms.Padding(15);
            this.OpenInternalColdState.Name = "OpenInternalColdState";
            this.OpenInternalColdState.OriginalColor = System.Drawing.Color.Gray;
            this.OpenInternalColdState.Size = new System.Drawing.Size(160, 55);
            this.OpenInternalColdState.TabIndex = 33;
            this.OpenInternalColdState.Text = "内部制冷状态";
            this.OpenInternalColdState.Click += new System.EventHandler(this.Function_Click);
            // 
            // OpenExternalTempState
            // 
            this.OpenExternalTempState.ActiveColor = System.Drawing.Color.Magenta;
            this.OpenExternalTempState.CornerRadius = 10;
            this.OpenExternalTempState.CustomerInformation = null;
            this.OpenExternalTempState.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenExternalTempState.ForeColor = System.Drawing.SystemColors.Control;
            this.OpenExternalTempState.Location = new System.Drawing.Point(784, 15);
            this.OpenExternalTempState.Margin = new System.Windows.Forms.Padding(15);
            this.OpenExternalTempState.Name = "OpenExternalTempState";
            this.OpenExternalTempState.OriginalColor = System.Drawing.Color.Gray;
            this.OpenExternalTempState.Size = new System.Drawing.Size(160, 55);
            this.OpenExternalTempState.TabIndex = 33;
            this.OpenExternalTempState.Text = "外部温度状态";
            this.OpenExternalTempState.Click += new System.EventHandler(this.Function_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.ControlBox = false;
            this.Controls.Add(this.FunctionBase);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Home_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.FunctionBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DisplayTime;
        private System.Windows.Forms.Label DisplayDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel FunctionBase;
        private System.Windows.Forms.ComboBox UserGroup;
        private HslControls.HslButton OpenInternalColdControl;
        private HslControls.HslButton OpenAlmDisplay;
        private HslControls.HslButton OpenExternalTempControl;
        private HslControls.HslButton OpenInternalColdState;
        private HslControls.HslButton OpenExternalTempState;
        private HslControls.HslButton OpenUnitWorkState;
        private HslControls.HslButton OpenDiagnosis;
        private HslControls.HslButton Manufacturer;
        private HslControls.HslButton ExitSystem;
        private System.Windows.Forms.Label FreeTimeDisplay;
    }
}

