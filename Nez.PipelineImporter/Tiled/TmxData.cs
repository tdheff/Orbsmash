using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    public class TmxData
    {
        [XmlAttribute(AttributeName = "compression")]
        public string compression;


        [XmlAttribute(AttributeName = "encoding")]
        public string encoding;

        [XmlElement(ElementName = "tile")] public List<TmxDataTile> tiles = new List<TmxDataTile>();

        [XmlText] public string value;


        public override string ToString()
        {
            return string.Format("{0} {1}", encoding, compression);
        }
    }
}