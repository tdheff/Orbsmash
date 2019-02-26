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
    public class SpacemanStateMachineSystem : PushdownAutomatonSystem<SpacemanStates, SpacemanState, SpacemanStateMachineComponent>
    {
        public SpacemanStateMachineSystem() : base(new Matcher().all(
            typeof(PlayerStateComponent),
            typeof(SpacemanStateMachineComponent),
            typeof(PlayerInputComponent),
            typeof(EventComponent)
        )) { }

        protected override void Update(Entity entity, SpacemanStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var input = entity.getComponent<PlayerInputComponent>();
            var state = stateMachine.State;
            var events = entity.getComponent<EventComponent>();
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var steps = soundEffects.First(x => x.Name == KnightSoundEffectGroups.STEPS);
            playerState.BallHitVector = Player.CalculateHitVector(playerState.side, input.MovementStick);
            switch (state.StateEnum)
            {
                case SpacemanStates.Idle:
                    break;
                case SpacemanStates.Walk:
                    steps.Play();
                    break;
                case SpacemanStates.Attack:
                    break;
                case SpacemanStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_BOUNCE))
                    {
                        playerState.HasKOBounced = true;
                    }
                    var deadVelocity = entity.getComponent<VelocityComponent>();
                    var speed = playerState.HasKOBounced ? -50 : -300;
                    deadVelocity.Velocity = new Vector2(Player.SignForSide(playerState.side) * speed, 0);
                    break;
                case SpacemanStates.Eliminated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
//            stateMachine.UpdateState(state);
        }

        protected override StateMachineTransition<SpacemanStates> Transition(Entity entity, SpacemanStateMachineComponent stateMachine)
        {
            var input = entity.getComponent<PlayerInputComponent>();
            var events = entity.getComponent<EventComponent>();
            var playerState = entity.getComponent<PlayerStateComponent>();
            var spacemanState = stateMachine.State;
            if (spacemanState.StateEnum != SpacemanStates.KO && spacemanState.StateEnum != SpacemanStates.Eliminated && playerState.IsKilled)
            {
                return StateMachineTransition<SpacemanStates>.Replace(SpacemanStates.KO);
            }
            
            switch (spacemanState.StateEnum)
            {
                case SpacemanStates.Idle:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<SpacemanStates>.Push(SpacemanStates.Attack);
                    }  else if (input.MovementStick.LengthSquared() > PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<SpacemanStates>.Replace(SpacemanStates.Walk);
                    }
                    break;
                case SpacemanStates.Walk:
                    if (input.AttackPressed)
                    {
                        return StateMachineTransition<SpacemanStates>.Push(SpacemanStates.Attack);
                    } else if (input.MovementStick.LengthSquared() < PlayerStateComponent.MOVEMENT_THRESHOLD_SQUARED)
                    {
                        return StateMachineTransition<SpacemanStates>.Replace(SpacemanStates.Idle);
                    }
                    break;
                case SpacemanStates.Attack:
                    playerState.SwingFinished = events.ConsumeEventAndReturnIfPresent(PlayerEvents.PLAYER_SWING_END);
                    if (playerState.SwingFinished)
                    {
                        return StateMachineTransition<SpacemanStates>.Pop();
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
                case SpacemanStates.KO:
                    if (events.ConsumeEventAndReturnIfPresent(PlayerEvents.KO_END) || Time.time - spacemanState.KoTime > SpacemanState.KO_ANIM_PLACEHOLDER_LENGTH)
                    {
                        return StateMachineTransition<SpacemanStates>.Replace(SpacemanStates.Eliminated);
                    }
                    break;
                case SpacemanStates.Eliminated:
                    if (!playerState.IsKilled)
                    {
                        return StateMachineTransition<SpacemanStates>.Replace(SpacemanStates.Idle);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return StateMachineTransition<SpacemanStates>.None();
        }

        protected override void OnEnter(Entity entity, SpacemanStateMachineComponent stateMachine)
        {
            var handyScene = (Handy.Scene)scene;
            var playerState = entity.getComponent<PlayerStateComponent>();
            var soundEffects = entity.getComponents<SoundEffectGroupComponent>();
            var swipes = soundEffects.First(x => x.Name == KnightSoundEffectGroups.SWIPES);
            var state = stateMachine.State;
            switch (state.StateEnum)
            {
                case SpacemanStates.Idle:
                    break;
                case SpacemanStates.Walk:
                    break;
                case SpacemanStates.Attack:
                    swipes.Play();
                    playerState.SwingFinished = false;
                    playerState.BallHitBoost = 2.5f;
                    break;
                case SpacemanStates.KO:
                    var gameState = handyScene.findEntity("Game");
                    var hitStop = gameState.getComponent<HitStopComponent>();
                    hitStop.Freeze(0.3f);
                    state.KoTime = Time.time;
                    playerState.HasKOBounced = false;
                    break;
                case SpacemanStates.Eliminated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnExit(Entity entity, SpacemanStateMachineComponent stateMachine)
        {
            var playerState = entity.getComponent<PlayerStateComponent>();
            var state = stateMachine.State;

            switch (state.StateEnum)
            {
                case SpacemanStates.Idle:
                    break;
                case SpacemanStates.Walk:
                    break;
                case SpacemanStates.Attack:
                    playerState.SwingFinished = false;
                    break;
                case SpacemanStates.KO:
                    break;
                case SpacemanStates.Eliminated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}