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
        #region 变量

        #region 属性变量
        private bool showTip;
        private Color hoverBKColor;
        private List<ColorItem> colorTable;
        #endregion
        /// <summary>
        /// 块大小
        /// </summary>
        private int BOX_SIZE = 30;
        /// <summary>
        /// 边距
        /// </summary>
        private int MARGIN = 3;
        /// <summary>
        /// 颜色块边距
        /// </summary>
        private int ITEM_MARGIN = 3;
        /// <summary>
        /// 单件实体
        /// </summary>
        private static ColorFrom instance;
        /// <summary>
        /// 字体格式
        /// </summary>
        private StringFormat sf;
        /// <summary>
        /// 鼠标下的颜色块索引
        /// </summary>
        private int hoverItem = -1;
        /// <summary>
        /// 父窗体
        /// </summary>
        private IWin32Window owner;
        /// <summary>
        /// 高亮刷
        /// </summary>
        private Brush hoverBrush;

        #endregion

        #region 事件
        /// <summary>
        /// 颜色更改委托
        /// </summary>
        /// <param name="color"></param>
        public delegate void SelectedChange(Color color);
        /// <summary>
        /// 颜色被选中
        /// </summary>
        public event SelectedChange SelectedColorChanged;

        #endregion

        #region 对外属性

        /// <summary>
        /// 选中的颜色
        /// </summary>
        public Color ResultColor { get; private set; }

        [Description("是否显示颜色提示")]
        [DefaultValue(true)]
        public bool ShowTip
        {
            get { return showTip; }
            set { showTip = value; }
        }

        [Description("高亮背景色")]
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
        /// 颜色表
        /// </summary>
        internal List<ColorItem> ColorTable
        {
            get { return colorTable; }
        }
        #endregion

        #region 初始化

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
        /// 单件实体
        /// <param name="owner">窗体拥用者，之所有需要这个属性，是因为
        /// 如果不设置的话，点“其他颜色”弹出对话框时，主窗体会失去焦点</param>
        /// </summary>
        public static ColorFrom Instance(IWin32Window owner, int BoxSize)
        {
            instance = new ColorFrom(owner, BoxSize);
            return instance;
        }

        #endregion

        #region 计算位置

        /// <summary>
        /// 计算窗体大小
        /// </summary>
        private void CalcWindowSize()
        {
            int width = BOX_SIZE * 8 + 2 * MARGIN;
            int height = BOX_SIZE * 5 + BOX_SIZE + 2 * MARGIN;
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// 绘制方块尺寸
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
        /// 根据鼠标坐标得到鼠标下的颜色块
        /// </summary>
        /// <param name="point"></param>
        /// <returns>颜色块索引, -1 则未选中任何颜色块</returns>
        private int GetIndexFromPoint(Point point)
        {
            //在边框外
            if (point.X <= MARGIN || point.X >= Width - MARGIN || point.Y <= MARGIN || point.Y >= Height - MARGIN)
            {
                return -1;
            }

            int x = (point.X - MARGIN) / BOX_SIZE;
            int y = (point.Y - MARGIN) / BOX_SIZE;
         
            if (y > 4 && x < 4)
            {
                return 40;         //其它颜色
            }
            else if (y > 4 && x >= 4)
            {
                return 41;         //关闭窗体
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

        #region 调用/返回

        /// <summary>
        /// 显示颜色下拉框
        /// </summary>
        /// <param name="pt">位置</param>
        public DialogResult ShowColor(Point pt)
        {
            Location = pt;
            this.Focus();
            return ShowDialog(owner);
        }

        /// <summary>
        /// 显示颜色下拉框
        /// </summary>
        /// <param name="topLevelCtl">顶级窗体</param>
        /// <param name="ctl">要调用颜色选择框的控件</param>
        public DialogResult ShowColor(Control TopLevelCtl, Control ctl)
        {
            this.Focus();
            Point pt = new Point(ctl.Location.X, ctl.Location.Y + ctl.Height);
            return ShowColor(TopLevelCtl.PointToScreen(pt));
        }

        /// <summary>
        /// 选中颜色
        /// </summary>
        /// <param name="clr">颜色值</param>
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

        #region 画颜色块

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
            if (e.Index == 40) //其它颜色
            {
                e.Graphics.DrawString("Other Color...", Font, Brushes.Black, e.Bounds, sf);
            }
            else if (e.Index == 41) //其它颜色
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

        #region 鼠标事件

        private void HWColorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            int index = GetIndexFromPoint(e.Location);

            if (index == hoverItem)
            {
                return;
            }
            //把前一个高亮擦掉
            if (hoverItem != -1)
            {
                DrawItem(new DrawItemEventArgs(g, Font, CalcItemBound(hoverItem), hoverItem, DrawItemState.Default));
            }
               
            //画当前高亮
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
                if (colorDialog.ShowDialog(owner) == DialogResult.OK)     //其他颜色
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

        #region 显示提示

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

        #region 隐藏自身

        private void HWColorPicker_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        #endregion

        #region 颜色表

        /// <summary>
        /// 初始化颜色表
        /// 
        /// </summary>
        protected void InitColor()
        {
            colorTable = new List<ColorItem>();
            colorTable.AddRange(new ColorItem[] { 
                new ColorItem("黑色", Color.FromArgb(0,0,0)),
                new ColorItem("褐色", Color.FromArgb(153,51,0)),
                new ColorItem("橄榄色", Color.FromArgb(51,51,0)),
                new ColorItem("深绿", Color.FromArgb(0,51,0)),
                new ColorItem("深青", Color.FromArgb(0,51,102)),
                new ColorItem("深蓝", Color.FromArgb(0,0,128)),
                new ColorItem("靛蓝", Color.FromArgb(51,51,153)),
                new ColorItem("灰色-80%", Color.FromArgb(51,51,51)),

                new ColorItem("深红", Color.FromArgb(128,0,0)),
                new ColorItem("橙色", Color.FromArgb(255,102,0)),
                new ColorItem("深黄", Color.FromArgb(128,128,0)),
                new ColorItem("绿色", Color.FromArgb(0,128,0)),
                new ColorItem("青色", Color.FromArgb(0,128,128)),
                new ColorItem("蓝色", Color.FromArgb(0,0,255)),
                new ColorItem("蓝灰", Color.FromArgb(102,102,153)),
                new ColorItem("灰色-50%", Color.FromArgb(128,128,128)),

                new ColorItem("红色", Color.FromArgb(255,0,0)),
                new ColorItem("浅橙", Color.FromArgb(255,153,0)),
                new ColorItem("酸橙", Color.FromArgb(153,204,0)),
                new ColorItem("海绿", Color.FromArgb(51,153,102)),
                new ColorItem("水绿", Color.FromArgb(51,204,204)),
                new ColorItem("浅蓝", Color.FromArgb(51,102,255)),
                new ColorItem("紫罗兰", Color.FromArgb(128,0,128)),
                new ColorItem("灰色-40%", Color.FromArgb(153,153,153)),

                new ColorItem("粉红", Color.FromArgb(255,0,255)),
                new ColorItem("金色", Color.FromArgb(255,104,0)),
                new ColorItem("黄色", Color.FromArgb(255,255,0)),
                new ColorItem("鲜绿", Color.FromArgb(0,255,0)),
                new ColorItem("青绿", Color.FromArgb(0,255,255)),
                new ColorItem("天蓝", Color.FromArgb(0,204,255)),
                new ColorItem("梅红", Color.FromArgb(153,51,102)),
                new ColorItem("灰色-25%", Color.FromArgb(192,192,192)),

                new ColorItem("玫瑰红", Color.FromArgb(255,153,204)),
                new ColorItem("茶色", Color.FromArgb(255,204,153)),
                new ColorItem("浅黄", Color.FromArgb(255,255,204)),
                new ColorItem("浅绿", Color.FromArgb(204,255,204)),
                new ColorItem("浅青绿", Color.FromArgb(204,255,255)),
                new ColorItem("淡蓝", Color.FromArgb(153,204,255)),
                new ColorItem("淡紫", Color.FromArgb(204,153,255)),
                new ColorItem("白色", Color.FromArgb(255,255,255)),

            });
        }
        #endregion
    }
}