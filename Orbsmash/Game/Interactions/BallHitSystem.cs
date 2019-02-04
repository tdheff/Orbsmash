using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Game.Effects;
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
                var playerState = entity.getComponent<PlayerStateComponent>();
                var soundState = entity.getComponent<PlayerSoundStateComponent>();
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
                    Debug.error($"No hitbox collider found for player {playerState.playerId}");
                    throw new Exception();
                }
                if(soundState == null)
                {
                    Debug.error($"No sound state found for player {playerState.playerId}");
                }

                var neighbors = Physics.boxcastBroadphaseExcludingSelf(collider);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.entity.name != EntityNames.BALL)
                    {
                        continue;
                    }

                    var ballState = neighbor.entity.getComponent<BallStateComponent>();
                    var ballVelocityComponent = neighbor.entity.getComponent<VelocityComponent>();

                    var wasLastToHit =
                        ballState.LastHitPlayerId == playerState.playerId &&
                        ballState.LastHitSide == playerState.side;

                    var ballMovingTowards = checkBallVelocity(playerState.side,
                        ballVelocityComponent.Velocity);
                    
                    if (wasLastToHit && !ballMovingTowards)
                    {
                        continue;
                    }

                    if (playerState.HitActive == false)
                    {
                        continue;
                    }

                    ballState.IsDeadly = true;

                    ballVelocityComponent.Freeze = false;
                    ballState.BaseSpeed *= 1.05f;
                    ballState.HitBoost = playerState.BallHitBoost;
                    ballVelocityComponent.Velocity = ballState.BaseSpeed * ballState.HitBoost *
                                                     playerState.BallHitVector;

                    ballState.LastHitPlayerId = playerState.playerId;
                    ballState.LastHitSide = playerState.side;

                    var hitEffect = new HitEffect();
                    hitEffect.transform.position = neighbor.transform.position;
                    Handy.Scene handyScene = scene as Handy.Scene;
                    handyScene.addEntity(hitEffect);

                    // sound
                    soundState.PlayBallHit = true;
                }
            }
        }
    }
}