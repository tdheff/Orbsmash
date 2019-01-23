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
            Flags.setFlagExclusive(ref _collider.collidesWithLayers, PhysicsLayers.WALLS);
            _kinematic.CollisionType = KinematicComponent.ECollisionType.Bounce;
            
            /*
             * STATE
             */
            _ballStateComponent = new BallStateComponent();
            
            /*
             * PARTICLES
             */
            
            _particleEmitter = new ParticleEmitterComponent(generateParticleEmitterConfig());
            _particleEmitter._active = true;
            _particleEmitter.renderLayer = RenderLayers.PRIMARY;
            
            addComponent(_kinematic);
            addComponent(_sprite);
            addComponent(_collider);
            addComponent(_ballStateComponent);
            addComponent(_particleEmitter);
        }

        private ParticleEmitterConfig generateParticleEmitterConfig()
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