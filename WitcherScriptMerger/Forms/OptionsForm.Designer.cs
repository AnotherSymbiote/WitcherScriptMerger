namespace WitcherScriptMerger.Forms
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.grpCheckForConflicts = new System.Windows.Forms.GroupBox();
            this.chkCheckBundleContents = new System.Windows.Forms.CheckBox();
            this.chkCheckXmlFiles = new System.Windows.Forms.CheckBox();
            this.chkCheckScripts = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.grpRefreshingMerges = new System.Windows.Forms.GroupBox();
            this.chkPromptPrioritize = new System.Windows.Forms.CheckBox();
            this.chkPromptOutdatedMerge = new System.Windows.Forms.CheckBox();
            this.grpMerging = new System.Windows.Forms.GroupBox();
            this.chkPackReport = new System.Windows.Forms.CheckBox();
            this.chkMergeReport = new System.Windows.Forms.CheckBox();
            this.chkShowPathsInKDiff3 = new System.Windows.Forms.CheckBox();
            this.chkCompletionSounds = new System.Windows.Forms.CheckBox();
            this.chkReviewEachMerge = new System.Windows.Forms.CheckBox();
            this.grpAutocollapse = new System.Windows.Forms.GroupBox();
            this.chkCollapseCustomLoadOrder = new System.Windows.Forms.CheckBox();
            this.chkCollapseNotMergeable = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.grpCheckForConflicts.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpRefreshingMerges.SuspendLayout();
            this.grpMerging.SuspendLayout();
            this.grpAutocollapse.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCheckForConflicts
            // 
            this.grpCheckForConflicts.Controls.Add(this.chkCheckBundleContents);
            this.grpCheckForConflicts.Controls.Add(this.chkCheckXmlFiles);
            this.grpCheckForConflicts.Controls.Add(this.chkCheckScripts);
            this.grpCheckForConflicts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCheckForConflicts.Location = new System.Drawing.Point(12, 12);
            this.grpCheckForConflicts.Name = "grpCheckForConflicts";
            this.grpCheckForConflicts.Size = new System.Drawing.Size(282, 94);
            this.grpCheckForConflicts.TabIndex = 1;
            this.grpCheckForConflicts.TabStop = false;
            this.grpCheckForConflicts.Text = "Check for Conflicts";
            // 
            // chkCheckBundleContents
            // 
            this.chkCheckBundleContents.AutoSize = true;
            this.chkCheckBundleContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckBundleContents.Location = new System.Drawing.Point(6, 66);
            this.chkCheckBundleContents.Name = "chkCheckBundleContents";
            this.chkCheckBundleContents.Size = new System.Drawing.Size(86, 17);
            this.chkCheckBundleContents.TabIndex = 2;
            this.chkCheckBundleContents.Text = "Bundled files";
            this.chkCheckBundleContents.UseVisualStyleBackColor = true;
            // 
            // chkCheckXmlFiles
            // 
            this.chkCheckXmlFiles.AutoSize = true;
            this.chkCheckXmlFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckXmlFiles.Location = new System.Drawing.Point(6, 43);
            this.chkCheckXmlFiles.Name = "chkCheckXmlFiles";
            this.chkCheckXmlFiles.Size = new System.Drawing.Size(133, 17);
            this.chkCheckXmlFiles.TabIndex = 1;
            this.chkCheckXmlFiles.Text = "Non-bundled XML files";
            this.chkCheckXmlFiles.UseVisualStyleBackColor = true;
            // 
            // chkCheckScripts
            // 
            this.chkCheckScripts.AutoSize = true;
            this.chkCheckScripts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckScripts.Location = new System.Drawing.Point(6, 20);
            this.chkCheckScripts.Name = "chkCheckScripts";
            this.chkCheckScripts.Size = new System.Drawing.Size(58, 17);
            this.chkCheckScripts.TabIndex = 0;
            this.chkCheckScripts.Text = "Scripts";
            this.chkCheckScripts.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox11);
            this.groupBox1.Controls.Add(this.checkBox12);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 71);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto-collapse conflicts in tree if...";
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox11.Location = new System.Drawing.Point(6, 43);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(172, 17);
            this.checkBox11.TabIndex = 1;
            this.checkBox11.Text = "Resolved by custom load order";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox12.Location = new System.Drawing.Point(6, 20);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(95, 17);
            this.checkBox12.TabIndex = 0;
            this.checkBox12.Text = "Not mergeable";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // grpRefreshingMerges
            // 
            this.grpRefreshingMerges.Controls.Add(this.chkPromptPrioritize);
            this.grpRefreshingMerges.Controls.Add(this.chkPromptOutdatedMerge);
            this.grpRefreshingMerges.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRefreshingMerges.Location = new System.Drawing.Point(12, 206);
            this.grpRefreshingMerges.Name = "grpRefreshingMerges";
            this.grpRefreshingMerges.Size = new System.Drawing.Size(282, 71);
            this.grpRefreshingMerges.TabIndex = 0;
            this.grpRefreshingMerges.TabStop = false;
            this.grpRefreshingMerges.Text = "Refreshing Merges";
            // 
            // chkPromptPrioritize
            // 
            this.chkPromptPrioritize.AutoSize = true;
            this.chkPromptPrioritize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPromptPrioritize.Location = new System.Drawing.Point(6, 42);
            this.chkPromptPrioritize.Name = "chkPromptPrioritize";
            this.chkPromptPrioritize.Size = new System.Drawing.Size(269, 17);
            this.chkPromptPrioritize.TabIndex = 4;
            this.chkPromptPrioritize.Text = "Prompt to prioritize merged files in custom load order";
            this.chkPromptPrioritize.UseVisualStyleBackColor = true;
            // 
            // chkPromptOutdatedMerge
            // 
            this.chkPromptOutdatedMerge.AutoSize = true;
            this.chkPromptOutdatedMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPromptOutdatedMerge.Location = new System.Drawing.Point(6, 19);
            this.chkPromptOutdatedMerge.Name = "chkPromptOutdatedMerge";
            this.chkPromptOutdatedMerge.Size = new System.Drawing.Size(185, 17);
            this.chkPromptOutdatedMerge.TabIndex = 3;
            this.chkPromptOutdatedMerge.Text = "Prompt to delete outdated merges";
            this.chkPromptOutdatedMerge.UseVisualStyleBackColor = true;
            // 
            // grpMerging
            // 
            this.grpMerging.Controls.Add(this.chkPackReport);
            this.grpMerging.Controls.Add(this.chkMergeReport);
            this.grpMerging.Controls.Add(this.chkShowPathsInKDiff3);
            this.grpMerging.Controls.Add(this.chkCompletionSounds);
            this.grpMerging.Controls.Add(this.chkReviewEachMerge);
            this.grpMerging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMerging.Location = new System.Drawing.Point(12, 291);
            this.grpMerging.Name = "grpMerging";
            this.grpMerging.Size = new System.Drawing.Size(282, 140);
            this.grpMerging.TabIndex = 1;
            this.grpMerging.TabStop = false;
            this.grpMerging.Text = "Merging";
            // 
            // chkPackReport
            // 
            this.chkPackReport.AutoSize = true;
            this.chkPackReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPackReport.Location = new System.Drawing.Point(6, 111);
            this.chkPackReport.Name = "chkPackReport";
            this.chkPackReport.Size = new System.Drawing.Size(183, 17);
            this.chkPackReport.TabIndex = 7;
            this.chkPackReport.Text = "Show report after packing bundle";
            this.chkPackReport.UseVisualStyleBackColor = true;
            // 
            // chkMergeReport
            // 
            this.chkMergeReport.AutoSize = true;
            this.chkMergeReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMergeReport.Location = new System.Drawing.Point(6, 88);
            this.chkMergeReport.Name = "chkMergeReport";
            this.chkMergeReport.Size = new System.Drawing.Size(166, 17);
            this.chkMergeReport.TabIndex = 6;
            this.chkMergeReport.Text = "Show report after each merge";
            this.chkMergeReport.UseVisualStyleBackColor = true;
            // 
            // chkShowPathsInKDiff3
            // 
            this.chkShowPathsInKDiff3.AutoSize = true;
            this.chkShowPathsInKDiff3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPathsInKDiff3.Location = new System.Drawing.Point(6, 42);
            this.chkShowPathsInKDiff3.Name = "chkShowPathsInKDiff3";
            this.chkShowPathsInKDiff3.Size = new System.Drawing.Size(141, 17);
            this.chkShowPathsInKDiff3.TabIndex = 5;
            this.chkShowPathsInKDiff3.Text = "Show file paths in KDiff3";
            this.chkShowPathsInKDiff3.UseVisualStyleBackColor = true;
            // 
            // chkCompletionSounds
            // 
            this.chkCompletionSounds.AutoSize = true;
            this.chkCompletionSounds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCompletionSounds.Location = new System.Drawing.Point(6, 65);
            this.chkCompletionSounds.Name = "chkCompletionSounds";
            this.chkCompletionSounds.Size = new System.Drawing.Size(137, 17);
            this.chkCompletionSounds.TabIndex = 4;
            this.chkCompletionSounds.Text = "Play completion sounds";
            this.chkCompletionSounds.UseVisualStyleBackColor = true;
            // 
            // chkReviewEachMerge
            // 
            this.chkReviewEachMerge.AutoSize = true;
            this.chkReviewEachMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReviewEachMerge.Location = new System.Drawing.Point(6, 19);
            this.chkReviewEachMerge.Name = "chkReviewEachMerge";
            this.chkReviewEachMerge.Size = new System.Drawing.Size(271, 17);
            this.chkReviewEachMerge.TabIndex = 3;
            this.chkReviewEachMerge.Text = "Review each merge in KDiff3 (even if auto-solvable)";
            this.chkReviewEachMerge.UseVisualStyleBackColor = true;
            // 
            // grpAutocollapse
            // 
            this.grpAutocollapse.Controls.Add(this.chkCollapseCustomLoadOrder);
            this.grpAutocollapse.Controls.Add(this.chkCollapseNotMergeable);
            this.grpAutocollapse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAutocollapse.Location = new System.Drawing.Point(12, 121);
            this.grpAutocollapse.Name = "grpAutocollapse";
            this.grpAutocollapse.Size = new System.Drawing.Size(282, 71);
            this.grpAutocollapse.TabIndex = 3;
            this.grpAutocollapse.TabStop = false;
            this.grpAutocollapse.Text = "Auto-collapse Conflicts in Tree";
            // 
            // chkCollapseCustomLoadOrder
            // 
            this.chkCollapseCustomLoadOrder.AutoSize = true;
            this.chkCollapseCustomLoadOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCollapseCustomLoadOrder.Location = new System.Drawing.Point(6, 43);
            this.chkCollapseCustomLoadOrder.Name = "chkCollapseCustomLoadOrder";
            this.chkCollapseCustomLoadOrder.Size = new System.Drawing.Size(172, 17);
            this.chkCollapseCustomLoadOrder.TabIndex = 1;
            this.chkCollapseCustomLoadOrder.Text = "Resolved by custom load order";
            this.chkCollapseCustomLoadOrder.UseVisualStyleBackColor = true;
            // 
            // chkCollapseNotMergeable
            // 
            this.chkCollapseNotMergeable.AutoSize = true;
            this.chkCollapseNotMergeable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCollapseNotMergeable.Location = new System.Drawing.Point(6, 20);
            this.chkCollapseNotMergeable.Name = "chkCollapseNotMergeable";
            this.chkCollapseNotMergeable.Size = new System.Drawing.Size(95, 17);
            this.chkCollapseNotMergeable.TabIndex = 0;
            this.chkCollapseNotMergeable.Text = "Not mergeable";
            this.chkCollapseNotMergeable.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(108, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Ca&ncel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(12, 442);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnApply.Location = new System.Drawing.Point(204, 442);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 25);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(306, 478);
            this.ControlBox = false;
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpAutocollapse);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpMerging);
            this.Controls.Add(this.grpCheckForConflicts);
            this.Controls.Add(this.grpRefreshingMerges);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.grpCheckForConflicts.ResumeLayout(false);
            this.grpCheckForConflicts.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpRefreshingMerges.ResumeLayout(false);
            this.grpRefreshingMerges.PerformLayout();
            this.grpMerging.ResumeLayout(false);
            this.grpMerging.PerformLayout();
            this.grpAutocollapse.ResumeLayout(false);
            this.grpAutocollapse.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpCheckForConflicts;
        private System.Windows.Forms.CheckBox chkCheckBundleContents;
        private System.Windows.Forms.CheckBox chkCheckXmlFiles;
        private System.Windows.Forms.CheckBox chkCheckScripts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.GroupBox grpRefreshingMerges;
        private System.Windows.Forms.CheckBox chkPromptPrioritize;
        private System.Windows.Forms.CheckBox chkPromptOutdatedMerge;
        private System.Windows.Forms.GroupBox grpMerging;
        private System.Windows.Forms.CheckBox chkPackReport;
        private System.Windows.Forms.CheckBox chkMergeReport;
        private System.Windows.Forms.CheckBox chkShowPathsInKDiff3;
        private System.Windows.Forms.CheckBox chkCompletionSounds;
        private System.Windows.Forms.CheckBox chkReviewEachMerge;
        private System.Windows.Forms.GroupBox grpAutocollapse;
        private System.Windows.Forms.CheckBox chkCollapseCustomLoadOrder;
        private System.Windows.Forms.CheckBox chkCollapseNotMergeable;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
    }
}