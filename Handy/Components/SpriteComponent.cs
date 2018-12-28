using Handy;
using Handy.Animation;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace Handy.Components
{
    
    public class SpriteComponent : Sprite
    {
        public string SpriteName;
        public SpriteComponent(Subtexture tex) : base(tex) { }
    }
}