using GoogleDiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public static class Extensions
    {
        public static bool AreOnlyEqualOrEmpty(this IEnumerable<Diff> diffs)
        {
            return diffs.All(d =>
                d.Operation == Operation.EQUAL ||
                string.IsNullOrEmpty(d.Text));
        }

        public static bool AreOnlyWhitespace(this IEnumerable<Diff> diffs)
        {
            return diffs.All(d =>
                d.Operation == Operation.EQUAL ||
                string.IsNullOrWhiteSpace(d.Text));
        }

        public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable, IComparable<T>
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static int NextLineBreak(this string s, int startIndex)
        {
            if (s.Substring(startIndex, Environment.NewLine.Length) == Environment.NewLine)
                ++startIndex;
            int nextIndex = s.IndexOf(Environment.NewLine, startIndex);
            if (nextIndex < s.Length)
                return nextIndex;
            else
                return s.Length - 1;
        }

        public static int PreviousLineBreak(this string s, int startIndex)
        {
            if (s.Substring(startIndex, Environment.NewLine.Length) == Environment.NewLine)
                --startIndex;
            int prevIndex = s.LastIndexOf(Environment.NewLine, startIndex);
            if (prevIndex > 0)
                return prevIndex;
            else
                return 0;
        }

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

        public static int CountChar(this RichTextBox rtb, char countChar, int startIndex, int endIndex)
        {
            return rtb.Text.CountChar(countChar, startIndex, endIndex);
        }

        public static int CountChar(this string text, char countChar, int startIndex = 0, int? endIndex = null)
        {
            if (!endIndex.HasValue)
                endIndex = text.Length - 1;
            int count = 0;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                if (text[i] == countChar)
                    ++count;
            }
            return count;
        }
    }
}
