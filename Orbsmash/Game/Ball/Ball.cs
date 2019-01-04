using Handy.Components;
using HandyScene = Handy.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Orbsmash.Player;
using Handy.Animation;
using Orbsmash.Constants;

namespace Orbsmash.Ball
{
    public class Ball : Entity
    {
        private readonly VelocityComponent _velocity;
        private CircleCollider _collider;
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private AnimatableSprite _sprite;
        private AnimationComponent _ballAnimation;
        private BallStateComponent _ballStateComponent;
        private string ballSprite;
        
        public Ball(string sprite)
        {
            ballSprite = sprite;
            name = EntityNames.BALL;
            
            _velocity = new VelocityComponent(new Vector2(300, 300));
            
            addComponent(_velocity);
        }

        public override void onAddedToScene()
        {
            var gameScene = (HandyScene) scene;
            var mySpriteDef = gameScene.SpriteDefinitions[ballSprite];
            _sprite = new AnimatableSprite(mySpriteDef.Subtextures);
            _ballAnimation = new AnimationComponent(_sprite, AnimationContexts.BALL_SPRITE_ANIMATIONS, BallAnimations.IDLE);
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new CircleCollider(9);
            Flags.setFlagExclusive(ref _collider.collidesWithLayers, PhysicsLayers.WALLS);
            //Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.WALLS);
            _kinematic.CollisionType = KinematicComponent.ECollisionType.Bounce;
            _ballStateComponent = new BallStateComponent();
            addComponent(_kinematic);
            addComponent(_sprite);
            addComponent(_ballAnimation);
            addComponent(_collider);
            addComponent(_ballStateComponent);
        }
    }
}