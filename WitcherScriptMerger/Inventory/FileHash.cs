using System.Xml.Serialization;

namespace WitcherScriptMerger.Inventory
{
    [XmlRoot]
    public class FileHash
    {
        [XmlAttribute]
        public string Hash { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}
