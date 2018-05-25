using System;
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
        private const string c_location = @"C:\view";
        private Settings m_settings;
        private ITreeItem[] m_slns;

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
            RefreshData();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Settings.Set(m_slns);
        }

        private void RefreshData()
        {
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

        private void Refresh(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void OnDataLoad(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            m_slns = TreeBuilder.Build(c_location, (x, s) => backgroundWorker.ReportProgress((int) ((x ?? -0.01)*100), s));
            m_settings = Settings.Get();
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
    }
}
