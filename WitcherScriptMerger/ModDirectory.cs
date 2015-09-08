using System.Collections.Generic;
using System.IO;
using System.Linq;
using WitcherScriptMerger.Forms;

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

        public static string GetModName(string modFilePath)
        {
            int nameEnd = modFilePath.IndexOfIgnoreCase(MainForm.ModScriptBase) - 1;
            string name = modFilePath.Substring(0, nameEnd);
            return name.Substring(name.LastIndexOf('\\') + 1);
        }

        public static string GetModName(FileInfo modFile)
        {
            return GetModName(modFile.FullName);
        }

        public static string GetRelativePath(string modFilePath, bool includeModName, bool includeLeadingSeparator = true)
        {
            int startIndex = modFilePath.IndexOfIgnoreCase(MainForm.ModScriptBase) - 1;
            if (includeModName)
                startIndex = modFilePath.LastIndexOfIgnoreCase("\\", startIndex - 1);
            string relPath = modFilePath.Substring(startIndex);
            if (!includeLeadingSeparator)
                relPath = relPath.Substring(1);  // Trim backslash.
            return relPath;
        }

        public static string GetMinimalRelativePath(string scriptPath)
        {
            string query = MainForm.ModScriptBase;
            int startIndex = scriptPath.IndexOfIgnoreCase(query) + query.Length + 1;
            return scriptPath.Substring(startIndex);
        }
    }
}
