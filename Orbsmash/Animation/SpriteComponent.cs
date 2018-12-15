using Handy;
using Handy.Animation;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;

namespace Orbsmash.Animation
{
    public enum EAnimations { PlayerIdle, PlayerWalkHorizontal, PlayerWalkVertical, PlayerCharge, PlayerSwing, PlayerDie }
    
    public class SpriteComponent : Sprite<EAnimations>
    {
        public SpriteComponent(Subtexture tex) : base(tex) { }
    }
}