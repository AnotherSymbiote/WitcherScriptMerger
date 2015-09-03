using System;
using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public partial class ReportForm : Form
    {
        private bool _success;

        private string BackupDirectory
        {
            get
            {
                if (Path.IsPathRooted(txtBackupDir.Text))
                    return txtBackupDir.Text;
                else
                    return Path.Combine(Program.MainForm.GameDirectory, txtBackupDir.Text);
            }
        }

        #region Initialization

        public ReportForm(
            int mergeNum, int mergeTotal, Changeset.ChangesetResult mergeResult,
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

            _success = (mergeResult.SuccessCount == mergeResult.AttemptCount);
            lblSuccessCount.Text = string.Format(lblSuccessCount.Text, mergeResult.SuccessCount, mergeResult.AttemptCount);

            txtBackupDir.Text = Program.Settings.Get("BackupDirectory");
            chkAutoBackup.Checked = Program.Settings.Get<bool>("MoveToBackupAfterMerge");

            grpFile1.Text = modName1;
            grpFile2.Text = modName2;
            
            txtFilePath1.Text = file1;
            txtFilePath2.Text = file2;
            txtOutputPath.Text = outputFile;
            if (outputFile.EqualsIgnoreCase(file1))
                DisableBackupButton(btnBackUpFile1);
            if (outputFile.EqualsIgnoreCase(file2))
                DisableBackupButton(btnBackUpFile2);
            
            chkAutoBackup.Select();
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.Set("MoveToBackupAfterMerge", chkAutoBackup.Checked);
        }

        private void DisableBackupButton(Button btn)
        {
            btnBackUpFile1.Enabled = false;
            btnBackUpFile1.Text = "Can't backup (path matches output file)";
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
            TryOpenFile(txtOutputPath.Text);
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
            TryOpenFileDir(txtOutputPath.Text);
        }

        private void btnBackUpFile1_Click(object sender, EventArgs e)
        {
            if (btnBackUpFile1.Text.StartsWith("Undo"))
                UndoBackUp(txtFilePath1, btnBackUpFile1);
            else
                BackUpFile(txtFilePath1, btnBackUpFile1);
        }

        private void btnBackUpFile2_Click(object sender, EventArgs e)
        {
            if (btnBackUpFile2.Text.StartsWith("Undo"))
                UndoBackUp(txtFilePath2, btnBackUpFile2);
            else
                BackUpFile(txtFilePath2, btnBackUpFile2);
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

        private void BackUpFile(TextBox txtPath, Button btnBackUp)
        {
            if (MoveFile(txtPath, Program.MainForm.ModsDirectory, txtBackupDir.Text))
                btnBackUp.Text = "Undo Move to Backup Directory";
        }

        private void UndoBackUp(TextBox txtPath, Button btnBackUp)
        {
            if (MoveFile(txtPath, txtBackupDir.Text, Program.MainForm.ModsDirectory))
                btnBackUp.Text = "Move to Backup Directory";
        }

        private bool MoveFile(TextBox txtPath, string sourceRelRoot, string destinationRelRoot)
        {
            if (!File.Exists(txtPath.Text))
            {
                MessageBox.Show("Can't find file: " + txtPath.Text);
                return false;
            }

            string relPath = ModDirectory.GetRelativePath(txtPath.Text, true, false);
            string destinationPath = Path.Combine(destinationRelRoot, relPath);
            string destinationDir = Path.GetDirectoryName(destinationPath);

            string fileName = Path.GetFileName(txtPath.Text);
            string outputFileName = Path.GetFileName(txtOutputPath.Text);
            if (!fileName.EqualsIgnoreCase(outputFileName))
                destinationPath = Path.Combine(destinationDir, outputFileName);

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);
            else if (File.Exists(destinationPath))
            {
                using (var renameForm = new RenameFileForm(destinationPath))
                {
                    if (renameForm.ShowDialog() == DialogResult.Cancel)
                        return false;
                    destinationPath = renameForm.FilePath;
                }
            }

            if (File.Exists(destinationPath))  // If user saw rename form & didn't cancel, it's
                File.Delete(destinationPath);  // okay to delete the existing destination file
            File.Move(txtPath.Text, destinationPath);
            txtPath.Text = destinationPath;

            string sourceModDirPath = Path.Combine(sourceRelRoot, ModDirectory.GetModName(relPath));
            var remainingModFiles = Directory.GetFiles(sourceModDirPath, "*", SearchOption.AllDirectories);
            if (remainingModFiles.Length == 0)
            {
                System.Threading.Thread.Sleep(100);  // Avoid directory not empty exception
                Directory.Delete(sourceModDirPath, true);
            }
            return true;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (_success && chkAutoBackup.Checked)
            {
                if (txtFilePath1.Text != txtOutputPath.Text && !btnBackUpFile1.Text.StartsWith("Undo"))
                    btnBackUpFile1_Click(null, null);
                if (txtFilePath2.Text != txtOutputPath.Text && !btnBackUpFile2.Text.StartsWith("Undo"))
                    btnBackUpFile2_Click(null, null);
            }
            DialogResult = DialogResult.OK;
        }

        private void btnSelectBackupDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlgSelectRoot = new FolderBrowserDialog();
            if (Directory.Exists(txtBackupDir.Text))
                dlgSelectRoot.SelectedPath = txtBackupDir.Text;
            dlgSelectRoot.ShowDialog();
            txtBackupDir.Text = dlgSelectRoot.SelectedPath;
        }

        private void txtBackupDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("BackupDirectory", txtBackupDir.Text);
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }
    }
}
