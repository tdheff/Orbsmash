using System.Xml.Serialization;

namespace Nez.BitmapFontImporter
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------
    public class BitmapFontKerning
    {
        [XmlAttribute("amount")] public int amount;

        [XmlAttribute("first")] public int first;

        [XmlAttribute("second")] public int second;
    }
}