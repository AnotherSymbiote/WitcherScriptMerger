using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    // Combines two lists of patches into a single list, with all position data
    // corrected for any preceding patches from the opposite list.
    public class ChangesetMerger
    {
        #region Members

        public string VanillaText { get; set; }
        public Changeset Set1 { get; set; }
        public Changeset Set2 { get; set; }

        private List<SMPatch> _combinedList;
        private int _index1 = 0;
        private int _index2 = 0;
        private int _deltaFor1From2 = 0;
        private int _deltaFor2From1 = 0;

        #endregion

        public ChangesetMerger(Changeset set1, Changeset set2, string vanillaText)
        {
            Set1 = set1;
            Set2 = set2;
            VanillaText = vanillaText;
        }

        public Changeset Merge()
        {
            _combinedList = new List<SMPatch>();

            SMPatch patch1 = (Set1.Patches.Any() ? Set1.Patches[_index1] : null);
            SMPatch patch2 = (Set2.Patches.Any() ? Set2.Patches[_index2] : null);
            while (patch1 != null || patch2 != null)  // While there are still patches to merge
            {
                bool foundWhitespacePatch = false;
                if (Program.MainForm.IsIgnoreWhitespaceEnabled())
                {
                    if (patch1 != null && patch1.Diffs.AreOnlyWhitespace())
                    {
                        IgnorePatch(patch1);
                        foundWhitespacePatch = true;
                    }
                    if (patch2 != null && patch2.Diffs.AreOnlyWhitespace())
                    {
                        IgnorePatch(patch2);
                        foundWhitespacePatch = true;
                    }
                }
                
                if (!foundWhitespacePatch)
                {
                    SMPatch chosenPatch = null;
                    if (patch1 != null && patch2 != null)
                        chosenPatch = ChooseNextFromPair(patch1, patch2);
                    else if (patch1 != null)
                        chosenPatch = patch1;
                    else
                        chosenPatch = patch2;

                    if (chosenPatch != null)
                        AddChosenPatch(chosenPatch);
                }

                if (_index1 < Set1.Patches.Count)
                    patch1 = Set1.Patches[_index1];
                else
                    patch1 = null;

                if (_index2 < Set2.Patches.Count)
                    patch2 = Set2.Patches[_index2];
                else
                    patch2 = null;
            }

            return new Changeset(_combinedList, Set1.FileName, Set1.ModName);
        }

        private void AddChosenPatch(SMPatch p)
        {
            if (p.Changeset == Set1)
            {
                _deltaFor2From1 += p.Delta;
                p.ShiftStart(_deltaFor1From2);
                ++_index1;
            }
            else
            {
                _deltaFor1From2 += p.Delta;
                p.ShiftStart(_deltaFor2From1);
                ++_index2;
            }
            _combinedList.Add(p);
        }

        private void IgnorePatch(SMPatch p)
        {
            // Adjust subsequent patches in the patch's changeset for its discarded delta
            if (p.Changeset == Set1)
            {
                _deltaFor1From2 -= p.Delta;
                ++_index1;
            }
            else
            {
                _deltaFor2From1 -= p.Delta;
                ++_index2;
            }
        }

        private SMPatch ChooseNextFromPair(SMPatch patch1, SMPatch patch2)
        {
            // If both patches are the same, just apply 1st one
            if (patch1.VanillaRange.Equals(patch2.VanillaRange) &&
                patch1.Diffs.Count == patch2.Diffs.Count &&
                patch1.Diffs.All(d1 => patch2.Diffs.Any(d2 => d1.Operation == d2.Operation && d1.Text == d2.Text)))
            {
                IgnorePatch(patch2);
                return patch1;
            }
            else if (patch1.VanillaRange.OverlapsWith(patch2.VanillaRange))  // If patches overlap
            {
                var trimmedVanillaRange1 = patch1.GetTrimmedVanillaRange();   // even after
                var trimmedVanillaRange2 = patch2.GetTrimmedVanillaRange();   // trimming
                if (trimmedVanillaRange1.OverlapsWith(trimmedVanillaRange2))  // equalities
                    return ChooseNextManually(patch1, patch2, trimmedVanillaRange1, trimmedVanillaRange2);  // Prompt for choice
                else
                {
                    int origDelta = patch1.Delta;  // If only equalities or inserts overlap, add
                    patch1.Assimilate(patch2);     // non-overlapping parts of patch2 into patch1

                    // Need to add patch HERE in order to:
                    //   1) Insert patch1
                    //   2) THEN adjust subsequent set1 patches for its newly added chars
                    // Otherwise patch1 would get adjusted too, in Merge() > AddChosenPatch()!
                    AddChosenPatch(patch1);
                    int lengthChange = patch1.Delta - origDelta;
                    _deltaFor2From1 -= lengthChange;  // Adjust set2 for change in patch1 length (prevent double-counting added chars)
                    _deltaFor1From2 += lengthChange;  // Adjust set1 for change in patch1 length
                    ++_index2;    // Skip patch2
                    return null;  // Already added here! Choose neither
                }
            }
            return (patch1.VanillaRange.Start < patch2.VanillaRange.Start
                ? patch1
                : patch2);
        }

        private SMPatch ChooseNextManually(
            SMPatch patch1,
            SMPatch patch2,
            Range trimmedVanillaRange1,
            Range trimmedVanillaRange2)
        {
            // Handle overlapping patches manually
            using (var resolveForm = new ResolveConflictForm(Set1.FileName, Set1.ModName, Set2.ModName))
            {
                var combinedVanillaRange = new Range(
                    Math.Min(trimmedVanillaRange1.Start, trimmedVanillaRange2.Start),
                    Math.Max(trimmedVanillaRange1.End, trimmedVanillaRange2.End));

                resolveForm.SetVanillaText(VanillaText, combinedVanillaRange);
                resolveForm.SetLeftText(Set1.ChangedText, patch1.GetTrimmedResultRange(), patch1);
                resolveForm.SetRightText(Set2.ChangedText, patch2.GetTrimmedResultRange(), patch2);

                var result = resolveForm.ShowDialog();

                switch (result)
                {
                    case (DialogResult)ResolveConflictForm.ConflictResolution.Vanilla:
                        IgnorePatch(patch1);
                        IgnorePatch(patch2);
                        break;
                    case (DialogResult)ResolveConflictForm.ConflictResolution.Left:
                        IgnorePatch(patch2);
                        return patch1;
                    case (DialogResult)ResolveConflictForm.ConflictResolution.Right:
                        IgnorePatch(patch1);
                        return patch2;
                }
            }
            return null;
        }
    }
}