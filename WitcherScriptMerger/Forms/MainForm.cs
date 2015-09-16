using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.Forms
{
    public partial class MainForm : Form
    {
        #region Members

        public string GameDirectorySetting
        {
            get { return txtGameDir.Text; }
        }

        public bool PathsInKdiff3Setting
        {
            get { return menuPathsInKDiff3.Checked; }
        }

        public bool ReviewEachMergeSetting
        {
            get { return menuReviewEach.Checked; }
        }

        public bool MergeReportSetting
        {
            get { return menuMergeReport.Checked; }
        }

        private MergeInventory _inventory = null;
        private ModFileIndex _modIndex = null;
        private TreeView _clickedTree = null;
        private TreeNode _clickedNode = null;

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
            menuCheckScripts.Checked = Program.Settings.Get<bool>("CheckScripts");
            menuCheckBundleContents.Checked = Program.Settings.Get<bool>("CheckBundleContents");
            menuReviewEach.Checked = Program.Settings.Get<bool>("ReviewEachMerge");
            menuPathsInKDiff3.Checked = Program.Settings.Get<bool>("ShowPathsInKDiff3");
            menuMergeReport.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            menuShowStatusBar.Checked = Program.Settings.Get<bool>("ShowStatusBar");

            LoadLastWindowConfiguration();
            Program.Settings.EndBatch();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Update();

            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) || !Paths.IsModsDirectoryDerived)
                RefreshMergeInventory();

            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) ||
                (!Paths.IsScriptsDirectoryDerived && !Paths.IsModsDirectoryDerived))
                RefreshConflictsTree();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.StartBatch();
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
            Program.Settings.Set("CheckScripts", menuCheckScripts.Checked);
            Program.Settings.Set("CheckBundleContents", menuCheckBundleContents.Checked);
            Program.Settings.Set("ReviewEachMerge", menuReviewEach.Checked);
            Program.Settings.Set("ShowPathsInKDiff3", menuPathsInKDiff3.Checked);
            Program.Settings.Set("ReportAfterMerge", menuMergeReport.Checked);
            Program.Settings.Set("ShowStatusBar", menuShowStatusBar.Checked);
            statusStrip.Visible = menuShowStatusBar.Checked;

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
            int top = Program.Settings.Get<int>("StartPosTop");
            int left = Program.Settings.Get<int>("StartPosLeft");
            if (top > 0)
                Top = top;
            if (left > 0)
                Left = left;
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

            int splitterPosPct = Program.Settings.Get<int>("StartSplitterPosPct");
            if (splitterPosPct > 0)
                splitContainer.SplitterDistance = (int)(splitterPosPct / 100f * splitContainer.Width);
        }

        private void RefreshMergeInventory()
        {
            _inventory = MergeInventory.Load(Paths.Inventory);
            RefreshMergeTree();
        }
        
        private void RefreshMergeTree()
        {
            btnDeleteMerges.Enabled = false;
            treMerges.Nodes.Clear();
            bool changed = false;
            for (int i = _inventory.Merges.Count - 1; i >= 0; --i)
            {
                var merge = _inventory.Merges[i];
                if (!File.Exists(merge.GetMergedFile()) &&
                    ConfirmPruneMissingMergeFile(Path.GetFileName(merge.RelativePath)))
                {
                    _inventory.Merges.RemoveAt(i);
                    changed = true;
                    continue;
                }
                var fileNode = new TreeNode(merge.RelativePath);
                fileNode.Tag = merge.GetMergedFile();

                fileNode.ForeColor = Color.Blue;
                treMerges.Nodes.Add(fileNode);

                foreach (var modName in merge.ModNames)
                {
                    var modNode = new TreeNode(modName);
                    modNode.Tag = merge.GetModFile(modName);
                    fileNode.Nodes.Add(modNode);
                }
            }
            if (changed)
                _inventory.Save(Paths.Inventory);
            treMerges.Sort();
            treMerges.ExpandAll();
            var modNodes = treMerges.GetTreeNodes().SelectMany(node => node.GetTreeNodes()).ToList();
            foreach (var modNode in modNodes)
                modNode.HideCheckBox();

            UpdateStatusText();
        }

        private bool ConfirmPruneMissingMergeFile(string mergedFileName)
        {
            return (DialogResult.Yes == MessageBox.Show(
                string.Format("Can't find the merged version of {0}.\n\nRemove from Merged Scripts list?", mergedFileName),
                "Missing Merge Inventory File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        private void RefreshConflictsTree(bool checkBundles = true)
        {
            checkBundles = checkBundles && menuCheckBundleContents.Checked;

            if (_inventory.ScriptsChanged && _inventory.BundleChanged)
                treConflicts.Nodes.Clear();
            else
            {
                var fileNodes = treConflicts.GetTreeNodes();
                var fileNodesToUpdate = new List<TreeNode>();
                if (_inventory.ScriptsChanged)
                    fileNodesToUpdate.AddRange(fileNodes.Where(node => node.Text.EndsWith(".ws")));
                if (_inventory.BundleChanged)
                    fileNodesToUpdate.AddRange(fileNodes.Where(node => !node.Text.EndsWith(".ws")));
                
                foreach (var node in fileNodesToUpdate)
                    treConflicts.Nodes.Remove(node);
            }

            btnRefreshConflicts.Enabled = btnMergeScripts.Enabled = false;
            treConflicts.BackColor = pnlRefreshProgress.BackColor;
            refreshBar.Value = 0;
            if (checkBundles)
                pnlRefreshProgress.Visible = true;

            _modIndex = new ModFileIndex();
            _modIndex.BuildAsync(_inventory,
                checkBundles,
                OnRefreshProgressChanged,
                OnRefreshComplete);
        }

        private void OnRefreshProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            refreshBar.Value = e.ProgressPercentage;
            lblRefreshProgress.Text = e.UserState as string;
        }

        private void OnRefreshComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_modIndex.HasConflict)
            {
                foreach (var conflict in _modIndex.Conflicts)
                {
                    var fileNode = treConflicts.GetTreeNodes().FirstOrDefault(node =>
                        node.Text.EqualsIgnoreCase(conflict.RelativePath));

                    if (fileNode == null)
                    {
                        fileNode = new TreeNode(conflict.RelativePath);
                        fileNode.Tag = conflict.GetVanillaFile();
                        fileNode.ForeColor = Color.Red;
                        treConflicts.Nodes.Add(fileNode);
                    }
                    foreach (string modName in conflict.ModNames)
                    {
                        if (fileNode.GetTreeNodes().Any(node => node.Text.EqualsIgnoreCase(modName)))
                            continue;
                        var modNode = new TreeNode(modName);
                        modNode.Tag = conflict.GetModFile(modName);
                        fileNode.Nodes.Add(modNode);
                    }
                }
                treConflicts.Sort();
                treConflicts.ExpandAll();
                treConflicts.Select();
            }
            pnlRefreshProgress.Visible = false;
            treConflicts.BackColor = Color.White;
            _modIndex = null;
            UpdateStatusText();
            btnRefreshConflicts.Enabled = true;
        }

        private void txtGameDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
        }

        private void menuShowStatusBar_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = menuShowStatusBar.Checked;
            if (statusStrip.Visible)
                splitContainer.Height -= statusStrip.Height;
            else
                splitContainer.Height += statusStrip.Height;
        }

        private void UpdateStatusText()
        {
            lblStatus.Text = string.Format("{0} conflicts     {1} merges",
                treConflicts.Nodes.Count,
                treMerges.Nodes.Count);
        }

        #endregion

        #region Button Clicks

        private void btnSelectGameDirectory_Click(object sender, EventArgs e)
        {
            string dirChoice = GetUserDirectoryChoice();
            if (!string.IsNullOrWhiteSpace(dirChoice))
            {
                txtGameDir.Text = dirChoice;
                RefreshTrees();
            }
        }

        private string GetUserDirectoryChoice()
        {
            FolderBrowserDialog dlgSelectRoot = new FolderBrowserDialog();
            if (Directory.Exists(txtGameDir.Text))
                dlgSelectRoot.SelectedPath = txtGameDir.Text;
            if (DialogResult.OK == dlgSelectRoot.ShowDialog())
                return dlgSelectRoot.SelectedPath;
            else
                return null;
        }

        private void btnRefreshMerged_Click(object sender, EventArgs e)
        {
            if (ValidateModsDirectory())
                RefreshMergeInventory();
        }

        private void btnRefreshConflicts_Click(object sender, EventArgs e)
        {
            RefreshTrees();
        }

        private void btnMergeScripts_Click(object sender, EventArgs e)
        {
            if (!ValidateModsDirectory() ||
                (treConflicts.GetTreeNodes().Any(node => node.Text.EndsWith(".ws")) && !ValidateScriptsDirectory()) ||
                (treConflicts.GetTreeNodes().Any(node => node.Text.EndsWith(".bundles")) && !ValidateBundlesDirectory()))
                return;

            string mergedModName = Program.Settings.Get("MergedModName");
            if (string.IsNullOrWhiteSpace(mergedModName))
            {
                MessageBox.Show("The MergedModName setting isn't configured in the .config file.");
                return;
            }
            if (mergedModName.Length > 64)
                mergedModName = mergedModName.Substring(0, 64);
            if (!mergedModName.IsAlphaNumeric() || !mergedModName.StartsWith("mod"))
            {
                if (!ConfirmInvalidModName(mergedModName))
                    return;
            }

            _inventory = MergeInventory.Load(Paths.Inventory);

            var merger = new FileMerger(_inventory);

            var fileNodes = treConflicts.GetTreeNodes().Where(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);

            merger.MergeByTreeNodes(fileNodes, mergedModName);

            if (_inventory.HasChanged)
            {
                _inventory.Save(Paths.Inventory);
                RefreshTrees(_inventory.BundleChanged);
            }
        }

        private bool ConfirmInvalidModName(string mergedModName)
        {
            return (DialogResult.Yes == MessageBox.Show(
                "The Witcher 3 won't load the merged script if the mod name isn't \"mod\" followed by numbers, letters, or underscores.\n\nUse this name anyway?\n" + mergedModName
                + "\n\nTo change the name: Click No, then edit \"MergedModName\" in the .config file.",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        private void btnDeleteMerges_Click(object sender, EventArgs e)
        {
            var scriptNodes = treMerges.GetTreeNodes().Where(node => node.Checked);
            DeleteMerges(scriptNodes);
        }

        private void DeleteMerges(IEnumerable<TreeNode> fileNodes)
        {
            foreach (var fileNode in fileNodes)
            {
                var mergePath = fileNode.Tag as string;

                if (File.Exists(mergePath))
                {
                    File.Delete(mergePath);
                    DeleteEmptyDirs(Path.GetDirectoryName(mergePath), Paths.ScriptsDirectory);
                }

                var merge = _inventory.Merges.FirstOrDefault(ms =>
                    mergePath.Contains(ms.MergedModName) &&
                    mergePath.Contains(ms.RelativePath));

                if (merge != null)
                    _inventory.Merges.Remove(merge);
            }
            _inventory.Save(Paths.Inventory);
            if (_inventory.HasChanged)
                RefreshTrees(_inventory.BundleChanged);
        }

        private void RefreshTrees(bool checkBundles = true)
        {
            if (ValidateModsDirectory() &&
                (!menuCheckScripts.Checked || ValidateScriptsDirectory()) &&
                (!menuCheckBundleContents.Checked || ValidateBundlesDirectory()))
            {
                RefreshMergeTree();
                RefreshConflictsTree(checkBundles);
            }
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
                if (_clickedNode != null)
                {
                    if (tree == treMerges && _clickedNode.IsLeaf())
                        _clickedNode = _clickedNode.Parent;
                    
                    _clickedNode.Checked = !_clickedNode.Checked;
                    HandleCheckedChange(sender);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                SetContextMenuItems(tree);

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

        private void SetContextMenuItems(TreeView tree)
        {
            foreach (var menuItem in treeContextMenu.Items.OfType<ToolStripItem>())
                menuItem.Available = false;
            
            if (_clickedNode != null)
            {
                contextCopyPath.Available = true;
                if (_clickedNode.Tag != null)
                {
                    string filePath = _clickedNode.Tag as string;
                    if (filePath.EndsWith(".ws"))
                    {
                        if (_clickedNode.IsLeaf())
                            contextOpenModScript.Available = contextOpenModScriptDir.Available = true;
                        else if (tree == treConflicts)
                            contextOpenVanillaScript.Available = contextOpenVanillaScriptDir.Available = true;
                        else
                            contextOpenMergedScript.Available = contextOpenMergedScriptDir.Available = true;
                    }
                    else
                    {
                        if (_clickedNode.IsLeaf())
                            contextOpenModBundleDir.Available = true;
                        else if (tree == treConflicts)
                            contextOpenVanillaBundleDir.Available = true;
                        else
                            contextOpenMergedBundleDir.Available = true;
                    }
                }

                if (tree == treMerges)
                {
                    contextDeleteMerge.Available = contextDeleteSeparator.Available = true;
                    if (_clickedNode.IsLeaf())
                    {
                        contextDeleteAssociatedMerges.Available = true;
                        contextDeleteAssociatedMerges.Text = string.Format("Delete All {0} Merges", _clickedNode.Text);
                    }
                }
            }

            // If can copy path, need separator above Select/Deselect All
            if (contextCopyPath.Available)
                contextOpenSeparator.Visible = true;

            if (!tree.IsEmpty())
            {
                if (tree.GetTreeNodes().Any(node => !node.Checked))     contextSelectAll.Available = true;
                if (tree.GetTreeNodes().Any(node =>  node.Checked))     contextDeselectAll.Available = true;
                if (tree.GetTreeNodes().Any(node => !node.IsExpanded))  contextExpandAll.Available = true;
                if (tree.GetTreeNodes().Any(node =>  node.IsExpanded))  contextCollapseAll.Available = true;
            }

            if (treeContextMenu.Items.OfType<ToolStripItem>().Any(item => item.Available))
            {
                int width = treeContextMenu.Items.OfType<ToolStripMenuItem>().Where(item => item.Available)
                    .Max(item => TextRenderer.MeasureText(item.Text, item.Font).Width);
                int height = treeContextMenu.GetAvailableItems()
                    .Sum(item => item.Height);
                treeContextMenu.Width = width + 45;
                treeContextMenu.Height = height + 5;
            }
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
            if (!_clickedNode.IsLeaf())  // Script node
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
            btnMergeScripts.Enabled = (validScriptNodes > 0);
            btnMergeScripts.Text = (validScriptNodes > 1
                ? "&Merge Selected Files"
                : "&Merge Selected File");
        }

        private void EnableUnmergeIfValidSelection()
        {
            int selectedNodes = treMerges.GetTreeNodes().Count(node => node.Checked);
            btnDeleteMerges.Enabled = (selectedNodes > 0);
            btnDeleteMerges.Text = (selectedNodes > 1
                ? "&Delete Selected Merges"
                : "&Delete Selected Merge");
        }

        #endregion

        #region TreeView Context Menu

        private void contextOpenScript_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            string filePath = _clickedNode.Tag as string;
            if (!File.Exists(filePath))
                MessageBox.Show("Can't find file: " + filePath);
            else
                Process.Start(filePath);
        }

        private void contextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            var dirPath = Path.GetDirectoryName(_clickedNode.Tag as string);
            if (!Directory.Exists(dirPath))
                MessageBox.Show("Can't find directory: " + dirPath);
            else
                Process.Start(dirPath);
        }

        private void contextCopyPath_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            Clipboard.SetText(_clickedNode.Tag as string);
        }

        private void contextDeleteMerge_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null || _clickedTree != treMerges)
                return;

            if (_clickedNode.IsLeaf())
                DeleteMerges(new TreeNode[] { _clickedNode.Parent });
            else
                DeleteMerges(new TreeNode[] { _clickedNode });
        }

        private void contextDeleteAssociatedMerges_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null || _clickedTree != treMerges || !_clickedNode.IsLeaf())
                return;

            // Find all script nodes that contain a node matching the clicked node
            var scriptNodes = treMerges.GetTreeNodes().Where(scrNode =>
                scrNode.GetTreeNodes().Any(modNode =>
                    modNode.Text == _clickedNode.Text));

            DeleteMerges(scriptNodes);
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

        private void treeContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_clickedNode != null)
            {
                _clickedNode.BackColor = Color.Transparent;
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

        private void treMergeInventory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && btnDeleteMerges.Enabled)
                btnDeleteMerges_Click(null, null);
        }

        private void splitContainer_Panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnMergeScripts.Enabled)
                btnMergeScripts_Click(null, null);
        }

        #endregion

        #region File/Dir Operations

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

        private bool ValidateModsDirectory()
        {
            if (!Directory.Exists(Paths.ModsDirectory))
            {
                MessageBox.Show(!Paths.IsModsDirectoryDerived
                    ? "Can't find the Mods directory specified in the config file."
                    : "Can't find Mods directory in the specified game directory.");
                return false;
            }
            return true;
        }

        private bool ValidateScriptsDirectory()
        {
            if (!Directory.Exists(Paths.ScriptsDirectory))
            {
                MessageBox.Show(!Paths.IsScriptsDirectoryDerived
                    ? "Can't find the Scripts directory specified in the config file."
                    : "Can't find \\content\\content0\\scripts directory in the specified game directory.\n\n" +
                      "It was added in patch 1.08.1 and should contain the game's vanilla scripts. If you don't have it, try this workaround:\n\n" +
                      "1) Download the official mod tools from Nexus Mods and install them.\n" +
                      "2) In the mod tools folder, open the 'r4data' folder.\n" +
                      "3) Copy the 'scripts' folder to " + Path.Combine(Paths.GameDirectory, "content\\content0") + ".\n" +
                      "4) Optional: Uninstall the mod tools.");
                return false;
            }
            return true;
        }

        private bool ValidateBundlesDirectory()
        {
            if (!Directory.Exists(Paths.BundlesDirectory))
            {
                MessageBox.Show(!Paths.IsBundlesDirectoryDerived
                    ? "Can't find the Bundles directory specified in the config file."
                    : "Can't find \\content\\content0\\bundles directory in the specified game directory.");
                return false;
            }
            return true;
        }

        #endregion
    }
}