using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class WizardStateMachineSystem : PushdownAutomatonSystem<WizardStates, WizardState, WizardStateMachineComponent>
    {
        public WizardStateMachineSystem() : base(new Matcher().all(
            typeof(WizardStateMachineComponent),
            typeof(PlayerInputComponent),
            typeof(EventComponent)
        )) { }

        protected override void Update(Entity entity, WizardStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;
            var input = entity.getComponent<PlayerInputComponent>();
            state.GlideCooldown -= Time.deltaTime;
            state.ImmaterialCooldown -= Time.deltaTime;
            playerState.BallHitBoost = 1.0f;
            playerState.BallHitVector = Player.calculateHitVector(playerState.side, input.MovementStick);
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    break;
                case WizardStates.Attack:
                    break;
                case WizardStates.Glide:
                    state.GlideTime += Time.deltaTime;
                    break;
                case WizardStates.Immaterial:
                    state.ImmaterialTime += Time.deltaTime;
                    break;
                case WizardStates.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
//            stateMachine.UpdateState(state);
        }

        protected override StateMachineTransition<WizardStates> Transition(Entity entity, WizardStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var events = entity.getComponent<EventComponent>();
            var state = stateMachine.State;
            var playerState = entity.getComponent<PlayerStateComponent>();
            if (state.StateEnum != WizardStates.Dead && playerState.IsKilled)
            {
                return StateMachineTransition<WizardStates>.Replace(WizardStates.Dead);
            }
            
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Push(WizardStates.Attack);
                    } else if (input.DefensePressed && state.ImmaterialCooldown < 0)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (input.DashPressed && state.GlideCooldown < 0)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Glide);
                    } else if (input.MovementStick.LengthSquared() > PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Walk);
                    }
                    break;
                case WizardStates.Walk:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Push(WizardStates.Attack);
                    } else if (input.DefensePressed && state.ImmaterialCooldown < 0)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (input.DashPressed && state.GlideCooldown < 0)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Glide);
                    } else if (input.MovementStick.LengthSquared() < PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }
                    break;
                case WizardStates.Attack:
                    playerState.SwingFinished = events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_SWING_END);
                    if (playerState.SwingFinished)
                    {
                        return StateMachineTransition<WizardStates>.Pop();
                    }
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_HIT_START))
                    {
                        playerState.HitActive = true;
                    }
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_HIT_END))
                    {
                        playerState.HitActive = false;
                    }
                    break;
                case WizardStates.Glide:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Push(WizardStates.Attack);
                    } else if (input.DefensePressed && state.ImmaterialCooldown < 0)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (!input.DashPressed || state.GlideTime >= WizardState.MAX_GLIDE_TIME)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }

                    break;
                case WizardStates.Immaterial:
                    if (state.ImmaterialTime >= WizardState.IMMATERIAL_TIME)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }

                    break;
                case WizardStates.Dead:
                    if (!playerState.IsKilled)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return StateMachineTransition<WizardStates>.None();
        }

        protected override void OnEnter(Entity entity, WizardStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;
            var input = entity.getComponent<PlayerInputComponent>();
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    break;
                case WizardStates.Attack:
                    playerState.SwingFinished = false;
                    break;
                case WizardStates.Glide:
                    state.GlideDirection = input.MovementStick;
                    break;
                case WizardStates.Dead:
                    break;
                case WizardStates.Immaterial:
                    playerState.BallHitBoost = WizardState.IMMATERIAL_HIT_BOOST;
                    playerState.HitActive = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnExit(Entity entity, WizardStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;

            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    break;
                case WizardStates.Attack:
                    playerState.SwingFinished = false;
                    break;
                case WizardStates.Glide:
                    state.GlideTime = 0;
                    state.GlideCooldown = WizardState.GLIDE_COOLDOWN;
                    break;
                case WizardStates.Dead:
                    break;
                case WizardStates.Immaterial:
                    playerState.HitActive = false;
                    playerState.BallHitBoost = 1.0f;
                    state.ImmaterialTime = 0;
                    state.ImmaterialCooldown = WizardState.IMMATERIAL_COOLDOWN;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}