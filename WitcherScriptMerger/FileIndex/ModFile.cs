using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WitcherScriptMerger.FileIndex
{
    public enum ModFileType
    {
        Script, BundleContent
    }

    public class ModFile
    {
        [XmlElement]
        public string RelativePath { get; set; }

        [XmlElement(ElementName = "IncludedMod")]
        public List<string> ModNames { get; private set; }

        [XmlIgnore]
        public string BundleName { get; set; }

        [XmlIgnore]
        public ModFileType Type { get; private set; }

        [XmlIgnore]
        public bool HasConflict
        {
            get { return ModNames.Count > 1; }
        }

        public ModFile(string relPath, string bundlePath = null)
        {
            RelativePath = relPath;
            ModNames = new List<string>();
            if (bundlePath != null)
            {
                BundleName = Path.GetFileName(bundlePath);
                Type = ModFileType.BundleContent;
            }
            else
                Type = ModFileType.Script;
        }

        public ModFile()
        {
            ModNames = new List<string>();
        }

        public string GetVanillaFile()
        {
            if (Type == ModFileType.Script)
                return Path.Combine(Paths.ScriptsDirectory, RelativePath);
            else
                return Path.Combine(Paths.BundlesDirectory, BundleName);
        }

        public string GetModFile(string modName)
        {
            if (Type == ModFileType.Script)
                return Path.Combine(Paths.ModsDirectory, modName, Paths.ModScriptBase, RelativePath);
            else
                return Path.Combine(Paths.ModsDirectory, modName, Paths.ModBundleBase, BundleName);
        }
    }
}
