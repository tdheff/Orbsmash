using System;
using System.Collections.Generic;
using System.Linq;
using Handy.Components;
using HandyScene = Handy.Scene;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Handy.Animation;
using Nez.PhysicsShapes;
using Orbsmash.Constants;
using Orbsmash.Game;
using Handy.Sound;

namespace Orbsmash.Player
{
   
    // <summary>
    // Entity representing a player in the game
    // </summary>
    public class Player : Entity
    {
        private static Dictionary<string, HashSet<string>> _knightEventTriggers = new Dictionary<string, HashSet<string>>
        {
            // LIGHT ATTACK
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 11 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 2 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 4 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(KnightAnimations.CHARGE, 4 ), new HashSet<string> { PlayerEvents.CHARGE_WINDUP_END }},
            // HEAVY ATTACK
            { EventComponent.BuildKey(KnightAnimations.ATTACK_HEAVY, 12 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK_HEAVY, 1 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK_HEAVY, 3 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(KnightAnimations.CHARGE_HEAVY, 3 ), new HashSet<string> { PlayerEvents.CHARGE_WINDUP_END }},
            { EventComponent.BuildKey(KnightAnimations.KO, 6 ), new HashSet<string> { PlayerEvents.KO_BOUNCE }},
            { EventComponent.BuildKey(KnightAnimations.KO, 10 ), new HashSet<string> { PlayerEvents.KO_END }},
        };
        
