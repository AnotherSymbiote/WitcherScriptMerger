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
        
        public static string Kdiff3Path = Program.Settings.Get("KDiff3Path");
        public static string BmsPath = Program.Settings.Get("QuickBmsPath");
        public static string BmsPluginPath = Path.Combine(Path.GetDirectoryName(BmsPath), "witcher3.bms");
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!File.Exists(Kdiff3Path))
            {
                MessageBox.Show("Launch failed. Can't find KDiff3 at the following path:\n\n" + Kdiff3Path,
                    "Can't Find KDiff3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
            if (!File.Exists(BmsPath))
            {
                MessageBox.Show("Launch failed. Can't find QuickBMS at the following path:\n\n" + BmsPath,
                    "Can't Find QuickBMS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainForm();
            Application.Run(MainForm);
        }
    }
}
