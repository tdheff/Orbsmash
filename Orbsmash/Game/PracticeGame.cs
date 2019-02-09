using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Tiled;
using Orbsmash.Player;
using System;
using Handy.Components;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Game.Effects;
using Orbsmash.Game.Interactions;

namespace Orbsmash.Game
{
    public class PracticeGame : ArenaScene
    {
        private Song song;
        public PracticeGame(): base()
        {
            Console.WriteLine("##### Practice Game Start ######");
            SetupAudio();
            CreateEntities();
        }

        private void SetupAudio()
        {
            // not sure yet
        }

        protected override EntitySystem[] Systems()
        {
            return new EntitySystem[]
            {
                new HitStopSystem(),
                new TimerSystem(),
                new CameraShakeSystem(),
                new ParticleEmitterSystem(),
                new PlayerInputSystem(),
                new GameStateMachineSystem(),
                new BallHitSystem(),
                new KnockoutSystem(),
                new KinematicSystem(),
                new KinematicSystem(),
                // BALL
                new BallStateSystem(),
                // KNIGHT
                new KnightMovementSystem(),
                new KnightStateMachineSystem(),
                new KnightAnimationSystem(),
                // WIZARD
                new WizardMovementSystem(),
                new WizardStateMachineSystem(),
                new WizardAnimationSystem(),
                // EFFECTS
                new HitEffectSystem(),
                new AnimationSystem()
            };
        }

        private void CreateEntities()
        {
            var tiledMap = content.Load<TiledMap>(_settings.MapTile);
            String[] tiledMapLayers = new[]
            {
                TiledImportCollisionLayers.BACK_WALLS, TiledImportCollisionLayers.SIDE_WALLS, TiledImportCollisionLayers.NET
            };
            int[] tiledMapPhysicsLayers = new[]
            {
                PhysicsLayers.BACK_WALLS, PhysicsLayers.SIDE_WALLS, PhysicsLayers.NET
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
            GameStateEntity.name = "Game";
            GameStateEntity.addComponent(new GameStateComponent(gameState));
            GameStateEntity.addComponent(new TimerComponent());
            GameStateEntity.addComponent(new HitStopComponent());
            addEntity(GameStateEntity);

            // add camera
            var cam = new Camera();
            var cameraShake = new CameraShakeComponent();
            CameraEntity.addComponent(cam);
            CameraEntity.addComponent(cameraShake);
            CameraEntity.name = "Camera";
            addEntity(CameraEntity);
        }

    }
}
