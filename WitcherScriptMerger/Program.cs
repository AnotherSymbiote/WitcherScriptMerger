using System;
using System.IO;
using System.Windows.Forms;
using WitcherScriptMerger.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger
{
    static class Program
    {
        public static AppSettings Settings = new AppSettings();
        public static CustomLoadOrder LoadOrder = null;
        public static MainForm MainForm;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Settings.HasConfigFile)
            {
                ShowLaunchFailure("Config file is missing.");
                return;
            }
            if (!Paths.ValidateDependencyPaths())
            {
                using (var dependencyForm = new DependencyForm())
                {
                    if (dependencyForm.ShowDialog() != DialogResult.OK)
                    {
                        ShowLaunchFailure("A dependency is missing.");
                        return;
                    }
                }
            }

            MainForm = new MainForm();
            Application.Run(MainForm);
        }
        static void ShowLaunchFailure(string message)
        {
            MessageBox.Show(
                $"Launch failure: {message}",
                "Script Merger Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void TryOpenFile(string path)
        {
            if (File.Exists(path))
                System.Diagnostics.Process.Start(path);
            else
                MainForm.ShowMessage("Can't find file: " + path);
        }
        public static void TryOpenFileLocation(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dirPath))
                System.Diagnostics.Process.Start(dirPath);
            else
                MainForm.ShowMessage("Can't find directory: " + dirPath);
        }

        public static void TryOpenDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
                System.Diagnostics.Process.Start(dirPath);
            else
                MainForm.ShowMessage("Can't find directory: " + dirPath);
        }
    }

}