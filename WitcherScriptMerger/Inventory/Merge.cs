using System;
using System.IO;
using System.Xml.Serialization;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class Merge : ModFile
    {
        [XmlElement]
        public string MergedModName;

        public string GetMergedFile()
        {
            if (Category == Categories.Script)
                return Path.Combine(Paths.ModsDirectory, MergedModName, Paths.ModScriptBase, RelativePath);
            else if (Category == Categories.Xml)
                return Path.Combine(Paths.ModsDirectory, MergedModName, RelativePath);
            else if (Category == Categories.BundleText)
                return Path.Combine(Paths.MergedBundleContent, RelativePath);
            else
                throw new NotImplementedException();
        }

        public string GetMergedBundle()
        {
            if (Category != Categories.BundleText)
                throw new Exception($"Can't get bundle for file of category '{Category.DisplayName}'.");

            return Path.Combine(Paths.ModsDirectory, MergedModName, Paths.BundleBase, BundleName);
        }
    }
}