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
    public class KnightSoundSystem : EntitySystem
    {
        public KnightSoundSystem(Matcher matcher) : base(matcher) { }
        public KnightSoundSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(KnightStateMachineComponent), typeof(PlayerStateComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var knightState = entity.getComponent<KnightStateMachineComponent>().State;
                var playerState = entity.getComponent<PlayerStateComponent>();
                var player = (Player)entity;
                var handyScene = (Handy.Scene) scene;
                var eventComponent = player.getComponent<EventComponent>();

                // get all the sound effects we care about
                var steps = handyScene.Sounds[SoundEffects.FOOTSTEPS_1];

                switch (knightState.StateEnum)
                {
                    case KnightStates.Idle:
                        steps.Stop();
                        break;
                    case KnightStates.Walk:
                        steps.IsLooped = true;
                        steps.Play();
                        break;
                    case KnightStates.Dash:
                        break;
                    case KnightStates.Charge:
                        break;
                    case KnightStates.Swing:
                        break;
                    case KnightStates.Block:
                        break;
                    case KnightStates.Dead:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

    }
}