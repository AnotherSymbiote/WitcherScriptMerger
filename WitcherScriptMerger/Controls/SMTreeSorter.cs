using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Controls
{
    public class SMTreeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var xNode = x as TreeNode;
            var yNode = y as TreeNode;

            switch (xNode.Level)
            {
                case (int)SMTree.LevelType.Categories:
                    var xCat = (ModFileCategory)xNode.Tag;
                    var yCat = (ModFileCategory)yNode.Tag;
                    return xCat.OrderIndex.CompareTo(yCat.OrderIndex);
                default:
                    return xNode.Text.CompareTo(yNode.Text);
            }
        }
    }
}
