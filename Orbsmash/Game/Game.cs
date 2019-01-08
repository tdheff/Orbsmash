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
using Handy.Components;
using Orbsmash.Constants;
using Orbsmash.Game.Interactions;

namespace Orbsmash.Game
{
    /// <summary>
    /// A scene representing an actual instance of the game, once play has been selected in the menu
    /// </summary>
    public class Game : Scene
    {
        private readonly GameSettings _settings;
        private AnimationSystem AnimationSystem;
        public Entity GameStateEntity;
        
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
                new TimerSystem(),
                new PlayerInputSystem(),
                new PlayerStateMachineSystem(),
                new GameStateMachineSystem(),
                new BallHitSystem(),
                new PlayerMovementSystem(),
                new KnockoutSystem(),
                new KinematicSystem(),
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
            String[] tiledMapLayers = new[]
            {
                TiledImportCollisionLayers.WALLS, TiledImportCollisionLayers.NET
            };
            int[] tiledMapPhysicsLayers = new[]
            {
                PhysicsLayers.WALLS, PhysicsLayers.NET
            };
            var tiledMapComponent = new Handy.Components.TiledMapComponent(tiledMap, tiledMapLayers, tiledMapPhysicsLayers, true);
            var entity = new Entity();
            entity.name = "Map";
            entity.addComponent(tiledMapComponent);
            addEntity(entity);
            
            var gameState = new GameState();
            gameState.StateEnum = GameStates.Ready;
            gameState.Players = new Player.Player[_settings.NumPlayers];
            var texture = content.Load<Texture2D>("Sprites/Characters/Knight/Knight");
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var playerSettings = _settings.Players[i];
                var player = new Player.Player(playerSettings);
                addEntity(player);
                gameState.Players[i] = player;
            }
            
            var ball = new Ball.Ball(_settings.BallSprite);
            addEntity(ball);
            gameState.Ball = ball;
            
            GameStateEntity = new Entity();
            GameStateEntity.addComponent(new GameStateComponent(gameState));
            GameStateEntity.addComponent(new TimerComponent());
            addEntity(GameStateEntity);
        }

        public override void onStart()
        {
            findEntity("Map").transform.position = new Vector2(0, 0);
            findEntity(EntityNames.BALL).transform.position = new Vector2(400, 400);
        }
    }
}