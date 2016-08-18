using System;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        void Options_Load(object sender, EventArgs e)
        {
            chkCheckScripts.Checked = Program.Settings.Get<bool>("CheckScripts");
            chkCheckXmlFiles.Checked = Program.Settings.Get<bool>("CheckScripts");
            chkCheckBundleContents.Checked = Program.Settings.Get<bool>("CheckBundleContents");

            chkCollapseNotMergeable.Checked = Program.Settings.Get<bool>("CollapseNotMergeable");
            chkCollapseCustomLoadOrder.Checked = Program.Settings.Get<bool>("CollapseCustomLoadOrder");

            chkPromptOutdatedMerge.Checked = Program.Settings.Get<bool>("ValidateMergeSources");
            chkPromptPrioritize.Checked = Program.Settings.Get<bool>("ValidateCustomLoadOrder");

            chkReviewEachMerge.Checked = Program.Settings.Get<bool>("ReviewEachMerge");
            chkShowPathsInKDiff3.Checked = Program.Settings.Get<bool>("ShowPathsInKDiff3");
            chkCompletionSounds.Checked = Program.Settings.Get<bool>("PlayCompletionSounds");
            chkMergeReport.Checked = Program.Settings.Get<bool>("ReportAfterMerge");
            chkPackReport.Checked = Program.Settings.Get<bool>("ReportAfterPack");

            btnOK.Select();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            Save();

            DialogResult = DialogResult.OK;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            Save();

            DialogResult = DialogResult.None;
        }

        void Save()
        {
            Program.Settings.Set("CheckScripts", chkCheckScripts.Checked);
            Program.Settings.Set("CheckXmlFiles", chkCheckXmlFiles.Checked);
            Program.Settings.Set("CheckBundleContents", chkCheckBundleContents.Checked);

            Program.Settings.Set("CollapseNotMergeable", chkCollapseNotMergeable.Checked);
            Program.Settings.Set("CollapseCustomLoadOrder", chkCollapseCustomLoadOrder.Checked);

            Program.Settings.Set("ValidateMergeSources", chkPromptOutdatedMerge.Checked);
            Program.Settings.Set("ValidateCustomLoadOrder", chkPromptPrioritize.Checked);

            Program.Settings.Set("ReviewEachMerge", chkReviewEachMerge.Checked);
            Program.Settings.Set("ShowPathsInKDiff3", chkShowPathsInKDiff3.Checked);
            Program.Settings.Set("PlayCompletionSounds", chkCompletionSounds.Checked);
            Program.Settings.Set("ReportAfterMerge", chkMergeReport.Checked);
            Program.Settings.Set("ReportAfterPack", chkPackReport.Checked);

            Program.Settings.Save();
        }
    }
}
