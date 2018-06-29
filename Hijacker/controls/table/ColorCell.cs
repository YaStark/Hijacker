using System;
using System.Drawing;

namespace Hijacker.controls.table
{
    public class ColorCell : StringCell
    {
        private readonly Color m_backColor;
        private readonly Color m_foreColor;
        public ColorCell(string value, Color backColor, Color foreColor, Action clickAction = null)
            : base(value, clickAction)
        {
            m_backColor = backColor;
            m_foreColor = foreColor;
        }

        public override void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
            using (var brush = new SolidBrush(m_backColor))
            {
                g.FillRectangle(brush, bounds);                
            }
            base.Paint(g, font, m_foreColor, backColor, bounds);
        }
    }
}