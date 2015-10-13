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
            set { menuMergeReport.Checked = value; }
        }

        public bool PackReportSetting
        {
            get { return menuPackReport.Checked; }
            set { menuPackReport.Checked = value; }
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
            menuShowUnsolvable.Checked = Program.Settings.Get<bool>("ShowUnsolvableConflicts");
            menuReviewEach.Checked = Program.Settings.Get<bool>("ReviewEachMerge");
            menuPathsInKDiff3.Checked = Program.Settings.Get<bool>("ShowPathsInKDiff3");
            menuMergeReport.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            menuPackReport.Checked = Program.Settings.Get<bool>("ReportAfterPack");
            menuShowStatusBar.Checked = Program.Settings.Get<bool>("ShowStatusBar");
            LoadLastWindowConfiguration();
            Program.Settings.EndBatch();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Update();

            bool repackingBundle = false;
            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) || !Paths.IsModsDirectoryDerived)
                repackingBundle = RefreshMergeInventory();
            if (repackingBundle)
                return;

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
            Program.Settings.Set("ShowUnsolvableConflicts", menuShowUnsolvable.Checked);
            Program.Settings.Set("ReviewEachMerge", menuReviewEach.Checked);
            Program.Settings.Set("ShowPathsInKDiff3", menuPathsInKDiff3.Checked);
            Program.Settings.Set("ReportAfterMerge", menuMergeReport.Checked);
            Program.Settings.Set("ReportAfterPack", menuPackReport.Checked);
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

        private void txtGameDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
        }

        private void menuShowStatusBar_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = menuShowStatusBar.Checked;
            if (statusStrip.Visible)
            {
                splitContainer.Height -= statusStrip.Height;
                pnlProgress.Height -= statusStrip.Height;
            }
            else
            {
                splitContainer.Height += statusStrip.Height;
                pnlProgress.Height += statusStrip.Height;
            }
        }

        private void UpdateStatusText()
        {
            int solvableCount = treConflicts.GetTreeNodes().Count(node => ModFile.IsMergeable(node.Text));
            string conflictText = (menuShowUnsolvable.Checked
                ? string.Format("{0} solvable,  {1} unsolvable", solvableCount, (treConflicts.Nodes.Count - solvableCount))
                : string.Format("{0} conflicts", solvableCount));

            lblStatus.Text = string.Format(
                "{0}              {1} merge{2}",
                conflictText,
                treMerges.Nodes.Count,
                (treMerges.Nodes.Count == 1 ? "" : "s"));
        }

        #endregion

        #region Refreshing Trees

        private bool RefreshMergeInventory()
        {
            _inventory = MergeInventory.Load(Paths.Inventory);
            return RefreshMergeTree();
        }
        
        private bool RefreshMergeTree()
        {
            treMerges.Nodes.Clear();
            bool changed = false;
            var bundleMergesPruned = new List<Merge>();
            var missingModMerges = new List<Merge>();
            for (int i = _inventory.Merges.Count - 1; i >= 0; --i)
            {
                var merge = _inventory.Merges[i];
                if (!File.Exists(merge.GetMergedFile()) &&
                    ConfirmPruneMissingMergeFile(merge.RelativePath, merge.Type))
                {
                    _inventory.Merges.RemoveAt(i);
                    changed = true;

                    if (merge.Type == ModFileType.BundleContent)
                        bundleMergesPruned.Add(merge);
                    continue;
                }
                else
                {
                    bool missingModFile = false;
                    foreach (string modName in merge.ModNames)
                    {
                        string modFilePath = merge.GetModFile(modName);
                        if (!File.Exists(modFilePath) &&
                            ConfirmDeleteChangedMerge(merge.RelativePath, modName, merge.Type))
                        {
                            missingModFile = true;
                            break;
                        }
                    }
                    if (missingModFile)
                    {
                        missingModMerges.Add(merge);
                        continue;
                    }
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
            if (missingModMerges.Any())
            {
                if (DeleteMerges(missingModMerges))
                    return true;
            }
            if (changed)
            {
                _inventory.Save();
                if (bundleMergesPruned.Any())
                    return DeleteMerges(bundleMergesPruned);
            }
            treMerges.Sort();
            treMerges.ExpandAll();
            treMerges.ScrollToTop();
            var modNodes = treMerges.GetTreeNodes().SelectMany(node => node.GetTreeNodes()).ToList();
            foreach (var modNode in modNodes)
                modNode.HideCheckBox();

            UpdateStatusText();
            EnableUnmergeIfValidSelection();
            return false;
        }

        private bool ConfirmPruneMissingMergeFile(string filePath, ModFileType type)
        {
            string msg = "Can't find the merged version of the following file.\n\n{0}\n\nRemove from Merged Files list";
            if (type == ModFileType.BundleContent)
                msg += " & repack merged bundle";
            msg += "?";
            return (DialogResult.Yes == ShowMessage(
                string.Format(msg, filePath),
                "Missing Merge Inventory File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        private bool ConfirmDeleteChangedMerge(string filePath, string missingModName, ModFileType type)
        {
            string msg =
                "Can't find the '{0}' version of the following file, " +
                "perhaps because the mod was uninstalled or updated.\n\n{1}\n\nDelete the affected merge";
            if (type == ModFileType.BundleContent)
                msg += " & repack merged bundle";
            msg += "?";
            return (DialogResult.Yes == ShowMessage(
                string.Format(msg, missingModName, filePath),
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

                if (_inventory.ScriptsChanged || !menuCheckScripts.Checked)
                {
                    fileNodesToUpdate.AddRange(
                        fileNodes.Where(node => ModFile.IsScript(node.Text)));
                }
                if (_inventory.BundleChanged || checkBundles || !menuCheckBundleContents.Checked)
                {
                    fileNodesToUpdate.AddRange(
                        fileNodes.Where(node => !ModFile.IsScript(node.Text)));
                }
                if (!menuShowUnsolvable.Checked)
                {
                    fileNodesToUpdate.AddRange(
                        fileNodes.Where(node => !ModFile.IsMergeable(node.Text)));
                }

                var missingFileNodes = fileNodes.Where(node =>
                    !fileNodesToUpdate.Contains(node) &&
                    node.GetTreeNodes().Any(modNode => !File.Exists(modNode.Tag as string)));
                fileNodesToUpdate.AddRange(missingFileNodes);

                foreach (var node in fileNodesToUpdate)
                    treConflicts.Nodes.Remove(node);
            }

            PrepareProgressScreen("Detecting Conflicts", ProgressBarStyle.Continuous);
            pnlProgress.Visible = true;

            _modIndex = new ModFileIndex();
            _modIndex.BuildAsync(_inventory,
                menuCheckScripts.Checked,
                checkBundles,
                OnRefreshProgressChanged,
                OnRefreshComplete);
        }

        private void OnRefreshProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblProgressState.Text = e.UserState as string;
        }

        private void OnRefreshComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_modIndex.HasConflict)
            {
                foreach (var conflict in _modIndex.Conflicts)
                {
                    if (!ModFile.IsMergeable(conflict.RelativePath) && !menuShowUnsolvable.Checked)
                        continue;

                    var fileNode = treConflicts.GetTreeNodes().FirstOrDefault(node =>
                        node.Text.EqualsIgnoreCase(conflict.RelativePath));

                    if (fileNode == null)
                    {
                        fileNode = new TreeNode(conflict.RelativePath);
                        fileNode.Tag = (conflict.Type == ModFileType.BundleContent
                            ? conflict.RelativePath
                            : conflict.GetVanillaFile());
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
                foreach (var fileNode in treConflicts.GetTreeNodes())
                {
                    if (!ModFile.IsMergeable(fileNode.Text))
                    {
                        fileNode.HideCheckBox();
                        foreach (var modNode in fileNode.GetTreeNodes())
                            modNode.HideCheckBox();
                    }
                }
            }
            treConflicts.ScrollToTop();
            UpdateStatusText();
            HideProgressScreen();
            EnableMergeIfValidSelection();
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
            if (Paths.ValidateModsDirectory())
                RefreshMergeInventory();
        }

        private void btnRefreshConflicts_Click(object sender, EventArgs e)
        {
            RefreshTrees();
        }

        private void btnMergeFiles_Click(object sender, EventArgs e)
        {
            if (!Paths.ValidateModsDirectory() ||
                (treConflicts.GetTreeNodes().Any(node => ModFile.IsScript(node.Text)) && !Paths.ValidateScriptsDirectory()) ||
                (treConflicts.GetTreeNodes().Any(node => ModFile.IsBundle(node.Text)) && !Paths.ValidateBundlesDirectory()))
                return;

            string mergedModName = Paths.RetrieveMergedModName();
            if (mergedModName == null)
                return;
            
            _inventory = MergeInventory.Load(Paths.Inventory);

            var merger = new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete);

            var fileNodes = treConflicts.GetTreeNodes().Where(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);

            PrepareProgressScreen("Merging", ProgressBarStyle.Marquee);
            pnlProgress.Visible = true;

            merger.MergeByTreeNodesAsync(fileNodes, mergedModName);
        }

        private void OnMergeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string[] userState = e.UserState as string[];
            lblProgressOf.Text = userState[0];
            lblProgressState.Text = userState[1];
        }

        private void OnMergeComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_inventory.HasChanged)
            {
                _inventory.Save();
                RefreshTrees(_inventory.BundleChanged);
            }
            else
            {
                HideProgressScreen();
                EnableMergeIfValidSelection();
            }
        }

        private void btnDeleteMerges_Click(object sender, EventArgs e)
        {
            var fileNodes = treMerges.GetTreeNodes().Where(node => node.Checked);
            DeleteMerges(fileNodes);
        }

        private void RefreshTrees(bool checkBundles = true)
        {
            if (!Paths.ValidateModsDirectory() ||
                (menuCheckScripts.Checked && !Paths.ValidateScriptsDirectory()) ||
                (menuCheckBundleContents.Checked && !Paths.ValidateBundlesDirectory()))
                return;

            if (_inventory == null)
                RefreshMergeInventory();
            else
                RefreshMergeTree();
            RefreshConflictsTree(checkBundles);
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
                    if (tree == treMerges)
                    {
                         if (_clickedNode.IsLeaf())
                            _clickedNode = _clickedNode.Parent;
                    }
                    else
                    {
                        string path = (_clickedNode.IsLeaf() ? _clickedNode.Parent.Tag : _clickedNode.Tag) as string;
                        if (!ModFile.IsMergeable(path))
                        {
                            tree.EndUpdate();
                            return;
                        }
                    }
                    
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
                    if (ModFile.IsScript(filePath))
                    {
                        if (_clickedNode.IsLeaf())
                            contextOpenModScript.Available = contextOpenModScriptDir.Available = true;
                        else if (tree == treConflicts)
                            contextOpenVanillaScript.Available = contextOpenVanillaScriptDir.Available = true;
                        else
                            contextOpenMergedFile.Available = contextOpenMergedFileDir.Available = true;
                    }
                    else
                    {
                        if (_clickedNode.IsLeaf())
                            contextOpenModBundleDir.Available = true;
                        else if (tree == treMerges)
                            contextOpenMergedFile.Available = contextOpenMergedFileDir.Available = true;
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
                if (tree.GetTreeNodes().Any(fileNode => !fileNode.Checked && ModFile.IsMergeable(fileNode.Text)) ||
                    (tree == treConflicts && tree.Get2ndLevelNodes().Any(modNode => !modNode.Checked && ModFile.IsMergeable(modNode.Parent.Text))))
                    contextSelectAll.Available = true;
                if (tree.GetTreeNodes().Any(fileNode => fileNode.Checked && ModFile.IsMergeable(fileNode.Text)) ||
                    (tree == treConflicts && tree.Get2ndLevelNodes().Any(modNode => modNode.Checked && ModFile.IsMergeable(modNode.Parent.Text))))
                    contextDeselectAll.Available = true;
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
            TreeNode fileNode;
            if (!_clickedNode.IsLeaf())  // File node
            {
                fileNode = _clickedNode;
                if (tree == treConflicts)
                {
                    foreach (var modNode in _clickedNode.GetTreeNodes())
                        modNode.Checked = _clickedNode.Checked;
                }
            }
            else  // Mod node
            {
                fileNode = _clickedNode.Parent;
                fileNode.Checked = fileNode.GetTreeNodes().All(node => node.Checked);
            }
            if (sender as TreeView == treConflicts)
                EnableMergeIfValidSelection();
            else
                EnableUnmergeIfValidSelection();
        }

        private void EnableMergeIfValidSelection()
        {
            int validFileNodes = treConflicts.GetTreeNodes().Count(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);
            btnMergeFiles.Enabled = (validFileNodes > 0);
            btnMergeFiles.Text = (validFileNodes > 1
                ? "&Merge " + validFileNodes + " Selected Files"
                : "&Merge Selected File");
        }

        private void EnableUnmergeIfValidSelection()
        {
            int selectedNodes = treMerges.GetTreeNodes().Count(node => node.Checked);
            btnDeleteMerges.Enabled = (selectedNodes > 0);
            btnDeleteMerges.Text = (selectedNodes > 1
                ? "&Delete " + selectedNodes + " Selected Merges"
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
                ShowMessage("Can't find file: " + filePath);
            else
                Process.Start(filePath);
        }

        private void contextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (_clickedNode == null)
                return;
            var dirPath = Path.GetDirectoryName(_clickedNode.Tag as string);
            if (!Directory.Exists(dirPath))
                ShowMessage("Can't find directory: " + dirPath);
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

            // Find all file nodes that contain a node matching the clicked node
            var fileNodes = treMerges.GetTreeNodes().Where(fileNode =>
                fileNode.GetTreeNodes().Any(modNode =>
                    modNode.Text == _clickedNode.Text));

            DeleteMerges(fileNodes);
        }

        private void contextSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var fileNode in _clickedTree.GetTreeNodes())
            {
                if (!ModFile.IsMergeable(fileNode.Text))
                    continue;
                fileNode.Checked = true;
                if (_clickedTree == treConflicts)
                {
                    foreach (var modNode in fileNode.GetTreeNodes())
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
            foreach (var fileNode in _clickedTree.GetTreeNodes())
            {
                if (!ModFile.IsMergeable(fileNode.Text))
                    continue;
                fileNode.Checked = false;
                if (_clickedTree == treConflicts)
                {
                    foreach (var modNode in fileNode.GetTreeNodes())
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

        private void tree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    _clickedTree = sender as TreeView;
                    contextSelectAll_Click(null, null);
                }
                if (e.KeyCode == Keys.D)
                {
                    _clickedTree = sender as TreeView;
                    contextDeselectAll_Click(null, null);
                }
            }
        }

        private void splitContainer_Panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnMergeFiles.Enabled)
                btnMergeFiles_Click(null, null);
        }

        private void splitContainer_Panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && btnDeleteMerges.Enabled)
                btnDeleteMerges_Click(null, null);
        }

        #endregion

        #region Deleting Merges

        private void DeleteMerges(IEnumerable<TreeNode> fileNodes)
        {
            var merges = fileNodes.Select(node =>
                _inventory.Merges.First(merge =>
                    merge.RelativePath.EqualsIgnoreCase(node.Text)))
                    .ToList();
            DeleteMerges(merges);
        }

        private bool DeleteMerges(List<Merge> merges)
        {
            var bundleMerges = new List<Merge>();
            foreach (var merge in merges)
            {
                string mergePath = merge.GetMergedFile();
                if (File.Exists(mergePath))
                {
                    File.Delete(mergePath);
                    DeleteEmptyDirs(Path.GetDirectoryName(mergePath), Paths.ScriptsDirectory);
                }
                if (merge.Type == ModFileType.BundleContent)
                {
                    var mergesForBundle = _inventory.Merges.Where(m =>
                    m.Type == ModFileType.BundleContent &&
                    m.MergedModName.EqualsIgnoreCase(merge.MergedModName) &&
                    m.BundleName.EqualsIgnoreCase(merge.BundleName));
                    if (mergesForBundle.All(m => merges.Contains(m)))
                    {
                        string bundlePath = merge.GetMergedBundle();
                        if (File.Exists(bundlePath))
                            File.Delete(bundlePath);

                        string metadataPath = Path.Combine(Path.GetDirectoryName(bundlePath), "metadata.store");
                        if (File.Exists(metadataPath))
                            File.Delete(metadataPath);
                        
                        DeleteEmptyDirs(Path.GetDirectoryName(bundlePath), Paths.ScriptsDirectory);
                    }
                    else if (merge.Type == ModFileType.BundleContent)
                        bundleMerges.Add(merge);
                }

                _inventory.Merges.Remove(merge);
            }
            if (_inventory.HasChanged)
            {
                _inventory.Save();
                if (bundleMerges.Count > 0)
                {
                    HandleDeletedBundleMerges(bundleMerges);
                    return true;
                }
                // If mod index is null, we haven't refreshed it for the 1st time yet. Don't do it here.
                if (_modIndex != null)
                    RefreshTrees(_inventory.BundleChanged);
            }
            return false;
        }

        private void HandleDeletedBundleMerges(List<Merge> bundleMerges)
        {
            var affectedBundles = bundleMerges.Select(merge => merge.GetMergedBundle()).Distinct();
            foreach (var bundlePath in affectedBundles)
            {
                PrepareProgressScreen("Merge Deleted — Repacking Bundle", ProgressBarStyle.Marquee);
                pnlProgress.Visible = true;
                new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete)
                    .PackNewBundleAsync(bundlePath);
            }
        }

        private void OnDeleteMergeComplete()
        {
            RefreshTrees();
        }

        #endregion

        #region File/Dir Operations

        private void DeleteEmptyDirs(string dirPath, string stopPath)
        {
            if (dirPath.EqualsIgnoreCase(stopPath))
                return;
            var dirInfo = new DirectoryInfo(dirPath);
            if (dirInfo.GetFiles().Length > 0 || dirInfo.GetDirectories().Length > 0)
                return;
            Directory.Delete(dirPath);
            DeleteEmptyDirs(dirInfo.Parent.FullName, stopPath);
        }

        #endregion

        #region Progress Screen

        private void PrepareProgressScreen(string progressOf, ProgressBarStyle style)
        {
            grpGameDir.Enabled = splitContainer.Panel1.Enabled = splitContainer.Panel2.Enabled = false;
            progressBar.Value = 0;
            lblProgressOf.Text = progressOf;
            progressBar.Style = style;
        }

        private void HideProgressScreen()
        {
            pnlProgress.Visible = false;
            grpGameDir.Enabled = splitContainer.Panel1.Enabled = splitContainer.Panel2.Enabled = true;
            treMerges.Select();
        }

        #endregion

        #region Cross-thread Operations

        public DialogResult ShowMessage(string text,
            string title = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            if (this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(new Func<DialogResult>(
                    () => { return MessageBox.Show(this, text, title, buttons, icon); }));
            }
            else
            {
                return MessageBox.Show(this, text, title, buttons, icon);
            }
        }

        public DialogResult ShowModal(Form form)
        {
            if (this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(new Func<DialogResult>(
                    () => { return form.ShowDialog(this); }));
            }
            else
            {
                return form.ShowDialog(this);
            }
        }

        public void ActivateSafely()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                    {
                        this.Activate();
                    });
            }
            else
                this.Activate();
        }

        #endregion

        private void menuDependencies_Click(object sender, EventArgs e)
        {
            using (var dependencyForm = new DependencyForm())
            {
                ShowModal(dependencyForm);
            }
        }
    }
}