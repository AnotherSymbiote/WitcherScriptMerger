using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Controls
{
    public abstract class SMTree : TreeView
    {
        #region Types

        public enum LevelType : int
        {
            Categories, Files, Mods
        }

        #endregion

        #region Members

        public List<TreeNode> CategoryNodes
        {
            get { return GetNodesAtLevel(LevelType.Categories); }
        }

        public List<TreeNode> FileNodes
        {
            get { return GetNodesAtLevel(LevelType.Files); }
        }

        public List<TreeNode> ModNodes
        {
            get { return GetNodesAtLevel(LevelType.Mods); }
        }

        ////public Color CategoryNodeColor;
        public Color FileNodeColor;
        ////public Color ModNodeColor;

        protected TreeNode ClickedNode = null;
        protected bool IsUpdating = false;

        #endregion

        #region Context Menu Members

        private ContextMenuStrip _contextMenu;

        protected ToolStripRegion ContextOpenRegion;
        protected ToolStripRegion ContextNodeRegion;
        protected List<ToolStripRegion> ContextRegions;

        private ToolStripMenuItem _contextOpenModScript = new ToolStripMenuItem();
        private ToolStripMenuItem _contextOpenModScriptDir = new ToolStripMenuItem();
        private ToolStripMenuItem _contextOpenModBundleDir = new ToolStripMenuItem();
        private ToolStripMenuItem _contextCopyPath = new ToolStripMenuItem();
        private ToolStripSeparator _contextOpenSeparator = new ToolStripSeparator();
        private ToolStripMenuItem _contextExpandAll = new ToolStripMenuItem();
        private ToolStripMenuItem _contextCollapseAll = new ToolStripMenuItem();
        protected ToolStripMenuItem ContextSelectAll = new ToolStripMenuItem();
        protected ToolStripMenuItem ContextDeselectAll = new ToolStripMenuItem();

        #endregion

        public SMTree()
        {
            InitializeContextMenu();
            TreeViewNodeSorter = new SMTreeSorter();
        }

        protected List<TreeNode> GetNodesAtLevel(LevelType level)
        {
            var nodes = Nodes.Cast<TreeNode>();
            for (int i = 0; i < (int)level; ++i)
                nodes = nodes.SelectMany(node => node.GetTreeNodes());
            return nodes.ToList();
        }

        public TreeNode GetCategoryNode(ModFileCategory category)
        {
            return CategoryNodes.FirstOrDefault(node =>
                category == (ModFileCategory)node.Tag);
        }

        public void SetFontBold(LevelType level)
        {
            BeginUpdate();
            foreach (var node in GetNodesAtLevel(level))
                node.SetFontBold();
            EndUpdate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                    ContextSelectAll_Click(null, null);
                else if (e.KeyCode == Keys.D)
                    ContextDeselectAll_Click(null, null);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            SelectedNode = null;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ClickedNode = GetNodeAt(e.Location);
            if (ClickedNode != null && !ClickedNode.Bounds.Contains(e.Location))
                ClickedNode = null;

            if (e.Button == MouseButtons.Right)
            {
                BeginUpdate();
                IsUpdating = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            var lastClicked = ClickedNode;
            ClickedNode = GetNodeAt(e.Location);
            if (ClickedNode != null &&
                (lastClicked != ClickedNode || !ClickedNode.Bounds.Contains(e.Location)))
                ClickedNode = null;

            if (e.Button == MouseButtons.Left)
                OnLeftMouseUp(e);
            else if (e.Button == MouseButtons.Right)
                OnRightMouseUp(e);
            EndUpdate();
            IsUpdating = false;
        }

        protected virtual void OnLeftMouseUp(MouseEventArgs e)
        {
            if (ClickedNode == null)
                return;
            ClickedNode.Checked = !ClickedNode.Checked;
            HandleCheckedChange();
        }

        protected virtual void OnRightMouseUp(MouseEventArgs e)
        {
            SetContextItemAvailability();

            if (ClickedNode != null)
                ClickedNode.BackColor = Color.Gainsboro;

            if (_contextMenu.Items.OfType<ToolStripMenuItem>().Any(item => item.Available))
            {
                ////_clickedTree = this;
                _contextMenu.Show(this, e.X, e.Y);
            }
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);
            if (e.Action != TreeViewAction.Unknown)  // Event was triggered programmatically
            {
                ClickedNode = e.Node;
                HandleCheckedChange();
            }
        }

        protected abstract void HandleCheckedChange();

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsUpdating)
                EndUpdate();
        }

        protected LevelType GetLevelType(TreeNode node)
        {
            return (LevelType)node.Level;
        }

        protected bool IsCategoryNode(TreeNode node)
        {
            return GetLevelType(node) == LevelType.Categories;
        }

        protected bool IsFileNode(TreeNode node)
        {
            return GetLevelType(node) == LevelType.Files;
        }

        protected bool IsModNode(TreeNode node)
        {
            return GetLevelType(node) == LevelType.Mods;
        }

        #region Context Menu

        private void InitializeContextMenu()
        {
            _contextMenu = new ContextMenuStrip();

            ContextRegions = new List<ToolStripRegion>()
            {
                ContextOpenRegion, ContextNodeRegion
            };

            ContextOpenRegion = new ToolStripRegion(_contextMenu as ToolStrip, new ToolStripItem[]
            {
                _contextOpenModScript,
                _contextOpenModScriptDir,
                _contextOpenModBundleDir,
                _contextCopyPath,
                _contextOpenSeparator
            });
            ContextNodeRegion = new ToolStripRegion(_contextMenu as ToolStrip, new ToolStripItem[]
            {
                ContextSelectAll, ContextDeselectAll,
                _contextExpandAll, _contextCollapseAll
            });

            // 
            // treeContextMenu
            // 
            _contextMenu.AutoSize = false;
            _contextMenu.Name = "treeContextMenu";
            _contextMenu.Size = new Size(239, 390);
            _contextMenu.Closing += ContextMenu_Closing;
            // 
            // contextOpenModScript
            // 
            _contextOpenModScript.Name = "contextOpenModScript";
            _contextOpenModScript.Size = new Size(225, 22);
            _contextOpenModScript.Text = "Open Mod Script";
            _contextOpenModScript.Click += ContextOpenScript_Click;
            // 
            // contextOpenModScriptDir
            // 
            _contextOpenModScriptDir.Name = "contextOpenModScriptDir";
            _contextOpenModScriptDir.Size = new Size(225, 22);
            _contextOpenModScriptDir.Text = "Open Mod Script Directory";
            _contextOpenModScriptDir.Click += ContextOpenDirectory_Click;

            // 
            // contextOpenModBundleDir
            // 
            _contextOpenModBundleDir.Name = "contextOpenModBundleDir";
            _contextOpenModBundleDir.Size = new Size(225, 22);
            _contextOpenModBundleDir.Text = "Open Mod Bundle Directory";
            _contextOpenModBundleDir.Click += ContextOpenDirectory_Click;
            // 
            // contextCopyPath
            // 
            _contextCopyPath.Name = "contextCopyPath";
            _contextCopyPath.Size = new Size(225, 22);
            _contextCopyPath.Text = "Copy Path";
            _contextCopyPath.Click += ContextCopyPath_Click;
            // 
            // contextOpenSeparator
            // 
            _contextOpenSeparator.Name = "contextOpenSeparator";
            _contextOpenSeparator.Size = new Size(235, 6);
            // 
            // contextSelectAll
            // 
            ContextSelectAll.Name = "contextSelectAll";
            ContextSelectAll.Size = new Size(225, 22);
            ContextSelectAll.Text = "Select All";
            ContextSelectAll.Click += ContextSelectAll_Click;
            // 
            // contextDeselectAll
            // 
            ContextDeselectAll.Name = "contextDeselectAll";
            ContextDeselectAll.Size = new Size(225, 22);
            ContextDeselectAll.Text = "Deselect All";
            ContextDeselectAll.Click += ContextDeselectAll_Click;
            // 
            // contextExpandAll
            // 
            _contextExpandAll.Name = "contextExpandAll";
            _contextExpandAll.Size = new Size(225, 22);
            _contextExpandAll.Text = "Expand All";
            _contextExpandAll.Click += ContextExpandAll_Click;
            // 
            // contextCollapseAll
            // 
            _contextCollapseAll.Name = "contextCollapseAll";
            _contextCollapseAll.Size = new Size(225, 22);
            _contextCollapseAll.Text = "Collapse All";
            _contextCollapseAll.Click += ContextCollapseAll_Click;
        }

        protected void BuildContextMenu()
        {
            _contextMenu.Items.Clear();
            _contextMenu.Items.AddRange(ContextOpenRegion.Items);
            _contextMenu.Items.Add(_contextOpenSeparator);
            _contextMenu.Items.AddRange(ContextNodeRegion.Items);
        }

        protected virtual void SetContextItemAvailability()
        {
            foreach (var menuItem in _contextMenu.Items.OfType<ToolStripItem>())
                menuItem.Available = false;

            if (ClickedNode != null && ClickedNode.Tag is string)
            {
                _contextCopyPath.Available = true;
                if (IsModNode(ClickedNode))
                {
                    string filePath = ClickedNode.Tag as string;
                    if (ModFile.IsScript(filePath))
                        _contextOpenModScript.Available = _contextOpenModScriptDir.Available = true;
                    else
                        _contextOpenModBundleDir.Available = true;
                }
            }

            // If can copy path, need separator above Select/Deselect All
            if (_contextCopyPath.Available)
                _contextOpenSeparator.Visible = true;

            if (!this.IsEmpty())
            {
                if (FileNodes.Any(fileNode => !fileNode.Checked && ModFile.IsMergeable(fileNode.Text)))
                    ContextSelectAll.Available = true;
                if (FileNodes.Any(fileNode => fileNode.Checked && ModFile.IsMergeable(fileNode.Text)))
                    ContextDeselectAll.Available = true;
                if (FileNodes.Any(node => !node.IsExpanded)) _contextExpandAll.Available = true;
                if (FileNodes.Any(node => node.IsExpanded)) _contextCollapseAll.Available = true;
            }

            if (_contextMenu.Items.OfType<ToolStripItem>().Any(item => item.Available))
            {
                int width = _contextMenu.Items.OfType<ToolStripMenuItem>().Where(item => item.Available)
                    .Max(item => TextRenderer.MeasureText(item.Text, item.Font).Width);
                int height = _contextMenu.GetAvailableItems()
                    .Sum(item => item.Height);
                _contextMenu.Width = width + 45;
                _contextMenu.Height = height + 5;
            }
        }

        protected void ContextOpenScript_Click(object sender, EventArgs e)
        {
            if (ClickedNode == null)
                return;
            string filePath = ClickedNode.Tag as string;
            if (!File.Exists(filePath))
                Program.MainForm.ShowMessage("Can't find file: " + filePath);
            else
                Process.Start(filePath);
        }

        protected void ContextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (ClickedNode == null)
                return;
            var dirPath = Path.GetDirectoryName(ClickedNode.Tag as string);
            if (!Directory.Exists(dirPath))
                Program.MainForm.ShowMessage("Can't find directory: " + dirPath);
            else
                Process.Start(dirPath);
        }

        private void ContextCopyPath_Click(object sender, EventArgs e)
        {
            if (ClickedNode == null)
                return;
            Clipboard.SetText(ClickedNode.Tag as string);
        }

        protected void ContextSelectAll_Click(object sender, EventArgs e)
        {
            SetAllChecked(true);
        }

        protected void ContextDeselectAll_Click(object sender, EventArgs e)
        {
            SetAllChecked(false);
        }

        protected abstract void SetAllChecked(bool isChecked);

        private void ContextExpandAll_Click(object sender, EventArgs e)
        {
            ExpandAll();
        }

        private void ContextCollapseAll_Click(object sender, EventArgs e)
        {
            CollapseAll();
        }

        private void ContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (ClickedNode != null)
            {
                ClickedNode.BackColor = Color.Transparent;
                ClickedNode.TreeView.Update();
            }
        }

        #endregion
    }
}