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

        protected override void SetupRenderer()
        {
            var gameRenderer = new RenderLayerRenderer(-1, new[] { 1, 2, 3 });
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            addRenderer(gameRenderer);
        }

        protected override EntitySystem[] Systems()
        {
            AnimationSystem = new AnimationSystem();
            return new EntitySystem[]
            {
                new TimerSystem(),
                new ParticleEmitterSystem(),
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
            LoadTextures(new []
            {
                BallSprites.DEFAULT,
                PlayerSprites.KNIGHT
            });
            LoadAnimationDefinitions(new []
            {
                PlayerAsepriteFiles.KNIGHT
            });
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
            tiledMapComponent.renderLayer = RenderLayers.BACKGROUND;
            entity.addComponent(tiledMapComponent);
            addEntity(entity);
            
            var gameState = new GameState();
            gameState.StateEnum = GameStates.Ready;
            gameState.Players = new Player.Player[_settings.NumPlayers];
            for (var i = 0; i < _settings.NumPlayers; i++)
            {
                var playerSettings = _settings.Players[i];
                var player = new Player.Player(playerSettings);
                addEntity(player);
                gameState.Players[i] = player;
            }
            
            var ball = new Ball.Ball();
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