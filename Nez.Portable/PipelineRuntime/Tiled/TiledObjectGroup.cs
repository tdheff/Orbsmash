using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nez.Tiled
{
    public class TiledObjectGroup
    {
        public Color color;
        public string name;
        public TiledObject[] objects;
        public float opacity;
        public Dictionary<string, string> properties = new Dictionary<string, string>();
        public bool visible;


        public TiledObjectGroup(string name, Color color, bool visible, float opacity)
        {
            this.name = name;
            this.color = color;
            this.visible = visible;
            this.opacity = opacity;
        }


        /// <summary>
        ///     gets the first TiledObject with the given name
        /// </summary>
        /// <returns>The with name.</returns>
        /// <param name="name">Name.</param>
        public TiledObject objectWithName(string name)
        {
            for (var i = 0; i < objects.Length; i++)
                if (objects[i].name == name)
                    return objects[i];
            return null;
        }


        /// <summary>
        ///     gets all the TiledObjects with the given name
        /// </summary>
        /// <returns>The objects with matching names.</returns>
        /// <param name="name">Name.</param>
        public List<TiledObject> objectsWithName(string name)
        {
            var list = new List<TiledObject>();
            for (var i = 0; i < objects.Length; i++)
                if (objects[i].name == name)
                    list.Add(objects[i]);
            return list;
        }

        /// <summary>
        ///     gets all the TiledObjects with the given type
        /// </summary>
        /// <returns>The objects with matching types.</returns>
        /// <param name="type">Type.</param>
        public List<TiledObject> objectsWithType(string type)
        {
            var list = new List<TiledObject>();
            for (var i = 0; i < objects.Length; i++)
                if (objects[i].type == type)
                    list.Add(objects[i]);
            return list;
        }
    }
}