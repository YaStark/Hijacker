using System.Collections.Generic;
using System.Windows.Forms;
using Hijacker.controls.table;
using Hijacker.model;
using Hijacker.Properties;

namespace Hijacker.controls
{
    class HijackItemsTableModel : SimpleTableModel
    {
        private static readonly string[] m_titles = {"From", "", "To", "", "", ""};
        private readonly List<HijackItem> m_items = new List<HijackItem>();

        public ProjectItem From { get; set; }

        public SolutionItem To { get; set; }

        public HijackItemsTableModel()
        {
            ColumnCount = 6;
            RowCount = 2;
        }

        public override IEnumerable<ColumnStyle> GetColumnStyles()
        {
            yield return new ColumnStyle(SizeType.Percent, 50);
            yield return new ColumnStyle(SizeType.Absolute, 30);
            yield return new ColumnStyle(SizeType.Percent, 50);
            yield return new ColumnStyle(SizeType.Absolute, 30);
            yield return new ColumnStyle(SizeType.Absolute, 80);
            yield return new ColumnStyle(SizeType.Absolute, 32);
        }

        protected override GridCell GetCellContent(int row, int column)
        {
            if (row == 0)
            {
                return HeaderLabel(m_titles[column]);
            }
            if (row == RowCount - 1)
            {
                return column == 0 ? new StringCell("+ Add hijack target", RequestAddRow) : null;
            }
            row--;
            if (row >= m_items.Count)
            {
                return null;
            }
            switch (column)
            {
                case 0: // from
                    return m_items[row].LabelFrom;
                case 1: // from status
                    return m_items[row].LabelFromStatus;
                case 2: // to
                    return m_items[row].LabelTo;
                case 3: // to status
                    return m_items[row].LabelToStatus;
                case 4: // apply
                    return m_items[row].ApplyButton;
                case 5: // clear
                    return m_items[row].RemoveButton;
            }
            return base.GetCellContent(row, column);
        }

        private HijackItem Create()
        {
            var item = new HijackItem(this);
            item.LabelFrom = EditLabel("", () => item.SetFrom(From));
            item.LabelFromStatus = new ImageCell(Resources.Error, () => item.ResolveFrom());
            item.LabelTo = EditLabel("", () => item.SetTo(To));
            item.LabelToStatus = new ImageCell(Resources.Error, () => item.ResolveTo());
            item.ApplyButton = new ButtonCell("Copy", () => item.Action());
            item.RemoveButton = new ImageCell(Resources.Close, () => Remove(item));
            item.Update();
            return item;
        }

        private void Remove(HijackItem item)
        {
            if (m_items.Contains(item))
            {
                m_items.Remove(item);
                RowCount--;
                Update();
            }
        }

        private void RequestAddRow()
        {
            m_items.Add(Create());
            RowCount++;
            Update();
        }

        public void UpdateLabels()
        {
            foreach (var kv in m_items)
            {
                kv.Update();
            }
        }
    }
}