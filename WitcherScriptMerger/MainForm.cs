using GoogleDiffMatchPatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public partial class MainForm : Form
    {
        #region Members
        private TreeNode _clickedNode = null;
        private List<TreeNode> _checkedModNodes = new List<TreeNode>();
        private FileInfo _vanillaFile = null;
        private DiffMatchPatch _differ = new DiffMatchPatch();

        private string ScriptsDirectory
        {
            get { return Path.Combine(txtGameDir.Text, "content\\content0\\scripts"); }
        }
        
        private string ModsDirectory
        {
            get { return Path.Combine(txtGameDir.Text, "Mods"); }
        }

        private string BackupDirectory
        {
            get
            {
                if (Path.IsPathRooted(txtBackupDir.Text))
                    return txtBackupDir.Text;
                else
                    return Path.Combine(txtGameDir.Text, txtBackupDir.Text); }
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

            if (chkCheckAtLaunch.Checked)
                btnCheckForConflicts_Click(null, null);
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
                MessageBox.Show("Couldn't find Mods directory in the specified game directory.");
                return;
            }
            if (!Directory.Exists(ScriptsDirectory))
            {
                MessageBox.Show("Couldn't find \\content\\content0\\scripts directory in the specified game directory.");
                return;
            }

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
                        .FirstOrDefault(node => node.Text == relPath);
                    
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
            if (string.IsNullOrWhiteSpace(txtMergedModName.Text))
            {
                MessageBox.Show("Enter a name for the mod folder where merged scripts will be saved.");
                return;
            }

            string vanillaText = File.ReadAllText(_vanillaFile.FullName);

            var file1 = _checkedModNodes[0].Tag as FileInfo;

            string outputPath = Path.Combine(
                    ModsDirectory,
                    txtMergedModName.Text,
                    ModDirectory.GetRelativePath(file1, ModsDirectory, false));
            if (!File.Exists(outputPath)
                || (DialogResult.Yes == MessageBox.Show(
                    "The output file below already exists! Overwrite?" + Environment.NewLine + Environment.NewLine + outputPath,
                    "Overwrite?",
                    MessageBoxButtons.YesNo)))
            {
                for (int i = 1; i < _checkedModNodes.Count; ++i)
                {
                    var file2 = _checkedModNodes[i].Tag as FileInfo;
                    var mergedFile = TryMergePair(i, vanillaText, file1, file2, outputPath);
                    if (mergedFile != null)
                        file1 = mergedFile;
                }

                _checkedModNodes.Clear();
                btnTryMergeSelected.Enabled = false;
                btnCheckForConflicts_Click(null, null);  // Update conflict list
            }
        }

        private FileInfo TryMergePair(int mergeNum, string vanillaText, FileInfo file1, FileInfo file2, string outputPath)
        {
            string text1 = File.ReadAllText(file1.FullName);
            string text2 = File.ReadAllText(file2.FullName);
            var diffs1 = _differ.DiffMain(vanillaText, text1);
            var diffs2 = _differ.DiffMain(vanillaText, text2);
            var patchList1 = _differ.PatchMake(vanillaText, diffs1);
            var patchList2 = _differ.PatchMake(vanillaText, diffs2);

            string modName1 = ModDirectory.GetModName(file1);
            string modName2 = ModDirectory.GetModName(file2);

            var combinedPatchList = patchList1.Combine(patchList2,
                vanillaText, text1, text2,
                file1.Name, modName1, modName2);

            var mergeResult = _differ.PatchApply(combinedPatchList, vanillaText);
            string mergeText = (string)mergeResult[0];
            bool[] mergeSuccesses = (bool[])mergeResult[1];
            int patchFailCount = mergeSuccesses.Count(success => !success);
            int patchSuccessCount = mergeSuccesses.Length - patchFailCount;
            int totalPatchCount = mergeSuccesses.Length;

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            File.WriteAllText(outputPath, mergeText);

            string backupMsg = "";
            if (patchFailCount == 0 && chkMoveToBackup.Checked)
                backupMsg = CleanUpAfterMerge(file1, file2, outputPath);

            string msg =
                string.Format("{0}  +  {1}", modName1, modName2) +
                Environment.NewLine +
                string.Format("{0} out of {1} changes merged successfully!",
                    (mergeSuccesses.Length - patchFailCount),
                    mergeSuccesses.Length) +
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
                if (file.FullName == outputPath)
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
                        DateTime.Now.ToString(Program.Settings.Get("BackupNameDateFormat")) +
                        backupPath.Substring(extensionStart);
                }

                File.Move(file.FullName, backupPath);
                backupMsg += Environment.NewLine + Environment.NewLine +
                    Environment.NewLine + "MOVED OBSOLETE SCRIPT" + Environment.NewLine + "——————————————" +
                    Environment.NewLine + file.FullName.Replace(txtGameDir.Text, "") +
                    Environment.NewLine +
                    Environment.NewLine + "TO " + Environment.NewLine + "—————" +
                    Environment.NewLine + backupPath.Replace(txtGameDir.Text, "");

                var remainingModFiles = Directory.GetFiles(modDirPath, "*", SearchOption.AllDirectories);
                if (remainingModFiles.Length == 0)
                {
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

        #endregion
    }
}