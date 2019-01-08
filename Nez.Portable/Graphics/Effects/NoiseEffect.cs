using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class NoiseEffect : Effect
    {
        private float _noise = 1f;
        private readonly EffectParameter _noiseParam;


        public NoiseEffect() : base(Core.graphicsDevice, EffectResource.noiseBytes)
        {
            _noiseParam = Parameters["noise"];
            _noiseParam.SetValue(_noise);
        }

        /// <summary>
        ///     Intensity of the noise. Defaults to 1.
        /// </summary>
        public float noise
        {
            get => _noise;
            set
            {
                if (_noise != value)
                {
                    _noise = value;
                    _noiseParam.SetValue(_noise);
                }
            }
        }
    }
}