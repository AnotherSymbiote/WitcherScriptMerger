using GoogleDiffMatchPatch;
using System.Collections.Generic;
using System.Linq;

namespace WitcherScriptMerger
{
    public class SMPatch
    {
        #region Members

        public List<Diff> Diffs { get; set; }
        public Changeset Changeset { get; set; }
        public int Delta { get { return (ResultLength - TargetLength); } }

        public int ChangesetOffset { get; private set; }
        public int OtherOffset { get; private set; }

        public int TargetStart { get; private set; }
        public int TargetLength { get; set; }
        public int ResultStart { get; private set; }
        public int ResultLength { get; set; }

        public Range VanillaRange
        {
            get { return Range.WithLength(TargetStart - ChangesetOffset - OtherOffset, TargetLength); }
        }

        public Range TargetRange
        {
            get { return Range.WithLength(TargetStart, TargetLength); }
        }

        public Range ResultRange
        {
            get { return Range.WithLength(ResultStart, ResultLength); }
        }
        
        #endregion

        public SMPatch(Changeset changeset, int changesetOffset)
        {
            Changeset = changeset;
            ChangesetOffset = changesetOffset;
        }

        public static implicit operator Patch(SMPatch smp)
        {
            return
                new Patch
                {
                    Diffs = smp.Diffs,
                    Start1 = smp.TargetStart,
                    Start2 = smp.ResultStart,
                    Length1 = smp.TargetLength,
                    Length2 = smp.ResultLength,
                };
        }

        public static SMPatch FromPatch(Patch p, Changeset changeset)
        {
            int offset = changeset.Patches.Sum(existing => existing.Delta);
            var smp = new SMPatch(changeset, offset);
            smp.TargetStart = p.Start1;
            smp.ResultStart = p.Start2;
            smp.TargetLength = p.Length1;
            smp.ResultLength = p.Length2;
            smp.Diffs = p.Diffs;
            return smp;
        }

        public void AddChangesetOffset(int offset)
        {
            ChangesetOffset += offset;
        }

        public void SetStart(int start)
        {
            int change = start - TargetStart;
            TargetStart = start;
            ResultStart = start;
            OtherOffset += change;
        }

        public void ShiftStart(int shift)
        {
            TargetStart += shift;
            ResultStart += shift;
            OtherOffset += shift;
        }

        public SMPatch Assimilate(SMPatch other)
        {
            return new PatchMerger(this, other).Merge();
        }

        public Range GetTrimmedResultRange()
        {
            return GetTrimmedRange(ResultRange);
        }

        public Range GetTrimmedVanillaRange()
        {
            var range = GetTrimmedRange(VanillaRange);
            range.Start -= OtherOffset;
            range.End -= OtherOffset;
            return range;
        }

        private Range GetTrimmedRange(Range range)
        {
            for (int i = 0; i < Diffs.Count; ++i)  // Start from beginning
            {
                var diff = Diffs[i];
                if (diff.Operation == Operation.EQUAL)
                    range.Start += diff.Text.Length;
                else
                    break;
            }
            for (int i = Diffs.Count - 1; i >= 0; --i)  // Start from end
            {
                var diff = Diffs[i];
                if (diff.Operation == Operation.EQUAL)
                    range.End -= diff.Text.Length;
                else
                    break;
            }
            return range;
        }

        public bool StartsWith(Diff diff)
        {
            return (Diffs.Count > 0 && Diffs.First() == diff);
        }

        public bool EndsWith(Diff diff)
        {
            return (Diffs.Count > 0 && Diffs.Last() == diff);
        }

        public override string ToString()
        {
            return ((Patch)this).ToString();
        }
    }

    public static class PatchExtensions
    {
        public static bool IsOnlyEqualOrEmpty(this Patch p)
        {
            return p.Diffs.All(d =>
                d.Operation == Operation.EQUAL ||
                string.IsNullOrEmpty(d.Text));
        }

        public static bool IsOnlyWhitespace(this Patch p)
        {
            return p.Diffs.All(d =>
                d.Operation == Operation.EQUAL ||
                string.IsNullOrWhiteSpace(d.Text));
        }
    }
}
