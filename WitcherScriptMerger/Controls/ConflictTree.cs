using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Controls
{
    public class ConflictTree : SMTree
    {
        #region Context Menu Members

        private ToolStripMenuItem _contextOpenVanillaScript = new ToolStripMenuItem();
        private ToolStripMenuItem _contextOpenVanillaScriptDir = new ToolStripMenuItem();

        #endregion

        public ConflictTree()
        {
            FileNodeForeColor = Color.Red;

            ContextOpenRegion.Items.AddRange(new ToolStripItem[]
            {
                _contextOpenVanillaScript, _contextOpenVanillaScriptDir
            });
            BuildContextMenu();

            // 
            // contextOpenVanillaScript
            // 
            _contextOpenVanillaScript.Name = "contextOpenVanillaScript";
            _contextOpenVanillaScript.Size = new Size(225, 22);
            _contextOpenVanillaScript.Text = "Open Vanilla Script";
            _contextOpenVanillaScript.Click += ContextOpenScript_Click;
            // 
            // contextOpenVanillaScriptDir
            // 
            _contextOpenVanillaScriptDir.Name = "contextOpenVanillaScriptDir";
            _contextOpenVanillaScriptDir.Size = new Size(225, 22);
            _contextOpenVanillaScriptDir.Text = "Open Vanilla Script Directory";
            _contextOpenVanillaScriptDir.Click += ContextOpenDirectory_Click;
        }

        protected override void HandleCheckedChange()
        {
            if (IsCategoryNode(ClickedNode))
            {
                foreach (var fileNode in ClickedNode.GetTreeNodes())
                {
                    fileNode.Checked = ClickedNode.Checked;
                    foreach (var modNode in fileNode.GetTreeNodes())
                        modNode.Checked = ClickedNode.Checked;
                }
            }
            else if (IsFileNode(ClickedNode))
            {
                foreach (var modNode in ClickedNode.GetTreeNodes())
                    modNode.Checked = ClickedNode.Checked;

                var catNode = ClickedNode.Parent;
                catNode.Checked = catNode.GetTreeNodes().All(node => node.Checked);
            }
            else if (IsModNode(ClickedNode))
            {
                var fileNode = ClickedNode.Parent;
                fileNode.Checked = fileNode.GetTreeNodes().All(node => node.Checked);

                var catNode = fileNode.Parent;
                catNode.Checked = catNode.GetTreeNodes().All(node => node.Checked);
            }
            Program.MainForm.EnableMergeIfValidSelection();
        }

        protected override void OnLeftMouseUp(MouseEventArgs e)
        {
            if (ClickedNode == null)
                return;

            TreeNode catNode;
            if (IsCategoryNode(ClickedNode))
                catNode = ClickedNode;
            else if (IsFileNode(ClickedNode))
                catNode = ClickedNode.Parent;
            else
                catNode = ClickedNode.Parent.Parent;

            var category = catNode.Tag as ModFileCategory;
            if (!category.IsSupported)
            {
                EndUpdate();
                IsUpdating = false;
            }
            else
                base.OnLeftMouseUp(e);
        }

        protected override void SetAllChecked(bool isChecked)
        {
            foreach (var catNode in CategoryNodes)
            {
                var category = catNode.Tag as ModFileCategory;
                if (!category.IsSupported)
                    continue;
                catNode.Checked = isChecked;
                foreach (var fileNode in catNode.GetTreeNodes())
                {
                    fileNode.Checked = isChecked;
                    foreach (var modNode in fileNode.GetTreeNodes())
                        modNode.Checked = isChecked;
                }
            }
            Program.MainForm.EnableMergeIfValidSelection();
        }

        protected override void SetContextItemAvailability()
        {
            base.SetContextItemAvailability();

            if (ClickedNode != null && IsFileNode(ClickedNode) && ModFile.IsScript(ClickedNode.Text))
            {
                _contextOpenVanillaScript.Available = true;
                _contextOpenVanillaScriptDir.Available = true;
            }

            if (!this.IsEmpty())
            {
                if (CategoryNodes.Any(catNode => !catNode.Checked && (catNode.Tag as ModFileCategory).IsSupported))
                    ContextSelectAll.Available = true;
                if (ModNodes.Any(modNode => modNode.Checked))
                    ContextDeselectAll.Available = true;
            }
        }
    }
}
