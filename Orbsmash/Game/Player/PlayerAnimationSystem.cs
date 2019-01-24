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
    public class PlayerAnimationSystem : EntitySystem
    {
        public PlayerAnimationSystem(Matcher matcher) : base(matcher) { }
        public PlayerAnimationSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(PlayerStateMachineComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var state = entity.getComponent<PlayerStateMachineComponent>().State;
                var player = (Player)entity;
                var playerAnimComponents = player.getComponents<AnimationComponent>();
                var mainBodyAnimation = playerAnimComponents.Where(a => a.Context == AnimationContexts.PLAYER_SPRITE_ANIMATIONS).First();

                switch (state.StateEnum)
                {
                    case PlayerStates.Idle:
                        if(state.LastDirection == Gameplay.Direction.UP || state.LastDirection == Gameplay.Direction.DOWN)
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.IDLE_VERTICAL);
                        }
                        else
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.IDLE_HORIZONTAL);
                        }
                        break;
                    case PlayerStates.Walk:
                        mainBodyAnimation.SetAnimation($"WALK_{state.LastDirection}");
                        break;
                    case PlayerStates.Dash:
                        if(state.LastDirection == Gameplay.Direction.UP || state.LastDirection == Gameplay.Direction.DOWN)
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.IDLE_VERTICAL);
                        }
                        else
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.IDLE_HORIZONTAL);
                        }
                        break;
                    case PlayerStates.Charge:
                        if (mainBodyAnimation.CurrentAnimation != PlayerAnimations.CHARGE_PULSE &&
                            mainBodyAnimation.CurrentAnimation != PlayerAnimations.CHARGE_IDLE)
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.CHARGE);
                        }

                        if (state.ChargeFinished)
                        {
                            mainBodyAnimation.SetAnimation(PlayerAnimations.CHARGE_PULSE);
                        }
                        break;
                    case PlayerStates.Swing:
                        mainBodyAnimation.SetAnimation(PlayerAnimations.BLOCK_HIT);
                        break;
                    case PlayerStates.Dead:
                       
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }

        }

    }
}