﻿using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tweens;

namespace Nez
{
	/// <summary>
	///     fades from the current Scene to the new Scene
	/// </summary>
	public class CrossFadeTransition : SceneTransition
    {
        private Color _color = Color.White;

        private Color _fromColor = Color.White;
        private Color _toColor = Color.Transparent;

        /// <summary>
        ///     duration for the fade
        /// </summary>
        public float fadeDuration = 1f;

        /// <summary>
        ///     ease equation to use for the cross fade
        /// </summary>
        public EaseType fadeEaseType = EaseType.QuartIn;


        public CrossFadeTransition(Func<Scene> sceneLoadAction) : base(sceneLoadAction, true)
        {
        }


        public CrossFadeTransition() : this(null)
        {
        }


        public override IEnumerator onBeginTransition()
        {
            yield return null;

            // load up the new Scene
            yield return Core.startCoroutine(loadNextScene());

            var elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                _color = Lerps.ease(fadeEaseType, ref _fromColor, ref _toColor, elapsed, fadeDuration);

                yield return null;
            }

            transitionComplete();
        }


        public override void render(Graphics graphics)
        {
            Core.graphicsDevice.setRenderTarget(null);
            graphics.batcher.begin(BlendState.NonPremultiplied, Core.defaultSamplerState, DepthStencilState.None, null);
            graphics.batcher.draw(previousSceneRender, Vector2.Zero, _color);
            graphics.batcher.end();
        }
    }
}