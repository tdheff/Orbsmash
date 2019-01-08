using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class VignettePostProcessor : PostProcessor
    {
        private float _power = 1f;
        private EffectParameter _powerParam;
        private float _radius = 1.25f;
        private EffectParameter _radiusParam;


        public VignettePostProcessor(int executionOrder) : base(executionOrder)
        {
        }

        public float power
        {
            get => _power;
            set
            {
                if (_power != value)
                {
                    _power = value;

                    if (effect != null)
                        _powerParam.SetValue(_power);
                }
            }
        }

        public float radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;

                    if (effect != null)
                        _radiusParam.SetValue(_radius);
                }
            }
        }


        public override void onAddedToScene()
        {
            effect = scene.content.loadEffect<Effect>("vignette", EffectResource.vignetteBytes);

            _powerParam = effect.Parameters["_power"];
            _radiusParam = effect.Parameters["_radius"];
            _powerParam.SetValue(_power);
            _radiusParam.SetValue(_radius);
        }
    }
}