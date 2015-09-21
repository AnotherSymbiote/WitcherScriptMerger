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
            if (Type == ModFileType.Script)
                return Path.Combine(Paths.ModsDirectory, MergedModName, Paths.ModScriptBase, RelativePath);
            else
                return Path.Combine(Paths.MergedBundleContent, RelativePath);
        }

        public string GetMergedBundle()
        {
            if (Type == ModFileType.Script)
                throw new System.Exception("Can't get bundle for ModFile of type Script.");

            return Path.Combine(Paths.ModsDirectory, MergedModName, Paths.BundleBase, BundleName);
        }
    }
}