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
            { EventComponent.BuildKey(KnightAnimations.BLOCK, 5 ), new HashSet<string> { PlayerEvents.BLOCK_END }}
        };
        
        private static Dictionary<string, HashSet<string>> _wizardEventTriggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 8 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 1 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(KnightAnimations.ATTACK, 7 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
        };
        
        private PlayerSettings _settings;
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

        public Player(PlayerSettings settings)
        {
            name = $"Player_{settings.Id}";
            scale = new Vector2(2);
            _settings = settings;
            
            // physics
            _stateComponent = new PlayerStateComponent(settings.Id, settings.Side, settings.Speed, settings.StartingPosition); 
            _velocity = new VelocityComponent(new Vector2(0, 0));
            Console.WriteLine(settings.StartingPosition);
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
            switch (_settings.Character)
            {
                case Gameplay.Character.KNIGHT:
                    _state = new KnightStateMachineComponent(new KnightState());
                    _events.SetTriggers(_knightEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[PlayerAsepriteFiles.KNIGHT];
                    break;
                case Gameplay.Character.WIZARD:
                    _state = new WizardStateMachineComponent(new WizardState());
                    _events.SetTriggers(_wizardEventTriggers);
                    animationDefinition = gameScene.AnimationDefinitions[PlayerAsepriteFiles.WIZARD];
                    break;
                case Gameplay.Character.SPACEMAN:
                    throw new NotImplementedException();
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
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.WALLS);
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
        }
    }
}