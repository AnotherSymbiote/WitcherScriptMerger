using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.Forms
{
    public partial class MainForm : Form
    {
        #region Members

        public const string InventoryPath = "MergeInventory.xml";
        public static string ModScriptBase = Path.Combine("content", "scripts");
        public static string VanillaScriptBase = Path.Combine("content", "content0", "scripts");

        private TreeView _clickedTree = null;
        private TreeNode _clickedNode = null;
        private int _mergesToDo = 0;
        private string _kdiff3PathSetting = Program.Settings.Get("KDiff3Path");

        public string GameDirectory
        {
            get { return txtGameDir.Text; }
        }
        
        private string _scriptsDirSetting = Program.Settings.Get("ScriptsDirectory");
        public string ScriptsDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_scriptsDirSetting))
                    return _scriptsDirSetting;
                return Path.Combine(txtGameDir.Text, VanillaScriptBase);
            }
        }

        private string _modsDirSetting = Program.Settings.Get("ModsDirectory");
        public string ModsDirectory
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
                    return Path.Combine(GameDirectory, txtBackupDir.Text);
            }
        }
        #endregion

        #region Form Operations

        public MainForm()
        {
            InitializeComponent();
            this.Text += " v" + Application.ProductVersion;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.Settings.StartBatch();
            txtGameDir.Text = Program.Settings.Get("GameDirectory");
            chkCheckAtLaunch.Checked = Program.Settings.Get<bool>("CheckAtLaunch");
            txtMergedModName.Text = Program.Settings.Get("MergedModName");
            txtBackupDir.Text = Program.Settings.Get("BackupDirectory");
            chkReviewEachMerge.Checked = Program.Settings.Get<bool>("ReviewEachMerge");

            LoadLastWindowConfiguration();
            Program.Settings.EndBatch();   
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LoadMergeInventory();
            if (chkCheckAtLaunch.Checked)
                btnCheckForConflicts_Click(null, null);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.StartBatch();
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
            Program.Settings.Set("CheckAtLaunch", chkCheckAtLaunch.Checked);
            Program.Settings.Set("MergedModName", txtMergedModName.Text);
            Program.Settings.Set("BackupDirectory", txtBackupDir.Text);
            Program.Settings.Set("ReviewEachMerge", chkReviewEachMerge.Checked);

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
            Program.Settings.Set("StartSplitterPosPct", (int)((float)splitContainer.SplitterDistance / splitContainer.Width * 100f));
            Program.Settings.EndBatch();
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

            splitContainer.SplitterDistance = (int)(Program.Settings.Get<int>("StartSplitterPosPct") / 100f * splitContainer.Width);
        }

        private void LoadMergeInventory()
        {
            btnUnmergeSelected.Enabled = false;
            treMergeInventory.Nodes.Clear();
            var inventory = MergeInventory.Load(InventoryPath);
            foreach (var script in inventory.MergedScripts)
            {
                var scriptNode = new TreeNode(script.RelativePath);
                scriptNode.Tag = GetModFile(ModsDirectory, script.MergedModName, script.RelativePath);
                scriptNode.ForeColor = Color.Blue;
                treMergeInventory.Nodes.Add(scriptNode);

                foreach (var modBackupName in script.ModBackups)
                {
                    var modNode = new TreeNode(modBackupName);
                    modNode.Tag = GetModFile(BackupDirectory, modBackupName, script.RelativePath);
                    scriptNode.Nodes.Add(modNode);
                }
            }
            treMergeInventory.Sort();
            treMergeInventory.ExpandAll();
            foreach (var modNode in treMergeInventory.GetTreeNodes().SelectMany(node => node.GetTreeNodes()))
                modNode.HideCheckBox();
        }

        private FileInfo GetModFile(string root, string modName, string relPath)
        {
            return new FileInfo(Path.Combine(root, modName, ModScriptBase, relPath));
        }

        private void txtGameDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
        }

        private void txtBackupDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("BackupDirectory", txtBackupDir.Text);
        }

        #endregion

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

            btnTryMergeSelected.Enabled = false;
            treConflicts.Nodes.Clear();

            var ignoredModNames = GetIgnoredModNames();
            var directories = Directory.GetDirectories(ModsDirectory, "*", SearchOption.TopDirectoryOnly);
            var modDirectories = directories
                .Select(path => new ModDirectory(path))
                .Where(modDir => modDir.ScriptFiles.Any())
                .Where(modDir => !ignoredModNames.Any(name => name.EqualsIgnoreCase(modDir.ModName)));

            foreach (var modDir in modDirectories)
            {
                foreach (var scriptFile in modDir.ScriptFiles)
                {
                    string relPath = ModDirectory.GetMinimalRelativePath(scriptFile.FullName);

                    var existingScriptNode = treConflicts.GetTreeNodes()
                        .FirstOrDefault(node => node.Text.EqualsIgnoreCase(relPath));

                    var newModNode = new TreeNode(modDir.ModName);
                    newModNode.Tag = scriptFile;

                    if (existingScriptNode == null)
                    {
                        string vanillaPath = Path.Combine(ScriptsDirectory, relPath);
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

        private IEnumerable<string> GetIgnoredModNames()
        {
            string ignoredNames = Program.Settings.Get("IgnoreModNames");
            return ignoredNames.Split(',')
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim());
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

            var inventory = MergeInventory.Load(InventoryPath);
            bool updatedInventory = false;
            
            var scriptNodes = treConflicts.GetTreeNodes().Where(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);
            foreach (var scriptNode in scriptNodes)
            {
                var modNodes = scriptNode.GetTreeNodes().Where(modNode => modNode.Checked).ToList();
                var file1 = modNodes[0].Tag as FileInfo;

                string outputPath = Path.Combine(
                        ModsDirectory,
                        txtMergedModName.Text,
                        ModDirectory.GetRelativePath(file1.FullName, false, false));
                if (File.Exists(outputPath)
                    && (DialogResult.No == MessageBox.Show(
                        "The output file below already exists! Overwrite?" + Environment.NewLine + Environment.NewLine + outputPath,
                        "Overwrite?",
                        MessageBoxButtons.YesNo)))
                    continue;

                var vanillaFile = scriptNode.Tag as FileInfo;
                _mergesToDo = modNodes.Count - 1;

                var mergedScript = inventory.MergedScripts.FirstOrDefault(ms => ms.RelativePath == scriptNode.Text);
                if (mergedScript == null)
                {
                    mergedScript = new MergedScript
                    {
                        RelativePath = scriptNode.Text,
                        MergedModName = txtMergedModName.Text,
                        ModBackups = new List<string>()
                    };
                    inventory.MergedScripts.Add(mergedScript);
                }

                for (int i = 1; i < modNodes.Count; ++i)
                {
                    var file2 = modNodes[i].Tag as FileInfo;
                    var mergedFile = TryMergePair(i, vanillaFile, file1, file2, outputPath, mergedScript);
                    if (mergedFile != null)
                    {
                        updatedInventory = true;
                        file1 = mergedFile;
                    }
                    else
                    {
                        string msg = "Merge was canceled.";
                        var buttons = MessageBoxButtons.OK;
                        if (_mergesToDo > 1 || scriptNodes.Count() > 1)
                        {
                            buttons = MessageBoxButtons.OKCancel;
                            if (_mergesToDo > 1)
                                msg = string.Format("Merge {0} of {1} was canceled.", i, _mergesToDo);
                        }
                        var result = MessageBox.Show(msg, "Skipped Merge", buttons, MessageBoxIcon.Information);
                        if (result == System.Windows.Forms.DialogResult.Cancel)
                        {
                            if (updatedInventory)
                                inventory.Save(InventoryPath);
                            UpdateTrees();
                            return;
                        }
                    }
                }
            }
            if (updatedInventory)
                inventory.Save(InventoryPath);
            UpdateTrees();
        }

        private FileInfo TryMergePair(
            int mergeNum,
            FileInfo vanillaFile, FileInfo file1, FileInfo file2,
            string outputPath,
            MergedScript mergedScript)
        {
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string modName1 = ModDirectory.GetModName(file1);
            string modName2 = ModDirectory.GetModName(file2);

            string args = string.Format(
                "\"{0}\" \"{1}\" \"{2}\" -o \"{3}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\"",
                vanillaFile.FullName, file1.FullName, file2.FullName, outputPath);

            if (!chkReviewEachMerge.Checked)
                args += " --auto";
            
            string kdiff3Path = (Path.IsPathRooted(_kdiff3PathSetting)
                ? _kdiff3PathSetting
                : Path.Combine(Environment.CurrentDirectory, _kdiff3PathSetting));
            
            var kdiff3Proc = System.Diagnostics.Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            if (kdiff3Proc.ExitCode == 0)
            {
                string filePath1 = file1.FullName;
                string filePath2 = file2.FullName;
                if (filePath1 != outputPath)
                {
                    if (null != MoveFile(filePath1, ModsDirectory, BackupDirectory))
                        mergedScript.ModBackups.Add(ModDirectory.GetModName(file1));
                }
                if (filePath2 != outputPath)
                {
                    if (null != MoveFile(filePath2, ModsDirectory, BackupDirectory))
                        mergedScript.ModBackups.Add(ModDirectory.GetModName(file2));
                }

                using (var reportForm = new ReportForm(
                    mergeNum, _mergesToDo,
                    filePath1, filePath2, outputPath,
                    modName1, modName2))
                {
                    reportForm.ShowDialog();
                }
            }
            else
                return null;

            return new FileInfo(outputPath);
        }

        private void btnUnmergeSelected_Click(object sender, EventArgs e)
        {
            var inventory = MergeInventory.Load(InventoryPath);
            var scriptNodes = treMergeInventory.GetTreeNodes().Where(node => node.Checked);
            foreach (var scriptNode in scriptNodes)
            {
                var mergedFile = scriptNode.Tag as FileInfo;
                if (!mergedFile.Exists
                    && DialogResult.No == MessageBox.Show("Can't find merged script file: " + Environment.NewLine + Environment.NewLine +
                        mergedFile.FullName + Environment.NewLine + Environment.NewLine +
                        "Move unmerged backups to the Mods directory anyway?",
                        "Missing File",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information))
                    continue;

                var mergedScript = inventory.MergedScripts.FirstOrDefault(ms =>
                    mergedFile.FullName.Contains(ms.MergedModName) &&
                    mergedFile.FullName.Contains(ms.RelativePath));

                foreach (var modNode in scriptNode.GetTreeNodes())
                {
                    var backupFile = modNode.Tag as FileInfo;
                    if (backupFile.Exists)
                    {
                        MoveFile(backupFile.FullName, BackupDirectory, ModsDirectory);
                        mergedScript.ModBackups.Remove(modNode.Text);
                    }
                    else
                        MessageBox.Show("Can't find backup script file: " + mergedFile.FullName);
                }
                if (mergedFile.Exists)
                {
                    mergedFile.Delete();
                    DeleteEmptyDirs(Path.GetDirectoryName(mergedFile.FullName), ScriptsDirectory);
                }
                
                if (mergedScript != null && mergedScript.ModBackups.Count == 0)
                    inventory.MergedScripts.Remove(mergedScript);
            }
            inventory.Save(InventoryPath);
            UpdateTrees();
        }

        private void UpdateTrees()
        {
            LoadMergeInventory();
            btnCheckForConflicts_Click(null, null);
        }

        #endregion

        #region TreeView

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            (sender as TreeView).SelectedNode = null;
        }

        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
            var tree = sender as TreeView;
            tree.EndUpdate();
            tree.BeginUpdate();
            _clickedNode = tree.GetNodeAt(e.Location);
            if (_clickedNode != null && !_clickedNode.Bounds.Contains(e.Location))
                _clickedNode = null;
        }

        private void tree_MouseUp(object sender, MouseEventArgs e)
        {
            var tree = sender as TreeView;
            _clickedNode = tree.GetNodeAt(e.Location);
            if (_clickedNode != null && !_clickedNode.Bounds.Contains(e.Location))
                _clickedNode = null;

            if (e.Button == MouseButtons.Left)
            {
                if (_clickedNode != null && (tree == treConflicts || _clickedNode.Level == 0))
                {
                    _clickedNode.Checked = !_clickedNode.Checked;
                    HandleCheckedChange(sender);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextExpandAll.Available = contextCollapseAll.Available = (tree.Nodes.Count > 0);
                contextCopyPath.Available = (_clickedNode != null);
                contextModScript.Available = contextModDir.Available =
                    (_clickedNode != null && _clickedNode.Nodes.Count == 0);
                contextVanillaScript.Available = contextVanillaDir.Available =
                    (_clickedNode != null && tree == treConflicts && _clickedNode.Nodes.Count > 0);
                contextMergedScript.Available = contextMergedDir.Available =
                    (_clickedNode != null && tree == treMergeInventory && _clickedNode.Nodes.Count > 0);

                // If can copy path, need separator above Select/Deselect All
                contextSelectSeparator.Visible = contextCopyPath.Available;

                if (_clickedNode != null)
                    _clickedNode.BackColor = Color.Gainsboro;

                if (treeContextMenu.Items.OfType<ToolStripMenuItem>().Any(item => item.Available))
                {
                    _clickedTree = tree;
                    treeContextMenu.Show(tree, e.X, e.Y);
                }
            }
            tree.EndUpdate();
        }

        private void tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)  // Event was triggered programmatically
                return;

            _clickedNode = e.Node;
            HandleCheckedChange(sender);
        }

        private void HandleCheckedChange(object sender)
        {
            TreeView tree = sender as TreeView;
            TreeNode scriptNode;
            if (_clickedNode.Nodes.Count > 0)  // Script node
            {
                scriptNode = _clickedNode;
                if (tree == treConflicts)
                {
                    foreach (var modNode in _clickedNode.GetTreeNodes())
                        modNode.Checked = _clickedNode.Checked;
                }
            }
            else  // Mod node
            {
                scriptNode = _clickedNode.Parent;
                scriptNode.Checked = scriptNode.GetTreeNodes().All(node => node.Checked);
            }
            if (sender as TreeView == treConflicts)
                EnableMergeIfValidSelection();
            else
                EnableUnmergeIfValidSelection();
        }

        private void ClearCheckedNodes(TreeNode subTreeToSkip = null)
        {
            foreach (var scriptNode in treConflicts.GetTreeNodes())
            {
                if (subTreeToSkip != null && scriptNode.Text == subTreeToSkip.Text)
                    continue;
                scriptNode.Checked = false;
                foreach (var modNode in scriptNode.GetTreeNodes())
                    modNode.Checked = false;
            }
        }

        private void EnableMergeIfValidSelection()
        {
            int validScriptNodes = treConflicts.GetTreeNodes().Count(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);
            btnTryMergeSelected.Enabled = (validScriptNodes > 0);
            btnTryMergeSelected.Text = (validScriptNodes > 1
                ? "Try to Merge Selected Scripts"
                : "Try to Merge Selected Script");
        }

        private void EnableUnmergeIfValidSelection()
        {
            int selectedNodes = treMergeInventory.GetTreeNodes().Count(node => node.Checked);
            btnUnmergeSelected.Enabled = (selectedNodes > 0);
            btnUnmergeSelected.Text = (selectedNodes > 1
                ? "Unmerge Selected Scripts"
                : "Unmerge Selected Script");
        }

        #endregion

        #region TreeView Context Menu

        private void contextOpenScript_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            var file = _clickedNode.Tag as FileInfo;
            if (!file.Exists)
                MessageBox.Show("Can't find file: " + file.FullName);
            else
                System.Diagnostics.Process.Start(file.FullName);
        }

        private void contextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            var path = GetPathFromNode(_clickedNode, false);
            if (!Directory.Exists(path))
                MessageBox.Show("Can't find directory: " + path);
            else
                System.Diagnostics.Process.Start(path);
        }

        private void contextCopyPath_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            Clipboard.SetText(GetPathFromNode(_clickedNode));
        }

        private void contextSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var scriptNode in _clickedTree.Nodes.Cast<TreeNode>())
            {
                scriptNode.Checked = true;
                if (_clickedTree == treConflicts)
                {
                    foreach (var modNode in scriptNode.Nodes.Cast<TreeNode>())
                        modNode.Checked = true;
                }
            }
            if (_clickedTree == treConflicts)
                EnableMergeIfValidSelection();
            else
                EnableUnmergeIfValidSelection();
        }

        private void contextDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (var scriptNode in _clickedTree.Nodes.Cast<TreeNode>())
            {
                scriptNode.Checked = false;
                if (_clickedTree == treConflicts)
                {
                    foreach (var modNode in scriptNode.Nodes.Cast<TreeNode>())
                        modNode.Checked = false;
                }
            }

            if (_clickedTree == treConflicts)
                EnableMergeIfValidSelection();
            else
                EnableUnmergeIfValidSelection();
        }

        private void contextExpandAll_Click(object sender, EventArgs e)
        {
            _clickedTree.ExpandAll();
        }

        private void contextCollapseAll_Click(object sender, EventArgs e)
        {
            _clickedTree.CollapseAll();
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

        private void treeContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_clickedNode != null)
            {
                _clickedNode.BackColor = Color.White;
                _clickedNode.TreeView.Update();
            }
        }

        #endregion

        #region Key Input

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }

        private void treConflicts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                contextSelectAll_Click(null, null);
        }

        #endregion

        #region File/Dir Operations

        private string MoveFile(string filePath, string sourceRelRoot, string destinationRelRoot)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Can't find file: " + filePath);
                return null;
            }

            string relPath = ModDirectory.GetRelativePath(filePath, true, false);
            string destinationPath = Path.Combine(destinationRelRoot, relPath);
            string destinationDir = Path.GetDirectoryName(destinationPath);
            string fileName = Path.GetFileName(filePath);

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);
            else if (File.Exists(destinationPath)
                && (DialogResult.No == MessageBox.Show(
                        "The file below already exists! Overwrite?" + Environment.NewLine + Environment.NewLine + destinationPath,
                        "Overwrite?",
                        MessageBoxButtons.YesNo)))
                return null;

            if (File.Exists(destinationPath))  // If user didn't cancel, it's okay to delete existing file
                File.Delete(destinationPath);
            File.Move(filePath, destinationPath);

            DeleteEmptyDirs(Path.GetDirectoryName(filePath), sourceRelRoot);
            return destinationPath;
        }

        private void DeleteEmptyDirs(string dirPath, string stopPath)
        {
            if (dirPath == stopPath)
                return;
            var dirInfo = new DirectoryInfo(dirPath);
            if (dirInfo.GetFiles().Length > 0 || dirInfo.GetDirectories().Length > 0)
                return;
            Directory.Delete(dirPath);
            DeleteEmptyDirs(dirInfo.Parent.FullName, stopPath);
        }

        #endregion
    }
}