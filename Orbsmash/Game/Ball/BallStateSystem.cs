using System;
using System.Collections.Generic;
using Handy.Components;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Ball
{
    public class BallStateSystem : EntitySystem
    {
        public BallStateSystem() : base(new Matcher().all(typeof(BallStateComponent), typeof(KinematicComponent)))
        {
        }

        protected override void process(List<Entity> entities)
        {
            foreach (var ball in entities)
            {
                var ballState = ball.getComponent<BallStateComponent>();
                var ballVelocityComponent = ball.getComponent<VelocityComponent>();
                var kinematicComponent = ball.getComponent<KinematicComponent>();
                if (kinematicComponent.LastCollision != null && Flags.isUnshiftedFlagSet(kinematicComponent.LastCollision.physicsLayer, PhysicsLayers.BACK_WALLS))
                {
                    ballState.LastHitPlayerId = -1;
                    // TODO - this seems vaguely dangerous?
                    ballVelocityComponent.Velocity = ballVelocityComponent.Velocity / ballState.HitBoost;
                    ballState.HitBoost = 1.0f;
                }

                var particles = ball.getComponent<ParticleEmitterComponent>();
                particles._emitterConfig.startParticleSize = 5.0f + ballVelocityComponent.Velocity.Length() / 8;
                
            }
        }
    }
}