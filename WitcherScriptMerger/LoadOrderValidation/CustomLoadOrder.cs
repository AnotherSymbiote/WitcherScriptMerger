using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WitcherScriptMerger.LoadOrderValidation
{
    class CustomLoadOrder
    {
        public const int MinPriority = 1;
        public const int MaxPriority = 999;

        public List<ModLoadSetting> Mods = new List<ModLoadSetting>();
        
        public CustomLoadOrder(string modSettingsFilePath)
        {
            ModLoadSetting currModSetting = null;

            var lines = 
                File.ReadAllLines(modSettingsFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim());

            if (!lines.Any())
                return;

            foreach (string line in lines)
            {
                if (line.StartsWith("["))
                {
                    if (currModSetting != null)
                    {
                        Mods.Add(currModSetting);
                    }
                    string modName = line.Substring(1, line.Length - 2);  // Trim brackets
                    currModSetting = new ModLoadSetting(modName);
                }
                else if (line.StartsWith("Enabled"))
                {
                    currModSetting.IsEnabled = line.EndsWith("1");
                }
                else if (line.StartsWith("Priority"))
                {
                    string priorityString = line.Substring(line.IndexOf('=') + 1);
                    currModSetting.Priority = int.Parse(priorityString);
                }
            }
            if (currModSetting != null)
                Mods.Add(currModSetting); // Final item

            Mods.OrderBy(m => m.Priority)
                .ThenBy(m => m.ModName);
        }

        public void Write(string filePath)
        {
            var builder = new StringBuilder();

            foreach (var modSetting in Mods)
            {
                builder
                    .Append("[").Append(modSetting.ModName).AppendLine("]")
                    .Append("Enabled=").AppendLine(Convert.ToInt32(modSetting.IsEnabled).ToString())
                    .Append("Priority=").AppendLine(modSetting.Priority.ToString())
                    .AppendLine();
            }

            File.WriteAllText(filePath, builder.ToString());
        }

        public ModLoadSetting GetTopPriorityEnabledMod()
        {
            return Mods
                .OrderBy(setting => setting, new LoadOrderComparer())
                .FirstOrDefault();
        }

        public string GetTopPriorityEnabledMod(IEnumerable<string> conflictingMods)
        {
            var conflictingModSettings =
                Mods.Where(loadSetting =>
                    conflictingMods.Any(modName =>
                        modName.EqualsIgnoreCase(loadSetting.ModName)));

            if (!conflictingModSettings.Any())
                return null;

            return conflictingModSettings
                .OrderBy(setting => setting, new LoadOrderComparer())
                .FirstOrDefault()
                ?.ModName;
        }
    }
}
