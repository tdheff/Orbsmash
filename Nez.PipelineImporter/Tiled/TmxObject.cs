using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Nez.TiledMaps
{
    public class TmxObject
    {
        [XmlElement(ElementName = "image")] public TmxImage image;

        [XmlElement(ElementName = "ellipse")] public TmxEllipse ellipse;

        [XmlAttribute(DataType = "int", AttributeName = "gid")]
        public int gid;

        [XmlAttribute(DataType = "float", AttributeName = "height")]
        public float height;

        [XmlAttribute(DataType = "int", AttributeName = "id")]
        public int id;


        [XmlAttribute(DataType = "string", AttributeName = "name")]
        public string name;

        [XmlElement(ElementName = "polygon")] public TmxPolygon polygon;

        [XmlElement(ElementName = "polyline")] public TmxPolyline polyline;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties;

        [XmlAttribute(DataType = "int", AttributeName = "rotation")]
        public int rotation;

        [XmlAttribute(DataType = "string", AttributeName = "type")]
        public string type;

        [XmlAttribute(DataType = "boolean", AttributeName = "visible")]
        public bool visible = true;

        [XmlAttribute(DataType = "float", AttributeName = "width")]
        public float width;

        [XmlAttribute(DataType = "float", AttributeName = "x")]
        public float x;

        [XmlAttribute(DataType = "float", AttributeName = "y")]
        public float y;
    }


    public class TmxEllipse
    {
    }


    public class TmxPolygon
    {
        public List<Vector2> points = new List<Vector2>();

        [XmlAttribute(DataType = "string", AttributeName = "points")]
        public string tempPoints
        {
            get => string.Empty;
            set
            {
                var parts = value.Split(' ');
                foreach (var p in parts)
                {
                    var pair = p.Split(',');
                    points.Add(new Vector2(float.Parse(pair[0], CultureInfo.InvariantCulture),
                        float.Parse(pair[1], CultureInfo.InvariantCulture)));
                }
            }
        }


        public override string ToString()
        {
            return string.Format("[TmxPolygon] point count: {0}", points.Count);
        }
    }


    public class TmxPolyline : TmxPolygon
    {
        public override string ToString()
        {
            return string.Format("[TmxPolyline] point count: {0}", points.Count);
        }
    }
}