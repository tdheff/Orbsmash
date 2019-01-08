using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nez.Tiled
{
    public class TiledObject
    {
        public enum TiledObjectType
        {
            None,
            Ellipse,
            Image,
            Polygon,
            Polyline
        }

        public int height;

        public int id;
        public string name;
        public string objectType;
        public Vector2[] polyPoints;
        public Dictionary<string, string> properties = new Dictionary<string, string>();
        public int rotation;
        public TiledObjectType tiledObjectType;
        public string type;
        public bool visible;
        public int width;
        public int x;
        public int y;

        /// <summary>
        ///     wraps the x/y fields in a Vector
        /// </summary>
        public Vector2 position
        {
            get => new Vector2(x, y);
            set
            {
                x = (int) value.X;
                y = (int) value.Y;
            }
        }
    }
}