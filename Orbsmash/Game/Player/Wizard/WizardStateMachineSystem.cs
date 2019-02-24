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
            state.GlideCooldown = Math.Min(state.GlideCooldown + Time.deltaTime, WizardState.GLIDE_COOLDOWN);
            state.ImmaterialCooldown = Math.Min(state.ImmaterialCooldown+ Time.deltaTime, WizardState.IMMATERIAL_COOLDOWN);
            playerState.BallHitBoost = 1.0f;
            playerState.BallHitVector = Player.CalculateHitVector(playerState.side, input.MovementStick);
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    break;
                case WizardStates.Attack:
                    break;
                case WizardStates.PreGlide:
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
            
            var rightCooldown = entity.getComponent<SpritesheetComponent>(ComponentNames.RIGHT_COOLDOWN);
            var leftCooldown = entity.getComponent<SpritesheetComponent>(ComponentNames.LEFT_COOLDOWN);

            var cooldownFrames = rightCooldown.Subtextures.Length;
            
            var sprintFraction = state.GlideCooldown / WizardState.GLIDE_COOLDOWN;
            var rightFrame = (int)Mathf.round((cooldownFrames - 1) * sprintFraction);
            rightCooldown.Frame = cooldownFrames - rightFrame - 1;
            
            var blockFraction = state.ImmaterialCooldown / WizardState.IMMATERIAL_COOLDOWN;
            var leftFrame = (int)Mathf.round((cooldownFrames - 1) * blockFraction);
            leftCooldown.Frame = cooldownFrames - leftFrame - 1;
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
                    } else if (input.DefensePressed && state.ImmaterialCooldown >= WizardState.IMMATERIAL_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (input.DashPressed && state.GlideCooldown >= WizardState.GLIDE_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.PreGlide);
                    } else if (input.MovementStick.LengthSquared() > PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Walk);
                    }
                    break;
                case WizardStates.Walk:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Push(WizardStates.Attack);
                    } else if (input.DefensePressed && state.ImmaterialCooldown >= WizardState.IMMATERIAL_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (input.DashPressed && state.GlideCooldown >= WizardState.GLIDE_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.PreGlide);
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
                case WizardStates.PreGlide:
                    if(state.GlideTime>= WizardState.GLIDE_DELAY)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Glide);
                    }
                    break;
                case WizardStates.Glide:
                    if (input.AttackPressed)
                    {
                        state.GlideTime = 10000; // kill the glide so it pops back to glide -> instantly to ilde
                        return StateMachineTransition<WizardStates>.Push(WizardStates.Attack);
                    } else if (input.DefensePressed && state.ImmaterialCooldown >= WizardState.IMMATERIAL_COOLDOWN)
                    {
                        state.LastGlideTime = state.GlideTime;
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Immaterial);
                    } else if (state.GlideTime >= WizardState.MAX_GLIDE_TIME)
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
            var handyScene = (Handy.Scene)scene;
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
                case WizardStates.PreGlide:
                    break;
                case WizardStates.Glide:
                    state.GlideCooldown = 0;
                    state.GlideDirection = input.MovementStick;
                    break;
                case WizardStates.Dead:
                    var gameState = handyScene.findEntity("Game");
                    var hitStop = gameState.getComponent<HitStopComponent>();
                    hitStop.Freeze(0.3f);
                    break;
                case WizardStates.Immaterial:
                    state.ImmaterialCooldown = 0;
                    playerState.BallHitBoost = WizardState.IMMATERIAL_MAX_HIT_BOOST + WizardState.IMMATERIAL_BOOST_RANGE * (state.LastGlideTime / WizardState.MAX_GLIDE_TIME);
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
                case WizardStates.PreGlide:
                    break;
                case WizardStates.Glide:
                    state.GlideTime = 0;
                    break;
                case WizardStates.Dead:
                    break;
                case WizardStates.Immaterial:
                    playerState.HitActive = false;
                    playerState.BallHitBoost = 1.0f;
                    state.ImmaterialTime = 0;
                    state.LastGlideTime = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}