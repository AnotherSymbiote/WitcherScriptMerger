using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.Controls;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.Forms
{
    public partial class MainForm : Form
    {
        #region Members

        public string GameDirectorySetting => txtGameDir.Text;

        public bool PathsInKdiff3Setting => menuPathsInKDiff3.Checked;

        public bool ReviewEachMergeSetting => menuReviewEach.Checked;

        public bool CompletionSoundsSetting
        {
            get { return menuCompletionSounds.Checked; }
            set { menuMergeReport.Checked = value; }
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
            menuCollapseUnsupported.Checked = Program.Settings.Get<bool>("CollapseUnsupported");
            menuReviewEach.Checked = Program.Settings.Get<bool>("ReviewEachMerge");
            menuPathsInKDiff3.Checked = Program.Settings.Get<bool>("ShowPathsInKDiff3");
            menuCompletionSounds.Checked = Program.Settings.Get<bool>("PlayCompletionSounds");
            menuMergeReport.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            menuPackReport.Checked = Program.Settings.Get<bool>("ReportAfterPack");
            menuShowStatusBar.Checked = Program.Settings.Get<bool>("ShowStatusBar");
            LoadLastWindowConfiguration();
            Program.Settings.EndBatch();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Update();

            bool repackingBundle = false;
            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) || !Paths.IsModsDirectoryDerived)
                repackingBundle = RefreshMergeInventory();
            if (repackingBundle)
                return;

            if (!string.IsNullOrWhiteSpace(txtGameDir.Text) ||
                (!Paths.IsScriptsDirectoryDerived && !Paths.IsModsDirectoryDerived))
                RefreshConflictsTree();
            else
                lblStatusLeft.Text = "Please locate your 'The Witcher 3 Wild Hunt' game directory.";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.StartBatch();
            Program.Settings.Set("GameDirectory", txtGameDir.Text);
            Program.Settings.Set("CheckScripts", menuCheckScripts.Checked);
            Program.Settings.Set("CheckBundleContents", menuCheckBundleContents.Checked);
            Program.Settings.Set("CollapseUnsupported", menuCollapseUnsupported.Checked);
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
            int solvableCount = treConflicts.FileNodes.Count(node => ModFile.IsMergeable(node.Text));

            if (treConflicts.IsEmpty())
                lblStatusLeft.Text = "0 conflicts";
            else
            {
                lblStatusLeft.Text = $"{solvableCount} mergeable";
                lblStatusLeft.Text += solvableCount < treConflicts.FileNodes.Count
                    ? string.Format(",  {0} unsupported", (treConflicts.FileNodes.Count - solvableCount))
                    : string.Format(" conflict{0}", solvableCount.GetPlural());
            }

            lblStatusLeft.Text += string.Format(
                "           {0} merge{1}",
                treMerges.FileNodes.Count,
                treMerges.FileNodes.Count.GetPlural());

            if (_modIndex != null)
            {

                lblStatusRight.Text = string.Format(
                    "Found {0} mod{1}, {2} script{3}, {4} bundle{5}",
                    _modIndex.ModCount,
                    _modIndex.ModCount.GetPlural(),
                    _modIndex.ScriptCount,
                    _modIndex.ScriptCount.GetPlural(),
                    _modIndex.BundleCount,
                    _modIndex.BundleCount.GetPlural());
            }
        }

        public void EnableMergeIfValidSelection()
        {
            int validFileNodeCount = treConflicts.FileNodes.Count(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);
            btnCreateMerges.Enabled = (validFileNodeCount > 0);
            btnCreateMerges.Text = (validFileNodeCount > 1
                ? "&Create " + validFileNodeCount + " Selected Merges"
                : "&Create Selected Merge");
        }

        public void EnableUnmergeIfValidSelection()
        {
            int selectedNodes = treMerges.FileNodes.Count(node => node.Checked);
            btnDeleteMerges.Enabled = (selectedNodes > 0);
            btnDeleteMerges.Text = (selectedNodes > 1
                ? "&Delete " + selectedNodes + " Selected Merges"
                : "&Delete Selected Merge");
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
                    ConfirmPruneMissingMergeFile(merge))
                {
                    _inventory.Merges.RemoveAt(i);
                    changed = true;

                    if (merge.IsBundleContent)
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
                            ConfirmDeleteChangedMerge(merge, modName))
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
                fileNode.ForeColor = treMerges.FileNodeForeColor;

                var categoryNode = treMerges.GetCategoryNode(merge.Category);
                if (categoryNode == null)
                {
                    categoryNode = new TreeNode(merge.Category.DisplayName);
                    categoryNode.ToolTipText = merge.Category.ToolTipText;
                    categoryNode.Tag = merge.Category;
                    treMerges.Nodes.Add(categoryNode);
                }
                categoryNode.Nodes.Add(fileNode);

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
            treMerges.SetFontBold(SMTree.LevelType.Categories);
            foreach (var modNode in treMerges.ModNodes)
                modNode.HideCheckBox();

            UpdateStatusText();
            EnableUnmergeIfValidSelection();
            return false;
        }

        private bool ConfirmPruneMissingMergeFile(Merge merge)
        {
            string msg =
                "Can't find the merged version of the following file.\n\n" +
                $"{merge.RelativePath}\n\n";

            msg += merge.IsBundleContent
                ? "Remove from Merged Files list & repack merged bundle?"
                : "Remove from Merged Files list?";

            return (DialogResult.Yes == ShowMessage(
                msg,
                "Missing Merge Inventory File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
        }

        private bool ConfirmDeleteChangedMerge(Merge merge, string missingModName)
        {
            string msg =
                $"Can't find the '{missingModName}' version of the following file, " +
                "perhaps because the mod was uninstalled or updated.\n\n" +
                $"{merge.RelativePath}\n\n";

            msg += merge.IsBundleContent
                ? "Delete the affected merge & repack the merged bundle?"
                : "Delete the affected merge?";

            return (DialogResult.Yes == ShowMessage(
                msg,
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
                var nodesToUpdate = new List<TreeNode>();

                if (_inventory.ScriptsChanged || !menuCheckScripts.Checked)
                {
                    var scriptCatNode = treConflicts.GetCategoryNode(Categories.Script);
                    if (scriptCatNode != null)
                        nodesToUpdate.Add(scriptCatNode);
                }
                if (_inventory.BundleChanged || checkBundles || !menuCheckBundleContents.Checked)
                {
                    var xmlCatNode = treConflicts.GetCategoryNode(Categories.BundleXml);
                    if (xmlCatNode != null)
                        nodesToUpdate.Add(xmlCatNode);
                    var unsupportedCatNode = treConflicts.GetCategoryNode(Categories.BundleUnsupported);
                    if (unsupportedCatNode != null)
                        nodesToUpdate.Add(unsupportedCatNode);
                }

                var missingFileNodes = treConflicts.FileNodes.Where(node =>
                    node.GetTreeNodes().Any(modNode => !File.Exists(modNode.Tag as string)));
                nodesToUpdate.AddRange(missingFileNodes);

                foreach (var node in nodesToUpdate)
                    treConflicts.Nodes.Remove(node);
            }

            PrepareProgressScreen("Detecting Conflicts", ProgressBarStyle.Continuous);
            pnlProgress.Visible = true;
            lblStatusLeft.Text = "Refreshing...";

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
                    var fileNode = treConflicts.FileNodes.FirstOrDefault(node =>
                        node.Text.EqualsIgnoreCase(conflict.RelativePath));

                    if (fileNode == null)
                    {
                        fileNode = new TreeNode(conflict.RelativePath);
                        fileNode.Tag = (conflict.Category == Categories.Script
                            ? conflict.GetVanillaFile()
                            : conflict.RelativePath);
                        fileNode.ForeColor = treConflicts.FileNodeForeColor;

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
                foreach (var catNode in treConflicts.CategoryNodes)
                {
                    if (!(catNode.Tag as ModFileCategory).IsSupported)
                    {
                        catNode.HideCheckBox();
                        foreach (var fileNode in catNode.GetTreeNodes())
                        {
                            fileNode.HideCheckBox();
                            foreach (var modNode in fileNode.GetTreeNodes())
                                modNode.HideCheckBox();
                        }
                        if (menuCollapseUnsupported.Checked)
                            catNode.Collapse();
                    }
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

        private void btnSelectGameDirectory_Click(object sender, EventArgs e)
        {
            string dirChoice = GetUserDirectoryChoice();
            if (!string.IsNullOrWhiteSpace(dirChoice))
            {
                if (dirChoice.EndsWith("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                    dirChoice = Path.GetDirectoryName(dirChoice);

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
            if (string.IsNullOrWhiteSpace(txtGameDir.Text))
            {
                Program.MainForm.ShowMessage(
                    "Please locate your 'The Witcher 3 Wild Hunt' game directory.");
                return;
            }

            if (txtGameDir.Text.EndsWith("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                txtGameDir.Text = Path.GetDirectoryName(txtGameDir.Text);

            if (Paths.ValidateModsDirectory())
                RefreshMergeInventory();
        }

        private void btnRefreshConflicts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGameDir.Text))
            {
                Program.MainForm.ShowMessage(
                    "Please locate your 'The Witcher 3 Wild Hunt' game directory.");
                return;
            }

            if (txtGameDir.Text.EndsWith("The Witcher 3 Wild Hunt\\Mods"))  // Auto-truncate "Mods"
                txtGameDir.Text = Path.GetDirectoryName(txtGameDir.Text);
            
            RefreshTrees();
        }

        private void btnMergeFiles_Click(object sender, EventArgs e)
        {
            if (!Paths.ValidateModsDirectory() ||
                (treConflicts.FileNodes.Any(node => ModFile.IsScript(node.Text)) && !Paths.ValidateScriptsDirectory()) ||
                (treConflicts.FileNodes.Any(node => ModFile.IsBundle(node.Text)) && !Paths.ValidateBundlesDirectory()))
                return;

            string mergedModName = Paths.RetrieveMergedModName();
            if (mergedModName == null)
                return;
            
            _inventory = MergeInventory.Load(Paths.Inventory);

            var merger = new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete);

            var fileNodes = treConflicts.FileNodes.Where(node => node.GetTreeNodes().Count(modNode => modNode.Checked) > 1);

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
            var fileNodes = treMerges.FileNodes.Where(node => node.Checked);
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
        
        #region Key Input

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                (sender as TextBox).SelectAll();
        }
        
        private void splitContainer_Panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnCreateMerges.Enabled)
                btnMergeFiles_Click(null, null);
        }

        private void splitContainer_Panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
                if (merge.IsBundleContent)
                {
                    var mergesForBundle = _inventory.Merges.Where(m =>
                    m.IsBundleContent &&
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

        private void HandleDeletedBundleMerges(List<Merge> bundleMerges)
        {
            var affectedBundles = bundleMerges.Select(merge => merge.GetMergedBundle()).Distinct();
            foreach (var bundlePath in affectedBundles)
            {
                PrepareProgressScreen("Merge Deleted — Repacking Bundle", ProgressBarStyle.Marquee);
                pnlProgress.Visible = true;
                new FileMerger(_inventory, OnMergeProgressChanged, OnMergeComplete)
                    .RepackBundleForDeleteAsync(bundlePath);
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
            if (!dirInfo.Exists || dirInfo.GetFiles().Length > 0 || dirInfo.GetDirectories().Length > 0)
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

        private void menuDependencies_Click(object sender, EventArgs e)
        {
            using (var dependencyForm = new DependencyForm())
            {
                ShowModal(dependencyForm);
            }
        }
    }
}