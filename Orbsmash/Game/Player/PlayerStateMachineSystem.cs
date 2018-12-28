using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using Nez;

namespace Orbsmash.Player
{
    public class PlayerStateMachineSystem : PushdownAutomatonSystem<PlayerStates, PlayerState, PlayerStateMachineComponent>
    {
        public PlayerStateMachineSystem() : base(new Matcher().all(
            typeof(PlayerStateMachineComponent),
            typeof(PlayerInputComponent)
        )) { }
        
        protected override StateMachineTransition<PlayerStates> Transition(Entity entity, PlayerStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var state = stateMachine.State;
            
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
                        return StateMachineTransition<PlayerStates>.Push(PlayerStates.Swing);
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
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Swing);
                    } else if (state.DashFinished)
                    {
                        return StateMachineTransition<PlayerStates>.Pop();
                    }
                    break;
                case PlayerStates.Charge:
                    if (state.ChargeFinished || !input.SwingPressed)
                    {
                        return StateMachineTransition<PlayerStates>.Replace(PlayerStates.Swing);
                    }
                    break;
                case PlayerStates.Swing:
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
            var player = (Player)entity;

            var playerAnimComponents = player.getComponents<AnimationComponent<Constants.EAnimations>>();

            var mainBodyAnimation = playerAnimComponents.Where(a => a.Context == Constants.AnimationContexts.PlayerSpriteAnimations.ToString()).First();

            switch (state.StateEnum)
            {
                case PlayerStates.Idle:
                    mainBodyAnimation.SetAnimation(Constants.EAnimations.PlayerIdle);
                    break;
                case PlayerStates.Walk:
                    mainBodyAnimation.SetAnimation(Constants.EAnimations.PlayerWalk);
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