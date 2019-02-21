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
        public Entity GameStateEntity;
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
                new AnimatingPropertySystem(),
                new HitStopSystem(),
                new TimerSystem(),
                new CameraShakeSystem(),
                new ParticleEmitterSystem(),
                new PlayerInputSystem(),
                new PracticeGameStateMachineSystem(),
                new CharacterChoiceStateMachineSystem(),
                new BallHitSystem(),
                new KnockoutSystem(),
                new KinematicSystem(),
                //// BALL
                new BallStateSystem(),
                //// KNIGHT
                new KnightMovementSystem(),
                new KnightStateMachineSystem(),
                new KnightAnimationSystem(),
                //// WIZARD
                new WizardMovementSystem(),
                new WizardStateMachineSystem(),
                new WizardAnimationSystem(),
                // SPACEMAN
                new SpacemanMovementSystem(),
                new SpacemanStateMachineSystem(),
                new SpacemanAnimationSystem(),
                //// EFFECTS
                new HitEffectSystem(),
                new AnimationSystem()
            };
        }

        private void CreateEntities()
        {
            var gameState = new PracticeGameState();
            gameState.StateEnum = PracticeGameStates.WaitingForPlayersToJoin;
            // to start we don't have any choosers or players!
            GameStateEntity = new Entity();
            GameStateEntity.name = "PracticeGame";
            GameStateEntity.addComponent(new PracticeGameStateComponent(gameState));
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
