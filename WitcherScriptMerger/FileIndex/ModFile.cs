using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
                else if (IsXml(RelativePath))
                    return Categories.Xml;
                else if (BundleName != null)
                {
                    if (IsTextFile(RelativePath))
                        return Categories.BundleText;
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
            else if (Category == Categories.Xml)
                return Path.Combine(Paths.GameDirectory, RelativePath);
            else
                throw new Exception($"Can't get vanilla file for category '{Category.DisplayName}'.");
        }

        public string GetModFile(string modName)
        {
            if (Category == Categories.Script)
                return Path.Combine(Paths.ModsDirectory, modName, Paths.ModScriptBase, RelativePath);
            else if (Category == Categories.Xml)
                return Path.Combine(Paths.ModsDirectory, modName, RelativePath);
            else if (Category.IsBundled)
                return Path.Combine(Paths.ModsDirectory, modName, Paths.BundleBase, BundleName);
            else
                throw new NotImplementedException();
        }

        public static string GetModNameFromPath(string modFilePath)
        {
            if (!modFilePath.StartsWith(Paths.ModsDirectory))  // Merged bundle content has internal path, not derived from mod folder
                return Paths.MergedBundleContent;

            int nameStart = Paths.ModsDirectory.Length + 1;
            string name = modFilePath.Substring(nameStart);
            return name.Substring(0, name.IndexOf('\\'));
        }

        public static bool IsScript(string path) => path.EndsWith(".ws");

        public static bool IsXml(string path) => path.EndsWith(".xml");

        public static bool IsFlatFile(string path) => (IsScript(path) || IsXml(path));

        public static bool IsBundle(string path) => path.EndsWith(".bundle");

        public static bool IsTextFile(string path) => (path.EndsWith(".ws") || path.EndsWith(".xml") || path.EndsWith(".txt") || path.EndsWith(".csv"));

        public override string ToString()
        {
            return $"({ModNames.Count} mod{ModNames.Count.GetPluralS()}) {RelativePath}";
        }
    }
}