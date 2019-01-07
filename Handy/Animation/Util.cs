using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using Newtonsoft.Json;
using System.Reflection;
using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Handy.Animation
{
    public static class Util
    {
        public static Dictionary<string, HitboxDefinition> LoadHitboxes(List<string> paths, Nez.Systems.NezContentManager content)
        {
            var hitboxDict = new Dictionary<string, HitboxDefinition>();
            foreach(var path in paths)
            {
                var texture = content.Load<Texture2D>(path);
                var myDataArray = new Color[texture.Height * texture.Width];
                texture.GetData(myDataArray);
            }
            return hitboxDict;
        }

        public static Dictionary<string, SpriteDefinition> LoadSprites(IList<AnimationDefinition> animDefs, Nez.Systems.NezContentManager content)
        {
            var spriteTextureDict = new Dictionary<string, SpriteDefinition>();
            var spriteDescriptors = animDefs.Where(a => a.SpriteDescriptor != null).Select(a => a.SpriteDescriptor).Distinct();
            foreach (var spr in spriteDescriptors)
            {
                var texture = content.Load<Texture2D>(spr.SpriteName);
                spriteTextureDict.Add(spr.SpriteName, new SpriteDefinition(texture, spr.VFrames, spr.HFrames));
            }
            return spriteTextureDict;
        }
        public static List<Subtexture> ExtractSubtextures(Texture2D texture, int vFrames, int hFrames)
        {
            int xs = texture.Width / hFrames;
            int ys = texture.Height / vFrames;
            
            List<Subtexture> subtextures = new List<Subtexture>();
            for (var y = 0; y < vFrames; y++)
            {
                for (var x = 0; x < hFrames; x++)
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
    }

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

    public class HitboxDefinition
    {
        public string Name;
        public Vector2[] Points;
        public HitboxDefinition(string name, Vector2[] points)
        {
            Name = name;
            Points = points;
        }

    }
}