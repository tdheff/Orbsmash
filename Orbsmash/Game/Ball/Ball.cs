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
        
        public Ball()
        {
            name = EntityNames.BALL;
            
            _velocity = new VelocityComponent(new Vector2(300, 300));
            
            addComponent(_velocity);
            addComponent(new SpriteDepthComponent { BaseRenderLayer = RenderLayers.PRIMARY});
        }

        public override void onAddedToScene()
        {
            var gameScene = (HandyScene) scene;
            
            /*
             * SPRITE AND ANIMATION
             */
            var subtextures = Util.ExtractSubtextures(gameScene.Textures[Constants.BallSprites.DEFAULT], 1, 1);
            _sprite = new AnimatableSprite(subtextures);
            _sprite.renderLayer = RenderLayers.PRIMARY;
            
            /*
             * COLLISIONS AND KINEMATICS
             */
            // must generate collider after we create the sprite,
            // otherwise the collider doesn't know how big it is (that's how it default works)
            _collider = new CircleCollider(9);
            Flags.setFlagExclusive(ref _collider.collidesWithLayers, PhysicsLayers.BACK_WALLS);
            Flags.setFlag(ref _collider.collidesWithLayers, PhysicsLayers.SIDE_WALLS);
            _kinematic.CollisionType = KinematicComponent.ECollisionType.Bounce;
            
            /*
             * STATE
             */
            _ballStateComponent = new BallStateComponent();
            
            /*
             * PARTICLES
             */
            
            _particleEmitter = new ParticleEmitterComponent(generateParticleEmitterConfig(gameScene.Textures[BallSprites.DEFAULT]));
            _particleEmitter._active = true;
            _particleEmitter.renderLayer = RenderLayers.PRIMARY;
            _particleEmitter.renderOffset = -1;
            
            addComponent(_kinematic);
            addComponent(_sprite);
            addComponent(_collider);
            addComponent(_ballStateComponent);
            addComponent(_particleEmitter);
            
            scale = new Vector2(3, 3);
        }

        private ParticleEmitterConfig generateParticleEmitterConfig(Texture2D texture)
        {
            var config = scene.content.Load<ParticleEmitterConfig>("Particles/Test_Particles");
            config.simulateInWorldSpace = true;
            config.subtexture = Util.ExtractSubtextures(texture, 1, 1)[0];
            config.blendFuncSource = Blend.One;
            config.blendFuncDestination = Blend.One;
            config.startColor = Color.Red;
            config.finishColor = Color.Orange;
            config.startColorVariance = Color.Black;
            config.finishColorVariance = Color.Black;
            config.speed = 500.0f;
            config.speedVariance = 200.0f;
            config.angle = 0;
            config.angleVariance = 360.0f;
            config.tangentialAcceleration = 0;
            config.tangentialAccelVariance = 0;
            config.particleLifespan = 0.2f;
            config.particleLifespanVariance = 0.0f;
            config.startParticleSize = 1000.0f;
            config.startParticleSizeVariance = 0;
            config.finishParticleSize = 0.0f;
            config.sourcePositionVariance = new Vector2(30, 30);
            return config;
        }
    }
}