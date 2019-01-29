using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handy.Animation
{
    public class AnimatableSprite :  Nez.Sprites.Sprite, IAnimatable
    {
        public List<Subtexture> frames;
        public AnimatableSprite(List<Subtexture> inputFrames)
        {
            frames = inputFrames;
            _subtexture = frames[0];
            _origin = frames[0].center;
        }
        public void SetFrame(int frame)
        {
            if(frames != null && frames.Count > frame)
            {
                setSubtexture(frames[frame]);
            }
        }
    }
}
