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
        public float Speed { get; set; } = 300.0f;
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

            // input
            _input = new PlayerInputComponent(playerId);
            
            addComponent(_state);
            addComponent(_velocity);
            addComponent(_mainPlayerBodySprite);
            addComponent(_mainBodyAnimation);
            addComponent(_collider);
            addComponent(_input);
            addComponent(_kinematic);
            
            scale = new Vector2(8, 8);
        }
    }
}