﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class SepiaEffect : Effect
    {
        private Vector3 _sepiaTone = new Vector3(1.2f, 1.0f, 0.8f);
        private readonly EffectParameter _sepiaToneParam;


        public SepiaEffect() : base(Core.graphicsDevice, EffectResource.sepiaBytes)
        {
            _sepiaToneParam = Parameters["_sepiaTone"];
            _sepiaToneParam.SetValue(_sepiaTone);
        }

        /// <summary>
        ///     multiplied by the grayscale value for the final output. Defaults to 1.2f, 1.0f, 0.8f
        /// </summary>
        /// <value>The sepia tone.</value>
        public Vector3 sepiaTone
        {
            get => _sepiaTone;
            set
            {
                _sepiaTone = value;
                _sepiaToneParam.SetValue(_sepiaTone);
            }
        }
    }
}