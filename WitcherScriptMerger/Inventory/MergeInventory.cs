using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class MergeInventory
    {
        [XmlElement(ElementName="MergedScript")]
        public List<MergedScript> MergedScripts;

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
            catch
            {
                return new MergeInventory
                {
                    MergedScripts = new List<MergedScript>()
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
    }
}
