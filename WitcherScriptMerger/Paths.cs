using System.IO;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    internal static class Paths
    {
        public static string Kdiff3 = Program.Settings.Get("KDiff3Path");
        public static string Bms = Program.Settings.Get("QuickBmsPath");
        public static string BmsPlugin = Path.Combine(Path.GetDirectoryName(Bms), "witcher3.bms");
        public static string WccLite = Program.Settings.Get("WccLitePath");

        public const string TempBundleContent = "tempbundlecontent";
        public static string MergedBundleContent = "Merged Bundle Content";
        public const string Inventory = "MergeInventory.xml";
        public static string ModScriptBase = Path.Combine("content", "scripts");
        public static string VanillaScriptBase = Path.Combine("content", "content0", "scripts");
        public static string BundleBase = "content";

        public static string GameDirectory
        {
            get { return Program.MainForm.GameDirectorySetting; }
        }

        public static string BundlesDirectory
        {
            get { return Path.Combine(GameDirectory, BundleBase); }
        }

        private static string _scriptsDirSetting = Program.Settings.Get("ScriptsDirectory");
        public static string ScriptsDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_scriptsDirSetting))
                    return _scriptsDirSetting;
                return Path.Combine(GameDirectory, VanillaScriptBase);
            }
        }

        private static string _modsDirSetting = Program.Settings.Get("ModsDirectory");
        public static string ModsDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_modsDirSetting))
                    return _modsDirSetting;
                return Path.Combine(GameDirectory, "Mods");
            }
        }

        public static bool IsScriptsDirectoryDerived
        {
            get { return string.IsNullOrWhiteSpace(_scriptsDirSetting); }
        }

        public static bool IsModsDirectoryDerived
        {
            get { return string.IsNullOrWhiteSpace(_modsDirSetting); }
        }

        public static string GetRelativePath(string fullPath, string basePath)
        {
            int startIndex = fullPath.IndexOfIgnoreCase(basePath) + basePath.Length + 1;
            return fullPath.Substring(startIndex);
        }

        public static bool ValidateModsDirectory()
        {
            if (!Directory.Exists(ModsDirectory))
            {
                Program.MainForm.ShowMessage(
                    (!IsModsDirectoryDerived
                     ? "Can't find the Mods directory specified in the config file."
                     : "Can't find Mods directory in the specified game directory."));
                return false;
            }
            return true;
        }

        public static bool ValidateScriptsDirectory()
        {
            if (!Directory.Exists(ScriptsDirectory))
            {
                Program.MainForm.ShowMessage(
                    (!IsScriptsDirectoryDerived
                     ? "Can't find the Scripts directory specified in the config file."
                     : "Can't find \\content\\content0\\scripts directory in the specified game directory.") +
                    "\n\nIt was added in patch 1.08.1 and should contain the game's vanilla scripts. If you don't have it, try this workaround:\n\n" +
                    "1) Open the folder where this .exe is located.\n" +
                    "2) Go to '\\Tools\\wcc_lite\\r4data'.\n" +
                    "3) Copy the 'scripts' folder to " + Path.Combine(GameDirectory, "content\\content0") + ".");
                return false;
            }
            return true;
        }

        public static bool ValidateBundlesDirectory()
        {
            if (!Directory.Exists(BundlesDirectory))
            {
                Program.MainForm.ShowMessage("Can't find 'content' directory in the specified game directory.");
                return false;
            }
            return true;
        }

        public static string RetrieveMergedBundlePath()
        {
            string mergedModName = RetrieveMergedModName();
            if (mergedModName != null)
                return Path.Combine(ModsDirectory, mergedModName, BundleBase, "blob0.bundle");
            else
                return null;
        }

        public static string RetrieveMergedModName()
        {
            string mergedModName = Program.Settings.Get("MergedModName");
            if (string.IsNullOrWhiteSpace(mergedModName))
            {
                Program.MainForm.ShowMessage("The MergedModName setting isn't configured in the .config file.");
                return null;
            }
            if (mergedModName.Length > 64)
                mergedModName = mergedModName.Substring(0, 64);
            if (!mergedModName.IsAlphaNumeric() || !mergedModName.StartsWith("mod"))
            {
                if (!ConfirmInvalidModName(mergedModName))
                    return null;
            }
            return mergedModName;
        }

        private static bool ConfirmInvalidModName(string mergedModName)
        {
            return (DialogResult.Yes == Program.MainForm.ShowMessage(
                "The Witcher 3 won't load the merged file if the mod name isn't \"mod\" followed by numbers, letters, or underscores.\n\nUse this name anyway?\n" + mergedModName
                + "\n\nTo change the name: Click No, then edit \"MergedModName\" in the .config file.",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }
    }
}