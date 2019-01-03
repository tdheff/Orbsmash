using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using Orbsmash.Player;
using Scene = Handy.Scene;
using Handy.Animation;
using System;
using System.Collections.Generic;
using Orbsmash.Constants;

namespace Orbsmash.Game
{
    /// <summary>
    /// A scene representing an actual instance of the game, once play has been selected in the menu
    /// </summary>
    public class Game : Scene
    {
        private readonly GameSettings _settings;
        private AnimationSystem AnimationSystem;
        
        public Game(GameSettings settings) : base(5)
        {
            Console.WriteLine("###### GAME START ######");
            setDesignResolution(2560, 1440, SceneResolutionPolicy.BestFit);
            _settings = settings;
            LoadContent();
            CreateEntities();

        }
        
        protected void SetupRendering()
        {
            //var gameRenderer = new RenderLayerRenderer(1, new int[] { 0, 1, 2, 3 });
            var gameRenderer = new DefaultRenderer();
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            addRenderer(gameRenderer);
        }

        protected override EntitySystem[] Systems()
        {
            AnimationSystem = new AnimationSystem();
            return new EntitySystem[]
            {
                new PlayerInputSystem(),
                new PlayerStateMachineSystem(),
                new PlayerMovementSystem(),
                new KinematicSystem(),
                new PlayerAnimationSystem(),
                AnimationSystem
            };
        }

        private void LoadContent()
        {
            var animationDefs = Util.LoadAnimationDefinitions(@"Animation/AnimationDefinitions");
            SpriteDefinitions = Util.LoadSprites(animationDefs, content);
            AnimationSystem.SetAnimationDefinitions(animationDefs);
            HitboxDefinitions = Util.LoadHitboxes(Hitboxes.HitboxesToLoad, content);
        }
        
        private void CreateEntities()
        {
            var tiledMap = content.Load<TiledMap>(_settings.MapTile);
            var tiledMapComponent = new TiledMapComponent(tiledMap, CollisionLayers.COLLIDERS, true);
            var entity = new Entity();
            entity.name = "Map";
            entity.addComponent(tiledMapComponent);
            addEntity(entity);
            
            var texture = content.Load<Texture2D>("Sprites/Characters/Knight/Knight");
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var playerSettings = _settings.Players[i];
                var player = new Player.Player(playerSettings);
                addEntity(player);
            }
            
            var ball = new Ball.Ball(_settings.BallSprite);
            addEntity(ball);
        }

        public override void onStart()
        {
            findEntity("Map").transform.position = new Vector2(-160, 0);
            findEntity("Player_0").transform.position = new Vector2(200, 200);
            findEntity("Ball").transform.position = new Vector2(400, 400);
        }
    }
}