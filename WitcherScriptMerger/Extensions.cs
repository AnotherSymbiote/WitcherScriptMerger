using System;
using System.Collections.Generic;
using System.Linq;
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
            return new Regex("^[a-zA-Z0-9]*$").IsMatch(s);
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeView treeView)
        {
            return treeView.Nodes.Cast<TreeNode>();
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>();
        }
    }
}
