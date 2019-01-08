using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Nez.TiledMaps
{
    public class TmxTilesetTile
    {
        [XmlArray("animation")] [XmlArrayItem("frame")]
        public List<TmxTilesetTileAnimationFrame> animationFrames;

        [XmlAttribute(AttributeName = "id")] public int id;

        [XmlElement(ElementName = "terrain")] public TmxTerrain terrain;

        [XmlElement(ElementName = "image")] public TmxImage image;

        [XmlElement(ElementName = "objectgroup")]
        public List<TmxObjectGroup> objectGroups;

        [XmlAttribute(AttributeName = "probability")]
        public float probability = 1f;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties = new List<TmxProperty>();

        /// <summary>
        ///     source Rectangle for tilesets that use the collection of images
        /// </summary>
        public Rectangle sourceRect;


        public override string ToString()
        {
            return string.Format("[TmxTilesetTile] id: {0}, animationFrames: {1}, image: {2}", id,
                animationFrames.Count, image);
        }
    }
}