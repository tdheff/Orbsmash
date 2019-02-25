using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Handy.Sound
{
    // if we need we can add dynamic instancing of audio files at this layer
    public class HandySoundEffect
    {
        public SoundEffect Effect;
        private SoundEffectInstance Instance;

        public HandySoundEffect(SoundEffect effect)
        {
            Effect = effect;
            Instance = Effect.CreateInstance();
        }

        public void Play(float pitchAdustment = 0)
        {
            Instance.Play();
            Instance.Pitch = pitchAdustment;
        }

        public void Stop()
        {
            Instance.Stop();
        }

        public SoundState GetState()
        {
            return Instance.State;
        }

        public void SetVolume(float vol)
        {
            Instance.Volume = vol;
        }




    }
}
