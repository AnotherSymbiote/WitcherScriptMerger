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

            lblTempContentFiles.Visible = outputFile.StartsWithIgnoreCase(Paths.MergedBundleContent);

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
            Program.TryOpenFile(txtFilePath1.Text);
        }

        void btnOpenFile2_Click(object sender, EventArgs e)
        {
            Program.TryOpenFile(txtFilePath2.Text);
        }

        void btnOpenOutputFile_Click(object sender, EventArgs e)
        {
            Program.TryOpenFile(txtMergedPath.Text);
        }

        void btnOpenDir1_Click(object sender, EventArgs e)
        {
            Program.TryOpenFileLocation(txtFilePath1.Text);
        }

        void btnOpenDir2_Click(object sender, EventArgs e)
        {
            Program.TryOpenFileLocation(txtFilePath2.Text);
        }

        void btnOpenOutputDir_Click(object sender, EventArgs e)
        {
            Program.TryOpenFileLocation(txtMergedPath.Text);
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
