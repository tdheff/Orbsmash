using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Nez.PipelineImporter;

namespace Nez.TiledMaps
{
    [XmlRoot(ElementName = "tileset")]
    public class TmxTileset
    {
        [XmlAttribute(AttributeName = "columns")]
        public int columns;

        [XmlAttribute(AttributeName = "firstgid")]
        public int firstGid;

        [XmlElement(ElementName = "tileoffset")]
        public TmxTileOffset tileOffset;

        [XmlElement(ElementName = "tile")] public List<TmxTilesetTile> tiles;

        [XmlElement(ElementName = "image")] public TmxImage image;

        public bool isStandardTileset = true;

        // we need this for tilesets that have no image. they use image collections and we need the path to save the new atlas we generate.
        public string mapFolder;

        [XmlAttribute(AttributeName = "margin")]
        public int margin;

        [XmlAttribute(AttributeName = "name")] public string name;

        [XmlArray("properties")] [XmlArrayItem("property")]
        public List<TmxProperty> properties;

        [XmlAttribute(AttributeName = "source")]
        public string source;

        [XmlAttribute(AttributeName = "spacing")]
        public int spacing;

        [XmlArray("terraintypes")] [XmlArrayItem("terrain")]
        public List<TmxTerrain> terrainTypes;

        [XmlAttribute(AttributeName = "tilecount")]
        public int tileCount;

        [XmlAttribute(AttributeName = "tileheight")]
        public int tileHeight;


        [XmlAttribute(AttributeName = "tilewidth")]
        public int tileWidth;


        public TmxTileset()
        {
            tileOffset = new TmxTileOffset();
            tiles = new List<TmxTilesetTile>();
            properties = new List<TmxProperty>();
            terrainTypes = new List<TmxTerrain>();
        }


        public void fixImagePath(string mapPath, string tilesetSource)
        {
            var mapDirectory = Path.GetDirectoryName(mapPath);
            var tilesetDirectory = Path.GetDirectoryName(tilesetSource);
            var imageDirectory = Path.GetDirectoryName(image.source);
            var imageFile = Path.GetFileName(image.source);

            var newPath = Path.GetFullPath(Path.Combine(mapDirectory, tilesetDirectory, imageDirectory, imageFile));
            image.source = Path.Combine(PathHelper.makeRelativePath(mapPath, newPath));
        }


        public override string ToString()
        {
            return string.Format("{0}: {1}", name, image);
        }
    }
}