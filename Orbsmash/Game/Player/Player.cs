using System;
using System.Collections.Generic;
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
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 11 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 2 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 4 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(KnightAnimations.CHARGE, 4 ), new HashSet<string> { PlayerEvents.CHARGE_WINDUP_END }},
            { EventComponent.BuildKey(KnightAnimations.BLOCK, 5 ), new HashSet<string> { PlayerEvents.BLOCK_END }},
            { EventComponent.BuildKey(KnightAnimations.BLOCK_HIT, 5 ), new HashSet<string> { PlayerEvents.BLOCK_HIT_END }},
            { EventComponent.BuildKey(KnightAnimations.KO, 6 ), new HashSet<string> { PlayerEvents.KO_BOUNCE }},
            { EventComponent.BuildKey(KnightAnimations.KO, 10 ), new HashSet<string> { PlayerEvents.KO_END }},
        };
        
        private static Dictionary<string, HashSet<string>> _wizardEventTriggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 8 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 1 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 7 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
        };
        
        private static Dictionary<string, HashSet<string>> _spacemanEventTriggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(SpacemanAnimations.ATTACK, 19 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 9 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 11 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
        };

        public PlayerSettings Settings;
        private PlayerStateComponent _stateComponent;
        private Component _state;
        private PlayerInputComponent _input;
        private VelocityComponent _velocity;
        private BoxCollider _collider;
        private PolygonCollider _hitbox;
        private KinematicComponent _kinematic = new KinematicComponent();
        private AnimationComponent _mainBodyAnimation;
        private AnimatableSprite _mainPlayerBodySprite;
        private EventComponent _events = new EventComponent();
        private SoundEffectGroupComponent _swipes;
        private SoundEffectGroupComponent _hits;
        private SoundEffectGroupComponent _steps;

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
            switch (Settings.Character)
            {
                case Gameplay.Character.KNIGHT:
                    _state = new KnightStateMachineComponent(new KnightState());
                    _events.SetTriggers(_knightEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.KNIGHT];
                    break;
                case Gameplay.Character.WIZARD:
                    _state = new WizardStateMachineComponent(new WizardState());
                    _events.SetTriggers(_wizardEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.WIZARD];
                    break;
                case Gameplay.Character.SPACEMAN:
                    _state = new SpacemanStateMachineComponent(new SpacemanState());
                    _events.SetTriggers(_spacemanEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[AsepriteFiles.SPACEMAN];
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
            _hitbox = new PolygonCollider(scene.content.Load<Polygon>(Hitboxes.KNIGHT_SWING_HITBOX).points);
            _hitbox.isTrigger = true;
            _hitbox.name = ComponentNames.HITBOX_COLLIDER;

            addComponent(_state);
            addComponent(_hitbox);
            // _hitbox = new PolygonCollider([]);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);
            addComponent(_collider);
            
            if (_stateComponent.side == Gameplay.Side.RIGHT)
            {
                _mainPlayerBodySprite.flipX = true;
                _collider.FlipX = true;
                _hitbox.FlipX = true;
            }

            SetupSound();
        }

        private void SetupSound()
        {
            // at this point mostly for knight, will make a switch statement at some point
            var gameScene = (HandyScene)scene;
            var sounds = gameScene.Sounds;
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
        }

        public static Vector2 CalculateHitVector(Gameplay.Side side, Vector2 input)
        {
            if (input.LengthSquared() < PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
            {
                return side == Gameplay.Side.LEFT ? new Vector2(1, 0) : new Vector2(-1, 0);
            }

            var hitVector = new Vector2(input.X, input.Y);
            if (side == Gameplay.Side.LEFT)
            {
                if (hitVector.X < 0)
                {
                    hitVector.X *= -1;
                }
            }
            else
            {
                if (hitVector.X > 0)
                {
                    hitVector.X *= -1;
                }
            }

            if (Math.Abs(hitVector.Y) > Math.Abs(hitVector.X))
            {
                var x = Math.Sign(hitVector.X) * MathUtil.SQRT_ONE_HALF;
                var y = Math.Sign(hitVector.Y) * MathUtil.SQRT_ONE_HALF;
                hitVector = new Vector2(x, y);
            }
            hitVector.Normalize();
            return hitVector;
        }

        public static float SignForSide(Gameplay.Side side)
        {
            return side == Gameplay.Side.LEFT ? 1.0f : -1.0f;
        }
    }
}