using Nez.Sprites;
using Nez.Textures;

namespace Handy.Components
{
    public class SpriteComponent : Sprite
    {
        public string SpriteName;

        public SpriteComponent(Subtexture tex) : base(tex)
        {
        }
    }
}