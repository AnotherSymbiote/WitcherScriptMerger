using System;
using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public partial class ReportForm : Form
    {
        bool _savedCheckedState;

        #region Initialization

        public ReportForm(
            int mergeNum, int mergeTotal,
            string file1, string file2, string outputFile,
            string modName1, string modName2)
        {
            InitializeComponent();

            if (mergeTotal > 1)
            {
                this.Text += string.Format(" ({0} of {1})", mergeNum, mergeTotal);
                if (mergeNum < mergeTotal)
                    btnOK.Text = "Continue";
            }

            grpFile1.Text = modName1;
            grpFile2.Text = modName2;
            
            txtFilePath1.Text = file1;
            txtFilePath2.Text = file2;
            txtMergedPath.Text = outputFile;

            chkShowAfterMerge.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            _savedCheckedState = chkShowAfterMerge.Checked;

            btnOK.Select();
        }

        #endregion

        #region Button Clicks

        private void btnOpenFile1_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtFilePath1.Text);
        }

        private void btnOpenFile2_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtFilePath2.Text);
        }

        private void btnOpenOutputFile_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtMergedPath.Text);
        }

        private void btnOpenDir1_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtFilePath1.Text);
        }

        private void btnOpenDir2_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtFilePath2.Text);
        }

        private void btnOpenOutputDir_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtMergedPath.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        private void TryOpenFile(string path)
        {
            if (File.Exists(path))
                System.Diagnostics.Process.Start(path);
            else
                MessageBox.Show("Can't find file: " + path);
        }

        private void TryOpenFileDir(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dirPath))
                System.Diagnostics.Process.Start(dirPath);
            else
                MessageBox.Show("Can't find directory: " + dirPath);
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkShowAfterMerge.Checked != _savedCheckedState)
                Program.Settings.Set("ReportAfterMerge", chkShowAfterMerge.Checked);
        }
    }
}
