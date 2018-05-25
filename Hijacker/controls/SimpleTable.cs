using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hijacker.controls
{
    public partial class SimpleTable : UserControl
    {
        private SimpleTableModel m_model;

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
        }

        public void RefreshView()
        {
            table.RowCount = 0;
            table.ColumnCount = 0;
            table.RowStyles.Clear();
            table.ColumnStyles.Clear();
            table.Controls.Clear();
            if (Model == null)
            {
                return;
            }
            foreach (ColumnStyle columnStyle in Model.GetColumnStyles())
            {
                table.ColumnStyles.Add(columnStyle);
            }
            for (int row = 0; row < Model.RowCount; row++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, row == 0 ? 20 : 26));
                for (int col = 0; col < Model.ColumnCount; col++)
                {
                    var ctrl = Model.GetCell(row, col);
                    if (ctrl != null)
                    {
                        table.Controls.Add(ctrl, col, row);
                    }
                }
            }
            Height = (int)table.RowStyles.Cast<RowStyle>().Sum(x => x.Height) + 2;
        }

        private void ModelOnRequestUpdate(object sender, EventArgs eventArgs)
        {
            RefreshView();
        }
    }

    public class SimpleTableModel
    {
        public event EventHandler RequestUpdate;

        public bool ForceUpdate { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        private readonly Dictionary<string, Control> m_controls = new Dictionary<string, Control>();

        public virtual IEnumerable<ColumnStyle> GetColumnStyles()
        {
            yield break;
        }

        public Control GetCell(int row, int column)
        {
            Control ctrl;
            string key = MakeKey(row, column);
            if (!m_controls.TryGetValue(key, out ctrl) || ForceUpdate)
            {
                m_controls[key] = GetCellContent(row, column);
            }
            return m_controls[key];
        }

        protected virtual Control GetCellContent(int row, int column)
        {
            return null;
        }

        protected virtual void Update()
        {
            EventHandler handler = RequestUpdate;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            ForceUpdate = false;
        }

        private static string MakeKey(int row, int col)
        {
            return row + ":" + col;
        }

        #region Control factory

        protected TControl Control<TControl>() where TControl : Control, new()
        {
            return new TControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
        }

        protected Label HeaderLabel(string text)
        {
            var label = ReadOnlyLabel(text);
            label.BackColor = Color.Black;
            label.ForeColor = Color.White;
            return label;
        }

        protected Label ReadOnlyLabel(string text)
        {
            var label = Control<Label>();
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSize = false;
            label.Text = text;
            label.AutoEllipsis = true;
            return label;
        }
        
        protected Label EditLabel(string text)
        {
            var label = ReadOnlyLabel(text);
            label.BorderStyle = BorderStyle.FixedSingle;
            label.BackColor = Color.White;
            label.ForeColor = Color.DarkBlue;
            return label;
        }

        #endregion Control factory
    }
}
