using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class HeatDistortionPostProcessor : PostProcessor
    {
        private float _distortionFactor = 0.005f;
        private EffectParameter _distortionFactorParam;
        private float _riseFactor = 0.15f;
        private EffectParameter _riseFactorParam;
        private EffectParameter _timeParam;


        public HeatDistortionPostProcessor(int executionOrder) : base(executionOrder)
        {
        }

        public float distortionFactor
        {
            get => _distortionFactor;
            set
            {
                if (_distortionFactor != value)
                {
                    _distortionFactor = value;

                    if (effect != null)
                        _distortionFactorParam.SetValue(_distortionFactor);
                }
            }
        }

        public float riseFactor
        {
            get => _riseFactor;
            set
            {
                if (_riseFactor != value)
                {
                    _riseFactor = value;

                    if (effect != null)
                        _riseFactorParam.SetValue(_riseFactor);
                }
            }
        }

        public Texture2D distortionTexture
        {
            set => effect.Parameters["_distortionTexture"].SetValue(value);
        }


        public override void onAddedToScene()
        {
            effect = scene.content.loadEffect<Effect>("heatDistortion", EffectResource.heatDistortionBytes);

            _timeParam = effect.Parameters["_time"];
            _distortionFactorParam = effect.Parameters["_distortionFactor"];
            _riseFactorParam = effect.Parameters["_riseFactor"];

            _distortionFactorParam.SetValue(_distortionFactor);
            _riseFactorParam.SetValue(_riseFactor);

            distortionTexture = scene.content.Load<Texture2D>("nez/textures/heatDistortionNoise");
        }


        public override void process(RenderTarget2D source, RenderTarget2D destination)
        {
            _timeParam.SetValue(Time.time);
            base.process(source, destination);
        }
    }
}