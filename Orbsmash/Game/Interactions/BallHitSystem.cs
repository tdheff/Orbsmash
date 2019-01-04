using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Player;

namespace Orbsmash.Game.Interactions
{
    public class BallHitSystem : EntitySystem
    {
        public BallHitSystem() : base(new Matcher().all(typeof(PlayerStateMachineComponent))) { }
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var playerStateMachineComponent = entity.getComponent<PlayerStateMachineComponent>();
                var colliders = entity.getComponents<PolygonCollider>();
                Collider collider = null;
                foreach (var polygonCollider in colliders)
                {
                    if (polygonCollider.name != ComponentNames.HITBOX_COLLIDER) continue;
                    collider = polygonCollider;
                    break;
                }

                if (collider == null)
                {
                    Debug.error("No hitbox collider found for player {}", playerStateMachineComponent.State.playerId);
                    throw new Exception();
                }

                // TODO - actually turn the hitbox on and off, this is a bandaid. that should be done with anim tags
                if (playerStateMachineComponent.State.StateEnum != PlayerStates.Swing)
                {
                    continue;
                }

                var neighbors = Physics.boxcastBroadphaseExcludingSelf(collider);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.entity.name != EntityNames.BALL)
                    {
                        continue;
                    }

                    var ballStateComponent = neighbor.entity.getComponent<BallStateComponent>();
                    if (ballStateComponent.LastHitPlayerId == playerStateMachineComponent.State.playerId)
                    {
                        continue;
                    }

                    var ballVelocityComponent = neighbor.entity.getComponent<VelocityComponent>();
                    Console.WriteLine("Setting ball speed to {0}", Vector2.Normalize(playerStateMachineComponent.State.LastVector) * ballVelocityComponent.Velocity.Length());
                    if (playerStateMachineComponent.State.LastVector.LengthSquared() < float.Epsilon)
                    {
                        ballVelocityComponent.Velocity = new Vector2(1, 0) * ballVelocityComponent.Velocity.Length();
                    }
                    else
                    {
                        ballVelocityComponent.Velocity = Vector2.Normalize(playerStateMachineComponent.State.LastVector) * ballVelocityComponent.Velocity.Length();
                    }
                    ballStateComponent.LastHitPlayerId = playerStateMachineComponent.State.playerId;
                }
            }
        }
    }
}