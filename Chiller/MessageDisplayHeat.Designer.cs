namespace Chiller
{
    partial class MessageDisplayHeat
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
            this.HeatSetText = new System.Windows.Forms.Label();
            this.HeatSetProgressLine = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // HeatSetText
            // 
            this.HeatSetText.AutoSize = true;
            this.HeatSetText.Font = new System.Drawing.Font("宋体", 25F);
            this.HeatSetText.Location = new System.Drawing.Point(12, 40);
            this.HeatSetText.Name = "HeatSetText";
            this.HeatSetText.Size = new System.Drawing.Size(389, 34);
            this.HeatSetText.TabIndex = 0;
            this.HeatSetText.Text = "正在设定温控器…………";
            // 
            // HeatSetProgressLine
            // 
            this.HeatSetProgressLine.Location = new System.Drawing.Point(12, 104);
            this.HeatSetProgressLine.Name = "HeatSetProgressLine";
            this.HeatSetProgressLine.Size = new System.Drawing.Size(581, 38);
            this.HeatSetProgressLine.TabIndex = 6;
            // 
            // MessageDisplayHeat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 163);
            this.ControlBox = false;
            this.Controls.Add(this.HeatSetProgressLine);
            this.Controls.Add(this.HeatSetText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MessageDisplayHeat";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MessageDisplayHeat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeatSetText;
        private System.Windows.Forms.ProgressBar HeatSetProgressLine;
    }
}