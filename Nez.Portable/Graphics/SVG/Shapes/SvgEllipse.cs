using System.Xml.Serialization;

namespace Nez.Svg
{
    public class SvgEllipse : SvgElement
    {
        [XmlAttribute("cx")] public float centerX;

        [XmlAttribute("cy")] public float centerY;

        [XmlAttribute("rx")] public float radiusX;

        [XmlAttribute("ry")] public float radiusY;
    }
}