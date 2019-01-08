using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Nez.BitmapFontImporter
{
    public class BitmapFontProcessorResult
    {
        public BitmapFontFile fontFile;
        public bool packTexturesIntoXnb;
        public List<string> textureNames = new List<string>();
        public List<Vector2> textureOrigins = new List<Vector2>();
        public List<Texture2DContent> textures = new List<Texture2DContent>();


        public BitmapFontProcessorResult(BitmapFontFile fontFile)
        {
            this.fontFile = fontFile;
        }
    }
}