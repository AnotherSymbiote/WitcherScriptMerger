using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    internal static class Extensions
    {
        #region Strings

        public static string ReplaceIgnoreCase(this string s, string oldValue, string newValue)
        {
            return Regex.Replace(s, Regex.Escape(oldValue), newValue.Replace("$", "$$"), RegexOptions.IgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string s, string otherString)
        {
            return s.Equals(otherString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int IndexOfIgnoreCase(this string s, string value, int startIndex = 0)
        {
            return s.IndexOf(value, startIndex, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int LastIndexOfIgnoreCase(this string s, string value, int startIndex = -1)
        {
            if (startIndex == -1)
                startIndex = s.Length - 1;
            return s.LastIndexOf(value, startIndex, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this string s, string value)
        {
            return s.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EndsWithIgnoreCase(this string s, string value)
        {
            return s.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsAlphaNumeric(this string s)
        {
            return new Regex("^[_a-zA-Z0-9]*$").IsMatch(s);
        }

        public static string GetPluralS(this int num)
        {
            return num == 1 ? "" : "s";
        }

        #endregion

        #region Tree & Context Menu

        public static IEnumerable<ToolStripItem> GetAvailableItems(this ContextMenuStrip menu)
        {
            return menu.Items.Cast<ToolStripItem>().Where(item => item.Available);
        }

        public static void SetFontStyle(this TreeNode node, FontStyle style)
        {
            var currFont = node.NodeFont ?? Control.DefaultFont;
            node.NodeFont = new Font(currFont, style);
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>();
        }

        public static Controls.SMTree.NodeMetadata GetMetadata(this TreeNode node)
        {
            return node.Tag as Controls.SMTree.NodeMetadata;
        }

        public static bool IsEmpty(this TreeView tree)
        {
            return (tree.Nodes.Count == 0);
        }

        #endregion

        #region Scrolling TreeView to Top

        const int WM_VSCROLL = 0x0115;
        const int SB_THUMBPOSITION = 0x0004;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static void ScrollToTop(this TreeView treeView)
        {
            if (SetScrollPos(treeView.Handle, WM_VSCROLL, 0, true) != -1)
                PostMessage(treeView.Handle, WM_VSCROLL, SB_THUMBPOSITION, 0);
        }

        #endregion

        #region TreeView Checkbox Visibility

        // From http://stackoverflow.com/a/22488652/1641069

        const int TVIF_STATE = 0x8;
        const int TVIS_STATEIMAGEMASK = 0xF000;
        const int TV_FIRST = 0x1100;
        const int TVM_GETITEM = TV_FIRST + 62;
        const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        /// <summary>
        /// Gets a value indicating if the checkbox is visible on the tree node.
        /// </summary>
        /// <param name="node">The tree node.</param>
        /// <returns><value>true</value> if the checkbox is visible on the tree node; otherwise <value>false</value>.</returns>
        public static bool IsCheckBoxVisible(this TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (node.TreeView == null)
                throw new InvalidOperationException("The node does not belong to a tree.");
            var tvi = new TVITEM
            {
                hItem = node.Handle,
                mask = TVIF_STATE
            };
            var result = SendMessage(node.TreeView.Handle, TVM_GETITEM, node.Handle, ref tvi);
            if (result == IntPtr.Zero)
                throw new ApplicationException("Error getting TreeNode state.");
            var imageIndex = (tvi.state & TVIS_STATEIMAGEMASK) >> 12;
            return (imageIndex != 0);
        }

        /// <summary>
        /// Sets a value indicating if the checkbox is visible on the tree node.
        /// </summary>
        /// <param name="node">The tree node.</param>
        /// <param name="isVisible"><value>true</value> to make the checkbox visible on the tree node; otherwise <value>false</value>.</param>
        public static void SetIsCheckBoxVisible(this TreeNode node, bool isVisible, bool applyToSubtree = false)
        {
            if (node.TreeView == null)
                throw new InvalidOperationException("The node does not belong to a tree.");
            var tvi = new TVITEM
            {
                hItem = node.Handle,
                mask = TVIF_STATE,
                stateMask = TVIS_STATEIMAGEMASK,
                state = (isVisible ? node.Checked ? 2 : 1 : 0) << 12
            };
            var result = SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
            if (result == IntPtr.Zero)
                throw new ApplicationException("Error setting TreeNode state.");

            if (applyToSubtree)
            {
                foreach (var childNode in node.GetTreeNodes())
                {
                    childNode.SetIsCheckBoxVisible(isVisible, applyToSubtree);
                }
            }
        }

        public static bool SetCheckedIfVisible(this TreeNode node, bool isChecked)
        {
            if (node.IsCheckBoxVisible())
            {
                node.Checked = isChecked;
                return true;
            }
            return false;
        }

        public static bool AreAllVisibleCheckboxesChecked(this TreeNode node)
        {
            return node.GetTreeNodes()
                .Where(child => child.IsCheckBoxVisible())
                .All(child => child.Checked);
        }

        #endregion
    }
}
