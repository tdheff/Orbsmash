using Handy.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Tiled;
using Orbsmash.Player;
using Scene = Handy.Scene;
using Handy.Animation;
using System;
using System.Collections.Generic;
using Handy.Components;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Game.Effects;
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
        private Song song;
        public Entity CameraEntity = new Entity();
        
        public Game(GameSettings settings) : base(5)
        {
            Console.WriteLine("###### GAME START ######");
            setDesignResolution(1920, 1080, SceneResolutionPolicy.BestFit);
            _settings = settings;
            LoadContent();
            SetupAudio();
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
                // SPACEMAN
                new SpacemanMovementSystem(),
                new SpacemanStateMachineSystem(),
                new SpacemanAnimationSystem(),
                // EFFECTS
                new HitEffectSystem(),
                AnimationSystem
            };
        }

        private void LoadContent()
        {
            LoadTextures(new []
            {
                BallSprites.DEFAULT,
                PlayerSprites.KNIGHT,
                PlayerSprites.WIZARD,
                PlayerSprites.SPACEMAN,
                Sprites.HitEffect
            });
            LoadAnimationDefinitions(new []
            {
                AsepriteFiles.KNIGHT,
                AsepriteFiles.WIZARD,
                AsepriteFiles.SPACEMAN,
                AsepriteFiles.HIT_EFFECT
            });

            var soundsToLoad = new List<string>()
            {
                SoundEffects.FOOTSTEPS_1,
            };
            soundsToLoad.AddRange(KnightSoundEffects.AllEffects);
            LoadSounds(soundsToLoad.ToArray());
        }


        // plan to add complexity here in terms of picking the correct song, etc once I have a better
        // system in place for starting this scene from  amenu scene
        private void SetupAudio()
        {
            this.song = content.Load<Song>(Constants.SoundEffects.MENU_MUSIC);
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
            findEntity("Map").transform.position = new Vector2(-80, -40);
            findEntity(EntityNames.BALL).transform.position = new Vector2(400, 400);
            camera = CameraEntity.getComponent<Camera>();
        }
    }
}