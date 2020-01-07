namespace Chiller
{
    partial class Logon
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
            this.User_Name = new System.Windows.Forms.ComboBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.Password_lb = new System.Windows.Forms.Label();
            this.User_Name_lb = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Logon_OK = new HslControls.HslButton();
            this.Logon_Cancel = new HslControls.HslButton();
            this.label1 = new System.Windows.Forms.Label();
            this.AutoLogout = new System.Windows.Forms.CheckBox();
            this.LogoutTime = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // User_Name
            // 
            this.User_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.User_Name.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.User_Name.FormattingEnabled = true;
            this.User_Name.Items.AddRange(new object[] {
            "None",
            "Engineer",
            "Administrator"});
            this.User_Name.Location = new System.Drawing.Point(317, 96);
            this.User_Name.Name = "User_Name";
            this.User_Name.Size = new System.Drawing.Size(220, 32);
            this.User_Name.TabIndex = 56;
            this.User_Name.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // Password
            // 
            this.Password.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password.Location = new System.Drawing.Point(317, 134);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(220, 35);
            this.Password.TabIndex = 53;
            this.Password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // Password_lb
            // 
            this.Password_lb.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password_lb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Password_lb.Location = new System.Drawing.Point(224, 140);
            this.Password_lb.Name = "Password_lb";
            this.Password_lb.Size = new System.Drawing.Size(96, 23);
            this.Password_lb.TabIndex = 52;
            this.Password_lb.Text = "密码：";
            this.Password_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // User_Name_lb
            // 
            this.User_Name_lb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.User_Name_lb.Location = new System.Drawing.Point(224, 101);
            this.User_Name_lb.Name = "User_Name_lb";
            this.User_Name_lb.Size = new System.Drawing.Size(96, 23);
            this.User_Name_lb.TabIndex = 51;
            this.User_Name_lb.Text = "用户名：";
            this.User_Name_lb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Chiller.Properties.Resources.user_2_256px;
            this.pictureBox2.Location = new System.Drawing.Point(36, 268);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(192, 44);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 50;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Chiller.Properties.Resources.users_unlock_256px;
            this.pictureBox1.Location = new System.Drawing.Point(57, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            // 
            // Logon_OK
            // 
            this.Logon_OK.CornerRadius = 10;
            this.Logon_OK.CustomerInformation = null;
            this.Logon_OK.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Logon_OK.Location = new System.Drawing.Point(259, 262);
            this.Logon_OK.Name = "Logon_OK";
            this.Logon_OK.OriginalColor = System.Drawing.Color.Orange;
            this.Logon_OK.Size = new System.Drawing.Size(120, 50);
            this.Logon_OK.TabIndex = 57;
            this.Logon_OK.Text = "登陆";
            this.Logon_OK.Click += new System.EventHandler(this.Logon_OK_Click);
            this.Logon_OK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // Logon_Cancel
            // 
            this.Logon_Cancel.CornerRadius = 10;
            this.Logon_Cancel.CustomerInformation = null;
            this.Logon_Cancel.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Logon_Cancel.Location = new System.Drawing.Point(417, 262);
            this.Logon_Cancel.Name = "Logon_Cancel";
            this.Logon_Cancel.OriginalColor = System.Drawing.Color.Orange;
            this.Logon_Cancel.Size = new System.Drawing.Size(120, 50);
            this.Logon_Cancel.TabIndex = 57;
            this.Logon_Cancel.Text = "取消";
            this.Logon_Cancel.Click += new System.EventHandler(this.Logon_Cancel_Click);
            this.Logon_Cancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(89, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 40);
            this.label1.TabIndex = 58;
            this.label1.Text = "Refrigeration System";
            // 
            // AutoLogout
            // 
            this.AutoLogout.AutoSize = true;
            this.AutoLogout.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AutoLogout.Location = new System.Drawing.Point(317, 195);
            this.AutoLogout.Name = "AutoLogout";
            this.AutoLogout.Size = new System.Drawing.Size(125, 28);
            this.AutoLogout.TabIndex = 59;
            this.AutoLogout.Text = "自动登出";
            this.AutoLogout.UseVisualStyleBackColor = true;
            this.AutoLogout.Click += new System.EventHandler(this.AutoLogout_Click);
            this.AutoLogout.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // LogoutTime
            // 
            this.LogoutTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LogoutTime.Enabled = false;
            this.LogoutTime.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogoutTime.FormattingEnabled = true;
            this.LogoutTime.Items.AddRange(new object[] {
            "1min",
            "5min",
            "10min",
            "20min",
            "30min",
            "40min",
            "50min",
            "60min"});
            this.LogoutTime.Location = new System.Drawing.Point(437, 193);
            this.LogoutTime.Name = "LogoutTime";
            this.LogoutTime.Size = new System.Drawing.Size(100, 32);
            this.LogoutTime.TabIndex = 60;
            this.LogoutTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            // 
            // Logon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(608, 361);
            this.ControlBox = false;
            this.Controls.Add(this.LogoutTime);
            this.Controls.Add(this.AutoLogout);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Logon_Cancel);
            this.Controls.Add(this.Logon_OK);
            this.Controls.Add(this.User_Name);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Password_lb);
            this.Controls.Add(this.User_Name_lb);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Logon";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "用户登陆";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ButtonKeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox User_Name;
        public System.Windows.Forms.TextBox Password;
        internal System.Windows.Forms.Label Password_lb;
        internal System.Windows.Forms.Label User_Name_lb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private HslControls.HslButton Logon_OK;
        private HslControls.HslButton Logon_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox AutoLogout;
        public System.Windows.Forms.ComboBox LogoutTime;
    }
}