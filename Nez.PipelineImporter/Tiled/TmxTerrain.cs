using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    public class TmxTerrain
    {
        [XmlAttribute(AttributeName = "name")] public string name;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties = new List<TmxProperty>();

        [XmlAttribute(AttributeName = "tile")] public int tileId;


        public override string ToString()
        {
            return name;
        }
    }
}