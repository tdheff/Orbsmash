using Microsoft.Xna.Framework;

namespace Nez.DeferredLighting
{
	/// <summary>
	///     AreaLights work like DirLights except they only affect a specific area specified by the width/height. Note that
	///     Transform.scale
	///     will affect the size of an AreaLight.
	/// </summary>
	public class AreaLight : DeferredLight
    {
        private float _areaWidth, _areaHeight;

        /// <summary>
        ///     direction of the light
        /// </summary>
        public Vector3 direction = new Vector3(500, 500, 50);

        /// <summary>
        ///     brightness of the light
        /// </summary>
        public float intensity = 12f;


        public AreaLight(float width, float height)
        {
            setWidth(width).setHeight(height);
        }

        public override float width => _areaWidth;

        public override float height => _areaHeight;


        public AreaLight setWidth(float width)
        {
            _areaWidth = width;
            _areBoundsDirty = true;
            return this;
        }


        public AreaLight setHeight(float height)
        {
            _areaHeight = height;
            _areBoundsDirty = true;
            return this;
        }


        public AreaLight setDirection(Vector3 direction)
        {
            this.direction = direction;
            return this;
        }


        public AreaLight setIntensity(float intensity)
        {
            this.intensity = intensity;
            return this;
        }
    }
}