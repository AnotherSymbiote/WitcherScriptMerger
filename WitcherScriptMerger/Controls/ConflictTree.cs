using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Controls
{
    class ConflictTree : SMTree
    {
        #region Members

        public static readonly Color UnresolvedForeColor = Color.Red;
        public static readonly Color ResolvedForeColor = Color.Purple;

        public static readonly new Color FileNodeForeColor = UnresolvedForeColor;

        ToolStripSeparator _contextCustomLoadOrderSeparator = new ToolStripSeparator();
        ToolStripMenuItem _contextPrioritizeMod = new ToolStripMenuItem();
        ToolStripMenuItem _contextToggleMod = new ToolStripMenuItem();
        ToolStripMenuItem _contextRemoveFromCustomLoadOrder = new ToolStripMenuItem();

        #endregion

        public ConflictTree()
        {
            ContextNodeRegion.Items.AddRange(new ToolStripItem[]
            {
                _contextCustomLoadOrderSeparator,
                _contextPrioritizeMod,
                _contextToggleMod,
                _contextRemoveFromCustomLoadOrder
            });
            BuildContextMenu();

            // contextCustomLoadOrderSeparator
            _contextCustomLoadOrderSeparator.Name = "contextCustomLoadOrderSeparator";
            _contextCustomLoadOrderSeparator.Size = new Size(235, 6);
            
            // contextPrioritizeMod
            _contextPrioritizeMod.Name = "contextPrioritizeMod";
            _contextPrioritizeMod.Size = new Size(225, 22);
            _contextPrioritizeMod.Text = "Set Overall Mod Priority...";
            _contextPrioritizeMod.ToolTipText = "Lets you define the load order of your mods";
            _contextPrioritizeMod.Click += ContextPrioritizeMod;

            // contextToggleMod
            _contextToggleMod.Name = "contextToggleMod";
            _contextToggleMod.Size = new Size(225, 22);
            _contextToggleMod.ToolTipText = "Tells the game whether to load any of this mod's files";
            _contextToggleMod.Click += ContextToggleMod;

            // contextRemoveFromCustomLoadOrder
            _contextRemoveFromCustomLoadOrder.Name = "contextRemoveFromCustomLoadOrder";
            _contextRemoveFromCustomLoadOrder.Size = new Size(225, 22);
            _contextRemoveFromCustomLoadOrder.ToolTipText = "Removes this mod's custom load order settings";
            _contextRemoveFromCustomLoadOrder.Click += ContextRemoveFromCustomLoadOrder;
        }

        protected override void HandleCheckedChange()
        {
            if (IsCategoryNode(ClickedNode))
            {
                foreach (var fileNode in ClickedNode.GetTreeNodes())
                {
                    fileNode.SetCheckedIfVisible(ClickedNode.Checked);
                    foreach (var modNode in fileNode.GetTreeNodes())
                        modNode.SetCheckedIfVisible(ClickedNode.Checked);
                }
            }
            else if (IsFileNode(ClickedNode))
            {
                foreach (var modNode in ClickedNode.GetTreeNodes())
                    modNode.SetCheckedIfVisible(ClickedNode.Checked);

                var catNode = ClickedNode.Parent;
                catNode.Checked = catNode.AreAllVisibleCheckboxesChecked();
            }
            else if (IsModNode(ClickedNode))
            {
                var fileNode = ClickedNode.Parent;
                fileNode.Checked = fileNode.AreAllVisibleCheckboxesChecked();

                var catNode = fileNode.Parent;
                catNode.Checked = catNode.AreAllVisibleCheckboxesChecked();
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
                        modNode.SetCheckedIfVisible(isChecked);
                }
            }
            Program.MainForm.EnableMergeIfValidSelection();
        }

        protected override void SetContextItemAvailability()
        {
            base.SetContextItemAvailability();

            if (ClickedNode != null)
            {
                if (IsModNode(ClickedNode))
                {
                    _contextCustomLoadOrderSeparator.Available = true;
                    _contextPrioritizeMod.Available = true;

                    foreach (var item in ContextNodeRegion.Items.Cast<ToolStripItem>())
                    {
                        item.Enabled = Program.LoadOrder.IsValid;
                    }

                    var isDisabled = Program.LoadOrder.IsModDisabledByName(ClickedNode.Text);

                    _contextToggleMod.Available = true;
                    _contextToggleMod.Text =
                        isDisabled
                        ? "Enable Mod"
                        : "Disable Mod";

                    _contextRemoveFromCustomLoadOrder.Available = Program.LoadOrder.ContainsMod(ClickedNode.Text);
                    _contextRemoveFromCustomLoadOrder.Text =
                        isDisabled
                        ? "Clear Priority && Disabled State"
                        : "Clear Priority";
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
            RightClickedNode.BackColor = Color.Gainsboro;

            var modName = RightClickedNode.Text;
            int? inputVal;

            using (var prompt = new PriorityPrompt())
            {
                inputVal = prompt.ShowDialog(Program.LoadOrder.GetPriorityByName(modName));
            }

            RightClickedNode.BackColor = Color.Transparent;

            if (!inputVal.HasValue)
                return;

            Program.LoadOrder.Refresh();
            Program.LoadOrder.SetPriorityByName(modName, inputVal.Value);
            Program.LoadOrder.AddMergedModIfMissing();
            Program.LoadOrder.Save();

            SetStylesForCustomLoadOrder();
        }

        void ContextToggleMod(object sender, EventArgs e)
        {
            var modName = RightClickedNode.Text;

            Program.LoadOrder.Refresh();
            Program.LoadOrder.ToggleModByName(modName);
            Program.LoadOrder.AddMergedModIfMissing();
            Program.LoadOrder.Save();

            SetStylesForCustomLoadOrder();

            var fileNode = RightClickedNode.Parent;

            if ((fileNode.Parent.Tag as ModFileCategory).IsSupported)
            {
                fileNode.Checked = fileNode.GetTreeNodes()
                    .Where(modNode => modNode.IsCheckBoxVisible())
                    .All(modNode => modNode.Checked);
            }

            Program.MainForm.EnableMergeIfValidSelection();
        }

        void ContextRemoveFromCustomLoadOrder(object sender, EventArgs e)
        {
            Program.LoadOrder.Refresh();

            var modName = RightClickedNode.Text;

            var index = Program.LoadOrder.Mods.FindIndex(setting => setting.ModName.EqualsIgnoreCase(modName));

            if (index > -1)
            {
                Program.LoadOrder.Mods.RemoveAt(index);
                Program.LoadOrder.Save();
            }

            SetStylesForCustomLoadOrder();
        }

        internal void SetStylesForCustomLoadOrder()
        {
            foreach (var fileNode in FileNodes)
            {
                var modNames = fileNode.GetTreeNodes().Select(modNode => modNode.Text);

                var isResolved = Program.LoadOrder.HasResolvedConflict(modNames);

                var topPriorityMod =
                    isResolved
                    ? Program.LoadOrder.GetTopPriorityEnabledMod(modNames)
                    : null;

                fileNode.ForeColor =
                    isResolved
                    ? ResolvedForeColor
                    : UnresolvedForeColor;

                foreach (var modNode in fileNode.GetTreeNodes())
                {
                    modNode.NodeFont = DefaultFont;
                    modNode.ForeColor = DefaultForeColor;
                    modNode.ToolTipText = "";

                    var priority = Program.LoadOrder.GetPriorityByName(modNode.Text);

                    modNode.ToolTipText =
                        priority > -1
                        ? $"Priority {priority}"
                        : "No Priority";

                    if (modNode.Text.EqualsIgnoreCase(topPriorityMod))
                    {
                        if (priority > -1)
                            modNode.ToolTipText += " - Top priority in this conflict";
                    }
                    else if (isResolved)
                    {
                        modNode.ToolTipText += " - Overridden by a higher-priority mod";
                        modNode.ForeColor = Color.Gray;
                    }

                    if (Program.LoadOrder.IsModDisabledByName(modNode.Text))
                    {
                        modNode.ToolTipText = "This mod is disabled in your custom load order";
                        modNode.ForeColor = Color.Gray;
                        modNode.SetFontStyle(FontStyle.Strikeout);
                        modNode.Checked = false;
                        modNode.SetIsCheckBoxVisible(false);
                    }
                    else if ((fileNode.Parent.Tag as ModFileCategory).IsSupported)
                        modNode.SetIsCheckBoxVisible(true);

                    var mergeFile = Program.Inventory
                        ?.GetMergeByRelativePath(fileNode.Text)
                        ?.GetHashByModName(modNode.Text);
                    if (mergeFile != null && mergeFile.IsOutdated)
                    {
                        modNode.ToolTipText += " - CHANGED SINCE MERGE";
                        modNode.SetFontStyle(FontStyle.Italic);
                    }
                }
            }
        }
    }
}
