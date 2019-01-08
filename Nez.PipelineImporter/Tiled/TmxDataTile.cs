using System.Xml.Serialization;

namespace Nez.TiledMaps
{
    public class TmxDataTile
    {
        public bool flippedDiagonally;
        public bool flippedHorizontally;
        public bool flippedVertically;


        [XmlAttribute(AttributeName = "gid")] public uint gid;

        public TmxDataTile()
        {
        }

        public TmxDataTile(uint gid)
        {
            this.gid = gid;
        }


        public override string ToString()
        {
            return gid.ToString();
        }
    }
}