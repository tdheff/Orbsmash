using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using Newtonsoft.Json;
using System.Reflection;
using System;

namespace Handy.Animation
{
    public class SpriteDefinition
    {
        public SpriteDefinition(Texture2D texture, int vFrames, int hFrames)
        {
            Texture = texture;
            VFrames = vFrames;
            HFrames = hFrames;
            Subtextures = Util.ExtractSubtextures(texture, vFrames, hFrames);
        }
        public Texture2D Texture;
        public List<Subtexture> Subtextures;
        public int VFrames;
        public int HFrames;
    }

    public static class Util
    {
        public static Dictionary<string, SpriteDefinition> LoadSprites(IList<AnimationDefinition> animDefs, Nez.Systems.NezContentManager content)
        {
            var spriteTextureDict = new Dictionary<String, SpriteDefinition>();
            foreach (var def in animDefs)
            {
                var texture = content.Load<Texture2D>(def.SpriteName);
                spriteTextureDict.Add(def.SpriteName, new SpriteDefinition(texture, def.VFrames, def.HFrames));
            }
            return spriteTextureDict;
        }
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

        public static IList<AnimationDefinition> LoadAnimationDefinitions(string contentFolderPath)
        {
            string path = Path.Combine(Environment.CurrentDirectory, contentFolderPath);
            string[] filePaths = Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly);
            List<AnimationDefinition> animationDefs = new List<AnimationDefinition>();
            foreach(var filepath in filePaths)
            {
                string text = System.IO.File.ReadAllText(filepath);
                var animationDef = JsonConvert.DeserializeObject<AnimationDefinition>(text);
                animationDefs.Add(animationDef);
            }
            return animationDefs;
        }
        

        private Dictionary<String, SpriteDefinition> SpriteTextureDict;

    }
}