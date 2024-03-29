﻿using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using System.Collections.Generic;
using Nez;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public class KnightAnimationSystem : EntitySystem
    {
        public KnightAnimationSystem(Matcher matcher) : base(matcher) { }
        public KnightAnimationSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(KnightStateMachineComponent), typeof(PlayerStateComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var knightState = entity.getComponent<KnightStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();
                var player = (Player)entity;
                var mainBodyAnimation = player.getComponent<AnimationComponent>();
                mainBodyAnimation.Paused = false; // that way it's "sticky" on false so the anims don't get stuck
                var eventComponent = player.getComponent<EventComponent>();

                switch (knightState.StateEnum)
                {
                    case KnightStates.Idle:
                        if(playerState.LastDirection == Gameplay.Direction.UP || playerState.LastDirection == Gameplay.Direction.DOWN)
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.IDLE_VERTICAL);
                        }
                        else
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.IDLE_HORIZONTAL);
                        }
                        break;
                    case KnightStates.Walk:
                        mainBodyAnimation.SetAnimation($"WALK_{playerState.LastDirection}");
                        break;
                    case KnightStates.Dash:
                        mainBodyAnimation.SetAnimation($"WALK_{playerState.LastDirection}", 1.2f);
                        break;
                    case KnightStates.Charge:
                        if (mainBodyAnimation.CurrentAnimation != KnightAnimations.CHARGE_IDLE &&
                            mainBodyAnimation.CurrentAnimation != KnightAnimations.CHARGE_FULL)
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE);
                        }

                        if (eventComponent.ConsumeEventAndReturnIfPresent(PlayerEvents.CHARGE_WINDUP_END))
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE_IDLE);
                        }

                        if (playerState.ChargeFinished)
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE_FULL);
                        }
                        break;
                    case KnightStates.Swing:
                        mainBodyAnimation.SetAnimation(KnightAnimations.ATTACK);
                        break;
                    case KnightStates.ChargeHeavy:
                        if (mainBodyAnimation.CurrentAnimation != KnightAnimations.CHARGE_HEAVY_IDLE &&
                            mainBodyAnimation.CurrentAnimation != KnightAnimations.CHARGE_HEAVY_FULL)
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE_HEAVY);
                        }

                        if (eventComponent.ConsumeEventAndReturnIfPresent(PlayerEvents.CHARGE_WINDUP_END))
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE_HEAVY_IDLE);
                        }

                        if (playerState.ChargeFinished)
                        {
                            mainBodyAnimation.SetAnimation(KnightAnimations.CHARGE_HEAVY_FULL);
                        }
                        break;
                    case KnightStates.SwingHeavy:
                        mainBodyAnimation.SetAnimation(KnightAnimations.ATTACK_HEAVY);
                        break;
                    case KnightStates.KO:
                        mainBodyAnimation.SetAnimation(KnightAnimations.KO);
                        break;
                    case KnightStates.Eliminated:
                        mainBodyAnimation.Paused = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }

        }

    }
}