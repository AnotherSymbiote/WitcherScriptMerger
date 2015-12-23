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
            this.lblProgressOf = new System.Windows.Forms.Label();
            this.lblProgressState = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpGameDir = new System.Windows.Forms.GroupBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckingForConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckBundleContents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollapseUnsupported = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMerging = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReviewEach = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPathsInKDiff3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMergeReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPackReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlProgress.SuspendLayout();
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
            this.btnCreateMerges.Location = new System.Drawing.Point(12, 525);
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
            this.splitContainer.Location = new System.Drawing.Point(0, 86);
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
            this.splitContainer.Size = new System.Drawing.Size(654, 563);
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
            this.treConflicts.Location = new System.Drawing.Point(12, 30);
            this.treConflicts.Name = "treConflicts";
            this.treConflicts.ShowNodeToolTips = true;
            this.treConflicts.Size = new System.Drawing.Size(312, 489);
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
            this.treMerges.Size = new System.Drawing.Size(310, 489);
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
            this.pnlProgress.Controls.Add(this.lblProgressOf);
            this.pnlProgress.Controls.Add(this.lblProgressState);
            this.pnlProgress.Controls.Add(this.progressBar);
            this.pnlProgress.Location = new System.Drawing.Point(9, 30);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Padding = new System.Windows.Forms.Padding(8);
            this.pnlProgress.Size = new System.Drawing.Size(635, 619);
            this.pnlProgress.TabIndex = 8;
            this.pnlProgress.Visible = false;
            // 
            // lblProgressOf
            // 
            this.lblProgressOf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressOf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressOf.Location = new System.Drawing.Point(11, 283);
            this.lblProgressOf.Name = "lblProgressOf";
            this.lblProgressOf.Size = new System.Drawing.Size(611, 20);
            this.lblProgressOf.TabIndex = 2;
            this.lblProgressOf.Text = "Initializing";
            this.lblProgressOf.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblProgressState
            // 
            this.lblProgressState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressState.Location = new System.Drawing.Point(11, 333);
            this.lblProgressState.Name = "lblProgressState";
            this.lblProgressState.Size = new System.Drawing.Size(611, 129);
            this.lblProgressState.TabIndex = 1;
            this.lblProgressState.Text = "...";
            this.lblProgressState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(25, 308);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(583, 20);
            this.progressBar.TabIndex = 0;
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
            this.menuCheckingForConflicts,
            this.menuMerging,
            this.menuDependencies,
            this.menuShowStatusBar});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // menuCheckingForConflicts
            // 
            this.menuCheckingForConflicts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCheckScripts,
            this.menuCheckBundleContents,
            this.menuCollapseUnsupported});
            this.menuCheckingForConflicts.Name = "menuCheckingForConflicts";
            this.menuCheckingForConflicts.Size = new System.Drawing.Size(203, 22);
            this.menuCheckingForConflicts.Text = "&Refreshing Conflicts...";
            // 
            // menuCheckScripts
            // 
            this.menuCheckScripts.CheckOnClick = true;
            this.menuCheckScripts.Name = "menuCheckScripts";
            this.menuCheckScripts.Size = new System.Drawing.Size(222, 22);
            this.menuCheckScripts.Text = "Check &Scripts";
            // 
            // menuCheckBundleContents
            // 
            this.menuCheckBundleContents.CheckOnClick = true;
            this.menuCheckBundleContents.Name = "menuCheckBundleContents";
            this.menuCheckBundleContents.Size = new System.Drawing.Size(222, 22);
            this.menuCheckBundleContents.Text = "Check &Bundle Contents";
            // 
            // menuCollapseUnsupported
            // 
            this.menuCollapseUnsupported.CheckOnClick = true;
            this.menuCollapseUnsupported.Name = "menuCollapseUnsupported";
            this.menuCollapseUnsupported.Size = new System.Drawing.Size(222, 22);
            this.menuCollapseUnsupported.Text = "Auto-Collapse &Unsupported";
            // 
            // menuMerging
            // 
            this.menuMerging.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuReviewEach,
            this.menuPathsInKDiff3,
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
            // 
            // menuPathsInKDiff3
            // 
            this.menuPathsInKDiff3.CheckOnClick = true;
            this.menuPathsInKDiff3.Name = "menuPathsInKDiff3";
            this.menuPathsInKDiff3.Size = new System.Drawing.Size(297, 22);
            this.menuPathsInKDiff3.Text = "Show File Paths in &KDiff3";
            // 
            // menuMergeReport
            // 
            this.menuMergeReport.CheckOnClick = true;
            this.menuMergeReport.Name = "menuMergeReport";
            this.menuMergeReport.Size = new System.Drawing.Size(297, 22);
            this.menuMergeReport.Text = "Show Report After Each &Merge";
            // 
            // menuPackReport
            // 
            this.menuPackReport.Name = "menuPackReport";
            this.menuPackReport.Size = new System.Drawing.Size(297, 22);
            this.menuPackReport.Text = "Show Report After &Packing Bundle";
            // 
            // menuDependencies
            // 
            this.menuDependencies.Name = "menuDependencies";
            this.menuDependencies.Size = new System.Drawing.Size(203, 22);
            this.menuDependencies.Text = "&Dependency Locations...";
            this.menuDependencies.Click += new System.EventHandler(this.menuDependencies_Click);
            // 
            // menuShowStatusBar
            // 
            this.menuShowStatusBar.CheckOnClick = true;
            this.menuShowStatusBar.Name = "menuShowStatusBar";
            this.menuShowStatusBar.Size = new System.Drawing.Size(203, 22);
            this.menuShowStatusBar.Text = "&Show Status Bar";
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
            this.lblStatus.Size = new System.Drawing.Size(58, 14);
            this.lblStatus.Text = "Loading...";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnCreateMerges;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 674);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.grpGameDir);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(485, 350);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Merger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlProgress.ResumeLayout(false);
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
        private Controls.ConflictTree treConflicts;
        private System.Windows.Forms.Button btnRefreshConflicts;
        private System.Windows.Forms.Button btnCreateMerges;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblMergeInventory;
        private System.Windows.Forms.Button btnDeleteMerges;
        private Controls.MergeTree treMerges;
        private System.Windows.Forms.GroupBox grpGameDir;
        private System.Windows.Forms.Label lblConflicts;
        private System.Windows.Forms.Button btnRefreshMerged;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgressState;
        private System.Windows.Forms.ProgressBar progressBar;
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
        private System.Windows.Forms.Label lblProgressOf;
        private System.Windows.Forms.ToolStripMenuItem menuPackReport;
        private System.Windows.Forms.ToolStripMenuItem menuCollapseUnsupported;
        private System.Windows.Forms.ToolStripMenuItem menuCheckingForConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuMerging;
        private System.Windows.Forms.ToolStripMenuItem menuDependencies;
    }
}

