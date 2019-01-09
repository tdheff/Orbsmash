using System;
using System.Collections.Generic;
using Handy.Animation;
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
                    if (ballStateComponent.LastHitPlayerId == playerStateMachineComponent.State.playerId &&
                        ballStateComponent.LastHitSide == playerStateMachineComponent.State.side)
                    {
                        continue;
                    }

                    ballStateComponent.IsDeadly = true;

                    var ballVelocityComponent = neighbor.entity.getComponent<VelocityComponent>();
                    ballVelocityComponent.Freeze = false;
                    ballStateComponent.BaseSpeed *= 1.05f;
                    // TODO - HIT BOOST
                    var velocity =
                        playerStateMachineComponent.State.LastVector.LengthSquared();
                    Vector2 velocityNormalized;
                    if (velocity < float.Epsilon)
                    {
                        velocityNormalized = playerStateMachineComponent.State.side == Gameplay.Side.LEFT
                            ? new Vector2(1, 0)
                            : new Vector2(-1, 0);
                    }
                    else
                    {
                        velocityNormalized = Vector2.Normalize(playerStateMachineComponent.State.LastVector);
                        if (Math.Abs(velocityNormalized.X) < Math.Abs(velocityNormalized.Y))
                        {
                            var xComponent = playerStateMachineComponent.State.side == Gameplay.Side.LEFT ? 0.70710678118f : -0.70710678118f;
                            var yComponent = Math.Sign(velocityNormalized.Y) * 0.70710678118f;
                            velocityNormalized = new Vector2(xComponent, yComponent);
                        }
                        if (playerStateMachineComponent.State.side == Gameplay.Side.LEFT)
                        {
                            if (velocityNormalized.X < 0)
                                velocityNormalized.X = -velocityNormalized.X;
                        }
                        else
                        {
                            if (velocityNormalized.X > 0)
                                velocityNormalized.X = -velocityNormalized.X;
                        }
                    }

                    ballVelocityComponent.Velocity = velocityNormalized * ballStateComponent.BaseSpeed;
                    if (!(velocityNormalized.X <= 1.0f && velocityNormalized.X >= -1.0f))
                    {
                        Console.WriteLine(velocityNormalized);
                    }
                    if (!(velocityNormalized.Y <= 1.0f && velocityNormalized.Y >= -1.0f))
                    {
                        Console.WriteLine(velocityNormalized);
                    }
                    
                    ballStateComponent.LastHitPlayerId = playerStateMachineComponent.State.playerId;
                    ballStateComponent.LastHitSide = playerStateMachineComponent.State.side;

                }
            }
        }
    }
}