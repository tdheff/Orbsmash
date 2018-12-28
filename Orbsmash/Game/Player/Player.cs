using Handy.Components;
using HandyScene = Handy.Scene;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Handy.Animation;

namespace Orbsmash.Player
{
    public enum PlayerAnimations { Idle, WalkHorizontal, WalkVertical, Charge, Swing, Death }

    // <summary>
    // Component that contains state unique to the player
    // </summary>
    public class PlayerStateComponent : Component
    {
        public int PlayerId { get; private set; }
        public float Speed { get; set; } = 500.0f;
        public Constants.Side Side { get; private set; }

        public PlayerStateComponent(int playerId, int deviceId, Constants.Side side)
        {
            PlayerId = playerId;
            Side = side;
        }
    }
    
    // <summary>
    // Entity representing a player in the game
    // </summary>
    public class Player : Entity
    {
        private readonly PlayerStateMachineComponent _state;
        private readonly PlayerInputComponent _input;
        private readonly VelocityComponent _velocity;
        private readonly BoxCollider _collider;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private readonly AnimationComponent<Constants.EAnimations> _mainBodyAnimation;
        private readonly AnimatableSprite<Constants.EAnimations> _mainPlayerBodySprite;

        public Player(int playerId, Texture2D texture, Constants.Side side = Constants.Side.Left)
        {
            name = $"Player_{playerId}";
            
            // physics
            _state = new PlayerStateMachineComponent(new PlayerState(playerId, side));
            _velocity = new VelocityComponent(new Vector2(0, 0));
            _collider = new BoxCollider();
            var gameScene = (HandyScene) scene;
            var mySpriteDef = gameScene.SpriteDefinitions["Player0"];
            _mainPlayerBodySprite = new AnimatableSprite<Constants.EAnimations>(mySpriteDef.Subtextures);
            _mainBodyAnimation = new AnimationComponent<Constants.EAnimations>(_mainPlayerBodySprite, "PlayerAnimations", Constants.EAnimations.PlayerIdle);
            
            /*
            // animation
            var subtextures = Util.ExtractSubtextures(texture, 19, 1);
            _sprite = new SpriteComponent(subtextures[0]);

            // IDLE
            var frames = subtextures.Take(2).ToList();
            _sprite.addAnimation(EAnimations.PlayerIdle, new SpriteAnimation(frames));
            // WALK VERTICAL
            frames = subtextures.Skip(2).Take(2).ToList();
            _sprite.addAnimation(EAnimations.PlayerWalkVertical, new SpriteAnimation(frames));
            // WALK HORIZONTAL
            frames = subtextures.Skip(4).Take(2).ToList();
            _sprite.addAnimation(EAnimations.PlayerWalkHorizontal, new SpriteAnimation(frames));
            // CHARGE
            frames = subtextures.Skip(6).Take(4).ToList();
            var chargeAnimation = new SpriteAnimation(frames)
            {
                completionBehavior = AnimationCompletionBehavior.RemainOnFinalFrame
            };
            _sprite.addAnimation(EAnimations.PlayerCharge, chargeAnimation);
            // SWING
            frames = subtextures.Skip(10).Take(6).ToList();
            _sprite.addAnimation(EAnimations.PlayerSwing, new SpriteAnimation(frames));
            // DIE
            frames = subtextures.Skip(16).Take(3).ToList();
            var dieAnimation = new SpriteAnimation(frames)
            {
                completionBehavior = AnimationCompletionBehavior.RemainOnFinalFrame
            };
            _sprite.addAnimation(EAnimations.PlayerDie, dieAnimation);
            
            */
            
            // input
            _input = new PlayerInputComponent(playerId);
            
            addComponent(_state);
            addComponent(_velocity);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);
            addComponent(_collider);
            addComponent(_input);
            addComponent(_kinematic);
        }
    }
}