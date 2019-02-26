using System;
using System.Linq;
using Handy.Components;
using Handy.Systems;
using System.Collections.Generic;
using Nez;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public class WizardAnimationSystem : EntitySystem
    {
        public WizardAnimationSystem(Matcher matcher) : base(matcher) { }
        public WizardAnimationSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(WizardStateMachineComponent), typeof(PlayerStateComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var playerState = entity.getComponent<PlayerStateComponent>();
                var wizardState = entity.getComponent<WizardStateMachineComponent>().State;
                var player = (Player)entity;
                var mainBodyAnimation = player.getComponent<AnimationComponent>();
                var eventComponent = player.getComponent<EventComponent>();
                mainBodyAnimation.Paused = false;

                switch (wizardState.StateEnum)
                {
                    case WizardStates.Idle:
                        if(playerState.LastDirection == Gameplay.Direction.UP || playerState.LastDirection == Gameplay.Direction.DOWN)
                        {
                            mainBodyAnimation.SetAnimation(WizardAnimations.IDLE_VERTICAL);
                        }
                        else
                        {
                            mainBodyAnimation.SetAnimation(WizardAnimations.IDLE_HORIZONTAL);
                        }
                        break;
                    case WizardStates.Walk:
                        mainBodyAnimation.SetAnimation($"WALK_{playerState.LastDirection}");
                        break;
                    case WizardStates.Attack:
                        mainBodyAnimation.SetAnimation(WizardAnimations.ATTACK);
                        break;
                    case WizardStates.PreGlide:
                    case WizardStates.Glide:
                        mainBodyAnimation.SetAnimation(WizardAnimations.GLIDE);
                        break;
                    case WizardStates.Charge:
                        if (mainBodyAnimation.CurrentAnimation != WizardAnimations.CHARGE_IDLE &&
                            mainBodyAnimation.CurrentAnimation != WizardAnimations.CHARGE_FULL)
                        {
                            mainBodyAnimation.SetAnimation(WizardAnimations.CHARGE_IDLE);
                        }

                        if (playerState.ChargeFinished)
                        {
                            var frame = mainBodyAnimation.CurrentAnimationFrame;
                            mainBodyAnimation.SetAnimation(WizardAnimations.CHARGE_FULL);
                            mainBodyAnimation.CurrentAnimationFrame = frame;
                        }
                        break;
                    case WizardStates.ChargeHeavy:
                        if (mainBodyAnimation.CurrentAnimation != WizardAnimations.CHARGE_HEAVY_IDLE &&
                            mainBodyAnimation.CurrentAnimation != WizardAnimations.CHARGE_HEAVY_FULL)
                        {
                            mainBodyAnimation.SetAnimation(WizardAnimations.CHARGE_HEAVY_IDLE);
                        }

                        if (playerState.ChargeFinished)
                        {
                            var frame = mainBodyAnimation.CurrentAnimationFrame;
                            mainBodyAnimation.SetAnimation(WizardAnimations.CHARGE_HEAVY_FULL);
                            mainBodyAnimation.CurrentAnimationFrame = frame;
                        }
                        break;
                    case WizardStates.AttackHeavy:
                        mainBodyAnimation.SetAnimation(WizardAnimations.ATTACK_HEAVY);
                        break;
                    case WizardStates.KO:
                        mainBodyAnimation.SetAnimation(WizardAnimations.KO);
                        break;
                    case WizardStates.Eliminated:
                        mainBodyAnimation.Paused = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }

        }

    }
}