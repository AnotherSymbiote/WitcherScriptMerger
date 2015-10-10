namespace WitcherScriptMerger.Forms
{
    partial class DependencyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DependencyForm));
            this.grpKDiff3 = new System.Windows.Forms.GroupBox();
            this.lblKDiff3Msg = new System.Windows.Forms.Label();
            this.lblKDiff3Path = new System.Windows.Forms.Label();
            this.btnKDiff3Path = new System.Windows.Forms.Button();
            this.txtKDiff3Path = new System.Windows.Forms.TextBox();
            this.lnkKDiff3 = new System.Windows.Forms.LinkLabel();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.grpBms = new System.Windows.Forms.GroupBox();
            this.lblBmsMsg = new System.Windows.Forms.Label();
            this.lblBmsPath = new System.Windows.Forms.Label();
            this.btnBmsPath = new System.Windows.Forms.Button();
            this.txtBmsPath = new System.Windows.Forms.TextBox();
            this.lnkBmsPlugin = new System.Windows.Forms.LinkLabel();
            this.grpBmsPlugin = new System.Windows.Forms.GroupBox();
            this.lblBmsPluginMsg = new System.Windows.Forms.Label();
            this.lblBmsPluginPath = new System.Windows.Forms.Label();
            this.btnBmsPluginPath = new System.Windows.Forms.Button();
            this.txtBmsPluginPath = new System.Windows.Forms.TextBox();
            this.lnkWccLite = new System.Windows.Forms.LinkLabel();
            this.grpWccLite = new System.Windows.Forms.GroupBox();
            this.lblWccLiteMsg = new System.Windows.Forms.Label();
            this.lblWccLitePath = new System.Windows.Forms.Label();
            this.btnWccLitePath = new System.Windows.Forms.Button();
            this.txtWccLitePath = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lnkBms = new System.Windows.Forms.LinkLabel();
            this.grpKDiff3.SuspendLayout();
            this.grpBms.SuspendLayout();
            this.grpBmsPlugin.SuspendLayout();
            this.grpWccLite.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpKDiff3
            // 
            this.grpKDiff3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpKDiff3.Controls.Add(this.lblKDiff3Msg);
            this.grpKDiff3.Controls.Add(this.lblKDiff3Path);
            this.grpKDiff3.Controls.Add(this.btnKDiff3Path);
            this.grpKDiff3.Controls.Add(this.txtKDiff3Path);
            this.grpKDiff3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpKDiff3.Location = new System.Drawing.Point(9, 60);
            this.grpKDiff3.Name = "grpKDiff3";
            this.grpKDiff3.Size = new System.Drawing.Size(445, 80);
            this.grpKDiff3.TabIndex = 1;
            this.grpKDiff3.TabStop = false;
            this.grpKDiff3.Text = "KDiff3.exe";
            // 
            // lblKDiff3Msg
            // 
            this.lblKDiff3Msg.AutoSize = true;
            this.lblKDiff3Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKDiff3Msg.Location = new System.Drawing.Point(7, 52);
            this.lblKDiff3Msg.Name = "lblKDiff3Msg";
            this.lblKDiff3Msg.Size = new System.Drawing.Size(360, 13);
            this.lblKDiff3Msg.TabIndex = 4;
            this.lblKDiff3Msg.Text = "Script Merger uses this open-source tool by Joachim Eibl to merge text files.";
            // 
            // lblKDiff3Path
            // 
            this.lblKDiff3Path.AutoSize = true;
            this.lblKDiff3Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKDiff3Path.Location = new System.Drawing.Point(7, 27);
            this.lblKDiff3Path.Name = "lblKDiff3Path";
            this.lblKDiff3Path.Size = new System.Drawing.Size(32, 13);
            this.lblKDiff3Path.TabIndex = 3;
            this.lblKDiff3Path.Text = "Path:";
            // 
            // btnKDiff3Path
            // 
            this.btnKDiff3Path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKDiff3Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKDiff3Path.Location = new System.Drawing.Point(413, 22);
            this.btnKDiff3Path.Name = "btnKDiff3Path";
            this.btnKDiff3Path.Size = new System.Drawing.Size(26, 23);
            this.btnKDiff3Path.TabIndex = 1;
            this.btnKDiff3Path.Text = "...";
            this.btnKDiff3Path.UseVisualStyleBackColor = true;
            this.btnKDiff3Path.Click += new System.EventHandler(this.btnKDiff3Path_Click);
            // 
            // txtKDiff3Path
            // 
            this.txtKDiff3Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKDiff3Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKDiff3Path.Location = new System.Drawing.Point(45, 24);
            this.txtKDiff3Path.Name = "txtKDiff3Path";
            this.txtKDiff3Path.Size = new System.Drawing.Size(362, 20);
            this.txtKDiff3Path.TabIndex = 0;
            // 
            // lnkKDiff3
            // 
            this.lnkKDiff3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkKDiff3.AutoSize = true;
            this.lnkKDiff3.LinkArea = new System.Windows.Forms.LinkArea(14, 53);
            this.lnkKDiff3.Location = new System.Drawing.Point(311, 60);
            this.lnkKDiff3.Name = "lnkKDiff3";
            this.lnkKDiff3.Size = new System.Drawing.Size(137, 17);
            this.lnkKDiff3.TabIndex = 0;
            this.lnkKDiff3.TabStop = true;
            this.lnkKDiff3.Text = "Download from KDiff3 Site";
            this.lnkKDiff3.UseCompatibleTextRendering = true;
            this.lnkKDiff3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkKDiff3_LinkClicked);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrompt.Location = new System.Drawing.Point(12, 9);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(317, 34);
            this.lblPrompt.TabIndex = 5;
            this.lblPrompt.Text = "Please locate all the required components below.\r\n(Download any that you don\'t ha" +
    "ve yet.)";
            // 
            // grpBms
            // 
            this.grpBms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBms.Controls.Add(this.lblBmsMsg);
            this.grpBms.Controls.Add(this.lblBmsPath);
            this.grpBms.Controls.Add(this.btnBmsPath);
            this.grpBms.Controls.Add(this.txtBmsPath);
            this.grpBms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBms.Location = new System.Drawing.Point(9, 156);
            this.grpBms.Name = "grpBms";
            this.grpBms.Size = new System.Drawing.Size(445, 80);
            this.grpBms.TabIndex = 4;
            this.grpBms.TabStop = false;
            this.grpBms.Text = "QuickBMS.exe";
            // 
            // lblBmsMsg
            // 
            this.lblBmsMsg.AutoSize = true;
            this.lblBmsMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBmsMsg.Location = new System.Drawing.Point(7, 52);
            this.lblBmsMsg.Name = "lblBmsMsg";
            this.lblBmsMsg.Size = new System.Drawing.Size(432, 13);
            this.lblBmsMsg.TabIndex = 4;
            this.lblBmsMsg.Text = "Script Merger uses this open-source tool by Luigi Auriemma to scan && unpack .bun" +
    "dle files.";
            // 
            // lblBmsPath
            // 
            this.lblBmsPath.AutoSize = true;
            this.lblBmsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBmsPath.Location = new System.Drawing.Point(7, 27);
            this.lblBmsPath.Name = "lblBmsPath";
            this.lblBmsPath.Size = new System.Drawing.Size(32, 13);
            this.lblBmsPath.TabIndex = 3;
            this.lblBmsPath.Text = "Path:";
            // 
            // btnBmsPath
            // 
            this.btnBmsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBmsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBmsPath.Location = new System.Drawing.Point(413, 22);
            this.btnBmsPath.Name = "btnBmsPath";
            this.btnBmsPath.Size = new System.Drawing.Size(26, 23);
            this.btnBmsPath.TabIndex = 1;
            this.btnBmsPath.Text = "...";
            this.btnBmsPath.UseVisualStyleBackColor = true;
            this.btnBmsPath.Click += new System.EventHandler(this.btnBmsPath_Click);
            // 
            // txtBmsPath
            // 
            this.txtBmsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBmsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBmsPath.Location = new System.Drawing.Point(45, 24);
            this.txtBmsPath.Name = "txtBmsPath";
            this.txtBmsPath.Size = new System.Drawing.Size(362, 20);
            this.txtBmsPath.TabIndex = 0;
            // 
            // lnkBmsPlugin
            // 
            this.lnkBmsPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkBmsPlugin.AutoSize = true;
            this.lnkBmsPlugin.LinkArea = new System.Windows.Forms.LinkArea(14, 53);
            this.lnkBmsPlugin.Location = new System.Drawing.Point(292, 252);
            this.lnkBmsPlugin.Name = "lnkBmsPlugin";
            this.lnkBmsPlugin.Size = new System.Drawing.Size(159, 17);
            this.lnkBmsPlugin.TabIndex = 5;
            this.lnkBmsPlugin.TabStop = true;
            this.lnkBmsPlugin.Text = "Download from QuickBMS Site";
            this.lnkBmsPlugin.UseCompatibleTextRendering = true;
            this.lnkBmsPlugin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBms_LinkClicked);
            // 
            // grpBmsPlugin
            // 
            this.grpBmsPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBmsPlugin.Controls.Add(this.lblBmsPluginMsg);
            this.grpBmsPlugin.Controls.Add(this.lblBmsPluginPath);
            this.grpBmsPlugin.Controls.Add(this.btnBmsPluginPath);
            this.grpBmsPlugin.Controls.Add(this.txtBmsPluginPath);
            this.grpBmsPlugin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBmsPlugin.Location = new System.Drawing.Point(12, 252);
            this.grpBmsPlugin.Name = "grpBmsPlugin";
            this.grpBmsPlugin.Size = new System.Drawing.Size(445, 80);
            this.grpBmsPlugin.TabIndex = 6;
            this.grpBmsPlugin.TabStop = false;
            this.grpBmsPlugin.Text = "witcher3.bms Plugin";
            // 
            // lblBmsPluginMsg
            // 
            this.lblBmsPluginMsg.AutoSize = true;
            this.lblBmsPluginMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBmsPluginMsg.Location = new System.Drawing.Point(7, 52);
            this.lblBmsPluginMsg.Name = "lblBmsPluginMsg";
            this.lblBmsPluginMsg.Size = new System.Drawing.Size(360, 13);
            this.lblBmsPluginMsg.TabIndex = 4;
            this.lblBmsPluginMsg.Text = "QuickBMS needs this plugin to handle The Witcher 3 .bundle files properly.";
            // 
            // lblBmsPluginPath
            // 
            this.lblBmsPluginPath.AutoSize = true;
            this.lblBmsPluginPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBmsPluginPath.Location = new System.Drawing.Point(7, 27);
            this.lblBmsPluginPath.Name = "lblBmsPluginPath";
            this.lblBmsPluginPath.Size = new System.Drawing.Size(32, 13);
            this.lblBmsPluginPath.TabIndex = 3;
            this.lblBmsPluginPath.Text = "Path:";
            // 
            // btnBmsPluginPath
            // 
            this.btnBmsPluginPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBmsPluginPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBmsPluginPath.Location = new System.Drawing.Point(413, 22);
            this.btnBmsPluginPath.Name = "btnBmsPluginPath";
            this.btnBmsPluginPath.Size = new System.Drawing.Size(26, 23);
            this.btnBmsPluginPath.TabIndex = 1;
            this.btnBmsPluginPath.Text = "...";
            this.btnBmsPluginPath.UseVisualStyleBackColor = true;
            this.btnBmsPluginPath.Click += new System.EventHandler(this.btnBmsPluginPath_Click);
            // 
            // txtBmsPluginPath
            // 
            this.txtBmsPluginPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBmsPluginPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBmsPluginPath.Location = new System.Drawing.Point(45, 24);
            this.txtBmsPluginPath.Name = "txtBmsPluginPath";
            this.txtBmsPluginPath.Size = new System.Drawing.Size(362, 20);
            this.txtBmsPluginPath.TabIndex = 0;
            // 
            // lnkWccLite
            // 
            this.lnkWccLite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWccLite.AutoSize = true;
            this.lnkWccLite.LinkArea = new System.Windows.Forms.LinkArea(14, 53);
            this.lnkWccLite.Location = new System.Drawing.Point(305, 348);
            this.lnkWccLite.Name = "lnkWccLite";
            this.lnkWccLite.Size = new System.Drawing.Size(146, 17);
            this.lnkWccLite.TabIndex = 7;
            this.lnkWccLite.TabStop = true;
            this.lnkWccLite.Text = "Download from Nexus Mods";
            this.lnkWccLite.UseCompatibleTextRendering = true;
            this.lnkWccLite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWccLite_LinkClicked);
            // 
            // grpWccLite
            // 
            this.grpWccLite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpWccLite.Controls.Add(this.lblWccLiteMsg);
            this.grpWccLite.Controls.Add(this.lblWccLitePath);
            this.grpWccLite.Controls.Add(this.btnWccLitePath);
            this.grpWccLite.Controls.Add(this.txtWccLitePath);
            this.grpWccLite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWccLite.Location = new System.Drawing.Point(12, 348);
            this.grpWccLite.Name = "grpWccLite";
            this.grpWccLite.Size = new System.Drawing.Size(445, 80);
            this.grpWccLite.TabIndex = 8;
            this.grpWccLite.TabStop = false;
            this.grpWccLite.Text = "wcc_lite.exe";
            // 
            // lblWccLiteMsg
            // 
            this.lblWccLiteMsg.AutoSize = true;
            this.lblWccLiteMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWccLiteMsg.Location = new System.Drawing.Point(7, 52);
            this.lblWccLiteMsg.Name = "lblWccLiteMsg";
            this.lblWccLiteMsg.Size = new System.Drawing.Size(405, 13);
            this.lblWccLiteMsg.TabIndex = 4;
            this.lblWccLiteMsg.Text = "Script Merger uses this official CDPR tool to pack merged XML files into .bundle " +
    "files.";
            // 
            // lblWccLitePath
            // 
            this.lblWccLitePath.AutoSize = true;
            this.lblWccLitePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWccLitePath.Location = new System.Drawing.Point(7, 27);
            this.lblWccLitePath.Name = "lblWccLitePath";
            this.lblWccLitePath.Size = new System.Drawing.Size(32, 13);
            this.lblWccLitePath.TabIndex = 3;
            this.lblWccLitePath.Text = "Path:";
            // 
            // btnWccLitePath
            // 
            this.btnWccLitePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWccLitePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWccLitePath.Location = new System.Drawing.Point(413, 22);
            this.btnWccLitePath.Name = "btnWccLitePath";
            this.btnWccLitePath.Size = new System.Drawing.Size(26, 23);
            this.btnWccLitePath.TabIndex = 1;
            this.btnWccLitePath.Text = "...";
            this.btnWccLitePath.UseVisualStyleBackColor = true;
            this.btnWccLitePath.Click += new System.EventHandler(this.btnWccLitePath_Click);
            // 
            // txtWccLitePath
            // 
            this.txtWccLitePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWccLitePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWccLitePath.Location = new System.Drawing.Point(45, 24);
            this.txtWccLitePath.Name = "txtWccLitePath";
            this.txtWccLitePath.Size = new System.Drawing.Size(362, 20);
            this.txtWccLitePath.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(251, 442);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(357, 442);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lnkBms
            // 
            this.lnkBms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkBms.AutoSize = true;
            this.lnkBms.LinkArea = new System.Windows.Forms.LinkArea(14, 53);
            this.lnkBms.Location = new System.Drawing.Point(289, 156);
            this.lnkBms.Name = "lnkBms";
            this.lnkBms.Size = new System.Drawing.Size(159, 17);
            this.lnkBms.TabIndex = 2;
            this.lnkBms.TabStop = true;
            this.lnkBms.Text = "Download from QuickBMS Site";
            this.lnkBms.UseCompatibleTextRendering = true;
            this.lnkBms.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBms_LinkClicked);
            // 
            // DependencyForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(469, 477);
            this.ControlBox = false;
            this.Controls.Add(this.lnkBms);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lnkWccLite);
            this.Controls.Add(this.grpWccLite);
            this.Controls.Add(this.lnkBmsPlugin);
            this.Controls.Add(this.grpBmsPlugin);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.grpBms);
            this.Controls.Add(this.lnkKDiff3);
            this.Controls.Add(this.grpKDiff3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 515);
            this.MinimumSize = new System.Drawing.Size(485, 515);
            this.Name = "DependencyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dependency Locations";
            this.Load += new System.EventHandler(this.DependencyForm_Load);
            this.grpKDiff3.ResumeLayout(false);
            this.grpKDiff3.PerformLayout();
            this.grpBms.ResumeLayout(false);
            this.grpBms.PerformLayout();
            this.grpBmsPlugin.ResumeLayout(false);
            this.grpBmsPlugin.PerformLayout();
            this.grpWccLite.ResumeLayout(false);
            this.grpWccLite.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox grpKDiff3;
        private System.Windows.Forms.TextBox txtKDiff3Path;
        private System.Windows.Forms.Label lblKDiff3Msg;
        private System.Windows.Forms.Label lblKDiff3Path;
        private System.Windows.Forms.Button btnKDiff3Path;
        private System.Windows.Forms.LinkLabel lnkKDiff3;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.GroupBox grpBms;
        private System.Windows.Forms.Label lblBmsMsg;
        private System.Windows.Forms.Label lblBmsPath;
        private System.Windows.Forms.Button btnBmsPath;
        private System.Windows.Forms.TextBox txtBmsPath;
        private System.Windows.Forms.LinkLabel lnkBmsPlugin;
        private System.Windows.Forms.GroupBox grpBmsPlugin;
        private System.Windows.Forms.Label lblBmsPluginMsg;
        private System.Windows.Forms.Label lblBmsPluginPath;
        private System.Windows.Forms.Button btnBmsPluginPath;
        private System.Windows.Forms.TextBox txtBmsPluginPath;
        private System.Windows.Forms.LinkLabel lnkWccLite;
        private System.Windows.Forms.GroupBox grpWccLite;
        private System.Windows.Forms.Label lblWccLiteMsg;
        private System.Windows.Forms.Label lblWccLitePath;
        private System.Windows.Forms.Button btnWccLitePath;
        private System.Windows.Forms.TextBox txtWccLitePath;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel lnkBms;
    }
}