using System.Xml.Serialization;

namespace Nez.BitmapFontImporter
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------
    public class BitmapFontCommon
    {
        // Littera doesnt seem to add these
        [XmlAttribute("alphaChnl")] public int alphaChannel;

        [XmlAttribute("base")] public int base_;

        [XmlAttribute("blueChnl")] public int blueChannel;

        [XmlAttribute("greenChnl")] public int greenChannel;

        [XmlAttribute("lineHeight")] public int lineHeight;

        [XmlAttribute("packed")] public int packed;

        [XmlAttribute("pages")] public int pages;

        [XmlAttribute("redChnl")] public int redChannel;

        [XmlAttribute("scaleH")] public int scaleH;

        [XmlAttribute("scaleW")] public int scaleW;
    }
}