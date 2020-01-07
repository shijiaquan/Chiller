using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ColorPicker
{
    public partial class ColorFrom : Form
    {
        #region ����

        #region ���Ա���
        private bool showTip;
        private Color hoverBKColor;
        private List<ColorItem> colorTable;
        #endregion
        /// <summary>
        /// ���С
        /// </summary>
        private int BOX_SIZE = 30;
        /// <summary>
        /// �߾�
        /// </summary>
        private int MARGIN = 3;
        /// <summary>
        /// ��ɫ��߾�
        /// </summary>
        private int ITEM_MARGIN = 3;
        /// <summary>
        /// ����ʵ��
        /// </summary>
        private static ColorFrom instance;
        /// <summary>
        /// �����ʽ
        /// </summary>
        private StringFormat sf;
        /// <summary>
        /// ����µ���ɫ������
        /// </summary>
        private int hoverItem = -1;
        /// <summary>
        /// ������
        /// </summary>
        private IWin32Window owner;
        /// <summary>
        /// ����ˢ
        /// </summary>
        private Brush hoverBrush;

        #endregion

        #region �¼�
        /// <summary>
        /// ��ɫ����ί��
        /// </summary>
        /// <param name="color"></param>
        public delegate void SelectedChange(Color color);
        /// <summary>
        /// ��ɫ��ѡ��
        /// </summary>
        public event SelectedChange SelectedColorChanged;

        #endregion

        #region ��������

        /// <summary>
        /// ѡ�е���ɫ
        /// </summary>
        public Color ResultColor { get; private set; }

        [Description("�Ƿ���ʾ��ɫ��ʾ")]
        [DefaultValue(true)]
        public bool ShowTip
        {
            get { return showTip; }
            set { showTip = value; }
        }

        [Description("��������ɫ")]
        [DefaultValue(typeof(Color), "255, 238, 194")]
        public Color HoverBKColor
        {
            get { return hoverBKColor; }
            set
            {
                if (hoverBKColor != value)
                {
                    if (hoverBrush != null)
                        hoverBrush.Dispose();
                    hoverBrush = new SolidBrush(value);
                }
                hoverBKColor = value;
            }
        }

        /// <summary>
        /// ��ɫ��
        /// </summary>
        internal List<ColorItem> ColorTable
        {
            get { return colorTable; }
        }
        #endregion

        #region ��ʼ��

        private ColorFrom(IWin32Window owner, int BoxSize)
        {
            BOX_SIZE = BoxSize;
            this.owner = owner;
            InitializeComponent();
            InitColor();
            CalcWindowSize();

            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            HoverBKColor = Color.FromArgb(255, 238, 194);
            ShowTip = true;
        }

        /// <summary>
        /// ����ʵ��
        /// <param name="owner">����ӵ���ߣ�֮������Ҫ������ԣ�����Ϊ
        /// ��������õĻ����㡰������ɫ�������Ի���ʱ���������ʧȥ����</param>
        /// </summary>
        public static ColorFrom Instance(IWin32Window owner, int BoxSize)
        {
            instance = new ColorFrom(owner, BoxSize);
            return instance;
        }

        #endregion

        #region ����λ��

        /// <summary>
        /// ���㴰���С
        /// </summary>
        private void CalcWindowSize()
        {
            int width = BOX_SIZE * 8 + 2 * MARGIN;
            int height = BOX_SIZE * 5 + BOX_SIZE + 2 * MARGIN;
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// ���Ʒ���ߴ�
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Rectangle CalcItemBound(int index)
        {
            if (index == 40)
            {
                return Rectangle.FromLTRB(MARGIN, MARGIN + BOX_SIZE * 5, Width / 2 - MARGIN, Height - MARGIN);
            }
            else if (index == 41)
            {
                return Rectangle.FromLTRB(MARGIN * 2 + Width / 2, MARGIN + BOX_SIZE * 5, Width - MARGIN, Height - MARGIN);
            }
            else if (index >= 0 || index <= 39)
            {
                return new Rectangle((index % 8) * BOX_SIZE + MARGIN, index / 8 * BOX_SIZE + MARGIN, BOX_SIZE, BOX_SIZE);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// �����������õ�����µ���ɫ��
        /// </summary>
        /// <param name="point"></param>
        /// <returns>��ɫ������, -1 ��δѡ���κ���ɫ��</returns>
        private int GetIndexFromPoint(Point point)
        {
            //�ڱ߿���
            if (point.X <= MARGIN || point.X >= Width - MARGIN || point.Y <= MARGIN || point.Y >= Height - MARGIN)
            {
                return -1;
            }

            int x = (point.X - MARGIN) / BOX_SIZE;
            int y = (point.Y - MARGIN) / BOX_SIZE;
         
            if (y > 4 && x < 4)
            {
                return 40;         //������ɫ
            }
            else if (y > 4 && x >= 4)
            {
                return 41;         //�رմ���
            }
            else if (y * 8 + x >= 0 && y * 8 + x <= 39)
            {
                return y * 8 + x;
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region ����/����

        /// <summary>
        /// ��ʾ��ɫ������
        /// </summary>
        /// <param name="pt">λ��</param>
        public DialogResult ShowColor(Point pt)
        {
            Location = pt;
            this.Focus();
            return ShowDialog(owner);
        }

        /// <summary>
        /// ��ʾ��ɫ������
        /// </summary>
        /// <param name="topLevelCtl">��������</param>
        /// <param name="ctl">Ҫ������ɫѡ���Ŀؼ�</param>
        public DialogResult ShowColor(Control TopLevelCtl, Control ctl)
        {
            this.Focus();
            Point pt = new Point(ctl.Location.X, ctl.Location.Y + ctl.Height);
            return ShowColor(TopLevelCtl.PointToScreen(pt));
        }

        /// <summary>
        /// ѡ����ɫ
        /// </summary>
        /// <param name="clr">��ɫֵ</param>
        private void SelectColor(Color clr)
        {
            ResultColor = clr;
            if (SelectedColorChanged != null)
            {
                SelectedColorChanged(ResultColor);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        #endregion

        #region ����ɫ��

        protected void DrawItem(DrawItemEventArgs e)
        {
            Rectangle rect = e.Bounds;
            rect.Inflate(-1, -1);
            e.DrawBackground();
            if ((e.State & DrawItemState.HotLight) != 0)
            {
                e.Graphics.FillRectangle(hoverBrush, rect);
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
            if (e.Index == 40) //������ɫ
            {
                e.Graphics.DrawString("Other Color...", Font, Brushes.Black, e.Bounds, sf);
            }
            else if (e.Index == 41) //������ɫ
            {
                e.Graphics.DrawString("Close", Font, Brushes.Black, e.Bounds, sf);
            }
            else
            {
                Rectangle item = e.Bounds;
                item.Inflate(-ITEM_MARGIN, -ITEM_MARGIN);
                using (Brush bru = new SolidBrush(ColorTable[e.Index].ItemColor))
                {
                    e.Graphics.FillRectangle(bru, item);
                }
                e.Graphics.DrawRectangle(Pens.Gray, item);
            }
        }

        private void HWColorPicker_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            for (int i = 0; i < 42; i++)
            {
                DrawItemEventArgs arg = new DrawItemEventArgs(e.Graphics, Font, CalcItemBound(i), i, DrawItemState.Default, ForeColor, BackColor);
                DrawItem(arg);
            }
        }

        #endregion

        #region ����¼�

        private void HWColorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            int index = GetIndexFromPoint(e.Location);

            if (index == hoverItem)
            {
                return;
            }
            //��ǰһ����������
            if (hoverItem != -1)
            {
                DrawItem(new DrawItemEventArgs(g, Font, CalcItemBound(hoverItem), hoverItem, DrawItemState.Default));
            }
               
            //����ǰ����
            if (index != -1 && ShowTip)
            {
                ShowToolTip(index);
                DrawItem(new DrawItemEventArgs(g, Font, CalcItemBound(index), index, DrawItemState.HotLight | DrawItemState.Default));
            }
            g.Dispose();
            hoverItem = index;
        }

        private void HWColorPicker_MouseClick(object sender, MouseEventArgs e)
        {
            int index = GetIndexFromPoint(e.Location);
            if (index == -1)
            {
                return;
            }
            else if (index == 40)
            {
                if (colorDialog.ShowDialog(owner) == DialogResult.OK)     //������ɫ
                {
                    SelectColor(colorDialog.Color);
                }
            }
            else if (index == 41)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Hide();
            }
            else if (index >= 0 && index <= 39)
            {
                SelectColor(colorTable[index].ItemColor);
            }
        }

        #endregion

        #region ��ʾ��ʾ

        private void ShowToolTip(int index)
        {
            if (index == 40)
            {
                tip.SetToolTip(this, "Click to select another color!");
            }
            else if (index == 41)
            {
                tip.SetToolTip(this, "Click Close");
            }
            else if (index >= 0 && index <= 39)
            {
                tip.SetToolTip(this, ColorTable[index].Name);
            }
        }

        #endregion

        #region ��������

        private void HWColorPicker_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion

        #region ��ɫ��

        /// <summary>
        /// ��ʼ����ɫ��
        /// 
        /// </summary>
        protected void InitColor()
        {
            colorTable = new List<ColorItem>();
            colorTable.AddRange(new ColorItem[] { 
                new ColorItem("��ɫ", Color.FromArgb(0,0,0)),
                new ColorItem("��ɫ", Color.FromArgb(153,51,0)),
                new ColorItem("���ɫ", Color.FromArgb(51,51,0)),
                new ColorItem("����", Color.FromArgb(0,51,0)),
                new ColorItem("����", Color.FromArgb(0,51,102)),
                new ColorItem("����", Color.FromArgb(0,0,128)),
                new ColorItem("����", Color.FromArgb(51,51,153)),
                new ColorItem("��ɫ-80%", Color.FromArgb(51,51,51)),

                new ColorItem("���", Color.FromArgb(128,0,0)),
                new ColorItem("��ɫ", Color.FromArgb(255,102,0)),
                new ColorItem("���", Color.FromArgb(128,128,0)),
                new ColorItem("��ɫ", Color.FromArgb(0,128,0)),
                new ColorItem("��ɫ", Color.FromArgb(0,128,128)),
                new ColorItem("��ɫ", Color.FromArgb(0,0,255)),
                new ColorItem("����", Color.FromArgb(102,102,153)),
                new ColorItem("��ɫ-50%", Color.FromArgb(128,128,128)),

                new ColorItem("��ɫ", Color.FromArgb(255,0,0)),
                new ColorItem("ǳ��", Color.FromArgb(255,153,0)),
                new ColorItem("���", Color.FromArgb(153,204,0)),
                new ColorItem("����", Color.FromArgb(51,153,102)),
                new ColorItem("ˮ��", Color.FromArgb(51,204,204)),
                new ColorItem("ǳ��", Color.FromArgb(51,102,255)),
                new ColorItem("������", Color.FromArgb(128,0,128)),
                new ColorItem("��ɫ-40%", Color.FromArgb(153,153,153)),

                new ColorItem("�ۺ�", Color.FromArgb(255,0,255)),
                new ColorItem("��ɫ", Color.FromArgb(255,104,0)),
                new ColorItem("��ɫ", Color.FromArgb(255,255,0)),
                new ColorItem("����", Color.FromArgb(0,255,0)),
                new ColorItem("����", Color.FromArgb(0,255,255)),
                new ColorItem("����", Color.FromArgb(0,204,255)),
                new ColorItem("÷��", Color.FromArgb(153,51,102)),
                new ColorItem("��ɫ-25%", Color.FromArgb(192,192,192)),

                new ColorItem("õ���", Color.FromArgb(255,153,204)),
                new ColorItem("��ɫ", Color.FromArgb(255,204,153)),
                new ColorItem("ǳ��", Color.FromArgb(255,255,204)),
                new ColorItem("ǳ��", Color.FromArgb(204,255,204)),
                new ColorItem("ǳ����", Color.FromArgb(204,255,255)),
                new ColorItem("����", Color.FromArgb(153,204,255)),
                new ColorItem("����", Color.FromArgb(204,153,255)),
                new ColorItem("��ɫ", Color.FromArgb(255,255,255)),

            });
        }
        #endregion
    }
}