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
            this.contextVanillaScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextVanillaDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMergedScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMergedDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextModScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextModDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSelectSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExpandSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefreshConflicts = new System.Windows.Forms.Button();
            this.btnTryMergeSelected = new System.Windows.Forms.Button();
            this.lblMergedModName = new System.Windows.Forms.Label();
            this.txtMergedModName = new System.Windows.Forms.TextBox();
            this.chkReviewEachMerge = new System.Windows.Forms.CheckBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lblConflicts = new System.Windows.Forms.Label();
            this.btnSelectBackupDir = new System.Windows.Forms.Button();
            this.txtBackupDir = new System.Windows.Forms.TextBox();
            this.lblBackupDir = new System.Windows.Forms.Label();
            this.btnRefreshMerged = new System.Windows.Forms.Button();
            this.lblMergeInventory = new System.Windows.Forms.Label();
            this.btnUnmergeSelected = new System.Windows.Forms.Button();
            this.treMergeInventory = new System.Windows.Forms.TreeView();
            this.grpGameDir = new System.Windows.Forms.GroupBox();
            this.treeContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.grpGameDir.SuspendLayout();
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
            this.treConflicts.Size = new System.Drawing.Size(312, 449);
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
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextVanillaScript,
            this.contextVanillaDir,
            this.contextMergedScript,
            this.contextMergedDir,
            this.contextModScript,
            this.contextModDir,
            this.contextCopyPath,
            this.contextSelectSeparator,
            this.contextSelectAll,
            this.contextDeselectAll,
            this.contextExpandSeparator,
            this.contextExpandAll,
            this.contextCollapseAll});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(232, 258);
            this.treeContextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.treeContextMenu_Closing);
            // 
            // contextVanillaScript
            // 
            this.contextVanillaScript.Name = "contextVanillaScript";
            this.contextVanillaScript.Size = new System.Drawing.Size(231, 22);
            this.contextVanillaScript.Text = "Open Vanilla Script";
            this.contextVanillaScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextVanillaDir
            // 
            this.contextVanillaDir.Name = "contextVanillaDir";
            this.contextVanillaDir.Size = new System.Drawing.Size(231, 22);
            this.contextVanillaDir.Text = "Open Vanilla Script Directory";
            this.contextVanillaDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextMergedScript
            // 
            this.contextMergedScript.Name = "contextMergedScript";
            this.contextMergedScript.Size = new System.Drawing.Size(231, 22);
            this.contextMergedScript.Text = "Open Merged Script";
            this.contextMergedScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextMergedDir
            // 
            this.contextMergedDir.Name = "contextMergedDir";
            this.contextMergedDir.Size = new System.Drawing.Size(231, 22);
            this.contextMergedDir.Text = "Open Merged Script Directory";
            this.contextMergedDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextModScript
            // 
            this.contextModScript.Name = "contextModScript";
            this.contextModScript.Size = new System.Drawing.Size(231, 22);
            this.contextModScript.Text = "Open Mod Script";
            this.contextModScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextModDir
            // 
            this.contextModDir.Name = "contextModDir";
            this.contextModDir.Size = new System.Drawing.Size(231, 22);
            this.contextModDir.Text = "Open Mod Script Directory";
            this.contextModDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextCopyPath
            // 
            this.contextCopyPath.Name = "contextCopyPath";
            this.contextCopyPath.Size = new System.Drawing.Size(231, 22);
            this.contextCopyPath.Text = "Copy Path";
            this.contextCopyPath.Click += new System.EventHandler(this.contextCopyPath_Click);
            // 
            // contextSelectSeparator
            // 
            this.contextSelectSeparator.Name = "contextSelectSeparator";
            this.contextSelectSeparator.Size = new System.Drawing.Size(228, 6);
            // 
            // contextSelectAll
            // 
            this.contextSelectAll.Name = "contextSelectAll";
            this.contextSelectAll.Size = new System.Drawing.Size(231, 22);
            this.contextSelectAll.Text = "Select All";
            this.contextSelectAll.Click += new System.EventHandler(this.contextSelectAll_Click);
            // 
            // contextDeselectAll
            // 
            this.contextDeselectAll.Name = "contextDeselectAll";
            this.contextDeselectAll.Size = new System.Drawing.Size(231, 22);
            this.contextDeselectAll.Text = "Deselect All";
            this.contextDeselectAll.Click += new System.EventHandler(this.contextDeselectAll_Click);
            // 
            // contextExpandSeparator
            // 
            this.contextExpandSeparator.Name = "contextExpandSeparator";
            this.contextExpandSeparator.Size = new System.Drawing.Size(228, 6);
            // 
            // contextExpandAll
            // 
            this.contextExpandAll.Name = "contextExpandAll";
            this.contextExpandAll.Size = new System.Drawing.Size(231, 22);
            this.contextExpandAll.Text = "Expand All";
            this.contextExpandAll.Click += new System.EventHandler(this.contextExpandAll_Click);
            // 
            // contextCollapseAll
            // 
            this.contextCollapseAll.Name = "contextCollapseAll";
            this.contextCollapseAll.Size = new System.Drawing.Size(231, 22);
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
            // btnTryMergeSelected
            // 
            this.btnTryMergeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTryMergeSelected.Enabled = false;
            this.btnTryMergeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTryMergeSelected.Location = new System.Drawing.Point(12, 564);
            this.btnTryMergeSelected.Name = "btnTryMergeSelected";
            this.btnTryMergeSelected.Size = new System.Drawing.Size(312, 35);
            this.btnTryMergeSelected.TabIndex = 6;
            this.btnTryMergeSelected.Text = "Try to &Merge Selected Script";
            this.btnTryMergeSelected.UseVisualStyleBackColor = true;
            this.btnTryMergeSelected.Click += new System.EventHandler(this.btnTryMergeSelected_Click);
            // 
            // lblMergedModName
            // 
            this.lblMergedModName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMergedModName.AutoSize = true;
            this.lblMergedModName.Location = new System.Drawing.Point(8, 488);
            this.lblMergedModName.Name = "lblMergedModName";
            this.lblMergedModName.Size = new System.Drawing.Size(101, 13);
            this.lblMergedModName.TabIndex = 9;
            this.lblMergedModName.Text = "Merged Mod Name:";
            // 
            // txtMergedModName
            // 
            this.txtMergedModName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMergedModName.Location = new System.Drawing.Point(115, 485);
            this.txtMergedModName.Name = "txtMergedModName";
            this.txtMergedModName.Size = new System.Drawing.Size(209, 20);
            this.txtMergedModName.TabIndex = 2;
            this.txtMergedModName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // chkReviewEachMerge
            // 
            this.chkReviewEachMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkReviewEachMerge.AutoSize = true;
            this.chkReviewEachMerge.Location = new System.Drawing.Point(20, 541);
            this.chkReviewEachMerge.Name = "chkReviewEachMerge";
            this.chkReviewEachMerge.Size = new System.Drawing.Size(228, 17);
            this.chkReviewEachMerge.TabIndex = 5;
            this.chkReviewEachMerge.Text = "Review &each merge (even if auto-solvable)";
            this.chkReviewEachMerge.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 63);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lblConflicts);
            this.splitContainer.Panel1.Controls.Add(this.chkReviewEachMerge);
            this.splitContainer.Panel1.Controls.Add(this.btnSelectBackupDir);
            this.splitContainer.Panel1.Controls.Add(this.treConflicts);
            this.splitContainer.Panel1.Controls.Add(this.txtBackupDir);
            this.splitContainer.Panel1.Controls.Add(this.lblBackupDir);
            this.splitContainer.Panel1.Controls.Add(this.lblMergedModName);
            this.splitContainer.Panel1.Controls.Add(this.btnRefreshConflicts);
            this.splitContainer.Panel1.Controls.Add(this.txtMergedModName);
            this.splitContainer.Panel1.Controls.Add(this.btnTryMergeSelected);
            this.splitContainer.Panel1MinSize = 250;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnRefreshMerged);
            this.splitContainer.Panel2.Controls.Add(this.lblMergeInventory);
            this.splitContainer.Panel2.Controls.Add(this.btnUnmergeSelected);
            this.splitContainer.Panel2.Controls.Add(this.treMergeInventory);
            this.splitContainer.Panel2MinSize = 235;
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
            // btnSelectBackupDir
            // 
            this.btnSelectBackupDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectBackupDir.Location = new System.Drawing.Point(298, 511);
            this.btnSelectBackupDir.Name = "btnSelectBackupDir";
            this.btnSelectBackupDir.Size = new System.Drawing.Size(26, 23);
            this.btnSelectBackupDir.TabIndex = 4;
            this.btnSelectBackupDir.Text = "...";
            this.btnSelectBackupDir.UseVisualStyleBackColor = true;
            this.btnSelectBackupDir.Click += new System.EventHandler(this.btnSelectBackupDir_Click);
            // 
            // txtBackupDir
            // 
            this.txtBackupDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupDir.Location = new System.Drawing.Point(115, 513);
            this.txtBackupDir.Name = "txtBackupDir";
            this.txtBackupDir.Size = new System.Drawing.Size(177, 20);
            this.txtBackupDir.TabIndex = 3;
            this.txtBackupDir.TextChanged += new System.EventHandler(this.txtBackupDir_TextChanged);
            this.txtBackupDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblBackupDir
            // 
            this.lblBackupDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBackupDir.AutoSize = true;
            this.lblBackupDir.Location = new System.Drawing.Point(17, 516);
            this.lblBackupDir.Name = "lblBackupDir";
            this.lblBackupDir.Size = new System.Drawing.Size(92, 13);
            this.lblBackupDir.TabIndex = 11;
            this.lblBackupDir.Text = "Backup Directory:";
            // 
            // btnRefreshMerged
            // 
            this.btnRefreshMerged.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshMerged.Location = new System.Drawing.Point(126, 3);
            this.btnRefreshMerged.MaximumSize = new System.Drawing.Size(150, 23);
            this.btnRefreshMerged.MinimumSize = new System.Drawing.Size(100, 23);
            this.btnRefreshMerged.Name = "btnRefreshMerged";
            this.btnRefreshMerged.Size = new System.Drawing.Size(117, 23);
            this.btnRefreshMerged.TabIndex = 8;
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
            this.lblMergeInventory.Size = new System.Drawing.Size(117, 16);
            this.lblMergeInventory.TabIndex = 7;
            this.lblMergeInventory.Text = "Merged Scripts:";
            // 
            // btnUnmergeSelected
            // 
            this.btnUnmergeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnmergeSelected.Enabled = false;
            this.btnUnmergeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnmergeSelected.Location = new System.Drawing.Point(3, 564);
            this.btnUnmergeSelected.Name = "btnUnmergeSelected";
            this.btnUnmergeSelected.Size = new System.Drawing.Size(310, 35);
            this.btnUnmergeSelected.TabIndex = 1;
            this.btnUnmergeSelected.Text = "&Unmerge Selected Script";
            this.btnUnmergeSelected.UseVisualStyleBackColor = true;
            this.btnUnmergeSelected.Click += new System.EventHandler(this.btnUnmergeSelected_Click);
            // 
            // treMergeInventory
            // 
            this.treMergeInventory.AllowDrop = true;
            this.treMergeInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treMergeInventory.CheckBoxes = true;
            this.treMergeInventory.Location = new System.Drawing.Point(3, 30);
            this.treMergeInventory.Name = "treMergeInventory";
            this.treMergeInventory.ShowNodeToolTips = true;
            this.treMergeInventory.ShowRootLines = false;
            this.treMergeInventory.Size = new System.Drawing.Size(310, 528);
            this.treMergeInventory.TabIndex = 0;
            this.treMergeInventory.TabStop = false;
            this.treMergeInventory.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            this.treMergeInventory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.treMergeInventory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            this.treMergeInventory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // grpGameDir
            // 
            this.grpGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGameDir.Controls.Add(this.txtGameDir);
            this.grpGameDir.Controls.Add(this.btnSelectGameDir);
            this.grpGameDir.Location = new System.Drawing.Point(11, 7);
            this.grpGameDir.Name = "grpGameDir";
            this.grpGameDir.Size = new System.Drawing.Size(633, 50);
            this.grpGameDir.TabIndex = 0;
            this.grpGameDir.TabStop = false;
            this.grpGameDir.Text = "Witcher 3 Directory";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnTryMergeSelected;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 674);
            this.Controls.Add(this.grpGameDir);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(505, 350);
            this.Name = "MainForm";
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
            this.grpGameDir.ResumeLayout(false);
            this.grpGameDir.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtGameDir;
        private System.Windows.Forms.Button btnSelectGameDir;
        private System.Windows.Forms.TreeView treConflicts;
        private System.Windows.Forms.Button btnRefreshConflicts;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextModDir;
        private System.Windows.Forms.ToolStripMenuItem contextCopyPath;
        private System.Windows.Forms.ToolStripMenuItem contextExpandAll;
        private System.Windows.Forms.ToolStripMenuItem contextCollapseAll;
        private System.Windows.Forms.ToolStripMenuItem contextModScript;
        private System.Windows.Forms.ToolStripMenuItem contextVanillaScript;
        private System.Windows.Forms.ToolStripMenuItem contextVanillaDir;
        private System.Windows.Forms.Button btnTryMergeSelected;
        private System.Windows.Forms.Label lblMergedModName;
        private System.Windows.Forms.TextBox txtMergedModName;
        private System.Windows.Forms.ToolStripSeparator contextSelectSeparator;
        private System.Windows.Forms.ToolStripMenuItem contextSelectAll;
        private System.Windows.Forms.ToolStripMenuItem contextDeselectAll;
        private System.Windows.Forms.ToolStripSeparator contextExpandSeparator;
        private System.Windows.Forms.CheckBox chkReviewEachMerge;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblMergeInventory;
        private System.Windows.Forms.Button btnUnmergeSelected;
        private System.Windows.Forms.TreeView treMergeInventory;
        private System.Windows.Forms.GroupBox grpGameDir;
        private System.Windows.Forms.Label lblConflicts;
        private System.Windows.Forms.Label lblBackupDir;
        private System.Windows.Forms.TextBox txtBackupDir;
        private System.Windows.Forms.Button btnSelectBackupDir;
        private System.Windows.Forms.ToolStripMenuItem contextMergedScript;
        private System.Windows.Forms.ToolStripMenuItem contextMergedDir;
        private System.Windows.Forms.Button btnRefreshMerged;
    }
}

