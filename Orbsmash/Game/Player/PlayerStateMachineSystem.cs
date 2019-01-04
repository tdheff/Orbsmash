using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class PlayerStateMachineSystem : PushdownAutomatonSystem<PlayerStates, PlayerState, PlayerStateMachineComponent>
    {
        public PlayerStateMachineSystem() : base(new Matcher().all(
            typeof(PlayerStateMachineComponent),
            typeof(PlayerInputComponent),
            typeof(EventComponent)
        )) { }

        protected override void Update(Entity entity, PlayerStateMachineComponent stateMachine)
        {
            var state = stateMachine.State;
            switch (state.StateEnum)
            {
                case PlayerStates.Idle:
                    break;
                case PlayerStates.Walk:
                    break;
                case PlayerStates.Dash:
                    break;
                case PlayerStates.Charge:
                    state.ChargeTime += Time.deltaTime;
                    if (state.ChargeTime >= PlayerState.MaxChargeTime)
                    {
                        state.ChargeFinished = true;
                    }
                    break;
                case PlayerStates.Swing:
                    break;
                case PlayerStates.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
//            stateMachine.UpdateState(state);
        }

        protected override StateMachineTransition<PlayerStates> Transition(Entity entity, PlayerStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var events = entity.getComponent<EventComponent>();
            var state = stateMachine.State;
            if (events.PeekEvents().Count > 0)
            {
                var eventsList = events.PeekEvents();
            }
            if (state.StateEnum != PlayerStates.Dead && state.IsKilled)
            {
                return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Dead);
            }
            
            switch (state.StateEnum)
            {
                case PlayerStates.Idle:
                    if (input.SwingPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Charge);
                    } else if (input.DashPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Dash);
                    } else if (input.MovementStick.LengthSquared() > PlayerState.MovementThresholdSquared)
                    {
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Walk);
                    }
                    break;
                case PlayerStates.Walk:
                    if (input.SwingPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Charge);
                    } else if (input.DashPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Dash);
                    } else if (input.MovementStick.LengthSquared() < PlayerState.MovementThresholdSquared)
                    {
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Idle);
                    }
                    break;
                case PlayerStates.Dash:
                    if (input.SwingPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Charge);
                    } else if (state.DashFinished)
                    {
                        return StateMachineTransition<PlayerStates>.Pop();
                    }
                    break;
                case PlayerStates.Charge:
                    if (!input.SwingPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Swing);
                    }
                    break;
                case PlayerStates.Swing:
                    state.SwingFinished = events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_SWING_END);
                    if (state.SwingFinished)
                    {
                        return StateMachineTransition<PlayerStates>.Pop();
                    }
                    break;
                case PlayerStates.Dead:
                    if (!state.IsKilled)
                    {
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Idle);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return StateMachineTransition<PlayerStates>.None();
        }

        protected override void OnEnter(Entity entity, PlayerStateMachineComponent stateMachine)
        {
            var state = stateMachine.State;
            switch (state.StateEnum)
            {
                case PlayerStates.Idle:
                    break;
                case PlayerStates.Walk:
                    break;
                case PlayerStates.Dash:
                    state.DashFinished = false;
                    break;
                case PlayerStates.Charge:
                    state.ChargeFinished = false;
                    break;
                case PlayerStates.Swing:

                    state.SwingFinished = false;
                    break;
                case PlayerStates.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnExit(Entity entity, PlayerStateMachineComponent stateMachine)
        {
            var state = stateMachine.State;

            switch (state.StateEnum)
            {
                case PlayerStates.Idle:
                    break;
                case PlayerStates.Walk:
                    break;
                case PlayerStates.Dash:
                    state.DashFinished = false;
                    break;
                case PlayerStates.Charge:
                    state.ChargeTime = 0;
                    state.ChargeFinished = false;
                    break;
                case PlayerStates.Swing:
                    state.SwingFinished = false;
                    break;
                case PlayerStates.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}