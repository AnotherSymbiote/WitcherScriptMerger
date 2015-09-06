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
            this.lblGameDir = new System.Windows.Forms.Label();
            this.btnSelectGameDir = new System.Windows.Forms.Button();
            this.treConflicts = new System.Windows.Forms.TreeView();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextVanillaScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextVanillaDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextModScript = new System.Windows.Forms.ToolStripMenuItem();
            this.contextModDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSelectSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExpandSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCheckForConflicts = new System.Windows.Forms.Button();
            this.btnTryMergeSelected = new System.Windows.Forms.Button();
            this.lblMergedModName = new System.Windows.Forms.Label();
            this.txtMergedModName = new System.Windows.Forms.TextBox();
            this.chkCheckAtLaunch = new System.Windows.Forms.CheckBox();
            this.chkIgnoreWhitespace = new System.Windows.Forms.CheckBox();
            this.treeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGameDir
            // 
            this.txtGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGameDir.Location = new System.Drawing.Point(116, 14);
            this.txtGameDir.Name = "txtGameDir";
            this.txtGameDir.Size = new System.Drawing.Size(207, 20);
            this.txtGameDir.TabIndex = 0;
            this.txtGameDir.TextChanged += new System.EventHandler(this.txtGameDir_TextChanged);
            this.txtGameDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblGameDir
            // 
            this.lblGameDir.AutoSize = true;
            this.lblGameDir.Location = new System.Drawing.Point(9, 17);
            this.lblGameDir.Name = "lblGameDir";
            this.lblGameDir.Size = new System.Drawing.Size(101, 13);
            this.lblGameDir.TabIndex = 1;
            this.lblGameDir.Text = "Witcher 3 Directory:";
            // 
            // btnSelectGameDir
            // 
            this.btnSelectGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectGameDir.Location = new System.Drawing.Point(329, 12);
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
            this.treConflicts.Location = new System.Drawing.Point(12, 90);
            this.treConflicts.Name = "treConflicts";
            this.treConflicts.ShowNodeToolTips = true;
            this.treConflicts.ShowRootLines = false;
            this.treConflicts.Size = new System.Drawing.Size(453, 408);
            this.treConflicts.TabIndex = 4;
            this.treConflicts.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treConflicts_AfterCheck);
            this.treConflicts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treConflicts_AfterSelect);
            this.treConflicts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treConflicts_KeyDown);
            this.treConflicts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treConflicts_MouseDown);
            this.treConflicts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treConflicts_MouseUp);
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextVanillaScript,
            this.contextVanillaDir,
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
            this.treeContextMenu.Size = new System.Drawing.Size(226, 214);
            this.treeContextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.treeContextMenu_Closing);
            // 
            // contextVanillaScript
            // 
            this.contextVanillaScript.Name = "contextVanillaScript";
            this.contextVanillaScript.Size = new System.Drawing.Size(225, 22);
            this.contextVanillaScript.Text = "Open Vanilla Script";
            this.contextVanillaScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextVanillaDir
            // 
            this.contextVanillaDir.Name = "contextVanillaDir";
            this.contextVanillaDir.Size = new System.Drawing.Size(225, 22);
            this.contextVanillaDir.Text = "Open Vanilla Script Directory";
            this.contextVanillaDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextModScript
            // 
            this.contextModScript.Name = "contextModScript";
            this.contextModScript.Size = new System.Drawing.Size(225, 22);
            this.contextModScript.Text = "Open Mod Script";
            this.contextModScript.Click += new System.EventHandler(this.contextOpenScript_Click);
            // 
            // contextModDir
            // 
            this.contextModDir.Name = "contextModDir";
            this.contextModDir.Size = new System.Drawing.Size(225, 22);
            this.contextModDir.Text = "Open Mod Script Directory";
            this.contextModDir.Click += new System.EventHandler(this.contextOpenDirectory_Click);
            // 
            // contextCopyPath
            // 
            this.contextCopyPath.Name = "contextCopyPath";
            this.contextCopyPath.Size = new System.Drawing.Size(225, 22);
            this.contextCopyPath.Text = "Copy Path";
            this.contextCopyPath.Click += new System.EventHandler(this.contextCopyPath_Click);
            // 
            // contextSelectSeparator
            // 
            this.contextSelectSeparator.Name = "contextSelectSeparator";
            this.contextSelectSeparator.Size = new System.Drawing.Size(222, 6);
            // 
            // contextSelectAll
            // 
            this.contextSelectAll.Name = "contextSelectAll";
            this.contextSelectAll.Size = new System.Drawing.Size(225, 22);
            this.contextSelectAll.Text = "Select All";
            this.contextSelectAll.Click += new System.EventHandler(this.contextSelectAll_Click);
            // 
            // contextDeselectAll
            // 
            this.contextDeselectAll.Name = "contextDeselectAll";
            this.contextDeselectAll.Size = new System.Drawing.Size(225, 22);
            this.contextDeselectAll.Text = "Deselect All";
            this.contextDeselectAll.Click += new System.EventHandler(this.contextDeselectAll_Click);
            // 
            // contextExpandSeparator
            // 
            this.contextExpandSeparator.Name = "contextExpandSeparator";
            this.contextExpandSeparator.Size = new System.Drawing.Size(222, 6);
            // 
            // contextExpandAll
            // 
            this.contextExpandAll.Name = "contextExpandAll";
            this.contextExpandAll.Size = new System.Drawing.Size(225, 22);
            this.contextExpandAll.Text = "Expand All";
            this.contextExpandAll.Click += new System.EventHandler(this.contextExpandAll_Click);
            // 
            // contextCollapseAll
            // 
            this.contextCollapseAll.Name = "contextCollapseAll";
            this.contextCollapseAll.Size = new System.Drawing.Size(225, 22);
            this.contextCollapseAll.Text = "Collapse All";
            this.contextCollapseAll.Click += new System.EventHandler(this.contextCollapseAll_Click);
            // 
            // btnCheckForConflicts
            // 
            this.btnCheckForConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckForConflicts.Location = new System.Drawing.Point(12, 41);
            this.btnCheckForConflicts.Name = "btnCheckForConflicts";
            this.btnCheckForConflicts.Size = new System.Drawing.Size(453, 43);
            this.btnCheckForConflicts.TabIndex = 3;
            this.btnCheckForConflicts.Text = "&Check for Script File Conflicts";
            this.btnCheckForConflicts.UseVisualStyleBackColor = true;
            this.btnCheckForConflicts.Click += new System.EventHandler(this.btnCheckForConflicts_Click);
            // 
            // btnTryMergeSelected
            // 
            this.btnTryMergeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTryMergeSelected.Enabled = false;
            this.btnTryMergeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTryMergeSelected.Location = new System.Drawing.Point(12, 553);
            this.btnTryMergeSelected.Name = "btnTryMergeSelected";
            this.btnTryMergeSelected.Size = new System.Drawing.Size(453, 43);
            this.btnTryMergeSelected.TabIndex = 7;
            this.btnTryMergeSelected.Text = "Try to &Merge Selected Scripts";
            this.btnTryMergeSelected.UseVisualStyleBackColor = true;
            this.btnTryMergeSelected.Click += new System.EventHandler(this.btnTryMergeSelected_Click);
            // 
            // lblMergedModName
            // 
            this.lblMergedModName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMergedModName.AutoSize = true;
            this.lblMergedModName.Location = new System.Drawing.Point(12, 505);
            this.lblMergedModName.Name = "lblMergedModName";
            this.lblMergedModName.Size = new System.Drawing.Size(151, 13);
            this.lblMergedModName.TabIndex = 9;
            this.lblMergedModName.Text = "Mod Name for Merged Scripts:";
            // 
            // txtMergedModName
            // 
            this.txtMergedModName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMergedModName.Location = new System.Drawing.Point(12, 527);
            this.txtMergedModName.Name = "txtMergedModName";
            this.txtMergedModName.Size = new System.Drawing.Size(539, 20);
            this.txtMergedModName.TabIndex = 5;
            this.txtMergedModName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // chkCheckAtLaunch
            // 
            this.chkCheckAtLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCheckAtLaunch.AutoSize = true;
            this.chkCheckAtLaunch.Location = new System.Drawing.Point(361, 16);
            this.chkCheckAtLaunch.Name = "chkCheckAtLaunch";
            this.chkCheckAtLaunch.Size = new System.Drawing.Size(104, 17);
            this.chkCheckAtLaunch.TabIndex = 2;
            this.chkCheckAtLaunch.Text = "Check at &launch";
            this.chkCheckAtLaunch.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreWhitespace
            // 
            this.chkIgnoreWhitespace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreWhitespace.AutoSize = true;
            this.chkIgnoreWhitespace.Location = new System.Drawing.Point(286, 504);
            this.chkIgnoreWhitespace.Name = "chkIgnoreWhitespace";
            this.chkIgnoreWhitespace.Size = new System.Drawing.Size(179, 17);
            this.chkIgnoreWhitespace.TabIndex = 14;
            this.chkIgnoreWhitespace.Text = "Ignore &whitespace-only changes";
            this.chkIgnoreWhitespace.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnTryMergeSelected;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 602);
            this.Controls.Add(this.chkIgnoreWhitespace);
            this.Controls.Add(this.chkCheckAtLaunch);
            this.Controls.Add(this.lblMergedModName);
            this.Controls.Add(this.txtMergedModName);
            this.Controls.Add(this.btnTryMergeSelected);
            this.Controls.Add(this.btnCheckForConflicts);
            this.Controls.Add(this.treConflicts);
            this.Controls.Add(this.btnSelectGameDir);
            this.Controls.Add(this.lblGameDir);
            this.Controls.Add(this.txtGameDir);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(410, 280);
            this.Name = "MainForm";
            this.Text = "Script Merger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.treeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGameDir;
        private System.Windows.Forms.Label lblGameDir;
        private System.Windows.Forms.Button btnSelectGameDir;
        private System.Windows.Forms.TreeView treConflicts;
        private System.Windows.Forms.Button btnCheckForConflicts;
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
        private System.Windows.Forms.CheckBox chkCheckAtLaunch;
        private System.Windows.Forms.CheckBox chkIgnoreWhitespace;
        private System.Windows.Forms.ToolStripSeparator contextSelectSeparator;
        private System.Windows.Forms.ToolStripMenuItem contextSelectAll;
        private System.Windows.Forms.ToolStripMenuItem contextDeselectAll;
        private System.Windows.Forms.ToolStripSeparator contextExpandSeparator;
    }
}

