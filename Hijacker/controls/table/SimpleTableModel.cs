using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hijacker.controls.table
{
    public class SimpleTableModel
    {
        public event EventHandler RequestUpdate;

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public bool SuspendRedraw { get; set; }

        private Dictionary<string, GridCell> m_cells = new Dictionary<string, GridCell>();

        public virtual IEnumerable<ColumnStyle> GetColumnStyles()
        {
            yield break;
        }

        public GridCell GetCell(int row, int column)
        {
            return GetCellContent(row, column);
        }

        protected virtual GridCell GetCellContent(int row, int column)
        {
            return null;
        }

        public virtual void Update()
        {
            EventHandler handler = RequestUpdate;
            if (!SuspendRedraw && handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region Control factory

        protected GridCell HeaderLabel(string text, Action clickAction = null)
        {
            return new ColorCell(text, Color.Black, Color.White, clickAction);
        }

        protected GridCell ReadOnlyLabel(string text, Action clickAction = null)
        {
            return new StringCell(text, clickAction);
        }

        protected GridCell EditLabel(string text, Action clickAction)
        {
            return new EditCell(text, Color.White, Color.DarkGoldenrod, clickAction);
        }

        #endregion Control factory
    }
}