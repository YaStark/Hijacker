using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Hijacker.ext;

namespace Hijacker.controls
{
    public partial class LocationsForm : Form
    {
        public string[] Locations 
        {
            get { return listBoxLocations.Items.Cast<string>().ToArray(); }
            set { listBoxLocations.Items.AddRange(value.Cast<object>().ToArray()); }
        }

        public LocationsForm()
        {
            InitializeComponent();
        }

        private void OnChooseCustom(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (!listBoxLocations.Items.Contains(folderBrowserDialog.SelectedPath))
                {
                    listBoxLocations.Items.Add(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void OnChooseCc(object sender, EventArgs e)
        {
            string data = PathEx.Cmd("lsview -host " + Environment.MachineName + " -quick", "cleartool.exe");
            if (data == null) return;

            var paths =
                data.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Substring(x.IndexOf("\\\\", StringComparison.InvariantCulture)).Trim())
                    .Select(x => PathEx.ApplyAllUpDirPaths(x + "\\..\\"))
                    .Distinct()
                    .Where(Directory.Exists).
                    Select(PathEx.ConvertUncPathToPhysicalPath);
            foreach (string path in paths)
            {
                if (!listBoxLocations.Items.Contains(path))
                {
                    listBoxLocations.Items.Add(path);
                }
            }
        }

        private void OnRemove(object sender, EventArgs e)
        {
            var items = listBoxLocations.SelectedItems.Cast<string>().ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                listBoxLocations.Items.Remove(items[i]);
            }
        }

        private void OnOk(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
