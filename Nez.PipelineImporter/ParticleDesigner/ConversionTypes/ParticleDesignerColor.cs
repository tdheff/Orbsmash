using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Nez.ParticleDesignerImporter
{
    public class ParticleDesignerColor
    {
        [XmlAttribute] public float alpha;

        [XmlAttribute] public float blue;

        [XmlAttribute] public float green;

        [XmlAttribute] public float red;


        public static implicit operator Color(ParticleDesignerColor obj)
        {
            return new Color(obj.red, obj.green, obj.blue, obj.alpha);
        }
    }
}