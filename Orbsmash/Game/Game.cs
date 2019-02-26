using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using PubSub;
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
    /// <summary>
    /// A scene representing an actual instance of the competitive game, once players have selected their characters and sides of the arena
    /// </summary>
    public class Game : ArenaScene
    {
        private readonly GameSettings _settings;
        public Entity GameStateEntity;
        private Song song;
        
        public Game(GameSettings settings) : base()
        {
            Console.WriteLine("###### Competitive Game Start ######");
            _settings = settings;
            SetupAudio();
            CreateEntities();
        }

        protected override EntitySystem[] Systems()
        {
            return new EntitySystem[]
            {
                new TimerSystem(),
                new CameraShakeSystem(),
                new ParticleEmitterSystem(),
                new PlayerInputSystem(),
                new GameStateMachineSystem(),
                new BallHitSystem(),
                new KnockoutSystem(),
                new KinematicSystem(), 
                //SCOREBOARD
                new ScoreboardSystem(),
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
                // SPACEMAN
                new SpacemanMovementSystem(),
                new SpacemanStateMachineSystem(),
                new SpacemanAnimationSystem(),
                new SpacemanShieldSpawnSystem(),
                new SpacemanShieldSystem(),
                // EFFECTS
                new HitEffectSystem(),
                new AnimationSystem(),
                new AimIndicatorSystem(),
                new HitStopSystem(),
                // RENDERING
                new SpriteDepthSystem()
            };
        }

        // plan to add complexity here in terms of picking the correct song, etc once I have a better
        // system in place for starting this scene from  amenu scene
        private void SetupAudio()
        {
            song = content.Load<Song>(SoundEffects.MENU_MUSIC);
            MediaPlayer.Play(song);
            SetMusicVolume();
            MediaPlayer.IsRepeating = true;
            SetVolumeLevels(_settings.SfxVolume);
        }

        private void RefreshAfterSettingsChange()
        {
            SetMusicVolume();
            SetVolumeLevels(_settings.SfxVolume);
        }

        private void SetMusicVolume()
        {
            MediaPlayer.Volume = _settings.MusicVolume;
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
            var scoreboard = new UICanvas();
            scoreboard.name = "scoreboard";
            scoreboard.renderLayer = RenderLayers.FOREGROUND;
            GameStateEntity.addComponent(scoreboard);
            addEntity(GameStateEntity);
            
            // add camera
            var cam = new Camera();
            var cameraShake = new CameraShakeComponent();
            CameraEntity.addComponent(cam);
            CameraEntity.addComponent(cameraShake);
            CameraEntity.name = "Camera";
            addEntity(CameraEntity);
        }

        public override void onStart()
        {
            findEntity("Map").transform.position = new Vector2(-232, -40);
            findEntity(EntityNames.BALL).transform.position = new Vector2(400, 400);
            camera = CameraEntity.getComponent<Camera>();
        }
    }
}