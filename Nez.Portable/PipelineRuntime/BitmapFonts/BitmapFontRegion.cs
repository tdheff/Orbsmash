using Nez.Textures;

namespace Nez.BitmapFonts
{
    public class BitmapFontRegion
    {
        public char character;
        public Subtexture subtexture;
        public int xAdvance;
        public int xOffset;
        public int yOffset;


        public BitmapFontRegion(Subtexture textureRegion, char character, int xOffset, int yOffset, int xAdvance)
        {
            subtexture = textureRegion;
            this.character = character;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.xAdvance = xAdvance;
        }

        public int width => subtexture.sourceRect.Width;

        public int height => subtexture.sourceRect.Height;
    }
}