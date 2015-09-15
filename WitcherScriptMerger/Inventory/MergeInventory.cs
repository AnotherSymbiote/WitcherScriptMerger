using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WitcherScriptMerger.FileIndex;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class MergeInventory
    {
        [XmlElement(ElementName="Merge")]
        public ObservableCollection<Merge> Merges;

        [XmlIgnore]
        public bool ScriptsChanged { get; private set; }

        [XmlIgnore]
        public bool BundleChanged { get; private set; }

        [XmlIgnore]
        public bool HasChanged { get { return ScriptsChanged || BundleChanged; } }

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
                var serializer = new XmlSerializer(typeof(MergeInventory));
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    inventory = (MergeInventory)serializer.Deserialize(stream);
                }
            }
            catch
            {
                inventory = new MergeInventory
                {
                    Merges = new ObservableCollection<Merge>()
                };
            }
            inventory.ScriptsChanged = inventory.BundleChanged = false;
            return inventory;
        }

        public void Merges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.NewItems != null && e.NewItems.Cast<Merge>().Any(merge => merge.Type == ModFileType.Script)) ||
                (e.OldItems != null && e.OldItems.Cast<Merge>().Any(merge => merge.Type == ModFileType.Script)))
                ScriptsChanged = true;
            if ((e.NewItems != null && e.NewItems.Cast<Merge>().Any(merge => merge.Type == ModFileType.BundleContent)) ||
                (e.OldItems != null && e.OldItems.Cast<Merge>().Any(merge => merge.Type == ModFileType.BundleContent)))
                BundleChanged = true;
        }

        public void Save(string path)
        {
            var serializer = new XmlSerializer(typeof(MergeInventory));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }

        public bool HasResolvedConflict(string relPath, string modName)
        {
            return Merges.Any(ms =>
                ms.RelativePath == relPath &&
                ms.ModNames.Contains(modName) &&
                ms.MergedModName.CompareTo(modName) <= 0);
        }
    }
}
