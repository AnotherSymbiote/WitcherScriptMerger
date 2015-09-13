using System.Collections.Generic;
using System.IO;

namespace WitcherScriptMerger.FileIndex
{
    public enum ModFileType
    {
        Script, BundleContent
    }

    public class ModFile
    {
        public string RelativePath { get; set; }
        public List<string> ModNames { get; private set; }
        public ModFileType Type { get; private set; }
        public FileInfo Info { get; private set; }

        public bool HasConflict
        {
            get { return ModNames.Count > 1; }
        }

        public ModFile(string relPath, string fullPath = null)
        {
            RelativePath = relPath;
            ModNames = new List<string>();
            if (fullPath != null)
            {
                Type = ModFileType.Script;
                Info = new FileInfo(fullPath);
            }
            else
            {
                Type = ModFileType.BundleContent;
                Info = null;
            }
        }
    }
}
