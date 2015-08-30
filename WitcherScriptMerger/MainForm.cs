using GoogleDiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public partial class MainForm : Form
    {
        #region Members
        private FormWindowState _lastWindowState = FormWindowState.Minimized;

        private string _scriptsDirSetting = Program.Settings.Get("ScriptsDirectory");
        private string _modsDirSetting = Program.Settings.Get("ModsDirectory");

        private TreeNode _clickedNode = null;
        private List<TreeNode> _checkedModNodes = new List<TreeNode>();
        private FileInfo _vanillaFile = null;
        private DiffMatchPatch _differ = new DiffMatchPatch();

        private string ScriptsDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_scriptsDirSetting))
                    return _scriptsDirSetting;
                return Path.Combine(txtGameDir.Text, "content\\content0\\scripts");
            }
        }

        private string ModsDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_modsDirSetting))
                    return _modsDirSetting;
                return Path.Combine(txtGameDir.Text, "Mods");
            }
        }

        private string BackupDirectory
        {
            get
            {
                if (Path.IsPathRooted(txtBackupDir.Text))
                    return txtBackupDir.Text;
                else
                    return Path.Combine(txtGameDir.Text, txtBackupDir.Text);
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();

            this.Text += " v" + Application.ProductVersion;

            txtGameDir.Text = Program.Settings.Get("GameDirectory");
            chkCheckAtLaunch.Checked = Program.Settings.Get<bool>("CheckAtLaunch");
            txtMergedModName.Text = Program.Settings.Get("MergedModName");
            txtBackupDir.Text = Program.Settings.Get("BackupDirectory");
            chkMoveToBackup.Checked = Program.Settings.Get<bool>("MoveToBackupAfterMerge");
            chkLineBreakSymbol.Checked = Program.Settings.Get<bool>("LineBreakSymbol");

            LoadLastWindowConfiguration();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (chkCheckAtLaunch.Checked)
                btnCheckForConflicts_Click(null, null);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                Program.Settings.Set("StartMaximized", true);
            else
            {
                Program.Settings.Set("StartMaximized", false);
                Program.Settings.Set("StartWidth", Width);
                Program.Settings.Set("StartHeight", Height);
                Program.Settings.Set("StartPosTop", Top);
                Program.Settings.Set("StartPosLeft", Left);
            }
        }

        private void LoadLastWindowConfiguration()
        {
            Top = Program.Settings.Get<int>("StartPosTop");
            Left = Program.Settings.Get<int>("StartPosLeft");
            if (Top > 0 || Left > 0)
                StartPosition = FormStartPosition.Manual;
            
            int startWidth = Program.Settings.Get<int>("StartWidth");
            int startHeight = Program.Settings.Get<int>("StartHeight");
            if (startWidth > 0)
                Width = startWidth;
            if (startHeight > 0)
                Height = startHeight;
            
            if (Program.Settings.Get<bool>("StartMaximized"))
                WindowState = FormWindowState.Maximized;
        }

        #region Button Clicks

        private void btnSelectGameDirectory_Click(object sender, EventArgs e)
        {
            txtGameDir.Text = GetUserDirectoryChoice();
        }

        private void btnSelectBackupDir_Click(object sender, EventArgs e)
        {
            txtBackupDir.Text = GetUserDirectoryChoice();
        }

        private string GetUserDirectoryChoice()
        {
            FolderBrowserDialog dlgSelectRoot = new FolderBrowserDialog();
            if (Directory.Exists(txtGameDir.Text))
                dlgSelectRoot.SelectedPath = txtGameDir.Text;
            dlgSelectRoot.ShowDialog();
            return dlgSelectRoot.SelectedPath;
        }

        private void btnCheckForConflicts_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(ModsDirectory))
            {
                MessageBox.Show(!string.IsNullOrWhiteSpace(_modsDirSetting)
                    ? "Couldn't find the Mods directory specified in the config file."
                    : "Couldn't find Mods directory in the specified game directory.");
                return;
            }
            if (!Directory.Exists(ScriptsDirectory))
            {
                MessageBox.Show(!string.IsNullOrWhiteSpace(_scriptsDirSetting)
                    ? "Couldn't find the Scripts directory specified in the config file."
                    : "Couldn't find \\content\\content0\\scripts directory in the specified game directory.");
                return;
            }

            _checkedModNodes.Clear();
            treConflicts.Nodes.Clear();
            _vanillaFile = null;

            var directories = Directory.GetDirectories(ModsDirectory, "*", SearchOption.TopDirectoryOnly);
            var modDirectories = directories
                .Select(path => new ModDirectory(path))
                .Where(modDir => modDir.ScriptFiles.Any());

            foreach (var modDir in modDirectories)
            {
                foreach (var scriptFile in modDir.ScriptFiles)
                {
                    string relPath = ModDirectory.GetRelativePath(scriptFile, ModsDirectory);

                    var existingScriptNode = treConflicts.GetTreeNodes()
                        .FirstOrDefault(node => node.Text.EqualsIgnoreCase(relPath));

                    var newModNode = new TreeNode(modDir.ModName);
                    newModNode.Tag = scriptFile;

                    if (existingScriptNode == null)
                    {
                        string vanillaPath = Path.Combine(
                            ScriptsDirectory,
                            relPath.Replace("\\content\\scripts\\", "").Replace(modDir.ModName, ""));
                        if (File.Exists(vanillaPath))
                        {
                            var newScriptNode = new TreeNode(relPath);
                            newScriptNode.Tag = new FileInfo(vanillaPath);
                            newScriptNode.ForeColor = Color.Red;
                            newScriptNode.Nodes.Add(newModNode);
                            treConflicts.Nodes.Add(newScriptNode);
                        }
                    }
                    else
                        existingScriptNode.Nodes.Add(newModNode);
                }
            }

            for (int i = treConflicts.Nodes.Count - 1; i >= 0; --i)
            {
                var scriptNode = treConflicts.Nodes[i];
                if (scriptNode.Nodes.Count == 1)
                    treConflicts.Nodes.RemoveAt(i);
                else
                    scriptNode.Expand();
            }

            if (treConflicts.Nodes.Count == 0)
                MessageBox.Show("No conflicts found.");
            else
            {
                treConflicts.Sort();
                treConflicts.Select();
            }
        }

        private void btnTryMergeSelected_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtGameDir.Text))
            {
                MessageBox.Show("Couldn't find the specified Witcher 3 directory.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMergedModName.Text))
            {
                MessageBox.Show("Enter a name for the mod folder where merged scripts will be saved.");
                return;
            }
            if (!txtMergedModName.Text.IsAlphaNumeric()
                && (DialogResult.No == MessageBox.Show(
                "The Witcher 3 only loads mods with alphanumeric names. Do you want to use the following name anyway?" + Environment.NewLine + Environment.NewLine + txtMergedModName.Text,
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)))
                return;

            var file1 = _checkedModNodes[0].Tag as FileInfo;

            string outputPath = Path.Combine(
                    ModsDirectory,
                    txtMergedModName.Text,
                    ModDirectory.GetRelativePath(file1, ModsDirectory, false));
            if (!File.Exists(outputPath)
                && (DialogResult.No == MessageBox.Show(
                    "The output file below already exists! Overwrite?" + Environment.NewLine + Environment.NewLine + outputPath,
                    "Overwrite?",
                    MessageBoxButtons.YesNo)))
                return;

            for (int i = 1; i < _checkedModNodes.Count; ++i)
            {
                var file2 = _checkedModNodes[i].Tag as FileInfo;
                var mergedFile = TryMergePair(i, file1, file2, outputPath);
                if (mergedFile != null)
                    file1 = mergedFile;
            }

            btnTryMergeSelected.Enabled = false;
            btnCheckForConflicts_Click(null, null);  // Update conflict list
        }

        private FileInfo TryMergePair(int mergeNum, FileInfo file1, FileInfo file2, string outputPath)
        {
            string vanillaText = File.ReadAllText(_vanillaFile.FullName);
            var set1 = new Changeset(vanillaText, file1);
            var set2 = new Changeset(vanillaText, file2);

            var combinedSet = new ChangesetMerger(set1, set2, vanillaText).Merge();
            var result = combinedSet.TryApply(vanillaText);

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            File.WriteAllText(outputPath, result.Text);

            string backupMsg = "";
            if (result.FailureCount == 0 && chkMoveToBackup.Checked)
                backupMsg = CleanUpAfterMerge(file1, file2, outputPath);

            string msg =
                string.Format("{0}  +  {1}", set1.ModName, set2.ModName) +
                Environment.NewLine +
                string.Format("{0} out of {1} changes merged successfully!",
                    result.SuccessCount,
                    result.AttemptCount) +
                Environment.NewLine +
                Environment.NewLine + "Output file: " +
                Environment.NewLine + outputPath.Replace(txtGameDir.Text, "");
            if (!string.IsNullOrWhiteSpace(backupMsg))
                msg += backupMsg;

            MessageBox.Show(msg, string.Format("Merge Finished ({0} of {1})", mergeNum, _checkedModNodes.Count - 1));

            return new FileInfo(outputPath);
        }

        private string CleanUpAfterMerge(FileInfo file1, FileInfo file2, string outputPath)
        {
            string backupMsg = "";
            foreach (var file in new FileInfo[] { file1, file2 })
            {
                if (file.FullName.EqualsIgnoreCase(outputPath))
                    continue;

                string modName = ModDirectory.GetModName(file);
                string modDirPath = Path.Combine(ModsDirectory, modName);
                string backupPath = Path.Combine(BackupDirectory, modName, ModDirectory.GetRelativePath(file, ModsDirectory, false));

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
                backupMsg += Environment.NewLine + Environment.NewLine +
                    Environment.NewLine + "MOVED OBSOLETE SCRIPT" + Environment.NewLine + "——————————————" +
                    Environment.NewLine + file.FullName.Replace(ModsDirectory, "") +
                    Environment.NewLine +
                    Environment.NewLine + "TO " + Environment.NewLine + "—————" +
                    Environment.NewLine + backupPath.Replace(txtGameDir.Text, "");

                var remainingModFiles = Directory.GetFiles(modDirPath, "*", SearchOption.AllDirectories);
                if (remainingModFiles.Length == 0)
                {
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(modDirPath, true);
                    backupMsg += Environment.NewLine +
                        Environment.NewLine + "DELETED " + Environment.NewLine + "—————" +
                        Environment.NewLine + "Empty " + modName + " folder";
                }
            }
            return backupMsg;
        }

        #endregion

        #region TreeView

        private void treConflicts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treConflicts.SelectedNode = null;
        }

        private void treConflicts_MouseDown(object sender, MouseEventArgs e)
        {
            treConflicts.BeginUpdate();
            _clickedNode = treConflicts.GetNodeAt(e.Location);
            if (_clickedNode != null && !_clickedNode.Bounds.Contains(e.Location))
                _clickedNode = null;
        }

        private void treConflicts_MouseUp(object sender, MouseEventArgs e)
        {
            _clickedNode = treConflicts.GetNodeAt(e.Location);
            if (_clickedNode != null && !_clickedNode.Bounds.Contains(e.Location))
                _clickedNode = null;

            if (e.Button == MouseButtons.Left)
            {
                if (_clickedNode != null)
                {
                    _clickedNode.Checked = !_clickedNode.Checked;
                    HandleCheckedChange();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextExpandAll.Available = contextCollapseAll.Available = (treConflicts.Nodes.Count > 0);
                contextCopyPath.Available = (_clickedNode != null);
                contextModScript.Available = contextModDir.Available =
                    (_clickedNode != null && _clickedNode.Nodes.Count == 0);
                contextVanillaScript.Available = contextVanillaDir.Available =
                    (_clickedNode != null && _clickedNode.Nodes.Count > 0);

                if (treeContextMenu.Items.Cast<ToolStripMenuItem>().Any(item => item.Available))
                    treeContextMenu.Show(treConflicts, e.X, e.Y);
            }
            treConflicts.EndUpdate();
        }

        private void treConflicts_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)  // Event was triggered programmatically
                return;

            _clickedNode = e.Node;
            HandleCheckedChange();
        }

        private void HandleCheckedChange()
        {
            TreeNode scriptNode;
            if (_clickedNode.Nodes.Count > 0)  // Script node
            {
                scriptNode = _clickedNode;
                if (_clickedNode.Checked)
                {
                    foreach (var modNode in _clickedNode.GetTreeNodes())
                    {
                        modNode.Checked = true;
                        if (!_checkedModNodes.Contains(modNode))
                            _checkedModNodes.Add(modNode);
                    }
                }
                else
                    ClearCheckedNodes();
            }
            else  // Mod node
            {
                scriptNode = _clickedNode.Parent;
                if (_clickedNode.Checked)
                    _checkedModNodes.Add(_clickedNode);
                else
                    _checkedModNodes.Remove(_clickedNode);
                scriptNode.Checked = scriptNode.GetTreeNodes().All(node => node.Checked);
            }
            if (_clickedNode.Checked && _vanillaFile != scriptNode.Tag as FileInfo)
            {
                _vanillaFile = scriptNode.Tag as FileInfo;
                ClearCheckedNodes(scriptNode);
            }
            btnTryMergeSelected.Enabled = (_checkedModNodes.Count > 1);
        }

        private void ClearCheckedNodes(TreeNode subTreeToSkip = null)
        {
            foreach (var scriptNode in treConflicts.GetTreeNodes())
            {
                if (subTreeToSkip != null && scriptNode.Text == subTreeToSkip.Text)
                    continue;
                scriptNode.Checked = false;
                foreach (var modNode in scriptNode.GetTreeNodes())
                {
                    modNode.Checked = false;
                    _checkedModNodes.Remove(modNode);
                }
            }
        }

        #endregion

        #region TreeView Context Menu

        private void contextOpenScript_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            System.Diagnostics.Process.Start(
                (_clickedNode.Tag as FileInfo).FullName);
        }

        private void contextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            System.Diagnostics.Process.Start(GetPathFromNode(_clickedNode, false));
        }

        private void contextCopyPath_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            Clipboard.SetText(GetPathFromNode(_clickedNode));
        }

        private void contextExpandAll_Click(object sender, EventArgs e)
        {
            treConflicts.ExpandAll();
        }

        private void contextCollapseAll_Click(object sender, EventArgs e)
        {
            treConflicts.CollapseAll();
        }

        private string GetPathFromNode(TreeNode node, bool includeFile = true)
        {
            if (node.Tag is FileInfo)
            {
                if (includeFile)
                    return (node.Tag as FileInfo).FullName;
                else
                    return Path.GetDirectoryName((node.Tag as FileInfo).FullName);
            }
            else
                return (node.Tag as DirectoryInfo).FullName;
        }

        #endregion

        #region Field Input

        private void txtGameDirectory_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
        }

        private void chkCheckAtLaunch_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("CheckAtLaunch", chkCheckAtLaunch.Checked);
        }

        private void txtMergedModName_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("MergedModName", txtMergedModName.Text);
        }

        private void txtBackupDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("BackupDirectory", txtBackupDir.Text);
        }

        private void chkMoveToBackup_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("MoveToBackupAfterMerge", chkMoveToBackup.Checked);
        }

        private void chkLineBreakSymbol_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("LineBreakSymbol", chkLineBreakSymbol.Checked);
        }

        #endregion
    }
}