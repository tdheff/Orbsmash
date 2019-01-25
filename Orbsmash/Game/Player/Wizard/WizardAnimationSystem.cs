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
                    case WizardStates.Dead:
                       
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }

        }

    }
}