namespace WitcherScriptMerger.Forms
{
    partial class ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.txtFilePath1 = new System.Windows.Forms.TextBox();
            this.btnOpenFile1 = new System.Windows.Forms.Button();
            this.grpFile1 = new System.Windows.Forms.GroupBox();
            this.btnOpenDir1 = new System.Windows.Forms.Button();
            this.lblMergedFiles = new System.Windows.Forms.Label();
            this.grpOutputFile = new System.Windows.Forms.GroupBox();
            this.btnOpenOutputDir = new System.Windows.Forms.Button();
            this.btnOpenOutputFile = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.grpFile2 = new System.Windows.Forms.GroupBox();
            this.btnOpenDir2 = new System.Windows.Forms.Button();
            this.btnOpenFile2 = new System.Windows.Forms.Button();
            this.txtFilePath2 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpFile1.SuspendLayout();
            this.grpOutputFile.SuspendLayout();
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
            this.txtFilePath1.Size = new System.Drawing.Size(633, 20);
            this.txtFilePath1.TabIndex = 2;
            this.txtFilePath1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOpenFile1
            // 
            this.btnOpenFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile1.Location = new System.Drawing.Point(7, 45);
            this.btnOpenFile1.Name = "btnOpenFile1";
            this.btnOpenFile1.Size = new System.Drawing.Size(313, 23);
            this.btnOpenFile1.TabIndex = 3;
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
            this.grpFile1.Size = new System.Drawing.Size(645, 79);
            this.grpFile1.TabIndex = 4;
            this.grpFile1.TabStop = false;
            this.grpFile1.Text = "Mod 1";
            // 
            // btnOpenDir1
            // 
            this.btnOpenDir1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDir1.Location = new System.Drawing.Point(326, 45);
            this.btnOpenDir1.Name = "btnOpenDir1";
            this.btnOpenDir1.Size = new System.Drawing.Size(313, 23);
            this.btnOpenDir1.TabIndex = 4;
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
            this.lblMergedFiles.Size = new System.Drawing.Size(303, 20);
            this.lblMergedFiles.TabIndex = 8;
            this.lblMergedFiles.Text = "Merged files && moved to backup directory!";
            // 
            // grpOutputFile
            // 
            this.grpOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOutputFile.Controls.Add(this.btnOpenOutputDir);
            this.grpOutputFile.Controls.Add(this.btnOpenOutputFile);
            this.grpOutputFile.Controls.Add(this.txtOutputPath);
            this.grpOutputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpOutputFile.Location = new System.Drawing.Point(12, 234);
            this.grpOutputFile.Name = "grpOutputFile";
            this.grpOutputFile.Size = new System.Drawing.Size(645, 77);
            this.grpOutputFile.TabIndex = 5;
            this.grpOutputFile.TabStop = false;
            this.grpOutputFile.Text = "Output File";
            // 
            // btnOpenOutputDir
            // 
            this.btnOpenOutputDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOutputDir.Location = new System.Drawing.Point(326, 45);
            this.btnOpenOutputDir.Name = "btnOpenOutputDir";
            this.btnOpenOutputDir.Size = new System.Drawing.Size(313, 23);
            this.btnOpenOutputDir.TabIndex = 4;
            this.btnOpenOutputDir.Text = "Open Directory";
            this.btnOpenOutputDir.UseVisualStyleBackColor = true;
            this.btnOpenOutputDir.Click += new System.EventHandler(this.btnOpenOutputDir_Click);
            // 
            // btnOpenOutputFile
            // 
            this.btnOpenOutputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOutputFile.Location = new System.Drawing.Point(7, 45);
            this.btnOpenOutputFile.Name = "btnOpenOutputFile";
            this.btnOpenOutputFile.Size = new System.Drawing.Size(313, 23);
            this.btnOpenOutputFile.TabIndex = 3;
            this.btnOpenOutputFile.Text = "Open File";
            this.btnOpenOutputFile.UseVisualStyleBackColor = true;
            this.btnOpenOutputFile.Click += new System.EventHandler(this.btnOpenOutputFile_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputPath.Location = new System.Drawing.Point(6, 19);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(633, 20);
            this.txtOutputPath.TabIndex = 2;
            this.txtOutputPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
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
            this.grpFile2.Size = new System.Drawing.Size(645, 79);
            this.grpFile2.TabIndex = 6;
            this.grpFile2.TabStop = false;
            this.grpFile2.Text = "Mod 2";
            // 
            // btnOpenDir2
            // 
            this.btnOpenDir2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDir2.Location = new System.Drawing.Point(326, 45);
            this.btnOpenDir2.Name = "btnOpenDir2";
            this.btnOpenDir2.Size = new System.Drawing.Size(313, 23);
            this.btnOpenDir2.TabIndex = 4;
            this.btnOpenDir2.Text = "Open Directory";
            this.btnOpenDir2.UseVisualStyleBackColor = true;
            this.btnOpenDir2.Click += new System.EventHandler(this.btnOpenDir2_Click);
            // 
            // btnOpenFile2
            // 
            this.btnOpenFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile2.Location = new System.Drawing.Point(7, 45);
            this.btnOpenFile2.Name = "btnOpenFile2";
            this.btnOpenFile2.Size = new System.Drawing.Size(313, 23);
            this.btnOpenFile2.TabIndex = 3;
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
            this.txtFilePath2.Size = new System.Drawing.Size(633, 20);
            this.txtFilePath2.TabIndex = 2;
            this.txtFilePath2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(550, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ReportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(669, 362);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpFile2);
            this.Controls.Add(this.grpOutputFile);
            this.Controls.Add(this.lblMergedFiles);
            this.Controls.Add(this.grpFile1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 400);
            this.MinimumSize = new System.Drawing.Size(685, 400);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Merge Finished";
            this.grpFile1.ResumeLayout(false);
            this.grpFile1.PerformLayout();
            this.grpOutputFile.ResumeLayout(false);
            this.grpOutputFile.PerformLayout();
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
        private System.Windows.Forms.GroupBox grpOutputFile;
        private System.Windows.Forms.Button btnOpenOutputDir;
        private System.Windows.Forms.Button btnOpenOutputFile;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.GroupBox grpFile2;
        private System.Windows.Forms.Button btnOpenDir2;
        private System.Windows.Forms.Button btnOpenFile2;
        private System.Windows.Forms.TextBox txtFilePath2;
        private System.Windows.Forms.Button btnOK;
    }
}