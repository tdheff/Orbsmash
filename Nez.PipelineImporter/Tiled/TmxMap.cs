using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    [XmlRoot(ElementName = "map")]
    public class TmxMap
    {
        [XmlAttribute(AttributeName = "backgroundcolor")]
        public string backgroundColor;

        [XmlAttribute(AttributeName = "firstgid")]
        public int firstGid;

        [XmlAttribute(AttributeName = "height")]
        public int height;

        [XmlElement(ElementName = "tileset")] public List<TmxTileset> tilesets;

        [XmlElement(ElementName = "objectgroup")]
        public List<TmxObjectGroup> objectGroups;

        [XmlElement(ElementName = "layer", Type = typeof(TmxTileLayer))]
        [XmlElement(ElementName = "imagelayer", Type = typeof(TmxImageLayer))]
        public List<TmxLayer> layers;

        [XmlAttribute("nextobjectid")] public int nextObjectId;


        [XmlAttribute(AttributeName = "orientation")]
        public TmxOrientation orientation;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties;

        [XmlAttribute(AttributeName = "renderorder")]
        public TmxRenderOrder renderOrder;

        [XmlAttribute(AttributeName = "tileheight")]
        public int tileHeight;


        [XmlAttribute(AttributeName = "tilewidth")]
        public int tileWidth;

        [XmlAttribute(AttributeName = "version")]
        public string version;

        [XmlAttribute(AttributeName = "width")]
        public int width;

        public TmxMap()
        {
            properties = new List<TmxProperty>();
            tilesets = new List<TmxTileset>();
            layers = new List<TmxLayer>();
            objectGroups = new List<TmxObjectGroup>();
        }
    }
}