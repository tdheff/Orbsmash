using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Orbsmash.Player;
using Scene = Handy.Scene;
using Handy.Animation;
using System;
using System.Collections.Generic;

namespace Orbsmash.Game
{
    /// <summary>
    /// A scene representing an actual instance of the game, once play has been selected in the menu
    /// </summary>
    public class Game : Scene
    {
        private readonly GameSettings _settings;
        private AnimationSystem<Constants.EAnimations> AnimationSystem;
        
        public Game(GameSettings settings)
        {
            Console.WriteLine("###### GAME START ######");
            _settings = settings;
            LoadAnimations();
            CreateEntities();

        }
        
        protected void SetupRendering()
        {
            var gameRenderer = new RenderLayerRenderer(1, new int[] { 0 });
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            var uiRenderer = new ScreenSpaceRenderer(1, new [] { 1 });
            addRenderer(gameRenderer);
        }

        protected override EntitySystem[] Systems()
        {
            AnimationSystem = new AnimationSystem<Constants.EAnimations>();
            return new EntitySystem[]
            {
                new PlayerInputSystem(),
                new PlayerMovementSystem(),
                new KinematicSystem(),
                AnimationSystem
            };
        }

        private void LoadAnimations()
        {
            var animationDefs = Util.LoadAnimationDefinitions(@"Animation\AnimationDefinitions");
            SpriteDefinitions = Util.LoadSprites(animationDefs, content);
            // this feels a bit hacky but easy enough to clean up at some point... essentialy the animation system needs to
            // know about the "animation tracks/definitions" that are the json.. but the json is also currently defining like... vframes and hframes
            // so maybe this is separated out but maybe not... anyways it'll do for now, can probably just redesign better json format now that I know more this was like
            // the first thing I wrote
            AnimationSystem.SetAnimationDefinitions(animationDefs);
        }
        
        private void CreateEntities()
        {
            var texture = content.Load<Texture2D>(Nez.Content.player0);
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var player = new Player.Player(i, texture, i % 2 == 0 ? Constants.Side.Left : Constants.Side.Right);
                addEntity(player);
            }
            
            var ball = new Ball.Ball(texture);
            addEntity(ball);
        }
    }
}