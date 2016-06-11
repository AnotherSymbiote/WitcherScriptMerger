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

            if (!loadOrder.Mods.Any())
                return;

            var mergedMod = loadOrder.Mods.Find(m => m.ModName == Paths.RetrieveMergedModName());

            bool isMergedModTopPriority =
                mergedMod != null &&
                mergedMod.IsEnabled &&
                !loadOrder.Mods.Any(m =>
                    m != mergedMod &&
                    m.IsEnabled &&
                    m.Priority < mergedMod.Priority);

            if (!isMergedModTopPriority && DialogResult.Yes == PromptToPrioritizeMergedMod())
            {
                PrioritizeMergedMod(loadOrder, mergedMod);
            }
        }

        DialogResult PromptToPrioritizeMergedMod()
        {
            return MessageBox.Show(
                $"{_modsSettingsPath}\n\n" +
                "Detected custom load order in the file above, and merged files aren't configured to load first.\n\n" +
                "Would you like Script Merger to modify your custom load order so that your merged files have top priority?",
                "Custom Load Order Problem",
                MessageBoxButtons.YesNo);
        }

        void PrioritizeMergedMod(CustomLoadOrder loadOrder, ModLoadSetting mergedModSetting)
        {
            // Priority 0 will be incremented to 1
            if (mergedModSetting != null)
            {
                mergedModSetting.Priority = 0;
                mergedModSetting.IsEnabled = true;
            }
            else
            {
                loadOrder.Mods.Insert(0, new ModLoadSetting
                {
                    ModName = Paths.RetrieveMergedModName(),
                    IsEnabled = true,
                    Priority = 0
                });
            }

            IncrementLeadingContiguousPriorities(loadOrder, 0);

            loadOrder.Write(_modsSettingsPath);
        }

        void IncrementLeadingContiguousPriorities(CustomLoadOrder loadOrder, int startingPriority)
        {
            int nextPriority = startingPriority + 1;
            var modsToIncrement = loadOrder.Mods.Where(mod => mod.Priority == startingPriority).ToArray();
            var displacedMods = loadOrder.Mods.Where(mod => mod.Priority == nextPriority).ToArray();

            if (!modsToIncrement.Any())
                return;

            if (displacedMods.Any() &&
                nextPriority < CustomLoadOrder.MaxPriority)
            {
                IncrementLeadingContiguousPriorities(loadOrder, nextPriority);
            }

            foreach (var mod in modsToIncrement)
                ++mod.Priority;
        }

        public static int GetModNameLoadOrder(string name1, string name2)
        {
            // The game loads numbers first, then underscores, then letters (upper or lower).
            // ASCII (ordinal) order is numbers, then uppercase letters, then underscores, then lowercase.
            // To achieve the game's load order, we can convert uppercase letters to lowercase, then take ASCII order.

            return string.Compare(name1.ToLowerInvariant(), name2.ToLowerInvariant(), StringComparison.Ordinal);
        }
    }
}
