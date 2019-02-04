using System;
using System.Linq;
using Handy.Components;
using Handy.Sound;
using Handy.Systems;
using System.Collections.Generic;
using Nez;
using Orbsmash.Constants;
using Microsoft.Xna.Framework;

namespace Orbsmash.Player
{
    public class KnightSoundSystem : EntitySystem
    {
        private bool IsSetup = false;
        private HandySoundEffectGroup Steps;
        private HandySoundEffectGroup Swipes;
        private HandySoundEffectGroup Hits;
        private float SfxVolume = 1f;
        public KnightSoundSystem(Matcher matcher) : base(matcher) { }
        public KnightSoundSystem() : base(new Matcher().all(typeof(PlayerInputComponent), typeof(KnightStateMachineComponent), typeof(PlayerStateComponent))) {}

        private void FirstTimeSetup()
        {
            var handyScene = (Handy.Scene)scene;
            var sounds = handyScene.Sounds;
            var stepEffects = new List<HandySoundEffect>()
            {
                sounds[KnightSoundEffects.BOOT_1],
                sounds[KnightSoundEffects.BOOT_2],
                sounds[KnightSoundEffects.BOOT_3],
                sounds[KnightSoundEffects.BOOT_4],
                sounds[KnightSoundEffects.BOOT_5],
                sounds[KnightSoundEffects.BOOT_6],
            };
            Steps = new HandySoundEffectGroup(stepEffects, 0.3f, 0.5f * handyScene.SfxVolume);
            var swipeEffects = new List<HandySoundEffect>()
            {
                sounds[KnightSoundEffects.SWIPE_1],
                sounds[KnightSoundEffects.SWIPE_2],
                sounds[KnightSoundEffects.SWIPE_3],
                sounds[KnightSoundEffects.SWIPE_4],
            };
            Swipes = new HandySoundEffectGroup(swipeEffects, 2f, 0.5f * handyScene.SfxVolume, true);
            var hitEffects = new List<HandySoundEffect>()
            {
                sounds[KnightSoundEffects.HIT_1],
            };
            Hits = new HandySoundEffectGroup(hitEffects, 2f, 0.5f * handyScene.SfxVolume, true);
            IsSetup = true;
        }

        protected override void process(List<Entity> entities)
        {
            if(!IsSetup)
            {
                FirstTimeSetup();
            }
            foreach (var entity in entities)
            {
                var knightState = entity.getComponent<KnightStateMachineComponent>().State;
                var soundState = entity.getComponent<KnightSoundStateComponent>();
                var playerState = entity.getComponent<PlayerStateComponent>();
                var player = (Player)entity;
                var handyScene = (Handy.Scene) scene;
                var eventComponent = player.getComponent<EventComponent>();

                // get all the sound effects we care about
                switch (knightState.StateEnum)
                {
                    case KnightStates.Idle:
                        break;
                    case KnightStates.Walk:
                        Steps.Play();
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
                // sound state flags
                if(soundState.PlaySwingWeapon1)
                {
                    Swipes.Play();
                }
                if (soundState.PlayBallHit)
                {
                    Hits.Play();
                }
                soundState.ClearFlags();
            }

        }

    }
}