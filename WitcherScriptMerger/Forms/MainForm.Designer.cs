namespace WitcherScriptMerger.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtGameDir = new System.Windows.Forms.TextBox();
            this.btnSelectGameDir = new System.Windows.Forms.Button();
            this.treConflicts = new System.Windows.Forms.TreeView();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextOpenVanillaBundleDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenModBundleDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenMergedBundleDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenVanillaScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenVanillaScriptDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenModScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenModScriptDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenMergedScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenMergedScriptDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOpenSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextDeleteMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeleteAssociatedMerges = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeleteSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefreshConflicts = new System.Windows.Forms.Button();
            this.btnMergeScripts = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlRefreshProgress = new System.Windows.Forms.Panel();
            this.lblRefreshProgress = new System.Windows.Forms.Label();
            this.refreshBar = new System.Windows.Forms.ProgressBar();
            this.lblConflicts = new System.Windows.Forms.Label();
            this.btnRefreshMerged = new System.Windows.Forms.Button();
            this.lblMergeInventory = new System.Windows.Forms.Label();
            this.btnDeleteMerges = new System.Windows.Forms.Button();
            this.treMerges = new System.Windows.Forms.TreeView();
            this.grpGameDir = new System.Windows.Forms.GroupBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckBundleContents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReviewEach = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPathsInKDiff3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMergeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlRefreshProgress.SuspendLayout();
            this.grpGameDir.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGameDir
            // 
            this.txtGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGameDir.Location = new System.Drawing.Point(6, 19);
            this.txtGameDir.Name = "txtGameDir";
            this.txtGameDir.Size = new System.Drawing.Size(589, 20);
            this.txtGameDir.TabIndex = 0;
            this.txtGameDir.TextChanged += new System.EventHandler(this.txtGameDir_TextChanged);
            this.txtGameDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnSelectGameDir
            // 
            this.btnSelectGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectGameDir.Location = new System.Drawing.Point(601, 17);
            this.btnSelectGameDir.Name = "btnSelectGameDir";
            this.btnSelectGameDir.Size = new System.Drawing.Size(26, 23);
            this.btnSelectGameDir.TabIndex = 1;
            this.btnSelectGameDir.Text = "...";
            this.btnSelectGameDir.UseVisualStyleBackColor = true;
            this.btnSelectGameDir.Click += new System.EventHandler(this.btnSelectGameDirectory_Click);
            // 
            // treConflicts
            // 
            this.treConflicts.AllowDrop = true;
            this.treConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treConflicts.CheckBoxes = true;
            this.treConflicts.Location = new System.Drawing.Point(12, 30);
            this.treConflicts.Name = "treConflicts";
            this.treConflicts.ShowNodeToolTips = true;
            this.treConflicts.ShowRootLines = false;
            this.treConflicts.Size = new System.Drawing.Size(312, 489);
            this.treConflicts.TabIndex = 1;
            this.treConflicts.TabStop = false;
            this.treConflicts.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            this.treConflicts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.treConflicts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treConflicts_KeyDown);
            this.treConflicts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            this.treConflicts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.AutoSize = false;
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextOpenVanillaBundleDir,
            this.contextOpenModBundleDir,
            this.contextOpenMergedBundleDir,
            this.contextOpenVanillaScript,
            this.contextOpenVanillaScriptDir,
            this.contextOpenModScript,
            this.contextOpenModScriptDir,
            this.contextOpenMergedScript,
            this.contextOpenMergedScriptDir,
            this.contextCopyPath,
            this.contextOpenSeparator,
            this.contextDeleteMerge,
            this.contextDeleteAssociatedMerges,
            this.contextDeleteSeparator,
            this.contextSelectAll,
            this.contextDeselectAll,
            this.contextExpandAll,
            this.contextCollapseAll});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(239, 390);
            this.treeContextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.treeContextMenu_Closing);
            // 
            // contextOpenVanillaBundleDir
            // 
            this.contextOpenVanillaBundleDir.Name = "contextOpenVanillaBundleDir";
            this.contextOpenVanillaBundleDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenVanillaBundleDir.Text = "Open Vanilla Bundle Directory";
            this.contextOpenVanillaBundleDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextOpenModBundleDir
            // 
            this.contextOpenModBundleDir.Name = "contextOpenModBundleDir";
            this.contextOpenModBundleDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenModBundleDir.Text = "Open Mod Bundle Directory";
            this.contextOpenModBundleDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextOpenMergedBundleDir
            // 
            this.contextOpenMergedBundleDir.Name = "contextOpenMergedBundleDir";
            this.contextOpenMergedBundleDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenMergedBundleDir.Text = "Open Merged Bundle Directory";
            this.contextOpenMergedBundleDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextOpenVanillaScript
            // 
            this.contextOpenVanillaScript.Name = "contextOpenVanillaScript";
            this.contextOpenVanillaScript.Size = new System.Drawing.Size(238, 22);
            this.contextOpenVanillaScript.Text = "Open Vanilla Script";
            this.contextOpenVanillaScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextOpenVanillaScriptDir
            // 
            this.contextOpenVanillaScriptDir.Name = "contextOpenVanillaScriptDir";
            this.contextOpenVanillaScriptDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenVanillaScriptDir.Text = "Open Vanilla Script Directory";
            this.contextOpenVanillaScriptDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextOpenModScript
            // 
            this.contextOpenModScript.Name = "contextOpenModScript";
            this.contextOpenModScript.Size = new System.Drawing.Size(238, 22);
            this.contextOpenModScript.Text = "Open Mod Script";
            this.contextOpenModScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextOpenModScriptDir
            // 
            this.contextOpenModScriptDir.Name = "contextOpenModScriptDir";
            this.contextOpenModScriptDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenModScriptDir.Text = "Open Mod Script Directory";
            this.contextOpenModScriptDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextOpenMergedScript
            // 
            this.contextOpenMergedScript.Name = "contextOpenMergedScript";
            this.contextOpenMergedScript.Size = new System.Drawing.Size(238, 22);
            this.contextOpenMergedScript.Text = "Open Merged Script";
            this.contextOpenMergedScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextOpenMergedScriptDir
            // 
            this.contextOpenMergedScriptDir.Name = "contextOpenMergedScriptDir";
            this.contextOpenMergedScriptDir.Size = new System.Drawing.Size(238, 22);
            this.contextOpenMergedScriptDir.Text = "Open Merged Script Directory";
            this.contextOpenMergedScriptDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextCopyPath
            // 
            this.contextCopyPath.Name = "contextCopyPath";
            this.contextCopyPath.Size = new System.Drawing.Size(238, 22);
            this.contextCopyPath.Text = "Copy Path";
            this.contextCopyPath.Click += new System.EventHandler(this.contextCopyPath_Click);
            // 
            // contextOpenSeparator
            // 
            this.contextOpenSeparator.Name = "contextOpenSeparator";
            this.contextOpenSeparator.Size = new System.Drawing.Size(235, 6);
            // 
            // contextDeleteMerge
            // 
            this.contextDeleteMerge.Name = "contextDeleteMerge";
            this.contextDeleteMerge.Size = new System.Drawing.Size(238, 22);
            this.contextDeleteMerge.Text = "Delete This Merge";
            this.contextDeleteMerge.Click += new System.EventHandler(this.contextDeleteMerge_Click);
            // 
            // contextDeleteAssociatedMerges
            // 
            this.contextDeleteAssociatedMerges.Name = "contextDeleteAssociatedMerges";
            this.contextDeleteAssociatedMerges.Size = new System.Drawing.Size(238, 22);
            this.contextDeleteAssociatedMerges.Text = "Delete All {0} Merges";
            this.contextDeleteAssociatedMerges.Click += new System.EventHandler(this.contextDeleteAssociatedMerges_Click);
            // 
            // contextDeleteSeparator
            // 
            this.contextDeleteSeparator.Name = "contextDeleteSeparator";
            this.contextDeleteSeparator.Size = new System.Drawing.Size(235, 6);
            // 
            // contextSelectAll
            // 
            this.contextSelectAll.Name = "contextSelectAll";
            this.contextSelectAll.Size = new System.Drawing.Size(238, 22);
            this.contextSelectAll.Text = "Select All";
            this.contextSelectAll.Click += new System.EventHandler(this.contextSelectAll_Click);
            // 
            // contextDeselectAll
            // 
            this.contextDeselectAll.Name = "contextDeselectAll";
            this.contextDeselectAll.Size = new System.Drawing.Size(238, 22);
            this.contextDeselectAll.Text = "Deselect All";
            this.contextDeselectAll.Click += new System.EventHandler(this.contextDeselectAll_Click);
            // 
            // contextExpandAll
            // 
            this.contextExpandAll.Name = "contextExpandAll";
            this.contextExpandAll.Size = new System.Drawing.Size(238, 22);
            this.contextExpandAll.Text = "Expand All";
            this.contextExpandAll.Click += new System.EventHandler(this.contextExpandAll_Click);
            // 
            // contextCollapseAll
            // 
            this.contextCollapseAll.Name = "contextCollapseAll";
            this.contextCollapseAll.Size = new System.Drawing.Size(238, 22);
            this.contextCollapseAll.Text = "Collapse All";
            this.contextCollapseAll.Click += new System.EventHandler(this.contextCollapseAll_Click);
            // 
            // btnRefreshConflicts
            // 
            this.btnRefreshConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshConflicts.Location = new System.Drawing.Point(86, 3);
            this.btnRefreshConflicts.MaximumSize = new System.Drawing.Size(150, 23);
            this.btnRefreshConflicts.MinimumSize = new System.Drawing.Size(100, 23);
            this.btnRefreshConflicts.Name = "btnRefreshConflicts";
            this.btnRefreshConflicts.Size = new System.Drawing.Size(150, 23);
            this.btnRefreshConflicts.TabIndex = 0;
            this.btnRefreshConflicts.Text = "&Refresh";
            this.btnRefreshConflicts.UseVisualStyleBackColor = true;
            this.btnRefreshConflicts.Click += new System.EventHandler(this.btnRefreshConflicts_Click);
            // 
            // btnMergeScripts
            // 
            this.btnMergeScripts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMergeScripts.Enabled = false;
            this.btnMergeScripts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMergeScripts.Location = new System.Drawing.Point(12, 525);
            this.btnMergeScripts.Name = "btnMergeScripts";
            this.btnMergeScripts.Size = new System.Drawing.Size(312, 35);
            this.btnMergeScripts.TabIndex = 6;
            this.btnMergeScripts.Text = "&Merge Selected File";
            this.btnMergeScripts.UseVisualStyleBackColor = true;
            this.btnMergeScripts.Click += new System.EventHandler(this.btnMergeScripts_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 86);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlRefreshProgress);
            this.splitContainer.Panel1.Controls.Add(this.lblConflicts);
            this.splitContainer.Panel1.Controls.Add(this.treConflicts);
            this.splitContainer.Panel1.Controls.Add(this.btnRefreshConflicts);
            this.splitContainer.Panel1.Controls.Add(this.btnMergeScripts);
            this.splitContainer.Panel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.splitContainer_Panel1_PreviewKeyDown);
            this.splitContainer.Panel1MinSize = 240;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnRefreshMerged);
            this.splitContainer.Panel2.Controls.Add(this.lblMergeInventory);
            this.splitContainer.Panel2.Controls.Add(this.btnDeleteMerges);
            this.splitContainer.Panel2.Controls.Add(this.treMerges);
            this.splitContainer.Panel2MinSize = 235;
            this.splitContainer.Size = new System.Drawing.Size(654, 563);
            this.splitContainer.SplitterDistance = 327;
            this.splitContainer.TabIndex = 1;
            // 
            // pnlRefreshProgress
            // 
            this.pnlRefreshProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRefreshProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRefreshProgress.Controls.Add(this.lblRefreshProgress);
            this.pnlRefreshProgress.Controls.Add(this.refreshBar);
            this.pnlRefreshProgress.Location = new System.Drawing.Point(12, 30);
            this.pnlRefreshProgress.Name = "pnlRefreshProgress";
            this.pnlRefreshProgress.Padding = new System.Windows.Forms.Padding(8);
            this.pnlRefreshProgress.Size = new System.Drawing.Size(312, 489);
            this.pnlRefreshProgress.TabIndex = 8;
            this.pnlRefreshProgress.Visible = false;
            // 
            // lblRefreshProgress
            // 
            this.lblRefreshProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefreshProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefreshProgress.Location = new System.Drawing.Point(11, 220);
            this.lblRefreshProgress.Name = "lblRefreshProgress";
            this.lblRefreshProgress.Size = new System.Drawing.Size(288, 20);
            this.lblRefreshProgress.TabIndex = 1;
            this.lblRefreshProgress.Text = "Refreshing";
            this.lblRefreshProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // refreshBar
            // 
            this.refreshBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshBar.Location = new System.Drawing.Point(25, 243);
            this.refreshBar.Name = "refreshBar";
            this.refreshBar.Size = new System.Drawing.Size(260, 20);
            this.refreshBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.refreshBar.TabIndex = 0;
            // 
            // lblConflicts
            // 
            this.lblConflicts.AutoSize = true;
            this.lblConflicts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConflicts.Location = new System.Drawing.Point(9, 6);
            this.lblConflicts.Name = "lblConflicts";
            this.lblConflicts.Size = new System.Drawing.Size(71, 16);
            this.lblConflicts.TabIndex = 16;
            this.lblConflicts.Text = "Conflicts:";
            // 
            // btnRefreshMerged
            // 
            this.btnRefreshMerged.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshMerged.Location = new System.Drawing.Point(112, 3);
            this.btnRefreshMerged.MaximumSize = new System.Drawing.Size(150, 23);
            this.btnRefreshMerged.MinimumSize = new System.Drawing.Size(100, 23);
            this.btnRefreshMerged.Name = "btnRefreshMerged";
            this.btnRefreshMerged.Size = new System.Drawing.Size(117, 23);
            this.btnRefreshMerged.TabIndex = 0;
            this.btnRefreshMerged.Text = "Re&fresh";
            this.btnRefreshMerged.UseVisualStyleBackColor = true;
            this.btnRefreshMerged.Click += new System.EventHandler(this.btnRefreshMerged_Click);
            // 
            // lblMergeInventory
            // 
            this.lblMergeInventory.AutoSize = true;
            this.lblMergeInventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMergeInventory.Location = new System.Drawing.Point(3, 6);
            this.lblMergeInventory.Name = "lblMergeInventory";
            this.lblMergeInventory.Size = new System.Drawing.Size(103, 16);
            this.lblMergeInventory.TabIndex = 7;
            this.lblMergeInventory.Text = "Merged Files:";
            // 
            // btnDeleteMerges
            // 
            this.btnDeleteMerges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteMerges.Enabled = false;
            this.btnDeleteMerges.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMerges.Location = new System.Drawing.Point(3, 525);
            this.btnDeleteMerges.Name = "btnDeleteMerges";
            this.btnDeleteMerges.Size = new System.Drawing.Size(310, 35);
            this.btnDeleteMerges.TabIndex = 2;
            this.btnDeleteMerges.Text = "&Delete Selected Merge";
            this.btnDeleteMerges.UseVisualStyleBackColor = true;
            this.btnDeleteMerges.Click += new System.EventHandler(this.btnDeleteMerges_Click);
            // 
            // treMerges
            // 
            this.treMerges.AllowDrop = true;
            this.treMerges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treMerges.CheckBoxes = true;
            this.treMerges.Location = new System.Drawing.Point(3, 30);
            this.treMerges.Name = "treMerges";
            this.treMerges.ShowNodeToolTips = true;
            this.treMerges.ShowRootLines = false;
            this.treMerges.Size = new System.Drawing.Size(310, 489);
            this.treMerges.TabIndex = 1;
            this.treMerges.TabStop = false;
            this.treMerges.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            this.treMerges.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.treMerges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treMergeInventory_KeyDown);
            this.treMerges.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            this.treMerges.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // grpGameDir
            // 
            this.grpGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGameDir.Controls.Add(this.txtGameDir);
            this.grpGameDir.Controls.Add(this.btnSelectGameDir);
            this.grpGameDir.Location = new System.Drawing.Point(9, 30);
            this.grpGameDir.Name = "grpGameDir";
            this.grpGameDir.Size = new System.Drawing.Size(633, 50);
            this.grpGameDir.TabIndex = 0;
            this.grpGameDir.TabStop = false;
            this.grpGameDir.Text = "Witcher 3 Directory";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(654, 24);
            this.menuStrip.TabIndex = 2;
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCheckScripts,
            this.menuCheckBundleContents,
            this.menuReviewEach,
            this.menuPathsInKDiff3,
            this.menuMergeReport,
            this.menuShowStatusBar});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // menuCheckScripts
            // 
            this.menuCheckScripts.CheckOnClick = true;
            this.menuCheckScripts.Name = "menuCheckScripts";
            this.menuCheckScripts.Size = new System.Drawing.Size(297, 22);
            this.menuCheckScripts.Text = "Check &Scripts";
            // 
            // menuCheckBundleContents
            // 
            this.menuCheckBundleContents.CheckOnClick = true;
            this.menuCheckBundleContents.Name = "menuCheckBundleContents";
            this.menuCheckBundleContents.Size = new System.Drawing.Size(297, 22);
            this.menuCheckBundleContents.Text = "Check &Bundle Contents";
            // 
            // menuReviewEach
            // 
            this.menuReviewEach.CheckOnClick = true;
            this.menuReviewEach.Name = "menuReviewEach";
            this.menuReviewEach.Size = new System.Drawing.Size(297, 22);
            this.menuReviewEach.Text = "&Review Each Merge (even if auto-solvable)";
            // 
            // menuPathsInKDiff3
            // 
            this.menuPathsInKDiff3.CheckOnClick = true;
            this.menuPathsInKDiff3.Name = "menuPathsInKDiff3";
            this.menuPathsInKDiff3.Size = new System.Drawing.Size(297, 22);
            this.menuPathsInKDiff3.Text = "Show &Paths in KDiff3";
            // 
            // menuMergeReport
            // 
            this.menuMergeReport.CheckOnClick = true;
            this.menuMergeReport.Name = "menuMergeReport";
            this.menuMergeReport.Size = new System.Drawing.Size(297, 22);
            this.menuMergeReport.Text = "Show Report &After Each Merge";
            // 
            // menuShowStatusBar
            // 
            this.menuShowStatusBar.CheckOnClick = true;
            this.menuShowStatusBar.Name = "menuShowStatusBar";
            this.menuShowStatusBar.Size = new System.Drawing.Size(297, 22);
            this.menuShowStatusBar.Text = "Show S&tatus Bar";
            this.menuShowStatusBar.Click += new System.EventHandler(this.menuShowStatusBar_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 655);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(654, 19);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(114, 14);
            this.lblStatus.Text = "0 conflicts   0 merges";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnMergeScripts;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 674);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.grpGameDir);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(495, 350);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Merger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.treeContextMenu.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlRefreshProgress.ResumeLayout(false);
            this.grpGameDir.ResumeLayout(false);
            this.grpGameDir.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGameDir;
        private System.Windows.Forms.Button btnSelectGameDir;
        private System.Windows.Forms.TreeView treConflicts;
        private System.Windows.Forms.Button btnRefreshConflicts;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextOpenModScriptDir;
        private System.Windows.Forms.ToolStripMenuItem contextCopyPath;
        private System.Windows.Forms.ToolStripMenuItem contextExpandAll;
        private System.Windows.Forms.ToolStripMenuItem contextCollapseAll;
        private System.Windows.Forms.ToolStripMenuItem contextOpenModScript;
        private System.Windows.Forms.ToolStripMenuItem contextOpenVanillaScript;
        private System.Windows.Forms.ToolStripMenuItem contextOpenVanillaScriptDir;
        private System.Windows.Forms.Button btnMergeScripts;
        private System.Windows.Forms.ToolStripSeparator contextOpenSeparator;
        private System.Windows.Forms.ToolStripMenuItem contextSelectAll;
        private System.Windows.Forms.ToolStripMenuItem contextDeselectAll;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblMergeInventory;
        private System.Windows.Forms.Button btnDeleteMerges;
        private System.Windows.Forms.TreeView treMerges;
        private System.Windows.Forms.GroupBox grpGameDir;
        private System.Windows.Forms.Label lblConflicts;
        private System.Windows.Forms.ToolStripMenuItem contextOpenMergedScript;
        private System.Windows.Forms.ToolStripMenuItem contextOpenMergedScriptDir;
        private System.Windows.Forms.Button btnRefreshMerged;
        private System.Windows.Forms.ToolStripMenuItem contextDeleteAssociatedMerges;
        private System.Windows.Forms.ToolStripMenuItem contextDeleteMerge;
        private System.Windows.Forms.ToolStripSeparator contextDeleteSeparator;
        private System.Windows.Forms.ToolStripMenuItem contextOpenVanillaBundleDir;
        private System.Windows.Forms.ToolStripMenuItem contextOpenModBundleDir;
        private System.Windows.Forms.ToolStripMenuItem contextOpenMergedBundleDir;
        private System.Windows.Forms.Panel pnlRefreshProgress;
        private System.Windows.Forms.Label lblRefreshProgress;
        private System.Windows.Forms.ProgressBar refreshBar;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuCheckScripts;
        private System.Windows.Forms.ToolStripMenuItem menuCheckBundleContents;
        private System.Windows.Forms.ToolStripMenuItem menuReviewEach;
        private System.Windows.Forms.ToolStripMenuItem menuPathsInKDiff3;
        private System.Windows.Forms.ToolStripMenuItem menuMergeReport;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem menuShowStatusBar;
    }
}

