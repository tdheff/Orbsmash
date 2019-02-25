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
        public static readonly Vector2 LEFT_RESET = new Vector2(800, 500);
        public static readonly Vector2 RIGHT_RESET = new Vector2(1200, 500);
    }
    
    public sealed class Timers
    {
        public const float POINT_SCORED_TIMER = 1.5f;
    }

    public sealed class EntityNames
    {
        public const string BALL = "BALL";
    }
    
    public sealed class ComponentNames
    {
        public const string HITBOX_COLLIDER = "HITBOX_COLLIDER";
        public const string PLAYER_COLLIDER = "PLAYER_COLLIDER";
        public const string AIM_INDICATOR = "AIM_INDICATOR";
        public const string CHARACTER_CIRCLE = "CHARACTER_CIRCLE";
        public const string RIGHT_COOLDOWN = "RIGHT_COOLDOWN";
        public const string LEFT_COOLDOWN = "LEFT_COOLDOWN";
    }

    public sealed class AnimationContexts
    {
        public const string PLAYER_SPRITE_ANIMATIONS = "PLAYER_SPRITE_ANIMATIONS";
        public const string BALL_SPRITE_ANIMATIONS = "BALL_SPRITE_ANIMATIONS";
    }

    public sealed class TiledImportCollisionLayers
    {
        public const string BACK_WALLS = "BACK_WALLS";
        public const string SIDE_WALLS = "SIDE_WALLS";
        public const string ENVIRONMENT = "ENVIRONMENT";
        public const string NET = "NET";
    }

    public sealed class MapTiles
    {
        public const string DUNGEON = "Tiles/DungeonMap";
        public const string MEDIEVAL = "Tiles/MedievalMap";
        public const string MEDIEVAL2 = "Tiles/MedievalMap2";
    }
    
    public sealed class PhysicsLayers
    {
        public const int ENVIRONMENT = 1;
        public const int BACK_WALLS = 2;
        public const int SIDE_WALLS = 3;
        public const int NET = 4;
        public const int PLAYER = 5;
        public const int BALL = 6;
    }

    public sealed class RenderLayers
    {
        public const int BACKGROUND = 3;
        public const int PRIMARY = 2;
        public const int FOREGROUND = 1;
    }

    public sealed class Sprites
    {
        public const string HIT_EFFECT = "Sprites/Effects/HitEffect";
        public const string AIM_INDICATOR = "Sprites/Indicators/AimIndicator";
        public const string CHARACTER_CIRCLE = "Sprites/Indicators/CharacterCircle";
        public const string LEFT_COOLDOWN = "Sprites/Indicators/LeftCooldown";
        public const string RIGHT_COOLDOWN = "Sprites/Indicators/RightCooldown";
    }

    public sealed class PlayerSprites
    {
        public const string KNIGHT = "Sprites/Characters/Knight/Knight";
        public const string WIZARD = "Sprites/Characters/Wizard/Wizard";
        public const string SPACEMAN = "Sprites/Characters/Spaceman/Spaceman";
    }
    
    public sealed class AsepriteFiles
    {
        public const string KNIGHT = "Sprites/Characters/Knight/KnightJson";
        public const string WIZARD = "Sprites/Characters/Wizard/WizardJson";
        public const string SPACEMAN = "Sprites/Characters/Spaceman/SpacemanJson";
        public const string HIT_EFFECT = "Sprites/Effects/HitEffectJson";
    }

    public sealed class SoundEffects
    {
        public const string MENU_MUSIC = "audio/music/bumpin_eighties";
        public const string FOOTSTEPS_1 = "audio/character/footsteps_1";
    }

    public sealed class KnightSoundEffects
    {
        public const string BOOT_1 = "audio/character/knight/footsteps/boot_1";
        public const string BOOT_2 = "audio/character/knight/footsteps/boot_2";
        public const string BOOT_3 = "audio/character/knight/footsteps/boot_3";
        public const string BOOT_4 = "audio/character/knight/footsteps/boot_4";
        public const string BOOT_5 = "audio/character/knight/footsteps/boot_5";
        public const string BOOT_6 = "audio/character/knight/footsteps/boot_6";
        public const string SWIPE_1 = "audio/character/knight/sword/swipe_1";
        public const string SWIPE_2 = "audio/character/knight/sword/swipe_2";
        public const string SWIPE_3 = "audio/character/knight/sword/swipe_3";
        public const string SWIPE_4 = "audio/character/knight/sword/swipe_4";
        public const string HIT_1 = "audio/character/knight/sword/hit_1";
        public static List<string> AllEffects = new List<string>() { BOOT_1, BOOT_2, BOOT_3, BOOT_4, BOOT_5, BOOT_6, SWIPE_1, SWIPE_2, SWIPE_3, SWIPE_4, HIT_1 };
    }

    public sealed class KnightSoundEffectGroups
    {
        public const string STEPS = "STEPS";
        public const string HITS = "HITS";
        public const string SWIPES = "SWIPES";
    }

    public sealed class WizardSoundEffects
    {
        public const string STEP_1 = "audio/character/wizard/footsteps/step_1";
        public const string STEP_2 = "audio/character/wizard/footsteps/step_2";
        public const string STEP_3 = "audio/character/wizard/footsteps/step_3";
        public const string STEP_4 = "audio/character/wizard/footsteps/step_4";
        public const string STEP_5 = "audio/character/wizard/footsteps/step_5";
        public const string STEP_6 = "audio/character/wizard/footsteps/step_6";
        public const string FLAME_LIGHT_1 = "audio/character/wizard/flame/flame_light_1";
        public const string FLAME_LIGHT_2 = "audio/character/wizard/flame/flame_light_2";
        public const string FLAME_LIGHT_3 = "audio/character/wizard/flame/flame_light_3";
        public const string FLAME_HIT = "audio/character/wizard/flame/flame_hit";

        public static List<string> AllEffects = new List<string>() { STEP_1, STEP_2, STEP_3, STEP_4, STEP_5, STEP_6, FLAME_LIGHT_1, FLAME_LIGHT_2, FLAME_LIGHT_3, FLAME_HIT };
    }

    public sealed class WizardSoundEffectGroups
    {
        public const string STEPS = "STEPS";
        public const string HITS = "HITS";
        public const string SWIPES = "SWIPES";
        public const string IMMATERIAL = "IMMATERIAL";
    }


    public sealed class BallSprites
    {
        public const string DEFAULT = "Sprites/Ball/Ball";
    }

    public sealed class Hitboxes
    {
        public const string KNIGHT_HITBOX = "Sprites/Characters/Knight/Knight_Hitbox";
        public const string WIZARD_HITBOX = "Sprites/Characters/Wizard/Wizard_Hitbox";
        public static readonly List<string> HITBOXES_TO_LOAD;
        static Hitboxes()
        {
            HITBOXES_TO_LOAD = new List<string> { KNIGHT_HITBOX, WIZARD_HITBOX };
        }
    }


    public sealed class BallAnimations
    {
        public const string IDLE = "IDLE";
    }

    public sealed class KnightAnimations
    {
        public const string IDLE_VERTICAL = "IDLE_VERTICAL";
        public const string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public const string WALK_UP = "WALK_UP";
        public const string WALK_DOWN = "WALK_DOWN";
        public const string WALK_BACKWARD = "WALK_BACKWARD";
        public const string WALK_FORWARD = "WALK_FORWARD";
        public const string CHARGE_IDLE = "CHARGE_IDLE";
        public const string CHARGE_FULL = "CHARGE_FULL";
        public const string CHARGE = "CHARGE";
        public const string ATTACK = "ATTACK";
        public const string BLOCK = "BLOCK";
        public const string BLOCK_HIT = "BLOCK_HIT";
        public const string KO = "KO";
    }
    
    public sealed class SpacemanAnimations
    {
        public const string IDLE_VERTICAL = "IDLE_VERTICAL";
        public const string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public const string WALK_UP = "WALK_UP";
        public const string WALK_DOWN = "WALK_DOWN";
        public const string WALK_BACKWARD = "WALK_BACKWARD";
        public const string WALK_FORWARD = "WALK_FORWARD";
        public const string ATTACK = "ATTACK";
    }
    
    public sealed class WizardAnimations
    {
        public const string IDLE_VERTICAL = "IDLE_VERTICAL";
        public const string IDLE_HORIZONTAL = "IDLE_HORIZONTAL";
        public const string WALK_DOWN = "WALK_DOWN";
        public const string WALK_FORWARD = "WALK_FORWARD";
        public const string ATTACK = "ATTACK";
        public const string GLIDE = "GLIDE";
        public const string IMMATERIAL = "IMMATERIAL";
    }

    public sealed class PlayerEvents
    {
        public const string PLAYER_HIT_START = "PLAYER_HIT_START";
        public const string PLAYER_HIT_END = "PLAYER_HIT_END";
        public const string PLAYER_SWING_END = "PLAYER_SWING_END";
        public const string PLAYER_DASH_END = "PLAYER_DASH_END";
        public const string CHARGE_WINDUP_END = "CHARGE_WINDUP_END";
        public const string BLOCK_END = "BLOCK_END";
        public const string BLOCK_HIT = "BLOCK_HIT";
        public const string BLOCK_HIT_END = "BLOCK_HIT_END";
        public const string KO_BOUNCE = "KO_BOUNCE";
        public const string KO_END = "KO_END";
    }

    public sealed class MovementSpeeds
    {
        public const float VERY_LOW = 250.0f;
        public const float LOW = 300.0f;
        public const float MEDIUM = 350.0f;
        public const float HIGH = 400.0f;
        public const float VERY_HIGH = 450.0f;
    }
    
}