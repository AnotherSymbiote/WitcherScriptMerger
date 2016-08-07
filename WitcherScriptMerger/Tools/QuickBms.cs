using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WitcherScriptMerger.Tools
{
    static class QuickBms
    {
        public static string ExePath = Program.Settings.Get("QuickBmsPath");
        public static string PluginPath = Program.Settings.Get("QuickBmsPluginPath");

        public static int UnpackFile(string bundlePath, string contentRelativePath, string outputDir)
        {
            if (!ValidateResources(bundlePath))
                return 1;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var startInfo = BuildStartInfo($"-Y -f \"{contentRelativePath}\" \"{PluginPath}\" \"{bundlePath}\" \"{outputDir}\"");

            using (var bmsProc = new Process { StartInfo = startInfo })
            {
                bmsProc.Start();
                var output = bmsProc.StandardError.ReadToEnd();  // QuickBMS prints results to std error, even if successful

                if (output.Contains("- 0 files found"))
                {
                    var errorMsg = "Error unpacking bundle content file using QuickBMS.\nIts output is below.";
                    var outputStart = output.IndexOf("- filter string");
                    if (outputStart != -1)
                    {
                        output = output.Substring(outputStart);
                        errorMsg += "\n\n" + output;
                    }
                    Program.MainForm.ShowError(errorMsg);
                    return 1;
                }

                return 0;
            }
        }

        public static string[] GetBundleContentPaths(string bundlePath)
        {
            if (!ValidateResources(bundlePath))
                return null;

            var contentPaths = new List<string>();

            var startInfo = BuildStartInfo($"-l \"{PluginPath}\" \"{bundlePath}\"");

            using (var bmsProc = new Process { StartInfo = startInfo })
            {
                bmsProc.Start();
                var output = bmsProc.StandardOutput.ReadToEnd() + "\n\n" + bmsProc.StandardError.ReadToEnd();
                var footerPos = output.LastIndexOf("QuickBMS generic");
                var outputLines = output.Substring(0, footerPos).Split('\n');
                var paths = outputLines
                    .Where(line => line.Length > 5)
                    .Select(line => line.Substring(line.LastIndexOf(' ')).Trim());
                contentPaths.AddRange(paths);
            }
            return contentPaths.ToArray();
        }

        static bool ValidateResources(string bundlePath)
        {
            if (!File.Exists(bundlePath))
            {
                Program.MainForm.ShowError("Can't find bundle file:\n\n" + bundlePath, "Missing Bundle");
                return false;
            }
            if (!File.Exists(ExePath))
            {
                Program.MainForm.ShowError("Can't find QuickBMS at this location:\n\n" + ExePath, "Missing QuickBMS");
                return false;
            }
            if (!File.Exists(PluginPath))
            {
                Program.MainForm.ShowError("Can't find QuickBMS plugin at this location:\n\n" + PluginPath, "Missing QuickBMS Plugin");
                return false;
            }
            return true;
        }

        static ProcessStartInfo BuildStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = ExePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
        }
    }
}
