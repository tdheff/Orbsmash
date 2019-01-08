using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nez.Textures
{
    public class NinePatchSubtexture : Subtexture
    {
        public int bottom;

        /// <summary>
        ///     used to indicate if this nine patch has additional padding information
        /// </summary>
        public bool hasPadding;

        public int left;
        public Rectangle[] ninePatchRects = new Rectangle[9];
        public int padBottom;
        public int padLeft;
        public int padRight;
        public int padTop;
        public int right;
        public int top;


        public NinePatchSubtexture(Texture2D texture, Rectangle sourceRect, int left, int right, int top, int bottom) :
            base(texture, sourceRect)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;

            generateNinePatchRects(sourceRect, ninePatchRects, left, right, top, bottom);
        }


        public NinePatchSubtexture(Texture2D texture, int left, int right, int top, int bottom) : this(texture,
            texture.Bounds, left, right, top, bottom)
        {
        }


        public NinePatchSubtexture(Subtexture subtexture, int left, int right, int top, int bottom) : this(subtexture,
            subtexture.sourceRect, left, right, top, bottom)
        {
        }
    }
}