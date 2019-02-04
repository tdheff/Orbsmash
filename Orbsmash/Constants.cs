using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Orbsmash.Constants
{
    public sealed class MathUtil
    {
        public const float SQRT_ONE_HALF = 0.70710678118f;
    }
    
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
            FORWARD,
            BACKWARD
        }

        public enum Character
        {
            KNIGHT,
            WIZARD,
            SPACEMAN,
            ALIEN,
            PIRATE,
            SKELETON
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
        public static readonly string MEDIEVAL2 = "Tiles/MedievalMap2";
    }
    
    public sealed class PhysicsLayers
    {
        public static readonly int ENVIRONMENT = 1;
        public static readonly int WALLS = 2;
        public static readonly int NET = 3;
        public static readonly int PLAYER = 4;
        public static readonly int BALL = 5;
    }

    public sealed class RenderLayers
    {
        public static readonly int BACKGROUND = 3;
        public static readonly int PRIMARY = 2;
        public static readonly int FOREGROUND = 1;
    }

    public sealed class Sprites
    {
        public static readonly string HitEffect = "Sprites/Effects/HitEffect";
    }

    public sealed class PlayerSprites
    {
        public static readonly string KNIGHT = "Sprites/Characters/Knight/Knight";
        public static readonly string WIZARD = "Sprites/Characters/Wizard/Wizard";
    }
    
    public sealed class AsepriteFiles
    {
        public static readonly string KNIGHT = "Sprites/Characters/Knight/KnightJson";
        public static readonly string WIZARD = "Sprites/Characters/Wizard/WizardJson";
        public static readonly string HIT_EFFECT = "Sprites/Effects/HitEffectJson";
    }

    public sealed class SoundEffects
    {
        public static readonly string MENU_MUSIC = "audio/music/bumpin_eighties";
        public static readonly string FOOTSTEPS_1 = "audio/character/footsteps_1";
    }

    public sealed class KnightSoundEffects
    {
        public static readonly string BOOT_1 = "audio/character/knight/footsteps/boot_1";
        public static readonly string BOOT_2 = "audio/character/knight/footsteps/boot_2";
        public static readonly string BOOT_3 = "audio/character/knight/footsteps/boot_3";
        public static readonly string BOOT_4 = "audio/character/knight/footsteps/boot_4";
        public static readonly string BOOT_5 = "audio/character/knight/footsteps/boot_5";
        public static readonly string BOOT_6 = "audio/character/knight/footsteps/boot_6";
        public static readonly string SWIPE_1 = "audio/character/knight/sword/swipe_1";
        public static readonly string SWIPE_2 = "audio/character/knight/sword/swipe_2";
        public static readonly string SWIPE_3 = "audio/character/knight/sword/swipe_3";
        public static readonly string SWIPE_4 = "audio/character/knight/sword/swipe_4";
        public static readonly string HIT_1 = "audio/character/knight/sword/hit_1";
        public static List<string> AllEffects = new List<string>() { BOOT_1, BOOT_2, BOOT_3, BOOT_4, BOOT_5, BOOT_6, SWIPE_1, SWIPE_2, SWIPE_3, SWIPE_4, HIT_1 };
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

    public sealed class KnightAnimations
    {
        public static readonly string IDLE_VERTICAL = "IDLE_VERTICAL";
        public static readonly string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public static readonly string WALK_UP = "WALK_UP";
        public static readonly string WALK_DOWN = "WALK_DOWN";
        public static readonly string WALK_BACKWARD = "WALK_BACKWARD";
        public static readonly string WALK_FORWARD = "WALK_FORWARD";
        public static readonly string CHARGE_IDLE = "CHARGE_IDLE";
        public static readonly string CHARGE_FULL = "CHARGE_FULL";
        public static readonly string CHARGE = "CHARGE";
        public static readonly string ATTACK = "ATTACK";
        public static readonly string BLOCK = "BLOCK";
        public static readonly string BLOCK_HIT = "BLOCK_HIT";
    }
    
    public sealed class WizardAnimations
    {
        public static readonly string IDLE_VERTICAL = "IDLE_VERTICAL";
        public static readonly string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public static readonly string WALK_DOWN = "WALK_DOWN";
        public static readonly string WALK_FORWARD = "WALK_FORWARD";
        public static readonly string ATTACK = "ATTACK";
        public static readonly string GLIDE = "GLIDE";
        public static readonly string IMMATERIAL = "IMMATERIAL";
    }

    public sealed class PlayerEvents
    {
        public static readonly string PLAYER_HIT_START = "PLAYER_HIT_START";
        public static readonly string PLAYER_HIT_END = "PLAYER_HIT_END";
        public static readonly string PLAYER_SWING_END = "PLAYER_SWING_END";
        public static readonly string PLAYER_DASH_END = "PLAYER_DASH_END";
        public static readonly string CHARGE_WINDUP_END = "CHARGE_WINDUP_END";
        public static readonly string BLOCK_END = "BLOCK_END";
    }

    
}