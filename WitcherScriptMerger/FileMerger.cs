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
        private MergeInventory Inventory { get; set; }
        private FileInfo VanillaFile { get; set; }
        private FileInfo File1 { get; set; }
        private FileInfo File2 { get; set; }
        private int _mergesToDo;

        public FileMerger(MergeInventory inventory)
        {
            Inventory = inventory;
        }

        public void MergeByTreeNodes(IEnumerable<TreeNode> nodesToMerge, string mergedModName)
        {
            foreach (var fileNode in nodesToMerge)
            {
                var modNodes = fileNode.GetTreeNodes().Where(modNode => modNode.Checked).ToList();

                if (modNodes.Any(node => mergedModName.CompareTo(node.Text) > 0) &&
                    !ConfirmRemainingConflict(mergedModName))
                    continue;

                File1 = new FileInfo(modNodes[0].Tag as string);

                string relPath = Paths.GetRelativePath(
                    File1.FullName,
                    Path.Combine(Paths.ModsDirectory, ModFile.GetModNameFromPath(File1.FullName)));

                string outputPath = Path.Combine(Paths.ModsDirectory, mergedModName, relPath);

                if (File.Exists(outputPath) && !ConfirmOutputOverwrite(outputPath))
                    continue;

                VanillaFile = new FileInfo(fileNode.Tag as string);
                _mergesToDo = modNodes.Count - 1;

                bool isNew = false;
                var merge = Inventory.Merges.FirstOrDefault(ms => ms.RelativePath == fileNode.Text);
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
                    File2 = new FileInfo(modNodes[i].Tag as string);
                    var mergedFile = MergePair(i, outputPath, merge);
                    if (mergedFile != null)
                        File1 = mergedFile;
                    else
                        HandleCanceledMerge(i, merge, nodesToMerge);
                }
                if (isNew && merge.ModNames.Count > 1)
                    Inventory.Merges.Add(merge);
            }
        }

        public FileInfo MergePair(int mergeNum, string outputPath, Merge mergedScript)
        {
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string args = string.Format(
                "\"{0}\" \"{1}\" \"{2}\" -o \"{3}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\"",
                VanillaFile.FullName, File1.FullName, File2.FullName, outputPath);

            string modName1 = ModFile.GetModNameFromPath(File1.FullName);
            string modName2 = ModFile.GetModNameFromPath(File2.FullName);

            if (!Program.MainForm.PathsInKdiff3Setting)
                args += string.Format(" --L1 Vanilla --L2 \"{0}\" --L3 \"{1}\"", modName1, modName2);

            if (!Program.MainForm.ReviewEachMergeSetting)
                args += " --auto";

            string kdiff3Path = (Path.IsPathRooted(Paths.Kdiff3)
                ? Paths.Kdiff3
                : Path.Combine(Environment.CurrentDirectory, Paths.Kdiff3));

            var kdiff3Proc = Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            if (kdiff3Proc.ExitCode == 0)
            {
                if (File1.FullName != outputPath)
                    mergedScript.ModNames.Add(modName1);

                if (File2.FullName != outputPath)
                    mergedScript.ModNames.Add(modName2);

                if (Program.MainForm.MergeReportSetting)
                {
                    using (var reportForm = new ReportForm(
                        mergeNum, _mergesToDo,
                        File1.FullName, File2.FullName, outputPath,
                        modName1, modName2))
                    {
                        reportForm.ShowDialog();
                    }
                }
                return new FileInfo(outputPath);
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
            string msg = string.Format("Merge was canceled for {0}.", VanillaFile.Name);
            var buttons = MessageBoxButtons.OK;
            if (_mergesToDo > 1 || nodesToMerge.Count() > 1)
            {
                if (_mergesToDo > 1)
                {
                    msg = string.Format("Merge {0} of {1} was canceled for {2}.", mergeNum, _mergesToDo, VanillaFile.Name);
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
    }
}