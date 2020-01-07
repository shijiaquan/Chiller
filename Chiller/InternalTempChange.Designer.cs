namespace Chiller
{
    partial class InternalTempChange
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
            this.SetTemp = new System.Windows.Forms.TextBox();
            this.Password_lb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SetRange = new System.Windows.Forms.TextBox();
            this.panel51 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Chiller = new System.Windows.Forms.Label();
            this.ChillerOffset3 = new System.Windows.Forms.TextBox();
            this.ChillerOffset2 = new System.Windows.Forms.TextBox();
            this.ChillerOffset4 = new System.Windows.Forms.TextBox();
            this.ChillerOffset1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ChillerOne = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.ChillerTwo = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.ChillerThree = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.BasePoint1 = new System.Windows.Forms.TextBox();
            this.BasePoint3 = new System.Windows.Forms.TextBox();
            this.BasePoint2 = new System.Windows.Forms.TextBox();
            this.panel67 = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.TempSetApply = new HslControls.HslButton();
            this.TempSetClose = new HslControls.HslButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.panel51.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel67.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetTemp
            // 
            this.SetTemp.Font = new System.Drawing.Font("宋体", 18F);
            this.SetTemp.Location = new System.Drawing.Point(272, 61);
            this.SetTemp.Name = "SetTemp";
            this.SetTemp.Size = new System.Drawing.Size(212, 35);
            this.SetTemp.TabIndex = 1;
            this.SetTemp.Text = "-100";
            this.SetTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SetTemp.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.SetTemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // Password_lb
            // 
            this.Password_lb.AutoSize = true;
            this.Password_lb.Font = new System.Drawing.Font("宋体", 18F);
            this.Password_lb.ForeColor = System.Drawing.Color.Black;
            this.Password_lb.Location = new System.Drawing.Point(90, 66);
            this.Password_lb.Name = "Password_lb";
            this.Password_lb.Size = new System.Drawing.Size(178, 24);
            this.Password_lb.TabIndex = 54;
            this.Password_lb.Text = "设定温度（℃）";
            this.Password_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(90, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 24);
            this.label1.TabIndex = 54;
            this.label1.Text = "温度范围（℃）";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SetRange
            // 
            this.SetRange.Font = new System.Drawing.Font("宋体", 18F);
            this.SetRange.Location = new System.Drawing.Point(272, 102);
            this.SetRange.Name = "SetRange";
            this.SetRange.Size = new System.Drawing.Size(212, 35);
            this.SetRange.TabIndex = 2;
            this.SetRange.Text = "3";
            this.SetRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SetRange.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // panel51
            // 
            this.panel51.BackColor = System.Drawing.Color.White;
            this.panel51.Controls.Add(this.panel3);
            this.panel51.Controls.Add(this.label6);
            this.panel51.Controls.Add(this.label7);
            this.panel51.Controls.Add(this.label8);
            this.panel51.Controls.Add(this.ChillerOne);
            this.panel51.Controls.Add(this.label9);
            this.panel51.Controls.Add(this.ChillerTwo);
            this.panel51.Controls.Add(this.label10);
            this.panel51.Controls.Add(this.ChillerThree);
            this.panel51.Controls.Add(this.label11);
            this.panel51.Controls.Add(this.BasePoint1);
            this.panel51.Controls.Add(this.BasePoint3);
            this.panel51.Controls.Add(this.BasePoint2);
            this.panel51.Controls.Add(this.panel67);
            this.panel51.Font = new System.Drawing.Font("宋体", 10F);
            this.panel51.Location = new System.Drawing.Point(12, 164);
            this.panel51.Name = "panel51";
            this.panel51.Size = new System.Drawing.Size(577, 233);
            this.panel51.TabIndex = 57;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.Chiller);
            this.panel3.Controls.Add(this.ChillerOffset3);
            this.panel3.Controls.Add(this.ChillerOffset2);
            this.panel3.Controls.Add(this.ChillerOffset4);
            this.panel3.Controls.Add(this.ChillerOffset1);
            this.panel3.Location = new System.Drawing.Point(4, 110);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(568, 37);
            this.panel3.TabIndex = 71;
            // 
            // Chiller
            // 
            this.Chiller.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chiller.Dock = System.Windows.Forms.DockStyle.Left;
            this.Chiller.Font = new System.Drawing.Font("宋体", 18F);
            this.Chiller.Location = new System.Drawing.Point(0, 0);
            this.Chiller.Margin = new System.Windows.Forms.Padding(0);
            this.Chiller.Name = "Chiller";
            this.Chiller.Size = new System.Drawing.Size(114, 35);
            this.Chiller.TabIndex = 70;
            this.Chiller.Text = "Chiller";
            this.Chiller.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChillerOffset3
            // 
            this.ChillerOffset3.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerOffset3.Location = new System.Drawing.Point(339, 0);
            this.ChillerOffset3.Margin = new System.Windows.Forms.Padding(0);
            this.ChillerOffset3.Name = "ChillerOffset3";
            this.ChillerOffset3.Size = new System.Drawing.Size(114, 35);
            this.ChillerOffset3.TabIndex = 2;
            this.ChillerOffset3.Text = "40";
            this.ChillerOffset3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChillerOffset3.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.ChillerOffset3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // ChillerOffset2
            // 
            this.ChillerOffset2.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerOffset2.Location = new System.Drawing.Point(226, 0);
            this.ChillerOffset2.Margin = new System.Windows.Forms.Padding(0);
            this.ChillerOffset2.Name = "ChillerOffset2";
            this.ChillerOffset2.Size = new System.Drawing.Size(114, 35);
            this.ChillerOffset2.TabIndex = 2;
            this.ChillerOffset2.Text = "40";
            this.ChillerOffset2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChillerOffset2.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.ChillerOffset2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // ChillerOffset4
            // 
            this.ChillerOffset4.Enabled = false;
            this.ChillerOffset4.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerOffset4.Location = new System.Drawing.Point(452, 0);
            this.ChillerOffset4.Margin = new System.Windows.Forms.Padding(0);
            this.ChillerOffset4.Name = "ChillerOffset4";
            this.ChillerOffset4.Size = new System.Drawing.Size(114, 35);
            this.ChillerOffset4.TabIndex = 2;
            this.ChillerOffset4.Text = "40";
            this.ChillerOffset4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChillerOffset4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // ChillerOffset1
            // 
            this.ChillerOffset1.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerOffset1.Location = new System.Drawing.Point(113, 0);
            this.ChillerOffset1.Margin = new System.Windows.Forms.Padding(0);
            this.ChillerOffset1.Name = "ChillerOffset1";
            this.ChillerOffset1.Size = new System.Drawing.Size(114, 35);
            this.ChillerOffset1.TabIndex = 2;
            this.ChillerOffset1.Text = "40";
            this.ChillerOffset1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChillerOffset1.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.ChillerOffset1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 18F);
            this.label6.Location = new System.Drawing.Point(335, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 24);
            this.label6.TabIndex = 64;
            this.label6.Text = "<";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 18F);
            this.label7.Location = new System.Drawing.Point(16, 73);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 24);
            this.label7.TabIndex = 55;
            this.label7.Text = "类型：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 16F);
            this.label8.Location = new System.Drawing.Point(364, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 22);
            this.label8.TabIndex = 66;
            this.label8.Text = "基点#3";
            // 
            // ChillerOne
            // 
            this.ChillerOne.AutoSize = true;
            this.ChillerOne.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerOne.Location = new System.Drawing.Point(138, 71);
            this.ChillerOne.Name = "ChillerOne";
            this.ChillerOne.Size = new System.Drawing.Size(64, 28);
            this.ChillerOne.TabIndex = 56;
            this.ChillerOne.TabStop = true;
            this.ChillerOne.Text = "1点";
            this.ChillerOne.UseVisualStyleBackColor = true;
            this.ChillerOne.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChillerOne_MouseClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 16F);
            this.label9.Location = new System.Drawing.Point(250, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 22);
            this.label9.TabIndex = 67;
            this.label9.Text = "基点#2";
            // 
            // ChillerTwo
            // 
            this.ChillerTwo.AutoSize = true;
            this.ChillerTwo.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerTwo.Location = new System.Drawing.Point(251, 71);
            this.ChillerTwo.Name = "ChillerTwo";
            this.ChillerTwo.Size = new System.Drawing.Size(64, 28);
            this.ChillerTwo.TabIndex = 57;
            this.ChillerTwo.TabStop = true;
            this.ChillerTwo.Text = "2点";
            this.ChillerTwo.UseVisualStyleBackColor = true;
            this.ChillerTwo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChillerOne_MouseClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 16F);
            this.label10.Location = new System.Drawing.Point(135, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 22);
            this.label10.TabIndex = 68;
            this.label10.Text = "基点#1";
            // 
            // ChillerThree
            // 
            this.ChillerThree.AutoSize = true;
            this.ChillerThree.Font = new System.Drawing.Font("宋体", 18F);
            this.ChillerThree.Location = new System.Drawing.Point(364, 71);
            this.ChillerThree.Name = "ChillerThree";
            this.ChillerThree.Size = new System.Drawing.Size(64, 28);
            this.ChillerThree.TabIndex = 58;
            this.ChillerThree.TabStop = true;
            this.ChillerThree.Text = "3点";
            this.ChillerThree.UseVisualStyleBackColor = true;
            this.ChillerThree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChillerOne_MouseClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 18F);
            this.label11.Location = new System.Drawing.Point(219, 164);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 24);
            this.label11.TabIndex = 69;
            this.label11.Text = "<";
            // 
            // BasePoint1
            // 
            this.BasePoint1.Font = new System.Drawing.Font("宋体", 16F);
            this.BasePoint1.Location = new System.Drawing.Point(130, 160);
            this.BasePoint1.Name = "BasePoint1";
            this.BasePoint1.Size = new System.Drawing.Size(87, 32);
            this.BasePoint1.TabIndex = 60;
            this.BasePoint1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BasePoint1.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.BasePoint1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // BasePoint3
            // 
            this.BasePoint3.Font = new System.Drawing.Font("宋体", 16F);
            this.BasePoint3.Location = new System.Drawing.Point(359, 160);
            this.BasePoint3.Name = "BasePoint3";
            this.BasePoint3.Size = new System.Drawing.Size(87, 32);
            this.BasePoint3.TabIndex = 62;
            this.BasePoint3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BasePoint3.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.BasePoint3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
            // 
            // BasePoint2
            // 
            this.BasePoint2.Font = new System.Drawing.Font("宋体", 16F);
            this.BasePoint2.Location = new System.Drawing.Point(245, 160);
            this.BasePoint2.Name = "BasePoint2";
            this.BasePoint2.Size = new System.Drawing.Size(87, 32);
            this.BasePoint2.TabIndex = 63;
            this.BasePoint2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BasePoint2.TextChanged += new System.EventHandler(this.OffsetNumberChange);
            this.BasePoint2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Float);
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
            this.panel67.Size = new System.Drawing.Size(577, 55);
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
            this.label27.Size = new System.Drawing.Size(577, 55);
            this.label27.TabIndex = 6;
            this.label27.Text = "偏移";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TempSetApply
            // 
            this.TempSetApply.CornerRadius = 10;
            this.TempSetApply.CustomerInformation = null;
            this.TempSetApply.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.TempSetApply.Location = new System.Drawing.Point(139, 416);
            this.TempSetApply.Name = "TempSetApply";
            this.TempSetApply.Size = new System.Drawing.Size(138, 63);
            this.TempSetApply.TabIndex = 59;
            this.TempSetApply.Text = "应用";
            this.TempSetApply.Click += new System.EventHandler(this.ChartSetApply_Click);
            // 
            // TempSetClose
            // 
            this.TempSetClose.CornerRadius = 10;
            this.TempSetClose.CustomerInformation = null;
            this.TempSetClose.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold);
            this.TempSetClose.Location = new System.Drawing.Point(314, 416);
            this.TempSetClose.Name = "TempSetClose";
            this.TempSetClose.Size = new System.Drawing.Size(138, 63);
            this.TempSetClose.TabIndex = 59;
            this.TempSetClose.Text = "关闭";
            this.TempSetClose.Click += new System.EventHandler(this.ChartSetCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.SetTemp);
            this.panel1.Controls.Add(this.Password_lb);
            this.panel1.Controls.Add(this.SetRange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("宋体", 10F);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 146);
            this.panel1.TabIndex = 60;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel2.Controls.Add(this.label14);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(577, 55);
            this.panel2.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(577, 55);
            this.label14.TabIndex = 6;
            this.label14.Text = "温度";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InternalTempChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(599, 493);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TempSetClose);
            this.Controls.Add(this.TempSetApply);
            this.Controls.Add(this.panel51);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InternalTempChange";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "水箱温度设置";
            this.Load += new System.EventHandler(this.InternalColdControlChart_Load);
            this.panel51.ResumeLayout(false);
            this.panel51.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel67.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox SetTemp;
        internal System.Windows.Forms.Label Password_lb;
        internal System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox SetRange;
        private System.Windows.Forms.Panel panel51;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Panel panel67;
        private HslControls.HslButton TempSetApply;
        private HslControls.HslButton TempSetClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton ChillerOne;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton ChillerTwo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton ChillerThree;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox BasePoint1;
        private System.Windows.Forms.TextBox BasePoint3;
        private System.Windows.Forms.TextBox BasePoint2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label Chiller;
        public System.Windows.Forms.TextBox ChillerOffset1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox ChillerOffset3;
        public System.Windows.Forms.TextBox ChillerOffset2;
        public System.Windows.Forms.TextBox ChillerOffset4;
    }
}