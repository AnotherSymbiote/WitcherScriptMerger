using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.Forms;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger
{
    internal class FileMerger
    {
        private MergeInventory _inventory;
        private FileInfo _vanillaFile;
        private FileInfo _file1;
        private FileInfo _file2;
        private string _modName1;
        private string _modName2;
        private int _mergesToDo;
        private string _outputPath;

        public FileMerger(MergeInventory inventory)
        {
            _inventory = inventory;
        }

        public void MergeByTreeNodes(IEnumerable<TreeNode> nodesToMerge, string mergedModName)
        {
            foreach (var fileNode in nodesToMerge)
            {
                bool isScript = ModFile.IsScriptPath(fileNode.Text);

                var modNodes = fileNode.GetTreeNodes().Where(modNode => modNode.Checked).ToList();

                if (modNodes.Any(node => mergedModName.CompareTo(node.Text) > 0) &&
                    !ConfirmRemainingConflict(mergedModName))
                    continue;

                _file1 = new FileInfo(modNodes[0].Tag as string);
                _modName1 = ModFile.GetModNameFromPath(_file1.FullName);

                // ### NEED TO CHANGE FOR BUNDLE OUTPUT PATH
                string relPath = Paths.GetRelativePath(
                    _file1.FullName,
                    Path.Combine(Paths.ModsDirectory, ModFile.GetModNameFromPath(_file1.FullName)));

                _outputPath = (isScript
                    ? Path.Combine(Paths.ModsDirectory, mergedModName, relPath)
                    : Path.Combine(Paths.MergedBundleContent, fileNode.Text));

                if (File.Exists(_outputPath) && !ConfirmOutputOverwrite(_outputPath))
                    continue;

                if (ModFile.IsScriptPath(fileNode.Text))
                    _vanillaFile = new FileInfo(fileNode.Tag as string);
                else
                    _vanillaFile = null;
                _mergesToDo = modNodes.Count - 1;

                bool isNew = false;
                var merge = _inventory.Merges.FirstOrDefault(ms => ms.RelativePath == fileNode.Text);
                if (merge == null)
                {
                    isNew = true;
                    merge = new Merge
                    {
                        RelativePath = fileNode.Text,
                        MergedModName = mergedModName,
                    };
                }

                for (int i = 1; i < modNodes.Count; ++i)
                {
                    _file2 = new FileInfo(modNodes[i].Tag as string);
                    _modName2 = ModFile.GetModNameFromPath(_file2.FullName);

                    if (_vanillaFile == null)
                        GetUnpackedFiles(fileNode.Text);
                    var mergedFile = MergeText(i, merge);
                    if (mergedFile != null)
                        _file1 = mergedFile;
                    else
                        HandleCanceledMerge(i, merge, nodesToMerge);
                }
                if (isNew && merge.ModNames.Count > 1)
                    _inventory.Merges.Add(merge);
            }
        }

        private FileInfo MergeText(int mergeNum, Merge mergedFile)
        {
            string outputDir = Path.GetDirectoryName(_outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string args = string.Format(
                "\"{0}\" \"{1}\" \"{2}\" -o \"{3}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\"",
                _vanillaFile.FullName, _file1.FullName, _file2.FullName, _outputPath);

            if (!Program.MainForm.PathsInKdiff3Setting)
                args += string.Format(" --L1 Vanilla --L2 \"{0}\" --L3 \"{1}\"", _modName1, _modName2);

            if (!Program.MainForm.ReviewEachMergeSetting)
                args += " --auto";

            string kdiff3Path = (Path.IsPathRooted(Paths.Kdiff3)
                ? Paths.Kdiff3
                : Path.Combine(Environment.CurrentDirectory, Paths.Kdiff3));

            var kdiff3Proc = Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            if (kdiff3Proc.ExitCode == 0)
            {
                if (_file1.FullName != _outputPath)
                    mergedFile.ModNames.Add(_modName1);

                if (_file2.FullName != _outputPath)
                    mergedFile.ModNames.Add(_modName2);

                if (Program.MainForm.MergeReportSetting)
                {
                    using (var reportForm = new ReportForm(
                        mergeNum, _mergesToDo,
                        _file1.FullName, _file2.FullName, _outputPath,
                        _modName1, _modName2))
                    {
                        reportForm.ShowDialog();
                    }
                }
                return new FileInfo(_outputPath);
            }
            else
                return null;
        }

        private bool ConfirmRemainingConflict(string mergedModName)
        {
            return (DialogResult.Yes == MessageBox.Show(
                "There will still be a conflict if you use the merged mod name " + mergedModName + ".\n\n" +
                    "The Witcher 3 loads mods in alphabetical order, so this merged mod name will load after one of the original mods and the merged file will be ignored.\n\n" +
                    "Use this name anyway?",
                "Merged Mod Name Conflict",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        private bool ConfirmOutputOverwrite(string outputPath)
        {
            return (DialogResult.Yes == MessageBox.Show(
                "The output file below already exists! Overwrite?\n\n" + outputPath,
                "Overwrite?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        private DialogResult HandleCanceledMerge(int mergeNum, Merge merge, IEnumerable<TreeNode> nodesToMerge)
        {
            string msg = string.Format("Merge was canceled for {0}.", _vanillaFile.Name);
            var buttons = MessageBoxButtons.OK;
            if (_mergesToDo > 1 || nodesToMerge.Count() > 1)
            {
                if (_mergesToDo > 1)
                {
                    msg = string.Format("Merge {0} of {1} was canceled for {2}.", mergeNum, _mergesToDo, _vanillaFile.Name);
                    if (mergeNum < _mergesToDo)
                    {
                        msg += "\n\nContinue with the remaining merges for this file?";
                        buttons = MessageBoxButtons.YesNo;
                    }
                }
            }
            Program.MainForm.Activate(); // Focus window
            var result = MessageBox.Show(msg, "Skipped Merge", buttons, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                return DialogResult.Abort;
            }
            return DialogResult.OK;
        }

        private bool GetUnpackedFiles(string contentRelativePath)
        {
            _vanillaFile = null;
            var bundleDirs = Directory.GetDirectories(Path.Combine(Paths.GameDirectory, "content"))
                .Select(path => Path.Combine(path, "bundles"))
                .Where(path => Directory.Exists(path))
                .ToList();
            for (int i = bundleDirs.Count - 1; i >= 0; --i)  // Search vanilla directories in reverse
            {                                                // order, as patches override content.
                var bundleFiles = Directory.GetFiles(bundleDirs[i], "*.bundle");
                foreach (var bundle in bundleFiles)  
                {
                    var contentPaths = ModFileIndex.GetBundleContentPaths(bundle);
                    if (contentPaths.Any(path => path.EqualsIgnoreCase(contentRelativePath)))
                    {
                        _vanillaFile = new FileInfo(bundle);
                        break;
                    }
                }
                if (_vanillaFile != null)
                    break;
            }
            if (_vanillaFile == null)
            {
                if (PromptForManualMerge(contentRelativePath, "Couldn't find this file in any vanilla bundles."))
                    throw new NotImplementedException();
                else
                    return false;
            }

            string vanillaContentFile = UnpackFile(_vanillaFile.FullName, contentRelativePath, "Vanilla");
            string modContentFile1 = UnpackFile(_file1.FullName, contentRelativePath, "Mod 1");
            string modContentFile2 = UnpackFile(_file2.FullName, contentRelativePath, "Mod 2");
            _vanillaFile = (vanillaContentFile != null
                ? new FileInfo(vanillaContentFile)
                : null);
            _file1 = (modContentFile1 != null
                ? new FileInfo(modContentFile1)
                : null);
            _file2 = (modContentFile2 != null
                ? new FileInfo(modContentFile2)
                : null);

            return (vanillaContentFile != null && modContentFile1 != null && modContentFile2 != null);
        }

        private bool PromptForManualMerge(string path, string reason)
        {
            return DialogResult.Yes == MessageBox.Show(
                path + "\n\nCan't auto-merge. " + reason + "\n\nDo a manual 2-way merge?",
                "No Vanilla Version",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
        }

        private string UnpackFile(string bundlePath, string contentRelativePath, string outputDirName)
        {
            string outputDir = Path.Combine(Paths.TempBundleContent, outputDirName);
            var procInfo = new ProcessStartInfo
            {
                FileName = Paths.Bms,
                Arguments = string.Format("-Y -f \"{0}\" \"{1}\" \"{2}\" \"{3}\"",
                    contentRelativePath,
                    Paths.BmsPlugin,
                    bundlePath,
                    outputDir),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using (var bmsProc = new Process { StartInfo = procInfo })
            {
                bmsProc.Start();
                string output = bmsProc.StandardOutput.ReadToEnd() + "\n\n" + bmsProc.StandardError.ReadToEnd();
                
                // ### ADD SUCCESS/ERROR CHECK
                ////if (output.LastIndexOf(""))

                return Path.Combine(outputDir, contentRelativePath);
            }
        }
    }
}