using System;
using System.IO;
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

            if (!Paths.ValidateDependencyPaths())
            {
                using (var dependencyForm = new DependencyForm())
                {
                    if (dependencyForm.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Launch failed. A dependency is missing.",
                            "Script Merger Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            MainForm = new MainForm();
            Application.Run(MainForm);
        }
    }
}