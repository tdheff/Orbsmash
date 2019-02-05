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
                var kinematicComponent = ball.getComponent<KinematicComponent>();
                if (kinematicComponent.LastCollision != null && Flags.isUnshiftedFlagSet(kinematicComponent.LastCollision.physicsLayer, PhysicsLayers.BACK_WALLS))
                {
                    var ballStateComponent = ball.getComponent<BallStateComponent>();
                    ballStateComponent.LastHitPlayerId = -1;
                }
            }
        }
    }
}