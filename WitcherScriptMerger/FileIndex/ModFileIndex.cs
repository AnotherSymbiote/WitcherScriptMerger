using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.FileIndex
{
    public class ModFileIndex
    {
        public List<ModFile> Files;

        public IEnumerable<ModFile> Conflicts
        {
            get { return Files.Where(f => f.HasConflict); }
        }

        public bool HasConflict
        {
            get { return Files.Any(f => f.HasConflict); }
        }

        public ModFileIndex()
        {
            Files = new List<ModFile>();
        }

        public ModFileIndex Build(MergeInventory inventory)
        {
            var ignoredModNames = GetIgnoredModNames();
            var modDirPaths = Directory.GetDirectories(Program.MainForm.ModsDirectory, "mod*", SearchOption.TopDirectoryOnly)
                .Where(path => !ignoredModNames.Any(name => name.EqualsIgnoreCase(Path.GetDirectoryName(path))))
                .ToList();
            if (!modDirPaths.Any())
            {
                MessageBox.Show("Can't find any mods in the Mods directory.");
                return null;
            }

            foreach (var dirPath in modDirPaths)
            {
                var filePaths = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
                var scriptPaths = filePaths.Where(path => path.EndsWith(".ws"));
                var bundlePaths = filePaths.Where(path => path.EndsWith(".bundle"));
                var contentPaths = GetBundleContentPaths(bundlePaths);
                string modName = Path.GetFileName(dirPath);
                Files.AddRange(GetModFiles(scriptPaths, inventory, modName));
                Files.AddRange(GetModFiles(contentPaths, inventory, modName));
            }
            return this;
        }

        private List<ModFile> GetModFiles(IEnumerable<string> filePaths, MergeInventory inventory, string modName)
        {
            var fileList = new List<ModFile>();
            foreach (var filePath in filePaths)
            {
                string relPath = (Path.IsPathRooted(filePath)
                    ? ModHelpers.GetMinimalRelativePath(filePath)
                    : filePath);

                if (IsConflictResolved(inventory, modName, relPath))
                    continue;

                var existingFile = Files.FirstOrDefault(c =>
                    c.RelativePath.EqualsIgnoreCase(relPath));
                if (existingFile == null)
                {
                    ////string vanillaPath = Path.Combine(ScriptsDirectory, relPath);
                    ////if (File.Exists(vanillaPath))
                    ////{
                    var newFile = new ModFile(relPath, filePath);
                    newFile.ModNames.Add(modName);
                    fileList.Add(newFile);
                    ////}
                }
                else
                    existingFile.ModNames.Add(modName);
            }
            return fileList;
        }

        private List<string> GetBundleContentPaths(IEnumerable<string> bundlePaths)
        {
            var contentPaths = new List<string>();

            string argBase = string.Format("-l {0} ", Program.BmsPluginPath);
            var procInfo = new ProcessStartInfo
            {
                FileName = Program.BmsPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            foreach (var bundlePath in bundlePaths)
            {
                procInfo.Arguments = string.Format("{0} \"{1}\"", argBase, bundlePath);
                using (var bmsProc = new Process { StartInfo = procInfo })
                {
                    bmsProc.Start();
                    string output = bmsProc.StandardOutput.ReadToEnd() + "\n\n" + bmsProc.StandardError.ReadToEnd();
                    bmsProc.WaitForExit();
                    int footerPos = output.LastIndexOf("QuickBMS generic");
                    var outputLines = output.Substring(0, footerPos).Split('\n');
                    var lineParts = outputLines.SelectMany(line => line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
                    var paths = lineParts.Where(part => part.Contains('.'));
                    contentPaths.AddRange(paths);
                }
            }
            return contentPaths;
        }

        private bool IsConflictResolved(MergeInventory inventory, string modName, string relPath)
        {
            return inventory.MergedScripts.Any(ms =>
                ms.RelativePath == relPath &&
                ms.IncludedMods.Contains(modName) &&
                ms.MergedModName.CompareTo(modName) <= 0);
        }

        private IEnumerable<string> GetIgnoredModNames()
        {
            string ignoredNames = Program.Settings.Get("IgnoreModNames");
            return ignoredNames.Split(',')
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim());
        }
    }
}
