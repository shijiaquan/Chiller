namespace Chiller
{
    partial class ExternalTempVarietiesList
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
            this.VarietiesList = new System.Windows.Forms.ListBox();
            this.VarietiesName = new System.Windows.Forms.TextBox();
            this.CloseVarieties = new System.Windows.Forms.Button();
            this.NewVarieties = new System.Windows.Forms.Button();
            this.CopyVarieties = new System.Windows.Forms.Button();
            this.DeleteVarieties = new System.Windows.Forms.Button();
            this.ChangeVarieties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // VarietiesList
            // 
            this.VarietiesList.Font = new System.Drawing.Font("宋体", 30F);
            this.VarietiesList.FormattingEnabled = true;
            this.VarietiesList.ItemHeight = 40;
            this.VarietiesList.Location = new System.Drawing.Point(12, 87);
            this.VarietiesList.Name = "VarietiesList";
            this.VarietiesList.Size = new System.Drawing.Size(278, 484);
            this.VarietiesList.TabIndex = 0;
            // 
            // VarietiesName
            // 
            this.VarietiesName.Font = new System.Drawing.Font("宋体", 30F);
            this.VarietiesName.Location = new System.Drawing.Point(12, 28);
            this.VarietiesName.Name = "VarietiesName";
            this.VarietiesName.Size = new System.Drawing.Size(278, 53);
            this.VarietiesName.TabIndex = 1;
            // 
            // CloseVarieties
            // 
            this.CloseVarieties.Font = new System.Drawing.Font("宋体", 15F);
            this.CloseVarieties.Location = new System.Drawing.Point(310, 498);
            this.CloseVarieties.Name = "CloseVarieties";
            this.CloseVarieties.Size = new System.Drawing.Size(135, 73);
            this.CloseVarieties.TabIndex = 2;
            this.CloseVarieties.Text = "退出";
            this.CloseVarieties.UseVisualStyleBackColor = true;
            this.CloseVarieties.Click += new System.EventHandler(this.VarietiesClose_Click);
            // 
            // NewVarieties
            // 
            this.NewVarieties.Font = new System.Drawing.Font("宋体", 15F);
            this.NewVarieties.Location = new System.Drawing.Point(310, 28);
            this.NewVarieties.Name = "NewVarieties";
            this.NewVarieties.Size = new System.Drawing.Size(135, 73);
            this.NewVarieties.TabIndex = 2;
            this.NewVarieties.Text = "新建";
            this.NewVarieties.UseVisualStyleBackColor = true;
            this.NewVarieties.Click += new System.EventHandler(this.NewVarieties_Click);
            // 
            // CopyVarieties
            // 
            this.CopyVarieties.Font = new System.Drawing.Font("宋体", 15F);
            this.CopyVarieties.Location = new System.Drawing.Point(310, 125);
            this.CopyVarieties.Name = "CopyVarieties";
            this.CopyVarieties.Size = new System.Drawing.Size(135, 73);
            this.CopyVarieties.TabIndex = 2;
            this.CopyVarieties.Text = "复制";
            this.CopyVarieties.UseVisualStyleBackColor = true;
            this.CopyVarieties.Click += new System.EventHandler(this.CopyVarieties_Click);
            // 
            // DeleteVarieties
            // 
            this.DeleteVarieties.Font = new System.Drawing.Font("宋体", 15F);
            this.DeleteVarieties.Location = new System.Drawing.Point(310, 222);
            this.DeleteVarieties.Name = "DeleteVarieties";
            this.DeleteVarieties.Size = new System.Drawing.Size(135, 73);
            this.DeleteVarieties.TabIndex = 2;
            this.DeleteVarieties.Text = "删除";
            this.DeleteVarieties.UseVisualStyleBackColor = true;
            this.DeleteVarieties.Click += new System.EventHandler(this.DeleteVarieties_Click);
            // 
            // ChangeVarieties
            // 
            this.ChangeVarieties.Font = new System.Drawing.Font("宋体", 15F);
            this.ChangeVarieties.Location = new System.Drawing.Point(310, 319);
            this.ChangeVarieties.Name = "ChangeVarieties";
            this.ChangeVarieties.Size = new System.Drawing.Size(135, 73);
            this.ChangeVarieties.TabIndex = 2;
            this.ChangeVarieties.Text = "更名";
            this.ChangeVarieties.UseVisualStyleBackColor = true;
            this.ChangeVarieties.Click += new System.EventHandler(this.ChangeVarieties_Click);
            // 
            // ExternalTempVarietiesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 593);
            this.ControlBox = false;
            this.Controls.Add(this.ChangeVarieties);
            this.Controls.Add(this.DeleteVarieties);
            this.Controls.Add(this.CopyVarieties);
            this.Controls.Add(this.NewVarieties);
            this.Controls.Add(this.CloseVarieties);
            this.Controls.Add(this.VarietiesName);
            this.Controls.Add(this.VarietiesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExternalTempVarietiesList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "温度品种设置";
            this.Load += new System.EventHandler(this.ExternalTempVarieties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox VarietiesList;
        private System.Windows.Forms.TextBox VarietiesName;
        private System.Windows.Forms.Button CloseVarieties;
        private System.Windows.Forms.Button NewVarieties;
        private System.Windows.Forms.Button CopyVarieties;
        private System.Windows.Forms.Button DeleteVarieties;
        private System.Windows.Forms.Button ChangeVarieties;
    }
}