using System;
using System.Collections.Generic;
using System.Linq;
using Handy.Components;
using Handy.Sound;
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
                var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
                var hits = soundEffects.First(x => x.Name == KnightSoundEffectGroups.HITS);
                var colliders = entity.getComponents<PolygonCollider>();
                Collider collider = null;
                foreach (var polygonCollider in colliders)
                {
                    if (playerState.AttackType == AttackTypes.Light && polygonCollider.name != ComponentNames.HITBOX_COLLIDER_LIGHT) continue;
                    if (playerState.AttackType == AttackTypes.Heavy && polygonCollider.name != ComponentNames.HITBOX_COLLIDER_HEAVY) continue;
                    collider = polygonCollider;
                    break;
                }

                if (collider == null)
                {
                    Debug.error($"No hitbox collider found for player {playerState.playerId}");
                    throw new Exception();
                }

                var neighbors = Physics.boxcastBroadphaseExcludingSelf(collider);
                foreach (var neighbor in neighbors)
                {
                    if (playerState.HitActive == false)
                    {
                        continue;
                    }
                    
                    if (neighbor.entity.name != EntityNames.BALL)
                    {
                        continue;
                    }

                    var ballState = neighbor.entity.getComponent<BallStateComponent>();
                    var ballVelocityComponent = neighbor.entity.getComponent<VelocityComponent>();

                    var wasLastToHit =
                        ballState.LastHitPlayerId == playerState.playerId &&
                        ballState.LastHitSide == playerState.side;
                    
                    if (wasLastToHit)
                    {
                        continue;
                    }

                    ballState.IsDeadly = true;
                    ballVelocityComponent.Freeze = false;
                    ballState.BaseSpeed *= 1.05f;
                    ballState.HitBoost = playerState.BallHitBoost;
                    ballState.DirectionVector = playerState.BallHitVector;
                    ballVelocityComponent.Velocity = ballState.BaseSpeed * ballState.HitBoost *
                                                     ballState.DirectionVector;

                    ballState.LastHitPlayerId = playerState.playerId;
                    ballState.LastHitSide = playerState.side;

                    var ballKinematicComponent = neighbor.entity.getComponent<KinematicComponent>();
                    ballKinematicComponent.LastCollision = collider;

                    var hitEffect = new HitEffect();
                    hitEffect.transform.position = neighbor.transform.position;

                    var handyScene = scene as Handy.Scene;
                    if (handyScene != null)
                    {
                        handyScene.addEntity(hitEffect);
                    }
                    else
                    {
                        Debug.warn("Entity {} has no scene", this);
                    }


                    if (!ballState.IsBeingServed)
                    {
                        var camera = handyScene.findEntity("Camera");
                        var shake = camera.getComponent<CameraShakeComponent>();
                        var cam = camera.getComponent<Camera>();
                        if (ballState.HitBoost > 1.5f)
                        {
                            shake.Shake(0.3f, ballState.HitBoost * 15);
                        }

                        var gameState = handyScene.findEntity("Game");
                        var hitStop = gameState.getComponent<HitStopComponent>();
                        if (ballState.HitBoost > 1.5f)
                        {
                            hitStop.Freeze(ballState.HitBoost / 8.0f);
                        }
                    }

                    // sound
                    hits.Play();
                    
                    // change possesion colors
                    var particles = neighbor.getComponent<ParticleEmitterComponent>();
                    if (playerState.side == Gameplay.Side.LEFT)
                    {
                        particles._emitterConfig.startColor = Color.Red;
                        particles._emitterConfig.finishColor = Color.Orange;
                    }
                    else
                    {
                        particles._emitterConfig.startColor = Color.Blue;
                        particles._emitterConfig.finishColor = Color.DarkBlue;
                    }

                    ballState.IsBeingServed = false;
                }
            }
        }
    }
}