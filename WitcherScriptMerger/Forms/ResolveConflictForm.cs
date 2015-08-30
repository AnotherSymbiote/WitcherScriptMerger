using GoogleDiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    public partial class ResolveConflictForm : Form
    {
        public enum ConflictResolution : int
        {
            Vanilla = DialogResult.Ignore,
            Left    = DialogResult.Yes,
            Right   = DialogResult.No
        }

        #region Members

        private int _vanillaStartLine;
        private int _leftStartLine;
        private int _rightStartLine;

        private int _beamCurrentlyMoving = -1;
        private float _col0WidthPct = 1f / 3f;
        private float _col1WidthPct = 1f / 3f;
        private float _col2WidthPct = 1f / 3f;

        private const float MinColWidthPct = 0.05f;

        #endregion

        public ResolveConflictForm(string fileName, string leftModName, string rightModName)
        {
            InitializeComponent();
            this.Text += fileName;
            btnUseLeft.Text += leftModName;
            btnUseRight.Text += rightModName;
            this.Width = Screen.PrimaryScreen.Bounds.Width;

            rtbVanilla.AddPeer(rtbLeft);
            rtbVanilla.AddPeer(rtbRight);
            rtbLeft.AddPeer(rtbVanilla);
            rtbLeft.AddPeer(rtbRight);
            rtbRight.AddPeer(rtbVanilla);
            rtbRight.AddPeer(rtbLeft);
        }

        #region Setting Text

        public void SetVanillaText(string text, Range changeRange)
        {
            SetText(rtbVanilla, text, changeRange);
        }

        public void SetLeftText(string text, Range changeRange, SMPatch patch)
        {
            SetText(rtbLeft, text, changeRange, patch);
        }

        public void SetRightText(string text, Range changeRange, SMPatch patch)
        {
            SetText(rtbRight, text, changeRange, patch);
        }

        private void SetText(RichTextBox rtb, string text, Range changeRange, SMPatch patch = null)
        {
            // Add lines before & after change range, to give the user some context
            Range contextRange = new Range(changeRange);
            int contextRadius = Program.Settings.Get<int>("ConflictResolverContextRadius");
            for (int i = 0; i < contextRadius; ++i)
            {
                if (contextRange.Start > 0)
                    contextRange.Start = text.PreviousLineBreak(contextRange.Start);
                if (contextRange.End < text.Length - 1)
                    contextRange.End = text.NextLineBreak(contextRange.End);
            }

            // RichTextBox strips out all carriage returns (\r), so just replace them with
            // a paragraph symbol as a placeholder to keep indexes correct
            rtb.Text = text.Substring(contextRange.Start, contextRange.Length).Replace('\r', '\xB6');

            changeRange.Start -= contextRange.Start;  // Offset change range to be
            changeRange.End -= contextRange.Start;    // relative to context start

            var insertRanges = new List<Range>();
            var deleteRanges = new List<Range>();
            if (patch != null)
            {
                int offset = 0;
                foreach (var diff in patch.Diffs)
                {
                    if (diff.Operation == Operation.EQUAL && offset == 0)  // Don't consider leading equalities.
                        continue;
                    if (diff.Operation == Operation.INSERT)
                    {
                        insertRanges.Add(Range.WithLength(changeRange.Start + offset, diff.Text.Length));
                    }
                    else if (diff.Operation == Operation.DELETE)
                    {
                        int insertPos = changeRange.Start + offset;
                        Range deleteRange;
                            deleteRange = Range.WithLength(insertPos, diff.Text.Length);
                        deleteRanges.Add(deleteRange);
                        rtb.Text = rtb.Text.Insert(insertPos, diff.Text.Replace('\r', '\xB6'));
                        changeRange.End += deleteRange.Length;
                    }
                    offset += diff.Text.Length;
                }
            }

            int startLine = text.Take(contextRange.Start).Count(c => c == '\n') + 1;
            if (rtb == rtbVanilla)
                _vanillaStartLine = startLine;
            else if (rtb == rtbLeft)
                _leftStartLine = startLine;
            else if (rtb == rtbRight)
                _rightStartLine = startLine;

            rtb.Select(changeRange.Start, changeRange.Length);
            rtb.SelectionBackColor = Color.Goldenrod;
            rtb.SelectionFont = new Font(rtbVanilla.SelectionFont, FontStyle.Bold);

            foreach (var insertRange in insertRanges)
            {
                rtb.Select(insertRange.Start, insertRange.Length);
                rtb.SelectionBackColor = Color.LightGreen;
            }
            foreach (var deleteRange in deleteRanges)
            {
                rtb.Select(deleteRange.Start, deleteRange.Length);
                rtb.SelectionBackColor = Color.LightPink;
                rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold | FontStyle.Strikeout);
            }

            if (rtb.Text[rtb.SelectionStart] == '\n' && rtb.SelectionStart < rtb.Text.Length - 1)
                ++rtb.SelectionStart;  // Workaround: Advance past \n to get correct line number
            rtb.SelectionLength = 0;

            // To preserve formatting, remove paragraph symbols from RTF code instead of just Text
            if (!Program.Settings.Get<bool>("LineBreakSymbol"))
                rtb.Rtf = rtb.Rtf.Replace("\\'b6", "");
        }

        #endregion

        #region Choosing Version

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

        #endregion

        #region Updating Info Text

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

        #endregion

        #region Resizing Columns

        private void tableLayoutPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var textAreaTopLeft = rtbVanilla.Location;
            var textAreaBottomRight = new Point(rtbRight.Right, rtbRight.Bottom);

            if (!IsInResizableColumns(e.Location) && _beamCurrentlyMoving == -1)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            
            int beam0 = (int)tableLayoutPanel.ColumnStyles[0].Width;
            int beam1 = (int)tableLayoutPanel.ColumnStyles[0].Width + (int)tableLayoutPanel.ColumnStyles[1].Width;

            this.Cursor = Cursors.VSplit;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                switch (_beamCurrentlyMoving)
                {
                    case 0:
                    {
                        if (e.X <= 0)
                            return;
                        float newCol0WidthPct = ((float)e.X / tableLayoutPanel.Width);
                        float newCol1WidthPct = _col1WidthPct + (_col0WidthPct - newCol0WidthPct);
                        if (newCol0WidthPct < MinColWidthPct || newCol1WidthPct < MinColWidthPct)
                            return;
                        _col0WidthPct = newCol0WidthPct;
                        _col1WidthPct = newCol1WidthPct;
                        UpdateColumnWidths();
                        break;
                    }
                    case 1:
                    {
                        if (e.X <= 0)
                            return;
                        float newCol2WidthPct = (1f - ((float)e.X / tableLayoutPanel.Width));
                        float newCol1WidthPct = _col1WidthPct + (_col2WidthPct - newCol2WidthPct);
                        if (newCol2WidthPct < MinColWidthPct || newCol1WidthPct < MinColWidthPct)
                            return;
                        _col2WidthPct = newCol2WidthPct;
                        _col1WidthPct = newCol1WidthPct;
                        UpdateColumnWidths();
                        break;
                    }
                }
            }
        }

        private void tableLayoutPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsInResizableColumns(e.Location))
                return;

            int beam0 = (int)tableLayoutPanel.ColumnStyles[0].Width;
            int beam1 = (int)(tableLayoutPanel.ColumnStyles[0].Width + tableLayoutPanel.ColumnStyles[1].Width);
            if (beam0.IsBetween(e.X - 20, e.X + 20))
                _beamCurrentlyMoving = 0;
            else if (beam1.IsBetween(e.X - 20, e.X + 20))
                _beamCurrentlyMoving = 1;
        }

        private void tableLayoutPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _beamCurrentlyMoving = -1;
            this.Cursor = Cursors.Default;
        }

        private bool IsInResizableColumns(Point p)
        {
            var textAreaTopLeft = new Point(rtbVanilla.Left + 1, rtbVanilla.Top + 1);
            var textAreaBottomRight = new Point(rtbRight.Right - 1, rtbRight.Bottom - 1);
            return 
                p.X.IsBetween(textAreaTopLeft.X, textAreaBottomRight.X) &&
                p.Y.IsBetween(textAreaTopLeft.Y, textAreaBottomRight.Y);
            
        }

        private void UpdateColumnWidths()
        {
            tableLayoutPanel.ColumnStyles[0].Width = tableLayoutPanel.Width * _col0WidthPct;
            tableLayoutPanel.ColumnStyles[1].Width = tableLayoutPanel.Width * _col1WidthPct;
            tableLayoutPanel.ColumnStyles[2].Width = tableLayoutPanel.Width * _col2WidthPct;
            tableLayoutPanel.Update();
        }

        private void ConflictResolver_Resize(object sender, EventArgs e)
        {
            UpdateColumnWidths();
        }

        private void ConflictResolver_ResizeBegin(object sender, EventArgs e)
        {
            _col0WidthPct = (tableLayoutPanel.ColumnStyles[0].Width / tableLayoutPanel.Width);
            _col1WidthPct = (tableLayoutPanel.ColumnStyles[1].Width / tableLayoutPanel.Width);
            _col2WidthPct = (tableLayoutPanel.ColumnStyles[2].Width / tableLayoutPanel.Width);
        }

        #endregion
    }
}