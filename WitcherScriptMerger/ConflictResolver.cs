using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public partial class ConflictResolver : Form
    {
        public enum ConflictResolution : int
        {
            Vanilla = DialogResult.Ignore,
            Left    = DialogResult.Yes,
            Right   = DialogResult.No
        }

        private int _vanillaStartLine;
        private int _leftStartLine;
        private int _rightStartLine;

        public ConflictResolver(string fileName, string leftModName, string rightModName)
        {
            InitializeComponent();
            this.Text += fileName;
            btnUseLeft.Text += leftModName;
            btnUseRight.Text += rightModName;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
        }

        public void SetVanillaText(string text, Range changeRange)
        {
            SetText(rtbVanilla, text, changeRange);
        }

        public void SetLeftText(string text, Range changeRange)
        {
            SetText(rtbLeft, text, changeRange);
        }

        public void SetRightText(string text, Range changeRange)
        {
            SetText(rtbRight, text, changeRange);
        }

        private void SetText(RichTextBox rtb, string text, Range changeRange, bool highlight = true)
        {
            // Add text 
            Range contextRange = new Range(changeRange);
            int contextRadius = Program.Settings.Get<int>("ConflictResolverContextRadius");
            for (int i = 0; i < contextRadius; ++i)
            {
                if (contextRange.Start > 0)
                    contextRange.Start = text.PreviousLineBreak(contextRange.Start);
                if (contextRange.End < text.Length - 1)
                    contextRange.End = text.NextLineBreak(contextRange.End);
            }
            rtb.Text = text.Substring(contextRange.Start, contextRange.Length);

            int startLine = text.Take(contextRange.Start).Count(c => c == '\n') + 1;
            if (rtb == rtbVanilla)
                _vanillaStartLine = startLine;
            else if (rtb == rtbLeft)
                _leftStartLine = startLine;
            else if (rtb == rtbRight)
                _rightStartLine = startLine;

            if (highlight)
            {
                changeRange.Start -= contextRange.Start;  // Offset change range to be
                changeRange.End -= contextRange.Start;    // relative to context start

                int breakCount1 = rtb.GetLineBreakCount(0, changeRange.Start);
                int breakCount2 = breakCount1 + rtb.GetLineBreakCount(changeRange.Start, changeRange.End);
                changeRange.Start -= breakCount1;
                changeRange.End -= breakCount2;

                rtb.Select(changeRange.Start, changeRange.Length);
                rtb.SelectionBackColor = Color.Goldenrod;
                rtb.SelectionFont = new Font(rtbVanilla.SelectionFont, FontStyle.Bold);
                if (rtb.Text[rtb.SelectionStart] == '\n' && rtb.SelectionStart < rtb.Text.Length - 1)
                    ++rtb.SelectionStart;  // Workaround: Advance past \n to get correct line number.
                rtb.SelectionLength = 0;
            }
        }

        private void btnUseLeft_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)ConflictResolution.Left;
        }

        private void btnUseRight_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)ConflictResolution.Right;
        }

        private void btnUseVanilla_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)ConflictResolution.Vanilla;
        }

        private void rtbVanilla_SelectionChanged(object sender, EventArgs e)
        {
            SetInfo(rtbVanilla, lblVanillaInfo, _vanillaStartLine);
        }

        private void rtbLeft_SelectionChanged(object sender, EventArgs e)
        {
            SetInfo(rtbLeft, lblLeftInfo, _leftStartLine);
        }

        private void rtbRight_SelectionChanged(object sender, EventArgs e)
        {
            SetInfo(rtbRight, lblRightInfo, _rightStartLine);
        }

        private void SetInfo(RichTextBox rtb, Label lbl, int contextStartLine)
        {
            int lineNum = contextStartLine + rtb.GetLineFromCharIndex(rtb.SelectionStart);
            lbl.Text = string.Format("Line: {0}", lineNum);
        }
    }
}
