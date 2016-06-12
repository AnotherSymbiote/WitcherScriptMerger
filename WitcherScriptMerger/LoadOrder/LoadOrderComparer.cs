using System;
using System.Collections.Generic;

namespace WitcherScriptMerger.LoadOrder
{
    class LoadOrderComparer : IComparer<string>, IComparer<ModLoadSetting>
    {
        public int Compare(string x, string y)
        {
            // The game loads numbers first, then underscores, then letters (upper or lower).
            // ASCII (ordinal) order is numbers, then uppercase letters, then underscores, then lowercase.
            // To achieve the game's load order, we can convert uppercase letters to lowercase, then take ASCII order.
            return string.Compare(
                x.ToLowerInvariant(),
                y.ToLowerInvariant(),
                StringComparison.Ordinal);
        }

        public int Compare(ModLoadSetting x, ModLoadSetting y)
        {
            if (x.IsEnabled)
            {
                if (y.IsEnabled)
                    return x.Priority.CompareTo(y.Priority);
                else
                    return -1;  // Only x is enabled
            }
            else if (y.IsEnabled)
                return 1;  // Only y is enabled
            else
                return Compare(x.ModName, y.ModName);  // Neither is enabled
        }
    }
}