        private static Dictionary<string, HashSet<string>> _wizardEventTriggers = new Dictionary<string, HashSet<string>>
        {
            // LIGHT ATTACK
            { EventComponent.BuildKey(WizardAnimations.ATTACK, 11 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(WizardAnimations.ATTACK, 3 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(WizardAnimations.ATTACK, 7 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            // HEAVY ATTACK
            { EventComponent.BuildKey(WizardAnimations.ATTACK_HEAVY, 20 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(WizardAnimations.ATTACK_HEAVY, 8 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(WizardAnimations.ATTACK_HEAVY, 11 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(WizardAnimations.KO, 6 ), new HashSet<string> { PlayerEvents.KO_BOUNCE }},
            { EventComponent.BuildKey(WizardAnimations.KO, 9 ), new HashSet<string> { PlayerEvents.KO_END }},
        };
        
        private static Dictionary<string, HashSet<string>> _spacemanEventTriggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(SpacemanAnimations.ATTACK, 19 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(SpacemanAnimations.ATTACK, 9 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(SpacemanAnimations.ATTACK, 11 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(SpacemanAnimations.SHIELD, 4 ), new HashSet<string> { PlayerEvents.BLOCK_END }},
        };

        public PlayerSettings Settings;
        private PlayerStateComponent _stateComponent;
        private Component _state;
        private PlayerInputComponent _input;
        private VelocityComponent _velocity;
        private BoxCollider _collider;
        private PolygonCollider _lightHitbox;
        private PolygonCollider _heavyHitbox;
        private KinematicComponent _kinematic = new KinematicComponent();
        private AnimationComponent _mainBodyAnimation;
        private AnimatableSprite _mainPlayerBodySprite;
        private EventComponent _events = new EventComponent();
        private SoundEffectGroupComponent _swipes;
        private SoundEffectGroupComponent _hits;
        private SoundEffectGroupComponent _steps;
        private SpriteComponent _aimIndicator;

        public Player(PlayerSettings settings)
        {
            name = $"Player_{settings.Id}";
            Settings = settings;
            
            // physics
            _stateComponent = new PlayerStateComponent(settings.Id, settings.Side, settings.Speed, settings.StartingPosition); 
            _velocity = new VelocityComponent(new Vector2(0, 0));
            transform.position = settings.StartingPosition;
            
            // input
            _input = new PlayerInputComponent(settings.Id);
            addComponent(_events);
            addComponent(_stateComponent);
            addComponent(_velocity);
            addComponent(_input);
            addComponent(_kinematic);
        }
        
        public override void onAddedToScene()
        {
            var gameScene = (HandyScene)scene;
            AnimationDefinition animationDefinition;
            Vector2[] lightPoints;
            Vector2[] heavyPoints;
            switch (Settings.Character)
            {
                case Gameplay.Character.KNIGHT:
                    _state = new KnightStateMachineComponent(new KnightState());
                    _events.SetTriggers(_knightEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.KNIGHT];
                    lightPoints = scene.content.Load<Polygon>(Hitboxes.KNIGHT_HITBOX_LIGHT).points;
                    heavyPoints = scene.content.Load<Polygon>(Hitboxes.KNIGHT_HITBOX_HEAVY).points;
                    break;
                case Gameplay.Character.WIZARD:
                    _state = new WizardStateMachineComponent(new WizardState());
                    _events.SetTriggers(_wizardEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.WIZARD];
                    lightPoints = scene.content.Load<Polygon>(Hitboxes.WIZARD_HITBOX_LIGHT).points;
                    heavyPoints = scene.content.Load<Polygon>(Hitboxes.WIZARD_HITBOX_HEAVY).points;

                    break;
                case Gameplay.Character.SPACEMAN:
                    _state = new SpacemanStateMachineComponent(new SpacemanState());
                    _events.SetTriggers(_spacemanEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.SPACEMAN];
                    lightPoints = scene.content.Load<Polygon>(Hitboxes.KNIGHT_HITBOX_LIGHT).points;
                    heavyPoints = scene.content.Load<Polygon>(Hitboxes.KNIGHT_HITBOX_HEAVY).points;

                    break;
                case Gameplay.Character.ALIEN:
                    throw new NotImplementedException();
                case Gameplay.Character.PIRATE:
                    throw new NotImplementedException();
                case Gameplay.Character.SKELETON:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _mainPlayerBodySprite = new AnimatableSprite(animationDefinition.SpriteDefinition.Subtextures);
            _mainPlayerBodySprite.renderLayer = RenderLayers.PRIMARY;
            _mainBodyAnimation = new AnimationComponent(_mainPlayerBodySprite, animationDefinition, KnightAnimations.IDLE_HORIZONTAL);
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new BoxCollider(15, 10);
            _collider.name = ComponentNames.PLAYER_COLLIDER;
            Flags.setFlagExclusive(ref _collider.physicsLayer, PhysicsLayers.PLAYER);
            _collider.collidesWithLayers = 0;
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.BACK_WALLS);
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.SIDE_WALLS);
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.ENVIRONMENT);
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.NET);
            
            
            // Aim Indicator
            var aimIndicator = new AimIndicator(this);
            scene.addEntity(aimIndicator);

            addComponent(_state);
            // _hitbox = new PolygonCollider([]);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);

            addComponent(_collider);
            if (_stateComponent.side == Gameplay.Side.RIGHT)
            {
                _mainPlayerBodySprite.flipX = true;
                // TODO - this is horrible but it's the only solution that worked
                Vector2 lightOffset = new Vector2();
                Vector2 heavyOffset = new Vector2();
                switch (Settings.Character)
                {
                    case Gameplay.Character.KNIGHT:
                        lightOffset = new Vector2(-8.5f, -1.5f);
                        heavyOffset = new Vector2(-25, 2.5f);
                        break;
                    case Gameplay.Character.WIZARD:
                        lightOffset = new Vector2(0.333f, 1f);
                        heavyOffset = new Vector2(-.286f, 0.429f);
                        break;
                    case Gameplay.Character.SPACEMAN:
                        break;
                    case Gameplay.Character.ALIEN:
                        break;
                    case Gameplay.Character.PIRATE:
                        break;
                    case Gameplay.Character.SKELETON:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                lightPoints = lightPoints.Reverse().Select(point => new Vector2(-point.X + lightOffset.X, point.Y + lightOffset.Y)).ToArray();
                heavyPoints = heavyPoints.Reverse().Select(point => new Vector2(-point.X + heavyOffset.X, point.Y + heavyOffset.Y)).ToArray();
            }
            _lightHitbox = new PolygonCollider(lightPoints);
            _lightHitbox.isTrigger = true;
            _lightHitbox.name = ComponentNames.HITBOX_COLLIDER_LIGHT;
            addComponent(_lightHitbox);
            _heavyHitbox = new PolygonCollider(heavyPoints);
            _heavyHitbox.isTrigger = true;
            _heavyHitbox.name = ComponentNames.HITBOX_COLLIDER_HEAVY;
            addComponent(_heavyHitbox);

            // Circle and cooldowns
            var subtextures = Util.ExtractSubtextures(gameScene.Textures[Sprites.CHARACTER_CIRCLE], 1, 1);
            var circle = new SpritesheetComponent(subtextures);
            circle.name = ComponentNames.CHARACTER_CIRCLE;
            circle.renderLayer = RenderLayers.BACKGROUND;
            circle.localOffset = new Vector2(0, 15);
            circle.renderOffset = 1;
            addComponent(circle);
            
            subtextures = Util.ExtractSubtextures(gameScene.Textures[Sprites.LEFT_COOLDOWN], 1, 15);
            var left = new SpritesheetComponent(subtextures);
            left.name = ComponentNames.LEFT_COOLDOWN;
            left.renderLayer = RenderLayers.BACKGROUND;
            left.localOffset = new Vector2(0, 15);
            left.renderOffset = 1;
            addComponent(left);
            
            subtextures = Util.ExtractSubtextures(gameScene.Textures[Sprites.RIGHT_COOLDOWN], 1, 15);
            var right = new SpritesheetComponent(subtextures);
            right.name = ComponentNames.RIGHT_COOLDOWN;
            right.renderLayer = RenderLayers.BACKGROUND;
            right.localOffset = new Vector2(0, 15);
            right.renderOffset = 1;
            addComponent(right);

            addComponent(new SpriteDepthComponent { BaseRenderLayer = RenderLayers.PRIMARY});
            
            SetupSound();
        }

        private void SetupSound()
        {
            // at this point mostly for knight, will make a switch statement at some point
            var gameScene = (HandyScene)scene;
            var sounds = gameScene.Sounds;
            switch (Settings.Character)
            {
                case Gameplay.Character.KNIGHT:
                    var stepEffects = new List<HandySoundEffect>()
                    {
                        sounds[KnightSoundEffects.BOOT_1],
                        sounds[KnightSoundEffects.BOOT_2],
                        sounds[KnightSoundEffects.BOOT_3],
                        sounds[KnightSoundEffects.BOOT_4],
                        sounds[KnightSoundEffects.BOOT_5],
                        sounds[KnightSoundEffects.BOOT_6],
                    };
                    _steps = new SoundEffectGroupComponent(KnightSoundEffectGroups.STEPS, stepEffects, 0.3f, 0.5f * gameScene.SfxVolume);
                    var swipeEffects = new List<HandySoundEffect>()
                    {
                        sounds[KnightSoundEffects.SWIPE_1],
                        sounds[KnightSoundEffects.SWIPE_2],
                        sounds[KnightSoundEffects.SWIPE_3],
                        sounds[KnightSoundEffects.SWIPE_4],
                    };
                    _swipes = new SoundEffectGroupComponent(KnightSoundEffectGroups.SWIPES, swipeEffects, 2f, 0.5f * gameScene.SfxVolume, true);
                    var hitEffects = new List<HandySoundEffect>()
                    {
                        sounds[KnightSoundEffects.HIT_1],
                    };
                    _hits = new SoundEffectGroupComponent(KnightSoundEffectGroups.HITS, hitEffects, 2f, 0.5f * gameScene.SfxVolume, true);
                    addComponent(_steps);
                    addComponent(_swipes);
                    addComponent(_hits);
                    break;
                case Gameplay.Character.WIZARD:
                    var wizardStepEffects = new List<HandySoundEffect>()
                    {
                        sounds[WizardSoundEffects.STEP_1],
                        sounds[WizardSoundEffects.STEP_2],
                        sounds[WizardSoundEffects.STEP_3],
                        sounds[WizardSoundEffects.STEP_4],
                        sounds[WizardSoundEffects.STEP_5],
                        sounds[WizardSoundEffects.STEP_6],
                    };
                    // 0.1f volume cuz i exported these guys pretty loud
                    _steps = new SoundEffectGroupComponent(WizardSoundEffectGroups.STEPS, wizardStepEffects, 0.3f, 0.1f * gameScene.SfxVolume);
                    var wizardFireEffects = new List<HandySoundEffect>()
                    {
                        sounds[WizardSoundEffects.FLAME_LIGHT_1],
                        sounds[WizardSoundEffects.FLAME_LIGHT_2],
                        sounds[WizardSoundEffects.FLAME_LIGHT_3]
                    };
                    _swipes = new SoundEffectGroupComponent(WizardSoundEffectGroups.SWIPES, wizardFireEffects, 2f, 0.5f * gameScene.SfxVolume, true);
                    var wizardHitEffects = new List<HandySoundEffect>()
                    {
                        sounds[WizardSoundEffects.FLAME_HIT],
                    };
                    _hits = new SoundEffectGroupComponent(WizardSoundEffectGroups.HITS, wizardHitEffects, 2f, 0.5f * gameScene.SfxVolume, true);
                    addComponent(_steps);
                    addComponent(_swipes);
                    addComponent(_hits);
                    break;
                case Gameplay.Character.SPACEMAN:
                    break;
                case Gameplay.Character.ALIEN:
                    throw new NotImplementedException();
                case Gameplay.Character.PIRATE:
                    throw new NotImplementedException();
                case Gameplay.Character.SKELETON:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public static Vector2 CalculateHitVector(Gameplay.Side side, Vector2 input)
        {
            if (input.LengthSquared() <= PlayerInputComponent.STICK_THRESHOLD_SQUARED)
            {
                return side == Gameplay.Side.LEFT
                    ? new Vector2(1, 0)
                    : new Vector2(-1, 0); 
            }

            var angle = Mathf.atan2(input.Y, side == Gameplay.Side.LEFT ? input.X : -input.X);
            if (angle > (float) Math.PI / 4)
            {
                angle = (float) Math.PI / 4;
            } else if (angle < -(float) Math.PI / 4)
            {
                angle = -(float) Math.PI / 4;
            }

            return side == Gameplay.Side.LEFT
                ? new Vector2(Mathf.cos(angle), Mathf.sin(angle))
                : new Vector2(-Mathf.cos(angle), Mathf.sin(angle));
        }

        public static float SignForSide(Gameplay.Side side)
        {
            return side == Gameplay.Side.LEFT ? 1.0f : -1.0f;
        }
    }

    public class AimIndicator : Entity
    {
        private Player _player;

        public AimIndicator(Player player)
        {
            _player = player;
        }

        public override void onAddedToScene()
        {
            name = $"AimIndicator_{_player.name}";
            
            var gameScene = (HandyScene)scene;
            var aimIndicatorSprite = gameScene.Textures[Sprites.AIM_INDICATOR];
            var subtex = Util.ExtractSubtextures(aimIndicatorSprite, 1, 1);
            var spriteComponent = new SpriteComponent(subtex[0]);
            spriteComponent.renderLayer = RenderLayers.PRIMARY;
            spriteComponent.localOffset = new Vector2(16*5, 0);
            spriteComponent.name = ComponentNames.AIM_INDICATOR;
            addComponent(spriteComponent);

            addComponent(new AimIndicatorComponent
            {
                player = _player
            });
        }
    }

    public class AimIndicatorComponent : Component
    {
        public Player player;
    }
}