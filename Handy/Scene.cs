using Handy.Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Handy
{
    public abstract class Scene : Nez.Scene
    {
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, AnimationDefinition> AnimationDefinitions = new Dictionary<string, AnimationDefinition>();
        private Vector2 _scale;
        protected Scene() : base()
        {
            _scale = Vector2.One;
            _initialize();
        }
        
        /// <summary>
        /// Constructs a scene with a scale factor
        /// </summary>
        /// <param name="scale">All entities added to the scene will be scaled by this amount</param>
        protected Scene(int scale) : base()
        {
            _scale = new Vector2(scale);
            _initialize();
        }

        public new Entity addEntity(Entity entity)
        {
            entity.scale *= _scale;
            return base.addEntity(entity);
        } 
        
        private void _initialize()
        {
            SetupRenderer();
            InstallSystems(Systems());
        }

        protected abstract EntitySystem[] Systems();
        
        protected void InstallSystems(params EntitySystem[] systems)
        {
            foreach (var system in systems)
            {
                addEntityProcessor(system);
            }
        }
        
        protected virtual void SetupRenderer()
        {
            var myRenderer = new RenderLayerRenderer(1, new int[] { 0 });
            clearColor = new Color(0.3f, 0.3f, 0.3f);
            addRenderer(myRenderer);
        }

        public void LoadTextures(string[] texturePaths)
        {
            for (int i = 0; i < texturePaths.Length; i++)
            {
                Textures.Add(texturePaths[i], content.Load<Texture2D>(texturePaths[i]));
            }
        }

        public void LoadAnimationDefinitions(string[] jsonPaths)
        {
            for (int i = 0; i < jsonPaths.Length; i++)
            {
                var asepriteJson = content.Load<AsepriteJson>(jsonPaths[i]);
                var spriteFileName = Path.GetFileNameWithoutExtension(asepriteJson.Meta.Image);
                var splitPath = jsonPaths[i].Split('/');
                splitPath[splitPath.Length - 1] = spriteFileName;
                var texturePath = string.Join("/", splitPath);
                var texture = Textures[texturePath];
                var animationDef = AnimationDefinition.FromAsepriteJson(asepriteJson, texture);
                AnimationDefinitions.Add(jsonPaths[i], animationDef);
            }
        }

    }
}