using System;
using System.IO;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Forms
{
    internal partial class MergeReportForm : Form
    {
        #region Initialization

        public MergeReportForm(
            int mergeNum, int mergeTotal,
            string file1, string file2, string outputFile,
            string modName1, string modName2)
        {
            InitializeComponent();

            if (mergeTotal > 1)
            {
                Text += $" ({mergeNum} of {mergeTotal})";  // Window title
                if (mergeNum < mergeTotal)
                    btnOK.Text = "Continue";
            }

            lblTempContentFiles.Visible = outputFile.StartsWith(Paths.MergedBundleContent);

            grpFile1.Text = modName1;
            grpFile2.Text = modName2;
            
            txtFilePath1.Text = file1;
            txtFilePath2.Text = file2;
            txtMergedPath.Text = outputFile;

            chkShowAfterMerge.Checked = Program.MainForm.MergeReportSetting;

            btnOK.Select();
        }

        void MergeReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.MainForm.MergeReportSetting = chkShowAfterMerge.Checked;
        }

        #endregion

        #region Button Clicks

        void btnOpenFile1_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtFilePath1.Text);
        }

        void btnOpenFile2_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtFilePath2.Text);
        }

        void btnOpenOutputFile_Click(object sender, EventArgs e)
        {
            TryOpenFile(txtMergedPath.Text);
        }

        void btnOpenDir1_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtFilePath1.Text);
        }

        void btnOpenDir2_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtFilePath2.Text);
        }

        void btnOpenOutputDir_Click(object sender, EventArgs e)
        {
            TryOpenFileDir(txtMergedPath.Text);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        void TryOpenFile(string path)
        {
            if (File.Exists(path))
                System.Diagnostics.Process.Start(path);
            else
                Program.MainForm.ShowMessage("Can't find file: " + path);
        }

        void TryOpenFileDir(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
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
