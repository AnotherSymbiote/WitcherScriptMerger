using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WitcherScriptMerger
{
    public struct ModDirectory
    {
        public DirectoryInfo Root;
        public IEnumerable<FileInfo> ScriptFiles;

        public string ModName
        {
            get { return Root.Name; }
        }

        public ModDirectory(string rootPath)
        {
            Root = new DirectoryInfo(rootPath);
            ScriptFiles = Directory.GetFiles(Root.FullName, "*.ws", SearchOption.AllDirectories)
                .Select(path => new FileInfo(path));
        }

        public static string GetModName(FileInfo modFile)
        {
            int nameEnd = modFile.FullName.IndexOfIgnoreCase(Path.Combine("content", "scripts")) - 1;
            string name = modFile.FullName.Substring(0, nameEnd);
            return name.Substring(name.LastIndexOf('\\') + 1);
        }

        public static string GetRelativePath(FileInfo modFile, string modsDir, bool leadingSeparator = true)
        {
            string relPath = modFile.FullName.ReplaceIgnoreCase(modsDir , "");  // Trim mod directory.
            relPath = relPath.Substring(1);                                     // Trim backslash.
            relPath = relPath.Substring(relPath.IndexOf('\\'));                 // Trim mod name.
            if (!leadingSeparator)                                              // Trim backslash.
                relPath = relPath.Substring(1);
            return relPath;
        }
    }
}
