using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Hijacker.ext;
using Hijacker.model;
using Hijacker.Properties;

namespace Hijacker.controls
{
    public partial class HijackItemsTable : SimpleTable
    {
        public HijackItemsTable()
        {
            InitializeComponent();
            Model = new HijackItemsTableModel();
            RefreshView();
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            ((HijackItemsTableModel) Model).UpdateLabels();
        }
    }

    class HijackItem
    {
        public ProjectItem From { get; set; }
        public SolutionItem To { get; set; }

        public Control LabelFrom { get; set; }
        public Control LabelTo { get; set; }
        public Button ApplyButton { get; set; }

        private string FromPath { get; set; }
        private string ToPath { get; set; }
        public PictureBox LabelFromStatus { get; set; }
        public PictureBox LabelToStatus { get; set; }

        public void Update()
        {
            if (From != null)
            {
                FromPath = PathEx.ApplyAllUpDirPaths(From.DllPath);
            }
            if (FromPath != null && To != null)
            {
                ProjectDescription descr =
                    To.Dependencies.First(
                        x =>
                            string.Equals(x.FileName, Path.GetFileNameWithoutExtension(FromPath),
                            StringComparison.CurrentCultureIgnoreCase));
                ToPath = PathEx.ApplyAllUpDirPaths(descr.Path);
            }
            if (LabelFrom != null)
            {
                LabelFrom.Text = FromPath == null ? string.Empty : Path.GetFileName(FromPath);
            }
            if (LabelTo != null)
            {
                LabelTo.Text = ToPath;
            }
            bool fromOk = FromPath != null && File.Exists(FromPath);
            ApplyButton.Enabled = fromOk && ToPath != null;
            LabelFromStatus.BackgroundImage = fromOk ? Resources.Ok : Resources.Error;
            LabelToStatus.BackgroundImage = ToPath != null ? Resources.Ok : Resources.Error;
        }

        public void Action()
        {
            File.Copy(FromPath, ToPath, true);
            File.Copy(FromPath.Replace(".dll", ".pdb"), ToPath.Replace(".dll", ".pdb"), true);
        }

        private static OpenFileDialog CreateOpenFileDialog(string path)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Library (*.dll)|*.dll|All files (*)|*"
            };
            if (path != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(path);
                dialog.FileName = Path.GetFileName(path);
            }
            return dialog;
        }

        public void ResolveFrom()
        {
            var dialog = CreateOpenFileDialog(FromPath);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FromPath = dialog.FileName;
                From = null;
            }
            Update();
        }

        public void ResolveTo()
        {
            var dialog = CreateOpenFileDialog(ToPath);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ToPath = dialog.FileName;
                To = null;
            }
            Update();
        }
    }

    class HijackItemsTableModel : SimpleTableModel
    {
        private static readonly string[] m_titles = {"From", "", "To", "", "", ""};
        private readonly Dictionary<int, HijackItem> m_items = new Dictionary<int, HijackItem>();

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

        protected override Control GetCellContent(int row, int column)
        {
            if (row == 0)
            {
                return HeaderLabel(m_titles[column]);
            }
            if (row == RowCount - 1)
            {
                return column == 0 ? RequestAddRowLabel() : null;
            }
            if (!m_items.ContainsKey(row))
            {
                m_items.Add(row, new HijackItem());
            }
            switch (column)
            {
                case 0: // from
                    var labelFrom = ClickLabel(string.Empty, SetFrom, row);
                    m_items[row].LabelFrom = labelFrom;
                    return labelFrom;
                case 1: // from status
                    var pictureBoxFrom = Pic();
                    pictureBoxFrom.Click += (s, e) => m_items[row].ResolveFrom();
                    m_items[row].LabelFromStatus = pictureBoxFrom;
                    return pictureBoxFrom;
                case 2: // to
                    var labelTo = ClickLabel(string.Empty, SetTo, row);
                    m_items[row].LabelTo = labelTo;
                    return labelTo;
                case 3: // to status
                    var pictureBox = Pic();
                    pictureBox.Click += (s, e) => m_items[row].ResolveTo();
                    m_items[row].LabelToStatus = pictureBox;
                    return pictureBox;
                case 4: // apply
                    var btn = Btn("Hijack", Action, row);
                    m_items[row].ApplyButton = btn;
                    m_items[row].Update();
                    return btn;
                case 5: // clear
                    return Btn("X", Clear, row);
            }
            return base.GetCellContent(row, column);
        }

        private void SetFrom(int row)
        {
            m_items[row].From = From;
            m_items[row].Update();
        }

        private void SetTo(int row)
        {
            m_items[row].To = To;
            m_items[row].Update();
        }

        private Control RequestAddRowLabel()
        {
            var label = ReadOnlyLabel("+ Add hijack target");
            label.Click += RequestAddRow;
            return label;
        }

        private void RequestAddRow(object sender, EventArgs e)
        {
            RowCount++;
            ForceUpdate = true;
            Update();
        }

        public void UpdateLabels()
        {
            foreach (var kv in m_items)
            {
                kv.Value.Update();
            }
        }

        private PictureBox Pic()
        {
            var pictureBox = Control<PictureBox>();
            pictureBox.BackgroundImage = Resources.Error;
            pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox.Margin = new Padding(1);
            return pictureBox;
        }

        private Label ClickLabel(string text, Action<int> clickAction, int row)
        {
            var label = EditLabel(text);
            label.Margin = new Padding(1);
            label.Click += (s, e) => clickAction(row);
            return label;
        }

        private Button Btn(string text, Action<int> clickAction, int row)
        {
            var btn = Control<Button>();
            btn.Margin = new Padding(1);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Text = text;
            btn.Tag = row;
            btn.Click += (s, e) => clickAction(row);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            return btn;
        }

        private void Action(int row)
        {
            m_items[row].Action();
        }

        private void Clear(int row)
        {
            m_items.Remove(row);
            RowCount--;
            ForceUpdate = true;
            Update();
            ForceUpdate = false;
        }
    }
}
