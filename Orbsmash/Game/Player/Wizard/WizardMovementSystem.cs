using System;
using System.Collections.Generic;
using Handy.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class WizardMovementSystem : EntitySystem
    {
        public WizardMovementSystem(Matcher matcher) : base(matcher) { }
        public WizardMovementSystem() : base(new Matcher().all(
            typeof(PlayerStateComponent),
            typeof(PlayerInputComponent),
            typeof(VelocityComponent),
            typeof(WizardStateMachineComponent)
        )) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var input = entity.getComponent<PlayerInputComponent>();
                var velocity = entity.getComponent<VelocityComponent>();
                var wizardState = entity.getComponent<WizardStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();

                var freeMovement = false;
                var lockMovement = false;
                var movementMultipler = 1.0f;
                switch (wizardState.StateEnum)
                {
                    case WizardStates.Idle:
                        freeMovement = true; // like if they're just starting to press it i guess we allow it
                        break;
                    case WizardStates.Walk:
                        freeMovement = true;
                        break;
                    case WizardStates.Attack:
                        lockMovement = true;
                        break;
                    case WizardStates.PreGlide:
                        lockMovement = true;
                        break;
                    case WizardStates.Glide:
                        velocity.Velocity = wizardState.GlideDirection * WizardState.GLIDE_SPEED;
                        break;
                    case WizardStates.Eliminated:
                        lockMovement = true;
                        break;
                    case WizardStates.KO:
                        break;
                    case WizardStates.Charge:
                        lockMovement = true;
                        break;
                    case WizardStates.ChargeHeavy:
                        lockMovement = true;
                        break;
                    case WizardStates.AttackHeavy:
                        lockMovement = true;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                if (freeMovement)
                {
                    var targetVelocity = input.MovementStick * MovementSpeeds.LOW * movementMultipler;
                    if (targetVelocity.LengthSquared() > 0.01)
                    {
                        velocity.Velocity = Vector2.Lerp(velocity.Velocity, targetVelocity,
                            0.08f);
                    }
                    else
                    {
                        velocity.Velocity = Vector2.Lerp(velocity.Velocity, Vector2.Zero,
                            0.08f);
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
                if (lockMovement)
                {
                    velocity.Velocity = new Vector2(0, 0);
                }
            }

        }
    }
}