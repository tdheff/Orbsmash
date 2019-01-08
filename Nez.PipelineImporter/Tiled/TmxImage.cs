using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    public class TmxImage
    {
        [XmlElement(ElementName = "data")] public TmxData data;

        [XmlAttribute(AttributeName = "format")]
        public string format;

        [XmlAttribute(AttributeName = "height")]
        public int height;

        [XmlAttribute(AttributeName = "source")]
        public string source;

        [XmlAttribute(AttributeName = "trans")]
        public string trans;

        [XmlAttribute(AttributeName = "width")]
        public int width;


        public override string ToString()
        {
            return source;
        }
    }
}