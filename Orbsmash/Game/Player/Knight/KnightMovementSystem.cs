using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class KnightMovementSystem : EntitySystem
    {
        public KnightMovementSystem(Matcher matcher) : base(matcher) { }
        public KnightMovementSystem() : base(new Matcher().all(
            typeof(PlayerStateComponent),
            typeof(PlayerInputComponent),
            typeof(VelocityComponent),
            typeof(KnightStateMachineComponent)
        )) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var input = entity.getComponent<PlayerInputComponent>();
                var velocity = entity.getComponent<VelocityComponent>();
                var knightState = entity.getComponent<KnightStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();

                velocity.Velocity = input.MovementStick * playerState.Speed;
                var allowMovement = false;
                var movementMultipler = 1.0f;
                switch (knightState.StateEnum)
                {
                    case KnightStates.Idle:
                        allowMovement = true; // like if they're just starting to press it i guess we allow it
                        break;
                    case KnightStates.Walk:
                        allowMovement = true;
                        break;
                    case KnightStates.Dash:
                        allowMovement = true;
                        movementMultipler = 1.2f;
                        break;
                    case KnightStates.Charge:
                        break;
                    case KnightStates.Swing:
                        break;
                    case KnightStates.Dead:
                        break;
                    case KnightStates.Block:
                        break;
                    case KnightStates.BlockHit:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (allowMovement)
                {
                    velocity.Velocity = input.MovementStick * playerState.Speed * movementMultipler;
                    if (input.MovementStick.LengthSquared() >= PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        playerState.LastVector = Vector2.Normalize(input.MovementStick);
                    }
                    else
                    {
                        playerState.LastVector = Vector2.Zero;
                    }

                    var up = new Vector2(0, 1);
                    var down = new Vector2(0, -1);
                    var left = new Vector2(-1, 0);
                    var right = new Vector2(1, 0);
                    if (Vector2.Dot(up, playerState.LastVector) > 0.707)
                    {
                        playerState.LastDirection = Gameplay.Direction.UP;
                    }
                    if (Vector2.Dot(down, playerState.LastVector) > 0.707)
                    {
                        playerState.LastDirection = Gameplay.Direction.DOWN;
                    }
                    if (Vector2.Dot(left, playerState.LastVector) > 0.707)
                    {
                        playerState.LastDirection = playerState.side == Gameplay.Side.RIGHT ? Gameplay.Direction.FORWARD : Gameplay.Direction.BACKWARD;
                    }
                    if (Vector2.Dot(right, playerState.LastVector) > 0.707)
                    {
                        playerState.LastDirection = playerState.side == Gameplay.Side.RIGHT ? Gameplay.Direction.BACKWARD : Gameplay.Direction.FORWARD;
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