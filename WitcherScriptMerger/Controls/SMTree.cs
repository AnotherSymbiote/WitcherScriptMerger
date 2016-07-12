using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static readonly Color FileNodeForeColor = Color.Black;

        public List<TreeNode> CategoryNodes => GetNodesAtLevel(LevelType.Categories);

        public List<TreeNode> FileNodes => GetNodesAtLevel(LevelType.Files);

        public List<TreeNode> ModNodes => GetNodesAtLevel(LevelType.Mods);

        protected TreeNode ClickedNode = null;
        protected bool IsUpdating = false;

        Color _clickedNodeForeColor;

        #endregion

        #region Double-buffering

        // From http://stackoverflow.com/a/10364283/1641069
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }

        #endregion

        #region Context Menu Members

        protected TreeNode RightClickedNode;

        ContextMenuStrip _contextMenu;

        protected ToolStripRegion ContextOpenRegion;
        ToolStripMenuItem _contextOpenModFile = new ToolStripMenuItem();
        ToolStripMenuItem _contextOpenModFileDir = new ToolStripMenuItem();
        ToolStripMenuItem _contextOpenModBundleDir = new ToolStripMenuItem();
        ToolStripMenuItem _contextCopyPath = new ToolStripMenuItem();

        protected ToolStripRegion ContextNodeRegion;

        protected ToolStripRegion ContextAllRegion;
        ToolStripSeparator _contextAllSeparator = new ToolStripSeparator();
        ToolStripMenuItem _contextExpandAll = new ToolStripMenuItem();
        ToolStripMenuItem _contextCollapseAll = new ToolStripMenuItem();
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
            if (ClickedNode != null)
            {
                if (!ClickedNode.Bounds.Contains(e.Location))
                    ClickedNode = null;
                else if (e.Button == MouseButtons.Left)
                {
                    _clickedNodeForeColor = ClickedNode.ForeColor;
                    ClickedNode.ForeColor = Color.White;
                    ClickedNode.BackColor = Color.CornflowerBlue;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                BeginUpdate();
                IsUpdating = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ClickedNode == null || RightClickedNode != null || e.Button == MouseButtons.Right)
                return;
            if (ClickedNode.Bounds.Contains(e.Location))
            {
                ClickedNode.BackColor = Color.CornflowerBlue;
                ClickedNode.ForeColor = Color.White;
            }
            else
            {
                ClickedNode.ForeColor = _clickedNodeForeColor;
                ClickedNode.BackColor = Color.Transparent;
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
            {
                OnLeftMouseUp(e);
                if (lastClicked != null && ClickedNode != null)
                {
                    lastClicked.ForeColor = _clickedNodeForeColor;
                    lastClicked.BackColor = Color.Transparent;
                }
                ClickedNode = null;
            }
            else if (e.Button == MouseButtons.Right)
                OnRightMouseUp(e);
            EndUpdate();
            IsUpdating = false;
        }

        protected virtual void OnLeftMouseUp(MouseEventArgs e)
        {
            if (ClickedNode == null)
                return;
            if (ClickedNode.SetCheckedIfVisible(!ClickedNode.Checked))
                HandleCheckedChange();
        }

        protected virtual void OnRightMouseUp(MouseEventArgs e)
        {
            ResetContextItemAvailability();
            SetContextItemAvailability();

            if (_contextMenu.Items.OfType<ToolStripMenuItem>().Any(item => item.Available))
            {
                if (ClickedNode != null)
                    ClickedNode.BackColor = Color.Gainsboro;

                SetContextMenuSize();

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

        protected LevelType GetLevelType(TreeNode node) => (LevelType)node.Level;

        protected bool IsCategoryNode(TreeNode node) => (GetLevelType(node) == LevelType.Categories);

        protected bool IsFileNode(TreeNode node) => (GetLevelType(node) == LevelType.Files);

        protected bool IsModNode(TreeNode node) => (GetLevelType(node) == LevelType.Mods);

        #region Context Menu

        void InitializeContextMenu()
        {
            _contextMenu = new ContextMenuStrip();

            ContextOpenRegion = new ToolStripRegion(_contextMenu as ToolStrip, new ToolStripItem[]
            {
                _contextCopyPath,
                _contextOpenModFile,
                _contextOpenModFileDir,
                _contextOpenModBundleDir
            });

            ContextNodeRegion = new ToolStripRegion(_contextMenu as ToolStrip, new ToolStripItem[0]);

            ContextAllRegion = new ToolStripRegion(_contextMenu as ToolStrip, new ToolStripItem[]
            {
                _contextAllSeparator,
                ContextSelectAll,
                ContextDeselectAll,
                _contextExpandAll,
                _contextCollapseAll
            });

            // treeContextMenu
            _contextMenu.AutoSize = false;
            _contextMenu.Name = "treeContextMenu";
            _contextMenu.Size = new Size(239, 390);
            _contextMenu.Closing += ContextMenu_Closing;

            // contextOpenModFile
            _contextOpenModFile.Name = "contextOpenModFile";
            _contextOpenModFile.Size = new Size(225, 22);
            _contextOpenModFile.Text = "Open Mod File";
            _contextOpenModFile.ToolTipText = "Opens this mod's version of the file";
            _contextOpenModFile.Click += ContextOpenFile_Click;

            // contextOpenModFileDir
            _contextOpenModFileDir.Name = "contextOpenModFileDir";
            _contextOpenModFileDir.Size = new Size(225, 22);
            _contextOpenModFileDir.Text = "Open Mod File Directory";
            _contextOpenModFileDir.ToolTipText = "Opens the location of this mod's version of the file";
            _contextOpenModFileDir.Click += ContextOpenDirectory_Click;

            // contextOpenModBundleDir
            _contextOpenModBundleDir.Name = "contextOpenModBundleDir";
            _contextOpenModBundleDir.Size = new Size(225, 22);
            _contextOpenModBundleDir.Text = "Open Mod Bundle Directory";
            _contextOpenModBundleDir.ToolTipText = "Opens the location of this mod's bundle file";
            _contextOpenModBundleDir.Click += ContextOpenDirectory_Click;

            // contextCopyPath
            _contextCopyPath.Name = "contextCopyPath";
            _contextCopyPath.Size = new Size(225, 22);
            _contextCopyPath.Text = "Copy Path";
            _contextCopyPath.Click += ContextCopyPath_Click;

            // contextAllSeparator
            _contextAllSeparator.Name = "contextAllSeparator";
            _contextAllSeparator.Size = new Size(235, 6);

            // contextSelectAll
            ContextSelectAll.Name = "contextSelectAll";
            ContextSelectAll.Size = new Size(225, 22);
            ContextSelectAll.Text = "Select All";
            ContextSelectAll.Click += ContextSelectAll_Click;

            // contextDeselectAll
            ContextDeselectAll.Name = "contextDeselectAll";
            ContextDeselectAll.Size = new Size(225, 22);
            ContextDeselectAll.Text = "Deselect All";
            ContextDeselectAll.Click += ContextDeselectAll_Click;

            // contextExpandAll
            _contextExpandAll.Name = "contextExpandAll";
            _contextExpandAll.Size = new Size(225, 22);
            _contextExpandAll.Text = "Expand All";
            _contextExpandAll.Click += ContextExpandAll_Click;

            // contextCollapseAll
            _contextCollapseAll.Name = "contextCollapseAll";
            _contextCollapseAll.Size = new Size(225, 22);
            _contextCollapseAll.Text = "Collapse All";
            _contextCollapseAll.Click += ContextCollapseAll_Click;
        }

        protected void BuildContextMenu()
        {
            _contextMenu.Items.Clear();
            _contextMenu.Items.AddRange(ContextOpenRegion.Items);
            _contextMenu.Items.AddRange(ContextNodeRegion.Items);
            _contextMenu.Items.AddRange(ContextAllRegion.Items);
        }

        void ResetContextItemAvailability()
        {
            foreach (var menuItem in _contextMenu.Items.OfType<ToolStripItem>())
                menuItem.Available = false;
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
                    if (ModFile.IsBundle(filePath))
                        _contextOpenModBundleDir.Available = true;
                    else
                        _contextOpenModFile.Available = _contextOpenModFileDir.Available = true;
                }
            }

            if (ClickedNode == null && !this.IsEmpty())
            {
                _contextExpandAll.Available =
                    CategoryNodes.Any(catNode => !catNode.IsExpanded)
                    || FileNodes.Any(fileNode => !fileNode.IsExpanded);

                _contextCollapseAll.Available = CategoryNodes.Any(node => node.IsExpanded);

                _contextAllSeparator.Visible =
                    (_contextExpandAll.Available || _contextCollapseAll.Available)
                    && (ContextOpenRegion.Available || ContextNodeRegion.Available);
            }
        }

        void SetContextMenuSize()
        {
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

        protected void ContextOpenFile_Click(object sender, EventArgs e)
        {
            if (RightClickedNode == null)
                return;

            string filePath = RightClickedNode.Tag as string;
            if (!File.Exists(filePath))
                Program.MainForm.ShowMessage("Can't find file: " + filePath);
            else
                Process.Start(filePath);

            RightClickedNode = null;
        }

        protected void ContextOpenDirectory_Click(object sender, EventArgs e)
        {
            if (RightClickedNode == null)
                return;

            var dirPath = Path.GetDirectoryName(RightClickedNode.Tag as string);
            if (!Directory.Exists(dirPath))
                Program.MainForm.ShowMessage("Can't find directory: " + dirPath);
            else
                Process.Start(dirPath);

            RightClickedNode = null;
        }

        void ContextCopyPath_Click(object sender, EventArgs e)
        {
            if (RightClickedNode == null)
                return;

            Clipboard.SetText(RightClickedNode.Tag as string);

            RightClickedNode = null;
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

        void ContextExpandAll_Click(object sender, EventArgs e)
        {
            ExpandAll();
        }

        void ContextCollapseAll_Click(object sender, EventArgs e)
        {
            CollapseAll();
        }

        void ContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (ClickedNode == null)
                return;
            ClickedNode.BackColor = Color.Transparent;
            ClickedNode.TreeView.Update();

            RightClickedNode = ClickedNode;  // Preserve reference to clicked node so context item handlers can access,
            ClickedNode = null;              // but clear ClickedNode so mouseover doesn't change back color.
        }

        #endregion
    }
}