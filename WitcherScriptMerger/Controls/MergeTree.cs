using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Controls
{
    public class MergeTree : SMTree
    {
        #region Context Menu Members

        private ToolStripMenuItem contextOpenMergedFile = new ToolStripMenuItem();
        private ToolStripMenuItem contextOpenMergedFileDir = new ToolStripMenuItem();
        private ToolStripMenuItem contextDeleteAssociatedMerges = new ToolStripMenuItem();
        private ToolStripMenuItem contextDeleteMerge = new ToolStripMenuItem();
        private ToolStripSeparator contextDeleteSeparator = new ToolStripSeparator();

        #endregion

        public MergeTree()
        {
            FileNodeColor = Color.Blue;

            ContextOpenRegion.Items.AddRange(new ToolStripItem[]
            {
                contextOpenMergedFile, contextOpenMergedFileDir,
                contextDeleteSeparator,
                contextDeleteMerge,
                contextDeleteAssociatedMerges,
            });
            BuildContextMenu();
            
            // 
            // contextOpenMergedFile
            // 
            contextOpenMergedFile.Name = "contextOpenMergedFile";
            contextOpenMergedFile.Size = new Size(225, 22);
            contextOpenMergedFile.Text = "Open Merged File";
            contextOpenMergedFile.Click += ContextOpenScript_Click;
            // 
            // contextOpenMergedFileDir
            // 
            contextOpenMergedFileDir.Name = "contextOpenMergedFileDir";
            contextOpenMergedFileDir.Size = new Size(225, 22);
            contextOpenMergedFileDir.Text = "Open Merged File Directory";
            contextOpenMergedFileDir.Click += ContextOpenDirectory_Click;
            // 
            // contextDeleteMerge
            // 
            contextDeleteMerge.Name = "contextDeleteMerge";
            contextDeleteMerge.Size = new Size(225, 22);
            contextDeleteMerge.Text = "Delete This Merge";
            contextDeleteMerge.Click += ContextDeleteMerge_Click;
            // 
            // contextDeleteAssociatedMerges
            // 
            contextDeleteAssociatedMerges.Name = "contextDeleteAssociatedMerges";
            contextDeleteAssociatedMerges.Size = new Size(225, 22);
            contextDeleteAssociatedMerges.Text = "Delete All {0} Merges";
            contextDeleteAssociatedMerges.Click += ContextDeleteAssociatedMerges_Click;
            // 
            // contextDeleteSeparator
            // 
            contextDeleteSeparator.Name = "contextDeleteSeparator";
            contextDeleteSeparator.Size = new Size(235, 6);
        }

        protected override void HandleCheckedChange()
        {
            Program.MainForm.EnableUnmergeIfValidSelection();
        }

        protected override void OnLeftMouseUp(MouseEventArgs e)
        {
            if (ClickedNode != null && IsModNode(ClickedNode))
                ClickedNode = ClickedNode.Parent;

            base.OnLeftMouseUp(e);
        }

        protected override void SetAllChecked(bool isChecked)
        {
            foreach (var fileNode in FileNodes)
            {
                if (!ModFile.IsMergeable(fileNode.Text))
                    continue;
                fileNode.Checked = true;
            }
            Program.MainForm.EnableUnmergeIfValidSelection();
        }

        private void ContextDeleteMerge_Click(object sender, EventArgs e)
        {
            if (ClickedNode == null)
                return;

            if (IsModNode(ClickedNode))
                Program.MainForm.DeleteMerges(new TreeNode[] { ClickedNode.Parent });
            else
                Program.MainForm.DeleteMerges(new TreeNode[] { ClickedNode });
        }

        private void ContextDeleteAssociatedMerges_Click(object sender, EventArgs e)
        {
            if (ClickedNode == null || !IsModNode(ClickedNode))
                return;

            // Find all file nodes that contain a node matching the clicked node
            var fileNodes = FileNodes.Where(node =>
                node.GetTreeNodes().Any(modNode =>
                    modNode.Text == ClickedNode.Text));

            Program.MainForm.DeleteMerges(fileNodes);
        }

        protected override void SetContextItemAvailability()
        {
            base.SetContextItemAvailability();

            if (ClickedNode != null)
            {
                if (ClickedNode.Tag != null && IsFileNode(ClickedNode))
                {
                    string filePath = ClickedNode.Tag as string;
                    if (ModFile.IsScript(filePath))
                        contextOpenMergedFile.Available = contextOpenMergedFileDir.Available = true;
                    else
                        contextOpenMergedFile.Available = contextOpenMergedFileDir.Available = true;
                }

                contextDeleteMerge.Available = contextDeleteSeparator.Available = true;
                if (IsModNode(ClickedNode))
                {
                    contextDeleteAssociatedMerges.Available = true;
                    contextDeleteAssociatedMerges.Text = string.Format("Delete All {0} Merges", ClickedNode.Text);
                }
            }
        }
    }
}
