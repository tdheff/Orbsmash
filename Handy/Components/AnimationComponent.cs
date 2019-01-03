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
        public string LastAnimation;
        public string Context = "";
        public string CurrentAnimation;
        public float ElapsedTime;
        public int CurrentFrame;
        public AnimationComponent(IAnimatable animationTarget, string context, string startingAnimation)
        {
            AnimationTarget = animationTarget;
            Context = context;
            CurrentAnimation = startingAnimation;
            ElapsedTime = 0;
        }

        public void SetAnimation(string newAnimation)
        {
            if(newAnimation != CurrentAnimation)
            {
            Console.WriteLine(newAnimation);
                ElapsedTime = 0;
            }
            CurrentAnimation = newAnimation;
        }

    }
}
