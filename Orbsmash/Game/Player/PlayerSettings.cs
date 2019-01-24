using Microsoft.Xna.Framework;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class PlayerSettings
    {
        public int Id;
        public float Speed;
        public string Sprite;
        public string Hitbox;
        public Gameplay.Side Side;
        public Vector2 StartingPosition;
        public Gameplay.Character Character;
    }
}