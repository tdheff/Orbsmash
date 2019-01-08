using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nez.Tiled
{
    public abstract class TiledLayer
    {
        public string name;
        public Vector2 offset;
        public float opacity;
        public Dictionary<string, string> properties;
        public bool visible = true;


        protected TiledLayer(string name)
        {
            this.name = name;
            properties = new Dictionary<string, string>();
        }


        public abstract void draw(Batcher batcher, Vector2 position, Vector2 scale, float layerDepth,
            RectangleF cameraClipBounds);
    }
}