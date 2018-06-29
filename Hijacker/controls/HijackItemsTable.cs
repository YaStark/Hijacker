using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hijacker.controls.table;
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

        public GridCell LabelFrom { get; set; }
        public GridCell LabelTo { get; set; }
        public ButtonCell ApplyButton { get; set; }

        private string FromPath { get; set; }
        private string ToPath { get; set; }
        public ImageCell LabelFromStatus { get; set; }
        public ImageCell LabelToStatus { get; set; }
        public SimpleTableModel Model { get; set; }
        public ImageCell RemoveButton { get; set; }

        public HijackItem(SimpleTableModel model)
        {
            Model = model;
        }

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
                LabelFrom.Value = FromPath == null ? string.Empty : Path.GetFileName(FromPath);
            }
            if (LabelTo != null)
            {
                LabelTo.Value = ToPath;
            }
            bool fromOk = FromPath != null && File.Exists(FromPath);
            ApplyButton.Enabled = fromOk && ToPath != null;
            LabelFromStatus.Value = fromOk ? Resources.Ok : Resources.Error;
            LabelToStatus.Value = ToPath != null ? Resources.Ok : Resources.Error;
            Model.Update();
        }

        public async void Action()
        {
            try
            {
                ApplyButton.Enabled = false;
                Model.Update();
                File.Copy(FromPath, ToPath, true);
                File.Copy(FromPath.Replace(".dll", ".pdb"), ToPath.Replace(".dll", ".pdb"), true);
                await Task.Delay(100);
                ApplyButton.BackColor = Color.LightGreen;
                Model.Update();
                await Task.Delay(200);
            }
            finally
            {
                ApplyButton.ResetBackColor();
                ApplyButton.Enabled = true;
                Model.Update();
            }
        }

        public void ResolveFrom()
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Library (*.dll)|*.dll|All files (*)|*"
            };
            if (FromPath != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(FromPath);
                dialog.FileName = Path.GetFileName(FromPath);
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FromPath = dialog.FileName;
                From = null;
            }
            Update();
        }

        public void ResolveTo()
        {
            var dialog = new SaveFileDialog
            {
                Filter = @"Library (*.dll)|*.dll|All files (*)|*"
            };
            if (ToPath != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(ToPath);
                dialog.FileName = Path.GetFileName(ToPath);
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ToPath = dialog.FileName;
                To = null;
            }
            Update();
        }

        public void SetFrom(ProjectItem from)
        {
            From = from;
            Update();
        }

        public void SetTo(SolutionItem to)
        {
            To = to;
            Update();
        }
    }
}
