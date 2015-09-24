namespace WitcherScriptMerger.Forms
{
    partial class PackReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackReportForm));
            this.txtBundlePath = new System.Windows.Forms.TextBox();
            this.btnOpenContentDir = new System.Windows.Forms.Button();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.btnOpenBundleDir = new System.Windows.Forms.Button();
            this.lblPackedBundle = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkShowAfterPack = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBundlePath
            // 
            this.txtBundlePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBundlePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBundlePath.Location = new System.Drawing.Point(12, 77);
            this.txtBundlePath.Name = "txtBundlePath";
            this.txtBundlePath.ReadOnly = true;
            this.txtBundlePath.Size = new System.Drawing.Size(538, 20);
            this.txtBundlePath.TabIndex = 1;
            this.txtBundlePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOpenContentDir
            // 
            this.btnOpenContentDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenContentDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenContentDir.Location = new System.Drawing.Point(458, 114);
            this.btnOpenContentDir.Name = "btnOpenContentDir";
            this.btnOpenContentDir.Size = new System.Drawing.Size(92, 21);
            this.btnOpenContentDir.TabIndex = 2;
            this.btnOpenContentDir.Text = "Open Directory";
            this.btnOpenContentDir.UseVisualStyleBackColor = true;
            this.btnOpenContentDir.Click += new System.EventHandler(this.btnOpenContentDir_Click);
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContent.Location = new System.Drawing.Point(12, 118);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(51, 13);
            this.lblContent.TabIndex = 12;
            this.lblContent.Text = "Content";
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.Location = new System.Drawing.Point(12, 139);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(538, 190);
            this.txtContent.TabIndex = 3;
            this.txtContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnOpenBundleDir
            // 
            this.btnOpenBundleDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenBundleDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenBundleDir.Location = new System.Drawing.Point(458, 52);
            this.btnOpenBundleDir.Name = "btnOpenBundleDir";
            this.btnOpenBundleDir.Size = new System.Drawing.Size(92, 21);
            this.btnOpenBundleDir.TabIndex = 0;
            this.btnOpenBundleDir.Text = "Open Directory";
            this.btnOpenBundleDir.UseVisualStyleBackColor = true;
            this.btnOpenBundleDir.Click += new System.EventHandler(this.btnOpenBundleDir_Click);
            // 
            // lblPackedBundle
            // 
            this.lblPackedBundle.AutoSize = true;
            this.lblPackedBundle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackedBundle.Location = new System.Drawing.Point(12, 11);
            this.lblPackedBundle.Name = "lblPackedBundle";
            this.lblPackedBundle.Size = new System.Drawing.Size(175, 20);
            this.lblPackedBundle.TabIndex = 8;
            this.lblPackedBundle.Text = "Packed new bundle file.";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(443, 335);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkShowAfterPack
            // 
            this.chkShowAfterPack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowAfterPack.AutoSize = true;
            this.chkShowAfterPack.Location = new System.Drawing.Point(16, 337);
            this.chkShowAfterPack.Name = "chkShowAfterPack";
            this.chkShowAfterPack.Size = new System.Drawing.Size(202, 17);
            this.chkShowAfterPack.TabIndex = 4;
            this.chkShowAfterPack.Text = "&Show this report after packing bundle";
            this.chkShowAfterPack.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Merged Bundle File";
            // 
            // PackReportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(562, 368);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBundlePath);
            this.Controls.Add(this.btnOpenContentDir);
            this.Controls.Add(this.btnOpenBundleDir);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.chkShowAfterPack);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPackedBundle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(360, 290);
            this.Name = "PackReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pack Finished";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PackReportForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBundlePath;
        private System.Windows.Forms.Button btnOpenBundleDir;
        private System.Windows.Forms.Label lblPackedBundle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkShowAfterPack;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Button btnOpenContentDir;
        private System.Windows.Forms.Label label1;
    }
}