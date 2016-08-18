using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WitcherScriptMerger.Controls;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Inventory;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Forms
{
    partial class MainForm : Form
    {
        #region Members

        public string GameDirectorySetting => txtGameDir.Text;

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
            txtGameDir.Text = Program.Settings.Get("GameDirectory");
            LoadLastWindowConfiguration();
        }

        async void MainForm_Shown(object sender, EventArgs e)
        {
            Update();

            var repackingBundle = false;
            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) || !Paths.IsModsDirectoryDerived)
                repackingBundle = await RefreshMergeInventory();
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

            Program.Settings.Set("GameDirectory", txtGameDir.Text);

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
            Program.Settings.Save();
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

        async Task<bool> RefreshMergeInventory()
        {
            InitializeProgressScreen("Loading Merges", ProgressBarStyle.Continuous);

            lblProgressCurrentAction.Text = "Loading MergeInventory.xml file";
            Program.Inventory = await Task.Run(() =>
                MergeInventory.Load(Paths.Inventory)
            );
            progressBar.Value = 25;

            lblProgressCurrentAction.Text = "Loading mods.settings file";
            Program.LoadOrder = await Task.Run(() =>
                new CustomLoadOrder()
            );
            progressBar.Value = 50;

            if (Program.Settings.Get<bool>("ValidateCustomLoadOrder") && Program.Inventory.Merges.Any())
            {
                lblProgressCurrentAction.Text = "Validating load order";
                await Task.Run(() =>
                    LoadOrderValidator.ValidateAndFix(Program.LoadOrder)
                );
            }
            progressBar.Value = 75;

            lblProgressCurrentAction.Text = "Refreshing merge tree";
            return await Task.Run(() =>
                RefreshMergeTree()
            );
        }
        
        bool RefreshMergeTree()
        {
            this.Invoke((MethodInvoker)delegate
            {
                treMerges.Nodes.Clear();
            });
            var changed = false;
            var bundleMergesPruned = new List<Merge>();
            var mergesToDelete = new List<Merge>();
            for (int i = Program.Inventory.Merges.Count - 1; i >= 0; --i)
            {
                var merge = Program.Inventory.Merges[i];
                if (!File.Exists(merge.GetMergedFile()) && ConfirmPruneMissingMergeFile(merge))
                {
                    Program.Inventory.Merges.RemoveAt(i);
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
                        var latestHash = Tools.Hasher.ComputeHash(modFilePath);
                        if (latestHash != null && mod.Hash != latestHash)
                        {
                            mod.IsOutdated = true;
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

                this.Invoke((MethodInvoker)delegate
                {
                    var fileNode = new TreeNode
                    {
                        Text = merge.RelativePath,
                        ForeColor = MergeTree.FileNodeForeColor,
                        Tag = new MergeTree.NodeMetadata
                        {
                            FilePath = merge.GetMergedFile(),
                            ModFile = merge
                        }
                    };

                    var categoryNode = treMerges.GetCategoryNode(merge.Category);
                    if (categoryNode == null)
                    {
                        categoryNode = new TreeNode
                        {
                            Text = merge.Category.DisplayName,
                            ToolTipText = merge.Category.ToolTipText,
                            Tag = merge.Category
                        };
                        treMerges.Nodes.Add(categoryNode);
                    }
                    categoryNode.Nodes.Add(fileNode);

                    foreach (var mod in merge.Mods)
                    {
                        fileNode.Nodes.Add(
                            new TreeNode
                            {
                                Text = mod.Name,
                                Tag = new MergeTree.NodeMetadata
                                {
                                    FilePath = merge.GetModFile(mod.Name),
                                    FileHash = mod,
                                    ModFile = merge
                                }
                            }
                        );
                    }
                });
            }
            if (mergesToDelete.Any())
            {
                if (DeleteMerges(mergesToDelete))
                    return true;
            }
            if (changed)
            {
                Program.Inventory.Save();
                if (bundleMergesPruned.Any())
                    return DeleteMerges(bundleMergesPruned);
            }
            this.Invoke((MethodInvoker)delegate
            {
                treMerges.Sort();
                treMerges.ExpandAll();
                treMerges.ScrollToTop();
                treMerges.SetFontBold(SMTree.LevelType.Categories);
                foreach (var modNode in treMerges.ModNodes)
                    modNode.SetIsCheckBoxVisible(false);

                UpdateStatusText();
                EnableUnmergeIfValidSelection();

                progressBar.Value = 100;
            });
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
            checkBundles = checkBundles && Program.Settings.Get<bool>("CheckBundleContents");

            InitializeProgressScreen("Detecting Conflicts", ProgressBarStyle.Continuous);
            lblStatusLeft1.Text = "Refreshing...";
            lblStatusLeft2.Visible = lblStatusLeft3.Visible = false;

            if (Program.Inventory.ScriptsChanged && Program.Inventory.BundleChanged)
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

                if (Program.Inventory.BundleChanged || checkBundles || !Program.Settings.Get<bool>("CheckBundleContents"))
                {
                    var bundleTextCatNode = treConflicts.GetCategoryNode(Categories.BundleText);
                    if (bundleTextCatNode != null)
                        nodesToUpdate.Add(bundleTextCatNode);
                    var bundleNotMergeableCatNode = treConflicts.GetCategoryNode(Categories.BundleNotMergeable);
                    if (bundleNotMergeableCatNode != null)
                        nodesToUpdate.Add(bundleNotMergeableCatNode);
                }

                var missingFileNodes = treConflicts.FileNodes.Where(node =>
                    node.GetTreeNodes().Any(modNode =>
                        !File.Exists(modNode.GetMetadata().FilePath)
                    )
                );
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
            _modIndex.BuildAsync(
                Program.Settings.Get<bool>("CheckScripts"),
                Program.Settings.Get<bool>("CheckXmlFiles"),
                Program.Settings.Get<bool>("CheckBundleContents"),
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
                    if (Program.Inventory.HasResolvedConflict(conflict))
                        continue;

                    var fileNode = treConflicts.FileNodes.FirstOrDefault(node =>
                        node.Text.EqualsIgnoreCase(conflict.RelativePath));

                    if (fileNode == null)
                    {
                        fileNode = new TreeNode
                        {
                            Text = conflict.RelativePath,
                            Tag = new SMTree.NodeMetadata
                            {
                                FilePath = (conflict.Category == Categories.Script || conflict.Category == Categories.Xml
                                    ? conflict.GetVanillaFile()
                                    : conflict.RelativePath),
                                ModFile = conflict
                            }
                        };

                        var categoryNode = treConflicts.GetCategoryNode(conflict.Category);
                        if (categoryNode == null)
                        {
                            categoryNode = new TreeNode
                            {
                                Text = conflict.Category.DisplayName,
                                ToolTipText = conflict.Category.ToolTipText,
                                Tag = conflict.Category
                            };
                            treConflicts.Nodes.Add(categoryNode);
                        }
                        categoryNode.Nodes.Add(fileNode);
                    }

                    var merge = Program.Inventory.Merges.FirstOrDefault(mrg => mrg.RelativePath.EqualsIgnoreCase(conflict.RelativePath));
                    foreach (var mod in conflict.Mods)
                    {
                        var mergeModHash = merge?.Mods.FirstOrDefault(m => m.Name.EqualsIgnoreCase(mod.Name));
                        if (mergeModHash != null && !mergeModHash.IsOutdated)
                            continue;

                        var modNode = fileNode.GetTreeNodes().FirstOrDefault(node =>
                            node.Text.EqualsIgnoreCase(mod.Name));

                        if (modNode == null)
                        {
                            modNode = new TreeNode
                            {
                                Text = mod.Name,
                                Tag = new SMTree.NodeMetadata
                                {
                                    FilePath = conflict.GetModFile(mod.Name),
                                    FileHash = mergeModHash,
                                    ModFile = conflict
                                }
                            };
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
                        if (Program.Settings.Get<bool>("CollapseNotMergeable"))
                            catNode.Collapse();
                    }
                }

                treConflicts.SetStylesForCustomLoadOrder();

                foreach (var fileNode in treConflicts.FileNodes)
                {
                    if (Program.Settings.Get<bool>("CollapseCustomLoadOrder") && fileNode.ForeColor == ConflictTree.ResolvedForeColor)
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

        async void btnRefreshMerged_Click(object sender, EventArgs e)
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
                await RefreshMergeInventory();

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

            Program.Inventory = MergeInventory.Load(Paths.Inventory);

            var merger = new FileMerger(Program.Inventory, OnMergeProgressChanged, OnMergeComplete);

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
            if (Program.Inventory.HasChanged)
            {
                Program.Inventory.Save();
                RefreshTrees(Program.Inventory.BundleChanged);
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

        async void RefreshTrees(bool checkBundles = true)
        {
            if (!Paths.ValidateModsDirectory() ||
                (Program.Settings.Get<bool>("CheckScripts") && !Paths.ValidateScriptsDirectory()) ||
                (Program.Settings.Get<bool>("CheckBundleContents") && !Paths.ValidateBundlesDirectory()))
                return;

            if (Program.Inventory == null)
                await RefreshMergeInventory();
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
                Program.Inventory.Merges.First(merge =>
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
                    var mergesForBundle = Program.Inventory.Merges.Where(m =>
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

                Program.Inventory.Merges.Remove(merge);
            }
            if (Program.Inventory.HasChanged)
            {
                Program.Inventory.Save();
                if (bundleMerges.Count > 0)
                {
                    HandleDeletedBundleMerges(bundleMerges);
                    return true;
                }
                // If mod index is null, we haven't refreshed it for the 1st time yet. Don't do it here.
                if (_modIndex != null)
                    RefreshTrees(Program.Inventory.BundleChanged);
            }
            return false;
        }

        void HandleDeletedBundleMerges(List<Merge> bundleMerges)
        {
            var affectedBundles = bundleMerges.Select(merge => merge.GetMergedBundle()).Distinct();
            foreach (var bundlePath in affectedBundles)
            {
                InitializeProgressScreen("Merge Deleted");

                new FileMerger(Program.Inventory, OnMergeProgressChanged, OnMergeComplete)
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

        public DialogResult ShowError(string text, string title = "Error")
        {
            return ShowMessage(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public DialogResult ShowModal(Form form)
        {
            this.ActivateSafely();

            if (this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(
                    new Func<DialogResult>(
                        () => { return form.ShowDialog(this); }
                    )
                );
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

        private void menuOptions_Click(object sender, EventArgs e)
        {
            using (var optionsForm = new OptionsForm())
            {
                ShowModal(optionsForm);
            }
        }

        private void menuRepackBundle_Click(object sender, EventArgs e)
        {
            var mergedBundles = Program.Inventory.Merges.Where(merge => merge.IsBundleContent).Select(merge => merge.GetMergedBundle()).Distinct();
            var mergedBundleCount = mergedBundles.Count();
            foreach (var bundlePath in mergedBundles)
            {
                InitializeProgressScreen($"Repacking Bundle{mergedBundleCount.GetPluralS()}");

                new FileMerger(Program.Inventory, OnMergeProgressChanged, OnMergeComplete)
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