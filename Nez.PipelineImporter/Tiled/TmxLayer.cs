using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    [XmlInclude(typeof(TmxTileLayer))]
    [XmlInclude(typeof(TmxImageLayer))]
    public abstract class TmxLayer
    {
        [XmlAttribute(AttributeName = "name")] public string name;

        [XmlAttribute(AttributeName = "offsetx")]
        public float offsetx;

        [XmlAttribute(AttributeName = "offsety")]
        public float offsety;

        [XmlAttribute(AttributeName = "opacity")]
        public float opacity = 1f;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties = new List<TmxProperty>();

        [XmlAttribute(AttributeName = "visible")]
        public bool visible = true;


        public override string ToString()
        {
            return string.Format("[TmxLayer] name: {0}, visible: {1}", name, visible);
        }
    }
}