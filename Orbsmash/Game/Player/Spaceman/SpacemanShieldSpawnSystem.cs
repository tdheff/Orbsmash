using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework;
using Nez;
using Optional;
using Optional.Unsafe;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class SpacemanShieldSpawnSystem : EntitySystem
    {
        public SpacemanShieldSpawnSystem() : base(new Matcher().all(typeof(SpacemanStateMachineComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var playerState = entity.getComponent<PlayerStateComponent>();
                var spacemanState = entity.getComponent<SpacemanStateMachineComponent>();
                if (spacemanState.State.ShieldSpawn.HasValue)
                {
                    var shield = new SpacemanShield(playerState.side);
                    shield.position = spacemanState.State.ShieldSpawn.ValueOrFailure();
                    scene.addEntity(shield);
                    spacemanState.State.ShieldSpawn = Option.None<Vector2>();
                }
            }
        }
    }
    
    public class SpacemanShieldSystem : EntitySystem
    {
        public SpacemanShieldSystem() : base(new Matcher().all(typeof(SpacemanShieldComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var events = entity.getComponent<EventComponent>();

                if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.BLOCK_END))
                {
                    entity.destroy();
                }
            }
        }
    }
}