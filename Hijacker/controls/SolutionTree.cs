using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Hijacker.model;

namespace Hijacker.controls
{
    public partial class SolutionTree : UserControl
    {
        private const int c_rowHeight = 19;
        private bool m_requestUpdateModel = true;

        private readonly List<Column> m_columns = new List<Column>();
        private int[] m_columnSizes;
        private ITreeItem[] m_items;
        private readonly Dictionary<ITreeItem, bool> m_collapsed = new Dictionary<ITreeItem, bool>();
        private Dictionary<ITreeItem, Dictionary<string, string>> m_data = new Dictionary<ITreeItem, Dictionary<string, string>>();
        private readonly Pen m_innerLinePen = Pens.LightGray;
        private bool m_requestUpdateSize = true;
        private readonly StringFormat m_format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisPath,
                FormatFlags = StringFormatFlags.NoWrap
            };

        public event EventHandler<ITreeItem> SelectionChanged;

        private static bool m_showAll;
        private ITreeItem[] m_colored;

        public ITreeItem Selected { get; private set; }

        public Column[] Columns
        {
            get { return m_columns.ToArray(); }
            set
            {
                m_columns.Clear();
                if (value != null)
                {
                    m_columns.AddRange(value);
                }
            }
        }

        public SolutionTree()
        {
            InitializeComponent();
        }

        public void SetModel(ITreeItem[] items)
        {
            m_items = items;
            UpdateColumnSizes();
            UpdateModel();
            canvas.Height = (VisibleRowsCount(m_items) + 1) * c_rowHeight;
        }

        public void Color(SolutionItem[] items)
        {
            if (items == null)
            {
                m_colored = null;
                return;
            }
            List<ITreeItem> colored = new List<ITreeItem>();
            foreach (SolutionItem item in items)
            {
                colored.AddRange(ColorUp(item));
            }
            m_colored = colored.Distinct().ToArray();
        }

        private static IEnumerable<ITreeItem> ColorUp(ITreeItem item)
        {
            if (item != null) return new[] { item }.Concat(ColorUp(item.Root));
            return Enumerable.Empty<ITreeItem>();
        }

        #region Drawing

        private void CanvasOnPaint(object sender, PaintEventArgs e)
        {
            if (m_columnSizes == null || m_requestUpdateSize)
            {
                UpdateColumnSizes();
                m_requestUpdateSize = false;
            }
            // headers
            var cellRect = new RectangleF(0, 0, 0, c_rowHeight);
            for (int i = 0; i < m_columns.Count; i++)
            {
                if (m_columns[i].Visible)
                {
                    cellRect.Width = m_columnSizes[i] - 1;
                    string text = String.IsNullOrEmpty(m_columns[i].Title) ? m_columns[i].Property : m_columns[i].Title;
                    e.Graphics.FillRectangle(Brushes.Black, cellRect);
                    e.Graphics.DrawString(text, Font, Brushes.White, cellRect, m_format);
                    cellRect.X += cellRect.Width;
                }
            }

            if (m_items == null) return;
            if (m_requestUpdateModel)
            {
                UpdateModel();
                m_requestUpdateModel = false;
            }
            int row = 1;
            foreach (ITreeItem treeItem in m_items)
            {
                DrawItem(e.Graphics, treeItem, 1, ref row);
            }
        }

        private Brush GetBkBrush(ITreeItem item)
        {
            if (m_colored != null && m_colored.Contains(item))
            {
                if (item == Selected)
                {
                    return Brushes.DarkGoldenrod;
                }
                return Brushes.PaleGoldenrod;
            }
            if (item == Selected)
            {
                return Brushes.LightSteelBlue;
            }
            return null;
        }

        private Brush GetForeBrush(ITreeItem item)
        {
            bool hidden = !item.Visible && m_showAll;
            bool colored = m_colored != null && m_colored.Contains(item);
            bool selected = item == Selected;
            if (hidden)
            {
                return Brushes.DarkMagenta;
            }
            if (colored)
            {
                return Brushes.DarkGreen;
            }
            if (selected)
            {
                return Brushes.DarkSlateGray;
            }
            return Brushes.Black;
        }

        private void DrawItem(Graphics g, ITreeItem item, int lvl, ref int row)
        {
            if (!item.Visible && !m_showAll)
            {
                return;
            }
            DrawRow(g, m_data[item], lvl * c_rowHeight, row, GetForeBrush(item), GetBkBrush(item));
            if (item.Children == null || item.Children.Count == 0)
            {
                row++;
                return;
            }
            DrawBox(g, row, lvl, m_collapsed[item]);
            row++;
            if (m_collapsed[item])
            {
                return;
            }
            foreach (ITreeItem treeItem in item.Children)
            {
                DrawItem(g, treeItem, lvl + 1, ref row);
            }
        }

        private void DrawRow(Graphics g, Dictionary<string, string> values, int offset, int row, Brush foreBrush, Brush bkBrush)
        {
            int top = row * c_rowHeight;
            int bottom = top + c_rowHeight - 1;
            g.DrawLine(m_innerLinePen, 0, bottom, canvas.Width, bottom);
            int x = offset;
            var cellRect = new RectangleF(x, top, 0, c_rowHeight);
            for (int i = 0; i < m_columns.Count; i++)
            {
                if (m_columns[i].Visible)
                {
                    cellRect.Width = i == 0 ? m_columnSizes[i] - offset : m_columnSizes[i];
                    DrawCell(g, values[m_columns[i].Property], cellRect, foreBrush, bkBrush);
                    cellRect.X += cellRect.Width;
                }
            }
        }

