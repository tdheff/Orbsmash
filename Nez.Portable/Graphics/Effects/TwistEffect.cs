﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class TwistEffect : Effect
    {
        private float _angle = 5f;
        private readonly EffectParameter _angleParam;
        private Vector2 _offset = Vector2Ext.halfVector();
        private readonly EffectParameter _offsetParam;

        private float _radius = 0.5f;

        private readonly EffectParameter _radiusParam;


        public TwistEffect() : base(Core.graphicsDevice, EffectResource.twistBytes)
        {
            _radiusParam = Parameters["radius"];
            _angleParam = Parameters["angle"];
            _offsetParam = Parameters["offset"];

            _radiusParam.SetValue(_radius);
            _angleParam.SetValue(_angle);
            _offsetParam.SetValue(_offset);
        }

        public float radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    _radiusParam.SetValue(_radius);
                }
            }
        }


        public float angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    _angleParam.SetValue(_angle);
                }
            }
        }

        public Vector2 offset
        {
            get => _offset;
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    _offsetParam.SetValue(_offset);
                }
            }
        }
    }
}