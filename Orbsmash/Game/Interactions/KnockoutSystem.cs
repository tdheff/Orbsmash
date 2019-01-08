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
    public class KnockoutSystem : EntitySystem
    {
        public KnockoutSystem() : base(new Matcher().all(typeof(PlayerStateMachineComponent))) { }
        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var playerStateMachineComponent = entity.getComponent<PlayerStateMachineComponent>();
                var colliders = entity.getComponents<BoxCollider>();
                Collider collider = null;
                foreach (var polygonCollider in colliders)
                {
                    if (polygonCollider.name != ComponentNames.PLAYER_COLLIDER) continue;
                    collider = polygonCollider;
                    break;
                }

                if (collider == null)
                {
                    Debug.error("No hitbox collider found for player {0}", playerStateMachineComponent.State.playerId);
                    throw new Exception();
                }

                if (playerStateMachineComponent.State.IsInvulnerable)
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
                    if (ballStateComponent.LastHitPlayerId == playerStateMachineComponent.State.playerId ||
                        ballStateComponent.LastHitSide == playerStateMachineComponent.State.side ||
                        !ballStateComponent.IsDeadly)
                    {
                        continue;
                    }

                    playerStateMachineComponent.State.IsKilled = true;

                }
            }
        }
    }
}