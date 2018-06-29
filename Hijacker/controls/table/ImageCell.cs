using System;
using System.Drawing;

namespace Hijacker.controls.table
{
    public class ImageCell : GridCell
    {
        private readonly Action m_clickAction;

        public ImageCell(Image img, Action clickAction) : base(img)
        {
            m_clickAction = clickAction;
        }

        public override void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
            var image = Value as Image;
            if (image != null)
            {
                Point location = bounds.Location + new Size(bounds.Width/2, bounds.Height/2) -
                                 new Size(image.Width/2, image.Height/2);
                g.DrawImageUnscaled(image, location);
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