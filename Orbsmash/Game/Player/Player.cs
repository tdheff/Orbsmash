using System;
using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.PhysicsShapes;
using Orbsmash.Constants;
using Orbsmash.Game;
using HandyScene = Handy.Scene;

namespace Orbsmash.Player
{
    // <summary>
    // Entity representing a player in the game
    // </summary>
    public class Player : Entity
    {
        private BoxCollider _collider;
        private readonly EventComponent _events = new EventComponent();
        private PolygonCollider _hitbox;
        private readonly PlayerInputComponent _input;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private AnimationComponent _mainBodyAnimation;
        private AnimatableSprite _mainPlayerBodySprite;
        private readonly PlayerStateMachineComponent _state;
        private readonly VelocityComponent _velocity;
        private readonly string playerSprite;

        public Player(PlayerSettings settings)
        {
            name = $"Player_{settings.Id}";
            playerSprite = settings.Sprite;
            scale = new Vector2(2);

            // physics
            _state = new PlayerStateMachineComponent(new PlayerState(settings.Id, settings.Side, settings.Speed,
                settings.StartingPosition));
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
            var gameScene = (HandyScene) scene;
            var mySpriteDef = gameScene.SpriteDefinitions[playerSprite];
            _mainPlayerBodySprite = new AnimatableSprite(mySpriteDef.Subtextures);
            _mainBodyAnimation = new AnimationComponent(_mainPlayerBodySprite,
                AnimationContexts.PLAYER_SPRITE_ANIMATIONS, PlayerAnimations.IDLE_HORIZONTAL);
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new BoxCollider(15, 10);
            _collider.name = ComponentNames.PLAYER_COLLIDER;
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