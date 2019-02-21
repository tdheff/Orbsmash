using System;
using System.Linq;
using Handy.Components;
using Handy.Sound;
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
            var events = entity.getComponent<EventComponent>();
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var steps = soundEffects.First(x => x.Name == KnightSoundEffectGroups.STEPS);
            playerState.BallHitVector = Player.CalculateHitVector(playerState.side, input.MovementStick);
            if (state.StateEnum != KnightStates.Dash)
            {
                state.SprintRemaining = Math.Min(state.SprintRemaining + Time.deltaTime * KnightState.SPRINT_RECOVERY_MULTIPLIER, KnightState.MAX_SPRINT);
            }

            state.BlockCooldown = Math.Min(state.BlockCooldown + Time.deltaTime, KnightState.BLOCK_COOLDOWN);
            switch (state.StateEnum)
            {
                case KnightStates.Idle:
                    break;
                case KnightStates.Walk:
                    steps.Play();
                    break;
                case KnightStates.Dash:
                    state.SprintRemaining = Math.Max(state.SprintRemaining - Time.deltaTime, 0);
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
                case KnightStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_BOUNCE))
                    {
                        playerState.HasKOBounced = true;
                    }
                    var deadVelocity = entity.getComponent<VelocityComponent>();
                    var speed = playerState.HasKOBounced ? -50 : -300;
                    deadVelocity.Velocity = new Vector2(Player.SignForSide(playerState.side) * speed, 0);
                    break;
                case KnightStates.Block:
                    playerState.BallHitVector = new Vector2(1, 0);
                    playerState.BallHitBoost = 1.0f;
                    break;
                case KnightStates.BlockHit:
                    var blockVelocity = entity.getComponent<VelocityComponent>();
                    blockVelocity.Velocity = state.BlockHitVector * (state.BlockHitTimeRemaining / 0.3f);
                    state.BlockHitTimeRemaining -= Time.deltaTime;
                    break;
                case KnightStates.Eliminated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rightCooldown = entity.getComponent<SpritesheetComponent>(ComponentNames.RIGHT_COOLDOWN);
            var leftCooldown = entity.getComponent<SpritesheetComponent>(ComponentNames.LEFT_COOLDOWN);

            var cooldownFrames = rightCooldown.Subtextures.Length;
            
            var sprintFraction = state.SprintRemaining / KnightState.MAX_SPRINT;
            var rightFrame = (int)Mathf.round((cooldownFrames - 1) * sprintFraction);
            rightCooldown.Frame = cooldownFrames - rightFrame - 1;
            
            var blockFraction = state.BlockCooldown / KnightState.BLOCK_COOLDOWN;
            var leftFrame = (int)Mathf.round((cooldownFrames - 1) * blockFraction);
            leftCooldown.Frame = cooldownFrames - leftFrame - 1;
        }

        protected override StateMachineTransition<KnightStates> Transition(Entity entity, KnightStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var events = entity.getComponent<EventComponent>();
            var playerState = entity.getComponent<PlayerStateComponent>();
            var knightState = stateMachine.State;
            if (knightState.StateEnum != KnightStates.KO && knightState.StateEnum != KnightStates.Eliminated && playerState.IsKilled)
            {
                return StateMachineTransition<KnightStates>.Replace(KnightStates.KO);
            }
            
            switch (knightState.StateEnum)
            {
                case KnightStates.Idle:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Charge);
                    } else if (input.DefensePressed && knightState.BlockCooldown >= KnightState.BLOCK_COOLDOWN)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Block);
                    } else if (input.DashPressed && knightState.SprintRemaining >= KnightState.MIN_START_SPRINT)
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
                    } else if (input.DefensePressed && knightState.BlockCooldown >= KnightState.BLOCK_COOLDOWN)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Block);
                    } else if (input.DashPressed && input.DashPressed && knightState.SprintRemaining >= KnightState.MIN_START_SPRINT)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Dash);
                    } else if (input.MovementStick.LengthSquared() < PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Idle);
                    }
                    break;
                case KnightStates.Dash:
                    if (!input.DashPressed || knightState.SprintRemaining <= 0.01)
                    {
                        return StateMachineTransition<KnightStates>.Pop();
                    } else if (input.AttackPressed)
                    {
                        return StateMachineTransition<KnightStates>.Push(KnightStates.Charge);
                    } else if (input.DefensePressed && knightState.BlockCooldown >= KnightState.BLOCK_COOLDOWN)
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
                case KnightStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_END))
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Eliminated);
                    }
                    break;
                case KnightStates.Eliminated:
                    if (!playerState.IsKilled)
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.Idle);
                    }
                    break;
                case KnightStates.Block:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.BLOCK_HIT))
                    {
                        return StateMachineTransition<KnightStates>.Replace(KnightStates.BlockHit);
                    }
                    else if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.BLOCK_END))
                    {
                        return StateMachineTransition<KnightStates>.Pop();
                    }
                    break;
                case KnightStates.BlockHit:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.BLOCK_HIT_END))
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
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var swipes = soundEffects.First(x => x.Name == KnightSoundEffectGroups.SWIPES);
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
                    swipes.Play();
                    playerState.SwingFinished = false;
                    break;
                case KnightStates.KO:
                    playerState.HasKOBounced = false;
                    break;
                case KnightStates.Eliminated:
                    break;
                case KnightStates.Block:
                    playerState.HitActive = true;
                    state.BlockCooldown = 0;
                    break;
                case KnightStates.BlockHit:
                    state.BlockHitTimeRemaining = 0.3f;
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
                case KnightStates.KO:
                    break;
                case KnightStates.Eliminated:
                    break;
                case KnightStates.Block:
                    playerState.HitActive = false;
                    break;
                case KnightStates.BlockHit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}