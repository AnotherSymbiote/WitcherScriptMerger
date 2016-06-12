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
        ToolStripSeparator _contextPrioritizeModSeparator = new ToolStripSeparator();
        ToolStripMenuItem _contextPrioritizeMod = new ToolStripMenuItem();


        #endregion

        public ConflictTree()
        {
            FileNodeForeColor = Color.Red;

            ContextOpenRegion.Items.AddRange(new ToolStripItem[]
            {
                _contextOpenVanillaFile,
                _contextOpenVanillaFileDir,
                _contextPrioritizeModSeparator,
                _contextPrioritizeMod
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

            // contextPrioritizeModSeparator
            _contextPrioritizeModSeparator.Name = "contextPrioritizeModSeparator";
            _contextPrioritizeModSeparator.Size = new Size(235, 6);
            
            // contextPrioritizeMod
            _contextPrioritizeMod.Name = "contextPrioritizeMod";
            _contextPrioritizeMod.Size = new Size(225, 22);
            _contextPrioritizeMod.Click += ContextPrioritizeMod;

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
                    _contextPrioritizeModSeparator.Available = true;
                    _contextPrioritizeMod.Available = true;
                    _contextPrioritizeMod.Text = $"Set Priority...";
                }
            }

            if (!this.IsEmpty())
            {
                if (CategoryNodes.Any(catNode => !catNode.Checked && (catNode.Tag as ModFileCategory).IsSupported))
                    ContextSelectAll.Available = true;
                if (ModNodes.Any(modNode => modNode.Checked))
                    ContextDeselectAll.Available = true;
            }
        }

        private void ContextPrioritizeMod(object sender, EventArgs e)
        {
            var modName = RightClickedNode.Text;
            var inputString = Prompt.ShowDialog("Priority:", modName);

            if (inputString == null)
                return;

            int inputInt;
            if (!int.TryParse(inputString, out inputInt))
            {
                Program.MainForm.ShowMessage($"Priority must be an integer.", "Invalid Priority");
                return;
            }
            if (inputInt < CustomLoadOrder.MinPriority + 1)
            {
                Program.MainForm.ShowMessage($"Priority must be greater than {CustomLoadOrder.MinPriority}.  Merged files take priority {CustomLoadOrder.MinPriority}.", "Invalid Priority");
                return;
            }

            var loadOrder = new CustomLoadOrder();
            loadOrder.SetPriorityByName(modName, inputInt);

            var mergedModName = Paths.RetrieveMergedModName();
            if (!loadOrder.Mods.Any(setting => setting.ModName.EqualsIgnoreCase(mergedModName)))
            {
                loadOrder.Mods.Insert(0,
                    new ModLoadSetting
                    {
                        ModName = mergedModName,
                        IsEnabled = true,
                        Priority = CustomLoadOrder.MinPriority
                    });
            }

            loadOrder.Save();

            Program.MainForm.SetStylesForCustomLoadOrder(loadOrder);
        }
    }
}
