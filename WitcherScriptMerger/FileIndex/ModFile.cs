using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.FileIndex
{
    public class ModFile
    {
        #region Members

        [XmlElement]
        public string RelativePath { get; set; }

        [XmlElement(ElementName = "IncludedMod")]
        public List<string> ModNames { get; private set; }

        [XmlElement]
        public string BundleName { get; set; }

        [XmlIgnore]
        public ModFileCategory Category
        {
            get
            {
                if (IsScript(RelativePath))
                    return Categories.Script;
                else if (BundleName != null)
                {
                    if (IsXml(RelativePath))
                        return Categories.BundleXml;
                    else
                        return Categories.BundleUnsupported;
                }
                else
                    return Categories.OtherUnsupported;
            }
        }

        [XmlIgnore]
        public bool IsBundleContent => (BundleName != null);

        [XmlIgnore]
        public bool HasConflict => (ModNames.Count > 1);

        #endregion

        public ModFile(string relPath, string bundlePath = null)
        {
            RelativePath = relPath;
            ModNames = new List<string>();
            if (bundlePath != null)
                BundleName = Path.GetFileName(bundlePath);
        }

        public ModFile()
        {
            ModNames = new List<string>();
        }

        public string GetVanillaFile()
        {
            if (Category == Categories.Script)
                return Path.Combine(Paths.ScriptsDirectory, RelativePath);
            else
                throw new System.Exception("Can only get vanilla file for scripts.");
        }

        public string GetModFile(string modName)
        {
            if (Category == Categories.Script)
                return Path.Combine(Paths.ModsDirectory, modName, Paths.ModScriptBase, RelativePath);
            else
                return Path.Combine(Paths.ModsDirectory, modName, Paths.BundleBase, BundleName);
        }

        public static string GetModNameFromPath(string modFilePath)
        {
            if (IsXml(modFilePath))           // Merged bundle content isn't in a mod folder
                return Paths.MergedBundleContent;
            string nameStart = (IsScript(modFilePath) ? Paths.ModScriptBase : Paths.BundleBase);
            int nameEnd = modFilePath.IndexOfIgnoreCase(nameStart) - 1;
            string name = modFilePath.Substring(0, nameEnd);
            return name.Substring(name.LastIndexOf('\\') + 1);
        }

        public static bool IsScript(string path) => path.EndsWith(".ws");

        public static bool IsBundle(string path) => path.EndsWith(".bundle");

        public static bool IsXml(string path) => path.EndsWith(".xml");

        public static bool IsMergeable(string path) => (IsScript(path) || IsXml(path));

        public static int GetLoadOrder(string modName1, string modName2)
        {
            // The game loads numbers first, then underscores, then letters (upper or lower).
            // ASCII (ordinal) order is numbers, then uppercase letters, then underscores, then lowercase.
            // To achieve the game's load order, we can convert uppercase letters to lowercase, then take ASCII order.

            return string.Compare(modName1.ToLowerInvariant(), modName2.ToLowerInvariant(), System.StringComparison.Ordinal);
        }
    }
}