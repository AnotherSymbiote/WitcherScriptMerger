using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class MergeInventory
    {
        [XmlElement("Merge")]
        public ObservableCollection<Merge> Merges { get; private set; }

        [XmlIgnore]
        public bool ScriptsChanged { get; private set; }

        [XmlIgnore]
        public bool XmlChanged { get; private set; }

        [XmlIgnore]
        public bool BundleChanged { get; private set; }

        [XmlIgnore]
        public bool HasChanged => (ScriptsChanged || XmlChanged || BundleChanged);

        static XmlSerializer _serializer = new XmlSerializer(typeof(MergeInventory));

        public MergeInventory()
        {
            Merges = new ObservableCollection<Merge>();
            Merges.CollectionChanged += Merges_CollectionChanged;
        }

        public static MergeInventory Load(string path)
        {
            MergeInventory inventory;
            try
            {
                _serializer = new XmlSerializer(typeof(MergeInventory));
                using (var stream = File.OpenRead(path))
                {
                    inventory = (MergeInventory)_serializer.Deserialize(stream);
                }

                AddMissingHashes(inventory);
            }
            catch
            {
                inventory = new MergeInventory();
            }
            inventory.ScriptsChanged = inventory.XmlChanged = inventory.BundleChanged = false;
            return inventory;
        }

        void Merges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.NewItems != null && e.NewItems.Cast<Merge>().Any(merge => merge.Category == Categories.Script)) ||
                (e.OldItems != null && e.OldItems.Cast<Merge>().Any(merge => merge.Category == Categories.Script)))
                ScriptsChanged = true;
            if ((e.NewItems != null && e.NewItems.Cast<Merge>().Any(merge => merge.Category == Categories.Xml)) ||
                (e.OldItems != null && e.OldItems.Cast<Merge>().Any(merge => merge.Category == Categories.Xml)))
                XmlChanged = true;
            if ((e.NewItems != null && e.NewItems.Cast<Merge>().Any(merge => merge.IsBundleContent)) ||
                (e.OldItems != null && e.OldItems.Cast<Merge>().Any(merge => merge.IsBundleContent)))
                BundleChanged = true;
        }

        public void AddModToMerge(FileMerger.MergeSource source, Merge m)
        {
            string modFilePath =
                m.IsBundleContent
                ? source.Bundle.FullName
                : source.TextFile.FullName;

            m.Mods.Add(
                new FileHash
                {
                    Hash = xxHash.ComputeHashHex(modFilePath),
                    Name = source.Name
                });

            if (m.Category == Categories.Script)
                ScriptsChanged = true;
            else if (m.Category == Categories.Xml)
                XmlChanged = true;
            else if (m.IsBundleContent)
                BundleChanged = true;
        }

        public void Save()
        {
            if (_serializer == null)
                return;
            using (var writer = new StreamWriter(Paths.Inventory))
            {
                _serializer.Serialize(writer, this);
            }
        }

        public bool HasResolvedConflict(string relPath, string modName)
        {
            var merge = Merges.FirstOrDefault(mrg => mrg.RelativePath.EqualsIgnoreCase(relPath));
            if (merge == null)
                return false;

            var mod = merge.Mods.FirstOrDefault(m => m.Name.EqualsIgnoreCase(modName));
            if (mod == null)
                return false;

            if (new LoadOrderComparer().Compare(merge.MergedModName, modName) > 0)
                return false;

            var latestHash = xxHash.ComputeHashHex(merge.GetModFile(modName));
            return (mod.Hash == latestHash);
        }

        // Adds file hashes to old inventories that don't have them
        static void AddMissingHashes(MergeInventory inventory)
        {
            bool anyMissing = false;

            foreach (var merge in inventory.Merges)
            {
                foreach (var mod in merge.Mods)
                {
                    if (mod.Hash == null)
                    {
                        anyMissing = true;
                        mod.Hash = xxHash.ComputeHashHex(merge.GetModFile(mod.Name));
                    }
                }
            }

            if (anyMissing)
                inventory.Save();
        }
    }
}
