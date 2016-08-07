using System;
using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    partial class PackReportForm : Form
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
            Program.TryOpenFileLocation(txtBundlePath.Text);
        }

        void btnOpenContentDir_Click(object sender, EventArgs e)
        {
            Program.TryOpenDirectory(Paths.MergedBundleContent);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }
    }
}
