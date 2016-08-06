using System;
using System.Diagnostics;
using System.IO;

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

            if (!Program.MainForm.PathsInKdiff3Setting)
            {
                if (hasVanillaVersion)
                    args += $" --L1 Vanilla --L2 \"{source1.Name}\" --L3 \"{source2.Name}\"";
                else
                    args += $" --L1 \"{source1.Name}\" --L2 \"{source2.Name}\"";
            }

            if (!Program.MainForm.ReviewEachMergeSetting && hasVanillaVersion)
                args += " --auto";

            var kdiff3Path = (Path.IsPathRooted(ExePath)
                ? ExePath
                : Path.Combine(Environment.CurrentDirectory, ExePath));

            var kdiff3Proc = Process.Start(kdiff3Path, args);
            kdiff3Proc.WaitForExit();

            return kdiff3Proc.ExitCode;
        }
    }
}
