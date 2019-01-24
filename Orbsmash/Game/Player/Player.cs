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
        private static Dictionary<string, HashSet<string>> _eventTriggers = new Dictionary<string, HashSet<string>>
        {
            { EventComponent.BuildKey(PlayerAnimations.SWING, 11 ), new HashSet<string> { PlayerEvents.PLAYER_SWING_END }},
            { EventComponent.BuildKey(PlayerAnimations.SWING, 2 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_START }},
            { EventComponent.BuildKey(PlayerAnimations.SWING, 4 ), new HashSet<string> { PlayerEvents.PLAYER_HIT_END }},
            { EventComponent.BuildKey(PlayerAnimations.CHARGE, 4 ), new HashSet<string> { PlayerEvents.CHARGE_WINDUP_END }},
            { EventComponent.BuildKey(PlayerAnimations.BLOCK, 5 ), new HashSet<string> { PlayerEvents.BLOCK_END }}
        };

        
        private PlayerSettings _settings;
        private PlayerStateMachineComponent _state;
        private PlayerInputComponent _input;
        private VelocityComponent _velocity;
        private BoxCollider _collider;
        private PolygonCollider _hitbox;
        private KinematicComponent _kinematic = new KinematicComponent();
        private AnimationComponent _mainBodyAnimation;
        private AnimatableSprite _mainPlayerBodySprite;
        private EventComponent _events = new EventComponent(_eventTriggers);

        public Player(PlayerSettings settings)
        {
            name = $"Player_{settings.Id}";
            scale = new Vector2(2);
            _settings = settings;
            
            // physics
            _state = new PlayerStateMachineComponent(new PlayerState(settings.Id, settings.Side, settings.Speed, settings.StartingPosition));
            _velocity = new VelocityComponent(new Vector2(0, 0));
            Console.WriteLine(settings.StartingPosition);
            transform.position = settings.StartingPosition;
            
            // input
            _input = new PlayerInputComponent(settings.Id);
            addComponent(_events);
            addComponent(_state);
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
                    animationDefinition = gameScene.AnimationDefinitions[PlayerAsepriteFiles.KNIGHT];;
                    break;
                case Gameplay.Character.WIZARD:
                    throw new NotImplementedException();
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
            _mainBodyAnimation = new AnimationComponent(_mainPlayerBodySprite, animationDefinition, PlayerAnimations.IDLE_HORIZONTAL);
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
            
            addComponent(_hitbox);
            // _hitbox = new PolygonCollider([]);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);
            addComponent(_collider);
            
            if (_state.State.side == Gameplay.Side.RIGHT)
            {
                _mainPlayerBodySprite.flipX = true;
                _collider.FlipX = true;
                _hitbox.FlipX = true;
            }
        }
    }
}