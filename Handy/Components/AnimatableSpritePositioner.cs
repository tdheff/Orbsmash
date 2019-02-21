using Nez.Textures;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handy.Animation
{
    public class AnimatableComponentPositioner : Component, IAnimatable
    {
        public Component Target;
        public AnimatableComponentPositioner(Component target)
        {
            Target = target;
        }
        public void SetFrame(int frame)
        {
        }
    }
}
