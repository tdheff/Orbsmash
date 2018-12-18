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
    public class AnimationComponent<TEnum> : Component
    {
        public IAnimatable AnimationTarget;
        public string AnimationTrackIdentifier = "";
        public TEnum CurrentAnimation;
        public float ElapsedTime;
        public AnimationComponent(IAnimatable animationTarget, string trackIdentifier, TEnum startingAnimation)
        {
            AnimationTarget = animationTarget;
            AnimationTrackIdentifier = trackIdentifier;
            CurrentAnimation = startingAnimation;
            ElapsedTime = 0;
        }
    }
}
