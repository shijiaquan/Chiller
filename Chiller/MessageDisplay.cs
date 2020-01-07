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
    public partial class MessageDisplay : Form
    {
        public MessageDisplay(string Message , string TopMessage)
        {
            InitializeComponent();
            this.Text = TopMessage;
            this.DisplayMessage.Text = Message;
        }

        public MessageDisplay(string Message)
        {
            InitializeComponent();
            this.Text = "";
            this.DisplayMessage.Text = Message;
        }

        private void MessageBox_Load(object sender, EventArgs e)
        {
            int Height = this.DisplayMessage.Size.Height + 120;
            int BoxWidth = this.DisplayMessage.Size.Width + 125 ;
            int BoxHeight = Height + 108;
            if(BoxWidth < 380)
            {
                BoxWidth = 380;
            }
            this.Width = BoxWidth;
            this.Height = BoxHeight;

            this.Box.Height = Height;

            Point Pos = new Point();

            Pos.Y = this.Size.Height - 97;
            Pos.X = this.Size.Width - 136;
            this.No.Location = Pos;

            Pos.Y = this.Size.Height - 97;
            Pos.X = this.Size.Width - 263;
            this.Yes.Location = Pos;

            Pos.X = 36;
            Pos.Y = 60;

            this.DisplayMessage.Location = Pos;
            this.CenterToScreen();
            this.TopMost = true;
        }

        private void No_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }
    }
}
