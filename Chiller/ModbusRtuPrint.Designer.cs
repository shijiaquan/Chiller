namespace Chiller
{
    partial class ModbusRtuPrint
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
            this.Display = new System.Windows.Forms.TextBox();
            this.Run = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.TextBox();
            this.NowNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ControlDig = new System.Windows.Forms.Button();
            this.readPower = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Display.Location = new System.Drawing.Point(0, 95);
            this.Display.Multiline = true;
            this.Display.Name = "Display";
            this.Display.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Display.Size = new System.Drawing.Size(254, 496);
            this.Display.TabIndex = 0;
            // 
            // Run
            // 
            this.Run.AutoSize = true;
            this.Run.Location = new System.Drawing.Point(12, 12);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(72, 16);
            this.Run.TabIndex = 1;
            this.Run.Text = "开始刷新";
            this.Run.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "地址：";
            // 
            // Address
            // 
            this.Address.Location = new System.Drawing.Point(57, 37);
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(100, 21);
            this.Address.TabIndex = 3;
            this.Address.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.In_Put_Int);
            // 
            // NowNumber
            // 
            this.NowNumber.Enabled = false;
            this.NowNumber.Location = new System.Drawing.Point(57, 64);
            this.NowNumber.Name = "NowNumber";
            this.NowNumber.Size = new System.Drawing.Size(100, 21);
            this.NowNumber.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "当前值：";
            // 
            // ControlDig
            // 
            this.ControlDig.Location = new System.Drawing.Point(163, 37);
            this.ControlDig.Name = "ControlDig";
            this.ControlDig.Size = new System.Drawing.Size(70, 47);
            this.ControlDig.TabIndex = 6;
            this.ControlDig.Text = "控制诊断";
            this.ControlDig.UseVisualStyleBackColor = true;
            this.ControlDig.Click += new System.EventHandler(this.ControlDig_Click);
            // 
            // readPower
            // 
            this.readPower.AutoSize = true;
            this.readPower.Location = new System.Drawing.Point(155, 12);
            this.readPower.Name = "readPower";
            this.readPower.Size = new System.Drawing.Size(72, 16);
            this.readPower.TabIndex = 7;
            this.readPower.Text = "查看功率";
            this.readPower.UseVisualStyleBackColor = true;
            // 
            // ModbusRtuPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 591);
            this.Controls.Add(this.readPower);
            this.Controls.Add(this.ControlDig);
            this.Controls.Add(this.NowNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Address);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.Display);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModbusRtuPrint";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModbusRtu诊断";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModbusRtuPrint_FormClosed);
            this.Load += new System.EventHandler(this.ModbusRtuPrint_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Display;
        private System.Windows.Forms.CheckBox Run;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Address;
        private System.Windows.Forms.TextBox NowNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ControlDig;
        private System.Windows.Forms.CheckBox readPower;
    }
}