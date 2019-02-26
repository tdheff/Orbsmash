using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class SpacemanMovementSystem : EntitySystem
    {
        public SpacemanMovementSystem(Matcher matcher) : base(matcher) { }
        public SpacemanMovementSystem() : base(new Matcher().all(
            typeof(PlayerStateComponent),
            typeof(PlayerInputComponent),
            typeof(VelocityComponent),
            typeof(SpacemanStateMachineComponent)
        )) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var input = entity.getComponent<PlayerInputComponent>();
                var velocity = entity.getComponent<VelocityComponent>();
                var knightState = entity.getComponent<SpacemanStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();

                var allowMovement = false;
                var movementMultipler = 1.0f;
                switch (knightState.StateEnum)
                {
                    case SpacemanStates.Idle:
                        allowMovement = true; // like if they're just starting to press it i guess we allow it
                        break;
                    case SpacemanStates.Walk:
                        allowMovement = true;
                        break;
                    case SpacemanStates.Attack:
                        break;
                    case SpacemanStates.KO:
                        break;
                    case SpacemanStates.Eliminated:
                        break;
                    case SpacemanStates.Shield:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (allowMovement)
                {
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