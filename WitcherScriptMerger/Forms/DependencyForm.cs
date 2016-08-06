using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WitcherScriptMerger.Tools;

namespace WitcherScriptMerger.Forms
{
    public partial class DependencyForm : Form
    {
        bool AreAnyPathsChanged
        {
            get
            {
                return (!txtKDiff3Path.Text.EqualsIgnoreCase(KDiff3.ExePath) ||
                        !txtBmsPath.Text.EqualsIgnoreCase(QuickBms.ExePath) ||
                        !txtBmsPluginPath.Text.EqualsIgnoreCase(QuickBms.PluginPath) ||
                        !txtWccLitePath.Text.EqualsIgnoreCase(WccLite.ExePath));
            }
        }

        public DependencyForm()
        {
            InitializeComponent();
        }

        void DependencyForm_Load(object sender, EventArgs e)
        {
            txtKDiff3Path.Text = KDiff3.ExePath;
            txtBmsPath.Text = QuickBms.ExePath;
            txtBmsPluginPath.Text = QuickBms.PluginPath;
            txtWccLitePath.Text = WccLite.ExePath;
            btnOK.Select();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            var allValid =
                Color.LightGreen == txtKDiff3Path.BackColor &&
                Color.LightGreen == txtBmsPath.BackColor &&
                Color.LightGreen == txtBmsPluginPath.BackColor &&
                Color.LightGreen == txtWccLitePath.BackColor;

            if (!allValid &&
                DialogResult.No == MessageBox.Show(
                    "Not all the files are located & valid. Save settings anyway?",
                    "Missing Dependency",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning))
            {
                DialogResult = DialogResult.None;
                return;
            }

            if (AreAnyPathsChanged)
            {
                Program.Settings.StartBatch();
                KDiff3.ExePath = UpdatePathSetting(KDiff3.ExePath, txtKDiff3Path.Text, "Kdiff3Path");
                QuickBms.ExePath = UpdatePathSetting(QuickBms.ExePath, txtBmsPath.Text, "QuickBmsPath");
                QuickBms.PluginPath = UpdatePathSetting(QuickBms.PluginPath, txtBmsPluginPath.Text, "QuickBmsPluginPath");
                WccLite.ExePath = UpdatePathSetting(WccLite.ExePath, txtWccLitePath.Text, "WccLitePath");
                Program.Settings.EndBatch();
            }

            DialogResult = (allValid
                ? DialogResult.OK
                : DialogResult.Cancel);
        }

        string UpdatePathSetting(string oldPath, string newPath, string settingName)
        {
            if (oldPath.EqualsIgnoreCase(newPath))
                return oldPath;
            Program.Settings.Set(settingName, newPath);
            return newPath;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region Selecting Files

        void btnKDiff3Path_Click(object sender, EventArgs e)
        {
            GetUserFileChoice(txtKDiff3Path, "Executables|*.exe");
        }

        void btnBmsPath_Click(object sender, EventArgs e)
        {
            GetUserFileChoice(txtBmsPath, "Executables|*.exe");
        }

        void btnBmsPluginPath_Click(object sender, EventArgs e)
        {
            GetUserFileChoice(txtBmsPluginPath, "QuickBMS Plugins|*.bms");
        }

        void btnWccLitePath_Click(object sender, EventArgs e)
        {
            GetUserFileChoice(txtWccLitePath, "Executables|*.exe");
        }

        void GetUserFileChoice(TextBox txt, string filter)
        {
            var dlgSelectFile = new OpenFileDialog();
            dlgSelectFile.Filter = filter;
            if (!string.IsNullOrWhiteSpace(txt.Text) && File.Exists(txt.Text))
                dlgSelectFile.FileName = txt.Text;
            if (DialogResult.OK == dlgSelectFile.ShowDialog())
                txt.Text = dlgSelectFile.FileName.Replace(Environment.CurrentDirectory + "\\", "");
        }

        #endregion

        #region Clicking Links

        void lnkKDiff3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://kdiff3.sourceforge.net/");
        }

        void lnkBms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://aluigi.altervista.org/quickbms.htm");
        }

        void lnkWccLite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.nexusmods.com/witcher3/news/12625/?");
        }

        #endregion

        #region Validation

        void exe_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBox(sender as TextBox, ".exe");
        }

        void bms_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBox(sender as TextBox, ".bms");
        }

        void ValidateTextBox(TextBox txt, string validExtension)
        {
            var path = txt.Text;
            txt.BackColor = (path.EndsWithIgnoreCase(validExtension) && File.Exists(path)
                ? Color.LightGreen
                : Color.LightPink);
        }

        #endregion
    }
}