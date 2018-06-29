using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Hijacker.controls;
using Hijacker.model;

namespace Hijacker
{
    public partial class Form1 : Form
    {
        private Settings m_settings;
        private ITreeItem[] m_slns;

        private List<string> m_locations = new List<string>();
        private string m_progressText = "Loading...";
        private readonly HijackItemsTableModel m_hijackModel;

        public Form1()
        {
            InitializeComponent();
            m_hijackModel = (HijackItemsTableModel)hijackItemsTable1.Model;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_settings = Settings.Get();
            m_locations.Clear();
            m_locations.AddRange(m_settings.VobPaths ?? Enumerable.Empty<string>());
            RefreshData();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Settings.Set(m_slns, m_locations.ToArray());
        }

        private void RefreshData()
        {
            if (m_slns != null)
            {
                foreach (var sln in m_slns.SelectMany(x => x.Children).OfType<SolutionItem>())
                {
                    sln.Close();
                }
            }
            progressBar.Value = 0;
            m_progressText = "Loading...";
            panelProgress.BringToFront();
            backgroundWorker.RunWorkerAsync();
        }

        private void OnTreeSelectionChanged(object sender, ITreeItem e)
        {
            var from = e as ProjectItem;
            if (from != null)
            {
                if (m_hijackModel.To != null && !m_hijackModel.To.IsInDependencies(from))
                {
                    m_hijackModel.To = null;
                }
                m_hijackModel.From = from;
                lblSelected.Text = Path.GetFileName(m_hijackModel.From.DllPath);
            }
            solutionTree1.Color(m_hijackModel.From != null
                ? m_slns.SelectMany(x => x.Children)
                    .OfType<SolutionItem>()
                    .Where(x => x.IsInDependencies(m_hijackModel.From))
                    .ToArray()
                : null);

            var to = e as SolutionItem;
            if (to != null && (m_hijackModel.From == null || to.Dependencies.Any(x => x.FileName == Path.GetFileNameWithoutExtension(m_hijackModel.From.DllPath))))
            {
                m_hijackModel.To = to;
            }
            m_hijackModel.UpdateLabels();
        }

        private void OnIconClick(object sender, MouseEventArgs e)
        {
            Visible = !Visible;
            if (Visible)
            {
                Focus();
            }
        }
        
        private void OnDataLoad(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            m_slns = TreeBuilder.Build(m_locations.ToArray(), (x, s) => backgroundWorker.ReportProgress((int) ((x ?? -0.01)*100), s));
        }

        private void OnDataProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                m_progressText = e.UserState.ToString();
                Refresh();
            }
            if (e.ProgressPercentage > 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }
        }

        private void OnDataLoaded(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            solutionTree1.SetModel(m_slns);
            if (m_settings != null)
            {
                m_settings.Apply(m_slns);
            }
            panelData.BringToFront();
            Refresh();
        }

        private void OnProgressPaint(object sender, PaintEventArgs e)
        {
            var rect = new RectangleF(0, 0, Width, Height);
            var format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            e.Graphics.FillRectangle(Brushes.LightGray, rect);
            e.Graphics.DrawString(m_progressText, Font, Brushes.Black, rect, format);
        }

        private void OnResetSelectedDll(object sender, EventArgs e)
        {
            m_hijackModel.From = null;
            solutionTree1.Color(null);
            m_hijackModel.UpdateLabels();
            lblSelected.Text = Path.GetFileName("(Empty selection)");
        }

        private void OnChooseLocations(object sender, EventArgs e)
        {
            using (var form = new LocationsForm())
            {
                form.Locations = m_locations.ToArray();
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    m_locations.Clear();
                    m_locations.AddRange(form.Locations);
                }
            }
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
