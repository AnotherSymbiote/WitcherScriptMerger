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

        private MergeInventory _inventory;
        private IEnumerable<TreeNode> _nodesToMerge;
        private FileInfo _vanillaFile;
        private FileInfo _file1;
        private FileInfo _file2;
        private string _modName1;
        private string _modName2;
        private string _mergedModName;
        private int _mergesToDo;
        private string _outputPath;
        
        private string _bundlePath;
        private bool _bundleChanged;
        private List<Merge> _pendingBundleMerges = new List<Merge>();
        
        private BackgroundWorker _bgWorker;
        private string[] _progressState = new string[2];

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
            _bgWorker.RunWorkerCompleted += completedHandler;
            _bundlePath = Paths.RetrieveMergedBundlePath();
        }

        ~FileMerger()
        {
            if (_bgWorker != null)
                _bgWorker.Dispose();
        }

        public void MergeByTreeNodesAsync(
            IEnumerable<TreeNode> nodesToMerge,
            string mergedModName)
        {
            
            _bgWorker.DoWork += (sender, e) =>
            {
                _nodesToMerge = nodesToMerge;
                _mergedModName = mergedModName;
                foreach (var fileNode in _nodesToMerge)
                {
                    ReportProgress("Starting merge", "Merging " + Path.GetFileName(fileNode.Text));
                    MergeTreeNode(fileNode);
                }
                if (_bundleChanged)
                {
                    string newBundlePath = PackNewBundle(_bundlePath);
                    if (newBundlePath != null)
                    {
                        ReportProgress("Adding bundle merge to inventory");
                        foreach (var bundleMerge in _pendingBundleMerges)
                            _inventory.Merges.Add(bundleMerge);

                        if (Program.MainForm.PackReportSetting)
                        {
                            using (var reportForm = new PackReportForm(newBundlePath))
                            {
                                Program.MainForm.ShowModal(reportForm);
                            }
                        }
                    }
                }
                if (Directory.Exists(Paths.TempBundleContent))
                {
                    ReportProgress("Deleting temporary unpacked bundle content");
                    DeleteDirectory(Paths.TempBundleContent);
                }
            };
            _bgWorker.RunWorkerAsync();
        }

        private void MergeTreeNode(TreeNode fileNode)
        {
            bool isScript = ModFile.IsScript(fileNode.Text);

            var modNodes = fileNode.GetTreeNodes().Where(modNode => modNode.Checked).ToList();

            if (modNodes.Any(node => ModFile.GetLoadOrder(node.Text, _mergedModName) < 0) &&
                !ConfirmRemainingConflict(_mergedModName))
                return;

            _file1 = new FileInfo(modNodes[0].Tag as string);
            _modName1 = ModFile.GetModNameFromPath(_file1.FullName);

            string relPath = Paths.GetRelativePath(
                _file1.FullName,
                Path.Combine(Paths.ModsDirectory, ModFile.GetModNameFromPath(_file1.FullName)));

            _outputPath = (isScript
                ? Path.Combine(Paths.ModsDirectory, _mergedModName, relPath)
                : Path.Combine(Paths.MergedBundleContent, fileNode.Text));

            if (File.Exists(_outputPath) && !ConfirmOutputOverwrite(_outputPath))
                return;

            _vanillaFile = (isScript
                ? new FileInfo(fileNode.Tag as string)
                : null);
            _mergesToDo = modNodes.Count - 1;

            bool isNew = false;
            var merge = _inventory.Merges.FirstOrDefault(ms => ms.RelativePath.EqualsIgnoreCase(fileNode.Text));
            if (merge == null)
            {
                isNew = true;
                merge = new Merge
                {
                    RelativePath = fileNode.Text,
                    MergedModName = _mergedModName,
                };
            }

            for (int i = 1; i < modNodes.Count; ++i)
            {
                _file2 = new FileInfo(modNodes[i].Tag as string);
                _modName2 = ModFile.GetModNameFromPath(_file2.FullName);

                if (_vanillaFile == null)  // Need to find vanilla bundle
                {
                    if (!GetUnpackedFiles(fileNode.Text))
                    {
                        HandleCanceledMerge(i, _nodesToMerge.Count(), merge);
                        continue;
                    }
                }

                var mergedFile = MergeText(i, merge);
                if (mergedFile != null)
                {
                    _file1 = mergedFile;
                    _modName1 = ModFile.GetModNameFromPath(_file1.FullName);
                }
                else
                    HandleCanceledMerge(i, _nodesToMerge.Count(), merge);
            }

            if (isNew && merge.ModNames.Count > 1)
            {
                if (isScript)
                {
                    ReportProgress(string.Format("Adding script merge to inventory"));
                    _inventory.Merges.Add(merge);
                }
                else
                {
                    if (_bundlePath == null)
                        return;
                    merge.BundleName = Path.GetFileName(_bundlePath);
                    _bundleChanged = true;
                    _pendingBundleMerges.Add(merge);
                }
            }
        }

        private FileInfo MergeText(int mergeNum, Merge mergedFile)
        {
            ReportProgress(string.Format("Using KDiff3 to merge {0} && {1}", _modName1, _modName2));

            string outputDir = Path.GetDirectoryName(_outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            bool hasVanillaVersion = (_vanillaFile != null && _vanillaFile.Exists);

            string args = (hasVanillaVersion
                ? "\"" + _vanillaFile.FullName + "\" "
                : "");

            args += string.Format(
                "\"{0}\" \"{1}\" -o \"{2}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\" " +
                "--cs \"CreateBakFiles=0\"",
                _file1.FullName, _file2.FullName, _outputPath);

            if (!Program.MainForm.PathsInKdiff3Setting)
            {
                if (hasVanillaVersion)
                    args += string.Format(" --L1 Vanilla --L2 \"{0}\" --L3 \"{1}\"", _modName1, _modName2);
                else
                    args += string.Format(" --L1 \"{0}\" --L2 \"{1}\"", _modName1, _modName2);
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
                if (!_file1.FullName.EqualsIgnoreCase(_outputPath))
                    mergedFile.ModNames.Add(_modName1);

                if (!_file2.FullName.EqualsIgnoreCase(_outputPath))
                    mergedFile.ModNames.Add(_modName2);

                if (Program.MainForm.MergeReportSetting)
                {
                    using (var reportForm = new MergeReportForm(
                        mergeNum, _mergesToDo,
                        _file1.FullName, _file2.FullName, _outputPath,
                        _modName1, _modName2))
                    {
                        Program.MainForm.ShowModal(reportForm);
                    }
                }
                return new FileInfo(_outputPath);
            }
            else
                return null;
        }

        private bool ConfirmRemainingConflict(string mergedModName)
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

        private bool ConfirmOutputOverwrite(string outputPath)
        {
            return (DialogResult.Yes == Program.MainForm.ShowMessage(
                "The output file below already exists! Overwrite?\n\n" + outputPath,
                "Overwrite?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation));
        }

        private DialogResult HandleCanceledMerge(int mergeNum, int mergeCount, Merge merge)
        {
            string fileName = Path.GetFileName(merge.RelativePath);
            string msg = string.Format("Merge was canceled for {0}.", fileName);
            var buttons = MessageBoxButtons.OK;
            if (_mergesToDo > 1 || mergeCount > 1)
            {
                if (_mergesToDo > 1)
                {
                    msg = string.Format("Merge {0} of {1} was canceled for {2}.", mergeNum, _mergesToDo, fileName);
                    if (mergeNum < _mergesToDo)
                    {
                        msg += "\n\nContinue with the remaining merges for this file?";
                        buttons = MessageBoxButtons.YesNo;
                    }
                }
            }
            Program.MainForm.ActivateSafely(); // Focus window
            var result = Program.MainForm.ShowMessage(msg, "Skipped Merge", buttons, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                return DialogResult.Abort;
            }
            return DialogResult.OK;
        }

        private bool GetUnpackedFiles(string contentRelativePath)
        {
            _vanillaFile = null;
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
                ReportProgress("Unpacking vanilla bundle content file");
                string vanillaContentFile = UnpackFile(_vanillaFile.FullName, contentRelativePath, "Vanilla");
                _vanillaFile = new FileInfo(vanillaContentFile);
            }

            ReportProgress("Unpacking bundle content file for " + _modName1);
            string modContentFile1 = UnpackFile(_file1.FullName, contentRelativePath, "Mod 1");
            if (modContentFile1 == null)
                return false;
            ReportProgress("Unpacking bundle content file for " + _modName2);
            string modContentFile2 = UnpackFile(_file2.FullName, contentRelativePath, "Mod 2");
            if (modContentFile2 == null)
                return false;
            _file1 = new FileInfo(modContentFile1);
            _file2 = new FileInfo(modContentFile2);
            return true;
        }

        private string UnpackFile(string bundlePath, string contentRelativePath, string outputDirName)
        {
            if (!ValidateBmsResources(bundlePath))
                return null;

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

        public void PackNewBundleAsync(string bundlePath)
        {
            if (_bgWorker.IsBusy)
                throw new Exception("BackgroundWorker can't run 2 tasks concurrently.");
            _bgWorker.DoWork += (sender, e) =>
            {
                string newBundlePath = PackNewBundle(bundlePath);
                if (newBundlePath != null && Program.MainForm.PackReportSetting)
                {
                    using (var reportForm = new PackReportForm(bundlePath))
                    {
                        Program.MainForm.ShowModal(reportForm);
                    }
                }
            };
            _bgWorker.RunWorkerAsync();
        }

        private string PackNewBundle(string bundlePath)
        {
            ReportProgress("Packing merged content into new blob0.bundle", "Packing Bundle");
            
            string contentDir = Path.Combine(Environment.CurrentDirectory, Paths.MergedBundleContent);
            
            if (!ValidateWccLiteResources(contentDir))
                return null;

            string outputDir = Path.GetDirectoryName(bundlePath);
            var procInfo = new ProcessStartInfo
            {
                FileName = Paths.WccLite,
                Arguments = string.Format("pack -dir=\"{0}\" -outdir=\"{1}\"",
                    contentDir,
                    outputDir),
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
            ReportProgress("Generating metadata.store for new blob0.bundle");
            procInfo.Arguments = string.Format("metadatastore -path=\"{0}\"", outputDir);
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

        private bool ValidateBmsResources(string bundlePath)
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

        private bool ValidateWccLiteResources(string contentDir)
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

        private void ShowError(string msg, string title = "Error")
        {
            Program.MainForm.ShowMessage(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReportProgress(string msg, string title = null)
        {
            if (title != null)
                _progressState[0] = title;
            _progressState[1] = msg;
            _bgWorker.ReportProgress(0, _progressState);
        }

        /// <summary>
        /// Depth-first recursive delete, with handling for descendant 
        /// directories open in Windows Explorer.
        /// </summary>
        private void DeleteDirectory(string path)
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
            catch (Exception ex)
            {
                Program.MainForm.ShowMessage(
                    "Non-critical error: Failed to delete temporary unpacked bundle content.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}