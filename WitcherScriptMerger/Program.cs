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
            if (!File.Exists(Paths.Kdiff3))
            {
                MessageBox.Show("Launch failed. Can't find KDiff3 at the following path:\n\n" + Paths.Kdiff3,
                    "Script Merger Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(Paths.Bms))
            {
                MessageBox.Show("Launch failed. Can't find QuickBMS at the following path:\n\n" + Paths.Bms,
                    "Script Merger Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainForm();
            Application.Run(MainForm);
        }
    }
}
