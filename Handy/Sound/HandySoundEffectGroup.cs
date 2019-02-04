using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Handy.Sound
{
    public class HandySoundEffectGroup
    {
        private List<HandySoundEffect> Effects;
        private System.Random Rand;
        private HandySoundEffect CurrentEffect;
        private float MaxDuration = 100000f;
        private float MinDuration = .02f;
        private float Volume;
        private bool HardRetrigger;
        private int Index = 0;
        public float TimeLastPlayed = 0f;
        public HandySoundEffectGroup(List<HandySoundEffect> effects, float maxDuration, float volume, bool hardRetrigger = false)
        {
            Effects = effects;
            Effects.shuffle();
            SetGroupVolume(volume);
            SetMaxDuration(maxDuration);
            HardRetrigger = hardRetrigger;
        }
        public HandySoundEffectGroup(List<HandySoundEffect> effects)
        {
            Effects = effects;
            Effects.shuffle();
        }

        public void SetMaxDuration(float max)
        {
            MaxDuration = max;
        }

        public void SetGroupVolume(float volume)
        {
            Volume = volume;
            Effects.ForEach(e => e.SetVolume(Volume));
        }

        private void PickNextEffect()
        {
            Index++;
            if(Index >= Effects.Count)
            {
                Index = 0;
            }
            CurrentEffect = Effects[Index];
            CurrentEffect.Stop();
            TimeLastPlayed = Time.time;
        }

        public void Play()
        {
            var timeDiff = Time.time - TimeLastPlayed;
            // there has to be some limit to the madness
            if(HardRetrigger && timeDiff > MinDuration)
            {
                PickNextEffect();
            }
            else if (CurrentEffect == null || (CurrentEffect.GetState() != SoundState.Playing && timeDiff > MinDuration))
            {
                PickNextEffect();
            }
            else if (CurrentEffect != null && timeDiff > MaxDuration)
            {
                PickNextEffect();
            }
            CurrentEffect.Play();
        }
    }
}
