using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    public class TmxTileLayer : TmxLayer
    {
        [XmlElement(ElementName = "data")] public TmxData data;

        [XmlAttribute(AttributeName = "height")]
        public int height;

        [XmlAttribute(AttributeName = "width")]
        public int width;

        [XmlAttribute(AttributeName = "x")] public int x;

        [XmlAttribute(AttributeName = "y")] public int y;
    }
}