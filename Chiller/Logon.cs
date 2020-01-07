using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chiller
{
    public partial class Logon : Form
    {
        public Logon()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string ReturnUserName { get; protected set; }   
        /// <summary>
        /// 访问权限
        /// </summary>
        public FlagEnum.UserAuthority ReturnUserAuthority { get; protected set; }
        /// <summary>
        /// 是否使用自动登出
        /// </summary>
        public bool IsAutoLogout { get; protected set; }               
        /// <summary>
        /// 自动登出时间
        /// </summary>
        public int AutoLogoutTime { get; protected set; }

        private void Logon_Cancel_Click(object sender, EventArgs e)
        {
            PressLogon_Cancel();
        }

        private void Logon_OK_Click(object sender, EventArgs e)
        {
            PressLogon_OK();
        }

        private void AutoLogout_Click(object sender, EventArgs e)
        {
            this.LogoutTime.Enabled = this.AutoLogout.Checked;
        }

        private void ButtonKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                PressLogon_Cancel();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                PressLogon_OK();
            }
        }

        private void PressLogon_OK()
        {
            #region 判断是否开启自动登出
            if (this.LogoutTime.Text != "")
            {
                IsAutoLogout = this.AutoLogout.Checked;
                AutoLogoutTime = int.Parse(this.LogoutTime.Text.Substring(0, this.LogoutTime.Text.Length - 3));
            }
            else
            {
                IsAutoLogout = false;
                AutoLogoutTime = 0;
            }
            #endregion

            #region 验证用户登陆情况
            if (User_Name.Text == "None")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else if (User_Name.Text == "Engineer")
            {
                if (Password.Text == "333")
                {
                    ReturnUserAuthority = FlagEnum.UserAuthority.Engineer;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.Password.SelectAll();
                    MessageBox.Show("账户验证失败，账户或口令错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (User_Name.Text == "Administrator")
            {
                if (Password.Text == "888")
                {
                    ReturnUserAuthority = FlagEnum.UserAuthority.Administrator;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.Password.SelectAll();
                    MessageBox.Show("账户验证失败，账户或口令错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion
        }

        private void PressLogon_Cancel()
        {
            IsAutoLogout = false;
            AutoLogoutTime = 0;
            ReturnUserAuthority = FlagEnum.UserAuthority.None;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
