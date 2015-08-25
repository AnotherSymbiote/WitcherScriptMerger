using GoogleDiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public static class MergeExtensions
    {
        // Combines two lists of patches into a single list, with all position data
        // corrected for any preceding patches from the opposite list.
        public static List<Patch> Combine(
            this List<Patch> patches1, List<Patch> patches2,
            string vanillaText, string text1, string text2,
            string fileName, string modName1, string modName2)
        {
            var q1 = new Queue<Patch>(patches1);
            var q2 = new Queue<Patch>(patches2);
            int deltaFor1From2 = 0;
            int deltaFor2From1 = 0;

            var combinedList = new List<Patch>();

            while (q1.Count > 0 || q2.Count > 0)  // While there are still patches to merge
            {
                var front1 = (q1.Any() ? q1.Peek() : null);
                var front2 = (q2.Any() ? q2.Peek() : null);
                var vanillaRange1 = new Range();
                var vanillaRange2 = new Range();

                if (front1 != null && front2 != null)
                {
                    // Find vanilla positions for both patches
                    vanillaRange1.Start = front1.GetVanillaStart(deltaFor2From1);
                    vanillaRange2.Start = front2.GetVanillaStart(deltaFor1From2);
                    vanillaRange1.End = vanillaRange1.Start + front1.Length1;
                    vanillaRange2.End = vanillaRange2.Start + front2.Length1;
                    
                    // If both patches are the same, just apply 1st one
                    if (vanillaRange1.Equals(vanillaRange2) &&
                        front1.Diffs.All(d1 => front2.Diffs.Any(d2 => d1.Operation == d2.Operation && d1.Text == d2.Text)))
                    {
                        // Adjust subsequent q2 patches for this discarded patch
                        deltaFor2From1 -= front2.GetDelta();
                        front2 = null;
                        q2.Dequeue();
                    }
                    else if (vanillaRange1.Start.IsBetween(vanillaRange2.Start, vanillaRange2.End) ||
                             vanillaRange1.End.IsBetween(vanillaRange2.Start, vanillaRange2.End))
                    {
                        // Handle overlapping patches manually
                        using (var conflictResolver = new ConflictResolver(fileName, modName1, modName2))
                        {
                            var vanillaRange = new Range(
                                Math.Min(vanillaRange1.Start, vanillaRange2.Start),
                                Math.Max(vanillaRange1.End, vanillaRange2.End));
                            var leftRange = new Range(front1.Start2, front1.Start2 + front1.Length2);
                            var rightRange = new Range(front2.Start2, front2.Start2 + front2.Length2);
                            conflictResolver.SetVanillaText(vanillaText, vanillaRange);
                            conflictResolver.SetLeftText(text1, leftRange);
                            conflictResolver.SetRightText(text2, rightRange);
                            var result = conflictResolver.ShowDialog();
                            if (result == (DialogResult)ConflictResolver.ConflictResolution.Vanilla ||
                                result == (DialogResult)ConflictResolver.ConflictResolution.Left)
                            {
                                // Adjust subsequent q2 patches for this discarded patch
                                deltaFor2From1 -= (front2.Length2 - front2.Length1);
                                front2 = null;
                                q2.Dequeue();
                            }
                            if (result == (DialogResult)ConflictResolver.ConflictResolution.Vanilla ||
                                result == (DialogResult)ConflictResolver.ConflictResolution.Right)
                            {
                                // Adjust subsequent q1 patches for this discarded patch
                                deltaFor1From2 -= (front1.Length2 - front1.Length1);
                                front1 = null;
                                q1.Dequeue();
                            }
                        }
                    }
                }
                
                if (front1 != null &&
                    (front2 == null || vanillaRange1.Start < vanillaRange2.Start))
                {
                    deltaFor2From1 += front1.GetDelta();

                    front1.Start1 += deltaFor1From2;
                    front1.Start2 += deltaFor1From2;

                    combinedList.Add(front1);
                    q1.Dequeue();
                }
                else
                {
                    deltaFor1From2 += front2.GetDelta();
                    
                    front2.Start1 += deltaFor2From1;
                    front2.Start2 += deltaFor2From1;
                    
                    combinedList.Add(front2);
                    q2.Dequeue();
                }
            }

            return combinedList;
        }

        public static int GetDelta(this Patch p)
        {
            return p.Length2 - p.Length1;
        }

        public static int GetVanillaStart(this Patch p, int delta)
        {
            return (p.Start1 - delta);
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
    }
    
    public static class ControlExtensions
    {
        public static IEnumerable<TreeNode> GetTreeNodes(this TreeView treeView)
        {
            return treeView.Nodes.Cast<TreeNode>();
        }

        public static IEnumerable<TreeNode> GetTreeNodes(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>();
        }

        // RichTextBox removes all carriage return characters (\r) when selecting text,
        // which invalidates character positions for the original text. This function
        // counts the number of line breaks in the text to determine how many carriage
        // returns were lost.
        public static int GetLineBreakCount(this RichTextBox rtb, int startIndex, int endIndex)
        {
            int count = 0;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                if (rtb.Text[i] == '\n')
                    ++count;
            }
            return count;
        }
    }
}
