using System;
using System.Collections.Generic;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Handy;
using Handy.Animation;
using Orbsmash.Animation;

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
        private readonly PlayerStateComponent _state;
        private readonly PlayerInputComponent _input;
        private readonly VelocityComponent _velocity;
        private readonly BoxCollider _collider;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private readonly SpriteComponent _sprite;

        public Player(int playerId, Texture2D texture, Constants.Side side = Constants.Side.Left)
        {
            // physics
            _state = new PlayerStateComponent(playerId, playerId, side);
            _velocity = new VelocityComponent(new Vector2(0, 0));
            _collider = new BoxCollider();
            
            // animation
            List<Subtexture> subtextures = Util.ExtractSubtextures(texture, 19, 1);
            _sprite = new SpriteComponent(subtextures[0]);
            
            // IDLE
            List<Subtexture> frames = subtextures.Take(2).ToList();
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
            
            // Initialize
            _sprite.play(EAnimations.PlayerIdle);
            
            // input
            _input = new PlayerInputComponent(playerId);
            
            addComponent(_state);
            addComponent(_velocity);
            addComponent(_sprite);
            addComponent(_collider);
            addComponent(_input);
            addComponent(_kinematic);
            
            scale = new Vector2(8, 8);
        }
    }
}

//using System;
//using Microsoft.Xna.Framework;
//using Monocle;
//using Orbsmash.Handy;
//
//namespace Orbsmash.Player
//{
//    public enum PlayerAnimations { Idle, WalkHorizontal, WalkVertical, Charge, Swing, Death }
//    
//    [Tracked()]
//    public class Player : Entity
//    {
//        public int PlayerId { get; private set; }
//        private int _deviceId;
//        public int DeviceId
//        {
//            get => _deviceId;
//            set
//            {
//                _deviceId = value;
//                if (_input != null)
//                    _input.DeviceId = value;
//            }
//        }
//
//        public Constants.Side Side { get; private set; }
//        
//        private readonly Spritesheet<PlayerAnimations> _spritesheet;
//        private readonly CollidableComponent _collidable;
//        private readonly PlayerInputComponent _input;
//        private readonly PlayerControlledKinematicBodyComponent _kinematicBody;
//
//        public Player(int playerId, Constants.Side side = Constants.Side.Left)
//        {
//            PlayerId = playerId;
//            DeviceId = playerId;
//            Side = side;
//            
//            // CREATE COLLIDER
//            Collider = new Hitbox(32*8, 32*8);
//            _collidable = new CollidableComponent(true, true, true)
//            {
//                Collider = new Hitbox(10, 10)
//            };
//            
//            // CREATE SPRITESHEET
//            _spritesheet = new Spritesheet<PlayerAnimations>(MTexture.FromFile("../../../../Content/Player0.png"), 32, 32)
//            {
//                CurrentFrame = 2,
//                Scale = new Vector2(8, 8),
//                FlipX = Side == Constants.Side.Right
//            };
//            
//            // CREATE INPUT
//            _input = new PlayerInputComponent(true, false, DeviceId);
//            
//            // CREATE KINEMATIC
//            _kinematicBody = new PlayerControlledKinematicBodyComponent(500);
//            
//            // TAGS
////            AddTag();
//
//            // ADD COMPONENTS
//            Add(new Component[] {_input, _collidable, _kinematicBody, _spritesheet});            
//        }
//    }
//}