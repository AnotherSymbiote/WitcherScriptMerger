using System;
using System.Windows.Forms;
using WitcherScriptMerger.Forms;

namespace WitcherScriptMerger
{
    static class Program
    {
        public static AppSettings Settings = new AppSettings();
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
    }

}