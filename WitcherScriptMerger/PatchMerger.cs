using GoogleDiffMatchPatch;
using System.Linq;

namespace WitcherScriptMerger
{
    public class PatchMerger
    {
        public SMPatch Patch1 { get; set; }
        public SMPatch Patch2 { get; set; }

        private bool IsPatch1AtLeft
        {
            get { return (Patch1.VanillaRange.Start <= Patch2.VanillaRange.Start); }
        }

        private int _vanillaDiffPos1;
        private int _vanillaDiffPos2;
        private int _lastInsertPos;
        private int _addedLength;

        public PatchMerger(SMPatch patch1, SMPatch patch2)
        {
            Patch1 = patch1;
            Patch2 = patch2;
        }

        public SMPatch Merge()
        {
            _vanillaDiffPos1 = Patch1.VanillaRange.Start;
            _vanillaDiffPos2 = Patch2.VanillaRange.Start;
            _lastInsertPos = 0;
            _addedLength = 0;
            for (int diff2Index = 0; diff2Index < Patch2.Diffs.Count; ++diff2Index)
            {
                var diff2 = Patch2.Diffs[diff2Index];

                for (int insertPos = _lastInsertPos; insertPos <= Patch1.Diffs.Count; ++insertPos)
                {
                    if (insertPos == Patch1.Diffs.Count)  // Now we're past Patch1
                    {
                        InsertNewDiff(insertPos, diff2);  // so just stick #2 at the end
                        break;
                    }
                    
                    var diff1 = Patch1.Diffs[insertPos];

                    var diffRange1 = Range.WithLength(_vanillaDiffPos1,
                        (diff1.Operation != Operation.INSERT ? diff1.Text.Length : 0));
                    var diffRange2 = Range.WithLength(_vanillaDiffPos2,
                        (diff2.Operation != Operation.INSERT ? diff2.Text.Length : 0));
                    if (!diffRange1.OverlapsWith(diffRange2))
                    {
                        if (_vanillaDiffPos1 < _vanillaDiffPos2)  // If #2 comes after #1 without overlap, keep looking
                        {
                            if (diff1.Operation != Operation.INSERT)
                            {
                                _vanillaDiffPos1 += diff1.Text.Length;
                                ++_lastInsertPos;
                            }
                            continue;
                        }
                        // If #2 comes first without overlap & it's not a frivolous equality in middle of Patch1, insert
                        if (diff2.Operation != Operation.EQUAL || !Patch1.VanillaRange.Contains(diffRange2))
                            InsertNewDiff(insertPos, diff2);
                        break;
                    }
                    else
                    {
                        HandleOverlap(diff1, diff2, diffRange1, diffRange2, insertPos);
                        break;
                    }
                }
                if (diff2.Operation != Operation.INSERT)
                    _vanillaDiffPos2 += diff2.Text.Length;
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

        private bool HandleOverlap(Diff diff1, Diff diff2, Range diffRange1, Range diffRange2, int insertPos)
        {
            if (diff2.Operation == Operation.EQUAL)
            {
                // If #2 is equality & doesn't protrude from start or end of Patch1, just throw it out
                if (Patch1.VanillaRange.Contains(diffRange2))
                    return false;
                // If #2 is leading equality & overlaps with anything other than end of Patch1, throw it out
                if (Patch2.StartsWith(diff2) && !Patch1.EndsWith(diff1))
                    return false;

                // So only insert/join #2 if it's a non-leading equality overlapping left of Patch1,
                // or it's any equality overlapping right of Patch1
            }

            if (diff2.Operation == Operation.INSERT)
            {
                // 2 inserts starting at same position (no real overlap)
                if (diff1.Operation == Operation.INSERT && diffRange1.Start == diffRange2.Start)
                {
                    JoinInserts(diff1, diff2);       // If inserts lie next to each other, we can safely join
                    _lastInsertPos = insertPos + 1;  // them into one bigger insert & advance past it
                    return true;
                }
                // Insertion right before or after #1 (no real overlap)
                if (diffRange1.Start == diffRange2.Start)
                {
                    InsertNewDiff(insertPos, diff2, false);
                    return true;
                }
                if (diffRange1.End == diffRange2.Start)
                {
                    InsertNewDiff(insertPos + 1, diff2, false);
                    if (diff1.Operation != Operation.INSERT)
                        _vanillaDiffPos1 += diff1.Text.Length;
                    return true;
                }
            }

            if (diffRange1.Contains(diffRange2))
            {
                if (diff2.Operation == Operation.INSERT)  // We know #1 must be equality
                {
                    PutInsertIntoEquality(diff1, diff2, diffRange1, diffRange2, insertPos);
                    return true;
                }
                if (diff2.Operation == Operation.DELETE)  // We know #1 must be equality
                {
                    PutDeleteIntoEquality(diff1, diff2, diffRange1, diffRange2, insertPos);
                    return true;
                }
            }

            ////if (diffRange2.Contains(diffRange1))
            ////{
            ////}
            
            // Otherwise if diffs overlap, we can shrink an equality or join 2 together
            bool joinedEqualities = AdjustEqualitiesToFit(diff1, diff2);
            if (!joinedEqualities)
                InsertNewDiff(insertPos, diff2);  // Now there's room for diff2 in front
            return true;
        }

        private void InsertNewDiff(int insertPos, Diff diff, bool addsToLength = true)
        {
            Patch1.Diffs.Insert(insertPos, diff);
            if (addsToLength)
                _addedLength += diff.Text.Length;
            _lastInsertPos = insertPos + 1;
        }

        private void PutInsertIntoEquality(Diff diff1, Diff diff2, Range diffRange1, Range diffRange2, int insertPos)
        {
            int insertIndex = (diffRange2.Start - diffRange1.Start);
            string diff1Txt = diff1.Text;
            diff1.Text = diff1Txt.Substring(0, insertIndex);  // Trim equality to left portion

            // Put new insertion diff in middle of equality
            InsertNewDiff(insertPos + 1, diff2, false);

            // Add back the right portion of equality as a separate diff
            Patch1.Diffs.Insert(insertPos + 2, new Diff(Operation.EQUAL, diff1Txt.Substring(insertIndex)));

            // Advance past 1st portion of #1
            _vanillaDiffPos1 += diff1.Text.Length;
        }

        private void PutDeleteIntoEquality(Diff diff1, Diff diff2, Range diffRange1, Range diffRange2, int insertPos)
        {
            int deleteStart = diffRange2.Start - diffRange1.Start;
            string diff1Txt = diff1.Text;
            diff1.Text = diff1Txt.Substring(0, deleteStart);  // Trim equality to left portion

            // Put new deletion diff in middle of equality
            InsertNewDiff(insertPos + 1, diff2, false);

            // Add back the right portion of equality as a separate diff, if it's not empty now
            int deleteEnd = diffRange2.End - diffRange1.Start;
            if (deleteEnd < diff1Txt.Length)
                Patch1.Diffs.Insert(insertPos + 2, new Diff(Operation.EQUAL, diff1Txt.Substring(deleteEnd)));

            // Advance past 1st portion of #1 & the new delete
            _vanillaDiffPos1 += diff1.Text.Length + diff2.Text.Length;
        }

        private void JoinInserts(Diff insert1, Diff insert2)
        {
            insert1.Text += insert2.Text;
            _vanillaDiffPos1 += insert2.Text.Length;
            _addedLength += insert2.Text.Length;  // New text added
        }

        private bool AdjustEqualitiesToFit(Diff diff1, Diff diff2)
        {
            bool joinedEqualities = false;

            bool isDiff1AtLeft = (_vanillaDiffPos1 <= _vanillaDiffPos2);
            
            int overlapIndex = FindOverlapIndex(diff1, diff2, isDiff1AtLeft);

            if (diff1.Operation == Operation.EQUAL)
            {
                if (diff2.Operation == Operation.EQUAL)  // Join 2 equalities: #2 into #1
                {
                    string textFrom2;
                    if (isDiff1AtLeft)
                    {
                        textFrom2 = diff2.Text.Substring(overlapIndex);
                        diff1.Text += textFrom2;
                    }
                    else
                    {
                        textFrom2 = diff2.Text.Substring(0, diff2.Text.Length - overlapIndex);
                        diff1.Text = textFrom2 + diff1.Text;
                    }
                    _vanillaDiffPos1 -= textFrom2.Length;  // For vanilla pos, discount the added chars that would otherwise be counted when adding diff1.Text.Length later
                    joinedEqualities = true;
                    _addedLength += textFrom2.Length;  // New text added
                }
                else
                {
                    if (isDiff1AtLeft)
                        diff1.Text = diff1.Text.Substring(0, overlapIndex);  // Right-shrink equal text
                    else
                        diff1.Text = diff1.Text.Substring(overlapIndex);     // Left-shrink equal text
                    _vanillaDiffPos1 += overlapIndex;  // Must count trimmed chars, plus diff1.Text.Length later
                    _addedLength -= overlapIndex;  // Text was removed (but will be added back in Merge())
                }
            }
            else
            {
                int indexFromEnd = diff2.Text.Length - overlapIndex;
                if (isDiff1AtLeft)
                    diff2.Text = diff2.Text.Substring(indexFromEnd);     // Left-shrink equal text
                else
                    diff2.Text = diff2.Text.Substring(0, indexFromEnd);  // Right-shrink equal text
                _vanillaDiffPos2 += overlapIndex;  // Must count trimmed chars, plus diff2.Text.Length later
                _addedLength -= overlapIndex;  // Text was removed (but will be added back in Merge())
            }
            return joinedEqualities;
        }

        private int FindOverlapIndex(Diff diff1, Diff diff2, bool isDiff1AtLeft)
        {
            Diff left = (isDiff1AtLeft ? diff1 : diff2);
            Diff right = (isDiff1AtLeft ? diff2 : diff1);

            bool isLeftLonger = (left.Text.Length > right.Text.Length);
            int substrIndex = (isLeftLonger ? right.Text.Length : left.Text.Length );
            string leftSubstr;
            string rightSubstr;
            while (true)
            {
                rightSubstr = right.Text.Substring(0, substrIndex);
                leftSubstr = left.Text.Substring(left.Text.Length - substrIndex);
                if (rightSubstr == leftSubstr)
                    break;
                --substrIndex;
            }
            return substrIndex;
        }
    }
}
