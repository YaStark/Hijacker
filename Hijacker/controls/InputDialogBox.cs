using System.Windows.Forms;

namespace Hijacker.controls
{
    public partial class InputDialogBox : Form
    {
        private InputDialogBox(string title, string text)
        {
            InitializeComponent();
            textBox.Text = text;
            Text = title;
        }

        private void OnOk(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public static bool ShowDialog(string title, ref string text)
        {
            using (var dlg = new InputDialogBox(title, text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    text = dlg.textBox.Text;
                    return true;
                }
                return false;
            }
        }
    }
}
