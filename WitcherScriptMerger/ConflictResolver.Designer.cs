namespace WitcherScriptMerger
{
    partial class ConflictResolver
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConflictResolver));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.rtbVanilla = new System.Windows.Forms.RichTextBox();
            this.rtbRight = new System.Windows.Forms.RichTextBox();
            this.rtbLeft = new System.Windows.Forms.RichTextBox();
            this.btnUseVanilla = new System.Windows.Forms.Button();
            this.btnUseLeft = new System.Windows.Forms.Button();
            this.btnUseRight = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblVanillaInfo = new System.Windows.Forms.Label();
            this.lblLeftInfo = new System.Windows.Forms.Label();
            this.lblRightInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
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
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(868, 712);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // rtbVanilla
            // 
            this.rtbVanilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbVanilla.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbVanilla.Location = new System.Drawing.Point(3, 73);
            this.rtbVanilla.Name = "rtbVanilla";
            this.rtbVanilla.Size = new System.Drawing.Size(283, 616);
            this.rtbVanilla.TabIndex = 3;
            this.rtbVanilla.Text = "";
            this.rtbVanilla.WordWrap = false;
            this.rtbVanilla.SelectionChanged += new System.EventHandler(this.rtbVanilla_SelectionChanged);
            // 
            // rtbRight
            // 
            this.rtbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRight.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbRight.Location = new System.Drawing.Point(581, 73);
            this.rtbRight.Name = "rtbRight";
            this.rtbRight.ReadOnly = true;
            this.rtbRight.Size = new System.Drawing.Size(284, 616);
            this.rtbRight.TabIndex = 5;
            this.rtbRight.Text = "";
            this.rtbRight.WordWrap = false;
            this.rtbRight.SelectionChanged += new System.EventHandler(this.rtbRight_SelectionChanged);
            // 
            // rtbLeft
            // 
            this.rtbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLeft.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLeft.Location = new System.Drawing.Point(292, 73);
            this.rtbLeft.Name = "rtbLeft";
            this.rtbLeft.ReadOnly = true;
            this.rtbLeft.Size = new System.Drawing.Size(283, 616);
            this.rtbLeft.TabIndex = 4;
            this.rtbLeft.Text = "";
            this.rtbLeft.WordWrap = false;
            this.rtbLeft.SelectionChanged += new System.EventHandler(this.rtbLeft_SelectionChanged);
            // 
            // btnUseVanilla
            // 
            this.btnUseVanilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseVanilla.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseVanilla.Location = new System.Drawing.Point(3, 28);
            this.btnUseVanilla.Name = "btnUseVanilla";
            this.btnUseVanilla.Size = new System.Drawing.Size(283, 39);
            this.btnUseVanilla.TabIndex = 0;
            this.btnUseVanilla.Text = "Use Vanilla Version";
            this.btnUseVanilla.UseVisualStyleBackColor = true;
            this.btnUseVanilla.Click += new System.EventHandler(this.btnUseVanilla_Click);
            // 
            // btnUseLeft
            // 
            this.btnUseLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseLeft.Location = new System.Drawing.Point(292, 28);
            this.btnUseLeft.Name = "btnUseLeft";
            this.btnUseLeft.Size = new System.Drawing.Size(283, 39);
            this.btnUseLeft.TabIndex = 1;
            this.btnUseLeft.Text = "Use ";
            this.btnUseLeft.UseVisualStyleBackColor = true;
            this.btnUseLeft.Click += new System.EventHandler(this.btnUseLeft_Click);
            // 
            // btnUseRight
            // 
            this.btnUseRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUseRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseRight.Location = new System.Drawing.Point(581, 28);
            this.btnUseRight.Name = "btnUseRight";
            this.btnUseRight.Size = new System.Drawing.Size(284, 39);
            this.btnUseRight.TabIndex = 2;
            this.btnUseRight.Text = "Use ";
            this.btnUseRight.UseVisualStyleBackColor = true;
            this.btnUseRight.Click += new System.EventHandler(this.btnUseRight_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInstructions.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblInstructions, 3);
            this.lblInstructions.Location = new System.Drawing.Point(3, 6);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(550, 13);
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
            // lblLeftInfo
            // 
            this.lblLeftInfo.AutoSize = true;
            this.lblLeftInfo.Location = new System.Drawing.Point(292, 692);
            this.lblLeftInfo.Name = "lblLeftInfo";
            this.lblLeftInfo.Size = new System.Drawing.Size(51, 13);
            this.lblLeftInfo.TabIndex = 8;
            this.lblLeftInfo.Text = "[Left info]";
            // 
            // lblRightInfo
            // 
            this.lblRightInfo.AutoSize = true;
            this.lblRightInfo.Location = new System.Drawing.Point(581, 692);
            this.lblRightInfo.Name = "lblRightInfo";
            this.lblRightInfo.Size = new System.Drawing.Size(58, 13);
            this.lblRightInfo.TabIndex = 9;
            this.lblRightInfo.Text = "[Right info]";
            // 
            // ConflictResolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 712);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConflictResolver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conflict Resolver - ";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnUseRight;
        private System.Windows.Forms.Button btnUseLeft;
        private System.Windows.Forms.RichTextBox rtbVanilla;
        private System.Windows.Forms.RichTextBox rtbRight;
        private System.Windows.Forms.RichTextBox rtbLeft;
        private System.Windows.Forms.Button btnUseVanilla;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblRightInfo;
        private System.Windows.Forms.Label lblLeftInfo;
        private System.Windows.Forms.Label lblVanillaInfo;
    }
}