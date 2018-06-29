using System.Drawing;

namespace Hijacker.controls.table
{
    public abstract class GridCell
    {
        public object Value { get; set; }
        public ContentAlignment Alignment { get; set; }

        protected GridCell(object value)
        {
            Value = value;
        }
        public virtual void Paint(Graphics g, Font font, Color foreColor, Color backColor, Rectangle bounds)
        {
        }
        public virtual void Click()
        {
        }
    }
}