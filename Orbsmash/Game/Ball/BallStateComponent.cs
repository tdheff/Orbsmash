using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Ball
{
    public class BallStateComponent : Component
    {
        public int LastHitPlayerId = -1;
        public Gameplay.Side LastHitSide = Gameplay.Side.NONE;
        public bool IsDeadly = false;

        public float BaseSpeedInitial = 700.0f;
        public float BaseSpeed = 700.0f;
        public float HitBoost = 1;
        
        public Color Color = Color.Cyan;
    }
}