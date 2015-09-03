using System;
using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public partial class ReportForm : Form
    {
        private string _modsDir;
        private string _backupsDir;
        private bool _success;

        public ReportForm(int mergeNum, int mergeTotal, Changeset.ChangesetResult mergeResult, string modsDir, string backupsDir)
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
            _modsDir = modsDir;
            _backupsDir = backupsDir;

            chkAutoBackup.Checked = Program.Settings.Get<bool>("MoveToBackupAfterMerge");
            chkAutoBackup.Select();
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.Set("MoveToBackupAfterMerge", chkAutoBackup.Checked);
        }

        public void SetModNames(string modName1, string modName2)
        {
            grpFile1.Text = modName1;
            grpFile2.Text = modName2;
        }

        public void SetFilePaths(string file1, string file2, string outputFile)
        {
            txtFilePath1.Text = file1;
            txtFilePath2.Text = file2;
            txtOutputPath.Text = outputFile;
            if (outputFile.EqualsIgnoreCase(file1))
                DisableBackupButton(btnBackUpFile1);
            if (outputFile.EqualsIgnoreCase(file2))
                DisableBackupButton(btnBackUpFile2);
        }

        private void DisableBackupButton(Button btn)
        {
            btnBackUpFile1.Enabled = false;
            btnBackUpFile1.Text = "Can't backup (path matches output file)";
        }

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

        private bool BackUpFile(TextBox txtPath, Button btnBackUp)
        {
            if (!File.Exists(txtPath.Text))
            {
                MessageBox.Show("Can't find file: " + txtPath.Text);
                return false;
            }

            var file = new FileInfo(txtPath.Text);

            string modName = ModDirectory.GetModName(file);
            string modDirPath = Path.Combine(_modsDir, modName);
            string backupPath = Path.Combine(_backupsDir, modName, ModDirectory.GetRelativePath(file, _modsDir, false));

            string backupDir = Path.GetDirectoryName(backupPath);
            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);
            else if (File.Exists(backupPath))
            {
                int extensionStart = backupPath.LastIndexOf('.');
                backupPath = backupPath.Substring(0, extensionStart) +
                    DateTime.Now.ToString("-yyyyMMdd-HHmmss") +
                    backupPath.Substring(extensionStart);
            }

            File.Move(file.FullName, backupPath);
            txtPath.Text = backupPath;

            var remainingModFiles = Directory.GetFiles(modDirPath, "*", SearchOption.AllDirectories);
            if (remainingModFiles.Length == 0)
            {
                System.Threading.Thread.Sleep(100);
                Directory.Delete(modDirPath, true);
            }

            btnBackUp.Text = "Undo Move to Backup Directory";
            return true;
        }

        private void UndoBackUp(TextBox txtPath, Button btnBackUp)
        {
            throw new NotImplementedException();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (_success && chkAutoBackup.Checked)
            {
                if (txtFilePath1.Text != txtOutputPath.Text && !btnBackUpFile1.Text.StartsWith("Undo"))
                    BackUpFile(txtFilePath1, btnBackUpFile1);
                if (txtFilePath2.Text != txtOutputPath.Text && !btnBackUpFile2.Text.StartsWith("Undo"))
                    BackUpFile(txtFilePath2, btnBackUpFile2);
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
