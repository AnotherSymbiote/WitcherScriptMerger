using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WitcherScriptMerger.LoadOrder
{
    class CustomLoadOrder
    {
        public const int TopPriority = 0;
        public const int BottomPriority = 9999;

        public readonly string FilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "The Witcher 3",
                "mods.settings");

        public List<ModLoadSetting> Mods { get; private set; }

        public bool IsValid { get; private set; }
        
        public CustomLoadOrder()
        {
            Refresh();
        }

        #region File Processing

        public void Refresh()
        {
            Mods = new List<ModLoadSetting>();
            IsValid = false;

            if (!File.Exists(FilePath))
            {
                IsValid = true;
                return;
            }

            var lines = File.ReadAllLines(FilePath);

            List<ModLoadSetting> mods = new List<ModLoadSetting>();
            ModLoadSetting currModSetting = null;

            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (!ProcessModNameLine(line, ref currModSetting))
                        return;
                }
                else if (line.StartsWith("Enabled="))
                {
                    if (!ProcessIsEnabledLine(line, i + 1, currModSetting))
                        return;
                }
                else if (line.StartsWith("Priority="))
                {
                    if (!ProcessPriorityLine(line, i + 1, currModSetting))
                        return;
                }
                else if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(";"))
                {
                    ShowWarningForMalformedFile($"Unrecognized value on line {i + 1}:\n\n{line}");
                    return;
                }

                if (currModSetting != null
                    && currModSetting.IsEnabled.HasValue
                    && currModSetting.Priority.HasValue)
                {
                    mods.Add(currModSetting);
                    currModSetting = null;
                }
            }

            IsValid = true;

            Mods = mods
                .OrderBy(m => m.Priority)
                .ThenBy(m => m.ModName)
                .ToList();
        }

        bool ProcessModNameLine(string line, ref ModLoadSetting setting)
        {
            if (setting != null)
            {
                ShowWarningForMalformedFile($"{setting.ModName} settings are incomplete.  'Enabled' and 'Priority' are both required.");
                return false;
            }

            string modName = line.Substring(1, line.Length - 2);  // Trim brackets
            setting = new ModLoadSetting(modName);
            return true;
        }

        bool ProcessIsEnabledLine(string line, int lineNum, ModLoadSetting setting)
        {
            if (setting == null)
            {
                ShowWarningForMalformedFile($"The 'Enabled' setting on line {lineNum} doesn't have a corresponding mod name.");
                return false;
            }
            if (!new Regex("^Enabled=[0|1]$").IsMatch(line))
            {
                ShowWarningForMalformedFile($"The 'Enabled' setting on line {lineNum} isn't within the valid range of 0 or 1:\n\n{line}");
                return false;
            }

            setting.IsEnabled = line.EndsWith("1");
            return true;
        }

        bool ProcessPriorityLine(string line, int lineNum, ModLoadSetting setting)
        {
            if (setting == null)
            {
                ShowWarningForMalformedFile($"The 'Priority' setting on line {lineNum} doesn't have a corresponding mod name.");
                return false;
            }

            string priorityString = line.Substring(line.IndexOf('=') + 1);
            int parsedPriority;

            if (!int.TryParse(priorityString, out parsedPriority))
            {
                ShowWarningForMalformedFile($"Can't parse the priority on line {lineNum}:\n\n{line}");
                return false;
            }
            if (TopPriority > parsedPriority || parsedPriority > BottomPriority)
            {
                ShowWarningForMalformedFile($"The priority on line {lineNum} isn't within the valid range of {TopPriority} to {BottomPriority}:\n\n{line}");
                return false;
            }

            setting.Priority = parsedPriority;
            return true;
        }

        void ShowWarningForMalformedFile(string reason)
        {
            Program.MainForm.ShowMessage(
                "Your mods.settings file is invalid.\n\n" + reason,
                "Invalid Load Order File",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Warning);
        }

        public void Save()
        {
            var builder = new StringBuilder();

            foreach (var modSetting in Mods)
            {
                builder
                    .Append("[").Append(modSetting.ModName).AppendLine("]")
                    .Append("Enabled=").AppendLine(Convert.ToInt32(modSetting.IsEnabled).ToString())
                    .Append("Priority=").AppendLine(modSetting.Priority.ToString());

                if (modSetting != Mods.Last())
                    builder.AppendLine();
            }

            File.WriteAllText(FilePath, builder.ToString());
        }

        #endregion

        public void AddMergedModIfMissing()
        {
            var mergedModName = Paths.RetrieveMergedModName();

            if (!Mods.Any(setting => setting.ModName.EqualsIgnoreCase(mergedModName)))
            {
                Mods.Insert(0,
                    new ModLoadSetting
                    {
                        ModName = mergedModName,
                        IsEnabled = true,
                        Priority = TopPriority
                    });
            }
        }

        public bool HasResolvedConflict(IEnumerable<string> modNames)
        {
            var loadSettings = modNames
                .Select(GetModLoadSettingByName)
                .Where(setting => setting != null);

            if (!loadSettings.Any())
                return false;

            if (loadSettings.Any(setting => setting.IsEnabled.Value))
                return true;

            int numSettings = loadSettings.Count();
            int numMods = modNames.Count();

            return (numSettings >= numMods - 1);
        }

        public bool Contains(string modName)
        {
            return Mods.Any(setting => setting.ModName.EqualsIgnoreCase(modName));
        }

        public ModLoadSetting GetTopPriorityEnabledMod()
        {
            return Mods
                .OrderBy(setting => setting, new LoadOrderComparer())
                .FirstOrDefault();
        }

        public string GetTopPriorityEnabledMod(IEnumerable<string> conflictMods)
        {
            var conflictModSettings = Mods.Where(setting => conflictMods.Any(modName => modName.EqualsIgnoreCase(setting.ModName)));
            var enabledModSettings = conflictModSettings.Where(setting => setting.IsEnabled.Value);

            if (!conflictModSettings.Any())
                return conflictMods
                    .OrderBy(name => name, new LoadOrderComparer())
                    .FirstOrDefault();

            if (!enabledModSettings.Any())
                return conflictMods
                    .Except(conflictModSettings.Select(setting => setting.ModName))
                    .OrderBy(name => name, new LoadOrderComparer())
                    .FirstOrDefault();

            return enabledModSettings
                .OrderBy(setting => setting, new LoadOrderComparer())
                .ThenBy(setting => setting.ModName, new LoadOrderComparer())
                .FirstOrDefault()
                ?.ModName;
        }

        public ModLoadSetting GetModLoadSettingByName(string modName)
        {
            return Mods.FirstOrDefault(setting => setting.ModName.EqualsIgnoreCase(modName));
        }

        public bool IsModDisabledByName(string modName)
        {
            var mod = GetModLoadSettingByName(modName);

            return (mod != null && !mod.IsEnabled.Value);
        }

        public void ToggleModByName(string modName)
        {
            var mod = GetModLoadSettingByName(modName);

            if (mod != null)
                mod.IsEnabled = !mod.IsEnabled;
            else
            {
                Mods.Add(new ModLoadSetting
                {
                    ModName = modName,
                    IsEnabled = false,
                    Priority = BottomPriority
                });
            }
        }

        public int GetPriorityByName(string modName)
        {
            var mod = GetModLoadSettingByName(modName);

            return
                mod != null
                ? mod.Priority.Value
                : -1;
        }

        public void SetPriorityByName(string modName, int priority)
        {
            var mod = GetModLoadSettingByName(modName);

            if (mod != null)
            {
                mod.Priority = priority;
            }
            else
            {
                Mods.Add(new ModLoadSetting
                {
                    ModName = modName,
                    IsEnabled = true,
                    Priority = priority
                });
            }
        }
    }
}
