using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.Controls;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Inventory;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Forms
{
    public partial class MainForm : Form
    {
        #region Members

        public string GameDirectorySetting => txtGameDir.Text;

        public bool PathsInKdiff3Setting => menuPathsInKDiff3.Checked;

        public bool ReviewEachMergeSetting => menuReviewEach.Checked;

        public bool CompletionSoundsSetting => menuCompletionSounds.Checked;

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

        public bool ValidateCustomLoadOrderSetting
        {
            get { return menuValidateCustomLoadOrder.Checked; }
            set { menuValidateCustomLoadOrder.Checked = value; }
        }

        MergeInventory _inventory = null;
        ModFileIndex _modIndex = null;

        #endregion

        #region Form Operations

        public MainForm()
        {
            InitializeComponent();
            this.Text += " v" + Application.ProductVersion;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            Program.Settings.StartBatch();
            txtGameDir.Text = Program.Settings.Get("GameDirectory");
            menuCheckScripts.Checked = Program.Settings.Get<bool>("CheckScripts");
            menuCheckXmlFiles.Checked = Program.Settings.Get<bool>("CheckXmlFiles");
            menuCheckBundledFiles.Checked = Program.Settings.Get<bool>("CheckBundleContents");
            menuValidateCustomLoadOrder.Checked = Program.Settings.Get<bool>("ValidateCustomLoadOrder");
            menuCollapseCustomLoadOrder.Checked = Program.Settings.Get<bool>("CollapseCustomLoadOrder");
            menuCollapseNotMergeable.Checked = Program.Settings.Get<bool>("CollapseNotMergeable");
            menuReviewEach.Checked = Program.Settings.Get<bool>("ReviewEachMerge");
            menuPathsInKDiff3.Checked = Program.Settings.Get<bool>("ShowPathsInKDiff3");
            menuCompletionSounds.Checked = Program.Settings.Get<bool>("PlayCompletionSounds");
            menuMergeReport.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            menuPackReport.Checked = Program.Settings.Get<bool>("ReportAfterPack");
            menuShowStatusBar.Checked = Program.Settings.Get<bool>("ShowStatusBar");
            LoadLastWindowConfiguration();
            Program.Settings.EndBatch();
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            Update();

            var repackingBundle = false;
            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) || !Paths.IsModsDirectoryDerived)
                repackingBundle = RefreshMergeInventory();
            if (repackingBundle)
                return;

            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) ||
                (!Paths.IsScriptsDirectoryDerived && !Paths.IsModsDirectoryDerived))
                RefreshConflictsTree();
            else
                lblStatusLeft1.Text = "Please locate your 'The Witcher 3 Wild Hunt' game directory.";
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pnlProgress.Visible)
            {
                e.Cancel = true;
                return;
            }

            Program.Settings.StartBatch();
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
            Program.Settings.Set("CheckScripts", menuCheckScripts.Checked);
            Program.Settings.Set("CheckXmlFiles", menuCheckXmlFiles.Checked);
            Program.Settings.Set("CheckBundleContents", menuCheckBundledFiles.Checked);
            Program.Settings.Set("ValidateCustomLoadOrder", menuValidateCustomLoadOrder.Checked);
            Program.Settings.Set("CollapseCustomLoadOrder", menuCollapseCustomLoadOrder.Checked);
            Program.Settings.Set("CollapseNotMergeable", menuCollapseNotMergeable.Checked);
            Program.Settings.Set("ReviewEachMerge", menuReviewEach.Checked);
            Program.Settings.Set("ShowPathsInKDiff3", menuPathsInKDiff3.Checked);
            Program.Settings.Set("PlayCompletionSounds", menuCompletionSounds.Checked);
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

        void LoadLastWindowConfiguration()
        {
            var top = Program.Settings.Get<int>("StartPosTop");
            var left = Program.Settings.Get<int>("StartPosLeft");
            if (top > 0)
                Top = top;
            if (left > 0)
                Left = left;
            if (Top > 0 || Left > 0)
                StartPosition = FormStartPosition.Manual;
            
            var startWidth = Program.Settings.Get<int>("StartWidth");
            var startHeight = Program.Settings.Get<int>("StartHeight");
            if (startWidth > 0)
                Width = startWidth;
            if (startHeight > 0)
                Height = startHeight;

            if (Program.Settings.Get<bool>("StartMaximized"))
                WindowState = FormWindowState.Maximized;

            var splitterPosPct = Program.Settings.Get<int>("StartSplitterPosPct");
            if (splitterPosPct > 0)
                splitContainer.SplitterDistance = (int)(splitterPosPct / 100f * splitContainer.Width);
        }

        void txtGameDir_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
        }

        void menuShowStatusBar_Click(object sender, EventArgs e)
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

        void UpdateStatusText()
        {
            var solvableCount = treConflicts.FileNodes.Count(node => ModFile.IsTextFile(node.Text));

            if (treConflicts.IsEmpty())
                lblStatusLeft1.Text = "0 conflicts";
            else
            {
                lblStatusLeft1.Text = $"{solvableCount} mergeable";
                if (solvableCount < treConflicts.FileNodes.Count)
                {
                    lblStatusLeft2.Text = $"{treConflicts.FileNodes.Count - solvableCount} not mergeable";
                    lblStatusLeft2.Visible = true;
                }
            }

            lblStatusLeft3.Text = string.Format(
                "{0} merge{1}",
                treMerges.FileNodes.Count,
                treMerges.FileNodes.Count.GetPluralS()
                );
            lblStatusLeft3.Visible = true;

            if (_modIndex != null)
            {
                lblStatusRight.Text = string.Format(
                    "Found {0} mod{1}, {2} script{3}, {4} XML{5}, {6} bundle{7}",
                    _modIndex.ModCount,
                    _modIndex.ModCount.GetPluralS(),
                    _modIndex.ScriptCount,
                    _modIndex.ScriptCount.GetPluralS(),
                    _modIndex.XmlCount,
                    _modIndex.XmlCount.GetPluralS(),
                    _modIndex.BundleCount,
                    _modIndex.BundleCount.GetPluralS());
            }
        }

        public void EnableMergeIfValidSelection()
        {
            var validFileNodeCount = treConflicts.FileNodes.Count(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);
            btnCreateMerges.Enabled = (validFileNodeCount > 0);
            btnCreateMerges.Text = (validFileNodeCount > 1
                ? "&Create " + validFileNodeCount + " Selected Merges"
                : "&Create Selected Merge");
        }

        public void EnableUnmergeIfValidSelection()
        {
            var selectedCount = treMerges.FileNodes.Count(node => node.Checked);
            btnDeleteMerges.Enabled = (selectedCount > 0);
            btnDeleteMerges.Text = (selectedCount > 1
                ? "&Delete " + selectedCount + " Selected Merges"
                : "&Delete Selected Merge");
        }

        #endregion

        #region Refreshing Trees

        bool RefreshMergeInventory()
        {
            InitializeProgressScreen("Loading Merges");

            UpdateProgressAction("Loading MergeInventory.xml file");
            _inventory = MergeInventory.Load(Paths.Inventory);

            UpdateProgressAction("Loading mods.settings file");
            Program.LoadOrder = new CustomLoadOrder();

            if (menuValidateCustomLoadOrder.Checked && _inventory.Merges.Any())
            {
                UpdateProgressAction("Validating load order");
                LoadOrderValidator.ValidateAndFix(Program.LoadOrder);
            }

            UpdateProgressAction("Refreshing merge tree");
            return RefreshMergeTree();
        }
        
        bool RefreshMergeTree()
        {
            treMerges.Nodes.Clear();
            var changed = false;
            var bundleMergesPruned = new List<Merge>();
            var mergesToDelete = new List<Merge>();
            for (int i = _inventory.Merges.Count - 1; i >= 0; --i)
            {
                var merge = _inventory.Merges[i];
                if (!File.Exists(merge.GetMergedFile()) && ConfirmPruneMissingMergeFile(merge))
                {
                    _inventory.Merges.RemoveAt(i);
                    changed = true;

                    if (merge.IsBundleContent)
                        bundleMergesPruned.Add(merge);
                    continue;
                }
                else
                {
                    var willDelete = false;
                    foreach (var mod in merge.Mods)
                    {
                        var modFilePath = merge.GetModFile(mod.Name);
                        if (!File.Exists(modFilePath) && ConfirmDeleteMergeForMissingMod(merge, mod.Name))
                        {
                            willDelete = true;
                            break;
                        }
                        var modLoadSetting = Program.LoadOrder.GetModLoadSettingByName(mod.Name);
                        if (modLoadSetting != null && !modLoadSetting.IsEnabled.Value && ConfirmDeleteMergeForDisabledMod(merge, mod.Name))
                        {
                            willDelete = true;
                            break;
                        }
                        var latestHash = Hasher.ComputeHash(modFilePath);
                        if (latestHash != null && mod.Hash != latestHash)
                        {
                            if (ConfirmDeleteForChangedHash(merge, modFilePath, mod.Name))
                            {
                                willDelete = true;
                                break;
                            }
                        }
                    }
                    if (willDelete)
                    {
                        mergesToDelete.Add(merge);
                        continue;
                    }
                }

                var fileNode = new TreeNode(merge.RelativePath);
                fileNode.Tag = merge.GetMergedFile();
                fileNode.ForeColor = MergeTree.FileNodeForeColor;

                var categoryNode = treMerges.GetCategoryNode(merge.Category);
                if (categoryNode == null)
                {
                    categoryNode = new TreeNode(merge.Category.DisplayName);
                    categoryNode.ToolTipText = merge.Category.ToolTipText;
                    categoryNode.Tag = merge.Category;
                    treMerges.Nodes.Add(categoryNode);
                }
                categoryNode.Nodes.Add(fileNode);

                foreach (var mod in merge.Mods)
                {
                    var modNode = new TreeNode(mod.Name);
                    modNode.Tag = merge.GetModFile(mod.Name);
                    fileNode.Nodes.Add(modNode);
                }
            }
            if (mergesToDelete.Any())
            {
                if (DeleteMerges(mergesToDelete))
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
            treMerges.SetFontBold(SMTree.LevelType.Categories);
            foreach (var modNode in treMerges.ModNodes)
                modNode.SetIsCheckBoxVisible(false);

            UpdateStatusText();
            EnableUnmergeIfValidSelection();
            return false;
        }

        bool ConfirmPruneMissingMergeFile(Merge merge)
        {
            var msg =
                "Can't find the merged version of the following file.\n\n" +
                merge.RelativePath + "\n        " +
                string.Join("\n        ", merge.Mods.Select(mod => mod.Name)) + "\n\n" +
                "Expected path:\n" +
                merge.GetMergedFile() + "\n\n";

            msg += merge.IsBundleContent
                ? "Remove from Merges list & repack merged bundle?"
                : "Remove from Merges list?";

            return (DialogResult.Yes == ShowMessage(
                msg,
                "Missing Merge Inventory File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        bool ConfirmDeleteMergeForMissingMod(Merge merge, string modName)
        {
            var msg =
                $"Can't find the '{modName}' version of the following file, " +
                "perhaps because the mod was uninstalled or updated.\n\n" +
                merge.RelativePath + "\n        " +
                string.Join("\n        ", merge.Mods.Select(mod => mod.Name)) + "\n\n" +
                "Expected path:\n" +
                merge.GetModFile(modName) + "\n\n";

            msg += merge.IsBundleContent
                ? "Delete this affected merge & repack the merged bundle?"
                : "Delete this affected merge?";

            return (DialogResult.Yes == ShowMessage(
                msg,
                "Missing Merge Inventory File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        bool ConfirmDeleteMergeForDisabledMod(Merge merge, string modName)
        {
            var msg =
                $"In your custom load order, {modName} is disabled.\n" +
                "Delete the following merge that includes the disabled mod?\n\n" +
                merge.RelativePath + "\n        " +
                string.Join("\n        ", merge.Mods.Select(mod => mod.Name));

            return (DialogResult.Yes == ShowMessage(
                msg,
                "Disabled Mod in Merge",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        private bool ConfirmDeleteForChangedHash(Merge merge, string modFilePath, string modName)
        {
            var msg =
                $"The '{modName}' {(merge.IsBundleContent ? "bundle" : "version of the following file")} " +
                "is different from when it was used in a merge, perhaps because the mod has been updated.\n\n" +
                $"This file has changed:\n\n{modFilePath}\n\n" +
                $"This merge is affected:\n\n{merge.RelativePath}\n        " +
                string.Join("\n        ", merge.Mods.Select(mod => mod.Name)) + "\n\n";

            msg += merge.IsBundleContent
                ? "Delete this affected merge & repack the merged bundle?"
                : "Delete this affected merge?";

            return (DialogResult.Yes == ShowMessage(
                msg,
                "Merged Mod File Changed",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        void RefreshConflictsTree(bool checkBundles = true)
        {
            checkBundles = checkBundles && menuCheckBundledFiles.Checked;

            InitializeProgressScreen("Detecting Conflicts", ProgressBarStyle.Continuous);
            lblStatusLeft1.Text = "Refreshing...";
            lblStatusLeft2.Visible = lblStatusLeft3.Visible = false;

            if (_inventory.ScriptsChanged && _inventory.BundleChanged)
                treConflicts.Nodes.Clear();
            else
            {
                var nodesToUpdate = new List<TreeNode>();

                var scriptCatNode = treConflicts.GetCategoryNode(Categories.Script);
                if (scriptCatNode != null)
                    nodesToUpdate.Add(scriptCatNode);

                var xmlCatNode = treConflicts.GetCategoryNode(Categories.Xml);
                if (xmlCatNode != null)
                    nodesToUpdate.Add(xmlCatNode);

                if (_inventory.BundleChanged || checkBundles || !menuCheckBundledFiles.Checked)
                {
                    var bundleTextCatNode = treConflicts.GetCategoryNode(Categories.BundleText);
                    if (bundleTextCatNode != null)
                        nodesToUpdate.Add(bundleTextCatNode);
                    var bundleNotMergeableCatNode = treConflicts.GetCategoryNode(Categories.BundleNotMergeable);
                    if (bundleNotMergeableCatNode != null)
                        nodesToUpdate.Add(bundleNotMergeableCatNode);
                }

                var missingFileNodes = treConflicts.FileNodes.Where(node =>
                    node.GetTreeNodes().Any(modNode => !File.Exists(modNode.Tag as string)));
                nodesToUpdate.AddRange(missingFileNodes);

                foreach (var node in nodesToUpdate)
                    treConflicts.Nodes.Remove(node);
                foreach (var catNode in treConflicts.CategoryNodes) // Hack-fix for bug: Empty category remained on refresh after resolving conflicts outside of SM
                {
                    if (catNode.Nodes.Count == 0)
                        treConflicts.Nodes.Remove(catNode);
                }
            }

            _modIndex = new ModFileIndex();
            _modIndex.BuildAsync(_inventory,
                menuCheckScripts.Checked,
                menuCheckXmlFiles.Checked,
                checkBundles,
                OnRefreshConflictsProgressChanged,
                OnRefreshConflictsComplete);
        }

        void OnRefreshConflictsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblProgressCurrentAction.Text = e.UserState as string;

            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Normal);
            TaskbarProgress.SetValue(this.Handle, e.ProgressPercentage, 100);
        }

        void OnRefreshConflictsComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_modIndex.HasConflict)
            {
                foreach (var conflict in _modIndex.Conflicts)
                {
                    if (_inventory.HasResolvedConflict(conflict))
                        continue;

                    var fileNode = treConflicts.FileNodes.FirstOrDefault(node =>
                        node.Text.EqualsIgnoreCase(conflict.RelativePath));

                    if (fileNode == null)
                    {
                        fileNode = new TreeNode(conflict.RelativePath);
                        fileNode.Tag = (conflict.Category == Categories.Script || conflict.Category == Categories.Xml
                            ? conflict.GetVanillaFile()
                            : conflict.RelativePath);

                        var categoryNode = treConflicts.GetCategoryNode(conflict.Category);
                        if (categoryNode == null)
                        {
                            categoryNode = new TreeNode(conflict.Category.DisplayName);
                            categoryNode.ToolTipText = conflict.Category.ToolTipText;
                            categoryNode.Tag = conflict.Category;
                            treConflicts.Nodes.Add(categoryNode);
                        }
                        categoryNode.Nodes.Add(fileNode);
                    }

                    foreach (var mod in conflict.Mods)
                    {
                        var modNode = fileNode.GetTreeNodes().FirstOrDefault(node =>
                            node.Text.EqualsIgnoreCase(mod.Name));

                        if (modNode == null)
                        {
                            modNode = new TreeNode(mod.Name);
                            modNode.Tag = conflict.GetModFile(mod.Name);
                            fileNode.Nodes.Add(modNode);
                        }
                    }
                }

                treConflicts.Sort();
                treConflicts.ExpandAll();
                treConflicts.Select();
                foreach (var catNode in treConflicts.CategoryNodes)
                {
                    if (!(catNode.Tag as ModFileCategory).IsSupported)
                    {
                        catNode.SetIsCheckBoxVisible(false, true);
                        if (menuCollapseNotMergeable.Checked)
                            catNode.Collapse();
                    }
                }

                treConflicts.SetStylesForCustomLoadOrder();

                foreach (var fileNode in treConflicts.FileNodes)
                {
                    if (menuCollapseCustomLoadOrder.Checked && fileNode.ForeColor == ConflictTree.ResolvedForeColor)
                        fileNode.Collapse();
                }
            }

            treConflicts.ScrollToTop();
            treConflicts.SetFontBold(SMTree.LevelType.Categories);
            UpdateStatusText();
            HideProgressScreen();
            EnableMergeIfValidSelection();
        }

        #endregion

        #region Button Clicks

        void btnSelectGameDirectory_Click(object sender, EventArgs e)
        {
            var dirChoice = GetUserDirectoryChoice();
            if (!string.IsNullOrWhiteSpace(dirChoice))
            {
                if (dirChoice.EndsWithIgnoreCase("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                    dirChoice = Path.GetDirectoryName(dirChoice);

                txtGameDir.Text = dirChoice;
                RefreshTrees();
            }
        }

        string GetUserDirectoryChoice()
        {
            var dlgSelectRoot = new FolderBrowserDialog();
            if (Directory.Exists(txtGameDir.Text))
                dlgSelectRoot.SelectedPath = txtGameDir.Text;
            if (DialogResult.OK == dlgSelectRoot.ShowDialog())
                return dlgSelectRoot.SelectedPath;
            else
                return null;
        }

        void btnRefreshMerged_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGameDir.Text))
            {
                Program.MainForm.ShowMessage(
                    "Please locate your 'The Witcher 3 Wild Hunt' game directory.");
                return;
            }

            if (txtGameDir.Text.EndsWithIgnoreCase("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                txtGameDir.Text = Path.GetDirectoryName(txtGameDir.Text);

            if (Paths.ValidateModsDirectory())
                RefreshMergeInventory();

            HideProgressScreen();
        }

        void btnRefreshConflicts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGameDir.Text))
            {
                Program.MainForm.ShowMessage(
                    "Please locate your 'The Witcher 3 Wild Hunt' game directory.");
                return;
            }

            if (txtGameDir.Text.EndsWithIgnoreCase("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                txtGameDir.Text = Path.GetDirectoryName(txtGameDir.Text);
            
            RefreshTrees();
        }

        void btnMergeFiles_Click(object sender, EventArgs e)
        {
            if (!Paths.ValidateModsDirectory() ||
                (treConflicts.FileNodes.Any(node => ModFile.IsScript(node.Text)) && !Paths.ValidateScriptsDirectory()) ||
                (treConflicts.FileNodes.Any(node => ModFile.IsBundle(node.Text)) && !Paths.ValidateBundlesDirectory()))
                return;

            var mergedModName = Paths.RetrieveMergedModName();
            if (mergedModName == null)
                return;
            
            InitializeProgressScreen("Merging");

            _inventory = MergeInventory.Load(Paths.Inventory);

            var merger = new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete);

            var fileNodes = treConflicts.FileNodes.Where(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);

            merger.MergeByTreeNodesAsync(fileNodes, mergedModName);
        }

        void OnMergeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var mergeProgress = (MergeProgressInfo)e.UserState;
            lblProgressCurrentPhase.Text = mergeProgress.CurrentPhase;
            lblProgressCurrentAction.Text = mergeProgress.CurrentAction;
        }

        void OnMergeComplete(object sender, RunWorkerCompletedEventArgs e)
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

        void btnDeleteMerges_Click(object sender, EventArgs e)
        {
            var fileNodes = treMerges.FileNodes.Where(node => node.Checked);
            DeleteMerges(fileNodes);
        }

        void RefreshTrees(bool checkBundles = true)
        {
            if (!Paths.ValidateModsDirectory() ||
                (menuCheckScripts.Checked && !Paths.ValidateScriptsDirectory()) ||
                (menuCheckBundledFiles.Checked && !Paths.ValidateBundlesDirectory()))
                return;

            if (_inventory == null)
                RefreshMergeInventory();
            else
            {
                InitializeProgressScreen("Loading Merges");
                Program.LoadOrder.Refresh();
                RefreshMergeTree();
            }
            RefreshConflictsTree(checkBundles);
        }

        #endregion

        #region Key Input

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && btnRefreshConflicts.Enabled)
                btnRefreshConflicts_Click(null, null);
        }

        void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }
        
        void splitContainer_Panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnCreateMerges.Enabled)
                btnMergeFiles_Click(null, null);
        }

        void splitContainer_Panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && btnDeleteMerges.Enabled)
                btnDeleteMerges_Click(null, null);
        }

        #endregion

        #region Deleting Merges

        public void DeleteMerges(IEnumerable<TreeNode> fileNodes)
        {
            var merges = fileNodes.Select(node =>
                _inventory.Merges.First(merge =>
                    merge.RelativePath.EqualsIgnoreCase(node.Text)))
                    .ToList();
            DeleteMerges(merges);
        }

        bool DeleteMerges(List<Merge> merges)
        {
            var bundleMerges = new List<Merge>();
            foreach (var merge in merges)
            {
                var mergePath = merge.GetMergedFile();
                if (File.Exists(mergePath))
                {
                    File.Delete(mergePath);
                    DeleteEmptyDirs(Path.GetDirectoryName(mergePath), Paths.ScriptsDirectory);
                }
                if (merge.IsBundleContent)
                {
                    var mergesForBundle = _inventory.Merges.Where(m =>
                    m.IsBundleContent &&
                    m.MergedModName.EqualsIgnoreCase(merge.MergedModName) &&
                    m.BundleName.EqualsIgnoreCase(merge.BundleName));
                    if (mergesForBundle.All(m => merges.Contains(m)))
                    {
                        var bundlePath = merge.GetMergedBundle();
                        if (File.Exists(bundlePath))
                            File.Delete(bundlePath);

                        var metadataPath = Path.Combine(Path.GetDirectoryName(bundlePath), "metadata.store");
                        if (File.Exists(metadataPath))
                            File.Delete(metadataPath);
                        
                        DeleteEmptyDirs(Path.GetDirectoryName(bundlePath), Paths.ScriptsDirectory);
                    }
                    else if (merge.IsBundleContent)
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

        void HandleDeletedBundleMerges(List<Merge> bundleMerges)
        {
            var affectedBundles = bundleMerges.Select(merge => merge.GetMergedBundle()).Distinct();
            foreach (var bundlePath in affectedBundles)
            {
                InitializeProgressScreen("Merge Deleted");

                new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete)
                    .RepackBundleAsync(bundlePath);
            }
        }

        #endregion

        #region File/Dir Operations

        void DeleteEmptyDirs(string dirPath, string stopPath)
        {
            if (dirPath.EqualsIgnoreCase(stopPath))
                return;
            var dirInfo = new DirectoryInfo(dirPath);
            if (!dirInfo.Exists || dirInfo.GetFiles().Length > 0 || dirInfo.GetDirectories().Length > 0)
                return;
            Directory.Delete(dirPath);
            DeleteEmptyDirs(dirInfo.Parent.FullName, stopPath);
        }

        #endregion

        #region Progress Screen

        void InitializeProgressScreen(string progressOf, ProgressBarStyle style = ProgressBarStyle.Marquee)
        {
            menuStrip.Enabled
                = lblGameDir.Enabled
                = txtGameDir.Enabled
                = btnSelectGameDir.Enabled
                = splitContainer.Panel1.Enabled
                = splitContainer.Panel2.Enabled
                = false;
            progressBar.Value = 0;
            lblProgressCurrentPhase.Text = progressOf;
            lblProgressCurrentAction.Text = string.Empty;
            progressBar.Style = style;

            switch (style)
            {
                case ProgressBarStyle.Continuous:
                    TaskbarProgress.SetValue(this.Handle, 0, 100);
                    TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Normal);
                    break;
                case ProgressBarStyle.Marquee:
                    TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Indeterminate);
                    break;
            }

            pnlProgress.Visible = true;
            Update();
        }

        void UpdateProgressAction(string action)
        {
            lblProgressCurrentAction.Text = action;
            Update();
        }

        void HideProgressScreen()
        {
            pnlProgress.Visible = false;
            menuStrip.Enabled
                = lblGameDir.Enabled
                = txtGameDir.Enabled
                = btnSelectGameDir.Enabled
                = splitContainer.Panel1.Enabled
                = splitContainer.Panel2.Enabled
                = true;
            treMerges.Select();

            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
        }

        #endregion

        #region Cross-thread Operations

        public DialogResult ShowMessage(string text,
            string title = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            this.ActivateSafely();

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
            this.ActivateSafely();

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

        #region Menus

        void menuDependencies_Click(object sender, EventArgs e)
        {
            using (var dependencyForm = new DependencyForm())
            {
                ShowModal(dependencyForm);
            }
        }

        private void menuOpenLoadOrderFile_Click(object sender, EventArgs e)
        {
            Program.TryOpenFile(Program.LoadOrder.FilePath);
        }

        private void menuOpenMergedModDir_Click(object sender, EventArgs e)
        {
            Program.TryOpenDirectory(Paths.RetrieveMergedModDir());
        }

        private void menuOpenBundleContentDir_Click(object sender, EventArgs e)
        {
            Program.TryOpenDirectory(Paths.MergedBundleContent);
        }

        private void menuRepackBundle_Click(object sender, EventArgs e)
        {
            var mergedBundles = _inventory.Merges.Where(merge => merge.IsBundleContent).Select(merge => merge.GetMergedBundle()).Distinct();
            var mergedBundleCount = mergedBundles.Count();
            foreach (var bundlePath in mergedBundles)
            {
                InitializeProgressScreen($"Repacking Bundle{mergedBundleCount.GetPluralS()}");

                new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete)
                    .RepackBundleAsync(bundlePath);
            }
        }

        private void menuExitAndPlay_Click(object sender, EventArgs e)
        {
            if (Program.TryOpenFile(Paths.GameExe))
                Environment.Exit(0);
        }

        private void menuFile_DropDownOpening(object sender, EventArgs e)
        {
            menuRepackBundle.Enabled = Directory.Exists(Paths.MergedBundleContent);

            menuExitAndPlay.Enabled = File.Exists(Paths.GameExe);
        }

        private void menuOpen_DropDownOpening(object sender, EventArgs e)
        {
            menuOpenLoadOrderFile.Enabled = File.Exists(Program.LoadOrder.FilePath);

            var mergedModDir = Paths.RetrieveMergedModDir();
            menuOpenMergedModDir.Enabled = (mergedModDir != null && Directory.Exists(mergedModDir));

            menuOpenBundleContentDir.Enabled = Directory.Exists(Paths.MergedBundleContent);
        }

        #endregion
    }
}