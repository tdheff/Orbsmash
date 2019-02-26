using System;
using System.Linq;
using Handy.Components;
using Handy.Sound;
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
            var events = entity.getComponent<EventComponent>();
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var steps = soundEffects.First(x => x.Name == WizardSoundEffectGroups.STEPS);
            state.GlideCooldown = Math.Min(state.GlideCooldown + Time.deltaTime, WizardState.GLIDE_COOLDOWN);
            state.ImmaterialCooldown = Math.Min(state.ImmaterialCooldown+ Time.deltaTime, WizardState.IMMATERIAL_COOLDOWN);
            playerState.BallHitVector = Player.CalculateHitVector(playerState.side, input.MovementStick);
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    steps.Play();
                    break;
                case WizardStates.Attack:
                    break;
                case WizardStates.PreGlide:
                case WizardStates.Glide:
                    state.GlideTime += Time.deltaTime;
                    break;
                case WizardStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_BOUNCE))
                    {
                        playerState.HasKOBounced = true;
                    }
                    var deadVelocity = entity.getComponent<VelocityComponent>();
                    var speed = playerState.HasKOBounced ? playerState.KnockoutVector / 2 : playerState.KnockoutVector;
                    deadVelocity.Velocity = speed;
                    break;
                case WizardStates.Charge:
                    playerState.ChargeTime += Time.deltaTime;
                    if (playerState.ChargeTime >= PlayerStateComponent.MAX_CHARGE_TIME)
                    {
                        playerState.ChargeFinished = true;
                    }

                    playerState.BallHitBoost = 1.0f + playerState.ChargeTime / PlayerStateComponent.MAX_CHARGE_TIME;
                    break;
                case WizardStates.ChargeHeavy:
                    playerState.ChargeTime += Time.deltaTime;
                    if (playerState.ChargeTime >= PlayerStateComponent.MAX_HEAVY_CHARGE_TIME)
                    {
                        playerState.ChargeFinished = true;
                    }

                    playerState.BallHitBoost = 2.0f + playerState.ChargeTime / PlayerStateComponent.MAX_HEAVY_CHARGE_TIME;
                    break;
                case WizardStates.AttackHeavy:
                    break;
                case WizardStates.Eliminated:
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
            var wizardState = stateMachine.State;
            var playerState = entity.getComponent<PlayerStateComponent>();
            if (wizardState.StateEnum != WizardStates.KO && wizardState.StateEnum != WizardStates.Eliminated && playerState.IsKilled)
            {
                return StateMachineTransition<WizardStates>.Replace(WizardStates.KO);
            }
            
            switch (wizardState.StateEnum)
            {
                case WizardStates.Idle:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Charge);
                    } else if (input.HeavyAttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.ChargeHeavy);
                    } else if (input.DashPressed && wizardState.GlideCooldown >= WizardState.GLIDE_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.PreGlide);
                    } else if (input.MovementStick.LengthSquared() > PlayerStateComponent.DEAD_ZONE)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Walk);
                    }
                    break;
                case WizardStates.Walk:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Charge);
                    } else if (input.HeavyAttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.ChargeHeavy);
                    } else if (input.DashPressed && wizardState.GlideCooldown >= WizardState.GLIDE_COOLDOWN)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.PreGlide);
                    } else if (input.MovementStick.LengthSquared() < PlayerStateComponent.DEAD_ZONE)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }
                    break;
                case WizardStates.Charge:
                    if (!input.AttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Attack);
                    }
                    break;
                case WizardStates.ChargeHeavy:
                    if (!input.HeavyAttackPressed)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.AttackHeavy);
                    }
                    break;
                case WizardStates.AttackHeavy:
                case WizardStates.Attack:
                    playerState.SwingFinished = events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_SWING_END);
                    if (playerState.SwingFinished)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
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
                    if(wizardState.GlideTime>= WizardState.GLIDE_DELAY)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Glide);
                    }
                    break;
                case WizardStates.Glide:
                    if (input.AttackPressed)
                    {
                        wizardState.GlideTime = 10000; // kill the glide so it pops back to glide -> instantly to ilde
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Charge);
                    } else if (input.HeavyAttackPressed)
                    {
                        wizardState.LastGlideTime = wizardState.GlideTime;
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.ChargeHeavy);
                    } else if (wizardState.GlideTime >= WizardState.MAX_GLIDE_TIME)
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Idle);
                    }

                    break;
                case WizardStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_END))
                    {
                        return StateMachineTransition<WizardStates>.Replace(WizardStates.Eliminated);
                    }
                    break;
                case WizardStates.Eliminated:
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
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var swipes = soundEffects.First(x => x.Name == WizardSoundEffectGroups.SWIPES);
            switch (state.StateEnum)
            {
                case WizardStates.Idle:
                    break;
                case WizardStates.Walk:
                    break;
                case WizardStates.Attack:
                    swipes.Play();
                    playerState.SwingFinished = false;
                    break;
                case WizardStates.PreGlide:
                    break;
                case WizardStates.Glide:
                    state.GlideCooldown = 0;
                    state.GlideDirection = input.MovementStick;
                    break;
                case WizardStates.KO:
                    playerState.IsInvulnerable = true;
                    playerState.HasKOBounced = false;
                    var gameState = handyScene.findEntity("Game");
                    var hitStop = gameState.getComponent<HitStopComponent>();
                    hitStop.Freeze(0.3f);
                    break;
                case WizardStates.Eliminated:
                    break;
                case WizardStates.Charge:
                    break;
                case WizardStates.ChargeHeavy:
                    break;
                case WizardStates.AttackHeavy:
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
                    
                    playerState.BallHitBoost = 1.0f;
                    break;
                case WizardStates.PreGlide:
                    break;
                case WizardStates.Glide:
                    state.GlideTime = 0;
                    break;
                case WizardStates.KO:
                    break;
                case WizardStates.Eliminated:
                    playerState.IsInvulnerable = false;
                    break;
                case WizardStates.Charge:
                    playerState.ChargeTime = 0;
                    playerState.ChargeFinished = false;
                    break;
                case WizardStates.ChargeHeavy:
                    playerState.ChargeTime = 0;
                    playerState.ChargeFinished = false;
                    break;
                case WizardStates.AttackHeavy:
                    playerState.BallHitBoost = 1.0f;
                    playerState.SwingFinished = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}