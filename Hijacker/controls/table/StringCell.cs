using System;
using System.Drawing;

namespace Hijacker.controls.table
{
    public class StringCell : GridCell
    {
        protected static readonly StringFormat StringFormat = new StringFormat
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisPath,
            FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit
        };

        private readonly Action m_clickAction;

        public StringCell(string value, Action clickAction = null) : base(value)
        {
            m_clickAction = clickAction;
        }

        public override void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
            if (Value == null) return;
            var strVal = Value.ToString();
            if (!string.IsNullOrEmpty(strVal))
            {
                using (var brush = new SolidBrush(foreColor))
                {
                    g.DrawString(strVal, font, brush, bounds, StringFormat);
                }
            }
        }

        public override void Click()
        {
            if (m_clickAction != null)
            {
                m_clickAction();
            }
        }
    }
}