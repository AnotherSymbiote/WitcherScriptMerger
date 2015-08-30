using GoogleDiffMatchPatch;
using System.Linq;

namespace WitcherScriptMerger
{
    public class PatchMerger
    {
        public SMPatch Patch1 { get; set; }
        public SMPatch Patch2 { get; set; }

        private int _vanillaPos1;
        private int _vanillaPos2;
        private int _lastInsertPos;
        private int _addedLength;

        public PatchMerger(SMPatch patch1, SMPatch patch2)
        {
            Patch1 = patch1;
            Patch2 = patch2;
        }

        public SMPatch Merge()
        {
            _vanillaPos1 = Patch1.VanillaRange.Start;
            _vanillaPos2 = Patch2.VanillaRange.Start;
            _lastInsertPos = 0;
            _addedLength = 0;
            for (int diff2Index = 0; diff2Index < Patch2.Diffs.Count; ++diff2Index)
            {
                var diff2 = Patch2.Diffs[diff2Index];

                bool foundPos = false;
                for (int insertPos = _lastInsertPos; insertPos < Patch1.Diffs.Count && !foundPos; ++insertPos)
                {
                    var diff1 = Patch1.Diffs[insertPos];

                    var changeRange1 = Range.WithLength(_vanillaPos1,
                        (diff1.Operation != Operation.INSERT ? diff1.Text.Length : 0));
                    var changeRange2 = Range.WithLength(_vanillaPos2,
                        (diff2.Operation != Operation.INSERT ? diff2.Text.Length : 0));
                    if (!changeRange1.OverlapsWith(changeRange2))
                    {
                        if (_vanillaPos1 < _vanillaPos2)  // If #2 comes after #1 without overlap, keep looking
                        {
                            if (diff1.Operation != Operation.INSERT)
                                _vanillaPos1 += diff1.Text.Length;
                            continue;
                        }
                        InsertNewDiff(insertPos, diff2);  // If #2 comes first & there's room, insert
                        _lastInsertPos = insertPos + 1;
                        foundPos = true;
                    }
                    else if (AreLeadingEqualities(diff1, diff2))  // If overlap & diffs are leading equalities,
                        break;                                    // just skip #2.
                    else if (AreSequentialInserts(diff1, diff2, changeRange1, changeRange2))
                    {
                        JoinInserts(diff1, diff2);
                        _lastInsertPos = insertPos + 1;
                        foundPos = true;
                    }
                    else if (diff2.Operation == Operation.INSERT && changeRange1.Contains(changeRange2.Start))
                    {
                        int insertIndex = (changeRange2.Start - changeRange1.Start);
                        string diff1Txt = diff1.Text;
                        diff1.Text = diff1Txt.Substring(0, insertIndex);  // Trim equality to left portion
                        
                        InsertNewDiff(insertPos + 1, diff2);  // Put new insertion diff in middle of equality
                        
                        // Add back the right portion of equality as a separate diff
                        Patch1.Diffs.Insert(insertPos + 2, new Diff(Operation.EQUAL, diff1Txt.Substring(insertIndex)));
                        _lastInsertPos = insertPos + 3;
                        foundPos = true;
                    }
                    else  // Otherwise if diffs overlap, we can shrink an equality or join 2 together
                    {
                        bool joinedEqualities = AdjustEqualitiesToFit(diff1, diff2);
                        if (!joinedEqualities)
                            InsertNewDiff(insertPos, diff2);  // Now there's room for diff2 in front
                        _lastInsertPos = insertPos + 1;
                        foundPos = true;
                    }
                }
                if (diff2.Operation != Operation.INSERT)
                    _vanillaPos2 += diff2.Text.Length;
            }

            Patch1.TargetLength = Patch1.Diffs
                .Where(d => d.Operation != Operation.INSERT)
                .Sum(d => d.Text.Length);
            Patch1.ResultLength = Patch1.Diffs
                .Where(d => d.Operation != Operation.DELETE)
                .Sum(d => d.Text.Length);

            if (Patch1.VanillaRange.Start > Patch2.VanillaRange.Start)  // #2 comes first, so #1 start moved up
                Patch1.SetStart(Patch1.TargetRange.Start - _addedLength);

            return Patch1;
        }

