using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WitcherScriptMerger.FileIndex;
using WitcherScriptMerger.LoadOrderValidation;

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
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    inventory = (MergeInventory)_serializer.Deserialize(stream);
                }
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

        public void AddModToMerge(string modName, Merge m)
        {
            m.ModNames.Add(modName);
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
            return Merges.Any(merge =>
                merge.RelativePath.EqualsIgnoreCase(relPath) &&
                merge.ModNames.Contains(modName) &&
                LoadOrderValidator.GetModNameLoadOrder(merge.MergedModName, modName) < 0);
        }
    }
}
