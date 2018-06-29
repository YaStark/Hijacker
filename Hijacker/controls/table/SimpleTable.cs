using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hijacker.controls.table
{
    public partial class SimpleTable : UserControl
    {
        private SimpleTableModel m_model;
        private const int c_headerRowHeight = 20;
        private const int c_rowHeight = 26;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public SimpleTableModel Model
        {
            get { return m_model; }
            set
            {
                if (m_model == value) return;
                if (m_model != null) m_model.RequestUpdate -= ModelOnRequestUpdate;
                m_model = value;
                if (m_model != null) m_model.RequestUpdate += ModelOnRequestUpdate;                
            }
        }

        public SimpleTable()
        {
            InitializeComponent();
            ResizeRedraw = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Model == null)
            {
                return;
            }
            int[] columnWidths = GetColumnWidths();
            Point loc = e.Location;
            int cumulatedWidth = 0;
            int col = 0;
            for (; col < columnWidths.Length; col++)
            {
                cumulatedWidth += columnWidths[col];
                if (cumulatedWidth > loc.X) break;
            }
            int row = loc.Y < c_headerRowHeight
                ? 0
                : ((loc.Y - c_headerRowHeight)/c_rowHeight + 1);
            if (row < Model.RowCount && col < Model.ColumnCount)
            {
                var cell = Model.GetCell(row, col);
                if(cell != null) cell.Click();
            }
        }

        public void RefreshView()
        {
            if (Model != null)
            {
                Height = GetColumnWidths().Sum() + 2;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Model == null) return;
            Model.SuspendRedraw = true;
            int offsetY = 0;
            int rowHeight = c_headerRowHeight;
            int[] colWidths = GetColumnWidths();
            for (int row = 0; row < Model.RowCount; row++)
            {
                int offsetX = 0;
                for (int col = 0; col < Model.ColumnCount; col++)
                {
                    var cell = Model.GetCell(row, col);
                    if (cell != null)
                    {
                        cell.Paint(e.Graphics, Font, ForeColor, BackColor, new Rectangle(offsetX, offsetY, colWidths[col], rowHeight));
                    }
                    offsetX += colWidths[col];
                }
                offsetY += rowHeight;
                rowHeight = c_rowHeight;
            }
            Model.SuspendRedraw = false;
        }

        private int[] GetColumnWidths()
        {
            var styles = Model.GetColumnStyles();
            if (styles == null || !styles.Any())
            {
                return new int[0];
            }
            float absWidth = styles.Where(x => x.SizeType == SizeType.Absolute).Sum(x => x.Width);
            float relWidth = styles.Where(x => x.SizeType == SizeType.Percent).Sum(x => x.Width);
            float deltaWidth = Math.Max(Width - absWidth, 1) / Math.Max(relWidth, 1);
            float minColumnWidth = 50;
            return Model.GetColumnStyles().Select(x => x.SizeType == SizeType.Absolute
                    ? (int) x.Width
                    : (int) Math.Max(minColumnWidth, x.Width*deltaWidth)
            ).ToArray();
        }

        private void ModelOnRequestUpdate(object sender, EventArgs eventArgs)
        {
            RefreshView();
        }
    }
}
