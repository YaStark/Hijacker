using System;
using System.Drawing;

namespace Hijacker.controls.table
{
    public class EditCell : StringCell
    {
        private readonly Color m_backColor;
        private readonly Color m_foreColor;
        public EditCell(string value, Color backColor, Color foreColor, Action clickAction = null)
            : base(value, clickAction)
        {
            m_backColor = backColor;
            m_foreColor = foreColor;
        }

        public override void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
            var innerBounds = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
            using (var brush = new SolidBrush(m_backColor))
            {
                g.FillRectangle(brush, innerBounds);
                g.DrawRectangle(Pens.Black, innerBounds);
            }
            base.Paint(g, font, m_foreColor, backColor, innerBounds);
        }
    }
}