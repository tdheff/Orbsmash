﻿using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tweens;

namespace Nez
{
	/// <summary>
	///     builds up a cover of squares then removes them
	/// </summary>
	public class SquaresTransition : SceneTransition
    {
        private readonly Rectangle _destinationRect;
        private Texture2D _overlayTexture;

        private readonly Effect _squaresEffect;

        /// <summary>
        ///     delay before removing squares
        /// </summary>
        public float delayBeforeSquaresInDuration = 0;

        /// <summary>
        ///     ease equation to use for the animation
        /// </summary>
        public EaseType easeType = EaseType.QuartOut;

        /// <summary>
        ///     duration for squares to populate the screen
        /// </summary>
        public float squaresInDuration = 0.6f;

        /// <summary>
        ///     duration for squares to unpopulate screen
        /// </summary>
        public float squaresOutDuration = 0.6f;


        public SquaresTransition(Func<Scene> sceneLoadAction) : base(sceneLoadAction, true)
        {
            _destinationRect = previousSceneRender.Bounds;

            // load Effect and set defaults
            _squaresEffect = Core.content.loadEffect("Content/nez/effects/transitions/Squares.mgfxo");
            squareColor = Color.Black;
            smoothness = 0.5f;

            var aspectRatio = Screen.width / (float) Screen.height;
            size = new Vector2(30, 30 / aspectRatio);
        }


        public SquaresTransition() : this(null)
        {
        }

        /// <summary>
        ///     color of the squares
        /// </summary>
        /// <value>The color of the square.</value>
        public Color squareColor
        {
            set => _squaresEffect.Parameters["_color"].SetValue(value.ToVector4());
        }

        public float smoothness
        {
            set => _squaresEffect.Parameters["_smoothness"].SetValue(value);
        }

        /// <summary>
        ///     size of the squares. If you want perfect squares use size, size / aspectRatio_of_screen
        /// </summary>
        /// <value>The size.</value>
        public Vector2 size
        {
            set => _squaresEffect.Parameters["_size"].SetValue(value);
        }


        public override IEnumerator onBeginTransition()
        {
            // create a single pixel transparent texture so we can do our squares out to the next scene
            _overlayTexture = Graphics.createSingleColorTexture(1, 1, Color.Transparent);

            // populate squares
            yield return Core.startCoroutine(tickEffectProgressProperty(_squaresEffect, squaresInDuration, easeType));

            // load up the new Scene
            yield return Core.startCoroutine(loadNextScene());

            // dispose of our previousSceneRender. We dont need it anymore.
            previousSceneRender.Dispose();
            previousSceneRender = null;

            // delay
            yield return Coroutine.waitForSeconds(delayBeforeSquaresInDuration);

            // unpopulate squares
            yield return Core.startCoroutine(tickEffectProgressProperty(_squaresEffect, squaresInDuration,
                EaseHelper.oppositeEaseType(easeType), true));

            transitionComplete();

            // cleanup
            _overlayTexture.Dispose();
            Core.content.unloadEffect(_squaresEffect.Name);
        }


        public override void render(Graphics graphics)
        {
            Core.graphicsDevice.setRenderTarget(null);
            graphics.batcher.begin(BlendState.NonPremultiplied, Core.defaultSamplerState, DepthStencilState.None, null,
                _squaresEffect);

            // we only render the previousSceneRender while populating the squares
            if (!_isNewSceneLoaded)
                graphics.batcher.draw(previousSceneRender, _destinationRect, Color.White);
            else
                graphics.batcher.draw(_overlayTexture, new Rectangle(0, 0, Screen.width, Screen.height),
                    Color.Transparent);

            graphics.batcher.end();
        }
    }
}