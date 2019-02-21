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
    public class SpacemanAnimationSystem : EntitySystem
    {
        public SpacemanAnimationSystem(Matcher matcher) : base(matcher) { }
        public SpacemanAnimationSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(SpacemanStateMachineComponent), typeof(PlayerStateComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var knightState = entity.getComponent<SpacemanStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();
                var player = (Player)entity;
                var mainBodyAnimation = player.getComponent<AnimationComponent>();
                mainBodyAnimation.Paused = false; // that way it's "sticky" on false so the anims don't get stuck
                var eventComponent = player.getComponent<EventComponent>();

                switch (knightState.StateEnum)
                {
                    case SpacemanStates.Idle:
                        if(playerState.LastDirection == Gameplay.Direction.UP || playerState.LastDirection == Gameplay.Direction.DOWN)
                        {
                            mainBodyAnimation.SetAnimation(SpacemanAnimations.IDLE_VERTICAL);
                        }
                        else
                        {
                            mainBodyAnimation.SetAnimation(SpacemanAnimations.IDLE_HORIZONTAL);
                        }
                        break;
                    case SpacemanStates.Walk:
                        mainBodyAnimation.SetAnimation($"WALK_{playerState.LastDirection}");
                        break;
                    case SpacemanStates.Attack:
                        mainBodyAnimation.SetAnimation(SpacemanAnimations.ATTACK);
                        break;
                    case SpacemanStates.KO:
                        mainBodyAnimation.SetAnimation(SpacemanAnimations.IDLE_HORIZONTAL);
                        break;
                    case SpacemanStates.Eliminated:
                        mainBodyAnimation.Paused = true;
                        break;
                    case SpacemanStates.Shield:
                        mainBodyAnimation.SetAnimation(SpacemanAnimations.SHIELD);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }

        }

    }
}