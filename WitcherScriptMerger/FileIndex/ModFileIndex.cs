using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.FileIndex
{
    internal class ModFileIndex
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
            var modDirPaths = Directory.GetDirectories(Paths.ModsDirectory, "mod*", SearchOption.TopDirectoryOnly)
                .Where(path => !ignoredModNames.Any(name => name.EqualsIgnoreCase(Path.GetDirectoryName(path))))
                .ToList();
            if (!modDirPaths.Any())
            {
                MessageBox.Show("Can't find any mods in the Mods directory.");
                return null;
            }

            foreach (var dirPath in modDirPaths)
            {
                string modName = Path.GetFileName(dirPath);
                var filePaths = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
                var scriptPaths = filePaths.Where(path => path.EndsWith(".ws"));
                Files.AddRange(GetModFilesFromPaths(scriptPaths, inventory, modName));

                foreach (string bundlePath in filePaths.Where(path => path.EndsWith(".bundle")))
                {
                    var contentPaths = GetBundleContentPaths(bundlePath);
                    Files.AddRange(GetModFilesFromPaths(contentPaths, inventory, modName, bundlePath));
                }
            }
            return this;
        }

        private List<ModFile> GetModFilesFromPaths(IEnumerable<string> filePaths, MergeInventory inventory, string modName, string bundlePath = null)
        {
            var fileList = new List<ModFile>();
            foreach (var filePath in filePaths)
            {
                string relPath = (Path.IsPathRooted(filePath)
                    ? Paths.GetRelativePath(filePath, Paths.ModScriptBase)
                    : filePath);

                if (inventory.HasResolvedConflict(relPath, modName))
                    continue;

                var existingFile = Files.FirstOrDefault(file =>
                    file.RelativePath.EqualsIgnoreCase(relPath));
                if (existingFile == null)
                {
                    var newFile = (bundlePath != null
                        ? new ModFile(relPath, bundlePath)
                        : new ModFile(relPath));
                    newFile.ModNames.Add(modName);
                    fileList.Add(newFile);
                }
                else
                    existingFile.ModNames.Add(modName);
            }
            return fileList;
        }

        private List<string> GetBundleContentPaths(string bundlePath)
        {
            var contentPaths = new List<string>();

            var procInfo = new ProcessStartInfo
            {
                FileName = Paths.Bms,
                Arguments = string.Format("-l {0} \"{1}\"", Paths.BmsPlugin, bundlePath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using (var bmsProc = new Process { StartInfo = procInfo })
            {
                bmsProc.Start();
                string output = bmsProc.StandardOutput.ReadToEnd() + "\n\n" + bmsProc.StandardError.ReadToEnd();
                bmsProc.WaitForExit();
                int footerPos = output.LastIndexOf("QuickBMS generic");
                var outputLines = output.Substring(0, footerPos).Split('\n');
                var paths = outputLines
                    .Where(line => line.Length > 5)
                    .Select(line => line.Substring(line.LastIndexOf(' ')).Trim());
                contentPaths.AddRange(paths);
            }
            return contentPaths;
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
