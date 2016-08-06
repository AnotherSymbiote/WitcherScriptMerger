using System.Diagnostics;
using System.IO;

namespace WitcherScriptMerger.Tools
{
    public static class WccLite
    {
        public static string ExePath = Program.Settings.Get("WccLitePath");

        public static int PackBundle(string sourceDir, string outputDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                Program.MainForm.ShowError("Can't find content directory to pack into bundle:\n\n" + sourceDir, "Missing Directory");
                return 1;
            }

            return Run(
                $"pack -dir=\"{sourceDir}\" -outdir=\"{outputDir}\"",
                "Error packing merged content into a new bundle using wcc_lite.\nIts error output is below."
            );
        }

        public static int GenerateMetadata(string bundleDir)
        {
            return Run(
                $"metadatastore -path=\"{bundleDir}\"",
                "Error generating metadata.store for new merged bundle using wcc_lite.\nIts error output is below."
            );
        }

        public static int Run(string arguments, string failureMsg)
        {
            if (!File.Exists(ExePath))
            {
                Program.MainForm.ShowError("Can't find wcc_lite at this location:\n\n" + ExePath, "Missing wcc_lite");
                return 1;
            }

            var procInfo = new ProcessStartInfo
            {
                FileName = ExePath,
                Arguments = arguments,
                WorkingDirectory = Path.GetDirectoryName(ExePath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var wccLiteProc = new Process { StartInfo = procInfo })
            {
                wccLiteProc.Start();
                var stdOutput = wccLiteProc.StandardOutput.ReadToEnd().Trim();
                var stdError = wccLiteProc.StandardError.ReadToEnd().Trim();

                string errorMsg = null;
                if (!string.IsNullOrWhiteSpace(stdError))
                    errorMsg = stdError;
                else if (stdOutput.EndsWith("Wcc operation failed"))
                    errorMsg = stdOutput;
                if (errorMsg != null)
                {
                    Program.MainForm.ShowError(failureMsg + "\n\n" + errorMsg);
                    return 1;
                }
            }
            return 0;
        }
    }
}
