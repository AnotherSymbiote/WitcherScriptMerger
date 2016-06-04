using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger.LoadOrderValidation
{
    class LoadOrderValidator
    {
        string _modsSettingsPath;

        public LoadOrderValidator()
        {
            var userDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _modsSettingsPath = Path.Combine(userDocsPath, "The Witcher 3", "mods.settings");
        }

        public void ValidateAndFix()
        {
            if (!File.Exists(_modsSettingsPath))
                return;

            var loadOrder = new CustomLoadOrder(_modsSettingsPath);

            string mergedModName = Paths.RetrieveMergedModName();
            var mergedMod = loadOrder.Mods.Find(m => m.ModName == mergedModName);

            bool isMergedModTopPriority =
                mergedMod != null &&
                mergedMod.IsEnabled &&
                !loadOrder.Mods.Any(m =>
                    m != mergedMod &&
                    m.IsEnabled &&
                    m.Priority < mergedMod.Priority);

            if (!isMergedModTopPriority)
            {
                string msg =
                    $"{_modsSettingsPath}\n\n" +
                    "Detected custom load order in the file above, and merged files aren't configured to load first.\n\n" +
                    "How would you like to fix this?\n\n" +
                    "Disable (preferred)\nDisables custom load order.  Script Merger is designed for the game's default load order based on mod folder names.  With this option, your mods.settings file will be renamed to mods.settings.backup (will overwrite existing backup, if any).\n\n" +
                    "Modify\nModifies custom load order to load merged files first.  With this option, only your merged files will be given priority 1.\n\n" +
                    "Do nothing\nLeaves custom load order unchanged.  With this option, the game may ignore your merged files.";
                MessageBoxManager.Abort = "&Disable";
                MessageBoxManager.Retry = "&Modify";
                MessageBoxManager.Ignore = "Do &nothing";
                MessageBoxManager.Register();
                var choice = MessageBox.Show(msg, "Custom Load Order Problem", MessageBoxButtons.AbortRetryIgnore);
                MessageBoxManager.Unregister();

                switch (choice)
                {
                    case DialogResult.Abort:
                        DisableCustomLoadOrder();
                        break;
                    case DialogResult.Retry:
                        PrioritizeMergedMod(loadOrder, mergedMod);
                        break;
                }
            }
        }

        void DisableCustomLoadOrder()
        {
            var backupPath = $"{_modsSettingsPath}.backup";
            if (File.Exists(backupPath))
                File.Delete(backupPath);
            File.Move(_modsSettingsPath, backupPath);
        }

        void PrioritizeMergedMod(CustomLoadOrder loadOrder, ModLoadSetting mergedModSetting)
        {
            mergedModSetting.Priority = 0;  // Will increment to 1

            IncrementLeadingContiguousPriorities(loadOrder, 0);

            loadOrder.Write(_modsSettingsPath);
        }

        void IncrementLeadingContiguousPriorities(CustomLoadOrder loadOrder, int startingPriority)
        {
            int nextPriority = startingPriority + 1;
            var modsToIncrement = loadOrder.Mods.Where(mod => mod.Priority == startingPriority).ToArray();
            var displacedMods = loadOrder.Mods.Where(mod => mod.Priority == nextPriority).ToArray();

            foreach (var mod in modsToIncrement)
                ++mod.Priority;

            if (displacedMods.Any() &&
                nextPriority < CustomLoadOrder.MaxPriority)
            {
                IncrementLeadingContiguousPriorities(loadOrder, nextPriority);
            }
        }

        public static int GetLoadOrder(string modName1, string modName2)
        {
            // The game loads numbers first, then underscores, then letters (upper or lower).
            // ASCII (ordinal) order is numbers, then uppercase letters, then underscores, then lowercase.
            // To achieve the game's load order, we can convert uppercase letters to lowercase, then take ASCII order.

            return string.Compare(modName1.ToLowerInvariant(), modName2.ToLowerInvariant(), System.StringComparison.Ordinal);
        }
    }
}
