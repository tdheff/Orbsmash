using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using Microsoft.Xna.Framework;
using Nez;
using Orbsmash.Constants;

namespace Orbsmash.Player
{
    public class KnightStateMachineSystem : PushdownAutomatonSystem<KnightStates, KnightState, KnightStateMachineComponent>
    {
        public KnightStateMachineSystem() : base(new Matcher().all(
            typeof(PlayerStateComponent),
            typeof(KnightStateMachineComponent),
            typeof(PlayerInputComponent),
            typeof(EventComponent)
        )) { }

        protected override void Update(Entity entity, KnightStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var input = entity.getComponent<PlayerInputComponent>();
            var state = stateMachine.State;
            playerState.BallHitBoost = 1.0f;
            playerState.BallHitVector = Player.calculateHitVector(playerState.side, input.MovementStick);
            switch (state.StateEnum)
            {
                case KnightStates.Idle:
                    break;
                case KnightStates.Walk:
                    break;
                case KnightStates.Dash:
                    break;
                case KnightStates.Charge:
                    playerState.ChargeTime += Time.deltaTime;
                    if (playerState.ChargeTime >= PlayerStateComponent.MAX_CHARGE_TIME)
                    {
                        playerState.ChargeFinished = true;
                    }

                    playerState.BallHitBoost = 1.0f + playerState.ChargeTime / PlayerStateComponent.MAX_CHARGE_TIME;
                    break;
                case KnightStates.Swing:
                    break;
                case KnightStates.Dead:
                    break;
                case KnightStates.Block:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
//            stateMachine.UpdateState(state);
        }

        protected override StateMachineTransition<KnightStates> Transition(Entity entity, KnightStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var events = entity.getComponent<EventComponent>();
            var playerState = entity.getComponent<PlayerStateComponent>();
            var knightState = stateMachine.State;
            if (knightState.StateEnum != KnightStates.Dead && playerState.IsKilled)
            {
                return StateMachineTransition<KnightStates>.Replace(KnightStates.Dead);
            }
            
            switch (knightState.StateEnum)
            {
                case KnightStates.Idle:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Charge);
                    } else if (input.DefensePressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Block);
                    } else if (input.DashPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Dash);
                    } else if (input.MovementStick.LengthSquared() > PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Walk);
                    }
                    break;
                case KnightStates.Walk:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Charge);
                    } else if (input.DefensePressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Block);
                    } else if (input.DashPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Dash);
                    } else if (input.MovementStick.LengthSquared() < PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Idle);
                    }
                    break;
                case KnightStates.Dash:
                    if (!input.DashPressed)
                    {
                        return StateMachineTransition<KnightStates>.Pop();
                    } else if (input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Charge);
                    } else if (input.DefensePressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Block);
                    } else if (playerState.DashFinished)
                    {
                        return StateMachineTransition<KnightStates>.Pop();
                    }
                    break;
                case KnightStates.Charge:
                    if (!input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Swing);
                    }
                    break;
                case KnightStates.Swing:
                    playerState.SwingFinished = events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_SWING_END);
                    if (playerState.SwingFinished)
                    {
                        return StateMachineTransition<KnightStates>.Pop();
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
                case KnightStates.Dead:
                    if (!playerState.IsKilled)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Idle);
                    }
                    break;
                case KnightStates.Block:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.BLOCK_END))
                    {
                        return StateMachineTransition<KnightStates>.Pop();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return StateMachineTransition<KnightStates>.None();
        }

        protected override void OnEnter(Entity entity, KnightStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;
            switch (state.StateEnum)
            {
                case KnightStates.Idle:
                    break;
                case KnightStates.Walk:
                    break;
                case KnightStates.Dash:
                    playerState.DashFinished = false;
                    break;
                case KnightStates.Charge:
                    playerState.ChargeFinished = false;
                    break;
                case KnightStates.Swing:

                    playerState.SwingFinished = false;
                    break;
                case KnightStates.Dead:
                    break;
                case KnightStates.Block:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnExit(Entity entity, KnightStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;

            switch (state.StateEnum)
            {
                case KnightStates.Idle:
                    break;
                case KnightStates.Walk:
                    break;
                case KnightStates.Dash:
                    playerState.DashFinished = false;
                    break;
                case KnightStates.Charge:
                    playerState.ChargeTime = 0;
                    playerState.ChargeFinished = false;
                    break;
                case KnightStates.Swing:
                    playerState.SwingFinished = false;
                    break;
                case KnightStates.Dead:
                    break;
                case KnightStates.Block:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}