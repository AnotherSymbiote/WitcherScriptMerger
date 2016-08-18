using System;
using System.Diagnostics;
using System.IO;
using WitcherScriptMerger.Inventory;

namespace WitcherScriptMerger.Tools
{
    static class KDiff3
    {
        public static string ExePath = Program.Settings.Get("KDiff3Path");

        public static int Run(
            FileMerger.MergeSource source1,
            FileMerger.MergeSource source2,
            FileInfo vanillaFile,
            string outputPath)
        {
            if (!File.Exists(ExePath))
            {
                Program.MainForm.ShowError("Can't find KDiff3 at this location:\n\n" + ExePath, "Missing KDiff3");
                return 1;
            }

            var outputDir = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var hasVanillaVersion = (vanillaFile != null && vanillaFile.Exists);

            var args = (hasVanillaVersion
                ? "\"" + vanillaFile.FullName + "\" "
                : "");

            args +=
                $"\"{source1.TextFile.FullName}\" \"{source2.TextFile.FullName}\" " +
                $"-o \"{outputPath}\" " +
                "--cs \"WhiteSpace3FileMergeDefault=2\" " +
                "--cs \"CreateBakFiles=0\" " +
                "--cs \"LineEndStyle=1\" " +
                "--cs \"FollowFileLinks=1\" " +
                "--cs \"FollowDirLinks=1\"";

            if (!Program.Settings.Get<bool>("ShowPathsInKDiff3"))
            {
                if (hasVanillaVersion)
                    args += $" --L1 Vanilla --L2 \"{source1.Name}\" --L3 \"{source2.Name}\"";
                else
                    args += $" --L1 \"{source1.Name}\" --L2 \"{source2.Name}\"";
            }

            if (!Program.Settings.Get<bool>("ReviewEachMerge") && hasVanillaVersion)
            {
                if (source1.TextFile.FullName.EqualsIgnoreCase(outputPath)
                    && source2.Hash != null && source2.Hash.IsOutdated)
                {
                    Program.MainForm.ShowMessage(
                        "You are merging an updated mod file into a merge created with a previous version of the file.\n\n" +
                        "You should carefully inspect this merge, because KDiff3's auto-solving behavior KEEPS changes from the previous version of the mod file that have been REMOVED in the new version.",
                        "Warning",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                    args += " --auto";
            }

            var kdiff3Path = (Path.IsPathRooted(ExePath)
                ? ExePath
                : Path.Combine(Environment.CurrentDirectory, ExePath));

            var kdiff3Proc = Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            return kdiff3Proc.ExitCode;
        }
    }
}
