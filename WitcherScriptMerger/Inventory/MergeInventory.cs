using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class MergeInventory
    {
        [XmlElement(ElementName="Merge")]
        public List<Merge> Merges;

        public static MergeInventory Load(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(MergeInventory));
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    return (MergeInventory)serializer.Deserialize(stream);
                }
            }
            catch (System.Exception ex)
            {
                string wat = ex.Message;
                return new MergeInventory
                {
                    Merges = new List<Merge>()
                };
            }
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
