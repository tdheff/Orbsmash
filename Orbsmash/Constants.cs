using System.Collections.Generic;

namespace Orbsmash.Constants
{
    public sealed class Gameplay
    {
        public enum Side
        {
            LEFT,
            RIGHT
        };

        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
    }


    public sealed class AnimationContexts
    {
        public static readonly string PLAYER_SPRITE_ANIMATIONS = "PLAYER_SPRITE_ANIMATIONS";
        public static readonly string BALL_SPRITE_ANIMATIONS = "BALL_SPRITE_ANIMATIONS";
    }

    public sealed class CollisionLayers
    {
        public static readonly string COLLIDERS = "Colliders";
    }

    public sealed class MapTiles
    {
        public static readonly string DUNGEON = "Tiles/DungeonMap";
    }

    public sealed class PlayerSprites
    {
        public static readonly string KNIGHT = "Sprites/Characters/Knight/Knight";
    }

    public sealed class BallSprites
    {
        public static readonly string DEFAULT = "Sprites/Ball/Ball";
    }

    public sealed class Hitboxes
    {
        public static readonly string KNIGHT_HITBOX = "Sprites/Characters/Knight/Knight_hitbox";
        public static readonly List<string> HitboxesToLoad;
        static Hitboxes()
        {
            HitboxesToLoad = new List<string>() { KNIGHT_HITBOX };
        }
    }


    public sealed class BallAnimations
    {
        public static readonly string IDLE = "IDLE";
    }

    public sealed class PlayerAnimations
    {
        public static readonly string IDLE_VERTICAL = "IDLE_VERTICAL";
        public static readonly string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public static readonly string WALK_UP = "WALK_UP";
        public static readonly string WALK_DOWN = "WALK_DOWN";
        public static readonly string WALK_LEFT = "WALK_LEFT";
        public static readonly string WALK_RIGHT = "WALK_RIGHT";
        public static readonly string CHARGE_PULSE = "CHARGE_PULSE";
        public static readonly string CHARGE = "CHARGE";
        public static readonly string SWING = "SWING";
    }

    public sealed class PlayerEvents
    {
        public static readonly string PLAYER_HIT_START = "PLAYER_HIT_START";
        public static readonly string PLAYER_HIT_END = "PLAYER_HIT_END";
        public static readonly string PLAYER_SWING_END = "PLAYER_SWING_END";
    }

    
}