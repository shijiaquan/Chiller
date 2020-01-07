namespace Chiller
{
    partial class MessageDisplay
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
            this.DisplayMessage = new System.Windows.Forms.Label();
            this.Box = new System.Windows.Forms.Panel();
            this.Yes = new System.Windows.Forms.Button();
            this.No = new System.Windows.Forms.Button();
            this.Box.SuspendLayout();
            this.SuspendLayout();
            // 
            // DisplayMessage
            // 
            this.DisplayMessage.AutoSize = true;
            this.DisplayMessage.Location = new System.Drawing.Point(36, 75);
            this.DisplayMessage.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.DisplayMessage.Name = "DisplayMessage";
            this.DisplayMessage.Size = new System.Drawing.Size(255, 27);
            this.DisplayMessage.TabIndex = 11;
            this.DisplayMessage.Text = "确认是否退出系统？";
            this.DisplayMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Box
            // 
            this.Box.BackColor = System.Drawing.Color.White;
            this.Box.Controls.Add(this.DisplayMessage);
            this.Box.Dock = System.Windows.Forms.DockStyle.Top;
            this.Box.Location = new System.Drawing.Point(0, 0);
            this.Box.Margin = new System.Windows.Forms.Padding(7);
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(364, 177);
            this.Box.TabIndex = 1;
            // 
            // Yes
            // 
            this.Yes.Location = new System.Drawing.Point(117, 188);
            this.Yes.Name = "Yes";
            this.Yes.Size = new System.Drawing.Size(108, 46);
            this.Yes.TabIndex = 0;
            this.Yes.Text = "是";
            this.Yes.UseVisualStyleBackColor = true;
            this.Yes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // No
            // 
            this.No.Location = new System.Drawing.Point(244, 188);
            this.No.Name = "No";
            this.No.Size = new System.Drawing.Size(108, 46);
            this.No.TabIndex = 10;
            this.No.Text = "否";
            this.No.UseVisualStyleBackColor = true;
            this.No.Click += new System.EventHandler(this.No_Click);
            // 
            // MessageDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 246);
            this.ControlBox = false;
            this.Controls.Add(this.No);
            this.Controls.Add(this.Yes);
            this.Controls.Add(this.Box);
            this.Font = new System.Drawing.Font("宋体", 20F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageDisplay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MessageBox_Load);
            this.Box.ResumeLayout(false);
            this.Box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DisplayMessage;
        private System.Windows.Forms.Panel Box;
        private System.Windows.Forms.Button Yes;
        private System.Windows.Forms.Button No;
    }
}