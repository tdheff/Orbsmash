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
    // Component for a body that uses simple kinematic physics
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
        public AnimationComponent(IAnimatable animationTarget, AnimationDefinition animationDefinition, string startingAnimation)
        {
            AnimationDefinition = animationDefinition;
            AnimationTarget = animationTarget;
            CurrentAnimation = startingAnimation;
            CurrentAnimationElapsedTime = 0;
        }

        public void SetAnimation(string newAnimation)
        {
            if (newAnimation == CurrentAnimation) return;
            CurrentAnimationElapsedTime = 0;
            CurrentFrameElapsedTime = 0;
            CurrentAnimationFrame = 0;
            CurrentAnimation = newAnimation;
        }

    }
}
