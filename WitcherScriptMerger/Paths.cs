using System.IO;

namespace WitcherScriptMerger
{
    internal static class Paths
    {
        public static string Kdiff3 = Program.Settings.Get("KDiff3Path");
        public static string Bms = Program.Settings.Get("QuickBmsPath");
        public static string BmsPlugin = Path.Combine(Path.GetDirectoryName(Bms), "witcher3.bms");

        public const string Inventory = "MergeInventory.xml";
        public static string ModScriptBase = Path.Combine("content", "scripts");
        public static string ModBundleBase = "content";
        public static string VanillaScriptBase = Path.Combine("content", "content0", "scripts");
        public static string VanillaBundleBase = Path.Combine("content", "content0", "bundles");

        public static string GameDirectory
        {
            get { return Program.MainForm.GetGameDirectory(); }
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

        private static string _bundlesDirSetting = Program.Settings.Get("BundlesDirectory");
        public static string BundlesDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_bundlesDirSetting))
                    return _bundlesDirSetting;
                return Path.Combine(GameDirectory, VanillaBundleBase);
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

        public static bool IsBundlesDirectoryDerived
        {
            get { return string.IsNullOrWhiteSpace(_bundlesDirSetting); }
        }

        public static bool IsModsDirectoryDerived
        {
            get { return string.IsNullOrWhiteSpace(_modsDirSetting); }
        }
    }
}
