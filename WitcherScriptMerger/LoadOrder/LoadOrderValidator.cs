using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger.LoadOrder
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

            var mergedModName = Paths.RetrieveMergedModName();
            var mergedMod = loadOrder.Mods.Find(m => m.ModName.EqualsIgnoreCase(mergedModName));

            if (mergedMod != null && mergedMod == loadOrder.GetTopPriorityEnabledMod())
                return;

            var choice = PromptToPrioritizeMergedMod();
            if (choice == DialogResult.Yes)
            {
                PrioritizeMergedMod(loadOrder, mergedMod);
            }
            else if (choice == DialogResult.Cancel)  // Never
            {
                Program.MainForm.ValidateCustomLoadOrderSetting = false;
            }
        }

        DialogResult PromptToPrioritizeMergedMod()
        {
            MessageBoxManager.Cancel = "Ne&ver";
            MessageBoxManager.Register();

            var choice = MessageBox.Show(
                $"{_modsSettingsPath}\n\n" +
                "Detected custom load order in the file above, and merged files aren't configured to load first.\n\n" +
                "Would you like Script Merger to modify your custom load order so that your merged files have top priority?",
                "Custom Load Order Problem",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            MessageBoxManager.Unregister();
            return choice;
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
    }
}
