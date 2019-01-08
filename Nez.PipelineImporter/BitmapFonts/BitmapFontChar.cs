using System.Xml.Serialization;

namespace Nez.BitmapFontImporter
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------
    public class BitmapFontChar
    {
        [XmlAttribute("chnl")] public int channel;

        [XmlAttribute("height")] public int height;

        [XmlAttribute("id")] public int id;

        [XmlAttribute("page")] public int page;

        [XmlAttribute("width")] public int width;

        [XmlAttribute("x")] public int x;

        [XmlAttribute("xadvance")] public int xAdvance;

        [XmlAttribute("xoffset")] public int xOffset;

        [XmlAttribute("y")] public int y;

        [XmlAttribute("yoffset")] public int yOffset;
    }
}