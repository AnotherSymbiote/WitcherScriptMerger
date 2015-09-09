using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public static class Extensions
    {
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
            return s.IndexOf(value, 0, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int LastIndexOfIgnoreCase(this string s, string value, int startIndex = -1)
        {
            if (startIndex == -1)
                startIndex = s.Length - 1;
            return s.LastIndexOf(value, startIndex, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsAlphaNumeric(this string s)
        {
            return new Regex("^[_a-zA-Z0-9]*$").IsMatch(s);
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeView treeView)
        {
            return treeView.Nodes.Cast<TreeNode>();
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>();
        }

        #region Hiding TreeView Checkbox

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
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
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        public static void HideCheckBox(this TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        #endregion
    }
}
