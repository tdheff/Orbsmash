using System.Xml.Serialization;

namespace Nez.BitmapFontImporter
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------
    public class BitmapFontInfo
    {
        [XmlAttribute("bold")] public int bold;

        [XmlAttribute("charset")] public string charSet;

        [XmlAttribute("face")] public string face;

        [XmlAttribute("italic")] public int italic;

        [XmlAttribute("outline")] public int outLine;

        [XmlAttribute("padding")] public string padding;

        [XmlAttribute("size")] public int size;

        [XmlAttribute("smooth")] public int smooth;

        [XmlAttribute("spacing")] public string spacing;

        [XmlAttribute("stretchH")] public int stretchHeight;

        [XmlAttribute("aa")] public int superSampling;

        [XmlAttribute("unicode")] public string unicode;
    }
}