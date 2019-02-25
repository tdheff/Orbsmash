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
                var hits = soundEffects.First(x => x.Name == KnightSoundEffectGroups.HITS); // wizard is named the same
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
                    ballVelocityComponent.Velocity = ballState.BaseSpeed * ballState.HitBoost *
                                                     playerState.BallHitVector;

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

                    var camera = handyScene.findEntity("Camera");
                    var shake = camera.getComponent<CameraShakeComponent>();
                    var cam = camera.getComponent<Camera>();
                    if (ballState.HitBoost > 1.5f)
                    {
                        shake.Shake(0.3f, ballState.HitBoost * 15);
                    }
                    
                    // sound
                    hits.Play();
                    
                    //
                    // CHARACTER SPECIFIC HIT ELEMENTS
                    //
                    
                    // KNIGHT
                    var player = (Player.Player)entity;
                    switch (player.Settings.Character)
                    {
                        case Gameplay.Character.KNIGHT:
                            knightHit(player, ballVelocityComponent);
                            break;
                        case Gameplay.Character.WIZARD:
                            break;
                        case Gameplay.Character.SPACEMAN:
                            break;
                        case Gameplay.Character.ALIEN:
                            break;
                        case Gameplay.Character.PIRATE:
                            break;
                        case Gameplay.Character.SKELETON:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    var gameState = handyScene.findEntity("Game");
                    var hitStop = gameState.getComponent<HitStopComponent>();
                    if (ballState.HitBoost > 1.5f)
                    {
                        hitStop.Freeze(ballState.HitBoost / 8.0f);
                    }
                    
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
                }
            }
        }

        private void knightHit(Player.Player player, VelocityComponent ballVelocity)
        {
            var events = player.getComponent<EventComponent>();
            var knightState = player.getComponent<KnightStateMachineComponent>();

            if (knightState.State.StateEnum == KnightStates.Block)
            {
                events.FireEvent(PlayerEvents.BLOCK_HIT);
                knightState.State.BlockHitVector = -ballVelocity.Velocity / 2;
            }
        }
    }
}