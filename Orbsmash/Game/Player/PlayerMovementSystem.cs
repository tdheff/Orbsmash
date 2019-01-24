using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

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
                var state = entity.getComponent<PlayerStateMachineComponent>().State;

                velocity.Velocity = input.MovementStick * state.Speed;
                var allowMovement = false;
                switch (state.StateEnum)
                {
                    case PlayerStates.Idle:
                        allowMovement = true; // like if they're just starting to press it i guess we allow it
                        break;
                    case PlayerStates.Walk:
                        allowMovement = true;
                        break;
                    case PlayerStates.Dash:
                        allowMovement = true;
                        break;
                    case PlayerStates.Charge:
                        break;
                    case PlayerStates.Swing:
                        break;
                    case PlayerStates.Dead:
                        break;
                    case PlayerStates.Block:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (allowMovement)
                {
                    velocity.Velocity = input.MovementStick * state.Speed;
                    if (input.MovementStick.LengthSquared() >= PlayerState.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        state.LastVector = Vector2.Normalize(input.MovementStick);
                    }
                    else
                    {
                        state.LastVector = Vector2.Zero;
                    }

                    var up = new Vector2(0, 1);
                    var down = new Vector2(0, -1);
                    var left = new Vector2(-1, 0);
                    var right = new Vector2(1, 0);
                    if (Vector2.Dot(up, state.LastVector) > 0.707)
                    {
                        state.LastDirection = Gameplay.Direction.UP;
                    }
                    if (Vector2.Dot(down, state.LastVector) > 0.707)
                    {
                        state.LastDirection = Gameplay.Direction.DOWN;
                    }
                    if (Vector2.Dot(left, state.LastVector) > 0.707)
                    {
                        state.LastDirection = state.side == Gameplay.Side.RIGHT ? Gameplay.Direction.FORWARD : Gameplay.Direction.BACKWARD;
                    }
                    if (Vector2.Dot(right, state.LastVector) > 0.707)
                    {
                        state.LastDirection = state.side == Gameplay.Side.RIGHT ? Gameplay.Direction.BACKWARD : Gameplay.Direction.FORWARD;
                    }
                }
                else
                {
                    velocity.Velocity = new Vector2(0, 0);
                }
            }

        }
    }
}