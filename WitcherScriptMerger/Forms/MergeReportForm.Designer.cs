namespace WitcherScriptMerger.Forms
{
    partial class MergeReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeReportForm));
            this.txtFilePath1 = new System.Windows.Forms.TextBox();
            this.btnOpenFile1 = new System.Windows.Forms.Button();
            this.grpFile1 = new System.Windows.Forms.GroupBox();
            this.btnOpenDir1 = new System.Windows.Forms.Button();
            this.lblMergedFiles = new System.Windows.Forms.Label();
            this.grpMergedFile = new System.Windows.Forms.GroupBox();
            this.btnOpenMergedDir = new System.Windows.Forms.Button();
            this.btnOpenMergedFile = new System.Windows.Forms.Button();
            this.txtMergedPath = new System.Windows.Forms.TextBox();
            this.grpFile2 = new System.Windows.Forms.GroupBox();
            this.btnOpenDir2 = new System.Windows.Forms.Button();
            this.btnOpenFile2 = new System.Windows.Forms.Button();
            this.txtFilePath2 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkShowAfterMerge = new System.Windows.Forms.CheckBox();
            this.lblTempContentFiles = new System.Windows.Forms.Label();
            this.grpFile1.SuspendLayout();
            this.grpMergedFile.SuspendLayout();
            this.grpFile2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilePath1
            // 
            this.txtFilePath1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath1.Location = new System.Drawing.Point(6, 19);
            this.txtFilePath1.Name = "txtFilePath1";
            this.txtFilePath1.ReadOnly = true;
            this.txtFilePath1.Size = new System.Drawing.Size(588, 20);
            this.txtFilePath1.TabIndex = 0;
            this.txtFilePath1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOpenFile1
            // 
            this.btnOpenFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile1.Location = new System.Drawing.Point(7, 45);
            this.btnOpenFile1.Name = "btnOpenFile1";
            this.btnOpenFile1.Size = new System.Drawing.Size(290, 23);
            this.btnOpenFile1.TabIndex = 1;
            this.btnOpenFile1.Text = "Open File";
            this.btnOpenFile1.UseVisualStyleBackColor = true;
            this.btnOpenFile1.Click += new System.EventHandler(this.btnOpenFile1_Click);
            // 
            // grpFile1
            // 
            this.grpFile1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFile1.Controls.Add(this.btnOpenDir1);
            this.grpFile1.Controls.Add(this.btnOpenFile1);
            this.grpFile1.Controls.Add(this.txtFilePath1);
            this.grpFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFile1.Location = new System.Drawing.Point(12, 54);
            this.grpFile1.Name = "grpFile1";
            this.grpFile1.Size = new System.Drawing.Size(600, 79);
            this.grpFile1.TabIndex = 0;
            this.grpFile1.TabStop = false;
            this.grpFile1.Text = "Mod 1";
            // 
            // btnOpenDir1
            // 
            this.btnOpenDir1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDir1.Location = new System.Drawing.Point(303, 45);
            this.btnOpenDir1.Name = "btnOpenDir1";
            this.btnOpenDir1.Size = new System.Drawing.Size(291, 23);
            this.btnOpenDir1.TabIndex = 2;
            this.btnOpenDir1.Text = "Open Directory";
            this.btnOpenDir1.UseVisualStyleBackColor = true;
            this.btnOpenDir1.Click += new System.EventHandler(this.btnOpenDir1_Click);
            // 
            // lblMergedFiles
            // 
            this.lblMergedFiles.AutoSize = true;
            this.lblMergedFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMergedFiles.Location = new System.Drawing.Point(12, 11);
            this.lblMergedFiles.Name = "lblMergedFiles";
            this.lblMergedFiles.Size = new System.Drawing.Size(185, 20);
            this.lblMergedFiles.TabIndex = 5;
            this.lblMergedFiles.Text = "Created new merged file!";
            // 
            // grpMergedFile
            // 
            this.grpMergedFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMergedFile.Controls.Add(this.btnOpenMergedDir);
            this.grpMergedFile.Controls.Add(this.btnOpenMergedFile);
            this.grpMergedFile.Controls.Add(this.txtMergedPath);
            this.grpMergedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMergedFile.Location = new System.Drawing.Point(12, 234);
            this.grpMergedFile.Name = "grpMergedFile";
            this.grpMergedFile.Size = new System.Drawing.Size(600, 77);
            this.grpMergedFile.TabIndex = 2;
            this.grpMergedFile.TabStop = false;
            this.grpMergedFile.Text = "Merged File";
            // 
            // btnOpenMergedDir
            // 
            this.btnOpenMergedDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenMergedDir.Location = new System.Drawing.Point(303, 45);
            this.btnOpenMergedDir.Name = "btnOpenMergedDir";
            this.btnOpenMergedDir.Size = new System.Drawing.Size(291, 23);
            this.btnOpenMergedDir.TabIndex = 2;
            this.btnOpenMergedDir.Text = "Open Directory";
            this.btnOpenMergedDir.UseVisualStyleBackColor = true;
            this.btnOpenMergedDir.Click += new System.EventHandler(this.btnOpenOutputDir_Click);
            // 
            // btnOpenMergedFile
            // 
            this.btnOpenMergedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenMergedFile.Location = new System.Drawing.Point(7, 45);
            this.btnOpenMergedFile.Name = "btnOpenMergedFile";
            this.btnOpenMergedFile.Size = new System.Drawing.Size(290, 23);
            this.btnOpenMergedFile.TabIndex = 1;
            this.btnOpenMergedFile.Text = "Open File";
            this.btnOpenMergedFile.UseVisualStyleBackColor = true;
            this.btnOpenMergedFile.Click += new System.EventHandler(this.btnOpenOutputFile_Click);
            // 
            // txtMergedPath
            // 
            this.txtMergedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMergedPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMergedPath.Location = new System.Drawing.Point(6, 19);
            this.txtMergedPath.Name = "txtMergedPath";
            this.txtMergedPath.ReadOnly = true;
            this.txtMergedPath.Size = new System.Drawing.Size(588, 20);
            this.txtMergedPath.TabIndex = 0;
            this.txtMergedPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // grpFile2
            // 
            this.grpFile2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFile2.Controls.Add(this.btnOpenDir2);
            this.grpFile2.Controls.Add(this.btnOpenFile2);
            this.grpFile2.Controls.Add(this.txtFilePath2);
            this.grpFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFile2.Location = new System.Drawing.Point(12, 144);
            this.grpFile2.Name = "grpFile2";
            this.grpFile2.Size = new System.Drawing.Size(600, 79);
            this.grpFile2.TabIndex = 1;
            this.grpFile2.TabStop = false;
            this.grpFile2.Text = "Mod 2";
            // 
            // btnOpenDir2
            // 
            this.btnOpenDir2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDir2.Location = new System.Drawing.Point(303, 45);
            this.btnOpenDir2.Name = "btnOpenDir2";
            this.btnOpenDir2.Size = new System.Drawing.Size(291, 23);
            this.btnOpenDir2.TabIndex = 2;
            this.btnOpenDir2.Text = "Open Directory";
            this.btnOpenDir2.UseVisualStyleBackColor = true;
            this.btnOpenDir2.Click += new System.EventHandler(this.btnOpenDir2_Click);
            // 
            // btnOpenFile2
            // 
            this.btnOpenFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile2.Location = new System.Drawing.Point(7, 45);
            this.btnOpenFile2.Name = "btnOpenFile2";
            this.btnOpenFile2.Size = new System.Drawing.Size(290, 23);
            this.btnOpenFile2.TabIndex = 1;
            this.btnOpenFile2.Text = "Open File";
            this.btnOpenFile2.UseVisualStyleBackColor = true;
            this.btnOpenFile2.Click += new System.EventHandler(this.btnOpenFile2_Click);
            // 
            // txtFilePath2
            // 
            this.txtFilePath2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath2.Location = new System.Drawing.Point(6, 19);
            this.txtFilePath2.Name = "txtFilePath2";
            this.txtFilePath2.ReadOnly = true;
            this.txtFilePath2.Size = new System.Drawing.Size(588, 20);
            this.txtFilePath2.TabIndex = 0;
            this.txtFilePath2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(505, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkShowAfterMerge
            // 
            this.chkShowAfterMerge.AutoSize = true;
            this.chkShowAfterMerge.Location = new System.Drawing.Point(16, 331);
            this.chkShowAfterMerge.Name = "chkShowAfterMerge";
            this.chkShowAfterMerge.Size = new System.Drawing.Size(185, 17);
            this.chkShowAfterMerge.TabIndex = 3;
            this.chkShowAfterMerge.Text = "&Show this report after each merge";
            this.chkShowAfterMerge.UseVisualStyleBackColor = true;
            // 
            // lblTempContentFiles
            // 
            this.lblTempContentFiles.AutoSize = true;
            this.lblTempContentFiles.Location = new System.Drawing.Point(233, 9);
            this.lblTempContentFiles.Name = "lblTempContentFiles";
            this.lblTempContentFiles.Size = new System.Drawing.Size(377, 26);
            this.lblTempContentFiles.TabIndex = 6;
            this.lblTempContentFiles.Text = "Note: The first 2 files listed below were temporarily unpacked from .bundle files" +
    ".\r\nThey will be deleted when all merges are finished.";
            // 
            // MergeReportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(624, 362);
            this.ControlBox = false;
            this.Controls.Add(this.lblTempContentFiles);
            this.Controls.Add(this.chkShowAfterMerge);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpFile2);
            this.Controls.Add(this.grpMergedFile);
            this.Controls.Add(this.lblMergedFiles);
            this.Controls.Add(this.grpFile1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 400);
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "MergeReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Merge Finished";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MergeReportForm_FormClosing);
            this.grpFile1.ResumeLayout(false);
            this.grpFile1.PerformLayout();
            this.grpMergedFile.ResumeLayout(false);
            this.grpMergedFile.PerformLayout();
            this.grpFile2.ResumeLayout(false);
            this.grpFile2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath1;
        private System.Windows.Forms.Button btnOpenFile1;
        private System.Windows.Forms.GroupBox grpFile1;
        private System.Windows.Forms.Button btnOpenDir1;
        private System.Windows.Forms.Label lblMergedFiles;
        private System.Windows.Forms.GroupBox grpMergedFile;
        private System.Windows.Forms.Button btnOpenMergedDir;
        private System.Windows.Forms.Button btnOpenMergedFile;
        private System.Windows.Forms.TextBox txtMergedPath;
        private System.Windows.Forms.GroupBox grpFile2;
        private System.Windows.Forms.Button btnOpenDir2;
        private System.Windows.Forms.Button btnOpenFile2;
        private System.Windows.Forms.TextBox txtFilePath2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkShowAfterMerge;
        private System.Windows.Forms.Label lblTempContentFiles;
    }
}