using System;
using System.Drawing;

namespace Hijacker.controls.table
{
    public class ButtonCell : StringCell
    {
        private static readonly Color s_disabledBackColor = Color.Bisque;
        private static readonly Color s_disabledForeColor = Color.SandyBrown;
        private static readonly Color s_enabledBackColor = Color.BurlyWood;
        private static readonly Color s_enabledForeColor = Color.DarkRed;

        public Color BackColor { get; set; }
        private Color m_foreColor = s_enabledForeColor;
        private bool m_enabled = true;

        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                m_enabled = value;
                ResetBackColor();
            }
        }

        public ButtonCell(string text, Action clickAction)
            : base(text, clickAction)
        {
            BackColor = s_enabledBackColor;
        }

        public override void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
            var innerBounds = new Rectangle(bounds.X + 3, bounds.Y + 3, bounds.Width - 6, bounds.Height - 6);
            using (var pen = new Pen(m_foreColor))
            using (var brush = new SolidBrush(BackColor))
            {
                g.FillRectangle(brush, innerBounds);
                g.DrawRectangle(pen, innerBounds);
            }
            if (Value == null) return;
            var strVal = Value.ToString();
            if (!string.IsNullOrEmpty(strVal))
            {
                using (var brush = new SolidBrush(m_foreColor))
                {
                    g.DrawString(strVal, font, brush, innerBounds, StringFormat);
                }
            }
        }

        public override void Click()
        {
            if (m_enabled)
            {
                base.Click();
            }
        }

        public void ResetBackColor()
        {
            BackColor = Enabled ? s_enabledBackColor : s_disabledBackColor;
            m_foreColor = Enabled ? s_enabledForeColor : s_disabledForeColor;
        }
    }
}