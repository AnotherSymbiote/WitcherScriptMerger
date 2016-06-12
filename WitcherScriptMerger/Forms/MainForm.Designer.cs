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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtGameDir = new System.Windows.Forms.TextBox();
            this.btnSelectGameDir = new System.Windows.Forms.Button();
            this.btnRefreshConflicts = new System.Windows.Forms.Button();
            this.btnCreateMerges = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lblConflicts = new System.Windows.Forms.Label();
            this.treConflicts = new WitcherScriptMerger.Controls.ConflictTree();
            this.btnRefreshMerged = new System.Windows.Forms.Button();
            this.lblMergeInventory = new System.Windows.Forms.Label();
            this.btnDeleteMerges = new System.Windows.Forms.Button();
            this.treMerges = new WitcherScriptMerger.Controls.MergeTree();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblProgressCurrentPhase = new System.Windows.Forms.Label();
            this.lblProgressCurrentAction = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRefreshingConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckXmlFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckBundleContents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuValidateCustomLoadOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMerging = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReviewEach = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPathsInKDiff3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompletionSounds = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMergeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPackReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGameDir = new System.Windows.Forms.Label();
            this.menuCollapseIf = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollapseNotMergeable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollapseCustomLoadOrder = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlProgress.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGameDir
            // 
            this.txtGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGameDir.Location = new System.Drawing.Point(213, 3);
            this.txtGameDir.Name = "txtGameDir";
            this.txtGameDir.Size = new System.Drawing.Size(399, 20);
            this.txtGameDir.TabIndex = 0;
            this.txtGameDir.TextChanged += new System.EventHandler(this.txtGameDir_TextChanged);
            this.txtGameDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnSelectGameDir
            // 
            this.btnSelectGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectGameDir.Location = new System.Drawing.Point(618, 1);
            this.btnSelectGameDir.Name = "btnSelectGameDir";
            this.btnSelectGameDir.Size = new System.Drawing.Size(26, 23);
            this.btnSelectGameDir.TabIndex = 1;
            this.btnSelectGameDir.Text = "...";
            this.btnSelectGameDir.UseVisualStyleBackColor = true;
            this.btnSelectGameDir.Click += new System.EventHandler(this.btnSelectGameDirectory_Click);
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
            // btnCreateMerges
            // 
            this.btnCreateMerges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateMerges.Enabled = false;
            this.btnCreateMerges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateMerges.Location = new System.Drawing.Point(12, 573);
            this.btnCreateMerges.Name = "btnCreateMerges";
            this.btnCreateMerges.Size = new System.Drawing.Size(312, 35);
            this.btnCreateMerges.TabIndex = 6;
            this.btnCreateMerges.Text = "&Create Selected Merge";
            this.btnCreateMerges.UseVisualStyleBackColor = true;
            this.btnCreateMerges.Click += new System.EventHandler(this.btnMergeFiles_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 38);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lblConflicts);
            this.splitContainer.Panel1.Controls.Add(this.treConflicts);
            this.splitContainer.Panel1.Controls.Add(this.btnRefreshConflicts);
            this.splitContainer.Panel1.Controls.Add(this.btnCreateMerges);
            this.splitContainer.Panel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.splitContainer_Panel1_PreviewKeyDown);
            this.splitContainer.Panel1MinSize = 195;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnRefreshMerged);
            this.splitContainer.Panel2.Controls.Add(this.lblMergeInventory);
            this.splitContainer.Panel2.Controls.Add(this.btnDeleteMerges);
            this.splitContainer.Panel2.Controls.Add(this.treMerges);
            this.splitContainer.Panel2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.splitContainer_Panel2_PreviewKeyDown);
            this.splitContainer.Panel2MinSize = 225;
            this.splitContainer.Size = new System.Drawing.Size(654, 611);
            this.splitContainer.SplitterDistance = 327;
            this.splitContainer.TabIndex = 1;
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
            // treConflicts
            // 
            this.treConflicts.AllowDrop = true;
            this.treConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treConflicts.CheckBoxes = true;
            this.treConflicts.FileNodeForeColor = System.Drawing.Color.Red;
            this.treConflicts.Location = new System.Drawing.Point(12, 30);
            this.treConflicts.Name = "treConflicts";
            this.treConflicts.ShowNodeToolTips = true;
            this.treConflicts.Size = new System.Drawing.Size(312, 537);
            this.treConflicts.Sorted = true;
            this.treConflicts.TabIndex = 1;
            this.treConflicts.TabStop = false;
            // 
            // btnRefreshMerged
            // 
            this.btnRefreshMerged.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshMerged.Location = new System.Drawing.Point(73, 3);
            this.btnRefreshMerged.MaximumSize = new System.Drawing.Size(150, 23);
            this.btnRefreshMerged.MinimumSize = new System.Drawing.Size(100, 23);
            this.btnRefreshMerged.Name = "btnRefreshMerged";
            this.btnRefreshMerged.Size = new System.Drawing.Size(150, 23);
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
            this.lblMergeInventory.Size = new System.Drawing.Size(64, 16);
            this.lblMergeInventory.TabIndex = 7;
            this.lblMergeInventory.Text = "Merges:";
            // 
            // btnDeleteMerges
            // 
            this.btnDeleteMerges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteMerges.Enabled = false;
            this.btnDeleteMerges.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMerges.Location = new System.Drawing.Point(3, 573);
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
            this.treMerges.FileNodeForeColor = System.Drawing.Color.Blue;
            this.treMerges.Location = new System.Drawing.Point(3, 30);
            this.treMerges.Name = "treMerges";
            this.treMerges.ShowNodeToolTips = true;
            this.treMerges.Size = new System.Drawing.Size(310, 537);
            this.treMerges.Sorted = true;
            this.treMerges.TabIndex = 1;
            this.treMerges.TabStop = false;
            // 
            // pnlProgress
            // 
            this.pnlProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgress.Controls.Add(this.lblProgressCurrentPhase);
            this.pnlProgress.Controls.Add(this.lblProgressCurrentAction);
            this.pnlProgress.Controls.Add(this.progressBar);
            this.pnlProgress.Location = new System.Drawing.Point(9, 30);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Padding = new System.Windows.Forms.Padding(8);
            this.pnlProgress.Size = new System.Drawing.Size(635, 619);
            this.pnlProgress.TabIndex = 8;
            this.pnlProgress.Visible = false;
            // 
            // lblProgressCurrentPhase
            // 
            this.lblProgressCurrentPhase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressCurrentPhase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressCurrentPhase.Location = new System.Drawing.Point(11, 233);
            this.lblProgressCurrentPhase.Name = "lblProgressCurrentPhase";
            this.lblProgressCurrentPhase.Size = new System.Drawing.Size(611, 70);
            this.lblProgressCurrentPhase.TabIndex = 2;
            this.lblProgressCurrentPhase.Text = "Initializing";
            this.lblProgressCurrentPhase.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblProgressCurrentAction
            // 
            this.lblProgressCurrentAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressCurrentAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressCurrentAction.Location = new System.Drawing.Point(11, 333);
            this.lblProgressCurrentAction.Name = "lblProgressCurrentAction";
            this.lblProgressCurrentAction.Size = new System.Drawing.Size(611, 129);
            this.lblProgressCurrentAction.TabIndex = 1;
            this.lblProgressCurrentAction.Text = "...";
            this.lblProgressCurrentAction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(25, 308);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(583, 20);
            this.progressBar.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
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
            this.menuRefreshingConflicts,
            this.menuMerging,
            this.menuDependencies,
            this.menuShowStatusBar});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // menuRefreshingConflicts
            // 
            this.menuRefreshingConflicts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCheckScripts,
            this.menuCheckXmlFiles,
            this.menuCheckBundleContents,
            this.menuValidateCustomLoadOrder,
            this.menuCollapseIf});
            this.menuRefreshingConflicts.Name = "menuRefreshingConflicts";
            this.menuRefreshingConflicts.Size = new System.Drawing.Size(203, 22);
            this.menuRefreshingConflicts.Text = "&Refreshing Conflicts...";
            // 
            // menuCheckScripts
            // 
            this.menuCheckScripts.CheckOnClick = true;
            this.menuCheckScripts.Name = "menuCheckScripts";
            this.menuCheckScripts.Size = new System.Drawing.Size(223, 22);
            this.menuCheckScripts.Text = "Check &Scripts";
            this.menuCheckScripts.ToolTipText = "Check for mod conflicts in script files";
            // 
            // menuCheckXmlFiles
            // 
            this.menuCheckXmlFiles.CheckOnClick = true;
            this.menuCheckXmlFiles.Name = "menuCheckXmlFiles";
            this.menuCheckXmlFiles.Size = new System.Drawing.Size(223, 22);
            this.menuCheckXmlFiles.Text = "Check &XML Files";
            this.menuCheckXmlFiles.ToolTipText = "Check for mod conflicts in XML files";
            // 
            // menuCheckBundleContents
            // 
            this.menuCheckBundleContents.CheckOnClick = true;
            this.menuCheckBundleContents.Name = "menuCheckBundleContents";
            this.menuCheckBundleContents.Size = new System.Drawing.Size(223, 22);
            this.menuCheckBundleContents.Text = "Check &Bundle Contents";
            this.menuCheckBundleContents.ToolTipText = "Check for mod conflicts in bundle file contents";
            // 
            // menuValidateCustomLoadOrder
            // 
            this.menuValidateCustomLoadOrder.CheckOnClick = true;
            this.menuValidateCustomLoadOrder.Name = "menuValidateCustomLoadOrder";
            this.menuValidateCustomLoadOrder.Size = new System.Drawing.Size(223, 22);
            this.menuValidateCustomLoadOrder.Text = "&Validate Custom Load Order";
            this.menuValidateCustomLoadOrder.ToolTipText = "Whether to detect mods.settings file on refresh and make sure it loads merged fil" +
    "es first";
            // 
            // menuMerging
            // 
            this.menuMerging.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuReviewEach,
            this.menuPathsInKDiff3,
            this.menuCompletionSounds,
            this.menuMergeReport,
            this.menuPackReport});
            this.menuMerging.Name = "menuMerging";
            this.menuMerging.Size = new System.Drawing.Size(203, 22);
            this.menuMerging.Text = "&Merging...";
            // 
            // menuReviewEach
            // 
            this.menuReviewEach.CheckOnClick = true;
            this.menuReviewEach.Name = "menuReviewEach";
            this.menuReviewEach.Size = new System.Drawing.Size(297, 22);
            this.menuReviewEach.Text = "&Review Each Merge (even if auto-solvable)";
            this.menuReviewEach.ToolTipText = "Show text comparison UI for every merge";
            // 
            // menuPathsInKDiff3
            // 
            this.menuPathsInKDiff3.CheckOnClick = true;
            this.menuPathsInKDiff3.Name = "menuPathsInKDiff3";
            this.menuPathsInKDiff3.Size = new System.Drawing.Size(297, 22);
            this.menuPathsInKDiff3.Text = "Show File Paths in &KDiff3";
            this.menuPathsInKDiff3.ToolTipText = "Show full file paths instead of just Vanilla, modName1, modName2";
            // 
            // menuCompletionSounds
            // 
            this.menuCompletionSounds.CheckOnClick = true;
            this.menuCompletionSounds.Name = "menuCompletionSounds";
            this.menuCompletionSounds.Size = new System.Drawing.Size(297, 22);
            this.menuCompletionSounds.Text = "Play Completion &Sounds";
            this.menuCompletionSounds.ToolTipText = "Play a sound after merging or packing a bundle";
            // 
            // menuMergeReport
            // 
            this.menuMergeReport.CheckOnClick = true;
            this.menuMergeReport.Name = "menuMergeReport";
            this.menuMergeReport.Size = new System.Drawing.Size(297, 22);
            this.menuMergeReport.Text = "Show Report After Each &Merge";
            this.menuMergeReport.ToolTipText = "Show a report with buttons to open files/directories";
            // 
            // menuPackReport
            // 
            this.menuPackReport.Name = "menuPackReport";
            this.menuPackReport.Size = new System.Drawing.Size(297, 22);
            this.menuPackReport.Text = "Show Report After &Packing Bundle";
            this.menuPackReport.ToolTipText = "Show a report with list of contents and button to open content directory";
            // 
            // menuDependencies
            // 
            this.menuDependencies.Name = "menuDependencies";
            this.menuDependencies.Size = new System.Drawing.Size(203, 22);
            this.menuDependencies.Text = "&Dependency Locations...";
            this.menuDependencies.ToolTipText = "Locate the 3rd-party tools that Script Merger depends on";
            this.menuDependencies.Click += new System.EventHandler(this.menuDependencies_Click);
            // 
            // menuShowStatusBar
            // 
            this.menuShowStatusBar.CheckOnClick = true;
            this.menuShowStatusBar.Name = "menuShowStatusBar";
            this.menuShowStatusBar.Size = new System.Drawing.Size(203, 22);
            this.menuShowStatusBar.Text = "&Show Status Bar";
            this.menuShowStatusBar.ToolTipText = "Show bar with statistics at the bottom of the window";
            this.menuShowStatusBar.Click += new System.EventHandler(this.menuShowStatusBar_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusLeft,
            this.lblStatusSpring,
            this.lblStatusRight});
            this.statusStrip.Location = new System.Drawing.Point(0, 655);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(654, 19);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatusLeft
            // 
            this.lblStatusLeft.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblStatusLeft.Name = "lblStatusLeft";
            this.lblStatusLeft.Size = new System.Drawing.Size(58, 14);
            this.lblStatusLeft.Text = "Loading...";
            // 
            // lblStatusSpring
            // 
            this.lblStatusSpring.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatusSpring.Name = "lblStatusSpring";
            this.lblStatusSpring.Size = new System.Drawing.Size(567, 14);
            this.lblStatusSpring.Spring = true;
            // 
            // lblStatusRight
            // 
            this.lblStatusRight.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblStatusRight.Name = "lblStatusRight";
            this.lblStatusRight.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblStatusRight.Size = new System.Drawing.Size(14, 14);
            // 
            // lblGameDir
            // 
            this.lblGameDir.AutoSize = true;
            this.lblGameDir.Location = new System.Drawing.Point(106, 6);
            this.lblGameDir.Name = "lblGameDir";
            this.lblGameDir.Size = new System.Drawing.Size(101, 13);
            this.lblGameDir.TabIndex = 9;
            this.lblGameDir.Text = "Witcher 3 Directory:";
            // 
            // menuCollapseIf
            // 
            this.menuCollapseIf.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCollapseNotMergeable,
            this.menuCollapseCustomLoadOrder});
            this.menuCollapseIf.Name = "menuCollapseIf";
            this.menuCollapseIf.Size = new System.Drawing.Size(223, 22);
            this.menuCollapseIf.Text = "Auto-&Collapse in Tree If...";
            // 
            // menuCollapseNotMergeable
            // 
            this.menuCollapseNotMergeable.CheckOnClick = true;
            this.menuCollapseNotMergeable.Name = "menuCollapseNotMergeable";
            this.menuCollapseNotMergeable.Size = new System.Drawing.Size(244, 22);
            this.menuCollapseNotMergeable.Text = "&Not Mergeable";
            this.menuCollapseNotMergeable.ToolTipText = "Auto-collapse conflicts that can\'t be merged (non-text files)";
            // 
            // menuCollapseCustomLoadOrder
            // 
            this.menuCollapseCustomLoadOrder.CheckOnClick = true;
            this.menuCollapseCustomLoadOrder.Name = "menuCollapseCustomLoadOrder";
            this.menuCollapseCustomLoadOrder.Size = new System.Drawing.Size(244, 22);
            this.menuCollapseCustomLoadOrder.Text = "Resolved by &Custom Load Order";
            this.menuCollapseCustomLoadOrder.ToolTipText = "Auto-collapse conflicts that are resolved by your custom load order";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnCreateMerges;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 674);
            this.Controls.Add(this.lblGameDir);
            this.Controls.Add(this.txtGameDir);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnSelectGameDir);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(485, 350);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Merger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlProgress.ResumeLayout(false);
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
        private Controls.ConflictTree treConflicts;
        private System.Windows.Forms.Button btnRefreshConflicts;
        private System.Windows.Forms.Button btnCreateMerges;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblMergeInventory;
        private System.Windows.Forms.Button btnDeleteMerges;
        private Controls.MergeTree treMerges;
        private System.Windows.Forms.Label lblConflicts;
        private System.Windows.Forms.Button btnRefreshMerged;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgressCurrentAction;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuCheckScripts;
        private System.Windows.Forms.ToolStripMenuItem menuCheckBundleContents;
        private System.Windows.Forms.ToolStripMenuItem menuReviewEach;
        private System.Windows.Forms.ToolStripMenuItem menuPathsInKDiff3;
        private System.Windows.Forms.ToolStripMenuItem menuMergeReport;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusLeft;
        private System.Windows.Forms.ToolStripMenuItem menuShowStatusBar;
        private System.Windows.Forms.Label lblProgressCurrentPhase;
        private System.Windows.Forms.ToolStripMenuItem menuPackReport;
        private System.Windows.Forms.ToolStripMenuItem menuRefreshingConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuMerging;
        private System.Windows.Forms.ToolStripMenuItem menuDependencies;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusRight;
        private System.Windows.Forms.ToolStripMenuItem menuCompletionSounds;
        private System.Windows.Forms.Label lblGameDir;
        private System.Windows.Forms.ToolStripMenuItem menuCheckXmlFiles;
        private System.Windows.Forms.ToolStripMenuItem menuValidateCustomLoadOrder;
        private System.Windows.Forms.ToolStripMenuItem menuCollapseIf;
        private System.Windows.Forms.ToolStripMenuItem menuCollapseNotMergeable;
        private System.Windows.Forms.ToolStripMenuItem menuCollapseCustomLoadOrder;
    }
}

