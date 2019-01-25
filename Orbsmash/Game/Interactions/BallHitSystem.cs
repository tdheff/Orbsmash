using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public BallHitSystem() : base(new Matcher().all(typeof(PlayerStateComponent))) { }

        private bool checkBallVelocity(Gameplay.Side side, Vector2 ballVelocity)
        {
            return side == Gameplay.Side.LEFT ? ballVelocity.X <= 0 : ballVelocity.X >= 0;
        }
        
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var playerStateMachineComponent = entity.getComponent<PlayerStateComponent>();
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
                    Debug.error("No hitbox collider found for player {}", playerStateMachineComponent.playerId);
                    throw new Exception();
                }

                var neighbors = Physics.boxcastBroadphaseExcludingSelf(collider);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.entity.name != EntityNames.BALL)
                    {
                        continue;
                    }

                    var ballStateComponent = neighbor.entity.getComponent<BallStateComponent>();
                    var ballVelocityComponent = neighbor.entity.getComponent<VelocityComponent>();

                    var wasLastToHit =
                        ballStateComponent.LastHitPlayerId == playerStateMachineComponent.playerId &&
                        ballStateComponent.LastHitSide == playerStateMachineComponent.side;

                    var ballMovingTowards = checkBallVelocity(playerStateMachineComponent.side,
                        ballVelocityComponent.Velocity);
                    
                    if (wasLastToHit && !ballMovingTowards)
                    {
                        continue;
                    }

                    if (playerStateMachineComponent.HitActive == false)
                    {
                        continue;
                    }

                    ballStateComponent.IsDeadly = true;

                    ballVelocityComponent.Freeze = false;
                    ballStateComponent.BaseSpeed *= 1.05f;
                    // TODO - HIT BOOST
                    var velocity =
                        playerStateMachineComponent.LastVector.LengthSquared();
                    Vector2 velocityNormalized;
                    if (velocity < float.Epsilon)
                    {
                        velocityNormalized = playerStateMachineComponent.side == Gameplay.Side.LEFT
                            ? new Vector2(1, 0)
                            : new Vector2(-1, 0);
                    }
                    else
                    {
                        velocityNormalized = Vector2.Normalize(playerStateMachineComponent.LastVector);
                        if (Math.Abs(velocityNormalized.X) < Math.Abs(velocityNormalized.Y))
                        {
                            var xComponent = playerStateMachineComponent.side == Gameplay.Side.LEFT ? 0.70710678118f : -0.70710678118f;
                            var yComponent = Math.Sign(velocityNormalized.Y) * 0.70710678118f;
                            velocityNormalized = new Vector2(xComponent, yComponent);
                        }
                        if (playerStateMachineComponent.side == Gameplay.Side.LEFT)
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
                    
                    ballStateComponent.LastHitPlayerId = playerStateMachineComponent.playerId;
                    ballStateComponent.LastHitSide = playerStateMachineComponent.side;

                }
            }
        }
    }
}