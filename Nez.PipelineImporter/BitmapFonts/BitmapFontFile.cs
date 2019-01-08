using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nez.BitmapFontImporter
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------
    [XmlRoot("font")]
    public class BitmapFontFile
    {
        [XmlArray("chars")] [XmlArrayItem("char")]
        public List<BitmapFontChar> chars;

        [XmlElement("info")] public BitmapFontInfo info;

        [XmlElement("common")] public BitmapFontCommon common;

        /// <summary>
        ///     the full path to the fnt font
        /// </summary>
        public string file;


        [XmlArray("kernings")] [XmlArrayItem("kerning")]
        public List<BitmapFontKerning> kernings;

        [XmlArray("pages")] [XmlArrayItem("page")]
        public List<BitmapFontPage> pages;
    }
}