using System;
using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    internal partial class PackReportForm : Form
    {
        #region Initialization

        public PackReportForm(string bundlePath)
        {
            InitializeComponent();
            
            txtBundlePath.Text = bundlePath;

            var contentPaths = Directory.GetFiles(Paths.MergedBundleContent, "*", SearchOption.AllDirectories);
            txtContent.Text = string.Join(Environment.NewLine, contentPaths);

            chkShowAfterPack.Checked = Program.MainForm.PackReportSetting;

            btnOK.Select();
        }

        void PackReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.MainForm.PackReportSetting = chkShowAfterPack.Checked;
        }

        #endregion

        #region Button Clicks

        void btnOpenBundleDir_Click(object sender, EventArgs e)
        {
            TryOpenDir(Path.GetDirectoryName(txtBundlePath.Text));
        }

        void btnOpenContentDir_Click(object sender, EventArgs e)
        {
            TryOpenDir(Paths.MergedBundleContent);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        void TryOpenDir(string dirPath)
        {
            if (Directory.Exists(dirPath))
                System.Diagnostics.Process.Start(dirPath);
            else
                Program.MainForm.ShowMessage("Can't find directory: " + dirPath);
        }

        void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }
    }
}
