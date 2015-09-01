using GoogleDiffMatchPatch;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WitcherScriptMerger
{
    public class Changeset
    {
        #region Types
        public struct ChangesetResult
        {
            public string Text;
            public int AttemptCount;
            public int SuccessCount;
            public int FailureCount
            {
                get { return AttemptCount - SuccessCount; }
            }
        }
        #endregion

        #region Members

        public List<SMPatch> Patches { get; set; }
        public string ChangedText { get; private set; }
        public string FileName { get; private set; }
        public string ModName { get; private set; }

        #endregion

        public Changeset(string vanillaText, FileInfo changedFile)
        {
            ChangedText = File.ReadAllText(changedFile.FullName);
            
            var differ = new DiffMatchPatch();
            var diffs = differ.DiffMain(vanillaText, ChangedText);
            var patchList = differ.PatchMake(vanillaText, diffs);

            // Removing these won't affect any positions — they're empty & thus have no deltas
            patchList = patchList.Where(p => !p.Diffs.AreOnlyEqualOrEmpty()).ToList();

            Patches = new List<SMPatch>();
            foreach (Patch p in patchList)
                Patches.Add(SMPatch.FromPatch(p, this));

            FileName = changedFile.Name;
            ModName = ModDirectory.GetModName(changedFile);
        }

        public Changeset(List<SMPatch> patches, string fileName, string modName)
        {
            Patches = patches;
            FileName = fileName;
            ModName = modName;
        }

        public ChangesetResult TryApply(string vanillaText)
        {
            List<Patch> patchList = Patches.Select(smp => (Patch)smp).ToList();
            var result = new DiffMatchPatch().PatchApply(patchList, vanillaText);
            var resultText = (string)result[0];
            var patchResults = (bool[])result[1];
            return
                new ChangesetResult
                {
                    Text = resultText,
                    AttemptCount = patchResults.Length,
                    SuccessCount = patchResults.Count(r => r == true)
                };
        }
    }
}