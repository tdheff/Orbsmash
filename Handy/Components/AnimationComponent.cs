using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handy.Animation;
using Microsoft.Xna.Framework;
using Nez;

namespace Handy.Components
{
    // <summary>
    // A hopefully generic component for managing animation
    // </summary>
    public class AnimationComponent : Component
    {
        public IAnimatable AnimationTarget;
        public AnimationDefinition AnimationDefinition;
        public string LastAnimation;
        public string CurrentAnimation;
        public float CurrentAnimationElapsedTime;
        public float CurrentFrameElapsedTime;
        public int CurrentAnimationFrame = 0;
        public float PlaybackSpeed = 1.0f;
        public bool Paused = false;
        public AnimationComponent(IAnimatable animationTarget, AnimationDefinition animationDefinition, string startingAnimation)
        {
            AnimationDefinition = animationDefinition;
            AnimationTarget = animationTarget;
            CurrentAnimation = startingAnimation;
            CurrentAnimationElapsedTime = 0;
        }

        public void SetAnimation(string newAnimation, float playbackSpeed = 1.0f)
        {
            if (newAnimation == CurrentAnimation) return;
            CurrentAnimationElapsedTime = 0;
            CurrentFrameElapsedTime = 0;
            CurrentAnimationFrame = 0;
            CurrentAnimation = newAnimation;
            PlaybackSpeed = playbackSpeed;
        }

    }
}
