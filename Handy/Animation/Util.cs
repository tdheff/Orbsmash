using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace Handy.Animation
{
    public static class Util
    {
        public static List<Subtexture> ExtractSubtextures(Texture2D texture, int hFrames, int vFrames)
        {
            int xs = texture.Width / hFrames;
            int ys = texture.Height / vFrames;
            
            List<Subtexture> subtextures = new List<Subtexture>();
            for (var x = 0; x < hFrames; x++)
            {
                for (var y = 0; y < vFrames; y++)
                {
                    subtextures.Add(new Subtexture(texture, xs * x, ys * y, xs, ys));
                }
            }

            return subtextures;
        }
        
    }
}