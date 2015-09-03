namespace WitcherScriptMerger
{
    partial class ResolveConflictForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResolveConflictForm));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.chkLineBreakSymbol = new System.Windows.Forms.CheckBox();
            this.lblRightInfo = new System.Windows.Forms.Label();
            this.lblLeftInfo = new System.Windows.Forms.Label();
            this.btnUseVanilla = new System.Windows.Forms.Button();
            this.btnUseLeft = new System.Windows.Forms.Button();
            this.btnUseRight = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblVanillaInfo = new System.Windows.Forms.Label();
            this.rtbVanilla = new WitcherScriptMerger.SyncedRichTextBox();
            this.rtbRight = new WitcherScriptMerger.SyncedRichTextBox();
            this.rtbLeft = new WitcherScriptMerger.SyncedRichTextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanel.Controls.Add(this.chkLineBreakSymbol, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.lblRightInfo, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.lblLeftInfo, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.rtbVanilla, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.rtbRight, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.rtbLeft, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.btnUseVanilla, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnUseLeft, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.btnUseRight, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.lblInstructions, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lblVanillaInfo, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(868, 712);
            this.tableLayoutPanel.TabIndex = 0;
            this.tableLayoutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel_MouseDown);
            this.tableLayoutPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel_MouseMove);
            this.tableLayoutPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel_MouseUp);
            // 
            // chkLineBreakSymbol
            // 
            this.chkLineBreakSymbol.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkLineBreakSymbol.AutoSize = true;
            this.chkLineBreakSymbol.Location = new System.Drawing.Point(758, 6);
            this.chkLineBreakSymbol.Name = "chkLineBreakSymbol";
            this.chkLineBreakSymbol.Size = new System.Drawing.Size(107, 17);
            this.chkLineBreakSymbol.TabIndex = 14;
            this.chkLineBreakSymbol.Text = "Show line &breaks";
            this.chkLineBreakSymbol.UseVisualStyleBackColor = true;
            this.chkLineBreakSymbol.CheckedChanged += new System.EventHandler(this.chkLineBreakSymbol_CheckedChanged);
            // 
            // lblRightInfo
            // 
            this.lblRightInfo.AutoSize = true;
            this.lblRightInfo.Location = new System.Drawing.Point(553, 692);
            this.lblRightInfo.Name = "lblRightInfo";
            this.lblRightInfo.Size = new System.Drawing.Size(58, 13);
            this.lblRightInfo.TabIndex = 9;
            this.lblRightInfo.Text = "[Right info]";
            // 
            // lblLeftInfo
            // 
            this.lblLeftInfo.AutoSize = true;
            this.lblLeftInfo.Location = new System.Drawing.Point(278, 692);
            this.lblLeftInfo.Name = "lblLeftInfo";
            this.lblLeftInfo.Size = new System.Drawing.Size(51, 13);
            this.lblLeftInfo.TabIndex = 8;
            this.lblLeftInfo.Text = "[Left info]";
            // 
            // btnUseVanilla
            // 
            this.btnUseVanilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseVanilla.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseVanilla.Location = new System.Drawing.Point(3, 33);
            this.btnUseVanilla.Name = "btnUseVanilla";
            this.btnUseVanilla.Size = new System.Drawing.Size(269, 39);
            this.btnUseVanilla.TabIndex = 0;
            this.btnUseVanilla.Text = "Use Vanilla Version";
            this.btnUseVanilla.UseVisualStyleBackColor = true;
            this.btnUseVanilla.Click += new System.EventHandler(this.btnUseVanilla_Click);
            // 
            // btnUseLeft
            // 
            this.btnUseLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseLeft.Location = new System.Drawing.Point(278, 33);
            this.btnUseLeft.Name = "btnUseLeft";
            this.btnUseLeft.Size = new System.Drawing.Size(269, 39);
            this.btnUseLeft.TabIndex = 1;
            this.btnUseLeft.Text = "Use ";
            this.btnUseLeft.UseVisualStyleBackColor = true;
            this.btnUseLeft.Click += new System.EventHandler(this.btnUseLeft_Click);
            // 
            // btnUseRight
            // 
            this.btnUseRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseRight.Location = new System.Drawing.Point(553, 33);
            this.btnUseRight.Name = "btnUseRight";
            this.btnUseRight.Size = new System.Drawing.Size(312, 39);
            this.btnUseRight.TabIndex = 2;
            this.btnUseRight.Text = "Use ";
            this.btnUseRight.UseVisualStyleBackColor = true;
            this.btnUseRight.Click += new System.EventHandler(this.btnUseRight_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInstructions.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblInstructions, 2);
            this.lblInstructions.Location = new System.Drawing.Point(3, 2);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(523, 26);
            this.lblInstructions.TabIndex = 6;
            this.lblInstructions.Text = "Found 2 different overlapping changes in the same part of the file.  Resolve this" +
    " by choosing which version to keep.";
            // 
            // lblVanillaInfo
            // 
            this.lblVanillaInfo.AutoSize = true;
            this.lblVanillaInfo.Location = new System.Drawing.Point(3, 692);
            this.lblVanillaInfo.Name = "lblVanillaInfo";
            this.lblVanillaInfo.Size = new System.Drawing.Size(64, 13);
            this.lblVanillaInfo.TabIndex = 7;
            this.lblVanillaInfo.Text = "[Vanilla info]";
            // 
            // rtbVanilla
            // 
            this.rtbVanilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbVanilla.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbVanilla.Location = new System.Drawing.Point(3, 78);
            this.rtbVanilla.Name = "rtbVanilla";
            this.rtbVanilla.Size = new System.Drawing.Size(269, 611);
            this.rtbVanilla.TabIndex = 3;
            this.rtbVanilla.Text = "";
            this.rtbVanilla.WordWrap = false;
            this.rtbVanilla.SelectionChanged += new System.EventHandler(this.rtbVanilla_SelectionChanged);
            // 
            // rtbRight
            // 
            this.rtbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRight.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbRight.Location = new System.Drawing.Point(553, 78);
            this.rtbRight.Name = "rtbRight";
            this.rtbRight.ReadOnly = true;
            this.rtbRight.Size = new System.Drawing.Size(312, 611);
            this.rtbRight.TabIndex = 5;
            this.rtbRight.Text = "";
            this.rtbRight.WordWrap = false;
            this.rtbRight.SelectionChanged += new System.EventHandler(this.rtbRight_SelectionChanged);
            // 
            // rtbLeft
            // 
            this.rtbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLeft.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLeft.Location = new System.Drawing.Point(278, 78);
            this.rtbLeft.Name = "rtbLeft";
            this.rtbLeft.ReadOnly = true;
            this.rtbLeft.Size = new System.Drawing.Size(269, 611);
            this.rtbLeft.TabIndex = 4;
            this.rtbLeft.Text = "";
            this.rtbLeft.WordWrap = false;
            this.rtbLeft.SelectionChanged += new System.EventHandler(this.rtbLeft_SelectionChanged);
            // 
            // ResolveConflictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 712);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "ResolveConflictForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resolve Conflict - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ResolveConflictForm_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.ConflictResolver_ResizeBegin);
            this.Resize += new System.EventHandler(this.ConflictResolver_Resize);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnUseRight;
        private System.Windows.Forms.Button btnUseLeft;
        private SyncedRichTextBox rtbVanilla;
        private SyncedRichTextBox rtbRight;
        private SyncedRichTextBox rtbLeft;
        private System.Windows.Forms.Button btnUseVanilla;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblRightInfo;
        private System.Windows.Forms.Label lblLeftInfo;
        private System.Windows.Forms.Label lblVanillaInfo;
        private System.Windows.Forms.CheckBox chkLineBreakSymbol;
    }
}