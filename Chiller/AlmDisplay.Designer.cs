namespace Chiller
{
    partial class AlmDisplay
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.NowLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ModbusRtuData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProhibitHandlerControl = new Switch.ZDSwitch();
            this.Reset = new HslControls.HslButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Paragraph = new System.Windows.Forms.ComboBox();
            this.Paragraph_lb = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.RunLog = new System.Windows.Forms.RichTextBox();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.NowLog);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 32);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(794, 841);
            this.panel4.TabIndex = 25;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel5.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(794, 3);
            this.panel5.TabIndex = 3;
            // 
            // NowLog
            // 
            this.NowLog.BackColor = System.Drawing.Color.White;
            this.NowLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NowLog.Enabled = false;
            this.NowLog.Font = new System.Drawing.Font("宋体", 15F);
            this.NowLog.Location = new System.Drawing.Point(0, 0);
            this.NowLog.Name = "NowLog";
            this.NowLog.ReadOnly = true;
            this.NowLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.NowLog.Size = new System.Drawing.Size(794, 841);
            this.NowLog.TabIndex = 22;
            this.NowLog.TabStop = false;
            this.NowLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(239, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 876);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备当前信息";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SkyBlue;
            this.panel2.Controls.Add(this.ModbusRtuData);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.ProhibitHandlerControl);
            this.panel2.Controls.Add(this.Reset);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 900);
            this.panel2.TabIndex = 35;
            // 
            // ModbusRtuData
            // 
            this.ModbusRtuData.Font = new System.Drawing.Font("宋体", 15F);
            this.ModbusRtuData.Location = new System.Drawing.Point(18, 374);
            this.ModbusRtuData.Name = "ModbusRtuData";
            this.ModbusRtuData.Size = new System.Drawing.Size(165, 57);
            this.ModbusRtuData.TabIndex = 3;
            this.ModbusRtuData.Text = "ModbusRtu诊断";
            this.ModbusRtuData.UseVisualStyleBackColor = true;
            this.ModbusRtuData.Click += new System.EventHandler(this.ModbusRtuData_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F);
            this.label1.Location = new System.Drawing.Point(17, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Handler控制权";
            // 
            // ProhibitHandlerControl
            // 
            this.ProhibitHandlerControl.BackColor = System.Drawing.Color.Transparent;
            this.ProhibitHandlerControl.Checked = false;
            this.ProhibitHandlerControl.FalseColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.ProhibitHandlerControl.Location = new System.Drawing.Point(18, 212);
            this.ProhibitHandlerControl.Name = "ProhibitHandlerControl";
            this.ProhibitHandlerControl.Size = new System.Drawing.Size(165, 68);
            this.ProhibitHandlerControl.SwitchType = Switch.SwitchType.Ellipse;
            this.ProhibitHandlerControl.TabIndex = 1;
            this.ProhibitHandlerControl.Texts = null;
            this.ProhibitHandlerControl.TrueColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ProhibitHandlerControl.CheckedChanged += new System.EventHandler(this.ProhibitHandlerControl_CheckedChanged);
            // 
            // Reset
            // 
            this.Reset.CornerRadius = 30;
            this.Reset.CustomerInformation = null;
            this.Reset.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Reset.Location = new System.Drawing.Point(18, 44);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(165, 104);
            this.Reset.TabIndex = 0;
            this.Reset.Text = "复位";
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Paragraph);
            this.groupBox2.Controls.Add(this.Paragraph_lb);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(1087, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(800, 876);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统运行日志";
            // 
            // Paragraph
            // 
            this.Paragraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Paragraph.FormattingEnabled = true;
            this.Paragraph.Items.AddRange(new object[] {
            "Now"});
            this.Paragraph.Location = new System.Drawing.Point(300, 29);
            this.Paragraph.Name = "Paragraph";
            this.Paragraph.Size = new System.Drawing.Size(304, 28);
            this.Paragraph.TabIndex = 27;
            this.Paragraph.SelectionChangeCommitted += new System.EventHandler(this.Paragraph_SelectionChangeCommitted);
            // 
            // Paragraph_lb
            // 
            this.Paragraph_lb.AutoSize = true;
            this.Paragraph_lb.Location = new System.Drawing.Point(180, 32);
            this.Paragraph_lb.Name = "Paragraph_lb";
            this.Paragraph_lb.Size = new System.Drawing.Size(114, 20);
            this.Paragraph_lb.TabIndex = 26;
            this.Paragraph_lb.Text = "查询段落：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.RunLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 807);
            this.panel1.TabIndex = 25;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(794, 3);
            this.panel3.TabIndex = 3;
            // 
            // RunLog
            // 
            this.RunLog.BackColor = System.Drawing.Color.White;
            this.RunLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunLog.Font = new System.Drawing.Font("宋体", 15F);
            this.RunLog.Location = new System.Drawing.Point(0, 0);
            this.RunLog.Name = "RunLog";
            this.RunLog.ReadOnly = true;
            this.RunLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.RunLog.Size = new System.Drawing.Size(794, 807);
            this.RunLog.TabIndex = 22;
            this.RunLog.TabStop = false;
            this.RunLog.Text = "";
            // 
            // AlmDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 900);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(0, 90);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlmDisplay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.AlmDisplay_Load);
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RichTextBox NowLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox RunLog;
        private HslControls.HslButton Reset;
        private System.Windows.Forms.ComboBox Paragraph;
        private System.Windows.Forms.Label Paragraph_lb;
        private System.Windows.Forms.Label label1;
        private Switch.ZDSwitch ProhibitHandlerControl;
        private System.Windows.Forms.Button ModbusRtuData;
    }
}