using Handy.Animation;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;
using HandyScene = Handy.Scene;

namespace Orbsmash.Ball
{
    public class Ball : Entity
    {
        private readonly KinematicComponent _kinematic = new KinematicComponent();
        private readonly VelocityComponent _velocity;
        private AnimationComponent _ballAnimation;
        private BallStateComponent _ballStateComponent;
        private CircleCollider _collider;
        private AnimatableSprite _sprite;
        private readonly string ballSprite;

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
            _ballAnimation =
                new AnimationComponent(_sprite, AnimationContexts.BALL_SPRITE_ANIMATIONS, BallAnimations.IDLE);
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