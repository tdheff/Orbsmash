using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Orbsmash.Player
{
    public class PlayerMovementSystem : EntitySystem
    {
        public PlayerMovementSystem(Matcher matcher) : base(matcher) { }
        public PlayerMovementSystem() : base(new Matcher().all(
            typeof(PlayerInputComponent),
            typeof(VelocityComponent),
            typeof(PlayerStateMachineComponent)
        )) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var input = entity.getComponent<PlayerInputComponent>();
                var velocity = entity.getComponent<VelocityComponent>();
                var state = entity.getComponent<PlayerStateMachineComponent>();

                velocity.Velocity = input.MovementStick * state.State.Speed;
            }
            
        }
    }
}