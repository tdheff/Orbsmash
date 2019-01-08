using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Orbsmash.Constants
{
    public sealed class Gameplay
    {
        public enum Side
        {
            LEFT,
            RIGHT,
            NONE
        };

        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
    }

    public sealed class BallResetPositions
    {
        public static readonly Vector2 LEFT_RESET = new Vector2(800, 800);
        public static readonly Vector2 RIGHT_RESET = new Vector2(1760, 800);
    }
    
    public sealed class Timers
    {
        public static readonly float POINT_SCORED_TIMER = 1.5f;
    }

    public sealed class EntityNames
    {
        public static readonly string BALL = "BALL";
    }
    
    public sealed class ComponentNames
    {
        public static readonly string HITBOX_COLLIDER = "HITBOX_COLLIDER";
        public static readonly string PLAYER_COLLIDER = "PLAYER_COLLIDER";
    }

    public sealed class AnimationContexts
    {
        public static readonly string PLAYER_SPRITE_ANIMATIONS = "PLAYER_SPRITE_ANIMATIONS";
        public static readonly string BALL_SPRITE_ANIMATIONS = "BALL_SPRITE_ANIMATIONS";
    }

    public sealed class TiledImportCollisionLayers
    {
        public static readonly string WALLS = "WALLS";
        public static readonly string ENVIRONMENT = "ENVIRONMENT";
        public static readonly string NET = "NET";
    }

    public sealed class MapTiles
    {
        public static readonly string DUNGEON = "Tiles/DungeonMap";
        public static readonly string MEDIEVAL = "Tiles/MedievalMap";
    }
    
    public sealed class PhysicsLayers
    {
        public static readonly int ENVIRONMENT = 1;
        public static readonly int WALLS = 2;
        public static readonly int NET = 3;
        public static readonly int PLAYER = 4;
        public static readonly int BALL = 5;
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
        public static readonly string KNIGHT_HITBOX = "Sprites/Characters/Knight/Knight_Hitbox";
        public static readonly string KNIGHT_SWING_HITBOX = "Sprites/Characters/Knight/Knight_Hitbox";
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
        public static readonly string CHARGE_IDLE = "CHARGE_IDLE";
        public static readonly string CHARGE = "CHARGE";
        public static readonly string SWING = "SWING";
    }

    public sealed class PlayerEvents
    {
        public static readonly string PLAYER_HIT_START = "PLAYER_HIT_START";
        public static readonly string PLAYER_HIT_END = "PLAYER_HIT_END";
        public static readonly string PLAYER_SWING_END = "PLAYER_SWING_END";
        public static readonly string PLAYER_DASH_END = "PLAYER_DASH_END";
    }

    
}