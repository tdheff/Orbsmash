using System.Collections.Generic;
using Nez.Sprites;
using Nez.Textures;

namespace Handy.Animation
{
    public class AnimatableSprite : Sprite, IAnimatable
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
            if (frames != null && frames.Count > frame) setSubtexture(frames[frame]);
        }
    }
}