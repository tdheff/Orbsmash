using System.Xml.Serialization;

namespace Nez.Svg
{
    public class SvgCircle : SvgElement
    {
        [XmlAttribute("cx")] public float centerX;

        [XmlAttribute("cy")] public float centerY;

        [XmlAttribute("r")] public float radius;
    }
}