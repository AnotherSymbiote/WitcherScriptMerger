using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Controls
{
    public class ConflictTree : SMTree
    {
        #region Context Menu Members

        ToolStripMenuItem _contextOpenVanillaFile = new ToolStripMenuItem();
        ToolStripMenuItem _contextOpenVanillaFileDir = new ToolStripMenuItem();
        ToolStripSeparator _contextCustomLoadOrderSeparator = new ToolStripSeparator();
        ToolStripMenuItem _contextPrioritizeMod = new ToolStripMenuItem();
        ToolStripMenuItem _contextToggleMod = new ToolStripMenuItem();
        ToolStripMenuItem _contextRemoveFromCustomLoadOrder = new ToolStripMenuItem();

        #endregion

        public ConflictTree()
        {
            FileNodeForeColor = Color.Red;

            ContextOpenRegion.Items.AddRange(new ToolStripItem[]
            {
                _contextOpenVanillaFile,
                _contextOpenVanillaFileDir
            });
            ContextNodeRegion.Items.AddRange(new ToolStripItem[]
            {
                _contextCustomLoadOrderSeparator,
                _contextPrioritizeMod,
                _contextToggleMod,
                _contextRemoveFromCustomLoadOrder
            });
            BuildContextMenu();

            // contextOpenVanillaFile
            _contextOpenVanillaFile.Name = "contextOpenVanillaFile";
            _contextOpenVanillaFile.Size = new Size(225, 22);
            _contextOpenVanillaFile.Text = "Open Vanilla File";
            _contextOpenVanillaFile.Click += ContextOpenFile_Click;

            // contextOpenVanillaFileDir
            _contextOpenVanillaFileDir.Name = "contextOpenVanillaFileDir";
            _contextOpenVanillaFileDir.Size = new Size(225, 22);
            _contextOpenVanillaFileDir.Text = "Open Vanilla File Directory";
            _contextOpenVanillaFileDir.Click += ContextOpenDirectory_Click;

            // contextCustomLoadOrderSeparator
            _contextCustomLoadOrderSeparator.Name = "contextCustomLoadOrderSeparator";
            _contextCustomLoadOrderSeparator.Size = new Size(235, 6);
            
            // contextPrioritizeMod
            _contextPrioritizeMod.Name = "contextPrioritizeMod";
            _contextPrioritizeMod.Size = new Size(225, 22);
            _contextPrioritizeMod.Click += ContextPrioritizeMod;
            _contextPrioritizeMod.Text = "Set Mod Priority...";

            // contextToggleMod
            _contextToggleMod.Name = "contextToggleMod";
            _contextToggleMod.Size = new Size(225, 22);
            _contextToggleMod.Click += ContextToggleMod;

            // contextRemoveFromCustomLoadOrder
            _contextRemoveFromCustomLoadOrder.Name = "contextRemoveFromCustomLoadOrder";
            _contextRemoveFromCustomLoadOrder.Size = new Size(225, 22);
            _contextRemoveFromCustomLoadOrder.Click += ContextRemoveFromCustomLoadOrder;
            _contextRemoveFromCustomLoadOrder.Text = "Remove from Custom Load Order";
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

            if (ClickedNode != null)
            {
                if (IsFileNode(ClickedNode) && !((ModFileCategory)ClickedNode.Parent.Tag).IsBundled)
                {
                    _contextOpenVanillaFile.Available = true;
                    _contextOpenVanillaFileDir.Available = true;
                }
                else if (IsModNode(ClickedNode))
                {
                    _contextCustomLoadOrderSeparator.Available = true;
                    _contextPrioritizeMod.Available = true;

                    _contextToggleMod.Available = true;
                    _contextToggleMod.Text = $"{(Program.LoadOrder.IsModDisabledByName(ClickedNode.Text) ? "Enable" : "Disable")} Mod";
                    _contextRemoveFromCustomLoadOrder.Available = Program.LoadOrder.Contains(ClickedNode.Text);
                }
            }
            else if (!this.IsEmpty())
            {
                ContextSelectAll.Available = CategoryNodes.Any(catNode => !catNode.Checked && (catNode.Tag as ModFileCategory).IsSupported);

                ContextDeselectAll.Available = ModNodes.Any(modNode => modNode.Checked);
            }
        }

        void ContextPrioritizeMod(object sender, EventArgs e)
        {
            var modName = RightClickedNode.Text;
            int? inputVal;

            using (var prompt = new NumericPrompt())
            {
                inputVal = prompt.ShowDialog("Set Priority", Program.LoadOrder.GetPriorityByName(modName));
            }

            if (!inputVal.HasValue)
                return;

            Program.LoadOrder = new CustomLoadOrder();
            Program.LoadOrder.SetPriorityByName(modName, inputVal.Value);
            Program.LoadOrder.AddMergedModIfMissing();
            Program.LoadOrder.Save();

            Program.MainForm.SetStylesForCustomLoadOrder();
        }

        void ContextToggleMod(object sender, EventArgs e)
        {
            var modName = RightClickedNode.Text;

            Program.LoadOrder = new CustomLoadOrder();
            Program.LoadOrder.ToggleModByName(modName);
            Program.LoadOrder.AddMergedModIfMissing();
            Program.LoadOrder.Save();

            Program.MainForm.SetStylesForCustomLoadOrder();
        }

        private void ContextRemoveFromCustomLoadOrder(object sender, EventArgs e)
        {
            var modName = RightClickedNode.Text;

            Program.LoadOrder = new CustomLoadOrder();

            int index = Program.LoadOrder.Mods.FindIndex(setting => setting.ModName.EqualsIgnoreCase(modName));

            if (index > -1)
                Program.LoadOrder.Mods.RemoveAt(index);

            Program.LoadOrder.Save();

            Program.MainForm.SetStylesForCustomLoadOrder();
        }
    }
}
