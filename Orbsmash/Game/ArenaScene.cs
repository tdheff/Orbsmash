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
using System.Linq;
using Handy.Components;
using Orbsmash.Ball;
using Orbsmash.Constants;
using Orbsmash.Game.Effects;
using Orbsmash.Game.Interactions;
namespace Orbsmash.Game
{
    public abstract class ArenaScene : Scene
    {
        public Entity CameraEntity = new Entity();

        public ArenaScene() : base(5)
        {
            Input.maxSupportedGamePads = 4; // jesus
            setDesignResolution(1920, 1080, SceneResolutionPolicy.BestFit);
            LoadContent();
        }

        protected override void SetupRenderer()
        {
            var gameRenderer = new RenderLayerRenderer(-1, Enumerable.Range(1, 10000).ToArray());
            clearColor = new Color(0.1f, 0.1f, 0.1f);
            addRenderer(gameRenderer);
        }


        // any scene in the arena format should load all the characters
        // it's just not that much data so no harm in being simple
        private void LoadContent()
        {
            LoadTextures(new[]
            {
                BallSprites.DEFAULT,
                PlayerSprites.KNIGHT,
                PlayerSprites.WIZARD,
                PlayerSprites.SPACEMAN,
                Sprites.HIT_EFFECT,
                Sprites.AIM_INDICATOR,
                Sprites.CHARACTER_CIRCLE,
                Sprites.LEFT_COOLDOWN,
                Sprites.RIGHT_COOLDOWN,
                Sprites.SPACEMAN_SHIELD
            });
            LoadAnimationDefinitions(new[]
            {
                AsepriteFiles.KNIGHT,
                AsepriteFiles.WIZARD,
                AsepriteFiles.SPACEMAN,
                AsepriteFiles.HIT_EFFECT,
                AsepriteFiles.SPACEMAN_SHIELD
            });

            var soundsToLoad = new List<string>(KnightSoundEffects.AllEffects);
            soundsToLoad.AddRange(WizardSoundEffects.AllEffects);
            LoadSounds(soundsToLoad.ToArray());
        }
    }
}
