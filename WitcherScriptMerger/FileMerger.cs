using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        #region Members
        public MergeProgressInfo ProgressInfo { get; private set; }

        MergeInventory _inventory;
        TreeNode[] _checkedFileNodes;
        FileInfo _vanillaFile;
        FileInfo _file1;
        FileInfo _file2;
        string _modName1;
        string _modName2;
        string _mergedModName;
        string _outputPath;
        
        string _bundlePath;
        bool _bundleChanged;
        List<Merge> _pendingBundleMerges = new List<Merge>();

        BackgroundWorker _bgWorker;

        #endregion

        public FileMerger(
            MergeInventory inventory,
            ProgressChangedEventHandler progressHandler,
            RunWorkerCompletedEventHandler completedHandler)
        {
            _inventory = inventory;

            _bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            _bgWorker.ProgressChanged += progressHandler;
            ProgressInfo = new MergeProgressInfo();
            ProgressInfo.PropertyChanged += (sender, e) =>
            {
                _bgWorker.ReportProgress(0, ProgressInfo);
            };
            _bgWorker.RunWorkerCompleted += completedHandler;

            _bundlePath = Paths.RetrieveMergedBundlePath();
        }

        ~FileMerger()
        {
            if (_bgWorker != null)
                _bgWorker.Dispose();
        }

        public void MergeByTreeNodesAsync(
            IEnumerable<TreeNode> fileNodesToMerge,
            string mergedModName)
        {
            _bgWorker.DoWork += (sender, e) =>
            {
                _checkedFileNodes = fileNodesToMerge.ToArray();
                _mergedModName = mergedModName;

                var checkedModNodesForFile =
                    _checkedFileNodes.Select(
                        fileNode =>
                        fileNode.GetTreeNodes().Where(
                            modNode =>
                            modNode.Checked
                        ).ToArray()
                    ).ToArray();

                ProgressInfo.TotalMergeCount = checkedModNodesForFile.Sum(modNodes => modNodes.Length - 1);
                ProgressInfo.TotalFileCount = _checkedFileNodes.Length;

                for (int i = 0; i < _checkedFileNodes.Length; ++i)
                {
                    var fileNode = _checkedFileNodes[i];

                    ProgressInfo.CurrentFileName = Path.GetFileName(fileNode.Text);
                    ProgressInfo.CurrentFileNum = i + 1;

                    var checkedModNodes = checkedModNodesForFile[i];

                    ProgressInfo.CurrentAction = "Starting merge";

                    if (checkedModNodes.Any(node => ModFile.GetLoadOrder(node.Text, _mergedModName) < 0) &&
                        !ConfirmRemainingConflict(_mergedModName))
                        continue;

                    _file1 = new FileInfo(checkedModNodes[0].Tag as string);
                    _modName1 = ModFile.GetModNameFromPath(_file1.FullName);

                    bool isNew = false;
                    var merge = _inventory.Merges.FirstOrDefault(m => m.RelativePath.EqualsIgnoreCase(fileNode.Text));
                    if (merge == null)
                    {
                        isNew = true;
                        merge = new Merge
                        {
                            RelativePath = fileNode.Text,
                            MergedModName = _mergedModName,
                        };
                    }

                    if (ModFile.IsScript(fileNode.Text))
                        MergeScriptFileNode(fileNode, checkedModNodes, merge, isNew);
                    else
                        MergeBundleFileNode(fileNode, checkedModNodes, merge, isNew);
                }
                if (_bundleChanged)
                {
                    string newBundlePath = PackNewBundle(_bundlePath);
                    if (newBundlePath != null)
                    {
                        ProgressInfo.CurrentAction = "Adding bundle merge to inventory";
                        foreach (var bundleMerge in _pendingBundleMerges)
                            _inventory.Merges.Add(bundleMerge);

                        if (Program.MainForm.CompletionSoundsSetting)
                        {
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        if (Program.MainForm.PackReportSetting)
                        {
                            using (var reportForm = new PackReportForm(newBundlePath))
                            {
                                ProgressInfo.CurrentAction = "Showing pack report";
                                Program.MainForm.ShowModal(reportForm);
                            }
                        }
                    }
                }
                CleanUpTempFiles();
                CleanUpEmptyDirectories();
            };
            _bgWorker.RunWorkerAsync();
        }

        void MergeScriptFileNode(TreeNode scriptNode, TreeNode[] checkedModNodes, Merge merge, bool isNew)
        {
            string relPath = Paths.GetRelativePath(
                _file1.FullName,
                Path.Combine(Paths.ModsDirectory, _modName1));

            _outputPath = Path.Combine(Paths.ModsDirectory, _mergedModName, relPath);

            if (File.Exists(_outputPath) && !ConfirmOutputOverwrite(_outputPath))
                return;

            _vanillaFile = new FileInfo(scriptNode.Tag as string);

            for (int i = 1; i < checkedModNodes.Length; ++i)
            {
                ++ProgressInfo.CurrentMergeNum;

                _file2 = new FileInfo(checkedModNodes[i].Tag as string);
                _modName2 = ModFile.GetModNameFromPath(_file2.FullName);

                var mergedFile = MergeText(merge);
                if (mergedFile != null)
                {
                    _file1 = mergedFile;
                    _modName1 = ModFile.GetModNameFromPath(_file1.FullName);
                }
                else if (DialogResult.Abort == HandleCanceledMerge(checkedModNodes.Length - i - 1, merge))
                    break;
            }

            if (isNew && merge.ModNames.Count > 1)
            {
                ProgressInfo.CurrentAction = "Adding script merge to inventory";
                _inventory.Merges.Add(merge);
            }
        }

        void MergeBundleFileNode(TreeNode fileNode, TreeNode[] checkedModNodes, Merge merge, bool isNew)
        {
            _outputPath = Path.Combine(Paths.MergedBundleContent, fileNode.Text);

            if (File.Exists(_outputPath) && !ConfirmOutputOverwrite(_outputPath))
                return;

            _vanillaFile = null;

            for (int i = 1; i < checkedModNodes.Length; ++i)
            {
                ++ProgressInfo.CurrentMergeNum;

                _file2 = new FileInfo(checkedModNodes[i].Tag as string);
                _modName2 = ModFile.GetModNameFromPath(_file2.FullName);

                if (!GetUnpackedFiles(fileNode.Text))
                {
                    if (DialogResult.Abort != HandleCanceledMerge(checkedModNodes.Length - i - 1, merge))
                        continue;
                    break;
                }

                var mergedFile = MergeText(merge);
                if (mergedFile != null)
                {
                    _file1 = mergedFile;
                    _modName1 = ModFile.GetModNameFromPath(_file1.FullName);
                }
                else if (DialogResult.Abort == HandleCanceledMerge(checkedModNodes.Length - i - 1, merge))
                    break;
            }

            if (_bundlePath != null && isNew && merge.ModNames.Count > 1)
            {
                merge.BundleName = Path.GetFileName(_bundlePath);
                _bundleChanged = true;
                _pendingBundleMerges.Add(merge);
            }
        }

        FileInfo MergeText(Merge merge)
        {
            ProgressInfo.CurrentAction = $"Using KDiff3 to merge {_modName1} && {_modName2}";

            string outputDir = Path.GetDirectoryName(_outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            bool hasVanillaVersion = (_vanillaFile != null && _vanillaFile.Exists);

            string args = (hasVanillaVersion
                ? "\"" + _vanillaFile.FullName + "\" "
                : "");

            args +=
                $"\"{_file1.FullName}\" \"{_file2.FullName}\" " +
                $"-o \"{_outputPath}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\" " +
                "--cs \"CreateBakFiles=0\" " +
                "--cs \"LineEndStyle=1\" " +
                "--cs \"FollowFileLinks=1\" " +
                "--cs \"FollowDirLinks=1\"";

            if (!Program.MainForm.PathsInKdiff3Setting)
            {
                if (hasVanillaVersion)
                    args += $" --L1 Vanilla --L2 \"{_modName1}\" --L3 \"{_modName2}\"";
                else
                    args += $" --L1 \"{_modName1}\" --L2 \"{_modName2}\"";
            }

            if (!Program.MainForm.ReviewEachMergeSetting && hasVanillaVersion)
                args += " --auto";

            string kdiff3Path = (Path.IsPathRooted(Paths.KDiff3)
                ? Paths.KDiff3
                : Path.Combine(Environment.CurrentDirectory, Paths.KDiff3));

            var kdiff3Proc = Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            if (kdiff3Proc.ExitCode == 0)
            {
                if (!_file1.FullName.EqualsIgnoreCase(_outputPath)
                    && !_file1.FullName.Contains(Paths.MergedBundleContent))
                    _inventory.AddModToMerge(_modName1, merge);

                if (!_file2.FullName.EqualsIgnoreCase(_outputPath)
                    && !_file2.FullName.Contains(Paths.MergedBundleContent))
                    _inventory.AddModToMerge(_modName2, merge);

                if (Program.MainForm.CompletionSoundsSetting)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                }
                if (Program.MainForm.MergeReportSetting)
                {
                    using (var reportForm = new MergeReportForm(
                        ProgressInfo.CurrentMergeNum, ProgressInfo.TotalMergeCount,
                        _file1.FullName, _file2.FullName, _outputPath,
                        _modName1, _modName2))
                    {
                        ProgressInfo.CurrentAction = "Showing merge report";
                        Program.MainForm.ShowModal(reportForm);
                    }
                }
                return new FileInfo(_outputPath);
            }
            else
                return null;
        }

        bool ConfirmRemainingConflict(string mergedModName)
        {
            return (DialogResult.Yes == Program.MainForm.ShowMessage(
                "There will still be a conflict if you use the merged mod name " + mergedModName + ".\n\n" +
                    "The Witcher 3 loads mods in case-insensitive ASCII order, " +
                    "so this merged mod name will load after one of the original mods, " +
                    "and the merged file will be ignored.\n\n" +
                    "Use this name anyway?",
                "Merged Mod Name Conflict",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        bool ConfirmOutputOverwrite(string outputPath)
        {
            return (DialogResult.Yes == Program.MainForm.ShowMessage(
                "The output file below already exists! Overwrite?\n\n" + outputPath,
                "Overwrite?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        DialogResult HandleCanceledMerge(int remainingMergesForFile, Merge merge)
        {
            string msg = $"Merge {ProgressInfo.CurrentMergeNum} of {ProgressInfo.TotalMergeCount} was canceled.";
            var buttons = MessageBoxButtons.OK;
            if (remainingMergesForFile > 0)
            {
                string fileName = Path.GetFileName(merge.RelativePath);
                msg += $"\n\nContinue with {remainingMergesForFile} remaining merge{remainingMergesForFile.GetPluralS()} for file {fileName}?";
                buttons = MessageBoxButtons.YesNo;
            }
            var result = Program.MainForm.ShowMessage(msg, "Skipped Merge", buttons, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                ProgressInfo.CurrentMergeNum += remainingMergesForFile;
                return DialogResult.Abort;
            }
            return DialogResult.OK;
        }

        bool GetUnpackedFiles(string contentRelativePath)
        {
            if (_vanillaFile == null)
            {
                ProgressInfo.CurrentAction = "Searching for corresponding vanilla bundle";
                var bundleDirs = Directory.GetDirectories(Paths.BundlesDirectory)
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
                if (_vanillaFile != null)
                {
                    ProgressInfo.CurrentAction = "Unpacking vanilla bundle content file";
                    string vanillaContentFile = UnpackFile(_vanillaFile.FullName, contentRelativePath, "Vanilla");
                    _vanillaFile = new FileInfo(vanillaContentFile);
                }
            }

            if (ModFile.IsBundle(_file1.FullName))
            {
                ProgressInfo.CurrentAction = $"Unpacking bundle content file for {_modName1}";
                string modContentFile1 = UnpackFile(_file1.FullName, contentRelativePath, "Mod 1");
                if (modContentFile1 == null)
                    return false;
                _file1 = new FileInfo(modContentFile1);
            }
            ProgressInfo.CurrentAction = $"Unpacking bundle content file for {_modName2}";
            string modContentFile2 = UnpackFile(_file2.FullName, contentRelativePath, "Mod 2");
            if (modContentFile2 == null)
                return false;
            _file2 = new FileInfo(modContentFile2);
            return true;
        }

        string UnpackFile(string bundlePath, string contentRelativePath, string outputDirName)
        {
            if (!ValidateBmsResources(bundlePath))
                return null;

            string outputDir = Path.Combine(Paths.TempBundleContent, outputDirName);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var procInfo = new ProcessStartInfo
            {
                FileName = Paths.Bms,
                Arguments = $"-Y -f \"{contentRelativePath}\" \"{Paths.BmsPlugin}\" \"{bundlePath}\" \"{outputDir}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using (var bmsProc = new Process { StartInfo = procInfo })
            {
                bmsProc.Start();
                string output = bmsProc.StandardError.ReadToEnd();  // QuickBMS prints results to std error, even if successful

                if (output.Contains("- 0 files found"))
                {
                    string errorMsg = "Error unpacking bundle content file using QuickBMS.\nIts output is below.";
                    int outputStart = output.IndexOf("- filter string");
                    if (outputStart != -1)
                    {
                        output = output.Substring(outputStart);
                        errorMsg += "\n\n" + output;
                    }
                    ShowError(errorMsg);
                    return null;
                }

                return Path.Combine(outputDir, contentRelativePath);
            }
        }

        public void RepackBundleForDeleteAsync(string bundlePath)
        {
            if (_bgWorker.IsBusy)
                throw new Exception("BackgroundWorker can't run 2 tasks concurrently.");
            _bgWorker.DoWork += (sender, e) =>
            {
                string newBundlePath = PackNewBundle(bundlePath, true);
                if (newBundlePath == null)
                    return;

                if (Program.MainForm.CompletionSoundsSetting)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                }
                if (Program.MainForm.PackReportSetting)
                {
                    using (var reportForm = new PackReportForm(bundlePath))
                    {
                        Program.MainForm.ShowModal(reportForm);
                    }
                }
            };
            _bgWorker.RunWorkerAsync();
        }

        string PackNewBundle(string bundlePath, bool isRepack = false)
        {
            ProgressInfo.CurrentPhase = (!isRepack ? "Packing Bundle" : "Deleted Merge — Repacking Bundle");
            ProgressInfo.CurrentAction = "Packing merged content into new blob0.bundle";

            string contentDir = Path.Combine(Environment.CurrentDirectory, Paths.MergedBundleContent);
            
            if (!ValidateWccLiteResources(contentDir))
                return null;

            string outputDir = Path.GetDirectoryName(bundlePath);
            var procInfo = new ProcessStartInfo
            {
                FileName = Paths.WccLite,
                Arguments = $"pack -dir=\"{contentDir}\" -outdir=\"{outputDir}\"",
                WorkingDirectory = Path.GetDirectoryName(Paths.WccLite),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using (var wccLiteProc = new Process { StartInfo = procInfo })
            {
                wccLiteProc.Start();
                string stdOutput = wccLiteProc.StandardOutput.ReadToEnd().Trim();
                string stdError = wccLiteProc.StandardError.ReadToEnd().Trim();

                string errorMsg = null;
                if (!string.IsNullOrWhiteSpace(stdError))
                    errorMsg = stdError;
                else if (stdOutput.EndsWith("Wcc operation failed"))
                    errorMsg = stdOutput;
                if (errorMsg != null)
                {
                    ShowError("Error packing merged content into a new bundle using wcc_lite.\nIts error output is below.\n\n" + errorMsg);
                    return null;
                }
            }
            ProgressInfo.CurrentAction = "Generating metadata.store for new blob0.bundle";
            procInfo.Arguments = $"metadatastore -path=\"{outputDir}\"";
            using (var wccLiteProc = new Process { StartInfo = procInfo })
            {
                wccLiteProc.Start();
                string stdOutput = wccLiteProc.StandardOutput.ReadToEnd();
                string stdError = wccLiteProc.StandardError.ReadToEnd();

                string errorMsg = null;
                if (!string.IsNullOrWhiteSpace(stdError))
                    errorMsg = stdError;
                else if (stdOutput.Contains("wcc operation failed"))
                    errorMsg = stdOutput;
                if (errorMsg != null)
                {
                    ShowError("Error generating metadata.store for new merged bundle using wcc_lite.\nIts error output is below.\n\n" + errorMsg);
                    return null;
                }
            }
            return bundlePath;
        }

        bool ValidateBmsResources(string bundlePath)
        {
            if (!File.Exists(bundlePath))
            {
                ShowError("Can't find bundle file:\n\n" + bundlePath, "Missing Bundle");
                return false;
            }
            if (!File.Exists(Paths.Bms))
            {
                ShowError("Can't find QuickBMS at this location:\n\n" + Paths.Bms, "Missing QuickBMS");
                return false;
            }
            if (!File.Exists(Paths.BmsPlugin))
            {
                ShowError("Can't find QuickBMS plugin at this location:\n\n" + Paths.BmsPlugin, "Missing QuickBMS Plugin");
                return false;
            }
            return true;
        }

        bool ValidateWccLiteResources(string contentDir)
        {
            if (!Directory.Exists(contentDir))
            {
                ShowError("Can't find Merged Bundle Content directory:\n\n" + contentDir, "Missing Directory");
                return false;
            }
            if (!File.Exists(Paths.WccLite))
            {
                ShowError("Can't find wcc_lite at this location:\n\n" + Paths.WccLite, "Missing wcc_lite");
                return false;
            }
            return true;
        }

        void ShowError(string msg, string title = "Error")
        {
            Program.MainForm.ShowMessage(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void CleanUpTempFiles()
        {
            if (!Directory.Exists(Paths.TempBundleContent))
                return;

            try
            {
                ProgressInfo.CurrentAction = "Deleting temporary unpacked bundle content";
                DeleteDirectory(Paths.TempBundleContent);
            }
            catch (Exception ex)
            {
                Program.MainForm.ShowMessage(
                    "Non-critical error: Failed to delete temporary unpacked bundle content.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        void CleanUpEmptyDirectories()
        {
            if (!Directory.Exists(Paths.MergedBundleContent))
                return;

            try
            {
                ProgressInfo.CurrentAction = "Deleting empty Merged Bundle Content directories";
                DeleteEmptyDirectories(Paths.MergedBundleContent);
            }
            catch (Exception ex)
            {
                Program.MainForm.ShowMessage(
                        "Non-critical error: Failed to delete empty Merged Bundle Content directories.\n\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Depth-first recursive delete, with handling for descendant 
        /// directories open in Windows Explorer.
        /// </summary>
        void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                System.Threading.Thread.Sleep(1);
                DeleteDirectory(directory);
            }

            try
            {
                System.Threading.Thread.Sleep(1);
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(1);
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                System.Threading.Thread.Sleep(1);
                Directory.Delete(path, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes any subdirectories of the root that are empty, AS WELL AS the root itself, if it's empty.
        /// </summary>
        void DeleteEmptyDirectories(string rootPath)
        {
            foreach (string directory in Directory.GetDirectories(rootPath))
            {
                System.Threading.Thread.Sleep(1);
                DeleteEmptyDirectories(directory);
            }

            if (Directory.GetFiles(rootPath).Any() || Directory.GetDirectories(rootPath).Any())
                return;
            
            try
            {
                System.Threading.Thread.Sleep(1);
                DeleteDirectory(rootPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}