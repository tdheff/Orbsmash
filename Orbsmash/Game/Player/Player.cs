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
        private readonly PlayerStateMachineComponent _state;
        private readonly PlayerInputComponent _input;
        private readonly VelocityComponent _velocity;
        private BoxCollider _collider;
        private PolygonCollider _hitbox;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private AnimationComponent _mainBodyAnimation;
        private AnimatableSprite _mainPlayerBodySprite;
        private EventComponent _events = new EventComponent();
        private string playerSprite;

        public Player(PlayerSettings settings)
        {
            name = $"Player_{settings.Id}";
            playerSprite = settings.Sprite;
            //scale = new Vector2(2);
            
            // physics
            _state = new PlayerStateMachineComponent(new PlayerState(settings.Id, settings.Side, settings.Speed));
            _velocity = new VelocityComponent(new Vector2(0, 0));
            
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
            var mySpriteDef = gameScene.SpriteDefinitions[playerSprite];
            _mainPlayerBodySprite = new AnimatableSprite(mySpriteDef.Subtextures);
            _mainBodyAnimation = new AnimationComponent(_mainPlayerBodySprite, AnimationContexts.PLAYER_SPRITE_ANIMATIONS, PlayerAnimations.IDLE_HORIZONTAL);
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new BoxCollider(15, 10);
            _hitbox = new PolygonCollider(scene.content.Load<Polygon>(Hitboxes.KNIGHT_SWING_HITBOX).points);
            _hitbox.isTrigger = true;
            addComponent(_hitbox);
            // _hitbox = new PolygonCollider([]);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);
            addComponent(_collider);
        }
    }
}