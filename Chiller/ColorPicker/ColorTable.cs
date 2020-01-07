using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ColorPicker
{
    internal class ColorItem
    {
        private string name;
        private Color color;

        public ColorItem(string name, Color clr)
        {
            Name = name;
            ItemColor = clr;
        }

        /// <summary>
        /// ÑÕÉ«
        /// </summary>
        public Color ItemColor
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Ãû³Æ
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
