using Handy.Animation;
using Nez;

namespace Handy.Components
{
    // <summary>
    // Component for a body that uses simple kinematic physics
    // </summary>
    public class AnimationComponent : Component
    {
        public IAnimatable AnimationTarget;
        public string Context = "";
        public string CurrentAnimation;
        public int CurrentFrame;
        public float ElapsedTime;
        public string LastAnimation;

        public AnimationComponent(IAnimatable animationTarget, string context, string startingAnimation)
        {
            AnimationTarget = animationTarget;
            Context = context;
            CurrentAnimation = startingAnimation;
            ElapsedTime = 0;
        }

        public void SetAnimation(string newAnimation)
        {
            if (newAnimation != CurrentAnimation) ElapsedTime = 0;
            CurrentAnimation = newAnimation;
        }
    }
}