using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Nez;

namespace Handy.Components
{
    public class VelocityComponent : Component
    {
        public Vector2 Velocity
        {
            get => Freeze ? Vector2.Zero : _velocity;
            set => _velocity = value;
        }
        public bool Freeze;

        private Vector2 _velocity = Vector2.Zero;

        public VelocityComponent() { }

        public VelocityComponent(Vector2 initialVelocity)
        {
            Velocity = initialVelocity;
        }
    }
}