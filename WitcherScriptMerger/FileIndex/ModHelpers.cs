using System.IO;
using WitcherScriptMerger.Forms;

namespace WitcherScriptMerger.FileIndex
{
    public static class ModHelpers
    {
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