        private void DrawCell(Graphics g, string text, RectangleF bounds, Brush foreBrush, Brush bkBrush)
        {
            if (bkBrush != null)
            {
                g.FillRectangle(bkBrush, bounds);
                g.DrawString(text, Font, foreBrush, bounds, m_format);
            }
            else
            {
                g.DrawString(text, Font, foreBrush, bounds, m_format);
            }
        }

        private static void DrawBox(Graphics g, int row, int lvl, bool plus)
        {
            int height = c_rowHeight * 5 / 8;
            int pad = (c_rowHeight - height) / 2;
            var rect = new Rectangle(lvl * c_rowHeight - height - 3, row * c_rowHeight + pad, height, height);
            if (plus)
            {
                g.FillRectangle(Brushes.DarkSlateGray, rect);
            }
            g.DrawRectangle(Pens.Black, rect);
        }

        #endregion Drawing

        private void CanvasOnMouseDown(object sender, MouseEventArgs e)
        {
            int row = e.Y / c_rowHeight - 1;
            int lvl = 0;
            var item = GetItem(m_items, ref row, ref lvl);
            if (item != null)
            {
                if (lvl*c_rowHeight > e.X)
                {
                    m_collapsed[item] = !m_collapsed[item];                        
                }
                Selected = item;
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, Selected);
                }
                if (e.Button == MouseButtons.Right)
                {
                    hideToolStripMenuItem.Checked = !Selected.Visible;
                    contextMenuStrip.Show(Cursor.Position);
                }
                canvas.Refresh();
            }
            canvas.Height = (VisibleRowsCount(m_items) + 1)*c_rowHeight;
        }

        private ITreeItem GetItem(IEnumerable<ITreeItem> items, ref int row, ref int lvl)
        {
            if (items != null)
            {
                foreach (ITreeItem item in items.Where(x => x.Visible || m_showAll))
                {
                    row--;
                    if (!m_collapsed[item])
                    {
                        if (row >= 0)
                        {
                            ITreeItem item0 = GetItem(item.Children, ref row, ref lvl);
                            if (row < 0)
                            {
                                lvl++;
                                return item0;
                            }
                        }
                    }
                    if (row == -1)
                    {
                        lvl = 1;
                        return item;
                    } 
                    if (row < 0)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private int VisibleRowsCount(IEnumerable<ITreeItem> items)
        {
            return items == null ? 0 : items.Where(x => x.Visible || m_showAll).Sum(item => !m_collapsed[item] ? VisibleRowsCount(item.Children) + 1 : 1);
        }

        private void UpdateModel()
        {
            m_data = new Dictionary<ITreeItem, Dictionary<string, string>>();
            Dictionary<string, PropertyInfo> props = m_columns.ToDictionary(x => x.Property,
                x => typeof(ITreeItem).GetProperty(x.Property));
            foreach (ITreeItem item in m_items)
            {
                var valInfo = new Dictionary<string, string>();
                m_data.Add(item, valInfo);
                foreach (Column column in m_columns)
                {
                    var value = props[column.Property].GetValue(item);
                    valInfo.Add(column.Property, value != null ? value.ToString() : String.Empty);
                }
                CollectItemInfo(props, item);
            }
        }

        private void CollectItemInfo(Dictionary<string, PropertyInfo> props, ITreeItem item)
        {
            m_collapsed[item] = true;
            if (item.Children == null) return;
            foreach (ITreeItem childItem in item.Children)
            {
                var valInfo = new Dictionary<string, string>();
                m_data.Add(childItem, valInfo);
                foreach (Column column in m_columns)
                {
                    var value = props[column.Property].GetValue(childItem);
                    valInfo.Add(column.Property, value != null ? value.ToString() : String.Empty);
                }
                CollectItemInfo(props, childItem);
            }
        }

        private void UpdateColumnSizes()
        {
            m_columnSizes = new int[m_columns.Count];
            float relSum = 0;
            for (int i = 0; i < m_columns.Count; i++)
            {
                if (m_columns[i].Visible)
                {
                    if (m_columns[i].Absolute)
                    {
                        m_columnSizes[i] = (int)m_columns[i].Width;
                    }
                    else
                    {
                        relSum += m_columns[i].Width;
                    }                    
                }
            }
            float relWidth = canvas.Width - m_columnSizes.Sum();
            if (relWidth > 0 && relSum > 0)
            {
                relWidth = relWidth/relSum;
                for (int i = 0; i < m_columns.Count; i++)
                {
                    if (m_columns[i].Visible)
                    {
                        if (!m_columns[i].Absolute)
                        {
                            m_columnSizes[i] = (int)(m_columns[i].Width * relWidth);
                        }
                    }
                }
            }
        }

        private void OnLocation(object sender, EventArgs e)
        {
            if (Selected != null)
            {
                Process.Start(Selected.Path);                
            }
        }

        private void OnOpenSln(object sender, EventArgs e)
        {
            if (Selected == null || Selected is FolderItem) return;
            Process.Start(Selected.FilePath);
        }

        private void OnHide(object sender, EventArgs e)
        {
            Selected.Visible = !Selected.Visible;
            canvas.Refresh();
        }

        private void OnShowAll(object sender, EventArgs e)
        {
            m_showAll = !m_showAll;
            canvas.Refresh();
        }
    }
}