        private void InsertNewDiff(int insertPos, Diff diff)
        {
            Patch1.Diffs.Insert(insertPos, diff);
            _addedLength += diff.Text.Length;
        }

        private bool AreLeadingEqualities(Diff diff1, Diff diff2)
        {
            return _lastInsertPos == 0 &&
                diff1.Operation == Operation.EQUAL &&
                diff2.Operation == Operation.EQUAL;
        }

        private bool AreSequentialInserts(Diff diff1, Diff diff2, Range changeRange1, Range changeRange2)
        {
            return changeRange1.Start == changeRange2.Start &&  // Join 2 inserts starting at same position (no real overlap)
                diff1.Operation == Operation.INSERT &&
                diff2.Operation == Operation.INSERT;
        }

        private void JoinInserts(Diff insert1, Diff insert2)
        {
            insert1.Text += insert2.Text;
            _vanillaPos1 += insert2.Text.Length;
            _addedLength += insert2.Text.Length;  // New text added
        }

        private bool AdjustEqualitiesToFit(Diff diff1, Diff diff2)
        {
            bool joinedEqualities = false;

            bool is1AtLeft = (_vanillaPos1 < _vanillaPos2);
            
            int overlapIndex = FindOverlapIndex(diff1, diff2, is1AtLeft);

            if (diff1.Operation == Operation.EQUAL)
            {
                if (diff2.Operation == Operation.EQUAL)  // Join 2 equalities: #2 into #1
                {
                    string textFrom2;
                    if (is1AtLeft)
                    {
                        textFrom2 = diff2.Text.Substring(overlapIndex);
                        diff1.Text += textFrom2;
                    }
                    else
                    {
                        textFrom2 = diff2.Text.Substring(0, diff2.Text.Length - overlapIndex);
                        diff1.Text = textFrom2 + diff1.Text;
                    }
                    _vanillaPos1 -= textFrom2.Length;  // For vanilla pos, discount the added chars that would otherwise be counted when adding diff1.Text.Length later
                    joinedEqualities = true;
                    _addedLength += textFrom2.Length;  // New text added
                }
                else
                {
                    if (is1AtLeft)
                        diff1.Text = diff1.Text.Substring(0, overlapIndex);  // Right-shrink equal text
                    else
                        diff1.Text = diff1.Text.Substring(overlapIndex);     // Left-shrink equal text
                    _vanillaPos1 += overlapIndex;  // Must count trimmed chars, plus diff1.Text.Length later
                    _addedLength -= overlapIndex;  // Text was removed (but will be added back in Merge())
                }
            }
            else
            {
                int indexFromEnd = diff2.Text.Length - overlapIndex;
                if (is1AtLeft)
                    diff2.Text = diff2.Text.Substring(indexFromEnd);     // Left-shrink equal text
                else
                    diff2.Text = diff2.Text.Substring(0, indexFromEnd);  // Right-shrink equal text
                _vanillaPos2 += overlapIndex;  // Must count trimmed chars, plus diff2.Text.Length later
                _addedLength -= overlapIndex;  // Text was removed (but will be added back in Merge())
            }
            return joinedEqualities;
        }

        private int FindOverlapIndex(Diff diff1, Diff diff2, bool is1AtLeft)
        {
            Diff left = (is1AtLeft ? diff1 : diff2);
            Diff right = (is1AtLeft ? diff2 : diff1);

            bool is1Longer = (diff1.Text.Length > diff2.Text.Length);
            int diffSubIndex = (is1Longer ? 0 : diff1.Text.Length);
            string diff2Substr;
            string diff1Substr;
            while (true)
            {
                if (is1Longer)
                    ++diffSubIndex;
                else
                    --diffSubIndex;
                diff1Substr = diff1.Text.Substring(0, diffSubIndex);
                diff2Substr = diff2.Text.Substring(diff2.Text.Length - diffSubIndex);
                if (diff1Substr == diff2Substr)
                    break;
            }
            return diffSubIndex;
        }
    }
}
