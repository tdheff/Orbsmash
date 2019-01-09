using Handy.Components;
using HandyScene = Handy.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Orbsmash.Player;
using Handy.Animation;
using Handy.Systems;
using Nez.Particles;
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
        private ParticleEmitterComponent _particleEmitter;
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
            
            /*
             * SPRITE AND ANIMATION
             */
            _sprite = new AnimatableSprite(mySpriteDef.Subtextures);
            _sprite.renderLayer = RenderLayers.PRIMARY;
            _ballAnimation = new AnimationComponent(_sprite, AnimationContexts.BALL_SPRITE_ANIMATIONS, BallAnimations.IDLE);
            
            /*
             * COLLISIONS AND KINEMATICS
             */
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new CircleCollider(9);
            Flags.setFlagExclusive(ref _collider.collidesWithLayers, PhysicsLayers.WALLS);
            _kinematic.CollisionType = KinematicComponent.ECollisionType.Bounce;
            
            /*
             * STATE
             */
            _ballStateComponent = new BallStateComponent();
            
            /*
             * PARTICLES
             */
            
            _particleEmitter = new ParticleEmitterComponent(generateParticleEmitterConfig(mySpriteDef.Subtextures[0]));
            _particleEmitter._active = true;
            _particleEmitter.renderLayer = RenderLayers.PRIMARY;
            
            addComponent(_kinematic);
            addComponent(_sprite);
            addComponent(_ballAnimation);
            addComponent(_collider);
            addComponent(_ballStateComponent);
            addComponent(_particleEmitter);
        }

        private ParticleEmitterConfig generateParticleEmitterConfig(Subtexture tex)
        {
            var config = scene.content.Load<ParticleEmitterConfig>("Particles/Test_Particles");
            config.subtexture = null;
            config.startColor = Color.Red;
            config.finishColor = Color.Cyan;
            config.speed = 500.0f;
            return config;
        }
    }
}