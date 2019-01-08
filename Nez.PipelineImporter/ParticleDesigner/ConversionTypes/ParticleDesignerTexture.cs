using System.Xml.Serialization;

namespace Nez.ParticleDesignerImporter
{
    public class ParticleDesignerTexture
    {
        [XmlAttribute] public string data;

        [XmlAttribute] public string name;
    }
}